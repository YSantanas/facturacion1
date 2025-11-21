Imports System.Data
Imports System.Drawing

Partial Class configuracion_personalizar_escritorio
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

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Asigno imagen de fondo
                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/fondo/" & tabla_usuario.Rows(0)("Id") & "/fondo_aplicacion.jpg") = True Then
                    Image_logo.ImageUrl = "~/imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/fondo/" & tabla_usuario.Rows(0)("Id") & "/fondo_aplicacion.jpg?" + numero_aleatorio.Next(100, 999999999).ToString()
                    Image_logo.DataBind()
                    chk_tema_original.Checked = False
                    chk_tema_original.Enabled = True
                Else
                    Image_logo.ImageUrl = "~/Imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg"
                    Image_logo.DataBind()
                    chk_tema_original.Checked = True
                    chk_tema_original.Enabled = False
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
            Dim nombre As String = "fondo_aplicacion.jpg"
            Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\fondo\" & tabla_usuario.Rows(0)("Id") & "\"

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists(ruta_imagen) Then
                System.IO.Directory.CreateDirectory(ruta_imagen)
            End If

            'Subo la imagen
            FileUpload_logo.SaveAs(ruta_imagen & "temp_" & nombre)

            'Indico la ruta de la imagen
            Dim bm_source As New Bitmap(ruta_imagen & "temp_" & nombre)

            'Calculo el factor Alto para esa imagen dada
            Dim alto_final As Integer = CInt(((CInt(bm_source.Height)) * 100) / ((CInt(bm_source.Width) * 100) / 2560))

            'Convierto a la resolución y tamaño deseados
            Dim imagen As New Bitmap(bm_source, 2560, alto_final)
            imagen.Save(ruta_imagen & nombre, System.Drawing.Imaging.ImageFormat.Jpeg)

            'Limpio variable para liberar el recurso
            bm_source.Dispose()
            imagen.Dispose()

            'Borro el fichero temporal
            System.IO.File.Delete(ruta_imagen & "temp_" & nombre)

            'Actualizo la imagen
            Dim numero_aleatorio As New Random
            Dim ruta_calculada As String = "imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/fondo/" & tabla_usuario.Rows(0)("Id") & "/" & nombre & "?" + numero_aleatorio.Next(100, 999999999).ToString()

            'Ver Logo
            Image_logo.ImageUrl = "../" & ruta_calculada
            Image_logo.DataBind()

            'Asigno
            chk_tema_original.Checked = False
            chk_tema_original.Enabled = True

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Personalizar Escritorio", "Ha cambiado la imagen de escritorio de la empresa.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "refresco('" & ruta_calculada & "');ok('Imagen actualizada correctamente.');", True)

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
            Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\fondo\" & tabla_usuario.Rows(0)("Id")

            'Elimino
            System.IO.Directory.Delete(ruta_imagen, True)

            'Actualizo la imagen
            Dim numero_aleatorio As New Random
            Dim ruta_calculada As String = "Imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg"

            'Asigno imagen de fondo
            Image_logo.ImageUrl = "../" & ruta_calculada
            Image_logo.DataBind()

            'Asigno
            chk_tema_original.Checked = True
            chk_tema_original.Enabled = False

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Personalizar Escritorio", "Ha asignado la imagen por defecto.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "refresco('" & ruta_calculada & "');ok('Imagen actualizada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_eliminar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_eliminar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
