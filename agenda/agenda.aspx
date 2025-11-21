<%@ Page Language="VB" 
    MasterPageFile="~/MasterPages/Site.master"
    AutoEventWireup="false" 
    CodeFile="agenda.aspx.vb" 
    Inherits="agenda_agenda" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%-- GridView --%>
<%-- GridView --%>
<div class="table-container">
<asp:GridView ID="gvCitas" runat="server"
    CssClass="table table-bordered table-hover align-middle"
    AutoGenerateColumns="False" 
    DataKeyNames="id_cita"
    OnRowDataBound="gvCitas_RowDataBound">
    
    <HeaderStyle CssClass="table-dark text-center" />
        
        
        <Columns>
            <%-- Pendientes --%>
            <asp:TemplateField HeaderText="Pendientes">
                <ItemTemplate>
                    <input type="checkbox"
                           class="chkPendiente"
                           data-id='<%# Eval("id_cita") %>'
                           <%# If(Not Convert.ToBoolean(Eval("acabada")), "checked", "") %> />
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Emitido --%>
            <asp:TemplateField HeaderText="Emitido">
                <ItemTemplate>
                    <input type="checkbox"
                           class="chkEmitido"
                           data-id='<%# Eval("id_cita") %>'
                           <%# If(Convert.ToBoolean(Eval("emitido")), "checked", "") %> />
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Fecha --%>
            <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />

            <%-- Hora - CORREGIDO --%>
            <asp:TemplateField HeaderText="Hora">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("hora")), "-", CType(Eval("hora"), TimeSpan).ToString("hh\:mm")) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Código - CORREGIDO --%>
            <asp:TemplateField HeaderText="Código">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("codigo")), "-", Eval("codigo").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Paciente (Nombre completo) - CORREGIDO --%>
            <asp:TemplateField HeaderText="Paciente">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("paciente_nombre")), "", Eval("paciente_nombre").ToString()) & " " & If(IsDBNull(Eval("paciente_apellido")), "", Eval("paciente_apellido").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Médico (Nombre completo) - CORREGIDO --%>
            <asp:TemplateField HeaderText="Médico">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("medico_nombre")), "", Eval("medico_nombre").ToString()) & " " & If(IsDBNull(Eval("medico_apellido")), "", Eval("medico_apellido").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- TV - CORREGIDO --%>
            <asp:TemplateField HeaderText="TV">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("tv")), "-", If(Convert.ToInt32(Eval("tv")) = 1, "Presencial", "Telemedicina")) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Asistió --%>
            <asp:TemplateField HeaderText="Asistió">
                <ItemTemplate>
                    <%# If(Convert.ToBoolean(Eval("asistio")), "Sí", "No") %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Pospuesto --%>
            <asp:TemplateField HeaderText="Pospuesto">
                <ItemTemplate>
                    <%# If(Convert.ToBoolean(Eval("pospuesto")), "Sí", "No") %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Observaciones - CORREGIDO --%>
            <asp:TemplateField HeaderText="Observaciones">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("observaciones")), "-", Eval("observaciones").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Acabada --%>
            <asp:TemplateField HeaderText="Acabada">
                <ItemTemplate>
                    <input type="checkbox"
                           class="chkAcabado"
                           data-id='<%# Eval("id_cita") %>'
                           <%# If(Convert.ToBoolean(Eval("acabada")), "checked", "") %> />
                </ItemTemplate>
            </asp:TemplateField>

            <%-- No Citar (numero_cita) --%>
            <asp:TemplateField HeaderText="No Citar">
                <ItemTemplate>
                    <input type="checkbox"
                           class="chkNC"
                           data-id='<%# Eval("id_cita") %>'
                           <%# If(Convert.ToBoolean(Eval("numero_cita")), "checked", "") %> />
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Deuda --%>
            <asp:BoundField DataField="deuda" HeaderText="Deuda (€)" DataFormatString="{0:F2}" HtmlEncode="False" />

            <%-- S.M. --%>
            <asp:TemplateField HeaderText="S.M.">
                <ItemTemplate>
                    <input type="checkbox"
                           class="chkSM"
                           data-id='<%# Eval("id_cita") %>'
                           <%# If(Convert.ToBoolean(Eval("s_m")), "checked", "") %> />
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Sustituye (id_medico_sustituto) - CORREGIDO --%>
            <asp:TemplateField HeaderText="Sustituye">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("id_medico_sustituto")), "-", Eval("id_medico_sustituto").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
</div>
<%-- Filtros de citas --%>
<div class="container-fluid mt-4 mb-5">
    <div class="card border-0 shadow-lg p-4 w-100 mx-auto" style="max-width: 1200px; border-radius: 15px;">
        <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom">
            <h4 class="mb-0 fw-bold text-primary">
                <i class="bi bi-funnel me-2"></i>Filtros de Citas
            </h4>
            <asp:Button ID="btnLimpiarFiltros" runat="server" CssClass="btn btn-outline-secondary btn-sm" Text="Limpiar" />
        </div>

        <%-- Primera fila: Médico, Fecha, Hora --%>
        <div class="row g-3 mb-3">
            <%-- Médico --%>
            <div class="col-12 col-lg-4">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkMedico" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="chkMedico">
                            <i class="bi bi-person-badge text-primary me-1"></i>Médico
                        </label>
                    </div>
                    <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" Enabled="False">
                        <asp:ListItem Text="Seleccione un médico..." Value="" />
                    </asp:DropDownList>
                </div>
            </div>

            <%-- Fecha --%>
            <div class="col-12 col-lg-4">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkFecha" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="chkFecha">
                            <i class="bi bi-calendar-event text-success me-1"></i>Rango de Fecha
                        </label>
                    </div>
                    <div class="d-flex gap-2">
                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control form-control-sm" TextMode="Date" Enabled="False" />
                        <span class="align-self-center">-</span>
                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control form-control-sm" TextMode="Date" Enabled="False" />
                    </div>
                </div>
            </div>

            <%-- Hora --%>
            <div class="col-12 col-lg-4">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkHora" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="chkHora">
                            <i class="bi bi-clock text-warning me-1"></i>Hora
                        </label>
                    </div>
                    <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" TextMode="Time" Enabled="False" />
                </div>
            </div>
        </div>

        <%-- Segunda fila: Estados y Nombre --%>
        <div class="row g-3 mb-3">
            <%-- Pendiente --%>
            <div class="col-12 col-md-6 col-lg-3">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkPendiente" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="chkPendiente">
                            <i class="bi bi-hourglass-split text-info me-1"></i>Pendiente
                        </label>
                    </div>
                    <asp:DropDownList ID="ddlPendiente" runat="server" CssClass="form-select form-select-sm" Enabled="False">
                        <asp:ListItem>Todos</asp:ListItem>
                        <asp:ListItem>Sí</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <%-- Gestionada --%>
            <div class="col-12 col-md-6 col-lg-3">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkGestionada" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="chkGestionada">
                            <i class="bi bi-check-circle text-success me-1"></i>Gestionada
                        </label>
                    </div>
                    <asp:DropDownList ID="ddlGestionada" runat="server" CssClass="form-select form-select-sm" Enabled="False">
                        <asp:ListItem>Todos</asp:ListItem>
                        <asp:ListItem>Sí</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <%-- Búsqueda por Nombre/Apellido --%>
            <div class="col-12 col-lg-6">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkNombre" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="chkNombre">
                            <i class="bi bi-person-search text-danger me-1"></i>Búsqueda por
                        </label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlNombre" runat="server" CssClass="form-select" Enabled="False" style="max-width: 140px;">
                            <asp:ListItem Text="Nombre" Value="nombre" />
                            <asp:ListItem Text="Apellido" Value="apellido" />
                        </asp:DropDownList>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Enabled="False" placeholder="Escribe aquí..." />
                    </div>
                </div>
            </div>
        </div>

        <%-- Tercera fila: Ficha y Citas de paciente --%>
        <div class="row g-3">
            <%-- Ficha de paciente --%>
            <div class="col-12 col-lg-6">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="d-flex align-items-center mb-2">
                        <i class="bi bi-folder-fill text-primary me-2"></i>
                        <span class="fw-semibold me-3">Ficha de paciente</span>
                        <div class="form-check form-switch ms-auto mb-0">
                            <asp:CheckBox ID="chkEstetica" runat="server" CssClass="form-check-input" />
                        </div>
                    </div>
                    <asp:DropDownList ID="ddlEstetica" runat="server" CssClass="form-select form-select-sm" Enabled="False">
                        <asp:ListItem>Estética</asp:ListItem>
                        <asp:ListItem>Medicina general</asp:ListItem>
                        <asp:ListItem>Odontología</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <%-- Citas de paciente --%>
            <div class="col-12 col-lg-6">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="d-flex align-items-center mb-2">
                        <i class="bi bi-calendar2-check text-success me-2"></i>
                        <span class="fw-semibold me-3">Citas de paciente</span>
                        <div class="form-check form-switch ms-auto mb-0">
                            <asp:CheckBox ID="chkTV" runat="server" CssClass="form-check-input" />
                        </div>
                    </div>
                    <asp:DropDownList ID="ddlTV" runat="server" CssClass="form-select form-select-sm" Enabled="False">
                        <asp:ListItem>T.V</asp:ListItem>
                        <asp:ListItem>Presencial</asp:ListItem>
                        <asp:ListItem>Telemedicina</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>

        <%-- Botón de búsqueda --%>
        <div class="text-center mt-4">
            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary btn-lg px-5 shadow-sm" Text="Aplicar" />
        </div>
    </div>
</div>

<%-- Estilos personalizados --%>

<style>
body {
  height: 100%;
  margin: 0;
}

body {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

main {
  flex: 1;
}

header {
  flex: 0 0 auto;
}

.navbar-custom {
  background-color: #3a628d !important;
}

.icon-menu {
  width: 40px;
  height: 40px;
  object-fit: contain;
}

.table-container {
  width: 100%;
  overflow-x: auto;
  padding: 0;
  margin: 0;
}

.dataTables_wrapper {
  width: 100%;
}

.dataTables_scrollBody {
  overflow-x: auto !important;
}

.filter-box {
  transition: all 0.3s ease;
  border: 1px solid transparent;
}

.filter-box:hover {
  background-color: #f0f8ff !important;
  border-color: #0d6efd !important;
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.form-check-input:checked {
  background-color: #0d6efd;
  border-color: #0d6efd;
}

.form-check-input:checked ~ .form-check-label {
  color: #0d6efd;
}

.card {
  background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
}

.btn-primary {
  background: linear-gradient(135deg, #0d6efd 0%, #0a58ca 100%);
  border: none;
  border-radius: 25px;
  transition: all 0.3s ease;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(13, 110, 253, 0.4);
}

.form-control:focus,
.form-select:focus {
  border-color: #0d6efd;
  box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.15);
}

.form-check-input[type="checkbox"]:checked {
  animation: checkboxPulse 0.3s ease;
}

@keyframes checkboxPulse {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}

/* ============================================ */
/* COLORES PARA GRIDVIEW - 10 MÉDICOS */
/* ============================================ */

/* Color Azul (1) - Yvette */
tr.bg-medico-1 td {
  background-color: #8dafe2 !important;
  color: #141414 !important;
}
tr.bg-medico-1:hover td {
  background-color: #4370b4 !important;
}

/* Color Verde (2) - Miguel */
tr.bg-medico-2 td {
  background-color: #97ceb4 !important;
  color: #141414 !important;
}
tr.bg-medico-2:hover td {
  background-color: #569678 !important;
}

/* Color Naranja (3) - Laura */
tr.bg-medico-3 td {
  background-color: #feba83 !important;
  color: #141414 !important;
}
tr.bg-medico-3:hover td {
  background-color: #926b4c !important;
}

/* Color Rojo (4) - Carlos */
tr.bg-medico-4 td {
  background-color: #d55965 !important;
  color: #141414 !important;
}
tr.bg-medico-4:hover td {
  background-color: #a33e48 !important;
}

/* Color Púrpura (5) - Ana */
tr.bg-medico-5 td {
  background-color: #b49ae6 !important;
  color: #141414 !important;
}
tr.bg-medico-5:hover td {
  background-color: #6a588b !important;
}

/* Color Cian (6) - David */
tr.bg-medico-6 td {
  background-color: #7dd3c0 !important;
  color: #141414 !important;
}
tr.bg-medico-6:hover td {
  background-color: #4a9b8a !important;
}

/* Color Rosa (7) - Elena */
tr.bg-medico-7 td {
  background-color: #f5a3b5 !important;
  color: #141414 !important;
}
tr.bg-medico-7:hover td {
  background-color: #c76f82 !important;
}

/* Color Amarillo (8) - Jorge */
tr.bg-medico-8 td {
  background-color: #ffe082 !important;
  color: #141414 !important;
}
tr.bg-medico-8:hover td {
  background-color: #d4a954 !important;
}

/* Color Índigo (9) - Paula */
tr.bg-medico-9 td {
  background-color: #9fa8da !important;
  color: #141414 !important;
}
tr.bg-medico-9:hover td {
  background-color: #6976a8 !important;
}

/* Color Marrón (10) - Ricardo */
tr.bg-medico-10 td {
  background-color: #bcaaa4 !important;
  color: #141414 !important;
}
tr.bg-medico-10:hover td {
  background-color: #8c7a75 !important;
}

/* Color por defecto */
tr.bg-medico-default td {
  background-color: #e0e0e0 !important;
  color: #141414 !important;
}
tr.bg-medico-default:hover td {
  background-color: #bdbdbd !important;
}

/* Asegurar que todas las filas con colores tengan text-center */
tr.bg-medico-1,
tr.bg-medico-2,
tr.bg-medico-3,
tr.bg-medico-4,
tr.bg-medico-5,
tr.bg-medico-6,
tr.bg-medico-7,
tr.bg-medico-8,
tr.bg-medico-9,
tr.bg-medico-10,
tr.bg-medico-default {
  text-align: center !important;
}

/* ============================================ */

/* ============================================ */

footer.footer {
  background-color: #465f7a !important;
  color: #d5d4d4 !important;
  text-align: center;
  padding: 1rem 0;
  flex-shrink: 0;
}

footer.footer p {
  color: #d5d4d4 !important;
}
</style>


    <%-- Scripts al final del Content --%>
<script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
<script>
$(document).ready(function () {
    // Activar/desactivar filtros al marcar checkboxes
    $('#<%= chkMedico.ClientID %>').change(function () { 
        $('#<%= ddlMedico.ClientID %>').prop('disabled', !this.checked); 
    });
    
    $('#<%= chkFecha.ClientID %>').change(function () { 
        $('#<%= txtFechaInicio.ClientID %>, #<%= txtFechaFin.ClientID %>').prop('disabled', !this.checked); 
    });
    
    $('#<%= chkHora.ClientID %>').change(function () { 
        $('#<%= txtHora.ClientID %>').prop('disabled', !this.checked); 
    });
    
    $('#<%= chkPendiente.ClientID %>').change(function () { 
        $('#<%= ddlPendiente.ClientID %>').prop('disabled', !this.checked); 
    });
    
    $('#<%= chkGestionada.ClientID %>').change(function () { 
        $('#<%= ddlGestionada.ClientID %>').prop('disabled', !this.checked); 
    });
    
    $('#<%= chkNombre.ClientID %>').change(function () { 
        $('#<%= ddlNombre.ClientID %>, #<%= txtNombre.ClientID %>').prop('disabled', !this.checked); 
    });
    
    $('#<%= chkEstetica.ClientID %>').change(function () { 
        $('#<%= ddlEstetica.ClientID %>').prop('disabled', !this.checked); 
    });
    
    $('#<%= chkTV.ClientID %>').change(function () { 
        $('#<%= ddlTV.ClientID %>').prop('disabled', !this.checked); 
    });

    // Ajax para actualizar estado de pendiente
    $('.chkPendiente').change(function () {
        
        var id = $(this).data('id');
        var pendiente = $(this).is(':checked');

        $.ajax({
            type: "POST",
            url: "agenda.aspx/ActualizarPendiente",
            data: JSON.stringify({ id: id, pendiente: pendiente }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                if (res.d) console.log("Estado actualizado");
            },
            error: function () { alert("Error al actualizar el estado"); }
        });
    });
});
</script>

</asp:Content>

