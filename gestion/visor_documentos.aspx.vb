Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports Ionic.Zip
Imports System.Data.SqlClient

Partial Class gestion_visor_documentos
    Inherits System.Web.UI.Page

    Dim funciones_globales As New funciones_globales

    Sub leer_treeview()

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Limpiar
            TreeView1.Nodes.Clear()

            'Asigno Path para IO Almacenamiento
            Dim rootInfo As New DirectoryInfo("D:/imagenes_usuarios/" & parametros_empresa(5) & "/drive/")
            Me.PopulateTreeView(rootInfo, Nothing)

            'Asigno
            TreeView1.Nodes(0).ExpandAll()

            'Seleccionar el nodo expandido
            Dim n As TreeNode
            For Each n In TreeView1.Nodes

                PrintRecursive(n)

            Next

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_treeview", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_treeview: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub PopulateTreeView(dirInfo As DirectoryInfo, treeNode As TreeNode)

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            For Each directory As DirectoryInfo In dirInfo.GetDirectories()

                Dim directoryNode As New TreeNode() With {
                    .Text = directory.Name,
                    .Value = directory.FullName,
                    .ImageUrl = "../Imagenes/web/Folder-05.png"
                }

                'Si es bandeja
                If directory.Name = "bandeja" Then
                    directoryNode.ImageUrl = "../Imagenes/web/Auto Archive-WF.png"
                    directoryNode.Text = "Bandeja de Entrada"
                End If

                'Si es Almacenamiento
                If directory.Name = "almacenamiento" Then
                    directoryNode.ImageUrl = "../Imagenes/web/Monitor-03-WF.png"
                    directoryNode.Text = "IO Almacenamiento"
                End If

                'Asigno
                If treeNode Is Nothing Then
                    'If Root Node, add to TreeView.
                    TreeView1.Nodes.Add(directoryNode)
                Else
                    'If Child Node, add to Parent Node.
                    treeNode.ChildNodes.Add(directoryNode)
                End If

                'Asigno
                PopulateTreeView(directory, directoryNode)

            Next

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "PopulateTreeView", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error PopulateTreeView: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub PrintRecursive(ByVal n As TreeNode)

        Dim aNode As TreeNode
        For Each aNode In n.ChildNodes

            If aNode.Value = txt_ruta.Text Then
                aNode.ExpandAll()
                Exit For
            End If

            PrintRecursive(aNode)

        Next

    End Sub

    Sub leer_ruta()

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Variables
            Dim ruta As String = txt_ruta.Text.Replace("D:|imagenes_usuarios|" & parametros_empresa(5) & "|drive|", "").Replace("almacenamiento", "IO Almacenamiento").Replace("bandeja", "Bandeja de Entrada")
            Dim palabra As String = Nothing

            If ruta.LastIndexOf("|") <> -1 Then

                'Asigno
                palabra = Mid(ruta, ruta.LastIndexOf("|") + 1).Replace("|", "\")
                ruta = Mid(ruta, 1, ruta.LastIndexOf("|")).Replace("|", "\")

            End If

            'Asigno
            Lt_ruta.Text = "<span style='color: #7b7b7b; font-size: 12px'>" & ruta & "<span><span style='color: #28a745; font-size: 12px'>" & palabra & "<span>"


        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_ruta", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_ruta: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Sub leer_ficheros(ByVal ruta As String)

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            Dim tabla_contenido As New DataTable
            tabla_contenido.Columns.Add("nombre")
            tabla_contenido.Columns.Add("fecha_modificacion")
            tabla_contenido.Columns.Add("tipo_fichero")
            tabla_contenido.Columns.Add("size")
            tabla_contenido.Columns.Add("ruta_imagen")
            tabla_contenido.Columns.Add("pdf")

            'Solo se lee si estamos en la Bandeja de Entrada
            If txt_ruta.Text.IndexOf("|drive|bandeja") <> -1 Then

                'Activo el Timer para refrescar cada x tiempo
                Timer1.Enabled = True

            Else

                'Activo el Timer para refrescar cada x tiempo
                Timer1.Enabled = False

            End If

            'Recorro
            For Each archivos In My.Computer.FileSystem.GetFiles(ruta)

                'Declaro para obtener las propiedades de los ficheros
                Dim propiedades = New FileInfo(archivos)

                Dim Renglon As DataRow = tabla_contenido.NewRow()
                Renglon("nombre") = propiedades.Name
                Renglon("fecha_modificacion") = propiedades.LastWriteTimeUtc.ToShortDateString & " - " & propiedades.LastWriteTimeUtc.ToShortTimeString
                Renglon("tipo_fichero") = propiedades.Extension.ToUpper
                Renglon("size") = Math.Round(propiedades.Length / 1024) & " KB"

                'Selección
                'allowedfileextensions: ['.pdf','.txt','.xlsx','.docx','.zip'],
                Select Case Path.GetExtension(propiedades.FullName).ToLower
                    Case ".pdf" : Renglon("ruta_imagen") = "~/imagenes/web/PDF_rojo.png"
                    Case ".txt" : Renglon("ruta_imagen") = "~/imagenes/web/File-WF_negro.png"
                    Case ".xlsx" : Renglon("ruta_imagen") = "~/imagenes/web/Excel_verde.png"
                    Case ".docx" : Renglon("ruta_imagen") = "~/imagenes/web/Word_azul.png"
                    Case ".zip" : Renglon("ruta_imagen") = "~/imagenes/web/File-Format-ZIP_amarillo.png"
                    Case ".jpg" : Renglon("ruta_imagen") = "~/imagenes/web/File-Format-JPEG_gris.png"
                    Case ".png" : Renglon("ruta_imagen") = "~/imagenes/web/File-Format-PNG_gris.png"
                End Select

                'Solo se lee si estamos en la Bandeja de Entrada
                If txt_ruta.Text.IndexOf("|drive|bandeja") <> -1 Then

                    If Path.GetExtension(propiedades.FullName).ToLower = ".pdf" Then

                        'Variables
                        Dim tabla_consulta As DataTable = funciones_globales.obtener("inforplan", "SELECT * FROM pdf_plantillas_r WHERE KEY_1<>'' and KEY_2<>'' AND KEY_3<>'';")

                        'Busco la plantilla
                        Renglon("pdf") = buscar_plantilla(propiedades.Name)

                    Else

                        'Asigno
                        Renglon("pdf") = "Sin analizar"

                    End If

                Else

                    'Asigno
                    Renglon("pdf") = "Sin analizar"

                End If

                'Inserto
                tabla_contenido.Rows.Add(Renglon)

            Next

            'Asigno
            gridview_consulta.DataSource = tabla_contenido
            gridview_consulta.DataBind()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_ficheros", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_ficheros: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            If Session("id_control") = "" Then
                Response.Redirect("../login.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then

                'Fecha acceso y hora
                lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

                'Compruebo
                Dim ruta_bandeja As String = "D:/imagenes_usuarios/" & parametros_empresa(5) & "/drive/bandeja"
                Dim ruta_io_almacenamiento As String = "D:/imagenes_usuarios/" & parametros_empresa(5) & "/drive/almacenamiento"
                If Not Directory.Exists(ruta_bandeja) Then
                    Directory.CreateDirectory(ruta_bandeja)
                End If
                If Not Directory.Exists(ruta_io_almacenamiento) Then
                    Directory.CreateDirectory(ruta_io_almacenamiento)
                End If

                'Asigno
                Dim ruta As String = "D:/imagenes_usuarios/" & parametros_empresa(5) & "/drive/almacenamiento"
                txt_ruta.Text = ruta.Replace("/", "|")

                'Leer_ruta
                leer_ruta()

                'Leer TreeView
                leer_treeview()

                'Leer_ficheros
                leer_ficheros(ruta)

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_tipo_asientos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_tipo_asientos: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged


        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Asigno
            txt_ruta.Text = TreeView1.SelectedValue.ToString.Replace("\", "|")

            'Leer_ruta
            leer_ruta()

            'Leer_ficheros
            leer_ficheros(TreeView1.SelectedValue.ToString)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "leer_tipo_asientos", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error leer_tipo_asientos: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_grabar_nueva_carpeta_Click(sender As Object, e As EventArgs) Handles btn_grabar_nueva_carpeta.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Creo
            Directory.CreateDirectory(txt_ruta.Text.Replace("|", "\") & "\" & funciones_globales.URLtoText(txt_nombre_nueva_carpeta.Text))

            'Leer TreeView
            leer_treeview()

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Agregó la carpeta " & txt_ruta.Text & "\" & txt_nombre_nueva_carpeta.Text & ".")

            'Limpieza
            txt_nombre_nueva_carpeta.Text = Nothing

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_grabar_nueva_carpeta_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_grabar_nueva_carpeta_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_borrar_carpeta_Click(sender As Object, e As EventArgs) Handles btn_borrar_carpeta.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Creo
            Directory.Delete(txt_ruta.Text.Replace("|", "\"), True)

            'Variables
            Dim ruta As String = txt_ruta.Text
            Dim palabra As String = Nothing

            'Asigno
            ruta = Mid(ruta, 1, ruta.LastIndexOf("|"))

            'Asigno
            txt_ruta.Text = ruta

            'Leer_ruta
            leer_ruta()

            'Leer TreeView
            leer_treeview()

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Eliminó la carpeta " & txt_ruta.Text & ".")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_borrar_carpeta_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_borrar_carpeta_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click

        'Cargo los datos a global
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Leer Ficheros
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Subió ficheros a la carpeta: " & txt_ruta.Text & ".")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btnUpload_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btnUpload_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub gridview_consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_consulta.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim onmouseoverStyle As String = "this.style.backgroundColor='#f2ffe4';this.style.cursor='Default'"
                Dim onmouseoutStyle As String = "this.style.backgroundColor='white'"
                Dim cursoronmouse As String = "this.style.cursor='Default'"

                'Asigno la propiedas                
                e.Row.Attributes.Add("onmouseover", onmouseoverStyle)
                e.Row.Attributes.Add("onmouseout", onmouseoutStyle)

                'Asigno un ID
                For x = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(x).Attributes.Add("id", e.Row.RowIndex & "_" & x)
                Next

                'DesAsigno la propiedas  
                e.Row.Cells(0).BackColor = ColorTranslator.FromHtml("#FFFFFF")
                e.Row.Cells(1).BackColor = ColorTranslator.FromHtml("#FFFFFF")
                e.Row.Cells(6).BackColor = ColorTranslator.FromHtml("#FFFFFF")

                'Obtengo control del boton
                Dim btnVal As ImageButton = DirectCast(e.Row.FindControl("img_pdf_masivo"), ImageButton)
                If (gridview_consulta.DataKeys(e.Row.RowIndex).Item("pdf") <> "0|Sin Plantilla|" And gridview_consulta.DataKeys(e.Row.RowIndex).Item("pdf") <> "Sin analizar") Then

                    btnVal.ImageUrl = "~/imagenes/web/icono_pdf.png"
                    btnVal.Height = 20
                    btnVal.Visible = True

                Else

                    btnVal.ImageUrl = "~/imagenes/web/1pixel.png"
                    btnVal.Visible = False

                End If

            End If

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_RowDataBound", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_RowDataBound: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_eliminar_fichero_Click(sender As Object, e As EventArgs) Handles btn_eliminar_fichero.Click

        'Cargo los datos a global
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            For Each mRow As GridViewRow In gridview_consulta.Rows
                Dim mCheck As CheckBox = CType(mRow.FindControl("chk_seleccionar"), CheckBox)
                If mCheck.Checked Then

                    'Borrar
                    File.Delete(txt_ruta.Text.Replace("|", "\") & "\" & gridview_consulta.DataKeys(mRow.RowIndex).Item("nombre").ToString)

                    'Grabar Registro
                    funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Eliminó el fichero: " & txt_ruta.Text & "|" & gridview_consulta.DataKeys(mRow.RowIndex).Item("nombre").ToString & ".")

                End If

            Next

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))


        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_eliminar_fichero_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_eliminar_fichero_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_descargar_fichero_Click(sender As Object, e As EventArgs) Handles btn_descargar_fichero.Click

        'Cargo los datos a global
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Variable
            Dim contador As Integer = 0
            Dim nombre As String = Nothing
            Dim ruta As String = txt_ruta.Text.Replace("|", "\")

            'Recorro para saber si es 1 fichero o varios
            For Each mRow As GridViewRow In gridview_consulta.Rows
                Dim mCheck As CheckBox = CType(mRow.FindControl("chk_seleccionar"), CheckBox)
                If mCheck.Checked Then

                    'Asigno
                    contador += 1
                    nombre = gridview_consulta.DataKeys(mRow.RowIndex).Item("nombre").ToString

                End If

            Next

            'Download 1 fichero
            If contador = 1 Then

                'Propiedades del fichero
                Dim propiedades_fichero As New System.IO.FileInfo(ruta & "\" & nombre)

                'Grabar Registro
                funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Descargó el fichero: " & txt_ruta.Text & "|" & nombre & ".")

                'Devuelvo fichero
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + propiedades_fichero.Name)
                HttpContext.Current.Response.AddHeader("Content-Length", propiedades_fichero.Length.ToString(CultureInfo.InvariantCulture))
                HttpContext.Current.Response.ContentType = "application/octet-stream"
                HttpContext.Current.Response.WriteFile(propiedades_fichero.FullName)
                HttpContext.Current.Response.End()

            End If

            If contador > 1 Then

                'Declaro
                Dim nombre_zip As String = Server.MapPath("..") & "\temp\IO_almacenamiento_" & parametros_usuario(0) & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "") & ".zip"

                'Comienzo la creacion del zip para el usuario
                Using zip As ZipFile = New ZipFile()

                    'Recorro para saber si es 1 fichero o varios
                    For Each mRow As GridViewRow In gridview_consulta.Rows
                        Dim mCheck As CheckBox = CType(mRow.FindControl("chk_seleccionar"), CheckBox)
                        If mCheck.Checked Then

                            'Propiedades del fichero
                            Dim propiedades_fichero As New System.IO.FileInfo(ruta & "\" & gridview_consulta.DataKeys(mRow.RowIndex).Item("nombre").ToString)

                            'Grabar Registro
                            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Descargó el fichero: " & txt_ruta.Text & "|" & gridview_consulta.DataKeys(mRow.RowIndex).Item("nombre").ToString & ".")

                            'Añadir al ZIP
                            zip.AddFile(propiedades_fichero.FullName, "")

                        End If
                    Next

                    'Grabo el fichero
                    zip.Save(Server.MapPath("..") & "\temp\IO_almacenamiento_" & parametros_usuario(0) & "_" & Date.Now.ToShortDateString.Replace("/", "") & "_" & Date.Now.ToLongTimeString.Replace(":", "") & ".zip")

                End Using

                'Devuelvo fichero
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(nombre_zip))
                HttpContext.Current.Response.ContentType = "application/zip"
                HttpContext.Current.Response.WriteFile(nombre_zip)
                HttpContext.Current.Response.End()

            End If

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_descargar_fichero_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_descargar_fichero_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_renombrar_fichero_Click(sender As Object, e As EventArgs) Handles btn_renombrar_fichero.Click

        'Cargo los datos a global
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Excepción
            If txt_rename_fichero.Text = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('El campo Nombre no puede estar vacío.'); $('#modal_rename_fichero').modal('show'); setTimeout(function () { $('#txt_rename_fichero').focus(); }, 100);$('#txt_rename_fichero').select();", True)
                Exit Sub
            End If

            'Variable
            Dim nombre As String = Nothing
            Dim ruta As String = txt_ruta.Text.Replace("|", "\")

            'Recorro para saber si es 1 fichero o varios
            For Each mRow As GridViewRow In gridview_consulta.Rows
                Dim mCheck As CheckBox = CType(mRow.FindControl("chk_seleccionar"), CheckBox)
                If mCheck.Checked Then

                    'Asigno
                    nombre = gridview_consulta.DataKeys(mRow.RowIndex).Item("nombre").ToString

                End If

            Next

            'Renombrar Fichero
            FileSystem.Rename(ruta & "\" & nombre, ruta & "\" & txt_rename_fichero.Text & Path.GetExtension(ruta & "\" & nombre))

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Renombró el fichero: " & nombre & " a " & txt_rename_fichero.Text & Path.GetExtension(ruta & "\" & nombre) & ".")

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_renombrar_fichero_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_renombrar_fichero_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub gridview_consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview_consulta.RowCommand

        Try

            'Recupero el index
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            If (e.CommandName = "ver_fichero") Then

                'Variable
                Dim contador As Integer = 0
                Dim informacion As String = Nothing
                Dim ruta As String = txt_ruta.Text.Replace("|", "\")

                'Recorrro
                For x = 0 To gridview_consulta.Rows.Count - 1
                    Dim mCheck As CheckBox = CType(gridview_consulta.Rows(x).FindControl("chk_seleccionar"), CheckBox)

                    If x = index Then
                        mCheck.Checked = True
                    Else
                        mCheck.Checked = False
                    End If

                    'Asigno
                    informacion += contador & "|" & gridview_consulta.DataKeys(x).Item("nombre").ToString() & "|" & ruta.Replace("D:\", "") & "|" & gridview_consulta.DataKeys(x).Item("size").ToString & "&"

                    'Sumo
                    contador += 1

                Next

                'Asigno valores
                If informacion <> "" Then
                    informacion = Mid(informacion, 1, informacion.Count - 1)
                End If
                txt_informacion.Text = informacion
                txt_total.Text = contador - 1
                txt_actual.Text = index

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "visor();", True)

            End If

            If (e.CommandName = "ver_pdf_masivo") Then

                'Asigno
                LT_adjuntar_pdf.Text = "¿Está seguro de querer enviar el/los fichero(s) <br><br>"

                'Variable
                Dim texto As String = Nothing

                'Recorrro
                For x = 0 To gridview_consulta.Rows.Count - 1
                    Dim mCheck As CheckBox = CType(gridview_consulta.Rows(x).FindControl("chk_seleccionar"), CheckBox)

                    If mCheck.Checked = True Then

                        texto += gridview_consulta.DataKeys(x).Item("nombre").ToString() & "<br>"

                    End If

                Next

                'Asigno
                LT_adjuntar_pdf.Text += texto

                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "alerta_borrar", "$('#modal_enviar_masivo').modal('show');", True)

            End If

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "gridview_consulta_RowCommand", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error gridview_consulta_RowCommand: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_derecha_Click(sender As Object, e As ImageClickEventArgs) Handles img_derecha.Click

        Try

            'Asignamos
            Dim ruta As String = txt_ruta.Text.Replace("|", "\")
            Dim tipo_compresor As ImageFormat = ImageFormat.Png

            'Giramos la imagen a la derecha 90º
            Dim imagen As System.Drawing.Image
            Dim strFilename As String = ruta & "\" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString()
            imagen = System.Drawing.Image.FromFile(strFilename)
            imagen.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone)
            If Path.GetExtension(strFilename) = ".jpg" Then
                tipo_compresor = ImageFormat.Jpeg
            End If
            imagen.Save(ruta & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString(), tipo_compresor)
            imagen.Dispose()

            'Borro la antigua
            File.Delete(ruta & "\" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString())

            'Renombro la nueva
            FileSystem.Rename(ruta & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString(), ruta & "\" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString())

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "visor();", True)

        Catch ex As Exception
            'Cargo las variables de Usuario
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_derecha_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_derecha_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub img_izquierda_Click(sender As Object, e As ImageClickEventArgs) Handles img_izquierda.Click

        Try

            'Asignamos
            Dim ruta As String = txt_ruta.Text.Replace("|", "\")
            Dim tipo_compresor As ImageFormat = ImageFormat.Png

            'Giramos la imagen a la derecha 90º
            Dim imagen As System.Drawing.Image
            Dim strFilename As String = ruta & "\" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString()
            imagen = System.Drawing.Image.FromFile(strFilename)
            imagen.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipXY)
            If Path.GetExtension(strFilename) = ".jpg" Then
                tipo_compresor = ImageFormat.Jpeg
            End If
            imagen.Save(ruta & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString(), tipo_compresor)
            imagen.Dispose()

            'Borro la antigua
            File.Delete(ruta & "\" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString())

            'Renombro la nueva
            FileSystem.Rename(ruta & "\temp_" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString(), ruta & "\" & gridview_consulta.DataKeys(txt_actual.Text).Item("nombre").ToString())

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "mostrar", "visor();", True)

        Catch ex As Exception
            'Cargo las variables de Usuario y Empresa
            Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "img_izquierda_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error img_izquierda_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_subir_fichero_Click(sender As Object, e As EventArgs) Handles btn_subir_fichero.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Ruta de la imagen subida
            Dim ruta As String = txt_ruta.Text.Replace("|", "\")
            Dim ruta_imagen As String = "D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\"

            If FileUpload.HasFiles Then

                'Recorrer
                For Each postedFile As HttpPostedFile In FileUpload.PostedFiles

                    'Reducimos Jpg y png
                    If postedFile.ContentType = "image/jpeg" Or postedFile.ContentType = "image/png" Then

                        If postedFile.ContentLength / 1024 > 512 Then

                            Response.Write(postedFile.ContentLength / 1024 & "<br>")

                            'Create an Encoder object based on the GUID for the Quality parameter category.
                            Dim myEncoder As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality

                            'EncoderParameter object in the array.
                            Dim myEncoderParameters As New Drawing.Imaging.EncoderParameters(1)
                            Dim myEncoderParameter As New Drawing.Imaging.EncoderParameter(myEncoder, 60&)
                            myEncoderParameters.Param(0) = myEncoderParameter

                            'Indico la ruta de la imagen
                            Dim bm_source As New Drawing.Bitmap(postedFile.InputStream)
                            Dim ancho_final As Integer = CInt((((1920 * 100) / bm_source.Width) * bm_source.Height) / 100)
                            Dim imagen_1920 As New Drawing.Bitmap(New Drawing.Bitmap(postedFile.InputStream), 1920, ancho_final)

                            'Guardo comprimido
                            If FileUpload.PostedFile.ContentType = "image/jpeg" Then
                                'Codificador
                                Dim jpgEncoder As Drawing.Imaging.ImageCodecInfo = funciones_globales.GetEncoder(Drawing.Imaging.ImageFormat.Jpeg)
                                imagen_1920.Save(ruta & "/" & postedFile.FileName, jpgEncoder, myEncoderParameters)
                            End If

                            'Guardo comprimido
                            If FileUpload.PostedFile.ContentType = "image/png" Then
                                'Codificador
                                Dim pngEncoder As Drawing.Imaging.ImageCodecInfo = funciones_globales.GetEncoder(Drawing.Imaging.ImageFormat.Png)
                                imagen_1920.Save(ruta & "/" & postedFile.FileName, pngEncoder, myEncoderParameters)
                            End If

                            'Limpiamos
                            bm_source.Dispose()
                            imagen_1920.Dispose()

                        Else

                            'Subo la imagen
                            postedFile.SaveAs(ruta & "/" & postedFile.FileName)

                        End If

                    Else

                        'Subo la imagen
                        postedFile.SaveAs(ruta & "/" & postedFile.FileName)

                    End If

                    'Grabar Registro
                    funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Subió el fichero a la carpeta: " & txt_ruta.Text & "|" & postedFile.FileName & ".")

                Next

            End If

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_subir_fichero_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_subir_fichero_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_pegar_Click(sender As Object, e As EventArgs) Handles btn_pegar.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Variables
            Dim ruta As String = txt_ruta.Text.Replace("|", "\")
            Dim string_envio As String = txt_copiar.Text
            Dim identificador As String = "copiar"

            'Excepcion
            If txt_mover.Text <> "" Then
                string_envio = txt_mover.Text
                identificador = "mover"
            End If

            'Asigno
            Dim vficheros() As String = string_envio.Split("&")

            'Recorro
            For x = 0 To vficheros.Count - 1

                If identificador = "copiar" Then

                    'Copio Fichero
                    File.Copy(vficheros(x).Replace("|", "\"), ruta & "\" & Path.GetFileName(vficheros(x).Replace("|", "\")))

                    'Grabar Registro
                    funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Copió el fichero: " & vficheros(x).Replace("|", "\") & " a " & ruta & "\" & Path.GetFileName(vficheros(x).Replace("|", "\")) & ".")

                End If

                If identificador = "mover" Then

                    'Copio Fichero
                    File.Move(vficheros(x).Replace("|", "\"), ruta & "\" & Path.GetFileName(vficheros(x).Replace("|", "\")))

                    'Grabar Registro
                    funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Movió el fichero: " & vficheros(x).Replace("|", "\") & " a " & ruta & "\" & Path.GetFileName(vficheros(x).Replace("|", "\")) & ".")

                End If

            Next

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_pegar_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_pegar_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_adjuntar_asiento_si_Click(sender As Object, e As EventArgs) Handles btn_adjuntar_asiento_si.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Variables
            Dim vruta() As String = txt_asignar_impuesto.Text.Split("|")

            'Asigno el nuevo nombre y subo la imagen
            Dim nombre As String = parametros_usuario(0) & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & Path.GetExtension(vruta(2))

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\")
            End If

            'Ruta de la imagen subida
            Dim ruta_imagen_relativa As String = "imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\"
            Dim ruta_original As String = "D:\" & vruta(3).Replace("|", "\") & "\" & vruta(2)
            Dim ruta_destino As String = "D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\" & nombre

            'Muevo el fichero
            System.IO.File.Move(ruta_original, ruta_destino)

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                'Comando
                Dim memComando As New SqlCommand

                'Observaciones
                Dim obser_tratado As String = nombre

                memComando.CommandText = "INSERT INTO [ficheros] (id_cabecera_impuesto,id_cabecera_asiento,fecha_creacion,hora_creacion,ruta,nombre_fichero,size,observaciones) VALUES ('0','" & vruta(1) & "','" & DateTime.Today & "','" & Now.ToString("HH:mm:ss") & "','" & ruta_imagen_relativa & "','" & nombre.ToLower & "','" & (vruta(4).Replace(" KB", "") * 1024) & "','" & obser_tratado & "');"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Adjuntó el fichero: " & vruta(2) & " al Nº Asiento: " & vruta(5) & ".")

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

            'Creamos una session para indicarle a asientos que se habra
            Session("numero_asiento") = vruta(1)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "refrescar_ventana", "ok('Fichero asignado.');$('#ientradas',window.parent.document).attr('src',$('#ientradas',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('entradas','description','1000','600','asientos/asientos.aspx','1');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_adjuntar_asiento_si_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_adjuntar_asiento_si_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_adjuntar_impuesto_si_Click(sender As Object, e As EventArgs) Handles btn_adjuntar_impuesto_si.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Variables
            Dim vruta() As String = txt_asignar_impuesto.Text.Split("|")

            'Asigno el nuevo nombre y subo la imagen
            Dim nombre As String = parametros_usuario(0) & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & Path.GetExtension(vruta(2))

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\")
            End If

            'Ruta de la imagen subida
            Dim ruta_imagen_relativa As String = "imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\"
            Dim ruta_original As String = "D:\" & vruta(3).Replace("|", "\") & "\" & vruta(2)
            Dim ruta_destino As String = "D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\" & nombre

            'Muevo el fichero
            System.IO.File.Move(ruta_original, ruta_destino)

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                'Comando
                Dim memComando As New SqlCommand

                'Observaciones
                Dim obser_tratado As String = nombre

                memComando.CommandText = "INSERT INTO [ficheros] (id_cabecera_impuesto,id_cabecera_asiento,fecha_creacion,hora_creacion,ruta,nombre_fichero,size,observaciones) VALUES ('" & vruta(0) & "','" & vruta(1) & "','" & DateTime.Today & "','" & Now.ToString("HH:mm:ss") & "','" & ruta_imagen_relativa & "','" & nombre.ToLower & "','" & (vruta(4).Replace(" KB", "") * 1024) & "','" & obser_tratado & "');"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Adjuntó el fichero: " & vruta(2) & " al Nº Impuesto: " & vruta(0) & ".")

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

            'Creamos una session para indicarle a asientos que se habra
            Session("numero_impuesto") = vruta(0)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "refrescar_ventana", "ok('Fichero asignado.');$('#ientradas_',window.parent.document).attr('src',$('#ientradas_',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('entradas_','currency_exchange','700','500','impuestos/impuestos.aspx','2');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_adjuntar_impuesto_si_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_adjuntar_impuesto_si_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub btn_adjuntar_impuesto_si_IA_Click(sender As Object, e As EventArgs) Handles btn_adjuntar_impuesto_si_IA.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Variables
            Dim vruta() As String = txt_asignar_impuesto_IA.Text.Split("|")

            'Asigno el nuevo nombre y subo la imagen
            Dim nombre As String = parametros_usuario(0) & "_" & Date.Now.Day & Date.Now.Month & Date.Now.Year & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & Path.GetExtension(vruta(2))

            'Creo la carpeta para el fondo de éste usuario
            If Not System.IO.Directory.Exists("D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\") Then
                System.IO.Directory.CreateDirectory("D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\")
            End If

            'Ruta de la imagen subida
            Dim ruta_imagen_relativa As String = "imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\"
            Dim ruta_original As String = "D:\" & vruta(3).Replace("|", "\") & "\" & vruta(2)
            Dim ruta_destino As String = "D:\imagenes_usuarios\" & parametros_empresa(5) & "\ficheros\" & Year(Now) & "\" & Month(Now) & "\" & Day(Now) & "\" & nombre

            'Muevo el fichero
            System.IO.File.Move(ruta_original, ruta_destino)

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                'Comando
                Dim memComando As New SqlCommand

                'Observaciones
                Dim obser_tratado As String = nombre

                memComando.CommandText = "INSERT INTO [ficheros] (id_cabecera_impuesto,id_cabecera_asiento,fecha_creacion,hora_creacion,ruta,nombre_fichero,size,observaciones) VALUES ('" & vruta(0) & "','" & vruta(1) & "','" & DateTime.Today & "','" & Now.ToString("HH:mm:ss") & "','" & ruta_imagen_relativa & "','" & nombre.ToLower & "','" & (vruta(4).Replace(" KB", "") * 1024) & "','" & obser_tratado & "');"
                memComando.Connection = memConn
                memComando.ExecuteNonQuery()

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Adjuntó el fichero: " & vruta(2) & " al Nº Impuesto: " & vruta(0) & ".")

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

            'Creamos una session para indicarle a asientos que se habra
            Session("numero_impuesto") = vruta(0)

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "refrescar_ventana", "ok('Fichero asignado.');$('#ientradas_IA_',window.parent.document).attr('src',$('#ientradas_IA_',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('ientradas_IA_','currency_exchange','700','500','impuestos/impuestos_IA.aspx','2');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_adjuntar_impuesto_si_IA_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_adjuntar_impuesto_si_IA_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Function buscar_plantilla(ByVal nombre_pdf As String) As String

        'Variables
        Dim ruta As String = txt_ruta.Text.Replace("|", "\")
        Dim ruta_fichero As String = ruta & "/" & nombre_pdf
        Dim sOut As String = Nothing

        'Leemos Documento
        Using oReader As New iTextSharp.text.pdf.PdfReader(ruta_fichero)

            'Comienzo la Lectura del PDF
            For i = 1 To oReader.NumberOfPages
                'Dim its As New iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy
                sOut &= iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(oReader, i)
            Next

        End Using

        'Asigno
        If Trim(sOut) = "" Then

            Return "0|PDF No Válido|"

        End If

        'Variables
        Dim tabla_consulta As DataTable = funciones_globales.obtener("inforplan", "SELECT * FROM pdf_plantillas_r WHERE KEY_1<>'' and KEY_2<>'' AND KEY_3<>'';")

        'Recorrer
        For x = 0 To tabla_consulta.Rows.Count - 1

            If tabla_consulta.Rows(x)("sin_nif") = True Then

                If sOut.IndexOf(tabla_consulta.Rows(x)("key_1").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_2").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_3").ToString) <> -1 Then

                    'Cargar plantilla
                    Return tabla_consulta.Rows(x)("id") & "|" & tabla_consulta.Rows(x)("denominacion") & "|" & tabla_consulta.Rows(x)("nif")

                End If

            Else

                If sOut.IndexOf(tabla_consulta.Rows(x)("nif").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_1").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_2").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_3").ToString) <> -1 Then

                    'Cargar plantilla
                    Return tabla_consulta.Rows(x)("id") & "|" & tabla_consulta.Rows(x)("denominacion") & "|" & tabla_consulta.Rows(x)("nif")

                End If

            End If

        Next

        'Variables
        tabla_consulta = funciones_globales.obtener("inforplan", "SELECT * FROM pdf_plantillas_r WHERE KEY_1<>'' and KEY_2<>'' AND KEY_3='';")

        'Recorrer
        For x = 0 To tabla_consulta.Rows.Count - 1

            If tabla_consulta.Rows(x)("sin_nif") = True Then

                If sOut.IndexOf(tabla_consulta.Rows(x)("key_1").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_2").ToString) <> -1 Then

                    'Cargar plantilla
                    Return tabla_consulta.Rows(x)("id") & "|" & tabla_consulta.Rows(x)("denominacion") & "|" & tabla_consulta.Rows(x)("nif")

                End If

            Else

                If sOut.IndexOf(tabla_consulta.Rows(x)("nif").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_1").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_2").ToString) <> -1 Then

                    'Cargar plantilla
                    Return tabla_consulta.Rows(x)("id") & "|" & tabla_consulta.Rows(x)("denominacion") & "|" & tabla_consulta.Rows(x)("nif")

                End If

            End If

        Next

        'Variables
        tabla_consulta = funciones_globales.obtener("inforplan", "SELECT * FROM pdf_plantillas_r WHERE KEY_1<>'' and KEY_2='' AND KEY_3='';")

        'Recorrer
        For x = 0 To tabla_consulta.Rows.Count - 1

            If tabla_consulta.Rows(x)("sin_nif") = True Then

                If sOut.IndexOf(tabla_consulta.Rows(x)("key_1").ToString) <> -1 Then

                    'Cargar plantilla
                    Return tabla_consulta.Rows(x)("id") & "|" & tabla_consulta.Rows(x)("denominacion") & "|" & tabla_consulta.Rows(x)("nif")

                End If

            Else

                If sOut.IndexOf(tabla_consulta.Rows(x)("nif").ToString) <> -1 And sOut.IndexOf(tabla_consulta.Rows(x)("key_1").ToString) <> -1 Then

                    'Cargar plantilla
                    Return tabla_consulta.Rows(x)("id") & "|" & tabla_consulta.Rows(x)("denominacion") & "|" & tabla_consulta.Rows(x)("nif")

                End If

            End If

        Next

        'Variables
        tabla_consulta = funciones_globales.obtener("inforplan", "SELECT * FROM pdf_plantillas_r WHERE KEY_1='' and KEY_2='' AND KEY_3='';")

        'Recorrer
        For x = 0 To tabla_consulta.Rows.Count - 1

            If sOut.IndexOf(tabla_consulta.Rows(x)("nif").ToString) <> -1 Then

                'Cargar plantilla
                Return tabla_consulta.Rows(x)("id") & "|" & tabla_consulta.Rows(x)("denominacion") & "|" & tabla_consulta.Rows(x)("nif")

            End If

        Next

        Return "0|Sin Plantilla|"

    End Function

    Protected Sub btn_adjuntar_pdf_Click(sender As Object, e As EventArgs) Handles btn_adjuntar_pdf.Click

        'Cargo las variables de Usuario y Empresa
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Parar el Timer
            Timer1.Enabled = False

            'Asignar
            Dim contador As Integer = 1

            'Conecto
            Using memConn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString & ";Initial Catalog=" & parametros_empresa(5))

                'Abrimos conexion
                memConn.Open()

                Dim memComando As New SqlCommand
                memComando.Parameters.Add(New SqlParameter("fecha_creacion", Data.SqlDbType.Date))
                memComando.Parameters.Add(New SqlParameter("hora_creacion", Data.SqlDbType.Time))

                'Variables
                Dim id_plantilla As Integer = 0
                Dim nombre_plantilla As String = Nothing
                Dim tipo_impuesto As String = "SOPORTADO"

                'Recorro
                For x = 0 To gridview_consulta.Rows.Count - 1
                    Dim mCheck As CheckBox = CType(gridview_consulta.Rows(x).FindControl("chk_seleccionar"), CheckBox)

                    If mCheck.Checked = True Then

                        'Asignar
                        Dim vector_resultado() As String = gridview_consulta.DataKeys(x).Item("pdf").ToString().Split("|")
                        id_plantilla = vector_resultado(0)
                        nombre_plantilla = vector_resultado(1)
                        tipo_impuesto = "SOPORTADO"

                        'Selecciono si el nif de la plantilla y el de la empresa son iguales es repercutido
                        If parametros_empresa(2) = Trim(vector_resultado(2).Replace(".", "").Replace("-", "").Replace(" ", "")) Then
                            tipo_impuesto = "REPERCUTIDO"
                        End If

                        memComando.CommandText = "INSERT INTO pdf_masivos (fecha_creacion,hora_creacion,nombre_pdf,id_plantilla,nombre_plantilla,tipo_impuesto) VALUES (@fecha_creacion,@hora_creacion,'" & gridview_consulta.DataKeys(x).Item("nombre").ToString() & "'," & id_plantilla & ",'" & nombre_plantilla & "','" & tipo_impuesto & "');"
                        memComando.Parameters("fecha_creacion").Value = DateTime.Today
                        memComando.Parameters("hora_creacion").Value = Now.ToString("HH:mm:ss")
                        memComando.Connection = memConn
                        memComando.ExecuteNonQuery()

                        'Mover Fichero
                        File.Move(txt_ruta.Text.Replace("|", "\") & "\" & gridview_consulta.DataKeys(x).Item("nombre").ToString, "D:\imagenes_usuarios\Temp\" & gridview_consulta.DataKeys(x).Item("nombre").ToString)

                        'Asignar
                        contador += 1

                    End If

                Next

                'Cerramos
                memComando.Dispose()
                SqlConnection.ClearPool(memConn)

            End Using

            'Leer Fichero
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Envió a la Entrada Masiva de PDFs: " & contador & " archivo(s).")

            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "refrescar_ventana", "ok('Fichero(s) asignado(s).');$('#imasivo_pdf',window.parent.document).attr('src',$('#imasivo_pdf',window.parent.document).attr('src'));window.parent.abrir_ventana_relacional('masivo_pdf','dashboard','700','500','impuestos/masivo_pdf.aspx','2');", True)

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_adjuntar_pdf_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_adjuntar_pdf_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_rename_carpeta_Click(sender As Object, e As EventArgs) Handles btn_rename_carpeta.Click

        'Cargo los datos a global
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Excepción
            If txt_rename_fonder.Text = "" Then
                'Registro como bloque en local para el jquery
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('El campo Nombre no puede estar vacío.'); $('#modal_renombrar_carpeta').modal('show'); setTimeout(function () { $('#txt_rename_fonder').focus(); }, 100);$('#txt_rename_fonder').select();", True)
                Exit Sub
            End If

            'Variable
            Dim ruta As String = txt_ruta.Text.Replace("|", "\")
            Dim vruta() As String = txt_ruta.Text.Split("|")
            Dim nombre_old As String = vruta(vruta.Length - 1)

            'Asigno
            vruta(vruta.Length - 1) = txt_rename_fonder.Text

            'Renombrar Directorio
            My.Computer.FileSystem.RenameDirectory(ruta, txt_rename_fonder.Text)

            'Leer TreeView
            leer_treeview()

            'Grabar Registro
            funciones_globales.grabar_registro(parametros_empresa(5), parametros_usuario(0), parametros_usuario(7) & " " & parametros_usuario(8) & " " & parametros_usuario(9), "IO Almacenamiento", "Renombró la carpeta: " & nombre_old & " a " & txt_rename_fonder.Text & ".")

            'Limpieza
            txt_rename_fonder.Text = Nothing

            'Asigno
            txt_ruta.Text = String.Join("|", vruta)

            'Leer Ruta
            leer_ruta()

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "btn_rename_carpeta_Click", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error btn_rename_carpeta_Click: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        'Cargo los datos a global
        Dim parametros_usuario() As String = HttpContext.Current.Session("parametros_usuario").Split("|")
        Dim parametros_empresa() As String = HttpContext.Current.Session("parametros_empresa").Split("|")

        Try

            'Leer_ficheros
            leer_ficheros(txt_ruta.Text.Replace("|", "\"))

            'Fecha acceso y hora
            lbl_conexion.Text = Date.Now.ToString("HH:mm:ss")

        Catch ex As Exception
            'Registro Error
            funciones_globales.grabar_registro_error(parametros_usuario(1), parametros_usuario(0), Request.Url.Segments.Last(), "Timer1_Tick", ex.Message)
            'Registro como bloque en local para el jquery
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Notificacion_error", "error('Error Timer1_Tick: " & ex.Message.Replace("'", " ") & "');", True)
        End Try

    End Sub

End Class