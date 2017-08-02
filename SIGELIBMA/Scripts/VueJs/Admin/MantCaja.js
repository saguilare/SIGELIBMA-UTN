/*********************************
***          Attention
*** some components used in this 
*** tool such as datepicker come  
*** from vuestrap.js and need vue.js
*** version lower than 2.0
*********************************/

//prod
var urlRoot = '';
//Dev
//var urlRoot = '';


var data = {};
data.tipo = { codigo: 0, descripcion:"", estado:0 };
data.tipos = [];
data.estados =[];
data.modalObject = { codigo: 0, descripcion:"", estado:0};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = {show : false, placement: "top-right", duration: "3000", type :"danger" ,width:"400px", dismissable:true,message:''};
data.validations = { activateFieldValidations:false, showSpinner: false, loadingMessage : 'Cargando datos de la base de datos, por favor espere! ...' };

data.sortKey = 'codigo';
data.reverse = 1;
data.search = '';
data.items = [];
data.searchItem = '';
data.paginatedItems = [];
data.selectedItems = [];
data.pagination = { range: 5, currentPage: 1, itemPerPage: 8, items: [], filteredItems: [] };

Vue.filter('numeral', function (value) {
    return numeral(value).format('0,0');
})

var vm = new Vue({
    el: '#pageMainContainer',
    data: data,
    components: {
        alert: VueStrap.alert,
        datepicker: VueStrap.datepicker,
        spinner: VueStrap.spinner,
        typeahead: VueStrap.typeahead,
        vueinput: VueStrap.input

    },

    //Define methods
    methods: {

        //PaginationMethods

        clearSearchItem : function() {
            this.searchItem = undefined
            this.searchInTheList('')
        },

        setSort: function(sortkey, reverse){
            vm.sortKey = sortkey;
            vm.reverse = reverse;
        },

        searchInTheList : function(searchText, currentPage) {
            if (_.isUndefined(searchText)) {
                this.filteredItems = _.filter(vm.items, function (v, k) {
                    return !v.selected
                })
            }
            else {
                this.filteredItems = _.filter(vm.items, function (v, k) {
                    if ((v.codigo!=null)&&(v.descripcion!=null)&&(v.estado!=null)) {
                        return (!v.selected && v.codigo.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1) 
                            | (!v.selected && v.descripcion.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                        | (!v.selected && v.estado.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                    } else {
                        if (v.codigo != null) {
                            return !v.selected && v.codigo.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1
                        } else {
                            if (v.nombre != null) {
                                return !v.selected && v.nombre.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                            }
                        }
                    }
                    
                })
            }
            this.filteredItems.forEach(function (v, k) {
                v.key = k + 1
            })
            this.buildPagination()

            if (_.isUndefined(currentPage)) {
                this.selectPage(1)
            }
            else {
                this.selectPage(currentPage)
            }
        },

        buildPagination : function() {
            let numberOfPage = Math.ceil(this.filteredItems.length / this.pagination.itemPerPage)
            this.pagination.items = []
            for (var i = 0; i < numberOfPage; i++) {
                this.pagination.items.push(i + 1)
            }
        },

        selectPage : function(item) {
            this.pagination.currentPage = item

            let start = 0
            let end = 0
            if (this.pagination.currentPage < this.pagination.range - 2) {
                start = 1
                end = start + this.pagination.range - 1
            }
            else if (this.pagination.currentPage <= this.pagination.items.length && this.pagination.currentPage > this.pagination.items.length - this.pagination.range + 2) {
                start = this.pagination.items.length - this.pagination.range + 1
                end = this.pagination.items.length
            }
            else {
                start = this.pagination.currentPage - 2
                end = this.pagination.currentPage + 2
            }
            if (start < 1) {
                start = 1
            }
            if (end > this.pagination.items.length) {
                end = this.pagination.items.length
            }

            this.pagination.filteredItems = []
            for (var i = start; i <= end; i++) {
                this.pagination.filteredItems.push(i);
            }

            this.paginatedItems = this.filteredItems.filter((v, k) => {
                return Math.ceil((k + 1) / this.pagination.itemPerPage) == this.pagination.currentPage
            })
        },

        changePagination: function () {
            vm.buildPagination();
            vm.selectPage(1);
        },

        //EndPafinationMothods


        displaySpinner: function (status, message) {
            vm.validations.showSpinner = status;
            vm.validations.loadingMessage = message;
        },

        activateAlert: function (type, message, status) {
            vm.alert.type = type;
            vm.alert.message = message;
            vm.alert.status = status;
        },

        activateAlertModal: function (type, message, status) {
            vm.alertModal.type = type;
            vm.alertModal.message = message;
            vm.alertModal.status = status;
        },

        activateToastr: function (type,message,status) {
            vm.toastr.show = status;
            vm.toastr.placement = 'top-right';
            vm.toastr.duration = 10000;
            vm.toastr.type = type;
            vm.toastr.width = '300px';
            vm.toastr.dismissable = true;
            vm.toastr.message = message;
        },

        lowerCase: function (stringValue) {
            return stringValue.toLowerCase();
        },

        getInitData: function () {
            $.ajax({
                url: urlRoot + 'mantcaja/ObtenerInitData',
                type: 'get',
                dataType: 'json',
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.tipos = result.Cajas;
                        vm.estados = result.Estados;
                        vm.items = vm.tipos;
                        vm.filteredItems = vm.tipos;
                        vm.buildPagination();
                        vm.selectPage(1);
                    } else {
                        vm.activateToastr('danger','Ha ocurrido un problema, por favor recargue la pagina.',true);
                    }
                    vm.displaySpinner(false,'');
                },
                error: function (error) {
                    vm.activateAlert('danger','Ha ocurrido un problema, por favor recargue la pagina.',true);
                    vm.displaySpinner(false,'');
                }
            });
              
            
        },

        getTipos: function () {
            $.ajax({
                url: urlRoot + 'mantcaja/ObtenerTodos',
                type: 'get',
                dataType: 'json',
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.tipos = result.Cajas;
                        vm.items = vm.tipos;
                        vm.filteredItems = vm.tipos;
                        vm.buildPagination();
                        vm.selectPage(1);
                        vm.activateToastr('success', 'La operacion se completo de manera exitosa.', true);
                    } else {
                        vm.activateToastr('danger','Ha ocurrido un problema, por favor recargue la pagina.',true);
                    }
                    vm.displaySpinner(false,'');
                },
                error: function (error) {
                    vm.activateToastr('danger','Ha ocurrido un problema, por favor recargue la pagina.',true);
                    vm.displaySpinner(false,'');
                }
            });
              
            
        },

        agregar: function () {
            vm.displaySpinner(true,'Agregando Tipo');
            $.ajax({
                url: urlRoot + 'mantcaja/Agregar',
                type: 'post',
                dataType: 'json',
                data: vm.tipo,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getTipos();     
                        vm.tipo = { codigo: 0, descripcion:"", estado:0 };
                    } else {
                        vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                        vm.displaySpinner(false);
                    }          
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
            });
        },

        modificar: function () {
            $("#edit-modal").modal('hide' );
            vm.displaySpinner(true, 'Editando Tipo');
            $.ajax({
                url: urlRoot + 'mantcaja/Modificar',
                type: 'post',
                dataType: 'json',
                data: vm.modalObject,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getTipos();
                    } else {
                        vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                        vm.displaySpinner(false);
                    }   
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
                
            });

        },

        eliminar: function (tipo) {
            vm.displaySpinner(true, 'Desabilitando Tipo');
            tipo.estado = 2;
            $.ajax({
                url: urlRoot + 'mantcaja/Desabilitar',
                type: 'post',
                dataType: 'json',
                data: tipo,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getTipos();    
                    } else {
                        vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                        vm.displaySpinner(false);
                    } 
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
            });


        },

        openEditModal: function (tipo) {
            vm.activateAlertModal('','',false);
            vm.modalObject = tipo;
            $("#edit-modal").modal({show:true});
        },
     
        init: function () {
            vm.displaySpinner(true, 'Obteniendo informacion de la base de datos, por favor espere!');
            vm.getInitData();
            vm.activateAlert('danger', '', false);
        }

    },
    
    //Define Computes
    computed: {

    },
    filters: {
    }

    
});//close vue instance

vm.init();







