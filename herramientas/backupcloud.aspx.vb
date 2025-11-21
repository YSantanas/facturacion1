Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO

Partial Class supervisor_backupcloud
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

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

                'BackupCloud
                DDL_dias_backup.SelectedIndex = DDL_dias_backup.Items.IndexOf(DDL_dias_backup.Items.FindByValue(tabla_empresa.Rows(0)("dias_backupcloud")))

                ''Restricción de Usuarios
                'If parametros_usuario(10) = "Invitado" Or parametros_usuario(10) = "Usuario Restringido" Then
                '    'Bloque Jquery
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                '    Calendar1.Enabled = False
                '    Exit Sub
                'End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Calendar1_SelectionChanged(sender As Object, e As EventArgs) Handles Calendar1.SelectionChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Carga los datos de la copia si la hay
            cargar_listado_copias(Calendar1.SelectedDate)

            'Activo para realizar copias
            Dim fecha_elegida As Date = Calendar1.SelectedDate
            Dim fecha_hoy As Date = Now.Date
            If fecha_elegida = fecha_hoy Then
                btn_agregar.Visible = True
            Else
                btn_agregar.Visible = False
            End If


        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Calendar1_SelectionChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Calendar1_SelectionChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Public Sub cargar_listado_copias(ByVal fecha As Date)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Ubico donde esta los bak
            Dim ruta As String = "D:\bak_sql_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\" & Month(fecha) & "_" & Year(fecha) & "\" & Day(fecha) & "\"

            'Recorro
            If System.IO.Directory.Exists(ruta) = True Then

                'Variables
                Dim tabla_consulta As New DataTable
                tabla_consulta.Columns.Add("fecha")
                tabla_consulta.Columns.Add("hora")
                tabla_consulta.Columns.Add("descripcion")
                tabla_consulta.Columns.Add("nombre_fichero")
                tabla_consulta.Columns.Add("ruta_completa_fichero")

                Dim fileNames_gestion = My.Computer.FileSystem.GetFiles(ruta, FileIO.SearchOption.SearchTopLevelOnly)
                For Each fileName As String In fileNames_gestion

                    'Propiedades de los ficheros
                    Dim propiedades_fichero As New FileInfo(fileName)

                    'Inserto los datos en la tabla temporal
                    Dim Renglon As DataRow = tabla_consulta.NewRow()
                    Renglon("fecha") = Mid(File.GetCreationTime(fileName), 1, 10)
                    Renglon("hora") = Mid(File.GetCreationTime(fileName), 12)

                    'Variable
                    Dim vector() As String = Path.GetFileNameWithoutExtension(propiedades_fichero.Name).Split("$")
                    Renglon("descripcion") = vector(3).ToUpper
                    Renglon("nombre_fichero") = propiedades_fichero.Name
                    Renglon("ruta_completa_fichero") = propiedades_fichero.FullName
                    tabla_consulta.Rows.Add(Renglon)

                Next

                If tabla_consulta.Rows.Count = 0 Then
                    'Asigno
                    gridview_consulta.DataSource = Nothing
                    gridview_consulta.DataBind()
                Else
                    'Asigno
                    gridview_consulta.DataSource = tabla_consulta
                    gridview_consulta.DataBind()
                End If

            Else

                'Asigno
                gridview_consulta.DataSource = Nothing
                gridview_consulta.DataBind()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "cargar_listado_copias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error cargar_listado_copias: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recorro el GV
            If e.Row.RowType = DataControlRowType.DataRow Then

                'Desconecto el boton de eliminar si es copia creada por Claudio
                Dim btndelete As Button = DirectCast(e.Row.FindControl("btndelete"), Button)
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("descripcion").ToString() = "GENERADA POR IA" Then
                    btndelete.Visible = False
                End If

                'Desconecto el boton de eliminar si es una copia de pre-restauracion
                If gridview_consulta.DataKeys(e.Row.RowIndex).Item("descripcion").ToString().IndexOf("COPIA PRE-RESTAURACIÓN") <> -1 Then
                    If CDate(gridview_consulta.DataKeys(e.Row.RowIndex).Item("fecha")) = Now.Date Then
                        btndelete.Enabled = False
                        btndelete.ToolTip = "No puedes eliminar esta copia durante el día de hoy, ya que se ha generado automáticamente por seguridad, a partir de mañana ya podrás."
                    End If
                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "borrar") Then

                'Asignamos
                lt_mensaje_eliminar.Text = "¿Está seguro de eliminar la copia: <b>" & gridview_consulta.DataKeys(index).Item("descripcion").ToString() & " </b>con fecha: <b>" & gridview_consulta.DataKeys(index).Item("fecha").ToString() & "</b> a las: <b>" & gridview_consulta.DataKeys(index).Item("hora").ToString() & "</b> ?"
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

            End If

            If (e.CommandName = "restaurar") Then

                'Asignamos
                lt_mensaje_restaurar.Text = "¿Está seguro de querer restaurar la copia: <b>" & gridview_consulta.DataKeys(index).Item("descripcion").ToString() & " </b>con fecha: <b>" & gridview_consulta.DataKeys(index).Item("fecha").ToString() & "</b> a las: <b>" & gridview_consulta.DataKeys(index).Item("hora").ToString() & "</b> ?"
                txt_index.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_restaurar').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_si_eliminar_Click(sender As Object, e As EventArgs) Handles btn_si_eliminar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Delete Fichero
            My.Computer.FileSystem.DeleteFile(gridview_consulta.DataKeys(txt_index.Text).Item("ruta_completa_fichero").ToString())

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Backupcloud", "Eliminó la copia: " & gridview_consulta.DataKeys(txt_index.Text).Item("descripcion").ToString() & " con fecha: " & gridview_consulta.DataKeys(txt_index.Text).Item("fecha").ToString() & " a las " & gridview_consulta.DataKeys(txt_index.Text).Item("hora").ToString() & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Carga los datos de la copia si la hay
            cargar_listado_copias(Calendar1.SelectedDate)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "ok('Copia eliminada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_eliminar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_eliminar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_si_restaurar_Click(sender As Object, e As EventArgs) Handles btn_si_restaurar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Variables
            Dim fecha_seleccionada As DateTime = Now.Date

            'Realizo una copia al instante 
            crear_copia(fecha_seleccionada, "Copia Pre-Restauración (" & gridview_consulta.DataKeys(txt_index.Text).Item("fecha").ToString().Replace("/", "") & "-" & gridview_consulta.DataKeys(txt_index.Text).Item("hora").ToString().Replace(":", "") & ")")

            'Variables
            Dim ruta_origen As String = gridview_consulta.DataKeys(txt_index.Text).Item("ruta_completa_fichero").ToString()

            'Copiar el fichero de D: 101 al c: 100
            My.Computer.FileSystem.CopyFile(ruta_origen, "\\192.168.1.100\Temp_bak_sql\" & Path.GetFileName(ruta_origen))

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=Master")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "ALTER DATABASE [" & tabla_empresa.Rows(0)("ruta_base_datos") & "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " &
                    "RESTORE DATABASE [" & tabla_empresa.Rows(0)("ruta_base_datos") & "] FROM  DISK = N'C:\Temp_bak_sql\" & Path.GetFileName(ruta_origen) & "' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 5; " &
                    "ALTER DATABASE [" & tabla_empresa.Rows(0)("ruta_base_datos") & "] SET MULTI_USER"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Backupcloud", "Restauró la copia de seguridad del " & gridview_consulta.DataKeys(txt_index.Text).Item("fecha").ToString() & " de las " & gridview_consulta.DataKeys(txt_index.Text).Item("hora").ToString() & ".(Tiempo:  " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Insertamos una notificaion
            funciones_globales.grabar_notificacion(tabla_empresa.Rows(0)("ruta_base_datos"), "Backupcloud", "La Restauración se ha completado.", "Baja", tabla_usuario.Rows(0)("id"))

            'Cierro la session Correctamente
            funciones_globales.cerrar_sesion(tabla_usuario.Rows(0)("id"), tabla_empresa.Rows(0)("id"))

            'Delete Fichero
            'Copiar el fichero de D: 101 al c: 100
            My.Computer.FileSystem.DeleteFile("\\192.168.1.100\Temp_bak_sql\" & Path.GetFileName(ruta_origen))

            'Reinicio
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Reinicio", "window.top.location.href = '../login.aspx'; ", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_si_restaurar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_si_restaurar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As EventArgs) Handles btn_agregar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            txt_descripcion.Text = "Copia de Seguridad"

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_solicitar_datos').modal('show');setTimeout(function () { $('#txt_descripcion').focus(); }, 100);$('#txt_descripcion').select();"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_agregar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_agregar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_agregar_si_Click(sender As Object, e As EventArgs) Handles btn_agregar_si.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepciones
            If txt_descripcion.Text = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('El campo Descripción no puede estar vacío.');$('#modal_solicitar_datos').modal('show');setTimeout(function () { $('#txt_descripcion').focus(); }, 100);", True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Variables
            Dim fecha_seleccionada As DateTime = Now.Date

            'Crear Copia
            Dim vfecha_hora() As String = crear_copia(fecha_seleccionada, txt_descripcion.Text).Split("|")

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Backupcloud", "Generó una copia de seguridad del dia " & vfecha_hora(0) & " a las " & vfecha_hora(1) & ".(Tiempo:  " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Carga los datos de la copia si la hay
            cargar_listado_copias(Calendar1.SelectedDate)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion", "ok('Copia creada correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_agregar_si_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_agregar_si_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Function crear_copia(ByVal fecha As Date, ByVal descripcion As String) As String

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Declaro
        Dim fecha_creacion As String = fecha.ToShortDateString
        Dim hora_creacion As String = Now.ToLongTimeString

        'Creo las copias de seguridad
        Dim nombre As String = tabla_empresa.Rows(0)("ruta_base_datos") & "$" & fecha_creacion.Replace("/", "") & "$" & hora_creacion.Replace(":", "") & "$" & descripcion & ".bak"

        'Conecto
        Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=Master")

            'Abrimos conexion
            memConn.Open()

            Dim memComando As New SqlCommand
            memComando.CommandText = "BACKUP DATABASE " & tabla_empresa.Rows(0)("ruta_base_datos") & " To DISK = 'C:\Temp_bak_sql\" & nombre & "'"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            'Cerramos
            memComando.Dispose()
            SqlConnection.ClearPool(memConn)

        End Using

        'Copiar del Temp SQL al 101
        Dim ruta_origen As String = "\\192.168.1.100\Temp_bak_sql\" & nombre
        Dim ruta_destino As String = "D:\bak_sql_facturacion\" & tabla_empresa.Rows(0)("ruta_base_datos") & "\" & Month(fecha) & "_" & Year(fecha) & "\" & Microsoft.VisualBasic.DateAndTime.Day(fecha) & "\"

        'Creo la carpeta destino
        If Not System.IO.Directory.Exists(ruta_destino) Then
            System.IO.Directory.CreateDirectory(ruta_destino)
        End If

        'Copio los ficheros
        If System.IO.File.Exists(ruta_origen) Then
            My.Computer.FileSystem.CopyFile(ruta_origen, ruta_destino & nombre, True)
        End If

        'Delete Fichero
        My.Computer.FileSystem.DeleteFile(ruta_origen)

        'Devuelvo
        Return fecha_creacion & "|" & hora_creacion

    End Function

    Protected Sub DDL_dias_backup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_dias_backup.SelectedIndexChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand("UPDATE empresa SET dias_backupcloud='" & DDL_dias_backup.SelectedValue & "' WHERE id=" & tabla_empresa.Rows(0)("Id") & ";", memConn)
                memComando.ExecuteNonQuery()

                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Obtengo tabla_clientes para actualizar
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Backupcloud", "Cambió opciones en BackupCloud a " & DDL_dias_backup.SelectedValue & ".")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_agregar_si_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_agregar_si_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class