<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tickets.aspx.vb" Inherits="actualizaciones_tickets" validateRequest="false" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

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
    <script src="../Scripts/js_articulos.js"></script>
    <script src="../Scripts/tinymce.min.js"></script>

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

            ////Eliminamos el click derecho de ratón para las multiples pantallas
            //$('html').bind("contextmenu", function (e) {
            //    return false;
            //});

            tinyMCE.init({
                mode: "textareas",
                setup: function(ed) {
                    if ($('#'+ed.id).prop('readonly')) {
                        ed.settings.readonly = true;
                    }
                },
                width: "100%",
                height: 150,
                theme: 'modern',
                plugins: [
            "advlist autolink autosave link image lists charmap print preview hr anchor pagebreak spellchecker",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            "table contextmenu directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern"
                ],

                toolbar1: "bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | forecolor backcolor | formatselect fontselect fontsizeselect",
                toolbar2: "cut copy paste | searchreplace | bullist numlist | outdent indent blockquote | undo redo | insertdatetime preview",
                toolbar3: "table | hr removeformat | subscript superscript | charmap | print fullscreen | ltr rtl | visualchars visualblocks nonbreaking",

                menubar: false,
                toolbar_items_size: 'small',

                style_formats: [{
                    title: 'Bold text',
                    inline: 'b'
                }, {
                    title: 'Red text',
                    inline: 'span',
                    styles: {
                        color: '#ff0000'
                    }
                }, {
                    title: 'Red header',
                    block: 'h1',
                    styles: {
                        color: '#ff0000'
                    }
                }, {
                    title: 'Example 1',
                    inline: 'span',
                    classes: 'example1'
                }, {
                    title: 'Example 2',
                    inline: 'span',
                    classes: 'example2'
                }, {
                    title: 'Table styles'
                }, {
                    title: 'Table row 1',
                    selector: 'tr',
                    classes: 'tablerow1'
                }],

                templates: [{
                    title: 'Test template 1',
                    content: 'Test 1'
                }, {
                    title: 'Test template 2',
                    content: 'Test 2'
                    }]
              
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

                if ($("#DDL_utilidad").val() == '') {
                    $('#modal_agregar').modal('show');
                    error('El campo Utilidad no puede estar vacío.')
                    setTimeout(function () { $("#DDL_utilidad").focus(); }, 100);
                    return false;
                }

                if ($("#txt_telefono").val() == '') {
                    $('#modal_agregar').modal('show');
                    error('El campo Teléfono no puede estar vacío.')
                    setTimeout(function () { $("#txt_telefono").focus(); }, 100);
                    return false;
                }

                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Agregando ticket, por favor espere.');

            })

        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
        
        <div class="container-fluid">
      
        <h5>
            <asp:LinkButton ID="Lkb_refrecar" runat="server" CssClass="bi bi-arrow-clockwise text-primary" Font-Size="13" ToolTip="Refrescar" TabIndex ="-1" Visible="true" OnClientClick="mostrar_trabajando('Actualizando, por favor espere.');"></asp:LinkButton>
            Atención Cliente</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>

        <div class="container-fluid" style="text-align:center">
            <table style="width :100%;border:0px solid red;">
            <tr>
                <td style="width:50%;border: 0px solid blue; text-align:left;">
                    <asp:LinkButton ID="img_exportar_excel" runat="server" CssClass ="bi bi-filetype-xls text-primary" Font-Size="20" height="25" ToolTip="Descargar Excel" TabIndex ="-1" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="img_exportar_txt" runat="server" CssClass ="bi bi-filetype-txt text-primary"  Font-Size="20" height="25" ToolTip="Descargar TXT" TabIndex ="-1" Visible="false"></asp:LinkButton>
                </td>
                    
                <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span><asp:LinkButton ID="img_nueva" runat="server" CssClass ="bi bi-file-earmark-plus text-primary" Font-Size="20" style="height:25px; margin-left: 10px;position:relative;" ToolTip="Nuevo"/></td>
            </tr>
            </table>
        </div>
      
        <div class="table-responsive">

        <asp:GridView ID="gridview_consulta"  
        AutoGenerateColumns="false" runat="server" 
        cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Tickets" EmptyDataText="No hay resultados."
        CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="id,fecha_creacion,hora_creacion,utilidad,apartado,consulta,fecha_respuesta,hora_respuesta,respuesta,telefono"
        ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
        <AlternatingRowStyle BackColor="White" />
            
        <Columns>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="50" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>

                <asp:LinkButton ID ="lk_tickets" runat ="server" 
                    CssClass ="bi bi-binoculars text-primary "
                    Font-Size="25px"
                    ToolTip ="Ver Ticket" TabIndex="-1" 
                    CommandName="ver_tickets"
                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />

            </ItemTemplate>
                <HeaderStyle CssClass="gvHeaderCenter" />
                <ItemStyle CssClass="gvHeaderCenter" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Estado" HeaderStyle-Width="45" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" >
                <ItemTemplate>
                    <asp:image ID="imgestado" runat="server" ImageUrl="" />
                    <asp:LinkButton ID="LB_estado" runat="server" Enabled ="false" Font-Size="20" height="25"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="id" HeaderText="Nº Ticket" HeaderStyle-Width="70px"
                SortExpression="id" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderCenter"/>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha Ticket" HeaderStyle-Width="100px"
                SortExpression="fecha_creacion" ReadOnly="True" DataFormatString="{0:d}">
            <HeaderStyle CssClass="gvHeaderCenter"/>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="hora_creacion" HeaderText="Hora Ticket" HeaderStyle-Width="100px"
                SortExpression="hora" ItemStyle-HorizontalAlign="Left">
             <HeaderStyle CssClass="gvHeaderCenter"/>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="nombre_usuario" HeaderText="Usuario" 
                SortExpression="nombre_usuario" ItemStyle-HorizontalAlign="Left">
            <HeaderStyle HorizontalAlign="Left"/>
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="fecha_respuesta" HeaderText="Fecha Respuesta" HeaderStyle-Width="120px"
                SortExpression="fecha_respuesta" ReadOnly="True" DataFormatString="{0:d}">
             <HeaderStyle CssClass="gvHeaderCenter"/>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="hora_respuesta" HeaderText="Hora Respuesta" HeaderStyle-Width="100px"
                SortExpression="hora_respuesta" ItemStyle-HorizontalAlign="Left">
             <HeaderStyle CssClass="gvHeaderCenter"/>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            
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

        <!-- Modal Agregar -->
        <div class="modal" id="modal_agregar" tabindex="-1" role="dialog">
            <div class="modal-dialog" style="max-width:70%; ">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5"><asp:Label ID="lbl_titulo" runat="server" Text="..."></asp:Label></h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    
                    <div class="modal-body">

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Utilidad:
                                </span>
                            </div>
                            <asp:DropDownList ID="DDL_utilidad" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Apartado:
                                </span>
                            </div>
                            <asp:DropDownList ID="DDL_apartado" runat="server" class="form-control"></asp:DropDownList>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Teléfono:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_telefono" runat="server" class="form-control" placeholder="Escriba el Teléfono directo de contacto"  maxlength="9"></asp:TextBox>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Consulta:
                                </span>
                            </div>
                            <asp:TextBox ID="__txt_consulta" runat="server" class="textbox" AcceptsReturn="true" ScrollBars="Vertical" width="100%" TextMode="MultiLine" Height="100" MaxLength ="1000"></asp:TextBox>
                        </div>
                        
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_grabar" runat="server" CssClass="btn btn-danger" Text="Guardar" />
                        <asp:Button id="btn_modificar" runat="server" CssClass="btn btn-danger" Text="Modificar" />
                    </div>

                </div>
            </div>
        </div>

        <!-- Modal Nota sobre horario -->
        <div class="modal" id="modal_horario" tabindex="-1" role="dialog">
        <div class="modal-dialog">

	        <div class="modal-content">
                
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Aviso</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="LT_mensaje_horario" runat="server"></asp:Literal>
                </div>
                    
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button id="btn_abrir_nuevo" runat="server" CssClass="btn btn-outline-primary" Text="Aceptar" />
                </div>
            </div>
        </div>
        </div>

        <!-- Modal Visualizar -->
        <div class="modal" id="modal_visualizar" tabindex="-1" role="dialog">
            <div class="modal-dialog" style="max-width:70%; ">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h1 class="modal-title fs-5"><asp:Label ID="lbl_titulo_visualizar" runat="server" Text="..."></asp:Label></h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="modal-body">

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Utilidad:
                                </span>
                            </div>
                            <asp:DropDownList ID="DDL_utilidad_visualizar" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Apartado:
                                </span>
                            </div>
                            <asp:DropDownList ID="DDL_apartado_visualizar" runat="server" class="form-control"></asp:DropDownList>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Teléfono:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_telefono_visualizar" runat="server" class="form-control" placeholder="Escriba el Teléfono directo de contacto"  maxlength="9"></asp:TextBox>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Consulta:
                                </span>
                            </div>
                        </div>
                        <asp:TextBox ID="__LT_consulta" runat="server" class="textbox" AcceptsReturn="true" ScrollBars="Vertical" width="100%" TextMode="MultiLine" Height="100" MaxLength ="1000" ReadOnly ="true"></asp:TextBox>
                        <br />

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Respuesta:
                                </span>
                            </div>
                        </div>
                        <asp:TextBox ID="__LT_respuesta" runat="server" class="textbox" AcceptsReturn="true" ScrollBars="Vertical" width="100%" TextMode="MultiLine" Height="100" MaxLength ="1000" ReadOnly ="true"></asp:TextBox>
                    </div>
                    
                    <div class="modal-footer">
                        <span class="text-muted" style="color:#0072c6;"><small><asp:Label ID="lbl_fecha_respuesta" runat="server" Text=""></asp:Label></small></span>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>