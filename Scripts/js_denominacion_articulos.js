var p_empresa = "";

$(document).ready(function () {

    //Obtengo el id_empresa
    var id_empresa = $(window.parent.document).find("#txt_id_empresa").val();
    
    $('.textbox_denominacion_articulos').autocomplete({
        minLength: 3,
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: '../default.aspx/autocompleta_denominacion_articulo',
                data: "{'prefix': '" + request.term + "','id_empresa': '" + id_empresa + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        var compuesto = item.split("|");
                        return {

                            label: "Cod: " + compuesto[0] + " - " + compuesto[1],
                            val: item
                        };
                    }));
                },
                error: function (response) { alert(response.responseText); },
                failure: function (response) { alert(response.responseText); }
            });
        },
        focus: function (event, ui) {
            return false;
        },
        select: function (event, ui) {
            //descompongo resultados
            var compuesto = ui.item.val.split("|");
            $('#txt_codigo').val(compuesto[0]);
            return false;
        }
    });

});