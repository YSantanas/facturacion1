<%@ Page Language="VB" AutoEventWireup="false" CodeFile="personalizar_usuario.aspx.vb" Inherits="configuracion_personalizar_usuario" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <title></title>

    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Content/alertify.core.css" rel="stylesheet" />
    <link href="../Content/alertify.default.css" rel="stylesheet" />
    <link href="../Content/bootstrap-icons.css" rel="stylesheet" />
    <link href="../Content/jquery-ui.css" rel="stylesheet" />

    <!-- JQUERY ------------------------------------------------------------------------------------------------->
    <script src="../Scripts/jquery-3.7.1.js"></script>
    <script src="../Scripts/bootstrap.js"></script>
    <script src="../Scripts/alertify.js"></script>
    <script src="../Scripts/shortcut.js"></script>
    <script src="../Scripts/jquery-ui-1-13.3.js"></script>
    <script src="../Scripts/device.js"></script>
    

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link href="../Content/interior.css" rel="stylesheet" />
    <script src="../Scripts/interior.js"></script>
    <link href="../Content/fileinput.css" rel="stylesheet" />
    <script src="../Scripts/fileinput.js"></script>
    <script src="../Scripts/locales/es.js"></script>
    <script src="../Scripts/bootstrap-strength.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        
        <table id="regresar" onclick="volver_ventana();" onmouseover="hand(this.id);">
        <tr>
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #007bff;"></span>Menú Configuración</td>
        </tr>
        </table>

        <div class="container-fluid">

        <h5>Personalizar Usuario</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <br />

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Nombre:
                    </span>
                </div>
                <asp:TextBox ID="txt_nombre_usuario" runat="server" class="form-control" placeholder="Nombre"  maxlength="25"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Primer Apellido:
                    </span>
                </div>
                <asp:TextBox ID="txt_primer_apellido_usuario" runat="server" class="form-control" placeholder="Primer Apellido"  maxlength="15"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Segundo Apellido:
                    </span>
                </div>
                <asp:TextBox ID="txt_segundo_apellido_usuario" runat="server" class="form-control" placeholder="Segundo Apellido"  maxlength="15"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        E-mail:
                    </span>
                </div>
                <asp:TextBox ID="txt_codigo_usuario" runat="server" class="form-control" placeholder="Email"  maxlength="30"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        PassWord:
                    </span>
                </div>
                <asp:TextBox ID="txt_paswword_usuario" runat="server" class="form-control" placeholder="PassWord" maxlength="20"></asp:TextBox>
            </div>

            <br />
            <table style="width:100%;border-spacing: 10px;border-collapse: separate;" class="img-thumbnail">
                <tr>
                    <td style="width:260px;" align="center"><asp:Literal ID="Lt_foto" runat="server"></asp:Literal></td>
                    <td style="width:20px;"></td>
                    <td><asp:FileUpload ID="FileUpload_logo" runat="server" class="file" type="file"></asp:FileUpload></td>
                </tr>
                <tr>
                    <td colspan ="3"><asp:CheckBox ID="chk_tema_original" runat="server" AutoPostBack ="true" /> Aplicar Tema por Defecto</td>
                </tr>
            </table>

            <br />
            <div class="container-fluid" style="text-align:center">
                <asp:Button ID="btn_grabar" runat="server" CssClass="btn btn-outline-primary btn-xs" Text="Actualizar" />
            </div>

        </div>

        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

        <!-- Modal -->
        <div class="modal" id="modal_eliminar_clave" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Advertencia</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">

                        <label id="nombre_usuario_eliminar" style="font-weight: normal;"></label><strong data-name=""></strong>
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-outline-primary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_grabar_confirmar" runat="server" CssClass="btn btn-outline-danger" Text="Aceptar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Eliminar-->
        <div class="modal" id="modal_eliminar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
          <div class="modal-dialog">

	        <div class="modal-content">

		        <div class="modal-header">
                    <h1 class="modal-title fs-5">Aviso</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
		        <div class="modal-body">
                    <asp:Literal ID="lt_mensaje_eliminar" runat="server"></asp:Literal>
                    <br />
		        </div>
		        <div class="modal-footer">
                    <button type="button" class="btn btn-outline-primary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btn_si_eliminar" runat="server" Text="Aplicar" CssClass="btn btn-outline-danger"/>
                 </div>
	        </div>
          </div>
        </div>

        <div id="partes_hidden" style="visibility:hidden; display: none;">
        txt_codigo_usuario_antiguo:<asp:TextBox ID="txt_codigo_usuario_antiguo" runat="server" Width="400"></asp:TextBox><br />
        txt_paswword_usuario_antiguo:<asp:TextBox ID="txt_paswword_usuario_antiguo" runat="server" Width="400"></asp:TextBox><br />
        </div>

         <script type="text/javascript">

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

            $("#FileUpload_logo").fileinput({
                showUpload: false,
                language: 'es',
                allowedFileExtensions: ['jpg'],
                browseClass: "btn btn btn-outline-primary"
            });

            //Barra para mostrar el nivel de seguridad de la clave
            $("#txt_paswword_usuario").bootstrapStrength({
	        active: true,
	        slimBar: true,
	        meterClasses: {
		        weak: "progress-bar bg-primary",
		        medium: "progress-bar bg-primary",
		        good: "progress-bar bg-primary"
	        }
            });

            //Control para focus
            $("#btn_grabar").focusin(function () {
                $("#btn_grabar").click();
            });

            //Control para el click
            $("#btn_grabar").on('click', function () {

                if ($("#txt_nombre_usuario").val() == '') {
                    error('El campo Nombre no puede estar vacío.')
                    setTimeout(function () { $("#txt_nombre_usuario").focus(); }, 100);
                    return false;
                }

                if ($("#txt_primer_apellido_usuario").val() == '') {
                    error('El campo Primer Apellido no puede estar vacío.')
                    setTimeout(function () { $("#txt_primer_apellido_usuario").focus(); }, 100);
                    return false;
                }

                if ($("#txt_codigo_usuario").val() == '') {
                    error('El campo E-mail no puede estar vacío.')
                    setTimeout(function () { $("#txt_codigo_usuario").focus(); }, 100);
                    return false;
                } else {
                    if ($("#txt_codigo_usuario").val().indexOf('@', 0) == -1 || $("#txt_codigo_usuario").val().indexOf('.', 0) == -1) {
                        error('El E-mail introducido no es correcto.')
                        setTimeout(function () { $("#txt_codigo_usuario").focus(); }, 100);
                        return false;
                    }
                    //Comparo si ha cambiado el correo
                    if ($("#txt_codigo_usuario").val() != $("#txt_codigo_usuario_antiguo").val()) {
                        // Lanzo la confirmacion
                        $("#nombre_usuario_eliminar").text("Recuerde que el Email: " + $("#txt_codigo_usuario_antiguo").val() + " es el nombre de inicio de sesión, ¿Está seguro de que desea modificarlo por " + $("#txt_codigo_usuario").val() + "?");
                        $("#modal_eliminar_clave").modal('show');
                        return false;
                    }
                }

                if ($("#txt_paswword_usuario").val() == '') {
                    error('El campo PassWord no puede estar vacío.')
                    setTimeout(function () { $("#txt_paswword_usuario").focus(); }, 100);
                    return false;
                } else {
                    //Comparo si ha cambiado el password
                    if ($("#txt_paswword_usuario").val() != $("#txt_paswword_usuario_antiguo").val()) {
                        // Lanzo la confirmacion
                        $("#nombre_usuario_eliminar").text("Recuerde que el PassWord es la contraseña necesaria para acceder a la aplicación, ¿Está seguro de que desea modificarla por: " + $("#txt_paswword_usuario").val() + "?");
                        $("#modal_eliminar_clave").modal('show');
                        return false;
                    }
                }

                //Mensaje al usuario
                mostrar_trabajando('Actualizando datos, por favor espere.');

            })

            //Notificación de actualizacion de direccion en Notificaciones
            function actualizar_foto(ruta) {
                 
                //Asigno
                padre = $(window.parent.document);
                padre.find('#img_logo').remove();

                //Evalúo
                if (ruta == 'sin_foto') {
                    padre.find('#div_logo').append("<span id='img_logo' class='bi bi-person-bounding-box' style='font-size:25px;'></span>");
                } else {
                    padre.find('#div_logo').append(ruta);
                }

             };

            function volver_ventana() {

                //Declaro
                padre = $(window.parent.document);
                var id_usuario = $(padre).find("#txt_id_usuario").val();
                var id_empresa = $(padre).find("#txt_id_empresa").val();
                var ruta = "configuracion/configuracion.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;

                //Informo de resolución de pantalla baja, para PC
                if (device.mobile() === false && device.tablet() === false) {

                    $(padre).find("#iconfiguracion_").attr('src', ruta);
                
                } else {

                    $(padre).find("#iframe").attr('src', ruta);

                }

             }

    </script>

    </form>
</body>
</html>
