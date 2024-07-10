
$(document).ready(function () {
    $('#img').change(function (e) {

        var url = $('#img').val();
        ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        console.log("ext: " + ext);
        if (img.files && img.files[0] && (ext == "gif" || ext == "jpg" || ext == "jfif" || ext == "png" || ext == "bmp")) {
            var reader = new FileReader();
            reader.onload = function () {
                var output = document.getElementById('prevImg');
                output.src = reader.result;    
            }
            reader.readAsDataURL(e.target.files[0]);
        }
    })

})









