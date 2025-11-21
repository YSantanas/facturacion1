Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class proveedores_altas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_tipo_vias()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[tipo_vias] ORDER BY nombre")

            'Asigno
            DDL_tipo_via.Items.Clear()
            DDL_tipo_via.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre").ToString, tabla_consulta.Rows(x)("Id").ToString)
                Me.DDL_tipo_via.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipo_vias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipo_vias: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_provincias()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT nombre,valor FROM [kernel_facturacion].[dbo].[provincia] ORDER BY nombre")

            'Asigno
            DDL_provincia.Items.Clear()
            DDL_provincia.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre").ToString, tabla_consulta.Rows(x)("valor").ToString)
                Me.DDL_provincia.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_provincias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_provincias: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub listar_localidad(ByVal provincia As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[localidad] WHERE id_provincia=" & provincia & " ORDER BY cp;")

            'Asigno
            DDL_localidad.Items.Clear()
            DDL_localidad.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre").ToString & " - (CP:" & tabla_consulta.Rows(x)("cp").ToString & ")", tabla_consulta.Rows(x)("id_localidad"))
                Me.DDL_localidad.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "listar_localidad", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error listar_localidad: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub listar_cp(ByVal id_provincia As Integer, ByVal id_localidad As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[localidad] WHERE id_provincia=" & id_provincia & " AND id_localidad='" & id_localidad & "';")

            'Asigno
            txt_cp.Text = tabla_consulta.Rows(0)("cp").ToString
            txt_cp_enable.Text = tabla_consulta.Rows(0)("cp").ToString

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "listar_cp", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error listar_cp: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_pais()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT id,nombre_pais FROM [kernel_facturacion].[dbo].[paises] ORDER BY nombre_pais;")

            'Asigno
            DDL_pais.Items.Clear()
            DDL_pais.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre_pais").ToString, tabla_consulta.Rows(x)("id"))
                Me.DDL_pais.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_pais", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_pais: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_sucursales(ByVal id_cliente As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT sucursales_proveedores.*,proveedores.nombre_fiscal, " &
                    "tipo_vias.nombre As nombre_tipo_via " &
                    "FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[proveedores] INNER JOIN " &
                    tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[sucursales_proveedores] ON proveedores.id=sucursales_proveedores.id_proveedor LEFT JOIN " &
                    "[kernel_facturacion].[dbo].tipo_vias ON sucursales_proveedores.tipo_via=tipo_vias.id " &
                    "WHERE sucursales_proveedores.id_proveedor=" & id_cliente & " AND sucursales_proveedores.id<>0 ORDER BY id;")

            'Asigno
            Lb_sucursales.Items.Clear()

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem("Sucursal: " & tabla_consulta.Rows(x)("Id").ToString & " - " & tabla_consulta.Rows(x)("nombre_tipo_via").ToString & " " & tabla_consulta.Rows(x)("calle").ToString & ", N:" & tabla_consulta.Rows(x)("numero").ToString, tabla_consulta.Rows(x)("Id").ToString)
                Me.Lb_sucursales.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipo_vias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipo_vias: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_contactos(ByVal id_cliente As Integer, ByVal id_sucursal As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM " &
                    tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[contactos_proveedores] " &
                    "WHERE id_proveedor=" & id_cliente & " AND id_sucursal_proveedor=" & id_sucursal & " ORDER BY nombre;")

            'Asigno
            Lb_contactos.Items.Clear()

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre").ToString & " " & tabla_consulta.Rows(x)("primer_apellido").ToString & " " & tabla_consulta.Rows(x)("segundo_apellido").ToString & " - " & tabla_consulta.Rows(x)("contacto").ToString, tabla_consulta.Rows(x)("id_cliente").ToString & "|" & tabla_consulta.Rows(x)("id_sucursal").ToString)
                Me.Lb_contactos.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipo_vias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipo_vias: " & ex.Message.Replace("'", " ") & "');", True)
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
                    txt_cliente.Text = Request.QueryString("id_proveedor")

                    Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_proveedores")
                    Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_clientes, "id", txt_cliente.Text, "nombre_fiscal")
                    lbl_txt_cliente.Text = consulta

                End If

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#btn_consultar').click();setTimeout(function () { $('#txt_cliente').focus();$('#txt_cliente').select();}, 100);", True)

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

            'Excepcion
            If IsNumeric(txt_cliente.Text) Then

                'Asigno
                Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_proveedores, "id", txt_cliente.Text, "nombre_fiscal")
                If consulta <> "0" Then
                    lbl_txt_cliente.Text = consulta
                End If

            End If

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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Solicitó listado del proveedor: " & txt_cliente.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

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
                    memComando.CommandText = "SELECT * FROM proveedores ORDER BY Id;"
                Else
                    memComando.CommandText = "SELECT * FROM proveedores WHERE Id = @id ORDER BY id;"
                    memComando.Parameters.Add(New SqlParameter("id", Data.SqlDbType.Int))
                    memComando.Parameters("id").Value = txt_cliente.Text
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
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("activo") = False Then
                    'DesAsigno la propiedas  
                    For x = 0 To 2
                        e.Row.Cells(x).CssClass = "gv_rojo"
                    Next
                End If

                'Fecha de imputacion
                e.Row.ToolTip = "Fecha de creación: " & Mid(gridview_consulta.DataKeys(e.Row.RowIndex).Item("fecha_creacion").ToString(), 1, 10) & " - " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("hora_creacion").ToString() & "."

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
            Dim nombre As String = funciones_globales.crear_excel("Alta de Proveedores", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            Dim nombre As String = funciones_globales.crear_txt("Alta de Proveedores", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            lbl_titulo.Text = "Agregar Proveedor"
            btn_grabar.Visible = True
            btn_modificar.Visible = False
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) id FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[proveedores] ORDER BY id DESC")
            If tabla_consulta.Rows.Count = 0 Then
                txt_cliente_modal.Text = "1"
            Else
                txt_cliente_modal.Text = tabla_consulta.Rows(0)("id") + 1
            End If
            CB_activo.Checked = True
            txt_nombre_fiscal.Text = Nothing
            txt_nombre_comercial.Text = Nothing
            txt_nif.Text = Nothing
            'Leer Tipo Via
            leer_tipo_vias()
            DDL_tipo_via.SelectedIndex = 0
            txt_domicilio.Text = Nothing
            txt_numero.Text = Nothing
            txt_escalera.Text = Nothing
            txt_piso.Text = Nothing
            txt_puerta.Text = Nothing
            'Leer Provincia
            leer_provincias()
            DDL_provincia.SelectedIndex = 0
            DDL_localidad.Items.Clear()
            'Leer Pais
            leer_pais()
            DDL_pais.SelectedIndex = 0
            txt_cp.Text = Nothing
            Lb_contactos.Items.Clear()
            Lkb_refrecar_contactos.Visible = False
            abrir_contactos.Visible = False
            Lb_sucursales.Items.Clear()
            Lkb_refrecar_sucursales.Visible = False
            abrir_sucursales.Visible = False
            __txt_observaciones.Text = Nothing
            'Oculto
            txt_index.Text = Nothing
            txt_cp_enable.Text = Nothing

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente_modal').focus();}, 100); $('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_nueva_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_nueva_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub DDL_provincia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_provincia.SelectedIndexChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            If txt_index.Text <> Nothing Then
                txt_cliente_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id").ToString()
            End If

            'Leer Porcentajes de impuestos
            listar_localidad(DDL_provincia.SelectedValue)

            If txt_index.Text <> Nothing Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_codigo').attr('disabled',true);setTimeout(function () { $('#DDL_localidad').focus();}, 100); $('#modal_agregar').modal('show');"), True)
            Else
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("setTimeout(function () { $('#DDL_localidad').focus();}, 100); $('#modal_agregar').modal('show');"), True)
            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "DDL_provincia_SelectedIndexChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error DDL_provincia_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub DDL_localidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_localidad.SelectedIndexChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            If txt_index.Text <> Nothing Then
                txt_cliente_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id").ToString()
            End If

            'Leer Localidad
            listar_cp(DDL_provincia.SelectedValue, DDL_localidad.SelectedValue)

            If txt_index.Text <> Nothing Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_codigo').attr('disabled',true);setTimeout(function () { $('#DDL_localidad').focus();}, 100); $('#modal_agregar').modal('show');"), True)
            Else
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("setTimeout(function () { $('#DDL_localidad').focus();}, 100); $('#modal_agregar').modal('show');"), True)
            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "DDL_localidad_SelectedIndexChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error DDL_localidad_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
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

                'Restricción si existen sucursales no se puede borrar
                Dim tabla As DataTable = funciones_globales.obtener_datos("SELECT TOP(1) * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].[sucursales_proveedores] WHERE id_cliente='" & gridview_consulta.DataKeys(index).Item("id").ToString() & "' AND id<>0;")
                If tabla.Rows.Count <> 0 Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('Existen Sucursales para el proveedor: " & gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString() & ", debe eliminar primero dichas sucursales.')", True)
                    Exit Sub
                End If

                'Asignamos
                txt_index.Text = index
                LT_mensaje.Text = "¿Está seguro de eliminar al Proveedor: " & gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString() & " (" & gridview_consulta.DataKeys(index).Item("nif").ToString() & ") ?"

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

            If (e.CommandName = "consultar") Then

                'Asigno
                lbl_titulo.Text = "Consultar Proveedor"
                txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("id").ToString()
                lbl_txt_codigo.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                CB_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                txt_nombre_fiscal.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                txt_nombre_comercial.Text = gridview_consulta.DataKeys(index).Item("nombre_comercial").ToString()
                txt_nif.Text = gridview_consulta.DataKeys(index).Item("nif").ToString()
                'Leer Tipo Via
                leer_tipo_vias()
                DDL_tipo_via.SelectedIndex = DDL_tipo_via.Items.IndexOf(DDL_tipo_via.Items.FindByValue(gridview_consulta.DataKeys(index).Item("tipo_via")))
                txt_domicilio.Text = gridview_consulta.DataKeys(index).Item("calle").ToString()
                txt_numero.Text = gridview_consulta.DataKeys(index).Item("numero").ToString()
                txt_escalera.Text = gridview_consulta.DataKeys(index).Item("escalera").ToString()
                txt_piso.Text = gridview_consulta.DataKeys(index).Item("piso").ToString()
                txt_puerta.Text = gridview_consulta.DataKeys(index).Item("puerta").ToString()
                'Leer Provincia
                leer_provincias()
                DDL_provincia.SelectedIndex = DDL_provincia.Items.IndexOf(DDL_provincia.Items.FindByValue(gridview_consulta.DataKeys(index).Item("provincia")))
                'Leer Localidad
                listar_localidad(DDL_provincia.SelectedValue)
                DDL_localidad.SelectedIndex = DDL_localidad.Items.IndexOf(DDL_localidad.Items.FindByValue(gridview_consulta.DataKeys(index).Item("localidad").ToString()))
                'Leer Pais
                leer_pais()
                DDL_pais.SelectedIndex = DDL_pais.Items.IndexOf(DDL_pais.Items.FindByValue(gridview_consulta.DataKeys(index).Item("pais")))
                txt_cp.Text = gridview_consulta.DataKeys(index).Item("cp").ToString()
                Lkb_refrecar_contactos.Visible = True
                abrir_contactos.Visible = True
                Lkb_refrecar_sucursales.Visible = True
                abrir_sucursales.Visible = True
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                'leer contactos
                leer_contactos(gridview_consulta.DataKeys(index).Item("id"), 0)
                'leer sucursales
                leer_sucursales(gridview_consulta.DataKeys(index).Item("id"))

                btn_grabar.Visible = False
                btn_modificar.Visible = False
                txt_index.Text = index
                txt_cp_enable.Text = gridview_consulta.DataKeys(index).Item("cp").ToString()

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);$('#txt_nombre_fiscal').attr('disabled',true);$('#txt_nombre_comercial').attr('disabled',true);$('#txt_nif').attr('disabled',true);$('#DDL_tipo_via').attr('disabled',true);$('#txt_domicilio').attr('disabled',true);$('#txt_numero').attr('disabled',true);$('#txt_escalera').attr('disabled',true);$('#txt_piso').attr('disabled',true);$('#txt_puerta').attr('disabled',true);$('#DDL_provincia').attr('disabled',true);$('#DDL_localidad').attr('disabled',true);$('#DDL_pais').attr('disabled',true);$('#Lb_contactos').attr('disabled',true);$('#Lb_sucursales').attr('disabled',true);$('#CB_activo').attr('disabled',true);setTimeout(function () { $('#txt_nombre_fiscal').focus();$('#txt_nombre_fiscal').select();}, 100);$('#modal_agregar').modal('show');"), True)

            End If

            If (e.CommandName = "editar") Then

                'Asigno
                lbl_titulo.Text = "Modificar Proveedor"
                txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("id").ToString()
                lbl_txt_codigo.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                CB_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                txt_nombre_fiscal.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                txt_nombre_comercial.Text = gridview_consulta.DataKeys(index).Item("nombre_comercial").ToString()
                txt_nif.Text = gridview_consulta.DataKeys(index).Item("nif").ToString()
                'Leer Tipo Via
                leer_tipo_vias()
                DDL_tipo_via.SelectedIndex = DDL_tipo_via.Items.IndexOf(DDL_tipo_via.Items.FindByValue(gridview_consulta.DataKeys(index).Item("tipo_via")))
                txt_domicilio.Text = gridview_consulta.DataKeys(index).Item("calle").ToString()
                txt_numero.Text = gridview_consulta.DataKeys(index).Item("numero").ToString()
                txt_escalera.Text = gridview_consulta.DataKeys(index).Item("escalera").ToString()
                txt_piso.Text = gridview_consulta.DataKeys(index).Item("piso").ToString()
                txt_puerta.Text = gridview_consulta.DataKeys(index).Item("puerta").ToString()
                'Leer Provincia
                leer_provincias()
                DDL_provincia.SelectedIndex = DDL_provincia.Items.IndexOf(DDL_provincia.Items.FindByValue(gridview_consulta.DataKeys(index).Item("provincia")))
                'Leer Localidad
                listar_localidad(DDL_provincia.SelectedValue)
                DDL_localidad.SelectedIndex = DDL_localidad.Items.IndexOf(DDL_localidad.Items.FindByValue(gridview_consulta.DataKeys(index).Item("localidad").ToString()))
                'Leer Pais
                leer_pais()
                DDL_pais.SelectedIndex = DDL_pais.Items.IndexOf(DDL_pais.Items.FindByValue(gridview_consulta.DataKeys(index).Item("pais")))
                txt_cp.Text = gridview_consulta.DataKeys(index).Item("cp").ToString()
                Lkb_refrecar_contactos.Visible = True
                abrir_contactos.Visible = True
                Lkb_refrecar_sucursales.Visible = True
                abrir_sucursales.Visible = True
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                'leer contactos
                leer_contactos(gridview_consulta.DataKeys(index).Item("id"), 0)
                'leer sucursales
                leer_sucursales(gridview_consulta.DataKeys(index).Item("id"))

                btn_grabar.Visible = False
                btn_modificar.Visible = True
                txt_index.Text = index
                txt_cp_enable.Text = gridview_consulta.DataKeys(index).Item("cp").ToString()

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);setTimeout(function () { $('#txt_nombre_fiscal').focus();$('#txt_nombre_fiscal').select();}, 100);$('#modal_agregar').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub Lkb_refrecar_contactos_Click(sender As Object, e As EventArgs) Handles Lkb_refrecar_contactos.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'leer sucursales
            leer_contactos(gridview_consulta.DataKeys(txt_index.Text).Item("id"), 0)

            'Asignar
            txt_cliente_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);setTimeout(function () { $('#Lb_contactos').focus();}, 100);$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Lkb_refrecar_contactos_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Lkb_refrecar_contactos_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub Lkb_refrecar_sucursales_Click(sender As Object, e As EventArgs) Handles Lkb_refrecar_sucursales.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'leer sucursales
            leer_sucursales(gridview_consulta.DataKeys(txt_index.Text).Item("id"))

            'Asignar
            txt_cliente_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);setTimeout(function () { $('#Lb_sucursales').focus();}, 100);$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Lkb_refrecar_sucursales_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Lkb_refrecar_sucursales_Click: " & ex.Message.Replace("'", " ") & "');", True)
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

                memComando.CommandText = "DELETE FROM proveedores WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                memComando.CommandText = "DELETE FROM sucursales_proveedores WHERE id_cliente=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & " AND id=0;"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                memComando.CommandText = "DELETE FROM contactos_proveedores WHERE id_cliente=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & ";"
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Eliminó al Proveedor: " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fiscal").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo tabla_clientes para actualizar
            HttpContext.Current.Session("f_" & tabla_empresa.Rows(0)("Id") & "_tabla_proveedores") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].proveedores;")

            'Cargar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Proveedor eliminado correctamente.');", True)

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
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_codigo').focus();$('#txt_codigo').select();}, 100);$('#modal_agregar').modal('show');error('El Código debe ser numérico.')"), True)
                Exit Sub
            End If

            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[proveedores] WHERE id=" & txt_cliente_modal.Text & ";")
            If tabla_consulta.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente_modal').focus();$('#txt_cliente_modal').select();}, 100);$('#modal_agregar').modal('show');error('El Código esta asignado al proveedor: " & tabla_consulta.Rows(0)("nombre_fiscal") & ".')"), True)
                Exit Sub
            End If

            If txt_nif.Text <> "" Then

                'Compruebo si es un CIF o NIF
                Dim esCIF As Boolean = funciones_globales.Verificar_CIF(txt_nif.Text)
                Dim esNIF As Boolean = funciones_globales.Verificar_NIF(txt_nif.Text)

                If esCIF = False And esNIF = False Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);$('#modal_agregar').modal('show');error('El NIF no es válido.')"), True)
                    Exit Sub
                End If

            Else
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);$('#modal_agregar').modal('show');error('El NIF no puede estar vacío.')"), True)
                Exit Sub
            End If

            tabla_consulta = funciones_globales.obtener_datos("SELECT TOP (1) * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[proveedores] WHERE nif='" & txt_nif.Text & "';")
            If tabla_consulta.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);$('#modal_agregar').modal('show');error('El NIF esta asignado al proveedor: " & tabla_consulta.Rows(0)("nombre_fiscal") & ".')"), True)
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
                memComando.CommandText = "INSERT INTO proveedores (id,fecha_creacion,hora_creacion,activo,nombre_fiscal,nombre_comercial,nif,tipo_via,calle,numero,escalera,piso,puerta,cp,pais,provincia,localidad,observaciones) VALUES " &
                            "(@id,@fecha_creacion,@hora_creacion,@activo,@nombre_fiscal,@nombre_comercial,@nif,@tipo_via,@calle,@numero,@escalera,@piso,@puerta,@cp,@pais,@provincia,@localidad,@observaciones);"
                memComando.Parameters.Add("@Id", Data.SqlDbType.Int).Value = txt_cliente_modal.Text
                memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
                memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@activo", System.Data.SqlDbType.Bit).Value = CB_activo.Checked
                memComando.Parameters.Add("@nombre_fiscal", Data.SqlDbType.VarChar, 250).Value = txt_nombre_fiscal.Text.ToUpper
                memComando.Parameters.Add("@nombre_comercial", Data.SqlDbType.VarChar, 250).Value = txt_nombre_comercial.Text.ToUpper
                memComando.Parameters.Add("@nif", Data.SqlDbType.VarChar, 15).Value = txt_nif.Text.ToUpper
                memComando.Parameters.Add("@tipo_via", Data.SqlDbType.VarChar, 50).Value = DDL_tipo_via.SelectedValue
                memComando.Parameters.Add("@calle", Data.SqlDbType.VarChar, 70).Value = txt_domicilio.Text.ToUpper
                memComando.Parameters.Add("@numero", Data.SqlDbType.VarChar, 5).Value = txt_numero.Text.ToUpper
                memComando.Parameters.Add("@escalera", Data.SqlDbType.VarChar, 2).Value = txt_escalera.Text.ToUpper
                memComando.Parameters.Add("@piso", Data.SqlDbType.VarChar, 3).Value = txt_piso.Text.ToUpper
                memComando.Parameters.Add("@puerta", Data.SqlDbType.VarChar, 3).Value = txt_puerta.Text.ToUpper
                memComando.Parameters.Add("@cp", Data.SqlDbType.VarChar, 6).Value = txt_cp_enable.Text.ToUpper
                memComando.Parameters.Add("@pais", Data.SqlDbType.Int).Value = DDL_pais.SelectedValue
                memComando.Parameters.Add("@provincia", Data.SqlDbType.Int).Value = DDL_provincia.SelectedValue
                If DDL_localidad.SelectedValue = "" Then
                    memComando.Parameters.Add("@localidad", Data.SqlDbType.Int).Value = 0
                Else
                    memComando.Parameters.Add("@localidad", Data.SqlDbType.Int).Value = DDL_localidad.SelectedValue
                End If
                memComando.Parameters.Add("@observaciones", Data.SqlDbType.VarChar, 1000).Value = __txt_observaciones.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Agrego Sucursal 0
                memComando.CommandText = "INSERT INTO sucursales_proveedores (id,id_proveedor) VALUES " &
                            "(0,@id);"
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Creó el Proveedor: " & txt_nombre_fiscal.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo tabla_clientes para actualizar
            HttpContext.Current.Session("f_" & tabla_empresa.Rows(0)("Id") & "_tabla_proveedores") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].proveedores;")

            'Leer Tipos
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Proveedor creado correctamente.');", True)

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

            'Excepcion
            If txt_nif.Text <> "" Then

                'Compruebo si es un CIF o NIF
                Dim esCIF As Boolean = funciones_globales.Verificar_CIF(txt_nif.Text)
                Dim esNIF As Boolean = funciones_globales.Verificar_NIF(txt_nif.Text)

                If esCIF = False And esNIF = False Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);$('#modal_agregar').modal('show');error('El NIF no es válido.')"), True)
                    Exit Sub
                End If

            Else
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);$('#modal_agregar').modal('show');error('El NIF no puede estar vacío.')"), True)
                Exit Sub
            End If

            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[proveedores] WHERE nif='" & txt_nif.Text & "' AND Id<>" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & ";")
            If tabla_consulta.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(Function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);$('#modal_agregar').modal('show');error('El NIF esta asignado al proveedor: " & tabla_consulta.Rows(0)("nombre_fiscal") & ".')"), True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE proveedores SET " &
                "activo=@activo, " &
                "nombre_fiscal=@nombre_fiscal, " &
                "nombre_comercial=@nombre_comercial, " &
                "nif=@nif, " &
                "tipo_via=@tipo_via, " &
                "calle=@calle, " &
                "numero=@numero, " &
                "escalera=@escalera, " &
                "piso=@piso, " &
                "puerta=@puerta, " &
                "cp=@cp, " &
                "pais=@pais, " &
                "provincia=@provincia, " &
                "localidad=@localidad, " &
                "observaciones=@observaciones " &
                "WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & ";"
                memComando.Parameters.Add("@activo", System.Data.SqlDbType.Bit).Value = CB_activo.Checked
                memComando.Parameters.Add("@nombre_fiscal", Data.SqlDbType.VarChar, 250).Value = txt_nombre_fiscal.Text.ToUpper
                memComando.Parameters.Add("@nombre_comercial", Data.SqlDbType.VarChar, 250).Value = txt_nombre_comercial.Text.ToUpper
                memComando.Parameters.Add("@nif", Data.SqlDbType.VarChar, 15).Value = txt_nif.Text.ToUpper
                memComando.Parameters.Add("@tipo_via", Data.SqlDbType.VarChar, 50).Value = DDL_tipo_via.SelectedValue
                memComando.Parameters.Add("@calle", Data.SqlDbType.VarChar, 70).Value = txt_domicilio.Text.ToUpper
                memComando.Parameters.Add("@numero", Data.SqlDbType.VarChar, 5).Value = txt_numero.Text.ToUpper
                memComando.Parameters.Add("@escalera", Data.SqlDbType.VarChar, 2).Value = txt_escalera.Text.ToUpper
                memComando.Parameters.Add("@piso", Data.SqlDbType.VarChar, 3).Value = txt_piso.Text.ToUpper
                memComando.Parameters.Add("@puerta", Data.SqlDbType.VarChar, 3).Value = txt_puerta.Text.ToUpper
                memComando.Parameters.Add("@cp", Data.SqlDbType.VarChar, 6).Value = txt_cp_enable.Text
                memComando.Parameters.Add("@pais", Data.SqlDbType.Int).Value = DDL_pais.SelectedValue
                memComando.Parameters.Add("@provincia", Data.SqlDbType.Int).Value = DDL_provincia.SelectedValue
                If DDL_localidad.SelectedValue = "" Then
                    memComando.Parameters.Add("@localidad", Data.SqlDbType.Int).Value = 0
                Else
                    memComando.Parameters.Add("@localidad", Data.SqlDbType.Int).Value = DDL_localidad.SelectedValue
                End If
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Modificó el Proveedor: " & txt_nombre_fiscal.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo tabla_clientes para actualizar
            HttpContext.Current.Session("f_" & tabla_empresa.Rows(0)("Id") & "_tabla_proveedores") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].proveedores;")

            'Leer Tipos
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Proveedor modificado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_modificar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_modificar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class