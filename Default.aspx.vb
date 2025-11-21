Imports System.Web.Services
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Data

Module Module_default

    Public funcion_default As New funciones_globales

End Module

Partial Class _Default
    Inherits System.Web.UI.Page

    '---------------------------------------------------------ASPX-----------------------------------------------------------------------
    Sub cargar_parametros_iniciales()

        'Obtengo las key por url
        Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
        querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, Request.QueryString("key").ToString)

        'Obtengo datos de empresa
        HttpContext.Current.Session("f_" & querystringSeguro("id_empresa") & "_tabla_empresa") = funcion_default.obtener_datos("Select * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & querystringSeguro("id_empresa") & ";")

        'Guardo en local
        txt_id_empresa.Text = querystringSeguro("id_empresa")

        'Obtengo datos de usuario
        HttpContext.Current.Session("f_" & querystringSeguro("id_usuario") & "_tabla_usuario") = funcion_default.obtener_datos("Select * FROM [kernel_facturacion].[dbo].usuarios WHERE usuarios.id=" & querystringSeguro("id_usuario") & ";")

        'Guardo en local
        txt_id_usuario.Text = querystringSeguro("id_usuario")

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & txt_id_usuario.Text & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & txt_id_empresa.Text & "_tabla_empresa")

        'Obtengo tabla_clientes
        HttpContext.Current.Session("f_" & querystringSeguro("id_empresa") & "_tabla_clientes") = funcion_default.obtener_datos("Select clientes.*, tipo_vias.nombre As nombre_via,localidad.nombre As nombre_provincia,provincia.nombre As nombre_provincia FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].clientes LEFT JOIN [kernel_facturacion].[dbo].[tipo_vias] On clientes.tipo_via=tipo_vias.id LEFT JOIN [kernel_facturacion].[dbo].[localidad] On (clientes .localidad=localidad.id_localidad And clientes .provincia=localidad.id_provincia) LEFT JOIN [kernel_facturacion].[dbo].[provincia] On clientes .provincia = provincia.id;")

        'Obtengo tabla_proveedores
        HttpContext.Current.Session("f_" & querystringSeguro("id_empresa") & "_tabla_proveedores") = funcion_default.obtener_datos("Select * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].proveedores;")

        'Obtengo tabla_articulos
        HttpContext.Current.Session("f_" & querystringSeguro("id_empresa") & "_tabla_articulos") = funcion_default.obtener_datos("Select * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].articulos;")

        'Obtengo tabla_tipo_impuestos
        HttpContext.Current.Session("f_" & querystringSeguro("id_empresa") & "_tabla_tipo_impuestos") = funcion_default.obtener_datos("Select * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].tipo_impuestos ORDER BY impuesto_defecto DESC,porcentaje ASC;")

        'Grabo el inicio de sesion
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "DELETE sessiones_activas WHERE usuario=" & tabla_usuario.Rows(0)("Id") & " And key_usuario='" & HttpContext.Current.Session("id_control") & "';"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Grabo el control para saber que usuario estan activos
            Dim navegador As String = Nothing
            Dim vector() As String = HttpContext.Current.Request.UserAgent.Split(" ")
            For x = 0 To vector.Count - 1
                If vector(x).IndexOf("Chrome") <> -1 Then
                    navegador = vector(x)
                End If
                If vector(x).IndexOf("Firefox") <> -1 Then
                    navegador = vector(x)
                End If
                If vector(x).IndexOf("Edg") <> -1 Then
                    navegador = vector(x)
                End If
                If vector(x).IndexOf("OPR") <> -1 Then
                    navegador = vector(x)
                End If
            Next

            memComando.CommandText = "INSERT INTO sessiones_activas " &
            "(empresa,usuario,nombre_personal,primer_apellido,fecha,hora,ip,key_usuario,nombre_empresa,navegador) VALUES " &
            "(" & tabla_empresa.Rows(0)("Id") & "," & tabla_usuario.Rows(0)("Id") & ", '" & tabla_usuario.Rows(0)("nombre") & "','" & tabla_usuario.Rows(0)("primer_apellido") & "', '" & DateTime.Now.ToString("dd/MM/yyyy") & "', '" & DateTime.Now.ToString("HH:mm:ss") & "','" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString & "', '" & HttpContext.Current.Session("id_control") & "','" & tabla_empresa.Rows(0)("nombre_fiscal") & "','" & HttpContext.Current.Request.Browser.Platform & ": " & navegador & ".');"
            memComando.ExecuteNonQuery() '

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Paso la traza para grabar el Log
        funcion_default.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Login", "El Usuario Accedió a la aplicacion desde la IP: " & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString)

        'Declaro
        Dim tabla_consulta As New DataTable

        'Obtener lo que ocupa en Espacio la BBDD y el numero de apuntes
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandType = CommandType.StoredProcedure
            memComando.CommandText = "sp_spaceused"
            memComando.Connection = memConn

            'Creamos un adaptador de datos
            Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

            'Llenamos de datos
            adapter.Fill(tabla_consulta)

            'Liberamos recursos
            adapter.Dispose()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno valores
        Dim datos As Decimal = tabla_consulta(0)(1).ToString.Replace(" MB", "").Replace(".", ",")
        Dim log As Decimal = tabla_consulta(0)(2).ToString.Replace(" MB", "").Replace(".", ",")
        Dim backup As Decimal = 0
        Dim gestion As Decimal = 0
        Dim almacen As Decimal = 0
        Dim total As Decimal = 0

        'Ubico donde esta la carpeta del BackupCloud
        Dim peso_backup As Decimal = 0
        Dim ruta_backupcloud As String = "D:\bak_sql_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\"
        If System.IO.Directory.Exists(ruta_backupcloud) = True Then
            Dim fileNames = My.Computer.FileSystem.GetFiles(ruta_backupcloud, FileIO.SearchOption.SearchAllSubDirectories)
            For Each fileName As String In fileNames
                'Propiedades de los ficheros
                Dim propiedades_fichero As New FileInfo(fileName)
                'Sumatorio de todos los archivos de Backup
                peso_backup += propiedades_fichero.Length
            Next
            backup = Math.Round((peso_backup * 0.000954) / 1000, 2)
        End If

        'Ubico donde esta la carpeta del Gestion Documental
        Dim peso_gestion As Decimal = 0
        Dim ruta_gestion As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\ficheros\"
        If System.IO.Directory.Exists(ruta_gestion) = True Then
            Dim fileNames_gestion = My.Computer.FileSystem.GetFiles(ruta_gestion, FileIO.SearchOption.SearchAllSubDirectories)
            For Each fileName As String In fileNames_gestion
                'Propiedades de los ficheros
                Dim propiedades_fichero As New FileInfo(fileName)
                'Sumatorio de todos los archivos de Backup
                peso_gestion += propiedades_fichero.Length
            Next
            gestion = Math.Round((peso_gestion * 0.000954) / 1000, 2)
        End If

        'Ubico donde esta la carpeta del Gestion Documental
        Dim peso_io_almacen As Decimal = 0
        Dim ruta_io_almacen As String = "D:\imagenes_usuarios_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\drive\"
        If System.IO.Directory.Exists(ruta_io_almacen) = True Then
            Dim fileNames_gestion = My.Computer.FileSystem.GetFiles(ruta_io_almacen, FileIO.SearchOption.SearchAllSubDirectories)
            For Each fileName As String In fileNames_gestion
                'Propiedades de los ficheros
                Dim propiedades_fichero As New FileInfo(fileName)
                'Sumatorio de todos los archivos de Backup
                peso_io_almacen += propiedades_fichero.Length
            Next
            almacen = Math.Round((peso_io_almacen * 0.000954) / 1000, 2)
        End If

        'Asigno valores
        total = datos + backup + gestion + almacen

        'Actualizo en el global
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "UPDATE empresa SET bbdd_mb=" & datos.ToString.Replace(",", ".") & ",log_mb=" & log.ToString.Replace(",", ".") & ",n_apuntes=0,backup_mb=" & backup.ToString.Replace(",", ".") & ",gestion_documental_mb=" & gestion.ToString.Replace(",", ".") & ",io_almacenamiento_mb=" & almacen.ToString.Replace(",", ".") & " WHERE ruta_base_datos='" & tabla_empresa.Rows(0)("ruta_base_datos") & "';"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Asigno
        lbl_datos.Text = datos & " MB"
        lbl_backup.Text = backup & " MB"
        lbl_gestion_documental.Text = gestion & " MB"
        lbl_almacen.Text = almacen & " MB"
        lbl_total.Text = total & " MB"

        'Declaro
        Dim porcentaje As Decimal = Math.Round(total * 100 / 4000)

        'Asigno
        If total < 3600 Then

            'Asigno
            LT_barra_ocupacion.Text = "<div class='progress bg-white' role='progressbar' aria-valuenow='" & porcentaje & "' aria-valuemin='0' aria-valuemax='100'> " &
                        "<div class='progress-bar progress-bar-striped progress-bar-animated bg-primary' style='width: " & porcentaje & "%'>" & total & " MB / 4000 MB </div>" &
                        "</div>"

            LT_barra_ocupacion_barra.Text = "<div class='progress bg-white' role='progressbar' aria-valuenow='" & porcentaje & "' aria-valuemin='0' aria-valuemax='100' style='height: 2px' title='Ocupacion: " & total & " MB / 4000 MB'> " &
                        "<div id='progreso_barra' class='progress-bar progress-bar-striped bg-primary' onmouseover=hand('progreso_barra') style='width: " & porcentaje & "%;'></div>" &
                        "</div>"

        Else

            'Asigno
            LT_barra_ocupacion.Text = "<div class='progress bg-white' role='progressbar' aria-valuenow='" & porcentaje & "' aria-valuemin='0' aria-valuemax='100'> " &
                        "<div class='progress-bar progress-bar-striped progress-bar-animated bg-danger' style='width: " & porcentaje & "%'>" & total & " MB / 4000 MB </div>" &
                        "</div>"

            LT_barra_ocupacion_barra.Text = "<div class='progress bg-white' role='progressbar' aria-valuenow='" & porcentaje & "' aria-valuemin='0' aria-valuemax='100' style='height: 2px' title='Ocupacion: " & total & " MB / 4000 MB'> " &
                        "<div id='progreso_barra' class='progress-bar progress-bar-striped bg-danger' onmouseover=hand('progreso_barra') style='width: " & porcentaje & "%;'></div>" &
                        "</div>"

        End If

        'Eliminación de ficheros temporales
        'Ubico donde esta la carpeta Temp para eliminar ficheros
        Dim ruta_temporal As String = HttpContext.Current.Server.MapPath("") & "\temp\"
        If System.IO.Directory.Exists(ruta_temporal) = True Then
            Dim fileNames = My.Computer.FileSystem.GetFiles(ruta_temporal, FileIO.SearchOption.SearchAllSubDirectories, "*_" & tabla_usuario.Rows(0)("Id") & "_*.*")
            For Each nombres As String In fileNames
                'Propiedades de los ficheros
                Dim propiedades_fichero As New FileInfo(nombres)
                'Borra el fichero
                System.IO.File.Delete(propiedades_fichero.FullName)
            Next
        End If

    End Sub

    Protected Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

        'Control de Seguridad
        If Session("id_control") = "" Then
            Response.Redirect("login.aspx")
            Exit Sub
        Else

            'Creo Sessiones específicas para esta empresa
            cargar_parametros_iniciales()

        End If

        'Cargo los datos a global
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & txt_id_usuario.Text & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & txt_id_empresa.Text & "_tabla_empresa")

        Try

            If Not IsPostBack Then

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Asigno imagen de fondo
                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/fondo/" & tabla_usuario.Rows(0)("Id") & "/fondo.jpg") = True Then
                    body_contenedor.Attributes.Add("style", "background: url('imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/fondo/" & tabla_usuario.Rows(0)("Id") & "/fondo.jpg?" + numero_aleatorio.Next(1, 13).ToString() & "') no-repeat center center;")
                Else
                    body_contenedor.Attributes.Add("style", "background: url('imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg') no-repeat center center fixed;")
                End If

                'Escribo la fecha
                lbl_fecha.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd, dd")) & " de " & CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("MMMM"))

                'Escribo la Key
                lblkey.Text = Session("id_control")

                'Foto de Usuario
                If My.Computer.FileSystem.FileExists("D:/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.jpg") = True Then

                    'Asigno foto
                    Lt_foto.Text = "<img id='img_logo' src='imagenes_usuarios_facturacion/imagenes_usuarios_facturacion/" & tabla_empresa.Rows(0)("ruta_base_datos") & "/foto/" & tabla_usuario.Rows(0)("Id") & "/foto.jpg" & "?" + numero_aleatorio.Next(1, 13).ToString & "' style='width:55px; height:55px;border-radius: 50%;' class='img-thumbnail' />"

                Else

                    'Asigno
                    Lt_foto.Text = "<span id='img_logo' class='bi bi-person-bounding-box' style='font-size:25px;'></span>"

                End If

            End If

        Catch ex As Exception
            'Registro Error
            funcion_default.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "form1_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error LOAD: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub


    '---------------------------------------------------------JSON-----------------------------------------------------------------------
    <WebMethod(EnableSession:=True)>
    Public Shared Function leer_sesion(ByVal nombre_sesion As String) As String

        'Variables
        Dim resultado As String = "vacio"

        'Cargo los valores de la sesión
        Dim tabla_consulta As DataTable = HttpContext.Current.Session(nombre_sesion)

        'Recorro
        For x = 0 To tabla_consulta.Rows.Count - 1

            For y = 0 To tabla_consulta.Columns.Count - 1

                If y = 0 Then
                    resultado = tabla_consulta(x)(y).ToString
                Else
                    resultado = resultado & "|" & tabla_consulta(x)(y).ToString
                End If

            Next

        Next

        'Devuelvo
        Return resultado

    End Function

    <WebMethod()>
    Public Shared Function Json_sonido(ByVal id_usuario As Integer, ByVal valores As String) As String

        'Declaro
        Dim valor() As String = valores.Split("|")

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "UPDATE usuarios SET " &
                "volumen_habla=" & (valor(0) / 100).ToString.Replace(",", ".") & ", " &
                "tono_habla=" & (valor(1) / 10).ToString.Replace(",", ".") & ", " &
                "velocidad_habla=" & (valor(2) / 10).ToString.Replace(",", ".") & " " &
                "WHERE id=" & id_usuario & ";"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Obtengo datos de usuario
        HttpContext.Current.Session("f_" & id_usuario & "_p_usuario") = funcion_default.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].usuarios WHERE usuarios.id=" & id_usuario & ";")

        'Devuelvo
        Return "OK"

    End Function

    <WebMethod>
    Public Shared Function numero_notificacion(ByVal bbdd As String, ByVal id_usuario As String) As String

        'Variables
        Dim contador_noticias As Integer = 0
        Dim contador_noticias_urgente As Integer = 0

        'Obtengo
        Dim tabla_consulta As DataTable = funcion_default.obtener_datos("SELECT Id,prioridad FROM [" & bbdd & "].[dbo].notificaciones WHERE activa=1 AND usuario = '" & id_usuario & "';")

        'asigno
        contador_noticias = tabla_consulta.Rows.Count

        'Recorro
        For x = 0 To tabla_consulta.Rows.Count - 1

            'Saber si hay noticias urgentes
            If tabla_consulta.Rows(x)("prioridad").ToString = "Normal" Then
                contador_noticias_urgente += 1
            End If

        Next

        'Obtengo
        Dim tabla_key As DataTable = funcion_default.obtener_datos("SELECT TOP(1) key_usuario FROM [kernel_facturacion].[dbo].sessiones_activas WHERE usuario=" & id_usuario & " ORDER BY id DESC;")

        'Variables
        Dim key As String = tabla_key.Rows(0)("key_usuario")

        'Devuelvo
        Return contador_noticias & "|" & key & "|" & contador_noticias_urgente

    End Function

    <WebMethod>
    Public Shared Function cerrar_sesion(ByVal id_usuario As Integer, ByVal id_empresa As Integer) As String

        Try

            'Cierro la session Correctamente
            Return funcion_default.cerrar_sesion(id_usuario, id_empresa)

        Catch ex As Exception

            Return "Error: cerrar_session"

        End Try

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_cliente(ByVal prefix As String, ByVal id_empresa As String) As String()

        Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_clientes")
        Dim customers As New List(Of String)()

        'Hago Select
        Dim expression As String = Nothing
        Dim sortOrder As String = "id ASC"

        'Compruebo si es numero o no
        If IsNumeric(prefix) = True Then
            expression = "Id = " & prefix & ""
        Else

            'Encabezado
            Select Case Mid(prefix, 1, 1)
                Case "*"
                    expression = "nif LIKE '%" & Mid(prefix, 2) & "%'"
                    sortOrder = "nif ASC"

                Case Else
                    expression = "nombre_fiscal LIKE '%" & prefix & "%' OR nombre_comercial LIKE '%" & prefix & "%'"
                    sortOrder = "nombre_fiscal,nombre_comercial ASC"
            End Select

        End If

        Dim foundRows() As DataRow

        ' Almaceno todos los rows obtenidos
        foundRows = tabla_clientes.Select(expression, sortOrder)

        Dim i As Integer
        Dim valor As String = Nothing

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            'Limitacion para sólo 100 cuentas
            If i = 100 Then
                Exit For
            End If

            customers.Add(foundRows(i)(0) & "|" & foundRows(i)(4) & "|" & foundRows(i)(6))

        Next i

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_proveedor(ByVal prefix As String, ByVal id_empresa As String) As String()

        Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_proveedores")
        Dim customers As New List(Of String)()

        'Hago Select
        Dim expression As String = Nothing
        Dim sortOrder As String = "id ASC"

        'Compruebo si es numero o no
        If IsNumeric(prefix) = True Then
            expression = "Id = " & prefix & ""
        Else

            'Encabezado
            Select Case Mid(prefix, 1, 1)
                Case "*"
                    expression = "nif LIKE '%" & Mid(prefix, 2) & "%'"
                    sortOrder = "nif ASC"

                Case Else
                    expression = "nombre_fiscal LIKE '%" & prefix & "%' OR nombre_comercial LIKE '%" & prefix & "%'"
                    sortOrder = "nombre_fiscal,nombre_comercial ASC"
            End Select

        End If

        Dim foundRows() As DataRow

        ' Almaceno todos los rows obtenidos
        foundRows = tabla_clientes.Select(expression, sortOrder)

        Dim i As Integer
        Dim valor As String = Nothing

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            'Limitacion para sólo 100 cuentas
            If i = 100 Then
                Exit For
            End If

            customers.Add(foundRows(i)(0) & "|" & foundRows(i)(4) & "|" & foundRows(i)(6))

        Next i

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_articulos(ByVal prefix As String, ByVal id_empresa As String) As String()

        Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_articulos")
        Dim customers As New List(Of String)()

        'Hago Select
        Dim expression As String = Nothing
        Dim sortOrder As String = "id ASC"

        'Compruebo si es numero o no
        If IsNumeric(prefix) = True Then

            expression = "Id = " & prefix & ""

        Else

            expression = "denominacion LIKE '%" & prefix & "%'"
            sortOrder = "denominacion,id ASC"

        End If

        Dim foundRows() As DataRow

        ' Almaceno todos los rows obtenidos
        foundRows = tabla_articulos.Select(expression, sortOrder)

        Dim i As Integer
        Dim valor As String = Nothing

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            'Limitacion para sólo 100 cuentas
            If i = 50 Then
                Exit For
            End If

            customers.Add(foundRows(i)(0) & "|" & foundRows(i)(3))

        Next i

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_cod_cliente(ByVal prefix As String, ByVal id_empresa As String) As String()

        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_empresa")
        Dim tabla_clientes As DataTable = funcion_default.obtener_datos("SELECT TOP(25) * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].[clientes] WHERE id LIKE '" & prefix & "%' ORDER BY id;")
        Dim customers As New List(Of String)()

        'Recorro
        For x = 0 To tabla_clientes.Rows.Count - 1

            'Asigno
            customers.Add(tabla_clientes.Rows(x)("id") & "|" & tabla_clientes.Rows(x)("nombre_fiscal") & "|" & tabla_clientes.Rows(x)("nif"))

        Next

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_denominacion_cliente(ByVal prefix As String, ByVal id_empresa As String) As String()

        Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_clientes")
        Dim customers As New List(Of String)()

        'Hago Select
        Dim expression As String = Nothing
        Dim sortOrder As String = "id ASC"

        'Encabezado
        Select Case Mid(prefix, 1, 1)
            Case "*"
                expression = "nif Like '%" & Mid(prefix, 2) & "%'"
                sortOrder = "nif ASC"

            Case Else
                expression = "nombre_fiscal LIKE '%" & prefix & "%' OR nombre_comercial LIKE '%" & prefix & "%'"
                sortOrder = "nombre_fiscal,nombre_comercial ASC"
        End Select

        'Declaro
        Dim foundRows() As DataRow

        ' Almaceno todos los rows obtenidos
        foundRows = tabla_clientes.Select(expression, sortOrder)

        Dim i As Integer
        Dim valor As String = Nothing

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            'Limitacion para sólo 100 cuentas
            If i = 50 Then
                Exit For
            End If

            customers.Add(foundRows(i)(0) & "|" & foundRows(i)(4) & "|" & foundRows(i)(6))

        Next i

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_cod_articulo(ByVal prefix As String, ByVal id_empresa As String) As String()

        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_empresa")
        Dim tabla_articulos As DataTable = funcion_default.obtener_datos("SELECT TOP(25) * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].[articulos] WHERE id LIKE '" & prefix & "%' ORDER BY id;")
        Dim customers As New List(Of String)()

        'Recorro
        For x = 0 To tabla_articulos.Rows.Count - 1

            'Asigno
            customers.Add(tabla_articulos.Rows(x)("id") & "|" & tabla_articulos.Rows(x)("denominacion"))

        Next

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_denominacion_articulo(ByVal prefix As String, ByVal id_empresa As String) As String()

        Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_articulos")
        Dim customers As New List(Of String)()

        'Hago Select
        Dim expression As String = Nothing
        Dim sortOrder As String = "id ASC"

        'Encabezado
        expression = "denominacion LIKE '%" & prefix & "%'"
        sortOrder = "denominacion,id ASC"

        'Declaro
        Dim foundRows() As DataRow

        ' Almaceno todos los rows obtenidos
        foundRows = tabla_articulos.Select(expression, sortOrder)

        Dim i As Integer
        Dim valor As String = Nothing

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            'Limitacion para sólo 100 cuentas
            If i = 50 Then
                Exit For
            End If

            customers.Add(foundRows(i)(0) & "|" & foundRows(i)(3))

        Next i

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function autocompleta_cargo(ByVal prefix As String, ByVal id_empresa As String) As String()

        'Obtengo
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_empresa")
        Dim tabla_cargo As DataTable = funcion_default.obtener_datos("SELECT cargo FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].[contactos] WHERE cargo LIKE '" & prefix & "%' GROUP BY cargo;")
        Dim customers As New List(Of String)()

        'Recorro
        For x = 0 To tabla_cargo.Rows.Count - 1

            'Asigno
            customers.Add(tabla_cargo.Rows(x)("cargo"))

        Next

        'Devuelvo resultado
        Return customers.ToArray()

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function Obtener_articulo(ByVal codigo As String, ByVal id_empresa As Integer) As String

        'Obtengo
        Dim tabla_articulos As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_articulos")

        'Regreso Datos
        Dim valor As String = funcion_default.buscar_datos_tabla(tabla_articulos, "id", codigo, "todos")

        'Devulevo
        Return valor

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function Obtener_cliente(ByVal codigo As String, ByVal id_empresa As Integer) As String

        'Obtengo
        Dim tabla_clientes As DataTable = HttpContext.Current.Session("f_" & id_empresa & "_tabla_clientes")

        'Regreso Datos
        Dim valor As String = funcion_default.buscar_datos_tabla(tabla_clientes, "id", codigo, "todos")

        'Devulevo
        Return valor

    End Function

    <WebMethod()>
    Public Shared Function Json_certificado(ByVal opcion As String, ByVal valores As String) As String

        If opcion = "actualizar" Then

            'Asigno
            Dim valor() As String = valores.Split("|")

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "UPDATE empresa SET " &
                        "certificado_posicion_x=" & valor(0) & ", " &
                        "certificado_posicion_y=" & valor(1) & " " &
                        "WHERE id=" & valor(2) & ";"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Obtengo datos de empresa
            HttpContext.Current.Session("f_" & valor(2) & "_tabla_empresa") = funcion_default.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & valor(2) & ";")

            Return "OK"

        End If

        Return "Error"

    End Function

End Class