﻿






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
data.emailSupport = "iglesiamana@gmail.com";
data.datepickerOptions = { format: 'dd/MM/yyyy' ,placeholder:'dd/mm/yyyy', close:true, value:''};
data.books = [];
data.modalObject = { item: {}, quantity: 1, total: 0 };
data.enableProccedBtn = false;
data.categories = [];
data.shoppingCart = { items: [], total: 0, totalItems: 0, sections: [], payment: {} };
data.shoppingCart.payment = { code: '',status: false };
data.toastr = {show : false, placement: "top-right", duration: "3000", type :"danger" ,width:"400px", dismissable:true,message:''};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.modalCart = { currentPage : 0};
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
        alert: VueStrap.alert,
        datepicker: VueStrap.datepicker,
        spinner: VueStrap.spinner,
        //typeahead: customAutocomplete,
        
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

activateToastr: function (type,message,status) {
    vm.toastr.show = status;
    vm.toastr.placement = 'top-right';
    vm.toastr.duration = 10000;
    vm.toastr.type = type;
    vm.toastr.width = '300px';
    vm.toastr.dismissable = true;
    vm.toastr.message = message;
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
        vm.modalObject = { item: {}, quantity: 1, total: 0 };
        vm.modalObject.item = object;
        vm.updateModalDetailTotal();
        $("#modalDetails").modal({ show: true });
    } else if (type === 'modalShoppingCart') {
        vm.modalCart.currentPage = 1;
        if (vm.shoppingCart.items.length <= 0) {
            vm.activateToastr('danger', 'El carrito esta vacio.', true);
            //vm.activateAlertModalShoppingCart('info', ' El carrito esta vacio', true);
            //vm.enableProccedBtn = false;
            //$("#modalShoppingCart").modal({ show: true });
        } else {
            
            vm.activateAlertModalShoppingCart('success', '', false);
            $("#modalShoppingCart").modal({ show: true });
            vm.enableProccedBtn = true;
            
        }
        
        
    }
    
},

processPayment: function () {
    vm.modalCart.currentPage = 4;
    this.$refs.spinner1.show();
    
    var param = 'test';
    $.ajax({
        url: urlRoot + 'Home/ProcessPayment',
        type: 'post',
        dataType: 'json',
        data: param,
        success: function (result) {
            if (result.OperationStatus) {
                vm.shoppingCart.payment.code = result.ConfirmationCode;
                vm.shoppingCart.payment.status = true;
            } 
            vm.$refs.spinner1.hide();
        },
        error: function (error) {
            vm.$refs.spinner1.hide();
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
 
removeFromCart: function (index, object) {
    if (object !== null && object != undefined && object.item !== null) {
            vm.shoppingCart.items.splice(index, 1);
            vm.shoppingCart.total -= object.total;
            vm.shoppingCart.totalItems -= object.quantity;
            vm.activateAlertModalBookDetails('success','El producto fue eliminado con exito del carrito', true);
    } else {
        vm.activateAlertModalBookDetails('danger', 'Hemos encontrado un problema para eliminar el producto del carrito,por favor intente de nuevo', true);
    }
    
},

addToCart: function (item) {
    if (item !== null && item != undefined && item.Precio !== '') {
        vm.shoppingCart.items.push(item);
        vm.shoppingCart.total += item.total;
        vm.shoppingCart.totalItems += item.quantity;
        vm.activateAlertModalBookDetails('success', 'El producto fue agregado con exito al carrito', true);
    } else {
        vm.activateAlertModalBookDetails('danger', 'Hemos encontrado un problema para agregar el producto al carrito,por favor intente de nuevo', true);
    }
},

updateModalDetailTotal: function () {
    vm.modalObject.total = (vm.modalObject.item.Precio * vm.modalObject.quantity);
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





