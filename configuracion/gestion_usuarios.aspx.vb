Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO

Partial Class configuracion_gestion_usuarios
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_nivel()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Limpio el control
            DDL_nivel_usuario.Items.Clear()

            'Añado un tipo para todos
            Me.DDL_nivel_usuario.Items.Add(New System.Web.UI.WebControls.ListItem("Invitado", "Invitado"))
            Me.DDL_nivel_usuario.Items.Add(New System.Web.UI.WebControls.ListItem("Usuario Restringido", "Usuario Restringido"))
            Me.DDL_nivel_usuario.Items.Add(New System.Web.UI.WebControls.ListItem("Usuario", "Usuario"))
            Me.DDL_nivel_usuario.Items.Add(New System.Web.UI.WebControls.ListItem("Auditor", "Auditor"))
            Me.DDL_nivel_usuario.Items.Add(New System.Web.UI.WebControls.ListItem("Supervisor", "Supervisor"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_nivel", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_nivel: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_conceptos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                Dim memComando As New SqlCommand

                memComando.CommandText = "SELECT Id,nombre,primer_apellido, segundo_apellido,email,password,nivel,baja,codigo_empresa,fecha_creacion,hora_creacion,ip,posicion " &
                "FROM usuarios INNER JOIN " &
                "usuarios_empresas ON usuarios.Id = id_usuario " &
                "WHERE usuarios_empresas.id_empresa = " & tabla_empresa.Rows(0)("Id") & " ORDER BY nombre;"
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
            gridview_consulta.DataSource = tabla_consulta
            gridview_consulta.DataBind()

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Datos Usuario
            lbl_informacion.Text = "Nª Registros: " & tabla_consulta.Rows.Count & " - Tiempo (" & tspan.ToString("s\:ff") & " segundos)"

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_conceptos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_conceptos: " & ex.Message.Replace("'", " ") & "');", True)
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

                ''Restricción de Usuarios
                'If tabla_usuario.Rows(0)("nivel") = "Invitado" Or tabla_usuario.Rows(0)("nivel") = "Usuario Restringido" Or tabla_usuario.Rows(0)("nivel") = "Usuario" Then
                '    'Bloque Jquery
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", funciones_globales.modal_register("$('#modal_sin_acceso').modal('show');"), True)
                '    img_nueva.Visible = False
                '    Exit Sub
                'End If

                'Leer Tipos
                leer_conceptos()

                'Leer Nivel
                leer_nivel()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then


                Dim onmouseoverStyle As String = "this.style.backgroundColor='#f2ffe4';this.style.cursor='Default'"
                Dim onmouseoutStyle As String = "this.style.backgroundColor='white'"
                Dim cursoronmouse As String = "this.style.cursor='Default'"

                'Asigno
                e.Row.Attributes.Add("onmouseover", onmouseoverStyle)
                e.Row.Attributes.Add("onmouseout", onmouseoutStyle)

                'DesAsigno la propiedas  
                For x = 6 To e.Row.Cells.Count - 1
                    e.Row.Cells(x).BackColor = ColorTranslator.FromHtml("#FFFFFF")
                Next

                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("baja").ToString() = True Then
                    e.Row.Cells(5).Text = "Si"
                Else
                    e.Row.Cells(5).Text = "No"
                End If

                'Si no es propietario no dejo ver la ubicacion de creacion
                Dim btn_ubicacion As Button = DirectCast(e.Row.FindControl("btn_ubicacion"), Button)
                If tabla_usuario.Rows(0)("nivel") <> "Propietario" Then
                    btn_ubicacion.Enabled = False
                    e.Row.Cells(8).ToolTip = "Su nivel es insuficiente"
                End If

                'Si es propietario 
                Dim btn_editar As Button = DirectCast(e.Row.FindControl("btnedit"), Button)
                Dim btn_borrar As Button = DirectCast(e.Row.FindControl("btndelete"), Button)
                If tabla_usuario.Rows(0)("nivel") = "Propietario" Then
                    btn_editar.Enabled = True
                    btn_borrar.Enabled = True
                End If

                If tabla_usuario.Rows(0)("nivel") = "Supervisor" Then
                    btn_editar.Enabled = True
                    btn_borrar.Enabled = True
                    If gridview_consulta.DataKeys(e.Row.RowIndex).Item("nivel").ToString() = "Propietario" Then
                        btn_editar.Enabled = False
                        btn_borrar.Enabled = False
                    End If
                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_DataBound(sender As Object, e As EventArgs) Handles gridview_consulta.DataBound

        Try

            'Control para menus de exportar
            If gridview_consulta.Rows.Count = 0 Then
                img_exportar_excel.Visible = False
                img_exportar_txt.Visible = False
                lbl_informacion.Visible = False
            Else
                img_exportar_excel.Visible = True
                img_exportar_txt.Visible = True
                lbl_informacion.Visible = True
            End If

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_asiento_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_asiento_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_excel_Click(sender As Object, e As EventArgs) Handles img_exportar_excel.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Usuarios", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "xlsx"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_exportar_excel_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_exportar_excel_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_txt_Click(sender As Object, e As EventArgs) Handles img_exportar_txt.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Usuarios", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "txt"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_exportar_txt_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_exportar_txt_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_nueva_Click(sender As Object, e As EventArgs) Handles img_nueva.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            lbl_titulo.Text = "Agregar Usuario"
            btn_grabar.Visible = True
            btn_modificar.Visible = False
            txt_nombre_usuario.Text = Nothing
            txt_primer_apellido_usuario.Text = Nothing
            txt_segundo_apellido_usuario.Text = Nothing
            txt_codigo_usuario.Text = Nothing
            txt_paswword_usuario.Text = Nothing
            leer_nivel()
            DDL_nivel_usuario.SelectedIndex = 0
            CB_baja.Checked = False


            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#txt_codigo_usuario').attr('disabled',false);setTimeout(function () { $('#txt_nombre_usuario').focus();}, 100);" & funciones_globales.modal_register(" $('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_nueva_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_nueva_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "borrar") Then

                'Asignamos
                txt_index.Text = index
                LT_mensaje.Text = "¿Está seguro de eliminar el Usuario: " & gridview_consulta.DataKeys(index).Item("nombre").ToString() & " " & gridview_consulta.DataKeys(index).Item("primer_apellido").ToString() & " " & gridview_consulta.DataKeys(index).Item("segundo_apellido").ToString() & "?"

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

            If (e.CommandName = "editar") Then

                'Asigno
                lbl_titulo.Text = "Modificar Usuario"
                txt_nombre_usuario.Text = gridview_consulta.DataKeys(index).Item("nombre").ToString()
                txt_primer_apellido_usuario.Text = gridview_consulta.DataKeys(index).Item("primer_apellido").ToString()
                txt_segundo_apellido_usuario.Text = gridview_consulta.DataKeys(index).Item("segundo_apellido").ToString()
                txt_codigo_usuario.Text = gridview_consulta.DataKeys(index).Item("email").ToString()
                txt_paswword_usuario.Text = gridview_consulta.DataKeys(index).Item("password").ToString()
                If tabla_usuario.Rows(0)("nivel") = "Propietario" And gridview_consulta.DataKeys(index).Item("nivel").ToString() = "Propietario" Then
                    'Limpio el control
                    DDL_nivel_usuario.Items.Clear()
                    'Añado un tipo para todos
                    Me.DDL_nivel_usuario.Items.Add(New System.Web.UI.WebControls.ListItem("Propietario", "Propietario"))
                Else
                    leer_nivel()
                    DDL_nivel_usuario.SelectedIndex = DDL_nivel_usuario.Items.IndexOf(DDL_nivel_usuario.Items.FindByValue(gridview_consulta.DataKeys(index).Item("nivel").ToString()))
                End If
                CB_baja.Checked = gridview_consulta.DataKeys(index).Item("baja").ToString()
                btn_grabar.Visible = False
                btn_modificar.Visible = True
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#txt_codigo_usuario').attr('disabled',false);setTimeout(function () { $('#txt_nombre_usuario').focus();$('#txt_nombre_usuario').select();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');"), True)

            End If

            If (e.CommandName = "ubicacion") Then

                'Asignamos
                txt_fecha_creacion.Text = Mid(gridview_consulta.DataKeys(index).Item("fecha_creacion").ToString(), 1, 10)
                txt_hora_creacion.Text = gridview_consulta.DataKeys(index).Item("hora_creacion").ToString()
                txt_ip.Text = gridview_consulta.DataKeys(index).Item("ip").ToString()
                txt_posicion.Text = gridview_consulta.DataKeys(index).Item("posicion").ToString() & "|" & gridview_consulta.DataKeys(index).Item("nombre").ToString() & " " & gridview_consulta.DataKeys(index).Item("primer_apellido").ToString()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_eliminar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_eliminar_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                memComando.CommandText = "DELETE FROM usuarios WHERE Id=" & gridview_consulta.DataKeys(txt_index.Text).Item("Id") & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                memComando.CommandText = "DELETE FROM usuarios_empresas WHERE Id_usuario=" & gridview_consulta.DataKeys(txt_index.Text).Item("Id") & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Gestión Usuarios", "Eliminó el Usuario: " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre").ToString() & " " & gridview_consulta.DataKeys(txt_index.Text).Item("primer_apellido").ToString() & " " & gridview_consulta.DataKeys(txt_index.Text).Item("segundo_apellido").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Leer Tipos
            leer_conceptos()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Usuario eliminado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_eliminar_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_eliminar_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_Click(sender As Object, e As EventArgs) Handles btn_grabar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Si el correo no es válido o intento inyección SQL
            If funciones_globales.IsValidEmail(txt_codigo_usuario.Text) = False Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('El E-mail introducido no es válido.');", True)
                Exit Sub
            End If

            'Restricción si existe
            Dim tabla As DataTable = funciones_globales.obtener_datos("SELECT usuarios.Id, usuarios_empresas.id_empresa FROM [kernel_facturacion].[dbo].[usuarios],[kernel_facturacion].[dbo].[usuarios_empresas] WHERE usuarios_empresas.id_usuario = usuarios.Id AND email='" & txt_codigo_usuario.Text & "' AND id_empresa=" & tabla_empresa.Rows(0)("Id") & ";")
            If tabla.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_codigo_usuario').focus();$('#txt_codigo_usuario').select();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');") & "error('Ya existe este Usuario en esta empresa.')", True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Agrego
                memComando.CommandText = "INSERT INTO usuarios (nombre,primer_apellido,segundo_apellido,email,password,codigo_empresa,nivel,baja,fecha_creacion,hora_creacion,ip,posicion) VALUES " &
                    "(@txt_nombre_usuario,@txt_primer_apellido_usuario,@txt_segundo_apellido_usuario,@txt_codigo_usuario,@txt_paswword_usuario,'" & tabla_empresa.Rows(0)("Id") & "','" & DDL_nivel_usuario.SelectedItem.ToString & "','" & CB_baja.Checked & "','" & DateTime.Today & "','" & Now.ToString("HH:mm:ss") & "','" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString() & "',@posicion);"
                memComando.Parameters.Add(New SqlParameter("txt_nombre_usuario", Data.SqlDbType.VarChar, 25))
                memComando.Parameters("txt_nombre_usuario").Value = txt_nombre_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_primer_apellido_usuario", Data.SqlDbType.VarChar, 50))
                memComando.Parameters("txt_primer_apellido_usuario").Value = txt_primer_apellido_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_segundo_apellido_usuario", Data.SqlDbType.VarChar, 50))
                memComando.Parameters("txt_segundo_apellido_usuario").Value = txt_segundo_apellido_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_codigo_usuario", Data.SqlDbType.VarChar, 80))
                memComando.Parameters("txt_codigo_usuario").Value = txt_codigo_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_paswword_usuario", Data.SqlDbType.VarChar, 20))
                memComando.Parameters("txt_paswword_usuario").Value = txt_paswword_usuario.Text
                memComando.Parameters.Add(New SqlParameter("posicion", Data.SqlDbType.VarChar, 40))
                memComando.Parameters("posicion").Value = txt_latitud.Text & "|" & txt_longitud.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Obtengo el ultimo numero 
                tabla = funciones_globales.obtener_datos("SELECT TOP (1) Id FROM [kernel_facturacion].[dbo].[usuarios] WHERE email='" & txt_codigo_usuario.Text & "' AND codigo_empresa=" & tabla_empresa.Rows(0)("Id") & ";")
                Dim leer_ultimo_numero_usuario As Integer = 0
                If tabla.Rows.Count <> 0 Then
                    leer_ultimo_numero_usuario = tabla.Rows(0)(0)
                End If
                tabla.Dispose()

                'Escribo en la tabla usuarios-empresa
                memComando.CommandText = "INSERT INTO usuarios_empresas (id_usuario, id_empresa) VALUES " &
                    "(" & leer_ultimo_numero_usuario & ", " & tabla_empresa.Rows(0)("Id") & ");"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Gestión Usuarios", "Creó el Usuario: " & txt_nombre_usuario.Text & " " & txt_primer_apellido_usuario.Text & " " & txt_segundo_apellido_usuario.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Leer Tipos
            leer_conceptos()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Usuario creado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_modificar_Click(sender As Object, e As EventArgs) Handles btn_modificar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Si el correo no es válido o intento inyección SQL
            If funciones_globales.IsValidEmail(txt_codigo_usuario.Text) = False Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('El E-mail introducido no es válido.');", True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE usuarios SET " &
                    "nombre=@txt_nombre_usuario, " &
                    "primer_apellido=@txt_primer_apellido_usuario, " &
                    "segundo_apellido=@txt_segundo_apellido_usuario, " &
                    "email=@txt_codigo_usuario, " &
                    "password=@txt_paswword_usuario, " &
                    "nivel='" & DDL_nivel_usuario.SelectedItem.ToString & "' ," &
                    "baja='" & CB_baja.Checked & "' " &
                    "WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("Id") & ";"

                memComando.Parameters.Add(New SqlParameter("txt_nombre_usuario", Data.SqlDbType.VarChar, 25))
                memComando.Parameters("txt_nombre_usuario").Value = txt_nombre_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_primer_apellido_usuario", Data.SqlDbType.VarChar, 50))
                memComando.Parameters("txt_primer_apellido_usuario").Value = txt_primer_apellido_usuario.Text
                memComando.Parameters.Add(New SqlParameter("txt_segundo_apellido_usuario", Data.SqlDbType.VarChar, 50))
                memComando.Parameters("txt_segundo_apellido_usuario").Value = txt_segundo_apellido_usuario.Text
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

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Gestión Usuarios", "Modificado el Usuario: " & txt_nombre_usuario.Text & " " & txt_primer_apellido_usuario.Text & " " & txt_segundo_apellido_usuario.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Leer Tipos
            leer_conceptos()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Usuario modificado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_modificar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_modificar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class