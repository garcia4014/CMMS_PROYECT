Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarEquipo
    Inherits System.Web.UI.Page
    Dim EquipoNeg As New clsEquipoNegocios
    Dim CatalogoNeg As New clsCatalogoNegocios
    Dim ContratoNeg As New clsContratoNegocios
    Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator
    Shared rowIndexDetalle As Int64
    Dim ClienteNeg As New clsClienteNegocios

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
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
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

    Sub CargarCestaCatalogoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoComponente")) 'Lo Cambie por el de arriba: 28/03/2023

        Dim CatalogoNeg As New clsCatalogoNegocios
        Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        'Dim Coleccion2 = EquipoNeg.EquipoListaBusquedaV2("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1", Session("IdContratoUsuario").ToString())

        Dim Coleccion2 As List(Of VI_LOGI_EQUIPO)
        If (Session("IdContratoUsuario") = "*") Then
            Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        ElseIf (Session("IdContratoUsuario") <> Nothing) Then
            Coleccion2 = EquipoNeg.EquipoListaBusquedaV2("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1", Session("IdContratoUsuario").ToString())
        End If

        Dim intContador As Integer = 0

        For Each Registro In Coleccion
            Dim booExiste As Boolean = False
            For Each Registro2 In Coleccion2
                If Registro.Codigo = Registro2.IdCatalogo And Registro.IdSistemaFuncional = Registro2.IdSistemaFuncional And Registro.IdTipoActivo = Registro2.IdTipoActivo Then
                    booExiste = True
                    Exit For
                End If
            Next
            If booExiste = False Then
                clsLogiCestaCatalogo.AgregarCesta(Registro.Codigo, Registro.IdTipoActivo, "1", Registro.IdSistemaFuncional,
                                                  cboCatalogo.SelectedValue, UCase(Registro.Descripcion),
                                                  UCase(Registro.DescripcionAbreviada), Registro.Estado, 0, Registro.IdCuentaContable, Registro.IdCuentaContableLeasing, Registro.DescripcionTipoActivo,
                                                  Registro.DescripcionSistemaFuncional, Registro.PeriodoGarantia, Registro.PeriodoMinimoMantenimiento,
                                                  Session("CestaCatalogoComponente"))
            End If
        Next

        grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
        Me.grdCatalogoComponente.DataBind()
    End Sub

    Sub CargarCestaCatalogoCaracteristica()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '0' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '0'")
            For Each Caracteristicas In dsCaracteristica.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "", "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
            Next
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaCaracteristicaCatalogoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & txtIdCatalogoComponente.Text & "' AND CATCAR.cIdJerarquiaCatalogo = '1'")
            For Each Caracteristicas In dsCaracteristica.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "", "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaCatalogoComponente"))
            Next
            Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponente")
            Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaCaracteristicaEquipoPrincipal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoPrincipal"))
            'JMUG: INICIO: 21/11/2023
            Dim dsCaracteristicaEquipoPrincipalExiste = CaracteristicaNeg.CaracteristicaGetData("SELECT COUNT(*) " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA " &
                                                                           "WHERE cIdEquipo = '" & txtIdEquipo.Text & "'")

            Dim dsCaracteristicaEquipoPrincipal
            If dsCaracteristicaEquipoPrincipalExiste.Rows(0).Item(0) = 0 Then
                'JMGU: 21/12/2023
                dsCaracteristicaEquipoPrincipal = CaracteristicaNeg.CaracteristicaGetData("SELECT '" & txtIdEquipo.Text & "' AS cIdEquipo, CAT.cIdCatalogo, '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "' AS cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica AS nIdNumeroItemEquipoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica AS cIdReferenciaSAPEquipoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica AS vDescripcionCampoSAPEquipoCaracteristica, CATCAR.vValorCatalogoCaracteristica AS vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_CATALOGO AS CAT ON CATCAR.cIdCatalogo = CAT.cIdCatalogo " &
                                                                           "     AND CATCAR.cIdJerarquiaCatalogo = CAT.cIdJerarquiaCatalogo " &
                                                                           "WHERE CAT.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CAT.bEstadoRegistroCatalogo = '1' AND CAT.cIdJerarquiaCatalogo = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "' ORDER BY CATCAR.nIdNumeroItemCatalogoCaracteristica")
            Else
                dsCaracteristicaEquipoPrincipal = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "' AS cIdJerarquiaCatalogo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                                                                           "WHERE EQU.cIdEquipo = '" & txtIdEquipo.Text & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
            End If
            'JMUG: FINAL: 21/11/2023

            For Each CaracteristicaEquipo In dsCaracteristicaEquipoPrincipal.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(CaracteristicaEquipo("cIdCatalogo"), CaracteristicaEquipo("cIdEquipo"), IIf(lnkEsComponenteOn.Visible = True, "1", "0"), CaracteristicaEquipo("cIdCaracteristica"), CaracteristicaEquipo("vDescripcionCaracteristica"), CaracteristicaEquipo("cIdReferenciaSAPEquipoCaracteristica"), CaracteristicaEquipo("vDescripcionCampoSAPEquipoCaracteristica"), CaracteristicaEquipo("vValorReferencialEquipoCaracteristica"), Session("CestaCaracteristicaEquipoPrincipal"))
            Next
            Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
            Me.grdDetalleCaracteristicaEquipo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaCaracteristicaEquipoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponente"))
            Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '1' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                                                                           "WHERE EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
            For Each CaracteristicaEquipo In dsCaracteristicaEquipoComponente.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(CaracteristicaEquipo("cIdCatalogo"), CaracteristicaEquipo("cIdEquipo"), "1", CaracteristicaEquipo("cIdCaracteristica"), CaracteristicaEquipo("vDescripcionCaracteristica"), CaracteristicaEquipo("cIdReferenciaSAPEquipoCaracteristica"), CaracteristicaEquipo("vDescripcionCampoSAPEquipoCaracteristica"), CaracteristicaEquipo("vValorReferencialEquipoCaracteristica"), Session("CestaCaracteristicaEquipoComponente"))
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

    Sub CargarCestaCaracteristicaEquipoComponenteTemporal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If
            If Session("CestaEquipoComponente").Rows.Count > 0 Then

                Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '1' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                                                                           "WHERE EQUCAR.cIdEquipo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdEquipo").ToString.Trim & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
                For Each CaracteristicaEquipo In dsCaracteristicaEquipoComponente.Rows
                    If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                        If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                            Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdJerarquia = '1'")
                            If resultCaracteristicaSimple.Length = 0 Then
                                Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Nothing
                            Else
                                Dim rowFil As DataRow() = resultCaracteristicaSimple
                                For Each fila As DataRow In rowFil
                                    clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdEquipo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                                Next
                                Exit Sub
                            End If
                        End If
                    End If
                Next
                If dsCaracteristicaEquipoComponente.Rows.Count = 0 Then

                    Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim & "' AND CATCAR.cIdJerarquiaCatalogo = '1'")
                    For Each Caracteristicas In dsCaracteristica.Rows
                        Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & Caracteristicas("cIdCatalogo").Trim & "' AND IdCaracteristica = '" & Caracteristicas("cIdCaracteristica").trim & "' AND IdJerarquia = '1'")
                        If resultCaracteristicaSimple.Length = 0 Then
                            clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaEquipoComponente"))
                        End If
                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                    Next
                End If
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

    Sub CargarCestaEquipoComponente()
        'Carga las opciones que aun no estan asignadas en la Grilla de Opciones del Módulo.
        Try
            VaciarCesta(Session("CestaEquipoComponente"))
            Dim EquipoActivoNeg As New clsEquipoNegocios

            If grdEquipoComponente.SelectedIndex >= (grdEquipoComponente.Rows.Count - 1) Then
                grdEquipoComponente.SelectedIndex = -1
            End If

            If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                    'Dim Coleccion = EquipoNeg.EquipoListaBusqueda("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
                    Dim Coleccion As List(Of VI_LOGI_EQUIPO)
                    If (Session("IdContratoUsuario") = "*") Then
                        Coleccion = EquipoNeg.EquipoListaBusqueda("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
                    ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                        Coleccion = EquipoNeg.EquipoListaBusquedaV2("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1, Session("IdContratoUsuario"))
                    End If

                    Dim intContador As Integer = 0

                    For Each Registro In Coleccion
                        Dim Equipo1 As New LOGI_EQUIPO
                        Equipo1 = EquipoNeg.EquipoListarPorIdDetalle(Registro.Codigo, Registro.IdCatalogo)

                        clsLogiCestaEquipo.AgregarCesta(Equipo1.cIdEquipo, Equipo1.cIdCatalogo, Equipo1.cIdTipoActivo, Equipo1.cIdJerarquiaCatalogo,
                                                Equipo1.cIdSistemaFuncionalEquipo, Equipo1.cIdEnlaceCatalogo, Equipo1.vDescripcionEquipo,
                                                Equipo1.vDescripcionAbreviadaEquipo, IIf(IsNothing(Equipo1.dFechaTransaccionEquipo), Now, Equipo1.dFechaTransaccionEquipo), Equipo1.bEstadoRegistroEquipo,
                                                Equipo1.cIdEnlaceEquipo, Equipo1.vObservacionEquipo, Equipo1.nVidaUtilEquipo,
                                                "", "", Equipo1.nPeriodoGarantiaEquipo, Equipo1.nPeriodoMinimoMantenimientoEquipo,
                                                Equipo1.vNumeroSerieEquipo, Equipo1.vNumeroParteEquipo, Equipo1.cIdEstadoComponenteEquipo,
                                               Session("CestaEquipoComponente"))

                        Dim TablaCestaMaestro As DataTable
                        TablaCestaMaestro = Session("CestaEquipoComponente")
                        Dim i As Integer

                        For i = 0 To TablaCestaMaestro.Rows.Count - 1
                            If TablaCestaMaestro.Rows(i)("IdTipoActivo") = cboTipoActivo.SelectedValue And
                               TablaCestaMaestro.Rows(i)("IdCatalogo") = cboCatalogo.SelectedValue Then
                                QuitarCesta(intContador, Session("CestaCatalogoComponente"))
                                intContador = intContador - 1
                            End If
                        Next
                        intContador = intContador + 1
                    Next
                End If
            Else
                'Dim Coleccion = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND EQU.cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
                Dim Coleccion As List(Of VI_LOGI_EQUIPO)

                If (Session("IdContratoUsuario") = "*") Then
                    Coleccion = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND EQU.cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
                ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                    Coleccion = EquipoNeg.EquipoListaBusquedaV2("EQU.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND EQU.cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1, Session("IdContratoUsuario"))
                End If

                Dim intContador As Integer = 0

                For Each Registro In Coleccion
                    Dim Equipo As LOGI_EQUIPO
                    Equipo = EquipoNeg.EquipoListarPorIdDetalle(Registro.Codigo, Registro.IdCatalogo)

                    'JMUG: 06/03/2023 - Inicio de Carga de última parte - REVISARRRR
                    clsLogiCestaEquipo.AgregarCesta(Equipo.cIdEquipo, Equipo.cIdCatalogo, Equipo.cIdTipoActivo, Equipo.cIdJerarquiaCatalogo,
                                                Equipo.cIdSistemaFuncionalEquipo, Equipo.cIdEnlaceCatalogo, Equipo.vDescripcionEquipo,
                                                Equipo.vDescripcionAbreviadaEquipo, IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo), Equipo.bEstadoRegistroEquipo,
                                                Equipo.cIdEnlaceEquipo, Equipo.vObservacionEquipo, Equipo.nVidaUtilEquipo,
                                                "", "", Equipo.nPeriodoGarantiaEquipo, Equipo.nPeriodoMinimoMantenimientoEquipo,
                                                Equipo.vNumeroSerieEquipo, Equipo.vNumeroParteEquipo, Equipo.cIdEstadoComponenteEquipo,
                                               Session("CestaEquipoComponente"))

                    Dim TablaCestaMaestro As DataTable
                    Dim TablaCestaCatalogo As DataTable
                    TablaCestaMaestro = Session("CestaEquipoComponente")
                    TablaCestaCatalogo = Session("CestaCatalogoComponente")
                    Dim i As Integer
                    For i = 0 To TablaCestaMaestro.Rows.Count - 1
                        For j = 0 To TablaCestaCatalogo.Rows.Count - 1
                            If TablaCestaMaestro.Rows(i)("IdJerarquia") = TablaCestaCatalogo.Rows(j)("IdJerarquia") And
                               TablaCestaMaestro.Rows(i)("IdCatalogo") = TablaCestaCatalogo.Rows(j)("IdCatalogo") Then
                                QuitarCesta(intContador, Session("CestaCatalogoComponente"))
                                intContador = intContador - 1
                                Exit For
                            End If
                        Next
                        intContador = intContador + 1
                    Next
                Next
            End If
            Me.grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            Me.grdCatalogoComponente.DataBind()
            Me.grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            Me.grdEquipoComponente.DataBind()
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
        cboFiltroEquipo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroEquipo.DataValueField = "vValor"
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("83", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
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

        cboTipoActivoMantenimientoCatalogo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoMantenimientoCatalogo.DataValueField = "cIdTipoActivo"
        cboTipoActivoMantenimientoCatalogo.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivoMantenimientoCatalogo.Items.Clear()
        cboTipoActivoMantenimientoCatalogo.Items.Add("SELECCIONE DATO")
        cboTipoActivoMantenimientoCatalogo.DataBind()

        cboTipoActivoCatalogoComponente.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoCatalogoComponente.DataValueField = "cIdTipoActivo"
        cboTipoActivoCatalogoComponente.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivoCatalogoComponente.Items.Clear()
        cboTipoActivoCatalogoComponente.Items.Add("SELECCIONE DATO")
        cboTipoActivoCatalogoComponente.DataBind()

        cboTipoActivoEquipoComponente.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoEquipoComponente.DataValueField = "cIdTipoActivo"
        cboTipoActivoEquipoComponente.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivoEquipoComponente.Items.Clear()
        cboTipoActivoEquipoComponente.Items.Add("SELECCIONE DATO")
        cboTipoActivoEquipoComponente.DataBind()
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

    Sub ListarArticuloSAPCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ArticuloSAPNeg As New clsArticuloSAPNegocios
        cboArticuloSAPEquipo.DataTextField = "vDescripcionArticuloSAP"
        cboArticuloSAPEquipo.DataValueField = "vIdArticuloSAP"
        cboArticuloSAPEquipo.DataSource = ArticuloSAPNeg.ArticuloSAPListarCombo("1")
        cboArticuloSAPEquipo.Items.Clear()
        cboArticuloSAPEquipo.Items.Add("SELECCIONE DATO")
        cboArticuloSAPEquipo.DataBind()
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

    Sub ListarSistemaFuncionalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncional.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncional.DataValueField = "cIdSistemaFuncional"
        cboSistemaFuncional.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        cboSistemaFuncional.Items.Clear()
        cboSistemaFuncional.DataBind()
    End Sub

    Sub ListarSistemaFuncionalCatalogoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncionalCatalogoComponente.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncionalCatalogoComponente.DataValueField = "cIdSistemaFuncional"
        cboSistemaFuncionalCatalogoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        cboSistemaFuncionalCatalogoComponente.Items.Clear()
        cboSistemaFuncionalCatalogoComponente.DataBind()
    End Sub

    Sub ListarSistemaFuncionalEquipoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncionalEquipoComponente.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncionalEquipoComponente.DataValueField = "cIdSistemaFuncional"
        cboSistemaFuncionalEquipoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        cboSistemaFuncionalEquipoComponente.Items.Clear()
        cboSistemaFuncionalEquipoComponente.DataBind()
    End Sub

    Sub ListarCaracteristicaEquipoPrincipalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaEquipo.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaEquipo.DataValueField = "cIdCaracteristica"
        cboCaracteristicaEquipo.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaEquipo.Items.Clear()
        cboCaracteristicaEquipo.DataBind()
    End Sub

    Sub ListarCaracteristicaCatalogoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaCatalogoComponente.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaCatalogoComponente.DataValueField = "cIdCaracteristica"
        cboCaracteristicaCatalogoComponente.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaCatalogoComponente.Items.Clear()
        cboCaracteristicaCatalogoComponente.DataBind()
    End Sub

    Sub ListarCaracteristicaEquipoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaEquipoComponente.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaEquipoComponente.DataValueField = "cIdCaracteristica"
        cboCaracteristicaEquipoComponente.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaEquipoComponente.Items.Clear()
        cboCaracteristicaEquipoComponente.DataBind()
    End Sub

    Sub ListarDescripcionCatalogoComponente()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CatalogoNeg As New clsCatalogoNegocios
        cboDescripcionCatalogoComponente.DataTextField = "vDescripcionCatalogo"
        cboDescripcionCatalogoComponente.DataValueField = "cIdCatalogo"
        cboDescripcionCatalogoComponente.DataSource = CatalogoNeg.CatalogoListarDescripcionCombo()
        cboDescripcionCatalogoComponente.Items.Clear()
        cboDescripcionCatalogoComponente.Items.Add(New ListItem("SELECCIONE DATO", ""))
        cboDescripcionCatalogoComponente.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdEquipo.Enabled = False 'bActivar
        txtDescripcionEquipo.Enabled = False
        txtNroSerieEquipo.Enabled = bActivar
        txtNroParteEquipo.Enabled = bActivar
        txtTagEquipo.Enabled = bActivar
        txtCapacidadEquipo.Enabled = bActivar
        cboArticuloSAPEquipo.Enabled = bActivar
        cboTipoActivo.Enabled = bActivar
        cboTipoEquipo.Enabled = bActivar
        txtDescripcionEquipoSAP.Enabled = bActivar
        cboCatalogo.Enabled = bActivar
        cboSistemaFuncional.Enabled = bActivar
        divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        divContratoReferencia.Visible = IIf(lnkEsConContratoOn.Visible = True, True, False)
        txtIdCliente.Enabled = bActivar
        txtRazonSocial.Enabled = False
        'cboTipoActivo.Enabled = bActivar

        'Mantenimiento Tipo Activo
        txtIdTipoActivoMantenimientoTipoActivo.Enabled = False
        txtDescripcionMantenimientoTipoActivo.Enabled = bActivar
        txtDescripcionAbreviadaMantenimientoTipoActivo.Enabled = bActivar

        'Mantenimiento de Catalogo
        txtIdCatalogoMantenimientoCatalogo.Enabled = False
        cboTipoActivoMantenimientoCatalogo.Enabled = bActivar
        txtDescripcionMantenimientoCatalogo.Enabled = bActivar
        txtDescripcionAbreviadaMantenimientoCatalogo.Enabled = bActivar
        txtVidaUtilMantenimientoCatalogo.Enabled = bActivar
        txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        txtCuentaContableMantenimientoCatalogo.Enabled = bActivar
        divCuentaContableMantenimientoCatalogo.Visible = False
        txtCuentaContableLeasingMantenimientoCatalogo.Enabled = bActivar
        divCuentaContableLeasingMantenimientoCatalogo.Visible = False

        'Mantenimiento de Catalogo / Características

        'Mantenimiento de Equipo

        'Mantenimiento de Equipo / Componente

        'Mantenimiento de Catalogo / Componente
        txtIdCatalogoComponente.Enabled = False
        cboTipoActivoCatalogoComponente.Enabled = bActivar
        cboSistemaFuncionalCatalogoComponente.Enabled = bActivar
        txtDescripcionCatalogoComponente.Enabled = bActivar
        txtDescripcionCatalogoComponente.Enabled = bActivar
        txtVidaUtilCatalogoComponente.Enabled = bActivar
        txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        txtCuentaContableCatalogoComponente.Enabled = bActivar
        txtCuentaContableLeasingCatalogoComponente.Enabled = bActivar

        'Mantenimiento de Sistema Funcional / Componente
        txtIdSistemaFuncionalMantenimientoSistemaFuncional.Enabled = False
        txtDescripcionMantenimientoSistemaFuncional.Enabled = bActivar
        txtDescripcionAbreviadaMantenimientoSistemaFuncional.Enabled = bActivar

    End Sub

    Sub LlenarData()
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text).Trim)
        txtIdEquipo.Text = Equipo.cIdEquipo
        hfdFechaEquipo.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
        txtDescripcionEquipo.Text = Equipo.vDescripcionEquipo
        txtNroSerieEquipo.Text = Equipo.vNumeroSerieEquipo
        txtNroParteEquipo.Text = Equipo.vNumeroParteEquipo
        txtTagEquipo.Text = Equipo.vTagEquipo
        txtCapacidadEquipo.Text = Equipo.vCapacidadEquipo
        txtAreaEquipo.Text = Equipo.vAreaEquipo
        lnkEsComponenteOn.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", False, True)
        lnkEsComponenteOff.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", True, False)
        lnkEsConContratoOn.Visible = IIf(CBool(Equipo.bTieneContratoEquipo) = False, False, True)
        lnkEsConContratoOff.Visible = IIf(CBool(Equipo.bTieneContratoEquipo) = False, True, False)

        If Equipo.cIdCliente.Trim <> "" Then
            ListarContratoReferenciaCombo(Equipo.cIdCliente, True)
        End If

        If CBool(Equipo.bTieneContratoEquipo) = False Then

            cboContratoReferencia.SelectedIndex = -1
        Else
            cboContratoReferencia.SelectedValue = Equipo.vContratoReferenciaActualEquipo
        End If

        divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        divContratoReferencia.Visible = IIf(CBool(Equipo.bTieneContratoEquipo) = False, False, True)
        If Equipo.cIdCliente.Trim <> "" Then
            Dim ClienteNeg As New clsClienteNegocios
            Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorId(Equipo.cIdCliente, Session("IdEmpresa"), "*")
            txtIdCliente.Text = Cliente.vRucCliente
            btnAdicionarCliente_Click(btnAdicionarCliente, New System.EventArgs())
        End If
        ListarClienteUbicacionCombo()
        cboClienteUbicacion.SelectedValue = IIf(Trim(Equipo.cIdClienteUbicacion) = "", "SELECCIONE DATO", Equipo.cIdClienteUbicacion)
        If Equipo.vIdArticuloSAPEquipo Is Nothing Then
            cboArticuloSAPEquipo.SelectedIndex = -1
        Else
            cboArticuloSAPEquipo.SelectedValue = Equipo.vIdArticuloSAPEquipo
        End If
        cboTipoActivo.SelectedValue = Equipo.cIdTipoActivo
        cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())
        cboTipoEquipo.SelectedValue = Equipo.cIdTipoEquipo
        txtDescripcionEquipoSAP.Text = Equipo.vDescripcionEquipoSAP

        'Iterar a través de los elementos del DropDownList
        If Trim(Equipo.cIdCatalogo) <> "" Then
            Dim bEncontrado As Boolean = False
            For Each item As ListItem In cboCatalogo.Items
                'Comparar el valor con el valor que estás buscando
                If item.Value = Equipo.cIdCatalogo Then
                    bEncontrado = True
                    Exit For 'Salir del bucle una vez que encuentres el valor
                End If
            Next

            If bEncontrado = False Then
                'El valor no fue encontrado, realiza la lógica que necesites
                Dim Cat As LOGI_CATALOGO = CatalogoNeg.CatalogoListarPorId(Equipo.cIdCatalogo, Equipo.cIdTipoActivo, Equipo.cIdJerarquiaCatalogo, "*")
                cboCatalogo.Items.Add(New ListItem(Cat.vDescripcionCatalogo, Cat.cIdCatalogo))
            End If
        End If

        cboCatalogo.SelectedValue = Equipo.cIdCatalogo
        ListarSistemaFuncionalCombo()
        CargarCestaCatalogoComponente()
        CargarCestaEquipoComponente()
        CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 20/03/2023
        CargarCestaCaracteristicaEquipoComponente()
        CargarCestaCaracteristicaEquipoComponenteTemporal()
        Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
        Me.grdDetalleCaracteristicaEquipoComponente.DataBind()

        hfdIdClienteSAPEquipo.Value = Equipo.vIdClienteSAPEquipo
        hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
        hfdIdArticuloSAPEquipo.Value = Equipo.vIdArticuloSAPEquipo
        cboArticuloSAPEquipo.SelectedValue = hfdIdArticuloSAPEquipo.Value
        hfdFechaRegistroTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaRegistroTarjetaSAPEquipo), "", Equipo.dFechaRegistroTarjetaSAPEquipo)
        hfdFechaManufacturaTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaManufacturaTarjetaSAPEquipo), "", Equipo.dFechaManufacturaTarjetaSAPEquipo)
        hfdFechaCreacionEquipo.Value = IIf(IsNothing(Equipo.dFechaCreacionEquipo), Now, Equipo.dFechaCreacionEquipo)
        hfdIdUsuarioCreacionEquipo.Value = IIf(IsNothing(Equipo.cIdUsuarioCreacionEquipo), Session("IdUsuario"), Equipo.cIdUsuarioCreacionEquipo)
        Dim script As String = String.Format("var valorAnterior = '{0}';", cboCatalogo.SelectedValue)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AsignarValorAnterior", script, True)

        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
    End Sub

    Sub LlenarDataTipoActivo()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosTipoActivo()
            Else
                LimpiarObjetosTipoActivo()
                Dim TipoActivoNeg As New clsTipoActivoNegocios
                Dim TipoActivo As LOGI_TIPOACTIVO = TipoActivoNeg.TipoActivoListarPorId(cboTipoActivo.SelectedValue)
                txtIdTipoActivoMantenimientoTipoActivo.Text = TipoActivo.cIdTipoActivo
                txtDescripcionMantenimientoTipoActivo.Text = TipoActivo.vDescripcionTipoActivo
                txtDescripcionAbreviadaMantenimientoTipoActivo.Text = TipoActivo.vDescripcionAbreviadaTipoActivo
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

    Sub LlenarDataCatalogo()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosCatalogo()
            Else
                LimpiarObjetosCatalogo()
                Dim CatalogoNeg As New clsCatalogoNegocios
                Dim Catalogo = CatalogoNeg.CatalogoListarPorId(cboCatalogo.SelectedValue, cboTipoActivo.SelectedValue, "0", "1")

                txtIdCatalogoMantenimientoCatalogo.Text = Catalogo.cIdCatalogo
                txtDescripcionMantenimientoCatalogo.Text = Catalogo.vDescripcionCatalogo
                txtDescripcionAbreviadaMantenimientoCatalogo.Text = Catalogo.vDescripcionAbreviadaCatalogo
                txtVidaUtilMantenimientoCatalogo.Text = IIf(Catalogo.nVidaUtilCatalogo Is Nothing, "0", Catalogo.nVidaUtilCatalogo)
                txtPeriodoGarantiaMantenimientoCatalogo.Text = Catalogo.nPeriodoGarantiaCatalogo
                txtPeriodoMinimoMantenimientoCatalogo.Text = Catalogo.nPeriodoMinimoMantenimientoCatalogo
                txtCuentaContableMantenimientoCatalogo.Text = Catalogo.cIdCuentaContableCatalogo
                txtCuentaContableLeasingMantenimientoCatalogo.Text = Catalogo.cIdCuentaContableLeasingCatalogo
            End If
            CargarCestaCatalogoCaracteristica()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub LlenarDataCatalogoComponente()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosCatalogoComponente()
                cboTipoActivoCatalogoComponente.SelectedValue = cboTipoActivo.SelectedValue
                ListarSistemaFuncionalCatalogoComponenteCombo()
            Else
                LimpiarObjetosCatalogoComponente()
                txtIdCatalogoComponente.Text = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(0).Text).Trim
                cboDescripcionCatalogoComponente.SelectedValue = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text)
                txtDescripcionCatalogoComponente.Text = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text)
                txtDescripcionAbreviadaCatalogoComponente.Text = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(4).Text)
                cboTipoActivoCatalogoComponente.SelectedValue = grdCatalogoComponente.SelectedRow.Cells(2).Text
                ListarSistemaFuncionalCatalogoComponenteCombo()
                cboSistemaFuncionalCatalogoComponente.SelectedValue = grdCatalogoComponente.SelectedRow.Cells(3).Text
                txtCuentaContableCatalogoComponente.Text = ""
                txtCuentaContableLeasingCatalogoComponente.Text = ""
                txtVidaUtilCatalogoComponente.Text = Session("CestaCatalogoComponente").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
                txtPeriodoGarantiaCatalogoComponente.Text = Session("CestaCatalogoComponente").Rows(rowIndexDetalle)("PeriodoGarantia").ToString.Trim
                txtPeriodoMinimoCatalogoComponente.Text = Session("CestaCatalogoComponente").Rows(rowIndexDetalle)("PeriodoMinimo").ToString.Trim
            End If
            CargarCestaCaracteristicaCatalogoComponente()
        Catch ex As Exception
            MyValidator.ErrorMessage = ex.Message
        End Try
    End Sub

    Sub LlenarDataEquipoComponente()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosEquipoComponente()
                cboTipoActivoEquipoComponente.SelectedValue = cboTipoActivo.SelectedValue
                ListarSistemaFuncionalEquipoComponenteCombo()
            Else
                LimpiarObjetosEquipoComponente()
                Dim Equipo As New LOGI_EQUIPO
                If grdLista.Rows.Count > 0 Then
                    If hfdOperacion.Value = "E" Then 'JMUG: 16/03/2023
                        Equipo = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, grdLista.SelectedRow.Cells(5).Text)
                    End If
                End If

                hfdIdCatalogoEquipoComponente.Value = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim
                txtIdEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdEquipo").ToString.Trim
                hfdFechaTransaccionEquipoComponente.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
                txtDescripcionEquipoComponente.Text = Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(2).Text)
                txtDescripcionAbreviadaEquipoComponente.Text = Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(5).Text)
                cboTipoActivoEquipoComponente.SelectedValue = grdEquipoComponente.SelectedRow.Cells(3).Text 'grdEquipoComponente.SelectedRow.Cells(4).Text
                ListarSistemaFuncionalEquipoComponenteCombo()
                cboSistemaFuncionalEquipoComponente.SelectedValue = grdEquipoComponente.SelectedRow.Cells(4).Text
                txtCuentaContableEquipoComponente.Text = ""
                txtCuentaContableLeasingEquipoComponente.Text = ""
                txtVidaUtilEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
                txtPeriodoGarantiaEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("PeriodoGarantia").ToString.Trim
                txtPeriodoMinimoEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("PeriodoMinimo").ToString.Trim
                txtNroSerieEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("NroSerie").ToString.Trim
                txtNroParteEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("NroParte").ToString.Trim
                hfdFechaRegistroTarjetaSAPEquipoComponente.Value = Equipo.dFechaRegistroTarjetaSAPEquipo.ToString.Trim
                hfdFechaManufacturaTarjetaSAPEquipoComponente.Value = Equipo.dFechaManufacturaTarjetaSAPEquipo.ToString.Trim
                hfdFechaCreacionEquipoComponente.Value = Equipo.dFechaCreacionEquipo.ToString.Trim
                hfdIdUsuarioCreacionEquipoComponente.Value = IIf(IsNothing(Equipo.cIdUsuarioCreacionEquipo), "", Equipo.cIdUsuarioCreacionEquipo)
            End If
            CargarCestaCaracteristicaEquipoComponenteTemporal()
        Catch ex As Exception
            MyValidator.ErrorMessage = ex.Message
        End Try
    End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvDescripcionEquipo.EnableClientScript = bValidar
        Me.rfvCatalogo.EnableClientScript = bValidar
        Me.rfvSistemaFuncional.EnableClientScript = bValidar
        Me.rfvIdCliente.EnableClientScript = bValidar
        Me.rfvRazonSocial.EnableClientScript = bValidar
        Me.rfvDescripcionEquipoSAP.EnableClientScript = bValidar
    End Sub

    Sub ValidarTextoSubirImagenEquipo(ByVal bValidar As Boolean)
        Me.rfvTituloSubirImagenEquipo.EnableClientScript = bValidar
        Me.rfvDescripcionSubirImagenEquipo.EnableClientScript = bValidar
        Me.rfvObservacionSubirImagenEquipo.EnableClientScript = bValidar
    End Sub

    Sub ValidarTextoSubirDocumentacionEquipo(ByVal bValidar As Boolean)
        Me.rfvTituloSubirDocumentacionEquipo.EnableClientScript = bValidar
        Me.rfvDescripcionSubirDocumentacionEquipo.EnableClientScript = bValidar
        'Me.rfvNombreArchivoSubirDocumentacionEquipo.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        hfdFechaTransaccionEquipoComponente.Value = Now
        txtIdEquipo.Text = ""
        txtDescripcionEquipo.Text = ""
        txtNroSerieEquipo.Text = ""
        txtNroParteEquipo.Text = ""
        txtTagEquipo.Text = ""
        txtCapacidadEquipo.Text = ""
        txtAreaEquipo.Text = ""
        lnkEsComponenteOn.Visible = False
        lnkEsComponenteOff.Visible = True
        divSistemaFuncional.Visible = False
        lnkEsConContratoOn.Visible = False
        lnkEsConContratoOff.Visible = True
        divContratoReferencia.Visible = False
        cboContratoReferencia.SelectedIndex = -1
        cboClienteUbicacion.SelectedIndex = -1
        cboTipoActivo.SelectedIndex = -1
        cboTipoEquipo.SelectedIndex = -1
        txtDescripcionEquipoSAP.Text = ""
        cboCatalogo.SelectedIndex = -1
        cboSistemaFuncional.SelectedIndex = -1
        hfdEstado.Value = "1"

        hfdIdAuxiliar.Value = ""
        hfdIdTipoPersonaCliente.Value = ""
        hfdIdTipoCliente.Value = ""
        hfdIdUbicacionGeograficaCliente.Value = ""
        hfdNroDocumentoCliente.Value = ""
        hfdDireccionFiscalCliente.Value = ""
        hfdTelefonoContactoCliente.Value = ""
        txtIdCliente.Text = ""
        txtRazonSocial.Text = ""

        hfdCorreoElectronicoCliente.Value = ""
        hfdDNICliente.Value = ""
        hfdRUCCliente.Value = ""
        hfdIdClienteSAPEquipo.Value = ""
        hfdIdEquipoSAPEquipo.Value = ""
        hfdIdArticuloSAPEquipo.Value = ""
        cboArticuloSAPEquipo.SelectedIndex = -1
        hfdFechaRegistroTarjetaSAPEquipo.Value = ""
        hfdFechaManufacturaTarjetaSAPEquipo.Value = ""
        hfdFechaCreacionEquipo.Value = ""
        hfdIdUsuarioCreacionEquipo.Value = ""
    End Sub

    Sub LimpiarObjetosTipoActivo()
        txtIdTipoActivoMantenimientoTipoActivo.Text = ""
        txtDescripcionMantenimientoTipoActivo.Text = ""
        txtDescripcionAbreviadaMantenimientoTipoActivo.Text = ""
    End Sub

    Sub LimpiarObjetosSistemaFuncional()
        txtIdSistemaFuncionalMantenimientoSistemaFuncional.Text = ""
        txtDescripcionMantenimientoSistemaFuncional.Text = ""
        txtDescripcionAbreviadaMantenimientoSistemaFuncional.Text = ""
    End Sub

    Sub LimpiarObjetosCatalogo()
        txtIdCatalogoMantenimientoCatalogo.Text = ""
        txtDescripcionMantenimientoCatalogo.Text = ""
        txtDescripcionAbreviadaMantenimientoCatalogo.Text = ""
        txtVidaUtilMantenimientoCatalogo.Text = ""
        txtPeriodoGarantiaMantenimientoCatalogo.Text = ""
        txtPeriodoMinimoMantenimientoCatalogo.Text = ""
        txtCuentaContableMantenimientoCatalogo.Text = ""
        txtCuentaContableLeasingMantenimientoCatalogo.Text = ""
        clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
        grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
        grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
    End Sub

    Sub LimpiarObjetosCatalogoComponente()
        MyValidator.ErrorMessage = ""
        txtIdCatalogoComponente.Text = ""
        cboTipoActivoCatalogoComponente.SelectedIndex = -1
        cboSistemaFuncionalCatalogoComponente.SelectedIndex = -1
        txtDescripcionCatalogoComponente.Text = ""
        txtDescripcionAbreviadaCatalogoComponente.Text = ""
        txtVidaUtilCatalogoComponente.Text = ""
        txtCuentaContableCatalogoComponente.Text = ""
        txtCuentaContableLeasingCatalogoComponente.Text = ""
    End Sub

    Sub LimpiarObjetosEquipoComponente()
        MyValidator.ErrorMessage = ""
        txtIdEquipoComponente.Text = ""
        cboTipoActivoEquipoComponente.SelectedIndex = -1
        cboSistemaFuncionalEquipoComponente.SelectedIndex = -1
        txtDescripcionEquipoComponente.Text = ""
        txtDescripcionAbreviadaEquipoComponente.Text = ""
        txtVidaUtilEquipoComponente.Text = ""
        txtCuentaContableEquipoComponente.Text = ""
        txtCuentaContableLeasingEquipoComponente.Text = ""
    End Sub


    Protected Sub Upload_File(sender As Object, e As EventArgs)
        If hfdOperacion.Value = "N" Then
            'GenerarComprobante()
        End If
        btnNuevo.Enabled = True
    End Sub

    Sub LimpiarObjetosCaracteristicas()
        Me.lblMensajeCaracteristica.Text = ""
        txtValorCaracteristica.Text = ""
        txtIdReferenciaSAPCaracteristica.Text = ""
        txtDescripcionCampoSAPCaracteristica.Text = ""
        divIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
        txtIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
        divDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        txtDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "127" 'Mantenimiento de los Maestros Activos / Componente.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroEquipo.SelectedIndex = 4
            ListarTipoActivoCombo()
            ListarTipoEquipoCombo()
            ListarCatalogoCombo()
            ListarSistemaFuncionalCombo()
            ListarArticuloSAPCombo() 'JMUG: 01/02/2024
            ListarDescripcionCatalogoComponente()

            'If hfdOperacion.Value = "E" Then
            '    
            'Else
            '    cboListadoCheckListMantenimientoOrdenTrabajo.Enabled = False
            'End If


            ListarCaracteristicaEquipoPrincipalCombo()
            ListarCaracteristicaCatalogoComponenteCombo()
            ListarCaracteristicaEquipoComponenteCombo()
            ListarContratoReferenciaCombo()
            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()

            If Session("CestaCatalogoCaracteristica") Is Nothing Then
                Session("CestaCatalogoCaracteristica") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            End If

            If Session("CestaCaracteristicaCatalogoComponente") Is Nothing Then
                Session("CestaCaracteristicaCatalogoComponente") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            End If

            If Session("CestaCaracteristicaEquipoPrincipal") Is Nothing Then
                Session("CestaCaracteristicaEquipoPrincipal") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoPrincipal"))
            End If

            If Session("CestaCatalogoComponente") Is Nothing Then
                Session("CestaCatalogoComponente") = clsLogiCestaCatalogo.CrearCesta()
            Else
                clsLogiCestaEquipo.VaciarCesta(Session("CestaCatalogoComponente"))
            End If

            If Session("CestaCaracteristicaCatalogoComponente") Is Nothing Then
                Session("CestaCaracteristicaCatalogoComponente") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            End If

            If Session("CestaEquipoComponente") Is Nothing Then
                Session("CestaEquipoComponente") = clsLogiCestaEquipo.CrearCesta()
            Else
                clsLogiCestaEquipo.VaciarCesta(Session("CestaEquipoComponente"))
            End If

            If Session("CestaCaracteristicaEquipoComponente") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponente") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponente"))
            End If

            BloquearMantenimiento(True, False, True, False)

            EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")

            If (String.IsNullOrEmpty(Session("IdContratoUsuario"))) Then
                btnNuevo.Visible = False
                btnEditar.Visible = False
                'lnkbtnVerContrato.Visible = False
                'lnkbtnVerProgramacion.Visible = False
            ElseIf (Session("IdContratoUsuario") = "*") Then
                Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0") 'Session("IdContratoUsuario")
                Me.grdLista.DataBind()
            ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
                Me.grdLista.DataBind()
            End If

            'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0") 'Session("IdContratoUsuario")
            'Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlEquipo.Visible = False
            pnlComponentes.Visible = False
        Else
            txtBuscarEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            txtIdEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_cboFamilia')")
            cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcion')")
            txtDescripcionEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
        End If
    End Sub

    Protected Sub cboTipoActivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivo.SelectedIndexChanged
        cboTipoActivoMantenimientoCatalogo.SelectedValue = cboTipoActivo.SelectedValue
        cboTipoActivoCatalogoComponente.SelectedValue = cboTipoActivo.SelectedValue
        ListarCatalogoCombo()
        CargarCestaCatalogoComponente()

        CargarCestaEquipoComponente()
        If (hfdOperacion.Value = "N") Then
            CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 21/11/2023
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0651", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True

            hfdOperacion.Value = "N"
            txtDescripcionEquipo.Focus()
            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            CargarCestaCatalogoComponente()
            CargarCestaEquipoComponente()
            CargarCestaCaracteristicaEquipoPrincipal()
            CargarCestaCaracteristicaEquipoComponente()
            ValidarTexto(True)
            ActivarObjetos(True)

            ListarContratoReferenciaCombo(String.Empty, True)


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

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0651", strOpcionModulo, "CMMS")
            EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdEquipo = '" & txtIdEquipo.Text & "'")

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

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0652", strOpcionModulo, "CMMS")

            If IsNothing(grdLista.SelectedRow) = False Then
                If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                    hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                End If
                If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                    Throw New Exception("Este registro se encuentra desactivado.")
                End If

                Dim dtEstado = EquipoNeg.EquipoGetData("SELECT ISNULL(cEstadoEquipo, 'R') AS cEstadoEquipo FROM LOGI_EQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                If dtEstado.Rows(0).Item(0) = "R" Then 'REGISTRADO
                    EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                ElseIf dtEstado.Rows(0).Item(0) = "T" Then 'TERMINADO
                    EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                ElseIf dtEstado.Rows(0).Item(0) = "B" Then 'BLOQUEADO
                    EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "' and cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")
                    'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                    'Me.grdLista.DataBind()

                    If (Session("IdContratoUsuario") = "*") Then
                        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                        Me.grdLista.DataBind()
                    ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
                        Me.grdLista.DataBind()
                    End If

                    Dim dtUsuario = EquipoNeg.EquipoGetData("SELECT LTRIM(RTRIM(vApellidoPaternoUsuario)) + ' ' + LTRIM(RTRIM(vApellidoMaternoUsuario)) + ', ' + LTRIM(RTRIM(vNombresUsuario)) AS vNombreCompletoUsuario " &
                                                            "FROM GNRL_USUARIO " &
                                                            "WHERE cIdUsuario IN (SELECT cIdUsuarioUltimaModificacionEquipo FROM LOGI_EQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "')")
                    Throw New Exception("Este registro se encuentra bloqueado y utilizado por el usuario " & dtUsuario.Rows(0).Item(0) & ".")
                End If
                hfdOperacion.Value = "E"
                BloquearMantenimiento(False, True, False, True)
                ValidarTexto(True)
                ActivarObjetos(True)
                LlenarData()
                pnlCabecera.Visible = False
                pnlEquipo.Visible = True
                pnlComponentes.Visible = True
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
            lblMensajeCaracteristica.Text = ""
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            lnkbtnImprimirTarjetaEquipo.Attributes.Add("onclick", "javascript:popupEmitirEquipoDetalleReporte('" & txtIdEquipo.Text & "');")
                            lnkbtnVerOrdenFabricacion.Attributes.Add("onclick", "javascript:popupEmitirOrdenFabricacionReporte('" & txtIdEquipo.Text & "');")
                            'lnkbtnVerOrdenTrabajo.Attributes.Add("onclick", "javascript:popupEmitirOrdenTrabajoReporte('" & txtIdEquipo.Text & "');") 'aqui
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

    Private Sub lnkbtnVerOrdenTrabajoModal_Click(sender As Object, e As EventArgs) Handles lnkbtnVerOrdenTrabajoModal.Click
        Try
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerbtnAceptarSeleccionarEquipofil"), "0668", strOpcionModulo, "CMMS")
            lnk_mostrarPanelOrdenTrabajoModal_ModalPopupExtender.Show()
            BindGridView()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub grdEquiposOrdenTrabajo_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "SelectOrdenTrabajo" Then
            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvData.Rows(rowIndex)
            ' Simulación de valores (ajusta con los valores reales de tu GridView)
            Dim cadena As String = row.Cells(3).Text
            Dim partes() As String = cadena.Split("-"c) ' Separar por el carácter '-'

            Dim ot As String = partes(0)
            Dim nroSerie As String = partes(1)
            Dim nroDoc As String = partes(2)

            ' Construir la URL correctamente
            Dim url As String = $"Informes/frmCmmsOrdenTrabajoReporte.aspx?TipDoc={ot}&NroSerie={nroSerie}&NroDoc={nroDoc}"

            ' Crear el script con la URL correcta
            Dim script As String = $"window.open('{url}', '', 'toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes');"

            ' Registrar y ejecutar el script en el cliente
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopupScript2", script, True)
        End If
    End Sub

    Protected Sub lnkbtnImprimirOrdenTrabajo_Click(sender As Object, e As EventArgs) 'Handles lnkbtnImprimirOrdenTrabajo.Click
        Try

            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeCaracteristica.Text = ""
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            Dim script As String = $"window.open('Informes/frmCmmsEquipoOrdenTrabajoReporte.aspx?IdEqu={txtIdEquipo.Text}', '', 'toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes');"
                            ' Registra el script para ejecutarlo en el cliente
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopupScript", script, True)

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

    Private Sub BindGridView()
        Dim query As String = "
SELECT 
    ORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo,
    ORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo,
    ORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
    CONCAT(
        ORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo, 
        '-', 
        ORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo, 
        '-', 
        ORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo
    ) AS Codigo,
    ORDTRA.dFechaEmisionCabeceraOrdenTrabajo,
    ORDTRA.dFechaInicioPlanificadaCabeceraOrdenTrabajo,
    TIPMAN.vDescripcionTipoMantenimiento,
    ORDTRA.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo,
    ORDTRA.vContratoReferenciaCabeceraOrdenTrabajo,  
    CASE 
        WHEN ORDTRA.cEstadoCabeceraOrdenTrabajo = 'R' THEN 'REGISTRADO' 
        WHEN ORDTRA.cEstadoCabeceraOrdenTrabajo = 'P' THEN 'EN PROCESO' 
        ELSE 'TERMINADO' 
    END AS vEstadoCabeceraOrdenTrabajo,
    ORDTRA.cIdTipoMantenimientoCabeceraOrdenTrabajo   
FROM 
    LOGI_CABECERAORDENTRABAJO AS ORDTRA
INNER JOIN 
    LOGI_TIPOMANTENIMIENTO AS TIPMAN 
    ON ORDTRA.cIdTipoMantenimientoCabeceraOrdenTrabajo = TIPMAN.cIdTipoMantenimiento
WHERE 
    ORDTRA.cIdEquipoCabeceraOrdenTrabajo = '" & txtIdEquipo.Text & "' 
    AND ORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "'"
        Dim datatable = EquipoNeg.EquipoGetData(query)

        gvData.DataSource = datatable
        gvData.DataBind()
    End Sub

    Private Sub btnAceptarMantenimientoTipoActivo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoTipoActivo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If txtDescripcionMantenimientoTipoActivo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripcion del tipo de activo.")
            ElseIf txtDescripcionAbreviadaMantenimientoTipoActivo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripcion abreviada del tipo de activo.")
            End If
            Dim TipoActivoNeg As New clsTipoActivoNegocios
            Dim TipoActivo As New LOGI_TIPOACTIVO
            TipoActivo.cIdTipoActivo = txtIdTipoActivoMantenimientoTipoActivo.Text
            TipoActivo.vDescripcionTipoActivo = UCase(IIf(txtDescripcionMantenimientoTipoActivo.Text = "", "", txtDescripcionMantenimientoTipoActivo.Text))
            TipoActivo.vDescripcionAbreviadaTipoActivo = UCase(IIf(txtDescripcionAbreviadaMantenimientoTipoActivo.Text = "", "", txtDescripcionAbreviadaMantenimientoTipoActivo.Text))
            TipoActivo.bEstadoRegistroTipoActivo = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(IIf(hfdEstado.Value = "", "1", hfdEstado.Value))))

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
                If (TipoActivoNeg.TipoActivoInserta(TipoActivo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_TIPOACTIVO 'SQL_INSERT', '','" & TipoActivo.cIdTipoActivo & "', '" &
                                       Session("IdEmpresa") & "', '" & Session("IdPuntoVenta") & "', '" & TipoActivo.vDescripcionTipoActivo & "', '" & TipoActivo.vDescripcionAbreviadaTipoActivo & "', '" & TipoActivo.bEstadoRegistroTipoActivo & "', '" & TipoActivo.cIdTipoActivo & "'"

                    LogAuditoria.vEvento = "INSERTAR TIPO DE ACTIVO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdTipoActivoMantenimientoTipoActivo.Text = TipoActivo.cIdTipoActivo
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    ListarTipoActivoCombo()
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (TipoActivoNeg.TipoActivoEdita(TipoActivo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_TIPOACTIVO 'SQL_UPDATE', '','" & TipoActivo.cIdTipoActivo & "', '" &
                                       Session("IdEmpresa") & "', '" & Session("IdPuntoVenta") & "', '" & TipoActivo.vDescripcionTipoActivo & "', '" & TipoActivo.vDescripcionAbreviadaTipoActivo & "', '" & TipoActivo.bEstadoRegistroTipoActivo & "', '" & TipoActivo.cIdTipoActivo & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR TIPO DE ACTIVO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    ListarTipoActivoCombo()
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacion.Value = "R"
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lblMensajeMantenimientoTipoActivo.Text = ex.Message
            lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaMantenimientoCatalogo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaMantenimientoCatalogo.Click
        Try
            fValidarSesion()

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
                                grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
                                grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
                                LimpiarObjetosCaracteristicas()
                                lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                                grdListaCaracteristica.SelectedIndex = -1
                                lblMensajeCaracteristica.Text = "Característica ya registrada, seleccione otro item."
                                Exit Sub
                            End If
                        Next

                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoMantenimientoCatalogo.Text, "", "0", grdListaCaracteristica.SelectedValue.trim, Server.HtmlDecode(grdListaCaracteristica.SelectedRow.Cells(1).Text).Trim, UCase(txtIdReferenciaSAPCaracteristica.Text.Trim), UCase(txtDescripcionCampoSAPCaracteristica.Text.Trim), UCase(txtValorCaracteristica.Text.Trim), Session("CestaCatalogoCaracteristica"))
                        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
                        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()

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

    Private Sub btnAceptarMantenimientoCatalogo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoCatalogo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If cboTipoActivo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de activo.")
            End If

            Dim Catalogo As New LOGI_CATALOGO
            Catalogo.cIdCatalogo = UCase(txtIdCatalogoMantenimientoCatalogo.Text)
            Catalogo.cIdTipoActivo = cboTipoActivoMantenimientoCatalogo.SelectedValue
            Catalogo.cIdJerarquiaCatalogo = "0"
            Catalogo.cIdEnlaceCatalogo = "" 'UCase(txtIdCatalogo.Text)
            Catalogo.vDescripcionCatalogo = UCase(txtDescripcionMantenimientoCatalogo.Text)
            Catalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviadaMantenimientoCatalogo.Text)
            Catalogo.bEstadoRegistroCatalogo = IIf(hfdOperacionDetalle.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
            Catalogo.cIdSistemaFuncional = Nothing '""
            Catalogo.nVidaUtilCatalogo = Convert.ToDouble(IIf(txtVidaUtilMantenimientoCatalogo.Text.Trim = "", "0", txtVidaUtilMantenimientoCatalogo.Text))
            Catalogo.cIdCuentaContableCatalogo = txtCuentaContableMantenimientoCatalogo.Text
            Catalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasingMantenimientoCatalogo.Text
            Catalogo.nPeriodoGarantiaCatalogo = txtPeriodoGarantiaMantenimientoCatalogo.Text
            Catalogo.nPeriodoMinimoMantenimientoCatalogo = txtPeriodoMinimoMantenimientoCatalogo.Text

            Dim ColeccionCatCar As New List(Of LOGI_CATALOGOCARACTERISTICA)
            Dim i As Integer
            For i = 0 To grdDetalleCaracteristicaMantenimientoCatalogo.Rows.Count - 1
                Dim DetDocumento As New LOGI_CATALOGOCARACTERISTICA

                DetDocumento.cIdCatalogo = txtIdCatalogoMantenimientoCatalogo.Text
                DetDocumento.cIdJerarquiaCatalogo = "0"
                DetDocumento.cIdCaracteristica = Server.HtmlDecode(Replace(grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i).Cells(3).Text.ToString, "&nbsp;", ""))
                DetDocumento.nIdNumeroItemCatalogoCaracteristica = grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i).Cells(2).Text
                Dim row = grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i)
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

            If hfdOperacionDetalle.Value = "N" Then
                If (CatalogoNeg.CatalogoInserta(Catalogo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" &
                                                   Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                                   Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                                   Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
                                                   Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" & Catalogo.cIdCatalogo & "'"

                    LogAuditoria.vEvento = "INSERTAR CATALOGO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdCatalogoMantenimientoCatalogo.Text = Catalogo.cIdCatalogo

                    If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
                    End If

                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    BloquearMantenimiento(False, True, False, True)
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (CatalogoNeg.CatalogoEdita(Catalogo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" &
                                               Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                               Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                               Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
                                               Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" & Catalogo.cIdCatalogo & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR CATALOGO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
                    End If
                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    BloquearMantenimiento(False, True, False, True)
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacionDetalle.Value = "R"
            Dim strIdCatalogo As String
            strIdCatalogo = cboCatalogo.SelectedValue
            ListarCatalogoCombo()
            cboCatalogo.SelectedValue = strIdCatalogo
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

    Private Sub btnCancelarCaracteristica_Click(sender As Object, e As EventArgs) Handles btnCancelarCaracteristica.Click
        lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnNuevoTipoActivo_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoTipoActivo.Click
        hfdOperacionDetalle.Value = "N"
        LlenarDataTipoActivo()
        lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarTipoActivo_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarTipoActivo.Click
        hfdOperacionDetalle.Value = "E"
        LlenarDataTipoActivo()
        If MyValidator.ErrorMessage = "" Then
            lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender.Show()
        End If
    End Sub

    Private Sub lnkbtnNuevoCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoCatalogo.Click
        hfdOperacionDetalle.Value = "N"
        LlenarDataCatalogo()
        lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarCatalogo.Click
        hfdOperacionDetalle.Value = "E"
        LlenarDataCatalogo()
        If MyValidator.ErrorMessage = "" Then
            lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
        End If
    End Sub

    Private Sub cboCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCatalogo.SelectedIndexChanged
        If (hfdOperacion.Value = "N") Then
            CargarCestaCatalogoComponente()
            CargarCestaEquipoComponente()
            CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 21/11/2023
        ElseIf (hfdOperacion.Value = "E") Then 'JMUG: 06/12/2023: INICIO
            hfdTipoOperacionMensajeDocumentoValidacion.Value = "2" 'Cambiar Catalogo Principal

            lblDescripcionMensajeDocumentoValidacion.Text = "¿Seguro desea cambiar de catalogo?, de aceptar este cambio todos los componentes y caracteristicas de los componentes asignados se eliminaran de manera permanente."
            lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender.Show()
        End If 'JMUG: 06/12/2023: FINAL
    End Sub

    Private Sub lnkbtnNuevoCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0672", strOpcionModulo, "CMMS")
            If cboCatalogo.SelectedValue = "SELECCIONE DATO" Then
                Throw New Exception("Debe de ingresar un catalogo para asignar sus componentes.")
            End If
            hfdOperacionDetalle.Value = "N"
            LlenarDataCatalogoComponente()
            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub btnAceptarCatalogoComponente_Click(sender As Object, e As EventArgs) Handles btnAceptarCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If cboTipoActivoCatalogoComponente.SelectedIndex <= 0 Then
                Throw New Exception("Debe de ingresar el código del tipo de activo correcto.")
            ElseIf cboSistemaFuncionalCatalogoComponente.SelectedIndex < 0 Then
                Throw New Exception("Debe de ingresar el código del sistema funcional correcto.")
            ElseIf txtDescripcionCatalogoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción correcta.")
            ElseIf txtDescripcionAbreviadaCatalogoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción abreviada correcta.")
            Else
                Dim Coleccion As New List(Of LOGI_CATALOGO)
                For i = 0 To 0
                    Dim DetCatalogo As New LOGI_CATALOGO
                    DetCatalogo.cIdCatalogo = ""  'Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
                    DetCatalogo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                    DetCatalogo.cIdJerarquiaCatalogo = "1" 'Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
                    DetCatalogo.cIdSistemaFuncional = cboSistemaFuncionalCatalogoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString.Trim
                    DetCatalogo.cIdTipoActivo = cboTipoActivoCatalogoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString.Trim
                    DetCatalogo.vDescripcionCatalogo = UCase(txtDescripcionCatalogoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion").ToString.Trim
                    DetCatalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString.Trim
                    DetCatalogo.bEstadoRegistroCatalogo = True 'Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
                    DetCatalogo.nVidaUtilCatalogo = Convert.ToInt32(IIf(txtVidaUtilCatalogoComponente.Text.Trim = "", "0", txtVidaUtilCatalogoComponente.Text)) 'Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
                    DetCatalogo.nPeriodoGarantiaCatalogo = Convert.ToInt32(IIf(txtPeriodoGarantiaMantenimientoCatalogo.Text.Trim = "", "0", txtPeriodoGarantiaMantenimientoCatalogo.Text))
                    DetCatalogo.nPeriodoMinimoMantenimientoCatalogo = Convert.ToInt32(IIf(txtPeriodoMinimoCatalogoComponente.Text.Trim = "", "0", txtPeriodoMinimoCatalogoComponente.Text))
                    DetCatalogo.cIdCuentaContableCatalogo = txtCuentaContableCatalogoComponente.Text 'Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
                    DetCatalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasingCatalogoComponente.Text  'Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString.Trim
                    Coleccion.Add(DetCatalogo)
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

                If hfdOperacion.Value = "E" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                        For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                            Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                            DetCaracteristica.cIdCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                            DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            ColeccionCaracteristica.Add(DetCaracteristica)
                        Next
                        If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                            clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                                         txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                                         "1", txtVidaUtilCatalogoComponente.Text.Trim,
                                                         txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                                         cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                        End If
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                            Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                                Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                DetCaracteristica.cIdCatalogo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                                DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                                DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                            If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                                clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                     "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                     txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                     "1", IIf(txtVidaUtilCatalogoComponente.Text.Trim = "", "0", txtVidaUtilCatalogoComponente.Text.Trim),
                                     txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                     cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"))
                            End If
                        End If
                    End If
                ElseIf hfdOperacion.Value = "N" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                        For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                            Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                            DetCaracteristica.cIdCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                            DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            ColeccionCaracteristica.Add(DetCaracteristica)
                        Next
                        If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                            clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                                         txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                                         "1", txtVidaUtilCatalogoComponente.Text.Trim,
                                                         txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                                         cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                        End If
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                            Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                                Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                DetCaracteristica.cIdCatalogo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                                DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                                DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                            If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                                clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                     "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                     txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                     "1", IIf(txtVidaUtilCatalogoComponente.Text.Trim = "", "0", txtVidaUtilCatalogoComponente.Text.Trim),
                                     txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                     cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"))
                            End If
                        End If
                    End If
                End If
            End If
            grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            grdCatalogoComponente.DataBind()

            Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Hide()
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
            Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        End Try
    End Sub

    Protected Sub btnAceptarEquipoComponente_Click(sender As Object, e As EventArgs) Handles btnAceptarEquipoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If cboTipoActivoEquipoComponente.SelectedIndex <= 0 Then
                Throw New Exception("Debe de ingresar el código del tipo de activo correcto.")
            ElseIf cboSistemaFuncionalEquipoComponente.SelectedIndex < 0 Then
                Throw New Exception("Debe de ingresar el código del sistema funcional correcto.")
            ElseIf txtDescripcionEquipoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción correcta.")
            ElseIf txtDescripcionAbreviadaEquipoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción abreviada correcta.")
            Else
                Dim Coleccion As New List(Of LOGI_EQUIPO)
                For i = 0 To 0
                    Dim DetEquipo As New LOGI_EQUIPO
                    DetEquipo.cIdCatalogo = Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text) '""  'Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
                    DetEquipo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                    DetEquipo.cIdEnlaceEquipo = txtIdEquipo.Text
                    DetEquipo.cIdEquipo = txtIdEquipoComponente.Text
                    DetEquipo.cIdJerarquiaCatalogo = "1" 'Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
                    DetEquipo.cIdSistemaFuncionalEquipo = cboSistemaFuncionalEquipoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString.Trim
                    DetEquipo.cIdTipoActivo = cboTipoActivoEquipoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString.Trim
                    DetEquipo.vIdEquivalenciaEquipo = ""
                    DetEquipo.vDescripcionEquipo = UCase(txtDescripcionEquipoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion").ToString.Trim
                    DetEquipo.vDescripcionAbreviadaEquipo = UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString.Trim
                    DetEquipo.bEstadoRegistroEquipo = True 'Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
                    DetEquipo.nVidaUtilEquipo = Convert.ToInt32(IIf(txtVidaUtilEquipoComponente.Text.Trim = "", "0", txtVidaUtilEquipoComponente.Text)) 'Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
                    DetEquipo.nPeriodoGarantiaEquipo = Convert.ToInt32(IIf(txtPeriodoGarantiaEquipoComponente.Text.Trim = "", "0", txtPeriodoGarantiaEquipoComponente.Text))
                    DetEquipo.nPeriodoMinimoMantenimientoEquipo = Convert.ToInt32(IIf(txtPeriodoMinimoEquipoComponente.Text.Trim = "", "0", txtPeriodoMinimoEquipoComponente.Text))
                    DetEquipo.vNumeroSerieEquipo = UCase(txtNroSerieEquipoComponente.Text.Trim)
                    DetEquipo.vNumeroParteEquipo = UCase(txtNroParteEquipoComponente.Text.Trim) 'DetEquipo.cIdCuentaContableEquipo = txtCuentaContableCatalogoComponente.Text 'Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
                    DetEquipo.cIdCliente = hfdIdAuxiliar.Value
                    DetEquipo.cIdEmpresa = Session("IdEmpresa")
                    DetEquipo.cIdEstadoComponenteEquipo = "01" '""
                    DetEquipo.cIdPaisOrigenEquipo = Session("IdPais")
                    DetEquipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
                    DetEquipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
                    DetEquipo.dFechaTransaccionEquipo = hfdFechaTransaccionEquipoComponente.Value
                    DetEquipo.dFechaRegistroTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaRegistroTarjetaSAPEquipoComponente.Value), hfdFechaRegistroTarjetaSAPEquipoComponente.Value, Now))
                    DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaManufacturaTarjetaSAPEquipoComponente.Value), hfdFechaManufacturaTarjetaSAPEquipoComponente.Value, Now))
                    Coleccion.Add(DetEquipo)
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

                If hfdOperacion.Value = "E" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        If Coleccion(0).cIdEquipo = "" Then
                            clsLogiCestaEquipo.EditarCesta(Coleccion(0).cIdEquipo, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)
                            MyValidator.ErrorMessage = "Componente editado con éxito."
                        Else
                            clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text.Trim, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)

                            Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                                For j = 0 To Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows.Count - 1
                                    If (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim) = Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdCatalogo").ToString.Trim And
                                   (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim) = Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdJerarquia").ToString.Trim And
                                   (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdCaracteristica").ToString.Trim Then
                                        clsLogiCestaCatalogoCaracteristica.EditarCesta(Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim, txtIdEquipoComponente.Text.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("Descripcion").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdReferenciaSAP").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("DescripcionCampoSAP").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("Valor").ToString.Trim, Session("CestaCaracteristicaEquipoComponente"), i)
                                    End If
                                Next
                            Next

                            MyValidator.ErrorMessage = "Componente editado con éxito."
                        End If
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        If EquipoNeg.EquipoInsertaDetalle(Coleccion, txtIdEquipo.Text, cboCatalogo.SelectedValue) = 0 Then
                            Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                                Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                                DetCaracteristica.cIdEquipo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                                DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                                DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                            If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(txtIdEquipo.Text, ColeccionCaracteristica, LogAuditoria) Then
                                clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoEquipoComponente.SelectedIndex > 0, cboTipoActivoEquipoComponente.SelectedValue, " "),
                                         "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                         txtIdEquipo.Text.Trim, UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                         "1", IIf(txtVidaUtilEquipoComponente.Text.Trim = "", "0", txtVidaUtilEquipoComponente.Text.Trim),
                                         txtCuentaContableEquipoComponente.Text.Trim, txtCuentaContableLeasingEquipoComponente.Text.Trim, cboTipoActivoEquipoComponente.SelectedItem.Text,
                                         cboSistemaFuncionalEquipoComponente.SelectedItem.Text, txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim, Session("CestaEquipoComponente"))
                            End If
                            MyValidator.ErrorMessage = "Componente agregado con éxito."
                        End If
                    End If
                ElseIf hfdOperacion.Value = "N" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text.Trim, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)
                        MyValidator.ErrorMessage = "Componente editado con éxito."
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)

                        Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                        For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                            Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                            DetCaracteristica.cIdEquipo = Coleccion(0).cIdEquipo 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                            DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                            DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                            DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                            DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            ColeccionCaracteristica.Add(DetCaracteristica)
                        Next
                    End If
                End If
            End If
            grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            grdEquipoComponente.DataBind()

            Me.lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Hide()
            ValidationSummary4.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarEquipoComponente"
            Me.Page.Validators.Add(MyValidator)
            Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub cboTipoActivoCatalogoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivoCatalogoComponente.SelectedIndexChanged
        ListarSistemaFuncionalCatalogoComponenteCombo()
        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarSistemaFuncionalCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarSistemaFuncionalCatalogoComponente.Click
        hfdOperacionDetalleAdicional.Value = "E"
        lnk_mostrarPanelMantenimientoSistemaFuncional_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnNuevoSistemaFuncionalCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoSistemaFuncionalCatalogoComponente.Click
        hfdOperacionDetalleAdicional.Value = "N"
        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        lnk_mostrarPanelMantenimientoSistemaFuncional_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAceptarMantenimientoSistemaFuncional_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoSistemaFuncional.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
            Dim SistemaFuncional As New LOGI_SISTEMAFUNCIONAL
            SistemaFuncional.cIdSistemaFuncional = UCase(txtIdSistemaFuncionalMantenimientoSistemaFuncional.Text)
            SistemaFuncional.vDescripcionSistemaFuncional = UCase(txtDescripcionMantenimientoSistemaFuncional.Text)
            SistemaFuncional.vDescripcionAbreviadaSistemaFuncional = UCase(txtDescripcionAbreviadaMantenimientoSistemaFuncional.Text)
            SistemaFuncional.bEstadoRegistroSistemaFuncional = 1 'IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacionDetalleAdicional.Value = "N" Then
                If (SistemaFuncionalNeg.SistemaFuncionalInserta(SistemaFuncional)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_SISTEMAFUNCIONAL 'SQL_INSERT', '','" & SistemaFuncional.cIdSistemaFuncional & "', '" &
                                       SistemaFuncional.vDescripcionSistemaFuncional & "', '" & SistemaFuncional.vDescripcionAbreviadaSistemaFuncional & "', '" &
                                       SistemaFuncional.bEstadoRegistroSistemaFuncional & "', '" & SistemaFuncional.cIdSistemaFuncional & "'"
                    LogAuditoria.vEvento = "INSERTAR SISTEMA FUNCIONAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdSistemaFuncionalMantenimientoSistemaFuncional.Text = SistemaFuncional.cIdSistemaFuncional
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalleAdicional.Value = "E" Then
                If (SistemaFuncionalNeg.SistemaFuncionalEdita(SistemaFuncional)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_SISTEMAFUNCIONAL 'SQL_UPDATE', '','" & SistemaFuncional.cIdSistemaFuncional & "', '" &
                                       SistemaFuncional.vDescripcionSistemaFuncional & "', '" & SistemaFuncional.vDescripcionAbreviadaSistemaFuncional & "', '" &
                                       SistemaFuncional.bEstadoRegistroSistemaFuncional & "', '" & SistemaFuncional.cIdSistemaFuncional & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR SISTEMA FUNCIONAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacionDetalleAdicional.Value = "R"
            ListarSistemaFuncionalCombo()
            ListarSistemaFuncionalCatalogoComponenteCombo()
            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
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

    Private Sub lnkbtnEditarCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0673", strOpcionModulo, "CMMS")

            hfdOperacionDetalle.Value = "E"
            If grdCatalogoComponente.Rows.Count > 0 Then
                If grdCatalogoComponente.SelectedIndex < grdCatalogoComponente.Rows.Count Then
                    If IsNothing(grdCatalogoComponente.SelectedRow) = False Then
                        If IsReference(grdCatalogoComponente.SelectedRow.Cells(0).Text) = True Then
                            Dim result As DataRow() = Session("CestaCatalogoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(0).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim & "'")
                            rowIndexDetalle = Session("CestaCatalogoComponente").Rows.IndexOf(result(0))
                            BloquearMantenimiento(False, True, False, True)
                            ValidarTexto(True)
                            ActivarObjetos(True)
                            LlenarDataCatalogoComponente()
                            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar algún item.")
                    End If
                End If
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

    Private Sub btnAdicionarCaracteristicaCatalogoComponente_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            If hfdOperacionDetalle.Value = "N" Or hfdOperacionDetalle.Value = "E" Then
                For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                    If (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaCatalogoComponente.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogoComponente.Text.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()

                        Throw New Exception("Característica ya registrada, seleccione otro item.")
                        Exit Sub
                    End If
                Next
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "", "1", cboCaracteristicaCatalogoComponente.SelectedValue.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogoComponente"))

                Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponente")
                Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()
                cboCaracteristicaCatalogoComponente.SelectedIndex = -1
                lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
                grdDetalleCaracteristicaCatalogoComponente.SelectedIndex = -1
                MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
            Else
                lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            End If
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaEquipoComponente_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaEquipoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0658", strOpcionModulo, "CMMS")

            If hfdOperacionDetalle.Value = "N" Or hfdOperacionDetalle.Value = "E" Then
                For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                    If (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaEquipoComponente.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim = txtIdEquipoComponente.Text.Trim And Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim <> "" Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                        Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                        Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
                        lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()

                        Throw New Exception("Característica ya registrada, seleccione otro item.")
                        Exit Sub
                    End If
                Next
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(hfdIdCatalogoEquipoComponente.Value, txtIdEquipoComponente.Text, "1", cboCaracteristicaEquipoComponente.SelectedValue.Trim, UCase(cboCaracteristicaEquipoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoComponente"))
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(hfdIdCatalogoEquipoComponente.Value, txtIdEquipoComponente.Text, "1", cboCaracteristicaEquipoComponente.SelectedValue.Trim, UCase(cboCaracteristicaEquipoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
                cboCaracteristicaEquipoComponente.SelectedIndex = -1
                lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
                grdDetalleCaracteristicaEquipoComponente.SelectedIndex = -1
                MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
            Else
                lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
            End If
            ValidationSummary2.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarEquipoComponente"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarEquipoComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub ListarDistritoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDistritoMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboDistritoMensajeCliente.DataValueField = "cIdDistritoUbicacionGeografica"
        cboDistritoMensajeCliente.DataSource = UbicacionGeograficaNeg.DistritoListarCombo(cboPaisMensajeCliente.SelectedValue, cboDepartamentoMensajeCliente.SelectedValue, cboProvinciaMensajeCliente.SelectedValue)
        cboDistritoMensajeCliente.Items.Clear()
        cboDistritoMensajeCliente.DataBind()
    End Sub

    Sub ListarProvinciaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboProvinciaMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboProvinciaMensajeCliente.DataValueField = "cIdProvinciaUbicacionGeografica"
        cboProvinciaMensajeCliente.DataSource = UbicacionGeograficaNeg.ProvinciaListarCombo(cboPaisMensajeCliente.SelectedValue, cboDepartamentoMensajeCliente.SelectedValue)
        cboProvinciaMensajeCliente.Items.Clear()
        cboProvinciaMensajeCliente.DataBind()
    End Sub

    Sub ListarPaisCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboPaisMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboPaisMensajeCliente.DataValueField = "cIdPaisUbicacionGeografica"
        cboPaisMensajeCliente.DataSource = UbicacionGeograficaNeg.PaisListarCombo
        cboPaisMensajeCliente.DataBind()
    End Sub

    Sub ListarDepartamentoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDepartamentoMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboDepartamentoMensajeCliente.DataValueField = "cIdDepartamentoUbicacionGeografica"
        cboDepartamentoMensajeCliente.DataSource = UbicacionGeograficaNeg.DepartamentoListarCombo(cboPaisMensajeCliente.SelectedValue)
        cboDepartamentoMensajeCliente.Items.Clear()
        cboDepartamentoMensajeCliente.DataBind()
    End Sub

    Protected Sub cboPaisMensajeCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPaisMensajeCliente.SelectedIndexChanged
        ListarDepartamentoCombo()
        ListarProvinciaCombo()
        ListarDistritoCombo()
        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboDepartamentoMensajeCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDepartamentoMensajeCliente.SelectedIndexChanged
        ListarProvinciaCombo()
        ListarDistritoCombo()
        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboProvinciaMensajeCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProvinciaMensajeCliente.SelectedIndexChanged
        ListarDistritoCombo()
        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnAdicionarCliente_Click(sender As Object, e As EventArgs) Handles btnAdicionarCliente.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            MyValidator.ErrorMessage = ""
            hfdIdTipoPersonaCliente.Value = ""
            hfdIdTipoCliente.Value = ""
            hfdIdUbicacionGeograficaCliente.Value = ""
            hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
            hfdIdTipoDocumentoCliente.Value = ""
            txtRazonSocial.Text = ""
            hfdNroDocumentoCliente.Value = ""
            cboClienteUbicacion.SelectedIndex = -1
            hfdDireccionFiscalCliente.Value = ""
            hfdTelefonoContactoCliente.Value = ""
            hfdCorreoElectronicoCliente.Value = ""
            txtTelefonoMensajeCliente.Text = ""
            lblTituloMensajeCliente.Visible = True

            Dim CliNeg As New clsClienteNegocios
            Dim ClienteRegistrado As New List(Of VI_GNRL_CLIENTE)
            If InStr(txtIdCliente.Text.Trim, "C") = 3 Then
                ClienteRegistrado = CliNeg.ClienteListaBusqueda("cIdCliente <> '' AND cIdCliente", IIf(txtIdCliente.Text.Trim = "", "NINGUNO", txtIdCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            ElseIf txtIdCliente.Text.Trim.Length = 8 Or txtIdCliente.Text.Trim.Length <> 11 Then 'DNI o CARNET EXTRANJERIA 
                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vDniCliente <> '' AND vDniCliente", IIf(txtIdCliente.Text.Trim = "", "NINGUNO", txtIdCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            Else
                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vRucCliente <> '' AND vRucCliente", IIf(txtIdCliente.Text.Trim = "", "NINGUNO", txtIdCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            End If

            'Crea la referencia al web service de BIMSIC
            Dim booValida As Boolean = IIf(ClienteRegistrado.Count > 0, False, True)
            'Dim booValida As Boolean
            'If ClienteRegistrado.Count > 0 Then
            '    booValida = False
            'Else
            '    booValida = FuncionesNeg.VerificarConexionURL("https://www.bimsic.net/WSSIW/wsSistemaIntegradoWebBIMSIC.asmx")
            'End If

            Dim WSCliente As New BIMSICWS.wsSistemaIntegradoWebBIMSICSoapClient
            Dim dsCliente As New DataSet
            If booValida = False Then 'No hay conexión con el WebServices
                If ClienteRegistrado.Count > 0 Then 'Existe en el sistema
                    Dim ClienteRegistradoFinal As GNRL_CLIENTE = CliNeg.ClienteListarPorId(ClienteRegistrado(0).Codigo, Session("IdEmpresa"), Session("IdPuntoVenta"))
                    IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente)
                    hfdIdAuxiliar.Value = ClienteRegistradoFinal.cIdCliente
                    hfdDNICliente.Value = ClienteRegistradoFinal.vDniCliente
                    hfdRUCCliente.Value = ClienteRegistradoFinal.vRucCliente
                    hfdNroDocumentoCliente.Value = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente) 'IIf(cboTipoDoc.SelectedValue = "FA", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                    txtRazonSocial.Text = ClienteRegistradoFinal.vRazonSocialCliente
                    hfdDireccionFiscalCliente.Value = ClienteRegistradoFinal.vDireccionCliente
                    hfdTelefonoContactoCliente.Value = ClienteRegistradoFinal.vTelefonoCliente
                    'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = hfdTelefonoContactoCliente.Value
                    hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                    hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona
                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    If cboClienteUbicacion.SelectedValue = "SELECCIONE DATO" Then
                        cboClienteUbicacion.SelectedIndex = -1
                    Else
                        cboClienteUbicacion.SelectedValue = "00"
                    End If

                    hfdIdTipoDocumentoCliente.Value = ClienteRegistradoFinal.cIdTipoDocumento
                    hfdCorreoElectronicoCliente.Value = ClienteRegistradoFinal.vEmailCliente
                    cboPaisMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdPaisUbicacionGeografica
                    cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                    cboDepartamentoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
                    cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                    cboProvinciaMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
                    cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                    cboDistritoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Hide()
                Else
                    If txtIdCliente.Text.Trim = "" Then
                        lnk_mostrarPanelCliente_ModalPopupExtender.Show()
                    Else
                        MyValidator.ErrorMessage = "Debe de ingresar el cliente desde la opción de mantenimiento."
                        ValidationSummary1.ValidationGroup = "vgrpValidar"
                        MyValidator.IsValid = False
                        MyValidator.ID = "ErrorPersonalizado"
                        MyValidator.ValidationGroup = "vgrpValidar"
                        Me.Page.Validators.Add(MyValidator)
                    End If
                End If
            Else
                If txtIdCliente.Text.Trim = "" Then
                    lnk_mostrarPanelCliente_ModalPopupExtender.Show()
                    Exit Sub
                ElseIf txtIdCliente.Text.Trim.Length = 8 Then 'cboTipoDoc.SelectedValue = "BV" And txtIdCliente.Text.Trim.Length = 8 Then

                ElseIf txtIdCliente.Text.Trim.Length = 11 Then 'cboTipoDoc.SelectedValue = "FA" And txtIdCliente.Text.Trim.Length = 11 Then
                    dsCliente = WSCliente.ConsultaRUC(txtIdCliente.Text)
                End If
                lblDatoInformativoMensajeCliente.Text = ""
                lblNroClienteMensajeCliente.Text = ""
                If dsCliente.Tables(0).Rows.Count > 0 Then
                    Dim fila As DataRow
                    For Each fila In dsCliente.Tables(0).Rows
                        If fila("vEstadoContribuyentePadronReducido") = "ACTIVO" And fila("vCondicionDomicilioPadronReducido") = "HABIDO" Then
                            If fila("cIdTipoDocumento") = "04" Then
                                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vRucCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                            Else
                                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vDniCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                            End If
                            If ClienteRegistrado.Count > 0 Then 'Existe en el sistema
                                Dim ClienteRegistradoFinal As GNRL_CLIENTE = CliNeg.ClienteListarPorId(ClienteRegistrado(0).Codigo, Session("IdEmpresa"), Session("IdPuntoVenta"))
                                If ClienteRegistradoFinal.vEstadoCliente = "ACTIVO" And ClienteRegistradoFinal.vCondicionCliente = "HABIDO" Then
                                    hfdIdAuxiliar.Value = ClienteRegistradoFinal.cIdCliente
                                    txtIdCliente.Text = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente)
                                    hfdDNICliente.Value = ClienteRegistradoFinal.vDniCliente
                                    hfdRUCCliente.Value = ClienteRegistradoFinal.vRucCliente
                                    hfdNroDocumentoCliente.Value = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente) 'IIf(cboTipoDoc.SelectedValue = "FA", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                                    txtRazonSocial.Text = ClienteRegistradoFinal.vRazonSocialCliente
                                    hfdDireccionFiscalCliente.Value = ClienteRegistradoFinal.vDireccionCliente
                                    hfdTelefonoContactoCliente.Value = ClienteRegistradoFinal.vTelefonoCliente
                                    hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                                    hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona

                                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                                    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                                    cboClienteUbicacion.SelectedValue = "00"
                                    hfdIdTipoDocumentoCliente.Value = ClienteRegistradoFinal.cIdTipoDocumento
                                    hfdCorreoElectronicoCliente.Value = ClienteRegistradoFinal.vEmailCliente

                                    cboPaisMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdPaisUbicacionGeografica
                                    cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                                    cboDepartamentoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
                                    cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                                    cboProvinciaMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
                                    cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                                    cboDistritoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
                                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Hide()
                                Else 'Si no está ACTIVO
                                    hfdIdAuxiliar.Value = ""
                                    hfdIdTipoPersonaCliente.Value = ""
                                    hfdIdTipoCliente.Value = ""
                                    hfdIdUbicacionGeograficaCliente.Value = ""
                                    hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
                                    hfdIdTipoDocumentoCliente.Value = ""
                                    txtIdCliente.Text = ""
                                    txtRazonSocial.Text = ""
                                    hfdDNICliente.Value = ""
                                    hfdRUCCliente.Value = ""
                                    hfdNroDocumentoCliente.Value = ""
                                    'cboDireccionEntrega.SelectedIndex = -1
                                    cboClienteUbicacion.SelectedIndex = -1
                                    hfdDireccionFiscalCliente.Value = ""
                                    hfdTelefonoContactoCliente.Value = ""
                                    'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = ""
                                    hfdCorreoElectronicoCliente.Value = ""
                                    lblNroClienteMensajeCliente.Text = "No puede emitir comprobantes"
                                    lblDatoInformativoMensajeCliente.Text = fila("vEstadoContribuyentePadronReducido") & "/" & fila("vCondicionDomicilioPadronReducido")
                                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                                End If
                            Else 'Esto es si no existe en el sistema pero esta como activo
                                Dim Cliente As New GNRL_CLIENTE
                                Cliente.bEstadoAceptanteCliente = False
                                Cliente.bEstadoRegistroCliente = True
                                Cliente.cGeneroCliente = "M"
                                Cliente.cIdCliente = ""
                                Cliente.cIdPaisUbicacionGeografica = "000"
                                Cliente.cIdDepartamentoUbicacionGeografica = Mid(fila("vUbiGeoPadronReducido"), 1, 2)
                                Cliente.cIdProvinciaUbicacionGeografica = Mid(fila("vUbiGeoPadronReducido"), 3, 2)
                                Cliente.cIdDistritoUbicacionGeografica = Mid(fila("vUbiGeoPadronReducido"), 5, 2)
                                Cliente.cIdEmpresa = Session("IdEmpresa")
                                Cliente.cIdEstadoCivil = "S"
                                Cliente.cIdPuntoVenta = Session("IdPuntoVenta")
                                Cliente.cIdTipoCliente = "01" 'CLIENTE FINAL
                                Cliente.cIdTipoDocumento = IIf(txtIdCliente.Text.Trim.Length = 8, "01", "04")
                                Cliente.cIdTipoPersona = CChar(IIf(txtIdCliente.Text.Trim.Length = 8, "N", "J"))
                                Cliente.dFechaNacimientoCliente = Nothing
                                If fila("cIdTipoDocumento") = "04" Then
                                    Cliente.vApellidoMaternoCliente = ""
                                    Cliente.vApellidoPaternoCliente = ""
                                    Cliente.vNombresCliente = ""
                                Else
                                    Dim strDatos() As String = fila("vRazonSocialPadronReducido").Split(",")
                                    Dim strDatos2() As String = strDatos(0).Trim.Split(" ")
                                    Cliente.vApellidoPaternoCliente = strDatos2(0).Trim
                                    Cliente.vApellidoMaternoCliente = strDatos2(1).Trim
                                    Cliente.vNombresCliente = strDatos(1).Trim
                                End If

                                Cliente.vCelularCliente = ""
                                Cliente.vDireccionCliente = IIf(txtIdCliente.Text.Trim.Length = 8, "", Trim(fila("vDireccionFiscal")))
                                If fila("cIdTipoDocumento") <> "04" Then
                                    Cliente.vDniCliente = Trim(fila("vDNIPadronReducido"))
                                Else
                                    Cliente.vDniCliente = ""
                                End If
                                Cliente.vEmailCliente = ""
                                Cliente.vFaxCliente = ""
                                Cliente.vRazonSocialCliente = Trim(Mid(fila("vRazonSocialPadronReducido"), 1, IIf(InStrRev(fila("vRazonSocialPadronReducido"), "-") = 0, Len(fila("vRazonSocialPadronReducido")), InStrRev(fila("vRazonSocialPadronReducido"), "-") - 1)))
                                Cliente.vRepresentanteLegalCliente = ""
                                Cliente.vRucCliente = IIf(fila("cIdTipoDocumento") = "04", fila("vRUCPadronReducido"), "")
                                Cliente.vTelefonoCliente = ""
                                Cliente.vEstadoCliente = Trim(fila("vEstadoContribuyentePadronReducido"))
                                Cliente.vCondicionCliente = Trim(fila("vCondicionDomicilioPadronReducido"))

                                hfdIdAuxiliar.Value = ""
                                hfdIdTipoPersonaCliente.Value = Cliente.cIdTipoPersona '"J"
                                hfdIdTipoCliente.Value = Cliente.cIdTipoCliente '"01"
                                hfdIdUbicacionGeograficaCliente.Value = "PE" & fila("vUbiGeoPadronReducido") 'El PE recién lo coloqué 20/06/2019
                                hfdIdUbicacionGeograficaClienteUbicacion.Value = "PE" & fila("vUbiGeoPadronReducido") 'El PE recién lo coloqué 20/06/2019
                                txtRazonSocial.Text = Cliente.vRazonSocialCliente
                                hfdDNICliente.Value = Cliente.vDniCliente
                                hfdRUCCliente.Value = Cliente.vRucCliente
                                hfdNroDocumentoCliente.Value = IIf(fila("cIdTipoDocumento") = "04", hfdRUCCliente.Value, hfdDNICliente.Value)
                                cboClienteUbicacion.SelectedIndex = -1
                                hfdDireccionFiscalCliente.Value = fila("vDireccionFiscal")
                                hfdTelefonoContactoCliente.Value = ""
                                hfdIdTipoDocumentoCliente.Value = Cliente.cIdTipoDocumento
                                If CliNeg.ClienteInserta(Cliente) = 0 Then
                                    hfdIdAuxiliar.Value = Cliente.cIdCliente
                                    lblNroClienteMensajeCliente.Text = Cliente.cIdCliente

                                    cboPaisMensajeCliente.SelectedValue = Cliente.cIdPaisUbicacionGeografica
                                    cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                                    cboDepartamentoMensajeCliente.SelectedValue = Cliente.cIdDepartamentoUbicacionGeografica
                                    cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                                    cboProvinciaMensajeCliente.SelectedValue = Cliente.cIdProvinciaUbicacionGeografica
                                    cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                                    cboDistritoMensajeCliente.SelectedValue = Cliente.cIdDistritoUbicacionGeografica

                                    lblDatoInformativoMensajeCliente.Text = "Cliente Registrado"
                                    txtDireccionAdicionalMensajeCliente.Text = hfdDireccionFiscalCliente.Value
                                    Dim TablaSistemaNeg As New clsTablaSistemaNegocios 'NUEVO JMUG: 02/03/2023
                                    hfdCorreoElectronicoCliente.Value = TablaSistemaNeg.TablaSistemaListarPorId("45", "15", Session("IdSistema"), Session("IdEmpresa"), "*").vValorOpcionalTablaSistema 'JMUG: 15/04/2022
                                    txtCorreoAdicionalMensajeCliente.Text = hfdCorreoElectronicoCliente.Value 'JMUG: 15/04/2022
                                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                                End If
                            End If
                        Else 'Si no está ACTIVO
                            hfdIdTipoPersonaCliente.Value = ""
                            hfdIdTipoCliente.Value = ""
                            hfdIdUbicacionGeograficaCliente.Value = ""
                            hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
                            hfdIdTipoDocumentoCliente.Value = ""
                            hfdIdAuxiliar.Value = ""
                            txtIdCliente.Text = ""
                            txtRazonSocial.Text = ""
                            hfdDNICliente.Value = ""
                            hfdRUCCliente.Value = ""
                            hfdNroDocumentoCliente.Value = ""
                            cboClienteUbicacion.SelectedIndex = -1
                            hfdDireccionFiscalCliente.Value = ""
                            hfdTelefonoContactoCliente.Value = ""
                            hfdCorreoElectronicoCliente.Value = ""
                            lblNroClienteMensajeCliente.Text = "No puede emitir comprobantes"
                            lblDatoInformativoMensajeCliente.Text = fila("vEstadoContribuyentePadronReducido") & "/" & fila("vCondicionDomicilioPadronReducido")
                            lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                        End If
                    Next
                Else 'No existe - Porque ese RUC no está registrado en la SUNAT
                    If txtIdCliente.Text.Trim.Length = 11 Then
                        ClienteRegistrado = CliNeg.ClienteListaBusqueda("vRucCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                    Else
                        ClienteRegistrado = CliNeg.ClienteListaBusqueda("vDniCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                    End If

                    If ClienteRegistrado.Count > 0 Then 'Existe en el sistema
                        Dim ClienteRegistradoFinal As GNRL_CLIENTE = CliNeg.ClienteListarPorId(ClienteRegistrado(0).Codigo, Session("IdEmpresa"), Session("IdPuntoVenta"))
                        hfdIdAuxiliar.Value = ClienteRegistradoFinal.cIdCliente
                        txtIdCliente.Text = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente)
                        hfdDNICliente.Value = ClienteRegistradoFinal.vDniCliente
                        hfdRUCCliente.Value = ClienteRegistradoFinal.vRucCliente
                        hfdNroDocumentoCliente.Value = IIf(ClienteRegistrado(0).Tipo_Doc = "04", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                        txtRazonSocial.Text = ClienteRegistradoFinal.vRazonSocialCliente
                        hfdDireccionFiscalCliente.Value = ClienteRegistradoFinal.vDireccionCliente
                        hfdTelefonoContactoCliente.Value = ClienteRegistradoFinal.vTelefonoCliente
                        hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                        hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona

                        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                        Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                        UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                        hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                        hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                        cboClienteUbicacion.SelectedValue = "00"
                        hfdIdTipoDocumentoCliente.Value = ClienteRegistradoFinal.cIdTipoDocumento

                        cboPaisMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdPaisUbicacionGeografica
                        cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                        cboDepartamentoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
                        cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                        cboProvinciaMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
                        cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                        cboDistritoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
                        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Hide()
                    Else
                        lblTituloMensajeCliente.Visible = False
                        hfdIdAuxiliar.Value = ""
                        txtIdCliente.Text = ""
                        hfdDNICliente.Value = ""
                        hfdRUCCliente.Value = ""
                        hfdNroDocumentoCliente.Value = ""
                        txtRazonSocial.Text = ""
                        hfdDireccionFiscalCliente.Value = ""
                        hfdTelefonoContactoCliente.Value = ""
                        cboClienteUbicacion.SelectedIndex = -1
                        hfdIdTipoCliente.Value = ""
                        hfdIdTipoPersonaCliente.Value = ""
                        hfdIdUbicacionGeograficaCliente.Value = ""
                        hfdIdTipoDocumentoCliente.Value = ""
                        lblNroClienteMensajeCliente.Text = "Este cliente no se encuentra registrado, por favor registrelo en el Mantenimiento de Clientes"
                        lblDatoInformativoMensajeCliente.Text = "Cliente No Registrado"
                        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                    End If
                End If
            End If
        Catch ex As Exception
            MyValidator.ErrorMessage = "No se estableció conexión con la SUNAT. Utilizará los datos del sistema."
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnAgregarCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnAgregarCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0674", strOpcionModulo, "CMMS")

            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If

            If IsNothing(grdCatalogoComponente.SelectedRow) = False Then
                clsLogiCestaEquipo.AgregarCesta("", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(0).Text),
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text),
                                               "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(3).Text),
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(0).Text).Trim,
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(4).Text), Now,
                                               "1", txtIdEquipo.Text.Trim, "", "1", "", "", "1", "1",
                                               "", "", "01",
                                               Session("CestaEquipoComponente"))

                clsLogiCestaCatalogo.QuitarCesta(grdCatalogoComponente.SelectedIndex, Session("CestaCatalogoComponente"))
                MyValidator.ErrorMessage = "Componente asignado satisfactoriamente."
            End If

            Dim result As DataRow() = Session("CestaEquipoComponente").Select("IdCatalogo ='" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(0).Text) & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim & "'")
            rowIndexDetalle = Session("CestaEquipoComponente").Rows.IndexOf(result(0))
            CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

            grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            grdCatalogoComponente.DataBind()

            grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            grdEquipoComponente.DataBind()

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

    Private Sub lnkEsComponenteOn_Click(sender As Object, e As EventArgs) Handles lnkEsComponenteOn.Click
        If lnkEsComponenteOn.Visible = True Then
            lnkEsComponenteOn.Visible = False
            lnkEsComponenteOff.Visible = True
            divSistemaFuncional.Visible = False
        Else
            lnkEsComponenteOn.Visible = True
            lnkEsComponenteOff.Visible = False
            divSistemaFuncional.Visible = True
        End If
    End Sub

    Private Sub lnkEsComponenteOff_Click(sender As Object, e As EventArgs) Handles lnkEsComponenteOff.Click
        If lnkEsComponenteOff.Visible = True Then
            lnkEsComponenteOn.Visible = True
            lnkEsComponenteOff.Visible = False
            divSistemaFuncional.Visible = True
        Else
            lnkEsComponenteOn.Visible = False
            lnkEsComponenteOff.Visible = True
            divSistemaFuncional.Visible = False
        End If
    End Sub

    Private Sub lnkEsConContratoOn_Click(sender As Object, e As EventArgs) Handles lnkEsConContratoOn.Click
        If lnkEsConContratoOn.Visible = True Then
            lnkEsConContratoOn.Visible = False
            lnkEsConContratoOff.Visible = True
            divContratoReferencia.Visible = False
        Else
            lnkEsConContratoOn.Visible = True
            lnkEsConContratoOff.Visible = False
            divContratoReferencia.Visible = True
        End If
    End Sub

    Private Sub lnkEsConContratoOff_Click(sender As Object, e As EventArgs) Handles lnkEsConContratoOff.Click
        If lnkEsConContratoOff.Visible = True Then
            lnkEsConContratoOn.Visible = True
            lnkEsConContratoOff.Visible = False
            divContratoReferencia.Visible = True
        Else
            lnkEsConContratoOn.Visible = False
            lnkEsConContratoOff.Visible = True
            divContratoReferencia.Visible = False
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Codigo
            e.Row.Cells(1).Visible = True 'RucCliente/RazonSocialCliente
            e.Row.Cells(2).Visible = True 'FechaRegistroTarjetaSAP
            e.Row.Cells(3).Visible = True 'FechaUltimaModificacion
            e.Row.Cells(4).Visible = False 'IdTipoActivo
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(7).Visible = True 'Descripcion
            e.Row.Cells(8).Visible = True 'Descripcion SAP
            e.Row.Cells(9).Visible = True 'Estado Equipo
            e.Row.Cells(10).Visible = False 'Estado Registro
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Codigo
            e.Row.Cells(1).Visible = True 'RucCliente/RazonSocialCliente
            e.Row.Cells(2).Visible = True 'FechaRegistroTarjetaSAP
            e.Row.Cells(3).Visible = True 'FechaUltimaModificacion
            e.Row.Cells(4).Visible = False 'IdTipoActivo
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(7).Visible = True 'Descripcion
            e.Row.Cells(8).Visible = True 'Descripcion SAP
            e.Row.Cells(9).Visible = True 'Estado Equipo
            e.Row.Cells(10).Visible = False 'Estado Registro
        End If
    End Sub

    Private Sub imgbtnBuscarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarEquipo.Click
        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")


        'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
        'Me.grdLista.DataBind()

        If (Session("IdContratoUsuario") = "*") Then
            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
            Me.grdLista.DataBind()
        ElseIf (Session("IdContratoUsuario") <> Nothing) Then
            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
            Me.grdLista.DataBind()
        End If
    End Sub

    Private Sub grdDetalleCaracteristicaCatalogoComponente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaCatalogoComponente.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogoComponente.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                            If (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaCatalogoComponente"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponente")
            Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()
            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnEditarEquipoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarEquipoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If

            hfdOperacionDetalle.Value = "E"
            If grdEquipoComponente.Rows.Count > 0 Then
                If grdEquipoComponente.SelectedIndex < grdEquipoComponente.Rows.Count Then
                    If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                        If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                            Dim result As DataRow() = Session("CestaEquipoComponente").Select("IdCatalogo ='" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text).Trim & "'")
                            rowIndexDetalle = Session("CestaEquipoComponente").Rows.IndexOf(result(0))
                            BloquearMantenimiento(False, True, False, True)
                            ValidarTexto(True)
                            ActivarObjetos(True)
                            LlenarDataEquipoComponente()
                            Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                            Me.grdDetalleCaracteristicaEquipoComponente.DataBind()

                            lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar algún item.")
                    End If
                End If
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

    Public Function SplitCadenaContratoReferencia(ByVal cadena As String) As String
        Dim resultado As String

        ' Encuentra el índice del primer espacio
        Dim indiceEspacio As Integer = cadena.IndexOf(" ")

        If indiceEspacio > 0 Then
            ' Extrae todo antes del primer espacio
            resultado = cadena.Substring(0, indiceEspacio)
        Else
            ' Si no hay espacio, toma toda la cadena
            resultado = cadena
        End If

        Return resultado
    End Function

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0653", strOpcionModulo, "CMMS")

            If cboTipoEquipo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de equipo.")
            End If

            Dim Equipo As New LOGI_EQUIPO
            Equipo.cIdEquipo = UCase(txtIdEquipo.Text)
            Equipo.cIdEmpresa = Session("IdEmpresa")
            Equipo.cIdTipoActivo = IIf(cboTipoActivo.SelectedValue = "SELECCIONE DATO", Nothing, cboTipoActivo.SelectedValue)
            Equipo.vDescripcionEquipo = UCase(txtDescripcionEquipo.Text)
            Equipo.vDescripcionAbreviadaEquipo = "" 'UCase(txtDescripcionAbreviada.Text)
            Equipo.dFechaTransaccionEquipo = Convert.ToDateTime(IIf(hfdFechaEquipo.Value = "", Now, hfdFechaEquipo.Value)) 'hfdFechaTransaccionEquipoComponente.Value
            Equipo.cIdEnlaceEquipo = ""
            Equipo.cIdEnlaceCatalogo = ""
            Equipo.bEstadoRegistroEquipo = True
            Equipo.cIdSistemaFuncionalEquipo = cboSistemaFuncional.SelectedValue  'lblSistemaFuncional.Value
            Equipo.cIdFamilia = Nothing
            Equipo.cIdSubFamilia = Nothing
            Equipo.vObservacionEquipo = "" 'UCase(txtObservacion.Text)
            Equipo.cIdEstadoComponenteEquipo = "01" '"cboEstadoActivoMaestroActivoPrincipal.SelectedValue
            Equipo.vIdEquivalenciaEquipo = "" 'txtIdInternoMaestroActivoPrincipal.Text
            Equipo.cIdPaisOrigenEquipo = Session("IdPais")
            Equipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
            Equipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
            Equipo.vIdArticuloSAPEquipo = hfdIdArticuloSAPEquipo.Value
            Equipo.dFechaRegistroTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaRegistroTarjetaSAPEquipo.Value), hfdFechaRegistroTarjetaSAPEquipo.Value, Nothing))
            Equipo.dFechaRegistroTarjetaSAPEquipo = IIf(Equipo.dFechaRegistroTarjetaSAPEquipo = "01/01/0001", Nothing, Equipo.dFechaRegistroTarjetaSAPEquipo)
            Equipo.dFechaManufacturaTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaManufacturaTarjetaSAPEquipo.Value), hfdFechaManufacturaTarjetaSAPEquipo.Value, Nothing))
            Equipo.dFechaManufacturaTarjetaSAPEquipo = IIf(Equipo.dFechaManufacturaTarjetaSAPEquipo = "01/01/0001", Nothing, Equipo.dFechaManufacturaTarjetaSAPEquipo)
            Equipo.cIdCatalogo = IIf(cboCatalogo.SelectedValue = "SELECCIONE DATO", Nothing, cboCatalogo.SelectedValue) 'txtIdMaestroActivo.Text
            Equipo.cIdJerarquiaCatalogo = "0"
            Equipo.nVidaUtilEquipo = Convert.ToInt32(IIf(txtVidaUtilMantenimientoCatalogo.Text.Trim = "", "0", txtVidaUtilMantenimientoCatalogo.Text.Trim)) 'Convert.ToDecimal(IIf(txtVidaUtilMaestroActivoPrincipal.Text.Trim = "", 0, txtVidaUtilMaestroActivoPrincipal.Text))
            Equipo.nPeriodoGarantiaEquipo = Convert.ToInt32(IIf(txtPeriodoGarantiaMantenimientoCatalogo.Text.Trim = "", "0", txtPeriodoGarantiaMantenimientoCatalogo.Text.Trim)) 'txtPeriodoGarant
            Equipo.nPeriodoMinimoMantenimientoEquipo = Convert.ToInt32(IIf(txtPeriodoMinimoMantenimientoCatalogo.Text.Trim = "", "0", txtPeriodoMinimoMantenimientoCatalogo.Text.Trim))
            Equipo.vNumeroSerieEquipo = txtNroSerieEquipo.Text
            Equipo.vNumeroParteEquipo = txtNroParteEquipo.Text
            Equipo.cIdCliente = hfdIdAuxiliar.Value
            Equipo.cIdClienteUbicacion = IIf(cboClienteUbicacion.SelectedValue = "SELECCIONE DATO", Nothing, cboClienteUbicacion.SelectedValue)
            Equipo.dFechaCreacionEquipo = Convert.ToDateTime(IIf(hfdFechaCreacionEquipo.Value = "", Now, hfdFechaCreacionEquipo.Value))
            Equipo.dFechaUltimaModificacionEquipo = Now
            Equipo.cIdUsuarioCreacionEquipo = hfdIdUsuarioCreacionEquipo.Value
            Equipo.cIdUsuarioUltimaModificacionEquipo = Session("IdUsuario")
            Equipo.cEstadoEquipo = "R" 'Registrado
            Equipo.bTieneContratoEquipo = lnkEsConContratoOn.Visible
            Equipo.vContratoReferenciaActualEquipo = IIf(CBool(Equipo.bTieneContratoEquipo) = True, IIf(cboContratoReferencia.SelectedValue = "SELECCIONE DATO", Nothing, cboContratoReferencia.SelectedValue), Nothing)
            Equipo.vTagEquipo = UCase(txtTagEquipo.Text.Trim)
            Equipo.vCapacidadEquipo = UCase(txtCapacidadEquipo.Text.Trim)
            Equipo.vAreaEquipo = UCase(txtAreaEquipo.Text.Trim)
            Equipo.cIdTipoEquipo = cboTipoEquipo.SelectedValue
            Equipo.vDescripcionEquipoSAP = UCase(txtDescripcionEquipoSAP.Text.Trim)

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacion.Value = "N" Then
                If (EquipoNeg.EquipoInserta(Equipo)) = 0 Then
                    Dim Coleccion As New List(Of LOGI_EQUIPO)

                    If (cboContratoReferencia.SelectedValue <> "SELECCIONE DATO" And Not String.IsNullOrEmpty(cboContratoReferencia.SelectedValue)) Then
                        Dim cadena = SplitCadenaContratoReferencia(cboContratoReferencia.SelectedValue)
                        Dim lista As List(Of String) = cadena.Split("-"c).ToList()
                        ContratoNeg.ContratoInsertaDetalleReferenciaxEquipo(lista(0), lista(1), lista(2), Equipo.cIdEmpresa, Equipo.cIdEquipo, Equipo.vDescripcionEquipo)
                    End If


                    For i = 0 To Session("CestaEquipoComponente").Rows.Count - 1
                        Dim DetEquipo As New LOGI_EQUIPO
                        DetEquipo.cIdEquipo = Session("CestaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                        DetEquipo.cIdEmpresa = Session("IdEmpresa")
                        DetEquipo.cIdTipoActivo = Session("CestaEquipoComponente").Rows(i)("IdTipoActivo").ToString.Trim
                        DetEquipo.vDescripcionEquipo = Session("CestaEquipoComponente").Rows(i)("Descripcion").ToString.Trim
                        DetEquipo.vDescripcionAbreviadaEquipo = Session("CestaEquipoComponente").Rows(i)("DescripcionAbreviada").ToString.Trim
                        DetEquipo.dFechaTransaccionEquipo = hfdFechaTransaccionEquipoComponente.Value
                        DetEquipo.cIdEnlaceEquipo = Equipo.cIdEquipo 'Session("CestaMaestroActivoComponente").Rows(i)("IdEnlace").ToString.Trim
                        DetEquipo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'MaestroActivo.cIdEnlaceCatalogo
                        DetEquipo.bEstadoRegistroEquipo = "1"
                        DetEquipo.cIdSistemaFuncionalEquipo = Session("CestaEquipoComponente").Rows(i)("IdSistemaFuncional").ToString.Trim
                        DetEquipo.cIdFamilia = Nothing
                        DetEquipo.cIdSubFamilia = Nothing
                        DetEquipo.vObservacionEquipo = Session("CestaEquipoComponente").Rows(i)("Observacion").ToString.Trim
                        DetEquipo.cIdEstadoComponenteEquipo = Session("CestaEquipoComponente").Rows(i)("IdEstadoActivo").ToString.Trim
                        DetEquipo.vIdEquivalenciaEquipo = "" 'Session("CestaEquipoComponente").Rows(i)("IdEquivalencia").ToString.Trim 'JMUG: 31/07/2023 LO QUITÉ PORQUE ME DA ERROR AL CREAR UNO NUEVO
                        DetEquipo.cIdPaisOrigenEquipo = Session("IdPais")
                        DetEquipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
                        DetEquipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
                        DetEquipo.dFechaRegistroTarjetaSAPEquipo = Now 'hfdFechaRegistroTarjetaSAPEquipo.value
                        DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Now
                        DetEquipo.cIdCatalogo = Session("CestaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                        DetEquipo.cIdJerarquiaCatalogo = "1"
                        DetEquipo.nVidaUtilEquipo = Session("CestaEquipoComponente").Rows(i)("VidaUtil").ToString.Trim
                        DetEquipo.nPeriodoGarantiaEquipo = Nothing
                        DetEquipo.nPeriodoMinimoMantenimientoEquipo = Nothing
                        DetEquipo.vNumeroSerieEquipo = Session("CestaEquipoComponente").Rows(i)("NroSerie").ToString.Trim
                        DetEquipo.vNumeroParteEquipo = Session("CestaEquipoComponente").Rows(i)("NroParte").ToString.Trim
                        DetEquipo.cIdCliente = hfdIdAuxiliar.Value

                        DetEquipo.cIdClienteUbicacion = IIf(cboClienteUbicacion.SelectedValue = "SELECCIONE DATO", Nothing, cboClienteUbicacion.SelectedValue)
                        DetEquipo.dFechaCreacionEquipo = IIf(hfdFechaCreacionEquipoComponente.Value = "", Now, hfdFechaCreacionEquipoComponente.Value)
                        DetEquipo.dFechaUltimaModificacionEquipo = Now
                        DetEquipo.cIdUsuarioCreacionEquipo = hfdIdUsuarioCreacionEquipoComponente.Value
                        DetEquipo.cIdUsuarioUltimaModificacionEquipo = Session("IdUsuario")
                        Coleccion.Add(DetEquipo)
                    Next

                    If EquipoNeg.EquipoInsertaDetalle(Coleccion, Equipo.cIdEquipo, Equipo.cIdCatalogo) = 0 Then
                        Session("Query") = "PA_LOGI_MNT_EQUIPO 'SQL_INSERT', '','" & Equipo.cIdEquipo & "', '" & Equipo.cIdCatalogo & "', '" &
                                           Equipo.cIdTipoActivo & "', '" & Equipo.vDescripcionEquipo & "', '" &
                                           Equipo.vDescripcionAbreviadaEquipo & "', '" & Equipo.cIdJerarquiaCatalogo & "', '" &
                                           Equipo.dFechaTransaccionEquipo & "', '" & Equipo.cIdEnlaceEquipo & "', '" &
                                           Equipo.cIdEnlaceCatalogo & "', '" & Equipo.cIdSistemaFuncionalEquipo & "', '" & Equipo.bEstadoRegistroEquipo & "', '" &
                                           Equipo.vObservacionEquipo & "', " & Equipo.nVidaUtilEquipo & ", '" &
                                           Equipo.cIdEmpresa & "', '" & Equipo.cIdEstadoComponenteEquipo & "', '" &
                                           Equipo.vIdEquivalenciaEquipo & "', '" & Equipo.cIdPaisOrigenEquipo & "', " &
                                           Equipo.vIdClienteSAPEquipo & ", '" & Equipo.cIdEquipoSAPEquipo & "', '" &
                                           Equipo.dFechaRegistroTarjetaSAPEquipo & "', '" & Equipo.dFechaManufacturaTarjetaSAPEquipo & "', '" &
                                           Equipo.nPeriodoGarantiaEquipo & "', '" & Equipo.nPeriodoMinimoMantenimientoEquipo & "', '" &
                                           Equipo.vNumeroSerieEquipo & "', '" & Equipo.vNumeroParteEquipo & "', '" & Equipo.cIdCliente & "', '" &
                                           Equipo.cIdEquipo & "'"

                        LogAuditoria.vEvento = "INSERTAR EQUIPO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo 'Session("IdModulo")

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        txtIdEquipo.Text = Equipo.cIdEquipo
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        CargarCestaCatalogoComponente()
                        CargarCestaEquipoComponente()

                        'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, 0)
                        'Me.grdLista.DataBind()

                        If (Session("IdContratoUsuario") = "*") Then
                            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                            Me.grdLista.DataBind()
                        ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
                            Me.grdLista.DataBind()
                        End If

                        pnlCabecera.Visible = True
                        pnlEquipo.Visible = False
                        pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarEquipo.Focus()
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (EquipoNeg.EquipoEdita(Equipo)) = 0 Then
                    Dim Coleccion As New List(Of LOGI_EQUIPO)
                    For i = 0 To Session("CestaEquipoComponente").Rows.Count - 1
                        Dim DetEquipo As New LOGI_EQUIPO

                        DetEquipo.cIdEquipo = Session("CestaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                        DetEquipo.cIdEmpresa = Session("IdEmpresa")
                        DetEquipo.cIdTipoActivo = Session("CestaEquipoComponente").Rows(i)("IdTipoActivo").ToString.Trim
                        DetEquipo.vDescripcionEquipo = Session("CestaEquipoComponente").Rows(i)("Descripcion").ToString.Trim
                        DetEquipo.vDescripcionAbreviadaEquipo = Session("CestaEquipoComponente").Rows(i)("DescripcionAbreviada").ToString.Trim
                        DetEquipo.dFechaTransaccionEquipo = Session("CestaEquipoComponente").Rows(i)("FechaTransaccion").ToString.Trim
                        DetEquipo.cIdEnlaceEquipo = Equipo.cIdEquipo 'Session("CestaMaestroActivoComponente").Rows(i)("IdEnlace").ToString.Trim
                        DetEquipo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'MaestroActivo.cIdEnlaceCatalogo
                        DetEquipo.bEstadoRegistroEquipo = "1"
                        DetEquipo.cIdSistemaFuncionalEquipo = Session("CestaEquipoComponente").Rows(i)("IdSistemaFuncional").ToString.Trim
                        DetEquipo.cIdFamilia = Nothing
                        DetEquipo.cIdSubFamilia = Nothing
                        DetEquipo.vObservacionEquipo = Session("CestaEquipoComponente").Rows(i)("Observacion").ToString.Trim
                        DetEquipo.cIdEstadoComponenteEquipo = "01" 'Session("CestaEquipoComponente").Rows(i)("IdEstadoActivo").ToString.Trim
                        DetEquipo.vIdEquivalenciaEquipo = "" 'Session("CestaEquipoComponente").Rows(i)("IdEquivalencia").ToString.Trim
                        DetEquipo.cIdPaisOrigenEquipo = Session("IdPais")
                        DetEquipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
                        DetEquipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
                        If String.IsNullOrEmpty(hfdFechaRegistroTarjetaSAPEquipo.Value) Then
                            DetEquipo.dFechaRegistroTarjetaSAPEquipo = Nothing
                        Else
                            DetEquipo.dFechaRegistroTarjetaSAPEquipo = Convert.ToDateTime(hfdFechaRegistroTarjetaSAPEquipo.Value)
                        End If
                        If String.IsNullOrEmpty(hfdFechaRegistroTarjetaSAPEquipo.Value) Then
                            DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Nothing ' Asignar valor predeterminado o Nothing según tu lógica
                        Else
                            DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Convert.ToDateTime(hfdFechaRegistroTarjetaSAPEquipo.Value)
                        End If
                        DetEquipo.cIdCatalogo = Session("CestaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                        DetEquipo.cIdJerarquiaCatalogo = "1"
                        DetEquipo.nVidaUtilEquipo = Session("CestaEquipoComponente").Rows(i)("VidaUtil").ToString.Trim
                        DetEquipo.nPeriodoGarantiaEquipo = Session("CestaEquipoComponente").Rows(i)("PeriodoGarantia").ToString.Trim 'Nothing
                        DetEquipo.nPeriodoMinimoMantenimientoEquipo = Session("CestaEquipoComponente").Rows(i)("PeriodoMinimo").ToString.Trim ' Nothing
                        DetEquipo.vNumeroSerieEquipo = Session("CestaEquipoComponente").Rows(i)("NroSerie").ToString.Trim
                        DetEquipo.vNumeroParteEquipo = Session("CestaEquipoComponente").Rows(i)("NroParte").ToString.Trim
                        DetEquipo.cIdCliente = hfdIdAuxiliar.Value
                        DetEquipo.cIdClienteUbicacion = IIf(cboClienteUbicacion.SelectedValue = "SELECCIONE DATO", Nothing, cboClienteUbicacion.SelectedValue)
                        DetEquipo.dFechaCreacionEquipo = Convert.ToDateTime(IIf(hfdFechaCreacionEquipoComponente.Value = "", Now, hfdFechaCreacionEquipoComponente.Value))
                        DetEquipo.dFechaUltimaModificacionEquipo = Now
                        DetEquipo.cIdUsuarioCreacionEquipo = hfdIdUsuarioCreacionEquipoComponente.Value
                        DetEquipo.cIdUsuarioUltimaModificacionEquipo = Session("IdUsuario")

                        Coleccion.Add(DetEquipo)
                    Next

                    If EquipoNeg.EquipoInsertaDetalle(Coleccion, Equipo.cIdEquipo, cboCatalogo.SelectedValue) = 0 Then
                        Session("Query") = "PA_LOGI_MNT_EQUIPO 'SQL_INSERT', '','" & Equipo.cIdEquipo & "', '" & Equipo.cIdCatalogo & "', '" &
                                           Equipo.cIdTipoActivo & "', '" & Equipo.vDescripcionEquipo & "', '" &
                                           Equipo.vDescripcionAbreviadaEquipo & "', '" & Equipo.cIdJerarquiaCatalogo & "', '" &
                                           Equipo.dFechaTransaccionEquipo & "', '" & Equipo.cIdEnlaceEquipo & "', '" &
                                           Equipo.cIdEnlaceCatalogo & "', '" & Equipo.cIdSistemaFuncionalEquipo & "', '" & Equipo.bEstadoRegistroEquipo & "', '" &
                                           Equipo.vObservacionEquipo & "', " & Equipo.nVidaUtilEquipo & ", '" &
                                           Equipo.cIdEmpresa & "', '" & Equipo.cIdEstadoComponenteEquipo & "', '" &
                                           Equipo.vIdEquivalenciaEquipo & "', '" & Equipo.cIdPaisOrigenEquipo & "', " &
                                           Equipo.vIdClienteSAPEquipo & ", '" & Equipo.cIdEquipoSAPEquipo & "', '" &
                                           Equipo.dFechaRegistroTarjetaSAPEquipo & "', '" & Equipo.dFechaManufacturaTarjetaSAPEquipo & "', '" &
                                           Equipo.nPeriodoGarantiaEquipo & "', '" & Equipo.nPeriodoMinimoMantenimientoEquipo & "', '" &
                                           Equipo.vNumeroSerieEquipo & "', '" & Equipo.vNumeroParteEquipo & "', '" & Equipo.cIdCliente & "', '" &
                                           Equipo.cIdEquipo & "'"

                        LogAuditoria.vEvento = "ACTUALIZAR EQUIPO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo 'Session("IdModulo")

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                        Dim item As Int16
                        Dim strIdEquipo As String = ""

                        item = 0
                        Dim resultCaracteristicaEquipoPrincipalSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & Equipo.cIdCatalogo & "'")
                        If resultCaracteristicaEquipoPrincipalSimple.Length > 0 Then
                            Dim rowFil As DataRow() = resultCaracteristicaEquipoPrincipalSimple
                            For Each filacaracteristica As DataRow In rowFil
                                Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                                item = item + 1
                                DetCaracteristica.cIdEquipo = Equipo.cIdEquipo
                                DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                                DetCaracteristica.cIdCaracteristica = filacaracteristica("IdCaracteristica") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                                DetCaracteristica.nIdNumeroItemEquipoCaracteristica = item 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorReferencialEquipoCaracteristica = UCase(filacaracteristica("Valor")) 'UCase(Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim)
                                DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = filacaracteristica("IdReferenciaSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = filacaracteristica("DescripcionCampoSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                DetCaracteristica.cIdCatalogoEquipoCaracteristica = filacaracteristica("IdCatalogo")
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                        End If
                        'JMUG: FINAL: 26/03/2023

                        For Each fila In Coleccion
                            item = 0
                            Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & fila.cIdCatalogo & "' AND IdJerarquia = '1'")
                            If resultCaracteristicaSimple.Length > 0 Then
                                Dim rowFil As DataRow() = resultCaracteristicaSimple
                                For Each filacaracteristica As DataRow In rowFil
                                    Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                                    item = item + 1
                                    DetCaracteristica.cIdEquipo = fila.cIdEquipo
                                    DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                                    DetCaracteristica.cIdCaracteristica = filacaracteristica("IdCaracteristica") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                                    DetCaracteristica.nIdNumeroItemEquipoCaracteristica = item 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                                    DetCaracteristica.vValorReferencialEquipoCaracteristica = UCase(filacaracteristica("Valor")) 'UCase(Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim)
                                    DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = filacaracteristica("IdReferenciaSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                    DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = filacaracteristica("DescripcionCampoSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                    DetCaracteristica.cIdCatalogoEquipoCaracteristica = filacaracteristica("IdCatalogo") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                    ColeccionCaracteristica.Add(DetCaracteristica)
                                Next
                            End If
                        Next

                        If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(Equipo.cIdEquipo, ColeccionCaracteristica, LogAuditoria) Then

                        End If

                        MyValidator.ErrorMessage = "Registro actualizado con éxito"

                        'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, 0)
                        'Me.grdLista.DataBind()

                        If (Session("IdContratoUsuario") = "*") Then
                            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                            Me.grdLista.DataBind()
                        ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
                            Me.grdLista.DataBind()
                        End If

                        pnlCabecera.Visible = True
                        pnlEquipo.Visible = False
                        pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarEquipo.Focus()
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                    hfdOperacion.Value = "R"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If

            Me.grdCatalogoComponente.DataSource = Nothing
            Me.grdCatalogoComponente.DataBind()
            Me.grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            Me.grdCatalogoComponente.DataBind()

            Me.grdEquipoComponente.DataSource = Nothing
            Me.grdEquipoComponente.DataBind()
            Me.grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            Me.grdEquipoComponente.DataBind()

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

                EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
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

                'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                'Me.grdLista.DataBind()

                If (Session("IdContratoUsuario") = "*") Then
                    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                    Me.grdLista.DataBind()
                ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
                    Me.grdLista.DataBind()
                End If
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
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

                EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 1 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
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

                'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                'Me.grdLista.DataBind()

                If (Session("IdContratoUsuario") = "*") Then
                    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                    Me.grdLista.DataBind()
                ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
                    Me.grdLista.DataBind()
                End If
            End If
            If e.CommandName = "Eliminar" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                EquipoNeg.EquipoGetData("UPDATE LOGI_DOCUMENTACIONEQUIPO SET bEstadoRegistroDocumentacionEquipo = 0 WHERE cIdEquipo = '" & Valores(1).ToString & "' AND nIdNumeroItemDocumentacionEquipo = '" & Valores(0).ToString & "'")
                grdListaVerDocumentosEquipo.DataSource = EquipoNeg.EquipoGetData("SELECT vTituloDocumentacionEquipo AS Titulo, vDescripcionDocumentacionEquipo AS Descripcion, CONVERT(VARCHAR(20),dFechaCarga,103)+' '+CONVERT(VARCHAR(20),dFechaCarga,108) AS Carga, vNombreArchivoDocumentacionEquipo AS NombreArchivo, cIdEquipo AS IdEquipo, nIdNumeroItemDocumentacionEquipo AS Item " &
                                                                                             "FROM LOGI_DOCUMENTACIONEQUIPO " &
                                                                                             "WHERE bEstadoRegistroDocumentacionEquipo=1 and cIdEquipo = '" & Valores(1).ToString & "'")
                grdListaVerDocumentosEquipo.DataBind()
                divDetalleVerDocumentosEquipo.Visible = False
                lnk_mostrarPanelSubirDocumentacionEquipo_ModalPopupExtender.Show()
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
        'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
        'Me.grdLista.DataBind() 'Recargo el grid.
        If (Session("IdContratoUsuario") = "*") Then
            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
            Me.grdLista.DataBind()
        ElseIf (Session("IdContratoUsuario") <> Nothing) Then
            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
            Me.grdLista.DataBind()
        End If
        grdLista.SelectedIndex = -1
    End Sub

    Sub ListarContratoReferenciaCombo(Optional ByVal cIdCliente As String = "", Optional ByVal isClear As Boolean = False)
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        If isClear = True Then
            cboContratoReferencia.Items.Clear()
            cboContratoReferencia.Items.Add("SELECCIONE DATO")
        End If

        Dim ContratoNeg As New clsContratoNegocios
        cboContratoReferencia.DataTextField = "vDescripcionCabeceraContrato"
        cboContratoReferencia.DataValueField = "vIdNumeroCorrelativoCabeceraContrato"
        'cboContratoReferencia.DataSource = ContratoNeg.ContratoListarCombo("1", "'R','P'")
        cboContratoReferencia.DataSource = IIf(cIdCliente.Trim <> "", ContratoNeg.ContratoListarComboByCliente("1", "'R','P'", cIdCliente), ContratoNeg.ContratoListarCombo("1", "'R','P'"))
        cboContratoReferencia.DataBind()
    End Sub

    Sub ListarPaisClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboPaisMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboPaisMensajeClienteUbicacion.DataValueField = "cIdPaisUbicacionGeografica"
        cboPaisMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.PaisListarCombo
        cboPaisMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarDepartamentoClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDepartamentoMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboDepartamentoMensajeClienteUbicacion.DataValueField = "cIdDepartamentoUbicacionGeografica"
        cboDepartamentoMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.DepartamentoListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue)
        cboDepartamentoMensajeClienteUbicacion.Items.Clear()
        cboDepartamentoMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarProvinciaClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboProvinciaMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboProvinciaMensajeClienteUbicacion.DataValueField = "cIdProvinciaUbicacionGeografica"
        cboProvinciaMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.ProvinciaListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue)
        cboProvinciaMensajeClienteUbicacion.Items.Clear()
        cboProvinciaMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarDistritoClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDistritoMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboDistritoMensajeClienteUbicacion.DataValueField = "cIdDistritoUbicacionGeografica"
        cboDistritoMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.DistritoListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue, cboProvinciaMensajeClienteUbicacion.SelectedValue)
        cboDistritoMensajeClienteUbicacion.Items.Clear()
        cboDistritoMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarClienteUbicacionCombo()
        Try
            Dim DirClienteNeg As New clsClienteNegocios
            cboClienteUbicacion.DataTextField = "vDescripcionClienteUbicacion"
            cboClienteUbicacion.DataValueField = "cIdUbicacion"
            cboClienteUbicacion.DataSource = DirClienteNeg.ClienteGetData("SELECT cIdCliente, CLIUBI.cIdUbicacion, UPPER(vDescripcionClienteUbicacion) + ' - ' + " &
                                             "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
                                             "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + '00' + '00') " &
                                             " + ' - ' + " &
                                             "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
                                             "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + CLIUBI.cIdProvinciaUbicacionGeografica + '00') " &
                                             " + ' - ' + " &
                                             "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
                                             "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + CLIUBI.cIdProvinciaUbicacionGeografica + CLIUBI.cIdDistritoUbicacionGeografica) AS vDescripcionClienteUbicacion " &
                                             "FROM GNRL_CLIENTEUBICACION AS CLIUBI WHERE LTRIM(RTRIM(CLIUBI.cIdCliente)) = '" & hfdIdAuxiliar.Value.Trim & "' " &
                                             "ORDER BY cIdCliente")
            cboClienteUbicacion.Items.Clear()
            cboClienteUbicacion.Items.Add("SELECCIONE DATO")
            cboClienteUbicacion.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub cboPaisMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPaisMensajeClienteUbicacion.SelectedIndexChanged
        ListarDepartamentoClienteUbicacionCombo()
        ListarProvinciaClienteUbicacionCombo()
        ListarDistritoClienteUbicacionCombo()
        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboDepartamentoMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDepartamentoMensajeClienteUbicacion.SelectedIndexChanged
        ListarProvinciaClienteUbicacionCombo()
        ListarDistritoClienteUbicacionCombo()
        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboProvinciaMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProvinciaMensajeClienteUbicacion.SelectedIndexChanged
        ListarDistritoClienteUbicacionCombo()
        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAdicionarClienteUbicacion_Click(sender As Object, e As EventArgs) Handles btnAdicionarClienteUbicacion.Click
        Try
            lblMensajeClienteUbicacion.Text = ""
            fValidarSesion()
            cboPaisMensajeClienteUbicacion.Focus()

            Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
            Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
            UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorIdEquivalencia(hfdIdUbicacionGeograficaCliente.Value)

            ListarPaisClienteUbicacionCombo()
            lblRazonSocialClienteMensajeClienteUbicacion.Text = txtRazonSocial.Text
            hfdIdUbicacionGeograficaClienteUbicacion.Value = hfdIdUbicacionGeograficaCliente.Value
            cboPaisMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdPaisUbicacionGeografica 'Mid(hfdIdUbicacionGeograficaEntregaCliente.Value, 1, 2) 'cboPaisMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdPaisUbicacionGeografica
            cboPaisMensajeClienteUbicacion_SelectedIndexChanged(cboPaisMensajeClienteUbicacion, New System.EventArgs())
            cboDepartamentoMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdDepartamentoUbicacionGeografica 'cboDepartamentoMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
            cboDepartamentoMensajeClienteUbicacion_SelectedIndexChanged(cboDepartamentoMensajeClienteUbicacion, New System.EventArgs())
            cboProvinciaMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdProvinciaUbicacionGeografica 'cboProvinciaMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
            cboProvinciaMensajeClienteUbicacion_SelectedIndexChanged(cboProvinciaMensajeClienteUbicacion, New System.EventArgs())
            cboDistritoMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdDistritoUbicacionGeografica 'cboDistritoMensajeCliente.SelectedValue 'ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
            txtDireccionAdicionalMensajeClienteUbicacion.Text = ""
            lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
        Catch ex As Exception
            lblMensajeClienteUbicacion.Text = ex.Message
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAceptarMensajeClienteUbicacion_Click(sender As Object, e As EventArgs) Handles btnAceptarMensajeClienteUbicacion.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            MyValidator.ErrorMessage = ""
            lblMensajeClienteUbicacion.Text = ""
            hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
            If txtDireccionAdicionalMensajeClienteUbicacion.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar una ubicación, favor de validar la información...!!!")
            End If

            AdicionarClienteUbicacion(hfdIdAuxiliar.Value)

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lblMensajeClienteUbicacion.Text = ex.Message
            lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
        End Try
    End Sub

    Sub AdicionarClienteUbicacion(ByVal strIdCliente As String)
        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

        MyValidator.ErrorMessage = ""
        lblMensajeClienteUbicacion.Text = ""
        hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
        Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA

        Dim DirClienteNeg As New clsClienteNegocios
        Dim dtDirCliente As New DataTable
        dtDirCliente = DirClienteNeg.ClienteGetData("SELECT COUNT(*) AS nCantidad FROM GNRL_CLIENTEUBICACION WHERE LTRIM(RTRIM(cIdCliente)) = '" & strIdCliente & "'")
        DirClienteNeg.ClienteGetData("INSERT INTO GNRL_CLIENTEUBICACION (cIdCliente, cIdUbicacion, vDescripcionClienteUbicacion, bEstadoRegistroClienteUbicacion, " &
                                         "cIdPaisUbicacionGeografica, cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica) " &
                                         "VALUES ('" & hfdIdAuxiliar.Value & "', (SELECT RIGHT ('00000' + CONVERT (VARCHAR(5), (CONVERT (NUMERIC, ISNULL(MAX(cIdUbicacion), 0)) + 1)), 5) FROM GNRL_CLIENTEUBICACION WHERE cIdCliente = '" & hfdIdAuxiliar.Value & "'), " &
                                         "         '" & txtDireccionAdicionalMensajeClienteUbicacion.Text.Trim.ToUpper & "', 1, '" & cboPaisMensajeClienteUbicacion.SelectedValue & "', " &
                                         "         '" & cboDepartamentoMensajeClienteUbicacion.SelectedValue & "', '" & cboProvinciaMensajeClienteUbicacion.SelectedValue & "', '" & cboDistritoMensajeClienteUbicacion.SelectedValue & "')")
        UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue, cboProvinciaMensajeClienteUbicacion.SelectedValue, cboDistritoMensajeClienteUbicacion.SelectedValue)
        hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
        ListarClienteUbicacionCombo()
        cboClienteUbicacion.SelectedIndex = cboClienteUbicacion.Items.Count - 1
        lblTituloMensajeClienteUbicacion.Visible = True
    End Sub

    Private Sub cboTipoActivoEquipoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivoEquipoComponente.SelectedIndexChanged
        ListarSistemaFuncionalEquipoComponenteCombo()
        lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
    End Sub

    Sub txtValorDetalle_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipo.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipo.Rows.Count * (grdDetalleCaracteristicaEquipo.PageIndex))
                Session("CestaCaracteristicaEquipoPrincipal").Rows(FilaActual)("Valor") = txtValorDetalle.Text
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

    Sub txtValordDetalleCaracteristicaMantenimientoCatalogo_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaMantenimientoCatalogo.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaMantenimientoCatalogo.Rows.Count * (grdDetalleCaracteristicaMantenimientoCatalogo.PageIndex))
                Session("CestaCatalogoCaracteristica").Rows(FilaActual)("Valor") = txtValorDetalle.Text
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

    Sub txtValorDetalleEquipoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalleEquipoComponente"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("Valor") = txtValorDetalle.Text
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

    Sub txtValorDetalleCaracteristicaCatalogoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogoComponente.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaCatalogoComponente.Rows.Count * (grdDetalleCaracteristicaCatalogoComponente.PageIndex))
                Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(FilaActual)("Valor") = txtValorDetalle.Text
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

    Sub txtIdReferenciaSAPDetalleEquipoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalleEquipoComponente"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("IdReferenciaSAP") = txtIdReferenciaSAPDetalle.Text
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

    Sub txtDescripcionCampoSAPDetalleEquipoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalleEquipoComponente"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("DescripcionCampoSAP") = txtDescripcionCampoSAPDetalle.Text
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

    Private Sub grdCatalogoComponente_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCatalogoComponente.PageIndexChanging
        grdCatalogoComponente.PageIndex = e.NewPageIndex
        grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
        Me.grdCatalogoComponente.DataBind()
        grdCatalogoComponente.SelectedIndex = -1
    End Sub

    Private Sub btnAdicionarCaracteristicaEquipo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaEquipo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0656", strOpcionModulo, "CMMS")

            If cboCatalogo.SelectedValue = "SELECCIONE DATO" Then
                Throw New Exception("Debe de asignarle un catalogo al equipo.")
            End If

            For i = 0 To Session("CestaCaracteristicaEquipoPrincipal").Rows.Count - 1
                If (Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaEquipo.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCatalogo").ToString.Trim = cboCatalogo.SelectedValue.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                    Throw New Exception("Característica ya registrada, seleccione otro item.")
                    Exit Sub
                End If
            Next
            clsLogiCestaCatalogoCaracteristica.AgregarCesta(cboCatalogo.SelectedValue, "", "'" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'", cboCaracteristicaEquipo.SelectedValue.Trim, UCase(cboCaracteristicaEquipo.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoPrincipal"))
            Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
            Me.grdDetalleCaracteristicaEquipo.DataBind()
            cboCaracteristicaEquipo.SelectedIndex = -1
            grdDetalleCaracteristicaEquipo.SelectedIndex = -1
            MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
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

    Private Sub grdDetalleCaracteristicaEquipo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaEquipo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0657", strOpcionModulo, "CMMS")

            For Each row As GridViewRow In grdDetalleCaracteristicaEquipo.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaCaracteristicaEquipoPrincipal").Rows.Count - 1
                            If (Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then 'And row.RowIndex = i Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoPrincipal"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
            Me.grdDetalleCaracteristicaEquipo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleCaracteristicaEquipoComponente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaEquipoComponente.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0659", strOpcionModulo, "CMMS")

            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        Dim FilaActual As Int16
                        For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                            FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                            If (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim) = hfdIdCatalogoEquipoComponente.Value.Trim And
                                    (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim) = "1" And
                                    (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim And
                                    (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim) = txtIdEquipoComponente.Text.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoComponente"))
                                Exit For
                            End If
                        Next

                        For i = 0 To Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows.Count - 1
                            If (Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
            Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
            lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnVerGaleriaFotosEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnVerGaleriaFotosEquipo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para mostrar su galería de imagenes.")
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

    Private Sub lnkbtnCargarFotoEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnCargarFotoEquipo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                ValidarTextoSubirImagenEquipo(True)
                                txtTituloSubirImagenEquipo.Text = ""
                                txtDescripcionSubirImagenEquipo.Text = ""
                                txtObservacionSubirImagenEquipo.Text = ""
                                pnlCabecera.Visible = True
                                pnlEquipo.Visible = False
                                pnlComponentes.Visible = False
                                ValidationSummary1.ValidationGroup = "vgrpValidarSubirImagenEquipo"
                                lnk_mostrarPanelSubirImagenEquipo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para mostrar su galería de imagenes.")
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

    Private Sub btnAceptarSubirImagenEquipo_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirImagenEquipo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If Not (fupSubirImagenEquipo.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
            Dim ext As String = fupSubirImagenEquipo.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"jpg", "jpeg", "bmp", "png", "gif"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de imagen inválido.")
            Dim Size As Integer = 0
            If Not (Integer.TryParse("160", Size)) Then
                Throw New Exception("El tamaño indicado para la imagen no es válido.")
            End If

            Dim GaleriaEquipo As New LOGI_GALERIAEQUIPO
            GaleriaEquipo.cIdEquipo = grdLista.SelectedRow.Cells(0).Text
            GaleriaEquipo.nIdNumeroItemGaleriaEquipo = 0
            GaleriaEquipo.vTituloGaleriaEquipo = UCase(txtTituloSubirImagenEquipo.Text.Trim)
            GaleriaEquipo.vDescripcionGaleriaEquipo = UCase(txtDescripcionSubirImagenEquipo.Text.Trim)
            GaleriaEquipo.vObservacionGaleriaEquipo = UCase(txtObservacionSubirImagenEquipo.Text.Trim)
            GaleriaEquipo.dFechaTransaccionGaleriaEquipo = Now
            Dim strExtension As String = Mid(fupSubirImagenEquipo.PostedFile.FileName.ToString, InStrRev(fupSubirImagenEquipo.PostedFile.FileName.ToString, ".", -1) + 1, 4)
            GaleriaEquipo.vNombreArchivoGaleriaEquipo = Trim(UCase(GaleriaEquipo.cIdEquipo & "-" & GaleriaEquipo.nIdNumeroItemGaleriaEquipo.ToString) & "." & strExtension)
            GaleriaEquipo.bEstadoRegistroGaleriaEquipo = True

            Dim ColeccionGaleria As New List(Of LOGI_GALERIAEQUIPO)

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

            Dim GaleriaEquipoNeg As New clsGaleriaEquipoNegocios
            If GaleriaEquipoNeg.GaleriaEquipoInserta(GaleriaEquipo) = 0 Then
                'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
                Dim FuncionesNeg As New clsFuncionesNegocios
                FuncionesNeg.GuardarArchivo(fupSubirImagenEquipo.PostedFile, True, "Imagenes\Equipo", Trim(UCase(GaleriaEquipo.cIdEquipo & "-" & GaleriaEquipo.nIdNumeroItemGaleriaEquipo.ToString)), True, "", 1200, 160)
                imgEquipo.ImageUrl = "~/Imagenes/Equipo/" & Trim(UCase(GaleriaEquipo.cIdEquipo & "-" & GaleriaEquipo.nIdNumeroItemGaleriaEquipo.ToString)) & ".jpg"

                Session("Query") = "PA_LOGI_MNT_GALERIAEQUIPO 'SQL_INSERT', '','" & GaleriaEquipo.cIdEquipo & "', " &
                               GaleriaEquipo.nIdNumeroItemGaleriaEquipo & ", '" & GaleriaEquipo.vDescripcionGaleriaEquipo & "', '" & GaleriaEquipo.vObservacionGaleriaEquipo & "', '" &
                               GaleriaEquipo.vTituloGaleriaEquipo & "', '" & GaleriaEquipo.bEstadoRegistroGaleriaEquipo & "', '" &
                               GaleriaEquipo.dFechaTransaccionGaleriaEquipo & "', '" & GaleriaEquipo.cIdEquipo & "'"

                LogAuditoria.vEvento = "INSERTAR GALERIA EQUIPO"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                MyValidator.ErrorMessage = "Transacción registrada con éxito"

                BloquearMantenimiento(True, False, True, False)
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary5.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirImagenEquipo_ModalPopupExtender.Show()
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
                e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Left 'Codigo
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'RucCliente
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'RazonSocialCliente
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'FechaRegistroTarjetaSAP
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'FechaUltimaModificacion
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'IdTipoActivo
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'Descripcion
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'Estado Equipo
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'Estado Registro
            Next
        End If
    End Sub

    Private Sub grdCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCatalogoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdCatalogoComponente, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'IdCatalogo
            e.Row.Cells(1).Visible = True 'Descripcion
            e.Row.Cells(2).Visible = False 'IdTipoActivo
            e.Row.Cells(3).Visible = False 'IdSistemaFuncional
            e.Row.Cells(4).Visible = True 'DescripcionAbreviada
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'IdCatalogo
            e.Row.Cells(1).Visible = True 'Descripcion
            e.Row.Cells(2).Visible = False 'IdTipoActivo
            e.Row.Cells(3).Visible = False 'IdSistemaFuncional
            e.Row.Cells(4).Visible = True 'DescripcionAbreviada
        End If
    End Sub

    Private Sub grdCatalogoComponente_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdCatalogoComponente.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'Descripcion
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Center 'IdTipoActivo
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'IdSistemaFuncional
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Left 'DescripcionAbreviada
            Next
        End If
    End Sub

    Private Sub grdEquipoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdEquipoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
    End Sub

    Private Sub grdEquipoComponente_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdEquipoComponente.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'Descripcion
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'IdTipoActivo
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdSistemaFuncional
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'DescripcionAbreviada
            Next
        End If
    End Sub

    Private Sub lnkbtnVerOrdenFabricacion_Click(sender As Object, e As EventArgs) Handles lnkbtnVerOrdenFabricacion.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para mostrar su orden de fabricación.")
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

    Private Sub lnkbtnImprimirTarjetaEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnImprimirTarjetaEquipo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
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

    Private Sub grdDetalleCaracteristicaCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaCatalogoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub grdDetalleCaracteristicaEquipo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaEquipo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub grdDetalleCaracteristicaEquipoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaEquipoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub grdDetalleCaracteristicaMantenimientoCatalogo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaMantenimientoCatalogo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub lnkbtnEliminarEquipoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEliminarEquipoComponente.Click
        Try
            If grdEquipoComponente IsNot Nothing Then
                If grdEquipoComponente.Rows.Count > 0 Then
                    If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                        If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                            hfdTipoOperacionMensajeDocumentoValidacion.Value = "1" 'Eliminación
                            lblDescripcionMensajeDocumentoValidacion.Text = "¿Seguro desea eliminar este componente asignado?"
                            lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender.Show()
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

    Private Sub btnSiMensajeDocumentoValidacion_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumentoValidacion.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            If hfdTipoOperacionMensajeDocumentoValidacion.Value = "1" Then 'Eliminación
                Dim intIndexEquipoComponente = grdEquipoComponente.SelectedRow.RowIndex + ((grdEquipoComponente.PageSize) * (grdEquipoComponente.PageIndex))
                EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")

                If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                    Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
                Else
                    clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                End If

                If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                    clsLogiCestaCatalogo.AgregarCesta(Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("IdCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("IdTipoActivo").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("IdJerarquia").ToString.Trim, Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("IdSistemaFuncional").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("IdEnlaceCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("Descripcion").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("DescripcionAbreviada").ToString.Trim, Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("Estado").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("VidaUtil").ToString.Trim, "",
                                                  "", Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("DescAbrevTipoActivo").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("DescAbrevSistemaFuncional").ToString.Trim, Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("PeriodoGarantia").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(intIndexEquipoComponente)("PeriodoMinimo").ToString.Trim,
                                               Session("CestaCatalogoComponente"))

                    clsLogiCestaEquipo.QuitarCesta(intIndexEquipoComponente, Session("CestaEquipoComponente"))
                End If

                rowIndexDetalle = 0
                CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

                grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
                grdEquipoComponente.DataBind()

                grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
                grdCatalogoComponente.DataBind()

                If MyValidator.ErrorMessage = "" Then
                    MyValidator.ErrorMessage = "Transacción eliminada con éxito"
                End If
            ElseIf hfdTipoOperacionMensajeDocumentoValidacion.Value = "2" Then 'Cambiar Catalogo Principal
                EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdEnlaceEquipo = '" & txtIdEquipo.Text & "' AND cIdJerarquiaCatalogo = '1'")

                If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                    Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
                Else
                    clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                End If

                'JMUG: INICIO: 14/12/2023
                rowIndexDetalle = 0
                CargarCestaCatalogoComponente()
                CargarCestaEquipoComponente()
                CargarCestaCaracteristicaEquipoPrincipal()
                CargarCestaCaracteristicaEquipoComponente()
                CargarCestaCaracteristicaEquipoComponenteTemporal()
                Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
                'JMUG: FINAL: 14/12/2023

                grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
                grdEquipoComponente.DataBind()

                grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
                grdCatalogoComponente.DataBind()

                If MyValidator.ErrorMessage = "" Then
                    MyValidator.ErrorMessage = "Transacción procesada con éxito"
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

    Private Sub grdDetalleCaracteristicaMantenimientoCatalogo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaMantenimientoCatalogo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            For Each row As GridViewRow In grdDetalleCaracteristicaMantenimientoCatalogo.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
                            If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCatalogoCaracteristica"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
            lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
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
        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
    End Sub

    Private Sub frmLogiGenerarEquipo_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            'Función para validar si tiene permisos
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If Request.QueryString("IdEquipo") <> "" Then
                txtBuscarEquipo.Text = Request.QueryString("IdEquipo")
                cboFiltroEquipo.SelectedIndex = 0
                'Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                'Me.grdLista.DataBind()
                If (Session("IdContratoUsuario") = "*") Then
                    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                    Me.grdLista.DataBind()
                ElseIf (Session("IdContratoUsuario") <> Nothing) Then
                    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusquedaV2(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0", Session("IdContratoUsuario"))
                    Me.grdLista.DataBind()
                End If

                grdLista.SelectedIndex = 0
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

    Private Sub grdDetalleCaracteristicaEquipo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleCaracteristicaEquipo.PageIndexChanging
        grdDetalleCaracteristicaEquipo.PageIndex = e.NewPageIndex
        Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
        Me.grdDetalleCaracteristicaEquipo.DataBind() 'Recargo el grid.
        grdDetalleCaracteristicaEquipo.SelectedIndex = -1

    End Sub

    Private Sub cboDescripcionCatalogoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDescripcionCatalogoComponente.SelectedIndexChanged
        txtDescripcionCatalogoComponente.Text = cboDescripcionCatalogoComponente.SelectedItem.Value
        If cboDescripcionCatalogoComponente.SelectedItem.Value = "" Then
            txtDescripcionCatalogoComponente.Text = ""
        End If
        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
    End Sub

    Sub ConcatenadorDescripcionEquipo()
        txtDescripcionEquipo.Text = IIf(cboTipoEquipo.SelectedItem.Text.Trim = "SELECCIONE DATO", "", cboTipoEquipo.SelectedItem.Text.Trim + " | ") + UCase(txtCapacidadEquipo.Text.Trim) + " | " + UCase(txtNroSerieEquipo.Text.Trim) + " | " + UCase(txtTagEquipo.Text.Trim)
        txtDescripcionEquipo.Text = txtDescripcionEquipo.Text.Replace(" |  | ", " | ")
        txtDescripcionEquipo.Text = IIf(InStrRev(txtDescripcionEquipo.Text.Trim, " | ") = Len(txtDescripcionEquipo.Text.Trim), Mid(txtDescripcionEquipo.Text.Trim, 1, Len(txtDescripcionEquipo.Text.Trim) - 1), txtDescripcionEquipo.Text.Trim)
        txtDescripcionEquipo.Text = IIf(InStr(txtDescripcionEquipo.Text.Trim, " | ") = 1, Mid(txtDescripcionEquipo.Text.Trim, 2, Len(txtDescripcionEquipo.Text.Trim) - 1), txtDescripcionEquipo.Text.Trim)
    End Sub

    Private Sub cboTipoEquipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoEquipo.SelectedIndexChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtNroSerieEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtNroSerieEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtNroParteEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtNroParteEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtTagEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtTagEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtCapacidadEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtCapacidadEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub lnkbtnAgregarTodosCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnAgregarTodosCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0674", strOpcionModulo, "CMMS")

            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If

            For i = 0 To Session("CestaCatalogoComponente").Rows.Count - 1
                clsLogiCestaEquipo.AgregarCesta("", Session("CestaCatalogoComponente").Rows(0)("IdCatalogo"),
                                               Session("CestaCatalogoComponente").Rows(0)("IdTipoActivo"),
                                               "1", Session("CestaCatalogoComponente").Rows(0)("IdSistemaFuncional"),
                                               Session("CestaCatalogoComponente").Rows(0)("IdCatalogo"),
                                               Session("CestaCatalogoComponente").Rows(0)("Descripcion"),
                                               Session("CestaCatalogoComponente").Rows(0)("DescripcionAbreviada"), Now,
                                               "1", txtIdEquipo.Text.Trim, "", "1", "", "", "1", "1",
                                               "", "", "01",
                                               Session("CestaEquipoComponente"))
                Dim result As DataRow() = Session("CestaEquipoComponente").Select("IdCatalogo ='" & Session("CestaCatalogoComponente").Rows(0)("IdCatalogo") & "' AND IdTipoActivo = '" & Session("CestaCatalogoComponente").Rows(0)("IdTipoActivo") & "'")
                rowIndexDetalle = Session("CestaEquipoComponente").Rows.IndexOf(result(0))
                CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

                clsLogiCestaCatalogo.QuitarCesta(0, Session("CestaCatalogoComponente"))

                grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
                grdCatalogoComponente.DataBind()

                MyValidator.ErrorMessage = "Componentes asignados satisfactoriamente."

            Next


            grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            grdEquipoComponente.DataBind()

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

    Private Sub grdEquipoComponente_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdEquipoComponente.PageIndexChanging
        grdEquipoComponente.PageIndex = e.NewPageIndex
        grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
        Me.grdEquipoComponente.DataBind()
        grdEquipoComponente.SelectedIndex = -1
    End Sub

    Private Sub btnNoMensajeDocumentoValidacion_Click(sender As Object, e As EventArgs) Handles btnNoMensajeDocumentoValidacion.Click
        If hfdIdCatalogoAnterior.Value = "" Then
            cboCatalogo.SelectedIndex = -1
            Dim script As String = String.Format("var valorAnterior = '{0}';", cboCatalogo.SelectedValue)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AsignarValorAnterior", script, True)
        Else
            cboCatalogo.SelectedValue = hfdIdCatalogoAnterior.Value
            Dim script As String = String.Format("var valorAnterior = '{0}';", cboCatalogo.SelectedValue)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AsignarValorAnterior", script, True)
        End If
        CargarCestaCatalogoComponente()
        CargarCestaEquipoComponente()
        For i = 0 To Session("CestaCaracteristicaEquipoPrincipal").Rows.Count - 1
            clsLogiCestaCatalogoCaracteristica.EditarCesta(cboCatalogo.SelectedValue, Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCatalogo").ToString.Trim, Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdJerarquia").ToString.Trim, Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCaracteristica").ToString.Trim, Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("Descripcion").ToString.Trim, Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdReferenciaSAP").ToString.Trim, Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("DescripcionCampoSAP").ToString.Trim, Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("Valor").ToString.Trim, Session("CestaCaracteristicaEquipoPrincipal"), i)
        Next
        Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
        Me.grdDetalleCaracteristicaEquipo.DataBind()
    End Sub

    Private Sub cboArticuloSAPEquipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboArticuloSAPEquipo.SelectedIndexChanged
        hfdIdArticuloSAPEquipo.Value = cboArticuloSAPEquipo.SelectedValue
    End Sub

    Private Sub lnkbtnSubirDocumentacionEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnSubirDocumentacionEquipo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                ValidarTextoSubirDocumentacionEquipo(True)
                                txtTituloSubirDocumentacionEquipo.Text = ""
                                txtDescripcionSubirDocumentacionEquipo.Text = ""
                                'txtNombreArchivoSubirDocumentacionEquipo.Text = ""
                                pnlCabecera.Visible = True
                                pnlEquipo.Visible = False
                                pnlComponentes.Visible = False
                                ValidationSummary1.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"

                                'Configurar la URL del iframe
                                grdListaVerDocumentosEquipo.DataSource = EquipoNeg.EquipoGetData("SELECT vTituloDocumentacionEquipo AS Titulo, vDescripcionDocumentacionEquipo AS Descripcion, CONVERT(VARCHAR(20),dFechaCarga,103)+' '+CONVERT(VARCHAR(20),dFechaCarga,108) AS Carga, vNombreArchivoDocumentacionEquipo AS NombreArchivo, cIdEquipo AS IdEquipo, nIdNumeroItemDocumentacionEquipo AS Item " &
                                                                                             "FROM LOGI_DOCUMENTACIONEQUIPO " &
                                                                                             "WHERE bEstadoRegistroDocumentacionEquipo=1 and cIdEquipo = '" & txtIdEquipo.Text & "'")
                                grdListaVerDocumentosEquipo.DataBind()
                                divDetalleVerDocumentosEquipo.Visible = False
                                lnk_mostrarPanelSubirDocumentacionEquipo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para agregar documentación.")
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

    Private Sub btnAceptarSubirDocumentacionEquipo_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirDocumentacionEquipo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If Not (fupSubirDocumentacionEquipo.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
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

            Dim ext As String = fupSubirDocumentacionEquipo.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            EquipoNeg.EquipoGetData("INSERT INTO LOGI_DOCUMENTACIONEQUIPO (nIdNumeroItemDocumentacionEquipo, cIdEquipo, vTituloDocumentacionEquipo, vDescripcionDocumentacionEquipo, bEstadoRegistroDocumentacionEquipo, vNombreArchivoDocumentacionEquipo, dFechaCarga) " &
                                    "VALUES ((SELECT ISNULL(MAX(nIdNumeroItemDocumentacionEquipo), 0 ) + 1 FROM LOGI_DOCUMENTACIONEQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "' ), '" & grdLista.SelectedRow.Cells(0).Text & "', '" & UCase(Trim(txtTituloSubirDocumentacionEquipo.Text)) & "', '" & UCase(Trim(txtDescripcionSubirDocumentacionEquipo.Text)) & "', 1, '', GETDATE())")

            Dim Item As Int16 = EquipoNeg.EquipoGetData("SELECT MAX(nIdNumeroItemDocumentacionEquipo) FROM LOGI_DOCUMENTACIONEQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'").Rows(0).Item(0)
            EquipoNeg.EquipoGetData("UPDATE LOGI_DOCUMENTACIONEQUIPO SET vNombreArchivoDocumentacionEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "-" & Item.ToString.Trim & "." & ext & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "' AND nIdNumeroItemDocumentacionEquipo = '" & Item.ToString.Trim & "'")

            Dim FuncionesNeg As New clsFuncionesNegocios
            FuncionesNeg.GuardarArchivoGeneral(fupSubirDocumentacionEquipo.PostedFile, "Downloads\Equipo", Trim(UCase(grdLista.SelectedRow.Cells(0).Text & "-" & Item.ToString)))

            divDetalleVerDocumentosEquipo.Visible = False
            MyValidator.ErrorMessage = "Documentación registrada con éxito"
            BloquearMantenimiento(True, False, True, False)
            ValidationSummary6.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary6.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirDocumentacionEquipo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAgregarVerDocumentosEquipo_Click(sender As Object, e As EventArgs) Handles btnAgregarVerDocumentosEquipo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                ValidarTextoSubirDocumentacionEquipo(True)
                                txtTituloSubirDocumentacionEquipo.Text = ""
                                txtDescripcionSubirDocumentacionEquipo.Text = ""
                                pnlCabecera.Visible = True
                                pnlEquipo.Visible = False
                                pnlComponentes.Visible = False
                                ValidationSummary1.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"

                                divDetalleVerDocumentosEquipo.Visible = True
                                lnk_mostrarPanelSubirDocumentacionEquipo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para agregar documentación.")
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

    Protected Sub btnCancelarMensajeClienteUbicacion_Click(sender As Object, e As EventArgs) Handles btnCancelarMensajeClienteUbicacion.Click

    End Sub

    Protected Sub cboContratoReferencia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboContratoReferencia.SelectedIndexChanged
        Dim value = sender 'As LOGI_CABECERACONTRATO
    End Sub

#Region "Model Buscar Cliente"
    Private Sub imgbtnBuscarCliente_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCliente.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            Me.grdListaCliente.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltroCliente.SelectedValue,
                                                                            IIf(txtBuscarCliente.Text.Trim = "", "***", txtBuscarCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            Me.grdListaCliente.DataBind()
            lnk_mostrarPanelCliente_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub txtBuscarCliente_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscarCliente.TextChanged
        ValidarTexto(False)
        Me.grdListaCliente.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltroCliente.SelectedValue,
                                                                        IIf(txtBuscarCliente.Text.Trim = "", "***", txtBuscarCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
        Me.grdListaCliente.DataBind()
        Me.lnk_mostrarPanelCliente_ModalPopupExtender.Show()
    End Sub

    Private Sub grdListaCliente_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdListaCliente.PageIndexChanging
        grdListaCliente.PageIndex = e.NewPageIndex
        Me.grdListaCliente.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltroCliente.SelectedValue,
                                                                        IIf(txtBuscarCliente.Text.Trim = "", "***", txtBuscarCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
        grdListaCliente.DataBind()
        grdListaCliente.SelectedIndex = -1
        lnk_mostrarPanelCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub grdListaCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdListaCliente.SelectedIndexChanged
        lblMensajeCliente.Text = ""
        Me.lnk_mostrarPanelCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnAceptarCliente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarCliente.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""

            If IsNothing(grdListaCliente.SelectedRow) = False Then
                If IsReference(grdListaCliente.SelectedRow.Cells(1).Text) = True Then
                    Dim Cliente As New GNRL_CLIENTE
                    Cliente = ClienteNeg.ClienteListarPorId(grdListaCliente.SelectedRow.Cells(1).Text, Session("IdEmpresa"), Session("IdPuntoVenta"))
                    If (Trim(Cliente.vDniCliente) = "" And Trim(Cliente.vRucCliente) = "") Then
                        Throw New Exception("Debe de ingresar el número de documento identidad o ruc, favor de validar la información...!!!")
                    ElseIf Cliente.cIdTipoPersona = "J" And Trim(Cliente.vRucCliente) = "" Then
                        Throw New Exception("Debe de ingresar el número de ruc, favor de validar la información...!!!")
                        'ElseIf Trim(Cliente.vEmailCliente) = "" Or FuncionesNeg.IsValidarEmail(Trim(Cliente.vEmailCliente)) = False Then
                        '    Throw New Exception("Debe de ingresar un correo válido, favor de validar la información...!!!")
                        'ElseIf Cliente.vDireccionCliente.Trim = "" Then
                        '    Throw New Exception("Debe de ingresar dirección fiscal, favor de validar la información...!!!")
                    End If

                    hfdIdAuxiliar.Value = grdListaCliente.SelectedRow.Cells(1).Text
                    txtIdCliente.Text = IIf(Cliente.cIdTipoDocumento <> "04", Cliente.vDniCliente, Cliente.vRucCliente)

                    txtRazonSocial.Text = Cliente.vRazonSocialCliente
                    hfdNroDocumentoCliente.Value = IIf(Cliente.cIdTipoDocumento = "04", Cliente.vRucCliente, IIf(Trim(Cliente.vDniCliente) = "", Trim(Cliente.vRucCliente), Trim(Cliente.vDniCliente)))
                    hfdDireccionFiscalCliente.Value = Cliente.vDireccionCliente
                    hfdTelefonoContactoCliente.Value = Cliente.vTelefonoCliente
                    hfdIdTipoPersonaCliente.Value = Cliente.cIdTipoPersona
                    hfdIdTipoCliente.Value = Cliente.cIdTipoCliente

                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(Cliente.cIdPaisUbicacionGeografica, Cliente.cIdDepartamentoUbicacionGeografica, Cliente.cIdProvinciaUbicacionGeografica, Cliente.cIdDistritoUbicacionGeografica)
                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    hfdIdTipoDocumentoCliente.Value = Cliente.cIdTipoDocumento
                End If
            Else
                lnk_mostrarPanelCliente_ModalPopupExtender.Show()
                Throw New Exception("Seleccione un item de la lista")
            End If
            If txtIdCliente.Text.Trim <> "" Then
                ValidarTexto(True)
            End If
        Catch ex As Exception
            lblMensajeCliente.Text = ex.Message
            lnk_mostrarPanelCliente_ModalPopupExtender.Show()
        End Try
    End Sub

#End Region


End Class