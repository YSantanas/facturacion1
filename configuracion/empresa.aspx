<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empresa.aspx.vb" Inherits="configuracion_empresa" %>

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
    <link href="../Content/fileinput.css" rel="stylesheet" />
    <script src="../Scripts/fileinput.js"></script>
    <script src="../Scripts/locales/es.js"></script>
     
    <script>

        $(document).ready(function () {

            //Desactivo la funcion F5 (refresco)
            shortcut.add("F5", function () { });

            //Control para el click
            $("#btn_grabar_direccion").on('click', function () {

                if ($("#txt_nombre_fiscal").val() == '') {
                    error('El campo Nombre Fiscal no puede estar vacío.')
                    setTimeout(function () { $("#txt_nombre_fiscal").focus(); }, 100);
                    return false;
                }

                if ($("#txt_nombre_comercial").val() == '') {
                    error('El campo Nombre Comercial no puede estar vacío.')
                    setTimeout(function () { $("#txt_nombre_comercial").focus(); }, 100);
                    return false;
                }
                               
                //Mensaje al usuario
                mostrar_trabajando('Actualizando sección Dirección, por favor espere.');
                
            })

            //Control para el click
            $("#btn_grabar_facturas").on('click', function () {
                        
                //Mensaje al usuario
                mostrar_trabajando('Actualizando sección Factura, por favor espere.');
                
            })

            //Control para el click
            $("#btn_grabar_informes").on('click', function () {
                        
                //Mensaje al usuario
                mostrar_trabajando('Actualizando sección Informes, por favor espere.');
                
            })

            //Control para el click
            $("#btn_grabar_backupcloud").on('click', function () {
                        
                //Mensaje al usuario
                mostrar_trabajando('Actualizando sección BackupCloud, por favor espere.');
                
            })

        });
                
       //Notificación de actualizacion de direccion en Notificaciones
        function actualizar_notificacion(nombre_fiscal, nif, telefono, email, informacion_extra) {

            padre = $(window.parent.document);
            
            $(padre).find("#lblempresa").text(nombre_fiscal);
            $(padre).find("#lblnif").text(nif);
            $(padre).find("#lbltelefono").text(telefono);
            $(padre).find("#lblemail").text(email);

            //Extension de la informacion
            $(padre).find("#lbl_informacion_extra").text(informacion_extra);

        };

        function volver_ventana() {

            //Declaro
            padre = $(window.parent.document);
            var id_usuario = $(padre).find("#txt_id_usuario").val();
            var id_empresa = $(padre).find("#txt_id_empresa").val();
            var ruta = "configuracion/configuracion.aspx?id_usuario=" + id_usuario + "&id_empresa=" + id_empresa;

            //Informo de resolución de pantalla baja, para PC
            if (device.mobile() === false && device.tablet() === false) {

                $(padre).find("#iconfiguracion_").attr('src', ruta);
                
            } else {

                $(padre).find("#iframe").attr('src', ruta);

            }

        }

    </script>

</head>
<body>
    
    <form id="form1" runat="server">
        
        <table id="regresar" onclick="volver_ventana();" onmouseover="hand(this.id);">
        <tr>
            <td style="height:50px;"><span class="bi bi-arrow-bar-left" style="margin-left:20px;margin-right:10px;color: #007bff;"></span>Menú Configuración</td>
        </tr>
        </table>

        <div class="container-fluid">

        <h5>Empresa</h5>
        <div class="posicion_hora"><asp:Label ID="lbl_conexion" runat="server" Text="..." Font-Size="9"></asp:Label></div>
        <br />


        <div class="card">
            <div class="card-header">

            <div class="nav nav-tabs" id="nav-tab" role="tablist">
                
                <button class="nav-link active" id="nav-direccion-tab" data-bs-toggle="tab" data-bs-target="#nav-direccion" type="button" role="tab" aria-controls="nav-direccion" aria-selected="true">Dirección</button>
                <button class="nav-link" id="nav-facturas-tab" data-bs-toggle="tab" data-bs-target="#nav-facturas" type="button" role="tab" aria-controls="nav-facturas" aria-selected="false">Facturas</button>
                <button class="nav-link" id="nav-albaranes-tab" data-bs-toggle="tab" data-bs-target="#nav-albaranes" type="button" role="tab" aria-controls="nav-albaranes" aria-selected="false">Albaranes</button>
                <button class="nav-link" id="nav-informes-tab" data-bs-toggle="tab" data-bs-target="#nav-informes" type="button" role="tab" aria-controls="nav-informes" aria-selected="false">Informes / Consultas</button>
                <button class="nav-link" id="nav-backupcloud-tab" data-bs-toggle="tab" data-bs-target="#nav-backupcloud" type="button" role="tab" aria-controls="nav-backupcloud" aria-selected="false">BackupCloud</button>
                
            </div>

            </div>
            <div class="card-body">

            <div class="tab-content" id="nav-tabContent">
            
                <div class="tab-pane fade show active" id="nav-direccion" role="tabpanel" aria-labelledby="nav-direccion-tab">
    
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Nombre Fiscal:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_nombre_fiscal" runat="server" class="form-control" placeholder="Escriba el Nombre Fiscal"  maxlength="200"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Nombre Comercial:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_nombre_comercial" runat="server" class="form-control" placeholder="Escriba el Nombre Comercial"  maxlength="200"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Teléfono:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_telefono" runat="server" class="form-control" placeholder="Teléfono"  maxlength="15"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Email:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_email" runat="server" class="form-control" placeholder="Email" maxlength="50"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                N.I.F.:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_nif" runat="server" class="form-control" placeholder="N.I.F."  maxlength="15"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Tipo Vía:
                            </span>
                        </div>
                        <asp:DropDownList ID="DDL_tipo_via" runat="server" class="form-control"></asp:DropDownList>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Nombre Vía:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_domicilio" runat="server" class="form-control" placeholder="Domicilio"  maxlength="15"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Nº:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_numero" runat="server" class="form-control" placeholder="Número" maxlength="5"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Escalera:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_escalera" runat="server" class="form-control" placeholder="Número de la Escalera" maxlength="2"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Piso:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_piso" runat="server" class="form-control" placeholder="Número del Piso" maxlength="15"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Puerta:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_puerta" runat="server" class="form-control" placeholder="Número de la Puerta" maxlength="3"></asp:TextBox>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Provincia:
                            </span>
                        </div>
                        <asp:DropDownList ID="DDL_provincia" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Localidad:
                            </span>
                        </div>
                        <asp:DropDownList ID="DDL_localidad" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Pais:
                            </span>
                        </div>
                        <asp:DropDownList ID="DDL_pais" runat="server" class="form-control"></asp:DropDownList>
                    </div>

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            C.P.:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_cp" runat="server" class="form-control" placeholder="Código Postal" maxlength="6" disabled></asp:TextBox>
                    </div>

                    <div class="container-fluid" style="text-align:center">
                        <asp:Button ID="btn_grabar_direccion" runat="server" Text="Actualizar" CssClass="btn btn-outline-primary btn-sm"/>
                    </div> 

                </div>

                <div class="tab-pane fade" id="nav-facturas" role="tabpanel" aria-labelledby="nav-facturas-tab">
                    
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" value="" id="chk_isp" runat ="server"  />
                      <label class="form-check-label" for="chk_isp">
                        Activar ISP (Inversión de Sujeto Pasivo)
                      </label>
                    </div>
                    <br />

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Texto ISP:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_isp" runat="server" class="form-control" placeholder="Escriba el texto legal"  maxlength="400" Text ="Operación con inversión del sujeto pasivo conforme al Artículo 84. Uno. 2º de la Ley 37/1992."></asp:TextBox>
                    </div>
                    
                    <hr class ="text-secondary"/>
                    
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" value="" id="chk_exento" runat ="server"  />
                      <label class="form-check-label" for="chk_exento">
                        Activar Exento
                      </label>
                    </div>
                    <br />

                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                                Texto Exento:
                            </span>
                        </div>
                        <asp:TextBox ID="txt_exento" runat="server" class="form-control" placeholder="Escriba el texto legal"  maxlength="400" Text ="Factura exenta de IGIC - (Art. 10.1.3 - Ley 20/91)"></asp:TextBox>
                    </div>
                                       
                    <div class="container-fluid" style="text-align:center">
                        <asp:Button ID="btn_grabar_facturas" runat="server" Text="Actualizar" CssClass="btn btn-outline-primary btn-sm"/>
                    </div> 

                </div> 

                 <div class="tab-pane fade" id="nav-albaranes" role="tabpanel" aria-labelledby="nav-albaranes-tab">
                    
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" value="" id="chk_valorado" runat ="server"  />
                      <label class="form-check-label" for="chk_valorado">
                        Valorado / Sin Valorar
                      </label>
                    </div>
                    <small><span class="text-secondary">Te permite elegir en el albarán la posibilidad de imprimir con / sin precios.</span></small>
                    <br />
                    
                    <hr class ="text-secondary"/>
                    
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" value="" id="chk_gestion_documental_albaran" runat ="server"  />
                      <label class="form-check-label" for="chk_gestion_documental_albaran">
                        Activar Gestión Documental
                      </label>
                    </div>
                    <small><span class="text-secondary">Aparecerá un QR en el albarán impreso para realizar tareas de gestión documental.</span></small>
                    <br />
                    <br />
                  
                    <div class="container-fluid" style="text-align:center">
                        <asp:Button ID="btn_grabar_albaranes" runat="server" Text="Actualizar" CssClass="btn btn-outline-primary btn-sm"/>
                    </div> 

                </div>

                <div class="tab-pane fade" id="nav-informes" role="tabpanel" aria-labelledby="nav-informes-tab">
                    
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Nº Registros Consultas:
                            </span>
                        </div> 
                        <asp:DropDownList ID="DDL_n_registros_consultas" runat="server" CssClass="form-control">
                            <asp:ListItem Value="25" Text="25" Selected="true"></asp:ListItem>
                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                            <asp:ListItem Value="75" Text="75"></asp:ListItem>
                            <asp:ListItem Value="100" Text="100"></asp:ListItem>
                            <asp:ListItem Value="125" Text="125"></asp:ListItem>
                            <asp:ListItem Value="150" Text="150"></asp:ListItem>
                            <asp:ListItem Value="175" Text="175"></asp:ListItem>
                            <asp:ListItem Value="200" Text="200"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="container-fluid" style="text-align:center">
                        <asp:Button ID="btn_grabar_informes" runat="server" Text="Actualizar" CssClass="btn btn-outline-primary btn-sm"/>
                    </div> 

                </div> 

                <div class="tab-pane fade" id="nav-backupcloud" role="tabpanel" aria-labelledby="nav-backupcloud-tab">
                    
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">
                            Mantener la copia durante:
                            </span>
                        </div> 
                        <asp:DropDownList ID="DDL_dias_backup" runat="server" CssClass="form-control">
                            <asp:ListItem Value="-7" Text="7 Días"></asp:ListItem>
                            <asp:ListItem Value="-15" Text="15 Días"></asp:ListItem>
                            <asp:ListItem Value="-30" Text="30 Días"></asp:ListItem>
                            <asp:ListItem Value="-60" Text="60 Días"></asp:ListItem>
                            <asp:ListItem Value="-90" Text="90 Días"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="container-fluid" style="text-align:center">
                        <asp:Button ID="btn_grabar_backupcloud" runat="server" Text="Actualizar" CssClass="btn btn-outline-primary btn-sm"/>
                    </div>

                </div> 

            </div>

            </div>


        </div>

        <uc1:cuadro_trabajando runat="server" ID="cuadro_trabajando" />


    <div id="partes_hidden" style="visibility:hidden; display: none;">
        txt_cp_enable:<asp:TextBox ID="txt_cp_enable" runat="server" Width="400"></asp:TextBox><br />
    </div>

    </form>
</body>
</html>