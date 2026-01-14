Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarContrato
    Inherits System.Web.UI.Page
    Dim ContratoNeg As New clsContratoNegocios
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

            grdDetalleEquipos_Load(grdDetalleEquipos, New System.EventArgs)

            For Each ListaEquipos In dsEquipos.Rows

                Dim dsPlanificacion = ContratoNeg.ContratoGetData("SELECT nIdNumeroItemDetalleContrato, cIdEquipoDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato, cIdTipoMantenimientoPlanificacionEquipoContrato, " &
                                                                  "       cIdPeriodoMesPlanificacionEquipoContrato, vOrdenTrabajoReferenciaPlanificacionEquipoContrato, vOrdenTrabajoClientePlanificacionEquipoContrato,  " &
                                                                  "       cIdNumeroPlantillaCheckListPlanificacionEquipoContrato " &
                                                                  "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                                                  "WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' " &
                                                                  "      AND nIdNumeroItemDetalleContrato = '" & ListaEquipos("nIdNumeroItemDetalleContrato") & "' AND cIdEquipoDetalleContrato = '" & ListaEquipos("cIdEquipoDetalleContrato") & "'  " &
                                                                  "      AND YEAR(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboPeriodo.SelectedValue & " AND MONTH(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboMes.SelectedValue & " " &
                                                                  "ORDER BY cIdPeriodoMesPlanificacionEquipoContrato, nIdNumeroItemDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato")
                Dim strNombreColumna As String = ""
                Dim strDatosColumna As String = ""
                Dim bValidaIngreso As Boolean = False
                For i = 1 To 31
                    If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                        For Each ListaPlanificacion In dsPlanificacion.Rows

                            If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato")) Then
                                strNombreColumna = strNombreColumna + ("D" & i) & "|"
                                strDatosColumna = strDatosColumna + ListaPlanificacion("cIdTipoMantenimientoPlanificacionEquipoContrato") & "|" & txtIdContrato.Text & "|" & ListaPlanificacion("cIdNumeroPlantillaCheckListPlanificacionEquipoContrato") & "|" & ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoClientePlanificacionEquipoContrato") & "*"
                                bValidaIngreso = True
                            End If
                        Next
                        If bValidaIngreso = False Then
                            strNombreColumna = strNombreColumna + ("D" & i) & "|"
                            strDatosColumna = strDatosColumna + "" & "|" & txtIdContrato.Text & "|" & "" & "|" & CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) & "|" & "" & "|" & "" & "*"
                        End If
                    End If
                Next
                strNombreColumna = Mid(strNombreColumna, 1, strNombreColumna.Length - 1)
                AgregarCestaEquipo(False, ListaEquipos("nIdNumeroItemDetalleContrato"), ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), strNombreColumna, strDatosColumna, Session("CestaEquipoContrato"))
            Next
            Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")

            Me.grdDetalleEquipos.DataBind()

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

    Sub ListarPeriodoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PeriodoNeg As New clsTipoCambioNegocios
        cboPeriodo.DataSource = PeriodoNeg.TipoCambioPeriodoListarCombo
        cboPeriodo.Items.Clear()
        cboPeriodo.DataBind()
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

        cboPeriodo.SelectedValue = Year(IIf(IsNothing(Contrato.dFechaVigenciaFinalCabeceraContrato) Or Contrato.dFechaVigenciaFinalCabeceraContrato > Now, Now, Contrato.dFechaVigenciaFinalCabeceraContrato))
        cboMes.SelectedValue = Month(IIf(IsNothing(Contrato.dFechaVigenciaFinalCabeceraContrato) Or Contrato.dFechaVigenciaFinalCabeceraContrato > Now, Now, Contrato.dFechaVigenciaFinalCabeceraContrato))

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
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(ValoresReferenciales(7).ToString)
        hfdIdEquipo.Value = Equipo.cIdEquipo
        hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
        hfdIdCatalogoEquipo.Value = Equipo.cIdCatalogo
        hfdIdTipoActivoEquipo.Value = Equipo.cIdTipoActivo
        hfdJerarquiaEquipo.Value = Equipo.cIdJerarquiaCatalogo
        txtDescripcionContrato.Text = Equipo.vDescripcionEquipo
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
        lblMensajeMantenimientoOrdenTrabajo.Text = ""
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = "01/" & Format(CInt(cboMes.SelectedValue), "00") & "/" & cboPeriodo.SelectedValue
        txtNroOTClienteMantenimientoOrdenTrabajo.Text = ""
        cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
        VaciarCestaPersonal(Session("CestaSSPersonal"))
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ValidarTextoOrdenTrabajo(ByVal bValidar As Boolean)
        Me.rfvTipoMantenimientoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalResponsableMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalAsignadoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
    End Sub

    Sub ActivarObjetosOrdenTrabajo(ByVal bActivar As Boolean)
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Enabled = bActivar
        cboTipoMantenimientoMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalResponsableMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Enabled = bActivar
    End Sub

    Sub LlenarDataOrdenTrabajo(ByVal ValoresReferenciales() As String)
        Try
            Dim OrdTraNeg As New clsOrdenTrabajoNegocios
            If hfdOperacionDetalle.Value = "N" Then
                hfdIdOrdenTrabajo.Value = ""
                hfdIdOrdenTrabajoCliente.Value = ""
                hfdFechaPlanificacion.Value = ""
            Else
                txtIdContrato.Text = ValoresReferenciales(1) 'IdContrato
                txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = ValoresReferenciales(3).ToString
                txtNroOTClienteMantenimientoOrdenTrabajo.Text = ValoresReferenciales(5).ToString
                hfdFechaPlanificacion.Value = ValoresReferenciales(3)
                hfdIdOrdenTrabajo.Value = ValoresReferenciales(4)
                hfdIdOrdenTrabajoCliente.Value = ValoresReferenciales(5)
            End If
            If txtIdContrato.Text <> "" And hfdIdOrdenTrabajo.Value.Trim.Length <> 0 Then
                Dim strOT = hfdIdOrdenTrabajo.Value.Trim.Split("-")
                Dim OrdenTrabajo As LOGI_CABECERAORDENTRABAJO = OrdTraNeg.OrdenTrabajoListarPorId(strOT(0), strOT(1), strOT(2), Session("IdEmpresa"))
            End If
            LlenarDataEquipo(ValoresReferenciales)
            ListarTipoMantenimientoCombo()
            If hfdOperacionDetalle.Value = "E" Then
                cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue = ValoresReferenciales(0) 'IdTipoMantenimiento
            End If
            ListarPlantillaCheckListCombo()
            If hfdOperacionDetalle.Value = "E" Then
                cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = ValoresReferenciales(2) 'IdPlantilla Check List
            End If
            ListarPersonalResponsableCombo()
            ListarPersonalCombo()
            CargarCestaPersonalFiltrado()
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
        Dim dsContrato = ContratoNeg.ContratoGetData("SELECT vOrdenTrabajoReferenciaPlanificacionEquipoContrato " &
                                                     "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                                     "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "'")
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
            ListarPeriodoCombo()

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
            Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataBind()


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

                                        DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyMM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
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
                                        OrdTra.cIdEmpresa = Session("IdEmpresa")
                                        OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
                                        OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato 'cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
                                        OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(11).Text
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
                        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
                        Me.grdLista.DataBind() 'Recargo el grid.

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

                                        DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyMM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
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
                                        OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato 'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                                        OrdTra.cIdEmpresa = Session("IdEmpresa")
                                        OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
                                        OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato 'cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
                                        OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(11).Text
                                        OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
                                        OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
                                        OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
                                        OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
                                        OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
                                        OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        ColeccionOrdTra.Add(OrdTra)
                                    End If
                                End If
                            End If
                        Next
                    Next

                    If ContratoNeg.ContratoInsertaDetalle(Contrato, ColeccionDetalleContrato, ColeccionPlanificacionContrato, ColeccionOrdTra, Session("CestaPersonalOrdenTrabajo")) = 0 Then
                        txtIdContrato.Text = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                        lblDescripcionMensajeDocumento.Text = "Se modificó el siguiente número de documento"
                        lblNroDocumentoMensajeDocumento.Visible = True
                        btnSiMensajeDocumento.Visible = False
                        btnNoMensajeDocumento.Visible = False
                        lblNroDocumentoMensajeDocumento.Text = txtIdContrato.Text
                        lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        Dim SubFiltro As String = ""
                        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
                        Me.grdLista.DataBind() 'Recargo el grid.

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
        Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("ISNULL(EQU.bTieneContratoEquipo, '0') = '0' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
                                                                 txtBuscarContrato.Text, "0")
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
                            For i = 1 To 31
                                If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                                    strNombreColumna = strNombreColumna & ("D" & i) & "|"
                                    strFechaColumna = strFechaColumna + grdDetalleEquipos.Columns(2 + i).HeaderText + "||*|||"
                                End If
                            Next
                            strNombreColumna = Mid(strNombreColumna, 1, strNombreColumna.Length - 1)
                            AgregarCestaEquipo(False, "0", grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text, grdListaSeleccionarEquipo.SelectedRow.Cells(4).Text, "1", strNombreColumna, strFechaColumna, Session("CestaEquipoContrato"))
                            lblMensajeSeleccionarEquipo.Text = "Equipo asignado satisfactoriamente"
                            grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
                            grdDetalleEquipos.DataBind()
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

    Private Sub grdDetalleEquipos_Load(sender As Object, e As EventArgs) Handles grdDetalleEquipos.Load
        grdDetalleEquipos.Width = 1000
        For i = 0 To 30
            grdDetalleEquipos.Columns(3 + i).HeaderText = i + 1
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
                strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.Parse("01/" & Format(CInt(cboMes.SelectedValue) + 1, "00") & "/" & Format(CInt(cboPeriodo.SelectedValue), "0000")).AddDays(-1))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) < String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Then
                strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.Parse("01/" & Format(CInt(cboMes.SelectedValue) + 1, "00") & "/" & Format(CInt(cboPeriodo.SelectedValue), "0000")).AddDays(-1))
            End If

            Dim dFechaInicial, dFechaFinal As Date
            dFechaInicial = DateTime.ParseExact(strFechaInicial, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
            dFechaFinal = DateTime.ParseExact(strFechaFinal, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
            cboPeriodo.SelectedValue = Year(dFechaInicial)
            cboMes.SelectedValue = Month(dFechaInicial)
            grdDetalleEquipos.Width = 500 + (DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal) * 70)
            For i = 0 To DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal)
                grdDetalleEquipos.Columns(3 + i).HeaderText = dFechaInicial.AddDays(i)
            Next
        End If
        Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
        Me.grdDetalleEquipos.DataBind()
    End Sub

    Private Sub grdDetalleEquipos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleEquipos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Mantenimientos
            e.Row.Cells(1).Visible = True 'IdEquipo
            e.Row.Cells(2).Visible = True 'DescripcionEquipo
            e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquipos.Columns(3).HeaderText) = True, True, False)
            e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquipos.Columns(4).HeaderText) = True, True, False)
            e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquipos.Columns(5).HeaderText) = True, True, False)
            e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquipos.Columns(6).HeaderText) = True, True, False)
            e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquipos.Columns(7).HeaderText) = True, True, False)
            e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquipos.Columns(8).HeaderText) = True, True, False)
            e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquipos.Columns(9).HeaderText) = True, True, False)
            e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquipos.Columns(10).HeaderText) = True, True, False)
            e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquipos.Columns(11).HeaderText) = True, True, False)
            e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquipos.Columns(12).HeaderText) = True, True, False)
            e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquipos.Columns(13).HeaderText) = True, True, False)
            e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquipos.Columns(14).HeaderText) = True, True, False)
            e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquipos.Columns(15).HeaderText) = True, True, False)
            e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquipos.Columns(16).HeaderText) = True, True, False)
            e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquipos.Columns(17).HeaderText) = True, True, False)
            e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquipos.Columns(18).HeaderText) = True, True, False)
            e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquipos.Columns(19).HeaderText) = True, True, False)
            e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquipos.Columns(20).HeaderText) = True, True, False)
            e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquipos.Columns(21).HeaderText) = True, True, False)
            e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquipos.Columns(22).HeaderText) = True, True, False)
            e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquipos.Columns(23).HeaderText) = True, True, False)
            e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquipos.Columns(24).HeaderText) = True, True, False)
            e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquipos.Columns(25).HeaderText) = True, True, False)
            e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquipos.Columns(26).HeaderText) = True, True, False)
            e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquipos.Columns(27).HeaderText) = True, True, False)
            e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquipos.Columns(28).HeaderText) = True, True, False)
            e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquipos.Columns(29).HeaderText) = True, True, False)
            e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquipos.Columns(30).HeaderText) = True, True, False)
            e.Row.Cells(31).Visible = IIf(IsDate(grdDetalleEquipos.Columns(31).HeaderText) = True, True, False)
            e.Row.Cells(32).Visible = IIf(IsDate(grdDetalleEquipos.Columns(32).HeaderText) = True, True, False)
            e.Row.Cells(33).Visible = IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText) = True, True, False)
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Mantenimientos
            e.Row.Cells(1).Visible = True 'IdEquipo
            e.Row.Cells(2).Visible = True 'DescripcionEquipo
            e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquipos.Columns(3).HeaderText) = True, True, False)
            e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquipos.Columns(4).HeaderText) = True, True, False)
            e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquipos.Columns(5).HeaderText) = True, True, False)
            e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquipos.Columns(6).HeaderText) = True, True, False)
            e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquipos.Columns(7).HeaderText) = True, True, False)
            e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquipos.Columns(8).HeaderText) = True, True, False)
            e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquipos.Columns(9).HeaderText) = True, True, False)
            e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquipos.Columns(10).HeaderText) = True, True, False)
            e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquipos.Columns(11).HeaderText) = True, True, False)
            e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquipos.Columns(12).HeaderText) = True, True, False)
            e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquipos.Columns(13).HeaderText) = True, True, False)
            e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquipos.Columns(14).HeaderText) = True, True, False)
            e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquipos.Columns(15).HeaderText) = True, True, False)
            e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquipos.Columns(16).HeaderText) = True, True, False)
            e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquipos.Columns(17).HeaderText) = True, True, False)
            e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquipos.Columns(18).HeaderText) = True, True, False)
            e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquipos.Columns(19).HeaderText) = True, True, False)
            e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquipos.Columns(20).HeaderText) = True, True, False)
            e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquipos.Columns(21).HeaderText) = True, True, False)
            e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquipos.Columns(22).HeaderText) = True, True, False)
            e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquipos.Columns(23).HeaderText) = True, True, False)
            e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquipos.Columns(24).HeaderText) = True, True, False)
            e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquipos.Columns(25).HeaderText) = True, True, False)
            e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquipos.Columns(26).HeaderText) = True, True, False)
            e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquipos.Columns(27).HeaderText) = True, True, False)
            e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquipos.Columns(28).HeaderText) = True, True, False)
            e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquipos.Columns(29).HeaderText) = True, True, False)
            e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquipos.Columns(30).HeaderText) = True, True, False)
            e.Row.Cells(31).Visible = IIf(IsDate(grdDetalleEquipos.Columns(31).HeaderText) = True, True, False)
            e.Row.Cells(32).Visible = IIf(IsDate(grdDetalleEquipos.Columns(32).HeaderText) = True, True, False)
            e.Row.Cells(33).Visible = IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText) = True, True, False)
        End If
    End Sub

    Protected Sub grdDetalleEquipos_RowCommand_Botones(sender As Object, e As GridViewCommandEventArgs)
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
                intIndexContratoEquipo = Valores(6)
                Dim EquipoNeg As New clsEquipoNegocios
                If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & Valores(7).ToString & "'").Rows(0).Item(0) = "0" Then
                    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                End If
                hfdOperacionDetalle.Value = "N"

                LimpiarObjetosOrdenTrabajo()
                ValidarTextoOrdenTrabajo(True)
                ActivarObjetosOrdenTrabajo(True)
                LlenarDataOrdenTrabajo(Valores)
                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ElseIf e.CommandName = "EliminarEquipo" Then
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0670", strOpcionModulo, "CMMS")
                Dim Valores() As String = e.CommandArgument.ToString.Split("|")
                intIndexContratoEquipo = Valores(6)
                hfdIdEquipo.Value = Valores(7)
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
                Dim Valores() As String = e.CommandArgument.ToString.Split("|")
                intIndexContratoEquipo = Valores(6)
                hfdIdEquipo.Value = Valores(7)
                hfdOperacionDetalle.Value = "E"

                LimpiarObjetosOrdenTrabajo()
                ValidarTextoOrdenTrabajo(True)
                ActivarObjetosOrdenTrabajo(True)

                LlenarDataOrdenTrabajo(Valores)

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

            Dim gvRow As GridViewRow = grdDetalleEquipos.Rows(intIndexContratoEquipo - ((grdDetalleEquipos.PageSize) * (grdDetalleEquipos.PageIndex))) 'JMUG: 19/05/2022
            Dim strNombreColumnaModificar As String = ""
            Dim strNombreColumna As String = ""
            For i = 1 To 31
                If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                    If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                        strNombreColumna = ("D" & i)
                    End If
                    If IsDate(hfdFechaPlanificacion.Value) AndAlso CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(hfdFechaPlanificacion.Value) Then
                        strNombreColumnaModificar = ("D" & i)
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
                                      strNombreColumna, cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "|" & txtIdContrato.Text & "|" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "|" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text & "|" & hfdIdOrdenTrabajo.Value & "|" & txtNroOTClienteMantenimientoOrdenTrabajo.Text, Session("CestaEquipoContrato"), intIndexContratoEquipo)
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
            Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
            Me.grdDetalleEquipos.DataBind()
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
                    For i = 1 To 31
                        If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                            If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                strNombreColumna = ("D" & i)
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

    Private Sub cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndexChanged
        ListarPlantillaCheckListCombo()
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboPersonalAsignadoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub grdDetalleEquipos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleEquipos.PageIndexChanging
        grdDetalleEquipos.PageIndex = e.NewPageIndex
        Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
        Me.grdDetalleEquipos.DataBind()
        grdDetalleEquipos.SelectedIndex = -1
    End Sub

    Private Sub txtFechaInicioPlanificadaMantenimientoOrdenTrabajo_TextChanged(sender As Object, e As EventArgs) Handles txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.TextChanged
        Try
            'Validar si existe datos para la planificación
            lblMensajeMantenimientoOrdenTrabajo.Text = ""
            Dim strValidacion As String = ""
            For x = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                If (Session("CestaEquipoContrato").Rows(x)("IdEquipo").ToString.Trim) = hfdIdEquipo.Value.Trim Then
                    Dim strNombreColumna As String = ""
                    For i = 1 To 31
                        If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                            If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                strNombreColumna = ("D" & i)
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

    Private Sub grdListaSeleccionarEquipo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdListaSeleccionarEquipo.PageIndexChanging
        grdListaSeleccionarEquipo.PageIndex = e.NewPageIndex
        Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("ISNULL(EQU.bTieneContratoEquipo, '0') = '0' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
                                                                 txtBuscarContrato.Text, "0")
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
        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        Me.grdLista.DataBind() 'Recargo el grid.
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

            ContratoNeg.ContratoGetData("DELETE LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                        "WHERE cIdTipoDocumentoCabeceraContrato + '-' + vIdNumeroSerieCabeceraContrato + '-' + vIdNumeroCorrelativoCabeceraContrato = '" & txtIdContrato.Text & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoDetalleContrato = '" & hfdIdEquipo.Value & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "'")
            ContratoNeg.ContratoGetData("DELETE LOGI_DETALLECONTRATO " &
                                        "WHERE cIdTipoDocumentoCabeceraContrato + '-' + vIdNumeroSerieCabeceraContrato + '-' + vIdNumeroCorrelativoCabeceraContrato = '" & txtIdContrato.Text & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoDetalleContrato = '" & hfdIdEquipo.Value & "'")

            QuitarCestaEquipo(intIndexContratoEquipo, Session("CestaEquipoContrato"))
            grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
            grdDetalleEquipos.DataBind()

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

        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        Me.grdLista.DataBind() 'Recargo el grid.
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