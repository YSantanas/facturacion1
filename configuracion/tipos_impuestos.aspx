<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tipos_impuestos.aspx.vb" Inherits="configuracion_tipos_impuestos" %>

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

            //Eliminamos el click derecho de ratón para las multiples pantallas
            $('html').bind("contextmenu", function (e) {
                return false;
            });

            //Control para el click
            $("#img_nueva").on('click', function () {

                //Mensaje al usuario
                mostrar_trabajando('Abriendo menú, por favor espere.');

            })

            //Control para focus
            $("#btn_grabar").focusin(function () {
                $("#btn_grabar").click();
            });

            //Control para el click
            $("#btn_grabar").on('click', function () {

                if ($("#txt_porcentaje").val() == '') {
                    $('#modal_agregar').modal('show');
                    error('El campo Porcentaje no puede estar vacío.')
                    setTimeout(function () { $("#txt_porcentaje").focus(); }, 100);
                    return false;
                }

                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Agregando tipo de impuesto, por favor espere.');

            })

              //Control para focus
            $("#btn_modificar").focusin(function () {
                $("#btn_modificar").click();
            });

            //Control para el click
            $("#btn_modificar").on('click', function () {

                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Modificando tipo de impuesto, por favor espere.');

            })

            //Control para el click
            $(".lanzar_mensaje").on('click', function () {

                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Modificando tipo de impuesto, por favor espere.');

            })

        });

        function volver_ventana() {

            //Declaro
            padre = $(window.parent.document);
            var id_usuario = $(padre).find("#txt_id_usuario").val();
            var id_empresa = $(padre).find("#txt_id_empresa").val();
            var ruta = "configuracion/configuracion.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;

            //Informo de resolución de pantalla baja, para PC
            if (device.mobile() === false && device.tablet() === false) {

                $(padre).find("#iconfiguracion_").attr('src', ruta);
                
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
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #007bff;"></span>Menú Configuración</td>
        </tr>
        </table>

        <div class="container-fluid">

        <h5>Tipos de Impuestos</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <br />

        <div class="container-fluid" style="text-align:center">
            <table style="width :100%;border:0px solid red;">
            <tr>
                <td style="width:50%;border: 0px solid blue; text-align:left;">
                    <asp:LinkButton ID="img_exportar_excel" runat="server" CssClass ="bi bi-filetype-xls text-primary" Font-Size="20" height="25" ToolTip="Descargar Excel" TabIndex ="-1" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="img_exportar_txt" runat="server" CssClass ="bi bi-filetype-txt text-primary"  Font-Size="20" height="25" ToolTip="Descargar TXT" TabIndex ="-1" Visible="false"></asp:LinkButton>
                </td>
                    
                <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span><asp:LinkButton ID="img_nueva" runat="server" CssClass="bi bi-file-earmark-plus text-primary" Font-Size="20" style="height:25px; margin-left: 10px;position:relative;" ToolTip="Nuevo"/></td>
            </tr>
            </table>
        </div>

        <div class="table-responsive">

        <asp:GridView ID="gridview_consulta"  
        AutoGenerateColumns="false" runat="server" 
        cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Tipos de Impuestos" EmptyDataText="No hay resultados."
        CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="Id,nombre,porcentaje,impuesto_defecto,activo"
        ShowHeaderWhenEmpty="false" AllowPaging="false" PageSize="25">
        <AlternatingRowStyle BackColor="White" />
            
            <Columns>

            <asp:BoundField DataField="nombre" HeaderText="Tipo" HeaderStyle-Width="140px"
                SortExpression="nombre" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="porcentaje" HeaderText="%" HeaderStyle-Width="140px"
                SortExpression="porcentaje" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:TemplateField HeaderText ="% Defecto">
                <ItemTemplate>
                    <asp:CheckBox ID="chk1" runat="server" AutoPostBack="True" checked='<%# Convert.ToBoolean(Eval("impuesto_defecto"))%>' OnCheckedChanged="chk1_CheckedChanged" CssClass="lanzar_mensaje" />
                </ItemTemplate>
                <HeaderStyle CssClass="gvHeaderleft" />
                <ItemStyle CssClass="gvHeaderleft" />
            </asp:TemplateField>

           <asp:BoundField DataField="" HeaderText=""
                SortExpression="" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="105">
                <ItemTemplate>
                    <asp:Button ID="btnconsultar" 
                    ControlStyle-CssClass="btn btn-outline-secondary btn-sm" runat="server" 
                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                    CommandName="consultar" Text="Consultar"/>
                </ItemTemplate>
                <HeaderStyle Width="105px" />
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
            <ItemTemplate>
                <asp:Button ID="btnedit" 
                ControlStyle-CssClass="btn btn-outline-success btn-sm" runat="server" 
                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                CommandName="editar" Text="Editar"/>
            </ItemTemplate>
                <HeaderStyle Width="80px" />
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
            <ItemTemplate>
                <asp:Button ID="btndelete" 
                ControlStyle-CssClass="btn btn-outline-danger btn-sm" runat="server" 
                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                CommandName="borrar" Text="Eliminar"/>
            </ItemTemplate>
                <HeaderStyle Width="80px" />
            </asp:TemplateField>
            
            </Columns>
                   
                <HeaderStyle CssClass ="table-primary"/>
                <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

        </asp:GridView>

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
                                <span class="bi bi-person-fill-slash" style="font-size:60px;color:red;"></span>
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

        <!-- Modal Eliminar-->
        <div class="modal" id="modal_eliminar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">Eliminar Tipo de Impuesto</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Literal ID="LT_mensaje" runat="server"></asp:Literal>
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-outline-primary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_eliminar_confirmar" runat="server" CssClass="btn btn-outline-danger" Text="Aceptar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Agregar -->
        <div class="modal" id="modal_agregar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5"><asp:Label ID="lbl_titulo" runat="server" Text="..."></asp:Label></h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Tipo:
                                </span>
                            </div>
                            <asp:DropDownList ID="DDL_tipo" runat="server" class="form-control"></asp:DropDownList>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Porcentaje:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_porcentaje" runat="server" class="form-control" placeholder="Escriba el %"  maxlength="5"></asp:TextBox>
                        </div>
                        
                        <div class="row">
                            <div class="col-sm-12 text-left">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chk_activo" runat="server" />
                                    <label class="custom-control-label" for="chk_defecto" style="color:gray; font-size: 13px;">Activo</label>
                                </div>
                            </div>
                        </div>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_grabar" runat="server" CssClass="btn btn-primary" Text="Guardar" />
                        <asp:Button id="btn_modificar" runat="server" CssClass="btn btn-success" Text="Modificar" />
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