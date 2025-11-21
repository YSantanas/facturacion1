<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ping.aspx.vb" Inherits="herramientas_ping" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <title></title>

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
    <script src="../Scripts/device.js"></script>

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link href="../Content/interior.css" rel="stylesheet" />
    <script src="../Scripts/interior.js"></script>

    <script>

        //url = "http://www.optimiza-facturacion.es/";

        $(document).ready(function () {

            //Desactivo la funcion F5 (refresco)
            shortcut.add("F5", function () { });

            //Quitar Enter
            $("form").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });

            //Imagen inicial del nivel de WIFI
            $("#imagen_nivel").append("<span class='bi bi-wifi-off color' title='Sin señal de Internet'></span>");

            var intervalTime = null;

            function ping(x) {
            
            var s = Number(x); //Get number of times to run
            var ping_avg = Number(); //sets start int
            var c = Number(1); //sets counter
            //console.log(c, ping_avg, s);
            intervalTime = setInterval(function () {
                var start = new Date().getTime(); //gets time of star
                $.ajax({
                    url: "http://www.optimiza-facturacion.es",
                    cache: false,
                    global: false
                }).always(function () {
                    msec = new Date().getTime() - start; //sets lenght of time in millinsecond
                    ping_avg = Number(msec) + ping_avg; //add's all up to get average
                    $("<i>").text(c + ": ping: " + msec + "ms").appendTo("#p_list"); // shows ping list
                    $("<br>").appendTo("#p_list");
                    var pingms = ping_avg / c; //averages ping
                    pingms = pingms.toFixed(2); //rounds off
                    $("#avg_ping").html("Ping: " + pingms + "ms / " + c); //shows ping
                    //console.log("Count: " + c + " | Current: " + msec + " | Average: " + pingms + "ms");
                    //console.log("count" + c);
                    //Asigno
                    c = c + 1;
                });
            }, 1000);
            return true;
        }

            ping(100);

            //Reviso velocidad de la red
            velocidad_red();

        });

        function velocidad_red() {
   
            var imageAddr = "http://www.optimiza-facturacion.es/imagenes/web/test_velocidad.jpg";
            var downloadSize = 496880; //bytes

            var startTime, endTime;
            var download = new Image();
            download.onload = function () {
                endTime = (new Date()).getTime();
                showResults();
            };

            download.onerror = function (err, msg) {

                $("#imagen_nivel").empty();
                $("#imagen_nivel").append("<span class='bi bi-wifi-off color' title='Sin señal de Internet'></span>");
                
            };

            startTime = (new Date()).getTime();
            var cacheBuster = "?nnn=" + startTime;
            download.src = imageAddr + cacheBuster;

            function showResults() {
                var duration = (endTime - startTime) / 1000;
                var bitsLoaded = downloadSize * 8;
                var speedBps = (bitsLoaded / duration).toFixed(2);
                var speedKbps = (speedBps / 1024).toFixed(2);
                var speedMbps = (speedKbps / 1024).toFixed(2);


                $("#imagen_nivel").empty();
                

                //network_wifi_2_bar
                if (speedMbps < 5) {
                    $("#imagen_nivel").append("<span class='bi bi-wifi-1 color'></span>");
                }
                if (speedMbps > 5 && speedMbps < 50) {
                    $("#imagen_nivel").append("<span class='bi bi-wifi-2 color'></span>");
                }
                if (speedMbps > 50) {
                    $("#imagen_nivel").append("<span class='bi bi-wifi color'></span>");
                }

                $("#imagen_nivel").attr('title', speedMbps + " Mbps");
                $("#lbl_velocidad_conexion").text(speedMbps + " Mbps");

            };

            //actualizo mi método cada 60 segundo 
            window.setTimeout("velocidad_red();", 15000);

        }

        function volver_ventana() {

            //Declaro
            padre = $(window.parent.document);
            var id_usuario = $(padre).find("#txt_id_usuario").val();
            var id_empresa = $(padre).find("#txt_id_empresa").val();
            var ruta = "herramientas/herramientas.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;

            //Informo de resolución de pantalla baja, para PC
            if (device.mobile() === false && device.tablet() === false) {

                $(padre).find("#iherramientas").attr('src', ruta);

            } else {

                $(padre).find("#iframe").attr('src', ruta);

            }

        }

    </script>

    <style>

        .color {
            font-size :60px;
            color: #0d6efd;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
        
        <table id="regresar" onclick="volver_ventana();" onmouseover="hand(this.id);">
        <tr>
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #0d6efd;"></span>Menú Herramientas</td>
        </tr>
        </table>

        <div class="container-fluid">

            <h5>Test de Velocidad</h5>
            <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>

            <table style="width:100%; border-spacing: 25px; border-collapse: separate;">
            <tr>
                <td style="background-color:white;padding:10px;">
                    <h4 id="timep" class="text-primary"><b id="avg_ping"></b></h4> 
                    <div id="p_list" class="text-secondary"></div>
                </td>
                <td valign="top" align="center">

                    <table style="border:0px solid red;">
                    <tr>
                        <td><div id="imagen_nivel"></div></td>
                        <td style="width:30px;"></td>
                        <td><span id="lbl_velocidad_conexion" style="font-size:25px; color:#0d6efd">Actualizando...</span></td>
                    </tr>
                    </table>

                </td>
            </tr>
            </table>

        </div> 

    </form>
</body>
</html>
