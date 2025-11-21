Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Partial Class actualizaciones_tickets
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_tipos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[tickets] WHERE id_empresa='" & tabla_empresa.Rows(0)("Id") & "' ORDER BY id DESC;")

            'Asigno
            gridview_consulta.DataSource = tabla_consulta
            gridview_consulta.DataBind()

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Datos Usuario
            lbl_informacion.Text = gridview_consulta.PageCount & " Página(s) - Tiempo (" & tspan.ToString("s\:ff") & " segundos)"

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipos: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Public Sub leer_Utilidad()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT seccion FROM [kernel_facturacion].[dbo].IA_menu GROUP BY seccion ORDER BY seccion;")

            'Limpio el control
            DDL_utilidad.Items.Clear()
            Me.DDL_utilidad.Items.Add(New ListItem("", ""))

            For x = 0 To tabla_consulta.Rows.Count - 1
                Me.DDL_utilidad.Items.Add(New ListItem(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tabla_consulta.Rows(x)("seccion").ToString), tabla_consulta.Rows(x)("seccion").ToString))
            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_Utilidad", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_Utilidad: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_apartado(ByVal utilidad As String)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT modulo FROM [kernel_facturacion].[dbo].IA_menu WHERE seccion='" & utilidad & "' ORDER BY modulo;")

            'Asigno
            DDL_apartado.Items.Clear()

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tabla_consulta.Rows(x)("modulo").ToString), tabla_consulta.Rows(x)("modulo").ToString)
                Me.DDL_apartado.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_apartado", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_apartado: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            'Asigno el n_consulta
            gridview_consulta.PageSize = tabla_empresa.Rows(0)("n_paginado_consultas")

            'Ver fondo
            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                ''Restricción de Usuarios
                'If parametros_usuario(10) = "Invitado" Then
                '    'Bloque Jquery
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_seguridad", "$('#modal_sin_acceso').modal('show');", True)
                'End If

                'Leer Tipos
                leer_tipos()

                'Leer Utilidad
                leer_Utilidad()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                'Controlo si es visible o no
                Dim imagen As System.Web.UI.WebControls.LinkButton = DirectCast(e.Row.FindControl("LB_estado"), System.Web.UI.WebControls.LinkButton)
                If Not IsDBNull(gridview_consulta.DataKeys(e.Row.RowIndex).Values("respuesta")) Then

                    'Asigno
                    imagen.CssClass = "bi bi-check text-success"

                Else

                    'Asigno
                    imagen.CssClass = "bi bi-x text-danger"

                End If

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_DataBound(sender As Object, e As EventArgs) Handles gridview_consulta.DataBound

        Try

            'Control para menus de exportar
            If gridview_consulta.Rows.Count = 0 Then
                img_exportar_excel.Visible = False
                img_exportar_txt.Visible = False
                lbl_informacion.Visible = False
            Else
                img_exportar_excel.Visible = True
                img_exportar_txt.Visible = True
                lbl_informacion.Visible = True
            End If

        Catch ex As Exception
            'Obtengo
            Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
            Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_asiento_DataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_asiento_DataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_excel_Click(sender As Object, e As EventArgs) Handles img_exportar_excel.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_excel("Consulta de Tickets", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "xlsx"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_exportar_excel_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_exportar_excel_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_exportar_txt_Click(sender As Object, e As EventArgs) Handles img_exportar_txt.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Crear Excel
            Dim nombre As String = funciones_globales.crear_txt("Consulta de Tickets", gridview_consulta, tabla_usuario.Rows(0)("Id"))

            'Descargar Sin Ajax
            Dim ruta As String = Path.GetFileName(Server.MapPath("..") & "\temp\" + nombre).ToString()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "txt"
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" & ruta)
            HttpContext.Current.Response.TransmitFile(Server.MapPath("..") & "\temp\" + nombre)
            HttpContext.Current.Response.[End]()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_exportar_txt_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_exportar_txt_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_nueva_Click(sender As Object, e As EventArgs) Handles img_nueva.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            lbl_titulo.Text = "Agregar Ticket"
            btn_grabar.Visible = True
            btn_modificar.Visible = False
            DDL_utilidad.SelectedIndex = 0
            'Leer apartado desde inforplan
            leer_apartado(DDL_utilidad.SelectedValue)
            txt_telefono.Text = Nothing
            __txt_consulta.Text = Nothing

            'Declaro
            Dim muestro_aviso As Boolean = True
            Dim hora As Integer = Hour(Now)
            Dim dia_semana As String = Now.ToString("dddd")

            'Restricción para el horario
            If dia_semana = "lunes" Or dia_semana = "martes" Or dia_semana = "miercoles" Or dia_semana = "jueves" Then

                If (hora > 8 And hora < 13) Then
                    muestro_aviso = False
                End If

                If (hora > 15 And hora < 18) Then
                    muestro_aviso = False
                End If

                If muestro_aviso = True Then

                    'Asigno
                    LT_mensaje_horario.Text = "Recuerde que nuestro horario es de <b>9h a 13h</b> y de <b>16h a 18h</b>, pero en cuanto lleguemos le resolveremos su duda."

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_horario').modal('show');"), True)
                    Exit Sub

                End If

            End If

            If dia_semana = "viernes" Then

                If (hora > 7 And hora < 14) Then
                    muestro_aviso = False
                End If

                If muestro_aviso = True Then

                    'Asigno
                    LT_mensaje_horario.Text = "Recuerde que nuestro horario los viernes es de <b>8h a 14h</b>, pero en cuanto lleguemos le resolveremos su duda."

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_horario').modal('show');"), True)
                    Exit Sub
                End If

            End If

            If dia_semana = "sábado" Or dia_semana = "domingo" Then

                'Asigno
                LT_mensaje_horario.Text = "Recuerde que nuestro horario es de <b>Lunes a Jueves</b> de <b>9h a 13h</b> y de <b>16h a 18h</b> y los Viernes de <b>8h a 14h</b>, pero en cuanto lleguemos le resolveremos su duda."

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", funciones_globales.modal_register("$('#modal_horario').modal('show');"), True)
                Exit Sub

            End If

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "setTimeout(function () { $('#DDL_utilidad').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "img_nueva_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error img_nueva_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "ver_tickets") Then

                'Asigno
                lbl_titulo_visualizar.Text = "Ver Ticket"
                Me.DDL_utilidad_visualizar.Items.Clear()
                Me.DDL_utilidad_visualizar.Items.Add(New ListItem(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(gridview_consulta.DataKeys(index).Item("utilidad").ToString()), gridview_consulta.DataKeys(index).Item("utilidad").ToString()))
                Me.DDL_apartado_visualizar.Items.Clear()
                Me.DDL_apartado_visualizar.Items.Add(New ListItem(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(gridview_consulta.DataKeys(index).Item("apartado").ToString()), gridview_consulta.DataKeys(index).Item("apartado").ToString()))
                txt_telefono_visualizar.Text = gridview_consulta.DataKeys(index).Item("telefono").ToString()
                __LT_consulta.Text = gridview_consulta.DataKeys(index).Item("consulta").ToString()
                __LT_respuesta.Text = gridview_consulta.DataKeys(index).Item("respuesta").ToString()
                lbl_fecha_respuesta.Text = Mid(gridview_consulta.DataKeys(index).Item("fecha_respuesta").ToString(), 1, 10) & " - " & gridview_consulta.DataKeys(index).Item("hora_respuesta").ToString()

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#DDL_utilidad_visualizar').attr('disabled',true);$('#DDL_apartado_visualizar').attr('disabled',true);$('#txt_telefono_visualizar').attr('disabled',true);" & funciones_globales.modal_register("$('#modal_visualizar').modal('show');"), True)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub DDL_utilidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_utilidad.SelectedIndexChanged

        Try

            'Leer_apartado
            leer_apartado(DDL_utilidad.SelectedItem.ToString)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "setTimeout(function () { $('#DDL_utilidad').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');"), True)

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "DDL_utilidad_SelectedIndexChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error DDL_utilidad_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_Click(sender As Object, e As EventArgs) Handles btn_grabar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepcion
            If __txt_consulta.Text.Count = 68 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_consulta').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');") & "error('El Campo Consulta no puede estar vacío.')", True)
                Exit Sub
            End If

            'Restricción si existen tipo con la misma denominacion
            Dim tabla As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].tickets WHERE consulta='" & __txt_consulta.Text & "';")
            If tabla.Rows.Count <> 0 Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#DDL_utilidad').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');") & "error('Ya existe un Ticket con la misma consulta.')", True)
                Exit Sub
            End If

            'Tiempo Inicio del Proceso
            Dim date1 As Date = Date.Now

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Agrego
                memComando.CommandText = "INSERT INTO tickets (fecha_creacion,hora_creacion,id_empresa,id_usuario,nombre_usuario,utilidad,apartado,consulta,telefono) VALUES " &
                "(@fecha_creacion,@hora_creacion,@id_empresa,@id_usuario,@nombre_usuario,@utilidad,@apartado,@consulta,@telefono);"
                memComando.Parameters.Add("@fecha_creacion", System.Data.SqlDbType.Date)
                memComando.Parameters("@fecha_creacion").Value = DateTime.Today
                memComando.Parameters.Add("@hora_creacion", System.Data.SqlDbType.Time)
                memComando.Parameters("@hora_creacion").Value = Now.ToString("HH:mm:ss")
                memComando.Parameters.Add("@id_empresa", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@id_empresa").Value = tabla_empresa.Rows(0)("Id")
                memComando.Parameters.Add("@id_usuario", System.Data.SqlDbType.VarChar)
                memComando.Parameters("@id_usuario").Value = tabla_usuario.Rows(0)("Id")
                memComando.Parameters.Add("@nombre_usuario", System.Data.SqlDbType.VarChar, 150)
                memComando.Parameters("@nombre_usuario").Value = tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido")
                memComando.Parameters.Add("@utilidad", System.Data.SqlDbType.VarChar, 50)
                memComando.Parameters("@utilidad").Value = DDL_utilidad.SelectedValue.ToString
                memComando.Parameters.Add("@apartado", System.Data.SqlDbType.VarChar, 50)
                memComando.Parameters("@apartado").Value = DDL_apartado.SelectedValue.ToString
                memComando.Parameters.Add("@consulta", System.Data.SqlDbType.VarChar, 1000)
                memComando.Parameters("@consulta").Value = __txt_consulta.Text
                memComando.Parameters.Add("@telefono", System.Data.SqlDbType.VarChar, 9)
                memComando.Parameters("@telefono").Value = txt_telefono.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Tiempo finalización
            Dim date2 As DateTime = Date.Now

            'Diferencia
            Dim tspan As TimeSpan
            tspan = date2.Subtract(date1)

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Atención Cliente", "Creó el Ticket de la Utilidad: " & DDL_utilidad.SelectedItem.ToString & " y Apartado: " & DDL_apartado.SelectedItem.ToString & ".(Tiempo: " & tspan.ToString("hh\:mm\:ss\:ff") & ").")

            'Leer Tipos
            leer_tipos()

            'Compongo Email
            email_cliente(tabla_usuario.Rows(0)("email").ToString, gridview_consulta.DataKeys(0).Item("id").ToString(), tabla_usuario.Rows(0)("nombre").ToString)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Ticket creado correctamente.');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub email_cliente(ByRef direccion As String, ByVal id As String, ByVal nombre_usuario As String)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Aqui mando un correo electrónico a los destinatarios que me diga el cliente.
            Dim correo As New System.Net.Mail.MailMessage
            correo.From = New System.Net.Mail.MailAddress("asistencia.optimus@gmail.com")
            correo.To.Add(direccion)
            correo.Bcc.Add("asistencia.optimus@gmail.com")
            correo.Subject = "Ticket Nº: " & id & " de Optimus Facturación."

            'Monto el cuerpo del correo
            Dim mensajemail As String

            mensajemail = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><head>" &
            "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'> " &
            "<title>Restauración de Claves de Cloud Projects</title>" &
            "<style type = 'text/css'>" &
            "/* Client - specific Styles */" &
            "#outlook a{padding:0;} /* Force Outlook To provide a 'view In browser' button. */" &
            "body{width:100% !important;} .ReadMsgBody{width:100%;} .ExternalClass{width:100%;} /* Force Hotmail to display emails at full width */ " &
            "body{-webkit - Text - size - adjust: none;} /* Prevent Webkit platforms from changing Default text sizes. */ " &
            "/* Reset Styles */ " &
            "body{margin: 0; padding:0;} " &
            "img{border: 0; line-height:100%; outline:none; text-decoration:none;} " &
            "Table td{border-collapse: collapse;} " &
            "</style> " &
            "</head> " &
            "<body leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0' style='font-family:Verdana,Arial,sans-serif;'> " &
            "<table border='0' cellpadding='0' cellspacing='0' width='100%' style='width:100%;'>" &
            "<tr>" &
            "<td style='width:25%;'></td>" &
            "<td style='width:50%; text-align:center;'><span style='color: #4D4D4D'>Se envió este correo electrónico desde una dirección exclusiva de notificaciones que no recibe correos electrónicos. <u>No respondas a este mensaje</u>.</span></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;'></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style='width:50%;text-align:center;'><table style='width:100%;'><tr><td style='text-align:left;'><span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Hola,<span style='color: #007bff;'> " & System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nombre_usuario) & "</span></span></td><td style='text-align:right;'><img src='https://www.optimus-soluciones.com/facturacion/imagenes/logo/logo_empresa.png' style='height:81px;'></td></tr></table></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%;height:40px;'></td>" &
            "<td style='width:50%;text-align:left;'>" &
            "<span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Has solicitado la siguiente consulta:</span><br><br>" &
            "<span style='color: #4D4D4D;font-size:14px;'>" & gridview_consulta.DataKeys(0).Item("consulta").ToString() & "</span><br><br><span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>Despreocúpate, ya estamos trabajando en ello y en breve obtendrás una respuesta.</span> " &
            "</td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
             "<tr>" &
            "<td style='width:25%;height:30px;'></td>" &
            "<td style='width:50%;'></td>" &
            "<td style='width:25%;'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td style='width:50%;text-align:left;'><span style='color: #b3b3b3;font-size:18px; font-weight:bold;'>El equipo de <span style='color: #000000;'>Optimus Facturación.</span></span></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%; height:30px;'></td>" &
            "<td style='width:50%; border: 1px; border-width: 2px; border-color: red;><hr></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "<tr>" &
            "<td style='width:25%'></td>" &
            "<td><span style='color: #b3b3b3'><b>Protege tu contraseña</b><br>" &
            "El personal de Optimus Contabilidad NUNCA te pedirá tu contraseña por correo electrónico. Sólo necesitarás introducirla cuando inicies sesión en Optimus Contabilidad. Siempre iniciarás sesión por medio de una conexión segura y te pediremos que verifiques que la dirección del explorador comience exactamente con https://www.optimus-soluciones.com. También debería aparecer el símbolo de un candado pequeño para indicar que la conexión es segura. " &
            "<br><br>Ten cuidado con mensajes de correo electrónico que soliciten información de cuenta o exijan acciones inmediatas. La misma recomendación rige para sitios web con direcciones atípicas. " &
            "<br><br>© " & Year(DateTime.Now) & " Optimiza Gestores Documentales S.L.</span></td>" &
            "<td style='width:25%'></td>" &
            "</tr>" &
            "</table>" &
            "</body></html>"

            correo.Body = mensajemail
            correo.IsBodyHtml = True
            correo.Priority = System.Net.Mail.MailPriority.Normal
            Dim smtp As New System.Net.Mail.SmtpClient

            Dim nombre As String = "asistencia.optimus@gmail.com"
            Dim contrasena As String = "pcxv zxsv qvwg uqso"
            smtp.UseDefaultCredentials = False
            smtp.EnableSsl = True
            smtp.Port = 587
            smtp.Host = "smtp.gmail.com"
            smtp.Credentials = New System.Net.NetworkCredential(nombre, contrasena)

            'Envio el correo a los destinatarios
            smtp.Send(correo)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "email_cliente", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error email_cliente: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridview_consulta.PageIndexChanging

        Try

            'Actualizar posicion y cosnulta
            gridview_consulta.PageIndex = e.NewPageIndex
            leer_tipos()

        Catch ex As Exception
            'Obtengo
            Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
            Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "gridview_consulta_PageIndexChanging", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error gridview_consulta_PageIndexChanging: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Lkb_refrecar_Click(sender As Object, e As EventArgs) Handles Lkb_refrecar.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'refrescar
            leer_tipos()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Lkb_refrecar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Lkb_refrecar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_abrir_nuevo_Click(sender As Object, e As EventArgs) Handles btn_abrir_nuevo.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "setTimeout(function () { $('#DDL_utilidad').focus();}, 100);" & funciones_globales.modal_register("$('#modal_agregar').modal('show');  "), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_abrir_nuevo_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_abrir_nuevo_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class
