// Small quality-of-life helpers for the calculator.
// The app works fully without JavaScript; this only adds polish.

document.addEventListener("DOMContentLoaded", function () {
    // Focus the first input on desktop so you can start typing right away.
    if (window.matchMedia("(min-width: 700px)").matches) {
        var firstInput = document.getElementById("filamentPriceInput");
        if (firstInput) {
            firstInput.focus();
        }
    }
});

// GitHub Pages SPA redirect: when someone opens a deep link (e.g. a shared
// /some/page URL), GitHub serves 404.html first. That page stores the path
// here, then redirects to index.html, which restores it.
(function () {
    var isDeepLinkRedirect = window.location.search.indexOf("redirected-from=") !== -1;
    if (isDeepLinkRedirect) {
        var pathAndQuery = window.location.search.substring("redirected-from=".length + 1);
        sessionStorage.redirectPath = pathAndQuery;
    }
})();
