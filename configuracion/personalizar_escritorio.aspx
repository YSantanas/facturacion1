<%@ Page Language="VB" AutoEventWireup="false" CodeFile="personalizar_escritorio.aspx.vb" Inherits="configuracion_personalizar_escritorio" %>

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

</head>
<body>
    
    <form id="form1" runat="server">
        
        <table id="regresar" onclick="volver_ventana();" onmouseover="hand(this.id);">
        <tr>
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #007bff;"></span>Menú Configuración</td>
        </tr>
        </table>

        <div class="container-fluid">
          
            <h5>Personalizar Escritorio</h5>
            <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
            
            <table style="width:100%;">
            <tr>
                <td valign="top" style="width:39%;">

                    <div class="card">
                    <div class="card-header">
                        Imagen
                    </div>
                    <div class="card-body">

                        <div class="jumbotron" style="text-align: center;">
                            <asp:Image ID="Image_logo" runat="server" width="100%" class="img-thumbnail" /> 
                        </div> 
                             
                    </div>
                    </div>

                </td>
                <td style="width:3%;"></td> 
                <td valign="top" style="width:68%;">

                    <div class="card">
                    <div class="card-header">
                        Seleccionar
                    </div>
                    <div class="card-body">

                        <asp:FileUpload ID="FileUpload_logo" runat="server" class="file" type="file"></asp:FileUpload>
                        <br />

                        <div class="container-fluid" style="text-align:center">
                            <asp:Button ID="btn_subir_logo" runat="server" Text="Subir Imagen" CssClass="btn btn-outline-primary btn-xs" />
                        </div>
                        <br />
                
                        <asp:CheckBox ID="chk_tema_original" runat="server" AutoPostBack ="true" /> 
                        <span class="text-secondary"><small>Aplicar Tema por Defecto</small></span>
                             
                    </div>
                    </div>
                    
                </td>
            </tr>
            </table>
            <br />

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
                    <h1 class="modal-title fs-5">Aviso</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
		        <div class="modal-body">
                    <asp:Literal ID="lt_mensaje_eliminar" runat="server"></asp:Literal>
                    <br />
		        </div>
		        <div class="modal-footer">
                    <button type="button" class="btn btn-outline-success" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btn_si_eliminar" runat="server" Text="Aplicar" CssClass="btn btn-outline-danger"/>
                 </div>
	        </div>
          </div>
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

            $("#FileUpload_logo").fileinput({
                showUpload: false,
                language: 'es',
                allowedFileExtensions: ['jpg'],
                browseClass: "btn btn btn-outline-primary"
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_subir_logo").on('click', function (e) {
                
                //CAmpo fileupload
                if ($("#FileUpload_logo").val() == '') {
                    error('No ha seleccionado ninguna imagen.')
                    return false;
                }

                //Mensaje al usuario
                mostrar_trabajando('Subiendo Imagen, por favor espere');
                    
            })

            //refresco fondo
            function refresco(ruta_calculada) {
                
                $(window.parent.document).find("#body_contenedor").css({ 'background': "url('" + ruta_calculada + "?" + Math.floor((Math.random() * 14) + 1) + "') no-repeat center center fixed" });
                
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

    </form>
</body>
</html>