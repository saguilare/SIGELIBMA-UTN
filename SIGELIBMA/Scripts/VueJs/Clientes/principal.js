

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
data.codigos = [];
data.titulos = [];
data.searchByData = data.titulos;
data.searchSelected = "";
data.emailSupport = "libreriaimana@gmail.com";
data.datepickerOptions = { format: 'MM/dd/yyyy' ,placeholder:'mm/dd/yyyy', close:true};
data.books = [];
data.modalObject = { item: {}, quantity: 1, total: 0 };
data.enableProccedBtn = false;
data.categories = [];
data.filteredCategories = [];
data.categoryFilter = {};
data.shoppingCart = { items: [], total: 0, totalItems: 0, sections: [], payment: {} };
data.shoppingCart.payment = { code: '', status: false };
data.showModalShoppingCartNavBar = true;
data.toastr = {show : false, placement: "top-right", duration: "3000", type :"danger" ,width:"400px", dismissable:true,message:''};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.modalCart = { currentPage : 0, acceptance : false};
data.alertModalShoppingCart = { type: 'success', message: 'alert', status: true };
data.alertModalBookDetails = { type: 'success', message: 'alert', status: true };
data.cliente = { Nombre1: "", Nombre2: "", Apellido1: "", Apellido2: "", Cedula: "", Telefono: "", Email: "" };
data.deposito = { Fecha: "", Referencia: "", BancoEmisor: "", BancoReceptor: "", Descripcion: "" };
data.asideWiki = { show: false, title: '' };
data.validations = {  showSpinner: false, loadingMessage : 'Cargando datos de la base de datos, por favor espere! ...' };
data.shoppingCartValidations = { date: false, bancoEmisor: false, bancoReceptor: false };
data.date = "";
data.modalSpinnerText = "Cargando Datos";
data.quantity = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
data.cedulaStatus = true;
data.validateCedula = false;
data.disableBuyButton = false;

Vue.filter('numeral', function (value) {
    return numeral(value).format('0,0');
});

Vue.component('backtotop', {
    template: '<button id="" class="goTop pull-right" v-if="isVisible" v-on:click="backToTop"><span class="fa glyphicon glyphicon-chevron-up" aria-hidden="true"></span> </button>',
    data: function () {
        return {
            isVisible: true
        };
    },
    methods: {

        
        initToTopButton: function () {
            document.getElementById('#pageMainContainer').bind('scroll', function () {
                var backToTopButton = $('.goTop');
                if (document.getElementById('#pageMainContainer').scrollTop() > 250) {
                    backToTopButton.addClass('isVisible');
                    this.isVisible = true;
                } else {
                    backToTopButton.removeClass('isVisible');
                    this.isVisible = false;
                }
            }.bind(this));
        },
        backToTop: function () {
            $('html,body').stop().animate({
                scrollTop: 0
            }, 'slow', 'swing');
        }
    },
    mounted: function () {
        this.$nextTick(function () {
            this.initToTopButton();
        });
    }
});

var vm = new Vue({
    el: '#pageMainContainer',
    data: data,
    components: {
        alert: VueStrap.alert,
        datepicker: VueStrap.datepicker,
        spinner: VueStrap.spinner,
        typeahead: VueStrap.typeahead,
        vueinput: VueStrap.input
        //modal not working the second time
        //bug in vue-strap
        //using bootstrap modal instead
        //modal: VueStrap.modal,
        //vueStrapAside: VueStrap.aside,
        //popover: VueStrap.popover,

    },

    //Define methods
    methods: {


   cleanCart: function () {
            vm.shoppingCart = { items: [], total: 0, totalItems: 0, sections: [], payment: {} };
            vm.shoppingCart.payment = { code: '', status: false };
            vm.cliente = { Nombre1: "", Nombre2: "", Apellido1: "", Apellido2: "", Cedula: "", Telefono: "", Email: "" };
            vm.deposito = { Fecha: "", Referencia: "", BancoEmisor: "", BancoReceptor: "", Descripcion: "" };
            vm.shoppingCartValidations = { date: false, bancoEmisor: false, bancoReceptor: false };
            $('#modalShoppingCart').modal('hide');
        },


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

filterCategory: function (category) {
    
    vm.filteredCategories = [];
    vm.filteredCategories.push(category);
  
},

scrollRight: function () {
    window.scrollBy(100, 0);
},

changeSearch: function (search) {
    if (search === 1) {
        vm.searchByData = vm.codigos;
    } else {
        vm.searchByData = vm.titulos;
    }
},

getPageData: function () {
    $.ajax({
        url: urlRoot + 'Home/Init',
        type: 'get',
        dataType: 'json',
        success: function (result) {
            if (result.EstadoOperacion) {
                vm.categories = result.Categorias;
                vm.date = result.Date;
                $.each(vm.categories, function (index, cat) {
                    $.each(cat.Libros, function (key, book){
                        vm.codigos.push(book.Codigo.toString());
                        vm.titulos.push(book.Titulo);
                        vm.books.push(book);
                    });
                });
                vm.filteredCategories = vm.categories;
                
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

getClient: function () {
    
    if (vm.cliente.Cedula === "") {
        vm.activateAlertModalShoppingCart("danger","Debe digitar la cedula", true);
        return false;
    }
    var cliente = { Nombre1: "", Nombre2: "", Apellido1: "", Apellido2: "", Cedula: vm.cliente.Cedula };
    vm.$refs.spinner.show();
    
    
    $.ajax({
        url: urlRoot + 'Home/BuscarCedula',
        type: 'post',
        dataType: 'json',
        data: cliente,
        success: function (result) {
            if (result.EstadoOperacion) {
                vm.cliente.Nombre1 = result.Usuario.Nombre;
                vm.cliente.Apellido1 = result.Usuario.Apellidos;
                vm.cliente.Cedula = result.Usuario.Cedula;
                vm.cliente.Telefono = result.Usuario.Telefono;
                vm.cliente.Email = result.Usuario.Correo;
            } else {
                vm.activateAlertModalShoppingCart('info', 'No se encontro ningun usuario registrado, por favor digite sus datos', true);
            }
            vm.$refs.spinner.hide();
        },
        error: function (error) {
            vm.activateAlertModalShoppingCart('info','No se pudo encontrar el cliente, por favor digite sus datos',true);
            vm.$refs.spinner.hide();
        }
    });

},

validateModalCartFields: function () {
    if (vm.modalCart.currentPage === 2) {
        if (vm.cliente.Cedula !== "" && vm.cliente.Nombre1 !== "" && vm.cliente.Apellido1 !== "" && vm.cliente.Telefono !== ""  && vm.cliente.Email !== "") {
            vm.modalCart.currentPage += 1;
        } else {
            vm.activateAlertModalShoppingCart("danger", "Debe ingresar los datos personales",true);
        }
    } else {
        vm.modalCart.currentPage += 1;
    }

},

openModal: function (object, type) {
    vm.disableBuyButton = false;
    if (type === 'bookDetails') {
        vm.activateAlertModalBookDetails('success', '', false);
        vm.modalObject = { item: {}, quantity: 1, total: 0 };
        vm.modalObject.item = object;
        vm.updateModalDetailTotal();
        $("#modalDetails").modal({ show: true });
    } else if (type === 'modalShoppingCart') {
        vm.modalCart.currentPage = 1;
        vm.modalCart.acceptance = false;
        if (vm.shoppingCart.items.length <= 0) {
            vm.activateToastr('danger', 'El carrito esta vacio.', true);
            //vm.activateAlertModalShoppingCart('info', ' El carrito esta vacio', true);
            //vm.enableProccedBtn = false;
            //$("#modalShoppingCart").modal({ show: true });
        } else {
            vm.deposito.Fecha = vm.date;
            vm.showModalShoppingCartNavBar = true;
            vm.activateAlertModalShoppingCart('success', '', false);
            $("#modalShoppingCart").modal({ show: true });
            vm.enableProccedBtn = true;
            
        }
    } else if (type === 'modal-about') {
        $("#modal-about").modal({ show: true });
    } else if (type === 'modal-contacto') {
        $("#modal-contacto").modal({ show: true });
    }
    
},

validatePayment: function () {
    if (vm.cliente.Nombre1 && vm.cliente.Apellido1 && vm.cliente.Cedula && vm.cliente.Telefono && vm.cliente.Email
        && vm.deposito.Fecha && vm.deposito.Referencia && vm.deposito.BancoEmisor !== "" && vm.deposito.BancoReceptor !== "") {
        vm.processPayment();
    } else {
        vm.activateAlertModalShoppingCart('danger', 'Debe llenar los campos requeridos', true);

    }
    
},

updateDetail: function (index) {

    var detail = vm.shoppingCart.items[index];

    detail.total = parseInt(detail.quantity) * detail.item.Precio;
    vm.shoppingCart.items[index] = detail;


    vm.shoppingCart.total = 0;
    vm.shoppingCart.totalItems = 0;
    $.each(vm.shoppingCart.items, function (index, value) {
        vm.shoppingCart.total += value.total;
        vm.shoppingCart.totalItems += value.quantity;
    });
    
},

processPayment: function () {
    vm.modalCart.currentPage = 4;
    vm.$refs.spinner.show();
    
    var productos = [];
    $.each(vm.shoppingCart.items, function (index, object) {
        productos.push({ Codigo: object.item.Codigo, Cantidad: object.quantity });
    });

    var compra = { Cliente: vm.cliente, Productos: productos, Deposito: vm.deposito };
    $.ajax({
        url: urlRoot + 'Home/ProcesarCompra',
        type: 'post',
        dataType: 'json',
        data: compra,
        success: function (result) {
            if (result.EstadoOperacion) {
                vm.shoppingCart.payment.code = result.Confirmacion;
                vm.shoppingCart.payment.status = true;
                vm.showModalShoppingCartNavBar = false;
                vm.modalCart.currentPage = 5;
                
            } else {
                if (result.Agotados !== null || result.Agotados.length > 0) {
                    var lista = "";
                    $.each(result.Agotados, function (index, object) {
                        lista += object.Titulo + " Existencia: "+object.Existencia+", ";
                    });
                    vm.activateAlertModalShoppingCart("danger", "Los siguientes libros no estan disponibles: "+lista+".", true);
                    vm.modalCart.currentPage = 1;
                }
            }
            vm.$refs.spinner.hide();
        },
        error: function (error) {
            vm.shoppingCart.payment.status = false;
           
            vm.$refs.spinner.hide();
        }
    });
    
},
     
search: function () {
    if (vm.searchSelected === '') {
        vm.activateToastr('danger','Debe ingresar los datos del producto que busca', true);
    } else {

        var itemFound = false;
        $.each(vm.books, function (index, book) {
            if (vm.searchSelected.toString().toLowerCase() === book.Codigo.toString().toLowerCase()
                || vm.searchSelected.toString().toLowerCase() === book.Titulo.toString().toLowerCase()) {
                vm.activateAlertModalBookDetails('success', '', false);
                vm.modalObject = { item: {}, quantity: 1, total: 0 };
                vm.modalObject.item = book;
                vm.updateModalDetailTotal();
                $("#modalDetails").modal({ show: true });
                itemFound = true;
                vm.searchSelected = "";
                return;
            }
        });
        if (itemFound === false) {
            vm.activateToastr('danger', 'Ninguno de nuestros productos coincide con los parametros de busqueda que ingreso', true);
        }
        
    }
    

    
    
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

validateCedulaCliente: function () {
    if (vm.cliente.Cedula === "" || vm.cliente.Cedula.length < 9) {
        vm.cedulaStatus = false;
    } else {
        vm.cedulaStatus = true;
    }
    vm.validateCedula = true;
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
        var agregado = false;
        $.each(vm.shoppingCart.items, function (index, value) {
            if (item.item.Codigo === value.item.Codigo) {
                value.quantity += item.quantity;
                value.total += item.total;
                vm.shoppingCart.items[index] = value;
                agregado = true;
            }
        });
        if (agregado === false) {
            vm.shoppingCart.items.push(item);
        }

        vm.shoppingCart.total =0;
        vm.shoppingCart.totalItems =0;
        $.each(vm.shoppingCart.items, function (index, value) {
            vm.shoppingCart.total += value.total;
            vm.shoppingCart.totalItems += value.quantity;
        });
        
        vm.activateAlertModalBookDetails('success', 'El producto fue agregado con exito al carrito', true);
    } else {
        vm.activateAlertModalBookDetails('danger', 'Hemos encontrado un problema para agregar el producto al carrito,por favor intente de nuevo', true);
    }
},

updateModalDetailTotal: function () {
    vm.modalObject.total = (vm.modalObject.item.Precio * vm.modalObject.quantity);
},

//showWikiSection1: function () {
//    var html = "<p>aqui va el codigo html</p>";
//    var title = "SIGELIBMA - Assistant";
//    this.createWiki(title, html);
//},

init: function () {
    vm.displaySpinner(true, 'Obteniendo informacion de la base de datos, por favor espere!');
    vm.activateAlert('danger', '', false);
    vm.getPageData();
    
},

verifyStock: function () {
    if (vm.modalObject.quantity > vm.modalObject.item.Stock) {
        vm.activateAlertModalBookDetails("danger", "No puede comprar una cantidad mayor a la disponible", true);
        vm.disableBuyButton = true;
    } else {
        vm.disableBuyButton = false;
    }
    vm.updateModalDetailTotal();
},

},
    
//Define Computes
computed: {

},
filters: {
}

    
}

    );

vm.init();





