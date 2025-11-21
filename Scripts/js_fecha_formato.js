$(document).ready(function () {

    $(".textbox_fecha").mask("99/99/9999");

     //Control para abreviar poniendo dia y mes
    $(".textbox_fecha").on('keydown', function (e) {
        var keyCode = e.which;

        //Restriccion con la tecla TAB=9
        if (keyCode === 9) {

            var valor_fecha = this.id;

            if ($("#" + valor_fecha).val().replace("____", "").length == 6) {
                $("#" + valor_fecha).val($("#" + valor_fecha).val() + (new Date).getFullYear())
            }

        }
    });

});
