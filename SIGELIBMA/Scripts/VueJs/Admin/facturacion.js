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
data.product = { item: {}, quantity: 1,total:0 };
data.modalObject = { Codigo: 0, Descripcion: '', Usuario: null, Rol: 0 };
data.cashBox = {id:0,status:false};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = { show: false, placement: "top-right", duration: "3000", type: "danger", width: "400px", dismissable: true, message: '' };
data.session = { id: 1, user: {name : 'steven aguilar', username : 'saguilar'}};
data.factura = { master: {}, details: [], taxes: 0, total:0, totalItems:0 };
data.validations = {showSpinner : false, loadingMessage : 'Cargando informacion de la base de datos, por favor espere.'};

Vue.filter('numeral', function (value) {
    return numeral(value).format('0,0');
})

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

        //changeSearch: function (search) {
        //    if (search === 1) {
        //        vm.searchByData = vm.codigos;
        //    } else {
        //        vm.searchByData = vm.titulos;
        //    }
        //},
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
            var param = 'test';
            $.ajax({
                url: urlRoot + 'Facturacion/OpenCashBox',
                type: 'post',
                dataType: 'json',
                data: param,
                success: function (result) {
                    vm.$refs.spinner1.hide();
                    if (result.OperationStatus) {
                        $('#modal-caja').modal('hide');
                        vm.activateToastr('success', 'La caja ha sido inicializada.', true);
                        $('#collapseFactMain').collapse('show');
                        
                        vm.cashBox.id = 1;
                        vm.cashBox.status = true;
                    } else {
                        vm.activateAlertModal('danger','La caja no se inicializo, trate nuevamente.',true);
                        vm.cashBox = null;
                    }

                },
                error: function (error) {
                    vm.$refs.spinner1.hide();
                    vm.activateAlertModal('danger', 'La caja no se inicializo, trate nuevamente.', true);
                    vm.cashBox = null;
                }
            });

        },

        closeCashBox: function () {
            $('#collapseFactMain').collapse('hide');
            vm.cashBox.status = false;
        },

        getInitData: function () {
            $.ajax({
                url: urlRoot + 'facturacion/Init',
                type: 'get',
                dataType: 'json',
                success: function (result) {
                    if (result.OperationStatus) {
                        vm.books = result.Books;
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
            if (target.toLowerCase() === 'modal-caja') {
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
            vm.product.total = (vm.product.item.Precio * vm.product.quantity);
        },

        addToFactura: function (obj) {
            if (vm.factura.details && vm.factura.details.length > 0) {
                var flag = false;
                $.each(vm.factura.details, function (key, item) {
                    if (obj.item.Codigo === item.item.Codigo) {
                        item.quantity = (parseInt(item.quantity) + parseInt(obj.quantity));
                        item.total = (parseInt(item.total) + parseInt(obj.total));
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
            
            vm.factura.total = (parseInt(vm.factura.total) + parseInt(vm.product.total));
            vm.factura.totalItems = parseInt(vm.factura.totalItems) + parseInt(vm.product.quantity);
            vm.factura.taxes = vm.factura.taxes + (vm.product.total / 13 * 100);
            
            vm.searchSelected = "";
            $('#modal-product').modal('hide');
            vm.product = { item: {}, quantity: 1, total: 0 };
            
        },
        
        removeFromFactura: function (index) {
        
            vm.factura.details.splice(index, 1);
            
        },

        //deleteRol: function (rol) {
        //    vm.displaySpinner(true, 'Desabilitando Rol');
        //    $.ajax({
        //        url: urlRoot + 'MantRoles/Delete',
        //        type: 'post',
        //        dataType: 'json',
        //        data: rol,
        //        success: function (result) {
        //            if (result.OperationStatus) {
        //                vm.getRoles();
        //                vm.activateAlert('success', 'La operacion se completo de manera exitosa.', true);
                        
        //            } else {
        //                vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
        //            } 
        //            vm.displaySpinner(false);
        //        },
        //        error: function (error) {
        //            vm.displaySpinner(false);
        //            vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
        //        }
        //    });


        //},

       
     
        init: function () {
            vm.displaySpinner(true, 'Obteniendo informacion de la base de datos, por favor espere!');
            vm.activateAlert('danger', '', false);
            vm.getInitData();
            $('#collapseFactMain').collapse('show')
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







