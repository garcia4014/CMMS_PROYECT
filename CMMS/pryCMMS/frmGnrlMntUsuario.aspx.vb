Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmGnrlMntUsuario
    Inherits System.Web.UI.Page
    Dim UsuarioNeg As New clsUsuarioNegocios
    Dim ConfiguracionNeg As New clsConfiguracionNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strOperacionDetalle As String
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

    Shared Function CrearCesta() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("IdEmpresa", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Empresa", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdPaisOrigenUsuarioAcceso", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("PaisOrigenUsuarioAcceso", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdUsuario", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("IdTipoUsuario", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("TipoUsuario", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("IdProyectoPredeterminado", GetType(System.String))) '8
        dt.Columns.Add(New DataColumn("ProyectoPredeterminado", GetType(System.String))) '9
        dt.Columns.Add(New DataColumn("IdPerfil", GetType(System.String))) '10
        dt.Columns.Add(New DataColumn("Perfil", GetType(System.String))) '11
        dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '12
        Return dt
    End Function

    Shared Sub EditarCesta(ByVal IdEmpresa As String, ByVal Empresa As String, ByVal IdPaisOrigenUsuarioAcceso As String,
                           ByVal PaisOrigenUsuarioAcceso As String, ByVal IdUsuario As String, ByVal IdTipoUsuario As String,
                           ByVal TipoUsuario As String, ByVal IdProyectoPredeterminado As String, ByVal ProyectoPredeterminado As String,
                           ByVal IdPerfil As String, ByVal Perfil As String, ByVal Estado As Boolean,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = IdEmpresa
                Tabla.Rows(Fila)(1) = Empresa
                Tabla.Rows(Fila)(2) = IdPaisOrigenUsuarioAcceso
                Tabla.Rows(Fila)(3) = PaisOrigenUsuarioAcceso
                Tabla.Rows(Fila)(4) = IdUsuario
                Tabla.Rows(Fila)(5) = IdTipoUsuario
                Tabla.Rows(Fila)(6) = TipoUsuario
                Tabla.Rows(Fila)(7) = IdProyectoPredeterminado
                Tabla.Rows(Fila)(8) = ProyectoPredeterminado
                Tabla.Rows(Fila)(9) = IdPerfil
                Tabla.Rows(Fila)(10) = Perfil
                Tabla.Rows(Fila)(11) = Estado
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCesta(ByVal IdEmpresa As String, ByVal Empresa As String, ByVal IdPaisOrigenUsuarioAcceso As String,
                           ByVal PaisOrigenUsuarioAcceso As String, ByVal IdUsuario As String, ByVal IdTipoUsuario As String,
                           ByVal TipoUsuario As String, ByVal IdProyectoPredeterminado As String, ByVal ProyectoPredeterminado As String,
                           ByVal IdPerfil As String, ByVal Perfil As String, ByVal Estado As Boolean,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("IdEmpresa") = IdEmpresa
        Fila("Empresa") = Empresa
        Fila("IdPaisOrigenUsuarioAcceso") = IdPaisOrigenUsuarioAcceso
        Fila("PaisOrigenUsuarioAcceso") = PaisOrigenUsuarioAcceso
        Fila("IdUsuario") = IdUsuario
        Fila("IdTipoUsuario") = IdTipoUsuario
        Fila("TipoUsuario") = TipoUsuario
        Fila("IdProyectoPredeterminado") = IdProyectoPredeterminado
        Fila("ProyectoPredeterminado") = ProyectoPredeterminado
        Fila("IdPerfil") = IdPerfil
        Fila("Perfil") = Perfil
        Fila("Estado") = Estado

        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCesta(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCesta(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub CargarCestaUsuarioAcceso()
        Try
            'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
            VaciarCesta(Session("CestaUsuarioAcceso"))

            Dim UsuarioAccesoNeg As New clsUsuarioAccesoNegocios
            Dim dtColeccion = UsuarioAccesoNeg.UsuarioAccesoGetData("SELECT USUACC.cIdEmpresa, EMP.vDescripcionEmpresa, USUACC.cIdPaisOrigenUsuarioAcceso, UBIGEO.vDescripcionUbicacionGeografica AS vDescripcionPaisOrigenUsuarioAcceso, USUACC.cIdLocalPredeterminado, LOC.vDescripcionLocal, USUACC.cIdUsuario, USUACC.cIdTipoUsuario, TIPUSU.vDescripcionTablaSistema AS vDescripcionTipoUsuario, USUACC.cIdPerfil, PERUSU.vDescripcionPerfil, USUACC.bEstadoRegistroUsuarioAcceso " &
                                                                   "FROM GNRL_USUARIOACCESO USUACC INNER JOIN GNRL_EMPRESA EMP ON " &
                                                                   "     USUACC.cIdEmpresa = EMP.cIdEmpresa " &
                                                                   "     INNER JOIN GNRL_UBICACIONGEOGRAFICA UBIGEO ON " &
                                                                   "     USUACC.cIdPaisOrigenUsuarioAcceso = UBIGEO.cIdPaisUbicacionGeografica AND " &
                                                                   "     UBIGEO.cIdDepartamentoUbicacionGeografica = '00' AND UBIGEO.cIdProvinciaUbicacionGeografica = '00' AND UBIGEO.cIdDistritoUbicacionGeografica = '00' " &
                                                                   "     INNER JOIN GNRL_LOCAL AS LOC ON " &
                                                                   "     USUACC.cIdLocalPredeterminado = LOC.cIdLocal AND USUACC.cIdEmpresa = LOC.cIdEmpresa " &
                                                                   "     INNER JOIN GNRL_PERFIL AS PERUSU ON " &
                                                                   "     USUACC.cIdPerfil = PERUSU.cIdPerfil " &
                                                                   "     INNER JOIN (SELECT * FROM GNRL_TABLASISTEMA WHERE cIdTablaSistema = '65' AND cIdSistema = 'GNRL' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND bEstadoRegistroTablaSistema = 1) AS TIPUSU ON " &
                                                                   "     USUACC.cIdTipoUsuario = TIPUSU.vValor " &
                                                                   "WHERE USUACC.cIdUsuario = '" & txtIdUsuario.Text & "'")

            For Each row In dtColeccion.Rows
                AgregarCesta(row("cIdEmpresa"), row("vDescripcionEmpresa"), row("cIdPaisOrigenUsuarioAcceso"), row("vDescripcionPaisOrigenUsuarioAcceso"),
                         row("cIdUsuario"), row("cIdTipoUsuario"), row("vDescripcionTipoUsuario"), row("cIdLocalPredeterminado"), row("vDescripcionLocal"),
                         row("cIdPerfil"), row("vDescripcionPerfil"), row("bEstadoRegistroUsuarioAcceso"), Session("CestaUsuarioAcceso"))
            Next

            grdListaUsuarioAcceso.DataSource = Session("CestaUsuarioAcceso")
            grdListaUsuarioAcceso.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
            grdListaUsuarioAcceso.DataSource = Nothing
            grdListaUsuarioAcceso.DataBind()
        End Try
    End Sub

    Sub ListarContratoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbiUsuNeg As New clsUsuarioNegocios
        cboContrato.DataTextField = "idContrato"
        cboContrato.DataValueField = "descContrato"
        cboContrato.DataSource = UbiUsuNeg.ContratoListarCombo
        cboContrato.Items.Clear()
        cboContrato.DataBind()
    End Sub

    Sub ListarPaisCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
        cboPais.DataTextField = "vDescripcionUbicacionGeografica"
        cboPais.DataValueField = "cIdPaisUbicacionGeografica"
        cboPais.DataSource = UbiGeoNeg.PaisListarCombo
        cboPais.DataBind()
    End Sub

    Sub ListarDepartamentoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
        cboDepartamento.DataTextField = "vDescripcionUbicacionGeografica"
        cboDepartamento.DataValueField = "cIdDepartamentoUbicacionGeografica"
        cboDepartamento.DataSource = UbiGeoNeg.DepartamentoListarCombo(cboPais.SelectedValue)
        cboDepartamento.Items.Clear()
        cboDepartamento.DataBind()
    End Sub

    Sub ListarProvinciaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
        cboProvincia.DataTextField = "vDescripcionUbicacionGeografica"
        cboProvincia.DataValueField = "cIdProvinciaUbicacionGeografica"
        cboProvincia.DataSource = UbiGeoNeg.ProvinciaListarCombo(cboPais.SelectedValue, cboDepartamento.SelectedValue)
        cboProvincia.Items.Clear()
        cboProvincia.DataBind()
    End Sub

    Sub ListarDistritoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
        cboDistrito.DataTextField = "vDescripcionUbicacionGeografica"
        cboDistrito.DataValueField = "cIdDistritoUbicacionGeografica"
        cboDistrito.DataSource = UbiGeoNeg.DistritoListarCombo(cboPais.SelectedValue, cboDepartamento.SelectedValue, cboProvincia.SelectedValue)
        cboDistrito.Items.Clear()
        cboDistrito.DataBind()
    End Sub

    'Sub ListarContratoDetalleAccesoGeneralCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim UbiUsuNeg As New clsUsuarioNegocios
    '    cboContrato.DataTextField = "idContrato"
    '    cboContrato.DataValueField = "descContrato"
    '    cboContrato.DataSource = UbiUsuNeg.ContratoListarCombo
    '    cboContrato.Items.Clear()
    '    'cboContrato.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
    '    cboContrato.DataBind()
    'End Sub

    Sub ListarPaisDetalleAccesoGeneralCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
        cboPaisDetalleAccesoGeneral.DataTextField = "vDescripcionUbicacionGeografica"
        cboPaisDetalleAccesoGeneral.DataValueField = "cIdPaisUbicacionGeografica"
        cboPaisDetalleAccesoGeneral.DataSource = UbiGeoNeg.PaisListarCombo
        cboPaisDetalleAccesoGeneral.Items.Clear()
        cboPaisDetalleAccesoGeneral.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboPaisDetalleAccesoGeneral.DataBind()
    End Sub

    Sub ListarEmpresaDetalleAccesoGeneralCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim EmpNeg As New clsEmpresaNegocios
        cboEmpresaDetalleAccesoGeneral.DataTextField = "vDescripcionEmpresa"
        cboEmpresaDetalleAccesoGeneral.DataValueField = "cIdEmpresa"
        cboEmpresaDetalleAccesoGeneral.DataSource = EmpNeg.EmpresaPaisListarCombo(cboPaisDetalleAccesoGeneral.SelectedValue, "1")
        cboEmpresaDetalleAccesoGeneral.Items.Clear()
        cboEmpresaDetalleAccesoGeneral.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboEmpresaDetalleAccesoGeneral.DataBind()
    End Sub

    Sub ListarTipoUsuarioCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboTipoUsuarioDetalleAccesoGeneral.DataTextField = "vDescripcionTablaSistema"
        cboTipoUsuarioDetalleAccesoGeneral.DataValueField = "vValor"
        cboTipoUsuarioDetalleAccesoGeneral.DataSource = FiltroNeg.TablaSistemaListarCombo("65", "GNRL", Session("IdEmpresa"))
        cboTipoUsuarioDetalleAccesoGeneral.Items.Clear()
        cboTipoUsuarioDetalleAccesoGeneral.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboTipoUsuarioDetalleAccesoGeneral.DataBind()
    End Sub

    Sub ListarLocalDetalleAccesoGeneralCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim LocalNeg As New clsLocalNegocios
        cboProyectoDefectoDetalleAccesoGeneral.DataTextField = "vDescripcionLocal"
        cboProyectoDefectoDetalleAccesoGeneral.DataValueField = "cIdLocal"
        cboProyectoDefectoDetalleAccesoGeneral.DataSource = LocalNeg.LocalEmpresaListarCombo(cboEmpresaDetalleAccesoGeneral.SelectedValue, "1")
        cboProyectoDefectoDetalleAccesoGeneral.Items.Clear()
        cboProyectoDefectoDetalleAccesoGeneral.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboProyectoDefectoDetalleAccesoGeneral.DataBind()
    End Sub

    Sub ListarPerfilDetalleAccesoGeneralCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PerfilNeg As New clsPerfilNegocios
        cboPerfilDetalleAccesoGeneral.DataTextField = "vDescripcionPerfil"
        cboPerfilDetalleAccesoGeneral.DataValueField = "cIdPerfil"
        cboPerfilDetalleAccesoGeneral.DataSource = PerfilNeg.PerfilListarCombo()
        cboPerfilDetalleAccesoGeneral.Items.Clear()
        cboPerfilDetalleAccesoGeneral.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboPerfilDetalleAccesoGeneral.DataBind()
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltro.DataTextField = "vDescripcionTablaSistema"
        cboFiltro.DataValueField = "vValor"
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("23", "GNRL", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtApellidoPaterno.Enabled = bActivar
        txtApellidoMaterno.Enabled = bActivar
        txtCargo.Enabled = bActivar
        txtIdUsuario.Enabled = False
        txtLogin.Enabled = bActivar
        txtNombres.Enabled = bActivar
        txtPassword.Enabled = bActivar
        txtDNI.Enabled = bActivar
    End Sub

    Sub ActivarObjetosUsuarioAcceso(ByVal bActivar As Boolean)
        cboPaisDetalleAccesoGeneral.Enabled = IIf(strOperacionDetalle = "E", False, bActivar)
        cboEmpresaDetalleAccesoGeneral.Enabled = IIf(strOperacionDetalle = "E", False, bActivar)
        cboTipoUsuarioDetalleAccesoGeneral.Enabled = bActivar
        cboProyectoDefectoDetalleAccesoGeneral.Enabled = IIf(strOperacionDetalle = "E", False, bActivar)
        cboPerfilDetalleAccesoGeneral.Enabled = bActivar
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
                            txtLogin.Focus()
                            BloquearPagina = False
                        End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "N" Then
                pnlListado.Enabled = False
                pnlGeneral.Enabled = True
                txtLogin.Focus()
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
            Dim Usuario As GNRL_USUARIO = UsuarioNeg.UsuarioListarPorId(grdLista.SelectedRow.Cells(1).Text)
            txtIdUsuario.Text = Usuario.cIdUsuario
            txtLogin.Text = Usuario.vLoginUsuario
            txtApellidoPaterno.Text = Usuario.vApellidoPaternoUsuario
            txtApellidoMaterno.Text = Usuario.vApellidoMaternoUsuario
            txtNombres.Text = Usuario.vNombresUsuario
            txtDNI.Text = Usuario.vIdNroDocumentoIdentidadUsuario
            txtCargo.Text = Usuario.vCargoUsuario
            txtPassword.Text = Usuario.vPasswordUsuario
            txtValidarContrasena.Text = Usuario.vPasswordUsuario
            txtPassword.Attributes.Add("value", txtPassword.Text)
            txtValidarContrasena.Attributes.Add("value", txtValidarContrasena.Text)

            cboContrato.SelectedValue = Usuario.IdContratoUsuario
            cboContrato_SelectedIndexChanged(cboContrato, New System.EventArgs())

            cboPais.SelectedValue = Usuario.cIdPaisUbicacionGeografica
            cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())
            cboDepartamento.SelectedValue = Usuario.cIdDepartamentoUbicacionGeografica
            cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())
            cboProvincia.SelectedValue = Usuario.cIdProvinciaUbicacionGeografica
            cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())
            cboDistrito.SelectedValue = Usuario.cIdDistritoUbicacionGeografica
            hfdEstado.Value = IIf(Usuario.bEstadoRegistroUsuario = False, "0", "1")
            chkLectura.Checked = IIf(Mid(Usuario.cPermisosUsuario, 1, 1) = "R", True, False)
            chkEscritura.Checked = IIf(Mid(Usuario.cPermisosUsuario, 2, 1) = "W", True, False)
            chkEjecucion.Checked = IIf(Mid(Usuario.cPermisosUsuario, 3, 1) = "X", True, False)
            txtUnidadTrabajo.Text = Trim(Usuario.vIdUnidadTrabajoUsuario)

            CargarCestaUsuarioAcceso()

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
        Me.rfvLogin.EnableClientScript = bValidar
        Me.rfvApellidoPaterno.EnableClientScript = bValidar
        Me.rfvApellidoMaterno.EnableClientScript = bValidar
        Me.rfvNombres.EnableClientScript = bValidar
    End Sub

    Sub ValidarTextoDetalleAccesoGeneral(ByVal bValidar As Boolean)
        Me.rfvPaisDetalleAccesoGeneral.EnableClientScript = bValidar
        Me.rfvEmpresaDetalleAccesoGeneral.EnableClientScript = bValidar
        Me.rfvTipoUsuarioDetalleAccesoGeneral.EnableClientScript = bValidar
        Me.rfvProyectoDefectoDetalleAccesoGeneral.EnableClientScript = bValidar
        Me.rfvPerfilDetalleAccesoGeneral.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdUsuario.Text = ""
        txtLogin.Text = ""
        txtApellidoMaterno.Text = ""
        txtApellidoPaterno.Text = ""
        txtNombres.Text = ""
        txtDNI.Text = ""
        txtPassword.Text = ""
        txtCargo.Text = ""
        chkLectura.Checked = False
        chkEscritura.Checked = False
        chkEjecucion.Checked = False
        cboContrato.SelectedIndex = -1
        cboPais.SelectedIndex = -1
        cboDepartamento.SelectedIndex = -1
        cboProvincia.SelectedIndex = -1
        cboDistrito.SelectedIndex = -1
        txtUnidadTrabajo.Text = ""
        hfdEstado.Value = "1"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            strOpcionModulo = "020" 'Mantenimiento de Usuarios - Contabilidad.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 3
            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()

            ListarPaisDetalleAccesoGeneralCombo()
            ListarPerfilDetalleAccesoGeneralCombo()

            ListarContratoCombo()

            If Session("CestaUsuarioAcceso") Is Nothing Then
                Session("CestaUsuarioAcceso") = CrearCesta()
            Else
                VaciarCesta(Session("CestaUsuarioAcceso"))
            End If

            cboContrato.SelectedValue = "*"
            cboContrato_SelectedIndexChanged(cboContrato, New System.EventArgs())

            cboPais.SelectedValue = "00"
            cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())

            cboDepartamento.SelectedValue = "15"
            cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())

            cboProvincia.SelectedValue = "01"
            cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())

            cboDistrito.SelectedValue = "01"

            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
            Me.grdLista.DataBind()
        Else
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanListado_imgbtnBuscarUsuario')")
            imgbtnBuscarUsuario.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtIdUsuario')")
            txtIdUsuario.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtLogin')")
            txtLogin.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtApellidoPaterno')")
            txtApellidoPaterno.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtApellidoMaterno')")
            txtApellidoMaterno.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtNombres')")
            txtNombres.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtDNI')")
            txtDNI.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtPassword')")
            txtPassword.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtValidarContrasena')")
            txtValidarContrasena.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_cboPerfil')")
            txtCargo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_chkLectura')")
            chkLectura.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_chkEscritura')")
            chkEscritura.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_chkEjecucion')")
            chkEjecucion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContUsuario_TabPanGeneral_txtUnidadTrabajo')")
            txtUnidadTrabajo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnGuardar')")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left
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
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = True
            e.Row.Cells(6).Visible = True
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

    Protected Sub cboContrato_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboContrato.SelectedIndexChanged
        txtPassword.Attributes.Add("value", txtPassword.Text)
        txtValidarContrasena.Attributes.Add("value", txtValidarContrasena.Text)
        ''ListarContratoCombo()
    End Sub

    Protected Sub cboPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPais.SelectedIndexChanged
        txtPassword.Attributes.Add("value", txtPassword.Text)
        txtValidarContrasena.Attributes.Add("value", txtValidarContrasena.Text)
        ListarDepartamentoCombo()
        ListarProvinciaCombo()
        ListarDistritoCombo()
    End Sub

    Protected Sub cboDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDepartamento.SelectedIndexChanged
        txtPassword.Attributes.Add("value", txtPassword.Text)
        txtValidarContrasena.Attributes.Add("value", txtValidarContrasena.Text)
        ListarProvinciaCombo()
        ListarDistritoCombo()
    End Sub

    Protected Sub cboProvincia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProvincia.SelectedIndexChanged
        txtPassword.Attributes.Add("value", txtPassword.Text)
        txtValidarContrasena.Attributes.Add("value", txtValidarContrasena.Text)
        ListarDistritoCombo()
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Usuario As New GNRL_USUARIO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0078", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Usuario.cIdUsuario = Valores(0).ToString()

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

                UsuarioNeg.UsuarioGetData("UPDATE GNRL_USUARIO SET bEstadoRegistroUsuario = 0 WHERE cIdUsuario = '" & Usuario.cIdUsuario & "'")
                Session("Query") = "UPDATE GNRL_USUARIO SET bEstadoRegistroUsuario = 0 WHERE cIdUsuario = '" & Usuario.cIdUsuario & "'"
                LogAuditoria.vEvento = "DESACTIVAR USUARIO"
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

                Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Usuario As New GNRL_USUARIO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0078", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Usuario.cIdUsuario = Valores(0).ToString()

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

                UsuarioNeg.UsuarioGetData("UPDATE GNRL_USUARIO SET bEstadoRegistroUsuario = 1 WHERE cIdUsuario = '" & Usuario.cIdUsuario & "'")
                Session("Query") = "UPDATE GNRL_USUARIO SET bEstadoRegistroUsuario = 1 WHERE cIdUsuario = '" & Usuario.cIdUsuario & "'"
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

                Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
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

    Protected Sub grdListaUsuarioAcceso_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim UsuarioAcceso As New GNRL_USUARIOACCESO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0078", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                UsuarioAcceso.cIdEmpresa = Valores(1).ToString()
                UsuarioAcceso.cIdPaisOrigenUsuarioAcceso = Valores(2).ToString()
                UsuarioAcceso.cIdLocalPredeterminado = Valores(3).ToString()
                UsuarioAcceso.cIdUsuario = Valores(4).ToString()

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

                UsuarioNeg.UsuarioGetData("UPDATE GNRL_USUARIOACCESO SET bEstadoRegistroUsuarioAcceso = 0 WHERE cIdUsuario = '" & UsuarioAcceso.cIdUsuario & "' AND cIdEmpresa = '" & UsuarioAcceso.cIdEmpresa & "' AND cIdPaisOrigenUsuarioAcceso = '" & UsuarioAcceso.cIdPaisOrigenUsuarioAcceso & "' AND cIdLocalPredeterminado = '" & UsuarioAcceso.cIdLocalPredeterminado & "'")
                Session("Query") = "UPDATE GNRL_USUARIOACCESO SET bEstadoRegistroUsuarioAcceso = 0 WHERE cIdUsuario = '" & UsuarioAcceso.cIdUsuario & "' AND cIdEmpresa = '" & UsuarioAcceso.cIdEmpresa & "' AND cIdPaisOrigenUsuarioAcceso = '" & UsuarioAcceso.cIdPaisOrigenUsuarioAcceso & "' AND cIdLocalPredeterminado = '" & UsuarioAcceso.cIdLocalPredeterminado & "'"
                LogAuditoria.vEvento = "DESACTIVAR USUARIO"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidar"
                MyValidator.ErrorMessage = "Registro desactivado con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidar"
                Me.Page.Validators.Add(MyValidator)

                CargarCestaUsuarioAcceso()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim UsuarioAcceso As New GNRL_USUARIOACCESO
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0078", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                UsuarioAcceso.cIdEmpresa = Valores(1).ToString()
                UsuarioAcceso.cIdPaisOrigenUsuarioAcceso = Valores(2).ToString()
                UsuarioAcceso.cIdLocalPredeterminado = Valores(3).ToString()
                UsuarioAcceso.cIdUsuario = Valores(4).ToString()

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

                UsuarioNeg.UsuarioGetData("UPDATE GNRL_USUARIOACCESO SET bEstadoRegistroUsuarioAcceso = 1 WHERE cIdUsuario = '" & UsuarioAcceso.cIdUsuario & "' AND cIdEmpresa = '" & UsuarioAcceso.cIdEmpresa & "' AND cIdPaisOrigenUsuarioAcceso = '" & UsuarioAcceso.cIdPaisOrigenUsuarioAcceso & "' AND cIdLocalPredeterminado = '" & UsuarioAcceso.cIdLocalPredeterminado & "'")
                Session("Query") = "UPDATE GNRL_USUARIOACCESO SET bEstadoRegistroUsuarioAcceso = 1 WHERE cIdUsuario = '" & UsuarioAcceso.cIdUsuario & "' AND cIdEmpresa = '" & UsuarioAcceso.cIdEmpresa & "' AND cIdPaisOrigenUsuarioAcceso = '" & UsuarioAcceso.cIdPaisOrigenUsuarioAcceso & "' AND cIdLocalPredeterminado = '" & UsuarioAcceso.cIdLocalPredeterminado & "'"
                LogAuditoria.vEvento = "ACTIVAR USUARIO"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidar"
                MyValidator.ErrorMessage = "Registro activado con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidar"
                Me.Page.Validators.Add(MyValidator)

                CargarCestaUsuarioAcceso()
            End If
            If e.CommandName = "EditarUsuarioAcceso" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0439", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                strOperacionDetalle = "E"

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                cboPaisDetalleAccesoGeneral.SelectedValue = Valores(2).ToString()
                cboPaisDetalleAccesoGeneral_SelectedIndexChanged(cboPaisDetalleAccesoGeneral, New System.EventArgs())

                cboEmpresaDetalleAccesoGeneral.SelectedValue = Valores(1).ToString()
                cboEmpresaDetalleAccesoGeneral_SelectedIndexChanged(cboEmpresaDetalleAccesoGeneral, New System.EventArgs())

                cboProyectoDefectoDetalleAccesoGeneral.SelectedValue = Valores(3).ToString()

                cboTipoUsuarioDetalleAccesoGeneral.SelectedValue = Valores(6).ToString()
                cboPerfilDetalleAccesoGeneral.SelectedValue = Valores(5).ToString()

                ValidarTextoDetalleAccesoGeneral(True)
                ActivarObjetosUsuarioAcceso(True)

                Dim i As Integer = Valores(0)
                grdListaUsuarioAcceso.SelectedIndex = i

                ValidationSummary1.ValidationGroup = "vgrpValidar"
                MyValidator.ErrorMessage = "Registro encontrado con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidar"
                Me.Page.Validators.Add(MyValidator)
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

    Private Sub btnAdicionarUsuarioAcceso_Click(sender As Object, e As EventArgs) Handles btnAdicionarUsuarioAcceso.Click
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0439", strOpcionModulo, Session("IdSistema")) = False Then
                Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
            End If

            strOperacionDetalle = "N"
            ListarPaisDetalleAccesoGeneralCombo()
            ListarEmpresaDetalleAccesoGeneralCombo()
            ListarTipoUsuarioCombo()
            ListarLocalDetalleAccesoGeneralCombo()
            ListarPerfilDetalleAccesoGeneralCombo()
            ValidarTextoDetalleAccesoGeneral(True)
            ActivarObjetosUsuarioAcceso(True)
            lnk_mostrarPanelDetalleAccesoGeneral_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub cboPaisDetalleAccesoGeneral_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPaisDetalleAccesoGeneral.SelectedIndexChanged
        ListarEmpresaDetalleAccesoGeneralCombo()
        ListarLocalDetalleAccesoGeneralCombo()
        lnk_mostrarPanelDetalleAccesoGeneral_ModalPopupExtender.Show()
    End Sub

    Private Sub cboEmpresaDetalleAccesoGeneral_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmpresaDetalleAccesoGeneral.SelectedIndexChanged
        ListarLocalDetalleAccesoGeneralCombo()
        lnk_mostrarPanelDetalleAccesoGeneral_ModalPopupExtender.Show()
    End Sub

    Private Sub cboTipoUsuarioDetalleAccesoGeneral_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoUsuarioDetalleAccesoGeneral.SelectedIndexChanged
        lnk_mostrarPanelDetalleAccesoGeneral_ModalPopupExtender.Show()
    End Sub

    Private Sub cboProyectoDefectoDetalleAccesoGeneral_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProyectoDefectoDetalleAccesoGeneral.SelectedIndexChanged
        lnk_mostrarPanelDetalleAccesoGeneral_ModalPopupExtender.Show()
    End Sub

    Private Sub cboPerfilDetalleAccesoGeneral_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPerfilDetalleAccesoGeneral.SelectedIndexChanged
        lnk_mostrarPanelDetalleAccesoGeneral_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAceptarDetalleAccesoGeneral_Click(sender As Object, e As EventArgs) Handles btnAceptarDetalleAccesoGeneral.Click
        Try
            If strOperacionDetalle = "N" Then
                AgregarCesta(cboEmpresaDetalleAccesoGeneral.SelectedValue, cboEmpresaDetalleAccesoGeneral.SelectedItem.Text,
                             cboPaisDetalleAccesoGeneral.SelectedValue, cboPaisDetalleAccesoGeneral.SelectedItem.Text,
                             txtIdUsuario.Text, cboTipoUsuarioDetalleAccesoGeneral.SelectedValue, cboTipoUsuarioDetalleAccesoGeneral.SelectedItem.Text,
                             cboProyectoDefectoDetalleAccesoGeneral.SelectedValue, cboProyectoDefectoDetalleAccesoGeneral.SelectedItem.Text,
                             cboPerfilDetalleAccesoGeneral.SelectedValue, cboPerfilDetalleAccesoGeneral.SelectedItem.Text,
                             "1", Session("CestaUsuarioAcceso"))
            ElseIf strOperacionDetalle = "E" Then
                EditarCesta(cboEmpresaDetalleAccesoGeneral.SelectedValue, cboEmpresaDetalleAccesoGeneral.SelectedItem.Text,
                             cboPaisDetalleAccesoGeneral.SelectedValue, cboPaisDetalleAccesoGeneral.SelectedItem.Text,
                             txtIdUsuario.Text, cboTipoUsuarioDetalleAccesoGeneral.SelectedValue, cboTipoUsuarioDetalleAccesoGeneral.SelectedItem.Text,
                             cboProyectoDefectoDetalleAccesoGeneral.SelectedValue, cboProyectoDefectoDetalleAccesoGeneral.SelectedItem.Text,
                             cboPerfilDetalleAccesoGeneral.SelectedValue, cboPerfilDetalleAccesoGeneral.SelectedItem.Text,
                             "1", Session("CestaUsuarioAcceso"), grdListaUsuarioAcceso.SelectedIndex)
            End If

            ValidarTextoDetalleAccesoGeneral(False)
            ActivarObjetosUsuarioAcceso(False)

            grdListaUsuarioAcceso.DataSource = Session("CestaUsuarioAcceso")
            grdListaUsuarioAcceso.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub imgbtnBuscarUsuario_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarUsuario.Click
        Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0074", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            CargarCestaUsuarioAcceso()
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0075", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0077", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0076", strOpcionModulo, Session("IdSistema"))

            If txtPassword.Text.Trim = "" Then
                Throw New Exception("La contraseña no puede ser en blanco.")
            ElseIf Len(txtPassword.Text.Trim) <= 7 Then
                Throw New Exception("La contraseña debe ser mayor a 7 dígitos.")
            End If

            Dim Usuario As New GNRL_USUARIO
            With Usuario
                .cIdUsuario = UCase(txtIdUsuario.Text)
                .vLoginUsuario = UCase(txtLogin.Text)
                .vApellidoMaternoUsuario = UCase(txtApellidoMaterno.Text)
                .vApellidoPaternoUsuario = UCase(txtApellidoPaterno.Text)
                .vNombresUsuario = UCase(txtNombres.Text)
                .vPasswordUsuario = UCase(txtPassword.Text)
                .vCargoUsuario = IIf(txtCargo.Text = "", Nothing, UCase(txtCargo.Text))
                .cPermisosUsuario = IIf(chkLectura.Checked = True, "R", "-") & IIf(chkEscritura.Checked = True, "W", "-") & IIf(chkEjecucion.Checked = True, "X", "-")
                .vRutaFotoUsuario = "C:"
                .cIdEmpresa = Session("IdEmpresa")
                .cIdPuntoVenta = Session("IdPuntoVenta")
                .bEstadoRegistroUsuario = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
                .cIdPaisUbicacionGeografica = cboPais.SelectedValue
                .cIdDepartamentoUbicacionGeografica = cboDepartamento.SelectedValue
                .cIdProvinciaUbicacionGeografica = cboProvincia.SelectedValue
                .cIdDistritoUbicacionGeografica = cboDistrito.SelectedValue
                .vIdNroDocumentoIdentidadUsuario = txtDNI.Text.Trim
                .vIdUnidadTrabajoUsuario = txtUnidadTrabajo.Text.Trim
                .IdContratoUsuario = cboContrato.SelectedValue
            End With

            Dim IdActualUsuario As String = ""
            Dim Coleccion As New List(Of GNRL_USUARIOACCESO)
            For i = 0 To Session("CestaUsuarioAcceso").Rows.Count - 1
                Dim DetUsuAcc As New GNRL_USUARIOACCESO
                DetUsuAcc.cIdEmpresa = Session("CestaUsuarioAcceso").rows(i)("IdEmpresa").ToString.Trim
                DetUsuAcc.cIdPaisOrigenUsuarioAcceso = Session("CestaUsuarioAcceso").rows(i)("IdPaisOrigenUsuarioAcceso").ToString.Trim
                DetUsuAcc.cIdLocalPredeterminado = Session("CestaUsuarioAcceso").rows(i)("IdProyectoPredeterminado").ToString.Trim
                DetUsuAcc.cIdUsuario = Session("CestaUsuarioAcceso").rows(i)("IdUsuario").ToString.Trim
                DetUsuAcc.bEstadoRegistroUsuarioAcceso = Session("CestaUsuarioAcceso").rows(i)("Estado").ToString.Trim
                DetUsuAcc.cIdTipoUsuario = Session("CestaUsuarioAcceso").rows(i)("IdTipoUsuario").ToString.Trim
                DetUsuAcc.cIdPerfil = Session("CestaUsuarioAcceso").rows(i)("IdPerfil").ToString.Trim
                IdActualUsuario = DetUsuAcc.cIdUsuario
                Coleccion.Add(DetUsuAcc)
            Next

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacion.Value = "N" Then
                If (UsuarioNeg.UsuarioInserta(Usuario)) = 0 Then
                    If Trim(IdActualUsuario) = "" Then
                        For Each BusquedaAcceso In Coleccion
                            BusquedaAcceso.cIdUsuario = Usuario.cIdUsuario
                        Next
                    End If
                    Dim UsuarioAccesoNeg As New clsUsuarioAccesoNegocios
                    If UsuarioAccesoNeg.UsuarioAccesoInsertaDetalle(Coleccion, Session("IdSistema"), strOpcionModulo) = 0 Then
                        Session("Query") = "PA_GNRL_MNT_USUARIO 'SQL_INSERT', '', '" & Usuario.vLoginUsuario & "', '" & Usuario.vApellidoPaternoUsuario & "', '" &
                                           Usuario.vApellidoMaternoUsuario & "', '" & Usuario.vNombresUsuario & "', '" & Usuario.vPasswordUsuario & "', '" &
                                           Usuario.vCargoUsuario & "', '" & Usuario.cPermisosUsuario & "', '" &
                                           Usuario.vRutaFotoUsuario & "', '" & Usuario.cIdPuntoVenta & "', '" &
                                           Usuario.cIdEmpresa & "', '" & Usuario.bEstadoRegistroUsuario & "', '" &
                                           Usuario.cIdPaisUbicacionGeografica & "', '" & Usuario.cIdDepartamentoUbicacionGeografica & "', '" & Usuario.cIdProvinciaUbicacionGeografica & "', '" & Usuario.cIdDistritoUbicacionGeografica & "', '" &
                                           Usuario.vIdUnidadTrabajoUsuario & "', '" & Usuario.IdContratoUsuario & "', '" & Usuario.cIdUsuario & "'"
                        LogAuditoria.vEvento = "INSERTAR USUARIO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        txtIdUsuario.Text = Usuario.cIdUsuario
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"

                        Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                        Me.grdLista.DataBind()
                        pnlGeneral.Enabled = False
                        BloquearMantenimiento(True, False, True, False)
                        hfTab.Value = "tab1"
                        hfdOperacion.Value = "R"
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                    hfdOperacion.Value = "R"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (UsuarioNeg.UsuarioEdita(Usuario)) = 0 Then
                    Dim UsuarioAccesoNeg As New clsUsuarioAccesoNegocios
                    If UsuarioAccesoNeg.UsuarioAccesoInsertaDetalle(Coleccion, Session("IdSistema"), strOpcionModulo) = 0 Then
                        Session("Query") = "PA_GNRL_MNT_USUARIO 'SQL_UPDATE', '', '" & Usuario.vLoginUsuario & "', '" & Usuario.vApellidoPaternoUsuario & "', '" &
                                            Usuario.vApellidoMaternoUsuario & "', '" & Usuario.vNombresUsuario & "', '" & Usuario.vPasswordUsuario & "', '" &
                                            Usuario.vCargoUsuario & "', '" & Usuario.cPermisosUsuario & "', '" &
                                            Usuario.vRutaFotoUsuario & "', '" & Usuario.cIdPuntoVenta & "', '" &
                                            Usuario.cIdEmpresa & "', '" & Usuario.bEstadoRegistroUsuario & "', '" &
                                            Usuario.cIdPaisUbicacionGeografica & "', '" & Usuario.cIdDepartamentoUbicacionGeografica & "', '" & Usuario.cIdProvinciaUbicacionGeografica & "', '" & Usuario.cIdDistritoUbicacionGeografica & "', '" &
                                            Usuario.vIdUnidadTrabajoUsuario & "', '" & Usuario.IdContratoUsuario & "', '" & Usuario.cIdUsuario & "'"
                        LogAuditoria.vEvento = "ACTUALIZAR USUARIO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        MyValidator.ErrorMessage = "Registro actualizado con éxito"
                        Me.grdLista.DataSource = UsuarioNeg.UsuarioListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*", IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                        Me.grdLista.DataBind()
                        pnlGeneral.Enabled = False
                        BloquearMantenimiento(True, False, True, False)
                        hfTab.Value = "tab1"
                        hfdOperacion.Value = "R"
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                    hfdOperacion.Value = "R"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
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
End Class