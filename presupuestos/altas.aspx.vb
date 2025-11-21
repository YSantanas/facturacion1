Imports System.Data
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser
Imports System.Drawing
Imports iTextSharp.text

Partial Class presupuestos_altas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Function LlenarDataTableDesdeGridView(ByVal gv As GridView) As DataTable

        'Declaro
        Dim tabla_consulta As New DataTable

        'Añado
        tabla_consulta.Columns.Add("fecha_creacion")
        tabla_consulta.Columns.Add("hora_creacion")
        tabla_consulta.Columns.Add("linea")
        tabla_consulta.Columns.Add("cantidad")
        tabla_consulta.Columns.Add("codigo")
        tabla_consulta.Columns.Add("articulo")
        tabla_consulta.Columns.Add("precio")
        tabla_consulta.Columns.Add("dto")
        tabla_consulta.Columns.Add("impuesto")
        tabla_consulta.Columns.Add("total")
        tabla_consulta.Columns.Add("observaciones")
        tabla_consulta.Columns.Add("imagen")

        'Añado las líneas
        For Each row As GridViewRow In gv.Rows

            'Relleno
            Dim Renglon As DataRow = tabla_consulta.NewRow()
            Renglon("fecha_creacion") = gv.DataKeys(row.RowIndex).Item("fecha_creacion")
            Renglon("hora_creacion") = gv.DataKeys(row.RowIndex).Item("hora_creacion")
            Renglon("linea") = gv.DataKeys(row.RowIndex).Item("linea")
            Renglon("cantidad") = gv.DataKeys(row.RowIndex).Item("cantidad")
            Renglon("codigo") = gv.DataKeys(row.RowIndex).Item("codigo")
            Renglon("articulo") = gv.DataKeys(row.RowIndex).Item("articulo")
            Renglon("precio") = gv.DataKeys(row.RowIndex).Item("precio")
            Renglon("dto") = gv.DataKeys(row.RowIndex).Item("dto")
            Renglon("impuesto") = gv.DataKeys(row.RowIndex).Item("impuesto")
            Renglon("total") = gv.DataKeys(row.RowIndex).Item("total")
            Renglon("observaciones") = gv.DataKeys(row.RowIndex).Item("observaciones")
            Renglon("imagen") = gv.DataKeys(row.RowIndex).Item("imagen")
            tabla_consulta.Rows.Add(Renglon)

        Next

        'Devuelvo
        Return tabla_consulta

    End Function

    Sub refrescar_totales()

        'Declaro
        Dim total_factura As Decimal = 0
        Dim porcentaje_anterior As String = Nothing
        Dim vporcentaje As New List(Of String)

        'Creo un Datatable
        Dim tabla_temp As New DataTable
        Dim tabla_totales_facturas As New DataTable

        'Añado
        tabla_temp.Columns.Add("base_imponible", GetType(Decimal))
        tabla_temp.Columns.Add("porcentaje", GetType(Decimal))
        tabla_temp.Columns.Add("cuota", GetType(Decimal))
        tabla_temp.Columns.Add("total", GetType(Decimal))
        tabla_totales_facturas.Columns.Add("base_imponible")
        tabla_totales_facturas.Columns.Add("porcentaje")
        tabla_totales_facturas.Columns.Add("cuota")
        tabla_totales_facturas.Columns.Add("total")

        'Recorro GV
        For x = 0 To GV_detalles_factura.Rows.Count - 1

            'Declaro
            Dim base_imponible As Decimal = Math.Round(CDec(GV_detalles_factura.DataKeys(x).Item("cantidad").ToString.Replace(".", ",")) * CDec(GV_detalles_factura.DataKeys(x).Item("precio").ToString.Replace(".", ",")), 2)

            'Si viene con descuento
            If CDec(GV_detalles_factura.DataKeys(x).Item("dto")) <> 0 Then
                'Asigno
                base_imponible = Math.Round(base_imponible - (base_imponible * CDec(GV_detalles_factura.DataKeys(x).Item("dto"))) / 100, 2)
            End If

            'Añado al vector de %
            vporcentaje.Add(GV_detalles_factura.DataKeys(x).Item("impuesto"))

            Dim total As Decimal = Math.Round(base_imponible + (base_imponible * CDec(GV_detalles_factura.DataKeys(x).Item("impuesto")) / 100), 2)

            'Inserto
            Dim Renglon As DataRow = tabla_temp.NewRow()
            Renglon("base_imponible") = base_imponible
            Renglon("porcentaje") = CDec(GV_detalles_factura.DataKeys(x).Item("impuesto"))
            Renglon("cuota") = total - base_imponible
            Renglon("total") = total
            tabla_temp.Rows.Add(Renglon)

            'Asigno
            total_factura += total

        Next

        'Quito los duplicados
        Dim conjunto As New HashSet(Of String)(vporcentaje)
        Dim vectorSinDuplicados As String() = conjunto.ToArray()

        'Recorro
        For x = 0 To vectorSinDuplicados.Count - 1

            'Inserto
            Dim Renglon As DataRow = tabla_totales_facturas.NewRow()
            Renglon("base_imponible") = tabla_temp.Compute("SUM(base_imponible)", "porcentaje='" & vectorSinDuplicados(x) & "'")
            Renglon("porcentaje") = vectorSinDuplicados(x)
            Renglon("cuota") = tabla_temp.Compute("SUM(cuota)", "porcentaje='" & vectorSinDuplicados(x) & "'")
            Renglon("total") = tabla_temp.Compute("SUM(total)", "porcentaje='" & vectorSinDuplicados(x) & "'")
            tabla_totales_facturas.Rows.Add(Renglon)

        Next

        'Asigno
        lbl_total.Text = FormatNumber(total_factura, 2) & " €"
        GV_totales.DataSource = tabla_totales_facturas
        GV_totales.DataBind()

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

            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")
                txt_fecha.Text = DateTime.Now.ToShortDateString

                'Si viene con parametros
                If Request.QueryString("id_presupuesto") <> Nothing Then

                    'Llamar al Presupuesto
                    consultar_factura(Request.QueryString("id_presupuesto"))

                Else

                    'Asigno
                    txt_cod_cliente.Focus()

                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub consultar_factura(ByVal n_factura As String)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Obtengo la Cabecera
        Dim tabla_cabecera As DataTable = funciones_globales.obtener_datos("SELECT * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[cabecera_presupuestos] WHERE n_presupuesto='" & n_factura & "';")

        'Asigno
        txt_cod_cliente.Text = tabla_cabecera.Rows(0)("cod_cliente").ToString
        txt_cliente.Text = tabla_cabecera.Rows(0)("cliente").ToString
        txt_nif.Text = tabla_cabecera.Rows(0)("nif").ToString
        txt_direccion.Text = tabla_cabecera.Rows(0)("direccion").ToString
        txt_cp.Text = tabla_cabecera.Rows(0)("cp").ToString
        txt_poblacion.Text = tabla_cabecera.Rows(0)("poblacion").ToString
        txt_provincia.Text = tabla_cabecera.Rows(0)("provincia").ToString
        txt_presupuesto.Text = tabla_cabecera.Rows(0)("n_presupuesto").ToString

        'Obtengo los Detalles
        Dim tabla_detalles As DataTable = funciones_globales.obtener_datos("SELECT fecha_creacion,hora_creacion,linea,cantidad,cod_articulo AS codigo,denominacion AS articulo,precio,dto_1 AS dto,porcentaje_impuesto AS impuesto,total,observaciones,imagen FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[detalles_presupuestos] WHERE n_presupuesto='" & n_factura & "' ORDER BY linea;")

        'Asigno
        GV_detalles_factura.DataSource = tabla_detalles
        GV_detalles_factura.DataBind()

        'Asigno
        LB_imprimir.Visible = True
        LB_email.Visible = True
        btn_grabar.Visible = False
        btn_actualizar.Visible = False
        PL_cabecera.Enabled = False
        PL_detalles.Enabled = True
        PL_total.Enabled = False

    End Sub

    Protected Sub btn_grabar_detalle_Click(sender As Object, e As EventArgs) Handles btn_grabar_detalle.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_articulos")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now
            Dim codigo_articulo As Integer = 0

            'Excepcion
            If txt_codigo.Text <> "" Then

                If funciones_globales.buscar_datos_tabla(tabla_articulos, "id", txt_codigo.Text, "denominacion") = "0" Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_codigo').focus();$('#txt_codigo').select();}, 100);error('El Código de artículo no existe.')", True)
                    Exit Sub
                End If

                'Asigno
                codigo_articulo = txt_codigo.Text

            End If

            'Creo un Datatable
            Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

            'Inserto
            Dim Renglon As DataRow = tabla_detalles_facturas.NewRow()
            Renglon("fecha_creacion") = DateTime.Today
            Renglon("hora_creacion") = Now.ToString("HH:mm:ss")
            If txt_index.Text <> "" Then
                Renglon("linea") = txt_index.Text
            Else
                Renglon("linea") = tabla_detalles_facturas.Rows.Count
            End If
            Renglon("cantidad") = txt_cantidad.Text.Replace(".", ",")
            Renglon("codigo") = codigo_articulo
            Renglon("articulo") = txt_denominacion.Text
            Renglon("precio") = txt_precio.Text.Replace(".", ",")
            Renglon("dto") = txt_descuento.Text.Replace(".", ",")
            Renglon("impuesto") = txt_impuestos.Text.Replace(".", ",")
            Dim cantidad_precio As Decimal = CDec(txt_cantidad.Text.Replace(".", ",")) * CDec(txt_precio.Text.Replace(".", ","))
            Dim descuento As Decimal = (cantidad_precio * CDec(txt_descuento.Text.Replace(".", ","))) / 100
            Dim impuesto As Decimal = ((cantidad_precio - descuento) * CDec(txt_impuestos.Text)) / 100
            Renglon("total") = Math.Round((cantidad_precio - descuento) + impuesto, 2)
            If txt_index.Text <> "" Then
                Renglon("observaciones") = __txt_observaciones_edit.Text
                Renglon("imagen") = txt_imagen_edit.Text
            End If
            tabla_detalles_facturas.Rows.Add(Renglon)

            'Ordeno
            tabla_detalles_facturas.DefaultView.Sort = "linea ASC"
            tabla_detalles_facturas = tabla_detalles_facturas.DefaultView.ToTable

            'Asigno
            GV_detalles_factura.DataSource = tabla_detalles_facturas
            GV_detalles_factura.DataBind()

            'Limpio
            txt_cantidad.Text = Nothing
            txt_codigo.Text = Nothing
            txt_denominacion.Text = Nothing
            txt_precio.Text = Nothing
            txt_descuento.Text = Nothing
            txt_impuestos.Text = Nothing
            txt_index.Text = Nothing
            __txt_observaciones_edit.Text = Nothing
            txt_imagen_edit.Text = Nothing

            'Asigno
            txt_cantidad.Focus()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_detalle_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_detalle_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub GV_detalles_factura_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GV_detalles_factura.RowDataBound

        Try

            'Recorro el GV
            If e.Row.RowType = DataControlRowType.DataRow Then

                'Maquillaje
                e.Row.Cells(5).Text = FormatNumber(e.Row.Cells(5).Text, 2)
                e.Row.Cells(6).Text = FormatNumber(e.Row.Cells(6).Text, 2)
                e.Row.Cells(7).Text = FormatNumber(e.Row.Cells(7).Text, 2)
                e.Row.Cells(8).Text = FormatNumber(e.Row.Cells(8).Text, 2)

            End If

        Catch ex As Exception
            'Obtengo
            Dim tabla_usuario As DataTable = HttpContext.Current.Session("c_" & Request.QueryString("id_usuario") & "_tabla_usuario")
            Dim tabla_empresa As DataTable = HttpContext.Current.Session("c_" & Request.QueryString("id_empresa") & "_tabla_empresa")
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "GV_detalles_factura_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error GV_detalles_factura_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub GV_detalles_factura_DataBound(sender As Object, e As EventArgs) Handles GV_detalles_factura.DataBound

        Try

            'Control para menus de exportar
            If GV_detalles_factura.Rows.Count = 0 Then

                'Asigno
                PL_total.Visible = False
                btn_grabar.Visible = False
                btn_actualizar.Visible = False

            Else

                'Asigno
                PL_total.Visible = True

                'Si es un presupuesto nuevo o una actualización
                If txt_presupuesto.Text = "" Then

                    'Asigno
                    btn_grabar.Visible = True
                    btn_actualizar.Visible = False

                Else

                    'Asigno
                    btn_actualizar.Visible = True
                    btn_grabar.Visible = False
                    LB_imprimir.Visible = False
                    LB_email.Visible = False

                End If

                'Refrecar los totales
                refrescar_totales()

            End If

            'Recorremos
            For x = 0 To GV_detalles_factura.Rows.Count - 1

                'Ocultar subir o bajar
                If GV_detalles_factura.DataKeys(x).Item("linea").ToString() = 0 Then

                    'Declaro
                    Dim lk_subir As LinkButton = CType(GV_detalles_factura.Rows(x).FindControl("lk_subir"), LinkButton)

                    'Asigno
                    lk_subir.Visible = False

                End If

                'Ocultar subir o bajar
                If GV_detalles_factura.DataKeys(x).Item("linea").ToString() = GV_detalles_factura.Rows.Count - 1 Then

                    'Declaro
                    Dim lk_bajar As LinkButton = CType(GV_detalles_factura.Rows(x).FindControl("lk_bajar"), LinkButton)

                    'Asigno
                    lk_bajar.Visible = False

                End If

            Next

        Catch ex As Exception
            'Obtengo
            Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
            Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "GV_detalles_factura_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error GV_detalles_factura_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub GV_detalles_factura_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GV_detalles_factura.RowCommand

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "borrar") Then

                'Asignamos
                txt_index.Text = index

                LT_mensaje_eliminar_apunte.Text = "¿Está seguro de eliminar la Línea con el artículo: " & GV_detalles_factura.Rows(index).Cells(3).Text & " - " & GV_detalles_factura.Rows(index).Cells(4).Text & " ?"

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar_apunte').modal('show');"), True)

            End If

            If (e.CommandName = "editar") Then

                'Asignamos
                txt_index.Text = index

                'Asigno
                txt_cantidad.Text = GV_detalles_factura.DataKeys(index).Item("cantidad").ToString()
                If GV_detalles_factura.DataKeys(index).Item("codigo") = 0 Then
                    txt_codigo.Text = Nothing
                Else
                    txt_codigo.Text = GV_detalles_factura.DataKeys(index).Item("codigo").ToString()
                End If
                txt_denominacion.Text = GV_detalles_factura.DataKeys(index).Item("articulo").ToString()
                txt_precio.Text = GV_detalles_factura.DataKeys(index).Item("precio").ToString()
                txt_descuento.Text = GV_detalles_factura.DataKeys(index).Item("dto").ToString()
                txt_impuestos.Text = GV_detalles_factura.DataKeys(index).Item("impuesto").ToString()
                txt_imagen_edit.Text = GV_detalles_factura.DataKeys(index).Item("imagen").ToString()
                __txt_observaciones_edit.Text = GV_detalles_factura.DataKeys(index).Item("observaciones").ToString()

                'Creo un Datatable
                Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

                'Borro la linea seleccionada
                tabla_detalles_facturas.Rows.RemoveAt(txt_index.Text)

                'Actualizo
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

                'Asigno
                txt_cantidad.Focus()

            End If

            If (e.CommandName = "lk_subir") Then

                'Creo un Datatable
                Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

                'Si es 1
                If index = 1 Then

                    'Asigno
                    tabla_detalles_facturas.Rows(index - 1)("linea") = 1
                    tabla_detalles_facturas.Rows(index)("linea") = 0

                Else

                    'Asigno
                    tabla_detalles_facturas.Rows(index)("linea") = index - 1.1

                End If

                'Ordeno
                tabla_detalles_facturas.DefaultView.Sort = "linea ASC"
                tabla_detalles_facturas = tabla_detalles_facturas.DefaultView.ToTable

                'Recorro y asigno
                For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                    'Asigno
                    tabla_detalles_facturas.Rows(x)("linea") = x

                Next

                'Asigno
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

                'Asigno
                btn_actualizar.Visible = True
                btn_grabar.Visible = False
                LB_imprimir.Visible = False
                LB_email.Visible = False

            End If

            If (e.CommandName = "lk_bajar") Then

                'Creo un Datatable
                Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

                'Asigno
                tabla_detalles_facturas.Rows(index)("linea") = index + 1.1

                'Ordeno
                tabla_detalles_facturas.DefaultView.Sort = "linea ASC"
                tabla_detalles_facturas = tabla_detalles_facturas.DefaultView.ToTable

                'Recorro y asigno
                For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                    'Asigno
                    tabla_detalles_facturas.Rows(x)("linea") = x

                Next

                'Asigno
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

                'Asigno
                btn_actualizar.Visible = True
                btn_grabar.Visible = False
                LB_imprimir.Visible = False
                LB_email.Visible = False

            End If

            If (e.CommandName = "btn_add") Then

                'Asigno
                txt_index.Text = index

                'Asigno
                __txt_observaciones.Text = GV_detalles_factura.DataKeys(index).Item("observaciones").ToString()
                chk_eliminar_imagen.Enabled = False

                'Limpio
                img_observaciones.ImageUrl = Nothing
                img_observaciones.DataBind()

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Asigno imagen
                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/presupuesto/imagenes/" & GV_detalles_factura.DataKeys(index).Item("imagen").ToString) = True Then
                    img_observaciones.ImageUrl = "~/imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/presupuesto/imagenes/" & GV_detalles_factura.DataKeys(index).Item("imagen").ToString & "?" + numero_aleatorio.Next(100, 999999999).ToString()
                    img_observaciones.DataBind()
                    chk_eliminar_imagen.Enabled = True

                End If

                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/temp/" & GV_detalles_factura.DataKeys(index).Item("imagen").ToString) = True Then
                    img_observaciones.ImageUrl = "~/imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/temp/" & GV_detalles_factura.DataKeys(index).Item("imagen").ToString & "?" + numero_aleatorio.Next(100, 999999999).ToString()
                    img_observaciones.DataBind()
                    chk_eliminar_imagen.Enabled = True
                End If

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_adjuntar", funciones_globales.modal_register("$('#modal_observaciones').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "GV_detalles_factura_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error GV_detalles_factura_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub chk_eliminar_imagen_CheckedChanged(sender As Object, e As EventArgs) Handles chk_eliminar_imagen.CheckedChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If chk_eliminar_imagen.Checked Then

                'Asigno
                lt_mensaje_eliminar.Text = "¿Esta seguro que desea eliminar la imagen?"
                chk_eliminar_imagen.Checked = False

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_observaciones').modal('show');$('#modal_eliminar_imagen').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "chk_eliminar_imagen_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error chk_eliminar_imagen_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_si_eliminar_Click(sender As Object, e As EventArgs) Handles btn_si_eliminar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Compruebo si existe en el almacén de imagenes
            If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/presupuesto/imagenes/" & GV_detalles_factura.DataKeys(txt_index.Text).Item("imagen").ToString) = True Then

                'Elimino
                System.IO.File.Delete("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/presupuesto/imagenes/" & GV_detalles_factura.DataKeys(txt_index.Text).Item("imagen").ToString)

            End If

            If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/temp/" & GV_detalles_factura.DataKeys(txt_index.Text).Item("imagen").ToString) = True Then

                'Elimino
                System.IO.File.Delete("D:/imagenes_usuarios_facturacion/temp/" & GV_detalles_factura.DataKeys(txt_index.Text).Item("imagen").ToString)

            End If

            'Limpio
            img_observaciones.ImageUrl = Nothing
            img_observaciones.DataBind()

            'Creo un Datatable
            Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

            'Asigno
            tabla_detalles_facturas.Rows(txt_index.Text)("imagen") = Nothing

            'Actualizo
            GV_detalles_factura.DataSource = tabla_detalles_facturas
            GV_detalles_factura.DataBind()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Imagen eliminada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_eliminar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_eliminar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_observaciones_Click(sender As Object, e As EventArgs) Handles btn_observaciones.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Creo un Datatable
            Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

            'Asigno
            tabla_detalles_facturas.Rows(txt_index.Text)("observaciones") = __txt_observaciones.Text

            'Compruebo si viene con imagen
            If FileUpload.HasFile Then

                'Asigno 
                Dim nombre As String = "Presupuesto_" & tabla_usuario.Rows(0)("Id").ToString & "_" & Date.Now.ToShortDateString.Replace("/", "") & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & Date.Now.Millisecond & Path.GetExtension(FileUpload.FileName)
                Dim ruta_imagen As String = "D:\imagenes_usuarios_facturacion\temp\"

                'Creo la carpeta para el fondo de éste usuario
                If Not System.IO.Directory.Exists(ruta_imagen) Then
                    System.IO.Directory.CreateDirectory(ruta_imagen)
                End If

                'Subo la imagen
                FileUpload.SaveAs(ruta_imagen & "temp_" & nombre)

                'Indico la ruta de la imagen
                Dim bm_source As New Bitmap(ruta_imagen & "temp_" & nombre)

                'Calculo el alto
                Dim alto_final As Integer = 0
                alto_final = ((350 * bm_source.Height) / bm_source.Width)

                'Convierto a la resolución y tamaño deseados
                Dim imagen As New Bitmap(bm_source, 350, alto_final)

                'Selecciono dependiendo del tipo
                Select Case Path.GetExtension(FileUpload.FileName.ToString.ToLower)
                    Case ".jpg", ".jpeg"

                        'Grabo
                        imagen.Save(ruta_imagen & nombre, System.Drawing.Imaging.ImageFormat.Jpeg)

                    Case ".png"

                        'Grabo
                        imagen.Save(ruta_imagen & nombre, System.Drawing.Imaging.ImageFormat.Png)

                    Case Else

                        'Grabo
                        imagen.Save(ruta_imagen & nombre, System.Drawing.Imaging.ImageFormat.Png)

                End Select

                'Limpio variable para liberar el recurso
                bm_source.Dispose()
                imagen.Dispose()

                'Borro el fichero temporal
                System.IO.File.Delete(ruta_imagen & "temp_" & nombre)

                'Asigno
                tabla_detalles_facturas.Rows(txt_index.Text)("imagen") = nombre

            End If

            'Actualizo
            GV_detalles_factura.DataSource = tabla_detalles_facturas
            GV_detalles_factura.DataBind()

            'Limpieza
            txt_index.Text = Nothing
            __txt_observaciones_edit.Text = Nothing
            txt_imagen_edit.Text = Nothing

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_observaciones_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_observaciones_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub GV_totales_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GV_totales.RowDataBound

        Try

            'Recorro el GV
            If e.Row.RowType = DataControlRowType.DataRow Then

                'Maquillaje
                e.Row.Cells(0).Text = FormatNumber(e.Row.Cells(0).Text, 2)
                e.Row.Cells(1).Text = FormatNumber(e.Row.Cells(1).Text, 2)
                e.Row.Cells(2).Text = FormatNumber(e.Row.Cells(2).Text, 2)
                e.Row.Cells(3).Text = FormatNumber(e.Row.Cells(3).Text, 2)

            End If

        Catch ex As Exception
            'Obtengo
            Dim tabla_usuario As DataTable = HttpContext.Current.Session("c_" & Request.QueryString("id_usuario") & "_tabla_usuario")
            Dim tabla_empresa As DataTable = HttpContext.Current.Session("c_" & Request.QueryString("id_empresa") & "_tabla_empresa")
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "GV_totales_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error GV_totales_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_eliminar_apunte_Click(sender As Object, e As EventArgs) Handles btn_eliminar_apunte.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Creo un Datatable
            Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

            'Borro la linea seleccionada
            tabla_detalles_facturas.Rows.RemoveAt(txt_index.Text)

            'Recorro y asigno
            For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                'Asigno
                tabla_detalles_facturas.Rows(x)("linea") = x

            Next

            'Actualizo
            GV_detalles_factura.DataSource = tabla_detalles_facturas
            GV_detalles_factura.DataBind()

            'Limpia
            txt_index.Text = Nothing
            __txt_observaciones_edit.Text = Nothing
            txt_imagen_edit.Text = Nothing

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('El Artículo ha sido eliminado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_eliminar_apunte_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_eliminar_apunte_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_nuevo_Click(sender As Object, e As ImageClickEventArgs) Handles img_nuevo.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Reinicio el sistema
            Response.Redirect("altas.aspx?id_usuario=" & Request.QueryString("id_usuario") & "&id_empresa=" & Request.QueryString("id_empresa"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_nuevo_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_nuevo_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_duplicar_Click(sender As Object, e As ImageClickEventArgs) Handles img_duplicar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Copio las imagenes
            For x = 0 To GV_detalles_factura.Rows.Count - 1

                If GV_detalles_factura.DataKeys(x).Item("imagen").ToString <> "" Then

                    'Asigno 
                    Dim nombre As String = GV_detalles_factura.DataKeys(x).Item("imagen").ToString()
                    Dim ruta_imagen_origen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos").ToString & "\presupuesto\imagenes\"
                    Dim ruta_imagen_destino As String = "D:\imagenes_usuarios_facturacion\temp\"

                    'Creo la carpeta para el fondo de éste usuario
                    If Not System.IO.Directory.Exists(ruta_imagen_destino) Then
                        System.IO.Directory.CreateDirectory(ruta_imagen_destino)
                    End If

                    'Copio al fila
                    System.IO.File.Copy(ruta_imagen_origen & nombre, ruta_imagen_destino & nombre, True)

                End If

            Next

            'Asigno
            txt_fecha.Text = DateTime.Now.ToShortDateString
            txt_presupuesto.Text = Nothing
            LB_imprimir.Visible = False
            LB_email.Visible = False
            btn_grabar.Visible = True
            btn_actualizar.Visible = False
            PL_cabecera.Enabled = True
            PL_detalles.Enabled = True
            PL_total.Enabled = True

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_duplicar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_duplicar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub img_convertir_albaran_Click(sender As Object, e As ImageClickEventArgs) Handles img_convertir_albaran.Click


        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            LT_mensaje_abonar.Text = "¿Está seguro de Convertir en Albarán?"

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_convertir_factura').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_convertir_albaran_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_convertir_albaran_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_convertir_factura_confirmar_Click(sender As Object, e As EventArgs) Handles btn_convertir_factura_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "abrir_ventana", "$('#ialtas_____',window.parent.document).attr('src',$('#ialtas_____',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('altas_____','bi bi-columns-gap','1000','600','albaranes/altas.aspx|id_presupuesto=" & txt_presupuesto.Text & "','6');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_convertir_factura_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_convertir_factura_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_Click(sender As Object, e As EventArgs) Handles btn_grabar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepcion
            If txt_cliente.Text = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_cliente').focus();$('#txt_cliente').select();}, 100);error('El campo Cliente no puede estar vacío.')"), True)
                Exit Sub
            End If

            If txt_nif.Text = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", funciones_globales.modal_register("setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);error('El campo NIF no puede estar vacío.')"), True)
                Exit Sub
            End If

            'Asigno
            LT_mensaje_confirmar.Text = "¿Está seguro de continuar y generar un Presupuesto?"

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_confirmar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_grabar_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Declaro
            Dim cod_cliente As Integer = 0
            Dim n_presupuesto As String = Nothing

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Declaro
            If txt_cod_cliente.Text <> "" Then
                cod_cliente = txt_cod_cliente.Text
            End If

            'Obtengo un nuevo numero de factura para el método elegido para la empresa
            Select Case tabla_empresa.Rows(0)("tipo_autonumerico_asientos")
                Case "normal" : n_presupuesto = funciones_globales.numero_presupuesto(tabla_empresa.Rows(0)("ruta_base_datos"))
            End Select

            'Asigno
            txt_presupuesto.Text = n_presupuesto

            'Grabo la Cabecera de la Factura
            funciones_globales.grabar_presupuestos_(tabla_empresa.Rows(0)("ruta_base_datos"), n_presupuesto, txt_fecha.Text, cod_cliente, txt_cliente.Text, txt_nif.Text, txt_direccion.Text, txt_cp.Text, txt_poblacion.Text, txt_provincia.Text, GV_detalles_factura, GV_totales)

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Creó el Presupuesto: " & txt_presupuesto.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Asigno
            LB_imprimir.Visible = True
            LB_email.Visible = True
            btn_grabar.Visible = False
            PL_cabecera.Enabled = False
            PL_detalles.Enabled = True
            PL_total.Enabled = True

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Presupuesto creado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_confirmar", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_confirmar: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_actualizar_Click(sender As Object, e As EventArgs) Handles btn_actualizar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            LT_mensaje_confirmar_actualizar.Text = "¿Está seguro de continuar y actualizar el Presupuesto?"

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_confirmar_actualizar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_actualizar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_actualizar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try
    End Sub

    Private Sub btn_actualizar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_actualizar_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Grabo la Cabecera de la Factura
            funciones_globales.modificar_presupuestos_(tabla_empresa.Rows(0)("ruta_base_datos"), txt_presupuesto.Text, GV_detalles_factura, GV_totales)

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Modificó el Presupuesto: " & txt_presupuesto.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Asigno
            LB_imprimir.Visible = True
            LB_email.Visible = True
            btn_grabar.Visible = False
            btn_actualizar.Visible = False
            PL_cabecera.Enabled = False
            PL_detalles.Enabled = True
            PL_total.Enabled = True

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Presupuesto modificado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_actualizar_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_actualizar_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub LB_imprimir_Click(sender As Object, e As EventArgs) Handles LB_imprimir.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Obtengo el nombre del fichero creado PDF
            Dim nombre_fichero As String = crear_pdf_salida(tabla_empresa.Rows(0)("ruta_base_datos"), txt_presupuesto.Text)

            'Paso la ruta al iframe
            ver_factura.Attributes("src") = "..\temp\" & nombre_fichero

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", funciones_globales.modal_register("$('#modal_visor').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "LB_imprimir_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error LB_imprimir_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Function crear_pdf_salida(ByVal ruta_base_datos As String, ByVal n_presupuesto As String) As String

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Genero el nombre del fichero
        Dim nombre As String = "Presupuesto_" & tabla_usuario.Rows(0)("Id") & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".pdf"

        'Creo la carpeta para contener el informe temporalmente.
        If Not System.IO.Directory.Exists(Server.MapPath("..") & "\temp") Then
            System.IO.Directory.CreateDirectory(Server.MapPath("..") & "\temp")
        End If

        'Creo una session para el constructor de PDF
        Session("presupuesto") = n_presupuesto & "|" & ruta_base_datos

        'Comienzo la generacion del PDF
        Dim oDoc As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4)
        oDoc.SetMargins(funciones_globales.puntos(0), funciones_globales.puntos(0), funciones_globales.puntos(0), funciones_globales.puntos(40))
        Dim pdfw As iTextSharp.text.pdf.PdfWriter
        Dim cb As iTextSharp.text.pdf.PdfContentByte
        Dim linea As iTextSharp.text.pdf.PdfContentByte
        Dim fuente As iTextSharp.text.pdf.BaseFont
        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

        'Genero el PDF
        pdfw = iTextSharp.text.pdf.PdfWriter.GetInstance(oDoc, New FileStream(Server.MapPath("..") & "\temp\" & nombre, FileMode.Create, FileAccess.Write, FileShare.None))

        Dim ev As New itsEvents
        pdfw.PageEvent = ev

        'Apertura del documento.
        oDoc.Open()
        cb = pdfw.DirectContent
        linea = pdfw.DirectContent

        'Instanciamos el objeto para el tipo de letra. 
        fuente = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont

        'Obtengo
        Dim tabla_detalles_factura As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & ruta_base_datos & "].[dbo].detalles_presupuestos WHERE n_presupuesto='" & n_presupuesto & "';")

        'Declaro
        Dim f = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 255))
        Dim f2 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 255))

        'Recorro
        For x = 0 To tabla_detalles_factura.Rows.Count - 1

            'Declaro
            Dim tabla_detalles As New iTextSharp.text.pdf.PdfPTable(6)
            tabla_detalles.TotalWidth = funciones_globales.puntos(190)
            tabla_detalles.LockedWidth = True
            tabla_detalles.SetWidths({funciones_globales.puntos(20), funciones_globales.puntos(140), funciones_globales.puntos(25), funciones_globales.puntos(25), funciones_globales.puntos(25), funciones_globales.puntos(25)})
            Dim cell As iTextSharp.text.pdf.PdfPCell

            'Creo la columna cod_articulo
            If tabla_detalles_factura.Rows(x)("cantidad") = 0 And tabla_detalles_factura.Rows(x)("precio") = 0 And tabla_detalles_factura.Rows(x)("dto_1") = 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else

                'Si tb es 0
                If tabla_detalles_factura.Rows(x)("cod_articulo") = 0 Then
                    cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
                Else
                    cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(tabla_detalles_factura.Rows(x)("cod_articulo"), f))
                End If

            End If
            cell.HorizontalAlignment = 1
            cell.Border = 0
            cell.BorderWidthTop = 1
            cell.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
            tabla_detalles.AddCell(cell)

            'Creo la columna denominacion del articulo
            If tabla_detalles_factura.Rows(x)("cantidad") = 0 And tabla_detalles_factura.Rows(x)("precio") = 0 And tabla_detalles_factura.Rows(x)("dto_1") = 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(tabla_detalles_factura.Rows(x)("denominacion"), f2))
            Else
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(tabla_detalles_factura.Rows(x)("denominacion"), f))
            End If
            cell.HorizontalAlignment = 3
            cell.Border = 0
            cell.BorderWidthTop = 1
            cell.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
            tabla_detalles.AddCell(cell)

            'Creo la columna cantidad
            If tabla_detalles_factura.Rows(x)("cantidad") = 0 And tabla_detalles_factura.Rows(x)("precio") = 0 And tabla_detalles_factura.Rows(x)("dto_1") = 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_detalles_factura.Rows(x)("cantidad"), 2), f))
            End If
            cell.HorizontalAlignment = 1
            cell.Border = 0
            cell.BorderWidthTop = 1
            cell.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
            tabla_detalles.AddCell(cell)

            'Creo la columna precio
            If tabla_detalles_factura.Rows(x)("cantidad") = 0 And tabla_detalles_factura.Rows(x)("precio") = 0 And tabla_detalles_factura.Rows(x)("dto_1") = 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_detalles_factura.Rows(x)("precio"), 2), f))
            End If
            cell.HorizontalAlignment = 1
            cell.Border = 0
            cell.BorderWidthTop = 1
            cell.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
            tabla_detalles.AddCell(cell)

            'Creo la columna dto
            If tabla_detalles_factura.Rows(x)("cantidad") = 0 And tabla_detalles_factura.Rows(x)("precio") = 0 And tabla_detalles_factura.Rows(x)("dto_1") = 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_detalles_factura.Rows(x)("dto_1"), 2) & " %", f))
            End If
            cell.HorizontalAlignment = 1
            cell.Border = 0
            cell.BorderWidthTop = 1
            cell.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
            tabla_detalles.AddCell(cell)

            'Creo la columna total
            Dim cantidad_precio As Decimal = Math.Round(tabla_detalles_factura.Rows(x)("cantidad") * tabla_detalles_factura.Rows(x)("precio"), 2)
            Dim total As Decimal = Math.Round(cantidad_precio - (cantidad_precio * tabla_detalles_factura.Rows(x)("dto_1")) / 100, 2)
            If tabla_detalles_factura.Rows(x)("cantidad") = 0 And tabla_detalles_factura.Rows(x)("precio") = 0 And tabla_detalles_factura.Rows(x)("dto_1") = 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(total, 2), f))
            End If
            cell.HorizontalAlignment = 1
            cell.Border = 0
            cell.BorderWidthTop = 1
            cell.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
            tabla_detalles.AddCell(cell)

            'Lo agrego a la pagina
            oDoc.Add(tabla_detalles)

            'Si viene con foto u observaciones
            If tabla_detalles_factura.Rows(x)("observaciones").ToString <> "" Or tabla_detalles_factura.Rows(x)("imagen").ToString <> "" Then

                'Declaro
                Dim tabla_detalles_observaciones As New iTextSharp.text.pdf.PdfPTable(2)
                tabla_detalles_observaciones.TotalWidth = funciones_globales.puntos(200)
                tabla_detalles_observaciones.LockedWidth = False
                tabla_detalles_observaciones.SetWidths({funciones_globales.puntos(50), funciones_globales.puntos(150)})
                Dim cell_observaciones As New iTextSharp.text.pdf.PdfPCell
                Dim cell_imagen As New iTextSharp.text.pdf.PdfPCell

                'Simula retorno de carro
                oDoc.Add(New Paragraph(" "))

                'Si viene con imagen
                If Not IsDBNull(tabla_detalles_factura.Rows(x)("imagen")) And tabla_detalles_factura.Rows(x)("imagen").ToString <> "" Then

                    Dim alto_final As Integer = 0
                    'Obtener alto y ancho de la imagen para ajustar al PDF
                    Dim fs As FileStream = New FileStream("D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\presupuesto\imagenes\" & tabla_detalles_factura.Rows(x)("imagen"), FileMode.Open, FileAccess.Read, FileShare.Read)
                    Dim Logo_imagen As System.Drawing.Image
                    Logo_imagen = System.Drawing.Image.FromStream(fs)
                    alto_final = ((25 * Logo_imagen.Height) / Logo_imagen.Width)
                    fs.Dispose()

                    'Insertamos la imagen de fondo
                    Dim imagenlogo As iTextSharp.text.Image 'Declaracion de una imagen
                    imagenlogo = iTextSharp.text.Image.GetInstance("D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\presupuesto\imagenes\" & tabla_detalles_factura.Rows(x)("imagen")) 'Direccion a la imagen que se hace referencia
                    imagenlogo.ScaleAbsoluteWidth(funciones_globales.puntos(25)) 'Ancho de la imagen
                    imagenlogo.ScaleAbsoluteHeight(funciones_globales.puntos(alto_final)) 'Altura de la imagen
                    cell_imagen = New iTextSharp.text.pdf.PdfPCell(imagenlogo)
                    cell_imagen.Border = 0
                    tabla_detalles_observaciones.AddCell(cell_imagen) ' Agrega la imagen al documento

                Else

                    cell_imagen = New PdfPCell(New Phrase(" "))
                    cell_imagen.Border = 0
                    tabla_detalles_observaciones.AddCell(cell_imagen)

                End If

                'Si viene con observaciones
                If Not IsDBNull(tabla_detalles_factura.Rows(x)("observaciones")) Then

                    'Declaro y convierto
                    Dim htmlElements As List(Of IElement) = HTMLWorker.ParseToList(New StringReader(tabla_detalles_factura.Rows(x)("observaciones")), Nothing)

                    'Recorro
                    For Each element As IElement In htmlElements

                        'Asigno
                        cell_observaciones.AddElement(element)

                    Next
                    cell_observaciones.HorizontalAlignment = 0
                    cell_observaciones.Border = 0
                    cell_observaciones.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
                    tabla_detalles_observaciones.AddCell(cell_observaciones)

                Else

                    cell_observaciones = New PdfPCell(New Phrase("a "))
                    cell_observaciones.Border = 0
                    tabla_detalles_observaciones.AddCell(cell_observaciones)

                End If

                'Lo agrego a la pagina
                oDoc.Add(tabla_detalles_observaciones)

                'Simula retorno de carro
                oDoc.Add(New iTextSharp.text.Paragraph(" "))

            End If

        Next

        'Insertamos la imagen de fondo
        imagendemo = iTextSharp.text.Image.GetInstance("D:\imagenes_usuarios_facturacion\" & ruta_base_datos & "\presupuesto\pie_presupuesto.png") 'Dirreccion a la imagen que se hace referencia
        imagendemo.SetAbsolutePosition(0, 0) 'Posicion en el eje cartesiano
        imagendemo.ScaleAbsoluteWidth(funciones_globales.puntos(210)) 'Ancho de la imagen
        imagendemo.ScaleAbsoluteHeight(funciones_globales.puntos(50)) 'Altura de la imagen
        oDoc.Add(imagendemo) ' Agrega la imagen al documento

        'Obtengo
        Dim tabla_pie_factura As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & ruta_base_datos & "].[dbo].pie_presupuestos WHERE n_presupuesto='" & n_presupuesto & "';")
        Dim total_factura As Decimal = 0

        'Declaro
        Dim tabla_totales As New iTextSharp.text.pdf.PdfPTable(4)
        tabla_totales.TotalWidth = funciones_globales.puntos(82)
        tabla_totales.LockedWidth = True

        'Recorro
        For x = 0 To tabla_pie_factura.Rows.Count - 1

            'Declaro
            Dim cell_totales As iTextSharp.text.pdf.PdfPCell

            'Creo la primera columna
            If tabla_pie_factura.Rows(x)("base_imponible") = 0 And tabla_pie_factura.Rows(x)("porcentaje") = 0 And tabla_pie_factura.Rows(x)("cuota") = 0 Then
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("base_imponible"), 2), f))
            End If
            cell_totales.HorizontalAlignment = 1
            cell_totales.Border = 0
            tabla_totales.AddCell(cell_totales)

            'Creo la primera columna
            If tabla_pie_factura.Rows(x)("base_imponible") = 0 And tabla_pie_factura.Rows(x)("porcentaje") = 0 And tabla_pie_factura.Rows(x)("cuota") = 0 Then
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("porcentaje"), 2), f))
            End If
            cell_totales.HorizontalAlignment = 1
            cell_totales.Border = 0
            tabla_totales.AddCell(cell_totales)

            'Creo la primera columna
            If tabla_pie_factura.Rows(x)("base_imponible") = 0 And tabla_pie_factura.Rows(x)("porcentaje") = 0 And tabla_pie_factura.Rows(x)("cuota") = 0 Then
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("cuota"), 2), f))
            End If
            cell_totales.HorizontalAlignment = 1
            cell_totales.Border = 0
            tabla_totales.AddCell(cell_totales)

            'Creo la primera columna
            If tabla_pie_factura.Rows(x)("base_imponible") = 0 And tabla_pie_factura.Rows(x)("porcentaje") = 0 And tabla_pie_factura.Rows(x)("cuota") = 0 Then
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("total"), 2), f))
            End If
            cell_totales.HorizontalAlignment = 1
            cell_totales.Border = 0
            tabla_totales.AddCell(cell_totales)

            'Asigno
            total_factura += tabla_pie_factura.Rows(x)("total")
        Next

        'Añado una columna de texto
        Dim ct As iTextSharp.text.pdf.ColumnText = New iTextSharp.text.pdf.ColumnText(cb)
        ct.SetSimpleColumn(New iTextSharp.text.Rectangle(funciones_globales.puntos(-10), funciones_globales.puntos(0), funciones_globales.puntos(100), funciones_globales.puntos(39)))
        ct.AddElement(tabla_totales)
        ct.Go()

        'Iniciamos el flujo de bytes.             
        cb.BeginText()

        'Total de la factura
        cb.SetFontAndSize(fuente, 18)
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, FormatNumber(total_factura, 2) & " €", funciones_globales.puntos(184.5), funciones_globales.puntos(30), 0)

        'Fin del flujo de bytes.             
        cb.EndText()

        'Cerramos el documento.             
        oDoc.Close()

        'Destruimos las variables
        oDoc.Dispose()
        pdfw.Dispose()

        'Devuelve
        Return nombre

    End Function

    Private Sub LB_email_Click(sender As Object, e As EventArgs) Handles LB_email.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepcion
            If tabla_empresa.Rows(0)("email_envios").ToString = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('No tienes configurado ninguna cuenta de correo electrónico.')", True)
                Exit Sub
            End If

            'Limpiar
            txt_destinatario.Text = Nothing
            __txt_mensaje.Text = Nothing
            txt_nombre_fichero_email.Text = Nothing
            PH_multiples_email.Visible = False

            'Obtengo posibles email
            Dim tabla_email As DataTable = funciones_globales.obtener_datos("SELECT contacto FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].[contactos] WHERE id_cliente =" & txt_cod_cliente.Text & " AND contacto LIKE '%@%' ORDER BY contacto;")

            'si tiene datos
            If tabla_email.Rows.Count <> 0 Then

                'Si tiene mas de un correo
                If tabla_email.Rows.Count > 1 Then

                    'Limpio
                    DDL_opciones_mail.Items.Clear()

                    'Asigno
                    PH_multiples_email.Visible = True
                    DDL_opciones_mail.Items.Add(New System.Web.UI.WebControls.ListItem("", ""))

                    'Recorro
                    For x = 0 To tabla_email.Rows.Count - 1

                        'Cargo los detalles
                        Dim lista As New System.Web.UI.WebControls.ListItem(tabla_email.Rows(x)("contacto").ToString, tabla_email.Rows(x)("contacto").ToString)
                        Me.DDL_opciones_mail.Items.Add(lista)

                    Next

                Else

                    'Asigno
                    txt_destinatario.Text = tabla_email.Rows(0)("contacto").ToString.ToLower

                End If

            End If

            'Obtengo el nombre del fichero creado PDF
            Dim nombre_fichero As String = crear_pdf_salida(tabla_empresa.Rows(0)("ruta_base_datos"), txt_presupuesto.Text)

            'Asigno
            txt_nombre_fichero_email.Text = nombre_fichero

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "setTimeout(function () { $('#txt_destinatario').focus(); }, 100);" & funciones_globales.modal_register("$('#modal_email').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "LB_email_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error LB_email_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_confirmar_email_Click(sender As Object, e As EventArgs) Handles btn_confirmar_email.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepción
            If funciones_globales.IsValidEmail(txt_destinatario.Text) = False Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "error", "error('El campo Email no es válido.');setTimeout(function () { $('#txt_destinatario').focus(); }, 100);" & funciones_globales.modal_register("$('#modal_email').modal('show');"), True)
                Exit Sub

            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Aqui mando un correo electrónico a los destinatarios que me diga el cliente.
            Dim correo As New System.Net.Mail.MailMessage
            correo.From = New System.Net.Mail.MailAddress(tabla_empresa.Rows(0)("email_envios").ToString)
            correo.To.Add(txt_destinatario.Text)
            correo.Subject = "Presupuesto Nº: " & txt_presupuesto.Text & "."

            'Monto el cuerpo del correo
            Dim mensajemail As String

            mensajemail = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><head>" &
            "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'> " &
            "<title>¡Te damos la bienvenida a Optimus Facturación!</title>" &
            "<style type = 'text/css'>" &
            "/* Client - specific Styles */" &
            "#outlook a{padding:0;} /* Force Outlook To provide a 'view In browser' button. */" &
            "body{width:100% !important;} .ReadMsgBody{width:100%;} .ExternalClass{width:100%;} /* Force Hotmail to display emails at full width */ " &
            "body{-webkit - Text - size - adjust: none;} /* Prevent Webkit platforms from changing Default text sizes. */ " &
            "/* Reset Styles */ " &
            "body{margin: 0; padding:0;} " &
            "img{border: 0; line-height:100%; outline:none; text-decoration:none;} " &
            "Table td{border-collapse: collapse;} " &
            "</style> " &
            "</head> " &
            "<body leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0' style='font-family:Verdana,Arial,sans-serif;'> " &
            "<table border='0' cellpadding='0' cellspacing='0' width='100%' style='width:100%;'>" &
            "<tr>" &
            "<td style='width:25%;'></td>" &
            "<td style='width:50%; text-align:center;'><span style='color: #b3b3b3'>Se envió este correo electrónico desde una dirección exclusiva de notificaciones que no recibe correos electrónicos. No respondas a este mensaje.</span></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;'></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style='width:50%;text-align:center;'><table style='width:100%;'><tr><td style='text-align:left;'><span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Hola,<span style='color: #0d6efd;'> " & System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_cliente.Text) & "</span></span></td><td style='text-align:right;'></td></tr></table></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;'></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;text-align:center;'>" &
            "<span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>" &
            "Te adjuntamos el presupuesto solicitado.</span> " &
            "</td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style = 'text-align:center; padding:30px;' align='center'>" & __txt_mensaje.Text &
            "</td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%; height:30px;'></td>" &
            "<td style='width:50%; border: 1px; border-width: 2px; border-color: red;><hr></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td><span style='color: #b3b3b3;'>" &
            "© " & Year(DateTime.Now) & " " & tabla_empresa.Rows(0)("nombre_comercial").ToString & "</span></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "</table>" &
            "</body></html>"

            correo.Body = mensajemail
            correo.IsBodyHtml = True
            correo.Priority = System.Net.Mail.MailPriority.Normal

            'Adjuntar Fichero
            correo.Attachments.Add(New Net.Mail.Attachment(Server.MapPath("..") & "\temp\" & txt_nombre_fichero_email.Text))

            'Declaro
            Dim smtp As New System.Net.Mail.SmtpClient
            Dim nombre As String = tabla_empresa.Rows(0)("email_envios").ToString
            Dim contrasena As String = tabla_empresa.Rows(0)("password_envios").ToString
            smtp.UseDefaultCredentials = False
            smtp.EnableSsl = tabla_empresa.Rows(0)("email_ssl")
            smtp.Port = tabla_empresa.Rows(0)("email_port")
            smtp.Host = tabla_empresa.Rows(0)("smtp_envios")
            smtp.Credentials = New System.Net.NetworkCredential(nombre, contrasena)

            Try

                'Envio el correo a los destinatarios
                smtp.Send(correo)

            Catch ex As Exception

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "error", "error('" & ex.Message & ".');", True)
                Exit Sub

            End Try

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Envió el Presupuesto: " & txt_presupuesto.Text & " al E-mail: " & txt_destinatario.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Email enviado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_confirmar_email_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_confirmar_email_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub DDL_opciones_mail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_opciones_mail.SelectedIndexChanged

        'Asigno
        txt_destinatario.Text = DDL_opciones_mail.SelectedItem.ToString

        DDL_opciones_mail.SelectedIndex = 0

        'Registro como bloque en local para el jquery
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "setTimeout(function () { $('#txt_destinatario').focus(); }, 100);" & funciones_globales.modal_register("$('#modal_email').modal('show');"), True)

    End Sub

End Class


Public Class itsEvents
    Inherits iTextSharp.text.pdf.PdfPageEventHelper

    Dim funciones_globales As New funciones_globales
    Dim sesion_presupuesto() As String = HttpContext.Current.Session("presupuesto").split("|")

    Public Overrides Sub OnStartPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)

        'Declaro
        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen
        Dim cb As iTextSharp.text.pdf.PdfContentByte
        Dim fuente As iTextSharp.text.pdf.BaseFont

        'Insertamos la imagen de fondo
        imagendemo = iTextSharp.text.Image.GetInstance("D:\imagenes_usuarios_facturacion\" & sesion_presupuesto(1) & "\presupuesto\fondo_presupuesto.png") 'Direccion a la imagen que se hace referencia
        imagendemo.SetAbsolutePosition(0, 0) 'Posicion en el eje cartesiano
        imagendemo.ScaleAbsoluteWidth(funciones_globales.puntos(210)) 'Ancho de la imagen
        imagendemo.ScaleAbsoluteHeight(funciones_globales.puntos(297)) 'Altura de la imagen
        document.Add(imagendemo) ' Agrega la imagen al documento

        'Saltos de Línea
        For x = 0 To 14

            'Añadismo
            document.Add(New iTextSharp.text.Paragraph(" "))

        Next

        'Obtengo
        Dim tabla_cabecera_factura As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & sesion_presupuesto(1) & "].[dbo].cabecera_presupuestos WHERE n_presupuesto='" & sesion_presupuesto(0) & "';")

        'Asignamos
        cb = writer.DirectContent
        fuente = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont

        'Iniciamos el flujo de bytes.             
        cb.BeginText()

        'Instanciamos el objeto para el tipo de letra. 
        fuente = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont

        'Tipo Letra y tamaño
        cb.SetFontAndSize(fuente, 8)

        'Montamos cuadro del cliente
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, tabla_cabecera_factura.Rows(0)("cliente").ToString.ToUpper & " - Cod.: " & tabla_cabecera_factura.Rows(0)("cod_cliente").ToString, funciones_globales.puntos(110), funciones_globales.puntos(285), 0)
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, tabla_cabecera_factura.Rows(0)("direccion").ToString.ToUpper, funciones_globales.puntos(110), funciones_globales.puntos(280), 0)
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, tabla_cabecera_factura.Rows(0)("cp").ToString.ToUpper & " - " & tabla_cabecera_factura.Rows(0)("poblacion").ToString.ToUpper, funciones_globales.puntos(110), funciones_globales.puntos(275), 0)
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, tabla_cabecera_factura.Rows(0)("provincia").ToString.ToUpper, funciones_globales.puntos(110), funciones_globales.puntos(270), 0)
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, "NIF / CIF: ", funciones_globales.puntos(110), funciones_globales.puntos(265), 0)
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, tabla_cabecera_factura.Rows(0)("nif").ToString.ToUpper, funciones_globales.puntos(125), funciones_globales.puntos(265), 0)

        'Nº Factura y Fecha
        cb.SetRGBColorFill(80, 80, 80)
        cb.SetFontAndSize(fuente, 8)
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, tabla_cabecera_factura.Rows(0)("n_presupuesto").ToString.ToUpper, funciones_globales.puntos(23), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, Mid(tabla_cabecera_factura.Rows(0)("fecha").ToString.ToUpper, 1, 10), funciones_globales.puntos(51), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, "1", funciones_globales.puntos(193), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición  

        'Fin del flujo de bytes.             
        cb.EndText()

    End Sub

End Class