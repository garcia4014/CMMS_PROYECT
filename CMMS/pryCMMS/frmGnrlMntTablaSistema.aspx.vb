Imports CapaNegocioCMMS
Imports CapaDatosCMMS
'----------------------------------

Public Class frmGnrlMntTablaSistema
    Inherits System.Web.UI.Page
    Dim TablaSistemaNeg As New clsTablaSistemaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
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

            Dim UsuarioNeg As New CapaNegocioCMMS.clsUsuarioNegocios
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

        'INICIO: Novedades en el perfil.
        Dim mpLabelNovedades As Label
        Dim dtNovedades = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                         "FROM GNRL_USUARIO USU ")
        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelNovedades = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblNovedadesNotificacion"), Label)
            If Not mpLabelNovedades Is Nothing Then
                If dtNovedades.Rows(0).Item(0) > 0 Then
                    mpLabelNovedades.Visible = False 'mpLabelNovedades.Visible = True
                    mpLabelNovedades.Text = dtNovedades.Rows(0).Item(0)
                Else
                    mpLabelNovedades.Visible = False
                End If
            End If
        End If
        'FIN: Invitación de Amigos

        'INICIO: Mis Alertas
        Dim mpLabelAlerta As Label
        Dim dtAlertas = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                      "FROM GNRL_ALERTA ALE " &
                                                      "WHERE ALE.bEstadoRegistroAlerta = 1 AND ALE.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                      "      AND (DATEDIFF(dd, GETDATE(), ALE.dFechaHoraAlerta) <= 5 And Month(ALE.dFechaHoraAlerta) = Month(GETDATE())) AND (DATEDIFF(mi, GETDATE(), dFechaHoraAlerta)) >= 0 ")
        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelAlerta = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblAlertasNotificacion"), Label)
            If Not mpLabelAlerta Is Nothing Then
                If dtAlertas.Rows(0).Item(0) > 0 Then
                    mpLabelAlerta.Visible = True
                    mpLabelAlerta.Text = dtAlertas.Rows(0).Item(0)
                Else
                    mpLabelAlerta.Visible = False
                End If
            End If
        End If
        'FIN: Mis Alertas

        'INICIO: Notificación de Eventos
        Dim mpLabelPreguntasFrecuentes As Label
        Dim dtEventoNotificacion = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                         "FROM GNRL_PREGUNTASFRECUENTES " &
                                                         "WHERE DATEDIFF(dd, dFechaCreacionPreguntasFrecuentes, GETDATE()) < 15")

        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelPreguntasFrecuentes = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblPreguntasFrecuentesNotificacion"), Label)
            If Not mpLabelPreguntasFrecuentes Is Nothing Then
                If dtEventoNotificacion.Rows(0).Item(0) > 0 Then
                    mpLabelPreguntasFrecuentes.Visible = True
                    mpLabelPreguntasFrecuentes.Text = dtEventoNotificacion.Rows(0).Item(0)
                Else
                    mpLabelPreguntasFrecuentes.Visible = False
                End If
            End If
        End If
        'FIN: Notificación de Eventos

        'INICIO: Notificación de Mensajes
        Dim mpLabelMensaje As Label
        Dim dtMensajeNotificacion = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                                  "FROM GNRL_MENSAJE MSJ " &
                                                                  "WHERE MSJ.cIdEmpresa = '" & Session("IdEmpresa") & "' AND " &
                                                                  "      MSJ.cIdPuntoVenta = '" & Session("IdPuntoVenta") & "' AND " &
                                                                  "      MSJ.bEstadoLeidoMensaje = 0 AND MSJ.vIdParaMensaje = '" & Session("IdUsuario") & "' AND " &
                                                                  "      MSJ.bEstadoRegistroMensaje = 1 ")
        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelMensaje = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblMensajeNotificacion"), Label)
            If Not mpLabelMensaje Is Nothing Then
                If dtMensajeNotificacion.Rows(0).Item(0) > 0 Then
                    mpLabelMensaje.Visible = True
                    mpLabelMensaje.Text = dtMensajeNotificacion.Rows(0).Item(0)
                Else
                    mpLabelMensaje.Visible = False
                End If
            End If
        End If
        'FIN: Notificación de Mensajes
    End Sub

    Function fValidarSesion() As Boolean
        fValidarSesion = False
        If Session("IdUsuario") = "" Then
            fValidarSesion = True
            Throw New Exception("Su sesión ha caducado, ingrese de nuevo por favor.")
        End If
        If Session("IdPuntoVenta") <> "" Or Session("IdEmpresa") <> "" Then
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
            End If
        End If
    End Function

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltro.DataTextField = "vDescripcionTablaSistema"
        cboFiltro.DataValueField = "vValor"
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("29", "GNRL", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ListarPeriodoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PeriodoNeg As New clsTipoCambioNegocios
        cboPeriodo.DataSource = PeriodoNeg.TipoCambioPeriodoListarCombo
        cboPeriodo.Items.Clear()
        cboPeriodo.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtCodigo.Enabled = False
        txtDescripcion.Enabled = bActivar
        txtDescripcionAbreviada.Enabled = bActivar
        txtValor.Enabled = bActivar
    End Sub

    Private Function BloquearPagina(ByVal NroPagina As Integer) As Boolean
        BloquearPagina = True
        If NroPagina = 1 Then
            pnlListado.Enabled = True
            pnlGeneral.Enabled = False
            txtBuscar.Focus()
        ElseIf NroPagina = 2 Then
            If hfdOperacion.Value = "R" Or hfdOperacion.Value = "E" Or IsNothing(hfdOperacion.Value) = True Then
                If grdLista.Rows.Count = 0 Then
                Else
                    If IsNothing(grdLista.SelectedRow) = True Then
                        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                        MyValidator.ErrorMessage = "Seleccione un registro a visualizar."
                        MyValidator.IsValid = False
                        MyValidator.ID = "ErrorPersonalizado"
                        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                        Me.Page.Validators.Add(MyValidator)
                        hfdOperacion.Value = "R"
                    Else
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            If hfdOperacion.Value = "R" Then
                                pnlListado.Enabled = True
                                LlenarData()
                            Else
                                pnlListado.Enabled = False
                            End If
                            pnlGeneral.Enabled = False
                            If hfdOperacion.Value = "E" Then pnlGeneral.Enabled = True
                            txtDescripcion.Focus()
                            BloquearPagina = False
                        End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "N" Then
                pnlListado.Enabled = False
                pnlGeneral.Enabled = True
                txtDescripcion.Focus()
                BloquearPagina = False
            End If
        ElseIf NroPagina = 0 Then
            If grdLista.Rows.Count = 0 Then
            Else
                If InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 1 Then
                    pnlListado.Enabled = True
                    pnlGeneral.Enabled = False
                    txtBuscar.Focus()
                    strTabContenedorActivo = 1
                ElseIf InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 0 Then
                    pnlListado.Enabled = True
                    pnlGeneral.Enabled = False
                    txtBuscar.Focus()
                    strTabContenedorActivo = 0
                Else
                    If InStr(MyValidator.ErrorMessage, "eliminar") > 0 And strTabContenedorActivo = 0 Then
                        pnlListado.Enabled = True
                        pnlGeneral.Enabled = False
                    Else
                    End If
                End If
            End If
            pnlListado.Enabled = True
            pnlGeneral.Enabled = False
        End If
    End Function

    Sub LlenarData()
        Try
            grdParametros.DataSource = Nothing
            grdParametros.DataSource = TablaSistemaNeg.TablaSistemaListaBusqueda("cIdTablaSistema", grdLista.SelectedRow.Cells(2).Text, IIf(grdLista.SelectedRow.Cells(3).Text = "0000", "0000", cboPeriodo.SelectedValue), Session("IdSistema"), Session("IdEmpresa"), "*")
            grdParametros.DataBind() 'Recargo el grid.
            If grdParametros.Rows.Count > 0 Then
                grdParametros.SelectedIndex = 0
            Else
                txtCodigo.Text = ""
                txtDescripcion.Text = ""
                txtDescripcionAbreviada.Text = ""
                txtValor.Text = ""
                hfdbEstadoEncryptado.Value = "0"
                Throw New Exception("Registro no generado para el periodo " & cboPeriodo.SelectedValue & "...!!!")
            End If

            If IsNothing(grdParametros.SelectedRow) = False Then
                If IsReference(grdParametros.SelectedRow.Cells(1).Text) = True Then
                    Dim TablaSistema As GNRL_TABLASISTEMA = TablaSistemaNeg.TablaSistemaListarPorId(grdLista.SelectedRow.Cells(2).Text, grdParametros.SelectedRow.Cells(2).Text, Session("IdSistema"), Session("IdEmpresa"))
                    If IsNothing(TablaSistema.bEstadoEncryptadoTablaSistema) = True Then
                        hfdbEstadoEncryptado.Value = "0"
                    Else
                        hfdbEstadoEncryptado.Value = IIf(TablaSistema.bEstadoEncryptadoTablaSistema = True, "1", "0")
                    End If
                    txtCodigo.Text = TablaSistema.vValor
                    txtDescripcion.Text = TablaSistema.vDescripcionTablaSistema
                    txtDescripcionAbreviada.Text = TablaSistema.vDescripcionAbreviadaTablaSistema
                    hfdEstado.Value = IIf(TablaSistema.bEstadoRegistroTablaSistema = False, "0", "1")
                    If hfdbEstadoEncryptado.Value = "1" Then
                        txtValor.Text = FuncionesNeg.Desencriptar(TablaSistema.vValorOpcionalTablaSistema)
                    Else
                        txtValor.Text = TablaSistema.vValorOpcionalTablaSistema
                    End If
                End If
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

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvCodigo.EnableClientScript = bValidar
        Me.rfvDescripcion.EnableClientScript = bValidar
        Me.rfvDescripcionAbreviada.EnableClientScript = bValidar
        Me.rfvValor.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtDescripcionAbreviada.Text = ""
        txtValor.Text = ""
        hfdEstado.Value = "1"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            strOpcionModulo = "083" 'Mantenimiento de Tabla Sistema - Contabilidad.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 1
            ListarPeriodoCombo()

            cboPeriodo.SelectedValue = Year((String.Format("{0:dd/MM/yyyy}", Now))).ToString()


            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            imgbtnCargarCertificado.Visible = False

            Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue,
                                                                   txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1")) '"0000")
            Me.grdLista.DataBind()
        Else
            cboPeriodo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanListado_cboFiltro')")
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanListado_imgbtnBuscarTablaSistema')")
            imgbtnBuscarTablaSistema.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanGeneral_grdParametros')")
            grdParametros.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanGeneral_txtCodigo')")
            txtCodigo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanGeneral_txtDescripcionAbreviada')")
            txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTablaSistema_TabPanGeneral_txtValor')")
            txtValor.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnGuardar')")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue,
                                               txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue,
                                                       txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub grdParametros_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdParametros.SelectedIndexChanged
        imgbtnCargarCertificado.Visible = False
        If IsNothing(grdParametros.SelectedRow) = False Then
            If IsReference(grdParametros.SelectedRow.Cells(1).Text) = True Then
                If grdParametros.SelectedRow.Cells(1).Text = "44" And grdParametros.SelectedRow.Cells(2).Text = "04" Then
                    imgbtnCargarCertificado.Visible = True
                End If
                Dim TablaSistema As GNRL_TABLASISTEMA = TablaSistemaNeg.TablaSistemaListarPorId(grdLista.SelectedRow.Cells(2).Text, grdParametros.SelectedRow.Cells(2).Text, Session("IdSistema"), Session("IdEmpresa"), IIf(grdLista.SelectedRow.Cells(3).Text = "0000", "0000", cboPeriodo.SelectedValue))
                If IsNothing(TablaSistema.bEstadoEncryptadoTablaSistema) = True Then
                    hfdbEstadoEncryptado.Value = "0"
                Else
                    hfdbEstadoEncryptado.Value = IIf(TablaSistema.bEstadoEncryptadoTablaSistema = True, "1", "0")
                End If
                txtCodigo.Text = TablaSistema.vValor
                txtDescripcion.Text = TablaSistema.vDescripcionTablaSistema
                txtDescripcionAbreviada.Text = TablaSistema.vDescripcionAbreviadaTablaSistema
                If hfdbEstadoEncryptado.Value = "1" Then
                    If TablaSistema.vValorOpcionalTablaSistema = "" Then
                        txtValor.Text = ""
                    Else
                        txtValor.Text = FuncionesNeg.Desencriptar(TablaSistema.vValorOpcionalTablaSistema)
                    End If
                Else
                    txtValor.Text = TablaSistema.vValorOpcionalTablaSistema
                End If
            End If
        End If
    End Sub

    Protected Sub cboPeriodo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPeriodo.SelectedIndexChanged
        Me.grdLista.DataSource = Nothing
        Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue,
                                               txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue,
                                               txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowDataBound
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

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdLista.SelectedIndexChanged
        MyValidator.ErrorMessage = ""
        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(6).Text) = "True", "1", "0")
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    Protected Sub btnCargarCertificado_Click(sender As Object, e As EventArgs) Handles btnCargarCertificado.Click
        Try
            lblMensajeSeleccionarCertificado.Text = ""

            If Not (fupCertificado.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
            Dim ext As String = fupCertificado.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"pfx"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de certificado inválido.")

            'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
            Dim FuncionesNeg As New clsFuncionesNegocios
            FuncionesNeg.GuardarArchivoGeneral(fupCertificado.PostedFile, "Certificados", Mid(fupCertificado.FileName, 1, InStrRev(fupCertificado.FileName, ".") - 1))
            txtValor.Text = Mid(fupCertificado.FileName, 1, InStrRev(fupCertificado.FileName, ".") - 1)
        Catch ex As Exception
            lblMensajeSeleccionarCertificado.Text = ex.Message
            lnk_mostrarPanelCertificado_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub imgbtnCargarCertificado_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnCargarCertificado.Click
        lnk_mostrarPanelCertificado_ModalPopupExtender.Show()
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim TablaSistema As New GNRL_TABLASISTEMA
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0285", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                TablaSistema.cIdTablaSistema = Valores(0).ToString()
                TablaSistema.vValor = Valores(1).ToString
                TablaSistema.cIdPeriodoTablaSistema = Valores(2).ToString
                TablaSistema.cIdSistema = Valores(3).ToString
                TablaSistema.cIdEmpresa = Session("IdEmpresa")

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                TablaSistemaNeg.TablaSistemaGetData("UPDATE GNRL_TABLASISTEMA SET bEstadoRegistroTablaSistema = 0 WHERE cIdTablaSistema = '" & TablaSistema.cIdTablaSistema & "' AND vValor = '" & TablaSistema.vValor & "' AND cIdPeriodoTablaSistema = '" & TablaSistema.cIdPeriodoTablaSistema & "' AND cIdSistema = '" & TablaSistema.cIdSistema & "' AND cIdEmpresa = '" & TablaSistema.cIdEmpresa & "'")
                Session("Query") = "UPDATE GNRL_TABLASISTEMA SET bEstadoRegistroTablaSistema = 0 WHERE cIdTablaSistema = '" & TablaSistema.cIdTablaSistema & "' AND vValor = '" & TablaSistema.vValor & "' AND cIdPeriodoTablaSistema = '" & TablaSistema.cIdPeriodoTablaSistema & "' AND cIdSistema = '" & TablaSistema.cIdSistema & "' AND cIdEmpresa = '" & TablaSistema.cIdEmpresa & "'"
                LogAuditoria.vEvento = "DESACTIVAR TABLA SISTEMA"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                MyValidator.ErrorMessage = "Registro desactivado con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                Me.Page.Validators.Add(MyValidator)

                Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue, txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), "*") '"0000"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim TablaSistema As New GNRL_TABLASISTEMA
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0285", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                TablaSistema.cIdTablaSistema = Valores(0).ToString()
                TablaSistema.vValor = Valores(1).ToString
                TablaSistema.cIdPeriodoTablaSistema = Valores(2).ToString
                TablaSistema.cIdSistema = Valores(3).ToString
                TablaSistema.cIdEmpresa = Session("IdEmpresa")

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                TablaSistemaNeg.TablaSistemaGetData("UPDATE GNRL_TABLASISTEMA SET bEstadoRegistroTablaSistema = 1 WHERE cIdTablaSistema = '" & TablaSistema.cIdTablaSistema & "' AND vValor = '" & TablaSistema.vValor & "' AND cIdPeriodoTablaSistema = '" & TablaSistema.cIdPeriodoTablaSistema & "' AND cIdSistema = '" & TablaSistema.cIdSistema & "' AND cIdEmpresa = '" & TablaSistema.cIdEmpresa & "'")
                Session("Query") = "UPDATE GNRL_TABLASISTEMA SET bEstadoRegistroTablaSistema = 1 WHERE cIdTablaSistema = '" & TablaSistema.cIdTablaSistema & "' AND vValor = '" & TablaSistema.vValor & "' AND cIdPeriodoTablaSistema = '" & TablaSistema.cIdPeriodoTablaSistema & "' AND cIdSistema = '" & TablaSistema.cIdSistema & "' AND cIdEmpresa = '" & TablaSistema.cIdEmpresa & "'"
                LogAuditoria.vEvento = "ACTIVAR TABLA SISTEMA"

                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                MyValidator.ErrorMessage = "Registro activado con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                Me.Page.Validators.Add(MyValidator)

                Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue, txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1")) '"0000"))
                Me.grdLista.DataBind()
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

    Private Sub imgbtnBuscarTablaSistema_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarTablaSistema.Click
        Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue, txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1")) '"0000"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0281", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ValidarTexto(True)
            ActivarObjetos(True)
            hfTab.Value = "tab2"
            ValidationSummary1.ValidationGroup = "vgrpValidar"
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                Throw New Exception("Este registro se encuentra desactivado.")
            End If
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0282", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "E"
            If BloquearPagina(2) = False Then
                BloquearMantenimiento(False, True, False, True)
                ValidarTexto(True)
                ActivarObjetos(True)
                LlenarData()
                hfTab.Value = "tab2"
                ValidationSummary1.ValidationGroup = "vgrpValidar"
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub btnDeshacer_Click(sender As Object, e As EventArgs) Handles btnDeshacer.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0284", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "R"
            BloquearPagina(0)
            BloquearMantenimiento(True, False, True, False)
            ValidarTexto(False)
            ActivarObjetos(False)
            hfTab.Value = "tab1"
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0283", strOpcionModulo, Session("IdSistema"))

            Dim TablaSistema As New GNRL_TABLASISTEMA
            With TablaSistema
                .cIdTablaSistema = grdLista.SelectedRow.Cells(2).Text
                .cIdPeriodoTablaSistema = IIf(grdLista.SelectedRow.Cells(3).Text = "0000", "0000", cboPeriodo.SelectedValue)
                .bEstadoRegistroTablaSistema = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
                .vDescripcionAbreviadaTablaSistema = UCase(txtDescripcionAbreviada.Text.Trim)
                .vDescripcionTablaSistema = UCase(txtDescripcion.Text.Trim)
                .vValor = IIf(hfdbEstadoEncryptado.Value = "1", txtCodigo.Text.Trim, UCase(txtCodigo.Text.Trim))
                .vValorOpcionalTablaSistema = IIf(hfdbEstadoEncryptado.Value = "1", FuncionesNeg.Encriptar(txtValor.Text.Trim), IIf(txtCodigo.Text.Trim = "19", txtValor.Text.Trim, UCase(txtValor.Text.Trim)))
                .cIdSistema = Session("IdSistema")
                .cIdEmpresa = Session("IdEmpresa")
                .bEstadoEncryptadoTablaSistema = hfdbEstadoEncryptado.Value
            End With

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacion.Value = "N" Then
                If (TablaSistemaNeg.TablaSistemaInserta(TablaSistema)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_TABLASISTEMA 'SQL_INSERT', '','" & TablaSistema.cIdTablaSistema & "', '" &
                                       TablaSistema.vValor & "', '" & TablaSistema.vDescripcionTablaSistema & "', '" &
                                       TablaSistema.vDescripcionAbreviadaTablaSistema & "', '" & TablaSistema.vValorOpcionalTablaSistema & "', '" &
                                       TablaSistema.bEstadoRegistroTablaSistema & "', '" & TablaSistema.cIdPeriodoTablaSistema & "', '" &
                                       TablaSistema.cIdSistema & "', '" & TablaSistema.bEstadoEncryptadoTablaSistema & "', '" & TablaSistema.cIdTablaSistema & "'"
                    LogAuditoria.vEvento = "INSERTAR TABLA SISTEMA"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtCodigo.Text = TablaSistema.vValor
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue,
                                             txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()

                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (TablaSistemaNeg.TablaSistemaEdita(TablaSistema)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_TABLASISTEMA 'SQL_UPDATE', '','" & TablaSistema.cIdTablaSistema & "', '" &
                                       TablaSistema.vValor & "', '" & TablaSistema.vDescripcionTablaSistema & "', '" &
                                       TablaSistema.vDescripcionAbreviadaTablaSistema & "', '" & TablaSistema.vValorOpcionalTablaSistema & "', '" &
                                       TablaSistema.bEstadoRegistroTablaSistema & "', '" & TablaSistema.cIdPeriodoTablaSistema & "', '" &
                                       TablaSistema.cIdSistema & "', '" & TablaSistema.bEstadoEncryptadoTablaSistema & "', '" & TablaSistema.cIdTablaSistema & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR TABLA SISTEMA"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.grdLista.DataSource = TablaSistemaNeg.TablaSistemaListaBusquedaPrincipal(cboFiltro.SelectedValue,
                                             txtBuscar.Text, cboPeriodo.SelectedValue, Session("IdSistema"), Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()

                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacion.Value = "R"
            BloquearPagina(0)
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
End Class