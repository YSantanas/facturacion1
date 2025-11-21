Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports OfficeOpenXml

Partial Class facturas_repetitivas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub cargar_perfiles()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * " &
                    "FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[perfil_repetitivo] " &
                    "ORDER BY nombre;")

            'Limpiar
            DDL_perfil.Items.Clear()

            'Añado
            DDL_perfil.Items.Add(New ListItem("", ""))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                DDL_perfil.Items.Add(New ListItem(tabla_consulta.Rows(x)("nombre").ToString, tabla_consulta.Rows(x)("nombre").ToString))

            Next

            'Limpio
            lbl_DDL_perfil.Text = Nothing

            'Liberamos
            tabla_consulta.Dispose()

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

            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Fecha inicial para la consulta
                txt_fecha.Text = "01/" & (Date.Now).ToString("MM") & "/" & Year(Date.Now)

                'Cargar_perfiles
                cargar_perfiles()

                'cargar_GV()
                cargar_GV()

                'Asigno
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Asigno", "setTimeout(function () { $('#txt_fecha').focus();}, 100);", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_consultar_Click(sender As Object, e As EventArgs) Handles btn_consultar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

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

                'Declaro
                Dim query As String = Nothing
                Dim filtro As String = Nothing
                Dim orden As String = Nothing
                Dim memComando As New SqlCommand

                'Compongo la SELECT
                query = "SELECT * FROM cabecera_facturas "
                filtro = "WHERE perfil_repetitivo IS NULL AND total>0 "
                orden = "ORDER BY fecha,n_factura;"

                'Si elijo fecha
                If txt_fecha.Text <> "" Then

                    filtro += "AND fecha>=@fecha "
                    memComando.Parameters.Add("@fecha", Data.SqlDbType.Date).Value = txt_fecha.Text

                End If

                'Si elijo codigo cliente
                If txt_cod_cliente.Text <> "" Then

                    filtro = "WHERE cod_cliente=@cod_cliente "
                    memComando.Parameters.Add("@cod_cliente", Data.SqlDbType.Int).Value = txt_cod_cliente.Text

                End If

                'Si elijo cliente
                If txt_cliente.Text <> "" Then

                    filtro += "AND cliente LIKE '%' + @cliente + '%' "
                    memComando.Parameters.Add("@cliente", Data.SqlDbType.VarChar, 250).Value = txt_cliente.Text

                End If

                'Si elijo nif
                If txt_nif.Text <> "" Then

                    filtro += "AND nif=@nif "
                    memComando.Parameters.Add("@nif", Data.SqlDbType.VarChar, 15).Value = txt_nif.Text

                End If

                'Si elijo n_factura
                If txt_n_factura.Text <> "" Then

                    filtro += "AND n_factura=@n_factura "
                    memComando.Parameters.Add("@n_factura", Data.SqlDbType.VarChar, 15).Value = txt_n_factura.Text

                End If

                'Si elijo total
                If txt_total.Text <> "" Then

                    filtro += "AND total>=@total "
                    memComando.Parameters.Add("@total", Data.SqlDbType.Decimal, 18, 2).Value = CDec(txt_total.Text.Replace(".", ","))

                End If

                memComando.CommandText = query & filtro & orden
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

            'Realizo la consulta
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

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "ver_factura") Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "abrir_ventana", "$('#ialtas___',window.parent.document).attr('src',$('#ialtas___',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('altas___','bi bi-columns-gap','1000','600','facturas/altas.aspx|id_factura=" & gridview_consulta.DataKeys(index).Item("n_factura") & "','4');", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridview_consulta.PageIndexChanging

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Actualizar posicion y cosnulta
            gridview_consulta.PageIndex = e.NewPageIndex
            cargar_GV()

        Catch ex As Exception
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
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Facturas", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Facturas", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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

    Protected Sub btn_add_facturas_Click(sender As Object, e As EventArgs) Handles btn_add_facturas.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepcion
            If DDL_perfil.SelectedValue.ToString = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('El Perfil no puede estar vacío, crea uno nuevo para poder crear un grupo de facturas repetitivas.')", True)
                Exit Sub
            End If

            'Comprobar que ha seleccionado algo
            Dim comprobador_ticar As Boolean = False
            For Each row As GridViewRow In gridview_consulta.Rows

                ' Busca el control CheckBox en la celda de la fila
                Dim chkSeleccionar As CheckBox = TryCast(row.FindControl("chk_marcar"), CheckBox)

                ' Verifica si el CheckBox está seleccionado
                If chkSeleccionar IsNot Nothing AndAlso chkSeleccionar.Checked Then

                    'Asigno
                    comprobador_ticar = True

                End If

            Next

            If comprobador_ticar = False Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('No has seleccionado ninguna factura para añadirla.')", True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Comienzo
                For Each row As GridViewRow In gridview_consulta.Rows

                    ' Busca el control CheckBox en la celda de la fila
                    Dim chkSeleccionar As CheckBox = TryCast(row.FindControl("chk_marcar"), CheckBox)

                    ' Verifica si el CheckBox está seleccionado
                    If chkSeleccionar IsNot Nothing AndAlso chkSeleccionar.Checked Then

                        'Declaro
                        Dim n_factura As String = gridview_consulta.DataKeys(row.RowIndex).Item("n_factura").ToString

                        'Agrego
                        memComando.CommandText = "UPDATE cabecera_facturas SET perfil_repetitivo='" & DDL_perfil.SelectedValue.ToArray & "',fecha_repetitivo='" & gridview_consulta.DataKeys(row.RowIndex).Item("fecha").ToString & "' " &
                        "WHERE n_factura='" & gridview_consulta.DataKeys(row.RowIndex).Item("n_factura").ToString & "';"
                        memComando.Connection = memConn
                        memComando.ExecuteNonQuery()

                    End If

                Next

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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Repetitivas", "Agregó facturas al Perfil: " & DDL_perfil.SelectedValue.ToString & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'cargar_GV_perfil()
            cargar_GV_perfil(DDL_perfil.SelectedValue.ToString)

            'Cargar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Factura(s) añadidas correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_add_facturas_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_add_facturas_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub add_perfil_Click(sender As Object, e As EventArgs) Handles add_perfil.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_agregar').modal('show');") & "setTimeout(function () { $('#txt_nombre').focus();$('#txt_nombre').select();}, 100);", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "add_perfil_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error add_perfil_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_grabar_perfil_Click(sender As Object, e As EventArgs) Handles btn_grabar_perfil.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepcion
            If txt_nombre.Text = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nombre').focus();$('#txt_nombre').select();}, 100);$('#modal_agregar').modal('show');error('El Nombre no puede estar vacío.')"), True)
                Exit Sub
            End If

            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT TOP (1) * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[perfil_repetitivo] WHERE nombre='" & txt_nombre.Text & "';")
            If tabla_consulta.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nombre').focus();$('#txt_nombre').select();}, 100);$('#modal_agregar').modal('show');error('El Nombre ya está asignado a otro Perfil.')"), True)
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
                memComando.CommandText = "INSERT INTO perfil_repetitivo (fecha_creacion,hora_creacion,nombre,facturacion) VALUES " &
                            "(@fecha_creacion,@hora_creacion,@nombre,@facturacion);"
                memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
                memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@nombre", Data.SqlDbType.VarChar, 100).Value = txt_nombre.Text.ToUpper
                memComando.Parameters.Add("@facturacion", Data.SqlDbType.VarChar, 25).Value = DDL_facturacion.SelectedValue
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Repetitivas", "Creó el Perfil: " & txt_nombre.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Limpio
            txt_nombre.Text = Nothing
            DDL_facturacion.SelectedIndex = 0

            'Cargar_perfiles
            cargar_perfiles()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Perfil creado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_perfil_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_perfil_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub minus_perfil_Click(sender As Object, e As EventArgs) Handles minus_perfil.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If DDL_perfil.SelectedValue.ToString = "" Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#DDL_perfil').focus();$('#DDL_perfil').select();}, 100);error('Debe elegir un Perfil para poderlo eliminar.')"), True)
                Exit Sub

            Else

                'Asigno
                txt_nombre_eliminar.Text = DDL_perfil.SelectedValue.ToString

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');") & "setTimeout(function () { $('#txt_nombre_eliminar').focus();$('#txt_nombre_eliminar').select();}, 100);", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "add_perfil_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error add_perfil_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_eliminar_perfil_Click(sender As Object, e As EventArgs) Handles btn_eliminar_perfil.Click

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

                'Agrego
                memComando.CommandText = "UPDATE cabecera_facturas SET perfil_repetitivo=NULL,fecha_repetitivo=NULL WHERE perfil_repetitivo='" & txt_nombre_eliminar.Text & "';"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Elimino
                memComando.CommandText = "DELETE perfil_repetitivo WHERE nombre='" & txt_nombre_eliminar.Text & "';"
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Repetitivas", "Eliminó el Perfil: " & txt_nombre_eliminar.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Cargar_perfiles
            cargar_perfiles()

            'Cargar_GV_perfil()
            cargar_GV_perfil(txt_nombre_eliminar.Text)

            'Cargar_GV
            cargar_GV()

            'Limpio
            txt_nombre_eliminar.Text = Nothing

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Perfil eliminado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_eliminar_perfil_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_eliminar_perfil_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub DDL_perfil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_perfil.SelectedIndexChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If DDL_perfil.SelectedValue.ToString <> "" Then

                'Consulto
                Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * " &
                    "FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[perfil_repetitivo] " &
                    "WHERE nombre='" & DDL_perfil.SelectedValue.ToString & "';")

                'Asigno
                lbl_DDL_perfil.Text = tabla_consulta.Rows(0)("facturacion").ToString

                'Liberamos
                tabla_consulta.Dispose()

            Else

                'Asigno
                lbl_DDL_perfil.Text = Nothing

            End If

            'cargar_GV_perfil()
            cargar_GV_perfil(DDL_perfil.SelectedValue.ToString)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "DDL_perfil_SelectedIndexChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error DDL_perfil_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub cargar_GV_perfil(ByVal facturacion As String)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If facturacion <> "" Then

                'Asigno
                Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * " &
                        "FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[cabecera_facturas] " &
                        "WHERE perfil_repetitivo='" & DDL_perfil.SelectedValue.ToString & "' ORDER BY n_factura;")

                'Realizo la consulta
                gridview_consulta_perfil.DataSource = tabla_consulta
                gridview_consulta_perfil.DataBind()

            Else

                'Realizo la consulta
                gridview_consulta_perfil.DataSource = Nothing
                gridview_consulta_perfil.DataBind()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "cargar_GV_perfil", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error cargar_GV_perfil: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub gridview_consulta_perfil_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta_perfil.RowCommand

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "ver_factura") Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "abrir_ventana", "$('#ialtas___',window.parent.document).attr('src',$('#ialtas___',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('altas___','bi bi-columns-gap','1000','600','facturas/altas.aspx|id_factura=" & gridview_consulta_perfil.DataKeys(index).Item("n_factura") & "','4');", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_perfil_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_perfil_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub chk_marcar_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("c_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("c_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Capturo de los controles
            Dim checkbox As CheckBox = DirectCast(sender, CheckBox)
            Dim row As GridViewRow = DirectCast(checkbox.NamingContainer, GridViewRow)

            'Asignamos
            txt_index.Text = row.RowIndex
            LT_mensaje_eliminar.Text = "¿Esta seguro de que quiere quitar la factura: " & gridview_consulta_perfil.DataKeys(row.RowIndex).Item("n_factura") & " del Perfil: " & DDL_perfil.SelectedValue.ToString & "? "

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar_factura').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "chk_marcar_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error chk_marcar_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_eliminar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_eliminar_confirmar.Click

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
                memComando.CommandText = "UPDATE cabecera_facturas SET perfil_repetitivo=NULL,fecha_repetitivo=NULL WHERE n_factura='" & gridview_consulta_perfil.DataKeys(txt_index.Text).Item("n_factura").ToString() & "';"
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Repetitivas", "Eliminó la factura: " & gridview_consulta_perfil.DataKeys(txt_index.Text).Item("n_factura").ToString() & " del perfil: " & DDL_perfil.SelectedValue.ToString & ".(Tiempo:  " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Cargar_GV_perfil()
            cargar_GV_perfil(DDL_perfil.SelectedValue.ToString)

            'Cargar_GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Factura eliminada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_eliminar_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_eliminar_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_perfil_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta_perfil.RowDataBound

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                'Maquillaje
                Dim fecha As Date = CDate(gridview_consulta_perfil.DataKeys(e.Row.RowIndex).Item("fecha_repetitivo"))

                'Evaluar
                Select Case lbl_DDL_perfil.Text
                    Case "Mensual" : fecha = fecha.AddMonths(1)
                    Case "Bimensual" : fecha = fecha.AddMonths(2)
                    Case "Trimestral" : fecha = fecha.AddMonths(3)
                    Case "Semestral" : fecha = fecha.AddMonths(6)
                    Case "Anual" : fecha = fecha.AddYears(1)
                End Select

                'Asigno
                e.Row.Cells(8).Text = fecha

                'Fecha de imputacion
                e.Row.ToolTip = "Fecha de creación: " & Mid(gridview_consulta_perfil.DataKeys(e.Row.RowIndex).Item("fecha_creacion").ToString(), 1, 10) & " - " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("hora_creacion").ToString() & "."

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_perfil_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_perfil_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_perfil_DataBound(sender As Object, e As EventArgs) Handles gridview_consulta_perfil.DataBound

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Control para menus de exportar
            If gridview_consulta_perfil.Rows.Count = 0 Then
                btn_facturar.Visible = False
            Else
                btn_facturar.Visible = True
            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_perfil_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_perfil_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_facturar_Click(sender As Object, e As EventArgs) Handles btn_facturar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Lanzo el Script
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ConciliarModalScript", funciones_globales.modal_register("$('#modal_confirmar_facturacion').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_perfil_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_perfil_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_facturar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_facturar_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            ''Cargo sesion
            'Dim tabla As DataTable = Session("c_" & Request.QueryString("id_empresa") & "_tabla_conciliacion")

            'Genero el nombre del fichero
            Dim nombre As String = "Facturacion_" & tabla_usuario.Rows(0)("Id") & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".xlsx"

            'Creo la carpeta para contener el informe temporalmente.
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios_facturacion\Temp\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios_facturacion\Temp\")
            End If

            'Comienzo la generacion del Excel
            Dim pck As ExcelPackage = New ExcelPackage(New FileInfo("D:\imagenes_usuarios_facturacion\Temp\" + nombre))

            'Nombre de la Hoja
            Dim ws = pck.Workbook.Worksheets.Add("Facturar")

            'Recorro los valores
            Dim contador As Integer = 1

            'Recorro
            For x = 0 To gridview_consulta_perfil.Rows.Count - 1

                'Instrucciones
                ws.Cells("A" & contador).Value = gridview_consulta_perfil.DataKeys(x).Item("n_factura").ToString()
                ws.Cells("B" & contador).Value = gridview_consulta_perfil.Rows(x).Cells(8).Text.ToString

                'Aumento contador
                contador += 1

            Next

            'Cerramos el documento.             
            pck.Save()

            'Claudia
            Dim parametros As String = nombre & "|" & DDL_perfil.SelectedValue.ToString
            funciones_globales.grabar_claudia("Facturas Repetitivas", tabla_empresa.Rows(0)("ruta_base_datos"), tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), parametros, 5)

            'Oculto el boton de facturar
            btn_facturar.Visible = False

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Repetitivas", "Realizó la Facturación con el Perfil: " & DDL_perfil.SelectedValue.ToString & ".")

            'Control para aviso al usuario
            Dim mensaje As String = tabla_usuario.Rows(0)("nombre") & ",he dejado la facturación en cola para ir haciéndolo. Puedes continuar trabajando y en cuánto esté listo te avisaré."
            Dim mensaje_final As String = Nothing
            mensaje_final = "advertencia('" & mensaje & "','1000');"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Aviso", mensaje_final, True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_facturar_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_facturar_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
