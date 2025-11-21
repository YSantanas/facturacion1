Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class proveedores_contactos
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_sucursal(ByVal id_cliente As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Obtengo
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].[sucursales_proveedores] WHERE id_proveedor = " & id_cliente & " AND id<>0 ORDER BY Id")

            'Asigno
            DDL_sucursal.Items.Clear()
            DDL_sucursal.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("Id").ToString & " - " & tabla_consulta.Rows(x)("denominacion").ToString, tabla_consulta.Rows(x)("Id").ToString)
                Me.DDL_sucursal.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_sucursal", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_sucursal: " & ex.Message.Replace("'", " ") & "');", True)
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

            'Asigno el n_consulta
            gridview_consulta.PageSize = tabla_empresa.Rows(0)("n_paginado_consultas")

            'Ver fondo
            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                '            'Restricción de Usuarios
                '            If parametros_usuario(10) = "Invitado" Then
                '                'Bloque Jquery
                '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                '            End If

                'Si viene con parametros
                If Request.QueryString("id_proveedor") <> Nothing Then

                    'Asigno
                    txt_cliente.Text = Request.QueryString("id_proveedores")
                    txt_sucursal.Text = Request.QueryString("id_sucursal_proveedor")

                    Dim tabla_proveedores As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_proveedores")
                    Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_proveedores, "id", txt_cliente.Text, "nombre_fiscal")
                    lbl_txt_cliente.Text = consulta

                End If

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#btn_consultar').click();", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub txt_cliente_TextChanged(sender As Object, e As EventArgs) Handles txt_cliente.TextChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_proveedores As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_proveedores")

        Try

            'Asigno
            lbl_txt_cliente.Text = ""

            If IsNumeric(txt_cliente_modal.Text) Then

                'Asigno
                Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_proveedores, "id", txt_cliente.Text, "nombre_fiscal")
                If consulta <> "0" Then
                    lbl_txt_cliente.Text = consulta
                End If

            End If

            'Focus
            txt_sucursal.Focus()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "txt_codigo_TextChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error txt_codigo_TextChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_consultar_Click(sender As Object, e As EventArgs) Handles btn_consultar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepción
            If txt_cliente.Text <> "" Then
                If Not IsNumeric(txt_cliente.Text) Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente').focus();$('#txt_cliente').select();}, 100);error('El Proveedor debe ser un código numérico.')"), True)
                    Exit Sub
                End If
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Cargar GV
            cargar_GV()

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Datos Usuario
            lbl_informacion.Text = gridview_consulta.PageCount & " Página(s) - Tiempo (" & tspan.ToString("s\:ff") & " segundos)"

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Contactos", "Solicitó listado de contactos del proveedor: " & txt_cliente.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_consultar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_consultar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub cargar_GV()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Si no selecciono nada
                If txt_cliente.Text = "" Then
                    memComando.CommandText = "SELECT contactos_proveedores.*,proveedores.nombre_fiscal " &
                    "FROM contactos_proveedores INNER JOIN " &
                    "proveedores ON contactos_proveedores.id_proveedor=proveedores.id;"
                Else

                    If txt_sucursal.Text = "" Then
                        memComando.CommandText = "SELECT contactos_proveedores.*,proveedores.nombre_fiscal " &
                        "FROM contactos_proveedores INNER JOIN " &
                        "proveedores ON contactos_proveedores.id_proveedor=proveedores.id " &
                        "WHERE contactos_proveedores.id_proveedor=@id_proveedor;"
                        memComando.Parameters.Add(New SqlParameter("id_proveedor", Data.SqlDbType.Int))
                        memComando.Parameters("id_proveedor").Value = txt_cliente.Text
                    Else
                        memComando.CommandText = "SELECT contactos_proveedores.*,proveedores.nombre_fiscal " &
                        "FROM contactos_proveedores INNER JOIN " &
                        "proveedores ON contactos_proveedores.id_proveedor=proveedores.id " &
                        "WHERE contactos_proveedores.id_proveedor=@id_proveedor AND id_sucursal=@id_sucursal;"
                        memComando.Parameters.Add(New SqlParameter("id_proveedor", Data.SqlDbType.Int))
                        memComando.Parameters("id_proveedor").Value = txt_cliente.Text
                        memComando.Parameters.Add(New SqlParameter("id_sucursal", Data.SqlDbType.Int))
                        memComando.Parameters("id_sucursal").Value = txt_sucursal.Text
                    End If

                End If
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

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "cargar_GV", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error cargar_GV: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                'Si no esta activo
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("id_sucursal") = 0 Then

                    'Asigno
                    e.Row.Cells(1).Text = "CENTRAL"

                End If

                'Fecha de imputacion
                e.Row.ToolTip = "Fecha de creación: " & Mid(gridview_consulta.DataKeys(e.Row.RowIndex).Item("fecha_creacion").ToString(), 1, 10) & " - " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("hora_creacion").ToString() & "."

                'Maquillaje
                e.Row.Cells(2).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("nombre").ToString & " " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("primer_apellido").ToString & " " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("segundo_apellido").ToString

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
            'Obtengo
            Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
            Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_asiento_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_asiento_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridview_consulta.PageIndexChanging

        Try

            'Actualizar posicion y cosnulta
            gridview_consulta.PageIndex = e.NewPageIndex
            cargar_GV()

        Catch ex As Exception
            'Obtengo
            Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
            Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_PageIndexChanging", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_PageIndexChanging: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_excel_Click(sender As Object, e As EventArgs) Handles img_exportar_excel.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Contactos", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Contactos", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            lbl_titulo.Text = "Agregar Contacto"
            btn_grabar.Visible = True
            btn_modificar.Visible = False
            txt_cliente_modal.Text = Nothing
            lbl_txt_cliente_modal.Text = Nothing
            DDL_sucursal.Items.Clear()
            DDL_sucursal.Items.Add(New ListItem("", "0"))
            txt_cargo.Text = Nothing
            txt_nombre.Text = Nothing
            txt_primer_apellido.Text = Nothing
            txt_segundo_apellido.Text = Nothing
            txt_valor.Text = Nothing
            __txt_observaciones.Text = Nothing
            'Oculto
            txt_index.Text = Nothing

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente_modal').focus();}, 100); $('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_nueva_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_nueva_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub txt_cliente_modal_TextChanged(sender As Object, e As EventArgs) Handles txt_cliente_modal.TextChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_proveedores As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_proveedores")

        Try

            If IsNumeric(txt_cliente_modal.Text) Then

                'Asigno
                Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_proveedores, "id", txt_cliente_modal.Text, "nombre_fiscal")
                If consulta <> "0" Then
                    lbl_txt_cliente_modal.Text = consulta
                Else
                    lbl_txt_cliente_modal.Text = Nothing
                End If

                'Leer Sucursal
                leer_sucursal(txt_cliente_modal.Text)

            End If

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("setTimeout(function () { $('#DDL_sucursal').focus();}, 100); $('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "txt_cliente_modal_TextChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error txt_cliente_modal_TextChanged: " & ex.Message.Replace("'", " ") & "');", True)
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
                LT_mensaje.Text = "¿Está seguro de eliminar el Contacto: " & gridview_consulta.DataKeys(index).Item("nombre").ToString() & " " & gridview_consulta.DataKeys(index).Item("primer_apellido").ToString() & " " & gridview_consulta.DataKeys(index).Item("segundo_apellido").ToString() & " del Proveedor: " & gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString() & " y Sucursal Nº: " & gridview_consulta.DataKeys(index).Item("id_sucursal_proveedor").ToString() & "?"

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

            If (e.CommandName = "consultar") Then

                'Asigno
                lbl_titulo.Text = "Consultar Contacto"
                txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("id_proveedor").ToString()
                lbl_txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                'Leer Sucursal
                leer_sucursal(txt_cliente_modal.Text)
                DDL_sucursal.SelectedIndex = DDL_sucursal.Items.IndexOf(DDL_sucursal.Items.FindByValue(gridview_consulta.DataKeys(index).Item("id_sucursal_proveedor")))
                txt_cargo.Text = gridview_consulta.DataKeys(index).Item("cargo").ToString()
                txt_nombre.Text = gridview_consulta.DataKeys(index).Item("nombre").ToString()
                txt_primer_apellido.Text = gridview_consulta.DataKeys(index).Item("primer_apellido").ToString()
                txt_segundo_apellido.Text = gridview_consulta.DataKeys(index).Item("segundo_apellido").ToString()
                txt_valor.Text = gridview_consulta.DataKeys(index).Item("contacto").ToString()
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                btn_grabar.Visible = False
                btn_modificar.Visible = False
                'Oculto
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);$('#DDL_sucursal').attr('disabled',true);$('#txt_cargo').attr('disabled',true);$('#txt_nombre').attr('disabled',true);$('#txt_primer_apellido').attr('disabled',true);$('#txt_segundo_apellido').attr('disabled',true);$('#txt_valor').attr('disabled',true);$('#__txt_observaciones').attr('disabled',true);setTimeout(function () { $('#txt_cargo').focus();$('#txt_cargo').select();}, 100);$('#modal_agregar').modal('show');"), True)

            End If

            If (e.CommandName = "editar") Then

                'Asigno
                lbl_titulo.Text = "Modificar Contacto"
                txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("id_proveedor").ToString()
                lbl_txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                'Leer Sucursal
                leer_sucursal(txt_cliente_modal.Text)
                DDL_sucursal.SelectedIndex = DDL_sucursal.Items.IndexOf(DDL_sucursal.Items.FindByValue(gridview_consulta.DataKeys(index).Item("id_sucursal_proveedor")))
                txt_cargo.Text = gridview_consulta.DataKeys(index).Item("cargo").ToString()
                txt_nombre.Text = gridview_consulta.DataKeys(index).Item("nombre").ToString()
                txt_primer_apellido.Text = gridview_consulta.DataKeys(index).Item("primer_apellido").ToString()
                txt_segundo_apellido.Text = gridview_consulta.DataKeys(index).Item("segundo_apellido").ToString()
                txt_valor.Text = gridview_consulta.DataKeys(index).Item("contacto").ToString()
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                btn_grabar.Visible = False
                btn_modificar.Visible = True
                'Oculto
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);$('#DDL_sucursal').attr('disabled',true);setTimeout(function () { $('#txt_cargo').focus();$('#txt_cargo').select();}, 100);$('#modal_agregar').modal('show');"), True)

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

                memComando.CommandText = "DELETE FROM contactos_proveedores WHERE id_proveedor=" & gridview_consulta.DataKeys(txt_index.Text).Item("id_cliente") & " AND id_sucursal_proveedor=" & gridview_consulta.DataKeys(txt_index.Text).Item("id_sucursal_proveedor") & " AND cargo='" & gridview_consulta.DataKeys(txt_index.Text).Item("cargo") & "' AND nombre='" & gridview_consulta.DataKeys(txt_index.Text).Item("nombre") & "' AND primer_apellido='" & gridview_consulta.DataKeys(txt_index.Text).Item("primer_apellido") & "' AND segundo_apellido='" & gridview_consulta.DataKeys(txt_index.Text).Item("segundo_apellido") & "';"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Contactos", "Eliminó el Conatcto: " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre").ToString() & " " & gridview_consulta.DataKeys(txt_index.Text).Item("primer_apellido").ToString() & " " & gridview_consulta.DataKeys(txt_index.Text).Item("segundo_apellido").ToString() & " del Cliente:  " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fiscal").ToString() & " y la Sucursal: " & gridview_consulta.DataKeys(txt_index.Text).Item("id_sucursal").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Cargar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Contacto eliminado correctamente.');", True)

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
            If Not IsNumeric(txt_cliente_modal.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente_modal').focus();$('#txt_cliente_modal').select();}, 100);$('#modal_agregar').modal('show');error('El Proveedor debe ser un código numérico.')"), True)
                Exit Sub
            End If

            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) id FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[proveedores] WHERE id=" & txt_cliente_modal.Text & ";")
            If tabla_consulta.Rows.Count = 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente_modal').focus();$('#txt_cliente_modal').select();}, 100);$('#modal_agregar').modal('show');error('El Código de Proveedor no existe.')"), True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Agrego
                memComando.CommandText = "INSERT INTO contactos_proveedores (id_proveedor,id_sucursal_proveedor,fecha_creacion,hora_creacion,cargo,nombre,primer_apellido,segundo_apellido,contacto,observaciones) VALUES " &
                            "(@id_cliente,@id_sucursal,@fecha_creacion,@hora_creacion,@cargo,@nombre,@primer_apellido,@segundo_apellido,@contacto,@observaciones);"
                memComando.Parameters.Add("@id_cliente", Data.SqlDbType.Int).Value = txt_cliente_modal.Text
                memComando.Parameters.Add("@id_sucursal", Data.SqlDbType.Int).Value = DDL_sucursal.SelectedValue
                memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
                memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@cargo", Data.SqlDbType.VarChar, 150).Value = txt_cargo.Text.ToUpper
                memComando.Parameters.Add("@nombre", Data.SqlDbType.VarChar, 150).Value = txt_nombre.Text.ToUpper
                memComando.Parameters.Add("@primer_apellido", Data.SqlDbType.VarChar, 150).Value = txt_primer_apellido.Text.ToUpper
                memComando.Parameters.Add("@segundo_apellido", Data.SqlDbType.VarChar, 150).Value = txt_segundo_apellido.Text.ToUpper
                memComando.Parameters.Add("@contacto", Data.SqlDbType.VarChar, 200).Value = txt_valor.Text.ToUpper
                memComando.Parameters.Add("@observaciones", Data.SqlDbType.VarChar, 1000).Value = __txt_observaciones.Text.ToUpper
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Contactos", "Creó el Contacto: " & txt_nombre.Text & " " & txt_primer_apellido.Text & " " & txt_segundo_apellido.Text & " para el Proveedor: " & lbl_txt_cliente_modal.Text & " y la Sucursal: " & DDL_sucursal.SelectedValue.ToString & ".(Tiempo:  " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Leer Tipos
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Contacto creado correctamente.');", True)

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
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE contactos_proveedores SET " &
                "cargo=@cargo, " &
                "nombre=@nombre, " &
                "primer_apellido=@primer_apellido, " &
                "segundo_apellido=@segundo_apellido, " &
                "contacto=@contacto, " &
                "observaciones=@observaciones " &
                "WHERE id_proveedor=" & gridview_consulta.DataKeys(txt_index.Text).Item("id_proveedor") & " AND id_sucursal_proveedor=" & gridview_consulta.DataKeys(txt_index.Text).Item("id_sucursal_proveedor") & " AND cargo='" & gridview_consulta.DataKeys(txt_index.Text).Item("cargo") & "' AND nombre='" & gridview_consulta.DataKeys(txt_index.Text).Item("nombre") & "' AND primer_apellido='" & gridview_consulta.DataKeys(txt_index.Text).Item("primer_apellido") & "' AND segundo_apellido='" & gridview_consulta.DataKeys(txt_index.Text).Item("segundo_apellido") & "';"
                memComando.Parameters.Add("@cargo", Data.SqlDbType.VarChar, 150).Value = txt_cargo.Text.ToUpper
                memComando.Parameters.Add("@nombre", Data.SqlDbType.VarChar, 150).Value = txt_nombre.Text.ToUpper
                memComando.Parameters.Add("@primer_apellido", Data.SqlDbType.VarChar, 150).Value = txt_primer_apellido.Text.ToUpper
                memComando.Parameters.Add("@segundo_apellido", Data.SqlDbType.VarChar, 150).Value = txt_segundo_apellido.Text.ToUpper
                memComando.Parameters.Add("@contacto", Data.SqlDbType.VarChar, 200).Value = txt_valor.Text.ToUpper
                memComando.Parameters.Add("@observaciones", Data.SqlDbType.VarChar, 1000).Value = __txt_observaciones.Text.ToUpper
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Cierro la BBDD
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Contactos", "Modificó el Contacto: " & txt_nombre.Text & " " & txt_primer_apellido.Text & " " & txt_segundo_apellido.Text & " para el Proveedor: " & lbl_txt_cliente_modal.Text & " y la Sucursal: " & DDL_sucursal.SelectedValue.ToString & ".(Tiempo:  " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Leer Tipos
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Sucursal modificada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_modificar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_modificar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
