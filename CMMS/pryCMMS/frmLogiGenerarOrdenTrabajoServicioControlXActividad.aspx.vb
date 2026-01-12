Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarOrdenTrabajoServicioControlXActividad
    Inherits System.Web.UI.Page
    Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios
    'Dim PersonalNeg As New clsPersonalNegocios
    Dim DetalleOrdenTrabajoNeg As New clsDetalleOrdenTrabajoNegocios
    'Dim CatalogoNeg As New clsCatalogoNegocios
    'Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    'Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator
    'Shared rowIndexDetalle As Int64

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

    Shared Function CrearCestaOrdenTrabajo() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Personal", GetType(System.String))) '2
        Return dt
    End Function

    Shared Sub EditarCestaOrdenTrabajo(ByVal Codigo As String, ByVal Personal As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        'ByVal FecVen As System.Nullable(Of DateTime), ByVal CentroCosto As String, _
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                'Tabla.Rows(Fila)(0) = xxx 'Numero de Linea
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Personal
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaOrdenTrabajo(ByVal Codigo As String, ByVal Personal As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Personal") = Personal
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaOrdenTrabajo(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaOrdenTrabajo(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Function CrearCestaInsumos() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '0
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Cantidad", GetType(System.Decimal))) '2
        dt.Columns.Add(New DataColumn("DescripcionUnidadMedida", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdActividad", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("IdArticuloSAP", GetType(System.String))) '8
        Return dt
    End Function

    Shared Sub EditarCestaInsumos(ByVal Codigo As String, ByVal Descripcion As String, ByVal Cantidad As Decimal,
                           ByVal DescripcionUnidadMedida As String, ByVal IdEquipo As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticuloSAP As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        'ByVal FecVen As System.Nullable(Of DateTime), ByVal CentroCosto As String, _
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Descripcion
                Tabla.Rows(Fila)(2) = Cantidad
                Tabla.Rows(Fila)(3) = DescripcionUnidadMedida
                Tabla.Rows(Fila)(4) = IdEquipo
                Tabla.Rows(Fila)(5) = IdCatalogo
                Tabla.Rows(Fila)(6) = IdJerarquia
                Tabla.Rows(Fila)(7) = IdActividad
                Tabla.Rows(Fila)(8) = IdArticuloSAP
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaInsumos(ByVal Codigo As String, ByVal Descripcion As String, ByVal Cantidad As Decimal,
                           ByVal DescripcionUnidadMedida As String, ByVal IdEquipo As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticuloSAP As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("Cantidad") = Cantidad
        Fila("DescripcionUnidadMedida") = DescripcionUnidadMedida
        Fila("IdEquipo") = IdEquipo
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdJerarquia") = IdJerarquia
        Fila("IdActividad") = IdActividad
        Fila("IdArticuloSAP") = IdArticuloSAP
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaInsumos(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaInsumos(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Function CrearCestaOtrosDatos() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Item", GetType(System.Int32))) '0 - Nro.Item
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1 - IdCaracteristica
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2 - Descripcion Caracteristica
        dt.Columns.Add(New DataColumn("ValorReferencial", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdActividad", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("IdArticuloSAP", GetType(System.String))) '8
        dt.Columns.Add(New DataColumn("IdEquipoCheckList", GetType(System.String))) '9
        Return dt
    End Function

    Shared Sub EditarCestaOtrosDatos(ByVal Codigo As String, ByVal Descripcion As String, ByVal ValorReferencial As String,
                           ByVal IdEquipo As String, ByVal IdActividad As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdArticuloSAP As String, ByVal IdEquipoCheckList As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        'ByVal FecVen As System.Nullable(Of DateTime), ByVal CentroCosto As String, _
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                'Tabla.Rows(Fila)(0) = Codigo
                'Tabla.Rows(Fila)(1) = Descripcion
                'Tabla.Rows(Fila)(2) = ValorReferencial
                'Tabla.Rows(Fila)(3) = IdEquipo
                'Tabla.Rows(Fila)(4) = IdActividad
                'Tabla.Rows(Fila)(5) = IdCatalogo
                'Tabla.Rows(Fila)(6) = IdJerarquia
                'Tabla.Rows(Fila)(7) = IdArticuloSAP
                'Tabla.Rows(Fila)(8) = IdEquipoCheckList
                Tabla.Rows(Fila)(1) = Codigo
                Tabla.Rows(Fila)(2) = Descripcion
                Tabla.Rows(Fila)(3) = ValorReferencial
                Tabla.Rows(Fila)(4) = IdEquipo
                Tabla.Rows(Fila)(5) = IdActividad
                Tabla.Rows(Fila)(6) = IdCatalogo
                Tabla.Rows(Fila)(7) = IdJerarquia
                Tabla.Rows(Fila)(8) = IdArticuloSAP
                Tabla.Rows(Fila)(9) = IdEquipoCheckList
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaOtrosDatos(ByVal Codigo As String, ByVal Descripcion As String, ByVal ValorReferencial As String,
                           ByVal IdEquipo As String, ByVal IdActividad As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdArticuloSAP As String, ByVal IdEquipoCheckList As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Tabla.Rows.Count + 1
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("ValorReferencial") = ValorReferencial
        Fila("IdEquipo") = IdEquipo
        Fila("IdActividad") = IdActividad
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdJerarquia") = IdJerarquia
        Fila("IdArticuloSAP") = IdArticuloSAP
        Fila("IdEquipoCheckList") = IdEquipoCheckList
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaOtrosDatos(ByVal Fila As Integer, ByVal Tabla As DataTable)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows.RemoveAt(Fila)
                Dim i As Integer
                For i = 0 To Tabla.Rows.Count - 1
                    Tabla.Rows(i).BeginEdit()
                    Tabla.Rows(i)(0) = i + 1
                    Tabla.Rows(i).EndEdit()
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub VaciarCestaOtrosDatos(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltroOrdenTrabajo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroOrdenTrabajo.DataValueField = "vValor"
        cboFiltroOrdenTrabajo.DataSource = FiltroNeg.TablaSistemaListarCombo("86", "LOGI", Session("IdEmpresa"))
        cboFiltroOrdenTrabajo.Items.Clear()
        cboFiltroOrdenTrabajo.DataBind()
    End Sub

    Sub ListarPersonalAsignadoCombo(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO)
        Dim PersonalNeg As New clsOrdenTrabajoNegocios
        cboPersonalAsignadoMantenimientoTarea.DataTextField = "vNombreCompletoPersonal"
        cboPersonalAsignadoMantenimientoTarea.DataValueField = "cIdPersonal"
        cboPersonalAsignadoMantenimientoTarea.DataSource = PersonalNeg.OrdenTrabajoRecursosListarCombo(OrdTra)
        cboPersonalAsignadoMantenimientoTarea.Items.Clear()
        cboPersonalAsignadoMantenimientoTarea.Items.Add("SELECCIONE DATO")
        cboPersonalAsignadoMantenimientoTarea.DataBind()
    End Sub

    Sub ListarInsumosCombo()

        Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
        Dim InsumosNeg As New clsDetalleOrdenTrabajoNegocios
        cboInsumosAgregarInsumos.DataTextField = "vDescripcionArticuloSAPDetalleOrdenFabricacion"
        cboInsumosAgregarInsumos.DataValueField = "vIdArticuloSAPDetalleOrdenFabricacion"
        cboInsumosAgregarInsumos.DataSource = InsumosNeg.DetalleOrdenTrabajoGetData("SELECT * FROM LOGI_DETALLEORDENFABRICACION " &
                                                                      "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & ValoresOS(0).ToString & "' " &
                                                                      "      AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & ValoresOS(1).ToString & "' " &
                                                                      "      AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & ValoresOS(2).ToString & "' " &
                                                                      "      AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
        cboInsumosAgregarInsumos.Items.Clear()
        cboInsumosAgregarInsumos.Items.Add("SELECCIONE DATO")
        cboInsumosAgregarInsumos.DataBind()
    End Sub

    Sub ListarTiempoCombo()
        'Llena los DropDownList de horas, minutos y segundos
        cboHorasMantenimientoTarea.Items.Clear()
        cboMinutosMantenimientoTarea.Items.Clear()
        cboSegundosMantenimientoTarea.Items.Clear()

        For i = 0 To 23
            'cboHorasMantenimientoTarea.Items.Add(New ListItem("TODOS(*)", "*"))
            cboHorasMantenimientoTarea.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next

        For i = 0 To 59
            cboMinutosMantenimientoTarea.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
            cboSegundosMantenimientoTarea.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next
        cboMeridianoMantenimientoTarea.Enabled = False
    End Sub

    Sub ListarOtrosDatosCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaOtrosDatos.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaOtrosDatos.DataValueField = "cIdCaracteristica"
        cboCaracteristicaOtrosDatos.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaOtrosDatos.Items.Clear()
        cboCaracteristicaOtrosDatos.DataBind()
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        'Me.btnEditar.Visible = bEditar
        'Me.btnGuardar.Visible = bGuardar
        'Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetosOtrosDatos()

        'Me.lblMensajeCaracteristica.Text = ""
        'txtValorCaracteristica.Text = ""
        'txtIdReferenciaSAPCaracteristica.Text = ""
        'txtDescripcionCampoSAPCaracteristica.Text = ""
        'divIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
        'txtIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
        'divDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        'txtDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    End Sub

    Sub LlenarDataEquipo()
        'Try 'JMUG: LO QUITE 11/09/2023
        Dim EquipoNeg As New clsEquipoNegocios
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(grdLista.SelectedRow.Cells(8).Text)
        lblIdEquipo.Text = Equipo.cIdEquipo
        'hfdFechaEquipo.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
        lblDescripcionEquipo.Text = Equipo.vDescripcionEquipo
        lblNroSerieEquipo.Text = Equipo.vNumeroSerieEquipo
        lblNroParteEquipo.Text = Equipo.vNumeroParteEquipo
        lnkEsComponenteOn.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", False, True)
        lnkEsComponenteOff.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", True, False)
        divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        Dim TipActNeg As New clsTipoActivoNegocios
        lblTipoActivoEquipo.Text = TipActNeg.TipoActivoListarPorId(Equipo.cIdTipoActivo).vDescripcionTipoActivo
        Dim CatNeg As New clsCatalogoNegocios
        lblCatalogoEquipo.Text = CatNeg.CatalogoListarPorId(Equipo.cIdCatalogo, Equipo.cIdTipoActivo, Equipo.cIdJerarquiaCatalogo, "1").vDescripcionCatalogo
        Dim SisFunNeg As New clsSistemaFuncionalNegocios
        lblSistemaFuncionalEquipo.Text = SisFunNeg.SistemaFuncionalListarPorId(Equipo.cIdSistemaFuncionalEquipo).vDescripcionSistemaFuncional
        Dim ClienteNeg As New clsClienteNegocios
        Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorIdSAP(Equipo.vIdClienteSAPEquipo, Session("IdEmpresa"))
        'txtIdCliente.Text = Cliente.vRucCliente
        'btnAdicionarCliente_Click(btnAdicionarCliente, New System.EventArgs())
        'ListarClienteUbicacionCombo()
        'cboClienteUbicacion.SelectedValue = IIf(Trim(Equipo.cIdClienteUbicacion) = "", "SELECCIONE DATO", Equipo.cIdClienteUbicacion)
        'cboTipoActivo.SelectedValue = Equipo.cIdTipoActivo
        'cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())
        'cboCatalogo.SelectedValue = Equipo.cIdCatalogo
        'ListarSistemaFuncionalCombo()
        'hfdIdClienteSAPEquipo.Value = Equipo.vIdClienteSAPEquipo
        'hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
        'hfdFechaRegistroTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaRegistroTarjetaSAPEquipo), "", Equipo.dFechaRegistroTarjetaSAPEquipo)
        'hfdFechaManufacturaTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaManufacturaTarjetaSAPEquipo), "", Equipo.dFechaManufacturaTarjetaSAPEquipo)
        'hfdFechaCreacionEquipo.Value = IIf(IsNothing(Equipo.dFechaCreacionEquipo), Now, Equipo.dFechaCreacionEquipo)
        'hfdIdUsuarioCreacionEquipo.Value = IIf(IsNothing(Equipo.cIdUsuarioCreacionEquipo), Session("IdUsuario"), Equipo.cIdUsuarioCreacionEquipo)
        CargarCestaInsumos()
        'CargarCestaInsumosTemporal()

        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
        'Catch ex As Exception 'JMUG: LO QUITE 11/09/2023
        '    'lblMensaje.Text = ex.Message
        '    ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        '    MyValidator.ErrorMessage = ex.Message
        '    MyValidator.IsValid = False
        '    MyValidator.ID = "ErrorPersonalizado"
        '    MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        '    Me.Page.Validators.Add(MyValidator)
        'End Try
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
        hfdOrdenFabricacionReferencia.Value = ""
    End Sub

    Sub LimpiarObjetosImagen()
        MyValidator.ErrorMessage = ""
        txtTituloSubirImagenActividad.Text = ""
        txtDescripcionSubirImagenActividad.Text = ""
        txtObservacionSubirImagenActividad.Text = ""
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
            cboFiltroOrdenTrabajo.SelectedIndex = 4
            'ListarTipoMantenimientoCombo()
            'ListarPersonalResponsableCombo()
            'ListarPersonalCombo()

            If Session("CestaOrdenTrabajo") Is Nothing Then
                Session("CestaOrdenTrabajo") = CrearCestaOrdenTrabajo()
            Else
                VaciarCestaOrdenTrabajo(Session("CestaOrdenTrabajo"))
            End If

            If Session("CestaInsumos") Is Nothing Then
                Session("CestaInsumos") = CrearCestaInsumos()
            Else
                VaciarCestaInsumos(Session("CestaInsumos"))
            End If

            If Session("CestaInsumosFiltrado") Is Nothing Then
                Session("CestaInsumosFiltrado") = CrearCestaInsumos()
            Else
                VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
            End If

            If Session("CestaOtrosDatos") Is Nothing Then
                Session("CestaOtrosDatos") = CrearCestaOtrosDatos()
            Else
                VaciarCestaOtrosDatos(Session("CestaOtrosDatos"))
            End If

            'BloquearMantenimiento(True, False, True, False)

            Dim SubFiltro As String = ""
            SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        "INNER JOIN GNRL_USUARIO AS USU ON " &
                        "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

            Me.grdLista.DataSource = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = '' AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            'Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroPersonal.SelectedValue,
            '                                                         txtBuscarPersonal.Text, Session("IdEmpresa"), Session("IdPuntoVenta"))
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
            'pnlEquipo.Visible = False
            'pnlComponentes.Visible = False
        Else
            txtBuscarOrdenTrabajo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            'txtIdEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_cboFamilia')")
            'cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcion')")
            'txtDescripcionEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'If lblOperacion.Value = "E" Or lblOperacion.Value = "N" Then
            '    BloquearPagina(2)
            'End If
        End If
    End Sub

    Sub CargarStatusActividad()
        'For Each row As GridViewRow In lstCheckList.Rows
        '    'For Each row As DataListItem In lstCheckList.Items
        '    '    'If row.RowType = DataControlRowType.DataRow Then
        '    '    '    'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalle"), CheckBox)
        '    '    '    Dim lblNroDocumento As Label = TryCast(row.Cells(0).FindControl("lblNroDoc"), Label)
        '    '    'End If
        '    '    If row.ItemType = DataControlRowType.DataRow Or (row.ItemType = ListItemType.AlternatingItem) Then
        '    '        Dim lnkbtnIniciarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
        '    '        Dim lnkbtnPendienteCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
        '    '        Dim lnkbtnRetomarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
        '    '        Dim lnkbtnFinalizarCheckList As LinkButton = TryCast(row.Find


        For Each elemento As DataListItem In lstCheckList.Items
            'For Each elemento As ListViewItem In lstCheckList.Items
            Dim hfdStatusCheckList As HiddenField = TryCast(elemento.FindControl("hfdStatusCheckList"), HiddenField)
            Dim lnkbtnIniciarCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnIniciarCheckList"), LinkButton)
            Dim lnkbtnPendienteCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnPendienteCheckList"), LinkButton)
            Dim lnkbtnRetomarCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnRetomarCheckList"), LinkButton)
            Dim lnkbtnFinalizarCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnFinalizarCheckList"), LinkButton)
            If hfdStatusCheckList.Value = "I" Then 'Iniciar
                lnkbtnIniciarCheckList.Visible = True
            Else
                lnkbtnIniciarCheckList.Visible = False
            End If
            If hfdStatusCheckList.Value = "E" Then 'En Proceso
                lnkbtnPendienteCheckList.Visible = True
                lnkbtnFinalizarCheckList.Visible = True
            Else
                lnkbtnPendienteCheckList.Visible = False
            End If
            If hfdStatusCheckList.Value = "P" Then 'Pendiente
                lnkbtnRetomarCheckList.Visible = True
                lnkbtnFinalizarCheckList.Visible = True
            Else
                lnkbtnRetomarCheckList.Visible = False
            End If
            If hfdStatusCheckList.Value = "F" Then 'Finalizar
                lnkbtnFinalizarCheckList.Visible = False
            End If
        Next
    End Sub

    Function fLlenarGrillaComponentes() As DataTable
        'Dim dsComponentes = OrdenTrabajoNeg.OrdenTrabajoListaBusquedaComponentes("EQU.cIdEnlaceEquipo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim), (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
        '                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
        '                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
        '                                                         Session("IdEmpresa"))
        Dim ComponentesEquipoNeg As New clsEquipoNegocios
        '        Dim Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        'Dim dsComponentes = ComponentesEquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim) & "' AND EQU.cIdEnlaceCatalogo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim), "1")
        Dim dsComponentes = ComponentesEquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim), "1")
        Dim dtComponentes As New DataTable
        dtComponentes = FuncionesNeg.ConvertirListADataTable(dsComponentes)
        dtComponentes.Columns.Add(New DataColumn("TotalCantidadComponentes", GetType(System.Int32)))
        dtComponentes.Columns.Add(New DataColumn("CantidadComponentesFinalizados", GetType(System.Int32)))
        dtComponentes.Columns.Add(New DataColumn("PorcentajeText", GetType(System.String)))
        dtComponentes.Columns.Add(New DataColumn("Porcentaje", GetType(System.Int32)))
        dtComponentes.Columns.Add(New DataColumn("Tiempo", GetType(System.Int32)))
        'dtComponentes.Columns.Add(New DataColumn("TiempoText", GetType(System.String)))
        For Each Componente In dtComponentes.Rows
            'Dim NombreArchivo = Empresa.vRucEmpresa & "-" & DocEmi("IdTipoDocumentoEquivalente") & "-" & DocEmi("NumeroSerieDocumento") & "-" & DocEmi("NumeroCorrelativo") & ".xml"
            'Componente("TotalCantidadComponentes") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') <> 'F'").Rows(0).Item(0)
            Componente("TotalCantidadComponentes") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "'").Rows(0).Item(0)
            Componente("CantidadComponentesFinalizados") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
            Componente("PorcentajeText") = IIf(Componente("TotalCantidadComponentes") = 0, "Sin actividad", Math.Round((100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes")), 2) & "%")
            Componente("Porcentaje") = IIf(Componente("TotalCantidadComponentes") = 0, 0, Math.Round(100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes"), 2))
            Componente("Tiempo") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT ISNULL(SUM(nTotalSegundosTrabajadosCheckListOrdenTrabajo), 0) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
        Next
        Return dtComponentes
    End Function

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

            'pnlCabecera.Visible = False
            'pnlEquipo.Visible = True
            'pnlComponentes.Visible = True
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            'Dim EquipoNeg As New clsEquipoNegocios
                            'If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'").Rows(0).Item(0) = "0" Then
                            '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                            'End If
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "N"
            'txtDescripcionEquipo.Focus()

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            'ValidarTexto(True)
            'ActivarObjetos(True)
            'LlenarDataEquipo()
            Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                 Session("IdEmpresa"))
            ListarPersonalAsignadoCombo(OrdTra)
            hfdOrdenFabricacionReferencia.Value = OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo
            LlenarDataEquipo()

            lstComponentes.DataSource = fLlenarGrillaComponentes()
            lstComponentes.DataBind()

            Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
            Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                                     Session("IdEmpresa"),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
            Me.lstCheckList.DataBind()
            CargarStatusActividad()
            pnlCabecera.Visible = False
            pnlContenido.Visible = True
            OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET cEstadoCabeceraOrdenTrabajo = 'P' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
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
                            'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            'hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            'lnkbtnImprimirTarjetaEquipo.Attributes.Add("onclick", "javascript:popupEmitirEquipoDetalleReporte('" & txtIdEquipo.Text & "');")
                            'lnkbtnVerOrdenFabricacion.Attributes.Add("onclick", "javascript:popupEmitirOrdenFabricacionReporte('" & txtIdEquipo.Text & "');")
                            lnkbtnVerOrdenTrabajo.Attributes.Add("onclick", "javascript:popupEmitirOrdenTrabajoReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text) & "', '" & Session("IdEmpresa") & "');")
                            Dim lblEstado As Label = TryCast(grdLista.SelectedRow.Cells(13).FindControl("lblEstado"), Label)
                            If lblEstado.Text = "Terminado" Then
                                BloquearMantenimiento(False, True, False, True)
                            Else
                                BloquearMantenimiento(True, False, True, False)
                            End If
                            If MyValidator.ErrorMessage = "" Then
                                MyValidator.ErrorMessage = "Registro encontrado con éxito"
                            End If
                        End If
                    End If
                Else
                    'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()

                End If
            End If
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
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Center 'IdTipoDocumento
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center 'IdNumeroSerie
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Center 'IdNumeroCorrelativo
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'FechaEmision
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdCliente
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Center 'IdClienteSAP
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left 'RucCliente
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'RazonSocial
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'IdEquipo
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'IdEquipoSAP
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'DescripcionEquipo
                e.Row.Cells.Item(11).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                e.Row.Cells.Item(12).HorizontalAlign = HorizontalAlign.Left 'IdArticuloSAPCabecera
                e.Row.Cells.Item(13).HorizontalAlign = HorizontalAlign.Left 'Estado
                e.Row.Cells.Item(14).HorizontalAlign = HorizontalAlign.Left 'Estado Registro
            Next
        End If
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
            e.Row.Cells(4).Visible = False 'IdCliente
            e.Row.Cells(5).Visible = False 'IdClienteSAP
            e.Row.Cells(6).Visible = True 'RucCliente
            e.Row.Cells(7).Visible = True 'RazonSocial
            e.Row.Cells(8).Visible = True 'IdEquipo
            e.Row.Cells(9).Visible = False 'IdEquipoSAP
            e.Row.Cells(10).Visible = True 'DescripcionEquipo
            e.Row.Cells(11).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(12).Visible = False 'IdArticuloSAPCabecera
            e.Row.Cells(13).Visible = True 'Estado
            e.Row.Cells(14).Visible = False 'Estado Registro
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = True 'IdNumeroSerie
            e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'FechaEmision
            e.Row.Cells(4).Visible = False 'IdCliente
            e.Row.Cells(5).Visible = False 'IdClienteSAP
            e.Row.Cells(6).Visible = True 'RucCliente
            e.Row.Cells(7).Visible = True 'RazonSocial
            e.Row.Cells(8).Visible = True 'IdEquipo
            e.Row.Cells(9).Visible = False 'IdEquipoSAP
            e.Row.Cells(10).Visible = True 'DescripcionEquipo
            e.Row.Cells(11).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(12).Visible = False 'IdArticuloSAPCabecera
            e.Row.Cells(13).Visible = True 'Estado
            e.Row.Cells(14).Visible = False 'Estado Registro
        End If
    End Sub

    Sub LlenarTarea()
        Dim ahora As DateTime = DateTime.Now

        ' Asignar la fecha y hora actual al TextBox de fecha
        txtFechaMantenimientoTarea.Text = ahora.ToString("dd/MM/yyyy")

        ' Obtener la hora actual y asignarla a los DropDownList de horas, minutos y segundos
        Dim hora As Integer = ahora.Hour
        Dim minuto As Integer = ahora.Minute
        Dim segundo As Integer = ahora.Second

        cboHorasMantenimientoTarea.SelectedValue = hora.ToString("00")
        cboMinutosMantenimientoTarea.SelectedValue = minuto.ToString("00")
        cboSegundosMantenimientoTarea.SelectedValue = segundo.ToString("00")
        If Convert.ToInt16(cboHorasMantenimientoTarea.SelectedValue) >= 12 Then
            cboMeridianoMantenimientoTarea.SelectedValue = "PM"
        Else
            cboMeridianoMantenimientoTarea.SelectedValue = "AM"
        End If
        Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
        If Valores(8).ToString = "I" Then 'Iniciar
            cboPersonalAsignadoMantenimientoTarea.Enabled = True
            txtDescripcionMantenimientoTarea.Enabled = True
            cboPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            txtDescripcionMantenimientoTarea.Text = ""
        ElseIf Valores(8).ToString = "E" Then 'En Proceso
            cboPersonalAsignadoMantenimientoTarea.Enabled = True
            txtDescripcionMantenimientoTarea.Enabled = True
            cboPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            txtDescripcionMantenimientoTarea.Text = ""
        ElseIf Valores(8).ToString = "P" Then 'Pendiente
            Dim dsTarea = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vDescripcionTareaCheckListOrdenTrabajo, cIdPersonal " &
                                                              "FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                                                              "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
            cboPersonalAsignadoMantenimientoTarea.Enabled = False
            txtDescripcionMantenimientoTarea.Enabled = False
            For Each fila In dsTarea.Rows
                cboPersonalAsignadoMantenimientoTarea.SelectedValue = fila("cIdPersonal")
                txtDescripcionMantenimientoTarea.Text = fila("vDescripcionTareaCheckListOrdenTrabajo")
            Next
        ElseIf Valores(8).ToString = "F" Then 'Finalizar
            Dim dsTarea = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vDescripcionTareaCheckListOrdenTrabajo, cIdPersonal " &
                                                              "FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                                                              "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
            cboPersonalAsignadoMantenimientoTarea.Enabled = False
            txtDescripcionMantenimientoTarea.Enabled = False
            For Each fila In dsTarea.Rows
                cboPersonalAsignadoMantenimientoTarea.SelectedValue = fila("cIdPersonal")
                txtDescripcionMantenimientoTarea.Text = fila("vDescripcionTareaCheckListOrdenTrabajo")
            Next
        End If
    End Sub

    Protected Sub lstCheckList_RowCommand_Botones(sender As Object, e As DataListCommandEventArgs)
        'Protected Sub lstCheckList_RowCommand_Botones(sender As Object, e As ListViewCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()
            'If Session("IdConfEmpresa") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
            '    Exit Sub
            'ElseIf Session("IdUsuario") = "" Then
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            If e.CommandName = "Iniciar" Then
                'Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTarea()
                ''OrdenTrabajoNeg.OrdenTrabajoGetData("INSERT LOGI_TAREACHECKLISTORDENTRABAJO (nIdNumeroItemTareaCheckListOrdenTrabajo, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo, cIdPersonal, vDescripcionTareaCheckListOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, cIdEquipoCabeceraOrdenTrabajo, cIdActividadCheckListOrdenTrabajo, cIdCatalogoCheckListOrdenTrabajo, cIdJerarquiaCatalogoCheckListOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo) " &
                ''                                    "VALUES nIdNumeroItemTareaCheckListOrdenTrabajo, GETDATE(), NULL, cIdPersonal, vDescripcionTareaCheckListOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, Valores(4).ToString, Valores(3).ToString, Valores(0).ToString, Valores(1).ToString, Valores(2).ToString")
                ''For Each row As GridViewRow In lstCheckList.Rows
                'For Each row As DataListItem In lstCheckList.Items
                '    'If row.RowType = DataControlRowType.DataRow Then
                '    '    'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalle"), CheckBox)
                '    '    Dim lblNroDocumento As Label = TryCast(row.Cells(0).FindControl("lblNroDoc"), Label)
                '    'End If
                '    If row.ItemType = DataControlRowType.DataRow Or (row.ItemType = ListItemType.AlternatingItem) Then
                '        Dim lnkbtnIniciarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
                '        Dim lnkbtnPendienteCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
                '        Dim lnkbtnRetomarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
                '        Dim lnkbtnFinalizarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
                '        lnkbtnIniciarCheckList.Visible = True
                '        lnkbtnPendienteCheckList.Visible = False
                '        lnkbtnRetomarCheckList.Visible = False
                '        lnkbtnFinalizarCheckList.Visible = False
                '    End If
                'Next

                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
            ElseIf e.CommandName = "Pendiente" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTarea()
                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()

                'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                'OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_TAREACHECKLISTORDENTRABAJO SET dFechaFinalTareaCheckListOrdenTrabajo = GETDATE() " &
                '                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND dFechaFinalTareaCheckListOrdenTrabajo IS NULL")

                'Dim Segundos As Decimal = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT SUM(ISNULL(DATEDIFF(SECOND, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo), 0)) AS nSegundos FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                '                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'").Rows(0).Item(0)

                'OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoCheckListOrdenTrabajo = 'P', nTotalSegundosTrabajadosCheckListOrdenTrabajo = " & Segundos & " " &
                '                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                'Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                'Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                '                                                                         Session("IdEmpresa"),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
                'Me.lstCheckList.DataBind()
                'CargarStatusActividad()
                'pnlCabecera.Visible = False
                'pnlContenido.Visible = True
                ''ValidationSummary1.ValidationGroup = "vgrpValidar"
                ''MyValidator.IsValid = False
                ''MyValidator.ID = "ErrorPersonalizado"
                ''MyValidator.ValidationGroup = "vgrpValidar"
                ''Me.Page.Validators.Add(MyValidator)
            ElseIf e.CommandName = "Retomar" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTarea()
                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
            ElseIf e.CommandName = "Finalizar" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "398", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTarea()
                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()

                'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                'OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_TAREACHECKLISTORDENTRABAJO SET dFechaFinalTareaCheckListOrdenTrabajo = GETDATE() " &
                '                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND dFechaFinalTareaCheckListOrdenTrabajo IS NULL")

                'Dim Segundos As Decimal = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT SUM(ISNULL(DATEDIFF(SECOND, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo), 0)) AS nSegundos FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                '                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'").Rows(0).Item(0)


                'OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoCheckListOrdenTrabajo = 'F', nTotalSegundosTrabajadosCheckListOrdenTrabajo = " & Segundos & " " &
                '                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")


                'Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                'Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                '                                                                         Session("IdEmpresa"),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
                'Me.lstCheckList.DataBind()
                'CargarStatusActividad()
                'pnlCabecera.Visible = False
                'pnlContenido.Visible = True
            ElseIf e.CommandName = "EstadoMalo" Then
                'MyValidator.ErrorMessage = ""
                'fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "399", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'M' " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
                Me.lstCheckList.DataBind()
                CargarStatusActividad()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado malo."
            ElseIf e.CommandName = "EstadoRegular" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "412", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'R' " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
                Me.lstCheckList.DataBind()
                CargarStatusActividad()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado regular."
            ElseIf e.CommandName = "EstadoBueno" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'B' " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
                Me.lstCheckList.DataBind()
                CargarStatusActividad()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado bueno."
            ElseIf e.CommandName = "EstadoNoAplica" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'N' " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
                Me.lstCheckList.DataBind()
                CargarStatusActividad()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado no aplica."
            ElseIf e.CommandName = "AgregarImagenes" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                LimpiarObjetosImagen()
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                Dim ActNeg As New clsActividadCheckListNegocios
                Dim Actividad As LOGI_ACTIVIDADCHECKLIST = ActNeg.ActividadCheckListListarPorId(Valores(3).ToString.Trim)
                Dim EquNeg As New clsEquipoNegocios
                Dim Equipo As LOGI_EQUIPO = EquNeg.EquipoListarPorId(Valores(4).ToString.Trim)
                txtTituloSubirImagenActividad.Text = Equipo.vDescripcionEquipo & " - " & Actividad.vDescripcionActividadCheckList
                txtDescripcionSubirImagenActividad.Text = Actividad.vDescripcionActividadCheckList
                lnk_mostrarPanelSubirImagenActividad_ModalPopupExtender.Show()
            ElseIf e.CommandName = "AgregarInsumos" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                Dim DetActNeg As New clsActividadCheckListNegocios
                lblDescripcionActividadAgregarInsumos.Text = DetActNeg.ActividadCheckListListarPorId(Valores(3).ToString.Trim).vDescripcionActividadCheckList
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarInsumosCombo()
                Dim DetOrdTra As New clsDetalleOrdenTrabajoNegocios
                'grdDetalleAgregarInsumos.DataSource = DetOrdTra.DetalleOrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo= ")
                CargarCestaInsumosTemporal()
                lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
            ElseIf e.CommandName = "AgregarDatosAdicionales" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                Dim EquipoNeg As New clsEquipoNegocios
                Dim DetActNeg As New clsActividadCheckListNegocios
                lblDescripcionEquipoCaracteristicaOtrosDatos.Text = EquipoNeg.EquipoListarPorId(Valores(4).ToString.Trim).vDescripcionEquipo
                lblDescripcionActividadCaracteristicaOtrosDatos.Text = DetActNeg.ActividadCheckListListarPorId(Valores(3).ToString.Trim).vDescripcionActividadCheckList
                ListarOtrosDatosCombo()
                CargarCestaOtrosDatos()
                grdDetalleCaracteristicaOtrosDatos.DataSource = Session("CestaOtrosDatos")
                grdDetalleCaracteristicaOtrosDatos.DataBind()
                lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
            End If
            'JMUG: Lo quité porque me mostraba un recuadro en blanco.   28/10/2023
            'ValidationSummary2.ValidationGroup = "vgrpValidar"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidar"
            'Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoTarea_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoTarea.Click
        Try
            If txtFechaMantenimientoTarea.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la fecha de mantenimiento.")
            ElseIf cboPersonalAsignadoMantenimientoTarea.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar el personal asignado al mantenimiento.")
            End If

            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            'Valores(8).ToString - I->Iniciar Actividad / P->Pendiente / R->Retomar / F->Finalizar

            'Obtener los valores seleccionados de los combos
            Dim hora As String = cboHorasMantenimientoTarea.SelectedValue
            Dim minutos As String = cboMinutosMantenimientoTarea.SelectedValue
            Dim segundos As String = cboSegundosMantenimientoTarea.SelectedValue
            Dim fecha As String = txtFechaMantenimientoTarea.Text

            'Concatenar los valores en una cadena en formato SQL
            Dim strFechaHoraAtencion As String = fecha & " " & hora & ":" & minutos & ":" & segundos

            If Valores(8).ToString = "I" Or Valores(8).ToString = "E" Then
                OrdenTrabajoNeg.OrdenTrabajoGetData("INSERT LOGI_TAREACHECKLISTORDENTRABAJO (nIdNumeroItemTareaCheckListOrdenTrabajo, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo, cIdPersonal, vDescripcionTareaCheckListOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, cIdEquipoCabeceraOrdenTrabajo, cIdActividadCheckListOrdenTrabajo, cIdCatalogoCheckListOrdenTrabajo, cIdJerarquiaCatalogoCheckListOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo, cIdEquipoCheckListOrdenTrabajo) " &
                                                    "VALUES (" & "(SELECT ISNULL(MAX(nIdNumeroItemTareaCheckListOrdenTrabajo), 0) + 1 " &
                                                    "FROM LOGI_TAREACHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "')" & ", " &
                                                    "'" & strFechaHoraAtencion & "', NULL, '" & cboPersonalAsignadoMantenimientoTarea.SelectedValue & "', '" & UCase(txtDescripcionMantenimientoTarea.Text.Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "', '" & Session("IdEmpresa") & "', '" & Valores(4).ToString & "', '" & Valores(3).ToString & "', '" & Valores(0).ToString & "', '" & Valores(1).ToString & "', '" & Valores(2).ToString & "', '" & Valores(7).ToString & "')")

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoCheckListOrdenTrabajo = 'E' " &
                                                    "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
            ElseIf Valores(8).ToString = "F" Or Valores(8).ToString = "P" Then
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_TAREACHECKLISTORDENTRABAJO SET dFechaFinalTareaCheckListOrdenTrabajo = '" & strFechaHoraAtencion & "' " &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND dFechaFinalTareaCheckListOrdenTrabajo IS NULL")

                Dim SegundosTotales As Decimal = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT SUM(ISNULL(DATEDIFF(SECOND, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo), 0)) AS nSegundos FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'").Rows(0).Item(0)

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoCheckListOrdenTrabajo = '" & IIf(Valores(8).ToString = "F", "F", IIf(Valores(8).ToString = "P", "P", "E")) & "', nTotalSegundosTrabajadosCheckListOrdenTrabajo = " & SegundosTotales & " " &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
                'ElseIf Valores(8).ToString = "P" Then
                '    OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_TAREACHECKLISTORDENTRABAJO SET dFechaFinalTareaCheckListOrdenTrabajo = '" & strFechaHoraAtencion & "' " &
                '                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND dFechaFinalTareaCheckListOrdenTrabajo IS NULL")

                '    Dim SegundosTotales As Decimal = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT SUM(ISNULL(DATEDIFF(SECOND, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo), 0)) AS nSegundos FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                '                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'").Rows(0).Item(0)

                '    OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoCheckListOrdenTrabajo = 'P', nTotalSegundosTrabajadosCheckListOrdenTrabajo = " & SegundosTotales & " " &
                '                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

            End If
            Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
            Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                                     Session("IdEmpresa"),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
            Me.lstCheckList.DataBind()
            CargarStatusActividad()
            pnlCabecera.Visible = False
            pnlContenido.Visible = True
        Catch ex As Exception
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoTarea"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTarea"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAceptarSubirImagenActividad_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirImagenActividad.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If Not (fupSubirImagenActividad.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
            Dim ext As String = fupSubirImagenActividad.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"jpg", "jpeg", "bmp", "png", "gif"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de imagen inválido.")
            Dim Size As Integer = 0
            If Not (Integer.TryParse("160", Size)) Then
                Throw New Exception("El tamaño indicado para la imagen no es válido.")
            End If
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList

            Dim GaleriaChkLst As New LOGI_GALERIACHECKLISTORDENTRABAJO 'LOGI_GALERIAEQUIPO
            GaleriaChkLst.cIdTipoDocumentoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim)
            GaleriaChkLst.vIdNumeroSerieCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim)
            GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim)
            GaleriaChkLst.cIdEmpresa = Session("IdEmpresa")
            GaleriaChkLst.cIdEquipoCabeceraOrdenTrabajo = Valores(4).ToString
            GaleriaChkLst.cIdActividadCheckListOrdenTrabajo = Valores(3).ToString
            GaleriaChkLst.cIdCatalogoCheckListOrdenTrabajo = Valores(0).ToString
            GaleriaChkLst.cIdJerarquiaCatalogoCheckListOrdenTrabajo = Valores(1).ToString
            GaleriaChkLst.vIdArticuloSAPCabeceraOrdenTrabajo = Valores(2).ToString
            GaleriaChkLst.cIdEquipoCheckListOrdenTrabajo = Valores(7).ToString
            GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo = 0
            GaleriaChkLst.vTituloGaleriaCheckListOrdenTrabajo = UCase(txtTituloSubirImagenActividad.Text.Trim)
            GaleriaChkLst.vDescripcionGaleriaCheckListOrdenTrabajo = UCase(txtDescripcionSubirImagenActividad.Text.Trim)
            GaleriaChkLst.vObservacionGaleriaCheckListOrdenTrabajo = UCase(txtObservacionSubirImagenActividad.Text.Trim)
            GaleriaChkLst.dFechaTransaccionGaleriaCheckListOrdenTrabajo = Now
            GaleriaChkLst.bEstadoRegistroGaleriaCheckListOrdenTrabajo = True

            Dim ColeccionGaleria As New List(Of LOGI_GALERIACHECKLISTORDENTRABAJO)

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

            Dim GaleriaChkLstNeg As New clsGaleriaCheckListNegocios
            If GaleriaChkLstNeg.GaleriaCheckListInserta(GaleriaChkLst) = 0 Then
                'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
                Dim FuncionesNeg As New clsFuncionesNegocios
                'FuncionesNeg.GuardarArchivo(fupSubirImagenActividad.PostedFile, True, "Imagenes\Actividad", Trim(UCase(GaleriaChkLst.cIdEquipoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString)), True, "", 1200, 160)
                FuncionesNeg.GuardarArchivo(fupSubirImagenActividad.PostedFile, True, "Imagenes\Actividad", Trim(UCase(GaleriaChkLst.cIdTipoDocumentoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.vIdNumeroSerieCabeceraOrdenTrabajo & "-" & GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString)), True, "", 1200, 160)

                Session("Query") = "PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO 'SQL_INSERT', '','" & GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.cIdTipoDocumentoCabeceraOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.vIdNumeroSerieCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.cIdEmpresa & "', '" &
                                                          GaleriaChkLst.cIdEquipoCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.cIdActividadCheckListOrdenTrabajo & "', '" & GaleriaChkLst.cIdCatalogoCheckListOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vIdArticuloSAPCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.cIdEquipoCheckListOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.vDescripcionGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vObservacionGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vTituloGaleriaCheckListOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.bEstadoRegistroGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.dFechaTransaccionGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "'"

                LogAuditoria.vEvento = "INSERTAR GALERIA CHECK LIST"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                MyValidator.ErrorMessage = "Transacción registrada con éxito"

                BloquearMantenimiento(True, False, True, False)
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidarSubirImagenActividad"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenActividad"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
            ValidationSummary5.ValidationGroup = "vgrpValidarSubirImagenActividad"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenActividad"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirImagenActividad_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub lnkbtnVerGaleria_Click(sender As Object, e As EventArgs) Handles lnkbtnVerGaleria.Click
        Dim GaleriaNeg As New clsGaleriaCheckListNegocios
        Me.lstGaleriaActividad.DataSource = GaleriaNeg.GaleriaCheckListListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa", Session("IdEmpresa"), "1")
        Me.lstGaleriaActividad.DataBind()

        lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender.Show()
    End Sub

    Protected Sub lstGaleriaActividad_ItemCommand(source As Object, e As DataListCommandEventArgs)
        If e.CommandName = "VerFotoGaleria" Then
            lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender.Hide()
            Dim i = e.Item.ItemIndex
            Dim hfdUrlGaleriaActividad As HiddenField = TryCast(lstGaleriaActividad.Items(i).FindControl("hfdUrlGaleriaActividad"), HiddenField)
            imgVerImagenActividad.ImageUrl = "~\Imagenes\Actividad\" & hfdUrlGaleriaActividad.Value & ".JPG"
            lnk_mostrarPanelVerImagenActividad_ModalPopupExtender.Show()
        End If
    End Sub

    Sub CargarCestaOtrosDatos()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaOtrosDatos(Session("CestaOtrosDatos"))

            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            Dim OtrosDatosNeg As New clsOtrosDatosOrdenTrabajoNegocios

            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            Dim dsOtrosDatos = OtrosDatosNeg.OtrosDatosOrdenTrabajoGetData("SELECT OTRDATORDTRA.*, " &
                                                                "     CAR.vDescripcionCaracteristica " &
                                                                "FROM LOGI_OTROSDATOSORDENTRABAJO AS OTRDATORDTRA " &
                                                                "INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
                                                                "      OTRDATORDTRA.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                "WHERE OTRDATORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "'" &
                                                                "      AND OTRDATORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' " &
                                                                "      AND OTRDATORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' " &
                                                                "      AND OTRDATORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                                "      AND OTRDATORDTRA.cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "'" &
                                                                "      AND OTRDATORDTRA.vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdEquipoCheckListOrdenTrabajo = '" & Valores(7).ToString & "'")
            For Each OtrosDatos In dsOtrosDatos.Rows
                AgregarCestaOtrosDatos(OtrosDatos("cIdCaracteristica"), OtrosDatos("vDescripcionCaracteristica"), OtrosDatos("vValorReferencialOtrosDatosOrdenTrabajo"), OtrosDatos("cIdEquipoCabeceraOrdenTrabajo"),
                                    OtrosDatos("cIdActividadCheckListOrdenTrabajo"), OtrosDatos("cIdCatalogoCheckListOrdenTrabajo"), OtrosDatos("cIdJerarquiaCatalogoCheckListOrdenTrabajo"),
                                    OtrosDatos("vIdArticuloSAPCabeceraOrdenTrabajo"), OtrosDatos("cIdEquipoCheckListOrdenTrabajo"), Session("CestaOtrosDatos"))
            Next
        Catch ex As Exception
            ValidationSummary7.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaInsumos()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaInsumos(Session("CestaInsumos"))

            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            Dim InsumosNeg As New clsDetalleOrdenTrabajoNegocios
            Dim dsInsumos = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT DETORDTRA.* " &
                                                                "" &
                                                                "FROM LOGI_DETALLEORDENTRABAJO AS DETORDTRA " &
                                                                "WHERE DETORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "'" &
                                                                "      AND DETORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' " &
                                                                "      AND DETORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' " &
                                                                "      AND DETORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "' ")
            For Each Insumos In dsInsumos.Rows
                AgregarCestaInsumos(Insumos("vIdArticuloSAPDetalleOrdenTrabajo"), Insumos("vDescripcionArticuloDetalleOrdenTrabajo"), Insumos("nCantidadArticuloDetalleOrdenTrabajo"), Insumos("vDescripcionUnidadMedidaDetalleOrdenTrabajo"),
                                    Insumos("cIdEquipoCabeceraOrdenTrabajo"), Insumos("cIdCatalogoCheckListDetalleOrdenTrabajo"), Insumos("cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo"), Insumos("cIdActividadCheckListDetalleOrdenTrabajo"), Insumos("vIdArticuloSAPCabeceraOrdenTrabajo"), Session("CestaInsumos"))
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaInsumosTemporal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            Dim resultInsumosSimple As DataRow() = Session("CestaInsumos").Select("IdCatalogo = '" & Valores(0).ToString & "' AND IdJerarquia = '" & Valores(1).ToString & "' AND IdActividad = '" & Valores(3).ToString & "' AND IdArticuloSAP = '" & Valores(2).ToString & "'")
            If resultInsumosSimple.Length = 0 Then
                VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
            Else
                Dim rowFil As DataRow() = resultInsumosSimple
                For Each Insumos As DataRow In rowFil
                    AgregarCestaInsumos(Insumos("Codigo"), Insumos("Descripcion"), Insumos("Cantidad"), Insumos("DescripcionUnidadMedida"),
                                Insumos("IdEquipo"), Insumos("IdCatalogo"), Insumos("IdJerarquia"), Insumos("IdActividad"), Insumos("IdArticuloSAP"), Session("CestaInsumosFiltrado"))
                Next
            End If
            Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
            Me.grdDetalleAgregarInsumos.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtCantidadDetalle_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleAgregarInsumos.Rows
                Dim txtCantidadDetalle As TextBox = TryCast(row.Cells(4).FindControl("txtCantidadDetalle"), TextBox)
                Dim hfdRowNumber As HiddenField = TryCast(row.Cells(4).FindControl("hfdRowNumber"), HiddenField)
                Dim FilaActual As Int16
                'FilaActual = row.RowIndex + (grdDetalleAgregarInsumos.Rows.Count * (grdDetalleAgregarInsumos.PageIndex))
                FilaActual = hfdRowNumber.Value
                Session("CestaInsumosFiltrado").Rows(FilaActual)("Cantidad") = txtCantidadDetalle.Text

                For i = 0 To Session("CestaInsumos").Rows.Count - 1
                    '        '    Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                    '        '    hfdValoresCheckList.Value = e.CommandArgument.ToString
                    '        '    'Valores(0).ToString - IdCatalogo
                    '        '    'Valores(1).ToString - IdJerarquiaCatalogo
                    '        '    'Valores(2).ToString - IdArticuloSAPCabecera
                    '        '    'Valores(3).ToString - IdActividad
                    '        '    'Valores(4).ToString - IdEquipo
                    '        '    'Valores(5).ToString - IdSistemaFuncional
                    '        '    'Valores(6).ToString - IdTipoActivo

                    'FilaActual = row.RowIndex - (grdDetalleAgregarInsumos.Rows.Count * (grdDetalleAgregarInsumos.PageIndex))
                    If Session("CestaInsumos").Rows(i)("IdCatalogo") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdCatalogo") And
                       Session("CestaInsumos").Rows(i)("IdJerarquia") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdJerarquia") And
                       Session("CestaInsumos").Rows(i)("IdActividad") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdActividad") And
                       Session("CestaInsumos").Rows(i)("IdEquipo") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdEquipo") And
                       Session("CestaInsumos").Rows(i)("IdArticuloSAP") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdArticuloSAP") And
                       Session("CestaInsumos").Rows(i)("Codigo") = Session("CestaInsumosFiltrado").Rows(FilaActual)("Codigo") Then
                        EditarCestaInsumos(Session("CestaInsumosFiltrado").Rows(FilaActual)("Codigo"), Session("CestaInsumosFiltrado").Rows(FilaActual)("Descripcion"), Session("CestaInsumosFiltrado").Rows(FilaActual)("Cantidad"), Session("CestaInsumosFiltrado").Rows(FilaActual)("DescripcionUnidadMedida"),
                                            Session("CestaInsumosFiltrado").Rows(FilaActual)("IdEquipo"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdCatalogo"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdJerarquia"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdActividad"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdArticuloSAP"), Session("CestaInsumos"), i)
                        'AgregarCestaInsumos(cboInsumosAgregarInsumos.SelectedValue, cboInsumosAgregarInsumos.SelectedItem.Text, 0, strUnidadMedida, Valores(4).ToString, Valores(0).ToString, Valores(1).ToString, Valores(3).ToString, Valores(2).ToString, Session("CestaInsumos"))
                        'Exit For
                    End If
                Next
            Next
            'CargarCestaInsumosTemporal()

        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarInsumos_Click(sender As Object, e As EventArgs) Handles btnAdicionarInsumos.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))
            If cboInsumosAgregarInsumos.SelectedIndex = 0 Then
                lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
                Throw New Exception("Debe de seleccionar algún item.")
            End If
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            For i = 0 To Session("CestaInsumos").Rows.Count - 1
                If (Session("CestaInsumos").Rows(i)("Codigo").ToString.Trim) = (cboInsumosAgregarInsumos.SelectedValue.ToString.Trim) And Session("CestaInsumos").Rows(i)("IdCatalogo").ToString.Trim = Valores(0).ToString.Trim Then
                    Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
                    Me.grdDetalleAgregarInsumos.DataBind()
                    lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
                    Throw New Exception("Característica ya registrada, seleccione otro item.")
                End If
            Next

            Dim InsumosNeg As New clsOrdenFabricacionNegocios 'clsDetalleOrdenTrabajoNegocios
            Dim strUnidadMedida = ""
            strUnidadMedida = Trim(InsumosNeg.OrdenFabricacionListarPorIdDetalle(ValoresOS(0).Trim, ValoresOS(1).Trim, ValoresOS(2).Trim, Session("IdEmpresa"), Valores(2).ToString.Trim, cboInsumosAgregarInsumos.SelectedValue).vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion)
            AgregarCestaInsumos(cboInsumosAgregarInsumos.SelectedValue, cboInsumosAgregarInsumos.SelectedItem.Text, 0, strUnidadMedida, Valores(4).ToString, Valores(0).ToString, Valores(1).ToString, Valores(3).ToString, Valores(2).ToString, Session("CestaInsumos"))
            AgregarCestaInsumos(cboInsumosAgregarInsumos.SelectedValue, cboInsumosAgregarInsumos.SelectedItem.Text, 0, strUnidadMedida, Valores(4).ToString, Valores(0).ToString, Valores(1).ToString, Valores(3).ToString, Valores(2).ToString, Session("CestaInsumosFiltrado"))
            Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
            Me.grdDetalleAgregarInsumos.DataBind()
            cboInsumosAgregarInsumos.SelectedIndex = -1
            grdDetalleAgregarInsumos.SelectedIndex = -1
            MyValidator.ErrorMessage = "Insumo agregado con éxito."
            ValidationSummary6.ValidationGroup = "vgrpValidarAgregarInsumos"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarAgregarInsumos"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary6.ValidationGroup = "vgrpValidarAgregarInsumos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarAgregarInsumos"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleAgregarInsumos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleAgregarInsumos.RowDeleting
        Try
            MyValidator.ErrorMessage = ""
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            For Each row As GridViewRow In grdDetalleAgregarInsumos.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleAgregarInsumos"), CheckBox)
                    If chkRow.Checked Then
                        'Dim FilaActual As Int16
                        For i = 0 To Session("CestaInsumos").Rows.Count - 1
                            '        '    Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                            '        '    hfdValoresCheckList.Value = e.CommandArgument.ToString
                            '        '    'Valores(0).ToString - IdCatalogo
                            '        '    'Valores(1).ToString - IdJerarquiaCatalogo
                            '        '    'Valores(2).ToString - IdArticuloSAPCabecera
                            '        '    'Valores(3).ToString - IdActividad
                            '        '    'Valores(4).ToString - IdEquipo
                            '        '    'Valores(5).ToString - IdSistemaFuncional
                            '        '    'Valores(6).ToString - IdTipoActivo

                            'FilaActual = row.RowIndex - (grdDetalleAgregarInsumos.Rows.Count * (grdDetalleAgregarInsumos.PageIndex))
                            If (Session("CestaInsumos").Rows(i)("IdCatalogo").ToString.Trim) = Valores(0).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdJerarquia").ToString.Trim) = Valores(1).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdActividad").ToString.Trim) = Valores(3).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdEquipo").ToString.Trim) = Valores(4).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdArticuloSAP").ToString.Trim) = Valores(2).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("Codigo").ToString.Trim) = row.Cells(2).Text.ToString.Trim Then
                                QuitarCestaInsumos(i, Session("CestaInsumos"))
                                Exit For
                            End If
                        Next

                        For i = 0 To Session("CestaInsumosFiltrado").Rows.Count - 1
                            If (Session("CestaInsumosFiltrado").Rows(i)("Codigo").ToString.Trim) = row.Cells(2).Text.ToString.Trim Then
                                QuitarCestaInsumos(i, Session("CestaInsumosFiltrado"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
            Me.grdDetalleAgregarInsumos.DataBind()
            lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleAgregarInsumos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleAgregarInsumos.PageIndexChanging
        grdDetalleAgregarInsumos.PageIndex = e.NewPageIndex
        Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
        Me.grdDetalleAgregarInsumos.DataBind() 'Recargo el grid.
        grdDetalleAgregarInsumos.SelectedIndex = -1
        lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
    End Sub

    Sub txtValorDetalleCaracteristicaOtrosDatos_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaOtrosDatos.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim hfdRowNumber As HiddenField = TryCast(row.Cells(5).FindControl("hfdRowNumber"), HiddenField)
                Dim FilaActual As Int16
                'FilaActual = row.RowIndex - (grdDetalleCaracteristicaOtrosDatos.Rows.Count * (grdDetalleCaracteristicaOtrosDatos.PageIndex))
                FilaActual = hfdRowNumber.Value
                Session("CestaOtrosDatos").Rows(FilaActual)("ValorReferencial") = txtValorDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub btnAceptarOtrosDatos_Click(sender As Object, e As EventArgs) Handles btnAceptarOtrosDatos.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "142", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            Dim Coleccion As New List(Of LOGI_OTROSDATOSORDENTRABAJO)
            For i = 0 To Session("CestaOtrosDatos").Rows.Count - 1
                Dim OtrDatOT As New LOGI_OTROSDATOSORDENTRABAJO
                OtrDatOT.cIdTipoDocumentoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim)
                OtrDatOT.vIdNumeroSerieCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim)
                OtrDatOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim)
                OtrDatOT.cIdEmpresa = Session("IdEmpresa")
                OtrDatOT.cIdEquipoCabeceraOrdenTrabajo = Valores(4).ToString
                OtrDatOT.nIdNumeroItemOtrosDatosOrdenTrabajo = i + 1
                OtrDatOT.cIdCatalogoCheckListOrdenTrabajo = Valores(0).ToString
                OtrDatOT.cIdJerarquiaCatalogoCheckListOrdenTrabajo = Valores(1).ToString
                OtrDatOT.cIdActividadCheckListOrdenTrabajo = Valores(3).ToString
                OtrDatOT.vIdArticuloSAPCabeceraOrdenTrabajo = Valores(2).ToString
                'OtrDatOT.vIdArticuloSAPDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("Codigo").ToString.Trim
                OtrDatOT.cIdEquipoCheckListOrdenTrabajo = Session("CestaOtrosDatos").Rows(i)("IdEquipoCheckList").ToString.Trim
                OtrDatOT.vValorReferencialOtrosDatosOrdenTrabajo = Session("CestaOtrosDatos").Rows(i)("ValorReferencial").ToString.Trim
                OtrDatOT.cIdCaracteristica = Session("CestaOtrosDatos").Rows(i)("Codigo").ToString.Trim
                Coleccion.Add(OtrDatOT)
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

            Dim OtrosDatosOrdenTrabajoNeg As New clsOtrosDatosOrdenTrabajoNegocios
            If OtrosDatosOrdenTrabajoNeg.OtrosDatosOrdenTrabajoInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
                MyValidator.ErrorMessage = "Transacción registrada con éxito"
                CargarCestaInsumos()
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

    Private Sub btnAceptarAgregarInsumos_Click(sender As Object, e As EventArgs) Handles btnAceptarAgregarInsumos.Click
        'Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
        'Valores(0).ToString - IdCatalogo
        'Valores(1).ToString - IdJerarquiaCatalogo
        'Valores(2).ToString - IdArticuloSAPCabecera
        'Valores(3).ToString - IdActividad
        'Valores(4).ToString - IdEquipo
        'Valores(5).ToString - IdSistemaFuncional
        'Valores(6).ToString - IdTipoActivo
        'Valores(7).ToString - IdEquipoCheckList
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "142", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            Dim Coleccion As New List(Of LOGI_DETALLEORDENTRABAJO)
            For i = 0 To Session("CestaInsumos").Rows.Count - 1
                Dim DetOT As New LOGI_DETALLEORDENTRABAJO
                DetOT.cIdTipoDocumentoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim)
                DetOT.vIdNumeroSerieCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim)
                DetOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim)
                DetOT.cIdEmpresa = Session("IdEmpresa")
                DetOT.cIdEquipoCabeceraOrdenTrabajo = Valores(4).ToString
                DetOT.nIdNumeroItemDetalleOrdenTrabajo = i + 1
                DetOT.cIdCatalogoCheckListDetalleOrdenTrabajo = Valores(0).ToString
                DetOT.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo = Valores(1).ToString
                DetOT.cIdActividadCheckListDetalleOrdenTrabajo = Valores(3).ToString
                DetOT.vIdArticuloSAPCabeceraOrdenTrabajo = Valores(2).ToString
                DetOT.vIdArticuloSAPDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("Codigo").ToString.Trim
                DetOT.vDescripcionArticuloDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("Descripcion").ToString.Trim
                DetOT.nCantidadArticuloDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("Cantidad").ToString.Trim
                DetOT.vDescripcionUnidadMedidaDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("DescripcionUnidadMedida").ToString.Trim
                Coleccion.Add(DetOT)
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
            'Esto lo coloco antes porque el LogAuditoria lo estoy mandando en un solo bloque.
            LogAuditoria.vEvento = "INSERTAR TIPO DE ACTIVO"
            LogAuditoria.vQuery = Session("Query")
            LogAuditoria.cIdSistema = Session("IdSistema")
            LogAuditoria.cIdModulo = strOpcionModulo

            If DetalleOrdenTrabajoNeg.DetalleOrdenTrabajoInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
                CargarCestaInsumos()
                MyValidator.ErrorMessage = "Transacción registrada con éxito"
            End If
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaOtrosDatos_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaOtrosDatos.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""

            If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
                'If IsNothing(grdDetalleCaracteristicaOtrosDatos.SelectedRow) = False Then
                If MyValidator.ErrorMessage = "" Then
                    'For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
                    For i = 0 To Session("CestaOtrosDatos").Rows.Count - 1
                        If (Session("CestaOtrosDatos").Rows(i)("Codigo").ToString.Trim) = (cboCaracteristicaOtrosDatos.SelectedValue.ToString.Trim) Then
                            grdDetalleCaracteristicaOtrosDatos.DataSource = Session("CestaOtrosDatos")
                            grdDetalleCaracteristicaOtrosDatos.DataBind()
                            LimpiarObjetosOtrosDatos()
                            lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
                            grdDetalleCaracteristicaOtrosDatos.SelectedIndex = -1
                            Throw New Exception("Característica ya registrada, seleccione otro item.")
                        End If
                    Next
                    Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
                    AgregarCestaOtrosDatos(cboCaracteristicaOtrosDatos.SelectedValue, cboCaracteristicaOtrosDatos.SelectedItem.Text, "",
                                           Valores(4).ToString, Valores(3).ToString, Valores(0).ToString, Valores(1).ToString, Valores(2).ToString,
                                           Valores(7).ToString, Session("CestaOtrosDatos"))
                    Me.grdDetalleCaracteristicaOtrosDatos.DataSource = Session("CestaOtrosDatos")
                    Me.grdDetalleCaracteristicaOtrosDatos.DataBind()
                    LimpiarObjetosOtrosDatos()
                    lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
                    grdDetalleCaracteristicaOtrosDatos.SelectedIndex = -1
                    MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
                Else
                    lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
                End If
            End If
            ValidationSummary7.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
            ValidationSummary7.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Dim SubFiltro As String = ""
        SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        "INNER JOIN GNRL_USUARIO AS USU ON " &
                        "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

        Me.grdLista.DataSource = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = '' AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        'Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroPersonal.SelectedValue,
        '                                                             txtBuscarPersonal.Text, Session("IdEmpresa"), Session("IdPuntoVenta"))
        Me.grdLista.DataBind()
        pnlCabecera.Visible = True
        pnlContenido.Visible = False
        BloquearMantenimiento(True, False, True, False)
    End Sub

    Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Dim SubFiltro As String = ""
        SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        "INNER JOIN GNRL_USUARIO AS USU ON " &
                        "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

        Me.grdLista.DataSource = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = '' AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Private Sub cboHorasMantenimientoTarea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHorasMantenimientoTarea.SelectedIndexChanged
        If Convert.ToInt16(cboHorasMantenimientoTarea.SelectedValue) >= 12 Then
            cboMeridianoMantenimientoTarea.SelectedValue = "PM"
        Else
            cboMeridianoMantenimientoTarea.SelectedValue = "AM"
        End If
    End Sub

    Private Sub lnkbtnTerminarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnTerminarOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            MyValidator.ErrorMessage = "Orden de Trabajo por Servicio Terminada"
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "R"
            BloquearMantenimiento(False, True, False, True)
            'LimpiarObjetos()
            OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET cEstadoCabeceraOrdenTrabajo = 'T' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
            Dim strOrdenFabricacion As String = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vOrdenFabricacionReferenciaCabeceraOrdenTrabajo FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ").Rows(0).Item(0).ToString
            OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'T' WHERE cIdTipoDocumentoCabeceraOrdenFabricacion + '-' + vIdNumeroSerieCabeceraOrdenFabricacion + '-' + vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & strOrdenFabricacion & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'")
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)

            Dim SubFiltro As String = ""
            SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        "INNER JOIN GNRL_USUARIO AS USU ON " &
                        "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

            Me.grdLista.DataSource = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = '' AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
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