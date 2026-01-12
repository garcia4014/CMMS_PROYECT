Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmBienvenida
    Inherits System.Web.UI.Page

    Shared Function CrearCestaOrdenTrabajo() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("value", GetType(System.Decimal))) '1
        dt.Columns.Add(New DataColumn("name", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("cantidad", GetType(System.String))) '2
        Return dt
    End Function

    Shared Sub EditarCestaOrdenTrabajo(ByVal Porcentaje As Decimal, ByVal Descripcion As String, ByVal Cantidad As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Porcentaje
                Tabla.Rows(Fila)(1) = Descripcion
                Tabla.Rows(Fila)(2) = Cantidad
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaOrdenTrabajo(ByVal Porcentaje As String, ByVal Descripcion As String, ByVal Cantidad As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("value") = Porcentaje
        Fila("name") = Descripcion
        Fila("cantidad") = Cantidad
        Tabla.Rows.Add(Fila)
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

    Function fAgregarTemporal(ByVal strTipo As String, ByVal dt As DataTable, ByVal dtSession As DataTable) As String
        If strTipo = "Semana" Then
            If Session("CestaOTWeek") Is Nothing Then
                Session("CestaOTWeek") = CrearCestaOrdenTrabajo()
            Else
                VaciarCestaOrdenTrabajo(Session("CestaOTWeek"))
            End If
            Dim IngresoOK As String = ""
            For Each OTWeek In dt.Rows
                If OTWeek("name") = "Registrado" Then
                    AgregarCestaOrdenTrabajo(OTWeek("value"), OTWeek("name"), OTWeek("cantidad"), Session("CestaOTWeek"))
                    IngresoOK = "R"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "Registrado", "0", Session("CestaOTWeek"))
                IngresoOK = ""
            End If
            IngresoOK = ""
            For Each OTWeek In dt.Rows
                If OTWeek("name") = "En Proceso" Then
                    AgregarCestaOrdenTrabajo(OTWeek("value"), OTWeek("name"), OTWeek("cantidad"), Session("CestaOTWeek"))
                    IngresoOK = "P"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "En Proceso", "0", Session("CestaOTWeek"))
                IngresoOK = ""
            End If
            IngresoOK = ""
            For Each OTWeek In dt.Rows
                If OTWeek("name") = "Finalizado" Then
                    AgregarCestaOrdenTrabajo(OTWeek("value"), OTWeek("name"), OTWeek("cantidad"), Session("CestaOTWeek"))
                    IngresoOK = "T"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "Finalizado", "0", Session("CestaOTWeek"))
                IngresoOK = ""
            End If
            Dim FuncionesNeg As New clsFuncionesNegocios
            Return FuncionesNeg.GetDSJSon(Session("CestaOTWeek"), "OrdenTrabajoWeek")
        End If
        If strTipo = "Mensual" Then
            If Session("CestaOTMonth") Is Nothing Then
                Session("CestaOTMonth") = CrearCestaOrdenTrabajo()
            Else
                VaciarCestaOrdenTrabajo(Session("CestaOTMonth"))
            End If
            Dim IngresoOK As String = ""
            For Each OTMonth In dt.Rows
                If OTMonth("name") = "Registrado" Then
                    AgregarCestaOrdenTrabajo(OTMonth("value"), OTMonth("name"), OTMonth("cantidad"), Session("CestaOTMonth"))
                    IngresoOK = "R"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "Registrado", "0", Session("CestaOTMonth"))
                IngresoOK = ""
            End If
            IngresoOK = ""
            For Each OTMonth In dt.Rows
                If OTMonth("name") = "En Proceso" Then
                    AgregarCestaOrdenTrabajo(OTMonth("value"), OTMonth("name"), OTMonth("cantidad"), Session("CestaOTMonth"))
                    IngresoOK = "P"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "En Proceso", "0", Session("CestaOTMonth"))
                IngresoOK = ""
            End If
            IngresoOK = ""
            For Each OTMonth In dt.Rows
                If OTMonth("name") = "Finalizado" Then
                    AgregarCestaOrdenTrabajo(OTMonth("value"), OTMonth("name"), OTMonth("cantidad"), Session("CestaOTMonth"))
                    IngresoOK = "T"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "Finalizado", "0", Session("CestaOTMonth"))
                IngresoOK = ""
            End If
            Dim FuncionesNeg As New clsFuncionesNegocios
            Return FuncionesNeg.GetDSJSon(Session("CestaOTMonth"), "OrdenTrabajoMonth")
        End If
        If strTipo = "Anual" Then
            If Session("CestaOTYear") Is Nothing Then
                Session("CestaOTYear") = CrearCestaOrdenTrabajo()
            Else
                VaciarCestaOrdenTrabajo(Session("CestaOTYear"))
            End If
            Dim IngresoOK As String = ""
            For Each OTYear In dt.Rows
                If OTYear("name") = "Registrado" Then
                    AgregarCestaOrdenTrabajo(OTYear("value"), OTYear("name"), OTYear("cantidad"), Session("CestaOTYear"))
                    IngresoOK = "R"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "Registrado", "0", Session("CestaOTYear"))
                IngresoOK = ""
            End If
            IngresoOK = ""
            For Each OTYear In dt.Rows
                If OTYear("name") = "En Proceso" Then
                    AgregarCestaOrdenTrabajo(OTYear("value"), OTYear("name"), OTYear("cantidad"), Session("CestaOTYear"))
                    IngresoOK = "P"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "En Proceso", "0", Session("CestaOTYear"))
                IngresoOK = ""
            End If
            IngresoOK = ""
            For Each OTYear In dt.Rows
                If OTYear("name") = "Finalizado" Then
                    AgregarCestaOrdenTrabajo(OTYear("value"), OTYear("name"), OTYear("cantidad"), Session("CestaOTYear"))
                    IngresoOK = "T"
                End If
            Next
            If IngresoOK = "" Then
                AgregarCestaOrdenTrabajo("0.00", "Finalizado", "0", Session("CestaOTYear"))
                IngresoOK = ""
            End If
            Dim FuncionesNeg As New clsFuncionesNegocios
            Return FuncionesNeg.GetDSJSon(Session("CestaOTYear"), "OrdenTrabajoYear")
        End If
    End Function

    Function fAgregarCuadroEstadistico2(ByVal Periodo As String)
        Dim strSQL = ""
        Dim nMes = 1
        strSQL = ""
        strSQL = strSQL + "SELECT 'N' AS cTipo, '12' AS nMes, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "01' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "01' THEN 1 ELSE 0 END) AS D1, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "02' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "04' THEN 1 ELSE 0 END) AS D4, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "05' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "07' THEN 1 ELSE 0 END) AS D7, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "08' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "10' THEN 1 ELSE 0 END) AS D10, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "11' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "13' THEN 1 ELSE 0 END) AS D13, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "14' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "16' THEN 1 ELSE 0 END) AS D16, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "17' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "19' THEN 1 ELSE 0 END) AS D19, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "20' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "22' THEN 1 ELSE 0 END) AS D22, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "23' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "25' THEN 1 ELSE 0 END) AS D25, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "26' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "28' THEN 1 ELSE 0 END) AS D28, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & CStr(CInt(Periodo) - 1) & "12" & "29' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & CStr(CInt(Periodo) - 1) & "12" & "31' THEN 1 ELSE 0 END) AS D31 " &
                 "FROM LOGI_CABECERAORDENTRABAJO " &
                 "WHERE cEstadoCabeceraOrdenTrabajo <> 'X' "
        strSQL = strSQL + "UNION "

        Do While nMes <= 12
            strSQL = strSQL + "SELECT 'N' AS cTipo, '" & Format(nMes, "00") & "' AS nMes, SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "01' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "01' THEN 1 ELSE 0 END) AS D1, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "02' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "04' THEN 1 ELSE 0 END) AS D4, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "05' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "07' THEN 1 ELSE 0 END) AS D7, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "08' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "10' THEN 1 ELSE 0 END) AS D10, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "11' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "13' THEN 1 ELSE 0 END) AS D13, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "14' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "16' THEN 1 ELSE 0 END) AS D16, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "17' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "19' THEN 1 ELSE 0 END) AS D19, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "20' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "22' THEN 1 ELSE 0 END) AS D22, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "23' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "25' THEN 1 ELSE 0 END) AS D25, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "26' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "28' THEN 1 ELSE 0 END) AS D28, " &
                 "       SUM(CASE WHEN CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) >= '" & Periodo & Format(nMes, "00") & "29' AND CONVERT(CHAR(8), dFechaEmisionCabeceraOrdenTrabajo, 112) <= '" & Periodo & Format(nMes, "00") & "31' THEN 1 ELSE 0 END) AS D31 " &
                 "FROM LOGI_CABECERAORDENTRABAJO " &
                 "WHERE cEstadoCabeceraOrdenTrabajo <> 'X' " & IIf(nMes = 12, "ORDER BY nMes, cTipo", "UNION ")
            nMes = nMes + 1
        Loop
        Dim FuncionesNeg As New clsFuncionesNegocios
        Dim dtDatosOrdenTrabajo = FuncionesNeg.FuncionesGetData(strSQL)
        Dim strCadena As String
        strCadena = "["
        For Each Fila In dtDatosOrdenTrabajo.Rows
            strCadena = strCadena & "[" & Fila("D1") & "," & Fila("D4") & "," & Fila("D7") & "," & Fila("D10") & "," & Fila("D13") & "," & Fila("D16") & "," & Fila("D19") & "," & Fila("D22") & "," & Fila("D25") & "," & Fila("D28") & "," & Fila("D31") & IIf(Fila("nMes") = "12", "]", "], ")
        Next
        strCadena = strCadena & "]"
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientVariable4", "var strOrdTrabajoNuevaTerminada = " & strCadena & ";", True)
    End Function

    Sub ResumenDashBoard()
        Dim FuncionesNeg As New clsFuncionesNegocios

        Dim SubFiltro As String = ""
        'SubFiltro = " AND (DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
        '                "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
        '                "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
        '                "INNER JOIN GNRL_USUARIO AS USU ON " &
        '                "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
        '                "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"
        'Me.grdLista.DataSource = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("(cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdUsuario") & "' AND (cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "')) OR ((cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = '' AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", ")", SubFiltro), "1")
        'Me.grdLista.DataBind()
        SubFiltro = " AND cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdUsuario") & "' OR (DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        "INNER JOIN GNRL_USUARIO AS USU ON " &
                        "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "'))"



        'Dim strSQL = ""
        'strSQL = "SELECT COUNT(*) AS nCantidad, " &
        '         "        CASE WHEN cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' " &
        '         "        ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' " &
        '         "             ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' " &
        '         "                  ELSE 'Anulado' END END END AS vDescripcion " &
        '         "FROM LOGI_CABECERAORDENTRABAJO " &
        '         "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT'" &
        '         "      AND cEstadoCabeceraOrdenTrabajo <> 'X' " &
        '         "GROUP BY cEstadoCabeceraOrdenTrabajo"
        Dim strSQL = ""
        'strSQL = "SELECT COUNT(*) AS nCantidad, " &
        '         "        CASE WHEN DOC.cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' " &
        '         "        ELSE CASE WHEN DOC.cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' " &
        '         "             ELSE CASE WHEN DOC.cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' " &
        '         "                  ELSE 'Anulado' END END END AS vDescripcion " &
        '         "FROM LOGI_CABECERAORDENTRABAJO AS DOC " &
        '         "WHERE DOC.cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT'" &
        '         "      AND DOC.cEstadoCabeceraOrdenTrabajo <> 'X' " &
        '         "      AND (cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdUsuario") & "') " &
        '         "" & IIf(Session("IdTipoUsuario") = "A", "", SubFiltro) & " " &
        '         "GROUP BY DOC.cEstadoCabeceraOrdenTrabajo"
        strSQL = "SELECT COUNT(*) AS nCantidad, " &
                 "        CASE WHEN DOC.cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' " &
                 "        ELSE CASE WHEN DOC.cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' " &
                 "             ELSE CASE WHEN DOC.cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' " &
                 "                  ELSE 'Anulado' END END END AS vDescripcion " &
                 "FROM LOGI_CABECERAORDENTRABAJO AS DOC " &
                 "WHERE DOC.cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT'" &
                 "      AND DOC.cEstadoCabeceraOrdenTrabajo <> 'X' " &
                 "" & IIf(Session("IdTipoUsuario") = "A", "", SubFiltro) & " " &
                 "GROUP BY DOC.cEstadoCabeceraOrdenTrabajo"

        'Me.grdLista.DataSource = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("(cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdUsuario") & "' AND (cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "')) OR ((cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = '' AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", ")", SubFiltro), "1")


        Dim dtDatosOrdenTrabajoDashBoard = FuncionesNeg.FuncionesGetData(strSQL)
        lblOrdTraRegistrada.Text = "0"
        lblOrdTraEnProceso.Text = "0"
        lblOrdTraFinalizada.Text = "0"
        lblTareasPendientes.Text = "0"
        For Each Fila In dtDatosOrdenTrabajoDashBoard.Rows
            If Fila("vDescripcion") = "Registrado" Then
                lblOrdTraRegistrada.Text = Fila("nCantidad")
            ElseIf Fila("vDescripcion") = "En Proceso" Then
                lblOrdTraEnProceso.Text = Fila("nCantidad")
            ElseIf Fila("vDescripcion") = "Finalizado" Then
                lblOrdTraFinalizada.Text = Fila("nCantidad")
            End If
        Next
        strSQL = "SELECT COUNT(*) AS nCantidad " &
                 "FROM LOGI_CHECKLISTORDENTRABAJO " &
                 "WHERE cEstadoCheckListOrdenTrabajo = 'P'"
        Dim dtTareasPedientesDashBoard = FuncionesNeg.FuncionesGetData(strSQL)
        For Each Fila In dtTareasPedientesDashBoard.Rows
            lblTareasPendientes.Text = Fila("nCantidad")
        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim FuncionesNeg As New clsFuncionesNegocios
        Dim strSQL = ""
        strSQL = "SELECT ROUND(100 * TMP.value / (SELECT CAST(SUM(TMP.value) AS Decimal) FROM " &
                 "(SELECT COUNT(*) AS value, CASE WHEN cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' ELSE 'Anulado' END END END AS name " &
                 "   FROM LOGI_CABECERAORDENTRABAJO " &
                 "   WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' " &
                 "         AND DATEDIFF(DD, dFechaEmisionCabeceraOrdenTrabajo, DATEADD (DAY, -7, GETDATE())) <= 0 " &
                 "GROUP BY cEstadoCabeceraOrdenTrabajo) AS TMP), 2) AS value, TMP.name, TMP.value AS cantidad FROM " &
                 "(SELECT COUNT(*) AS value, CASE WHEN cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' ELSE 'Anulado' END END END AS name " &
                 "FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' " &
                 "     AND DATEDIFF(DD, dFechaEmisionCabeceraOrdenTrabajo, DATEADD (DAY, -7, GETDATE())) <= 0 " &
                 "GROUP BY cEstadoCabeceraOrdenTrabajo) AS TMP"
        Dim dtDatosOrdenTrabajoWeek = FuncionesNeg.FuncionesGetData(strSQL)
        Dim strOrdenTrabajoWeek As String = ""
        strOrdenTrabajoWeek = fAgregarTemporal("Semana", dtDatosOrdenTrabajoWeek, Session("CestaOTWeek"))
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientVariable1", "var jsondataOrdenTrabajoWeek = " & strOrdenTrabajoWeek & ";", True)
        Dim strFiltro, strFiltro2 As String
        strFiltro = "" : strFiltro2 = ""
        For Each Fila As DataRow In Session("CestaOTWeek").rows
            If Fila("name") = "Registrado" Then
                lblPorcentajeOTRegistradas.Text = Math.Round(Fila("value"), 2) & "%"
                lblCantidadOTRegistradas.Text = Fila("cantidad")
                strFiltro = Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = Fila("cantidad")
            ElseIf Fila("name") = "En Proceso" Then
                lblPorcentajeOTEnProceso.Text = Math.Round(Fila("value"), 2) & "%"
                lblCantidadOTEnProceso.Text = Fila("cantidad")
                strFiltro = strFiltro & "', '" & Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = strFiltro2 & "', '" & Fila("cantidad")
            ElseIf Fila("name") = "Finalizado" Then
                lblPorcentajeOTFinalizadas.Text = Math.Round(Fila("value"), 2) & "%"
                lblCantidadOTFinalizadas.Text = Fila("cantidad")
                strFiltro = strFiltro & "', '" & Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = strFiltro2 & "', '" & Fila("cantidad")
            End If
        Next
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientstrData1", "var strData1 = ['" & strFiltro & "'];", True)
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientstrData4", "var strData4 = ['" & strFiltro2 & "'];", True)

        strSQL = "SELECT ROUND(100 * TMP.value / (SELECT CAST(SUM(TMP.value) AS DECIMAL) FROM " &
                 "(SELECT COUNT(*) AS value, CASE WHEN cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' ELSE 'Anulado' END END END AS name " &
                 "   FROM LOGI_CABECERAORDENTRABAJO " &
                 "   WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' " &
                 "         AND DATEDIFF(MM, dFechaEmisionCabeceraOrdenTrabajo, GETDATE()) = 0 " &
                 "GROUP BY cEstadoCabeceraOrdenTrabajo) AS TMP), 2) AS value, TMP.name, TMP.value AS cantidad FROM " &
                 "(SELECT COUNT(*) AS value, CASE WHEN cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' ELSE 'Anulado' END END END AS name " &
                 "FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' " &
                 "     AND DATEDIFF(MM, dFechaEmisionCabeceraOrdenTrabajo, GETDATE()) = 0 " &
                 "GROUP BY cEstadoCabeceraOrdenTrabajo) AS TMP"
        Dim dtDatosOrdenTrabajoMonth = FuncionesNeg.FuncionesGetData(strSQL)
        Dim strOrdenTrabajoMonth As String = ""
        strOrdenTrabajoMonth = fAgregarTemporal("Mensual", dtDatosOrdenTrabajoMonth, Session("CestaOTMonth"))
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientVariable2", "var jsondataOrdenTrabajoMonth = " & strOrdenTrabajoMonth & ";", True)
        strFiltro = "" : strFiltro2 = ""
        For Each Fila As DataRow In Session("CestaOTMonth").rows
            If Fila("name") = "Registrado" Then
                strFiltro = Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = Fila("cantidad")
            ElseIf Fila("name") = "En Proceso" Then
                strFiltro = strFiltro & "', '" & Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = strFiltro2 & "', '" & Fila("cantidad")
            ElseIf Fila("name") = "Finalizado" Then
                strFiltro = strFiltro & "', '" & Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = strFiltro2 & "', '" & Fila("cantidad")
            End If
        Next
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientstrData2", "var strData2 = ['" & strFiltro & "'];", True)
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientstrData5", "var strData5 = ['" & strFiltro2 & "'];", True)

        strSQL = "SELECT ROUND(100 * TMP.value / (SELECT CAST(SUM(TMP.value) AS DECIMAL) FROM " &
                 "(SELECT COUNT(*) AS value, CASE WHEN cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' ELSE 'Anulado' END END END AS name " &
                 "   FROM LOGI_CABECERAORDENTRABAJO " &
                 "   WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' " &
                 "         AND DATEDIFF(YYYY, dFechaEmisionCabeceraOrdenTrabajo, GETDATE()) = 0 " &
                 "GROUP BY cEstadoCabeceraOrdenTrabajo) AS TMP), 2) AS value, TMP.name, TMP.value AS cantidad FROM " &
                 "(SELECT COUNT(*) AS value, CASE WHEN cEstadoCabeceraOrdenTrabajo = 'R' THEN 'Registrado' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'P' THEN 'En Proceso' ELSE CASE WHEN cEstadoCabeceraOrdenTrabajo = 'T' THEN 'Finalizado' ELSE 'Anulado' END END END AS name " &
                 "FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' " &
                 "     AND DATEDIFF(YYYY, dFechaEmisionCabeceraOrdenTrabajo, GETDATE()) = 0 " &
                 "GROUP BY cEstadoCabeceraOrdenTrabajo) AS TMP"
        Dim dtDatosOrdenTrabajoYear = FuncionesNeg.FuncionesGetData(strSQL)
        Dim strOrdenTrabajoYear As String = ""
        strOrdenTrabajoYear = fAgregarTemporal("Anual", dtDatosOrdenTrabajoYear, Session("CestaOTYear"))
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientVariable3", "var jsondataOrdenTrabajoYear = " & strOrdenTrabajoYear & ";", True)
        strFiltro = "" : strFiltro2 = ""
        For Each Fila As DataRow In Session("CestaOTYear").rows
            If Fila("name") = "Registrado" Then
                strFiltro = Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = Fila("cantidad")
            ElseIf Fila("name") = "En Proceso" Then
                strFiltro = strFiltro & "', '" & Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = strFiltro2 & "', '" & Fila("cantidad")
            ElseIf Fila("name") = "Finalizado" Then
                strFiltro = strFiltro & "', '" & Math.Round(Fila("value"), 2) & "%"
                strFiltro2 = strFiltro2 & "', '" & Fila("cantidad")
            End If
        Next
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientstrData3", "var strData3 = ['" & strFiltro & "'];", True)
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "initMyClientstrData6", "var strData6 = ['" & strFiltro2 & "'];", True)

        ResumenDashBoard()
        fAgregarCuadroEstadistico2(Year(Now).ToString)
    End Sub

    Private Sub frmBienvenida_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub
End Class