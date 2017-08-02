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
data.inventarios = [];
data.libros = [];
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = {show : false, placement: "top-right", duration: "3000", type :"danger" ,width:"400px", dismissable:true,message:''};
data.validations = { activateFieldValidations:false, showSpinner: false, loadingMessage : 'Cargando datos de la base de datos, por favor espere! ...' };
data.editmodal= {currentPage : 1};

data.modalObject ={};
data.estados = [];

data.sortKey = 'libro.codigo';
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
                    console.log(v,searchText);
                    if ((v.libro.codigo!=null)&&(v.libro.titulo!=null)&&(v.estado.descripcion!=null)
                        &&(v.stock!=null)&&(v.maximo!=null)&&(v.minimo!=null)
                        ) {
                        return 
                        ( !v.selected && v.libro.codigo.toLowerCase().indexOf(searchText.toLowerCase()) > -1) 
                        | (!v.selected && v.libro.titulo.toLowerCase().indexOf(searchText.toLowerCase()) > -1) 
                        | (!v.selected && v.estado.descripcion.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                        | (!v.selected && v.stock.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                        | (!v.selected && v.minimo.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                        | (!v.selected && v.maximo.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                    } 
 
                    
                });
            }
            console.log(this.filteredItems);
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
                url: urlRoot + 'inventario/ObtenerInitData',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.estados = result.Estados;
                        vm.libros = result.Libros;
                        vm.inventarios = result.Inventarios;
                        vm.items = vm.inventarios;
                        vm.filteredItems = vm.inventarios;
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

        getInventarios: function () {
            $.ajax({
                url: urlRoot + 'inventario/ObtenerInventarios',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.inventarios = result.Inventarios;
                        vm.items = vm.inventarios;
                        vm.filteredItems = vm.inventarios;
                        vm.buildPagination();
                        vm.selectPage(1);
                        vm.activateToastr('success', 'La lista de inventario fue actualizada con exito.', true);
                    } else {
                        vm.activateToastr('danger','Ha ocurrido un problema actualizando la lista de inventario, por favor recargue la pagina.',true);
                    }
                
                },
                error: function (error) {
                    vm.activateToastr('danger','Ha ocurrido un problema actualizando la lista de inventario, por favor recargue la pagina.',true);
                }
            });
              
            
        },

        modificar: function () {
            this.$refs.spinner1.show();
            var inv = {Libro: vm.modalObject.libro.codigo ,Stock: vm.modalObject.stock,Maximo: vm.modalObject.maximo,Minimo: vm.modalObject.minimo,Estado: vm.modalObject.estado.codigo}; 
            $.ajax({
                url: urlRoot + 'inventario/Modificar',
                type: 'post',
                dataType: 'json',
                data: inv,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getInventarios();
                        $("#edit-modal").modal('hide' );
                        vm.activateToastr('success', 'La operacion se realizo con exito.', true);

                    } else {
                        vm.activateAlertModal("danger","Ha ocurrido un error, intente nuevamente", true);  
                    }   
                    vm.$refs.spinner1.hide();
                },
                error: function (error) {
                    vm.$refs.spinner1.hide();
                    vm.activateAlertModal("danger","Ha ocurrido un error, intente nuevamente", true);  
                }
                
            });

        },

        agregar: function () {
            this.$refs.spinner2.show();
            var inv = {Libro: vm.modalObject.libro ,Stock: vm.modalObject.stock,Maximo: vm.modalObject.maximo,Minimo: vm.modalObject.minimo,Estado: vm.modalObject.estado}; 
            $.ajax({
                url: urlRoot + 'inventario/agregar',
                type: 'post',
                dataType: 'json',
                data: inv,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.getInventarios();
                        $("#agregar-modal").modal('hide' );
                        vm.activateToastr('success', 'La operacion se realizo con exito.', true);

                    } else {
                        vm.activateAlertModal("danger","Ha ocurrido un error, intente nuevamente", true);  
                    }   
                    vm.$refs.spinner2.hide();
                },
                error: function (error) {
                    vm.$refs.spinner2.hide();
                    vm.activateAlertModal("danger","Ha ocurrido un error, intente nuevamente", true);  
                }
                
            });

        },

        openEditModal: function (obj, modal) {
            if (modal.toLowerCase() === 'edit-modal') {
                vm.activateAlertModal('','',false);
                vm.modalObject = obj;
                $("#edit-modal").modal({show:true});
            }else {
                vm.activateAlertModal('','',false);
                vm.modalObject = {libro: vm.libros[1].codigo ,stock: 1,maximo: 1,minimo: 1,estado: vm.estados[1].codigo}; 
                $("#agregar-modal").modal({show:true});
            }
            
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







