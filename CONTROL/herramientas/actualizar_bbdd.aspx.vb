
Imports System.Data
Imports System.Data.SqlClient

Partial Class CONTROL_herramientas_actualizar_bbdd
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_historico()

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[control_version_sql] ORDER BY fecha DESC,id DESC;")

            'Asigno
            gridview_consulta.DataSource = tabla_consulta
            gridview_consulta.DataBind()

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception

            'Asigno
            lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            'Control de Seguridad
            If Session("nombre_master") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            'Ver fondo
            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Leer historico
                leer_historico()

            End If

        Catch ex As Exception

            'Asigno
            lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                'Un tooltip nuevo
                e.Row.ToolTip = gridview_consulta.DataKeys(e.Row.RowIndex).Item("texto").ToString()

            End If

        Catch ex As Exception

            'Asigno
            lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub gridview_consulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridview_consulta.PageIndexChanging

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Actualizar posicion y cosnulta
            gridview_consulta.PageIndex = e.NewPageIndex
            leer_historico()

        Catch ex As Exception

            'Asigno
            lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub btn_anadir_actualizacion_Click(sender As Object, e As EventArgs) Handles btn_anadir_actualizacion.Click

        Try

            'Asigno valores
            LT_mensaje.Text = "¿Esta totalmente seguro de que desea ejecutar este SQL?"

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_eliminar').modal('show');"), True)

        Catch ex As Exception

            'Asigno
            lt_error.Text = ex.Message

        End Try

    End Sub

    Protected Sub btn_si_Click(sender As Object, e As EventArgs) Handles btn_si.Click

        Try

            'Asigno
            lt_error.Text = Nothing
            Dim tabla_consulta As New DataTable

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=master")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "SELECT name " &
                "FROM sys.databases " &
                "WHERE database_id >4 " &
                "ORDER BY sys.databases.database_id;"
                memComando.Connection = memConn

                'Creamos un adaptador de datos
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(memComando)

                'Llenamos de datos
                adapter.Fill(tabla_consulta)

                'Cerramos
                adapter.Dispose()
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Restricción solo para las tablas de Facturacion
                If Mid(tabla_consulta.Rows(x)("name").ToString, 1, 2) = "OP" And tabla_consulta.Rows(x)("name").ToString.IndexOf("F") <> -1 Then


                    '1º) Cargo con los nuevos datos ----------------------------------------------------------------------------------------------------------
                    Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=master")

                    Try

                        'Abro la conexion
                        conn.Open()

                        Dim script As String = txt_texto.Text
                        'remplazo
                        script = "USE " & tabla_consulta.Rows(x)(0) & vbCrLf + "GO" + vbCrLf & script

                        Dim commandStrings As IEnumerable(Of String) = Regex.Split(script, "^\s*GO\s*$", RegexOptions.Multiline Or RegexOptions.IgnoreCase)
                        For Each commandString As String In commandStrings
                            If commandString.Trim() <> "" Then
                                Dim meme As New SqlCommand(commandString, conn)
                                meme.ExecuteNonQuery()
                            End If
                        Next

                    Catch er As SqlException

                        'Asigno
                        lt_error.Text = tabla_consulta.Rows(x)(0) & ": " & er.Message & "<br>"

                    Finally
                        conn.Close()
                        SqlConnection.ClearPool(conn)
                    End Try

                End If

            Next

            'Inserto en la tabla Control Versión SQL
            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.CommandText = "INSERT INTO control_version_sql (fecha, hora, comentario,texto) VALUES " &
                    "('" & DateTime.Today & "','" & Now.ToString("HH:mm:ss") & "','" & txt_comentario.Text & "',@texto);"
                memComando.Parameters.Add("@texto", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@texto").Value = txt_texto.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Leer historico
            leer_historico()

            'Libero
            tabla_consulta.Dispose()

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Actualización Completada.');", True)

        Catch ex As Exception

            'Asigno
            lt_error.Text = ex.Message

        End Try

    End Sub

End Class
