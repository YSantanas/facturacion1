Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data
Imports System.IO
Imports System.Drawing.Imaging

Partial Class gestion_consulta_gestion
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales
    Dim color_verde As Color = System.Drawing.Color.FromArgb(21, 87, 36)
    Dim color_blanco As Color = System.Drawing.Color.FromArgb(255, 255, 255)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            'Asigno el n_consulta
            gridview_consulta.PageSize = parametros_empresa(50)

            'Parametros de empresa para ocultar campos
            If parametros_empresa(17) = True Then
                PH_referencia.Visible = True
                PH_referencia_nombre.Visible = True
            Else
                PH_referencia.Visible = False
                PH_referencia_nombre.Visible = False
            End If

            If parametros_empresa(18) = True Then
                PH_serie.Visible = True
                PH_serie_nombre.Visible = True
            Else
                PH_serie.Visible = False
                PH_serie_nombre.Visible = False
            End If

            If parametros_empresa(19) = True Then
                PH_factura.Visible = True
                PH_factura_nombre.Visible = True
            Else
                PH_factura.Visible = False
                PH_factura_nombre.Visible = False
            End If

            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Asigno el maximo valor para una cuenta de esa empresa
                txt_cuenta.MaxLength = parametros_empresa(7)

                'Cargo el mes de cierre
                lbl_fechas_proceso.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(MonthName(parametros_empresa(12)))

                'Parametro resultado estricto
                CB_estricto.Checked = parametros_empresa(49)

                'Carga la fecha desde el dia 1 del mes actual
                txt_fecha.Text = DateTime.Now.ToString("01/MM/yyyy")

                'Asigno
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Asigno", "setTimeout(function () { $('#txt_fecha').focus();}, 100);", True)

                'Si viene con parámetros
                If Session("numero_consulta_gestion") <> Nothing Then

                    'Descompongo los valores
                    Dim vector() As String = Session("numero_consulta_gestion").split("|")

                    'Asigno
                    txt_fecha.Text = vector(0)
                    txt_numero.Text = vector(1)

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Click Buscar", "$('#btn_consultar').click();", True)

                    'Destruyo
                    Session.Remove("numero_consulta_gestion")

                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_consultar_Click(sender As Object, e As EventArgs) Handles btn_consultar.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

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
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Consultar Gestión Documental", "Solicitó listado Fecha: " & txt_fecha.Text & ", Nº Asiento: " & txt_numero.Text & ", Referencia: " & txt_referencia.Text & ", Cuenta: " & txt_cuenta.Text & " , Serie: " & txt_serie.Text & " , Factura: " & txt_factura.Text & " , Concepto: " & txt_concepto.Text & " , Importe: " & txt_importe.Text & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_consultar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_consultar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_fechas(ByVal ejercicio As Integer)

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Calculo la fecha final
            txt_fecha_final.Text = funciones_globales.fecha_final(parametros_empresa(12), ejercicio, parametros_empresa(12))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_fechas", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_fechas: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub cargar_GV()

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")
        Dim query_filtros As String = Nothing

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                'Compongo la SELECT
                Dim query As String = "SELECT ficheros.id_cabecera_impuesto, " &
                "ficheros.id_cabecera_asiento, " &
                "ficheros.fecha_creacion, " &
                "ficheros.hora_creacion, " &
                "ficheros.ruta, " &
                "ficheros.nombre_fichero, " &
                "ficheros.size, " &
                "ficheros.observaciones, " &
                "cabecera_asientos.fecha, " &
                "cabecera_asientos.referencia_asiento, " &
                "detalles_asientos.cuenta, " &
                "detalles_asientos.denominacion_cuenta, " &
                "detalles_asientos.concepto, " &
                "detalles_asientos.factura, " &
                "detalles_asientos.importe_final " &
                "FROM [ficheros] INNER JOIN " &
                "cabecera_asientos ON ficheros.id_cabecera_asiento=cabecera_asientos.id INNER JOIN " &
                "detalles_asientos ON cabecera_asientos.id=detalles_asientos.Id_cabecera_asientos "

                Dim filtro As String = Nothing
                Dim orden As String = Nothing

                'Si elijo Fecha
                If txt_fecha.Text <> "" Then

                    'Extraigo el año para poder obtener las fechas del ejercicio contable
                    Dim descomponer_fecha As Date = txt_fecha.Text

                    'Obtengo los valores de fecha inicial y final del ejercicio
                    leer_fechas(Year(descomponer_fecha))

                    'Estricto
                    If CB_estricto.Checked = True Then
                        filtro += "AND fecha between '" & txt_fecha.Text & "' AND '" & txt_fecha.Text & "' "
                    Else
                        filtro += "AND fecha between '" & descomponer_fecha & "' AND '" & txt_fecha_final.Text & "' "
                    End If
                    orden = "ORDER BY fecha,referencia_asiento,nombre_fichero"

                Else

                    filtro += "AND fecha between '01/01/2008' AND '31/12/2999' "
                    orden = "ORDER BY fecha,referencia_asiento,nombre_fichero"

                End If

                'Si elijo NºAsiento
                If txt_numero.Text <> "" Then

                    'Estricto
                    If CB_estricto.Checked = True Then
                        filtro += "AND referencia_asiento = @txt_numero "
                        orden = "ORDER BY fecha,referencia_asiento,nombre_fichero"
                    Else
                        filtro += "AND referencia_asiento LIKE @txt_numero + '%' "
                        orden = "ORDER BY fecha, referencia_asiento,nombre_fichero"
                    End If

                End If

                'Si elijo Referencia
                If txt_referencia.Text <> "" Then
                    filtro += "AND referencia LIKE @txt_referencia + '%' "
                    orden = "ORDER BY referencia,fecha,referencia_asiento,nombre_fichero"
                End If

                'Si elijo Cuenta
                If txt_cuenta.Text <> "" Then

                    If IsNumeric(txt_cuenta.Text) = True Then

                        'Estricto
                        If CB_estricto.Checked = True Then
                            filtro += "AND cuenta = @txt_cuenta "
                            orden = "ORDER BY cuenta,fecha,referencia_asiento,nombre_fichero"
                        Else
                            filtro += "AND cuenta >= @txt_cuenta "
                            orden = "ORDER BY cuenta,fecha,referencia_asiento,nombre_fichero"
                        End If

                    Else

                        filtro += "AND denominacion_cuenta LIKE @txt_cuenta + '%' "
                        orden = "ORDER BY denominacion_cuenta,fecha,referencia_asiento,nombre_fichero"

                    End If

                End If

                'Si elijo Serie
                If txt_serie.Text <> "" Then
                    filtro += "AND serie LIKE @txt_serie + '%' "
                    orden = "ORDER BY serie,fecha,referencia_asiento,nombre_fichero"
                End If

                'Si elijo Factura
                If txt_factura.Text <> "" Then
                    filtro += "AND factura LIKE @txt_factura +  '%' "
                    orden = "ORDER BY factura,fecha,referencia_asiento,nombre_fichero"
                End If

                'Si elijo Concepto
                If txt_concepto.Text <> "" Then
                    filtro += "AND concepto LIKE '%' + @txt_concepto + '%' "
                    orden = "ORDER BY fecha,referencia_asiento,nombre_fichero"
                End If

                'Si elijo Importe
                If txt_importe.Text <> "" Then

                    'Estricto
                    If CB_estricto.Checked = True Then
                        filtro += "AND importe_final= @txt_importe "
                        orden = "ORDER BY importe_final,fecha,cuenta,referencia_asiento,nombre_fichero"
                    Else
                        filtro += "AND importe_final>= @txt_importe "
                        orden = "ORDER BY importe_final,fecha,cuenta,referencia_asiento,nombre_fichero"
                    End If

                End If

                'Si elijo observaciones
                If txt_observaciones.Text <> "" Then
                    filtro += "AND observaciones LIKE '%' + @txt_observaciones + '%' "
                    orden = "ORDER BY observaciones,fecha,cuenta,referencia_asiento,nombre_fichero"
                End If

                Dim memComando As New SqlCommand
                memComando.CommandText = query & filtro & orden
                memComando.Parameters.Add("@txt_numero", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_numero").Value = txt_numero.Text
                memComando.Parameters.Add("@txt_referencia", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_referencia").Value = txt_referencia.Text
                memComando.Parameters.Add("@txt_cuenta", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_cuenta").Value = txt_cuenta.Text
                memComando.Parameters.Add("@txt_serie", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_serie").Value = txt_serie.Text
                memComando.Parameters.Add("@txt_factura", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_factura").Value = txt_factura.Text
                memComando.Parameters.Add("@txt_concepto", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_concepto").Value = txt_concepto.Text
                memComando.Parameters.Add("@txt_importe", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_importe").Value = txt_importe.Text
                memComando.Parameters.Add("@txt_observaciones", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@txt_observaciones").Value = txt_observaciones.Text
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

            'Creo mi datatable y columnas
            Dim tabla_consulta_gestion As New DataTable
            tabla_consulta_gestion.Columns.Add("id_cabecera_impuesto")
            tabla_consulta_gestion.Columns.Add("id_cabecera_asiento")
            tabla_consulta_gestion.Columns.Add("fecha_creacion")
            tabla_consulta_gestion.Columns.Add("hora_creacion")
            tabla_consulta_gestion.Columns.Add("ruta")
            tabla_consulta_gestion.Columns.Add("nombre_fichero")
            tabla_consulta_gestion.Columns.Add("size")
            tabla_consulta_gestion.Columns.Add("observaciones")
            tabla_consulta_gestion.Columns.Add("fecha")
            tabla_consulta_gestion.Columns.Add("referencia_asiento")
            tabla_consulta_gestion.Columns.Add("cuenta")
            tabla_consulta_gestion.Columns.Add("denominacion_cuenta")
            tabla_consulta_gestion.Columns.Add("concepto")
            tabla_consulta_gestion.Columns.Add("factura")
            tabla_consulta_gestion.Columns.Add("importe_final")

            'Variables
            Dim nombre_fichero_anterior As String = Nothing

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                If tabla_consulta.Rows(x)("nombre_fichero") <> nombre_fichero_anterior Then

                    'Inserto los datos en la tabla temporal
                    Dim Renglon As DataRow = tabla_consulta_gestion.NewRow()
                    Renglon("id_cabecera_impuesto") = tabla_consulta.Rows(x)("id_cabecera_impuesto")
                    Renglon("id_cabecera_asiento") = tabla_consulta.Rows(x)("id_cabecera_asiento")
                    Renglon("fecha_creacion") = Mid(tabla_consulta.Rows(x)("fecha_creacion").ToString, 1, 10)
                    Renglon("hora_creacion") = tabla_consulta.Rows(x)("hora_creacion")
                    Renglon("ruta") = tabla_consulta.Rows(x)("ruta")
                    Renglon("nombre_fichero") = tabla_consulta.Rows(x)("nombre_fichero")
                    Renglon("size") = tabla_consulta.Rows(x)("size")
                    Renglon("observaciones") = tabla_consulta.Rows(x)("observaciones")
                    Renglon("fecha") = Mid(tabla_consulta.Rows(x)("fecha").ToString, 1, 10)
                    Renglon("referencia_asiento") = tabla_consulta.Rows(x)("referencia_asiento")
                    Renglon("cuenta") = tabla_consulta.Rows(x)("cuenta") & " - " & tabla_consulta.Rows(x)("denominacion_cuenta")
                    Renglon("concepto") = tabla_consulta.Rows(x)("concepto")
                    Renglon("factura") = tabla_consulta.Rows(x)("factura")
                    Renglon("importe_final") = tabla_consulta.Rows(x)("importe_final")

                    'Inserto
                    tabla_consulta_gestion.Rows.Add(Renglon)

                    'Igualo
                    nombre_fichero_anterior = tabla_consulta.Rows(x)("nombre_fichero")

                End If

            Next

            'Realizo la consulta
            gridview_consulta.DataSource = tabla_consulta_gestion
            gridview_consulta.DataBind()

            'Liberamos
            tabla_consulta_gestion.Dispose()
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "refrescar_GV", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error refrescar_GV: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim onmouseoverStyle As String = "this.style.backgroundColor='#f2ffe4';this.style.cursor='Default'"
                Dim onmouseoutStyle As String = "this.style.backgroundColor='white'"
                Dim cursoronmouse As String = "this.style.cursor='Default'"

                'Asigno la propiedas                
                e.Row.Attributes.Add("onmouseover", onmouseoverStyle)
                e.Row.Attributes.Add("onmouseout", onmouseoutStyle)

                'DesAsigno la propiedas  
                e.Row.Cells(0).BackColor = ColorTranslator.FromHtml("#FFFFFF")
                e.Row.Cells(1).BackColor = ColorTranslator.FromHtml("#FFFFFF")
                e.Row.Cells(9).BackColor = ColorTranslator.FromHtml("#FFFFFF")
                e.Row.Cells(10).BackColor = ColorTranslator.FromHtml("#FFFFFF")

                'Fecha de imputacion
                Dim btn_ver As ImageButton = DirectCast(e.Row.FindControl("btn_ver"), ImageButton)
                Dim resultado As Integer = Math.Truncate(gridview_consulta.DataKeys(e.Row.RowIndex).Item("size").ToString() / 1024)
                btn_ver.ToolTip = "Fecha de creación: " & Mid(gridview_consulta.DataKeys(e.Row.RowIndex).Item("fecha_creacion").ToString(), 1, 10) & " a las " & Mid(gridview_consulta.DataKeys(e.Row.RowIndex).Item("hora_creacion").ToString(), 1, 10) & " (" & resultado & " Kb)."

                'Restricción para el nivel
                If parametros_usuario(10) = "Invitado" Then
                    Dim btnedit As ImageButton = DirectCast(e.Row.FindControl("btnedit"), ImageButton)
                    btnedit.Enabled = False
                    Dim btndelete As ImageButton = DirectCast(e.Row.FindControl("btndelete"), ImageButton)
                    btndelete.Enabled = False
                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
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

            Dim contador As Integer = 1
            Dim informacion As String = Nothing

            For x = 0 To gridview_consulta.Rows.Count - 1

                'Asigno
                informacion += contador & "|" & gridview_consulta.DataKeys(x).Item("nombre_fichero").ToString() & "|" & gridview_consulta.DataKeys(x).Item("ruta").ToString() & "|" & Math.Round(gridview_consulta.DataKeys(x).Item("size") / 1024, 0) & "&"

                'Sumo
                contador += 1

            Next

            'Asigno valores
            If informacion <> "" Then
                informacion = Mid(informacion, 1, informacion.Count - 1)
            End If
            txt_informacion.Text = informacion
            txt_total.Text = contador - 1

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_asiento_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_asiento_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            Dim onmouseoverStyle As String = "this.style.backgroundColor='#e2e2e2';this.style.cursor='Default'"
            Dim onmouseoutStyle As String = "this.style.backgroundColor='white'"
            Dim cursoronmouse As String = "this.style.cursor='Default'"

            If (e.CommandName = "ver_fichero") Then

                'Reset a blanco
                For x = 0 To gridview_consulta.Rows.Count - 1

                    'For y = 0 To gridview_consulta.Columns.Count - 1
                    If index = x Then
                        gridview_consulta.Rows(x).Attributes.Remove("onmouseover")
                        gridview_consulta.Rows(x).Attributes.Remove("onmouseout")
                        gridview_consulta.Rows(index).BackColor = color_verde
                        gridview_consulta.Rows(index).ForeColor = color_blanco
                    Else
                        gridview_consulta.Rows(x).Attributes.Add("onmouseover", onmouseoverStyle)
                        gridview_consulta.Rows(x).Attributes.Add("onmouseout", onmouseoutStyle)
                        gridview_consulta.Rows(x).BackColor = Nothing
                        gridview_consulta.Rows(x).ForeColor = Color.Black
                    End If

                Next

                'Asigno
                txt_actual.Text = index + 1

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "visor();", True)

            End If

            If (e.CommandName = "ver_apuntes") Then

                'Creamos una session para indicarle a asientos que se habra
                Session("numero_asiento") = gridview_consulta.DataKeys(index).Item("id_cabecera_asiento").ToString()

                'Reset a blanco
                For x = 0 To gridview_consulta.Rows.Count - 1

                    'For y = 0 To gridview_consulta.Columns.Count - 1
                    If index = x Then
                        gridview_consulta.Rows(x).Attributes.Remove("onmouseover")
                        gridview_consulta.Rows(x).Attributes.Remove("onmouseout")
                        gridview_consulta.Rows(index).BackColor = color_verde
                        gridview_consulta.Rows(index).ForeColor = color_blanco
                    Else
                        gridview_consulta.Rows(x).Attributes.Add("onmouseover", onmouseoverStyle)
                        gridview_consulta.Rows(x).Attributes.Add("onmouseout", onmouseoutStyle)
                        gridview_consulta.Rows(x).BackColor = Nothing
                        gridview_consulta.Rows(x).ForeColor = Color.Black
                    End If

                Next

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "refrescar_ventana", "$('#ientradas',window.parent.document).attr('src',$('#ientradas',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('entradas','description','1000','600','asientos/asientos.aspx','1');", True)

            End If

            If (e.CommandName = "borrar") Then

                'Mensaje
                LT_mensaje_eliminar.Text = "¿Está seguro de eliminar el fichero: " & gridview_consulta.DataKeys(index).Item("nombre_fichero") & " del Asiento: " & gridview_consulta.DataKeys(index).Item("referencia_asiento") & "?"
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#modal_eliminar').modal('show');", True)

            End If

            If (e.CommandName = "editar") Then

                'Asigno
                lbl_titulo.Text = "Observaciones"
                txt_observacion.Text = gridview_consulta.DataKeys(index).Item("observaciones").ToString()
                btn_modificar.Visible = True
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "setTimeout(function () { $('#txt_denominacion').focus();$('#txt_denominacion').select();}, 100); $('#modal_agregar').modal('show');", True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridview_consulta.PageIndexChanging

        Try

            'Actualizar posicion y cosnulta
            gridview_consulta.PageIndex = e.NewPageIndex
            cargar_GV()

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_exportar_excel_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_exportar_excel_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_excel_Click(sender As Object, e As ImageClickEventArgs) Handles img_exportar_excel.Click

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Gestion", gridview_consulta)

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "xlsx"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_exportar_excel_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_exportar_excel_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_txt_Click(sender As Object, e As ImageClickEventArgs) Handles img_exportar_txt.Click

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Gestion", gridview_consulta)

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "txt"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_exportar_txt_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_exportar_txt_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_borrar_fichero_Click(sender As Object, e As EventArgs) Handles btn_borrar_fichero.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Control de refresco
            Dim sentencia As String = "ok('El Fichero ha sido Borrado con Éxito.');"

            If gridview_consulta.DataKeys(txt_index.Text).Item("id_cabecera_impuesto").ToString() = "0" Then
                'Creamos una session para indicarle a asientos que se habra
                Session("numero_asiento") = gridview_consulta.DataKeys(txt_index.Text).Item("id_cabecera_asiento").ToString()
                sentencia += "$('#ientradas',window.parent.document).attr('src',$('#ientradas',window.parent.document).attr('src'));"
            End If

            If gridview_consulta.DataKeys(txt_index.Text).Item("id_cabecera_impuesto").ToString() <> "0" And gridview_consulta.DataKeys(txt_index.Text).Item("id_cabecera_asiento").ToString() <> "0" Then
                'Creamos una session para indicarle a asientos que se habra
                Session("numero_impuesto") = gridview_consulta.DataKeys(txt_index.Text).Item("id_cabecera_impuesto").ToString()
                sentencia += "$('#ientradas_',window.parent.document).attr('src',$('#ientradas_',window.parent.document).attr('src'));"
            End If

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "DELETE FROM [ficheros] WHERE nombre_fichero='" & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fichero").ToString() & "';"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cierro la BBDD
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Borro el fichero
            System.IO.File.Delete("D:\" & gridview_consulta.DataKeys(txt_index.Text).Item("ruta").ToString() & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fichero").ToString())

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Paso la traza para grabar el Log
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Consultar Gestión Documental", "Eliminó el fichero: " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fichero").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Control para el log
            txt_index.Text = ""

            'Refrescar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_borrado", sentencia, True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_borrar_fichero_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_borrar_fichero_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_modificar_Click(sender As Object, e As EventArgs) Handles btn_modificar.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                'Actualizo
                Dim memComando As New SqlCommand
                memComando.CommandText = "UPDATE [ficheros] SET observaciones=@observaciones WHERE nombre_fichero='" & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fichero").ToString() & "';"
                memComando.Parameters.Add("@observaciones", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@observaciones").Value = txt_observacion.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cierro la BBDD
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Paso la traza para grabar el Log
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Consultar Gestión Documental", "Modificó la observación del fichero: " & gridview_consulta.DataKeys(txt_index.Text).Item("nombre_fichero").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Control para el log
            txt_index.Text = ""

            'Refrescar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Observación modificada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_modificar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_modificar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_derecha_Click(sender As Object, e As ImageClickEventArgs) Handles img_derecha.Click

        Try

            'Giramos la imagen a la derecha 90º
            Dim imagen As System.Drawing.Image
            Dim strFilename As String = "D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString()
            imagen = System.Drawing.Image.FromFile(strFilename)
            imagen.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone)
            imagen.Save("D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString(), ImageFormat.Jpeg)
            imagen.Dispose()

            'Borro la antigua
            File.Delete("D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString())

            'Renombro la nueva
            File.Move("D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString(), "D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString())

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "visor();", True)

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_derecha_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_derecha_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_izquierda_Click(sender As Object, e As ImageClickEventArgs) Handles img_izquierda.Click

        Try

            'Giramos la imagen a la derecha 90º
            Dim imagen As System.Drawing.Image
            Dim strFilename As String = "D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString()
            imagen = System.Drawing.Image.FromFile(strFilename)
            imagen.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipXY)
            imagen.Save("D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString(), ImageFormat.Jpeg)
            imagen.Dispose()

            'Borro la antigua
            File.Delete("D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString())

            'Renombro la nueva
            File.Move("D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString(), "D:\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("ruta").ToString() & "\" & gridview_consulta.DataKeys(txt_actual.Text - 1).Item("nombre_fichero").ToString())

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "visor();", True)

        Catch ex As Exception
            'Cargo las variables de Usuario y Empresa
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_izquierda_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_izquierda_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
