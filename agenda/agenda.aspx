<%@ Page Language="VB" 
    MasterPageFile="~/MasterPages/Site.master"
    AutoEventWireup="false" 
    CodeFile="agenda.aspx.vb" 
    Inherits="agenda_agenda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%-- Filtros rápidos con botones --%>
<div class="row mt-3 mb-3 align-items-center">
    <div class="col-md-8">
        <div class="btn-group" role="group">
            <button type="button" class="btn btn-outline-primary btn-filter active" data-filter="all">
                <i class="bi bi-list"></i> Todas <span class="badge bg-secondary" id="count-all">0</span>
            </button>
            <button type="button" class="btn btn-outline-warning btn-filter" data-filter="pendientes">
                <i class="bi bi-hourglass-split"></i> Pendientes <span class="badge bg-warning text-dark" id="count-pendientes">0</span>
            </button>
            <button type="button" class="btn btn-outline-success btn-filter" data-filter="acabadas">
                <i class="bi bi-check-circle"></i> Acabadas <span class="badge bg-success" id="count-acabadas">0</span>
            </button>
            <button type="button" class="btn btn-outline-info btn-filter" data-filter="hoy">
                <i class="bi bi-calendar-day"></i> Hoy <span class="badge bg-info text-dark" id="count-hoy">0</span>
            </button>
            <button type="button" class="btn btn-outline-danger btn-filter" data-filter="semana">
                <i class="bi bi-calendar-week"></i> Esta semana <span class="badge bg-danger" id="count-semana">0</span>
            </button>
        </div>
    </div>
<div class="col-md-4">
    <div class="d-flex gap-2">
        <!-- Input de búsqueda -->
        <div class="input-group flex-grow-1">
            <span class="input-group-text">
                <i class="bi bi-search"></i>
            </span>
            <input type="text" id="globalSearch" class="form-control" placeholder="Buscar en todas las columnas...">
            <button class="btn btn-outline-secondary" type="button" id="clearSearch">
                <i class="bi bi-x-lg"></i>
            </button>
        </div>

        <!-- Botón Limpiar Filtros -->
        <asp:Button ID="btnLimpiarFiltros" runat="server" CssClass="btn btn-outline-danger" Text="Limpiar Filtros Avanzados" />
    </div>
</div>



<%-- GridView --%>
<div class="table-container my-3">
    <asp:GridView ID="gvCitas" runat="server"
        CssClass="table table-bordered table-hover align-middle"
        AutoGenerateColumns="False" 
        DataKeyNames="id_cita"
        OnRowDataBound="gvCitas_RowDataBound"
        UseAccessibleHeader="True">
        
        <HeaderStyle CssClass="table-dark text-center" />
        
        <Columns>
            <%-- Pendientes --%>
            <asp:TemplateField HeaderText="Pendientes">
                <ItemTemplate>
                    <input type="checkbox"
                           class="chkPendiente"
                           data-id='<%# Eval("id_cita") %>'
                           data-estado="pendiente"
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

            <%-- Hora --%>
            <asp:TemplateField HeaderText="Hora">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("hora")), "-", CType(Eval("hora"), TimeSpan).ToString("hh\:mm")) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Código --%>
            <asp:TemplateField HeaderText="Código">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("codigo")), "-", Eval("codigo").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Paciente --%>
            <asp:TemplateField HeaderText="Paciente">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("paciente_nombre")), "", Eval("paciente_nombre").ToString()) & " " & If(IsDBNull(Eval("paciente_apellido")), "", Eval("paciente_apellido").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- Médico --%>
            <asp:TemplateField HeaderText="Médico">
                <ItemTemplate>
                    <span data-color='<%# Eval("medico_color") %>'>
                        <%# If(IsDBNull(Eval("medico_nombre")), "", Eval("medico_nombre").ToString()) & " " & If(IsDBNull(Eval("medico_apellido")), "", Eval("medico_apellido").ToString()) %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>

            <%-- TV --%>
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

            <%-- Observaciones --%>
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
                           data-estado="acabada"
                           <%# If(Convert.ToBoolean(Eval("acabada")), "checked", "") %> />
                </ItemTemplate>
            </asp:TemplateField>

            <%-- No Citar --%>
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

            <%-- Sustituye --%>
            <asp:TemplateField HeaderText="Sustituye">
                <ItemTemplate>
                    <%# If(IsDBNull(Eval("id_medico_sustituto")), "-", Eval("id_medico_sustituto").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    
</div>


<%-- ⭐ Filtros Avanzados Desplegables --%>
<div class="container-fluid mt-4 mb-5">
    <!-- Botón de colapsar/expandir -->
    <div class="d-flex justify-content-between align-items-center mb-3 pb-2 border-bottom">
        <h5 class="mb-0 fw-bold text-primary">
            <i class="bi bi-funnel me-2"></i>Filtros Avanzados
        </h5>
        <button class="btn btn-outline-primary btn-sm" type="button" data-bs-toggle="collapse" data-bs-target="#filtrosAvanzados" aria-expanded="false" aria-controls="filtrosAvanzados">
            <i class="bi bi-chevron-down"></i> Mostrar/Ocultar
        </button>
    </div>

    <!-- Contenedor colapsable -->
    <div class="collapse" id="filtrosAvanzados">
        <%-- Primera fila: Médico, Fecha, Hora --%>
        <div class="row g-3 mb-3">
            <div class="col-12 col-lg-4">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkMedico" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="<%= chkMedico.ClientID %>">
                            <i class="bi bi-person-badge text-primary me-1"></i>Médico
                        </label>
                    </div>
                    <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" Enabled="False">
                        <asp:ListItem Text="Seleccione un médico..." Value="0" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-12 col-lg-4">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkFecha" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="<%= chkFecha.ClientID %>">
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

            <div class="col-12 col-lg-4">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkHora" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="<%= chkHora.ClientID %>">
                            <i class="bi bi-clock text-warning me-1"></i>Hora
                        </label>
                    </div>
                    <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" TextMode="Time" Enabled="False" />
                </div>
            </div>
        </div>

        <%-- Segunda fila: Estados y Nombre --%>
        <div class="row g-3 mb-3">
            <div class="col-12 col-md-6 col-lg-3">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkPendiente" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="<%= chkPendiente.ClientID %>">
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

            <div class="col-12 col-md-6 col-lg-3">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkGestionada" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="<%= chkGestionada.ClientID %>">
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

            <div class="col-12 col-lg-6">
                <div class="filter-box p-3 bg-light rounded-3 h-100">
                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkNombre" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label fw-semibold" for="<%= chkNombre.ClientID %>">
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
        <div class="row g-3 mb-3">
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

        <%-- Botón de aplicar --%>
        <div class="row">
            <div class="col-12 text-center mt-3">
                <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary btn-lg px-5 shadow-sm" Text="Aplicar Filtros Avanzados" />
            </div>
        </div>
    </div>
</div>


<%-- Información de resultados --%>
<div class="row mt-2">
    <div class="col-12">
        <div class="alert alert-info d-flex justify-content-between align-items-center">
            <span>
                Mostrando <strong id="filas-visibles">0</strong> de <strong id="filas-totales">0</strong> citas
                <span id="filtros-avanzados-activos" class="badge bg-warning text-dark ms-2" style="display:none;">
                    <i class="bi bi-funnel-fill"></i> Filtros avanzados aplicados
                </span>
                <span id="filtros-rapidos-activos" class="badge bg-primary ms-2" style="display:none;">
                    <i class="bi bi-lightning-fill"></i> Filtro rápido activo
                </span>
            </span>
        </div>
    </div>
</div>


<%-- Estilos personalizados --%>
<style>
body {
  height: 100%;
  margin: 0;
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
  0%, 100% { transform: scale(1); }
  50% { transform: scale(1.1); }
}

.btn-filter {
    margin: 2px;
    transition: all 0.3s ease;
}

.btn-filter.active {
    background-color: #0d6efd;
    color: white;
    border-color: #0d6efd;
}

.btn-filter:not(.active):hover {
    transform: translateY(-2px);
}

.btn-filter .badge {
    margin-left: 5px;
}

#globalSearch {
    border: 2px solid #dee2e6;
    transition: all 0.3s ease;
}

#globalSearch:focus {
    border-color: #0d6efd;
    box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
}

/* Fila oculta */
tr.fila-oculta {
    display: none !important;
}

/* Colores para 10 médicos */
tr.bg-medico-1 td { background-color: #8dafe2 !important; color: #141414 !important; }
tr.bg-medico-1:hover td { background-color: #4370b4 !important; }

tr.bg-medico-2 td { background-color: #97ceb4 !important; color: #141414 !important; }
tr.bg-medico-2:hover td { background-color: #569678 !important; }

tr.bg-medico-3 td { background-color: #feba83 !important; color: #141414 !important; }
tr.bg-medico-3:hover td { background-color: #926b4c !important; }

tr.bg-medico-4 td { background-color: #d55965 !important; color: #141414 !important; }
tr.bg-medico-4:hover td { background-color: #a33e48 !important; }

tr.bg-medico-5 td { background-color: #b49ae6 !important; color: #141414 !important; }
tr.bg-medico-5:hover td { background-color: #6a588b !important; }

tr.bg-medico-6 td { background-color: #7dd3c0 !important; color: #141414 !important; }
tr.bg-medico-6:hover td { background-color: #4a9b8a !important; }

tr.bg-medico-7 td { background-color: #f5a3b5 !important; color: #141414 !important; }
tr.bg-medico-7:hover td { background-color: #c76f82 !important; }

tr.bg-medico-8 td { background-color: #ffe082 !important; color: #141414 !important; }
tr.bg-medico-8:hover td { background-color: #d4a954 !important; }

tr.bg-medico-9 td { background-color: #9fa8da !important; color: #141414 !important; }
tr.bg-medico-9:hover td { background-color: #6976a8 !important; }

tr.bg-medico-10 td { background-color: #bcaaa4 !important; color: #141414 !important; }
tr.bg-medico-10:hover td { background-color: #8c7a75 !important; }

tr.bg-medico-default td { background-color: #e0e0e0 !important; color: #141414 !important; }
tr.bg-medico-default:hover td { background-color: #bdbdbd !important; }

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

<!-- jQuery -->
<script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>

<script>
$(document).ready(function () {
    console.log("✅ Sistema de filtros cargado");

    var gridView = $('#<%= gvCitas.ClientID %>');
    var todasLasFilas = gridView.find('tbody tr');
    var filtrosAvanzadosActivos = <%= If(ViewState("ColorSeleccionado") IsNot Nothing OrElse ViewState("PendienteSeleccionado") IsNot Nothing OrElse ViewState("FechaInicio") IsNot Nothing OrElse ViewState("HoraSeleccionada") IsNot Nothing OrElse ViewState("GestionadaSeleccionada") IsNot Nothing OrElse ViewState("NombreBusqueda") IsNot Nothing OrElse ViewState("TipoFicha") IsNot Nothing OrElse ViewState("TipoTV") IsNot Nothing, "true", "false") %>;

    // ========================================
    // 1. ACTIVAR/DESACTIVAR FILTROS AVANZADOS
    // ========================================
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

    // ========================================
    // 2. FUNCIONES AUXILIARES
    // ========================================
    function getFechaHoy() {
        var hoy = new Date();
        return formatearFecha(hoy);
    }

    function formatearFecha(fecha) {
        var dd = String(fecha.getDate()).padStart(2, '0');
        var mm = String(fecha.getMonth() + 1).padStart(2, '0');
        var yyyy = fecha.getFullYear();
        return dd + '/' + mm + '/' + yyyy;
    }

    function getRangoSemana() {
        var hoy = new Date();
        var diaSemana = hoy.getDay();
        var diff = hoy.getDate() - diaSemana + (diaSemana === 0 ? -6 : 1);
        var primerDia = new Date(hoy.setDate(diff));
        var ultimoDia = new Date(primerDia);
        ultimoDia.setDate(primerDia.getDate() + 6);
        
        return {
            inicio: formatearFecha(primerDia),
            fin: formatearFecha(ultimoDia)
        };
    }

    function parsearFecha(fechaStr) {
        var partes = fechaStr.split('/');
        if (partes.length === 3) {
            return new Date(partes[2], partes[1] - 1, partes[0]);
        }
        return null;
    }

    function actualizarContadores() {
        var visibles = todasLasFilas.filter(':visible').length;
        var totales = todasLasFilas.length;
        
        $('#filas-visibles').text(visibles);
        $('#filas-totales').text(totales);

        // Contadores por categoría
        var pendientes = 0, acabadas = 0, hoy = 0, semana = 0;
        var fechaHoy = getFechaHoy();
        var rango = getRangoSemana();

        todasLasFilas.each(function() {
            var $fila = $(this);
            var fechaFila = $fila.find('td:eq(2)').text().trim();
            
            // Pendientes
            if ($fila.find('.chkPendiente').is(':checked')) {
                pendientes++;
            }
            
            // Acabadas
            if ($fila.find('.chkAcabado').is(':checked')) {
                acabadas++;
            }
            
            // Hoy
            if (fechaFila === fechaHoy) {
                hoy++;
            }
            
            // Esta semana
            var fechaObj = parsearFecha(fechaFila);
            var fechaInicioObj = parsearFecha(rango.inicio);
            var fechaFinObj = parsearFecha(rango.fin);
            
            if (fechaObj && fechaInicioObj && fechaFinObj) {
                if (fechaObj >= fechaInicioObj && fechaObj <= fechaFinObj) {
                    semana++;
                }
            }
        });

        $('#count-all').text(totales);
        $('#count-pendientes').text(pendientes);
        $('#count-acabadas').text(acabadas);
        $('#count-hoy').text(hoy);
        $('#count-semana').text(semana);
        
        // Mostrar indicadores de filtros activos
        var hayBusquedaGlobal = $('#globalSearch').val().length > 0;
        var hayFiltroRapido = !$('.btn-filter[data-filter="all"]').hasClass('active');
        
        if (filtrosAvanzadosActivos) {
            $('#filtros-avanzados-activos').show();
        } else {
            $('#filtros-avanzados-activos').hide();
        }

        if (hayFiltroRapido || hayBusquedaGlobal) {
            $('#filtros-rapidos-activos').show();
        } else {
            $('#filtros-rapidos-activos').hide();
        }

        // Mostrar botón de reseteo si hay algún filtro activo
        if (filtrosAvanzadosActivos || hayFiltroRapido || hayBusquedaGlobal) {
            $('#btnResetearTodo').show();
        } else {
            $('#btnResetearTodo').hide();
        }
    }

    // ========================================
    // 3. APLICAR FILTROS DEL CLIENTE
    // ========================================
    function aplicarFiltrosCliente() {
        todasLasFilas.removeClass('fila-oculta').show();
        
        var busquedaGlobal = $('#globalSearch').val().toLowerCase();
        
        todasLasFilas.each(function() {
            var $fila = $(this);
            var mostrar = true;
            
            // Búsqueda global
            if (busquedaGlobal) {
                var textoFila = $fila.text().toLowerCase();
                if (textoFila.indexOf(busquedaGlobal) === -1) {
                    mostrar = false;
                }
            }
            
            if (!mostrar) {
                $fila.addClass('fila-oculta').hide();
            }
        });
        
        actualizarContadores();
    }

    // ========================================
    // 4. BÚSQUEDA GLOBAL
    // ========================================
    $('#globalSearch').on('keyup', function() {
        aplicarFiltrosCliente();
    });

    $('#clearSearch').on('click', function() {
        $('#globalSearch').val('');
        aplicarFiltrosCliente();
    });

    // ========================================
    // 5. FILTROS RÁPIDOS (BOTONES)
    // ========================================
    $('.btn-filter').on('click', function() {
        // ⚠️ Advertir si hay filtros avanzados activos
        if (filtrosAvanzadosActivos) {
            if (!confirm('⚠️ HAY FILTROS AVANZADOS APLICADOS.\n\nLos filtros rápidos solo funcionarán sobre los resultados ya filtrados.\n\n¿Deseas continuar o prefieres limpiar los filtros avanzados primero?')) {
                return;
            }
        }

        var filtro = $(this).data('filter');
        
        // Actualizar botón activo
        $('.btn-filter').removeClass('active');
        $(this).addClass('active');
        
        // Limpiar búsqueda global
        $('#globalSearch').val('');
        
        // Mostrar todas las filas primero
        todasLasFilas.removeClass('fila-oculta').show();
        
        // Aplicar filtro
        switch(filtro) {
            case 'all':
                console.log("🔵 Filtro: Todas las citas");
                break;
                
            case 'pendientes':
                todasLasFilas.each(function() {
                    var checkbox = $(this).find('.chkPendiente');
                    if (!checkbox.is(':checked')) {
                        $(this).addClass('fila-oculta').hide();
                    }
                });
                console.log("🟡 Filtro: Pendientes");
                break;
                
            case 'acabadas':
                todasLasFilas.each(function() {
                    var checkbox = $(this).find('.chkAcabado');
                    if (!checkbox.is(':checked')) {
                        $(this).addClass('fila-oculta').hide();
                    }
                });
                console.log("🟢 Filtro: Acabadas");
                break;
                
            case 'hoy':
                var fechaHoy = getFechaHoy();
                todasLasFilas.each(function() {
                    var fechaFila = $(this).find('td:eq(2)').text().trim();
                    if (fechaFila !== fechaHoy) {
                        $(this).addClass('fila-oculta').hide();
                    }
                });
                console.log("🔵 Filtro: Hoy (" + fechaHoy + ")");
                break;
                
            case 'semana':
                var rango = getRangoSemana();
                var fechaInicio = parsearFecha(rango.inicio);
                var fechaFin = parsearFecha(rango.fin);
                
                todasLasFilas.each(function() {
                    var fechaStr = $(this).find('td:eq(2)').text().trim();
                    var fechaFila = parsearFecha(fechaStr);
                    
                    if (!fechaFila || fechaFila < fechaInicio || fechaFila > fechaFin) {
                        $(this).addClass('fila-oculta').hide();
                    }
                });
                console.log("🔵 Filtro: Esta semana (" + rango.inicio + " - " + rango.fin + ")");
                break;
        }
        
        actualizarContadores();
    });

    // ========================================
    // 6. BOTÓN RESETEAR TODO
    // ========================================
    $('#btnResetearTodo').on('click', function() {
        if (filtrosAvanzadosActivos) {
            if (confirm('Esto limpiará TODOS los filtros (avanzados y rápidos) y recargará la página. ¿Continuar?')) {
                // Hacer clic en el botón de limpiar filtros avanzados
                $('#<%= btnLimpiarFiltros.ClientID %>').click();
            }
        } else {
            // Solo limpiar filtros del cliente
            $('#globalSearch').val('');
            $('.btn-filter[data-filter="all"]').click();
        }
    });

    // ========================================
    // 7. EVENTOS AJAX PARA CHECKBOXES
    // ========================================
    function aplicarEventosCheckboxes() {
        $('.chkPendiente').off('change').on('change', function () {
            actualizarCheckbox($(this), 'ActualizarPendiente', 'pendiente', 'Pendiente');
        });

        $('.chkEmitido').off('change').on('change', function () {
            actualizarCheckbox($(this), 'ActualizarEmitido', 'emitido', 'Emitido');
        });

        $('.chkAcabado').off('change').on('change', function () {
            actualizarCheckbox($(this), 'ActualizarAcabada', 'acabada', 'Acabada');
        });

        $('.chkNC').off('change').on('change', function () {
            actualizarCheckbox($(this), 'ActualizarNumeroCita', 'numeroCita', 'No Citar');
        });

        $('.chkSM').off('change').on('change', function () {
            actualizarCheckbox($(this), 'ActualizarSM', 'sm', 'S.M.');
        });
    }

    function actualizarCheckbox($checkbox, metodo, parametro, nombreCampo) {
        var id = $checkbox.data('id');
        var valor = $checkbox.is(':checked');
        
        $checkbox.prop('disabled', true);

        var data = { id: id };
        data[parametro] = valor;

        $.ajax({
            type: "POST",
            url: "agenda.aspx/" + metodo,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                if (res.d) {
                    console.log("✅ " + nombreCampo + " actualizado (ID: " + id + ")");
                    $checkbox.closest('tr').fadeOut(100).fadeIn(100);
                    
                    // Actualizar contadores después de cambio
                    setTimeout(actualizarContadores, 200);
                } else {
                    alert("Error al actualizar " + nombreCampo);
                    $checkbox.prop('checked', !valor);
                }
            },
            error: function (xhr, status, error) {
                console.error("❌ Error al actualizar " + nombreCampo + ":", error);
                alert("Error de conexión al actualizar " + nombreCampo);
                $checkbox.prop('checked', !valor);
            },
            complete: function() {
                $checkbox.prop('disabled', false);
            }
        });
    }

    // ========================================
    // 8. INICIALIZAR
    // ========================================
    aplicarEventosCheckboxes();
    actualizarContadores();
    
    // Si hay filtros avanzados activos, mostrar advertencia
    if (filtrosAvanzadosActivos) {
        console.log("⚠️ Filtros avanzados detectados. Los filtros rápidos funcionarán sobre estos resultados.");
    }
    
    console.log("✅ Sistema listo. Total de citas:", todasLasFilas.length);
});
</script>
</asp:Content>