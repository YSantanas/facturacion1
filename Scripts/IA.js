//Introduccion de bienvenida
var tiempo = new Date();
var hora = tiempo.getHours();
var resultado = ''

switch (hora) {
    case 1:
    case 2:
    case 3:
    case 4:
    case 5:
    case 6:
    case 7:
    case 8:
    case 9:
    case 10:
    case 11:
    case 12: {
        resultado = "buenos dias"
        break;
    }
    case 13:
    case 14:
    case 15:
    case 16:
    case 17:
    case 18:
    case 19:
    case 20: {
        resultado = "buenas tardes"
        break;
    }
    case 21:
    case 22:
    case 23:
    case 00: {
        resultado = "buenas noches"
        break;
    }
}

var p_usuario = "";
var p_empresa = "";

$(document).ready(function () {

    $.ajax({
        async: false,
        type: "POST",
        url: "default.aspx/leer_sesion",
        data: '{nombre_sesion: "f_' + $("#txt_id_usuario").val() + '_tabla_usuario"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Cargo los valores traidos
            p_usuario = (data.d).split("|");

            //Mensaje        
            hablar(resultado + " " + p_usuario[1])

        }

    });

    $.ajax({
        async: false,
        type: "POST",
        url: "default.aspx/leer_sesion",
        data: '{nombre_sesion: "f_' + $("#txt_id_empresa").val() + '_tabla_empresa"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Cargo los valores traidos
            var p_empresa = (data.d).split("|");

            //miro si tiene o no notificaciones
            mirar_notificacion();
        
        }
    });

function mirar_notificacion() {
    
    $.ajax({
        async: false,
        type: "POST",
        url: "default.aspx/numero_notificacion",
        data: '{bbdd: "' + p_empresa[37]  + '",id_usuario: "' + p_usuario[0] + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: grabado,
        error: errores
    });

    function grabado(msg) {
        
        //Descompongo los valores
        var vector_valores = msg.d.split("|");
        
        //Si viene a 0
        if (vector_valores[0] != 0) {

            var mensaje;

            if (vector_valores[0] == 1) {

                if (vector_valores[2] != 0) {

                    //mensaje
                    mensaje = "Tienes " + vector_valores[0] + " Notificacion, que necesita tu intervención."

                        if ($("#notificaciones-charm").is(":visible") == false) {

                            $("#notificaciones").css({ height: ($(document).height() - 300) + 'px' });
                            $("#notificaciones").attr('src', 'actualizaciones/noticiario.aspx?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val())
                            $("#notificaciones_claudia").attr('src', 'actualizaciones/claudia.aspx?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val())
                            $('#notificaciones-charm').toggle();

                        }

                } else {

                    //mensaje
                    mensaje = "Tienes " + vector_valores[0] + " Notificacion."

                }

            } else {

                if (vector_valores[2] != 0) {

                    //mensaje
                    mensaje = "Tienes " + vector_valores[0] + " Notificaciones, de las cuales alguna de ellas necesitan de tu intervención."

                        if ($("#notificaciones-charm").is(":visible") == false) {

                            $("#notificaciones").css({ height: ($(document).height() - 300) + 'px' });
                            $("#notificaciones").attr('src', 'actualizaciones/noticiario.aspx?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val())
                            $("#notificaciones_claudia").attr('src', 'actualizaciones/claudia.aspx?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val())
                            $('#notificaciones-charm').toggle();

                        }

                } else {

                    //mensaje
                    mensaje = "Tienes " + vector_valores[0] + " Notificaciones."

                }

            }

            //Asigno
            $("#lbl_n_notificaciones").empty()
            $("#lbl_n_notificaciones").append(vector_valores[0])

            //componente para hablar
            hablar(mensaje)

        } else {

            $("#lbl_n_notificaciones").empty()
        
        }
        
        //Si la llave es vacia
        if (vector_valores[1] == '') {
            error("Un operario está efectuando labores de mantenimiento.")
            setTimeout(
                function () {
                    //Lo que debe pasar pasados 10 segundos (10mil milisegundos)
                    cerrar_session()
                }, 10000);
        } 
       
    };

    function errores(msg) {
        //msg.responseText tiene el mensaje de error enviado por el servidor
        alert('Error: ' + msg.responseText);
    };

    //Repetimos cada X segundo
    setTimeout(mirar_notificacion, 60000);
}

});