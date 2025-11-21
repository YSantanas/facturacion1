<%@ Page Language="VB" AutoEventWireup="false" CodeFile="repetitivas.aspx.vb" Inherits="facturas_repetitivas" %>

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

             //-------------------------------------------------------------------------------------------------------------------
            //Control para grabar
            $('#btn_add_facturas').on('click', function (e) {
                
                //Mensaje al usuario
                mostrar_trabajando('Añadiendo Factura(s), por favor espere.');

            });

            //Asigno alto
            $("#PL_facturas").css("height", $(window).height() - 80)
            $("#scroll_GV_Consulta").css("height", $(window).height() - 265)
            $("#PL_facturas_perfil").css("height", $(window).height() - 230)
            $("#scroll_GV_perfiles").css("height", $(window).height() - 250)
           
        });

        //Tamaño para el desktop
        $(window).resize(function (e) {

            //Asigno alto
            $("#PL_facturas").css("height", $(window).height() - 80)
            $("#scroll_GV_Consulta").css("height", $(window).height() - 265)
            $("#PL_facturas_perfil").css("height", $(window).height() - 230)
            $("#scroll_GV_perfiles").css("height", $(window).height() - 250)
            
        })

    </script>

    <style>

        .input-sm {
            font-size:12px; 
        }

         td { 
            padding: 2px;
         }

    </style>

</head>
<body>
    <form id="form1" runat="server">

    <div class="container-fluid" style="position :absolute; top:0px;left:0px;right:0px;bottom:0px;z-index:1;">

        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
   
        <table style="width: 100%;">
        <tr>
            <td style="width: 400px;">
                <h5>Facturas Repetitivas</h5></td>
            <td></td>
            <td style="width: 40px;">
                <%--<asp:ImageButton ID="img_nuevo" runat="server" ToolTip="Nueva Factura (Ctrl + A)" ImageUrl="~/imagenes/web/nuevo.png" Width="25" TabIndex="-1"/>--%>
            </td>
            <td style="width: 40px;">
                <%--<asp:ImageButton ID="img_abonar" runat="server" ToolTip="Abonar Factura (Ctrl + M)" ImageUrl="~/imagenes/web/abono.png" Width="25" TabIndex="-1"/>--%>
            </td>
            <td style="width: 40px;">
                <%--<asp:ImageButton ID="img_duplicar" runat="server" ToolTip="Duplicar Factura (Ctrl + E)" ImageUrl="~/imagenes/web/duplicar.png" Width="25" TabIndex="-1"/>--%>
            </td>
            <td style="width: 190px; text-align: right;"><span class="text-muted"><small><asp:Label ID="lbl_tecla" runat="server" Text=""></asp:Label></small></span></td>
        </tr>
        </table>
        <br />

        <table style="width:100%;">
        <tr>
            <td valign="top" style="width:50%;">

                <asp:Panel ID="PL_facturas" runat="server" Enabled="true" CssClass="borde_entradas"> 
                    
                    <div class="table-responsive">
                    <table style="width: 100%">
                    <tr>
                        <td width="100" align="center"><small>Fecha</small></td>
                        <td width="100" align="center"><small>Cod. Cliente</small></td>
                        <td width="350" align="center"><small>Cliente</small></td>                                       
                        <td width="100" align="center"><small>N.I.F.</small></td>
                        <td width="100" align="center"><small>Nº Factura</small></td>
                        <td width="70" align="center"><small>Total</small></td>
                        <td align="center"><small></small></td>
                    </tr>
                    <tr>
                        <td align="right"><asp:TextBox ID="txt_fecha" runat="server" CssClass="form-control input-sm" MaxLength="15" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                        <td align="right"><asp:TextBox ID="txt_cod_cliente" runat="server" CssClass="form-control input-sm textbox_cliente" MaxLength="7" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                        <td align="right"><asp:TextBox ID="txt_cliente" runat="server" class="form-control input-sm"  MaxLength="250" width="99%" Style="text-transform: uppercase"></asp:TextBox> </td>              
                        <td align="right"><asp:TextBox ID="txt_nif" runat="server" CssClass="form-control input-sm" MaxLength="15" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                        <td align="right"><asp:TextBox ID="txt_n_factura" runat="server" CssClass="form-control input-sm" MaxLength="150" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                        <td align="right"><asp:TextBox ID="txt_total" runat="server" CssClass="form-control input-sm" MaxLength="150" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                        <td align="center"><small></small></td>
                    </tr>
                    </table>
                    </div> 
                    <p></p>

                    <div class="container-fluid" style="text-align:center;">

                        <asp:Button ID="btn_consultar" runat="server" Text="Buscar" CssClass="btn btn-outline-primary btn-sm"/>

                        <table style="width :100%;border:0px solid red;">
                        <tr>
                            <td style="width:50%;border: 0px solid blue; text-align:left;">
                                <asp:LinkButton ID="img_exportar_excel" runat="server" CssClass ="bi bi-filetype-xls text-primary" Font-Size="20" height="25" ToolTip="Descargar Excel" TabIndex ="-1" Visible="false"></asp:LinkButton>
                                <asp:LinkButton ID="img_exportar_txt" runat="server" CssClass ="bi bi-filetype-txt text-primary"  Font-Size="20" height="25" ToolTip="Descargar TXT" TabIndex ="-1" Visible="false"></asp:LinkButton>
                            </td>
    
                            <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span></td>
                        </tr>
                        </table>

                        <div id="scroll_GV_Consulta" style="width:100%;overflow: auto;">
                        
                        <asp:GridView ID="gridview_consulta"  
                        AutoGenerateColumns="false" runat="server" 
                        cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta de Facturas" EmptyDataText="No hay resultados."
                        CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="n_factura,fecha_creacion,hora_creacion,fecha,cod_cliente,cliente,nif,total,perfil_repetitivo" 
                        ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="10">
                        <AlternatingRowStyle BackColor="White" />
           
                        <Columns>
                        
                        <asp:TemplateField ShowHeader="False" HeaderText ="" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_marcar" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
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

                </asp:Panel> 

            </td> 
            <td align="center" valign="top" style="width:80px;padding:10px;">
                
                <br /><br /><br />
                <asp:Button ID="btn_add_facturas" runat="server" Text="Añadir" CssClass="btn btn-outline-primary btn-sm"/>

            </td> 
            <td valign="top" style="width:50%;">

                <asp:Panel ID="PL_perfil" runat="server" Enabled="true" CssClass="borde_entradas"> 

                <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Perfil:
                    </span>
                </div>
                <asp:DropDownList ID="DDL_perfil" runat="server" CssClass="form-control" AutoPostBack ="true"></asp:DropDownList>
                <div class="text-primary" style="position:absolute; top:35px;font-size: 12px;height:20px;">
                    <asp:Label ID="lbl_DDL_perfil" runat="server" Text=""></asp:Label>
                </div>
                <span class="input-group-text" id="basic-addon2">
                    <asp:LinkButton ID="add_perfil" runat="server" CssClass="bi bi-plus text-primary" Font-Size="13" ToolTip="Añadir Perfil" TabIndex ="-1"></asp:LinkButton>
                    <asp:LinkButton ID="minus_perfil" runat="server" CssClass="bi bi-dash text-danger" Font-Size="13" ToolTip="Eliminar Perfil" TabIndex ="-1"></asp:LinkButton>
                </span>

                </div>
                
                </asp:Panel> 
                <br />
                <asp:Panel ID="PL_facturas_perfil" runat="server" Enabled="true" CssClass="borde_entradas">

                    <div id="scroll_GV_perfiles" style="width:100%;overflow: auto;">

                        <asp:GridView ID="gridview_consulta_perfil"  
                    AutoGenerateColumns="false" runat="server" 
                    cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta de Facturas" EmptyDataText="No hay resultados."
                    CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="n_factura,fecha_creacion,hora_creacion,fecha,cod_cliente,cliente,nif,total,perfil_repetitivo,origen,fecha_repetitivo" 
                    ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
                    <AlternatingRowStyle BackColor="White" />
           
                    <Columns>
                        
                    <asp:TemplateField ShowHeader="False" HeaderText ="" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_marcar" runat="server" Checked="True" AutoPostBack="True" OnCheckedChanged="chk_marcar_CheckedChanged"/>
                        </ItemTemplate>
                        <ItemStyle Width="40px" />
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

                    <asp:BoundField DataField="fecha_repetitivo" HeaderText="Próxima Fecha Facturación" HeaderStyle-Width="100"
                        SortExpression="fecha_repetitivo" ReadOnly="True" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="left">
                    <HeaderStyle HorizontalAlign="Left" CssClass="bg-danger-subtle"  />
                    <ItemStyle HorizontalAlign="Left" Width ="100" CssClass="bg-danger-subtle"></ItemStyle>
                    </asp:BoundField>

                    </Columns>
                   
                        <HeaderStyle CssClass ="table-primary"/>
                        <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

                    </asp:GridView>
    
                    </div> 

                </asp:Panel> 
                <br />
                <div class="container-fluid" style="text-align:center;">
                            
                    <asp:Button ID="btn_facturar" runat="server" Text="Facturar" CssClass="btn btn-outline-primary btn-sm" Visible ="false"/>

                </div> 

            </td> 
        </tr>
        </table>
   
        </div>

        <!-- Modal Agregar Perfil -->
        <div class="modal" id="modal_agregar" tabindex="-1" role="dialog" data-bs-backdrop="static" >
            <div class="modal-dialog" style="max-width:70%; ">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Agregar un Perfil</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="modal-body">

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Nombre:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_nombre" runat="server" class="form-control input-sm" placeholder="Escriba un Nombre"  maxlength="100"></asp:TextBox>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Facturación:
                                </span>
                            </div>
                            <asp:DropDownList ID="DDL_facturacion" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Value="Mensual">Mensual</asp:ListItem>
                                <asp:ListItem Value="Bimensual">Bimensual</asp:ListItem>
                                <asp:ListItem Value="Trimestral">Trimestral</asp:ListItem>
                                <asp:ListItem Value="Semestral">Semestral</asp:ListItem>
                                <asp:ListItem Value="Anual">Anual</asp:ListItem>
                            </asp:DropDownList>
                         </div>
                        
                    </div> 
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_grabar_perfil" runat="server" CssClass="btn btn-primary" Text="Guardar" />
                    </div>

                </div>

            </div>

        </div>

         <!-- Modal Eliminar Perfil-->
        <div class="modal" id="modal_eliminar" tabindex="-1" role="dialog" data-bs-backdrop="static" >
            <div class="modal-dialog" style="max-width:70%; ">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Eliminar un Perfil</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="modal-body">

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Nombre:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_nombre_eliminar" runat="server" class="form-control input-sm" placeholder="Escriba un Nombre"  maxlength="100" ReadOnly ="true"></asp:TextBox>
                        </div>

                    </div> 
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_eliminar_perfil" runat="server" CssClass="btn btn-danger" Text="Eliminar" />
                    </div>

                </div>

            </div>

        </div>

        <!-- Modal Eliminar Facturas-->
        <div class="modal" id="modal_eliminar_factura" tabindex="-1" role="dialog">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Eliminar Factura del Perfil</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Literal ID="LT_mensaje_eliminar" runat="server"></asp:Literal>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_eliminar_confirmar" runat="server" CssClass="btn btn-danger" Text="Aceptar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Confirmación Facturación-->
        <div class="modal" id="modal_confirmar_facturacion" tabindex="-1" role="dialog">
            <div class="modal-dialog">
            
                <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Confirmar Facturación</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                        
                    <div class="modal-body">
                        ¿Esta seguro de querer realizar la Facturación seleccionada?
                        <asp:HiddenField ID="HiddenField3" runat="server" />
                        <asp:HiddenField ID="HiddenField4" runat="server" />
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_facturar_confirmar" runat="server" CssClass="btn btn-danger" Text="Facturar" />
                    </div>

                </div>
            </div>
        </div>

        <div id="partes_hidden" style="visibility:hidden; display: none;">
            txt_index:<asp:TextBox ID="txt_index" runat="server" Width="400"></asp:TextBox><br />
        </div>

    </form>
</body>
</html>
