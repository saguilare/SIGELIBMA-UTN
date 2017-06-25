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
data.rol = { Codigo: 0, Descripcion: '', Usuario : null };
data.roles = [];
data.alert = { type: 'success', message: 'alert', status: false };
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

        clearSearchItem () {
            this.searchItem = undefined
            this.searchInTheList('')
        },

        searchInTheList(searchText, currentPage) {
            if (_.isUndefined(searchText)) {
                this.filteredItems = _.filter(items, function (v, k) {
                    return !v.selected
                })
            }
            else {
                this.filteredItems = _.filter(items, function (v, k) {
                    if ((v.deviceName!=null)&&(v.hostName!=null)) {
                        return (!v.selected && v.deviceName.toLowerCase().indexOf(searchText.toLowerCase()) > -1) | (!v.selected && v.hostName.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                    } else {
                        if (v.deviceName != null) {
                               return !v.selected && v.deviceName.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                        } else {
                            if (v.hostName != null) {
                                return !v.selected && v.hostName.toLowerCase().indexOf(searchText.toLowerCase()) > -1
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

        buildPagination() {
            let numberOfPage = Math.ceil(this.filteredItems.length / this.pagination.itemPerPage)
            this.pagination.items = []
            for (var i = 0; i < numberOfPage; i++) {
                this.pagination.items.push(i + 1)
            }
        },

        selectPage(item) {
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

        selectItem(item) {
            item.selected = true
            this.selectedItems.push(item)
            this.searchInTheList(this.searchItem, this.pagination.currentPage)
        },

        removeSelectedItem(item) {
            item.selected = false
            this.selectedItems.$remove(item)
            this.searchInTheList(this.searchItem, this.pagination.currentPage)
        },
        //EndPafinationMothods

        customFilter: function(device) {
            return device.deviceName.indexOf(this.search) != -1
            || device.hostName.indexOf(this.search) != -1
            ;
        },

        sortBy: function (sortKey) {
            this.reverse = (this.sortKey == sortKey) ? !this.reverse : false;

            this.sortKey = sortKey;
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
            vm.displaySpinner(true, 'Cargando datos de la base de datos, por favor espere!');
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
                    this.getRoles();
                    this.displaySpinner(false);
                },
                error: function (error) {
                    vm.displaySpinner(false);
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







