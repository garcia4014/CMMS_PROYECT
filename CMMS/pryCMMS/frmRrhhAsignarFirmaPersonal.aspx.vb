Imports CapaNegocioCMMS
Imports CapaDatosCMMS
Imports Microsoft.SqlServer.Server

Public Class frmRrhhAsignarFirmaPersonal
    Inherits System.Web.UI.Page
    Dim PersonalNeg As New clsPersonalNegocios
    Dim FirmaNeg As New clsAsignarFirmaNegocios
    'Dim TipCertNeg As New clsTipoCertificadoNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Dim MyValidator As New CustomValidator

    Public Sub CargarPerfil()
        Dim FuncionesNeg As New clsFuncionesNegocios

        Dim mpContentPlaceHolder As ContentPlaceHolder
        Dim mpImage As Image
        Dim mpNombreUsuario As Label
        Dim mpApellidoPaternoUsuario As Label
        Dim mpApellidoMaternoUsuario As Label

        mpContentPlaceHolder =
            CType(Master.FindControl("LeftPanel$mnu_izq"),
            ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            mpImage = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$imgPerfil"), Image)
            If Not mpImage Is Nothing Then
                mpImage.ImageUrl = "~\Imagenes\Usr\" & Trim(Session("IdUsuario")) & ".JPG"
            End If

            Dim UsuarioNeg As New clsUsuarioNegocios
            Dim Usuario As GNRL_USUARIO = UsuarioNeg.UsuarioListarPorId(Session("IdUsuario"))

            mpNombreUsuario = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblNombreUsuario"), Label)
            If Not mpNombreUsuario Is Nothing Then
                mpNombreUsuario.Text = StrConv(Usuario.vNombresUsuario, VbStrConv.ProperCase)
            End If

            mpApellidoPaternoUsuario = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblApellidoPaternoUsuario"), Label)
            If Not mpApellidoPaternoUsuario Is Nothing Then
                mpApellidoPaternoUsuario.Text = StrConv(Usuario.vApellidoPaternoUsuario, VbStrConv.ProperCase)
            End If

            mpApellidoMaternoUsuario = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblApellidoMaternoUsuario"), Label)
            If Not mpApellidoMaternoUsuario Is Nothing Then
                mpApellidoMaternoUsuario.Text = StrConv(Usuario.vApellidoMaternoUsuario, VbStrConv.ProperCase)
            End If
        End If
    End Sub

    Function fValidarSesion() As Boolean
        fValidarSesion = False
        If Session("IdUsuario") = "" Then
            fValidarSesion = True
            'Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Throw New Exception("Su sesión ha caducado, ingrese de nuevo por favor.")
            'ElseIf Session("IdConfEmpresa") = "" Then
            '    fValidarSesion = True
            '    'Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
            '    Throw New Exception("No se ha ingresado al sistema correctamente.")
        End If
        'If Session("IdPuntoVenta") <> "" Or Session("IdEmpresa") <> "" Then
        If Session("IdEmpresa") <> "" Then
            Dim mpIdEmpresa, mpIdPuntoVenta As HiddenField
            mpIdEmpresa = CType(Page.Master.FindControl("lblIdEmpresa"), HiddenField)
            mpIdPuntoVenta = CType(Page.Master.FindControl("lblIdPuntoVenta"), HiddenField)

            If Not mpIdEmpresa Is Nothing Then
                If mpIdEmpresa.Value <> Session("IdEmpresa") Then
                    fValidarSesion = True
                End If
            End If
            If Not mpIdPuntoVenta Is Nothing Then
                If mpIdPuntoVenta.Value <> Session("IdPuntoVenta") Then
                    fValidarSesion = True
                End If
            End If

            If fValidarSesion = True Then
                Dim message As String = "Tiene dos sesiones distintas abiertas en la misma ventana de navegación y eso no es correcto, presione [F5] para refrescar la información actual de su navegador.  " &
                                            "Para poder utilizar dos sesiones distintas, realicelo en dos ventanas diferentes del navegador que usted utilice."
                Dim sb As New System.Text.StringBuilder()
                sb.Append("alert('")
                sb.Append(message)
                sb.Append("');")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowAlert", sb.ToString(), True)
                Throw New Exception("Existen distintas sesiones, favor de actualizar su explorador!")
                'Dim segmentosURL = HttpContext.Current.Request.Url.Segments
                'Response.Redirect(segmentosURL(segmentosURL.Length - 1))
            End If
        End If
    End Function

    Shared Function CrearCestaCertificados() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Item", GetType(System.Int32))) '0
        dt.Columns.Add(New DataColumn("CodUsu", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("NombreCompleto", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("Url", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("FechaRegistro", GetType(System.DateTime))) '3
        dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '7
        Return dt
    End Function

    'Shared Sub EditarCestaCertificados(ByVal Codigo As String, ByVal Descripcion As String, ByVal NroCertificado As String,
    '                       ByVal FechaVigenciaInicio As DateTime, ByVal FechaVigenciaFinal As DateTime, ByVal UrlDescarga As String,
    '                       ByVal Estado As Boolean, ByVal Status As String, ByVal EstadoGeneral As String,
    '                       ByVal Tabla As DataTable, ByVal Fila As Integer)
    Shared Sub EditarCestaCertificados(ByVal Item As Integer, ByVal CodUsu As String, ByVal NombreCompleto As String,
                           ByVal Url As String, ByVal FechaRegistro As DateTime, ByVal Estado As Boolean,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Item
                Tabla.Rows(Fila)(1) = CodUsu
                Tabla.Rows(Fila)(2) = NombreCompleto
                Tabla.Rows(Fila)(3) = Url
                Tabla.Rows(Fila)(4) = FechaRegistro
                Tabla.Rows(Fila)(5) = Estado
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    'Shared Sub AgregarCestaFirmas(ByVal Codigo As String, ByVal Descripcion As String, ByVal NroCertificado As String,
    '                       ByVal FechaVigenciaInicio As DateTime, ByVal FechaVigenciaFinal As DateTime, ByVal UrlDescarga As String,
    '                       ByVal Estado As Boolean, ByVal Status As String, ByVal EstadoGeneral As String,
    '                       ByVal Tabla As DataTable)
    Shared Sub AgregarCestaFirmas(ByVal Item As Integer, ByVal CodUsu As String, ByVal NombreCompleto As String,
                           ByVal Url As String, ByVal FechaRegistro As DateTime, ByVal Estado As Boolean,
                           ByVal Tabla As DataTable)

        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Item 'Tabla.Rows.Count + 1
        Fila("CodUsu") = CodUsu
        Fila("NombreCompleto") = NombreCompleto
        Fila("Url") = Url
        Fila("FechaRegistro") = FechaRegistro
        Fila("Estado") = Estado
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaFirmas(ByVal Fila As Integer, ByVal Tabla As DataTable)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows.RemoveAt(Fila)
                Dim i As Integer
                For i = 0 To Tabla.Rows.Count - 1
                    Tabla.Rows(i).BeginEdit()
                    Tabla.Rows(i).EndEdit()
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub VaciarCestaFirmas(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltroFirma.DataTextField = "vDescripcionTablaSistema"
        cboFiltroFirma.DataValueField = "vValor"
        cboFiltroFirma.DataSource = FiltroNeg.TablaSistemaListarCombo("96", "CMMS", Session("IdEmpresa"))
        cboFiltroFirma.Items.Clear()
        cboFiltroFirma.DataBind()
    End Sub

    Sub ListarUsuarioCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsUsuarioNegocios
        cboUsuario.DataTextField = "vNombreCompleto"
        cboUsuario.DataValueField = "cIdUsuario"
        cboUsuario.DataSource = FiltroNeg.UsuarioGetData("SELECT cIdUsuario, vApellidoPaternoUsuario + ' ' + vApellidoMaternoUsuario + ', ' + vNombresUsuario AS vNombreCompleto
                                                          FROM GNRL_USUARIO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND bEstadoRegistroUsuario = 1 
                                                          ORDER BY vApellidoPaternoUsuario, vApellidoMaternoUsuario, vNombresUsuario")
        cboUsuario.Items.Clear()
        cboUsuario.DataBind()
    End Sub



    'Sub ListarTipoDocumentoCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim FiltroNeg As New clsTablaSistemaNegocios
    '    cboTipoDocumentoPersonal.DataTextField = "vDescripcionTablaSistema"
    '    cboTipoDocumentoPersonal.DataValueField = "vValor"
    '    cboTipoDocumentoPersonal.DataSource = FiltroNeg.TablaSistemaListarCombo("01", "CTBL", Session("IdEmpresa"))
    '    cboTipoDocumentoPersonal.Items.Clear()
    '    cboTipoDocumentoPersonal.DataBind()
    'End Sub

    'Sub ListarCertificadoCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim CertificadoNeg As New clsTipoCertificadoNegocios
    '    cboCertificado.DataTextField = "vDescripcionTipoCertificado"
    '    cboCertificado.DataValueField = "cIdTipoCertificado"
    '    cboCertificado.DataSource = CertificadoNeg.TipoCertificadoListarCombo()
    '    cboCertificado.Items.Clear()
    '    cboCertificado.DataBind()
    'End Sub

    'Sub ValidarTextoCertificado(ByVal bValidar As Boolean)
    '    Me.rfvDescripcionMantenimientoCertificado.EnableClientScript = bValidar
    '    Me.rfvDescripcionAbreviadaMantenimientoCertificado.EnableClientScript = bValidar
    'End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        'Me.btnGuardar.Visible = bGuardar
        Me.btnEditar.Visible = bEditar
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        'cboUsuario.Enabled = bActivar 'bActivar
        cboUsuario.Enabled = IIf(hfdOperacion.Value = "N", bActivar, False) 'bActivar
    End Sub

    Sub LlenarDataPersonal()
        Try
            'Dim PersonalNeg As New clsPersonalNegocios
            'Dim Personal As RRHH_PERSONAL = PersonalNeg.PersonalListarPorId(grdLista.SelectedRow.Cells(0).Text, Session("IdEmpresa"))
            'lblIdPersonal.Text = Personal.cIdPersonal
            'lblNombreCompletoPersonal.Text = Personal.vNombreCompletoPersonal
            'cboTipoDocumentoPersonal.SelectedValue = Personal.cIdTipoDocumento
            'lblNumeroDocumentoPersonal.Text = Personal.vNumeroDocumentoPersonal
            cboUsuario.SelectedValue = grdLista.SelectedRow.Cells(0).Text
            CargarCestaFirmas()

            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Registro encontrado con éxito"
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Sub LlenarDataFirma()
    '    Try
    '        If hfdOperacionDetalle.Value = "N" Then
    '            LimpiarObjetosCertificado()
    '        Else
    '            LimpiarObjetosCertificado()
    '            Dim CertificadoNeg As New clsTipoCertificadoNegocios
    '            Dim Certificado As RRHH_TIPOCERTIFICADO = CertificadoNeg.TipoCertificadoListarPorId(cboCertificado.SelectedValue)
    '            txtIdMantenimientoCertificado.Text = Certificado.cIdTipoCertificado
    '            txtDescripcionMantenimientoCertificado.Text = Certificado.vDescripcionTipoCertificado
    '            txtDescripcionAbreviadaMantenimientoCertificado.Text = Certificado.vDescripcionAbreviadaTipoCertificado
    '        End If
    '        If MyValidator.ErrorMessage = "" Then
    '            MyValidator.ErrorMessage = "Registro encontrado con éxito"
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        cboUsuario.SelectedIndex = -1
        VaciarCestaFirmas(Session("CestaFirmas"))
        grdListaFirmas.DataSource = Session("CestaFirmas")
        grdListaFirmas.DataBind()
        'lblIdPersonal.Text = ""
        'lblNombreCompletoPersonal.Text = ""
        'cboTipoDocumentoPersonal.SelectedIndex = -1
        'lblNumeroDocumentoPersonal.Text = ""
    End Sub

    'Sub LimpiarObjetosCertificado()
    '    MyValidator.ErrorMessage = ""
    '    txtIdMantenimientoCertificado.Text = ""
    '    txtDescripcionMantenimientoCertificado.Text = ""
    '    txtDescripcionAbreviadaMantenimientoCertificado.Text = ""
    'End Sub

    Sub LimpiarObjetosAgregarFirma()
        MyValidator.ErrorMessage = ""
        'lblDescripcionAgregarCertificados.Text = ""
        'txtNroCertificadoAgregarCertificados.Text = ""
        'txtFechaInicioAgregarCertificados.Text = String.Format("{0:dd/MM/yyyy}", Now)
        'txtFechaFinalAgregarCertificados.Text = String.Format("{0:dd/MM/yyyy}", Now)
        'fupSubirFirma.FileName = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "087" 'Mantenimiento de Asignación de Certificados por Personal.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroFirma.SelectedIndex = 4
            ListarUsuarioCombo()
            'ListarTipoDocumentoCombo()
            'ListarCertificadoCombo()

            If Session("CestaFirmas") Is Nothing Then
                Session("CestaFirmas") = CrearCestaCertificados()
            Else
                VaciarCestaFirmas(Session("CestaFirmas"))
            End If

            BloquearMantenimiento(True, False, True, False)
            'Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroFirma.SelectedValue,
            '                                                         txtBuscarFirma.Text, Session("IdEmpresa"))
            Me.grdLista.DataSource = FirmaNeg.AsignarFirmaListaBusqueda(cboFiltroFirma.SelectedValue, txtBuscarFirma.Text, Session("IdEmpresa"), "*")
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
        Else
            txtBuscarFirma.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0678", strOpcionModulo, "CMMS")

            'If grdLista IsNot Nothing Then
            '    If grdLista.Rows.Count > 0 Then
            '        If IsNothing(grdLista.SelectedRow) = False Then
            '            If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            '                hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(4).Text) = "True", "1", "0")
            '                If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
            '                    Throw New Exception("Este registro se encuentra desactivado.")
            '                End If
            '            End If
            '        Else
            '            Throw New Exception("Debe de seleccionar un item.")
            '        End If
            '    End If
            'End If
            hfdOperacion.Value = "N"

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ActivarObjetos(True)
            'LlenarDataPersonal()

            pnlCabecera.Visible = False
            pnlContenido.Visible = True
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        Catch ex As Exception
            BloquearMantenimiento(True, False, True, False)
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0684", strOpcionModulo, "CMMS")
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            'JMUG: 31/08/2025
                            'lblIdPersonal.Text = grdLista.SelectedRow.Cells(0).Text

                            'hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(4).Text) = "True", "1", "0")
                            'lnkbtnVerPersonal.Attributes.Add("onclick", "javascript:popupEmitirCertificadoPersonalReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "');")
                        End If
                    End If
                Else
                    'lnk_mostrarPanelCaracteristica_ModalPopupExtsender.Show()
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left
                'e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                'e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            'e.Row.Cells(2).Visible = True
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            'e.Row.Cells(2).Visible = True
        End If
    End Sub

    Protected Sub grdListaFirmas_RowCommand_Botones(sender As Object, e As GridViewCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If
            If e.CommandName = "Activar" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresFirma.Value = e.CommandArgument.ToString
                lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender.Show()
            ElseIf e.CommandName = "VerFirma" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresFirma.Value = e.CommandArgument.ToString
                'lnk_mostrarPanelSubirFirma_ModalPopupExtender.Show()
                Dim strNombreFirma As String = ""
                strNombreFirma = Valores(3).ToString
                'imgVerImagenFirma.ImageUrl = "~\Imagenes\Firmas\" & strNombreFirma 'Puede ser ahora JPG/JPEG/PNG/etc.
                imgVerImagenFirma.ImageUrl = "~\" & strNombreFirma 'Puede ser ahora JPG/JPEG/PNG/etc.

                lnk_mostrarPanelVerImagenFirma_ModalPopupExtender.Show()
            End If
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
            'lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
        End Try
    End Sub

    'Private Sub btnAceptarMantenimientoCertificado_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoCertificado.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

    '        Dim CertificadoNeg As New clsTipoCertificadoNegocios
    '        Dim Certificado As New RRHH_TIPOCERTIFICADO
    '        Certificado.cIdTipoCertificado = txtIdMantenimientoCertificado.Text
    '        Certificado.vDescripcionTipoCertificado = UCase(txtDescripcionMantenimientoCertificado.Text.Trim)
    '        Certificado.vDescripcionAbreviadaTipoCertificado = UCase(txtDescripcionAbreviadaMantenimientoCertificado.Text.Trim)
    '        Certificado.bEstadoRegistroTipoCertificado = True

    '        Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '        LogAuditoria.cIdPaisOrigen = Session("IdPais")
    '        LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '        LogAuditoria.cIdLocal = Session("IdLocal")
    '        'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
    '        'LogAuditoria.cIdUsuario = Session("IdUsuario")
    '        LogAuditoria.cIdUsuario = Session("IdUsuario")
    '        LogAuditoria.vIP1 = Session("IP1")
    '        LogAuditoria.vIP2 = Session("IP2")
    '        LogAuditoria.vPagina = Session("URL")
    '        LogAuditoria.vSesion = Session("IdSesion")
    '        If hfdOperacionDetalle.Value = "N" Then
    '            If (CertificadoNeg.TipoCertificadoInserta(Certificado)) = 0 Then
    '                Session("Query") = "PA_RRHH_MNT_TIPOCERTIFICADO 'SQL_INSERT', '','" & Certificado.cIdTipoCertificado & "', '" & Certificado.vDescripcionTipoCertificado & "', '" & Certificado.vDescripcionAbreviadaTipoCertificado & "', '" &
    '                                    Certificado.bEstadoRegistroTipoCertificado & "', '" & Certificado.cIdTipoCertificado & "'"
    '                LogAuditoria.vEvento = "INSERTAR TIPO DE CERTIFICADO"
    '                LogAuditoria.vQuery = Session("Query")
    '                LogAuditoria.cIdSistema = Session("IdSistema")
    '                LogAuditoria.cIdModulo = strOpcionModulo
    '                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)
    '                txtIdMantenimientoCertificado.Text = Certificado.cIdTipoCertificado
    '                MyValidator.ErrorMessage = "Transacción registrada con éxito"
    '                BloquearMantenimiento(False, True, False, True)
    '            Else
    '                Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
    '            End If
    '        ElseIf hfdOperacionDetalle.Value = "E" Then
    '            If (CertificadoNeg.TipoCertificadoEdita(Certificado)) = 0 Then
    '                Session("Query") = "PA_RRHH_MNT_TIPOCERTIFICADO 'SQL_UPDATE', '','" & Certificado.cIdTipoCertificado & "', '" & Certificado.vDescripcionTipoCertificado & "', '" & Certificado.vDescripcionAbreviadaTipoCertificado & "', '" &
    '                                    Certificado.bEstadoRegistroTipoCertificado & "', '" & Certificado.cIdTipoCertificado & "'"
    '                LogAuditoria.vEvento = "ACTUALIZAR TIPO DE CERTIFICADO"
    '                LogAuditoria.vQuery = Session("Query")
    '                LogAuditoria.cIdSistema = Session("IdSistema")
    '                LogAuditoria.cIdModulo = strOpcionModulo
    '                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)
    '                MyValidator.ErrorMessage = "Registro actualizado con éxito"
    '                BloquearMantenimiento(False, True, False, True)
    '            Else
    '                Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
    '            End If
    '        End If
    '        hfdOperacionDetalle.Value = "R"
    '        ListarCertificadoCombo()
    '        cboCertificado.SelectedValue = Certificado.cIdTipoCertificado
    '        pnlCabecera.Visible = False
    '        pnlContenido.Visible = True
    '        ValidarTextoCertificado(False)
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Function fStatusCertificados(ByVal dFechaInicial As DateTime, ByVal dFechaFinal As DateTime) As String
        fStatusCertificados = IIf(DateDiff(DateInterval.Day, Now, dFechaFinal) < 0, "VENCIDO", "EN VIGENCIA")
    End Function

    Sub CargarCestaFirmas()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaFirmas(Session("CestaFirmas"))

            Dim AsignarFirmaNeg As New clsAsignarFirmaNegocios
            'Dim dsFirmas = AsignarFirmaNeg.AsignarFirmaGetData("SELECT ASICER.cIdPersonal, ASICER.cIdTipoCertificado, TIPCER.vDescripcionTipoCertificado, ASICER.dFechaVigenciaInicioAsignarCertificado, ASICER.dFechaVigenciaFinalAsignarCertificado, ASICER.vURLAsignarCertificado, ASICER.nItemAsignarCertificado, ASICER.vNumeroReferenciaAsignarCertificado, ASICER.bEstadoRegistroAsignarCertificado, ASICER.cEstadoRegistroAsignarCertificado " &
            '                                                    "FROM RRHH_ASIGNARCERTIFICADO AS ASICER INNER JOIN RRHH_TIPOCERTIFICADO AS TIPCER ON " &
            '                                                    "ASICER.cIdTipoCertificado = TIPCER.cIdTipoCertificado " &
            '                                                    "WHERE ASICER.cIdPersonal = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' " &
            '                                                    "      AND ASICER. bEstadoRegistroAsignarCertificado = '1'")

            Dim dsFirmas = AsignarFirmaNeg.AsignarFirmaGetData("SELECT ASIFIR.cIdUsuario, ASIFIR.nItemAsignarFirma, ASIFIR.cIdEmpresa, USU.vApellidoPaternoUsuario + ' ' + USU.vApellidoMaternoUsuario + ', ' + USU.vNombresUsuario AS vNombreCompleto, " &
                                                     "ASIFIR.vURLAsignarFirma, ASIFIR.dFechaRegistroAsignarFirma, ASIFIR.bEstadoRegistroAsignarFirma " &
                                                     "FROM RRHH_ASIGNARFIRMA AS ASIFIR INNER JOIN GNRL_USUARIO AS USU ON " &
                                                     "     ASIFIR.cIdEmpresa = USU.cIdEmpresa AND ASIFIR.cIdUsuario = USU.cIdUsuario " &
                                                     "WHERE ASIFIR.cIdUsuario = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "'" &
                                                     "      AND ASIFIR.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                     "ORDER BY USU.vApellidoPaternoUsuario + ' ' + USU.vApellidoMaternoUsuario + ', ' + USU.vNombresUsuario ")

            For Each Firmas In dsFirmas.Rows
                AgregarCestaFirmas(Firmas("nItemAsignarFirma"), Firmas("cIdUsuario"), Firmas("vNombreCompleto"),
                                   Firmas("vURLAsignarFirma"), Firmas("dFechaRegistroAsignarFirma"), Firmas("bEstadoRegistroAsignarFirma"),
                                   Session("CestaFirmas"))
            Next
            grdListaFirmas.DataSource = Session("CestaFirmas")
            grdListaFirmas.DataBind()
            LimpiarObjetosAgregarFirma()
            grdListaFirmas.SelectedIndex = -1
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Private Sub btnAceptarAgregarCertificados_Click(sender As Object, e As EventArgs) Handles btnAceptarAgregarCertificados.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

    '        If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
    '            If MyValidator.ErrorMessage = "" Then
    '                For i = 0 To Session("CestaCertificados").Rows.Count - 1
    '                    If (Session("CestaCertificados").Rows(i)("Codigo").ToString.Trim) = (cboCertificado.SelectedValue.ToString.Trim) And
    '                       (Session("CestaCertificados").Rows(i)("NroCertificado").ToString.Trim) = (txtNroCertificadoAgregarCertificados.Text.Trim) Then
    '                        grdListaCertificados.DataSource = Session("CestaCertificados")
    '                        grdListaCertificados.DataBind()
    '                        LimpiarObjetosAgregarCertificado()
    '                        lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
    '                        grdListaCertificados.SelectedIndex = -1
    '                        Throw New Exception("Certificado ya registrado, seleccione otro item.")
    '                    End If
    '                Next
    '                AgregarCestaFirmas(cboCertificado.SelectedValue, cboCertificado.SelectedItem.Text, UCase(txtNroCertificadoAgregarCertificados.Text.Trim), txtFechaInicioAgregarCertificados.Text,
    '                                         txtFechaFinalAgregarCertificados.Text, "", "1", fStatusCertificados(txtFechaInicioAgregarCertificados.Text, txtFechaFinalAgregarCertificados.Text), "R", Session("CestaCertificados"))
    '                grdListaCertificados.DataSource = Session("CestaCertificados")
    '                grdListaCertificados.DataBind()
    '                LimpiarObjetosAgregarCertificado()
    '                grdListaCertificados.SelectedIndex = -1
    '                MyValidator.ErrorMessage = "Certificado agregado con éxito."
    '            Else
    '                lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
    '            End If
    '        End If
    '        ValidationSummary2.ValidationGroup = "vgrpValidar"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
    '        ValidationSummary6.ValidationGroup = "vgrpValidarAgregarCertificados"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarAgregarCertificados"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub btnAdicionarCertificado_Click(sender As Object, e As EventArgs) Handles btnAdicionarCertificado.Click
    '    Try
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        MyValidator.ErrorMessage = ""
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0683", strOpcionModulo, "CMMS")
    '        LimpiarObjetosAgregarCertificado()
    '        lblDescripcionAgregarCertificados.Text = cboCertificado.SelectedItem.Text
    '        lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
    '    Catch ex As Exception
    '        lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
    '        ValidationSummary6.ValidationGroup = "vgrpValidarAgregarCertificados"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarAgregarCertificados"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Function fValidarFirma() As Boolean
        If FirmaNeg.AsignarFirmaGetData("SELECT COUNT(*) FROM RRHH_ASIGNARFIRMA WHERE cIdUsuario = '" & cboUsuario.SelectedValue & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'").Rows(0).Item(0) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub btnAdicionarFirma_Click(sender As Object, e As EventArgs) Handles btnAdicionarFirma.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0683", strOpcionModulo, "CMMS")
            'LimpiarObjetosAgregarCertificado()
            LimpiarObjetosAgregarFirma()
            'lblDescripcionAgregarCertificados.Text = cboUsuario.SelectedItem.Text 'cboCertificado.SelectedItem.Text
            'lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
            If fValidarFirma() And cboUsuario.Enabled = True Then
                MyValidator.ErrorMessage = "Firma de usuario ya registrada, seleccione otro usuario."
                ValidationSummary2.ValidationGroup = "vgrpValidar"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidar"
                Me.Page.Validators.Add(MyValidator)
                Exit Sub
            End If
            lnk_mostrarPanelSubirFirma_ModalPopupExtender.Show()
        Catch ex As Exception
            lnk_mostrarPanelSubirFirma_ModalPopupExtender.Show()
            ValidationSummary4.ValidationGroup = "vgrpValidarAgregarFirmas"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarAgregarFirmas"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0680", strOpcionModulo, "CMMS")
            Me.grdLista.DataSource = FirmaNeg.AsignarFirmaListaBusqueda(cboFiltroFirma.SelectedValue, txtBuscarFirma.Text, Session("IdEmpresa"), "*")
            Me.grdLista.DataBind()
            pnlCabecera.Visible = True
            pnlContenido.Visible = False
            BloquearMantenimiento(True, False, True, False)
            'LO ACABO DE QUITAR JMUG: 31/08/2025
            'ValidationSummary1.ValidationGroup = "vgrpValidar"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidar"
            'Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Protected Sub lnkbtnNuevoCertificado_Click(sender As Object, e As EventArgs)
    '    FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0681", strOpcionModulo, "CMMS")
    '    hfdOperacionDetalle.Value = "N"
    '    LlenarDataFirma()
    '    ValidarTextoCertificado(True)
    '    lnk_mostrarPanelMantenimientoCertificado_ModalPopupExtender.Show()
    'End Sub

    'Protected Sub lnkbtnEditarCertificado_Click(sender As Object, e As EventArgs)
    '    FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0682", strOpcionModulo, "CMMS")
    '    hfdOperacionDetalle.Value = "E"
    '    LlenarDataFirma()
    '    ValidarTextoCertificado(True)
    '    lnk_mostrarPanelMantenimientoCertificado_ModalPopupExtender.Show()
    'End Sub

    'Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0679", strOpcionModulo, "CMMS")
    '        Dim Coleccion As New List(Of RRHH_ASIGNARCERTIFICADO)
    '        For i = 0 To Session("CestaCertificados").Rows.Count - 1
    '            Dim AsignarCertificado As New RRHH_ASIGNARCERTIFICADO
    '            'JMUG: 31/08/2025 AsignarCertificado.cIdPersonal = lblIdPersonal.Text
    '            AsignarCertificado.cIdTipoCertificado = Session("CestaCertificados").Rows(i)("Codigo").ToString.Trim
    '            AsignarCertificado.nItemAsignarCertificado = i + 1 'Session("CestaCertificados").Rows(i)("Item").ToString.Trim
    '            AsignarCertificado.vNumeroReferenciaAsignarCertificado = Session("CestaCertificados").Rows(i)("NroCertificado").ToString.Trim
    '            AsignarCertificado.dFechaVigenciaInicioAsignarCertificado = Session("CestaCertificados").Rows(i)("FechaVigenciaInicio").ToString.Trim
    '            AsignarCertificado.dFechaVigenciaFinalAsignarCertificado = Session("CestaCertificados").Rows(i)("FechaVigenciaFinal").ToString.Trim
    '            AsignarCertificado.vURLAsignarCertificado = Session("CestaCertificados").Rows(i)("UrlDescarga").ToString.Trim
    '            AsignarCertificado.bEstadoRegistroAsignarCertificado = Session("CestaCertificados").Rows(i)("Estado").ToString.Trim
    '            AsignarCertificado.cEstadoRegistroAsignarCertificado = Session("CestaCertificados").Rows(i)("EstadoGeneral").ToString.Trim

    '            Coleccion.Add(AsignarCertificado)
    '        Next

    '        Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '        LogAuditoria.cIdPaisOrigen = Session("IdPais")
    '        LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '        LogAuditoria.cIdLocal = Session("IdLocal")
    '        LogAuditoria.cIdUsuario = Session("IdUsuario")
    '        LogAuditoria.vIP1 = Session("IP1")
    '        LogAuditoria.vIP2 = Session("IP2")
    '        LogAuditoria.vPagina = Session("URL")
    '        LogAuditoria.vSesion = Session("IdSesion")

    '        LogAuditoria.cIdSistema = "CMMS" 'Session("IdSistema")
    '        LogAuditoria.cIdModulo = strOpcionModulo

    '        If hfdOperacion.Value = "N" Then
    '            If TipCertNeg.DetalleAsignarCertificadoInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
    '                MyValidator.ErrorMessage = "Transacción registrada con éxito"
    '                Me.grdListaFirmas.DataSource = Session("CestaFirmas")
    '                Me.grdListaFirmas.DataBind()
    '                ValidationSummary1.ValidationGroup = "vgrpValidar"
    '                MyValidator.IsValid = False
    '                MyValidator.ID = "ErrorPersonalizado"
    '                MyValidator.ValidationGroup = "vgrpValidar"
    '                Me.Page.Validators.Add(MyValidator)

    '                pnlCabecera.Visible = True
    '                pnlContenido.Visible = False

    '                BloquearMantenimiento(True, False, True, False)
    '                hfdOperacion.Value = "R"
    '                txtBuscarFirma.Focus()
    '            End If

    '        ElseIf hfdOperacion.Value = "E" Then

    '        End If

    '        Me.grdListaFirmas.DataSource = Nothing
    '        Me.grdListaFirmas.DataBind()
    '        Me.grdListaFirmas.DataSource = Session("CestaFirmas")
    '        Me.grdListaFirmas.DataBind()
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Private Sub btnAceptarSubirFirma_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirFirma.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If Not (fupSubirFirma.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
            Dim ext As String = fupSubirFirma.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"jpg", "png"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de archivo inválido.")

            'JMUG: 31/08/2025 Dim Valores() As String = hfdValoresCertificado.Value.ToString.Split("*")

            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0679", strOpcionModulo, "CMMS")
            'Dim Coleccion As New List(Of RRHH_ASIGNARFIRMA)
            'For i = 0 To Session("CestaFirmas").Rows.Count - 1
            '    Dim AsignarCertificado As New RRHH_ASIGNARCERTIFICADO
            '    'JMUG: 31/08/2025 AsignarCertificado.cIdPersonal = lblIdPersonal.Text
            '    AsignarCertificado.cIdTipoCertificado = Session("CestaCertificados").Rows(i)("Codigo").ToString.Trim
            '    AsignarCertificado.nItemAsignarCertificado = i + 1 'Session("CestaCertificados").Rows(i)("Item").ToString.Trim
            '    AsignarCertificado.vNumeroReferenciaAsignarCertificado = Session("CestaCertificados").Rows(i)("NroCertificado").ToString.Trim
            '    AsignarCertificado.dFechaVigenciaInicioAsignarCertificado = Session("CestaCertificados").Rows(i)("FechaVigenciaInicio").ToString.Trim
            '    AsignarCertificado.dFechaVigenciaFinalAsignarCertificado = Session("CestaCertificados").Rows(i)("FechaVigenciaFinal").ToString.Trim
            '    AsignarCertificado.vURLAsignarCertificado = Session("CestaCertificados").Rows(i)("UrlDescarga").ToString.Trim
            '    AsignarCertificado.bEstadoRegistroAsignarCertificado = Session("CestaCertificados").Rows(i)("Estado").ToString.Trim
            '    AsignarCertificado.cEstadoRegistroAsignarCertificado = Session("CestaCertificados").Rows(i)("EstadoGeneral").ToString.Trim

            '    Coleccion.Add(AsignarCertificado)
            'Next
            Dim Firma As New RRHH_ASIGNARFIRMA
            Firma.cIdUsuario = cboUsuario.SelectedValue
            Firma.dFechaRegistroAsignarFirma = Now
            Firma.vURLAsignarFirma = "Imagenes\Firmas\" & Session("IdEmpresa") & "-FI-" & Now.ToString("yyyy") & "-." & ext
            Firma.nItemAsignarFirma = 0
            Firma.cIdEmpresa = Session("IdEmpresa")
            Firma.bEstadoRegistroAsignarFirma = True

            If FirmaNeg.AsignarFirmaInserta(Firma) = 0 Then
                FirmaNeg.AsignarFirmaGetData("UPDATE RRHH_ASIGNARFIRMA SET bEstadoRegistroAsignarFirma = 0 WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdUsuario = '" & Firma.cIdUsuario & "' AND nItemAsignarFirma <> " & Firma.nItemAsignarFirma)
            End If


            Dim strNombreFirma As String = ""
            strNombreFirma = Session("IdEmpresa") & "-FI-" & Now.ToString("yyyy") & "-" & Firma.nItemAsignarFirma
            'Dim FuncionesNeg As New clsFuncionesNegocios
            'FuncionesNeg.GuardarArchivoGeneral(fupSubirFirma.PostedFile, "Firmas", Mid(fupSubirFirma.FileName, 1, InStrRev(fupSubirFirma.FileName, ".") - 1))
            FuncionesNeg.GuardarArchivoGeneral(fupSubirFirma.PostedFile, "Imagenes/Firmas", strNombreFirma)

            'JMUG: 31/08/2025 Lo quite momentaneamente para ver si sube la imagen
            'Dim rowIndexDetalle As Int32
            'Dim result As DataRow() = Session("CestaCertificados").Select("Codigo = '" & Valores(0).ToString & "' AND NroCertificado = '" & Valores(1).ToString & "'")
            'rowIndexDetalle = Session("CestaCertificados").Rows.IndexOf(result(0))
            'EditarCestaCertificados(Session("CestaCertificados").Rows(rowIndexDetalle)("Codigo"), Session("CestaCertificados").Rows(rowIndexDetalle)("Descripcion"), Session("CestaCertificados").Rows(rowIndexDetalle)("NroCertificado"),
            '                             Session("CestaCertificados").Rows(rowIndexDetalle)("FechaVigenciaInicio"), Session("CestaCertificados").Rows(rowIndexDetalle)("FechaVigenciaFinal"), fupSubirCertificado.FileName,
            '                             Session("CestaCertificados").Rows(rowIndexDetalle)("Estado"), Session("CestaCertificados").Rows(rowIndexDetalle)("Status"), Session("CestaCertificados").Rows(rowIndexDetalle)("EstadoGeneral"), Session("CestaCertificados"), rowIndexDetalle)

            'grdListaCertificados.DataSource = Session("CestaCertificados")
            'grdListaCertificados.DataBind()
            CargarCestaFirmas()
            cboUsuario.Enabled = False
            hfdOperacion.Value = "E"
            MyValidator.ErrorMessage = "Transacción registrada con éxito"
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirFirma_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnCancelarSubirFirma_Click(sender As Object, e As EventArgs) Handles btnCancelarSubirFirma.Click
        ValidationSummary4.ValidationGroup = "vgrpValidarSubirImagenEquipo"
        MyValidator.ErrorMessage = ""
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarSubirImagenEquipo"
    End Sub

    Private Sub btnSiMensajeDocumentoValidacion_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumentoValidacion.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            Dim Valores() As String = hfdValoresFirma.Value.ToString.Split("*")

            'JMUG: 31/08/2025 TipCertNeg.TipoCertificadoGetData("UPDATE RRHH_ASIGNARCERTIFICADO SET bEstadoRegistroAsignarCertificado = '0' WHERE cIdPersonal = '" & lblIdPersonal.Text & "' AND cIdTipoCertificado = '" & Valores(0).ToString & "' AND vNumeroReferenciaAsignarCertificado = '" & Valores(1).ToString & "'")
            FirmaNeg.AsignarFirmaGetData("UPDATE RRHH_ASIGNARFIRMA SET bEstadoRegistroAsignarFirma = '0' WHERE cIdUsuario = '" & Valores(1).ToString & "' AND nItemAsignarFirma <> '" & Valores(0).ToString & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'")
            FirmaNeg.AsignarFirmaGetData("UPDATE RRHH_ASIGNARFIRMA SET bEstadoRegistroAsignarFirma = '1' WHERE cIdUsuario = '" & Valores(1).ToString & "' AND nItemAsignarFirma = '" & Valores(0).ToString & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'")
            CargarCestaFirmas()

            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Transacción activada con éxito"
            End If

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = FirmaNeg.AsignarFirmaListaBusqueda(cboFiltroFirma.SelectedValue, txtBuscarFirma.Text, Session("IdEmpresa"), "*")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Private Sub imgbtnBuscarFirma_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarFirma.Click
        Me.grdLista.DataSource = FirmaNeg.AsignarFirmaListaBusqueda(cboFiltroFirma.SelectedValue, txtBuscarFirma.Text, Session("IdEmpresa"), "*")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0678", strOpcionModulo, "CMMS")

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            'hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(4).Text) = "True", "1", "0")
                            'If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                            '    Throw New Exception("Este registro se encuentra desactivado.")
                            'End If
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "E"

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ActivarObjetos(True)
            LlenarDataPersonal()

            pnlCabecera.Visible = False
            pnlContenido.Visible = True
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        Catch ex As Exception
            BloquearMantenimiento(True, False, True, False)
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try

    End Sub
End Class