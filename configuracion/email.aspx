<%@ Page Language="VB" AutoEventWireup="false" CodeFile="email.aspx.vb" Inherits="configuracion_email" %>

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
    
    <script>

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
           
            //Control para el click
            $("#btn_actualizar").on('click', function () {

                if ($("#txt_email").val() == '') {
                    error('El campo Email no puede estar vacío.')
                    setTimeout(function () { $("#txt_email").focus(); }, 100);
                    return false;
                }

                if ($("#txt_password").val() == '') {
                    error('El campo Password no puede estar vacío.')
                    setTimeout(function () { $("#txt_password").focus(); }, 100);
                    return false;
                }

                if ($("#txt_smtp").val() == '') {
                    error('El campo Servidor Salida no puede estar vacío.')
                    setTimeout(function () { $("#txt_smtp").focus(); }, 100);
                    return false;
                }

                 if ($("#txt_port").val() == '') {
                    error('El campo Puerto no puede estar vacío.')
                    setTimeout(function () { $("#txt_port").focus(); }, 100);
                    return false;
                }

                //Mensaje al usuario
                $('#modal_confirmar').modal('hide');
                mostrar_trabajando('Comprobando Datos, por favor espere.');

            })

        });

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

</head>
<body>
    <form id="form1" runat="server">
        
        <table id="regresar" onclick="volver_ventana();" onmouseover="hand(this.id);">
        <tr>
            <td style="height:50px;"><span class="bi bi-arrow-bar-left text-primary" style="margin-left:20px;margin-right:10px;"></span>Menú Herramientas</td>
        </tr>
        </table>

        <div class="container-fluid">

        <h5>E-mail</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <br />

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Email:
                    </span>
                </div>
                <asp:TextBox ID="txt_email" runat="server" class="form-control" placeholder="E-mail"  maxlength="200"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Contraseña:
                    </span>
                </div>
                <asp:TextBox ID="txt_password" runat="server" class="form-control" placeholder="Contraseña"  maxlength="50"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                       Servidor Salida:
                    </span>
                </div>
                <asp:TextBox ID="txt_smtp" runat="server" class="form-control" placeholder="SMTP, IMAP, etc"  maxlength="50"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Puerto:
                    </span>
                </div>
                <asp:TextBox ID="txt_port" runat="server" class="form-control" placeholder="Puerto de salida del E-mail"  maxlength="5"></asp:TextBox>
            </div>

            <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        SSL:
                    </span>
                </div>
                <asp:DropDownList ID="DDL_ssl" runat="server" class="form-control"></asp:DropDownList>
            </div>

            <br />
            <div class="container-fluid" style="text-align:center">
                <asp:Button ID="btn_actualizar" runat="server" CssClass="btn btn-outline-primary btn-xs" Text="Actualizar" />
            </div>

        </div>

        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

        <!-- Modal -->
        <div class="modal" id="modal_confirmar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Advertencia</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="LT_mensaje" runat="server" Text="Label"></asp:Label>
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_grabar_confirmar" runat="server" CssClass="btn btn-danger" Text="Aceptar" />
                    </div>
                </div>
            </div>
        </div>
       
    </form>
</body>
</html>