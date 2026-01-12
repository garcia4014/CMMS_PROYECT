Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiMntCatalogo
    Inherits System.Web.UI.Page
    Dim CatalogoNeg As New clsCatalogoNegocios
    Dim CaracteristicaNeg As New clsCaracteristicaNegocios
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
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("55", "LOGI", Session("IdEmpresa")) 'JMUG: 06/02/2023
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
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdCatalogo.Enabled = False
        cboTipoActivo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        txtDescripcion.Enabled = bActivar
        txtDescripcionAbreviada.Enabled = bActivar
        txtVidaUtil.Enabled = bActivar
        txtCuentaContable.Enabled = bActivar
        txtCuentaContableLeasing.Enabled = bActivar
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
    'JMUG: 06/02/2023

    Function fLlenarGrillaDetalleCaracteristica() As DataTable
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, ISNULL(CATCAR.vValorCatalogoCaracteristica, '') AS vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '0' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & txtIdCatalogo.Text & "' AND CATCAR.cIdJerarquiaCatalogo = '0'")
            For Each Caracteristicas In dsCaracteristica.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "", "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
            Next
            Return Session("CestaCatalogoCaracteristica")
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Function

    Sub LlenarData()
        Try
            Dim Catalogo As LOGI_CATALOGO = CatalogoNeg.CatalogoListarPorId(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, "0", "1")
            txtIdCatalogo.Text = Catalogo.cIdCatalogo
            txtDescripcion.Text = Catalogo.vDescripcionCatalogo
            txtDescripcionAbreviada.Text = Catalogo.vDescripcionAbreviadaCatalogo
            cboTipoActivo.SelectedValue = Catalogo.cIdTipoActivo
            txtVidaUtil.Text = IIf(IsNothing(Catalogo.nVidaUtilCatalogo), "", Catalogo.nVidaUtilCatalogo)
            txtCuentaContable.Text = Catalogo.cIdCuentaContableCatalogo
            txtCuentaContableLeasing.Text = Catalogo.cIdCuentaContableLeasingCatalogo
            hfdEstado.Value = IIf(IsNothing(Catalogo.bEstadoRegistroCatalogo) = True OrElse Catalogo.bEstadoRegistroCatalogo = False, "0", "1")

            Me.grdDetalleCaracteristica.DataSource = fLlenarGrillaDetalleCaracteristica()
            Me.grdDetalleCaracteristica.DataBind()

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

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdCatalogo.Text = ""
        cboTipoActivo.SelectedIndex = -1
        txtDescripcion.Text = ""
        txtDescripcionAbreviada.Text = ""
        txtVidaUtil.Text = ""
        txtCuentaContable.Text = ""
        txtCuentaContableLeasing.Text = ""
        hfdEstado.Value = "1"
    End Sub

    Sub LimpiarObjetosCaracteristicas()
        Me.lblMensajeCaracteristica.Text = ""
        txtValorCaracteristica.Text = ""
        txtIdReferenciaSAPCaracteristica.Text = ""
        txtDescripcionCampoSAPCaracteristica.Text = ""
    End Sub

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
            ListarTipoActivoCombo()

            If Session("CestaCatalogo") Is Nothing Then
                Session("CestaCatalogo") = clsLogiCestaCatalogo.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogo"))
            End If

            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            If Session("CestaCatalogoCaracteristica") Is Nothing Then
                Session("CestaCatalogoCaracteristica") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            End If
            Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristica.DataBind()

            Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
            Me.grdLista.DataBind()
        Else
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanListado_txtBuscar')")
            txtIdCatalogo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_cboTipoActivo')")
            cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcioAbreviada')")
            txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtVidaUtil')")
            txtVidaUtil.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContable')")
            txtCuentaContable.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContableLeasing')")
            txtCuentaContableLeasing.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnNuevo')")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
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
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = True
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = True
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

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0381", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Catalogo.cIdCatalogo = Valores(0).ToString()
                Catalogo.cIdTipoActivo = Valores(1).ToString()

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

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
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

                Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0381", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Catalogo.cIdCatalogo = Valores(0).ToString()
                Catalogo.cIdTipoActivo = Valores(1).ToString()

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

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
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

                Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
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

    Private Sub imgbtnBuscarCatalogo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCatalogo.Click
        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
        Me.grdLista.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0377", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
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

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                Throw New Exception("Este registro se encuentra desactivado.")
            End If
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0378", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0380", strOpcionModulo, Session("IdSistema"))

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

            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0379", strOpcionModulo, Session("IdSistema"))

            If cboTipoActivo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de activo.")
            End If

            Dim Catalogo As New LOGI_CATALOGO
            Catalogo.cIdCatalogo = UCase(txtIdCatalogo.Text)
            Catalogo.cIdTipoActivo = cboTipoActivo.SelectedValue
            Catalogo.cIdJerarquiaCatalogo = "0"
            Catalogo.cIdEnlaceCatalogo = "" 'UCase(txtIdCatalogo.Text)
            Catalogo.vDescripcionCatalogo = UCase(txtDescripcion.Text)
            Catalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviada.Text)
            Catalogo.bEstadoRegistroCatalogo = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
            Catalogo.cIdSistemaFuncional = Nothing '""
            'Catalogo.nVidaUtilCatalogo = txtVidaUtil.Text
            'Catalogo.nPeriodoGarantiaCatalogo = Convert.ToInt32(IIf(txtPeriodoGarantia.Text.Trim = "", Nothing, txtPeriodoGarantia.Text))
            'Catalogo.nPeriodoMinimoMantenimientoCatalogo = Convert.ToInt32(IIf(txtPeriodoMinimo.Text.Trim = "", Nothing, txtPeriodoMinimo.Text))
            Catalogo.nVidaUtilCatalogo = Convert.ToInt32(IIf(txtVidaUtil.Text.Trim = "", "0", txtVidaUtil.Text)) 'Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
            Catalogo.nPeriodoGarantiaCatalogo = Convert.ToInt32(IIf(txtPeriodoGarantia.Text.Trim = "", "0", txtPeriodoGarantia.Text))
            Catalogo.nPeriodoMinimoMantenimientoCatalogo = Convert.ToInt32(IIf(txtPeriodoMinimo.Text.Trim = "", "0", txtPeriodoMinimo.Text))
            Catalogo.cIdCuentaContableCatalogo = txtCuentaContable.Text
            Catalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasing.Text

            Dim ColeccionCatCar As New List(Of LOGI_CATALOGOCARACTERISTICA)
            Dim i As Integer
            For i = 0 To grdDetalleCaracteristica.Rows.Count - 1
                Dim DetDocumento As New LOGI_CATALOGOCARACTERISTICA

                DetDocumento.cIdCatalogo = txtIdCatalogo.Text
                DetDocumento.cIdJerarquiaCatalogo = "0"
                DetDocumento.cIdCaracteristica = Server.HtmlDecode(Replace(grdDetalleCaracteristica.Rows(i).Cells(3).Text.ToString, "&nbsp;", ""))
                DetDocumento.nIdNumeroItemCatalogoCaracteristica = grdDetalleCaracteristica.Rows(i).Cells(2).Text
                Dim row = grdDetalleCaracteristica.Rows(i)
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalle"), TextBox)
                Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalle"), TextBox)

                DetDocumento.vValorCatalogoCaracteristica = UCase(txtValorDetalle.Text.Trim)
                DetDocumento.cIdReferenciaSAPCatalogoCaracteristica = UCase(txtIdReferenciaSAPDetalle.Text.Trim) 'Server.HtmlDecode(Replace(grdDetalleCaracteristica.Rows(i).Cells(5).Text.ToString, "&nbsp;", ""))
                DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica = UCase(txtDescripcionCampoSAPDetalle.Text.Trim) 'Server.HtmlDecode(Replace(grdDetalleCaracteristica.Rows(i).Cells(6).Text.ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("Codigo")
                ColeccionCatCar.Add(DetDocumento)
            Next

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
                If (CatalogoNeg.CatalogoInserta(Catalogo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" &
                                                   Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                                   Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                                   Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
                                                   Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" &
                                                   Catalogo.cIdCatalogo & "'"

                    LogAuditoria.vEvento = "INSERTAR CATALOGO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdCatalogo.Text = Catalogo.cIdCatalogo

                    If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
                    End If

                    MyValidator.ErrorMessage = "Transacción registrada con éxito"

                    Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (CatalogoNeg.CatalogoEdita(Catalogo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" &
                                                Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                                Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                                Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
                                                Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" &
                                                Catalogo.cIdCatalogo & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR CATALOGO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
                    End If
                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda("cEstadoVisible='1' AND " & cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
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

    Private Sub btnAdicionarCaracteristica_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristica.Click
        Try
            fValidarSesion()
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0635", strOpcionModulo, Session("IdSistema"))

            txtBuscarCaracteristica.Focus()
            Dim EmpresaNeg As New clsEmpresaNegocios
            Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(Session("IdEmpresa"))

            Me.grdListaCaracteristica.DataSource = CaracteristicaNeg.CaracteristicaListaBusqueda(cboFiltroCaracteristica.SelectedValue, txtBuscarCaracteristica.Text.Trim, "*")
            Me.grdListaCaracteristica.DataBind()
            Me.grdListaCaracteristica.SelectedIndex = -1

            LimpiarObjetosCaracteristicas()
            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
        Catch ex As Exception
            lblMensajeCaracteristica.Text = ex.Message
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub imgbtnBuscarCaracteristica_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCaracteristica.Click
        Me.grdListaCaracteristica.DataSource = CaracteristicaNeg.CaracteristicaListaBusqueda(cboFiltroCaracteristica.SelectedValue, txtBuscarCaracteristica.Text.Trim, "*")
        Me.grdListaCaracteristica.DataBind()
        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
    End Sub

    Private Sub grdListaCaracteristica_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdListaCaracteristica.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeCaracteristica.Text = ""
            If grdListaCaracteristica IsNot Nothing Then
                If grdListaCaracteristica.Rows.Count > 0 Then
                    If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
                        If IsReference(grdListaCaracteristica.SelectedRow.Cells(1).Text) = True Then
                            txtValorCaracteristica.Text = ""
                            txtIdReferenciaSAPCaracteristica.Text = ""
                            txtDescripcionCampoSAPCaracteristica.Text = ""
                            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                        End If
                    End If
                Else
                    lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                End If
            End If
        Catch ex As Exception
            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
            lblMensajeCaracteristica.Text = ex.Message
        End Try
    End Sub

    Private Sub grdListaCaracteristica_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdListaCaracteristica.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdListaCaracteristica, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Código
            e.Row.Cells(1).Visible = True 'Descripción Característica
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Código
            e.Row.Cells(1).Visible = True 'Descripcion Característica
        End If
    End Sub

    Private Sub btnAceptarCaracteristica_Click(sender As Object, e As EventArgs) Handles btnAceptarCaracteristica.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeCaracteristica.Text = ""

            If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
                If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
                    If lblMensajeCaracteristica.Text = "" Then
                        For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
                            If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (grdListaCaracteristica.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                                grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
                                grdDetalleCaracteristica.DataBind()
                                LimpiarObjetosCaracteristicas()
                                lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                                grdListaCaracteristica.SelectedIndex = -1
                                lblMensajeCaracteristica.Text = "Característica ya registrada, seleccione otro item."
                                Exit Sub
                            End If
                        Next
                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogo.Text, "", "0", grdListaCaracteristica.SelectedValue.trim, Server.HtmlDecode(grdListaCaracteristica.SelectedRow.Cells(1).Text).Trim, UCase(txtIdReferenciaSAPCaracteristica.Text.Trim), UCase(txtDescripcionCampoSAPCaracteristica.Text.Trim), UCase(txtValorCaracteristica.Text.Trim), Session("CestaCatalogoCaracteristica"))
                        Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
                        Me.grdDetalleCaracteristica.DataBind()

                        LimpiarObjetosCaracteristicas()
                        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                        grdListaCaracteristica.SelectedIndex = -1
                        lblMensajeCaracteristica.Text = "Caracteristica agregada con éxito."
                    Else
                        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                    End If
                Else
                    lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                    lblMensajeCaracteristica.Text = "Debe de seleccionar un item."
                End If
            End If
        Catch ex As Exception
            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
            lblMensajeCaracteristica.Text = ex.Message
        End Try
    End Sub

    Private Sub grdDetalleCaracteristica_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristica.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0636", strOpcionModulo, Session("IdSistema"))

            Dim fila As Integer = 0
            For Each row As GridViewRow In grdDetalleCaracteristica.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        clsLogiCestaCatalogoCaracteristica.QuitarCesta(e.RowIndex, Session("CestaCatalogoCaracteristica"))
                        fila += 1
                    End If
                End If
            Next

            Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristica.DataBind()

            BloquearMantenimiento(False, True, False, False)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdListaCaracteristica_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdListaCaracteristica.PageIndexChanging
        grdListaCaracteristica.PageIndex = e.NewPageIndex
        Me.grdListaCaracteristica.DataSource = CaracteristicaNeg.CaracteristicaListaBusqueda(cboFiltroCaracteristica.SelectedValue, txtBuscarCaracteristica.Text.Trim, "*")
        Me.grdListaCaracteristica.DataBind() 'Recargo el grid.
        grdListaCaracteristica.SelectedIndex = -1
    End Sub
End Class