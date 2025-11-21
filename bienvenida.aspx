<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bienvenida.aspx.vb" Inherits="bienvenida" EnableEventValidation="false"  %>

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

    <!-- JQUERY ------------------------------------------------------------------------------------------------->
    <script src="Scripts/jquery-3.7.1.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <script src="Scripts/alertify.js"></script>
    <script src="Scripts/device.js"></script>

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" />
    <link href="Scripts/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Scripts/assets/css/material-bootstrap-wizard.css" rel="stylesheet" />
    <link href="Scripts/assets/css/demo.css" rel="stylesheet" />
    <script src="Scripts/assets/js/jquery-2.2.4.min.js" type="text/javascript"></script>
    <script src="Scripts/assets/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Scripts/assets/js/jquery.bootstrap.js" type="text/javascript"></script>
    <script src="Scripts/assets/js/material-bootstrap-wizard.js"></script>
    <script src="Scripts/assets/js/jquery.validate.min.js"></script>

    <style>

    .transparent_negro {
        position: absolute;
        top: 1px;
        bottom: 1px;
        left: 1px;
        right: 1px;
        z-index: 0;
        opacity: 0.80;
        background-color: black;
        background-size: cover;
    }

    #logo {
        position: fixed;
        bottom: 35px;
        left: 18px;
        z-index: 2;
    }
    
    /* Sticky footer styles
    -------------------------------------------------- */

    .footer2 {
        position: fixed;
        bottom: 10px;
        left:20px;
        width: 100%;
        font-size: 12px;
        z-index: 2;
        color: white;
    }

    </style>

     <script>

        $(document).ready(function () {

            //Pantalla de Trabajo
            function mostrar_trabajando(mensaje) {

                //eliminamos el scroll de la pagina
                $("body").css({ "overflow-y": "hidden" });
                //guardamos en una variable el alto del que tiene tu browser que no es lo mismo que del DOM
                var alto = $(window).height();
                //agregamos en el body un div que sera que ocupe toda la pantalla y se muestra encima de todo
                $("body").append("<div id='pre-load-web'><div id='imagen-load'><table width='300' border='0'><tr><td align='center' height='100'><img src='imagenes/web/logo.png'></span></td></tr><tr><td align='center' height='40'><img src='imagenes/web/493.gif'><br><span class='small' style='color: white;'>" + mensaje + "</span></td></tr></table></div>");
                //le damos el alto
                $("#pre-load-web").css({ height: alto + "px" });
                //esta sera la capa que esta dento de la capa que muestra un gif
                $("#imagen-load").css({ "margin-top": (alto / 2) - 200 + "px" });

            }

            //Notificación de Error
            function error(mensaje) {
                alertify.error(mensaje)
                return false;
            }

            //Notificación de Correcto
            function ok(mensaje) {
                alertify.success(mensaje);
                return false;
            }

            //-------------------------------------------------------------------------------------------------------------------
            //Control de nombre fiscal
            $("#txt_nombre_fiscal").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_nombre_fiscal").val() == '') {
                        alertify.error('El campo Nombre Fiscal no puede estar vacío.')
                        setTimeout(function () { $('#txt_nombre_fiscal').focus(); }, 100);
                    } else {
                        $("#txt_nombre_comercial").val($("#txt_nombre_fiscal").val());
                    }
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de nombre comercial
            $("#txt_nombre_comercial").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_nombre_comercial").val() == '') {
                        alertify.error('El campo Nombre Comercial no puede estar vacío.')
                        setTimeout(function () { $('#txt_nombre_comercial').focus(); }, 100);
                    }
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de CIF
            $("#txt_cif").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_cif").val() == '') {
                        alertify.error('El campo C.I.F. no puede estar vacío.')
                        setTimeout(function () { $('#txt_cif').focus(); }, 100);
                    }
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#btn_actualizar").on('click', function (e) {

                //Limpio errores anteriores
                $("#lblerror").text('');

                if ($("#txt_nombre_fiscal").val() == '') {
                    alertify.error('El campo Nombre Fiscal no puede estar vacío.')
                    setTimeout(function () { $('#txt_nombre_fiscal').focus(); }, 100);
                    return false;
                }

                if ($("#txt_nombre_comercial").val() == '') {
                    alertify.error('El campo Nombre Comercial no puede estar vacío.')
                    setTimeout(function () { $('#txt_nombre_comercial').focus(); }, 100);
                    return false;
                }

                if ($("#txt_cif").val() == '') {
                    alertify.error('El campo C.I.F. no puede estar vacío')
                    setTimeout(function () { $('#txt_cif').focus(); }, 100);
                    return false;
                } else {
                    if (validar_dni_nif_nie($("#txt_cif").val(), 'txt_cif', 'N.I.F.') == false) { return false; };
                };
                
                //Mensaje al usuario
                mostrar_trabajando('Actualizando sus preferencia, ¿Listo para Empezar?.');

            })
      
            //Funcion validación de DNI
            function validar_dni_nif_nie(campo, idDiv, texto_mostrar) {

                var validChars = 'TRWAGMYFPDXBNJZSQVHLCKET';
                var str = campo.toString().toUpperCase();

                if (str.length < 8) {
                    alertify.error('El campo ' + texto_mostrar + ' tiene menos de 8 caracteres.')
                    setTimeout(function () { $('#' + idDiv + '').focus(); }, 100);
                    $('#' + idDiv + '').select();
                    return false;
                }

                //tipo nif empresa
                if (str.length == 9 && (str.substr(0, 1) == "A" || str.substr(0, 1) == "B" || str.substr(0, 1) == "C" || str.substr(0, 1) == "D" || str.substr(0, 1) == "E" || str.substr(0, 1) == "F" || str.substr(0, 1) == "G" || str.substr(0, 1) == "H" || str.substr(0, 1) == "J" || str.substr(0, 1) == "R")) {
                    return true;
                }

                var nifRexp = "";
                var nieRexp = "";

                if (str.length == 8) {
                    nifRexp = /^[0-9]{8}$/i;
                    nieRexp = /^[XYZ]{1}[0-9]{7}$/i;
                }

                if (str.length == 9) {
                    nifRexp = /^[0-9]{8}[TRWAGMYFPDXBNJZSQVHLCKET]{1}$/i;
                    nieRexp = /^[XYZAB]{1}[0-9]{7}[TRWAGMYFPDXBNJZSQVHLCKET]{1}$/i;
                }

                if (!nifRexp.test(str) && !nieRexp.test(str)) {
                    alertify.error('El campo ' + texto_mostrar + ' no tiene estructura de N.I.F.')
                    setTimeout(function () { $('#' + idDiv + '').focus(); }, 100);
                    $('#' + idDiv + '').select();
                    return false;
                }

                var nie = str
                    .replace(/^[X]/, '0')
                    .replace(/^[Y]/, '1')
                    .replace(/^[Z]/, '2');
                var letter = str.substr(-1);
                var charIndex = parseInt(nie.substr(0, 8)) % 23;

                if (nie.length == 8) {
                    $('#' + idDiv + '').val(campo.toString().toUpperCase() + validChars.charAt(charIndex));
                } else {
                    if (validChars.charAt(charIndex) === letter) {
                        return true;
                    } else {
                        alertify.error('El ' + texto_mostrar + ' no es válido.')
                        setTimeout(function () { $('#' + idDiv + '').focus(); }, 100);
                        $('#' + idDiv + '').select();
                        return false;
                    }
                }

            }

        });

    </script>

</head>
<body id="cuerpo" runat="server">
    
    <form id="form1" runat="server">
           
        <div class="transparent_negro"></div>

        <div id="logo">
            <img src="Imagenes/logo/logo_empresa.png" style="height:20px;"/>
        </div>

        <!--   Big container   -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-8 col-sm-offset-2">
                    <!--      Wizard container        -->
                    <div class="wizard-container">
                        <div class="card wizard-card" data-color="green" id="wizardProfile">
                                
                                <!--        You can switch " data-color="purple" "  with one of the next bright colors: "green", "orange", "red", "blue"       -->
                            <asp:PlaceHolder ID="PH_datos" runat="server">
                                <div class="wizard-header">
                                    
                                    <h3 class="wizard-title">
                                        <img src="Imagenes/logo/logo_empresa.png" style="height:60px;"/>
                                        <br /><br />
                                        Bienvenido a Optimus Facturación
                                    </h3>
                                    <h5>Necesitamos algunos datos para personalizar tu entorno de trabajo.</h5>
                                </div>
                                <div class="wizard-navigation bg-success">
                                    <ul>
                                        <li><a href="#datos_fiscales" data-toggle="tab">Datos Fiscales</a></li>
                                    </ul>
                                </div>

                                <div class="tab-content">
                                    <asp:PlaceHolder ID="PH_avisos" runat="server" Visible="false">
                                    <table runat="server" border="0" style="width:100%;">
                                    <tr>
                                        <td align="center"><asp:Label ID="lblerror" runat="server" ForeColor="Red" ></asp:Label></td>
                                    </tr>
                                    </table>
                                    </asp:PlaceHolder> 
                                    
                                    <div class="tab-pane" id="datos_fiscales">
                                        <div class="row">
                                            
                                            <div class="col-sm-10 col-sm-offset-1">
                                                <div class="input-group">
                                                    <span class="input-group-addon">
                                                        <i class="material-icons">create</i>
                                                    </span>
                                                    <div class="form-group label-floating">
                                                        <label class="control-label">Nombre Fiscal <small>(*)</small></label>
                                                        <asp:TextBox type="text" ID="txt_nombre_fiscal" class="form-control" runat="server" TextMode="SingleLine" MaxLength ="150" autofocus></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-group">
                                                    <span class="input-group-addon">
                                                        <i class="material-icons">create</i>
                                                    </span>
                                                    <div class="form-group label-floating">
                                                        <label class="control-label">Nombre Comercial <small>(*)</small></label>
                                                        <asp:TextBox type="text" ID="txt_nombre_comercial" class="form-control" runat="server" TextMode="SingleLine" MaxLength ="150" autofocus></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-group">
                                                    <span class="input-group-addon">
                                                        <i class="material-icons">create</i>
                                                    </span>
                                                    <div class="form-group label-floating">
                                                        <label class="control-label">C.I.F. <small>(*)</small></label>
                                                        <asp:TextBox type="text" ID="txt_cif" class="form-control" runat="server" TextMode="SingleLine" autofocus MaxLength ="15"></asp:TextBox>
                                                    </div>
                                                </div>
                                               
                                            </div>
                                        </div>
                                    </div>
                                      
                                </div>
                                <div class="wizard-footer">
                                    <div class="pull-right">
                                        <asp:Button ID="btn_actualizar" class="btn btn-success" runat="server" Text="Terminar" />
                                    </div>

                                    <div class="pull-left">
                                        <%--<input type='button' class='btn btn-previous btn-fill btn-default btn-wd' name='previous' value='Anterior' />--%>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="PH_empezar" runat="server" Visible ="false">
                                <div class="wizard-header">
                                    <br />
                                    <br />
                                    <br />
                                    <h3 class="wizard-title">
                                        Enhorabuena!
                                    </h3>
                                    <h5>Ya hemos terminado, sólo queda que te Identifiques y comiences.</h5>
                                    <br />
                                    <br />
                                    <asp:Button ID="btn_iniciar" class="btn  btn-success" runat="server" Text="Iniciar Sesión" />
                                </div>
                            </asp:PlaceHolder> 

                        </div>
                    </div> <!-- wizard container -->
                </div>
            </div><!-- end row -->
        </div> <!--  big container -->
  
         <!-- Pie-->
        <div class="footer2">
            <span>&copy; <% =Year(Date.Now) %> Optimiza Gestores Documentales S.L.</span>
        </div>

    </form>
</body>
</html>
