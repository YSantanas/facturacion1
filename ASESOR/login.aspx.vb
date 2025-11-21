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
            Dim tabla_multiempresa As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT usuarios.id as id_usuario,usuarios.nombre,usuarios.primer_apellido,usuarios.segundo_apellido,usuarios.email,usuarios.password,usuarios.baja,empresa.id,empresa.codigo_empresa,empresa.nombre_fiscal,empresa.nif,empresa.ruta_base_datos,empresa.Id_tipo_plan_cuentas,empresa.demo,empresa.bienvenida,empresa.fecha_creacion,empresa.servicio_suspendido,usuarios.plan_facturacion,empresa.nombre_comercial,usuarios.nivel,empresa.custodia " &
                "FROM usuarios_empresas INNER JOIN " &
                "usuarios ON usuarios_empresas.id_usuario = usuarios.Id INNER JOIN " &
                "empresa ON usuarios_empresas.id_empresa = empresa.id " &
                "WHERE usuarios.email = @email AND usuarios.password = @password AND nivel='Auditor' " &
                "ORDER BY id"
                memComando.Parameters.Add(New SqlParameter("email", txt_email.Text))
                memComando.Parameters.Add(New SqlParameter("password", txt_contrasena.Text))
                memComando.Connection = memConn

                'Creamos un adaptador de datos
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

                'Llenamos de datos
                adapter.Fill(tabla_multiempresa)

                'Cerramos
                adapter.Dispose()
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Control para si el usuario o contraseña no existe
            If tabla_multiempresa.Rows.Count = 0 Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "error('Email o Contraseña Incorrectos.');", True)
                Exit Sub

            Else

                'Carga de Control de seguridad
                Session("id_control") = Session.SessionID() 'Control de Seguridad

                'Si el usuario esta de baja
                If tabla_multiempresa.Rows(0).Item("baja") = True Then

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "error('Usuario de Baja.');", True)
                    Exit Sub

                End If

                'Asignacion
                PH_datos.Visible = True
                lk_login.Visible = False

                'Creo la llave de Seguridad
                Dim querystringSeguro As TSHAK.Components.SecureQueryString
                querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8})
                querystringSeguro("id_usuario") = tabla_multiempresa.Rows(0).Item("id_usuario").ToString
                querystringSeguro("id_empresa") = tabla_multiempresa.Rows(0).Item("id").ToString

                'Asignar Movil, Tablet o Desktop
                Dim vvalor() As String = txt_tipo_dispositivo.Text.Split("|")

                'Envio
                Response.Redirect("Default.aspx?key=" & HttpUtility.UrlEncode(querystringSeguro.ToString()))

            End If

            'Liberamos
            tabla_multiempresa.Dispose()

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error lk_login_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
