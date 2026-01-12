Imports CapaNegocioCMMS
Imports CapaDatosCMMS
'----------------------------------
'Esto es para buscar el contenido del tipo de cambio
Imports HtmlAgilityPack
Imports System.Net
Imports System.IO

Public Class frmGnrlMntTipoCambio
    Inherits System.Web.UI.Page
    Dim TipoCambioNeg As New clsTipoCambioNegocios
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

    Sub CargarTipoCambioWebSunat(dFecha As Date)
        Try
            MyValidator.ErrorMessage = ""
            Dim booExiste As Boolean = False
            Dim sUrl As String = "http://www.sunat.gob.pe/cl-at-ittipcam/tcS01Alias"
            Dim objEncoding = Encoding.GetEncoding("ISO-8859-1")
            Dim objCookies = New CookieCollection

            'Usando GET
            Dim getRequest As HttpWebRequest = CType(WebRequest.Create(sUrl & "?mes=" & Format(Month(dFecha), "00") & "&anho=" & Format(Year(dFecha), "0000")), HttpWebRequest)
            getRequest.Credentials = CredentialCache.DefaultNetworkCredentials
            getRequest.ProtocolVersion = HttpVersion.Version11
            getRequest.UserAgent = ".NET Framework 4.0"
            getRequest.Method = "GET"

            getRequest.CookieContainer = New CookieContainer()
            getRequest.CookieContainer.Add(objCookies)

            'Como se puede ver usamos el Httpwebrequest para realizar la petición a la web de SUNAT 
            'y con esto deberíamos obtener una respuesta que se cargara en el Httpwebresponse que se muestra continuación.
            Dim sGetResponse = String.Empty

            Using getResponse As HttpWebResponse = CType(getRequest.GetResponse(), HttpWebResponse)
                objCookies = getResponse.Cookies
                Using srGetResponse = New StreamReader(getResponse.GetResponseStream(), objEncoding)
                    sGetResponse = srGetResponse.ReadToEnd
                End Using
            End Using

            'Obtenemos Información
            Dim document = New HtmlAgilityPack.HtmlDocument
            document.LoadHtml(sGetResponse)

            Dim strCadena As String = "//table[@class='class=""form-table""'] //tr"
            Dim NodesTr As HtmlAgilityPack.HtmlNodeCollection = document.DocumentNode.SelectNodes(strCadena)
            If (Not IsNothing(NodesTr)) Then
                Dim dt As New DataTable
                dt.Columns.Add("Día", GetType(System.String))
                dt.Columns.Add("Compra", GetType(System.String))
                dt.Columns.Add("Venta", GetType(System.String))

                Dim iNumFila As Integer = 0
                For Each Node In NodesTr
                    If iNumFila > 0 Then
                        Dim iNumColumna As Integer = 0
                        Dim dr As DataRow = dt.NewRow
                        For Each subNode In Node.Elements("td")
                            If iNumColumna = 0 Then
                                dr = dt.NewRow
                            End If
                            Dim sValue As String = subNode.InnerHtml.ToString.Trim
                            sValue = System.Text.RegularExpressions.Regex.Replace(sValue, "<.*?>", " ")
                            dr(iNumColumna) = sValue
                            iNumColumna = iNumColumna + 1
                            If (iNumColumna = 3) Then
                                dt.Rows.Add(dr)
                                iNumColumna = 0
                            End If
                        Next
                    End If
                    iNumFila = iNumFila + 1
                Next
                dt.AcceptChanges()
                For Each dtTipoCambio In dt.Rows
                    If Day(dFecha).ToString.Trim = dtTipoCambio(0).ToString.Trim Then
                        txtTCCompra.Text = dtTipoCambio(1)
                        txtTCVenta.Text = dtTipoCambio(2)
                        booExiste = True
                    End If
                Next
            End If
            If booExiste = False Then
                Throw New Exception("No existe la fecha seleccionada.")
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

    Sub CargarTipoCambioWebBIMSIC(dFecha As Date)
        Try
            Dim ConBIMSICNeg As New clsConsultasBIMSICNegocios
            Dim dtTipoCambio = ConBIMSICNeg.ConsultaBIMSICGetData("SELECT * FROM `tipo_cambio` WHERE `fecha_cambio` = '" & String.Format("{0:yyyy-MM-dd}", dFecha) & "'")

            dtTipoCambio.AcceptChanges()
            For Each dtTC In dtTipoCambio.Rows
                If Day(dFecha).ToString.Trim = Day(dtTC(0)).ToString.Trim Then
                    txtTCCompra.Text = dtTC(1)
                    txtTCVenta.Text = dtTC(2)
                End If
            Next
        Catch ex As Exception
            Dim MyValidator As New CustomValidator
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltro.DataTextField = "vDescripcionTablaSistema"
        cboFiltro.DataValueField = "vValor"
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("24", "GNRL", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ListarTipoMonedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoMonedaNeg As New clsTipoMonedaNegocios
        cboTipoMoneda.DataTextField = "vDescripcionTipoMoneda"
        cboTipoMoneda.DataValueField = "cIdTipoMoneda"
        cboTipoMoneda.DataSource = TipoMonedaNeg.TipoMonedaListarCombo
        cboTipoMoneda.Items.Clear()
        cboTipoMoneda.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtFecha.Enabled = IIf(hfdOperacion.Value = "N", bActivar, False) 'bActivar
        txtTCCompra.Enabled = bActivar
        txtTCVenta.Enabled = bActivar
        cboTipoMoneda.Enabled = bActivar
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
                            txtTCCompra.Focus()
                            BloquearPagina = False
                        End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "N" Then
                pnlListado.Enabled = False
                pnlGeneral.Enabled = True
                txtTCCompra.Focus()
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
            Dim TipoCambio As GNRL_TIPOCAMBIO = TipoCambioNeg.TipoCambioListarPorId(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(2).Text, Session("IdPais"))
            cboTipoMoneda.SelectedValue = TipoCambio.cIdTipoMoneda
            txtTCCompra.Text = IIf(String.IsNullOrEmpty(TipoCambio.nTipoCambioCompra) = True, "0", TipoCambio.nTipoCambioCompra)
            txtTCVenta.Text = IIf(String.IsNullOrEmpty(TipoCambio.nTipoCambioVenta) = True, "0", TipoCambio.nTipoCambioVenta)
            txtFecha.Text = IIf(String.IsNullOrEmpty(TipoCambio.dFechaHoraTipoCambio) = True, "0", TipoCambio.dFechaHoraTipoCambio)
            hfdEstado.Value = IIf(TipoCambio.bEstadoRegistroTipoCambio = False, "0", "1")

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
        Me.rfvFecha.EnableClientScript = bValidar
        Me.rfvTCCompra.EnableClientScript = bValidar
        Me.rfvTCVenta.EnableClientScript = bValidar
        Me.rfvTipoMoneda.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtTCCompra.Text = "0"
        txtTCVenta.Text = "0"
        cboTipoMoneda.SelectedIndex = -1
        txtFecha.Text = String.Format("{0:dd/MM/yyyy}", Now)
        hfdEstado.Value = "1"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            strOpcionModulo = "012" 'Modulo de Tipo de Cambio - Contabilidad.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarTipoMonedaCombo()

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 1
            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
            Me.grdLista.DataBind()
        Else
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTipoCambio_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTipoCambio_TabPanListado_imgbtnBuscarTipoCambio')")
            imgbtnBuscarTipoCambio.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTipoCambio_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTipoCambio_TabPanGeneral_txtFecha')")
            txtFecha.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTipoCambio_TabPanGeneral_cboTipoMoneda')")
            cboTipoMoneda.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTipoCambio_TabPanGeneral_txtTCCompra')")
            txtTCCompra.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContTipoCambio_TabPanGeneral_txtTCVenta')")
            txtTCVenta.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnGuardar')")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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
            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text) = "True", "1", "0")
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    Protected Sub imgbtnCargarTipoCambio_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnCargarTipoCambio.Click
        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
        Call CargarTipoCambioWebBIMSIC(String.Format("{0:dd/MM/yyyy}", txtFecha.Text))
        txtTCCompra.Focus()
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim TipoCambio As New GNRL_TIPOCAMBIO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0063", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                TipoCambio.dFechaHoraTipoCambio = Valores(0).ToString()

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

                TipoCambioNeg.TipoCambioGetData("UPDATE GNRL_TIPOCAMBIO SET bEstadoRegistroTipoCambio = 0 WHERE CONVERT(CHAR(10), dFechaHoraTipoCambio, 103) = '" & String.Format("{0:dd/MM/yyyy}", TipoCambio.dFechaHoraTipoCambio) & "'")
                Session("Query") = "UPDATE GNRL_TIPOCAMBIO SET bEstadoRegistroTipoCambio = 0 WHERE CONVERT(CHAR(10), dFechaHoraTipoCambio, 103) = '" & String.Format("{0:dd/MM/yyyy}", TipoCambio.dFechaHoraTipoCambio) & "'"
                LogAuditoria.vEvento = "DESACTIVAR TIPO CAMBIO"
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

                Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim TipoCambio As New GNRL_TIPOCAMBIO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0063", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                TipoCambio.dFechaHoraTipoCambio = Valores(0).ToString()

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

                TipoCambioNeg.TipoCambioGetData("UPDATE GNRL_TIPOCAMBIO SET bEstadoRegistroTipoCambio = 1 WHERE CONVERT(CHAR(10), dFechaHoraTipoCambio, 103) = '" & String.Format("{0:dd/MM/yyyy}", TipoCambio.dFechaHoraTipoCambio) & "'")
                Session("Query") = "UPDATE GNRL_TIPOCAMBIO SET bEstadoRegistroTipoCambio = 1 WHERE CONVERT(CHAR(10), dFechaHoraTipoCambio, 103) = '" & String.Format("{0:dd/MM/yyyy}", TipoCambio.dFechaHoraTipoCambio) & "'"
                LogAuditoria.vEvento = "ACTIVAR USUARIO"
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

                Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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

    Private Sub imgbtnBuscarTipoCambio_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarTipoCambio.Click
        Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0059", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0060", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0062", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0061", strOpcionModulo, Session("IdSistema"))

            If Convert.ToDateTime(txtFecha.Text) > Now Then
                Throw New Exception("La fecha no puede superar a la de hoy.")
            End If

            Dim TipoCambio As New GNRL_TIPOCAMBIO
            With TipoCambio
                .cIdTipoMoneda = cboTipoMoneda.SelectedValue
                .dFechaHoraTipoCambio = txtFecha.Text
                .nTipoCambioCompra = txtTCCompra.Text
                .nTipoCambioVenta = txtTCVenta.Text
                .bEstadoRegistroTipoCambio = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
                .cIdPaisOrigenTipoCambio = Session("IdPais")
                .cIdTipoMonedaBaseTipoCambio = Session("IdTMonedaBase")
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
                If (TipoCambioNeg.TipoCambioInserta(TipoCambio)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_TIPOCAMBIO 'SQL_INSERT', '', '" & TipoCambio.dFechaHoraTipoCambio & "', " & TipoCambio.nTipoCambioVenta & ", " &
                                       TipoCambio.nTipoCambioCompra & ", '" & TipoCambio.cIdTipoMoneda & "', '" & TipoCambio.bEstadoRegistroTipoCambio & "'"

                    LogAuditoria.vEvento = "INSERTAR TIPO CAMBIO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (TipoCambioNeg.TipoCambioEdita(TipoCambio)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_TIPOCAMBIO 'SQL_UPDATE', '', '" & TipoCambio.dFechaHoraTipoCambio & "', " & TipoCambio.nTipoCambioVenta & ", " &
                                       TipoCambio.nTipoCambioCompra & ", '" & TipoCambio.cIdTipoMoneda & "', '" & TipoCambio.bEstadoRegistroTipoCambio & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR TIPO CAMBIO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.grdLista.DataSource = TipoCambioNeg.TipoCambioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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