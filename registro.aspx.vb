
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class login
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then

                'Carga de fondo
                Dim aleatorio As New Random
                Dim numero As Integer = aleatorio.Next(1, 10)
                cuerpo.Attributes.Add("style", "background:url('imagenes/fondo/fondo_aplicacion" & numero & ".jpg') no-repeat center center fixed;")

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "hablar('Alta de cuenta.');" & funciones_globales.modal_register("$('#modal_login').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_crear_bd_Click(sender As Object, e As EventArgs) Handles btn_crear_bd.Click

        Try

            'Ataco a la conexion del programa
            Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion"
            Dim memConn As New SqlConnection(ruta_base)
            memConn.Open()
            Dim memComando As New SqlCommand
            Dim memDatos As SqlDataReader

            'Restriccion para los Usuarios ya registrados
            memComando.CommandText = "SELECT TOP(1) usuarios.Id,nombre,segundo_apellido ,email,password ,usuarios.codigo_empresa ,back_imagen ,nivel  ,empresa.demo " &
                "FROM usuarios,usuarios_empresas,empresa " &
                "WHERE usuarios.Id =usuarios_empresas.id_usuario " &
                "And usuarios_empresas .id_empresa = empresa .Id " &
                "And email='" & Trim(txt_email.Text) & "' " &
                "ORDER BY Id;"
            memComando.Connection = memConn

            memDatos = memComando.ExecuteReader

            If memDatos.HasRows Then
                Do While memDatos.Read

                    If memDatos.Item("nivel").ToString <> "Propietario" Then
                        'Registro como bloque en local para el jquery
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error2", "error('Ya existe ese Email en nuestro registro.');", True)
                        Exit Sub
                    Else
                        'Oculto los contendores
                        PH_login.Visible = False
                        PH_multi_empresa.Visible = True
                        PH_acceder.Visible = False

                        'Mensaje
                        lbl_aviso_multiempresa.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(memDatos.Item("nombre").ToString)

                        'Registro como bloque en local para el jquery
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_hablar", "hablar('" & memDatos.Item("nombre").ToString & " ya tienes empresas registradas con este e-mail, ¿Deseas asociar esta nueva empresa a tu cuenta?');", True)
                        txt_id_usuario.Text = memDatos.Item("id").ToString
                        txt_demo.Text = memDatos.Item("demo").ToString
                        Exit Sub

                    End If

                Loop
            End If

            ' Cierro la base de datos
            memDatos.Close()
            memComando.Dispose()
            memConn.Close()

            'Lanzo el proceso de Crear la empresa
            crear_empresa("")

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_crear_bd_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub crear_empresa(ByVal id_usuario_acumulado As String)

        Try

            'Ataco a la conexion del programa
            Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion"
            Dim memConn As New SqlConnection(ruta_base)
            Dim memComando As New SqlCommand
            Dim memDatos As SqlDataReader

            '1º )Necesito acceder al último numero de BBDD----------------------------------------------------------------------------------------------------------
            memComando.CommandText = "SELECT * FROM id_bbdd;"
            memComando.Connection = memConn
            memConn.Open()
            memDatos = memComando.ExecuteReader

            'Id de la bbdd
            Dim id_bbdd As Integer = Nothing
            If memDatos.HasRows Then
                Do While memDatos.Read
                    id_bbdd = memDatos.Item("id_bbdd").ToString + 1
                Loop
            End If

            ' Cierro la base de datos
            memDatos.Close()
            memComando.Dispose()
            memConn.Close()

            '2º) Necesito crear el nombre de la BBDD----------------------------------------------------------------------------------------------------------
            Dim nombre_final As String = "OP" & id_bbdd & "F"
            Dim dia As String = Nothing
            Select Case Date.Now.Day.ToString.Count
                Case Is = 1 : dia = "0" & Date.Now.Day
                Case Else
                    dia = Date.Now.Day
            End Select
            Dim mes As String = Nothing
            Select Case Date.Now.Month.ToString.Count
                Case Is = 1 : mes = "0" & Date.Now.Month
                Case Else
                    mes = Date.Now.Month
            End Select
            nombre_final += dia & mes & Date.Now.Year

            '3º) Genero la BBDD
            'Ataco a la conexion del programa
            ruta_base = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=master"
            memConn.Open()

            'Ataco a la conexion del programa
            memComando.CommandText = "CREATE DATABASE " & nombre_final
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Vacio buffer
            memComando.Dispose()
            memConn.Close()

            '4º) Actualizo el valor de la BBDD----------------------------------------------------------------------------------------------------------
            ruta_base = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion"
            memConn.Open()
            memComando.CommandText = "UPDATE id_bbdd SET id_bbdd=id_bbdd+1;"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()
            memConn.Close()

            '5º) Cargo con los nuevos datos ----------------------------------------------------------------------------------------------------------
            Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & nombre_final)
            Try

                conn.Open()

                'Cargo el Fichero
                Dim script As String = File.ReadAllText(Server.MapPath("") & "\sql\maestra.sql")
                'remplazo
                script = script.Replace("car2", nombre_final)

                Dim commandStrings As IEnumerable(Of String) = Regex.Split(script, "^\s*GO\s*$", RegexOptions.Multiline Or RegexOptions.IgnoreCase)
                For Each commandString As String In commandStrings
                    If commandString.Trim() <> "" Then
                        Dim meme As New SqlCommand(commandString, conn)
                        meme.ExecuteNonQuery()
                    End If
                Next

            Catch er As SqlException
                Response.Write("Error: " & er.Message)
            Finally
                conn.Close()
            End Try

            '6º) Cargo una nueva empresa  ----------------------------------------------------------------------------------------------------------
            'Obtengo el último ID
            ruta_base = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion"
            memConn.Open()

            memComando.CommandText = "SELECT TOP(1) Id FROM empresa ORDER BY id desc;"
            memComando.Connection = memConn
            memDatos = memComando.ExecuteReader

            'Id de la esmpresa
            Dim id_empresa As Integer = Nothing
            If memDatos.HasRows Then
                Do While memDatos.Read
                    id_empresa = memDatos.Item("Id").ToString + 1
                Loop
            End If

            'Cierro la base de datos
            memDatos.Close()
            memComando.Dispose()

            '7º) Inserto en Empresa ----------------------------------------------------------------------------------------------------------
            memComando.CommandText = "INSERT INTO [empresa] (Id,fecha_creacion,hora_creacion,codigo_empresa,ruta_base_datos,demo) VALUES (@id,@fecha_creacion,@hora_creacion,@codigo_empresa,@ruta_base_datos,'" & txt_demo.Text & "');"
            memComando.Parameters.Add("@id", System.Data.SqlDbType.Int)
            memComando.Parameters("@id").Value = id_empresa
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date)
            memComando.Parameters("@fecha_creacion").Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time)
            memComando.Parameters("@hora_creacion").Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@codigo_empresa", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@codigo_empresa").Value = nombre_final
            memComando.Parameters.Add("@ruta_base_datos", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@ruta_base_datos").Value = nombre_final
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Si es una multiempresa, no se necesita grabar ese nuevo usuario
            Dim id_usuario As Integer = Nothing
            If id_usuario_acumulado = "" Then
                '8º) Inserto en Usuarios ----------------------------------------------------------------------------------------------------------

                memComando.CommandText = "INSERT INTO [usuarios] (nombre, email, password,codigo_empresa,nivel,fecha_creacion,hora_creacion,ip,posicion) VALUES (@nombre, @email, @password,@codigo_empresa2,'Propietario',@fecha,@hora,@ip,@posicion);"
                memComando.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@nombre").Value = txt_nombre.Text
                memComando.Parameters.Add("@email", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@email").Value = Trim(txt_email.Text)
                memComando.Parameters.Add("@password", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@password").Value = Trim(txt_password.Text)
                memComando.Parameters.Add("@codigo_empresa2", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@codigo_empresa2").Value = id_empresa
                memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
                memComando.Parameters("@fecha").Value = DateTime.Today
                memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
                memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@ip", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@ip").Value = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString()
                memComando.Parameters.Add("@posicion", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@posicion").Value = txt_latitud.Text & "|" & txt_longitud.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                '9ª) Obtengo el id de usuario
                'Obtengo el último ID usuario
                memComando.CommandText = "SELECT TOP(1) Id FROM usuarios ORDER BY id desc;"
                memDatos = memComando.ExecuteReader

                'Id de la esmpresa
                If memDatos.HasRows Then
                    Do While memDatos.Read
                        id_usuario = memDatos.Item("Id").ToString
                    Loop
                End If

                ' Cierro la base de datos
                memDatos.Close()
                memComando.Dispose()
            Else
                id_usuario = id_usuario_acumulado
            End If

            '10º) Inserto en Usuarios - Empresa ----------------------------------------------------------------------------------------------------------
            memComando.CommandText = "INSERT INTO [usuarios_empresas] (id_usuario, id_empresa) VALUES ( " & id_usuario & " , " & id_empresa & ");"
            memComando.ExecuteNonQuery()

            'Oculto los contendores
            PH_login.Visible = False
            PH_multi_empresa.Visible = False
            PH_acceder.Visible = True

            'Enviar un correo
            email_cliente(Trim(txt_email.Text))

            'Cierro la base de datos
            memConn.Close()

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error crear_empresa: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_cancelar_multiempresa_Click(sender As Object, e As EventArgs) Handles btn_cancelar_multiempresa.Click

        Try

            'redirigir
            Response.Redirect("registro.aspx")

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_cancelar_multiempresa_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_multiempresa_Click(sender As Object, e As EventArgs) Handles btn_multiempresa.Click

        Try

            'identificar al usuario
            'Ataco a la conexion del programa
            Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion"
            Dim memConn As New SqlConnection(ruta_base)
            memConn.Open()
            Dim memComando As New SqlCommand
            Dim memDatos As SqlDataReader

            'Restriccion para los Usuarios ya registrados
            memComando.CommandText = "SELECT TOP(1) * FROM usuarios WHERE email='" & Trim(txt_email.Text) & "' AND password='" & txt_password_multiempresa.Text & "';"
            memComando.Connection = memConn

            memDatos = memComando.ExecuteReader

            If memDatos.HasRows Then
                Do While memDatos.Read

                    'Lanzo el proceso
                    crear_empresa(txt_id_usuario.Text)

                Loop
            Else
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error3", "error('Su clave no es correcta');setTimeout(function () { $('#txt_password_multiempresa').focus(); }, 100);", True)
                Exit Sub
            End If

            ' Cierro la base de datos
            memDatos.Close()
            memComando.Dispose()
            memConn.Close()


        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_multiempresa_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click

        Try

            'redirigir
            Response.Redirect("login.aspx")

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_login_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub lk_login_Click(sender As Object, e As EventArgs) Handles lk_login.Click

        Try

            'Excepción
            If txt_contrasena.Text = "" Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "error('El campo Contraseña no puede estar vacío.');" & funciones_globales.modal_register("$('#modal_login').modal('show');") & "setTimeout(function() { $('#txt_contrasena').focus(); }, 100);", True)
                Exit Sub

            End If

            'Asigno
            Dim tabla_identificarte As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT * FROM acceso_panel_control WHERE password=@password;"
                memComando.Parameters.Add(New SqlParameter("password", txt_contrasena.Text))
                memComando.Connection = memConn

                'Creamos un adaptador de datos
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

                'Llenamos de datos
                adapter.Fill(tabla_identificarte)

                'Cerramos
                Adapter.Dispose()
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

            End Using

            'Control para si el usuario o contraseña no existe
            If tabla_identificarte.Rows.Count = 0 Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "error('Contraseña Incorrecta.');" & funciones_globales.modal_register("$('#modal_login').modal('show');"), True)
                Exit Sub

            Else

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", funciones_globales.modal_register("$('#modal_login').modal('hide');"), True)

            End If

            'Liberamos
            tabla_identificarte.Dispose()

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error lk_login_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub email_cliente(ByRef direccion As String)

        Try

            'Aqui mando un correo electrónico a los destinatarios que me diga el cliente.
            Dim correo As New System.Net.Mail.MailMessage
            correo.From = New System.Net.Mail.MailAddress("alta.optimus@gmail.com")
            correo.To.Add(direccion)
            correo.To.Add("alta.optimus@gmail.com")
            correo.Subject = "Bienvenido a Optimus Facturación."

            'Monto el cuerpo del correo
            Dim mensajemail As String

            mensajemail = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><head>" &
            "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'> " &
            "<title>¡Te damos la bienvenida a Optimus Facturación!</title>" &
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
            "<td style='width:50%;text-align:center;'><table style='width:100%;'><tr><td style='text-align:left;'><span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Hola,<span style='color: #007bff;'> " & System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_nombre.Text) & "</span></span></td><td style='text-align:right;'><img src='https://www.optimus-soluciones.com/facturacion/imagenes/logo/logo_empresa.png' style='height:81px;'></td></tr></table></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;'></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;text-align:center;'>" &
            "<span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>¡Ya está!<br>" &
            "Tu Cuenta está lista para usarse:</span> " &
            "</td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style = 'text-align:center; padding:30px;' align='center'>" &
            "<a style =' " &
            "        border-radius: 3px;" &
            "        color: #0d6efd;" &
            "        display: inline-block;" &
            "        font-size: 18px;" &
            "        padding: 12px 16px;" &
            "        text-decoration: none;" &
            "        text-align: center;" &
            "        background-color: #FFFFFF;" &
            "        border: 1px solid #0d6efd;" &
            "        width: 400px;'" &
            "        href='https://www.optimus-soluciones.com/facturacion'>" &
            "Iniciar Sesión" &
            "</a>" &
            "</td>" &
            "<td style='width:25%'></td>" &
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

            Dim nombre As String = "alta.optimus@gmail.com"
            Dim contrasena As String = "jafa oday awlo leve"
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

End Class
