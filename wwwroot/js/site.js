// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Basic UI behaviors for responsiveness
document.addEventListener('DOMContentLoaded', function () {
    var menuToggle = document.getElementById('menuToggle');
    var wrapper = document.getElementById('wrapper');
    var sidebarOverlay = document.getElementById('sidebarOverlay');

    if (menuToggle && wrapper) {
        menuToggle.addEventListener('click', function () {
            wrapper.classList.toggle('toggled');
        });
    }

    if (sidebarOverlay) {
        sidebarOverlay.addEventListener('click', function () {
            wrapper.classList.remove('toggled');
        });
    }
});
