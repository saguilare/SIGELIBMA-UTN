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
data.sesion = { id: '', usuario: '', username :"" };

var vm2 = new Vue({
    el: '#navbar',
    data: data,
    components: {

    },

    //Define methods
    methods: {
       

        getSession: function () {
            $.ajax({
                url: urlRoot + 'Login/GetSesion',
                type: 'get',
                dataType: 'json',
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm2.sesion = result.Sesion;
                        console.log(vm2.sesion)
                    } else {
                        window.location.href = result.redirectUrl;
                    }
                },
                error: function (error) {
                    window.location.href = result.redirectUrl;
                }
            });
        },

        logOut: function () {
            $.ajax({
                url: urlRoot + 'Login/Logout',
                type: 'get',
                dataType: 'json',
                success: function (result) {

                },
                error: function (error) {
                    console.log("error loging out");
                }
            });
        },

        init: function () {
            vm2.getSession();

        },





    },

    //Define Computes
    computed: {

    },
    filters: {
    }


});

vm2.init();








