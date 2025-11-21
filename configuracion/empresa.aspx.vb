Imports System.Data
Imports System.Data.SqlClient

Partial Class configuracion_empresa
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_tipo_vias()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[tipo_vias] ORDER BY nombre")

            'Asigno
            DDL_tipo_via.Items.Clear()
            DDL_tipo_via.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre").ToString, tabla_consulta.Rows(x)("Id").ToString)
                Me.DDL_tipo_via.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_tipo_vias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_tipo_vias: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_provincias()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT nombre,valor FROM [kernel_facturacion].[dbo].[provincia] ORDER BY nombre")

            'Asigno
            DDL_provincia.Items.Clear()
            DDL_provincia.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre").ToString, tabla_consulta.Rows(x)("valor").ToString)
                Me.DDL_provincia.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_provincias", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_provincias: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub listar_localidad(ByVal provincia As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[localidad] WHERE id_provincia=" & provincia & " ORDER BY cp;")

            'Asigno
            DDL_localidad.Items.Clear()
            DDL_localidad.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre").ToString & " - (CP:" & tabla_consulta.Rows(x)("cp").ToString & ")", tabla_consulta.Rows(x)("id_localidad"))
                Me.DDL_localidad.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "listar_localidad", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error listar_localidad: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_pais()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT id,nombre_pais FROM [kernel_facturacion].[dbo].[paises] ORDER BY nombre_pais;")

            'Asigno
            DDL_pais.Items.Clear()
            DDL_pais.Items.Add(New ListItem("", "0"))

            'Recorro
            For x = 0 To tabla_consulta.Rows.Count - 1

                'Cargo los detalles
                Dim lista As New System.Web.UI.WebControls.ListItem(tabla_consulta.Rows(x)("nombre_pais").ToString, tabla_consulta.Rows(x)("id"))
                Me.DDL_pais.Items.Add(lista)

            Next

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_pais", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_pais: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub listar_cp(ByVal id_provincia As Integer, ByVal id_localidad As Integer)

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].[localidad] WHERE id_provincia=" & id_provincia & " AND id_localidad='" & id_localidad & "';")

            'Asigno
            txt_cp.Text = tabla_consulta.Rows(0)("cp").ToString
            txt_cp_enable.Text = tabla_consulta.Rows(0)("cp").ToString

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "listar_cp", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error listar_cp: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_datos()

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Asigno
            Dim tabla_consulta As DataTable = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Dirección
            txt_nombre_fiscal.Text = tabla_consulta.Rows(0)("nombre_fiscal").ToString
            txt_nombre_comercial.Text = tabla_consulta.Rows(0)("nombre_comercial").ToString
            txt_telefono.Text = tabla_consulta.Rows(0)("telefono").ToString
            txt_email.Text = tabla_consulta.Rows(0)("e_mail").ToString
            txt_nif.Text = tabla_consulta.Rows(0)("nif").ToString

            'Leer Tipo Via
            leer_tipo_vias()
            DDL_tipo_via.SelectedIndex = DDL_tipo_via.Items.IndexOf(DDL_tipo_via.Items.FindByValue(tabla_consulta.Rows(0)("tipo_domicilio")))
            txt_domicilio.Text = tabla_consulta.Rows(0)("domicilio").ToString
            txt_numero.Text = tabla_consulta.Rows(0)("numero").ToString
            txt_escalera.Text = tabla_consulta.Rows(0)("escalera").ToString
            txt_piso.Text = tabla_consulta.Rows(0)("piso").ToString
            txt_puerta.Text = tabla_consulta.Rows(0)("puerta").ToString

            'Leer Provincia
            leer_provincias()
            DDL_provincia.SelectedIndex = DDL_provincia.Items.IndexOf(DDL_provincia.Items.FindByValue(tabla_consulta.Rows(0)("provincia")))

            'Leer Localidad
            listar_localidad(DDL_provincia.SelectedValue)
            DDL_localidad.SelectedIndex = DDL_localidad.Items.IndexOf(DDL_localidad.Items.FindByValue(tabla_consulta.Rows(0)("poblacion").ToString()))

            'Leer Pais
            leer_pais()
            DDL_pais.SelectedIndex = DDL_pais.Items.IndexOf(DDL_pais.Items.FindByValue(tabla_consulta.Rows(0)("pais")))
            txt_cp.Text = tabla_consulta.Rows(0)("cp").ToString()

            'Facturas
            chk_isp.Checked = tabla_consulta.Rows(0)("facturas_isp")
            txt_isp.Text = tabla_consulta.Rows(0)("texto_isp")
            chk_exento.Checked = tabla_consulta.Rows(0)("facturas_exento")
            txt_exento.Text = tabla_consulta.Rows(0)("texto_exento")

            'Albaranes
            chk_valorado.Checked = tabla_consulta.Rows(0)("albaran_sin_valorar")
            chk_gestion_documental_albaran.Checked = tabla_consulta.Rows(0)("albaran_qr")

            'Informes
            DDL_n_registros_consultas.SelectedIndex = DDL_n_registros_consultas.Items.IndexOf(DDL_n_registros_consultas.Items.FindByValue(tabla_consulta.Rows(0)("n_paginado_consultas")))

            'BackupCloud
            DDL_dias_backup.SelectedIndex = DDL_dias_backup.Items.IndexOf(DDL_dias_backup.Items.FindByValue(tabla_consulta.Rows(0)("dias_backupcloud").ToString))

            'Liberamos
            tabla_consulta.Dispose()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "leer_datos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error leer_datos: " & ex.Message.Replace("'", " ") & "');", True)
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

            'Ver fondo
            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Leer Datos
                leer_datos()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "Page_Load", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error Page_Load: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub DDL_provincia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_provincia.SelectedIndexChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Leer localidad con respecto a la provincia
            listar_localidad(DDL_provincia.SelectedValue)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "setTimeout(function () { $('#DDL_localidad').focus();}, 100);" & funciones_globales.modal_register("$('#nav-direccion-tab').click();"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "DDL_provincia_SelectedIndexChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error DDL_provincia_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub DDL_localidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDL_localidad.SelectedIndexChanged

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Leer Localidad
            listar_cp(DDL_provincia.SelectedValue, DDL_localidad.SelectedValue)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "DDL_localidad_SelectedIndexChanged", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error DDL_localidad_SelectedIndexChanged: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_grabar_direccion_Click(sender As Object, e As EventArgs) Handles btn_grabar_direccion.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepcion
            If txt_nif.Text <> "" Then

                'Compruebo si es un CIF o NIF
                Dim esCIF As Boolean = funciones_globales.Verificar_CIF(txt_nif.Text)
                Dim esNIF As Boolean = funciones_globales.Verificar_NIF(txt_nif.Text)

                If esCIF = False And esNIF = False Then
                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);error('El NIF no es válido.');" & funciones_globales.modal_register("$('#nav-direccion-tab').click();"), True)
                    Exit Sub
                End If

            Else
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_nif').focus();$('#txt_nif').select();}, 100);error('El NIF no puede estar vacío.');" & funciones_globales.modal_register("$('#nav-direccion-tab').click();"), True)
                Exit Sub
            End If

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand

                'Actualizo
                memComando.CommandText = "UPDATE empresa SET " &
                "nombre_fiscal=@nombre_fiscal, " &
                "nombre_comercial=@nombre_comercial, " &
                "telefono=@telefono, " &
                "e_mail=@e_mail, " &
                "nif=@nif, " &
                "tipo_domicilio=@tipo_domicilio, " &
                "domicilio=@domicilio, " &
                "numero=@numero, " &
                "escalera=@escalera, " &
                "piso=@piso, " &
                "puerta=@puerta, " &
                "provincia=@provincia, " &
                "poblacion=@poblacion, " &
                "pais=@pais, " &
                "cp=@cp " &
                "WHERE Id=" & tabla_empresa.Rows(0)("Id") & ";"
                memComando.Parameters.Add("@nombre_fiscal", System.Data.SqlDbType.VarChar, 200)
                memComando.Parameters("@nombre_fiscal").Value = txt_nombre_fiscal.Text
                memComando.Parameters.Add("@nombre_comercial", System.Data.SqlDbType.VarChar, 200)
                memComando.Parameters("@nombre_comercial").Value = txt_nombre_comercial.Text
                memComando.Parameters.Add("@telefono", System.Data.SqlDbType.VarChar, 12)
                memComando.Parameters("@telefono").Value = txt_telefono.Text
                memComando.Parameters.Add("@e_mail", System.Data.SqlDbType.VarChar, 50)
                memComando.Parameters("@e_mail").Value = txt_email.Text
                memComando.Parameters.Add("@nif", System.Data.SqlDbType.VarChar, 15)
                memComando.Parameters("@nif").Value = txt_nif.Text
                memComando.Parameters.Add("@tipo_domicilio", System.Data.SqlDbType.VarChar, 50)
                memComando.Parameters("@tipo_domicilio").Value = DDL_tipo_via.SelectedValue
                memComando.Parameters.Add("@domicilio", System.Data.SqlDbType.VarChar, 250)
                memComando.Parameters("@domicilio").Value = txt_domicilio.Text
                memComando.Parameters.Add("@numero", System.Data.SqlDbType.VarChar, 5)
                memComando.Parameters("@numero").Value = txt_numero.Text
                memComando.Parameters.Add("@escalera", System.Data.SqlDbType.VarChar, 10)
                memComando.Parameters("@escalera").Value = txt_escalera.Text
                memComando.Parameters.Add("@piso", System.Data.SqlDbType.VarChar, 10)
                memComando.Parameters("@piso").Value = txt_piso.Text
                memComando.Parameters.Add("@puerta", System.Data.SqlDbType.VarChar, 10)
                memComando.Parameters("@puerta").Value = txt_puerta.Text
                memComando.Parameters.Add("@provincia", System.Data.SqlDbType.VarChar, 50)
                memComando.Parameters("@provincia").Value = DDL_provincia.SelectedValue
                memComando.Parameters.Add("@poblacion", System.Data.SqlDbType.VarChar, 50)
                memComando.Parameters("@poblacion").Value = DDL_localidad.SelectedValue
                memComando.Parameters.Add("@pais", System.Data.SqlDbType.VarChar, 50)
                memComando.Parameters("@pais").Value = DDL_pais.SelectedValue
                memComando.Parameters.Add("@cp", System.Data.SqlDbType.VarChar, 5)
                memComando.Parameters("@cp").Value = txt_cp_enable.Text
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()
                memComando.Dispose()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Asigno
            txt_cp.Text = txt_cp_enable.Text

            'Obtengo datos de la empresa
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Empresa", "Cambió opciones en Dirección.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "actualizar_notificacion('" & txt_nombre_fiscal.Text & "','" & txt_nif.Text & "','" & txt_telefono.Text & "','" & txt_email.Text & "','Empresa: " & tabla_empresa.Rows(0)("codigo_empresa").ToString & " - " & txt_nombre_fiscal.Text & " (" & tabla_empresa.Rows(0)("ruta_base_datos").ToString & ")');ok('Cambios Realizados Correctamente.');" & funciones_globales.modal_register("$('#nav-direccion-tab').click();"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_direccion_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_direccion_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_facturas_Click(sender As Object, e As EventArgs) Handles btn_grabar_facturas.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Excepción
            If chk_isp.Checked = True Then

                If txt_isp.Text = "" Then

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_isp').focus();$('#txt_isp').select();}, 100);error('Si has activado ISP, no puedes dejar el texto legal vacío.');" & funciones_globales.modal_register("$('#nav-facturas-tab').click();"), True)
                    Exit Sub

                End If

            End If

            'Excepción
            If chk_exento.Checked = True Then

                If txt_exento.Text = "" Then

                    'Registro como bloque en local para el jquery
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Error_", "setTimeout(function () { $('#txt_exento').focus();$('#txt_exento').select();}, 100);error('Si has activado Exento, no puedes dejar el texto legal vacío.');" & funciones_globales.modal_register("$('#nav-facturas-tab').click();"), True)
                    Exit Sub

                End If

            End If

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand("UPDATE empresa SET facturas_isp='" & chk_isp.Checked & "', texto_isp=@texto_isp,facturas_exento='" & chk_exento.Checked & "', texto_exento=@texto_exento WHERE id=" & tabla_empresa.Rows(0)("Id") & ";", memConn)
                memComando.Parameters.Add("@texto_isp", System.Data.SqlDbType.VarChar, 400)
                memComando.Parameters("@texto_isp").Value = txt_isp.Text
                memComando.Parameters.Add("@texto_exento", System.Data.SqlDbType.VarChar, 400)
                memComando.Parameters("@texto_exento").Value = txt_exento.Text
                memComando.ExecuteNonQuery()

                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Obtengo datos de la empresa
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Empresa", "Cambió opciones en Facturas.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Cambios Realizados Correctamente.');" & funciones_globales.modal_register("$('#nav-facturas-tab').click();"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_facturas_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_facturas_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_albaranes_Click(sender As Object, e As EventArgs) Handles btn_grabar_albaranes.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand("UPDATE empresa SET albaran_sin_valorar=@albaran_sin_valorar, albaran_qr=@albaran_qr WHERE id=" & tabla_empresa.Rows(0)("Id") & ";", memConn)
                memComando.Parameters.Add("@albaran_sin_valorar", System.Data.SqlDbType.Bit)
                memComando.Parameters("@albaran_sin_valorar").Value = chk_valorado.Checked
                memComando.Parameters.Add("@albaran_qr", System.Data.SqlDbType.Bit)
                memComando.Parameters("@albaran_qr").Value = chk_gestion_documental_albaran.Checked
                memComando.ExecuteNonQuery()

                'memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Obtengo datos de la empresa
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Empresa", "Cambió opciones en Albaranes.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Cambios Realizados Correctamente.');" & funciones_globales.modal_register("$('#nav-albaranes-tab').click();"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_albaranes_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_albaranes_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_grabar_informes_Click(sender As Object, e As EventArgs) Handles btn_grabar_informes.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try
            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand("UPDATE empresa SET " &
                    "n_paginado_consultas=" & DDL_n_registros_consultas.SelectedValue & " " &
                    "WHERE id=" & tabla_empresa.Rows(0)("Id") & ";", memConn)
                memComando.ExecuteNonQuery()

                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Obtengo datos de la empresa
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Empresa", "Cambió opciones en Informes.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Cambios Realizados Correctamente.');" & funciones_globales.modal_register("$('#nav-informes-tab').click();"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_informes_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_informes_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_backupcloud_Click(sender As Object, e As EventArgs) Handles btn_grabar_backupcloud.Click

        'Obtengo
        Dim tabla_usuario As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_usuario") & "_tabla_usuario")
        Dim tabla_empresa As DataTable = HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa")

        Try

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=kernel_facturacion")

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand("UPDATE empresa SET dias_backupcloud='" & DDL_dias_backup.SelectedValue & "' WHERE id=" & tabla_empresa.Rows(0)("Id") & ";", memConn)
                memComando.ExecuteNonQuery()

                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Obtengo datos de la empresa
            HttpContext.Current.Session("f_" & Request.QueryString("id_empresa") & "_tabla_empresa") = funciones_globales.obtener_datos("SELECT * FROM [kernel_facturacion].[dbo].empresa WHERE empresa.id=" & tabla_empresa.Rows(0)("Id") & ";")

            'Limpio el trazador
            funciones_globales.grabar_registro(tabla_empresa.Rows(0)("ruta_base_datos"), tabla_usuario.Rows(0)("Id"), tabla_usuario.Rows(0)("nombre") & " " & tabla_usuario.Rows(0)("primer_apellido") & " " & tabla_usuario.Rows(0)("segundo_apellido"), "Empresa", "Cambió opciones en BackupCloud.")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_ok", "ok('Cambios Realizados Correctamente.');" & funciones_globales.modal_register("$('#nav-backupcloud-tab').click();"), True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(tabla_empresa.Rows(0)("Id"), tabla_usuario.Rows(0)("Id"), Request.Url.Segments.Last(), "btn_grabar_backupcloud_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "Error('Error btn_grabar_backupcloud_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class

