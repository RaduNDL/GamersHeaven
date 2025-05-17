document.getElementById("addNewGenre").addEventListener("click", async function () {
    const newGenreInput = document.getElementById("newGenre");
    const newGenre = newGenreInput.value.trim();
    const genreDropdown = document.getElementById("genreDropdown");
    const errorSpan = document.getElementById("newGenreError");

    if (!newGenre) {
        errorSpan.textContent = "Please enter a genre name.";
        return;
    }

    const exists = Array.from(genreDropdown.options).some(opt => opt.text.toLowerCase() === newGenre.toLowerCase());
    if (exists) {
        errorSpan.textContent = "Genre already exists in dropdown.";
        return;
    }

    try {
        const response = await fetch("/Games/AddGenreAjax", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newGenre)
        });

        if (!response.ok) {
            throw new Error("Failed to save genre.");
        }

        const result = await response.json();

        const option = document.createElement("option");
        option.value = result.id;
        option.text = result.name;
        genreDropdown.appendChild(option);
        genreDropdown.value = result.id;

        newGenreInput.value = "";
        errorSpan.textContent = "";
    } catch (err) {
        console.error(err);
        errorSpan.textContent = "Error saving genre.";
    }
});
