<%@ Page Language="VB" AutoEventWireup="false" CodeFile="actualizar_bbdd.aspx.vb" Inherits="CONTROL_herramientas_actualizar_bbdd" %>

<%@ Register Src="~/cuadro_trabajando.ascx" TagPrefix="uc1" TagName="cuadro_trabajando" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <title></title>

    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/alertify.core.css" rel="stylesheet" />
    <link href="../../Content/alertify.default.css" rel="stylesheet" />
    <link href="../../Content/bootstrap-icons.css" rel="stylesheet" />
    <link href="../../Content/jquery-ui.css" rel="stylesheet" />

    <!-- JQUERY ------------------------------------------------------------------------------------------------->
    <script src="../../Scripts/jquery-3.7.1.js"></script>
    <script src="../../Scripts/bootstrap.js"></script>
    <script src="../../Scripts/alertify.js"></script>
    <script src="../../Scripts/shortcut.js"></script>
    <script src="../../Scripts/jquery-ui-1-13.3.js"></script>

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link href="../../Content/interior.css" rel="stylesheet" />
    <script src="../../Scripts/interior.js"></script>

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

             //-------------------------------------------------------------------------------------------------------------------
             //Control para el click
             $("#btn_anadir_actualizacion").on('click', function (e) {

                 if ($("#txt_comentario").val() == '') {
                     error('El campo Descripción no puede estar vacío.')
                     setTimeout(function () { $("#txt_comentario").focus(); }, 100);
                     return false;
                 };

                 if ($("#txt_texto").val() == '') {
                     error('El campo Instrucción SQL no puede estar vacío.')
                     setTimeout(function () { $("#txt_texto").focus(); }, 100);
                     return false;
                 };

             })

             //-------------------------------------------------------------------------------------------------------------------
             //Control para el click
             $("#btn_si").on('click', function (e) {

                 //Ocultar
                 $('#modal_eliminar').modal('hide');

                 //Mensaje al usuario
                 mostrar_trabajando('Ejecutando SQL, por favor espere.');

             })
        });

    </script>

</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid">

        <h5>SQL Server</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <br />
        
        <span style="color:white;"><asp:Literal ID="lt_error" runat="server"></asp:Literal></span>
        <div class="input-group input-group-sm mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text">
                    Descripción:
                </span>
            </div>
            <asp:TextBox ID="txt_comentario" runat="server" class="form-control" placeholder="Descripción" MaxLength="250"></asp:TextBox>
        </div>
        
        <div class="input-group">
          <span class="input-group-text">Instrucción SQL:</span>
            <asp:TextBox ID="txt_texto" runat="server" class="form-control" TextMode="MultiLine" Enabled="true" Height="250"></asp:TextBox>
        </div>
        
        <br />
        <div class="container-fluid" style="text-align:center">
            <asp:Button ID="btn_anadir_actualizacion" runat="server" Text="Actualizar" CssClass="btn btn-outline-primary btn-xs" />
        </div> 
        <br />

        <div class="table-responsive">

            <asp:GridView ID="gridview_consulta"  
            AutoGenerateColumns="false" runat="server" 
            cssclass="table table-condensed" Font-Size="11px" Width="100%" ToolTip="Consulta de Artículos" EmptyDataText="No hay resultados."
            CellPadding="4" ForeColor="#333333" GridLines="Both" DataKeyNames="Id,texto" 
            ShowHeaderWhenEmpty="false" AllowPaging="true" PageSize="25">
            <AlternatingRowStyle BackColor="White" />
           
            <Columns>

                <asp:BoundField DataField="fecha" HeaderText="Fecha" 
                    SortExpression="fecha" DataFormatString="{0:dd-MM-yyyy}" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Left"/>
                </asp:BoundField>

                <asp:BoundField DataField="hora" HeaderText="Hora" 
                    SortExpression="hora" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Left" Width="70px" />
                </asp:BoundField>
            
                <asp:BoundField DataField="comentario" HeaderText="Comentario" 
                    SortExpression="comentario" ReadOnly="False">
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>

            </Columns>
                  
                <HeaderStyle CssClass ="table-primary"/>
                <PagerStyle HorizontalAlign = "Center" CssClass="GridPager" />

            </asp:GridView>

        </div>

    </div> 
     
    <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando"/>

    <!-- Modal Eliminar-->
    <div class="modal" id="modal_eliminar" tabindex="-1" role="dialog">
        <div class="modal-dialog">

	        <div class="modal-content">
                
                <div class="modal-header">
                    <h1 class="modal-title fs-5">! ATENCIÓN ¡</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="LT_mensaje" runat="server"></asp:Literal>
                </div>
                    
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button id="btn_si" runat="server" CssClass="btn btn-danger" Text="Aceptar" />
                </div>
            </div>
        </div>
    </div>

    </form>
</body>
</html>
