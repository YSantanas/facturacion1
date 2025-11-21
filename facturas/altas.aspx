<%@ Page Language="VB" AutoEventWireup="false" CodeFile="altas.aspx.vb" Inherits="facturas_altas" %>

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

    <script>

        $(document).ready(function () {

            //Desactivo la funcion F5 (refresco)
            shortcut.add("F5", function () { });

            //Tecla Nuevo
            shortcut.add("Ctrl+A", function () { $("#img_nuevo").click(); });

            //Tecla Abonar
            shortcut.add("Ctrl+M", function () { $("#img_abonar").click(); });

            //Tecla Abonar
            shortcut.add("Ctrl+E", function () { $("#img_duplicar").click(); });

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

            //Comprobrar código cliente
            $("#txt_cod_cliente").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {

                    if ($("#txt_cod_cliente").val() == '') {

                        $("#txt_cliente").val('');
                        $("#txt_nif").val('');
                        $("#txt_direccion").val('');
                        $("#txt_cp").val('');
                        $("#txt_poblacion").val('');
                        $("#txt_provincia").val('');
                        $("#txt_cliente").removeAttr('readonly');
                        $("#txt_nif").removeAttr('readonly');
                        $("#txt_direccion").removeAttr('readonly');
                        $("#txt_cp").removeAttr('readonly');
                        $("#txt_poblacion").removeAttr('readonly');
                        $("#txt_provincia").removeAttr('readonly');

                    } else {
                        $("#btn_comprobar").click();
                    }

                }
            });

            //Comprobrar denominacion cliente
            $("#txt_cliente").on('keydown', function (e) {
            var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    $("#btn_comprobar").click();
                }
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para comprobar el cliente
            $('#btn_comprobar').on('click', function (e) {
                
                    //asigno la cuenta
                    var codigo = $("#txt_cod_cliente").val();
                    var elemento = this.id
                    if (codigo != '') {
                        $.ajax({
                        async: true,
                        type: "POST",
                        url: '../default.aspx/Obtener_cliente',
                        data: "{ 'codigo': '" + codigo + "','id_empresa': '" + $(window.parent.document).find("#txt_id_empresa").val() + "'}",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "0") {
                                error('El Cliente ' + $("#txt_cod_cliente").val() + ' no existe.');
                                $("#txt_cod_cliente").val('');
                                $("#txt_cliente").val('');
                                $("#txt_nif").val('');
                                $("#txt_direccion").val('');
                                $("#txt_cp").val('');
                                $("#txt_poblacion").val('');
                                $("#txt_provincia").val('');

                                $("#txt_cliente").removeAttr('readonly');
                                $("#txt_nif").removeAttr('readonly');
                                $("#txt_direccion").removeAttr('readonly');
                                $("#txt_cp").removeAttr('readonly');
                                $("#txt_poblacion").removeAttr('readonly');
                                $("#txt_provincia").removeAttr('readonly');

                            } else {
                                var datos = (data.d).split("|")

                                if (datos[3] == "False") {
                                    error('El Cliente ' + datos[4] + ' esta dado de baja.');
                                    $("#txt_cod_cliente").val('');
                                    $("#txt_cliente").val('');
                                    $("#txt_nif").val('');
                                    $("#txt_direccion").val('');
                                    $("#txt_cp").val('');
                                    $("#txt_poblacion").val('');
                                    $("#txt_provincia").val('');

                                    $("#txt_cliente").removeAttr('readonly');
                                    $("#txt_nif").removeAttr('readonly');
                                    $("#txt_direccion").removeAttr('readonly');
                                    $("#txt_cp").removeAttr('readonly');
                                    $("#txt_poblacion").removeAttr('readonly');
                                    $("#txt_provincia").removeAttr('readonly');
                                } else {
                                    $("#txt_cliente").val(datos[4]);
                                    $("#txt_cliente").attr('readonly', 'readonly');
                                    $("#txt_nif").val(datos[6]);
                                    $("#txt_nif").attr('readonly', 'readonly');
                                    var escalera = '';
                                    if (datos[10] != '') {
                                        escalera = "Esc: " + datos[10];
                                    }
                                    var piso = '';
                                    if (datos[11] != '') {
                                        piso = "Piso: " + datos[11];
                                    }
                                    var puerta = '';
                                    if (datos[12] != '') {
                                        puerta = "Puerta: " + datos[12];
                                    }

                                    $("#txt_direccion").val(datos[18] + ' ' + datos[8] + ', ' + datos[9] + ' ' + escalera + ' ' + piso + ' ' + puerta);
                                    $("#txt_direccion").attr('readonly', 'readonly');
                                    $("#txt_cp").val(datos[13]);
                                    $("#txt_cp").attr('readonly', 'readonly');
                                    $("#txt_poblacion").val(datos[19]);
                                    $("#txt_poblacion").attr('readonly', 'readonly');
                                    $("#txt_provincia").val(datos[20]);
                                    $("#txt_provincia").attr('readonly', 'readonly');
                                    setTimeout(function () { $("#txt_cantidad").focus(); }, 100);
                                }

                            }

                        },
                        error: function (response) { alert(response.responseText); }
                    });
                    }

            });

            //Comprobrar código cliente
            $("#txt_codigo").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {

                    if ($("#txt_codigo").val() == '') {

                        //Asigno
                        $("#txt_denominacion").val('');
                        $("#txt_precio").val('');
                        $("#txt_descuento").val('');
                        $("#txt_impuestos").val('');

                        //Liberar para que el usuario pueda escribir
                        $("#txt_denominacion").removeAttr('readonly');
                        $("#txt_precio").removeAttr('readonly');
                        $("#txt_descuento").removeAttr('readonly');
                        $("#txt_impuestos").removeAttr('readonly');

                    } else {
                        $("#btn_comprobar_articulo").click();
                    }

                }
            });

            //Comprobrar denominacion cliente
            $("#txt_denominacion").on('keydown', function (e) {
            var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    $("#btn_comprobar_articulo").click();
                }
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para comprobar el cliente
            $('#btn_comprobar_articulo').on('click', function (e) {
                
                    //asigno la cuenta
                    var codigo = $("#txt_codigo").val();
                    var elemento = "txt_codigo"
                    if (codigo != '') {
                        $.ajax({
                        async: true,
                        type: "POST",
                        url: '../default.aspx/Obtener_articulo',
                        data: "{ 'codigo': '" + codigo + "','id_empresa': '" + $(window.parent.document).find("#txt_id_empresa").val() + "'}",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "0") {

                                //Asigno
                                error('El Artículo no existe.');
                                $("#txt_codigo").val('');
                                $("#txt_denominacion").val('');
                                $("#txt_precio").val('');
                                $("#txt_descuento").val('');
                                $("#txt_impuestos").val('');

                                //Liberar para que el usuario pueda escribir
                                $("#txt_denominacion").removeAttr('readonly');
                                $("#txt_precio").removeAttr('readonly');
                                $("#txt_descuento").removeAttr('readonly');
                                $("#txt_impuestos").removeAttr('readonly');

                                //Asigno
                                setTimeout(function () { $("#" + elemento + "").focus(); }, 100);
                                $("#" + elemento + "").select();
                               
                            } else {
                                var datos = (data.d).split("|")
                                
                                if (datos[5] == "False") {

                                    //Asigno
                                    error('El Artículo esta de baja.');
                                    $("#txt_codigo").val('');
                                    $("#txt_denominacion").val('');
                                    $("#txt_precio").val('');
                                    $("#txt_descuento").val('');
                                    $("#txt_impuestos").val('');

                                    //Liberar para que el usuario pueda escribir
                                    $("#txt_denominacion").removeAttr('readonly');
                                    $("#txt_precio").removeAttr('readonly');
                                    $("#txt_descuento").removeAttr('readonly');
                                    $("#txt_impuestos").removeAttr('readonly');

                                    //Asigno
                                    setTimeout(function () { $("#" + elemento + "").focus(); }, 100);
                                    $("#" + elemento + "").select();

                                } else {

                                    //Asigno
                                    $("#txt_denominacion").val(datos[3]);
                                    $("#txt_precio").val(datos[6]);
                                    $("#txt_precio").attr('readonly', 'readonly');
                                    $("#txt_descuento").val(datos[7]);
                                    $("#txt_impuestos").val(datos[9]);
                                    $("#txt_impuestos").attr('readonly', 'readonly');

                                }

                            }

                        },
                        error: function (response) { alert(response.responseText); }
                    });
                    }

            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el focus
            $('#btn_grabar_detalle').on('focus', function (e) {

                //Dispara el grabar detalles asientos
                $('#btn_grabar_detalle').click();

            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para grabar
            $('#btn_grabar_detalle').on('click', function (e) {

                //Paro la propagacion del click
                e.preventDefault();

                //Excepciones
                if ($("#txt_cantidad").val() == '') {
                    error('La Cantidad no puede estar vacía.')
                    setTimeout(function () { $("#txt_cantidad").focus(); }, 100);
                    return false;
                }

                //if ($("#txt_codigo").val() == '') {
                //    error('El Código no puede estar vacío.')
                //    setTimeout(function () { $("#txt_codigo").focus(); }, 100);
                //    return false;
                //}

                if ($("#txt_denominacion").val() == '') {
                    error('La Denominación no puede estar vacía.')
                    setTimeout(function () { $("#txt_denominacion").focus(); }, 100);
                    return false;
                }

                if ($("#txt_precio").val() == '') {
                    error('El Precio no puede estar vacío.')
                    setTimeout(function () { $("#txt_precio").focus(); }, 100);
                    return false;
                }

                if ($("#txt_descuento").val() == '') {
                    error('El Descuento no puede estar vacía.')
                    setTimeout(function () { $("#txt_descuento").focus(); }, 100);
                    return false;
                }
                           
                //Activo
                $("#btn_grabar_detalle").unbind('click').click();

            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para grabar
            $('#btn_grabar').on('click', function (e) {

                //Paro la propagacion del click
                e.preventDefault();

                //Excepciones
                if ($("#txt_cliente").val() == '') {
                    error('El Cliente no puede estar vacío.')
                    setTimeout(function () { $("#txt_cliente").focus(); }, 100);
                    return false;
                }

                if ($("#txt_nif").val() == '') {
                    error('El NIF no puede estar vacío.')
                    setTimeout(function () { $("#txt_nif").focus(); }, 100);
                    return false;
                }
           
                //Activo
                $("#btn_grabar").unbind('click').click();

            });

            //Control para el click
            $("#img_cliente_nuevo").on('click', function (e) {
                
                //Abro Clientes
                $('#ialtas', window.parent.document).attr('src', $('#ialtas', window.parent.document).attr('src')); window.parent.abrir_ventana_relacional('altas', 'bi bi-person-lines-fill', '1000', '600', 'clientes/altas.aspx', '1');

            })

            //Control para el click
            $("#img_articulo_nuevo").on('click', function (e) {
                
                //Abro Clientes
                $('#ialtas__', window.parent.document).attr('src', $('#ialtas__', window.parent.document).attr('src')); window.parent.abrir_ventana_relacional('altas__', 'bi bi-columns-gap', '1000', '600', 'articulos/altas.aspx', '1');

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#img_impuesto_nuevo").on('click', function (e) {

                //Activo el menu
                window.parent.abrir_ventana_relacional('configuracion_','bi bi-gear-wide-connected','500','440','configuracion/configuracion.aspx','8')

                //Llamo al menú Cuentas
                padre = $(window.parent.document);
                var id_usuario = $(padre).find("#txt_id_usuario").val();
                var id_empresa = $(padre).find("#txt_id_empresa").val();
                var ruta = "configuracion/tipos_impuestos.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;
                padre = $(window.parent.document);
                $(padre).find("#iconfiguracion_").attr('src', ruta);

            });

            //Control para el click
            $("#img_presupuesto").on('click', function (e) {

                if ($("#txt_presupuesto").val() == '') {
                    //mensaje
                    error('Esta Factura no proviene de ningún Presupuesto.')
                    return false;
                }

                //Abro presupuesto
                $('#ialtas____', window.parent.document).attr('src', $('#ialtas____', window.parent.document).attr('src')); window.parent.abrir_ventana_relacional('altas____', 'bi bi-columns-gap', '1000', '600', 'presupuestos/altas.aspx|id_presupuesto=' + $("#txt_presupuesto").val() + '', '5');

            })

            //Control para el click
            $("#img_albaran").on('click', function (e) {

                if ($("#txt_albaran").val() == '') {
                    //mensaje
                    error('Esta Factura no proviene de ningún Albarán.')
                    return false;
                }

                //Abro albaran
                $('#ialtas_____',window.parent.document).attr('src',$('#ialtas_____',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('altas_____','bi bi-columns-gap','1000','600','albaranes/altas.aspx|id_albaran=' + $("#txt_albaran").val() + '','6');

            })

            //Control para el click
            $("#LB_imprimir").on('click', function (e) {

                //Mensaje al usuario
                mostrar_trabajando('Generando Factura con Certificado Digital, por favor espere.');

            })

            //Control para el click
            $("#LB_email").on('click', function (e) {

                //Mensaje al usuario
                mostrar_trabajando('Generando Factura con Certificado Digital, por favor espere.');

            })

            //Control para el click
            $("#btn_confirmar_email").on('click', function (e) {

                //Excepciones
                if ($("#txt_destinatario").val() == '') {
                    error('El Destinatario no puede estar vacío.')
                    setTimeout(function () { $("#txt_destinatario").focus(); }, 100);
                    return false;
                }

                //Oculto el pop-up
                $('#modal_email').modal('hide');

                //Mensaje al usuario
                mostrar_trabajando('Enviando E-mail, por favor espere.');

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para abonar Factura
            $('#img_abonar').on('click', function (e) {

                if ($("#txt_factura").val() == '') {
                    //mensaje
                    error('No hay ninguna factura para abonar.')
                    return false;
                }

            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para duplicar Factura
            $('#img_duplicar').on('click', function (e) {

                if ($("#txt_factura").val() == '') {
                    //mensaje
                    error('No hay ninguna factura a duplicar.')
                    return false;
                }

            });

            //Asigno alto
            $("#ver_factura").css("height", $(window).height() - 150)

            //Control para el click
            $("#ir_configuracion").on('click', function (e) {

                //Activo el menu
                window.parent.abrir_ventana_relacional('configuracion_','bi bi-gear-wide-connected','500','440','configuracion/configuracion.aspx','8')

                //Llamo al menú Cuentas
                padre = $(window.parent.document);
                var id_usuario = $(padre).find("#txt_id_usuario").val();
                var id_empresa = $(padre).find("#txt_id_empresa").val();
                var ruta = "configuracion/certificado.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;
                padre = $(window.parent.document);
                $(padre).find("#iconfiguracion_").attr('src', ruta);

            })

        });

        //Tamaño para el desktop
        $(window).resize(function (e) {

            $(".panel-container").css ('height', $(window).height() - 90 + 'px' );

            if ($("#modal_visor").is(":visible") == true) {
                //Asigno alto
                $("#ver_factura").css("height", $(window).height() - 150)
            }

        })

    </script>

    <style>

        .input-sm {
            font-size:12px; 
            background-color:#FFEDDE ; 
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
                <h5>Alta de Facturas</h5></td>
            <td></td>
            <td style="width: 40px;">
                <asp:ImageButton ID="img_nuevo" runat="server" ToolTip="Nueva Factura (Ctrl + A)" ImageUrl="~/imagenes/web/nuevo.png" Width="20" TabIndex="-1"/>
            </td>
            <td style="width: 40px;">
                <asp:ImageButton ID="img_abonar" runat="server" ToolTip="Abonar Factura (Ctrl + M)" ImageUrl="~/imagenes/web/abono.png" Width="20" TabIndex="-1"/>
            </td>
            
            <td style="width: 40px;">
                <asp:ImageButton ID="img_duplicar" runat="server" ToolTip="Duplicar Factura (Ctrl + E)" ImageUrl="~/imagenes/web/duplicar.png" Width="20" TabIndex="-1"/>
            </td>
            <td style="width: 190px; text-align: right;"><span class="text-muted"><small><asp:Label ID="lbl_tecla" runat="server" Text=""></asp:Label></small></span></td>
        </tr>
        </table>
                
        <div class="table-responsive">

        <table style="width:100%;">
        <tr>
            <td>
            
            <asp:Panel ID="PL_cabecera" runat="server" Enabled="true" CssClass="borde_entradas"> 
            
                <table style="width:100%;">
                    <tr>
                        <td>

                            <table>
                            <tr>
                                <td style="text-align :right;width:100px;">
                                    <small>Código:</small>&nbsp;
                                </td>
                                <td style="width:100px;">
                                    <asp:TextBox ID="txt_cod_cliente" runat="server" CssClass="form-control input-sm textbox_cod_cliente" MaxLength="10" width="100%"></asp:TextBox>
                                </td>
                                 <td style="text-align :right;width:80px;">
                                    <asp:LinkButton ID="img_cliente_nuevo" runat="server" CssClass="bi bi-person-lines-fill text-primary" Font-Size="13" ToolTip="Crear Cliente" TabIndex ="-1"></asp:LinkButton>
                                    <small>Cliente:</small>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_cliente" runat="server" CssClass="form-control input-sm textbox_denominacion_cliente" MaxLength="250" width="100%" style="text-transform: uppercase;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align :right;width:100px;">
                                    <small>N.I.F.:</small>&nbsp;
                                </td>
                                <td colspan ="4">
                                    <asp:TextBox ID="txt_nif" runat="server" CssClass="form-control input-sm" MaxLength="15" width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align :right;">
                                    <small>Dirección:</small>&nbsp;
                                </td>
                                <td colspan ="4">
                                    <asp:TextBox ID="txt_direccion" runat="server" CssClass="form-control input-sm" MaxLength="250" width="100%" style="text-transform: uppercase;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align :right;">
                                    <small>CP:</small>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_cp" runat="server" CssClass="form-control input-sm" MaxLength="5" width="100"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;<small> Población:</small>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_poblacion" runat="server" CssClass="form-control input-sm" MaxLength="80" width="300" style="text-transform: uppercase;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align :right;">
                                    <small>Provincia:</small>&nbsp;
                                </td>
                                <td colspan ="4">
                                    <asp:TextBox ID="txt_provincia" runat="server" CssClass="form-control input-sm" MaxLength="80" width="100%" style="text-transform: uppercase;"></asp:TextBox>
                                </td>
                            </tr>
                            </table> 

                        </td>
                        <td align="left" valign="top">

                            <table width="100">
                                <tr>
                                    <td><asp:Label ID="lbl_txt_cliente" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            
                        </td>
                        <td valign="top" align="right">

                            <table style="width:270px;">
                            <tr>
                                <td style="text-align:right;">
                                    <small>Fecha:</small>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fecha" runat="server" CssClass="form-control input-sm" MaxLength="10" width="120" ReadOnly="true" TabIndex ="-1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    <small>Nº Factura:</small>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_factura" runat="server" CssClass="form-control input-sm" MaxLength="10" width="120" ReadOnly="true" TabIndex ="-1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    <asp:LinkButton ID="img_albaran" runat="server" CssClass="bi bi-columns-gap" ForeColor="#ffc107" Font-Size="13" ToolTip="Ir a Albarán" TabIndex ="-1" Visible ="false"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <small>Nº Albarán:</small>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_albaran" runat="server" CssClass="form-control input-sm" MaxLength="10" width="120" ReadOnly="true" TabIndex ="-1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    <asp:LinkButton ID="img_presupuesto" runat="server" CssClass="bi bi-columns-gap" ForeColor="#dc3545" Font-Size="13" ToolTip="Ir a Presupuesto" TabIndex ="-1" Visible ="false"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <small>Nº Presupuesto:</small>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_presupuesto" runat="server" CssClass="form-control input-sm" MaxLength="10" width="120" ReadOnly="true" TabIndex ="-1"></asp:TextBox>
                                </td>
                            </tr>
                            </table> 

                        </td>
                    </tr>
                </table>
                
            </asp:Panel>
            <br />

            <asp:Panel ID="PL_opciones" runat="server" Enabled="true" CssClass="borde_entradas" Visible ="false"> 
                
                <table style="width:100%;">
                    <tr>
                        <td style="width:25%;">
                            <span style="font-size: 12px;"><asp:CheckBox ID="chk_isp" runat="server" Visible ="false" AutoPostBack ="true" Text="&nbsp; Inversión de Sujeto Pasivo"/></span>
                        </td>
                        <td style="width:25%;">
                            <span style="font-size: 12px;"><asp:CheckBox ID="chk_exento" runat="server" Visible ="false" AutoPostBack ="true" Text="&nbsp;Exento"/> </span>
                        </td>
                        <td style="width:25%;"></td>
                        <td style="width:25%;"></td>
                    </tr>
                </table> 
                
            </asp:Panel> 
            <br />
            

            <asp:Panel ID="PL_detalles" runat="server" CssClass="borde_entradas"> 
            
            <table style="width: 100%;">
            <tr>
                <td style="width: 100px;text-align :center;"><small>Cantidad</small></td>
                <td style="width: 100px;text-align :center;"><small>Código</small></td>
                <td style="width: 250px;text-align: center;">
                    <asp:LinkButton ID="img_articulo_nuevo" runat="server" CssClass="bi bi-columns-gap text-success" Font-Size="13" ToolTip="Crear Artículo" TabIndex ="-1"></asp:LinkButton>
                    &nbsp;&nbsp;
                    <small>Artículo</small>
                </td>
                <td style="width: 100px;text-align :center;"><small>Precio</small></td>
                <td style="width: 60px;text-align :center;"><small>Dto.</small></td>
                <td style="width: 100px;text-align :center;">
                    <asp:LinkButton ID="img_impuesto_nuevo" runat="server" CssClass="bi bi bi-menu-app text-success" Font-Size="13" ToolTip="Crear Impuesto" TabIndex ="-1"></asp:LinkButton>
                    &nbsp;&nbsp;
                    <small>% Imp.</small></td>
                <td style="width: 90px;text-align :center"></td>
            </tr>
            <tr>
                <td><asp:TextBox ID="txt_cantidad" runat="server" CssClass="form-control input-sm" MaxLength="7" width="100%"></asp:TextBox></td>
                <td><asp:TextBox ID="txt_codigo" runat="server" class="form-control input-sm textbox_cod_articulos"  MaxLength="10" width="100%"></asp:TextBox></td>
                <td><asp:TextBox ID="txt_denominacion" runat="server" CssClass="form-control input-sm textbox_denominacion_articulos" MaxLength="200" width="100%" Style="text-transform: uppercase"></asp:TextBox></td>
                <td><asp:TextBox ID="txt_precio" runat="server" CssClass="form-control input-sm" width="100%" MaxLength="8"></asp:TextBox></td>
                <td><asp:TextBox ID="txt_descuento" runat="server" CssClass="form-control input-sm" width="100%" MaxLength="5"></asp:TextBox></td>
                <td><asp:TextBox ID="txt_impuestos" runat="server" CssClass="form-control input-sm" width="100%" MaxLength="5"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btn_grabar_detalle" runat="server" Text="Siguiente" CssClass="btn btn-outline-primary btn-sm" Width="100%" />
                    <asp:Button ID="btn_modificar_detalle" runat="server" Text="Modificar" CssClass="btn btn-outline-danger btn-sm" Width="100%" Visible ="false"/>
                </td>
            </tr>
            </table>
            <br />
            <br />

            <asp:GridView ID="GV_detalles_factura"  
                AutoGenerateColumns="false" runat="server" 
                cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Detalles de la Factura" EmptyDataText="No hay detalles."
                DataKeyNames="fecha_creacion,hora_creacion,cantidad,codigo,articulo,precio,dto,impuesto,total" 
                CellPadding="4" ForeColor="#333333" GridLines="Both" 
                ShowHeaderWhenEmpty="false" AllowPaging="false" PageSize="25">
                <AlternatingRowStyle BackColor="White" />

                <Columns>

                    <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
                    <ItemTemplate>
                        <asp:Button ID="btnedit" 
                        ControlStyle-CssClass="btn btn-outline-success btn-sm" runat="server" 
                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                         tooltip="Editar este Apunte"
                        CommandName="editar" Text="Editar" TabIndex="-1"/>
                    
                    </ItemTemplate>
                        <HeaderStyle Width="80px"/>
                        <ItemStyle HorizontalAlign="Center"/>
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

                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" 
                        SortExpression="cantidad" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                   <asp:BoundField DataField="codigo" HeaderText="Código" 
                        SortExpression="codigo" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="articulo" HeaderText="Denominación" 
                        SortExpression="articulo" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                     
                    <asp:BoundField DataField="precio" HeaderText="Precio" 
                        SortExpression="precio" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="dto" HeaderText="Descuento" 
                        SortExpression="dto" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="impuesto" HeaderText="% Imp." 
                        SortExpression="impuesto" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="total" HeaderText="Total" 
                        SortExpression="total" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    </Columns>
                                
                        <HeaderStyle CssClass="table-primary"/>
                        <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

                </asp:GridView>
               
            </asp:Panel>
            <br />

            <asp:Panel ID="PL_total" runat="server" CssClass="borde_entradas" Visible ="false"> 
            
                <table style="width:100%;">
                    <tr>
                        <td style="width:50%;">

                            <asp:GridView ID="GV_totales"  
                            AutoGenerateColumns="false" runat="server" 
                            cssclass="table table-condensed" Font-Size="11px" ToolTip="Detalles del Total de la Factura" EmptyDataText="No hay detalles."
                            DataKeyNames="base_imponible,porcentaje,cuota,total" 
                            CellPadding="4" ForeColor="#333333" GridLines="Both" 
                            ShowHeaderWhenEmpty="false" AllowPaging="false" PageSize="25">
                            <AlternatingRowStyle BackColor="White" />

                            <Columns>

                                <asp:BoundField DataField="base_imponible" HeaderText="Base Imponible" 
                                    SortExpression="base_imponible" ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="porcentaje" HeaderText="Porcentaje" 
                                    SortExpression="porcentaje" ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="cuota" HeaderText="Cuota" 
                                    SortExpression="cuota" ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                    
                                <asp:BoundField DataField="total" HeaderText="Total" 
                                    SortExpression="total" ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                </Columns>
                                
                                    <HeaderStyle CssClass="table-primary"/>
                                    <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

                            </asp:GridView>

                        </td>
                        <td style="width:25%;"></td>
                        <td style="width:25%;">

                            <table style="width:100%;border-spacing:0px;border-collapse: separate;">
                                <tr>
                                    <td style="width:50%;text-align:center;"><span class="fs-4">Total</span></td>
                                </tr>
                                <tr>
                                    <td style="width:55%;text-align:center;">
                                        <asp:Label ID="lbl_total" CssClass ="text-primary" runat="server" Font-Bold="True" Font-Size="20"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                </table>
        
            </asp:Panel> 
            <br />
            
            <table style="width:100%;">
            <tr>
                <td></td>
                <td style="width:50px;" align="center"><asp:LinkButton ID="LB_imprimir" runat="server" CssClass="bi bi-printer text-primary" Font-Size="25" ToolTip="Imprimir Factura" TabIndex ="-1" Visible ="false"></asp:LinkButton></td>
                <td style="width:50px;" align="center"><asp:LinkButton ID="LB_email" runat="server" CssClass="bi bi-envelope-arrow-up text-primary" Font-Size="25" ToolTip="Enviar Factura" TabIndex ="-1" Visible ="false" ></asp:LinkButton></td>
                <td style="width:40px;"></td> 
                <td style="width:150px;"><asp:Button ID="btn_grabar" runat="server" Text="Finalizar" CssClass="btn btn-outline-primary btn-sm" Width="100%" Visible ="false" /></td>
                <td style="width:40px;"></td> 
            </tr>
            </table>

            </td>

        </tr> 
        </table>

        </div>

    </div>
        
    <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando"/>

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

    <!-- Modal Sin Certificado-->
    <div class="modal fade" id="modal_sin_certificado" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-body">

                <table style="width:100%;">
                    <tr>
                        <td>
                            <asp:LinkButton ID="Lkb_refrecar" runat="server" CssClass="bi bi-arrow-clockwise text-primary" Font-Size="13" ToolTip="Refrescar" TabIndex ="-1" Visible="true"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <span class="bi bi-sim" style="font-size:60px;color:red;"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height:25px;"></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <h5><span class="text-primary"><asp:Label ID="lbl_certificado_invalido" runat="server" Text=""></asp:Label></span></h5>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="ir_configuracion" title="Configuración" onmouseover="hand('img_configuracion')">
                                <span class="bi bi-gear-wide-connected" style="font-size:15px;"></span>
                                <span style="font-size:12px;">Ir a Configuración</span>
                            </div>
                        </td>
                    </tr>
                </table>

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
                    <button type="button" class="btn btn-outline-success" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button id="btn_grabar_confirmar" runat="server" CssClass="btn btn-outline-danger" Text="Aceptar" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Confirmar Abonar-->
    <div class="modal" id="modal_abonar" tabindex="-1" role="dialog">
        <div class="modal-dialog">

	        <div class="modal-content">
                
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Confirmación</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="LT_mensaje_abonar" runat="server"></asp:Literal>
                </div>
                    
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-success" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button id="btn_abonar_confirmar" runat="server" CssClass="btn btn-outline-danger" Text="Aceptar" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Eliminar Apunte-->
    <div class="modal" id="modal_eliminar_apunte" tabindex="-1" role="dialog">
        <div class="modal-dialog">

	        <div class="modal-content">
                
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Eliminar Línea</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="LT_mensaje_eliminar_apunte" runat="server"></asp:Literal>
                </div>
                    
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-success" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button id="btn_eliminar_apunte" runat="server" CssClass="btn btn-outline-danger" Text="Aceptar" />
                </div>
            </div>
        </div>
    </div>
    
    <!-- Modal PDF Imprimir-->
    <div class="modal fade"  id="modal_visor" role="dialog">
        <div class="modal-dialog" style="min-width: 80% !important;height:95% !important;">
    
            <!-- Modal content-->
            <div class="modal-content">
            <div class="modal-header">
                
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                
            </div>
            <div class="modal-body">
                <iframe id="ver_factura" src="" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" runat="server" width ="100%"></iframe>
            </div>
            </div>
      
        </div>
    </div>

    <!-- Modal Email-->
    <div class="modal fade"  id="modal_email" role="dialog">
        <div class="modal-dialog" style="min-width: 80% !important;height:95% !important;">
    
            <!-- Modal content-->
            <div class="modal-content">
            
            <div class="modal-header">
                <h1 class="modal-title fs-5">Enviar E-mail</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                
            </div>
            <div class="modal-body">

                <div class="input-group input-group-sm mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">
                            Destinatario:
                        </span>
                    </div>
                    <asp:TextBox ID="txt_destinatario" runat="server" class="form-control" placeholder="E-mail"  maxlength="50"></asp:TextBox>
                </div>

                <asp:PlaceHolder ID="PH_multiples_email" runat="server" Visible ="false">
                <div class="input-group input-group-sm mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">
                            Opciones:
                        </span>
                    </div>
                    <asp:DropDownList ID="DDL_opciones_mail" runat="server" class="form-control" AutoPostBack ="true"></asp:DropDownList>
                </div>
                </asp:PlaceHolder>

                <div class="input-group">
                  <span class="input-group-text">Mensaje:</span>
                  <asp:TextBox ID="txt_mensaje" runat="server" class="form-control" placeholder="Cuerpo del correo" maxlength="500" TextMode="MultiLine" Rows="10"></asp:TextBox>
                </div>

            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <asp:Button id="btn_confirmar_email" runat="server" CssClass="btn btn-primary" Text="Enviar" />
            </div>

            </div>
      
        </div>
    </div>

    <div id="partes_hidden" style="visibility:hidden; display: none;">
        <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
        <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
        txt_index:<asp:TextBox ID="txt_index" runat="server" width="400"></asp:TextBox><br />
        txt_nombre_fichero_email:<asp:TextBox ID="txt_nombre_fichero_email" runat="server" width="400"></asp:TextBox><br />
        <input id="btn_comprobar" type="button" value="button" />
        <input id="btn_comprobar_articulo" type="button" value="button" />
    </div> 

    </form>
</body>
</html>
