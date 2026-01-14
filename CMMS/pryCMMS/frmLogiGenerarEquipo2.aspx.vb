Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarEquipo2
    Inherits System.Web.UI.Page
    Dim EquipoNeg As New clsEquipoNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator
    Shared rowIndexDetalle As Int64


    Public Sub CargarPerfil()
        'Dim AmigoNeg As New clsAmigoNegocios
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

            Dim UsuarioNeg As New clsUsuarioNegocios 'CapaNegocioGnrl.clsUsuarioNegocios
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
        'If Session("IdPuntoVenta") <> "" Or Session("IdEmpresa") <> "" Then
        If Session("IdEmpresa") <> "" Then
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
        cboFiltroEquipo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroEquipo.DataValueField = "vValor"
        'cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("14", "LOGI")
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("83", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
    End Sub

    Protected Sub Upload_File(sender As Object, e As EventArgs)
        If hfdOperacion.Value = "N" Then
            'GenerarComprobante()
        End If
        btnNuevo.Enabled = True
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "035" 'Mantenimiento de los Maestros Activos / Componente.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroEquipo.SelectedIndex = 3
            'ListarTipoActivoCombo()
            'ListarSistemaFuncionalCombo()
            'ListarTipoMonedaCombo()

            'Dim TipMonNeg As New clsTipoMonedaNegocios
            'cboTipoMonedaMaestroActivoPrincipal.SelectedValue = TipMonNeg.TipoMonedaBase(Session("IdEmpresa"))
            'cboTipoMonedaDetalleMaestroActivo.SelectedValue = TipMonNeg.TipoMonedaBase(Session("IdEmpresa"))

            'ListarMetodoDepreciacionCombo()
            'ListarEstadoActivoCombo()
            'ListarIndicadorOperacionCombo()
            'ListarEstadoActivoSunatCombo()

            'If Session("CestaCatalogoComponente") Is Nothing Then
            '    Session("CestaCatalogoComponente") = CrearCesta()
            'Else
            '    VaciarCesta(Session("CestaCatalogoComponente"))
            'End If

            'If Session("CestaMaestroActivoComponente") Is Nothing Then
            '    Session("CestaMaestroActivoComponente") = clsLogiCestaMaestroActivo.CrearCesta()
            'Else
            '    clsLogiCestaMaestroActivo.VaciarCesta(Session("CestaMaestroActivoComponente"))
            'End If

            'BloquearPagina(1)
            'BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlEquipo.Visible = False
            pnlComponentes.Visible = False


        Else
            txtBuscarEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            txtIdEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_cboFamilia')")
            cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcion')")
            txtDescripcionEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'If lblOperacion.Value = "E" Or lblOperacion.Value = "N" Then
            '    BloquearPagina(2)
            'End If
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True
        Catch ex As Exception

        End Try
    End Sub


End Class