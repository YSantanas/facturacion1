Imports System.Data

Partial Class herramientas_datos_activos
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Public Sub leer_cookies()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Limpio
            LT_cookies.Text = Nothing

            'Recorro las cookies
            For i = 0 To Request.Cookies.Count - 1

                'Solo las de Facturación
                If Request.Cookies(i).Name.ToUpper.IndexOf("FACTURACION") <> -1 Then
                    LT_cookies.Text += "<span style='font-size:12px; color:#0d6efd;'>" & Request.Cookies(i).Name.ToUpper & ":</span><br>"
                    LT_cookies.Text += "<span style='font-size:12px; color:#424242;'>" & Request.Cookies(i).Value & "</span><br><br>"
                End If

            Next

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_cookies", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_cookies: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Public Sub leer_session()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Añado Session ID
            LT_sesiones.Text = "<span style='font-size:12px; color:#0d6efd;'>ID_CONTROL</span><br>"
            LT_sesiones.Text += "<span style='font-size:12px; color:#424242;'>" & Session("ID_CONTROL").ToString() & "</span><br><br>"

            For i As Integer = 0 To Session.Count - 1

                If Session.Keys(i).ToString().IndexOf("f_" & tabla_usuario.Rows(0)("Id") & "_tabla_") <> -1 Or Session.Keys(i).ToString().IndexOf("f_" & tabla_empresa.Rows(0)("Id") & "_tabla_") <> -1 Then

                    'Declaro
                    Dim LT As New Literal

                    LT.Text = "<span style='font-size:12px; color:#0d6efd;'>" & Session.Keys(i).ToString().ToUpper & ":</span><br>"

                    Dim tabla As DataTable = HttpContext.Current.Session(Session.Keys(i))

                    Dim GV As New GridView
                    GV.CssClass = "table table-sm table-bordered"
                    GV.Font.Size = 9
                    GV.HeaderStyle.BackColor = Drawing.ColorTranslator.FromHtml("#d4edda")
                    GV.HeaderStyle.Font.Bold = True
                    GV.HeaderStyle.ForeColor = Drawing.Color.Black
                    GV.DataSource = tabla
                    GV.DataBind()

                    'Agrego
                    PL_sesion.Controls.Add(LT)
                    PL_sesion.Controls.Add(GV)

                End If

            Next

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_cookies", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_cookies: " & ex.Message.Replace("'", " ") & "');", True)
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

                'Restricción de Usuarios
                If tabla_usuario.Rows(0)("nivel") = "Invitado" Or tabla_usuario.Rows(0)("nivel") = "Usuario Restringido" Or tabla_usuario.Rows(0)("nivel") = "Usuario" Then
                    'Bloque Jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                End If

                'Leer cookies
                leer_cookies()

                'Leer sessiones
                leer_session()

                'Limpio el trazador
                funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Sesión Activa", "Visualizó todas las variables.")

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
