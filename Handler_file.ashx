<%@ WebHandler Language="VB" Class="HandlerVB" %>

Imports System.IO
Public Class HandlerVB : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim funciones_globales As New funciones_globales

        For Each key As String In context.Request.Files
            Dim postedFile As HttpPostedFile = context.Request.Files(key)
            Dim parametro As String = context.Request.Params("param1").ToString.Replace("|", "\")
            Dim nombre As String = HttpUtility.UrlDecode(postedFile.FileName)

            If parametro.IndexOf("claudio") <> -1 Then

                'Obtengo la empresa
                Dim ruta As String = "D:\imagenes_usuarios\Temp\"
                Dim empresa As String = Mid(parametro, parametro.IndexOf("-") + 2)

                'Compruebo si la ruta existe
                If Not System.IO.Directory.Exists(ruta & empresa) Then

                    'Creo la carpeta contenedora
                    My.Computer.FileSystem.CreateDirectory(ruta & empresa)

                End If

                'Grabo
                If System.IO.File.Exists(ruta & empresa & "\" & nombre) Then

                    'Borro
                    System.IO.File.Delete(ruta & empresa & "\" & nombre)

                End If

                'Grabo
                postedFile.SaveAs(ruta & empresa & "\" & nombre)

            Else

                'Compruebo si la ruta existe
                If Not System.IO.Directory.Exists(parametro) Then

                    'Creo la carpeta contenedora
                    My.Computer.FileSystem.CreateDirectory(parametro)

                End If

                'Grabo el fichero
                postedFile.SaveAs(parametro & "\" & funciones_globales.URLtoText(nombre))

            End If

        Next

        context.Response.StatusCode = 200
        context.Response.ContentType = "text/plain"
        context.Response.Write("Success")
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class