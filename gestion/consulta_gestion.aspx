<%@ Page Language="VB" AutoEventWireup="false" CodeFile="consulta_gestion.aspx.vb" Inherits="gestion_consulta_gestion" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    
    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link href="../Content46/bootstrap.css" rel="stylesheet" />
    <link href="../Content46/alertifyjs/alertify.min.css" rel="stylesheet" />
    <link href="../Content46/bootstrap-datepicker.standalone.min.css" rel="stylesheet" />

    <!-- JQUERY-------------------------------------------------------------------------------------------------->    
    <script src="../Scripts46/jquery-3.6.0.js"></script>
    <script src="../Scripts46/jquery-ui.js"></script>
    <script src="../Scripts46/bootstrap.js"></script>
    <script src="../Scripts46/alertify.js"></script>
    <script src="../Scripts46/shortcut.js"></script>
    <script src="../Scripts46/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts46/locales/bootstrap-datepicker.es.js"></script>
    <script src="../Scripts46/jquery.maskedinput.js"></script>
    <script src="../Scripts46/device.js"></script>

    <!-- PERSONAL---------------------------------------------------------------------------------------------------->     
    <link href="../Content46/interior.css" rel="stylesheet" />
    <script src="../Scripts46/interior.js"></script>
    <script src="../Scripts46/js_fecha_formato.js"></script>
    <script src="../Scripts46/js_cuenta_formato.js"></script>
  
    <script type="text/javascript">
        
        $(document).ready(function () {
                     
            //Desactivo la funcion F5 (refresco)
            shortcut.add("F5", function () { });

            //Tecla Fecha
            shortcut.add("Ctrl+F", function () { setTimeout(function () { $('#txt_fecha').focus(); $('#txt_fecha').select(); }, 100); });

            //Tecla Cuenta
            shortcut.add("Ctrl+C", function () { setTimeout(function () { $('#txt_cuenta').focus(); $('#txt_cuenta').select(); }, 100); });

            //Tecla Importe
            shortcut.add("Ctrl+I", function () { setTimeout(function () { $('#txt_importe').focus(); $('#txt_importe').select();}, 100); });

             //Tecla Cuenta Adelante
            shortcut.add("Ctrl+Right", function () { $("#cuenta_adelante").click(); });

            //Tecla Cuenta Atrás
            shortcut.add("Ctrl+Left", function () { $("#cuenta_atras").click(); });

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
            //Control para las cuentas
            $(".textbox_cuenta").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    //asigno la cuenta
                    var cuenta = $("#" + this.id + "").val();
                    var elemento = this.id
                    $.ajax({
                        type: "POST",
                        url: '../default.aspx/Obtener_cuenta',
                        data: "{ 'cuenta': '" + cuenta + "'}",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "0") {
                                error('La Cuenta no existe.');
                                $("#lbl_inicial_numerica").text('')
                            } else {
                                var parametro = (data.d).split("|");
                                $("#lbl_inicial_numerica").text(parametro[2]);
                            }

                        },
                        error: function (response) { alert(response.responseText); }
                    });

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
                mostrar_trabajando('Consultando Archivos, por favor espere.');
                
            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#cuenta_adelante").on('click', function (e) {
                
                //almaceno la cuenta que escribió el usuario
                var cuenta1 = $("#txt_cuenta").val();
                var elemento1 = "txt_cuenta"
                $.ajax({
                    type: "POST",
                    url: '../default.aspx/siguiente_cuenta',
                    data: "{ 'cuenta': '" + cuenta1 + "'}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#" + elemento1 + "").val(data.d);
                        //disparo la consulta
                        $("#btn_consultar").click();
                    }
                });

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#cuenta_atras").on('click', function (e) {

                //almaceno la cuenta que escribió el usuario
                var cuenta1 = $("#txt_cuenta").val();
                var elemento1 = "txt_cuenta"
                $.ajax({
                    type: "POST",
                    url: '../default.aspx/atras_cuenta',
                    data: "{ 'cuenta': '" + cuenta1 + "'}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#" + elemento1 + "").val(data.d);
                        //disparo la consulta
                        $("#btn_consultar").click();
                    }
                });

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#imagen_posterior").on('click', function (e) {

                //Aumento el numero a buscar
                var actual = parseInt($("#txt_actual").val())
                $("#txt_actual").val(actual + 1)

                //Leo de control txt_informacion
                var informacion = $("#txt_informacion").val().split("&")
                for (var i = 0; i <= informacion.length - 1; i++) {

                    //Descompongo los detalles de cada ventana
                    var detalles_ventana = informacion[i].split("|")

                    if (detalles_ventana[0] == $("#txt_actual").val()) {

                        //Asigno Ruta,nombre,peso y muestro el modal
                        if (detalles_ventana[1].toLowerCase().indexOf(".pdf") != -1) {

                            //Asigno ancho y alto
                            $("#ver_factura").css("width", "100%")
                            $("#ver_factura").css("height", $(window).height() - 150)

                            //Asigno Ruta,nombre,peso y muestro el modal
                            $('#ver_factura').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + detalles_ventana[1])
                            $('#ver_factura').show();
                            $('#ver_factura_imagen').hide();

                        } else {

                            //Asigno Ruta,nombre,peso y muestro el modal
                            $('#ver_factura_imagen').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + detalles_ventana[1])
                            $('#ver_factura_imagen').show();
                            $('#ver_factura').hide();

                        }

                        $("#informacion").html('Nombre: <span style="color:#28a745;">' + detalles_ventana[1] + '</span> - (' + detalles_ventana[3] + ' Kb)')
                        $("#total_imagenes").text($("#txt_actual").val() + "  de  " + $("#txt_total").val())

                        //Dibujar flechas
                        dibujar_flechas();

                        //Dibujar imagenes de rotar
                        dibujar_rotar(detalles_ventana[1]);

                        break;
                    }

                }

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#imagen_anterior").on('click', function (e) {

                //Aumento el numero a buscar
                var actual = parseInt($("#txt_actual").val())
                $("#txt_actual").val(actual - 1)

                //Leo de control txt_informacion
                var informacion = $("#txt_informacion").val().split("&")
                for (var i = 0; i <= informacion.length - 1; i++) {

                    //Descompongo los detalles de cada ventana
                    var detalles_ventana = informacion[i].split("|")

                    if (detalles_ventana[0] == $("#txt_actual").val()) {

                        //Asigno Ruta,nombre,peso y muestro el modal
                        if (detalles_ventana[1].toLowerCase().indexOf(".pdf") != -1) {

                            //Asigno ancho y alto
                            $("#ver_factura").css("width", "100%")
                            $("#ver_factura").css("height", $(window).height() - 150)

                            //Asigno Ruta,nombre,peso y muestro el modal
                            $('#ver_factura').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + detalles_ventana[1])
                            $('#ver_factura').show();
                            $('#ver_factura_imagen').hide();

                        } else {

                            //Asigno Ruta,nombre,peso y muestro el modal
                            $('#ver_factura_imagen').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + detalles_ventana[1])
                            $('#ver_factura_imagen').show();
                            $('#ver_factura').hide();

                        }

                        $("#informacion").html('Nombre: <span style="color:#28a745;">' + detalles_ventana[1] + '</span> - (' + detalles_ventana[3] + ' Kb)')
                        $("#total_imagenes").text($("#txt_actual").val() + "  de  " + $("#txt_total").val())

                        //Dibujar flechas
                        dibujar_flechas();

                        //Dibujar imagenes de rotar
                        dibujar_rotar(detalles_ventana[1]);

                        break;
                    }

                }

            })

        });

        //Tamaño para el Modal cuando redimensiona la ventana
        $(window).resize(function (e) {
            if ($("#modal_visor").is(":visible") == true) {
                //Asigno alto
                $("#ver_factura").css("height", $(window).height() - 150)
            }
        });

        function visor() {

            //Leo de control txt_informacion
            var informacion = $("#txt_informacion").val().split("&")
            for (var i = 0; i <= informacion.length - 1; i++) {
                //Descompongo los detalles de cada ventana
                var detalles_ventana = informacion[i].split("|")
                //Si coincide el numero
                if (detalles_ventana[0] == $("#txt_actual").val()) {

                    if (detalles_ventana[1].toLowerCase().indexOf(".pdf") != -1) {

                        //Asigno ancho y alto
                        $("#ver_factura").css("width", "100%")
                        $("#ver_factura").css("height", $(window).height() - 150)

                        //Asigno Ruta,nombre,peso y muestro el modal
                        $('#ver_factura').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + detalles_ventana[1])
                        $('#ver_factura').show();
                        $('#ver_factura_imagen').hide();

                    } else {

                        //Asigno Ruta,nombre,peso y muestro el modal
                        $('#ver_factura_imagen').attr('src', '../imagenes_usuarios/' + detalles_ventana[2] + detalles_ventana[1] + '?ver=' + new Date())
                        $('#ver_factura_imagen').show();
                        $('#ver_factura').hide();

                    }

                    $("#informacion").html('Nombre: <span style="color:#28a745;">' + detalles_ventana[1] + '</span> - (' + detalles_ventana[3] + ' Kb)')
                    $("#total_imagenes").text($("#txt_actual").val() + "  de  " + $("#txt_total").val())

                    //Dibujar flechas
                    dibujar_flechas();

                    //Dibujar imagenes de rotar
                    dibujar_rotar(detalles_ventana[1]);

                    //Muestro imagen
                    $('#modal_visor').modal('show');

                    break;
                }

            }
            
        }

        function dibujar_flechas() {

            //Si es la ultima imagen a mostrar
            if ($("#txt_actual").val() == $("#txt_total").val()) {
                $("#imagen_posterior").hide();
            } else {
                $("#imagen_posterior").show();
            }

            //Si es la ultima imagen a mostrar
            if ($("#txt_actual").val() == "1") {
                $("#imagen_anterior").hide();
            } else {
                $("#imagen_anterior").show();
            }

        }

        function dibujar_rotar(nombre_fichero) {

            //Control para el sistema de rotado de imagenes
            var ext = nombre_fichero.split('.');
            // ahora obtenemos el ultimo valor despues el punto
            // obtenemos el length por si el archivo lleva nombre con mas de 2 puntos
            ext = ext[ext.length - 1];


            switch (ext) {
                case 'pdf':
                    $("#img_izquierda").hide();
                    $("#img_derecha").hide();
                    break;
                default:
                    $("#img_izquierda").show();
                    $("#img_derecha").show();
                    break;
            }

        }

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
        
        <h5>Consultas de Gestión Documental</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <div class="table-responsive">       
        <table border="0" style="width:100%;">
            <tr>
                <td width="100" align="center"><small><u>F</u>echa</small></td>
                <td width="110" align="center"><small>Nº Asiento</small></td>
                <asp:PlaceHolder ID="PH_referencia_nombre" runat="server">
                    <td width="120" align="center"><small>Referencia</small></td>
                </asp:PlaceHolder> 
                <td width="150" align="center">

                    <span id="cuenta_atras" class="material-icons" onmouseover="hand('cuenta_atras')" title="Retroceder Cuenta (Ctrl + flecha Izquierda)" style="font-size:30px; color: #155724; position:relative; top:10px;">arrow_left</span>
                        &nbsp;<small><u>C</u>uenta:</small>&nbsp;
                    <span id="cuenta_adelante" class="material-icons" onmouseover="hand('cuenta_adelante')" title="Avanzar Cuenta (Ctrl + flecha Derecha)" style="font-size:30px; color: #155724; position:relative; top:10px;">arrow_right</span>

                </td>
                <asp:PlaceHolder ID="PH_serie_nombre" runat="server">
                    <td width="80" align="center"><small>Serie</small></td>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PH_factura_nombre" runat="server">
                    <td width="100" align="center"><small>Factura</small></td>
                </asp:PlaceHolder> 
                <td width="80" align="center"><small>Concepto</small></td>
                <td width="100" align="center"><small><u>I</u>mporte</small></td>
                <td width="90" align="center"><small>Observaciones</small></td>
            </tr>
            <tr>
                <td align="right"><asp:TextBox ID="txt_fecha" runat="server" CssClass="form-control input-sm textbox_fecha" MaxLength="10" width="95%"></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_numero" runat="server" class="form-control input-sm"  MaxLength="15" width="95%"></asp:TextBox> </td>
                <asp:PlaceHolder ID="PH_referencia" runat="server">
                    <td align="center"><asp:TextBox ID="txt_referencia" runat="server" CssClass="form-control input-sm" MaxLength="30" width="95%"></asp:TextBox></td>
                </asp:PlaceHolder>
                <td align="center" align="center"><asp:TextBox ID="txt_cuenta" runat="server" CssClass="form-control input-sm textbox_cuenta" AutoCompleteType="Search" width="95%"></asp:TextBox></td>
                <asp:PlaceHolder ID="PH_serie" runat="server">
                    <td width=""><asp:TextBox ID="txt_serie" runat="server" CssClass="form-control input-sm" MaxLength="6" width="95%" Style="text-transform: uppercase"></asp:TextBox></td>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PH_factura" runat="server">
                    <td align="center"><asp:TextBox ID="txt_factura" runat="server" CssClass="form-control input-sm" MaxLength="20" width="95%"></asp:TextBox></td>
                </asp:PlaceHolder> 
                <td align="center"><asp:TextBox ID="txt_concepto" runat="server" CssClass="form-control input-sm" Width="95%" MaxLength ="200" ></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_importe" runat="server" CssClass="form-control input-sm" Width="95%" MaxLength ="10" ></asp:TextBox></td>
                <td align="center"><asp:TextBox ID="txt_observaciones" runat="server" CssClass="form-control input-sm" Width="95%" MaxLength="250" ></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" align="left"><span class="text-muted" style="font-size: 9px;">Mes de Cierre: <asp:Label ID="lbl_fechas_proceso" runat="server" ForeColor="#155724"></asp:Label></span></td>
                <td colspan="3" style="border-left: 1px dashed #155724;border-bottom: 1px dashed #155724;">
                    <div style="color:#155724;font-size: 12px;height:20px;">
                        <asp:Label ID="lbl_inicial_numerica" runat="server" Text=""></asp:Label>
                    </div>
                </td>
                <td align="center" colspan="2" style="border-bottom: 1px dashed #155724; border-right: 1px dashed #155724;">

                    <div class="custom-control custom-checkbox">
                        <input type="checkbox" class="custom-control-input" id="CB_estricto" runat="server"/>
                        <label class="custom-control-label" for="CB_estricto" style="color:gray; font-size:9px;">Resultados Estrictos</label>
                    </div>
                 
                </td>
            </tr>
        </table>
        </div> 
        <p></p>
    
        <div class="container-fluid" style="text-align:center">

            <asp:Button ID="btn_consultar" runat="server" Text="Buscar" CssClass="btn btn-outline-success btn-sm"/>

            <table style="width :100%;">
            <tr>
                <td style="width:33%;border: 0px solid blue; text-align:left;">
                    <asp:ImageButton ID="img_exportar_excel" src="../Imagenes/web/xls_icono.png" runat="server" height="25" ToolTip="Descargar Excel" TabIndex ="-1" Visible="false"/>
                    <asp:ImageButton ID="img_exportar_txt" src="../Imagenes/web/txt_icono.png" runat="server" height="25" ToolTip="Descargar TXT" TabIndex ="-1" Visible="false"/>
                </td>
                <td style="width:33%;"></td>
                <td style="width:33%; text-align:right;"><span class="text-muted" style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span></td>
            </tr>
            </table>
        </div>
       
        <div class="table-responsive">

        <div id="ver" style="position:fixed;top:0px;background-color:#d4edda;color:#333333;font-weight:700; width:100%;display:none;">

            <table border="0" style="font-size:12px;">
            <tr>
                <td style="width:40px;padding: 0px;"></td>
                <td style="width:40px;padding: 0px;"></td>
                <td style="width:70px;padding: 0px;">Fecha</td>
                <td style="width:70px;padding: 0px;">Asiento</td>
                <td style="width:250px;padding: 0px;">Cuenta</td>
                <td style="width:250px;padding: 0px;">Concepto</td>
                <td style="width:80px;padding: 0px;">Importe</td>
                <td style="width:80px;padding: 0px;">Factura</td>
                <td style="width:150px;padding: 0px;">Observaciones</td>
                <td style="width:80px;padding: 0px;"></td>
                <td style="width:80px;padding: 0px;"></td>
            </tr>
            </table>

        </div>
           
            <asp:GridView ID="gridview_consulta"  
            AutoGenerateColumns="false" runat="server" 
            cssclass="table-bordered" Font-Size="11px" Width="100%" ToolTip="Consulta de Gestión Documental" EmptyDataText="No hay resultados."
            DataKeyNames="nombre_fichero,fecha_creacion,hora_creacion,size,ruta,id_cabecera_asiento,id_cabecera_impuesto,fecha,referencia_asiento,observaciones"
            CellPadding="4" ForeColor="#333333" GridLines="Both" 
            ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25" PagerSettings-Position="TopAndBottom">
            <AlternatingRowStyle BackColor="White" />
            
            <Columns>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>
            <asp:ImageButton ID="Button2" runat="server" CausesValidation="false" ImageUrl="~/imagenes/web/Audit-WF.png" 
            CommandName="ver_apuntes"
            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
            Text="<span class='glyphicon glyphicon-search' aria-hidden='true'></span>" CssClass="boton" />
            </ItemTemplate>
                <HeaderStyle CssClass="gvHeaderCenter" />
                <ItemStyle CssClass="gvHeaderCenter" Width ="40" />
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="40" HeaderStyle-BackColor="#ffffff">
            <ItemTemplate>
                <asp:ImageButton ID="btn_ver" runat="server" CausesValidation="false" ImageUrl="~/imagenes/web/Paperclip_01_lila.png" 
            CommandName="ver_fichero" ToolTip ="Ver Fichero"
            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
            Text="<span class='glyphicon glyphicon-search' aria-hidden='true'></span>" CssClass="boton" />
            </ItemTemplate>
                <HeaderStyle CssClass="gvHeaderCenter" />
                <ItemStyle CssClass="gvHeaderCenter" Width ="40"/>
            </asp:TemplateField>

            <asp:BoundField DataField="fecha" HeaderText="Fecha" HeaderStyle-Width="70"
                SortExpression="fecha" ReadOnly="True" DataFormatString="{0:d}">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left" Width="70"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="referencia_asiento" HeaderText="Asiento" HeaderStyle-Width="70"
                SortExpression="referencia_asiento" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="70"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="cuenta" HeaderText="Cuenta" HeaderStyle-Width="250"
                SortExpression="cuenta" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="250"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="concepto" HeaderText="Concepto" HeaderStyle-Width="250"
                SortExpression="concepto" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width ="250"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="importe_final" HeaderText="Importe" HeaderStyle-Width="80"
                SortExpression="importe_final" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <ItemStyle HorizontalAlign="Right" Width="80px" />
            <HeaderStyle CssClass="gvHeaderright" />
            </asp:BoundField>

            <asp:BoundField DataField="factura" HeaderText="Factura" HeaderStyle-Width="80"
                SortExpression="factura" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <ItemStyle HorizontalAlign="Right" Width="80px" />
            <HeaderStyle CssClass="gvHeaderCenter" />
            </asp:BoundField>

            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" HeaderStyle-Width="150"
                SortExpression="observaciones" ReadOnly="True" ItemStyle-HorizontalAlign="left">
            <ItemStyle HorizontalAlign="Right" Width="150px" />
            <HeaderStyle CssClass="gvHeaderCenter" />
            </asp:BoundField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
            <ItemTemplate>
                <asp:Button ID="btnedit" 
                ControlStyle-CssClass="btn btn-outline-success btn-sm" runat="server" 
                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                    tooltip="Editar este Apunte"
                CommandName="editar" Text="Editar" TabIndex="-1"/>
            </ItemTemplate>
                <HeaderStyle Width="80px"/>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
            <ItemTemplate>
                <asp:Button ID="btndelete"
                ControlStyle-CssClass="btn btn-outline-danger btn-sm" runat="server" 
                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                tooltip="Borrar este Apunte"
                CommandName="borrar" Text="Eliminar" TabIndex="-1"/>
            </ItemTemplate>
                <HeaderStyle Width="80px" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>

            <%--<asp:BoundField DataField="" HeaderText="" 
                SortExpression="" ReadOnly="True" ItemStyle-HorizontalAlign="center">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle CssClass="gvHeaderCenter" />
            </asp:BoundField>--%>

            </Columns>
                   
            <HeaderStyle BackColor="#d4edda" Font-Bold="True" ForeColor="black" />
            <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />
            
        </asp:GridView>

        </div>

        </div>

        <!-- Modal -->
        <div class="modal fade"  id="modal_visor" role="dialog">
            <div class="modal-dialog" style="min-width: 80% !important;height:95% !important;">
    
                <!-- Modal content-->
                <div class="modal-content">
                <div class="modal-header">
                
                    <table style="width:100%;height:40px;" class="cerrar">
                        <tr>
                            <td><span id="informacion" style="font-size: 14px;"></span></td>
                            <td style="width:20px;"></td>
                            <td style="width:35px;"><asp:ImageButton ID="img_izquierda" runat="server" src="../imagenes/web/Portrait-01-WF_270.png" Height="25" alt="Girar imagen a la izquierda" title="Girar imagen a la izquierda" /></td>
                            <td style="width:5px;"></td>
                            <td style="width:5px;"></td>
                            <td style="width:35px;"><asp:ImageButton ID="img_derecha" runat="server" src="../imagenes/web/Portrait-01-WF.png" Height="25" alt="Girar imagen a la derecha" title="Girar imagen a la derecha" /></td>
                            <td style="width:25px;"></td>
                            <td style="width:35px;"><span id="imagen_anterior" class="material-icons" onmouseover="hand('imagen_anterior')" title="Imagen Anterior" style="font-size:30px; color: #155724; position:relative; top:5px;">arrow_left</span></td>
                            <td style="text-align: center;width:90px;"><span id="total_imagenes" class="text-muted"><small></small></span></td>
                            <td style="width:35px;"><span id="imagen_posterior" class="material-icons" onmouseover="hand('imagen_posterior')" title="Imagen Posterior" style="font-size:30px; color: #155724; position:relative; top:5px;">arrow_right</span></td>
                            <td style="width:30px;"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button></td>
                        </tr>
                    </table>

                </div>
                <div class="modal-body">
                    <iframe id="ver_factura" src="" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe>
                    <img id="ver_factura_imagen" src="" border="0" width="100%" />
                </div>
                </div>
      
            </div>
        </div>

        <!-- Modal Eliminar-->
        <div class="modal" id="modal_eliminar" tabindex="-1" role="dialog">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h3 class="modal-title">Eliminar Fichero</h3>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <asp:Literal ID="LT_mensaje_eliminar" runat="server"></asp:Literal>
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_borrar_fichero" runat="server" CssClass="btn btn-danger" Text="Aceptar" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Agregar -->
        <div class="modal" id="modal_agregar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">

	            <div class="modal-content">
                
                    <div class="modal-header">
                        <h4 class="modal-title">
                            <asp:Label ID="lbl_titulo" runat="server" Text="..."></asp:Label></h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" style="font-size: 40px;" class="text-danger">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                         <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Observación:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_observacion" runat="server" class="form-control" placeholder="Escriba una observación"  maxlength="250"></asp:TextBox>
                        </div>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_modificar" runat="server" CssClass="btn btn-danger" Text="Modificar" />
                    </div>
                </div>
            </div>
        </div>
      
        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />
       
        <div id="partes_hidden" style="visibility:hidden; display: none; ">
            Index_borrar_linea<asp:TextBox ID="txt_index" runat="server" text="" Width="400"></asp:TextBox><br />
            txt_fecha_final:<asp:TextBox ID="txt_fecha_final" runat="server" Width="400"></asp:TextBox><br />
            txt_informacion:<asp:TextBox ID="txt_informacion" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox><br />
            txt_total:<asp:TextBox ID="txt_total" runat="server" Width="100%"></asp:TextBox><br />
            txt_actual:<asp:TextBox ID="txt_actual" runat="server" Width="100%"></asp:TextBox><br />
        </div>

    </form>

</body>
</html>