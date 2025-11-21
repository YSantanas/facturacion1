var p_usuario = "";
var p_empresa = "";

$(document).ready(function () {
    
    //CONTROL DE TECLAS RAPIDAS Escritorios Avanza
    shortcut.add("F4", function () {

        $("#full_pantalla").click();

    });

    //CONTROL DE TECLAS RAPIDAS Limpia Escritorios
    shortcut.add("F5", function () {

        $("#img_limpia_pantalla").click();

    });

    //CONTROL DE TECLAS RAPIDAS Notificaciones
    shortcut.add("F8", function () {

        $("#img_notificaciones").click();

    });
    
    //Eliminamos el click derecho de ratón para las multiples pantallas
    $('html').bind("contextmenu", function (e) {
        return false;
    });
    
    //Leo la sesion de los parametros de usuario, parametros empresa y creo un array general para el control de pantallas
    $.ajax({
        async: false,
        type: "POST",
        url: "default.aspx/leer_sesion",
        data: '{nombre_sesion: "f_' + $("#txt_id_usuario").val() + '_tabla_usuario"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            
            //Cargo los valores traidos
            p_usuario = data.d.split("|");

            //Cargo Transparencias y color barra inferior
            $(".transparente").css({ 'opacity': p_usuario[10] / 100, 'filter': "'alpha(opacity = " + p_usuario[10] + ")'" });
            $(".color").css({ 'background-color': p_usuario[11] });

            //Aparece Menu Inicio
            $("#menu").fadeIn(900).show();
            $("#menu_superior").fadeIn(900).show();

            var alto = ($(window).height() * 75) / 100;
            $("#barra_menu").css({ 'left': '10px', 'height': alto + 'px' });
            alto = alto - 65 //alto de los 3 menus
            $("#central_inferior").css({ 'height': alto + 'px' });

            //Variables de Sonido
            var valor_volumen = parseFloat(p_usuario[30].replace(",", "."));
            var valor_imagen = '';

            if (valor_volumen === 0) { valor_imagen = "<i class='bi bi-volume-mute' style='font-size:20px;'></i>"; }
            if (valor_volumen > 0 && valor_volumen <= 0.50) { valor_imagen = "<i class='bi bi-volume-down' style='font-size:20px;'></i>"; }
            if (valor_volumen > 0.50) { valor_imagen = "<i class='bi-volume-up' style='font-size:20px;'></i>"; }
            $("#imagen_sonido").attr({ "title": "Volumen: " + parseInt(valor_volumen * 100) });
            $("#imagen_sonido").append(valor_imagen);

            //Informacion Usuario
            $("#lblnombre").text(p_usuario[1] + " " + p_usuario[2] + " " + p_usuario[3]);
            var texto_nombre = p_usuario[1] + " " + p_usuario[2] + " " + p_usuario[3]
            if (texto_nombre.length > 10) {
                $("#lblnombre2").text(texto_nombre.substring(0, 10) + '...');
            } else {
                $("#lblnombre2").text(texto_nombre);
            }

            //Informacion Nivel
            $("#lblnivel").text(p_usuario[8]);

            //Volumen,velocidad y tono
            $('#txt_volumen').val(parseFloat(p_usuario[30].replace(",", ".")) * 100);
            $('#lbl_volumen').text('Volumen: ' + parseFloat(p_usuario[30].replace(",", ".")) * 100);
            $('#txt_tono').val(parseFloat(p_usuario[29].replace(",", ".")) * 10);
            $('#lbl_tono').text('Tono: ' + parseFloat(p_usuario[29].replace(",", ".")) * 10);
            $('#txt_velocidad').val(parseFloat(p_usuario[28].replace(",", ".")) * 10);
            $('#lbl_velocidad').text('Velocidad: ' + parseFloat(p_usuario[28].replace(",", ".")) * 10);

        }
    });
       
    //var parametros_empresa;
    $.ajax({
        async: false,
        type: "POST",
        url: "default.aspx/leer_sesion",
        data: '{nombre_sesion: "f_' + $("#txt_id_empresa").val() + '_tabla_empresa"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Cargo los valores traidos
            p_empresa = data.d.split("|");

            //Controlo Aparición de Inmovilizado
            if (p_empresa[55] === "Si") {
                $('#Ph_Inmovilizado').css({ 'visibility': 'visible', 'display': 'inline' });
                $('#Ph_Inmovilizado2').css({ 'visibility': 'visible', 'display': 'inline' });
            }

            //        //Controlo Aparición de Cartera
            //        if (parametros_empresa[14] === "Si") {
            //            $('#Ph_Cartera').css({ 'visibility': 'visible', 'display': 'inline' });
            //        }

            //Controlo Aparición de Gestion Documental
            if (p_empresa[73] === "Si") {
                $('#Ph_Gestion_documental').css({ 'visibility': 'visible', 'display': 'inline' });
            }

    //        //Controlo Aparición de Nóminas de Trabajadores
    //        if (parametros_empresa[52] === "Si") {
    //            $('#Ph_nominas_trabajadores').css({ 'visibility': 'visible', 'display': 'inline' });
    //        }

            //Menu Información
            //Informacion de la empresa
            $("#lblcodigo").text(p_empresa[5]);
            $("#lbl_informacion_extra").text('Empresa: ' + p_empresa[5] + ' - ' + p_empresa[6] + ' (' + p_empresa[37] + ')');
            $("#lblempresa").text(p_empresa[6]);
            $("#lblnif").text(p_empresa[10]);
            $("#lbltelefono").text(p_empresa[8]);
            $("#lblemail").text(p_empresa[9]);

            //Informacion BBDD
            $("#lblbbdd").text(p_empresa[37]);
                        
        }
    });
    
    //Tamaño para el contenedor
    $("#body_contenedor_desktop").css({ height: $("#body_contenedor").height() - 100 + 'px', top: '50px' });

    //Click en Notificaciones
    $("#img_notificaciones").on('click', function () {
        //Controlo que la ruta Venga o no y la asigno 
        var src = $("#notificaciones").attr('src');
        if (src.indexOf('vacia.html') != -1) {
            $("#notificaciones").attr('src', 'actualizaciones/noticiario.aspx?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val());
            $("#notificaciones_claudia").attr('src', 'actualizaciones/claudia.aspx?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val());
            hablar("Notificaciones.");
        } else {
            $("#notificaciones").attr('src', 'actualizaciones/vacia.html');
            $("#notificaciones_claudia").attr('src', 'actualizaciones/vacia.html');
        }

        $("#notificaciones").css({ height: ($("#body_contenedor").height() - 300) + 'px' });
        $('#notificaciones-charm').toggle();
    });

    //Click en Sonido
    $("#imagen_sonido").on('click', function () {
        //Activo a claudia
        hablar("Configuración de sonido.");
        //Activo preguntar
        $('#modal_sonido').modal('show');
    });

    //Cambio volumen
    $('#txt_volumen').on('change', function (e) {

        //Cambio el valor
        $('#lbl_volumen').text('Volumen: ' + $('#txt_volumen').val());
        $("#imagen_sonido").attr({ "title": "Volumen: " + $("#txt_volumen").val() });

        //Calculo        
        var valor_volumen = parseFloat($("#txt_volumen").val() / 100);

        if (valor_volumen === 0) { valor_imagen = "<i class='bi bi-volume-mute' style='font-size:20px;'></i>"; }
        if (valor_volumen > 0 && valor_volumen <= 0.50) { valor_imagen = "<i class='bi bi-volume-down' style='font-size:20px;'></i>"; }
        if (valor_volumen > 0.50) { valor_imagen = "<i class='bi-volume-up' style='font-size:20px;'></i>"; }
        $("#imagen_sonido").attr({ "title": "Volumen: " + parseInt(valor_volumen * 100) });
        $("#imagen_sonido").empty();
        $("#imagen_sonido").append(valor_imagen);

        //Lanzo la funcion
        cambiar_sonido();

        //Hablar
        hablar("Prueba de volumen de voz.");

    });
   
    //Cambio velocidad
    $('#txt_tono').on('change', function (e) {

        //cambio el valor
        $('#lbl_tono').text('Tono: ' + $('#txt_tono').val());

        //Lanzo la funcion
        cambiar_sonido();

        //Hablar
        hablar("Prueba del tono de voz.");

    });

    //Cambio velocidad
    $('#txt_velocidad').on('change', function (e) {

        //cambio el valor
        $('#lbl_velocidad').text('Velocidad: ' + $('#txt_velocidad').val());

        //Lanzo la funcion
        cambiar_sonido();

        //Hablar
        hablar("Prueba de velocidad de voz.");

    });

    //Despliego el menu de limpiar pantallas
    $("#img_limpia_pantalla").click(function () {

        //Activo a claudia
        hablar("Limpiar Ventana.");
        //Asigno
        var ruta = "actualizaciones/vacia.html"
        $("#icontenido").attr('src', ruta);
        $("#icontenido").css({ 'visibility': 'hidden' });

    });

    //Click en Informacion
    $("#img_informacion").on('click', function () {
        //Activo a claudia
        hablar("Información de la empresa.");
        //Activo preguntar
        $('#modal_informacion').modal('show');
    });
    
    //Click en Contacto
    $("#img_contacto_superior").on('click', function () {
        //Activo a claudia
        hablar("Contacto.");
        //Activo preguntar
        $('#modal_contacto').modal('show');
    });

    //Despliego el menu de inicio
    $("#img_inicio").click(function () {
        if ($('#barra_menu').is(":visible")) {
            $("#barra_menu").fadeOut(200);
        } else {
            $("#barra_menu").fadeIn(200);
        }
    });

    //Click en Whatssap
    $("#img_salir").on('click', function () {
        //Salgo de Pantalla Completa
        exitFullScreen();
        //Activo a claudia
        hablar("¿Está seguro de querer cerrar la aplicación?");
        //Activo preguntar
        $('#modal_salir').modal('show');
    });

    //Click en Cerrar
    $("#btn_cerrar").on('click', function () {
        //Cerrar
        cerrar_session()
    });
    
    //Ampliar tiempo
    $("#btn_ampliar_session").click(function () {
        //Grabo el nuevo valor
        createCookie("Time_Session", 20, 365);
        //Cerrar PopUp
        $('#modal_inactividad').modal('hide');
    });

    //Muetra el reloj en la barra
    setInterval('digiClock()', 1000);

});

//------------------FUNCIONES GENERALES------------------------------------
//Tratamiento con las Cookies
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {

    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

//Notificación de Error
function error(mensaje) {
    alertify.error(mensaje);
    hablar(mensaje);
    return false;
}

//Notificación de Correcto
function ok(mensaje) {
    alertify.success(mensaje);
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

//Cambia el puntero del raton por una mano de enlace
function hand(idDiv) {
    $("#" + idDiv).css({ 'cursor': 'pointer' });
}

//Funciones para maximizar y minimizar la pantalla
function isFullScreen() {

    return (document.fullScreenElement && document.fullScreenElement !== null)
        || document.mozFullScreen
        || document.webkitIsFullScreen;
}

function requestFullScreen(element) {
    if (element.requestFullscreen)
        element.requestFullscreen();
    else if (element.msRequestFullscreen)
        element.msRequestFullscreen();
    else if (element.mozRequestFullScreen)
        element.mozRequestFullScreen();
    else if (element.webkitRequestFullscreen)
        element.webkitRequestFullscreen();
    //Activo a Claudia
    hablar("Pantalla completa.");
}

function exitFullScreen() {
    if (document.exitFullscreen)
        document.exitFullscreen();
    else if (document.msExitFullscreen)
        document.msExitFullscreen();
    else if (document.mozCancelFullScreen)
        document.mozCancelFullScreen();
    else if (document.webkitExitFullscreen)
        document.webkitExitFullscreen();
}

function toggleFullScreen(element) {

    if (isFullScreen()) {
        exitFullScreen();
    } else {
        requestFullScreen(element || document.documentElement);
    }

}

function cambiar_sonido() {

    //Cambio los parametros de usuario
    p_usuario[30] = ($("#txt_volumen").val() / 100).toString().replace(",", ".");
    p_usuario[29] = ($("#txt_tono").val() / 10).toString().replace(",", ".");
    p_usuario[28] = ($("#txt_velocidad").val() / 10).toString().replace(",", ".");

    //Valores a enviar
    var valores = $("#txt_volumen").val() + "|" + $("#txt_tono").val() + "|" + $("#txt_velocidad").val();

    $.ajax({
        async: false,
        type: "POST",
        url: "default.aspx/Json_sonido",
        data: '{id_usuario: "' + $("#txt_id_usuario").val() + '", valores: "' + valores + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Ninguna acción

        },
        error: function errores(msg) { alert('Error: ' + msg.responseText); }
    });

}

function cerrar_session() {

    //-------------------------------------------------------------------------------------------------------------------
    $.ajax({
        type: "POST",
        url: "default.aspx/cerrar_sesion",
        data: '{id_usuario: "' + $("#txt_id_usuario").val() + '",id_empresa: "' + $("#txt_id_empresa").val() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: cerrando,
        error: errores
    });

    function cerrando(msg) {

        //redirijo
        window.location = "login.aspx"
    };

    function errores(msg) {
        //msg.responseText tiene el mensaje de error enviado por el servidor
        alert('Error: ' + msg.responseText);
    };

}

function abrir_ventana(idDiv, imagen, ancho, alto, ruta, grupo) {

    //Declaro
    var ruta2 = '';

    //Actualizar la url para las distintas partes de los menús
    if (ruta.indexOf("|") != -1) {

        //Extraigo el texto a quitar
        var texto = ruta.substr(ruta.indexOf("|"))

        //Quito el texto de la URL
        ruta = ruta.replace(texto, "")

        //Asigno los valores nuevos
        ruta2 = ruta + '?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val() + texto.replace("|", "&");

    } else {

        //Asigno
        ruta2 = ruta + '?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val();

    }

    //Asigno
    $("#iframe").attr('src', ruta2);

    //Ajusto el Iframe al tamaño de la ventana general
    $("#icontenido").css({'visibility': 'visible'});

    //Oculto la barra al abrir la ventana    
    $("#barra_menu").hide(800);

}

function abrir_ventana_relacional(idDiv, imagen, ancho, alto, ruta, grupo) {
   
    abrir_ventana(idDiv, imagen, ancho, alto, ruta, grupo)

}

function hablar(texto) {

    //Si es distinto de 0 es que tiene volumen
    if (p_usuario[30] != "0,00") {

        var msg = new SpeechSynthesisUtterance(texto);
        var voices = window.speechSynthesis.getVoices();
        msg.rate = p_usuario[28].replace(",", "."); // 0.1 to 10
        msg.pitch = p_usuario[29].replace(",", "."); //0 to 2
        msg.volume = p_usuario[30].replace(",", "."); //0 to 1

        window.speechSynthesis.speak(msg);

    }; 
       
}

function lessMinutes() {

    //cargo el valor de la cookie
    var valor_inicial = readCookie("Time_Session")
    
    //Si quedan 2 minutos de inactividad
    if (valor_inicial - 1 == 2) {
        alertify.warning("Su sesión caducará en 2 minutos por inactividad.", 60);
        hablar("Su sesión caducará en 2 minutos por inactividad")
    }

    //Si quedan 2 minutos de inactividad
    if (valor_inicial - 1 == 1) {

        //Activo a claudia
        hablar("Su sesión caducará en 1 minuto por inactividad");
        //Activo preguntar
        $('#modal_inactividad').modal('show');

    }

    //Si quedan 0 minutos de inactividad
    if (valor_inicial - 1 == 0) {
        cerrar_session();
    }

    //Informo al trazador
    obj = document.getElementById('TimeLeft');
    obj.innerText = valor_inicial - 1 + " minuto(s)."

    //Grabo el nuevo valor
    createCookie("Time_Session", valor_inicial - 1, 365);

    //actualizo mi método cada 60 segundo 
    window.setTimeout("lessMinutes();", 60000)

}