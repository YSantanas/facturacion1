
Imports System.Data

Partial Class herramientas_herramientas
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

            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Asigno los datos del operario
                lbl_usuario.Text = tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido")
                lbl_cargo.Text = tabla_usuario.Rows(0)("nivel")

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Foto de Usuario
                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.png") = True Then

                    'Asigno foto
                    Lt_foto.Text = "<img src='imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.png" & "?" + numero_aleatorio.Next(1, 13).ToString & "' style='width:55px; height:55px;border-radius: 50%;' class='img-thumbnail' />"

                Else

                    'Asigno
                    Lt_foto.Text = "<span class='bi bi-person-bounding-box' style='font-size:25px;'></span>"

                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
