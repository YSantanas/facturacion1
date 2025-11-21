<%@ Page Language="VB" AutoEventWireup="false" CodeFile="registro.aspx.vb" Inherits="login" %>

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
    <link href="Content/login.css" rel="stylesheet" />
    <script src="Scripts/bootstrap-strength.min.js"></script>
    
    <style>
                                                                                                                                                                                                         
        .card-container.card {
            margin-top: 100px !important;
        }
      
    </style>

    <script>
         
        $(document).ready(function () {

            //Pido permiso para geolocalizar
            navigator.geolocation.getCurrentPosition(GetPosition, funcionError, options);

            function funcionError(error) {
                alert(error.message);
            }

            function GetPosition(posicion) {
                $("#txt_latitud").val(posicion.coords.latitude);
                $("#txt_longitud").val(posicion.coords.longitude);
            }

            var options = {
                enableHighAccuracy: true,
                timeout: 45000
            };
            
            //-------------------------------------------------------------------------------------------------------------------
            //Control para capaque oscurece el video
            colors = ['0.60', '0.20', '0.10', '0.00'];
            var i = 0;
            animate_loop = function () {
                $('.transparent_negro ').animate({
                    opacity: colors[(i++) % colors.length]
                }, 3000, function () {
                    animate_loop();
                });
            }
            animate_loop();

            //Barra para mostrar el nivel de seguridad de la clave
            $("#txt_password").bootstrapStrength({
                slimBar: true
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de Nombre
            $("#txt_nombre").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_nombre").val() == '') {
                        error('El campo Nombre no puede estar vacío.')
                        setTimeout(function () { $('#txt_nombre').focus(); }, 100);
                    } 
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de email
            $("#txt_email").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_email").val() == '') {
                        error('El campo Correo Electrónico no puede estar vacío.')
                        setTimeout(function () { $('#txt_email').focus(); }, 100);
                    } else {
                        if ($("#txt_email").val().indexOf('@', 0) == -1 || $("#txt_email").val().indexOf('.', 0) == -1) {
                            error('El correo electrónico introducido no es correcto.')
                            setTimeout(function () { $('#txt_email').focus(); }, 100);
                            $('#txt_email').select();
                        }
                    }
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de Password
            $("#txt_password").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_password").val() == '') {
                        error('El campo Password no puede estar vacío.')
                        setTimeout(function () { $('#txt_password').focus(); }, 100);
                    } else {
                        if ($("#txt_password").val().length < 8) {
                            error('El campo Password no puede ser inferior a 8 dígitos.')
                            setTimeout(function () { $('#txt_password').focus(); }, 100);
                        }
                    }
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de Re-Password
            $("#txt_password_repeat").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_password_repeat").val() == '') {
                        error('El campo Repetir Password no puede estar vacío.')
                        setTimeout(function () { $('#txt_password_repeat').focus(); }, 100);
                    } else {
                        if ($("#txt_password_repeat").val().length < 8) {
                            error('El campo Repetir Password no puede ser inferior a 8 dígitos.')
                            setTimeout(function () { $('#txt_password_repeat').focus(); }, 100);
                        }
                    }
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#btn_crear_bd").on('click', function (e) {

                if ($("#txt_nombre").val() == '') {
                    error('El campo Nombre no puede estar vacío.')
                    setTimeout(function () { $('#txt_nombre').focus(); }, 100);
                    return false;
                } 

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

                if ($("#txt_password").val() == '') {
                    error('El campo Password no puede estar vacío.')
                    setTimeout(function () { $('#txt_password').focus(); }, 100);
                    return false;
                } else {
                    if ($("#txt_password").val().length < 8) {
                        error('El campo Password no puede ser inferior a 8 dígitos.')
                        setTimeout(function () { $('#txt_password').focus(); }, 100);
                        return false;
                    }
                }

                if ($("#txt_password_repeat").val() == '') {
                    error('El campo Repetir Password no puede estar vacío.')
                    setTimeout(function () { $('#txt_password_repeat').focus(); }, 100);
                    return false;
                } else {
                    if ($("#txt_password_repeat").val().length < 8) {
                        error('El campo Repetir Password no puede ser inferior a 8 dígitos.')
                        setTimeout(function () { $('#txt_password_repeat').focus(); }, 100);
                        return false;
                    }
                }

                if ($("#txt_password").val() != $("#txt_password_repeat").val()) {
                    error('El campo PassWord y Repetir Password son distintos.')
                    setTimeout(function () { $('#txt_password_repeat').focus(); }, 100);
                    return false;
                }

                if ($("#chk_aviso").prop('checked') == false) {
                    alertify.error('Debe aceptar Las Condiciones de Uso para poder continuar.')
                    setTimeout(function () { $('#chk_aviso').focus(); }, 100);
                    return false;
                }
                
            })
            
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

            })
            
            //-------------------------------------------------------------------------------------------------------------------
            //Control de repetir password
            $("#txt_repetir_password").on('keydown', function (e) {
                var keyCode = e.which;
                //Restriccion con la tecla TAB=9
                if (keyCode === 9) {
                    if ($("#txt_repetir_password").val() == '') {
                        error('El campo Repetir PassWord no puede estar vacío.')
                        setTimeout(function () { $("#txt_repetir_password").focus(); }, 100);
                        return false;
                    } else {
                        //Comparo si ha cambiado el correo
                        if ($("#txt_repetir_password").val() != $("#txt_password").val()) {
                            error('El campo PassWord y Repetir PassWord no pueden ser distintos.')
                            setTimeout(function () { $("#txt_repetir_password").focus(); }, 100);
                            return false;
                        }
                    }
                };
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#btn_cambiar").on('click', function (e) {

                if ($("#txt_password").val() == '') {
                    error('El campo PassWord no puede estar vacío.')
                    setTimeout(function () { $("#txt_password").focus(); }, 100);
                    return false;
                } 

                if ($("#txt_repetir_password").val() == '') {
                    error('El campo Repetir PassWord no puede estar vacío.')
                    setTimeout(function () { $("#txt_repetir_password").focus(); }, 100);
                    return false;
                } else {
                    //Comparo si ha cambiado el correo
                    if ($("#txt_repetir_password").val() != $("#txt_password").val()) {
                        error('El campo PassWord y Repetir PassWord no pueden ser distintos.')
                        setTimeout(function () { $("#txt_repetir_password").focus(); }, 100);
                        return false;
                    }
                }

            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#modal_notificacion").on('click', function (e) {
                hablar("Condiciones de uso")
            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#modal_privacidad").on('click', function (e) {
                hablar("Política de privacidad")
            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control de click
            $("#modal_cookies").on('click', function (e) {
                hablar("Política de Cookies")
            })
            
            //Informo de resolución de pantalla baja
            if ($(window).width() < 1280) {
                alertify.warning("La resolución de tu monitor es muy baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.", 20);
                hablar("La resolución de tu monitor es muy baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.")
            }
            if ($(window).width() >= 1280 && $(window).width() < 1440) {
                alertify.warning("La resolución de tu monitor es baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.", 20);
                hablar("La resolución de tu monitor es baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.")
            } 
           
        });

        //Funcion de habla
        function hablar(texto) {
            var msg = new SpeechSynthesisUtterance(texto);
            msg.rate = 1.2; // 0.1 to 10
            msg.pitch = 1; //0 to 2
            window.speechSynthesis.speak(msg);
        }

        //Notificación de Error
        function error(mensaje) {
            alertify.error(mensaje);
            hablar(mensaje);
            return false;
        }

    </script>

</head>

<body id="cuerpo" runat="server">
    
    <form id="form1" runat="server">
    
        <div class="transparent_negro"></div>

        <div class="container-fluid ">

        <!-- Login -->
        <div class="card card-container">

        <p id="profile-name" class="profile-name-card d-flex flex-row justify-content-center alig-items-center">
            <img src="Imagenes/logo/logo_empresa.png" style="height:55px;"/>
        </p>
            
        <asp:PlaceHolder ID="PH_login" runat="server" Visible="true">

            <span id="reauth-email" class="reauth-email"></span>
            <div id="div-login-msg">
                                 
                <table style="width:100%;">
                <tr>
                    <td align="center"><label><span class="text-muted">¿Estás Listo?, Activa tu cuenta.</span> </label></td>
                </tr>
                </table>
                <br />

                <div class="input-group mb-3">
                  <div class="input-group-prepend">
                    <span class="input-group-text">
                        <i class="bi bi-person-fill" style="font-size:20px;"></i> 
                    </span>
                  </div>
                <asp:TextBox type="text" ID="txt_nombre" class="form-control input-sm" runat="server" placeholder="Nombre" TextMode="SingleLine" MaxLength="25"></asp:TextBox>
                </div>

                <div class="input-group mb-3">
                  <div class="input-group-prepend">
                    <span class="input-group-text">
                        <i class="bi bi-envelope-at-fill" style="font-size:20px;"></i> 
                    </span>
                  </div>
                <asp:TextBox type="text" ID="txt_email" class="form-control input-sm" runat="server" placeholder="Correo Electrónico" TextMode="SingleLine" MaxLength ="80"></asp:TextBox>
                </div>

                <div class="input-group mb-3">
                  <div class="input-group-prepend">
                    <span class="input-group-text">
                        <i class="bi bi-lock-fill" style="font-size:20px;"></i>
                    </span>
                  </div>
                <asp:TextBox type="password" ID="txt_password" class="form-control input-sm" runat="server" placeholder="PassWord" TextMode="password" MaxLength="20"></asp:TextBox>
                </div>

                <div class="input-group mb-3">
                  <div class="input-group-prepend">
                    <span class="input-group-text">
                        <i class="bi bi-lock-fill" style="font-size:20px;"></i> 
                    </span>
                  </div>
                <asp:TextBox type="password" ID="txt_password_repeat" class="form-control input-sm" runat="server" placeholder="Repetir PassWord" TextMode="password" MaxLength ="20"></asp:TextBox>
                </div>
               
            </div>
            <p></p>

            <asp:Button ID="btn_crear_bd" runat="server" Text="Crear Cuenta" class="btn btn-outline-primary btn-xs" Enabled ="true" />
            </asp:PlaceHolder>

        <asp:PlaceHolder ID="PH_multi_empresa" runat="server" Visible="false">
                
            <span id="reauth-email2" class="reauth-email"></span>
            <div id="div-login-msg2">

            <table border="0" style="width:100%;">
            <tr>
                <td align="center"><label><span class="text-muted"><asp:Label ID="lbl_aviso_multiempresa" runat="server" Text="" ForeColor ="#155724"></asp:Label>, ya tienes empresas registradas con este e-mail, ¿Deseas asociar esta nueva empresa a tu cuenta?.</span> </label></td>
            </tr>
            </table>
            <br />
                    
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                <span class="input-group-text">
                    <i class="bi bi-lock-fill" style="font-size:20px;"></i>
                </span>
                </div>
            <asp:TextBox type="password" ID="txt_password_multiempresa" class="form-control input-sm" runat="server" placeholder="PassWord" TextMode="Password"></asp:TextBox>
            </div>
                <asp:TextBox ID="txt_id_usuario" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txt_demo" runat="server" Visible="false" Text="1"></asp:TextBox>
            </div>
        <p></p>

        <table border="0" style="width:100%;">
        <tr>
            <td align="center"><asp:Button ID="btn_multiempresa" runat="server" Text="Asociar a Cuenta" class="btn btn-outline-primary btn-xs" /></td>
            <td align="center"><asp:Button ID="btn_cancelar_multiempresa" runat="server" Text="Cancelar" class="btn btn-danger btn-xs" /></td>
        </tr>
        </table>

        </asp:PlaceHolder>

        <asp:PlaceHolder ID="PH_acceder" runat="server" Visible="false">
            <br />
            <table style="width:100%;" border="0">
            <tr>
                <td align="center">
                    <label>
                        <i class="bi bi-hand-thumbs-up text-primary" style="font-size:80px;"></i> 
                        <span class="text-muted">
                        <br />Ya estamos listos para empezar</span> </label>
                    <br />
                    <br />
                    <asp:Button ID="btn_login" runat="server" Text="Iniciar Sesión" class="btn btn-outline-primary btn-sm" Width="100" />
                </td>
            </tr>
            </table>
        </asp:PlaceHolder>

        <hr style="border-top: 1px dotted #b3b3b3 !important; " />
         
        <div class="custom-control custom-checkbox">
                                
            <input type="checkbox" class="custom-control-input" id="chk_aviso" runat="server"/>
            <label class="custom-control-label text-muted" for="chk_aviso" style="font-size : 11px;">
                Acepto las <a id="modal_notificacion" href="condiciones.aspx" data-toggle="modal" data-target="#condiciones">Condiciones de uso</a>, nuestro <a id="modal_privacidad" href="privacidad.aspx" data-toggle="modal" data-target="#privacidad">Política de Privacidad</a> y nuestras <a id="modal_cookies" href="cookies.aspx" data-toggle="modal" data-target="#cookies">Condiciones de Cookies y publicidad en Internet</a>. 
            </label>
           
            <table style="position:relative; bottom :-40px; left:0px;">
            <tr>
                <td><span class="text-secondary" style="font-size:9px;">&copy; <% =Year(Date.Now) %> Optimiza Gestores Documentales S.L.</span></td>
                <td><span id="img_dispositivo" class="material-icons text-success"></span></td>
            </tr>
            </table>

        </div> 
            
        <div id="partes_hidden" style="visibility:hidden; display: none;">
            <asp:TextBox ID="txt_latitud" runat="server"></asp:TextBox>
            <asp:TextBox ID="txt_longitud" runat="server"></asp:TextBox>
        </div> 
      
        </div>
   
        </div>

     <%--   <!-- Modal Condiciones de Uso -->
        <div class="modal fade" id="condiciones" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
          <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLongTitle">Condiciones de Uso</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body" style="font-size: 14px;">
            
                  <p class="text-center" style="color:#000000">AVISO LEGAL</p>
              
                  <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bienvenido a este espacio. Como usuario, es importante que conozcas estos términos antes de continuar navegando. 
                    <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span> como responsable de esta página web, se compromete a procesar la información de nuestros usuarios y clientes con plenas garantías y cumplir con los requisitos nacionales y europeos que regulan la recopilación y uso de datos personales de nuestros usuarios.
                    Esta página Web, por lo tanto, cumple de forma rigurosa con la Ley Orgánica de Protección de Datos de Carácter Personal (en adelante LOPD) y demás normativa aplicable, con el Reglamento (UE) 2016/279 del Parlamento Europeo y del Consejo de 27 de abril de 2016 relativo a la protección de las personas físicas (en adelante RGPD) y con la Ley 34/2002, de 11 de julio, de Servicios de la Sociedad de la Información y Comercio Electrónico (en adelante LSSICE). 
                  </p>

                <p class="text-center" style="color:#000000">DATOS DEL RESPONSABLE</p>

                <ul class="list-unstyled">
                    <ul>
                        <li>Identidad del Responsable: <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span></li>
                        <li>Nombre comercial: <span style="color:#155724">IO CLOUD COMPUTING, S.L.</span></li>
                        <li>NIF/CIF: <span style="color:#155724">B09973652</span></li>
                        <li>Datos Registro Mercantil: <span style="color:#155724"><br />Reg. Mercantil de Las Palmas el 28/03/2022, Tomo 2289, Libro 0, Folio 202, Hoja CG-59505 e Inscripción 1ª.</span></li>
                        <li>Dirección: <span style="color:#155724"><br />Calle Margarita, 18<br />35212 - Playa de la Garita (Telde)<br />Las Palmas</span></li>
                        <li>Correo electrónico:  <span style="color:#155724">iocc@iocc.io</span></li>
                        <li>Actividad: <span style="color:#155724">(CNAE) “6201”</span></li>
                    </ul>
                </ul>

                <p class="text-center" style="color:#000000">CONDICIONES GENERALES DE USO</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Las presentes Condiciones Generales regulan el uso (incluyendo el acceso) de las páginas web integrantes del sitio web de <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span> incluidos los contenidos y servicios puestos a disposición en ellas. Toda persona que acceda a la web https://www.iocloudcomputing.io  (“usuario”) acepta someterse a las Condiciones Generales vigentes en cada momento. Por usuario se entiende a la persona que acceda, navegue, utilice o participe en los servicios y actividades desarrolladas en el sitio web.  
                </p>

                <p class="text-center" style="color:#000000">DATOS PERSONALES QUE RECOPILAMOS Y CÓMO LO HACEMOS</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Los términos relativos a la privacidad y protección de datos, están estipulados en nuestra Política de privacidad. 
                </p>

                <p class="text-center" style="color:#000000">OBLIGACIONES Y COMPROMISOS DE LOS USUARIOS</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El usuario queda informado, y acepta que, el acceso a la presente web, no supone en modo alguno, el inicio de una relación comercial con <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span> De esta forma, el usuario se compromete a utilizar la Web, sus servicios y contenidos sin contravenir la legislación vigente, la buena fe, los usos generalmente aceptados y el orden público. Así mismo, queda prohibido, el uso de la Web con fines ilícitos o lesivos contra <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span> o cualquier tercero o que, de cualquier forma, puedan causar perjuicio o impedir el normal funcionamiento de la Web. 
                    <br /><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Respecto de los contenidos (informaciones, textos, gráficos, archivos de sonido y/o imagen, fotografías, diseños, etc.), se prohíbe:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li>Su reproducción, distribución o modificación, a menos que se cuente con la autorización de sus legítimos titulares o resulte legalmente permitido.</li>
                        <li>Cualquier vulneración de los derechos de la empresa o de sus legítimos titulares sobre los mismos. </li>
                        <li>Su utilización para todo tipo de fines comerciales o publicitarios, distintos de los estrictamente permitidos. </li>
                        <li>Cualquier intento de obtener los contenidos de la Web por cualquier medio distinto de los que se pongan a disposición de los usuarios, así como de los que habitualmente se empleen en la red, siempre que no causen perjuicio alguno a la Web de <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span></li>
                    </ul>
                </ul>
 
                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;En la utilización de la web https://www.iocloudcomputing.io el usuario se compromete a no llevar a cabo ninguna conducta que pudiera dañar la imagen, los intereses y los derechos de <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, o de terceros o que pudiera dañar, inutilizar o sobrecargar el portal https://www.iocloudcomputing.io, o que impidiera, de cualquier forma, la normal utilización de la web.
                </p>

                 <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;No obstante, el usuario debe ser consciente de que las medidas de seguridad de los sistemas informáticos en Internet no son enteramente fiables y que, por tanto, <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, no puede garantizar la inexistencia de malware u otros elementos que puedan producir alteraciones en los sistemas informáticos (software y hardware) del usuario o en sus documentos electrónicos y ficheros contenidos en los mismos, aunque ponga todos los medios necesarios y las medidas de seguridad oportunas para evitar la presencia de estos elementos dañinos.
                </p>

                <p class="text-center" style="color:#000000">MEDIDAS DE SEGURIDAD</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Los datos personales comunicados por el usuario a <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, pueden ser almacenados en ficheros, cuya titularidad corresponde en exclusiva a <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, asumiendo ésta todas las medidas de índole técnica, organizativa y de seguridad que garantizan la confidencialidad, integridad y calidad de la información contenida en las mismas de acuerdo con lo establecido en la normativa vigente en protección de datos.
                    <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, utiliza un canal seguro y los datos transmitidos son cifrados gracias a protocolos a https, por tanto, garantizamos las mejores condiciones de seguridad para que la confidencialidad de los usuarios esté garantizada. 
                </p>

              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>

        <!-- Modal Política de Privacidad -->
        <div class="modal fade" id="privacidad" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
          <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLongTitle2">Política de Privacidad</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body" style="font-size: 14px;">
            
                <p class="text-center" style="color:#000000">POLÍTICA DE PRIVACIDAD</p>
              
                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Te damos la bienvenida a nuestra página web <span style="color:#155724">https://www.iocloudcomputing.io </span> , y te invitamos y explicamos los términos de nuestra política de privacidad antes de proporcionarnos tus datos personales en ella.
                </p>

                <p class="text-center" style="color:#000000">AVISO MENORES DE EDAD</p>
              
                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Los mayores de 14 años pueden registrarse en <span style="color:#155724">https://www.iocloudcomputing.io </span> como usuarios sin el consentimiento previo de sus padres o tutor. En menores de 14 años, se requiere el consentimiento de los padres o tutor para el tratamiento de sus datos personales.
                    En ningún supuesto se tomarán del menor de edad información relativa a la situación profesional, económica o a la intimidad de otros miembros de la familia, sin el consentimiento de éstos. 
                    Si eres menor de 14 años y has accedido a nuestra página web sin el previo aviso a tus padres, no debes registrarte como usuario.
                </p>

                <p class="text-center" style="color:#000000">PRIVACIDAD Y DERECHOS QUE SE RESPETAN EN <span style="color:#155724">https://www.iocloudcomputing.io </span></p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Nuestra empresa se caracteriza por el respeto y debido cuidado de los datos personales de nuestros usuarios/clientes. Como usuario, debes conocer que tus derechos están garantizados. 
                    <br /><br />
                    Nuestros principios en lo relativo a tu privacidad son los siguientes:            
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li>No pedimos información personal a menos que sea necesario para prestarte nuestros servicios.</li>
                        <li>No compartimos información personal de nuestros usuarios con nadie, excepto para cumplir con la ley o en el caso de que contemos con su autorización expresa.</li>
                        <li>No usamos sus datos personales con una finalidad distinta a la expresada en esta política de privacidad.</li>
                    </ul>
                </ul>

                <p class="text-center" style="color:#000000">AVISO SOBRE NUESTRA POLITICA DE PRIVACIDAD</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Puede cambiar atendiendo a las exigencias legislativas o de autorregulación, por lo que aconsejamos que la visite y revise periódicamente. Será aplicable en caso de que los usuarios decidan rellenar algún formulario de contacto donde se recaben datos personales.
                </p>

                <p class="text-center" style="color:#000000">REGULACIONES LEGALES A LAS QUE SE ACOGE ESTA WEB</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;https://www.iocloudcomputing.io, ha adaptado esta página web conforme a las exigencias de la Ley Orgánica de Protección de Datos de Carácter Personal (en adelante LOPD) y demás normativa aplicable, con el Reglamento (UE) 2016/679 del Parlamento Europeo y del Consejo de 27 de abril de 2016 relativo a la protección de las personas físicas (en adelante RGPD), así como con la Ley 34/2002, de 11 de julio, de Servicios de la Sociedad de la Información y Comercio Electrónico (en adelante LSSICE).
                </p>

                <p class="text-center" style="color:#000000">RESPONSABLE DEL TRATAMIENTO DE TUS DATOS PERSONALES</p>

                <ul class="list-unstyled">
                    <ul>
                        <li>Identidad del Responsable: <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span></li>
                        <li>Nombre comercial: <span style="color:#155724">IO CLOUD COMPUTING, S.L.</span></li>
                        <li>NIF/CIF: <span style="color:#155724">B09973652</span></li>
                        <li>Datos Registro Mercantil: <span style="color:#155724"><br />Reg. Mercantil de Las Palmas el 28/03/2022, Tomo 2289, Libro 0, Folio 202, Hoja CG-59505 e Inscripción 1ª.</span></li>
                        <li>Dirección: <span style="color:#155724"><br />Calle Margarita, 18<br />35212 - Playa de la Garita (Telde)<br />Las Palmas</span></li>
                        <li>Correo electrónico:  <span style="color:#155724">iocc@iocc.io</span></li>
                        <li>Actividad: <span style="color:#155724">(CNAE) “6201”</span></li>
                    </ul>
                </ul>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A efectos de lo previsto en el Reglamento General de Protección de Datos 2016/679 de 27 de abril de 2016, los datos personales que nos facilite mediante los formularios de la página web, recibirán el tratamiento de datos “web”. Para tratar los datos de nuestros usuarios, empleamos todas las medidas técnicas y organizativas de seguridad dispuestas en la legislación vigente.
                </p>

                <p class="text-center" style="color:#000000">PRINCIPIOS DE APLICACIÓN A SU INFORMACIÓN PERSONAL</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cuando tratemos con sus datos personales, emplearemos los principios contenidos en el Reglamento (UE) 2016/679 del Parlamento Europeo y del Consejo de 27 de abril de 2016 relativo a la protección de las personas físicas siendo estos:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li>Principio de licitud, lealtad y transparencia.</li>
                        <li>Principio de minimización de datos.</li>
                        <li>Principio de limitación del plazo de conservación.</li>
                        <li>Principio de integridad y confidencialidad.</li>
                    </ul>
                </ul>

                <p class="text-center" style="color:#000000">CÓMO HEMOS OBTENIDO TUS DATOS PERSONALES EN NUESTRA PÁGINA WEB</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Los datos personales que tratamos en https://www.iocloudcomputing.io, proceden de:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li>Formulario de contacto.</li>
                        <li>Alta de Cliente Web.</li>
                    </ul>
                </ul>

                <p class="text-center" style="color:#000000">DERECHO</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El interesado puede ejercer los siguientes derechos:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li><b>Derecho a solicitar el Acceso</b> a sus datos personales, y obtener información sobre si estamos tratando datos personales que le conciernen o no.</li>
                        <li><b>Derecho a solicitar la Rectificación</b> de sus datos inexactos, o en su caso, solicitar su Supresión cuando, entre otros motivos, los datos ya no sean necesarios para los fines para los que fueron recogidos.</li>
                        <li><b>Derecho a solicitar la Limitación del Tratamiento</b> de sus Datos, en cuyo caso únicamente los conservaremos para el ejercicio o defensa de reclamaciones.</li>
                        <li><b>Derecho a la Portabilidad de sus Datos.</b></li>
                        <li><b>Derecho a Retirar el Consentimiento Prestado</b>, sin que dicha retirada afecte a la licitud de los tratamientos anteriores basados en dicho consentimiento.</li>
                        <li><b>Derecho</b> a la tutela judicial efectiva <b>y derecho a reclamar ante una Autoridad de Control.</b></li>
                    </ul>
                </ul>

                <p class="text-center" style="color:#000000">FINALIDAD DEL TRATAMIENTO</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cuando un usuario se conecta con nuestra página web para, por ejemplo, enviar un correo al titular, realizar alguna contratación o solicitar algún servicio, está proporcionando información personal de la que es responsable <span style="color:#155724">https://www.iocloudcomputing.io </span>. Dicha información puede incluir datos personales como pueden ser dirección de IP, dirección física, nombre, dirección de correo electrónico, número de teléfono y otra información. Al facilitar dicha información, el usuario da su consentimiento para que su información sea recopilada, utilizada, gestionada y almacenada por <span style="color:#155724"> https://www.iocloudcomputing.io </span>, como sólo se describe y detalla en el Aviso Legal y en la presente Política de Privacidad. En <span style="color:#155724"> https://www.iocloudcomputing.io, </span> hay un sistema de captación de datos donde tratamos la información que nos facilitan las personas interesadas con la siguiente finalidad:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li><u>Formulario de contacto:</u> Solicitamos los siguientes datos personales: nombre, email, asunto y mensaje. Para contestar a los requerimientos de los usuarios de <span style="color:#155724">https://www.iocloudcomputing.io </span>utilizamos esos datos para responder a las solicitudes y responder a las dudas, quejas, comentarios, entre otros, que puedan suscitarles en lo que respecta a la información incluida en la página web, los servicios que se prestan a través de la página web, el tratamiento de sus datos personales, cuestiones referidas a los textos legales incluidos en la página web, así como cualquier otra consulta que pudieran tener y que no estén sujetas a condiciones de contratación.</li>
                    </ul>
                </ul>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Otras finalidades por las que tratamos sus datos personales son:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li><u>Para garantizar que se cumplen las condiciones de uso y la ley aplicable</u>. Esto puede incluir el desarrollo de herramientas y algoritmos que ayuden a nuestra página web a garantizar la confidencialidad de los datos de carácter personal que se recogen.</li>
                        <li><u>Apoyo y mejora de los servicios que ofrece nuestra página</u>.</li>
                        <li><u>Se recogen otros datos no identificativos </u>obtenidos a través de algunas cookies que se descargan en el ordenador del usuario cuando navega en nuestra página web y que detallamos en nuestra Política de Cookies.</li>
                        <li><u>Gestión de redes sociales</u>. <span style="color:#155724">https://www.iocloudcomputing.io</span>, puede estar presente en las redes sociales. El tratamiento de los datos que se lleve a cabo de las personas que se hagan seguidoras en las redes sociales de las páginas oficiales de <span style="color:#155724">https://www.iocloudcomputing.io</span>, se regirá por este apartado, así como por las condiciones de uso, política de privacidad y normativas de acceso pertenecientes a la red social que proceda en cada supuesto y aceptadas de forma previa por el usuario de https://www.iocloudcomputing.io. Tratará sus datos con la finalidad de administrar de forma correcta su presencia en la red social, informando de actividades o servicios de <span style="color:#155724">https://www.iocloudcomputing.io</span> , así como cualquier otra finalidad que las normativas de las redes sociales permitan. No usaremos los perfiles de seguidores en redes sociales para el envío de publicidad de forma individual.</li>
                    </ul>
                </ul>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Atendiendo a lo dispuesto en el Reglamento (UE) 2016/679 del Parlamento Europeo y del Consejo de 27 de abril de 2016, <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, con domicilio en Calle Margarita, 18
                    Playa de la Garita (Telde) - Las Palmas, CP 35212, será responsable del tratamiento de los datos correspondientes a usuarios de la página web.
                    <span style="color:#155724">https://www.iocloudcomputing.io</span> , no vende, alquila ni cede datos personales que puedan identificar al usuario, ni lo hará en el futuro, a terceros sin el consentimiento previo. No obstante, en diversos casos pueden realizarse colaboraciones con otros profesionales, en tales casos, se requerirá el consentimiento de los usuarios informando sobre la identidad del colaborador y la finalidad de la colaboración, todo ello, bajo la más estricta seguridad. 
                </p>

                <p class="text-center" style="color:#000000">LEGITIMACIÓN PARA EL TRATAMIENTO DE TUS DATOS</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;La base legal para el tratamiento de sus datos es el consentimiento. Por lo tanto, para contactarnos o realizar comentarios en nuestra página web, se requiere el consentimiento y aceptación de esta política de privacidad.
                    <br /><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;La oferta de nuestros servicios se basa en el consentimiento que se solicita, así como también la contratación de nuestros servicios según los términos y condiciones que constan en la Política de contratación.
                    <br /><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El uso de sus datos está legitimado por la normativa de protección de datos europea y española de conformidad con las siguientes bases jurídicas:
                    <br /><br />
                    - Usted ha manifestado su consentimiento (se le ha presentado un formulario de consentimiento para que autorice el tratamiento de sus datos con determinadas finalidades; usted puede revocar el consentimiento prestado en todo momento);
                    <br /><br />
                    - El tratamiento de sus datos es necesario para la gestión y mantenimiento de un contrato suscrito con usted;
                    <br /><br />
                    - El tratamiento de sus datos es necesario para cumplir con nuestras obligaciones legales;
                    <br /><br />
                    - Utilizamos sus datos para conseguir un interés legítimo y nuestros motivos para hacerlo compensen los posibles perjuicios para sus derechos a la protección de sus datos;
                    <br /><br />
                    Puede haber finalidades que estén permitidas en virtud de otras bases jurídicas; en tales casos, haremos todo lo posible por identificar la base jurídica en cuestión y comunicársela lo antes posible en cuanto tengamos conocimiento de su existencia.
                </p>
              
                <p class="text-center" style="color:#000000">CATEGORÍA DE DATOS</p>

                <ul class="list-unstyled">
                    <ul>
                        <li><b>Datos identificativos:</b> nombre y correo electrónico.</li>
                        <li><b>Criterios de conservación de los datos:</b> se conservarán mientras no se solicite su supresión por parte del interesado.</li>
                    </ul>
                </ul>

                <p class="text-center" style="color:#000000">DESTINATARIOS A LOS QUE SE COMUNICARÁN TUS DATOS</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Google Analytics:</b> un servicio analítico de web prestado por Google, Inc., una compañía de Delaware cuya oficina principal está en 1600 Amphitheatre Parkway, Mountain View (California), CA 94043, Estados Unidos (“Google”). Google Analytics utiliza “cookies”, que son archivos de texto ubicados en tu ordenador, para ayudar a <span style="color:#155724">https://www.iocloudcomputing.io </span> a analizar el uso que hacen los usuarios del sitio web. La información que genera la cookie acerca de su uso de https://www.iocloudcomputing.io (incluyendo tu dirección IP) será directamente transmitida y archivada por Google en los servidores de Estados Unidos. 
                </p>

                <p class="text-center" style="color:#000000">DATOS DE NAVEGACIÓN</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cuando navegas por <span style="color:#155724">https://www.iocloudcomputing.io</span>, pueden recogerse datos no identificables, tales como direcciones de IP, ubicación geográfica (aproximadamente), un registro de cómo se utilizan nuestros servicios, así como otros datos que no pueden usarse para identificación del usuario. Entre los datos no identificativos, están también los relaciones a tus hábitos de navegación a través de servicios de terceros. Esta web utiliza los siguientes servicios de análisis de terceros:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li>Google Analytics.</li>
                    </ul>
                </ul>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Utilizamos esta información para analizar tendencias, administrar el sitio, rastrear los movimientos de los usuarios alrededor del sitio y para recopilar información demográfica sobre nuestra base de usuarios en conjunto.
                </p>

                <p class="text-center" style="color:#000000">SECRETO Y SEGURIDAD DE LOS DATOS. EXACTITUD Y VERACIDAD DE LOS DATOS</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="color:#155724">https://www.iocloudcomputing.io </span> se compromete en el uso y tratamiento de los datos incluyendo los datos personales de los usuarios, ello con el debido respeto a la confidencialidad y a usarlos conforme a la finalidad del mismo, así como se compromete a cumplir con su obligación de guardarlos y adaptar todas las medidas para evitar la alteración, pérdida, tratamiento o acceso no autorizado (como los protocolos https que utilizamos), ello atendiendo a la legislación vigente de protección de datos. <span style="color:#155724"> https://www.iocloudcomputing.io </span>, no puede garantizar la absoluta inexpugnabilidad de la red de Internet y, por ende, la violación de datos a través de accesos fraudulentos a ellos por parte de terceros. Como usuario en <span style="color:#155724">https://www.iocloudcomputing.io </span>, eres el responsable de la corrección y veracidad de los datos que proporciones a <span style="color:#155724">https://www.iocloudcomputing.io</span>, exonerando así a <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, de cualquier responsabilidad al respecto. Los usuarios garantizan y responden de la exactitud, vigencia y autenticidad de los datos personales que faciliten, así como se comprometen a mantenerlos actualizados de forma debida. El usuario acepta dar la información completa, así como correcta en el formulario de contacto dispuesto en nuestra página web.
                </p>

                <p class="text-center" style="color:#000000">ACEPTACIÓN Y CONSENTIMIENTO</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El usuario declara haber sido informado de las condiciones sobre protección de datos de carácter personal, aceptando y dando su consentimiento al tratamiento de los mismos por parte de <span style="color:#155724">INPUT OUTPUT CLOUD COMPUTING, S.L.</span>, en la forma y para las finalidades señaladas en esta política de privacidad.
                </p>

                <p class="text-center" style="color:#000000">MODIFICACIONES EN LA POLÍTICA DE PRIVACIDAD</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="color:#155724">https://www.iocloudcomputing.io</span>, se reserva el derecho a cambiar la política de privacidad para su adaptación a las novedades legislativas o jurisprudenciales, así como a prácticas de la industria. En tales casos, el Prestador anunciará en esta página las modificaciones hechas con antelación previa en lo que respecta a la puesta en práctica.
                </p>

                <p class="text-center" style="color:#000000">CORREOS INFORMATIVOS / COMERCIALES</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Conforme a la Ley 34/2002, de 11 de julio, de Servicios de la Sociedad de la Información y Comercio Electrónico (LSSICE), <span style="color:#155724">https://www.iocloudcomputing.io</span>, no realiza prácticas de SPAM, por lo que no envía correos comerciales por vía electrónica que no se hubiesen solicitado o autorizado de forma previa por el usuario. En el formulario dispuesto en la página web, el usuario tiene la opción de otorgar su consentimiento expreso para recibir el boletín, independientemente de la información comercial solicitada de forma puntual. Atendiendo a la Ley 34/2002 de Servicios de la Sociedad de la Información y de comercio electrónico, <span style="color:#155724">https://www.iocloudcomputing.io</span>, se compromete a no enviar comunicaciones de carácter comercial sin su debida autorización e identificación.
                </p>

                <p class="text-center" style="color:#000000">CONSULTAS SOBRE LA POLÍTICA DE PRIVACIDAD O SOBRE TUS DERECHOS</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Si tiene alguna cuestión, consulta o duda sobre la gestión de alguna información personal que haya proporcionado, puede contactarnos a través del correo electrónico indicado a continuación.
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li>Email: iocc@iocc.io</li>
                    </ul>
                </ul>

              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>

         <!-- Modal Cookies -->
        <div class="modal fade" id="cookies" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
          <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLongTitle3">Política de Cookies</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body" style="font-size: 14px;">
              
                  <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;En <span style="color:#155724">https://www.iocloudcomputing.io </span> utilizamos cookies con el objetivo de prestar un mejor servicio y proporcionarle una mejor experiencia en lu navegación. Queremos informarle de manera clara y precisa sobre las cookies que utilizamos, detallando a continuación, que es una cookie, para qué sirve, qué tipos de cookies utilizamos, cuáles son su finalidad y cómo puede configurarlas o deshabilitarlas si así lo desea.
                  </p>

                <p class="text-center" style="color:#000000">¿QUÉ ES UNA COOKIE Y PARA QUÉ SIRVE?</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Una "Cookie" es un pequeño archivo que se almacena en el ordenador del usuario, tablet, smartphone o cualquier otro dispositivo con información sobre la navegación.
                    El conjunto de "cookies" de todos nuestros usuarios nos ayuda a mejorar la calidad de nuestra web, permitiéndonos controlar qué páginas son útiles, cuáles no y cuáles son susceptibles de mejora.
                    Las cookies son esenciales para el funcionamiento de internet, aportando innumerables ventajas en la prestación de servicios interactivos, facilitándote la navegación y usabilidad de nuestra web. En ningún caso las cookies podrían dañar tu equipo. Por contra, el que estén activas nos ayuda a identificar y resolver los errores.
                </p>

                <p class="text-center" style="color:#000000">¿QUÉ TIPOS DE COOKIES UTILIZAMOS?</p>

                <p class="text-left" style="color:#000000">SEGÚN LA ENTIDAD QUE LA GESTIONA</p>

                <ul class="list-unstyled">
                    <ul>
                        <li><b>Cookies propias:</b> Son aquellas que se envían a tu equipo desde nuestros propios equipos o dominios y desde el que prestamos el servicio que nos solicitas.</li>
                    </ul>
                </ul>

                <p class="text-left" style="color:#000000">SEGÚN EL PLAZO DE TIEMPO QUE PERMANECEN ACTIVADAS</p>

                <ul class="list-unstyled">
                    <ul>
                        <li><b>Cookies de sesión:</b> Son cookies temporales que permanecen en el archivo de cookies de tu navegador hasta que abandonas la página web, por lo que ninguna queda registrada en el disco duro de tu ordenador. La información obtenida por medio de estas cookies, sirven para analizar pautas de tráfico en la web. A la larga, esto nos permite proporcionar una mejor experiencia para mejorar el contenido y facilitar su uso.</li>
                        <li><b>Cookies persistentes:</b> Son almacenadas en el disco duro y nuestra web las lee cada vez que realizas una nueva visita. Una web permanente posee una fecha de expiración determinada. La cookie dejará de funcionar después de esa fecha. Estas cookies las utilizamos, generalmente, para facilitar los servicios de compra y registro.</li>
                    </ul>
                </ul>

                <p class="text-left" style="color:#000000">SEGÚN SU FINALIDAD</p>

                <ul class="list-unstyled">
                    <ul>
                        <li><b>Cookies técnicas:</b> Son aquellas necesarias para la navegación y el buen funcionamiento de nuestra página web. Permiten por ejemplo, controlar el tráfico y la comunicación de datos, acceder a partes de acceso restringido, realizar el proceso de compra de un pedido, utilizar elementos de seguridad, almacenar contenidos para poder difundir vídeos o compartir contenidos a través de redes sociales.</li>
                        <li><b>Cookies de personalización:</b> Son aquéllas que te permiten acceder al servicio con unas características predefinidas en función de una serie de criterios, como por ejemplo el idioma, el tipo de navegador a través del cual se accede al servicio, la configuración regional desde donde se accede al servicio, etc.</li>
                        <li><b>Cookies de análisis:</b> Son aquéllas que nos permiten cuantificar el número de usuarios y así realizar la medición y análisis estadístico de la utilización que hacen los usuarios de los servicios prestados. Para ello se analiza su navegación en nuestra página web con el fin de mejorar la oferta de productos y servicios que ofrecemos.</li>
                        <li><b>Cookies publicitarias:</b> Son aquéllas que permiten la gestión, de la forma más eficaz posible, de los espacios publicitarios que se pudieran incluir en nuestra página web.</li>
                        <li><b>Cookies de publicidad:</b> comportamental: Estas cookies almacenan información del comportamiento de los usuarios obtenida a través de la observación continuada. Gracias a ellas, podemos conocer los hábitos de navegación en internet y mostrarte publicidad relacionada con tu perfil de navegación.</li>
                    </ul>
                </ul>

                <p class="text-center" style="color:#000000">¿CÓMO PUEDES CONFIGURAR O DESHABILITAR TUS COOKIES?</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Puedes permitir, bloquear o eliminar las cookies instaladas en su equipo mediante la configuración de las opciones de tu navegador de Internet. En caso de que no permitas la instalación de cookies en tu navegador es posible que no puedas acceder a algunos de los servicios y que tu experiencia en nuestra web pueda resultar menos satisfactoria. En los siguientes enlaces tienes a tu disposición toda la información para configurar o deshabilitar tus cookies en cada navegador:
                </p>

                <ul class="list-unstyled">
                    <ul>
                        <li><a href="https://support.google.com/chrome/answer/95647?hl=es" target ="_blank">Google Chrome</a></li>
                        <li><a href="https://support.mozilla.org/es/kb/habilitar-y-deshabilitar-cookies-sitios-web-rastrear-preferencias?redirectlocale=es&redirectslug=habilitar-y-deshabilitar-cookies-que-los-sitios-we" target ="_blank">Mozilla Firefox</a></li>
                        <li><a href="https://support.microsoft.com/en-us/help/17442/windows-internet-explorer-delete-manage-cookies" target ="_blank">Internet Explorer</a></li>
                        <li><a href="https://support.apple.com/es-es/guide/safari/sfri11471/mac" target ="_blank">Safari</a></li>
                        <li><a href="https://support.apple.com/en-us/HT201265" target ="_blank">Safari para IOS (iPhone y iPad)</a></li>
                        <li><a href="https://support.google.com/chrome/answer/114662?hl=es&visit_id=636771911824076788-731088213&rd=1" target ="_blank">Chrome para Android</a></li>
                    </ul>
                </ul>
          
                <p class="text-center" style="color:#000000">CONSENTIMIENTO</p>

                <p class="text-justify">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;En <span style="color:#155724">https://www.iocloudcomputing.io </span> nunca guardamos los datos personales de nuestros usuarios, a excepción de la dirección IP de acuerdo a lo descrito anteriormente, salvo que quieras registrarte, de forma voluntaria con el fin de realizar compras de los productos y servicios que ponemos a tu disposición o de recibir información sobre promociones y contenidos de tu interés.
                    Al navegar y continuar en nuestra web nos indicas que estás consintiendo el uso de las cookies antes enunciadas, y en las condiciones contenidas en la presente Política de Cookies.
                </p>

              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>--%>

        <!-- Modal Login -->
        <div class="modal" id="modal_login" tabindex="-1" role="dialog" data-bs-backdrop="static" >
        <div class="modal-dialog">

	        <div class="modal-content">
                
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Identifícate</h1>
                </div>
                <div class="modal-body">

                    <table border="0" style="width:100%;">
                    <tr>
                        <td align="center"><label><span class="text-muted" style="font-size: 15px;">¡Vaya!, ¿Quién eres?.</span> </label></td>
                    </tr>
                    </table>
                    <p></p>
                    <div class="input-group mb-3">
                        <span class="input-group-text">
                            <i class="bi bi-lock-fill" style="font-size:20px;"></i>
                        </span>
                        <asp:TextBox type="password" ID="txt_contrasena" class="form-control input-sm" runat="server" placeholder="Contraseña" TextMode="SingleLine"></asp:TextBox>
                        <span class="input-group-text">
                            <asp:LinkButton ID="lk_login" runat="server" class="input-sm text-primary" Font-Size="16" title="Recuperar"><i class ="bi bi-box-arrow-in-right"></i></asp:LinkButton>
                        </span> 
                    </div>

                </div>
                
            </div>
        </div>
        </div>

    </form>
</body>
</html>