Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Imports HtmlAgilityPack
Imports System.Net
Imports System.IO

Public Class frmGnrlLogin
    Inherits System.Web.UI.Page
    Dim UsuarioNeg As New clsUsuarioNegocios
    Dim TipoCambioNeg As New clsTipoCambioNegocios
    Dim TablaSistemaNeg As New clsTablaSistemaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Dim TipoMonedaNeg As New clsTipoMonedaNegocios
    Dim PerfilNeg As New clsPerfilNegocios
    Shared strOperacion As String
    Shared strOpcionModulo As String
    Shared strAmbienteProduccion As String

    Sub CargarTipoCambioWebSunat(dFecha As Date)
        Try
            Dim sUrl As String = "http://www.sunat.gob.pe/cl-at-ittipcam/tcS01Alias"
            Dim objEncoding = Encoding.GetEncoding("ISO-8859-1")
            Dim objCookies = New CookieCollection

            'Usando GET
            Dim getRequest As HttpWebRequest = CType(WebRequest.Create(sUrl), HttpWebRequest)
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
                    End If
                Next
            End If
        Catch ex As Exception
            Dim MyValidator As New CustomValidator
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
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

    Sub ListarEmpresaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim EmpresaNeg As New clsEmpresaNegocios
        cboEmpresa.DataTextField = "vDescripcionEmpresa"
        cboEmpresa.DataValueField = "cIdEmpresa"
        cboEmpresa.DataSource = EmpresaNeg.EmpresaListarCombo
        cboEmpresa.Items.Clear()
        cboEmpresa.DataBind()
    End Sub

    Sub ListarPuntoVentaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PuntoVentaNeg As New clsPuntoVentaNegocios
        cboPuntoVenta.DataTextField = "vDescripcionPuntoVenta"
        cboPuntoVenta.DataValueField = "cIdPuntoVenta"
        cboPuntoVenta.DataSource = PuntoVentaNeg.PuntoVentaListarCombo(cboEmpresa.SelectedValue, cboLocal.SelectedValue)
        cboPuntoVenta.Items.Clear()
        cboPuntoVenta.DataBind()
    End Sub

    Sub ListarLocalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim LocalNeg As New clsLocalNegocios
        cboLocal.DataTextField = "vDescripcionLocal"
        cboLocal.DataValueField = "cIdLocal"
        cboLocal.DataSource = LocalNeg.LocalEmpresaListarCombo(cboEmpresa.SelectedValue, "1")
        cboLocal.Items.Clear()
        cboLocal.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
                Server.ScriptTimeout = 10800
                Session("IdSesion") = Session.SessionID
                Session("IP1") = Request.ServerVariables("REMOTE_ADDR") 'Si no hay proxy se toma la IP original
                Session("IP2") = Request.ServerVariables("HTTP_X_FORWARDED_FOR") 'Este se chequea si hay un proxy
                Session("URL") = Request.ServerVariables("URL")

                strOperacion = "R"
                ListarEmpresaCombo()
                Dim EmpresaNeg As New clsEmpresaNegocios
                Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(cboEmpresa.SelectedValue)
                Session("IdTMonedaBase") = Empresa.cIdTipoMonedaBaseEmpresa
                hfdIdPais.Value = Empresa.cIdPaisUbicacionGeografica

                ListarLocalCombo()
                ListarPuntoVentaCombo()
                Session("TipoAmbiente") = ConfigurationManager.AppSettings("Ambiente")
                If Session("TipoAmbiente") = "0" Then
                    lblAmbiente.Text = "(DESARROLLO)"
                Else
                    lblAmbiente.Text = ""
                End If

                If IsNothing(Request.Cookies("EmpresaLogin")) = True Or IsNothing(Request.Cookies("PaisLogin")) = True Or IsNothing(Request.Cookies("LocalLogin")) = True Or IsNothing(Request.Cookies("PuntoVentaLogin")) = True Or IsNothing(Request.Cookies("AreaLogin")) = True Or IsNothing(Request.Cookies("UsuarioLogin")) = True Or IsNothing(Request.Cookies("PasswordLogin")) = True Then
                    'Creando las Cookies
                    Dim cookieEmpresaLogin = New HttpCookie("EmpresaLogin", cboEmpresa.SelectedValue)
                    cookieEmpresaLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                    Response.Cookies.Add(cookieEmpresaLogin)
                    Dim cookiePaisLogin = New HttpCookie("PaisLogin", hfdIdPais.Value)
                    cookiePaisLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                    Response.Cookies.Add(cookiePaisLogin)
                    Dim cookieLocalLogin = New HttpCookie("LocalLogin", cboLocal.SelectedValue)
                    cookieLocalLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                    Response.Cookies.Add(cookieLocalLogin)
                    Dim cookiePuntoVentaLogin = New HttpCookie("PuntoVentaLogin", cboPuntoVenta.SelectedValue)
                    cookiePuntoVentaLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                    Response.Cookies.Add(cookiePuntoVentaLogin)
                    Dim cookieUsuarioLogin = New HttpCookie("UsuarioLogin", txtUsuario.Text.Trim)
                    cookieUsuarioLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                    Response.Cookies.Add(cookieUsuarioLogin)
                    Dim cookiePasswordLogin = New HttpCookie("PasswordLogin", txtContraseña.Text.Trim)
                    cookiePasswordLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                    Response.Cookies.Add(cookiePasswordLogin)
                Else
                    cboEmpresa.SelectedValue = Request.Cookies("EmpresaLogin").Value
                    If Request.Cookies("PaisLogin").Value.Trim = "" Then
                        Dim EmpresaNew As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(cboEmpresa.SelectedValue)
                        Session("IdTMonedaBase") = EmpresaNew.cIdTipoMonedaBaseEmpresa
                        hfdIdPais.Value = EmpresaNew.cIdPaisUbicacionGeografica
                        Dim cookiePaisLogin = New HttpCookie("PaisLogin", hfdIdPais.Value)
                        cookiePaisLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                        Response.Cookies.Add(cookiePaisLogin)
                    End If

                    hfdIdPais.Value = Request.Cookies("PaisLogin").Value
                    ListarLocalCombo()
                    cboLocal.SelectedValue = Request.Cookies("LocalLogin").Value

                    ListarPuntoVentaCombo()

                    cboPuntoVenta.SelectedValue = Request.Cookies("PuntoVentaLogin").Value
                    txtUsuario.Text = Request.Cookies("UsuarioLogin").Value
                    txtContraseña.Text = Request.Cookies("PasswordLogin").Value
                    txtContraseña.Attributes.Add("value", txtContraseña.Text)
                End If

                Dim strTipoMonedaExtranjera As String = TipoMonedaNeg.TipoMonedaExtranjera
                txtTCCompra.Text = "1"
                txtTCVenta.Text = "1"
                txtUsuario.Focus()
            Else
                cboEmpresa.Attributes.Add("onkeypress", "CambiaFoco('cboLocal')")
                cboLocal.Attributes.Add("onkeypress", "CambiaFoco('cboPuntoVenta')")
                cboPuntoVenta.Attributes.Add("onkeypress", "CambiaFoco('cboArea')")
                txtUsuario.Attributes.Add("onkeypress", "CambiaFoco('txtContraseña')")
                txtContraseña.Attributes.Add("onkeypress", "CambiaFoco('txtTCCompra')")
                txtTCCompra.Attributes.Add("onkeypress", "CambiaFoco('txtTCVenta')")
                txtTCVenta.Attributes.Add("onkeypress", "CambiaFoco('txtTCVenta')")
            End If
        Catch ex As Exception
            Dim MyValidator As New CustomValidator
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            Me.Page.Validators.Add(MyValidator)

            strOperacion = "N"
            txtTCCompra.Enabled = True
            txtTCCompra.Text = ""
            txtTCVenta.Enabled = True
            txtTCVenta.Text = ""
        End Try
    End Sub

    Protected Sub txtUsuario_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtUsuario.TextChanged
        Try
            txtUsuario.Text = txtUsuario.Text.Trim
            Dim Usuario As GNRL_USUARIO = UsuarioNeg.UsuarioListarPorLogin(UCase(txtUsuario.Text.Trim), cboPuntoVenta.SelectedValue, cboEmpresa.SelectedValue)
            imgUsuario.ImageUrl = "~\Imagenes\Usr\" & IIf(RTrim(LTrim(Usuario.cIdUsuario)) = "", "00000000", RTrim(LTrim(Usuario.cIdUsuario))) & ".JPG"
            txtContraseña.Focus()
        Catch ex As Exception
            Dim MyValidator As New CustomValidator
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
        End Try
    End Sub

    Protected Sub cboEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpresa.SelectedIndexChanged
        Try
            Dim EmpresaNeg As New clsEmpresaNegocios
            Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(cboEmpresa.SelectedValue)
            Session("IdTMonedaBase") = Empresa.cIdTipoMonedaBaseEmpresa
            hfdIdPais.Value = Empresa.cIdPaisUbicacionGeografica

            ListarLocalCombo()
            ListarPuntoVentaCombo()

            Dim TipoCambio As GNRL_TIPOCAMBIO = TipoCambioNeg.TipoCambioListarPorId(Now, TipoMonedaNeg.TipoMonedaExtranjera, hfdIdPais.Value)
            strOperacion = "R"
            If TipoCambio.cIdTipoMoneda <> "" Then
                txtTCCompra.Text = TipoCambio.nTipoCambioCompra
                txtTCVenta.Text = TipoCambio.nTipoCambioVenta
            End If

            txtUsuario.Focus()
        Catch ex As Exception
            Dim MyValidator As New CustomValidator
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            Me.Page.Validators.Add(MyValidator)

            If InStrRev(ex.Message, "no Existe") > 0 Then
                strOperacion = "N"
                txtTCCompra.Enabled = True
                txtTCCompra.Text = ""
                txtTCVenta.Enabled = True
                txtTCVenta.Text = ""
            End If
        End Try
    End Sub

    Protected Sub imgbtnCargarTipoCambio_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnCargarTipoCambio.Click
        Call CargarTipoCambioWebBIMSIC(Now)
    End Sub

    Protected Sub cboPuntoVenta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPuntoVenta.SelectedIndexChanged
        txtUsuario_TextChanged(txtUsuario, New System.EventArgs())
    End Sub

    Protected Sub btnAcceder_Click(sender As Object, e As EventArgs) Handles btnAcceder.Click
        Try
            Session("IdSistema") = "CMMS"
            strOpcionModulo = "011" 'Módulo de Acceso al Sistema de Contabilidad.
            Dim Usuario As GNRL_USUARIO = UsuarioNeg.UsuarioValidar(UCase(txtUsuario.Text.Trim), UCase(txtContraseña.Text), cboEmpresa.SelectedValue, Session("IdSistema"), strOpcionModulo, cboLocal.SelectedValue)
            If IsNothing(Usuario.cIdUsuario) = True Then
                Dim MyValidator As New CustomValidator
                MyValidator.ErrorMessage = "Usuario y/o contraseña invalida." ' ex.Message
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                Me.Page.Validators.Add(MyValidator)
                Exit Sub
            End If

            Dim EmpresaNeg As New clsEmpresaNegocios
            Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(cboEmpresa.SelectedValue)
            Session("IdTMonedaBase") = Empresa.cIdTipoMonedaBaseEmpresa
            Session("IdTipoEmpresa") = Empresa.cIdTipoEmpresa
            Session("IdEmpresa") = Usuario.cIdEmpresa
            Session("IdPais") = hfdIdPais.Value 'cboPais.SelectedValue
            Session("IdLocal") = cboLocal.SelectedValue
            Session("IdPuntoVenta") = cboPuntoVenta.SelectedValue  'Usuario.cIdPuntoVenta
            Session("IdUsuario") = Usuario.cIdUsuario
            Session("IdContratoUsuario") = Usuario.IdContratoUsuario '"CN-2023-1" '"CN-2024-1"
            Session("vIdNroDocumentoIdentidadUsuario") = Usuario.vIdNroDocumentoIdentidadUsuario
            Session("IdArea") = "MANT"
            Session("IdLogin") = Usuario.vLoginUsuario

            Dim TablaSistema As GNRL_TABLASISTEMA = TablaSistemaNeg.TablaSistemaListarPorId("45", "11", "VTAS", cboEmpresa.SelectedValue) 'Tabla Impuesto
            Session("bFacturacionElectronica") = TablaSistema.vValorOpcionalTablaSistema

            Dim LogUltimoAcceso As New GNRL_LOGAUDITORIA
            LogUltimoAcceso = FuncionesNeg.LogAuditoriaListarAcceso(Session("IdUsuario"), Session("IdPais"), Session("IdEmpresa"), Session("IdLocal"), Session("IdPuntoVenta"), Session("IdSistema"), strOpcionModulo)
            Session("UltimaSesion") = LogUltimoAcceso.dFechaHora
            Session("IdPerfil") = PerfilNeg.PerfilObtenerPerfil(Session("IdUsuario")).Item(0).cIdPerfil
            If Session("IdPerfil") = "" Then
                Throw New Exception("El Perfil del usuario " & txtUsuario.Text.Trim & " no Existe!!!")
            End If

            Dim TablaSistemaImp As GNRL_TABLASISTEMA = TablaSistemaNeg.TablaSistemaListarPorId("02", "01", "VTAS", cboEmpresa.SelectedValue) 'Tabla Impuesto
            Session("nImpuesto") = Convert.ToDecimal(TablaSistemaImp.vValorOpcionalTablaSistema)

            Dim UsuAccNeg As New clsUsuarioAccesoNegocios
            Dim UsuAcc As GNRL_USUARIOACCESO = UsuAccNeg.UsuarioAccesoListarPorId(cboEmpresa.SelectedValue, hfdIdPais.Value, cboLocal.SelectedValue, Usuario.cIdUsuario) 'txtUsuario.Text)

            Session("IdTipoUsuario") = UsuAcc.cIdTipoUsuario

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")
            LogAuditoria.vIP3Usuario = Session("IPUsuario")

            Session("Query") = "PA_GNRL_MNT_USUARIO 'SQL_NONE', '', '" & Usuario.vLoginUsuario & "', '" & Usuario.vApellidoPaternoUsuario & "', '" &
                                           Usuario.vApellidoMaternoUsuario & "', '" & Usuario.vNombresUsuario & "', '" & Usuario.vPasswordUsuario & "', '" &
                                           Usuario.vCargoUsuario & "', '" & Usuario.cPermisosUsuario & "', '" &
                                           Usuario.vRutaFotoUsuario & "', '" & Usuario.cIdPuntoVenta & "', '" &
                                           Usuario.cIdEmpresa & "', '" & Usuario.bEstadoRegistroUsuario & "', '" &
                                           Usuario.cIdPaisUbicacionGeografica & "', '" & Usuario.cIdDepartamentoUbicacionGeografica & "', '" & Usuario.cIdProvinciaUbicacionGeografica & "', '" & Usuario.cIdDistritoUbicacionGeografica & "', '" &
                                           Usuario.cIdUsuario & "'"

            LogAuditoria.vEvento = "INGRESO AL SISTEMA"
            LogAuditoria.vQuery = Session("Query")
            LogAuditoria.cIdSistema = Session("IdSistema")
            LogAuditoria.cIdModulo = strOpcionModulo

            FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

            If Session("TipoAmbiente") = "0" Then
                Session("Server") = "+E15vc7ag2lYkcUUHetrNg=="
                Session("Database") = "VXlRcL/FN1s="
                Session("DBUser") = "G8hJHfPq1EM="
                Session("DBPassword") = "L/JYALZ/19YZXVZemKTKTopFKvaZYTvuw/Po91tQb9Gq/BQoVI5oFA=="
            ElseIf Session("TipoAmbiente") = "1" Then
                Session("Server") = "8OlcRYym/R6KsdPBSxHWBQ=="
                Session("Database") = "VXlRcL/FN1s="
                Session("DBUser") = "G8hJHfPq1EM="
                Session("DBPassword") = "ybGIrf61OHU="
            ElseIf Session("TipoAmbiente") = "2" Then
                Session("Server") = "8OlcRYym/R6KsdPBSxHWBQ=="
                Session("Database") = "KovliD789EUq91+vK0rA4w=="
                Session("DBUser") = "G8hJHfPq1EM="
                Session("DBPassword") = "ybGIrf61OHU="
            End If

            FuncionesNeg.FuncionesGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")

            'INICIO: ENVIO DE CORREO CUANDO LOS CERTIFICADOS SE VENCEN DESPUES DE 7 DIAS
            Dim strCantidadDiasPorVencer = TablaSistemaNeg.TablaSistemaListarPorId("87", "01", "RRHH", Session("IdEmpresa"), "*").vValorOpcionalTablaSistema
            Dim dsVigenciasCertificados = FuncionesNeg.FuncionesGetData("SELECT ASICER.*, PER.vNombreCompletoPersonal FROM RRHH_ASIGNARCERTIFICADO AS ASICER INNER JOIN RRHH_PERSONAL AS PER ON ASICER.cIdPersonal = PER.cIdPersonal WHERE DATEDIFF (dd, GETDATE(), ASICER.dFechaVigenciaFinalAsignarCertificado) <= " & strCantidadDiasPorVencer & " AND ISNULL(ASICER.cEstadoRegistroAsignarCertificado, 'R') = 'R' AND ASICER.bEstadoRegistroAsignarCertificado = '1'")
            For Each Param In dsVigenciasCertificados.Rows
                Dim strCorreoResponsable As String
                strCorreoResponsable = TablaSistemaNeg.TablaSistemaListarPorId("87", "02", "RRHH", Session("IdEmpresa"), "*").vValorOpcionalTablaSistema
                Dim strHTML As String
                Dim EmpNeg As New clsEmpresaNegocios
                strHTML = FuncionesNeg.FormatoEnvioGeneral("002", String.Format("{0:dd-MM-yyyy}", Param("dFechaVigenciaFinalAsignarCertificado")) & "*" & Param("vNumeroReferenciaAsignarCertificado") & "*" & Param("vNombreCompletoPersonal") & "*" & EmpNeg.EmpresaListarPorId(Session("IdEmpresa")).vDescripcionEmpresa)
                'Dim strFrom As String = "cmms@movitecnica.pe"
                'Dim strPwd As String = "MVTcmms@2305"
                'Dim strConfiguracionCorreo As String = "movitecnica.pe" & "|" & "465"
                Dim strFrom As String = "notificaciones@movitecnica.com.pe"
                Dim strPwd As String = "NTmvt$1604"
                Dim strConfiguracionCorreo As String = "smtp.office365.com" & "|" & "587"
                Dim strConfiguracionPeriodo As String = "" 'String.Format("{0:0000}", Year(Documento.dFechaEmisionCabeceraDocumento)) & "|" & String.Format("{0:00}", Month(Documento.dFechaEmisionCabeceraDocumento))
                FuncionesNeg.EnviarCorreo(LCase(strFrom), strPwd, LCase(strCorreoResponsable), "Orden de Trabajo Generada", strHTML, "", "", strConfiguracionCorreo, strConfiguracionPeriodo, "", False)
                FuncionesNeg.FuncionesGetData("UPDATE RRHH_ASIGNARCERTIFICADO SET cEstadoRegistroAsignarCertificado = 'T' WHERE cIdPersonal = '" & Param("cIdPersonal") & "' AND cIdTipoCertificado = '" & Param("cIdTipoCertificado") & "' AND vNumeroReferenciaAsignarCertificado = '" & Param("vNumeroReferenciaAsignarCertificado") & "'")
            Next
            'FINAL: ENVIO DE CORREO CUANDO LOS CERTIFICADOS SE VENCEN DESPUES DE 7 DIAS

            If strOperacion = "N" And txtTCCompra.Text <> "" And txtTCVenta.Text <> "" Then
                Dim TipoCambio As New GNRL_TIPOCAMBIO
                TipoCambio.dFechaHoraTipoCambio = Now
                TipoCambio.cIdTipoMoneda = TipoMonedaNeg.TipoMonedaExtranjera
                TipoCambio.bEstadoRegistroTipoCambio = True
                TipoCambio.nTipoCambioCompra = txtTCCompra.Text
                TipoCambio.nTipoCambioVenta = txtTCVenta.Text
                TipoCambio.cIdPaisOrigenTipoCambio = hfdIdPais.Value
                TipoCambio.cIdTipoMonedaBaseTipoCambio = Session("IdTMonedaBase")
                TipoCambioNeg.TipoCambioInserta(TipoCambio)

                Session("nTCambioCompra") = Convert.ToDecimal(txtTCCompra.Text)
                Session("nTCambioVenta") = Convert.ToDecimal(txtTCVenta.Text)

                'Creando las Cookies
                Dim cookieEmpresaLogin = New HttpCookie("EmpresaLogin", cboEmpresa.SelectedValue)
                cookieEmpresaLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookieEmpresaLogin)
                Dim cookieLocalLogin = New HttpCookie("LocalLogin", cboLocal.SelectedValue)
                cookieLocalLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookieLocalLogin)
                Dim cookiePuntoVentaLogin = New HttpCookie("PuntoVentaLogin", cboPuntoVenta.SelectedValue)
                cookiePuntoVentaLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookiePuntoVentaLogin)
                Dim cookieUsuarioLogin = New HttpCookie("UsuarioLogin", txtUsuario.Text.Trim)
                cookieUsuarioLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookieUsuarioLogin)
                Dim cookiePasswordLogin = New HttpCookie("PasswordLogin", txtContraseña.Text.Trim)
                cookiePasswordLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookiePasswordLogin)

                Response.Redirect("frmBienvenida.aspx")
            End If
            If Usuario.cIdPuntoVenta <> "" And txtTCCompra.Text <> "" And txtTCVenta.Text <> "" Then
                Session("nTCambioCompra") = Convert.ToDecimal(txtTCCompra.Text)
                Session("nTCambioVenta") = Convert.ToDecimal(txtTCVenta.Text)

                'Creando las Cookies
                Dim cookieEmpresaLogin = New HttpCookie("EmpresaLogin", cboEmpresa.SelectedValue)
                cookieEmpresaLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookieEmpresaLogin)
                Dim cookieLocalLogin = New HttpCookie("LocalLogin", cboLocal.SelectedValue)
                cookieLocalLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookieLocalLogin)
                Dim cookiePuntoVentaLogin = New HttpCookie("PuntoVentaLogin", cboPuntoVenta.SelectedValue)
                cookiePuntoVentaLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookiePuntoVentaLogin)
                Dim cookieUsuarioLogin = New HttpCookie("UsuarioLogin", txtUsuario.Text.Trim)
                cookieUsuarioLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookieUsuarioLogin)
                Dim cookiePasswordLogin = New HttpCookie("PasswordLogin", txtContraseña.Text.Trim)
                cookiePasswordLogin.Expires = New DateTime(Year(Now) + 10, 12, 31)
                Response.Cookies.Add(cookiePasswordLogin)

                Response.Redirect("frmBienvenida.aspx")
            End If

        Catch ex As Exception
            If InStr(ex.Message, "ya existe") > 0 Then
                strOperacion = "R"
            End If
            Dim MyValidator As New CustomValidator
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub cboLocal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLocal.SelectedIndexChanged
        Try
            ListarPuntoVentaCombo()

            Dim TipoCambio As GNRL_TIPOCAMBIO = TipoCambioNeg.TipoCambioListarPorId(Now, TipoMonedaNeg.TipoMonedaExtranjera, hfdIdPais.Value)
            strOperacion = "R"
            If TipoCambio.cIdTipoMoneda <> "" Then
                txtTCCompra.Text = TipoCambio.nTipoCambioCompra
                txtTCVenta.Text = TipoCambio.nTipoCambioVenta
            End If

            txtUsuario.Focus()
        Catch ex As Exception
            Dim MyValidator As New CustomValidator
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            Me.Page.Validators.Add(MyValidator)

            If InStrRev(ex.Message, "no Existe") > 0 Then
                strOperacion = "N"
                txtTCCompra.Enabled = True
                txtTCCompra.Text = ""
                txtTCVenta.Enabled = True
                txtTCVenta.Text = ""
            End If
        End Try
    End Sub

    Protected Sub btnMenuPrincipal_Click(sender As Object, e As EventArgs) Handles btnMenuPrincipal.Click
        Response.Redirect("..\GNRL\frmConfEmpresaVTAS.aspx")
    End Sub
End Class