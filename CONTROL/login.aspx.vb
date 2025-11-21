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
                cuerpo.Attributes.Add("style", "background:url('../imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg') no-repeat center center fixed;")

                'Carga Fecha y Día
                lbl_fecha.Text = " - " & DateTime.Now.ToString("dd/MM/yyyy")

            End If

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub lk_login_Click(sender As Object, e As EventArgs) Handles lk_login.Click

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT * FROM acceso_panel_control WHERE password=@password AND email=@email;"
                memComando.Parameters.Add(New SqlParameter("email", txt_nombre.Text))
                memComando.Parameters.Add(New SqlParameter("password", txt_contrasena.Text))
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

            'Control para si el usuario o contraseña no existe
            If tabla_consulta.Rows.Count = 0 Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "error('Email o Contraseña Incorrectos.');", True)
                Exit Sub

            Else

                'Carga de Control de seguridad
                Session("id_master") = tabla_consulta.Rows(0)("id").ToString
                Session("nombre_master") = tabla_consulta.Rows(0)("nombre").ToString
                Session("primer_apellido_master") = tabla_consulta.Rows(0)("primer_apellido").ToString
                Session("segundo_apellido_master") = tabla_consulta.Rows(0)("segundo_apellido").ToString
                Session("email") = tabla_consulta.Rows(0)("email").ToString
                Session("nivel_master") = tabla_consulta.Rows(0)("nivel").ToString

                'Destruyo
                tabla_consulta.Dispose()

                'Redirijo
                Response.Redirect("Default.aspx")

            End If

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error lk_login_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
