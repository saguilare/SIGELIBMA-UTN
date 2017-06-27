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

//bootstrap modal opptions


//var options = { backdrop: false, keyboard: false, show: false };
//$('#modal').modal(options);
var indexFound = 0;
var comeBackToSecction3 = false;
var enableDevice = {};
enableDevice.login = { account: '', passwd: '' };
enableDevice.employee = {};
enableDevice.retrieveUser = true;
enableDevice.device = { deviceName: '', devicePort: '' };
enableDevice.alert = { type: 'success', message: 'alert', status: false };
enableDevice.asideWiki = { show: false, title: '' };



var vm = new Vue({
    el: '#pageMainContainer',
    data: enableDevice,
    components: {
        //typeahead: customAutocomplete,
        //datepicker: VueStrap.datepicker,
        //modal not working the second time
        //bug in vue-strap
        //using bootstrap modal instead
        //modal: VueStrap.modal,
        vueStrapAside: VueStrap.aside,
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

        setlogin: function () {
            if (!vm.login.passwd || !vm.login.account) {
                vm.activateAlert('Danger', 'PLease provide username and password', true);
                return;
            }


            console.log(urlRoot + 'Login/validateLogin');
            $.ajax({
                url: urlRoot + 'Login/validateLogin',
                type: 'post',
                dataType: 'json',
                data: vm.login,
                async: true,
                success: function (result) {
                    if (result.OperationStatus) {
                        window.location.href = result.Url;
                    } else {
                        vm.activateAlert('Danger', result.Message, true);
                    }


                },
                error: function (error) {
                
                    vm.activateAlert('Danger',"Unexpected system error, please try again or report incident to Network Automation Group" , true);
                }
            });
        },

        getUser: function () {
           
                $.ajax({
                    url: urlRoot + 'Login/GetUsername',
                    type: 'get',
                    dataType: 'json',
                    async: true,
                    success: function (result) {
                        if (result.OperationStatus) {
                            vm.employee = result.Employee;
                            vm.login.account = vm.employee.Idsid;
                        } else {
                            vm.activateAlert('Danger', result.Message, true);
                        }
                        
                    },
                    error: function (error) {
              
                        vm.activateAlert('Danger', "Unexpected system error, please try again or report incident to Network Automation Group", true);
                    }
                });
                vm.retrieveUser = false;
          

        },
        checkForErrors: function () {
            var url = window.location.href;
            var query = window.location.search.substring(1);
            var code = query.split('=');
            if (code[0].toLowerCase() === 'errorcode' && code[1] === '1') {
                vm.activateAlert('Danger', "Session Expired/Terminated, pLease log in again", true);
            }
        },
        init: function () {
            this.checkForErrors();
            this.getUser();

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








