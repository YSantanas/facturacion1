Imports System.Data.SqlClient
Imports RedsysAPIPrj

Partial Class propietario_pago_tarjeta
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

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Asigno
                Dim vector() As String = Request.Params("id").Split("*")
                lbl_fecha.Text = vector(0)
                lbl_factura.Text = vector(1)
                lbl_importe.Text = vector(3) & " €"
                Dim nombre_pdf As String = vector(4)

                'Para el pago TVP virtual
                Dim version As String = "HMAC_SHA256_V1"
                Dim r As RedsysAPI = New RedsysAPI()
                Dim fuc = "097218705"
                Dim terminal = "001"
                Dim currency = "978"
                Dim trans = "0"
                Dim url = ""
                Dim numeroaleatorio As New Random
                Dim envio_numero_final As String = numeroaleatorio.Next(100, 999999999).ToString()
                'Dim urlOK = "http://85.117.245.147:81/io/contabilidad/propietario/respuesta_pago_tarjeta.aspx?dat=OK&n_factura=" & vector(1) & "*" & envio_numero_final
                'Dim urlKO = "http://85.117.245.147:81/io/contabilidad/propietario/respuesta_pago_tarjeta.aspx?dat=KO&n_factura=" & vector(1) & "*" & envio_numero_final

                Dim urlOK = "https://www.iocloudcomputing.io/contabilidad/propietario/respuesta_pago_tarjeta.aspx?dat=OK&n_factura=" & vector(1) & "*" & envio_numero_final
                Dim urlKO = "https://www.iocloudcomputing.io/contabilidad/propietario/respuesta_pago_tarjeta.aspx?dat=KO&n_factura=" & vector(1) & "*" & envio_numero_final

                'Actualizo la imagen
                Dim id = envio_numero_final 'Numero de la factura '+
                Dim amount = vector(3).ToString.Replace(",", "") 'Cantidad a pagar
                Dim kc = "bmTqAqNu+d3juVzKd7bGP3IBe+3LpbAY"

                'Preparamos Variables
                r.SetParameter("DS_MERCHANT_AMOUNT", amount)
                r.SetParameter("DS_MERCHANT_ORDER", id)
                r.SetParameter("DS_MERCHANT_MERCHANTCODE", fuc)
                r.SetParameter("DS_MERCHANT_CURRENCY", currency)
                r.SetParameter("DS_MERCHANT_TRANSACTIONTYPE", trans)
                r.SetParameter("DS_MERCHANT_TERMINAL", terminal)
                r.SetParameter("DS_MERCHANT_MERCHANTURL", url)
                r.SetParameter("DS_MERCHANT_URLOK", urlOK)
                r.SetParameter("DS_MERCHANT_URLKO", urlKO)

                Ds_SignatureVersion.Text = version

                Dim parms As String = r.createMerchantParameters()
                Ds_MerchantParameters.Text = parms

                Dim sig As String = r.createMerchantSignature(kc)
                Ds_Signature.Text = sig

                'Actualizo el id generado de la operacion de redsys
                'Conecto
                Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=inforplan")

                    'Abrimos conexion
                    memConn.Open()

                    Dim memComando As New SqlCommand
                    memComando.CommandText = "UPDATE facturacion SET sysred='" & envio_numero_final & "' WHERE n_factura='" & vector(1) & "';"
                    memComando.Connection = memConn
                    memComando.ExecuteNonQuery()

                    'Cierro la base de datos
                    memComando.Dispose()
                    SqlConnection.ClearPool(memConn)

                End Using

                'Paso la ruta al iframe
                ver_factura_embed.Visible = True
                ver_factura_embed.Attributes("src") = "..\temp\" & nombre_pdf

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class