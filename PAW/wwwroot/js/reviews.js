document.addEventListener("DOMContentLoaded", function () {
    const gameId = parseInt(document.getElementById("add-to-cart-direct").getAttribute("data-gameid"));
    const currentUserId = window.currentUserId;

    function renderStars(rating) {
        return "★".repeat(rating) + "☆".repeat(5 - rating);
    }

    function loadReviews() {
        fetch(`/reviews/${gameId}`)
            .then(response => response.json())
            .then(data => {
                const list = document.getElementById("review-list");
                list.innerHTML = '';
                if (data.length === 0) {
                    list.innerHTML = "<p>No reviews yet.</p>";
                } else {
                    data.forEach(r => {
                        const isMyReview = currentUserId && (currentUserId === r.userId);
                        list.innerHTML += `
                            <div class="single-review" data-reviewid="${r.reviewID}">
                                <div class="review-header" style="display:flex;align-items:center;gap:1rem;margin-bottom:0.3rem;">
                                    <img src="${r.imagePath}" alt="avatar" class="review-avatar" style="width:48px;height:48px;border-radius:50%;object-fit:cover;">
                                    <div>
                                        <b class="review-username">${r.userName || "Unknown user"}</b>
                                        <div class="review-stars" style="color:#f2cb57;font-size:1.15em;">${renderStars(r.rating)}</div>
                                    </div>
                                </div>
                                <div class="review-body">
                                    <div style="font-size:0.98em;color:#b3b3c6;"><b>Date:</b> ${new Date(r.date).toLocaleString()}</div>
                                    <div class="review-comment" style="margin:0.5em 0;">${r.comment}</div>
                                </div>
                                <div class="review-actions" style="margin-top: 0.4rem;">
                                    ${isMyReview ? `
                                        <button class="edit-review-btn" data-reviewid="${r.reviewID}" data-rating="${r.rating}" data-comment="${r.comment}">Edit</button>
                                        <button class="delete-review-btn" data-reviewid="${r.reviewID}">Delete</button>
                                    ` : ''}
                                </div>
                                <hr>
                            </div>`;
                    });
                }

                document.querySelectorAll('.delete-review-btn').forEach(btn => {
                    btn.onclick = function () {
                        const reviewId = parseInt(this.getAttribute('data-reviewid'));
                        if (confirm('Are you sure you want to delete this review?')) {
                            fetch(`/reviews/delete/${reviewId}`, { method: 'DELETE' })
                                .then(resp => {
                                    if (resp.ok) loadReviews();
                                    else alert("Delete failed.");
                                });
                        }
                    }
                });

                document.querySelectorAll('.edit-review-btn').forEach(btn => {
                    btn.onclick = function () {
                        const reviewId = parseInt(this.getAttribute('data-reviewid'));
                        const oldComment = this.getAttribute('data-comment');
                        const oldRating = this.getAttribute('data-rating');
                        const newComment = prompt("Edit your comment:", oldComment);
                        let newRating = prompt("Edit your rating (1-5):", oldRating);

                        if (newComment === null || newRating === null) return;
                        newRating = parseInt(newRating);

                        fetch(`/reviews/edit/${reviewId}`, {
                            method: 'PUT',
                            headers: { "Content-Type": "application/json" },
                            body: JSON.stringify({
                                reviewID: reviewId,
                                comment: newComment,
                                rating: newRating,
                                gameID: gameId
                            })
                        })
                            .then(resp => {
                                if (resp.ok) loadReviews();
                                else alert("Edit failed.");
                            });
                    }
                });
            });
    }

    document.getElementById("submit-review").onclick = function () {
        const comment = document.getElementById("review-text").value;
        let rating = prompt("Give a rating from 1 to 5:");

        if (!comment.trim() || !rating) {
            alert("You must write a comment and give a rating!");
            return;
        }
        rating = parseInt(rating);

        fetch('/reviews/add', {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                comment: comment,
                rating: rating,
                gameID: gameId
            })
        })
            .then(resp => {
                if (resp.ok) {
                    document.getElementById("review-text").value = '';
                    loadReviews();
                } else {
                    alert("Failed to add review. Try again!");
                }
            });
    };

    loadReviews();
});
