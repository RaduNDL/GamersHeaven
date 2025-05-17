document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.favorite-star').forEach(star => {
        star.addEventListener('click', toggleFavorite);
    });
});

async function toggleFavorite(event) {
    const star = event.currentTarget;
    const gameId = star.getAttribute('data-game');
    const isFavorited = star.classList.contains('favorited');

    const url = isFavorited
        ? '/Favorites/RemoveByGameId'
        : '/Favorites/AddToFavorites';

    const body = new URLSearchParams();
    body.append('gameId', gameId);

    try {
        const res = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=UTF-8'
            },
            body: body.toString()
        });

        if (!res.ok) {
            console.error(`HTTP ${res.status} – nu s-a putut actualiza favoritele.`);
            alert('Eroare la actualizarea listei de favorite.');
            return;
        }

        if (isFavorited) {
            star.classList.remove('favorited');
            star.textContent = '☆';
        } else {
            star.classList.add('favorited');
            star.textContent = '★';
        }

    } catch (err) {
        console.error('Favorite toggle error:', err);
        alert('A apărut o eroare la server. Încearcă din nou.');
    }
}
