Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class facturas
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub cargar_treeview()

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT * FROM facturacion WHERE Id_usuario=" & parametros_usuario(0) & " ORDER BY year(fecha) DESC,month(fecha);"
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

            'Control para la no repeticion
            Dim control As String = Nothing
            Dim control_meses As String = Nothing

            'Excepción
            If tabla_consulta.Rows.Count = 0 Then

                'Mensaje
                Dim mensaje As String = parametros_usuario(7) & ", aún no tienes ninguna factura disponible."
                Dim mensaje_final As String = Nothing
                mensaje_final = "alertify.warning('" & mensaje & "',0);window.parent.hablar('" & mensaje & "');"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Aviso_factura", mensaje_final, True)
                Exit Sub

            End If

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Solo añade al nodo principal años
                'Nodo Principal
                Dim theRootNode As TreeNode = New TreeNode
                If control <> Year(tabla_consulta.Rows(x)(4).ToString) Then

                    'Asigno
                    theRootNode.Text = Year(tabla_consulta.Rows(x)(4).ToString)
                    TreeView1.Nodes.Add(theRootNode)
                    control = Year(tabla_consulta.Rows(x)(4).ToString)

                    For y = 0 To tabla_consulta.Rows.Count - 1

                        If control = Year(tabla_consulta.Rows(y)("fecha").ToString) Then

                            'Asigno
                            Dim theChild1 As TreeNode = New TreeNode
                            theChild1.Text = "<font color='#4D4D4D'>" & Mid(tabla_consulta.Rows(y)("fecha").ToString, 1, 10) & " -  Nº Factura: " & tabla_consulta.Rows(y)("n_factura").ToString & " (Importe: " & tabla_consulta.Rows(y)("importe_total").ToString & "€ )"
                            'Pagada o No
                            If tabla_consulta.Rows(y)("pagada") = True Then
                                theChild1.Text += " - <font color='#28a745'>Pagada"
                            Else
                                theChild1.Text += " - <font color='#dc3545'>No Pagada"
                            End If
                            theChild1.Value = Mid(tabla_consulta.Rows(y)("fecha").ToString, 1, 10) & "*" & tabla_consulta.Rows(y)("n_factura").ToString & "*" & tabla_consulta.Rows(y)("pagada").ToString & "*" & tabla_consulta.Rows(y)("importe_total").ToString
                            theRootNode.ChildNodes.Add(theChild1)

                        End If

                    Next

                End If

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "cargar_treeview", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error cargar_treeview: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Cargar_Treeview
                cargar_treeview()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles TreeView1.SelectedNodeChanged

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Asigno
            Dim vector() As String = TreeView1.SelectedValue.ToString.Split("*")

            'Extraigo fecha de la factura y su número
            Dim fecha As String = vector(0)
            Dim numero_factura As String = vector(1)
            Dim pagada As Boolean = CBool(vector(2))
            Dim total_pagar As Decimal = vector(3)

            'Una tabla para contenerlos
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                memComando.CommandText = "SELECT facturacion.*,datos_facturacion.*,provincia.nombre as nombre_provincia,localidad.nombre as nombre_localidad " &
                "FROM facturacion,datos_facturacion,provincia,localidad " &
                "WHERE facturacion.Id_usuario=datos_facturacion.Id_usuario " &
                "AND datos_facturacion.provincia = provincia.Id " &
                "AND (localidad.id_provincia = datos_facturacion.provincia AND localidad.id_localidad=datos_facturacion .localidad) " &
                "AND facturacion.Id_usuario=" & parametros_usuario(0) & " And facturacion.fecha='" & fecha & "';"
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

            'Nombre 
            Dim nombre As String = "Factura_" & parametros_usuario(0) & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".pdf"

            'Comienzo la generacion del PDF
            Dim oDoc As New iTextSharp.text.Document(PageSize.A4)
            Dim pdfw As iTextSharp.text.pdf.PdfWriter
            Dim cb As PdfContentByte
            Dim linea As PdfContentByte

            'Dim rectangulo As PdfContentByte         
            Dim fuente As iTextSharp.text.pdf.BaseFont
            Dim fuente2 As iTextSharp.text.pdf.BaseFont
            Dim imagendemo As iTextSharp.text.Image 'Declaracion de una imagen

            pdfw = PdfWriter.GetInstance(oDoc, New FileStream(Server.MapPath("..") & "\temp\" & nombre, FileMode.Create, FileAccess.Write, FileShare.None))

            'Apertura del documento.
            oDoc.Open()
            cb = pdfw.DirectContent
            linea = pdfw.DirectContent

            'Variable para controlar la cabecera, avance de la linea de detalles y la posicion en la que se encuentra
            Dim cabecera As Boolean = True
            Dim avance_linea As Integer = 199
            Dim posicion_linea As Integer = 1

            'Asigno
            Dim fecha_factura As String = Mid(tabla_consulta.Rows(0)("fecha").ToString, 1, 10)

            'Dim metodo_pago As String = tabla_consulta.Rows(0)(32).ToString




            'Dim porcentaje7 As String = tabla_consulta.Rows(0)(7)
            'Dim porcentaje21 As String = Nothing
            'Dim cuota2 As String = Nothing
            'Dim cuota3 As String = Nothing
            'Dim cuota5 As String = Nothing
            'Dim cuota6_5 As String = Nothing
            Dim cuota7 As String = tabla_consulta.Rows(0)(6)
            'Dim cuota21 As String = Nothing
            Dim total_neto As String = tabla_consulta.Rows(0)(10).ToString

            If cabecera = True Then

                'Agregamos una pagina.             
                oDoc.NewPage()

                'Insertamos la imagen de fondo
                imagendemo = iTextSharp.text.Image.GetInstance(Server.MapPath("../imagenes/factura/fondo_factura.png")) 'Dirreccion a la imagen que se hace referencia
                imagendemo.SetAbsolutePosition(0, 0) 'Posicion en el eje cartesiano
                imagendemo.ScaleAbsoluteWidth(funciones_globales.puntos(210)) 'Ancho de la imagen
                imagendemo.ScaleAbsoluteHeight(funciones_globales.puntos(297)) 'Altura de la imagen
                oDoc.Add(imagendemo) ' Agrega la imagen al documento

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Instanciamos el objeto para el tipo de letra. 
                fuente = FontFactory.GetFont(FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont
                cb.SetFontAndSize(fuente, 8) 'fuente definida en la linea anterior y tamaño 

                'Montamos cuadro del cliente
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tabla_consulta.Rows(0)("denominacion_fiscal").ToString.ToUpper & " - Cod. Cliente: " & tabla_consulta.Rows(0)("id_usuario").ToString, funciones_globales.puntos(110), funciones_globales.puntos(285), 0)
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tabla_consulta.Rows(0)("tipo").ToString.ToUpper & " " & tabla_consulta.Rows(0)("denominacion_calle").ToString.ToUpper & ", " & tabla_consulta.Rows(0)("numero").ToString.ToUpper, funciones_globales.puntos(110), funciones_globales.puntos(280), 0)
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tabla_consulta.Rows(0)("cp").ToString.ToUpper & " - " & tabla_consulta.Rows(0)("nombre_localidad").ToString.ToUpper, funciones_globales.puntos(110), funciones_globales.puntos(275), 0)
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tabla_consulta.Rows(0)("nombre_provincia").ToString.ToUpper, funciones_globales.puntos(110), funciones_globales.puntos(270), 0)
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "NIF / CIF: ", funciones_globales.puntos(110), funciones_globales.puntos(265), 0)
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tabla_consulta.Rows(0)("nif").ToString.ToUpper, funciones_globales.puntos(125), funciones_globales.puntos(265), 0)

                'Fecha Factura y Nº
                cb.SetRGBColorFill(110, 110, 110)

                'NIF IO
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "CIF: B09973652", funciones_globales.puntos(5.5), funciones_globales.puntos(230), 90) 'alineación, texto a escribir y posición     
                cb.SetRGBColorFill(80, 80, 80)
                cb.SetFontAndSize(fuente, 8)
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, tabla_consulta.Rows(0)("n_factura").ToString.ToUpper, funciones_globales.puntos(23), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Mid(tabla_consulta.Rows(0)("fecha").ToString.ToUpper, 1, 10), funciones_globales.puntos(51), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición     
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "1", funciones_globales.puntos(193), funciones_globales.puntos(219), 0) 'alineación, texto a escribir y posición  

                'Fin del flujo de bytes.             
                cb.EndText()
                cabecera = False

            End If

            If posicion_linea < 26 Then

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ALQUILER IO CONTABILIDAD (INCLUYE PROGRAMA + 4000 MB ALMACENAMIENTO)", funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, tabla_consulta.Rows(0)("cantidad_1"), funciones_globales.puntos(150), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Decimal.Round(CDec(tabla_consulta.Rows(0)("precio_1")), 2), funciones_globales.puntos(172), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Decimal.Round(CDec(tabla_consulta.Rows(0)("importe_1")), 2), funciones_globales.puntos(198), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "OCUPACION ACTUAL " & tabla_consulta.Rows(0)("MB_ocupados").ToString.ToUpper & " MB", funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                If tabla_consulta.Rows(0)("MB_ocupados") > 4000 Then
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, tabla_consulta.Rows(0)("cantidad_2"), funciones_globales.puntos(150), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Decimal.Round(CDec(tabla_consulta.Rows(0)("precio_2")), 2), funciones_globales.puntos(172), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Decimal.Round(CDec(tabla_consulta.Rows(0)("importe_2")), 2), funciones_globales.puntos(198), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                End If

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "- DATOS " & tabla_consulta.Rows(0)("bbdd_mb") & " MB", funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "- BACKUP " & tabla_consulta.Rows(0)("backup_mb") & " MB", funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "- GESTIÓN DOCUMENTAL " & tabla_consulta.Rows(0)("gestion_documental_mb") & " MB", funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "- IO ALMACÉN " & tabla_consulta.Rows(0)("io_almacenamiento") & " MB", funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PLANTILLAS PDF CON INTELIGENCIA ARTIFICIAL", funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                If tabla_consulta.Rows(0)("cantidad_3") <> 0 Then
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, tabla_consulta.Rows(0)("cantidad_3"), funciones_globales.puntos(150), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Decimal.Round(CDec(tabla_consulta.Rows(0)("precio_3")), 3), funciones_globales.puntos(172), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Decimal.Round(CDec(tabla_consulta.Rows(0)("importe_3")), 2), funciones_globales.puntos(198), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    
                End If

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

                'Determino grosor y color de la linea
                linea.SetLineWidth(0.3)
                linea.SetColorStroke(New CMYKColor(0, 0, 0, 100))
                linea.MoveTo(funciones_globales.puntos(10), funciones_globales.puntos(avance_linea - 1))
                linea.LineTo(funciones_globales.puntos(200), funciones_globales.puntos(avance_linea - 1))
                linea.Stroke()

                'Iniciamos el flujo de bytes.             
                cb.BeginText()

                'Lineas de detalle de la factura
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "CUOTA MENSUAL PERÍODO DEL 01/" & CDate(fecha_factura).ToString("MM", CultureInfo.InvariantCulture) & "/" & Year(fecha_factura) & " AL " & CDate(fecha_factura).AddMonths(1).AddDays(-1), funciones_globales.puntos(12), funciones_globales.puntos(avance_linea), 0) 'alineación, texto a escribir y posición    

                'Asigno
                avance_linea -= 5
                posicion_linea += 1

                'Termino el flujo de bytes
                cb.EndText()

            End If

            'Escribitos las bases imponibles,porcentaje y cuotas
            'Iniciamos el flujo de bytes.             
            cb.BeginText()

            'Lineas de bases imponibles,% y cuota
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "BASE IMPONIBLE", funciones_globales.puntos(139), funciones_globales.puntos(61), 0) 'alineación, texto a escribir y posición    
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "% IMPUESTO", funciones_globales.puntos(164), funciones_globales.puntos(61), 0) 'alineación, texto a escribir y posición    
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "CUOTA", funciones_globales.puntos(189), funciones_globales.puntos(61), 0) 'alineación, texto a escribir y posición    

            'Lineas de bases imponibles de la factura
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, tabla_consulta.Rows(0)("base_total"), funciones_globales.puntos(139), funciones_globales.puntos(56), 0) 'alineación, texto a escribir y posición    
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, tabla_consulta.Rows(0)("porcentaje_impuesto_total") & " %", funciones_globales.puntos(164), funciones_globales.puntos(56), 0) 'alineación, texto a escribir y posición    
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, tabla_consulta.Rows(0)("cuota_total"), funciones_globales.puntos(189), funciones_globales.puntos(56), 0) 'alineación, texto a escribir y posición    

            'Método de Pago
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, tabla_consulta.Rows(0)("metodo_pago"), funciones_globales.puntos(160), funciones_globales.puntos(35), 0) 'alineación, texto a escribir y posición    

            'Instanciamos el objeto para el tipo de letra. 
            fuente2 = FontFactory.GetFont(FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.BOLD).BaseFont

            'Texto de inversión de sujeto pasivo
            cb.SetFontAndSize(fuente2, 8)
            If tabla_consulta.Rows(0)(36).ToString = "IVA" Then
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "INVERSION DEL SUJETO PASIVO SEGÚN EL ART.84 UNO 2ºA) DE LA LEY 37/1992.", funciones_globales.puntos(15), funciones_globales.puntos(73), 0)
            End If

            'Comenzamos la escritura del Total de la factura
            'Instanciamos el objeto para el tipo de letra. 
            fuente = FontFactory.GetFont(FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont
            cb.SetFontAndSize(fuente, 11) 'fuente definida en la linea anterior y tamaño 
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "TOTAL FACTURA:", funciones_globales.puntos(144), funciones_globales.puntos(44), 0) 'alineación, texto a escribir y posición    

            cb.SetFontAndSize(fuente2, 11) 'fuente definida en la linea anterior y tamaño 
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, tabla_consulta.Rows(0)("importe_total") & " €", funciones_globales.puntos(190), funciones_globales.puntos(44), 0) 'alineación, texto a escribir y posición    
            cb.EndText()

            'Forzamos vaciamiento del buffer.             
            pdfw.Flush()

            'Cerramos el documento.             
            oDoc.Close()

            'Destruimos Variables
            cb = Nothing
            pdfw = Nothing
            oDoc = Nothing

            'Paso la ruta al iframe
            ver_factura_embed.Visible = True
            ver_factura_embed.Attributes("src") = "..\temp\" & nombre

            'Asigno
            txt_seleccion_factura.Text = TreeView1.SelectedValue.ToString & "*" & nombre
            If pagada = False Then
                PH_tarjeta.Visible = True
            Else
                PH_tarjeta.Visible = False
            End If

            'Paso al trazador
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Facturas", "Listó la factura nº:  " & tabla_consulta.Rows(0)("n_factura").ToString.ToUpper & " del año: " & Year(fecha) & ".")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub LB_pagar_Click(sender As Object, e As EventArgs) Handles LB_pagar.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Oculto el panel
            Panel_Mostrar_PDF.Visible = True

            'Paso la ruta al iframe
            Iframe_pago.Attributes("src") = "pago_tarjeta.aspx?id=" & txt_seleccion_factura.Text

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "LB_pagar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error LB_pagar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Img_cerrar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles Img_cerrar.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Cargar_Treeview
            Response.Redirect("facturas.aspx")

            'Oculto el panel
            Panel_Mostrar_PDF.Visible = "false"

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Img_cerrar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Img_cerrar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class

