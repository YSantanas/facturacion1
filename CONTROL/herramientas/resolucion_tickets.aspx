<%@ Page Language="VB" AutoEventWireup="false" CodeFile="resolucion_tickets.aspx.vb" Inherits="CONTROL_herramientas_resolucion_tickets" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="refresh" content="60" />
    
    <title></title>

     <!-- CSS ---------------------------------------------------------------------------------------------------->
     <link href="../../Content/bootstrap.css" rel="stylesheet" />
     <link href="../../Content/alertify.core.css" rel="stylesheet" />
     <link href="../../Content/alertify.default.css" rel="stylesheet" />
     <link href="../../Content/bootstrap-icons.css" rel="stylesheet" />
     <link href="../../Content/jquery-ui.css" rel="stylesheet" />

     <!-- JQUERY ------------------------------------------------------------------------------------------------->
     <script src="../../Scripts/jquery-3.7.1.js"></script>
     <script src="../../Scripts/bootstrap.js"></script>
     <script src="../../Scripts/alertify.js"></script>
     <script src="../../Scripts/shortcut.js"></script>
     <script src="../../Scripts/jquery-ui-1-13.3.js"></script>
     <script src="../../Scripts/device.js"></script>
     <script src="../../Scripts/jquery.maskedinput.js"></script>

     <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
     <link href="../../Content/interior.css" rel="stylesheet" />
     <script src="../../Scripts/interior.js"></script>
     <script src="../../Scripts/js_fecha_formato.js"></script>
     <script src="../../Scripts/tinymce.min.js"></script>

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

            //Control para el click
            $("#btn_consultar").on('click', function () {

                //Fecha Inicial y Final
                var fecha_inicial = $("#txt_fecha_inicio").val().replace('__/__/____', '');
                var fecha_final = $("#txt_fecha_final").val().replace('__/__/____', '');

                if (fecha_inicial == '') {
                    error('La Fecha Inicial no puede estar vacía.')
                    setTimeout(function () { $("#txt_fecha_inicio").focus(); }, 100);
                    return false;
                }
                if (fecha_final == '') {
                    error('La Fecha Final no puede estar vacía.')
                    setTimeout(function () { $("#txt_fecha_final").focus(); }, 100);
                    return false;
                }

                //Mensaje al usuario
                mostrar_trabajando('Consultando Tickets, por favor espere.');

            })

            //Control para el click
            $("#recoger").on('click', function (e) {

                $(".panel-left").toggle(500);

            })

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
            $("#btn_grabar_confirmar").on('click', function () {

                //Mensaje al usuario
                $('#modal_confirmar').modal('hide');
                mostrar_trabajando('Enviando ticket, por favor espere.');

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
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
      
        <div class="container-fluid">
      
        <h5><span id="recoger" class="bi bi-arrow-left-right text-primary" style="font-size:16px;" onmouseover="hand('recoger')" title="Contraer/Expandir menú"></span> Tickets <asp:LinkButton ID="Lkb_reset" runat="server" CssClass="bi bi-arrow-clockwise text-primary" Font-Size="13" ToolTip="Refrescar" TabIndex ="-1" Visible="true" OnClientClick ="mostrar_trabajando('Actualizando Tickets, por favor espere.');"></asp:LinkButton>
            <span style="color:red;"><asp:Literal ID="Lt_error" runat="server"></asp:Literal></span>
        </h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
       
        <div class="panel-container">

            <div class="panel-left">
             
                <div class="input-group input-group-sm mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">
                            Fecha Inicial:
                        </span>
                    </div>
                    <asp:TextBox ID="txt_fecha_inicio" runat="server" class="form-control textbox_fecha" placeholder="Fecha Inicial"></asp:TextBox>
                </div>

                <div class="input-group input-group-sm mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">
                            Fecha Final:
                        </span>
                    </div>
                    <asp:TextBox ID="txt_fecha_final" runat="server" class="form-control textbox_fecha" placeholder="Fecha Final"></asp:TextBox>
                </div>

                <div class="input-group input-group-sm mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">
                            Tickets:
                        </span>
                    </div>
                    <asp:DropDownList ID="DDL_opcion_ticket" runat="server" class="form-control"></asp:DropDownList>
                </div>
      
                <div class="container-fluid" style="text-align:center">
                    <asp:Button ID="btn_consultar" runat="server" CssClass="btn btn-outline-primary btn-sm" Text="Ver Tickets" />
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
                    
                    <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span></td>
                </tr>
                </table>
            </div>

           <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>

                <div class="table-responsive">
                
                <asp:GridView ID="gridview_consulta"  
                AutoGenerateColumns="false" runat="server" 
                cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Tickets" EmptyDataText="No hay resultados."
                CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="Id,fecha_creacion,hora_creacion,utilidad,apartado,consulta,fecha_respuesta,hora_respuesta,respuesta,nombre_usuario,nombre_fiscal,email,nombre"
                ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
                <AlternatingRowStyle BackColor="White" />
            
                <Columns>

                    <asp:TemplateField ShowHeader="False" HeaderStyle-ForeColor="white" HeaderStyle-Width="30" Visible ="true">
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

                    <asp:BoundField DataField="id" HeaderText="Id" HeaderStyle-Width="50px"
                        SortExpression="id" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha Ticket" HeaderStyle-Width="80px"
                        SortExpression="Fecha Inicio" ReadOnly="True" DataFormatString="{0:dd-M-yyyy}" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="hora_creacion" HeaderText="Hora Ticket" HeaderStyle-Width="80px"
                        SortExpression="hora" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                           
                    <asp:BoundField DataField="nombre_fiscal" HeaderText="Nombre Fiscal" 
                        SortExpression="nombre_fiscal" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="nombre_usuario" HeaderText="Usuario" 
                        SortExpression="nombre_usuario" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="email" HeaderText="Email" 
                        SortExpression="email" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="telefono" HeaderText="Teléfono" HeaderStyle-Width="80px"
                        SortExpression="telefono" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                </Columns>
                   
                    <HeaderStyle CssClass ="table-primary"/>
                    <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

                </asp:GridView>
                
                </div>

            <%--</ContentTemplate>
            </asp:UpdatePanel>--%>

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

    <!-- Modal Ver Ticket -->
    <div class="modal" id="modal_ver_tickets" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg ">

            <div class="modal-content">

                <div class="modal-body">
                    
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Utilidad:
                            </span>
                        </div>
                        <asp:DropDownList ID="DDL_utilidad_visual" runat="server" class="form-control" disabled="true"></asp:DropDownList>
                    </div>
                    
                     <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Apartado:
                            </span>
                        </div>
                        <asp:DropDownList ID="DDL_apartado_visual" runat="server" class="form-control" disabled="true"></asp:DropDownList>
                    </div>
                    
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Consulta:
                            </span>
                        </div>
                    </div>
                    <asp:TextBox ID="__txt_consulta" runat="server" class="textbox" AcceptsReturn="true" ScrollBars="Vertical" width="100%" TextMode="MultiLine" Height="100" MaxLength ="1000"></asp:TextBox>
                    <br />

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Respuesta:
                            </span>
                        </div>
                    </div>
                    <asp:TextBox ID="__txt_respuesta" runat="server" class="textbox" AcceptsReturn="true" ScrollBars="Vertical" width="100%" TextMode="MultiLine" Height="100" MaxLength ="1000"></asp:TextBox>

                </div>

                <div class="modal-footer">
                    <span class="text-muted" style="color:#0072c6;"><small><asp:Label ID="lbl_fecha_respuesta" runat="server" Text=""></asp:Label></small></span>
                    <asp:Button id="btn_grabar" runat="server" CssClass="btn btn-danger" Text="Responder" />
                </div> 
            </div>
        </div>
    </div>

    <!-- Modal Confirmar-->
    <div class="modal" id="modal_confirmar" tabindex="-1" role="dialog">
        <div class="modal-dialog">

	        <div class="modal-content">
                
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Confirmación</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="LT_mensaje_confirmar" runat="server"></asp:Literal>
                </div>
                    
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button id="btn_grabar_confirmar" runat="server" CssClass="btn btn-danger" Text="Aceptar" />
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