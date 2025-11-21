<%@ Page Language="VB" AutoEventWireup="false" CodeFile="visor_documentos.aspx.vb" Inherits="gestion_visor_documentos" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    
    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link href="../Content46/bootstrap.css" rel="stylesheet" />
    <link href="../Content46/alertifyjs/alertify.min.css" rel="stylesheet" />

    <!-- JQUERY-------------------------------------------------------------------------------------------------->    
    <script src="../Scripts46/jquery-3.6.0.js"></script>
    <script src="../Scripts46/jquery-ui.js"></script>
    <script src="../Scripts46/bootstrap.js"></script>
    <script src="../Scripts46/alertify.js"></script>
    <script src="../Scripts46/shortcut.js"></script>
    <script src="../Scripts46/device.js"></script>

    <!-- PERSONAL---------------------------------------------------------------------------------------------------->     
    <link href="../Content46/interior.css" rel="stylesheet" />
    <script src="../Scripts46/interior.js"></script>
    <link href="../Content46/justcontext.css" rel="stylesheet" />
    <script src="../Scripts46/justcontext.js"></script>
    <script src="../Scripts46/filedrop.js"></script>
    <link href="../Content46/fileinput.css" rel="stylesheet" />
    <script src="../Scripts46/fileinput.js"></script>
    <script src="../Scripts46/locales/es.js"></script>

    <script type="text/javascript">

        //Parametros Globales
        var parametros_usuario;

        $(document).ready(function () {

            //Desactivo la funcion F5 (refresco)
            shortcut.add("F5", function () { });
           
            //Quitar Enter
            $("form").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });

            //Eliminamos el click derecho de ratón para las multiples pantallas
            $('html').bind("contextmenu", function (e) {
                return false;
            });

            //Cambio de contenedor
            $(".panel-container").css('height', $(window).height() - 80 + 'px');

            //Control para el click
            $("#recoger").on('click', function (e) {

                $(".panel-left").toggle(500);

            })

            //Control para el click sobre el GV
            $("#<%=gridview_consulta.ClientID%> tr td").mousedown(function (e) {

                //Asigno
                var vposicion = (this.id).split("_")
                var fila = vposicion[0]
                var columna = vposicion[1]
              
                //1: izquierda, 2: medio/ruleta, 3: derecho
        	    //if (e.which == 1) {
           	    //       if ($("#gridview_consulta_chk_seleccionar_" + this.id).prop('checked')==false) {
                //           $("#gridview_consulta_chk_seleccionar_" + this.id).prop("checked", true);
                //       } else {
                //           $("#gridview_consulta_chk_seleccionar_" + this.id).prop("checked",false);
                //       }
                //   }
                //if (e.which == 3) {
                //if (columna != "0") {
                //    $("#gridview_consulta_chk_seleccionar_" + fila).prop("checked", true);
                //}
                //}
                
            })

            $("#<%=TreeView1.ClientID%> td a").on('click', function (e) {
           
                //Asigno
                mostrar_trabajando('Cargando datos, por favor espere.');

            })

            //Control para arrastras PDF
            $("#table_pdf").filedrop({
                fallback_id: 'btnUpload',
                fallback_dropzoneClick: true,
                url: '<%=ResolveUrl("~/Handler_file.ashx")%>',
                //allowedfiletypes: ['image/jpeg', 'image/png', 'image/gif', 'application/pdf', 'application/doc'],
                allowedfileextensions: ['.pdf','.txt','.xlsx','.docx','.zip','.jpg','.png'],
                paramname: 'fileData',
                data: { param1: $('#txt_ruta').val() },
                error: function(err, file) {
		            switch(err) {
			            case 'BrowserNotSupported':
				            alert('browser does not support HTML5 drag and drop')
				            break;
			            case 'TooManyFiles':
				            // user uploaded more than 'maxfiles'
				            break;
                        case 'FileTooLarge':
                            error("Sólo esta permitido ficheros con un máximo de 5 Mb.")
				            // program encountered a file whose size is greater than 'maxfilesize'
				            // FileTooLarge also has access to the file which was too large
				            // use file.name to reference the filename of the culprit file
				            break;
			            case 'FileTypeNotAllowed':
				            // The file type is not in the specified list 'allowedfiletypes'
				            break;
                        case 'FileExtensionNotAllowed':
                            error("Formato de fichero no permitido.")
				            // The file extension is not in the specified list 'allowedfileextensions'
				            break;
			            default:
				            break;
		            }
	            },
                maxfiles: 50, //Maximum Number of Files allowed at a time.
                maxfilesize: 100, //Maximum File Size in MB.
                queuefiles: 1,
                dragOver: function () {
                    $('#table_pdf').css({'border':'1px dashed #721c24','background-color':'#c3e6cb' })
                },
                dragLeave: function () {
                    $('#table_pdf').css({'border':'0px solid #c3e6cb','background-color':'white' })
                },
                drop: function () {
                    $('#table_pdf').css({'border':'0px solid #c3e6cb','background-color':'white' })
                },
                uploadStarted: function (i, file, len) {

                    //Asigno
                    mostrar_trabajando('Subiendo Fichero: ' + (i+1) + ' de ' + len);
                    
		            // a file began uploading
		            // i = index => 0, 1, 2, 3, 4 etc
		            // file is the actual file of the index
		            // len = total files user dropped
	            },
                uploadFinished: function (i, file, response, time) {
                    
                },
                progressUpdated: function(i, file, progress) {
		            // this function is used for large files and updates intermittently
		            // progress is the integer value of file being uploaded percentage to completion
	            },
	            globalProgressUpdated: function(progress) {
		            // progress for all the files uploaded on the current instance (percentage)
		            // ex: $('#progress div').width(progress+"%");
	            },
	            speedUpdated: function(i, file, speed) {
		            // speed in kb/s
                },
                rename: function (name) {
                    //var d = new Date();
                    //var dia = d.getDate();
                    //var mes = d.getMonth();
                    //var ano = d.getFullYear();
                    //var hora = d.getHours()
                    //var minutos = d.getMinutes()
                    //var segundos = d.getSeconds()
                    //var milisegundos = d.getMilliseconds()
                    //var nombre_fichero = "PDF_" + parametros_usuario[0] + "_" + dia + (mes + 1) + ano + "_" + hora + minutos + segundos + milisegundos + '.pdf'

                    //Asigno
                    //$('#txt_nombre_pdf').val(name + '|' + $('#txt_nombre_pdf').val())

                    return name
		            // name in string format
		            // must return alternate name as string
	            },
                beforeEach: function (file) {
		            // file is a file object
		            // return false to cancel upload
	            },

                afterAll: function (e) {
                    //$('#processing-modal').hide();

                    mostrar_trabajando("Procesando PDF, por favor espere")
                    $('#btnUpload').click();
                   
                }
            })
          
            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#imagen_posterior").on('click', function (e) {

                //Aumento el numero a buscar
                var actual = parseInt($("#txt_actual").val())
                $("#txt_actual").val(actual + 1)

                //Leo de control txt_informacion
                var informacion = $("#txt_informacion").val().split("&")
                for (var i = 0; i <= informacion.length - 1; i++) {

                    //Descompongo los detalles de cada ventana
                    var detalles_ventana = informacion[i].split("|")

                    if (detalles_ventana[0] == $("#txt_actual").val()) {

                        //Dependiendo de la extensión del fichero
                        var fichero = detalles_ventana[1].toLowerCase();
                        var ext = fichero.split('.');
                        ext = '.' + ext[ext.length -1];
                    
                        switch (ext) { 
	                        case '.docx': 
		                        $('#ver_documento').attr('src', 'https://docs.google.com/gview?url=' + 'https://www.iocloudcomputing.io/contabilidad/imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '&embedded=true')
		                        break;
	                        case '.xlsx': 
		                        $('#ver_documento').attr('src', 'https://docs.google.com/gview?url=' + 'https://www.iocloudcomputing.io/contabilidad/imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '&embedded=true')
		                        break;
                            case '.jpg': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
		                        break;		
	                        case '.png': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                                break;
                            case '.txt': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                                break;
                            case '.pdf': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                                break;
                            case '.zip': 
                                error('No se puede visualizar archivos comprimidos, descárguelos para poder verlos.')
                                return false;
                            }

                        $("#informacion").html('Nombre: <span style="color:#28a745;">' + detalles_ventana[1] + '</span> - (' + detalles_ventana[3] + ' Kb)')
                        $("#total_imagenes").text($("#txt_actual").val() + "  de  " + $("#txt_total").val())

                        //Dibujar flechas
                        dibujar_flechas();

                        //Dibujar imagenes de rotar
                        dibujar_rotar(detalles_ventana[1]);

                        break;
                    }

                }

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#imagen_anterior").on('click', function (e) {

                //Aumento el numero a buscar
                var actual = parseInt($("#txt_actual").val())
                $("#txt_actual").val(actual-1)

                //Leo de control txt_informacion
                var informacion = $("#txt_informacion").val().split("&")
                for (var i = 0; i <= informacion.length - 1; i++) {

                    //Descompongo los detalles de cada ventana
                    var detalles_ventana = informacion[i].split("|")

                    if (detalles_ventana[0] == $("#txt_actual").val()) {

                        //Dependiendo de la extensión del fichero
                        var fichero = detalles_ventana[1].toLowerCase();
                        var ext = fichero.split('.');
                        ext = '.' + ext[ext.length -1];
                    
                        switch (ext) { 
	                        case '.docx': 
		                        $('#ver_documento').attr('src', 'https://docs.google.com/gview?url=' + 'https://www.iocloudcomputing.io/contabilidad/imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '&embedded=true')
		                        break;
	                        case '.xlsx': 
		                        $('#ver_documento').attr('src', 'https://docs.google.com/gview?url=' + 'https://www.iocloudcomputing.io/contabilidad/imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '&embedded=true')
		                        break;
                            case '.jpg': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
		                        break;		
	                        case '.png': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                                break;
                            case '.txt': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                                break;
                            case '.pdf': 
		                        $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                                break;
                            case '.zip': 
                                error('No se puede visualizar archivos comprimidos, descárguelos para poder verlos.')
                                return false;
                        }

                        $("#informacion").html('Nombre: <span style="color:#28a745;">' + detalles_ventana[1] + '</span> - (' + detalles_ventana[3] + ' Kb)')
                        $("#total_imagenes").text($("#txt_actual").val() + "  de  " + $("#txt_total").val())

                        //Dibujar flechas
                        dibujar_flechas();

                        //Dibujar imagenes de rotar
                        dibujar_rotar(detalles_ventana[1]);

                        break;
                    }

                }

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#img_adjuntar_asiento").on('click', function (e) {

                //Obtener el numero de asiento si lo hay
                var id_asiento = String(window.parent.$("#ientradas").contents().find("#txt_id_cabecera_asiento").val());
                if (id_asiento == 'undefined') {
                    error("La ventana de asiento no esta activa");
                    return false;
                }

                //Compruebo si existe cabecera de asiento
                if (id_asiento == '') {
                    error("No hay ningún asiento activo");
                    return false;
                }

                //Variables
                var n_asiento = String(window.parent.$("#ientradas").contents().find("#txt_numero_asiento").val());
                var n_apuntes = String(window.parent.$("#ientradas").contents().find("#lbl_n_apuntes").text());

                //Compruebo si existe lineas de asiento
                if (n_apuntes=='0' || n_apuntes=='') {
                    error("El asiento no contiene aún ningún apunte.");
                    return false;
                }
                
                //Busco los detalles de la imagen seleccionada
                //Leo de control txt_informacion
                var valores = '';
                var informacion = $("#txt_informacion").val().split("&")
                for (var i = 0; i <= informacion.length - 1; i++) {
                    //Descompongo los detalles de cada ventana
                    var detalles_ventana = informacion[i].split("|")
                    if (detalles_ventana[0] == $("#txt_actual").val()) {
                    //Preparo la insercion
                        valores = "0|" + id_asiento + "|" + detalles_ventana[1] + "|" + detalles_ventana[2] + "|" + detalles_ventana[3] + "|" + n_asiento
                        break;
                    }
                }

                //Asigno
                $('#txt_asignar_impuesto').val(valores);

                //Muestro
                $('#lbl_adjuntar_asiento').text(n_asiento);
                $('#modal_adjuntar_asiento').modal('show');
                                
            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#img_adjuntar_impuesto").on('click', function (e) {
                
                //Obtener el numero de impuestos si lo hay
                var id_impuesto = String(window.parent.$("#ientradas_").contents().find("#lbl_identificador_impuesto").text());
                var id_impuesto_IA = String(window.parent.$("#ientradas_IA_").contents().find("#lbl_identificador_impuesto").text());

                if (id_impuesto == '' && id_impuesto_IA == '') {
                    error("No hay ningún impuesto activo");
                    return false;
                }

                //Si es impuesto normal
                if (id_impuesto != '') {
                
                    var id_asiento = window.parent.$("#ientradas_").contents().find("#numero_asiento").val();
                    var n_asiento = window.parent.$("#ientradas_").contents().find("#lbl_identificador_asiento").text();
                    var n_apuntes = window.parent.$("#ientradas_").contents().find("#txt_n_lineas").val();
                    if (n_apuntes == "0") {
                        error("El impuesto no contiene aún ningún apunte.");
                        return false;
                    }

                }

                //Si es impuesto IA
                if (id_impuesto_IA != '') {
                    var id_asiento_IA = window.parent.$("#ientradas_IA_").contents().find("#txt_id_cabecera_asiento").val();
                    var n_asiento_IA = window.parent.$("#ientradas_IA_").contents().find("#lbl_identificador_asiento").text();
                }

                //Busco los detalles de la imagen seleccionada
                //Leo de control txt_informacion
                var valores = '';
                var valores_IA = '';
                var informacion = $("#txt_informacion").val().split("&")
                for (var i = 0; i <= informacion.length - 1; i++) {
                    //Descompongo los detalles de cada ventana
                    var detalles_ventana = informacion[i].split("|")
                    if (detalles_ventana[0] == $("#txt_actual").val()) {
                    //Preparo la insercion
                        valores = id_impuesto + "|" + id_asiento + "|" + detalles_ventana[1] + "|" + detalles_ventana[2] + "|" + detalles_ventana[3] + "|" + n_asiento
                        valores_IA = id_impuesto_IA + "|" + id_asiento_IA + "|" + detalles_ventana[1] + "|" + detalles_ventana[2] + "|" + detalles_ventana[3] + "|" + n_asiento_IA
                        break;
                    }
                }

                //Asigno
                $('#txt_asignar_impuesto').val(valores);
                $('#txt_asignar_impuesto_IA').val(valores_IA);
                $('#lbl_adjuntar_impuesto').text(n_asiento);
                $('#lbl_adjuntar_impuesto_IA').text(n_asiento_IA);
                if (id_impuesto_IA == '') {
                    $("#tabla_IA").css({ "visibility": "hidden","display": "none" })
                } else {
                    $("#tabla_IA").css({ "visibility": "visible","display": "inline" })
                }
                if (id_impuesto == '') {
                    $("#tabla").css({ "visibility": "hidden","display": "none" })
                } else {
                    $("#tabla").css({ "visibility": "visible","display": "inline" })
                }

                //Muestro
                $('#modal_adjuntar_impuesto').modal('show');
                                
            })

        });

        //Tamaño para el desktop
        $(window).resize(function (e) {

            if ($("#modal_visualizar_fichero").is(":visible") == true) {
                //Asigno alto
                $("#ver_documento").css("height", $(window).height() - 150)
            }

            $(".panel-container").css ('height', $(window).height() - 80 + 'px' );
      
        })

        function handleMenuAction(evt) {

            if (evt=="new_folder") {
                $('#modal_agregar_carpeta').modal('show');
                setTimeout(function () { $("#txt_nombre_nueva_carpeta").focus(); }, 100);
            };

            if (evt == "delete_folder") {

                //Asigno
                var carpeta = $('#txt_ruta').val();
                var ext = carpeta.split('|');
                ext = ext[ext.length - 1];

                if (ext == "almacenamiento") {
                    error('No puedes eliminar la carpeta raíz del IO Almacenamiento.')
                    return false;
                }

                if (ext == "bandeja") {
                    error('No puedes eliminar la carpeta raíz de la Bandeja de Entrada.')
                    return false;
                }

                //Extraer la ultima palabra
                var ruta = $('#txt_ruta').val()
                var palabra = ruta.substr(ruta.lastIndexOf("|") + 1)

                //Asigno
                $('#lbl_carpeta_borrar').text(palabra)
                $('#modal_eliminar_carpeta').modal('show');
                
            };

            if (evt == "rename_folder") {

                 //Asigno
                var carpeta = $('#txt_ruta').val();
                var ext = carpeta.split('|');
                ext = ext[ext.length - 1];

                if (ext == "almacenamiento") {
                    error('No puedes cambiar el nombre de la carpeta raíz del IO Almacenamiento.')
                    return false;
                }

                if (ext == "bandeja") {
                    error('No puedes cambiar el nombre de la carpeta raíz de la Bandeja de Entrada.')
                    return false;
                }

                //Asigno
                var carpeta = $('#txt_ruta').val();
                var ext = carpeta.split('|');
                ext = ext[ext.length - 1];

                $('#txt_rename_fonder').val(ext);
                $('#modal_renombrar_carpeta').modal('show');
                setTimeout(function () { $('#txt_rename_fonder').focus(); $('#txt_rename_fonder').select(); }, 100);
                
            }

            if (evt == "delete_file") {

                //Declaro
                var contador = 0;
                var nombre = '';

                //Busco los marcados
                $("#<%=gridview_consulta.ClientID%> :checkbox").each(function() {
                    
                    if (this.checked) {
                        contador += 1;
                        nombre = $("#" + this.id).parents("tr").find("td").eq(3).html();
                    }

                });

                //Excepcion
                 if (contador==0) {
                    error('No tiene ningún fichero seleccionado.')
                    return false;
                }

                //Asigno
                $('#lbl_fichero_borrar').text("¿Esta seguro de querer eliminar el fichero " + nombre + " ?");
                if (contador > 1) {
                    $('#lbl_fichero_borrar').text("¿Esta seguro de querer eliminar los " + contador + " ficheros seleccionados?");
                } 
                $('#modal_eliminar_fichero').modal('show');

            };

            if (evt == "download_file") {

                //Declaro
                var contador = 0;
                var nombre = '';

                //Busco los marcados
                $("#<%=gridview_consulta.ClientID%> :checkbox").each(function() {
                    
                    if (this.checked) {
                        contador += 1;
                        nombre = $("#" + this.id).parents("tr").find("td").eq(3).html();
                    }

                });

                //Excepcion
                 if (contador==0) {
                    error('No tiene ningún fichero seleccionado.')
                    return false;
                }

                 //Asigno
                $('#lbl_descargar_fichero').text("¿Esta seguro de querer descargar el fichero " + nombre + " ?");
                if (contador > 1) {
                    $('#lbl_descargar_fichero').text("¿Esta seguro de querer descargar los " + contador + " ficheros seleccionados?");
                } 
                $('#modal_descargar_fichero').modal('show');

            };

            if (evt == "rename_file") {
                
                //Declaro
                var contador = 0;
                var nombre = '';

                //Busco los marcados
                $("#<%=gridview_consulta.ClientID%> :checkbox").each(function() {
                    
                    if (this.checked) {
                        contador += 1;
                        nombre = $("#" + this.id).parents("tr").find("td").eq(2).html();
                    }

                });

                //Excepcion
                if (contador==0) {
                    error('No tiene ningún fichero seleccionado.')
                    return false;
                }

                if (contador > 1) {
                    error('Para poder cambiar el nombre de un fichero, sólo  puede seleccionar 1 a la vez.')
                    return false;
                }

                //Asigno
                var fichero = nombre;
                var ext = fichero.split('.');
                ext = '.' + ext[ext.length -1];

                $('#txt_rename_fichero').val(nombre.replace(ext,''));
                $('#modal_rename_fichero').modal('show');
                setTimeout(function () { $('#txt_rename_fichero').focus(); $('#txt_rename_fichero').select(); }, 100);

            };

            if (evt=="upload_file") {
                $('#modal_fichero_adjunto').modal('show');
            };

            if (evt == "copy_file") {

                //Declaro
                var contador = 0;
                var nombre = '';

                //Busco los marcados
                $("#<%=gridview_consulta.ClientID%> :checkbox").each(function() {
                    
                    if (this.checked) {
                        contador += 1;
                        nombre += $("#txt_ruta").val() + "|" + $("#" + this.id).parents("tr").find("td").eq(2).html() + "&"
                    }

                });

                //Excepcion
                if (contador==0) {
                    error('No tiene ningún fichero seleccionado.')
                    return false;
                }

                //Asigno
                $("#txt_copiar").val(nombre.substring(0,nombre.length-1))
                $("#txt_mover").val('')
                

                //Informo
                 ok('Fichero(s) copiado(s).')

            }

            if (evt == "move_file") {

                //Declaro
                var contador = 0;
                var nombre = '';

                //Busco los marcados
                $("#<%=gridview_consulta.ClientID%> :checkbox").each(function() {
                    
                    if (this.checked) {
                        contador += 1;
                        nombre += $("#txt_ruta").val() + "|" + $("#" + this.id).parents("tr").find("td").eq(2).html() + "&"
                    }

                });

                //Excepcion
                if (contador == 0) {
                    error('No tiene ningún fichero seleccionado.')
                    return false;
                } 
                //Asigno
                $("#txt_mover").val(nombre.substring(0,nombre.length-1))
                $("#txt_copiar").val('')

                //Informo
                ok('Fichero(s) listo(s) para mover.')

            }

            if (evt == "paste_file") {

                //Excepción
                if ($("#txt_copiar").val()=='' && $("#txt_mover").val()=='') {
                    error('No tiene ningún fichero seleccionado a copiar o mover.')
                    return false;
                }

                //click al elemento
                $('#btn_pegar').click();
                
            }

	    }

        function visor() {
            
            //Leo de control txt_informacion
            var informacion = $("#txt_informacion").val().split("&")
            for (var i = 0; i <= informacion.length - 1; i++) {
                //Descompongo los detalles de cada ventana
                var detalles_ventana = informacion[i].split("|")
                //Si coincide el numero
                if (detalles_ventana[0] == $("#txt_actual").val()) {

                    //Asigno Ancho y alto del Iframe
                    $("#ver_documento").css("width", "100%")
                    $("#ver_documento").css("height", $(window).height() - 150)

                    //Dependiendo de la extensión del fichero
                    var fichero = detalles_ventana[1].toLowerCase();
                    var ext = fichero.split('.');
                    ext = '.' + ext[ext.length -1];
                    
                    switch (ext) { 
	                    case '.docx': 
		                    $('#ver_documento').attr('src', 'https://docs.google.com/gview?url=' + 'https://www.iocloudcomputing.io/contabilidad/imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '&embedded=true')
		                    break;
	                    case '.xlsx': 
		                    $('#ver_documento').attr('src', 'https://docs.google.com/gview?url=' + 'https://www.iocloudcomputing.io/contabilidad/imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '&embedded=true')
		                    break;
                        case '.jpg': 
		                    $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
		                    break;		
	                    case '.png': 
		                    $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                            break;
                        case '.txt': 
		                    $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                            break;
                        case '.pdf': 
		                    $('#ver_documento').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + "/" +  detalles_ventana[1] + '?ver=' + new Date())
                            break;
                        case '.zip': 
                            error('No se puede visualizar archivos comprimidos, descárguelos para poder verlos.')
                            return false;
                    }

                    $("#informacion").html('Nombre: <span style="color:#28a745;">' + detalles_ventana[1] + '</span> - (' + detalles_ventana[3] + ' Kb)')
                    $("#total_imagenes").text($("#txt_actual").val() + "  de  " + $("#txt_total").val())

                    //Dibujar flechas
                    dibujar_flechas();

                    //Dibujar imagenes de rotar
                    dibujar_rotar(detalles_ventana[1]);

                    //Muestro imagen
                    $('#modal_visualizar_fichero').modal('show');

                    break;
                }

            }
            
        };               

        function dibujar_flechas() {

            //Si es la ultima imagen a mostrar
            if ($("#txt_actual").val() == $("#txt_total").val()) {
                $("#imagen_posterior").hide();
            } else {
                $("#imagen_posterior").show();
            }

            //Si es la ultima imagen a mostrar
            if ($("#txt_actual").val() == "0") {
                $("#imagen_anterior").hide();
            } else {
                $("#imagen_anterior").show();
            }

        };

        function dibujar_rotar(nombre_fichero) {

            //Dependiendo de la extensión del fichero
            var fichero = nombre_fichero.toLowerCase();
            var ext = fichero.split('.');
            ext = '.' + ext[ext.length - 1];

            switch (ext) {
                case '.jpg':
                case '.png':
                    $("#img_izquierda").show();
                    $("#img_derecha").show();
                    break;
                default:
                    $("#img_izquierda").hide();
                    $("#img_derecha").hide();
                    break;
            }

        };

    </script>

    <style>

    .panel-left {
        flex: 0 0 auto;
        /* only manually resize */
        padding: 10px;
        width: 250px;
        min-width: 250px;
        white-space: nowrap;
        background: #FFF;
        color: white;
        overflow :auto;
    }

    .panel-right {
        border:1px solid #28a745;
    }

    </style>

 </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container-fluid">

        <h5><span id="recoger" class="material-icons" style="color: #155724;" onmouseover="hand('recoger')" title="Contraer/Expandir menú">swap_horiz</span> IO Almacenamiento</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
      
        <table style="width:100%;">
            <tr>
                <td style="width:250px;"></td>
                <td id="table_pdf">
                <div style="border:1px solid #e2e3e5;">
                    <span class="material-icons" style="font-size:15px;">
                    keyboard_arrow_right
                    </span>
                    <asp:Literal ID="Lt_ruta" runat="server"></asp:Literal>
                </div>
                </td>
            </tr>
        </table>

        <div class="panel-container">

                <div class="panel-left">

                     <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ExpandDepth="0" NodeIndent="15">
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                            NodeSpacing="0px" VerticalPadding="0px"></NodeStyle>
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle Font-Underline="false" HorizontalPadding="5px"
                            VerticalPadding="0px" ForeColor="#28a745" />
                    </asp:TreeView>
                    
                </div> 
            
                <div class="panel-right jctx-host jctx-id-foo">

                    <asp:GridView ID="gridview_consulta"  
                    AutoGenerateColumns="false" runat="server" 
                    cssclass="table-bordered" Font-Size="11px" Width="100%" ToolTip="Listado Ficheros" EmptyDataText="No hay resultados."
                    DataKeyNames="nombre,fecha_modificacion,tipo_fichero,size,pdf"            
                    CellPadding="4" ForeColor="#333333" GridLines="Both" 
                    ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25" PagerSettings-Position="TopAndBottom">
                    <AlternatingRowStyle BackColor="White" />

                    <Columns>
                        
                    <asp:TemplateField HeaderText ="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_seleccionar" runat="server" Enabled ="true" CssClass="check_disabled" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="gvHeaderCenter" />
                        <ItemStyle CssClass="gvHeaderCenter" />
                    </asp:TemplateField>

                    <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40">
                    <ItemTemplate>
                    <asp:ImageButton ID="imgestado" runat="server" tooltip="Tipo de fichero" ImageUrl='<%# Bind("ruta_imagen") %>'
                    CommandName="ver_fichero"
                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                    onclientclick="mostrar_trabajando('Buscando...');"
                    Text="Ver" CssClass="boton" />
                    </ItemTemplate>
                        <HeaderStyle CssClass="gvHeaderCenter" />
                        <ItemStyle CssClass="gvHeaderCenter" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="nombre" HeaderText="Nombre" 
                        SortExpression="nombre" ReadOnly="True">
                    <HeaderStyle CssClass="gvHeaderleft" />
                    <ItemStyle CssClass="gvHeaderleft"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="fecha_modificacion" HeaderText="Fecha" HeaderStyle-Width="120"
                        SortExpression="fecha_modificacion" ReadOnly="True" ItemStyle-HorizontalAlign="left">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="tipo_fichero" HeaderText="Tipo" HeaderStyle-Width="40"
                        SortExpression="tipo_fichero" ReadOnly="True">
                    <HeaderStyle CssClass="gvHeaderCenter" />
                    <ItemStyle CssClass="gvHeaderCenter"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="size" HeaderText="Tamaño" HeaderStyle-Width="70"
                        SortExpression="size" ReadOnly="True">
                    <HeaderStyle CssClass="gvHeaderCenter" />
                    <ItemStyle CssClass="gvHeaderright"></ItemStyle>
                    </asp:BoundField>

                    <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40">
                    <ItemTemplate>
                    <asp:ImageButton ID="img_pdf_masivo" runat="server" tooltip="Enviar a Entradas Masivas de PDF´s" ImageUrl='~/imagenes/web/icono_pdf.png'
                    CommandName="ver_pdf_masivo"
                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                    onclientclick="mostrar_trabajando('Buscando...');"
                    Text="Ver" CssClass="boton" />
                    </ItemTemplate>
                        <HeaderStyle CssClass="gvHeaderCenter" />
                        <ItemStyle CssClass="gvHeaderCenter" />
                    </asp:TemplateField>

                    </Columns>
                   
                    <HeaderStyle BackColor="#d4edda" Font-Bold="True" ForeColor="black" />
                    <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

                </asp:GridView>

                    <ul class="jctx jctx-id-foo jctx-white">
	                <li data-action="new_folder"><span class="material-icons" style="font-size:15px;color:#28a745;">create_new_folder</span>&nbsp; Nueva Carpeta</li>
                    <li data-action="delete_folder"><span class="material-icons" style="font-size:15px;color:#28a745;">folder_delete</span>&nbsp; Eliminar Carpeta</li>
                    <li data-action="rename_folder"><span class="material-icons" style="font-size:15px;color:#28a745;">rule_folder</span>&nbsp; Renombrar Carpeta</li>
                    <hr style="background-color: #28a745;">
                    <li data-action="upload_file"><span class="material-icons" style="font-size:15px;color:#28a745;">file_upload</span>&nbsp; Subir Fichero(s)</li>
                    <li data-action="download_file"><span class="material-icons" style="font-size:15px;color:#28a745;">file_download</span>&nbsp; Descargar Fichero(s)</li>
                    <li data-action="delete_file"><span class="material-icons" style="font-size:15px;color:#28a745;">delete_sweep</span>&nbsp; Eliminar Fichero(s)</li>
                    <li data-action="rename_file"><span class="material-icons" style="font-size:15px;color:#28a745;">drive_file_rename_outline</span>&nbsp; Renombrar Fichero</li>
                    <hr style="background-color: #28a745;">
                    <li data-action="copy_file"><span class="material-icons" style="font-size:15px;color:#28a745;">content_copy</span>&nbsp; Copiar</li>
                    <li data-action="move_file"><span class="material-icons" style="font-size:15px;color:#28a745;">flip_to_front</span>&nbsp; Mover</li>
                    <li data-action="paste_file"><span class="material-icons" style="font-size:15px;color:#28a745;">content_paste_go</span>&nbsp; Pegar</li>
                    </ul>
                    
                </div> 
        
            </div>

        </div> 
 
        <!-- Modal Agregar Carpeta -->
        <div class="modal" id="modal_agregar_carpeta" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Nueva Carpeta</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                         <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Nombre:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_nombre_nueva_carpeta" runat="server" class="form-control" placeholder="Escriba un nombre de carpeta"  maxlength="50"></asp:TextBox>
                        </div>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_grabar_nueva_carpeta" runat="server" CssClass="btn btn-danger" Text="Guardar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Eliminar Carpeta -->
        <div class="modal" id="modal_eliminar_carpeta" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Eliminar Carpeta</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        ¿Esta seguro de querer eliminar la carpeta <asp:Label ID="lbl_carpeta_borrar" runat="server" Text="Label"></asp:Label> ?
                        
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_borrar_carpeta" runat="server" CssClass="btn btn-danger" Text="Eliminar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Rename Carpeta-->
        <div class="modal" id="modal_renombrar_carpeta" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Renombrar Carpeta</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                         <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Nombre:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_rename_fonder" runat="server" class="form-control" placeholder="Escriba un nombre para la carpeta"  maxlength="50"></asp:TextBox>
                        </div>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_rename_carpeta" runat="server" CssClass="btn btn-danger" Text="Guardar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Eliminar fichero -->
        <div class="modal" id="modal_eliminar_fichero" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Eliminar Fichero</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:Label ID="lbl_fichero_borrar" runat="server" Text="Label"></asp:Label>
                       
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_eliminar_fichero" runat="server" CssClass="btn btn-danger" Text="Eliminar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Descargar fichero -->
        <div class="modal" id="modal_descargar_fichero" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Descargar Fichero</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:Label ID="lbl_descargar_fichero" runat="server" Text="Label"></asp:Label>
                       
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_descargar_fichero" runat="server" CssClass="btn btn-danger" Text="Descargar" OnClientClick="$('#modal_descargar_fichero').modal('hide');" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Rename Fichero-->
        <div class="modal" id="modal_rename_fichero" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Renombrar Fichero</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                         <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Nombre:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_rename_fichero" runat="server" class="form-control" placeholder="Escriba un nombre para el fichero"  maxlength="50"></asp:TextBox>
                        </div>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_renombrar_fichero" runat="server" CssClass="btn btn-danger" Text="Guardar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Visualizar Fichero-->
        <div class="modal" id="modal_visualizar_fichero" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog" style="max-width:80%; ">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        
                        <table style="width:100%;height:40px;border:0px solid red;" class="cerrar">
                        <tr>
                            <td><span id="informacion" style="font-size: 14px;"></span></td>
                            <td style="width:35px;"><img id="img_adjuntar_asiento" src="../imagenes/web/Export-01-WF_azul.png" title="Adjuntar al Asiento activo" onmouseover="hand('img_adjuntar_asiento')" /></td>
                            <td style="width:35px;"><img id="img_adjuntar_impuesto" src="../imagenes/web/Export-01-WF_lila.png" title="Adjuntar al Impuesto activo" onmouseover="hand('img_adjuntar_impuesto')" /></td>
                            <td style="width:20px;"></td>
                            <td style="width:35px;"><asp:ImageButton ID="img_izquierda" runat="server" src="../imagenes/web/Portrait-01-WF_270.png" Height="25" alt="Girar imagen a la izquierda" title="Girar imagen a la izquierda" /></td>
                            <td style="width:10px;"></td>
                            <td style="width:35px;"><asp:ImageButton ID="img_derecha" runat="server" src="../imagenes/web/Portrait-01-WF.png" Height="25" alt="Girar imagen a la derecha" title="Girar imagen a la derecha" /></td>
                            <td style="width:25px;"></td>
                            <td style="width:35px;"><span id="imagen_anterior" class="material-icons" onmouseover="hand('imagen_anterior')" title="Imagen Anterior" style="font-size:30px; color: #155724; position:relative; top:5px;">arrow_left</span></td>
                            <td style="text-align: center;width:90px;"><span id="total_imagenes" class="text-muted"><small></small></span></td>
                            <td style="width:35px;"><span id="imagen_posterior" class="material-icons" onmouseover="hand('imagen_posterior')" title="Imagen Posterior" style="font-size:30px; color: #155724; position:relative; top:5px;">arrow_right</span></td>
                            <td style="width:30px;">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                                </button>
                            </td>
                        </tr>
                    </table>
                        
                        
                    </div>
                    <div class="modal-body" style="align-items:center;">

                         <iframe id="ver_documento" style="border:0px;"></iframe>

                    </div>
                   
                </div>
            </div>
        </div>

        <!-- Modal Fichero Adjunto-->
        <div class="modal" id="modal_fichero_adjunto" tabindex="-1" role="dialog">
            <div class="modal-dialog" style="max-width:80%;">
            
                <div class="modal-content">

                    <div class="modal-header">
                        <h3 class="modal-title">Subir Fichero(s)</h3>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>

                    <div class="modal-body">
                        <span class="text-muted" style="color:#0072c6;"><small>Observaciones</small></span>
                        
                        <asp:FileUpload ID="FileUpload" runat="server" class="file" type="file" AllowMultiple="true"></asp:FileUpload>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btn_subir_fichero" runat="server" Text="Aceptar" class="btn btn-danger" /> 
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Adjuntar Asiento-->
        <div class="modal" id="modal_adjuntar_asiento" tabindex="-1" role="dialog">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Adjuntar al Asiento</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        ¿Está seguro de querer adjuntar el fichero al asiento: <asp:Label ID="lbl_adjuntar_asiento" runat="server" Text="Label"></asp:Label>?
                       
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_adjuntar_asiento_si" runat="server" CssClass="btn btn-danger" Text="Adjuntar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Adjuntar Impuesto-->
        <div class="modal" id="modal_adjuntar_impuesto" tabindex="-1" role="dialog">
            <div class="modal-dialog" style="max-width:50%;" >

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Adjuntar al Impuesto</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <table style="width:100%;">
                            <tr>
                                <td>

                                    <table id="tabla" style="width:100%;">
                                    <tr>
                                        <td>¿Adjuntar el fichero al impuesto Normal: <asp:Label ID="lbl_adjuntar_impuesto" runat="server" Text="Label"></asp:Label>?</td>
                                    </tr>
                                    <tr>
                                        <td style="height:70px;" align="center"><asp:Button id="btn_adjuntar_impuesto_si" runat="server" CssClass="btn btn-danger" Text="Adjuntar" /></td>
                                    </tr>
                                    </table>

                                </td>
                                <td>

                                    <table id="tabla_IA" style="width:100%;">
                                    <tr>
                                        <td>¿Adjuntar el fichero al impuesto con IA: <asp:Label ID="lbl_adjuntar_impuesto_IA" runat="server" Text="Label"></asp:Label>?</td>
                                    </tr>
                                    <tr>
                                        <td style="height:70px;" align="center"><asp:Button id="btn_adjuntar_impuesto_si_IA" runat="server" CssClass="btn btn-danger" Text="Adjuntar al IA" /></td>
                                    </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Modal Adjuntar a PDF-->
        <div class="modal" id="modal_enviar_masivo" tabindex="-1" role="dialog">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            Enviar a la Entrada Masiva de PDF´s</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:Literal ID="LT_adjuntar_pdf" runat="server"></asp:Literal>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_adjuntar_pdf" runat="server" CssClass="btn btn-danger" Text="Adjuntar"  OnClientClick ="$('#modal_enviar_masivo').modal('hide');mostrar_trabajando('Enviando a Entrada Masiva de PDFs, por favor espere');"/>
                    </div>
                </div>
            </div>
        </div>
        
        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

        <div id="partes_hidden" style="visibility:hidden; display: none; ">
            txt_ruta:<asp:TextBox ID="txt_ruta" runat="server" Width="100%"></asp:TextBox><br />
            <asp:Button ID="btnUpload" runat="server" Text="Button" /><br />
            txt_informacion:<asp:TextBox ID="txt_informacion" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox><br />
            txt_total:<asp:TextBox ID="txt_total" runat="server" Width="100%"></asp:TextBox><br />
            txt_actual:<asp:TextBox ID="txt_actual" runat="server" Width="100%"></asp:TextBox><br />
            txt_copiar:<asp:TextBox ID="txt_copiar" runat="server" Width="100%"></asp:TextBox><br />
            txt_mover:<asp:TextBox ID="txt_mover" runat="server" Width="100%"></asp:TextBox><br />
            <asp:Button ID="btn_pegar" runat="server" Text="Button" />
            txt_asignar_impuesto:<asp:TextBox ID="txt_asignar_impuesto" runat="server" Width="100%"></asp:TextBox><br />
            txt_asignar_impuesto_IA:<asp:TextBox ID="txt_asignar_impuesto_IA" runat="server" Width="100%"></asp:TextBox><br />
            <asp:Timer ID="Timer1" runat="server" Interval="60000" Enabled="False"></asp:Timer>
        </div>
        
        <script type="text/javascript">

            $("#FileUpload").fileinput({
            showUpload: false,
            language: 'es',
            maxFileSize: 5120,
            allowedFileExtensions: ['pdf','zip','txt','xlsx','png', 'jpg','docx'],
            browseClass: "btn btn btn-outline-success"
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_subir_fichero").on('click', function (e) {

                //CAmpo fileupload
                if ($("#FileUpload").val() == '') {
                    error('No ha seleccionado ningun fichero.')
                    return false;
                }

                //Mensaje al usuario
                mostrar_trabajando('Subiendo Imagen, por favor espere');
            })

        </script>
        
    </form>
</body>
</html>