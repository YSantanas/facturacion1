Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class agenda_agenda
    Inherits System.Web.UI.Page

    Private connectionString As String = "Server=93.90.25.27;Database=OP21F20112025;User ID=sa;Password=prueba2025_%;;TrustServerCertificate=True;"

    ' Diccionario de colores
    Private ReadOnly Colores As New Dictionary(Of String, Integer) From {
        {"Azul", 1},
        {"Verde", 2},
        {"Naranja", 3},
        {"Rojo", 4},
        {"Púrpura", 5},
        {"Cian", 6},
        {"Rosa", 7},
        {"Amarillo", 8},
        {"Índigo", 9},
        {"Marrón", 10}
    }

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarMedicos()
            CargarPendientes()
            CargarCitas() ' Carga inicial sin filtros
        End If

        RestaurarEstadoControles()
    End Sub

    ' ========================================================
    ' CARGAR MÉDICOS (MÉTODO QUE FALTABA)
    ' ========================================================
    Private Sub CargarMedicos()
        ddlMedico.Items.Clear()
        ddlMedico.Items.Add(New ListItem("Seleccione un color...", "0"))

        For Each kvp In Colores
            ddlMedico.Items.Add(New ListItem(kvp.Key, kvp.Value.ToString()))
        Next
    End Sub

    ' ========================================================
    ' LÓGICA DE ESTADO DE CONTROLES (Permite acumular filtros)
    ' ========================================================
    Private Sub RestaurarEstadoControles()
        ' Médico
        ddlMedico.Enabled = chkMedico.Checked
        If chkMedico.Checked AndAlso ViewState("ColorSeleccionado") IsNot Nothing Then
            Dim val As String = ViewState("ColorSeleccionado").ToString()
            If ddlMedico.Items.FindByValue(val) IsNot Nothing Then 
                ddlMedico.SelectedValue = val
            End If
        End If

        ' Fecha
        txtFechaInicio.Enabled = chkFecha.Checked
        txtFechaFin.Enabled = chkFecha.Checked
        If chkFecha.Checked Then
            If ViewState("FechaInicio") IsNot Nothing Then 
                txtFechaInicio.Text = CDate(ViewState("FechaInicio")).ToString("yyyy-MM-dd")
            End If
            If ViewState("FechaFin") IsNot Nothing Then 
                txtFechaFin.Text = CDate(ViewState("FechaFin")).ToString("yyyy-MM-dd")
            End If
        End If

        ' Hora
        txtHora.Enabled = chkHora.Checked
        If chkHora.Checked AndAlso ViewState("HoraSeleccionada") IsNot Nothing Then
            txtHora.Text = ViewState("HoraSeleccionada").ToString()
        End If

        ' Pendiente
        ddlPendiente.Enabled = chkPendiente.Checked
        If chkPendiente.Checked AndAlso ViewState("PendienteSeleccionado") IsNot Nothing Then
            Dim val As String = ViewState("PendienteSeleccionado").ToString()
            If ddlPendiente.Items.FindByValue(val) IsNot Nothing Then 
                ddlPendiente.SelectedValue = val
            End If
        End If

        ' Gestionada
        ddlGestionada.Enabled = chkGestionada.Checked
        If chkGestionada.Checked AndAlso ViewState("GestionadaSeleccionada") IsNot Nothing Then
            Dim val As String = ViewState("GestionadaSeleccionada").ToString()
            If ddlGestionada.Items.FindByValue(val) IsNot Nothing Then 
                ddlGestionada.SelectedValue = val
            End If
        End If

        ' Nombre
        ddlNombre.Enabled = chkNombre.Checked
        txtNombre.Enabled = chkNombre.Checked
        If chkNombre.Checked Then
            If ViewState("NombreBusqueda") IsNot Nothing Then
                txtNombre.Text = ViewState("NombreBusqueda").ToString()
            End If
            If ViewState("TipoBusqueda") IsNot Nothing Then
                Dim val As String = ViewState("TipoBusqueda").ToString()
                If ddlNombre.Items.FindByValue(val) IsNot Nothing Then 
                    ddlNombre.SelectedValue = val
                End If
            End If
        End If

        ' Estética
        ddlEstetica.Enabled = chkEstetica.Checked
        If chkEstetica.Checked AndAlso ViewState("TipoFicha") IsNot Nothing Then
            Dim val As String = ViewState("TipoFicha").ToString()
            If ddlEstetica.Items.FindByValue(val) IsNot Nothing Then 
                ddlEstetica.SelectedValue = val
            End If
        End If

        ' TV
        ddlTV.Enabled = chkTV.Checked
        If chkTV.Checked AndAlso ViewState("TipoTV") IsNot Nothing Then
            Dim val As String = ViewState("TipoTV").ToString()
            If ddlTV.Items.FindByValue(val) IsNot Nothing Then 
                ddlTV.SelectedValue = val
            End If
        End If
    End Sub

    ' ========================================================
    ' CARGA DE DATOS
    ' ========================================================
    Private Sub CargarCitas(Optional colorNombre As Integer = 0, 
                           Optional pendiente As String = "Todos",
                           Optional fechaInicio As DateTime? = Nothing, 
                           Optional fechaFin As DateTime? = Nothing,
                           Optional hora As String = "",
                           Optional gestionada As String = "Todos",
                           Optional nombreBusqueda As String = "",
                           Optional tipoBusqueda As String = "nombre",
                           Optional tipoFicha As String = "",
                           Optional tipoTV As String = "")

        Dim sql As String = 
            "SELECT c.id_cita, c.fecha, c.hora, c.codigo, c.tv, c.asistio, c.pospuesto, c.observaciones, c.acabada, c.emitido, " &
            "c.numero_cita, c.deuda, c.s_m, c.id_medico_sustituto, " &
            "m.id_medico, m.nombre AS medico_nombre, m.apellido AS medico_apellido, m.color AS medico_color, " &
            "p.id_paciente, p.nombre AS paciente_nombre, p.apellido AS paciente_apellido " &
            "FROM agenda_yvette c " &
            "INNER JOIN medico_yvette m ON c.id_medico = m.id_medico " &
            "INNER JOIN paciente_yvette p ON c.id_paciente = p.id_paciente " &
            "WHERE 1=1"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand()
                cmd.Connection = con
                
                ' ✅ Filtro por color de médico
                If colorNombre > 0 Then 
                    sql &= " AND m.color = @color"
                    cmd.Parameters.AddWithValue("@color", colorNombre)
                End If

                ' ✅ Filtro por estado pendiente/acabada
                If pendiente = "Sí" Then
                    sql &= " AND c.acabada = 0"
                ElseIf pendiente = "No" Then
                    sql &= " AND c.acabada = 1"
                End If

                ' ✅ Filtro por rango de fechas
                If fechaInicio.HasValue AndAlso fechaFin.HasValue Then
                    sql &= " AND c.fecha >= @fechaInicio AND c.fecha <= @fechaFin"
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value.Date)
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Value.Date)
                End If

                ' ✅ Filtro por hora
                If Not String.IsNullOrEmpty(hora) Then
                    sql &= " AND c.hora = @hora"
                    cmd.Parameters.AddWithValue("@hora", TimeSpan.Parse(hora))
                End If

                ' ✅ Filtro por gestionada (emitido)
                If gestionada = "Sí" Then
                    sql &= " AND c.emitido = 1"
                ElseIf gestionada = "No" Then
                    sql &= " AND c.emitido = 0"
                End If

                ' ✅ Filtro por nombre o apellido
                If Not String.IsNullOrEmpty(nombreBusqueda) Then
                    If tipoBusqueda = "nombre" Then
                        sql &= " AND p.nombre LIKE @nombre"
                    Else
                        sql &= " AND p.apellido LIKE @nombre"
                    End If
                    cmd.Parameters.AddWithValue("@nombre", "%" & nombreBusqueda & "%")
                End If

                ' ✅ Filtro por tipo de TV
                If tipoTV = "Presencial" OrElse tipoTV = "T.V" Then
                    sql &= " AND c.tv = 1"
                ElseIf tipoTV = "Telemedicina" Then
                    sql &= " AND c.tv = 0"
                End If

                sql &= " ORDER BY c.fecha DESC, c.hora DESC;"
                cmd.CommandText = sql

                Dim dt As New DataTable()
                con.Open()
                dt.Load(cmd.ExecuteReader())
                gvCitas.DataSource = dt
                gvCitas.DataBind()
            End Using
        End Using
    End Sub

    Private Sub CargarPendientes()
        Dim sql As String =
            "SELECT c.id_cita, c.fecha, c.hora, c.codigo, c.tv, c.asistio, c.pospuesto, c.observaciones, c.acabada, c.emitido, " &
            "c.numero_cita, c.deuda, c.s_m, c.id_medico_sustituto, " &
            "m.id_medico, m.nombre AS medico_nombre, m.apellido AS medico_apellido, m.color AS medico_color, " &
            "p.id_paciente, p.nombre AS paciente_nombre, p.apellido AS paciente_apellido " &
            "FROM agenda_yvette c " &
            "INNER JOIN medico_yvette m ON c.id_medico = m.id_medico " &
            "INNER JOIN paciente_yvette p ON c.id_paciente = p.id_paciente " &
            "WHERE c.acabada = 0 " &
            "ORDER BY c.fecha DESC, c.hora DESC;"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                Dim dt As New DataTable()
                con.Open()
                dt.Load(cmd.ExecuteReader())
                Session("Pendientes") = dt
            End Using
        End Using
    End Sub

    ' ========================================================
    ' EVENTOS
    ' ========================================================
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        ' 📌 Recoger TODOS los filtros activos
        Dim colorSeleccionado As Integer = 0
        Dim pendienteSeleccionado As String = "Todos"
        Dim fechaIni As DateTime? = Nothing
        Dim fechaFn As DateTime? = Nothing
        Dim horaSeleccionada As String = ""
        Dim gestionadaSeleccionada As String = "Todos"
        Dim nombreBusqueda As String = ""
        Dim tipoBusqueda As String = "nombre"
        Dim tipoFicha As String = ""
        Dim tipoTV As String = ""

        ' Médico
        If chkMedico.Checked AndAlso ddlMedico.SelectedValue <> "0" Then
            colorSeleccionado = Integer.Parse(ddlMedico.SelectedValue)
            ViewState("ColorSeleccionado") = colorSeleccionado
        Else
            ViewState("ColorSeleccionado") = Nothing
        End If

        ' Pendiente
        If chkPendiente.Checked Then
            pendienteSeleccionado = ddlPendiente.SelectedValue
            ViewState("PendienteSeleccionado") = pendienteSeleccionado
        Else
            ViewState("PendienteSeleccionado") = Nothing
        End If

        ' Fecha
        If chkFecha.Checked AndAlso txtFechaInicio.Text <> "" AndAlso txtFechaFin.Text <> "" Then
            fechaIni = DateTime.Parse(txtFechaInicio.Text)
            fechaFn = DateTime.Parse(txtFechaFin.Text)
            ViewState("FechaInicio") = fechaIni.Value
            ViewState("FechaFin") = fechaFn.Value
        Else
            ViewState("FechaInicio") = Nothing
            ViewState("FechaFin") = Nothing
        End If

        ' Hora
        If chkHora.Checked AndAlso txtHora.Text <> "" Then
            horaSeleccionada = txtHora.Text
            ViewState("HoraSeleccionada") = horaSeleccionada
        Else
            ViewState("HoraSeleccionada") = Nothing
        End If

        ' Gestionada
        If chkGestionada.Checked Then
            gestionadaSeleccionada = ddlGestionada.SelectedValue
            ViewState("GestionadaSeleccionada") = gestionadaSeleccionada
        Else
            ViewState("GestionadaSeleccionada") = Nothing
        End If

        ' Nombre/Apellido
        If chkNombre.Checked AndAlso txtNombre.Text <> "" Then
            nombreBusqueda = txtNombre.Text
            tipoBusqueda = ddlNombre.SelectedValue
            ViewState("NombreBusqueda") = nombreBusqueda
            ViewState("TipoBusqueda") = tipoBusqueda
        Else
            ViewState("NombreBusqueda") = Nothing
            ViewState("TipoBusqueda") = Nothing
        End If

        ' Tipo de Ficha
        If chkEstetica.Checked Then
            tipoFicha = ddlEstetica.SelectedValue
            ViewState("TipoFicha") = tipoFicha
        Else
            ViewState("TipoFicha") = Nothing
        End If

        ' Tipo de TV
        If chkTV.Checked Then
            tipoTV = ddlTV.SelectedValue
            ViewState("TipoTV") = tipoTV
        Else
            ViewState("TipoTV") = Nothing
        End If

        ' ✅ Aplicar TODOS los filtros a la vez
        CargarCitas(colorSeleccionado, pendienteSeleccionado, fechaIni, fechaFn, 
                    horaSeleccionada, gestionadaSeleccionada, nombreBusqueda, 
                    tipoBusqueda, tipoFicha, tipoTV)
        
        RestaurarEstadoControles()
    End Sub

    Protected Sub btnLimpiarFiltros_Click(sender As Object, e As EventArgs) Handles btnLimpiarFiltros.Click
        ' Desmarcar todos los checkboxes
        chkMedico.Checked = False
        chkFecha.Checked = False
        chkHora.Checked = False
        chkPendiente.Checked = False
        chkGestionada.Checked = False
        chkNombre.Checked = False
        chkEstetica.Checked = False
        chkTV.Checked = False

        ' Resetear todos los valores
        ddlMedico.SelectedIndex = 0
        txtFechaInicio.Text = ""
        txtFechaFin.Text = ""
        txtHora.Text = ""
        ddlPendiente.SelectedIndex = 0
        ddlGestionada.SelectedIndex = 0
        txtNombre.Text = ""
        ddlNombre.SelectedIndex = 0
        ddlEstetica.SelectedIndex = 0
        ddlTV.SelectedIndex = 0

        ' Limpiar ViewState
        ViewState("ColorSeleccionado") = Nothing
        ViewState("PendienteSeleccionado") = Nothing
        ViewState("FechaInicio") = Nothing
        ViewState("FechaFin") = Nothing
        ViewState("HoraSeleccionada") = Nothing
        ViewState("GestionadaSeleccionada") = Nothing
        ViewState("NombreBusqueda") = Nothing
        ViewState("TipoBusqueda") = Nothing
        ViewState("TipoFicha") = Nothing
        ViewState("TipoTV") = Nothing

        RestaurarEstadoControles()
        CargarCitas() ' Cargar todas las citas sin filtros
    End Sub

    ' ========================================================
    ' GRIDVIEW
    ' ========================================================
    Protected Sub gvCitas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCitas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If ViewState("ColorSeleccionado") IsNot Nothing AndAlso CInt(ViewState("ColorSeleccionado")) > 0 Then
                Try
                    Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
                    Dim colorMedico As Integer = If(IsDBNull(drv("medico_color")), 0, Convert.ToInt32(drv("medico_color")))

                    e.Row.CssClass = "text-center bg-medico-" & colorMedico
                Catch
                    e.Row.CssClass = "text-center"
                End Try
            Else
                e.Row.CssClass = "text-center"
            End If
        End If
    End Sub

    ' ========================================================
    ' FUNCIÓN CENTRALIZADA PARA WEBMETHODS
    ' ========================================================
    Private Shared Function EjecutarSeguro(sql As String, params As Dictionary(Of String, Object)) As Boolean
        Try
            Dim connectionString As String = "Server=93.90.25.27;Database=OP21F20112025;User ID=sa;Password=prueba2025_%;;TrustServerCertificate=True;"

            Using con As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, con)
                    For Each p In params
                        cmd.Parameters.AddWithValue(p.Key, p.Value)
                    Next

                    con.Open()
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using

        Catch
            Return False
        End Try
    End Function

    ' ========================================================
    ' WEBMETHODS
    ' ========================================================
    <WebMethod()>
    Public Shared Function ActualizarPendiente(id As Integer, pendiente As Boolean) As Boolean
        Return EjecutarSeguro("UPDATE agenda_yvette SET acabada = @acabada WHERE id_cita = @id",
            New Dictionary(Of String, Object) From {
                {"@id", id},
                {"@acabada", Not pendiente}
            })
    End Function

    <WebMethod()>
    Public Shared Function ActualizarEmitido(id As Integer, emitido As Boolean) As Boolean
        Return EjecutarSeguro("UPDATE agenda_yvette SET emitido = @emitido WHERE id_cita = @id",
            New Dictionary(Of String, Object) From {
                {"@id", id},
                {"@emitido", emitido}
            })
    End Function

    <WebMethod()>
    Public Shared Function ActualizarAcabada(id As Integer, acabada As Boolean) As Boolean
        Return EjecutarSeguro("UPDATE agenda_yvette SET acabada = @acabada WHERE id_cita = @id",
            New Dictionary(Of String, Object) From {
                {"@id", id},
                {"@acabada", acabada}
            })
    End Function

    <WebMethod()>
    Public Shared Function ActualizarNumeroCita(id As Integer, numeroCita As Boolean) As Boolean
        Return EjecutarSeguro("UPDATE agenda_yvette SET numero_cita = @numero_cita WHERE id_cita = @id",
            New Dictionary(Of String, Object) From {
                {"@id", id},
                {"@numero_cita", numeroCita}
            })
    End Function

    <WebMethod()>
    Public Shared Function ActualizarSM(id As Integer, sm As Boolean) As Boolean
        Return EjecutarSeguro("UPDATE agenda_yvette SET s_m = @s_m WHERE id_cita = @id",
            New Dictionary(Of String, Object) From {
                {"@id", id},
                {"@s_m", sm}
            })
    End Function

End Class