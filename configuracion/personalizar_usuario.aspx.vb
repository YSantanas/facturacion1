Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Partial Class configuracion_personalizar_usuario
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_datos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT nombre,primer_apellido,segundo_apellido,email,password FROM usuarios WHERE Id='" & tabla_usuario.Rows(0)("Id") & "';"
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

            'Asigno
            txt_nombre_usuario.Text = tabla_consulta(0)(0).ToString
            txt_primer_apellido_usuario.Text = tabla_consulta(0)(1).ToString
            txt_segundo_apellido_usuario.Text = tabla_consulta(0)(2).ToString
            txt_codigo_usuario.Text = tabla_consulta(0)(3).ToString
            txt_paswword_usuario.Text = tabla_consulta(0)(4).ToString
            txt_codigo_usuario_antiguo.Text = tabla_consulta(0)(3).ToString
            txt_paswword_usuario_antiguo.Text = tabla_consulta(0)(4).ToString

            'Actualizo la imagen
            Dim numero_aleatorio As New Random

            'Foto de Usuario
            If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.jpg") = True Then

                'Asigno foto
                Lt_foto.Text = "<img src='../imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.jpg" & "?" + numero_aleatorio.Next(1, 13).ToString & "' style='width:200px; height:200px;border-radius: 50%;' class='img-thumbnail' />"

            Else

                'Asigno
                Lt_foto.Text = "<span class='bi bi-person-bounding-box' style='font-size:25px;'></span>"

                'Asigno
                chk_tema_original.Checked = True
                chk_tema_original.Enabled = False

            End If

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_datos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_datos: " & ex.Message.Replace("'", " ") & "');", True)
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

                'Leer Datos
                leer_datos()

                'Asigno foco
                txt_nombre_usuario.Focus()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_Click(sender As Object, e As EventArgs) Handles btn_grabar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Grabar datos
            grabar_datos()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_grabar_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Grabar datos
            grabar_datos()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub grabar_datos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Actualizo la imagen
        Dim numero_aleatorio As New Random

        Try

            'Si el correo no es válido o intento inyección SQL
            If funciones_globales.IsValidEmail(txt_codigo_usuario.Text) = False Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('El E-mail introducido no es válido.');", True)
                Exit Sub
            End If

            'Si cambia el correo electronico hay que mirar si existe el mismo correo en otro usuario
            If txt_codigo_usuario.Text <> txt_codigo_usuario_antiguo.Text Then
                'Comprobar si existe email
                Dim tabla As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].usuarios WHERE email='" & txt_codigo_usuario.Text & "';")
                If tabla.Rows.Count > 0 Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Ya existe un Usuario Registrado en el sistema con el mismo E-mail.');", True)
                    Exit Sub
                End If
            End If

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE usuarios SET " &
                "nombre=@txt_nombre_usuario, " &
                "primer_apellido=@txt_primer_apellido, " &
                "segundo_apellido=@txt_segundo_apellido, " &
                "email=@txt_codigo_usuario, " &
                "password=@txt_paswword_usuario " &
                "WHERE id=" & tabla_usuario.Rows(0)("Id") & ";"
                memComando.Parameters.Add(New SqlParameter("txt_nombre_usuario", Data.SqlDbType.VarChar, 25))
                memComando.Parameters("txt_nombre_usuario").Value = txt_nombre_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_primer_apellido", Data.SqlDbType.VarChar, 50))
                memComando.Parameters("txt_primer_apellido").Value = txt_primer_apellido_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_segundo_apellido", Data.SqlDbType.VarChar, 50))
                memComando.Parameters("txt_segundo_apellido").Value = txt_segundo_apellido_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_codigo_usuario", Data.SqlDbType.VarChar, 80))
                memComando.Parameters("txt_codigo_usuario").Value = txt_codigo_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_paswword_usuario", Data.SqlDbType.VarChar, 20))
                memComando.Parameters("txt_paswword_usuario").Value = txt_paswword_usuario.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Obtengo datos de usuario
            HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].usuarios WHERE usuarios.id=" & Request.QueryString("id_usuario") & ";")

            'Asigno
            txt_codigo_usuario_antiguo.Text = txt_codigo_usuario.Text
            txt_paswword_usuario_antiguo.Text = txt_paswword_usuario.Text

            'Si sube logo
            If FileUpload_logo.HasFile = True Then

                'Asigno 
                Dim nombre As String = "foto.jpg"
                Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\foto\" & tabla_usuario.Rows(0)("Id") & "\"

                'Creo la carpeta para el fondo de éste usuario
                If Not System.IO.Directory.Exists(ruta_imagen) Then
                    System.IO.Directory.CreateDirectory(ruta_imagen)
                End If

                'Subo la imagen
                FileUpload_logo.SaveAs(ruta_imagen & "temp_" & nombre)

                'Indico la ruta de la imagen
                Dim bm_source As New Bitmap(ruta_imagen & "temp_" & nombre)

                'Calculo el factor Alto para esa imagen dada
                Dim alto_final As Integer = CInt(((CInt(bm_source.Height)) * 100) / ((CInt(bm_source.Width) * 100) / 500))

                'Convierto a la resolución y tamaño deseados
                Dim imagen As New Bitmap(bm_source, 500, alto_final)
                imagen.Save(ruta_imagen & nombre, System.Drawing.Imaging.ImageFormat.Jpeg)

                'Limpio variable para liberar el recurso
                bm_source.Dispose()
                imagen.Dispose()

                'Borro el fichero temporal
                System.IO.File.Delete(ruta_imagen & "temp_" & nombre)

                'Ver Logo
                Dim nueva_ruta As String = "imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.jpg" & "?" + numero_aleatorio.Next(1, 13).ToString
                Lt_foto.Text = "<img src='../" & nueva_ruta & "' style='width:200px; height:200px;border-radius: 50%;' class='img-thumbnail' />"
                Lt_foto.DataBind()

                'Asigno
                chk_tema_original.Checked = False
                chk_tema_original.Enabled = True

            End If

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Personalizar Usuario", "Actualizó los datos de Usuario.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "$(window.parent.document).find('#lblnombre').text('" & txt_nombre_usuario.Text & " " & txt_primer_apellido_usuario.Text & " " & txt_segundo_apellido_usuario.Text & "');ok('Actualizó los datos de Usuario.');actualizar_foto('<img id=""img_logo"" src=""imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.jpg?" + numero_aleatorio.Next(1, 13).ToString & """ style=""width:55px; height:55px;border-radius: 50%;"" class=""img-thumbnail"">');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "grabar_datos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error grabar_datos: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub chk_tema_original_CheckedChanged(sender As Object, e As EventArgs) Handles chk_tema_original.CheckedChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If chk_tema_original.Checked Then

                'Asigno
                lt_mensaje_eliminar.Text = "¿Esta seguro que desea aplicar la imagen por defecto?"
                chk_tema_original.Checked = False

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "chk_tema_original_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error chk_tema_original_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_si_eliminar_Click(sender As Object, e As EventArgs) Handles btn_si_eliminar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno 
            Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\foto\" & tabla_usuario.Rows(0)("Id")

            'Elimino
            System.IO.Directory.Delete(ruta_imagen, True)

            'Asigno
            Lt_foto.Text = "<span class='bi bi-person-bounding-box' style='font-size:25px;'></span>"
            Lt_foto.DataBind()

            'Asigno
            chk_tema_original.Checked = True
            chk_tema_original.Enabled = False

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Personalizar Usuario", "Ha asignado la imagen por defecto.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Imagen actualizada correctamente.');actualizar_foto('sin_foto');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_eliminar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_eliminar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
