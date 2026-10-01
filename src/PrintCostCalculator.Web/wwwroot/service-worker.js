// Minimal service worker so the app can be installed as a PWA on phones.
// Serves cached static files; the app itself runs live via the server.

const CACHE_NAME = "print-cost-calculator-v1";
const PRECACHE_ASSETS = [
    "/css/app.css",
    "/js/app.js",
    "/favicon.svg",
    "/manifest.json"
];

self.addEventListener("install", (event) => {
    event.waitUntil(caches.open(CACHE_NAME).then((cache) => cache.addAll(PRECACHE_ASSETS)));
    self.skipWaiting();
});

self.addEventListener("activate", (event) => {
    event.waitUntil(
        caches.keys().then((keys) =>
            Promise.all(keys.filter((key) => key !== CACHE_NAME).map((key) => caches.delete(key)))
        )
    );
    self.clients.claim();
});

self.addEventListener("fetch", (event) => {
    // Only handle static file requests; let everything else hit the network.
    const requestUrl = new URL(event.request.url);
    if (requestUrl.origin !== self.location.origin) {
        return;
    }

    const isStaticFile =
        requestUrl.pathname.startsWith("/css/") ||
        requestUrl.pathname.startsWith("/js/") ||
        requestUrl.pathname === "/favicon.svg" ||
        requestUrl.pathname === "/manifest.json";

    if (!isStaticFile) {
        return;
    }

    event.respondWith(
        caches.match(event.request).then((cachedResponse) => cachedResponse || fetch(event.request))
    );
});
