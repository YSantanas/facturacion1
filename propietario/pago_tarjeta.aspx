<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pago_tarjeta.aspx.vb" Inherits="propietario_pago_tarjeta" %>

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

             //Asigno alto
            $("#ver_factura_embed").css("height", $(window).height() - 100) 

        });
       
    </script>

    <style>

        .label {
            color: black;
        }

         .pdf {
            position: relative;
            width: 100%;
            height: 90%;
            border: 1px solid #155724;
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
         }

    </style>

</head>
<body>
    <form action="https://sis.redsys.es/sis/realizarPago" method="post" runat="server">
    
        <div class="container-fluid">
        
        <h5>Pago con Tarjeta</h5>
        <div class="posicion_hora"><span style="color: black;"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></span></div>
        <br />
        <br />

            <table style="width:100%;">
                <tr>
                    <td style="width:35%;" valign="top">
                        <div class="card">
                            <div class="card-header">
                            <h5>Se va a realizar el pago de la siguiente factura:</h5>
                            </div>
                            <div class="card-body">
                        
                                <table style="font-size: 25px; border:0px solid red;" class="text-secondary;">
                                <tr>
                                    <td>Fecha:</td>
                                    <td style="width:20px;"></td>
                                    <td style="text-align:right;"><asp:Label ID="lbl_fecha" runat="server" Text="Label" CssClass ="label"></asp:Label></td>
                                </tr>
                                    <tr>
                                    <td>Nº Factura:</td>
                                    <td style="width:20px;"></td>
                                    <td style="text-align:right;"><asp:Label ID="lbl_factura" runat="server" Text="Label" CssClass ="label"></asp:Label></td>
                                </tr>
                                    <tr>
                                    <td>Importe:</td>
                                    <td style="width:20px;"></td>
                                    <td style="text-align:right;"><asp:Label ID="lbl_importe" runat="server" Text="Label" CssClass ="label"></asp:Label></td>
                                </tr>
                                </table>

                                <hr class="my-4">
                                <input id="pagar" runat="server" type="submit" class="btn btn-outline-success btn-lg" value="Realizar Pago"/>
                
                            </div>
                        </div>
                    </td>
                    <td style="width:5%;" valign="top">
                    </td> 
                    <td style="width:60%;" valign="top">
                        <iframe id="ver_factura_embed" class="pdf" visible="false" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" runat="server"></iframe>
                    </td> 
                </tr>
            </table>
          
        </div>
         
        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />
        
        <div id="partes_hidden" style="visibility:hidden; display: none;">
        <asp:TextBox id="Ds_SignatureVersion" runat="server" Width="400"></asp:TextBox><br />
        <asp:TextBox id="Ds_MerchantParameters" runat="server" Width="400"></asp:TextBox><br />
        <asp:TextBox id="Ds_Signature" runat="server" Width="400"></asp:TextBox><br />
        </div>
        
    </form>
</body>
</html>
