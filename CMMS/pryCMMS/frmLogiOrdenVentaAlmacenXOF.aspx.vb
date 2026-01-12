Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiOrdenVentaAlmacenXOF
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
            'Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Throw New Exception("Su sesión ha caducado, ingrese de nuevo por favor.")
            'ElseIf Session("IdConfEmpresa") = "" Then
            '    fValidarSesion = True
            '    'Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
            '    Throw New Exception("No se ha ingresado al sistema correctamente.")
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
        Return dt
    End Function

    Shared Sub EditarCestaControlDespacho(ByVal Item As Int16, ByVal Codigo As String, ByVal Descripcion As String, ByVal CantidadOrigen As Decimal,
                                          ByVal CantidadEncontrada As Decimal, ByVal CantidadSaldo As Decimal,
                                          ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Item
                Tabla.Rows(Fila)(1) = Codigo
                Tabla.Rows(Fila)(2) = Descripcion
                Tabla.Rows(Fila)(3) = CantidadOrigen
                Tabla.Rows(Fila)(4) = CantidadEncontrada
                Tabla.Rows(Fila)(5) = CantidadSaldo
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaControlDespacho(ByVal Item As Int16, ByVal Codigo As String, ByVal Descripcion As String, ByVal CantidadOrigen As Decimal,
                                          ByVal CantidadEncontrada As Decimal, ByVal CantidadSaldo As Decimal,
                                          ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Item
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("CantidadOrigen") = CantidadOrigen
        Fila("CantidadEncontrada") = CantidadEncontrada
        Fila("CantidadSaldo") = CantidadSaldo
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

    Sub CargarCestaControlDespachoPrincipal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaControlDespacho(Session("CestaControlDespacho"))
            Dim dsControlDespacho = CaracteristicaNeg.CaracteristicaGetData("SELECT nIdNumeroItemDetalleOrdenFabricacion, vIdArticuloSAPDetalleOrdenFabricacion, vDescripcionArticuloSAPDetalleOrdenFabricacion, nCantidadSAPDetalleOrdenFabricacion, " &
                                                                            "       ISNULL(nCantidadAlmacenEncontradaDetalleOrdenFabricacion, 0) AS nCantidadAlmacenEncontradaDetalleOrdenFabricacion, ISNULL(nCantidadAlmacenSaldoDetalleOrdenFabricacion, nCantidadSAPDetalleOrdenFabricacion) AS nCantidadAlmacenSaldoDetalleOrdenFabricacion, ISNULL(vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion, 0) AS vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion " &
                                                                            "FROM LOGI_DETALLEORDENFABRICACION " &
                                                                            "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(1).Text & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(2).Text & "'")
            For Each ControlDespacho In dsControlDespacho.Rows
                AgregarCestaControlDespacho(ControlDespacho("nIdNumeroItemDetalleOrdenFabricacion"), ControlDespacho("vIdArticuloSAPDetalleOrdenFabricacion"), ControlDespacho("vDescripcionArticuloSAPDetalleOrdenFabricacion"), ControlDespacho("nCantidadSAPDetalleOrdenFabricacion"), ControlDespacho("nCantidadAlmacenEncontradaDetalleOrdenFabricacion"), ControlDespacho("nCantidadAlmacenSaldoDetalleOrdenFabricacion"), Session("CestaControlDespacho"))
            Next
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
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("85", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
    End Sub

    Sub ListarPersonalResponsableCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboAuxiliarMantenimientoAsesorAuxiliar.DataTextField = "vNombreCompletoPersonal"
        cboAuxiliarMantenimientoAsesorAuxiliar.DataValueField = "cIdPersonal"
        cboAuxiliarMantenimientoAsesorAuxiliar.DataSource = ResponsableNeg.PersonalListarCombo()
        cboAuxiliarMantenimientoAsesorAuxiliar.Items.Clear()
        cboAuxiliarMantenimientoAsesorAuxiliar.Items.Add("SELECCIONE DATO")
        cboAuxiliarMantenimientoAsesorAuxiliar.DataBind()
    End Sub

    Sub ListarUnidadTrabajoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UniTraNeg As New clsOrdenFabricacionNegocios
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataTextField = "vIdUnidadTrabajoOrdenFabricacion"
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataValueField = "vIdUnidadTrabajoOrdenFabricacion"
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataSource = UniTraNeg.OrdenFabricacionUnidadTrabajoListarCombo("*")
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.Items.Clear()
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.Items.Add("SELECCIONE DATO")
        cboUnidadTrabajoMantenimientoAsesorAuxiliar.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Enabled = bActivar
        cboAuxiliarMantenimientoAsesorAuxiliar.Enabled = bActivar
    End Sub

    Sub LlenarData()
        Dim SolicitudServicio As LOGI_CABECERAORDENFABRICACION = OrdenFabricacionNeg.OrdenFabricacionListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Session("IdEmpresa"))
        'txtDescripcionOrden.Text = SolicitudServicio.vDescripcionCabeceraOrdenFabricacion
        lblNroOrden.Text = SolicitudServicio.vOrdenFabricacionCabeceraOrdenFabricacion
        lblNroOrdenVenta.Text = SolicitudServicio.vOrdenVentaCabeceraOrdenFabricacion
        lblDescripcionOrden.Text = SolicitudServicio.vDescripcionCabeceraOrdenFabricacion
        Dim CliNeg As New clsClienteNegocios
        Dim Cliente As GNRL_CLIENTE = CliNeg.ClienteListarPorId(SolicitudServicio.cIdCliente, Session("IdEmpresa"), "*")
        lblRucCliente.Text = Cliente.vRucCliente
        lblRazonSocialCliente.Text = Cliente.vRazonSocialCliente

        CargarCestaControlDespachoPrincipal()

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
        'Me.btnEditar.Visible = bEditar
        'Me.btnGuardar.Visible = bGuardar
        'Me.btnDeshacer.Visible = bDeshacer
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

    'Sub LimpiarObjetosCaracteristicas()
    '    Me.lblMensajeCaracteristica.Text = ""
    '    txtIdReferenciaSAPCaracteristica.Text = ""
    '    txtDescripcionCampoSAPCaracteristica.Text = ""
    'End Sub

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
            cboFiltroEquipo.SelectedIndex = 4
            ListarPersonalResponsableCombo()
            ListarUnidadTrabajoCombo()

            If Session("CestaControlDespacho") Is Nothing Then
                Session("CestaControlDespacho") = CrearCestaControlDespacho()
            Else
                VaciarCestaControlDespacho(Session("CestaControlDespacho"))
            End If

            BloquearMantenimiento(True, False, True, False)

            OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'R' WHERE cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion = '" & Session("IdUsuario") & "'")
            'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
            Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
            'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
            Me.grdLista.DataBind()
            pnlCabecera.Visible = True
            pnlControlDespacho.Visible = False
        Else
            txtBuscarEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
        End If
    End Sub

    Protected Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0652", strOpcionModulo, "CMMS")

            If IsNothing(grdLista.SelectedRow) = False Then
                If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                    hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0")
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
                            hfdIdEquipoSAPEquipo.Value = Server.HtmlDecode(grdLista.SelectedRow.Cells(18).Text) 'JMUG: 21/09/2023
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0") 'JMUG: 21/09/2023
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
            e.Row.Cells(8).Visible = True 'DescripcionOrdenFabricacion
            e.Row.Cells(9).Visible = True 'CantidadItems
            e.Row.Cells(10).Visible = False 'IdCliente
            e.Row.Cells(11).Visible = False 'IdClienteSAP
            e.Row.Cells(12).Visible = True 'Cliente
            e.Row.Cells(13).Visible = True 'NombreCompletoAuxiliar
            e.Row.Cells(14).Visible = True 'FechaAsignacionAuxiliar
            e.Row.Cells(15).Visible = False 'FechaProgramacionAlmacen
            e.Row.Cells(16).Visible = True 'StatusOrdenFabricacion
            e.Row.Cells(17).Visible = False 'Estado
            e.Row.Cells(18).Visible = False 'IdEquipoSAP
            e.Row.Cells(19).Visible = False 'IdUnidadTrabajo
            e.Row.Cells(20).Visible = False 'IdEquipo
            e.Row.Cells(21).Visible = False 'NumeroSerieEquipo
            e.Row.Cells(22).Visible = False 'IdArticuloSAPCabecera
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
            e.Row.Cells(8).Visible = True 'DescripcionOrdenFabricacion
            e.Row.Cells(9).Visible = True 'CantidadItems
            e.Row.Cells(10).Visible = False 'IdCliente
            e.Row.Cells(11).Visible = False 'IdClienteSAP
            e.Row.Cells(12).Visible = True 'Cliente
            e.Row.Cells(13).Visible = True 'NombreCompletoAuxiliar
            e.Row.Cells(14).Visible = True 'FechaAsignacionAuxiliar
            e.Row.Cells(15).Visible = False 'FechaProgramacionAlmacen
            e.Row.Cells(16).Visible = True 'StatusOrdenFabricacion
            e.Row.Cells(17).Visible = False 'Estado
            e.Row.Cells(18).Visible = False 'IdEquipoSAP
            e.Row.Cells(19).Visible = False 'IdUnidadTrabajo
            e.Row.Cells(20).Visible = False 'IdEquipo
            e.Row.Cells(21).Visible = False 'NumeroSerieEquipo
            e.Row.Cells(22).Visible = False 'IdArticuloSAPCabecera
        End If
    End Sub

    Private Sub imgbtnBuscarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarEquipo.Click
        'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("DOC.cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        Me.grdLista.DataBind()
    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                'Dim Catalogo As New LOGI_CATALOGO
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

                'CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                'Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
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

                'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataBind()
            ElseIf e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Equipo As New LOGI_EQUIPO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                'Catalogo.cIdCatalogo = Valores(0).ToString()
                'Catalogo.cIdTipoActivo = Valores(1).ToString()
                'Catalogo.cIdJerarquiaCatalogo = "0"
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

                'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
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
        'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("cIdTipoDocumentoCabeceraOrdenFabricacion = 'OS' AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
        Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
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
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'DescripcionOrdenFabricacion
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'CantidadItems
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'IdCliente
                e.Row.Cells.Item(11).HorizontalAlign = HorizontalAlign.Left 'IdClienteSAP
                e.Row.Cells.Item(12).HorizontalAlign = HorizontalAlign.Left 'Cliente
                e.Row.Cells.Item(13).HorizontalAlign = HorizontalAlign.Left 'NombreCompletoAuxiliar
                e.Row.Cells.Item(14).HorizontalAlign = HorizontalAlign.Left 'FechaAsignacionAuxiliar
                e.Row.Cells.Item(15).HorizontalAlign = HorizontalAlign.Left 'FechaProgramacionAlmacen
                e.Row.Cells.Item(16).HorizontalAlign = HorizontalAlign.Left 'StatusOrdenFabricacion
                e.Row.Cells.Item(17).HorizontalAlign = HorizontalAlign.Left 'Estado
                e.Row.Cells.Item(18).HorizontalAlign = HorizontalAlign.Left 'IdEquipoSAP
                e.Row.Cells.Item(19).HorizontalAlign = HorizontalAlign.Left 'IdUnidadTrabajo
                e.Row.Cells.Item(20).HorizontalAlign = HorizontalAlign.Left 'IdEquipo
                e.Row.Cells.Item(21).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                e.Row.Cells.Item(22).HorizontalAlign = HorizontalAlign.Left 'IdArticuloSAPCabecera
            Next
        End If
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0651", strOpcionModulo, "CMMS")
            'EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdEquipo = '" & txtIdEquipo.Text & "'")

            pnlCabecera.Visible = True
            pnlControlDespacho.Visible = False
            'pnlComponentes.Visible = False
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
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0653", strOpcionModulo, "CMMS")

            Dim coleccion As New List(Of LOGI_DETALLEORDENFABRICACION)
            For i = 0 To Session("CestaControlDespacho").rows.count - 1
                Dim DetOrdFab As New LOGI_DETALLEORDENFABRICACION
                DetOrdFab.cIdTipoDocumentoCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim
                DetOrdFab.vIdNumeroSerieCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim
                DetOrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim
                DetOrdFab.cIdEmpresa = Session("IdEmpresa")
                DetOrdFab.nIdNumeroItemDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("Item").ToString.Trim
                'DetOrdFab.vIdArticuloSAPCabeceraOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("IdEquipo").ToString.Trim
                DetOrdFab.cIdEquipoSAPCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(18).Text).Trim
                DetOrdFab.vIdArticuloSAPDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("Codigo").ToString.Trim
                'DetOrdFab.vDescripcionArticuloSAPDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("IdEquipo").ToString.Trim
                'DetOrdFab.nCantidadSAPDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("CantidadOrigen").ToString.Trim
                'DetOrdFab.vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("Descripcion").ToString.Trim
                'DetOrdFab.nTotalCantidadConsumidoDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("IdEquipo").ToString.Trim
                DetOrdFab.nCantidadAlmacenEncontradaDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("CantidadEncontrada").ToString.Trim
                DetOrdFab.nCantidadAlmacenSaldoDetalleOrdenFabricacion = Session("CestaControlDespacho").Rows(i)("CantidadSaldo").ToString.Trim
                coleccion.Add(DetOrdFab)
            Next

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
                If OrdenFabricacionNeg.OrdenFabricacionActualizarDisponibilidadStock(coleccion, LogAuditoria) = 0 Then
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                    Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                    Me.grdLista.DataBind()

                    pnlCabecera.Visible = True
                    pnlControlDespacho.Visible = False

                    BloquearMantenimiento(True, False, True, False)
                    hfdOperacion.Value = "R"
                    txtBuscarEquipo.Focus()
                End If
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

    Sub txtValorDetalleControlDespacho_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleControlDespacho.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(2).FindControl("txtValorDetalleControlDespacho"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleControlDespacho.Rows.Count * (grdDetalleControlDespacho.PageIndex))
                'Session("CestaControlDespacho").Rows(FilaActual)("CantidadOrigen") = txtValorDetalle.Text
                Session("CestaControlDespacho").Rows(FilaActual)("CantidadEncontrada") = txtValorDetalle.Text
                Session("CestaControlDespacho").Rows(FilaActual)("CantidadSaldo") = Session("CestaControlDespacho").Rows(FilaActual)("CantidadOrigen") - Convert.ToDecimal(txtValorDetalle.Text)
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

    Private Sub btnAceptarMantenimientoAsesorAuxiliar_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoAsesorAuxiliar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0676", strOpcionModulo, "CMMS")

            If txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de inicio del control.")
                'ElseIf cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                '    Throw New Exception("Debe de seleccionar un tipo de mantenimiento.")
                'ElseIf cboAuxiliarMantenimientoAsesorAuxiliar.SelectedIndex = 0 Then
                '    Throw New Exception("Debe de seleccionar un asesor auxiliar.")
            End If

            'OrdenFabricacionNeg.OrdenFabricacionGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoSAPCabeceraOrdenFabricacion = '" & hfdIdEquipoSAPEquipo.Value & "' AND vIdArticuloSAPCabeceraOrdenFabricacion = '" & hfdIdArticuloSAPPrincipal.Value & "'")
            Dim OrdFab As New LOGI_CABECERAORDENFABRICACION
            OrdFab.cIdTipoDocumentoCabeceraOrdenFabricacion = grdLista.SelectedRow.Cells(0).Text
            OrdFab.vIdNumeroSerieCabeceraOrdenFabricacion = grdLista.SelectedRow.Cells(1).Text 'Year(Now).ToString
            OrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion = grdLista.SelectedRow.Cells(2).Text
            OrdFab.cIdEmpresa = Session("IdEmpresa")
            OrdFab.cIdEquipoSAPCabeceraOrdenFabricacion = hfdIdEquipoSAPEquipo.Value
            'OrdFab.vIdArticuloSAPCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(14).Text).Trim 'JMUG: 21/09/2023
            OrdFab.vIdArticuloSAPCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(22).Text).Trim 'JMUG: 21/09/2023
            OrdFab.dFechaTransaccionCabeceraOrdenFabricacion = Now
            OrdFab.vIdClienteSAPCabeceraOrdenFabricacion = hfdIdClienteSAPEquipo.Value
            OrdFab.cIdCliente = Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text).Trim

            'Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            'Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(9).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
            'OrdFab.cIdEquipo = Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim
            OrdFab.cIdEquipo = IIf(Trim(Server.HtmlDecode(grdLista.SelectedRow.Cells(20).Text)) = "", Nothing, Server.HtmlDecode(grdLista.SelectedRow.Cells(20).Text).Trim)

            'OrdFab.vNumeroSerieEquipoCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim 'JMUG_ 18/10/2023
            OrdFab.vNumeroSerieEquipoCabeceraOrdenFabricacion = Server.HtmlDecode(grdLista.SelectedRow.Cells(21).Text).Trim 'JMUG_ 18/10/2023
            OrdFab.vOrdenVentaCabeceraOrdenFabricacion = txtOrdenVentaMantenimientoAsesorAuxiliar.Text
            OrdFab.vOrdenCompraCabeceraOrdenFabricacion = txtOrdenCompraMantenimientoAsesorAuxiliar.Text
            OrdFab.cEstadoCabeceraOrdenFabricacion = "R"
            OrdFab.bEstadoRegistroCabeceraOrdenFabricacion = True
            OrdFab.dFechaUltimaModificacionCabeceraOrdenFabricacion = Now
            OrdFab.dFechaCreacionCabeceraOrdenFabricacion = hfdFechaCreacionEquipo.Value
            OrdFab.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion = Session("IdUsuario")
            OrdFab.cIdUsuarioCreacionCabeceraOrdenFabricacion = hfdIdUsuarioCreacionEquipo.Value
            OrdFab.vOrdenFabricacionCabeceraOrdenFabricacion = grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
            OrdFab.dFechaEmisionCabeceraOrdenFabricacion = grdLista.SelectedRow.Cells(7).Text
            OrdFab.vIdUnidadTrabajoOrdenFabricacion = cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedValue
            'OrdFab.dFechaProgramacionAlmacenCabeceraOrdenFabricacion = txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text
            Dim hora As String = cboHorasMantenimientoAsesorAuxiliar.SelectedValue
            Dim minutos As String = cboMinutosMantenimientoAsesorAuxiliar.SelectedValue
            Dim segundos As String = cboSegundosMantenimientoAsesorAuxiliar.SelectedValue
            Dim fecha As String = txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text

            'Concatenar los valores en una cadena en formato SQL
            Dim strFechaHoraProgramacionAlmacen As String = fecha & " " & hora & ":" & minutos & ":" & segundos
            OrdFab.dFechaProgramacionAlmacenCabeceraOrdenFabricacion = strFechaHoraProgramacionAlmacen

            OrdFab.cIdPersonal = cboAuxiliarMantenimientoAsesorAuxiliar.SelectedValue
            OrdFab.dFechaOrdenVentaCabeceraOrdenFabricacion = grdLista.SelectedRow.Cells(5).Text
            OrdFab.dFechaAsignacionAuxiliarCabeceraOrdenFabricacion = grdLista.SelectedRow.Cells(14).Text

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

            If OrdenFabricacionNeg.OrdenFabricacionEdita(OrdFab) = 0 Then
                Session("Query") = "PA_LOGI_MNT_CABECERAORDENFABRICACION 'SQL_UPDATE', '', '" & OrdFab.cIdTipoDocumentoCabeceraOrdenFabricacion & "', '" & OrdFab.vIdNumeroSerieCabeceraOrdenFabricacion & "', '" &
                                           OrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion & "', '" & OrdFab.cIdEmpresa & "', '" & OrdFab.cIdEquipoSAPCabeceraOrdenFabricacion & "', '" &
                                           OrdFab.vIdArticuloSAPCabeceraOrdenFabricacion & "', '" & OrdFab.dFechaTransaccionCabeceraOrdenFabricacion & "', '" & OrdFab.vIdClienteSAPCabeceraOrdenFabricacion & "', '" & OrdFab.cIdCliente & "', '" &
                                           OrdFab.cIdEquipo & "', '" & OrdFab.vNumeroSerieEquipoCabeceraOrdenFabricacion & "', '" & OrdFab.vOrdenVentaCabeceraOrdenFabricacion & "', '" & OrdFab.vOrdenCompraCabeceraOrdenFabricacion & "', '" & OrdFab.cEstadoCabeceraOrdenFabricacion & "', '" &
                                           OrdFab.bEstadoRegistroCabeceraOrdenFabricacion & "', '" & OrdFab.dFechaUltimaModificacionCabeceraOrdenFabricacion & "', '" & OrdFab.dFechaCreacionCabeceraOrdenFabricacion & "', '" &
                                           OrdFab.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion & "', '" & OrdFab.cIdUsuarioCreacionCabeceraOrdenFabricacion & "', '" &
                                           OrdFab.dFechaEmisionCabeceraOrdenFabricacion & "', '" & OrdFab.vIdUnidadTrabajoOrdenFabricacion & "', '" & OrdFab.dFechaProgramacionAlmacenCabeceraOrdenFabricacion & "', '" & OrdFab.cIdPersonal & "', '" & OrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion

                LogAuditoria.vEvento = "ACTUALIZAR ORDEN FABRICACION"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                MyValidator.ErrorMessage = "Registro actualizado con éxito"
                'Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
                Me.grdLista.DataSource = OrdenFabricacionNeg.OrdenFabricacionListaBusqueda("(vIdUnidadTrabajoOrdenFabricacion IN (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "') OR '*' = (SELECT vIdUnidadTrabajoUsuario FROM GNRL_USUARIO AS USU WHERE USU.cIdUsuario = '" & Session("IdUsuario") & "')) AND " & cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, Session("IdEmpresa"), "1")
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

    'Private Sub lnkbtnAsignarAsesor_Click(sender As Object, e As EventArgs) Handles lnkbtnAsignarAsesor.Click
    '    Try
    '        If grdLista IsNot Nothing Then
    '            If grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = False Then
    '                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        lblDescripcionMantenimientoAsesorAuxiliar.Text = ""
    '                        'txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = String.Format("{0:dd/MM/yyyy}", Now)

    '                        cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedIndex = -1
    '                        txtOrdenVentaMantenimientoAsesorAuxiliar.Text = ""
    '                        txtOrdenCompraMantenimientoAsesorAuxiliar.Text = ""
    '                        txtOrdenFabricacionMantenimientoAsesorAuxiliar.Text = ""
    '                        hfdIdEquipoSAPEquipo.Value = ""
    '                        hfdIdClienteSAPEquipo.Value = ""
    '                        hfdEstado.Value = "1"

    '                        Dim EquipoNeg As New clsEquipoNegocios
    '                        Dim OFNeg As New clsOrdenFabricacionNegocios
    '                        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
    '                        'Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(9).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
    '                        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
    '                        Dim OrdFab As LOGI_CABECERAORDENFABRICACION = OFNeg.OrdenFabricacionListarPorId(row.Cells(0).Text, row.Cells(1).Text, row.Cells(2).Text, Session("IdEmpresa"))
    '                        'lblDescripcionMantenimientoAsesorAuxiliar.Text = Equipo.vDescripcionEquipo
    '                        lblDescripcionMantenimientoAsesorAuxiliar.Text = OrdFab.vDescripcionCabeceraOrdenFabricacion
    '                        ListarTiempoCombo()


    '                        'Dim ahora As DateTime = DateTime.Now

    '                        '' Asignar la fecha y hora actual al TextBox de fecha
    '                        'txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = ahora.ToString("dd/MM/yyyy")

    '                        '' Obtener la hora actual y asignarla a los DropDownList de horas, minutos y segundos
    '                        'Dim hora As Integer = ahora.Hour
    '                        'Dim minuto As Integer = ahora.Minute
    '                        'Dim segundo As Integer = ahora.Second

    '                        'cboHorasMantenimientoAsesorAuxiliar.SelectedValue = hora.ToString("00")
    '                        'cboMinutosMantenimientoAsesorAuxiliar.SelectedValue = minuto.ToString("00")
    '                        'cboSegundosMantenimientoAsesorAuxiliar.SelectedValue = segundo.ToString("00")
    '                        'If Convert.ToInt16(cboHorasMantenimientoAsesorAuxiliar.SelectedValue) >= 12 Then
    '                        '    cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "PM"
    '                        'Else
    '                        '    cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "AM"
    '                        'End If




    '                        Dim ahora As DateTime
    '                        ahora = IIf(IsNothing(OrdFab.dFechaProgramacionAlmacenCabeceraOrdenFabricacion) = True, DateTime.Now, OrdFab.dFechaProgramacionAlmacenCabeceraOrdenFabricacion)

    '                        txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = ahora.ToString("dd/MM/yyyy")

    '                        ' Obtener la hora actual y asignarla a los DropDownList de horas, minutos y segundos
    '                        Dim hora As Integer = ahora.Hour
    '                        Dim minuto As Integer = ahora.Minute
    '                        Dim segundo As Integer = ahora.Second

    '                        cboHorasMantenimientoAsesorAuxiliar.SelectedValue = hora.ToString("00")
    '                        cboMinutosMantenimientoAsesorAuxiliar.SelectedValue = minuto.ToString("00")
    '                        cboSegundosMantenimientoAsesorAuxiliar.SelectedValue = segundo.ToString("00")
    '                        If Convert.ToInt16(cboHorasMantenimientoAsesorAuxiliar.SelectedValue) >= 12 Then
    '                            cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "PM"
    '                        Else
    '                            cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "AM"
    '                        End If
    '                        cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedValue = row.Cells(8).Text
    '                        txtOrdenVentaMantenimientoAsesorAuxiliar.Text = OrdFab.vOrdenVentaCabeceraOrdenFabricacion
    '                        txtOrdenCompraMantenimientoAsesorAuxiliar.Text = OrdFab.vOrdenCompraCabeceraOrdenFabricacion
    '                        txtOrdenFabricacionMantenimientoAsesorAuxiliar.Text = OrdFab.vOrdenFabricacionCabeceraOrdenFabricacion

    '                        hfdFechaCreacionEquipo.Value = IIf(IsNothing(OrdFab.dFechaCreacionCabeceraOrdenFabricacion), Now, OrdFab.dFechaCreacionCabeceraOrdenFabricacion)
    '                        hfdIdUsuarioCreacionEquipo.Value = IIf(IsNothing(OrdFab.cIdUsuarioCreacionCabeceraOrdenFabricacion), Session("IdUsuario"), OrdFab.cIdUsuarioCreacionCabeceraOrdenFabricacion)
    '                        hfdIdEquipoSAPEquipo.Value = Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text) 'JMUG: 21/09/2023
    '                        hfdIdClienteSAPEquipo.Value = Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text) 'JMUG: 18/10/2023
    '                        hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(14).Text) = "True", "1", "0") 'JMUG: 21/09/2023
    '                        lnk_mostrarPanelMantenimientoAsesorAuxiliar_ModalPopupExtender.Show()
    '                    End If
    '                Else
    '                    Throw New Exception("Seleccione un item para asignar un asesor.")
    '                End If
    '            Else
    '                Throw New Exception("Seleccione un equipo.")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

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
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            lblDescripcionMantenimientoAsesorAuxiliar.Text = ""
                            'txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = String.Format("{0:dd/MM/yyyy}", Now)

                            cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedIndex = -1
                            txtOrdenVentaMantenimientoAsesorAuxiliar.Text = ""
                            txtOrdenCompraMantenimientoAsesorAuxiliar.Text = ""
                            txtOrdenFabricacionMantenimientoAsesorAuxiliar.Text = ""
                            hfdIdEquipoSAPEquipo.Value = ""
                            hfdIdClienteSAPEquipo.Value = ""
                            hfdEstado.Value = "1"

                            Dim EquipoNeg As New clsEquipoNegocios
                            Dim OFNeg As New clsOrdenFabricacionNegocios
                            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
                            'Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(9).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
                            'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim)
                            Dim OrdFab As LOGI_CABECERAORDENFABRICACION = OFNeg.OrdenFabricacionListarPorId(row.Cells(0).Text, row.Cells(1).Text, row.Cells(2).Text, Session("IdEmpresa"))
                            'lblDescripcionMantenimientoAsesorAuxiliar.Text = Equipo.vDescripcionEquipo
                            lblDescripcionMantenimientoAsesorAuxiliar.Text = OrdFab.vDescripcionCabeceraOrdenFabricacion
                            ListarTiempoCombo()


                            'Dim ahora As DateTime = DateTime.Now

                            '' Asignar la fecha y hora actual al TextBox de fecha
                            'txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = ahora.ToString("dd/MM/yyyy")

                            '' Obtener la hora actual y asignarla a los DropDownList de horas, minutos y segundos
                            'Dim hora As Integer = ahora.Hour
                            'Dim minuto As Integer = ahora.Minute
                            'Dim segundo As Integer = ahora.Second

                            'cboHorasMantenimientoAsesorAuxiliar.SelectedValue = hora.ToString("00")
                            'cboMinutosMantenimientoAsesorAuxiliar.SelectedValue = minuto.ToString("00")
                            'cboSegundosMantenimientoAsesorAuxiliar.SelectedValue = segundo.ToString("00")
                            'If Convert.ToInt16(cboHorasMantenimientoAsesorAuxiliar.SelectedValue) >= 12 Then
                            '    cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "PM"
                            'Else
                            '    cboMeridianoMantenimientoAsesorAuxiliar.SelectedValue = "AM"
                            'End If




                            Dim ahora As DateTime
                            ahora = IIf(IsNothing(OrdFab.dFechaProgramacionAlmacenCabeceraOrdenFabricacion) = True, DateTime.Now, OrdFab.dFechaProgramacionAlmacenCabeceraOrdenFabricacion)

                            txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.Text = ahora.ToString("dd/MM/yyyy")

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
                            cboUnidadTrabajoMantenimientoAsesorAuxiliar.SelectedValue = Server.HtmlDecode(row.Cells(19).Text).Trim
                            txtOrdenVentaMantenimientoAsesorAuxiliar.Text = OrdFab.vOrdenVentaCabeceraOrdenFabricacion
                            txtOrdenCompraMantenimientoAsesorAuxiliar.Text = OrdFab.vOrdenCompraCabeceraOrdenFabricacion
                            txtOrdenFabricacionMantenimientoAsesorAuxiliar.Text = OrdFab.vOrdenFabricacionCabeceraOrdenFabricacion

                            hfdFechaCreacionEquipo.Value = IIf(IsNothing(OrdFab.dFechaCreacionCabeceraOrdenFabricacion), Now, OrdFab.dFechaCreacionCabeceraOrdenFabricacion)
                            hfdIdUsuarioCreacionEquipo.Value = IIf(IsNothing(OrdFab.cIdUsuarioCreacionCabeceraOrdenFabricacion), Session("IdUsuario"), OrdFab.cIdUsuarioCreacionCabeceraOrdenFabricacion)
                            hfdIdEquipoSAPEquipo.Value = Server.HtmlDecode(grdLista.SelectedRow.Cells(18).Text) 'JMUG: 21/09/2023
                            hfdIdClienteSAPEquipo.Value = Server.HtmlDecode(grdLista.SelectedRow.Cells(11).Text) 'JMUG: 18/10/2023
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0") 'JMUG: 21/09/2023
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
End Class