Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarConfiguracionContrato
    Inherits System.Web.UI.Page
    Dim ContratoNeg As New clsContratoNegocios
    Dim OTNeg As New clsOrdenTrabajoNegocios
    Dim ClienteNeg As New clsClienteNegocios
    Dim EquipoNeg As New clsEquipoNegocios
    Dim DetalleContratoNeg As New clsDetalleContratoNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Dim MyValidator As New CustomValidator
    Shared intIndexContratoEquipo As Integer

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

    Shared Function CrearCestaEquipo(ByVal NroDiasMes As Integer) As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Seleccionar", GetType(System.Boolean))) '1
        dt.Columns.Add(New DataColumn("Item", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("DescripcionEquipo", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '4
        'For i = 1 To NroDiasMes
        '    dt.Columns.Add(New DataColumn("D" & i, GetType(System.String))) '12
        'Next
        Return dt
    End Function

    Shared Sub EditarCestaEquipo(ByVal Seleccionar As Boolean, ByVal CodigoEquipo As String, ByVal DescripcionEquipo As String,
                             ByVal Estado As Boolean, 'ByVal FechaActividad As DateTime,
                             ByVal NombreColumna As String, ByVal IdTipoMantenimiento As String, 'ByVal NroDiasMes As Integer,
                             ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)("Seleccionar") = Seleccionar
                Tabla.Rows(Fila)("IdEquipo") = CodigoEquipo
                Tabla.Rows(Fila)("DescripcionEquipo") = DescripcionEquipo
                Tabla.Rows(Fila)("Estado") = Estado
                'If NombreColumna <> "" Then
                '    Tabla.Rows(Fila)(NombreColumna) = IdTipoMantenimiento
                'End If
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaEquipo(ByVal Seleccionar As Boolean, ByVal Item As String, ByVal CodigoEquipo As String, ByVal DescripcionEquipo As String,
                               ByVal Estado As Boolean, ByVal IdTipoMantenimiento As String,
                               ByVal Tabla As DataTable)
        'Shared Sub AgregarCestaEquipo(ByVal Seleccionar As Boolean, ByVal Item As String, ByVal CodigoEquipo As String, ByVal DescripcionEquipo As String,
        '                               ByVal Estado As Boolean, ByVal NombreColumna As String, ByVal IdTipoMantenimiento As String,
        '                               ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Seleccionar") = Seleccionar
        Fila("Item") = Item
        Fila("IdEquipo") = CodigoEquipo
        Fila("DescripcionEquipo") = DescripcionEquipo
        Fila("Estado") = Estado
        'Dim Columnas As String() = NombreColumna.Split("|")
        'Dim DatosColumnas As String() = IdTipoMantenimiento.Split("*")
        'For i = 0 To Columnas.Length - 1
        '    If DatosColumnas(i) <> "" Then
        '        Fila(Columnas(i)) = DatosColumnas(i)
        '    End If
        'Next
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaEquipo(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaEquipo(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

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

    Shared Function CrearCestaPersonalOrdenTrabajo() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("FechaPlanificacion", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdContrato", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("Responsable", GetType(System.Boolean))) '4
        dt.Columns.Add(New DataColumn("IdPersonal", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("Personal", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdOrdenTrabajo", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("IdOrdenTrabajoCliente", GetType(System.String))) '8
        Return dt
    End Function

    Shared Sub EditarCestaPersonalOrdenTrabajo(ByVal FechaPlanificacion As String, ByVal IdEquipo As String, ByVal IdContrato As String,
                                           ByVal Responsable As Boolean, ByVal IdPersonal As String, ByVal Personal As String,
                                           ByVal IdOrdenTrabajo As String, ByVal IdOrdenTrabajoCliente As String,
                                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)("FechaPlanificacion") = FechaPlanificacion
                Tabla.Rows(Fila)("IdEquipo") = IdEquipo
                Tabla.Rows(Fila)("IdContrato") = IdContrato
                Tabla.Rows(Fila)("Responsable") = Responsable
                Tabla.Rows(Fila)("IdPersonal") = IdPersonal
                Tabla.Rows(Fila)("Personal") = Personal
                Tabla.Rows(Fila)("IdOrdenTrabajo") = IdOrdenTrabajo
                Tabla.Rows(Fila)("IdOrdenTrabajoCliente") = IdOrdenTrabajoCliente
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaPersonalOrdenTrabajo(ByVal FechaPlanificacion As String, ByVal IdEquipo As String, ByVal IdContrato As String,
                                            ByVal Responsable As Boolean, ByVal IdPersonal As String, ByVal Personal As String,
                                            ByVal IdOrdenTrabajo As String, ByVal IdOrdenTrabajoCliente As String,
                                            ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("FechaPlanificacion") = FechaPlanificacion
        Fila("IdEquipo") = IdEquipo
        Fila("IdContrato") = IdContrato
        Fila("Responsable") = Responsable
        Fila("IdPersonal") = IdPersonal
        Fila("Personal") = Personal
        Fila("IdOrdenTrabajo") = IdOrdenTrabajo
        Fila("IdOrdenTrabajoCliente") = IdOrdenTrabajoCliente
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaPersonalOrdenTrabajo(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaPersonalOrdenTrabajo(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub CargarCestaEquiposImprimir()
        'Carga las opciones en la Grilla de todos los Equipos asignados a este contrato.
        Try
            VaciarCestaEquipo(Session("CestaEquipoContratoImprimir"))
            Dim dsEquipos = ContratoNeg.ContratoGetData("SELECT CON.cIdTipoDocumentoCabeceraContrato, CON.vIdNumeroSerieCabeceraContrato, CON.vIdNumeroCorrelativoCabeceraContrato, CON.cIdEmpresa, " &
                                                    "       CON.dFechaEmisionCabeceraContrato, CON.cIdCliente, CON.cEstadoCabeceraContrato, CON.vDescripcionCabeceraContrato, " &
                                                    "       CON.vNroLicitacionCabeceraContrato, DETCON.cIdEquipoDetalleContrato, DETCON.vDescripcionDetalleContrato, " &
                                                    "       DETCON.bEstadoRegistroDetalleContrato, DETCON.nIdNumeroItemDetalleContrato " &
                                                    "FROM LOGI_CABECERACONTRATO AS CON " &
                                                    "     INNER JOIN LOGI_DETALLECONTRATO AS DETCON ON " &
                                                    "     CON.cIdEmpresa = DETCON.cIdEmpresa AND " &
                                                    "     CON.cIdTipoDocumentoCabeceraContrato = DETCON.cIdTipoDocumentoCabeceraContrato AND " &
                                                    "     CON.vIdNumeroSerieCabeceraContrato = DETCON.vIdNumeroSerieCabeceraContrato AND " &
                                                    "     CON.vIdNumeroCorrelativoCabeceraContrato = DETCON.vIdNumeroCorrelativoCabeceraContrato " &
                                                    "WHERE CON.cIdEmpresa = '" & Session("IdEmpresa") & "' AND " &
                                                    "      CON.cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND " &
                                                    "      CON.vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND " &
                                                    "      CON.vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "'")

            For Each ListaEquipos In dsEquipos.Rows
                'AgregarCestaEquipo(False, ListaEquipos("nIdNumeroItemDetalleContrato"), ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), "", "", Session("CestaEquipoContratoImprimir"))
                AgregarCestaEquipo(False, ListaEquipos("nIdNumeroItemDetalleContrato"), ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), "", Session("CestaEquipoContratoImprimir"))
            Next

            'Me.grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            'Me.grdDetalleEquipoImprimirProgramacion.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub



    'Sub ListarFiltroBusquedaCombo()
    '    ''Hay que hacer referencia a la Capa de Datos
    '    ''porque se encuentran las entidades en dicha capa.
    '    'Dim FiltroNeg As New clsTablaSistemaNegocios
    '    'cboFiltroContrato.DataTextField = "vDescripcionTablaSistema"
    '    'cboFiltroContrato.DataValueField = "vValor"
    '    'cboFiltroContrato.DataSource = FiltroNeg.TablaSistemaListarCombo("88", "LOGI", Session("IdEmpresa"))
    '    'cboFiltroContrato.Items.Clear()
    '    'cboFiltroContrato.DataBind()
    'End Sub

    Sub ListarFiltroBusquedaCombo()

        Dim ContratoNeg As New clsContratoNegocios
        cboFiltroContrato.DataTextField = "vDescripcionCabeceraContrato"
        cboFiltroContrato.DataValueField = "vIdNumeroCorrelativoCabeceraContrato"
        cboFiltroContrato.DataSource = ContratoNeg.ContratoListarCombo("1", "'R','P'")
        cboFiltroContrato.DataBind()
    End Sub
    Sub ListarTipoFrecuencia()

        Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios

        cboTipoFrecuencia.DataTextField = "DescripcionTipoFrecuencia"
        cboTipoFrecuencia.DataValueField = "Id"
        'JMUG: cboTipoFrecuencia.DataSource = OrdenTrabajoNeg.TipoFrecuenciaLista()
        cboTipoFrecuencia.DataBind()
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
        cboCatalogo.DataSource = CatalogoNeg.CatalogoListarCombo(0, cboTipoActivo.SelectedValue, "", "1")
        cboCatalogo.Items.Clear()
        cboCatalogo.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboCatalogo.DataBind()
    End Sub

    Sub ListarPlantillaCheckListCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        cboListadoCheckListMantenimientoOrdenTrabajo.DataTextField = "vDescripcionCabeceraCheckListPlantilla"
        cboListadoCheckListMantenimientoOrdenTrabajo.DataValueField = "cIdNumeroCabeceraCheckListPlantilla"
        cboListadoCheckListMantenimientoOrdenTrabajo.DataSource = PlantillaCheckListNeg.ChecklistPlantillaListarCombo(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue, hfdIdTipoActivoEquipo.Value, hfdIdCatalogoEquipo.Value, hfdJerarquiaEquipo.Value)
        cboListadoCheckListMantenimientoOrdenTrabajo.Items.Clear()
        cboListadoCheckListMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        'Me.btnNuevo.Visible = bNuevo
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "071" 'Mantenimiento para Generar Contratos en el CMMS.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            ListarTipoMantenimientoCombo()
            ListarTipoActivoCombo()
            ListarCatalogoCombo()
            ListarTipoFrecuencia()
            'cboFiltroContrato.SelectedIndex = 4

            If Session("CestaEquipoContrato2") Is Nothing Then
                Session("CestaEquipoContrato2") = CrearCestaEquipo(31)
            Else
                VaciarCestaEquipo(Session("CestaEquipoContrato2"))
            End If

            If Session("CestaEquipoContratoImprimir") Is Nothing Then
                Session("CestaEquipoContratoImprimir") = CrearCestaEquipo(31)
            Else
                VaciarCestaEquipo(Session("CestaEquipoContratoImprimir"))
            End If

            If Session("CestaSSPersonal") Is Nothing Then
                Session("CestaSSPersonal") = CrearCestaPersonal()
            Else
                VaciarCestaPersonal(Session("CestaSSPersonal"))
            End If

            If Session("CestaPersonalOrdenTrabajo") Is Nothing Then
                Session("CestaPersonalOrdenTrabajo") = CrearCestaPersonalOrdenTrabajo()
            Else
                VaciarCestaPersonalOrdenTrabajo(Session("CestaPersonalOrdenTrabajo"))
            End If

            BloquearMantenimiento(True, False, True, False)

            'gdrlLista()


            pnlCabecera.Visible = True

        Else
        End If
    End Sub

    Sub gdrlLista()
        Dim SubFiltro As String = ""

        Dim filtro As String = ""
        If (String.IsNullOrEmpty(Session("IdContratoUsuario"))) Then
            'btnNuevo.Visible = False
            'btnEditar.Visible = False
            'lnkbtnVerContrato.Visible = False
            'lnkbtnVerProgramacion.Visible = False
        ElseIf (Session("IdContratoUsuario") = "*") Then
            filtro = "cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue
            Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda(filtro, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataBind()
        ElseIf (Session("IdContratoUsuario") <> Nothing) Then
            filtro = "CONCAT(cIdTipoDocumentoCabeceraContrato, '-', vIdNumeroSerieCabeceraContrato, '-', vIdNumeroCorrelativoCabeceraContrato) = '" & Session("IdContratoUsuario") & "' AND cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue
            Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda(filtro, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataBind()
        End If

        'Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda(filtro, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        'Me.grdLista.DataBind()
    End Sub


    Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Dim SubFiltro As String = ""
        Dim filtro As String = ""

        If (Session("IdContratoUsuario") = "*") Then
            filtro = "cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue
            Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda(filtro, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataBind()
        ElseIf (Session("IdContratoUsuario") <> Nothing) Then
            filtro = "CONCAT(cIdTipoDocumentoCabeceraContrato, '-', vIdNumeroSerieCabeceraContrato, '-', vIdNumeroCorrelativoCabeceraContrato) = '" & Session("IdContratoUsuario") & "' AND cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue
            Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda(filtro, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataBind()
        End If

        'Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        'Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = True 'IdNumeroSerie
            e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'FechaEmision
            e.Row.Cells(4).Visible = True 'IdCliente
            e.Row.Cells(5).Visible = False 'RucCliente
            e.Row.Cells(6).Visible = True 'RazonSocialCliente
            e.Row.Cells(7).Visible = False 'Estado
            e.Row.Cells(8).Visible = False 'Estado Registro
            e.Row.Cells(9).Visible = True 'DescripcionCabeceraContrato
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = True 'IdNumeroSerie
            e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'FechaEmision
            e.Row.Cells(4).Visible = True 'IdCliente
            e.Row.Cells(5).Visible = False 'RucCliente
            e.Row.Cells(6).Visible = True 'RazonSocialCliente
            e.Row.Cells(7).Visible = False 'Estado
            e.Row.Cells(8).Visible = False 'Estado Registro
            e.Row.Cells(9).Visible = True 'DescripcionCabeceraContrato
        End If
    End Sub
    'Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0666", strOpcionModulo, "CMMS")

    '        pnlCabecera.Visible = True
    '        'pnlContenido.Visible = False
    '        BloquearMantenimiento(True, False, True, False)
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Private Sub cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndexChanged
        'ListarPlantillaCheckListCombo()
        'lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboTipoActivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivo.SelectedIndexChanged
        ListarCatalogoCombo()
        'CargarCestaCatalogoComponente() 'JMUG: 28/03/2023
    End Sub

    Private Sub cboCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCatalogo.SelectedIndexChanged
        'CargarCestaCatalogoComponente()
    End Sub

End Class
