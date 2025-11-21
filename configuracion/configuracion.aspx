<%@ Page Language="VB" AutoEventWireup="false" CodeFile="configuracion.aspx.vb" Inherits="configuracion_configuracion" %>

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

        function abrir_sub_ventana_configuracion(idDiv) {

            //Declaro
            padre = $(window.parent.document);
            var id_usuario = $(padre).find("#txt_id_usuario").val();
            var id_empresa = $(padre).find("#txt_id_empresa").val();
            var ruta = "configuracion/" + idDiv + ".aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;

            //Informo de resolución de pantalla baja, para PC
            if (device.mobile() === false && device.tablet() === false) {
                
                $(padre).find("#iconfiguracion_").attr('src', ruta);

            } else {

                $(padre).find("#iframe").attr('src', ruta);

            }

        }

        function resaltar(idDiv) {

            $("#" + idDiv).css({ 'cursor': 'pointer' });
            $("#" + idDiv).css({ 'border': '1px solid #424242' });

        }

        function desresaltar(idDiv) {
    
            $("#" + idDiv).css({ 'border': '1px solid white' });

        }

    </script>

</head>
<body>
    <form id="form1" runat="server">

        <table style="width:100%; height: 90px; background-color :#F2F2F2;">
        <tr>
            <td style="width:100px; text-align:center;">
                <asp:Literal ID="Lt_foto" runat="server"></asp:Literal>
            </td>
            <td style="width:300px;">
                Usuario: <asp:Label ID="lbl_usuario" runat="server" Text="..." Font-Bold="True"></asp:Label><br />
                Nivel: <asp:Label ID="lbl_cargo" runat="server" Text="..." Font-Bold="True"></asp:Label>
            </td>
            <td><div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div></td>
        </tr>
        </table>
        
        <div class="container-fluid">
            <br />

            <div class="table-responsive">
            <table style="width:100%;">
            <tr>
                <td valign="middle">
                    
                    <table id="personalizar_escritorio" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-display" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #007bff;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#007bff;">Personalizar Escritorio</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Modifica la imagen de tu escritorio</span></td>
                        </tr>
                    </table>

                </td>
               <td valign="middle">
                    
                    <table id="personalizar_logo" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-file-earmark-richtext" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #007bff;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#007bff;">Personalizar Logo</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Modifica el logo de la empresa</span></td>
                        </tr>
                    </table>

                </td>
                <td valign="middle">
                    
                    <table id="personalizar_usuario" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-person-fill-gear" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #007bff;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#007bff;">Personalizar Usuario</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Cambia nombre, contraseña, nivel, etc</span></td>
                        </tr>
                    </table>

                </td>
                <td valign="middle">
                    
                    <table id="email" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-envelope-at text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px;" class="text-primary">E-mail</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Modifica los datos para enviar los E-mail</span></td>
                        </tr>
                    </table>

               </td>
            </tr>
            <tr><td colspan ="4" style="height:20px;"></td></tr>
            <tr>
                <td valign="middle">
                    
                     <table id="empresa" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo Empresa, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-building" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #007bff;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#007bff;">Empresa</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Configura los apartados y funciones que estarán disponibles en esta empresa para su utilización</span></td>
                        </tr>
                    </table>
                  
                </td>
                <td valign="middle">
                    
                    <table id="gestion_usuarios" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-people-fill" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #007bff;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#007bff;">Gestión de Usuarios</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los usuarios que pueden acceder a esta empresa</span></td>
                        </tr>
                    </table>

                </td>
                <td valign="middle">
                    
                      <%--<table id="api" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>data_object</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">API</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Activa conexión exterior</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                    
                    <%--<table id="configurar_claudia" style="border:1px solid white;" onclick="abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><img src="../imagenes/web/Female-03.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td valign="middle"><span style="font-size:12px; color:#146abd;">ClaudIA</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">ClaudIA es nuestra asistenta de Inteligencia Artificial.<br /> Configure dónde quiere que intervenga</span></td>
                        </tr>
                    </table>--%>

                </td>
            </tr>
            </table>
            </div> 
            <br />

            <div class="table-responsive">
            <table style="width:100%;background-color: #fd7e14;">
            <tr>
                <td><span style="margin-left:20px;color:#000000;font-size:14px;">Facturas</span></td>
            </tr>
            </table>

            <table style="width:100%;">
            <tr>
                <td valign="middle">
                    
                    <table id="tipos_impuestos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-card-checklist" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #fd7e14;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#fd7e14;">Tipos de Impuestos</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los tipos de impuestos utilizados como pueden ser % para el IGIC,IVA o IPSI</span></td>
                        </tr>
                    </table>

                </td>
               
                <td valign="middle">

                     <table id="certificado" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-sim" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #fd7e14;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#fd7e14;">Certificado Digital</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona tu Certificado Digital para poder emitir facturas</span></td>
                        </tr>
                    </table>

                </td>
                <td valign="middle">
                   
                     <table id="fondo_factura" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-image" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #fd7e14;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#fd7e14;">Diseño de Factura</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Modifica el diseño de tu factura</span></td>
                        </tr>
                    </table>

                    <td valign="middle">
                    
                    <%--<table id="empresa" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo Empresa, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>corporate_fare</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Empresa</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Configura los apartados y funciones que estarán disponibles en esta empresa para su utilización</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                    
                    <%--<table id="configurar_claudia" style="border:1px solid white;" onclick="abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><img src="../imagenes/web/Female-03.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td valign="middle"><span style="font-size:12px; color:#146abd;">ClaudIA</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">ClaudIA es nuestra asistenta de Inteligencia Artificial.<br /> Configure dónde quiere que intervenga</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                
                </td> 
            </tr>
            </table>
            </div> 
            <br />

            <div class="table-responsive">
            <table style="width:100%;background-color: #ffc107;">
            <tr>
                <td><span style="margin-left:20px;color:black;font-size:14px;">Albaranes</span></td>
            </tr>
            </table>

            <table style="width:100%;">
            <tr>
                <td valign="middle">
                   
                    <table id="fondo_albaran" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-image" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #ffc107;"></span></td>
                            <td valign="middle"><span style="font-size:12px;color:#ffc107;">Diseño de Albaranes</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Modifica el diseño del Albarán</span></td>
                        </tr>
                    </table>

                </td>

                <td valign="middle">
                    
                </td>
                <td valign="middle">
                    
                </td>
                <td valign="middle">
                    
                </td>
                
            </tr>
            </table>
            </div>
            <br />

            <div class="table-responsive">
            <table style="width:100%;background-color: #dc3545;">
            <tr>
                <td><span style="margin-left:20px;color:#000000;font-size:14px;">Presupuestos</span></td>
            </tr>
            </table>

            <table style="width:100%;">
            <tr>
                <%--<td valign="middle">
                   
                     <table id="fondo_factura" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-image" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #fd7e14;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#fd7e14;">Diseño de Factura</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Modifica el diseño de tu factura</span></td>
                        </tr>
                    </table>

                </td>
               
                <td valign="middle">

                     <table id="certificado" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-sim" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #fd7e14;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#fd7e14;">Certificado Digital</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona tu Certificado Digital para poder emitir facturas</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                   
                     <table id="fondo_presupuesto" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-image" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #dc3545;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#dc3545;">Diseño del Presupuesto</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Modifica el diseño del Presupuesto</span></td>
                        </tr>
                    </table>



                    <%--<table id="restriccion_fechas" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo restricción de fechas, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #007bff;"/>data_array</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#007bff;">Restricción de Fechas</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los límites de fechas para introducir asientos contables y otras opciones relacionadas con la misma</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                    
                </td>
                
            </tr>
            </table>
            </div>
            <br />
            
            <div class="table-responsive">
            <%--<table style="width:100%;background-color: #6f42c1;">
            <tr>
                <td><span style="margin-left:20px;color:white;font-size:14px;">Impuestos</span></td>
            </tr>
            </table>--%>

            <table style="width:100%;">
            <tr>
                <td valign="middle">
                    
                    <%--<table id="conceptos_contables" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo conceptos contables, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>space_dashboard</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6f42c1;">Conceptos Contables</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los accesos rápidos para componer la denominación a la hora de introducir impuestos</span></td>
                        </tr>
                    </table>--%>

                </td>
               <td valign="middle">

                   <%-- <table id="tipos_impuestos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>list_alt</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6610f2;">Tipos de Impuestos</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los tipos de impuestos utilizados como pueden ser IGIC,IVA o IPSI</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">

                     <%--<table id="prorrata" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo prorratas, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>table_chart</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6610f2;">Prorrata</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona las distintas prorratas aplicadas para cada ejercicio contable</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                    
                    <%--<table id="restriccion_fechas_impuestos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo restricción de fechas, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>data_object</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6610f2;">Restricción de Fechas</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los límites de fechas para introducir impuestos y otras opciones relacionadas con la misma</span></td>
                        </tr>
                    </table>--%>

                </td>
                
            </tr>
               <tr>
                <td valign="middle">
                    
                    <%--<table id="constructor_plantillas_pdf" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo constructor, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>square_foot</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6f42c1;">Constructor Plantillas PDF</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Fabrica plantillas para imputar impuestos a partir de un PDF de manera automática</span></td>
                        </tr>
                    </table>--%>

                </td>
               <td valign="middle">

                    <%--<table id="tipos_impuestos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>list_alt</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6610f2;">Tipos de Impuestos</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los tipos de impuestos utilizados como pueden ser IGIC,IVA o IPSI</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">

                    <%-- <table id="prorrata" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo prorratas, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>table_chart</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6610f2;">Prorrata</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona las distintas prorratas aplicadas para cada ejercicio contable</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                    
                   <%-- <table id="restriccion_fechas_impuestos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo restricción de fechas, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #6f42c1;"/>data_object</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#6610f2;">Restricción de Fechas</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los límites de fechas para introducir impuestos y otras opciones relacionadas con la misma</span></td>
                        </tr>
                    </table>--%>

                </td>
                
            </tr>
            </table>
            </div> 
            <br />

            <asp:PlaceHolder ID="PH_cartera" runat="server" Visible ="false">
            <div class="table-responsive">
            <%--<table style="width:100%;background-color: #FB530A;">
            <tr>
                <td><span style="margin-left:20px;color:white;font-size:14px;">Cartera</span></td>
            </tr>
            </table>--%>

            <table style="width:100%;">
            <tr>
                <td>
                    
                    <%--<table id="cobradores" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo cobradores, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td><img src="../imagenes/web/Home-Loan-WF.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td><span style="font-size:12px; color:#FB530A;">Cobradores</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los cobradores para dicha empresa</span></td>
                        </tr>
                    </table>

                </td>
               <td>

                   <%-- <table id="tipos_impuestos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo tipos de impuestos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td><img src="../imagenes/web/Money-Coin.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td><span style="font-size:12px; color:#6610f2;">Tipos de Impuestos</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona los tipos de impuestos utilizados como pueden ser IGIC,IVA o IPSI</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td>

                    <%-- <table id="prorrata" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo prorratas, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td><img src="../imagenes/web/Group-WF.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td><span style="font-size:12px; color:#6610f2;">Prorrata</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona las distintas prorratas aplicadas para cada ejercicio contable</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td>
                    
                </td>
                
            </tr>
            </table>
            </div> 
            <br />
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PH_inmo" runat="server" Visible ="false">
            <div class="table-responsive">
            <%--<table style="width:100%;background-color: #dc3545;">
            <tr>
                <td><span style="margin-left:20px;color:white;font-size:14px;">Inmovilizados</span></td>
            </tr>
            </table>--%>

            <table style="width:100%;">
            <tr>
                <td>
                    
                    <%--<table id="ubicaciones" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo Ubicaciones, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #dc3545;"/>web</span></td>
                            <td><span style="font-size:12px; color:#EE1414;">Ubicaciones</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona las ubicaciones de los distintos inmovilizados</span></td>
                        </tr>
                    </table>--%>

                </td>
               <td>

                    <%--<table id="grupos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo Grupos, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #dc3545;"/>groups</span></td>
                            <td><span style="font-size:12px; color:#EE1414;">Grupos</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Gestiona por grupos los inmovilizados que amortizas en un mismo período de años o porcentajes</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td>

                </td>
                <td>
                    
                </td>
                
            </tr>
            </table>
            <br />
            </div> 
            </asp:PlaceHolder>
            
            <div class="table-responsive">
            <%--<table style="width:100%;background-color: #ffc107;">
            <tr>
                <td><span style="margin-left:20px;color:white;font-size:14px;">Equivalencias Registro Facturas</span></td>
            </tr>
            </table>--%>

            <table style="width:100%;">
            <tr>
                <td>
                    
                    <%--<table id="equivalencia_facturas" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo Equivalencias, por favor espere.');abrir_sub_ventana_configuracion(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #ffc107;"/>share</span></td>
                            <td><span style="font-size:12px; color:#FCB200;">Equivalencia</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Crea las equivalencias necesarias para procesar el informe de registro de facturas</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td>

                </td>
                <td>

                </td>
                <td>
                    
                </td>
                
            </tr>
            </table>
            </div> 
            <br />
            
        </div>

        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

    </form>
</body>
</html>
