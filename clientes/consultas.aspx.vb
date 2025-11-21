Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class clientes_consultas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

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

                'Asigno
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Asigno", "setTimeout(function () { $('#txt_cod_cliente').focus();}, 100);", True)

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

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Consultas de Clientes", "Solicitó listado.(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

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
                query = "SELECT clientes.id as id_cliente,clientes.activo,clientes.nombre_fiscal,clientes.nif,clientes.calle as calle_cliente,clientes.numero as numero_cliente, " &
                        "clientes.cp as cp_cliente, clientes.observaciones as observaciones_cliente,sucursales.id as id_sucursal, sucursales.activo as activo_sucursal, " &
                        "sucursales.calle as calle_sucursal,sucursales.numero as numero_sucursal,sucursales.cp as cp_sucursal, sucursales.observaciones as observaciones_sucursal, " &
                        "contactos.id_cliente as id_cliente_contacto, contactos.id_sucursal as id_sucursal_contacto, contactos.nombre,contactos.primer_apellido, contactos.segundo_apellido, " &
                        "contactos.contacto, contactos.observaciones as observaciones_contacto FROM " &
                        "clientes INNER JOIN " &
                        "sucursales ON clientes.id=sucursales.id_cliente LEFT JOIN " &
                        "contactos ON (contactos.id_cliente=sucursales.id_cliente) AND (contactos.id_sucursal=sucursales.id) "

                filtro = "WHERE clientes.id<100000000 "
                orden = "ORDER BY clientes.id,sucursales.id,contactos.nombre;"

                'Si elijo codigo cliente
                If txt_cod_cliente.Text <> "" Then

                    filtro = "WHERE clientes.id=@txt_cod_cliente "
                    memComando.Parameters.Add("@txt_cod_cliente", Data.SqlDbType.Int).Value = txt_cod_cliente.Text

                End If

                'Si elijo Nombre Fiscal o Comercial
                If txt_nombre.Text <> "" Then

                    filtro += "And nombre_fiscal Like '%' + @txt_nombre + '%' "
                    memComando.Parameters.Add("@txt_nombre", Data.SqlDbType.VarChar, 250).Value = txt_nombre.Text

                End If

                'Si elijo Nombre Fiscal o Comercial
                If txt_nif.Text <> "" Then

                    filtro += "AND nif LIKE '%' + @txt_nif + '%' "
                    memComando.Parameters.Add("@txt_nif", Data.SqlDbType.VarChar, 250).Value = txt_nif.Text

                End If

                'Si elijo dirección
                If txt_direccion.Text <> "" Then

                    filtro += "AND (clientes.calle LIKE '%' + @txt_direccion + '%' OR sucursales.calle LIKE '%' + @txt_direccion + '%') "
                    memComando.Parameters.Add("@txt_direccion", Data.SqlDbType.VarChar, 70).Value = txt_direccion.Text

                End If

                'Si elijo CP
                If txt_cp.Text <> "" Then

                    filtro += "AND (clientes.cp LIKE @txt_cp + '%' OR sucursales.cp LIKE @txt_cp + '%') "
                    memComando.Parameters.Add("@txt_cp", Data.SqlDbType.VarChar, 15).Value = txt_cp.Text

                End If

                'Si elijo Sucursales
                If txt_sucursal.Text <> "" Then

                    filtro += "AND sucursales.id = @txt_sucursal "
                    memComando.Parameters.Add("@txt_sucursal", Data.SqlDbType.VarChar, 15).Value = txt_sucursal.Text

                End If

                'Si elijo Contactos
                If txt_contacto.Text <> "" Then

                    filtro += "AND (contactos.nombre LIKE '%' + @txt_contacto + '%' OR contactos.primer_apellido LIKE '%' + @txt_contacto + '%' OR contactos.segundo_apellido LIKE '%' + @txt_contacto + '%') "
                    memComando.Parameters.Add("@txt_contacto", Data.SqlDbType.VarChar, 70).Value = txt_contacto.Text

                End If

                'Si elijo tlf, email etc
                If txt_tlf.Text <> "" Then

                    filtro += "AND (contactos.contacto LIKE '%' + @txt_tlf + '%') "
                    memComando.Parameters.Add("@txt_tlf", Data.SqlDbType.VarChar, 70).Value = txt_tlf.Text

                End If

                'Si elijo Contactos
                If txt_observaciones.Text <> "" Then

                    filtro += "AND (clientes.observaciones LIKE '%' + @txt_observaciones + '%' OR sucursales.observaciones LIKE '%' + @txt_observaciones + '%' OR contactos.observaciones LIKE '%' + @txt_observaciones + '%') "
                    memComando.Parameters.Add("@txt_observaciones", Data.SqlDbType.VarChar, 70).Value = txt_observaciones.Text

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

                'Declaro
                Dim temp_sucursal As Integer = 0
                If Not IsDBNull(gridview_consulta.DataKeys(e.Row.RowIndex).Item("id_sucursal")) Then
                    temp_sucursal = gridview_consulta.DataKeys(e.Row.RowIndex).Item("id_sucursal")
                End If

                'Si la sucursal es 0
                If temp_sucursal = 0 Then

                    'Asigno
                    e.Row.Cells(4).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("calle_cliente").ToString & ", " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("numero_cliente").ToString
                    e.Row.Cells(5).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("cp_cliente").ToString
                    e.Row.Cells(7).Text = "Central"


                Else

                    'Asigno
                    e.Row.Cells(4).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("calle_sucursal").ToString & ", " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("numero_sucursal").ToString
                    e.Row.Cells(5).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("cp_sucursal").ToString
                    e.Row.Cells(7).Text = temp_sucursal

                    'Oculto el boton de Sucursal
                    Dim LK_sucursal As LinkButton = DirectCast(e.Row.FindControl("lk_sucursal"), LinkButton)
                    LK_sucursal.Visible = True

                End If

                'Asigno
                e.Row.Cells(9).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("nombre").ToString & " " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("primer_apellido").ToString & " " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("segundo_apellido").ToString
                e.Row.Cells(10).Text = gridview_consulta.DataKeys(e.Row.RowIndex).Item("contacto").ToString

                'Muestro el boton de Contacto
                Dim LK_contacto As LinkButton = DirectCast(e.Row.FindControl("lk_contacto"), LinkButton)
                LK_contacto.Visible = True

                'Mestro colores de Inactividad
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("activo") = False Then
                    e.Row.Cells(1).CssClass = "gv_rojo"
                    e.Row.Cells(2).CssClass = "gv_rojo"
                End If
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("activo_sucursal").ToString = "False" Then
                    e.Row.Cells(7).CssClass = "gv_rojo gvHeaderCenter"
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

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "ver_cliente") Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "abrir_ventana", "$('#ialtas',window.parent.document).attr('src',$('#ialtas',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('altas','bi-person-lines-fill','1000','600','clientes/altas.aspx|id_cliente=" & gridview_consulta.DataKeys(index).Item("id_cliente") & "','1');", True)

            End If

            If (e.CommandName = "ver_sucursal") Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "abrir_ventana", "$('#isucursales',window.parent.document).attr('src',$('#isucursales',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('sucursales','bi-file-earmark-break','1000','600','clientes/sucursales.aspx|id_cliente=" & gridview_consulta.DataKeys(index).Item("id_cliente") & "&id_sucursal=" & gridview_consulta.DataKeys(index).Item("id_sucursal") & "','1');", True)

            End If

            If (e.CommandName = "ver_contacto") Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "abrir_ventana", "$('#icontactos',window.parent.document).attr('src',$('#icontactos',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('contactos','bi-columns','1000','600','clientes/contactos.aspx|id_cliente=" & gridview_consulta.DataKeys(index).Item("id_cliente") & "&id_sucursal=" & gridview_consulta.DataKeys(index).Item("id_sucursal") & "','1');", True)

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
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Clientes", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Clientes", gridview_consulta, tabla_usuario.Rows(0)("Id"))

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

End Class