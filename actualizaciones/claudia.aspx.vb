Imports System.Data
Imports System.Data.SqlClient

Partial Class actualizaciones_noticiario
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub eliminar_noticia(ByVal id As String)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            Dim id_multiples() As String = id.Split("|")

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()
                Dim memComando As New SqlCommand

                'Recorro las noticias
                For x = 0 To id_multiples.Count - 1

                    memComando.CommandText = "UPDATE tareas_IA SET realizado=1 WHERE id=" & id_multiples(x) & ";"
                    memComando.Connection = memConn
                    memComando.ExecuteNonQuery()

                Next

                'Libero recurso
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "eliminar_noticia", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error eliminar_noticia: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        'Si mando a eliminar
        If Request.Params("id") <> "" Then
            eliminar_noticia(Request.Params("id").ToString)
        End If

        'Asigno
        Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].tareas_IA WHERE realizado=0 And id_usuario=" & tabla_usuario.Rows(0)("Id") & " And id_empresa=" & tabla_empresa.Rows(0)("Id") & " ORDER BY id DESC;")

        'Contador para cerrar todas las notificaciones a la vez
        Dim contador As Integer = 0
        Dim multiple_id As String = Nothing
        Dim color As String = "#007bff"
        Dim clase As String = "cambiar_azul"

        If tabla_consulta.Rows.Count = 0 Then

            Lt_noticiario.Text = "<br><table style='width:100%;' border='0' ><tr>" &
               "<td width='94%' align='center'><span style='color: #6c757d; font-size:13px;'>Sin Tareas</span></td>" &
               "</tr></table>"

        Else

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                Lt_noticiario.Text += "<table Class='" & clase & "' style='width:100%; background-color:white; border: 1px solid #dddddd; border-spacing: 3px; border-collapse: separate;'><tr>" &
                    "<td style='width:3px;'>" &
                    "<td style='background-color: " & color & "; width:3px;'>" &
                    "<td style='font-size: 10px; color: #000000;'> Fecha: " & Mid(tabla_consulta.Rows(x)("fecha").ToString, 1, 10) & "</td>" &
                    "<td style='font-size: 10px; color: #000000;'> Hora: " & tabla_consulta.Rows(x)("hora").ToString & "</td>" &
                    "<td style='font-size: 10px; color: #000000;'> Tarea: " & tabla_consulta.Rows(x)("tarea").ToString & "</td>" &
                    "<td width='6%' align='center'><a href='claudia.aspx?id_usuario=" & Request.QueryString("id_usuario") & "&id_empresa=" & Request.QueryString("id_empresa") & "&id=" & tabla_consulta.Rows(x)("id").ToString & "'><span id='btn_cerrar_noticiario' class='bi bi-x' onmouseover='hand('btn_cerrar_noticiario')' title='Borrar' style='color:red;font-size:20px;'></span></a></td>" &
                    "</tr><table><table><tr><td style='height:8px;'></td></tr></table>"

                'Aumento contador
                contador += 1

                'Control para multiples id
                If contador = 1 Then
                    multiple_id = tabla_consulta.Rows(x)("id").ToString
                Else
                    multiple_id += "|" & tabla_consulta.Rows(x)("id").ToString
                End If

            Next

            'Control para que aparezca el boton de eliminar todo
            If contador > 1 Then

                Lt_noticiario.Text = "<table width='90%' style='border-spacing: 0px; border-collapse: separate;'><tr><td align='right' style='height:35px;'> " &
                    "<a href='claudia.aspx?id_usuario=" & Request.QueryString("id_usuario") & "&id_empresa=" & Request.QueryString("id_empresa") & "&id=" & multiple_id & "' style='text-decoration:none;'>&nbsp; <span class='bi bi-x' style='color: #dc3545;font-size:16px;'></span> <span style='color: #6c757d;font-size:13px;'> Eliminar Tareas</span></a>" &
                    "</td></tr></table> " & Lt_noticiario.Text

            End If

        End If

    End Sub

End Class
