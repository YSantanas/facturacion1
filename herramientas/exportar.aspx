<%@ Page Language="VB" AutoEventWireup="false" CodeFile="exportar.aspx.vb" Inherits="herramientas_exportar" %>

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

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_si_aviso").on('click', function (e) {

                //Mensaje al usuario
                $('#modal_aviso').modal('hide');
                mostrar_trabajando('Exportando tabla(s) seleccionada(s), por favor espere.');

            })

        });

        function volver_ventana() {

            //Declaro
            padre = $(window.parent.document);
            var id_usuario = $(padre).find("#txt_id_usuario").val();
            var id_empresa = $(padre).find("#txt_id_empresa").val();
            var ruta = "herramientas/herramientas.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;

            //Informo de resolución de pantalla baja, para PC
            if (device.mobile() === false && device.tablet() === false) {

                $(padre).find("#iherramientas").attr('src', ruta);

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
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #0d6efd;"></span>Menú Herramientas</td>
        </tr>
        </table>
         
        <div class="container-fluid">

        <h5>Copia de Seguridad (Manual)</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <br />

        <asp:CheckBox ID="CB_all" runat="server" CssClass="text-primary" Text="&nbsp;Seleccionar Todo" Font-Size="10" AutoPostBack="True" />

        <div class="table-responsive">

        <table style="width:100%; border: 1px solid #b3b3b3;">
        <tr>
            <td style="width:200px;vertical-align: text-top">
                <asp:TreeView ID="TV_tablas" runat="server" ShowCheckBoxes="All" ShowLines="True" EnableTheming="False" CssClass="custom-checkbox" Font-Size="9"></asp:TreeView>
            </td>
            <td align="center" valign="top" style="padding-left:10px;padding-right:10px;">
            <span style="color:#808080;font-size:12px;">Seleccione las tablas que desea exportar y generará un archivo ZIP que podrá guardar en su equipo. Este proceso puede tardar dependiendo de la conexión a Internet.<br /><br /></span>
            <asp:Button ID="btn_exportar" runat="server" Text="Exportar Tabla(s)" CssClass="btn btn-outline-primary btn-sm" OnClientClick="mostrar_trabajando('Exportando tabla(s) seleccionada(s), por favor espere.');" />
                       
            <asp:PlaceHolder ID="PH_comprimido" runat="server" Visible ="false">
                <br />
                <br />
                <br />
                <br />
                <table>
                <tr>
                    <td width="90" align="center"><span class="bi bi-file-zip" style="font-size :40px; color:#0d6efd"></span></td>
                    <td align="center">

                        <span style="font-size: 12px; color:#808080;">
                        Pulse 
                        <asp:HyperLink ID="HL_enlace" runat="server" CssClass="enlaces_envios">
                        <span style='color: #0d6efd; font-weight: 400;'>Aquí </span>
                        </asp:HyperLink>para descargar el fichero <br />
                        <asp:Label ID="lbl_etiqueta" runat="server" Text=""></asp:Label>
                        <br />en su equipo. </span>
                    </td>
                </tr>
                </table>
            </asp:PlaceHolder>

        </td>
        </tr>
        </table>

        </div>

        </div>

    <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />
        
    <!-- Modal Sin Acceso-->
    <div class="modal modal-static fade" id="modal_sin_acceso" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-body">

                    <table style="width:100%;">
                        <tr>
                            <td align="center">
                                <span class="material-icons" style="font-size:60px;color:red;">person_off</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px;"></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <h5><span style="color:#155724">Su nivel de clave no permite acceder a este menú.</span></h5>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>

    </form>
</body>
</html>