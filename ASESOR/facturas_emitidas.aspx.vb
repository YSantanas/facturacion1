Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Ionic.Zip

Partial Class informes_facturas_emitidas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_empresas()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Asigno
        Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT usuarios.id as id_usuario,usuarios.nombre,usuarios.primer_apellido,usuarios.segundo_apellido,usuarios.email,usuarios.password,usuarios.baja,empresa.id,empresa.codigo_empresa,empresa.nombre_fiscal,empresa.nif,empresa.ruta_base_datos,empresa.Id_tipo_plan_cuentas,empresa.demo,empresa.bienvenida,empresa.fecha_creacion,empresa.servicio_suspendido,usuarios.plan_facturacion,empresa.nombre_comercial,usuarios.nivel,empresa.custodia " &
            "FROM [kernel_facturacion].[dbo].usuarios_empresas INNER JOIN " &
            "[kernel_facturacion].[dbo].usuarios ON usuarios_empresas.id_usuario = usuarios.Id INNER JOIN " &
            "[kernel_facturacion].[dbo].empresa ON usuarios_empresas.id_empresa = empresa.id " &
            "WHERE usuarios.email = '" & tabla_usuario.Rows(0)("email").ToString & "' AND usuarios.password = '" & tabla_usuario.Rows(0)("password").ToString & "' AND nivel='Auditor' " &
            "ORDER BY id;")

        'Asigno para poner clase que cambie de color al seleccionar
        Dim clase As String = "cambiar_primary"

        If tabla_consulta.Rows.Count = 0 Then

            Lt_facturas_emitidas.Text = "<br><table style='width:100%;' border='0' ><tr>" &
              "<td width='94%' align='center'><span style='color: #6c757d; font-size:13px;'>Sin Empresas Asociadas</span></td>" &
              "</tr></table>"

        Else

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                Lt_facturas_emitidas.Text += "<div class='col-md-auto'>" &
                    "<a href='facturas_emitidas.aspx?id_usuario=" & Request.QueryString("id_usuario") & "&id_empresa=" & Request.QueryString("id_empresa") & "&bbdd=" & tabla_consulta.Rows(x)("ruta_base_datos").ToString & "&nombre=" & tabla_consulta.Rows(x)("nombre_fiscal").ToString & "'>" &
                    "<table class='bordes_redondo " & clase & "'>" &
                    "<tr>" &
                        "<td style='text-align:center;'><span class='bi bi-building-down text-black-50' style='font-size:50px;'></span></td>" &
                    "</tr>" &
                    "<tr>" &
                        "<td><span class='text-black-50'>Empresa:</span>" & tabla_consulta.Rows(x)("nombre_fiscal").ToString & "</td>" &
                    "</tr>" &
                    "<tr>" &
                        "<td><span Class='text-black-50'>BBDD:</span>" & tabla_consulta.Rows(x)("ruta_base_datos").ToString & "</td>" &
                    "</tr>" &
                    "</table>" &
                    "</a>" &
                    "<br />" &
                    "</div>"

            Next

        End If

    End Sub

    Sub mostrar_menu(ByVal bbdd As String, ByVal nombre As String)

        'Asigno
        txt_bbdd.Text = bbdd

        'Asigno
        lbl_titulo.Text = "Facturas Emitidas: " & nombre

        'Registro como bloque en local para el jquery
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_mostrar').modal('show');"), True)

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

                'Fecha inicial y final para la consulta
                Dim fecha_inicial As Date = "01/" & (Date.Now).ToString("MM") & "/" & Year(Date.Now)
                txt_fecha_inicial.Text = fecha_inicial
                txt_fecha_final.Text = fecha_inicial.AddMonths(1).AddDays(-1)

                'Leer empresas
                leer_empresas()

                If Request.Params("bbdd") <> "" Then
                    mostrar_menu(Request.Params("bbdd").ToString, Request.Params("nombre").ToString)
                End If

            End If

        Catch ex As Exception
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

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_mostrar').modal('show');"), True)

        Catch ex As Exception
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
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & txt_bbdd.Text)

                'Abrimos conexion
                memConn.Open()

                'Declaro
                Dim query As String = Nothing
                Dim filtro As String = Nothing
                Dim orden As String = Nothing
                Dim memComando As New SqlCommand

                'Compongo la SELECT
                query = "SELECT * FROM cabecera_facturas "
                filtro = "WHERE n_factura<>'' "
                orden = "ORDER BY fecha,n_factura;"

                'Si elijo fecha
                If txt_fecha_inicial.Text <> "" And txt_fecha_final.Text <> "" Then

                    filtro += "AND fecha BETWEEN @fecha_inicial AND @fecha_final "
                    memComando.Parameters.Add("@fecha_inicial", Data.SqlDbType.Date).Value = txt_fecha_inicial.Text
                    memComando.Parameters.Add("@fecha_final", Data.SqlDbType.Date).Value = txt_fecha_final.Text

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

                'Maquillaje
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("n_descargado") > 0 Then

                    'Asigno
                    e.Row.Cells(0).CssClass = "gv_rojo"

                    If gridview_consulta.DataKeys(e.Row.RowIndex).Item("n_descargado") > 1 Then

                        'Asigno
                        e.Row.Cells(0).ToolTip = "Esta factura la has descargado " & gridview_consulta.DataKeys(e.Row.RowIndex).Item("n_descargado") & " veces."

                    End If

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

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Control para menus de exportar
            If gridview_consulta.Rows.Count = 0 Then
                img_exportar_excel.Visible = False
                img_exportar_txt.Visible = False
                img_descargar.Visible = False
                lbl_informacion.Visible = False
            Else
                img_exportar_excel.Visible = True
                img_exportar_txt.Visible = True
                img_descargar.Visible = True
                lbl_informacion.Visible = True
            End If

        Catch ex As Exception
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

                'Obtengo el nombre del fichero creado PDF
                Dim nombre_fichero As String = crear_pdf_salida(txt_bbdd.Text, tabla_usuario.Rows(0)("Id"), gridview_consulta.DataKeys(index).Item("n_factura").ToString())

                'Paso la ruta al iframe
                ver_factura.Attributes("src") = "..\temp\" & nombre_fichero

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", funciones_globales.modal_register("$('#modal_mostrar').modal('show');$('#modal_visor').modal('show');"), True)

            End If

        Catch ex As Exception
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
            Dim nombre As String = funciones_globales.crear_excel("Informe Facturas Emitidas", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "xlsx"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
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
            Dim nombre As String = funciones_globales.crear_txt("Informe Facturas Emitidas", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "txt"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_exportar_txt_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub img_descargar_Click(sender As Object, e As EventArgs) Handles img_descargar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Declaro
            Dim nombre_zip As String = Nothing

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
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "error('No has seleccionado ninguna factura para descargar.')", True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & txt_bbdd.Text)

                'Abrimos conexion
                memConn.Open()

                'Declaro
                Dim memComando As New SqlCommand

                'Comienzo la creacion del zip para el usuario
                Using zip As ZipFile = New ZipFile()

                    'Comienzo
                    For Each row As GridViewRow In gridview_consulta.Rows

                        ' Busca el control CheckBox en la celda de la fila
                        Dim chkSeleccionar As CheckBox = TryCast(row.FindControl("chk_marcar"), CheckBox)

                        ' Verifica si el CheckBox está seleccionado
                        If chkSeleccionar IsNot Nothing AndAlso chkSeleccionar.Checked Then

                            'Declaro
                            Dim n_factura As String = gridview_consulta.DataKeys(row.RowIndex).Item("n_factura").ToString

                            'Obtengo el nombre del fichero creado PDF
                            Dim nombre_fichero As String = crear_pdf_salida(txt_bbdd.Text, tabla_usuario.Rows(0)("Id"), n_factura)

                            'Añado el fichero
                            zip.AddFile(Server.MapPath("..") & "\temp\" & nombre_fichero, "")

                            'Actualizo Contador para la descarga
                            memComando.CommandText = "UPDATE cabecera_facturas SET n_descargado=n_descargado+1 WHERE n_factura='" & n_factura & "';"
                            memComando.Connection = memConn
                            memComando.ExecuteNonQuery()

                        End If

                    Next

                    'Grabo el fichero
                    nombre_zip = "Facturas_emitidas_" & tabla_usuario.Rows(0)("Id").ToString & "_" & Date.Now.ToShortDateString.Replace("/", "") & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & Date.Now.Millisecond & ".zip"
                    zip.Save(Server.MapPath("..") & "\temp\" & nombre_zip)

                End Using

                'Cierro la BBDD
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Muestro el placeholder con la foto y la ruta
            Dim fecha As String = DateTime.Today
            Dim hora As String = Now.ToString("HH:mm:ss")
            HL_enlace.NavigateUrl = "../temp/" & nombre_zip
            lbl_etiqueta.Text = nombre_zip
            lbl_titulo_descargar.Text = "Empresa: " & Request.Params("nombre").ToString

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Cargar GV
            cargar_GV()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_descargar').modal('show');"), True)

        Catch ex As Exception
            Response.Write(ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_descargar_Click: " & ex.Message.Replace("'", " ") & "');", True)
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
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, Mid(tabla_cabecera_factura.Rows(0)("fecha").ToString.ToUpper, 1, 10), funciones_globales.puntos(51), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
        cb.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, "1", funciones_globales.puntos(193), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición  

        'Fin del flujo de bytes.             
        cb.EndText()

    End Sub

End Class