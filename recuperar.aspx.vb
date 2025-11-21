Imports System.Data
Imports System.Data.SqlClient

Partial Class login
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then

                'Carga de fondo
                Dim numero_aleatorio As New Random
                cuerpo.Attributes.Add("style", "background:url('imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg') no-repeat center center fixed;")

                'Carga Fecha y Día
                lbl_fecha.Text = " - " & DateTime.Now.ToString("dd/MM/yyyy")

                'Si llega con la key
                If Not Request.Params("key") Is Nothing Then

                    Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
                    querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, Request.Params("key"))

                    'Obtenemos las variables que vienen dentro del querystringSeguro
                    Dim nombre_usuario As String = querystringSeguro("nombre_usuario")
                    Dim fecha_hora As DateTime = querystringSeguro("fecha_hora")

                    'Si la fecha es por debajo de la actual cancelo el enlace
                    If fecha_hora.AddHours(2) < DateTime.Now() Then

                        Lt_caducado.Text = "<span class='text-primary' style='font-weight: bolder;'>" & nombre_usuario & "</span>, has solicitado modificar tu PassWord el <span class='text-primary' style='font-weight: bolder;'>" & fecha_hora.ToShortDateString & "</span> a las <span class='text-primary' style='font-weight: bolder;'>" & fecha_hora.ToShortTimeString & "</span>.<br>Ese enlace sólo estaba disponible hasta la(s) <span class='text-primary' style='font-weight: bolder;'>" & fecha_hora.AddHours(2).ToShortTimeString & "</span>."

                        'Mostramos los menus
                        Ph_recordar.Visible = False
                        PH_restaurar.Visible = False
                        PH_recordar_OK.Visible = False
                        Ph_cambiar_ok.Visible = False
                        PH_caducado.Visible = True

                        'Registro como bloque en local para el jquery
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_caducado", "hablar('" & nombre_usuario & ",  has solicitado modificar tu PassWord el " & fecha_hora.ToShortDateString & " a las " & fecha_hora.ToShortTimeString & ".Ese enlace sólo estaba disponible hasta la(s) " & fecha_hora.AddHours(2).ToShortTimeString & "');", True)
                        Exit Sub

                    End If

                    'Componer mensaje
                    lbl_restaurar_clave.Text = nombre_usuario

                    'Mostramos los menus
                    Ph_recordar.Visible = False
                    PH_restaurar.Visible = True
                    PH_recordar_OK.Visible = False
                    Ph_cambiar_ok.Visible = False
                    PH_caducado.Visible = False

                Else

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "hablar('Recuperar contraseña.');", True)

                    'Asigno
                    txt_email.Focus()

                End If

            End If

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_recuperar_Click(sender As Object, e As EventArgs) Handles btn_recuperar.Click

        Try

            'Variables
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT TOP(1) id,email,nombre FROM usuarios WHERE email=@email;"
                memComando.Parameters.Add(New SqlParameter("email", txt_email.Text))
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

            'Excepcion
            If tabla_consulta.Rows.Count = 0 Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Este correo no se encuentra Registrado.');setTimeout(function () { $('#txt_email').focus();$('#txt_email').select(); }, 100);", True)

            Else

                'Compongo Email
                email_cliente(tabla_consulta.Rows(0)("email").ToString, tabla_consulta.Rows(0)("id").ToString, tabla_consulta.Rows(0)("nombre").ToString)

                'Mensaje al usuario
                lbl_correo_recuperacion.Text = tabla_consulta.Rows(0)("email").ToString

                'Mostramos los Ph
                Ph_recordar.Visible = False
                PH_restaurar.Visible = False
                PH_recordar_OK.Visible = True
                Ph_cambiar_ok.Visible = False
                PH_caducado.Visible = False

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "hablar2", "hablar('Ya lo tienes solucionado, te hemos enviado instrucciones a tu e-mail de " & tabla_consulta.Rows(0)("email").ToString & "');", True)

            End If

            'Limpio
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_recuperar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub email_cliente(ByRef direccion As String, ByVal id As String, ByVal nombre_usuario As String)

        Try

            Dim key As TSHAK.Components.SecureQueryString
            key = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8})
            key("id") = id
            key("nombre_usuario") = nombre_usuario
            key("fecha_hora") = DateTime.Now()

            Dim secure_active As String = HttpContext.Current.Server.UrlEncode(key.ToString)

            'Aqui mando un correo electrónico a los destinatarios que me diga el cliente.
            Dim correo As New System.Net.Mail.MailMessage
            correo.From = New System.Net.Mail.MailAddress("recuperar.optimus@gmail.com")
            correo.To.Add(direccion)
            correo.Bcc.Add("recuperar.optimus@gmail.com")
            correo.Subject = "Claves de Optimus Facturación."

            'Monto el cuerpo del correo
            Dim mensajemail As String

            mensajemail = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><head>" &
            "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'> " &
            "<title>Restauración de Claves de IO Contabilidad</title>" &
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
            "<td style='width:50%; text-align:center;'><span style='color: #b3b3b3'>Se envió este correo electrónico desde una dirección exclusiva de notificaciones que no recibe correos electrónicos. No respondas a este mensaje.</span></td>" &
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
            "<span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Pasos para recuperar la contraseña:</span><br><br>" &
            "<span style='color: #4D4D4D;font-size:14px;'>Restablece tu contraseña pulsando sobre este enlace: <br><br><a href='https://www.optimus-soluciones.com/facturacion/recuperar.aspx?key=" & secure_active & "'>" & secure_active & "</a><br><br>Éste vínculo sólo estará activo durante 2 horas desde el momento de su recepción, una vez superado el límite de tiempo, el enlace dejará de funcionar y deberás volver a solicitar un cambio de contraseña.</span> " &
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
            Dim nombre As String = "recuperar.optimus@gmail.com"
            Dim contrasena As String = "nafi jrwz lkok eolb "
            smtp.UseDefaultCredentials = False
            smtp.EnableSsl = True
            smtp.Port = 587
            smtp.Host = "smtp.gmail.com"
            smtp.Credentials = New System.Net.NetworkCredential(nombre, contrasena)

            'Envio el correo a los destinatarios
            smtp.Send(correo)

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error email_cliente: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_cambiar_Click(sender As Object, e As EventArgs) Handles btn_cambiar.Click

        Try

            Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
            querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, Request.Params("key"))

            'Obtenemos las variables que vienen dentro del querystringSeguro
            Dim id As Integer = CInt(querystringSeguro("id"))

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "UPDATE usuarios SET " &
                "password=@password " &
                "WHERE id=" & id & ";"
                memComando.Parameters.Add(New SqlParameter("password", txt_password.Text))
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Asigno
            'Mostramos los Ph
            Ph_recordar.Visible = False
            PH_restaurar.Visible = False
            PH_recordar_OK.Visible = False
            Ph_cambiar_ok.Visible = True

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "hablar3", "hablar('Listo, ya puedes acceder a Optimus Facturación.');", True)

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_cambiar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
