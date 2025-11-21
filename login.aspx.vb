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
                cuerpo.Attributes.Add("style", "background:url('imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg') no-repeat center center fixed;")

                'Carga Fecha y Día
                lbl_fecha.Text = " - " & DateTime.Now.ToString("dd/MM/yyyy")

                'Si seleccionó recuerdame
                If Not Request.Cookies("Security.Facturacion") Is Nothing Then

                    Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
                    querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, Request.Cookies("Security.Facturacion").Value)

                    'Obtenemos las variables que vienen dentro del querystringSeguro
                    Dim nombre As String = querystringSeguro("nombre").ToString
                    Dim apellido_1 As String = querystringSeguro("apellido_1").ToString
                    Dim apellido_2 As String = querystringSeguro("apellido_2").ToString
                    Dim email As String = querystringSeguro("email").ToString
                    Dim password As String = querystringSeguro("password").ToString

                    'Asigno los valores
                    Chk_recuerdame.Checked = True
                    txt_email.Text = email
                    txt_contrasena.Text = password
                    lbl_bienvenida.Text = "Hola, " & nombre & " " & apellido_1 & " " & apellido_2

                End If

                'Creo si no existe porque es la primera vez
                If HttpContext.Current.Request.Cookies("Ventanas.Facturacion") Is Nothing Then
                    'Creo la Cookie de 20 minutos de control
                    Dim Cookie As HttpCookie = New HttpCookie("Ventanas.Facturacion")
                    Cookie.Value = "Vacio"
                    Cookie.Expires = DateTime.Now.AddDays(364)
                    HttpContext.Current.Response.Cookies.Add(Cookie)
                End If

                'Leer modo_mantenimiento
                leer_modo_mantenimiento()

            End If

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_modo_mantenimiento()

        'Asigno
        Dim tabla_consulta As New DataTable

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "SELECT * FROM [mantenimiento];"
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

        'Activar o Desactivar
        If tabla_consulta.Rows(0)("activo") = 0 Then
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Ocultar_modal", funciones_globales.modal_register("$('#modal_mantenimiento').modal('hide');"), True)
        Else
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "visible_modal", funciones_globales.modal_register("$('#modal_mantenimiento').modal('show');$('#lbl_tiempo').text('" & tabla_consulta.Rows(0)("activo").ToString & "');hablar('Estamos realizando tareas de mantenimiento para mejorar aún más nuestro servicio. El sistema estará disponible en " + tabla_consulta.Rows(0)("activo").ToString + " minutos. Por favor inténtelo de nuevo más tarde.Disculpen las molestias.');"), True)
        End If

        'Liberamos
        tabla_consulta.Dispose()

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
                "WHERE (usuarios.email = @email) AND (usuarios.password = @password) " &
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

                'Si el check esta ticado es que el usuario quiere que recuerde
                If Chk_recuerdame.Checked = True Then

                    Dim aCookiesecurity As New HttpCookie("Security.Facturacion")
                    'Cargo un componente de seguridad
                    Dim querystringSeguro As TSHAK.Components.SecureQueryString
                    querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8})
                    querystringSeguro("nombre") = tabla_multiempresa.Rows(0)("nombre").ToString
                    querystringSeguro("apellido_1") = tabla_multiempresa.Rows(0)("primer_apellido").ToString
                    querystringSeguro("apellido_2") = tabla_multiempresa.Rows(0)("segundo_apellido").ToString
                    querystringSeguro("email") = txt_email.Text
                    querystringSeguro("password") = txt_contrasena.Text
                    aCookiesecurity.Value = querystringSeguro.ToString()
                    aCookiesecurity.Expires = DateTime.Now.AddDays(364)
                    Response.Cookies.Add(aCookiesecurity)

                Else

                    'Destruyo las cookies
                    Dim aCookierecuerdame As New HttpCookie("Security.Facturacion")
                    aCookierecuerdame.Expires = DateTime.Now.AddDays(-1)
                    Response.Cookies.Add(aCookierecuerdame)

                End If

                'Carga de Control de seguridad
                Session("id_control") = Session.SessionID() 'Control de Seguridad

                'Si sólo tiene una empresa o varias
                If tabla_multiempresa.Rows.Count = 1 Then

                    'Si el usuario esta de baja
                    If tabla_multiempresa.Rows(0).Item("baja") = True Then

                        'Registro como bloque en local para el jquery
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "error('Usuario de Baja.');", True)
                        Exit Sub

                    End If

                    'Si es una Empresa Demo
                    If tabla_multiempresa.Rows(0).Item("demo").ToString = True Then

                        'Cargo las fechas para poder manipularlas
                        Dim fecha_creacion As Date = tabla_multiempresa.Rows(0).Item("fecha_creacion").ToString
                        Dim fecha_calculada As Date = fecha_creacion.AddDays(30)

                        If Today >= fecha_calculada Then

                            'Registro como bloque en local para el jquery
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion4", "error('Ha caducado el tiempo de Demostración de la empresa " & tabla_multiempresa.Rows(0).Item("nombre_fiscal").ToString & ".');", True)
                            Exit Sub

                        Else

                            'Span para obtener la diferencia de la fecha
                            Dim ts As TimeSpan = fecha_calculada - Today

                            'Insertamos una notificaion
                            funciones_globales.grabar_notificacion(tabla_multiempresa.Rows(0).Item(8), "Periodo en Demo", "Recuerda que te quedan " & ts.Days.ToString & " dias para finalizar el periodo de prueba", "Normal", tabla_multiempresa.Rows(0).Item("Id"))

                        End If

                    End If

                    'Si la empresa esta por impagado
                    If tabla_multiempresa.Rows(0).Item("servicio_suspendido").ToString = True Then
                        'Registro como bloque en local para el jquery
                        Dim texto As String = "Estimado cliente, se ha detectado una incidencia administrativa en el servicio que le prestamos. Rogamos nos indique la mejor forma de contactar con su administración, enviándonos un e-mail a: administracion@iocc.io ó un whatsapp al 613058003."
                        Dim texto_hablado As String = "Estimado cliente, se ha detectado una incidencia administrativa en el servicio que le prestamos. Rogamos nos indique la mejor forma de contactar con su administración, enviándonos un e-mail a: administracion@iocc.io ó un watsaap al seis uno tres cero cinco ocho cero cero tres"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_impagado", "alertify.warning('" & texto & "',20);hablar('" & texto_hablado & "');", True)
                        Exit Sub
                    End If

                    'Si es un usuario nuevo, debe hacer el test de bienvenida
                    If tabla_multiempresa.Rows(0).Item("bienvenida") = True Then
                        Response.Redirect("bienvenida.aspx?cod_empresa=" & tabla_multiempresa.Rows(0).Item("id").ToString & "&bbdd=" & tabla_multiempresa.Rows(0).Item(8))
                        Exit Sub
                    End If

                    'Asignacion
                    PH_datos.Visible = True
                    PH_cargando.Visible = True
                    lk_login.Visible = False

                    'Creo la llave de Seguridad
                    Dim querystringSeguro As TSHAK.Components.SecureQueryString
                    querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8})
                    querystringSeguro("id_usuario") = tabla_multiempresa.Rows(0).Item("id_usuario").ToString
                    querystringSeguro("id_empresa") = tabla_multiempresa.Rows(0).Item("id").ToString

                    'Asignar Movil, Tablet o Desktop
                    Dim vvalor() As String = txt_tipo_dispositivo.Text.Split("|")
                    If vvalor(0) = False And vvalor(1) = False Then
                        Response.Redirect("Default.aspx?key=" & HttpUtility.UrlEncode(querystringSeguro.ToString()))
                    Else
                        Response.Redirect("Default_movil.aspx?key=" & HttpUtility.UrlEncode(querystringSeguro.ToString()))
                    End If

                Else

                    'Cargo Valores
                    gridview_empresa.DataSource = tabla_multiempresa

                    'Refresco Valores
                    gridview_empresa.DataBind()

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "multi_empresa", funciones_globales.modal_register("$('#modal_multi_empresa').modal('show');$('#modal_multi_empresa').on('shown.bs.modal', function () { $('#filter').focus(); });"), True)

                End If

            End If

            'Liberamos
            tabla_multiempresa.Dispose()

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error lk_login_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_empresa_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_empresa.RowDataBound

        Try

            'Recorro el GV
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim onmouseoverStyle As String = "this.style.backgroundColor='#e2e2e2';this.style.cursor='pointer'"
                Dim onmouseoutStyle As String = "this.style.backgroundColor='white';this.style.cursor='Default'"

                'Asigno la propiedas                
                e.Row.Attributes.Add("onmouseover", onmouseoverStyle)
                e.Row.Attributes.Add("onmouseout", onmouseoutStyle)

                'Asigno una variable a manipular
                Dim imagen As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("imgestado"), System.Web.UI.WebControls.Image)

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Compruebo la existencia de la imagen
                If System.IO.File.Exists("D:/imagenes_usuarios_facturacion/" & gridview_empresa.DataKeys(e.Row.RowIndex).Item("ruta_base_datos").ToString() & "/logo/logo_empresa.jpg") Then
                    imagen.ImageUrl = "~/imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & gridview_empresa.DataKeys(e.Row.RowIndex).Item("ruta_base_datos").ToString() & "/logo/logo_empresa.jpg" + "?" + numero_aleatorio.Next(100, 999999999).ToString()
                End If
                imagen.Height = "30"

                Dim imagen2 As System.Web.UI.WebControls.LinkButton = DirectCast(e.Row.FindControl("lk_login"), System.Web.UI.WebControls.LinkButton)
                imagen2.ToolTip = "Entrar en la empresa: " & gridview_empresa.DataKeys(e.Row.RowIndex).Item("nombre_comercial").ToString()

                'Si es Demo
                If gridview_empresa.DataKeys(e.Row.RowIndex).Item("demo").ToString() = True Then

                    Dim fecha_creacion As Date = gridview_empresa.DataKeys(e.Row.RowIndex).Item("fecha_creacion").ToString()
                    Dim fecha_calculada As Date = fecha_creacion.AddDays(30)
                    For x = 4 To e.Row.Cells.Count - 1
                        e.Row.Cells(x).BackColor = System.Drawing.Color.FromArgb(255, 199, 200)
                    Next

                    'Informo de cuantos dias quedan
                    Dim ts As TimeSpan = fecha_calculada - Today
                    If ts.Days.ToString >= 0 Then
                        imagen2.ToolTip = "Se encuentra en modo DEMO y expirará en " & ts.Days.ToString & " dia(s)."
                        For x = 4 To e.Row.Cells.Count - 1
                            e.Row.Cells(x).BackColor = System.Drawing.Color.FromArgb(207, 226, 255)
                        Next
                    Else
                        imagen2.ToolTip = "Se encuentra en modo DEMO y expiró su acceso."
                        For x = 0 To e.Row.Cells.Count - 1
                            e.Row.Cells(x).BackColor = System.Drawing.Color.FromArgb(255, 199, 200)
                            e.Row.Enabled = False
                        Next
                    End If

                    'Informo con Claudia
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Modo_demo", "advertencia('Existen Empresas Demo asociadas a tu Usuario.', 20);", True)

                End If

                'Si es Baja
                If gridview_empresa.DataKeys(e.Row.RowIndex).Item("baja").ToString() = True Then

                    For x = 0 To e.Row.Cells.Count - 1
                        e.Row.Cells(x).BackColor = System.Drawing.Color.FromArgb(255, 199, 200)
                        e.Row.ToolTip = "Usuario de Baja"
                        e.Row.Enabled = False
                    Next

                End If

            End If

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_empresa_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_empresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridview_empresa.SelectedIndexChanged

        Try

            'Si la empresa esta por impagado
            If gridview_empresa.DataKeys(gridview_empresa.SelectedIndex).Item("servicio_suspendido").ToString() = True Then
                'Registro como bloque en local para el jquery
                Dim texto As String = "Estimado cliente, se ha detectado una incidencia administrativa en el servicio que le prestamos. Rogamos nos indique la mejor forma de contactar con su administración, enviándonos un e-mail a: administracion@iocc.io ó un whatsapp al 613058003."
                Dim texto_hablado As String = "Estimado cliente, se ha detectado una incidencia administrativa en el servicio que le prestamos. Rogamos nos indique la mejor forma de contactar con su administración, enviándonos un e-mail a: administracion@iocc.io ó un watsaap al seis uno tres cero cinco ocho cero cero tres"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_impagado", "alertify.warning('" & texto & "',20);hablar('" & texto_hablado & "');", True)
                Exit Sub
            End If

            'Si es un usuario nuevo, debe hacer el test de bienvenida
            If gridview_empresa.DataKeys(gridview_empresa.SelectedIndex).Item("bienvenida").ToString() = True Then
                Response.Redirect("bienvenida.aspx?cod_empresa=" & gridview_empresa.DataKeys(gridview_empresa.SelectedIndex).Item("id").ToString() & "&bbdd=" & gridview_empresa.DataKeys(gridview_empresa.SelectedIndex).Item("ruta_base_datos").ToString())
                Exit Sub
            End If

            'Asignacion
            PH_datos.Visible = True
            PH_cargando.Visible = True
            lk_login.Visible = False


            'Creo la llave de Seguridad
            Dim querystringSeguro As TSHAK.Components.SecureQueryString
            querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8})
            querystringSeguro("id_usuario") = gridview_empresa.DataKeys(gridview_empresa.SelectedIndex).Item("id_usuario").ToString()
            querystringSeguro("id_empresa") = gridview_empresa.DataKeys(gridview_empresa.SelectedIndex).Item("id").ToString()

            'Asignar Movil, Tablet o Desktop
            Dim vvalor() As String = txt_tipo_dispositivo.Text.Split("|")
            If vvalor(0) = False And vvalor(1) = False Then
                Response.Redirect("Default.aspx?key=" & HttpUtility.UrlEncode(querystringSeguro.ToString()))
            Else
                Response.Redirect("Default_movil.aspx?key=" & HttpUtility.UrlEncode(querystringSeguro.ToString()))
            End If

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_empresa_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
