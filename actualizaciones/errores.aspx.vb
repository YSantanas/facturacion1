Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class actualizaciones_errores
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Public Sub leer_ejercicio()

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Asigno
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT YEAR(cabecera_asientos.fecha) as ano FROM cabecera_asientos GROUP BY year(cabecera_asientos.fecha) ORDER BY year(cabecera_asientos.fecha) DESC;"
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

            'Limpio el control
            DDL_ejercicio.Items.Clear()

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("ano").ToString, tabla_consulta.Rows(x)("ano").ToString)
                Me.DDL_ejercicio.Items.Add(lista)

            Next

            'Limpio
            tabla_consulta.Dispose()

            'Seleccion el año actual
            DDL_ejercicio.SelectedIndex = DDL_ejercicio.Items.IndexOf(DDL_ejercicio.Items.FindByValue(Year(Now)))

            'Cargo las fechas
            If DDL_ejercicio.SelectedItem.Text <> "" And DDL_ejercicio.SelectedItem.Text <> "Todos" Then
                leer_fechas(DDL_ejercicio.SelectedItem.Text)
            Else
                lbl_fechas_proceso.Text = Nothing
            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_ejercicio", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_ejercicio: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_fechas(ByVal ejercicio As Integer)

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Calculo la fecha inicial y final
            txt_fecha_inicial.Text = funciones_globales.fecha_inicial(ejercicio, parametros_empresa(12))
            txt_fecha_final.Text = funciones_globales.fecha_final(parametros_empresa(12), ejercicio, parametros_empresa(12))

            'Asigno valor a los label
            lbl_fechas_proceso.Text = "(Periodo del: " & txt_fecha_inicial.Text & " al: " & txt_fecha_final.Text & ")"

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_fechas", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_fechas: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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

                'Restricción de Usuarios
                If parametros_usuario(10) = "Invitado" Or parametros_usuario(10) = "Usuario Restringido" Or parametros_usuario(10) = "Usuario" Then
                    'Bloque Jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                End If

                'Cargo los ejercicios
                leer_ejercicio()

                'Asigno foco
                DDL_ejercicio.Focus()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub DDL_ejercicio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_ejercicio.SelectedIndexChanged

        Try

            'Cargo las fechas
            If DDL_ejercicio.SelectedItem.Text <> "" And DDL_ejercicio.SelectedItem.Text <> "Todos" Then
                leer_fechas(DDL_ejercicio.SelectedItem.Text)
            Else
                lbl_fechas_proceso.Text = Nothing
            End If

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "setTimeout(function () { $('#DDL_ejercicio').focus();}, 100);", True)

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "DDL_ejercicio_SelectedIndexChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error DDL_ejercicio_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_consultar_Click(sender As Object, e As EventArgs) Handles btn_consultar.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

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
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Control", "Solicitó listado Extraer Cuentas para plantillas PDFs para el ejercicio: " & DDL_ejercicio.SelectedItem.ToString & ".")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_consultar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_consultar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub cargar_GV()

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Declaro
            Dim tabla_bbdd As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                'Conecto
                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT COUNT(*) AS contador,nif,denominacion from cabecera_impuestos " &
                "WHERE fecha_realizacion BETWEEN '" & txt_fecha_inicial.Text & "' AND '" & txt_fecha_final.Text & "' " &
                "GROUP BY nif,denominacion " &
                "ORDER BY COUNT(*) DESC"
                memComando.Connection = memConn

                'Creamos un adaptador de datos
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

                'Llenamos de datos
                adapter.Fill(tabla_bbdd)

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Asigno
            gridview_consulta.DataSource = tabla_bbdd
            gridview_consulta.DataBind()

            'Limpio
            tabla_bbdd.Dispose()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Completada Correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "cargar_GV", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error cargar_GV: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        Try

            'Recorro el GV
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim onmouseoverStyle As String = "this.style.backgroundColor='#f2ffe4';this.style.cursor='Default'"
                Dim onmouseoutStyle As String = "this.style.backgroundColor='white'"
                Dim cursoronmouse As String = "this.style.cursor='Default'"

                'Asigno
                e.Row.Attributes.Add("onmouseover", onmouseoverStyle)
                e.Row.Attributes.Add("onmouseout", onmouseoutStyle)

            End If

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
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

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_asiento_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_asiento_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
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
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_PageIndexChanging", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_PageIndexChanging: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_reset_Click(sender As Object, e As ImageClickEventArgs) Handles img_reset.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Refresco Ejercicio
            leer_ejercicio()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_reset_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_reset_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_excel_Click(sender As Object, e As ImageClickEventArgs) Handles img_exportar_excel.Click

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_excel("Extraer Cuentas", gridview_consulta)

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
            Dim nombre As String = funciones_globales.crear_txt("Extraer Cuentas", gridview_consulta)

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


    Sub gestion_documental()

        Try


            'Asigno
            Dim tabla As DataTable

            'Obtengo
            tabla = funciones_globales.obtener("inforplan", "select Id, ruta_base_datos from empresa;")

            'Recorro
            For x = 0 To tabla.Rows.Count - 1

                'Asigno
                Dim tabla_ficheros As DataTable

                'Leo
                tabla_ficheros = funciones_globales.obtener(tabla.Rows(x)("ruta_base_datos"), "SELECT ruta,nombre_fichero FROM ficheros;")

                'Recorro
                For y = 0 To tabla_ficheros.Rows.Count - 1

                    'Asigno
                    Dim ruta_destino As String = "D:\" & tabla_ficheros.Rows(y)("ruta") & tabla_ficheros.Rows(y)("nombre_fichero")
                    Dim ruta_origen As String = "D:\" & tabla_ficheros.Rows(y)("ruta").ToString.Replace("imagenes_usuarios\" & tabla.Rows(x)("ruta_base_datos").ToString & "\", "images_usuarios\" & tabla.Rows(x)("Id").ToString & "\") & tabla_ficheros.Rows(y)("nombre_fichero")

                    ''Existe
                    If My.Computer.FileSystem.FileExists(ruta_origen) Then

                        'Muevo
                        My.Computer.FileSystem.MoveFile(ruta_origen, ruta_destino, True)

                    End If

                Next

            Next

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub






    'Protected Sub Sql_acumulados_Init(sender As Object, e As EventArgs) Handles Sql_acumulados.Init

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Conecto al plan de cuentas personalizadas
    '        Sql_acumulados.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Sql_errores_Init", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Sql_errores_Init: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Public Sub leer_ejercicio()

    '    Try

    '        Dim ejercicio As String = HttpContext.Current.Session("tabla_ejercicios")

    '        If ejercicio <> "" Then

    '            'Cargo los valores de la session
    '            Dim valor_linea() As String = ejercicio.Split("|")

    '            'Limpio el control
    '            DDL_ejercicio_impuesto_asiento.Items.Clear()

    '            'Recorro
    '            For z = 0 To valor_linea.GetUpperBound(0)

    '                'Cargo los detalles
    '                Dim lista_paises As New System.Web.UI.WebControls.ListItem(valor_linea(z), valor_linea(z))
    '                Me.DDL_ejercicio_impuesto_asiento.Items.Add(lista_paises)
    '                Me.DDL_leer_fecha_extraccion.Items.Add(lista_paises)

    '                'Seleccion el año actual
    '                DDL_ejercicio_impuesto_asiento.SelectedIndex = DDL_ejercicio_impuesto_asiento.Items.IndexOf(DDL_ejercicio_impuesto_asiento.Items.FindByValue(Year(Now)))

    '                'Cargo las fechas
    '                If DDL_ejercicio_impuesto_asiento.SelectedItem.Text <> "" Then
    '                    leer_fechas_impuesto_asiento(DDL_ejercicio_impuesto_asiento.SelectedItem.Text)
    '                End If

    '                'Seleccion el año actual
    '                DDL_leer_fecha_extraccion.SelectedIndex = DDL_leer_fecha_extraccion.Items.IndexOf(DDL_leer_fecha_extraccion.Items.FindByValue(Year(Now)))

    '                'Cargo las fechas
    '                If DDL_leer_fecha_extraccion.SelectedItem.Text <> "" Then
    '                    leer_fecha_extraccion(DDL_leer_fecha_extraccion.SelectedItem.Text)
    '                End If

    '            Next

    '        End If

    '    Catch ex As Exception
    '        'Cargo las variables de Usuario
    '        Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_ejercicio", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_ejercicio: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Public Sub leer_error()

    '    Try

    '        'Lo primero es borrar los acumulados anteriores
    '        'Ataco a la conexion del programa
    '        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan"
    '        Dim memConn As New SqlConnection(ruta_base)
    '        memConn.Open()
    '        Dim memComando As New SqlCommand

    '        Dim string_query As String = "SELECT ruta_base_datos,empresa " &
    '        "FROM historico_sessiones_activas, empresa " &
    '        "WHERE fecha='03/05/2021' " &
    '        "AND historico_sessiones_activas.empresa=empresa.id " &
    '        "GROUP BY ruta_base_datos, empresa " &
    '        "ORDER BY empresa;"

    '        'Sentencia completa
    '        memComando.CommandText = string_query
    '        memComando.Connection = memConn

    '        'Creamos un adaptador de datos
    '        Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '        'Una tabla para contenerlos
    '        Dim tabla_cuentas As New DataTable

    '        'Llenamos de datos
    '        adapter.Fill(tabla_cuentas)
    '        memComando.Dispose()
    '        Dim contador As Integer = 1

    '        For x = 0 To tabla_cuentas.Rows.Count - 1

    '            'Response.Write(tabla_cuentas.Rows(x)(0) & "<br>")


    '            Dim query As String = "select * " &
    '            "from [" & tabla_cuentas.Rows(x)(0) & "].[dbo].detalles_asientos " &
    '            "where IsNumeric(cuenta) = 0;"

    '            'Sentencia completa
    '            memComando.CommandText = query
    '            memComando.Connection = memConn

    '            'Creamos un adaptador de datos
    '            Dim adapter_now As SqlDataAdapter = New SqlDataAdapter(memComando)

    '            'Una tabla para contenerlos
    '            Dim tabla_cuentas_now As New DataTable

    '            'Llenamos de datos
    '            adapter.Fill(tabla_cuentas_now)
    '            memComando.Dispose()


    '            If tabla_cuentas_now.Rows.Count <> 0 Then
    '                Response.Write(tabla_cuentas.Rows(x)(0) & "<br>")
    '            End If

    '            For z = 0 To tabla_cuentas_now.Rows.Count - 1

    '                Response.Write(contador & "__" & tabla_cuentas_now.Rows(z)(5).ToString & "<br>")
    '                contador += 1

    '            Next

    '        Next

    '    Catch ex As Exception
    '        'Cargo las variables de Usuario
    '        Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_ejercicio", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_ejercicio: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Sub leer_fechas_impuesto_asiento(ByVal ejercicio As Integer)

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Calculo la fecha inicial y final
    '        txt_fecha_inicial.Text = funciones_globales.fecha_inicial(ejercicio, parametros_empresa(12))
    '        txt_fecha_final.Text = funciones_globales.fecha_final(parametros_empresa(12), ejercicio, parametros_empresa(12))

    '        'Asigno valor a los label
    '        lbl_fechas_proceso_impuesto_asiento.Text = "(Periodo del: " & txt_fecha_inicial.Text & " al: " & txt_fecha_final.Text & ")"

    '        'Ataco a la conexion del programa
    '        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" + parametros_empresa(5)
    '        Dim memConn As New SqlConnection(ruta_base)
    '        Dim memComando As New SqlCommand

    '        memComando.CommandText = "SELECT COUNT(*) as Contador " &
    '            "FROM cabecera_impuestos " &
    '            "WHERE (residuo='' or residuo is null or residuo='0') " &
    '            "AND fecha_realizacion BETWEEN '" & txt_fecha_inicial.Text & "' AND '" & txt_fecha_final.Text & "';"
    '        memComando.Connection = memConn
    '        memConn.Open()

    '        'Creamos un adaptador de datos
    '        Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '        'Vaciamos y volcamos datos
    '        Dim tabla_bbdd As New DataTable

    '        'Llenamos de datos
    '        adapter.Fill(tabla_bbdd)

    '        'Liberamos recursos
    '        adapter.Dispose()
    '        memComando.Dispose()
    '        memConn.Close()
    '        memConn.Dispose()

    '        'Asigno
    '        GV_impuesto_asiento.DataSource = tabla_bbdd
    '        GV_impuesto_asiento.DataBind()
    '        GV_impuesto_asiento.DataSource = Nothing

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_fechas", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_fechas: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub DDL_ejercicio_impuesto_asiento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_ejercicio_impuesto_asiento.SelectedIndexChanged

    '    Try

    '        If DDL_ejercicio_impuesto_asiento.SelectedItem.ToString <> "" Then
    '            leer_fechas_impuesto_asiento(DDL_ejercicio_impuesto_asiento.SelectedItem.Text)
    '        End If

    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "posicion", "$('.nav-tabs a[href=" + """#tab5default""" + "]').tab('show');", True)

    '    Catch ex As Exception
    '        'Cargo las variables de Usuario
    '        Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "DDL_ejercicio_impuesto_asiento_SelectedIndexChanged", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error DDL_ejercicio_impuesto_asiento_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub btn_index_impuesto_asiento_Click(sender As Object, e As EventArgs) Handles btn_index_impuesto_asiento.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Tiempo Inicio del Proceso
    '        Dim date1 As Date = Date.Now

    '        'Ataco a la conexion del programa
    '        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" + parametros_empresa(5)
    '        Dim memConn As New SqlConnection(ruta_base)
    '        Dim memComando As New SqlCommand

    '        memComando.CommandText = "SELECT cabecera_asientos.referencia_asiento, cabecera_asientos.id, detalles_asientos.id_cabecera_impuestos " &
    '            "FROM detalles_asientos, cabecera_asientos " &
    '            "WHERE id_cabecera_impuestos <>'0' " &
    '            "AND cabecera_asientos.fecha BETWEEN '" & txt_fecha_inicial.Text & "' AND '" & txt_fecha_final.Text & "' " &
    '            " And cabecera_asientos.id = detalles_asientos.Id_cabecera_asientos " &
    '            "group by cabecera_asientos.referencia_asiento,cabecera_asientos.id, detalles_asientos .id_cabecera_impuestos " &
    '            "order by detalles_asientos .id_cabecera_impuestos "
    '        memComando.Connection = memConn
    '        memConn.Open()

    '        'Creamos un adaptador de datos
    '        Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '        'Vaciamos y volcamos datos
    '        Dim tabla_bbdd As New DataTable

    '        'Llenamos de datos
    '        adapter.Fill(tabla_bbdd)

    '        'Liberamos recursos
    '        adapter.Dispose()
    '        memComando.Dispose()

    '        'Recorro
    '        For x = 0 To tabla_bbdd.Rows.Count - 1

    '            'Actualizo
    '            memComando.CommandText = "UPDATE cabecera_impuestos Set " &
    '                "residuo='" & tabla_bbdd.Rows(x)(0) & "|" & tabla_bbdd.Rows(x)(1) & "' " &
    '                "WHERE Id='" & tabla_bbdd.Rows(x)(2) & "';"
    '            memComando.Connection = memConn
    '            memComando.ExecuteNonQuery()
    '            memComando.Dispose()

    '        Next

    '        'Consulto para saber quien no tiene relacion
    '        memComando.CommandText = "SELECT * " &
    '            "FROM cabecera_impuestos " &
    '            "WHERE (residuo='' or residuo is null or residuo='0') " &
    '            "AND fecha_realizacion BETWEEN '" & txt_fecha_inicial.Text & "' AND '" & txt_fecha_final.Text & "';"
    '        memComando.Connection = memConn
    '        memComando.ExecuteNonQuery()

    '        'Creamos un adaptador de datos
    '        Dim adapter2 As SqlDataAdapter = New SqlDataAdapter(memComando)

    '        'Vaciamos y volcamos datos
    '        Dim tabla_sin_union As New DataTable

    '        'Llenamos de datos
    '        adapter2.Fill(tabla_sin_union)

    '        'Liberamos recursos
    '        adapter2.Dispose()
    '        memComando.Dispose()

    '        'Refresco GV
    '        GV_impuesto_asiento.DataSource = tabla_sin_union
    '        GV_impuesto_asiento.DataBind()
    '        GV_impuesto_asiento.DataSource = Nothing

    '        'Cierro la base de datos
    '        memConn.Dispose()
    '        memConn.Close()

    '        'Tiempo finalización
    '        Dim date2 As DateTime = Date.Now

    '        'Diferencia
    '        Dim tspan As TimeSpan
    '        tspan = date2.Subtract(date1)

    '        'Paso la traza para grabar el Log
    '        funciones_globales.grabarregitro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Errores", "Realizó una Operación de Re-generar Index Impuesto-Asiento del año: " & DDL_ejercicio_impuesto_asiento.SelectedValue.ToString & " (Tiempo Grabación: " & tspan.Seconds & "," & tspan.Milliseconds & " Milisegundos).")

    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Re-Generar Index completada Correctamente.');$('.nav-tabs a[href=" + """#tab5default""" + "]').tab('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_index_impuesto_asiento_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_index_impuesto_asiento_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub img_reset2_Click(sender As Object, e As ImageClickEventArgs) Handles img_reset2.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Refresco la session de tabla_ejercicios
    '        funciones_globales.tabla_ejercicios(parametros_empresa(5))

    '        'Refresco Ejercicio
    '        leer_ejercicio()

    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "posicion", "$('.nav-tabs a[href=" + """#tab5default""" + "]').tab('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_reset2_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_reset2_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Sub leer_fechas_cuenta(ByVal ejercicio As Integer)

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Calculo la fecha inicial y final
    '        txt_fecha_inicial.Text = funciones_globales.fecha_inicial(ejercicio, parametros_empresa(12))
    '        txt_fecha_final.Text = funciones_globales.fecha_final(parametros_empresa(12), ejercicio, parametros_empresa(12))

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_fechas", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_fechas: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub btn_grupo_inmovilizado_Click(sender As Object, e As EventArgs) Handles btn_grupo_inmovilizado.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Tiempo Inicio del Proceso
    '        Dim date1 As Date = Date.Now

    '        'Ataco a la conexion del programa
    '        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" + parametros_empresa(5)
    '        Dim memConn As New SqlConnection(ruta_base)
    '        Dim memComando As New SqlCommand

    '        memComando.CommandText = "SELECT cuenta_debe_amortizacion,cuenta_haber_amortizacion,tipo_amortizacion,valor " &
    '            "FROM cabecera_inmovilizado " &
    '            "GROUP BY cuenta_debe_amortizacion,cuenta_haber_amortizacion,tipo_amortizacion,valor " &
    '            "ORDER BY cuenta_debe_amortizacion,cuenta_haber_amortizacion,tipo_amortizacion,valor "
    '        memComando.Connection = memConn
    '        memConn.Open()

    '        'Creamos un adaptador de datos
    '        Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '        'Vaciamos y volcamos datos
    '        Dim tabla_grupos As New DataTable

    '        'Llenamos de datos
    '        adapter.Fill(tabla_grupos)

    '        'Liberamos recursos
    '        adapter.Dispose()
    '        memComando.Dispose()

    '        'Recorro las cuentas
    '        For x = 0 To tabla_grupos.Rows.Count - 1

    '            'Variable
    '            Dim denominacion As String = tabla_grupos.Rows(x)("cuenta_debe_amortizacion").ToString & "-" & tabla_grupos.Rows(x)("cuenta_haber_amortizacion").ToString
    '            Dim tipo_amortizacion As String = tabla_grupos.Rows(x)("tipo_amortizacion").ToString.Replace("A?os", "Años")
    '            Dim valor As String = tabla_grupos.Rows(x)("valor").ToString

    '            'Compruebo que no exista
    '            If funciones_globales.Obtener_datos(parametros_empresa(5), "SELECT TOP(1) id FROM inmovilizado_grupos WHERE denominacion='" & denominacion & "';") = "0" Then

    '                'Traer el último id
    '                Dim id_grupo As Integer = funciones_globales.Obtener_datos(parametros_empresa(5), "SELECT TOP(1) id FROM inmovilizado_grupos ORDER BY id DESC;")

    '                'Variables
    '                Dim beneficio As String = 6
    '                Dim perdida As String = 7

    '                'Creo el grupo
    '                memComando.CommandText = "INSERT INTO inmovilizado_grupos (Id,denominacion,medio,valor,cuenta_debe,cuenta_haber,cuenta_beneficio_venta,cuenta_perdida_venta) VALUES " &
    '                "(" & id_grupo + 1 & ",'" & denominacion & "','" & tabla_grupos.Rows(x)("tipo_amortizacion").ToString.Replace("A?os", "Años") & "','" & tabla_grupos.Rows(x)("valor").ToString & "','" & tabla_grupos.Rows(x)("cuenta_debe_amortizacion").ToString & "', '" & tabla_grupos.Rows(x)("cuenta_haber_amortizacion").ToString & "','" & beneficio.PadRight(parametros_empresa(7), "0") & "','" & perdida.PadRight(parametros_empresa(7), "0") & "');"
    '                memComando.ExecuteNonQuery()

    '                'UPDATE
    '                memComando.CommandText = "UPDATE cabecera_inmovilizado SET codigo_grupo=" & id_grupo + 1 & " WHERE cuenta_debe_amortizacion='" & tabla_grupos.Rows(x)("cuenta_debe_amortizacion").ToString & "' AND cuenta_haber_amortizacion='" & tabla_grupos.Rows(x)("cuenta_haber_amortizacion").ToString & "';"
    '                memComando.ExecuteNonQuery()

    '            End If

    '        Next

    '        'Cierro la base de datos
    '        memConn.Dispose()
    '        memConn.Close()

    '        'Tiempo finalización
    '        Dim date2 As DateTime = Date.Now

    '        'Diferencia
    '        Dim tspan As TimeSpan
    '        tspan = date2.Subtract(date1)

    '        'Paso la traza para grabar el Log
    '        funciones_globales.grabarregitro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Errores", "Realizó una Operación Generar Grupo Inmovilizados (Tiempo Grabación: " & tspan.Seconds & "," & tspan.Milliseconds & " Milisegundos).")

    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Generar Grupo Inmovilizados completada Correctamente.');$('.nav-tabs a[href=" + """#tab9default""" + "]').tab('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_grupo_inmovilizado_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_grupo_inmovilizado_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub btn_elimina_asientos_Click(sender As Object, e As EventArgs) Handles btn_elimina_asientos.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Tiempo Inicio del Proceso
    '        Dim date1 As Date = Date.Now

    '        'Ataco a la conexion del programa
    '        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" + parametros_empresa(5)
    '        Dim memConn As New SqlConnection(ruta_base)
    '        Dim memComando As New SqlCommand

    '        memComando.CommandText = "SELECT * FROM detalles_asientos WHERE concepto like 'N/FRA. %' order by id_cabecera_asientos;"
    '        memComando.Connection = memConn
    '        memConn.Open()

    '        'Creamos un adaptador de datos
    '        Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '        'Vaciamos y volcamos datos
    '        Dim tabla_elimina As New DataTable

    '        'Llenamos de datos
    '        adapter.Fill(tabla_elimina)

    '        'Liberamos recursos
    '        adapter.Dispose()
    '        memComando.Dispose()

    '        ''Recorro las cuentas
    '        'For x = 0 To tabla_elimina.Rows.Count - 1

    '        '    'Elimino Detalles de cartera
    '        '    memComando.CommandText = "DELETE detalles_cartera WHERE id_cabecera_cartera=" & tabla_elimina.Rows(x)(25) & ";"
    '        '    memComando.ExecuteNonQuery()

    '        '    'Elimino Cabecera de cartera
    '        '    memComando.CommandText = "DELETE cabecera_cartera WHERE id=" & tabla_elimina.Rows(x)(25) & ";"
    '        '    memComando.ExecuteNonQuery()

    '        '    'Elimino Detalles de Impuestos
    '        '    memComando.CommandText = "DELETE detalles_impuestos WHERE id_cabecera_impuestos='" & tabla_elimina.Rows(x)(24).ToString & "';"
    '        '    memComando.ExecuteNonQuery()

    '        '    'Elimino Cabecera de Impuestos
    '        '    memComando.CommandText = "DELETE cabecera_impuestos WHERE id='" & tabla_elimina.Rows(x)(24).ToString & "';"
    '        '    memComando.ExecuteNonQuery()

    '        '    'Elimino Cabecera de Asientos
    '        '    memComando.CommandText = "DELETE cabecera_asientos WHERE id=" & tabla_elimina.Rows(x)(1).ToString & ";"
    '        '    memComando.ExecuteNonQuery()

    '        '    'Elimino Detalles de Asientos
    '        '    memComando.CommandText = "DELETE detalles_asientos WHERE id_cabecera_asientos=" & tabla_elimina.Rows(x)(1).ToString & ";"
    '        '    memComando.ExecuteNonQuery()

    '        'Next

    '        ''Cierro la base de datos
    '        'memConn.Dispose()
    '        'memConn.Close()

    '        ''Tiempo finalización
    '        'Dim date2 As DateTime = Date.Now

    '        ''Diferencia
    '        'Dim tspan As TimeSpan
    '        'tspan = date2.Subtract(date1)

    '        'Response.Write("ya lo hizo" & tabla_elimina.Rows.Count & " - (Tiempo Grabación: " & tspan.Seconds & "," & tspan.Milliseconds & " Milisegundos).")

    '        ''Paso la traza para grabar el Log
    '        'funciones_globales.grabarregitro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "Errores", "Realizó una Operación Generar Grupo Inmovilizados (Tiempo Grabación: " & tspan.Seconds & "," & tspan.Milliseconds & " Milisegundos).")

    '        ''Registro como bloque en local para el jquery
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Generar Grupo Inmovilizados completada Correctamente.');$('.nav-tabs a[href=" + """#tab9default""" + "]').tab('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_elimina_asientos_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_elimina_asientos_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub btn_restaurar_Click(sender As Object, e As EventArgs) Handles btn_restaurar.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Nombre
    '        Dim nombre As String = Server.MapPath("..\temp\a" + FileUpload_logo.FileName)
    '        Dim nombre_nuevo As String = Server.MapPath("..\temp\Nuevo_" + FileUpload_logo.FileName)

    '        'Subo el fichero de manera asincronica
    '        FileUpload_logo.SaveAs(nombre)

    '        'Elimino
    '        If File.Exists(nombre_nuevo) Then
    '            File.Delete(nombre_nuevo)
    '        End If

    '        'Comienzo con la lectura del fichero
    '        Dim lector_log As New System.IO.StreamReader(nombre)

    '        'Leer el contenido mientras no se llegue al final
    '        Dim contador As Integer = 0
    '        Dim nueva_linea As String = Nothing

    '        While lector_log.Peek() <> -1
    '            Dim linea As String = lector_log.ReadLine()
    '            If Not String.IsNullOrEmpty(linea) Then

    '                If contador = 0 Then
    '                    grabar_fichero(nombre_nuevo, linea)
    '                Else

    '                    Dim vector_campos() As String = linea.Split("|")
    '                    nueva_linea = Nothing

    '                    If vector_campos(29).ToString = "" Then

    '                        nueva_linea = linea

    '                    Else

    '                        For x = 0 To 28
    '                            nueva_linea += vector_campos(x) & "|"
    '                        Next

    '                        If vector_campos(29).ToString.IndexOf("&") <> -1 Then

    '                            nueva_linea = linea

    '                        Else

    '                            nueva_linea += vector_campos(29) & "&" & vector_campos(30) & "|" & vector_campos(31) & "|"

    '                        End If

    '                    End If

    '                    'Grabar
    '                    grabar_fichero(nombre_nuevo, nueva_linea)

    '                End If


    '                'If contador = 0 Then
    '                '    'Almaceno la primera linea(nombre de campos) en un vector
    '                '    Dim vector_campos() = Mid(linea, 1, linea.Count - 1).Split("|")
    '                '    'Comienzo a leer el vector y crear el insert
    '                '    sentencia_a = "INSERT INTO [" & tabla_tareas.Rows(0)(4) & "].[dbo].[" & nombre & "]  ("
    '                '    For x = 0 To vector_campos.GetUpperBound(0)
    '                '        sentencia_a += vector_campos(x) & ","
    '                '    Next
    '                '    sentencia_a = Mid(sentencia_a, 1, sentencia_a.Count - 1) & ") VALUES ("
    '                'Else
    '                '    'Almaceno la primera linea(nombre de campos) en un vector
    '                '    Dim vector_campos_valores() = Mid(linea, 1, linea.Count - 1).Split("|")

    '                '    'Comienzo a leer el vector y crear el insert
    '                '    sentencia_b = Nothing
    '                '    For x = 0 To vector_campos_valores.GetUpperBound(0)
    '                '        sentencia_b += "'" & vector_campos_valores(x).Replace(",", ".").Replace("'", " ") & "'" & ","
    '                '    Next
    '                '    'Insert final
    '                '    sentencia = sentencia_a + Mid(sentencia_b, 1, sentencia_b.Count - 1) & ");"

    '                '    'Inserto los nuevos valores
    '                '    'Comienzo la grabacion de datos
    '                '    Dim memComando As New Data.SqlClient.SqlCommand

    '                '    'Inserto
    '                '    memComando.CommandText = sentencia
    '                '    memComando.Connection = memConnSQL
    '                '    memComando.ExecuteNonQuery()
    '                '    memComando.Dispose()

    '                'End If
    '                contador += 1

    '            End If

    '        End While

    '        ' Cerrar el fichero
    '        lector_log.Close()












    '        'Response.Write(Server.MapPath("..\temp"))
    '        ''Asigno el nuevo nombre y subo el ZIP de manera temporal
    '        'txt_nombre_importacion.Text = "Importar_Tinfor_" & parametros_usuario(0) & "_" & Mid(Date.Now, 1, 2) & Mid(Date.Now, 4, 2) & Mid(Date.Now, 7, 4) & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second

    '        ''Creo la carpeta para la descompresion
    '        'If Not System.IO.Directory.Exists("D:\images_usuarios\Temp\" & txt_nombre_importacion.Text) Then
    '        '    System.IO.Directory.CreateDirectory("D:\images_usuarios\Temp\" & txt_nombre_importacion.Text)
    '        'End If

    '        '''Subo el fichero de manera asincronica
    '        'FileUpload_logo.SaveAs(Server.MapPath("..\temp\" + FileUpload_logo.FileName))

    '        ''Asaigno valores
    '        'lbl_mensaje_advertencia.Text = "Todos los datos de esta empresa serán borrados, ¿Esta totalmente seguro de que desea comenzar el proceso de importación?"

    '        ''Registro como bloque en local para el jquery
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Aviso", "$('#deleteModal').modal('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_restaurar_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_restaurar_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Sub grabar_fichero(ByVal ruta As String, ByVal linea As String)

    '    'Conecto
    '    Using flujoArchivo As FileStream = New FileStream(ruta, FileMode.Append, FileAccess.Write, FileShare.None)

    '        Using escritor As StreamWriter = New StreamWriter(flujoArchivo)

    '            escritor.WriteLine(linea)

    '        End Using

    '    End Using

    'End Sub

    'Protected Sub btn_crea_logo_Click(sender As Object, e As EventArgs) Handles btn_crea_logo.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Asigno
    '        Dim tabla_bbdd As New DataTable

    '        'Conecto
    '        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan")

    '            'Abrimos conexion
    '            memConn.Open()

    '            Dim memComando As New SqlCommand
    '            memComando.CommandText = "SELECT Id,ruta_base_datos FROM " &
    '            "empresa ORDER BY Id;"
    '            memComando.Connection = memConn

    '            'Creamos un adaptador de datos
    '            Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '            'Llenamos de datos
    '            adapter.Fill(tabla_bbdd)

    '            'Cerramos
    '            adapter.Dispose()
    '            memComando.Dispose()
    '            SqlConnection.ClearPool(memConn)

    '        End Using

    '        'Recorremos
    '        For x = 0 To tabla_bbdd.Rows.Count - 1

    '            Dim origen As String = "D:\images_usuarios\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\logo_empresa.jpg"

    '            If System.IO.File.Exists(origen) Then

    '                Dim destino As String = "D:\imagenes_usuarios\" & tabla_bbdd.Rows(x)(1).ToString & "\logo\logo_empresa.jpg"
    '                My.Computer.FileSystem.CopyFile(origen, destino)

    '            End If

    '            '    Dim destino_carpeta_logo As String = "D:\images_usuarios\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\"
    '            '    If Not System.IO.Directory.Exists(destino_carpeta_logo) Then
    '            '        System.IO.Directory.CreateDirectory(destino_carpeta_logo)
    '            '    End If

    '            '    'Copio los ficheros
    '            '    Dim origen_fichero As String = "D:\images_usuarios\" & tabla_bbdd.Rows(0)(0).ToString & "\logo\logo_empresa.jpg"
    '            '    Dim destino_fichero As String = "D:\images_usuarios\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\logo_empresa.jpg"
    '            '    If Not System.IO.File.Exists(destino_fichero) Then
    '            '        My.Computer.FileSystem.CopyFile(origen_fichero, destino_fichero, True)
    '            '    End If

    '        Next




    '        'Dim ruta_origen As String = "D:\images_usuarios2\0\logo\logo_empresa.jpg"
    '        'Dim ruta_comprobar As String = "D:\images_usuarios2\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\logo_empresa.jpg"

    '        'If System.IO.File.Exists(ruta_comprobar) Then

    '        '    'Obtengo las propiedades
    '        '    Dim info As New FileInfo(ruta_comprobar)

    '        '    If info.Length = 17912 Then

    '        '        'Copio la imagen nueva
    '        '        My.Computer.FileSystem.CopyFile(ruta_origen, ruta_comprobar, True)

    '        '    End If

    '        'End If



    '        ''Añado un campo para el orden
    '        'tabla_bbdd.Columns.Add("id_orden", System.Type.GetType("System.Int32"))

    '        ''Creo un Order
    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    'Response.Write(Mid(tabla_bbdd.Rows(x)(0).ToString, 2, tabla_bbdd.Rows(x)(0).ToString.IndexOf("O") - 1))
    '        '    tabla_bbdd.Rows(x)(1) = Mid(tabla_bbdd.Rows(x)(0).ToString, 2, tabla_bbdd.Rows(x)(0).ToString.IndexOf("O") - 1)

    '        'Next

    '        ''Creo un View
    '        'Dim dv As New DataView(tabla_bbdd)

    '        'Dim orden As String = Nothing
    '        'orden = "ASC"

    '        ''Ordeno
    '        'dv.Sort = "id_orden" & " " & orden

    '        ''Convierto la vista en una tabla
    '        'tabla_bbdd = dv.ToTable



    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    Response.Write(tabla_bbdd.Rows(x)(0).ToString & "::" & tabla_bbdd.Rows(x)(1).ToString & "<br>")
    '        'Next








    '        ''Asigno los datos de la bbdd de CP
    '        'memComando.CommandText = "SELECT ruta_base_datos " &
    '        '    "FROM [inforplan].[dbo].[empresa] " &
    '        '    "ORDER BY id;"
    '        'memComando.Connection = memConn

    '        ''Creamos un adaptador de datos
    '        'adapter = New SqlDataAdapter(memComando)

    '        ''Vaciamos y volcamos datos
    '        'Dim tabla_cp As New DataTable

    '        ''Llenamos de datos
    '        'adapter.Fill(tabla_cp)

    '        ''Liberamos recursos
    '        'adapter.Dispose()
    '        'memComando.Dispose()

    '        ''Añado un campo para el orden
    '        'tabla_bbdd.Columns.Add("BBDD_CP")

    '        ''Creo un Order
    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    'Busco la comparativa
    '        '    tabla_bbdd.Rows(x)(2) = obtener_CP(tabla_bbdd.Rows(x)(0), tabla_cp).ToString

    '        'Next

    '        ''Tabla filtrada
    '        'Dim tabla_filtrada As New DataTable
    '        'tabla_filtrada.Columns.Add("name")
    '        'tabla_filtrada.Columns.Add("id_orden", System.Type.GetType("System.Int32"))
    '        'tabla_filtrada.Columns.Add("BBDD_CP")

    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    If tabla_bbdd.Rows(x)(0).ToString <> tabla_bbdd.Rows(x)(2).ToString Then

    '        '        'Inserto los datos en la tabla temporal
    '        '        Dim Renglon As DataRow = tabla_filtrada.NewRow()
    '        '        Renglon("name") = tabla_bbdd.Rows(x)(0).ToString
    '        '        Renglon("id_orden") = tabla_bbdd.Rows(x)(1)
    '        '        Renglon("BBDD_CP") = tabla_bbdd.Rows(x)(2).ToString

    '        '        'Inserto
    '        '        tabla_filtrada.Rows.Add(Renglon)

    '        '    End If

    '        'Next

    '        ''asigno tabla
    '        'gridview_bbdd.DataSource = tabla_filtrada
    '        'gridview_bbdd.DataBind()

    '        '    'Cerramos
    '        '    adapter.Dispose()
    '        '    memComando.Dispose()
    '        '    SqlConnection.ClearPool(memConn)

    '        '    'Limpiamos
    '        '    tabla_bbdd.Dispose()
    '        '    'tabla_cp.Dispose()
    '        '    'tabla_filtrada.Dispose()

    '        'End Using

    '        'Ataco a la conexion del programa
    '        'Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan"
    '        'Dim memConn As New SqlConnection(ruta_base)
    '        'memConn.Open()
    '        'Dim memComando As New SqlCommand

    '        'Creacion de Sentencia
    '        'Dim sentencia As String = Nothing
    '        'sentencia = "SELECT Id,ruta_base_datos FROM " &
    '        '"empresa ORDER BY Id;"
    '        'memComando.CommandText = sentencia
    '        'memComando.Connection = memConn

    '        'Dim memDatos As SqlDataReader
    '        'memDatos = memComando.ExecuteReader

    '        'Limpio el control
    '        'DDL_bbdd_destino.Items.Clear()
    '        'DDL_bbdd_destino.Items.Add(New System.Web.UI.WebControls.ListItem("", ""))

    '        'contador
    '        'Dim contador As Integer = 1

    '        'If memDatos.HasRows Then
    '        '    Do While memDatos.Read

    '        '        Cargo las BBDD
    '        '        DDL_bbdd_destino.Items.Add(New System.Web.UI.WebControls.ListItem(memDatos.Item("ruta_base_datos").ToString, memDatos.Item("id").ToString))

    '        '    Loop
    '        'End If

    '        'Cierro la base de datos
    '        'memDatos.Close()
    '        'memComando.Dispose()
    '        'memConn.Close()
    '        'memConn.Dispose()

    '        ''Nombre
    '        'Dim nombre As String = Server.MapPath("..\temp\a" + FileUpload_logo.FileName)
    '        'Dim nombre_nuevo As String = Server.MapPath("..\temp\Nuevo_" + FileUpload_logo.FileName)

    '        ''Subo el fichero de manera asincronica
    '        'FileUpload_logo.SaveAs(nombre)

    '        ''Elimino
    '        'If File.Exists(nombre_nuevo) Then
    '        '    File.Delete(nombre_nuevo)
    '        'End If

    '        ''Comienzo con la lectura del fichero
    '        'Dim lector_log As New System.IO.StreamReader(nombre)

    '        ''Leer el contenido mientras no se llegue al final
    '        'Dim contador As Integer = 0
    '        'Dim nueva_linea As String = Nothing

    '        'While lector_log.Peek() <> -1
    '        '    Dim linea As String = lector_log.ReadLine()
    '        '    If Not String.IsNullOrEmpty(linea) Then

    '        '        If contador = 0 Then
    '        '            grabar_fichero(nombre_nuevo, linea)
    '        '        Else

    '        '            Dim vector_campos() As String = linea.Split("|")
    '        '            nueva_linea = Nothing

    '        '            If vector_campos(29).ToString = "" Then

    '        '                nueva_linea = linea

    '        '            Else

    '        '                For x = 0 To 28
    '        '                    nueva_linea += vector_campos(x) & "|"
    '        '                Next

    '        '                If vector_campos(29).ToString.IndexOf("&") <> -1 Then

    '        '                    nueva_linea = linea

    '        '                Else

    '        '                    nueva_linea += vector_campos(29) & "&" & vector_campos(30) & "|" & vector_campos(31) & "|"

    '        '                End If

    '        '            End If

    '        '            'Grabar
    '        '            grabar_fichero(nombre_nuevo, nueva_linea)

    '        '        End If


    '        '        'If contador = 0 Then
    '        '        '    'Almaceno la primera linea(nombre de campos) en un vector
    '        '        '    Dim vector_campos() = Mid(linea, 1, linea.Count - 1).Split("|")
    '        '        '    'Comienzo a leer el vector y crear el insert
    '        '        '    sentencia_a = "INSERT INTO [" & tabla_tareas.Rows(0)(4) & "].[dbo].[" & nombre & "]  ("
    '        '        '    For x = 0 To vector_campos.GetUpperBound(0)
    '        '        '        sentencia_a += vector_campos(x) & ","
    '        '        '    Next
    '        '        '    sentencia_a = Mid(sentencia_a, 1, sentencia_a.Count - 1) & ") VALUES ("
    '        '        'Else
    '        '        '    'Almaceno la primera linea(nombre de campos) en un vector
    '        '        '    Dim vector_campos_valores() = Mid(linea, 1, linea.Count - 1).Split("|")

    '        '        '    'Comienzo a leer el vector y crear el insert
    '        '        '    sentencia_b = Nothing
    '        '        '    For x = 0 To vector_campos_valores.GetUpperBound(0)
    '        '        '        sentencia_b += "'" & vector_campos_valores(x).Replace(",", ".").Replace("'", " ") & "'" & ","
    '        '        '    Next
    '        '        '    'Insert final
    '        '        '    sentencia = sentencia_a + Mid(sentencia_b, 1, sentencia_b.Count - 1) & ");"

    '        '        '    'Inserto los nuevos valores
    '        '        '    'Comienzo la grabacion de datos
    '        '        '    Dim memComando As New Data.SqlClient.SqlCommand

    '        '        '    'Inserto
    '        '        '    memComando.CommandText = sentencia
    '        '        '    memComando.Connection = memConnSQL
    '        '        '    memComando.ExecuteNonQuery()
    '        '        '    memComando.Dispose()

    '        '        'End If
    '        '        contador += 1

    '        '    End If

    '        'End While

    '        '' Cerrar el fichero
    '        'lector_log.Close()

    '        'Response.Write(Server.MapPath("..\temp"))
    '        ''Asigno el nuevo nombre y subo el ZIP de manera temporal
    '        'txt_nombre_importacion.Text = "Importar_Tinfor_" & parametros_usuario(0) & "_" & Mid(Date.Now, 1, 2) & Mid(Date.Now, 4, 2) & Mid(Date.Now, 7, 4) & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second

    '        ''Creo la carpeta para la descompresion
    '        'If Not System.IO.Directory.Exists("D:\images_usuarios\Temp\" & txt_nombre_importacion.Text) Then
    '        '    System.IO.Directory.CreateDirectory("D:\images_usuarios\Temp\" & txt_nombre_importacion.Text)
    '        'End If

    '        '''Subo el fichero de manera asincronica
    '        'FileUpload_logo.SaveAs(Server.MapPath("..\temp\" + FileUpload_logo.FileName))

    '        ''Asaigno valores
    '        'lbl_mensaje_advertencia.Text = "Todos los datos de esta empresa serán borrados, ¿Esta totalmente seguro de que desea comenzar el proceso de importación?"

    '        ''Registro como bloque en local para el jquery
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Aviso", "$('#deleteModal').modal('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_crea_logo_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_crea_logo_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub btn_ruta_gestion_documental_Click(sender As Object, e As EventArgs) Handles btn_ruta_gestion_documental.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Asigno
    '        Dim tabla_bbdd As New DataTable

    '        'Conecto
    '        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

    '            'Abrimos conexion
    '            memConn.Open()

    '            Dim memComando As New SqlCommand
    '            memComando.CommandText = "SELECT * FROM " &
    '            "ficheros;"
    '            memComando.Connection = memConn

    '            'Creamos un adaptador de datos
    '            Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '            'Llenamos de datos
    '            adapter.Fill(tabla_bbdd)

    '            'Recorremos
    '            For x = 0 To tabla_bbdd.Rows.Count - 1

    '                memComando.CommandText = "UPDATE ficheros SET ruta='" & tabla_bbdd.Rows(x)("ruta").ToString.Replace("abajo", parametros_empresa(5)) & "' WHERE nombre_fichero='" & tabla_bbdd.Rows(x)("nombre_fichero") & "' AND size=" & tabla_bbdd.Rows(x)("size") & " AND observaciones='" & tabla_bbdd.Rows(x)("observaciones") & "';"
    '                memComando.Connection = memConn
    '                memComando.ExecuteNonQuery()

    '            Next

    '            'Cerramos
    '            adapter.Dispose()
    '            memComando.Dispose()
    '            SqlConnection.ClearPool(memConn)

    '        End Using

    '        'Recorremos
    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        'Dim origen As String = "D:\images_usuarios\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\logo_empresa.jpg"

    '        'If System.IO.File.Exists(origen) Then

    '        '    Dim destino As String = "D:\imagenes_usuarios\" & tabla_bbdd.Rows(x)(1).ToString & "\logo\logo_empresa.jpg"
    '        '    My.Computer.FileSystem.CopyFile(origen, destino)

    '        'End If

    '        '    Dim destino_carpeta_logo As String = "D:\images_usuarios\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\"
    '        '    If Not System.IO.Directory.Exists(destino_carpeta_logo) Then
    '        '        System.IO.Directory.CreateDirectory(destino_carpeta_logo)
    '        '    End If

    '        '    'Copio los ficheros
    '        '    Dim origen_fichero As String = "D:\images_usuarios\" & tabla_bbdd.Rows(0)(0).ToString & "\logo\logo_empresa.jpg"
    '        '    Dim destino_fichero As String = "D:\images_usuarios\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\logo_empresa.jpg"
    '        '    If Not System.IO.File.Exists(destino_fichero) Then
    '        '        My.Computer.FileSystem.CopyFile(origen_fichero, destino_fichero, True)
    '        '    End If

    '        'Next




    '        'Dim ruta_origen As String = "D:\images_usuarios2\0\logo\logo_empresa.jpg"
    '        'Dim ruta_comprobar As String = "D:\images_usuarios2\" & tabla_bbdd.Rows(x)(0).ToString & "\logo\logo_empresa.jpg"

    '        'If System.IO.File.Exists(ruta_comprobar) Then

    '        '    'Obtengo las propiedades
    '        '    Dim info As New FileInfo(ruta_comprobar)

    '        '    If info.Length = 17912 Then

    '        '        'Copio la imagen nueva
    '        '        My.Computer.FileSystem.CopyFile(ruta_origen, ruta_comprobar, True)

    '        '    End If

    '        'End If



    '        ''Añado un campo para el orden
    '        'tabla_bbdd.Columns.Add("id_orden", System.Type.GetType("System.Int32"))

    '        ''Creo un Order
    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    'Response.Write(Mid(tabla_bbdd.Rows(x)(0).ToString, 2, tabla_bbdd.Rows(x)(0).ToString.IndexOf("O") - 1))
    '        '    tabla_bbdd.Rows(x)(1) = Mid(tabla_bbdd.Rows(x)(0).ToString, 2, tabla_bbdd.Rows(x)(0).ToString.IndexOf("O") - 1)

    '        'Next

    '        ''Creo un View
    '        'Dim dv As New DataView(tabla_bbdd)

    '        'Dim orden As String = Nothing
    '        'orden = "ASC"

    '        ''Ordeno
    '        'dv.Sort = "id_orden" & " " & orden

    '        ''Convierto la vista en una tabla
    '        'tabla_bbdd = dv.ToTable



    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    Response.Write(tabla_bbdd.Rows(x)(0).ToString & "::" & tabla_bbdd.Rows(x)(1).ToString & "<br>")
    '        'Next








    '        ''Asigno los datos de la bbdd de CP
    '        'memComando.CommandText = "SELECT ruta_base_datos " &
    '        '    "FROM [inforplan].[dbo].[empresa] " &
    '        '    "ORDER BY id;"
    '        'memComando.Connection = memConn

    '        ''Creamos un adaptador de datos
    '        'adapter = New SqlDataAdapter(memComando)

    '        ''Vaciamos y volcamos datos
    '        'Dim tabla_cp As New DataTable

    '        ''Llenamos de datos
    '        'adapter.Fill(tabla_cp)

    '        ''Liberamos recursos
    '        'adapter.Dispose()
    '        'memComando.Dispose()

    '        ''Añado un campo para el orden
    '        'tabla_bbdd.Columns.Add("BBDD_CP")

    '        ''Creo un Order
    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    'Busco la comparativa
    '        '    tabla_bbdd.Rows(x)(2) = obtener_CP(tabla_bbdd.Rows(x)(0), tabla_cp).ToString

    '        'Next

    '        ''Tabla filtrada
    '        'Dim tabla_filtrada As New DataTable
    '        'tabla_filtrada.Columns.Add("name")
    '        'tabla_filtrada.Columns.Add("id_orden", System.Type.GetType("System.Int32"))
    '        'tabla_filtrada.Columns.Add("BBDD_CP")

    '        'For x = 0 To tabla_bbdd.Rows.Count - 1

    '        '    If tabla_bbdd.Rows(x)(0).ToString <> tabla_bbdd.Rows(x)(2).ToString Then

    '        '        'Inserto los datos en la tabla temporal
    '        '        Dim Renglon As DataRow = tabla_filtrada.NewRow()
    '        '        Renglon("name") = tabla_bbdd.Rows(x)(0).ToString
    '        '        Renglon("id_orden") = tabla_bbdd.Rows(x)(1)
    '        '        Renglon("BBDD_CP") = tabla_bbdd.Rows(x)(2).ToString

    '        '        'Inserto
    '        '        tabla_filtrada.Rows.Add(Renglon)

    '        '    End If

    '        'Next

    '        ''asigno tabla
    '        'gridview_bbdd.DataSource = tabla_filtrada
    '        'gridview_bbdd.DataBind()

    '        '    'Cerramos
    '        '    adapter.Dispose()
    '        '    memComando.Dispose()
    '        '    SqlConnection.ClearPool(memConn)

    '        '    'Limpiamos
    '        '    tabla_bbdd.Dispose()
    '        '    'tabla_cp.Dispose()
    '        '    'tabla_filtrada.Dispose()

    '        'End Using

    '        'Ataco a la conexion del programa
    '        'Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan"
    '        'Dim memConn As New SqlConnection(ruta_base)
    '        'memConn.Open()
    '        'Dim memComando As New SqlCommand

    '        'Creacion de Sentencia
    '        'Dim sentencia As String = Nothing
    '        'sentencia = "SELECT Id,ruta_base_datos FROM " &
    '        '"empresa ORDER BY Id;"
    '        'memComando.CommandText = sentencia
    '        'memComando.Connection = memConn

    '        'Dim memDatos As SqlDataReader
    '        'memDatos = memComando.ExecuteReader

    '        'Limpio el control
    '        'DDL_bbdd_destino.Items.Clear()
    '        'DDL_bbdd_destino.Items.Add(New System.Web.UI.WebControls.ListItem("", ""))

    '        'contador
    '        'Dim contador As Integer = 1

    '        'If memDatos.HasRows Then
    '        '    Do While memDatos.Read

    '        '        Cargo las BBDD
    '        '        DDL_bbdd_destino.Items.Add(New System.Web.UI.WebControls.ListItem(memDatos.Item("ruta_base_datos").ToString, memDatos.Item("id").ToString))

    '        '    Loop
    '        'End If

    '        'Cierro la base de datos
    '        'memDatos.Close()
    '        'memComando.Dispose()
    '        'memConn.Close()
    '        'memConn.Dispose()











    '        ''Nombre
    '        'Dim nombre As String = Server.MapPath("..\temp\a" + FileUpload_logo.FileName)
    '        'Dim nombre_nuevo As String = Server.MapPath("..\temp\Nuevo_" + FileUpload_logo.FileName)

    '        ''Subo el fichero de manera asincronica
    '        'FileUpload_logo.SaveAs(nombre)

    '        ''Elimino
    '        'If File.Exists(nombre_nuevo) Then
    '        '    File.Delete(nombre_nuevo)
    '        'End If

    '        ''Comienzo con la lectura del fichero
    '        'Dim lector_log As New System.IO.StreamReader(nombre)

    '        ''Leer el contenido mientras no se llegue al final
    '        'Dim contador As Integer = 0
    '        'Dim nueva_linea As String = Nothing

    '        'While lector_log.Peek() <> -1
    '        '    Dim linea As String = lector_log.ReadLine()
    '        '    If Not String.IsNullOrEmpty(linea) Then

    '        '        If contador = 0 Then
    '        '            grabar_fichero(nombre_nuevo, linea)
    '        '        Else

    '        '            Dim vector_campos() As String = linea.Split("|")
    '        '            nueva_linea = Nothing

    '        '            If vector_campos(29).ToString = "" Then

    '        '                nueva_linea = linea

    '        '            Else

    '        '                For x = 0 To 28
    '        '                    nueva_linea += vector_campos(x) & "|"
    '        '                Next

    '        '                If vector_campos(29).ToString.IndexOf("&") <> -1 Then

    '        '                    nueva_linea = linea

    '        '                Else

    '        '                    nueva_linea += vector_campos(29) & "&" & vector_campos(30) & "|" & vector_campos(31) & "|"

    '        '                End If

    '        '            End If

    '        '            'Grabar
    '        '            grabar_fichero(nombre_nuevo, nueva_linea)

    '        '        End If


    '        '        'If contador = 0 Then
    '        '        '    'Almaceno la primera linea(nombre de campos) en un vector
    '        '        '    Dim vector_campos() = Mid(linea, 1, linea.Count - 1).Split("|")
    '        '        '    'Comienzo a leer el vector y crear el insert
    '        '        '    sentencia_a = "INSERT INTO [" & tabla_tareas.Rows(0)(4) & "].[dbo].[" & nombre & "]  ("
    '        '        '    For x = 0 To vector_campos.GetUpperBound(0)
    '        '        '        sentencia_a += vector_campos(x) & ","
    '        '        '    Next
    '        '        '    sentencia_a = Mid(sentencia_a, 1, sentencia_a.Count - 1) & ") VALUES ("
    '        '        'Else
    '        '        '    'Almaceno la primera linea(nombre de campos) en un vector
    '        '        '    Dim vector_campos_valores() = Mid(linea, 1, linea.Count - 1).Split("|")

    '        '        '    'Comienzo a leer el vector y crear el insert
    '        '        '    sentencia_b = Nothing
    '        '        '    For x = 0 To vector_campos_valores.GetUpperBound(0)
    '        '        '        sentencia_b += "'" & vector_campos_valores(x).Replace(",", ".").Replace("'", " ") & "'" & ","
    '        '        '    Next
    '        '        '    'Insert final
    '        '        '    sentencia = sentencia_a + Mid(sentencia_b, 1, sentencia_b.Count - 1) & ");"

    '        '        '    'Inserto los nuevos valores
    '        '        '    'Comienzo la grabacion de datos
    '        '        '    Dim memComando As New Data.SqlClient.SqlCommand

    '        '        '    'Inserto
    '        '        '    memComando.CommandText = sentencia
    '        '        '    memComando.Connection = memConnSQL
    '        '        '    memComando.ExecuteNonQuery()
    '        '        '    memComando.Dispose()

    '        '        'End If
    '        '        contador += 1

    '        '    End If

    '        'End While

    '        '' Cerrar el fichero
    '        'lector_log.Close()












    '        'Response.Write(Server.MapPath("..\temp"))
    '        ''Asigno el nuevo nombre y subo el ZIP de manera temporal
    '        'txt_nombre_importacion.Text = "Importar_Tinfor_" & parametros_usuario(0) & "_" & Mid(Date.Now, 1, 2) & Mid(Date.Now, 4, 2) & Mid(Date.Now, 7, 4) & "-" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second

    '        ''Creo la carpeta para la descompresion
    '        'If Not System.IO.Directory.Exists("D:\images_usuarios\Temp\" & txt_nombre_importacion.Text) Then
    '        '    System.IO.Directory.CreateDirectory("D:\images_usuarios\Temp\" & txt_nombre_importacion.Text)
    '        'End If

    '        '''Subo el fichero de manera asincronica
    '        'FileUpload_logo.SaveAs(Server.MapPath("..\temp\" + FileUpload_logo.FileName))

    '        ''Asaigno valores
    '        'lbl_mensaje_advertencia.Text = "Todos los datos de esta empresa serán borrados, ¿Esta totalmente seguro de que desea comenzar el proceso de importación?"

    '        ''Registro como bloque en local para el jquery
    '        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Aviso", "$('#deleteModal').modal('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_ruta_gestion_documental_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_ruta_gestion_documental_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub DDL_leer_fecha_extraccion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_leer_fecha_extraccion.SelectedIndexChanged

    '    Try

    '        'If DDL_leer_fecha_extraccion.SelectedItem.ToString <> "" Then
    '        '    leer_fecha_extraccion(DDL_leer_fecha_extraccion.SelectedItem.Text)
    '        'End If

    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "posicion", "$('.nav-tabs a[href=" + """#tab8default""" + "]').tab('show');", True)

    '    Catch ex As Exception
    '        'Cargo las variables de Usuario
    '        Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "DDL_leer_fecha_extraccion_SelectedIndexChanged", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error DDL_leer_fecha_extraccion_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Sub leer_fecha_extraccion(ByVal ejercicio As Integer)

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Calculo la fecha inicial y final
    '        txt_fecha_inicial.Text = funciones_globales.fecha_inicial(ejercicio, parametros_empresa(12))
    '        txt_fecha_final.Text = funciones_globales.fecha_final(parametros_empresa(12), ejercicio, parametros_empresa(12))

    '        'Asigno valor a los label
    '        lbl_fechas_extraccion_pdf.Text = "(Periodo del: " & txt_fecha_inicial.Text & " al: " & txt_fecha_final.Text & ")"

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_fecha_extraccion", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_fecha_extraccion: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

    'Protected Sub btn_pdf_Click(sender As Object, e As EventArgs) Handles btn_pdf.Click

    '    'Cargo las variables de Usuario y Empresa
    '    Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
    '    Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

    '    Try

    '        'Tiempo Inicio del Proceso
    '        Dim date1 As Date = Date.Now

    '        'Ataco a la conexion del programa
    '        Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" + parametros_empresa(5)
    '        Dim memConn As New SqlConnection(ruta_base)
    '        Dim memComando As New SqlCommand

    '        memComando.CommandText = "SELECT COUNT(*) AS contador,nif,denominacion from cabecera_impuestos " &
    '            "WHERE fecha_realizacion BETWEEN '" & txt_fecha_inicial.Text & "' AND '" & txt_fecha_final.Text & "' " &
    '            "GROUP BY nif,denominacion " &
    '            "ORDER BY COUNT(*) DESC"
    '        memComando.Connection = memConn
    '        memConn.Open()

    '        'Creamos un adaptador de datos
    '        Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

    '        'Vaciamos y volcamos datos
    '        Dim tabla_bbdd As New DataTable

    '        'Llenamos de datos
    '        adapter.Fill(tabla_bbdd)

    '        'Liberamos recursos
    '        adapter.Dispose()
    '        memComando.Dispose()

    '        GV_pdf.DataSource = tabla_bbdd
    '        GV_pdf.DataBind()

    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Completada Correctamente.');$('.nav-tabs a[href=" + """#tab8default""" + "]').tab('show');", True)

    '    Catch ex As Exception
    '        'Registro Error
    '        funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_pdf_Click", ex.Message)
    '        'Registro como bloque en local para el jquery
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_pdf_Click: " & ex.Message.Replace("'", " ") & "');", True)
    '    End Try

    'End Sub

End Class