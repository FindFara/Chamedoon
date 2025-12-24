document.getElementById('ProfileImageFile')?.addEventListener('change', function (event) {
    const file = event.target.files?.[0];
    if (!file) {
        return;
    }
    const reader = new FileReader();
    reader.onload = function (e) {
        const target = document.getElementById('avatarPreview');
        if (target && e.target?.result) {
            target.src = e.target.result;
        }
    };
    reader.readAsDataURL(file);
});
