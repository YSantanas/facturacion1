<%@ Page Language="VB" AutoEventWireup="false" CodeFile="certificado.aspx.vb" Inherits="configuracion_certificado" %>

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
    <link href="../Content/fileinput.css" rel="stylesheet" />
    <script src="../Scripts/fileinput.js"></script>
    <script src="../Scripts/locales/es.js"></script>

    <script>

        $(document).ready(function () {

            //Cambio de posicion
            $("#img_sello").draggable({
                containment: "parent",
                stop: function (event, ui) {
                    $("#txt_x").val(parseInt(ui.position.left));
                    $("#txt_y").val(parseInt(ui.position.top));

                    //Lanzo la funcion
                    cambiar_certificado();
                }
            });

            function cambiar_certificado() {

                var id_empresa = $(window.parent.document).find("#txt_id_empresa").val();

                var valores = $("#txt_x").val() + "|" + $("#txt_y").val() + "|" + id_empresa;

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "../default.aspx/Json_certificado",
                    data: '{opcion: "actualizar", valores: "' + valores + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                   
                        //Ninguna acción

                    },
                    error: function errores(msg) { alert('Error: ' + msg.responseText); }
                });

            };

        });
       
    </script>

</head>
<body>
    
    <form id="form1" runat="server">
        
        <table id="regresar" onclick="volver_ventana();" onmouseover="hand(this.id);">
        <tr>
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #007bff;"></span>Menú Configuración</td>
        </tr>
        </table>

        <div class="container-fluid">
            
            <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>

            <div class="card">
             <div class="card-header">
                Certificado Digital
              </div>
              <div class="card-body">
            
                <asp:Literal ID="LT_certificado" runat="server"></asp:Literal>

                <asp:FileUpload ID="FileUpload_certificado" runat="server" class="file" type="file"></asp:FileUpload>
                <br />

                <div class="input-group input-group-sm mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">
                            Password:
                        </span>
                    </div>
                    <asp:TextBox ID="txt_certificado_digital" runat="server" class="form-control" placeholder="Escriba la contraseña"  maxlength="200"></asp:TextBox>
                </div>

                <div class="container-fluid" style="text-align:center">
                    <asp:Button ID="btn_subir_certificado" runat="server" Text="Subir Certificado" CssClass="btn btn-outline-primary btn-xs" />
                </div>
                <br />

              </div>
            </div>
            <br />

            <div class="card">
                <div class="card-header">
                Opciones
                </div>
                <div class="card-body">

                    <table style="width:100%;">
                    <tr>
                        <td style="width:310px;">
                            
                            <div id="contenedor" class="borde_entradas" style="width: 298px; height: 421px;">
                                <asp:Image ID="img_sello" runat="server" /> 
                            </div> 

                        </td>
                        <td style="width:2%;"></td>
                        <td valign="top">
                            
                            <div class="input-group input-group-sm mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">
                                        Motivo:
                                    </span>
                                </div>
                                <asp:TextBox ID="txt_motivo" runat="server" class="form-control" placeholder="Escriba el motivo que quiere que aparezca en el certificado"  maxlength="50"></asp:TextBox>
                            </div>

                            <div class="input-group input-group-sm mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">
                                        Localización:
                                    </span>
                                </div>
                                <asp:TextBox ID="txt_localizacion" runat="server" class="form-control" placeholder="Escriba la dirección que quiere que aparezca en el certificado"  maxlength="50"></asp:TextBox>
                            </div>

                            <div class="container-fluid" style="text-align:center">
                                <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar" CssClass="btn btn-outline-primary btn-xs"/>
                            </div>
                           
                        </td>
                    </tr>
                    </table>

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

        <!-- Modal Agregar-->
        <div class="modal" id="modal_agregar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
          <div class="modal-dialog">

	        <div class="modal-content">

		        <div class="modal-header">
                    <h1 class="modal-title fs-5">Confirmar</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
		        <div class="modal-body">
                    <asp:Literal ID="lt_mensaje_agregar" runat="server"></asp:Literal>
                    <br />
		        </div>
		        <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btn_si_agregar" runat="server" Text="Aplicar" CssClass="btn btn-primary"/>
                 </div>
	        </div>
          </div>
        </div>

        <div id="partes_hidden" style="visibility:hidden; display: none;">
            txt_x:<asp:TextBox ID="txt_x" runat="server" Width="400"></asp:TextBox><br />
            txt_y:<asp:TextBox ID="txt_y" runat="server" Width="400"></asp:TextBox>
        </div> 

        <script type="text/javascript">
           
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

            $("#FileUpload_certificado").fileinput({
                showUpload: false,
                language: 'es',
                allowedFileExtensions: ['pfx','p12'],
                browseClass: "btn btn btn-outline-primary"
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_subir_certificado").on('click', function (e) {
                
                //Campo fileupload
                if ($("#FileUpload_certificado").val() == '') {
                    error('No ha seleccionado ningún certificado.')
                    return false;
                }

                if ($("#txt_certificado_digital").val() == '') {
                    error('El campo Password no puede estar vacío.')
                    setTimeout(function () { $("#txt_certificado_digital").focus(); }, 100);
                    return false;
                }

                //Mensaje al usuario
                mostrar_trabajando('Subiendo Certificado, por favor espere');
                    
            })

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

    </form>
</body>
</html>
