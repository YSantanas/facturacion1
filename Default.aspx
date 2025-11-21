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
    <link href="Content/desktop.css" rel="stylesheet" />
    <script src="Scripts/desktop.js"></script>
    <script src="Scripts/IA.js"></script>

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

        <!-- Aviso cambio de Escritorio  -->            
        <div id="aviso_cambio_escritorio"></div>
            
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
                <td>TAREAS DE INTELIGENCIA ARTIFICIAL</td>
            </tr>
            <tr>
                <td><iframe id="notificaciones_claudia" src="actualizaciones/vacia.html" style="border: 1px solid #dedede; width: 100%; padding: 2px; overflow-y:hidden;overflow-x:hidden;" allowtransparency="true" class="barra_notificacion"></iframe></td>
            </tr>
            </table>
             
        </div>
        
        <!-- Aquí coloco las opciones de Clientes -->
        <div id="altas" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-person-lines-fill icono_menu" style="color: #0d6efd;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Altas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('altas')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_altas" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('altas')"><i class="bi bi-border-all" style="height:16px;"></i></a>
                   
                         <div id="_altas_posicion_orden" class="posicion_orden">
                             
                             <div id="_altas_1" onclick="ajustar_izquierdo('altas')" class="color_posicion posicion_1" onmouseover="hand('_altas_1')"></div>
                             <div id="_altas_2" onclick="ajustar_derecho('altas')" class="color_posicion posicion_2" onmouseover="hand('_altas_2')"></div>
                             <div id="_altas_3" onclick="ajustar_izquierdo_60('altas')" class="color_posicion posicion_3" onmouseover="hand('_altas_3')"></div>
                             <div id="_altas_4" onclick="ajustar_derecho_40('altas')" class="color_posicion posicion_4" onmouseover="hand('_altas_4')"></div>                             
                             <div id="_altas_5" onclick="ajustar_izquierdo_40('altas')" class="color_posicion posicion_5" onmouseover="hand('_altas_5')"></div>                             
                             <div id="_altas_6" onclick="ajustar_derecho_60('altas')" class="color_posicion posicion_6" onmouseover="hand('_altas_6')"></div>                             
                             <div id="_altas_7" onclick="ajustar_arriba_izquierdo('altas')" class="color_posicion posicion_7" onmouseover="hand('_altas_7')"></div>
                             <div id="_altas_8" onclick="ajustar_arriba_derecho('altas')" class="color_posicion posicion_8" onmouseover="hand('_altas_8')"></div>
                             <div id="_altas_9" onclick="ajustar_abajo_izquierdo('altas')" class="color_posicion posicion_9" onmouseover="hand('_altas_9')"></div>
                             <div id="_altas_10" onclick="ajustar_abajo_derecho('altas')" class="color_posicion posicion_10" onmouseover="hand('_altas_10')"></div>
                             <div id="_altas_11" onclick="maximizar_ventana('altas')" class="color_posicion posicion_11" onmouseover="hand('_altas_11')"></div>
                             <div id="_altas_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_altas_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                             <div id="_altas_13" onclick="ajustar_arriba('altas')" class="color_posicion posicion_13" onmouseover="hand('_altas_13')"></div>
                             <div id="_altas_14" onclick="ajustar_abajo('altas')" class="color_posicion posicion_14" onmouseover="hand('_altas_14')"></div>
                             <div id="_altas_15" onclick="ajustar_arriba_40('altas')" class="color_posicion posicion_15" onmouseover="hand('_altas_15')"></div>
                             <div id="_altas_16" onclick="ajustar_abajo_60('altas')" class="color_posicion posicion_16" onmouseover="hand('_altas_16')"></div>
                             <div id="_altas_17" onclick="ajustar_arriba_60('altas')" class="color_posicion posicion_17" onmouseover="hand('_altas_17')"></div>
                             <div id="_altas_18" onclick="ajustar_abajo_40('altas')" class="color_posicion posicion_18" onmouseover="hand('_altas_18')"></div>
                             
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('altas')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_altas" class="dragresizable_hija">
                <iframe id="ialtas" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="sucursales" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-file-earmark-break icono_menu" style="color: #0d6efd;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Sucursales</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('sucursales')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
            
                    <div id="_sucursales" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('sucursales')"><i class="bi bi-border-all" style="height:16px;"></i></a>
           
                         <div id="_sucursales_posicion_orden" class="posicion_orden">
                     
                             <div id="_sucursales_1" onclick="ajustar_izquierdo('sucursales')" class="color_posicion posicion_1" onmouseover="hand('_sucursales_1')"></div>
                             <div id="_sucursales_2" onclick="ajustar_derecho('sucursales')" class="color_posicion posicion_2" onmouseover="hand('_sucursales_2')"></div>
                             <div id="_sucursales_3" onclick="ajustar_izquierdo_60('sucursales')" class="color_posicion posicion_3" onmouseover="hand('_sucursales_3')"></div>
                             <div id="_sucursales_4" onclick="ajustar_derecho_40('sucursales')" class="color_posicion posicion_4" onmouseover="hand('_sucursales_4')"></div>                             
                             <div id="_sucursales_5" onclick="ajustar_izquierdo_40('sucursales')" class="color_posicion posicion_5" onmouseover="hand('_sucursales_5')"></div>                             
                             <div id="_sucursales_6" onclick="ajustar_derecho_60('sucursales')" class="color_posicion posicion_6" onmouseover="hand('_sucursales_6')"></div>                             
                             <div id="_sucursales_7" onclick="ajustar_arriba_izquierdo('sucursales')" class="color_posicion posicion_7" onmouseover="hand('_sucursales_7')"></div>
                             <div id="_sucursales_8" onclick="ajustar_arriba_derecho('sucursales')" class="color_posicion posicion_8" onmouseover="hand('_sucursales_8')"></div>
                             <div id="_sucursales_9" onclick="ajustar_abajo_izquierdo('sucursales')" class="color_posicion posicion_9" onmouseover="hand('_sucursales_9')"></div>
                             <div id="_sucursales_10" onclick="ajustar_abajo_derecho('sucursales')" class="color_posicion posicion_10" onmouseover="hand('_sucursales_10')"></div>
                             <div id="_sucursales_11" onclick="maximizar_ventana('sucursales')" class="color_posicion posicion_11" onmouseover="hand('_sucursales_11')"></div>
                             <div id="_sucursales_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_sucursales_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                             <div id="_sucursales_13" onclick="ajustar_arriba('sucursales')" class="color_posicion posicion_13" onmouseover="hand('_sucursales_13')"></div>
                             <div id="_sucursales_14" onclick="ajustar_abajo('sucursales')" class="color_posicion posicion_14" onmouseover="hand('_sucursales_14')"></div>
                             <div id="_sucursales_15" onclick="ajustar_arriba_40('sucursales')" class="color_posicion posicion_15" onmouseover="hand('_sucursales_15')"></div>
                             <div id="_sucursales_16" onclick="ajustar_abajo_60('sucursales')" class="color_posicion posicion_16" onmouseover="hand('_sucursales_16')"></div>
                             <div id="_sucursales_17" onclick="ajustar_arriba_60('sucursales')" class="color_posicion posicion_17" onmouseover="hand('_sucursales_17')"></div>
                             <div id="_sucursales_18" onclick="ajustar_abajo_40('sucursales')" class="color_posicion posicion_18" onmouseover="hand('_sucursales_18')"></div>
                     
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('sucursales')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_sucursales" class="dragresizable_hija">
                <iframe id="isucursales" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="contactos" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-columns icono_menu" style="color: #0d6efd;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Contactos</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('contactos')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
    
                    <div id="_contactos" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('contactos')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                         <div id="_contactos_posicion_orden" class="posicion_orden">
             
                             <div id="_contactos_1" onclick="ajustar_izquierdo('contactos')" class="color_posicion posicion_1" onmouseover="hand('_contactos_1')"></div>
                             <div id="_contactos_2" onclick="ajustar_derecho('contactos')" class="color_posicion posicion_2" onmouseover="hand('_contactos_2')"></div>
                             <div id="_contactos_3" onclick="ajustar_izquierdo_60('contactos')" class="color_posicion posicion_3" onmouseover="hand('_contactos_3')"></div>
                             <div id="_contactos_4" onclick="ajustar_derecho_40('contactos')" class="color_posicion posicion_4" onmouseover="hand('_contactos_4')"></div>                             
                             <div id="_contactos_5" onclick="ajustar_izquierdo_40('contactos')" class="color_posicion posicion_5" onmouseover="hand('_contactos_5')"></div>                             
                             <div id="_contactos_6" onclick="ajustar_derecho_60('contactos')" class="color_posicion posicion_6" onmouseover="hand('_contactos_6')"></div>                             
                             <div id="_contactos_7" onclick="ajustar_arriba_izquierdo('contactos')" class="color_posicion posicion_7" onmouseover="hand('_contactos_7')"></div>
                             <div id="_contactos_8" onclick="ajustar_arriba_derecho('contactos')" class="color_posicion posicion_8" onmouseover="hand('_contactos_8')"></div>
                             <div id="_contactos_9" onclick="ajustar_abajo_izquierdo('contactos')" class="color_posicion posicion_9" onmouseover="hand('_contactos_9')"></div>
                             <div id="_contactos_10" onclick="ajustar_abajo_derecho('contactos')" class="color_posicion posicion_10" onmouseover="hand('_contactos_10')"></div>
                             <div id="_contactos_11" onclick="maximizar_ventana('contactos')" class="color_posicion posicion_11" onmouseover="hand('_contactos_11')"></div>
                             <div id="_contactos_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_contactos_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                             <div id="_contactos_13" onclick="ajustar_arriba('contactos')" class="color_posicion posicion_13" onmouseover="hand('_contactos_13')"></div>
                             <div id="_contactos_14" onclick="ajustar_abajo('contactos')" class="color_posicion posicion_14" onmouseover="hand('_contactos_14')"></div>
                             <div id="_contactos_15" onclick="ajustar_arriba_40('contactos')" class="color_posicion posicion_15" onmouseover="hand('_contactos_15')"></div>
                             <div id="_contactos_16" onclick="ajustar_abajo_60('contactos')" class="color_posicion posicion_16" onmouseover="hand('_contactos_16')"></div>
                             <div id="_contactos_17" onclick="ajustar_arriba_60('contactos')" class="color_posicion posicion_17" onmouseover="hand('_contactos_17')"></div>
                             <div id="_contactos_18" onclick="ajustar_abajo_40('contactos')" class="color_posicion posicion_18" onmouseover="hand('_contactos_18')"></div>
             
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('contactos')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_contactos" class="dragresizable_hija">
                <iframe id="icontactos" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="consultas" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-search icono_menu" style="color: #0d6efd;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Consultas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('consultas')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
    
                    <div id="_consultas" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('consultas')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                         <div id="_consultas_posicion_orden" class="posicion_orden">
     
                             <div id="_consultas_1" onclick="ajustar_izquierdo('consultas')" class="color_posicion posicion_1" onmouseover="hand('_consultas_1')"></div>
                             <div id="_consultas_2" onclick="ajustar_derecho('consultas')" class="color_posicion posicion_2" onmouseover="hand('_consultas_2')"></div>
                             <div id="_consultas_3" onclick="ajustar_izquierdo_60('consultas')" class="color_posicion posicion_3" onmouseover="hand('_consultas_3')"></div>
                             <div id="_consultas_4" onclick="ajustar_derecho_40('consultas')" class="color_posicion posicion_4" onmouseover="hand('_consultas_4')"></div>                             
                             <div id="_consultas_5" onclick="ajustar_izquierdo_40('consultas')" class="color_posicion posicion_5" onmouseover="hand('_consultas_5')"></div>                             
                             <div id="_consultas_6" onclick="ajustar_derecho_60('consultas')" class="color_posicion posicion_6" onmouseover="hand('_consultas_6')"></div>                             
                             <div id="_consultas_7" onclick="ajustar_arriba_izquierdo('consultas')" class="color_posicion posicion_7" onmouseover="hand('_consultas_7')"></div>
                             <div id="_consultas_8" onclick="ajustar_arriba_derecho('consultas')" class="color_posicion posicion_8" onmouseover="hand('_consultas_8')"></div>
                             <div id="_consultas_9" onclick="ajustar_abajo_izquierdo('consultas')" class="color_posicion posicion_9" onmouseover="hand('_consultas_9')"></div>
                             <div id="_consultas_10" onclick="ajustar_abajo_derecho('consultas')" class="color_posicion posicion_10" onmouseover="hand('_consultas_10')"></div>
                             <div id="_consultas_11" onclick="maximizar_ventana('consultas')" class="color_posicion posicion_11" onmouseover="hand('_consultas_11')"></div>
                             <div id="_consultas_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_consultas_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                             <div id="_consultas_13" onclick="ajustar_arriba('consultas')" class="color_posicion posicion_13" onmouseover="hand('_consultas_13')"></div>
                             <div id="_consultas_14" onclick="ajustar_abajo('consultas')" class="color_posicion posicion_14" onmouseover="hand('_consultas_14')"></div>
                             <div id="_consultas_15" onclick="ajustar_arriba_40('consultas')" class="color_posicion posicion_15" onmouseover="hand('_consultas_15')"></div>
                             <div id="_consultas_16" onclick="ajustar_abajo_60('consultas')" class="color_posicion posicion_16" onmouseover="hand('_consultas_16')"></div>
                             <div id="_consultas_17" onclick="ajustar_arriba_60('consultas')" class="color_posicion posicion_17" onmouseover="hand('_consultas_17')"></div>
                             <div id="_consultas_18" onclick="ajustar_abajo_40('consultas')" class="color_posicion posicion_18" onmouseover="hand('_consultas_18')"></div>
     
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('consultas')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_consultas" class="dragresizable_hija">
                <iframe id="iconsultas" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <!-- Aquí coloco las opciones de Proveedores -->
        <div id="altas_" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-person-lines-fill icono_menu" style="color: #6f42c1;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Altas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('altas_')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
             
                    <div id="_altas_" class="selector_movimiento">
                        <a href="#" title="Maximizar" onclick="maximizar_ventana('altas_')"><i class="bi bi-border-all" style="height:16px;"></i></a>
            
                        <div id="_altas__posicion_orden" class="posicion_orden">
                      
                            <div id="_altas__1" onclick="ajustar_izquierdo('altas_')" class="color_posicion posicion_1" onmouseover="hand('_altas__1')"></div>
                            <div id="_altas__2" onclick="ajustar_derecho('altas_')" class="color_posicion posicion_2" onmouseover="hand('_altas__2')"></div>
                            <div id="_altas__3" onclick="ajustar_izquierdo_60('altas_')" class="color_posicion posicion_3" onmouseover="hand('_altas__3')"></div>
                            <div id="_altas__4" onclick="ajustar_derecho_40('altas_')" class="color_posicion posicion_4" onmouseover="hand('_altas__4')"></div>                             
                            <div id="_altas__5" onclick="ajustar_izquierdo_40('altas_')" class="color_posicion posicion_5" onmouseover="hand('_altas__5')"></div>                             
                            <div id="_altas__6" onclick="ajustar_derecho_60('altas_')" class="color_posicion posicion_6" onmouseover="hand('_altas__6')"></div>                             
                            <div id="_altas__7" onclick="ajustar_arriba_izquierdo('altas_')" class="color_posicion posicion_7" onmouseover="hand('_altas__7')"></div>
                            <div id="_altas__8" onclick="ajustar_arriba_derecho('altas_')" class="color_posicion posicion_8" onmouseover="hand('_altas__8')"></div>
                            <div id="_altas__9" onclick="ajustar_abajo_izquierdo('altas_')" class="color_posicion posicion_9" onmouseover="hand('_altas__9')"></div>
                            <div id="_altas__10" onclick="ajustar_abajo_derecho('altas_')" class="color_posicion posicion_10" onmouseover="hand('_altas__10')"></div>
                            <div id="_altas__11" onclick="maximizar_ventana('altas_')" class="color_posicion posicion_11" onmouseover="hand('_altas__11')"></div>
                            <div id="_altas__12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_altas__12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                            <div id="_altas__13" onclick="ajustar_arriba('altas_')" class="color_posicion posicion_13" onmouseover="hand('_altas__13')"></div>
                            <div id="_altas__14" onclick="ajustar_abajo('altas_')" class="color_posicion posicion_14" onmouseover="hand('_altas__14')"></div>
                            <div id="_altas__15" onclick="ajustar_arriba_40('altas_')" class="color_posicion posicion_15" onmouseover="hand('_altas__15')"></div>
                            <div id="_altas__16" onclick="ajustar_abajo_60('altas_')" class="color_posicion posicion_16" onmouseover="hand('_altas__16')"></div>
                            <div id="_altas__17" onclick="ajustar_arriba_60('altas_')" class="color_posicion posicion_17" onmouseover="hand('_altas__17')"></div>
                            <div id="_altas__18" onclick="ajustar_abajo_40('altas_')" class="color_posicion posicion_18" onmouseover="hand('_altas__18')"></div>
                      
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('altas_')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_altas_" class="dragresizable_hija">
                <iframe id="ialtas_" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="sucursales_" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-file-earmark-break icono_menu" style="color: #6f42c1;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Sucursales</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('sucursales_')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
     
                     <div id="_sucursales_" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('sucursales_')"><i class="bi bi-border-all" style="height:16px;"></i></a>
    
                          <div id="_sucursales__posicion_orden" class="posicion_orden">
              
                              <div id="_sucursales__1" onclick="ajustar_izquierdo('sucursales_')" class="color_posicion posicion_1" onmouseover="hand('_sucursales__1')"></div>
                              <div id="_sucursales__2" onclick="ajustar_derecho('sucursales_')" class="color_posicion posicion_2" onmouseover="hand('_sucursales__2')"></div>
                              <div id="_sucursales__3" onclick="ajustar_izquierdo_60('sucursales_')" class="color_posicion posicion_3" onmouseover="hand('_sucursales__3')"></div>
                              <div id="_sucursales__4" onclick="ajustar_derecho_40('sucursales_')" class="color_posicion posicion_4" onmouseover="hand('_sucursales__4')"></div>                             
                              <div id="_sucursales__5" onclick="ajustar_izquierdo_40('sucursales_')" class="color_posicion posicion_5" onmouseover="hand('_sucursales__5')"></div>                             
                              <div id="_sucursales__6" onclick="ajustar_derecho_60('sucursales_')" class="color_posicion posicion_6" onmouseover="hand('_sucursales__6')"></div>                             
                              <div id="_sucursales__7" onclick="ajustar_arriba_izquierdo('sucursales_')" class="color_posicion posicion_7" onmouseover="hand('_sucursales__7')"></div>
                              <div id="_sucursales__8" onclick="ajustar_arriba_derecho('sucursales_')" class="color_posicion posicion_8" onmouseover="hand('_sucursales__8')"></div>
                              <div id="_sucursales__9" onclick="ajustar_abajo_izquierdo('sucursales_')" class="color_posicion posicion_9" onmouseover="hand('_sucursales__9')"></div>
                              <div id="_sucursales__10" onclick="ajustar_abajo_derecho('sucursales_')" class="color_posicion posicion_10" onmouseover="hand('_sucursales__10')"></div>
                              <div id="_sucursales__11" onclick="maximizar_ventana('sucursales_')" class="color_posicion posicion_11" onmouseover="hand('_sucursales__11')"></div>
                              <div id="_sucursales__12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_sucursales__12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_sucursales__13" onclick="ajustar_arriba('sucursales_')" class="color_posicion posicion_13" onmouseover="hand('_sucursales__13')"></div>
                              <div id="_sucursales__14" onclick="ajustar_abajo('sucursales_')" class="color_posicion posicion_14" onmouseover="hand('_sucursales__14')"></div>
                              <div id="_sucursales__15" onclick="ajustar_arriba_40('sucursales_')" class="color_posicion posicion_15" onmouseover="hand('_sucursales__15')"></div>
                              <div id="_sucursales__16" onclick="ajustar_abajo_60('sucursales_')" class="color_posicion posicion_16" onmouseover="hand('_sucursales__16')"></div>
                              <div id="_sucursales__17" onclick="ajustar_arriba_60('sucursales_')" class="color_posicion posicion_17" onmouseover="hand('_sucursales__17')"></div>
                              <div id="_sucursales__18" onclick="ajustar_abajo_40('sucursales_')" class="color_posicion posicion_18" onmouseover="hand('_sucursales__18')"></div>
              
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('sucursales_')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_sucursales_" class="dragresizable_hija">
                 <iframe id="isucursales_" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
         </div>

        <div id="contactos_" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-columns icono_menu" style="color: #6f42c1;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Contactos</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('contactos_')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_contactos_" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('contactos_')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_contactos__posicion_orden" class="posicion_orden">
      
                              <div id="_contactos__1" onclick="ajustar_izquierdo('contactos_')" class="color_posicion posicion_1" onmouseover="hand('_contactos__1')"></div>
                              <div id="_contactos__2" onclick="ajustar_derecho('contactos_')" class="color_posicion posicion_2" onmouseover="hand('_contactos__2')"></div>
                              <div id="_contactos__3" onclick="ajustar_izquierdo_60('contactos_')" class="color_posicion posicion_3" onmouseover="hand('_contactos__3')"></div>
                              <div id="_contactos__4" onclick="ajustar_derecho_40('contactos_')" class="color_posicion posicion_4" onmouseover="hand('_contactos__4')"></div>                             
                              <div id="_contactos__5" onclick="ajustar_izquierdo_40('contactos_')" class="color_posicion posicion_5" onmouseover="hand('_contactos__5')"></div>                             
                              <div id="_contactos__6" onclick="ajustar_derecho_60('contactos_')" class="color_posicion posicion_6" onmouseover="hand('_contactos__6')"></div>                             
                              <div id="_contactos__7" onclick="ajustar_arriba_izquierdo('contactos_')" class="color_posicion posicion_7" onmouseover="hand('_contactos__7')"></div>
                              <div id="_contactos__8" onclick="ajustar_arriba_derecho('contactos_')" class="color_posicion posicion_8" onmouseover="hand('_contactos__8')"></div>
                              <div id="_contactos__9" onclick="ajustar_abajo_izquierdo('contactos_')" class="color_posicion posicion_9" onmouseover="hand('_contactos__9')"></div>
                              <div id="_contactos__10" onclick="ajustar_abajo_derecho('contactos_')" class="color_posicion posicion_10" onmouseover="hand('_contactos__10')"></div>
                              <div id="_contactos__11" onclick="maximizar_ventana('contactos_')" class="color_posicion posicion_11" onmouseover="hand('_contactos__11')"></div>
                              <div id="_contactos__12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_contactos__12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_contactos__13" onclick="ajustar_arriba('contactos_')" class="color_posicion posicion_13" onmouseover="hand('_contactos__13')"></div>
                              <div id="_contactos__14" onclick="ajustar_abajo('contactos_')" class="color_posicion posicion_14" onmouseover="hand('_contactos__14')"></div>
                              <div id="_contactos__15" onclick="ajustar_arriba_40('contactos_')" class="color_posicion posicion_15" onmouseover="hand('_contactos__15')"></div>
                              <div id="_contactos__16" onclick="ajustar_abajo_60('contactos_')" class="color_posicion posicion_16" onmouseover="hand('_contactos__16')"></div>
                              <div id="_contactos__17" onclick="ajustar_arriba_60('contactos_')" class="color_posicion posicion_17" onmouseover="hand('_contactos__17')"></div>
                              <div id="_contactos__18" onclick="ajustar_abajo_40('contactos_')" class="color_posicion posicion_18" onmouseover="hand('_contactos__18')"></div>
      
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('contactos_')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_contactos_" class="dragresizable_hija">
                 <iframe id="icontactos_" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
         </div>

        <div id="consultas_" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-search icono_menu" style="color: #6f42c1;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Consultas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('consultas_')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_consultas_" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('consultas_')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_consultas__posicion_orden" class="posicion_orden">
     
                              <div id="_consultas__1" onclick="ajustar_izquierdo('consultas_')" class="color_posicion posicion_1" onmouseover="hand('_consultas__1')"></div>
                              <div id="_consultas__2" onclick="ajustar_derecho('consultas_')" class="color_posicion posicion_2" onmouseover="hand('_consultas__2')"></div>
                              <div id="_consultas__3" onclick="ajustar_izquierdo_60('consultas_')" class="color_posicion posicion_3" onmouseover="hand('_consultas__3')"></div>
                              <div id="_consultas__4" onclick="ajustar_derecho_40('consultas_')" class="color_posicion posicion_4" onmouseover="hand('_consultas__4')"></div>                             
                              <div id="_consultas__5" onclick="ajustar_izquierdo_40('consultas_')" class="color_posicion posicion_5" onmouseover="hand('_consultas__5')"></div>                             
                              <div id="_consultas__6" onclick="ajustar_derecho_60('consultas_')" class="color_posicion posicion_6" onmouseover="hand('_consultas__6')"></div>                             
                              <div id="_consultas__7" onclick="ajustar_arriba_izquierdo('consultas_')" class="color_posicion posicion_7" onmouseover="hand('_consultas__7')"></div>
                              <div id="_consultas__8" onclick="ajustar_arriba_derecho('consultas_')" class="color_posicion posicion_8" onmouseover="hand('_consultas__8')"></div>
                              <div id="_consultas__9" onclick="ajustar_abajo_izquierdo('consultas_')" class="color_posicion posicion_9" onmouseover="hand('_consultas__9')"></div>
                              <div id="_consultas__10" onclick="ajustar_abajo_derecho('consultas_')" class="color_posicion posicion_10" onmouseover="hand('_consultas__10')"></div>
                              <div id="_consultas__11" onclick="maximizar_ventana('consultas_')" class="color_posicion posicion_11" onmouseover="hand('_consultas__11')"></div>
                              <div id="_consultas__12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_consultas__12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_consultas__13" onclick="ajustar_arriba('consultas_')" class="color_posicion posicion_13" onmouseover="hand('_consultas__13')"></div>
                              <div id="_consultas__14" onclick="ajustar_abajo('consultas_')" class="color_posicion posicion_14" onmouseover="hand('_consultas__14')"></div>
                              <div id="_consultas__15" onclick="ajustar_arriba_40('consultas_')" class="color_posicion posicion_15" onmouseover="hand('_consultas__15')"></div>
                              <div id="_consultas__16" onclick="ajustar_abajo_60('consultas_')" class="color_posicion posicion_16" onmouseover="hand('_consultas__16')"></div>
                              <div id="_consultas__17" onclick="ajustar_arriba_60('consultas_')" class="color_posicion posicion_17" onmouseover="hand('_consultas__17')"></div>
                              <div id="_consultas__18" onclick="ajustar_abajo_40('consultas_')" class="color_posicion posicion_18" onmouseover="hand('_consultas__18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('consultas_')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_consultas_" class="dragresizable_hija">
                 <iframe id="iconsultas_" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
         </div>

        <!-- Aquí coloco las opciones de Artículos -->
        <div id="altas__" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-columns-gap icono_menu" style="color: #28a745;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Altas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('altas__')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
             
                    <div id="_altas__" class="selector_movimiento">
                        <a href="#" title="Maximizar" onclick="maximizar_ventana('altas__')"><i class="bi bi-border-all" style="height:16px;"></i></a>
            
                        <div id="_altas___posicion_orden" class="posicion_orden">
                      
                            <div id="_altas___1" onclick="ajustar_izquierdo('altas__')" class="color_posicion posicion_1" onmouseover="hand('_altas___1')"></div>
                            <div id="_altas___2" onclick="ajustar_derecho('altas__')" class="color_posicion posicion_2" onmouseover="hand('_altas___2')"></div>
                            <div id="_altas___3" onclick="ajustar_izquierdo_60('altas__')" class="color_posicion posicion_3" onmouseover="hand('_altas___3')"></div>
                            <div id="_altas___4" onclick="ajustar_derecho_40('altas__')" class="color_posicion posicion_4" onmouseover="hand('_altas___4')"></div>                             
                            <div id="_altas___5" onclick="ajustar_izquierdo_40('altas__')" class="color_posicion posicion_5" onmouseover="hand('_altas___5')"></div>                             
                            <div id="_altas___6" onclick="ajustar_derecho_60('altas__')" class="color_posicion posicion_6" onmouseover="hand('_altas___6')"></div>                             
                            <div id="_altas___7" onclick="ajustar_arriba_izquierdo('altas__')" class="color_posicion posicion_7" onmouseover="hand('_altas___7')"></div>
                            <div id="_altas___8" onclick="ajustar_arriba_derecho('altas__')" class="color_posicion posicion_8" onmouseover="hand('_altas___8')"></div>
                            <div id="_altas___9" onclick="ajustar_abajo_izquierdo('altas__')" class="color_posicion posicion_9" onmouseover="hand('_altas___9')"></div>
                            <div id="_altas___10" onclick="ajustar_abajo_derecho('altas__')" class="color_posicion posicion_10" onmouseover="hand('_altas___10')"></div>
                            <div id="_altas___11" onclick="maximizar_ventana('altas__')" class="color_posicion posicion_11" onmouseover="hand('_altas___11')"></div>
                            <div id="_altas___12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_altas___12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                            <div id="_altas___13" onclick="ajustar_arriba('altas__')" class="color_posicion posicion_13" onmouseover="hand('_altas___13')"></div>
                            <div id="_altas___14" onclick="ajustar_abajo('altas__')" class="color_posicion posicion_14" onmouseover="hand('_altas___14')"></div>
                            <div id="_altas___15" onclick="ajustar_arriba_40('altas__')" class="color_posicion posicion_15" onmouseover="hand('_altas___15')"></div>
                            <div id="_altas___16" onclick="ajustar_abajo_60('altas__')" class="color_posicion posicion_16" onmouseover="hand('_altas___16')"></div>
                            <div id="_altas___17" onclick="ajustar_arriba_60('altas__')" class="color_posicion posicion_17" onmouseover="hand('_altas___17')"></div>
                            <div id="_altas___18" onclick="ajustar_abajo_40('altas__')" class="color_posicion posicion_18" onmouseover="hand('_altas___18')"></div>
                      
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('altas__')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_altas__" class="dragresizable_hija">
                <iframe id="ialtas__" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="consultas__" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-search icono_menu" style="color: #28a745;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Consultas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('consultas__')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_consultas__" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('consultas__')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_consultas___posicion_orden" class="posicion_orden">
     
                              <div id="_consultas___1" onclick="ajustar_izquierdo('consultas__')" class="color_posicion posicion_1" onmouseover="hand('_consultas___1')"></div>
                              <div id="_consultas___2" onclick="ajustar_derecho('consultas__')" class="color_posicion posicion_2" onmouseover="hand('_consultas___2')"></div>
                              <div id="_consultas___3" onclick="ajustar_izquierdo_60('consultas__')" class="color_posicion posicion_3" onmouseover="hand('_consultas___3')"></div>
                              <div id="_consultas___4" onclick="ajustar_derecho_40('consultas__')" class="color_posicion posicion_4" onmouseover="hand('_consultas___4')"></div>                             
                              <div id="_consultas___5" onclick="ajustar_izquierdo_40('consultas__')" class="color_posicion posicion_5" onmouseover="hand('_consultas___5')"></div>                             
                              <div id="_consultas___6" onclick="ajustar_derecho_60('consultas__')" class="color_posicion posicion_6" onmouseover="hand('_consultas___6')"></div>                             
                              <div id="_consultas___7" onclick="ajustar_arriba_izquierdo('consultas__')" class="color_posicion posicion_7" onmouseover="hand('_consultas___7')"></div>
                              <div id="_consultas___8" onclick="ajustar_arriba_derecho('consultas__')" class="color_posicion posicion_8" onmouseover="hand('_consultas___8')"></div>
                              <div id="_consultas___9" onclick="ajustar_abajo_izquierdo('consultas__')" class="color_posicion posicion_9" onmouseover="hand('_consultas___9')"></div>
                              <div id="_consultas___10" onclick="ajustar_abajo_derecho('consultas__')" class="color_posicion posicion_10" onmouseover="hand('_consultas___10')"></div>
                              <div id="_consultas___11" onclick="maximizar_ventana('consultas__')" class="color_posicion posicion_11" onmouseover="hand('_consultas___11')"></div>
                              <div id="_consultas___12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_consultas___12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_consultas___13" onclick="ajustar_arriba('consultas__')" class="color_posicion posicion_13" onmouseover="hand('_consultas___13')"></div>
                              <div id="_consultas___14" onclick="ajustar_abajo('consultas__')" class="color_posicion posicion_14" onmouseover="hand('_consultas___14')"></div>
                              <div id="_consultas___15" onclick="ajustar_arriba_40('consultas__')" class="color_posicion posicion_15" onmouseover="hand('_consultas___15')"></div>
                              <div id="_consultas___16" onclick="ajustar_abajo_60('consultas__')" class="color_posicion posicion_16" onmouseover="hand('_consultas___16')"></div>
                              <div id="_consultas___17" onclick="ajustar_arriba_60('consultas__')" class="color_posicion posicion_17" onmouseover="hand('_consultas___17')"></div>
                              <div id="_consultas___18" onclick="ajustar_abajo_40('consultas__')" class="color_posicion posicion_18" onmouseover="hand('_consultas___18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('consultas__')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_consultas__" class="dragresizable_hija">
                 <iframe id="iconsultas__" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>

        <!-- Aquí coloco las opciones de Facturas -->
        <div id="altas___" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-columns-gap icono_menu" style="color: #fd7e14;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Altas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('altas___')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
             
                    <div id="_altas___" class="selector_movimiento">
                        <a href="#" title="Maximizar" onclick="maximizar_ventana('altas___')"><i class="bi bi-border-all" style="height:16px;"></i></a>
            
                        <div id="_altas____posicion_orden" class="posicion_orden">
                      
                            <div id="_altas____1" onclick="ajustar_izquierdo('altas___')" class="color_posicion posicion_1" onmouseover="hand('_altas____1')"></div>
                            <div id="_altas____2" onclick="ajustar_derecho('altas___')" class="color_posicion posicion_2" onmouseover="hand('_altas____2')"></div>
                            <div id="_altas____3" onclick="ajustar_izquierdo_60('altas___')" class="color_posicion posicion_3" onmouseover="hand('_altas____3')"></div>
                            <div id="_altas____4" onclick="ajustar_derecho_40('altas___')" class="color_posicion posicion_4" onmouseover="hand('_altas____4')"></div>                             
                            <div id="_altas____5" onclick="ajustar_izquierdo_40('altas___')" class="color_posicion posicion_5" onmouseover="hand('_altas____5')"></div>                             
                            <div id="_altas____6" onclick="ajustar_derecho_60('altas___')" class="color_posicion posicion_6" onmouseover="hand('_altas____6')"></div>                             
                            <div id="_altas____7" onclick="ajustar_arriba_izquierdo('altas___')" class="color_posicion posicion_7" onmouseover="hand('_altas____7')"></div>
                            <div id="_altas____8" onclick="ajustar_arriba_derecho('altas___')" class="color_posicion posicion_8" onmouseover="hand('_altas____8')"></div>
                            <div id="_altas____9" onclick="ajustar_abajo_izquierdo('altas___')" class="color_posicion posicion_9" onmouseover="hand('_altas____9')"></div>
                            <div id="_altas____10" onclick="ajustar_abajo_derecho('altas___')" class="color_posicion posicion_10" onmouseover="hand('_altas____10')"></div>
                            <div id="_altas____11" onclick="maximizar_ventana('altas___')" class="color_posicion posicion_11" onmouseover="hand('_altas____11')"></div>
                            <div id="_altas____12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_altas____12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                            <div id="_altas____13" onclick="ajustar_arriba('altas___')" class="color_posicion posicion_13" onmouseover="hand('_altas____13')"></div>
                            <div id="_altas____14" onclick="ajustar_abajo('altas___')" class="color_posicion posicion_14" onmouseover="hand('_altas____14')"></div>
                            <div id="_altas____15" onclick="ajustar_arriba_40('altas___')" class="color_posicion posicion_15" onmouseover="hand('_altas____15')"></div>
                            <div id="_altas____16" onclick="ajustar_abajo_60('altas___')" class="color_posicion posicion_16" onmouseover="hand('_altas____16')"></div>
                            <div id="_altas____17" onclick="ajustar_arriba_60('altas___')" class="color_posicion posicion_17" onmouseover="hand('_altas____17')"></div>
                            <div id="_altas____18" onclick="ajustar_abajo_40('altas___')" class="color_posicion posicion_18" onmouseover="hand('_altas____18')"></div>
                      
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('altas___')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_altas___" class="dragresizable_hija">
                <iframe id="ialtas___" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="repetitivas" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="bi bi-collection icono_menu" style="color: #fd7e14;"></span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Repetitivas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('repetitivas')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                <td width="30" align="center" valign="bottom">
             
                    <div id="_repetitivas" class="selector_movimiento">
                        <a href="#" title="Maximizar" onclick="maximizar_ventana('repetitivas')"><i class="bi bi-border-all" style="height:16px;"></i></a>
            
                        <div id="_repetitivas_posicion_orden" class="posicion_orden">
                      
                            <div id="_repetitivas_1" onclick="ajustar_izquierdo('repetitivas')" class="color_posicion posicion_1" onmouseover="hand('_repetitivas_1')"></div>
                            <div id="_repetitivas_2" onclick="ajustar_derecho('repetitivas')" class="color_posicion posicion_2" onmouseover="hand('_repetitivas_2')"></div>
                            <div id="_repetitivas_3" onclick="ajustar_izquierdo_60('repetitivas')" class="color_posicion posicion_3" onmouseover="hand('_repetitivas_3')"></div>
                            <div id="_repetitivas_4" onclick="ajustar_derecho_40('repetitivas')" class="color_posicion posicion_4" onmouseover="hand('_repetitivas_4')"></div>                             
                            <div id="_repetitivas_5" onclick="ajustar_izquierdo_40('repetitivas')" class="color_posicion posicion_5" onmouseover="hand('_repetitivas_5')"></div>                             
                            <div id="_repetitivas_6" onclick="ajustar_derecho_60('repetitivas')" class="color_posicion posicion_6" onmouseover="hand('_repetitivas_6')"></div>                             
                            <div id="_repetitivas_7" onclick="ajustar_arriba_izquierdo('repetitivas')" class="color_posicion posicion_7" onmouseover="hand('_repetitivas_7')"></div>
                            <div id="_repetitivas_8" onclick="ajustar_arriba_derecho('repetitivas')" class="color_posicion posicion_8" onmouseover="hand('_repetitivas_8')"></div>
                            <div id="_repetitivas_9" onclick="ajustar_abajo_izquierdo('repetitivas')" class="color_posicion posicion_9" onmouseover="hand('_repetitivas_9')"></div>
                            <div id="_repetitivas_10" onclick="ajustar_abajo_derecho('repetitivas')" class="color_posicion posicion_10" onmouseover="hand('_repetitivas_10')"></div>
                            <div id="_repetitivas_11" onclick="maximizar_ventana('repetitivas')" class="color_posicion posicion_11" onmouseover="hand('_repetitivas_11')"></div>
                            <div id="_repetitivas_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_repetitivas_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                            <div id="_repetitivas_13" onclick="ajustar_arriba('repetitivas')" class="color_posicion posicion_13" onmouseover="hand('_repetitivas_13')"></div>
                            <div id="_repetitivas_14" onclick="ajustar_abajo('repetitivas')" class="color_posicion posicion_14" onmouseover="hand('_repetitivas_14')"></div>
                            <div id="_repetitivas_15" onclick="ajustar_arriba_40('repetitivas')" class="color_posicion posicion_15" onmouseover="hand('_repetitivas_15')"></div>
                            <div id="_repetitivas_16" onclick="ajustar_abajo_60('repetitivas')" class="color_posicion posicion_16" onmouseover="hand('_repetitivas_16')"></div>
                            <div id="_repetitivas_17" onclick="ajustar_arriba_60('repetitivas')" class="color_posicion posicion_17" onmouseover="hand('_repetitivas_17')"></div>
                            <div id="_repetitivas_18" onclick="ajustar_abajo_40('repetitivas')" class="color_posicion posicion_18" onmouseover="hand('_repetitivas_18')"></div>
                      
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('repetitivas')"><i class="bi bi-x-lg text-danger"></i></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_repetitivas" class="dragresizable_hija">
                <iframe id="irepetitivas" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="consultas___" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-search icono_menu" style="color: #fd7e14;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Consultas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('consultas___')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_consultas___" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('consultas___')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_consultas____posicion_orden" class="posicion_orden">
     
                              <div id="_consultas____1" onclick="ajustar_izquierdo('consultas___')" class="color_posicion posicion_1" onmouseover="hand('_consultas____1')"></div>
                              <div id="_consultas____2" onclick="ajustar_derecho('consultas___')" class="color_posicion posicion_2" onmouseover="hand('_consultas____2')"></div>
                              <div id="_consultas____3" onclick="ajustar_izquierdo_60('consultas___')" class="color_posicion posicion_3" onmouseover="hand('_consultas____3')"></div>
                              <div id="_consultas____4" onclick="ajustar_derecho_40('consultas___')" class="color_posicion posicion_4" onmouseover="hand('_consultas____4')"></div>                             
                              <div id="_consultas____5" onclick="ajustar_izquierdo_40('consultas___')" class="color_posicion posicion_5" onmouseover="hand('_consultas____5')"></div>                             
                              <div id="_consultas____6" onclick="ajustar_derecho_60('consultas___')" class="color_posicion posicion_6" onmouseover="hand('_consultas____6')"></div>                             
                              <div id="_consultas____7" onclick="ajustar_arriba_izquierdo('consultas___')" class="color_posicion posicion_7" onmouseover="hand('_consultas____7')"></div>
                              <div id="_consultas____8" onclick="ajustar_arriba_derecho('consultas___')" class="color_posicion posicion_8" onmouseover="hand('_consultas____8')"></div>
                              <div id="_consultas____9" onclick="ajustar_abajo_izquierdo('consultas___')" class="color_posicion posicion_9" onmouseover="hand('_consultas____9')"></div>
                              <div id="_consultas____10" onclick="ajustar_abajo_derecho('consultas___')" class="color_posicion posicion_10" onmouseover="hand('_consultas____10')"></div>
                              <div id="_consultas____11" onclick="maximizar_ventana('consultas___')" class="color_posicion posicion_11" onmouseover="hand('_consultas____11')"></div>
                              <div id="_consultas____12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_consultas____12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_consultas____13" onclick="ajustar_arriba('consultas___')" class="color_posicion posicion_13" onmouseover="hand('_consultas____13')"></div>
                              <div id="_consultas____14" onclick="ajustar_abajo('consultas___')" class="color_posicion posicion_14" onmouseover="hand('_consultas____14')"></div>
                              <div id="_consultas____15" onclick="ajustar_arriba_40('consultas___')" class="color_posicion posicion_15" onmouseover="hand('_consultas____15')"></div>
                              <div id="_consultas____16" onclick="ajustar_abajo_60('consultas___')" class="color_posicion posicion_16" onmouseover="hand('_consultas____16')"></div>
                              <div id="_consultas____17" onclick="ajustar_arriba_60('consultas___')" class="color_posicion posicion_17" onmouseover="hand('_consultas____17')"></div>
                              <div id="_consultas____18" onclick="ajustar_abajo_40('consultas___')" class="color_posicion posicion_18" onmouseover="hand('_consultas____18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('consultas___')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_consultas___" class="dragresizable_hija">
                 <iframe id="iconsultas___" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>

        <!-- Aquí coloco las opciones de Albaranes -->
        <div id="altas_____" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-card-checklist icono_menu" style="color: #ffc107;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Altas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('altas_____')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_altas_____" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('altas_____')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_altas______posicion_orden" class="posicion_orden">
     
                              <div id="_altas______1" onclick="ajustar_izquierdo('altas_____')" class="color_posicion posicion_1" onmouseover="hand('_altas______1')"></div>
                              <div id="_altas______2" onclick="ajustar_derecho('altas_____')" class="color_posicion posicion_2" onmouseover="hand('_altas______2')"></div>
                              <div id="_altas______3" onclick="ajustar_izquierdo_60('altas_____')" class="color_posicion posicion_3" onmouseover="hand('_altas______3')"></div>
                              <div id="_altas______4" onclick="ajustar_derecho_40('altas_____')" class="color_posicion posicion_4" onmouseover="hand('_altas______4')"></div>                             
                              <div id="_altas______5" onclick="ajustar_izquierdo_40('altas_____')" class="color_posicion posicion_5" onmouseover="hand('_altas______5')"></div>                             
                              <div id="_altas______6" onclick="ajustar_derecho_60('altas_____')" class="color_posicion posicion_6" onmouseover="hand('_altas______6')"></div>                             
                              <div id="_altas______7" onclick="ajustar_arriba_izquierdo('altas_____')" class="color_posicion posicion_7" onmouseover="hand('_altas______7')"></div>
                              <div id="_altas______8" onclick="ajustar_arriba_derecho('altas_____')" class="color_posicion posicion_8" onmouseover="hand('_altas______8')"></div>
                              <div id="_altas______9" onclick="ajustar_abajo_izquierdo('altas_____')" class="color_posicion posicion_9" onmouseover="hand('_altas______9')"></div>
                              <div id="_altas______10" onclick="ajustar_abajo_derecho('altas_____')" class="color_posicion posicion_10" onmouseover="hand('_altas______10')"></div>
                              <div id="_altas______11" onclick="maximizar_ventana('altas_____')" class="color_posicion posicion_11" onmouseover="hand('_altas______11')"></div>
                              <div id="_altas______12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_altas______12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_altas______13" onclick="ajustar_arriba('altas_____')" class="color_posicion posicion_13" onmouseover="hand('_altas______13')"></div>
                              <div id="_altas______14" onclick="ajustar_abajo('altas_____')" class="color_posicion posicion_14" onmouseover="hand('_altas______14')"></div>
                              <div id="_altas______15" onclick="ajustar_arriba_40('altas_____')" class="color_posicion posicion_15" onmouseover="hand('_altas______15')"></div>
                              <div id="_altas______16" onclick="ajustar_abajo_60('altas_____')" class="color_posicion posicion_16" onmouseover="hand('_altas______16')"></div>
                              <div id="_altas______17" onclick="ajustar_arriba_60('altas_____')" class="color_posicion posicion_17" onmouseover="hand('_altas______17')"></div>
                              <div id="_altas______18" onclick="ajustar_abajo_40('altas_____')" class="color_posicion posicion_18" onmouseover="hand('_altas______18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('altas_____')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_altas_____" class="dragresizable_hija">
                 <iframe id="ialtas_____" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>
            
        <div id="consultas_____" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-search icono_menu" style="color: #ffc107;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Consultas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('consultas_____')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_consultas_____" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('consultas_____')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_consultas______posicion_orden" class="posicion_orden">
     
                              <div id="_consultas______1" onclick="ajustar_izquierdo('consultas_____')" class="color_posicion posicion_1" onmouseover="hand('_consultas______1')"></div>
                              <div id="_consultas______2" onclick="ajustar_derecho('consultas_____')" class="color_posicion posicion_2" onmouseover="hand('_consultas______2')"></div>
                              <div id="_consultas______3" onclick="ajustar_izquierdo_60('consultas_____')" class="color_posicion posicion_3" onmouseover="hand('_consultas______3')"></div>
                              <div id="_consultas______4" onclick="ajustar_derecho_40('consultas_____')" class="color_posicion posicion_4" onmouseover="hand('_consultas______4')"></div>                             
                              <div id="_consultas______5" onclick="ajustar_izquierdo_40('consultas_____')" class="color_posicion posicion_5" onmouseover="hand('_consultas______5')"></div>                             
                              <div id="_consultas______6" onclick="ajustar_derecho_60('consultas_____')" class="color_posicion posicion_6" onmouseover="hand('_consultas______6')"></div>                             
                              <div id="_consultas______7" onclick="ajustar_arriba_izquierdo('consultas_____')" class="color_posicion posicion_7" onmouseover="hand('_consultas______7')"></div>
                              <div id="_consultas______8" onclick="ajustar_arriba_derecho('consultas_____')" class="color_posicion posicion_8" onmouseover="hand('_consultas______8')"></div>
                              <div id="_consultas______9" onclick="ajustar_abajo_izquierdo('consultas_____')" class="color_posicion posicion_9" onmouseover="hand('_consultas______9')"></div>
                              <div id="_consultas______10" onclick="ajustar_abajo_derecho('consultas_____')" class="color_posicion posicion_10" onmouseover="hand('_consultas______10')"></div>
                              <div id="_consultas______11" onclick="maximizar_ventana('consultas_____')" class="color_posicion posicion_11" onmouseover="hand('_consultas______11')"></div>
                              <div id="_consultas______12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_consultas______12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_consultas______13" onclick="ajustar_arriba('consultas_____')" class="color_posicion posicion_13" onmouseover="hand('_consultas______13')"></div>
                              <div id="_consultas______14" onclick="ajustar_abajo('consultas_____')" class="color_posicion posicion_14" onmouseover="hand('_consultas______14')"></div>
                              <div id="_consultas______15" onclick="ajustar_arriba_40('consultas_____')" class="color_posicion posicion_15" onmouseover="hand('_consultas______15')"></div>
                              <div id="_consultas______16" onclick="ajustar_abajo_60('consultas_____')" class="color_posicion posicion_16" onmouseover="hand('_consultas______16')"></div>
                              <div id="_consultas______17" onclick="ajustar_arriba_60('consultas_____')" class="color_posicion posicion_17" onmouseover="hand('_consultas______17')"></div>
                              <div id="_consultas______18" onclick="ajustar_abajo_40('consultas_____')" class="color_posicion posicion_18" onmouseover="hand('_consultas______18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('consultas_____')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_consultas_____" class="dragresizable_hija">
                 <iframe id="iconsultas_____" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>

        <!-- Aquí coloco las opciones de Presupuestos -->
        <div id="altas____" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-columns-gap icono_menu" style="color: #dc3545;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Altas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('altas____')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_altas____" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('altas____')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_altas_____posicion_orden" class="posicion_orden">
     
                              <div id="_altas_____1" onclick="ajustar_izquierdo('altas____')" class="color_posicion posicion_1" onmouseover="hand('_altas_____1')"></div>
                              <div id="_altas_____2" onclick="ajustar_derecho('altas____')" class="color_posicion posicion_2" onmouseover="hand('_altas_____2')"></div>
                              <div id="_altas_____3" onclick="ajustar_izquierdo_60('altas____')" class="color_posicion posicion_3" onmouseover="hand('_altas_____3')"></div>
                              <div id="_altas_____4" onclick="ajustar_derecho_40('altas____')" class="color_posicion posicion_4" onmouseover="hand('_altas_____4')"></div>                             
                              <div id="_altas_____5" onclick="ajustar_izquierdo_40('altas____')" class="color_posicion posicion_5" onmouseover="hand('_altas_____5')"></div>                             
                              <div id="_altas_____6" onclick="ajustar_derecho_60('altas____')" class="color_posicion posicion_6" onmouseover="hand('_altas_____6')"></div>                             
                              <div id="_altas_____7" onclick="ajustar_arriba_izquierdo('altas____')" class="color_posicion posicion_7" onmouseover="hand('_altas_____7')"></div>
                              <div id="_altas_____8" onclick="ajustar_arriba_derecho('altas____')" class="color_posicion posicion_8" onmouseover="hand('_altas_____8')"></div>
                              <div id="_altas_____9" onclick="ajustar_abajo_izquierdo('altas____')" class="color_posicion posicion_9" onmouseover="hand('_altas_____9')"></div>
                              <div id="_altas_____10" onclick="ajustar_abajo_derecho('altas____')" class="color_posicion posicion_10" onmouseover="hand('_altas_____10')"></div>
                              <div id="_altas_____11" onclick="maximizar_ventana('altas____')" class="color_posicion posicion_11" onmouseover="hand('_altas_____11')"></div>
                              <div id="_altas_____12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_altas_____12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_altas_____13" onclick="ajustar_arriba('altas____')" class="color_posicion posicion_13" onmouseover="hand('_altas_____13')"></div>
                              <div id="_altas_____14" onclick="ajustar_abajo('altas____')" class="color_posicion posicion_14" onmouseover="hand('_altas_____14')"></div>
                              <div id="_altas_____15" onclick="ajustar_arriba_40('altas____')" class="color_posicion posicion_15" onmouseover="hand('_altas_____15')"></div>
                              <div id="_altas_____16" onclick="ajustar_abajo_60('altas____')" class="color_posicion posicion_16" onmouseover="hand('_altas_____16')"></div>
                              <div id="_altas_____17" onclick="ajustar_arriba_60('altas____')" class="color_posicion posicion_17" onmouseover="hand('_altas_____17')"></div>
                              <div id="_altas_____18" onclick="ajustar_abajo_40('altas____')" class="color_posicion posicion_18" onmouseover="hand('_altas_____18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('altas____')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_altas____" class="dragresizable_hija">
                 <iframe id="ialtas____" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>

        <div id="consultas____" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-search icono_menu" style="color: #dc3545;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Consultas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('consultas____')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_consultas____" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('consultas____')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_consultas_____posicion_orden" class="posicion_orden">
     
                              <div id="_consultas_____1" onclick="ajustar_izquierdo('consultas____')" class="color_posicion posicion_1" onmouseover="hand('_consultas_____1')"></div>
                              <div id="_consultas_____2" onclick="ajustar_derecho('consultas____')" class="color_posicion posicion_2" onmouseover="hand('_consultas_____2')"></div>
                              <div id="_consultas_____3" onclick="ajustar_izquierdo_60('consultas____')" class="color_posicion posicion_3" onmouseover="hand('_consultas_____3')"></div>
                              <div id="_consultas_____4" onclick="ajustar_derecho_40('consultas____')" class="color_posicion posicion_4" onmouseover="hand('_consultas_____4')"></div>                             
                              <div id="_consultas_____5" onclick="ajustar_izquierdo_40('consultas____')" class="color_posicion posicion_5" onmouseover="hand('_consultas_____5')"></div>                             
                              <div id="_consultas_____6" onclick="ajustar_derecho_60('consultas____')" class="color_posicion posicion_6" onmouseover="hand('_consultas_____6')"></div>                             
                              <div id="_consultas_____7" onclick="ajustar_arriba_izquierdo('consultas____')" class="color_posicion posicion_7" onmouseover="hand('_consultas_____7')"></div>
                              <div id="_consultas_____8" onclick="ajustar_arriba_derecho('consultas____')" class="color_posicion posicion_8" onmouseover="hand('_consultas_____8')"></div>
                              <div id="_consultas_____9" onclick="ajustar_abajo_izquierdo('consultas____')" class="color_posicion posicion_9" onmouseover="hand('_consultas_____9')"></div>
                              <div id="_consultas_____10" onclick="ajustar_abajo_derecho('consultas____')" class="color_posicion posicion_10" onmouseover="hand('_consultas_____10')"></div>
                              <div id="_consultas_____11" onclick="maximizar_ventana('consultas____')" class="color_posicion posicion_11" onmouseover="hand('_consultas_____11')"></div>
                              <div id="_consultas_____12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_consultas_____12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_consultas_____13" onclick="ajustar_arriba('consultas____')" class="color_posicion posicion_13" onmouseover="hand('_consultas_____13')"></div>
                              <div id="_consultas_____14" onclick="ajustar_abajo('consultas____')" class="color_posicion posicion_14" onmouseover="hand('_consultas_____14')"></div>
                              <div id="_consultas_____15" onclick="ajustar_arriba_40('consultas____')" class="color_posicion posicion_15" onmouseover="hand('_consultas_____15')"></div>
                              <div id="_consultas_____16" onclick="ajustar_abajo_60('consultas____')" class="color_posicion posicion_16" onmouseover="hand('_consultas_____16')"></div>
                              <div id="_consultas_____17" onclick="ajustar_arriba_60('consultas____')" class="color_posicion posicion_17" onmouseover="hand('_consultas_____17')"></div>
                              <div id="_consultas_____18" onclick="ajustar_abajo_40('consultas____')" class="color_posicion posicion_18" onmouseover="hand('_consultas_____18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('consultas____')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_consultas____" class="dragresizable_hija">
                 <iframe id="iconsultas____" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>

        <!-- Aquí coloco las opciones de Informes -->
        <div id="Facturas_emitidas" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-file-earmark-text icono_menu" style="color: #292929;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Facturas Emitidas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('Facturas_emitidas')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_Facturas_emitidas" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('Facturas_emitidas')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_Facturas_emitidas_posicion_orden" class="posicion_orden">
     
                              <div id="_Facturas_emitidas_1" onclick="ajustar_izquierdo('Facturas_emitidas')" class="color_posicion posicion_1" onmouseover="hand('_Facturas_emitidas_1')"></div>
                              <div id="_Facturas_emitidas_2" onclick="ajustar_derecho('Facturas_emitidas')" class="color_posicion posicion_2" onmouseover="hand('_Facturas_emitidas_2')"></div>
                              <div id="_Facturas_emitidas_3" onclick="ajustar_izquierdo_60('Facturas_emitidas')" class="color_posicion posicion_3" onmouseover="hand('_Facturas_emitidas_3')"></div>
                              <div id="_Facturas_emitidas_4" onclick="ajustar_derecho_40('Facturas_emitidas')" class="color_posicion posicion_4" onmouseover="hand('_Facturas_emitidas_4')"></div>                             
                              <div id="_Facturas_emitidas_5" onclick="ajustar_izquierdo_40('Facturas_emitidas')" class="color_posicion posicion_5" onmouseover="hand('_Facturas_emitidas_5')"></div>                             
                              <div id="_Facturas_emitidas_6" onclick="ajustar_derecho_60('Facturas_emitidas')" class="color_posicion posicion_6" onmouseover="hand('_Facturas_emitidas_6')"></div>                             
                              <div id="_Facturas_emitidas_7" onclick="ajustar_arriba_izquierdo('Facturas_emitidas')" class="color_posicion posicion_7" onmouseover="hand('_Facturas_emitidas_7')"></div>
                              <div id="_Facturas_emitidas_8" onclick="ajustar_arriba_derecho('Facturas_emitidas')" class="color_posicion posicion_8" onmouseover="hand('_Facturas_emitidas_8')"></div>
                              <div id="_Facturas_emitidas_9" onclick="ajustar_abajo_izquierdo('Facturas_emitidas')" class="color_posicion posicion_9" onmouseover="hand('_Facturas_emitidas_9')"></div>
                              <div id="_Facturas_emitidas_10" onclick="ajustar_abajo_derecho('Facturas_emitidas')" class="color_posicion posicion_10" onmouseover="hand('_Facturas_emitidas_10')"></div>
                              <div id="_Facturas_emitidas_11" onclick="maximizar_ventana('Facturas_emitidas')" class="color_posicion posicion_11" onmouseover="hand('_Facturas_emitidas_11')"></div>
                              <div id="_Facturas_emitidas_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_Facturas_emitidas_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_Facturas_emitidas_13" onclick="ajustar_arriba('Facturas_emitidas')" class="color_posicion posicion_13" onmouseover="hand('_Facturas_emitidas_13')"></div>
                              <div id="_Facturas_emitidas_14" onclick="ajustar_abajo('Facturas_emitidas')" class="color_posicion posicion_14" onmouseover="hand('_Facturas_emitidas_14')"></div>
                              <div id="_Facturas_emitidas_15" onclick="ajustar_arriba_40('Facturas_emitidas')" class="color_posicion posicion_15" onmouseover="hand('_Facturas_emitidas_15')"></div>
                              <div id="_Facturas_emitidas_16" onclick="ajustar_abajo_60('Facturas_emitidas')" class="color_posicion posicion_16" onmouseover="hand('_Facturas_emitidas_16')"></div>
                              <div id="_Facturas_emitidas_17" onclick="ajustar_arriba_60('Facturas_emitidas')" class="color_posicion posicion_17" onmouseover="hand('_Facturas_emitidas_17')"></div>
                              <div id="_Facturas_emitidas_18" onclick="ajustar_abajo_40('Facturas_emitidas')" class="color_posicion posicion_18" onmouseover="hand('_Facturas_emitidas_18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('Facturas_emitidas')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_Facturas_emitidas" class="dragresizable_hija">
                 <iframe id="iFacturas_emitidas" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>
      
        <div id="Agenda" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-file-earmark-text icono_menu" style="color: #292929;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Agenda</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('Agenda')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_Agenda" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('Agenda')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_Agenda_posicion_orden" class="posicion_orden">
     
                              <div id="_Agenda_1" onclick="ajustar_izquierdo('Agenda')" class="color_posicion posicion_1" onmouseover="hand('_Agenda_1')"></div>
                              <div id="_Agenda_2" onclick="ajustar_derecho('Agenda')" class="color_posicion posicion_2" onmouseover="hand('_Agenda_2')"></div>
                              <div id="_Agenda_3" onclick="ajustar_izquierdo_60('Agenda')" class="color_posicion posicion_3" onmouseover="hand('_Agenda_3')"></div>
                              <div id="_Agenda_4" onclick="ajustar_derecho_40('Agenda')" class="color_posicion posicion_4" onmouseover="hand('_Agenda_4')"></div>                             
                              <div id="_Agenda_5" onclick="ajustar_izquierdo_40('Agenda')" class="color_posicion posicion_5" onmouseover="hand('_Agenda_5')"></div>                             
                              <div id="_Agenda_6" onclick="ajustar_derecho_60('Agenda')" class="color_posicion posicion_6" onmouseover="hand('_Agenda_6')"></div>                             
                              <div id="_Agenda_7" onclick="ajustar_arriba_izquierdo('Agenda')" class="color_posicion posicion_7" onmouseover="hand('_Agenda_7')"></div>
                              <div id="_Agenda_8" onclick="ajustar_arriba_derecho('Agenda')" class="color_posicion posicion_8" onmouseover="hand('_Agenda_8')"></div>
                              <div id="_Agenda_9" onclick="ajustar_abajo_izquierdo('Agenda')" class="color_posicion posicion_9" onmouseover="hand('_Agenda_9')"></div>
                              <div id="_Agenda_10" onclick="ajustar_abajo_derecho('Agenda')" class="color_posicion posicion_10" onmouseover="hand('_Agenda_10')"></div>
                              <div id="_Agenda_11" onclick="maximizar_ventana('Agenda')" class="color_posicion posicion_11" onmouseover="hand('_Agenda_11')"></div>
                              <div id="_Agenda_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_Agenda_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_Agenda_13" onclick="ajustar_arriba('Agenda')" class="color_posicion posicion_13" onmouseover="hand('_Agenda_13')"></div>
                              <div id="_Agenda_14" onclick="ajustar_abajo('Agenda')" class="color_posicion posicion_14" onmouseover="hand('_Agenda_14')"></div>
                              <div id="_Agenda_15" onclick="ajustar_arriba_40('Agenda')" class="color_posicion posicion_15" onmouseover="hand('_Agenda_15')"></div>
                              <div id="_Agenda_16" onclick="ajustar_abajo_60('Agenda')" class="color_posicion posicion_16" onmouseover="hand('_Agenda_16')"></div>
                              <div id="_Agenda_17" onclick="ajustar_arriba_60('Agenda')" class="color_posicion posicion_17" onmouseover="hand('_Agenda_17')"></div>
                              <div id="_Agenda_18" onclick="ajustar_abajo_40('Agenda')" class="color_posicion posicion_18" onmouseover="hand('_Agenda_18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('Agenda')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_Agenda" class="dragresizable_hija">
                 <iframe id="iAgenda" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>

        <!-- Aquí coloco las opciones de Configuracion -->
        <div id="configuracion_" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-gear-wide-connected icono_menu" style="color: #292929;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Configuración</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('configuracion_')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
      
                     <div id="_configuracion_" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('configuracion_')"><i class="bi bi-border-all" style="height:16px;"></i></a>
     
                         <div id="_configuracion__posicion_orden" class="posicion_orden">
               
                             <div id="_configuracion__1" onclick="ajustar_izquierdo('configuracion_')" class="color_posicion posicion_1" onmouseover="hand('_configuracion__1')"></div>
                             <div id="_configuracion__2" onclick="ajustar_derecho('configuracion_')" class="color_posicion posicion_2" onmouseover="hand('_configuracion__2')"></div>
                             <div id="_configuracion__3" onclick="ajustar_izquierdo_60('configuracion_')" class="color_posicion posicion_3" onmouseover="hand('_configuracion__3')"></div>
                             <div id="_configuracion__4" onclick="ajustar_derecho_40('configuracion_')" class="color_posicion posicion_4" onmouseover="hand('_configuracion__4')"></div>                             
                             <div id="_configuracion__5" onclick="ajustar_izquierdo_40('configuracion_')" class="color_posicion posicion_5" onmouseover="hand('_configuracion__5')"></div>                             
                             <div id="_configuracion__6" onclick="ajustar_derecho_60('configuracion_')" class="color_posicion posicion_6" onmouseover="hand('_configuracion__6')"></div>                             
                             <div id="_configuracion__7" onclick="ajustar_arriba_izquierdo('configuracion_')" class="color_posicion posicion_7" onmouseover="hand('_configuracion__7')"></div>
                             <div id="_configuracion__8" onclick="ajustar_arriba_derecho('configuracion_')" class="color_posicion posicion_8" onmouseover="hand('_configuracion__8')"></div>
                             <div id="_configuracion__9" onclick="ajustar_abajo_izquierdo('configuracion_')" class="color_posicion posicion_9" onmouseover="hand('_configuracion__9')"></div>
                             <div id="_configuracion__10" onclick="ajustar_abajo_derecho('configuracion_')" class="color_posicion posicion_10" onmouseover="hand('_configuracion__10')"></div>
                             <div id="_configuracion__11" onclick="maximizar_ventana('configuracion_')" class="color_posicion posicion_11" onmouseover="hand('_configuracion__11')"></div>
                             <div id="_configuracion__12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_configuracion__12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                             <div id="_configuracion__13" onclick="ajustar_arriba('configuracion_')" class="color_posicion posicion_13" onmouseover="hand('_configuracion__13')"></div>
                             <div id="_configuracion__14" onclick="ajustar_abajo('configuracion_')" class="color_posicion posicion_14" onmouseover="hand('_configuracion__14')"></div>
                             <div id="_configuracion__15" onclick="ajustar_arriba_40('configuracion_')" class="color_posicion posicion_15" onmouseover="hand('_configuracion__15')"></div>
                             <div id="_configuracion__16" onclick="ajustar_abajo_60('configuracion_')" class="color_posicion posicion_16" onmouseover="hand('_configuracion__16')"></div>
                             <div id="_configuracion__17" onclick="ajustar_arriba_60('configuracion_')" class="color_posicion posicion_17" onmouseover="hand('_configuracion__17')"></div>
                             <div id="_configuracion__18" onclick="ajustar_abajo_40('configuracion_')" class="color_posicion posicion_18" onmouseover="hand('_configuracion__18')"></div>
               
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('configuracion_')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_configuracion_" class="dragresizable_hija">
                 <iframe id="iconfiguracion_" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
         </div>

        <!-- Aquí coloco las opciones de Herramientas -->
        <div id="herramientas" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-tools icono_menu" style="color: #292929;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Herramientas</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('herramientas')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
      
                     <div id="_herramientas" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('herramientas')"><i class="bi bi-border-all" style="height:16px;"></i></a>
     
                         <div id="_herramientas_posicion_orden" class="posicion_orden">
               
                             <div id="_herramientas_1" onclick="ajustar_izquierdo('herramientas')" class="color_posicion posicion_1" onmouseover="hand('_herramientas_1')"></div>
                             <div id="_herramientas_2" onclick="ajustar_derecho('herramientas')" class="color_posicion posicion_2" onmouseover="hand('_herramientas_2')"></div>
                             <div id="_herramientas_3" onclick="ajustar_izquierdo_60('herramientas')" class="color_posicion posicion_3" onmouseover="hand('_herramientas_3')"></div>
                             <div id="_herramientas_4" onclick="ajustar_derecho_40('herramientas')" class="color_posicion posicion_4" onmouseover="hand('_herramientas_4')"></div>                             
                             <div id="_herramientas_5" onclick="ajustar_izquierdo_40('herramientas')" class="color_posicion posicion_5" onmouseover="hand('_herramientas_5')"></div>                             
                             <div id="_herramientas_6" onclick="ajustar_derecho_60('herramientas')" class="color_posicion posicion_6" onmouseover="hand('_herramientas_6')"></div>                             
                             <div id="_herramientas_7" onclick="ajustar_arriba_izquierdo('herramientas')" class="color_posicion posicion_7" onmouseover="hand('_herramientas_7')"></div>
                             <div id="_herramientas_8" onclick="ajustar_arriba_derecho('herramientas')" class="color_posicion posicion_8" onmouseover="hand('_herramientas_8')"></div>
                             <div id="_herramientas_9" onclick="ajustar_abajo_izquierdo('herramientas')" class="color_posicion posicion_9" onmouseover="hand('_herramientas_9')"></div>
                             <div id="_herramientas_10" onclick="ajustar_abajo_derecho('herramientas')" class="color_posicion posicion_10" onmouseover="hand('_herramientas_10')"></div>
                             <div id="_herramientas_11" onclick="maximizar_ventana('herramientas')" class="color_posicion posicion_11" onmouseover="hand('_herramientas_11')"></div>
                             <div id="_herramientas_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_herramientas_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                             <div id="_herramientas_13" onclick="ajustar_arriba('herramientas')" class="color_posicion posicion_13" onmouseover="hand('_herramientas_13')"></div>
                             <div id="_herramientas_14" onclick="ajustar_abajo('herramientas')" class="color_posicion posicion_14" onmouseover="hand('_herramientas_14')"></div>
                             <div id="_herramientas_15" onclick="ajustar_arriba_40('herramientas')" class="color_posicion posicion_15" onmouseover="hand('_herramientas_15')"></div>
                             <div id="_herramientas_16" onclick="ajustar_abajo_60('herramientas')" class="color_posicion posicion_16" onmouseover="hand('_herramientas_16')"></div>
                             <div id="_herramientas_17" onclick="ajustar_arriba_60('herramientas')" class="color_posicion posicion_17" onmouseover="hand('_herramientas_17')"></div>
                             <div id="_herramientas_18" onclick="ajustar_abajo_40('herramientas')" class="color_posicion posicion_18" onmouseover="hand('_herramientas_18')"></div>
               
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('herramientas')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_herramientas" class="dragresizable_hija">
                 <iframe id="iherramientas" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
         </div>

        
        


        <!-- Aquí coloco las opciones de Gestión Documental -->
       <%-- <div id="consultas______" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="material-icons icono_menu" style="color: #6610f2;">manage_search</span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Consultas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('consultas______')"><img src="imagenes/web/Minimize-WF.png" style="height:16px;" border="0"/></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_consultas______" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('consultas______')"><img src="imagenes/web/Maximize-WF.png" style="height:16px;" border="0"/></a>
                   
                         <div id="_consultas_______posicion_orden" class="posicion_orden">
                             <div id="_consultas_______1" onclick="ajustar_izquierdo('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______1')" style="position:absolute;top:5px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______2" onclick="ajustar_derecho('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______2')" style="position:absolute;top:5px;left:60px;width:50px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______3" onclick="ajustar_izquierdo('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______3')" style="position:absolute;top:75px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______4" onclick="ajustar_arriba_derecho('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______4')" style="position:absolute;top:75px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______5" onclick="ajustar_abajo_derecho('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______5')" style="position:absolute;top:107px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______6" onclick="ajustar_izquierdo_60('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______6')" style="position:absolute;top:5px;left:120px;width:70px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______7" onclick="ajustar_derecho_40('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______7')" style="position:absolute;top:5px;left:195px;width:30px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______8" onclick="ajustar_arriba_izquierdo('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______8')" style="position:absolute;top:75px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-top-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______9" onclick="ajustar_arriba_derecho('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______9')" style="position:absolute;top:75px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______10" onclick="ajustar_abajo_izquierdo('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______10')" style="position:absolute;top:107px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______11" onclick="ajustar_abajo_derecho('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______11')" style="position:absolute;top:107px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_consultas_______12" onclick="maximizar_ventana('consultas______')" class="color_posicion" onmouseover="hand('_consultas_______12')" style="position:absolute;top:64px;left:109px;width:12px;height:12px;background-color:#e2e3e5;border-radius:50px;border:1px solid gray;"></div> 
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('consultas______')"><img src="imagenes/web/close-WF.png" title="Cerrar" border="0"/></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_consultas______" class="dragresizable_hija">
                <iframe id="iconsultas______" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="IO_almacenamiento" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="material-icons icono_menu" style="color: #6610f2;">inbox</span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;IO Almacenamiento</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('IO_almacenamiento')"><img src="imagenes/web/Minimize-WF.png" style="height:16px;" border="0"/></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_IO_almacenamiento" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('IO_almacenamiento')"><img src="imagenes/web/Maximize-WF.png" style="height:16px;" border="0"/></a>
                   
                         <div id="_IO_almacenamiento_posicion_orden" class="posicion_orden">
                             <div id="_IO_almacenamiento_1" onclick="ajustar_izquierdo('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_1')" style="position:absolute;top:5px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_2" onclick="ajustar_derecho('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_2')" style="position:absolute;top:5px;left:60px;width:50px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_3" onclick="ajustar_izquierdo('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_3')" style="position:absolute;top:75px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_4" onclick="ajustar_arriba_derecho('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_4')" style="position:absolute;top:75px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_5" onclick="ajustar_abajo_derecho('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_5')" style="position:absolute;top:107px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_6" onclick="ajustar_izquierdo_60('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_6')" style="position:absolute;top:5px;left:120px;width:70px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_7" onclick="ajustar_derecho_40('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_7')" style="position:absolute;top:5px;left:195px;width:30px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_8" onclick="ajustar_arriba_izquierdo('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_8')" style="position:absolute;top:75px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-top-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_9" onclick="ajustar_arriba_derecho('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_9')" style="position:absolute;top:75px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_10" onclick="ajustar_abajo_izquierdo('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_10')" style="position:absolute;top:107px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_11" onclick="ajustar_abajo_derecho('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_11')" style="position:absolute;top:107px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_IO_almacenamiento_12" onclick="maximizar_ventana('IO_almacenamiento')" class="color_posicion" onmouseover="hand('_IO_almacenamiento_12')" style="position:absolute;top:64px;left:109px;width:12px;height:12px;background-color:#e2e3e5;border-radius:50px;border:1px solid gray;"></div> 
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('IO_almacenamiento')"><img src="imagenes/web/close-WF.png" title="Cerrar" border="0"/></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_IO_almacenamiento" class="dragresizable_hija">
                <iframe id="iIO_almacenamiento" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>--%>
       

        
        <!-- Aquí coloco las opciones de Propietario -->
        <%--<div id="Facturas" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="material-icons icono_menu" style="color: #292929;">receipt</span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Facturas</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('Facturas')"><img src="imagenes/web/Minimize-WF.png" style="height:16px;" border="0"/></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_Facturas" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('Facturas')"><img src="imagenes/web/Maximize-WF.png" style="height:16px;" border="0"/></a>
                   
                         <div id="_Facturas_posicion_orden" class="posicion_orden">
                             <div id="_Facturas_1" onclick="ajustar_izquierdo('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_1')" style="position:absolute;top:5px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_2" onclick="ajustar_derecho('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_2')" style="position:absolute;top:5px;left:60px;width:50px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_3" onclick="ajustar_izquierdo('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_3')" style="position:absolute;top:75px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_4" onclick="ajustar_arriba_derecho('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_4')" style="position:absolute;top:75px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_5" onclick="ajustar_abajo_derecho('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_5')" style="position:absolute;top:107px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_6" onclick="ajustar_izquierdo_60('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_6')" style="position:absolute;top:5px;left:120px;width:70px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_7" onclick="ajustar_derecho_40('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_7')" style="position:absolute;top:5px;left:195px;width:30px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_8" onclick="ajustar_arriba_izquierdo('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_8')" style="position:absolute;top:75px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-top-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_9" onclick="ajustar_arriba_derecho('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_9')" style="position:absolute;top:75px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_10" onclick="ajustar_abajo_izquierdo('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_10')" style="position:absolute;top:107px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_11" onclick="ajustar_abajo_derecho('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_11')" style="position:absolute;top:107px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Facturas_12" onclick="maximizar_ventana('Facturas')" class="color_posicion" onmouseover="hand('_Facturas_12')" style="position:absolute;top:64px;left:109px;width:12px;height:12px;background-color:#e2e3e5;border-radius:50px;border:1px solid gray;"></div> 
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('Facturas')"><img src="imagenes/web/close-WF.png" title="Cerrar" border="0"/></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_Facturas" class="dragresizable_hija">
                <iframe id="iFacturas" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>--%>
        
       

        <!-- Aquí coloco las opciones de Estadisticas BI -->
       <%-- <div id="bi_asientos" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="material-icons icono_menu" style="color: #292929;">insights</span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;BI Asientos</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('bi_asientos')"><img src="imagenes/web/Minimize-WF.png" style="height:16px;" border="0"/></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_bi_asientos" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('bi_asientos')"><img src="imagenes/web/Maximize-WF.png" style="height:16px;" border="0"/></a>
                   
                         <div id="_bi_asientos_posicion_orden" class="posicion_orden">
                             <div id="_bi_asientos_1" onclick="ajustar_izquierdo('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_1')" style="position:absolute;top:5px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_2" onclick="ajustar_derecho('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_2')" style="position:absolute;top:5px;left:60px;width:50px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_3" onclick="ajustar_izquierdo('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_3')" style="position:absolute;top:75px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_4" onclick="ajustar_arriba_derecho('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_4')" style="position:absolute;top:75px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_5" onclick="ajustar_abajo_derecho('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_5')" style="position:absolute;top:107px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_6" onclick="ajustar_izquierdo_60('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_6')" style="position:absolute;top:5px;left:120px;width:70px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_7" onclick="ajustar_derecho_40('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_7')" style="position:absolute;top:5px;left:195px;width:30px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_8" onclick="ajustar_arriba_izquierdo('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_8')" style="position:absolute;top:75px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-top-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_9" onclick="ajustar_arriba_derecho('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_9')" style="position:absolute;top:75px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_10" onclick="ajustar_abajo_izquierdo('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_10')" style="position:absolute;top:107px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_11" onclick="ajustar_abajo_derecho('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_11')" style="position:absolute;top:107px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_asientos_12" onclick="maximizar_ventana('bi_asientos')" class="color_posicion" onmouseover="hand('_bi_asientos_12')" style="position:absolute;top:64px;left:109px;width:12px;height:12px;background-color:#e2e3e5;border-radius:50px;border:1px solid gray;"></div> 
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('bi_asientos')"><img src="imagenes/web/close-WF.png" title="Cerrar" border="0"/></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_bi_asientos" class="dragresizable_hija">
                <iframe id="ibi_asientos" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>

        <div id="bi_impuestos" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="material-icons icono_menu" style="color: #292929;">insights</span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;BI Impuestos</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('bi_impuestos')"><img src="imagenes/web/Minimize-WF.png" style="height:16px;" border="0"/></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_bi_impuestos" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('bi_impuestos')"><img src="imagenes/web/Maximize-WF.png" style="height:16px;" border="0"/></a>
                   
                         <div id="_bi_impuestos_posicion_orden" class="posicion_orden">
                             <div id="_bi_impuestos_1" onclick="ajustar_izquierdo('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_1')" style="position:absolute;top:5px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_2" onclick="ajustar_derecho('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_2')" style="position:absolute;top:5px;left:60px;width:50px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_3" onclick="ajustar_izquierdo('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_3')" style="position:absolute;top:75px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_4" onclick="ajustar_arriba_derecho('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_4')" style="position:absolute;top:75px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_5" onclick="ajustar_abajo_derecho('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_5')" style="position:absolute;top:107px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_6" onclick="ajustar_izquierdo_60('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_6')" style="position:absolute;top:5px;left:120px;width:70px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_7" onclick="ajustar_derecho_40('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_7')" style="position:absolute;top:5px;left:195px;width:30px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_8" onclick="ajustar_arriba_izquierdo('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_8')" style="position:absolute;top:75px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-top-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_9" onclick="ajustar_arriba_derecho('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_9')" style="position:absolute;top:75px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_10" onclick="ajustar_abajo_izquierdo('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_10')" style="position:absolute;top:107px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_11" onclick="ajustar_abajo_derecho('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_11')" style="position:absolute;top:107px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_bi_impuestos_12" onclick="maximizar_ventana('bi_impuestos')" class="color_posicion" onmouseover="hand('_bi_impuestos_12')" style="position:absolute;top:64px;left:109px;width:12px;height:12px;background-color:#e2e3e5;border-radius:50px;border:1px solid gray;"></div> 
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('bi_impuestos')"><img src="imagenes/web/close-WF.png" title="Cerrar" border="0"/></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_bi_impuestos" class="dragresizable_hija">
                <iframe id="ibi_impuestos" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>--%>

        <!-- Aquí coloco las opciones de Ayuda -->
        <div id="Atencion_cliente" style="visibility:hidden; display:inline;" class="screen">
             <table border="0" class="screen_table">
             <tr>
                 <td width="30"><span class="bi bi-ticket-perforated icono_menu" style="color: #292929;"></span></td>
                 <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Atencion Cliente</td>
                 <td class="move"></td>
                 <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('Atencion_cliente')"><i class="bi bi-arrow-bar-down" style="height:16px;" title="Minimizar"></i></a></td>
                 <td width="30" align="center" valign="bottom">
    
                     <div id="_Atencion_cliente" class="selector_movimiento">
                          <a href="#" title="Maximizar" onclick="maximizar_ventana('Atencion_cliente')"><i class="bi bi-border-all" style="height:16px;"></i></a>
   
                          <div id="_Atencion_cliente_posicion_orden" class="posicion_orden">
     
                              <div id="_Atencion_cliente_1" onclick="ajustar_izquierdo('Atencion_cliente')" class="color_posicion posicion_1" onmouseover="hand('_Atencion_cliente_1')"></div>
                              <div id="_Atencion_cliente_2" onclick="ajustar_derecho('Atencion_cliente')" class="color_posicion posicion_2" onmouseover="hand('_Atencion_cliente_2')"></div>
                              <div id="_Atencion_cliente_3" onclick="ajustar_izquierdo_60('Atencion_cliente')" class="color_posicion posicion_3" onmouseover="hand('_Atencion_cliente_3')"></div>
                              <div id="_Atencion_cliente_4" onclick="ajustar_derecho_40('Atencion_cliente')" class="color_posicion posicion_4" onmouseover="hand('_Atencion_cliente_4')"></div>                             
                              <div id="_Atencion_cliente_5" onclick="ajustar_izquierdo_40('Atencion_cliente')" class="color_posicion posicion_5" onmouseover="hand('_Atencion_cliente_5')"></div>                             
                              <div id="_Atencion_cliente_6" onclick="ajustar_derecho_60('Atencion_cliente')" class="color_posicion posicion_6" onmouseover="hand('_Atencion_cliente_6')"></div>                             
                              <div id="_Atencion_cliente_7" onclick="ajustar_arriba_izquierdo('Atencion_cliente')" class="color_posicion posicion_7" onmouseover="hand('_Atencion_cliente_7')"></div>
                              <div id="_Atencion_cliente_8" onclick="ajustar_arriba_derecho('Atencion_cliente')" class="color_posicion posicion_8" onmouseover="hand('_Atencion_cliente_8')"></div>
                              <div id="_Atencion_cliente_9" onclick="ajustar_abajo_izquierdo('Atencion_cliente')" class="color_posicion posicion_9" onmouseover="hand('_Atencion_cliente_9')"></div>
                              <div id="_Atencion_cliente_10" onclick="ajustar_abajo_derecho('Atencion_cliente')" class="color_posicion posicion_10" onmouseover="hand('_Atencion_cliente_10')"></div>
                              <div id="_Atencion_cliente_11" onclick="maximizar_ventana('Atencion_cliente')" class="color_posicion posicion_11" onmouseover="hand('_Atencion_cliente_11')"></div>
                              <div id="_Atencion_cliente_12" onclick="$('#img_grid').click();" class="color_posicion posicion_12" onmouseover="hand('_Atencion_cliente_12')"><i class="bi bi-grip-horizontal" style="font-size:38px;"></i></div>
                              <div id="_Atencion_cliente_13" onclick="ajustar_arriba('Atencion_cliente')" class="color_posicion posicion_13" onmouseover="hand('_Atencion_cliente_13')"></div>
                              <div id="_Atencion_cliente_14" onclick="ajustar_abajo('Atencion_cliente')" class="color_posicion posicion_14" onmouseover="hand('_Atencion_cliente_14')"></div>
                              <div id="_Atencion_cliente_15" onclick="ajustar_arriba_40('Atencion_cliente')" class="color_posicion posicion_15" onmouseover="hand('_Atencion_cliente_15')"></div>
                              <div id="_Atencion_cliente_16" onclick="ajustar_abajo_60('Atencion_cliente')" class="color_posicion posicion_16" onmouseover="hand('_Atencion_cliente_16')"></div>
                              <div id="_Atencion_cliente_17" onclick="ajustar_arriba_60('Atencion_cliente')" class="color_posicion posicion_17" onmouseover="hand('_Atencion_cliente_17')"></div>
                              <div id="_Atencion_cliente_18" onclick="ajustar_abajo_40('Atencion_cliente')" class="color_posicion posicion_18" onmouseover="hand('_Atencion_cliente_18')"></div>
     
                         </div>

                     </div>

                 </td> 
                 <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('Atencion_cliente')"><i class="bi bi-x-lg text-danger"></i></a></td>
             </tr>
             </table>
             <br />
             <div id="iframe_Atencion_cliente" class="dragresizable_hija">
                 <iframe id="iAtencion_cliente" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
             </div>
        </div>

       
        <!-- Aquí coloco las opciones de Errores -->
       <%-- <div id="Controles" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30"><span class="material-icons icono_menu">quiz</span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Controles</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('Controles')"><img src="imagenes/web/Minimize-WF.png" style="height:16px;" border="0"/></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_Controles" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('Controles')"><img src="imagenes/web/Maximize-WF.png" style="height:16px;" border="0"/></a>
                   
                         <div id="_Controles_posicion_orden" class="posicion_orden">
                             <div id="_Controles_1" onclick="ajustar_izquierdo('Controles')" class="color_posicion" onmouseover="hand('_Controles_1')" style="position:absolute;top:5px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_2" onclick="ajustar_derecho('Controles')" class="color_posicion" onmouseover="hand('_Controles_2')" style="position:absolute;top:5px;left:60px;width:50px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_3" onclick="ajustar_izquierdo('Controles')" class="color_posicion" onmouseover="hand('_Controles_3')" style="position:absolute;top:75px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_4" onclick="ajustar_arriba_derecho('Controles')" class="color_posicion" onmouseover="hand('_Controles_4')" style="position:absolute;top:75px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_5" onclick="ajustar_abajo_derecho('Controles')" class="color_posicion" onmouseover="hand('_Controles_5')" style="position:absolute;top:107px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_6" onclick="ajustar_izquierdo_60('Controles')" class="color_posicion" onmouseover="hand('_Controles_6')" style="position:absolute;top:5px;left:120px;width:70px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_7" onclick="ajustar_derecho_40('Controles')" class="color_posicion" onmouseover="hand('_Controles_7')" style="position:absolute;top:5px;left:195px;width:30px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_8" onclick="ajustar_arriba_izquierdo('Controles')" class="color_posicion" onmouseover="hand('_Controles_8')" style="position:absolute;top:75px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-top-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_9" onclick="ajustar_arriba_derecho('Controles')" class="color_posicion" onmouseover="hand('_Controles_9')" style="position:absolute;top:75px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_10" onclick="ajustar_abajo_izquierdo('Controles')" class="color_posicion" onmouseover="hand('_Controles_10')" style="position:absolute;top:107px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_11" onclick="ajustar_abajo_derecho('Controles')" class="color_posicion" onmouseover="hand('_Controles_11')" style="position:absolute;top:107px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_Controles_12" onclick="maximizar_ventana('Controles')" class="color_posicion" onmouseover="hand('_Controles_12')" style="position:absolute;top:64px;left:109px;width:12px;height:12px;background-color:#e2e3e5;border-radius:50px;border:1px solid gray;"></div> 
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('Controles')"><img src="imagenes/web/close-WF.png" title="Cerrar" border="0"/></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_Controles" class="dragresizable_hija">
                <iframe id="iControles" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>--%>

        <!-- Aquí coloco las opciones de Visualizador -->
       <%-- <div id="visualizador" style="visibility:hidden; display:inline;" class="screen">
            <table border="0" class="screen_table">
            <tr>
                <td width="30" align="center"><span class="material-icons icono_menu" style="color: #292929;">plagiarism</span></td>
                <td width="350" style="font-size: 14px; font-weight:500;" class="move">&nbsp;&nbsp;Visualizador</td>
                <td class="move"></td>
                <td width="30" align="center" valign="bottom"><a href="#" title="Minimizar" onclick="minimizar_ventana('visualizador')"><img src="imagenes/web/Minimize-WF.png" style="height:16px;" border="0"/></a></td>
                <td width="30" align="center" valign="bottom">
                    
                    <div id="_visualizador" class="selector_movimiento">
                         <a href="#" title="Maximizar" onclick="maximizar_ventana('visualizador')"><img src="imagenes/web/Maximize-WF.png" style="height:16px;" border="0"/></a>
                   
                         <div id="_visualizador_posicion_orden" class="posicion_orden">
                             <div id="_visualizador_1" onclick="ajustar_izquierdo('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_1')" style="position:absolute;top:5px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_2" onclick="ajustar_derecho('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_2')" style="position:absolute;top:5px;left:60px;width:50px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_3" onclick="ajustar_izquierdo('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_3')" style="position:absolute;top:75px;left:5px;width:50px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_4" onclick="ajustar_arriba_derecho('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_4')" style="position:absolute;top:75px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_5" onclick="ajustar_abajo_derecho('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_5')" style="position:absolute;top:107px;left:60px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_6" onclick="ajustar_izquierdo_60('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_6')" style="position:absolute;top:5px;left:120px;width:70px;height:60px;background-color:#e2e3e5;border-top-left-radius:5px;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_7" onclick="ajustar_derecho_40('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_7')" style="position:absolute;top:5px;left:195px;width:30px;height:60px;background-color:#e2e3e5;border-top-right-radius:5px;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_8" onclick="ajustar_arriba_izquierdo('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_8')" style="position:absolute;top:75px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-top-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_9" onclick="ajustar_arriba_derecho('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_9')" style="position:absolute;top:75px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-top-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_10" onclick="ajustar_abajo_izquierdo('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_10')" style="position:absolute;top:107px;left:120px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-left-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_11" onclick="ajustar_abajo_derecho('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_11')" style="position:absolute;top:107px;left:175px;width:50px;height:28px;background-color:#e2e3e5;border-bottom-right-radius:5px;border:1px solid gray;"></div>
                             <div id="_visualizador_12" onclick="maximizar_ventana('visualizador')" class="color_posicion" onmouseover="hand('_visualizador_12')" style="position:absolute;top:64px;left:109px;width:12px;height:12px;background-color:#e2e3e5;border-radius:50px;border:1px solid gray;"></div> 
                        </div>

                    </div>

                </td> 
                <td width="30" align="center" valign="bottom"><a href="#" title="Cerrar" onclick="cerrar_ventana('visualizador')"><img src="imagenes/web/close-WF.png" title="Cerrar" border="0"/></a></td>
            </tr>
            </table>
            <br />
            <div id="iframe_visualizador" class="dragresizable_hija">
                <iframe id="ivisualizador" src="actualizaciones/vacia.html" style="border: 0px solid red; width: 100%; height: 100%; padding : 0px; overflow: hidden;"></iframe>
            </div>
        </div>--%>

        </div> 
        
    <%--Fin del body contenedor--%>
    </div>
   
    <!-- Menu  ------------------------------------------------------------------------------------------------->
    <div id="barra_menu" class="transparente color">
        
        <div id="superior_menu">
            <img src="Imagenes/logo/logo_empresa.png" style="height:30px;margin-left:10px;margin-top:10px;" />
        </div>
        
        <div id="central_superior">
            
        </div>

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

                        <li data-bs-toggle="collapse" data-bs-target="#albaranes" class="collapsed">
                            <a href="#"><span class="bi bi-card-checklist" style="margin-left: 10px;color: #ffc107; font-size:15px;"></span><span style="font-weight:500;margin-left: 10px;">Albaranes</span><i class="bi bi-caret-down"></i></a>
                        </li>
                        <ul class="sub-menu collapse" id="albaranes">

                            <div class="container text-center" style="margin-top:10px; ">
                                 <div class="row row-cols-2 row-cols-lg-5 g-5 g-sm-3">
                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('altas_____','bi bi-columns-gap','1000','600','albaranes/altas.aspx','6')">
                                         <span class="bi bi-columns-gap text-secondary" style="font-size:30px;"></span><br />
                                         <span style="font-size:13px;">Altas</span>
                                     </a> 
                                 </div>

                                 <div class="col">
                                     <a href="#" onclick="abrir_ventana('consultas_____','bi bi-search','1000','600','albaranes/consultas.aspx','6')">
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

                                <div class="col">
                                    <a href="#" onclick="abrir_ventana('Agenda','bi bi-file-earmark-text','1000','600','agenda/agenda.aspx','7')">
                                        <span class="bi bi-file-earmark-text" style="font-size:30px;"></span><br />
                                        <span style="font-size:13px;">Agenda</span>
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
       
         <!-- Centrar Menu --->
        <div id="contenedor_menu">

            <!-- Imagen Inicio --->
            <img id="img_inicio" src="imagenes/logo/icono_windows_facturacion.png" title="Despliega las opciones, aquí comienza todo" onmouseover="hand('img_inicio')"/>

            <!-- Contenedores -->
            <div id="menu_abiertos">
              
            <div id="contenedor_asientos" class="contenedor_menus" style="visibility:hidden; display:none;">
                <div id="contenido_contenedor_asientos" class="contenido_contenedor transparente color">
                    <table id="barra_asientos" style="width: 100%;">
                        <tr><td colspan="2" style="padding:2px;border-bottom:1px solid #0d6efd; text-transform: uppercase; text-align:center; font-size:12px;"> Clientes</td></tr>
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp" style="color: #0d6efd"></span>
                <span class="bi bi-person-lines-fill conten_position" style="color: #0d6efd;"></span>
	        </div>

            <div id="contenedor_impuestos" class="contenedor_menus" style="visibility:hidden; display:none;">
                <div id="contenido_contenedor_impuestos" class="contenido_contenedor transparente color">
                    <table id="barra_impuestos" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #6f42c1; text-transform: uppercase; text-align:center; font-size:12px;"> Proveedores</td></tr>
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp" style="color: #6f42c1"></span>
                <span class="bi bi-person-lines-fill conten_position" style="color: #6f42c1"></span>
            </div>

            <div id="contenedor_inmovilizados" class="contenedor_menus" style="visibility:hidden; display:none;">
                <div id="contenido_contenedor_inmovilizados" class="contenido_contenedor transparente color">
                    <table id="barra_inmovilizados" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #28a745; text-transform: uppercase; text-align:center; font-size:12px;"> Artículos</td></tr>                   
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp" style="color: #28a745"></span>
                <span class="bi bi-columns-gap conten_position" style="color: #28a745"></span>
            </div>

            <div id="contenedor_cartera" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_cartera" class="contenido_contenedor transparente color">
                    <table id="barra_cartera" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #fd7e14; text-transform: uppercase; text-align:center; font-size:12px;"> Facturas</td></tr>   
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp" style="color: #fd7e14"></span>
                <span class="bi bi-receipt-cutoff conten_position" style="color: #fd7e14"></span>
            </div>
                
            <div id="contenedor_cobros_pagos" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_cobros_pagos" class="contenido_contenedor transparente color">
                    <table id="barra_cobros_pagos" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #dc3545; text-transform: uppercase; text-align:center; font-size:12px;"> Presupuestos</td></tr>
                    </table>
                </div>
                    <span class="bi bi-caret-up imgcolexp" style="color: #dc3545"></span>
                    <span class="bi bi-card-list conten_position" style="color: #dc3545"></span>
            </div>
               
            <div id="contenedor_remesas" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_remesas" class="contenido_contenedor transparente color">
                    <table id="barra_remesas" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #ffc107; text-transform: uppercase; text-align:center; font-size:12px;"> Albaranes</td></tr>   
                    </table>
                </div>
                    <span class="bi bi-caret-up imgcolexp" style="color: #ffc107"></span>
                    <span class="bi bi-card-checklist conten_position" style="color: #ffc107"></span>
            </div>


                 <%--
            <div id="contenedor_gestion" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_gestion" class="contenido_contenedor transparente color">
                    <table id="barra_gestion" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #6610f2; text-transform: uppercase; text-align:center; font-size:12px;"> Gestión Documental</td></tr>   
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp" style="color: #6610f2"></span>
                <span class="bi bi-folder-plus conten_position" style="color: #6610f2"></span>
            </div>--%>

            <div id="contenedor_informes" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_informes" class="contenido_contenedor transparente color">
                    <table id="barra_informes" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #292929; text-transform: uppercase; text-align:center; font-size:12px;"> Informes</td></tr>   
                    </table>
                </div>
                    <span class="bi bi-caret-up imgcolexp"></span>
                    <span class="bi bi-file-earmark-text conten_position"></span>
            </div> 

            <div id="contenedor_configuracion" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_configuracion" class="contenido_contenedor transparente color">
                    <table id="barra_configuracion" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:1px solid #292929; text-transform: uppercase; text-align:center; font-size:12px;"> Configuración</td></tr>
                    </table>
                </div>
                    <span class="bi bi-caret-up imgcolexp" style="color: #292929"></span>
                    <span class="bi bi-gear-wide-connected conten_position" style="color: #292929;"></span>
            </div> 

           <div id="contenedor_supervisor" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_supervisor" class="contenido_contenedor transparente color">
                    <table id="barra_supervisor" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:1px solid #292929; text-transform: uppercase; text-align:center; font-size:12px;"> Herramientas</td></tr>   
                    </table>
                </div>
                    <span class="bi bi-caret-up imgcolexp" style="color: #292929"></span>
                    <span class="bi bi-tools conten_position" style="color: #292929;"></span>
            </div> 
                <%-- 
            <div id="contenedor_estadisticas" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_estadisticas" class="contenido_contenedor transparente color">
                    <table id="barra_estadisticas" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #292929; text-transform: uppercase; text-align:center; font-size:12px;"> Estadísticas</td></tr>   
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp"></span>
                <span class="bi bi-graph-up-arrow conten_position"></span>
            </div> --%>

            <div id="contenedor_ayuda" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_ayuda" class="contenido_contenedor transparente color">
                    <table id="barra_ayuda" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #292929; text-transform: uppercase; text-align:center; font-size:12px;"> Ayuda</td></tr>   
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp"></span>
                <span class="bi bi-question-square conten_position"></span>
	        </div>

            <div id="contenedor_propietario" class="contenedor_menus" style="visibility:visible; display:none;">
                <div id="contenido_contenedor_propietario" class="contenido_contenedor transparente color">
                    <table id="barra_propietario" style="width: 100%";>
                        <tr><td colspan="2" style="padding:2px;border-bottom:3px solid #292929; text-transform: uppercase; text-align:center; font-size:12px;"> Propietario</td></tr>
                    </table>
                </div>
                <span class="bi bi-caret-up imgcolexp">expand_less</span>
                <span class="material-icons conten_position">card_travel</span>
            </div> 

            </div>
        
        </div>

        <!-- Menú Expandor para seleccionar el monitor virtual -->   
        <div id="menu_expandir_monitor" class="transparente color">

            <div id="monitor_1" onclick="ir_escritorio(1)" class=" color_posicion" onmouseover="hand('monitor_1')" title="Ir al Monitor 1" style="position:absolute; top: 5px; left: 5px; width:40px; height:40px; background-color: #e2e3e5;border-radius: 5px; border-bottom-right-radius:5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">1</span></div>
            <div id="monitor_2" onclick="ir_escritorio(2)" class=" color_posicion" onmouseover="hand('monitor_2')" title="Ir al Monitor 2" style="position:absolute; top: 5px; left: 50px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">2</span></div>
            <div id="monitor_3" onclick="ir_escritorio(3)" class=" color_posicion" onmouseover="hand('monitor_3')" title="Ir al Monitor 3" style="position:absolute; top: 5px; left: 95px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">3</span></div>
            <div id="monitor_4" onclick="ir_escritorio(4)" class=" color_posicion" onmouseover="hand('monitor_4')" title="Ir al Monitor 4" style="position:absolute; top: 50px; left: 5px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">4</span></div>
            <div id="monitor_5" onclick="ir_escritorio(5)" class=" color_posicion" onmouseover="hand('monitor_5')" title="Ir al Monitor 5" style="position:absolute; top: 50px; left: 50px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">5</span></div>
            <div id="monitor_6" onclick="ir_escritorio(6)" class=" color_posicion" onmouseover="hand('monitor_6')" title="Ir al Monitor 6" style="position:absolute; top: 50px; left: 95px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">6</span></div>
            <div id="monitor_7" onclick="ir_escritorio(7)" class=" color_posicion" onmouseover="hand('monitor_7')" title="Ir al Monitor 7" style="position:absolute; top: 95px; left: 5px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">7</span></div>
            <div id="monitor_8" onclick="ir_escritorio(8)" class=" color_posicion" onmouseover="hand('monitor_8')" title="Ir al Monitor 8" style="position:absolute; top: 95px; left: 50px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">8</span></div>
            <div id="monitor_9" onclick="ir_escritorio(9)" class=" color_posicion" onmouseover="hand('monitor_9')" title="Ir al Monitor 9" style="position:absolute; top: 95px; left: 95px; width:40px; height:40px;background-color: #e2e3e5; border-radius: 5px; border-bottom-right-radius: 5px;"><i class="bi bi-display" style="position:absolute;top:4px;left:10px;font-size:20px;"></i><span style="position:absolute;top:10px;left:17px;font-size:10px;">9</span></div>

        </div>
        
        <div id="gear_barra">

            <table style="height:50px;">
                <tr>
                    <td style="width:30px;text-align:center;">
                        <!-- Informacion -->   
                        <span id="img_informacion" class="bi bi-info-circle" style="font-size:15px;" title="Información de la empresa" onmouseover="hand('img_informacion')"></span><br />
                    </td>
                    <td style="width:340px;">
                        <!-- Información Usuario y Empresa -->
                        <asp:Label ID="lbl_informacion_extra" runat="server" Font-Size="10px"></asp:Label>
                        <asp:Literal ID="LT_barra_ocupacion_barra" runat="server"></asp:Literal>
                    </td>
                    <td style="width:30px;text-align:center;">
                        <!-- Configuración --->
                        <a href="#" onclick="abrir_ventana('configuracion_','bi bi-gear-wide-connected','500','440','configuracion/configuracion.aspx','8')">
                        <span id="img_configuracion" class="bi bi-gear-wide-connected" style="font-size:15px;" title="Configuración" onmouseover="hand('img_configuracion')"></span>
                        </a>
                    </td> 
                    <td style="width:80px;text-align:center;">
                        <!-- Selector de Monitor -->
                        <table id="monitores">
                        <tr>
                            <td>
                                <span id="numero_monitor"><label id="numero_pantalla" title="Monitor actual">1</label></span>
                            </td>
                            <td align="left">
                                <span id="arrow_left" class="bi bi-caret-left" title="Anterior Pantalla (Tecla: F1)" onmouseover="hand('arrow_left')"></span>
                            </td>
                            <td>
                                <span id="img_monitor" class="bi bi-display" onmouseover="hand('img_monitor')"></span>
                            </td>
                            <td align="right">
                                <span id="arrow_right" class="bi bi-caret-right" title="Siguiente Pantalla (Tecla: F2)" onmouseover="hand('arrow_right')"></span>
                            </td> 
                        </tr>
                        </table>
                    </td> 
                    <td style="width:30px;text-align:center;">
                        <!-- Limpiar Pantalla --->
                        <span id="img_limpia_pantalla" class="bi bi-border-outer" style="font-size:15px;" title="Cerrar todas las ventanas (Tecla: F5)" onmouseover="hand('img_limpia_pantalla')"></span>
                    </td>
                    <td style="width:30px;text-align:center;">
                        <!-- Re-ordenador de Ventanas -->   
                        <span id="img_grid" class="bi bi-layout-wtf" style="font-size:15px;" title="Organizar Ventanas (Tecla: F6)" onmouseover="hand('img_grid')"></span>
                    </td>
                    <td style="width:30px;text-align:center;">
                        <!-- Full Screen -->
                        <span id="full_pantalla" class="bi bi-arrows-fullscreen" style="font-size:15px;" title="Pantalla Completa (Tecla: F4)" onclick="toggleFullScreen()" onmouseover="hand('full_pantalla')"></span>
                    </td>
                    <td style="width:30px;text-align:center;">
                        <!-- Sonido del sistema -->     
                        <div id="imagen_sonido" title="Sonido del sistema" onmouseover="hand('imagen_sonido')"></div>
                    </td>
                    <td style="width:145px;">
                        <!-- Reloj -->
                        <table id="tabreloj">
                        <tr>
                            <td style="text-align: right;"><div id="clock"></div></td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 155px;"><asp:Label ID="lbl_fecha" runat="server"></asp:Label></td>
                        </tr>
                        </table> 
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

                <table style="width:100%;">
                <tr>
                    <td style="width:10%;border: 0px solid red;"></td>
                    <td align="center" style="width:30px;"><i class="bi bi-globe text-primary" style="font-size:22px; "></i></td>
                    <td style="font-size: 23px;"><a href="https://www.optimus-soluciones.com" target ="_blank"> www.optimus-soluciones.com</a></td>
                    <td align="center" style="width:30px;"><i class="bi bi-telephone text-primary" style="font-size:20px; "></i></td>
                    <td style="font-size: 23px;"><a href="tel:+34928583936">928583936</a> - <a href="tel:+34928583936">922937249</a></td>
                    <td align="center" style="width:30px;"><i class="bi bi-envelope-at text-primary" style="font-size:20px; "></i></td>
                    <td style="font-size: 23px;"><a href="mailto:info@optimizalo.com">info@optimizalo.com</a></td>
                    <td style="width:10%;border: 0px solid red;"></td>
                </tr>
                </table>

                <div class="row g-4 py-5 row-cols-1 row-cols-lg-3">
                  <div class="col d-flex align-items-start">
                    <div class="icon-square bg-light text-primary bi bi-geo-alt flex-shrink-0 me-3"></div>
                    <div>
                        <h5>Gran Canaria</h5>
                        <hr />
                        Calle Diego Ordaz, nº 5
                        <br />
                        35007, Las Palmas de Gran Canaria
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
