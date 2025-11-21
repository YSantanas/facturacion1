Imports System.Data
Imports System.Drawing

Partial Class configuracion_fondo_presupuesto
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

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

                'Compruebo si existe la carpeta albaran
                If Not System.IO.Directory.Exists("D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\") Then

                    'Creo el directorio
                    System.IO.Directory.CreateDirectory("D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\")

                    'Creo el pie del presupuesto
                    System.IO.File.Copy(Server.MapPath("..") & "\imagenes\albaran\fondo_albaran.png", "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\fondo_albaran.png", True)
                    System.IO.File.Copy(Server.MapPath("..") & "\imagenes\albaran\pie_albaran.png", "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\pie_albaran.png", True)

                End If

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Asigno imagen de fondo
                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/fondo_albaran.png") = True Then
                    Image_factura.ImageUrl = "~/imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/fondo_albaran.png?" + numero_aleatorio.Next(100, 999999999).ToString()
                    Image_factura.DataBind()
                End If

                'Asigno imagen de pie
                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/pie_albaran.png") = True Then
                    Image_pie.ImageUrl = "~/imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/pie_albaran.png?" + numero_aleatorio.Next(100, 999999999).ToString()
                    Image_pie.DataBind()
                End If

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

    Protected Sub btn_subir_logo_Click(sender As Object, e As EventArgs) Handles btn_subir_logo.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno 
            Dim nombre As String = "fondo_albaran.png"
            Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\"

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists(ruta_imagen) Then
                System.IO.Directory.CreateDirectory(ruta_imagen)
            End If

            'Subo la imagen
            FileUpload_logo.SaveAs(ruta_imagen & "temp_" & nombre)

            'Indico la ruta de la imagen
            Dim bm_source As New Bitmap(ruta_imagen & "temp_" & nombre)

            'Convierto a la resolución y tamaño deseados
            Dim imagen As New Bitmap(bm_source, 1317, 1862)
            imagen.Save(ruta_imagen & nombre, System.Drawing.Imaging.ImageFormat.Png)

            'Limpio variable para liberar el recurso
            bm_source.Dispose()
            imagen.Dispose()

            'Borro el fichero temporal
            System.IO.File.Delete(ruta_imagen & "temp_" & nombre)

            'Actualizo la imagen
            Dim numero_aleatorio As New Random
            Dim ruta_calculada As String = "imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/" & nombre & "?" + numero_aleatorio.Next(100, 999999999).ToString()

            'Ver Logo
            Image_factura.ImageUrl = "../" & ruta_calculada
            Image_factura.DataBind()

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Diseño del Albarán", "Ha cambiado la imagen del diseño del albarán.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Imagen actualizada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_subir_logo_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_subir_logo_Click: " & ex.Message.Replace("'", " ") & "');", True)
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
            Dim ruta_imagen_origen As String = Server.MapPath("..") & "\imagenes\albaran\fondo_albaran.png"
            Dim ruta_imagen_destino As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\fondo_albaran.png"

            'Copio
            System.IO.File.Copy(ruta_imagen_origen, ruta_imagen_destino, True)

            'Actualizo la imagen
            Dim numero_aleatorio As New Random
            Dim ruta_calculada As String = "imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/fondo_albaran.png?" & numero_aleatorio.Next(100, 999999999).ToString()

            'Asigno imagen de fondo
            Image_factura.ImageUrl = "../" & ruta_calculada
            Image_factura.DataBind()

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Diseño de Albarán", "Ha asignado la imagen por defecto.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Imagen actualizada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_eliminar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_eliminar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_subir_pie_Click(sender As Object, e As EventArgs) Handles btn_subir_pie.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno 
            Dim nombre As String = "pie_albaran.png"
            Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\"

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists(ruta_imagen) Then
                System.IO.Directory.CreateDirectory(ruta_imagen)
            End If

            'Subo la imagen
            FileUpload_pie.SaveAs(ruta_imagen & "temp_" & nombre)

            'Indico la ruta de la imagen
            Dim bm_source As New Bitmap(ruta_imagen & "temp_" & nombre)

            'Convierto a la resolución y tamaño deseados
            Dim imagen As New Bitmap(bm_source, 1317, 315)
            imagen.Save(ruta_imagen & nombre, System.Drawing.Imaging.ImageFormat.Png)

            'Limpio variable para liberar el recurso
            bm_source.Dispose()
            imagen.Dispose()

            'Borro el fichero temporal
            System.IO.File.Delete(ruta_imagen & "temp_" & nombre)

            'Actualizo la imagen
            Dim numero_aleatorio As New Random
            Dim ruta_calculada As String = "imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/" & nombre & "?" + numero_aleatorio.Next(100, 999999999).ToString()

            'Ver Logo
            Image_pie.ImageUrl = "../" & ruta_calculada
            Image_pie.DataBind()

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Diseño de Albarán", "Ha cambiado la imagen del pie del albarán.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Imagen actualizada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_subir_logo_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_subir_logo_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub chk_tema_original_pie_CheckedChanged(sender As Object, e As EventArgs) Handles chk_tema_original_pie.CheckedChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If chk_tema_original_pie.Checked Then

                'Asigno
                lt_mensaje_eliminar_pie.Text = "¿Esta seguro que desea aplicar la imagen por defecto?"
                chk_tema_original_pie.Checked = False

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar_pie').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "chk_tema_original_pie_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error chk_tema_original_pie_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_si_eliminar_pie_Click(sender As Object, e As EventArgs) Handles btn_si_eliminar_pie.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno 
            Dim ruta_imagen_origen As String = Server.MapPath("..") & "\imagenes\albaran\pie_albaran.png"
            Dim ruta_imagen_destino As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\albaran\pie_albaran.png"

            'Copio
            System.IO.File.Copy(ruta_imagen_origen, ruta_imagen_destino, True)

            'Actualizo la imagen
            Dim numero_aleatorio As New Random
            Dim ruta_calculada As String = "imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/albaran/pie_albaran.png?" & numero_aleatorio.Next(100, 999999999).ToString()

            'Asigno imagen de fondo
            Image_pie.ImageUrl = "../" & ruta_calculada
            Image_pie.DataBind()

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Diseño del albarán", "Ha asignado la imagen del pie por defecto.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Imagen actualizada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_eliminar_pie_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_eliminar_pie_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
