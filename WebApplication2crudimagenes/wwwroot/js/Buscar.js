$('#buscar').keyup(function () {
    var valor = $(this).val();
    console.log("valor: " + valor);

    $('tbody tr').each(function () {
        console.log("fila: ", $(this).text().search(new RegExp(valor, "i")));

        if ($(this).text().search(new RegExp(valor, "i")) < 0) {

            $(this).fadeOut();
            console.log("fade: ", $(this).fadeOut());

        } else {


            $(this).show();
        }
    });


})