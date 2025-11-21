Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO

Partial Class configuracion_tipos_impuestos
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_tipos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                memComando.CommandText = "SELECT * FROM [tipo_impuestos] ORDER BY nombre,porcentaje,id,impuesto_defecto;"
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
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipos: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_tipos_impuestos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")


        Try

            'Asigno
            DDL_tipo.Items.Clear()

            'Cargo los detalles
            Me.DDL_tipo.Items.Add(New System.Web.UI.WebControls.ListItem("IGIC", "IGIC"))
            Me.DDL_tipo.Items.Add(New System.Web.UI.WebControls.ListItem("IVA", "IVA"))
            Me.DDL_tipo.Items.Add(New System.Web.UI.WebControls.ListItem("IPSI", "IPSI"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipos_impuestos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipos_impuestos: " & ex.Message.Replace("'", " ") & "');", True)
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
                'End If

                'Leer Tipos
                leer_tipos()

                'Leer tipos de impuestos desde kernel_facturacion
                leer_tipos_impuestos()

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

                'Si no esta activo
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("activo") = False Then
                    'DesAsigno la propiedas  
                    For x = 0 To 2
                        e.Row.Cells(x).CssClass = "gv_rojo"
                    Next
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

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

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
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_excel_Click(sender As Object, e As EventArgs) Handles img_exportar_excel.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Tipos_Impuestos", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Tipos_Impuestos", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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

    Protected Sub chk1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Asigno
            Dim tabla_consulta As New DataTable

            'Capturo de los controles
            Dim checkbox As CheckBox = DirectCast(sender, CheckBox)
            Dim row As GridViewRow = DirectCast(checkbox.NamingContainer, GridViewRow)

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Si activo el % por defecto el resto tienen que ser falsos
                memComando.CommandText = "UPDATE tipo_impuestos SET " &
                    "impuesto_defecto=0;"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Actualizo
                memComando.CommandText = "UPDATE tipo_impuestos SET " &
                    "impuesto_defecto='" & checkbox.Checked & "' " &
                    "WHERE Id ='" & gridview_consulta.DataKeys(row.RowIndex).Values("Id").ToString() & "';"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Tipo Impuestos", "Activó % por Defecto para el tipo: " & gridview_consulta.DataKeys(row.RowIndex).Values("nombre").ToString() & " y el porcentaje: " & gridview_consulta.DataKeys(row.RowIndex).Item("porcentaje").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo datos de tipo de impuestos
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_tipo_impuestos") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].tipo_impuestos ORDER BY impuesto_defecto DESC,porcentaje ASC;")

            'Leer Tipos
            leer_tipos()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Tipo Impuesto modificado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "chk1_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error chk1_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_nueva_Click(sender As Object, e As EventArgs) Handles img_nueva.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            lbl_titulo.Text = "Agregar Tipo de Impuesto"
            btn_grabar.Visible = True
            btn_modificar.Visible = False
            DDL_tipo.SelectedIndex = 0
            txt_porcentaje.Text = Nothing
            chk_activo.Checked = True

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "setTimeout(function () { $('#DDL_tipo').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');"), True)

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

                'Restricción si existen lineas de facturas con ese tipo
                Dim tabla As DataTable = funciones_globales.obtener_datos("SELECT TOP(1) * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].detalles_facturas WHERE porcentaje_impuesto=" & gridview_consulta.DataKeys(index).Item("porcentaje").ToString().Replace(",", ".") & ";")
                If tabla.Rows.Count <> 0 Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('Existen Facturas en el que se ha usado el Tipo de Impuestos que desea eliminar.')", True)
                    Exit Sub
                End If

                'Asignamos
                txt_index.Text = index
                LT_mensaje.Text = "¿Está seguro de eliminar el Tipo de Impuesto: " & gridview_consulta.DataKeys(index).Item("nombre").ToString() & " (" & gridview_consulta.DataKeys(index).Item("porcentaje").ToString() & "% ) ?"

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

            If (e.CommandName = "consultar") Then

                'Asigno
                lbl_titulo.Text = "Consultar Tipo de Impuesto"
                DDL_tipo.SelectedIndex = DDL_tipo.Items.IndexOf(DDL_tipo.Items.FindByText(gridview_consulta.DataKeys(index).Item("nombre").ToString()))
                txt_porcentaje.Text = gridview_consulta.DataKeys(index).Item("porcentaje").ToString().Replace(",", ".")
                chk_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                btn_grabar.Visible = False
                btn_modificar.Visible = False
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#DDL_tipo').attr('disabled',true);$('#txt_porcentaje').attr('disabled',true);$('#chk_activo').attr('disabled',true);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');"), True)

            End If

            If (e.CommandName = "editar") Then

                'Asigno
                lbl_titulo.Text = "Modificar Tipo de Impuesto"
                DDL_tipo.SelectedIndex = DDL_tipo.Items.IndexOf(DDL_tipo.Items.FindByText(gridview_consulta.DataKeys(index).Item("nombre").ToString()))
                txt_porcentaje.Text = gridview_consulta.DataKeys(index).Item("porcentaje").ToString().Replace(",", ".")
                chk_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                btn_grabar.Visible = False
                btn_modificar.Visible = True
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#DDL_tipo').attr('disabled',true);$('#txt_porcentaje').attr('disabled',true);setTimeout(function () { $('#chk_defecto').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');"), True)

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

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                memComando.CommandText = "DELETE FROM tipo_impuestos WHERE Id=" & gridview_consulta.DataKeys(txt_index.Text).Item("Id") & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Tipo Impuestos", "Eliminó el Tipo Impuestos: " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre").ToString() & " (" & gridview_consulta.DataKeys(txt_index.Text).Item("porcentaje").ToString() & ").(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo datos de tipo de impuestos
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_tipo_impuestos") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].tipo_impuestos ORDER BY impuesto_defecto DESC,porcentaje ASC;")

            'Leer Tipos
            leer_tipos()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Tipo de Impuesto eliminado correctamente.');", True)

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

            'Excepcion
            If Not IsNumeric(txt_porcentaje.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_porcentaje').focus();$('#txt_porcentaje').select();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');") & "error('El Porcentaje debe ser numérico.')", True)
                Exit Sub
            End If

            'Restricción si ya existe
            Dim tabla As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].tipo_impuestos WHERE nombre='" & DDL_tipo.SelectedItem.ToString & "' AND porcentaje='" & txt_porcentaje.Text & "';")
            If tabla.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#DDL_tipo').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');") & "error('Ya existe un Tipo de Impuesto con esas características.')", True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                'Declaro
                Dim memComando As New SqlCommand

                'Obtengo el ultimo numero de tipo de impuesto
                tabla = funciones_globales.obtener_datos("SELECT TOP (1) Id FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].tipo_impuestos ORDER BY Id DESC;")
                Dim leer_ultimo_numero_tipos As Integer = 1
                If tabla.Rows.Count <> 0 Then
                    leer_ultimo_numero_tipos = tabla.Rows(0)(0) + 1
                End If
                tabla.Dispose()

                'Agrego
                memComando.CommandText = "INSERT INTO tipo_impuestos (Id,nombre,porcentaje,impuesto_defecto,activo) VALUES " &
                "(" & leer_ultimo_numero_tipos & ",'" & DDL_tipo.SelectedItem.ToString & "',@txt_porcentaje,0,'" & chk_activo.Checked & "');"
                memComando.Parameters.Add(New SqlParameter("@txt_porcentaje", Data.SqlDbType.Decimal, 5, 2))
                memComando.Parameters("@txt_porcentaje").Value = txt_porcentaje.Text.Replace(".", ",")
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Tipo Impuestos", "Creó el Tipo de Impuesto con Tipo: " & DDL_tipo.SelectedItem.ToString & " y Porcentaje: " & txt_porcentaje.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo datos de tipo de impuestos
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_tipo_impuestos") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].tipo_impuestos ORDER BY impuesto_defecto DESC,porcentaje ASC;")

            'Leer Tipos
            leer_tipos()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Tipo de Impuesto creado correctamente.');", True)

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

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                'Declaro
                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE tipo_impuestos SET " &
                    "activo='" & chk_activo.Checked & "' " &
                    "WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("Id") & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Tipo Impuestos", "Actualizó el Tipo de Impuesto para el Tipo: " & DDL_tipo.SelectedItem.ToString & " y Porcentaje: " & txt_porcentaje.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo datos de tipo de impuestos
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_tipo_impuestos") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].tipo_impuestos ORDER BY impuesto_defecto DESC,porcentaje ASC;")

            'Leer Tipos
            leer_tipos()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Tipo de Impuesto modificado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_modificar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_modificar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
