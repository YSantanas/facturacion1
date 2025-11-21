var p_usuario = "";
var p_empresa = "";

$(document).ready(function () {
    
    //CONTROL DE TECLAS RAPIDAS Escritorios Retrocede
    shortcut.add("F1", function () {

        $("#arrow_left").click();

    });

    //CONTROL DE TECLAS RAPIDAS Escritorios Avanza
    shortcut.add("F2", function () {

        $("#arrow_right").click();

    });

    //CONTROL DE TECLAS RAPIDAS Escritorios Pantalla Completa
    shortcut.add("F4", function () {

        $("#full_pantalla").click();

    });

    //CONTROL DE TECLAS RAPIDAS Limpia Escritorios
    shortcut.add("F5", function () {

        $("#img_limpia_pantalla").click();

    });

    //CONTROL DE TECLAS RAPIDAS Reorganiza Ventanas
    shortcut.add("F6", function () {

        $("#img_grid").click();

    });

    //CONTROL DE TECLAS RAPIDAS Notificaciones
    shortcut.add("F8", function () {

        $("#img_notificaciones").click();

    });
    
    //CONTROL DE TECLAS RAPIDAS Errores
    shortcut.add("CTRL+E", function () {

        abrir_ventana('Controles', 'quiz', '910', '300', 'actualizaciones/errores.aspx', '11');

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

            //Ubicacion menu central
            var ancho = $(window).width() - 775;
            $("#contenedor_menu").css({ 'width': ancho + 'px' });

            //Aparece Menu Inicio
            $("#menu").fadeIn(900).show();

            izquierdo = 50; // ($(window).width() - 700) / 2;
            var alto = ($(window).height() - 400);
            $("#barra_menu").css({ 'left': izquierdo + 'px', 'height': alto + 'px' });
            alto = alto - 108 //alto de los 3 menus
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
            $("#lblnombre2").text(p_usuario[1] + " " + p_usuario[2] + " " + p_usuario[3]);

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
    $("#body_contenedor_desktop").css({ height: $("#body_contenedor").height() - 50 + 'px', top: '0px' });
   
    //Tamaño para el desktop
    $(window).resize(function (e) {
        $("#body_contenedor_desktop").css({ height: $("#body_contenedor").height() - 50 + 'px', top: '0px' });
        $("#notificaciones").css({ height: ($("#body_contenedor").height() - 300) + 'px' });
        //autoajuste de pantalla
        autoajustado();
    });

    function autoajustado() {
        
        //Leo de control pantalla hasta llegar a los datos determinados
        for (var i = 0; i <= control_pantalla.length - 1; i++) {
            //Descompongo los detalles de cada ventana
            var detalles_ventana = control_pantalla[i].split("#")
            if (detalles_ventana[3] == "1") {
                //calculo el top + height de la ventana
                var altura_total = parseFloat(detalles_ventana[5]) + parseFloat(detalles_ventana[8])
                
                if (detalles_ventana[11] == "1") {
                    maximizar_ventana(detalles_ventana[0])
                }

                //Detectamos la(s) ventanas que no caben en la pantalla que lo contiene
                if (altura_total > $("#body_contenedor_desktop").height()) {

                    //calcular la diferencia
                    var calculo = altura_total - parseFloat($("#body_contenedor_desktop").height())
                    //Declarar nuevos valores
                    var nuevo_top = parseFloat(detalles_ventana[5] - calculo) 
                    var nuevo_left = detalles_ventana[6]
                    var nuevo_ancho = detalles_ventana[7]
                    var nuevo_alto = detalles_ventana[8]

                    //construyo los nuevos valores
                    var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#" + detalles_ventana[4] + "#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#" + detalles_ventana[11]
                    //asigno valor
                    control_pantalla[i] = nuevo_valor_temp
                    
                    //Ajusto el Iframe al tamaño de la ventana general
                    $("#" + detalles_ventana[0]).css({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' });
                    $("#iframe_" + detalles_ventana[0]).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });
                  
                }

                if (parseFloat(detalles_ventana[8]) > $("#body_contenedor_desktop").height()) {

                    //Declarar nuevos valores
                    var nuevo_top = detalles_ventana[5]
                    var nuevo_left = detalles_ventana[6]
                    var nuevo_ancho = detalles_ventana[7]
                    var nuevo_alto = $("#body_contenedor_desktop").height()

                    //construyo los nuevos valores
                    var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#" + detalles_ventana[4] + "#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10]
                    //asigno valor
                    control_pantalla[i] = nuevo_valor_temp

                    //Ajusto el Iframe al tamaño de la ventana general
                    $("#" + detalles_ventana[0]).css({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' });
                    $("#iframe_" + detalles_ventana[0]).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

                }

            }
        }

    }
    
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

    //Re-organizar ventanas
    $("#img_grid").on('click', function () {

        //averiguar cuantas ventanas activas tengo en esa pantalla
        var numero_total = 0;

        //Descompongo las ventanas
        for (var i = 0; i <= control_pantalla.length - 1; i++) {

            //Descompongo los detalles de cada ventana
            var detalles_ventana = control_pantalla[i].split("#")

            //Averiguo que ventanas estan activas en esa pantalla
            if (detalles_ventana[2] == $("#numero_pantalla").text() && detalles_ventana[3] == "1" && detalles_ventana[4] == "0") {
                numero_total += 1;
            }

        }

        //Si el numero es 0 no hay nada que organizar
        if (numero_total == 0) {
            hablar("No hay ventanas para organizar.");
            return;
        }

        //Si el numero es 1 no hay nada que organizar
        if (numero_total == 1) {
            hablar("No hay nada que organizar con una sola ventana.");
            return;
        }

        //Realizo cálculo para saber el alto y ancho de cada ventana
        var calculo = numero_total / 3;
        var alto_calculo = Math.floor(calculo);

        //Si es decimal entonces necesito uno mas para el calculo de la altura
        if (calculo % 1 != 0) {
            alto_calculo += 1;
        }

        //Excepciones-----------------------------------------------------
        var ancho_calculado = 3;
        var grid = 3

        //si son dos ventanas
        if (numero_total == 2) {
            ancho_calculado = 2
        }

        //si son 4 ventanas
        if (numero_total == 4) {
            ancho_calculado = 2
            alto_calculo = 2
            grid = 2
        }

        //Nuevos anchos y altos de las ventanas
        var ancho_pantalla = $('#body_contenedor_desktop').width() / ancho_calculado
        var alto_pantalla = $('#body_contenedor_desktop').height() / alto_calculo

        //Variables
        var nuevo_top = 0;
        var nuevo_left = 0;
        var contador = 1;

        //Inforno al usuario
        hablar("Organizando ventanas");

        //Descompongo las ventanas
        for (var i = 0; i <= control_pantalla.length - 1; i++) {

            //Descompongo los detalles de cada ventana
            var detalles_ventana = control_pantalla[i].split("#")

            //Averiguo que ventanas estan activas en esa pantalla
            if (detalles_ventana[2] == $("#numero_pantalla").text() && detalles_ventana[3] == "1" && detalles_ventana[4] == "0") {

                //Ajusto el Iframe al tamaño de la ventana general
                $("#" + detalles_ventana[0]).animate({ top: nuevo_top + 'px', left: nuevo_left + 'px', width: ancho_pantalla + 'px', height: alto_pantalla + 'px' }, { duration: 500 })
                $("#iframe_" + detalles_ventana[0]).css({ width: (ancho_pantalla - 5) + 'px', height: (alto_pantalla - 40) + 'px' });

                //$("#" + detalles_ventana[0]).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
                if (contador == grid) {
                    nuevo_left = 0;
                    nuevo_top += alto_pantalla
                    contador = 1
                } else {
                    nuevo_left += ancho_pantalla
                    contador += 1

                }

            }

        }

    });

    //Despliego el menu de limpiar pantallas
    $("#img_limpia_pantalla").click(function () {

        //leo los valores de la cookie Maestra
        var nuevo_valor_temp = "";
        var claudia = false //voz del sistema

        //Leo de control pantalla hasta llegar a los datos determinados
        for (var i = 0; i <= control_pantalla.length - 1; i++) {

            var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
            if (detalles_ventana[3] == "1") {
                //cierro esa ventana abierta
                cerrar_ventana(detalles_ventana[0])
                //Voz para Claudia
                var claudia = true
            }

        }

        if (claudia == true) {
            hablar("Cerrando Ventanas.");
        } else {
            hablar("No hay nada que cerrar.");
        }

    });

    //Click en Expandir Monitor
    $("#img_monitor").on('click', function () {
        if ($('#menu_expandir_monitor').is(":visible")) {
            $("#menu_expandir_monitor").hide(200);
        } else {
            $("#menu_expandir_monitor").show(200);
        }
    });
  
    //Despliego el multi-screen
    $("#arrow_right").click(function () {

        //Si el valor ya es 3
        var number_screen = parseInt($("#numero_pantalla").text());

        if (number_screen + 1 === 10) {
            hablar("No hay mas escritorios disponibles");
            return;
        }

        //Muestro la nueva pantalla
        $("#numero_pantalla").text(number_screen + 1);
        
        //Aviso de cambio de escritorio
        aviso_cambio_escritorio(number_screen + 1);
        
        //Se encarga de abrir las ventanas de esa pantalla elegida
        abrir_ventanas_pantalla();

    });

    $("#arrow_left").click(function () {

        //Si el valor ya es 1
        var number_screen = parseInt($("#numero_pantalla").text());

        if (number_screen - 1 < 1) {
            hablar("No hay mas escritorios disponibles");
            return;
        }

        //Muestro la nueva pantalla
        $("#numero_pantalla").text(number_screen - 1);

        //Aviso de cambio de escritorio
        aviso_cambio_escritorio(number_screen - 1);

        //Se encarga de abrir las ventanas de esa pantalla elegida
        abrir_ventanas_pantalla();

    });

    //Click en Informacion
    $("#img_informacion").on('click', function () {
        //Activo a claudia
        hablar("Información de la empresa.");
        //Activo preguntar
        $('#modal_informacion').modal('show');
    });

    //Click en Informacion
    $("#img_contacto").on('click', function () {
        //Activo a claudia
        hablar("contactos");
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

    //Click en Salir
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
        createCookie("Time_Session.Facturacion", 20, 365);
        //Cerrar PopUp
        $('#modal_inactividad').modal('hide');
    });

    //Despliegue de las posiciones de las ventanas
    $(".selector_movimiento").mouseenter(function () {
        $("#" + this.id + "_posicion_orden").show(0);
    });

    $(".selector_movimiento").mouseleave(function () {
        $("#" + this.id + "_posicion_orden").hide(100);
    });

    $(".color_posicion").mouseenter(function () {
        $("#" + this.id).css({ 'background-color': '#cce5ff' });
    });

    $(".color_posicion").mouseleave(function () {
        $("#" + this.id).css({ 'background-color': '#e2e3e5' });
    });

    //Muetra el reloj en la barra
    setInterval('digiClock()', 1000);

    //Control para traer al frente la ventana click-eada
    $(".screen").click(function () {
        $(".screen").css({ 'z-index': '9' });
        $(".screen").css({ 'border': '1px solid #b3b3b3' });

        $("#" + this.id).css({ 'z-index': '10' });
        $("#" + this.id).css({ 'border': '2px solid #d4edda' });
    });

    //Leer ventanas abiertas de la ultima session
    activar_ventana();

    //Desplegando menus
    $(".contenedor_menus").mouseenter(function () {
        //$("#contenido_" + this.id + "").slideToggle(100);
        $("#contenido_" + this.id + "").show(100);
    });

    $(".contenedor_menus").mouseleave(function () {
        //$("#contenido_" + this.id + "").slideToggle(100);
        $("#contenido_" + this.id + "").hide(100);
    });

});

//Cargo un array con los valores de las ventanas
var control_pantalla = [];

if (readCookie("Ventanas.Facturacion") != "Vacio") {
    //leo los valores de la cookie Maestra
    var valores = readCookie("Ventanas.Facturacion").split(","); //{Ventana, Grupo, Screen, Activa, Estado, Top,left,width,height,imagen,estado_max_min,visible}
    
    for (var i = 0; i <= valores.length - 1; i++) {
        //Asigno al vector
        control_pantalla.push(valores[i])
    }
}

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
    p_usuario[30] = ($("#txt_volumen").val() / 100).toString().replace(",",".");
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

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        //Leo los detalles de la ventana
        var detalles_ventana = control_pantalla[i].split("#");

        //construyo los nuevos valores
        var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#" + detalles_ventana[4] + "#" + detalles_ventana[5] + "#" + detalles_ventana[6] + "#" + detalles_ventana[7] + "#" + detalles_ventana[8] + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#" + detalles_ventana[11]

        //asigno valor
        control_pantalla[i] = nuevo_valor_temp

    }

    //-------------------------------------------------------------------------------------------------------------------
    $.ajax({
        type: "POST",
        url: "default.aspx/cerrar_sesion",
        data: '{id_usuario: "' + $("#txt_id_usuario").val() + '",id_empresa: "' + $("#txt_id_empresa").val()  + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: cerrando,
        error: errores
    });

    function cerrando(msg) {
       
        //grabo la cookie de menu ventana
        createCookie("Ventanas.Facturacion", control_pantalla.join(), 364);

        //redirijo
        window.location = "login.aspx"
    };

    function errores(msg) {
        //msg.responseText tiene el mensaje de error enviado por el servidor
        alert('Error: ' + msg.responseText);
    };

}

function ir_escritorio(numero) {

    //Si el valor ya es 3
    var number_screen = parseInt($("#numero_pantalla").text());

    if (number_screen === numero) {

        //Simulo click para cerrar la ventana
        $("#img_monitor").click();

        hablar("Ya estas en el escritorio que has seleccionado");
        return;
    }

    //Muestro la nueva pantalla
    $("#numero_pantalla").text(numero);

    //Aviso de cambio de escritorio
    aviso_cambio_escritorio(numero);

    //Se encarga de abrir las ventanas de esa pantalla elegida
    abrir_ventanas_pantalla();

    //Simulo click para cerrar la ventana
    $("#img_monitor").click();

}

//Funciones para colorear la barra
function mouse_hover_barra(idDiv, color) {
    
    //Cambio de color en la barra
    $("#barra_" + idDiv).css({ 'border-left': '1px solid ' + color + '', 'cursor': 'pointer' });

}

function salir_mouse_barra(idDiv) {

    //Dejo el color como estaba
    $("#barra_" + idDiv).css({ 'border-left': '0px solid gray', 'cursor': 'default' });
    
}

function aviso_cambio_escritorio(numero_escritorio) {

    switch (numero_escritorio) {
        case 1:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-1-square'></span>";
            break;
        case 2:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-2-square'></span>";
            break;
        case 3:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-3-square'></span>";
            break;
        case 4:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-4-square'></span>";
            break;
        case 5:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-5-square'></span>";
            break;
        case 6:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-6-square'></span>";
            break;
        case 7:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-7-square'></span>";
            break;
        case 8:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-8-square'></span>";
            break;
        case 9:
            valor_imagen = "<span style='font-size:120px;' class='bi bi-9-square'></span>";
            break;
    } 
    
    $("#aviso_cambio_escritorio").empty();
    $("#aviso_cambio_escritorio").append(valor_imagen);
    $("#aviso_cambio_escritorio").show(100).delay(500).hide(100)
        
}

//Funciones comunes a todas las ventanas(minimizar, maximizar,cerrar,etc
function activar_ventana() {
    
    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {
        //Descompongo los detalles de cada ventana
        var detalles_ventana = control_pantalla[i].split("#")
        
        if (detalles_ventana[3] == "1" && detalles_ventana[2] == $("#numero_pantalla").text() && detalles_ventana[4] == "0") {
            
            //Abrir ventana
            abrir_ventana(detalles_ventana[0], detalles_ventana[9], detalles_ventana[7], detalles_ventana[8], detalles_ventana[10], detalles_ventana[1])
        }
        //Activo en la barra los menus abiertos en otras pantallas
        if (detalles_ventana[3] == "1") {
            componer_barra(detalles_ventana[0])
        }
        
    }
}

function abrir_ventanas_pantalla() {
    
    //Descompongo las ventanas
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        //Descompongo los detalles de cada ventana
        var detalles_ventana = control_pantalla[i].split("#")

        //Ocultar la pantalla
        $("#" + detalles_ventana[0]).css({ 'visibility': 'hidden' });

        if (detalles_ventana[2] == $("#numero_pantalla").text() && detalles_ventana[3] == "1" && detalles_ventana[4] == "0") {

            //Abrir ventana
            var ruta2 = detalles_ventana[10] + '?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val();
            abrir_ventana(detalles_ventana[0], 'Appointment.png', '1000', '600', ruta2)
            
        }
       
    }
    
}

function cerrar_ventana(idDiv) {
   
    //leo los valores de la cookie Maestra
    var grupo;
    
    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            
            //construyo los nuevos valores
            var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#0#0#" + detalles_ventana[5] + "#" + detalles_ventana[6] + "#" + detalles_ventana[7] + "#" + detalles_ventana[8] + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            // asigno el grupo perteneciente
            grupo = detalles_ventana[1]
            //termino el bucle
            break;
        }

    }
    
    //Ocultar la pantalla
    $("#" + idDiv).css({ 'visibility': 'hidden' });

    //Componer barra
    descomponer_barra(idDiv, grupo);

}

function abrir_ventana(idDiv, imagen, ancho, alto, ruta, grupo) {
    
   //Excepcion para cargar los videos de youtube
    if (idDiv != 'visor_youtube' && idDiv != 'visualizador') {

        //Controlo que la ruta Venga, sino la asigno
        var src = $("#i" + idDiv).attr('src');
        
        if (src.indexOf('vacia.html') != -1) {

            var ruta2 = '';
            if (ruta.indexOf('id_usuario') != -1) {
                ruta2 = ruta;
            } else {
                ruta2 = ruta + '?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val();
            }
            $("#i" + idDiv).attr('src', ruta2);

        }

    } else {

        $("#i" + idDiv).attr('src', ruta);

    }
    
    //Leo las posiciones
    posicion_efecto(idDiv, imagen, ancho, alto, ruta, grupo)

    //Asigno al Div que sea visible y que este por encima de todos
    $(".screen").css({ zIndex: 9 });
    $("#" + idDiv).css({ 'visibility': 'visible', 'z-index': '10' });

    //Componer barra
    //Remueve de la barra de tareas
    $("#barra_" + idDiv).remove();
    $("#barra_" + idDiv + "_1").remove();
    componer_barra(idDiv);

    //Oculto la barra al abrir la ventana    
    $("#barra_menu").hide(800);

}

function posicion_efecto(idDiv, imagen, ancho, alto, ruta, grupo) {

    $("#" + idDiv).draggable({
        containment: "#body_contenedor_desktop",
        handle: ".move",
        cursor: "move",
        opacity: 0.75,
        //delay: 3000,
        stack: ".screen",
        drag: function (event, ui) {

            //leo los valores de la cookie Maestra
            var nuevo_top = ui.position.top
            var nuevo_left = ui.position.left

            //Leo de control pantalla hasta llegar a los datos determinados
            for (var i = 0; i <= control_pantalla.length - 1; i++) {
                
                var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
                if (detalles_ventana[0] == idDiv) {
                    //construyo los nuevos valores
                    var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + $("#numero_pantalla").text() + "#" + detalles_ventana[3] + "#" + detalles_ventana[4] + "#" + nuevo_top + "#" + nuevo_left + "#" + detalles_ventana[7] + "#" + detalles_ventana[8] + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#" + detalles_ventana[11]
                    //asigno valor
                    control_pantalla[i] = nuevo_valor_temp
                    //termino el bucle
                    break;
                }


            }

            //asigno un valor para el zindex para que aparezca en la parte superior
            $(".screen").css({ zIndex: 9 });
            $(".screen").css({ 'border': '1px solid #b3b3b3' });

            $("#" + idDiv).css({ 'visibility': 'visible', 'z-index': '10' });
            $("#" + this.id).css({ 'border': '2px solid #d4edda' });
        },
        stop: function (event, ui) {

            //Remueve de la barra de tareas
            $("#barra_" + idDiv).remove();
            $("#barra_" + idDiv + "_1").remove();

            //
            componer_barra(idDiv);
            
        }
    });

    $("#" + idDiv).resizable({
        minHeight: 200,
        minWidth: 480,
        handles: 'e,w,n,s',
        containment: "#body_contenedor_desktop",
        //ghost: true,
        //animate: true,
        //animateDuration: "fast",
        start: function (event, ui) {
            $('<div class="ui-resizable-iframeFix" style="background: #fff;"></div>')
                .css({
                    width: '100%', height: '95%',
                    position: "absolute", opacity: "0.001", zIndex: 1000
                })
                .appendTo("body");
        },
        resize: function (event, ui) {
            $("#iframe_" + idDiv).css({ width: '' + (ui.size.width - 5) + 'px', height: '' + (ui.size.height - 45) + 'px' });
        },
        stop: function (event, ui) {
            $('.ui-resizable-iframeFix').remove();

            //leo los valores de la cookie Maestra
            var nuevo_ancho = ui.size.width
            var nuevo_alto = ui.size.height

            //Leo de control pantalla hasta llegar a los datos determinados
            for (var i = 0; i <= control_pantalla.length - 1; i++) {

                var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
                if (detalles_ventana[0] == idDiv) {
                    //construyo los nuevos valores
                    var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#" + detalles_ventana[4] + "#" + detalles_ventana[5] + "#" + detalles_ventana[6] + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
                    //asigno valor
                    control_pantalla[i] = nuevo_valor_temp
                    //termino el bucle
                    break;
                }

            }

            //asigno un valor para el zindex para que aparezca en la parte superior
            $(".screen").css({ zIndex: 9 });
            $(".screen").css({ 'border': '1px solid #b3b3b3' });

            $("#" + idDiv).css({ 'visibility': 'visible', 'z-index': '10' });
            $("#" + this.id).css({ 'border': '2px solid #d4edda' });

        }
    });
   
    //----------------------------------------------------------------------------------------------------------
    
    //Declaración de Variables
    var grupo, screen, activa, minimizado, top, left, width, height, imagen, ruta, maximizado

    if (control_pantalla.join().length == 0) {

        // Creo los valores básico para mostrar la ventana
        grupo = grupo;
        screen = $("#numero_pantalla").text();
        activa = 1;
        minimizado = 0;
        top = Math.floor(Math.random() * 100);
        left = (Math.floor(Math.random() * 200) + 55);
        width = ancho;
        height = alto;
        imagen = imagen;
        ruta = ruta;
        maximizado = 0;

        //valores a grabar
        var valores_grabar = idDiv + "#" + grupo + "#" + screen + "#" + activa + "#" + minimizado + "#" + top + "#" + left + "#" + width + "#" + height + "#" + imagen + "#" + ruta + "#" + maximizado

        //Grabo el valor en la variable
        control_pantalla.push(valores_grabar)

    } else {

        //asignador nuevo
        var nueva = true;

        //Leo de control pantalla hasta llegar a los datos determinados
        for (var i = 0; i <= control_pantalla.length - 1; i++) {

            var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana

            //Busco en esos detalles el nombre de la ventana que se ha mandado a abrir
            if (detalles_ventana[0] == idDiv) {

                //construyo los nuevos valores
                var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + $("#numero_pantalla").text() + "#1#" + detalles_ventana[4] + "#" + detalles_ventana[5] + "#" + detalles_ventana[6] + "#" + detalles_ventana[7] + "#" + detalles_ventana[8] + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#" + detalles_ventana[11]
                
                //asigno valor
                control_pantalla[i] = nuevo_valor_temp
                
                //Asigno los valores
                minimizado = detalles_ventana[4] //minimizado
                top = detalles_ventana[5] //top
                left = detalles_ventana[6] //left
                width = detalles_ventana[7] //width
                height = detalles_ventana[8] //height
                maximizado = detalles_ventana[11] //minimizado

                //si la encontramos no asignamos nueva
                nueva = false;

                //salimos del for
                break;

            }

        }

        //si no la hemos encontrado en el for asignamos nuevo
        if (nueva == true) {
            // Creo los valores básico para mostrar la ventana
            grupo = grupo;
            screen = $("#numero_pantalla").text();
            activa = 1;
            minimizado = 0;
            top = Math.floor(Math.random() * 100);
            left = (Math.floor(Math.random() * 200) + 55);
            width = ancho;
            height = alto;
            imagen = imagen;
            ruta = ruta;
            maximizado = 0;

            //valores a grabar
            var valores_grabar = idDiv + "#" + grupo + "#" + screen + "#" + activa + "#" + minimizado + "#" + top + "#" + left + "#" + width + "#" + height + "#" + imagen + "#" + ruta + "#" + maximizado

            //Grabo el valor en la variable
            control_pantalla.push(valores_grabar)

        }

    }

    ////Si el estado es maximizado
    if (maximizado == "1") {
        top = 0;
        left = 0;
        width = parseInt($(window).width());
        height = parseInt($(window).height() - 50);
    }

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).css({ top: '' + top + 'px', left: '' + left + 'px', width: '' + width + 'px', height: '' + height + 'px', 'visibility': 'visible', 'display': 'inline'  });
    $("#iframe_" + idDiv).css({ width: (width - 5) + 'px', height: (height - 45) + 'px' });
   
}

function componer_barra(idDiv) {
    
    //Recorro todas las ventanas abiertas
    //Leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
   
    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        //Leo los detalles de la ventana
        var detalles_ventana = control_pantalla[i].split("#"); 

        //Si la ventana esta activa y el nombre de la ventana es la enviada
        if (detalles_ventana[3] == "1" && detalles_ventana[0] == idDiv) {

        //Preparo los nombres
        var patron = /_/g
        var nuevoValor = " "
        var string_escribe = detalles_ventana[0].replace(patron,nuevoValor).toUpperCase()
        
        //Controlo activar el grupo
        switch (detalles_ventana[1]) {
            case "1":
                $("#contenedor_asientos").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) { 
                    $('#barra_asientos').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#0d6efd') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #0d6efd;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #0d6efd; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "2":
                $("#contenedor_impuestos").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) { 
                    $('#barra_impuestos').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#6f42c1') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #6f42c1;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #6f42c1; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "3":
                $("#contenedor_inmovilizados").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_inmovilizados').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#28a745') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #28a745;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #28a745; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "4":
                $("#contenedor_cartera").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_cartera').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#fd7e14') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #fd7e14;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #fd7e14; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "5":
                $("#contenedor_cobros_pagos").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_cobros_pagos').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#dc3545') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #dc3545;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #dc3545; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "6":
                $("#contenedor_remesas").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_remesas').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#ffc107') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #ffc107;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #ffc107; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "7":
                $("#contenedor_informes").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_informes').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#292929') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #292929;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #292929; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "8":
                $("#contenedor_configuracion").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_configuracion').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#292929') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #292929;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #292929; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "9":
                $("#contenedor_supervisor").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_supervisor').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#292929') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #292929;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #292929; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "10":
                $("#contenedor_estadisticas").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_estadisticas').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#292929') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #292929;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #292929; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "11":
                $("#contenedor_ayuda").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_ayuda').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#292929') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #292929;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #292929; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "12":
                $("#contenedor_gestion").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_gestion').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#6610f2') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #6610f2;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #6610f2; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            case "13":
                $("#contenedor_propietario").css({ 'visibility': 'visible', 'display': 'inline' });
                if ($("#barra_" + detalles_ventana[0] + "").length == 0) {
                    $('#barra_propietario').append("<tr><td id=barra_" + detalles_ventana[0] + " align='center' onclick=restaurar_barra('" + detalles_ventana[0] + "') onmouseover=mouse_hover_barra('" + detalles_ventana[0] + "','#292929') onmouseout=salir_mouse_barra('" + detalles_ventana[0] + "') class='transparencia_barra'><span class='" + detalles_ventana[9] + "' style='color: #292929;font-size: 16px;'></span>&nbsp;&nbsp;<span style='font-size: 12px;'>" + string_escribe + "</span></td><td id=barra_" + detalles_ventana[0] + "_1 style='color: #292929; width:35px; padding-left: 2px; padding-right: 2px; '>" + detalles_ventana[2] + "&nbsp;<span class='bi bi-display' style='font-size:15px;'></span></td></tr>");
                }
                break;
            }

        }

    }

}

function descomponer_barra(idDiv, grupo) {

    //Remueve de la barra de tareas
    $("#barra_" + idDiv).remove();
    $("#barra_" + idDiv + "_1").remove();

    //Recorro todas las ventanas 
    var ocultar_grupo = true;

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        //Leo los detalles de la ventana
        var detalles_ventana = control_pantalla[i].split("#");
        
        //Si la ventana esta activa y el nombre de la ventana es la enviada
        if (detalles_ventana[1] == grupo && detalles_ventana[3] == "1") {
            //asigno valor
            ocultar_grupo = false;
            break;
        }

    }
    
    if (ocultar_grupo == true) {
        
        switch (grupo) {
            case "1":
                $("#contenedor_asientos").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "2":
                $("#contenedor_impuestos").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "3":
                $("#contenedor_inmovilizados").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "4":
                $("#contenedor_cartera").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "5":
                $("#contenedor_cobros_pagos").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "6":
                $("#contenedor_remesas").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "7":
                $("#contenedor_informes").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "8":
                $("#contenedor_configuracion").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "9":
                $("#contenedor_supervisor").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "10":
                $("#contenedor_estadisticas").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "11":
                $("#contenedor_ayuda").css({ 'visibility': 'hidden', 'display': 'none' });
                break;
            case "12":
                $("#contenedor_gestion").css({ 'visibility': 'hidden', 'display': 'none' });
                break;         
            case "13":
                $("#contenedor_propietario").css({ 'visibility': 'hidden', 'display': 'none' });
                break;         
        }

    }
    
}

function restaurar_barra(idDiv) {

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + detalles_ventana[5] + "#" + detalles_ventana[6] + "#" + detalles_ventana[7] + "#" + detalles_ventana[8] + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#" + detalles_ventana[11]
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp

            //detectando la pantalla original que la abrio
            $("#numero_pantalla").text(detalles_ventana[2]);
            abrir_ventanas_pantalla();

            //termino el bucle
            break;
        }
    }

    //Asigno al Div que sea visible y que este por encima de todos
    $(".screen").css({ zIndex: 9 });
    $("#" + idDiv).fadeTo(200, 1);
    $("#" + idDiv).css({ 'z-index': '10' });

}

function abrir_ventana_relacional(idDiv, imagen, ancho, alto, ruta, grupo) {

    //Actualizar la url para las distintas partes de los menús
    if (ruta.indexOf("|") != -1) {
        
        //Extraigo el texto a quitar
        var texto = ruta.substr(ruta.indexOf("|"))

        //Quito el texto de la URL
        ruta = ruta.replace(texto, "")

        //Asigno los valores nuevos
        ruta = ruta + '?id_usuario=' + $("#txt_id_usuario").val() + '&id_empresa=' + $("#txt_id_empresa").val() + texto.replace("|","&");
        
        //Asigno
        $('#i' + idDiv).attr('src', ruta);

    }

    //compruebo si esta en la barra de tareas
    if ($("#barra_" + idDiv).length > 0) {

        restaurar_barra(idDiv)

    } else {

        abrir_ventana(idDiv, imagen, ancho, alto, ruta, grupo)

    }

}

function minimizar_ventana(idDiv) {
    
    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#1#" + detalles_ventana[5] + "#" + detalles_ventana[6] + "#" + detalles_ventana[7] + "#" + detalles_ventana[8] + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#" + detalles_ventana[11]
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Oculto el Div
    $("#" + idDiv).fadeTo(200, 0);
    $("#" + idDiv).css({ 'z-index': -1 });

    //Claudia
    hablar("minimizando pantalla")

}

function ajustar_izquierdo(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width() / 2
    var nuevo_alto = $('#body_contenedor_desktop').height()

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado izquierdo");

};

function ajustar_derecho(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = $('#body_contenedor_desktop').width() / 2
    var nuevo_ancho = $('#body_contenedor_desktop').width() / 2
    var nuevo_alto = $('#body_contenedor_desktop').height()

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado derecho")

};

function ajustar_izquierdo_60(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width() * 60 / 100
    var nuevo_alto = $('#body_contenedor_desktop').height()

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado izquierdo");

};

function ajustar_derecho_40(idDiv) {

    //Si lo pega al lado derecho
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = $('#body_contenedor_desktop').width() * 60 / 100
    var nuevo_ancho = $('#body_contenedor_desktop').width() * 40 / 100
    var nuevo_alto = $('#body_contenedor_desktop').height()

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado derecho")

};

function ajustar_izquierdo_40(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width() * 40 / 100
    var nuevo_alto = $('#body_contenedor_desktop').height()

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado izquierdo");

};

function ajustar_derecho_60(idDiv) {

    //Si lo pega al lado derecho
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = $('#body_contenedor_desktop').width() * 40 / 100
    var nuevo_ancho = $('#body_contenedor_desktop').width() * 60 / 100
    var nuevo_alto = $('#body_contenedor_desktop').height()

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado derecho")

};

function ajustar_arriba_izquierdo(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width() / 2
    var nuevo_alto = $('#body_contenedor_desktop').height() / 2

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado superior izquierdo");
    
};

function ajustar_arriba_derecho(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = $('#body_contenedor_desktop').width() / 2
    var nuevo_ancho = $('#body_contenedor_desktop').width() / 2
    var nuevo_alto = $('#body_contenedor_desktop').height() / 2
    
    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }
    
    //Claudia
    hablar("ajustando al lado superior derecho")

};

function ajustar_abajo_izquierdo(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = $('#body_contenedor_desktop').height() / 2
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width() / 2
    var nuevo_alto = $('#body_contenedor_desktop').height() / 2

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado inferior izquierdo")

};

function ajustar_abajo_derecho(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = $('#body_contenedor_desktop').height() / 2
    var nuevo_left = $('#body_contenedor_desktop').width() / 2
    var nuevo_ancho = $('#body_contenedor_desktop').width() / 2
    var nuevo_alto = $('#body_contenedor_desktop').height() / 2

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado inferior derecho")

};

function maximizar_ventana(idDiv) {

    //leo los valores de la cookie Maestra
    var nuevo_estado, nuevo_top, nuevo_left, nuevo_ancho, nuevo_alto

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //comprobamos si esta normal o maximizada
            if (detalles_ventana[11] == "1") {
                nuevo_estado = "0"
            } else {
                nuevo_estado = "1"
            }

            //Asigno los valores
            nuevo_top = detalles_ventana[5]
            nuevo_left = detalles_ventana[6]
            nuevo_ancho = detalles_ventana[7]
            nuevo_alto = detalles_ventana[8]
            //construyo los nuevos valores
            var nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#" + detalles_ventana[4] + "#" + detalles_ventana[5] + "#" + detalles_ventana[6] + "#" + detalles_ventana[7] + "#" + detalles_ventana[8] + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#" + nuevo_estado
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //asigno valores
    if (nuevo_estado == true) {

        nuevo_top = 0;
        nuevo_left = 0;
        nuevo_ancho = parseInt($(window).width());
        nuevo_alto = parseInt($(window).height() - 50);

        //Claudia
        hablar("maximizando ventana")
    }

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

}

function ajustar_arriba(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width() 
    var nuevo_alto = $('#body_contenedor_desktop').height() / 2

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado superior");

};

function ajustar_abajo(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = $('#body_contenedor_desktop').height() / 2
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width()
    var nuevo_alto = $('#body_contenedor_desktop').height() / 2

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado inferior");

};

function ajustar_arriba_40(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width()
    var nuevo_alto = $('#body_contenedor_desktop').height() * 40 / 100

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado superior");

};

function ajustar_abajo_60(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = $('#body_contenedor_desktop').height() * 40 / 100
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width()
    var nuevo_alto = $('#body_contenedor_desktop').height() * 60 / 100

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado inferior");

};

function ajustar_arriba_60(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = 0
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width()
    var nuevo_alto = $('#body_contenedor_desktop').height() * 60 / 100

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado superior");

};

function ajustar_abajo_40(idDiv) {

    //Si lo pega al lado izquierdo
    //leo los valores de la cookie Maestra
    var nuevo_valor_temp = "";
    var nuevo_top = $('#body_contenedor_desktop').height() * 60 / 100
    var nuevo_left = 0
    var nuevo_ancho = $('#body_contenedor_desktop').width()
    var nuevo_alto = $('#body_contenedor_desktop').height() * 40 / 100

    //Ajusto el Iframe al tamaño de la ventana general
    $("#" + idDiv).animate({ top: '' + nuevo_top + 'px', left: '' + nuevo_left + 'px', width: '' + nuevo_ancho + 'px', height: '' + nuevo_alto + 'px' }, { duration: 500 })
    $("#iframe_" + idDiv).css({ width: (nuevo_ancho - 5) + 'px', height: (nuevo_alto - 40) + 'px' });

    //Leo de control pantalla hasta llegar a los datos determinados
    for (var i = 0; i <= control_pantalla.length - 1; i++) {

        var detalles_ventana = control_pantalla[i].split("#"); //Leo los detalles de la ventana
        if (detalles_ventana[0] == idDiv) {
            //construyo los nuevos valores
            nuevo_valor_temp = detalles_ventana[0] + "#" + detalles_ventana[1] + "#" + detalles_ventana[2] + "#" + detalles_ventana[3] + "#0#" + nuevo_top + "#" + nuevo_left + "#" + nuevo_ancho + "#" + nuevo_alto + "#" + detalles_ventana[9] + "#" + detalles_ventana[10] + "#0"
            //asigno valor
            control_pantalla[i] = nuevo_valor_temp
            //termino el bucle
            break;
        }

    }

    //Claudia
    hablar("ajustando al lado inferior");

};

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
    var valor_inicial = readCookie("Time_Session.Facturacion")
    
    //Si quedan 2 minutos de inactividad
    if (valor_inicial - 1 == 2) {
        advertencia("Su sesión caducará en 2 minutos por inactividad.", 60);
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
    createCookie("Time_Session.Facturacion", valor_inicial - 1, 365);

    //actualizo mi método cada 60 segundo 
    window.setTimeout("lessMinutes();", 60000)

}