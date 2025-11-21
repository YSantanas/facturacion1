<%@ Page Language="VB" AutoEventWireup="false" CodeFile="altas.aspx.vb" Inherits="articulos_altas" %>

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

            //Eliminamos el click derecho de ratón para las multiples pantallas
            $('html').bind("contextmenu", function (e) {
                return false;
            });

            tinyMCE.init({
                mode: "textareas",
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

            //Cambio de contenedor
            $(".panel-container").css('height', $(window).height() - 90 + 'px');

            //Resize Panel
            $(".panel-left").resizable({
                handleSelector: ".splitter",
                resizeHeight: false
            });

            //Control para focus
            $("#btn_consultar").focusin(function () {
                $("#btn_consultar").click();
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_consultar").on('click', function (e) {
               
                //Mensaje al usuario
                mostrar_trabajando('Consultando Artículos, por favor espere.');
                
            })

            //Control para el click
            $("#btn_grabar").on('click', function () {

                //Excepciones
                if ($.trim($("#txt_articulo_modal").val()) == '') {
                    error('El campo Artículo no puede estar vacío.')
                    setTimeout(function () { $("#txt_articulo_modal").focus(); }, 100);
                    return false;
                } 
                    
                //Excepciones
                if ($.trim($("#txt_denominacion").val()) == '') {
                    error('El campo Denominación no puede estar vacío.')
                    setTimeout(function () { $("#txt_denominacion").focus(); }, 100);
                    return false;
                };

                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Agregando Artículo, por favor espere.');
                
            })

            //Control para el click
            $("#btn_modificar").on('click', function () {

                //Excepciones
                if ($.trim($("#txt_denominacion").val()) == '') {
                    error('El campo Denominación no puede estar vacío.')
                    setTimeout(function () { $("#txt_denominacion").focus(); }, 100);
                    return false;
                };

                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Modificando Artículo, por favor espere.');

           })

            //Control para el click
            $("#recoger").on('click', function (e) {

                $(".panel-left").toggle(500);

            })

            //Control para el click
            $("#abrir_tipo_impuestos").on('click', function (e) {

                //Activo el menu
                window.parent.abrir_ventana_relacional('configuracion_','bi bi-gear-wide-connected','500','440','configuracion/configuracion.aspx','8')

                //Llamo al menú Cuentas
                padre = $(window.parent.document);
                var id_usuario = $(padre).find("#txt_id_usuario").val();
                var id_empresa = $(padre).find("#txt_id_empresa").val();
                var ruta = "configuracion/tipos_impuestos.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;
                padre = $(window.parent.document);
                $(padre).find("#iconfiguracion_").attr('src', ruta);

            })

        });

        //Tamaño para el desktop
        $(window).resize(function (e) {

            $(".panel-container").css ('height', $(window).height() - 90 + 'px' );
      
        })

    </script>

</head>
<body>
    <form id="form1" runat="server">
        
        <div class="container-fluid">

        <h5><span id="recoger" class="bi bi-arrow-left-right text-primary" style="font-size:16px;" onmouseover="hand('recoger')" title="Contraer/Expandir menú"></span> Alta de Artículos</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
            
        <div class="panel-container">

            <div class="panel-left">
                
                <div class="input-group input-group-sm mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        Artículo:
                    </span>
                </div>
                <asp:TextBox ID="txt_articulo" runat="server" class="form-control textbox_articulos" placeholder="Código, Denominación" AutoPostBack="true"></asp:TextBox>
                <div class="text-primary" style="position:absolute; top:35px;font-size: 12px;height:20px;">
                    <asp:Label ID="lbl_txt_articulo" runat="server" Text=""></asp:Label>
                </div>
                </div>
                <br />
                
                <div class="container-fluid" style="text-align:center">
                    <asp:Button ID="btn_consultar" runat="server" CssClass="btn btn-outline-primary btn-sm" Text="Buscar" />
                </div>

            </div>

            <div class="splitter"></div>
            
            <div class="panel-right">
       
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
                cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta de Artículos" EmptyDataText="No hay resultados."
                CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="id,fecha_creacion,hora_creacion,denominacion,cod_barras,activo,precio,dto_1,impuesto,observaciones" 
                ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
                <AlternatingRowStyle BackColor="White" />
            
                <Columns>

                    <asp:BoundField DataField="id" HeaderText="Código" HeaderStyle-Width="90"
                        SortExpression="id" ReadOnly="True">
                    <HeaderStyle CssClass="gvHeaderleft"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="denominacion" HeaderText="Denominación"
                        SortExpression="denominacion" ReadOnly="True" ItemStyle-HorizontalAlign="left">
                    <HeaderStyle HorizontalAlign="Left" />
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

    <!-- Modal Eliminar-->
    <div class="modal" id="modal_eliminar" tabindex="-1" role="dialog">
        <div class="modal-dialog">

	        <div class="modal-content">
                
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Eliminar Artículo</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="LT_mensaje" runat="server"></asp:Literal>
                </div>
                    
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button id="btn_eliminar_confirmar" runat="server" CssClass="btn btn-danger" Text="Aceptar" />
                </div>
            </div>
        </div>
    </div>
    
    <!-- Modal Agregar -->
    <div class="modal" id="modal_agregar" tabindex="-1" role="dialog" data-bs-backdrop="static">
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
                                Artículo:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_articulo_modal" runat="server" class="form-control textbox_cliente" placeholder="Escriba un Código de Artículo"  maxlength="15"></asp:TextBox>
                        <div class="text-primary" style="position:absolute; top:35px;font-size: 12px;height:20px;">
                            <asp:Label ID="lbl_txt_codigo" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-sm-12 text-left">
                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="CB_activo" runat="server"/>
                                <label class="custom-control-label" for="CB_activo" style="color:gray; font-size: 13px;">Activo</label>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Denominación:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_denominacion" runat="server" class="form-control" placeholder="Escriba la Denominación"  maxlength="200"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Código Barras:
                            </span>
                        </div>
                       <asp:TextBox ID="txt_cod_barras" runat="server" class="form-control" placeholder="Escriba el Código de Barras"  maxlength="200"></asp:TextBox>
                    </div>

                     <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                <asp:LinkButton ID="Lkb_refrecar_familia" runat="server" CssClass="bi bi-arrow-clockwise text-primary" Font-Size="13" ToolTip="Refrescar" TabIndex ="-1" Visible="true"></asp:LinkButton>
                                <span id="abrir_familia" class="bi bi-box-arrow-in-up-right text-primary" title="Abrir Contactos" tabindex="-1" onmouseover="hand('abrir_contactos')" runat ="server"></span>
                                &nbsp;Familia:
                            </span>
                        </div>
                         <br />
                        <asp:ListBox ID="Lb_familia" runat="server" class="form-control"></asp:ListBox>
                     </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Precio:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_precio" runat="server" class="form-control" placeholder="Escriba el Precio"  maxlength="10"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                % Descuento:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_dto_1" runat="server" class="form-control" placeholder="Escriba el % de Descuento"  maxlength="10"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                <asp:LinkButton ID="Lkb_refrecar_tipo_impuestos" runat="server" CssClass="bi bi-arrow-clockwise text-primary" Font-Size="13" ToolTip="Refrescar" TabIndex ="-1" Visible="true"></asp:LinkButton>
                                <span id="abrir_tipo_impuestos" class="bi bi-box-arrow-in-up-right text-primary" title="Abrir Tipo de Impuestos" tabindex="-1" onmouseover="hand('abrir_contactos')" runat ="server"></span>
                                &nbsp;Impuesto:
                            </span>
                        </div>
                        <asp:DropDownList ID="DDL_tipo_impuestos" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                    </div>
             
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Observaciones:
                            </span>
                        </div>
                        <asp:TextBox ID="__txt_observaciones" runat="server" class="textbox" AcceptsReturn="true" ScrollBars="Vertical" width="100%" TextMode="MultiLine" Height="100" MaxLength ="2000"></asp:TextBox>
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