<%@ Page Language="VB" AutoEventWireup="false" CodeFile="backupcloud.aspx.vb" Inherits="supervisor_backupcloud" %>

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

        $(document).ready(function () {

            //Desactivo la funcion F5 (refresco)
            shortcut.add("F5", function () { });

            //Quitar Enter
            $("form").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });

            //Eliminamos el click derecho de ratón para las multiples pantallas
            $('html').bind("contextmenu", function (e) {
                return false;
            });

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_si_restaurar").on('click', function (e) {

                //Mensaje al usuario
                $('#modal_restaurar').modal('hide');
                mostrar_trabajando('Comenzando Restauración, por favor espere.');
                
            })

            //-------------------------------------------------------------------------------------------------------------------
            //Control para el click
            $("#btn_agregar_si").on('click', function (e) {

                //Mensaje al usuario
                $('#modal_solicitar_datos').modal('hide');
                mostrar_trabajando('Comenzando Copia de Seguridad, por favor espere.');
             
            })

        });

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

    .calendarWrapper {
        background-color: #b3b3b3;
        padding: 2px;
        display: inline-block;
    }

    .myCalendar {
        background-color: #ffffff;
        width: 300px;
        border: none !important;
    }

    .myCalendar a {
        text-decoration: none;
    }

    .myCalendar .myCalendarTitle {
        font-weight: 400;
        height: 40px;
        line-height: 40px;
        background-color: #cce5ff;
        color: #000000;
        border: none !important;
    }

    .myCalendar th.myCalendarDayHeader {
        height: 25px;
        text-align :center;
    }

    .myCalendar tr {
        border-bottom: solid 1px #ddd;
    }

    .myCalendar table tr {
        border-bottom: none !important;
    }

    .myCalendar tr:last-child td {
        border-bottom: none;
    }

    .myCalendar tr td.myCalendarDay, .myCalendar tr th.myCalendarDayHeader {
        border-right: solid 1px #ddd;
    }

    .myCalendar tr td:last-child.myCalendarDay, .myCalendar tr th:last-child.myCalendarDayHeader {
        border-right: none;
    }

    .myCalendar td.myCalendarDay:nth-child(7) a {
        color: #c52e2e !important;
    }

    .myCalendar .myCalendarNextPrev {
        text-align: center;
    }

    .myCalendar .myCalendarNextPrev a {
        font-size: 1px;
    }

    .myCalendar .myCalendarNextPrev:nth-child(1) a {
        color: black !important;
        font-family: 'Material Icons';
        content: "\e5cf";
        font-size :20px;
        display: inline-block;
    }

    .myCalendar .myCalendarNextPrev:nth-child(1) a:hover, .myCalendar .myCalendarNextPrev:nth-child(3) a:hover {
        background-color: transparent;
    }

    .myCalendar .myCalendarNextPrev:nth-child(3) a {
        color: black !important;
        font-family: 'Material Icons';
        content: "\e5cb";
        font-size :20px;
        display: inline-block; 
    }

    .myCalendar td.myCalendarSelector a {
        background-color: #0d6efd;
    }

    .myCalendar .myCalendarDayHeader a,
    .myCalendar .myCalendarDay a,
    .myCalendar .myCalendarSelector a,
    .myCalendar .myCalendarNextPrev a {
        display: block;
        line-height: 20px;
        color: white;
    }

    .myCalendar .myCalendarToday {
        background-color: #f2f2f2;
        -webkit-box-shadow: 1px 1px 8px 1px #8f8f8f;
        box-shadow: 1px 1px 8px 1px #8f8f8f;
        display: inline-block;
        width: 42px !important;
        height: 19px !important;
        border: 2px solid #f2f2f2;
        margin-left: -1px;
        margin-top: -1px;
        position: relative;
    }

    .myCalendar .myCalendarToday a {
        color: #004085 !important;
        font-weight: bold;
    }

    .myCalendar .myCalendarToday a:after {
        content: "HOY";
        color: #000;
        font-size: 0.5em;
        display: inline-block;
        pointer-events: none;
        width: 100%;
        float: left;
    }

    .myCalendar .myCalendarDay a:hover,
    .myCalendar .myCalendarSelector a:hover {
        background-color: #cce5ff;
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

            <h4>BackupCloud</h4>
            <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        
            <div class="table-responsive">

            <table style="width:100%;">
            <tr>
                <td valign="top" style="width:320px;border:0px solid red;">
                    <div class="calendarWrapper">
                    <!--Calendar here -->
                    <asp:Calendar ID="Calendar1" runat="server"  Font-Names="Tahoma" 
                        Font-Size="11px" NextMonthText="&gt;" PrevMonthText="&lt;" SelectMonthText="»" 
                        SelectWeekText="›" CssClass="myCalendar" CellPadding="0">
                        <OtherMonthDayStyle ForeColor="#b0b0b0" />
                        <DayStyle CssClass="myCalendarDay" ForeColor="#2d3338" />
                        <DayHeaderStyle CssClass="myCalendarDayHeader" ForeColor="#2d3338" />
                        <SelectedDayStyle Font-Bold="True" Font-Size="12px" CssClass="myCalendarSelector" />
                        <TodayDayStyle CssClass="myCalendarToday" />
                        <SelectorStyle CssClass="myCalendarSelector" />
                        <NextPrevStyle CssClass="myCalendarNextPrev" />
                        <TitleStyle CssClass="myCalendarTitle" />
                    </asp:Calendar>
                    </div>
                    <br />
                    <br />
                    <div class="container" style="text-align:center">
                        <asp:Button ID="btn_agregar" runat="server" Text="Generar BackupCloud" CssClass="btn btn-outline-primary btn-sm" visible="false" /> 
                    </div>    
                    <hr />
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Mantener la copia durante:
                            </span>
                        </div> 
                        <asp:DropDownList ID="DDL_dias_backup" runat="server" CssClass="form-control" AutoPostBack="true">
                            <asp:ListItem Value="-7" Text="7 Días"></asp:ListItem>
                            <asp:ListItem Value="-15" Text="15 Días"></asp:ListItem>
                            <asp:ListItem Value="-30" Text="30 Días"></asp:ListItem>
                            <asp:ListItem Value="-60" Text="60 Días"></asp:ListItem>
                            <asp:ListItem Value="-90" Text="90 Días"></asp:ListItem>
                        </asp:DropDownList>
                    </div>


                </td>
                <td style="width:100px;"></td>
                <td valign="top">

                    <div class="table-responsive">

                    <asp:GridView ID="gridview_consulta"
                    AutoGenerateColumns="false" runat="server"  
                    cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Puntos de Restauración" EmptyDataText="No hay resultados."        
                    CellPadding="4" ForeColor="#333333" GridLines="Both" 
                    DataKeyNames="fecha,hora,descripcion,nombre_fichero,ruta_completa_fichero" ShowHeaderWhenEmpty="false">
                    <AlternatingRowStyle BackColor="White" />
                
                    <Columns>
                    
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" 
                            SortExpression="fecha" ReadOnly="True" ItemStyle-HorizontalAlign="left">
                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="hora" HeaderText="Hora" 
                            SortExpression="hora" ReadOnly="True" ItemStyle-HorizontalAlign="left">
                        <HeaderStyle HorizontalAlign="Left" Width="70px" />
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" 
                            SortExpression="descripcion" ReadOnly="True" ItemStyle-HorizontalAlign="left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>

                        <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
                        <ItemTemplate>
                            <asp:Button ID="btnrestaurar" 
                            ControlStyle-CssClass="btn btn-outline-primary btn-sm" runat="server" 
                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                            CommandName="restaurar" Text="Restaurar" TabIndex="-1"/>
                        </ItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False" HeaderText="" HeaderStyle-Width="80">
                        <ItemTemplate>
                            <asp:Button ID="btndelete" 
                            ControlStyle-CssClass="btn btn-outline-danger btn-sm" runat="server" 
                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                            CommandName="borrar" Text="Eliminar" TabIndex="-1"/>
                        </ItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                                               
                    </Columns>
                
                        <HeaderStyle CssClass ="table-primary"/>
                        <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />
                        
                    </asp:GridView>

                    </div>

                </td>
            </tr>
            </table>

            </div>

        </div>

        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />
             
        <!-- Modal Eliminar-->
        <div class="modal" id="modal_eliminar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
          <div class="modal-dialog">

	        <div class="modal-content">
		        
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Eliminar</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

		        <div class="modal-body">
                    <asp:Literal ID="lt_mensaje_eliminar" runat="server"></asp:Literal>
                    <br />
		        </div>
		        <div class="modal-footer">
            
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <asp:Button ID="btn_si_eliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger"/>

              </div>
	        </div>
          </div>
        </div>

        <!-- Modal Restaurar-->
        <div class="modal" id="modal_restaurar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
          <div class="modal-dialog">

	        <div class="modal-content">

                <div class="modal-header">
                    <h1 class="modal-title fs-5">Restaurar</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
		        
		        <div class="modal-body">
                    <asp:Literal ID="lt_mensaje_restaurar" runat="server"></asp:Literal>
                    <br /><br />
                    <span class="text-secondary"><small>Nota: Una vez finalizada la restauración el sistema se reiniciará.</small></span>
		        </div>
		        <div class="modal-footer">
            
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <asp:Button ID="btn_si_restaurar" runat="server" Text="Restaurar" CssClass="btn btn-primary"/>

              </div>
	        </div>
          </div>
        </div>

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

        <!-- Modal Solicitar Datos-->
        <div class="modal" id="modal_solicitar_datos" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
          <div class="modal-dialog">

	        <div class="modal-content">

                <div class="modal-header">
                    <h1 class="modal-title fs-5">Solicitud de datos</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
		    
		        <div class="modal-body">

                    <div class="input-group input-group-sm mb-3">
                      <div class="input-group-prepend">
                        <span class="input-group-text">Descripción:</span>
                      </div>
                      <asp:TextBox ID="txt_descripcion" runat="server" class="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                    <span class="text-secondary"><small>Max. 50 caracteres.</small></span>
		        </div>
		        <div class="modal-footer">
            
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <asp:Button ID="btn_agregar_si" runat="server" Text="Crear" CssClass="btn btn-primary"/>

              </div>
	        </div>
          </div>
        </div>

        <div id="partes_hidden" style="visibility:hidden; display: none; ">
            txt_index:<asp:TextBox ID="txt_index" runat="server" Width="400"></asp:TextBox><br />
        </div>

    </form>
</body>
</html>
