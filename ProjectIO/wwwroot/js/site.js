// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", () => {
    const toggleCheckbox = document.getElementById("theme-toggle-checkbox");

    // Check saved theme preference in localStorage
    const savedTheme = localStorage.getItem("theme");
    if (savedTheme === "dark") {
        document.body.classList.add("dark-mode");
        toggleCheckbox.checked = true; // Set toggle switch to "on"
    }

    // Toggle theme and save preference
    toggleCheckbox.addEventListener("change", () => {
        if (toggleCheckbox.checked) {
            document.body.classList.add("dark-mode");
            localStorage.setItem("theme", "dark");
        } else {
            document.body.classList.remove("dark-mode");
            localStorage.setItem("theme", "light");
        }
    });
});


