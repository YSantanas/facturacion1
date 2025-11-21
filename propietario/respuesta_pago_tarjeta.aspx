<%@ Page Language="VB" AutoEventWireup="false" CodeFile="respuesta_pago_tarjeta.aspx.vb" Inherits="propietario_respuesta_pago_tarjeta" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link href="../Content46/bootstrap.css" rel="stylesheet" />
    <link href="../Content46/alertifyjs/alertify.min.css" rel="stylesheet" />

    <!-- JQUERY-------------------------------------------------------------------------------------------------->    
    <script src="../Scripts46/jquery-3.6.0.js"></script>
    <script src="../Scripts46/bootstrap.js"></script>
    <script src="../Scripts46/alertify.js"></script>
    <script src="../Scripts46/shortcut.js"></script>
    <script src="../Scripts46/device.js"></script>

    <!-- PERSONAL---------------------------------------------------------------------------------------------------->     
    <link href="../Content46/interior.css" rel="stylesheet" />
    <script src="../Scripts46/interior.js"></script>

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

        });
       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    
        <div class="container-fluid">
        
        <h5>Pago con Tarjeta</h5>
        <div class="posicion_hora"><span style="color: black;"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></span></div>
        <br />
        <br />
            
        <table style="width:100%;">
        <tr>
            <td align="center">
                <table>
                <tr>
                    <td align="center" valign="top">
                        <asp:Literal ID="LT_imagen" runat="server"></asp:Literal>
                    </td>
                    <td style="width:50px;"></td>
                    <td style="height:50px;" align="right">
                        <asp:Literal ID="LT_mensaje" runat="server"></asp:Literal>
                        <br />
                        <br />
                        <br />
                        <img src="../Imagenes/logo/logo_iocc.png" style="height:40px;"/>
                        <p class="lead">Gracias por confiar en nosotros.</p>
                    </td>
                </tr>
                </table>
            </td>
        </tr>
        </table> 

        </div> 

    </form>
</body>
</html>
