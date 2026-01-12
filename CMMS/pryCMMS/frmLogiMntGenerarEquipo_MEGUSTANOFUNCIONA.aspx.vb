Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiMntGenerarEquipo_MEGUSTANOFUNCIONA
    Inherits System.Web.UI.Page
    Dim EquipoNeg As New clsEquipoNegocios
    Dim CatalogoNeg As New clsCatalogoNegocios
    Dim CaracteristicaNeg As New clsCaracteristicaNegocios
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

    Shared Function CrearCesta() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdTipoActivo", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdSistemaFuncional", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("Estado", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("DescripcionAbreviada", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdCuentaContable", GetType(System.String))) '7
        Return dt
    End Function

    Shared Sub EditarCesta(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdTipoActivo As String,
                           ByVal IdSistemaFuncional As String, ByVal Estado As String, ByVal DescripcionAbreviada As String,
                           ByVal IdCuentaContable As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        'ByVal FecVen As System.Nullable(Of DateTime), ByVal CentroCosto As String, _
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Descripcion
                Tabla.Rows(Fila)(2) = IdTipoActivo
                Tabla.Rows(Fila)(3) = IdSistemaFuncional
                Tabla.Rows(Fila)(4) = Estado
                Tabla.Rows(Fila)(5) = DescripcionAbreviada
                Tabla.Rows(Fila)(6) = IdCuentaContable
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCesta(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdTipoActivo As String,
                           ByVal IdSistemaFuncional As String, ByVal Estado As String, ByVal DescripcionAbreviada As String,
                           ByVal IdCuentaContable As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("Estado") = Estado
        Fila("IdTipoActivo") = IdTipoActivo
        Fila("IdSistemaFuncional") = IdSistemaFuncional
        Fila("DescripcionAbreviada") = DescripcionAbreviada
        Fila("IdCuentaContable") = IdCuentaContable
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

    'Sub CargarCestaComponenteCatalogo()
    '    'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
    '    VaciarCesta(Session("CestaCatalogoComponente"))

    '    Dim CatalogoNeg As New clsCatalogoNegocios
    '    Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
    '    Dim Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")

    '    Dim intContador As Integer = 0

    '    For Each Registro In Coleccion
    '        Dim booExiste As Boolean = False
    '        For Each Registro2 In Coleccion2
    '            If Registro.Codigo = Registro2.IdCatalogo And Registro.IdSistemaFuncional = Registro2.IdSistemaFuncional And Registro.IdTipoActivo = Registro2.IdTipoActivo Then
    '                booExiste = True
    '            End If
    '        Next
    '        If booExiste = False Then
    '            AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.IdTipoActivo, Registro.IdSistemaFuncional, Registro.Estado, Registro.DescripcionAbreviada, "", Session("CestaCatalogoComponente"))
    '        End If
    '    Next

    '    grdComponenteCatalogo.DataSource = Session("CestaCatalogoComponente")
    '    Me.grdComponenteCatalogo.DataBind()
    'End Sub

    'Sub CargarCestaCatalogoCaracteristica()
    '    'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
    '    Try
    '        clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
    '        Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
    '                                                                       "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '0' " &
    '                                                                       "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '0'")
    '        For Each Caracteristicas In dsCaracteristica.Rows
    '            clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
    '        Next
    '        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
    '        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Sub CargarCestaCatalogoComponenteCaracteristica()
    '    'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
    '    Try
    '        clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
    '        Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
    '                                                                       "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '0' " &
    '                                                                       "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '0'")
    '        For Each Caracteristicas In dsCaracteristica.Rows
    '            clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
    '        Next
    '        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
    '        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Sub CargarCestaEquipoComponente()
    '    'Carga las opciones que aun no estan asignadas en la Grilla de Opciones del Módulo.
    '    Try
    '        VaciarCesta(Session("CestaEquipoComponente"))
    '        Dim EquipoActivoNeg As New clsEquipoNegocios

    '        If grdComponenteCatalogo.SelectedIndex >= (grdComponenteCatalogo.Rows.Count - 1) Then
    '            grdComponenteCatalogo.SelectedIndex = -1
    '        End If

    '        If IsNothing(grdComponenteCatalogo.SelectedRow) = False Then
    '            If IsReference(grdComponenteCatalogo.SelectedRow.Cells(1).Text) = True Then
    '                Dim Coleccion = EquipoNeg.EquipoListaBusqueda("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
    '                Dim intContador As Integer = 0
    '                For Each Registro In Coleccion
    '                    'Opcion nueva
    '                    Dim Equipo1 As New LOGI_EQUIPO
    '                    Equipo1 = EquipoNeg.EquipoListarPorId(txtIdEquipo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)

    '                    Dim TablaCestaMaestro As DataTable
    '                    TablaCestaMaestro = Session("CestaEquipoComponente")
    '                    Dim i As Integer

    '                    For i = 0 To TablaCestaMaestro.Rows.Count - 1
    '                        If TablaCestaMaestro.Rows(i)("IdTipoActivo") = cboTipoActivo.SelectedValue And
    '                           TablaCestaMaestro.Rows(i)("IdCatalogo") = cboCatalogo.SelectedValue Then
    '                            QuitarCesta(intContador, Session("CestaCatalogoComponente"))
    '                            intContador = intContador - 1
    '                        End If
    '                    Next
    '                    intContador = intContador + 1
    '                Next
    '            End If
    '        Else
    '            Dim Coleccion = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND EQU.cIdEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
    '            Dim intContador As Integer = 0
    '            For Each Registro In Coleccion
    '                Dim Equipo As LOGI_EQUIPO
    '                Equipo = EquipoNeg.EquipoListarPorIdDetalle(txtIdEquipo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)
    '                Dim TablaCestaMaestro As DataTable
    '                TablaCestaMaestro = Session("CestaEquipoComponente")
    '                Dim i As Integer
    '                'Coordinar tema de registro de información.
    '                For i = 0 To TablaCestaMaestro.Rows.Count - 1
    '                    If TablaCestaMaestro.Rows(i)("IdTipoActivo") = cboTipoActivo.SelectedValue And
    '                       TablaCestaMaestro.Rows(i)("IdCatalogo") = cboCatalogo.SelectedValue Then
    '                        QuitarCesta(intContador, Session("CestaCatalogoComponente"))
    '                        intContador = intContador - 1
    '                    End If
    '                Next
    '                intContador = intContador + 1
    '            Next
    '        End If
    '        Me.grdComponenteEquipo.DataSource = Session("CestaEquipoComponente")
    '        Me.grdComponenteEquipo.DataBind()
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    Finally
    '        'cn.Close()
    '    End Try
    'End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltroEquipo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroEquipo.DataValueField = "vValor"
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("83", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
    End Sub

    Sub ListarTipoActivoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        'Dim TipoActivoNeg As New clsTipoActivoNegocios
        'cboTipoActivo.DataTextField = "vDescripcionTipoActivo"
        'cboTipoActivo.DataValueField = "cIdTipoActivo"
        'cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        'cboTipoActivo.DataBind()

        'cboTipoActivoDetalleCatalogo.DataTextField = "vDescripcionTipoActivo"
        'cboTipoActivoDetalleCatalogo.DataValueField = "cIdTipoActivo"
        'cboTipoActivoDetalleCatalogo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        'cboTipoActivoDetalleCatalogo.DataBind()
    End Sub

    Sub ListarSistemaFuncionalCombo()
        ''Hay que hacer referencia a la Capa de Datos
        ''porque se encuentran las entidades en dicha capa.
        'Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        'cboSistemaFuncionalDetalleCatalogo.DataTextField = "vDescripcionSistemaFuncional"
        'cboSistemaFuncionalDetalleCatalogo.DataValueField = "cIdSistemaFuncional"
        'cboSistemaFuncionalDetalleCatalogo.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo(cboTipoActivo.SelectedValue, txtIdCatalogo.Text.Trim)
        'cboSistemaFuncionalDetalleCatalogo.Items.Clear()
        'cboSistemaFuncionalDetalleCatalogo.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        'txtIdCatalogo.Enabled = False
        ''cboTipoActivo.Enabled = IIf(hfdOperacion.Value = "N", bActivar, False)
        'txtDescripcion.Enabled = bActivar
        'txtDescripcionAbreviada.Enabled = bActivar
        'txtCuentaContable.Enabled = bActivar
        'txtCuentaContableLeasing.Enabled = bActivar

        txtIdDetalleCatalogo.Enabled = False
        cboTipoActivoDetalleCatalogo.Enabled = False ' bActivar
        cboSistemaFuncionalDetalleCatalogo.Enabled = bActivar
        txtDescripcionDetalleCatalogo.Enabled = bActivar
        txtDescripcionAbreviadaDetalleCatalogo.Enabled = bActivar
        'txtDimensionesDetalleCatalogo.Enabled = bActivar
        'txtVoltajeDetalleCatalogo.Enabled = bActivar
        'txtPesoDetalleCatalogo.Enabled = bActivar
        txtVidaUtilDetalleCatalogo.Enabled = bActivar
        txtCuentaContableDetalleCatalogo.Enabled = bActivar
        txtCuentaContableLeasingDetalleCatalogo.Enabled = bActivar
    End Sub

    Sub LlenarDataDetalle()
        Try
            LimpiarObjetos()

            txtIdDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim
            txtDescripcionDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(3).Text)
            txtDescripcionAbreviadaDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(4).Text)
            cboTipoActivoDetalleCatalogo.SelectedValue = grdListaDetalle.SelectedRow.Cells(5).Text
            cboSistemaFuncionalDetalleCatalogo.SelectedValue = grdListaDetalle.SelectedRow.Cells(7).Text
            txtCuentaContableDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(9).Text)
            txtCuentaContableLeasingDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(10).Text)
            'If hfdOperacionDetalle.Value = "E" Then
            '    'txtDimensionesDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("Dimensiones").ToString.Trim
            '    'txtVoltajeDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("Voltaje").ToString.Trim
            '    'txtPesoDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("Peso").ToString.Trim
            '    txtVidaUtilDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
            'Else
            '    Dim Catalogo = CatalogoNeg.CatalogoListarPorId(txtIdDetalleCatalogo.Text, cboTipoActivoDetalleCatalogo.SelectedValue, "1", "1")
            '    'txtDimensionesDetalleCatalogo.Text = Trim(Catalogo.vDimensionesCatalogo)
            '    'txtVoltajeDetalleCatalogo.Text = Trim(Catalogo.vVoltajeCatalogo)
            '    'txtPesoDetalleCatalogo.Text = Trim(Catalogo.vPesoCatalogo)
            '    txtVidaUtilDetalleCatalogo.Text = IIf(Catalogo.nVidaUtilCatalogo Is Nothing, "0", Catalogo.nVidaUtilCatalogo)
            'End If
        Catch ex As Exception
            lblMensajeDetalleCatalogo.Text = ex.Message
        End Try
    End Sub

    'Private Function BloquearPagina(ByVal NroPagina As Integer) As Boolean
    '    BloquearPagina = True
    '    If NroPagina = 1 Then
    '        TabContCatalogo.ActiveTabIndex = 0
    '        TabContCatalogo.Tabs(0).Enabled = True
    '        TabContCatalogo.Tabs(1).Enabled = False
    '        pnlListado.Enabled = True
    '        pnlGeneral.Enabled = False
    '        txtBuscar.Focus()
    '    ElseIf NroPagina = 2 Then
    '        If hfdOperacion.Value = "R" Or hfdOperacion.Value = "E" Or IsNothing(hfdOperacion.Value) = True Then
    '            If grdLista.Rows.Count = 0 Then
    '                TabContCatalogo.Tabs(1).Enabled = False
    '            Else
    '                If IsNothing(grdLista.SelectedRow) = True Then
    '                    TabContCatalogo.Tabs(1).Enabled = False
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
    '                            TabContCatalogo.Tabs(0).Enabled = True
    '                            TabContCatalogo.Tabs(1).Enabled = True
    '                            pnlListado.Enabled = True
    '                            LlenarData()
    '                        Else
    '                            TabContCatalogo.ActiveTabIndex = 1
    '                            TabContCatalogo.Tabs(0).Enabled = False
    '                            TabContCatalogo.Tabs(1).Enabled = True
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
    '            TabContCatalogo.ActiveTabIndex = 1 'LO PUSE AHORITAAA
    '            TabContCatalogo.Tabs(0).Enabled = False
    '            TabContCatalogo.Tabs(1).Enabled = True
    '            pnlListado.Enabled = False
    '            pnlGeneral.Enabled = True
    '            BloquearPagina = False
    '        End If
    '    ElseIf NroPagina = 0 Then
    '        TabContCatalogo.ActiveTabIndex = 0
    '        TabContCatalogo.Tabs(0).Enabled = True
    '        If grdLista.Rows.Count = 0 Then
    '            TabContCatalogo.Tabs(1).Enabled = False
    '        Else
    '            If InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 1 Then
    '                TabContCatalogo.ActiveTabIndex = 1
    '                TabContCatalogo.Tabs(0).Enabled = True
    '                TabContCatalogo.Tabs(1).Enabled = True 'False
    '                pnlListado.Enabled = True
    '                pnlGeneral.Enabled = False
    '                txtBuscar.Focus()
    '                strTabContenedorActivo = 1
    '            ElseIf InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 0 Then
    '                TabContCatalogo.ActiveTabIndex = 0
    '                TabContCatalogo.Tabs(0).Enabled = True
    '                TabContCatalogo.Tabs(1).Enabled = IIf(grdLista.SelectedIndex >= 0, True, False)
    '                pnlListado.Enabled = True
    '                pnlGeneral.Enabled = False
    '                txtBuscar.Focus()
    '                strTabContenedorActivo = 0
    '            Else
    '                If InStr(MyValidator.ErrorMessage, "eliminar") > 0 And strTabContenedorActivo = 0 Then
    '                    TabContCatalogo.Tabs(0).Enabled = True
    '                    TabContCatalogo.Tabs(1).Enabled = False
    '                    pnlListado.Enabled = True
    '                    pnlGeneral.Enabled = False
    '                Else
    '                    TabContCatalogo.Tabs(1).Enabled = True
    '                End If
    '            End If
    '        End If
    '        pnlListado.Enabled = True
    '        pnlGeneral.Enabled = False
    '    End If
    'End Function
    'JMUG: 10/02/2022
    'Private Function BloquearPagina(ByVal NroPagina As Integer) As Boolean
    '    BloquearPagina = True
    '    If NroPagina = 1 Then
    '        pnlListado.Enabled = True
    '        pnlGeneral.Enabled = False
    '        txtBuscar.Focus()
    '    ElseIf NroPagina = 2 Then
    '        If hfdOperacion.Value = "R" Or hfdOperacion.Value = "E" Or IsNothing(hfdOperacion.Value) = True Then
    '            If grdLista.Rows.Count = 0 Then
    '            Else
    '                If IsNothing(grdLista.SelectedRow) = True Then
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

    Sub CargarCestaCatalogo()
        Try
            'Lo quite porque no me genera el mensaje correcto.
            clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogo"))
            'If hfdOperacion.Value <> "N" Then
            'Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("cIdEnlaceCatalogo", txtIdCatalogo.Text, "1", "1")
            '    If Coleccion.Count > 0 Then
            '        'Dim dtNewColeccion = CatalogoNeg.CatalogoGetData("SELECT CAT.cIdCatalogo, CAT.cIdTipoActivo, CAT.cIdJerarquiaCatalogo, CAT.cIdSistemaFuncionalCatalogo, CAT.cIdEnlaceCatalogo, CAT.vDescripcionCatalogo, " &
            '        '                                                 "       CAT.vDescripcionAbreviadaCatalogo, CAT.bEstadoRegistroCatalogo, CAT.vDimensionesCatalogo, CAT.vVoltajeCatalogo, CAT.vPesoCatalogo, " &
            '        '                                                 "       CAT.bActivacionAutomaticaCatalogo, CAT.cIdCuentaContableCatalogo, CAT.cIdCuentaContableLeasingCatalogo, CAT.nVidaUtilCatalogo, " &
            '        '                                                 "       TIPACT.vDescripcionAbreviadaTipoActivo, SISFUN.vDescripcionAbreviadaSistemaFuncional " &
            '        '                                                 "FROM LOGI_CATALOGO AS CAT LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
            '        '                                                 "     CAT.cIdTipoActivo = TIPACT.cIdTipoActivo  " &
            '        '                                                 "     LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
            '        '                                                 "     CAT.cIdSistemaFuncional = SISFUN.cIdSistemaFuncional AND " &
            '        '                                                 "     CAT.cIdEnlaceCatalogo = SISFUN.cIdCatalogo AND " &
            '        '                                                 "     CAT.cIdTipoActivo = SISFUN.cIdTipoActivo " &
            '        '                                                 "WHERE CAT.cIdEnlaceCatalogo = '" & txtIdCatalogo.Text & "' " &
            '        '                                                 "ORDER BY cIdCatalogo")
            '        Dim dtNewColeccion = CatalogoNeg.CatalogoGetData("SELECT CAT.cIdCatalogo, CAT.cIdTipoActivo, CAT.cIdJerarquiaCatalogo, CAT.cIdSistemaFuncionalCatalogo, CAT.cIdEnlaceCatalogo, CAT.vDescripcionCatalogo, " &
            '                                                         "       CAT.vDescripcionAbreviadaCatalogo, CAT.bEstadoRegistroCatalogo, CAT.cIdCuentaContableCatalogo, CAT.cIdCuentaContableLeasingCatalogo, CAT.nVidaUtilCatalogo, " &
            '                                                         "       TIPACT.vDescripcionAbreviadaTipoActivo, SISFUN.vDescripcionAbreviadaSistemaFuncional " &
            '                                                         "FROM LOGI_CATALOGO AS CAT LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
            '                                                         "     CAT.cIdTipoActivo = TIPACT.cIdTipoActivo  " &
            '                                                         "     LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
            '                                                         "     CAT.cIdSistemaFuncionalCatalogo = SISFUN.cIdSistemaFuncional AND " &
            '                                                         "     CAT.cIdEnlaceCatalogo = SISFUN.cIdCatalogo AND " &
            '                                                         "     CAT.cIdTipoActivo = SISFUN.cIdTipoActivoSistemaFuncional " &
            '                                                         "WHERE CAT.cIdEnlaceCatalogo = '" & txtIdCatalogo.Text & "' " &
            '                                                         "ORDER BY cIdCatalogo")
            '        For Each row In dtNewColeccion.Rows
            '            'clsLogiCestaCatalogo.AgregarCesta(row("cIdCatalogo"), row("cIdTipoActivo").ToString, row("cIdJerarquiaCatalogo").ToString, row("cIdSistemaFuncional").ToString,
            '            '                           row("cIdEnlaceCatalogo").ToString, row("vDescripcionCatalogo").ToString.Trim, row("vDescripcionAbreviadaCatalogo").ToString.Trim,
            '            '                           "1", row("vDimensionesCatalogo").ToString, row("vVoltajeCatalogo").ToString, row("vPesoCatalogo").ToString, IIf(row("nVidaUtilCatalogo").ToString.Trim = "", "0", row("nVidaUtilCatalogo").ToString),
            '            '                           row("cIdCuentaContableCatalogo").ToString, row("cIdCuentaContableLeasingCatalogo").ToString, row("vDescripcionAbreviadaTipoActivo").ToString, row("vDescripcionAbreviadaSistemaFuncional").ToString,
            '            '                           Session("CestaCatalogo"))
            '            clsLogiCestaCatalogo.AgregarCesta(row("cIdCatalogo"), row("cIdTipoActivo").ToString, row("cIdJerarquiaCatalogo").ToString, row("cIdSistemaFuncionalCatalogo").ToString,
            '                                       row("cIdEnlaceCatalogo").ToString, row("vDescripcionCatalogo").ToString.Trim, row("vDescripcionAbreviadaCatalogo").ToString.Trim,
            '                                       "1", IIf(row("nVidaUtilCatalogo").ToString.Trim = "", "0", row("nVidaUtilCatalogo").ToString),
            '                                       row("cIdCuentaContableCatalogo").ToString, row("cIdCuentaContableLeasingCatalogo").ToString, row("vDescripcionAbreviadaTipoActivo").ToString, row("vDescripcionAbreviadaSistemaFuncional").ToString,
            '                                       Session("CestaCatalogo"))
            '        Next
            '    End If
            'End If
            Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
            Me.grdListaDetalle.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            grdListaDetalle.DataSource = Nothing
            grdListaDetalle.DataBind()
        End Try
    End Sub

    Sub CargarCestaCaracteristica()
        Try
            ''Lo quite porque no me genera el mensaje correcto.
            'clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            ''If hfdOperacion.Value <> "N" Then
            'Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("cIdEnlaceCatalogo", txtIdCatalogo.Text, "1", "1")
            '    If Coleccion.Count > 0 Then
            '        Dim dtNewColeccion = CatalogoNeg.CatalogoGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, " &
            '                                                         "       CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
            '                                                         "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
            '                                                         "     CATCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
            '                                                         "     INNER JOIN LOGI_CATALOGO AS CAT ON " &
            '                                                         "     CAT.cIdCatalogo = CATCAR.cIdCatalogo AND CAT.cIdJerarquiaCatalogo = CATCAR.cIdJerarquiaCatalogo " &
            '                                                         "WHERE CAT.cIdEnlaceCatalogo = '" & txtIdCatalogo.Text & "' AND CATCAR.cIdJerarquiaCatalogo = '1' " &
            '                                                         "ORDER BY CATCAR.cIdCatalogo")
            '        For Each row In dtNewColeccion.Rows
            '            clsLogiCestaCatalogoCaracteristica.AgregarCesta(row("cIdCatalogo"), row("cIdJerarquiaCatalogo").ToString,
            '                                       row("cIdCaracteristica").ToString, row("vDescripcionCaracteristica"), row("cIdReferenciaSAPCatalogoCaracteristica"), row("vDescripcionCampoSAPCatalogoCaracteristica"), row("vValorCatalogoCaracteristica"),
            '                                       Session("CestaCatalogoCaracteristica"))
            '        Next
            '    'End If
            'End If
            'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            'Me.grdDetalleCaracteristica.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            grdListaDetalle.DataSource = Nothing
            grdListaDetalle.DataBind()
        End Try
    End Sub

    Sub LlenarData()
        Try
            'Dim Catalogo As LOGI_CATALOGO = CatalogoNeg.CatalogoListarPorId(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(7).Text, "0", "1")
            'txtIdCatalogo.Text = Catalogo.cIdCatalogo
            'txtDescripcion.Text = Catalogo.vDescripcionCatalogo
            'txtDescripcionAbreviada.Text = Catalogo.vDescripcionAbreviadaCatalogo
            'txtVidaUtil.Text = IIf(IsNothing(Catalogo.nVidaUtilCatalogo), "0", Catalogo.nVidaUtilCatalogo)
            'txtCuentaContable.Text = Catalogo.cIdCuentaContableCatalogo
            'txtCuentaContableLeasing.Text = Catalogo.cIdCuentaContableLeasingCatalogo
            'cboTipoActivo.SelectedValue = Catalogo.cIdTipoActivo

            ListarSistemaFuncionalCombo()
            'hfdSistemaFuncional.Value = Catalogo.cIdSistemaFuncionalCatalogo
            CargarCestaCatalogo()
            CargarCestaCaracteristica()
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
        'Me.rfvTipoActivo.EnableClientScript = bValidar
        'Me.rfvDescripcion.EnableClientScript = bValidar
        'Me.rfvDescripcionAbreviada.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        'Me.btnNuevo.Visible = bNuevo
        'Me.btnEditar.Visible = bEditar
        'Me.btnGuardar.Visible = bGuardar
        'Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        'If hfdOperacion.Value = "N" Then
        '    MyValidator.ErrorMessage = ""
        '    txtIdCatalogo.Text = ""
        '    cboTipoActivo.SelectedIndex = -1
        '    txtDescripcion.Text = ""
        '    txtDescripcionAbreviada.Text = ""
        '    txtVidaUtil.Text = ""
        '    txtCuentaContable.Text = ""
        '    txtCuentaContableLeasing.Text = ""
        'End If

        'If hfdOperacionDetalle.Value = "N" Then
        '    txtIdDetalleCatalogo.Text = ""
        '    cboTipoActivoDetalleCatalogo.SelectedIndex = -1
        '    cboSistemaFuncionalDetalleCatalogo.SelectedIndex = -1
        '    txtDescripcionDetalleCatalogo.Text = ""
        '    txtDescripcionAbreviadaDetalleCatalogo.Text = ""
        '    'txtDimensionesDetalleCatalogo.Text = ""
        '    'txtVoltajeDetalleCatalogo.Text = ""
        '    'txtPesoDetalleCatalogo.Text = ""
        '    txtVidaUtilDetalleCatalogo.Text = ""
        '    txtCuentaContableDetalleCatalogo.Text = ""
        '    txtCuentaContableLeasingDetalleCatalogo.Text = ""
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "125" 'Mantenimiento de los Catálogos/Componente.

            CargarPerfil()

            'If hfdOperacion.Value = "" Then
            '    hfdOperacion.Value = "R"
            'End If

            'ListarFiltroBusquedaCombo()
            'cboFiltro.SelectedIndex = 3
            ListarTipoActivoCombo()
            ListarSistemaFuncionalCombo()

            If Session("CestaCatalogo") Is Nothing Then
                Session("CestaCatalogo") = clsLogiCestaCatalogo.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogo"))
            End If

            'BloquearPagina(1)
            BloquearMantenimiento(True, False, True, False)


            If Session("CestaCatalogoCaracteristica") Is Nothing Then
                Session("CestaCatalogoCaracteristica") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            End If
            Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristica.DataBind()

            'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
            'Me.grdLista.DataBind()
        Else
            'txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanListado_txtBuscar')")
            'txtIdCatalogo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_cboFamilia')")
            'cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcion')")
            ''cboSistemaFuncional.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcion')")
            'txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcioAbreviada')")
            'txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContable')")
            'txtCuentaContable.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContableLeasing')")
            'txtCuentaContableLeasing.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtCuentaContableLeasing')")
            'btnNuevo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContCatalogo_TabPanGeneral_txtDescripcion')")
            'btnGuardar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnEditar')")
            'btnEditar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnDeshacer')")
            'btnDeshacer.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_btnEliminar')")

            'If hfdOperacion.Value = "E" Or hfdOperacion.Value = "N" Then
            '    BloquearPagina(2)
            'End If
        End If
    End Sub

    'Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
    '    MyValidator.ErrorMessage = ""
    '    Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
    '    Me.grdLista.DataBind()
    '    Me.grdLista.SelectedIndex = 0
    'End Sub

    'Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
    '    grdLista.PageIndex = e.NewPageIndex
    '    Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
    '    Me.grdLista.DataBind() 'Recargo el grid.
    '    grdLista.SelectedIndex = -1
    '    BloquearPagina(1)
    'End Sub
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True
        Catch ex As Exception

        End Try
    End Sub


    'Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

    '        'hfdOperacion.Value = "N"
    '        'BloquearPagina(2)
    '        txtDescripcion.Focus()

    '        BloquearMantenimiento(False, True, False, True)
    '        LimpiarObjetos()
    '        CargarCestaCatalogo()
    '        ValidarTexto(True)
    '        ActivarObjetos(True)
    '        hfTab.Value = "tab2"
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

    '        'hfdOperacion.Value = "E"
    '        'If BloquearPagina(2) = False Then
    '        '    BloquearMantenimiento(False, True, False, True)
    '        '    ValidarTexto(True)
    '        '    ActivarObjetos(True)
    '        '    LlenarData()
    '        '    hfTab.Value = "tab2"
    '        '    ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        'End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Protected Sub btnDeshacer_Click(sender As Object, e As EventArgs) Handles btnDeshacer.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0640", strOpcionModulo, Session("IdSistema"))

    '        'hfdOperacion.Value = "R"
    '        'BloquearPagina(0)
    '        BloquearMantenimiento(True, False, True, False)
    '        ValidarTexto(False)
    '        ActivarObjetos(False)
    '        hfTab.Value = "tab1"
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0639", strOpcionModulo, Session("IdSistema"))

    '        Dim Catalogo As New LOGI_CATALOGO
    '        Catalogo.cIdCatalogo = UCase(txtIdCatalogo.Text)
    '        Catalogo.cIdTipoActivo = cboTipoActivo.SelectedValue
    '        'Catalogo.cIdSistemaFuncionalCatalogo = hfdSistemaFuncional.Value
    '        Catalogo.cIdJerarquiaCatalogo = "0"
    '        Catalogo.cIdEnlaceCatalogo = ""
    '        Catalogo.vDescripcionCatalogo = UCase(txtDescripcion.Text)
    '        Catalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviada.Text)
    '        Catalogo.bEstadoRegistroCatalogo = True
    '        Catalogo.cIdCuentaContableCatalogo = txtCuentaContable.Text
    '        Catalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasing.Text
    '        'Catalogo.bActivacionAutomaticaCatalogo = IIf(lnkEstadoOnActivacion.Visible = True, True, False)

    '        Dim Coleccion As New List(Of LOGI_CATALOGO)
    '        For i = 0 To Session("CestaCatalogo").Rows.Count - 1
    '            Dim DetCatalogo As New LOGI_CATALOGO
    '            DetCatalogo.cIdCatalogo = Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
    '            DetCatalogo.cIdEnlaceCatalogo = Catalogo.cIdCatalogo 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
    '            DetCatalogo.cIdJerarquiaCatalogo = Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
    '            DetCatalogo.cIdSistemaFuncionalCatalogo = Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString.Trim
    '            DetCatalogo.cIdTipoActivo = Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString.Trim
    '            DetCatalogo.vDescripcionCatalogo = Session("CestaCatalogo").Rows(i)("Descripcion").ToString.Trim
    '            DetCatalogo.vDescripcionAbreviadaCatalogo = Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString.Trim
    '            DetCatalogo.bEstadoRegistroCatalogo = Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
    '            'DetCatalogo.vDimensionesCatalogo = Session("CestaCatalogo").Rows(i)("Dimensiones").ToString.Trim
    '            'DetCatalogo.vVoltajeCatalogo = Session("CestaCatalogo").Rows(i)("Voltaje").ToString.Trim
    '            'DetCatalogo.vPesoCatalogo = Session("CestaCatalogo").Rows(i)("Peso").ToString.Trim
    '            DetCatalogo.nVidaUtilCatalogo = Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
    '            DetCatalogo.cIdCuentaContableCatalogo = Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
    '            DetCatalogo.cIdCuentaContableLeasingCatalogo = Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString.Trim
    '            'DetCatalogo.bActivacionAutomaticaCatalogo = Catalogo.bActivacionAutomaticaCatalogo
    '            Coleccion.Add(DetCatalogo)
    '        Next

    '        Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
    '        For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
    '            Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
    '            DetCaracteristica.cIdCatalogo = Session("CestaCatalogoCaracteristica").Rows(i)("IdCatalogo").ToString.Trim
    '            DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCatalogoCaracteristica").Rows(i)("IdJerarquia").ToString.Trim
    '            DetCaracteristica.cIdCaracteristica = Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
    '            DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCatalogoCaracteristica").Rows(i)("Item").ToString.Trim
    '            DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCatalogoCaracteristica").Rows(i)("IdReferenciaSAP").ToString.Trim
    '            DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCatalogoCaracteristica").Rows(i)("DescripcionCampoSAP").ToString.Trim
    '            'DetCaracteristica.vDescripcionAbreviadaCatalogo = Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString.Trim
    '            'DetCaracteristica.bEstadoRegistroCatalogo = Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
    '            ''DetCatalogo.vDimensionesCatalogo = Session("CestaCatalogo").Rows(i)("Dimensiones").ToString.Trim
    '            ''DetCatalogo.vVoltajeCatalogo = Session("CestaCatalogo").Rows(i)("Voltaje").ToString.Trim
    '            ''DetCatalogo.vPesoCatalogo = Session("CestaCatalogo").Rows(i)("Peso").ToString.Trim
    '            'DetCaracteristica.nVidaUtilCatalogo = Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
    '            'DetCaracteristica.cIdCuentaContableCatalogo = Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
    '            'DetCaracteristica.cIdCuentaContableLeasingCatalogo = Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString.Trim
    '            ''DetCatalogo.bActivacionAutomaticaCatalogo = Catalogo.bActivacionAutomaticaCatalogo
    '            ColeccionCaracteristica.Add(DetCaracteristica)
    '        Next

    '        Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '        LogAuditoria.cIdPaisOrigen = Session("IdPais")
    '        LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '        LogAuditoria.cIdLocal = Session("IdLocal")
    '        LogAuditoria.cIdUsuario = Session("IdUsuario")
    '        LogAuditoria.vIP1 = Session("IP1")
    '        LogAuditoria.vIP2 = Session("IP2")
    '        LogAuditoria.vPagina = Session("URL")
    '        LogAuditoria.vSesion = Session("IdSesion")
    '        If hfdOperacion.Value = "N" Then
    '            If (CatalogoNeg.CatalogoInserta(Catalogo)) = 0 Then
    '                If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, Catalogo.cIdCatalogo) = 0 Then
    '                    If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then

    '                    End If
    '                    'Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" & Catalogo.cIdTipoActivo & "', '" &
    '                    '                   Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncionalCatalogo & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
    '                    '                   Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
    '                    '                   Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.vDimensionesCatalogo & "', '" &
    '                    '                   Catalogo.vVoltajeCatalogo & "', '" & Catalogo.vPesoCatalogo & "', " & Catalogo.nVidaUtilCatalogo & ", '" &
    '                    '                   Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCatalogo & "'"
    '                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" & Catalogo.cIdTipoActivo & "', '" &
    '                                       Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncionalCatalogo & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
    '                                       Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
    '                                       Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.nVidaUtilCatalogo & ", '" &
    '                                       Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCatalogo & "'"

    '                        LogAuditoria.vEvento = "INSERTAR CATALOGO"
    '                        LogAuditoria.vQuery = Session("Query")
    '                        LogAuditoria.cIdSistema = Session("IdSistema")
    '                        LogAuditoria.cIdModulo = strOpcionModulo

    '                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

    '                        txtIdCatalogo.Text = Catalogo.cIdCatalogo
    '                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
    '                        CargarCestaCatalogo()
    '                        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
    '                        Me.grdLista.DataBind()
    '                        pnlGeneral.Enabled = False
    '                        BloquearMantenimiento(True, False, True, False)
    '                        hfdOperacion.Value = "R"
    '                        hfTab.Value = "tab1"
    '                    Else
    '                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
    '                End If
    '            End If
    '        ElseIf hfdOperacion.Value = "E" Then
    '            If (CatalogoNeg.CatalogoEdita(Catalogo)) = 0 Then
    '                If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, Catalogo.cIdCatalogo) = 0 Then
    '                    If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then

    '                    End If
    '                    'Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" & Catalogo.cIdTipoActivo & "', '" &
    '                    '                   Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
    '                    '                   Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
    '                    '                   Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.vDimensionesCatalogo & "', '" &
    '                    '                   Catalogo.vVoltajeCatalogo & "', '" & Catalogo.vPesoCatalogo & "', " & Catalogo.nVidaUtilCatalogo & ", '" &
    '                    '                   Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCatalogo & "'"
    '                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" & Catalogo.cIdTipoActivo & "', '" &
    '                                       Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncionalCatalogo & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
    '                                       Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
    '                                       Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.nVidaUtilCatalogo & ", '" &
    '                                       Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCatalogo & "'"

    '                    LogAuditoria.vEvento = "ACTUALIZAR CATALOGO"
    '                    LogAuditoria.vQuery = Session("Query")
    '                    LogAuditoria.cIdSistema = Session("IdSistema")
    '                    LogAuditoria.cIdModulo = strOpcionModulo

    '                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

    '                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
    '                    Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
    '                    Me.grdLista.DataBind()
    '                    pnlGeneral.Enabled = False
    '                    BloquearMantenimiento(True, False, True, False)
    '                    hfdOperacion.Value = "R"
    '                    hfTab.Value = "tab1"
    '                Else
    '                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
    '                End If
    '            End If
    '        End If
    '        BloquearPagina(0)
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub



    'Protected Sub imgbtnEliminar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEliminar.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        lblMensaje.Text = ""
    '        Dim Catalogo As New LOGI_CATALOGO
    '        'If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "234", strOpcionModulo, Session("IdSistema"), Session("IdArea")) Then
    '        If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "069", strOpcionModulo, Session("IdSistema"), Session("IdArea")) Then
    '            Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
    '        Else
    '            If grdLista.Rows.Count = 0 Then
    '                Throw New Exception("Seleccione un registro a eliminar.")
    '            ElseIf grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = True Then
    '                    Throw New Exception("Seleccione un registro a eliminar.")
    '                Else
    '                    If TabContCatalogo.Tabs(1).Enabled = True And IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        If hfdOperacion.Value = "R" Or IsNothing(hfdOperacion.Value) = True Then
    '                            LlenarData()
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If
    '        'Catalogo.cIdFamilia = cboFamilia.SelectedValue
    '        Catalogo.cIdCatalogo = txtIdCatalogo.Text

    '        If (CatalogoNeg.CatalogoElimina(Catalogo)) = 0 Then
    '            Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '            'LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '            'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
    '            'LogAuditoria.cIdUsuario = Session("IdUsuario")
    '            LogAuditoria.cIdUsuario = Session("IdUsuario")
    '            LogAuditoria.vIP1 = Session("IP1")
    '            LogAuditoria.vIP2 = Session("IP2")
    '            LogAuditoria.vPagina = Session("URL")
    '            LogAuditoria.vSesion = Session("IdSesion")

    '            'Session("Query") = "PA_LOGI_MNT_Catalogo 'SQL_NONE', 'UPDATE LOGI_Catalogo SET bEstadoRegistroCatalogo = 0 " & _
    '            '                   "WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdAlmacen = '" & Catalogo.cIdAlmacen & "' AND cIdEmpresa = '" & Catalogo.cIdEmpresa & "' AND cIdPuntoVenta = '" & Catalogo.cIdPuntoVenta & "', '', '', '', '', '', '', '0', '', '', '' "
    '            'UPDATE LOGI_Catalogo SET bEstadoRegistroCatalogo = 0 " & _

    '            Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_NONE', 'UPDATE LOGI_Catalogo SET bEstadoRegistroCatalogo = 0 " & _
    '                               "WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "', " & _
    '                               "'', '', '', '', '', '', '', '1', ''"

    '            LogAuditoria.vEvento = "ELIMINAR CATALOGO"
    '            LogAuditoria.vQuery = Session("Query")
    '            LogAuditoria.cIdSistema = Session("IdSistema")
    '            LogAuditoria.cIdModulo = strOpcionModulo

    '            FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

    '            lblMensaje.Text = "Registro eliminado con éxito"
    '        End If
    '        'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, txtIdCatalogo.Text, _
    '        '                                                         Session("IdEmpresa"), Session("IdPuntoVenta"), 0)
    '        Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, 0)
    '        Me.grdLista.DataBind()
    '        pnlGeneral.Enabled = False
    '        BloquearMantenimiento(True, False, True, False, True)
    '        hfdOperacion.Value = "R"
    '        grdLista.SelectedIndex = -1
    '        BloquearPagina(0)
    '    Catch ex As Exception
    '        lblMensaje.Text = ex.Message
    '        BloquearPagina(0)
    '    End Try
    'End Sub

    'Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
    '    Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
    '    Me.grdLista.DataBind()
    'End Sub

    'Private Sub grdLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim td As TableCell
    '        For Each td In e.Row.Cells
    '            e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
    '            e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
    '            e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
    '            e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Left
    '            e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left
    '            e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left
    '            'e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left
    '        Next
    '    End If
    'End Sub

    'Private Sub grdLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(1).Visible = True
    '        e.Row.Cells(2).Visible = True
    '        e.Row.Cells(3).Visible = True
    '        e.Row.Cells(4).Visible = True
    '        e.Row.Cells(5).Visible = True
    '        e.Row.Cells(6).Visible = True
    '        e.Row.Cells(7).Visible = False
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(1).Visible = True
    '        e.Row.Cells(2).Visible = True
    '        e.Row.Cells(3).Visible = True
    '        e.Row.Cells(4).Visible = True
    '        e.Row.Cells(5).Visible = True
    '        e.Row.Cells(6).Visible = True
    '        e.Row.Cells(7).Visible = False
    '    End If
    'End Sub

    'Private Sub grdLista_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdLista.SelectedIndexChanged
    '    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '        BloquearPagina(0)
    '        ValidarTexto(False)
    '        LlenarData() 'JMUG: 06/02/2023
    '    End If
    'End Sub

    'Private Sub TabPanGeneral_PreRender(sender As Object, e As System.EventArgs) Handles TabPanGeneral.PreRender
    '    If InStr(MyValidator.ErrorMessage, "permiso") > 0 Then
    '        BloquearPagina(0)
    '        Exit Sub
    '    End If

    '    If grdLista.Rows.Count > 0 Then
    '        If IsNothing(grdLista.SelectedRow) = True Then
    '            If hfdOperacion.Value = "R" And MyValidator.ErrorMessage.Trim = "" Then
    '                MyValidator.ErrorMessage = "Seleccione un registro a visualizar."
    '                MyValidator.IsValid = False
    '                MyValidator.ID = "ErrorPersonalizado"
    '                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '                Me.Page.Validators.Add(MyValidator)
    '            ElseIf hfdOperacion.Value = "R" And InStr(MyValidator.ErrorMessage.Trim, "Seleccione un registro") > 0 Then
    '                txtBuscar.Focus()
    '            ElseIf hfdOperacion.Value = "N" And TabContCatalogo.Tabs(0).Enabled = True And TabContCatalogo.Tabs(1).Enabled = True And strTabContenedorActivo = 1 Then 'And intCantidadInstancia = 1 Then
    '                txtDescripcion.Focus()
    '            End If
    '        Else
    '            If TabContCatalogo.Tabs(1).Enabled = True And IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                If hfdOperacion.Value = "R" Or IsNothing(hfdOperacion.Value) = True Then
    '                    LlenarData()
    '                    txtDescripcion.Focus()
    '                Else
    '                    If Not strTabContenedorActivo Is Nothing Then
    '                        BloquearPagina(2)
    '                        txtDescripcion.Focus()
    '                    End If
    '                End If
    '            End If
    '        End If
    '    Else
    '        If hfdOperacion.Value = "N" Then
    '            BloquearPagina(2)
    '        Else
    '            BloquearPagina(0)
    '        End If
    '    End If
    'End Sub

    'Private Sub TabContCatalogo_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContCatalogo.ActiveTabChanged
    '    'Solo se activa cuando es el primer tag
    '    strTabContenedorActivo = TabContCatalogo.ActiveTabIndex.ToString()
    'End Sub

    'Protected Sub imgbtnAceptarDetalleCatalogo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnAceptarDetalleCatalogo.Click
    Protected Sub btnAceptarDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles btnAceptarDetalleCatalogo.Click
        Try
            If cboTipoActivoDetalleCatalogo.SelectedIndex <= 0 Then
                Throw New Exception("Debe de ingresar el código del tipo de activo correcto.")
            ElseIf cboSistemaFuncionalDetalleCatalogo.SelectedIndex < 0 Then
                Throw New Exception("Debe de ingresar el código del sistema funcional correcto.")
            ElseIf txtDescripcionDetalleCatalogo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción correcta.")
            ElseIf txtDescripcionAbreviadaDetalleCatalogo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción abreviada correcta.")
            Else
                'If hfdOperacion.Value = "E" Then
                '    If hfdOperacionDetalle.Value = "E" Then
                'clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '                                 "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '                                 txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                '                                 "1", txtDimensionesDetalleCatalogo.Text.Trim, txtVoltajeDetalleCatalogo.Text.Trim, txtPesoDetalleCatalogo.Text, txtVidaUtilDetalleCatalogo.Text.Trim,
                '                                 txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '                                 cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                'clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '                                         "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '                                         txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                '                                         "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                '                                         txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '                                         cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                'ElseIf hfdOperacionDetalle.Value = "N" Then
                '    'clsLogiCestaCatalogo.AgregarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '    '             "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '    '             txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                '    '             "1", txtDimensionesDetalleCatalogo.Text.Trim, txtVoltajeDetalleCatalogo.Text.Trim, txtPesoDetalleCatalogo.Text.Trim, txtVidaUtilDetalleCatalogo.Text.Trim,
                '    '             txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '    '             cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"))
                '    clsLogiCestaCatalogo.AgregarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '                 "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '                 txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                '                 "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                '                 txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '                 cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"))
                'End If
                'ElseIf hfdOperacion.Value = "N" Then
                '    If hfdOperacionDetalle.Value = "E" Then
                'clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '                                 "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '                                 txtIdCatalogo.Text.Trim, txtDescripcionDetalleCatalogo.Text.Trim, txtDescripcionAbreviadaDetalleCatalogo.Text.Trim,
                '                                 "1", txtDimensionesDetalleCatalogo.Text.Trim, txtVoltajeDetalleCatalogo.Text.Trim, txtPesoDetalleCatalogo.Text.Trim, txtVidaUtilDetalleCatalogo.Text.Trim,
                '                                 txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '             cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                'clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '                                         "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '                                         txtIdCatalogo.Text.Trim, txtDescripcionDetalleCatalogo.Text.Trim, txtDescripcionAbreviadaDetalleCatalogo.Text.Trim,
                '                                         "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                '                                         txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '                                         cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                'ElseIf hfdOperacionDetalle.Value = "N" Then
                'clsLogiCestaCatalogo.AgregarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '             "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '             txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                '             "1", txtDimensionesDetalleCatalogo.Text.Trim, txtVoltajeDetalleCatalogo.Text.Trim, txtPesoDetalleCatalogo.Text.Trim, txtVidaUtilDetalleCatalogo.Text.Trim,
                '             txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '             cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"))
                'clsLogiCestaCatalogo.AgregarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                '                                          "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                '                                          txtIdCatalogo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                '                                          "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                '                                          txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                '                                          cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, Session("CestaCatalogo"))
            End If
            '    End If
            'End If
            Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
            Me.grdListaDetalle.DataBind()
            Me.lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Hide()
        Catch ex As Exception
            lblMensajeDetalleCatalogo.Text = ex.Message
            Me.lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdListaDetalle_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdListaDetalle.PageIndexChanging
        grdListaDetalle.PageIndex = e.NewPageIndex
        Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
        Me.grdListaDetalle.DataBind()
        grdListaDetalle.SelectedIndex = -1
    End Sub

    Private Sub grdListaDetalle_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdListaDetalle.RowDeleting
        'Try
        '    'Función para validar si tiene permisos
        '    MyValidator.ErrorMessage = ""
        '    fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
        '    FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "069", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

        '    'If grdListaDetalle.Rows.Count > 0 Then
        '    ' If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
        '    ' If IsNothing(grdListaDetalle.SelectedRow) = False Then
        '    ' If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then

        '    Dim dtCatalogoComponente = CatalogoNeg.CatalogoGetData("SELECT COUNT(*) AS Cantidad FROM LOGI_MAESTROACTIVO WHERE cIdEnlaceCatalogo = '" & UCase(txtIdCatalogo.Text) & "' AND cIdCatalogo = '" & grdListaDetalle.Rows.Item(e.RowIndex).Cells(2).Text & "'")
        '    If dtCatalogoComponente.Rows(0).Item(0) > 0 Then
        '        Throw New Exception("No puede eliminar este item porque tiene movimientos.")
        '    End If

        '    'clsLogiCestaCatalogo.QuitarCesta(e.RowIndex, Session("CestaCatalogo"), IIf(txtNroPagina.Visible = True, txtNroPagina.Text, 0))

        '    clsLogiCestaCatalogo.QuitarCesta(e.RowIndex, Session("CestaCatalogo"), 0)

        '    Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
        '    Me.grdListaDetalle.DataBind()

        '    'BloquearMantenimiento(True, True, True, True, False)
        '    BloquearMantenimiento(False, True, False, True, False)
        '    'hfdOperacion.Value = "E"
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

    'Protected Sub btnNuevoDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles btnNuevoDetalleCatalogo.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        lblMensajeDetalleCatalogo.Text = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0642", strOpcionModulo, Session("IdSistema"))
    '        hfdOperacionDetalle.Value = "N"
    '        LimpiarObjetos()
    '        'cboTipoActivoDetalleCatalogo.Enabled = True
    '        cboTipoActivoDetalleCatalogo.SelectedValue = cboTipoActivo.SelectedValue
    '        If cboSistemaFuncionalDetalleCatalogo.Items.Count = 0 Then
    '            Throw New Exception("Debe de ingresar los sistemas funcionales del catálogo.")
    '        End If
    '        cboSistemaFuncionalDetalleCatalogo.SelectedIndex = 0

    '        lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '        BloquearPagina(2)
    '    End Try
    'End Sub

    'Protected Sub btnEditarDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles btnEditarDetalleCatalogo.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        lblMensajeDetalleCatalogo.Text = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

    '        hfdOperacionDetalle.Value = "E"
    '        If grdListaDetalle.Rows.Count > 0 Then
    '            If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
    '                If IsNothing(grdListaDetalle.SelectedRow) = False Then
    '                    If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then
    '                        Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(5).Text).Trim & "'")
    '                        rowIndexDetalle = Session("CestaCatalogo").Rows.IndexOf(result(0))
    '                        LlenarDataDetalle()
    '                        BloquearMantenimiento(False, True, False, True)
    '                        ValidarTexto(True)
    '                        ActivarObjetos(True)
    '                        lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
    '                    End If
    '                Else
    '                    Throw New Exception("Debe de seleccionar algún item.")
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '        BloquearPagina(2)
    '    End Try
    'End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                'Valores(0).ToString() 'Codigo.

                Catalogo.cIdCatalogo = Valores(0).ToString()
                Catalogo.cIdTipoActivo = Valores(1).ToString()
                Catalogo.cIdJerarquiaCatalogo = "0"

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
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

                'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                'Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                'Valores(0).ToString() 'Codigo.

                Catalogo.cIdCatalogo = Valores(0).ToString()
                Catalogo.cIdTipoActivo = Valores(1).ToString()
                Catalogo.cIdJerarquiaCatalogo = "0"

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
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

                'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                'Me.grdLista.DataBind()
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

    Private Sub grdListaDetalle_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdListaDetalle.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(11).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdListaDetalle_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdListaDetalle.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = True
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = True
            e.Row.Cells(9).Visible = True
            e.Row.Cells(10).Visible = True
            e.Row.Cells(11).Visible = False 'Estado
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = True
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = True
            e.Row.Cells(9).Visible = True
            e.Row.Cells(10).Visible = True
            e.Row.Cells(11).Visible = False 'Estado
        End If
    End Sub

    'Sub ActivarDesactivarSwitch(Obj As LinkButton, ActDes As Boolean)
    '    Obj.Visible = ActDes
    'End Sub

    'Private Sub lnkEstadoOnActivacion_Click(sender As Object, e As EventArgs) Handles lnkEstadoOnActivacion.Click
    '    ActivarDesactivarSwitch(lnkEstadoOnActivacion, False)
    '    ActivarDesactivarSwitch(lnkEstadoOffActivacion, True)
    'End Sub

    'Private Sub lnkEstadoOffActivacion_Click(sender As Object, e As EventArgs) Handles lnkEstadoOffActivacion.Click
    '    ActivarDesactivarSwitch(lnkEstadoOnActivacion, True)
    '    ActivarDesactivarSwitch(lnkEstadoOffActivacion, False)
    'End Sub

    Protected Sub grdListaDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0643", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                'Valores(0).ToString() 'Codigo.

                Dim i As Integer

                i = Valores(0)
                Catalogo.cIdEnlaceCatalogo = Valores(1).ToString
                Catalogo.cIdCatalogo = Valores(2).ToString()
                Catalogo.cIdTipoActivo = Valores(3).ToString()
                Catalogo.cIdJerarquiaCatalogo = "1"
                'TipoActivo.cIdEmpresa = Session("IdEmpresa")
                'TipoActivo.cIdPuntoVenta = Session("IdPuntoVenta")

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

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "DESACTIVAR CATALOGO/COMPONENTE"
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

                'clsLogiCestaCatalogo.EditarCesta(Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString, Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString, Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString,
                '                                 Session("CestaCatalogo").Rows(i)("IdEnlace").ToString, Session("CestaCatalogo").Rows(i)("Descripcion").ToString, Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString, False,
                '                                 Session("CestaCatalogo").Rows(i)("Dimensiones").ToString, Session("CestaCatalogo").Rows(i)("Voltaje").ToString, Session("CestaCatalogo").Rows(i)("Peso").ToString, Session("CestaCatalogo").Rows(i)("VidaUtil").ToString,
                '                                 Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString, Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevSistemaFuncional").ToString,
                '                                 Session("CestaCatalogo"), i)
                clsLogiCestaCatalogo.EditarCesta(Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString, Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString, Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdEnlace").ToString, Session("CestaCatalogo").Rows(i)("Descripcion").ToString, Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString, False,
                                                 Session("CestaCatalogo").Rows(i)("VidaUtil").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString, Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("PeriodoGarantia").ToString, Session("CestaCatalogo").Rows(i)("PeriodoMinimo").ToString,
                                                 Session("CestaCatalogo"), i)
                Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
                Me.grdListaDetalle.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Catalogo As New LOGI_CATALOGO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0643", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                'Valores(0).ToString() 'Codigo.

                Dim i As Integer
                i = Valores(0)
                Catalogo.cIdEnlaceCatalogo = Valores(1).ToString
                Catalogo.cIdCatalogo = Valores(2).ToString()
                Catalogo.cIdTipoActivo = Valores(3).ToString()
                Catalogo.cIdJerarquiaCatalogo = "1"
                'Producto.cIdEmpresa = Session("IdEmpresa")
                'Producto.cIdPuntoVenta = Session("IdPuntoVenta")

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

                CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdEnlaceCatalogo = '" & Catalogo.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "ACTIVAR CATALOGO/COMPONENTE"
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

                'clsLogiCestaCatalogo.EditarCesta(Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString, Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString, Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString,
                '                                 Session("CestaCatalogo").Rows(i)("IdEnlace").ToString, Session("CestaCatalogo").Rows(i)("Descripcion").ToString, Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString, True,
                '                                 Session("CestaCatalogo").Rows(i)("Dimensiones").ToString, Session("CestaCatalogo").Rows(i)("Voltaje").ToString, Session("CestaCatalogo").Rows(i)("Peso").ToString, Session("CestaCatalogo").Rows(i)("VidaUtil").ToString,
                '                                 Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString, Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevSistemaFuncional").ToString,
                '                                 Session("CestaCatalogo"), i)
                clsLogiCestaCatalogo.EditarCesta(Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString, Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString, Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdEnlace").ToString, Session("CestaCatalogo").Rows(i)("Descripcion").ToString, Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString, True,
                                                 Session("CestaCatalogo").Rows(i)("VidaUtil").ToString,
                                                 Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString, Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevTipoActivo").ToString, Session("CestaCatalogo").Rows(i)("DescAbrevSistemaFuncional").ToString,
                                                 Session("CestaCatalogo").Rows(i)("PeriodoGarantia").ToString, Session("CestaCatalogo").Rows(i)("PeriodoMinimo").ToString,
                                                 Session("CestaCatalogo"), i)
                Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
                Me.grdListaDetalle.DataBind()
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

    Private Sub lnkbtnAgregarComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnAgregarComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            lblMensajeDetalleCatalogo.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0642", strOpcionModulo, Session("IdSistema"))
            'hfdOperacionDetalle.Value = "N"
            LimpiarObjetos()
            'cboTipoActivoDetalleCatalogo.Enabled = True
            'cboTipoActivoDetalleCatalogo.SelectedValue = cboTipoActivo.SelectedValue
            'If cboSistemaFuncionalDetalleCatalogo.Items.Count = 0 Then
            '    Throw New Exception("Debe de ingresar los sistemas funcionales del catálogo.")
            'End If
            'JMUG: 28/02/2023 cboSistemaFuncionalDetalleCatalogo.SelectedIndex = 0

            lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            'BloquearPagina(2)
        End Try
    End Sub

    Private Sub lnkbtnEditarComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            lblMensajeDetalleCatalogo.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If Session("CestaCatalogoCaracteristicaFiltrado") Is Nothing Then
                Session("CestaCatalogoCaracteristicaFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristicaFiltrado"))
            End If

            'hfdOperacionDetalle.Value = "E"
            'If grdListaDetalle.Rows.Count > 0 Then
            '    If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
            '        If IsNothing(grdListaDetalle.SelectedRow) = False Then
            '            If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then
            '                Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(5).Text).Trim & "'")
            '                rowIndexDetalle = Session("CestaCatalogo").Rows.IndexOf(result(0))
            '                LlenarDataDetalle()
            '                BloquearMantenimiento(False, True, False, True)
            '                ValidarTexto(True)
            '                ActivarObjetos(True)
            '                ''CargarCestaCaracteristica()
            '                ''Dim rowIndexDetalleCaracteristica As Int64
            '                'Dim resultCaracteristica As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
            '                ''rowIndexDetalleCaracteristica = Session("CestaCatalogoCaracteristica").Rows.IndexOf(resultCaracteristica(0))
            '                'If resultCaracteristica.Length = 0 Then
            '                '    Me.grdDetalleCaracteristica.DataSource = Nothing
            '                'Else
            '                '    Me.grdDetalleCaracteristica.DataSource = resultCaracteristica(0).Table
            '                'End If
            '                'Me.grdDetalleCaracteristica.DataBind()

            '                Dim resultCaracteristicaSimple As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
            '                If resultCaracteristicaSimple.Length = 0 Then
            '                    Me.grdDetalleCaracteristica.DataSource = Nothing
            '                Else
            '                    Dim rowFil As DataRow() = resultCaracteristicaSimple
            '                    For Each fila As DataRow In rowFil
            '                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCatalogoCaracteristicaFiltrado"))
            '                    Next
            '                End If
            '                Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristicaFiltrado")
            '                Me.grdDetalleCaracteristica.DataBind()
            '                lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
            '            End If
            '        Else
            '            Throw New Exception("Debe de seleccionar algún item.")
            '        End If
            '    End If
            'End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
            'BloquearPagina(2)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaDetalleCatalogo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            lblMensajeDetalleCatalogo.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If Session("CestaCatalogoCaracteristicaFiltrado") Is Nothing Then
                Session("CestaCatalogoCaracteristicaFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristicaFiltrado"))
            End If

            'hfdOperacionDetalle.Value = "E"
            'If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
            '    'If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
            '    '    If lblMensajeCaracteristica.Text = "" Then
            '    For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
            '        If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (hfdIdCaracteristicaDetalleCatalogo.Value.ToString.Trim) And Session("CestaCatalogoCaracteristica").Rows(i)("IdCatalogo").ToString.Trim = txtIdDetalleCatalogo.Text Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
            '            Dim resultCaracteristicaSimple As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
            '            If resultCaracteristicaSimple.Length = 0 Then
            '                Me.grdDetalleCaracteristica.DataSource = Nothing
            '            Else
            '                Dim rowFil As DataRow() = resultCaracteristicaSimple
            '                For Each fila As DataRow In rowFil
            '                    clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCatalogoCaracteristicaFiltrado"))
            '                Next
            '            End If
            '            Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristicaFiltrado")
            '            Me.grdDetalleCaracteristica.DataBind()
            '            lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()

            '            'TotalizarGrid()
            '            'txtImporteFormaPagoOp1.Text = Math.Round(Convert.ToDecimal(txtTotalGeneral.Text), 2)
            '            'LimpiarObjetosProducto()
            '            'LimpiarObjetosCaracteristicas()
            '            'lnk_mostrarPanelProducto_ModalPopupExtender.Show()
            '            'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
            '            'grdListaCaracteristica.SelectedIndex = -1
            '            'lblMensajeCaracteristica.Text = "Característica ya registrada, seleccione otro item."
            '            Throw New Exception("Característica ya registrada, seleccione otro item.")
            '            Exit Sub
            '        End If
            '    Next
            '    clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdDetalleCatalogo.Text, "1", hfdIdCaracteristicaDetalleCatalogo.Value.Trim, UCase(txtBuscarCaracteristicaDetalleCatalogo.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristica"))
            '    clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdDetalleCatalogo.Text, "1", hfdIdCaracteristicaDetalleCatalogo.Value.Trim, UCase(txtBuscarCaracteristicaDetalleCatalogo.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristicaFiltrado"))

            '    ''Dim rowIndexDetalleCaracteristica As Int64
            '    ''Dim resultCaracteristica As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
            '    'Session("CestaCatalogoCaracteristica").row
            '    ''rowIndexDetalleCaracteristica = Session("CestaCatalogoCaracteristica").Rows.IndexOf(resultCaracteristica(0))
            '    'If resultCaracteristica.Length = 0 Then
            '    '    Me.grdDetalleCaracteristica.DataSource = Nothing
            '    'Else
            '    '    Me.grdDetalleCaracteristica.DataSource = resultCaracteristica(0).Table
            '    'End If
            '    'Me.grdDetalleCaracteristica.DataBind()

            '    Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristicaFiltrado")
            '    Me.grdDetalleCaracteristica.DataBind()
            '    lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()


            '    'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            '    'Me.grdDetalleCaracteristica.DataBind()

            '    'LimpiarObjetosCaracteristicas()
            '    txtBuscarCaracteristicaDetalleCatalogo.Text = ""
            '    'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
            '    lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
            '    grdDetalleCaracteristica.SelectedIndex = -1
            '    'grdListaCaracteristica.SelectedIndex = -1
            '    'lblMensajeCaracteristica.Text = "Caracteristica agregada con éxito."
            '    lblMensajeDetalleCatalogo.Text = "Caracteristica agregada con éxito."
            'Else
            '    lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
            'End If
            '    End If
            'End If
            'End If

            'If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
            '            If IsNothing(grdListaDetalle.SelectedRow) = False Then
            '                If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then
            '                    Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(5).Text).Trim & "'")
            '                    rowIndexDetalle = Session("CestaCatalogo").Rows.IndexOf(result(0))
            '                    LlenarDataDetalle()
            '                    BloquearMantenimiento(False, True, False, True)
            '                    ValidarTexto(True)
            '                    ActivarObjetos(True)
            '                    lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
            '                End If
            '            Else
            '                Throw New Exception("Debe de seleccionar algún item.")
            '            End If
            '        End If
            '    End If
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
            'BloquearPagina(2)
        End Try
    End Sub
End Class