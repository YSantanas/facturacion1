Imports System.Data.SqlClient
Imports RedsysAPIPrj

Partial Class propietario_respuesta_pago_tarjeta
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Fecha acceso y hora
        lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

        'Recibo los parametros
        Dim version As String = Request.QueryString("Ds_SignatureVersion")
        Dim params As String = Request.QueryString("Ds_MerchantParameters")
        Dim signatureRecibida As String = Request.QueryString("Ds_Signature")

        'LLamo a la API de Redsys
        Dim r As RedsysAPI = New RedsysAPI()

        'Genero los parametros para comparar
        Dim kc As String = "bmTqAqNu+d3juVzKd7bGP3IBe+3LpbAY"
        Dim signatureCalculada As String = r.createMerchantSignatureNotif(kc, params)

        'Excepción
        If signatureRecibida <> signatureCalculada Then

            'Grafico OK
            LT_imagen.Text = "<span class='material-icons' style='font-size:150px; color:#721c24;'>thumb_down_alt</span>"
            'Mensaje
            LT_mensaje.Text = "&nbsp;&nbsp;<span style='font-size:35px;color:#721c24;'>Firma inválida.</span>"

        End If

        'Exepcion
        If Request.Params("dat") = "OK" Then

            'Grafico OK
            LT_imagen.Text = "<span class='material-icons' style='font-size:150px; color:#155724;'>thumb_up</span>"
            'Mensaje
            LT_mensaje.Text = "&nbsp;&nbsp;<span style='font-size:35px;color:#155724;'>Pago realizado correctamente.</span>"

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Cargo los datos
                Dim vector() As String = Request.Params("n_factura").ToString.Split("*")

                memComando.CommandText = "UPDATE facturacion SET pagada=1, metodo_pago='Tarjeta', sysred='" & vector(1) & "' WHERE n_factura='" & vector(0) & "';"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

        Else

            'Grafico OK
            LT_imagen.Text = "<span class='material-icons' style='font-size:150px; color:#721c24;'>thumb_down_alt</span>"
            'Mensaje
            LT_mensaje.Text = "&nbsp;&nbsp;<span style='font-size:35px;color:#721c24;'>Pago NO realizado.</span>"

        End If

    End Sub

End Class
