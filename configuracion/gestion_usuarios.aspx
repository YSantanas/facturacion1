<%@ Page Language="VB" AutoEventWireup="false" CodeFile="gestion_usuarios.aspx.vb" Inherits="configuracion_gestion_usuarios" %>

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
    <script type="text/javascript" src="https://www.bing.com/maps/sdkrelease/mapcontrol?v=7.0&mkt=es-es" async defer></script>
    <link href="../Content/interior.css" rel="stylesheet" />
    <script src="../Scripts/interior.js"></script>
    <script src="../Scripts/bootstrap-strength.min.js"></script>
     
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

            //Barra para mostrar el nivel de seguridad de la clave
            $("#txt_paswword_usuario").bootstrapStrength({
            active: true,
	        slimBar: true,
	        meterClasses: {
		        weak: "progress-bar bg-success",
		        medium: "progress-bar bg-success",
		        good: "progress-bar bg-success"
	        }
            });

            //Pido permiso para geolocalizar
            navigator.geolocation.getCurrentPosition(GetPosition, funcionError, options);

            function funcionError(error) {
                alert("Si no permite la geolocalización, no podra crear nuevos usuarios.");
                $("#btn_grabar").attr("disabled","disabled");
            }

            function GetPosition(posicion) {
                $("#lbl_latitud").text(posicion.coords.latitude);
                $("#txt_latitud").val(posicion.coords.latitude);
                $("#lbl_longitud").text(posicion.coords.longitude);
                $("#txt_longitud").val(posicion.coords.longitude);
            }

            var options = {
                enableHighAccuracy: true,
                timeout: 45000
            };

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

                 if ($("#txt_nombre_usuario").val() == '') {
                    error('El campo Nombre no puede estar vacío.')
                    setTimeout(function () { $("#txt_nombre_usuario").focus(); }, 100);
                    return false;
                }

                if ($("#txt_codigo_usuario").val() == '') {
                    error('El campo E-mail no puede estar vacío.')
                    setTimeout(function () { $("#txt_codigo_usuario").focus(); }, 100);
                    return false;
                } else {
                    if ($("#txt_codigo_usuario").val().indexOf('@', 0) == -1 || $("#txt_codigo_usuario").val().indexOf('.', 0) == -1) {
                        error('El E-mail introducido no es correcto.')
                        setTimeout(function () { $("#txt_codigo_usuario").focus(); }, 100);
                        return false;
                    }
                }

                if ($("#txt_paswword_usuario").val() == '') {
                    error('El campo PassWord no puede estar vacío.')
                    setTimeout(function () { $("#txt_paswword_usuario").focus(); }, 100);
                    return false;
                } else {
                    if ($("#txt_paswword_usuario").val().length < 8) {
                        error('El campo PassWord no puede ser menor de 8 caracteres.')
                        setTimeout(function () { $("#txt_paswword_usuario").focus(); }, 100);
                        return false;
                    } 
                }
                               
                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Agregando Usuario, por favor espere.');

            })

              //Control para focus
            $("#btn_modificar").focusin(function () {
                $("#btn_modificar").click();
            });

            //Control para el click
            $("#btn_modificar").on('click', function () {

               if ($("#txt_nombre_usuario").val() == '') {
                    error('El campo Nombre no puede estar vacío.')
                    setTimeout(function () { $("#txt_nombre_usuario").focus(); }, 100);
                    return false;
                }

                if ($("#txt_codigo_usuario").val() == '') {
                    error('El campo E-mail no puede estar vacío.')
                    setTimeout(function () { $("#txt_codigo_usuario").focus(); }, 100);
                    return false;
                } else {
                    if ($("#txt_codigo_usuario").val().indexOf('@', 0) == -1 || $("#txt_codigo_usuario").val().indexOf('.', 0) == -1) {
                        error('El E-mail introducido no es correcto.')
                        setTimeout(function () { $("#txt_codigo_usuario").focus(); }, 100);
                        return false;
                    }
                }

                if ($("#txt_paswword_usuario").val() == '') {
                    error('El campo PassWord no puede estar vacío.')
                    setTimeout(function () { $("#txt_paswword_usuario").focus(); }, 100);
                    return false;
                } else {
                    if ($("#txt_paswword_usuario").val().length < 8) {
                        error('El campo PassWord no puede ser menor de 8 caracteres.')
                        setTimeout(function () { $("#txt_paswword_usuario").focus(); }, 100);
                        return false;
                    } 
                }

                //Mensaje al usuario
                $('#modal_agregar').modal('hide');
                mostrar_trabajando('Modificando Usuario, por favor espere.');

            })

        });

        
        function GetMap()
        {

            //Leo
            var coordenadas = $("#txt_posicion").val().split("|")

            //Elimino valores
            $("#txt_posicion").val('');

            if (coordenadas[0] != '' && coordenadas[1] != '') {
                           
                    bingMap = new Microsoft.Maps.Map('#mapDiv',
                        {
                            credentials: "AsM3mCa-J4RvmqgKfhTPGpGkBygKclJ5rXHdBjAjQEJffcH21CywXtf6DdeJQigW",
                                center: new Microsoft.Maps.Location(coordenadas[0], coordenadas[1]),
                                mapTypeId: Microsoft.Maps.MapTypeId.aerial,
                                zoom: 10
                        });

                    var center = bingMap.getCenter();
                    var pin = new Microsoft.Maps.Pushpin(center, {
                        text: '1',
                        title: coordenadas[2],
                        subTitle: 'Lat:' + coordenadas[0].substr(0,6) + ' Long: ' + coordenadas[1].substr(0,6),
                        color: '#146abd'});
                    
                    //Add handler for the pushpin click event.
                    bingMap.entities.push(pin);
                
                    //Mensaje usuario
                    $('#modal_geo').modal('show');

            }
           
        }

        function displayEventInfo(e) {
            if (e.targetType == "pushpin") {

                var pix = bingMap.tryLocationToPixel(e.target.getLocation(), Microsoft.maps.pixelReferencia.control);
                alert(e.target.getLocation())
                alert(pix.x)
                $("#infoboxTitle").html(e.target.title);
                $("#infoboxDescription").html(e.target.description);

                var infobox = $("#infoBox");
                infobox.css({
                    "top": (pix.y - 60) + "px",
                    "left": (pix.x + 5) + "px",
                    "visibility": "visible"
                });

                $("#mapDiv")[0].append(infobox);
            }
        }   

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
<body onload="GetMap();">
    <form id="form1" runat="server">
        
        <table id="regresar" onclick="volver_ventana();" onmouseover="hand(this.id);">
        <tr>
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #007bff;"></span>Menú Configuración</td>
        </tr>
        </table>

        <div class="container-fluid">

        <h5>Gestión de Usuarios</h5>
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
        cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Usuarios" EmptyDataText="No hay resultados."
        CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="Id,nombre,primer_apellido,segundo_apellido,email,password,nivel,baja,codigo_empresa,fecha_creacion,hora_creacion,ip,posicion"
        ShowHeaderWhenEmpty="false" AllowPaging="false" PageSize="25">
        <AlternatingRowStyle BackColor="White" />
    
            <Columns>

            <asp:BoundField DataField="nombre" HeaderText="Nombre" HeaderStyle-Width="100"
                SortExpression="nombre" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="primer_apellido" HeaderText="1º Apellido" HeaderStyle-Width="120px"
                SortExpression="primer_apellido" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>
    
            <asp:BoundField DataField="segundo_apellido" HeaderText="2º Apellido" HeaderStyle-Width="120px"
                SortExpression="segundo_apellido" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="email" HeaderText="E-mail"
                SortExpression="email" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="nivel" HeaderText="Nivel"
                SortExpression="nivel" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

            <asp:BoundField DataField="baja" HeaderText="Baja"
                SortExpression="baja" ReadOnly="True">
            <HeaderStyle CssClass="gvHeaderleft"  />
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>

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

            <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
            <ItemTemplate>
                <asp:Button ID="btn_ubicacion" 
                ControlStyle-CssClass="btn btn-outline-dark btn-sm No_activar" runat="server" 
                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                CommandName="ubicacion" Text="Ubicación" OnClientClick="mostrar_trabajando('Solicitando Ubicación, por favor espere.');" />
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
                        <h1 class="modal-title fs-5">Eliminar Usuario</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Literal ID="LT_mensaje" runat="server"></asp:Literal>
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-outline-success" data-bs-dismiss="modal">Cancelar</button>
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
                                    Nombre:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_nombre_usuario" runat="server" class="form-control" placeholder="Nombre"  maxlength="25"></asp:TextBox>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Primer Apellido:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_primer_apellido_usuario" runat="server" class="form-control" placeholder="Primer Apellido"  maxlength="15"></asp:TextBox>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Segundo Apellido:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_segundo_apellido_usuario" runat="server" class="form-control" placeholder="Segundo Apellido"  maxlength="15"></asp:TextBox>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Email:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_codigo_usuario" runat="server" class="form-control" placeholder="Email"  maxlength="50"></asp:TextBox>
                        </div>

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    PassWord:
                                </span>
                            </div>
                            <asp:TextBox ID="txt_paswword_usuario" runat="server" class="form-control" placeholder="PassWord"  maxlength="20"></asp:TextBox>
                        </div>
                        <br />

                        <div class="input-group input-group-sm mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    Nivel:
                                </span>
                            </div>
                            <asp:DropDownList ID="DDL_nivel_usuario" runat="server" class="form-control">
                            </asp:DropDownList>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 text-left">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="CB_baja" runat="server"/>
                                    <label class="custom-control-label" for="CB_baja" style="color:gray; font-size: 13px;">Usuario de Baja</label>
                                </div>
                            </div>
                        </div>

                        <hr />

                         <span style="font-size: 12px;color:#808080;">Posición Detectada:
                           Latitud: <span style="color:#155724;"><asp:Label ID="lbl_latitud" runat="server"></asp:Label></span> 
                           , Longitud: <span style="color:#155724;"><asp:Label ID="lbl_longitud" runat="server"></asp:Label></span> 
                           <asp:Literal ID="lt_informacion" runat="server"></asp:Literal> 
                        </span>

                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-outline-danger" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button id="btn_grabar" runat="server" CssClass="btn btn-outline-success" Text="Guardar" />
                        <asp:Button id="btn_modificar" runat="server" CssClass="btn btn-outline-success" Text="Modificar" />
                    </div>
                </div>
            </div>
        </div>
            
        <!-- Modal Geolocalizacion -->
        <div class="modal" id="modal_geo" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">
            
            <div class="modal-content">

                <div class="modal-header">
                    <h1 class="modal-title fs-5">Información</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <div class="modal-body">

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Fecha Creación:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_fecha_creacion" runat="server" class="form-control" placeholder="Fecha Creación"  maxlength="25" disabled></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Hora Creación:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_hora_creacion" runat="server" class="form-control" placeholder="Hora Creación"  maxlength="25" disabled></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                IP:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_ip" runat="server" class="form-control" placeholder="IP" disabled></asp:TextBox>
                    </div>
                    <div id='mapDiv' style="position:relative;width:100%; height:450px;border:10px solid #eaeaea;"></div>

                </div>
            </div>
            
            </div>
        </div>

        <div id="partes_hidden" style="visibility:hidden; display: none;">
            txt_index:<asp:TextBox ID="txt_index" runat="server" Width="400"></asp:TextBox><br />
            txt_latitud:<asp:TextBox ID="txt_latitud" runat="server" Width="400"></asp:TextBox><br />
            txt_longitud:<asp:TextBox ID="txt_longitud" runat="server" Width="400"></asp:TextBox><br />
            txt_posicion:<asp:TextBox ID="txt_posicion" runat="server" Width="400"></asp:TextBox><br />
        </div>

    </form>
</body>
</html>