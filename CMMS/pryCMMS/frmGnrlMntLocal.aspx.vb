Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmGnrlMntLocal
    Inherits System.Web.UI.Page
    Dim LocalNeg As New clsLocalNegocios
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
            Throw New Exception("Su sesión ha caducado, ingrese de nuevo por favor.")
        End If
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
            End If
        End If
    End Function

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltro.DataTextField = "vDescripcionTablaSistema"
        cboFiltro.DataValueField = "vValor"
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("32", "LOGI", Session("IdEmpresa")) ', Session("IdPuntoVenta"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ListarEmpresaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim EmpresaNeg As New clsEmpresaNegocios
        cboEmpresa.DataTextField = "vDescripcionEmpresa"
        cboEmpresa.DataValueField = "cIdEmpresa"
        cboEmpresa.DataSource = EmpresaNeg.EmpresaListarCombo()
        cboEmpresa.Items.Clear()
        cboEmpresa.DataBind()
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

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdLocal.Enabled = False 'bActivar
        txtDescripcion.Enabled = bActivar
        txtDescripcionAbreviada.Enabled = bActivar
        txtDireccionLocal.Enabled = bActivar
        txtAforo.Enabled = bActivar
        txtLocalSunat.Enabled = bActivar
        cboEmpresa.Visible = IIf(Session("IdTipoUsuario") = "U", False, True)
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
            Dim Local As GNRL_LOCAL = LocalNeg.LocalListarPorId(grdLista.SelectedRow.Cells(1).Text, Session("IdEmpresa"))
            txtIdLocal.Text = Local.cIdLocal
            txtDescripcion.Text = Local.vDescripcionLocal
            txtDescripcionAbreviada.Text = Local.vDescripcionAbreviadaLocal
            txtDireccionLocal.Text = Local.vDireccionLocal
            txtAforo.Text = IIf(Local.nAforoLocal Is Nothing, "", Local.nAforoLocal)
            txtLocalSunat.Text = Local.vIdLocalAnexoSunat 'Local.vIdEquivalenciaLocal
            hfdEstado.Value = IIf(Local.bEstadoRegistroLocal = False, "0", "1")

            Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
            cboPais.SelectedValue = UbiGeoNeg.UbicacionGeograficaListarPorIdEquivalencia(Mid(Local.vIdEquivalenciaUbicacionGeografica, 1, 2)).cIdPaisUbicacionGeografica  'Cliente.cIdPaisUbicacionGeografica
            cboPais_SelectedIndexChanged(cboPais, New System.EventArgs())
            cboDepartamento.SelectedValue = UbiGeoNeg.UbicacionGeograficaListarPorIdEquivalencia(Mid(Local.vIdEquivalenciaUbicacionGeografica, 1, 4)).cIdDepartamentoUbicacionGeografica
            cboDepartamento_SelectedIndexChanged(cboDepartamento, New System.EventArgs())
            cboProvincia.SelectedValue = UbiGeoNeg.UbicacionGeograficaListarPorIdEquivalencia(Mid(Local.vIdEquivalenciaUbicacionGeografica, 1, 6)).cIdProvinciaUbicacionGeografica
            cboProvincia_SelectedIndexChanged(cboProvincia, New System.EventArgs())
            cboDistrito.SelectedValue = UbiGeoNeg.UbicacionGeograficaListarPorIdEquivalencia(Mid(Local.vIdEquivalenciaUbicacionGeografica, 1, 8)).cIdDistritoUbicacionGeografica

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
        txtIdLocal.Text = ""
        txtDescripcion.Text = ""
        txtDescripcionAbreviada.Text = ""
        txtDireccionLocal.Text = ""
        txtAforo.Text = ""
        txtLocalSunat.Text = ""
        cboPais.SelectedIndex = -1
        cboDepartamento.SelectedIndex = -1
        cboProvincia.SelectedIndex = -1
        cboDistrito.SelectedIndex = -1
        hfdEstado.Value = "1"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "023" 'Mantenimiento de los Locales.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 1
            ListarEmpresaCombo()
            cboEmpresa.SelectedValue = Session("IdEmpresa")

            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()

            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
            Me.grdLista.DataBind()
        Else
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanListado_imgbtnBuscarTipoActivo')")
            imgbtnBuscarLocal.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanGeneral_txtIdLocal')")
            txtIdLocal.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanGeneral_txtDescripcionAbreviada')")
            txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanGeneral_txtDireccionLocal')")
            txtDireccionLocal.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanGeneral_txtAforo')")
            txtAforo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContLocal_TabPanGeneral_txtLocalSunat')")
            txtLocalSunat.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnNuevo')")
        End If
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
        Me.grdLista.DataBind()
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
            If CBool(e.Row.Cells(3).Text) = False Then 'Si está Desactivado
                e.Row.ForeColor = Drawing.Color.White
                e.Row.BackColor = Drawing.Color.Tomato
            End If
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
            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text) = "True", "1", "0")
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Local As New GNRL_LOCAL
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0417", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Local.cIdLocal = Valores(0).ToString()
                Local.cIdEmpresa = Session("IdEmpresa")

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

                LocalNeg.LocalGetData("UPDATE GNRL_LOCAL SET bEstadoRegistroLocal = 0 WHERE cIdLocal = '" & Local.cIdLocal & "' AND cIdEmpresa = '" & Local.cIdEmpresa & "'")
                Session("Query") = "UPDATE GNRL_LOCAL SET bEstadoRegistroLocal = 0 WHERE cIdLocal = '" & Local.cIdLocal & "' AND cIdEmpresa = '" & Local.cIdEmpresa & "'"
                LogAuditoria.vEvento = "DESACTIVAR LOCAL"
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

                Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Local As New GNRL_LOCAL
                If FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0417", strOpcionModulo, Session("IdSistema")) = False Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                Local.cIdLocal = Valores(0).ToString()
                Local.cIdEmpresa = Session("IdEmpresa")

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

                LocalNeg.LocalGetData("UPDATE GNRL_LOCAL SET bEstadoRegistroLocal = 1 WHERE cIdLocal = '" & Local.cIdLocal & "' AND cIdEmpresa = '" & Local.cIdEmpresa & "'")
                Session("Query") = "UPDATE GNRL_LOCAL SET bEstadoRegistroLocal = 1 WHERE cIdLocal = '" & Local.cIdLocal & "' AND cIdEmpresa = '" & Local.cIdEmpresa & "'"
                LogAuditoria.vEvento = "ACTIVAR LOCAL"
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

                Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
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

    Private Sub imgbtnBuscarLocal_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarLocal.Click
        Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
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

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0413", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0414", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0416", strOpcionModulo, Session("IdSistema"))

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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0415", strOpcionModulo, Session("IdSistema"))

            Dim Local As New GNRL_LOCAL
            With Local
                .cIdLocal = UCase(txtIdLocal.Text)
                .vDescripcionLocal = UCase(txtDescripcion.Text.Trim)
                .vDescripcionAbreviadaLocal = UCase(txtDescripcionAbreviada.Text.Trim)
                .bEstadoRegistroLocal = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
                .vDireccionLocal = UCase(txtDireccionLocal.Text.Trim)
                .nAforoLocal = Convert.ToInt16(IIf(txtAforo.Text.Trim = "", Nothing, txtAforo.Text))
                .vIdLocalAnexoSunat = UCase(Trim(txtLocalSunat.Text))
                .cIdEmpresa = cboEmpresa.SelectedValue
                Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                .vIdEquivalenciaUbicacionGeografica = UbiGeoNeg.UbicacionGeograficaListarPorId(cboPais.SelectedValue, cboDepartamento.SelectedValue, cboProvincia.SelectedValue, cboDistrito.SelectedValue).vIdEquivalenciaUbicacionGeografica
            End With
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
                If (LocalNeg.LocalInserta(Local)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_LOCAL 'SQL_INSERT', '','" & Local.cIdLocal & "', '" &
                                       Local.cIdEmpresa & "', '" & Local.vDescripcionLocal & "', '" &
                                       Local.vDescripcionAbreviadaLocal & "', '" & Local.vDireccionLocal & "', '" & Local.nAforoLocal & "', '" &
                                       Local.vIdLocalAnexoSunat & "', '" & Local.bEstadoRegistroLocal & "', '" & Local.vIdEquivalenciaUbicacionGeografica & "', '" & Local.cIdLocal & "'"
                    LogAuditoria.vEvento = "INSERTAR LOCAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdLocal.Text = Local.cIdLocal
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfdOperacion.Value = "R"
                    hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (LocalNeg.LocalEdita(Local)) = 0 Then
                    Session("Query") = "PA_GNRL_MNT_LOCAL 'SQL_UPDATE', '','" & Local.cIdLocal & "', '" &
                                       Local.cIdEmpresa & "', '" & Local.vDescripcionLocal & "', '" &
                                       Local.vDescripcionAbreviadaLocal & "', '" & Local.vDireccionLocal & "', '" & Local.nAforoLocal & "', '" &
                                       Local.vIdLocalAnexoSunat & "', '" & Local.bEstadoRegistroLocal & "', '" & Local.vIdEquivalenciaUbicacionGeografica & "', '" & Local.cIdLocal & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR LOCAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    Me.grdLista.DataSource = LocalNeg.LocalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, Session("IdEmpresa"), "*")
                    Me.grdLista.DataBind()
                    pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    hfdOperacion.Value = "R"
                    hfTab.Value = "tab1"
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