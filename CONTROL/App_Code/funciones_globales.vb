Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class funciones_globales

    Sub grabar_regitro(ByVal ruta_base_empresa As String, ByVal id_usuario As Integer, ByVal nombre_usuario As String, ByVal modulo As String, ByVal mensajes As String)

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "INSERT INTO registro_usuarios (fecha,hora,id_usuario,nombre_usuario,modulo,descripcion) VALUES (@fecha,@hora,@id_usuario,@nombre_usuario,@modulo,@descripcion);"
            memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
            memComando.Parameters("@fecha").Value = DateTime.Today
            memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
            memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@id_usuario", System.Data.SqlDbType.Int)
            memComando.Parameters("@id_usuario").Value = id_usuario
            memComando.Parameters.Add("@nombre_usuario", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@nombre_usuario").Value = nombre_usuario
            memComando.Parameters.Add("@modulo", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@modulo").Value = modulo
            memComando.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@descripcion").Value = mensajes
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Reseteo de la cookie
            Dim CookieTime_Session As HttpCookie = New HttpCookie("Time_Session")
            CookieTime_Session.Value = 20
            CookieTime_Session.Expires = DateTime.Now.AddDays(364)
            HttpContext.Current.Response.Cookies.Add(CookieTime_Session)

            'Libero Pool
            SqlConnection.ClearPool(memConn)

        End Using

    End Sub

    Sub grabar_regitro_error(ByVal id_empresa As Integer, ByVal id_usuario As Integer, ByVal modulo As String, ByVal seccion As String, ByVal mensajes As String)

        'Excepcion Si se trata de la finalización de un subproceso, no es un error
        If mensajes <> "Subproceso anulado." Then

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=Inforplan")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "INSERT INTO error_program_control (fecha,hora, id_empresa,id_usuario,modulo,seccion,descripcion) VALUES (@fecha,@hora,@id_empresa,@id_usuario,@modulo,@seccion,@descripcion);"
                memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
                memComando.Parameters("@fecha").Value = DateTime.Today
                memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
                memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@id_usuario", System.Data.SqlDbType.Int)
                memComando.Parameters("@id_usuario").Value = id_usuario
                memComando.Parameters.Add("@id_empresa", System.Data.SqlDbType.Int)
                memComando.Parameters("@id_empresa").Value = id_empresa
                memComando.Parameters.Add("@modulo", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@modulo").Value = modulo
                memComando.Parameters.Add("@seccion", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@seccion").Value = seccion
                memComando.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@descripcion").Value = mensajes
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Libero Pool
                SqlConnection.ClearPool(memConn)

            End Using

        End If

    End Sub

    Function comprobar(ByVal ruta_base_empresa As String, ByVal query As String) As String

        'Asigno
        Dim tabla_consulta As New DataTable

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = query
            memComando.Connection = memConn

            'Creamos un adaptador de datos
            Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

            'Llenamos de datos
            adapter.Fill(tabla_consulta)

            'Cerramos
            adapter.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno
        Dim resultado As Boolean = False

        If tabla_consulta.Rows.Count <> 0 Then
            resultado = True
        End If

        'Cierro
        tabla_consulta.Dispose()

        'Devuelvo
        Return resultado

    End Function

    Function obtener(ByVal bbdd As String, ByVal query As String) As DataTable

        'Asigno
        Dim tabla_consulta As New DataTable

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = query
            memComando.Connection = memConn

            'Creamos un adaptador de datos
            Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

            'Llenamos de datos
            adapter.Fill(tabla_consulta)

            'Cerramos
            adapter.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        Return tabla_consulta

    End Function

    Public Function validar_Mail(ByVal sMail As String) As Boolean
        ' retorna true o false 
        Return Regex.IsMatch(sMail,
                "^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$")
    End Function

    Public Function limpiar(ByVal texto As String, ByVal opcion As String) As String

        'Declaro
        Dim resultado As String = Nothing

        If opcion = "numero" Then

            'Convertimos a minúsculas
            texto = Trim(texto.ToLower)

            'Limpieza
            Dim vnumero() As String = texto.Split(" ")
            Dim valor_anterior As String = Nothing
            For x = 0 To vnumero.Count - 1
                If vnumero(x) = valor_anterior Then
                    texto = texto.Replace(vnumero(x) & " ", "")
                End If
                valor_anterior = vnumero(x)
            Next
            texto = texto.Replace("%", "")
            texto = texto.Replace("€", "")
            texto = texto.Replace("(", "")
            texto = texto.Replace(")", "")
            texto = texto.Replace(":", "")
            texto = texto.Replace(" ", "")
            texto = texto.Replace(ChrW(160), "") 'á
            texto = texto.Replace("impuestos", "")
            texto = texto.Replace("importe", "")
            texto = texto.Replace("eur", "")
            texto = texto.Replace("e", "") 'E
            texto = texto.Replace("igic", "")
            texto = texto.Replace("iva", "")
            texto = texto.Replace("cuota", "")
            texto = texto.Replace("irpf", "")
            texto = texto.Replace("retención", "")
            texto = texto.Replace("i.g.i.c............", "")
            texto = texto.Replace("i.g.i.c.", "")
            texto = texto.Replace("i.r.p.f.", "")

            texto = Trim(texto)

            'Si viene con . y ,
            If texto.IndexOf(".") <> -1 And texto.IndexOf(",") <> -1 Then

                If texto.IndexOf(".") > texto.IndexOf(",") Then

                    Return texto.Replace(",", "")

                Else

                    Return texto.Replace(".", "").Replace(",", ".")

                End If

            End If

            'Si viene con , sólo
            If texto.IndexOf(",") <> -1 Then

                Return texto.Replace(",", ".")

            End If

            'Si viene con . sólo
            If texto.IndexOf(".") <> -1 Then

                Return texto.Replace(".00", "")

            End If

            'Si el texto noe s vacio
            If texto.Count <> 0 Then
                Return texto & ".00"
            End If

            Return texto

        End If

        If opcion = "texto" Then

            'Convertimos a minúsculas
            texto = Trim(texto.ToLower)

            'Limpieza
            Dim vtexto() As String = texto.Split(" ")
            Dim valor_anterior As String = Nothing
            For x = 0 To vtexto.Count - 1
                If vtexto(x) = valor_anterior Then
                    texto = texto.Replace(vtexto(x) & " ", "")
                End If
                valor_anterior = vtexto(x)
            Next
            texto = texto.Replace("cif", "")
            texto = texto.Replace(":", "")
            texto = texto.Replace("factura", "")
            texto = texto.Replace("Nº", "")
            texto = texto.Replace("nif", "")
            texto = texto.Replace("/", "")
            texto = texto.Replace("relación", "")
            texto = texto.Replace("fact.", "")
            texto = texto.Replace("nº", "")
            texto = texto.Replace("c.i.f.", "")
            texto = texto.Replace("número", "")
            texto = texto.Replace("de", "")
            texto = texto.Replace("|", "")
            texto = Trim(texto) 'Quitamos espacios

            Return texto.ToUpper

        End If

        If opcion = "fecha" Then

            'Convertimos a minúsculas
            texto = Trim(texto.ToLower)

            'Limpieza
            Dim vfecha() As String = texto.Split(" ")
            Dim valor_anterior As String = Nothing
            For x = 0 To vfecha.Count - 1
                If vfecha(x) = valor_anterior Then
                    texto = texto.Replace(vfecha(x) & " ", "")
                End If
                valor_anterior = vfecha(x)
            Next
            texto = texto.Replace("fecha", "")
            texto = texto.Replace("factura", "")
            texto = texto.Replace("fra", "")
            texto = texto.Replace("relación", "")
            texto = texto.Replace("fra", "")
            texto = texto.Replace(":", "")

            If IsDate(texto) Then

                Return Trim(texto)

            End If

            texto = texto.Replace(".", "")

            If IsDate(texto) Then

                Return Trim(texto)

            End If

            'Si vienen con abreviatura
            texto = texto.Replace("ene", "enero")
            texto = texto.Replace("feb", "febrero")
            texto = texto.Replace("mar", "marzo")
            texto = texto.Replace("abr", "abril")
            texto = texto.Replace("may", "mayo")
            texto = texto.Replace("jun", "junio")
            texto = texto.Replace("jul", "julio")
            texto = texto.Replace("ago", "agosto")
            texto = texto.Replace("sep", "septiembre")
            texto = texto.Replace("oct", "octubre")
            texto = texto.Replace("nov", "noviembre")
            texto = texto.Replace("dic", "diciembre")

            If IsDate(texto) Then

                Return texto

            End If


            Return texto

        End If

        If opcion = "nif" Then

            'Convertimos a minúsculas
            texto = Trim(texto.ToLower)

            'Limpieza
            Dim vnif() As String = texto.Split(" ")
            Dim valor_anterior As String = Nothing
            For x = 0 To vnif.Count - 1
                If vnif(x) = valor_anterior Then
                    texto = texto.Replace(vnif(x) & " ", "")
                End If
                valor_anterior = vnif(x)
            Next
            texto = texto.Replace("cif", "")
            texto = texto.Replace("dni", "")
            texto = texto.Replace("cliente", "")
            texto = texto.Replace("nif", "")
            texto = texto.Replace("/", "")
            texto = texto.Replace(".", "")
            texto = texto.Replace("-", "")
            texto = texto.Replace(":", "")
            texto = texto.Replace(" ", "")
            texto = Trim(texto) 'Quitamos espacios

            Return texto.ToUpper

        End If

    End Function

    Public Function limpiar_(ByVal texto As String) As String

        texto = texto.Replace(Chr(160), "")
        texto = Trim(texto)

        'Devuelvo
        Return texto

    End Function

    Function HTMLToText(ByVal HTMLCode As String) As String

        ' Replace special characters like &, <, >, " etc.
        Dim sbHTML As StringBuilder = New StringBuilder(HTMLCode)

        'Tag para los acentos
        sbHTML.Replace("&#243;", "ó")
        sbHTML.Replace("&nbsp;", "")
        sbHTML.Replace("&#233;", "é")
        sbHTML.Replace("&#252;", "ü")
        sbHTML.Replace("&#225;", "á")
        sbHTML.Replace("&#186;", "º")
        sbHTML.Replace("&#241;", "ñ")

        ' Finally, remove all HTML tags and return plain text
        Return sbHTML.ToString()

    End Function

    Function crear_excel(ByVal nombre_hoja As String, ByVal gv As GridView, Optional ByVal texto As String = "") As String

        Try

            'Genero el nombre del fichero
            Dim nombre As String = nombre_hoja.Replace(" de ", "_") & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".xlsx"

            'Creo la carpeta para contener el informe temporalmente.
            If Not System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("..") & "\temp\") Then
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("..") & "\temp\")
            End If

            'Comienzo la generacion del Excel
            Dim pck As ExcelPackage = New ExcelPackage(New FileInfo(HttpContext.Current.Server.MapPath("..") & "\temp\" + nombre))

            'Nombre de la Hoja
            Dim ws = pck.Workbook.Worksheets.Add(nombre_hoja)

            'Nombre de la cabecera
            Dim letras() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

            'Nombres Opcionales
            Dim vector() As String = Nothing
            If texto <> "" Then
                vector = texto.Split("|")
            End If

            'Añado cabecera
            Dim posicion_letra As Integer = 0
            For y = 0 To gv.Columns.Count - 1


                If texto = "" Then

                    'Recorro Cabecera
                    If gv.HeaderRow.Cells(y).Text <> "&nbsp;" Then

                        'Cambiar el color
                        ws.Cells(letras(posicion_letra) & "1").Style.Fill.PatternType = ExcelFillStyle.Solid
                        ws.Cells(letras(posicion_letra) & "1").Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#146ABD"))
                        ws.Cells(letras(posicion_letra) & "1").Style.Font.Color.SetColor(Color.White)
                        ws.Cells(letras(posicion_letra) & "1").Value = HTMLToText(gv.HeaderRow.Cells(y).Text)
                        'Continuar
                        posicion_letra += 1

                    End If

                Else

                    'Recorro Cabecera
                    If vector(y).ToString <> "" Then

                        'Cambiar el color
                        ws.Cells(letras(posicion_letra) & "1").Style.Fill.PatternType = ExcelFillStyle.Solid
                        ws.Cells(letras(posicion_letra) & "1").Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#146ABD"))
                        ws.Cells(letras(posicion_letra) & "1").Style.Font.Color.SetColor(Color.White)
                        ws.Cells(letras(posicion_letra) & "1").Value = vector(y)
                        'Continuar
                        posicion_letra += 1

                    End If

                End If

            Next

            'Datos
            posicion_letra = 0
            For x = 0 To gv.Rows.Count - 1
                For y = 0 To gv.Columns.Count - 1

                    If texto = "" Then

                        If gv.HeaderRow.Cells(y).Text <> "&nbsp;" Then

                            'Converción a numerico
                            If gv.HeaderRow.Cells(y).Text.ToUpper = "DEBE" Or gv.HeaderRow.Cells(y).Text.ToUpper = "HABER" Or gv.HeaderRow.Cells(y).Text.ToUpper = "IMPORTE" Or HTMLToText(gv.HeaderRow.Cells(y).Text).ToUpper = "VALOR ADQUISICIÓN" Or gv.HeaderRow.Cells(y).Text.ToUpper = "SALDO" Then
                                If Trim(gv.Rows(x).Cells(y).Text) <> "" Then
                                    ws.Cells(letras(posicion_letra) & x + 2).Style.Numberformat.Format = "#,##0.00"
                                    ws.Cells(letras(posicion_letra) & x + 2).Value = CDec(gv.Rows(x).Cells(y).Text)
                                End If
                            Else
                                ws.Cells(letras(posicion_letra) & x + 2).Value = HTMLToText(gv.Rows(x).Cells(y).Text)
                            End If

                            'Continuar
                            posicion_letra += 1

                        End If

                    Else

                        If vector(y).ToString <> "" Then

                            If vector(y).ToString.ToUpper = "DEBE" Or vector(y).ToString.ToUpper = "HABER" Or vector(y).ToString.ToUpper = "IMPORTE" Or vector(y).ToString.ToUpper = "VALOR ADQUISICIÓN" Or vector(y).ToString.ToUpper = "SALDO" Then
                                If Trim(gv.Rows(x).Cells(y).Text) <> "" Then
                                    ws.Cells(letras(posicion_letra) & x + 2).Style.Numberformat.Format = "#,##0.00"
                                    ws.Cells(letras(posicion_letra) & x + 2).Value = CDec(gv.Rows(x).Cells(y).Text)
                                End If
                            Else
                                ws.Cells(letras(posicion_letra) & x + 2).Value = HTMLToText(gv.Rows(x).Cells(y).Text)
                            End If

                            'Continuar
                            posicion_letra += 1

                        End If

                    End If

                Next

                'Continuar
                posicion_letra = 0

            Next

            'Autoajuste
            ws.Cells("A1:" & letras(gv.Columns.Count - 1) & gv.Rows.Count + 1).AutoFitColumns()

            'Cerramos el documento.             
            pck.Save()

            Return nombre

        Catch ex As Exception

            Return "Error: crear_excel"

        End Try

    End Function

    Function crear_txt(ByVal nombre_hoja As String, ByVal gv As GridView, Optional ByVal texto As String = "") As String

        Try

            'Genero el nombre del fichero
            Dim nombre As String = nombre_hoja.Replace(" de ", "_") & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".txt"

            'Creo la carpeta para contener el informe temporalmente.
            If Not System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("..") & "\temp\") Then
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("..") & "\temp\")
            End If

            'Escribo el fichero
            Dim oSW As New StreamWriter(HttpContext.Current.Server.MapPath("..") & "\temp\" & nombre, False)

            'Nombre de la cabecera
            Dim letras() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

            'Nombres Opcionales
            Dim vector() As String = Nothing
            If texto <> "" Then
                vector = texto.Split("|")
            End If

            'Añado cabecera
            Dim posicion_letra As Integer = 0
            Dim texto_cabecera As String = Nothing
            For y = 0 To gv.Columns.Count - 1

                If texto = "" Then

                    'Recorro Cabecera
                    If gv.HeaderRow.Cells(y).Text <> "&nbsp;" Then

                        'añado línea
                        texto_cabecera += HTMLToText(gv.HeaderRow.Cells(y).Text) & "|"

                    End If

                Else

                    'añado línea
                    texto_cabecera += vector(y) & "|"

                End If

                'Continuar
                posicion_letra += 1

            Next

            'Grabo
            oSW.WriteLine(Mid(texto_cabecera, 1, texto_cabecera.Count - 1))

            'Datos
            posicion_letra = 0
            texto_cabecera = Nothing
            For x = 0 To gv.Rows.Count - 1
                For y = 0 To gv.Columns.Count - 1
                    If gv.HeaderRow.Cells(y).Text <> "&nbsp;" Then

                        'añado línea
                        texto_cabecera += HTMLToText(Trim(gv.Rows(x).Cells(y).Text)) & "|"

                        'Continuar
                        posicion_letra += 1
                    End If
                Next

                'Grabo
                oSW.WriteLine(Mid(texto_cabecera, 1, texto_cabecera.Count - 1))

                'Continuar
                texto_cabecera = Nothing
                posicion_letra = 0

            Next

            'Finalizo
            oSW.Close()

            Return nombre

        Catch ex As Exception

            Return "Error: crear_txt"

        End Try

    End Function






















    Function Obtener_datos_variados(ByVal ruta_base_empresa As String, ByVal instruccion As String, ByVal campos As String) As String

        'Ataco a la conexion del programa
        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa
        Dim memConn As New SqlConnection(ruta_base)
        memConn.Open()

        Dim memComando As New SqlCommand
        memComando.CommandText = instruccion
        memComando.Connection = memConn
        Dim memDatos As SqlDataReader
        memDatos = memComando.ExecuteReader
        Dim vector_campos() As String = campos.Split("|")

        If memDatos.HasRows Then
            Do While memDatos.Read

                Select Case vector_campos.Count
                    Case 1 : Return memDatos.Item(vector_campos(0)).ToString
                    Case 2 : Return memDatos.Item(vector_campos(0)).ToString & "|" & memDatos.Item(vector_campos(1)).ToString
                End Select

            Loop
            Return True
        Else
            Return "0"
        End If

        ' Cierro la base de datos
        memDatos.Close()
        memComando.Dispose()
        memConn.Close()

    End Function

    Public Function grabar_notificacion(ByVal ruta_base_empresa As String, ByVal titulo As String, ByVal cuerpo As String, ByVal prioridad As String, ByVal usuario As String) As String

        'Ataco a la conexion del programa
        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa
        Dim memConn As New SqlConnection(ruta_base)
        memConn.Open()
        Dim memComando As New SqlCommand

        memComando.CommandText = "SELECT TOP(1) Id FROM notificaciones ORDER BY id DESC;"
        memComando.Connection = memConn
        Dim memDatos As SqlDataReader
        memDatos = memComando.ExecuteReader
        Dim contador As Integer = 0
        If memDatos.HasRows Then
            Do While memDatos.Read
                contador = memDatos.Item("Id").ToString
            Loop
        End If

        'Cierro la base de datos
        memDatos.Close()
        memComando.Dispose()

        memComando.CommandText = "INSERT INTO notificaciones (id,fecha,hora,titulo,texto,usuario,prioridad) VALUES (" & contador + 1 & ",@fecha,@hora,@titulo,@texto,@usuario,@prioridad);"
        memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
        memComando.Parameters("@fecha").Value = DateTime.Today
        memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
        memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
        memComando.Parameters.Add("@titulo", System.Data.SqlDbType.VarChar)
        memComando.Parameters("@titulo").Value = titulo
        memComando.Parameters.Add("@texto", System.Data.SqlDbType.VarChar)
        memComando.Parameters("@texto").Value = cuerpo
        memComando.Parameters.Add("@usuario", System.Data.SqlDbType.VarChar)
        memComando.Parameters("@usuario").Value = usuario
        memComando.Parameters.Add("@prioridad", System.Data.SqlDbType.VarChar)
        memComando.Parameters("@prioridad").Value = prioridad
        memComando.Connection = memConn
        memComando.ExecuteNonQuery()

        'Vacio buffer
        memComando.Dispose()
        memConn.Close()
        memConn.Dispose()

        Return "OK"

    End Function

End Class
