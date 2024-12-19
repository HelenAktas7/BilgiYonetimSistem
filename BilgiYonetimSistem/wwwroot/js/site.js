document.addEventListener('DOMContentLoaded', function () {
    // Giriş formu doğrulama
    const form = document.querySelector('form');
    form.addEventListener('submit', function (e) {
        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        // Kullanıcı adı veya şifre boş ise uyarı göster
        if (!username || !password) {
            e.preventDefault();
            alert("Kullanıcı adı ve şifre boş olamaz!");
        }
    });
});
