Imports CapaNegocioCMMS
Imports CapaDatosCMMS
'----------------------------------

Imports HtmlAgilityPack
Imports System.Net
Imports System.IO

Imports Tesseract 'Herramienta instalada desde NuGet que se utiliza para capturar el texto desde una imagen.  Se copió la carpeta \bin\tessdata después de la instalación del paquete. 

Public Class frmGnrlMntCliente
    Inherits System.Web.UI.Page
    Dim ClienteNeg As New clsClienteNegocios
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
#Region "VARIABLES"

    Private miCookie As New CookieContainer

    Public Enum eResultadoConsulta
        ConsultaSatisfactoria = 1
        ErrorDeCaptcha = 2
        RucNoEncontrado = 3
        Otro = 4
    End Enum

#End Region

#Region "VOID"

    Public Function ObtieneDatosSunat(ByVal str_dni As String, ByVal str_captcha As String, ByVal _txtRzSoc As TextBox, ByVal _txt_tipoemp As TextBox, ByVal _txtNomCom As TextBox, ByVal _txtFecIns As TextBox, ByVal _txtFecIni As TextBox, ByVal _txtEstCont As TextBox, ByVal _txtCondCont As TextBox, ByVal _txtDirec As TextBox, ByVal _txtTelef As TextBox) As eResultadoConsulta

        Dim urlReniec As String = String.Format("http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc={0}&codigo={1}&tipdoc=1", str_dni, str_captcha)
        Dim enlaceReniec As HttpWebRequest = WebRequest.Create(urlReniec)
        enlaceReniec.CookieContainer = Me.miCookie

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
        enlaceReniec.Credentials = CredentialCache.DefaultCredentials

        Dim respuesta_web As WebResponse = enlaceReniec.GetResponse

        Dim myStream As Stream = respuesta_web.GetResponseStream

        Dim myStreamReader As New StreamReader(myStream)

        Dim Count As Integer = 0
        Dim LineaVerificacionTexto As String = ""

        Dim Adicionar As Integer = 0
        If Me.txtRuc.Text.Substring(0, 1) = "1" Then 'RUC 10
            Adicionar = 4
        End If

        While LineaVerificacionTexto = myStreamReader.ReadLine <> vbNull

            Count += 1

            LineaVerificacionTexto = myStreamReader.ReadLine.Trim

            Select Case Count
                Case 15
                    If LineaVerificacionTexto = "<td class=beta>" Then
                        Return eResultadoConsulta.RucNoEncontrado
                    End If
                Case 25
                    If LineaVerificacionTexto.Remove(0, 17) = "El codigo ingresado es incorrecto</p>" Then
                        Return eResultadoConsulta.ErrorDeCaptcha
                    End If
                Case 61
                    _txtRzSoc.Text = LineaVerificacionTexto '<td  class="bg" colspan=3>20482736450 - EMDERSOFT S.A.C.</td>
                    Me.quitaEtiquetas(_txtRzSoc)
                    _txtRzSoc.Text = _txtRzSoc.Text.Remove(0, 15)
                Case 63
                    _txt_tipoemp.Text = LineaVerificacionTexto '<td class="bg" colspan=3>SOCIEDAD ANONIMA CERRADA</td>
                    Me.quitaEtiquetas(_txt_tipoemp)
                Case 66
                    _txtNomCom.Text = LineaVerificacionTexto
                Case 68 + Adicionar
                    _txtFecIns.Text = LineaVerificacionTexto '<td class="bg" colspan=1>28/10/2010</td>
                    If Me.txtRuc.Text.Substring(0, 1) = "1" Then
                        _txtFecIns.Text = myStreamReader.ReadLine.Trim
                    End If
                    Me.quitaEtiquetas(_txtFecIns)
                Case 69 + Adicionar
                    _txtFecIni.Text = LineaVerificacionTexto '<td class="bg" colspan=1> 29/11/2010</td>
                    Me.quitaEtiquetas(_txtFecIni)
                Case 71 + Adicionar
                    _txtEstCont.Text = LineaVerificacionTexto '<td class="bg" colspan=1>ACTIVO</td>
                    Me.quitaEtiquetas(_txtEstCont)
                Case 76 + Adicionar
                    _txtCondCont.Text = LineaVerificacionTexto
                Case 79 + Adicionar
                    _txtDirec.Text = LineaVerificacionTexto '<td class="bg" colspan=3>----PR.MIRAFLORES NRO. 2099 URB.  MANPUESTO (COSTADO PARROQUIA SAN ESTEBAN)LA LIBERTAD  
                    Me.quitaEtiquetas(_txtDirec)
                    _txtDirec.Text = _txtDirec.Text.Replace("----", "")
                    _txtDirec.Text = _txtDirec.Text.Trim
                Case 81 + Adicionar
                    LineaVerificacionTexto = myStreamReader.ReadLine.Trim
                    Me.txtTelefono.Text = LineaVerificacionTexto '<!--              <td class="bg" colspan=1>213292 / 948543560 </td> -->
                    Me.txtTelefono.Text = Me.txtTelefono.Text.Replace("<!--              ", "")
                    Me.txtTelefono.Text = Me.txtTelefono.Text.Replace(" -->", "")
                    Me.quitaEtiquetas(txtTelefono)
                    Return eResultadoConsulta.ConsultaSatisfactoria
            End Select

        End While

        Return eResultadoConsulta.Otro

    End Function

    Private Sub quitaEtiquetas(ByVal _txt As TextBox)
        _txt.Text = _txt.Text.Remove(0, 25)
        _txt.Text = _txt.Text.Replace("</td>", "")
    End Sub


#End Region

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltro.DataTextField = "vDescripcionTablaSistema"
        cboFiltro.DataValueField = "vValor"
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("03", "VTAS", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ListarTipoDocumentoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboTipoDocumento.DataTextField = "vDescripcionTablaSistema"
        cboTipoDocumento.DataValueField = "vValor"
        cboTipoDocumento.DataSource = FiltroNeg.TablaSistemaListarCombo("01", "CTBL", Session("IdEmpresa"))
        cboTipoDocumento.Items.Clear()
        cboTipoDocumento.DataBind()
    End Sub

    Sub ListarTipoPersonaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipPerNeg As New clsTipoPersonaNegocios
        cboTipoPersona.DataTextField = "vDescripcionTipoPersona"
        cboTipoPersona.DataValueField = "cIdTipoPersona"
        cboTipoPersona.DataSource = TipPerNeg.TipoPersonaListarCombo
        cboTipoPersona.DataBind()
    End Sub

    Sub ListarPaisCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboPais.DataTextField = "vDescripcionUbicacionGeografica"
        cboPais.DataValueField = "cIdPaisUbicacionGeografica"
        cboPais.DataSource = UbicacionGeograficaNeg.PaisListarCombo
        cboPais.DataBind()
    End Sub

    Sub ListarDepartamentoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDepartamento.DataTextField = "vDescripcionUbicacionGeografica"
        cboDepartamento.DataValueField = "cIdDepartamentoUbicacionGeografica"
        cboDepartamento.DataSource = UbicacionGeograficaNeg.DepartamentoListarCombo(cboPais.SelectedValue)
        cboDepartamento.Items.Clear()
        cboDepartamento.DataBind()
    End Sub

    Sub ListarProvinciaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboProvincia.DataTextField = "vDescripcionUbicacionGeografica"
        cboProvincia.DataValueField = "cIdProvinciaUbicacionGeografica"
        cboProvincia.DataSource = UbicacionGeograficaNeg.ProvinciaListarCombo(cboPais.SelectedValue, cboDepartamento.SelectedValue)
        cboProvincia.Items.Clear()
        cboProvincia.DataBind()
    End Sub

    Sub ListarDistritoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDistrito.DataTextField = "vDescripcionUbicacionGeografica"
        cboDistrito.DataValueField = "cIdDistritoUbicacionGeografica"
        cboDistrito.DataSource = UbicacionGeograficaNeg.DistritoListarCombo(cboPais.SelectedValue, cboDepartamento.SelectedValue, cboProvincia.SelectedValue)
        cboDistrito.Items.Clear()
        cboDistrito.DataBind()
    End Sub

    Sub ListarEstadoCivilCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim EstCivNeg As New clsEstadoCivilNegocios
        cboEstadoCivil.DataTextField = "vDescripcionEstadoCivil"
        cboEstadoCivil.DataValueField = "cIdEstadoCivil"
        cboEstadoCivil.DataSource = EstCivNeg.EstadoCivilListarCombo()
        cboEstadoCivil.Items.Clear()
        cboEstadoCivil.DataBind()
    End Sub

    Sub ListarTipoClienteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipCliNeg As New clsTablaSistemaNegocios
        cboTipoCliente.DataTextField = "vDescripcionTablaSistema"
        cboTipoCliente.DataValueField = "vValor"
        cboTipoCliente.DataSource = TipCliNeg.TablaSistemaListarCombo("47", "VTAS", Session("IdEmpresa"))
        cboTipoCliente.Items.Clear()
        cboTipoCliente.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        cboTipoPersona.Enabled = bActivar
        txtRuc.Enabled = bActivar
        txtRazonSocial.Enabled = bActivar
        txtRepresentanteLegal.Enabled = bActivar
        cboTipoDocumento.Enabled = bActivar
        txtDni.Enabled = bActivar
        txtNombres.Enabled = bActivar
        txtApellidoPaterno.Enabled = bActivar
        txtApellidoMaterno.Enabled = bActivar
        cboPais.Enabled = bActivar
        cboDepartamento.Enabled = bActivar
        cboProvincia.Enabled = bActivar
        cboDistrito.Enabled = bActivar
        txtDireccion.Enabled = bActivar
        txtTelefono.Enabled = bActivar
        txtCelular.Enabled = bActivar
        txtFax.Enabled = bActivar
        txtEmail.Enabled = bActivar
        chkAceptante.Enabled = bActivar
        optM.Enabled = bActivar
        optF.Enabled = bActivar
        txtFechaNacimiento.Enabled = bActivar
        cboEstadoCivil.Enabled = bActivar
        txtEstadoContribuyente.Enabled = bActivar
        txtCondicionContribuyente.Enabled = bActivar
        txtTipoEmpresa.Enabled = bActivar
        txtFechaCreacion.Enabled = bActivar
        txtTipoEmpresa.Enabled = bActivar
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
                            cboTipoPersona.Focus()
                            BloquearPagina = False
                        End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "N" Then
                pnlListado.Enabled = False
                pnlGeneral.Enabled = True
                cboTipoPersona.Focus()
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
            Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorId(grdLista.SelectedRow.Cells(1).Text, Session("IdEmpresa"), Session("IdPuntoVenta"))
            txtIdCliente.Text = Cliente.cIdCliente
            txtApellidoMaterno.Text = Cliente.vApellidoMaternoCliente
            txtApellidoPaterno.Text = Cliente.vApellidoPaternoCliente
            txtNombres.Text = Cliente.vNombresCliente
            txtRepresentanteLegal.Text = Cliente.vRepresentanteLegalCliente
            txtDireccion.Text = Cliente.vDireccionCliente
            txtDni.Text = Cliente.vDniCliente
            txtEmail.Text = Cliente.vEmailCliente
            txtFax.Text = Cliente.vFaxCliente
            txtRazonSocial.Text = Cliente.vRazonSocialCliente
            txtRuc.Text = Cliente.vRucCliente
            txtTelefono.Text = Cliente.vTelefonoCliente
            txtCelular.Text = Cliente.vCelularCliente
            chkAceptante.Checked = IIf(IsDBNull(Cliente.bEstadoAceptanteCliente) = True, False, Cliente.bEstadoAceptanteCliente)
            cboTipoPersona.SelectedValue = Cliente.cIdTipoPersona
            cboTipoCliente.SelectedValue = IIf(IsDBNull(Cliente.cIdTipoCliente) = True, "01", Cliente.cIdTipoCliente)
            cboPais.SelectedValue = Cliente.cIdPaisUbicacionGeografica
            cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())
            cboDepartamento.SelectedValue = Cliente.cIdDepartamentoUbicacionGeografica
            cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())
            cboProvincia.SelectedValue = Cliente.cIdProvinciaUbicacionGeografica
            cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())
            cboDistrito.SelectedValue = Cliente.cIdDistritoUbicacionGeografica
            IIf(IsNothing(Cliente.cGeneroCliente) = True, True, IIf(Cliente.cGeneroCliente.ToString.Trim = "M", True, False))
            optF.Checked = IIf(IsNothing(Cliente.cGeneroCliente) = True, False, IIf(Cliente.cGeneroCliente.ToString.Trim = "F", True, False))
            txtFechaNacimiento.Text = IIf(IsDBNull(Cliente.dFechaNacimientoCliente) = True, "", Cliente.dFechaNacimientoCliente)
            cboEstadoCivil.SelectedValue = IIf(IsDBNull(Cliente.cIdEstadoCivil) = True, "", Cliente.cIdEstadoCivil)
            cboTipoDocumento.SelectedValue = IIf(IsDBNull(Cliente.cIdTipoDocumento) = True, "", Cliente.cIdTipoDocumento)
            If cboTipoDocumento.SelectedValue = "04" Then 'RUC
                cboTipoDocumento.Enabled = False
            Else
                cboTipoDocumento.Enabled = True
            End If
            txtTipoEmpresa.Text = IIf(IsDBNull(Cliente.vTipoEmpresaCliente) = True, "", Cliente.vTipoEmpresaCliente)
            txtEstadoContribuyente.Text = IIf(IsDBNull(Cliente.vEstadoCliente) = True, "", Cliente.vEstadoCliente)
            txtCondicionContribuyente.Text = IIf(IsDBNull(Cliente.vCondicionCliente) = True, "", Cliente.vCondicionCliente)
            hfdEstado.Value = IIf(Cliente.bEstadoRegistroCliente = False, "0", "1")

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
        Me.rfvDireccion.EnableClientScript = bValidar
        Me.rfvEmail.EnableClientScript = bValidar
        Me.rfvPais.EnableClientScript = bValidar
        Me.rfvDepartamento.EnableClientScript = bValidar
        Me.rfvProvincia.EnableClientScript = bValidar
        Me.rfvDistrito.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos(Optional bLimpiar As Boolean = True)
        MyValidator.ErrorMessage = ""
        txtIdCliente.Text = IIf(hfdOperacion.Value = "E", txtIdCliente.Text, "")
        txtRepresentanteLegal.Text = ""
        txtApellidoMaterno.Text = ""
        txtApellidoPaterno.Text = ""
        txtCelular.Text = ""
        txtDireccion.Text = ""
        txtDni.Text = IIf(bLimpiar = True, "", txtDni.Text)
        txtEmail.Text = ""
        txtFax.Text = ""
        txtNombres.Text = ""
        txtRazonSocial.Text = ""
        txtRuc.Text = IIf(bLimpiar = True, "", txtRuc.Text)
        txtTelefono.Text = ""
        txtFechaNacimiento.Text = ""
        chkAceptante.Checked = False
        cboTipoPersona.SelectedIndex = -1
        cboTipoCliente.SelectedIndex = -1
        cboPais.SelectedIndex = -1
        cboDepartamento.SelectedIndex = -1
        cboProvincia.SelectedIndex = -1
        cboDistrito.SelectedIndex = -1
        cboTipoDocumento.SelectedIndex = -1
        optM.Checked = True
        optF.Checked = False
        txtEstadoContribuyente.Text = ""
        txtCondicionContribuyente.Text = ""
        txtTipoEmpresa.Text = ""
        txtFechaCreacion.Text = ""
        hfdEstado.Value = "1"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            strOpcionModulo = "014" 'Mantenimiento de Clientes General.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 1
            ListarTipoPersonaCombo()
            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()
            ListarEstadoCivilCombo()
            ListarTipoDocumentoCombo()
            ListarTipoClienteCombo()

            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda("ISNULL(" & cboFiltro.SelectedValue & ", '')",
                                                                     txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
            Me.grdLista.DataBind()
            cboTipoPersona_SelectedIndexChanged(cboTipoPersona, New System.EventArgs())

            If chkValidarRucWS.Checked = True Then
                txtRuc.AutoPostBack = True
                txtDni.AutoPostBack = True
            Else
                txtRuc.AutoPostBack = False
                txtDni.AutoPostBack = False
            End If
            Me.cboTipoPersona.Attributes.Add("onclick", "fnSetFocus('" & cboTipoPersona.UniqueID & "');")
            Me.cboTipoDocumento.Attributes.Add("onclick", "fnSetFocus('" & cboTipoDocumento.UniqueID & "');")
            Me.cboPais.Attributes.Add("onclick", "fnSetFocus('" & cboPais.UniqueID & "');")
            Me.cboDepartamento.Attributes.Add("onclick", "fnSetFocus('" & cboDepartamento.UniqueID & "');")
            Me.cboProvincia.Attributes.Add("onclick", "fnSetFocus('" & cboProvincia.UniqueID & "');")

        Else
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanListado_imgbtnBuscarCliente')")
            imgbtnBuscarCliente.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtIdCliente')")
            txtIdCliente.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboTipoPersona')")
            cboTipoPersona.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_chkValidarRucWS')")
            chkValidarRucWS.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtRuc')")
            txtRuc.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtRazonSocial')")
            txtRazonSocial.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtRepresentanteLegal')")
            txtRepresentanteLegal.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboTipoCliente')")
            cboTipoCliente.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboTipoDocumento')")
            cboTipoDocumento.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtDni')")
            txtDni.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtNombres')")
            txtNombres.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtApellidoPaterno')")
            txtApellidoPaterno.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtApellidoMaterno')")
            txtApellidoMaterno.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboPais')")
            cboPais.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboDepartamento')")
            cboDepartamento.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboProvincia')")
            cboProvincia.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboDistrito')")
            cboDistrito.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtDireccion')")
            txtDireccion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtTelefono')")
            txtTelefono.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtCelular')")
            txtCelular.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtFax')")
            txtFax.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtEmail')")
            txtEmail.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_chkAceptante')")
            chkAceptante.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_optM')")
            optM.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_optF')")
            optF.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtFechaNacimiento')")
            txtFechaNacimiento.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_cboEstadoCivil')")
            cboEstadoCivil.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtEstadoContribuyente')")
            txtEstadoContribuyente.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtCondicionContribuyente')")
            txtCondicionContribuyente.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtTipoEmpresa')")
            txtTipoEmpresa.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCliente_TabPanGeneral_txtFechaCreacion')")

            Me.cboTipoPersona.Attributes.Add("onclick", "fnSetFocus('" & cboTipoPersona.UniqueID & "');")
            Me.cboTipoDocumento.Attributes.Add("onclick", "fnSetFocus('" & cboTipoDocumento.UniqueID & "');")
            Me.cboPais.Attributes.Add("onclick", "fnSetFocus('" & cboPais.UniqueID & "');")
            Me.cboDepartamento.Attributes.Add("onclick", "fnSetFocus('" & cboDepartamento.UniqueID & "');")
            Me.cboProvincia.Attributes.Add("onclick", "fnSetFocus('" & cboProvincia.UniqueID & "');")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                                 txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                              txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                                 txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub cboPais_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPais.SelectedIndexChanged
        ListarDepartamentoCombo()
        ListarProvinciaCombo()
        ListarDistritoCombo()
    End Sub

    Protected Sub cboDepartamento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDepartamento.SelectedIndexChanged
        ListarProvinciaCombo()
        ListarDistritoCombo()
    End Sub

    Protected Sub cboProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProvincia.SelectedIndexChanged
        ListarDistritoCombo()
    End Sub

    Protected Sub cboTipoDocumento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTipoDocumento.SelectedIndexChanged
        Try
            If cboTipoDocumento.SelectedValue = "01" Then 'DNI
                txtDni.Focus()
                If cboTipoPersona.SelectedValue = "J" Then 'Persona Juridica
                    Throw New Exception("No puede seleccionar DNI si es persona Juridica.")
                    ValidationSummary1.ValidationGroup = "vgrpValidar"
                    MyValidator.ErrorMessage = ""
                    MyValidator.IsValid = False
                    MyValidator.ID = "ErrorPersonalizado"
                    MyValidator.ValidationGroup = "vgrpValidar"
                    Me.Page.Validators.Add(MyValidator)
                End If
            ElseIf cboTipoDocumento.SelectedValue = "04" Then 'RUC
                txtRuc.Focus()
                If cboTipoPersona.SelectedValue = "N" Then 'Persona Natural
                    cboTipoDocumento.SelectedValue = "01" 'DNI
                    txtDni.Focus()
                    Throw New Exception("No puede seleccionar RUC si es persona Natural.")
                    ValidationSummary1.ValidationGroup = "vgrpValidar"
                    MyValidator.ErrorMessage = ""
                    MyValidator.IsValid = False
                    MyValidator.ID = "ErrorPersonalizado"
                    MyValidator.ValidationGroup = "vgrpValidar"
                    Me.Page.Validators.Add(MyValidator)
                End If
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

    Protected Sub cboTipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTipoPersona.SelectedIndexChanged
        Try
            If cboTipoPersona.SelectedValue = "J" Then 'Persona Juridica
                cboTipoDocumento.SelectedValue = "04" 'RUC
                cboTipoDocumento.Enabled = False
                txtDni.Enabled = False
            Else
                cboTipoDocumento.SelectedValue = "01" 'DNI
                cboTipoDocumento.Enabled = True
                txtDni.Enabled = True
            End If
            cboTipoPersona.Focus()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub txtRuc_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtRuc.TextChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            MyValidator.ErrorMessage = ""
            lblEstado.Text = ""
            If chkValidarRucWS.Checked = False Then
                Exit Sub
            End If
            If txtRuc.Text.Trim = "" Then
                LimpiarObjetos(True)
                Exit Sub
            Else
                LimpiarObjetos(False)
            End If

            'Crea la referencia al web service de BIMSIC
            Dim booValida As Boolean = FuncionesNeg.VerificarConexionURL("http://www.grupotodohogar.com.pe:81/WSSIW/wsSistemaIntegradoWebBIMSIC.asmx")
            Dim WSCliente As New BIMSICWS.wsSistemaIntegradoWebBIMSICSoapClient
            Dim dsCliente As New DataSet
            If booValida = False Then 'No hay conexión con el WebServices
                Throw New Exception("No hay conexión con el WebServices.")
            Else
                dsCliente = WSCliente.ConsultaRUC(txtRuc.Text)
                If dsCliente.Tables(0).Rows.Count > 0 Then
                    Dim fila As DataRow
                    For Each fila In dsCliente.Tables(0).Rows
                        cboTipoPersona.SelectedValue = "J"
                        cboTipoDocumento.SelectedValue = "04" 'RUC
                        cboTipoDocumento_SelectedIndexChanged(cboTipoDocumento, New System.EventArgs())
                        txtRazonSocial.Text = fila("vRazonSocialPadronReducido")
                        cboPais.SelectedValue = "000"
                        cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())

                        cboDepartamento.SelectedValue = Mid(fila("vUbiGeoPadronReducido"), 1, 2)
                        cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())

                        cboProvincia.SelectedValue = Mid(fila("vUbiGeoPadronReducido"), 3, 2)
                        cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())

                        cboDistrito.SelectedValue = Mid(fila("vUbiGeoPadronReducido"), 5, 2)

                        txtDireccion.Text = Trim(fila("vDireccionFiscal"))
                        txtEstadoContribuyente.Text = fila("vEstadoContribuyentePadronReducido")
                        txtCondicionContribuyente.Text = fila("vCondicionDomicilioPadronReducido")
                    Next
                End If
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
            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    Protected Sub chkValidarRucWS_CheckedChanged(sender As Object, e As EventArgs) Handles chkValidarRucWS.CheckedChanged
        If chkValidarRucWS.Checked = True Then
            txtRuc.AutoPostBack = True
            txtDni.AutoPostBack = True
        Else
            txtRuc.AutoPostBack = False
            txtDni.AutoPostBack = False
        End If
    End Sub

    Private Sub txtDni_TextChanged(sender As Object, e As EventArgs) Handles txtDni.TextChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            MyValidator.ErrorMessage = ""
            lblEstado.Text = ""
            If chkValidarRucWS.Checked = False Then
                Exit Sub
            End If
            If txtDni.Text.Trim = "" Then
                LimpiarObjetos(True)
                Exit Sub
            Else
                LimpiarObjetos(False)
            End If

            'Crea la referencia al web service de BIMSIC
            Dim booValida As Boolean = FuncionesNeg.VerificarConexionURL("http://www.grupotodohogar.com.pe:81/WSSIW/wsSistemaIntegradoWebBIMSIC.asmx")
            Dim dsCliente As New DataSet
            If booValida = False Then 'No hay conexión con el WebServices
                Throw New Exception("No hay conexión con el WebServices.")
            Else
                Dim SunatNeg As New clsLibSunatNegocios
                Dim stSunatDNI As clsLibSunatMetodos.stSunatDNI = SunatNeg.SunatLoadConsultaDNI(txtDni.Text.Trim)
                cboTipoPersona.SelectedValue = "N"
                cboTipoDocumento.SelectedValue = "01"
                txtRazonSocial.Text = stSunatDNI.RazonSocial
                cboPais.SelectedValue = "000"
                cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())

                cboDepartamento.SelectedItem.Text = stSunatDNI.Departamento
                cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())

                cboProvincia.SelectedItem.Text = stSunatDNI.Provincia
                cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())

                cboDistrito.SelectedItem.Text = stSunatDNI.Distrito

                Dim strDatos() As String = stSunatDNI.RazonSocial.Split(",")
                Dim strDatos2() As String = strDatos(0).Trim.Split(" ")
                txtApellidoPaterno.Text = strDatos2(0).Trim
                txtApellidoMaterno.Text = strDatos2(1).Trim
                txtNombres.Text = strDatos(1).Trim
                txtDireccion.Text = ""
                txtEstadoContribuyente.Text = ""
                txtCondicionContribuyente.Text = ""
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

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Cli As New GNRL_CLIENTE
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0023", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Cli.cIdCliente = Valores(0).ToString()
                Cli.cIdEmpresa = Session("IdEmpresa")
                Cli.cIdPuntoVenta = Session("IdPuntoVenta")

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
                LogAuditoria.vIP3Usuario = Session("IPUsuario")

                ClienteNeg.ClienteGetData("UPDATE GNRL_CLIENTE SET bEstadoRegistroCliente = 0 WHERE cIdEmpresa = '" & Cli.cIdEmpresa & "' AND cIdCliente = '" & Cli.cIdCliente & "'")
                Session("Query") = "UPDATE GNRL_CLIENTE SET bEstadoRegistroCliente = 0 WHERE cIdEmpresa = '" & Cli.cIdEmpresa & "' AND cIdCliente = '" & Cli.cIdCliente & "'"
                LogAuditoria.vEvento = "DESACTIVAR CLIENTE"
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

                Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                                         txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Cli As New GNRL_CLIENTE
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0023", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Cli.cIdCliente = Valores(0).ToString()
                Cli.cIdEmpresa = Session("IdEmpresa")
                Cli.cIdPuntoVenta = Session("IdPuntoVenta")

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

                ClienteNeg.ClienteGetData("UPDATE GNRL_CLIENTE SET bEstadoRegistroCliente = 1 WHERE cIdEmpresa = '" & Cli.cIdEmpresa & "' AND cIdCliente = '" & Cli.cIdCliente & "'")
                Session("Query") = "UPDATE GNRL_CLIENTE SET bEstadoRegistroCliente = 1 WHERE cIdEmpresa = '" & Cli.cIdEmpresa & "' AND cIdCliente = '" & Cli.cIdCliente & "'"
                LogAuditoria.vEvento = "ACTIVAR CLIENTE"
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

                Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                                         txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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

    Private Sub imgbtnBuscarCliente_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCliente.Click
        Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                                 txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0019", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ValidarTexto(True)
            ActivarObjetos(True)

            cboTipoPersona.SelectedValue = "N"
            cboEstadoCivil.SelectedValue = "S"
            cboTipoCliente.SelectedValue = "01"

            cboPais.SelectedValue = "000"
            cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())

            cboDepartamento.SelectedValue = "15"
            cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())

            cboProvincia.SelectedValue = "01"
            cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())

            cboDistrito.SelectedValue = "01"

            cboTipoPersona_SelectedIndexChanged(cboTipoPersona, New System.EventArgs())
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0020", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0022", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0021", strOpcionModulo, Session("IdSistema"))

            If cboTipoPersona.SelectedIndex <= 0 Then
                Throw New Exception("Debe de seleccionar el tipo de persona.")
            ElseIf cboTipoDocumento.SelectedIndex < 0 Then
                Throw New Exception("Debe de seleccionar el tipo de documento.")
            ElseIf txtRazonSocial.Text = "" And txtApellidoPaterno.Text = "" And txtApellidoMaterno.Text = "" And txtNombres.Text = "" Then
                Throw New Exception("No puede registrar un proveedor con sus datos principales en blanco.")
            ElseIf hfdOperacion.Value = "N" And FuncionesNeg.IsValidarDNI_RUC(Session("IdEmpresa"), IIf(cboTipoPersona.SelectedValue = "N", IIf(txtDni.Text.Trim = "", txtRuc.Text, txtDni.Text), txtRuc.Text), cboTipoDocumento.SelectedValue) = True Then
                'Función para validar si el DNI o el RUC existen ya registrado
                Throw New Exception("Ya exite un cliente con el mismo número de DNI/RUC.")
            ElseIf cboTipoPersona.SelectedValue = "N" And txtDni.Text.Trim.Length <> 0 And txtDni.Text.Length <> 8 And cboTipoDocumento.SelectedValue = "01" Then 'DNI
                Throw New Exception("Longitud del DNI inválido.")
            ElseIf cboTipoPersona.SelectedValue = "N" And cboTipoDocumento.SelectedValue = "01" And IsNumeric(txtDni.Text.Trim) = False And txtDni.Text.Trim.Length <> 0 Then 'DNI
                Throw New Exception("Debe de ser numérico el DNI.")
            ElseIf cboTipoPersona.SelectedValue = "N" And txtDni.Text.Length > 12 And cboTipoDocumento.SelectedValue = "03" Then 'CARNET EXTRANJERIA
                Throw New Exception("Longitud del carnet de extranjería inválido.")
            ElseIf cboTipoPersona.SelectedValue = "N" And txtDni.Text.Length > 12 And cboTipoDocumento.SelectedValue = "00" Then 'OTROS
                Throw New Exception("Longitud del carnet de extranjería inválido.")
            ElseIf cboTipoPersona.SelectedValue = "N" And txtDni.Text.Length > 12 And cboTipoDocumento.SelectedValue = "00" Then 'OTROS
                Throw New Exception("Longitud del carnet de extranjería inválido.")
            ElseIf cboTipoPersona.SelectedValue = "J" And txtRuc.Text.Length <> 11 And cboTipoDocumento.SelectedValue = "04" Then 'RUC
                Throw New Exception("Longitud del RUC inválido para persona jurídica.")
            ElseIf cboTipoPersona.SelectedValue = "N" And txtRuc.Text.Trim.Length <> 11 And txtRuc.Text.Trim.Length <> 0 Then 'RUC
                Throw New Exception("Longitud del RUC inválido para persona natural.")
            ElseIf cboTipoPersona.SelectedValue = "N" And txtRuc.Text.Trim.Length <> 0 And IsNumeric(txtRuc.Text.Trim) = False And txtRuc.Text.Trim.Length <> 0 Then 'RUC
                Throw New Exception("Debe de ser numérico el RUC para persona natural.")
            ElseIf cboPais.SelectedIndex = 0 Then
                Throw New Exception("Debe de ingresar el país.")
            End If

            Dim Cliente As New GNRL_CLIENTE
            With Cliente
                .cIdCliente = txtIdCliente.Text
                .bEstadoRegistroCliente = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(IIf(hfdEstado.Value = "", "1", hfdEstado.Value))))
                .cIdPaisUbicacionGeografica = cboPais.SelectedValue
                .cIdDepartamentoUbicacionGeografica = cboDepartamento.SelectedValue
                .cIdProvinciaUbicacionGeografica = cboProvincia.SelectedValue
                .cIdDistritoUbicacionGeografica = cboDistrito.SelectedValue
                .cIdTipoPersona = cboTipoPersona.SelectedValue
                .vNombresCliente = Trim(UCase(IIf(txtNombres.Text = "", "", txtNombres.Text)))
                .vApellidoMaternoCliente = Trim(UCase(IIf(txtApellidoMaterno.Text = "", "", txtApellidoMaterno.Text)))
                .vApellidoPaternoCliente = Trim(UCase(IIf(txtApellidoPaterno.Text = "", "", txtApellidoPaterno.Text)))
                .vRazonSocialCliente = Trim(UCase(IIf(txtRazonSocial.Text = "", IIf(txtApellidoPaterno.Text = "", "", txtApellidoPaterno.Text) + " " + IIf(txtApellidoMaterno.Text = "", "", txtApellidoMaterno.Text) + ", " + IIf(txtNombres.Text = "", "", txtNombres.Text), txtRazonSocial.Text)))
                .vRepresentanteLegalCliente = Trim(UCase(IIf(txtRepresentanteLegal.Text = "", "", txtRepresentanteLegal.Text)))
                .vRucCliente = Trim(IIf(txtRuc.Text = "", "", txtRuc.Text))
                .vCelularCliente = IIf(txtCelular.Text = "", "", txtCelular.Text)
                .vDireccionCliente = Trim(UCase(IIf(txtDireccion.Text = "", "", txtDireccion.Text)))
                .vDniCliente = IIf(txtDni.Text = "", "", txtDni.Text)
                .vEmailCliente = Trim(UCase(IIf(txtEmail.Text = "", "", txtEmail.Text)))
                .vFaxCliente = IIf(txtFax.Text = "", "", txtFax.Text)
                .vTelefonoCliente = IIf(txtTelefono.Text = "", "", txtTelefono.Text)
                .bEstadoAceptanteCliente = chkAceptante.Checked
                .cIdEmpresa = Session("IdEmpresa")
                .cIdPuntoVenta = Session("IdPuntoVenta")
                .cGeneroCliente = IIf(optM.Checked = True, "M", "F").ToString
                If IsDate(txtFechaNacimiento.Text) = True Then .dFechaNacimientoCliente = txtFechaNacimiento.Text
                .cIdEstadoCivil = cboEstadoCivil.SelectedValue
                .cIdTipoDocumento = cboTipoDocumento.SelectedValue
                .cIdTipoCliente = cboTipoCliente.SelectedValue
                .vTipoEmpresaCliente = IIf(txtTipoEmpresa.Text.Trim = "", "", txtTipoEmpresa.Text)
                .vEstadoCliente = txtEstadoContribuyente.Text.Trim
                .vCondicionCliente = txtCondicionContribuyente.Text.Trim
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
                If (ClienteNeg.ClienteInserta(Cliente)) = 0 Then
                    Dim DirClienteNeg As New clsClienteNegocios
                    DirClienteNeg.ClienteGetData("INSERT INTO GNRL_CLIENTEDIR (cIdCliente, cIdClienteDir, vNombreClienteDir, vDireccionClienteDir, vTelefonoClienteDir, vCelularClienteDir, vFaxClienteDir, vEmailClienteDir, bEstadoRegistroClienteDir, cIdPaisUbicacionGeografica, " &
                                         "cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica) " &
                                         "VALUES ('" & Cliente.cIdCliente & "', '00', 'DIR-LEGAL', " &
                                         "         '" & Cliente.vDireccionCliente.Trim.ToUpper & "', '" & Cliente.vTelefonoCliente & "', '" & Cliente.vCelularCliente & "', '" & Cliente.vFaxCliente & "', " &
                                         "         '" & Cliente.vEmailCliente.Trim.ToUpper & "', 1, '" & Cliente.cIdPaisUbicacionGeografica & "', " &
                                         "         '" & Cliente.cIdDepartamentoUbicacionGeografica & "', '" & Cliente.cIdProvinciaUbicacionGeografica & "', '" & Cliente.cIdDistritoUbicacionGeografica & "')")

                    Session("Query") = "PA_GNRL_MNT_CLIENTE 'SQL_INSERT', '', '" & Cliente.vRucCliente & "', '" & Cliente.vDniCliente & "', '" & Cliente.vRazonSocialCliente & "', '" &
                                       Cliente.vApellidoPaternoCliente & "', '" & Cliente.vApellidoMaternoCliente & "', '" & Cliente.vNombresCliente & "', '" &
                                       Cliente.vDireccionCliente & "', '" & Cliente.vTelefonoCliente & "', '" & Cliente.vCelularCliente & "', '" & Cliente.vFaxCliente & "', '" &
                                       Cliente.vEmailCliente & "', '" & Cliente.cIdPaisUbicacionGeografica & "', '" & Cliente.cIdDepartamentoUbicacionGeografica & "', '" & Cliente.cIdProvinciaUbicacionGeografica & "', '" &
                                       Cliente.cIdDistritoUbicacionGeografica & "', '" & Cliente.cIdTipoPersona & "', '" & Cliente.bEstadoRegistroCliente & "', '" & Cliente.cIdPuntoVenta & "', '" &
                                       Cliente.cIdEmpresa & "', '" & Cliente.bEstadoAceptanteCliente & "', '" & Cliente.cGeneroCliente & "', '" & Cliente.dFechaNacimientoCliente & "', '" &
                                       Cliente.cIdEstadoCivil & "', '" & Cliente.cIdTipoDocumento & "', '" & Cliente.vRepresentanteLegalCliente & "', '" & Cliente.cIdTipoCliente & "', '" & Cliente.vTipoEmpresaCliente & "', '" &
                                       Cliente.vEstadoCliente & "', '" & Cliente.vCondicionCliente & "', '" & Cliente.cIdCliente & "'"

                    LogAuditoria.vEvento = "INSERTAR CLIENTE"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdCliente.Text = Cliente.cIdCliente
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                                             txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (ClienteNeg.ClienteEdita(Cliente)) = 0 Then
                    Dim DirClienteNeg As New clsClienteNegocios
                    Session("Query") = "PA_GNRL_MNT_CLIENTE 'SQL_UPDATE', '', '" & Cliente.vRucCliente & "', '" & Cliente.vDniCliente & "', '" & Cliente.vRazonSocialCliente & "', '" &
                                       Cliente.vApellidoPaternoCliente & "', '" & Cliente.vApellidoMaternoCliente & "', '" & Cliente.vNombresCliente & "', '" &
                                       Cliente.vDireccionCliente & "', '" & Cliente.vTelefonoCliente & "', '" & Cliente.vCelularCliente & "', '" & Cliente.vFaxCliente & "', '" &
                                       Cliente.vEmailCliente & "', '" & Cliente.cIdPaisUbicacionGeografica & "', '" & Cliente.cIdDepartamentoUbicacionGeografica & "', '" & Cliente.cIdProvinciaUbicacionGeografica & "', '" &
                                       Cliente.cIdDistritoUbicacionGeografica & "', '" & Cliente.cIdTipoPersona & "', '" & Cliente.bEstadoRegistroCliente & "', '" & Cliente.cIdPuntoVenta & "', '" &
                                       Cliente.cIdEmpresa & "', '" & Cliente.bEstadoAceptanteCliente & "', '" & Cliente.cGeneroCliente & "', '" & Cliente.dFechaNacimientoCliente & "', '" &
                                       Cliente.cIdEstadoCivil & "', '" & Cliente.cIdTipoDocumento & "', '" & Cliente.vRepresentanteLegalCliente & "', '" & Cliente.cIdTipoCliente & "', '" & Cliente.vTipoEmpresaCliente & "', '" &
                                       Cliente.vEstadoCliente & "', '" & Cliente.vCondicionCliente & "', '" & Cliente.cIdCliente & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR CLIENTE"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.grdLista.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltro.SelectedValue,
                                                                         txtBuscar.Text, Session("IdEmpresa"), "*", True, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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