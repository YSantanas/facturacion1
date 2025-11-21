Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Partial Class CONTROL_herramientas_resolucion_tickets
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Public Sub leer_opciones()

        Try

            'Limpio el control
            DDL_opcion_ticket.Items.Clear()
            Me.DDL_opcion_ticket.Items.Add(New ListItem("Sin Finalizar", "Sin Finalizar"))
            Me.DDL_opcion_ticket.Items.Add(New ListItem("Finalizados", "Finalizados"))
            Me.DDL_opcion_ticket.Items.Add(New ListItem("Todos", "Todos"))

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Public Sub leer_Utilidad()

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT seccion FROM [kernel_facturacion].[dbo].IA_menu GROUP BY seccion ORDER BY seccion;")

            'Limpio el control
            DDL_utilidad_visual.Items.Clear()
            Me.DDL_utilidad_visual.Items.Add(New ListItem("", ""))

            For x = 0 To tabla_consulta.Rows.Count - 1
                Me.DDL_utilidad_visual.Items.Add(New ListItem(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tabla_consulta.Rows(x)("seccion").ToString), tabla_consulta.Rows(x)("seccion").ToString))
            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Sub leer_apartado(ByVal utilidad As String)

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT modulo FROM [kernel_facturacion].[dbo].IA_menu WHERE seccion='" & utilidad & "' ORDER BY modulo;")

            'Asigno
            DDL_apartado_visual.Items.Clear()

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tabla_consulta.Rows(x)("modulo").ToString), tabla_consulta.Rows(x)("modulo").ToString)
                Me.DDL_apartado_visual.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            'Control de Seguridad
            If Session("nombre_master") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            'Asigno el n_consulta
            gridview_consulta.PageSize = 25

            'Ver fondo
            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                ''Restricción de Usuarios
                'If tabla_usuario.Rows(0)("nivel") = "Invitado" Or tabla_usuario.Rows(0)("nivel") = "Usuario Restringido" Or tabla_usuario.Rows(0)("nivel") = "Usuario" Then
                '    'Bloque Jquery
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                'End If

                'Fecha Inicial y Final
                txt_fecha_inicio.Text = "01/" & (Date.Now).ToString("MM") & "/" & Year(Date.Now)
                txt_fecha_final.Text = Now.ToShortDateString

                'Leer_opciones
                leer_opciones()

                'Leer Utilidad
                leer_Utilidad()

                'Asigno foco
                txt_fecha_inicio.Focus()

                'Cargar GV
                cargar_GV()

            End If

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Private Sub Lkb_reset_Click(sender As Object, e As EventArgs) Handles Lkb_reset.Click

        Try

            'refrescar
            cargar_GV()

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub btn_consultar_Click(sender As Object, e As EventArgs) Handles btn_consultar.Click

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Cargar GV
            cargar_GV()

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Datos Usuario
            lbl_informacion.Text = gridview_consulta.PageCount & " Página(s) - Tiempo (" & tspan.ToString("s\:ff") & " segundos)"

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Sub cargar_GV()

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                'Declaro
                Dim memComando As New SqlCommand

                Select Case DDL_opcion_ticket.SelectedItem.ToString

                    Case "Sin Finalizar"

                        memComando.CommandText = "SELECT tickets.*,empresa.nombre_fiscal,usuarios.email,usuarios.nombre " &
                        "FROM [kernel_facturacion].[dbo].[tickets] LEFT JOIN [kernel_facturacion].[dbo].empresa ON tickets.id_empresa=empresa.id " &
                        "LEFT JOIN [kernel_facturacion].[dbo].usuarios ON tickets.id_usuario=usuarios.Id " &
                        "WHERE tickets.fecha_creacion BETWEEN '" & txt_fecha_inicio.Text & "' AND '" & txt_fecha_final.Text & "' " &
                        "AND fecha_respuesta is null " &
                        "ORDER BY id DESC;"

                    Case "Finalizados"

                        memComando.CommandText = "SELECT tickets.*,empresa.nombre_fiscal,usuarios.email,usuarios.nombre,usuarios.email " &
                        "FROM [kernel_facturacion].[dbo].[tickets] LEFT JOIN [kernel_facturacion].[dbo].empresa ON tickets.id_empresa=empresa.id " &
                        "LEFT JOIN [kernel_facturacion].[dbo].usuarios ON tickets.id_usuario=usuarios.Id " &
                        "WHERE tickets.fecha_creacion BETWEEN '" & txt_fecha_inicio.Text & "' AND '" & txt_fecha_final.Text & "' " &
                        "AND fecha_respuesta is not null " &
                        "ORDER BY id DESC;"

                    Case Else

                        memComando.CommandText = "SELECT tickets.*,empresa.nombre_fiscal,usuarios.email,usuarios.nombre,usuarios.email " &
                        "FROM [kernel_facturacion].[dbo].[tickets] LEFT JOIN [kernel_facturacion].[dbo].empresa ON tickets.id_empresa=empresa.id " &
                        "LEFT JOIN [kernel_facturacion].[dbo].usuarios ON tickets.id_usuario=usuarios.Id " &
                        "WHERE tickets.fecha_creacion BETWEEN @txt_fecha_inicio AND @txt_fecha_final ORDER BY id DESC;"

                End Select
                memComando.Parameters.Add(New SqlParameter("@txt_fecha_inicio", Data.SqlDbType.DateTime))
                memComando.Parameters("@txt_fecha_inicio").Value = txt_fecha_inicio.Text
                memComando.Parameters.Add(New SqlParameter("@txt_fecha_final", Data.SqlDbType.DateTime))
                memComando.Parameters("@txt_fecha_final").Value = txt_fecha_final.Text

                memComando.Connection = memConn

                'Creamos un adaptador de datos
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

                'Llenamos de datos
                adapter.Fill(tabla_consulta)

                'Cerramos
                adapter.Dispose()
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Realizo la consulta
            gridview_consulta.DataSource = tabla_consulta
            gridview_consulta.DataBind()

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                'Si no esta activo
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("fecha_respuesta").ToString = "" Then

                    'DesAsigno la propiedas  
                    For x = 1 To 7
                        e.Row.Cells(x).CssClass = "gv_rojo"
                    Next

                Else

                    'DesAsigno la propiedas  
                    For x = 1 To 7
                        e.Row.Cells(x).CssClass = "gv_verde"
                    Next

                End If

            End If

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub gridview_consulta_DataBound(sender As Object, e As EventArgs) Handles gridview_consulta.DataBound

        Try

            'Control para menus de exportar
            If gridview_consulta.Rows.Count = 0 Then
                img_exportar_excel.Visible = False
                img_exportar_txt.Visible = False
                lbl_informacion.Visible = False
            Else
                img_exportar_excel.Visible = True
                img_exportar_txt.Visible = True
                lbl_informacion.Visible = True
            End If

            'Limpio 
            Lt_error.Text = Nothing

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub gridview_consulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridview_consulta.PageIndexChanging

        Try

            'Actualizar posicion y cosnulta
            gridview_consulta.PageIndex = e.NewPageIndex
            cargar_GV()

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub img_exportar_excel_Click(sender As Object, e As EventArgs) Handles img_exportar_excel.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Tickets", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "xlsx"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_exportar_excel_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_exportar_excel_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_txt_Click(sender As Object, e As EventArgs) Handles img_exportar_txt.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Tickets", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "txt"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_exportar_txt_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_exportar_txt_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "ver_tickets") Then

                'cargo los datos
                DDL_utilidad_visual.SelectedIndex = DDL_utilidad_visual.Items.IndexOf(DDL_utilidad_visual.Items.FindByValue(gridview_consulta.DataKeys(index).Item("utilidad").ToString()))
                leer_apartado(gridview_consulta.DataKeys(index).Item("utilidad").ToString())
                DDL_apartado_visual.SelectedIndex = DDL_apartado_visual.Items.IndexOf(DDL_apartado_visual.Items.FindByValue(gridview_consulta.DataKeys(index).Item("apartado").ToString()))
                __txt_consulta.Text = gridview_consulta.DataKeys(index).Item("consulta").ToString()
                __txt_respuesta.Text = gridview_consulta.DataKeys(index).Item("respuesta").ToString()
                lbl_fecha_respuesta.Text = Mid(gridview_consulta.DataKeys(index).Item("fecha_respuesta").ToString(), 1, 10) & " - " & gridview_consulta.DataKeys(index).Item("hora_respuesta").ToString()

                'Restricciones
                If gridview_consulta.DataKeys(index).Item("fecha_respuesta").ToString <> "" Then

                    'Asigno
                    btn_grabar.Visible = False
                    lbl_fecha_respuesta.Visible = True

                Else

                    'Asigno
                    btn_grabar.Visible = True
                    lbl_fecha_respuesta.Visible = False

                End If

                'Asignamos
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "tab_ok", funciones_globales.modal_register("$('#modal_ver_tickets').modal('show')"), True)

            End If

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Private Sub btn_grabar_Click(sender As Object, e As EventArgs) Handles btn_grabar.Click

        Try

            'Excepcion
            If __txt_respuesta.Text.Count = 68 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#__txt_consulta').focus();}, 100);" & funciones_globales.modal_register("$('#modal_ver_tickets').modal('show');") & "error('El Campo Respuesta no puede estar vacío.')", True)
                Exit Sub
            End If

            'Asigno
            LT_mensaje_confirmar.Text = "¿Está seguro de enviar Respuesta?"

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_confirmar').modal('show');"), True)

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Private Sub btn_grabar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_grabar_confirmar.Click

        Try

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "UPDATE tickets SET fecha_respuesta=@fecha_respuesta,hora_respuesta=@hora_respuesta,respuesta=@respuesta WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("Id").ToString() & ";"
                memComando.Parameters.Add("@fecha_respuesta", System.Data.SqlDbType.Date)
                memComando.Parameters("@fecha_respuesta").Value = DateTime.Today
                memComando.Parameters.Add("@hora_respuesta", System.Data.SqlDbType.Time)
                memComando.Parameters("@hora_respuesta").Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@respuesta", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@respuesta").Value = __txt_respuesta.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Liberamos recursos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Cargar GV
            cargar_GV()

            'Compongo Email
            email_cliente(gridview_consulta.DataKeys(txt_index.Text).Item("email").ToString(), gridview_consulta.DataKeys(txt_index.Text).Item("Id").ToString(), gridview_consulta.DataKeys(txt_index.Text).Item("nombre_usuario").ToString(), gridview_consulta.DataKeys(txt_index.Text).Item("consulta").ToString())

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Se ha resuelto el Tickets correctamente.');", True)

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

    Sub email_cliente(ByRef direccion As String, ByVal id As String, ByVal nombre_usuario As String, ByVal consulta As String)

        Try

            'Aqui mando un correo electrónico a los destinatarios que me diga el cliente.
            Dim correo As New System.Net.Mail.MailMessage
            correo.From = New System.Net.Mail.MailAddress("asistencia.optimus@gmail.com")
            correo.To.Add(direccion)
            correo.Bcc.Add("asistencia.optimus@gmail.com")
            correo.Subject = "Respuesta sobre el Ticket Nº: " & id & " de Optimus Facturación."

            'Monto el cuerpo del correo
            Dim mensajemail As String

            mensajemail = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><head>" &
            "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'> " &
            "<title>Restauración de Claves de Cloud Projects</title>" &
            "<style type = 'text/css'>" &
            "/* Client - specific Styles */" &
            "#outlook a{padding:0;} /* Force Outlook To provide a 'view In browser' button. */" &
            "body{width:100% !important;} .ReadMsgBody{width:100%;} .ExternalClass{width:100%;} /* Force Hotmail to display emails at full width */ " &
            "body{-webkit - Text - size - adjust: none;} /* Prevent Webkit platforms from changing Default text sizes. */ " &
            "/* Reset Styles */ " &
            "body{margin: 0; padding:0;} " &
            "img{border: 0; line-height:100%; outline:none; text-decoration:none;} " &
            "Table td{border-collapse: collapse;} " &
            "</style> " &
            "</head> " &
            "<body leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0' style='font-family:Verdana,Arial,sans-serif;'> " &
            "<table border='0' cellpadding='0' cellspacing='0' width='100%' style='width:100%;'>" &
            "<tr>" &
            "<td style='width:25%;'></td>" &
            "<td style='width:50%; text-align:center;'><span style='color: #4D4D4D'>Se envió este correo electrónico desde una dirección exclusiva de notificaciones que no recibe correos electrónicos.<u>No respondas a este mensaje</u>.</span></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;'></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style='width:50%;text-align:center;'><table style='width:100%;'><tr><td style='text-align:left;'><span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Hola,<span style='color: #007bff;'> " & System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nombre_usuario) & "</span></span></td><td style='text-align:right;'><img src='https://www.optimus-soluciones.com/facturacion/imagenes/logo/logo_empresa.png' style='height:81px;'></td></tr></table></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:40px;'></td>" &
            "<td style='width:50%;text-align:left;'>" &
            "<span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Sobre la consulta que nos planteaste:</span><br><br>" &
            "<span style='color: #4D4D4D;font-size:14px;'>" & consulta & "</span><br><br>" &
            "<span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Te recomendamos esta solución:</span><br><br>" &
            "<span style='color: #4D4D4D;font-size:14px;'>" & __txt_respuesta.Text & "</span><br><br>" &
            "</td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
             "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;'></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style='width:50%;text-align:left;'><span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>El equipo de <span style='color: #000000;'>Optimus Facturación.</span></span></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%; height:30px;'></td>" &
            "<td style='width:50%; border: 1px; border-width: 2px; border-color: red;><hr></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td><span style='color: #b3b3b3'><b>Protege tu contraseña</b><br>" &
            "El personal de Optimus Contabilidad NUNCA te pedirá tu contraseña por correo electrónico. Sólo necesitarás introducirla cuando inicies sesión en Optimus Contabilidad. Siempre iniciarás sesión por medio de una conexión segura y te pediremos que verifiques que la dirección del explorador comience exactamente con https://www.optimus-soluciones.com. También debería aparecer el símbolo de un candado pequeño para indicar que la conexión es segura. " &
            "<br><br>Ten cuidado con mensajes de correo electrónico que soliciten información de cuenta o exijan acciones inmediatas. La misma recomendación rige para sitios web con direcciones atípicas. " &
            "<br><br>© " & Year(DateTime.Now) & " Optimiza Gestores Documentales S.L.</span></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "</table>" &
            "</body></html>"

            correo.Body = mensajemail
            correo.IsBodyHtml = True
            correo.Priority = System.Net.Mail.MailPriority.Normal
            Dim smtp As New System.Net.Mail.SmtpClient

            Dim nombre As String = "asistencia.optimus@gmail.com"
            Dim contrasena As String = "pcxv zxsv qvwg uqso"
            smtp.UseDefaultCredentials = False
            smtp.EnableSsl = True
            smtp.Port = 587
            smtp.Host = "smtp.gmail.com"
            smtp.Credentials = New System.Net.NetworkCredential(nombre, contrasena)

            'Envio el correo a los destinatarios
            smtp.Send(correo)

        Catch ex As Exception

            'Asigno
            Lt_error.Text = ex.Message

        End Try

    End Sub

End Class
