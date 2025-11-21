<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="facturas.aspx.vb" Inherits="facturas" %>

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

            //Control para el click
            $(".tree").on('click', function () {

                //Mensaje al usuario
                mostrar_trabajando('Generando factura, por favor espere.');

            })

            //Asigno alto
            $("#ver_factura_embed").css("height", $(window).height() - 50) 

        })

        //Tamaño para el Modal cuando redimensiona la ventana
        $(window).resize(function (e) {

            if ($("#ver_factura_embed").is(":visible") == true) {
                //Asigno alto
                $("#ver_factura_embed").css("height", $(window).height() - 50)
            }
           
        });

    </script>

    <style>

        #TreeView1 :link,:visited,:hover,:active {
            text-decoration: none;
            color: #155724;
        }

        .pdf {
            position: relative;
            width: 100%;
            height: 90%;
            border: 1px solid #155724;
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
        }

        .tree {
            font-size: 14px !important;
            border: 0px solid #b3b3b3 !important ;
        }
        
        .pasarela_pago {
            position: absolute;
            width: 80%;
            height: 90%;
            border: 2px solid red;
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
        }

        .fondo_completo_negro {
            position: fixed;
            top: 0px; 
            bottom: 0px;
            left: 0px; 
            right: 0px; 
            background-color: black; 
            z-index:1000;
            opacity:0.8;
            filter:alpha(opacity=80);
        }
        
        .cuadro_alerta_PDF {
            position: fixed;
            width: 90%;
            background-color: transparent; 
    
            /*height: 200px;*/
            left: 50%;
            margin-left: -45%;
            bottom: +1%;
            z-index:1001;
            /*-moz-box-shadow: inset 0 -2px 2px #dadada;
	        -webkit-box-shadow:inset  0 -2px 2px #dadada;
	        box-shadow: inset 0 -2px 2px #dadada;*/
        }

        .iframe_pdf { 
            position: relative ;
            width: 100%;
            height: 90%;
            border: 2px solid #2E9C07; 
            border-top-left-radius:4px;  
	        border-top-right-radius:4px;
        }

    </style>

 </head>
<body>
    <form id="form1" runat="server">
        
        <div class="container-fluid">
        
        <h5>Facturas</h5>
        <div class="posicion_hora"><span style="color: black;"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></span></div>

        <div class="row">
            <div class="col-12 col-sm-4">
                
                <div class="table-responsive">
                <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" EnableTheming="False" CssClass="tree"
                    ShowExpandCollapse="true" >
                </asp:TreeView>
                </div>

            </div>
            <div class="col-12 col-sm-8">
            
                <asp:PlaceHolder ID="PH_tarjeta" runat="server" Visible ="false">
                <table style="" >
                <tr>
                    <td><span class="material-icons" style="color: #155754; font-size:80px;">credit_card</span></td>
                    <td style="width:20px;"></td>
                    <td><asp:LinkButton ID="LB_pagar" runat="server" ForeColor="#155724">Pagar con Tarjeta de Crédito / Débito.</asp:LinkButton></td>
                </tr>
                </table>
                <br />
                </asp:PlaceHolder>

                <iframe id="ver_factura_embed" class="pdf" visible="false" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" runat="server"></iframe>

            </div>
        </div>

        </div>

    <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />
        
    <div id="partes_hidden" style="visibility:hidden; display: none;">
        txt_seleccion_factura:<asp:TextBox ID="txt_seleccion_factura" runat="server" Width="400"></asp:TextBox><br />
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

    <asp:Panel ID="Panel_Mostrar_PDF" runat="server" Visible="false">
    <br />
    <div class="fondo_completo_negro"></div>
        <div id="menu" class="cuadro_alerta_PDF" style="top: 1%;">
            <div id="boton_cerrar" style="text-align : right;"><asp:ImageButton ID="Img_cerrar" runat="server" AlternateText="X" ForeColor="Red" ImageUrl="~/imagenes/web/close.png" /></div>
            <iframe id="Iframe_pago" class="iframe_pdf" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" runat="server"></iframe>
        </div>
    </asp:Panel>

    </form>
</body>
</html>

