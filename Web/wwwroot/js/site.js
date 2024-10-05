// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// variables
const overlay    = document.getElementById("nav-overlay");
const navContent = document.getElementById("nav-content");
const toggle = document.getElementById("header-toggle");

// button event
toggle && toggle.addEventListener("click", () => {
        try {
            overlay.classList.toggle("nav_visible");
            navContent.classList.toggle("show_navigate");
        } catch (error) {
            console.log(error);
        }
    });

// webpage controller focus manage
// Let the document know when the mouse is being used
document.body.addEventListener("mousedown", function () {
    try {
        document.body.classList.add("used-mouse");
    } catch (error) {
        console.log(error);
    }
});

// re-enable focus styling when Tab is pressed
document.body.addEventListener("keydown", function (event) {
    try {
        if (event.keyCode === 9) {
            document.body.classList.remove("used-mouse");
        }
    } catch (error) {
        console.log(error);
    }
});

// go back
const goBack = document.getElementById("go-back");

goBack && goBack.addEventListener("click", (e) => {
    try {
        let reffer = document.referrer;
        window.location.replace(reffer);
    } catch (error) {
        console.log(error);
    }
});
