Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiOrdenVentaAlmacen
    Inherits System.Web.UI.Page
    Dim OrdenVentaNeg As New clsOrdenVentaNegocios
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

    Shared Function CrearCestaControlDespacho() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Item", GetType(System.Int16))) '1
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("CantidadOrigen", GetType(System.Decimal))) '3
        dt.Columns.Add(New DataColumn("CantidadEncontrada", GetType(System.Decimal))) '4
        dt.Columns.Add(New DataColumn("CantidadSaldo", GetType(System.Decimal))) '5
        dt.Columns.Add(New DataColumn("UbicacionAlmacenReferencia", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("NumeroSerie", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("CantidadAtendida", GetType(System.Decimal))) '8
        Return dt
    End Function

    Shared Sub EditarCestaControlDespacho(ByVal Item As Int16, ByVal Codigo As String, ByVal Descripcion As String, ByVal CantidadOrigen As Decimal,
                                          ByVal CantidadEncontrada As Decimal, ByVal CantidadSaldo As Decimal, ByVal UbicacionAlmacenRef As String,
                                          ByVal CantidadAtendida As Decimal, ByVal NroSerie As String, ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Item
                Tabla.Rows(Fila)(1) = Codigo
                Tabla.Rows(Fila)(2) = Descripcion
                Tabla.Rows(Fila)(3) = CantidadOrigen
                Tabla.Rows(Fila)(4) = CantidadEncontrada
                Tabla.Rows(Fila)(5) = CantidadSaldo
                Tabla.Rows(Fila)(6) = UbicacionAlmacenRef
                Tabla.Rows(Fila)(7) = NroSerie
                Tabla.Rows(Fila)(8) = CantidadAtendida
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaControlDespacho(ByVal Item As Int16, ByVal Codigo As String, ByVal Descripcion As String, ByVal CantidadOrigen As Decimal,
                                          ByVal CantidadEncontrada As Decimal, ByVal CantidadSaldo As Decimal, ByVal UbicacionAlmacenRef As String,
                                          ByVal CantidadAtendida As Decimal, ByVal NroSerie As String, ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Item
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("CantidadOrigen") = Math.Round(CantidadOrigen, 2)
        Fila("CantidadEncontrada") = Math.Round(CantidadEncontrada, 2)
        Fila("CantidadSaldo") = Math.Round(CantidadSaldo, 2)
        Fila("UbicacionAlmacenReferencia") = UbicacionAlmacenRef
        Fila("NumeroSerie") = NroSerie
        Fila("CantidadAtendida") = Math.Round(CantidadAtendida, 2)
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaControlDespacho(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaControlDespacho(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub CargarCestaControlDespachoPrincipal(ByVal strOVKey As String, ByVal strOFKey As String)
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaControlDespacho(Session("CestaControlDespacho"))
            Dim dsControlDespacho
            If strOFKey <> "" And hfdTieneOF.Value = "1" Then
                dsControlDespacho = CaracteristicaNeg.CaracteristicaGetData("SELECT DETDOC.nIdNumeroItemDetalleOrdenFabricacion, DETDOC.vIdArticuloSAPDetalleOrdenFabricacion, DETDOC.vDescripcionArticuloSAPDetalleOrdenFabricacion, DETDOC.nCantidadSAPDetalleOrdenFabricacion, " &
                                                                            "       ISNULL(DETDOC.nCantidadAlmacenEncontradaDetalleOrdenFabricacion, 0) AS nCantidadAlmacenEncontradaDetalleOrdenFabricacion, (ISNULL(DETDOC.nCantidadSAPDetalleOrdenFabricacion,0)-ISNULL(DETDOC.nCantidadAtendida, 0)) AS nCantidadAlmacenSaldoDetalleOrdenFabricacion, ISNULL(DETDOC.vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion, 0) AS vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion, " &
                                                                            "       ISNULL(DETDOC.vUbicacionAlmacenReferenciaDetalleOrdenFabricacion, '') AS vUbicacionAlmacenReferenciaDetalleOrdenFabricacion, " &
                                                                            "       ISNULL(DETDOC.vNumeroSerieDetalleOrdenFabricacion, '') AS vNumeroSerieDetalleOrdenFabricacion, ISNULL(DETDOC.nCantidadAtendida, 0) AS nCantidadAtendida " &
                                                                            "FROM LOGI_DETALLEORDENFABRICACION AS DETDOC INNER JOIN LOGI_CABECERAORDENFABRICACION CABDOC ON " &
                                                                            "     DETDOC.cIdEmpresa = CABDOC.cIdEmpresa AND DETDOC.cIdTipoDocumentoCabeceraOrdenFabricacion = CABDOC.cIdTipoDocumentoCabeceraOrdenFabricacion " &
                                                                            "     AND DETDOC.vIdNumeroSerieCabeceraOrdenFabricacion = CABDOC.vIdNumeroSerieCabeceraOrdenFabricacion AND DETDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion = CABDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion " &
                                                                            "     AND DETDOC.cIdEquipoSAPCabeceraOrdenFabricacion = CABDOC.cIdEquipoSAPCabeceraOrdenFabricacion AND DETDOC.vIdArticuloSAPCabeceraOrdenFabricacion = CABDOC.vIdArticuloSAPCabeceraOrdenFabricacion " &
                                                                            "WHERE CABDOC.cIdEmpresa = '" & Session("IdEmpresa") & "' AND CABDOC.cIdTipoDocumentoCabeceraOrdenFabricacion + '-' + CONVERT(CHAR(4), YEAR(CABDOC.dFechaEmisionCabeceraOrdenFabricacion)) + '-' + CABDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & strOFKey & "' " &
                                                                            "      AND vIdArticuloSAPDetalleOrdenFabricacion NOT LIKE 'CPMO%' AND vIdArticuloSAPDetalleOrdenFabricacion NOT LIKE 'CPCI%'")
                For Each ControlDespacho In dsControlDespacho.Rows
                    AgregarCestaControlDespacho(ControlDespacho("nIdNumeroItemDetalleOrdenFabricacion"), ControlDespacho("vIdArticuloSAPDetalleOrdenFabricacion"), ControlDespacho("vDescripcionArticuloSAPDetalleOrdenFabricacion"), ControlDespacho("nCantidadSAPDetalleOrdenFabricacion"), ControlDespacho("nCantidadAlmacenEncontradaDetalleOrdenFabricacion"), ControlDespacho("nCantidadAlmacenSaldoDetalleOrdenFabricacion"), ControlDespacho("vUbicacionAlmacenReferenciaDetalleOrdenFabricacion"), ControlDespacho("nCantidadAtendida"), ControlDespacho("vNumeroSerieDetalleOrdenFabricacion"), Session("CestaControlDespacho"))
                Next
            ElseIf strOVKey <> "" Then
                dsControlDespacho = CaracteristicaNeg.CaracteristicaGetData("SELECT nIdNumeroItemDetalleOrdenVenta, vIdArticuloSAPDetalleOrdenVenta, vDescripcionArticuloSAPDetalleOrdenVenta, nCantidadSAPDetalleOrdenVenta, " &
                                                                            "       ISNULL(nCantidadAlmacenEncontradaDetalleOrdenVenta, 0) AS nCantidadAlmacenEncontradaDetalleOrdenVenta, (ISNULL(nCantidadSAPDetalleOrdenVenta,0)-ISNULL(nCantidadAtendida, 0)) AS nCantidadAlmacenSaldoDetalleOrdenVenta, ISNULL(vDescripcionUnidadMedidaSAPDetalleOrdenVenta, 0) AS vDescripcionUnidadMedidaSAPDetalleOrdenVenta, " &
                                                                            "       ISNULL(vUbicacionAlmacenReferenciaDetalleOrdenVenta, '') AS vUbicacionAlmacenReferenciaDetalleOrdenVenta, " &
                                                                            "       ISNULL(vNumeroSerieDetalleOrdenVenta, '') AS vNumeroSerieDetalleOrdenVenta, ISNULL(nCantidadAtendida, 0) AS nCantidadAtendida " &
                                                                            "FROM LOGI_DETALLEORDENVENTA " &
                                                                            "WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenVenta = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & grdLista.SelectedRow.Cells(1).Text & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & grdLista.SelectedRow.Cells(2).Text & "' " &
                                                                            "      AND vOrdenFabricacionCabeceraOrdenVenta = '" & strOFKey & "' " &
                                                                            "      AND vIdArticuloSAPDetalleOrdenVenta NOT LIKE 'CPMO%' AND vIdArticuloSAPDetalleOrdenVenta NOT LIKE 'CPCI%'")
                For Each ControlDespacho In dsControlDespacho.Rows
                    AgregarCestaControlDespacho(ControlDespacho("nIdNumeroItemDetalleOrdenVenta"), ControlDespacho("vIdArticuloSAPDetalleOrdenVenta"), ControlDespacho("vDescripcionArticuloSAPDetalleOrdenVenta"), ControlDespacho("nCantidadSAPDetalleOrdenVenta"), ControlDespacho("nCantidadAlmacenEncontradaDetalleOrdenVenta"), ControlDespacho("nCantidadAlmacenSaldoDetalleOrdenVenta"), ControlDespacho("vUbicacionAlmacenReferenciaDetalleOrdenVenta"), ControlDespacho("nCantidadAtendida"), ControlDespacho("vNumeroSerieDetalleOrdenVenta"), Session("CestaControlDespacho"))
                Next
            End If

            Me.grdDetalleControlDespacho.DataSource = Session("CestaControlDespacho")
            Me.grdDetalleControlDespacho.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltroEquipo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroEquipo.DataValueField = "vValor"
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("90", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
    End Sub

    Sub ListarPersonalResponsableCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboAuxiliarMantenimientoAsesorAuxiliar.DataTextField = "vNombreCompletoPersonal"
        cboAuxiliarMantenimientoAsesorAuxiliar.DataValueField = "cIdPersonal"
        cboAuxiliarMantenimientoAsesorAuxiliar.DataSource = ResponsableNeg.PersonalListarCombo("ALM")
        cboAuxiliarMantenimientoAsesorAuxiliar.Items.Clear()
        cboAuxiliarMantenimientoAsesorAuxiliar.Items.Add("SELECCIONE DATO")
        cboAuxiliarMantenimientoAsesorAuxiliar.DataBind()
    End Sub

    Sub ListarUnidadTrabajoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UniTraNeg As New clsOrdenVentaNegocios
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataTextField = "vIdUnidadTrabajoOrdenVenta"
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataValueField = "vIdUnidadTrabajoOrdenVenta"
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataSource = UniTraNeg.OrdenVentaUnidadTrabajoListarCombo("*")
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.Items.Clear()
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.Items.Add("SELECCIONE DATO")
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Enabled = bActivar
        cboAuxiliarMantenimientoAsesorAuxiliar.Enabled = bActivar
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.Enabled = bActivar
    End Sub

    Sub LlenarData()
        Dim OrdenVenta As LOGI_CABECERAORDENVENTA = OrdenVentaNeg.OrdenVentaListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim, Session("IdEmpresa"))
        Dim strFechaOV, strFechaOF As String
        If OrdenVenta.dFechaOrdenVentaCabeceraOrdenVenta Is Nothing Then
            strFechaOV = ""
        Else
            strFechaOV = Year(OrdenVenta.dFechaOrdenVentaCabeceraOrdenVenta)
        End If
        If OrdenVenta.dFechaOrdenFabricacionCabeceraOrdenVenta Is Nothing Then
            strFechaOF = ""
        Else
            strFechaOF = Year(OrdenVenta.dFechaOrdenFabricacionCabeceraOrdenVenta)
        End If
        chkTodoConforme.Checked = False
        lblNroOrdenVenta.Text = ""
        lblNroOrdenVenta.Text = grdLista.SelectedRow.Cells(0).Text + "-" + strFechaOV + "-" + OrdenVenta.vOrdenVentaSAPCabeceraOrdenVenta
        hfdNroOrdenVenta.Value = ""
        hfdNroOrdenVenta.Value = grdLista.SelectedRow.Cells(0).Text + "-" + strFechaOV + "-" + OrdenVenta.vOrdenVentaCabeceraOrdenVenta
        Dim dsOFOrigen
        dsOFOrigen = CaracteristicaNeg.CaracteristicaGetData("SELECT CABDOC.cIdTipoDocumentoCabeceraOrdenFabricacion, CABDOC.vIdNumeroSerieCabeceraOrdenFabricacion, CABDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion " &
                                                                    "FROM LOGI_CABECERAORDENFABRICACION CABDOC " &
                                                                    "WHERE CABDOC.cIdEmpresa = '" & Session("IdEmpresa") & "' AND CONVERT(CHAR(4), YEAR(CABDOC.dFechaEmisionCabeceraOrdenFabricacion)) + '-' + CABDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & strFechaOF + "-" + OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta & "'")
        lblNroOrdenFabricacion.Text = ""
        hfdNroOrdenFabricacion.Value = ""
        For Each ControlOFOrigen In dsOFOrigen.Rows
            lblNroOrdenFabricacion.Text = ControlOFOrigen("cIdTipoDocumentoCabeceraOrdenFabricacion") + "-" + strFechaOF + "-" + OrdenVenta.vOrdenFabricacionSAPCabeceraOrdenVenta
            hfdNroOrdenFabricacion.Value = ControlOFOrigen("cIdTipoDocumentoCabeceraOrdenFabricacion") + "-" + strFechaOF + "-" + OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta
        Next

        lblDescripcionOrden.Text = OrdenVenta.vDescripcionCabeceraOrdenVenta
        Dim CliNeg As New clsClienteNegocios
        Dim Cliente As GNRL_CLIENTE = CliNeg.ClienteListarPorId(OrdenVenta.cIdCliente, Session("IdEmpresa"), "*")
        lblRucCliente.Text = Cliente.vRucCliente
        lblRazonSocialCliente.Text = Cliente.vRazonSocialCliente
        If IsNothing(OrdenVenta.bTieneOrdenFabricacionCabeceraOrdenVenta) = True Then
            If Trim(OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta) = "" Then
                hfdTieneOF.Value = "0"
            Else
                hfdTieneOF.Value = "1"
            End If
        Else
            hfdTieneOF.Value = IIf(Convert.ToBoolean(OrdenVenta.bTieneOrdenFabricacionCabeceraOrdenVenta) = True, "1", "0")
        End If
        If hfdTieneOF.Value = "1" Then
            lblTituloAdicional.Text = "(Se visualiza la información de la O.Fabricación)"
        Else
            lblTituloAdicional.Text = "(Se visualiza la información de la O.Venta)"
        End If
        CargarCestaControlDespachoPrincipal(hfdNroOrdenVenta.Value, hfdNroOrdenFabricacion.Value)

        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
    End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvTxtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.EnableClientScript = bValidar
        Me.rfvAuxiliarMantenimientoAsesorAuxiliar.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        'Me.btnNuevo.Visible = bNuevo
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        hfdCorreoElectronicoCliente.Value = ""
        hfdDNICliente.Value = ""
        hfdRUCCliente.Value = ""
        hfdIdClienteSAPEquipo.Value = ""
        hfdFechaCreacionEquipo.Value = ""
        hfdIdUsuarioCreacionEquipo.Value = ""
        hfdTieneOF.Value = ""
    End Sub

    Sub LimpiarObjetosOrdenTrabajo()
        txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = ""
        cboAuxiliarMantenimientoAsesorAuxiliar.SelectedIndex = -1
    End Sub

    Sub ListarTiempoCombo()
        'Llena los DropDownList de horas, minutos y segundos
        cboHorasMantenimientoAsesorAuxiliar.Items.Clear()
        cboMinutosMantenimientoAsesorAuxiliar.Items.Clear()
        cboSegundosMantenimientoAsesorAuxiliar.Items.Clear()

        For i = 0 To 23
            cboHorasMantenimientoAsesorAuxiliar.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next

        For i = 0 To 59
            cboMinutosMantenimientoAsesorAuxiliar.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
            cboSegundosMantenimientoAsesorAuxiliar.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next
        cboMeridianoMantenimientoAsesorAuxiliar.Enabled = False
    End Sub

    Protected Sub Upload_File(sender As Object, e As EventArgs)
        If hfdOperacion.Value = "N" Then
            'GenerarComprobante()
        End If
        'btnNuevo.Enabled = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "128" 'Mantenimiento de las Solicitudes de Servicios.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroEquipo.SelectedIndex = 4
            ListarPersonalResponsableCombo()
            ListarUnidadTrabajoCombo()
            cboFiltroStatus.SelectedIndex = 5

            If Session("CestaControlDespacho") Is Nothing Then
                Session("CestaControlDespacho") = CrearCestaControlDespacho()
            Else
                VaciarCestaControlDespacho(Session("CestaControlDespacho"))
            End If

            btnAsignarAsesor.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0716", strOpcionModulo, "CMMS")

            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
            Me.grdLista.DataBind()
            pnlCabecera.Visible = True
            pnlControlDespacho.Visible = False
            btnImprimirOrdenVentaReporte.Attributes.Add("onclick", "javascript:popupEmitirListaOrdenVentaReporte();")
        Else
            txtBuscarEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            btnImprimirOrdenVentaReporte.Attributes.Add("onclick", "javascript:popupEmitirListaOrdenVentaReporte();")
        End If
    End Sub

    Protected Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0710", strOpcionModulo, "CMMS")

            If IsNothing(grdLista.SelectedRow) = False Then
                If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                    hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(16).Text) = "True", "1", "0")
                End If
                If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                    Throw New Exception("Este registro se encuentra desactivado.")
                End If

                hfdOperacion.Value = "E"
                BloquearMantenimiento(False, True, False, True)
                ValidarTexto(True)
                ActivarObjetos(True)
                LlenarData()
                pnlCabecera.Visible = False
                pnlControlDespacho.Visible = True
                If btnGenerar.Text = "Ver" Then
                    hfdOperacion.Value = "R"
                End If
            Else
                Throw New Exception("Debe de seleccionar un item.")
            End If
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

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(16).Text) = "True", "1", "0") 'JMUG: 21/09/2023
                            Dim OrdenVenta As LOGI_CABECERAORDENVENTA = OrdenVentaNeg.OrdenVentaListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim, Session("IdEmpresa"))
                            Dim TieneOF = IIf(OrdenVenta.bTieneOrdenFabricacionCabeceraOrdenVenta = True, "1", "0")
                            Dim NroOF = Trim(OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta)
                            hfdTieneOF.Value = TieneOF
                            Dim lblEstado As Label = TryCast(grdLista.SelectedRow.Cells(16).FindControl("lblEstado"), Label)
                            If lblEstado.Text = "Atendido" Or lblEstado.Text = "Atención Parcial" Then
                                btnAsignarAsesor.Visible = False
                                btnGenerar.Text = "Ver"
                            Else
                                btnGenerar.Text = "Ver Detalle"
                                btnAsignarAsesor.Visible = True
                            End If
                            lnkbtnVerOrdenVenta.Attributes.Add("onclick", "javascript:popupEmitirOrdenVentaReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "', '" & hfdTieneOF.Value & "', '" & NroOF & "');")
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
            e.Row.Cells(0).Visible = False 'IdTipoDocumento
            e.Row.Cells(1).Visible = False 'IdNumeroSerie
            e.Row.Cells(2).Visible = False 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'OrdenCompra
            e.Row.Cells(4).Visible = True 'OrdenVenta
            e.Row.Cells(5).Visible = True 'FechaOrdenVenta
            e.Row.Cells(6).Visible = True 'OrdenFabricacion
            e.Row.Cells(7).Visible = True 'FechaEmision
            e.Row.Cells(8).Visible = True 'CantidadItems
            e.Row.Cells(9).Visible = False 'IdCliente
            e.Row.Cells(10).Visible = False 'IdClienteSAP
            e.Row.Cells(11).Visible = True 'Cliente
            e.Row.Cells(12).Visible = True 'NombreCompletoAuxiliar
            e.Row.Cells(13).Visible = True 'FechaAsignacionAuxiliar
            e.Row.Cells(14).Visible = True 'Fechas
            e.Row.Cells(15).Visible = True 'StatusOrdenVenta
            e.Row.Cells(16).Visible = False 'Estado
            e.Row.Cells(17).Visible = False 'IdUnidadTrabajo
            e.Row.Cells(18).Visible = False 'IdEquipo
            e.Row.Cells(19).Visible = False 'NumeroSerieEquipo
            e.Row.Cells(20).Visible = False 'OrdenCompraKey
            e.Row.Cells(21).Visible = False 'OrdenVentaKey
            e.Row.Cells(22).Visible = False 'OrdenFabricacionKey
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = False 'IdTipoDocumento
            e.Row.Cells(1).Visible = False 'IdNumeroSerie
            e.Row.Cells(2).Visible = False 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'OrdenCompra
            e.Row.Cells(4).Visible = True 'OrdenVenta
            e.Row.Cells(5).Visible = True 'FechaOrdenVenta
            e.Row.Cells(6).Visible = True 'OrdenFabricacion
            e.Row.Cells(7).Visible = True 'FechaEmision
            e.Row.Cells(8).Visible = True 'CantidadItems
            e.Row.Cells(9).Visible = False 'IdCliente
            e.Row.Cells(10).Visible = False 'IdClienteSAP
            e.Row.Cells(11).Visible = True 'Cliente
            e.Row.Cells(12).Visible = True 'NombreCompletoAuxiliar
            e.Row.Cells(13).Visible = True 'FechaAsignacionAuxiliar
            e.Row.Cells(14).Visible = True 'Fechas
            e.Row.Cells(15).Visible = True 'StatusOrdenVenta
            e.Row.Cells(16).Visible = False 'Estado
            e.Row.Cells(17).Visible = False 'IdUnidadTrabajo
            e.Row.Cells(18).Visible = False 'IdEquipo
            e.Row.Cells(19).Visible = False 'NumeroSerieEquipo
            e.Row.Cells(20).Visible = False 'OrdenCompraKey
            e.Row.Cells(21).Visible = False 'OrdenVentaKey
            e.Row.Cells(22).Visible = False 'OrdenFabricacionKey
        End If
    End Sub

    Private Sub imgbtnBuscarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarEquipo.Click
        Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
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

                OrdenVentaNeg.OrdenVentaGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
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

                Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
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

                OrdenVentaNeg.OrdenVentaGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 1 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
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

                Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
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
        Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
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
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'OrdenCompra
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'OrdenVenta
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Center 'FechaOrdenVenta
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left 'OrdenFabricacion
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'FechaEmision
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'CantidadItems
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'IdCliente
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'IdClienteSAP
                e.Row.Cells.Item(11).HorizontalAlign = HorizontalAlign.Left 'Cliente
                e.Row.Cells.Item(12).HorizontalAlign = HorizontalAlign.Left 'NombreCompletoAuxiliar
                e.Row.Cells.Item(13).HorizontalAlign = HorizontalAlign.Left 'FechaAsignacionAuxiliar
                e.Row.Cells.Item(14).HorizontalAlign = HorizontalAlign.Left 'Fechas
                e.Row.Cells.Item(15).HorizontalAlign = HorizontalAlign.Left 'StatusOrdenVenta
                e.Row.Cells.Item(16).HorizontalAlign = HorizontalAlign.Left 'Estado
                e.Row.Cells.Item(17).HorizontalAlign = HorizontalAlign.Left 'IdUnidadTrabajo
                e.Row.Cells.Item(18).HorizontalAlign = HorizontalAlign.Left 'IdEquipo
                e.Row.Cells.Item(19).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                e.Row.Cells.Item(20).HorizontalAlign = HorizontalAlign.Left = False 'OrdenCompraKey
                e.Row.Cells.Item(21).HorizontalAlign = HorizontalAlign.Left = False 'OrdenVentaKey
                e.Row.Cells.Item(22).HorizontalAlign = HorizontalAlign.Left = False 'OrdenFabricacionKey
            Next
        End If
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0713", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = True
            pnlControlDespacho.Visible = False
            BloquearMantenimiento(True, False, True, False)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0712", strOpcionModulo, "CMMS")

            Dim coleccionOF As New List(Of LOGI_DETALLEORDENFABRICACION)
            Dim coleccionOV As New List(Of LOGI_DETALLEORDENVENTA)

            Dim dsOrdenVenta = OrdenVentaNeg.OrdenVentaGetData("SELECT * FROM LOGI_CABECERAORDENVENTA " &
                                                        "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim & "' AND " &
                                                        "      vIdNumeroSerieCabeceraOrdenVenta = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND " &
                                                        "      vIdNumeroCorrelativoCabeceraOrdenVenta= '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' AND " &
                                                        "      cIdEmpresa = '" & Session("IdEmpresa") & "' AND ISNULL(vOrdenFabricacionCabeceraOrdenVenta, '') = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim & "'")
            Dim strEstado As String = "R"
            For Each OrdVta In dsOrdenVenta.Rows
                If Not IsDBNull(OrdVta("cEstadoCabeceraOrdenVenta")) Then strEstado = OrdVta("cEstadoCabeceraOrdenVenta")
                'strEstado = OrdVta("cEstadoCabeceraOrdenVenta")
            Next
            For i = 0 To Session("CestaControlDespacho").rows.count - 1
                If (Session("CestaControlDespacho").Rows(i)("CantidadSaldo") <> 0) Then
                    strEstado = "P"
                    Exit For
                Else
                    strEstado = "T"
                End If
            Next

            If hfdTieneOF.Value = "1" Then 'Tiene Orden de Fabricacion
                Dim dsOFOrigen
                dsOFOrigen = CaracteristicaNeg.CaracteristicaGetData("SELECT CABDOC.cIdTipoDocumentoCabeceraOrdenFabricacion, CABDOC.vIdNumeroSerieCabeceraOrdenFabricacion, CABDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion, CABDOC.cIdEquipoSAPCabeceraOrdenFabricacion, CABDOC.vIdArticuloSAPCabeceraOrdenFabricacion " &
                                                                    "FROM LOGI_CABECERAORDENFABRICACION CABDOC " &
                                                                    "WHERE CABDOC.cIdEmpresa = '" & Session("IdEmpresa") & "' AND CABDOC.cIdTipoDocumentoCabeceraOrdenFabricacion + '-' + CONVERT(CHAR(4), YEAR(CABDOC.dFechaEmisionCabeceraOrdenFabricacion)) + '-' + CABDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & hfdNroOrdenFabricacion.Value & "'")
                Dim OFOrigen As String
                For Each ControlOFOrigen In dsOFOrigen.Rows
                    OFOrigen = ControlOFOrigen("cIdTipoDocumentoCabeceraOrdenFabricacion") + "-" + ControlOFOrigen("vIdNumeroSerieCabeceraOrdenFabricacion") + "-" + ControlOFOrigen("vIdNumeroCorrelativoCabeceraOrdenFabricacion") + "-" + ControlOFOrigen("cIdEquipoSAPCabeceraOrdenFabricacion") + "-" + ControlOFOrigen("vIdArticuloSAPCabeceraOrdenFabricacion")
                Next

                For i = 0 To Session("CestaControlDespacho").rows.count - 1
                    Dim DetOrdFab As New LOGI_DETALLEORDENFABRICACION
                    DetOrdFab.cIdTipoDocumentoCabeceraOrdenFabricacion = OFOrigen.Split("-")(0) 'Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim
                    DetOrdFab.vIdNumeroSerieCabeceraOrdenFabricacion = OFOrigen.Split("-")(1) 'Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim
                    DetOrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion = OFOrigen.Split("-")(2) 'Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim
                    DetOrdFab.cIdEmpresa = Session("IdEmpresa")
                    DetOrdFab.nIdNumeroItemDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("Item").ToString.Trim
                    DetOrdFab.nCantidadAlmacenEncontradaDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("CantidadEncontrada").ToString.Trim
                    DetOrdFab.nCantidadAlmacenSaldoDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("CantidadSaldo").ToString.Trim
                    DetOrdFab.cIdEquipoSAPCabeceraOrdenFabricacion = OFOrigen.Split("-")(3)
                    DetOrdFab.vIdArticuloSAPCabeceraOrdenFabricacion = OFOrigen.Split("-")(4)
                    DetOrdFab.vIdArticuloSAPDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("Codigo").ToString.Trim
                    DetOrdFab.vNumeroSerieDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("NumeroSerie").ToString.Trim
                    coleccionOF.Add(DetOrdFab)
                Next
            Else
                For i = 0 To Session("CestaControlDespacho").rows.count - 1
                    Dim DetOrdVta As New LOGI_DETALLEORDENVENTA
                    DetOrdVta.cIdTipoDocumentoCabeceraOrdenVenta = Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim
                    DetOrdVta.vIdNumeroSerieCabeceraOrdenVenta = Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim
                    DetOrdVta.vIdNumeroCorrelativoCabeceraOrdenVenta = Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim
                    DetOrdVta.cIdEmpresa = Session("IdEmpresa")
                    DetOrdVta.nIdNumeroItemDetalleOrdenVenta = Session("CestaControlDespacho").Rows(i)("Item").ToString.Trim
                    If lblNroOrdenFabricacion.Text <> "" Then
                        DetOrdVta.vOrdenFabricacionCabeceraOrdenVenta = lblNroOrdenFabricacion.Text.Split("-")(2).ToString
                    End If
                    DetOrdVta.nCantidadAlmacenEncontradaDetalleOrdenVenta = Session("CestaControlDespacho").Rows(i)("CantidadEncontrada").ToString.Trim
                    DetOrdVta.nCantidadAlmacenSaldoDetalleOrdenVenta = Session("CestaControlDespacho").Rows(i)("CantidadSaldo").ToString.Trim
                    DetOrdVta.vIdArticuloSAPDetalleOrdenVenta = Session("CestaControlDespacho").Rows(i)("Codigo").ToString.Trim
                    DetOrdVta.vNumeroSerieDetalleOrdenVenta = Session("CestaControlDespacho").Rows(i)("NumeroSerie").ToString.Trim
                    coleccionOV.Add(DetOrdVta)
                Next
            End If

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

            If hfdOperacion.Value = "E" Then
                If hfdTieneOF.Value = "1" Then 'Tiene Orden de Fabricacion
                    Dim OrdenFabricacionNeg As New clsOrdenFabricacionNegocios
                    If OrdenFabricacionNeg.OrdenFabricacionActualizarDisponibilidadStock(coleccionOF, LogAuditoria) = 0 Then
                        OrdenVentaNeg.OrdenVentaGetData("UPDATE LOGI_CABECERAORDENVENTA SET bTieneOrdenFabricacionCabeceraOrdenVenta = '" & hfdTieneOF.Value & "', cEstadoCabeceraOrdenVenta = '" & strEstado & IIf(strEstado = "T", "', dFechaTerminadaCabeceraOrdenVenta = GETDATE() ", "' ") &
                                                        "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim & "' AND " &
                                                        "      vIdNumeroSerieCabeceraOrdenVenta = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND " &
                                                        "      vIdNumeroCorrelativoCabeceraOrdenVenta= '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' AND " &
                                                        "      cIdEmpresa = '" & Session("IdEmpresa") & "' AND ISNULL(vOrdenFabricacionCabeceraOrdenVenta, '') = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim & "'")
                    End If
                Else
                    If OrdenVentaNeg.OrdenVentaActualizarDisponibilidadStock(coleccionOV, LogAuditoria) = 0 Then
                        OrdenVentaNeg.OrdenVentaGetData("UPDATE LOGI_CABECERAORDENVENTA SET bTieneOrdenFabricacionCabeceraOrdenVenta = '" & hfdTieneOF.Value & "', cEstadoCabeceraOrdenVenta = '" & strEstado & IIf(strEstado = "T", "', dFechaTerminadaCabeceraOrdenVenta = GETDATE() ", "' ") &
                                                        "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & coleccionOV(0).cIdTipoDocumentoCabeceraOrdenVenta & "' AND " &
                                                        "      vIdNumeroSerieCabeceraOrdenVenta = '" & coleccionOV(0).vIdNumeroSerieCabeceraOrdenVenta & "' AND " &
                                                        "      vIdNumeroCorrelativoCabeceraOrdenVenta= '" & coleccionOV(0).vIdNumeroCorrelativoCabeceraOrdenVenta & "' AND " &
                                                        "      cIdEmpresa = '" & Session("IdEmpresa") & "' AND ISNULL(vOrdenFabricacionCabeceraOrdenVenta, '') = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim & "'")
                    End If
                End If
                MyValidator.ErrorMessage = "Transacción registrada con éxito"
                Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataBind()

                pnlCabecera.Visible = True
                pnlControlDespacho.Visible = False

                BloquearMantenimiento(True, False, True, False)
                hfdOperacion.Value = "R"
                txtBuscarEquipo.Focus()
            ElseIf hfdOperacion.Value = "R" Then
                Throw New Exception("Orden de Venta cerrada no se puede modificar.")
            End If

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

    Sub txtNroSerieDetalleControlDespacho_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleControlDespacho.Rows
                Dim txtNumeroSerieDetalle As TextBox = TryCast(row.Cells(2).FindControl("txtNroSerieDetalleControlDespacho"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleControlDespacho.Rows.Count * (grdDetalleControlDespacho.PageIndex))
                Session("CestaControlDespacho").Rows(FilaActual)("NumeroSerie") = txtNumeroSerieDetalle.Text
            Next
            Me.grdDetalleControlDespacho.DataSource = Session("CestaControlDespacho")
            Me.grdDetalleControlDespacho.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub


    Sub txtValorDetalleControlDespacho_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleControlDespacho.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(2).FindControl("txtValorDetalleControlDespacho"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleControlDespacho.Rows.Count * (grdDetalleControlDespacho.PageIndex))
                Session("CestaControlDespacho").Rows(FilaActual)("CantidadEncontrada") = txtValorDetalle.Text
                Session("CestaControlDespacho").Rows(FilaActual)("CantidadSaldo") = Session("CestaControlDespacho").Rows(FilaActual)("CantidadOrigen") - Convert.ToDecimal(txtValorDetalle.Text)
            Next
            OrdenVentaNeg.OrdenVentaGetData("UPDATE LOGI_CABECERAORDENVENTA SET dFechaInicioCabeceraOrdenVenta = GETDATE() " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' " &
                                            "      AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND ISNULL(vOrdenFabricacionCabeceraOrdenVenta, '') = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim) & "' " &
                                            "      AND dFechaInicioCabeceraOrdenVenta IS NULL")

            Me.grdDetalleControlDespacho.DataSource = Session("CestaControlDespacho")
            Me.grdDetalleControlDespacho.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoAsesorAuxiliar_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoAsesorAuxiliar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0676", strOpcionModulo, "CMMS")

            If txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de inicio del control.")
            End If

            Dim OrdVta As New LOGI_CABECERAORDENVENTA
            OrdVta = OrdenVentaNeg.OrdenVentaListarPorId(grdLista.SelectedRow.Cells(0).Text, grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(2).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim, Session("IdEmpresa"))
            OrdVta.cEstadoCabeceraOrdenVenta = "R"
            OrdVta.bEstadoRegistroCabeceraOrdenVenta = True
            OrdVta.dFechaUltimaModificacionCabeceraOrdenVenta = Now
            OrdVta.cIdUsuarioUltimaModificacionCabeceraOrdenVenta = Session("IdUsuario")
            OrdVta.vIdUnidadTrabajoOrdenVenta = cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedValue
            OrdVta.bTieneOrdenFabricacionCabeceraOrdenVenta = hfdTieneOF.Value
            Dim hora As String = cboHorasMantenimientoAsesorAuxiliar.SelectedValue
            Dim minutos As String = cboMinutosMantenimientoAsesorAuxiliar.SelectedValue
            Dim segundos As String = cboSegundosMantenimientoAsesorAuxiliar.SelectedValue
            Dim fecha As String = txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text

            'Concatenar los valores en una cadena en formato SQL
            Dim strFechaHoraProgramacionAlmacen As String = fecha & " " & hora & ":" & minutos & ":" & segundos
            OrdVta.dFechaProgramacionAlmacenCabeceraOrdenVenta = Convert.ToDateTime(strFechaHoraProgramacionAlmacen)

            OrdVta.cIdPersonal = cboAuxiliarMantenimientoAsesorAuxiliar.SelectedValue
            OrdVta.dFechaAsignacionAuxiliarCabeceraOrdenVenta = Convert.ToDateTime(IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(14).Text).Trim = "", Now, Server.HtmlDecode(grdLista.SelectedRow.Cells(14).Text)))

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

            If OrdenVentaNeg.OrdenVentaEdita(OrdVta) = 0 Then
                Session("Query") = "PA_LOGI_MNT_CABECERAORDENVENTA 'SQL_UPDATE', '', '" & OrdVta.cIdTipoDocumentoCabeceraOrdenVenta & "', '" & OrdVta.vIdNumeroSerieCabeceraOrdenVenta & "', '" &
                                           OrdVta.vIdNumeroCorrelativoCabeceraOrdenVenta & "', '" & OrdVta.cIdEmpresa & "', '" & OrdVta.vOrdenFabricacionCabeceraOrdenVenta & "', '" &
                                           OrdVta.dFechaTransaccionCabeceraOrdenVenta & "', '" & OrdVta.vIdClienteSAPCabeceraOrdenVenta & "', '" & OrdVta.cIdCliente & "', '" &
                                           OrdVta.cIdEquipo & "', '" & OrdVta.vNumeroSerieEquipoCabeceraOrdenVenta & "', '" & OrdVta.vOrdenVentaCabeceraOrdenVenta & "', '" & OrdVta.vOrdenCompraCabeceraOrdenVenta & "', '" & OrdVta.cEstadoCabeceraOrdenVenta & "', '" &
                                           OrdVta.bEstadoRegistroCabeceraOrdenVenta & "', '" & OrdVta.dFechaUltimaModificacionCabeceraOrdenVenta & "', '" & OrdVta.dFechaCreacionCabeceraOrdenVenta & "', '" &
                                           OrdVta.cIdUsuarioUltimaModificacionCabeceraOrdenVenta & "', '" & OrdVta.cIdUsuarioCreacionCabeceraOrdenVenta & "', '" &
                                           OrdVta.dFechaEmisionCabeceraOrdenVenta & "', '" & OrdVta.vIdUnidadTrabajoOrdenVenta & "', '" & OrdVta.dFechaProgramacionAlmacenCabeceraOrdenVenta & "', '" & OrdVta.cIdPersonal & "', '" &
                                           OrdVta.vDescripcionCabeceraOrdenVenta & "', '" & OrdVta.dFechaOrdenVentaCabeceraOrdenVenta & "', '" & OrdVta.dFechaAsignacionAuxiliarCabeceraOrdenVenta & "', '" &
                                           OrdVta.vIdNumeroCorrelativoCabeceraOrdenVenta & "'"

                LogAuditoria.vEvento = "ACTUALIZAR ORDEN VENTA"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                Dim UsuCorreo As New GNRL_USUARIO
                Dim dsUsuario = OrdenVentaNeg.OrdenVentaGetData("SELECT * FROM GNRL_USUARIO WHERE vIdNroDocumentoIdentidadUsuario IN (SELECT vNumeroDocumentoPersonal FROM RRHH_PERSONAL WHERE cIdPersonal = '" & OrdVta.cIdPersonal & "') AND cIdEmpresa = '" & Session("IdEmpresa") & "'")
                For Each fila In dsUsuario.Rows
                    With UsuCorreo
                        UsuCorreo.cIdUsuario = fila("cIdUsuario")
                        UsuCorreo.vLoginUsuario = fila("vLoginUsuario")
                        UsuCorreo.vPasswordUsuario = fila("vPasswordUsuario")
                    End With
                Next

                Dim dtCorreoResponsable = OrdenVentaNeg.OrdenVentaGetData("SELECT PER.cIdPersonal, PER.vEmailPersonal, PER.vNombresPersonal, PER.vApellidoPaternoPersonal FROM LOGI_CABECERAORDENVENTA AS ORDVTA " &
                                                    "INNER JOIN RRHH_PERSONAL AS PER ON " &
                                                    "ORDVTA.cIdPersonal = PER.cIdPersonal AND ORDVTA.cIdEmpresa = PER.cIdEmpresa " &
                                                    "WHERE ORDVTA.cIdTipoDocumentoCabeceraOrdenVenta = '" & OrdVta.cIdTipoDocumentoCabeceraOrdenVenta & "' AND ORDVTA.vIdNumeroSerieCabeceraOrdenVenta = '" & OrdVta.vIdNumeroSerieCabeceraOrdenVenta & "' AND ORDVTA.vIdNumeroCorrelativoCabeceraOrdenVenta = '" & OrdVta.vIdNumeroCorrelativoCabeceraOrdenVenta & "' AND ORDVTA.cIdEmpresa = '" & OrdVta.cIdEmpresa & "' AND ISNULL(ORDVTA.vOrdenFabricacionCabeceraOrdenVenta, '') = '" & Trim(OrdVta.vOrdenFabricacionCabeceraOrdenVenta) & "'")
                If dtCorreoResponsable.Rows.Count > 0 Then
                    If dtCorreoResponsable.Rows(0).Item(1).ToString() = "" Then
                        Throw New Exception("El responsable debe de tener un correo asignado.")
                    End If
                Else
                    Throw New Exception("No tiene asignado a ningún responsable para esta orden de trabajo.")
                End If
                Dim strHTML As String
                Dim EmpNeg As New clsEmpresaNegocios
                strHTML = FuncionesNeg.FormatoEnvioGeneral("003", OrdVta.cIdTipoDocumentoCabeceraOrdenVenta & "-" & OrdVta.vIdNumeroSerieCabeceraOrdenVenta & "-" & OrdVta.vIdNumeroCorrelativoCabeceraOrdenVenta & "*" & String.Format("{0:dd-MM-yyyy}", OrdVta.dFechaAsignacionAuxiliarCabeceraOrdenVenta) & "*" & EmpNeg.EmpresaListarPorId(Session("IdEmpresa")).vDescripcionEmpresa & "*" & UsuCorreo.vLoginUsuario & "*" & UsuCorreo.vPasswordUsuario)
                'Dim strFrom As String = "cmms@movitecnica.pe"
                'Dim strPwd As String = "MVTcmms@2305"
                'Dim strConfiguracionCorreo As String = "movitecnica.pe" & "|" & "465"
                Dim strFrom As String = "notificaciones@movitecnica.com.pe"
                Dim strPwd As String = "NTmvt$1604"
                Dim strConfiguracionCorreo As String = "smtp.office365.com" & "|" & "587"
                Dim strConfiguracionPeriodo As String = ""
                Dim strAsunto As String = ""
                strAsunto = "[" + Split(dtCorreoResponsable.Rows(0).Item(2).ToString(), " ")(0).ToString + " " + dtCorreoResponsable.Rows(0).Item(3).ToString() + "] [" + IIf(hfdTieneOF.Value = "0", "OV", "OF") & "-" & OrdVta.vIdNumeroSerieCabeceraOrdenVenta & "-" & IIf(hfdTieneOF.Value = "0", Trim(OrdVta.vOrdenVentaSAPCabeceraOrdenVenta), Trim(OrdVta.vOrdenFabricacionSAPCabeceraOrdenVenta)) + " ASIGNADA]"
                FuncionesNeg.EnviarCorreo(LCase(strFrom), strPwd, LCase(dtCorreoResponsable.Rows(0).Item(1).ToString()), strAsunto, strHTML, "", "", strConfiguracionCorreo, strConfiguracionPeriodo, "", False)

                MyValidator.ErrorMessage = "Registro actualizado con éxito"
                Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataBind()
                BloquearMantenimiento(True, False, True, False)
            End If

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelMantenimientoAsesorAuxiliar_ModalPopupExtender.Show()
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub cboHorasMantenimientoAsesorAuxiliar_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHorasMantenimientoAsesorAuxiliar.SelectedIndexChanged
        If Convert.ToInt16(cboHorasMantenimientoAsesorAuxiliar.SelectedValue) >= 12 Then
            cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "PM"
        Else
            cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "AM"
        End If
        lnk_mostrarPanelMantenimientoAsesorAuxiliar_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAsignarAsesor_Click(sender As Object, e As EventArgs) Handles btnAsignarAsesor.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0716", strOpcionModulo, "CMMS")

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedIndex = -1
                            txtOrdenVentaMantenimientoAsesorAuxiliar.Text = ""
                            txtOrdenCompraMantenimientoAsesorAuxiliar.Text = ""
                            txtOrdenFabricacionMantenimientoAsesorAuxiliar.Text = ""
                            hfdIdClienteSAPEquipo.Value = ""
                            hfdEstado.Value = "1"

                            Dim EquipoNeg As New clsEquipoNegocios
                            Dim OVNeg As New clsOrdenVentaNegocios
                            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
                            Dim OrdVta As LOGI_CABECERAORDENVENTA = OVNeg.OrdenVentaListarPorId(row.Cells(0).Text, row.Cells(1).Text, row.Cells(2).Text, Server.HtmlDecode(row.Cells(22).Text).Trim, Session("IdEmpresa"))
                            ListarTiempoCombo()

                            Dim ahora As DateTime
                            ahora = IIf(IsNothing(OrdVta.dFechaProgramacionAlmacenCabeceraOrdenVenta) = True, DateTime.Now, OrdVta.dFechaProgramacionAlmacenCabeceraOrdenVenta)

                            txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = ahora.ToString("yyyy-MM-dd")

                            ' Obtener la hora actual y asignarla a los DropDownList de horas, minutos y segundos
                            Dim hora As Integer = ahora.Hour
                            Dim minuto As Integer = ahora.Minute
                            Dim segundo As Integer = ahora.Second

                            cboHorasMantenimientoAsesorAuxiliar.SelectedValue = hora.ToString("00")
                            cboMinutosMantenimientoAsesorAuxiliar.SelectedValue = minuto.ToString("00")
                            cboSegundosMantenimientoAsesorAuxiliar.SelectedValue = segundo.ToString("00")
                            If Convert.ToInt16(cboHorasMantenimientoAsesorAuxiliar.SelectedValue) >= 12 Then
                                cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "PM"
                            Else
                                cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "AM"
                            End If
                            cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedValue = Server.HtmlDecode(row.Cells(18).Text).Trim
                            txtOrdenVentaMantenimientoAsesorAuxiliar.Text = OrdVta.vOrdenVentaSAPCabeceraOrdenVenta
                            txtOrdenCompraMantenimientoAsesorAuxiliar.Text = OrdVta.vOrdenCompraSAPCabeceraOrdenVenta
                            txtOrdenFabricacionMantenimientoAsesorAuxiliar.Text = OrdVta.vOrdenFabricacionSAPCabeceraOrdenVenta

                            hfdFechaCreacionEquipo.Value = IIf(IsNothing(OrdVta.dFechaCreacionCabeceraOrdenVenta), Now, OrdVta.dFechaCreacionCabeceraOrdenVenta)
                            hfdIdUsuarioCreacionEquipo.Value = IIf(IsNothing(OrdVta.cIdUsuarioCreacionCabeceraOrdenVenta), Session("IdUsuario"), OrdVta.cIdUsuarioCreacionCabeceraOrdenVenta)
                            hfdIdClienteSAPEquipo.Value = Server.HtmlDecode(grdLista.SelectedRow.Cells(11).Text) 'JMUG: 18/10/2023
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0") 'JMUG: 21/09/2023
                            If IsNothing(OrdVta.bTieneOrdenFabricacionCabeceraOrdenVenta) = True Then
                                If Trim(OrdVta.vOrdenFabricacionCabeceraOrdenVenta) = "" Then
                                    hfdTieneOF.Value = "0"
                                Else
                                    hfdTieneOF.Value = "1"
                                End If
                            Else
                                hfdTieneOF.Value = IIf(Convert.ToBoolean(OrdVta.bTieneOrdenFabricacionCabeceraOrdenVenta) = True, "1", "0")
                            End If
                            lnk_mostrarPanelMantenimientoAsesorAuxiliar_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Seleccione un item para asignar un asesor.")
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

    Private Sub lnkbtnTerminarOrdenVenta_Click(sender As Object, e As EventArgs) Handles lnkbtnTerminarOrdenVenta.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0715", strOpcionModulo, "CMMS")

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            LlenarData()
                            'NUEVO JMUG: 04/12/2023
                            Dim dsOrdenVenta = OrdenVentaNeg.OrdenVentaGetData("SELECT * FROM LOGI_CABECERAORDENVENTA " &
                                                        "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim & "' AND " &
                                                        "      vIdNumeroSerieCabeceraOrdenVenta = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND " &
                                                        "      vIdNumeroCorrelativoCabeceraOrdenVenta= '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' AND " &
                                                        "      cIdEmpresa = '" & Session("IdEmpresa") & "' AND ISNULL(vOrdenFabricacionCabeceraOrdenVenta, '') = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim & "'")
                            Dim strEstado As String = ""
                            For Each OrdVta In dsOrdenVenta.Rows
                                strEstado = OrdVta("cEstadoCabeceraOrdenVenta")
                            Next
                            For i = 0 To Session("CestaControlDespacho").rows.count - 1
                                If (Session("CestaControlDespacho").Rows(i)("CantidadSaldo") <> 0) Then
                                    strEstado = "A"
                                    Exit For
                                    'Else
                                    '    strEstado = "T"
                                End If
                            Next
                            'NUEVO JMUG: 04/12/2023

                            OrdenVentaNeg.OrdenVentaGetData("UPDATE LOGI_CABECERAORDENVENTA SET cEstadoCabeceraOrdenVenta = '" & strEstado & "', dFechaTerminadaCabeceraOrdenVenta = GETDATE() WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND ISNULL(vOrdenFabricacionCabeceraOrdenVenta, '') = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim) & "'")


                            MyValidator.ErrorMessage = "Orden de Venta Terminada"
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "R"
            BloquearMantenimiento(True, False, False, False) 'JMUG: 15/11/2023
            btnGenerar.Text = "Ver"

            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)

            Me.grdLista.DataSource = OrdenVentaNeg.OrdenVentaListaBusqueda("(ISNULL(DOC.cEstadoCabeceraOrdenVenta, '') = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.cIdPersonal = (SELECT cIdPersonal FROM RRHH_PERSONAL WHERE vNumeroDocumentoPersonal = (SELECT vIdNroDocumentoIdentidadUsuario FROM GNRL_USUARIO WHERE cIdUsuario = '" & Session("IdUsuario") & "')) " &
                                                                           "OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND ISNULL(" & cboFiltroEquipo.SelectedValue & ", '') ", txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlControlDespacho.Visible = False
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub chkTodoConforme_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodoConforme.CheckedChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleControlDespacho.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(2).FindControl("txtValorDetalleControlDespacho"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleControlDespacho.Rows.Count * (grdDetalleControlDespacho.PageIndex))
                Session("CestaControlDespacho").Rows(FilaActual)("CantidadEncontrada") = IIf(chkTodoConforme.Checked, Session("CestaControlDespacho").Rows(FilaActual)("CantidadOrigen"), 0)
                Session("CestaControlDespacho").Rows(FilaActual)("CantidadSaldo") = IIf(chkTodoConforme.Checked, 0, Session("CestaControlDespacho").Rows(FilaActual)("CantidadOrigen"))
            Next
            Me.grdDetalleControlDespacho.DataSource = Session("CestaControlDespacho")
            Me.grdDetalleControlDespacho.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnVerListadoOrdenVenta_Click(sender As Object, e As EventArgs) Handles lnkbtnVerListadoOrdenVenta.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            txtFechaInicialOrdenVentaReporte.Text = Now.ToString("yyyy-MM-dd")
            txtFechaFinalOrdenVentaReporte.Text = Now.ToString("yyyy-MM-dd")
            chkSinIniciarOrdenVentaReporte.Checked = True
            chkAtencionParcialOrdenVentaReporte.Checked = True
            chkAtendidoOrdenVentaReporte.Checked = True
            lnk_mostrarPanelOrdenVentaReporte_ModalPopupExtender.Show()
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)

        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnVerOrdenVenta_Click(sender As Object, e As EventArgs) Handles lnkbtnVerOrdenVenta.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            'MyValidator.ErrorMessage = "Orden de Venta"
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
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

    Private Sub grdDetalleControlDespacho_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleControlDespacho.PageIndexChanging
        grdDetalleControlDespacho.PageIndex = e.NewPageIndex
        Me.grdDetalleControlDespacho.DataSource = Session("CestaControlDespacho")
        Me.grdDetalleControlDespacho.DataBind() 'Recargo el grid.
        grdDetalleControlDespacho.SelectedIndex = -1
    End Sub
End Class