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
data.rol = { Codigo: 0, Descripcion: '', Usuario : null, Estado:0 };
data.roles = [];
data.modalObject = { Codigo: 0, Descripcion: '', Usuario: null ,Rol:0};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = {show : false, placement: "top-right", duration: "3000", type :"danger" ,width:"400px", dismissable:true,message:''};
data.validations = { activateFieldValidations:false, showSpinner: false, loadingMessage : 'Cargando datos de la base de datos, por favor espere! ...' };

data.sortKey = 'Codigo';
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
                    if ((v.Codigo!=null)&&(v.Descripcion!=null)) {
                        return (!v.selected && v.Codigo.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1) | (!v.selected && v.Descripcion.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                    } else {
                        if (v.Codigo != null) {
                            return !v.selected && v.Codigo.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1
                        } else {
                            if (v.Descripcion != null) {
                                return !v.selected && v.Descripcion.toLowerCase().indexOf(searchText.toLowerCase()) > -1
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
                    url: urlRoot + 'MantRoles/ObtenerInitData',
                    type: 'get',
                    dataType: 'json',
                    async: true,
                    success: function (result) {
                        if (result.EstadoOperacion) {
                            vm.roles = result.Roles;
                            vm.items = vm.roles;
                            vm.filteredItems = vm.roles;
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

        getRoles: function () {
            $.ajax({
                url: urlRoot + 'MantRoles/ObtenerTodos',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.roles = result.Roles;
                        vm.items = vm.roles;
                        vm.filteredItems = vm.roles;
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

        addRol: function (rol) {
            vm.displaySpinner(true,'Agregando Rol');
            $.ajax({
                url: urlRoot + 'MantRoles/Agregar',
                type: 'post',
                dataType: 'json',
                data: rol,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getRoles();                        
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

        updateRol: function (rol) {
            $("#edit-modal").modal('hide' );
            vm.displaySpinner(true, 'Editando Rol');
            $.ajax({
                url: urlRoot + 'MantRoles/Modificar',
                type: 'post',
                dataType: 'json',
                data: rol,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getRoles();
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

        deleteRol: function (rol) {
            vm.displaySpinner(true, 'Desabilitando Rol');
            rol.Estado = 0;
            $.ajax({
                url: urlRoot + 'MantRoles/Desabilitar',
                type: 'post',
                dataType: 'json',
                data: rol,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getRoles();    
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

        openEditModal: function (rol) {
            vm.activateAlertModal('','',false);
            vm.modalObject = rol;
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







