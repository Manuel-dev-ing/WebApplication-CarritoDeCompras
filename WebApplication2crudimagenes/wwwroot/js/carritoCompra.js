document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM carrito");
    myfunction();
    
    recuperarLocalStorage();
    mostrarToastAgregarCarrito();
    
    actualizarPreciosInputNumber();
    calcularTotal();
    contarElementos();
    procesarCompra();
    
    document.querySelectorAll('.agregarCarrito').forEach(btn => {
        btn.addEventListener('click', function () {

            var libroId = this.getAttribute('data-libro');
            var titulo = this.getAttribute('data-titulo');
            var autor = this.getAttribute('data-autor');
            var precio = this.getAttribute('data-precio');
            var imagen = this.getAttribute('data-imagen');

            const libro = {
                Id: libroId,
                Titulo: titulo,
                Autor: autor,
                Precio: precio,
                Cantidad: stock,
                Imagen: imagen
            }

            //var stockAgotado = document.getElementById("stockAgotado").value;

            let id_libro;
            let libros;

            libros = recuperarLocalStorage();

            libros.forEach(lib => {
                if (lib.Id === libro.Id) {
                    id_libro = lib.Id;
                }
            })
            if (id_libro === libro.Id) {
                let mensaje = 'El libro ya existe en el carrito';
                mostrarMensajeError(mensaje);

            }else {
                template = `
                        <div class="d-flex gap-3" id="itemsCarrito" data-libroId="${libro.Id}">
                            <input type="hidden" id="inputHidden" value="${libro.Id}" />
                                <div class="imagen border col-4">
                                    
                                    <img src="~/Imagenes/${libro.Imagen}"/>

                                </div>
                                <div class="border titulos">
                                    <div class="titulo border">
                                        <p>${libro.Titulo}</p>
                                    </div>
                                </div>

                                <div class="autor border">
                                    <p>${libro.Autor}</p>
                                </div>
                                <div class="precio border">
                                    <input class="cantidadItem form-control col-2" type="number" min="1" value="${libro.Cantidad}" />
                                </div>

                                <div class="border precio">
                                    <p>$${libro.Precio}</p>
                                </div>
                                <div class="border precio">
                                    <button class="btn btn-sm btn-outline-danger" onClick="eliminarItem(${libro.Id})"><i class="bi bi-x-lg"></i></button>
                                </div>
                            </div>
                `;

                $('#listaCarrito').append(template);
                agregarLocalStorage(libro);

                console.log("Agregado al carrito");
                contarElementos();
                calcularTotal();
                actualizarPreciosInputNumber();
            }



        });
    })

   
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

    async function procesarCompra() {
        var btnCompra = document.getElementById('comprar');

        btnCompra.addEventListener('click', async function () {
            var linkElement = btnCompra.getAttribute('data-url');

            console.log("link Elemen: ", linkElement);
            var subTotal = document.getElementById('subtotal').textContent;
            var Total = document.getElementById('total').textContent;

            let estaAutenticado = document.getElementById('esAutenticado').value;

            const requestData = {
                ItemsCarrito: recuperarLocalStorage(),
                SubTotal: subTotal,
                Total: Total
            };

            var datos = JSON.stringify(requestData);

            console.log('click btn comprar');
        
            console.log('requestData: ');
            console.log(requestData);

            if (estaAutenticado == 0) {
                let mensaje = "Inicia Sesion Para proceder con la compra"
                mostrarMensajeError(mensaje);
            }

           const respuesta = await fetch(linkElement, {
                method: 'POST',
                body: datos,
                headers: {
                    'Content-Type': 'application/json'
                }
           });
            console.log("respuesta FETCH: ")

            if (respuesta.ok) {
                Swal.fire({
                    icon: 'success',
                    title: 'Compra Realizada Exitosamente',
                    text: 'tu compra ha sido realizada exitosamente'
                });
                console.log("log OK: ");

                console.log(respuesta)
                eliminarElementos();

                eliminarLocalStorage()
                console.log("localStora eliminado");
                contarElementos();
            }

            const result = await respuesta.json();
            

            if (result.code == "400") {

                console.log("ERROR: ")
                console.log(result);
                template = `
                        <div class="alert alert-warning alert-dismissible fade show col-6" id="alerta">
                            <strong>No hay Stock para el libro "${result.mensaje}"</strong>
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                        </div>
                    `;
                $('#alertcontenedor').append(template);

                var alerta = document.getElementById('alerta');
                console.log(alerta);

                setTimeout(() => {

                    $('#alerta').hide();
                    alerta.remove();

                }, 10000);

            }

            
        })

    }

    function eliminarLocalStorage() {
        localStorage.clear();
    }

    function myfunction() {
        console.log("DOM myfunction");

        let librosCarrito;
        let template;

        librosCarrito = JSON.parse(localStorage.getItem('libros'));
        if (librosCarrito != null) {
            librosCarrito.forEach(lib => {
                
                template = `

                    <div class="d-flex gap-3" id="itemsCarrito" data-libroId="${lib.Id}">
                            <input type="hidden" id="inputHidden" value="${lib.Id}" />
                                <div class="imagen border col-3">
                                    
                                    <img class="imagenCarrito" src="/Imagenes/${lib.Imagen}" style="height:195px"/>

                                </div>
                                <div class="border titulos">
                                    <div class="titulo border">
                                        <p>${lib.Titulo}</p>
                                    </div>
                                </div>

                                <div class="autor border">
                                    <p>${lib.Autor}</p>
                                </div>
                                <div class="precio border">
                                    <input class="cantidadItem form-control col-2" id="cantidadId" type="number" min="1" value="${lib.Cantidad}" />
                                </div>

                                <div class="border precio">
                                    <p>$${lib.Precio}</p>
                                </div>
                                <div class="border precio">
                                    <button class="btn btn-sm btn-outline-danger" onClick="eliminarItem(${lib.Id})"><i class="bi bi-x-lg"></i></button>
                                </div>
                            </div>
                    `;
                $('#listaCarrito').append(template);

            });
            //localStorage.setItem('libros', JSON.stringify(librosCarrito));
        }
     

        //console.log(template)

    }

    
});
function recuperarLocalStorage() {
    let libros;

    if (localStorage.getItem('libros') === null) {
        libros = [];
    } else {
        libros = JSON.parse(localStorage.getItem('libros'))
    }

    return libros
}

function actualizarPreciosInputNumber() {

    document.querySelectorAll('#cantidadId').forEach(btn => {
        btn.addEventListener('change', function (event) {

            var id = btn.parentElement.parentElement.getAttribute('data-libroId');
            var cantidad = event.target.value

            var libros = recuperarLocalStorage();
            console.log(libros);

            libros.forEach(libro => {
                if (libro.Id === id) {
                    var total = libro.Precio * cantidad;
                    console.log("libro precio total: " + total);
                    libro.Cantidad = parseInt(cantidad);

                }
            })
            localStorage.setItem('libros', JSON.stringify(libros));
            console.log("cantidad: " + cantidad);
            console.log("El valor del Atributo id: " + id)
            calcularTotal();
            contarElementos(); 
        })
    })

}

function calcularTotal() {
    let libros = 0;
    var Total = 0;
    var precio = 0;

    formatDollar = new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        minimumFractionDigits: 2,
    });


    
    libros = JSON.parse(localStorage.getItem('libros'))


    if (libros == null) {
        Total = 0;
    } else {
        libros.forEach(elemento => {
            var subTotal = elemento.Precio * elemento.Cantidad;
            Total = subTotal + Total;
            
        })
        let totalconiIVA = calcularTotalConIVA(Total);
        Total = formatDollar.format(Total);

        subtotalId = document.getElementById('subtotal');
        totalId = document.getElementById('total');
       // precioid = document.getElementById('precioid');

        //subTotal
        subtotalId.textContent = Total; 

        /*let totaliva = calcularTotalConIVA(Total);
        console.log("totaliva");
        console.log(totaliva);*/
        

        //Total
        totalId.textContent = totalconiIVA;


    }

}

function calcularTotalConIVA(total) {
    let totalConIva = 0;
    let iva = 16;

    formatDollar = new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        minimumFractionDigits: 2,
    });


    let porcentaje = iva / 100;

    let subTotal = total * porcentaje;

    totalConIva = total + subTotal;

    totalConIva = formatDollar.format(totalConIva);

    return totalConIva;
}

function contarElementos() {
    let libros = 0;
    let contador = 0;

    libros = JSON.parse(localStorage.getItem('libros'))

    console.log("funcion contar elementos subTotal y cantiDAD: ");
    console.log(libros);
    if (libros == null) {

        contador = 0;

    } else if (libros !== null) {
        libros.forEach(libro => {
            let cantidad = parseInt(libro.Cantidad)
            contador = contador + cantidad;
        });
        console.log("cantidad items:", contador);
    }
    let contarItems = document.getElementById('subTotal');
    contarItems.textContent = "Subtotal (" + contador + " libros)";

    return contador;
}

function eliminarItem(id) {
    let libros;

    //libros = recuperarLocalStorage();

    libros = JSON.parse(localStorage.getItem('libros'));

    libros.forEach(function (libro, indice) {
        if (libro.Id == id) {
            console.log("el id si es igual");
            libros.splice(indice, 1);
            /*eliminarItem.remove();*/
            eliminarFilaPorAtributo(id);
            localStorage.setItem('libros', JSON.stringify(libros));
        }
    });
    
    contarElementos();
    calcularTotal();
    actualizarPreciosInputNumber();
    
}


function eliminarElementos() {
    console.log("funcion eliminarElementos ");
    libros = JSON.parse(localStorage.getItem('libros'));

    libros.forEach(libro => {
        let id = libro.Id
        eliminarFilaPorAtributo(id)
    })
}

function eliminarFilaPorAtributo(id) {
    console.log("atributo id de la fila", id);

    var div = document.getElementById('itemsCarrito');

    var divfila = document.querySelector('[data-libroId="' + id + '"]');

    if (divfila) {
        divfila.remove();
        console.log("DIV ELIMINAD0");
 
    }
}




