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
data.codigos = ['123','233','234','543','23'];
data.titulos = ['Alabama', 'Alaska', 'Arizona','bolivia'];
data.searchByData = data.codigos;
data.rol = { Codigo: 0, Descripcion: '', Usuario : null, Estado:0 };
data.roles = [];
data.modalObject = { Codigo: 0, Descripcion: '', Usuario: null ,Rol:0};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.asideWiki = { show: false, title: '' };
data.validations = { activateFieldValidations:false, showSpinner: false, loadingMessage : 'Cargando datos de la base de datos, por favor espere! ...' };
data.sortKey = 'deviceName';
data.reverse = false;
data.search = '';
data.showEnableButtons = false;
data.showSearchTab = false;
data.columns = ['deviceName', 'devicePort'];
data.searchItem = '';
data.items = [];

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
        typeahead : VueStrap.typeahead,
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

        lowerCase: function (stringValue) {
            return stringValue.toLowerCase();
        },

        addRol: function (rol) {
            vm.displaySpinner(true,'Agregando Rol');
            $.ajax({
                url: urlRoot + 'MantRoles/Add',
                type: 'post',
                dataType: 'json',
                data: rol,
                success: function (result) {
                    if (result.OperationStatus) {
                        vm.getRoles();
                        vm.activateAlert('success', 'La operacion se completo de manera exitosa.', true);
                        
                    } else {
                        vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                    } 
                    vm.displaySpinner(false);
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
            });
        },

        getRoles: function () {
            $.ajax({
                url: urlRoot + 'MantRoles/GetAll',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (result) {
                    if (result.OperationStatus) {
                        vm.roles = result.Roles;
                    } else {
                        // window.location.href = result.Url;
                    }
                    vm.displaySpinner(false,'');
                },
                error: function (error) {
                    vm.displaySpinner(false,'');
                }
            });
              
            
        },

        getRol: function (rol) {
            this.displaySpinner(true);
            $.ajax({
                url: urlRoot + 'MantRoles/GetAll',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (result) {
                    if (result.OperationStatus) {
                        vm.roles = result.Roles;
                    } else {
                        window.location.href = result.Url;
                    }
                    this.displaySpinner(false);
                },
                error: function (error) {
                    vm.displaySpinner(false);
                }
            });


        },

        updateRol: function (rol) {

            $("#edit-modal").modal('hide' );
            vm.displaySpinner(true, 'Editando Rol');
            $.ajax({
                url: urlRoot + 'MantRoles/Update',
                type: 'post',
                dataType: 'json',
                data: rol,
                success: function (result) {
                    if (result.OperationStatus) {
                        vm.getRoles();
                        vm.activateAlert('success', 'La operacion se completo de manera exitosa.', true);

                    } else {
                        vm.activateAlertModal('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                    }

                    vm.displaySpinner(false);
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateAlertModal('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
                
            });

        },

        deleteRol: function (rol) {
            vm.displaySpinner(true, 'Desabilitando Rol');
            $.ajax({
                url: urlRoot + 'MantRoles/Delete',
                type: 'post',
                dataType: 'json',
                data: rol,
                success: function (result) {
                    if (result.OperationStatus) {
                        vm.getRoles();
                        vm.activateAlert('success', 'La operacion se completo de manera exitosa.', true);
                        
                    } else {
                        vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                    } 
                    vm.displaySpinner(false);
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateAlert('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
            });


        },

        openEditModal: function (rol) {
            vm.modalObject = rol;
            $("#edit-modal").modal({show:true});
        },
     
        createWiki: function (title, html) {
            //clean before append new content
            $("#divaside").html("");
            $("#divaside").prepend(html);
            vm.asideWiki.title = title
            vm.asideWiki.show = true;
        },

        showWikiSection1: function () {
            var html = "<p>aqui va el codigo html</p>";
            var title = "SIGELIBMA - Assistant";
            this.createWiki(title, html);
        },

        init: function () {
            vm.displaySpinner(true, 'Obteniendo informacion de la base de datos, por favor espere!');
            vm.getRoles();
            vm.activateAlert('danger', '', false);
        }

        },
    
        //Define Computes
        computed: {

        },
        filters: {
        }

    
        }

);

vm.init();







