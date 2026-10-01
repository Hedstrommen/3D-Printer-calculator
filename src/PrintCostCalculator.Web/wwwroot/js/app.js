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
