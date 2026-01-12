Imports CapaNegocioCMMS
Imports CapaDatosCMMS
Imports System.Globalization
Imports CrystalDecisions.ReportAppServer.CommonControls

Public Class frmLogiGenerarContrato_v3
    Inherits System.Web.UI.Page
    Dim ContratoNeg As New clsContratoNegocios
    Dim ClienteNeg As New clsClienteNegocios
    Dim EquipoNeg As New clsEquipoNegocios
    Dim DetalleContratoNeg As New clsDetalleContratoNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Dim MyValidator As New CustomValidator
    Shared intIndexContratoEquipo As Integer
    Shared stringCodigoOt As String
    Shared stringCodigoCN As String
    Shared stringCodigoEquipo As String
    Dim OrdenFabricacionNeg As New clsOrdenFabricacionNegocios

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
        For i = 1 To NroDiasMes
            dt.Columns.Add(New DataColumn("D" & i, GetType(System.String))) '12
        Next
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
                If NombreColumna <> "" Then
                    Tabla.Rows(Fila)(NombreColumna) = IdTipoMantenimiento
                End If
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaEquipo(ByVal Seleccionar As Boolean, ByVal Item As String, ByVal CodigoEquipo As String, ByVal DescripcionEquipo As String,
                                   ByVal Estado As Boolean, ByVal NombreColumna As String, ByVal IdTipoMantenimiento As String,
                                   ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Seleccionar") = Seleccionar
        Fila("Item") = Item
        Fila("IdEquipo") = CodigoEquipo
        Fila("DescripcionEquipo") = DescripcionEquipo
        Fila("Estado") = Estado
        Dim Columnas As String() = NombreColumna.Split("|")
        Dim DatosColumnas As String() = IdTipoMantenimiento.Split("*")
        For i = 0 To Columnas.Length - 1
            If DatosColumnas(i) <> "" Then
                Fila(Columnas(i)) = DatosColumnas(i)
            End If
        Next
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
                AgregarCestaEquipo(False, ListaEquipos("nIdNumeroItemDetalleContrato"), ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), "", "", Session("CestaEquipoContratoImprimir"))
            Next

            Me.grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            Me.grdDetalleEquipoImprimirProgramacion.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaEquipos()
        'Carga las opciones en la Grilla de todos los Equipos asignados a este contrato.
        Try
            VaciarCestaEquipo(Session("CestaEquipoContrato"))
            VaciarCestaPersonalOrdenTrabajo(Session("CestaPersonalOrdenTrabajo"))


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

            grdDetalleEquiposFixed_Load(grdDetalleEquiposFixed, New System.EventArgs)
            grdDetalleEquiposScrollable_Load(grdDetalleEquiposScrollable, New System.EventArgs)

            For Each ListaEquipos In dsEquipos.Rows
                'JMUG: 07/09/2025
                'Dim query = "SELECT nIdNumeroItemDetalleContrato, cIdEquipoDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato, cIdTipoMantenimientoPlanificacionEquipoContrato, " &
                '                                                  "       cIdPeriodoMesPlanificacionEquipoContrato, vOrdenTrabajoReferenciaPlanificacionEquipoContrato, vOrdenTrabajoClientePlanificacionEquipoContrato,  " &
                '                                                  "       cIdNumeroPlantillaCheckListPlanificacionEquipoContrato " &
                '                                                  "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                '                                                  "WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' " &
                '                                                  "      AND nIdNumeroItemDetalleContrato = '" & ListaEquipos("nIdNumeroItemDetalleContrato") & "' AND cIdEquipoDetalleContrato = '" & ListaEquipos("cIdEquipoDetalleContrato") & "'  " &
                '                                                  "      AND YEAR(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboPeriodo.SelectedValue & " AND MONTH(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboMes.SelectedValue & " " &
                '                                                  "ORDER BY cIdPeriodoMesPlanificacionEquipoContrato, nIdNumeroItemDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato"

                'Dim dsPlanificacion = ContratoNeg.ContratoGetData("SELECT nIdNumeroItemDetalleContrato, cIdEquipoDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato, cIdTipoMantenimientoPlanificacionEquipoContrato, " &
                '                                                  "       cIdPeriodoMesPlanificacionEquipoContrato, vOrdenTrabajoReferenciaPlanificacionEquipoContrato, vOrdenTrabajoClientePlanificacionEquipoContrato,  " &
                '                                                  "       cIdNumeroPlantillaCheckListPlanificacionEquipoContrato " &
                '                                                  "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                '                                                  "WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' " &
                '                                                  "      AND nIdNumeroItemDetalleContrato = '" & ListaEquipos("nIdNumeroItemDetalleContrato") & "' AND cIdEquipoDetalleContrato = '" & ListaEquipos("cIdEquipoDetalleContrato") & "'  " &
                '                                                  "      AND YEAR(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboPeriodo.SelectedValue & " AND MONTH(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboMes.SelectedValue & " " &
                '                                                  "ORDER BY cIdPeriodoMesPlanificacionEquipoContrato, nIdNumeroItemDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato")
                Dim dsPlanificacion = ContratoNeg.ContratoGetData("SELECT PLA.nIdNumeroItemDetalleContrato, PLA.cIdEquipoDetalleContrato, PLA.dFechaHoraProgramacionPlanificacionEquipoContrato, PLA.cIdTipoMantenimientoPlanificacionEquipoContrato, " &
                                                                  "       PLA.cIdPeriodoMesPlanificacionEquipoContrato, PLA.vOrdenTrabajoReferenciaPlanificacionEquipoContrato, PLA.vOrdenTrabajoClientePlanificacionEquipoContrato,  " &
                                                                  "       PLA.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato, OT.bEstadoRegistroCabeceraOrdenTrabajo, OT.cEstadoCabeceraOrdenTrabajo " &
                                                                  "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO AS PLA INNER JOIN LOGI_CABECERAORDENTRABAJO AS OT ON " &
                                                                  "     PLA.cIdEmpresa = OT.cIdEmpresa AND LTRIM(RTRIM(PLA.vOrdenTrabajoReferenciaPlanificacionEquipoContrato)) = LTRIM(RTRIM(OT.cIdTipoDocumentoCabeceraOrdenTrabajo)) + '-' + LTRIM(RTRIM(OT.vIdNumeroSerieCabeceraOrdenTrabajo)) + '-' + LTRIM(RTRIM(OT.vIdNumeroCorrelativoCabeceraOrdenTrabajo)) " &
                                                                  "WHERE PLA.cIdEmpresa = '" & Session("IdEmpresa") & "' AND PLA.cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND PLA.vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND PLA.vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' " &
                                                                  "      AND PLA.nIdNumeroItemDetalleContrato = '" & ListaEquipos("nIdNumeroItemDetalleContrato") & "' AND PLA.cIdEquipoDetalleContrato = '" & ListaEquipos("cIdEquipoDetalleContrato") & "'  " &
                                                                  "      AND YEAR(PLA.dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboPeriodo.SelectedValue & " AND MONTH(PLA.dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboMes.SelectedValue & " " &
                                                                  "      AND OT.bEstadoRegistroCabeceraOrdenTrabajo = 1 " &
                                                                  "ORDER BY PLA.cIdPeriodoMesPlanificacionEquipoContrato, PLA.nIdNumeroItemDetalleContrato, PLA.dFechaHoraProgramacionPlanificacionEquipoContrato")

                Dim strNombreColumna As String = ""
                Dim strDatosColumna As String = ""
                Dim bValidaIngreso As Boolean = False
                'JMUG: 22/07/2025 18:08
                'For i = 1 To 31
                '    If IsDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) Then
                '        For Each ListaPlanificacion In dsPlanificacion.Rows

                '            If CDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) = CDate(ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato")) Then
                '                strNombreColumna = strNombreColumna + ("D" & i) & "|"
                '                strDatosColumna = strDatosColumna + ListaPlanificacion("cIdTipoMantenimientoPlanificacionEquipoContrato") & "|" & txtIdContrato.Text & "|" & ListaPlanificacion("cIdNumeroPlantillaCheckListPlanificacionEquipoContrato") & "|" & ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoClientePlanificacionEquipoContrato") & "*"
                '                bValidaIngreso = True
                '            End If
                '        Next
                '        If bValidaIngreso = False Then
                '            strNombreColumna = strNombreColumna + ("D" & i) & "|"
                '            strDatosColumna = strDatosColumna + "" & "|" & txtIdContrato.Text & "|" & "" & "|" & CDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) & "|" & "" & "|" & "" & "*"
                '        End If
                '    End If
                'Next
                For i = 0 To 30
                    bValidaIngreso = False 'JMUG: 09/09/2025
                    If IsDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) Then
                        For Each ListaPlanificacion In dsPlanificacion.Rows

                            If CDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) = CDate(ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato")) Then
                                strNombreColumna = strNombreColumna + ("D" & (i + 1)) & "|"
                                strDatosColumna = strDatosColumna + ListaPlanificacion("cIdTipoMantenimientoPlanificacionEquipoContrato") & "|" & txtIdContrato.Text & "|" & ListaPlanificacion("cIdNumeroPlantillaCheckListPlanificacionEquipoContrato") & "|" & ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoClientePlanificacionEquipoContrato") & "|" & ListaPlanificacion("cEstadoCabeceraOrdenTrabajo") & "*"
                                bValidaIngreso = True
                            End If
                        Next
                        If bValidaIngreso = False Then
                            strNombreColumna = strNombreColumna + ("D" & (i + 1)) & "|"
                            strDatosColumna = strDatosColumna + "" & "|" & txtIdContrato.Text & "|" & "" & "|" & CDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) & "|" & "" & "|" & "|" & "*"
                        End If
                    Else 'JMUG: 09/09/2025
                        strNombreColumna = strNombreColumna + ("D" & (i + 1)) & "|"
                        strDatosColumna = strDatosColumna + "" & "|" & txtIdContrato.Text & "|" & "" & "|" & "|" & "|" & "|" & "*"
                    End If
                Next
                strNombreColumna = Mid(strNombreColumna, 1, strNombreColumna.Length - 1)
                AgregarCestaEquipo(False, ListaEquipos("nIdNumeroItemDetalleContrato"), ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), strNombreColumna, strDatosColumna, Session("CestaEquipoContrato"))
            Next
            Me.grdDetalleEquiposFixed.DataSource = Session("CestaEquipoContrato")
            Me.grdDetalleEquiposFixed.DataBind()

            Me.grdDetalleEquiposScrollable.DataSource = Session("CestaEquipoContrato")
            Me.grdDetalleEquiposScrollable.DataBind()

            Dim dsPersonal = ContratoNeg.ContratoGetData("SELECT PLAEQUCON.vOrdenTrabajoClientePlanificacionEquipoContrato, ORDTRA.vContratoReferenciaCabeceraOrdenTrabajo, ORDTRA.dFechaInicioPlanificadaCabeceraOrdenTrabajo, RRHH.cIdTipoDocumentoCabeceraOrdenTrabajo, RRHH.vIdNumeroSerieCabeceraOrdenTrabajo, " &
                                                         "       RRHH.vIdNumeroCorrelativoCabeceraOrdenTrabajo, RRHH.cIdEmpresa, RRHH.cIdPersonal, RRHH.vObservacionRecursosOrdenTrabajo, RRHH.bEstadoRegistroRecursosOrdenTrabajo, RRHH.cIdEquipoCabeceraOrdenTrabajo, " &
                                                         "       RRHH.nTotalMinutosTrabajadosRecursosOrdenTrabajo, RRHH.vIdArticuloSAPCabeceraOrdenTrabajo, RRHH.bResponsableRecursosOrdenTrabajo, " &
                                                         "       PER.vApellidoPaternoPersonal + ' ' + PER.vApellidoPaternoPersonal + ', ' + PER.vNombresPersonal AS vNombreCompletoPersonal " &
                                                         "FROM LOGI_RECURSOSORDENTRABAJO AS RRHH INNER JOIN LOGI_CABECERAORDENTRABAJO AS ORDTRA ON " &
                                                         "     RRHH.cIdEmpresa = ORDTRA.cIdEmpresa AND " &
                                                         "     RRHH.cIdTipoDocumentoCabeceraOrdenTrabajo = ORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo AND " &
                                                         "     RRHH.vIdNumeroSerieCabeceraOrdenTrabajo = ORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo AND " &
                                                         "     RRHH.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                                                         "     INNER JOIN RRHH_PERSONAL AS PER ON" &
                                                         "     RRHH.cIdEmpresa = PER.cIdEmpresa AND " &
                                                         "     RRHH.cIdPersonal = PER.cIdPersonal " &
                                                         "     INNER JOIN LOGI_PLANIFICACIONEQUIPOCONTRATO AS PLAEQUCON ON " &
                                                         "     ORDTRA.cIdEmpresa = PLAEQUCON.cIdEmpresa AND ORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + ORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + ORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = PLAEQUCON.vOrdenTrabajoReferenciaPlanificacionEquipoContrato " &
                                                         "WHERE ORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                         "      AND ORDTRA.vContratoReferenciaCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "-" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "-" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "'")
            For Each ListaPersonal In dsPersonal.Rows
                AgregarCestaPersonalOrdenTrabajo(ListaPersonal("dFechaInicioPlanificadaCabeceraOrdenTrabajo"), ListaPersonal("cIdEquipoCabeceraOrdenTrabajo"), txtIdContrato.Text, ListaPersonal("bResponsableRecursosOrdenTrabajo"), ListaPersonal("cIdPersonal").ToString.Trim, ListaPersonal("vNombreCompletoPersonal").ToString.Trim, ListaPersonal("cIdTipoDocumentoCabeceraOrdenTrabajo") + "-" + ListaPersonal("vIdNumeroSerieCabeceraOrdenTrabajo") + "-" + ListaPersonal("vIdNumeroCorrelativoCabeceraOrdenTrabajo"), ListaPersonal("vOrdenTrabajoClientePlanificacionEquipoContrato"), Session("CestaPersonalOrdenTrabajo"))
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

    Sub CargarCestaPersonalFiltrado()
        'Carga las opciones que aun no estan asignadas en la Grilla de Opciones del Módulo.
        Try
            VaciarCestaPersonal(Session("CestaSSPersonal"))
            Dim EquipoActivoNeg As New clsEquipoNegocios

            If Session("CestaPersonalOrdenTrabajo").Rows.Count > 0 Then
                Dim resultPersonalSimple As DataRow() = Session("CestaPersonalOrdenTrabajo").Select("IdEquipo = '" & hfdIdEquipo.Value.Trim & "' AND FechaPlanificacion = '" & IIf(hfdOperacionDetalle.Value = "N", "", txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) & "'")
                If resultPersonalSimple.Length = 0 Then
                    Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Nothing
                Else
                    Dim rowFil As DataRow() = resultPersonalSimple
                    For Each fila As DataRow In rowFil
                        AgregarCestaPersonal(fila("IdPersonal"), UCase(fila("Personal")), fila("Responsable"), Session("CestaSSPersonal"))
                    Next
                    Exit Sub
                End If
            End If
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
        cboFiltroContrato.DataTextField = "vDescripcionTablaSistema"
        cboFiltroContrato.DataValueField = "vValor"
        cboFiltroContrato.DataSource = FiltroNeg.TablaSistemaListarCombo("88", "LOGI", Session("IdEmpresa"))
        cboFiltroContrato.Items.Clear()
        cboFiltroContrato.DataBind()
    End Sub

    Sub ListarPeriodoCombo(ByVal fechaIni As Date, ByVal fechaFin As Date)
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        cboPeriodo.DataTextField = "Anio"
        cboPeriodo.DataValueField = "Anio"
        Dim lista = ContratoNeg.ComboPeriodoVigencia(fechaIni, fechaFin)
        cboPeriodo.DataSource = lista
        cboPeriodo.Items.Clear()
        If (lista.Any(Function(item) item.Anio = DateTime.Now.Year)) Then
            cboPeriodo.SelectedValue = DateTime.Now.Year
        End If
        cboPeriodo.DataBind()
    End Sub

    Sub ListarMesPeriodoCombo(ByVal fechaIni As Date, ByVal fechaFin As Date)
        cboMes.DataTextField = "NombreMes"
        cboMes.DataValueField = "Months"
        Dim lista = ContratoNeg.ContratoPeriodoMes(fechaIni, fechaFin, Integer.Parse(cboPeriodo.SelectedValue))
        cboMes.DataSource = lista
        cboMes.Items.Clear()
        If (lista.Any(Function(item) item.Years = DateTime.Now.Year And item.Months = DateTime.Now.Month)) Then
            cboMes.SelectedValue = DateTime.Now.Month
        End If
        cboMes.DataBind()
    End Sub

    Sub ListarPersonalAsignadoCombo(ByVal Contrato As LOGI_CABECERACONTRATO)

    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdContrato.Enabled = False 'bActivar
        txtDescripcionContrato.Enabled = bActivar
        txtNroLicitacionContrato.Enabled = bActivar
        txtFechaEmisionContrato.Enabled = bActivar
        txtFechaVigenciaInicio.Enabled = bActivar
        txtFechaVigenciaFinal.Enabled = bActivar
        txtIdCliente.Enabled = bActivar
        txtRazonSocial.Enabled = False
    End Sub

    Sub LlenarData()
        Dim Contrato As LOGI_CABECERACONTRATO = ContratoNeg.ContratoListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Session("IdEmpresa"))
        txtIdContrato.Text = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato.Trim + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato.Trim
        txtDescripcionContrato.Text = Contrato.vDescripcionCabeceraContrato
        txtNroLicitacionContrato.Text = Contrato.vNroLicitacionCabeceraContrato
        txtFechaEmisionContrato.Text = Contrato.dFechaEmisionCabeceraContrato
        txtFechaVigenciaInicio.Text = Contrato.dFechaVigenciaInicialCabeceraContrato
        txtFechaVigenciaFinal.Text = Contrato.dFechaVigenciaFinalCabeceraContrato

        ListarPeriodoCombo(Contrato.dFechaVigenciaInicialCabeceraContrato, Contrato.dFechaVigenciaFinalCabeceraContrato)
        ListarMesPeriodoCombo(Contrato.dFechaVigenciaInicialCabeceraContrato, Contrato.dFechaVigenciaFinalCabeceraContrato)

        'cboPeriodo.SelectedValue = Year(IIf(IsNothing(Contrato.dFechaVigenciaFinalCabeceraContrato) Or Contrato.dFechaVigenciaFinalCabeceraContrato > Now, Now, Contrato.dFechaVigenciaFinalCabeceraContrato))
        'cboMes.SelectedValue = Month(IIf(IsNothing(Contrato.dFechaVigenciaFinalCabeceraContrato) Or Contrato.dFechaVigenciaFinalCabeceraContrato > Now, Now, Contrato.dFechaVigenciaFinalCabeceraContrato))

        Dim ClienteNeg As New clsClienteNegocios
        Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorId(Contrato.cIdCliente, Session("IdEmpresa"), "*")
        txtIdCliente.Text = Cliente.vRucCliente
        btnAdicionarCliente_Click(btnAdicionarCliente, New System.EventArgs())
        CargarCestaEquipos()

        hfdFechaCreacionContrato.Value = IIf(IsNothing(Contrato.dFechaCreacionCabeceraContrato), Now, Contrato.dFechaCreacionCabeceraContrato)
        hfdIdUsuarioCreacionContrato.Value = IIf(IsNothing(Contrato.cIdUsuarioCreacionCabeceraContrato), Session("IdUsuario"), Contrato.cIdUsuarioCreacionCabeceraContrato)
        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
    End Sub

    Sub LlenarDataEquipo(ByVal ValoresReferenciales() As String)
        'JMUG: 09/09/2025 Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(ValoresReferenciales(7).ToString)
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(ValoresReferenciales(8).ToString)
        hfdIdEquipo.Value = Equipo.cIdEquipo
        hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
        hfdIdCatalogoEquipo.Value = Equipo.cIdCatalogo
        hfdIdTipoActivoEquipo.Value = Equipo.cIdTipoActivo
        hfdJerarquiaEquipo.Value = Equipo.cIdJerarquiaCatalogo

        'txtDescripcionContrato.Text = Equipo.vDescripcionEquipo
        Dim ClienteNeg As New clsClienteNegocios
        Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorIdSAP(Equipo.vIdClienteSAPEquipo, Session("IdEmpresa"))
    End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvDescripcionContrato.EnableClientScript = bValidar
        Me.rfvFechaEmisionContrato.EnableClientScript = bValidar
        Me.rfvFechaVigenciaInicio.EnableClientScript = bValidar
        Me.rfvFechaVigenciaFinal.EnableClientScript = bValidar
        Me.rfvIdCliente.EnableClientScript = bValidar
        Me.rfvRazonSocial.EnableClientScript = bValidar
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdContrato.Text = ""
        txtDescripcionContrato.Text = ""
        txtNroLicitacionContrato.Text = ""
        txtFechaEmisionContrato.Text = String.Format("{0:dd/MM/yyyy}", Now)
        txtFechaVigenciaInicio.Text = String.Format("{0:dd/MM/yyyy}", Now)
        txtFechaVigenciaFinal.Text = String.Format("{0:dd/MM/yyyy}", Now)
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
        hfdFechaCreacionContrato.Value = ""
        hfdIdUsuarioCreacionContrato.Value = ""

        VaciarCestaEquipo(Session("CestaEquipoContrato"))
        VaciarCestaPersonal(Session("CestaSSPersonal"))
        VaciarCestaPersonalOrdenTrabajo(Session("CestaPersonalOrdenTrabajo"))
    End Sub

    Sub LimpiarObjetosOrdenTrabajo()

        Dim fechaTexto As String = "01/" & Format(CInt(cboMes.SelectedValue), "00") & "/" & cboPeriodo.SelectedValue
        Dim fechaCorrecta As Date = DateTime.ParseExact(fechaTexto, "dd/MM/yyyy", CultureInfo.InvariantCulture)

        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = fechaCorrecta '.ToString("dd/MM/yyyy")
        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = fechaCorrecta '.ToString("dd/MM/yyyy")
        lblMensajeMantenimientoOrdenTrabajo.Text = ""

        'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = "1/" & Format(CInt(cboMes.SelectedValue), "00") & "/" & cboPeriodo.SelectedValue
        'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = "1/" & Format(CInt(cboMes.SelectedValue), "00") & "/" & cboPeriodo.SelectedValue

        cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboTipoControlTiempoMantenimientoOrdenTrabajo.SelectedIndex = -1
        txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text = ""
        chkReajustarProgramacion.Checked = False
        cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
        VaciarCestaPersonal(Session("CestaSSPersonal"))
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ValidarTextoOrdenTrabajo(ByVal bValidar As Boolean)
        Me.rfvTipoMantenimientoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalResponsableMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvTipoControlTiempoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalAsignadoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
    End Sub

    Sub ActivarObjetosOrdenTrabajo(ByVal bActivar As Boolean)
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Enabled = bActivar
        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Enabled = bActivar
        cboTipoMantenimientoMantenimientoOrdenTrabajo.Enabled = bActivar
        cboListadoCheckListMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalResponsableMantenimientoOrdenTrabajo.Enabled = bActivar
        cboTipoControlTiempoMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Enabled = bActivar
    End Sub

    Sub LlenarDataOrdenTrabajo(ByVal ValoresReferenciales() As String, isOperacion As String)
        Try
            'stringCodigoOt = Valores(4)
            Dim OrdTraNeg As New clsOrdenTrabajoNegocios
            Dim OrdenTrabajo As LOGI_CABECERAORDENTRABAJO
            If hfdOperacionDetalle.Value = "N" Then
                hfdIdOrdenTrabajo.Value = ""
                hfdIdOrdenTrabajoCliente.Value = ""
                hfdFechaPlanificacion.Value = ""
            Else
                Dim strOT = ValoresReferenciales(4).Split("-")
                If (String.IsNullOrEmpty(strOT(0))) Then

                End If

                OrdenTrabajo = OrdTraNeg.OrdenTrabajoListarPorId(strOT(0), strOT(1), strOT(2), Session("IdEmpresa"))
                txtIdContrato.Text = ValoresReferenciales(1) 'IdContrato

                txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = OrdenTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo
                txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = OrdenTrabajo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo
                txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text = If(OrdenTrabajo.nPeriodicidadDiasCabeceraOrdenTrabajo.HasValue,
                                                                    OrdenTrabajo.nPeriodicidadDiasCabeceraOrdenTrabajo.Value.ToString(),
                                                                    "")

                Dim ComponentesEquipoNeg As New clsComponenteOrdenTrabajoNegocios
                Dim dsComponentes = ComponentesEquipoNeg.ComponenteOrdenTrabajoListaGridPorOrdEquipo(strOT(0), strOT(1), strOT(2),
                                                                                             Session("IdEmpresa"), stringCodigoEquipo, OrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo)
                Dim tieneROF As Boolean = dsComponentes.Any(Function(componente) componente.StatusComponente = "R" OrElse componente.StatusComponente = "F")
                If tieneROF Then
                    txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Enabled = False
                    txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Enabled = False
                    cboTipoMantenimientoMantenimientoOrdenTrabajo.Enabled = False
                    cboListadoCheckListMantenimientoOrdenTrabajo.Enabled = False


                End If

                hfdFechaPlanificacion.Value = ValoresReferenciales(3)
                hfdIdOrdenTrabajo.Value = ValoresReferenciales(4)
                hfdIdOrdenTrabajoCliente.Value = ValoresReferenciales(5)
            End If
            'If txtIdContrato.Text <> "" And hfdIdOrdenTrabajo.Value.Trim.Length <> 0 Then
            '    Dim strOT = hfdIdOrdenTrabajo.Value.Trim.Split("-")
            '    OrdenTrabajo = OrdTraNeg.OrdenTrabajoListarPorId(strOT(0), strOT(1), strOT(2), Session("IdEmpresa"))
            'End If
            LlenarDataEquipo(ValoresReferenciales)

            ListarTipoMantenimientoCombo()

            If hfdOperacionDetalle.Value = "E" Then
                cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue = OrdenTrabajo.cIdTipoMantenimientoCabeceraOrdenTrabajo 'IdTipoMantenimiento
                'cboTipoControlTiempoMantenimientoOrdenTrabajo.SelectedValue = OrdenTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo
                cboTipoControlTiempoMantenimientoOrdenTrabajo.SelectedValue = IIf(String.IsNullOrWhiteSpace(OrdenTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo.ToString()), "C", OrdenTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo)
            End If

            ListarPlantillaCheckListCombo()

            If hfdOperacionDetalle.Value = "E" Then
                cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = OrdenTrabajo.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo 'IdPlantilla Check List
            End If
            ListarPersonalResponsableCombo()

            ListarPersonalCombo(OrdenTrabajo, isOperacion)

            If (hfdOperacionDetalle.Value = "E") Then
                Dim PersonalNeg As New clsOrdenTrabajoNegocios
                Dim result = PersonalNeg.OrdenTrabajoRecursos(OrdenTrabajo)

                Dim responsable = result.Find(Function(item) item.bResponsableRecursosOrdenTrabajo = True And item.bEstadoRegistroRecursosOrdenTrabajo = True)
                If (responsable Is Nothing) Then
                    cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0
                Else
                    cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue = responsable.RRHH_PERSONAL.cIdPersonal
                End If


                VaciarCestaPersonal(Session("CestaSSPersonal"))

                For Each item In result.Where(Function(x) x.bEstadoRegistroRecursosOrdenTrabajo = True)
                    AgregarCestaPersonal(item.RRHH_PERSONAL.cIdPersonal.Trim, UCase(item.RRHH_PERSONAL.vNombreCompletoPersonal).Trim, item.bResponsableRecursosOrdenTrabajo, Session("CestaSSPersonal"))
                Next

            Else
                CargarCestaPersonalFiltrado()
            End If

            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Function fValidarSiHayMovimientosOrdenTrabajo(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String) As Boolean
        fValidarSiHayMovimientosOrdenTrabajo = False
        'JMUG: 07/09/2025
        'Dim dsContrato = ContratoNeg.ContratoGetData("SELECT vOrdenTrabajoReferenciaPlanificacionEquipoContrato " &
        '                                             "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
        '                                             "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "'")
        Dim dsContrato = ContratoNeg.ContratoGetData("SELECT vOrdenTrabajoReferenciaPlanificacionEquipoContrato " &
                                                     "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                                     "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text).ToString("yyyyM") & "'")

        For Each Fila In dsContrato.Rows
            If ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_TAREACHECKLISTORDENTRABAJO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Fila("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "'").Rows(0).Item(0) > 0 Then
                Return True
            ElseIf ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_GALERIACHECKLISTORDENTRABAJO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Fila("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "'").Rows(0).Item(0) > 0 Then
                Return True
            ElseIf ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_OTROSDATOSORDENTRABAJO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Fila("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "'").Rows(0).Item(0) > 0 Then
                Return True
            End If
        Next
    End Function

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

    Sub ListarPersonalCombo(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO, isEditar As String)
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
            Dim booValida As Boolean
            If ClienteRegistrado.Count > 0 Then
                booValida = False
            Else
                booValida = FuncionesNeg.VerificarConexionURL("https://www.bimsic.net/WSSIW/wsSistemaIntegradoWebBIMSIC.asmx")
            End If

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
                    hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                    hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona
                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
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
                                    hfdDireccionFiscalCliente.Value = ""
                                    hfdTelefonoContactoCliente.Value = ""
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
            cboFiltroContrato.SelectedIndex = 4

            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()
            'ListarPeriodoCombo()

            If Session("CestaEquipoContrato") Is Nothing Then
                Session("CestaEquipoContrato") = CrearCestaEquipo(31)
            Else
                VaciarCestaEquipo(Session("CestaEquipoContrato"))
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

            Dim SubFiltro As String = ""

            Dim filtro As String = ""
            If (String.IsNullOrEmpty(Session("IdContratoUsuario"))) Then
                btnNuevo.Visible = False
                btnEditar.Visible = False
                lnkbtnVerContrato.Visible = False
                lnkbtnVerProgramacion.Visible = False
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

            pnlCabecera.Visible = True
            pnlContenido.Visible = False

        Else
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0663", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = False
            pnlContenido.Visible = True
            hfdOperacion.Value = "N"

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ValidarTexto(True) 'BORRAR
            ActivarObjetos(True) 'BORRAR
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
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0665", strOpcionModulo, "CMMS")



            Dim Contrato As New LOGI_CABECERACONTRATO
            Dim strContrato = txtIdContrato.Text.ToString.Split("-")
            If hfdOperacion.Value = "N" Then
                Contrato.cIdTipoDocumentoCabeceraContrato = "CN"
                Contrato.vIdNumeroSerieCabeceraContrato = Year(Now).ToString
                Contrato.vIdNumeroCorrelativoCabeceraContrato = ""
            ElseIf hfdOperacion.Value = "E" Then
                Contrato.cIdTipoDocumentoCabeceraContrato = strContrato(0)
                Contrato.vIdNumeroSerieCabeceraContrato = strContrato(1)
                Contrato.vIdNumeroCorrelativoCabeceraContrato = strContrato(2)
            End If
            Contrato.cIdEmpresa = Session("IdEmpresa")
            Contrato.dFechaTransaccionCabeceraContrato = Now
            Contrato.dFechaEmisionCabeceraContrato = txtFechaEmisionContrato.Text
            Contrato.dFechaVigenciaInicialCabeceraContrato = txtFechaVigenciaInicio.Text
            Contrato.dFechaVigenciaFinalCabeceraContrato = txtFechaVigenciaFinal.Text
            Contrato.cIdCliente = hfdIdAuxiliar.Value
            Contrato.bEstadoRegistroCabeceraContrato = hfdEstado.Value
            Contrato.cEstadoCabeceraContrato = "R" 'Registrado
            Contrato.vDescripcionCabeceraContrato = UCase(txtDescripcionContrato.Text.Trim)
            Contrato.vNroLicitacionCabeceraContrato = UCase(txtNroLicitacionContrato.Text.Trim)
            Contrato.dFechaUltimaModificacionCabeceraContrato = Now
            Contrato.dFechaCreacionCabeceraContrato = Convert.ToDateTime(IIf(hfdFechaCreacionContrato.Value = "", Now, hfdFechaCreacionContrato.Value))
            Contrato.cIdUsuarioUltimaModificacionCabeceraContrato = Session("IdUsuario")
            Contrato.cIdUsuarioCreacionCabeceraContrato = hfdIdUsuarioCreacionContrato.Value

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
                If (ContratoNeg.ContratoInserta(Contrato)) = 0 Then
                    Dim ColeccionDetalleContrato As New List(Of LOGI_DETALLECONTRATO)
                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        Dim DetContrato As New LOGI_DETALLECONTRATO
                        DetContrato.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                        DetContrato.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                        DetContrato.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                        DetContrato.cIdEmpresa = Session("IdEmpresa")
                        DetContrato.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                        DetContrato.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")
                        DetContrato.vDescripcionDetalleContrato = Session("CestaEquipoContrato").rows(i)("DescripcionEquipo")
                        DetContrato.bEstadoRegistroDetalleContrato = Session("CestaEquipoContrato").rows(i)("Estado")
                        ColeccionDetalleContrato.Add(DetContrato)
                    Next

                    Dim ColeccionPlanificacionContrato As New List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO)
                    Dim ColeccionOrdTra As New List(Of LOGI_CABECERAORDENTRABAJO)

                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        For x = 1 To 31
                            If (Session("CestaEquipoContrato").rows(i)("D" & x)).ToString.Trim <> "" Then
                                If IsDate(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)) Then
                                    If Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) <> "" Then
                                        Dim DetPlanificacion As New LOGI_PLANIFICACIONEQUIPOCONTRATO
                                        DetPlanificacion.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                                        DetPlanificacion.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                                        DetPlanificacion.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        DetPlanificacion.cIdEmpresa = Contrato.cIdEmpresa
                                        DetPlanificacion.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                                        DetPlanificacion.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")

                                        'JMUG: 07/09/2025
                                        'DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyMM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
                                        DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
                                        DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato = Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3))
                                        DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) 'Session("CestaEquipoContrato").rows(i)("IdTipoMantenimiento")
                                        DetPlanificacion.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(4) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajo")
                                        DetPlanificacion.vOrdenTrabajoClientePlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(5) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        DetPlanificacion.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(2) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        ColeccionPlanificacionContrato.Add(DetPlanificacion)

                                        Dim OrdTra As New LOGI_CABECERAORDENTRABAJO
                                        OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = "OT"
                                        OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = Year(Now).ToString
                                        OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ""
                                        OrdTra.dFechaTransaccionCabeceraOrdenTrabajo = Now
                                        OrdTra.dFechaEmisionCabeceraOrdenTrabajo = Now
                                        OrdTra.cIdClienteCabeceraOrdenTrabajo = hfdIdAuxiliar.Value 'Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text).Trim
                                        OrdTra.cIdEquipoCabeceraOrdenTrabajo = DetPlanificacion.cIdEquipoDetalleContrato 'Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim
                                        OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato 'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                                        OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
                                        OrdTra.cIdEmpresa = Session("IdEmpresa")
                                        OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
                                        OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato 'cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
                                        OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = "" 'Equipo.vIdArticuloSAPEquipo
                                        OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
                                        OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
                                        OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
                                        OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
                                        OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
                                        OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        OrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo = Session("IdUsuario")
                                        ColeccionOrdTra.Add(OrdTra)
                                    End If
                                End If
                            End If
                        Next
                    Next

                    If ContratoNeg.ContratoInsertaDetalle(Contrato, ColeccionDetalleContrato, ColeccionPlanificacionContrato, ColeccionOrdTra, Session("CestaPersonalOrdenTrabajo")) = 0 Then
                        txtIdContrato.Text = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                        lblDescripcionMensajeDocumento.Text = "Se creó el siguiente número de documento"
                        lblNroDocumentoMensajeDocumento.Visible = True
                        btnSiMensajeDocumento.Visible = False
                        btnNoMensajeDocumento.Visible = False
                        lblNroDocumentoMensajeDocumento.Text = txtIdContrato.Text
                        lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"

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


                        pnlCabecera.Visible = True
                        pnlContenido.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarContrato.Focus()
                    End If
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (ContratoNeg.ContratoEdita(Contrato)) = 0 Then
                    Dim ColeccionDetalleContrato As New List(Of LOGI_DETALLECONTRATO)
                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        Dim DetContrato As New LOGI_DETALLECONTRATO
                        DetContrato.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                        DetContrato.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                        DetContrato.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                        DetContrato.cIdEmpresa = Session("IdEmpresa")
                        DetContrato.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                        DetContrato.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")
                        DetContrato.vDescripcionDetalleContrato = Session("CestaEquipoContrato").rows(i)("DescripcionEquipo")
                        DetContrato.bEstadoRegistroDetalleContrato = Session("CestaEquipoContrato").rows(i)("Estado")
                        ColeccionDetalleContrato.Add(DetContrato)
                    Next

                    Dim ColeccionPlanificacionContrato As New List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO)
                    Dim ColeccionOrdTra As New List(Of LOGI_CABECERAORDENTRABAJO)
                    Dim ColeccionComponente As New List(Of LOGI_COMPONENTEORDENTRABAJO)

                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        For x = 1 To 31
                            If (Session("CestaEquipoContrato").rows(i)("D" & x)).ToString.Trim <> "" Then
                                If IsDate(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)) Then
                                    If Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) <> "" Then
                                        Dim DetPlanificacion As New LOGI_PLANIFICACIONEQUIPOCONTRATO
                                        DetPlanificacion.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                                        DetPlanificacion.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                                        DetPlanificacion.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        DetPlanificacion.cIdEmpresa = Contrato.cIdEmpresa
                                        DetPlanificacion.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                                        DetPlanificacion.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")

                                        'JMUG: 07/09/2025
                                        'DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyMM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
                                        DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
                                        DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato = Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3))
                                        DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) 'Session("CestaEquipoContrato").rows(i)("IdTipoMantenimiento")
                                        DetPlanificacion.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(4) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajo")
                                        DetPlanificacion.vOrdenTrabajoClientePlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(5) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        DetPlanificacion.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(2) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        ColeccionPlanificacionContrato.Add(DetPlanificacion)

                                        Dim OrdTra As New LOGI_CABECERAORDENTRABAJO
                                        Dim strOrdTra = Split((Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(4)), "-")
                                        OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = If(strOrdTra(0) = "", "OT", strOrdTra(0))
                                        OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = If(strOrdTra(0) = "", Year(Now).ToString, strOrdTra(1))
                                        OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = If(strOrdTra(0) = "", "", strOrdTra(2))
                                        OrdTra.dFechaTransaccionCabeceraOrdenTrabajo = Now
                                        OrdTra.dFechaEmisionCabeceraOrdenTrabajo = Now
                                        OrdTra.cIdClienteCabeceraOrdenTrabajo = hfdIdAuxiliar.Value 'Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text).Trim
                                        OrdTra.cIdEquipoCabeceraOrdenTrabajo = DetPlanificacion.cIdEquipoDetalleContrato 'Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim
                                        OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato
                                        OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
                                        OrdTra.cIdEmpresa = Session("IdEmpresa")
                                        OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
                                        OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato 'cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
                                        OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = "" 'Equipo.vIdArticuloSAPEquipo
                                        OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
                                        OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
                                        OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
                                        OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
                                        OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
                                        OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        ColeccionOrdTra.Add(OrdTra)

                                        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
                                        Dim cIdPlantilla = String.Concat(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue.Trim(), cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue)

                                        Dim result = PlantillaCheckListNeg.ChecklistPlantillaInsertaCopiaComponentes(stringCodigoEquipo, cIdPlantilla)

                                        Dim OrdenFabricacionNeg As New clsOrdenFabricacionNegocios
                                        Dim dsComponenteEquipo = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdJerarquiaCatalogo " &
                                                                                 "FROM LOGI_EQUIPO AS EQU " &
                                                                                 "WHERE EQU.cIdEnlaceEquipo = '" & stringCodigoEquipo & "' " &
                                                                                 "      AND EQU.cIdJerarquiaCatalogo = '1' " &
                                                                                 "      AND EQU.bEstadoRegistroEquipo = '1' " &
                                                                                 "      AND EQU.cIdEnlaceCatalogo = '" & hfdIdCatalogoEquipo.Value & "' " &
                                                                                 "ORDER BY EQU.vDescripcionEquipo")

                                        For Each ComponenteEquipo In dsComponenteEquipo.Rows
                                            Dim ComEqu As New LOGI_COMPONENTEORDENTRABAJO
                                            With ComEqu
                                                .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                                                .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                                                .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                                                .cIdEmpresa = OrdTra.cIdEmpresa
                                                .cIdEquipoCabeceraOrdenTrabajo = stringCodigoEquipo
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
                                    End If
                                End If
                            End If
                        Next
                    Next

                    If ContratoNeg.ContratoInsertaDetalle(Contrato, ColeccionDetalleContrato, ColeccionPlanificacionContrato, ColeccionOrdTra, Session("CestaPersonalOrdenTrabajo")) = 0 Then
                        'If ContratoNeg.ContratoInsertaDetallev2(Contrato, ColeccionDetalleContrato, ColeccionPlanificacionContrato, ColeccionOrdTra, Session("CestaPersonalOrdenTrabajo"), ColeccionComponente) = 0 Then
                        txtIdContrato.Text = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                        lblDescripcionMensajeDocumento.Text = "Se modificó el siguiente número de documento"
                        lblNroDocumentoMensajeDocumento.Visible = True
                        btnSiMensajeDocumento.Visible = False
                        btnNoMensajeDocumento.Visible = False
                        lblNroDocumentoMensajeDocumento.Text = txtIdContrato.Text
                        lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
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

                        pnlCabecera.Visible = True
                        pnlContenido.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarContrato.Focus()
                    End If
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidar"
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

    Private Sub lnkbtnAgregarEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnAgregarEquipo.Click
        Try
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0668", strOpcionModulo, "CMMS")
            lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub imgbtnBuscarSeleccionarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarSeleccionarEquipo.Click
        'Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("ISNULL(EQU.bTieneContratoEquipo, '0') = '0' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
        '                                                         txtBuscarContrato.Text, "0")
        Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("ISNULL(EQU.bTieneContratoEquipo, '0') = '0' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
                                                                 txtBuscarSeleccionarEquipo.Text, "0")
        Me.grdListaSeleccionarEquipo.DataBind()
        lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
    End Sub

    Private Sub grdListaSeleccionarEquipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdListaSeleccionarEquipo.SelectedIndexChanged
        MyValidator.ErrorMessage = ""
        lblMensajeSeleccionarEquipo.Text = ""
        lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAceptarSeleccionarEquipo_Click(sender As Object, e As EventArgs) Handles btnAceptarSeleccionarEquipo.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeSeleccionarEquipo.Text = ""
            If grdListaSeleccionarEquipo IsNot Nothing Then
                If grdListaSeleccionarEquipo.Rows.Count > 0 Then
                    If IsNothing(grdListaSeleccionarEquipo.SelectedRow) = False Then
                        If IsReference(grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text) = True Then
                            'Validamos si el equipo está asignado
                            For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                                If (Session("CestaEquipoContrato").Rows(i)("IdEquipo").ToString.Trim) = (Server.HtmlDecode(grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text).Trim) Then
                                    lblMensajeSeleccionarEquipo.Text = "Equipo ya ingresado en este contrato"
                                    Throw New Exception("Equipo ya ingresado en este contrato")
                                End If
                            Next
                            Dim EquipoNeg As New clsEquipoNegocios
                            If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & (Server.HtmlDecode(grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text).Trim) & "'").Rows(0).Item(0) = "0" Then
                                Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                            End If

                            Dim strNombreColumna As String = ""
                            Dim strFechaColumna As String = "|||"
                            'For i = 1 To 31
                            '    If IsDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) Then
                            '        strNombreColumna = strNombreColumna & ("D" & i) & "|"
                            '        strFechaColumna = strFechaColumna + grdDetalleEquiposFixed.Columns(2 + i).HeaderText + "||*|||"
                            '    End If
                            'Next
                            'JMUG: 22/07/2025 16:12
                            For i = 0 To 30
                                If IsDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) Then
                                    strNombreColumna = strNombreColumna & ("D" & (i + 1)) & "|"
                                    strFechaColumna = strFechaColumna + grdDetalleEquiposScrollable.Columns(i).HeaderText + "|||*|||"
                                End If
                            Next
                            strNombreColumna = Mid(strNombreColumna, 1, strNombreColumna.Length - 1)
                            AgregarCestaEquipo(False, "0", grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text, grdListaSeleccionarEquipo.SelectedRow.Cells(4).Text, "1", strNombreColumna, strFechaColumna, Session("CestaEquipoContrato"))
                            lblMensajeSeleccionarEquipo.Text = "Equipo asignado satisfactoriamente"
                            grdDetalleEquiposFixed.DataSource = Session("CestaEquipoContrato")
                            grdDetalleEquiposFixed.DataBind()
                            'JMUG: 22/07/2025 16:30
                            grdDetalleEquiposScrollable.DataSource = Session("CestaEquipoContrato")
                            grdDetalleEquiposScrollable.DataBind()
                            lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
                        End If
                    End If
                Else
                    lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
                End If
            End If
        Catch ex As Exception
            lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
            lblMensajeSeleccionarEquipo.Text = ex.Message
        End Try
    End Sub

    Private Sub grdDetalleEquiposFixed_Load(sender As Object, e As EventArgs) Handles grdDetalleEquiposFixed.Load
        'grdDetalleEquiposFixed.Width = 1000
        'For i = 0 To 30
        '    'grdDetalleEquiposFixed.Columns(3 + i).HeaderText = i + 1
        '    grdDetalleEquiposFixed.Columns(i).HeaderText = i + 1
        'Next
        'If txtFechaVigenciaInicio.Text.Trim <> "" And txtFechaVigenciaFinal.Text.Trim <> "" Then
        '    Dim iDias As Int16 = 0
        '    Dim strFechaInicial As String = ""
        '    Dim strFechaFinal As String = ""
        '    'Dim strFechaInicial As String = txtFechaVigenciaInicio.Text
        '    'Dim strFechaFinal As String = txtFechaVigenciaFinal.Text

        '    'Establecer la Fecha Inicial

        '    If String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Or String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") Then
        '        strFechaInicial = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaVigenciaInicio.Text))
        '    ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) >= Format(cboPeriodo.SelectedValue, "0000") & String.Format(cboMes.SelectedValue, "00") Then
        '        strFechaInicial = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
        '    ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) < String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Then
        '        strFechaInicial = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddDays(-1))
        '    End If
        '    'Establecer la Fecha Final
        '    If String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Or (String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) = Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00")) Then
        '        strFechaFinal = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaVigenciaFinal.Text))
        '    ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) >= Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") Then
        '        'strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.Parse("01/" & Format(CInt(cboMes.SelectedValue) + 1, "00") & "/" & Format(CInt(cboPeriodo.SelectedValue), "0000")).AddDays(-1))
        '        Dim fechaTemp As New DateTime(CInt(cboPeriodo.SelectedValue), CInt(cboMes.SelectedValue), 1)
        '        strFechaFinal = String.Format("{0:yyyyMMdd}", fechaTemp.AddMonths(1).AddDays(-1))

        '    ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) < String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Then
        '        Dim fechaTemp As New DateTime(CInt(cboPeriodo.SelectedValue), CInt(cboMes.SelectedValue), 1)
        '        'strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.Parse("01/" & Format(CInt(cboMes.SelectedValue) + 1, "00") & "/" & Format(CInt(cboPeriodo.SelectedValue), "0000")).AddDays(-1))
        '        strFechaFinal = String.Format("{0:yyyyMMdd}", fechaTemp.AddMonths(1).AddDays(-1))
        '    End If

        '    Dim dFechaInicial, dFechaFinal As Date
        '    'dFechaInicial = DateTime.ParseExact(strFechaInicial, "d/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        '    'dFechaFinal = DateTime.ParseExact(strFechaFinal, "d/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        '    dFechaInicial = DateTime.ParseExact(strFechaInicial, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
        '    dFechaFinal = DateTime.ParseExact(strFechaFinal, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)


        '    'cboPeriodo.SelectedValue = Year(dFechaInicial)
        '    'cboMes.SelectedValue = Month(dFechaInicial)

        '    grdDetalleEquiposFixed.Width = 500 + (DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal) * 70)
        '    For i = 0 To DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal)
        '        grdDetalleEquiposFixed.Columns(3 + i).HeaderText = dFechaInicial.AddDays(i)
        '    Next
        'End If
        'Me.grdDetalleEquiposFixed.DataSource = Session("CestaEquipoContrato")
        'Me.grdDetalleEquiposFixed.DataBind()
    End Sub

    Private Sub grdDetalleEquiposScrollable_Load(sender As Object, e As EventArgs) Handles grdDetalleEquiposScrollable.Load
        grdDetalleEquiposScrollable.Width = 1000
        For i = 0 To 29
            grdDetalleEquiposScrollable.Columns(1 + i).HeaderText = i + 1
        Next
        If txtFechaVigenciaInicio.Text.Trim <> "" And txtFechaVigenciaFinal.Text.Trim <> "" Then
            Dim iDias As Int16 = 0
            Dim strFechaInicial As String = ""
            Dim strFechaFinal As String = ""

            'Establecer la Fecha Inicial
            If String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Or String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") Then
                strFechaInicial = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaVigenciaInicio.Text))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) >= Format(cboPeriodo.SelectedValue, "0000") & String.Format(cboMes.SelectedValue, "00") Then
                strFechaInicial = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) < String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Then
                strFechaInicial = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddDays(-1))
            End If

            'Establecer la Fecha Final
            If String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Or (String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) = Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00")) Then
                strFechaFinal = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaVigenciaFinal.Text))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) >= Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") Then
                Dim fechaTemp As New DateTime(CInt(cboPeriodo.SelectedValue), CInt(cboMes.SelectedValue), 1)
                strFechaFinal = String.Format("{0:yyyyMMdd}", fechaTemp.AddMonths(1).AddDays(-1))

            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) < String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Then
                Dim fechaTemp As New DateTime(CInt(cboPeriodo.SelectedValue), CInt(cboMes.SelectedValue), 1)
                strFechaFinal = String.Format("{0:yyyyMMdd}", fechaTemp.AddMonths(1).AddDays(-1))
            End If

            Dim dFechaInicial, dFechaFinal As Date
            dFechaInicial = DateTime.ParseExact(strFechaInicial, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
            dFechaFinal = DateTime.ParseExact(strFechaFinal, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)

            grdDetalleEquiposScrollable.Width = 500 + (DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal) * 70)
            For i = 0 To DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal)
                'JMUG: 06/09/2025 grdDetalleEquiposScrollable.Columns(i).HeaderText = dFechaInicial.AddDays(i)
                grdDetalleEquiposScrollable.Columns(i).HeaderText = dFechaInicial.AddDays(i)
                'grdDetalleEquiposScrollable.Columns(i).HeaderText = String.Format("{0:dd/MM/yyyy}", dFechaInicial.AddDays(i))
            Next
        End If
        Me.grdDetalleEquiposScrollable.DataSource = Session("CestaEquipoContrato")
        Me.grdDetalleEquiposScrollable.DataBind()
    End Sub

    Private Sub grdDetalleEquiposFixed_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleEquiposFixed.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Mantenimientos
            e.Row.Cells(1).Visible = True 'IdEquipo
            e.Row.Cells(2).Visible = True 'DescripcionEquipo
            'e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(3).HeaderText) = True, True, False)
            'e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(4).HeaderText) = True, True, False)
            'e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(5).HeaderText) = True, True, False)
            'e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(6).HeaderText) = True, True, False)
            'e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(7).HeaderText) = True, True, False)
            'e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(8).HeaderText) = True, True, False)
            'e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(9).HeaderText) = True, True, False)
            'e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(10).HeaderText) = True, True, False)
            'e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(11).HeaderText) = True, True, False)
            'e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(12).HeaderText) = True, True, False)
            'e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(13).HeaderText) = True, True, False)
            'e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(14).HeaderText) = True, True, False)
            'e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(15).HeaderText) = True, True, False)
            'e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(16).HeaderText) = True, True, False)
            'e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(17).HeaderText) = True, True, False)
            'e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(18).HeaderText) = True, True, False)
            'e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(19).HeaderText) = True, True, False)
            'e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(20).HeaderText) = True, True, False)
            'e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(21).HeaderText) = True, True, False)
            'e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(22).HeaderText) = True, True, False)
            'e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(23).HeaderText) = True, True, False)
            'e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(24).HeaderText) = True, True, False)
            'e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(25).HeaderText) = True, True, False)
            'e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(26).HeaderText) = True, True, False)
            'e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(27).HeaderText) = True, True, False)
            'e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(28).HeaderText) = True, True, False)
            'e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(29).HeaderText) = True, True, False)
            'e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(30).HeaderText) = True, True, False)
            'e.Row.Cells(31).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(31).HeaderText) = True, True, False)
            'e.Row.Cells(32).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(32).HeaderText) = True, True, False)
            'e.Row.Cells(33).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(33).HeaderText) = True, True, False)
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Mantenimientos
            e.Row.Cells(1).Visible = True 'IdEquipo
            e.Row.Cells(2).Visible = True 'DescripcionEquipo
            'e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(3).HeaderText) = True, True, False)
            'e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(4).HeaderText) = True, True, False)
            'e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(5).HeaderText) = True, True, False)
            'e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(6).HeaderText) = True, True, False)
            'e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(7).HeaderText) = True, True, False)
            'e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(8).HeaderText) = True, True, False)
            'e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(9).HeaderText) = True, True, False)
            'e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(10).HeaderText) = True, True, False)
            'e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(11).HeaderText) = True, True, False)
            'e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(12).HeaderText) = True, True, False)
            'e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(13).HeaderText) = True, True, False)
            'e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(14).HeaderText) = True, True, False)
            'e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(15).HeaderText) = True, True, False)
            'e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(16).HeaderText) = True, True, False)
            'e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(17).HeaderText) = True, True, False)
            'e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(18).HeaderText) = True, True, False)
            'e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(19).HeaderText) = True, True, False)
            'e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(20).HeaderText) = True, True, False)
            'e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(21).HeaderText) = True, True, False)
            'e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(22).HeaderText) = True, True, False)
            'e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(23).HeaderText) = True, True, False)
            'e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(24).HeaderText) = True, True, False)
            'e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(25).HeaderText) = True, True, False)
            'e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(26).HeaderText) = True, True, False)
            'e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(27).HeaderText) = True, True, False)
            'e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(28).HeaderText) = True, True, False)
            'e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(29).HeaderText) = True, True, False)
            'e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(30).HeaderText) = True, True, False)
            'e.Row.Cells(31).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(31).HeaderText) = True, True, False)
            'e.Row.Cells(32).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(32).HeaderText) = True, True, False)
            'e.Row.Cells(33).Visible = IIf(IsDate(grdDetalleEquiposFixed.Columns(33).HeaderText) = True, True, False)
        End If
    End Sub

    Private Sub grdDetalleEquiposScrollable_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleEquiposScrollable.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(0).HeaderText) = True, True, False)
            e.Row.Cells(1).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(1).HeaderText) = True, True, False)
            e.Row.Cells(2).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(2).HeaderText) = True, True, False)
            e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(3).HeaderText) = True, True, False)
            e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(4).HeaderText) = True, True, False)
            e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(5).HeaderText) = True, True, False)
            e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(6).HeaderText) = True, True, False)
            e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(7).HeaderText) = True, True, False)
            e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(8).HeaderText) = True, True, False)
            e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(9).HeaderText) = True, True, False)
            e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(10).HeaderText) = True, True, False)
            e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(11).HeaderText) = True, True, False)
            e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(12).HeaderText) = True, True, False)
            e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(13).HeaderText) = True, True, False)
            e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(14).HeaderText) = True, True, False)
            e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(15).HeaderText) = True, True, False)
            e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(16).HeaderText) = True, True, False)
            e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(17).HeaderText) = True, True, False)
            e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(18).HeaderText) = True, True, False)
            e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(19).HeaderText) = True, True, False)
            e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(20).HeaderText) = True, True, False)
            e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(21).HeaderText) = True, True, False)
            e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(22).HeaderText) = True, True, False)
            e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(23).HeaderText) = True, True, False)
            e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(24).HeaderText) = True, True, False)
            e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(25).HeaderText) = True, True, False)
            e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(26).HeaderText) = True, True, False)
            e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(27).HeaderText) = True, True, False)
            e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(28).HeaderText) = True, True, False)
            e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(29).HeaderText) = True, True, False)
            e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(30).HeaderText) = True, True, False)
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(0).HeaderText) = True, True, False)
            e.Row.Cells(1).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(1).HeaderText) = True, True, False)
            e.Row.Cells(2).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(2).HeaderText) = True, True, False)
            e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(3).HeaderText) = True, True, False)
            e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(4).HeaderText) = True, True, False)
            e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(5).HeaderText) = True, True, False)
            e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(6).HeaderText) = True, True, False)
            e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(7).HeaderText) = True, True, False)
            e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(8).HeaderText) = True, True, False)
            e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(9).HeaderText) = True, True, False)
            e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(10).HeaderText) = True, True, False)
            e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(11).HeaderText) = True, True, False)
            e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(12).HeaderText) = True, True, False)
            e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(13).HeaderText) = True, True, False)
            e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(14).HeaderText) = True, True, False)
            e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(15).HeaderText) = True, True, False)
            e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(16).HeaderText) = True, True, False)
            e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(17).HeaderText) = True, True, False)
            e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(18).HeaderText) = True, True, False)
            e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(19).HeaderText) = True, True, False)
            e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(20).HeaderText) = True, True, False)
            e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(21).HeaderText) = True, True, False)
            e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(22).HeaderText) = True, True, False)
            e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(23).HeaderText) = True, True, False)
            e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(24).HeaderText) = True, True, False)
            e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(25).HeaderText) = True, True, False)
            e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(26).HeaderText) = True, True, False)
            e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(27).HeaderText) = True, True, False)
            e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(28).HeaderText) = True, True, False)
            e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(29).HeaderText) = True, True, False)
            e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquiposScrollable.Columns(30).HeaderText) = True, True, False)
        End If
    End Sub


    Protected Sub grdDetalleEquiposFixed_RowCommand_Botones(sender As Object, e As GridViewCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If

            If e.CommandName = "GenerarOT" Then
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0669", strOpcionModulo, "CMMS")
                Dim Valores() As String = e.CommandArgument.ToString.Split("|")
                'intIndexContratoEquipo = Valores(6)
                intIndexContratoEquipo = Valores(7)

                stringCodigoCN = Valores(1)
                stringCodigoOt = ""
                lblTitleOT.Text = String.Concat(" ", stringCodigoOt)
                'stringCodigoEquipo = Valores(7)
                stringCodigoEquipo = Valores(8)
                Dim EquipoNeg As New clsEquipoNegocios
                'If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & Valores(7).ToString & "'").Rows(0).Item(0) = "0" Then
                '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                'End If
                hfdOperacionDetalle.Value = "N"

                LimpiarObjetosOrdenTrabajo()
                ValidarTextoOrdenTrabajo(True)
                ActivarObjetosOrdenTrabajo(True)
                LlenarDataOrdenTrabajo(Valores, hfdOperacionDetalle.Value)
                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ElseIf e.CommandName = "EliminarEquipo" Then
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0670", strOpcionModulo, "CMMS")
                Dim Valores() As String = e.CommandArgument.ToString.Split("|")
                'intIndexContratoEquipo = Valores(6)
                intIndexContratoEquipo = Valores(7)
                'hfdIdEquipo.Value = Valores(7)
                hfdIdEquipo.Value = Valores(8)
                If Split(Valores(1), "-")(0) <> "" AndAlso fValidarSiHayMovimientosOrdenTrabajo(Split(Valores(1), "-")(0), Split(Valores(1), "-")(1), Split(Valores(1), "-")(2)) Then
                    lblDescripcionMensajeDocumento.Text = "Existe información ya registrada en este equipo para el periodo " & cboMes.SelectedItem.Text
                    lblNroDocumentoMensajeDocumento.Visible = False
                    btnSiMensajeDocumento.Visible = False
                    btnNoMensajeDocumento.Visible = False
                    lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                Else
                    lblDescripcionMensajeDocumento.Text = "¿Desea eliminar este equipo del contrato?" '& cboMes.SelectedItem.Text
                    lblNroDocumentoMensajeDocumento.Visible = False
                    btnSiMensajeDocumento.Visible = True
                    btnNoMensajeDocumento.Visible = True
                    lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                End If
                ValidationSummary2.ValidationGroup = "vgrpValidar"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidar"
                Me.Page.Validators.Add(MyValidator)
            ElseIf e.CommandName = "VerMantenimientoPlanificacionEquipo" Then
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0671", strOpcionModulo, "CMMS")
                Dim argumnt As String = e.CommandArgument.ToString().Replace("*", "|")
                Dim Valores() As String = argumnt.Split("|")
                'intIndexContratoEquipo = Valores(6)
                intIndexContratoEquipo = Valores(7)
                'hfdIdEquipo.Value = Valores(7)
                hfdIdEquipo.Value = Valores(8)

                stringCodigoCN = Valores(1)
                stringCodigoOt = Valores(4)
                'stringCodigoEquipo = Valores(7)
                stringCodigoEquipo = Valores(8)
                hfdOperacionDetalle.Value = "E"
                lblTitleOT.Text = String.Concat(" ", stringCodigoOt)

                LimpiarObjetosOrdenTrabajo()
                ValidarTextoOrdenTrabajo(True)
                ActivarObjetosOrdenTrabajo(True)

                LlenarDataOrdenTrabajo(Valores, hfdOperacionDetalle.Value)

                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ElseIf e.CommandName = "VerEquipo" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                Response.Redirect("~/frmLogiGenerarEquipo.aspx?IdEquipo=" & Valores(0).ToString)
            End If
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
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
            lblMensajeMantenimientoOrdenTrabajo.Text = "Personal agregado con éxito."
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            lblMensajeMantenimientoOrdenTrabajo.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de inicio de planificación del mantenimiento.")
            ElseIf txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de término de planificación del mantenimiento.")
            ElseIf CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) > CDate(txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text) Then
                Throw New Exception("La fecha de término debe de ser mayor a la fecha de inicio de planificación del mantenimiento.")
            ElseIf cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            ElseIf cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = "" Then
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
            'JMUG: 07/09/2025
            hfdFechaPlanificacion.Value = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
            MarcarOTenCalendario()

            'Guardar o Editar | validar x codigo OT 
            Dim strCodCN As String() = stringCodigoCN.Split("-")
            Dim detalleContratoMet = New clsDetalleContratoMetodos
            Dim DetalleContrato = detalleContratoMet.DetalleContratoListarPorId(strCodCN(0), strCodCN(1), strCodCN(2), Session("IdEmpresa"), stringCodigoEquipo)
            Dim listDetalleContrato = detalleContratoMet.lstDetalleContratoListarPorId(strCodCN(0), strCodCN(1), strCodCN(2), Session("IdEmpresa"), stringCodigoEquipo)

            If (String.IsNullOrEmpty(stringCodigoOt)) Then 'Nuevo
                'Crear DONE, replicar SolicitudServicio DONE ,95% cambiar mensaje al crear una nueva OT
                CrearOt(DetalleContrato)
                'JMUG: 06/09/2025 CargarCestaEquipos()
            Else 'Editar
                'Validar si el componente no iniciado actividad, bloquear desde abrir OT la fecha y los combos correspondientes
                'If (chkReajustarProgramacion.Checked = True And txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text <> "" And IsNumeric(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text)) Then
                '    'If txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim <> "" And IsNumeric(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim) Then
                '    'Dim strContratoRef As String() = strCodCN.Split("-")
                '    'Dim Contrato As LOGI_CABECERACONTRATO = ContratoNeg.ContratoListarPorId(strContratoRef(0).ToString, strContratoRef(1).ToString, strContratoRef(2).ToString, Session("IdEmpresa"))
                '    Dim Contrato As LOGI_CABECERACONTRATO = ContratoNeg.ContratoListarPorId(strCodCN(0), strCodCN(1), strCodCN(2), Session("IdEmpresa"))
                '    Dim dFecIni As DateTime = Contrato.dFechaVigenciaInicialCabeceraContrato
                '    Dim dFecFin As DateTime = Contrato.dFechaVigenciaFinalCabeceraContrato

                '    'Obtienes la cantidad de días entre las dos fechas del contrato
                '    Dim nDias As Integer = DateDiff(DateInterval.Day, dFecIni, dFecFin)
                '    Dim dFechaInicioOTDiferencial As DateTime = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                '    Dim nDiasDiferencial As Integer = DateDiff(DateInterval.Day, dFecIni, dFechaInicioOTDiferencial)
                '    Dim nDiasAcumulados As Integer = 0

                '    Dim dsOTGrupo = ContratoNeg.ContratoGetData("
                '        SELECT dFechaInicioPlanificadaCabeceraOrdenTrabajo, dFechaTerminoPlanificadaCabeceraOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo 
                '        FROM LOGI_CABECERAORDENTRABAJO
                '        WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND 
                '              vContratoReferenciaCabeceraOrdenTrabajo = '" & stringCodigoCN & "' AND 
                '              nPeriodicidadDiasCabeceraOrdenTrabajo = '" & txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text & "' AND
                '              cIdTipoMantenimientoCabeceraOrdenTrabajo = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "'
                '        ")


                '    Do While nDias >= (nDiasAcumulados + nDiasDiferencial)
                '        Dim dFechaInicioOT, dFechaTerminoOT As DateTime
                '        'OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ""
                '        dFechaInicioOT = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                '        dFechaTerminoOT = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
                '        'OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = dFechaInicioOT.AddDays(nDiasAcumulados)
                '        'OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = dFechaTerminoOT.AddDays(nDiasAcumulados)
                '        'planContrato.dFechaHoraProgramacionPlanificacionEquipoContrato = dFechaInicioOT.AddDays(nDiasAcumulados)
                '        'planContrato.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = ""

                '        'Dim fechaCorrecta As Date = DateTime.ParseExact(fechaTexto, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                '        'String.Format("{0:dd/MM/yyyy}", Now)
                '        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", dFechaInicioOT.AddDays(nDiasAcumulados))
                '        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", dFechaTerminoOT.AddDays(nDiasAcumulados))

                '        For Each OTGrupo In dsOTGrupo.Rows
                '            OTGrupo.dFechaInicioPlanificadaCabeceraOrdenTrabajo
                '            OTGrupo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo
                '            OTGrupo.cIdTipoDocumentoCabeceraOrdenTrabajo
                '            OTGrupo.vIdNumeroSerieCabeceraOrdenTrabajo
                '            OTGrupo.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                '        Next

                '        nDiasAcumulados = nDiasAcumulados + Convert.ToInt16(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text)
                '        'If ContratoNeg.ContratoInsertaDetallev2(DetalleContrato, planContrato, OrdTra, ColeccionRRHH, ColeccionComponente) = 0 Then
                '        'End If
                '        EditarOT(DetalleContrato)
                '    Loop
                '    'End If

                'Else
                '    EditarOT(DetalleContrato)
                'End If

                If chkReajustarProgramacion.Checked AndAlso
                   txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text <> "" AndAlso
                   IsNumeric(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text) Then

                    Dim Contrato As LOGI_CABECERACONTRATO =
                        ContratoNeg.ContratoListarPorId(strCodCN(0), strCodCN(1), strCodCN(2), Session("IdEmpresa"))

                    Dim dFecIni As DateTime = Contrato.dFechaVigenciaInicialCabeceraContrato
                    Dim dFecFin As DateTime = Contrato.dFechaVigenciaFinalCabeceraContrato

                    Dim nDias As Integer = DateDiff(DateInterval.Day, dFecIni, dFecFin)
                    'JMUG: 06/09/2025 Dim dFechaInicioOTDiferencial As DateTime = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                    Dim formatos() As String = {"dd/MM/yyyy", "d/M/yyyy"}
                    Dim dFechaInicioOTDiferencial As DateTime = DateTime.ParseExact(
                        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text,
                        formatos,
                        Globalization.CultureInfo.InvariantCulture,
                        Globalization.DateTimeStyles.None
                    )
                    'Dim dFechaInicioOTDiferencial As DateTime = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text

                    Dim nDiasDiferencial As Integer = DateDiff(DateInterval.Day, dFecIni, dFechaInicioOTDiferencial)
                    Dim nDiasAcumulados As Integer = 0
                    Dim periodicidad As Integer = Convert.ToInt32(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text)

                    'Traemos las OT existentes
                    Dim dsOTGrupo = ContratoNeg.ContratoGetData("
                        SELECT dFechaInicioPlanificadaCabeceraOrdenTrabajo,
                               dFechaTerminoPlanificadaCabeceraOrdenTrabajo,
                               cIdTipoDocumentoCabeceraOrdenTrabajo,
                               vIdNumeroSerieCabeceraOrdenTrabajo,
                               vIdNumeroCorrelativoCabeceraOrdenTrabajo
                        FROM LOGI_CABECERAORDENTRABAJO
                        WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND 
                              vContratoReferenciaCabeceraOrdenTrabajo = '" & stringCodigoCN & "' AND 
                              cIdTipoMantenimientoCabeceraOrdenTrabajo = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "' AND
                              cIdEquipoCabeceraOrdenTrabajo = '" & stringCodigoEquipo & "' AND
                              cEstadoCabeceraOrdenTrabajo = 'R' AND DATEDIFF(dd, dFechaInicioPlanificadaCabeceraOrdenTrabajo, '" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text.Trim & "')<=0
                        ORDER BY CONVERT(INT, vIdNumeroCorrelativoCabeceraOrdenTrabajo), dFechaInicioPlanificadaCabeceraOrdenTrabajo
                    ")

                    ' 1) Generamos la lista de nuevas fechas
                    Dim listaFechas As New List(Of DateTime)
                    Do While nDias >= (nDiasAcumulados + nDiasDiferencial)
                        Dim dFechaInicioOT As DateTime = dFechaInicioOTDiferencial.AddDays(nDiasAcumulados)
                        listaFechas.Add(dFechaInicioOT)
                        nDiasAcumulados += periodicidad
                        If periodicidad = 0 Then
                            Exit Do
                        End If
                    Loop

                    'Dim dFechaTerminoOTDiferencial As DateTime = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
                    Dim dFechaTerminoOTDiferencial As DateTime = DateTime.ParseExact(
                        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text,
                        formatos,
                        Globalization.CultureInfo.InvariantCulture,
                        Globalization.DateTimeStyles.None
                    )
                    'Dim dFechaTerminoOTDiferencial As DateTime = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text

                    Dim nDiasInicioTerminoDiferencial As Integer = DateDiff(DateInterval.Day, dFechaInicioOTDiferencial, dFechaTerminoOTDiferencial)


                    ' 2) Reajustamos comparando nuevas fechas vs OT existentes
                    Dim totalExistentes As Integer = dsOTGrupo.Rows.Count
                    Dim totalNuevas As Integer = listaFechas.Count
                    Dim total As Integer = Math.Max(totalExistentes, totalNuevas)

                    For i As Integer = 0 To total - 1
                        If i < totalExistentes AndAlso i < totalNuevas Then
                            ' Existe OT y hay fecha nueva -> actualizar
                            Dim row As DataRow = dsOTGrupo.Rows(i)
                            row("dFechaInicioPlanificadaCabeceraOrdenTrabajo") = listaFechas(i)

                            stringCodigoOt = row("cIdTipoDocumentoCabeceraOrdenTrabajo") + "-" + row("vIdNumeroSerieCabeceraOrdenTrabajo") + "-" + row("vIdNumeroCorrelativoCabeceraOrdenTrabajo")
                            'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", listaFechas(i).AddDays(nDiasAcumulados))
                            'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:d/MM/yyyy}", listaFechas(i))

                            'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(listaFechas(i), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(
                                listaFechas(i),
                                formatos,
                                Globalization.CultureInfo.InvariantCulture,
                                Globalization.DateTimeStyles.None
                            )

                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", dFechaTerminoOT.AddDays(nDiasAcumulados))
                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", listaFechas(i).AddDays(nDiasAcumulados + nDiasInicioTerminoDiferencial))
                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:d/MM/yyyy}", listaFechas(i).AddDays(nDiasInicioTerminoDiferencial))
                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(listaFechas(i), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(nDiasInicioTerminoDiferencial)
                            txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(
                                listaFechas(i),
                                formatos,
                                Globalization.CultureInfo.InvariantCulture,
                                Globalization.DateTimeStyles.None
                            ).AddDays(nDiasInicioTerminoDiferencial)

                            ' Aquí haces UPDATE en BD con IdDocumento (row("cIdTipoDocumentoCabeceraOrdenTrabajo"), row("vIdNumeroSerieCabeceraOrdenTrabajo"), row("vIdNumeroCorrelativoCabeceraOrdenTrabajo"))
                            'ActualizarOT(row, listaFechas(i))
                            EliminarOT(row, True) 'Activa lo eliminado
                            EditarOT(DetalleContrato)
                        ElseIf i >= totalExistentes AndAlso i < totalNuevas Then
                            ' No existe OT -> insertar
                            'InsertarOT(listaFechas(i))



                            'Dim row As DataRow = dsOTGrupo.Rows(i)
                            'row("dFechaInicioPlanificadaCabeceraOrdenTrabajo") = listaFechas(i)

                            'stringCodigoOt = row("cIdTipoDocumentoCabeceraOrdenTrabajo") + "-" + row("vIdNumeroSerieCabeceraOrdenTrabajo") + "-" + row("vIdNumeroCorrelativoCabeceraOrdenTrabajo")
                            'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", listaFechas(i).AddDays(nDiasAcumulados))
                            'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:d/MM/yyyy}", listaFechas(i))

                            'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(listaFechas(i), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(
                                listaFechas(i),
                                formatos,
                                Globalization.CultureInfo.InvariantCulture,
                                Globalization.DateTimeStyles.None
                            )

                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", dFechaTerminoOT.AddDays(nDiasAcumulados))
                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:dd/MM/yyyy}", listaFechas(i).AddDays(nDiasAcumulados + nDiasInicioTerminoDiferencial))
                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = String.Format("{0:d/MM/yyyy}", listaFechas(i).AddDays(nDiasInicioTerminoDiferencial))
                            'txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(listaFechas(i), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(nDiasInicioTerminoDiferencial)
                            txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = DateTime.ParseExact(
                                listaFechas(i),
                                formatos,
                                Globalization.CultureInfo.InvariantCulture,
                                Globalization.DateTimeStyles.None
                            ).AddDays(nDiasInicioTerminoDiferencial)




                            CrearOT(DetalleContrato)
                        ElseIf i < totalExistentes AndAlso i >= totalNuevas Then
                            ' Sobra OT -> eliminar
                            Dim row As DataRow = dsOTGrupo.Rows(i)
                            EliminarOT(row, False) 'Desactiva el registro
                        End If
                    Next

                Else
                    'Si no se marcó reajustar, solo se edita la OT actual
                    EditarOT(DetalleContrato)
                End If

            End If

            CargarCestaEquipos()
            Me.grdDetalleEquiposFixed.DataSource = Session("CestaEquipoContrato")
            Me.grdDetalleEquiposFixed.DataBind()

            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Sub EliminarOT(Row As DataRow, bEstado As Boolean)
        stringCodigoOt = Row("cIdTipoDocumentoCabeceraOrdenTrabajo") + "-" +
            Row("vIdNumeroSerieCabeceraOrdenTrabajo") + "-" +
            Row("vIdNumeroCorrelativoCabeceraOrdenTrabajo")
        Dim dsOTGrupo = ContratoNeg.ContratoGetData("
            UPDATE LOGI_CABECERAORDENTRABAJO
            SET bEstadoRegistroCabeceraOrdenTrabajo = '" & If(bEstado, "1", "0") & "' 
            WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND 
                  vContratoReferenciaCabeceraOrdenTrabajo = '" & stringCodigoCN & "' AND 
                  cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Row("cIdTipoDocumentoCabeceraOrdenTrabajo") & "' AND 
                  vIdNumeroSerieCabeceraOrdenTrabajo = '" & Row("vIdNumeroSerieCabeceraOrdenTrabajo") & "' AND
                  vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Row("vIdNumeroCorrelativoCabeceraOrdenTrabajo") & "'
        ")
    End Sub

    Sub CrearOT(DetalleContrato As LOGI_DETALLECONTRATO)
        Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios

        'orden trabajo contrato
        Dim OrdTra As New LOGI_CABECERAORDENTRABAJO
        OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = "OT"
        OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = Year(Now).ToString
        OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ""
        OrdTra.dFechaTransaccionCabeceraOrdenTrabajo = Now
        OrdTra.dFechaEmisionCabeceraOrdenTrabajo = Now
        OrdTra.cIdClienteCabeceraOrdenTrabajo = hfdIdAuxiliar.Value
        OrdTra.cIdEquipoCabeceraOrdenTrabajo = stringCodigoEquipo
        OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
        OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
        OrdTra.cIdEmpresa = Session("IdEmpresa")
        OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = ""
        OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
        OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = ""
        OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
        OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
        OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
        OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
        OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
        OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = stringCodigoCN 'JMUG: 03/09/2025 stringCodigoCN(0) + "-" + stringCodigoCN(1) + "-" + stringCodigoCN(2)
        OrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo = Session("IdUsuario")
        OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo = cboTipoControlTiempoMantenimientoOrdenTrabajo.SelectedValue
        OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo = cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue
        OrdTra.nPeriodicidadDiasCabeceraOrdenTrabajo = If(IsNumeric(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text) And txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim <> "", txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text, Nothing)
        'orden trabajo contrato

        'DetalleContratoNeg SOLO TRAER POR EQUIPO MAS CONTRATO
        'DetalleContrato
        Dim planContrato As New LOGI_PLANIFICACIONEQUIPOCONTRATO
        planContrato.cIdTipoDocumentoCabeceraContrato = DetalleContrato.cIdTipoDocumentoCabeceraContrato
        planContrato.vIdNumeroSerieCabeceraContrato = DetalleContrato.vIdNumeroSerieCabeceraContrato
        planContrato.vIdNumeroCorrelativoCabeceraContrato = DetalleContrato.vIdNumeroCorrelativoCabeceraContrato
        planContrato.cIdEmpresa = DetalleContrato.cIdEmpresa
        planContrato.nIdNumeroItemDetalleContrato = DetalleContrato.nIdNumeroItemDetalleContrato
        planContrato.cIdEquipoDetalleContrato = DetalleContrato.cIdEquipoDetalleContrato

        'JMUG: 07/09/2025
        'planContrato.cIdPeriodoMesPlanificacionEquipoContrato = String.Concat(cboPeriodo.SelectedValue, cboMes.SelectedValue)
        planContrato.cIdPeriodoMesPlanificacionEquipoContrato = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text).ToString("yyyyM")
        planContrato.dFechaHoraProgramacionPlanificacionEquipoContrato = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
        planContrato.cIdTipoMantenimientoPlanificacionEquipoContrato = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
        planContrato.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = ""
        planContrato.vOrdenTrabajoClientePlanificacionEquipoContrato = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
        planContrato.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato = cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue


        'checklist y componentes

        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        Dim cIdPlantilla = String.Concat(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue.Trim(), cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue)

        Dim result = PlantillaCheckListNeg.ChecklistPlantillaInsertaCopiaComponentes(stringCodigoEquipo, cIdPlantilla)

        Dim OrdenFabricacionNeg As New clsOrdenFabricacionNegocios
        Dim dsComponenteEquipo = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdJerarquiaCatalogo " &
                                                                             "FROM LOGI_EQUIPO AS EQU " &
                                                                             "WHERE EQU.cIdEnlaceEquipo = '" & stringCodigoEquipo & "' " &
                                                                             "      AND EQU.cIdJerarquiaCatalogo = '1' " &
                                                                             "      AND EQU.bEstadoRegistroEquipo = '1' " &
                                                                             "      AND EQU.cIdEnlaceCatalogo = '" & hfdIdCatalogoEquipo.Value & "' " &
                                                                             "ORDER BY EQU.vDescripcionEquipo")
        Dim ColeccionComponente As New List(Of LOGI_COMPONENTEORDENTRABAJO)

        For Each ComponenteEquipo In dsComponenteEquipo.Rows
            Dim ComEqu As New LOGI_COMPONENTEORDENTRABAJO
            With ComEqu
                .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                .cIdEmpresa = OrdTra.cIdEmpresa
                .cIdEquipoCabeceraOrdenTrabajo = stringCodigoEquipo
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
        'checklist y componentes

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

        If txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim <> "" And IsNumeric(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim) Then
            Dim strContratoRef As String() = OrdTra.vContratoReferenciaCabeceraOrdenTrabajo.Split("-")
            Dim Contrato As LOGI_CABECERACONTRATO = ContratoNeg.ContratoListarPorId(strContratoRef(0).ToString, strContratoRef(1).ToString, strContratoRef(2).ToString, Session("IdEmpresa"))
            Dim dFecIni As DateTime = Contrato.dFechaVigenciaInicialCabeceraContrato
            Dim dFecFin As DateTime = Contrato.dFechaVigenciaFinalCabeceraContrato

            'Obtienes la cantidad de días entre las dos fechas del contrato
            Dim nDias As Integer = DateDiff(DateInterval.Day, dFecIni, dFecFin)
            'Dim dFechaInicioOTDiferencial As DateTime = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
            Dim formatos() As String = {"dd/MM/yyyy", "d/M/yyyy"}
            Dim dFechaInicioOTDiferencial As DateTime = DateTime.ParseExact(
                txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text,
                formatos,
                Globalization.CultureInfo.InvariantCulture,
                Globalization.DateTimeStyles.None
            )
            Dim nDiasDiferencial As Integer = DateDiff(DateInterval.Day, dFecIni, dFechaInicioOTDiferencial)
            Dim nDiasAcumulados As Integer = 0
            Do While nDias >= (nDiasAcumulados + nDiasDiferencial)
                Dim dFechaInicioOT, dFechaTerminoOT As DateTime
                OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ""
                dFechaInicioOT = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                dFechaTerminoOT = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
                OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = dFechaInicioOT.AddDays(nDiasAcumulados)
                OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = dFechaTerminoOT.AddDays(nDiasAcumulados)
                planContrato.dFechaHoraProgramacionPlanificacionEquipoContrato = dFechaInicioOT.AddDays(nDiasAcumulados)
                planContrato.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = ""
                nDiasAcumulados = nDiasAcumulados + Convert.ToInt16(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text)
                If ContratoNeg.ContratoInsertaDetallev2(DetalleContrato, planContrato, OrdTra, ColeccionRRHH, ColeccionComponente) = 0 Then
                End If
            Loop
            'ElseIf txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim <> "" Then
        ElseIf txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim = "" Then
            If ContratoNeg.ContratoInsertaDetallev2(DetalleContrato, planContrato, OrdTra, ColeccionRRHH, ColeccionComponente) = 0 Then
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

                MyValidator.ErrorMessage = "Transacción registrada con éxito"
            Else
                Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
            End If
        End If
    End Sub

    Sub EditarOT(DetalleContrato As LOGI_DETALLECONTRATO)
        Dim strCodigo As String() = stringCodigoOt.Split("-")
        Dim strCodigoCn As String() = stringCodigoCN.Split("-")
        Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios
        Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId(strCodigo(0),
                                                             strCodigo(1),
                                                             strCodigo(2),
                                                             Session("IdEmpresa"))


        Dim RrhhOrdTraBD = OrdenTrabajoNeg.OrdenTrabajoRecursos(OrdTra)

        Dim UsuCorreo As New GNRL_USUARIO

        Dim ColeccionRRHH As New List(Of LOGI_RECURSOSORDENTRABAJO)
        For i = 0 To Session("CestaSSPersonal").Rows.Count - 1

            Dim existe As Boolean = RrhhOrdTraBD.Any(Function(p) p.RRHH_PERSONAL.cIdPersonal.Trim = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim))
            If existe Then
                'update
                Dim item = RrhhOrdTraBD.Find(Function(p) p.RRHH_PERSONAL.cIdPersonal.Trim = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim))
                item.bEstadoRegistroRecursosOrdenTrabajo = True
                item.bResponsableRecursosOrdenTrabajo = (Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim)

                OrdenTrabajoNeg.OrdenTrabajoUpdateResponsable(item)
            Else
                'insert
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


                OrdenTrabajoNeg.OrdenTrabajoInsertResponsable(RrhhOrdTra)
            End If

        Next

        OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
        OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text

        'JMUG: 18/07/2025
        OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo = cboTipoControlTiempoMantenimientoOrdenTrabajo.SelectedValue

        'JMUG: 05/09/2025
        OrdTra.nPeriodicidadDiasCabeceraOrdenTrabajo = If(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text.Trim <> "" And IsNumeric(txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text), txtPeriodicidadDiasMantenimientoOrdenTrabajo.Text, Nothing)

        If (OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo <> cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue) Then
            OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
            OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo = cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue

            InsertComponentesAndActividades(OrdTra, UsuCorreo)

        End If

        OrdenTrabajoNeg.OrdenTrabajoEdita(OrdTra)

        'Dim queryUpdatePlanificacion = "UPDATE LOGI_PLANIFICACIONEQUIPOCONTRATO SET 
        '     dFechaHoraProgramacionPlanificacionEquipoContrato = '" & Convert.ToDateTime(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) &
        '     "' , cIdTipoMantenimientoPlanificacionEquipoContrato = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue &
        '     "' , vOrdenTrabajoClientePlanificacionEquipoContrato = '" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text &
        '     "' , cIdNumeroPlantillaCheckListPlanificacionEquipoContrato = '" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue &
        '    "' WHERE cIdTipoDocumentoCabeceraContrato = '" & strCodigoCn(0) &
        '    "' AND vIdNumeroSerieCabeceraContrato = '" & strCodigoCn(1) &
        '    "' AND vIdNumeroCorrelativoCabeceraContrato = '" & strCodigoCn(2) &
        '    "' AND cIdEmpresa = '" & Session("IdEmpresa") &
        '    "' AND vOrdenTrabajoReferenciaPlanificacionEquipoContrato = '" & stringCodigoOt &
        '    "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & String.Concat(cboPeriodo.SelectedValue, cboMes.SelectedValue) & "'"
        Dim queryUpdatePlanificacion = "UPDATE LOGI_PLANIFICACIONEQUIPOCONTRATO SET 
             dFechaHoraProgramacionPlanificacionEquipoContrato = '" & Convert.ToDateTime(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) &
             "' , cIdTipoMantenimientoPlanificacionEquipoContrato = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue &
             "' , vOrdenTrabajoClientePlanificacionEquipoContrato = '" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text &
             "' , cIdNumeroPlantillaCheckListPlanificacionEquipoContrato = '" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue &
            "' WHERE cIdTipoDocumentoCabeceraContrato = '" & strCodigoCn(0) &
            "' AND vIdNumeroSerieCabeceraContrato = '" & strCodigoCn(1) &
            "' AND vIdNumeroCorrelativoCabeceraContrato = '" & strCodigoCn(2) &
            "' AND cIdEmpresa = '" & Session("IdEmpresa") &
            "' AND vOrdenTrabajoReferenciaPlanificacionEquipoContrato = '" & stringCodigoOt &
            "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text).ToString("yyyyM") & "'"

        Dim getPlanificacionByOtId = OrdenTrabajoNeg.OrdenTrabajoGetData(queryUpdatePlanificacion)



    End Sub

    Sub InsertComponentesAndActividades(OrdTra As LOGI_CABECERAORDENTRABAJO, UsuCorreo As GNRL_USUARIO)
        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        Dim cIdPlantilla = String.Concat(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue.Trim(), cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue)

        Dim result = PlantillaCheckListNeg.ChecklistPlantillaInsertaCopiaComponentes(stringCodigoEquipo, cIdPlantilla)

        Dim EquipoMet As New clsEquipoMetodos
        Dim Equipo As LOGI_EQUIPO = EquipoMet.EquipoListarPorId(stringCodigoEquipo)
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
                                                                               "     EQU.cIdEnlaceEquipo = '" & stringCodigoEquipo & "' AND " &
                                                                               "     ISNULL(EQU.bEstadoRegistroEquipo, '1') = '1' " &
                                                                               "WHERE CABCHKLISPLA.cIdTipoMantenimiento = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "' AND CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "' " &
                                                                               "      AND CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla = '" & Equipo.cIdCatalogo & "' AND CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0' " &
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
                .cIdEquipoCabeceraOrdenTrabajo = stringCodigoEquipo
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
                                                                             "WHERE EQU.cIdEnlaceEquipo = '" & stringCodigoEquipo & "' " &
                                                                             "      AND EQU.cIdJerarquiaCatalogo = '1' " &
                                                                             "      AND EQU.bEstadoRegistroEquipo = '1' " &
                                                                             "      AND EQU.cIdEnlaceCatalogo = '" & hfdIdCatalogoEquipo.Value & "' " &
                                                                             "ORDER BY EQU.vDescripcionEquipo")

        Dim ColeccionComponente As New List(Of LOGI_COMPONENTEORDENTRABAJO)
        For Each ComponenteEquipo In dsComponenteEquipo.Rows
            Dim ComEqu As New LOGI_COMPONENTEORDENTRABAJO
            With ComEqu
                .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                .cIdEmpresa = OrdTra.cIdEmpresa
                .cIdEquipoCabeceraOrdenTrabajo = stringCodigoEquipo
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
        If (OrdenTrabajoNeg.OrdenTrabajoDeleteAndInsertComponentes(OrdTra, ColeccionCheckList, ColeccionComponente) = 0) Then
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

            'OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(1).Text & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(2).Text & "'")
        End If

    End Sub

    Sub MarcarOTenCalendario()
        Dim gvRow As GridViewRow = grdDetalleEquiposFixed.Rows(intIndexContratoEquipo - ((grdDetalleEquiposFixed.PageSize) * (grdDetalleEquiposFixed.PageIndex))) 'JMUG: 19/05/2022
        Dim strNombreColumnaModificar As String = ""
        Dim strNombreColumna As String = ""
        'JMUG: 22/07/2025 18:17
        'For i = 1 To 31
        '    If IsDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) Then
        '        If CDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
        '            strNombreColumna = ("D" & i)
        '        End If
        '        If IsDate(hfdFechaPlanificacion.Value) AndAlso CDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) = CDate(hfdFechaPlanificacion.Value) Then
        '            strNombreColumnaModificar = ("D" & i)
        '        End If
        '    Else
        '        If strNombreColumna = "" Then
        '            Throw New Exception("Esta fecha no esta asignada en la programación.")
        '        End If
        '    End If
        'Next
        For i = 0 To 30
            If IsDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) Then
                If CDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                    strNombreColumna = ("D" & (i + 1))
                End If
                If IsDate(hfdFechaPlanificacion.Value) AndAlso CDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) = CDate(hfdFechaPlanificacion.Value) Then
                    strNombreColumnaModificar = ("D" & (i + 1))
                End If
            Else
                If strNombreColumna = "" Then
                    Throw New Exception("Esta fecha no esta asignada en la programación.")
                End If
            End If
        Next

        Dim resultProgramacionContratoSimple As DataRow() = Session("CestaEquipoContrato").Select("IdEquipo = '" & hfdIdEquipo.Value & "'")
        If resultProgramacionContratoSimple.Length = 0 Then
            'VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
        Else
            Dim rowFil As DataRow() = resultProgramacionContratoSimple
            For Each EquipoContrato As DataRow In rowFil
                If hfdFechaPlanificacion.Value <> "" Then
                    EditarCestaEquipo(True, hfdIdEquipo.Value, EquipoContrato("DescripcionEquipo"), EquipoContrato("Estado"),
                                   strNombreColumnaModificar, "|||" & hfdFechaPlanificacion.Value & "|" & hfdIdOrdenTrabajo.Value & "|" & hfdIdOrdenTrabajoCliente.Value, Session("CestaEquipoContrato"), intIndexContratoEquipo)
                End If
                EditarCestaEquipo(True, hfdIdEquipo.Value, EquipoContrato("DescripcionEquipo"), EquipoContrato("Estado"),
                                  strNombreColumna,
                                  cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "|" & txtIdContrato.Text & "|" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "|" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text & "|" & hfdIdOrdenTrabajo.Value & "|" & txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text, Session("CestaEquipoContrato"), intIndexContratoEquipo)
                MyValidator.ErrorMessage = "Programación registrada con éxito"
                If hfdOperacionDetalle.Value = "E" Then
                    Dim fila As Int16 = 0
                    For i = 0 To Session("CestaPersonalOrdenTrabajo").Rows.Count - 1
                        If Session("CestaPersonalOrdenTrabajo").Rows(fila)("IdEquipo").ToString.Trim = hfdIdEquipo.Value.Trim And Session("CestaPersonalOrdenTrabajo").Rows(fila)("FechaPlanificacion").ToString.Trim = hfdFechaPlanificacion.Value.Trim Then
                            QuitarCestaPersonalOrdenTrabajo(fila, Session("CestaPersonalOrdenTrabajo"))
                            fila = fila - 1
                        End If
                        fila = fila + 1
                    Next
                End If

                For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                    AgregarCestaPersonalOrdenTrabajo(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text, hfdIdEquipo.Value, txtIdContrato.Text, Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Personal").ToString.Trim, hfdIdOrdenTrabajo.Value, hfdIdOrdenTrabajoCliente.Value, Session("CestaPersonalOrdenTrabajo"))
                Next
            Next
        End If
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

            Dim strValidacion As String = ""
            For x = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                If (Session("CestaEquipoContrato").Rows(x)("IdEquipo").ToString.Trim) = hfdIdEquipo.Value.Trim Then
                    Dim strNombreColumna As String = ""
                    'JMUG: 22/07/2025 18:30
                    'For i = 1 To 31
                    '    If IsDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) Then
                    '        If CDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                    '            strNombreColumna = ("D" & i)
                    '            strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                    '            Exit For
                    '        End If
                    '    Else
                    '        If strNombreColumna = "" Then
                    '            Throw New Exception("Esta fecha no esta asignada en la programación.")
                    '        End If
                    '    End If
                    'Next
                    For i = 0 To 30
                        If IsDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) Then
                            If CDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                strNombreColumna = ("D" & (i + 1))
                                strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                                Exit For
                            End If
                        Else
                            If strNombreColumna = "" Then
                                Throw New Exception("Esta fecha no esta asignada en la programación.")
                            End If
                        End If
                    Next
                End If
            Next
            Dim strValidaFecha As String() = strValidacion.Split("|")
            If strValidaFecha(0) <> "" Then
                Throw New Exception("Esta fecha ya esta registrada en la programación para este equipo.")
            Else
                AgregarCestaPersonal(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, True, Session("CestaSSPersonal"))
            End If

            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            lblMensajeMantenimientoOrdenTrabajo.Text = "Personal agregado satisfactoriamente."
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMantenimientoOrdenTrabajo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPersonalAsignadoMantenimientoOrdenTrabajo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            Dim strCodigo = stringCodigoOt.Split("-")
            If (String.IsNullOrEmpty(strCodigo(0))) Then

                For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                    If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                        QuitarCestaPersonal(i, Session("CestaSSPersonal"))
                        Exit For
                    End If
                Next
            Else
                Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios
                Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId(strCodigo(0),
                                                                     strCodigo(1),
                                                                     strCodigo(2),
                                                                     Session("IdEmpresa"))


                Dim RrhhOrdTraBD = OrdenTrabajoNeg.OrdenTrabajoRecursos(OrdTra)

                For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                    If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                        Dim existe As Boolean = RrhhOrdTraBD.Any(Function(p) p.RRHH_PERSONAL.cIdPersonal.Trim = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) And p.bEstadoRegistroRecursosOrdenTrabajo = True)
                        If existe Then
                            'update estatus false
                            Dim query = "UPDATE LOGI_RECURSOSORDENTRABAJO SET bEstadoRegistroRecursosOrdenTrabajo = '" & False & "'
                                    WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & strCodigo(0) & "' AND
                                    vIdNumeroSerieCabeceraOrdenTrabajo = '" & strCodigo(1) & "' AND
                                    vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strCodigo(2) & "' AND
                                    cIdEmpresa = '" & Session("IdEmpresa") & "' And cIdPersonal = '" & (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) & "' "

                            Dim ok = OrdenTrabajoNeg.OrdenTrabajoQueryResponsable(query)
                        End If

                        QuitarCestaPersonal(i, Session("CestaSSPersonal"))
                        Exit For
                    End If
                Next
            End If

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

    Private Sub cboPeriodo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPeriodo.SelectedIndexChanged
        ListarMesPeriodoCombo(txtFechaVigenciaInicio.Text, txtFechaVigenciaFinal.Text)
        CargarCestaEquipos()
    End Sub

    Private Sub cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndexChanged
        ListarPlantillaCheckListCombo()
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboPersonalAsignadoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub grdDetalleEquiposFixed_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleEquiposFixed.PageIndexChanging
        grdDetalleEquiposFixed.PageIndex = e.NewPageIndex
        Me.grdDetalleEquiposFixed.DataSource = Session("CestaEquipoContrato")
        Me.grdDetalleEquiposFixed.DataBind()
        grdDetalleEquiposFixed.SelectedIndex = -1
    End Sub

    Private Sub grdDetalleEquiposScrollable_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleEquiposScrollable.PageIndexChanging
        grdDetalleEquiposScrollable.PageIndex = e.NewPageIndex
        Me.grdDetalleEquiposScrollable.DataSource = Session("CestaEquipoContrato")
        Me.grdDetalleEquiposScrollable.DataBind()
        grdDetalleEquiposScrollable.SelectedIndex = -1
    End Sub


    Private Sub txtFechaInicioPlanificadaMantenimientoOrdenTrabajo_TextChanged(sender As Object, e As EventArgs) Handles txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.TextChanged
        Try
            'Validar si existe datos para la planificación
            lblMensajeMantenimientoOrdenTrabajo.Text = ""
            Dim strValidacion As String = ""
            For x = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                If (Session("CestaEquipoContrato").Rows(x)("IdEquipo").ToString.Trim) = hfdIdEquipo.Value.Trim Then
                    Dim strNombreColumna As String = ""
                    'JMUG: 22/07/2025 18:33
                    'For i = 1 To 31
                    '    If IsDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) Then
                    '        If CDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                    '            strNombreColumna = ("D" & i)
                    '            strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                    '            Exit For
                    '        End If
                    '    Else
                    '        If strNombreColumna = "" Then
                    '            Throw New Exception("Esta fecha no esta asignada en la programación.")
                    '        End If
                    '    End If
                    'Next
                    For i = 0 To 30
                        If IsDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) Then
                            If CDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                strNombreColumna = ("D" & (i + 1))
                                strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                                Exit For
                            End If
                        Else
                            If strNombreColumna = "" Then
                                Throw New Exception("Esta fecha no esta asignada en la programación.")
                            End If
                        End If
                    Next

                End If
            Next

            Dim strValidaFecha As String() = strValidacion.Split("|")
            If strValidaFecha(0) <> "" Then
                Throw New Exception("Esta fecha ya esta registrada en la programación para este equipo.")
            Else
                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            End If
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub


    Private Sub txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo_TextChanged(sender As Object, e As EventArgs) Handles txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.TextChanged
        Try
            'Validar si existe datos para la planificación
            lblMensajeMantenimientoOrdenTrabajo.Text = ""
            Dim strValidacion As String = ""
            For x = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                If (Session("CestaEquipoContrato").Rows(x)("IdEquipo").ToString.Trim) = hfdIdEquipo.Value.Trim Then
                    Dim strNombreColumna As String = ""
                    'JMUG: 23/07/2025 00:34
                    'For i = 1 To 31
                    '    If IsDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) Then
                    '        If CDate(grdDetalleEquiposFixed.Columns(2 + i).HeaderText) = CDate(txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text) Then
                    '            strNombreColumna = ("D" & i)
                    '            strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                    '            Exit For
                    '        End If
                    '    Else
                    '        If strNombreColumna = "" Then
                    '            Throw New Exception("Esta fecha no esta asignada en la programación.")
                    '        End If
                    '    End If
                    'Next
                    For i = 0 To 30
                        If IsDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) Then
                            If CDate(grdDetalleEquiposScrollable.Columns(i).HeaderText) = CDate(txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                strNombreColumna = ("D" & (i + 1))
                                strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                                Exit For
                            End If
                        Else
                            If strNombreColumna = "" Then
                                Throw New Exception("Esta fecha no esta asignada en la programación.")
                            End If
                        End If
                    Next
                End If
            Next

            Dim strValidaFecha As String() = strValidacion.Split("|")
            If strValidaFecha(0) <> "" Then
                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
                'Throw New Exception("Esta fecha ya esta registrada en la programación para este equipo.")
            Else
                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            End If
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub
    Private Sub grdListaSeleccionarEquipo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdListaSeleccionarEquipo.PageIndexChanging
        grdListaSeleccionarEquipo.PageIndex = e.NewPageIndex
        'Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("ISNULL(EQU.bTieneContratoEquipo, '0') = '0' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
        '                                                         txtBuscarContrato.Text, "0")
        Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("ISNULL(EQU.bTieneContratoEquipo, '0') = '0' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
                                                                 txtBuscarSeleccionarEquipo.Text, "0")
        Me.grdListaSeleccionarEquipo.DataBind() 'Recargo el grid.
        grdListaSeleccionarEquipo.SelectedIndex = -1

        lblMensajeSeleccionarEquipo.Text = ""
        lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboMes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMes.SelectedIndexChanged
        Try
            CargarCestaEquipos()
        Catch ex As Exception

        End Try
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

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            If IsNothing(grdLista.SelectedRow) = False Then
                If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                    hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                End If
                If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                    Throw New Exception("Este registro se encuentra desactivado.")
                End If
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0664", strOpcionModulo, "CMMS")

                Dim dtEstado = EquipoNeg.EquipoGetData("SELECT ISNULL(cEstadoEquipo, 'R') AS cEstadoEquipo FROM LOGI_EQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                If dtEstado.Rows.Count > 0 Then
                    If dtEstado.Rows(0).Item(0) = "R" Then 'REGISTRADO
                        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                    ElseIf dtEstado.Rows(0).Item(0) = "T" Then 'TERMINADO
                        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                    ElseIf dtEstado.Rows(0).Item(0) = "B" Then 'BLOQUEADO
                        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, "0")
                        Me.grdLista.DataBind()
                        Throw New Exception("Este registro se encuentra bloqueado y utilizado por otro usuario.")
                    End If
                End If
                hfdOperacion.Value = "E"
                BloquearMantenimiento(False, True, False, True)
                ValidarTexto(True)
                ActivarObjetos(True)
                LlenarData()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
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

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdContrato.Text = grdLista.SelectedRow.Cells(0).Text + "-" + grdLista.SelectedRow.Cells(1).Text + "-" + grdLista.SelectedRow.Cells(2).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                            lnkbtnVerContrato.Attributes.Add("onclick", "javascript:popupEmitirContratoReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "');")

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

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0666", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
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

    Private Sub btnSiMensajeDocumento_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumento.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'JMUG: 07/09/2025
            'ContratoNeg.ContratoGetData("DELETE LOGI_PLANIFICACIONEQUIPOCONTRATO " &
            '                            "WHERE cIdTipoDocumentoCabeceraContrato + '-' + vIdNumeroSerieCabeceraContrato + '-' + vIdNumeroCorrelativoCabeceraContrato = '" & txtIdContrato.Text & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoDetalleContrato = '" & hfdIdEquipo.Value & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "'")
            ContratoNeg.ContratoGetData("DELETE LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                        "WHERE cIdTipoDocumentoCabeceraContrato + '-' + vIdNumeroSerieCabeceraContrato + '-' + vIdNumeroCorrelativoCabeceraContrato = '" & txtIdContrato.Text & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoDetalleContrato = '" & hfdIdEquipo.Value & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text).ToString("yyyyM") & "'")
            ContratoNeg.ContratoGetData("DELETE LOGI_DETALLECONTRATO " &
                                        "WHERE cIdTipoDocumentoCabeceraContrato + '-' + vIdNumeroSerieCabeceraContrato + '-' + vIdNumeroCorrelativoCabeceraContrato = '" & txtIdContrato.Text & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoDetalleContrato = '" & hfdIdEquipo.Value & "'")

            QuitarCestaEquipo(intIndexContratoEquipo, Session("CestaEquipoContrato"))
            grdDetalleEquiposFixed.DataSource = Session("CestaEquipoContrato")
            grdDetalleEquiposFixed.DataBind()

            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Transacción eliminada con éxito"
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

    Private Sub imgbtnBuscarContrato_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarContrato.Click
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
    End Sub

    Private Sub lnkbtnVerProgramacion_Click(sender As Object, e As EventArgs) Handles lnkbtnVerProgramacion.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdContrato.Text = grdLista.SelectedRow.Cells(0).Text + "-" + grdLista.SelectedRow.Cells(1).Text + "-" + grdLista.SelectedRow.Cells(2).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                            txtFechaInicialImprimirProgramacion.Text = Now.ToString("yyyy-MM-dd")
                            txtFechaFinalImprimirProgramacion.Text = Now.ToString("yyyy-MM-dd")
                            CargarCestaEquiposImprimir()
                            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Seleccione un contrato para ver la programación.")
                    End If
                Else
                    Throw New Exception("Seleccione un contrato.")
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

    Private Sub lnkbtnVerContrato_Click(sender As Object, e As EventArgs) Handles lnkbtnVerContrato.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdContrato.Text = grdLista.SelectedRow.Cells(0).Text + "-" + grdLista.SelectedRow.Cells(1).Text + "-" + grdLista.SelectedRow.Cells(2).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                        End If
                    Else
                        Throw New Exception("Seleccione un contrato para ver los equipos asignados.")
                    End If
                Else
                    Throw New Exception("Seleccione un contrato.")
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

    Private Sub chkTodosImprimirProgramacion_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodosImprimirProgramacion.CheckedChanged
        If chkTodosImprimirProgramacion.Checked = True Then
            Dim x As Integer = 0
            For Each Equipo In Session("CestaEquipoContratoImprimir").rows
                EditarCestaEquipo(True, Equipo("IdEquipo"), Equipo("DescripcionEquipo"), Equipo("Estado"),
                                    "", "", Session("CestaEquipoContratoImprimir"), x)
                x = x + 1
            Next
            grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            grdDetalleEquipoImprimirProgramacion.DataBind()
            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
        Else
            Dim x As Integer = 0
            For Each Equipo In Session("CestaEquipoContratoImprimir").rows
                EditarCestaEquipo(False, Equipo("IdEquipo"), Equipo("DescripcionEquipo"), Equipo("Estado"),
                                    "", "", Session("CestaEquipoContratoImprimir"), x)
                x = x + 1
            Next
            grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            grdDetalleEquipoImprimirProgramacion.DataBind()
            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
        End If

        'Obtener la cadena de equipos
        Dim strEquipo As String = ""
        For Each Equipo In Session("CestaEquipoContratoImprimir").rows
            If Equipo("Seleccionar") = True Then
                strEquipo = strEquipo + Equipo("IdEquipo") & ","
            End If
        Next
        If strEquipo <> "" Then
            strEquipo = If(InStrRev(strEquipo, ",") = Len(strEquipo), Mid(strEquipo, 1, InStrRev(strEquipo, ",") - 1), strEquipo)
        End If
        btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaInicialImprimirProgramacion.Text)) & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaFinalImprimirProgramacion.Text)) & "', '" & strEquipo & "');")
    End Sub

    Private Sub grdDetalleEquipoImprimirProgramacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleEquipoImprimirProgramacion.PageIndexChanging
        grdDetalleEquipoImprimirProgramacion.PageIndex = e.NewPageIndex
        grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
        grdDetalleEquipoImprimirProgramacion.DataBind()
    End Sub

    Private Sub btnAceptarImprimirProgramacion_Click(sender As Object, e As EventArgs) Handles btnAceptarImprimirProgramacion.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Seleccione un contrato para ver la programación.")
                    End If
                Else
                    Throw New Exception("Seleccione un contrato.")
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

    Protected Sub chkRowDetalleEquipo_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Obtener la fila que contiene el CheckBox que se activó/desactivó
        Dim row As GridViewRow = DirectCast(DirectCast(sender, CheckBox).NamingContainer, GridViewRow)

        Dim checkBox As CheckBox = DirectCast(sender, CheckBox)
        Dim FilaActual As Int16
        FilaActual = row.RowIndex - (grdDetalleEquipoImprimirProgramacion.Rows.Count * (grdDetalleEquipoImprimirProgramacion.PageIndex))
        EditarCestaEquipo(checkBox.Checked, Session("CestaEquipoContratoImprimir").Rows(FilaActual)("IdEquipo"), Session("CestaEquipoContratoImprimir").Rows(FilaActual)("DescripcionEquipo"), Session("CestaEquipoContratoImprimir").Rows(FilaActual)("Estado"),
                                    "", "", Session("CestaEquipoContratoImprimir"), FilaActual)

        grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
        grdDetalleEquipoImprimirProgramacion.DataBind()

        Dim strEquipo As String = ""
        For Each Equipo In Session("CestaEquipoContratoImprimir").rows
            If Equipo("Seleccionar") = True Then
                strEquipo = strEquipo + Equipo("IdEquipo") & ","
            End If
        Next
        strEquipo = If(InStrRev(strEquipo, ",") = Len(strEquipo), Mid(strEquipo, 1, InStrRev(strEquipo, ",") - 1), strEquipo)
        btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaInicialImprimirProgramacion.Text)) & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaFinalImprimirProgramacion.Text)) & "', '" & strEquipo & "');")
        lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
    End Sub

    Private Sub cboListadoCheckListMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboListadoCheckListMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub
End Class