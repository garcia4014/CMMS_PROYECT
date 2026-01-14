Imports CapaNegocioCMMS
Imports CapaDatosCMMS
Imports Microsoft.Win32
Public Class frmLogiMntPlantillaChecklist
    Inherits System.Web.UI.Page
    Dim CheckListPlantillaNeg As New CapaNegocioCMMS.clsCabeceraChecklistPlantillaNegocios
    Dim TipoMantenimientoNeg As New clsTipoMantenimientoNegocios
    Dim ActividadNeg As New clsActividadCheckListNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
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

    Shared Function CrearCesta() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Item", GetType(System.Int32))) '0
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("Estado", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("DescripcionComponente", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("OrdenUbicacion", GetType(System.Int32))) '6
        Return dt
    End Function

    Shared Sub EditarCesta(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal Estado As String, ByVal DescripcionComponente As String,
                           ByVal OrdenUbicacion As Int32,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Descripcion
                Tabla.Rows(Fila)(2) = IdCatalogo
                Tabla.Rows(Fila)(3) = IdJerarquia
                Tabla.Rows(Fila)(4) = Estado
                Tabla.Rows(Fila)(5) = DescripcionComponente
                Tabla.Rows(Fila)(6) = OrdenUbicacion
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCesta(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdCatalogo As String,
                            ByVal IdJerarquia As String, ByVal Estado As String, ByVal DescripcionComponente As String,
                            ByVal OrdenUbicacion As Int32,
                            ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Tabla.Rows.Count + 1
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdJerarquia") = IdJerarquia
        Fila("Estado") = Estado
        Fila("DescripcionComponente") = DescripcionComponente
        Fila("OrdenUbicacion") = OrdenUbicacion
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

    Sub CargarCestaCatalogoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoComponente"))

        Dim CatalogoNeg As New clsCatalogoNegocios
        Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        Dim intContador As Integer = 0

        For Each Registro In Coleccion
            clsLogiCestaCatalogo.AgregarCesta(Registro.Codigo, Registro.IdTipoActivo, "1", Registro.IdSistemaFuncional,
                                              cboCatalogo.SelectedValue, UCase(Registro.Descripcion),
                                              UCase(Registro.DescripcionAbreviada), Registro.Estado, 0, Registro.IdCuentaContable, Registro.IdCuentaContableLeasing, Registro.DescripcionTipoActivo,
                                              Registro.DescripcionSistemaFuncional, Registro.PeriodoGarantia, Registro.PeriodoMinimoMantenimiento,
                                              Session("CestaCatalogoComponente"))
        Next
        grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
        Me.grdCatalogoComponente.DataBind()
    End Sub

    Sub CargarCestaActividadComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        VaciarCesta(Session("CestaActividadCatalogoComponenteChkList"))

        Dim DetChkLstPlaNeg As New clsDetalleChecklistPlantillaNegocios
        If grdLista IsNot Nothing Then
            If grdLista.Rows.Count > 0 Then
                If IsNothing(grdLista.SelectedRow) = False Then
                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                        Dim Coleccion = DetChkLstPlaNeg.DetalleChecklistPlantillaListaBusqueda("CHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND CHKLISPLA.cIdTipoMantenimiento = '" & cboTipoMantenimiento.SelectedValue & "' AND CHKLISPLA.cIdJerarquiaCatalogo", "1", Session("IdEmpresa"), "1")
                        Dim intContador As Integer = 0
                        For Each Registro In Coleccion
                            AgregarCesta(Registro.Codigo, UCase(Registro.DescripcionActividad).Trim, Registro.IdCatalogo, "1", "1", Registro.DescripcionCatalogo, 0, Session("CestaActividadCatalogoComponenteChkList"))
                        Next
                    End If
                End If
            End If
        End If

        grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponenteChkList")
        Me.grdDetalleActividadCatalogoComponente.DataBind()
    End Sub

    Sub CargarCestaActividadComponenteTemporal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            If Session("CestaActividadCatalogoComponenteChkListFiltrado") Is Nothing Then
                Session("CestaActividadCatalogoComponenteChkListFiltrado") = CrearCesta() 'clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                'clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaActividadCatalogoComponenteChkListFiltrado"))
                VaciarCesta(Session("CestaActividadCatalogoComponenteChkListFiltrado"))
            End If
            If Session("CestaCatalogoComponente").Rows.Count > 0 Then
                'Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '1' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                '                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                '                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                '                                                           "WHERE EQUCAR.cIdEquipo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdEquipo").ToString.Trim & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
                'Dim DetChkLstPlaNeg As New clsDetalleChecklistPlantillaNegocios
                'Dim Coleccion = DetChkLstPlaNeg.DetalleChecklistPlantillaListaBusqueda("CHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND CHKLISPLA.cIdTipoMantenimiento = '" & cboTipoMantenimiento.SelectedValue & "' AND CHKLISPLA.cIdJerarquiaCatalogo", "1", Session("IdEmpresa"), "1")
                ''For Each CaracteristicaEquipo In dsCaracteristicaEquipoComponente.Rows
                'For Each CaracteristicaEquipo In Coleccion
                If IsNothing(grdCatalogoComponente.SelectedRow) = False Then
                    If IsReference(grdCatalogoComponente.SelectedRow.Cells(1).Text) = True Then
                        Dim resultCaracteristicaSimple As DataRow() = Session("CestaActividadCatalogoComponenteChkList").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdJerarquia = '1'")
                        If resultCaracteristicaSimple.Length = 0 Then
                            Me.grdDetalleActividadCatalogoComponente.DataSource = Nothing
                        Else
                            Dim rowFil As DataRow() = resultCaracteristicaSimple
                            For Each fila As DataRow In rowFil
                                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdEquipo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                                'AgregarCesta(fila("Codigo"), UCase(fila("DescripcionActividad")).Trim, fila("IdCatalogo"), "1", "1", fila("DescripcionCatalogo"), 0, Session("CestaActividadCatalogoComponenteChkListFiltrado"))
                                AgregarCesta(fila("Codigo"), UCase(fila("Descripcion")).Trim, fila("IdCatalogo"), "1", "1", fila("DescripcionComponente"), 0, Session("CestaActividadCatalogoComponenteChkListFiltrado"))
                            Next
                            'Exit Sub
                        End If
                    End If
                End If
                'Next
                ''If dsCaracteristicaEquipoComponente.Rows.Count = 0 Then
                'If Coleccion.Count = 0 Then
                '    'Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                '    '                                                       "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
                '    '                                                       "WHERE CATCAR.cIdCatalogo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim & "' AND CATCAR.cIdJerarquiaCatalogo = '1'")
                '    'Dim DetChkLstPlaNeg As New clsDetalleChecklistPlantillaNegocios
                '    Dim Coleccion = DetChkLstPlaNeg.DetalleChecklistPlantillaListaBusqueda("CHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND CHKLISPLA.cIdTipoMantenimiento = '" & cboTipoMantenimiento.SelectedValue & "' AND CHKLISPLA.cIdJerarquiaCatalogo", "1", Session("IdEmpresa"), "1")
                '    For Each Caracteristicas In dsCaracteristica.Rows
                '        Dim resultCaracteristicaSimple As DataRow() = Session("CestaActividadCatalogoComponenteChkList").Select("IdCatalogo = '" & Caracteristicas("cIdCatalogo").Trim & "' AND IdCaracteristica = '" & Caracteristicas("cIdCaracteristica").trim & "' AND IdJerarquia = '1'")
                '        If resultCaracteristicaSimple.Length = 0 Then
                '            'clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaActividadCatalogoComponenteChkList"))
                '            AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaActividadCatalogoComponenteChkList"))
                '        End If
                '        'clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaActividadCatalogoComponenteChkListFiltrado"))
                '        AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaActividadCatalogoComponenteChkListFiltrado"))
                '    Next
                'End If
            End If
            grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponenteChkListFiltrado")
            Me.grdDetalleActividadCatalogoComponente.DataBind()
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
        cboFiltroCheckList.DataTextField = "vDescripcionTablaSistema"
        cboFiltroCheckList.DataValueField = "vValor"
        cboFiltroCheckList.DataSource = FiltroNeg.TablaSistemaListarCombo("84", "LOGI", Session("IdEmpresa"))
        cboFiltroCheckList.Items.Clear()
        cboFiltroCheckList.DataBind()
    End Sub

    Sub ListarTipoMantenimientoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoMantNeg As New clsTipoMantenimientoNegocios
        cboTipoMantenimiento.DataTextField = "vDescripcionTipoMantenimiento"
        cboTipoMantenimiento.DataValueField = "cIdTipoMantenimiento"
        cboTipoMantenimiento.DataSource = TipoMantNeg.TipoMantenimientoListarCombo("1")
        cboTipoMantenimiento.Items.Clear()
        cboTipoMantenimiento.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboTipoMantenimiento.DataBind()

        cboTipoMantenimientoDuplicarPlantillaChecklist.DataTextField = "vDescripcionTipoMantenimiento"
        cboTipoMantenimientoDuplicarPlantillaChecklist.DataValueField = "cIdTipoMantenimiento"
        cboTipoMantenimientoDuplicarPlantillaChecklist.DataSource = TipoMantNeg.TipoMantenimientoListarCombo("1")
        cboTipoMantenimientoDuplicarPlantillaChecklist.Items.Clear()
        cboTipoMantenimientoDuplicarPlantillaChecklist.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboTipoMantenimientoDuplicarPlantillaChecklist.DataBind()
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

    Sub ListarActividadCheckListCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ActCheckListNeg As New clsActividadCheckListNegocios
        cboActividadCatalogoComponente.DataTextField = "vDescripcionActividadCheckList"
        cboActividadCatalogoComponente.DataValueField = "cIdActividadCheckList"
        cboActividadCatalogoComponente.DataSource = ActCheckListNeg.ActividadCheckListListarCombo()
        cboActividadCatalogoComponente.Items.Clear()
        cboActividadCatalogoComponente.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboActividadCatalogoComponente.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        cboTipoActivo.Enabled = bActivar
        cboCatalogo.Enabled = bActivar

        txtIdActividadMantenimientoActividad.Enabled = False
        txtDescripcionPlantilla.Enabled = bActivar
        txtFormatoArchivo.Enabled = bActivar
    End Sub

    Sub LlenarData()
        Dim CheckList As LOGI_CABECERACHECKLISTPLANTILLA = CheckListPlantillaNeg.ChecklistPlantillaListarPorId(Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim)
        cboTipoMantenimiento.SelectedValue = CheckList.cIdTipoMantenimiento
        cboTipoActivo.SelectedValue = CheckList.cIdTipoActivoCabeceraCheckListPlantilla
        cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())
        cboCatalogo.SelectedValue = CheckList.cIdCatalogoCabeceraCheckListPlantilla
        txtDescripcionPlantilla.Text = CheckList.vDescripcionCabeceraCheckListPlantilla
        txtFormatoArchivo.Text = CheckList.vFormatoArchivoCabeceraCheckListPlantilla
        hfdFechaTransaccion.Value = CheckList.dFechaTransaccionCabeceraCheckListPlantilla
        hfdIdNroDoc.Value = CheckList.cIdNumeroCabeceraCheckListPlantilla

        CargarCestaCatalogoComponente()
        CargarCestaActividadComponente()

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
        Me.rfvCatalogo.EnableClientScript = bValidar
        Me.rfvDescripcionPlantilla.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
    End Sub

    Sub LimpiarObjetos()
        cboTipoMantenimiento.SelectedIndex = -1
        cboTipoActivo.SelectedIndex = -1
        cboCatalogo.SelectedIndex = -1
        txtDescripcionPlantilla.Text = ""
        txtFormatoArchivo.Text = ""
        hfdEstado.Value = "1"
    End Sub

    Sub LimpiarObjetosTipoMantenimiento()
        txtIdMantenimientoTipoMantenimiento.Text = ""
        txtDescripcionMantenimientoTipoMantenimiento.Text = ""
        txtDescripcionAbreviadaMantenimientoTipoMantenimiento.Text = ""
        txtMesesDesdeContratoMantenimientoTipoMantenimiento.Text = ""
    End Sub

    Sub LimpiarObjetosActividad()
        txtIdActividadMantenimientoActividad.Text = ""
        txtDescripcionMantenimientoActividad.Text = ""
        txtDescripcionAbreviadaMantenimientoActividad.Text = ""
    End Sub

    Sub LlenarDataTipoMantenimiento()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosTipoMantenimiento()
            Else
                LimpiarObjetosTipoMantenimiento()
                Dim TipoMantNeg As New clsTipoMantenimientoNegocios
                Dim TipMan = TipoMantNeg.TipoMantenimientoListarPorId(cboTipoMantenimiento.SelectedValue)

                txtIdMantenimientoTipoMantenimiento.Text = TipMan.cIdTipoMantenimiento
                txtDescripcionMantenimientoTipoMantenimiento.Text = TipMan.vDescripcionTipoMantenimiento
                txtDescripcionAbreviadaMantenimientoTipoMantenimiento.Text = TipMan.vDescripcionAbreviadaTipoMantenimiento
                txtMesesDesdeContratoMantenimientoTipoMantenimiento.Text = TipMan.nMesesDesdeContratoTipoMantenimiento
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

    Sub LlenarDataActividad()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosActividad()
            Else
                LimpiarObjetosActividad()
                Dim ActividadNeg As New clsActividadCheckListNegocios
                Dim Actividad = ActividadNeg.ActividadCheckListListarPorId(cboActividadCatalogoComponente.SelectedValue)

                txtIdActividadMantenimientoActividad.Text = Actividad.cIdActividadCheckList
                txtDescripcionMantenimientoActividad.Text = Actividad.vDescripcionActividadCheckList
                txtDescripcionAbreviadaMantenimientoActividad.Text = Actividad.vDescripcionAbreviadaActividadCheckList
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "126" 'Mantenimiento de los CheckList Plantilla.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroCheckList.SelectedIndex = 3
            ListarTipoMantenimientoCombo()
            ListarTipoActivoCombo()
            ListarCatalogoCombo()
            ListarActividadCheckListCombo()

            If Session("CestaCatalogoComponente") Is Nothing Then
                Session("CestaCatalogoComponente") = clsLogiCestaCatalogo.CrearCesta()
            Else
                clsLogiCestaEquipo.VaciarCesta(Session("CestaCatalogoComponente"))
            End If

            If Session("CestaActividadCatalogoComponenteChkList") Is Nothing Then
                Session("CestaActividadCatalogoComponenteChkList") = CrearCesta()
            Else
                VaciarCesta(Session("CestaActividadCatalogoComponenteChkList"))
            End If

            BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlEquipo.Visible = False
            pnlComponentes.Visible = False
        Else
            txtBuscarCheckList.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcion')")
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0644", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True

            hfdOperacion.Value = "N"
            cboTipoMantenimiento.Focus()
            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            CargarCestaCatalogoComponente()
            CargarCestaActividadComponente()
            ValidarTexto(True)
            ActivarObjetos(True)
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

    Protected Sub btnAceptarMantenimientoTipoMantenimiento_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoTipoMantenimiento.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If txtIdMantenimientoTipoMantenimiento.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar un código personalizado en el tipo de mantenimiento.")
            End If
            Dim TipoMantenimiento As New LOGI_TIPOMANTENIMIENTO
            TipoMantenimiento.cIdTipoMantenimiento = UCase(txtIdMantenimientoTipoMantenimiento.Text)
            TipoMantenimiento.vDescripcionTipoMantenimiento = UCase(txtDescripcionMantenimientoTipoMantenimiento.Text)
            TipoMantenimiento.vDescripcionAbreviadaTipoMantenimiento = UCase(txtDescripcionAbreviadaMantenimientoTipoMantenimiento.Text)
            TipoMantenimiento.nMesesDesdeContratoTipoMantenimiento = Convert.ToDouble(IIf(txtMesesDesdeContratoMantenimientoTipoMantenimiento.Text.Trim = "", "0", txtMesesDesdeContratoMantenimientoTipoMantenimiento.Text))
            TipoMantenimiento.bEstadoRegistroTipoMantenimiento = True

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacionDetalle.Value = "N" Then
                If (TipoMantenimientoNeg.TipoMantenimientoInserta(TipoMantenimiento)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_TIPOMANTENIMIENTO 'SQL_INSERT', '','" & TipoMantenimiento.cIdTipoMantenimiento & "', '" &
                                                   TipoMantenimiento.vDescripcionTipoMantenimiento & "', '" & TipoMantenimiento.vDescripcionAbreviadaTipoMantenimiento & "', '" & TipoMantenimiento.nMesesDesdeContratoTipoMantenimiento & "', '" &
                                                   TipoMantenimiento.bEstadoRegistroTipoMantenimiento & "', '" & TipoMantenimiento.cIdTipoMantenimiento & "'"

                    LogAuditoria.vEvento = "INSERTAR TIPO MANTENIMIENTO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdMantenimientoTipoMantenimiento.Text = TipoMantenimiento.cIdTipoMantenimiento

                    MyValidator.ErrorMessage = "Transacción registrada con éxito"

                    BloquearMantenimiento(False, True, False, True)
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (TipoMantenimientoNeg.TipoMantenimientoEdita(TipoMantenimiento)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_TIPOMANTENIMIENTO 'SQL_UPDATE', '','" & TipoMantenimiento.cIdTipoMantenimiento & "', '" &
                                                   TipoMantenimiento.vDescripcionTipoMantenimiento & "', '" & TipoMantenimiento.vDescripcionAbreviadaTipoMantenimiento & "', '" & TipoMantenimiento.nMesesDesdeContratoTipoMantenimiento & "', '" &
                                                   TipoMantenimiento.bEstadoRegistroTipoMantenimiento & "', '" & TipoMantenimiento.cIdTipoMantenimiento & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR TIPO MANTENIMIENTO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    BloquearMantenimiento(False, True, False, True)
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacionDetalle.Value = "R"
            ListarTipoMantenimientoCombo()
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarMantenimientoTipoMantenimiento"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTipoMantenimiento"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoTipoMantenimiento_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub lnkbtnNuevoTipoMantenimiento_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoTipoMantenimiento.Click
        hfdOperacionDetalle.Value = "N"
        LlenarDataTipoMantenimiento()
        txtIdMantenimientoTipoMantenimiento.Enabled = True
        lnk_mostrarPanelMantenimientoTipoMantenimiento_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarTipoMantenimiento_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarTipoMantenimiento.Click
        Try
            MyValidator.ErrorMessage = ""

            If cboTipoMantenimiento.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de mantenimiento.")
            End If

            hfdOperacionDetalle.Value = "E"
            LlenarDataTipoMantenimiento()
            txtIdMantenimientoTipoMantenimiento.Enabled = False
            If MyValidator.ErrorMessage = "" Then
                lnk_mostrarPanelMantenimientoTipoMantenimiento_ModalPopupExtender.Show()
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

    Private Sub cboTipoActivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivo.SelectedIndexChanged
        ListarCatalogoCombo()
        CargarCestaCatalogoComponente() 'JMUG: 28/03/2023
    End Sub

    Private Sub cboCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCatalogo.SelectedIndexChanged
        CargarCestaCatalogoComponente()
    End Sub

    Private Sub grdCatalogoComponente_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCatalogoComponente.PageIndexChanging
        grdCatalogoComponente.PageIndex = e.NewPageIndex
        grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
        Me.grdCatalogoComponente.DataBind()
        grdCatalogoComponente.SelectedIndex = -1
    End Sub

    Private Sub grdCatalogoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdCatalogoComponente.SelectedIndexChanged
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.If grdCatalogoComponente.Rows.Count > 0 Then
            If grdCatalogoComponente.SelectedIndex < grdCatalogoComponente.Rows.Count Then
                If IsNothing(grdCatalogoComponente.SelectedRow) = False Then
                    If IsReference(grdCatalogoComponente.SelectedRow.Cells(1).Text) = True Then
                        CargarCestaActividadComponenteTemporal()
                    End If
                Else
                    Throw New Exception("Debe de seleccionar algún item de la lista de componentes.")
                End If
            End If
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarActividadCatalogoComponente_Click(sender As Object, e As EventArgs) Handles btnAdicionarActividadCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0649", strOpcionModulo, "CMMS")

            If grdCatalogoComponente.Rows.Count > 0 Then
                If grdCatalogoComponente.SelectedIndex < grdCatalogoComponente.Rows.Count Then
                    If IsNothing(grdCatalogoComponente.SelectedRow) = False Then
                        If IsReference(grdCatalogoComponente.SelectedRow.Cells(1).Text) = True Then
                            For i = 0 To Session("CestaActividadCatalogoComponenteChkList").Rows.Count - 1
                                If (Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Codigo").ToString.Trim) = (cboActividadCatalogoComponente.SelectedValue.ToString.Trim) And Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdCatalogo").ToString.Trim = (Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                                    Throw New Exception("Actividad ya registrada, seleccione otro item.")
                                    Exit Sub
                                End If
                            Next
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar algún item de la lista de componentes.")
                    End If
                End If
            End If

            If cboActividadCatalogoComponente.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar una actividad del catálogo.")
                Exit Sub
            Else
                AgregarCesta(cboActividadCatalogoComponente.SelectedValue.Trim, UCase(cboActividadCatalogoComponente.SelectedItem.Text).Trim, Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim, "1", "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim, 0, Session("CestaActividadCatalogoComponenteChkList"))
                'AgregarCesta(cboActividadCatalogoComponente.SelectedValue.Trim, UCase(cboActividadCatalogoComponente.SelectedItem.Text).Trim, Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim, "1", "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim, 0, Session("CestaActividadCatalogoComponenteChkListTemporal"))
            End If
            Me.grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponenteChkList")
            Me.grdDetalleActividadCatalogoComponente.DataBind()
            cboActividadCatalogoComponente.SelectedIndex = -1
            grdDetalleActividadCatalogoComponente.SelectedIndex = -1
            MyValidator.ErrorMessage = "Actividad agregada con éxito."
            ValidationSummary4.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnEditarActividadCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarActividadCatalogoComponente.Click
        Try
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0677", strOpcionModulo, "CMMS")

            If cboActividadCatalogoComponente.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar una actividad del catálogo.")
            End If

            hfdOperacionDetalle.Value = "E"
            LlenarDataActividad()
            txtIdActividadMantenimientoActividad.Enabled = False
            If MyValidator.ErrorMessage = "" Then
                lnk_mostrarPanelMantenimientoActividad_ModalPopupExtender.Show()
            End If
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtUbicacionActividad_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""

            If Session("CestaActividadCatalogoComponenteChkListTemporal") Is Nothing Then
                Session("CestaActividadCatalogoComponenteChkListTemporal") = CrearCesta()
            Else
                VaciarCesta(Session("CestaActividadCatalogoComponenteChkListTemporal"))
            End If

            Dim iUbicacionFinal As Integer = 0
            Dim iUbicacionActual As Integer = 0
            For Each row As GridViewRow In grdDetalleActividadCatalogoComponente.Rows
                Dim txtUbicacionActividad As TextBox = TryCast(row.Cells(8).FindControl("txtUbicacionActividad"), TextBox)
                If txtUbicacionActividad.Text = "" Then
                    txtUbicacionActividad.Text = "0"
                End If

                'Inicio: Actualizo el valor de ubicacion en la variable de sesión
                Dim i As Integer
                For i = 0 To Session("CestaActividadCatalogoComponenteChkList").Rows.Count - 1
                    If Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Codigo") = row.Cells(3).Text And
                       Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdCatalogo") = row.Cells(5).Text And
                       Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdJerarquia") = row.Cells(6).Text Then
                        Session("CestaActividadCatalogoComponenteChkList").Rows(i)("OrdenUbicacion") = txtUbicacionActividad.Text
                        If txtUbicacionActividad.Text <> "0" Then
                            iUbicacionFinal = Convert.ToInt32(txtUbicacionActividad.Text) - 1
                            iUbicacionActual = i
                        End If
                        Exit For
                    End If
                Next
                'Final: Actualizo el valor de ubicacion en la variable de sesión
            Next

            'Inicio: Recorro todo el cursor y lo agrego en cursor temporal
            For i = 0 To Session("CestaActividadCatalogoComponenteChkList").Rows.Count - 1
                If i = iUbicacionFinal Then
                    AgregarCesta(Session("CestaActividadCatalogoComponenteChkList").Rows(iUbicacionActual)("Codigo"), Session("CestaActividadCatalogoComponenteChkList").Rows(iUbicacionActual)("Descripcion"), Session("CestaActividadCatalogoComponenteChkList").Rows(iUbicacionActual)("IdCatalogo"), Session("CestaActividadCatalogoComponenteChkList").Rows(iUbicacionActual)("IdJerarquia"), "1", Session("CestaActividadCatalogoComponenteChkList").Rows(iUbicacionActual)("DescripcionComponente"), 0, Session("CestaActividadCatalogoComponenteChkListTemporal"))
                End If
                If i <> iUbicacionActual Then
                    AgregarCesta(Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Codigo"), Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Descripcion"), Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdCatalogo"), Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdJerarquia"), "1", Session("CestaActividadCatalogoComponenteChkList").Rows(i)("DescripcionComponente"), 0, Session("CestaActividadCatalogoComponenteChkListTemporal"))
                End If
            Next
            'Final: Recorro todo el cursor y lo agrego en cursor temporal

            'Inicio: Cargo todo lo que tiene el temporal nuevamente a la Actividades
            If Session("CestaActividadCatalogoComponenteChkList") Is Nothing Then
                Session("CestaActividadCatalogoComponenteChkList") = CrearCesta()
            Else
                VaciarCesta(Session("CestaActividadCatalogoComponenteChkList"))
            End If
            For i = 0 To Session("CestaActividadCatalogoComponenteChkListTemporal").Rows.Count - 1
                AgregarCesta(Session("CestaActividadCatalogoComponenteChkListTemporal").Rows(i)("Codigo"), Session("CestaActividadCatalogoComponenteChkListTemporal").Rows(i)("Descripcion"), Session("CestaActividadCatalogoComponenteChkListTemporal").Rows(i)("IdCatalogo"), Session("CestaActividadCatalogoComponenteChkListTemporal").Rows(i)("IdJerarquia"), "1", Session("CestaActividadCatalogoComponenteChkListTemporal").Rows(i)("DescripcionComponente"), 0, Session("CestaActividadCatalogoComponenteChkList"))
            Next
            'Final: Cargo todo lo que tiene el temporal nuevamente a la Actividades

            Me.grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponenteChkList")
            Me.grdDetalleActividadCatalogoComponente.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleActividadCatalogoComponente_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleActividadCatalogoComponente.PageIndexChanging
        grdDetalleActividadCatalogoComponente.PageIndex = e.NewPageIndex
        grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponenteChkList")
        Me.grdDetalleActividadCatalogoComponente.DataBind()
        grdDetalleActividadCatalogoComponente.SelectedIndex = -1
    End Sub

    Private Sub btnAceptarMantenimientoActividad_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoActividad.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            Dim ActividadCheckList As New LOGI_ACTIVIDADCHECKLIST
            ActividadCheckList.cIdActividadCheckList = UCase(txtIdActividadMantenimientoActividad.Text)
            ActividadCheckList.vDescripcionActividadCheckList = UCase(txtDescripcionMantenimientoActividad.Text)
            ActividadCheckList.vDescripcionAbreviadaActividadCheckList = UCase(txtDescripcionAbreviadaMantenimientoActividad.Text)
            ActividadCheckList.bEstadoRegistroActividadCheckList = True

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacionDetalle.Value = "N" Then
                If (ActividadNeg.ActividadCheckListInserta(ActividadCheckList)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_ACTIVIDADCHECKLIST 'SQL_INSERT', '','" & ActividadCheckList.cIdActividadCheckList & "', '" &
                                                   ActividadCheckList.vDescripcionActividadCheckList & "', '" & ActividadCheckList.vDescripcionAbreviadaActividadCheckList & "', '" &
                                                   ActividadCheckList.bEstadoRegistroActividadCheckList & "', '" & ActividadCheckList.cIdActividadCheckList & "'"

                    LogAuditoria.vEvento = "INSERTAR ACTIVIDAD"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdActividadMantenimientoActividad.Text = ActividadCheckList.cIdActividadCheckList

                    MyValidator.ErrorMessage = "Transacción registrada con éxito"

                    BloquearMantenimiento(False, True, False, True)
                    rfvDescripcionMantenimientoActividad.EnableClientScript = False
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (ActividadNeg.ActividadCheckListEdita(ActividadCheckList)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_ACTIVIDADCHECKLIST 'SQL_UPDATE', '','" & ActividadCheckList.cIdActividadCheckList & "', '" &
                                                   ActividadCheckList.vDescripcionActividadCheckList & "', '" & ActividadCheckList.vDescripcionAbreviadaActividadCheckList & "', '" &
                                                   ActividadCheckList.bEstadoRegistroActividadCheckList & "', '" & ActividadCheckList.cIdActividadCheckList & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR ACTIVIDAD"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    BloquearMantenimiento(False, True, False, True)
                    rfvDescripcionMantenimientoActividad.EnableClientScript = False
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacionDetalle.Value = "R"
            ListarActividadCheckListCombo()
            ValidationSummary3.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoActividad"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoActividad"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoActividad_ModalPopupExtender.Show()
        End Try
    End Sub

    Function fValidacionTipoMantenimiento(ByVal IdTipoMantenimiento As String, ByVal IdTipoActivo As String, ByVal IdCatalogo As String) As Boolean
        Dim dtChkList = CheckListPlantillaNeg.ChecklistPlantillaGetData("SELECT COUNT(*) FROM LOGI_CABECERACHECKLISTPLANTILLA WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "' AND cIdTipoActivoCabeceraCheckListPlantilla = '" & IdTipoActivo & "' AND cIdCatalogoCabeceraCheckListPlantilla = '" & IdCatalogo & "' AND cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0'")
        Return IIf(dtChkList.Rows(0).Item(0) > 0, True, False)
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0646", strOpcionModulo, "CMMS")

            If cboTipoMantenimiento.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de mantenimiento.")
            ElseIf cboTipoActivo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de activo.")
            ElseIf cboCatalogo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un catalogo principal.")
            End If

            Dim ChkList As New LOGI_CABECERACHECKLISTPLANTILLA
            ChkList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
            ChkList.cIdNumeroCabeceraCheckListPlantilla = IIf(hfdOperacion.Value = "N", "", hfdIdNroDoc.Value)
            ChkList.cIdCatalogoCabeceraCheckListPlantilla = cboCatalogo.SelectedValue
            ChkList.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = "0"
            ChkList.cIdTipoActivoCabeceraCheckListPlantilla = cboTipoActivo.SelectedValue
            ChkList.dFechaTransaccionCabeceraCheckListPlantilla = Convert.ToDateTime(IIf(hfdOperacion.Value = "N", Now, hfdFechaTransaccion.Value))
            ChkList.bEstadoRegistroCabeceraCheckListPlantilla = True
            ChkList.vDescripcionCabeceraCheckListPlantilla = UCase(txtDescripcionPlantilla.Text.Trim)
            ChkList.vFormatoArchivoCabeceraCheckListPlantilla = UCase(txtFormatoArchivo.Text.Trim)

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

            If fValidacionTipoMantenimiento(cboTipoMantenimiento.SelectedValue, cboTipoActivo.SelectedValue, cboCatalogo.SelectedValue) And hfdOperacion.Value = "N" Then
                lblDescripcionTipoMantenimientoMensajeDocumentoValidacion.Text = cboTipoMantenimiento.SelectedItem.Text.Trim
                lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender.Show()
                Exit Sub
            End If

            If hfdOperacion.Value = "N" Then
                If CheckListPlantillaNeg.ChecklistPlantillaInserta(ChkList) = 0 Then

                    Dim CheckListDetallePlantillaNeg As New clsDetalleChecklistPlantillaNegocios

                    Dim Coleccion As New List(Of LOGI_DETALLECHECKLISTPLANTILLA)
                    For i = 0 To Session("CestaActividadCatalogoComponenteChkList").Rows.Count - 1
                        Dim CheckList As New LOGI_DETALLECHECKLISTPLANTILLA
                        CheckList.cIdNumeroCabeceraCheckListPlantilla = ChkList.cIdNumeroCabeceraCheckListPlantilla
                        CheckList.cIdActividadCheckList = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Codigo").ToString.Trim
                        CheckList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
                        CheckList.cIdCatalogo = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdCatalogo").ToString.Trim 'cboCatalogo.SelectedValue
                        CheckList.cIdJerarquiaCatalogo = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdJerarquia").ToString.Trim '"1"
                        CheckList.nIdNumeroItemDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Item").ToString.Trim
                        CheckList.vDescripcionDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Descripcion").ToString.Trim
                        CheckList.bEstadoRegistroDetalleCheckListPlantilla = 1
                        CheckList.cIdTipoActivo = cboTipoActivo.SelectedValue
                        Coleccion.Add(CheckList)
                    Next

                    If CheckListDetallePlantillaNeg.DetalleChecklistPlantillaInsertaDetalle(ChkList, Coleccion, LogAuditoria) = 0 Then
                        Session("Query") = "PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA 'SQL_INSERT', '','" & ChkList.cIdTipoMantenimiento & "', '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "', '" &
                                        ChkList.dFechaTransaccionCabeceraCheckListPlantilla & "', '" & ChkList.bEstadoRegistroCabeceraCheckListPlantilla & "', '" & ChkList.cIdTipoActivoCabeceraCheckListPlantilla & "', '" &
                                        ChkList.cIdCatalogoCabeceraCheckListPlantilla & "', '" & ChkList.cIdJerarquiaCatalogoCabeceraCheckListPlantilla & "', '" &
                                        ChkList.vDescripcionCabeceraCheckListPlantilla & "', '" & ChkList.vFormatoArchivoCabeceraCheckListPlantilla & "', '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "'"

                        LogAuditoria.vEvento = "INSERTAR CHECK LIST PLANTILLA"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        MyValidator.ErrorMessage = "Transacción registrada con éxito"

                        Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                        Me.grdLista.DataBind()

                        pnlCabecera.Visible = True
                        pnlEquipo.Visible = False
                        pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarCheckList.Focus()
                    End If
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If CheckListPlantillaNeg.ChecklistPlantillaEdita(ChkList) = 0 Then

                    Dim CheckListDetallePlantillaNeg As New clsDetalleChecklistPlantillaNegocios

                    Dim Coleccion As New List(Of LOGI_DETALLECHECKLISTPLANTILLA)
                    For i = 0 To Session("CestaActividadCatalogoComponenteChkList").Rows.Count - 1
                        Dim CheckList As New LOGI_DETALLECHECKLISTPLANTILLA
                        CheckList.cIdNumeroCabeceraCheckListPlantilla = ChkList.cIdNumeroCabeceraCheckListPlantilla
                        CheckList.cIdActividadCheckList = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Codigo").ToString.Trim
                        CheckList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
                        CheckList.cIdCatalogo = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdCatalogo").ToString.Trim 'cboCatalogo.SelectedValue
                        CheckList.cIdJerarquiaCatalogo = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdJerarquia").ToString.Trim '"1"
                        CheckList.nIdNumeroItemDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Item").ToString.Trim
                        CheckList.vDescripcionDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Descripcion").ToString.Trim
                        CheckList.bEstadoRegistroDetalleCheckListPlantilla = 1
                        CheckList.cIdTipoActivo = cboTipoActivo.SelectedValue
                        Coleccion.Add(CheckList)
                    Next

                    If CheckListDetallePlantillaNeg.DetalleChecklistPlantillaInsertaDetalle(ChkList, Coleccion, LogAuditoria) = 0 Then
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                        Me.grdLista.DataBind()

                        pnlCabecera.Visible = True
                        pnlEquipo.Visible = False
                        pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarCheckList.Focus()
                    End If
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

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'IdTipoMantenimiento
            e.Row.Cells(1).Visible = False 'IdNumero
            e.Row.Cells(2).Visible = True 'IdCatalogo
            e.Row.Cells(3).Visible = False 'IdJerarquiaCatalogo
            e.Row.Cells(4).Visible = True 'FormatoArchivo
            e.Row.Cells(5).Visible = True 'Descripcion
            e.Row.Cells(6).Visible = True 'DescripcionTipoMantenimiento
            e.Row.Cells(7).Visible = True 'DescripcionCatalogo
            e.Row.Cells(8).Visible = True 'Estado
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'IdTipoMantenimiento
            e.Row.Cells(1).Visible = False 'IdNumero
            e.Row.Cells(2).Visible = True 'IdCatalogo
            e.Row.Cells(3).Visible = False 'IdJerarquiaCatalogo
            e.Row.Cells(4).Visible = True 'FormatoArchivo
            e.Row.Cells(5).Visible = True 'Descripcion
            e.Row.Cells(6).Visible = True 'DescripcionTipoMantenimiento
            e.Row.Cells(7).Visible = True 'DescripcionCatalogo
            e.Row.Cells(8).Visible = True 'Estado
        End If
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            lnkbtnImprimirPlantilla.Attributes.Add("onclick", "javascript:popupEmitirCheckListPlantillaReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "');")
                            If MyValidator.ErrorMessage = "" Then
                                MyValidator.ErrorMessage = "Registro encontrado con éxito"
                            End If
                        End If
                    End If
                Else

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

    Protected Sub grdLista_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles grdLista.RowCommand
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim ChkList As New LOGI_CABECERACHECKLISTPLANTILLA
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0648", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                ChkList.cIdTipoMantenimiento = Valores(0).ToString()
                ChkList.cIdNumeroCabeceraCheckListPlantilla = Valores(1).ToString()

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

                CheckListPlantillaNeg.ChecklistPlantillaGetData("UPDATE LOGI_CABECERACHECKLISTPLANTILLA SET bEstadoRegistroCabeceraCheckListPlantilla = 0 WHERE cIdTipoMantenimiento = '" & ChkList.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CABECERACHECKLISTPLANTILLA SET bEstadoRegistroCabeceraCheckListPlantilla = 0 WHERE cIdTipoMantenimiento = '" & ChkList.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "DESACTIVAR CABECERA CHECKLIST PLANTILLA"
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

                Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim ChkList As New LOGI_CABECERACHECKLISTPLANTILLA
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0648", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                ChkList.cIdTipoMantenimiento = Valores(0).ToString()
                ChkList.cIdNumeroCabeceraCheckListPlantilla = Valores(1).ToString()

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

                CheckListPlantillaNeg.ChecklistPlantillaGetData("UPDATE LOGI_CABECERACHECKLISTPLANTILLA SET bEstadoRegistroCabeceraCheckListPlantilla = 1 WHERE cIdTipoMantenimiento = '" & ChkList.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CABECERACHECKLISTPLANTILLA SET bEstadoRegistroCabeceraCheckListPlantilla = 1 cIdTipoMantenimiento = '" & ChkList.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "ACTIVAR CABECERA CHECKLIST PLANTILLA"
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

                Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Activar" OrElse e.CommandName = "Desactivar" Then
                ' Aquí va la lógica para realizar las acciones correspondientes a Activar o Desactivar
                updpnnlEquipo.Update() ' Actualizar el UpdatePanel
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
        Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0645", strOpcionModulo, "CMMS")

            hfdOperacion.Value = "E"
            BloquearMantenimiento(False, True, False, True)
            ValidarTexto(True)
            ActivarObjetos(True)
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            LlenarData()
                        End If
                    Else
                        BloquearMantenimiento(True, False, True, False)
                        Throw New Exception("Seleccione una plantilla para mostrar su detalle.")
                    End If
                Else
                    BloquearMantenimiento(True, False, True, False)
                    Throw New Exception("Seleccione una plantilla.")
                End If
            End If
            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True

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

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0647", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = True
            pnlEquipo.Visible = False
            pnlComponentes.Visible = False
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

    Private Sub btnSiMensajeDocumentoValidacion_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumentoValidacion.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0646", strOpcionModulo, Session("IdSistema"))

            Dim ChkList As New LOGI_CABECERACHECKLISTPLANTILLA
            ChkList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
            ChkList.cIdNumeroCabeceraCheckListPlantilla = ""
            ChkList.cIdCatalogoCabeceraCheckListPlantilla = cboCatalogo.SelectedValue
            ChkList.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = "0"
            ChkList.cIdTipoActivoCabeceraCheckListPlantilla = cboTipoActivo.SelectedValue
            ChkList.dFechaTransaccionCabeceraCheckListPlantilla = Now
            ChkList.bEstadoRegistroCabeceraCheckListPlantilla = True
            ChkList.vDescripcionCabeceraCheckListPlantilla = UCase(txtDescripcionPlantilla.Text.Trim)
            ChkList.vFormatoArchivoCabeceraCheckListPlantilla = UCase(txtFormatoArchivo.Text.Trim)

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

            If hfdOperacion.Value = "N" Then
                If CheckListPlantillaNeg.ChecklistPlantillaInserta(ChkList) = 0 Then
                    Dim CheckListDetallePlantillaNeg As New clsDetalleChecklistPlantillaNegocios
                    Dim Coleccion As New List(Of LOGI_DETALLECHECKLISTPLANTILLA)
                    For i = 0 To Session("CestaActividadCatalogoComponenteChkList").Rows.Count - 1
                        Dim CheckList As New LOGI_DETALLECHECKLISTPLANTILLA
                        CheckList.cIdNumeroCabeceraCheckListPlantilla = ChkList.cIdNumeroCabeceraCheckListPlantilla
                        CheckList.cIdActividadCheckList = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Codigo").ToString.Trim
                        CheckList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
                        'JMUG: 22/10/2025 CheckList.cIdCatalogo = cboCatalogo.SelectedValue
                        CheckList.cIdCatalogo = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdCatalogo").ToString.Trim 'cboCatalogo.SelectedValue
                        'JMUG: 22/10/2025 CheckList.cIdJerarquiaCatalogo = "1"
                        CheckList.cIdJerarquiaCatalogo = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdJerarquia").ToString.Trim '"1"
                        CheckList.nIdNumeroItemDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Item").ToString.Trim
                        CheckList.vDescripcionDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Descripcion").ToString.Trim
                        CheckList.bEstadoRegistroDetalleCheckListPlantilla = 1
                        CheckList.cIdTipoActivo = cboTipoActivo.SelectedValue
                        Coleccion.Add(CheckList)
                    Next

                    If CheckListDetallePlantillaNeg.DetalleChecklistPlantillaInsertaDetalle(ChkList, Coleccion, LogAuditoria) = 0 Then
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                        Me.grdLista.DataBind()

                        pnlCabecera.Visible = True
                        pnlEquipo.Visible = False
                        pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarCheckList.Focus()
                    End If
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

    Private Sub grdDetalleActividadCatalogoComponente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleActividadCatalogoComponente.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "489", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            For Each row As GridViewRow In grdDetalleActividadCatalogoComponente.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleActividadCatalogoComponente"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaActividadCatalogoComponenteChkList").Rows.Count - 1
                            If (Session("CestaActividadCatalogoComponenteChkList").Rows(i)("Codigo").ToString.Trim) = row.Cells(3).Text.ToString.Trim And (Session("CestaActividadCatalogoComponenteChkList").Rows(i)("IdCatalogo").ToString.Trim) = row.Cells(5).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaActividadCatalogoComponenteChkList"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            Me.grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponenteChkList")
            Me.grdDetalleActividadCatalogoComponente.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleActividadCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleActividadCatalogoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleActividadCatalogoComponente
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = False 'Codigo
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = False 'IdJerarquia
            e.Row.Cells(7).Visible = True 'DescripcionComponente
            e.Row.Cells(8).Visible = True 'Ubic
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleActividadCatalogoComponente
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = False 'Codigo
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = False 'IdJerarquia
            e.Row.Cells(7).Visible = True 'DescripcionComponente
            e.Row.Cells(8).Visible = True 'Ubic
        End If
    End Sub

    Private Sub grdCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCatalogoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = False 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = False 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
    End Sub

    Private Sub lnkbtnNuevoActividadCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoActividadCatalogoComponente.Click
        hfdOperacionDetalle.Value = "N"
        LlenarDataActividad()
        rfvDescripcionMantenimientoActividad.EnableClientScript = True
        txtIdActividadMantenimientoActividad.Enabled = False
        lnk_mostrarPanelMantenimientoActividad_ModalPopupExtender.Show()
    End Sub

    Private Sub imgbtnBuscarCheckList_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCheckList.Click
        Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
        Me.grdLista.DataBind()
    End Sub

    Private Sub lnkbtnImprimirPlantilla_Click(sender As Object, e As EventArgs) Handles lnkbtnImprimirPlantilla.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            'hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            'If MyValidator.ErrorMessage = "" Then
                            '    Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
                            'End If
                        End If
                    Else
                        Throw New Exception("Seleccione un registro para ver la plantilla.")
                    End If
                Else
                    Throw New Exception("Seleccione un registro.")
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

    Private Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0696", strOpcionModulo, "CMMS") 'Session("IdSistema"))

            If grdLista.Rows.Count = 0 Then
                MyValidator.ErrorMessage = "No existen registros para duplicar."
                hfdOperacion.Value = "R"
            Else
                If IsNothing(grdLista.SelectedRow) = True Then
                    MyValidator.ErrorMessage = "Seleccione un registro a visualizar."
                    hfdOperacion.Value = "R"
                Else
                    cboTipoMantenimientoDuplicarPlantillaChecklist.SelectedValue = Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text)
                    txtDescripcionDuplicarPlantillaChecklist.Text = ""
                    txtFormatoArchivoDuplicarPlantillaChecklist.Text = ""
                    lnk_mostrarPanelDuplicarPlantillaChecklist_ModalPopupExtender.Show()
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

    Protected Sub btnAceptarDuplicarPlantillaChecklist_Click(sender As Object, e As EventArgs) Handles btnAceptarDuplicarPlantillaChecklist.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0696", strOpcionModulo, "CMMS") 'Session("IdSistema"))

            If cboTipoMantenimientoDuplicarPlantillaChecklist.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de mantenimiento.")
            End If

            If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                Dim ChkList As LOGI_CABECERACHECKLISTPLANTILLA = CheckListPlantillaNeg.ChecklistPlantillaListarPorId(grdLista.SelectedRow.Cells(0).Text, grdLista.SelectedRow.Cells(1).Text)
                ChkList.cIdNumeroCabeceraCheckListPlantilla = ""
                ChkList.vDescripcionCabeceraCheckListPlantilla = UCase(txtDescripcionDuplicarPlantillaChecklist.Text.Trim)
                ChkList.vFormatoArchivoCabeceraCheckListPlantilla = UCase(txtFormatoArchivoDuplicarPlantillaChecklist.Text.Trim)
                ChkList.dFechaTransaccionCabeceraCheckListPlantilla = Now
                ChkList.cIdTipoMantenimiento = cboTipoMantenimientoDuplicarPlantillaChecklist.SelectedValue

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
                If (CheckListPlantillaNeg.ChecklistPlantillaInserta(ChkList)) = 0 Then
                    Dim CheckListDetallePlantillaNeg As New clsDetalleChecklistPlantillaNegocios
                    Dim dtCheckListColeccion = CheckListPlantillaNeg.ChecklistPlantillaGetData("SELECT DETCHKLISPLA.cIdActividadCheckList, DETCHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla, DETCHKLISPLA.vDescripcionDetalleCheckListPlantilla, " &
                                                                                                            "       DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla, DETCHKLISPLA.cIdTipoActivo, DETCHKLISPLA.cIdTipoMantenimiento, DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, " &
                                                                                                            "       DETCHKLISPLA.cIdCatalogo, DETCHKLISPLA.cIdJerarquiaCatalogo " &
                                                                        "FROM LOGI_DETALLECHECKLISTPLANTILLA AS DETCHKLISPLA " &
                                                                        "WHERE DETCHKLISPLA.cIdTipoMantenimiento = '" & grdLista.SelectedRow.Cells(0).Text & "' AND DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & grdLista.SelectedRow.Cells(1).Text & "'" &
                                                                        "ORDER BY DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla")
                    Dim ColeccionCheckList As New List(Of LOGI_DETALLECHECKLISTPLANTILLA)
                    For Each row In dtCheckListColeccion.Rows
                        Dim DetCheckList As New LOGI_DETALLECHECKLISTPLANTILLA
                        DetCheckList.cIdActividadCheckList = row("cIdActividadCheckList")
                        DetCheckList.bEstadoRegistroDetalleCheckListPlantilla = row("bEstadoRegistroDetalleCheckListPlantilla").ToString
                        DetCheckList.vDescripcionDetalleCheckListPlantilla = row("vDescripcionDetalleCheckListPlantilla").ToString
                        DetCheckList.nIdNumeroItemDetalleCheckListPlantilla = row("nIdNumeroItemDetalleCheckListPlantilla").ToString
                        DetCheckList.cIdTipoActivo = row("cIdTipoActivo")
                        DetCheckList.cIdTipoMantenimiento = cboTipoMantenimientoDuplicarPlantillaChecklist.SelectedValue
                        DetCheckList.cIdNumeroCabeceraCheckListPlantilla = ChkList.cIdNumeroCabeceraCheckListPlantilla 'row("cIdNumeroCabeceraCheckListPlantilla")
                        DetCheckList.cIdCatalogo = row("cIdCatalogo")
                        DetCheckList.cIdJerarquiaCatalogo = row("cIdJerarquiaCatalogo")
                        ColeccionCheckList.Add(DetCheckList)
                    Next

                    If CheckListDetallePlantillaNeg.DetalleChecklistPlantillaInsertaDetalle(ChkList, ColeccionCheckList, LogAuditoria) = 0 Then
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    End If

                    Session("Query") = "PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA 'SQL_INSERT', '','" & ChkList.cIdTipoMantenimiento & "', '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "', '" &
                                ChkList.dFechaTransaccionCabeceraCheckListPlantilla & "', '" & ChkList.bEstadoRegistroCabeceraCheckListPlantilla & "', '" & ChkList.cIdTipoActivoCabeceraCheckListPlantilla & "', '" &
                                ChkList.cIdCatalogoCabeceraCheckListPlantilla & "', '" & ChkList.cIdJerarquiaCatalogoCabeceraCheckListPlantilla & "', '" &
                                ChkList.vDescripcionCabeceraCheckListPlantilla & "', '" & ChkList.vFormatoArchivoCabeceraCheckListPlantilla & "', '" & ChkList.cIdNumeroCabeceraCheckListPlantilla & "'"

                    LogAuditoria.vEvento = "DUPLICAR CHECK LIST PLANTILLA"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdMantenimientoTipoMantenimiento.Text = ChkList.cIdTipoMantenimiento
                    MyValidator.ErrorMessage = "Transacción duplicada con éxito"
                    Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    Me.grdLista.DataBind()
                    pnlCabecera.Visible = True
                    pnlEquipo.Visible = False
                    pnlComponentes.Visible = False

                    BloquearMantenimiento(True, False, True, False)
                    hfdOperacion.Value = "R"
                    txtBuscarCheckList.Focus()

                    ValidationSummary1.ValidationGroup = "vgrpValidar"
                    MyValidator.IsValid = False
                    MyValidator.ID = "ErrorPersonalizado"
                    MyValidator.ValidationGroup = "vgrpValidar"
                    Me.Page.Validators.Add(MyValidator)
                End If
            End If
        Catch ex As Exception
            ValidationSummary5.ValidationGroup = "vgrpValidarPlantillaChecklist"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarPlantillaChecklist"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelDuplicarPlantillaChecklist_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub lnkbtnVerActividadCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnVerActividadCatalogoComponente.Click
        Try
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0677", strOpcionModulo, "CMMS")

            grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponenteChkList")
            Me.grdDetalleActividadCatalogoComponente.DataBind()
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub
End Class