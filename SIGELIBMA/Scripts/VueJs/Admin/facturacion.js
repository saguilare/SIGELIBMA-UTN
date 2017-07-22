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
data.emailSupport = "iglesiamana@gmail.com";
data.books = [];
data.codigos = [];
data.titulos = [];
data.searchByData = data.codigos;
data.searchSelected = "";
data.product = { item: {}, quantity: 1,subtotal:0,total:0 };
data.modalObject = { Codigo: 0, Descripcion: '', Usuario: null, Rol: 0 };
data.modalFact = { currentPage: 1 };
data.cashier = null;
data.cashBoxes = [];
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = { show: false, placement: "top-right", duration: "3000", type: "danger", width: "400px", dismissable: true, message: '' };
data.session = { id: 1, user: {name : 'steven aguilar', username : 'saguilar'}};
data.factura = { master: { id: 0, client: { id: 0, name: '' }, taxes: 0,subtotal:0, total: 0, totalReceived: 0, change: 0, totalItems: 0, date: '00/00/0000' }, details: [] };
data.validations = {showSpinner : false, loadingMessage : 'Cargando informacion de la base de datos, por favor espere.'};

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
        //typeahead: customAutocomplete,
        //datepicker: VueStrap.datepicker,
        //modal not working the second time
        //bug in vue-strap
        //using bootstrap modal instead
        //modal: VueStrap.modal,
        //vueStrapAside: VueStrap.aside,
        //popover: VueStrap.popover,

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
            this.$refs.spinner1.show();
            vm.cashier.Estado = 2;
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
                    } else {
                        vm.cashier.Estado = 1;
                        vm.activateAlertModal('danger','La caja no se inicializo, trate nuevamente.',true);
                    }
                },
                error: function (error) {
                    vm.$refs.spinner1.hide();
                    vm.cashier.Estado = 1;
                    vm.activateAlertModal('danger', 'La caja no se inicializo, trate nuevamente.', true);
             
                }
            });

        },

        closeCashBox: function () {
            this.$refs.spinner1.show();
            vm.cashier.Estado = 1;
            $.ajax({
                url: urlRoot + 'Facturacion/AbrirCerrarCaja',
                type: 'post',
                dataType: 'json',
                data: vm.cashier,
                success: function (result) {
                    vm.$refs.spinner1.hide();
                    if (result.EstadoOperacion) {
                        $('#modal-caja').modal('hide');
                        vm.activateToastr('success', 'La caja ha sido cerrada con exito.', true);
                        $('#collapseFactMain').collapse('hide');
                        vm.cashier = null;
                    } else {
                        vm.cashier.Estado = 2;
                        vm.activateAlertModal('danger', 'La caja no se cerro, trate nuevamente.', true);
                    }
                },
                error: function (error) {
                    vm.$refs.spinner1.hide();
                    vm.cashier.Estado = 2;
                    vm.activateAlertModal('danger', 'La caja no se cerro, trate nuevamente.', true);

                }
            });

        },


        getInitData: function () {
            $.ajax({
                url: urlRoot + 'facturacion/Init',
                type: 'get',
                dataType: 'json',
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.cashBoxes = result.Cajas;
                        vm.books = result.Libros;
                        if (vm.books !== null && vm.books !== undefined && vm.books.length > 0) {
                            $.each(vm.books,function (key, book) {
                                vm.codigos.push(book.Codigo.toString());
                                vm.titulos.push(book.Titulo);
                            });
                        }

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

        openModal: function (object, target) {
            vm.activateAlertModal('info','',false);
            if (target.toLowerCase() === 'modal-caja') {
                vm.cashier = {};
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
                $("#modal-factura").modal({ show: true });
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
            vm.product = { item: {}, quantity: 1, total: 0 ,subtotal:0};
            
        },
        
        removeFromFactura: function (index) {
        
            vm.factura.details.splice(index, 1);
            if (vm.factura.details === null || vm.factura.details.length < 1) {
                vm.factura.master.subtotal = 0;
                vm.factura.master.total = 0;
                vm.factura.master.taxes = 0;
            }
            
        },

        updateChange : function(){
            vm.factura.master.change = (parseInt(vm.factura.master.totalReceived) - parseInt(vm.factura.master.total));
        },

        processPayment: function () {
            vm.modalFact.currentPage = 4;
            this.$refs.spinner1.show();

            var param = 'test';
            $.ajax({
                url: urlRoot + 'Facturacion/ProcessPayment',
                type: 'post',
                dataType: 'json',
                data: param,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.factura.master.id = result.Factura;
                        vm.modalFact.currentPage = 3;
                    } else {
                        vm.activateAlertModal('danger', 'No se proceso la factura, por favor intente nuevamente.', true);
                        vm.modalFact.currentPage = 2;
                    }
                    
                    vm.$refs.spinner1.hide();
                },
                error: function (error) {
                    vm.$refs.spinner1.hide();
                    $('#modal-factura').modal('hide');
                    vm.activateToastr('danger', 'No se proceso la factura, por favor intente nuevamente.', true);
                }
            });

        },
       
     
        init: function () {
            vm.displaySpinner(true, 'Obteniendo informacion de la base de datos, por favor espere!');
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







