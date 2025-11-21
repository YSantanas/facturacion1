Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class clientes_sucursales
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

    Sub leer_contactos(ByVal id_cliente As Integer, ByVal id_sucursal As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM " &
                    tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[contactos] " &
                    "WHERE id_cliente=" & id_cliente & " AND id_sucursal=" & id_sucursal & " ORDER BY nombre;")

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
                If Request.QueryString("id_cliente") <> Nothing Then

                    'Asigno
                    txt_cliente.Text = Request.QueryString("id_cliente")
                    txt_sucursal.Text = Request.QueryString("id_sucursal")

                    Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_clientes")
                    Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_clientes, "id", txt_cliente.Text, "nombre_fiscal")
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
        Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_clientes")

        Try

            'Asigno
            lbl_txt_cliente.Text = ""

            'Excepcion
            If IsNumeric(txt_cliente.Text) Then

                'Asigno
                Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_clientes, "id", txt_cliente.Text, "nombre_fiscal")
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
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente').focus();$('#txt_cliente').select();}, 100);error('El Cliente debe ser un código numérico.')"), True)
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Sucursales", "Solicitó listado de sucursales del cliente: " & txt_cliente.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

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
                    memComando.CommandText = "SELECT sucursales.*,clientes.nombre_fiscal, " &
                    "tipo_vias.nombre As nombre_tipo_via " &
                    "FROM clientes INNER JOIN " &
                    "sucursales ON clientes.id=sucursales.id_cliente LEFT JOIN " &
                    "[kernel_facturacion].[dbo].tipo_vias ON sucursales.tipo_via=tipo_vias.id " &
                    "WHERE sucursales.id<>0 ORDER BY sucursales.id_cliente,sucursales.id;"
                Else
                    If txt_sucursal.Text = "" Then
                        memComando.CommandText = "SELECT sucursales.*,clientes.nombre_fiscal, " &
                        "tipo_vias.nombre As nombre_tipo_via " &
                        "FROM clientes INNER JOIN " &
                        "sucursales ON clientes.id=sucursales.id_cliente LEFT JOIN " &
                        "[kernel_facturacion].[dbo].tipo_vias ON sucursales.tipo_via=tipo_vias.id " &
                        "WHERE sucursales.id_cliente=@id_cliente AND sucursales.id<>0 ORDER BY id_cliente,id;"
                        memComando.Parameters.Add(New SqlParameter("id_cliente", Data.SqlDbType.Int))
                        memComando.Parameters("id_cliente").Value = txt_cliente.Text
                    Else
                        memComando.CommandText = "SELECT sucursales.*,clientes.nombre_fiscal, " &
                        "tipo_vias.nombre As nombre_tipo_via " &
                        "FROM clientes INNER JOIN " &
                        "sucursales ON clientes.id=sucursales.id_cliente LEFT JOIN " &
                        "[kernel_facturacion].[dbo].tipo_vias ON sucursales.tipo_via=tipo_vias.id " &
                        "WHERE sucursales.id_cliente=@id_cliente AND sucursales.id=@id_sucursal AND sucursales.id<>0 ORDER BY id_cliente,id;"
                        memComando.Parameters.Add(New SqlParameter("id_cliente", Data.SqlDbType.Int))
                        memComando.Parameters("id_cliente").Value = txt_cliente.Text
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
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("activo") = False Then
                    'DesAsigno la propiedas  
                    For x = 0 To 3
                        e.Row.Cells(x).CssClass = "gv_rojo"
                    Next
                End If

                'Fecha de imputacion
                e.Row.ToolTip = "Fecha de creación: " & Mid(gridview_consulta.DataKeys(e.Row.RowIndex).Item("fecha_creacion").ToString(), 1, 10) & " - " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("hora_creacion").ToString() & "."

                'Maquillaje
                e.Row.Cells(3).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("nombre_tipo_via").ToString & " " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("calle").ToString & ", " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("numero").ToString

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
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Sucursales", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Sucursales", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            lbl_titulo.Text = "Agregar Sucursal"
            btn_grabar.Visible = True
            btn_modificar.Visible = False
            txt_cliente_modal.Text = Nothing
            lbl_txt_cliente_modal.Text = Nothing
            txt_sucursal_modal.Text = Nothing
            CB_activo.Checked = True
            txt_denominacion.Text = Nothing
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
            Lkb_refrecar_contactos.Visible = False
            abrir_contactos.Visible = False
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

    Private Sub txt_cliente_modal_TextChanged(sender As Object, e As EventArgs) Handles txt_cliente_modal.TextChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_clientes")

        Try

            If IsNumeric(txt_cliente_modal.Text) Then

                'Asigno
                Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_clientes, "id", txt_cliente_modal.Text, "nombre_fiscal")
                If consulta <> "0" Then
                    lbl_txt_cliente_modal.Text = consulta
                    txt_denominacion.Text = consulta
                Else
                    lbl_txt_cliente_modal.Text = Nothing
                    txt_denominacion.Text = Nothing
                End If

                'Recomiendo el numero de sucursal
                Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) id FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[sucursales] WHERE id_cliente=" & txt_cliente_modal.Text & " ORDER BY id DESC")
                If tabla_consulta.Rows.Count = 0 Then
                    txt_sucursal_modal.Text = "1"
                Else
                    txt_sucursal_modal.Text = tabla_consulta.Rows(0)("id") + 1
                End If

            End If

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("setTimeout(function () { $('#txt_sucursal_modal').focus();}, 100); $('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "txt_cliente_modal_TextChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error txt_cliente_modal_TextChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub DDL_provincia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_provincia.SelectedIndexChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            If txt_index.Text <> Nothing Then
                txt_sucursal_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id").ToString()
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
                txt_sucursal_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id").ToString()
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

                'Asignamos
                txt_index.Text = index
                LT_mensaje.Text = "¿Está seguro de eliminar la Sucursal: " & gridview_consulta.DataKeys(index).Item("id").ToString() & " del Cliente: " & gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString() & "?"

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

            If (e.CommandName = "consultar") Then

                'Asigno
                lbl_titulo.Text = "Consultar Sucursal"
                txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("id_cliente").ToString()
                txt_sucursal_modal.Text = gridview_consulta.DataKeys(index).Item("id").ToString()
                lbl_txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                CB_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                txt_denominacion.Text = gridview_consulta.DataKeys(index).Item("denominacion").ToString()
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
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                'leer contactos
                leer_contactos(gridview_consulta.DataKeys(index).Item("id_cliente"), gridview_consulta.DataKeys(index).Item("id"))

                btn_grabar.Visible = False
                btn_modificar.Visible = False
                txt_index.Text = index
                txt_cp_enable.Text = gridview_consulta.DataKeys(index).Item("cp").ToString()

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);$('#txt_sucursal_modal').attr('disabled',true);$('#txt_denominacion').attr('disabled',true);$('#DDL_tipo_via').attr('disabled',true);$('#txt_domicilio').attr('disabled',true);$('#txt_domicilo').attr('disabled',true);$('#txt_numero').attr('disabled',true);$('#txt_escalera').attr('disabled',true);$('#txt_piso').attr('disabled',true);$('#txt_puerta').attr('disabled',true);$('#DDL_provincia').attr('disabled',true);$('#DDL_localidad').attr('disabled',true);$('#DDL_pais').attr('disabled',true);$('#Lb_contactos').attr('disabled',true);$('#__txt_observaciones').attr('disabled',true);$('#DDL_pais').attr('disabled',true);$('#txt_cp').attr('disabled',true);$('#Lb_contactos').attr('disabled',true);$('#Lb_sucursales').attr('disabled',true);$('#__txt_observaciones').attr('disabled',true);$('#CB_activo').attr('disabled',true);setTimeout(function () { $('#txt_denominacion').focus();$('#txt_denominacion').select();}, 100);$('#modal_agregar').modal('show');"), True)

            End If

            If (e.CommandName = "editar") Then

                'Asigno
                lbl_titulo.Text = "Modificar Sucursal"
                txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("id_cliente").ToString()
                txt_sucursal_modal.Text = gridview_consulta.DataKeys(index).Item("id").ToString()
                lbl_txt_cliente_modal.Text = gridview_consulta.DataKeys(index).Item("nombre_fiscal").ToString()
                CB_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                txt_denominacion.Text = gridview_consulta.DataKeys(index).Item("denominacion").ToString()
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
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                'leer contactos
                leer_contactos(gridview_consulta.DataKeys(index).Item("id_cliente"), gridview_consulta.DataKeys(index).Item("id"))

                btn_grabar.Visible = False
                btn_modificar.Visible = True
                txt_index.Text = index
                txt_cp_enable.Text = gridview_consulta.DataKeys(index).Item("cp").ToString()

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);$('#txt_sucursal_modal').attr('disabled',true);setTimeout(function () { $('#txt_denominacion').focus();$('#txt_denominacion').select();}, 100);$('#modal_agregar').modal('show');"), True)

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
            leer_contactos(gridview_consulta.DataKeys(txt_index.Text).Item("id_cliente"), gridview_consulta.DataKeys(txt_index.Text).Item("id"))

            'Asignar
            txt_cliente_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id_cliente")
            txt_sucursal_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);$('#txt_sucursal_modal').attr('disabled',true);setTimeout(function () { $('#Lb_contactos').focus();}, 100);$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Lkb_refrecar_contactos_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Lkb_refrecar_contactos_Click: " & ex.Message.Replace("'", " ") & "');", True)
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

                memComando.CommandText = "DELETE FROM sucursales WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & " AND id_cliente=" & gridview_consulta.DataKeys(txt_index.Text).Item("id_cliente") & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                memComando.CommandText = "DELETE FROM contactos WHERE id_cliente=" & gridview_consulta.DataKeys(txt_index.Text).Item("id_cliente") & " AND id_sucursal=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & ";"
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Sucursales", "Eliminó la Sucursal N: " & gridview_consulta.DataKeys(txt_index.Text).Item("id").ToString() & " del Cliente: " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fiscal").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Cargar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Sucursal eliminada correctamente.');", True)

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
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente_modal').focus();$('#txt_cliente_modal').select();}, 100);$('#modal_agregar').modal('show');error('El Cliente debe ser un código numérico.')"), True)
                Exit Sub
            End If

            If Not IsNumeric(txt_sucursal_modal.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_codigo').focus();$('#txt_codigo').select();}, 100);$('#modal_agregar').modal('show');error('El Código debe ser numérico.')"), True)
                Exit Sub
            End If

            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) id FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[sucursales] WHERE id_cliente=" & txt_cliente_modal.Text & " AND id=" & txt_sucursal_modal.Text & " ORDER BY id DESC")
            If tabla_consulta.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_codigo').focus();$('#txt_codigo').select();}, 100);$('#modal_agregar').modal('show');error('El Código esta asignado a otra sucursal: " & tabla_consulta.Rows(0)("denominacion") & ".')"), True)
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
                memComando.CommandText = "INSERT INTO sucursales (Id,id_cliente,fecha_creacion,hora_creacion,activo,denominacion,tipo_via,calle,numero,escalera,piso,puerta,cp,pais,provincia,localidad,observaciones) VALUES " &
                            "(@Id,@Id_cliente,@fecha_creacion,@hora_creacion,@activo,@denominacion,@tipo_via,@calle,@numero,@escalera,@piso,@puerta,@cp,@pais,@provincia,@localidad,@observaciones);"
                memComando.Parameters.Add("@Id", Data.SqlDbType.Int).Value = txt_sucursal_modal.Text
                memComando.Parameters.Add("@id_cliente", Data.SqlDbType.Int).Value = txt_cliente_modal.Text
                memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
                memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@activo", System.Data.SqlDbType.Bit).Value = CB_activo.Checked
                memComando.Parameters.Add("@denominacion", Data.SqlDbType.VarChar, 250).Value = txt_denominacion.Text.ToUpper
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Sucursales", "Creó la Sucursal Nº: " & txt_sucursal_modal.Text & " del Cliente: " & lbl_txt_cliente_modal.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Leer Tipos
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Sucursal creada correctamente.');", True)

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
                memComando.CommandText = "UPDATE sucursales SET " &
                "activo=@activo, " &
                "denominacion=@denominacion, " &
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
                "WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & " AND id_cliente=" & gridview_consulta.DataKeys(txt_index.Text).Item("id_cliente") & ";"
                memComando.Parameters.Add("@activo", System.Data.SqlDbType.Bit).Value = CB_activo.Checked
                memComando.Parameters.Add("@denominacion", Data.SqlDbType.VarChar, 250).Value = txt_denominacion.Text.ToUpper
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Sucursales", "Modificó la Sucursal: " & txt_sucursal_modal.Text & " del Cliente: " & lbl_txt_cliente_modal.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

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