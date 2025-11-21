<%@ Page Language="VB" AutoEventWireup="false" CodeFile="errores.aspx.vb" Inherits="actualizaciones_errores" EnableEventValidation="false" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link href="../Content46/bootstrap.css" rel="stylesheet" />
    <link href="../Content46/alertifyjs/alertify.min.css" rel="stylesheet" />

    <!-- JQUERY-------------------------------------------------------------------------------------------------->    
    <script src="../Scripts46/jquery-3.6.0.js"></script>
    <script src="../Scripts46/bootstrap.js"></script>
    <script src="../Scripts46/alertify.js"></script>
    <script src="../Scripts46/shortcut.js"></script>
    <script src="../Scripts46/jquery-resizable.js"></script>

    <!-- PERSONAL---------------------------------------------------------------------------------------------------->     
    <link href="../Content46/interior.css" rel="stylesheet" />
    <script src="../Scripts46/interior.js"></script>
  
    <script type="text/javascript">

        $(document).ready(function () {

            //Desactivo la funcion F5 (refresco)
            shortcut.add("F5", function () { });

            //Quitar Enter
            $("form").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });

            ////Eliminamos el click derecho de ratón para las multiples pantallas
            //$('html').bind("contextmenu", function (e) {
            //    return false;
            //});

            //Cambio de contenedor
            $(".panel-container").css('height', $(window).height() - 90 + 'px');

            //Resize Panel
            $(".panel-left").resizable({
                handleSelector: ".splitter",
                resizeHeight: false
            });

            //Control para focus
            $("#btn_consultar").focusin(function () {
                $("#btn_consultar").click();
            });

            //Control para el click
            $("#btn_consultar").on('click', function () {

                //Mensaje al usuario
                mostrar_trabajando('Consultando registros, por favor espere.');

            })

            //Control para el click
            $("#recoger").on('click', function (e) {

                $(".panel-left").toggle(500);

            })
            
        });

        //Tamaño para el desktop
        $(window).resize(function (e) {

            $(".panel-container").css ('height', $(window).height() - 90 + 'px' );
      
        })

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      
    <div class="container-fluid">
      
        <h5><span id="recoger" class="material-icons" style="color: #155724;" onmouseover="hand('recoger')" title="Contraer/Expandir menú">swap_horiz</span> Extraer Cuentas para Plantillas PDF</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
       
        <div class="panel-container">

            <div class="panel-left">
                
            <div class="input-group input-group-sm mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text">
                    <asp:ImageButton ID="img_reset" runat="server" ImageUrl="../imagenes/web/Command-Reset.png" style="width: 10px;" ToolTip="Refrescar Usuarios" OnClientClick ="mostrar_trabajando('Actualizando usuarios, por favor espere.');"/>&nbsp;&nbsp;Ejercicio:
                </span>
            </div>
            <asp:DropDownList ID="DDL_ejercicio" runat="server" class="form-control"></asp:DropDownList>
            <div style="position:absolute; top:35px;color:#155724;font-size: 12px;height:20px;">
            <asp:Label ID="lbl_fechas_proceso" runat="server" Text=""></asp:Label>
            </div>
            </div>
                
            <br />
            <div class="container-fluid" style="text-align:center">
                <asp:Button ID="btn_consultar" runat="server" CssClass="btn btn-outline-success btn-sm" Text="Obtener cuentas mas usadas" />
            </div>

            </div>

            <div class="splitter"></div>
            
            <div class="panel-right">
       
            <div class="container-fluid" style="text-align:center">
                <table style="width :100%;border:0px solid red;">
                <tr>
                    <td style="width:50%;border: 0px solid blue; text-align:left;">
                        <asp:ImageButton ID="img_exportar_excel" src="../imagenes/web/xls_icono.png" runat="server" height="25" ToolTip="Descargar Excel" TabIndex ="-1" Visible="false"/>
                        <asp:ImageButton ID="img_exportar_txt" src="../Imagenes/web/txt_icono.png" runat="server" height="25" ToolTip="Descargar TXT" TabIndex ="-1" Visible="false"/>
                    </td>
                    
                    <td style="width:50%; text-align:right;"><span style="font-size: 9px;"><asp:Label ID="lbl_informacion" runat="server" Text="" Visible ="false"></asp:Label></span></td>
                </tr>
                </table>
            </div>

                <div class="table-responsive">
                
                <asp:GridView ID="gridview_consulta"  
                AutoGenerateColumns="false" runat="server" 
                cssclass="table-bordered" Font-Size="11px" Width="100%" ToolTip="Registros de Usuarios" EmptyDataText="No hay resultados."
                CellPadding="4" ForeColor="#333333" GridLines="Both" 
                ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
                <AlternatingRowStyle BackColor="White" />
            
                <Columns>

                    <asp:BoundField DataField="contador" HeaderText="Nº Veces"
                        SortExpression="contador" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="nif" HeaderText="N.I.F."
                        SortExpression="nif" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="denominacion" HeaderText="Denominación"
                        SortExpression="denominacion" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"/>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                   
                <HeaderStyle BackColor="#d4edda" Font-Bold="True" ForeColor="black" />
                <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

                </asp:GridView>
                
                </div>

            </div>

        </div>

    </div>
        
    <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />

    <!-- Modal Sin Acceso-->
    <div class="modal modal-static fade" id="modal_sin_acceso" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-body">

                    <table style="width:100%;">
                        <tr>
                            <td align="center">
                                <span class="material-icons" style="font-size:60px;color:red;">person_off</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px;"></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <h5><span style="color:#155724">Su nivel de clave no permite acceder a este menú.</span></h5>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>
        
    <div id="partes_hidden" style="visibility:hidden; display: none; ">
        txt_fecha_inicial<asp:TextBox ID="txt_fecha_inicial" runat="server" text="" Width="400"></asp:TextBox><br />
        txt_fecha_final:<asp:TextBox ID="txt_fecha_final" runat="server" Width="400"></asp:TextBox><br />
        txt_index:<asp:TextBox ID="txt_index" runat="server" Width="400"></asp:TextBox><br />
        txt_cuenta_original:<asp:TextBox ID="txt_cuenta_original" runat="server" Width="400"></asp:TextBox><br />
        txt_cuenta_destino:<asp:TextBox ID="txt_cuenta_destino" runat="server" Width="400"></asp:TextBox><br />
        txt_denominacion_destino:<asp:TextBox ID="txt_denominacion_destino" runat="server" Width="400"></asp:TextBox><br />
        txt_nif_destino:<asp:TextBox ID="txt_nif_destino" runat="server" Width="400"></asp:TextBox><br />
    </div>

    </form>
</body>
</html>

