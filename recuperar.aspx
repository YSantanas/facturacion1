<%@ Page Language="VB" AutoEventWireup="false" CodeFile="recuperar.aspx.vb" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
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
    <link href="Content/login.css" rel="stylesheet" />
    <script src="Scripts/login.js"></script>

    <script>
         
        $(document).ready(function () {

            //-------------------------------------------------------------------------------------------------------------------
            //Auto Click
            $("#btn_recuperar").on('focus', function (e) {

                //click sobre grabar cabecera
                $("#btn_recuperar").click();

            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#btn_recuperar").on('click', function (e) {

                if ($("#txt_email").val() == '') {
                    error('El campo Correo Electrónico no puede estar vacío.')
                    setTimeout(function () { $('#txt_email').focus(); }, 100);
                    return false;

                } else {

                    if ($("#txt_email").val().indexOf('@', 0) == -1 || $("#txt_email").val().indexOf('.', 0) == -1) {
                        error('El correo electrónico introducido no es correcto.')
                        setTimeout(function () { $('#txt_email').focus(); }, 100);
                        $('#txt_email').select();
                        return false;
                    }

                }

            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control Click sobre Ojo
            $('#ver_pass').on('click', function () {

                $('#txt_password').attr('type', function (index, attr) {

                    if (attr == 'text') {
                        $("#ver_pass").attr("class", "bi bi-eye text-primary")
                        $("#ver_pass").attr("title", "Ver Password")
                        return 'password'
                    } else {
                        $("#ver_pass").attr("class", "bi bi-eye-slash text-primary")
                        $("#ver_pass").attr("title", "Ocultar Password")
                        return 'text'
                    };

                })

            });

             //-------------------------------------------------------------------------------------------------------------------
            //Control Click sobre Ojo
            $('#ver_repetir_pass').on('click', function () {

                $('#txt_repetir_password').attr('type', function (index, attr) {

                    if (attr == 'text') {
                        $("#ver_repetir_pass").attr("class", "bi bi-eye text-primary")
                        $("#ver_repetir_pass").attr("title", "Ver Password")
                        return 'password'
                    } else {
                        $("#ver_repetir_pass").attr("class", "bi bi-eye-slash text-primary")
                        $("#ver_repetir_pass").attr("title", "Ocultar Password")
                        return 'text'
                    };

                })

            });

            //-------------------------------------------------------------------------------------------------------------------
            //Auto Click
            $("#btn_cambiar").on('focus', function (e) {
                //click sobre grabar cabecera
                $("#btn_cambiar").click();
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#btn_cambiar").on('click', function (e) {

                if ($("#txt_password").val() == '') {
                    error('El campo Contraseña no puede estar vacío.')
                    setTimeout(function () { $("#txt_password").focus(); }, 100);
                    return false;
                } 

                if ($("#txt_repetir_password").val() == '') {
                    error('El campo Repetir Contraseña no puede estar vacío.')
                    setTimeout(function () { $("#txt_repetir_password").focus(); }, 100);
                    return false;
                } else {
                    //Comparo si las dos claves son iguales
                    if ($("#txt_repetir_password").val() != $("#txt_password").val()) {
                        error('El campo Contraseña y Repetir Contraseña no pueden ser distintos.')
                        setTimeout(function () { $("#txt_repetir_password").focus(); }, 100);
                        return false;
                    }
                }

            })
           
        });

    </script>

</head>

<body id="cuerpo" runat="server">
    
    <form id="form1" runat="server">

        <div class="transparent_negro"></div>
        
        <div class="container-fluid">

        <!-- Login -->
        <div class="card card-container">

        <p id="profile-name" class="profile-name-card d-flex flex-row justify-content-center alig-items-center">
            <img src="Imagenes/logo/logo_empresa.png" style="height:55px;"/>
        </p>

        <div id="tabreloj" class="text-secondary">
             <asp:Label ID="clock" runat="server" Font-Size="9px"></asp:Label><asp:Label ID="lbl_fecha" runat="server" Font-Size="9px"></asp:Label>
         </div>

        <asp:PlaceHolder ID="Ph_recordar" runat="server" Visible="true">

        <span id="reauth-email" class="reauth-email"></span>
        <div id="div-login-msg">
                                
            <table border="0" style="width:100%;">
            <tr>
                <td align="center"><label><span class="text-muted" style="font-size: 15px;">¡Vaya!, ¿No recuerdas tu clave?.<br />Déjanos tu e-mail y te ayudamos.</span> </label></td>
            </tr>
            </table>
            <p></p>
            <div class="input-group mb-3">
                <span class="input-group-text">
                    <i class="bi bi-envelope-at-fill" style="font-size:20px;"></i>
                </span>
                <asp:TextBox type="txt_email" ID="txt_email" class="form-control input-sm" runat="server" placeholder="Correo Electrónico" TextMode="SingleLine"></asp:TextBox>
                <span class="input-group-text">
                    <asp:LinkButton ID="btn_recuperar" runat="server" class="input-sm text-primary" Font-Size="16" title="Recuperar"><i class ="bi bi-box-arrow-in-right"></i></asp:LinkButton>
                </span> 
            </div>

        </div> 

        </asp:PlaceHolder>
        
        <asp:PlaceHolder ID="PH_recordar_OK" runat="server" Visible="false">
            <br />
            <table border="0" style="width:100%;">
            <tr>
                <td style="vertical-align:top; width: 50px;">
                    <span class="bi bi-envelope-check-fill text-primary" style="font-size:45px;"></span>
                </td> 
                <td align="center"><label><span class="text-muted">Ya lo tienes solucionado, <br />te hemos enviado instrucciones a tu e-mail de <asp:Label ID="lbl_correo_recuperacion" runat="server"  CssClass="text-primary" Font-Bold="True"></asp:Label>.</span> </label></td>
            </tr>
            </table>
            <br />
            <br />
        </asp:PlaceHolder> 

        <asp:PlaceHolder ID="PH_restaurar" runat="server" Visible="false">

        <span id="reauth-email2" class="reauth-email"></span>
        <div id="div-login-msg2">

            <table border="0" style="width:100%;">
            <tr>
                <td align="center"><label><span class="text-muted"><asp:Label ID="lbl_restaurar_clave" runat="server" CssClass="text-primary" font-Bold="True"></asp:Label>, vas a modificar tu contraseña, escríbela y confírmala.</span> </label></td>
            </tr>
            </table>
            <br />

            <div class="input-group mb-3">
                <span class="input-group-text">
                   <i class="bi bi-lock-fill" style="font-size:20px;"></i>
                 </span>
                <asp:TextBox type="password" ID="txt_password" class="form-control input-sm" runat="server" placeholder="Contraseña" TextMode="SingleLine"></asp:TextBox>
                <span class="input-group-text">
                     <i id="ver_pass" class="bi bi-eye text-primary" style="cursor:pointer; font-size:16px;" title ="Ver Password"></i>
                 </span> 
            </div>

            <div class="input-group mb-3">
                <span class="input-group-text">
                   <i class="bi bi-lock-fill" style="font-size:20px;"></i>
                 </span>
                <asp:TextBox type="password" ID="txt_repetir_password" class="form-control input-sm" runat="server" placeholder="Repetir Contraseña" TextMode="SingleLine"></asp:TextBox>
                <span class="input-group-text">
                     <i id="ver_repetir_pass" class="bi bi-eye text-primary" style="cursor:pointer; font-size:16px;" title ="Ver Password"></i>
                 </span> 
                <span class="input-group-text">
                    <asp:LinkButton ID="btn_cambiar" runat="server" class="input-sm text-primary" Font-Size="16" title="Cambiar"><i class ="bi bi-box-arrow-in-right"></i></asp:LinkButton>
                </span>
            </div>

        </div>

        </asp:PlaceHolder>  
            
        <asp:PlaceHolder ID="Ph_cambiar_ok" runat="server" Visible="false">
            <p></p>
            <table>
            <tr>
                <td align="center" style="vertical-align:top;">
                    <span class="bi bi-check2-circle text-primary" style="font-size:45px;"></span>
                </td>
                <td align="center"><label><span class="text-muted" style="font-size: 15px;">!Listo!, <br /> ya puedes acceder.</span> </label></td>
            </tr>
            </table>
        </asp:PlaceHolder> 
                         
        <asp:PlaceHolder ID="PH_caducado" runat="server" Visible="false">
            <br />
            <table border="0" style="width:100%;">
            <tr>
                <td style="text-align:center"><label><span class="text-muted"> <asp:Literal ID="Lt_caducado" runat="server"></asp:Literal></span> </label></td>
            </tr>
            </table>
            <br />
            <br />
        </asp:PlaceHolder> 

        <table style="position:relative; bottom :-40px;">
        <tr>
            <td><span class="text-secondary" style="font-size:9px;">&copy; <% =Year(Date.Now) %> Optimiza Gestores Documentales S.L.</span></td>
            <td>
                <a href="default.aspx">
                <span class="bi bi-arrow-bar-left text-primary" style="font-size:25px;position:relative; right:-20px;" title="Volver al Inicio"></span>
                </a> 

            </td>
        </tr>
        </table>

    </div><!-- /card-container -->

    </div><!-- /container -->
 
    </form>
</body>
</html>