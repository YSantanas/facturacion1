<%@ Page Language="VB" AutoEventWireup="false" CodeFile="facturas_emitidas.aspx.vb" Inherits="informes_facturas_emitidas" %>

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
    <script src="../Scripts/js_cod_clientes.js"></script>
    <script src="../Scripts/js_denominacion_clientes.js"></script>
    <script src="../Scripts/js_cod_articulos.js"></script>
    <script src="../Scripts/js_denominacion_articulos.js"></script>
    <script src="../Scripts/tinymce.min.js"></script>
    <script src="../Scripts/jquery.maskedinput.js"></script>
    <script src="../Scripts/js_fecha_formato.js"></script>
   
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

            // EVENTO CUANDO SE MUEVE EL SCROLL
	        $(window).scroll(function(event){
                var posicionScroll = $(this).scrollTop();
                if (posicionScroll > 260) {
                          $("#ver").css("display","");
	   	        } else {
	    	        $("#ver").css("display","none");
	   	        }
	        });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el Focus
            $("#btn_consultar").on('focus', function (e) {
                //click sobre grabar cabecera
                $("#btn_consultar").click();
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_consultar").on('click', function (e) {
               
                //Mensaje al usuario
                mostrar_trabajando('Consultando Facturas, por favor espere.');
                
            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#img_descargar").on('click', function (e) {
               
                //Mensaje al usuario
                mostrar_trabajando('Generando Facturas, por favor espere.');
                
            })
          
        });
         
        </script>

    <style>
        
        .input-sm {
            font-size:13px; 
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         
        <div class="container-fluid">
        
        <h5>Facturas Emitidas</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <div class="table-responsive">
        
            <table style="width: 100%">
            <tr>
                <td width="100" align="center"><small>Fecha Inicial</small></td>
                <td width="100" align="center"><small>Fecha Final</small></td>
                <td width="100" align="center"><small>Cod. Cliente</small></td>
                <td align="center"><small>Cliente</small></td>                                       
                <td width="100" align="center"><small>N.I.F.</small></td>
                <td width="100" align="center"><small>Nº Factura</small></td>
                <td width="100" align="center"><small>Total</small></td>
                <td align="center"><small></small></td>
            </tr>
            <tr>
                <td align="right"><asp:TextBox ID="txt_fecha_inicial" runat="server" CssClass="form-control input-sm textbox_fecha" MaxLength="15" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_fecha_final" runat="server" CssClass="form-control input-sm textbox_fecha" MaxLength="15" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_cod_cliente" runat="server" CssClass="form-control input-sm textbox_cod_cliente" MaxLength="7" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_cliente" runat="server" class="form-control input-sm textbox_denominacion_cliente"  MaxLength="250" width="99%" Style="text-transform: uppercase"></asp:TextBox> </td>              
                <td align="right"><asp:TextBox ID="txt_nif" runat="server" CssClass="form-control input-sm" MaxLength="15" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_n_factura" runat="server" CssClass="form-control input-sm" MaxLength="150" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_total" runat="server" CssClass="form-control input-sm" MaxLength="150" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="center"><small></small></td>
            </tr>
        </table>
        </div> 
        <p></p>
       
        <div class="container-fluid" style="text-align:center">
            
            <asp:Button ID="btn_consultar" runat="server" Text="Buscar" CssClass="btn btn-outline-primary btn-sm"/>
            
            <table style="width :100%;border:0px solid red;">
            <tr>
                <td style="width:50%;border: 0px solid blue; text-align:left;">
                    <asp:LinkButton ID="img_exportar_excel" runat="server" CssClass ="bi bi-filetype-xls text-primary" Font-Size="20" height="25" ToolTip="Descargar Excel" TabIndex ="-1" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="img_exportar_txt" runat="server" CssClass ="bi bi-filetype-txt text-primary"  Font-Size="20" height="25" ToolTip="Descargar TXT" TabIndex ="-1" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="img_descargar" runat="server" CssClass ="bi bi-download text-primary"  Font-Size="20" height="25" ToolTip="Descargar Facturas" TabIndex ="-1" Visible="false"></asp:LinkButton>
                </td>
    
                <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span></td>
            </tr>
            </table>

        </div>
            
     
        <div class="table-responsive">

            <div id="ver" style="position:fixed;top:0px;background-color:#cce5ff;color:#333333;font-weight:700; width:100%;display:none;">

                <table border="0" style="font-size:12px;">
                <tr>                 
                     <td width="100" align="center"><small>Fecha Inicial</small></td>
                    <td width="100" align="center"><small>Fecha Final</small></td>
                    <td width="100" align="center"><small>Cod. Cliente</small></td>
                    <td align="center"><small>Cliente</small></td>                                       
                    <td width="100" align="center"><small>N.I.F.</small></td>
                    <td width="100" align="center"><small>Nº Factura</small></td>
                    <td width="100" align="center"><small>Total</small></td>
                    <td align="center"><small></small></td>
                </tr>
                </table>

            </div>
            
            <asp:GridView ID="gridview_consulta"  
            AutoGenerateColumns="false" runat="server" 
            cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta de Facturas" EmptyDataText="No hay resultados."
            CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="n_factura,fecha_creacion,hora_creacion,fecha,cod_cliente,cliente,nif,total,n_descargado" 
            ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
            <AlternatingRowStyle BackColor="White" />
           
            <Columns>

            <asp:BoundField DataField="n_descargado" HeaderText="Nº Descargas" HeaderStyle-Width="80"
                SortExpression="n_descargado" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderCenter"  />
            <ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
            </asp:BoundField>    

            <asp:TemplateField HeaderText ="Descargar" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:CheckBox ID="chk_marcar" runat="server" />
                </ItemTemplate>
                <ItemStyle Width="90px" />
                <HeaderStyle CssClass="gvHeaderCenter" />
                <ItemStyle CssClass="gvHeaderCenter" />
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>

                <asp:LinkButton ID ="lk_articulo" runat ="server" 
                    CssClass ="bi bi-columns-gap "
                    Font-Size="25px"
                    ToolTip ="Ir a Facturas" TabIndex="-1" 
                    CommandName="ver_factura"
                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ForeColor="#fd7e14" />
            </ItemTemplate>
                <HeaderStyle CssClass="gvHeaderCenter" />
                <ItemStyle CssClass="gvHeaderCenter" Width ="40" />
            </asp:TemplateField>

            <asp:BoundField DataField="fecha" HeaderText="Fecha" HeaderStyle-Width="100"
                SortExpression="fecha" ReadOnly="True" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>
            
            <asp:BoundField DataField="cod_cliente" HeaderText="Cod. Cliente" HeaderStyle-Width="80"
                SortExpression="cod_cliente" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left" Width="100"></ItemStyle>
            </asp:BoundField>        
                    
            <asp:BoundField DataField="cliente" HeaderText="Cliente" HeaderStyle-Width="180"
                SortExpression="cliente" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="180"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="nif" HeaderText="N.I.F." HeaderStyle-Width="100"
                SortExpression="nif" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="n_factura" HeaderText="Nº Factura" HeaderStyle-Width="100"
                SortExpression="n_factura" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>
                               
            <asp:BoundField DataField="total" HeaderText="Total" HeaderStyle-Width="100"
                SortExpression="total" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>

            </Columns>
                   
                <HeaderStyle CssClass ="table-primary"/>
                <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

        </asp:GridView>

        </div>

        </div> 
       
        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

        <!-- Modal Descargar-->
        <div class="modal" id="modal_descargar" tabindex="-1" role="dialog">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Descargar</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        
                        <table>
                <tr>
                    <td width="90" align="center"><span class="bi bi-file-zip text-primary" style="font-size :40px;"></span></td>
                    <td align="center">

                        <span style="font-size: 12px; color:#808080;">
                        Pulse 
                        <asp:HyperLink ID="HL_enlace" runat="server" CssClass="enlaces_envios">
                        <span class="text-primary" font-weight: 400;'>Aquí </span>
                        </asp:HyperLink>para descargar el fichero <br />
                        <asp:Label ID="lbl_etiqueta" runat="server" Text=""></asp:Label>
                        <br />en su equipo. </span>
                    </td>
                </tr>
                </table>

                    </div>
                   
                </div>
            </div>
        </div>

        <div id="partes_hidden" style="visibility:hidden; display:none; ">
            <%--txt_fecha_inicial:<asp:TextBox ID="txt_fecha_inicial" runat="server" Width="400"></asp:TextBox><br />
            txt_fecha_final:<asp:TextBox ID="txt_fecha_final" runat="server" Width="400"></asp:TextBox><br />--%>
        </div>

    </form>

</body>
</html>
