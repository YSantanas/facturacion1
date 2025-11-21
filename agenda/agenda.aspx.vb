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
            CargarCitas()
        Else
            ' ⭐ CRÍTICO: Aplicar filtros guardados después del postback
            AplicarFiltrosGuardados()
        End If
    End Sub

    ' ⭐ NUEVO MÉTODO: Restaurar estado de los filtros después del postback
    Private Sub AplicarFiltrosGuardados()
        ' Restaurar filtro de médico/color
        If ViewState("ColorSeleccionado") IsNot Nothing Then
            Dim colorGuardado As Integer = CInt(ViewState("ColorSeleccionado"))
            If colorGuardado > 0 Then
                chkMedico.Checked = True
                ddlMedico.Enabled = True
                ddlMedico.SelectedValue = colorGuardado.ToString()
            End If
        End If
        
        ' Restaurar filtro de pendiente
        If ViewState("PendienteSeleccionado") IsNot Nothing Then
            chkPendiente.Checked = True
            ddlPendiente.Enabled = True
            ddlPendiente.SelectedValue = ViewState("PendienteSeleccionado").ToString()
        End If
        
        ' Restaurar filtro de fechas
        If ViewState("FechaInicio") IsNot Nothing AndAlso ViewState("FechaFin") IsNot Nothing Then
            chkFecha.Checked = True
            txtFechaInicio.Enabled = True
            txtFechaFin.Enabled = True
            txtFechaInicio.Text = CDate(ViewState("FechaInicio")).ToString("yyyy-MM-dd")
            txtFechaFin.Text = CDate(ViewState("FechaFin")).ToString("yyyy-MM-dd")
        End If
    End Sub

    ' Cargar dropdown de médicos/colores
    Private Sub CargarMedicos()
        ddlMedico.Items.Clear()
        ddlMedico.Items.Add(New ListItem("Seleccione un color...", "0"))
        
        For Each kvp In Colores
            ddlMedico.Items.Add(New ListItem(kvp.Key, kvp.Value.ToString()))
        Next
    End Sub

    ' Cargar citas con filtros
    Private Sub CargarCitas(Optional ByVal colorNombre As Integer = 0, Optional ByVal pendiente As String = "Todos", Optional ByVal fechaInicio As DateTime? = Nothing, Optional ByVal fechaFin As DateTime? = Nothing)
        Dim sql As String = _
            "SELECT c.id_cita, c.fecha, c.hora, c.codigo, c.tv, c.asistio, c.pospuesto, c.observaciones, c.acabada, c.emitido, " &
            "c.numero_cita, c.deuda, c.s_m, c.id_medico_sustituto, " &
            "m.id_medico, m.nombre AS medico_nombre, m.apellido AS medico_apellido, m.color AS medico_color, " &
            "p.id_paciente, p.nombre AS paciente_nombre, p.apellido AS paciente_apellido " &
            "FROM agenda_yvette c " &
            "INNER JOIN medico_yvette m ON c.id_medico = m.id_medico " &
            "INNER JOIN paciente_yvette p ON c.id_paciente = p.id_paciente " &
            "WHERE 1=1"

        ' FILTRO POR COLOR
        If colorNombre > 0 Then
            sql &= " AND m.color = @color"
            ViewState("ColorSeleccionado") = colorNombre
        Else
            ViewState("ColorSeleccionado") = Nothing
        End If

        ' FILTRO POR PENDIENTE
        If pendiente = "Sí" Then
            sql &= " AND c.acabada = 0"
            ViewState("PendienteSeleccionado") = "Sí"
        ElseIf pendiente = "No" Then
            sql &= " AND c.acabada = 1"
            ViewState("PendienteSeleccionado") = "No"
        Else
            ViewState("PendienteSeleccionado") = Nothing
        End If

        ' FILTRO POR FECHAS
        If fechaInicio.HasValue AndAlso fechaFin.HasValue Then
            sql &= " AND c.fecha >= @fechaInicio AND c.fecha <= @fechaFin"
            ViewState("FechaInicio") = fechaInicio.Value
            ViewState("FechaFin") = fechaFin.Value
        Else
            ViewState("FechaInicio") = Nothing
            ViewState("FechaFin") = Nothing
        End If

        sql &= " ORDER BY c.fecha DESC, c.hora DESC;"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                If colorNombre > 0 Then cmd.Parameters.AddWithValue("@color", colorNombre)
                If fechaInicio.HasValue AndAlso fechaFin.HasValue Then
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value.Date)
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Value.Date)
                End If

                Dim dt As New DataTable()
                con.Open()
                dt.Load(cmd.ExecuteReader())
                gvCitas.DataSource = dt
                gvCitas.DataBind()
            End Using
        End Using
    End Sub

    ' Evento del botón Buscar
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim colorSeleccionado As Integer = 0
        Dim pendienteSeleccionado As String = "Todos"
        Dim fechaIni As DateTime? = Nothing
        Dim fechaFn As DateTime? = Nothing

        ' Obtener filtro de color (solo si el checkbox está marcado)
        If chkMedico.Checked AndAlso ddlMedico.SelectedValue <> "0" Then
            colorSeleccionado = Integer.Parse(ddlMedico.SelectedValue)
        End If

        ' Obtener filtro de pendiente
        If chkPendiente.Checked Then
            pendienteSeleccionado = ddlPendiente.SelectedValue
        End If

        ' Obtener filtro de fechas
        If chkFecha.Checked AndAlso Not String.IsNullOrEmpty(txtFechaInicio.Text) AndAlso Not String.IsNullOrEmpty(txtFechaFin.Text) Then
            fechaIni = DateTime.Parse(txtFechaInicio.Text)
            fechaFn = DateTime.Parse(txtFechaFin.Text)
        End If

        CargarCitas(colorSeleccionado, pendienteSeleccionado, fechaIni, fechaFn)
    End Sub

    ' Evento del botón Limpiar
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

        ' Resetear y deshabilitar controles
        ddlMedico.SelectedIndex = 0
        ddlMedico.Enabled = False
        txtFechaInicio.Text = ""
        txtFechaFin.Text = ""
        txtFechaInicio.Enabled = False
        txtFechaFin.Enabled = False
        txtHora.Text = ""
        txtHora.Enabled = False
        ddlPendiente.SelectedIndex = 0
        ddlPendiente.Enabled = False
        ddlGestionada.SelectedIndex = 0
        ddlGestionada.Enabled = False
        txtNombre.Text = ""
        txtNombre.Enabled = False
        ddlNombre.Enabled = False
        ddlEstetica.Enabled = False
        ddlTV.Enabled = False

        ' Limpiar ViewState
        ViewState("ColorSeleccionado") = Nothing
        ViewState("PendienteSeleccionado") = Nothing
        ViewState("FechaInicio") = Nothing
        ViewState("FechaFin") = Nothing

        ' Recargar sin filtros
        CargarCitas()
    End Sub

    ' ⭐ EVENTO CORREGIDO: Aplicar clases CSS a las filas según el color del médico
Protected Sub gvCitas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCitas.RowDataBound
    If e.Row.RowType = DataControlRowType.DataRow Then
        ' ⭐ SOLO aplicar color si hay un filtro de médico/color activo
        If ViewState("ColorSeleccionado") IsNot Nothing AndAlso CInt(ViewState("ColorSeleccionado")) > 0 Then
            Try
                Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim colorMedico As Integer = If(IsDBNull(drv("medico_color")), 0, Convert.ToInt32(drv("medico_color")))
                
                ' DEBUG: Descomentar para ver qué color tiene cada fila
                ' System.Diagnostics.Debug.WriteLine("ID Cita: " & drv("id_cita").ToString() & " - Color Médico: " & colorMedico.ToString())
                
                ' Aplicar clase CSS según el color del médico
                Select Case colorMedico
                    Case 1
                        e.Row.CssClass = "bg-medico-1 text-center"
                    Case 2
                        e.Row.CssClass = "bg-medico-2 text-center"
                    Case 3
                        e.Row.CssClass = "bg-medico-3 text-center"
                    Case 4
                        e.Row.CssClass = "bg-medico-4 text-center"
                    Case 5
                        e.Row.CssClass = "bg-medico-5 text-center"
                    Case 6
                        e.Row.CssClass = "bg-medico-6 text-center"
                    Case 7
                        e.Row.CssClass = "bg-medico-7 text-center"
                    Case 8
                        e.Row.CssClass = "bg-medico-8 text-center"
                    Case 9
                        e.Row.CssClass = "bg-medico-9 text-center"
                    Case 10
                        e.Row.CssClass = "bg-medico-10 text-center"
                    Case Else
                        e.Row.CssClass = "bg-medico-default text-center"
                End Select
            Catch ex As Exception
                ' En caso de error, aplicar estilo por defecto
                e.Row.CssClass = "text-center"
                System.Diagnostics.Debug.WriteLine("Error aplicando color: " & ex.Message)
            End Try
        Else
            ' Sin filtro de color: solo aplicar centrado
            e.Row.CssClass = "text-center"
        End If
    End If
End Sub

    ' Método Ajax para actualizar pendiente
    <WebMethod()>
    Public Shared Function ActualizarPendiente(id As Integer, pendiente As Boolean) As Boolean
        Dim connectionString As String = "Server=93.90.25.27;Database=OP21F20112025;User ID=sa;Password=prueba2025_%;;TrustServerCertificate=True;"
        Dim sql As String = "UPDATE agenda_yvette SET acabada = @acabada WHERE id_cita = @id"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.Parameters.AddWithValue("@acabada", Not pendiente)
                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        Return True
    End Function

End Class