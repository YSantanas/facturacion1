Imports System.Data
Imports System.Data.SqlClient

Partial Class configuracion_email
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_ssl()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            DDL_ssl.Items.Clear()

            'Cargo los detalles
            Me.DDL_ssl.Items.Add(New System.Web.UI.WebControls.ListItem("Activado", "True"))
            Me.DDL_ssl.Items.Add(New System.Web.UI.WebControls.ListItem("Desactivado", "False"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_ssl", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_ssl: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_email()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE Id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Asigno
            txt_email.Text = tabla_consulta.Rows(0)("email_envios").ToString
            txt_password.Text = tabla_consulta.Rows(0)("password_envios").ToString
            txt_smtp.Text = tabla_consulta.Rows(0)("smtp_envios").ToString
            txt_port.Text = tabla_consulta.Rows(0)("email_port").ToString
            DDL_ssl.SelectedIndex = DDL_ssl.Items.IndexOf(DDL_ssl.Items.FindByValue(tabla_consulta.Rows(0)("email_ssl")))

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(0, tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_email", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_email: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            'Ver fondo
            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Leer_ssl
                leer_ssl()

                'Leer Tipos
                leer_email()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_actualizar_Click(sender As Object, e As EventArgs) Handles btn_actualizar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepción
            If funciones_globales.IsValidEmail(txt_email.Text) = False Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "error", "error('El campo Email no es válido.');", True)
                Exit Sub

            End If

            'Probar el envío
            Dim correo As New System.Net.Mail.MailMessage
            correo.From = New System.Net.Mail.MailAddress(txt_email.Text)
            correo.To.Add(txt_email.Text)
            correo.Subject = "Test de Email"
            correo.Body = "Se envió el email correctamente."
            correo.IsBodyHtml = True
            correo.Priority = System.Net.Mail.MailPriority.Normal

            Dim smtp As New System.Net.Mail.SmtpClient
            Dim nombre As String = txt_email.Text
            Dim contrasena As String = txt_password.Text
            smtp.UseDefaultCredentials = False
            smtp.EnableSsl = DDL_ssl.SelectedValue
            smtp.Port = txt_port.Text
            smtp.Host = txt_smtp.Text
            smtp.Credentials = New System.Net.NetworkCredential(nombre, contrasena)

            Try

                'Envio el correo a los destinatarios
                smtp.Send(correo)

            Catch ex As Exception

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "error", "error('" & ex.Message & ".');", True)
                Exit Sub

            End Try

            'Asignamos
            LT_mensaje.Text = "¿Está seguro de actualizar los datos? "

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_confirmar", funciones_globales.modal_register("$('#modal_confirmar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(0, tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_actualizar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_actualizar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_grabar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_grabar_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE empresa SET email_envios=@email_envios, password_envios=@password_envios, smtp_envios=@smtp_envios, email_ssl=@email_ssl, email_port=@email_port " &
                    "WHERE id=" & tabla_empresa.Rows(0)("Id") & ";"
                memComando.Parameters.Add("@email_envios", Data.SqlDbType.VarChar, 200).Value = txt_email.Text
                memComando.Parameters.Add("@password_envios", Data.SqlDbType.VarChar, 50).Value = txt_password.Text
                memComando.Parameters.Add("@smtp_envios", Data.SqlDbType.VarChar, 50).Value = txt_smtp.Text
                memComando.Parameters.Add("@email_ssl", Data.SqlDbType.Bit).Value = DDL_ssl.SelectedValue
                memComando.Parameters.Add("@email_port", Data.SqlDbType.Int).Value = txt_port.Text

                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Email", "Modificó el Email: " & txt_email.Text & ", Contraseña: " & txt_password.Text & ", Servidor Salida: " & txt_smtp.Text & ", Puerto: " & txt_port.Text & " y SSL: " & DDL_ssl.SelectedItem.ToString & ".(Tiempo:  " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo datos de tipo de impuestos
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Datos modificados correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(0, tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
