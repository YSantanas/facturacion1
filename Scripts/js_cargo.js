var p_empresa = "";

$(document).ready(function () {

    //Obtengo el id_empresa
    var id_empresa = $(window.parent.document).find("#txt_id_empresa").val();
    
    $('.textbox_cargo').autocomplete({
        minLength: 1,
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: '../default.aspx/autocompleta_cargo',
                data: "{'prefix': '" + request.term + "','id_empresa': '" + id_empresa + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {

                            label: item,
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
            $('#' + this.id + '').val(ui.item.val);
            return false;
        }
    });

});