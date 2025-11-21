Imports System.Data
Imports System.IO
Imports iTextSharp.text.pdf
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.Crypto
Imports iTextSharp.text.pdf.security
Imports System.Security.Cryptography.X509Certificates

Partial Class facturas_altas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Function LlenarDataTableDesdeGridView(ByVal gv As GridView) As DataTable

        'Declaro
        Dim tabla_consulta As New DataTable

        'Añado
        tabla_consulta.Columns.Add("fecha_creacion")
        tabla_consulta.Columns.Add("hora_creacion")
        tabla_consulta.Columns.Add("cantidad")
        tabla_consulta.Columns.Add("codigo")
        tabla_consulta.Columns.Add("articulo")
        tabla_consulta.Columns.Add("precio")
        tabla_consulta.Columns.Add("dto")
        tabla_consulta.Columns.Add("impuesto")
        tabla_consulta.Columns.Add("total")

        'Añado las líneas
        For Each row As GridViewRow In gv.Rows

            'Relleno
            Dim Renglon As DataRow = tabla_consulta.NewRow()
            Renglon("fecha_creacion") = gv.DataKeys(row.RowIndex).Item("fecha_creacion")
            Renglon("hora_creacion") = gv.DataKeys(row.RowIndex).Item("hora_creacion")
            Renglon("cantidad") = gv.DataKeys(row.RowIndex).Item("cantidad")
            Renglon("codigo") = gv.DataKeys(row.RowIndex).Item("codigo")
            Renglon("articulo") = gv.DataKeys(row.RowIndex).Item("articulo")
            Renglon("precio") = gv.DataKeys(row.RowIndex).Item("precio")
            Renglon("dto") = gv.DataKeys(row.RowIndex).Item("dto")
            Renglon("impuesto") = gv.DataKeys(row.RowIndex).Item("impuesto")
            Renglon("total") = gv.DataKeys(row.RowIndex).Item("total")
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

                'Asigno Opcion Inversión de Sujeto Pasivo, Exento
                If tabla_empresa.Rows(0)("facturas_isp") = True Or tabla_empresa.Rows(0)("facturas_exento") = True Then

                    'Asigno
                    PL_opciones.Visible = True

                    'Asigno
                    If tabla_empresa.Rows(0)("facturas_isp") = True Then
                        chk_isp.Visible = True
                    End If

                    If tabla_empresa.Rows(0)("facturas_exento") = True Then
                        chk_exento.Visible = True
                    End If

                End If

                'Comprobar certificado
                Dim ruta_certificado As String = "D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/certificado/certificado.pfx"

                'Ver si existe el certificado
                If My.Computer.FileSystem.FileExists(ruta_certificado) = False Then

                    'Asigno
                    lbl_certificado_invalido.Text = "No puedes emitir facturas si no tiene un certificado digital activo."

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_sin_certificado').modal('show');"), True)
                    Exit Sub

                Else

                    'Declaro
                    Dim certificate As New X509Certificate2(ruta_certificado, tabla_empresa.Rows(0)("certificado_password").ToString)
                    Dim fecha_final As Date = CDate(certificate.NotAfter)

                    If fecha_final < Now Then

                        'Asigno
                        lbl_certificado_invalido.Text = "Tu Certificado ha expirado en la fecha " & certificate.NotAfter & ", debe renovarlo para poder continuar."

                        'Registro como bloque en local para el jquery
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_sin_certificado').modal('show');"), True)
                        Exit Sub

                    End If

                End If

                'Si viene con parametros
                If Request.QueryString("id_factura") <> Nothing Then

                    'Llamar a la Factura
                    consultar_factura(Request.QueryString("id_factura"))

                End If

                If Request.QueryString("id_albaran") <> Nothing Then

                    'Llamar al Presupuesto
                    consultar_albaran(Request.QueryString("id_albaran"))

                End If

                'Asigno
                txt_cod_cliente.Focus()

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
        Dim tabla_cabecera As DataTable = funciones_globales.obtener_datos("SELECT * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[cabecera_facturas] WHERE n_factura='" & n_factura & "';")

        'Asigno
        txt_cod_cliente.Text = tabla_cabecera.Rows(0)("cod_cliente")
        txt_cliente.Text = tabla_cabecera.Rows(0)("cliente")
        txt_nif.Text = tabla_cabecera.Rows(0)("nif")
        txt_direccion.Text = tabla_cabecera.Rows(0)("direccion")
        txt_cp.Text = tabla_cabecera.Rows(0)("cp")
        txt_poblacion.Text = tabla_cabecera.Rows(0)("poblacion")
        txt_provincia.Text = tabla_cabecera.Rows(0)("provincia")
        txt_factura.Text = tabla_cabecera.Rows(0)("n_factura")
        txt_albaran.Text = tabla_cabecera.Rows(0)("n_albaran").ToString
        If tabla_cabecera.Rows(0)("n_albaran").ToString <> "" Then
            img_albaran.Visible = True
        End If
        txt_presupuesto.Text = tabla_cabecera.Rows(0)("n_presupuesto").ToString
        If tabla_cabecera.Rows(0)("n_presupuesto").ToString <> "" Then
            img_presupuesto.Visible = True
        End If
        chk_isp.Checked = tabla_cabecera.Rows(0)("isp")
        chk_exento.Checked = tabla_cabecera.Rows(0)("exento")

        'Obtengo los Detalles
        Dim tabla_detalles As DataTable = funciones_globales.obtener_datos("SELECT fecha_creacion,hora_creacion,cantidad,cod_articulo AS codigo,denominacion AS articulo,precio,dto_1 AS dto,porcentaje_impuesto AS impuesto,total FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[detalles_facturas] WHERE n_factura='" & n_factura & "' ORDER BY linea;")

        'Asigno
        GV_detalles_factura.DataSource = tabla_detalles
        GV_detalles_factura.DataBind()

        'Asigno
        LB_imprimir.Visible = True
        LB_email.Visible = True
        btn_grabar.Visible = False
        PL_cabecera.Enabled = False
        PL_opciones.Enabled = False
        PL_detalles.Enabled = False
        PL_total.Enabled = False
        If tabla_cabecera.Rows(0)("origen").ToString = "Abono" Then
            img_abonar.Visible = False
        End If

    End Sub

    Sub consultar_albaran(ByVal n_albaran As String)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Obtengo la Cabecera
        Dim tabla_cabecera As DataTable = funciones_globales.obtener_datos("SELECT * FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[cabecera_albaranes] WHERE n_albaran='" & n_albaran & "';")

        'Asigno
        txt_cod_cliente.Text = tabla_cabecera.Rows(0)("cod_cliente").ToString
        txt_cliente.Text = tabla_cabecera.Rows(0)("cliente").ToString
        txt_nif.Text = tabla_cabecera.Rows(0)("nif").ToString
        txt_direccion.Text = tabla_cabecera.Rows(0)("direccion").ToString
        txt_cp.Text = tabla_cabecera.Rows(0)("cp").ToString
        txt_poblacion.Text = tabla_cabecera.Rows(0)("poblacion").ToString
        txt_provincia.Text = tabla_cabecera.Rows(0)("provincia").ToString
        txt_albaran.Text = tabla_cabecera.Rows(0)("n_albaran").ToString
        If tabla_cabecera.Rows(0)("n_albaran").ToString <> "" Then
            img_albaran.Visible = True
        End If
        txt_presupuesto.Text = tabla_cabecera.Rows(0)("n_presupuesto").ToString
        If tabla_cabecera.Rows(0)("n_presupuesto").ToString <> "" Then
            img_presupuesto.Visible = True
        End If

        'Obtengo los Detalles
        Dim tabla_detalles As DataTable = funciones_globales.obtener_datos("SELECT fecha_creacion,hora_creacion,cantidad,cod_articulo AS codigo,denominacion AS articulo,precio,dto_1 AS dto,porcentaje_impuesto AS impuesto,total FROM " & tabla_empresa.Rows(0)("ruta_base_datos") & ".[dbo].[detalles_albaranes] WHERE n_albaran='" & n_albaran & "' ORDER BY linea;")

        'Asigno
        GV_detalles_factura.DataSource = tabla_detalles
        GV_detalles_factura.DataBind()

        'Asigno
        LB_imprimir.Visible = False
        LB_email.Visible = False
        btn_grabar.Visible = True
        PL_cabecera.Enabled = True
        PL_detalles.Enabled = True
        PL_total.Enabled = True

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
            Renglon("cantidad") = txt_cantidad.Text.Replace(".", ",")
            Renglon("codigo") = codigo_articulo
            Renglon("articulo") = txt_denominacion.Text
            Renglon("precio") = txt_precio.Text.Replace(".", ",")
            Renglon("dto") = txt_descuento.Text.Replace(".", ",")
            'Si es ISP
            If chk_isp.Checked = True Or chk_exento.Checked = True Then
                Renglon("impuesto") = 0
            Else
                Renglon("impuesto") = txt_impuestos.Text.Replace(".", ",")
            End If
            Dim cantidad_precio As Decimal = CDec(txt_cantidad.Text.Replace(".", ",")) * CDec(txt_precio.Text.Replace(".", ","))
            Dim descuento As Decimal = (cantidad_precio * CDec(txt_descuento.Text.Replace(".", ","))) / 100
            Dim impuesto As Decimal = ((cantidad_precio - descuento) * CDec(txt_impuestos.Text)) / 100
            Renglon("total") = Math.Round((cantidad_precio - descuento) + impuesto, 2)
            tabla_detalles_facturas.Rows.Add(Renglon)

            'Asigno
            GV_detalles_factura.DataSource = tabla_detalles_facturas
            GV_detalles_factura.DataBind()

            'Limpio
            txt_cantidad.Text = Nothing
            txt_codigo.Text = Nothing
            txt_denominacion.Text = Nothing
            txt_precio.Text = Nothing
            txt_descuento.Text = Nothing

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

            Else

                'Asigno
                PL_total.Visible = True
                btn_grabar.Visible = True

                'Refrecar los totales
                refrescar_totales()

            End If

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
                txt_codigo.Text = GV_detalles_factura.DataKeys(index).Item("codigo").ToString()
                txt_denominacion.Text = GV_detalles_factura.DataKeys(index).Item("articulo").ToString()
                txt_precio.Text = GV_detalles_factura.DataKeys(index).Item("precio").ToString()
                txt_descuento.Text = GV_detalles_factura.DataKeys(index).Item("dto").ToString()
                txt_impuestos.Text = GV_detalles_factura.DataKeys(index).Item("impuesto").ToString()
                '                DDL_tipo_impuestos.SelectedIndex = DDL_tipo_impuestos.Items.IndexOf(DDL_tipo_impuestos.Items.FindByValue(GV_detalles_factura.DataKeys(index).Item("impuesto").ToString()))

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

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_asientos_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_asientos_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
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

            'Actualizo
            GV_detalles_factura.DataSource = tabla_detalles_facturas
            GV_detalles_factura.DataBind()

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

            'Comprobar si el total esta en negativo o no
            If CDec(lbl_total.Text.Replace(" €", "")) < 0 Then

                'Poner en negativo los detalles
                Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

                'Recorro
                For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                    'Asigno
                    tabla_detalles_facturas.Rows(x)("precio") = tabla_detalles_facturas.Rows(x)("precio") * -1
                    tabla_detalles_facturas.Rows(x)("total") = tabla_detalles_facturas.Rows(x)("total") * -1

                Next

                'Asigno
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

            End If

            'Asigno
            txt_fecha.Text = DateTime.Now.ToShortDateString
            txt_factura.Text = Nothing
            txt_albaran.Text = Nothing
            txt_presupuesto.Text = Nothing
            LB_imprimir.Visible = False
            LB_email.Visible = False
            btn_grabar.Visible = True
            PL_cabecera.Enabled = True
            PL_opciones.Enabled = True
            PL_detalles.Enabled = True
            PL_total.Enabled = True

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_duplicar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_duplicar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub img_abonar_Click(sender As Object, e As ImageClickEventArgs) Handles img_abonar.Click


        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            LT_mensaje_abonar.Text = "¿Está seguro de Abonar esta Factura?"

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_abonar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_abonar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_abonar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_abonar_confirmar_Click(sender As Object, e As EventArgs) Handles btn_abonar_confirmar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Declaro
            Dim cod_cliente As Integer = 0
            Dim n_abono As String = Nothing

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Declaro
            If txt_cod_cliente.Text <> "" Then
                cod_cliente = txt_cod_cliente.Text
            End If

            'Poner en negativo los detalles
            Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

            'Recorro
            For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                'Asigno
                tabla_detalles_facturas.Rows(x)("precio") = tabla_detalles_facturas.Rows(x)("precio") * -1
                tabla_detalles_facturas.Rows(x)("total") = tabla_detalles_facturas.Rows(x)("total") * -1

            Next

            'Asigno
            GV_detalles_factura.DataSource = tabla_detalles_facturas
            GV_detalles_factura.DataBind()

            'Obtengo un nuevo numero de factura para el método elegido para la empresa
            Select Case tabla_empresa.Rows(0)("tipo_autonumerico_asientos")
                Case "normal" : n_abono = funciones_globales.numero_abono(tabla_empresa.Rows(0)("ruta_base_datos"))
            End Select

            'Asigno
            txt_factura.Text = n_abono

            'Grabo la Cabecera de la Factura
            funciones_globales.grabar_facturas_(tabla_empresa.Rows(0)("ruta_base_datos"), n_abono, txt_fecha.Text, cod_cliente, txt_cliente.Text, txt_nif.Text, txt_direccion.Text, txt_cp.Text, txt_poblacion.Text, txt_provincia.Text, GV_detalles_factura, GV_totales, "Abono", txt_presupuesto.Text, txt_albaran.Text, chk_isp.Checked, chk_exento.Checked)

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Creó el Abono: " & txt_factura.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Asigno
            LB_imprimir.Visible = True
            LB_email.Visible = True
            btn_grabar.Visible = False
            PL_cabecera.Enabled = False
            PL_detalles.Enabled = False
            PL_total.Enabled = False
            img_abonar.Visible = False

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Abono creado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_abonar_confirmar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_abonar_confirmar_Click: " & ex.Message.Replace("'", " ") & "');", True)
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
            LT_mensaje_confirmar.Text = "¿Está seguro de continuar y generar la Factura?"

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
            Dim n_factura As String = Nothing
            Dim n_presupuesto As String = txt_presupuesto.Text
            Dim n_albaran As String = txt_albaran.Text
            Dim origen As String = "Factura"

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Declaro
            If txt_cod_cliente.Text <> "" Then
                cod_cliente = txt_cod_cliente.Text
            End If

            'Obtengo un nuevo numero de factura para el método elegido para la empresa
            Select Case tabla_empresa.Rows(0)("tipo_autonumerico_asientos")
                Case "normal" : n_factura = funciones_globales.numero_factura(tabla_empresa.Rows(0)("ruta_base_datos"))
            End Select

            'Asigno
            txt_factura.Text = n_factura

            'Si viene con numero de presupuesto
            If txt_albaran.Text <> "" Then
                'Asigno
                origen = "Albaran"
            End If

            'Si viene con numero de presupuesto
            If txt_presupuesto.Text <> "" Then
                'Asigno
                origen = "Presupuesto"
            End If

            'Grabo la Cabecera de la Factura
            funciones_globales.grabar_facturas_(tabla_empresa.Rows(0)("ruta_base_datos"), n_factura, txt_fecha.Text, cod_cliente, txt_cliente.Text, txt_nif.Text, txt_direccion.Text, txt_cp.Text, txt_poblacion.Text, txt_provincia.Text, GV_detalles_factura, GV_totales, origen, n_presupuesto, n_albaran, chk_isp.Checked, chk_exento.Checked)

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Creó la Factura: " & txt_factura.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Asigno
            LB_imprimir.Visible = True
            LB_email.Visible = True
            btn_grabar.Visible = False
            PL_cabecera.Enabled = False
            PL_opciones.Enabled = False
            PL_detalles.Enabled = False
            PL_total.Enabled = False

            'Actualizo el albarán
            If txt_albaran.Text <> "" Then

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "abrir_ventana", "ok('Factura creada correctamente.');$('#ialtas_____',window.parent.document).attr('src',$('#ialtas_____',window.parent.document).attr('src'));", True)

            Else

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Factura creada correctamente.');", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_confirmar", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_confirmar: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub LB_imprimir_Click(sender As Object, e As EventArgs) Handles LB_imprimir.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Obtengo el nombre del fichero creado PDF
            Dim nombre_fichero As String = crear_pdf_salida(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), txt_factura.Text)

            'Obtengo el nombre del fichero Firmado
            Dim nombre_firmado As String = firmar_pdf(nombre_fichero)

            'Paso la ruta al iframe
            ver_factura.Attributes("src") = "..\temp\" & nombre_firmado

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", funciones_globales.modal_register("$('#modal_visor').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "LB_imprimir_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error LB_imprimir_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Function crear_pdf_salida(ByVal ruta_base_datos As String, ByVal id_usuario As Integer, ByVal n_factura As String) As String

        'Obtengo
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Genero el nombre del fichero
        Dim nombre As String = "Factura_" & n_factura & "_" & id_usuario & "_" & Date.Now.ToShortDateString.Replace("/", "") & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & Date.Now.Millisecond & ".pdf"

        'Creo la carpeta para contener el informe temporalmente.
        If Not System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("..") & "\temp") Then
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("..") & "\temp")
        End If

        'Creo una session para el constructor de PDF
        HttpContext.Current.Session("factura") = n_factura & "|" & ruta_base_datos

        'Comienzo la generacion del PDF
        Dim oDoc As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4)
        oDoc.SetMargins(funciones_globales.puntos(0), funciones_globales.puntos(0), funciones_globales.puntos(0), funciones_globales.puntos(40))
        Dim pdfw As iTextSharp.text.pdf.PdfWriter
        Dim cb As iTextSharp.text.pdf.PdfContentByte
        Dim linea As iTextSharp.text.pdf.PdfContentByte
        Dim fuente As iTextSharp.text.pdf.BaseFont
        Dim fuente2 As iTextSharp.text.pdf.BaseFont
        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

        'Genero el PDF
        pdfw = iTextSharp.text.pdf.PdfWriter.GetInstance(oDoc, New FileStream(HttpContext.Current.Server.MapPath("..") & "\temp\" & nombre, FileMode.Create, FileAccess.Write, FileShare.None))

        Dim ev As New itsEvents
        pdfw.PageEvent = ev

        'Apertura del documento.
        oDoc.Open()
        cb = pdfw.DirectContent
        linea = pdfw.DirectContent

        'Instanciamos el objeto para el tipo de letra. 
        fuente = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont
        fuente2 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.BOLD).BaseFont

        'Obtengo
        Dim tabla_cabecera_factura As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & ruta_base_datos & "].[dbo].cabecera_facturas WHERE n_factura='" & n_factura & "';")
        Dim tabla_detalles_factura As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & ruta_base_datos & "].[dbo].detalles_facturas WHERE n_factura='" & n_factura & "';")

        'Declaro
        Dim tabla_detalles As New iTextSharp.text.pdf.PdfPTable(6)
        Dim f = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 255))

        tabla_detalles.TotalWidth = funciones_globales.puntos(190)
        tabla_detalles.LockedWidth = True
        tabla_detalles.SetWidths({funciones_globales.puntos(20), funciones_globales.puntos(140), funciones_globales.puntos(25), funciones_globales.puntos(25), funciones_globales.puntos(25), funciones_globales.puntos(25)})

        'Recorro
        For x = 0 To tabla_detalles_factura.Rows.Count - 1

            'Declaro
            Dim cell As iTextSharp.text.pdf.PdfPCell

            'Creo la primera columna
            If tabla_detalles_factura.Rows(x)("cantidad") = 0 And tabla_detalles_factura.Rows(x)("precio") = 0 And tabla_detalles_factura.Rows(x)("dto_1") = 0 Then
                cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else

                'Si el codigo del articulo es 0 no pintes nada
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

            'Creo la segunda columna
            cell = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(tabla_detalles_factura.Rows(x)("denominacion"), f))
            cell.HorizontalAlignment = 3
            cell.Border = 0
            cell.BorderWidthTop = 1
            cell.BorderColorTop = New iTextSharp.text.pdf.CMYKColor(0, 0, 0, 75)
            tabla_detalles.AddCell(cell)

            'Creo la tercera columna
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

            'Creo la cuarta columna
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

            'Creo la quinta columna
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

            'Creo la sexta columna
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

        Next

        'Lo agrego a la pagina
        oDoc.Add(tabla_detalles)

        'Insertamos la imagen de fondo
        imagendemo = iTextSharp.text.Image.GetInstance("D:\imagenes_usuarios_facturacion\" & ruta_base_datos & "\factura\pie_factura.png") 'Dirreccion a la imagen que se hace referencia
        imagendemo.SetAbsolutePosition(0, 0) 'Posicion en el eje cartesiano
        imagendemo.ScaleAbsoluteWidth(funciones_globales.puntos(210)) 'Ancho de la imagen
        imagendemo.ScaleAbsoluteHeight(funciones_globales.puntos(50)) 'Altura de la imagen
        oDoc.Add(imagendemo) ' Agrega la imagen al documento

        'Obtengo
        Dim tabla_pie_factura As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & ruta_base_datos & "].[dbo].pie_facturas WHERE n_factura='" & n_factura & "';")
        Dim total_factura As Decimal = 0

        'Si tengo activado ISP
        If tabla_cabecera_factura.Rows(0)("isp") = True Then

            'Iniciamos el flujo de bytes.             
            cb.BeginText()

            'Tipo Letra y tamaño
            cb.SetFontAndSize(fuente, 10)

            'ISP
            cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, tabla_empresa.Rows(0)("texto_isp").ToString, funciones_globales.puntos(15), funciones_globales.puntos(55), 0)

            'Fin del flujo de bytes.             
            cb.EndText()

        End If

        'Si tengo activado ISP
        If tabla_cabecera_factura.Rows(0)("exento") = True Then

            'Iniciamos el flujo de bytes.             
            cb.BeginText()

            'Tipo Letra y tamaño
            cb.SetFontAndSize(fuente, 10)

            'ISP
            cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, tabla_empresa.Rows(0)("texto_exento").ToString, funciones_globales.puntos(15), funciones_globales.puntos(55), 0)

            'Fin del flujo de bytes.             
            cb.EndText()

        End If

        'Declaro
        Dim tabla_totales As New iTextSharp.text.pdf.PdfPTable(4)
        tabla_totales.TotalWidth = funciones_globales.puntos(82)
        tabla_totales.LockedWidth = True

        'Recorro
        For x = 0 To tabla_pie_factura.Rows.Count - 1

            'Declaro
            Dim cell_totales As iTextSharp.text.pdf.PdfPCell

            'Creo la primera columna
            cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("base_imponible"), 2), f))
            cell_totales.HorizontalAlignment = 1
            cell_totales.Border = 0
            tabla_totales.AddCell(cell_totales)

            'Creo la primera columna
            'Si tengo activado ISP
            If tabla_cabecera_factura.Rows(0)("isp") = True Or tabla_cabecera_factura.Rows(0)("exento") = True Then
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("porcentaje"), 2), f))
            End If
            cell_totales.HorizontalAlignment = 1
            cell_totales.Border = 0
            tabla_totales.AddCell(cell_totales)

            'Creo la primera columna
            If tabla_cabecera_factura.Rows(0)("isp") = True Or tabla_cabecera_factura.Rows(0)("exento") = True Then
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase("", f))
            Else
                cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("cuota"), 2), f))
            End If
            cell_totales.HorizontalAlignment = 1
            cell_totales.Border = 0
            tabla_totales.AddCell(cell_totales)

            'Creo la primera columna
            cell_totales = New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(FormatNumber(tabla_pie_factura.Rows(x)("total"), 2), f))
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

    Function firmar_pdf(ByVal nombre_fichero As String) As String

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Ruta al PDF y al archivo PFX
        Dim pdfOriginalPath As String = Server.MapPath("..") & "\temp\" & nombre_fichero
        Dim pfxFilePath As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\certificado\certificado.pfx"
        Dim pfxPassword As String = tabla_empresa.Rows(0)("certificado_password")
        Dim pdfFirmadoPath As String = Server.MapPath("..") & "\temp\" & nombre_fichero.Replace(".pdf", "_firmado.pdf")

        'Load the existing PDF into a reader
        Dim pdfReader As New PdfReader(pdfOriginalPath)

        'Initialize a new PKCS#12 key store (used for handling the PFX certificate)
        Dim Pkcs12StoreBuilder As New Pkcs12StoreBuilder()
        Dim pfxKeyStore As Pkcs12Store = Pkcs12StoreBuilder.Build()

        'Load the certificate and private key from the PFX file
        Using pfxStream As New FileStream(pfxFilePath, FileMode.Open, FileAccess.Read)

            'Load into the key store using the provided password
            pfxKeyStore.Load(pfxStream, pfxPassword.ToCharArray())

        End Using

        'Create a PdfStamper that enables signing and appends the signature to the document
        Dim pdfStamper As PdfStamper = PdfStamper.CreateSignature(pdfReader, New FileStream(pdfFirmadoPath, FileMode.Create), ControlChars.NullChar, Nothing, True)

        'Access the signature appearance settings
        Dim signatureAppearance As PdfSignatureAppearance = pdfStamper.SignatureAppearance

        'Add optional metadata (shows up in PDF signature details)
        If Not IsDBNull(tabla_empresa.Rows(0)("certificado_motivo")) Then
            Dim reason As String = tabla_empresa.Rows(0)("certificado_motivo")  '"Digital Signature Reason"
            signatureAppearance.Reason = reason
        End If

        If Not IsDBNull(tabla_empresa.Rows(0)("certificado_localizacion")) Then
            Dim location As String = tabla_empresa.Rows(0)("certificado_localizacion")
            signatureAppearance.Location = location
        End If
        signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION

        'Position the visible signature on the page (x, y, width, height in points)
        Dim x As Decimal = tabla_empresa.Rows(0)("certificado_posicion_x") * 2
        Dim y As Decimal = 842 - 90 - (tabla_empresa.Rows(0)("certificado_posicion_y") * 2)
        Dim w As Decimal = 189 + x 'tabla_empresa.Rows(0)("certificado_posicion_y")
        Dim h As Decimal = 79 + y 'tabla_empresa.Rows(0)("certificado_posicion_y")
        signatureAppearance.Acro6Layers = False ' Use compact signature appearance
        'signatureAppearance.Layer4Text = PdfSignatureAppearance.CERTIFIED_NO_CHANGES_ALLOWED  ' Custom label text
        signatureAppearance.SetVisibleSignature(New iTextSharp.text.Rectangle(x, y, w, h), 1, "signature")

        'Find the first alias in the PFX that has a private key entry
        Dim [alias] As String = pfxKeyStore.Aliases.Cast(Of String)().FirstOrDefault(Function(entryAlias) pfxKeyStore.IsKeyEntry(entryAlias))

        'Ensure a valid alias (certificate) was found
        If [alias] IsNot Nothing Then

            'Retrieve the private key for signing
            Dim privateKey As ICipherParameters = pfxKeyStore.GetKey([alias]).Key

            'Create a signer using SHA-256 and the private key
            Dim pks As IExternalSignature = New PrivateKeySignature(privateKey, DigestAlgorithms.SHA256)

            'Perform the digital signing operation using CMS format
            MakeSignature.SignDetached(signatureAppearance, pks, New Org.BouncyCastle.X509.X509Certificate() {pfxKeyStore.GetCertificate([alias]).Certificate}, Nothing, Nothing, Nothing, 0, CryptoStandard.CMS)

        Else

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Llave privada no encontrado en el Certificado');", True)

        End If

        ' Close the stamper to save and finalize the signed PDF
        pdfStamper.Close()
        pdfStamper.Dispose()
        pdfReader.Dispose()

        'Devulevo
        Return nombre_fichero.Replace(".pdf", "_firmado.pdf")

    End Function

    Protected Sub Lkb_refrecar_Click(sender As Object, e As EventArgs) Handles Lkb_refrecar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Reinicio el sistema
            Response.Redirect("altas.aspx?id_usuario=" & Request.QueryString("id_usuario") & "&id_empresa=" & Request.QueryString("id_empresa"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Lkb_refrecar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Lkb_refrecar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

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
            txt_mensaje.Text = Nothing
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
            Dim nombre_fichero As String = crear_pdf_salida(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), txt_factura.Text)

            'Obtengo el nombre del fichero Firmado
            Dim nombre_firmado As String = firmar_pdf(nombre_fichero)

            'Asigno
            txt_nombre_fichero_email.Text = nombre_firmado

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
            correo.Subject = "Factura Nº: " & txt_factura.Text & "."

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
            "Te adjuntamos la factura solicitada.</span> " &
            "</td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style = 'text-align:center; padding:30px;' align='center'>" & txt_mensaje.Text &
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
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Altas", "Envió la Factura: " & txt_factura.Text & " al E-mail: " & txt_destinatario.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Email enviado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_confirmar_email_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_confirmar_email_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub chk_isp_CheckedChanged(sender As Object, e As EventArgs) Handles chk_isp.CheckedChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_articulos")

        Try

            'Creo un Datatable
            Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

            If chk_isp.Checked = True Then

                'Asigno
                chk_exento.Checked = False

                'Recorro
                For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                    'Asigno
                    tabla_detalles_facturas.Rows(x)("impuesto") = 0

                Next

                'Asigno
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

            Else

                'Recorro
                For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                    'Asigno
                    tabla_detalles_facturas.Rows(x)("impuesto") = funciones_globales.buscar_datos_tabla(tabla_articulos, "id", tabla_detalles_facturas.Rows(x)("codigo"), "impuesto")

                Next

                'Asigno
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "chk_isp_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error chk_isp_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub chk_exento_CheckedChanged(sender As Object, e As EventArgs) Handles chk_exento.CheckedChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
        Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_articulos")

        Try

            'Creo un Datatable
            Dim tabla_detalles_facturas As DataTable = LlenarDataTableDesdeGridView(GV_detalles_factura)

            If chk_exento.Checked Then

                'Asigno
                chk_isp.Checked = False

                'Recorro
                For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                    'Asigno
                    tabla_detalles_facturas.Rows(x)("impuesto") = 0

                Next

                'Asigno
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

            Else

                'Recorro
                For x = 0 To tabla_detalles_facturas.Rows.Count - 1

                    'Asigno
                    tabla_detalles_facturas.Rows(x)("impuesto") = funciones_globales.buscar_datos_tabla(tabla_articulos, "id", tabla_detalles_facturas.Rows(x)("codigo"), "impuesto")

                Next

                'Asigno
                GV_detalles_factura.DataSource = tabla_detalles_facturas
                GV_detalles_factura.DataBind()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "chk_exento_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error chk_exento_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
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
    Dim sesion_factura() As String = HttpContext.Current.Session("factura").split("|")

    Public Overrides Sub OnStartPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)

        'Declaro
        Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen
        Dim cb As iTextSharp.text.pdf.PdfContentByte
        Dim fuente As iTextSharp.text.pdf.BaseFont

        'Insertamos la imagen de fondo
        imagendemo = iTextSharp.text.Image.GetInstance("D:\imagenes_usuarios_facturacion\" & sesion_factura(1) & "\factura\fondo_factura.png") 'Dirreccion a la imagen que se hace referencia
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
        Dim tabla_cabecera_factura As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & sesion_factura(1) & "].[dbo].cabecera_facturas WHERE n_factura='" & sesion_factura(0) & "';")

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
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, tabla_cabecera_factura.Rows(0)("n_factura").ToString.ToUpper, funciones_globales.puntos(23), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, tabla_cabecera_factura.Rows(0)("n_albaran").ToString.ToUpper, funciones_globales.puntos(51), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, tabla_cabecera_factura.Rows(0)("n_presupuesto").ToString.ToUpper, funciones_globales.puntos(77), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, Mid(tabla_cabecera_factura.Rows(0)("fecha").ToString.ToUpper, 1, 10), funciones_globales.puntos(100), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, "1", funciones_globales.puntos(193), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición  

        'Fin del flujo de bytes.             
        cb.EndText()

    End Sub

End Class