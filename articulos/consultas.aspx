<%@ Page Language="VB" AutoEventWireup="false" CodeFile="consultas.aspx.vb" Inherits="articulos_consultas" %>

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
                mostrar_trabajando('Consultando Artículos, por favor espere.');
                
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
        
        <h5>Consultas de Artículos</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <div class="table-responsive">
        <table style="width: 100%">
            <tr>
                <td width="70" align="center"><small>Cod.Artículo</small></td>
                <td width="250" align="center"><small>Denominación</small></td>
                <td width="180" align="center"><small>Código Barras</small></td>
                <td width="100" align="center"><small>Precio</small></td>
                <td width="100" align="center"><small>% Descuento</small></td>
                <td width="" align="center"><small>Observaciones</small></td>
                <td align="center"><small></small></td>
            </tr>
            <tr>
                <td align="right"><asp:TextBox ID="txt_cod_articulo" runat="server" CssClass="form-control input-sm textbox_cod_articulos" MaxLength="7" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_denominacion" runat="server" CssClass="form-control input-sm textbox_denominacion_articulos" MaxLength="200" width="99%" Style="text-transform: uppercase"></asp:TextBox> </td>
                <td align="right"><asp:TextBox ID="txt_cod_barras" runat="server" CssClass="form-control input-sm" MaxLength="15" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_precio" runat="server" CssClass="form-control input-sm" MaxLength="7" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_dto_1" runat="server" CssClass="form-control input-sm" MaxLength="7" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td align="right"><asp:TextBox ID="txt_observaciones" runat="server" CssClass="form-control input-sm" MaxLength="100" width="99%" Style="text-transform: uppercase"></asp:TextBox></td>
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
                 <td width="70" align="center"><small>Cod.Artículo</small></td>
                 <td width="250" align="center"><small>Denominación</small></td>
                 <td width="180" align="center"><small>Código Barras</small></td>
                 <td width="70" align="center"><small>Precio</small></td>
                 <td width="100" align="center"><small>% Descuento</small></td>
                 <td width="" align="center"><small>Observaciones</small></td>
            </tr>
            </table>

        </div>
            
            <asp:GridView ID="gridview_consulta"  
            AutoGenerateColumns="false" runat="server" 
            cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta de Artículos" EmptyDataText="No hay resultados."
            CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="id,fecha_creacion,hora_creacion,denominacion,cod_barras,activo,precio,dto_1,observaciones" 
            ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
            <AlternatingRowStyle BackColor="White" />
           
            <Columns>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>

                <asp:LinkButton ID ="lk_articulo" runat ="server" 
                    CssClass ="bi bi-person-lines-fill "
                    
                    Font-Size="25px"
                    ToolTip ="Ir a Artículos" TabIndex="-1" 
                    CommandName="ver_articulo"
                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ForeColor="#28a745" />
            </ItemTemplate>
                <HeaderStyle CssClass="gvHeaderCenter" />
                <ItemStyle CssClass="gvHeaderCenter" Width ="40" />
            </asp:TemplateField>

            <asp:BoundField DataField="id" HeaderText="Cod.Artículo" HeaderStyle-Width="80"
                SortExpression="id" ReadOnly="True" DataFormatString="{0:d}">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left" Width="80"></ItemStyle>
            </asp:BoundField>
                    
            <asp:BoundField DataField="denominacion" HeaderText="Denominación" HeaderStyle-Width="180"
                SortExpression="denominacion" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="180"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="cod_barras" HeaderText="Código Barras" HeaderStyle-Width="100"
                SortExpression="cod_barras" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="precio" HeaderText="Precio" HeaderStyle-Width="100"
               SortExpression="precio" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="dto_1" HeaderText="% Descuento" HeaderStyle-Width="100"
                SortExpression="dto_1" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="100"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" HeaderStyle-Width="280"
                 SortExpression="" ReadOnly="True" ItemStyle-HorizontalAlign="left">
             <HeaderStyle HorizontalAlign="Left" />
             <ItemStyle HorizontalAlign="Left" Width ="280"></ItemStyle>
             </asp:BoundField>

            <%--<asp:BoundField DataField="" HeaderText="" 
                SortExpression="" ReadOnly="True" ItemStyle-HorizontalAlign="center">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle CssClass="gvHeaderCenter" />
            </asp:BoundField>--%>

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