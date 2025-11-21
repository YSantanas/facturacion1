<%@ Page Language="VB" AutoEventWireup="false" CodeFile="noticiario.aspx.vb" Inherits="actualizaciones_noticiario" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="refresh" content="5" />

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

    <style>

        body{
            background-color: white;
            font-size: 14px;
        }
        
        .cambiar_verde:hover {
            box-shadow: 0px 0px 3px 3px rgba(40,167,69,0.5);
            -webkit-box-shadow: 0px 0px 3px 3px rgba(40,167,69,0.5);
            -moz-box-shadow: 0px 0px 3px 3px rgba(40,167,69,0.5);
            border: 1px solid #28a745;
            cursor: pointer;
        }

        .cambiar_naranja:hover {
            box-shadow: 0px 0px 3px 3px rgba(255,193,7,0.5);
            -webkit-box-shadow: 0px 0px 3px 3px rgba(255,193,7,0.5);
            -moz-box-shadow: 0px 0px 3px 3px rgba(255,193,7,0.5);
            border: 1px solid #ffc107;
            cursor: pointer;
        }

        .cambiar_rojo:hover {
            box-shadow: 0px 0px 3px 3px rgba(220,53,69,0.5);
            -webkit-box-shadow: 0px 0px 3px 3px rgba(220,53,69,0.5);
            -moz-box-shadow: 0px 0px 3px 3px rgba(220,53,69,0.5);
            border: 1px solid #dc3545;
            cursor: pointer;
        }

    </style>

    <script>

        $(document).ready(function () {

            //Eliminamos el click derecho de ratón para las multiples pantallas
            $('html').bind("contextmenu", function (e) {
                return false;
            });
                     
            //Oculto todos los elementos
            $(".oculto").hide()

            //Control para el click
            $(".desplegable").on('click', function (e) {

                //Efecto
                $("#capa" + this.id).toggle(100);
                
            });

        });

        //Cambia el puntero del raton por una mano de enlace
        function hand(idDiv) {
            $("#" + idDiv).css({ 'cursor': 'pointer' });
        }

        ////Notificación de actualizacion de direccion en Notificaciones
        //function operacion() {
        //    //ataco a la operacion de cerrar la session
        //    window.parent.cerrar_session();
        //};

        ////Notificación de actualizacion de direccion en Notificaciones
        //function operacion2(texto) {
        //    //ataco a la operacion abrir visualizados
        //    window.parent.abrir_ventana('visualizador', 'plagiarism', '560', '315', 'actualizaciones/visualizador.aspx?id=' + texto + '', '11');
        //};

        ////Notificación de actualizacion de direccion en Notificaciones
        //function operacion3(texto) {

        //    //Leo los valores parametros_empresa
        //    $.ajax({
        //        async: false,
        //        type: "POST",
        //        url: "../default.aspx/crear_session",
        //        data: '{nombre_sesion: "numero_conciliacion",parametros:"' + texto + '"}',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (datos) {

        //            //alert(datos.d)
                    
        //        }
        //    });

        //    //Refrecamos
        //    $('#iconciliacion', window.parent.document).attr('src', $('#iconciliacion', window.parent.document).attr('src'));
        //    //Abrimos
        //    window.parent.abrir_ventana_relacional('conciliacion', 'task', '1000', '600', 'asientos/conciliacion.aspx', '1');
            
        //};

        ////Notificación para los Modelos Oficiales
        //function operacion4() {
        //    //ataco a la operacion abrir Modelos Oficiales
        //    window.parent.abrir_ventana_relacional('Modelos_oficiales', 'Agreement-01.png', '900', '500', 'configuracion/modelos_oficiales.aspx', '8');
        //    padre = $(window.parent.document);
        //    $(padre).contents().find("#iModelos_oficiales").contents().find("#btn_consultar").click(); 
        //};

        ////Notificación de actualizar plan de cuentas
        //function operacion5(texto) {

        //    $.ajax({
        //        async:false,
        //        type: "POST",
        //        url: "../default.aspx/tabla_cuentas",
        //        data: '{bbdd: "' + texto + '"}',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (datos) {

        //            //Actualizar cuenta
        //            $('#modal_eliminar').modal('show');

        //        }
        //    });
            
        //};

    </script>
   
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="Lt_noticiario" runat="server" ></asp:Literal>
    </div>

    <!-- Modal Eliminar Asiento-->
    <div class="modal fade" id="modal_eliminar" tabindex="-1" role="dialog" aria-labelledby="modal_generarLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modal_titulo_asiento_impuesto">Cambio de Cuenta</h4>
                </div>
                <div class="modal-body">
                    Has actualizado el Plan de Cuentas con los nuevos cambios.
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>

    </form>
</body>
</html>
