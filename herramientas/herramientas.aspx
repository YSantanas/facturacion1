<%@ Page Language="VB" AutoEventWireup="false" CodeFile="herramientas.aspx.vb" Inherits="herramientas_herramientas" %>

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

        function abrir_sub_ventana_herramientas(idDiv) {

            //Declaro
            padre = $(window.parent.document);
            var id_usuario = $(padre).find("#txt_id_usuario").val();
            var id_empresa = $(padre).find("#txt_id_empresa").val();
            var ruta = "herramientas/" + idDiv + ".aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;

            //Informo de resolución de pantalla baja, para PC
            if (device.mobile() === false && device.tablet() === false) {
                
                $(padre).find("#iherramientas").attr('src', ruta);

            } else {

                $(padre).find("#iframe").attr('src', ruta);

            }

         }

        function resaltar(idDiv) {

            $("#" + idDiv).css({ 'cursor': 'pointer' });
            $("#" + idDiv).css({ 'border': '1px solid #0d6efd' });

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
            <br>

            <div class ="table-responsive">
            <table style="width:100%;" border="0">
            <tr>
                <td valign="middle">
                    
                    <table id="registros_usuarios" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-person-workspace text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">Registros de Usuarios</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Permite visualizar todas las operaciones realizadas por uno o varios usuarios</span></td>
                        </tr>
                    </table>

               </td>
               <td valign="middle">
                    
                    <table id="datos_activos" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo sesiones, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-eye-fill text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">Sesión Activa</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Visualiza todos los datos cargados de su sesión</span></td>
                        </tr>
                    </table>

                </td>
                <td valign="middle">
                    
                    <table id="errores" style="border:1px solid white;height:100%;" onclick="mostrar_trabajando('Leyendo errores, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-bug text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">Errores</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Compilación de errores detectados</span></td>
                        </tr>
                    </table>

                </td>
                <td valign="middle">
                    
                    <table id="sql" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-filetype-sql text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">SQL</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Consultas SQL (Lenguaje de Consulta Estructurada)</span></td>
                        </tr>
                    </table>

                </td>
                
            </tr>
                <tr><td colspan ="4" style="height: 20px;"></td></tr>
            <tr>
                <td valign="middle">
                    
                    <table id="limpiar_entorno" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-radioactive text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">Limpiar Entorno</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Permite limpiar las variables del entorno e inicializarlas</span></td>
                        </tr>
                    </table>

                </td>
               <td valign="middle">
                    
                    <table id="ping" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-speedometer2 text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">Test de Velocidad</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Realiza pruebas de rendimiento de velocidad contra el servidor</span></td>
                        </tr>
                    </table>

                </td>
                <td valign="middle">

                    <table id="exportar" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-device-hdd text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">Copia de Seguridad (Manual)</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Genera una copia de sus datos para guardarla en su equipo</span></td>
                        </tr>
                    </table>
                     
                </td>
                <td valign="middle">
                    
                    <table id="backupcloud" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo puntos de restauración, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="bi bi-hdd-network text-primary" style="font-size:40px; margin-left: 10px; margin-right: 10px;"></span></td>
                            <td valign="middle"><span style="font-size:12px; color:#0d6efd;">BackupCloud</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Crea copias en la nube o restáuralas cuando lo necesites</span></td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
               <tr><td colspan ="4" style="height: 20px;"></td></tr>
            <tr>
                <td valign="middle">
                        
                   <%-- <table id="eliminar_empresa" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo empresa(s), por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>domain_disabled</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Eliminar Empresa</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Elimina de forma permanente cualquiera de sus empresas</span></td>
                        </tr>
                    </table>--%>

                </td>
               <td valign="middle">
                    
                     <%--<table id="borrar_apertura_cierre" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>track_changes</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Eliminar Apertura-Cierre</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Permite borrar forzosamente los asientos de Apertura y Cierre generados por el cierre del ejercicio</span></td>
                        </tr>
                    </table>--%>

                    
                </td>
                <td valign="middle">

                    <%--<table id="registros_notificaciones" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo notificaciones, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>push_pin</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Registros de Notificaciones</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Permite visualizar todas las notificaciones recibidas por uno o varios usuarios</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                      
                   <%-- <table id="ajuste_bbdd" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo estructura, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>build</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Ajuste BBDD</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Indexa todas las tablas para un correcta distribución de los datos</span></td>
                        </tr>
                    </table>--%>
                   
                </td>
                
            </tr>
                <tr><td colspan ="4" style="height: 20px;"></td></tr>
            <tr>
                <td valign="middle">
                    
                    <%--<table id="borrar_asiento_forzoso" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>delete_sweep</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Borrar Asiento Forzoso</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Elimina sin ningún tipo de restricción asientos contables</span></td>
                        </tr>
                    </table>--%>
                    
                </td>
               <td valign="middle">
                    
                  <%-- <table id="cambio_cuenta" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>display_settings</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Cambio de Cuenta</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Permite cambiar la denominación, NIF y datos contables</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">

                    <%--<table id="relacion_impuesto_asiento" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>biotech</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Verificar Impuestos</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Verifica la integridad de datos entre impuestos y contabilidad</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                      
                    <%--<table id="busca_cuentas_vacias" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>troubleshoot</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Verificar Cuentas Contables</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Detecta contables con cuentas vacías o inexistentes <br />en el Plan de Cuentas, NIF incorrectos y Marcas del 347/415</span></td>
                        </tr>
                    </table>--%>

                </td>
                
            </tr>
                 <tr><td colspan ="4" style="height: 20px;"></td></tr>
            <tr>
                <td valign="middle">
                    
                    <%--<table id="acelerador" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo estructura de BBDD, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>speed</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">Acelerador BBDD</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Estructura y optimiza la BBDD</span></td>
                        </tr>
                    </table>--%>

                </td>
                
                <td valign="middle">
                    
                    <%--<table id="importador_IO" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/>cloud_upload</span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">IO Importador</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Importa datos contables desde aplicaciones externas</span></td>
                        </tr>
                    </table>--%>

                </td>

                <td valign="middle">

                     <%--<table id="exportador_IO" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/> cloud_download </span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">IO Exportador</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Exporta datos contables hacia aplicaciones externas</span></td>
                        </tr>
                    </table>--%>

                </td>
               
                <td valign="middle">

                    <%--<table id="InBox_IO" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><span class="material-icons" style="font-size:40px; margin-left: 10px; margin-right: 10px; color: #155724;"/> move_to_inbox </span></td>
                            <td valign="middle"><span style="font-size:12px; color:#155724;">IO InBox</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Recibir documentación desde varios sitios como pueden ser equipos locales o incluso dispositivos móviles</span></td>
                        </tr>
                    </table>--%>

                </td>
                
            </tr>
                <tr><td colspan ="4" style="height: 20px;"></td></tr>
            <tr>
                <td valign="middle">
                    
                    <%--<table id="importar_fichero" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><img src="../imagenes/web/Data-Import.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td valign="middle"><span style="font-size:12px; color:#146abd;">Importar Fichero</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Importa datos contables desde aplicaciones externas</span></td>
                        </tr>
                    </table>--%>

                </td>
               <td valign="middle">
                    
                    <%--<table id="importar_norma43" style="border:1px solid white;" onclick="abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><img src="../imagenes/web/Headers download-WF.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td valign="middle"><span style="font-size:12px; color:#146abd;">Importar Norma 43</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Importa datos contables desde ficheros generados por los distintos bancos</span></td>
                        </tr>
                    </table>--%>

                       <%--   <table id="importar_nominas" style="border:1px solid white;" onclick="abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><img src="../imagenes/web/Import-01-WF.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td valign="middle"><span style="font-size:12px; color:#146abd;">Importar Nóminas</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Importa datos contables desde distintos programas de nóminas</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">

                    <%--<table id="registros_notificaciones" style="border:1px solid white;" onclick="mostrar_trabajando('Leyendo notificaciones, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><img src="../imagenes/web/Pinned.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td valign="middle"><span style="font-size:12px; color:#146abd;">Registros de Notificaciones</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Permite visualizar todas las notificaciones recibidas por uno o varios usuarios</span></td>
                        </tr>
                    </table>--%>

                </td>
                <td valign="middle">
                      
                    <%--<table id="importar_contabilidad" style="border:1px solid white;" onclick="mostrar_trabajando('Preparando menú, por favor espere.');abrir_sub_ventana_herramientas(this.id);" onmouseover="resaltar(this.id);" onmouseout="desresaltar(this.id);"  >
                        <tr>
                            <td valign="middle"><img src="../imagenes/web/Data_Import_Tinfor.png" style="height:30px; margin-left: 10px; margin-right: 10px;" /></td>
                            <td valign="middle"><span style="font-size:12px; color:#146abd;">Importar Contabilidad de Tinfor</span><br />
                            <span style="font-size:12px; color:#424242; margin-right:10px;">Permite importar ficheros directamente desde la contabilidad de Tinfor</span></td>
                        </tr>
                    </table>--%>

                </td>
                
            </tr>
            </table>
            </div>

        </div>

        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

    </form>
</body>
</html>
