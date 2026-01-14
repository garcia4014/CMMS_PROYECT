Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmGnrlMntCorrelativoDocumento
    Inherits System.Web.UI.Page
    Dim CorrelativoNeg As New clsCorrelativoDocumentoNegocios
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
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("42", "VTAS", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtFechaVigencia.Enabled = bActivar
        txtNroCorrelativoDesde.Enabled = bActivar
        txtNroCorrelativoHasta.Enabled = bActivar
        txtNroDigitoSerie.Enabled = bActivar
        txtNroDigitoDoc.Enabled = bActivar
        txtNroSerie.Enabled = bActivar
        txtSerieAlfanumerico.Enabled = bActivar 'IIf(hfdOperacion.Value = "E", False, bActivar)
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
                            txtNroSerie.Focus()
                            BloquearPagina = False
                        End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "N" Then
                pnlListado.Enabled = False
                pnlGeneral.Enabled = True
                txtNroSerie.Focus()
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
            Dim Correlativo As GNRL_CORRELATIVODOCUMENTO = CorrelativoNeg.CorrelativoListarPorId(Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text), (Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text)).ToString.Trim, Session("IdEmpresa"), Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text), IIf(grdLista.SelectedRow.Cells(7).Text = "True", 1, 0))
            txtNroSerie.Text = Correlativo.nNumeroSerieCorrelativoDocumento
            txtNroCorrelativoActual.Text = Correlativo.nNumeroCorrelativoActualCorrelativoDocumento.ToString
            txtNroCorrelativoDesde.Text = Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento
            txtNroCorrelativoHasta.Text = Correlativo.nNumeroCorrelativoHastaCorrelativoDocumento
            txtNroDigitoSerie.Text = Correlativo.nNumeroDigitoSerieCorrelativoDocumento
            txtNroDigitoDoc.Text = Correlativo.nNumeroDigitoDocumentoCorrelativoDocumento
            txtFechaVigencia.Text = Correlativo.dFechaVigenciaCorrelativoDocumento
            cboTipoDoc.SelectedValue = Correlativo.cIdTipoDocumento
            chkFacturacionElectronica.Checked = IIf(Correlativo.bFacturacionElectronicaCorrelativoDocumento = True, True, False)
            chkFacturacionElectronica_CheckedChanged(chkFacturacionElectronica, New System.EventArgs())
            cboTipoDocumentoReferencial.SelectedValue = IIf(Correlativo.cIdTipoDocumentoRefCorrelativoDocumento.ToString.Trim = "", "SELECCIONE DATO", Correlativo.cIdTipoDocumentoRefCorrelativoDocumento)
            txtSerieAlfanumerico.Text = IIf(Correlativo.bFacturacionElectronicaCorrelativoDocumento = True, Correlativo.vSerieCorrelativoDocumento, "")
            hfdEstado.Value = IIf(Correlativo.bEstadoRegistroCorrelativoDocumento = False, "0", "1")
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
        Me.rfvFechaVigencia.EnableClientScript = bValidar
        Me.rfvNroDigitoDoc.EnableClientScript = bValidar
        Me.rfvNroDigitoSerie.EnableClientScript = bValidar
        Me.rfvNroSerie.EnableClientScript = bValidar
        Me.rfvNroCorrelativoActual.EnableClientScript = bValidar
        Me.rfvTipoDocumento.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub ListarTipoDocumentoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoDocumentoNeg As New clsTipoDocumentoNegocios
        cboTipoDoc.DataTextField = "vDescripcionTipoDocumento"
        cboTipoDoc.DataValueField = "cIdTipoDocumento"
        cboTipoDoc.DataSource = TipoDocumentoNeg.TipoDocumentoListar()
        cboTipoDoc.Items.Clear()
        cboTipoDoc.DataBind()
    End Sub

    Sub ListarTipoDocumentoReferencialCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoDocumentoNeg As New clsTipoDocumentoNegocios
        cboTipoDocumentoReferencial.DataTextField = "vDescripcionTipoDocumento"
        cboTipoDocumentoReferencial.DataValueField = "cIdTipoDocumento"
        cboTipoDocumentoReferencial.DataSource = TipoDocumentoNeg.TipoDocumentoListar()
        cboTipoDocumentoReferencial.Items.Clear()
        cboTipoDocumentoReferencial.Items.Add("SELECCIONE DATO")
        cboTipoDocumentoReferencial.DataBind()
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtFechaVigencia.Text = ""
        txtNroCorrelativoDesde.Text = ""
        txtNroCorrelativoHasta.Text = ""
        txtNroDigitoDoc.Text = ""
        txtNroDigitoSerie.Text = ""
        txtNroSerie.Text = ""
        txtNroCorrelativoActual.Text = ""
        cboTipoDoc.SelectedIndex = -1
        chkFacturacionElectronica.Checked = False
        cboTipoDocumentoReferencial.SelectedIndex = -1
        txtSerieAlfanumerico.Text = ""
        hfdEstado.Value = "1"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If
        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            strOpcionModulo = "015" 'Mantenimiento de los Correlativos de los Documentos General.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If
            ListarTipoDocumentoCombo()
            ListarTipoDocumentoReferencialCombo()

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 1
            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                             txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
            Me.grdLista.DataBind()
        Else
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanListado_imgbtnBuscarCorrelativoDocumento')")
            imgbtnBuscarCorrelativoDocumento.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_cbotipoDoc')")
            cboTipoDoc.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtNroSerie')")
            txtNroSerie.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtNroCorrelativoActual')")
            txtNroCorrelativoActual.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtNroCorrelativoDesde')")
            txtNroCorrelativoDesde.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtNroCorrelativoHasta')")
            txtNroCorrelativoHasta.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_chkFacturacionElectronica')")
            chkFacturacionElectronica.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtFechaVigencia')")
            txtFechaVigencia.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtNroDigitoSerie')")
            txtNroDigitoSerie.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtNroDigitoDoc')")
            If cboTipoDocumentoReferencial.Visible = True Then
                txtNroDigitoDoc.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_cboTipoDocumentoReferencial')")
                cboTipoDocumentoReferencial.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCorrelativoDocumento_TabPanGeneral_txtSerieAlfanumerico')")
                txtSerieAlfanumerico.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnNuevo')")
            Else
                txtNroDigitoDoc.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnNuevo')")
            End If
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                         txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex

        Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                             txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))

        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                         txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Right
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
        End If
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdLista.SelectedIndexChanged
        MyValidator.ErrorMessage = ""
        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text) = "True", "1", "0")
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    Protected Sub chkFacturacionElectronica_CheckedChanged(sender As Object, e As EventArgs) Handles chkFacturacionElectronica.CheckedChanged
        If chkFacturacionElectronica.Checked = True Then
            lblTipoDocumentoReferencial.Visible = True
            cboTipoDocumentoReferencial.Visible = True
            lblSerieAlfanumerico.Visible = True
            txtSerieAlfanumerico.Visible = True
        Else
            lblTipoDocumentoReferencial.Visible = False
            cboTipoDocumentoReferencial.Visible = False
            lblSerieAlfanumerico.Visible = False
            txtSerieAlfanumerico.Visible = False
        End If
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Correlativo As New GNRL_CORRELATIVODOCUMENTO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0033", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Correlativo.cIdTipoDocumento = Valores(0).ToString()
                Correlativo.vSerieCorrelativoDocumento = Valores(1).ToString()
                Correlativo.cIdEmpresa = Session("IdEmpresa")

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

                CorrelativoNeg.CorrelativoGetData("UPDATE GNRL_CORRELATIVODOCUMENTO SET bEstadoRegistroCorrelativoDocumento = 0 WHERE cIdEmpresa = '" & Correlativo.cIdEmpresa & "' AND cIdTipoDocumento = '" & Correlativo.cIdTipoDocumento & "' AND vSerieCorrelativoDocumento = '" & Correlativo.vSerieCorrelativoDocumento & "'")
                Session("Query") = "UPDATE GNRL_CORRELATIVODOCUMENTO SET bEstadoRegistroCorrelativoDocumento = 0 WHERE cIdEmpresa = '" & Correlativo.cIdEmpresa & "' AND cIdTipoDocumento = '" & Correlativo.cIdTipoDocumento & "' AND vSerieCorrelativoDocumento = '" & Correlativo.vSerieCorrelativoDocumento & "'"
                LogAuditoria.vEvento = "DESACTIVAR CORRELATIVO DOCUMENTO"
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

                Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                                 txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Correlativo As New GNRL_CORRELATIVODOCUMENTO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0033", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Correlativo.cIdTipoDocumento = Valores(0).ToString()
                Correlativo.vSerieCorrelativoDocumento = Valores(1).ToString()
                Correlativo.cIdEmpresa = Session("IdEmpresa")

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

                CorrelativoNeg.CorrelativoGetData("UPDATE GNRL_CORRELATIVODOCUMENTO SET bEstadoRegistroCorrelativoDocumento = 1 WHERE cIdEmpresa = '" & Correlativo.cIdEmpresa & "' AND cIdTipoDocumento = '" & Correlativo.cIdTipoDocumento & "' AND vSerieCorrelativoDocumento = '" & Correlativo.vSerieCorrelativoDocumento & "'")
                Session("Query") = "UPDATE GNRL_CORRELATIVODOCUMENTO SET bEstadoRegistroCorrelativoDocumento = 1 WHERE cIdEmpresa = '" & Correlativo.cIdEmpresa & "' AND cIdTipoDocumento = '" & Correlativo.cIdTipoDocumento & "' AND vSerieCorrelativoDocumento = '" & Correlativo.vSerieCorrelativoDocumento & "'"
                LogAuditoria.vEvento = "ACTIVAR CORRELATIVO DOCUMENTO"
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

                Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                                 txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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

    Private Sub imgbtnBuscarCorrelativoDocumento_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCorrelativoDocumento.Click
        Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                             txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0029", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ValidarTexto(True)
            ActivarObjetos(True)
            hfTab.Value = "tab2"
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda" 'JMUG: 31/12/2020 ANTES ERA vgrpValidar
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0030", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0032", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0031", strOpcionModulo, Session("IdSistema"))

            If chkFacturacionElectronica.Checked = True And cboTipoDocumentoReferencial.SelectedValue = "SELECCIONE DATO" And (cboTipoDoc.SelectedValue = "NC" Or cboTipoDoc.SelectedValue = "ND") Then
                Throw New Exception("Debe de ingresar el tipo de documento referencial.")
            ElseIf chkFacturacionElectronica.Checked = True And txtSerieAlfanumerico.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la serie alfanumérico.")
            End If

            Dim Correlativo As New GNRL_CORRELATIVODOCUMENTO
            With Correlativo
                .nNumeroSerieCorrelativoDocumento = txtNroSerie.Text
                .nNumeroCorrelativoActualCorrelativoDocumento = txtNroCorrelativoActual.Text
                .nNumeroCorrelativoDesdeCorrelativoDocumento = txtNroCorrelativoDesde.Text
                .nNumeroCorrelativoHastaCorrelativoDocumento = txtNroCorrelativoHasta.Text
                .nNumeroDigitoDocumentoCorrelativoDocumento = txtNroDigitoDoc.Text
                .nNumeroDigitoSerieCorrelativoDocumento = txtNroDigitoSerie.Text
                .dFechaVigenciaCorrelativoDocumento = txtFechaVigencia.Text
                .cIdEmpresa = Session("IdEmpresa")
                .cIdTipoDocumento = cboTipoDoc.SelectedValue
                .bEstadoRegistroCorrelativoDocumento = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
                .bFacturacionElectronicaCorrelativoDocumento = chkFacturacionElectronica.Checked
                .cIdTipoDocumentoRefCorrelativoDocumento = IIf(chkFacturacionElectronica.Checked = True, IIf(cboTipoDocumentoReferencial.SelectedValue = "SELECCIONE DATO", "", cboTipoDocumentoReferencial.SelectedValue), "")
                .vSerieCorrelativoDocumento = IIf(chkFacturacionElectronica.Checked = True, txtSerieAlfanumerico.Text, "")
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
                If (CorrelativoNeg.CorrelativoInserta(Correlativo)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_CORRELATIVODOCUMENTO 'SQL_INSERT', '', '" & Correlativo.cIdEmpresa & "', '" & Correlativo.cIdTipoDocumento & "', " &
                                       Correlativo.nNumeroSerieCorrelativoDocumento & ", " & Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento & ", " & Correlativo.nNumeroCorrelativoHastaCorrelativoDocumento & ", '" &
                                       Correlativo.dFechaVigenciaCorrelativoDocumento & "', " & Correlativo.nNumeroDigitoSerieCorrelativoDocumento & ", " & Correlativo.nNumeroDigitoDocumentoCorrelativoDocumento & ", '" &
                                       Correlativo.bEstadoRegistroCorrelativoDocumento & "', '" & Correlativo.bFacturacionElectronicaCorrelativoDocumento & "', " & Correlativo.nNumeroCorrelativoActualCorrelativoDocumento & ", " &
                                       Correlativo.cIdTipoDocumentoRefCorrelativoDocumento & "', '" & Correlativo.vSerieCorrelativoDocumento & "', ''"

                    LogAuditoria.vEvento = "INSERTAR CORRELATIVO DOCUMENTO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    cboTipoDoc.SelectedValue = Correlativo.cIdTipoDocumento

                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                                     txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (CorrelativoNeg.CorrelativoEdita(Correlativo)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_CORRELATIVODOCUMENTO 'SQL_UPDATE', '', '" & Correlativo.cIdEmpresa & "', '" & Correlativo.cIdTipoDocumento & "', " &
                                       Correlativo.nNumeroSerieCorrelativoDocumento & ", " & Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento & ", " & Correlativo.nNumeroCorrelativoHastaCorrelativoDocumento & ", '" &
                                       Correlativo.dFechaVigenciaCorrelativoDocumento & "', " & Correlativo.nNumeroDigitoSerieCorrelativoDocumento & ", " & Correlativo.nNumeroDigitoDocumentoCorrelativoDocumento & ", '" &
                                       Correlativo.bEstadoRegistroCorrelativoDocumento & "', '" & Correlativo.bFacturacionElectronicaCorrelativoDocumento & "', " & Correlativo.nNumeroCorrelativoActualCorrelativoDocumento & ", " &
                                       Correlativo.cIdTipoDocumentoRefCorrelativoDocumento & "', '" & Correlativo.vSerieCorrelativoDocumento & "', ''"

                    LogAuditoria.vEvento = "ACTUALIZAR CORRELATIVO DOCUMENTO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.grdLista.DataSource = CorrelativoNeg.CorrelativoListaBusqueda(cboFiltro.SelectedValue,
                                                                                     txtBuscar.Text, Session("IdEmpresa"), Session("bFacturacionElectronica"), IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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