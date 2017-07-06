






function move_right(index) {
    document.getElementById('mainbookcontainer' + index).scrollLeft += 90;
}

function move_left(index) {
    document.getElementById('mainbookcontainer'+index).scrollLeft -= 90;
}


function updateArrows() {
    $(".category").each(function (key, value) {
        var row = $(this).find('.category-body');
        if ($(row).prop('scrollWidth') > $(row).width()) {
            $(this).find('.scrollBtn').show();
            $(this).find('.scrollBtn').show();
        } else {
            $(this).find('.scrollBtn').hide();
            $(this).find('.scrollBtn').hide();
        }
    });

    
}

//prod
var urlRoot = '';
//Dev
//var urlRoot = '';


var data = {};
data.books = [];
data.modalObject = {Quantity:1,Total:0};
data.categories = [];
data.shoppingCart = { items: [], total: 0 };
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true }; 
data.alertModalShoppingCart = { type: 'success', message: 'alert', status: true };
data.alertModalBookDetails = { type: 'success', message: 'alert', status: true };
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

activateAlertModalBookDetails: function (type, message, status) {
    vm.alertModalBookDetails.type = type;
    vm.alertModalBookDetails.message = message;
    vm.alertModalBookDetails.status = status;
},

activateAlertModalShoppingCart: function (type, message, status) {
    vm.alertModalShoppingCart.type = type;
    vm.alertModalShoppingCart.message = message;
    vm.alertModalShoppingCart.status = status;
},

        

lowerCase: function (stringValue) {
    return stringValue.toLowerCase();
},

scrollRight: function () {
    window.scrollBy(100, 0);
},


//addRol: function (rol) {
//    vm.displaySpinner(true,'Agregando Rol');
//    $.ajax({
//        url: urlRoot + 'MantRoles/Add',
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
            vm.hideShowArrowsOnLoad();
            vm.displaySpinner(false,'');
        },
        error: function (error) {
            vm.displaySpinner(false,'');
        }
    });
              
            
},

openModal: function (object, type) {
    if (type === 'bookDetails') {
        vm.activateAlertModalBookDetails('success', '', false);
        vm.modalObject = object;
        vm.modalObject.Quantity = 1;
        vm.modalObject.Total = 0;
        vm.updateModalDetailTotal();
        $("#modalDetails").modal({ show: true });
    } else if (type === 'modalShoppingCart') {
        vm.activateAlertModalShoppingCart('success', '', false);
        $("#modalShoppingCart").modal({ show: true });
    }
    
},
     
createWiki: function (title, html) {
    //clean before append new content
    $("#divaside").html("");
    $("#divaside").prepend(html);
    vm.asideWiki.title = title
    vm.asideWiki.show = true;
},
hideShowArrowsOnLoad: function() {
    $(".category").each(function (key, value) {
        var row = $(this).find('.category-body');
        if ($(row).prop('scrollWidth') > $(row).width()) {
            $(this).find('.scrollBtn').show();
            $(this).find('.scrollBtn').show();
        } else {
            $(this).find('.scrollBtn').hide();
            $(this).find('.scrollBtn').hide();
        }
    });
},
 
removeFromCart: function (index,item) {
    if (item !== null && item != undefined && item.Precio !== '') {

            vm.shoppingCart.items.splice(index, 1);
            vm.shoppingCart.total -= item.Precio;
 
        
        vm.activateAlertModalBookDetails('success','El producto fue eliminado con exito del carrito', true);
    } else {
        vm.activateAlertModalBookDetails('danger', 'Hemos encontrado un problema para eliminar el producto del carrito,por favor intente de nuevo', true);
    }
    
},

addToCart: function (item) {
    if (item !== null && item != undefined && item.Precio !== '') {
        for (var i = 1; i <= item.Quantity; i++) {
            vm.shoppingCart.items.push(item);
            vm.shoppingCart.total += item.Precio;
        };
        vm.activateAlertModalBookDetails('success', 'El producto fue agregado con exito al carrito', true);
    } else {
        vm.activateAlertModalBookDetails('danger', 'Hemos encontrado un problema para agregar el producto al carrito,por favor intente de nuevo', true);
    }
},

updateModalDetailTotal: function () {
    
    vm.modalObject.Total = (vm.modalObject.Precio * vm.modalObject.Quantity);
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





