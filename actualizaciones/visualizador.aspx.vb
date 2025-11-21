Imports System.IO

Partial Class actualizaciones_visualizador
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            If Request.Params("id") <> "" Then

                'Mostrar icono de descarga
                If Path.GetExtension(Request.Params("id").ToString.ToLower) = ".pdf" Then

                    'Imagen
                    img_logo.Attributes.Add("src", "../Imagenes/web/imagen_pdf.png")

                    'Copio de Ruta disco D a disco C
                    System.IO.File.Copy("D:\imagenes_usuarios\Temp\" & Request.Params("id").ToString, Server.MapPath("..") & "\temp\" & Request.Params("id").ToString, True)

                    'Asigno el enlace
                    HL_enlace.NavigateUrl = "..\temp\" & Request.Params("id").ToString

                    'Ruta
                    ver_factura.Attributes("src") = "..\temp\" & Request.Params("id").ToString

                    'Panel
                    Panel_Mostrar_PDF.Visible = true

                End If

                If Path.GetExtension(Request.Params("id").ToString.ToLower) = ".xlsx" Then

                    'Imagen
                    img_logo.Attributes.Add("src", "../imagenes/web/xls_icono.png")

                    'Copio de Ruta disco D a disco C
                    System.IO.File.Copy("D:\imagenes_usuarios\Temp\" & Request.Params("id").ToString, Server.MapPath("..") & "\temp\" & Request.Params("id").ToString, True)

                    'Asigno el enlace
                    HL_enlace.NavigateUrl = "..\temp\" & Request.Params("id").ToString

                    'Ruta
                    ver_factura.Attributes("src") = "https://docs.google.com/gview?url=https://www.iocloudcomputing.io/contabilidad/temp/" & Request.Params("id").ToString & "&embedded=true"
                    'Panel
                    Panel_Mostrar_PDF.Visible = True

                End If

                If Path.GetExtension(Request.Params("id").ToString.ToLower) = ".zip" Then

                    'Imagen
                    img_logo.Attributes.Add("src", "../Imagenes/web/File-Format-ZIP_amarillo.png")

                    'Copio de Ruta disco D a disco C
                    System.IO.File.Copy("D:\imagenes_usuarios\Temp\" & Request.Params("id").ToString, Server.MapPath("..") & "\temp\" & Request.Params("id").ToString, True)

                    'Asigno el enlace
                    HL_enlace.NavigateUrl = "..\temp\" & Request.Params("id").ToString

                    'Ruta
                    ver_factura.Attributes("src") = "..\actualizaciones\visual_zip.aspx?id=" & Request.Params("id").ToString

                    'Panel
                    Panel_Mostrar_PDF.Visible = True

                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
