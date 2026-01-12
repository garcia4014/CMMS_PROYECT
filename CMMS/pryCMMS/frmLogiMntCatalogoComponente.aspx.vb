Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiMntCatalogoComponente
    Inherits System.Web.UI.Page
    Dim CatalogoNeg As New clsCatalogoNegocios
    Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator
    Shared rowIndexDetalle As Int64

    Public Sub CargarPerfil()
        Dim FuncionesNeg As New clsFuncionesNegocios

        Dim mpContentPlaceHolder As ContentPlaceHolder
        Dim mpImage As Image
        Dim mpNombreUsuario As Label
        Dim mpApellidoPaternoUsuario As Label
        Dim mpApellidoMaternoUsuario As Label
        'LeftPanel_mnu_izq_pnlPerfil_imgPerfil
        'CType(Master.FindControl("mnu_izq"),
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

        ''INICIO: Novedades en el perfil.
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
            'Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
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
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("55", "LOGI", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ListarTipoActivoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoActivoNeg As New clsTipoActivoNegocios
        cboTipoActivo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivo.DataValueField = "cIdTipoActivo"
        cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        cboTipoActivo.DataBind()

        cboTipoActivoDetalleCatalogo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoDetalleCatalogo.DataValueField = "cIdTipoActivo"
        cboTipoActivoDetalleCatalogo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        cboTipoActivoDetalleCatalogo.DataBind()
    End Sub

    Sub ListarSistemaFuncionalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncionalDetalleCatalogo.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncionalDetalleCatalogo.DataValueField = "cIdSistemaFuncional"
        cboSistemaFuncionalDetalleCatalogo.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        cboSistemaFuncionalDetalleCatalogo.Items.Clear()
        cboSistemaFuncionalDetalleCatalogo.DataBind()
    End Sub

    Sub ListarCaracteristicaCatalogoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaCatalogo.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaCatalogo.DataValueField = "cIdCaracteristica"
        cboCaracteristicaCatalogo.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaCatalogo.Items.Clear()
        cboCaracteristicaCatalogo.DataBind()
    End Sub

    Sub ListarCaracteristicaCatalogoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaDetalleCatalogo.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaDetalleCatalogo.DataValueField = "cIdCaracteristica"
        cboCaracteristicaDetalleCatalogo.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaDetalleCatalogo.Items.Clear()
        cboCaracteristicaDetalleCatalogo.DataBind()
    End Sub

    Sub ListarDescripcionDetalleCatalogo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CatalogoNeg As New clsCatalogoNegocios
        cboDescripcionDetalleCatalogo.DataTextField = "vDescripcionCatalogo"
        cboDescripcionDetalleCatalogo.DataValueField = "cIdCatalogo"
        cboDescripcionDetalleCatalogo.DataSource = CatalogoNeg.CatalogoListarDescripcionCombo()
        cboDescripcionDetalleCatalogo.Items.Clear()
        cboDescripcionDetalleCatalogo.Items.Add(New ListItem("SELECCIONE DATO", ""))
        cboDescripcionDetalleCatalogo.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdCatalogo.Enabled = False
        cboTipoActivo.Enabled = IIf(hfdOperacion.Value = "N", bActivar, False)
        txtDescripcion.Enabled = bActivar
        txtDescripcionAbreviada.Enabled = bActivar
        txtCuentaContable.Enabled = bActivar
        txtCuentaContableLeasing.Enabled = bActivar

        txtIdDetalleCatalogo.Enabled = False
        cboTipoActivoDetalleCatalogo.Enabled = False ' bActivar
        cboSistemaFuncionalDetalleCatalogo.Enabled = bActivar
        txtDescripcionDetalleCatalogo.Enabled = bActivar
        txtDescripcionAbreviadaDetalleCatalogo.Enabled = bActivar
        txtVidaUtilDetalleCatalogo.Enabled = bActivar
        txtCuentaContableDetalleCatalogo.Enabled = bActivar
        txtCuentaContableLeasingDetalleCatalogo.Enabled = bActivar
    End Sub

    Sub LlenarDataDetalle()
        Try
            LimpiarObjetos()

            txtIdDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim
            cboDescripcionDetalleCatalogo.SelectedValue = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(3).Text)
            txtDescripcionDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(3).Text)
            txtDescripcionAbreviadaDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(4).Text)
            cboTipoActivoDetalleCatalogo.SelectedValue = grdListaDetalle.SelectedRow.Cells(5).Text
            cboSistemaFuncionalDetalleCatalogo.SelectedValue = IIf(Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(7).Text).Trim = "", Nothing, Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(7).Text))
            txtCuentaContableDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(9).Text)
            txtCuentaContableLeasingDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(10).Text)
            If hfdOperacionDetalle.Value = "E" Then
                txtVidaUtilDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
                txtPeriodoGarantiaDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("PeriodoGarantia").ToString.Trim
                txtPeriodoMinimoDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("PeriodoMinimo").ToString.Trim
            Else
                Dim Catalogo = CatalogoNeg.CatalogoListarPorId(txtIdDetalleCatalogo.Text, cboTipoActivoDetalleCatalogo.SelectedValue, "1", "1")
                txtVidaUtilDetalleCatalogo.Text = IIf(Catalogo.nVidaUtilCatalogo Is Nothing, "0", Catalogo.nVidaUtilCatalogo)
                txtPeriodoGarantiaDetalleCatalogo.Text = IIf(Catalogo.nPeriodoGarantiaCatalogo Is Nothing, "0", Catalogo.nPeriodoGarantiaCatalogo)
                txtPeriodoMinimoDetalleCatalogo.Text = IIf(Catalogo.nPeriodoMinimoMantenimientoCatalogo Is Nothing, "0", Catalogo.nPeriodoMinimoMantenimientoCatalogo)
            End If
        Catch ex As Exception
            MyValidator.ErrorMessage = ex.Message
        End Try
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

    Sub CargarCestaCatalogo()
        Try
            'Lo quite porque no me genera el mensaje correcto.
            clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogo"))
            If hfdOperacion.Value <> "N" Then
                Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("cIdEnlaceCatalogo", txtIdCatalogo.Text, "1", "1")
                If Coleccion.Count > 0 Then
                    Dim dtNewColeccion = CatalogoNeg.CatalogoGetData("SELECT CAT.cIdCatalogo, CAT.cIdTipoActivo, CAT.cIdJerarquiaCatalogo, CAT.cIdSistemaFuncional, CAT.cIdEnlaceCatalogo, CAT.vDescripcionCatalogo, " &
                                                                     "       CAT.vDescripcionAbreviadaCatalogo, CAT.bEstadoRegistroCatalogo, CAT.cIdCuentaContableCatalogo, CAT.cIdCuentaContableLeasingCatalogo, CAT.nVidaUtilCatalogo, " &
                                                                     "       TIPACT.vDescripcionAbreviadaTipoActivo, SISFUN.vDescripcionAbreviadaSistemaFuncional, CAT.nPeriodoGarantiaCatalogo, CAT.nPeriodoMinimoMantenimientoCatalogo " &
                                                                     "FROM LOGI_CATALOGO AS CAT LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
                                                                     "     CAT.cIdTipoActivo = TIPACT.cIdTipoActivo  " &
                                                                     "     LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
                                                                     "     CAT.cIdSistemaFuncional = SISFUN.cIdSistemaFuncional " &
                                                                     "WHERE CAT.cIdEnlaceCatalogo = '" & txtIdCatalogo.Text & "' " &
                                                                     IIf(Session("IdTipoUsuario") = "U", "      AND CAT.bEstadoRegistroCatalogo = '1' ", "") &
                                                                     "ORDER BY cIdCatalogo")
                    For Each row In dtNewColeccion.Rows
                        clsLogiCestaCatalogo.AgregarCesta(row("cIdCatalogo"), row("cIdTipoActivo").ToString, row("cIdJerarquiaCatalogo").ToString, row("cIdSistemaFuncional").ToString,
                                                   row("cIdEnlaceCatalogo").ToString, row("vDescripcionCatalogo").ToString.Trim, row("vDescripcionAbreviadaCatalogo").ToString.Trim,
                                                   row("bEstadoRegistroCatalogo").ToString, IIf(row("nVidaUtilCatalogo").ToString.Trim = "", "0", row("nVidaUtilCatalogo").ToString),
                                                   row("cIdCuentaContableCatalogo").ToString, row("cIdCuentaContableLeasingCatalogo").ToString, row("vDescripcionAbreviadaTipoActivo").ToString, row("vDescripcionAbreviadaSistemaFuncional").ToString,
                                                   IIf(row("nPeriodoGarantiaCatalogo").ToString.Trim = "", "0", row("nPeriodoGarantiaCatalogo").ToString), IIf(row("nPeriodoMinimoMantenimientoCatalogo").ToString.Trim = "", "0", row("nPeriodoMinimoMantenimientoCatalogo").ToString),
                                                   Session("CestaCatalogo"))
                    Next
                End If
            Else

            End If
            Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
            Me.grdListaDetalle.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            grdListaDetalle.DataSource = Nothing
            grdListaDetalle.DataBind()
        End Try
    End Sub

    Sub CargarCestaCaracteristicaCatalogo()
        Try
            'Lo quite porque no me genera el mensaje correcto.
            clsLogiCestaCatalogo.VaciarCesta(Session("CestaCaracteristicaCatalogo"))
            If hfdOperacion.Value <> "N" Then
                Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdCatalogo", txtIdCatalogo.Text, "0", "1")
                If Coleccion.Count > 0 Then
                    Dim dtNewColeccion = CatalogoNeg.CatalogoGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, " &
                                                                     "       CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                     "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
                                                                     "     CATCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                     "     INNER JOIN LOGI_CATALOGO AS CAT ON " &
                                                                     "     CAT.cIdCatalogo = CATCAR.cIdCatalogo AND CAT.cIdJerarquiaCatalogo = CATCAR.cIdJerarquiaCatalogo " &
                                                                     "WHERE CAT.cIdCatalogo = '" & txtIdCatalogo.Text & "' AND CATCAR.cIdJerarquiaCatalogo = '0' " &
                                                                     "ORDER BY CATCAR.cIdCatalogo")
                    For Each row In dtNewColeccion.Rows
                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(row("cIdCatalogo"), "", row("cIdJerarquiaCatalogo").ToString,
                                                   row("cIdCaracteristica").ToString, row("vDescripcionCaracteristica"), row("cIdReferenciaSAPCatalogoCaracteristica"), row("vDescripcionCampoSAPCatalogoCaracteristica"), row("vValorCatalogoCaracteristica"),
                                                   Session("CestaCaracteristicaCatalogo"))
                    Next
                End If
            End If
            Me.grdDetalleCaracteristicaCatalogo.DataSource = Session("CestaCaracteristicaCatalogo")
            Me.grdDetalleCaracteristicaCatalogo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            grdListaDetalle.DataSource = Nothing
            grdListaDetalle.DataBind()
        End Try
    End Sub

    Sub CargarCestaCaracteristicaCatalogoComponente()
        Try
            'Lo quite porque no me genera el mensaje correcto.
            clsLogiCestaCatalogo.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            If hfdOperacion.Value <> "N" Then
                Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("cIdEnlaceCatalogo", txtIdCatalogo.Text, "1", "1")
                If Coleccion.Count > 0 Then
                    Dim dtNewColeccion = CatalogoNeg.CatalogoGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, " &
                                                                     "       CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                     "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
                                                                     "     CATCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                     "     INNER JOIN LOGI_CATALOGO AS CAT ON " &
                                                                     "     CAT.cIdCatalogo = CATCAR.cIdCatalogo AND CAT.cIdJerarquiaCatalogo = CATCAR.cIdJerarquiaCatalogo " &
                                                                     "WHERE CAT.cIdEnlaceCatalogo = '" & txtIdCatalogo.Text & "' AND CATCAR.cIdJerarquiaCatalogo = '1' " &
                                                                     "ORDER BY CATCAR.cIdCatalogo")
                    For Each row In dtNewColeccion.Rows
                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(row("cIdCatalogo"), "", row("cIdJerarquiaCatalogo").ToString,
                                                   row("cIdCaracteristica").ToString, row("vDescripcionCaracteristica"), row("cIdReferenciaSAPCatalogoCaracteristica"), row("vDescripcionCampoSAPCatalogoCaracteristica"), row("vValorCatalogoCaracteristica"),
                                                   Session("CestaCaracteristicaCatalogoComponente"))
                    Next
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            grdListaDetalle.DataSource = Nothing
            grdListaDetalle.DataBind()
        End Try
    End Sub

    Sub LlenarData()
        Try
            Dim Catalogo As LOGI_CATALOGO = CatalogoNeg.CatalogoListarPorId(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(7).Text, "0", "1")
            txtIdCatalogo.Text = Catalogo.cIdCatalogo
            txtDescripcion.Text = Catalogo.vDescripcionCatalogo
            txtDescripcionAbreviada.Text = Catalogo.vDescripcionAbreviadaCatalogo
            txtVidaUtil.Text = IIf(IsNothing(Catalogo.nVidaUtilCatalogo), "0", Catalogo.nVidaUtilCatalogo)
            txtCuentaContable.Text = Catalogo.cIdCuentaContableCatalogo
            txtCuentaContableLeasing.Text = Catalogo.cIdCuentaContableLeasingCatalogo
            cboTipoActivo.SelectedValue = Catalogo.cIdTipoActivo
            txtPeriodoGarantia.Text = Catalogo.nPeriodoGarantiaCatalogo.ToString.Trim
            txtPeriodoMinimo.Text = Catalogo.nPeriodoMinimoMantenimientoCatalogo.ToString.Trim
            ListarSistemaFuncionalCombo()
            hfdSistemaFuncional.Value = Catalogo.cIdSistemaFuncional
            CargarCestaCaracteristicaCatalogo()
            'Detalle
            CargarCestaCatalogo()
            CargarCestaCaracteristicaCatalogoComponente()
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
        Me.rfvTipoActivo.EnableClientScript = bValidar
        Me.rfvDescripcion.EnableClientScript = bValidar
        Me.rfvDescripcionAbreviada.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean, ByVal bDuplicar As Boolean)
        Me.btnNuevo.Visible = False
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
        Me.btnDuplicar.Visible = bDuplicar
    End Sub

    Sub LimpiarObjetos()
        If hfdOperacion.Value = "N" Then
            MyValidator.ErrorMessage = ""
            txtIdCatalogo.Text = ""
            cboTipoActivo.SelectedIndex = -1
            txtDescripcion.Text = ""
            txtDescripcionAbreviada.Text = ""
            txtVidaUtil.Text = ""
            txtPeriodoGarantia.Text = ""
            txtPeriodoMinimo.Text = ""
            txtCuentaContable.Text = ""
            txtCuentaContableLeasing.Text = ""
        End If

        If hfdOperacionDetalle.Value = "N" Then
            txtIdDetalleCatalogo.Text = ""
            cboTipoActivoDetalleCatalogo.SelectedIndex = -1
            cboSistemaFuncionalDetalleCatalogo.SelectedIndex = -1
            txtDescripcionDetalleCatalogo.Text = ""
            txtDescripcionAbreviadaDetalleCatalogo.Text = ""
            txtVidaUtilDetalleCatalogo.Text = ""
            txtPeriodoGarantiaDetalleCatalogo.Text = ""
            txtPeriodoMinimoDetalleCatalogo.Text = ""
            txtCuentaContableDetalleCatalogo.Text = ""
            txtCuentaContableLeasingDetalleCatalogo.Text = ""
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "125" 'Mantenimiento de los Catálogos/Componente.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 3
            ListarTipoActivoCombo()
            ListarSistemaFuncionalCombo()
            ListarDescripcionDetalleCatalogo()
            ListarCaracteristicaCatalogoCombo()
            ListarCaracteristicaCatalogoComponenteCombo()

            If Session("CestaCatalogo") Is Nothing Then
                Session("CestaCatalogo") = clsLogiCestaCatalogo.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogo"))
            End If

            BloquearPagina(1)
            BloquearMantenimiento(True, False, True, False, True)

            If Session("CestaCaracteristicaCatalogo") Is Nothing Then
                Session("CestaCaracteristicaCatalogo") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogo"))
            End If
            Me.grdDetalleCaracteristicaCatalogo.DataSource = Session("CestaCaracteristicaCatalogo")
            Me.grdDetalleCaracteristicaCatalogo.DataBind()

            If Session("CestaCaracteristicaCatalogoComponente") Is Nothing Then
                Session("CestaCaracteristicaCatalogoComponente") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            End If
            Me.grdDetalleCaracteristicaDetalleCatalogo.DataSource = Session("CestaCaracteristicaCatalogoComponente")
            Me.grdDetalleCaracteristicaDetalleCatalogo.DataBind()

            Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
            Me.grdLista.DataBind()
        Else
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanListado_txtBuscar')")
            txtIdCatalogo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_cboFamilia')")
            cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcioAbreviada')")
            txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContable')")
            txtCuentaContable.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContableLeasing')")
            txtCuentaContableLeasing.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContableLeasing')")
            btnNuevo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcion')")
            btnGuardar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnEditar')")
            btnEditar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnDeshacer')")
            btnDeshacer.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnEliminar')")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
            txtDescripcion.Focus()

            BloquearMantenimiento(False, True, False, True, False)
            LimpiarObjetos()
            CargarCestaCatalogo()
            CargarCestaCaracteristicaCatalogo()
            CargarCestaCaracteristicaCatalogoComponente()
            ValidarTexto(True)
            ActivarObjetos(True)
            hfTab.Value = "tab2"
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

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "E"
            If BloquearPagina(2) = False Then
                BloquearMantenimiento(False, True, False, True, False)
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0640", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "R"
            BloquearPagina(0)
            BloquearMantenimiento(True, False, True, False, True)
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0639", strOpcionModulo, Session("IdSistema"))

            Dim Catalogo As New LOGI_CATALOGO
            Catalogo.cIdCatalogo = UCase(txtIdCatalogo.Text)
            Catalogo.cIdTipoActivo = cboTipoActivo.SelectedValue
            Catalogo.cIdSistemaFuncional = IIf(hfdSistemaFuncional.Value.Trim = "", Nothing, hfdSistemaFuncional.Value)
            Catalogo.cIdJerarquiaCatalogo = "0"
            Catalogo.cIdEnlaceCatalogo = ""
            Catalogo.vDescripcionCatalogo = UCase(txtDescripcion.Text)
            Catalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviada.Text)
            Catalogo.bEstadoRegistroCatalogo = True
            Catalogo.cIdCuentaContableCatalogo = txtCuentaContable.Text
            Catalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasing.Text
            'Catalogo.nPeriodoGarantiaCatalogo = txtPeriodoGarantia.Text
            'Catalogo.nPeriodoMinimoMantenimientoCatalogo = txtPeriodoMinimo.Text
            Catalogo.nVidaUtilCatalogo = Convert.ToInt32(IIf(txtVidaUtil.Text.Trim = "", "0", txtVidaUtil.Text)) 'Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
            Catalogo.nPeriodoGarantiaCatalogo = Convert.ToInt32(IIf(txtPeriodoGarantia.Text.Trim = "", "0", txtPeriodoGarantia.Text))
            Catalogo.nPeriodoMinimoMantenimientoCatalogo = Convert.ToInt32(IIf(txtPeriodoMinimo.Text.Trim = "", "0", txtPeriodoMinimo.Text))

            Dim Coleccion As New List(Of LOGI_CATALOGO)
            For i = 0 To Session("CestaCatalogo").Rows.Count - 1
                Dim DetCatalogo As New LOGI_CATALOGO
                DetCatalogo.cIdCatalogo = Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
                DetCatalogo.cIdEnlaceCatalogo = Catalogo.cIdCatalogo 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                DetCatalogo.cIdJerarquiaCatalogo = Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
                DetCatalogo.cIdSistemaFuncional = Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString.Trim
                DetCatalogo.cIdTipoActivo = Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString.Trim
                DetCatalogo.vDescripcionCatalogo = Session("CestaCatalogo").Rows(i)("Descripcion").ToString.Trim
                DetCatalogo.vDescripcionAbreviadaCatalogo = Session("CestaCatalogo").Rows(i)("DescripcionAbreviada").ToString.Trim
                DetCatalogo.bEstadoRegistroCatalogo = Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
                DetCatalogo.cIdCuentaContableCatalogo = Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
                DetCatalogo.cIdCuentaContableLeasingCatalogo = Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString.Trim
                DetCatalogo.nVidaUtilCatalogo = Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
                DetCatalogo.nPeriodoGarantiaCatalogo = Session("CestaCatalogo").Rows(i)("PeriodoGarantia").ToString.Trim
                DetCatalogo.nPeriodoMinimoMantenimientoCatalogo = Session("CestaCatalogo").Rows(i)("PeriodoMinimo").ToString.Trim

                Coleccion.Add(DetCatalogo)
            Next

            Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)

            For i = 0 To Session("CestaCaracteristicaCatalogo").Rows.Count - 1
                Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                DetCaracteristica.cIdCatalogo = Session("CestaCaracteristicaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
                DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogo").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogo").Rows(i)("Item").ToString.Trim
                DetCaracteristica.vValorCatalogoCaracteristica = UCase(Session("CestaCaracteristicaCatalogo").Rows(i)("Valor").ToString.Trim)
                DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogo").Rows(i)("IdReferenciaSAP").ToString.Trim
                DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = UCase(Session("CestaCaracteristicaCatalogo").Rows(i)("DescripcionCampoSAP").ToString.Trim)
                ColeccionCaracteristica.Add(DetCaracteristica)
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
            If hfdOperacion.Value = "N" Then
                If (CatalogoNeg.CatalogoInserta(Catalogo)) = 0 Then
                    If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, Catalogo.cIdCatalogo) = 0 Then
                        If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then

                        End If
                        Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" & Catalogo.cIdTipoActivo & "', '" &
                                           Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                           Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                           Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.nVidaUtilCatalogo & ", '" &
                                           Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCatalogo & "'"

                        LogAuditoria.vEvento = "INSERTAR CATALOGO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        txtIdCatalogo.Text = Catalogo.cIdCatalogo
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        CargarCestaCatalogo()
                        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                        Me.grdLista.DataBind()
                        pnlGeneral.Enabled = False
                        BloquearMantenimiento(True, False, True, False, True)
                        hfdOperacion.Value = "R"
                        hfTab.Value = "tab1"
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (CatalogoNeg.CatalogoEdita(Catalogo)) = 0 Then
                    If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, Catalogo.cIdCatalogo) = 0 Then
                        If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then

                        End If
                        Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" & Catalogo.cIdTipoActivo & "', '" &
                                           Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                           Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                           Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.nVidaUtilCatalogo & ", '" &
                                           Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCatalogo & "'"

                        LogAuditoria.vEvento = "ACTUALIZAR CATALOGO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        MyValidator.ErrorMessage = "Registro actualizado con éxito"
                        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                        Me.grdLista.DataBind()
                        pnlGeneral.Enabled = False
                        BloquearMantenimiento(True, False, True, False, True)
                        hfdOperacion.Value = "R"
                        hfTab.Value = "tab1"
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                End If
            End If
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

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
        Me.grdLista.DataBind()
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = True
            e.Row.Cells(6).Visible = True
            e.Row.Cells(7).Visible = False
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = True
            e.Row.Cells(6).Visible = True
            e.Row.Cells(7).Visible = False
        End If
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdLista.SelectedIndexChanged
        MyValidator.ErrorMessage = ""
        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData() 'JMUG: 06/02/2023
        End If
    End Sub

    Protected Sub btnAceptarDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles btnAceptarDetalleCatalogo.Click
        Try
            If cboTipoActivoDetalleCatalogo.SelectedIndex <= 0 Then
                Throw New Exception("Debe de ingresar el código del tipo de activo correcto.")
            ElseIf cboSistemaFuncionalDetalleCatalogo.SelectedIndex < 0 Then
                Throw New Exception("Debe de ingresar el código del sistema funcional correcto.")
            ElseIf txtDescripcionDetalleCatalogo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción correcta.")
            ElseIf txtDescripcionAbreviadaDetalleCatalogo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción abreviada correcta.")
            Else
                Dim Coleccion As New List(Of LOGI_CATALOGO)
                For i = 0 To 0
                    Dim DetCatalogo As New LOGI_CATALOGO
                    DetCatalogo.cIdCatalogo = ""  'Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
                    DetCatalogo.cIdEnlaceCatalogo = txtIdCatalogo.Text 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                    DetCatalogo.cIdJerarquiaCatalogo = "1" 'Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
                    DetCatalogo.cIdSistemaFuncional = cboSistemaFuncionalDetalleCatalogo.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString.Trim
                    DetCatalogo.cIdTipoActivo = cboTipoActivoDetalleCatalogo.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString.Trim
                    DetCatalogo.vDescripcionCatalogo = UCase(txtDescripcionDetalleCatalogo.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion").ToString.Trim
                    DetCatalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString.Trim
                    DetCatalogo.bEstadoRegistroCatalogo = True 'Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
                    DetCatalogo.nVidaUtilCatalogo = Convert.ToInt32(IIf(txtVidaUtilDetalleCatalogo.Text.Trim = "", "0", txtVidaUtilDetalleCatalogo.Text)) 'Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
                    DetCatalogo.nPeriodoGarantiaCatalogo = Convert.ToInt32(IIf(txtPeriodoGarantiaDetalleCatalogo.Text.Trim = "", "0", txtPeriodoGarantiaDetalleCatalogo.Text))
                    DetCatalogo.nPeriodoMinimoMantenimientoCatalogo = Convert.ToInt32(IIf(txtPeriodoMinimoDetalleCatalogo.Text.Trim = "", "0", txtPeriodoMinimoDetalleCatalogo.Text))
                    DetCatalogo.cIdCuentaContableCatalogo = txtCuentaContableDetalleCatalogo.Text 'Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
                    DetCatalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasingDetalleCatalogo.Text  'Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString.Trim
                    Coleccion.Add(DetCatalogo)
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

                If hfdOperacion.Value = "E" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                        For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                            For j = 0 To Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows.Count - 1
                                If (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim) = Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(j)("IdCatalogo").ToString.Trim And
                                   (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim) = Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(j)("IdJerarquia").ToString.Trim And
                                   (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(j)("IdCaracteristica").ToString.Trim Then
                                    clsLogiCestaCatalogoCaracteristica.EditarCesta(Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim, "", Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim, Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim, Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Descripcion").ToString.Trim, Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(j)("IdReferenciaSAP").ToString.Trim, Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(j)("DescripcionCampoSAP").ToString.Trim, Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(j)("Valor").ToString.Trim, Session("CestaCaracteristicaCatalogoComponente"), i)
                                    Exit For
                                End If
                            Next
                            Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                            DetCaracteristica.cIdCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                            DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            ColeccionCaracteristica.Add(DetCaracteristica)
                        Next

                        LogAuditoria.vEvento = "INSERTAR CATALOGO CARACTERISTICA"
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) = 0 Then
                            Dim SisFunNeg As New clsSistemaFuncionalNegocios
                            clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                                         txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                                                         "1", IIf(txtVidaUtilDetalleCatalogo.Text.Trim = "", "0", txtVidaUtilDetalleCatalogo.Text.Trim),
                                                         txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                                         SisFunNeg.SistemaFuncionalListarPorId(cboSistemaFuncionalDetalleCatalogo.SelectedValue).vDescripcionAbreviadaSistemaFuncional, IIf(txtPeriodoGarantiaDetalleCatalogo.Text.Trim = "", "0", txtPeriodoGarantiaDetalleCatalogo.Text.Trim), IIf(txtPeriodoMinimoDetalleCatalogo.Text.Trim = "", "0", txtPeriodoMinimoDetalleCatalogo.Text.Trim),
                                                         Session("CestaCatalogo"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                            MyValidator.ErrorMessage = "Registro actualizado con éxito"
                        End If
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, txtIdCatalogo.Text) = 0 Then
                            Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                                Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                DetCaracteristica.cIdCatalogo = IIf(Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim = "", Coleccion(0).cIdCatalogo, Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim)
                                DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                                DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                            'INICIO: JMUG: 24/10/2023
                            LogAuditoria.vEvento = "INSERTAR CATALOGO CARACTERISTICA"
                            LogAuditoria.cIdSistema = Session("IdSistema")
                            LogAuditoria.cIdModulo = strOpcionModulo
                            'FINAL: JMUG: 24/10/2023
                            If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                                clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                     "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                     txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                                     "1", IIf(txtVidaUtilDetalleCatalogo.Text.Trim = "", "0", txtVidaUtilDetalleCatalogo.Text.Trim),
                                     txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                     cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, IIf(txtPeriodoGarantiaDetalleCatalogo.Text.Trim = "", "0", txtPeriodoGarantiaDetalleCatalogo.Text.Trim), IIf(txtPeriodoMinimoDetalleCatalogo.Text.Trim = "", "0", txtPeriodoMinimoDetalleCatalogo.Text.Trim),
                                     Session("CestaCatalogo"))
                            End If
                            MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        End If
                    End If
                ElseIf hfdOperacion.Value = "N" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                                         txtIdCatalogo.Text.Trim, txtDescripcionDetalleCatalogo.Text.Trim, txtDescripcionAbreviadaDetalleCatalogo.Text.Trim,
                                                         "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                                                         txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                                         cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, txtPeriodoGarantiaDetalleCatalogo.Text, txtPeriodoMinimoDetalleCatalogo.Text,
                                                         Session("CestaCatalogo"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        clsLogiCestaCatalogo.AgregarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                                                  "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                                                  txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                                                                  "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                                                                  txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                                                  cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, txtPeriodoGarantiaDetalleCatalogo.Text, txtPeriodoMinimoDetalleCatalogo.Text,
                                                                  Session("CestaCatalogo"))
                    End If
                End If
            End If
            Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
            Me.grdListaDetalle.DataBind()
            Me.lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Hide()
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
            MyValidator.ErrorMessage = ex.Message
            Me.lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdListaDetalle_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdListaDetalle.PageIndexChanging
        grdListaDetalle.PageIndex = e.NewPageIndex
        Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
        Me.grdListaDetalle.DataBind()
        grdListaDetalle.SelectedIndex = -1
    End Sub

    Private Sub grdListaDetalle_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdListaDetalle.RowDeleting

    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Catalogo.cIdCatalogo = Valores(0).ToString()
                Catalogo.cIdTipoActivo = Valores(1).ToString()
                Catalogo.cIdJerarquiaCatalogo = "0"

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "DESACTIVAR CATALOGO"
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

                Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Catalogo.cIdCatalogo = Valores(0).ToString()
                Catalogo.cIdTipoActivo = Valores(1).ToString()
                Catalogo.cIdJerarquiaCatalogo = "0"

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "ACTIVAR CATALOGO"
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

                Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
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

    Private Sub grdListaDetalle_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdListaDetalle.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(11).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdListaDetalle_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdListaDetalle.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = True
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = True
            e.Row.Cells(9).Visible = True
            e.Row.Cells(10).Visible = True
            e.Row.Cells(11).Visible = False 'Estado
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = True
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = True
            e.Row.Cells(9).Visible = True
            e.Row.Cells(10).Visible = True
            e.Row.Cells(11).Visible = False 'Estado
        End If
    End Sub

    Protected Sub grdListaDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0643", strOpcionModulo, Session("IdSistema"))

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Dim i As Integer

                i = Valores(0)
                Catalogo.cIdEnlaceCatalogo = Valores(1).ToString
                Catalogo.cIdCatalogo = Valores(2).ToString()
                Catalogo.cIdTipoActivo = Valores(3).ToString()
                Catalogo.cIdJerarquiaCatalogo = "1"

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

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "DESACTIVAR CATALOGO/COMPONENTE"
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

                clsLogiCestaCatalogo.EditarCesta(Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString, Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString, Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdEnlace").ToString, Session("CestaCatalogo").Rows(i)("Descripcion").ToString, Session("CestaCatalogo").Rows(i)("DescripcionAbreviada").ToString, False,
                                                 Session("CestaCatalogo").Rows(i)("VidaUtil").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString, Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("PeriodoGarantia").ToString, Session("CestaCatalogo").Rows(i)("PeriodoMinimo").ToString,
                                                 Session("CestaCatalogo"), i)
                Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
                Me.grdListaDetalle.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0643", strOpcionModulo, Session("IdSistema"))
                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Dim i As Integer
                i = Valores(0)
                Catalogo.cIdEnlaceCatalogo = Valores(1).ToString
                Catalogo.cIdCatalogo = Valores(2).ToString()
                Catalogo.cIdTipoActivo = Valores(3).ToString()
                Catalogo.cIdJerarquiaCatalogo = "1"

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "ACTIVAR CATALOGO/COMPONENTE"
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

                clsLogiCestaCatalogo.EditarCesta(Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString, Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString, Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdEnlace").ToString, Session("CestaCatalogo").Rows(i)("Descripcion").ToString, Session("CestaCatalogo").Rows(i)("DescripcionAbreviada").ToString, True,
                                                 Session("CestaCatalogo").Rows(i)("VidaUtil").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString, Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("PeriodoGarantia").ToString, Session("CestaCatalogo").Rows(i)("PeriodoMinimo").ToString,
                                                 Session("CestaCatalogo"), i)
                Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
                Me.grdListaDetalle.DataBind()
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

    Private Sub lnkbtnAgregarComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnAgregarComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0642", strOpcionModulo, Session("IdSistema"))

            If Session("CestaCaracteristicaCatalogoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaCatalogoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponenteFiltrado"))
            End If

            hfdOperacionDetalle.Value = "N"
            LimpiarObjetos()
            cboTipoActivoDetalleCatalogo.SelectedValue = cboTipoActivo.SelectedValue
            If cboSistemaFuncionalDetalleCatalogo.Items.Count = 0 Then
                Throw New Exception("Debe de ingresar los sistemas funcionales del catálogo.")
            End If
            cboSistemaFuncionalDetalleCatalogo.SelectedIndex = 0

            CargarCestaCaracteristicaCatalogoComponente()
            Me.grdDetalleCaracteristicaDetalleCatalogo.DataSource = Session("CestaCaracteristicaCatalogoComponenteFiltrado")
            Me.grdDetalleCaracteristicaDetalleCatalogo.DataBind()

            lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            BloquearPagina(2)
        End Try
    End Sub

    Private Sub lnkbtnEditarComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If Session("CestaCaracteristicaCatalogoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaCatalogoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponenteFiltrado"))
            End If

            hfdOperacionDetalle.Value = "E"
            If grdListaDetalle.Rows.Count > 0 Then
                If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
                    If IsNothing(grdListaDetalle.SelectedRow) = False Then
                        If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then
                            Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(5).Text).Trim & "'")
                            rowIndexDetalle = Session("CestaCatalogo").Rows.IndexOf(result(0))
                            LlenarDataDetalle()
                            CargarCestaCaracteristicaCatalogoComponente()
                            BloquearMantenimiento(False, True, False, True, False)
                            ValidarTexto(True)
                            ActivarObjetos(True)

                            Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaCatalogoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                            If resultCaracteristicaSimple.Length = 0 Then
                                Me.grdDetalleCaracteristicaDetalleCatalogo.DataSource = Nothing
                            Else
                                Dim rowFil As DataRow() = resultCaracteristicaSimple
                                For Each fila As DataRow In rowFil
                                    clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), "", fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaCatalogoComponenteFiltrado"))
                                Next
                            End If
                            Me.grdDetalleCaracteristicaDetalleCatalogo.DataSource = Session("CestaCaracteristicaCatalogoComponenteFiltrado")
                            Me.grdDetalleCaracteristicaDetalleCatalogo.DataBind()
                            lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar algún item.")
                    End If
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            BloquearPagina(2)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaCatalogo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaCatalogo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            For i = 0 To Session("CestaCaracteristicaCatalogo").Rows.Count - 1
                If (Session("CestaCaracteristicaCatalogo").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaCatalogo.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaCatalogo").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogo.Text.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                    'lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
                    Throw New Exception("Característica ya registrada, seleccione otro item.")
                    Exit Sub
                End If
            Next
            clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogo.Text, "", "0", cboCaracteristicaCatalogo.SelectedValue.Trim, UCase(cboCaracteristicaCatalogo.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogo"))
            Me.grdDetalleCaracteristicaCatalogo.DataSource = Session("CestaCaracteristicaCatalogo")
            Me.grdDetalleCaracteristicaCatalogo.DataBind()
            cboCaracteristicaCatalogo.SelectedIndex = -1
            grdDetalleCaracteristicaCatalogo.SelectedIndex = -1
            MyValidator.ErrorMessage = "Caracteristica agregada con éxito."

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

    Sub txtValorDetalleCatalogo_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogo.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Session("CestaCaracteristicaCatalogo").Rows(row.RowIndex)("Valor") = txtValorDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtIdReferenciaSAPDetalleCatalogo_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogo.Rows
                Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalle"), TextBox)
                Session("CestaCaracteristicaCatalogo").Rows(row.RowIndex)("IdReferenciaSAP") = txtIdReferenciaSAPDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtDescripcionCampoSAPDetalleCatalogo_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogo.Rows
                Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalle"), TextBox)
                Session("CestaCaracteristicaCatalogo").Rows(row.RowIndex)("DescripcionCampoSAP") = txtDescripcionCampoSAPDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtValorDetalleCaracteristica_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaDetalleCatalogo.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(row.RowIndex)("Valor") = txtValorDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtIdReferenciaSAPDetalleCaracteristica_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaDetalleCatalogo.Rows
                Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalle"), TextBox)
                Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(row.RowIndex)("IdReferenciaSAP") = txtIdReferenciaSAPDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtDescripcionCampoSAPDetalleCaracteristica_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaDetalleCatalogo.Rows
                Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalle"), TextBox)
                Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(row.RowIndex)("DescripcionCampoSAP") = txtDescripcionCampoSAPDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaDetalleCatalogo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
                For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                    If (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaDetalleCatalogo.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim = txtIdDetalleCatalogo.Text.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                        lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
                        Throw New Exception("Característica ya registrada, seleccione otro item.")
                        Exit Sub
                    End If
                Next
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdDetalleCatalogo.Text, "", "1", cboCaracteristicaDetalleCatalogo.SelectedValue.Trim, UCase(cboCaracteristicaDetalleCatalogo.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogoComponente"))
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdDetalleCatalogo.Text, "", "1", cboCaracteristicaDetalleCatalogo.SelectedValue.Trim, UCase(cboCaracteristicaDetalleCatalogo.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogoComponenteFiltrado"))

                Me.grdDetalleCaracteristicaDetalleCatalogo.DataSource = Session("CestaCaracteristicaCatalogoComponenteFiltrado")
                Me.grdDetalleCaracteristicaDetalleCatalogo.DataBind()
                lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()

                cboCaracteristicaDetalleCatalogo.SelectedIndex = -1
                lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
                grdDetalleCaracteristicaDetalleCatalogo.SelectedIndex = -1
                MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
            Else
                lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
            End If
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
            BloquearPagina(2)
        End Try
    End Sub

    Private Sub grdDetalleCaracteristicaCatalogo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaCatalogo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "489", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogo.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaCaracteristicaCatalogo").Rows.Count - 1
                            If (Session("CestaCaracteristicaCatalogo").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaCatalogo"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            Me.grdDetalleCaracteristicaCatalogo.DataSource = Session("CestaCaracteristicaCatalogo")
            Me.grdDetalleCaracteristicaCatalogo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleCaracteristicaDetalleCatalogo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaDetalleCatalogo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "489", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            Dim fila As Integer = 0
            For Each row As GridViewRow In grdDetalleCaracteristicaDetalleCatalogo.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        clsLogiCestaCatalogoCaracteristica.QuitarCestaGrupo(txtIdDetalleCatalogo.Text, row.Cells(3).Text, Session("CestaCaracteristicaCatalogoComponenteFiltrado"))
                        clsLogiCestaCatalogoCaracteristica.QuitarCestaGrupo(txtIdDetalleCatalogo.Text, row.Cells(3).Text, Session("CestaCaracteristicaCatalogoComponente"))
                        fila += 1
                    End If
                End If
            Next

            Me.grdDetalleCaracteristicaDetalleCatalogo.DataSource = Session("CestaCaracteristicaCatalogoComponenteFiltrado")
            Me.grdDetalleCaracteristicaDetalleCatalogo.DataBind()

            lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
        txtDescripcionDuplicarCatalogoComponente.Text = ""
        txtDescripcionAbreviadaDuplicarCatalogoComponente.Text = ""
        lnk_mostrarPanelDuplicarCatalogoComponente_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnAceptarDuplicarCatalogoComponente_Click(sender As Object, e As EventArgs) Handles btnAceptarDuplicarCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0639", strOpcionModulo, Session("IdSistema"))

            If grdLista.Rows.Count = 0 Then
                MyValidator.ErrorMessage = "No existen registros para duplicar."
                hfdOperacion.Value = "R"
            Else
                If IsNothing(grdLista.SelectedRow) = True Then
                    MyValidator.ErrorMessage = "Seleccione un registro a visualizar."
                    hfdOperacion.Value = "R"
                Else
                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                        Dim Catalogo As LOGI_CATALOGO = CatalogoNeg.CatalogoListarPorId(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(7).Text, "0", "1")
                        Catalogo.cIdCatalogo = ""
                        Catalogo.vDescripcionCatalogo = UCase(txtDescripcionDuplicarCatalogoComponente.Text.Trim)
                        Catalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviadaDuplicarCatalogoComponente.Text.Trim)

                        If (CatalogoNeg.CatalogoInserta(Catalogo)) = 0 Then
                            'Inicio: Inserta las características del equipo principal
                            Dim dtCaracteristicaCatalogoColeccion = CatalogoNeg.CatalogoGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, " &
                                                                             "       CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                             "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
                                                                             "     CATCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                             "     INNER JOIN LOGI_CATALOGO AS CAT ON " &
                                                                             "     CAT.cIdCatalogo = CATCAR.cIdCatalogo AND CAT.cIdJerarquiaCatalogo = CATCAR.cIdJerarquiaCatalogo " &
                                                                             "WHERE CAT.cIdCatalogo = '" & grdLista.SelectedRow.Cells(1).Text & "' AND CATCAR.cIdJerarquiaCatalogo = '0' " &
                                                                             "ORDER BY CATCAR.cIdCatalogo")
                            Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                            For Each row In dtCaracteristicaCatalogoColeccion.Rows
                                Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                DetCaracteristica.cIdCatalogo = Catalogo.cIdCatalogo 'row("cIdCatalogo")
                                DetCaracteristica.cIdJerarquiaCatalogo = row("cIdJerarquiaCatalogo").ToString
                                DetCaracteristica.cIdCaracteristica = row("cIdCaracteristica").ToString
                                DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = row("nIdNumeroItemCatalogoCaracteristica").ToString
                                DetCaracteristica.vValorCatalogoCaracteristica = row("vValorCatalogoCaracteristica")
                                DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = row("cIdReferenciaSAPCatalogoCaracteristica")
                                DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = row("vDescripcionCampoSAPCatalogoCaracteristica")
                                ColeccionCaracteristica.Add(DetCaracteristica)
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

                            If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then

                            End If
                            'Final: Inserta las características del equipo principal

                            'Inicio: Inserta los componentes del equipo principal
                            Dim dtNewColeccion = CatalogoNeg.CatalogoGetData("SELECT CAT.cIdCatalogo, CAT.cIdTipoActivo, CAT.cIdJerarquiaCatalogo, CAT.cIdSistemaFuncional, CAT.cIdEnlaceCatalogo, CAT.vDescripcionCatalogo, " &
                                                                         "       CAT.vDescripcionAbreviadaCatalogo, CAT.bEstadoRegistroCatalogo, CAT.cIdCuentaContableCatalogo, CAT.cIdCuentaContableLeasingCatalogo, CAT.nVidaUtilCatalogo, " &
                                                                         "       TIPACT.vDescripcionAbreviadaTipoActivo, SISFUN.vDescripcionAbreviadaSistemaFuncional, CAT.nPeriodoGarantiaCatalogo, CAT.nPeriodoMinimoMantenimientoCatalogo " &
                                                                         "FROM LOGI_CATALOGO AS CAT LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
                                                                         "     CAT.cIdTipoActivo = TIPACT.cIdTipoActivo  " &
                                                                         "     LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
                                                                         "     CAT.cIdSistemaFuncional = SISFUN.cIdSistemaFuncional " &
                                                                         "WHERE CAT.cIdEnlaceCatalogo = '" & grdLista.SelectedRow.Cells(1).Text & "' " &
                                                                         "ORDER BY cIdCatalogo")
                            Dim CatalogoColeccion As New List(Of LOGI_CATALOGO)
                            For Each row In dtNewColeccion.Rows
                                Dim DetCatalogo As New LOGI_CATALOGO
                                DetCatalogo.cIdCatalogo = ""
                                DetCatalogo.cIdEnlaceCatalogo = Catalogo.cIdCatalogo 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCatalogo.cIdJerarquiaCatalogo = row("cIdJerarquiaCatalogo")
                                DetCatalogo.cIdSistemaFuncional = row("cIdSistemaFuncional")
                                DetCatalogo.cIdTipoActivo = row("cIdTipoActivo")
                                DetCatalogo.vDescripcionCatalogo = row("vDescripcionCatalogo").ToString.Trim
                                DetCatalogo.vDescripcionAbreviadaCatalogo = row("vDescripcionAbreviadaCatalogo").ToString.Trim
                                DetCatalogo.bEstadoRegistroCatalogo = 1 'Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
                                DetCatalogo.nVidaUtilCatalogo = row("nVidaUtilCatalogo")
                                DetCatalogo.cIdCuentaContableCatalogo = row("cIdCuentaContableCatalogo")
                                DetCatalogo.cIdCuentaContableLeasingCatalogo = row("cIdCuentaContableLeasingCatalogo")
                                DetCatalogo.nPeriodoGarantiaCatalogo = row("nPeriodoGarantiaCatalogo")
                                DetCatalogo.nPeriodoMinimoMantenimientoCatalogo = row("nPeriodoMinimoMantenimientoCatalogo")
                                If CatalogoNeg.CatalogoInserta(DetCatalogo) = 0 Then
                                    'Inicio: Inserta las caracteristicass por cada componente insertado.
                                    Dim dtCaracteristicaCatalogoComponenteColeccion = CatalogoNeg.CatalogoGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, " &
                                                                             "       CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                             "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
                                                                             "     CATCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                             "     INNER JOIN LOGI_CATALOGO AS CAT ON " &
                                                                             "     CAT.cIdCatalogo = CATCAR.cIdCatalogo AND CAT.cIdJerarquiaCatalogo = CATCAR.cIdJerarquiaCatalogo " &
                                                                             "WHERE CAT.cIdEnlaceCatalogo = '" & grdLista.SelectedRow.Cells(1).Text & "' AND CATCAR.cIdJerarquiaCatalogo = '1' AND CATCAR.cIdCatalogo = '" & row("cIdCatalogo") & "' " &
                                                                             "ORDER BY CATCAR.cIdCatalogo")

                                    Dim ColeccionComponenteCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                                    For Each rowComponente In dtCaracteristicaCatalogoComponenteColeccion.Rows
                                        Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                        DetCaracteristica.cIdCatalogo = DetCatalogo.cIdCatalogo 'rowComponente("cIdCatalogo")
                                        DetCaracteristica.cIdJerarquiaCatalogo = rowComponente("cIdJerarquiaCatalogo").ToString
                                        DetCaracteristica.cIdCaracteristica = rowComponente("cIdCaracteristica").ToString
                                        DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = rowComponente("nIdNumeroItemCatalogoCaracteristica").ToString
                                        DetCaracteristica.vValorCatalogoCaracteristica = rowComponente("vValorCatalogoCaracteristica")
                                        DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = rowComponente("cIdReferenciaSAPCatalogoCaracteristica")
                                        DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = rowComponente("vDescripcionCampoSAPCatalogoCaracteristica")
                                        ColeccionComponenteCaracteristica.Add(DetCaracteristica)
                                    Next

                                    If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionComponenteCaracteristica, LogAuditoria) Then

                                    End If
                                    'Final: Inserta las caracteristicass por cada componente insertado.
                                End If
                            Next
                            'Final: Inserta los componentes del equipo principal

                            Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" & Catalogo.cIdTipoActivo & "', '" &
                                                       Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                                       Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                                       Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.nVidaUtilCatalogo & ", '" &
                                                       Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCatalogo & "'"

                            LogAuditoria.vEvento = "INSERTAR CATALOGO"
                            LogAuditoria.vQuery = Session("Query")
                            LogAuditoria.cIdSistema = Session("IdSistema")
                            LogAuditoria.cIdModulo = strOpcionModulo

                            FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                            txtIdCatalogo.Text = Catalogo.cIdCatalogo
                            MyValidator.ErrorMessage = "Transacción duplicada con éxito"
                            CargarCestaCatalogo()
                            Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                            Me.grdLista.DataBind()
                            pnlGeneral.Enabled = False
                            BloquearMantenimiento(True, False, True, False, True)
                            hfdOperacion.Value = "R"
                            hfTab.Value = "tab1"
                        End If
                    End If
                End If
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidarDuplicado"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarDuplicado"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarDuplicado"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarDuplicado"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub cboDescripcionDetalleCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDescripcionDetalleCatalogo.SelectedIndexChanged
        txtDescripcionDetalleCatalogo.Text = cboDescripcionDetalleCatalogo.SelectedItem.Value
        If cboDescripcionDetalleCatalogo.SelectedItem.Value = "" Then
            txtDescripcionDetalleCatalogo.Text = ""
        End If
        lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
    End Sub
End Class