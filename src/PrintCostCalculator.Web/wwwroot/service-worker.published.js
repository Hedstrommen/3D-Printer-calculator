// The deployed service worker: caches the whole app so the calculator
// works fully OFFLINE after your first visit (on the web or installed as an app).
// The build injects self.assetManifest with the list of app files.

self.addEventListener('install', async (event) => {
    async function addUnknownCacheEntries(cache) {
        const cacheEntries = [];
        const assetManifest = self.assetManifest || [];
        for (const asset of assetManifest) {
            if (!asset.url || !asset.url.endsWith('/')) {
                cacheEntries.push(new Request(asset.url, { cache: 'reload' }));
            }
        }
        await Promise.all(cacheEntries.map((entry) => cache.add(entry)));
    }

    event.waitUntil(
        (async () => {
            const cache = await caches.open(cacheName);
            await addUnknownCacheEntries(cache);
        })()
    );
}, false);

self.addEventListener('activate', (event) => {
    event.waitUntil(
        (async () => {
            const keys = await caches.keys();
            await Promise.all(keys.filter((key) => key.startsWith(cacheNamePrefix) && key !== cacheName).map((key) => caches.delete(key)));
        })()
    );
}, false);

const cacheNamePrefix = 'blazor-resources-v';
const cacheName = `${cacheNamePrefix}${self.assetsManifest ? self.assetsManifest.id : '1'}`;

self.addEventListener('message', (event) => {
    if (event.data === 'SKIP_PWA_WAITING') {
        self.skipWaiting();
    }
});

self.addEventListener('fetch', (event) => {
    if (event.request.method !== 'GET' || event.request.headers.has('range') || event.request.mode === 'websocket') {
        return;
    }

    const shouldServeIndexHtml = event.request.mode === 'navigate';
    const request = shouldServeIndexHtml ? new Request('index.html', { cache: 'reload' }) : event.request;
    const shouldCache = event.request.method === 'GET' && event.request.url.startsWith(self.location.origin) && !event.request.url.includes('/api/');

    event.respondWith(
        (async () => {
            const cache = await caches.open(cacheName);
            const cachedResponse = await cache.match(request);
            if (cachedResponse && !shouldServeIndexHtml) {
                return cachedResponse;
            }

            try {
                const networkResponse = await fetch(request);
                if (shouldCache && networkResponse && networkResponse.status === 200 && networkResponse.type === 'basic') {
                    const clonedResponse = networkResponse.clone();
                    await cache.put(request, clonedResponse);
                }
                return networkResponse;
            } catch (error) {
                const fallbackResponse = await cache.match(request);
                if (fallbackResponse) {
                    return fallbackResponse;
                }
                if (shouldServeIndexHtml) {
                    return new Response('Offline', { status: 503, statusText: 'Offline' });
                }
                throw error;
            }
        })()
    );
});
