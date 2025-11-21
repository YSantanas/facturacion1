<%@ Page Language="VB" AutoEventWireup="false" CodeFile="consultas.aspx.vb" Inherits="proveedores_consultas" %>

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
    <script src="../Scripts/js_proveedor.js"></script>
    <script src="../Scripts/tinymce.min.js"></script>
    
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
                mostrar_trabajando('Consultando Proveedores, por favor espere.');
                
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
        
        <h5>Consultas de Proveedores</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <div class="table-responsive">
        <table style="width: 100%">
            <tr>
                <td width="70" align="center"><small>Cod.Proveedor</small></td>
                <td width="180" align="center"><small>Nombre</small></td>
                <td width="100" align="center"><small>N.I.F.</small></td>
                <td width="280" align="center"><small>Dirección</small></td>
                <td width="80" align="center"><small>Cod. Postal</small></td>
                <td width="70" align="center"><small>Sucursales</small></td>
                <td width="280" align="center"><small>Contacto</small></td>
                <td width="250" align="center"><small>Tlf, Email, etc</small></td>
                <td width="250" align="center"><small>Observaciones</small></td>
                <td align="center"><small></small></td>
            </tr>
            <tr>
                <td align="right"><asp:TextBox ID="txt_cod_cliente" runat="server" CssClass="form-control input-sm" MaxLength="7" width="95%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_nombre" runat="server" class="form-control input-sm"  MaxLength="200" width="95%" Style="text-transform: uppercase"></asp:TextBox> </td>
                <td align="center"><asp:TextBox ID="txt_nif" runat="server" CssClass="form-control input-sm" MaxLength="15" width="95%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_direccion" runat="server" CssClass="form-control input-sm" MaxLength="250" width="95%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_cp" runat="server" CssClass="form-control input-sm" MaxLength="5" width="95%" Style="text-transform: uppercase;"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_sucursal" runat="server" CssClass="form-control input-sm" MaxLength="8" width="95%" Style="text-transform: uppercase;"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_contacto" runat="server" CssClass="form-control input-sm" Width="95%" MaxLength ="200" Style="text-transform: uppercase;"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_tlf" runat="server" CssClass="form-control input-sm" Width="95%" MaxLength ="10" Style="text-transform: uppercase;"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_observaciones" runat="server" CssClass="form-control input-sm" Width="95%" MaxLength ="10" Style="text-transform: uppercase;"></asp:TextBox></td>
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
                </td>
    
                <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span></td>
            </tr>
            </table>

        </div>
            
       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        --%>
        <div class="table-responsive">

        <div id="ver" style="position:fixed;top:0px;background-color:#cce5ff;color:#333333;font-weight:700; width:100%;display:none;">

            <table border="0" style="font-size:12px;">
            <tr>
                 <td width="70" align="center"><small>Cod.Proveedor</small></td>
                 <td width="180" align="center"><small>Nombre</small></td>
                 <td width="100" align="center"><small>N.I.F.</small></td>
                 <td width="280" align="center"><small>Dirección</small></td>
                 <td width="100" align="center"><small>Cod. Postal</small></td>
                 <td width="80" align="center"><small>Sucursales</small></td>
                 <td width="280" align="center"><small>Contactos</small></td>
                 <td width="250" align="center"><small>Tlf, Email, etc</small></td>
                 <td align="center"><small></small></td>
            </tr>
            </table>

        </div>
            
            <asp:GridView ID="gridview_consulta"  
            AutoGenerateColumns="false" runat="server" 
            cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta de Proveedores" EmptyDataText="No hay resultados."
            CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="id_cliente,activo,nombre_fiscal,nif,calle_cliente,numero_cliente,cp_cliente,observaciones_cliente,id_sucursal,activo_sucursal,calle_sucursal,numero_sucursal,cp_sucursal,observaciones_sucursal, id_cliente_contacto, id_sucursal_contacto, nombre, primer_apellido, segundo_apellido, contacto, observaciones_contacto" 
            ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
            <AlternatingRowStyle BackColor="White" />
           
            <Columns>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>

                <asp:LinkButton ID ="lk_cliente" runat ="server" 
                    CssClass ="bi bi-person-lines-fill "
                    
                    Font-Size="25px"
                    ToolTip ="Ir a Proveedores" TabIndex="-1" 
                    CommandName="ver_cliente"
                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ForeColor="#6F42C1" />
            </ItemTemplate>
                <HeaderStyle CssClass="gvHeaderCenter" />
                <ItemStyle CssClass="gvHeaderCenter" Width ="40" />
            </asp:TemplateField>

            <asp:BoundField DataField="id_cliente" HeaderText="Cod. Cliente" HeaderStyle-Width="80"
                SortExpression="id" ReadOnly="True" DataFormatString="{0:d}">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left" Width="80"></ItemStyle>
            </asp:BoundField>
                    
            <asp:BoundField DataField="nombre_fiscal" HeaderText="Nombre" HeaderStyle-Width="180"
                SortExpression="nombre_fiscal" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="180"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="nif" HeaderText="N.I.F." HeaderStyle-Width="100"
                SortExpression="nif" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="" HeaderText="Dirección" HeaderStyle-Width="280"
                 SortExpression="" ReadOnly="True" ItemStyle-HorizontalAlign="left">
             <HeaderStyle HorizontalAlign="Left" />
             <ItemStyle HorizontalAlign="Left" Width ="280"></ItemStyle>
             </asp:BoundField>

            <asp:BoundField DataField="" HeaderText="Cod. Postal" HeaderStyle-Width="100"
                SortExpression="" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>
            
            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>
                 <asp:LinkButton ID ="lk_sucursal" runat ="server" visible="false" 
                     CssClass ="bi bi-file-earmark-break" 
                     Font-Size="25px"
                     ToolTip ="Ir al Sucursales" TabIndex="-1" 
                     CommandName="ver_sucursal"
                     CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ForeColor="#6F42C1" />
             </ItemTemplate>
                 <HeaderStyle CssClass="gvHeaderCenter" />
                 <ItemStyle CssClass="gvHeaderCenter" Width ="40" />
             </asp:TemplateField>

            <asp:BoundField DataField="" HeaderText="Sucursal" HeaderStyle-Width="80"
                SortExpression="" ReadOnly="True" >
            <HeaderStyle CssClass="gvHeaderCenter"  />
            <ItemStyle CssClass="gvHeaderCenter" Width ="80" />
            </asp:BoundField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>
                 <asp:LinkButton ID ="lk_contacto" runat ="server" visible="false" 
                     CssClass ="bi bi-columns" 
                     Font-Size="25px"
                     ToolTip ="Ir al Contactos" TabIndex="-1" 
                     CommandName="ver_contacto"
                     CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ForeColor="#6F42C1" />
             </ItemTemplate>
                 <HeaderStyle CssClass="gvHeaderCenter" />
                 <ItemStyle CssClass="gvHeaderCenter" Width ="40" />
             </asp:TemplateField>

            <asp:BoundField DataField="" HeaderText="Contacto" HeaderStyle-Width="280"
                SortExpression="nombre" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="280"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="" HeaderText="Enlace" HeaderStyle-Width="280"
                SortExpression="" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="280"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="" HeaderText="" 
                SortExpression="" ReadOnly="True" ItemStyle-HorizontalAlign="center">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle CssClass="gvHeaderCenter" />
            </asp:BoundField>

            </Columns>
                   
                <HeaderStyle CssClass ="table-primary"/>
                <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

        </asp:GridView>

        </div>

       <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>

        </div> 
       
        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

        <div id="partes_hidden" style="visibility:hidden; display:none; ">
            txt_fecha_inicial:<asp:TextBox ID="txt_fecha_inicial" runat="server" Width="400"></asp:TextBox><br />
            txt_fecha_final:<asp:TextBox ID="txt_fecha_final" runat="server" Width="400"></asp:TextBox><br />
        </div>

    </form>

</body>
</html>