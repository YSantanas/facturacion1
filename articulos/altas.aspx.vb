Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class articulos_altas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_sucursales(ByVal id_cliente As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT sucursales.*,clientes.nombre_fiscal, " &
                    "tipo_vias.nombre As nombre_tipo_via " &
                    "FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[clientes] INNER JOIN " &
                    tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[sucursales] ON clientes.id=sucursales.id_cliente LEFT JOIN " &
                    "[kernel_facturacion].[dbo].tipo_vias ON sucursales.tipo_via=tipo_vias.id " &
                    "WHERE sucursales.id_cliente=" & id_cliente & " AND sucursales.id<>0 ORDER BY id;")

            ''Asigno
            'Lb_sucursales.Items.Clear()

            ''Recorro
            'For x = 0 To tabla_consulta.Rows.Count - 1

            '    'Cargo los detalles
            '    Dim lista As New System.Web.UI.WebControls.ListItem("Sucursal: " & tabla_consulta.Rows(x)("Id").ToString & " - " & tabla_consulta.Rows(x)("nombre_tipo_via").ToString & " " & tabla_consulta.Rows(x)("calle").ToString & ", N:" & tabla_consulta.Rows(x)("numero").ToString, tabla_consulta.Rows(x)("Id").ToString)
            '    Me.Lb_sucursales.Items.Add(lista)

            'Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipo_vias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipo_vias: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_tipos_impuestos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_tipo_impuestos As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_tipo_impuestos")

        Try

            'Limpiar
            DDL_tipo_impuestos.Items.Clear()

            'Recorro
            For x = 0 To tabla_tipo_impuestos.Rows.Count - 1

                If tabla_tipo_impuestos.Rows(x)("activo") = True Then

                    'Cargo los detalles
                    DDL_tipo_impuestos.Items.Add(New ListItem(tabla_tipo_impuestos.Rows(x)("porcentaje").ToString, tabla_tipo_impuestos.Rows(x)("porcentaje").ToString))

                End If

            Next

            'Liberamos
            tabla_tipo_impuestos.Dispose()

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

            'Asigno el n_consulta
            gridview_consulta.PageSize = tabla_empresa.Rows(0)("n_paginado_consultas")

            'Ver fondo
            If Not IsPostBack Then

                'leer_tipos_impuestos
                leer_tipos_impuestos()

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                '            'Restricción de Usuarios
                '            If parametros_usuario(10) = "Invitado" Then
                '                'Bloque Jquery
                '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                '            End If

                'Si viene con parametros
                If Request.QueryString("id_articulo") <> Nothing Then

                    'Asigno
                    txt_articulo.Text = Request.QueryString("id_articulo")

                    Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_articulos")
                    Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_articulos, "id", txt_articulo.Text, "denominacion")
                    lbl_txt_articulo.Text = consulta

                End If

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#btn_consultar').click();setTimeout(function () { $('#txt_articulo').focus();$('#txt_articulo').select();}, 100);", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub txt_articulo_TextChanged(sender As Object, e As EventArgs) Handles txt_articulo.TextChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_articulos")

        Try

            'Asigno
            lbl_txt_articulo.Text = ""

            'Excepcion
            If IsNumeric(txt_articulo.Text) Then

                'Asigno
                Dim consulta As String = funciones_globales.buscar_datos_tabla(tabla_articulos, "id", txt_articulo.Text, "denominacion")
                If consulta <> "0" Then
                    lbl_txt_articulo.Text = consulta
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
            If txt_articulo.Text <> "" Then
                If Not IsNumeric(txt_articulo.Text) Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_articulo').focus();$('#txt_articulo').select();}, 100);error('El Artículo debe ser un código numérico.')"), True)
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Solicitó listado del artículo: " & txt_articulo.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

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
                If txt_articulo.Text = "" Then
                    memComando.CommandText = "SELECT * FROM articulos ORDER BY Id;"
                Else
                    memComando.CommandText = "SELECT * FROM articulos WHERE Id = @id ORDER BY id;"
                    memComando.Parameters.Add(New SqlParameter("id", Data.SqlDbType.Int))
                    memComando.Parameters("id").Value = txt_articulo.Text
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
                    For x = 0 To 1
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
            Dim nombre As String = funciones_globales.crear_excel("Alta de Artículos", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            Dim nombre As String = funciones_globales.crear_txt("Alta de Artículos", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            lbl_titulo.Text = "Agregar Artículo"
            btn_grabar.Visible = True
            btn_modificar.Visible = False
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP(1) id FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[articulos] ORDER BY id DESC")
            If tabla_consulta.Rows.Count = 0 Then
                txt_articulo_modal.Text = "1"
            Else
                txt_articulo_modal.Text = tabla_consulta.Rows(0)("id") + 1
            End If
            CB_activo.Checked = True
            txt_denominacion.Text = Nothing
            txt_cod_barras.Text = Nothing
            Lb_familia.Items.Clear()
            Lkb_refrecar_familia.Visible = False
            abrir_familia.Visible = False
            txt_precio.Text = Nothing
            txt_dto_1.Text = 0
            __txt_observaciones.Text = Nothing
            'Oculto
            txt_index.Text = Nothing

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("setTimeout(function () { $('#txt_articulo_modal').focus();}, 100); $('#modal_agregar').modal('show');"), True)

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

                'Restricción si existen lineas de facturas con ese articulo
                Dim tabla As DataTable = funciones_globales.obtener_datos("SELECT TOP(1) * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].detalles_facturas WHERE cod_articulo=" & gridview_consulta.DataKeys(index).Item("id").ToString().Replace(",", ".") & ";")
                If tabla.Rows.Count <> 0 Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('Existen Facturas emitidas con este artículo y no se puede eliminar.')", True)
                    Exit Sub
                End If

                'Asignamos
                txt_index.Text = index
                LT_mensaje.Text = "¿Está seguro de eliminar el Artículo: " & gridview_consulta.DataKeys(index).Item("denominacion").ToString() & ") ?"

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

            If (e.CommandName = "consultar") Then

                'Asigno
                lbl_titulo.Text = "Consultar Artículo"
                txt_articulo_modal.Text = gridview_consulta.DataKeys(index).Item("id").ToString()
                lbl_txt_codigo.Text = gridview_consulta.DataKeys(index).Item("denominacion").ToString()
                CB_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                txt_denominacion.Text = gridview_consulta.DataKeys(index).Item("denominacion").ToString()
                txt_cod_barras.Text = gridview_consulta.DataKeys(index).Item("cod_barras").ToString()
                txt_precio.Text = gridview_consulta.DataKeys(index).Item("precio").ToString()
                txt_dto_1.Text = gridview_consulta.DataKeys(index).Item("dto_1").ToString()
                DDL_tipo_impuestos.SelectedIndex = DDL_tipo_impuestos.Items.IndexOf(DDL_tipo_impuestos.Items.FindByValue(gridview_consulta.DataKeys(index).Item("impuesto").ToString()))
                Lkb_refrecar_familia.Visible = True
                abrir_familia.Visible = True
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                'leer familia
                'leer_sucursales(gridview_consulta.DataKeys(index).Item("id"))

                btn_grabar.Visible = False
                btn_modificar.Visible = False
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_articulo_modal').attr('disabled',true);$('#CB_activo').attr('disabled',true);$('#txt_denominacion').attr('disabled',true);$('#txt_cod_barras').attr('disabled',true);$('#Lb_familia').attr('disabled',true);$('#txt_precio').attr('disabled',true);$('#txt_dto_1').attr('disabled',true);$('#DDL_tipo_impuestos').attr('disabled',true);$('#__txt_observaciones').attr('disabled',true);setTimeout(function () { $('#txt_denominacion').focus();$('#txt_denominacion').select();}, 100);$('#modal_agregar').modal('show');"), True)

            End If

            If (e.CommandName = "editar") Then

                'Asigno
                lbl_titulo.Text = "Modificar Artículo"
                txt_articulo_modal.Text = gridview_consulta.DataKeys(index).Item("id").ToString()
                lbl_txt_codigo.Text = gridview_consulta.DataKeys(index).Item("denominacion").ToString()
                CB_activo.Checked = gridview_consulta.DataKeys(index).Item("activo")
                txt_denominacion.Text = gridview_consulta.DataKeys(index).Item("denominacion").ToString()
                txt_cod_barras.Text = gridview_consulta.DataKeys(index).Item("cod_barras").ToString()
                txt_precio.Text = gridview_consulta.DataKeys(index).Item("precio").ToString()
                txt_dto_1.Text = gridview_consulta.DataKeys(index).Item("dto_1").ToString()
                DDL_tipo_impuestos.SelectedIndex = DDL_tipo_impuestos.Items.IndexOf(DDL_tipo_impuestos.Items.FindByValue(gridview_consulta.DataKeys(index).Item("impuesto").ToString()))
                Lkb_refrecar_familia.Visible = True
                abrir_familia.Visible = True
                __txt_observaciones.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString
                'leer familia
                'leer_sucursales(gridview_consulta.DataKeys(index).Item("id"))

                btn_grabar.Visible = False
                btn_modificar.Visible = True
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_articulo_modal').attr('disabled',true);setTimeout(function () { $('#txt_denominacion').focus();$('#txt_denominacion').select();}, 100);$('#modal_agregar').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub Lkb_refrecar_familia_Click(sender As Object, e As EventArgs) Handles Lkb_refrecar_familia.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'leer sucursales
            leer_sucursales(gridview_consulta.DataKeys(txt_index.Text).Item("id"))

            'Asignar
            'txt_cliente_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_cliente_modal').attr('disabled',true);setTimeout(function () { $('#Lb_sucursales').focus();}, 100);$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Lkb_refrecar_sucursales_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Lkb_refrecar_familia_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub Lkb_refrecar_tipo_impuestos_Click(sender As Object, e As EventArgs) Handles Lkb_refrecar_tipo_impuestos.Click


        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'leer tipos de impuestos
            leer_tipos_impuestos()

            'Asignar
            DDL_tipo_impuestos.SelectedIndex = DDL_tipo_impuestos.Items.IndexOf(DDL_tipo_impuestos.Items.FindByValue(gridview_consulta.DataKeys(txt_index.Text).Item("impuesto").ToString()))
            txt_articulo_modal.Text = gridview_consulta.DataKeys(txt_index.Text).Item("id").ToString()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#txt_articulo_modal').attr('disabled',true);setTimeout(function () { $('#DDL_tipo_impuestos').focus();}, 100);$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Lkb_refrecar_tipo_impuestos_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Lkb_refrecar_tipo_impuestos_Click: " & ex.Message.Replace("'", " ") & "');", True)
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

                memComando.CommandText = "DELETE FROM articulos WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Limpiar
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Eliminó el Artículo: " & gridview_consulta.DataKeys(txt_index.Text).Item("denominacion").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo tabla_articulos para actualizar
            HttpContext.Current.Session("f_" & tabla_empresa.Rows(0)("Id") & "_tabla_articulos") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].articulos;")

            'Cargar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Artículo eliminado correctamente.');", True)

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
            If Not IsNumeric(txt_articulo_modal.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_articulo_modal').focus();$('#txt_articulo_modal').select();}, 100);$('#modal_agregar').modal('show');error('El Artículo debe ser numérico.')"), True)
                Exit Sub
            End If

            If txt_articulo_modal.Text = "0" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_articulo_modal').focus();$('#txt_articulo_modal').select();}, 100);$('#modal_agregar').modal('show');error('El Artículo con código 0 está restringido para uso interno.')"), True)
                Exit Sub
            End If

            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[articulos] WHERE id=" & txt_articulo_modal.Text & ";")
            If tabla_consulta.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_articulo_modal').focus();$('#txt_articulo_modal').select();}, 100);$('#modal_agregar').modal('show');error('El Código esta asignado al artículo: " & tabla_consulta.Rows(0)("denominacion") & ".')"), True)
                Exit Sub
            End If

            If Not IsNumeric(txt_precio.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_precio').focus();$('#txt_precio').select();}, 100);$('#modal_agregar').modal('show');error('El Precio debe ser numérico.')"), True)
                Exit Sub
            End If

            If Not IsNumeric(txt_dto_1.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_dto_1').focus();$('#txt_dto_1').select();}, 100);$('#modal_agregar').modal('show');error('El Descuento debe ser numérico.')"), True)
                Exit Sub
            Else
                If CDec(txt_dto_1.Text.Replace(".", ",")) < 0 Or CDec(txt_dto_1.Text.Replace(".", ",")) > 100 Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_dto_1').focus();$('#txt_dto_1').select();}, 100);$('#modal_agregar').modal('show');error('El Descuento tiene que estar comprendido entre 0-100.')"), True)
                    Exit Sub
                End If
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Agrego
                memComando.CommandText = "INSERT INTO articulos (id,fecha_creacion,hora_creacion,activo,denominacion,cod_barras,precio,dto_1,impuesto,observaciones) VALUES " &
                            "(@id,@fecha_creacion,@hora_creacion,@activo,@denominacion,@cod_barras,@precio,@dto_1,@impuesto,@observaciones);"
                memComando.Parameters.Add("@Id", Data.SqlDbType.Int).Value = txt_articulo_modal.Text
                memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
                memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@activo", System.Data.SqlDbType.Bit).Value = CB_activo.Checked
                memComando.Parameters.Add("@denominacion", Data.SqlDbType.VarChar, 250).Value = txt_denominacion.Text.ToUpper
                memComando.Parameters.Add("@cod_barras", Data.SqlDbType.VarChar, 50).Value = txt_cod_barras.Text.ToUpper
                memComando.Parameters.Add("@precio", Data.SqlDbType.Decimal, 9, 2).Value = txt_precio.Text.Replace(".", ",")
                memComando.Parameters.Add("@dto_1", Data.SqlDbType.Decimal, 9, 2).Value = txt_dto_1.Text.Replace(".", ",")
                memComando.Parameters.Add("@impuesto", Data.SqlDbType.Decimal, 5, 2).Value = DDL_tipo_impuestos.SelectedValue.ToString.Replace(".", ",")
                memComando.Parameters.Add("@observaciones", Data.SqlDbType.VarChar, 1000).Value = __txt_observaciones.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                ''Agrego Sucursal 0
                'memComando.CommandText = "INSERT INTO sucursales (id,id_cliente) VALUES " &
                '            "(0,@id);"
                'memComando.ExecuteNonQuery()

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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Creó el Artículo: " & txt_denominacion.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo tabla_articulo para actualizar
            HttpContext.Current.Session("f_" & tabla_empresa.Rows(0)("Id") & "_tabla_articulos") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].articulos;")

            'Leer Tipos
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Artículo creado correctamente.');", True)

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
            If Not IsNumeric(txt_precio.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_precio').focus();$('#txt_precio').select();}, 100);$('#modal_agregar').modal('show');error('El Precio debe ser numérico.')"), True)
                Exit Sub
            End If

            If Not IsNumeric(txt_dto_1.Text) Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_dto_1').focus();$('#txt_dto_1').select();}, 100);$('#modal_agregar').modal('show');error('El Descuento debe ser numérico.')"), True)
                Exit Sub
            Else
                If CDec(txt_dto_1.Text.Replace(".", ",")) < 0 Or CDec(txt_dto_1.Text.Replace(".", ",")) > 100 Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_dto_1').focus();$('#txt_dto_1').select();}, 100);$('#modal_agregar').modal('show');error('El Descuento tiene que estar comprendido entre 0-100.')"), True)
                    Exit Sub
                End If
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE articulos SET " &
                "activo=@activo, " &
                "denominacion=@denominacion, " &
                "cod_barras=@cod_barras, " &
                "precio=@precio, " &
                "dto_1=@dto_1, " &
                "impuesto=@impuesto, " &
                "observaciones=@observaciones " &
                "WHERE id=" & gridview_consulta.DataKeys(txt_index.Text).Item("id") & ";"
                memComando.Parameters.Add("@activo", System.Data.SqlDbType.Bit).Value = CB_activo.Checked
                memComando.Parameters.Add("@denominacion", Data.SqlDbType.VarChar, 250).Value = txt_denominacion.Text.ToUpper
                memComando.Parameters.Add("@cod_barras", Data.SqlDbType.VarChar, 50).Value = txt_cod_barras.Text
                memComando.Parameters.Add("@precio", Data.SqlDbType.Decimal, 9, 2).Value = txt_precio.Text.Replace(".", ",")
                memComando.Parameters.Add("@dto_1", Data.SqlDbType.Decimal, 9, 2).Value = txt_dto_1.Text.Replace(".", ",")
                memComando.Parameters.Add("@impuesto", Data.SqlDbType.Decimal, 5, 2).Value = DDL_tipo_impuestos.SelectedValue.ToString.Replace(".", ",")
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Modificó el Artículo: " & txt_denominacion.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Obtengo tabla_articulos para actualizar
            HttpContext.Current.Session("f_" & tabla_empresa.Rows(0)("Id") & "_tabla_articulos") = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].articulos;")

            'Leer Tipos
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Artículo modificado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_modificar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_modificar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
