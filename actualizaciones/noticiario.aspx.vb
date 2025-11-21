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

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & tabla_empresa.Rows(0)("ruta_base_datos"))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                If id = "ALL" Then

                    memComando.CommandText = "SELECT * FROM notificaciones WHERE activa='true' AND usuario = '" & tabla_usuario.Rows(0)("Id") & "' ORDER BY id DESC;"
                    memComando.Connection = memConn

                    'Creamos un adaptador de datos
                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

                    'Vaciamos y volcamos datos
                    Dim tabla_bbdd As New DataTable

                    'Llenamos de datos
                    adapter.Fill(tabla_bbdd)

                    'Liberamos recursos
                    adapter.Dispose()

                    'Recorro
                    Dim valores As String = Nothing
                    For x = 0 To tabla_bbdd.Rows.Count - 1
                        valores += "id=" & tabla_bbdd.Rows(x)(0).ToString & " OR "
                    Next

                    'Eliminar
                    tabla_bbdd.Dispose()

                    'Executo
                    memComando.CommandText = "UPDATE notificaciones SET " &
                   "activa='false' " &
                   "WHERE " & Mid(valores, 1, valores.Count - 4) & ";"
                    memComando.Connection = memConn
                    memComando.ExecuteNonQuery()

                Else

                    'Executo
                    memComando.CommandText = "UPDATE notificaciones SET " &
                   "activa='false' " &
                   "WHERE id=" & id & ";"
                    memComando.Connection = memConn
                    memComando.ExecuteNonQuery()

                End If

                'Libero recurso
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Refresco el número de noticias del escritorio
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Limpiar", "$(window.parent.document).find('#lbl_n_notificaciones').empty();", True)

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

        If Request.Params("id") <> "" Then
            eliminar_noticia(Request.Params("id").ToString)
        End If

        'Asigno
        Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [" & tabla_empresa.Rows(0)("ruta_base_datos") & "].[dbo].notificaciones WHERE activa='true' AND usuario = '" & tabla_usuario.Rows(0)("Id") & "' ORDER BY id DESC;")

        'Contador para cerrar todas las notificaciones a la vez
        Dim contador As Integer = 0
        Dim color As String = Nothing
        Dim clase As String = Nothing

        If tabla_consulta.Rows.Count = 0 Then

            Lt_noticiario.Text = "<br><table style='width:100%;' border='0' ><tr>" &
              "<td width='94%' align='center'><span style='color: #6c757d; font-size:13px;'>Sin Notificaciones</span></td>" &
              "</tr></table>"

        Else

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                Select Case tabla_consulta.Rows(x)("prioridad").ToString
                    Case "Alta" : color = "#dc3545" : clase = "cambiar_rojo"
                    Case "Normal" : color = "#ffc107" : clase = "cambiar_naranja"
                    Case "Baja" : color = "#28a745" : clase = "cambiar_verde"
                End Select

                Lt_noticiario.Text += "<table width='100%'><tr><td align='center'>" &
                    "<table width='90%' Class='" & clase & "' style='border-spacing: 0px; border-collapse: separate; border: 1px solid #dddddd;border-top: 1px solid " & color & ";'><tr><td>" &
                    "<table style='width:100%;' border='0' ><tr>" &
                        "<td width='94%' align='center'><span style='color: #000000; font-size:16px;'>" & tabla_consulta.Rows(x)("titulo").ToString & "</span></td>" &
                        "<td width='6%' align='center'><a href='noticiario.aspx?id_usuario=" & Request.QueryString("id_usuario") & "&id_empresa=" & Request.QueryString("id_empresa") & "&id=" & tabla_consulta.Rows(x)("id").ToString & "'><span class='bi bi-x' title='Borrar' style='color:red;font-size:20px;'></span></a></td>" &
                    "</tr></table>" &
                    "<table style='width:100%; border-spacing: 2px; border-collapse: separate;'><tr>" &
                    "<td style='background-color: " & color & "; width:3px;'>" &
                    "<td align='center'>" &
                        "<table style='width: 100%;'><tr>" &
                        "<tr>" &
                            "<td style='font-size: 12px; color: #000000;'> Fecha: " & Mid(tabla_consulta.Rows(x)("fecha").ToString, 1, 10) & "</td>" &
                            "<td style='font-size: 12px; color: #000000;'> Hora: " & Mid(tabla_consulta.Rows(x)("hora").ToString, 1, 5) & "</td>" &
                            "<td align='right'><span id='" & contador & "' class='bi bi-box-arrow-down desplegable' title='Mostrar Detalles' style='color: 000000;font-size:18px;'></span></td>" &
                        "</tr>" &
                        "<tr>" &
                            "<td colspan='3'><div class='oculto' id='capa" & contador & "'><br><span style='font-size: 13px; color: #000000'>" & tabla_consulta.Rows(x)("texto").ToString & "</span></div></td>" &
                        "</tr></table>" &
                    "</td>" &
                    "</tr></table>" &
                 "</td></tr></table>" &
                 "</td></tr></table><table><tr><td style='height:4px;'></td></tr></table>"

                'Aumento contador
                contador += 1

            Next

            'Control para que aparezca el boton de eliminar todo
            If contador > 1 Then

                Lt_noticiario.Text = "<table width='90%' style='border-spacing: 0px; border-collapse: separate;' border='0'><tr><td align='right'> " &
                    "<a href='noticiario.aspx?id_usuario=" & Request.QueryString("id_usuario") & "&id_empresa=" & Request.QueryString("id_empresa") & "&id=ALL' style='text-decoration:none;'>&nbsp; <span class='bi bi-x' style='color: #dc3545;font-size:16px;'></span> <span style='color: #6c757d;font-size:13px;'>Eliminar Notificaciones</span></a><p></p>" &
                    "</td></tr></table> " & Lt_noticiario.Text

                If contador > 7 Then
                    Dim mensaje_final As String = "window.parent.hablar('Tienes varias notificaciones sin cerrar, recuerda que puedes pulsar sobre Borrar Todas en la parte superior derecha y eliminarlas todas a la vez.');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Aviso", mensaje_final, True)
                End If

            End If

        End If

    End Sub

End Class