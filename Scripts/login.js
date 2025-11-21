$(document).ready(function () {

    //Muetra el reloj en la barra
    setInterval('digiClock()', 1000);

    //-------------------------------------------------------------------------------------------------------------------
    //Control para capaque oscurece el video
    colors = ['0.40', '0.45', '0.50', '0.55', '0.60', '0.65', '0.70', '0.75'];
    var i = 0;
    animate_loop = function () {
        $('.transparent_negro ').animate({
            opacity: colors[(i++) % colors.length]
        }, 3000, function () {
            animate_loop();
        });
            }
    animate_loop();

    //Eliminamos el click derecho de ratón para las multiples pantallas
    $('body').bind("contextmenu", function (e) {
        return false;
    });

    //Aparece fecha y hora
    $("#clock").hide().fadeIn(3000);
    $("#lbl_fecha").hide().fadeIn(7000);

    //Asigno para conocer el tipo de dispositivo
    $('#txt_tipo_dispositivo').val(device.mobile() + "|" + device.tablet());

    //-------------------------------------------------------------------------------------------------------------------
    //Control Click sobre Ojo
    $('#ver_pass').on('click', function () {

        $('#txt_contrasena').attr('type', function (index, attr) {

            if (attr == 'text') {
                $("#ver_pass").attr("class", "bi bi-eye text-primary")
                $("#ver_pass").attr("title", "Ver Password")
                return 'password'
            } else {
                $("#ver_pass").attr("class", "bi bi-eye-slash text-primary")
                $("#ver_pass").attr("title", "Ver Password")
                return 'text'
            };

        })

    });

    //-------------------------------------------------------------------------------------------------------------------
    //Auto Click
    $("#lk_login").on('focus', function (e) {
        //click sobre grabar cabecera
        $("#lk_login")[0].click();
    });

    //-------------------------------------------------------------------------------------------------------------------
    //Control de click
    $("#lk_login").on('click', function (e) {

        if ($("#txt_email").val() == '') {
            error('El campo Correo Electrónico no puede estar vacío.')
            setTimeout(function () { $('#txt_email').focus(); }, 100);
            return false;
        }

        if ($("#txt_contrasena").val() == '') {
            error('El campo Contraseña no puede estar vacío.')
            setTimeout(function () {$('#txt_contrasena').focus(); }, 100);
            return false;
        }

    })

    //Informo de resolución de pantalla baja, para PC
    if (device.mobile() === false && device.tablet() === false) {

        if ($(window).width() < 1280) {
            error("La resolución de tu monitor es muy baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.", 20);
            hablar("La resolución de tu monitor es muy baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.")
        }
        if ($(window).width() >= 1280 && $(window).width() < 1440) {
            advertencia("La resolución de tu monitor es baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.", 20);
            hablar("La resolución de tu monitor es baja y muchos de los menús no aparecerán correctamente en tu barra de tareas.")
        }
    }

    //Grafico para las 3 versiones
    if (device.mobile()) {
        $("#img_dispositivo").addClass("bi bi-phone")
        $("#img_dispositivo").attr("title", "Modo Móvil")
    }
    if (device.tablet()) {
        $("#img_dispositivo").addClass("bi bi-tablet-landscape")
        $("#img_dispositivo").attr("title", "Modo Tablet")
    }
    if (!device.mobile() && !device.tablet()) {
        $("#img_dispositivo").addClass("bi bi-laptop")
        $("#img_dispositivo").attr("title", "Modo Escritorio")
    }

    //Filtro para la tabla
    (function ($) {
        //$("#gridview_empresa tbody").addClass("search");
        $('#filter').keyup(function () {

            var rex = new RegExp($(this).val(), 'i');
            // var $t = $(this).children(":eq(4))");
            $('.table  tr ').hide();

            //Recusively filter the jquery object to get results.
            $('.table  tr ').filter(function (i, v) {
                //Get the 3rd column object here which is userNamecolumn
                var $t = $(this).children(":eq(" + "3" + ")");
                var $t2 = $(this).children(":eq(" + "2" + ")");
                if (rex.test($t.text()) == '') {
                    return rex.test($t2.text());
                }
                return rex.test($t.text());
            }).show();
        })

    }(jQuery));

});

//Muestra el reloj en la barra
function digiClock() {
    var crTime = new Date();
    var crHrs = crTime.getHours();
    var crMns = crTime.getMinutes();
    var crScs = crTime.getSeconds();
    crMns = (crMns < 10 ? "0" : "") + crMns;
    crScs = (crScs < 10 ? "0" : "") + crScs;
    var timeOfDay = (crHrs < 12) ? "AM" : "PM";
            crHrs = (crHrs > 12) ? crHrs - 12 : crHrs;
    crHrs = (crHrs == 0) ? 12 : crHrs;
    var crTimeString = crHrs + ":" + crMns + " " + timeOfDay;
    $("#clock").html(crTimeString);
}

//Funcion de habla
function hablar(texto) {
    var msg = new SpeechSynthesisUtterance(texto);
    msg.rate = 1.2; // 0.1 to 10
    msg.pitch = 1; //0 to 2
    window.speechSynthesis.speak(msg);
}

//Notificación de Correcto
function ok(mensaje) {
    alertify.success(mensaje);
    return false;
}

//Notificación de Error
function error(mensaje) {
    alertify.error(mensaje);
    hablar(mensaje);
    return false;
}

//Notificación de Advertencia
function advertencia(mensaje, retraso) {
    if (retraso != '') {
        alertify.set({ delay: retraso * 1000 });
    };
    alertify.log(mensaje);
    return false;
}