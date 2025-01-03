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

// Funkcja do zmiany wielkości czcionki
document.addEventListener('DOMContentLoaded', () => {
    const buttons = document.querySelectorAll('.font-size-btn');
    const defaultSize = '16px';

    // Przywrócenie zapisanego rozmiaru czcionki
    const savedSize = localStorage.getItem('fontSize') || defaultSize;
    document.documentElement.style.setProperty('--base-font-size', savedSize);
    setActiveButton(savedSize);

    // Obsługa kliknięcia przycisku
    buttons.forEach(button => {
        button.addEventListener('click', () => {
            const size = button.getAttribute('data-size');
            document.documentElement.style.setProperty('--base-font-size', size);
            localStorage.setItem('fontSize', size);
            setActiveButton(size);
        });
    });

    // Ustawienie aktywnego przycisku
    function setActiveButton(size) {
        buttons.forEach(button => {
            if (button.getAttribute('data-size') === size) {
                button.classList.add('active');
            } else {
                button.classList.remove('active');
            }
        });
    }
});



