
Partial Class subir
    Inherits System.Web.UI.Page

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

        For Each f As String In Request.Files.AllKeys
            Dim file As HttpPostedFile = Request.Files(f)
            file.SaveAs("D:\imagenes_usuarios\Temp\" & file.FileName)
        Next

    End Sub
End Class
