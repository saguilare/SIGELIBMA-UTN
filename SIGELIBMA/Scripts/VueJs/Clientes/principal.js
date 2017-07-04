function move_right(index) {
    document.getElementById('mainbookcontainer' + index).scrollLeft += 90;
}

function move_left(index) {
    document.getElementById('mainbookcontainer'+index).scrollLeft -= 90;
}


function showArrows(index) {
    if ($('#mainbookcontainer' + index).prop('scrollWidth') > $('#mainbookcontainer' + index).width()) {
        $('#scrollleft' + index).show();
        $('#scrollRight' + index).show();
        
    } else {
        $('#scrollleft' + index).hide();
        $('#scrollRight' + index).hide();
    }
}

//prod
var urlRoot = '';
//Dev
//var urlRoot = '';


var data = {};
data.books = [];
data.categories = [];
data.modalObject = { Codigo: 0, Descripcion: '', Usuario: null ,Rol:0};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.asideWiki = { show: false, title: '' };
data.validations = { activateFieldValidations:false, showSpinner: false, loadingMessage : 'Cargando datos de la base de datos, por favor espere! ...' };


Vue.filter('numeral', function (value) {
    return numeral(value).format('0,0');
});

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

scrollRight: function () {
    window.scrollBy(100, 0);
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

getPageData: function () {
    $.ajax({
        url: urlRoot + 'Home/Init',
        type: 'get',
        dataType: 'json',
        success: function (result) {
            if (result.OperationStatus) {
                vm.books = result.Books;
                vm.categories = result.Categories;
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
    vm.activateAlert('danger', '', false);
    vm.getPageData();
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







