<%@ Page Language="VB" AutoEventWireup="false" CodeFile="visualizador.aspx.vb" Inherits="actualizaciones_visualizador" %>

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

    <!-- PERSONAL---------------------------------------------------------------------------------------------------->     
    <link href="../Content46/interior.css" rel="stylesheet" />
    <script src="../Scripts46/interior.js"></script>

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

         });

    </script>

    <style>

        .cuadro_alerta_PDF {
            position: fixed;
            width: 90%;
            background-color: transparent;
            left: 50%;
            margin-left: -45%;
            bottom: +1%;
            z-index: 1001;
        }

        
        .fondo_completo_negro {
            position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            background-color: white;
            z-index: 1000;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }

        .iframe_pdf {
            position: relative;
            width: 100%;
            height: 90%;
            border: 2px solid #28a745;
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
        }

        #HL_enlace:link {
            color: #28a745;
            text-decoration:none ;
        }

        
        #HL_enlace:visited {
            color:#28a745;
            text-decoration:none ;
        }

        #HL_enlace:hover {
            color:#28a745;
            text-decoration:none ;
        }

        #HL_enlace:active {
            color:#28a745;
            text-decoration:none ;
        }

        .iframe_pdf {

            top: 10px;
            position:absolute;
        }

        #tb_descargar {

            position: fixed;
            right:3%;
            top:30px;
            z-index: 1001;
            width: 100px;
            height: 70px;
            border: 2px solid #28a745;
            border-radius: 0px;
            background-color: #fff;
        }
      
    </style>

</head>
<body>
    <form id="form1" runat="server">
    
        <div class="container-fluid">
            
        <asp:Panel ID="Panel_Mostrar_PDF" runat="server" Visible="true">
        <br />
        <div class="fondo_completo_negro"></div>
            <div id="menu" class="cuadro_alerta_PDF" style="top: 5%;">
                
                <br />
                <iframe id="ver_factura" class="iframe_pdf" visible="true" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0" runat="server"></iframe>
            </div>

            <table id="tb_descargar">
                <tr>
                    <td style="text-align :center;">
                        <img id="img_logo" Height="30" runat ="server" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align :center;"><span style="font-size: 11px; "><asp:HyperLink ID="HL_enlace" runat="server" CssClass="enlaces_envios" download>Descargar</asp:HyperLink></span></td>
                </tr>
            </table>

        </asp:Panel>

        </div>

        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />
       
    </form>
</body>
</html>
