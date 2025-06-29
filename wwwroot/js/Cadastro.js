// CARREGAR IMAGEM EM CRIAR
document.addEventListener('DOMContentLoaded', function() {
    const uploadCapaInput = document.getElementById('uploadCapa');
    const previewCapaImg = document.getElementById('previewCapa');

    if (uploadCapaInput && previewCapaImg) {
        uploadCapaInput.addEventListener('change', function(event) {
            const file = event.target.files[0];

            if (file) {
                const reader = new FileReader(); 

                reader.onload = function(e) {
                    previewCapaImg.src = e.target.result;
                };

                reader.readAsDataURL(file); 
            } else {
                previewCapaImg.src = ""; 
            }
        });
    }
});