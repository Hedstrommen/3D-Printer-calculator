// In development (dotnet run) the service worker does nothing,
// so you always test the latest code. The real offline logic
// lives in service-worker.published.js, which is what gets deployed.
self.addEventListener('fetch', () => { });
