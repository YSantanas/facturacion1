<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>OPTIMUS | Facturación</title>
    <link rel="icon" href="imagenes/icono/icono.ico" />

    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/alertify.core.css" rel="stylesheet" />
    <link href="Content/alertify.default.css" rel="stylesheet" />
    <link href="Content/bootstrap-icons.css" rel="stylesheet" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />

    <!-- JQUERY ------------------------------------------------------------------------------------------------->
    <script src="Scripts/jquery-3.7.1.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <script src="Scripts/alertify.js"></script>
    <script src="Scripts/device.js"></script>
    <script src="Scripts/shortcut.js"></script>
    <script src="Scripts/jquery-ui-1-13.3.js"></script>

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link href="Content/desktop_movil.css" rel="stylesheet" />
    <script src="Scripts/desktop_movil.js"></script>
   <%-- <script src="Scripts46/IA.js"></script>--%>

</head>
<body>
    <form id="form1" runat="server">
        
    <!-- Body ------------------------------------------------------------------------------------------------->     
    <div id="body_contenedor" runat="server">

        <div id="partes_hidden" style="visibility:hidden; display: none;">
            txt_id_empresa:<asp:TextBox ID="txt_id_empresa" runat="server" Width="100%"></asp:TextBox><br />
            txt_id_usuario:<asp:TextBox ID="txt_id_usuario" runat="server" Width="100%"></asp:TextBox><br />
        </div>

        <div id="body_contenedor_desktop">

        <div class="transparent_negro"></div>

            <!-- Menu Noticias empresa -->
            <div id="notificaciones-charm">

                <table id="tabla_notificaciones" class="transparente">
                <tr>
                    <td>CENTRO DE NOTIFICACIONES</td>
                </tr>
                <tr>
                    <td><iframe id="notificaciones" src="actualizaciones/vacia.html" style="border: 1px solid #dedede; width: 100%; padding: 2px; overflow-y:hidden;overflow-x:hidden;" allowtransparency="true" class="barra_notificacion"></iframe></td>
                </tr>
            
                <tr>
                    <td>TAREAS DE CLAUD-IO</td>
                </tr>
                <tr>
                    <td><iframe id="notificaciones_claudia" src="actualizaciones/vacia.html" style="border: 1px solid #dedede; width: 100%; padding: 2px; overflow-y:hidden;overflow-x:hidden;" allowtransparency="true" class="barra_notificacion"></iframe></td>
                </tr>
                </table>
             
            </div>

            <!-- Iframe principal -->
            <div id="icontenido">
                <iframe id="iframe" src="actualizaciones/vacia.html"  style="width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
       
        </div> 
          
    <%--Fin del body contenedor--%>
    </div>

    <!-- Superior del Default ------------------------------------------------------------------------------------------------->
    <div id="menu_superior">

        <div id="id_fondo_barra_superior" class="transparente color"></div>
                       
        <!-- Información Usuario y Empresa -->
        <asp:Label ID="lbl_informacion_extra" runat="server" Font-Size="10px"></asp:Label>
        <asp:Literal ID="LT_barra_ocupacion_barra" runat="server"></asp:Literal>
        
        <!-- Informacion -->   
        <span id="img_informacion" class="bi bi-info-circle" style="font-size:15px;" title="Información de la empresa" onmouseover="hand('img_informacion')"></span><br />
        
        <!-- Informacion -->   
        <span id="img_contacto_superior" class="bi bi-geo-alt" style="font-size:15px;" title="Contacto" onmouseover="hand('img_contacto')"></span><br />
                
        <!-- Reloj -->   
        <div id="tabreloj">
        <table>
            <tr>
                <td style="text-align: right;padding: 3px;width: 155px;"><asp:Label ID="lbl_fecha" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right;padding: 3px;"><div id="clock"></div></td>
            </tr>
        </table> 
        </div>
            
    </div> 

    <!-- Menu  ------------------------------------------------------------------------------------------------->
    <div id="barra_menu" class="transparente color">
       
        <div id="central_inferior">
            
            <div class="nav-side-menu">
            <i class="fa fa-bars fa-2x toggle-btn" data-toggle="collapse" data-target="#menu-content"></i>
                <div class="menu-list">
                    <ul id="menu-content" class="menu-content collapse out">
                        <li data-bs-toggle="collapse" data-bs-target="#clientes" class="collapsed">
                          <a href="#"><span class="bi bi-person-lines-fill" style="margin-left: 10px;color: #0d6efd; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Clientes</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="clientes">

                            <div class="container text-center" style="margin-top:10px; ">
                                <div class="row row-cols-2 row-cols-lg-5 g-5 g-sm-3">
                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('altas','bi bi-person-lines-fill','1000','600','clientes/altas.aspx','1')">
                                        <span class="bi bi-person-lines-fill text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Altas</span>
                                    </a> 
                                </div>

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('sucursales','bi-file-earmark-break','1000','600','clientes/sucursales.aspx','1')">
                                        <span class="bi bi-file-earmark-break text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Sucursales</span>
                                    </a> 
                                </div>

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('contactos','bi bi-columns','1000','600','clientes/contactos.aspx','1')">
                                        <span class="bi bi-columns text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Contactos</span>
                                    </a> 
                                </div>

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('consultas','bi bi-search','1000','600','clientes/consultas.aspx','1')">
                                        <span class="bi bi-search text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Consultas</span>
                                    </a> 
                                </div>
                                </div>
                            </div>

                        </ul>

                        <li data-bs-toggle="collapse" data-bs-target="#proveedores" class="collapsed">
                            <a href="#"><span class="bi bi-person-lines-fill" style="margin-left: 10px;color: #6f42c1; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Proveedores</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="proveedores">

                              <div class="container text-center" style="margin-top:10px; ">
                                 <div class="row row-cols-2 row-cols-lg-5 g-5 g-sm-3">
                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('altas_','bi bi-person-lines-fill','1000','600','proveedores/altas.aspx','2')">
                                         <span class="bi bi-person-lines-fill text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Altas</span>
                                     </a> 
                                 </div>

                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('sucursales_','bi-file-earmark-break','1000','600','proveedores/sucursales.aspx','2')">
                                         <span class="bi bi-file-earmark-break text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Sucursales</span>
                                     </a> 
                                 </div>

                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('contactos_','bi bi-columns','1000','600','proveedores/contactos.aspx','2')">
                                         <span class="bi bi-columns text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Contactos</span>
                                     </a> 
                                 </div>

                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('consultas_','bi bi-search','1000','600','proveedores/consultas.aspx','2')">
                                         <span class="bi bi-search text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Consultas</span>
                                     </a> 
                                 </div>
                                 </div>
                             </div>
                        
                        </ul>

                        <li data-bs-toggle="collapse" data-bs-target="#articulos" class="collapsed">
                            <a href="#"><span class="bi bi-columns-gap" style="margin-left: 10px;color: #28a745; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Artículos</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="articulos">

                              <div class="container text-center" style="margin-top:10px; ">
                                 <div class="row row-cols-2 row-cols-lg-5 g-5 g-sm-3">
                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('altas__','bi bi-columns-gap','1000','600','articulos/altas.aspx','3')">
                                         <span class="bi bi-columns-gap text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Altas</span>
                                     </a> 
                                 </div>

                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('consultas__','bi bi-search','1000','600','articulos/consultas.aspx','3')">
                                         <span class="bi bi-search text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Consultas</span>
                                     </a> 
                                 </div>
                                 </div>
                             </div>
                        
                        </ul>

                        <li data-bs-toggle="collapse" data-bs-target="#facturas" class="collapsed">
                            <a href="#"><span class="bi bi-receipt-cutoff" style="margin-left: 10px;color: #fd7e14; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Facturas</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="facturas">

                            <div class="container text-center" style="margin-top:10px; ">
                                 <div class="row row-cols-2 row-cols-lg-5 g-5 g-sm-3">
                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('altas___','bi bi-columns-gap','1000','600','facturas/altas.aspx','4')">
                                         <span class="bi bi-columns-gap text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Altas</span>
                                     </a> 
                                 </div>

                                <div class="col">
                                     <a href="#" onclick="abrir_ventana('repetitivas','bi bi-collection','1000','600','facturas/repetitivas.aspx','4')">
                                         <span class="bi bi-collection text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Repetitivas</span>
                                     </a> 
                                 </div>

                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('consultas___','bi bi-search','1000','600','facturas/consultas.aspx','4')">
                                         <span class="bi bi-search text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Consultas</span>
                                     </a> 
                                 </div>
                                 </div>
                             </div>
                        
                        </ul>

                        <li data-bs-toggle="collapse" data-bs-target="#presupuestos" class="collapsed">
                             <a href="#"><span class="bi bi-card-list" style="margin-left: 10px;color: #dc3545; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Presupuestos</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="presupuestos">

                            <div class="container text-center" style="margin-top:10px; ">
                                <div class="row row-cols-2 row-cols-lg-4 g-4 g-sm-3">
                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('altas____','bi bi-columns-gap','1000','600','presupuestos/altas.aspx','5')">
                                        <span class="bi bi-columns-gap text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Altas</span>
                                    </a> 
                                </div>

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('consultas____','bi bi-search','1000','600','presupuestos/consultas.aspx','5')">
                                        <span class="bi bi-search text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Consultas</span>
                                    </a> 
                                </div>

                                </div>
                            </div>

                        </ul>
                        
                        <li data-bs-toggle="collapse" data-bs-target="#informes" class="collapsed">
                            <a href="#"><span class="bi bi-file-earmark-text text-secondary-emphasis" style="margin-left: 10px; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Informes</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="informes">

                             <div class="container text-center" style="margin-top:10px; ">
                                 <div class="row row-cols-2 row-cols-lg-6 g-6 g-sm-3">
                                 <div class="col">
                                      <a href="#" onclick="abrir_ventana('Facturas_emitidas','bi bi-file-earmark-text','1000','600','informes/facturas_emitidas.aspx','7')">
                                          <span class="bi bi-file-earmark-text" style="font-size:30px;"></span><br />
                                          <span style="font-size:13px;">Fact. Emitidas</span>
                                      </a> 
                                 </div>

                              
                                </div>
                            </div>
                            
                        </ul>

                        <li data-toggle="collapse" data-target="#configuracion" class="collapsed">
                            <a href="#" onclick="abrir_ventana('configuracion_','bi bi-gear-wide-connected','500','440','configuracion/configuracion.aspx','8')"><span class="bi bi-gear-wide-connected text-secondary-emphasis" style="margin-left: 10px;font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Configuración</span></a>
                        </li>
              
                        <li data-bs-toggle="collapse" data-bs-target="#supervisor" class="collapsed">
                            <a href="#" onclick="abrir_ventana('herramientas','bi bi-tools','700','600','herramientas/herramientas.aspx','9')"><span class="bi bi-tools text-secondary-emphasis" style="margin-left: 10px;font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Herramientas</span></a>
                        </li>

                        <%--   
                        <div id="Ph_Inmovilizado" style="visibility:hidden; display:none;">
                        <li data-bs-toggle="collapse" data-bs-target="#inmovilizados" class="collapsed">
                             <a href="#"><span class="bi bi-buildings" style="margin-left: 10px;color: #dc3545; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Inmovilizados</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="inmovilizados">

                            <div class="container text-center" style="margin-top:10px; ">
                                <div class="row row-cols-2 row-cols-lg-4 g-4 g-sm-3">
                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('entradas__','bi bi-buildings','700','500','inmovilizado/entrada_inmovilizado.aspx','3')">
                                        <span class="bi bi-buildings text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Entradas</span>
                                    </a> 
                                </div>

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('consultas__','bi bi-search','700','500','inmovilizado/consulta_inmovilizados.aspx','3')">
                                        <span class="bi bi-search text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Consultas</span>
                                    </a> 
                                </div>

                                <div class="col">
                                     <a href="#" onclick="abrir_ventana('contabilizacion','currency_exchange','700','500','inmovilizado/contabilizar_inmovilizado.aspx','3')">
                                         <span class="bi bi-card-checklist text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Contabilización</span>
                                     </a> 
                                </div>

                                </div>
                            </div>

                        </ul>
                        </div>
                           
                        <div id="Ph_Gestion_documental" style="visibility:hidden; display:none;">
                        <li data-bs-toggle="collapse" data-bs-target="#gestion" class="collapsed">
                             <a href="#"><span class="bi bi-folder-plus" style="margin-left: 10px;color: #6610f2; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Gestión Documental</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="gestion">

                            <div class="container text-center" style="margin-top:10px; ">
                                <div class="row row-cols-2 row-cols-lg-4 g-4 g-sm-3">
                                <div class="col">
                                     <a href="#" onclick="abrir_ventana('IO_almacenamiento','bi-hdd','800','500','gestion/visor_documentos.aspx','12')">
                                         <span class="bi bi-hdd text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Almacenamiento</span>
                                     </a> 
                                </div>

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('consultas______','bi bi-search','800','500','gestion/consulta_gestion.aspx','12')">
                                        <span class="bi bi bi-search text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Consultas</span>
                                    </a> 
                                </div>

                                </div>
                            </div>
                            
                        </ul>
                        </div>

                        <li data-bs-toggle="collapse" data-bs-target="#estadisticas_bi" class="collapsed">
                             <a href="#"><span class="bi bi-graph-up-arrow text-secondary-emphasis" style="margin-left: 10px; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Estadísticas</span><i class="bi bi-caret-down"></i></a>
                        </li>
                         <ul class="sub-menu collapse" id="estadisticas_bi">

                             <div class="container text-center" style="margin-top:10px; ">
                                <div class="row row-cols-2 row-cols-lg-4 g-4 g-sm-3">
                                <div class="col">
                                     <a href="#" onclick="abrir_ventana('bi_asientos','bi bi-graph-up-arrow','600','690','estadisticas_bi/bi_asientos.aspx','10')">
                                         <span class="bi bi-graph-up-arrow text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">BI Asientos</span>
                                     </a> 
                                </div>

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('bi_impuestos','bi bi-graph-up-arrow','600','690','estadisticas_bi/bi_impuestos.aspx','10')">
                                        <span class="bi bi-graph-up-arrow text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">BI Impuestos</span>
                                    </a> 
                                </div>

                                </div>
                            </div>

                         </ul>--%>

                        <li data-bs-toggle="collapse" data-bs-target="#ayuda" class="collapsed">
                             <a href="#"><span class="bi bi-question-square text-secondary-emphasis" style="margin-left: 10px; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Ayuda</span><i class="bi bi-caret-down"></i></a>
                        </li>
                         <ul class="sub-menu collapse" id="ayuda">

                             <div class="container text-center" style="margin-top:10px; ">
                                <div class="row row-cols-2 row-cols-lg-4 g-4 g-sm-3">
                                <div class="col">
                                     <a href="#" onclick="abrir_ventana('Atencion_cliente','bi bi-ticket-perforated','910','500','actualizaciones/tickets.aspx','11')">
                                         <span class="bi bi-ticket-perforated text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Atención al Cliente</span>
                                     </a> 
                                </div>

                                <div class="col">
                                    <a href="https://www.iperiusremote.com/download-iperius-remote-desktop-windows.aspx" target="_blank">
                                        <span class="bi bi-router text-secondary" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Soporte On-line</span>
                                    </a> 
                                </div>

                                <div class="col">
                                    <div id="img_contacto">
                                         <span class="bi bi-geo-alt text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:12px;">Contacto</span>
                                    </div>
                                </div>

                                </div>
                            </div>

                         </ul>
  
                        </li>
                    </ul>
                 </div>
            </div>
            
        </div>

        <div id="inferior_menu">
            
            <table style="width:480px;height:50px;">
            <tr>
                <td style="width:100px;text-align:right">
                    <a href="#" onclick="abrir_ventana('configuracion_','engineering','500','440','configuracion/configuracion.aspx','8')">
                        <div id="div_logo">
                            <asp:Literal ID="Lt_foto" runat="server"></asp:Literal>
                        </div>
                    </a>
                </td>
                <td style="width:10px;"></td>
                <td style="text-align:left; "><span id="lblnombre2"></span></td>
            </tr>
            </table>
            
            <span id="img_salir" class="bi bi-box-arrow-right" style="position :absolute; top:4px; right:50px;color:Red;font-size:30px;" title="Salir de la Aplicación" onmouseover="hand('img_salir')"></span>

        </div> 

    </div>

    <!-- Pie del Default ------------------------------------------------------------------------------------------------->
    <div id="menu">
           
        <div id="id_fondo_barra_inferior" class="transparente color"></div>

        <!-- Imagen Inicio --->
        <img id="img_inicio" src="imagenes/logo/icono_windows_facturacion.png" title="Despliega las opciones, aquí comienza todo" onmouseover="hand('img_inicio')"/>

        <div id="gear_barra">

            <table style="height:50px;">
            <tr>
                <td style="width:30px;text-align:center;">
                    <!-- Limpiar Pantalla --->
                    <span id="img_limpia_pantalla" class="bi bi-border-outer" style="font-size:15px;" title="Limpiar ventanas (Tecla: F5)" onmouseover="hand('img_limpia_pantalla')"></span>
                </td>
                <td style="width:30px;text-align:center;">
                    <!-- Full Screen -->
                    <span id="full_pantalla" class="bi bi-arrows-fullscreen" style="font-size:15px;" title="Pantalla Completa (Tecla: F4)" onclick="toggleFullScreen()" onmouseover="hand('full_pantalla')"></span>
                </td>
                <td style="width:30px;text-align:center;">
                    <!-- Sonido del sistema -->     
                    <div id="imagen_sonido" title="Sonido del sistema" onmouseover="hand('imagen_sonido')"></div>
                </td>
                <td style="width:30px;text-align:center;">
                    <!-- Notificaciones -->            
                    <div id="lbl_n_notificaciones"></div>
                    <span id="img_notificaciones" class="bi bi-inbox" style="font-size:20px;" title="Notificaciones (Tecla: F8)" onmouseover="hand('img_notificaciones')"></span>
                </td> 
            </tr>
            </table>

        </div> 

    </div>
   
    <!-- Modal Sonido-->
    <div class="modal" id="modal_sonido" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
      <div class="modal-dialog">

	    <div class="modal-content">

		    <div class="modal-header">
                <h1 class="modal-title fs-5">Sonido</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
		    </div>
		    <div class="modal-body">

                    <asp:Label ID="lbl_volumen" runat="server" Text="Volumen:"></asp:Label>
                    <asp:TextBox ID="txt_volumen" runat="server" type="range" class="form-range" min="0" max="100" step="1" ></asp:TextBox>

                    <asp:Label ID="lbl_tono" runat="server" Text="Tono:"></asp:Label>
                    <asp:TextBox ID="txt_tono" runat="server" type="range" class="form-range" min="0" max="20" step="1"></asp:TextBox>

                    <asp:Label ID="lbl_velocidad" runat="server" Text="Velocidad:"></asp:Label>
                    <asp:TextBox ID="txt_velocidad" runat="server" type="range" class="form-range" min="0" max="20" step="1"></asp:TextBox>
           
                <br />
		    </div>
		   
	    </div>
      </div>
    </div>

    <!-- Modal Informacion-->
    <div class="modal" id="modal_informacion" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
      <div class="modal-dialog" style="min-width:40%;">

	    <div class="modal-content">
            
            <div class="modal-header">
                <h1 class="modal-title fs-5">Información</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
		    <div class="modal-body">
                <!-- content goes here -->
                <div class="form-group">
                   <label style="color: #004085;background-color: #cce5ff; width: 100%">Global</label>
                   <p>Empresa: &nbsp;<span id="lblcodigo"></span> - <span id="lblempresa"></span><br />
                   N.I.F.: &nbsp;<span id="lblnif"></span><br />
                   Tlf.: &nbsp;<span id="lbltelefono"></span><br />
                   Email: &nbsp;<span id="lblemail"></span></p>
                    
                   <table style="width:100%;border:0px solid red;">
                   <tr>
                        <td style="color: #004085;background-color: #cce5ff; width: 25%"><label>Usuario</label></td>
                        <td style="color: #004085;background-color: #cce5ff; width: 25%"><label>Nivel</label></td>
                   </tr>
                    <tr>
                        <td><span id="lblnombre"></span></td>
                        <td><span id="lblnivel"></span></td>
                   </tr>
                   </table>
                   <p></p>

                   <table style="width:100%;border:0px solid red;">
                   <tr>
                        <td style="color: #004085;background-color: #cce5ff; width: 25%"><label>Bbdd</label></td>
                        <td style="color: #004085;background-color: #cce5ff; width: 25%"><label></label></td>
                   </tr>
                    <tr>
                        <td><span id="lblbbdd"></span></td>
                        <td></td>
                   </tr>
                   </table>
                    <p></p>

                   <table style="width:100%;border:0px solid red;">
                   <tr>
                        <td style="color: #004085;background-color: #cce5ff; width: 20%"><label>Datos</label></td>
                        <td style="color: #004085;background-color: #cce5ff; width: 20%"><label>Backup</label></td>
                        <td style="color: #004085;background-color: #cce5ff; width: 20%"><label>Gest. Documental</label></td>
                        <td style="color: #004085;background-color: #cce5ff; width: 20%"><label>Almacén</label></td>
                        <td style="color: #004085;background-color: #cce5ff; width: 20%"><label>Total</label></td>
                   </tr>
                    <tr>
                        <td><asp:Label ID="lbl_datos" runat="server" Text=""></asp:Label></td>
                        <td><asp:Label ID="lbl_backup" runat="server" Text=""></asp:Label></td>
                        <td><asp:Label ID="lbl_gestion_documental" runat="server" Text=""></asp:Label></td>
                        <td><asp:Label ID="lbl_almacen" runat="server" Text=""></asp:Label></td>
                        <td><asp:Label ID="lbl_total" runat="server" Text=""></asp:Label></td>
                   </tr>
                   </table>
                   <p></p>

                   <label style="color: #004085;background-color: #cce5ff; width: 100%">Espacio Ocupado</label>
                   <asp:Literal ID="LT_barra_ocupacion" runat="server"></asp:Literal>
                   <p></p>
                   
                   <label style="color: #004085;background-color: #cce5ff; width: 100%">Key</label>
                   <p><asp:Label ID="lblkey" runat="server" Text=""></asp:Label></p>
                   
                   <label style="color: #004085;background-color: #cce5ff; width: 100%">Time-Session</label>
                   <p><span id="TimeLeft"></span></p>
                 </div>
		    </div>
		    
	    </div>
      </div>
    </div>

    <!-- Modal Salir-->
    <div class="modal" id="modal_salir" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
      <div class="modal-dialog">

	    <div class="modal-content">

		    <div class="modal-header">
                <h1 class="modal-title fs-5">Cerrar Sesión</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
		    </div>
		    <div class="modal-body">

                <p>¿Está seguro de querer cerrar la aplicación?</p>
                   
                <br />
		    </div>
		    <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            <button id="btn_cerrar" type="button" class="btn btn-danger ">Salir</button>
          </div>
	    </div>
      </div>
    </div>

    <!-- Modal Inactividad-->
    <div class="modal" id="modal_inactividad" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
      <div class="modal-dialog">

	    <div class="modal-content">

            <div class="modal-header">
                <h1 class="modal-title fs-5">Inactividad Detectada</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
    	     <div class="modal-body">
                Ha estado sin actividad durante 20 minutos <br />
                ¿Desea seguir conectado o desconectar por seguridad?
            </div>
            <div class="modal-footer">
                <button id="btn_ampliar_session" type="button" class="btn btn-primary">Ampliar tiempo</button>
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            </div>
	    </div>
      </div>
    </div>
    
    <!-- Modal Contacto-->
    <div class="modal" id="modal_contacto" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
      <div class="modal-dialog" style="min-width:70%;">

	    <div class="modal-content">
            
            <div class="modal-header">
                <h1 class="modal-title fs-5">Contacto</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
		    <div class="modal-body">

                <div class="row g-4 py-5 row-cols-1 row-cols-lg-3">
                  <div class="col d-flex align-items-start">
                    <div>
                        <i class="bi bi-globe text-primary" style="font-size:22px; "></i>
                        <a href="https://www.optimus-soluciones.com" target ="_blank" style="font-size:22px;"> www.optimus-soluciones.com</a>
                    </div>
                  </div>
                  <div class="col d-flex align-items-start">
                    <div>
                        <i class="bi bi-telephone text-primary" style="font-size:22px; "></i>
                        <a href="tel:+34928583936" style="font-size:22px;">928583936</a> - <a href="tel:+34928583936" style="font-size:22px;">922937249</a>
                    </div>
                  </div>
                  <div class="col d-flex align-items-start">
                    <div>
                        <i class="bi bi-envelope-at text-primary" style="font-size:22px; "></i>
                        <a href="mailto:info@optimizalo.com" style="font-size:22px;">info@optimizalo.com</a>
                    </div>
                  </div>
                </div>

                <div class="row g-4 py-5 row-cols-1 row-cols-lg-3">
                  <div class="col d-flex align-items-start">
                    <div class="icon-square bg-light text-primary bi bi-geo-alt flex-shrink-0 me-3"></div>
                    <div>
                        <h5>Gran Canaria</h5>
                        <hr />
                        Calle Diego Ordaz, nº 5
                        <br />
                        37007, Las Palmas de Gran Canaria
                        <br /><br />
                        <span class="text-primary">Horario:</span>
                        <br />
                        Lunes a Jueves: 9:00h a 13:00h y de 16:00h a 18:00h.
                        Viernes: 8:00h a 14:00h.
                    </div>
                  </div>
                  <div class="col d-flex align-items-start">
                    <div class="icon-square bg-light text-primary bi bi-geo-alt flex-shrink-0 me-3"></div>
                    <div>
                        <h5>Tenerife</h5>
                        <hr />
                        Calle Santa Rosalía, nº 49
                        <br />
                        Planta 3, Oficina C
                        <br />
                        37002, Santa Cruz de Tenerife
                        <br /><br />
                        <span class="text-primary">Horario:</span>
                        <br />
                        Lunes a Jueves: 9:00h a 13:00h y de 16:00h a 18:00h.
                        Viernes: 8:00h a 14:00h.
                    </div>
                  </div>
                  <div class="col d-flex align-items-start">
                    <div class="icon-square bg-light text-primary bi bi-geo-alt flex-shrink-0 me-3"></div>
                    <div>
                        <h5>Cádiz</h5>
                        <hr />
                        Avenida San Severiano, nº 47
                        <br />
                        Local 3
                        <br />
                        11012, Cádiz
                        <br /><br />
                        <span class="text-primary">Horario:</span>
                        <br />
                        Lunes a Jueves: 9:00h a 13:00h y de 16:00h a 18:00h.
                        Viernes: 8:00h a 14:00h.
                    </div>
                  </div>
                </div>
               
		    </div>
		    
	    </div>
      </div>
    </div>

    </form>

     <%--Carga del Time Session--%>
    <script type="text/javascript">
         lessMinutes();
    </script>

</body>
</html>
