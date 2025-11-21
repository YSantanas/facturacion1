<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>PANEL CONTROL | FACTURACIÓN</title>
    <link rel="icon" href="../imagenes/icono/icono.ico" />
   
    <!-- CSS ---------------------------------------------------------------------------------------------------->
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Content/alertify.core.css" rel="stylesheet" />
    <link href="../Content/alertify.default.css" rel="stylesheet" />
    <link href="../Content/bootstrap-icons.css" rel="stylesheet" />

    <!-- JQUERY ------------------------------------------------------------------------------------------------->
    <script src="../Scripts/jquery-3.7.1.js"></script>
    <script src="../Scripts/bootstrap.js"></script>
    <script src="../Scripts/alertify.js"></script>
    <script src="../Scripts/device.js"></script>

    <!-- PERSONALIZADOS ----------------------------------------------------------------------------------------->
    <link href="../Content/login.css" rel="stylesheet" />
    <script src="../Scripts/login.js"></script>

</head>

<body id="cuerpo" runat="server">
    
    <form id="form1" runat="server">

        <div class="transparent_negro"></div>
        
        <div class="container-fluid">

        <!-- Login -->
        <div class="card card-container">

        <p id="profile-name" class="profile-name-card d-flex flex-row justify-content-center alig-items-center">
            <img src="../Imagenes/logo/logo_empresa.png" style="height:75px;"/>
        </p>
            
         <div id="tabreloj" class="text-secondary">
             <asp:Label ID="clock" runat="server" Font-Size="9px"></asp:Label><asp:Label ID="lbl_fecha" runat="server" Font-Size="9px"></asp:Label>
         </div>
        
        <table style="width:100%;">
        <tr>
            <td align="center" class="text-primary fw-bold">PANEL DE CONTROL</td>
        </tr>
        </table>
            
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
                 <asp:TextBox type="email" ID="txt_nombre" class="form-control input-sm" runat="server" placeholder="Correo Electrónico" TextMode="SingleLine"></asp:TextBox>
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
        
        </asp:PlaceHolder>

         <table style="position:relative; bottom :-40px;">
         <tr>
             <td><span class="text-secondary" style="font-size:9px;">&copy; <% =Year(Date.Now) %> Optimiza Gestores Documentales S.L.</span></td>
             <td><span id="img_dispositivo" class="text-primary" style="font-size :20px;"></span></td>
         </tr>
         </table>

        </div>
            
        </div>
 
        <div id="partes_hidden" style="visibility:hidden; display: none;">
            txt_tipo_dispositivo:<asp:TextBox ID="txt_tipo_dispositivo" runat="server" Width="400"></asp:TextBox><br />
        </div>

    </form>
</body>
</html>