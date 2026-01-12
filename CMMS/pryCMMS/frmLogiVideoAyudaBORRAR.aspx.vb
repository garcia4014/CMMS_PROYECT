Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiVideoAyuda
    Inherits System.Web.UI.Page
    Dim VideosNeg As New clsVideoTutorialNegocios
    'Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator

    Public Sub CargarPerfil()
        'Dim AmigoNeg As New clsAmigoNegocios
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
            'ElseIf Session("IdConfEmpresa") = "" Then
            '    fValidarSesion = True
            '    'Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
            '    Throw New Exception("No se ha ingresado al sistema correctamente.")
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
                'Dim segmentosURL = HttpContext.Current.Request.Url.Segments
                'Response.Redirect(segmentosURL(segmentosURL.Length - 1))
            End If
        End If
    End Function

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltro.DataTextField = "vDescripcionTablaSistema"
        cboFiltro.DataValueField = "vValor"
        'cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("33", "LOGI", Session("IdEmpresa"), Session("IdPuntoVenta"))
        'cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("14", "LOGI", Session("IdEmpresa"))
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("89", "CMMS", Session("IdEmpresa")) 'JMUG: 06/02/2023
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    'Sub ListarTipoActivoCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim TipoActivoNeg As New clsTipoActivoNegocios
    '    cboTipoActivo.DataTextField = "vDescripcionTipoActivo"
    '    cboTipoActivo.DataValueField = "cIdTipoActivo"
    '    'cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo(Session("IdEmpresa"), Session("IdPuntoVenta"))
    '    cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
    '    'cboAlmacen.Items.Clear()
    '    cboTipoActivo.DataBind()
    'End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdVideo.Enabled = False
        txtTitulo.Enabled = bActivar
        txtDescripcion.Enabled = bActivar
        txtLinkYoutube.Enabled = bActivar
        txtTiempoMinutos.Enabled = bActivar
    End Sub

    'Private Function BloquearPagina(ByVal NroPagina As Integer) As Boolean
    '    BloquearPagina = True
    '    If NroPagina = 1 Then
    '        pnlListado.Enabled = True
    '        pnlGeneral.Enabled = False
    '        txtBuscar.Focus()
    '    ElseIf NroPagina = 2 Then
    '        If hfdOperacion.Value = "R" Or hfdOperacion.Value = "E" Or IsNothing(hfdOperacion.Value) = True Then
    '            'If grdLista.Rows.Count = 0 Then
    '            If rptVideoTutorial.Items.Count = 0 Then
    '            Else
    '                'If IsNothing(grdLista.SelectedRow) = True Then
    '                If IsNothing(rptVideoTutorial.Items. .SelectedRow) = True Then
    '                    ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '                    MyValidator.ErrorMessage = "Seleccione un registro a visualizar."
    '                    MyValidator.IsValid = False
    '                    MyValidator.ID = "ErrorPersonalizado"
    '                    MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '                    Me.Page.Validators.Add(MyValidator)
    '                    hfdOperacion.Value = "R"
    '                Else
    '                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        If hfdOperacion.Value = "R" Then
    '                            pnlListado.Enabled = True
    '                            LlenarData()
    '                        Else
    '                            pnlListado.Enabled = False
    '                        End If
    '                        pnlGeneral.Enabled = False
    '                        If hfdOperacion.Value = "E" Then pnlGeneral.Enabled = True
    '                        txtDescripcion.Focus()
    '                        BloquearPagina = False
    '                    End If
    '                End If
    '            End If
    '        ElseIf hfdOperacion.Value = "N" Then
    '            pnlListado.Enabled = False
    '            pnlGeneral.Enabled = True
    '            txtDescripcion.Focus()
    '            BloquearPagina = False
    '        End If
    '    ElseIf NroPagina = 0 Then
    '        If grdLista.Rows.Count = 0 Then
    '        Else
    '            If InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 1 Then
    '                pnlListado.Enabled = True
    '                pnlGeneral.Enabled = False
    '                txtBuscar.Focus()
    '                strTabContenedorActivo = 1
    '            ElseIf InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 0 Then
    '                pnlListado.Enabled = True
    '                pnlGeneral.Enabled = False
    '                txtBuscar.Focus()
    '                strTabContenedorActivo = 0
    '            Else
    '                If InStr(MyValidator.ErrorMessage, "eliminar") > 0 And strTabContenedorActivo = 0 Then
    '                    pnlListado.Enabled = True
    '                    pnlGeneral.Enabled = False
    '                Else
    '                End If
    '            End If
    '        End If
    '        pnlListado.Enabled = True
    '        pnlGeneral.Enabled = False
    '    End If
    'End Function

    Sub LlenarData()
        Try
            'JMUG: 07/09/2023 Dim Videos As GNRL_VIDEOTUTORIAL = VideosNeg.VideoTutorialListarPorId(grdLista.SelectedRow.Cells(1).Text, "1")
            Dim Videos As GNRL_VIDEOTUTORIAL = VideosNeg.VideoTutorialListarPorId("00001", "1")
            txtIdVideo.Text = Videos.cIdVideoTutorial
            txtTitulo.Text = Videos.vTituloVideoTutorial
            txtDescripcion.Text = Videos.vDescripcionVideoTutorial
            txtLinkYoutube.Text = Videos.vLinkYouTubeVideoTutorial
            txtTiempoMinutos.Text = Videos.vTiempoMinutosVideoTutorial
            'cboTipoActivo.SelectedValue = Videos.cIdTipoActivo
            'txtDimensiones.Text = Videos.vDimensionesVideos
            'txtVoltaje.Text = Videos.vVoltajeVideos
            'txtPeso.Text = Videos.vPesoVideos
            'txtVidaUtil.Text = IIf(IsNothing(Videos.nVidaUtilVideos), "", Videos.nVidaUtilVideos)
            'txtCuentaContable.Text = Videos.cIdCuentaContableVideos
            'txtCuentaContableLeasing.Text = Videos.cIdCuentaContableLeasingVideos
            hfdEstado.Value = IIf(Videos.bEstadoRegistroVideoTutorial = False, "0", "1")

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
        'Me.rfvTITULO.EnableClientScript = bValidar
        Me.rfvTitulo.EnableClientScript = bValidar
        Me.rfvDescripcion.EnableClientScript = bValidar
        Me.rfvLinkYouTube.EnableClientScript = bValidar
        'Me.rfvDescripcionAbreviada.EnableClientScript = bValidar
        'Me.rfvVidaUtil.EnableClientScript = bValidar
        'Me.rfvCuentaContable.EnableClientScript = bValidar
        'Me.rfvCuentaContableLeasing.EnableClientScript = bValidar
    End Sub

    'Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
    '    Me.imgbtnNuevo.Enabled = bNuevo
    '    If Me.imgbtnNuevo.Enabled = True Then
    '        Me.imgbtnNuevo.ImageUrl = "~/Imagenes/Nuevo.jpg"
    '    Else
    '        Me.imgbtnNuevo.ImageUrl = "~/Imagenes/NoEnabledNuevo.jpg"
    '    End If
    '    Me.imgbtnGuardar.Enabled = bGuardar
    '    If Me.imgbtnGuardar.Enabled = True Then
    '        Me.imgbtnGuardar.ImageUrl = "~/Imagenes/Guardar.jpg"
    '    Else
    '        Me.imgbtnGuardar.ImageUrl = "~/Imagenes/NoEnabledGuardar.jpg"
    '    End If
    '    Me.imgbtnEditar.Enabled = bEditar
    '    If Me.imgbtnEditar.Enabled = True Then
    '        Me.imgbtnEditar.ImageUrl = "~/Imagenes/Editar.jpg"
    '    Else
    '        Me.imgbtnEditar.ImageUrl = "~/Imagenes/NoEnabledEditar.jpg"
    '    End If
    '    Me.imgbtnDeshacer.Enabled = bDeshacer
    '    If Me.imgbtnDeshacer.Enabled = True Then
    '        Me.imgbtnDeshacer.ImageUrl = "~/Imagenes/Deshacer.jpg"
    '    Else
    '        Me.imgbtnDeshacer.ImageUrl = "~/Imagenes/NoEnabledDeshacer.jpg"
    '    End If
    '    'Me.imgbtnEliminar.Enabled = bEliminar
    '    'If Me.imgbtnEliminar.Enabled = True Then
    '    '    Me.imgbtnEliminar.ImageUrl = "~/Imagenes/Eliminar.jpg"
    '    'Else
    '    '    Me.imgbtnEliminar.ImageUrl = "~/Imagenes/NoEnabledEliminar.jpg"
    '    'End If
    'End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        'Me.lblMensaje.Text = ""
        MyValidator.ErrorMessage = ""
        txtIdVideo.Text = ""
        txtTitulo.Text = ""
        'cboTipoActivo.SelectedIndex = -1
        txtDescripcion.Text = ""
        'txtDescripcionAbreviada.Text = ""
        'txtDimensiones.Text = ""
        'txtVoltaje.Text = ""
        'txtPeso.Text = ""
        'txtVidaUtil.Text = ""
        'txtCuentaContable.Text = ""
        'txtCuentaContableLeasing.Text = ""
        hfdEstado.Value = "1"
    End Sub

    'Sub LimpiarObjetosCaracteristicas()
    '    Me.lblMensajeCaracteristica.Text = ""
    '    txtValorCaracteristica.Text = ""
    '    txtIdReferenciaSAPCaracteristica.Text = ""
    '    txtDescripcionCampoSAPCaracteristica.Text = ""
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "084" 'Mantenimiento de los Catálogos.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 3
            'ListarTipoActivoCombo()

            'If Session("CestaVideos") Is Nothing Then
            '    Session("CestaVideos") = clsLogiCestaVideos.CrearCesta
            'Else
            '    clsLogiCestaVideos.VaciarCesta(Session("CestaVideos"))
            'End If

            'JMUG: 07/09/2023 BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)


            'If Session("CestaVideosCaracteristica") Is Nothing Then
            '    Session("CestaVideosCaracteristica") = clsLogiCestaVideosCaracteristica.CrearCesta
            'Else
            '    clsLogiCestaVideos.VaciarCesta(Session("CestaVideosCaracteristica"))
            'End If
            'Me.grdDetalleCaracteristica.DataSource = Session("CestaVideosCaracteristica")
            'Me.grdDetalleCaracteristica.DataBind()

            'Me.grdLista.DataSource = VideosNeg.VideosListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", Session("IdEmpresa"), "*")
            Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
            Me.rptVideoTutorial.DataBind()
        Else
            'Me.imgbtnEliminar.Attributes.Add("onclick", "if(!confirm('Seguro desea eliminar el registro?')){return false;};")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanListado_txtBuscar')")
            txtIdVideo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtTitulo')")
            txtTitulo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtDescripcion')")
            'cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtLinkYoutube')")
            txtLinkYoutube.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtTiempoMinutos')")
            'txtTiempoMinutos.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtDescripcioAbreviada')")

            'btnNuevo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnGuardar')")
            'btnGuardar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnEditar')")
            'btnEditar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnDeshacer')")
            'btnDeshacer.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnEliminar')")
            'If hfdOperacion.Value = "E" Or hfdOperacion.Value = "N" Then
            '    BloquearPagina(2)
            'End If
        End If
    End Sub
    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        'Me.lblMensaje.Text = ""
        MyValidator.ErrorMessage = ""
        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        Me.rptVideoTutorial.DataBind()
        'JMUG 08/09/2023 Me.rptVideoTutorial.SelectedIndex = 0
    End Sub

    'Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
    '    grdLista.PageIndex = e.NewPageIndex
    '    Me.grdLista.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
    '    Me.grdLista.DataBind() 'Recargo el grid.
    '    grdLista.SelectedIndex = -1
    '    'JMUG: 07/09/2023 BloquearPagina(1)
    'End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        'Me.grdLista.DataSource = VideosNeg.VideosListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", Session("IdEmpresa"), "*")
        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        Me.rptVideoTutorial.DataBind()
    End Sub

    'Private Sub grdLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim td As TableCell
    '        For Each td In e.Row.Cells
    '            e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
    '            e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
    '            e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
    '            e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Left
    '            e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left
    '        Next
    '    End If
    'End Sub

    'Private Sub grdLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(1).Visible = True
    '        e.Row.Cells(2).Visible = True
    '        e.Row.Cells(3).Visible = False
    '        e.Row.Cells(4).Visible = True
    '        e.Row.Cells(5).Visible = True
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(1).Visible = True
    '        e.Row.Cells(2).Visible = True
    '        e.Row.Cells(3).Visible = False
    '        e.Row.Cells(4).Visible = True
    '        e.Row.Cells(5).Visible = True
    '    End If
    'End Sub

    'Private Sub grdLista_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdLista.SelectedIndexChanged
    '    MyValidator.ErrorMessage = ""
    '    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '        BloquearPagina(0)
    '        ValidarTexto(False)
    '        LlenarData() 'JMUG: 06/02/2023
    '    End If
    'End Sub

    'Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
    'Protected Sub rptVideoTutorial_RowCommand_Botones(sender As Object, e As DataListCommandEventArgs)
    '    Try
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        If e.CommandName = "Activar" Then
    '            Dim Videos As New GNRL_VIDEOTUTORIAL
    '            'If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "0069", strOpcionModulo, Session("IdSistema"), Session("IdArea")) Then
    '            If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0381", strOpcionModulo, Session("IdSistema")) Then
    '                Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
    '            End If

    '            Dim Valores() As String = e.CommandArgument.ToString.Split(",")
    '            'Valores(0).ToString() 'Codigo.

    '            Videos.cIdVideoTutorial = Valores(0).ToString()
    '            'Videos.cIdTipoActivo = Valores(1).ToString()
    '            'TipoActivo.cIdEmpresa = Session("IdEmpresa")
    '            'TipoActivo.cIdPuntoVenta = Session("IdPuntoVenta")

    '            Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '            LogAuditoria.cIdPaisOrigen = Session("IdPais")
    '            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '            LogAuditoria.cIdLocal = Session("IdLocal")
    '            'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
    '            'LogAuditoria.cIdUsuario = Session("IdUsuario")
    '            LogAuditoria.cIdUsuario = Session("IdUsuario")
    '            LogAuditoria.vIP1 = Session("IP1")
    '            LogAuditoria.vIP2 = Session("IP2")
    '            LogAuditoria.vPagina = Session("URL")
    '            LogAuditoria.vSesion = Session("IdSesion")

    '            VideosNeg.VideoTutorialGetData("UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 0 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
    '            Session("Query") = "UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 0 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
    '            LogAuditoria.vEvento = "DESACTIVAR VIDEO TUTORIAL"
    '            LogAuditoria.vQuery = Session("Query")
    '            LogAuditoria.cIdSistema = Session("IdSistema")
    '            LogAuditoria.cIdModulo = strOpcionModulo

    '            FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

    '            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '            MyValidator.ErrorMessage = "Registro desactivado con éxito"
    '            MyValidator.IsValid = False
    '            MyValidator.ID = "ErrorPersonalizado"
    '            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '            Me.Page.Validators.Add(MyValidator)

    '            Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
    '            Me.rptVideoTutorial.DataBind()
    '        End If
    '        If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
    '            MyValidator.ErrorMessage = ""
    '            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '            Dim Videos As New GNRL_VIDEOTUTORIAL
    '            If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0381", strOpcionModulo, Session("IdSistema")) Then
    '                Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
    '            End If

    '            Dim Valores() As String = e.CommandArgument.ToString.Split(",")
    '            'Valores(0).ToString() 'Codigo.

    '            Videos.cIdVideoTutorial = Valores(0).ToString()
    '            'Videos.cIdTipoActivo = Valores(1).ToString()
    '            'Producto.cIdEmpresa = Session("IdEmpresa")
    '            'Producto.cIdPuntoVenta = Session("IdPuntoVenta")

    '            Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '            LogAuditoria.cIdPaisOrigen = Session("IdPais")
    '            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '            LogAuditoria.cIdLocal = Session("IdLocal")
    '            'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
    '            LogAuditoria.cIdUsuario = Session("IdUsuario")
    '            LogAuditoria.vIP1 = Session("IP1")
    '            LogAuditoria.vIP2 = Session("IP2")
    '            LogAuditoria.vPagina = Session("URL")
    '            LogAuditoria.vSesion = Session("IdSesion")

    '            VideosNeg.VideoTutorialGetData("UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 1 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
    '            Session("Query") = "UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 1 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
    '            LogAuditoria.vEvento = "ACTIVAR VIDEO TUTORIAL"
    '            LogAuditoria.vQuery = Session("Query")
    '            LogAuditoria.cIdSistema = Session("IdSistema")
    '            LogAuditoria.cIdModulo = strOpcionModulo

    '            FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

    '            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '            MyValidator.ErrorMessage = "Registro activado con éxito"
    '            MyValidator.IsValid = False
    '            MyValidator.ID = "ErrorPersonalizado"
    '            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '            Me.Page.Validators.Add(MyValidator)

    '            'Me.grdLista.DataSource = ProductoNeg.ProductoListaBusqueda(IIf(chkFiltrarSubFamilia.Checked = True, "ISNULL(cIdSubFamiliaProducto, '') = '' AND ", "") & cboFiltro.SelectedValue,
    '            '                                                       txtBuscar.Text, Session("IdEmpresa"), "*")
    '            Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
    '            Me.rptVideoTutorial.DataBind()
    '        End If
    '    Catch ex As Exception
    '        'lblMensaje.Text = ex.Message
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub imgbtnBuscarVideos_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarVideos.Click
    Protected Sub imgbtnBuscarVideos_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarVideos.Click
        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        Me.rptVideoTutorial.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            'lblMensaje.Text = ""
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'JMUG: 08/02/2023
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "0065", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0377", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            'JMUG: 07/09/2023 BloquearPagina(2)
            txtDescripcion.Focus()
            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ValidarTexto(True)
            ActivarObjetos(True)
            hfTab.Value = "tab2"
            'ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            ValidationSummary1.ValidationGroup = "vgrpValidar"
        Catch ex As Exception
            'ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarBusqueda"
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
            'lblMensaje.Text = ""
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'JMUG: 08/02/2023
            ''FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "0066", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0378", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "E"
            'JMUG: 07/09/2023 If BloquearPagina(2) = False Then
            BloquearMantenimiento(False, True, False, True)
            ValidarTexto(True)
            ActivarObjetos(True)
            LlenarData()
            hfTab.Value = "tab2"
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            'JMUG: 07/09/2023 End If
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
            'JMUG: 08/02/2023
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "0068", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0380", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "R"
            'JMUG: 07/09/2023 BloquearPagina(0)
            BloquearMantenimiento(True, False, True, False)
            ValidarTexto(False)
            ActivarObjetos(False)
            hfTab.Value = "tab1"
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
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

            'JMUG: 08/02/2023
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "0067", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0379", strOpcionModulo, Session("IdSistema"))

            'If cboTipoActivo.SelectedIndex = 0 Then
            '    Throw New Exception("Debe de seleccionar un tipo de activo.")
            'End If

            Dim Videos As New GNRL_VIDEOTUTORIAL
            Videos.cIdVideoTutorial = UCase(txtIdVideo.Text)
            Videos.vTituloVideoTutorial = UCase(txtTitulo.Text) 'cboTipoActivo.SelectedValue
            Videos.vDescripcionVideoTutorial = UCase(txtDescripcion.Text)
            Videos.vLinkYouTubeVideoTutorial = txtLinkYoutube.Text
            Videos.dFechaRegistroVideoTutorial = Now
            Videos.vTiempoMinutosVideoTutorial = txtTiempoMinutos.Text
            Videos.bEstadoRegistroVideoTutorial = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
            Videos.vIdYouTubeVideoTutorial = Mid(Videos.vLinkYouTubeVideoTutorial, InStrRev(Videos.vLinkYouTubeVideoTutorial, "/") + 1, Len(Videos.vLinkYouTubeVideoTutorial))

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

            If hfdOperacion.Value = "N" Then
                If (VideosNeg.VideoTutorialInserta(Videos)) = 0 Then
                    'If (CaracteristicaNeg.CaracteristicaVideosInsertaDetalle(Videos, ColeccionCatCar, LogAuditoria)) Then
                    Session("Query") = "PA_GNRL_MNT_VIDEOTUTORIAL 'SQL_INSERT', '','" & Videos.cIdVideoTutorial & "', '" &
                                                   Videos.vTituloVideoTutorial & "', '" & Videos.vDescripcionVideoTutorial & "', '" & Videos.vLinkYouTubeVideoTutorial & "', '" &
                                                   Videos.dFechaRegistroVideoTutorial & "', '" & Videos.vTiempoMinutosVideoTutorial & "', '" &
                                                   Videos.bEstadoRegistroVideoTutorial & "', '" & Videos.cIdVideoTutorial & "'"

                    LogAuditoria.vEvento = "INSERTAR VIDEO TUTORIAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdVideo.Text = Videos.cIdVideoTutorial

                    'lblMensaje.Text = "Transacción registrada con éxito"
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"

                    Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
                    Me.rptVideoTutorial.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (VideosNeg.VideoTutorialEdita(Videos)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_VIDEOTUTORIAL 'SQL_UPDATE', '','" & Videos.cIdVideoTutorial & "', '" &
                                                   Videos.vTituloVideoTutorial & "', '" & Videos.vDescripcionVideoTutorial & "', '" & Videos.vLinkYouTubeVideoTutorial & "', '" &
                                                   Videos.dFechaRegistroVideoTutorial & "', '" & Videos.vTiempoMinutosVideoTutorial & "', '" &
                                                   Videos.bEstadoRegistroVideoTutorial & "', '" & Videos.cIdVideoTutorial & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR VIDEO TUTORIAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
                    Me.rptVideoTutorial.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    'hfdOperacion.Value = "R"
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    'Exit Sub
                End If
            End If
            hfdOperacion.Value = "R"
            'JMUG: 07/09/2023 BloquearPagina(0)
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub rptVideoTutorial_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        'Try
        '    MyValidator.ErrorMessage = ""
        '    fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
        '    If e.CommandName = "EditarVideo" Then
        '        Dim Videos As New GNRL_VIDEOTUTORIAL
        '        'If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "0069", strOpcionModulo, Session("IdSistema"), Session("IdArea")) Then
        '        If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0381", strOpcionModulo, Session("IdSistema")) Then
        '            Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
        '        End If

        '        Dim Valores() As String = e.CommandArgument.ToString.Split(",")
        '        'Valores(0).ToString() 'Codigo.

        '        Videos.cIdVideoTutorial = Valores(0).ToString()
        '        'Videos.cIdTipoActivo = Valores(1).ToString()
        '        'TipoActivo.cIdEmpresa = Session("IdEmpresa")
        '        'TipoActivo.cIdPuntoVenta = Session("IdPuntoVenta")

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

        '        VideosNeg.VideoTutorialGetData("UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 0 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
        '        Session("Query") = "UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 0 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
        '        LogAuditoria.vEvento = "DESACTIVAR VIDEO TUTORIAL"
        '        LogAuditoria.vQuery = Session("Query")
        '        LogAuditoria.cIdSistema = Session("IdSistema")
        '        LogAuditoria.cIdModulo = strOpcionModulo

        '        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

        '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        '        MyValidator.ErrorMessage = "Registro desactivado con éxito"
        '        MyValidator.IsValid = False
        '        MyValidator.ID = "ErrorPersonalizado"
        '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        '        Me.Page.Validators.Add(MyValidator)

        '        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        '        Me.rptVideoTutorial.DataBind()
        '    End If
        '    If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
        '        MyValidator.ErrorMessage = ""
        '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
        '        Dim Videos As New GNRL_VIDEOTUTORIAL
        '        If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0381", strOpcionModulo, Session("IdSistema")) Then
        '            Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
        '        End If

        '        Dim Valores() As String = e.CommandArgument.ToString.Split(",")
        '        'Valores(0).ToString() 'Codigo.

        '        Videos.cIdVideoTutorial = Valores(0).ToString()
        '        'Videos.cIdTipoActivo = Valores(1).ToString()
        '        'Producto.cIdEmpresa = Session("IdEmpresa")
        '        'Producto.cIdPuntoVenta = Session("IdPuntoVenta")

        '        Dim LogAuditoria As New GNRL_LOGAUDITORIA
        '        LogAuditoria.cIdPaisOrigen = Session("IdPais")
        '        LogAuditoria.cIdEmpresa = Session("IdEmpresa")
        '        LogAuditoria.cIdLocal = Session("IdLocal")
        '        'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
        '        LogAuditoria.cIdUsuario = Session("IdUsuario")
        '        LogAuditoria.vIP1 = Session("IP1")
        '        LogAuditoria.vIP2 = Session("IP2")
        '        LogAuditoria.vPagina = Session("URL")
        '        LogAuditoria.vSesion = Session("IdSesion")

        '        VideosNeg.VideoTutorialGetData("UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 1 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
        '        Session("Query") = "UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 1 WHERE cIdVideoTutorial = '" & Videos.cIdVideoTutorial & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
        '        LogAuditoria.vEvento = "ACTIVAR VIDEO TUTORIAL"
        '        LogAuditoria.vQuery = Session("Query")
        '        LogAuditoria.cIdSistema = Session("IdSistema")
        '        LogAuditoria.cIdModulo = strOpcionModulo

        '        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

        '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        '        MyValidator.ErrorMessage = "Registro activado con éxito"
        '        MyValidator.IsValid = False
        '        MyValidator.ID = "ErrorPersonalizado"
        '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        '        Me.Page.Validators.Add(MyValidator)

        '        'Me.grdLista.DataSource = ProductoNeg.ProductoListaBusqueda(IIf(chkFiltrarSubFamilia.Checked = True, "ISNULL(cIdSubFamiliaProducto, '') = '' AND ", "") & cboFiltro.SelectedValue,
        '        '                                                       txtBuscar.Text, Session("IdEmpresa"), "*")
        '        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        '        Me.rptVideoTutorial.DataBind()
        '    End If
        'Catch ex As Exception
        '    'lblMensaje.Text = ex.Message
        '    ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        '    MyValidator.ErrorMessage = ex.Message
        '    MyValidator.IsValid = False
        '    MyValidator.ID = "ErrorPersonalizado"
        '    MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        '    Me.Page.Validators.Add(MyValidator)
        'End Try
    End Sub
End Class