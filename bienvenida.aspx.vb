Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization

Module Module_Bienvenida

    Public tabla_impuesto As New DataTable
    Public tabla_cuentas_plan As New DataTable

End Module

Partial Class bienvenida
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then

                'Carga de fondo
                Dim aleatorio As New Random
                Dim numero As Integer = aleatorio.Next(1, 10)
                cuerpo.Attributes.Add("style", "background:url('imagenes/fondo/fondo_aplicacion" & numero & ".jpg') no-repeat center center fixed;")

            End If

        Catch ex As Exception
            PH_avisos.Visible = True
            lblerror.Text = "Error Page_Load: " & ex.Message
        End Try

    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As EventArgs) Handles btn_actualizar.Click

        Try

            'Limpieza de errore
            lblerror.Text = Nothing

            '1º )Necesito acceder al último numero de BBDD----------------------------------------------------------------------------------------------------------
            'Ataco a la conexion del programa
            Dim ruta_base As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion"
            Dim memConn As New SqlConnection(ruta_base)
            memConn.Open()

            'Declaro
            Dim memComando As New SqlCommand

            'Ataco a la conexion del programa
            memComando.CommandText = "UPDATE empresa SET nombre_fiscal='" & txt_nombre_fiscal.Text & "'," &
            "nombre_comercial='" & txt_nombre_comercial.Text & "'," &
            "nif='" & txt_cif.Text.ToUpper & "'," &
            "licencia_usuario='20', " &
            "licencia_uso='1', " &
            "digitos_asientos='10', " &
            "bienvenida='0', " &
            "certificado_password='RDP57g7P' " &
            "WHERE id=" & Request.Params("cod_empresa") & ";"
            memComando.Connection = memConn
            memComando.ExecuteNonQuery()

            '2º )Necesito insertar el tipo de impuesto por defecto IGIC 7%--------------------------------------------
            memComando.CommandText = "INSERT INTO [" & Request.QueryString("bbdd") & "].[dbo].tipo_impuestos (Id,nombre,porcentaje,impuesto_defecto,activo) VALUES " &
                 "(1,'IGIC',7,1,1);"
            memComando.ExecuteNonQuery()

            '3º )Necesito insertar Nº contadores a 0 -----------------------------------------------------------------
            memComando.CommandText = "INSERT INTO [" & Request.QueryString("bbdd") & "].[dbo].contadores (n_factura,n_abono,n_presupuesto,n_albaran) VALUES " &
                 "(1,1,1,1);"
            memComando.ExecuteNonQuery()

            'Vacio buffer
            memComando.Dispose()
            memConn.Close()
            memConn.Dispose()

            'Debo grabar el logo por defecto 
            'Primero crar las carpetas
            'Asigno el nuevo nombre y subo la imagen
            Dim nombre As String = "logo_empresa.jpg"

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\logo\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\logo\")
            End If

            'Copio el log de cloud a la nueva empresa
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\logo\logo_empresa.jpg", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\logo\logo_empresa.jpg", True)

            'Creo la carpeta para el fondo de la factura
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\factura\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\factura\")
            End If

            'Creo el pie de la factura
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\factura\fondo_factura.png", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\factura\fondo_factura.png", True)
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\factura\pie_factura.png", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\factura\pie_factura.png", True)

            'Creo la carpeta para el fondo del albaran
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\albaran\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\albaran\")
            End If

            'Creo el pie del albaran
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\albaran\fondo_albaran.png", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\albaran\fondo_albaran.png", True)
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\albaran\pie_albaran.png", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\albaran\pie_albaran.png", True)

            'Creo la carpeta para el fondo del presupuesto
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\presupuesto\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\presupuesto\")
            End If

            'Creo el pie del presupuesto
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\presupuesto\fondo_presupuesto.png", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\presupuesto\fondo_presupuesto.png", True)
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\presupuesto\pie_presupuesto.png", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\presupuesto\pie_presupuesto.png", True)

            'Creo la carpeta para el certificado
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\certificado\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\certificado\")
            End If

            'Creo el certificado
            System.IO.File.Copy(Server.MapPath("") & "\imagenes\certificado\certificado.pfx", "D:\imagenes_usuarios_facturacion\" & Request.Params("bbdd") & "\certificado\certificado.pfx", True)

            'Activo la pantallla final
            PH_empezar.Visible = True
            PH_datos.Visible = False

        Catch ex As Exception
            PH_avisos.Visible = True
            lblerror.Text = "Error btn_actualizar_Click: " & ex.Message
        End Try

    End Sub

    Protected Sub btn_iniciar_Click(sender As Object, e As EventArgs) Handles btn_iniciar.Click

        Try

            'Redirijo a login
            Response.Redirect("login.aspx")

        Catch ex As Exception
            PH_avisos.Visible = True
            lblerror.Text = "Error btn_login_Click: " & ex.Message
        End Try

    End Sub

End Class
