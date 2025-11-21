Imports System.IO
Imports Ionic.Zip

Partial Class actualizaciones_visual_zip
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            If Request.Params("id") <> "" Then

                Dim ruta_destino As String = "D:\imagenes_usuarios\temp\" & Request.Params("id").ToString

                'Creo la carpeta para la descompresion
                If Not System.IO.Directory.Exists(ruta_destino.Replace(".zip", "\")) Then
                    System.IO.Directory.CreateDirectory(ruta_destino.Replace(".zip", "\"))
                End If

                'Descomprimo
                Using zip As ZipFile = ZipFile.Read(ruta_destino)
                    zip.ExtractAll(ruta_destino.Replace(".zip", "\"), ExtractExistingFileAction.OverwriteSilently)
                    zip.Dispose()
                End Using

                'Me creo una variable para extraer los atributos de los archivos
                Dim archivoInfo As FileInfo

                LT_ficheros.Text = "<table style='width:100%;'>" &
                   "<tr>" &
                   "<td colspan='3' style='border-bottom: 1px solid #B3B3B3; height:70px'>" &
                   "<span class='text-muted' style='font-size:20px;'>Nombre Fichero:</span>" &
                   "<span class='text-muted' style='font-size:20px;color: #28a745;'> " & Request.Params("id").ToString & "</span>" &
                   "</td>" &
                   "</tr>"

                For Each fichero As String In Directory.GetFiles(ruta_destino.Replace(".zip", "\"))

                    'Recupero las propiedades de los ficheros para poder ver el nombres.
                    archivoInfo = New FileInfo(fichero)

                    'Mensaje
                    LT_ficheros.Text += "<tr style='border-bottom: 1px solid #B3B3B3; height:50px;'>" &
                       "<td style='width:20px;'></td>" &
                       "<td style='width:40px;'>"

                    If Path.GetExtension(archivoInfo.Name.ToLower) = ".xlsx" Then

                        'Mensaje
                        LT_ficheros.Text += "<img src ='../Imagenes/web/xls_icono.png' style='width:30px;'/>"

                    Else

                        'Mensaje
                        LT_ficheros.Text += "<img src ='../Imagenes/web/Document-02-azul.png' style='width:30px;'/>"

                    End If

                    'Mensaje
                    LT_ficheros.Text += "</td>" &
                       "<td><span class='text-muted'><small>" & archivoInfo.Name & "</small></span></td>" &
                       "</tr>"

                Next

                'Mensaje
                LT_ficheros.Text += "</table>"

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
