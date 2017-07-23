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
data.login = { Username: '', Password: '' };
data.alert = { type: 'success', message: 'alert', status: false };

var vm = new Vue({
    el: '#LoginContainer',
    data: data,
    components: {
         vueinput: VueStrap.input,
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
        activateAlert: function (type, message, status) {
            vm.alert.type = type;
            vm.alert.message = message;
            vm.alert.status = status;
        },

        lowerCase: function (stringValue) {
            return stringValue.toLowerCase();
        },

        executeLogin: function () {
            if (!vm.login.Password || !vm.login.Username) {
                vm.activateAlert('Danger', 'Debe ingresar su usuario y contraseña.', true);
                return;
            }
            
            $.ajax({
                url: urlRoot + 'Login/ValidarLogin',
                type: 'post',
                dataType: 'json',
                data: vm.login,
                async: true,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        window.location.href = result.Url;
                    } else {
                        vm.activateAlert('Danger', result.Mensaje, true);
                    }
                },
                error: function (error) {
                    vm.activateAlert('Danger',"Error inesperado, por favor intente de nuevo o notifique a soporte tecnico." , true);
                }
            });
        },

      
        init: function () {
          

        },

      

      

    },

    //Define Computes
    computed: {

    },
    filters: {
    }


});

vm.init();








