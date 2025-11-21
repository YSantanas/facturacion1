<%@ Page Language="VB" AutoEventWireup="false" CodeFile="sql.aspx.vb" Inherits="herramientas_sql" %>

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

            //Lanzo el evento
            $("#txt_sentencia").on("keyup", function () {
                //Si pasa de 6 caracteres activo el boton de enviar
                if ($("#txt_sentencia").val().length > 10) {
                    
                    //activar el boton para la cnsulta
                    $('#btn_consultar').attr('disabled',false)
                }
            })

            //Cambio de contenedor
            $(".panel-container").css('height', $(window).height() - 90 + 'px');
            $("#contenedor_treeview").css('height', $(window).height() - 120 + 'px');

            //Resize Panel
            $(".panel-left").resizable({
                handleSelector: ".splitter",
                resizeHeight: false
            });

            //Control para focus
            $("#btn_consultar").focusin(function () {
                $("#btn_consultar").click();
            });

            //Control para el click
            $("#btn_consultar").on('click', function () {

                //Mensaje al usuario
                mostrar_trabajando('Realizando consulta SQL, por favor espere.');

            })

            //Control para el click
            $("#recoger").on('click', function (e) {

                $(".panel-left").toggle(500);

            })

        });

        //Tamaño para el desktop
        $(window).resize(function (e) {

            $(".panel-container").css('height', $(window).height() - 90 + 'px');

            

            $("#contenedor_treeview").css('height', $(window).height() - 120 + 'px');
      
        })

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
       
        <h5><span id="recoger" class="bi-arrow-left-right text-primary" style="font-size:16px;" onmouseover="hand('recoger')" title="Contraer/Expandir menú"></span> SQL</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
       
            <div class="panel-container">

                <div class="panel-left ">
                
                    <div id="contenedor_treeview" style="overflow-x: hidden; overflow-y: scroll; width: 99%;">
                        <asp:TreeView ID="TV_tablass" runat="server" ForeColor="Black" ShowLines="True" ShowExpandCollapse="True" ExpandDepth="0"></asp:TreeView>
                    </div>

                </div>

                <div class="splitter"></div>
            
                <div class="panel-right">

                    <div class="form-group">
                      <asp:TextBox ID="txt_sentencia" runat="server" class="form-control rounded-0" TextMode="MultiLine" Rows="6"></asp:TextBox>
                    </div>

                    <br />
                    <div class="container-fluid" style="text-align:center">
                        <asp:Button ID="btn_consultar" runat="server" CssClass="btn btn-outline-primary btn-sm" Text="Realizar Consulta" Enabled ="false" />
                    </div>


                <div class="container-fluid" style="text-align:center">
                    <table style="width :100%;border:0px solid red;">
                    <tr>
                        <td style="width:50%;border: 0px solid blue; text-align:left;">
                            <asp:LinkButton ID="img_exportar_excel" runat="server" CssClass ="bi bi-filetype-xls text-primary" Font-Size="20" height="25" ToolTip="Descargar Excel" TabIndex ="-1" Visible="false"></asp:LinkButton>
                            <asp:LinkButton ID="img_exportar_txt" runat="server" CssClass ="bi bi-filetype-txt text-primary"  Font-Size="20" height="25" ToolTip="Descargar TXT" TabIndex ="-1" Visible="false"></asp:LinkButton>
                        </td>
                    
                        <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span></td>
                    </tr>
                    </table>
                </div>
              
                    <div class="table-responsive">

                    <asp:GridView ID="gridview_consulta"  
                    AutoGenerateColumns="true" runat="server" 
                    cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta SQL" EmptyDataText="No hay resultados."
                    CellPadding="4" ForeColor="#333333" GridLines="Both" 
                    ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
                    <AlternatingRowStyle BackColor="White" />
            
                    <HeaderStyle CssClass ="table-primary"/>
                    <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

                    </asp:GridView>

                    </div>

                </div>

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
