document.addEventListener("DOMContentLoaded", function () {
    const toggle = document.getElementById("GenreDropdown-toggle");
    const menu = document.getElementById("GenreDropdown-menu");
    const content = document.getElementById("contentWrapper");
    const filterButtons = document.querySelectorAll(".filter-btn");
    const gameCards = document.querySelectorAll(".game-card");

    toggle.addEventListener("click", function (e) {
        e.stopPropagation();
        menu.classList.toggle("show");
        content.classList.toggle("with-sidebar");
    });

    document.addEventListener("click", function (e) {
        if (!menu.contains(e.target) && !toggle.contains(e.target)) {
            menu.classList.remove("show");
            content.classList.remove("with-sidebar");
        }
    });

    filterButtons.forEach(button => {
        button.addEventListener("click", () => {
            const selectedGenre = button.getAttribute("data-filter");

            gameCards.forEach(card => {
                const genre = card.getAttribute("data-genre");
                card.style.display = (selectedGenre === "all" || genre === selectedGenre) ? "block" : "none";
            });

            menu.classList.remove("show");
            content.classList.remove("with-sidebar");
        });
    });
});
