Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Http.Cors
Imports System.Web.Http

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="https://www.iocloudcomputing.io/")>
<EnableCors("*", "*", "*")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WS_IOContabilidad
    Inherits System.Web.Services.WebService

    Dim funciones_globales As New funciones_globales

    <WebMethod()>
    Public Function Test_Conexion(ByVal KeyCode As String) As String

        'Declaro
        Dim bbdd As String = Nothing

        Try
            'Descompongo el Token
            Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
            querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, KeyCode)

            'Compruebo si la empresa permite api
            bbdd = querystringSeguro("bbdd").ToString

            'devuelvo
            Return "Se ha conectado correctamente con la bbdd: " & bbdd

        Catch ex As Exception
            Return ex.Message & "Ha ocurrido un Error"
        End Try

    End Function

    Public Function GetJson(ByVal dt As DataTable) As String

        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName, dr(col))
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)

    End Function

    <WebMethod()>
    Public Function API_Asientos(ByVal KeyCode As String, ByVal datos As String) As String

        'Declaro
        Dim bbdd As String = Nothing
        Dim id_usuario As Integer = Nothing
        Dim nombre_usuario As String = Nothing
        Dim primer_apellido As String = Nothing
        Dim segundo_apellido As String = Nothing
        Dim nivel As String = Nothing
        Dim id_tipo_plan_cuentas As Integer = 0
        Dim fecha As Date = Nothing
        Dim denominacion_cuenta As String = ""
        Dim nif_cuenta As String = ""
        Dim suma_debe As Decimal = 0
        Dim suma_haber As Decimal = 0
        Dim id_cabecera_asiento As String = Nothing
        Dim referencia_asiento As String = Nothing

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Descompongo el Token
            Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
            querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, KeyCode)

            'Compruebo si la empresa permite api
            bbdd = querystringSeguro("bbdd").ToString
            id_usuario = querystringSeguro("usuario").ToString
            nombre_usuario = querystringSeguro("nombre").ToString
            primer_apellido = querystringSeguro("1_apellido").ToString
            segundo_apellido = querystringSeguro("2_apellido").ToString
            nivel = querystringSeguro("nivel").ToString
            id_tipo_plan_cuentas = querystringSeguro("id_tipo_plan_cuentas").ToString

            'Excepciones
            'Comprobacion API
            Dim tabla As DataTable = funciones_globales.obtener("inforplan", "SELECT api FROM empresa WHERE ruta_base_datos='" & bbdd & "';")
            If tabla.Rows(0)(0) = False Then
                Return "Esta empresa no tiene activada las funciones de API."
                Exit Function
            End If

            'Comprobar Nivel
            tabla = funciones_globales.obtener("inforplan", "SELECT nivel FROM usuarios WHERE Id=" & id_usuario & ";")
            nivel = tabla.Rows(0)(0).ToString

            'Descompongo
            Dim vector_lineas() As String = datos.Split("&")

            'Recorro
            For x = 0 To vector_lineas.Count - 1

                'Descompongo
                Dim vector_elementos() As String = vector_lineas(x).Split("|")

                'Cabecera
                If x = 0 Then

                    'Comprobar si existe el tipo de asiento
                    Dim tabla_tipo_asientos As DataTable = funciones_globales.obtener(bbdd, "SELECT * FROM tipo_asientos WHERE Id=" & vector_elementos(1) & ";")
                    If tabla_tipo_asientos.Rows.Count = 0 Then
                        Return "El Tipo de Asiento no existe"
                    End If

                    'Compruebo restricción de Fechas
                    fecha = vector_elementos(0)

                    'Comprobar rango permitido de asientos
                    tabla = funciones_globales.obtener("inforplan", "SELECT fecha_inicial_datos,fecha_final_datos FROM empresa WHERE ruta_base_datos='" & bbdd & "';")

                    'Asigno
                    Dim fecha_inicial As Date = CDate(tabla.Rows(0)(0))
                    Dim fecha_final As Date = CDate(tabla.Rows(0)(1))
                    Dim fecha_evaluar As Date = CDate(vector_elementos(0))

                    If fecha_evaluar > fecha_final Or fecha_evaluar < fecha_inicial Then
                        Return "Los rangos permitidos para la entrada de asientos van desde: " & fecha_inicial & " al " & fecha_final
                    End If

                End If

                If x > 0 Then

                    'Comprobar que la cuenta existe
                    tabla = funciones_globales.obtener(bbdd, "SELECT denominacion,nif FROM plan_cuenta_personalizadas WHERE cuenta='" & vector_elementos(1) & "';")
                    If tabla.Rows.Count = 0 Then
                        Return "La cuenta " & vector_elementos(1) & " no existe"
                    Else
                        denominacion_cuenta = tabla.Rows(0)(0).ToString
                        nif_cuenta = tabla.Rows(0)(1).ToString
                    End If

                    'El concepto no puede estar vacio
                    If Trim(vector_elementos(4)) = "" Then
                        Return "El Concepto no puede estar vacío"
                    End If

                    'Compruebo cantidades debe y haber
                    Dim debe As Decimal = CDec(vector_elementos(5))
                    Dim haber As Decimal = CDec(vector_elementos(6))

                    If debe = 0 And haber = 0 Then
                        Return "Debe y Haber no pueden ser ambos 0"
                    End If
                    If debe <> 0 And haber <> 0 Then
                        Return "Debe y Haber no pueden tener ambos cantidades"
                    End If

                    'Asigno
                    suma_debe += debe
                    suma_haber += haber

                End If

            Next

            'Que la suma del total debe y haber sean iguales
            If suma_debe <> suma_haber Then
                Return "La suma total de los debe y de los haber son diferentes"
            End If

            'Si todo es OK grabo cabecera y detalles
            'Recorro
            For x = 0 To vector_lineas.Count - 1

                'Descompongo
                Dim vector_elementos() As String = vector_lineas(x).Split("|")

                'Cabecera
                If x = 0 Then

                    'Obtengo un nuevo numero de asiento para el metodo elegido para la empresa
                    referencia_asiento = funciones_globales.asiento_tinfor(bbdd, vector_elementos(0), vector_elementos(1))

                    'Grabo la Cabecera de asiento
                    id_cabecera_asiento = funciones_globales.grabar_cabecera_asiento_(bbdd, vector_elementos(0), vector_elementos(1), "Asiento", referencia_asiento)

                End If

                If x > 0 Then

                    'Asigno
                    Dim id_detalle_asiento As Integer = 0

                    'Conecto
                    Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd)

                        'Abrimos conexion
                        memConn.Open()

                        'Ataco a la conexion del programa
                        Dim memComando As New SqlCommand

                        'Grabo la cabecera
                        memComando.CommandText = "Usp_detalles_asientos"
                        memComando.Connection = memConn
                        memComando.CommandType = CommandType.StoredProcedure

                        'Parametros de entrada
                        memComando.Parameters.Add("@Id_cabecera_asiento", SqlDbType.Int, ParameterDirection.Input).Value = id_cabecera_asiento
                        memComando.Parameters.Add("@id_apunte_asiento", SqlDbType.Int, ParameterDirection.Input).Value = 0
                        memComando.Parameters.Add("@referencia", SqlDbType.VarChar, 20, ParameterDirection.Input).Value = Trim(vector_elementos(0))
                        memComando.Parameters.Add("@cuenta", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = vector_elementos(1)
                        'Comprobar que la cuenta existe
                        tabla = funciones_globales.obtener(bbdd, "SELECT denominacion,nif FROM plan_cuenta_personalizadas WHERE cuenta='" & vector_elementos(1) & "';")
                        denominacion_cuenta = tabla.Rows(0)(0).ToString
                        nif_cuenta = tabla.Rows(0)(1).ToString
                        memComando.Parameters.Add("@denominacion_cuenta", SqlDbType.VarChar, 200, ParameterDirection.Input).Value = denominacion_cuenta
                        memComando.Parameters.Add("@nif_cuenta", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = nif_cuenta
                        memComando.Parameters.Add("@serie", SqlDbType.VarChar, 6, ParameterDirection.Input).Value = Trim(vector_elementos(2))
                        memComando.Parameters.Add("@factura", SqlDbType.VarChar, 20, ParameterDirection.Input).Value = Trim(vector_elementos(3))
                        memComando.Parameters.Add("@id_codigo_concepto", SqlDbType.Int, ParameterDirection.Input).Value = 0
                        memComando.Parameters.Add("@concepto", SqlDbType.VarChar, 200, ParameterDirection.Input).Value = Trim(vector_elementos(4))
                        memComando.Parameters.Add("@debe", SqlDbType.Decimal, ParameterDirection.Input).Value = vector_elementos(5).Replace(".", ",")
                        memComando.Parameters.Add("@haber", SqlDbType.Decimal, ParameterDirection.Input).Value = vector_elementos(6).Replace(".", ",")
                        memComando.Parameters.Add("@id_tipo_plan_cuentas", SqlDbType.Int, ParameterDirection.Input).Value = id_tipo_plan_cuentas
                        memComando.Parameters.Add("@id_cobrador", SqlDbType.Int, ParameterDirection.Input).Value = 0
                        memComando.Parameters.Add("@id_banco_empresa", SqlDbType.Int, ParameterDirection.Input).Value = 0
                        memComando.Parameters.Add("@id_banco_cliente", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = "0"
                        memComando.Parameters.Add("@id_forma_pago", SqlDbType.Int, ParameterDirection.Input).Value = 0
                        memComando.Parameters.Add("@origen", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = "Asientos"
                        memComando.Parameters.Add("@id_cabecera_impuesto", SqlDbType.VarChar, 50, ParameterDirection.Input).Value = "0"
                        memComando.Parameters.Add("@id_cod_usuario", SqlDbType.Int, ParameterDirection.Input).Value = id_usuario
                        memComando.Parameters.Add("@nivel_cod_usuario", SqlDbType.VarChar, 20, ParameterDirection.Input).Value = nivel
                        memComando.Parameters.Add("@id_cabecera_inmovilizado", SqlDbType.VarChar, 8000, ParameterDirection.Input).Value = "0"

                        'Parametros de salida
                        memComando.Parameters.Add("@id_detalle_asiento", SqlDbType.Int)
                        memComando.Parameters("@id_detalle_asiento").Direction = ParameterDirection.Output
                        memComando.ExecuteNonQuery()

                        'Libero recurso
                        memComando.Dispose()
                        SqlConnection.ClearPool(memConn)

                    End Using

                End If

            Next

            'Cierro la base de datos
            tabla.Dispose()

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Grabar Registro
            funciones_globales.grabar_registro(bbdd, id_usuario, nombre_usuario & " " & primer_apellido & " " & segundo_apellido, "WS API_asientos", "Creó el asiento con Nº: " & referencia_asiento & " y fecha: " & fecha & " (Tiempo Grabación: " & tspan.Seconds & "," & tspan.Milliseconds & " Milisegundos).")

            'Devuelvo
            Return "OK-" & referencia_asiento

        Catch ex As Exception
            Return ex.Message & "Ha ocurrido un Error"
        End Try

    End Function

    <WebMethod()>
    Public Function API_Cuentas(ByVal KeyCode As String, ByVal cuenta As String, ByVal denominacion As String, ByVal nif As String) As String

        'Declaro
        Dim bbdd As String = Nothing
        Dim id_usuario As Integer = Nothing
        Dim nombre_usuario As String = Nothing
        Dim primer_apellido As String = Nothing
        Dim segundo_apellido As String = Nothing
        Dim id_tipo_plan_cuentas As Integer = 0
        Dim nivel As String = Nothing
        Dim denominacion_cuenta As String = ""
        Dim nif_cuenta As String = ""

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Descompongo el Token
            Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
            querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, KeyCode)

            'Compruebo si la empresa permite api
            bbdd = querystringSeguro("bbdd").ToString
            id_usuario = querystringSeguro("usuario").ToString
            nombre_usuario = querystringSeguro("nombre").ToString
            primer_apellido = querystringSeguro("1_apellido").ToString
            segundo_apellido = querystringSeguro("2_apellido").ToString
            id_tipo_plan_cuentas = querystringSeguro("id_tipo_plan_cuentas").ToString

            'Excepciones
            Dim tabla As DataTable = funciones_globales.obtener("inforplan", "SELECT api,custodia,digitos_asientos FROM empresa WHERE ruta_base_datos='" & bbdd & "';")
            If tabla.Rows(0)(0) = False Then
                Return "Esta empresa no tiene activada las funciones de API."
                Exit Function
            End If

            If tabla.Rows(0)(1) = True Then
                Return "Esta empresa esta en Custodia y no tiene activada las funciones de API."
                Exit Function
            End If

            Dim numero As Integer = CInt(cuenta.Count)
            Dim numero_dos As Integer = CInt(tabla.Rows(0)(2))
            If numero <> numero_dos Then
                Return "La Cuenta no tiene los digitos suficientes."
                Exit Function
            End If

            'Comprobar que la cuenta no es vacia y no existe
            If cuenta = "" Then Return "La cuenta no puede estar vacía"
            tabla = funciones_globales.obtener(bbdd, "SELECT denominacion,nif FROM plan_cuenta_personalizadas WHERE cuenta='" & cuenta & "';")
            If tabla.Rows.Count > 0 Then
                Return "La cuenta ya existe"
                Exit Function
            End If

            'Comprobar que la denominacion no viene vacía
            If denominacion = "" Then Return "La Denominación no puede estar vacía"

            'Comprobar que si pertenece al 347, necesita NIF
            Dim tabla_pertenece_347 As DataTable = funciones_globales.obtener("inforplan", "SELECT TOP(1) * FROM verificar_cuenta_nif WHERE id_tipo_plan_cuentas=" & id_tipo_plan_cuentas & " AND cuenta='" & Mid(cuenta, 1, 3) & "';")
            If tabla_pertenece_347.Rows.Count = 0 Then
                If nif = "" Then
                    Return "La Cuenta introducida pertenece al 347 y debe introducir un NIF."
                    Exit Function
                End If
            End If

            'Ataco a la conexion del programa
            Dim leer_ultimo_numero_cuenta As Integer = 0

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd)

                'Abrimos conexion
                memConn.Open()

                'Obtengo el ultimo numero de cuenta
                Dim tabla_ultimo_numero_cuenta As DataTable = funciones_globales.obtener(bbdd, "SELECT TOP (1) Id FROM plan_cuenta_personalizadas ORDER BY Id DESC;")
                leer_ultimo_numero_cuenta = tabla_ultimo_numero_cuenta.Rows(0)("Id") + 1

                'Grabo Datos
                Dim memComando As New SqlCommand
                memComando.CommandText = "INSERT INTO [plan_cuenta_personalizadas] (Id,cuenta,denominacion,nif) VALUES (" & leer_ultimo_numero_cuenta & ",@cuenta,@denominacion,@nif);"
                memComando.Parameters.Add("@cuenta", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@cuenta").Value = cuenta
                memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@denominacion").Value = denominacion
                memComando.Parameters.Add("@nif", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@nif").Value = nif
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cierro
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Grabar Registro
            funciones_globales.grabar_registro(bbdd, id_usuario, nombre_usuario & " " & primer_apellido & " " & segundo_apellido, "WS Cuenta", "Creó la cuenta: " & cuenta & " (" & denominacion & ") (Tiempo Grabación:  " & tspan.Seconds & "," & tspan.Milliseconds & " Milisegundos).")

            'Devuelvo
            Return leer_ultimo_numero_cuenta

        Catch ex As Exception
            Return ex.Message & "Ha ocurrido un Error"
        End Try

    End Function

    <WebMethod()>
    Public Function API_Impuestos(ByVal KeyCode As String, ByVal datos As String) As String

        'Declaro
        Dim bbdd As String = Nothing
        Dim id_usuario As Integer = Nothing
        Dim nombre_usuario As String = Nothing
        Dim primer_apellido As String = Nothing
        Dim segundo_apellido As String = Nothing
        Dim nivel As String = Nothing
        Dim id_tipo_plan_cuentas As Integer = 0
        Dim tabla_cuotas_bases As New DataTable
        tabla_cuotas_bases.Columns.Add("Cuenta")
        tabla_cuotas_bases.Columns.Add("Denominacion")
        tabla_cuotas_bases.Columns.Add("NIF")
        tabla_cuotas_bases.Columns.Add("Debe")
        tabla_cuotas_bases.Columns.Add("Haber")
        Dim fecha As Date = Nothing
        Dim denominacion_cuenta As String = ""
        Dim nif_cuenta As String = ""
        Dim suma_cuota As Decimal = 0
        Dim suma_base As Decimal = 0
        Dim id_cabecera_asiento As String = Nothing
        Dim referencia_asiento As String = Nothing

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Descompongo el Token
            Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
            querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, KeyCode)

            'Compruebo si la empresa permite api
            bbdd = querystringSeguro("bbdd").ToString
            id_usuario = querystringSeguro("usuario").ToString
            nombre_usuario = querystringSeguro("nombre").ToString
            primer_apellido = querystringSeguro("1_apellido").ToString
            segundo_apellido = querystringSeguro("2_apellido").ToString
            nivel = querystringSeguro("nivel").ToString
            id_tipo_plan_cuentas = querystringSeguro("id_tipo_plan_cuentas").ToString

            Dim tipo_impuesto As String = Nothing
            Dim referencia As String = Nothing
            Dim serie As String = Nothing
            Dim factura As String = Nothing
            Dim concepto As String = Nothing

            Dim total_factura As Decimal = 0
            Dim identificador As String = Nothing
            Dim cuentas As New List(Of String)

            'Excepciones
            'Comprobacion API
            Dim tabla As DataTable = funciones_globales.obtener("inforplan", "SELECT api FROM empresa WHERE ruta_base_datos='" & bbdd & "';")
            If tabla.Rows(0)(0) = False Then
                Return "Esta empresa no tiene activada las funciones de API."
                Exit Function
            End If

            'Comprobar Nivel
            tabla = funciones_globales.obtener("inforplan", "SELECT nivel FROM usuarios WHERE Id=" & id_usuario & ";")
            nivel = tabla.Rows(0)(0).ToString

            'Descompongo
            Dim vector_lineas() As String = datos.Split("&")

            'Recorro
            For x = 0 To vector_lineas.Count - 1

                'Descompongo
                Dim vector_elementos() As String = vector_lineas(x).Split("|")

                'Cabecera
                If x = 0 Then

                    'Comprobar rango permitido de asientos e impuestos
                    tabla = funciones_globales.obtener("inforplan", "SELECT fecha_inicial_datos,fecha_final_datos,fecha_inicial_impuesto ,fecha_final_impuesto FROM empresa WHERE ruta_base_datos='" & bbdd & "';")

                    'Compruebo las fechas
                    Dim fecha_inicial_asiento As Date = CDate(tabla.Rows(0)(0))
                    Dim fecha_final_asiento As Date = CDate(tabla.Rows(0)(1))
                    Dim fecha_inicial_impuestos As Date = CDate(tabla.Rows(0)(2))
                    Dim fecha_final_impuestos As Date = CDate(tabla.Rows(0)(3))
                    Dim fecha_evaluar As Date = CDate(vector_elementos(0))

                    If fecha_evaluar > fecha_final_impuestos Or fecha_evaluar < fecha_inicial_impuestos Then
                        Return "Los rangos permitidos para la entrada de impuestos van desde: " & fecha_inicial_impuestos & " al " & fecha_final_impuestos
                    End If

                    If fecha_evaluar > fecha_final_asiento Or fecha_evaluar < fecha_inicial_asiento Then
                        Return "Los rangos permitidos para la entrada de asientos van desde: " & fecha_inicial_asiento & " al " & fecha_final_asiento
                    End If

                    'Comprobar que la cuenta existe
                    tabla = funciones_globales.obtener(bbdd, "SELECT denominacion,nif FROM plan_cuenta_personalizadas WHERE cuenta='" & vector_elementos(2) & "';")
                    If tabla.Rows.Count = 0 Then
                        Return "La cuenta " & vector_elementos(2) & " no existe"
                    Else
                        denominacion_cuenta = tabla.Rows(0)(0).ToString
                        nif_cuenta = tabla.Rows(0)(1).ToString
                    End If

                    'Comprobar el tipo de impuesto para esa cuenta (IGIC,IVA,IPSI)
                    Dim tabla_tipo_impuesto As DataTable = funciones_globales.obtener("inforplan", "SELECT * FROM cuenta_impuestos_cliente WHERE cuenta='" & Mid(vector_elementos(2), 1, 3) & "' AND Id_tipo_plan_cuentas=" & id_tipo_plan_cuentas & " AND impuesto='" & vector_elementos(3) & "';")
                    If tabla_tipo_impuesto.Rows.Count = 0 Then
                        Return "El tipo de impuesto no es correcto"
                    Else
                        tipo_impuesto = vector_elementos(3)
                    End If

                    'Comprobar si ya esta metida o no :SOPORTADO
                    tabla = funciones_globales.obtener(bbdd, "SELECT Id FROM cabecera_impuestos WHERE cuenta='" & vector_elementos(2) & "' AND fecha_expedicion BETWEEN '01/01/" & Year(fecha_evaluar) & "' AND '31/12/" & Year(fecha_evaluar) & "' AND numero_factura='" & Trim(vector_elementos(7)) & "';")
                    If tabla.Rows.Count <> 0 Then
                        Return "Nº de Factura y Cuenta ya contabilizada"
                    End If
                    'Comprobar si ya esta metida o no :REPERCUTIDO
                    tabla = funciones_globales.obtener(bbdd, "SELECT Id FROM cabecera_impuestos WHERE serie_factura='" & Trim(vector_elementos(6)) & "' AND fecha_expedicion BETWEEN '01/01/" & Year(fecha_evaluar) & "' AND '31/12/" & Year(fecha_evaluar) & "' AND numero_factura='" & Trim(vector_elementos(7)) & "' AND tipo_impuesto = '" & vector_elementos(3) & "';")
                    If tabla.Rows.Count <> 0 Then
                        Return "Nº de Factura y Cuenta ya contabilizada"
                    End If

                    'Comprobar que el concepto no esta vacío
                    If Trim(vector_elementos(8)) = "" Then
                        Return "El concepto no puede estar vacío"
                    End If

                    'Comprobamos que el importe no puede ser 0
                    total_factura = CDec(Trim(vector_elementos(13)).Replace(".", ","))
                    If total_factura = 0 Then
                        Return "El importe de la factura no puede ser 0"
                    End If

                    'La denominacion del tipo de asiento debe existir
                    Dim tabla_denominacion_tipo_asiento As DataTable = funciones_globales.obtener(bbdd, "SELECT * FROM tipo_asientos WHERE denominacion='" & vector_elementos(21) & "';")
                    If tabla_denominacion_tipo_asiento.Rows.Count = 0 Then
                        Return "El Tipo de Asiento no existe"
                    End If

                    'Asigno
                    cuentas.Add(Trim(vector_elementos(2)))
                    referencia = vector_elementos(22)
                    serie = Trim(vector_elementos(6))
                    factura = Trim(vector_elementos(7))
                    concepto = Trim(vector_elementos(8))

                    'Añado para crear el detalle de asiento
                    Dim Renglon As DataRow = tabla_cuotas_bases.NewRow()
                    Renglon("Cuenta") = Trim(vector_elementos(2))
                    Renglon("Denominacion") = denominacion_cuenta
                    Renglon("NIF") = nif_cuenta
                    If tipo_impuesto.Replace("IGIC ", "").Replace("IVA ", "").Replace("IPSI ", "") = "REPERCUTIDO" Then
                        Renglon("Debe") = vector_elementos(13).Replace(".", ",")
                        Renglon("Haber") = 0
                    Else
                        Renglon("Debe") = 0
                        Renglon("Haber") = vector_elementos(13).Replace(".", ",")
                    End If
                    tabla_cuotas_bases.Rows.Add(Renglon)

                End If

                If x > 0 Then

                    'Comprobar clave
                    If tipo_impuesto.Replace("IGIC ", "").Replace("IVA ", "").Replace("IPSI ", "") = "REPERCUTIDO" Then
                        vector_elementos(1) = "1 - Régimen Ordinario."
                        'If vector_elementos(1) = "1 - Régimen Ordinario." Then
                        '    Return "La Clave no corresponde al tipo de impuesto seleccionado"
                        'End If
                    Else
                        vector_elementos(1) = "1 - Cuotas soportadas en operaciones interiores, Bienes y servicios corrientes."
                        'If vector_elementos(1) <> "1 - Cuotas soportadas en operaciones interiores, Bienes y servicios corrientes." Then
                        '    Return "La Clave no corresponde al tipo de impuesto seleccionado"
                        'End If
                    End If

                    'Comprobar % impuesto
                    Dim nombre_impuesto As String = tipo_impuesto.Replace(" REPERCUTIDO", "").Replace(" SOPORTADO", "")
                    tabla = funciones_globales.obtener(bbdd, "SELECT Id FROM tipo_impuestos WHERE porcentaje=" & vector_elementos(2) & " AND nombre='" & nombre_impuesto & "';")
                    If tabla.Rows.Count = 0 Then
                        Return "El porcentaje: " & vector_elementos(2) & " no existe."
                    End If

                    'Comprobar que la cuenta_cuota existe
                    tabla = funciones_globales.obtener(bbdd, "SELECT denominacion,nif FROM plan_cuenta_personalizadas WHERE cuenta='" & vector_elementos(8) & "';")
                    If tabla.Rows.Count = 0 Then
                        Return "La cuenta_cuota " & vector_elementos(8) & " no existe"
                    Else
                        Dim Renglon As DataRow = tabla_cuotas_bases.NewRow()
                        Renglon("Cuenta") = Trim(vector_elementos(8))
                        Renglon("Denominacion") = tabla.Rows(0)(0).ToString
                        Renglon("NIF") = tabla.Rows(0)(1).ToString
                        If tipo_impuesto.Replace("IGIC ", "").Replace("IVA ", "").Replace("IPSI ", "") = "REPERCUTIDO" Then
                            Renglon("Debe") = 0
                            Renglon("Haber") = vector_elementos(3).Replace(".", ",")
                        Else
                            Renglon("Debe") = vector_elementos(3).Replace(".", ",")
                            Renglon("Haber") = 0
                        End If
                        tabla_cuotas_bases.Rows.Add(Renglon)
                        'Asigno
                        cuentas.Add(Trim(vector_elementos(8)))
                    End If

                    'Comprobar que la cuenta_base existe
                    tabla = funciones_globales.obtener(bbdd, "SELECT denominacion,nif FROM plan_cuenta_personalizadas WHERE cuenta='" & vector_elementos(9) & "';")
                    If tabla.Rows.Count = 0 Then
                        Return "La cuenta_base " & vector_elementos(9) & " no existe"
                    Else
                        Dim Renglon As DataRow = tabla_cuotas_bases.NewRow()
                        Renglon("Cuenta") = Trim(vector_elementos(9))
                        Renglon("Denominacion") = tabla.Rows(0)(0).ToString
                        Renglon("NIF") = tabla.Rows(0)(1).ToString
                        If tipo_impuesto.Replace("IGIC ", "").Replace("IVA ", "").Replace("IPSI ", "") = "REPERCUTIDO" Then
                            Renglon("Debe") = 0
                            Renglon("Haber") = vector_elementos(4).Replace(".", ",")
                        Else
                            Renglon("Debe") = vector_elementos(4).Replace(".", ",")
                            Renglon("Haber") = 0
                        End If
                        tabla_cuotas_bases.Rows.Add(Renglon)
                        'Asigno
                        cuentas.Add(Trim(vector_elementos(9)))
                    End If

                    'Las cuenta base y cuota son correctas
                    If tipo_impuesto.Replace("IGIC ", "").Replace("IVA ", "").Replace("IPSI ", "") = "REPERCUTIDO" Then
                        'Cuenta Cuota
                        If Mid(vector_elementos(8), 1, 3) <> "477" Then
                            Return "La cuenta_cuota no coincide para el tipo de impuesto"
                        End If
                        'Cuenta Base
                        If Mid(vector_elementos(9), 1, 1) <> "7" Then
                            Return "La cuenta_base no coincide para el tipo de impuesto"
                        End If
                    Else
                        'Cuenta Cuota
                        If Mid(vector_elementos(8), 1, 3) <> "472" Then
                            Return "La cuenta_cuota no coincide para el tipo de impuesto"
                        End If
                        'Cuenta Base
                        If Mid(vector_elementos(9), 1, 1) <> "6" Then
                            Return "La cuenta_base no coincide para el tipo de impuesto"
                        End If
                    End If

                    'Asigno
                    suma_cuota += vector_elementos(3).Replace(".", ",")
                    suma_base += vector_elementos(4).Replace(".", ",")

                End If

            Next

            'Que la suma de las cuotas y de las bases sea el total de la factura
            If suma_cuota + suma_base <> total_factura Then
                Return "La suma total de los debe y de los haber son diferentes"
            End If

            'Si todo es OK grabo cabecera y detalles de impuestos
            'Recorro
            For x = 0 To vector_lineas.Count - 1

                'Descompongo
                Dim vector_elementos() As String = vector_lineas(x).Split("|")

                'Cabecera
                If x = 0 Then

                    'Obtengo un nuevo numero de asiento para el metodo elegido para la empresa
                    referencia_asiento = funciones_globales.asiento_tinfor(bbdd, vector_elementos(0), 1)

                    'Grabo la Cabecera de asiento
                    id_cabecera_asiento = funciones_globales.grabar_cabecera_asiento_(bbdd, vector_elementos(0), 1, "Asiento", referencia_asiento)

                    'Genero el numero de Impuesto
                    identificador = id_usuario & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "")

                    'Conecto
                    Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd)

                        'Abrimos conexion
                        memConn.Open()

                        'Grabo Datos
                        Dim memComando As New SqlCommand
                        memComando.CommandText = "INSERT INTO [cabecera_impuestos] (id,fecha_creacion,hora_creacion,fecha_expedicion,fecha_realizacion,cuenta,denominacion,nif,tipo_impuesto,cuenta_cancelacion,efectivo,serie_factura,numero_factura,codigo_concepto_impuesto,naturaleza,base_retencion,porcentaje_retencion,cuota_retencion,cuenta_retencion,total_factura_retencion,total_factura,retencion,prorrata,minorista,exento,re,prorrata_check,no_sujeto,tipo_asiento,referencia_siento,residuo) VALUES (@identificador,@fecha,@hora,@fecha_expedicion,@fecha_realizacion,@cuenta,@denominacion,@nif,@tipo_impuesto,@cuenta_cancelacion,@efectivo,@serie_factura,@numero_factura,@codigo_concepto_impuesto,@naturaleza,@base_retencion,@porcentaje_retencion,@cuota_retencion,@cuenta_retencion,@total_factura_retencion,@total_factura,@retencion,@prorrata,@minorista,@exento,@re,@prorrata_check,@no_sujeto,@tipo_asiento,@referencia_siento,@residuo);"
                        memComando.Parameters.Add("@identificador", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@identificador").Value = identificador
                        memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
                        memComando.Parameters("@fecha").Value = DateTime.Today
                        memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
                        memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
                        memComando.Parameters.Add("@fecha_expedicion", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@fecha_expedicion").Value = vector_elementos(0)
                        memComando.Parameters.Add("@fecha_realizacion", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@fecha_realizacion").Value = vector_elementos(1)
                        memComando.Parameters.Add("@cuenta", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@cuenta").Value = vector_elementos(2)
                        memComando.Parameters.Add("@denominacion", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@denominacion").Value = denominacion_cuenta
                        memComando.Parameters.Add("@nif", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@nif").Value = nif_cuenta
                        memComando.Parameters.Add("@tipo_impuesto", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@tipo_impuesto").Value = tipo_impuesto
                        memComando.Parameters.Add("@cuenta_cancelacion", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@cuenta_cancelacion").Value = Trim(vector_elementos(4))
                        memComando.Parameters.Add("@efectivo", System.Data.SqlDbType.Bit)
                        memComando.Parameters("@efectivo").Value = CBool(vector_elementos(5))
                        memComando.Parameters.Add("@serie_factura", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@serie_factura").Value = Trim(vector_elementos(6))
                        memComando.Parameters.Add("@numero_factura", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@numero_factura").Value = Trim(vector_elementos(7))
                        memComando.Parameters.Add("@codigo_concepto_impuesto", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@codigo_concepto_impuesto").Value = ""
                        memComando.Parameters.Add("@naturaleza", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@naturaleza").Value = Trim(vector_elementos(8))
                        memComando.Parameters.Add("@base_retencion", System.Data.SqlDbType.Decimal)
                        memComando.Parameters("@base_retencion").Value = vector_elementos(9)
                        memComando.Parameters.Add("@porcentaje_retencion", System.Data.SqlDbType.Decimal)
                        memComando.Parameters("@porcentaje_retencion").Value = vector_elementos(10)
                        memComando.Parameters.Add("@cuota_retencion", System.Data.SqlDbType.Decimal)
                        memComando.Parameters("@cuota_retencion").Value = vector_elementos(11)
                        memComando.Parameters.Add("@cuenta_retencion", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@cuenta_retencion").Value = Trim(vector_elementos(12))
                        memComando.Parameters.Add("@total_factura_retencion", System.Data.SqlDbType.Decimal)
                        memComando.Parameters("@total_factura_retencion").Value = vector_elementos(13).Replace(".", ",") - vector_elementos(11).Replace(".", ",")
                        memComando.Parameters.Add("@total_factura", System.Data.SqlDbType.Decimal)
                        memComando.Parameters("@total_factura").Value = vector_elementos(13).Replace(".", ",")
                        memComando.Parameters.Add("@retencion", System.Data.SqlDbType.Bit)
                        memComando.Parameters("@retencion").Value = CBool(vector_elementos(14))
                        memComando.Parameters.Add("@prorrata", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@prorrata").Value = Trim(vector_elementos(15))
                        memComando.Parameters.Add("@minorista", System.Data.SqlDbType.Bit)
                        memComando.Parameters("@minorista").Value = CBool(vector_elementos(16))
                        memComando.Parameters.Add("@exento", System.Data.SqlDbType.Bit)
                        memComando.Parameters("@exento").Value = CBool(vector_elementos(17))
                        memComando.Parameters.Add("@re", System.Data.SqlDbType.Bit)
                        memComando.Parameters("@re").Value = CBool(vector_elementos(18))
                        memComando.Parameters.Add("@prorrata_check", System.Data.SqlDbType.Bit)
                        memComando.Parameters("@prorrata_check").Value = CBool(vector_elementos(19))
                        memComando.Parameters.Add("@no_sujeto", System.Data.SqlDbType.Bit)
                        memComando.Parameters("@no_sujeto").Value = CBool(vector_elementos(20))
                        memComando.Parameters.Add("@tipo_asiento", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@tipo_asiento").Value = Trim(vector_elementos(21))
                        memComando.Parameters.Add("@referencia_siento", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@referencia_siento").Value = vector_elementos(22)
                        memComando.Parameters.Add("@residuo", System.Data.SqlDbType.VarChar)
                        memComando.Parameters("@residuo").Value = referencia_asiento & "&" & id_cabecera_asiento
                        memComando.Connection = memConn
                        memComando.ExecuteNonQuery()

                        'Cerramos
                        memComando.Dispose()
                        SqlConnection.ClearPool(memConn)

                    End Using

                End If

                If x > 0 Then

                    'Ataco a la conexion del programa
                    Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd
                    Dim memConn As New SqlConnection(ruta_base)
                    memConn.Open()

                    'Grabo detalle impuesto
                    Dim memComando As New SqlCommand
                    memComando.CommandText = "INSERT INTO [detalles_impuestos] (Id_cabecera_impuestos,fecha_creacion,hora_creacion,orden,exento,clave,porcentaje,cuota,base,porcentaje_re,cuota_re,cuenta_re,cuenta_cuota,cuenta_base,porcentaje_minorista, cuota_minorista, cuota_prorrata, cuota_deducible, base_calculada,intracomunitaria_inversion_sujeto,suma_base,suma_cuota,cuadre_importacion) VALUES " &
                        "(@identificador,@fecha,@hora," & x & ",@exento_linea,@clave,@porcentaje,@cuota,@base,@porcentaje_re,@cuota_re,@cuenta_re,@cuenta_cuota,@cuenta_base,@porcentaje_minorista, @cuota_minorista, @cuota_prorrata, @cuota_deducible, @base_calculada,@intracomunitaria_inversion_sujeto,@suma_base,@suma_cuota,@cuadre_importacion);"
                    memComando.Parameters.Add("@identificador", System.Data.SqlDbType.VarChar)
                    memComando.Parameters("@identificador").Value = identificador
                    memComando.Parameters.Add("@fecha", System.Data.SqlDbType.Date)
                    memComando.Parameters("@fecha").Value = DateTime.Today
                    memComando.Parameters.Add("@hora", System.Data.SqlDbType.Time)
                    memComando.Parameters("@hora").Value = Now.ToString("HH:mm:ss")
                    memComando.Parameters.Add("@exento_linea", System.Data.SqlDbType.Bit)
                    memComando.Parameters("@exento_linea").Value = CBool(vector_elementos(0))
                    memComando.Parameters.Add("@clave", System.Data.SqlDbType.VarChar)
                    memComando.Parameters("@clave").Value = vector_elementos(1)
                    memComando.Parameters.Add("@porcentaje", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@porcentaje").Value = vector_elementos(2).Replace(".", ",")
                    memComando.Parameters.Add("@cuota", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@cuota").Value = vector_elementos(3).Replace(".", ",")
                    memComando.Parameters.Add("@base", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@base").Value = vector_elementos(4).Replace(".", ",")
                    memComando.Parameters.Add("@porcentaje_re", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@porcentaje_re").Value = vector_elementos(5).Replace(".", ",")
                    memComando.Parameters.Add("@cuota_re", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@cuota_re").Value = vector_elementos(6).Replace(".", ",")
                    memComando.Parameters.Add("@cuenta_re", System.Data.SqlDbType.VarChar)
                    memComando.Parameters("@cuenta_re").Value = Trim(vector_elementos(7))
                    memComando.Parameters.Add("@cuenta_cuota", System.Data.SqlDbType.VarChar)
                    memComando.Parameters("@cuenta_cuota").Value = Trim(vector_elementos(8))
                    memComando.Parameters.Add("@cuenta_base", System.Data.SqlDbType.VarChar)
                    memComando.Parameters("@cuenta_base").Value = Trim(vector_elementos(9))
                    memComando.Parameters.Add("@porcentaje_minorista", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@porcentaje_minorista").Value = vector_elementos(10).Replace(".", ",")
                    memComando.Parameters.Add("@cuota_minorista", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@cuota_minorista").Value = vector_elementos(11).Replace(".", ",")
                    memComando.Parameters.Add("@cuota_prorrata", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@cuota_prorrata").Value = vector_elementos(12).Replace(".", ",")
                    memComando.Parameters.Add("@cuota_deducible", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@cuota_deducible").Value = vector_elementos(3).Replace(".", ",")
                    memComando.Parameters.Add("@base_calculada", System.Data.SqlDbType.Decimal)
                    memComando.Parameters("@base_calculada").Value = vector_elementos(4).Replace(".", ",")
                    memComando.Parameters.Add("@intracomunitaria_inversion_sujeto", System.Data.SqlDbType.VarChar)
                    memComando.Parameters("@intracomunitaria_inversion_sujeto").Value = vector_elementos(15)
                    memComando.Parameters.Add("@suma_cuota", System.Data.SqlDbType.Bit)
                    memComando.Parameters("@suma_cuota").Value = CBool(vector_elementos(16))
                    memComando.Parameters.Add("@suma_base", System.Data.SqlDbType.Bit)
                    memComando.Parameters("@suma_base").Value = CBool(vector_elementos(17))
                    memComando.Parameters.Add("@cuadre_importacion", System.Data.SqlDbType.Bit)
                    memComando.Parameters("@cuadre_importacion").Value = CBool(vector_elementos(18))
                    memComando.Connection = memConn
                    memComando.ExecuteNonQuery()

                    'Cierro
                    memComando.Dispose()
                    memConn.Close()
                    memConn.Dispose()

                    ''Asigno
                    'cuentas.Add(Trim(vector_elementos(8)))
                    'cuentas.Add(Trim(vector_elementos(9)))

                    'Dim Renglon As DataRow = tabla_cuotas_bases.NewRow()
                    'Renglon("Cuenta") = Trim(vector_elementos(8))
                    'If tipo_impuesto.Replace("IGIC ", "").Replace("IVA ", "").Replace("IPSI ", "") = "REPERCUTIDO" Then
                    '    Renglon("Debe") = 0
                    '    Renglon("haber") = vector_elementos(3).Replace(".", ",")
                    'Else
                    '    Renglon("Debe") = vector_elementos(3).Replace(".", ",")
                    '    Renglon("haber") = 0
                    'End If
                    'tabla_cuotas_bases.Rows.Add(Renglon)

                    'Dim Renglon2 As DataRow = tabla_cuotas_bases.NewRow()
                    'Renglon2("Cuenta") = Trim(vector_elementos(9))
                    'If tipo_impuesto.Replace("IGIC ", "").Replace("IVA ", "").Replace("IPSI ", "") = "REPERCUTIDO" Then
                    '    Renglon2("Debe") = 0
                    '    Renglon2("haber") = vector_elementos(4).Replace(".", ",")
                    'Else
                    '    Renglon2("Debe") = vector_elementos(4).Replace(".", ",")
                    '    Renglon2("haber") = 0
                    'End If
                    'tabla_cuotas_bases.Rows.Add(Renglon2)

                End If

            Next

            'Preparo un enumerador para quitar las cuentas duplicadas
            Dim cuentas_limpias As IEnumerable(Of String) = cuentas.Distinct()

            'Preparo una lista ordenada
            Dim myList As List(Of String) = cuentas_limpias.ToList()

            'Ordeno
            myList.Sort()

            For x = 0 To myList.Count - 1

                'Conecto
                Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & bbdd)

                    'Abrimos conexion
                    memConn.Open()

                    'Ataco a la conexion del programa
                    Dim memComando As New SqlCommand

                    'Grabo la cabecera
                    memComando.CommandText = "Usp_detalles_asientos"
                    memComando.Connection = memConn
                    memComando.CommandType = CommandType.StoredProcedure

                    'Parametros de entrada
                    memComando.Parameters.Add("@Id_cabecera_asiento", SqlDbType.Int, ParameterDirection.Input).Value = id_cabecera_asiento
                    memComando.Parameters.Add("@id_apunte_asiento", SqlDbType.Int, ParameterDirection.Input).Value = 0
                    memComando.Parameters.Add("@referencia", SqlDbType.VarChar, 20, ParameterDirection.Input).Value = referencia
                    memComando.Parameters.Add("@cuenta", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = myList(x)
                    memComando.Parameters.Add("@denominacion_cuenta", SqlDbType.VarChar, 200, ParameterDirection.Input).Value = obtener_debe_haber(tabla_cuotas_bases, myList(x), "Denominacion")
                    memComando.Parameters.Add("@nif_cuenta", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = obtener_debe_haber(tabla_cuotas_bases, myList(x), "NIF")
                    memComando.Parameters.Add("@serie", SqlDbType.VarChar, 6, ParameterDirection.Input).Value = serie
                    memComando.Parameters.Add("@factura", SqlDbType.VarChar, 20, ParameterDirection.Input).Value = factura
                    memComando.Parameters.Add("@id_codigo_concepto", SqlDbType.Int, ParameterDirection.Input).Value = 0
                    memComando.Parameters.Add("@concepto", SqlDbType.VarChar, 200, ParameterDirection.Input).Value = concepto
                    memComando.Parameters.Add("@debe", SqlDbType.Decimal, ParameterDirection.Input).Value = obtener_debe_haber(tabla_cuotas_bases, myList(x), "Debe")
                    memComando.Parameters.Add("@haber", SqlDbType.Decimal, ParameterDirection.Input).Value = obtener_debe_haber(tabla_cuotas_bases, myList(x), "Haber")
                    memComando.Parameters.Add("@id_tipo_plan_cuentas", SqlDbType.Int, ParameterDirection.Input).Value = id_tipo_plan_cuentas
                    memComando.Parameters.Add("@id_cobrador", SqlDbType.Int, ParameterDirection.Input).Value = 0
                    memComando.Parameters.Add("@id_banco_empresa", SqlDbType.Int, ParameterDirection.Input).Value = 0
                    memComando.Parameters.Add("@id_banco_cliente", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = "0"
                    memComando.Parameters.Add("@id_forma_pago", SqlDbType.Int, ParameterDirection.Input).Value = 0
                    memComando.Parameters.Add("@origen", SqlDbType.VarChar, 15, ParameterDirection.Input).Value = "Impuestos"
                    memComando.Parameters.Add("@id_cabecera_impuesto", SqlDbType.VarChar, 50, ParameterDirection.Input).Value = identificador
                    memComando.Parameters.Add("@id_cod_usuario", SqlDbType.Int, ParameterDirection.Input).Value = id_usuario
                    memComando.Parameters.Add("@nivel_cod_usuario", SqlDbType.VarChar, 20, ParameterDirection.Input).Value = nivel
                    memComando.Parameters.Add("@id_cabecera_inmovilizado", SqlDbType.VarChar, 8000, ParameterDirection.Input).Value = "0"

                    'Parametros de salida
                    memComando.Parameters.Add("@id_detalle_asiento", SqlDbType.Int)
                    memComando.Parameters("@id_detalle_asiento").Direction = ParameterDirection.Output
                    memComando.ExecuteNonQuery()

                    'Libero recurso
                    memComando.Dispose()
                    SqlConnection.ClearPool(memConn)

                End Using

            Next

            'Cierro la base de datos
            tabla.Dispose()

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Grabar Registro
            funciones_globales.grabar_registro(bbdd, id_usuario, nombre_usuario & " " & primer_apellido & " " & segundo_apellido, "WS API_Impuestos", "Creó cabecera de impuesto con Nº: " & identificador & " y Nº Asiento: " & referencia_asiento & " (Tiempo Grabación: " & tspan.Seconds & "," & tspan.Milliseconds & " Milisegundos).")

            'Devuelvo
            Return "OK-" & identificador

        Catch ex As Exception
            Return ex.Message & "Ha ocurrido un Error"
        End Try

    End Function

    Public Function obtener_debe_haber(ByVal tabla_debe_haber As DataTable, ByVal cuenta As String, ByVal campo_solicitado As String) As String

        'Hago Select
        Dim expression As String
        expression = "cuenta = '" & cuenta & "' "
        Dim foundRows() As DataRow

        'Almaceno todos los rows obtenidos
        foundRows = tabla_debe_haber.Select(expression)

        Dim i As Integer
        Dim resultado As String = Nothing
        Dim sum_debe As Decimal = 0
        Dim sum_haber As Decimal = 0

        'Recorro para obtener los datos
        For i = 0 To foundRows.GetUpperBound(0)

            Select Case campo_solicitado
                Case "Denominacion"
                    resultado = foundRows(i)(1)
                Case "NIF"
                    resultado = foundRows(i)(2)
                Case "Debe"
                    sum_debe += foundRows(i)(3)
                Case "Haber"
                    sum_haber += foundRows(i)(4)

            End Select


            'If Not IsDBNull(foundRows(i)(5)) Then
            '    sum_debe += foundRows(i)(5)
            'End If
            'If Not IsDBNull(foundRows(i)(6)) Then
            '    sum_haber += foundRows(i)(6)
            'End If

        Next i

        Select Case campo_solicitado
            Case "Debe"
                resultado = sum_debe
            Case "Haber"
                resultado = sum_haber
        End Select

        'Devuelvo resultado
        Return resultado

    End Function

    <WebMethod()>
    Public Function Obtener_datos(ByVal KeyCode As String, ByVal query As String) As String

        'Declaro
        Dim bbdd As String = Nothing
        Dim id_usuario As Integer = Nothing
        Dim nombre_usuario As String = Nothing
        Dim primer_apellido As String = Nothing
        Dim segundo_apellido As String = Nothing

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Descompongo el Token
            Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
            querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, KeyCode)

            'Compruebo si la empresa permite api
            bbdd = querystringSeguro("bbdd").ToString
            id_usuario = querystringSeguro("usuario").ToString
            nombre_usuario = querystringSeguro("nombre").ToString
            primer_apellido = querystringSeguro("1_apellido").ToString
            segundo_apellido = querystringSeguro("2_apellido").ToString

            'Excepciones
            'Comprobacion API
            Dim tabla As DataTable = funciones_globales.obtener("inforplan", "SELECT api FROM empresa WHERE ruta_base_datos='" & bbdd & "';")
            If tabla.Rows(0)(0) = False Then
                Return "Esta empresa no tiene activada las funciones de API."
            End If

            'Impedir Insert,Update o Delete
            If query.ToUpper.IndexOf("INSERT") <> -1 Or query.ToUpper.IndexOf("UPDATE") <> -1 Or query.ToUpper.IndexOf("DELETE") <> -1 Then
                Return "Sólo puedes utilizar el comando SELECT"
            End If

            'Petición
            If query.ToUpper.IndexOf("SELECT TOP") <> -1 Then
                tabla = funciones_globales.obtener(bbdd, query)
            Else
                tabla = funciones_globales.obtener(bbdd, query.Replace("SELECT ", "SELECT TOP(10000) "))
            End If

            'Si solo devuelve 1 dato            
            If tabla.Rows.Count = 1 Then
                Return tabla.Rows(0)(0)
            Else
                Return GetJson(tabla)
            End If

        Catch ex As Exception
            Return ex.Message & "Ha ocurrido un Error"
        End Try

    End Function

End Class