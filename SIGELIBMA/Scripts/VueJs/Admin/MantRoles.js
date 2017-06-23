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

var indexFound = 0;
var items = [];
var comeBackToSecction3 = false;
var data = {};
data.roles = [];
data.employee = {};
data.device = { deviceName: '', devicePort: '', hostName: '', hostIPAddress: '' };
data.devices = [];
data.alert = { type: 'success', message: 'alert', status: false };
data.asideWiki = { show: false, title: '' };
data.validations = { showSpinner: false };
data.displayWizzard = { showSection1: true };
data.sortKey = 'deviceName';
data.reverse = false;
data.search = '';
data.showEnableButtons = false;
data.showSearchTab = false;
data.columns = ['deviceName', 'devicePort'];
data.disableNavTabs = { show: false };
data.displayWizzard = {
    showSection1: false,
    showSection2: false,
    showAutomaticTab: false,
    showManuallyTab: true,
};
data.validations.section1 = {

    showNavTabs: false,
    disableAutomaticTab: false,
    disableManuallyTab: false,

};
data.searchItem = '';
data.items = items;
data.filteredItems = [];
data.paginatedItems = [];
data.selectedItems = [];
data.pagination = { range: 5, currentPage: 1, itemPerPage: 8, items: [], filteredItems: [] };
data.loadingMessage = "The tool is collecting Device data, please wait!...";

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

        activateAlert: function (type, message, status) {
            vm.alert.type = type;
            vm.alert.message = message;
            vm.alert.status = status;
        },
        lowerCase: function (stringValue) {
            return stringValue.toLowerCase();
        },
        goToWizzardSection1: function () {
            if (!vm.disableNavTabs.show) {
                vm.alert.status = false;
                vm.displayWizzard.showSection1 = true;
                vm.displayWizzard.showSection2 = false;
                vm.displayWizzard.showAutomaticTab = false;
                vm.displayWizzard.showManuallyTab = true;
            } else {
                return false;
            }
        },
        goToWizzardSection2: function () {
            if (!vm.disableNavTabs.show) {
                vm.alert.status = false;
                vm.displayWizzard.showSection1 = false;
                vm.displayWizzard.showSection2 = true;
                vm.displayWizzard.showAutomaticTab = true;
                vm.displayWizzard.showManuallyTab = false;
            } else {
                return false;
            }
        },

        getRoles: function () {
           
                $.ajax({
                    url: urlRoot + 'MantRoles/GetRoles',
                    type: 'get',
                    dataType: 'json',
                    async: true,
                    success: function (result) {
                        if (result.OperationStatus) {
                            vm.roles = result.Roles;
                        } else {
                            window.location.href = result.Url;
                        }
                       
                    },
                    error: function (error) {
                        vm.displaySpinner(false);
                        vm.validations.section1.showNavTabs = false;
                        vm.activateAlert('Danger', "Unexpected system error, please try again or report incident to Network Automation Group", true);
                    }
                });
              
            
        },
     
        displaySpinner: function (action) {
            if (action) {
                vm.validations.showSpinner = action;
                vm.validations.section1.showNavTabs = false;
            } else {
                vm.validations.showSpinner = action;
                vm.validations.section1.showNavTabs = true;
            }
            
        },

        changePagination: function () {
            vm.buildPagination();
            vm.selectPage(1);
        },

        initializeVariables: function () {
            $.ajax({
                url: urlRoot + 'InitializeVariables',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (data) {
                    if (data.OperationStatus) {
                        vm.showEnableButtons = data.showEnableButtons;
                        vm.showSearchTab = data.showSearchTab;
                        if (vm.showSearchTab) {
                            vm.getAllDevices();
                        } else {
                            vm.displayWizzard.showSection1 = true;
                            vm.validations.section1.showNavTabs = true;
                        }

                    }

                },
                error: function (error) {
                    vm.showEnableButtons = false;
                    vm.showSearchTab = false;

                }
            });


        },

        getAllDevices: function () {
            vm.displaySpinner(true);
            $.ajax({
                url: urlRoot + 'GetAllDevices',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (data) {
                    if (data.OperationStatus) {
                        vm.displaySpinner(false);
                        vm.devices = data.devices;
                        items = data.devices;
                        vm.filteredItems = items;
                        vm.buildPagination();
                        vm.selectPage(1);
                        vm.displayWizzard.showSection1 = true;
                        vm.validations.section1.showNavTabs = true;
                        
                    } 

                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.displayWizzard.showSection1 = true;
                    vm.activateAlert('Danger', "Unexpected system error, please try again or report incident to Network Automation Group", true);
                }
            });


        },

        sentInformation: function () {
            vm.alert.status = false;
            vm.displayWizzard.showSection1 = false;
            vm.loadingMessage = "The switch port is being enabled. Please wait!...";
            vm.displaySpinner(true);
            var device = { deviceName: vm.device.deviceName, devicePort: vm.device.devicePort };
            console.log(urlRoot + 'sentInformation');
            $.ajax({
                url: urlRoot + 'sentInformation',
                type: 'post',
                dataType: 'json',
                data: device,
                async: true,
                success: function (data) {
                    if (data.OperationStatus && data.Result) {
                        vm.displaySpinner(false);
                        vm.displayWizzard.showSection1 = true;
                        vm.alert.type = 'success';
                        vm.alert.message = data.Message;
                        vm.alert.status = true;
                    }else if (data.OperationStatus && !data.Result) {
                        vm.displaySpinner(false);
                        vm.displayWizzard.showSection1 = true;
                        vm.alert.type = 'danger';
                        vm.alert.message = data.Message;
                        vm.alert.status = true;
                    }
                    else {
                        vm.displaySpinner(false);
                        vm.displayWizzard.showSection1 = true;
                        vm.alert.type = 'danger';
                        vm.alert.message = data.Message;
                        vm.alert.status = true;
                    }

                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.displayWizzard.showSection1 = true;
                    vm.activateAlert('Danger', "Unexpected system error, please try again or report incident to Network Automation Group", true);
                }
            });
            },
        sentInformationAutomatic: function (device) {
            vm.alert.status = false;
            vm.displayWizzard.showSection2 = false;
            vm.loadingMessage = "The switch port is being enabled. Please wait!...";
            vm.displaySpinner(true);
            console.log(urlRoot + 'sentInformation');
            $.ajax({
                url: urlRoot + 'sentInformation',
                type: 'post',
                dataType: 'json',
                data: device,
                async: true,
                success: function (data) {
                    if (data.OperationStatus && data.Result) {
                        vm.displaySpinner(false);
                        vm.displayWizzard.showSection2 = true;
                        vm.alert.type = 'success';
                        vm.alert.message = data.Message;
                        vm.alert.status = true;
                    } else if (data.OperationStatus && !data.Result) {
                        vm.displaySpinner(false);
                        vm.displayWizzard.showSection2 = true;
                        vm.alert.type = 'danger';
                        vm.alert.message = data.Message;
                        vm.alert.status = true;
                    }
                    else {
                        vm.displaySpinner(false);
                        vm.displayWizzard.showSection2 = true;
                        vm.alert.type = 'danger';
                        vm.alert.message = data.Message;
                        vm.alert.status = true;
                    }

                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.displayWizzard.showSection2 = true;
                    vm.activateAlert('Danger', "Unexpected system error, please try again or report incident to Network Automation Group", true);
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
            var html = "<h3 class=text-center>Instructions</h3> \n" +
                        "<hr style=\"border-color:#0071C5!important\"/> \n" +
                        "<p><b>Only Fully Qualified Switch Names hould be used.</b></p>\n" +
                        "<p><b>Do not use host names or server names.</b></p>\n" +
                        "<p><b style='color:red; text-decoration: underline'>Please double check the inputs before enable the port. Putting the wrong port will enable configure an incorrect port.</b></p>\n" +
                        "<hr style=\"border-color:#0071C5!important\"/> \n" +
                        "<h4 class=text-center>Reference file for network switch name list</h4> \n" +
                        "<hr style=\"border-color:#0071C5!important\"/> \n" +
                        "<p><b>Please verify the switch name agains the following spreadsheet file: </b><a style='color:red; text-decoration: underline' href='http://ntsscssvr2.amr.corp.intel.com/editdatabase/index.php?-table=reconnect' target='_blank'>CSC Reconnect List</a></p>\n" +
                        "<hr style=\"border-color:#0071C5!important\"/> \n" +
                        "<h4 class=text-center>Instructions to create a ticket in case that the reconnection tool does not work</h4> \n" +
                        "<hr style=\"border-color:#0071C5!important\"/> \n" +
                        "<p>1.	Create a ticket with the following settings. **Make sure it is set to ISL2 (Incident Specialist Level 2) so that we can see it right away**. </p>\n" +
                        "<p>2.	Include the following details in the ticket’s description:</p>\n" +
                        "<ul><div class='thumbnail'><div class='image'> <img src='../Images/ITERP1.jpg' alt=''></div></div>\n" +
                        "<li>  	a.	Customer’s name and if possible a contact number.</li>\n" +
                        "<li>  	b.	Switch’s FQDN and port that failed to reconnect.</li>\n" +
                        "<li>  	c.	Optionally, include a screenshot of the message received by the Reconnection tool.</li></ul>\n" +
                        "<p>3.	Sent an email to gsm.network-collaboration@intel.com including the ticket number in the subject so that we can start working on it as soon as possible.</p>\n"
            ;

            var title = "Port Enable Tool - Assistant";
            this.createWiki(title, html);
        },
        init: function () {
            this.getRoles();
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







