<%@ Page Language="VB" AutoEventWireup="false" CodeFile="claudia.aspx.vb" Inherits="actualizaciones_noticiario" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="refresh" content="5" />

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
    
    <style>

        body{
            background-color:white;
            font-size: 14px;
        }

        .cambiar_azul:hover {
            box-shadow: 1px 1px 10px -1px rgba(0,123,255,0.78);
            -webkit-box-shadow: 1px 1px 10px -1px rgba(0,123,255,0.78);
            -moz-box-shadow: 1px 1px 10px -1px rgba(0,123,255,0.78);
            border: 10px solid #d4edda;
            cursor: pointer;
        }
        
    </style>

    <script>

        $(document).ready(function () {

            //Eliminamos el click derecho de ratón para las multiples pantallas
            $('html').bind("contextmenu", function (e) {
                return false;
            });
       
        });

        //Cambia el puntero del raton por una mano de enlace
        function hand(idDiv) {
            $("#" + idDiv).css({ 'cursor': 'pointer' });
        }

    </script>
   
</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid" style="position:relative; top:5px;">
        
            <asp:Literal ID="Lt_noticiario" runat="server" ></asp:Literal>

        </div> 
        
    </form>
</body>
</html>
