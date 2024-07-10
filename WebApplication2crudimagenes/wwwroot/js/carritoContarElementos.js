document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM carrito");
  
    contarElementos();



    //procesarCompra();
    document.querySelectorAll('.agregarCarrito').forEach(btn => {
        btn.addEventListener('click', function () {

            var libroId = this.getAttribute('data-libro');
            var titulo = this.getAttribute('data-titulo');
            var autor = this.getAttribute('data-autor');
            var precio = this.getAttribute('data-precio');
            var imagen = this.getAttribute('data-imagen');
            var stock = this.getAttribute('data-stock');

            const libro = {
                Id: libroId,
                Titulo: titulo,
                Autor: autor,
                Precio: precio,
                Cantidad: 1,
                Imagen: imagen
            }

            console.log("sotck: ", stock)

            let id_libro;
            let libros;

            libros = recuperarLocalStorage();

            libros.forEach(lib => {
                if (lib.Id === libro.Id) {
                    id_libro = lib.Id;
                }
            })
            if (stock == 0) {
                let mensaje = "Libro Agotado. No hay stock para este libro";
                mostrarMensajeError(mensaje);
             

            }
            else if (id_libro === libro.Id) {

                let mensaje = 'El libro ya existe en el carrito';
                mostrarMensajeError(mensaje);

            }else {
                mostrarToastAgregarCarrito();
                agregarLocalStorage(libro);

                
                console.log("Agregado al carrito");
                contarElementos();
                
            }
        });
    })

    function recuperarLocalStorage() {
        let libros;

        if (localStorage.getItem('libros') === null) {
            libros = [];
        } else {
            libros = JSON.parse(localStorage.getItem('libros'))
        }

        return libros
    }

    function agregarLocalStorage(libro) {
        let libros;
        libros = recuperarLocalStorage();
        libros.push(libro);

        localStorage.setItem('libros', JSON.stringify(libros));
    }

    function mostrarMensajeError(mensaje) {
        Swal.fire({
            icon: 'error',
            title: 'Error...',
            text: mensaje
        });
    }

    function mostrarToastAgregarCarrito() {
        document.querySelectorAll('#toastbtn').forEach(btn => {
            btn.addEventListener('click', function () {
                const liveToast = document.getElementById('liveToast');

                const toastBootstrap = bootstrap.Toast.getOrCreateInstance(liveToast);
                toastBootstrap.show();



            })


        })



    }

});




function contarElementos() {
    let libros = 0;
    let contador = 0;

    libros = JSON.parse(localStorage.getItem('libros'))

    if (libros == null) {

        contador = 0;

    } else if (libros !== null) {
        libros.forEach(libro => {
            contador++;
        });
    }

    var itemsContar = document.getElementById('contador');
    itemsContar.textContent = contador;

    console.log("cantidad de productos: ", contador);
    return contador;
}