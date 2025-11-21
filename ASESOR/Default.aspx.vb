Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Ionic.Zip

Partial Class ASESOR_Default
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    '---------------------------------------------------------ASPX-----------------------------------------------------------------------
    Sub cargar_parametros_iniciales()

        'Obtengo las key por url
        Dim querystringSeguro As TSHAK.Components.SecureQueryString        'instanciamos el objeto y le pasamos como argumento el mismo array ‘de bits mas el parámetro data, que viene de la llamada de la ‘pagina default.aspx que contiene todo el queryString
        querystringSeguro = New TSHAK.Components.SecureQueryString(New Byte() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8}, Request.QueryString("key").ToString)

        'Obtengo datos de empresa
        HttpContext.Current.Session("f_" & querystringSeguro("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("Select * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & querystringSeguro("id_empresa") & ";")

        'Guardo en local
        txt_id_empresa.Text = querystringSeguro("id_empresa")

        'Obtengo datos de usuario
        HttpContext.Current.Session("f_" & querystringSeguro("id_usuario") & "_tabla_usuario") = funciones_globales.obtener_datos("Select * FROM [kernel_facturacion].[dbo].usuarios WHERE usuarios.id=" & querystringSeguro("id_usuario") & ";")

        'Guardo en local
        txt_id_usuario.Text = querystringSeguro("id_usuario")

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & txt_id_usuario.Text & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & txt_id_empresa.Text & "_tabla_empresa")

        'Eliminación de ficheros temporales
        'Ubico donde esta la carpeta Temp para eliminar ficheros
        Dim ruta_temporal As String = HttpContext.Current.Server.MapPath("..") & "\temp\"
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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then

                'Actualizo la imagen
                Dim numero_aleatorio As New Random

                'Asigno imagen de fondo
                body_contenedor.Attributes.Add("style", "background: url('../imagenes/fondo/fondo_aplicacion" & numero_aleatorio.Next(1, 13).ToString() & ".jpg') no-repeat center center fixed;")

                idesktop.Attributes.Add("src", "facturas_emitidas.aspx?id_usuario=" & tabla_usuario.Rows(0)("Id") & "&id_empresa=" & tabla_empresa.Rows(0)("Id"))

            End If

        Catch ex As Exception
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
