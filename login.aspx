<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>OPTIMUS | Facturación</title>
    <link rel="icon" href="imagenes/icono/icono.ico" />
   
    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/alertify.core.css" rel="stylesheet" />
    <link href="Content/alertify.default.css" rel="stylesheet" />
    <link href="Content/bootstrap-icons.css" rel="stylesheet" />

    <!-- JQUERY ------------------------------------------------------------------------------------------------->
    <script src="Scripts/jquery-3.7.1.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <script src="Scripts/alertify.js"></script>
    <script src="Scripts/device.js"></script>

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link href="Content/login.css" rel="stylesheet" />
    <script src="Scripts/login.js"></script>

</head>

<body id="cuerpo" runat="server">
    
    <form id="form1" runat="server">

        <div class="transparent_negro"></div>
        
        <div class="container-fluid">
        
        <!-- Login -->
        <div class="card card-container">

        <p id="profile-name" class="profile-name-card d-flex flex-row justify-content-center alig-items-center">
            <img src="Imagenes/logo/logo_empresa.png" style="height:75px;"/>
        </p>

        <div id="tabreloj" class="text-secondary">
            <asp:Label ID="clock" runat="server" Font-Size="9px"></asp:Label><asp:Label ID="lbl_fecha" runat="server" Font-Size="9px"></asp:Label>
        </div>

        <asp:PlaceHolder ID="PH_datos" runat="server">

        <span id="reauth-email" class="reauth-email"></span>
        <div id="div-login-msg">
            <p>
                <asp:Label ID="lbl_bienvenida" runat="server" Text="" CssClass ="text-secondary d-flex flex-row justify-content-center alig-items-center"></asp:Label>
            </p>
             <div class="input-group mb-3">
                 <span class="input-group-text">
                     <i class="bi bi-envelope-at-fill" style="font-size:20px;"></i>
                 </span>
                 <asp:TextBox type="email" ID="txt_email" class="form-control input-sm" runat="server" placeholder="Correo Electrónico" TextMode="SingleLine"></asp:TextBox>
             </div>

             <div class="input-group mb-3">
                 <span class="input-group-text">
                   <i class="bi bi-lock-fill" style="font-size:20px;"></i>
                 </span>
                 <asp:TextBox type="password" ID="txt_contrasena" class="form-control input-sm" runat="server" placeholder="Contraseña" TextMode="SingleLine"></asp:TextBox>
                 <span class="input-group-text">
                     <i id="ver_pass" class="bi bi-eye text-primary" style="cursor:pointer; font-size:16px;" title ="Ver Password"></i>
                 </span> 
                 <span class="input-group-text">
                     <asp:LinkButton ID="lk_login" runat="server" class="input-sm text-primary" Font-Size="16" title="Acceder"><i class ="bi bi-box-arrow-in-right"></i></asp:LinkButton>
                 </span>
             </div>
        </div>

        <asp:PlaceHolder ID="PH_cargando" runat="server" Visible="false">
     
            <div class="progress" style="height: 5px;">
                <div class="progress-bar bg-primary" role="progressbar" style="width: 1%;" aria-valuenow="1" aria-valuemin="0" aria-valuemax="100"></div>
            </div>
            <p></p>
            <asp:Label ID="lbl_mensaje_trabajando" runat="server" Text="" Font-Size="10px"></asp:Label>
    
        </asp:PlaceHolder>

        <hr style="border-top: 2px dotted #198754 !important;" />

        <table style="width:100%;">
            <tr>
                <td>
                    <a href="recuperar.aspx">
                        <span class="bi bi-unlock text-primary" style="font-size:16px;" title="Recuperar Contraseña"></span> 
                        <span class="text-secondary" style="font-size:14px;" title="Recuperar Contraseña">Recuperar</span> 
                    </a>
                </td>
               <td align="right">
                   <input id="Chk_recuerdame" runat="server" class="form-check-input mt-0" type="checkbox" style="position:relative;top:5px;"/>
                   <label class="custom-control-label text-secondary" for="Chk_recuerdame" style="font-size:14px;" title="Recordar claves para iniciar sessión de manera automática">Recuérdame</label>
                </td>
            </tr>
            <tr>
                <td style="height:10px;"></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <a href="registro.aspx">
                        <span class="bi-box-seam text-primary" style="font-size:16px;" title="Alta"></span> 
                        <span class="text-secondary" style="font-size:14px;" title="Alta">Alta</span> 
                    </a>
                </td>
                <td align="right">
                    <span class="bi bi-person-raised-hand text-primary" style="font-size:16px;" title="Ayuda"></span> 
                    <span class="text-secondary" style="font-size:14px;" title="Alta">Ayuda</span> 
                </td>
            </tr>
            <tr>
                <td style="height:10px;"></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <a href="ASESOR/login.aspx">
                        <span class="bi bi-person-video3 text-primary" style="font-size:16px;" title="Asesor"></span> 
                        <span class="text-secondary" style="font-size:14px;" title="Acceder al módulo Asesor">Asesor</span> 
                    </a>
                </td>
                <td>

                </td>
           </tr>
        </table>
        
        </asp:PlaceHolder>

         <table style="position:relative; bottom :-40px;">
         <tr>
             <td><span class="text-secondary" style="font-size:9px;">&copy; <% =Year(Date.Now) %> Optimiza Gestores Documentales S.L.</span></td>
             <td><span id="img_dispositivo" class="text-primary" style="font-size :20px;"></span></td>
         </tr>
         </table>

        </div>
            
        </div>

        <!-- Modal Multi-Empresa-->
        <div class="modal fade modal-xl" id="modal_multi_empresa" data-bs-keyboard="true" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title fs-5" id="exampleModalLabel">Seleccione Empresa</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body text-center">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="basic-addon1">Filtrar:</span>
                            </div>
                            <input type="text" class="form-control" id="filter" placeholder="Buscar por Nombre o Código de Empresa" aria-label="Username" aria-describedby="basic-addon1" />
                        </div>
                        <br />
                        <div class="table-responsive">
                            <asp:GridView ID="gridview_empresa"
                                AutoGenerateColumns="False" runat="server" ShowHeaderWhenEmpty="True"
                                CssClass="table table-condensed" Font-Size="12px" Width="100%"
                                CellPadding="2" ForeColor="#333333"
                                DataKeyNames="id_usuario,id,ruta_base_datos,Id_tipo_plan_cuentas,nombre_comercial,demo,fecha_creacion,servicio_suspendido,bienvenida,custodia,baja"
                                GridLines="None">
                                <AlternatingRowStyle BackColor="White" />

                                <Columns>
                                    <asp:TemplateField HeaderText="Logo">
                                        <ItemTemplate>
                                            <asp:Image ID="imgestado" runat="server" ImageUrl="~/imagenes/logo/sin_logo.png" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvHeaderCenter" Width="70" />
                                        <ItemStyle CssClass="gvHeaderCenter" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ruta_base_datos" HeaderText="BBDD"
                                        SortExpression="ruta_base_datos">
                                        <HeaderStyle CssClass="gvHeaderCenter" />
                                        <ItemStyle CssClass="gvHeaderCenter" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="codigo_empresa" HeaderText="Código"
                                        SortExpression="codigo_empresa">
                                        <HeaderStyle CssClass="gvHeaderCenter" />
                                        <ItemStyle CssClass="gvHeaderCenter" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nombre_fiscal" HeaderText="Denominación"
                                        SortExpression="nombre_fiscal">
                                        <HeaderStyle CssClass="gvHeaderleft" />
                                        <ItemStyle CssClass="gvHeaderleft" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nif" HeaderText="N.I.F."
                                        SortExpression="nif">
                                        <HeaderStyle CssClass="gvHeaderleft" />
                                        <ItemStyle CssClass="gvHeaderleft" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nivel" HeaderText="Nivel"
                                        SortExpression="nivel">
                                        <HeaderStyle CssClass="gvHeaderleft" />
                                        <ItemStyle CssClass="gvHeaderleft" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                             <asp:LinkButton ID ="lk_login" runat ="server" 
                                                 CssClass ="bi bi-box-arrow-in-right text-primary" 
                                                 Font-Size="25px"
                                                 CommandName="Select" 
                                                 ToolTip ="Entrar" TabIndex="-1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                                <FooterStyle CssClass ="table-primary"/>
                                <HeaderStyle CssClass ="table-primary"/>
                                <PagerStyle CssClass ="table-primary"/>

                            </asp:GridView>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Mantenimiento-->
        <div class="modal fade bd-example-modal-sm " id="modal_mantenimiento" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title">Aviso</h5>
                    </div>
                    <div class="modal-body text-center">

                        <h4>Estamos realizando tareas de mantenimiento <br />para mejorar aún más nuestro servicio.
                            <br />
                            El sistema estará disponible en 
                    <asp:Label ID="lbl_tiempo" runat="server" Text=""></asp:Label>
                            minuto(s).<br />
                            Por favor inténtelo de nuevo más tarde.
                        <br />
                            Disculpen las molestias.
                        </h4>
                        <p></p>

                    </div>
                </div>
            </div>
        </div>
 
        <div id="partes_hidden" style="visibility:hidden; display: none;">
            txt_tipo_dispositivo:<asp:TextBox ID="txt_tipo_dispositivo" runat="server" Width="400"></asp:TextBox><br />
        </div>

    </form>
</body>
</html>