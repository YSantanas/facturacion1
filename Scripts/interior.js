$(document).ready(function () {
  

});

//Funcion para activar la pantalla de trabajando
function mostrar_trabajando(mensaje) {

    //Activo la pantalla grande de trabajando con tamaño personalizado
    $("#cuadro_trabajando_lbl_mensaje_trabajando").text(mensaje);
    $('#processing-modal').modal('show');

}

//Funcion para activar la pantalla de trabajando
function mostrar_modal(titulo,mensaje,boton) {
    
    //Activo la pantalla 
    $("#modal_lbl_titulo").text(titulo);
    $("#modal_lbl_mensaje").text(mensaje);
    $("#modal_btn_si").val(boton);
    $('#modal').modal('show');

}

//Cambia el puntero del raton por una mano de enlace
function hand(idDiv) {
    $("#" + idDiv).css({ 'cursor': 'pointer' });
}

//Notificación de Error
function error(mensaje) {
    alertify.error(mensaje);
    hablar(mensaje);
}

//Notificación de Correcto
function ok(mensaje) {
    alertify.success(mensaje);
}

//Notificación de Advertencia
function advertencia(mensaje, retraso) {
    if (retraso != '') {
        alertify.set({ delay: retraso * 1000 });
    };
    alertify.log(mensaje);
    hablar(mensaje);
    return false;
}

function hablar(texto) {

    //Obtengo el id_empresa
    var id_usuario = $(window.parent.document).find("#txt_id_usuario").val();

    //Vuelvo a leer los valores
    $.ajax({
        type: "POST",
        url: "../default.aspx/leer_sesion",
        data: '{nombre_sesion: "f_' + id_usuario + '_tabla_usuario"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Cargo los valores traidos
            var p_usuario = data.d.split("|");

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
    });

}

//CONTROL DE TECLAS RAPIDAS Escritorios Retrocede
shortcut.add("F1", function () {
    
    $(window.parent.document).find('#arrow_left').click();

});

//CONTROL DE TECLAS RAPIDAS Escritorios Avanza
shortcut.add("F2", function () {

    $(window.parent.document).find("#arrow_right").click();

});

//CONTROL DE TECLAS RAPIDAS Escritorios Avanza
shortcut.add("F4", function () {

    $(window.parent.document).find("#full_pantalla").click();

});

//CONTROL DE TECLAS RAPIDAS Limpia Escritorios
shortcut.add("F5", function () {

    $(window.parent.document).find("#img_limpia_pantalla").click();

});

//CONTROL DE TECLAS RAPIDAS Reorganiza Ventanas
shortcut.add("F6", function () {

    $(window.parent.document).find("#img_grid").click();

});

//CONTROL DE TECLAS RAPIDAS Ocultar/Mostrar barra
shortcut.add("F8", function () {

    $(window.parent.document).find("#img_inicio").click();

});

//Control de tamaño de fuente para los informes
function Size_fuente(size) {
    $("#txttamano_fuente").text(size);
    var valor = parseInt($("#txt_slider").val()) + 10
    $("#txttamano_fuente").css({ 'font-size': valor + 'px' });
}

//Calendario en español
$(function () {

    //Activo la fecha en los campos fecha
    //$(function () {
    //    $.datepicker.regional['es'] = {
    //        closeText: 'Cerrar',
    //        prevText: '<Ant',
    //        nextText: 'Sig>',
    //        currentText: 'Hoy',
    //        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    //        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    //        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
    //        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
    //        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
    //        weekHeader: 'Sm',
    //        dateFormat: 'dd/mm/yy',
    //        firstDay: 1,
    //        isRTL: false,
    //        showMonthAfterYear: false,
    //        yearSuffix: ''
    //    };
    //    $.datepicker.setDefaults($.datepicker.regional['es']);

        //$(".textbox_fecha_calendario").datepicker({
        //    dateFormat: 'dd/mm/yy',
        //    changeMonth: true,
        //    changeYear: true,
        //    language: "es"
        //});
    //});

    //$(".textbox_fecha").mask("99/99/9999");

    ////Control para abreviar poniendo dia y mes
    //$(".textbox_fecha").on('keydown', function (e) {
    //    var keyCode = e.which;

    //    //Restriccion con la tecla TAB=9
    //    if (keyCode === 9) {

    //        var valor_fecha = this.id;

    //        if ($("#" + valor_fecha).val().replace("____", "").length == 6) {
    //            $("#" + valor_fecha).val($("#" + valor_fecha).val() + (new Date).getFullYear())
    //        }

    //    }
    //});

    ////Leo los valores parametros_empresa
    //var parametros_empresa;
    //$.ajax({
    //    async: false,
    //    type: "POST",
    //    url: "../default.aspx/leer_session",
    //    data: '{nombre_sesion: "parametros_empresa" }',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data_empresa) {
    //        alert(data_empresa.d)
    //        parametros_empresa = (data_empresa.d).split("|");

    //    }

    //})

    ////Si es un campo cuenta activo todas las caracteristicas de tinfor
    //$(".textbox_cuenta").on('keydown', function (e) {
    //    var keyCode = e.which;
    //    var cuenta = $('#' + this.id).val();
    //    var elemento = this.id;
    //    var total_max = parametros_empresa[7];
    //    if ($.isNumeric(cuenta)) {
    //        $("#" + elemento).attr('maxlength', parametros_empresa[7]);
    //    } else {
    //        $("#" + elemento).attr('maxlength', '30');
    //    }

    //    if (keyCode === 9) {
    //        var diferencia = 0

    //        //cuenta con ++
    //        if (cuenta.indexOf("++") != -1) {
    //            cuenta = cuenta.substring(0, cuenta.length - 2)
    //            //busco la siguiente cuenta
    //            $.ajax({
    //                async: false,
    //                type: "POST",
    //                url: '../default.aspx/ultima_cuenta_nueva',
    //                data: "{ 'cuenta': '" + cuenta + "'}",
    //                dataType: "json",
    //                contentType: "application/json; charset=utf-8",
    //                success: function (data) {
    //                    //Asigno el valor
    //                    $("#" + elemento + "").val(data.d);
    //                },
    //                error: function (response) { alert(response.responseText); }
    //            });

    //            //Si el proceso entra en ++ añadir el resto no lo activo
    //            return;
    //        } else {

    //            //cuenta con +
    //            if (cuenta.indexOf("+") != -1) {
    //                cuenta = cuenta.substring(0, cuenta.length - 1)
    //                //busco la siguiente cuenta
    //                $.ajax({
    //                    async: false,
    //                    type: "POST",
    //                    url: '../default.aspx/ultima_cuenta',
    //                    data: "{ 'cuenta': '" + cuenta + "'}",
    //                    dataType: "json",
    //                    contentType: "application/json; charset=utf-8",
    //                    success: function (data) {
    //                        //Asigno el valor
    //                        $("#" + elemento + "").val(data.d);
    //                    },
    //                    error: function (response) { alert(response.responseText); }
    //                });

    //                //Si el proceso entra en + añadir el resto no lo activo
    //                return;
    //            }

    //        }

    //        //Si la cuenta no trae punto y la longitud no es la maxima de la empresa
    //        if (cuenta.indexOf(".") == -1) {
    //            var total_cuenta = cuenta.length;
    //            diferencia = total_max - total_cuenta;
    //            var string = "";
    //            var i;
    //            for (i = 0; i < diferencia; i++) {
    //                string += "0";
    //            };
    //            $('#' + this.id).val(cuenta + string);
    //        } else {
    //            //Si la cuenta trae punto y la longitud no es la maxima de la empresa
    //            var cuenta_inicial = cuenta.substring(0, cuenta.indexOf("."))
    //            var cuenta_final = cuenta.substring(cuenta.indexOf(".") + 1)
    //            var cuenta_inicial_count = cuenta_inicial.length
    //            var cuenta_final_count = cuenta_final.length
    //            diferencia = total_max - (cuenta_inicial_count + cuenta_final_count);
    //            var string = "";
    //            var i;
    //            for (i = 0; i < diferencia; i++) {
    //                string += "0";
    //            };
    //            $('#' + this.id).val(cuenta_inicial + string + cuenta_final);
    //        }

    //        //caso especial cuando lo deja en blanco y sustituye por 0
    //        var a = parseInt($('#' + this.id).val());
    //        if (a == 0) {
    //            $('#' + this.id).val('');
    //        }
    //    }
    //});

    //$('.textbox_cuenta').autocomplete({
    //    minLength: 3,
    //    source: function (request, response) {
    //        $.ajax({
    //            type: "POST",
    //            url: '../default.aspx/busca_cuenta',
    //            data: "{'prefix': '" + request.term + "'}",
    //            dataType: "json",
    //            contentType: "application/json; charset=utf-8",
    //            success: function (data) {
    //                response($.map(data.d, function (item) {
    //                    var compuesto = item.split("|");
    //                    return {

    //                        label: compuesto[0] + " - " + compuesto[1] + " (" + compuesto[2] + ")",
    //                        val: item
    //                    }
    //                }))
    //            },
    //            error: function (response) { alert(response.responseText); },
    //            failure: function (response) { alert(response.responseText); }
    //        });
    //    },
    //    focus: function (event, ui) {
    //        //descompongo resultados
    //        var compuesto = ui.item.val.split("|");
    //        $('#' + this.id + '').val(compuesto[0]);
    //        return false;
    //    },
    //    select: function (event, ui) {
    //        //descompongo resultados
    //        var compuesto = ui.item.val.split("|");
    //        $('#' + this.id + '').val(compuesto[0]);
    //        //para la denominacion
    //        $("#lbl_" + this.id + "").val(compuesto[1]);
    //        //para el nif
    //        $("#lbl_nif_" + this.id + "").val(compuesto[2]);
    //        return false;
    //    }

    //});

});
