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
data.datepickerOptions = { format: 'MM/dd/yyyy', placeholder: 'mm/dd/yyyy', close: true };
data.categorias= [{ Codigo: 10, Descripcion: 'Test', Estado: 0 }, { Codigo: 15, Descripcion: 'Tes2', Estado: 0 }],
data.autores = [];
data.proveedores = [];
data.libro = { Codigo: 1, Titulo: '', Descripcion: '', Fecha: '00/00/0000', categoria: { Codigo: 0, Descripcion: '', Estado: 0 }, autor: { Codigo: 0, Nombre: '', Apellidos: '', Estado: 0 }, proveedor: { Codigo: 0, Nombre: '', Telefono: '', Correo: '', Estado: 0 }, PrecioBase: 0, ProcetajeGanancia: 0, PrecioVentaSinImpuestos: 0, PrecioVentaConImpuestos: 0, Imagen: '', Estado: 0 };
data.libros = [];
data.modalObject = { Codigo: 0, Titulo: '', Descripcion: ''};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = { show: false, placement: "top-right", duration: "3000", type: "danger", width: "400px", dismissable: true, message: '' };
data.validations = { activateFieldValidations: false, showSpinner: false, loadingMessage: 'Cargando datos de la base de datos, por favor espere! ...' };
data.validarFechaIngreso = { date: false};
data.modalAccion = 'Agregar Libro';
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

    methods: {

        openEditModal: function (libro) {
        vm.activateAlertModal('', '', false);
        vm.modalObject = libro;
        $("#edit-modal").modal({ show: true });
        },

        guardarLibro: function (libro) {
            $("#edit-modal").modal('hide');
            vm.displaySpinner(true, 'Guarando Rol');
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

    activateToastr: function (type, message, status) {
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

    init: function () {
       
    }

    },

    //Define Computes
    computed: {

    },
    filters: {
    }


});//close vue instance

vm.init();

