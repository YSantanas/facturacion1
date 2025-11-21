Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Web.Services
Imports MessagingToolkit.QRCode.Codec
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class funciones_globales

    '----------------------------FUNCIONES GLOBALES-----------------------------------------------------
    Function modal_register(ByVal texto As String) As String

        texto = "$(document).ready(function () { " & texto & "});"

        Return texto

    End Function

    Function obtener_datos(ByVal query As String) As DataTable

        'Asigno
        Dim tabla_consulta As New DataTable

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)

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
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        Return tabla_consulta

    End Function

    Sub grabar_registro_error(ByVal id_empresa As Integer, ByVal id_usuario As Integer, ByVal modulo As String, ByVal seccion As String, ByVal mensajes As String)

        'Excepcion Si se trata de la finalización de un subproceso, no es un error
        If mensajes <> "Subproceso anulado." Then


            'Actualizo
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "INSERT INTO error_program (fecha,hora, id_empresa,id_usuario,modulo,seccion,descripcion) VALUES (@fecha,@hora,@id_empresa,@id_usuario,@modulo,@seccion,@descripcion);"
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

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

        End If

    End Sub

    Sub grabar_registro(ByVal ruta_base_empresa As String, ByVal id_usuario As Integer, ByVal nombre_usuario As String, ByVal modulo As String, ByVal mensajes As String)

        'Actualizo
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

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Reseteo de la cookie
        Dim CookieTime_Session As HttpCookie = New HttpCookie("Time_Session.Facturacion")
        CookieTime_Session.Value = 20
        CookieTime_Session.Expires = DateTime.Now.AddDays(364)
        HttpContext.Current.Response.Cookies.Add(CookieTime_Session)

    End Sub

    Function cerrar_sesion(ByVal id_usuario As Integer, ByVal id_empresa As Integer) As String

        Try

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "DELETE FROM sessiones_activas WHERE usuario=" & id_usuario & " AND key_usuario='" & HttpContext.Current.Session("id_control") & "';"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cargo los datos a global
                Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & id_usuario & "_tabla_usuario")
                Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_empresa")

                'Paso la traza para grabar el Log
                grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Login", "El Usuario Cerró la aplicación.")

                'Finalizamo Sesion
                HttpContext.Current.Session.Abandon()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearAllPools()

            End Using

            Return "Terminado OK"

        Catch ex As Exception

            Return "Error: cerrar_session"

        End Try

    End Function

    Function crear_excel(ByVal nombre_hoja As String, ByVal gv As GridView, ByVal id_usuario As Integer, Optional ByVal texto As String = "") As String

        Try

            'Genero el nombre del fichero
            Dim nombre As String = nombre_hoja.Replace(" de ", "_") & "_" & id_usuario & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "") & ".xlsx"

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
                            If gv.HeaderRow.Cells(y).Text.ToUpper = "CÓDIGO" Or gv.HeaderRow.Cells(y).Text.ToUpper = "HABER" Or gv.HeaderRow.Cells(y).Text.ToUpper = "IMPORTE" Or HTMLToText(gv.HeaderRow.Cells(y).Text).ToUpper = "VALOR ADQUISICIÓN" Or gv.HeaderRow.Cells(y).Text.ToUpper = "SALDO" Then
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

                            If vector(y).ToString.ToUpper = "CÓDIGO" Or vector(y).ToString.ToUpper = "HABER" Or vector(y).ToString.ToUpper = "IMPORTE" Or vector(y).ToString.ToUpper = "VALOR ADQUISICIÓN" Or vector(y).ToString.ToUpper = "SALDO" Then
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

    Function crear_txt(ByVal nombre_hoja As String, ByVal gv As GridView, ByVal id_usuario As Integer, Optional ByVal texto As String = "") As String

        Try

            'Genero el nombre del fichero
            Dim nombre As String = nombre_hoja.Replace(" de ", "_") & "_" & id_usuario & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "") & ".txt"

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

    Function buscar_datos_tabla(ByVal tabla_datos As DataTable, ByVal campo_buscar As String, ByVal valor_buscar As String, ByVal campo_devolver As String) As String

        'Excepcion
        If valor_buscar = "" Then
            Return "0"
        End If

        'Hago Select
        Dim expression As String = campo_buscar & " = " & valor_buscar & ""
        Dim sortOrder As String = Nothing

        'Declaro
        Dim foundRows() As DataRow

        'Almaceno todos los rows obtenidos
        foundRows = tabla_datos.Select(expression, sortOrder)

        Dim i As Integer
        Dim valor As String = "0"

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            'si me pide un campo
            If campo_devolver <> "todos" Then

                'Asigno
                valor = foundRows(i)(campo_devolver)

            Else

                'Recorro
                For y = 0 To tabla_datos.Columns.Count - 1

                    'Excepcion
                    If y = 0 Then
                        valor = Nothing
                    End If

                    'Asigno
                    valor += foundRows(i)(y).ToString & "|"

                Next


            End If

        Next i

        'Devuelvo resultado
        Return valor

    End Function

    Public Function Verificar_NIF(ByVal valor As String) As Boolean

        'Declaro
        Dim aux As String

        valor = valor.ToUpper ' ponemos la letra en mayúscula
        aux = valor.Substring(0, valor.Length - 1) ' quitamos la letra del NIF

        If aux.Length >= 7 AndAlso IsNumeric(aux) Then
            aux = CalculaNIF(aux) ' calculamos la letra del NIF para comparar con la que tenemos
        Else
            Return False
        End If

        If valor <> aux Then ' comparamos las letras
            Return False
        End If

        'Devuelve
        Return True

    End Function

    Public Function CalculaNIF(ByVal strA As String) As String
        '----------------------------------------------------------------------
        ' Calcular la letra del NIF
        ' Código original adaptado a Visual Basic                   (13/Sep/95)
        ' Adaptado a Visual Basic .NET (VB 9.0/2008)                (09/May/08)
        ' y convertido en función que devuelve el NIF correcto
        '----------------------------------------------------------------------
        Const cCADENA As String = "TRWAGMYFPDXBNJZSQVHLCKE"
        Const cNUMEROS As String = "0123456789"
        Dim a, b, c, NIF As Integer
        Dim sb As New StringBuilder

        strA = Trim(strA)
        If Len(strA) = 0 Then Return ""

        ' Dejar sólo los números
        For i As Integer = 0 To strA.Length - 1
            If cNUMEROS.IndexOf(strA(i)) > -1 Then
                sb.Append(strA(i))
            End If
        Next

        strA = sb.ToString
        a = 0
        NIF = CInt(Val(strA))
        Do
            b = CInt(Int(NIF / 24))
            c = NIF - (24 * b)
            a = a + c
            NIF = b
        Loop While b <> 0
        b = CInt(Int(a / 23))
        c = a - (23 * b)

        Return strA & Mid(cCADENA, CInt(c + 1), 1)

    End Function

    Public Function Verificar_CIF(ByVal valor As String) As Boolean

        'Declaro
        Dim strLetra As String, strNumero As String, strDigit As String
        Dim strDigitAux As String
        Dim auxNum As Integer
        Dim i As Integer
        Dim suma As Integer
        Dim letras As String

        letras = "ABCDEFGHKLMPQSX"

        valor = UCase(valor)

        If Len(valor) < 9 OrElse Not IsNumeric(Mid(valor, 2, 7)) Then
            Return False
        End If

        strLetra = Mid(valor, 1, 1)     ' letra del CIF
        strNumero = Mid(valor, 2, 7)    ' Codigo de Control
        strDigit = Mid(valor, 9)        ' CIF menos primera y ultima posiciones

        If InStr(letras, strLetra) = 0 Then ' comprobamos la letra del CIF (1ª posicion)
            Return False
        End If

        For i = 1 To 7
            If i Mod 2 = 0 Then
                suma = suma + CInt(Mid(strNumero, i, 1))
            Else
                auxNum = CInt(Mid(strNumero, i, 1)) * 2
                suma = suma + (auxNum \ 10) + (auxNum Mod 10)
            End If
        Next

        suma = (10 - (suma Mod 10)) Mod 10

        Select Case strLetra
            Case "K", "P", "Q", "S"
                suma = suma + 64
                strDigitAux = Chr(suma)
            Case "X"
                strDigitAux = Mid(CalculaNIF(strNumero), 8, 1)
            Case Else
                strDigitAux = CStr(suma)
        End Select

        If strDigit = strDigitAux Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function grabar_notificacion(ByVal ruta_base_empresa As String, ByVal titulo As String, ByVal cuerpo As String, ByVal prioridad As String, ByVal usuario As String) As String

        'Declaro
        Dim contador As Integer = 0

        'Obtengo el último numero
        Dim tabla As DataTable = obtener_datos("SELECT TOP(1) Id FROM [" & ruta_base_empresa & "].[dbo].notificaciones ORDER BY id DESC;")
        If tabla.Rows.Count <> 0 Then
            contador = CInt(tabla.Rows(0)("Id"))
        End If

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
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

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        Return "OK"

    End Function

    Public Function numero_factura(ByVal ruta_base_empresa As String) As String

        'Declaro
        Dim tabla_consulta As DataTable = obtener_datos("SELECT n_factura FROM [" & ruta_base_empresa & "].[dbo].contadores;")
        Dim primer_caracter As String = "F"
        Dim segundo_caracter As String = Year(Now)

        'Devuelvo
        Return primer_caracter & segundo_caracter & tabla_consulta(0)("n_factura").ToString.PadLeft(6, "0")

    End Function

    Public Function numero_abono(ByVal ruta_base_empresa As String) As String

        'Declaro
        Dim tabla_consulta As DataTable = obtener_datos("SELECT n_abono FROM [" & ruta_base_empresa & "].[dbo].contadores;")
        Dim primer_caracter As String = "A"
        Dim segundo_caracter As String = Year(Now)

        'Devuelvo
        Return primer_caracter & segundo_caracter & tabla_consulta(0)("n_abono").ToString.PadLeft(6, "0")

    End Function

    Public Function numero_presupuesto(ByVal ruta_base_empresa As String) As String

        'Declaro
        Dim tabla_consulta As DataTable = obtener_datos("SELECT n_presupuesto FROM [" & ruta_base_empresa & "].[dbo].contadores;")
        Dim primer_caracter As String = "P"
        Dim segundo_caracter As String = Year(Now)

        'Devuelvo
        Return primer_caracter & segundo_caracter & tabla_consulta(0)("n_presupuesto").ToString.PadLeft(6, "0")

    End Function

    Public Function numero_albaran(ByVal ruta_base_empresa As String) As String

        'Declaro
        Dim tabla_consulta As DataTable = obtener_datos("SELECT n_albaran FROM [" & ruta_base_empresa & "].[dbo].contadores;")
        Dim primer_caracter As String = "ALB"
        Dim segundo_caracter As String = Year(Now)

        'Devuelvo
        Return primer_caracter & segundo_caracter & tabla_consulta(0)("n_albaran").ToString.PadLeft(6, "0")

    End Function

    Function grabar_facturas_(ByVal ruta_base_empresa As String, ByVal n_factura As String, ByVal fecha As Date, ByVal cod_cliente As Integer, ByVal cliente As String, ByVal nif As String, ByVal direccion As String, ByVal cp As String, ByVal poblacion As String, ByVal provincia As String, ByVal GV_detalles As GridView, ByVal GV_pie As GridView, ByVal origen As String, ByVal n_presupuesto As String, ByVal n_albaran As String, ByVal chk_isp As Boolean, ByVal chk_exento As Boolean) As String

        'Declaro
        Dim total_factura As Decimal = 0

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand

            'Declaro
            memComando.Parameters.Add("@n_factura", System.Data.SqlDbType.VarChar, 15).Value = n_factura
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@linea", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@cantidad", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@cod_articulo", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar, 250)
            memComando.Parameters.Add("@precio", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@dto_1", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@porcentaje_impuesto", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@total_linea", System.Data.SqlDbType.Decimal, 10, 2)

            'Agrego detalles
            For x = 0 To GV_detalles.Rows.Count - 1

                memComando.CommandText = "INSERT INTO detalles_facturas (n_factura,fecha_creacion,hora_creacion,linea,cantidad,cod_articulo,denominacion,precio,dto_1,porcentaje_impuesto,total) VALUES " &
               "(@n_factura,@fecha_creacion,@hora_creacion,@linea,@cantidad,@cod_articulo,@denominacion,@precio,@dto_1,@porcentaje_impuesto,@total_linea);"
                memComando.Parameters("@linea").Value = x
                memComando.Parameters("@cantidad").Value = GV_detalles.DataKeys(x).Item("cantidad")
                memComando.Parameters("@cod_articulo").Value = GV_detalles.DataKeys(x).Item("codigo")
                memComando.Parameters("@denominacion").Value = GV_detalles.DataKeys(x).Item("articulo")
                memComando.Parameters("@precio").Value = GV_detalles.DataKeys(x).Item("precio")
                memComando.Parameters("@dto_1").Value = GV_detalles.DataKeys(x).Item("dto")
                memComando.Parameters("@porcentaje_impuesto").Value = GV_detalles.DataKeys(x).Item("impuesto")
                memComando.Parameters("@total_linea").Value = GV_detalles.DataKeys(x).Item("total")
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

            Next

            'Declaro
            memComando.Parameters.Add("@base_imponible", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@porcentaje", System.Data.SqlDbType.Decimal, 9, 2)
            memComando.Parameters.Add("@cuota", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@total", System.Data.SqlDbType.Decimal, 18, 2)

            'Agrego Pie
            For x = 0 To GV_pie.Rows.Count - 1

                memComando.CommandText = "INSERT INTO pie_facturas (n_factura,fecha_creacion,hora_creacion,base_imponible,porcentaje,cuota,total) VALUES " &
               "(@n_factura,@fecha_creacion,@hora_creacion,@base_imponible,@porcentaje,@cuota,@total);"
                memComando.Parameters("@base_imponible").Value = GV_pie.DataKeys(x).Item("base_imponible")
                memComando.Parameters("@porcentaje").Value = GV_pie.DataKeys(x).Item("porcentaje")
                memComando.Parameters("@cuota").Value = GV_pie.DataKeys(x).Item("cuota")
                memComando.Parameters("@total").Value = GV_pie.DataKeys(x).Item("total")
                memComando.ExecuteNonQuery()

                'Asigno
                total_factura += CDec(GV_pie.DataKeys(x).Item("total"))

            Next

            'Agrego
            memComando.CommandText = "INSERT INTO cabecera_facturas (n_factura,fecha_creacion,hora_creacion,fecha,cod_cliente,cliente,nif,direccion,cp,poblacion,provincia,total,origen,n_presupuesto,isp,exento,n_albaran) VALUES " &
            "(@n_factura,@fecha_creacion,@hora_creacion,@fecha,@cod_cliente,@cliente,@nif,@direccion,@cp,@poblacion,@provincia,@total_factura,@origen,@n_presupuesto,@isp,@exento,@n_albaran);"
            memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date).Value = fecha
            memComando.Parameters.Add("@cod_cliente", System.Data.SqlDbType.Int).Value = cod_cliente
            memComando.Parameters.Add("@cliente", System.Data.SqlDbType.VarChar, 250).Value = cliente.ToUpper
            memComando.Parameters.Add("@nif", System.Data.SqlDbType.VarChar, 15).Value = nif
            memComando.Parameters.Add("@direccion", System.Data.SqlDbType.VarChar, 250).Value = direccion.ToUpper
            memComando.Parameters.Add("@cp", System.Data.SqlDbType.VarChar, 6).Value = cp
            memComando.Parameters.Add("@poblacion", System.Data.SqlDbType.VarChar, 80).Value = poblacion.ToUpper
            memComando.Parameters.Add("@provincia", System.Data.SqlDbType.VarChar, 80).Value = provincia.ToUpper
            memComando.Parameters.Add("@total_factura", System.Data.SqlDbType.Decimal, 18, 2).Value = total_factura
            memComando.Parameters.Add("@origen", System.Data.SqlDbType.VarChar, 50).Value = origen
            memComando.Parameters.Add("@n_presupuesto", System.Data.SqlDbType.VarChar, 15).Value = n_presupuesto
            memComando.Parameters.Add("@n_albaran", System.Data.SqlDbType.VarChar, 15).Value = n_albaran
            memComando.Parameters.Add("@isp", System.Data.SqlDbType.Bit, 15).Value = chk_isp
            memComando.Parameters.Add("@exento", System.Data.SqlDbType.Bit, 15).Value = chk_exento
            memComando.ExecuteNonQuery()

            'Actualizar el albaran como que esta facturado
            If n_albaran <> "" Then
                memComando.CommandText = "UPDATE cabecera_albaranes SET facturado='True' WHERE n_albaran='" & n_albaran & "';"
                memComando.ExecuteNonQuery()
            End If

            'Actualizo Contador
            If total_factura > 0 Then
                memComando.CommandText = "UPDATE contadores SET n_factura=n_factura+1;"
                memComando.ExecuteNonQuery()
            Else
                memComando.CommandText = "UPDATE contadores SET n_abono=n_abono+1;"
                memComando.ExecuteNonQuery()
            End If

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Devuelvo
        Return "OK"

    End Function

    Function grabar_presupuestos_(ByVal ruta_base_empresa As String, ByVal n_presupuesto As String, ByVal fecha As Date, ByVal cod_cliente As Integer, ByVal cliente As String, ByVal nif As String, ByVal direccion As String, ByVal cp As String, ByVal poblacion As String, ByVal provincia As String, ByVal GV_detalles As GridView, ByVal GV_pie As GridView) As String

        'Declaro
        Dim total_factura As Decimal = 0

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand

            'Declaro
            memComando.Parameters.Add("@n_presupuesto", System.Data.SqlDbType.VarChar, 15).Value = n_presupuesto
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@linea", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@cantidad", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@cod_articulo", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar, 250)
            memComando.Parameters.Add("@precio", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@dto_1", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@porcentaje_impuesto", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@total_linea", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@observaciones", System.Data.SqlDbType.VarChar, 3000)
            memComando.Parameters.Add("@imagen", System.Data.SqlDbType.VarChar, 50)

            'Agrego detalles
            For x = 0 To GV_detalles.Rows.Count - 1

                memComando.CommandText = "INSERT INTO detalles_presupuestos (n_presupuesto,fecha_creacion,hora_creacion,linea,cantidad,cod_articulo,denominacion,precio,dto_1,porcentaje_impuesto,total,observaciones,imagen) VALUES " &
               "(@n_presupuesto,@fecha_creacion,@hora_creacion,@linea,@cantidad,@cod_articulo,@denominacion,@precio,@dto_1,@porcentaje_impuesto,@total_linea,@observaciones,@imagen);"
                memComando.Parameters("@linea").Value = x
                memComando.Parameters("@cantidad").Value = GV_detalles.DataKeys(x).Item("cantidad")
                memComando.Parameters("@cod_articulo").Value = GV_detalles.DataKeys(x).Item("codigo")
                memComando.Parameters("@denominacion").Value = GV_detalles.DataKeys(x).Item("articulo")
                memComando.Parameters("@precio").Value = GV_detalles.DataKeys(x).Item("precio")
                memComando.Parameters("@dto_1").Value = GV_detalles.DataKeys(x).Item("dto")
                memComando.Parameters("@porcentaje_impuesto").Value = GV_detalles.DataKeys(x).Item("impuesto")
                memComando.Parameters("@total_linea").Value = GV_detalles.DataKeys(x).Item("total")
                If GV_detalles.DataKeys(x).Item("observaciones").ToString.Count <= 68 Or GV_detalles.DataKeys(x).Item("observaciones").ToString.Count = 0 Then
                    memComando.Parameters("@observaciones").Value = ""
                Else
                    memComando.Parameters("@observaciones").Value = GV_detalles.DataKeys(x).Item("observaciones")
                End If
                If GV_detalles.DataKeys(x).Item("imagen").ToString = "" Then
                    memComando.Parameters("@imagen").Value = ""
                Else
                    memComando.Parameters("@imagen").Value = GV_detalles.DataKeys(x).Item("imagen")
                End If
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Si viene con imagen
                If GV_detalles.DataKeys(x).Item("imagen").ToString <> "" Then

                    'Asigno 
                    Dim nombre As String = GV_detalles.DataKeys(x).Item("imagen").ToString
                    Dim ruta_imagen_origen As String = "D:\imagenes_usuarios_facturacion\temp\"
                    Dim ruta_imagen_destino As String = "D:\imagenes_usuarios_facturacion\" & ruta_base_empresa & "\presupuesto\imagenes\"

                    'Creo la carpeta para el fondo de éste usuario
                    If Not System.IO.Directory.Exists(ruta_imagen_destino) Then
                        System.IO.Directory.CreateDirectory(ruta_imagen_destino)
                    End If

                    'Muevo al fila
                    If Not System.IO.File.Exists(ruta_imagen_destino & nombre) Then
                        System.IO.File.Move(ruta_imagen_origen & nombre, ruta_imagen_destino & nombre)
                    End If

                End If

            Next

            'Declaro
            memComando.Parameters.Add("@base_imponible", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@porcentaje", System.Data.SqlDbType.Decimal, 9, 2)
            memComando.Parameters.Add("@cuota", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@total", System.Data.SqlDbType.Decimal, 18, 2)

            'Agrego Pie
            For x = 0 To GV_pie.Rows.Count - 1

                memComando.CommandText = "INSERT INTO pie_presupuestos (n_presupuesto,fecha_creacion,hora_creacion,base_imponible,porcentaje,cuota,total) VALUES " &
               "(@n_presupuesto,@fecha_creacion,@hora_creacion,@base_imponible,@porcentaje,@cuota,@total);"
                memComando.Parameters("@base_imponible").Value = GV_pie.DataKeys(x).Item("base_imponible")
                memComando.Parameters("@porcentaje").Value = GV_pie.DataKeys(x).Item("porcentaje")
                memComando.Parameters("@cuota").Value = GV_pie.DataKeys(x).Item("cuota")
                memComando.Parameters("@total").Value = GV_pie.DataKeys(x).Item("total")
                memComando.ExecuteNonQuery()

                'Asigno
                total_factura += CDec(GV_pie.DataKeys(x).Item("total"))

            Next

            'Agrego
            memComando.CommandText = "INSERT INTO cabecera_presupuestos (n_presupuesto,fecha_creacion,hora_creacion,fecha,cod_cliente,cliente,nif,direccion,cp,poblacion,provincia,total) VALUES " &
            "(@n_presupuesto,@fecha_creacion,@hora_creacion,@fecha,@cod_cliente,@cliente,@nif,@direccion,@cp,@poblacion,@provincia,@total_factura);"
            memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date).Value = fecha
            memComando.Parameters.Add("@cod_cliente", System.Data.SqlDbType.Int).Value = cod_cliente
            memComando.Parameters.Add("@cliente", System.Data.SqlDbType.VarChar, 250).Value = cliente.ToUpper
            memComando.Parameters.Add("@nif", System.Data.SqlDbType.VarChar, 15).Value = nif
            memComando.Parameters.Add("@direccion", System.Data.SqlDbType.VarChar, 250).Value = direccion.ToUpper
            memComando.Parameters.Add("@cp", System.Data.SqlDbType.VarChar, 6).Value = cp
            memComando.Parameters.Add("@poblacion", System.Data.SqlDbType.VarChar, 80).Value = poblacion.ToUpper
            memComando.Parameters.Add("@provincia", System.Data.SqlDbType.VarChar, 80).Value = provincia.ToUpper
            memComando.Parameters.Add("@total_factura", System.Data.SqlDbType.Decimal, 18, 2).Value = total_factura
            memComando.ExecuteNonQuery()

            'Actualizo Contador
            memComando.CommandText = "UPDATE contadores SET n_presupuesto=n_presupuesto+1;"
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Devuelvo
        Return "OK"

    End Function

    Function modificar_presupuestos_(ByVal ruta_base_empresa As String, ByVal n_factura As String, ByVal GV_detalles As GridView, ByVal GV_pie As GridView) As String

        'Declaro
        Dim total_factura As Decimal = 0

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand

            'Borro detalles de presupuesto
            memComando.CommandText = "DELETE detalles_presupuestos WHERE n_presupuesto='" & n_factura & "';"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Declaro
            memComando.Parameters.Add("@n_presupuesto", System.Data.SqlDbType.VarChar, 15).Value = n_factura
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@linea", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@cantidad", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@cod_articulo", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar, 250)
            memComando.Parameters.Add("@precio", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@dto_1", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@porcentaje_impuesto", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@total_linea", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@observaciones", System.Data.SqlDbType.VarChar, 3000)
            memComando.Parameters.Add("@imagen", System.Data.SqlDbType.VarChar, 50)

            'Agrego detalles
            For x = 0 To GV_detalles.Rows.Count - 1

                memComando.CommandText = "INSERT INTO detalles_presupuestos (n_presupuesto,fecha_creacion,hora_creacion,linea,cantidad,cod_articulo,denominacion,precio,dto_1,porcentaje_impuesto,total,observaciones,imagen) VALUES " &
               "(@n_presupuesto,@fecha_creacion,@hora_creacion,@linea,@cantidad,@cod_articulo,@denominacion,@precio,@dto_1,@porcentaje_impuesto,@total_linea,@observaciones,@imagen);"
                memComando.Parameters("@linea").Value = x
                memComando.Parameters("@cantidad").Value = GV_detalles.DataKeys(x).Item("cantidad")
                memComando.Parameters("@cod_articulo").Value = GV_detalles.DataKeys(x).Item("codigo")
                memComando.Parameters("@denominacion").Value = GV_detalles.DataKeys(x).Item("articulo")
                memComando.Parameters("@precio").Value = GV_detalles.DataKeys(x).Item("precio")
                memComando.Parameters("@dto_1").Value = GV_detalles.DataKeys(x).Item("dto")
                memComando.Parameters("@porcentaje_impuesto").Value = GV_detalles.DataKeys(x).Item("impuesto")
                memComando.Parameters("@total_linea").Value = GV_detalles.DataKeys(x).Item("total")
                If GV_detalles.DataKeys(x).Item("observaciones").ToString.Count <= 68 Or GV_detalles.DataKeys(x).Item("observaciones").ToString.Count = 0 Then
                    memComando.Parameters("@observaciones").Value = ""
                Else
                    memComando.Parameters("@observaciones").Value = GV_detalles.DataKeys(x).Item("observaciones")
                End If
                If GV_detalles.DataKeys(x).Item("imagen").ToString = "" Then
                    memComando.Parameters("@imagen").Value = ""
                Else
                    memComando.Parameters("@imagen").Value = GV_detalles.DataKeys(x).Item("imagen")
                End If
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Si viene con imagen
                If GV_detalles.DataKeys(x).Item("imagen").ToString <> "" Then

                    'Asigno 
                    Dim nombre As String = GV_detalles.DataKeys(x).Item("imagen").ToString
                    Dim ruta_imagen_origen As String = "D:\imagenes_usuarios_facturacion\temp\"
                    Dim ruta_imagen_destino As String = "D:\imagenes_usuarios_facturacion\" & ruta_base_empresa & "\presupuesto\imagenes\"

                    'Si existe en temp
                    If My.Computer.FileSystem.FileExists(ruta_imagen_origen & nombre) = True Then

                        'Creo la carpeta para el fondo de éste usuario
                        If Not System.IO.Directory.Exists(ruta_imagen_destino) Then
                            System.IO.Directory.CreateDirectory(ruta_imagen_destino)
                        End If

                        'Muevo al fila
                        System.IO.File.Move(ruta_imagen_origen & nombre, ruta_imagen_destino & nombre)

                    End If

                End If

            Next

            'Borro detalles de presupuesto
            memComando.CommandText = "DELETE pie_presupuestos WHERE n_presupuesto='" & n_factura & "';"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Declaro
            memComando.Parameters.Add("@base_imponible", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@porcentaje", System.Data.SqlDbType.Decimal, 9, 2)
            memComando.Parameters.Add("@cuota", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@total", System.Data.SqlDbType.Decimal, 18, 2)

            'Agrego Pie
            For x = 0 To GV_pie.Rows.Count - 1

                memComando.CommandText = "INSERT INTO pie_presupuestos (n_presupuesto,fecha_creacion,hora_creacion,base_imponible,porcentaje,cuota,total) VALUES " &
               "(@n_presupuesto,@fecha_creacion,@hora_creacion,@base_imponible,@porcentaje,@cuota,@total);"
                memComando.Parameters("@base_imponible").Value = GV_pie.DataKeys(x).Item("base_imponible")
                memComando.Parameters("@porcentaje").Value = GV_pie.DataKeys(x).Item("porcentaje")
                memComando.Parameters("@cuota").Value = GV_pie.DataKeys(x).Item("cuota")
                memComando.Parameters("@total").Value = GV_pie.DataKeys(x).Item("total")
                memComando.ExecuteNonQuery()

                'Asigno
                total_factura += CDec(GV_pie.DataKeys(x).Item("total"))

            Next

            'Agrego
            memComando.CommandText = "UPDATE cabecera_presupuestos SET total=@total_factura WHERE n_presupuesto=@n_presupuesto;"
            memComando.Parameters.Add("@total_factura", System.Data.SqlDbType.Decimal, 18, 2).Value = total_factura
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Devuelvo
        Return "OK"

    End Function

    Function grabar_albaran_(ByVal ruta_base_empresa As String, ByVal n_albaran As String, ByVal fecha As Date, ByVal cod_cliente As Integer, ByVal cliente As String, ByVal nif As String, ByVal direccion As String, ByVal cp As String, ByVal poblacion As String, ByVal provincia As String, ByVal GV_detalles As GridView, ByVal GV_pie As GridView, ByVal origen As String, ByVal n_presupuesto As String) As String

        'Declaro
        Dim total_factura As Decimal = 0

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand

            'Declaro
            memComando.Parameters.Add("@n_albaran", System.Data.SqlDbType.VarChar, 15).Value = n_albaran
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@linea", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@cantidad", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@cod_articulo", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar, 250)
            memComando.Parameters.Add("@precio", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@dto_1", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@porcentaje_impuesto", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@total_linea", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@observaciones", System.Data.SqlDbType.VarChar, 3000)
            memComando.Parameters.Add("@imagen", System.Data.SqlDbType.VarChar, 50)

            'Agrego detalles
            For x = 0 To GV_detalles.Rows.Count - 1

                memComando.CommandText = "INSERT INTO detalles_albaranes (n_albaran,fecha_creacion,hora_creacion,linea,cantidad,cod_articulo,denominacion,precio,dto_1,porcentaje_impuesto,total,observaciones,imagen) VALUES " &
               "(@n_albaran,@fecha_creacion,@hora_creacion,@linea,@cantidad,@cod_articulo,@denominacion,@precio,@dto_1,@porcentaje_impuesto,@total_linea,@observaciones,@imagen);"
                memComando.Parameters("@linea").Value = x
                memComando.Parameters("@cantidad").Value = GV_detalles.DataKeys(x).Item("cantidad")
                memComando.Parameters("@cod_articulo").Value = GV_detalles.DataKeys(x).Item("codigo")
                memComando.Parameters("@denominacion").Value = GV_detalles.DataKeys(x).Item("articulo")
                memComando.Parameters("@precio").Value = GV_detalles.DataKeys(x).Item("precio")
                memComando.Parameters("@dto_1").Value = GV_detalles.DataKeys(x).Item("dto")
                memComando.Parameters("@porcentaje_impuesto").Value = GV_detalles.DataKeys(x).Item("impuesto")
                memComando.Parameters("@total_linea").Value = GV_detalles.DataKeys(x).Item("total")
                If GV_detalles.DataKeys(x).Item("observaciones").ToString.Count <= 68 Or GV_detalles.DataKeys(x).Item("observaciones").ToString.Count = 0 Then
                    memComando.Parameters("@observaciones").Value = ""
                Else
                    memComando.Parameters("@observaciones").Value = GV_detalles.DataKeys(x).Item("observaciones")
                End If
                If GV_detalles.DataKeys(x).Item("imagen").ToString = "" Then
                    memComando.Parameters("@imagen").Value = ""
                Else
                    memComando.Parameters("@imagen").Value = GV_detalles.DataKeys(x).Item("imagen")
                End If
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Si viene con imagen
                If GV_detalles.DataKeys(x).Item("imagen").ToString <> "" Then

                    'Asigno 
                    Dim nombre As String = GV_detalles.DataKeys(x).Item("imagen").ToString
                    Dim ruta_imagen_origen As String = "D:\imagenes_usuarios_facturacion\temp\"
                    Dim ruta_imagen_destino As String = "D:\imagenes_usuarios_facturacion\" & ruta_base_empresa & "\albaran\imagenes\"

                    'Creo la carpeta para el fondo de éste usuario
                    If Not System.IO.Directory.Exists(ruta_imagen_destino) Then
                        System.IO.Directory.CreateDirectory(ruta_imagen_destino)
                    End If

                    'Muevo al fila
                    If Not System.IO.File.Exists(ruta_imagen_destino & nombre) Then
                        System.IO.File.Move(ruta_imagen_origen & nombre, ruta_imagen_destino & nombre)
                    End If

                End If

            Next

            'Declaro
            memComando.Parameters.Add("@base_imponible", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@porcentaje", System.Data.SqlDbType.Decimal, 9, 2)
            memComando.Parameters.Add("@cuota", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@total", System.Data.SqlDbType.Decimal, 18, 2)

            'Agrego Pie
            For x = 0 To GV_pie.Rows.Count - 1

                memComando.CommandText = "INSERT INTO pie_albaranes (n_albaran,fecha_creacion,hora_creacion,base_imponible,porcentaje,cuota,total) VALUES " &
               "(@n_albaran,@fecha_creacion,@hora_creacion,@base_imponible,@porcentaje,@cuota,@total);"
                memComando.Parameters("@base_imponible").Value = GV_pie.DataKeys(x).Item("base_imponible")
                memComando.Parameters("@porcentaje").Value = GV_pie.DataKeys(x).Item("porcentaje")
                memComando.Parameters("@cuota").Value = GV_pie.DataKeys(x).Item("cuota")
                memComando.Parameters("@total").Value = GV_pie.DataKeys(x).Item("total")
                memComando.ExecuteNonQuery()

                'Asigno
                total_factura += CDec(GV_pie.DataKeys(x).Item("total"))

            Next

            'Agrego
            memComando.CommandText = "INSERT INTO cabecera_albaranes (n_albaran,fecha_creacion,hora_creacion,fecha,cod_cliente,cliente,nif,direccion,cp,poblacion,provincia,total,origen,n_presupuesto,facturado) VALUES " &
            "(@n_albaran,@fecha_creacion,@hora_creacion,@fecha,@cod_cliente,@cliente,@nif,@direccion,@cp,@poblacion,@provincia,@total_factura,@origen,@n_presupuesto,'False');"
            memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date).Value = fecha
            memComando.Parameters.Add("@cod_cliente", System.Data.SqlDbType.Int).Value = cod_cliente
            memComando.Parameters.Add("@cliente", System.Data.SqlDbType.VarChar, 250).Value = cliente.ToUpper
            memComando.Parameters.Add("@nif", System.Data.SqlDbType.VarChar, 15).Value = nif
            memComando.Parameters.Add("@direccion", System.Data.SqlDbType.VarChar, 250).Value = direccion.ToUpper
            memComando.Parameters.Add("@cp", System.Data.SqlDbType.VarChar, 6).Value = cp
            memComando.Parameters.Add("@poblacion", System.Data.SqlDbType.VarChar, 80).Value = poblacion.ToUpper
            memComando.Parameters.Add("@provincia", System.Data.SqlDbType.VarChar, 80).Value = provincia.ToUpper
            memComando.Parameters.Add("@total_factura", System.Data.SqlDbType.Decimal, 18, 2).Value = total_factura
            memComando.Parameters.Add("@origen", System.Data.SqlDbType.VarChar, 50).Value = origen
            memComando.Parameters.Add("@n_presupuesto", System.Data.SqlDbType.VarChar, 15).Value = n_presupuesto
            memComando.ExecuteNonQuery()

            'Actualizo Contador
            memComando.CommandText = "UPDATE contadores SET n_albaran=n_albaran+1;"
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Devuelvo
        Return "OK"

    End Function

    Function modificar_albaran_(ByVal ruta_base_empresa As String, ByVal n_albaran As String, ByVal GV_detalles As GridView, ByVal GV_pie As GridView) As String

        'Declaro
        Dim total_factura As Decimal = 0

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand

            'Borro detalles de presupuesto
            memComando.CommandText = "DELETE detalles_albaranes WHERE n_albaran='" & n_albaran & "';"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Declaro
            memComando.Parameters.Add("@n_albaran", System.Data.SqlDbType.VarChar, 15).Value = n_albaran
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@linea", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@cantidad", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@cod_articulo", System.Data.SqlDbType.Int)
            memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar, 250)
            memComando.Parameters.Add("@precio", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@dto_1", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@porcentaje_impuesto", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@total_linea", System.Data.SqlDbType.Decimal, 10, 2)
            memComando.Parameters.Add("@observaciones", System.Data.SqlDbType.VarChar, 3000)
            memComando.Parameters.Add("@imagen", System.Data.SqlDbType.VarChar, 50)

            'Agrego detalles
            For x = 0 To GV_detalles.Rows.Count - 1

                memComando.CommandText = "INSERT INTO detalles_albaranes (n_albaran,fecha_creacion,hora_creacion,linea,cantidad,cod_articulo,denominacion,precio,dto_1,porcentaje_impuesto,total,observaciones,imagen) VALUES " &
               "(@n_albaran,@fecha_creacion,@hora_creacion,@linea,@cantidad,@cod_articulo,@denominacion,@precio,@dto_1,@porcentaje_impuesto,@total_linea,@observaciones,@imagen);"
                memComando.Parameters("@linea").Value = x
                memComando.Parameters("@cantidad").Value = GV_detalles.DataKeys(x).Item("cantidad")
                memComando.Parameters("@cod_articulo").Value = GV_detalles.DataKeys(x).Item("codigo")
                memComando.Parameters("@denominacion").Value = GV_detalles.DataKeys(x).Item("articulo")
                memComando.Parameters("@precio").Value = GV_detalles.DataKeys(x).Item("precio")
                memComando.Parameters("@dto_1").Value = GV_detalles.DataKeys(x).Item("dto")
                memComando.Parameters("@porcentaje_impuesto").Value = GV_detalles.DataKeys(x).Item("impuesto")
                memComando.Parameters("@total_linea").Value = GV_detalles.DataKeys(x).Item("total")
                If GV_detalles.DataKeys(x).Item("observaciones").ToString.Count <= 68 Or GV_detalles.DataKeys(x).Item("observaciones").ToString.Count = 0 Then
                    memComando.Parameters("@observaciones").Value = ""
                Else
                    memComando.Parameters("@observaciones").Value = GV_detalles.DataKeys(x).Item("observaciones")
                End If
                If GV_detalles.DataKeys(x).Item("imagen").ToString = "" Then
                    memComando.Parameters("@imagen").Value = ""
                Else
                    memComando.Parameters("@imagen").Value = GV_detalles.DataKeys(x).Item("imagen")
                End If
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Si viene con imagen
                If GV_detalles.DataKeys(x).Item("imagen").ToString <> "" Then

                    'Asigno 
                    Dim nombre As String = GV_detalles.DataKeys(x).Item("imagen").ToString
                    Dim ruta_imagen_origen As String = "D:\imagenes_usuarios_facturacion\temp\"
                    Dim ruta_imagen_destino As String = "D:\imagenes_usuarios_facturacion\" & ruta_base_empresa & "\albaran\imagenes\"

                    'Si existe en temp
                    If My.Computer.FileSystem.FileExists(ruta_imagen_origen & nombre) = True Then

                        'Creo la carpeta para el fondo de éste usuario
                        If Not System.IO.Directory.Exists(ruta_imagen_destino) Then
                            System.IO.Directory.CreateDirectory(ruta_imagen_destino)
                        End If

                        'Muevo al fila
                        System.IO.File.Move(ruta_imagen_origen & nombre, ruta_imagen_destino & nombre)

                    End If

                End If

            Next

            'Borro detalles de presupuesto
            memComando.CommandText = "DELETE pie_albaranes WHERE n_albaran='" & n_albaran & "';"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Declaro
            memComando.Parameters.Add("@base_imponible", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@porcentaje", System.Data.SqlDbType.Decimal, 9, 2)
            memComando.Parameters.Add("@cuota", System.Data.SqlDbType.Decimal, 18, 2)
            memComando.Parameters.Add("@total", System.Data.SqlDbType.Decimal, 18, 2)

            'Agrego Pie
            For x = 0 To GV_pie.Rows.Count - 1

                memComando.CommandText = "INSERT INTO pie_albaranes (n_albaran,fecha_creacion,hora_creacion,base_imponible,porcentaje,cuota,total) VALUES " &
               "(@n_albaran,@fecha_creacion,@hora_creacion,@base_imponible,@porcentaje,@cuota,@total);"
                memComando.Parameters("@base_imponible").Value = GV_pie.DataKeys(x).Item("base_imponible")
                memComando.Parameters("@porcentaje").Value = GV_pie.DataKeys(x).Item("porcentaje")
                memComando.Parameters("@cuota").Value = GV_pie.DataKeys(x).Item("cuota")
                memComando.Parameters("@total").Value = GV_pie.DataKeys(x).Item("total")
                memComando.ExecuteNonQuery()

                'Asigno
                total_factura += CDec(GV_pie.DataKeys(x).Item("total"))

            Next

            'Agrego
            memComando.CommandText = "UPDATE cabecera_albaranes SET total=@total_factura WHERE n_albaran=@n_albaran;"
            memComando.Parameters.Add("@total_factura", System.Data.SqlDbType.Decimal, 18, 2).Value = total_factura
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Devuelvo
        Return "OK"

    End Function

    Function puntos(ByVal milimetros As Decimal) As Decimal

        'Devulevo
        Return iTextSharp.text.Utilities.MillimetersToPoints(milimetros)

        'Return (milimetros / 0.35)

    End Function

    Function IsValidEmail(ByVal strIn As String) As Boolean

        Try

            'Declaro
            Dim mail As New System.Net.Mail.MailAddress(strIn)

            'Devuelvo
            Return True

        Catch ex As Exception

            'Devuelvo
            Return False

        End Try

    End Function

    Public Function generar_qr_albaran(ByVal bbdd As String, ByVal id_usuario As Integer, ByVal n_albaran As String) As String

        'Asigno el nombre de la imagen QR
        Dim qrCodeImgFileName As String = "QR_" & id_usuario & "_" & bbdd & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "") & ".jpg"

        'Informacion a codificar
        Dim qrCodeInformation As String = bbdd & "|" & n_albaran 

        Dim qe As QRCodeEncoder = New QRCodeEncoder()
        qe.QRCodeScale = 2
        Dim bm As System.Drawing.Bitmap = qe.Encode(qrCodeInformation)
        Dim memStream = New MemoryStream()
        bm.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg)
        Using fileStream = New FileStream(HttpContext.Current.Server.MapPath("~/temp/") & qrCodeImgFileName, FileMode.CreateNew, FileAccess.ReadWrite)
            memStream.Position = 0
            memStream.CopyTo(fileStream)
        End Using

        Return qrCodeImgFileName

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
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        Return tabla_consulta

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
        sbHTML.Replace("&#237;", "í")

        ' Finally, remove all HTML tags and return plain text
        Return sbHTML.ToString()

    End Function



    '-------------------------
    Sub crear_acumulado(ByVal ruta_base_empresa As String, ByVal fecha_inicial As Date, ByVal fecha_final As Date, ByVal identificador As String)

        'Leo los parametros
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        'Leo el plan general de contabilidad
        Dim tabla_plan_general As DataTable = obtener("inforplan", "SELECT id,cuenta,denominacion FROM plan_cuentas_standard " &
            "WHERE id_tipo_plan_cuentas=" & parametros_empresa(6) & " ORDER BY cuenta;")

        'Lo primero es borrar los acumulados anteriores
        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            Dim string_query As String = "DELETE FROM temp_trigger_acumulados WHERE " &
            "fecha BETWEEN '" & fecha_inicial & "' AND '" & fecha_final & "' "

            'Si el identificador Apertura,Asiento o Cierre viene con ALL
            Dim opciones As String = Nothing
            If identificador <> "ALL" Then
                opciones = " AND identificador='" & identificador & "';"
            End If

            'Sentencia completa
            memComando.CommandText = string_query & opciones
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Consulto los movimientos de Detalles_asientos
            Dim sentencia As String = Nothing

            If identificador <> "ALL" Then
                sentencia = "SELECT detalles_asientos.cuenta, year(cabecera_asientos.fecha) as ano, month(cabecera_asientos.fecha) as mes, " &
                "cabecera_asientos.Id_tipo_asientos, cabecera_asientos.identificador_asiento,sum(debe) AS total_debe, sum(haber) AS total_haber " &
                "FROM cabecera_asientos,detalles_asientos " &
                "WHERE cabecera_asientos.id=detalles_asientos.Id_cabecera_asientos " &
                "And cabecera_asientos.fecha BETWEEN '" & fecha_inicial & "' AND '" & fecha_final & "' " &
                "AND identificador_asiento='" & identificador & "' " &
                "GROUP BY detalles_asientos.cuenta, year(cabecera_asientos.fecha), month(cabecera_asientos.fecha), cabecera_asientos.Id_tipo_asientos, cabecera_asientos.identificador_asiento " &
                "ORDER BY year(fecha),month(fecha),detalles_asientos.cuenta"
            Else
                sentencia = "SELECT detalles_asientos.cuenta, year(cabecera_asientos.fecha) as ano, month(cabecera_asientos.fecha) as mes, " &
                "cabecera_asientos.Id_tipo_asientos, cabecera_asientos.identificador_asiento,sum(debe) AS total_debe, sum(haber) AS total_haber " &
                "FROM cabecera_asientos,detalles_asientos " &
                "WHERE cabecera_asientos.id=detalles_asientos.Id_cabecera_asientos " &
                "And cabecera_asientos.fecha BETWEEN '" & fecha_inicial & "' AND '" & fecha_final & "' " &
                "GROUP BY detalles_asientos.cuenta, year(cabecera_asientos.fecha), month(cabecera_asientos.fecha), cabecera_asientos.Id_tipo_asientos, cabecera_asientos.identificador_asiento " &
                "ORDER BY year(fecha),month(fecha),detalles_asientos.cuenta"
            End If

            memComando.CommandText = sentencia
            memComando.Connection = memConn
            Dim da = New SqlDataAdapter(sentencia, memConn)

            'Creo una Tabla
            Dim tabla As New DataTable

            'Cargo los datos
            da.Fill(tabla)

            'Duplico
            Dim tabla_original As New DataTable
            tabla_original = tabla.Copy()

            'Recorro para generar el piramidal
            For x = 0 To tabla.Rows.Count - 1

                For y = CInt(parametros_empresa(7)) - 1 To 1 Step -1

                    Dim Renglon As DataRow = tabla.NewRow()
                    Renglon("cuenta") = Mid(tabla.Rows(x).Item(0), 1, y)
                    Renglon("ano") = tabla.Rows(x).Item(1)
                    Renglon("mes") = tabla.Rows(x).Item(2)
                    Renglon("id_tipo_asientos") = tabla.Rows(x).Item(3)
                    Renglon("identificador_asiento") = tabla.Rows(x).Item(4)
                    Renglon("total_debe") = 0
                    Renglon("total_haber") = 0
                    tabla.Rows.Add(Renglon)

                Next

            Next

            'Creo una vista y quito los duplicados del piramidal
            Dim vista As New DataView(tabla)
            Dim dtsindupl As New DataTable
            dtsindupl = vista.ToTable(True, "cuenta", "ano", "mes", "Id_tipo_asientos", "identificador_asiento", "total_debe", "total_haber")

            'Ordeno por cuenta
            dtsindupl.DefaultView.Sort = "cuenta ASC"
            dtsindupl = dtsindupl.DefaultView.ToTable()

            'Recorro la BBDD para obtener los totales
            memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar)
            For x = 0 To dtsindupl.Rows.Count - 1

                'Si la cuenta trae el mismo numero de digitos que la empresa
                Dim sumaa As String = "0"
                Dim sumab As String = "0"

                If dtsindupl.Rows(x).Item(0).ToString.Count = CInt(parametros_empresa(7)) Then
                    If Not IsDBNull(dtsindupl.Rows(x).Item(5)) Then
                        sumaa = dtsindupl.Rows(x).Item(5).ToString
                    End If
                    If Not IsDBNull(dtsindupl.Rows(x).Item(6)) Then
                        sumab = dtsindupl.Rows(x).Item(6).ToString
                    End If
                Else
                    Dim datos() As String = Obtener_sumdebe_sumhaber(tabla_original, dtsindupl.Rows(x).Item(0).ToString, dtsindupl.Rows(x).Item(1), dtsindupl.Rows(x).Item(2), dtsindupl.Rows(x).Item(3), dtsindupl.Rows(x).Item(4)).Split("|")
                    If datos(0) <> Nothing Then
                        sumaa = datos(0)
                    End If
                    If datos(1) <> Nothing Then
                        sumab = datos(1)
                    End If
                End If

                'Denominaciones para las cuentas
                Dim nombre_cuenta As String = Nothing
                If dtsindupl.Rows(x).Item(0).ToString.Count <= 5 Then
                    nombre_cuenta = Obtener_cuenta(tabla_plan_general, dtsindupl.Rows(x).Item(0), "denominacion")
                Else
                    nombre_cuenta = Obtener_cuenta(HttpContext.Current.Session("tabla_cuentas"), dtsindupl.Rows(x).Item(0), "denominacion")
                End If

                Dim fecha_grabacion As Date = "01/" & dtsindupl.Rows(x)(2) & "/" & dtsindupl.Rows(x).Item(1)
                Dim sentencia_final As String = "INSERT INTO temp_trigger_acumulados (cuenta,denominacion,fecha,identificador,tipo,total_debe,total_haber " &
                 ") VALUES " &
                "('" & dtsindupl.Rows(x).Item(0) & "',@denominacion,'" & fecha_grabacion & "','" & dtsindupl.Rows(x).Item(4) & "'," & dtsindupl.Rows(x).Item(3) & "," & sumaa.Replace(",", ".") & "," & sumab.Replace(",", ".") & " );"
                memComando.Parameters("@denominacion").Value = nombre_cuenta

                memComando.CommandText = sentencia_final
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

            Next

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

    End Sub

    Function Obtener_sumdebe_sumhaber(ByVal contenedor As System.Data.DataTable, ByVal cuenta As String, ByVal ano As Integer, ByVal mes As Integer, ByVal Id_tipo_asientos As Integer, ByVal identificador_asiento As String) As String

        'Hago Select
        Dim expression As String
        expression = "cuenta Like '" & cuenta & "%' AND " &
            "ano= " & ano & " AND " &
            "mes= " & mes & " AND " &
            "Id_tipo_asientos= " & Id_tipo_asientos & " AND " &
            "identificador_asiento= '" & identificador_asiento & "' "
        Dim foundRows() As DataRow

        'Almaceno todos los rows obtenidos
        foundRows = contenedor.Select(expression)

        Dim i As Integer
        Dim sum_debe As Decimal = 0
        Dim sum_haber As Decimal = 0

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            If Not IsDBNull(foundRows(i)(5)) Then
                sum_debe += foundRows(i)(5)
            End If
            If Not IsDBNull(foundRows(i)(6)) Then
                sum_haber += foundRows(i)(6)
            End If

        Next i

        'Devuelvo resultado
        Return sum_debe & "|" & sum_haber

    End Function
    '-------------------------

    Function Obtener_cuenta(ByVal contenedor As System.Data.DataTable, ByVal cuenta As String, Optional ByVal campo As String = "") As String

        'Hago Select
        Dim expression As String
        expression = "cuenta = '" & cuenta & "'"
        Dim foundRows() As DataRow

        ' Almaceno todos los rows obtenidos
        foundRows = contenedor.Select(expression)

        Dim i As Integer
        Dim valor As String = ""

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            For y = 0 To contenedor.Columns.Count - 1

                If campo <> "" Then

                    If contenedor.Columns.Item(y).ColumnName = campo Then
                        valor += foundRows(i)(y)
                    End If

                Else
                    valor += foundRows(i)(y) & "|"
                End If

            Next

        Next i

        'Devuelvo resultado
        Return valor

    End Function

    Function Obtener_nif_datos_cuenta(ByVal contenedor As System.Data.DataTable, ByVal nif As String, ByVal campo As String) As String

        'Hago Select
        Dim expression As String
        expression = "nif = '" & nif & "'"
        Dim foundRows() As DataRow

        ' Almaceno todos los rows obtenidos
        foundRows = contenedor.Select(expression)

        Dim i As Integer
        Dim valor As String = ""

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)
            Select Case campo

                Case "id" : valor = (foundRows(i)(0))
                Case "cuenta" : valor = (foundRows(i)(1))
                Case "denominacion" : valor = (foundRows(i)(2))
                Case "nif"

                    If IsDBNull(foundRows(i)(7)) Then
                        valor = ""
                    Else
                        valor = (foundRows(i)(7))
                    End If

                Case "id_cobrador" : valor = (foundRows(i)(14))
                Case "id_formas_pago" : valor = (foundRows(i)(15))
                Case "id_banco_empresa" : valor = (foundRows(i)(16))
                Case "provincia" : valor = (foundRows(i)(28))


            End Select
        Next i

        'Devuelvo resultado
        Return valor

    End Function

    Public Function fecha_inicial(ByVal año As Integer, ByVal mes_cierre As Integer) As Date

        Dim mes_convertido As Integer = mes_cierre + 1
        If mes_convertido >= 13 Then
            mes_convertido = 1
        End If

        Return "01/" & mes_convertido & "/" & año

    End Function

    Public Function fecha_final(ByVal mes As Integer, ByVal año As Integer, ByVal mes_cierre As Integer) As Date

        Dim mes_convertido As Integer = mes + 1
        If mes_convertido = 13 Then
            mes_convertido = 1
        End If

        If mes_cierre = 12 And mes_convertido = 1 Then
            año = año + 1
        End If

        If mes_cierre <> 12 And mes <= mes_cierre Then
            año = año + 1
        End If

        Dim fecha_convertida As Date = "01/" & mes_convertido & "/" & año
        fecha_convertida = fecha_convertida.AddDays(-1)
        Return fecha_convertida

    End Function

    Public Function fecha_final_informes(ByVal mes As Integer, ByVal año As Integer, ByVal mes_cierre As Integer) As Date

        '1º Lo primero es saber si es ejercicio partido o no
        Dim partido As Boolean = False
        If mes_cierre <> 12 Then
            partido = True
        End If

        If partido = False Then
            Dim dias_mes As Integer = System.DateTime.DaysInMonth(año, mes)
            Return dias_mes & "/" & mes & "/" & año
        Else

            If mes <= mes_cierre Then
                año = año + 1
            End If

            'Obtener_datos dia de mes
            Dim dias_mes As Integer = System.DateTime.DaysInMonth(año, mes)
            Return dias_mes & "/" & mes & "/" & año

        End If

    End Function

    Sub grabar_claudia(ByVal tarea As String, ByVal ruta_base_datos As String, ByVal id_empresa As Integer, ByVal id_usuario As Integer, ByVal parametros As String, ByVal prioridad As Integer)

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "INSERT INTO tareas_IA (fecha,hora,tarea,ruta_base_datos,id_empresa,id_usuario,parametros,prioridad) VALUES (@fecha,@hora,@tarea,@ruta_base_datos,@id_empresa,@id_usuario,@parametros,@prioridad);"
            memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
            memComando.Parameters("@fecha").Value = DateTime.Today
            memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
            memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@tarea", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@tarea").Value = tarea
            memComando.Parameters.Add("@ruta_base_datos", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@ruta_base_datos").Value = ruta_base_datos
            memComando.Parameters.Add("@id_empresa", System.Data.SqlDbType.Int)
            memComando.Parameters("@id_empresa").Value = id_empresa
            memComando.Parameters.Add("@id_usuario", System.Data.SqlDbType.Int)
            memComando.Parameters("@id_usuario").Value = id_usuario
            memComando.Parameters.Add("@parametros", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@parametros").Value = parametros
            memComando.Parameters.Add("@prioridad", System.Data.SqlDbType.Int)
            memComando.Parameters("@prioridad").Value = prioridad
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

    End Sub

    Public Function obtener_soportado_repercutido(ByVal contenedor As System.Data.DataTable, ByVal cuenta As String, ByVal id_tipo_plan_cuentas As Integer) As String

        Try

            'Hago Select
            Dim expression As String
            expression = "id_tipo_plan_cuentas=" & id_tipo_plan_cuentas & " AND cuenta=" & Mid(cuenta, 1, 3) & " "
            Dim foundRows() As DataRow

            ' Almaceno todos los rows obtenidos
            foundRows = contenedor.Select(expression)

            Dim i As Integer
            Dim valor As String = ""

            'Recorro para obtener los datos
            For i = 0 To foundRows.GetUpperBound(0)

                If foundRows(i)("impuesto").IndexOf("SOPORTADO") <> -1 Then
                    valor = "SOPORTADO"
                Else
                    valor = "REPERCUTIDO"
                End If

                'Salgo
                Exit For

            Next i

            'Devuelvo resultado
            Return valor

        Catch ex As Exception

            Return "Error: obtener_soportado_repercutido"

        End Try

    End Function

    Public Function calcular_saldo(ByVal ruta_base_empresa As String, ByVal cuenta As String, ByVal fecha As Date) As String

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT sum(total_debe - total_haber) as total " &
                    "FROM temp_trigger_acumulados " &
                    "WHERE cuenta='" & cuenta & "' " &
                    "AND year(fecha)='" & Year(fecha) & "' " &
                    "AND identificador<>'Cierre';"
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

            If Not IsDBNull(tabla_consulta.Rows(0)("total")) Then
                Return FormatNumber(tabla_consulta.Rows(0)("total"), 2)
            Else
                Return "0,00"
            End If

            'Limpio
            tabla_consulta.Dispose()

        Catch ex As Exception

            Return "Error: calcular_saldo"

        End Try

    End Function



    Public Function URLtoText(ByVal texto As String) As String

        'Limpieza
        texto = texto.Replace("%20", " ")
        texto = texto.Replace("%C3%91", "Ñ")
        texto = texto.Replace("%C3%B1", "ñ")
        texto = texto.Replace("á", "a")
        texto = texto.Replace("é", "e")
        texto = texto.Replace("í", "i")
        texto = texto.Replace("ó", "o")
        texto = texto.Replace("ú", "u")
        texto = texto.Replace("%C3%81", "Á")
        texto = texto.Replace("%C3%89", "É")
        texto = texto.Replace("%C3%8D", "Í")
        texto = texto.Replace("%C3%93", "Ó")
        texto = texto.Replace("%C3%9A", "Ú")
        texto = texto.Replace("%26", "&")

        texto = Trim(texto) 'Quitamos espacios

        Return texto


    End Function

    Function cerrar_ejercicio(ByVal ruta_base_empresa As String, ByVal fecha_inicial As Date, ByVal fecha_final As Date, ByVal id_cierre As Integer, ByVal id_apertura As Integer, ByVal cuenta_129 As String, ByVal denominacion_129 As String, ByVal fecha_apertura As Date) As Integer

        'Variables
        Dim resultado As Integer = 0

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand

            'Grabo la cabecera
            memComando.CommandText = "Usp_cierre_ejercicio"
            memComando.Connection = memConn
            memComando.CommandType = CommandType.StoredProcedure

            'Leo los parametros
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")

            'Parametros de entrada
            memComando.Parameters.Add("@fecha_inicial", SqlDbType.Date, ParameterDirection.Input).Value = fecha_inicial
            memComando.Parameters.Add("@fecha_final", SqlDbType.Date, ParameterDirection.Input).Value = fecha_final
            memComando.Parameters.Add("@id_cierre", SqlDbType.Int, ParameterDirection.Input).Value = id_cierre
            memComando.Parameters.Add("@id_apertura", SqlDbType.Int, ParameterDirection.Input).Value = id_apertura
            memComando.Parameters.Add("@cuenta_129", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = cuenta_129
            memComando.Parameters.Add("@denominacion_129", SqlDbType.VarChar, 200, ParameterDirection.Input).Value = denominacion_129
            memComando.Parameters.Add("@id_cod_usuario", SqlDbType.Int, ParameterDirection.Input).Value = parametros_usuario(0)
            memComando.Parameters.Add("@nivel_cod_usuario", SqlDbType.VarChar, 20, ParameterDirection.Input).Value = parametros_usuario(10)
            memComando.Parameters.Add("@fecha_apertura", SqlDbType.Date, ParameterDirection.Input).Value = fecha_apertura

            'Parametros de salida
            memComando.Parameters.Add("@estado_cierre", SqlDbType.Int)
            memComando.Parameters("@estado_cierre").Direction = ParameterDirection.Output
            memComando.ExecuteNonQuery()

            'Asigno
            resultado = Convert.ToInt32(memComando.Parameters("@estado_cierre").Value)

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno valor
        Return resultado

    End Function

    Public Function asiento_tinfor(ByVal ruta_base_empresa As String, ByVal fecha As String, ByVal tipo_asiento As Integer) As String

        'Transformo en formato Tinfor
        Dim primer_caracter As String = Mid(Year(fecha), 4, 1)
        Dim segundo_caracter As String = Nothing
        Dim tercer_caracter As String = Nothing
        Dim cuarto_caracter As String = Nothing
        Dim tabla_consulta As New DataTable

        'Extraigo el mes
        Select Case Month(fecha)
            Case 1 : segundo_caracter = "EN"
            Case 2 : segundo_caracter = "FB"
            Case 3 : segundo_caracter = "MZ"
            Case 4 : segundo_caracter = "AB"
            Case 5 : segundo_caracter = "MY"
            Case 6 : segundo_caracter = "JN"
            Case 7 : segundo_caracter = "JL"
            Case 8 : segundo_caracter = "AG"
            Case 9 : segundo_caracter = "SP"
            Case 10 : segundo_caracter = "OC"
            Case 11 : segundo_caracter = "NO"
            Case 12 : segundo_caracter = "DC"
        End Select

        'Extraigo el dia
        tercer_caracter = Mid(fecha.ToString, 1, 2)

        'Revisa si existe un asiento anterior para fecha y tipo
        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            'Localizar el ultimo numero de asiento
            Dim memComando As New SqlCommand
            memComando.CommandText = "SELECT referencia_asiento FROM cabecera_asientos WHERE fecha='" & fecha & "' AND " &
                "Id_tipo_asientos='" & tipo_asiento & "' AND identificador_asiento='Asiento' ORDER BY referencia_asiento DESC"
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

        'Si no hay datos
        Dim descomponer As String = Nothing
        If tabla_consulta.Rows.Count = 0 Then
            Return primer_caracter & segundo_caracter & tercer_caracter & "0001"
        Else

            'Recorremos tabla al reves
            For x = 0 To tabla_consulta.Rows.Count - 1

                If (primer_caracter & segundo_caracter & tercer_caracter) = Mid(tabla_consulta.Rows(x)(0).ToString, 1, 5) Then

                    If IsNumeric(Mid(tabla_consulta.Rows(x)(0), 6, 4)) = True Then
                        descomponer = CInt(Mid(tabla_consulta.Rows(x)(0).ToString, 6, 4)) + 1
                        Exit For
                    End If

                End If

            Next

        End If

        'No encontro el ultimo tinfor
        If descomponer = Nothing Then
            Return primer_caracter & segundo_caracter & tercer_caracter & "0001"
        End If

        'Relleno con 0 hacia la izquierda
        Select Case descomponer.Count
            Case 1 : cuarto_caracter = "000" & descomponer
            Case 2 : cuarto_caracter = "00" & descomponer
            Case 3 : cuarto_caracter = "0" & descomponer
            Case Else
                cuarto_caracter = descomponer
        End Select

        Return primer_caracter & segundo_caracter & tercer_caracter & cuarto_caracter

    End Function

    Function grabar_cabecera_asiento_(ByVal ruta_base_empresa As String, ByVal fecha As Date, ByVal id_tipo_asientos As Integer, ByVal identificador_asiento As String, ByVal referencia_asiento As String) As Integer

        'Declaro
        Dim contador As Integer = 0

        'Obtengo el último numero
        Dim tabla As DataTable = obtener(ruta_base_empresa, "SELECT TOP(1) Id FROM cabecera_asientos ORDER BY id DESC;")
        If tabla.Rows.Count <> 0 Then
            contador = CInt(tabla.Rows(0)(0)) + 1
        Else
            contador = 1
        End If

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "INSERT INTO cabecera_asientos (id,fecha_creacion,hora_creacion,fecha,Id_tipo_asientos,identificador_asiento,referencia_asiento) VALUES (" & contador & ",@fecha_creacion,@hora_creacion,@fecha,@Id_tipo_asientos,@identificador_asiento,@referencia_asiento);"

            'Parametros
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date).Value = fecha
            memComando.Parameters.Add("@Id_tipo_asientos", System.Data.SqlDbType.Int).Value = id_tipo_asientos
            memComando.Parameters.Add("@identificador_asiento", System.Data.SqlDbType.VarChar, 50).Value = identificador_asiento
            memComando.Parameters.Add("@referencia_asiento", System.Data.SqlDbType.VarChar, 15).Value = referencia_asiento
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        Return contador

    End Function

    Function grabar_detalles_asientos_(ByVal ruta_base_empresa As String, ByVal Id_cabecera_asiento As Integer, ByVal referencia As String, ByVal cuenta As String, ByVal denominacion_cuenta As String, ByVal nif_cuenta As String, ByVal serie As String, ByVal factura As String, ByVal id_codigo_concepto As Integer, ByVal concepto As String, ByVal debe As Decimal, ByVal haber As Decimal, ByVal efectivo As Boolean, ByVal id_asiento_fijo As Integer, ByVal tipo_asiento_fijo As Integer, ByVal conciliado As Boolean, ByVal fecha_valor As String, ByVal interviene_347 As Boolean, ByVal origen As String, ByVal id_cod_usuario As Integer, ByVal nivel_cod_usuario As String, Optional ByVal id_cabecera_impuestos As String = "0", Optional ByVal id_cabecera_cartera As Integer = 0, Optional ByVal id_cabecera_cobros_pagos As Integer = 0, Optional ByVal id_cabecera_remesa As Integer = 0, Optional ByVal id_cabecera_inmovilizado As String = "0", Optional ByVal Id_detalles_asiento As Integer = 0, Optional ByVal id_apunte_asiento As Integer = 0) As Integer

        'Declaro
        Dim resultado As Integer = 0
        Dim tabla As DataTable

        'Obtengo el último numero, si viene en 0 el id_detalles_asiento
        If Id_detalles_asiento = 0 Then
            tabla = obtener(ruta_base_empresa, "SELECT TOP(1) Id FROM detalles_asientos ORDER BY id DESC;")
            If tabla.Rows.Count <> 0 Then
                Id_detalles_asiento = CInt(tabla.Rows(0)(0)) + 1
            Else
                Id_detalles_asiento = 1
            End If
        End If

        'Obtengo el último numero, si viene en 0 el id_apunte_asiento
        If id_apunte_asiento = 0 Then
            tabla = obtener(ruta_base_empresa, "SELECT TOP(1) apunte FROM detalles_asientos WHERE id_cabecera_asientos=" & Id_cabecera_asiento & " ORDER BY apunte DESC;")
            If tabla.Rows.Count <> 0 Then
                id_apunte_asiento = CInt(tabla.Rows(0)(0)) + 1
            Else
                id_apunte_asiento = 1
            End If
        End If

        'Compruebo si marco 347
        If factura <> "" Then
            Dim soportado_repercutido As String = Nothing
            If debe <> 0 Then soportado_repercutido = "REPERCUTIDO" Else soportado_repercutido = "SOPORTADO"
            If Mid(cuenta, 1, 3) <> "465" And Mid(cuenta, 1, 3) <> "476" Then
                tabla = obtener("inforplan", "SELECT Id FROM cuenta_impuestos_cliente WHERE cuenta=" & Mid(cuenta, 1, 3) & " AND impuesto LIKE '%" & soportado_repercutido & "%';")
                If tabla.Rows.Count <> 0 Then
                    interviene_347 = 1
                End If
            End If
        End If

        'Actualizo
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            'Grabo DEtalles de Asientos
            Dim memComando As New SqlCommand

            'Compruebo efectivo
            If Mid(cuenta, 1, 3) = "570" Then
                tabla = obtener(ruta_base_empresa, "SELECT count(*) FROM detalles_asientos WHERE id_cabecera_asientos=" & Id_cabecera_asiento & " AND interviene_347=1;")
                If tabla.Rows.Count = 1 Then
                    memComando.CommandText = "UPDATE detalles_asientos SET efectivo=1 WHERE id_cabecera_asientos=" & Id_cabecera_asiento & " AND interviene_347=1;"
                    memComando.Connection = memConn
                    memComando.ExecuteNonQuery()
                End If
            End If

            memComando.CommandText = "INSERT INTO detalles_asientos " &
           "(Id" &
           ",Id_cabecera_asientos" &
           ",apunte" &
           ",subapunte" &
           ",referencia" &
           ",cuenta" &
           ",denominacion_cuenta" &
           ",nif_cuenta" &
           ",serie" &
           ",factura" &
           ",Id_codigo_concepto" &
           ",concepto" &
           ",debe" &
           ",haber" &
           ",efectivo" &
           ",fecha_imputacion" &
           ",hora_imputacion" &
           ",importe_final" &
           ",id_asiento_fijo" &
           ",tipo_asiento_fijo" &
           ",conciliado" &
           ",fecha_valor" &
           ",interviene_347" &
           ",origen" &
           ",id_cabecera_impuestos" &
           ",id_cabecera_cartera" &
           ",id_cabecera_cobros_pagos" &
           ",id_cabecera_remesa" &
           ",id_cabecera_inmovilizado" &
           ",id_cod_usuario" &
           ",nivel_cod_usuario) " &
        "VALUES " &
           "(@Id" &
           ",@Id_cabecera_asientos" &
           ",@apunte" &
           ",@subapunte" &
           ",@referencia" &
           ",@cuenta" &
           ",@denominacion_cuenta" &
           ",@nif_cuenta" &
           ",@serie" &
           ",@factura" &
           ",@Id_codigo_concepto" &
           ",@concepto" &
           ",@debe" &
           ",@haber" &
           ",@efectivo" &
           ",@fecha_imputacion" &
           ",@hora_imputacion" &
           ",@importe_final" &
           ",@id_asiento_fijo" &
           ",@tipo_asiento_fijo" &
           ",@conciliado" &
           ",@fecha_valor" &
           ",@interviene_347" &
           ",@origen" &
           ",@id_cabecera_impuestos" &
           ",@id_cabecera_cartera" &
           ",@id_cabecera_cobros_pagos" &
           ",@id_cabecera_remesa" &
           ",@id_cabecera_inmovilizado" &
           ",@id_cod_usuario" &
           ",@nivel_cod_usuario)"

            'Parametros en entrada
            memComando.Parameters.Add("@id", SqlDbType.Int).Value = Id_detalles_asiento
            memComando.Parameters.Add("@Id_cabecera_asientos", SqlDbType.Int).Value = Id_cabecera_asiento
            memComando.Parameters.Add("@apunte", SqlDbType.Int).Value = id_apunte_asiento
            memComando.Parameters.Add("@subapunte", SqlDbType.Int).Value = 0
            memComando.Parameters.Add("@referencia", SqlDbType.VarChar, 20).Value = referencia
            memComando.Parameters.Add("@cuenta", SqlDbType.VarChar, 15).Value = cuenta
            memComando.Parameters.Add("@denominacion_cuenta", SqlDbType.VarChar, 200).Value = denominacion_cuenta
            memComando.Parameters.Add("@nif_cuenta", SqlDbType.VarChar, 15).Value = nif_cuenta
            memComando.Parameters.Add("@serie", SqlDbType.VarChar, 6).Value = serie
            memComando.Parameters.Add("@factura", SqlDbType.VarChar, 20).Value = factura
            memComando.Parameters.Add("@id_codigo_concepto", SqlDbType.Int).Value = id_codigo_concepto
            memComando.Parameters.Add("@concepto", SqlDbType.VarChar, 200).Value = concepto
            memComando.Parameters.Add("@debe", SqlDbType.Decimal).Value = debe
            memComando.Parameters.Add("@haber", SqlDbType.Decimal).Value = haber
            memComando.Parameters.Add("@efectivo", SqlDbType.Bit).Value = efectivo
            memComando.Parameters.Add("@fecha_imputacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_imputacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            Dim importe_final As Decimal = 0
            If debe <> 0 Then importe_final = debe Else importe_final = haber
            memComando.Parameters.Add("@importe_final", SqlDbType.Decimal).Value = importe_final
            memComando.Parameters.Add("@id_asiento_fijo", SqlDbType.Int).Value = id_asiento_fijo
            memComando.Parameters.Add("@tipo_asiento_fijo", SqlDbType.Int).Value = tipo_asiento_fijo
            memComando.Parameters.Add("@conciliado", SqlDbType.Bit).Value = conciliado
            If fecha_valor = "" Then
                memComando.Parameters.Add("@fecha_valor", System.Data.SqlDbType.Date).Value = DBNull.Value
            Else
                memComando.Parameters.Add("@fecha_valor", System.Data.SqlDbType.Date).Value = Mid(fecha_valor, 1, 10)
            End If
            memComando.Parameters.Add("@interviene_347", SqlDbType.Bit).Value = interviene_347
            memComando.Parameters.Add("@origen", SqlDbType.VarChar, 15).Value = origen
            memComando.Parameters.Add("@id_cabecera_impuestos", SqlDbType.VarChar, 50).Value = id_cabecera_impuestos
            memComando.Parameters.Add("@id_cabecera_cartera", SqlDbType.Int).Value = id_cabecera_cartera
            memComando.Parameters.Add("@id_cabecera_cobros_pagos", SqlDbType.Int).Value = id_cabecera_cobros_pagos
            memComando.Parameters.Add("@id_cabecera_remesa", SqlDbType.Int).Value = id_cabecera_remesa
            memComando.Parameters.Add("@id_cabecera_inmovilizado", SqlDbType.VarChar, 8000).Value = id_cabecera_inmovilizado
            memComando.Parameters.Add("@id_cod_usuario", SqlDbType.Int).Value = id_cod_usuario
            memComando.Parameters.Add("@nivel_cod_usuario", SqlDbType.VarChar, 8000).Value = nivel_cod_usuario
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Asigno
            resultado = Id_detalles_asiento

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno valor
        Return resultado

    End Function

    Function grabar_cabecera_impuesto_(ByVal ruta_base_empresa As String, ByVal n_usuario As Integer, ByVal fecha_expedicion As String, ByVal fecha_realizacion As String, ByVal cuenta As String, ByVal denominacion As String, ByVal nif As String, ByVal tipo_impuesto As String, ByVal cuenta_cancelacion As String, ByVal efectivo As Boolean, ByVal serie As String, ByVal n_factura As String, ByVal codigo_concepto As String, ByVal concepto As String, ByVal base_retencion As String, ByVal porcentaje_retencion As String, ByVal cuota_retencion As String, ByVal cuenta_retencion As String, ByVal total_factura As String, ByVal chb_retencion As Boolean, ByVal prorrata As String, ByVal chb_minorista As Boolean, ByVal chb_exento As Boolean, ByVal chb_re As Boolean, ByVal chb_no_sujeto As Boolean, ByVal chb_prorrata As Boolean, ByVal denominacion_tipo_asiento As String, ByVal referencia_asiento As String, ByVal n_asiento As String, ByVal id_cabecera_asiento As Integer) As String

        'Declaro
        Dim identificador As String = Nothing

        'Actualizo
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            'Grabo DEtalles de Asientos
            Dim memComando As New SqlCommand

            'Identificador de la cabecera de impuestos
            identificador = n_usuario & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "")

            'Grabo Datos
            memComando.CommandText = "INSERT INTO [cabecera_impuestos] (id,fecha_creacion,hora_creacion,fecha_expedicion,fecha_realizacion,cuenta,denominacion,nif,tipo_impuesto,cuenta_cancelacion,efectivo,serie_factura,numero_factura,codigo_concepto_impuesto,naturaleza,base_retencion,porcentaje_retencion,cuota_retencion,cuenta_retencion,total_factura_retencion,total_factura,retencion,prorrata,minorista,exento,re,prorrata_check,no_sujeto,tipo_asiento,referencia_siento,residuo) VALUES (@identificador,@fecha,@hora,@fecha_expedicion,@fecha_realizacion,@cuenta,@denominacion,@nif,@tipo_impuesto,@cuenta_cancelacion,@efectivo,@serie_factura,@numero_factura,@codigo_concepto_impuesto,@naturaleza,@base_retencion,@porcentaje_retencion,@cuota_retencion,@cuenta_retencion,@total_factura_retencion,@total_factura,@retencion,@prorrata,@minorista,@exento,@re,@prorrata_check,@no_sujeto,@tipo_asiento,@referencia_siento,@residuo);"
            memComando.Parameters.Add("@identificador", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@identificador").Value = identificador
            memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
            memComando.Parameters("@fecha").Value = DateTime.Today
            memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
            memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@fecha_expedicion", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@fecha_expedicion").Value = fecha_expedicion
            memComando.Parameters.Add("@fecha_realizacion", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@fecha_realizacion").Value = fecha_realizacion
            memComando.Parameters.Add("@cuenta", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@cuenta").Value = cuenta
            memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@denominacion").Value = denominacion
            memComando.Parameters.Add("@nif", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@nif").Value = nif
            memComando.Parameters.Add("@tipo_impuesto", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@tipo_impuesto").Value = tipo_impuesto
            memComando.Parameters.Add("@cuenta_cancelacion", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@cuenta_cancelacion").Value = cuenta_cancelacion
            memComando.Parameters.Add("@efectivo", System.Data.SqlDbType.Bit)
            memComando.Parameters("@efectivo").Value = efectivo
            memComando.Parameters.Add("@serie_factura", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@serie_factura").Value = serie
            memComando.Parameters.Add("@numero_factura", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@numero_factura").Value = n_factura
            memComando.Parameters.Add("@codigo_concepto_impuesto", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@codigo_concepto_impuesto").Value = codigo_concepto
            memComando.Parameters.Add("@naturaleza", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@naturaleza").Value = concepto
            memComando.Parameters.Add("@base_retencion", System.Data.SqlDbType.Decimal)
            If base_retencion = "" Then base_retencion = 0
            memComando.Parameters("@base_retencion").Value = base_retencion.Replace(".", ",")
            memComando.Parameters.Add("@porcentaje_retencion", System.Data.SqlDbType.Decimal)
            memComando.Parameters("@porcentaje_retencion").Value = porcentaje_retencion
            memComando.Parameters.Add("@cuota_retencion", System.Data.SqlDbType.Decimal)
            If cuota_retencion = "" Then cuota_retencion = 0
            memComando.Parameters("@cuota_retencion").Value = cuota_retencion.Replace(".", ",")
            memComando.Parameters.Add("@cuenta_retencion", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@cuenta_retencion").Value = cuenta_retencion
            memComando.Parameters.Add("@total_factura_retencion", System.Data.SqlDbType.Decimal)
            memComando.Parameters("@total_factura_retencion").Value = total_factura.Replace(".", ",") - cuota_retencion.Replace(".", ",")
            memComando.Parameters.Add("@total_factura", System.Data.SqlDbType.Decimal)
            memComando.Parameters("@total_factura").Value = total_factura.Replace(".", ",")
            memComando.Parameters.Add("@retencion", System.Data.SqlDbType.Bit)
            memComando.Parameters("@retencion").Value = chb_retencion
            memComando.Parameters.Add("@prorrata", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@prorrata").Value = prorrata
            memComando.Parameters.Add("@minorista", System.Data.SqlDbType.Bit)
            memComando.Parameters("@minorista").Value = chb_minorista
            memComando.Parameters.Add("@exento", System.Data.SqlDbType.Bit)
            memComando.Parameters("@exento").Value = chb_exento
            memComando.Parameters.Add("@re", System.Data.SqlDbType.Bit)
            memComando.Parameters("@re").Value = chb_re
            memComando.Parameters.Add("@prorrata_check", System.Data.SqlDbType.Bit)
            memComando.Parameters("@prorrata_check").Value = chb_prorrata
            memComando.Parameters.Add("@no_sujeto", System.Data.SqlDbType.Bit)
            memComando.Parameters("@no_sujeto").Value = chb_no_sujeto
            memComando.Parameters.Add("@tipo_asiento", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@tipo_asiento").Value = denominacion_tipo_asiento
            memComando.Parameters.Add("@referencia_siento", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@referencia_siento").Value = referencia_asiento
            memComando.Parameters.Add("@residuo", System.Data.SqlDbType.VarChar)
            memComando.Parameters("@residuo").Value = n_asiento & "&" & id_cabecera_asiento
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno valor
        Return identificador

    End Function

    Function grabar_detalles_impuestos_(ByVal ruta_base_empresa As String, ByVal Id_cabecera_impuesto As String, ByVal exento As Boolean,
            ByVal clave As String, ByVal porcentaje As Decimal, ByVal cuota As Decimal, ByVal base As Decimal,
            ByVal porcentaje_re As Decimal, ByVal cuota_re As Decimal, ByVal cuenta_re As String, ByVal cuenta_cuota As String,
            ByVal cuenta_base As String, ByVal porcentaje_minorista As Decimal, ByVal cuota_minorista As Decimal,
            ByVal cuota_prorrata As Decimal, ByVal cuota_deducible As Decimal, ByVal base_calculada As Decimal,
            ByVal intracomunitaria_inversion_sujeto As String, ByVal suma_base As Boolean, ByVal suma_cuota As Boolean,
            ByVal cuadre_importacion As Boolean, Optional orden As Integer = 0) As Integer

        'Declaro
        Dim resultado As Integer = 0
        Dim tabla As DataTable

        'Obtengo el último orden, si viene en 0 el orden
        If orden = 0 Then
            tabla = obtener(ruta_base_empresa, "SELECT TOP(1) orden FROM detalles_impuestos WHERE id_cabecera_impuestos='" & Id_cabecera_impuesto & "' ORDER BY orden DESC;")
            If tabla.Rows.Count <> 0 Then
                orden = CInt(tabla.Rows(0)(0)) + 1
            Else
                orden = 1
            End If
        End If

        'Actualizo
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            'Grabo DEtalles de Asientos
            Dim memComando As New SqlCommand
            memComando.CommandText = "INSERT INTO detalles_impuestos " &
           "(Id_cabecera_impuestos" &
           ",fecha_creacion" &
           ",hora_creacion" &
           ",orden" &
           ",exento" &
           ",clave" &
           ",porcentaje" &
           ",cuota" &
           ",base" &
           ",porcentaje_re" &
           ",cuota_re" &
           ",cuenta_re" &
           ",cuenta_cuota" &
           ",cuenta_base" &
           ",porcentaje_minorista" &
           ",cuota_minorista" &
           ",cuota_prorrata" &
           ",cuota_deducible" &
           ",base_calculada" &
           ",intracomunitaria_inversion_sujeto" &
           ",suma_base" &
           ",suma_cuota" &
           ",cuadre_importacion)" &
        "VALUES " &
           "(@Id_cabecera_impuestos" &
           ",@fecha_creacion" &
           ",@hora_creacion" &
           ",@orden" &
           ",@exento" &
           ",@clave" &
           ",@porcentaje" &
           ",@cuota" &
           ",@base" &
           ",@porcentaje_re" &
           ",@cuota_re" &
           ",@cuenta_re" &
           ",@cuenta_cuota" &
           ",@cuenta_base" &
           ",@porcentaje_minorista" &
           ",@cuota_minorista" &
           ",@cuota_prorrata" &
           ",@cuota_deducible" &
           ",@base_calculada" &
           ",@intracomunitaria_inversion_sujeto" &
           ",@suma_base" &
           ",@suma_cuota" &
           ",@cuadre_importacion);"

            'Parametros en entrada
            memComando.Parameters.Add("@Id_cabecera_impuestos", SqlDbType.VarChar, 50).Value = Id_cabecera_impuesto
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@orden", SqlDbType.Int).Value = orden
            memComando.Parameters.Add("@exento", System.Data.SqlDbType.Bit).Value = exento
            memComando.Parameters.Add("@clave", SqlDbType.VarChar, 100).Value = clave
            memComando.Parameters.Add("@porcentaje", System.Data.SqlDbType.Decimal).Value = porcentaje
            memComando.Parameters.Add("@cuota", System.Data.SqlDbType.Decimal).Value = cuota
            memComando.Parameters.Add("@base", System.Data.SqlDbType.Decimal).Value = base
            memComando.Parameters.Add("@porcentaje_re", System.Data.SqlDbType.Decimal).Value = porcentaje_re
            memComando.Parameters.Add("@cuota_re", System.Data.SqlDbType.Decimal).Value = cuota_re
            memComando.Parameters.Add("@cuenta_re", System.Data.SqlDbType.VarChar).Value = cuenta_re
            memComando.Parameters.Add("@cuenta_cuota", System.Data.SqlDbType.VarChar).Value = cuenta_cuota
            memComando.Parameters.Add("@cuenta_base", System.Data.SqlDbType.VarChar).Value = cuenta_base
            memComando.Parameters.Add("@porcentaje_minorista", System.Data.SqlDbType.Decimal).Value = porcentaje_minorista
            memComando.Parameters.Add("@cuota_minorista", System.Data.SqlDbType.Decimal).Value = cuota_minorista
            memComando.Parameters.Add("@cuota_prorrata", System.Data.SqlDbType.Decimal).Value = cuota_prorrata
            memComando.Parameters.Add("@cuota_deducible", System.Data.SqlDbType.Decimal).Value = cuota_deducible
            memComando.Parameters.Add("@base_calculada", System.Data.SqlDbType.Decimal).Value = base_calculada
            memComando.Parameters.Add("@intracomunitaria_inversion_sujeto", System.Data.SqlDbType.VarChar, 30).Value = intracomunitaria_inversion_sujeto
            memComando.Parameters.Add("@suma_cuota", System.Data.SqlDbType.Bit).Value = suma_cuota
            memComando.Parameters.Add("@suma_base", System.Data.SqlDbType.Bit).Value = suma_base
            memComando.Parameters.Add("@cuadre_importacion", System.Data.SqlDbType.Bit).Value = cuadre_importacion
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Asigno
            resultado = orden

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno valor
        Return resultado

    End Function

    Function grabar_cabecera_inmovilizado_(ByVal ruta_base_empresa As String, ByVal id As Integer, ByVal denominacion As String, ByVal codigo_barras As String,
        ByVal codigo_grupo As Integer, ByVal ubicacion As Integer, ByVal fecha_adquisicion As Date, ByVal serie As String, ByVal factura As String,
        ByVal importe_factura As Decimal, ByVal cuenta_inmovilizado As String, ByVal cuenta_proveedor As String, ByVal cuenta_debe_amortizacion As String, ByVal cuenta_haber_amortizacion As String,
        ByVal fecha_primera_amortizacion As Date, ByVal fecha_inicio_amortizacion As Date, ByVal tipo_amortizacion As String, ByVal valor As Decimal,
        ByVal valor_adquisicion As Decimal, ByVal valor_anterior_amortizado As Decimal, ByVal valor_residual As Decimal,
        ByVal valor_a_amortizacion As Decimal, ByVal contabiliza As Boolean,
        ByVal anual As Boolean, ByVal acumulado As Boolean, ByVal vendido As Boolean, ByVal baja As Boolean,
        ByVal n_asiento_compra As Integer, ByVal id_cod_usuario As Integer, ByVal nivel_cod_usuario As String) As Integer

        'Actualizo
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            'Grabo DEtalles de Asientos
            Dim memComando As New SqlCommand
            memComando.CommandText = "INSERT INTO cabecera_inmovilizado " &
           "(Id" &
           ",fecha_creacion" &
           ",hora_creacion" &
           ",denominacion" &
           ",codigo_barras" &
           ",codigo_grupo" &
           ",ubicacion" &
           ",fecha_adquisicion" &
           ",serie" &
           ",factura" &
           ",importe_factura" &
           ",cuenta_inmovilizado" &
           ",cuenta_proveedor" &
           ",cuenta_debe_amortizacion" &
           ",cuenta_haber_amortizacion" &
           ",fecha_primera_amortizacion" &
           ",fecha_inicio_amortizacion" &
           ",tipo_amortizacion" &
           ",valor" &
           ",valor_adquisicion" &
           ",valor_anterior_amortizado" &
           ",valor_residual" &
           ",valor_a_amortizacion" &
           ",contabiliza" &
           ",anual" &
           ",acumulado" &
           ",vendido" &
           ",baja" &
           ",id_cod_usuario" &
           ",nivel_cod_usuario" &
           ",id_cabecera_asiento_compra) " &
        "VALUES " &
           "(@Id" &
           ",@fecha_creacion" &
           ",@hora_creacion" &
           ",@denominacion" &
           ",@codigo_barras" &
           ",@codigo_grupo" &
           ",@ubicacion" &
           ",@fecha_adquisicion" &
           ",@serie" &
           ",@factura" &
           ",@importe_factura" &
           ",@cuenta_inmovilizado" &
           ",@cuenta_proveedor" &
           ",@cuenta_debe_amortizacion" &
           ",@cuenta_haber_amortizacion" &
           ",@fecha_primera_amortizacion" &
           ",@fecha_inicio_amortizacion" &
           ",@tipo_amortizacion_cabecera" &
           ",@valor_cabecera" &
           ",@valor_adquisicion" &
           ",@valor_anterior_amortizado" &
           ",@valor_residual" &
           ",@valor_a_amortizacion" &
           ",@contabiliza" &
           ",@anual" &
           ",@acumulado" &
           ",@vendido" &
           ",@baja" &
           ",@id_cod_usuario" &
           ",@nivel_cod_usuario" &
           ",@id_cabecera_asiento_compra);"

            'Parametros en entrada
            memComando.Parameters.Add("@id", SqlDbType.Int).Value = id
            memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date).Value = DateTime.Today
            memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time).Value = Now.ToString("HH:mm:ss")
            memComando.Parameters.Add("@denominacion", SqlDbType.VarChar, 100, ParameterDirection.Input).Value = denominacion
            memComando.Parameters.Add("@codigo_barras", SqlDbType.VarChar, 15).Value = codigo_barras
            memComando.Parameters.Add("@codigo_grupo", SqlDbType.Int).Value = codigo_grupo
            memComando.Parameters.Add("@ubicacion", SqlDbType.Int).Value = ubicacion
            memComando.Parameters.Add("@fecha_adquisicion", SqlDbType.Date, ParameterDirection.Input).Value = fecha_adquisicion
            memComando.Parameters.Add("@serie", SqlDbType.VarChar, 7).Value = serie
            memComando.Parameters.Add("@factura", SqlDbType.VarChar, 20).Value = factura
            memComando.Parameters.Add("@importe_factura", SqlDbType.Decimal).Value = importe_factura
            memComando.Parameters.Add("@cuenta_inmovilizado", SqlDbType.VarChar, 15).Value = cuenta_inmovilizado
            memComando.Parameters.Add("@cuenta_proveedor", SqlDbType.VarChar, 15).Value = cuenta_proveedor
            memComando.Parameters.Add("@cuenta_debe_amortizacion", SqlDbType.VarChar, 15).Value = cuenta_debe_amortizacion
            memComando.Parameters.Add("@cuenta_haber_amortizacion", SqlDbType.VarChar, 15).Value = cuenta_haber_amortizacion
            memComando.Parameters.Add("@fecha_primera_amortizacion", SqlDbType.Date).Value = fecha_primera_amortizacion
            memComando.Parameters.Add("@fecha_inicio_amortizacion", SqlDbType.Date).Value = fecha_inicio_amortizacion
            memComando.Parameters.Add("@tipo_amortizacion_cabecera", SqlDbType.VarChar, 10).Value = tipo_amortizacion
            memComando.Parameters.Add("@valor_cabecera", SqlDbType.Decimal).Value = valor
            memComando.Parameters.Add("@valor_adquisicion", SqlDbType.Decimal).Value = valor_adquisicion
            memComando.Parameters.Add("@valor_anterior_amortizado", SqlDbType.Decimal).Value = valor_anterior_amortizado
            memComando.Parameters.Add("@valor_residual", SqlDbType.Decimal).Value = valor_residual
            memComando.Parameters.Add("@valor_a_amortizacion", SqlDbType.Decimal).Value = valor_a_amortizacion
            memComando.Parameters.Add("@contabiliza", SqlDbType.Bit).Value = contabiliza
            memComando.Parameters.Add("@anual", SqlDbType.Bit).Value = anual
            memComando.Parameters.Add("@acumulado", SqlDbType.Bit).Value = acumulado
            memComando.Parameters.Add("@vendido", SqlDbType.Bit).Value = vendido
            memComando.Parameters.Add("@baja", SqlDbType.Bit).Value = baja
            memComando.Parameters.Add("@id_cod_usuario", SqlDbType.Int).Value = id_cod_usuario
            memComando.Parameters.Add("@nivel_cod_usuario", SqlDbType.VarChar, 20).Value = nivel_cod_usuario
            memComando.Parameters.Add("@id_cabecera_asiento_compra", SqlDbType.Int).Value = n_asiento_compra
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Grabo los detelles
            grabar_detalles_inmovilizado_(ruta_base_empresa, id, fecha_primera_amortizacion, valor_a_amortizacion, tipo_amortizacion, valor)

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno valor
        Return id

    End Function

    Function grabar_detalles_inmovilizado_(ByVal ruta_base_empresa As String, ByVal n_inmovilizado As Integer, ByVal fecha As Date, ByVal importe As Decimal, ByVal tipo_amortizacion As String, ByVal valor As Decimal) As Integer

        'Actualizo
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & ruta_base_empresa)

            'Abrimos conexion
            memConn.Open()

            'Grabo Detalles 
            Dim memComando As New SqlCommand
            memComando.CommandText = "Usp_detalles_inmovilizado"
            memComando.Connection = memConn
            memComando.CommandType = CommandType.StoredProcedure

            'Parametros de entrada
            memComando.Parameters.Add("@n_inmovilizado", SqlDbType.Int, ParameterDirection.Input).Value = n_inmovilizado
            memComando.Parameters.Add("@fecha", SqlDbType.Date, ParameterDirection.Input).Value = fecha
            memComando.Parameters.Add("@importe", SqlDbType.Decimal, ParameterDirection.Input).Value = importe
            memComando.Parameters.Add("@tipo_amortizacion", SqlDbType.VarChar, 10, ParameterDirection.Input).Value = tipo_amortizacion
            memComando.Parameters.Add("@valor", SqlDbType.Decimal, ParameterDirection.Input).Value = valor

            'Parametros de salida
            memComando.Parameters.Add("@id_cabecera_inmovilizado", SqlDbType.Int)
            memComando.Parameters("@id_cabecera_inmovilizado").Direction = ParameterDirection.Output
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno valor
        Return n_inmovilizado

    End Function



    Function IA_cabecera_impuestos(ByVal bbdd As String, ByVal fecha_expedicion As Date, ByVal cuenta As String) As String

        'Declaro
        Dim tabla_consulta As New DataTable
        Dim tabla_consulta2 As New DataTable
        Dim tabla_consulta3 As New DataTable
        Dim resultado As String = ""

        'Actualizo
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd)

            'Abrimos conexion
            memConn.Open()

            'Grabo DEtalles de Asientos
            Dim memComando As New SqlCommand
            memComando.CommandText = "SELECT TOP(5) fecha_expedicion " &
                    "FROM cabecera_impuestos " &
                    "WHERE cuenta='" & cuenta & "' " &
                    "ORDER BY fecha_expedicion DESC;"
            memComando.Connection = memConn

            'Creamos un adaptador de datos
            Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

            'Llenamos de datos
            adapter.Fill(tabla_consulta2)

            If tabla_consulta2.Rows.Count > 0 Then

                Dim fecha_final As Date = tabla_consulta2.Rows(0)("fecha_expedicion")
                Dim fecha_inicial As Date = tabla_consulta2.Rows(tabla_consulta2.Rows.Count - 1)("fecha_expedicion")

                memComando.CommandText = "SELECT count(*) as contador,cabecera_impuestos.[cuenta],cabecera_impuestos.[denominacion],cabecera_impuestos.[nif],cabecera_impuestos.[tipo_impuesto],cabecera_impuestos.[cuenta_cancelacion],cabecera_impuestos.[efectivo],cabecera_impuestos.[serie_factura],cabecera_impuestos.[codigo_concepto_impuesto],cabecera_impuestos.[porcentaje_retencion],cabecera_impuestos.[cuenta_retencion],cabecera_impuestos.[retencion],cabecera_impuestos.[prorrata],cabecera_impuestos.[minorista],cabecera_impuestos.exento,cabecera_impuestos.[re],cabecera_impuestos.[no_sujeto],cabecera_impuestos.[prorrata_check],forzar_peticion,cabecera_impuestos.[base_retencion],cabecera_impuestos.[total_factura_retencion] " &
                "FROM cabecera_impuestos, plan_cuenta_personalizadas " &
                "WHERE cabecera_impuestos.fecha_expedicion BETWEEN '" & fecha_inicial & "' AND '" & fecha_final & "' " &
                "AND cabecera_impuestos.cuenta='" & cuenta & "' " &
                "AND cabecera_impuestos.cuenta = plan_cuenta_personalizadas.cuenta " &
                "GROUP BY cabecera_impuestos.[cuenta] " &
                ",cabecera_impuestos.[denominacion] " &
                ",cabecera_impuestos.[nif] " &
                ",cabecera_impuestos.[tipo_impuesto] " &
                ",cabecera_impuestos.[cuenta_cancelacion] " &
                ",cabecera_impuestos.[efectivo] " &
                ",cabecera_impuestos.[serie_factura] " &
                ",cabecera_impuestos.[codigo_concepto_impuesto] " &
                ",cabecera_impuestos.[porcentaje_retencion] " &
                ",cabecera_impuestos.[cuenta_retencion] " &
                ",cabecera_impuestos.[retencion] " &
                ",cabecera_impuestos.[prorrata] " &
                ",cabecera_impuestos.[minorista] " &
                ",cabecera_impuestos.[exento] " &
                ",cabecera_impuestos.[re] " &
                ",cabecera_impuestos.[no_sujeto] " &
                ",cabecera_impuestos.[prorrata_check] " &
                ",plan_cuenta_personalizadas.[forzar_peticion] " &
                ",cabecera_impuestos.[base_retencion] " &
                ",cabecera_impuestos.[total_factura_retencion] " &
                "ORDER BY contador DESC;"
                memComando.Connection = memConn

                'Llenamos de datos
                adapter.Fill(tabla_consulta)

                memComando.CommandText = "SELECT TOP(2) total_factura " &
                "FROM cabecera_impuestos " &
                "WHERE cabecera_impuestos.fecha_expedicion BETWEEN '" & fecha_inicial & "' AND '" & fecha_final & "' " &
                "AND cabecera_impuestos.cuenta ='" & cuenta & "' " &
                "GROUP BY cabecera_impuestos.total_factura;"
                memComando.Connection = memConn

                'Llenamos de datos
                adapter.Fill(tabla_consulta3)

            End If

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Comprobacion
        If tabla_consulta.Rows.Count = 0 Then

            Return resultado

        Else

            For y = 0 To tabla_consulta.Columns.Count - 1
                resultado += tabla_consulta.Rows(0)(y).ToString & "|"
            Next

            'Obtengo el total de factura
            If tabla_consulta3.Rows.Count = 1 Then
                'Asigno
                resultado += tabla_consulta3.Rows(0)("total_factura").ToString.Replace(",", ".")
            Else
                resultado += "0"
            End If

            Return resultado

        End If

        'Limpiar
        tabla_consulta.Dispose()
        tabla_consulta2.Dispose()
        tabla_consulta3.Dispose()

    End Function

    Function IA_detalles_impuesto(ByVal bbdd As String, ByVal fecha_expedicion As String, ByVal cuenta As String) As DataTable

        'Declaro
        Dim tabla_consulta As New DataTable
        Dim tabla_consulta2 As New DataTable

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd)

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand

            memComando.CommandText = "SELECT TOP(30) fecha_expedicion " &
                   "FROM cabecera_impuestos " &
                   "WHERE cuenta='" & cuenta & "' " &
                   "ORDER BY fecha_expedicion DESC;"
            memComando.Connection = memConn

            'Creamos un adaptador de datos
            Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

            'Llenamos de datos
            adapter.Fill(tabla_consulta2)

            If tabla_consulta2.Rows.Count > 0 Then

                'Declaro
                Dim fecha_final As Date = tabla_consulta2.Rows(0)("fecha_expedicion")
                Dim fecha_inicial As Date = tabla_consulta2.Rows(tabla_consulta2.Rows.Count - 1)("fecha_expedicion")

                Dim query As String = "SELECT clave,porcentaje,cuenta_cuota,cuenta_base " &
                "FROM cabecera_impuestos, detalles_impuestos " &
                "WHERE cabecera_impuestos.fecha_expedicion BETWEEN '" & fecha_inicial & "' AND '" & fecha_final & "' " &
                "AND cabecera_impuestos.Id=detalles_impuestos.id_cabecera_impuestos " &
                "AND cabecera_impuestos.cuenta='" & cuenta & "' " &
                "AND cabecera_impuestos.total_factura>0 " &
                "GROUP BY clave,porcentaje,cuenta_cuota ,cuenta_base " &
                "ORDER BY porcentaje DESC,cuenta_cuota, cuenta_base;"

                memComando.CommandText = query
                memComando.Connection = memConn

                'Llenamos de datos
                adapter.Fill(tabla_consulta)

            End If

            'Cerramos
            adapter.Dispose()
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Variable
        Dim tabla_final As New DataTable
        tabla_final.Columns.Add("clave")
        tabla_final.Columns.Add("porcentaje")
        tabla_final.Columns.Add("cuenta_cuota")
        tabla_final.Columns.Add("cuenta_base")
        Dim porcentaje_anterior As String = Nothing

        'Recorro
        For x = 0 To tabla_consulta.Rows.Count - 1

            If porcentaje_anterior <> tabla_consulta.Rows(x).Item("porcentaje").ToString Then
                Dim Renglon As DataRow = tabla_final.NewRow()
                Renglon("clave") = tabla_consulta.Rows(x).Item("clave").ToString
                Renglon("porcentaje") = tabla_consulta.Rows(x).Item("porcentaje")
                Renglon("cuenta_cuota") = 0  'tabla.Rows(x).Item(2)
                Renglon("cuenta_base") = 0 'tabla.Rows(x).Item(3)
                tabla_final.Rows.Add(Renglon)
            End If

            'Asigno
            porcentaje_anterior = tabla_consulta.Rows(x).Item("porcentaje").ToString
        Next

        'Variables
        Dim expression As String = Nothing
        Dim foundRows() As DataRow
        Dim valor_cuota As String = Nothing
        Dim cuenta_cuota_anterior As String = Nothing
        Dim valor_base As String = Nothing
        Dim cuenta_base_anterior As String = Nothing

        For x = 0 To tabla_final.Rows.Count - 1

            'Asigno
            expression = "porcentaje = '" & tabla_final.Rows(x)("porcentaje") & "'"

            'Almaceno todos los rows obtenidos
            foundRows = tabla_consulta.Select(expression)

            'Recorro
            For i = 0 To foundRows.GetUpperBound(0)
                If cuenta_cuota_anterior <> foundRows(i)(2) Then
                    valor_cuota += foundRows(i)(2) & "#"
                End If
                If cuenta_base_anterior <> foundRows(i)(3) Then
                    valor_base += foundRows(i)(3) & "#"
                End If

                'Asigno
                cuenta_cuota_anterior = foundRows(i)(2)
                cuenta_base_anterior = foundRows(i)(3)
            Next

            'Asigno
            tabla_final.Rows(x)("cuenta_cuota") = Mid(valor_cuota, 1, valor_cuota.Count - 1)
            tabla_final.Rows(x)("cuenta_base") = Mid(valor_base, 1, valor_base.Count - 1)

            'Limpio
            cuenta_cuota_anterior = Nothing
            cuenta_base_anterior = Nothing
            valor_cuota = Nothing
            valor_base = Nothing

        Next

        'Devuelvo
        Return tabla_final

    End Function

    Function GetEncoder(ByVal format As ImageFormat) As ImageCodecInfo

        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageDecoders()

        Dim codec As ImageCodecInfo
        For Each codec In codecs
            If codec.FormatID = format.Guid Then
                Return codec
            End If
        Next codec
        Return Nothing

    End Function

End Class


