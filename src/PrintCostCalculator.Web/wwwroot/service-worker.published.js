// The deployed service worker: makes the calculator work OFFLINE after the
// first visit, while still applying updates immediately on the next visit.
//
// How updates stay fresh:
//   - index.html (and other non-hashed files like CSS/JS) are fetched from
//     the network first, falling back to the cache only when offline.
//   - The /_framework/ files have content-hashed names (e.g. PrintCostCalculator.Web.assembly.bin),
//     so a new build gets new URLs and can never be stale. Those are served
//     from the cache first — instant offline startup.
//
// The build generates service-worker-assets.js (a list of all app files),
// which is loaded before this file and read here as self.assetsManifest.

const CACHE_NAME = 'print-cost-calculator-v2';

self.addEventListener('install', (event) => {
    event.waitUntil((async () => {
        const cache = await caches.open(CACHE_NAME);

        const manifest = self.assetsManifest || [];
        const filesToPrecache = manifest
            .map((manifestEntry) => manifestEntry && (manifestEntry.url || manifestEntry))
            .filter((fileUrl) => fileUrl && !fileUrl.endsWith('/'));

        await Promise.all(
            filesToPrecache.map((fileUrl) => cache.add(new Request(fileUrl, { cache: 'reload' })))
        );
    })());

    self.skipWaiting();
});

self.addEventListener('activate', (event) => {
    event.waitUntil((async () => {
        // Delete every cache from older versions of this service worker.
        const allCacheNames = await caches.keys();
        await Promise.all(
            allCacheNames
                .filter((cacheName) => cacheName !== CACHE_NAME)
                .map((cacheName) => caches.delete(cacheName))
        );

        await self.clients.claim();
    })());
});

self.addEventListener('fetch', (event) => {
    const request = event.request;

    if (request.method !== 'GET') {
        return;
    }

    const requestUrl = new URL(request.url);
    if (requestUrl.origin !== self.location.origin) {
        return;
    }

    // Opening the app = a navigation request. Always try the network first
    // so the user gets the newest version without needing a hard refresh.
    if (request.mode === 'navigate') {
        event.respondWith(networkFirstForIndexPage());
        return;
    }

    // Content-hashed files never change once built, so the cache is always valid.
    if (requestUrl.pathname.includes('/_framework/')) {
        event.respondWith(cacheFirst(request));
        return;
    }

    // Everything else (CSS, JS, icons) can change between builds,
    // so prefer the network and only fall back to the cache when offline.
    event.respondWith(networkFirst(request));
});

async function networkFirstForIndexPage() {
    const cache = await caches.open(CACHE_NAME);

    try {
        const freshIndexPage = await fetch(new Request('index.html', { cache: 'reload' }));
        await cache.put('index.html', freshIndexPage.clone());
        return freshIndexPage;
    } catch {
        const cachedIndexPage = await cache.match('index.html');
        if (cachedIndexPage) {
            return cachedIndexPage;
        }

        return new Response('Offline', { status: 503, statusText: 'Offline' });
    }
}

async function cacheFirst(request) {
    const cache = await caches.open(CACHE_NAME);

    const cachedResponse = await cache.match(request);
    if (cachedResponse) {
        return cachedResponse;
    }

    const freshResponse = await fetch(request);
    if (freshResponse && freshResponse.status === 200) {
        await cache.put(request, freshResponse.clone());
    }
    return freshResponse;
}

async function networkFirst(request) {
    const cache = await caches.open(CACHE_NAME);

    try {
        const freshResponse = await fetch(new Request(request, { cache: 'reload' }));
        if (freshResponse && freshResponse.status === 200) {
            await cache.put(request, freshResponse.clone());
        }
        return freshResponse;
    } catch {
        const cachedResponse = await cache.match(request);
        if (cachedResponse) {
            return cachedResponse;
        }

        return new Response('Offline', { status: 503, statusText: 'Offline' });
    }
}
