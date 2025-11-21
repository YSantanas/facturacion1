
Module Module_default

    Public funcion_default As New funciones_globales

End Module

Partial Class _Default
    Inherits System.Web.UI.Page

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

        Try

            'Control de Seguridad
            If Session("nombre_master") = "" Then
                Response.Redirect("login.aspx")
                Exit Sub
            End If

            'Agregar la ruta para el menú seleccionado
            If Not String.IsNullOrEmpty(Request.QueryString("ruta")) Then

                'Paso la ruta al iframe
                ventanas.Attributes("src") = "herramientas\" & Request.QueryString("ruta") & ".aspx"

            End If

            If Not IsPostBack Then

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Asigno imagen de fondo
                body_contenedor.Attributes.Add("style", "background: url('../imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg') no-repeat center center fixed;")

                'Paso la traza para grabar el Log
                funcion_default.grabar_registro("kernel_facturacion", Session("id_master"), Session("nombre_master") & " " & Session("primer_apellido_master") & " " & Session("segundo_apellido_master"), "Login", "El Usuario Accedió a la aplicacion desde la IP: " & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString & ".")

            End If

        Catch ex As Exception
            'Registro Error
            funcion_default.grabar_registro_error("0", Session("id_master"), Request.Url.Segments.Last(), "form1_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error LOAD: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
