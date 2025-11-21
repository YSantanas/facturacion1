Imports System.Data.SqlClient
Imports Ionic.Zip
Imports System.IO
Imports System.Data

Partial Class herramientas_exportar
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Public Sub cargar_treeview_tablas()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT object_id, name " &
            "FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].sys.objects WHERE type='U' " &
            "ORDER BY name;")

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Asigno
                Dim theRootNode As TreeNode = New TreeNode
                theRootNode.Text = " " & tabla_consulta.Rows(x)("name")
                theRootNode.Value = tabla_consulta.Rows(x)("object_id")
                theRootNode.SelectAction = TreeNodeSelectAction.None
                TV_tablas.Nodes.Add(theRootNode)

            Next

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "cargar_treeview_tablas", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error cargar_treeview_tablas: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

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

            'Ver fondo
            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                ''Restricción de Usuarios
                'If parametros_usuario(10) = "Invitado" Or parametros_usuario(10) = "Usuario Restringido" Or parametros_usuario(10) = "Usuario" Then
                '    'Bloque Jquery
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                'End If

                'Carga las tablas de la BBDD
                cargar_treeview_tablas()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub CB_all_CheckedChanged(sender As Object, e As EventArgs) Handles CB_all.CheckedChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If CB_all.Checked = True Then
                For Each tv As TreeNode In TV_tablas.Nodes
                    tv.Checked = True
                Next
            Else
                For Each tv As TreeNode In TV_tablas.Nodes
                    tv.Checked = False
                Next
            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "CB_all_CheckedChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error CB_all_CheckedChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_exportar_Click(sender As Object, e As EventArgs) Handles btn_exportar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Restricción
            Dim contador_check As Integer = 0
            For Each tv As TreeNode In TV_tablas.Nodes
                If tv.Checked = True Then
                    contador_check += 1
                End If
            Next

            If contador_check = 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Debes seleccionar alguna tabla para exportar.');", True)
                Exit Sub
            End If

            'Exportar
            exportar()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_exportar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_exportar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub exportar()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Genero el nombre del fichero
            Dim nombre_carpeta As String = "Exportar_" & tabla_usuario.Rows(0)("Id") & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "")

            'Creo la carpeta que contendra el fichero de la exportacion
            If Not System.IO.Directory.Exists(Server.MapPath("..") & "\temp\" & nombre_carpeta) Then
                System.IO.Directory.CreateDirectory(Server.MapPath("..") & "\temp\" & nombre_carpeta)
            End If

            'Nombre de las tablas elegidas
            Dim nombre_tablas_elegidas As String = Nothing

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                'Comiendo con el recorrido para ver los check marcados
                For Each tv As TreeNode In TV_tablas.Nodes

                    'Si esta seleccionado
                    If tv.Checked = True Then

                        'Asigno
                        nombre_tablas_elegidas += tv.Text & ","

                        'Con el nombre de la tabla, necesito obtener las columnas que tiene
                        Dim memComando2 As New SqlCommand
                        memComando2.CommandText = " SELECT name " &
                        "FROM sys.columns WHERE " &
                        "object_id='" & tv.Value & "' "
                        memComando2.Connection = memConn
                        Dim memDatos2 As SqlDataReader
                        memDatos2 = memComando2.ExecuteReader

                        'Guarda los nombres de los campos
                        Dim nombre_campos As String = Nothing
                        Dim contador As Integer = 0
                        If memDatos2.HasRows Then
                            Do While memDatos2.Read
                                nombre_campos += memDatos2.Item("name").ToString & ","
                                contador += 1
                            Loop
                        End If

                        ' Cierro la base de datos
                        memDatos2.Close()
                        memComando2.Dispose()

                        'Vector para contener los nombres de los campos
                        Dim vector_nombre_campos(contador - 1) As String

                        'Elimino el residuo de la ultima ,
                        nombre_campos = Mid(nombre_campos, 1, nombre_campos.Count - 1)

                        'Cargo un vector con todos los datos
                        vector_nombre_campos = nombre_campos.Split(",")

                        Dim sentencia As String = "SELECT * FROM " & Trim(tv.Text) & ";"
                        'Ataco a la conexion del programa
                        Dim memComando As New SqlCommand
                        memComando.CommandText = sentencia
                        memComando.Connection = memConn
                        Dim memDatos As SqlDataReader
                        memDatos = memComando.ExecuteReader

                        'Genero el nombre del fichero
                        Dim nombre As String = Trim(tv.Text) & ".txt"

                        'Escribo el fichero
                        Dim oSW As New StreamWriter(Server.MapPath("..") & "\temp\" & nombre_carpeta & "\" & nombre, False)

                        'Inserto el nombre de los campos
                        Dim texto_titulo As String = Nothing
                        For x = 0 To vector_nombre_campos.GetUpperBound(0)
                            texto_titulo += (vector_nombre_campos(x)).ToString & "|"
                        Next
                        oSW.WriteLine(texto_titulo)

                        If memDatos.HasRows Then
                            Do While memDatos.Read
                                Dim texto As String = Nothing
                                For x = 0 To vector_nombre_campos.GetUpperBound(0)
                                    texto += memDatos.Item(vector_nombre_campos(x)).ToString.Replace(Chr(13), "").Replace(Chr(10), "") & "|"
                                Next
                                oSW.WriteLine(texto)
                            Loop
                        End If

                        ' Cierro la base de datos
                        memDatos.Close()
                        memComando.Dispose()

                        'Finalizo
                        oSW.Close()

                    End If

                Next

                'Cerramos
                SqlConnection.ClearPool(memConn)

            End Using

            'Ruta inicial para mirar los ficheros generados
            Dim ruta_inicial As String = Server.MapPath("..") & "\temp\" & nombre_carpeta

            'Me creo una variable para extraer los atributos de los archivos
            Dim archivoInfo As FileInfo

            'Comienzo la creacion del zip para el usuario
            Using zip As ZipFile = New ZipFile()

                'Recorro los valores del vector para saber los ficheros que hay dentro de cada carpeta
                For Each archivo In Directory.GetFiles(ruta_inicial)
                    archivoInfo = New FileInfo(archivo)
                    zip.AddFile(ruta_inicial & "\" & archivoInfo.Name, "")
                    'archivoInfo.Delete()
                Next

                'Grabo el fichero
                zip.Save(Server.MapPath("..") & "\temp\" & tabla_empresa.Rows(0)("ruta_base_datos") & "_" & nombre_carpeta & ".zip")

            End Using

            'Elimino el directorio que se genero temporalmente y sus ficheros txt
            System.IO.Directory.Delete(Server.MapPath("..") & "\temp\" & nombre_carpeta, True)

            'Muestro el placeholder con la foto y la ruta
            PH_comprimido.Visible = True
            Dim fecha As String = DateTime.Today
            Dim hora As String = Now.ToString("HH:mm:ss")
            HL_enlace.NavigateUrl = "../temp/" & tabla_empresa.Rows(0)("ruta_base_datos") & "_" & nombre_carpeta & ".zip"
            lbl_etiqueta.Text = tabla_empresa.Rows(0)("ruta_base_datos") & "_" & nombre_carpeta & ".zip"

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Copia de Seguridad", "Exportó Correctamente la(s) siguiente(s) tabla(s):" & Mid(nombre_tablas_elegidas, 1, nombre_tablas_elegidas.Count - 1) & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Ficheros exportados correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "exportar", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error exportar: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
