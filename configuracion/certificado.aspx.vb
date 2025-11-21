Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Security.Cryptography.X509Certificates

Partial Class configuracion_certificado
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_certificado()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Declaro
            Dim ruta_certificado As String = "D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/certificado/certificado.pfx"

            'Ver si existe el certificado
            If My.Computer.FileSystem.FileExists(ruta_certificado) = True Then

                'Declaro
                Dim certificate As New X509Certificate2(ruta_certificado, tabla_empresa.Rows(0)("certificado_password").ToString)

                'Asigno
                LT_certificado.Text = "<div class='d-flex justify-content-center align-items-center'>" &
                    "<table style='width:70%;border: 1px solid #b8daff;border-radius: 5px; border-spacing: 5px;border-collapse: separate;'>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Emitido por:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.Issuer &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Emitido para:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.Subject &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Número de serie:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.SerialNumber &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Fecha validez:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.NotBefore & " al " & certificate.NotAfter &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "</table>" &
                    "</div>"

            Else

                'Asigno
                LT_certificado.Text = "<div class='text-center text-primary fw-bold'>Aún no tiene un certificado digital activo.</div>"

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_certificado", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_certificado: " & ex.Message.Replace("'", " ") & "');", True)
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

                'Leer Certificado
                leer_certificado()

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Ver Simulacion
                img_sello.ImageUrl = "~/Imagenes/web/simulacion_certificado.png?" + numero_aleatorio.Next(100, 999999999).ToString()
                img_sello.Width = (50 * 3.76) / 2
                img_sello.Height = (20.8 * 3.76) / 2
                img_sello.DataBind()

                'Position
                txt_x.Text = tabla_empresa.Rows(0)("certificado_posicion_x")
                img_sello.Style.Add("left", tabla_empresa.Rows(0)("certificado_posicion_x") & "px")
                txt_y.Text = tabla_empresa.Rows(0)("certificado_posicion_y")
                img_sello.Style.Add("top", tabla_empresa.Rows(0)("certificado_posicion_y") & "px")

                'Asigno
                txt_motivo.Text = tabla_empresa.Rows(0)("certificado_motivo").ToString
                txt_localizacion.Text = tabla_empresa.Rows(0)("certificado_localizacion").ToString

                ''Restricción de Usuarios
                'If tabla_usuario.Rows(0)("nivel") = "Invitado" Or tabla_usuario.Rows(0)("nivel") = "Usuario Restringido" Or tabla_usuario.Rows(0)("nivel") = "Usuario" Then
                '    'Bloque Jquery
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                '    FileUpload_logo.Enabled = False
                '    chk_tema_original.Enabled = False
                '    Exit Sub
                'End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_subir_certificado_Click(sender As Object, e As EventArgs) Handles btn_subir_certificado.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno el nuevo nombre y subo la imagen
            Dim nombre As String = "temp_" & tabla_usuario.Rows(0)("Id") & "_certificado.pfx"
            Dim ruta_imagen As String = Server.MapPath("..") & "\temp\"

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists(ruta_imagen) Then
                System.IO.Directory.CreateDirectory(ruta_imagen)
            End If

            'Subo la imagen
            FileUpload_certificado.SaveAs(ruta_imagen & nombre)

            Try

                'Leo el certificado
                Dim certificate As New X509Certificate2(ruta_imagen & nombre, txt_certificado_digital.Text)

                'Asigno
                lt_mensaje_agregar.Text = "<div class='d-flex justify-content-center align-items-center'>" &
                    "<table style='width:70%;border: 1px solid #b8daff;border-radius: 5px; border-spacing: 5px;border-collapse: separate;'>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Emitido por:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.Issuer &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Emitido para:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.Subject &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Número de serie:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.SerialNumber &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "<tr>" &
                    "<td style='width:170px;'><span class='fw-light'>Fecha validez:</span></td>" &
                    "<td>" &
                        "<span class='fw-normal text-primary' style='font-size :12px;'>" &
                        certificate.NotBefore & " al " & certificate.NotAfter &
                        "</span>" &
                    "</td>" &
                    "</tr>" &
                    "</table>" &
                    "</div>"

            Catch ex As Exception

                Response.Write(ex.Message)
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('" & ex.Message & ".');", True)
                Exit Sub

            End Try

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_subir_certificado_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_subir_certificado_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_si_agregar_Click(sender As Object, e As EventArgs) Handles btn_si_agregar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno el nuevo nombre y subo la imagen
            Dim nombre As String = "temp_" & tabla_usuario.Rows(0)("Id") & "_certificado.pfx"
            Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\certificado\"

            'Copio para cambiar el nombre
            System.IO.File.Copy(Server.MapPath("..") & "\temp\" & nombre, ruta_imagen & "\certificado.pfx", True)

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE empresa SET " &
                "certificado_password=@certificado_password " &
                "WHERE id=" & tabla_empresa.Rows(0)("Id") & ";"
                memComando.Parameters.Add("@certificado_password", Data.SqlDbType.VarChar, 25).Value = txt_certificado_digital.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Cierro la BBDD
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Limpio
            txt_certificado_digital.Text = Nothing

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Certificado Digital", "Ha cambiado el Certificado Digital.")

            'Obtengo datos de tipo de impuestos
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'leer_certificado
            leer_certificado()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Certificado actualizado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_agregar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_agregar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_actualizar_Click(sender As Object, e As EventArgs) Handles btn_actualizar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE empresa SET " &
                "certificado_motivo=@certificado_motivo, " &
                "certificado_localizacion=@certificado_localizacion " &
                "WHERE id=" & tabla_empresa.Rows(0)("Id") & ";"
                memComando.Parameters.Add("@certificado_motivo", Data.SqlDbType.VarChar, 25).Value = txt_motivo.Text
                memComando.Parameters.Add("@certificado_localizacion", Data.SqlDbType.VarChar, 25).Value = txt_localizacion.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Cierro la BBDD
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Certificado Digital", "Ha cambiado la posición del Certificado Digital.")

            'Obtengo datos de tipo de impuestos
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Opciones del Certificado actualizado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_actualizar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_actualizar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
