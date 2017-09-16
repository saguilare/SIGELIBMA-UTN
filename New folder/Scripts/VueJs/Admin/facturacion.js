/*********************************
***          Attention
*** some components used in this 
*** tool such as datepicker come  
*** from vuestrap.js and need vue.js
*** version lower than 2.0
*********************************/

//prod
var urlRoot = 'http://localhost:2814/';
//Dev
//var urlRoot = '';


var data = {};
data.enableFacturar = false;
data.emailSupport = "iglesiamana@gmail.com";
data.books = [];
data.codigos = [];
data.titulos = [];
data.searchByData = data.codigos;
data.searchSelected = "";
data.product = { item: {}, quantity: 1,subtotal:0,total:0 };
data.modalObject = { Codigo: 0, Descripcion: '', Usuario: null, Rol: 0 };
data.modalFact = { currentPage: 1 };
data.cashier = { Codigo: 0, Descripcion: "", Estado: 0, Monto: 0, MontoReal :0};
data.cashBoxes = [];
data.tiposPago = [];
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = { show: false, placement: "top-right", duration: "3000", type: "danger", width: "400px", dismissable: true, message: '' };
data.session = { id: 1, user: {name : 'steven aguilar', username : 'saguilar'}};
data.factura = { master: { id: "", client: { id: "", name: '',lastname:"",phone:"",email:"" },tipoPago:{Codigo:0,Descripcion:""},referencia:"", taxes: 0,subtotal:0, total: 0, totalReceived: 0, change: 0, totalItems: 0, date: '00/00/0000' }, details: [] };
data.validations = { showSpinner: false, loadingMessage: 'Cargando información de la base de datos, por favor espere.' };
data.showFacturaModelNavbar = false;
data.movimientosDelDia = [];
data.modalRetiroAbono = { currentPage: 1 };
data.movimiento = { caja: {}, tipo: 0, monto: 0, razon:"" ,procesado: false};
data.tiposMovimiento = [{ codigo: 1, descripcion: 'Abono' }, { codigo: 2, descripcion: 'Retiro' }];
data.cierre = { saldo: 0, totalCreditos: 0, totalRetiros: 0, total: 0, montoCaja: 0, faltante: 0, exedente: 0 };
data.quantity = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
data.validateCajaCombo = false;
data.disableBuyButton = false;

Vue.filter('numeral', function (value) {
    return numeral(value).format('0,0');
})

//$("#collapseFactMain").collapse('show');
var vm = new Vue({
    el: '#pageMainContainer',
    data: data,
    components: {
        typeahead: VueStrap.typeahead,
        vueinput: VueStrap.input,
        toastr: VueStrap.alert,
        spinner: VueStrap.spinner,
    },

    //Define methods
    methods: {

        //PaginationMethods

        changeSearch: function (search) {
            if (search === 1) {
                vm.searchByData = vm.codigos;
            } else {
                vm.searchByData = vm.titulos;
            }
        },
        lowerCase: function (stringValue) {
            return stringValue.toLowerCase();
        },
        displaySpinner: function (status, message) {
            vm.validations.showSpinner = status;
            vm.validations.loadingMessage = message;
        },

        //for critical erros - unable to load page init data
        activateAlert: function (type, message, status) {
            vm.alert.type = type;
            vm.alert.message = message;
            vm.alert.status = status;
        },
        //alert in open cashbox modal
        activateAlertModal: function (type, message, status) {
            vm.alertModal.type = type;
            vm.alertModal.message = message;
            vm.alertModal.status = status;
        },
        //toast
        activateToastr: function (type, message, status) {
            vm.toastr.show = status;
            vm.toastr.placement = 'top-right';
            vm.toastr.duration = 10000;
            vm.toastr.type = type;
            vm.toastr.width = '300px';
            vm.toastr.dismissable = true;
            vm.toastr.message = message;
        },

        initializeCashBox: function () {
            if (vm.cashier.Monto <= 0) {
                vm.activateAlertModal("danger","El fondo de caja no puede ser menor o igual a 0",true);
                return false;
            }
            vm.enableFacturar = false;
            vm.$refs.spinner1.show();
            vm.cashier.Estado = 1;
            vm.cashier.MontoReal = vm.cashier.Monto;
            $.ajax({
                url: urlRoot + 'Facturacion/AbrirCerrarCaja',
                type: 'post',
                dataType: 'json',
                data: vm.cashier,
                success: function (result) {
                    vm.$refs.spinner1.hide();
                    if (result.EstadoOperacion) {
                        $('#modal-caja').modal('hide');
                        vm.activateToastr('success', 'La caja ha sido inicializada.', true);
                        $('#collapseFactMain').collapse('show');
                        vm.actualizarMovimientos();
                    } else {
                        vm.cashier.Estado = 2;
                        vm.activateAlertModal('danger','La caja no se inicializo, trate nuevamente.',true);
                    }
                },
                error: function (error) {
                    vm.$refs.spinner1.hide();
                    vm.cashier.Estado = 2;
                    vm.activateAlertModal('danger', 'La caja no se inicializo, trate nuevamente.', true);
             
                }
            });

        },

        updateClosing: function () {
            if (vm.cierre.total > vm.cierre.montoCaja) {
                vm.cierre.exedente = 0;
                vm.cierre.faltante = vm.cierre.total - vm.cierre.montoCaja;
            } else if (vm.cierre.total < vm.cierre.montoCaja) {
                vm.cierre.faltante = 0;
                vm.cierre.exedente = vm.cierre.montoCaja - vm.cierre.total;
            } else {
                vm.cierre.faltante = 0;
                vm.cierre.exedente = 0;
            }
        },

        closeCashBox: function () {
            vm.$refs.spinner3.show();
            vm.cashier.Estado = 2;
            vm.cashier.Monto = vm.cierre.total;
            vm.cashier.MontoReal = vm.cierre.montoCaja;
            $.ajax({
                url: urlRoot + 'Facturacion/AbrirCerrarCaja',
                type: 'post',
                dataType: 'json',
                data: vm.cashier,
                success: function (result) {
                    if (result.EstadoOperacion) {                   
                        vm.activateToastr('success', 'La caja ha sido cerrada con éxito.', true);
                        $('#modal-cierre').modal('hide');
                        $('#collapseFactMain').collapse('hide');
                        vm.cashier = {};
                        vm.cierre = { saldo: 0, totalCreditos: 0, totalRetiros: 0, total: 0, montoCaja: 0, faltante: 0, exedente: 0 };
                    } else {
                        vm.cashier.Estado = 1;
                        vm.activateAlertModal('danger', 'La caja no se cerro, trate nuevamente.', true);
                    }
                    vm.$refs.spinner3.hide();

                },
                error: function (error) {
                    vm.$refs.spinner3.hide();
                    vm.cashier.Estado = 1;
                    $('#modal-cierre').modal('hide');
                    vm.activateToastr('danger', 'La caja no se cerro, trate nuevamente.', true);

                }
            });

        },

        print: function () {
            
            $.ajax({
                url: urlRoot + 'Facturacion/ImprimirFactura',
                type: 'get',
                dataType: 'json',
                
                success: function (result) {
                    //if (result.EstadoOperacion) {
                    //    vm.movimientosDelDia = result.Movimientos;
                    //    $("#modal-movimientos").modal({ show: true });
                    //} else {
                    //    vm.activateToastr("danger", "Hubo un problema obteniendo los movimientos de la caja", true);
                    //}
                },
                error: function (error) {
                    vm.activateToastr("danger", "Hubo un problema al imprimir, trate nuevamente", true);
                }
            });
        },

        obtenerMovimientos: function () {
            $.ajax({
                url: urlRoot + 'Facturacion/MovimientosCaja',
                type: 'post',
                dataType: 'json',
                data: vm.cashier,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.movimientosDelDia = result.Movimientos;
                        $("#modal-movimientos").modal({ show: true });
                    } else {
                        vm.activateToastr("danger", "Hubo un problema obteniendo los movimientos de la caja", true);
                    }
                },
                error: function (error) {
                    vm.activateToastr("danger", "Hubo un problema obteniendo los movimientos de la caja", true);
                }
            });

        },

        actualizarMovimientos: function () {
            $.ajax({
                url: urlRoot + 'Facturacion/MovimientosCaja',
                type: 'post',
                dataType: 'json',
                data: vm.cashier,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.movimientosDelDia = result.Movimientos;
                        
                    } 
                },
                error: function (error) {
                    console.log("Error al actualizar movimientos");
                }
            });

        },

        verifyStock: function () {
            if (vm.product.quantity > vm.product.item.Stock) {
                vm.activateAlertModal("danger","No puede comprar una cantidad mayor a la disponible",true);
                vm.disableBuyButton = true;
            } else {
                vm.disableBuyButton = false;
                vm.updateModalDetailProdTotal();
                
            }
        },

        getInitData: function () {
            $.ajax({
                url: urlRoot + 'facturacion/Init',
                type: 'get',
                dataType: 'json',
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.cashBoxes = result.Cajas;
                        vm.cashier = result.Caja;
                        if (vm.cashier !== null && vm.cashier.Codigo > 0) {
                            $('#collapseFactMain').collapse('show');
                        }
                        vm.books = result.Libros;
                        vm.tiposPago = result.TiposPago;
                        if (vm.books !== null && vm.books !== undefined && vm.books.length > 0) {
                            $.each(vm.books,function (key, book) {
                                vm.codigos.push(book.Codigo.toString());
                                vm.titulos.push(book.Titulo);
                            });
                        }
                        vm.actualizarMovimientos();
                    } else {
                        vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                    }
                    vm.displaySpinner(false);
                    
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente (Recargue la pagina).', true);
                }
            });
        },

        aplicarMovimiento: function () {
            vm.$refs.spinner1.show();
            vm.cashier.Monto = vm.movimiento.monto;
            vm.cashier.MontoReal = vm.movimiento.monto;
            vm.cashier.Razon = vm.movimiento.razon;
            vm.cashier.Movimiento = vm.movimiento.tipo;
            $.ajax({
                url: urlRoot + 'Facturacion/RetirarAbonarCaja',
                type: 'post',
                dataType: 'json',
                data: vm.cashier,
                success: function (result) {
                    vm.$refs.spinner1.hide();
                    if (result.EstadoOperacion) {
                        vm.movimiento.procesado = true;
                        vm.actualizarMovimientos();
                    } else {
                        vm.activateAlertModal('danger', 'La transaccion no se proceso, por favor intente de nuevo.', true);
                    }
                },
                error: function (error) {
                    vm.$refs.spinner1.hide();
                    vm.activateAlertModal('danger', 'La transaccion no se proceso, por favor intente de nuevo.', true);

                }
            });
        },

        openModal: function (object, target) {
            vm.disableBuyButton = false;
            vm.activateAlertModal('info','',false);
            if (target.toLowerCase() === 'modal-caja') {
                vm.cashier = { Codigo: 0, Descripcion: "", Estado: 0, Monto: 0, MontoReal :0 };
                if (vm.cashBoxes === null || vm.cashBoxes.length < 1) {
                    vm.activateAlertModal("danger","Lo sentimos todas las cajas estan abiertas en este momento", true);
                } else {
                    vm.activateAlertModal("","", false);
                }
                $("#modal-caja").modal({ show: true });
            } else if (target.toLowerCase() === 'modal-product') {
                if (vm.searchSelected === '') {
                    vm.activateToastr('danger','Debe ingresar el codigo o titulo del libro',true);
                } else {
                    $.each(vm.books, function (index, book) {
                        if (vm.searchSelected.toString().toLowerCase() === book.Codigo.toString().toLowerCase()
                            || vm.searchSelected.toString().toLowerCase() === book.Titulo.toString().toLowerCase()) {
                            vm.product.item = book;
                            vm.product.quantity = 1;
                            vm.product.total = book.Precio;
                            vm.product.subtotal = book.PrecioSinImp;
                            $('#modal-product').modal('show');
                            return;
                        }
                    }); 
                }
                
            } else if (target.toLowerCase() === 'modal-factura') {
                vm.showFacturaModelNavbar = true;
                vm.modalFact.currentPage = 1;
                
                $("#modal-factura").modal({ show: true });
            } else if (target.toLowerCase() === 'modal-movimientos') {
                vm.obtenerMovimientos();
                
            } else if (target.toLowerCase() === 'modal-retiroabono') {
                vm.movimiento = { caja: {}, tipo: 0, monto: 0, razon: "", procesado: false };
                $("#modal-retiroAbono").modal({ show: true });
            } else if (target.toLowerCase() === 'modal-cierre') {
                vm.activateAlertModal("danger", "", false);
                vm.cierre.totalCreditos = 0;
                vm.cierre.totalRetiros = 0;
                $.each(vm.movimientosDelDia, function (index, mov) {
                    if (mov.Tipo.Codigo === 1 || mov.Tipo.Codigo === 4 || mov.Tipo.Codigo === 5 || mov.Tipo.Codigo === 6) {
                        vm.cierre.totalCreditos += mov.Monto;
                    }
                    if (mov.Tipo.Codigo === 3) {
                        vm.cierre.totalRetiros += mov.Monto;
                    }
                })

                vm.cierre.total = vm.cierre.totalCreditos - vm.cierre.totalRetiros;
                $("#modal-cierre").modal({ show: true });
            }

            

        },

        validateModalCartFields: function () {
            if (vm.modalFact.currentPage === 1) {
                if (vm.factura.master.client.id !== "" && vm.factura.master.client.name !== "" && vm.factura.master.client.lastname !== ""
                    && vm.factura.master.client.phone !== "" && vm.factura.master.client.email !== "") {
                    vm.modalFact.currentPage += 1;
                } else {
                    vm.activateAlertModal("danger", "Debe ingresar los datos personales", true);
                }
            } else {
                vm.modalFact.currentPage += 1;
            }

        },

        updateModalDetailProdTotal: function () {
            vm.product.subtotal = (vm.product.item.PrecioSinImp * vm.product.quantity);
            vm.product.total = (vm.product.item.Precio * vm.product.quantity);
        },

        updateFacturaDetail:function(index){
  
            var detail = vm.factura.details[index];
            detail.total = (parseInt(detail.item.Precio) * parseInt(detail.quantity));
            detail.subtotal = (parseInt(detail.item.PrecioSinImp) * parseInt(detail.quantity));
            vm.factura.details[index] = detail;
            vm.updateFactura();
        },

        updateFactura: function () {
            vm.factura.master.total = 0;
            vm.factura.master.subtotal = 0;
            vm.factura.master.totalItems = 0;
            $.each(vm.factura.details, function (key, item) {
                vm.factura.master.total = (parseInt(vm.factura.master.total) + parseInt(item.total));
                vm.factura.master.subtotal = (parseInt(vm.factura.master.subtotal) + parseInt(item.subtotal));
                vm.factura.master.totalItems = parseInt(vm.factura.master.totalItems) + parseInt(item.quantity);
            });
            vm.factura.master.taxes = vm.factura.master.total - vm.factura.master.subtotal;
            
        },

        addToFactura: function (obj) {
            if (vm.factura.details && vm.factura.details.length > 0) {
                var flag = false;
                $.each(vm.factura.details, function (key, item) {
                    if (obj.item.Codigo === item.item.Codigo) {
                        item.quantity = (parseInt(item.quantity) + parseInt(obj.quantity));
                        item.total = (parseInt(item.item.Precio) * parseInt(item.quantity));
                        item.subtotal = (parseInt(item.item.PrecioSinImp) * parseInt(item.quantity));
                        vm.factura.details[key] = item;
                        flag = true;
                    }
                });
                if (flag === false) {
                    vm.factura.details.push(obj);
                }
            } else {
                 vm.factura.details.push(obj);   
            }
            
            vm.factura.master.total = (parseInt(vm.factura.master.total) + parseInt(vm.product.total));
            vm.factura.master.subtotal = (parseInt(vm.factura.master.subtotal) + parseInt(vm.product.subtotal));

            vm.factura.master.totalItems = parseInt(vm.factura.master.totalItems) + parseInt(vm.product.quantity);
            vm.factura.master.taxes = vm.factura.master.total - vm.factura.master.subtotal;
       

            vm.searchSelected = "";
            $('#modal-product').modal('hide');
            vm.product = { item: {}, quantity: 1, total: 0, subtotal: 0 };

            
        },
        
        removeFromFactura: function (index) {
        
            vm.factura.details.splice(index, 1);
            if (vm.factura.details === null || vm.factura.details.length < 1) {
                vm.factura.master.subtotal = 0;
                vm.factura.master.total = 0;
                vm.factura.master.taxes = 0;
            } else {
                vm.updateFactura();
            }
            
        },

        updateChange : function(){
            vm.factura.master.change = (parseInt(vm.factura.master.totalReceived) - parseInt(vm.factura.master.total));
        },

        processPayment: function () {
            if (vm.factura.master.tipoPago.Codigo === 1 && (vm.factura.master.totalReceived === 0 || vm.factura.master.totalReceived === "")) {
                vm.activateAlertModal('danger', 'Debe ingresar el monto recibido.', true);
                return false;
            }

            if (vm.factura.master.tipoPago.Codigo === 1 && (vm.factura.master.totalReceived < vm.factura.master.total)) {
                vm.activateAlertModal('danger', 'El monto recibido no puede ser menor al monto a pagar.', true);
                return false;
            }

            vm.modalFact.currentPage = 4;
            vm.$refs.spinner.show();

            //Set cliente
            var cliente = { Nombre1: "", Nombre2: "", Apellido1: "", Apellido2: "", Cedula: "", Telefono: "", Email: "" };
            cliente.Nombre1 = vm.factura.master.client.name;
            cliente.Apellido1 = vm.factura.master.client.lastname;
            cliente.Cedula = vm.factura.master.client.id;
            cliente.Telefono = vm.factura.master.client.phone;
            cliente.Email = vm.factura.master.client.email;
            //Set Productos
            var productos = [];
            $.each(vm.factura.details, function (index, detail) {
                productos.push({ Codigo: detail.item.Codigo, Cantidad: detail.quantity });
            });

            
            var factura = { Caja: vm.cashier.Codigo, Cliente: cliente, Productos: productos, TipoPago: vm.factura.master.tipoPago, referencia: vm.factura.master.referencia };
            
            $.ajax({
                url: urlRoot + 'Facturacion/ProcesarCompra',
                type: 'post',
                dataType: 'json',
                data: factura,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.factura.master.id = result.Factura;
                        vm.modalFact.currentPage = 3;
                        vm.showFacturaModelNavbar = false;
                        vm.actualizarMovimientos();

                    } else {
                        if (result.Agotados !== null || result.Agotados.length > 0) {
                            var lista = "";
                            $.each(result.Agotados, function (index, object) {
                                lista += object.Titulo + " Existencia: " + object.Existencia + ", ";
                            });
                            vm.activateAlertModal("danger", "Los siguientes libros no estan disponibles: " + lista + ".", true);
                            vm.modalFact.currentPage = 2;
                        }
                        //vm.activateAlertModal('danger', 'No se proceso la factura, por favor intente nuevamente.', true);
                        //vm.modalFact.currentPage = 2;
                    }
                    
                    vm.$refs.spinner.hide();
                },
                error: function (error) {
                    vm.activateAlertModal('danger', 'No se proceso la factura, por favor intente nuevamente.', true);
                    vm.modalFact.currentPage = 2;
                    vm.$refs.spinner.hide();
                    
                }
            });

        },
       
        modalFacturaClose: function () {
            if (vm.factura !== null && vm.factura.master.id > 0) {
                vm.factura = { master: { id: "", client: { id: "", name: '',lastname:"",phone:"",email:"" },tipoPago:{Codigo:0,Descripcion:""},referencia:"", taxes: 0,subtotal:0, total: 0, totalReceived: 0, change: 0, totalItems: 0, date: '00/00/0000' }, details: [] };
            }
        },

        getClient: function () {

            if (vm.factura.master.client.id === "") {
                vm.activateAlertModal("danger", "Debe digitar la cedula", true);
                return false;
            }
            var cliente = { Nombre1: "", Nombre2: "", Apellido1: "", Apellido2: "", Cedula: vm.factura.master.client.id };
            vm.$refs.spinner.show();


            $.ajax({
                url: urlRoot + 'Home/BuscarCedula',
                type: 'post',
                dataType: 'json',
                data: cliente,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.factura.master.client.name = result.Usuario.Nombre;
                        vm.factura.master.client.lastname = result.Usuario.Apellidos;
                        vm.factura.master.client.id = result.Usuario.Cedula;
                        vm.factura.master.client.phone = result.Usuario.Telefono;
                        vm.factura.master.client.email = result.Usuario.Correo;
                    } else {
                        vm.activateAlertModal('info', 'No se encontro ningun usuario registrado, por favor digite sus datos', true);
                    }
                    vm.$refs.spinner.hide();
                },
                error: function (error) {
                    vm.activateAlertModal('info', 'No se pudo encontrar el cliente, por favor digite sus datos', true);
                    vm.$refs.spinner.hide();
                }
            });

        },

        init: function () {
            vm.displaySpinner(true, 'Obteniendo información de la base de datos, por favor espere!');
            vm.activateAlert('danger', '', false);
            vm.getInitData();
            //$('#collapseFactMain').collapse('show');
            //$("#modal-factura").modal({ show: true });
        }

        },
    
        //Define Computes
        computed: {

        },
        filters: {
        }

    
        }

);

vm.init( );







