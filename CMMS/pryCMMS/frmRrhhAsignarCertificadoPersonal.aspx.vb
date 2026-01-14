Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmRrhhAsignarCertificadoPersonal
    Inherits System.Web.UI.Page
    Dim PersonalNeg As New clsPersonalNegocios
    Dim TipCertNeg As New clsTipoCertificadoNegocios
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
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("NroCertificado", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("FechaVigenciaInicio", GetType(System.DateTime))) '4
        dt.Columns.Add(New DataColumn("FechaVigenciaFinal", GetType(System.DateTime))) '5
        dt.Columns.Add(New DataColumn("UrlDescarga", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '7
        dt.Columns.Add(New DataColumn("Status", GetType(System.String))) '8
        dt.Columns.Add(New DataColumn("EstadoGeneral", GetType(System.String))) '9
        Return dt
    End Function

    Shared Sub EditarCestaCertificados(ByVal Codigo As String, ByVal Descripcion As String, ByVal NroCertificado As String,
                           ByVal FechaVigenciaInicio As DateTime, ByVal FechaVigenciaFinal As DateTime, ByVal UrlDescarga As String,
                           ByVal Estado As Boolean, ByVal Status As String, ByVal EstadoGeneral As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(1) = Codigo
                Tabla.Rows(Fila)(2) = Descripcion
                Tabla.Rows(Fila)(3) = NroCertificado
                Tabla.Rows(Fila)(4) = FechaVigenciaInicio
                Tabla.Rows(Fila)(5) = FechaVigenciaFinal
                Tabla.Rows(Fila)(6) = UrlDescarga
                Tabla.Rows(Fila)(7) = Estado
                Tabla.Rows(Fila)(8) = Status
                Tabla.Rows(Fila)(9) = EstadoGeneral
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaCertificados(ByVal Codigo As String, ByVal Descripcion As String, ByVal NroCertificado As String,
                           ByVal FechaVigenciaInicio As DateTime, ByVal FechaVigenciaFinal As DateTime, ByVal UrlDescarga As String,
                           ByVal Estado As Boolean, ByVal Status As String, ByVal EstadoGeneral As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Tabla.Rows.Count + 1
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("NroCertificado") = NroCertificado
        Fila("FechaVigenciaInicio") = FechaVigenciaInicio
        Fila("FechaVigenciaFinal") = FechaVigenciaFinal
        Fila("UrlDescarga") = UrlDescarga
        Fila("Estado") = Estado
        Fila("Status") = Status
        Fila("EstadoGeneral") = EstadoGeneral
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaCertificados(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaCertificados(ByVal Tabla As DataTable)
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
        cboFiltroPersonal.DataTextField = "vDescripcionTablaSistema"
        cboFiltroPersonal.DataValueField = "vValor"
        cboFiltroPersonal.DataSource = FiltroNeg.TablaSistemaListarCombo("38", "RRHH", Session("IdEmpresa"))
        cboFiltroPersonal.Items.Clear()
        cboFiltroPersonal.DataBind()
    End Sub

    Sub ListarTipoDocumentoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboTipoDocumentoPersonal.DataTextField = "vDescripcionTablaSistema"
        cboTipoDocumentoPersonal.DataValueField = "vValor"
        cboTipoDocumentoPersonal.DataSource = FiltroNeg.TablaSistemaListarCombo("01", "CTBL", Session("IdEmpresa"))
        cboTipoDocumentoPersonal.Items.Clear()
        cboTipoDocumentoPersonal.DataBind()
    End Sub

    Sub ListarCertificadoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CertificadoNeg As New clsTipoCertificadoNegocios
        cboCertificado.DataTextField = "vDescripcionTipoCertificado"
        cboCertificado.DataValueField = "cIdTipoCertificado"
        cboCertificado.DataSource = CertificadoNeg.TipoCertificadoListarCombo()
        cboCertificado.Items.Clear()
        cboCertificado.DataBind()
    End Sub

    Sub ValidarTextoCertificado(ByVal bValidar As Boolean)
        Me.rfvDescripcionMantenimientoCertificado.EnableClientScript = bValidar
        Me.rfvDescripcionAbreviadaMantenimientoCertificado.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnGuardar.Visible = bGuardar
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        cboTipoDocumentoPersonal.Enabled = False 'bActivar
    End Sub

    Sub LlenarDataPersonal()
        Try
            Dim PersonalNeg As New clsPersonalNegocios
            Dim Personal As RRHH_PERSONAL = PersonalNeg.PersonalListarPorId(grdLista.SelectedRow.Cells(0).Text, Session("IdEmpresa"))
            lblIdPersonal.Text = Personal.cIdPersonal
            lblNombreCompletoPersonal.Text = Personal.vNombreCompletoPersonal
            cboTipoDocumentoPersonal.SelectedValue = Personal.cIdTipoDocumento
            lblNumeroDocumentoPersonal.Text = Personal.vNumeroDocumentoPersonal
            CargarCestaCertificados()

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

    Sub LlenarDataCertificado()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosCertificado()
            Else
                LimpiarObjetosCertificado()
                Dim CertificadoNeg As New clsTipoCertificadoNegocios
                Dim Certificado As RRHH_TIPOCERTIFICADO = CertificadoNeg.TipoCertificadoListarPorId(cboCertificado.SelectedValue)
                txtIdMantenimientoCertificado.Text = Certificado.cIdTipoCertificado
                txtDescripcionMantenimientoCertificado.Text = Certificado.vDescripcionTipoCertificado
                txtDescripcionAbreviadaMantenimientoCertificado.Text = Certificado.vDescripcionAbreviadaTipoCertificado
            End If
            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Registro encontrado con éxito"
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        lblIdPersonal.Text = ""
        lblNombreCompletoPersonal.Text = ""
        cboTipoDocumentoPersonal.SelectedIndex = -1
        lblNumeroDocumentoPersonal.Text = ""
    End Sub

    Sub LimpiarObjetosCertificado()
        MyValidator.ErrorMessage = ""
        txtIdMantenimientoCertificado.Text = ""
        txtDescripcionMantenimientoCertificado.Text = ""
        txtDescripcionAbreviadaMantenimientoCertificado.Text = ""
    End Sub

    Sub LimpiarObjetosAgregarCertificado()
        MyValidator.ErrorMessage = ""
        lblDescripcionAgregarCertificados.Text = ""
        txtNroCertificadoAgregarCertificados.Text = ""
        txtFechaInicioAgregarCertificados.Text = String.Format("{0:dd/MM/yyyy}", Now)
        txtFechaFinalAgregarCertificados.Text = String.Format("{0:dd/MM/yyyy}", Now)
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
            cboFiltroPersonal.SelectedIndex = 4
            ListarTipoDocumentoCombo()
            ListarCertificadoCombo()

            If Session("CestaCertificados") Is Nothing Then
                Session("CestaCertificados") = CrearCestaCertificados()
            Else
                VaciarCestaCertificados(Session("CestaCertificados"))
            End If

            BloquearMantenimiento(True, False, True, False)
            Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroPersonal.SelectedValue,
                                                                     txtBuscarPersonal.Text, Session("IdEmpresa"))
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
        Else
            txtBuscarPersonal.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0678", strOpcionModulo, "CMMS")

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text) = "True", "1", "0")
                            If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                                Throw New Exception("Este registro se encuentra desactivado.")
                            End If
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "N"

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()

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

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0684", strOpcionModulo, "CMMS")
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            lblIdPersonal.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text) = "True", "1", "0")
                            lnkbtnVerPersonal.Attributes.Add("onclick", "javascript:popupEmitirCertificadoPersonalReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "');")
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
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
        End If
    End Sub

    Protected Sub grdListaCertificados_RowCommand_Botones(sender As Object, e As GridViewCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If
            If e.CommandName = "Eliminar" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCertificado.Value = e.CommandArgument.ToString
                lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender.Show()
            ElseIf e.CommandName = "Adjuntar" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCertificado.Value = e.CommandArgument.ToString
                lnk_mostrarPanelSubirCertificado_ModalPopupExtender.Show()
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
            lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoCertificado_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoCertificado.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            Dim CertificadoNeg As New clsTipoCertificadoNegocios
            Dim Certificado As New RRHH_TIPOCERTIFICADO
            Certificado.cIdTipoCertificado = txtIdMantenimientoCertificado.Text
            Certificado.vDescripcionTipoCertificado = UCase(txtDescripcionMantenimientoCertificado.Text.Trim)
            Certificado.vDescripcionAbreviadaTipoCertificado = UCase(txtDescripcionAbreviadaMantenimientoCertificado.Text.Trim)
            Certificado.bEstadoRegistroTipoCertificado = True

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
            'LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")
            If hfdOperacionDetalle.Value = "N" Then
                If (CertificadoNeg.TipoCertificadoInserta(Certificado)) = 0 Then
                    Session("Query") = "PA_RRHH_MNT_TIPOCERTIFICADO 'SQL_INSERT', '','" & Certificado.cIdTipoCertificado & "', '" & Certificado.vDescripcionTipoCertificado & "', '" & Certificado.vDescripcionAbreviadaTipoCertificado & "', '" &
                                        Certificado.bEstadoRegistroTipoCertificado & "', '" & Certificado.cIdTipoCertificado & "'"
                    LogAuditoria.vEvento = "INSERTAR TIPO DE CERTIFICADO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo
                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)
                    txtIdMantenimientoCertificado.Text = Certificado.cIdTipoCertificado
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    BloquearMantenimiento(False, True, False, True)
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (CertificadoNeg.TipoCertificadoEdita(Certificado)) = 0 Then
                    Session("Query") = "PA_RRHH_MNT_TIPOCERTIFICADO 'SQL_UPDATE', '','" & Certificado.cIdTipoCertificado & "', '" & Certificado.vDescripcionTipoCertificado & "', '" & Certificado.vDescripcionAbreviadaTipoCertificado & "', '" &
                                        Certificado.bEstadoRegistroTipoCertificado & "', '" & Certificado.cIdTipoCertificado & "'"
                    LogAuditoria.vEvento = "ACTUALIZAR TIPO DE CERTIFICADO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo
                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)
                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    BloquearMantenimiento(False, True, False, True)
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacionDetalle.Value = "R"
            ListarCertificadoCombo()
            cboCertificado.SelectedValue = Certificado.cIdTipoCertificado
            pnlCabecera.Visible = False
            pnlContenido.Visible = True
            ValidarTextoCertificado(False)
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

    Function fStatusCertificados(ByVal dFechaInicial As DateTime, ByVal dFechaFinal As DateTime) As String
        fStatusCertificados = IIf(DateDiff(DateInterval.Day, Now, dFechaFinal) < 0, "VENCIDO", "EN VIGENCIA")
    End Function

    Sub CargarCestaCertificados()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaCertificados(Session("CestaCertificados"))

            Dim CertificadosNeg As New clsTipoCertificadoNegocios
            Dim dsCertificados = CertificadosNeg.TipoCertificadoGetData("SELECT ASICER.cIdPersonal, ASICER.cIdTipoCertificado, TIPCER.vDescripcionTipoCertificado, ASICER.dFechaVigenciaInicioAsignarCertificado, ASICER.dFechaVigenciaFinalAsignarCertificado, ASICER.vURLAsignarCertificado, ASICER.nItemAsignarCertificado, ASICER.vNumeroReferenciaAsignarCertificado, ASICER.bEstadoRegistroAsignarCertificado, ASICER.cEstadoRegistroAsignarCertificado " &
                                                                        "FROM RRHH_ASIGNARCERTIFICADO AS ASICER INNER JOIN RRHH_TIPOCERTIFICADO AS TIPCER ON " &
                                                                        "ASICER.cIdTipoCertificado = TIPCER.cIdTipoCertificado " &
                                                                        "WHERE ASICER.cIdPersonal = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' " &
                                                                        "      AND ASICER. bEstadoRegistroAsignarCertificado = '1'")
            For Each Certificados In dsCertificados.Rows
                AgregarCestaCertificados(Certificados("cIdTipoCertificado"), Certificados("vDescripcionTipoCertificado"), Certificados("vNumeroReferenciaAsignarCertificado"),
                                         Certificados("dFechaVigenciaInicioAsignarCertificado"), Certificados("dFechaVigenciaFinalAsignarCertificado"), Certificados("vURLAsignarCertificado"),
                                         Certificados("bEstadoRegistroAsignarCertificado"), fStatusCertificados(Certificados("dFechaVigenciaInicioAsignarCertificado"), Certificados("dFechaVigenciaFinalAsignarCertificado")), Certificados("cEstadoRegistroAsignarCertificado"), Session("CestaCertificados"))
            Next
            grdListaCertificados.DataSource = Session("CestaCertificados")
            grdListaCertificados.DataBind()
            LimpiarObjetosAgregarCertificado()
            grdListaCertificados.SelectedIndex = -1
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAceptarAgregarCertificados_Click(sender As Object, e As EventArgs) Handles btnAceptarAgregarCertificados.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
                If MyValidator.ErrorMessage = "" Then
                    For i = 0 To Session("CestaCertificados").Rows.Count - 1
                        If (Session("CestaCertificados").Rows(i)("Codigo").ToString.Trim) = (cboCertificado.SelectedValue.ToString.Trim) And
                           (Session("CestaCertificados").Rows(i)("NroCertificado").ToString.Trim) = (txtNroCertificadoAgregarCertificados.Text.Trim) Then
                            grdListaCertificados.DataSource = Session("CestaCertificados")
                            grdListaCertificados.DataBind()
                            LimpiarObjetosAgregarCertificado()
                            lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
                            grdListaCertificados.SelectedIndex = -1
                            Throw New Exception("Certificado ya registrado, seleccione otro item.")
                        End If
                    Next
                    AgregarCestaCertificados(cboCertificado.SelectedValue, cboCertificado.SelectedItem.Text, UCase(txtNroCertificadoAgregarCertificados.Text.Trim), txtFechaInicioAgregarCertificados.Text,
                                             txtFechaFinalAgregarCertificados.Text, "", "1", fStatusCertificados(txtFechaInicioAgregarCertificados.Text, txtFechaFinalAgregarCertificados.Text), "R", Session("CestaCertificados"))
                    grdListaCertificados.DataSource = Session("CestaCertificados")
                    grdListaCertificados.DataBind()
                    LimpiarObjetosAgregarCertificado()
                    grdListaCertificados.SelectedIndex = -1
                    MyValidator.ErrorMessage = "Certificado agregado con éxito."
                Else
                    lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
                End If
            End If
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
            ValidationSummary6.ValidationGroup = "vgrpValidarAgregarCertificados"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarAgregarCertificados"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarCertificado_Click(sender As Object, e As EventArgs) Handles btnAdicionarCertificado.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0683", strOpcionModulo, "CMMS")
            LimpiarObjetosAgregarCertificado()
            lblDescripcionAgregarCertificados.Text = cboCertificado.SelectedItem.Text
            lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
        Catch ex As Exception
            lnk_mostrarPanelAgregarCertificados_ModalPopupExtender.Show()
            ValidationSummary6.ValidationGroup = "vgrpValidarAgregarCertificados"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarAgregarCertificados"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0680", strOpcionModulo, "CMMS")
            Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroPersonal.SelectedValue,
                                                                     txtBuscarPersonal.Text, Session("IdEmpresa"))
            Me.grdLista.DataBind()
            pnlCabecera.Visible = True
            pnlContenido.Visible = False
            BloquearMantenimiento(True, False, True, False)
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

    Protected Sub lnkbtnNuevoCertificado_Click(sender As Object, e As EventArgs)
        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0681", strOpcionModulo, "CMMS")
        hfdOperacionDetalle.Value = "N"
        LlenarDataCertificado()
        ValidarTextoCertificado(True)
        lnk_mostrarPanelMantenimientoCertificado_ModalPopupExtender.Show()
    End Sub

    Protected Sub lnkbtnEditarCertificado_Click(sender As Object, e As EventArgs)
        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0682", strOpcionModulo, "CMMS")
        hfdOperacionDetalle.Value = "E"
        LlenarDataCertificado()
        ValidarTextoCertificado(True)
        lnk_mostrarPanelMantenimientoCertificado_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0679", strOpcionModulo, "CMMS")
            Dim Coleccion As New List(Of RRHH_ASIGNARCERTIFICADO)
            For i = 0 To Session("CestaCertificados").Rows.Count - 1
                Dim AsignarCertificado As New RRHH_ASIGNARCERTIFICADO
                AsignarCertificado.cIdPersonal = lblIdPersonal.Text
                AsignarCertificado.cIdTipoCertificado = Session("CestaCertificados").Rows(i)("Codigo").ToString.Trim
                AsignarCertificado.nItemAsignarCertificado = i + 1 'Session("CestaCertificados").Rows(i)("Item").ToString.Trim
                AsignarCertificado.vNumeroReferenciaAsignarCertificado = Session("CestaCertificados").Rows(i)("NroCertificado").ToString.Trim
                AsignarCertificado.dFechaVigenciaInicioAsignarCertificado = Session("CestaCertificados").Rows(i)("FechaVigenciaInicio").ToString.Trim
                AsignarCertificado.dFechaVigenciaFinalAsignarCertificado = Session("CestaCertificados").Rows(i)("FechaVigenciaFinal").ToString.Trim
                AsignarCertificado.vURLAsignarCertificado = Session("CestaCertificados").Rows(i)("UrlDescarga").ToString.Trim
                AsignarCertificado.bEstadoRegistroAsignarCertificado = Session("CestaCertificados").Rows(i)("Estado").ToString.Trim
                AsignarCertificado.cEstadoRegistroAsignarCertificado = Session("CestaCertificados").Rows(i)("EstadoGeneral").ToString.Trim

                Coleccion.Add(AsignarCertificado)
            Next

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            LogAuditoria.cIdSistema = "CMMS" 'Session("IdSistema")
            LogAuditoria.cIdModulo = strOpcionModulo

            If hfdOperacion.Value = "N" Then
                If TipCertNeg.DetalleAsignarCertificadoInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    Me.grdListaCertificados.DataSource = Session("CestaCertificados")
                    Me.grdListaCertificados.DataBind()
                    ValidationSummary1.ValidationGroup = "vgrpValidar"
                    MyValidator.IsValid = False
                    MyValidator.ID = "ErrorPersonalizado"
                    MyValidator.ValidationGroup = "vgrpValidar"
                    Me.Page.Validators.Add(MyValidator)

                    pnlCabecera.Visible = True
                    pnlContenido.Visible = False

                    BloquearMantenimiento(True, False, True, False)
                    hfdOperacion.Value = "R"
                    txtBuscarPersonal.Focus()
                End If

            ElseIf hfdOperacion.Value = "E" Then

            End If

            Me.grdListaCertificados.DataSource = Nothing
            Me.grdListaCertificados.DataBind()
            Me.grdListaCertificados.DataSource = Session("CestaCertificados")
            Me.grdListaCertificados.DataBind()
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

    Private Sub btnAceptarSubirCertificado_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirCertificado.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If Not (fupSubirCertificado.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
            Dim ext As String = fupSubirCertificado.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"pdf"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de archivo inválido.")

            Dim Valores() As String = hfdValoresCertificado.Value.ToString.Split("*")

            Dim FuncionesNeg As New clsFuncionesNegocios
            FuncionesNeg.GuardarArchivoGeneral(fupSubirCertificado.PostedFile, "DocCertificadoPDF", Mid(fupSubirCertificado.FileName, 1, InStrRev(fupSubirCertificado.FileName, ".") - 1))

            Dim rowIndexDetalle As Int32
            Dim result As DataRow() = Session("CestaCertificados").Select("Codigo = '" & Valores(0).ToString & "' AND NroCertificado = '" & Valores(1).ToString & "'")
            rowIndexDetalle = Session("CestaCertificados").Rows.IndexOf(result(0))
            EditarCestaCertificados(Session("CestaCertificados").Rows(rowIndexDetalle)("Codigo"), Session("CestaCertificados").Rows(rowIndexDetalle)("Descripcion"), Session("CestaCertificados").Rows(rowIndexDetalle)("NroCertificado"),
                                         Session("CestaCertificados").Rows(rowIndexDetalle)("FechaVigenciaInicio"), Session("CestaCertificados").Rows(rowIndexDetalle)("FechaVigenciaFinal"), fupSubirCertificado.FileName,
                                         Session("CestaCertificados").Rows(rowIndexDetalle)("Estado"), Session("CestaCertificados").Rows(rowIndexDetalle)("Status"), Session("CestaCertificados").Rows(rowIndexDetalle)("EstadoGeneral"), Session("CestaCertificados"), rowIndexDetalle)

            grdListaCertificados.DataSource = Session("CestaCertificados")
            grdListaCertificados.DataBind()

            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary5.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirCertificado_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnCancelarSubirCertificado_Click(sender As Object, e As EventArgs) Handles btnCancelarSubirCertificado.Click
        ValidationSummary5.ValidationGroup = "vgrpValidarSubirImagenEquipo"
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
            Dim Valores() As String = hfdValoresCertificado.Value.ToString.Split("*")

            TipCertNeg.TipoCertificadoGetData("UPDATE RRHH_ASIGNARCERTIFICADO SET bEstadoRegistroAsignarCertificado = '0' WHERE cIdPersonal = '" & lblIdPersonal.Text & "' AND cIdTipoCertificado = '" & Valores(0).ToString & "' AND vNumeroReferenciaAsignarCertificado = '" & Valores(1).ToString & "'")
            CargarCestaCertificados()

            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Transacción eliminada con éxito"
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
        Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroPersonal.SelectedValue,
                                                                     txtBuscarPersonal.Text, Session("IdEmpresa"))
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub
End Class