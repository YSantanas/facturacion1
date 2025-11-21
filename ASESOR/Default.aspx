<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="ASESOR_Default" %>

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

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link href="../Content/interior.css" rel="stylesheet" />
    <script src="../Scripts/interior.js"></script>

    <script type="text/javascript">

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

            //Asigno alto
            $("#idesktop").css("height", $(window).height() - 8)
            $("#menu").css("height", $(window).height() - 8)
            

        });

        //Tamaño para el desktop
        $(window).resize(function (e) {

            $("#idesktop").css("height", $(window).height() - 8)

        })

        </script>

    <style>

        body {
            color: white;
        }

    </style>
    


</head>
<body id="body_contenedor" runat ="server">
    <form id="form1" runat="server">

        <table style="width:100%;">
        <tr>
            <td style="width:15%" valign="top">

                <table id="menu" style="width:100%;">
                <tr>
                    <td style="height:80px;background-color : rgba(255,255,255,0.80); padding-left :30px;" ><img src="../Imagenes/logo/logo_empresa.png" style="height:30px;margin-left:10px;margin-top:10px;" /></td>
                </tr>
                <tr>
                    <td class="bg-dark" style="padding :10px;">
                        
                        <%--<a href="facturas_emitidas.aspx">  Emitidas </a>--%>

                    </td>
                </tr>
                </table>

            </td>
            <td>

                <iframe id="idesktop" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;" runat ="server"></iframe>
                
            </td>
        </tr>
        </table>
       
        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

        <div id="partes_hidden" style="visibility:hidden; display:none; ">
            txt_id_empresa:<asp:TextBox ID="txt_id_empresa" runat="server" Width="100%"></asp:TextBox><br />
            txt_id_usuario:<asp:TextBox ID="txt_id_usuario" runat="server" Width="100%"></asp:TextBox><br />
        </div>

    </form> 
</body>
</html>