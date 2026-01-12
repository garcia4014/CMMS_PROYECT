Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmGnrlMntEmpresa
    Inherits System.Web.UI.Page
    Dim EmpresaNeg As New clsEmpresaNegocios
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
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("20", "GNRL", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
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

    Sub ListarConfiguracionListaPrecioCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ConfiguracionListaPrecioNeg As New clsTablaSistemaNegocios
        cboConfiguracionListaPrecio.DataTextField = "vDescripcionTablaSistema"
        cboConfiguracionListaPrecio.DataValueField = "vValor"
        cboConfiguracionListaPrecio.DataSource = ConfiguracionListaPrecioNeg.TablaSistemaListarCombo("62", "GNRL", Session("IdEmpresa"))
        cboConfiguracionListaPrecio.Items.Clear()
        cboConfiguracionListaPrecio.DataBind()
    End Sub

    Sub ListarConfiguracionCodigoProductoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        cboConfiguracionCodigoProducto.Items.Clear()
        cboConfiguracionCodigoProducto.Items.Add(New ListItem("GENERACIÓN DE CÓDIGO PRODUCTO FORMATO SUNAT", "1"))
        cboConfiguracionCodigoProducto.Items.Add(New ListItem("GENERACIÓN DE CÓDIGO PRODUCTO INTELIGENTE", "2"))
        cboConfiguracionCodigoProducto.DataBind()
    End Sub

    Sub ListarTipoProductoAUsarCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        cboTipoProductoAUsar.Items.Clear()
        cboTipoProductoAUsar.Items.Add(New ListItem("CÓDIGO DEL SISTEMA", "0"))
        cboTipoProductoAUsar.Items.Add(New ListItem("CÓDIGO PERSONALIZADO INTERNO", "1"))
        cboTipoProductoAUsar.DataBind()
    End Sub

    Sub ListarTipoMonedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoMonedaNeg As New clsTipoMonedaNegocios
        cboTipoMonedaBase.DataTextField = "vDescripcionTipoMoneda"
        cboTipoMonedaBase.DataValueField = "cIdTipoMoneda"
        cboTipoMonedaBase.DataSource = TipoMonedaNeg.TipoMonedaListarCombo()
        cboTipoMonedaBase.Items.Clear()
        cboTipoMonedaBase.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdEmpresa.Enabled = False
        txtDescripcion.Enabled = bActivar
        txtDescripcionCorta.Enabled = bActivar
        txtRUC.Enabled = bActivar
        txtDomicilioFiscal.Enabled = bActivar
        txtNombreComercial.Enabled = bActivar
        cboPais.Enabled = bActivar
        cboDepartamento.Enabled = bActivar
        cboProvincia.Enabled = bActivar
        cboDistrito.Enabled = bActivar
        txtPaginaWeb.Enabled = bActivar
        txtTelefono.Enabled = bActivar
        txtURLLogo.Enabled = bActivar
        txtDescripcionTipoDocumento.Enabled = bActivar
        cboTipoMonedaBase.Enabled = bActivar
        cboConfiguracionListaPrecio.Enabled = bActivar
        cboConfiguracionCodigoProducto.Enabled = bActivar
        cboTipoProductoAUsar.Enabled = bActivar
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
            Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(grdLista.SelectedRow.Cells(1).Text)
            txtIdEmpresa.Text = Empresa.cIdEmpresa
            txtDescripcion.Text = Empresa.vDescripcionEmpresa
            txtDescripcionCorta.Text = Empresa.vDescripcionAbreviadaEmpresa
            txtRUC.Text = Empresa.vRucEmpresa
            txtDomicilioFiscal.Text = Empresa.vDomicilioFiscalEmpresa
            txtNombreComercial.Text = Empresa.vNombreComercialEmpresa
            cboPais.SelectedValue = Empresa.cIdPaisUbicacionGeografica
            cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())
            cboDepartamento.SelectedValue = Empresa.cIdDepartamentoUbicacionGeografica
            cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())
            cboProvincia.SelectedValue = Empresa.cIdProvinciaUbicacionGeografica
            cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())
            cboDistrito.SelectedValue = Empresa.cIdDistritoUbicacionGeografica
            txtPaginaWeb.Text = Empresa.vPaginaWebEmpresa
            txtTelefono.Text = Empresa.vTelefonoEmpresa
            txtURLLogo.Text = Empresa.vURLLogoEmpresa
            imgEmpresa.ImageUrl = "~/Imagenes/Emp/" & Empresa.cIdEmpresa & ".jpg"
            txtDescripcionTipoDocumento.Text = Empresa.vDescripcionTipoDocumentoEmpresa
            cboTipoMonedaBase.SelectedValue = Empresa.cIdTipoMonedaBaseEmpresa
            cboConfiguracionListaPrecio.SelectedValue = Empresa.nIdConfiguracionListaPrecio
            cboConfiguracionCodigoProducto.SelectedValue = IIf(IsNothing(Empresa.cIdTipoGeneracionCodProd), "1", Empresa.cIdTipoGeneracionCodProd)
            cboTipoProductoAUsar.SelectedValue = IIf(IsNothing(Empresa.cIdTipoCodigoProductoAUsar), "0", Empresa.cIdTipoCodigoProductoAUsar)
            hfdEstado.Value = IIf(Empresa.bEstadoRegistroEmpresa = False, "0", "1")
            hfdPrincipal.Value = IIf(Empresa.bPrincipalEmpresa = False, "0", "1")
            cboTipoEmpresa.SelectedValue = Empresa.cIdTipoEmpresa
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
        Me.rfvDescripcion.EnableClientScript = bValidar
        Me.rfvRUC.EnableClientScript = bValidar
        Me.rfvDomicilioFiscal.EnableClientScript = bValidar
        Me.rfvNombreComercial.EnableClientScript = bValidar
        Me.rfvTelefono.EnableClientScript = bValidar
        Me.rfvURLLogo.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdEmpresa.Text = ""
        txtDescripcion.Text = ""
        txtDescripcionCorta.Text = ""
        txtRUC.Text = ""
        txtDomicilioFiscal.Text = ""
        txtNombreComercial.Text = ""
        txtPaginaWeb.Text = ""
        txtTelefono.Text = ""
        txtURLLogo.Text = ""
        hfdEstado.Value = "1"
        hfdPrincipal.Value = "0"
        cboConfiguracionListaPrecio.SelectedIndex = -1
        cboConfiguracionCodigoProducto.SelectedIndex = -1
        cboTipoProductoAUsar.SelectedIndex = -1
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            strOpcionModulo = "037" 'Mantenimiento de Empresas Contabilidad.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 1
            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()
            ListarTipoMonedaCombo()
            ListarConfiguracionListaPrecioCombo()
            ListarConfiguracionCodigoProductoCombo()
            ListarTipoProductoAUsarCombo()

            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue,
                                                                     txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
            Me.grdLista.DataBind()
            Me.cboPais.Attributes.Add("onclick", "fnSetFocus('" & cboPais.UniqueID & "');")
            Me.cboDepartamento.Attributes.Add("onclick", "fnSetFocus('" & cboDepartamento.UniqueID & "');")
            Me.cboProvincia.Attributes.Add("onclick", "fnSetFocus('" & cboProvincia.UniqueID & "');")
        Else
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanListado_imgbtnBuscarEmpresa')")
            imgbtnBuscarEmpresa.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtIdEmpresa')")
            txtIdEmpresa.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtDescripcionCorta')")
            txtDescripcionCorta.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtRUC')")
            txtRUC.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtDomicilioFiscal')")
            txtDomicilioFiscal.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtNombreComercial')")
            txtNombreComercial.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboPais')")
            cboPais.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboDepartamento')")
            cboDepartamento.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboProvincia')")
            cboProvincia.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboDistrito')")
            cboDistrito.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtPaginaWeb')")
            txtPaginaWeb.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtTelefono')")
            txtTelefono.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtURLLogo')")
            txtURLLogo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_txtDescripcionTipoDocumento')")
            txtDescripcionTipoDocumento.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboTipoMonedaBase')")
            cboTipoMonedaBase.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboConfiguracionListaPrecio')")
            cboConfiguracionListaPrecio.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboConfiguracionCodigoProducto')")
            cboConfiguracionCodigoProducto.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContEmpresa_TabPanGeneral_cboTipoProductoAUsar')")
            cboTipoProductoAUsar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnGuardar')")
            Me.cboPais.Attributes.Add("onclick", "fnSetFocus('" & cboPais.UniqueID & "');")
            Me.cboDepartamento.Attributes.Add("onclick", "fnSetFocus('" & cboDepartamento.UniqueID & "');")
            Me.cboProvincia.Attributes.Add("onclick", "fnSetFocus('" & cboProvincia.UniqueID & "');")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue,
                                                                 txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue,
                                                              txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue,
                                                                 txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = False
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = False
        End If
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdLista.SelectedIndexChanged
        MyValidator.ErrorMessage = ""
        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text) = "True", "1", "0")
            hfdPrincipal.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text) = "True", "1", "0")
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    Protected Sub btnAceptarImagenEmpresa_Click(sender As Object, e As EventArgs) Handles btnAceptarImagenEmpresa.Click
        Try
            lblMensajeSeleccionarImagenEmpresa.Text = ""

            If Not (fupEmpresa.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos

            Dim ext As String = fupEmpresa.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"jpg", "jpeg", "bmp", "png", "gif"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de imagen inválido.")
            Dim Size As Integer = 0
            If Not (Integer.TryParse("160", Size)) Then
                Throw New Exception("El tamaño indicado para la imagen no es válido.")
            End If

            'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
            Dim FuncionesNeg As New clsFuncionesNegocios
            FuncionesNeg.GuardarArchivo(fupEmpresa.PostedFile, True, "Imagenes\Emp", Trim(UCase(txtIdEmpresa.Text)), True, "", 1200, 160)
            imgEmpresa.ImageUrl = "~/Imagenes/Emp/" & Trim(UCase(txtIdEmpresa.Text)) & ".jpg"
            txtURLLogo.Text = "Imagenes\Emp\" & Trim(UCase(txtIdEmpresa.Text)) & ".jpg"
        Catch ex As Exception
            lblMensajeSeleccionarImagenEmpresa.Text = ex.Message
            imgbtnCargarImagenEmpresa_ModalPopupExtender.Show()
        End Try
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Empresa As New GNRL_EMPRESA
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0038", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Empresa.cIdEmpresa = Valores(0).ToString()

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

                EmpresaNeg.EmpresaGetData("UPDATE GNRL_EMPRESA SET bEstadoRegistroEmpresa = 0 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE GNRL_EMPRESA SET bEstadoRegistroEmpresa = 0 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'"
                LogAuditoria.vEvento = "DESACTIVAR EMPRESA"
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

                Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Empresa As New GNRL_EMPRESA
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0038", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Empresa.cIdEmpresa = Valores(0).ToString()

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

                EmpresaNeg.EmpresaGetData("UPDATE GNRL_EMPRESA SET bEstadoRegistroEmpresa = 1 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'")
                Session("Query") = "UPDATE GNRL_EMPRESA SET bEstadoRegistroEmpresa= 1 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'"
                LogAuditoria.vEvento = "ACTIVAR EMPRESA"
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

                Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Si" Then 'Empresa Principal
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Empresa As New GNRL_EMPRESA
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0038", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Empresa.cIdEmpresa = Valores(0).ToString()

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

                EmpresaNeg.EmpresaGetData("UPDATE GNRL_EMPRESA SET bPrincipalEmpresa = 0 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'")
                Session("Query") = "UPDATE GNRL_EMPRESA SET bPrincipalEmpresa = 0 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'"
                LogAuditoria.vEvento = "DESACTIVAR EMPRESA PRINCIPAL"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                MyValidator.ErrorMessage = "Empresa principal desactivada con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                Me.Page.Validators.Add(MyValidator)

                Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "No" Then 'Empresa Principal
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Empresa As New GNRL_EMPRESA
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0038", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Empresa.cIdEmpresa = Valores(0).ToString()

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

                EmpresaNeg.EmpresaGetData("UPDATE GNRL_EMPRESA SET bPrincipalEmpresa = 0 WHERE cIdEmpresa <> '" & Empresa.cIdEmpresa & "'")
                EmpresaNeg.EmpresaGetData("UPDATE GNRL_EMPRESA SET bPrincipalEmpresa = 1 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'")
                Session("Query") = "UPDATE GNRL_EMPRESA SET bPrincipalEmpresa = 1 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'"
                LogAuditoria.vEvento = "ACTIVAR EMPRESA PRINCIPAL"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                MyValidator.ErrorMessage = "Empresa principal activada con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                Me.Page.Validators.Add(MyValidator)
                Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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

    Private Sub imgbtnBuscarEmpresa_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarEmpresa.Click
        Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue,
                                                                     txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0034", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
            BloquearMantenimiento(False, True, False, True) ', False)
            LimpiarObjetos()
            ValidarTexto(True)
            ActivarObjetos(True)

            cboPais.SelectedValue = "00"
            cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())

            cboDepartamento.SelectedValue = "15"
            cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())

            cboProvincia.SelectedValue = "01"
            cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())

            cboDistrito.SelectedValue = "01"
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0035", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "E"
            If BloquearPagina(2) = False Then
                BloquearMantenimiento(False, True, False, True) ', False)
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0037", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "R"
            BloquearPagina(0)
            BloquearMantenimiento(True, False, True, False) ', True)
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0036", strOpcionModulo, Session("IdSistema"))

            Dim Empresa As New GNRL_EMPRESA
            With Empresa
                .cIdEmpresa = txtIdEmpresa.Text
                .vDescripcionEmpresa = UCase(IIf(txtDescripcion.Text = "", "", txtDescripcion.Text))
                .vDescripcionAbreviadaEmpresa = UCase(IIf(txtDescripcionCorta.Text = "", "", txtDescripcionCorta.Text))
                .vRucEmpresa = txtRUC.Text
                .bEstadoRegistroEmpresa = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
                .vDomicilioFiscalEmpresa = UCase(IIf(txtDomicilioFiscal.Text.Trim = "", "", txtDomicilioFiscal.Text))
                .vNombreComercialEmpresa = UCase(IIf(txtNombreComercial.Text.Trim = "", "", txtNombreComercial.Text))
                .cIdPaisUbicacionGeografica = IIf(cboPais.SelectedValue = "SELECCIONE EL PAIS", Nothing, cboPais.SelectedValue)
                .cIdDepartamentoUbicacionGeografica = IIf(cboDepartamento.SelectedValue = "", Nothing, cboDepartamento.SelectedValue)
                .cIdProvinciaUbicacionGeografica = IIf(cboProvincia.SelectedValue = "", Nothing, cboProvincia.SelectedValue)
                .cIdDistritoUbicacionGeografica = IIf(cboDistrito.SelectedValue = "", Nothing, cboDistrito.SelectedValue)
                .vPaginaWebEmpresa = UCase(txtPaginaWeb.Text.Trim)
                .vTelefonoEmpresa = txtTelefono.Text.Trim
                .vURLLogoEmpresa = txtURLLogo.Text.Trim
                .vDescripcionTipoDocumentoEmpresa = txtDescripcionTipoDocumento.Text.Trim
                .cIdTipoMonedaBaseEmpresa = cboTipoMonedaBase.SelectedValue
                .nIdConfiguracionListaPrecio = cboConfiguracionListaPrecio.SelectedValue
                .cIdTipoGeneracionCodProd = cboConfiguracionCodigoProducto.SelectedValue
                .cIdTipoCodigoProductoAUsar = cboTipoProductoAUsar.SelectedValue
                .bPrincipalEmpresa = IIf(hfdOperacion.Value = "N", False, Convert.ToBoolean(Convert.ToInt32(hfdPrincipal.Value)))
                .cIdTipoEmpresa = cboTipoEmpresa.SelectedValue
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
                If (EmpresaNeg.EmpresaInserta(Empresa)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_EMPRESA 'SQL_INSERT', '', '" & Empresa.cIdEmpresa & "', '" & Empresa.vDescripcionEmpresa & "', '" &
                                        Empresa.vDescripcionAbreviadaEmpresa & "', '" & Empresa.bEstadoRegistroEmpresa & "', '" &
                                        Empresa.vRucEmpresa & "', '" & Empresa.vDomicilioFiscalEmpresa & "', '" &
                                        Empresa.vNombreComercialEmpresa & "', '" & Empresa.cIdPaisUbicacionGeografica & "', '" &
                                        Empresa.cIdDepartamentoUbicacionGeografica & "', '" & Empresa.cIdProvinciaUbicacionGeografica & "', '" &
                                        Empresa.cIdDistritoUbicacionGeografica & "', '" & Empresa.vPaginaWebEmpresa & "', '" &
                                        Empresa.vTelefonoEmpresa & "', '" & Empresa.vURLLogoEmpresa & "', " & Empresa.nIdConfiguracionListaPrecio & ", '" & Empresa.vDescripcionTipoDocumentoEmpresa & "', '" &
                                        Empresa.cIdTipoMonedaBaseEmpresa & "', '" & Empresa.cIdTipoGeneracionCodProd & "', '" & Empresa.cIdTipoCodigoProductoAUsar & "', '" & Empresa.cIdEmpresa & "'"

                    LogAuditoria.vEvento = "INSERTAR EMPRESA"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdEmpresa.Text = Empresa.cIdEmpresa
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue,
                                                                             txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False) ', True)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (EmpresaNeg.EmpresaEdita(Empresa)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_EMPRESA 'SQL_UPDATE', '', '" & Empresa.cIdEmpresa & "', '" & Empresa.vDescripcionEmpresa & "', '" &
                                        Empresa.vDescripcionAbreviadaEmpresa & "', '" & Empresa.bEstadoRegistroEmpresa & "', '" &
                                        Empresa.vRucEmpresa & "', '" & Empresa.vDomicilioFiscalEmpresa & "', '" &
                                        Empresa.vNombreComercialEmpresa & "', '" & Empresa.cIdPaisUbicacionGeografica & "', '" &
                                        Empresa.cIdDepartamentoUbicacionGeografica & "', '" & Empresa.cIdProvinciaUbicacionGeografica & "', '" &
                                        Empresa.cIdDistritoUbicacionGeografica & "', '" & Empresa.vPaginaWebEmpresa & "', '" &
                                        Empresa.vTelefonoEmpresa & "', '" & Empresa.vURLLogoEmpresa & "', '" & Empresa.vDescripcionTipoDocumentoEmpresa & "', '" &
                                        Empresa.cIdTipoMonedaBaseEmpresa & "', '" & Empresa.cIdTipoGeneracionCodProd & "', '" & Empresa.cIdTipoCodigoProductoAUsar & "', '" & Empresa.cIdEmpresa & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR EMPRESA"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.grdLista.DataSource = EmpresaNeg.EmpresaListaBusqueda(cboFiltro.SelectedValue,
                                                                             txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False) ', True)
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

    Private Sub grdLista_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles grdLista.RowUpdated

    End Sub
End Class