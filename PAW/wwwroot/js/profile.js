// Change Username
document.getElementById('change-username').addEventListener('click', async function (e) {
    const userName = document.getElementById('UserNameInput').value;
    const formData = new FormData();
    formData.append('UserName', userName);

    const response = await fetch(window.location.pathname, {
        method: 'POST',
        body: formData,
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    });

    const data = await response.json();
    if (data.success && data.userName) {
        document.getElementById('navbar-username').textContent = data.userName;
        document.getElementById('UserNameInput').value = data.userName;
    } else {
        alert('Username change failed! ' + (data.error || 'invalid sau deja există'));
    }
});

// Change Photo
document.getElementById('change-photo').addEventListener('click', async function (e) {
    const photoInput = document.getElementById('upload-profile-pic');
    if (!photoInput.files.length) {
        alert('Select a photo first!');
        return;
    }
    const formData = new FormData();
    formData.append('profileImage', photoInput.files[0]);

    const response = await fetch(window.location.pathname, {
        method: 'POST',
        body: formData,
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    });

    const data = await response.json();
    if (data.success && data.profileImageUrl) {
        document.getElementById('profile-img').src = data.profileImageUrl + '?v=' + Math.random();
        document.getElementById('navbar-profile-img').src = data.profileImageUrl + '?v=' + Math.random();
    } else {
        alert('Photo change failed!');
    }
});

// Local preview pentru profil
document.getElementById('upload-profile-pic').addEventListener('change', function (event) {
    const [file] = event.target.files;
    if (file) {
        document.getElementById('profile-img').src = URL.createObjectURL(file);
    }
});
