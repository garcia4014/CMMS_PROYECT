Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmGnrlVideoAyuda
    Inherits System.Web.UI.Page
    Dim VideosNeg As New clsVideoTutorialNegocios
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
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("89", "CMMS", Session("IdEmpresa")) 'JMUG: 06/02/2023
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdVideo.Enabled = False
        txtTitulo.Enabled = bActivar
        txtDescripcion.Enabled = bActivar
        txtLinkYoutube.Enabled = bActivar
        txtTiempoMinutos.Enabled = bActivar
    End Sub

    Sub LlenarData(ByVal strIdVideo As String)
        Dim Videos As GNRL_VIDEOTUTORIAL = VideosNeg.VideoTutorialListarPorId(strIdVideo, "1")
        txtIdVideo.Text = Videos.cIdVideoTutorial
        txtTitulo.Text = Videos.vTituloVideoTutorial
        txtDescripcion.Text = Videos.vDescripcionVideoTutorial
        txtLinkYoutube.Text = Videos.vLinkYouTubeVideoTutorial
        txtTiempoMinutos.Text = Videos.vTiempoMinutosVideoTutorial
        hfdEstado.Value = IIf(Videos.bEstadoRegistroVideoTutorial = False, "0", "1")

        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
    End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvTitulo.EnableClientScript = bValidar
        Me.rfvDescripcion.EnableClientScript = bValidar
        Me.rfvLinkYouTube.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdVideo.Text = ""
        txtTitulo.Text = ""
        txtDescripcion.Text = ""
        hfdEstado.Value = "1"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "091" 'Mantenimiento de los Catálogos.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 3

            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)
            Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
            Me.rptVideoTutorial.DataBind()
        Else
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanListado_txtBuscar')")
            txtIdVideo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtTitulo')")
            txtTitulo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtLinkYoutube')")
            txtLinkYoutube.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContVideos_TabPanGeneral_txtTiempoMinutos')")
        End If
    End Sub
    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        Me.rptVideoTutorial.DataBind()
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        Me.rptVideoTutorial.DataBind()
    End Sub

    Protected Sub imgbtnBuscarVideos_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarVideos.Click
        Me.rptVideoTutorial.DataSource = VideosNeg.VideoTutorialListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
        Me.rptVideoTutorial.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0377", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            txtDescripcion.Focus()
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

    Protected Sub btnDeshacer_Click(sender As Object, e As EventArgs) Handles btnDeshacer.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0380", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "R"
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

            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0379", strOpcionModulo, Session("IdSistema"))

            Dim Videos As New GNRL_VIDEOTUTORIAL
            Videos.cIdVideoTutorial = UCase(txtIdVideo.Text)
            Videos.vTituloVideoTutorial = UCase(txtTitulo.Text) 'cboTipoActivo.SelectedValue
            Videos.vDescripcionVideoTutorial = UCase(txtDescripcion.Text)
            Videos.vLinkYouTubeVideoTutorial = txtLinkYoutube.Text
            Videos.dFechaRegistroVideoTutorial = Now
            Videos.vTiempoMinutosVideoTutorial = txtTiempoMinutos.Text
            Videos.bEstadoRegistroVideoTutorial = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
            Videos.vIdYouTubeVideoTutorial = Mid(Videos.vLinkYouTubeVideoTutorial, InStrRev(Videos.vLinkYouTubeVideoTutorial, "/") + 1, Len(Videos.vLinkYouTubeVideoTutorial))
            Videos.cIdSistemaVideoTutorial = Session("IdSistema")

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
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacion.Value = "R"
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

    Protected Sub rptVideoTutorial_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            If e.CommandName = "EditarVideo" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdEstado.Value = Valores(1).ToString()
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0378", strOpcionModulo, Session("IdSistema"))

                hfdOperacion.Value = "E"
                txtTitulo.Focus()
                BloquearMantenimiento(False, True, False, True)
                LimpiarObjetos() 'JMUG: 09/09/2023
                ValidarTexto(True)
                ActivarObjetos(True)
                LlenarData(Valores(0).ToString())
                hfTab.Value = "tab2"
                ValidationSummary1.ValidationGroup = "vgrpValidar"
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
End Class