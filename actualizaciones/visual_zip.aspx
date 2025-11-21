<%@ Page Language="VB" AutoEventWireup="false" CodeFile="visual_zip.aspx.vb" Inherits="actualizaciones_visual_zip" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="../Content46/bootstrap.css" rel="stylesheet" />

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

</head>
<body>
    <form id="form1" runat="server">
         
        <div class="container-fluid">
            <asp:Literal ID="LT_ficheros" runat="server"></asp:Literal>
        </div>

    </form>
</body>
</html>
