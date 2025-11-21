<%@ Page Language="VB" AutoEventWireup="false" CodeFile="datos_activos.aspx.vb" Inherits="herramientas_datos_activos" %>

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
               
        <h5>Sesión Activa</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>

        <div class="card">
            <div class="card-header">
            Cookies Activas
            </div>
            <div class="card-body">

                <asp:Literal ID="LT_cookies" runat="server"></asp:Literal>

            </div>
        </div>
        <br />

        <div class="card">
            <div class="card-header">
            Sesiones Activas
            </div>
            <div class="card-body">

                <div class="table-responsive">
                    <asp:Literal ID="LT_sesiones" runat="server"></asp:Literal>
                    <asp:Panel ID="PL_sesion" runat="server"></asp:Panel>
                </div>

            </div>
        </div>

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

        </div> 

    </form>
</body>
</html>
