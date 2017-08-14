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
data.categorias = [],
data.autores = [];
data.imagen='';
data.proveedores = [];
data.libro = { Codigo: 1, Titulo: '', Descripcion: '', Fecha: '00/00/0000', Categoria1: { Codigo: 0, Descripcion: '', Estado: 0 }, Autor1: { Codigo: 0, Nombre: '', Apellidos: '', Estado: 0 }, Proveedor1: { Codigo: 0, Nombre: '', Telefono: '', Correo: '', Estado: 0 }, PrecioBase: '', PorcentajeGanancia: '', PrecioVentaSinImpuestos: '', PrecioVentaConImpuestos: '', NombreImagen: '', Imagen: [], Estado: 0 };
data.libros = [];
data.modalObject = { Codigo: 0, Titulo: '', Descripcion: ''};
data.alert = { type: 'success', message: 'alert', status: false };
data.alertModal = { type: 'success', message: 'alert', status: true };
data.toastr = { show: false, placement: "top-right", duration: "3000", type: "danger", width: "400px", dismissable: true, message: '' };
data.validations = { activateFieldValidations: false, showSpinner: false, loadingMessage: 'Cargando datos de la base de datos, por favor espere! ...' };
data.validarFechaIngreso = { date: false};
data.modalAccion = 'Agregar Libro';
data.sortKey = 'Codigo';
data.modalCart = { currentPage: 0 };
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

        //PaginationMethods

        clearSearchItem : function() {
            this.searchItem = undefined
            this.searchInTheList('')
        },

        setSort: function(sortkey, reverse){
            vm.sortKey = sortkey;
            vm.reverse = reverse;
        },

        searchInTheList : function(searchText, currentPage) {
            if (_.isUndefined(searchText)) {
                this.filteredItems = _.filter(vm.items, function (v, k) {
                    return !v.selected
                })
            }
            else {
                this.filteredItems = _.filter(vm.items, function (v, k) {
                    console.log(vm.items);
                    if ((v.Codigo!=null)&&(v.Descripcion!=null)&&(v.Titulo!=null)&&(v.Proveedor1.Nombre!=null)) {
                        return (!v.selected && v.Codigo.toString().toLowerCase().indexOf(searchText.toLowerCase()) > -1) 
                            | (!v.selected && v.Descripcion.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                          | (!v.selected && v.Titulo.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                        | (!v.selected && v.Proveedor1.Nombre.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
                       
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

        buildPagination : function() {
            let numberOfPage = Math.ceil(this.filteredItems.length / this.pagination.itemPerPage)
            this.pagination.items = []
            for (var i = 0; i < numberOfPage; i++) {
                this.pagination.items.push(i + 1)
            }
        },

        selectPage : function(item) {
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

        changePagination: function () {
            vm.buildPagination();
            vm.selectPage(1);
        },

        //EndPafinationMothods

        onFileChange(e) {
            var files = e.target.files || e.dataTransfer.files;
            if (!files.length)
                return;
            if (files[0].type === 'image/jpeg') {
                vm.libro.NombreImagen = files[0].name;
                this.createImage(files[0]);
            } else {
                vm.activateAlertModal('danger', 'Solo imagenes jpg', true);
            }
        },

        createImage(file) {
            var imagen = new Image();
            var reader = new FileReader();
            var vm = this;

            reader.onload = (e) => {
                vm.imagen = e.target.result;
            };
            reader.readAsDataURL(file);
        },

        removeImage: function (e) {
            this.imagen = '';
            vm.libro.NombreImagen = '';
        },

        

        openNewModal: function () {
            
            vm.modalAccion = 'Agregar Libro';
            vm.modalCart.currentPage = 1;
            vm.activateAlertModal('', '', false);
            vm.libro = { Codigo: '', Titulo: '', Descripcion: '', Fecha: '00/00/0000', Categoria1: { Codigo: 0, Descripcion: '', Estado: 0 }, Autor1: { Codigo: 0, Nombre: '', Apellidos: '', Estado: 0 }, Proveedor1: { Codigo: 0, Nombre: '', Telefono: '', Correo: '', Estado: 0 }, PrecioBase: '', PorcentajeGanancia: '', PrecioVentaSinImpuestos: '', PrecioVentaConImpuestos: '', NombreImagen: '', Imagen: [], Estado: 0 };
            $("#edit-modal").modal({ show: true });

        },

        openEditModal: function (libro) {
            vm.modalAccion = 'Editar Libro';
            vm.libro = libro;
            vm.modalCart.currentPage = 1;
            vm.activateAlertModal('', '', false);
            
            $("#edit-modal").modal({ show: true });

        },

        calcularPrecios(libro) {
            libro.PrecioVentaSinImpuestos = ((libro.PrecioBase * libro.PorcentajeGanancia) / 100) + parseInt(libro.PrecioBase);
            libro.PrecioVentaConImpuestos = (libro.PrecioVentaSinImpuestos * 0.13) + parseInt(libro.PrecioVentaSinImpuestos);
            
        },

        validarCamposLibro: function (libro,existeLibro) {
            if (existeLibro) {
                vm.activateAlertModal('danger', 'El Codigo del libro ingresado ya existe', true);
            } else {
                if (vm.libro.Codigo && vm.libro.Titulo && vm.libro.Descripcion && vm.libro.Fecha && vm.libro.Categoria1.Codigo && vm.libro.Autor1.Codigo && vm.libro.Proveedor1.Codigo && vm.libro.PrecioBase && vm.libro.PorcentajeGanancia && vm.libro.PrecioVentaSinImpuestos && vm.libro.PrecioVentaConImpuestos && vm.libro.NombreImagen) {
                    if (vm.modalAccion == 'Agregar Libro') {
                        vm.addLibro(libro);
                    } else {
                        vm.modificarLibro(libro);
                    }
                  
                } else {
                    vm.activateAlertModal('danger', 'Debe llenar los campos requeridos', true);
                }
            }

        },


        validarCodigoLibro: function (libro) {
            if (vm.modalAccion == 'Agregar Libro') {
                $.ajax({
                    url: urlRoot + 'mantlibros/validarLibro',
                    type: 'post',
                    dataType: 'json',
                    data: libro,
                    success: function (result) {
                        if (result.ExisteLibro) {
                            vm.validarCamposLibro(libro, true);
                        } else {
                            vm.validarCamposLibro(libro, false);
                        }
                    },
                    error: function (error) {
                        vm.displaySpinner(false);
                        vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                    }
                });
            } else {
                 vm.validarCamposLibro(libro, false);

            }


        },

        addLibro: function (libro) {
            libro.Imagen = vm.imagen;
            vm.imagen = '';
            libro.Estado = 1;
            $("#edit-modal").modal('hide');
            vm.displaySpinner(true, 'Guarando Libro');
            $.ajax({
                url: urlRoot + 'mantlibros/Agregar',
                type: 'post',
                dataType: 'json',
                data: libro,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.activateAlertModal('success', 'El libro fue agregado correctamente', true);
                        vm.displaySpinner(false);
                        vm.getLibros();

                    } else {
                        vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                        vm.displaySpinner(false);
                    }
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
            });
       },

        modificarLibro: function (libro) {
            libro.Imagen = vm.imagen;
            vm.imagen = '';
            $("#edit-modal").modal('hide');
            vm.displaySpinner(true, 'Modificando Libro');
            $.ajax({
                url: urlRoot + 'mantlibros/Modificar',
                type: 'post',
                dataType: 'json',
                data: libro,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.activateAlertModal('success', 'El libro fue modificado correctamente', true);
                        vm.displaySpinner(false);
                        vm.getLibros();

                    } else {
                        vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                        vm.displaySpinner(false);
                    }
                },
                error: function (error) {
                    vm.displaySpinner(false);
                    vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                }
            });
        },

       deleteLibro: function (libro) {
           vm.displaySpinner(true, 'Desabilitando Libro');
           libro.Estado = 0;
           $.ajax({
               url: urlRoot + 'mantlibros/Desabilitar',
               type: 'post',
               dataType: 'json',
               data: libro,
               success: function (result) {
                   if (result.EstadoOperacion) {
                       vm.getLibros();
                   } else {
                       vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
                      
                   }
                   vm.displaySpinner(false);
               },
               error: function (error) {
                   vm.displaySpinner(false);
                   vm.activateToastr('danger', 'La operacion ha fallado, por favor intente nuevamente.', true);
               }
           });


       },

        getLibros: function () {
            $.ajax({
                url: urlRoot + 'mantlibros/Libros',
                type: 'get',
                dataType: 'json',
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.libros = result.Libros;
                        vm.items = vm.tipos;
                        vm.filteredItems = vm.tipos;
                        vm.buildPagination();
                        vm.selectPage(1);
                        vm.activateToastr('success', 'La operacion se completo de manera exitosa.', true);
                    } else {
                        vm.activateToastr('danger','Ha ocurrido un problema, por favor recargue la pagina.',true);
                    }
                    vm.displaySpinner(false,'');
                },
                error: function (error) {
                    vm.activateToastr('danger','Ha ocurrido un problema, por favor recargue la pagina.',true);
                    vm.displaySpinner(false,'');
                }
            });
              
            
        },

        getInitData: function () {
            $.ajax({
                url: urlRoot + 'mantlibros/Init',
                type: 'get',
                dataType: 'json',
                async: true,
                success: function (result) {
                    if (result.EstadoOperacion) {
                        vm.libros = result.Libros;
                        vm.categorias = result.Categorias;
                        vm.autores = result.Autores;
                        vm.proveedores = result.Proveedores;
                        vm.items = vm.libros;
                        vm.filteredItems = vm.libros;
                        vm.buildPagination();
                        vm.selectPage(1);
                        
                    } else {
                        vm.activateToastr('danger', 'Ha ocurrido un problema, por favor recargue la pagina.', true);
                    }
                    vm.displaySpinner(false, '');
                },
                error: function (error) {
                    vm.activateToastr('danger', 'Ha ocurrido un problema, por favor recargue la pagina.', true);
                    vm.displaySpinner(false, '');
                }
            });
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
            vm.getInitData();
        }
      
    },

    //Define Computes
    computed: {

    },
    filters: {
    }


});//close vue instance

vm.init();

