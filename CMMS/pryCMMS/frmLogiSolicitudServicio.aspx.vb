Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiSolicitudServicio
    Inherits System.Web.UI.Page
    Dim OrdenFabricacionNeg As New clsOrdenFabricacionNegocios
    Dim CatalogoNeg As New clsCatalogoNegocios
    Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator
    Shared rowIndexDetalle As Int64

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

    Shared Function CrearCestaPersonal() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Personal", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("Responsable", GetType(System.Boolean))) '3
        Return dt
    End Function

    Shared Sub EditarCestaPersonal(ByVal Codigo As String, ByVal Personal As String, ByVal Responsable As Boolean,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Personal
                Tabla.Rows(Fila)(2) = Responsable
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaPersonal(ByVal Codigo As String, ByVal Personal As String, ByVal Responsable As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Personal") = Personal
        Fila("Responsable") = Responsable
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaPersonal(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaPersonal(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    'OT sin equipo
    Sub ListarTipoActivoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoActivoNeg As New clsTipoActivoNegocios
        cboTipoActivo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivo.DataValueField = "cIdTipoActivo"
        cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivo.Items.Clear()
        cboTipoActivo.Items.Add("SELECCIONE DATO")
        cboTipoActivo.DataBind()
    End Sub

    Sub ListarCatalogoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CatalogoNeg As New clsCatalogoNegocios
        cboCatalogo.DataTextField = "vDescripcionCatalogo"
        cboCatalogo.DataValueField = "cIdCatalogo"
        cboCatalogo.DataSource = CatalogoNeg.CatalogoListarCombo(0, cboTipoActivo.SelectedValue, "", "1") 'JMUG: 06/12/2023
        cboCatalogo.Items.Clear()
        cboCatalogo.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboCatalogo.DataBind()
    End Sub

    Sub ListarTipoEquipoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoEquipoNeg As New clsTipoEquipoNegocios
        cboTipoEquipo.DataTextField = "vDescripcionTipoEquipo"
        cboTipoEquipo.DataValueField = "cIdTipoEquipo"
        cboTipoEquipo.DataSource = TipoEquipoNeg.TipoEquipoListarCombo("1")
        cboTipoEquipo.Items.Clear()
        cboTipoEquipo.Items.Add("SELECCIONE DATO")
        cboTipoEquipo.DataBind()
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltroEquipo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroEquipo.DataValueField = "vValor"
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("85", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
    End Sub

    Sub ListarTipoMantenimientoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoMantenimientoNeg As New clsTipoMantenimientoNegocios
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataTextField = "vDescripcionTipoMantenimiento"
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataValueField = "cIdTipoMantenimiento"
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataSource = TipoMantenimientoNeg.TipoMantenimientoListarCombo("1")
        cboTipoMantenimientoMantenimientoOrdenTrabajo.Items.Clear()
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ListarTipoMantenimientoOTxEquipoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoMantenimientoNeg As New clsTipoMantenimientoNegocios
        cboPersonalResponsableMntOTxEquipos.DataTextField = "vDescripcionTipoMantenimiento"
        cboPersonalResponsableMntOTxEquipos.DataValueField = "cIdTipoMantenimiento"
        cboPersonalResponsableMntOTxEquipos.DataSource = TipoMantenimientoNeg.TipoMantenimientoListarCombo("1")
        cboPersonalResponsableMntOTxEquipos.Items.Clear()
        cboPersonalResponsableMntOTxEquipos.DataBind()
    End Sub

    Sub ListarPlantillaCheckListCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        cboListadoCheckListMantenimientoOrdenTrabajo.DataTextField = "vDescripcionCabeceraCheckListPlantilla"
        cboListadoCheckListMantenimientoOrdenTrabajo.DataValueField = "cIdNumeroCabeceraCheckListPlantilla"
        Dim valueTMantOT = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
        Dim valueTActCbc = IIf((valueTMantOT.Trim().Equals("MP") And hfdIdTipoActivoCabecera.Value.Trim().Equals("00")), "15", hfdIdTipoActivoCabecera.Value.Trim())
        Dim valueIdCatCbc = IIf((valueTMantOT.Trim().Equals("MP") And (hfdIdCatalogoCabecera.Value.Trim().Equals("00") Or String.IsNullOrEmpty(hfdIdCatalogoCabecera.Value))), "1500036", hfdIdCatalogoCabecera.Value)
        cboListadoCheckListMantenimientoOrdenTrabajo.DataSource = PlantillaCheckListNeg.ChecklistPlantillaListarCombo(valueTMantOT, valueTActCbc, valueIdCatCbc, hfdIdJerarquiaCatalogoCabecera.Value)
        cboListadoCheckListMantenimientoOrdenTrabajo.Items.Clear()
        cboListadoCheckListMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ListarPlantillaCheckListMntOTxEquipoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        cboListadoCheckListMntOTxEquipos.DataTextField = "vDescripcionCabeceraCheckListPlantilla"
        cboListadoCheckListMntOTxEquipos.DataValueField = "cIdNumeroCabeceraCheckListPlantilla"
        Dim valueTMantOT = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
        Dim valueTActCbc = IIf((valueTMantOT.Trim().Equals("MP") And hfdIdTipoActivoCabecera.Value.Trim().Equals("00")), "15", hfdIdTipoActivoCabecera.Value.Trim())
        Dim valueIdCatCbc = IIf((valueTMantOT.Trim().Equals("MP") And (hfdIdCatalogoCabecera.Value.Trim().Equals("00") Or String.IsNullOrEmpty(hfdIdCatalogoCabecera.Value))), "1500036", hfdIdCatalogoCabecera.Value)
        cboListadoCheckListMntOTxEquipos.DataSource = PlantillaCheckListNeg.ChecklistPlantillaListarCombo(valueTMantOT, valueTActCbc, valueIdCatCbc, hfdIdJerarquiaCatalogoCabecera.Value)
        cboListadoCheckListMntOTxEquipos.Items.Clear()
        cboListadoCheckListMntOTxEquipos.DataBind()
    End Sub

    Sub ListarPersonalResponsableCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataTextField = "vNombreCompletoPersonal"
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataValueField = "cIdPersonal"
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataSource = ResponsableNeg.PersonalListarCombo()
        cboPersonalResponsableMantenimientoOrdenTrabajo.Items.Clear()
        cboPersonalResponsableMantenimientoOrdenTrabajo.Items.Add("SELECCIONE DATO")
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ListarPersonalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataTextField = "vNombreCompletoPersonal"
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataValueField = "cIdPersonal"
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = ResponsableNeg.PersonalListarCombo()
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Items.Clear()
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Items.Add("SELECCIONE DATO")
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ListarPersonalResponsableComboMntOTxEquipos()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboPersonalResponsableMntOTxEquipos.DataTextField = "vNombreCompletoPersonal"
        cboPersonalResponsableMntOTxEquipos.DataValueField = "cIdPersonal"
        cboPersonalResponsableMntOTxEquipos.DataSource = ResponsableNeg.PersonalListarCombo()
        cboPersonalResponsableMntOTxEquipos.Items.Clear()
        cboPersonalResponsableMntOTxEquipos.Items.Add("SELECCIONE DATO")
        cboPersonalResponsableMntOTxEquipos.DataBind()
    End Sub

    Sub ListarPersonalComboMntOTxEquipos()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboPersonalAsignadoMntOTxEquipos.DataTextField = "vNombreCompletoPersonal"
        cboPersonalAsignadoMntOTxEquipos.DataValueField = "cIdPersonal"
        cboPersonalAsignadoMntOTxEquipos.DataSource = ResponsableNeg.PersonalListarCombo()
        cboPersonalAsignadoMntOTxEquipos.Items.Clear()
        cboPersonalAsignadoMntOTxEquipos.Items.Add("SELECCIONE DATO")
        cboPersonalAsignadoMntOTxEquipos.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Enabled = bActivar
        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Enabled = bActivar
        cboTipoMantenimientoMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalResponsableMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Enabled = bActivar
    End Sub

    Sub LlenarData()
        Dim SolicitudServicio As LOGI_CABECERAORDENFABRICACION = OrdenFabricacionNeg.OrdenFabricacionListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Session("IdEmpresa"))
        Dim EquipoNeg As New clsEquipoNegocios
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(9).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = Now.ToString("yyyy-MM-dd")
        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = Now.ToString("yyyy-MM-dd")
        hfdIdCatalogoCabecera.Value = Equipo.cIdCatalogo
        hfdIdTipoActivoCabecera.Value = Equipo.cIdTipoActivo
        hfdIdJerarquiaCatalogoCabecera.Value = Equipo.cIdJerarquiaCatalogo
        cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(cboTipoMantenimientoMantenimientoOrdenTrabajo, New System.EventArgs())

        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
    End Sub

    Sub LlenarDataOTxEquipo()
        ListarTipoActivoCombo()
        ListarTipoEquipoCombo()
        cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())

        txtFechaInicioPlanificadaMntOTxEquipos.Text = Now.ToString("yyyy-MM-dd")
        txtFechaTerminoPlanificadaMntOTxEquipos.Text = Now.ToString("yyyy-MM-dd")
        ListarTipoMantenimientoOTxEquipoCombo()
        ListarPersonalResponsableComboMntOTxEquipos()
        ListarPersonalComboMntOTxEquipos()
        'Dim SolicitudServicio As LOGI_CABECERAORDENFABRICACION = OrdenFabricacionNeg.OrdenFabricacionListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Session("IdEmpresa"))
        'Dim EquipoNeg As New clsEquipoNegocios
        'Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        'Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(9).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
        'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = Now.ToString("yyyy-MM-dd")
        'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = Now.ToString("yyyy-MM-dd")
        'hfdIdCatalogoCabecera.Value = Equipo.cIdCatalogo
        'hfdIdTipoActivoCabecera.Value = Equipo.cIdTipoActivo
        'hfdIdJerarquiaCatalogoCabecera.Value = Equipo.cIdJerarquiaCatalogo
        'cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(cboTipoMantenimientoMantenimientoOrdenTrabajo, New System.EventArgs())

        'If MyValidator.ErrorMessage = "" Then
        '    MyValidator.ErrorMessage = "Registro encontrado con éxito"
        'End If
        'ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        'MyValidator.IsValid = False
        'MyValidator.ID = "ErrorPersonalizado"
        'MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        'Me.Page.Validators.Add(MyValidator)

    End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvTipoMantenimientoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalResponsableMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalAsignadoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvTipoControlTiempoOrdenTrabajoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        hfdCorreoElectronicoCliente.Value = ""
        hfdDNICliente.Value = ""
        hfdRUCCliente.Value = ""
        hfdIdClienteSAPEquipo.Value = ""
        hfdIdEquipoSAPEquipo.Value = ""
        hfdFechaCreacionEquipo.Value = ""
        hfdIdUsuarioCreacionEquipo.Value = ""
    End Sub

    Sub LimpiarObjetosOrdenTrabajo()
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = ""
        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = ""
        cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
        VaciarCestaPersonal(Session("CestaSSPersonal"))
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub LimpiarObjetosMntOTxEquipo()
        txtFechaInicioPlanificadaMntOTxEquipos.Text = ""
        txtFechaTerminoPlanificadaMntOTxEquipos.Text = ""
        cboTipoMantenimientoMntOTxEquipos.SelectedIndex = -1
        cboPersonalResponsableMntOTxEquipos.SelectedIndex = -1
        cboPersonalAsignadoMntOTxEquipos.SelectedIndex = -1
        txtCantidadEquipos.Text = ""
        VaciarCestaPersonal(Session("CestaSSPersonal"))
        grdPersonalAsignadoMntOTxEquipos.DataSource = Session("CestaSSPersonal")
        grdPersonalAsignadoMntOTxEquipos.DataBind()
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
            strOpcionModulo = "070" 'Mantenimiento de las Solicitudes de Servicios.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroEquipo.SelectedIndex = 5
            ListarTipoMantenimientoCombo()
            ListarPlantillaCheckListCombo()
            ListarPersonalResponsableCombo()
            ListarPersonalCombo()

            If Session("CestaSSPersonal") Is Nothing Then
                Session("CestaSSPersonal") = CrearCestaPersonal()
            Else
                VaciarCestaPersonal(Session("CestaSSPersonal"))
            End If

            BloquearMantenimiento(True, False, True, False)

            OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'R' WHERE cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion = '" & Session("IdUsuario") & "'")
            Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("DOC.cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND (vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
            Me.grdLista.DataBind()
        Else
            txtBuscarEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0675", strOpcionModulo, "CMMS")

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            Dim EquipoNeg As New clsEquipoNegocios
                            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
                            Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(8).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
                            'If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & (Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim) & "'").Rows(0).Item(0) = "0" Then
                            '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                            'End If
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "N"
            BloquearMantenimiento(True, False, False, False)
            LimpiarObjetosOrdenTrabajo()
            ValidarTexto(True)
            ActivarObjetos(True)
            LlenarData()

            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
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

    Protected Sub btnAsignarEquipos_Click(sender As Object, e As EventArgs) Handles btnAsignarEquipos.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0675", strOpcionModulo, "CMMS")

            'If grdLista IsNot Nothing Then
            '    If grdLista.Rows.Count > 0 Then
            '        If IsNothing(grdLista.SelectedRow) = False Then
            '            If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            '                Dim EquipoNeg As New clsEquipoNegocios
            '                Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            '                Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(8).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
            '                'If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & (Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim) & "'").Rows(0).Item(0) = "0" Then
            '                '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
            '                'End If
            '            End If
            '        Else
            '            Throw New Exception("Debe de seleccionar un item.")
            '        End If
            '    End If
            'End If
            'hfdOperacion.Value = "N"
            'BloquearMantenimiento(True, False, False, False)
            'LimpiarObjetosOrdenTrabajo()
            'ValidarTexto(True)
            'ActivarObjetos(True)
            LimpiarObjetosMntOTxEquipo()
            LlenarDataOTxEquipo()

            lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
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
    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            hfdIdEquipoSAPEquipo.Value = Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text) 'JMUG: 21/09/2023
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(16).Text) = "True", "1", "0") 'JMUG: 21/09/2023
                            lnkbtnVerOrdenServicio.Attributes.Add("onclick", "javascript:popupEmitirOrdenServicioReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text) & "');")
                            lnkbtnVerDetalleOrdenTrabajoServicio.Attributes.Add("onclick", "javascript:popupEmitirDetalleOrdenTrabajoServicioReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text) & "');")
                            If MyValidator.ErrorMessage = "" Then
                                MyValidator.ErrorMessage = "Registro encontrado con éxito"
                            End If
                        End If
                    End If
                Else
                    'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = False 'IdNumeroSerie
            e.Row.Cells(2).Visible = False 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'FechaEmision
            e.Row.Cells(4).Visible = False 'IdCliente
            e.Row.Cells(5).Visible = False 'IdClienteSAP
            e.Row.Cells(6).Visible = True 'RucCliente
            e.Row.Cells(7).Visible = True 'RazonSocial
            e.Row.Cells(8).Visible = True 'IdEquipo
            e.Row.Cells(9).Visible = False 'IdEquipoSAP
            e.Row.Cells(10).Visible = False 'IdUnidadTrabajo
            e.Row.Cells(11).Visible = True 'OrdenVenta / OrdenCompra / OrdenFabricacion
            e.Row.Cells(12).Visible = True 'DescripcionEquipo
            e.Row.Cells(13).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(14).Visible = False 'IdArticuloSAPCabecera
            e.Row.Cells(15).Visible = True 'StatusOrdenFabricacion
            e.Row.Cells(16).Visible = False 'Estado
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = False 'IdNumeroSerie
            e.Row.Cells(2).Visible = False 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'FechaEmision
            e.Row.Cells(4).Visible = False 'IdCliente
            e.Row.Cells(5).Visible = False 'IdClienteSAP
            e.Row.Cells(6).Visible = True 'RucCliente
            e.Row.Cells(7).Visible = True 'RazonSocial
            e.Row.Cells(8).Visible = True 'IdEquipo
            e.Row.Cells(9).Visible = False 'IdEquipoSAP
            e.Row.Cells(10).Visible = False 'IdUnidadTrabajo
            e.Row.Cells(11).Visible = True 'OrdenVenta / OrdenCompra / OrdenFabricacion
            e.Row.Cells(12).Visible = True 'DescripcionEquipo
            e.Row.Cells(13).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(14).Visible = False 'IdArticuloSAPCabecera
            e.Row.Cells(15).Visible = True 'StatusOrdenFabricacion
            e.Row.Cells(16).Visible = False 'Estado
        End If
    End Sub

    Private Sub imgbtnBuscarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarEquipo.Click
        Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("DOC.cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        Me.grdLista.DataBind()
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Equipo As New LOGI_EQUIPO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                Equipo.cIdEquipo = Valores(0).ToString

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
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

                Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("DOC.cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataBind()
            ElseIf e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Equipo As New LOGI_EQUIPO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                Equipo.cIdEquipo = Valores(0).ToString

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 1 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 1 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
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

                Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("DOC.cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataBind()
            ElseIf e.CommandName = "VerEquipo" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                Response.Redirect("~/frmLogiGenerarEquipo.aspx?IdEquipo=" & Valores(0).ToString)
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

    Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("DOC.cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Left 'IdTipoDocumento
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'IdNumeroSerie
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'IdNumeroCorrelativo
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'FechaEmision
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdCliente
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Center 'IdClienteSAP
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left 'RucCliente
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'RazonSocial
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'IdEquipo
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'IdEquipoSAP
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'IdUnidadTrabajo
                e.Row.Cells.Item(11).HorizontalAlign = HorizontalAlign.Left 'OrdenVenta / OrdenCompra / OrdenFabricacion
                e.Row.Cells.Item(12).HorizontalAlign = HorizontalAlign.Left 'DescripcionEquipo
                e.Row.Cells.Item(13).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                e.Row.Cells.Item(14).HorizontalAlign = HorizontalAlign.Left 'IdArticuloSAPCabecera
                e.Row.Cells.Item(15).HorizontalAlign = HorizontalAlign.Left 'StatusOrdenFabricacion
                e.Row.Cells.Item(16).HorizontalAlign = HorizontalAlign.Left 'Estado
            Next
        End If
    End Sub

    Private Sub lnkbtnVerOrdenServicio_Click(sender As Object, e As EventArgs) Handles lnkbtnVerOrdenServicio.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            hfdIdEquipoSAPEquipo.Value = grdLista.SelectedRow.Cells(9).Text 'JMUG: 21/09/2023
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(16).Text) = "True", "1", "0") 'JMUG: 21/09/2023
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para ver la tarjeta de equipo.")
                    End If
                Else
                    Throw New Exception("Seleccione un equipo.")
                End If
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

    Private Sub lnkbtnVerDetalleOrdenTrabajoServicio_Click(sender As Object, e As EventArgs) Handles lnkbtnVerDetalleOrdenTrabajoServicio.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            hfdIdEquipoSAPEquipo.Value = grdLista.SelectedRow.Cells(9).Text 'JMUG: 21/09/2023
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(16).Text) = "True", "1", "0") 'JMUG: 21/09/2023
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para ver la tarjeta de equipo.")
                    End If
                Else
                    Throw New Exception("Seleccione un equipo.")
                End If
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

    Private Sub btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count > 0 Then
                If grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex < grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count Then
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                        End If
                    Next
                End If
            End If

            If cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal.")
                Exit Sub
            Else
                AgregarCestaPersonal(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, False, Session("CestaSSPersonal"))
            End If
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
            grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
            MyValidator.ErrorMessage = "Personal agregado con éxito."
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0676", strOpcionModulo, "CMMS")

            If txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de inicio de planificación del mantenimiento.")
            ElseIf txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de termino de planificación del mantenimiento.")
            ElseIf cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            ElseIf cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = "" Then
                Throw New Exception("Debe de seleccionar una plantilla.")
            ElseIf cboTipoControlTiempoOrdenTrabajoMantenimientoOrdenTrabajo.SelectedValue = "" Then
                Throw New Exception("Debe de seleccionar un tipo de control de tiempo de orden de trabajo.")
            End If

            Dim bTieneResponsable As Boolean = False
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Responsable")) = True Then
                    bTieneResponsable = True
                End If
            Next
            If bTieneResponsable = False Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            End If

            OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoSAPCabeceraOrdenFabricacion = '" & hfdIdEquipoSAPEquipo.Value & "' AND vIdArticuloSAPCabeceraOrdenFabricacion = '" & hfdIdArticuloSAPPrincipal.Value & "'")
            Dim OrdTra As New LOGI_CABECERAORDENTRABAJO
            OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = "OT"
            OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = Year(Now).ToString
            OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ""
            OrdTra.dFechaTransaccionCabeceraOrdenTrabajo = Now
            OrdTra.dFechaEmisionCabeceraOrdenTrabajo = Now
            OrdTra.cIdClienteCabeceraOrdenTrabajo = Server.HtmlDecode(grdLista.SelectedRow.Cells(4).Text).Trim
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(9).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
            OrdTra.cIdEquipoCabeceraOrdenTrabajo = Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim
            OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
            OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
            OrdTra.cIdEmpresa = Session("IdEmpresa")
            OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
            OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
            OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = grdLista.SelectedRow.Cells(14).Text 'JMUG: 21/09/2023
            OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
            OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
            OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
            OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
            OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
            OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = ""
            OrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo = Session("IdUsuario")
            OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo = cboTipoControlTiempoOrdenTrabajoMantenimientoOrdenTrabajo.SelectedValue

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            LogAuditoria.cIdSistema = Session("IdSistema")
            LogAuditoria.cIdModulo = strOpcionModulo

            Dim ColeccionDetalleOrdTra As New List(Of LOGI_DETALLEORDENTRABAJO)

            Dim UsuCorreo As New GNRL_USUARIO

            Dim ColeccionRRHH As New List(Of LOGI_RECURSOSORDENTRABAJO)
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                Dim RrhhOrdTra As New LOGI_RECURSOSORDENTRABAJO
                With RrhhOrdTra
                    .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                    .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                    .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                    .cIdEmpresa = OrdTra.cIdEmpresa
                    .cIdPersonal = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim)
                    .vObservacionRecursosOrdenTrabajo = ""
                    .bEstadoRegistroRecursosOrdenTrabajo = True
                    .cIdEquipoCabeceraOrdenTrabajo = OrdTra.cIdEquipoCabeceraOrdenTrabajo
                    .nTotalMinutosTrabajadosRecursosOrdenTrabajo = 0
                    .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                    .bResponsableRecursosOrdenTrabajo = (Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim)

                    If .bResponsableRecursosOrdenTrabajo = True Then
                        Dim dsUsuario = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT * FROM GNRL_USUARIO WHERE vIdNroDocumentoIdentidadUsuario IN (SELECT vNumeroDocumentoPersonal FROM RRHH_PERSONAL WHERE cIdPersonal = '" & .cIdPersonal & "') AND cIdEmpresa = '" & Session("IdEmpresa") & "'")
                        For Each fila In dsUsuario.Rows
                            With UsuCorreo
                                UsuCorreo.cIdUsuario = fila("cIdUsuario")
                                UsuCorreo.vLoginUsuario = fila("vLoginUsuario")
                                UsuCorreo.vPasswordUsuario = fila("vPasswordUsuario")
                            End With
                        Next
                        If UsuCorreo.vLoginUsuario Is Nothing Then
                            Throw New Exception("El responsable debe de tener un DNI asignado como Usuario.")
                        End If
                    End If
                End With
                ColeccionRRHH.Add(RrhhOrdTra)
            Next

            Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
            Dim cIdPlantilla = String.Concat(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue.Trim(), cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue)

            Dim result = PlantillaCheckListNeg.ChecklistPlantillaInsertaCopiaComponentes(Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim, cIdPlantilla)


            Dim dsCheckListPlantilla = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT CABCHKLISPLA.cIdTipoMantenimiento, CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, CABCHKLISPLA.dFechaTransaccionCabeceraCheckListPlantilla, " &
                                                                                   "       CABCHKLISPLA.bEstadoRegistroCabeceraCheckListPlantilla, CABCHKLISPLA.cIdTipoActivoCabeceraCheckListPlantilla, CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla, " &
                                                                                   "       CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, DETCHKLISPLA.cIdActividadCheckList, DETCHKLISPLA.vDescripcionDetalleCheckListPlantilla, " &
                                                                                   "       DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla, DETCHKLISPLA.cIdTipoActivo, DETCHKLISPLA.cIdCatalogo, DETCHKLISPLA.cIdJerarquiaCatalogo, EQU.cIdEquipo " &
                                                                                   "FROM LOGI_CABECERACHECKLISTPLANTILLA AS CABCHKLISPLA INNER JOIN LOGI_DETALLECHECKLISTPLANTILLA AS DETCHKLISPLA ON " &
                                                                                   "     CABCHKLISPLA.cIdTipoMantenimiento = DETCHKLISPLA.cIdTipoMantenimiento AND " &
                                                                                   "     CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla " &
                                                                                   "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                                                   "     DETCHKLISPLA.cIdCatalogo = EQU.cIdCatalogo AND " &
                                                                                   "     DETCHKLISPLA.cIdJerarquiaCatalogo = EQU.cIdJerarquiaCatalogo AND " &
                                                                                   "     EQU.cIdEnlaceEquipo = '" & Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim & "' AND " &
                                                                                   "     ISNULL(EQU.bEstadoRegistroEquipo, '1') = '1' " &
                                                                                   "WHERE CABCHKLISPLA.cIdTipoMantenimiento = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "' AND CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "' " &
                                                                                   "      AND CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla = '" & hfdIdCatalogoCabecera.Value & "' AND CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0' " &
                                                                                   "      AND DETCHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '1' " &
                                                                                   "ORDER BY DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla")

            Dim ColeccionCheckList As New List(Of LOGI_CHECKLISTORDENTRABAJO)
            For Each CheckListPlantilla In dsCheckListPlantilla.Rows
                Dim ChkList As New LOGI_CHECKLISTORDENTRABAJO
                With ChkList
                    .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                    .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                    .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                    .vObservacionCheckListOrdenTrabajo = ""
                    .cIdEmpresa = OrdTra.cIdEmpresa
                    .cEstadoCheckListOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                    .nIdNumeroItemCheckListOrdenTrabajo = CheckListPlantilla("nIdNumeroItemDetalleCheckListPlantilla")
                    .nTotalSegundosTrabajadosCheckListOrdenTrabajo = 0
                    .dFechaInicioCheckListOrdenTrabajo = Nothing
                    .dFechaFinalCheckListOrdenTrabajo = Nothing
                    .cIdEquipoCabeceraOrdenTrabajo = (Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
                    .cIdEquipoCheckListOrdenTrabajo = CheckListPlantilla("cIdEquipo")
                    .cIdActividadCheckListOrdenTrabajo = CheckListPlantilla("cIdActividadCheckList")
                    .cIdTipoMantenimientoCheckListOrdenTrabajo = CheckListPlantilla("cIdTipoMantenimiento")
                    .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")
                    .cIdCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdCatalogo")
                    .cIdJerarquiaCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdJerarquiaCatalogo")
                    .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                    .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue
                End With

                OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")

                ColeccionCheckList.Add(ChkList)
            Next
            Dim dsComponenteEquipo = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdJerarquiaCatalogo " &
                                                                                 "FROM LOGI_EQUIPO AS EQU " &
                                                                                 "WHERE EQU.cIdEnlaceEquipo = '" & Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim & "' " &
                                                                                 "      AND EQU.cIdJerarquiaCatalogo = '1' " &
                                                                                 "      AND EQU.bEstadoRegistroEquipo = '1' " &
                                                                                 "      AND EQU.cIdEnlaceCatalogo = '" & hfdIdCatalogoCabecera.Value & "' " &
                                                                                 "ORDER BY EQU.vDescripcionEquipo")

            Dim ColeccionComponente As New List(Of LOGI_COMPONENTEORDENTRABAJO)
            For Each ComponenteEquipo In dsComponenteEquipo.Rows
                Dim ComEqu As New LOGI_COMPONENTEORDENTRABAJO
                With ComEqu
                    .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                    .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                    .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                    .cIdEmpresa = OrdTra.cIdEmpresa
                    .cIdEquipoCabeceraOrdenTrabajo = (Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
                    .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                    .cIdEquipoComponenteOrdenTrabajo = ComponenteEquipo("cIdEquipo")
                    .cIdCatalogoComponenteOrdenTrabajo = ComponenteEquipo("cIdCatalogo")
                    .cIdJerarquiaCatalogoComponenteOrdenTrabajo = ComponenteEquipo("cIdJerarquiaCatalogo")
                    .nIdNumeroItemComponenteOrdenTrabajo = 0
                    .vObservacionComponenteOrdenTrabajo = ""
                    .cEstadoComponenteOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                    .dFechaInicioActividadComponenteOrdenTrabajo = Nothing
                    .dFechaFinalActividadComponenteOrdenTrabajo = Nothing
                    .nTotalSegundosTrabajadosComponenteOrdenTrabajo = 0
                End With
                ColeccionComponente.Add(ComEqu)
            Next

            Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios

            If OrdenTrabajoNeg.OrdenTrabajoInsertaDetalle(OrdTra, ColeccionDetalleOrdTra, ColeccionCheckList, ColeccionRRHH, ColeccionComponente) = 0 Then
                Dim dtCorreoResponsable = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT PER.cIdPersonal, PER.vEmailPersonal FROM LOGI_RECURSOSORDENTRABAJO AS REC " &
                                                    "INNER JOIN RRHH_PERSONAL AS PER ON " &
                                                    "REC.cIdPersonal = PER.cIdPersonal AND REC.cIdEmpresa = PER.cIdEmpresa " &
                                                    "WHERE REC.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND REC.vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND REC.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND REC.cIdEmpresa = '" & OrdTra.cIdEmpresa & "' " &
                                                    "ORDER BY REC.bResponsableRecursosOrdenTrabajo DESC")
                If dtCorreoResponsable.Rows.Count > 0 Then
                    If dtCorreoResponsable.Rows(0).Item(1).ToString() = "" Then
                        Throw New Exception("El responsable debe de tener un correo asignado.")
                    Else
                        For i = 0 To dtCorreoResponsable.Rows.Count - 1

                            Dim strHTML As String
                            Dim EmpNeg As New clsEmpresaNegocios
                            strHTML = FuncionesNeg.FormatoEnvioGeneral("001", OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "-" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "-" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "*" & String.Format("{0:dd-MM-yyyy}", OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo) & "*" & EmpNeg.EmpresaListarPorId(Session("IdEmpresa")).vDescripcionEmpresa & "*" & UsuCorreo.vLoginUsuario & "*" & UsuCorreo.vPasswordUsuario)
                            Dim strFrom As String = "notificaciones@movitecnica.com.pe"
                            Dim strPwd As String = "NTmvt$1604"
                            Dim strConfiguracionCorreo As String = "smtp.office365.com" & "|" & "587"
                            Dim strConfiguracionPeriodo As String = "" 'String.Format("{0:0000}", Year(Documento.dFechaEmisionCabeceraDocumento)) & "|" & String.Format("{0:00}", Month(Documento.dFechaEmisionCabeceraDocumento))
                            FuncionesNeg.EnviarCorreo(LCase(strFrom), strPwd, LCase(dtCorreoResponsable.Rows(i).Item(1).ToString()), "Orden de Trabajo Generada", strHTML, "", "", strConfiguracionCorreo, strConfiguracionPeriodo, "", False)

                        Next
                    End If
                Else
                    Throw New Exception("No tiene asignado a ningún responsable para esta orden de trabajo.")
                End If

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(1).Text & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(2).Text & "'")

                MyValidator.ErrorMessage = "Transacción registrada con éxito"
            End If

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Falta agregar procedure para crear varios equipos y revisar si se modifica el insert de ot por un foreach
    Private Sub btnAceptarMntOTxEquipos_Click(sender As Object, e As EventArgs) Handles btnAceptarMntOTxEquipos.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0676", strOpcionModulo, "CMMS")

            If txtFechaInicioPlanificadaMntOTxEquipos.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de inicio de planificación del mantenimiento.")
            ElseIf txtFechaTerminoPlanificadaMntOTxEquipos.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de termino de planificación del mantenimiento.")
            ElseIf cboPersonalResponsableMntOTxEquipos.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            ElseIf cboListadoCheckListMntOTxEquipos.SelectedValue = "" Then
                Throw New Exception("Debe de seleccionar una plantilla.")
            End If

            Dim bTieneResponsable As Boolean = False
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Responsable")) = True Then
                    bTieneResponsable = True
                End If
            Next
            If bTieneResponsable = False Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            End If

            OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoSAPCabeceraOrdenFabricacion = '" & hfdIdEquipoSAPEquipo.Value & "' AND vIdArticuloSAPCabeceraOrdenFabricacion = '" & hfdIdArticuloSAPPrincipal.Value & "'")
            Dim OrdTra As New LOGI_CABECERAORDENTRABAJO
            OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = "OT"
            OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = Year(Now).ToString
            OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ""
            OrdTra.dFechaTransaccionCabeceraOrdenTrabajo = Now
            OrdTra.dFechaEmisionCabeceraOrdenTrabajo = Now
            OrdTra.cIdClienteCabeceraOrdenTrabajo = Server.HtmlDecode(grdLista.SelectedRow.Cells(4).Text).Trim
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(9).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
            OrdTra.cIdEquipoCabeceraOrdenTrabajo = Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim
            OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
            OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
            OrdTra.cIdEmpresa = Session("IdEmpresa")
            OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
            OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
            OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = grdLista.SelectedRow.Cells(14).Text 'JMUG: 21/09/2023
            OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
            OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
            OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
            OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
            OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
            OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = ""
            OrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo = Session("IdUsuario")

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            LogAuditoria.cIdSistema = Session("IdSistema")
            LogAuditoria.cIdModulo = strOpcionModulo

            Dim ColeccionDetalleOrdTra As New List(Of LOGI_DETALLEORDENTRABAJO)

            Dim UsuCorreo As New GNRL_USUARIO

            Dim ColeccionRRHH As New List(Of LOGI_RECURSOSORDENTRABAJO)
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                Dim RrhhOrdTra As New LOGI_RECURSOSORDENTRABAJO
                With RrhhOrdTra
                    .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                    .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                    .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                    .cIdEmpresa = OrdTra.cIdEmpresa
                    .cIdPersonal = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim)
                    .vObservacionRecursosOrdenTrabajo = ""
                    .bEstadoRegistroRecursosOrdenTrabajo = True
                    .cIdEquipoCabeceraOrdenTrabajo = OrdTra.cIdEquipoCabeceraOrdenTrabajo
                    .nTotalMinutosTrabajadosRecursosOrdenTrabajo = 0
                    .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                    .bResponsableRecursosOrdenTrabajo = (Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim)

                    If .bResponsableRecursosOrdenTrabajo = True Then
                        Dim dsUsuario = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT * FROM GNRL_USUARIO WHERE vIdNroDocumentoIdentidadUsuario IN (SELECT vNumeroDocumentoPersonal FROM RRHH_PERSONAL WHERE cIdPersonal = '" & .cIdPersonal & "') AND cIdEmpresa = '" & Session("IdEmpresa") & "'")
                        For Each fila In dsUsuario.Rows
                            With UsuCorreo
                                UsuCorreo.cIdUsuario = fila("cIdUsuario")
                                UsuCorreo.vLoginUsuario = fila("vLoginUsuario")
                                UsuCorreo.vPasswordUsuario = fila("vPasswordUsuario")
                            End With
                        Next
                        If UsuCorreo.vLoginUsuario Is Nothing Then
                            Throw New Exception("El responsable debe de tener un DNI asignado como Usuario.")
                        End If
                    End If
                End With
                ColeccionRRHH.Add(RrhhOrdTra)
            Next

            Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
            Dim cIdPlantilla = String.Concat(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue.Trim(), cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue)

            Dim result = PlantillaCheckListNeg.ChecklistPlantillaInsertaCopiaComponentes(Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim, cIdPlantilla)


            Dim dsCheckListPlantilla = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT CABCHKLISPLA.cIdTipoMantenimiento, CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, CABCHKLISPLA.dFechaTransaccionCabeceraCheckListPlantilla, " &
                                                                                   "       CABCHKLISPLA.bEstadoRegistroCabeceraCheckListPlantilla, CABCHKLISPLA.cIdTipoActivoCabeceraCheckListPlantilla, CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla, " &
                                                                                   "       CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, DETCHKLISPLA.cIdActividadCheckList, DETCHKLISPLA.vDescripcionDetalleCheckListPlantilla, " &
                                                                                   "       DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla, DETCHKLISPLA.cIdTipoActivo, DETCHKLISPLA.cIdCatalogo, DETCHKLISPLA.cIdJerarquiaCatalogo, EQU.cIdEquipo " &
                                                                                   "FROM LOGI_CABECERACHECKLISTPLANTILLA AS CABCHKLISPLA INNER JOIN LOGI_DETALLECHECKLISTPLANTILLA AS DETCHKLISPLA ON " &
                                                                                   "     CABCHKLISPLA.cIdTipoMantenimiento = DETCHKLISPLA.cIdTipoMantenimiento AND " &
                                                                                   "     CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla " &
                                                                                   "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                                                   "     DETCHKLISPLA.cIdCatalogo = EQU.cIdCatalogo AND " &
                                                                                   "     DETCHKLISPLA.cIdJerarquiaCatalogo = EQU.cIdJerarquiaCatalogo AND " &
                                                                                   "     EQU.cIdEnlaceEquipo = '" & Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim & "' AND " &
                                                                                   "     ISNULL(EQU.bEstadoRegistroEquipo, '1') = '1' " &
                                                                                   "WHERE CABCHKLISPLA.cIdTipoMantenimiento = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "' AND CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "' " &
                                                                                   "      AND CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla = '" & hfdIdCatalogoCabecera.Value & "' AND CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0' " &
                                                                                   "      AND DETCHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '1' " &
                                                                                   "ORDER BY DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla")

            Dim ColeccionCheckList As New List(Of LOGI_CHECKLISTORDENTRABAJO)
            For Each CheckListPlantilla In dsCheckListPlantilla.Rows
                Dim ChkList As New LOGI_CHECKLISTORDENTRABAJO
                With ChkList
                    .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                    .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                    .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                    .vObservacionCheckListOrdenTrabajo = ""
                    .cIdEmpresa = OrdTra.cIdEmpresa
                    .cEstadoCheckListOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                    .nIdNumeroItemCheckListOrdenTrabajo = CheckListPlantilla("nIdNumeroItemDetalleCheckListPlantilla")
                    .nTotalSegundosTrabajadosCheckListOrdenTrabajo = 0
                    .dFechaInicioCheckListOrdenTrabajo = Nothing
                    .dFechaFinalCheckListOrdenTrabajo = Nothing
                    .cIdEquipoCabeceraOrdenTrabajo = (Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
                    .cIdEquipoCheckListOrdenTrabajo = CheckListPlantilla("cIdEquipo")
                    .cIdActividadCheckListOrdenTrabajo = CheckListPlantilla("cIdActividadCheckList")
                    .cIdTipoMantenimientoCheckListOrdenTrabajo = CheckListPlantilla("cIdTipoMantenimiento")
                    .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")
                    .cIdCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdCatalogo")
                    .cIdJerarquiaCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdJerarquiaCatalogo")
                    .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                    .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue
                End With

                OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")

                ColeccionCheckList.Add(ChkList)
            Next
            Dim dsComponenteEquipo = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdJerarquiaCatalogo " &
                                                                                 "FROM LOGI_EQUIPO AS EQU " &
                                                                                 "WHERE EQU.cIdEnlaceEquipo = '" & Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim & "' " &
                                                                                 "      AND EQU.cIdJerarquiaCatalogo = '1' " &
                                                                                 "      AND EQU.bEstadoRegistroEquipo = '1' " &
                                                                                 "      AND EQU.cIdEnlaceCatalogo = '" & hfdIdCatalogoCabecera.Value & "' " &
                                                                                 "ORDER BY EQU.vDescripcionEquipo")

            Dim ColeccionComponente As New List(Of LOGI_COMPONENTEORDENTRABAJO)
            For Each ComponenteEquipo In dsComponenteEquipo.Rows
                Dim ComEqu As New LOGI_COMPONENTEORDENTRABAJO
                With ComEqu
                    .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                    .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                    .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                    .cIdEmpresa = OrdTra.cIdEmpresa
                    .cIdEquipoCabeceraOrdenTrabajo = (Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
                    .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                    .cIdEquipoComponenteOrdenTrabajo = ComponenteEquipo("cIdEquipo")
                    .cIdCatalogoComponenteOrdenTrabajo = ComponenteEquipo("cIdCatalogo")
                    .cIdJerarquiaCatalogoComponenteOrdenTrabajo = ComponenteEquipo("cIdJerarquiaCatalogo")
                    .nIdNumeroItemComponenteOrdenTrabajo = 0
                    .vObservacionComponenteOrdenTrabajo = ""
                    .cEstadoComponenteOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                    .dFechaInicioActividadComponenteOrdenTrabajo = Nothing
                    .dFechaFinalActividadComponenteOrdenTrabajo = Nothing
                    .nTotalSegundosTrabajadosComponenteOrdenTrabajo = 0
                End With
                ColeccionComponente.Add(ComEqu)
            Next

            Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios

            If OrdenTrabajoNeg.OrdenTrabajoInsertaDetalle(OrdTra, ColeccionDetalleOrdTra, ColeccionCheckList, ColeccionRRHH, ColeccionComponente) = 0 Then
                Dim dtCorreoResponsable = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT PER.cIdPersonal, PER.vEmailPersonal FROM LOGI_RECURSOSORDENTRABAJO AS REC " &
                                                    "INNER JOIN RRHH_PERSONAL AS PER ON " &
                                                    "REC.cIdPersonal = PER.cIdPersonal AND REC.cIdEmpresa = PER.cIdEmpresa " &
                                                    "WHERE REC.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND REC.vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND REC.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND REC.cIdEmpresa = '" & OrdTra.cIdEmpresa & "' " &
                                                    "ORDER BY REC.bResponsableRecursosOrdenTrabajo DESC")
                If dtCorreoResponsable.Rows.Count > 0 Then
                    If dtCorreoResponsable.Rows(0).Item(1).ToString() = "" Then
                        Throw New Exception("El responsable debe de tener un correo asignado.")
                    Else
                        For i = 0 To dtCorreoResponsable.Rows.Count - 1

                            Dim strHTML As String
                            Dim EmpNeg As New clsEmpresaNegocios
                            strHTML = FuncionesNeg.FormatoEnvioGeneral("001", OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "-" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "-" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "*" & String.Format("{0:dd-MM-yyyy}", OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo) & "*" & EmpNeg.EmpresaListarPorId(Session("IdEmpresa")).vDescripcionEmpresa & "*" & UsuCorreo.vLoginUsuario & "*" & UsuCorreo.vPasswordUsuario)
                            Dim strFrom As String = "notificaciones@movitecnica.com.pe"
                            Dim strPwd As String = "NTmvt$1604"
                            Dim strConfiguracionCorreo As String = "smtp.office365.com" & "|" & "587"
                            Dim strConfiguracionPeriodo As String = "" 'String.Format("{0:0000}", Year(Documento.dFechaEmisionCabeceraDocumento)) & "|" & String.Format("{0:00}", Month(Documento.dFechaEmisionCabeceraDocumento))
                            FuncionesNeg.EnviarCorreo(LCase(strFrom), strPwd, LCase(dtCorreoResponsable.Rows(i).Item(1).ToString()), "Orden de Trabajo Generada", strHTML, "", "", strConfiguracionCorreo, strConfiguracionPeriodo, "", False)

                        Next
                    End If
                Else
                    Throw New Exception("No tiene asignado a ningún responsable para esta orden de trabajo.")
                End If

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(1).Text & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(2).Text & "'")

                MyValidator.ErrorMessage = "Transacción registrada con éxito"
            End If

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarPersonalAsignadoMntOTxEquipos_Click(sender As Object, e As EventArgs) Handles btnAdicionarPersonalAsignadoMntOTxEquipos.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If grdPersonalAsignadoMntOTxEquipos.Rows.Count > 0 Then
                If grdPersonalAsignadoMntOTxEquipos.SelectedIndex < grdPersonalAsignadoMntOTxEquipos.Rows.Count Then
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalAsignadoMntOTxEquipos.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                        End If
                    Next
                End If
            End If

            If cboPersonalAsignadoMntOTxEquipos.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal.")
                Exit Sub
            Else
                AgregarCestaPersonal(cboPersonalAsignadoMntOTxEquipos.SelectedValue.Trim, UCase(cboPersonalAsignadoMntOTxEquipos.SelectedItem.Text).Trim, False, Session("CestaSSPersonal"))
            End If
            Me.grdPersonalAsignadoMntOTxEquipos.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMntOTxEquipos.DataBind()
            cboPersonalAsignadoMntOTxEquipos.SelectedIndex = -1
            grdPersonalAsignadoMntOTxEquipos.SelectedIndex = -1
            MyValidator.ErrorMessage = "Personal agregado con éxito."
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub cboPersonalResponsableMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndexChanged
        Try
            If cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            End If
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                Session("CestaSSPersonal").Rows(i)("Responsable") = False
            Next
            If grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count > 0 Then
                If grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex < grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count Then
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue.ToString.Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                        End If
                    Next
                End If
            End If
            AgregarCestaPersonal(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, True, Session("CestaSSPersonal"))
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = "Personal agregado satisfactoriamente."
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMantenimientoOrdenTrabajo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPersonalAsignadoMantenimientoOrdenTrabajo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                    QuitarCestaPersonal(i, Session("CestaSSPersonal"))
                    Exit For
                End If
            Next
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMntOTxEquipos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPersonalAsignadoMntOTxEquipos.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMntOTxEquipos.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                    QuitarCestaPersonal(i, Session("CestaSSPersonal"))
                    Exit For
                End If
            Next
            Me.grdPersonalAsignadoMntOTxEquipos.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMntOTxEquipos.DataBind()
            lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary3.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub cboPersonalResponsableMntOTxEquipos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalResponsableMntOTxEquipos.SelectedIndexChanged
        Try
            If cboPersonalResponsableMntOTxEquipos.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            End If
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                Session("CestaSSPersonal").Rows(i)("Responsable") = False
            Next
            If grdPersonalAsignadoMntOTxEquipos.Rows.Count > 0 Then
                If grdPersonalAsignadoMntOTxEquipos.SelectedIndex < grdPersonalAsignadoMntOTxEquipos.Rows.Count Then
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalResponsableMntOTxEquipos.SelectedValue.ToString.Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                        End If
                    Next
                End If
            End If
            AgregarCestaPersonal(cboPersonalResponsableMntOTxEquipos.SelectedValue.Trim, UCase(cboPersonalResponsableMntOTxEquipos.SelectedItem.Text).Trim, True, Session("CestaSSPersonal"))
            grdPersonalAsignadoMntOTxEquipos.DataSource = Session("CestaSSPersonal")
            grdPersonalAsignadoMntOTxEquipos.DataBind()
            lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = "Personal agregado satisfactoriamente."
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndexChanged
        ListarPlantillaCheckListCombo()
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboTipoMantenimientoMntOTxEquipos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoMantenimientoMntOTxEquipos.SelectedIndexChanged
        ListarPlantillaCheckListMntOTxEquipoCombo()
        lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
    End Sub

    Private Sub cboPersonalAsignadoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboListadoCheckListMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboListadoCheckListMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboTipoActivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivo.SelectedIndexChanged
        ListarCatalogoCombo()
        lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()

    End Sub

    Private Sub cboCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCatalogo.SelectedIndexChanged
        lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
    End Sub

    Private Sub cboTipoEquipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoEquipo.SelectedIndexChanged
        lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
    End Sub

    Private Sub cboPersonalAsignadoMntOTxEquipos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalAsignadoMntOTxEquipos.SelectedIndexChanged
        lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender.Show()
    End Sub
End Class