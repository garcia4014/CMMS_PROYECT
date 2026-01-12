Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsOtrosDatosOrdenTrabajoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function OtrosDatosOrdenTrabajoGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        Dim constr As String = My.Settings.CMMSConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strQuery)
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    sda.Fill(dt)
                End Using
            End Using
            Return dt
        End Using
    End Function

    'Public Function OtrosDatosOrdenTrabajoListarCombo() As List(Of LOGI_OTROSDATOSORDENTRABAJO)
    '    Dim Consulta = Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_OTROSDATOSORDENTRABAJO",
    '                                                  0, "", "", "", "", "", "", "", " ", "", "", "", "", "", "1", Now, "")
    '    Dim Coleccion As New List(Of LOGI_OTROSDATOSORDENTRABAJO)
    '    For Each OtrosDatosOrdenTrabajo In Consulta
    '        Dim TipAct As New LOGI_OTROSDATOSORDENTRABAJO
    '        TipAct.cIdCheckList = OtrosDatosOrdenTrabajo.cIdCheckList
    '        TipAct.vDescripcionOtrosDatosOrdenTrabajo = OtrosDatosOrdenTrabajo.vDescripcionOtrosDatosOrdenTrabajo
    '        Coleccion.Add(TipAct)
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function OtrosDatosOrdenTrabajoListarPorId(ByVal IdOtrosDatosOrdenTrabajo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_OTROSDATOSORDENTRABAJO
    Public Function OtrosDatosOrdenTrabajoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdNroItem As String) As LOGI_OTROSDATOSORDENTRABAJO
        Dim Consulta = Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_OTROSDATOSORDENTRABAJO " &
                                                                     "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND nIdNumeroItemOtrosDatosOrdenTrabajoOrdenTrabajo = " & IdNroItem &
                                                                     " AND cIdEmpresa = '" & IdEmpresa & "'",
                                                                     "", "", "", "", 0, "", "", "", "", " ", "", "", "", "")
        Dim Coleccion As New LOGI_OTROSDATOSORDENTRABAJO
        For Each LOGI_OTROSDATOSORDENTRABAJO In Consulta
            Coleccion.nIdNumeroItemOtrosDatosOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.nIdNumeroItemOtrosDatosOrdenTrabajo
            Coleccion.cIdTipoDocumentoCabeceraOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.cIdTipoDocumentoCabeceraOrdenTrabajo
            Coleccion.vIdNumeroSerieCabeceraOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.vIdNumeroSerieCabeceraOrdenTrabajo
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.vIdNumeroCorrelativoCabeceraOrdenTrabajo
            Coleccion.cIdEmpresa = LOGI_OTROSDATOSORDENTRABAJO.cIdEmpresa
            Coleccion.cIdEquipoCabeceraOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.cIdEquipoCabeceraOrdenTrabajo
            Coleccion.cIdActividadCheckListOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.cIdActividadCheckListOrdenTrabajo
            Coleccion.cIdCatalogoCheckListOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.cIdCatalogoCheckListOrdenTrabajo
            Coleccion.cIdJerarquiaCatalogoCheckListOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.cIdJerarquiaCatalogoCheckListOrdenTrabajo
            Coleccion.vIdArticuloSAPCabeceraOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.vIdArticuloSAPCabeceraOrdenTrabajo
            Coleccion.cIdEquipoCheckListOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.cIdEquipoCheckListOrdenTrabajo
            Coleccion.cIdCaracteristica = LOGI_OTROSDATOSORDENTRABAJO.cIdCaracteristica
            Coleccion.vValorReferencialOtrosDatosOrdenTrabajo = LOGI_OTROSDATOSORDENTRABAJO.vValorReferencialOtrosDatosOrdenTrabajo
        Next
        Return Coleccion
    End Function

    'Public Function OtrosDatosOrdenTrabajoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_OTROSDATOSORDENTRABAJO)
    Public Function OtrosDatosOrdenTrabajoListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_LOGI_OTROSDATOSORDENTRABAJO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_NONE", "SELECT OTRDATORDTRA.*, CAR.vDescripcionCaracteristica " &
                                                               "FROM LOGI_OTROSDATOSORDENTRABAJO AS OTRDATORDTRA INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
                                                               "" &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%')",
                                                   "", "", "", "", 0, "", "", "", "", " ", "", "", "", "")
        Dim Coleccion As New List(Of VI_LOGI_OTROSDATOSORDENTRABAJO)
        For Each Busqueda In Consulta
            Dim BuscarChkLst As New VI_LOGI_OTROSDATOSORDENTRABAJO
            BuscarChkLst.IdNumeroItem = Busqueda.nIdNumeroItemOtrosDatosOrdenTrabajo
            BuscarChkLst.IdCaracteristica = Busqueda.cIdCaracteristica
            BuscarChkLst.DescripcionCaracteristica = Busqueda.vDescripcionCaracteristica
            BuscarChkLst.ValorReferencial = Busqueda.vValorReferencialOtrosDatosOrdenTrabajo
            BuscarChkLst.IdActividadCheckList = Busqueda.cIdActividadCheckListOrdenTrabajo
            BuscarChkLst.IdEquipoCheckList = Busqueda.cIdEquipoCheckListOrdenTrabajo
            BuscarChkLst.IdEquipo = Busqueda.cIdEquipoCabeceraOrdenTrabajo
            BuscarChkLst.IdActividadCheckList = Busqueda.cIdActividadCheckListOrdenTrabajo
            BuscarChkLst.IdArticuloSAP = Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo
            BuscarChkLst.IdCatalogoCheckList = Busqueda.cIdCatalogoCheckListOrdenTrabajo
            BuscarChkLst.IdJerarquiaCatalogoCheckList = Busqueda.cIdJerarquiaCatalogoCheckListOrdenTrabajo
            Coleccion.Add(BuscarChkLst)
        Next
        Return Coleccion
    End Function

    Public Function OtrosDatosOrdenTrabajoInserta(ByVal OtrosDatosOrdenTrabajo As LOGI_OTROSDATOSORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_INSERT", "", OtrosDatosOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdEmpresa,
                                                          OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo,
                                                          OtrosDatosOrdenTrabajo.cIdCatalogoCheckListOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdJerarquiaCatalogoCheckListOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdActividadCheckListOrdenTrabajo,
                                                          OtrosDatosOrdenTrabajo.cIdEquipoCheckListOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdCaracteristica, OtrosDatosOrdenTrabajo.vValorReferencialOtrosDatosOrdenTrabajo,
                                                          OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function OtrosDatosOrdenTrabajoInsertaDetalle(ByVal DetalleOtrosDatosOrdenTrabajo As List(Of LOGI_OTROSDATOSORDENTRABAJO), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_NONE", "DELETE LOGI_OTROSDATOSORDENTRABAJO " &
                                         "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).cIdTipoDocumentoCabeceraOrdenTrabajo & "' " &
                                         "      AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).vIdNumeroSerieCabeceraOrdenTrabajo & "' " &
                                         "      AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' " &
                                         "      AND cIdEmpresa = '" & DetalleOtrosDatosOrdenTrabajo(0).cIdEmpresa & "' " &
                                         "      AND cIdEquipoCabeceraOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).cIdEquipoCabeceraOrdenTrabajo & "' " &
                                         "      AND cIdActividadCheckListOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).cIdActividadCheckListOrdenTrabajo & "' " &
                                         "      AND cIdCatalogoCheckListOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).cIdCatalogoCheckListOrdenTrabajo & "' " &
                                         "      AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).cIdJerarquiaCatalogoCheckListOrdenTrabajo & "'" &
                                         "      AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).vIdArticuloSAPCabeceraOrdenTrabajo & "'" &
                                         "      AND cIdEquipoCheckListOrdenTrabajo = '" & DetalleOtrosDatosOrdenTrabajo(0).cIdEquipoCheckListOrdenTrabajo & "'",
                                         "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "").ReturnValue.ToString
            For Each Busqueda In DetalleOtrosDatosOrdenTrabajo
                'If bExiste = False Then
                x = Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_INSERT", "", Busqueda.cIdTipoDocumentoCabeceraOrdenTrabajo, Busqueda.vIdNumeroSerieCabeceraOrdenTrabajo, Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo, Busqueda.cIdEmpresa,
                                                          Busqueda.nIdNumeroItemOtrosDatosOrdenTrabajo, Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda.cIdEquipoCabeceraOrdenTrabajo,
                                                          Busqueda.cIdCatalogoCheckListOrdenTrabajo, Busqueda.cIdJerarquiaCatalogoCheckListOrdenTrabajo, Busqueda.cIdActividadCheckListOrdenTrabajo,
                                                          Busqueda.cIdEquipoCheckListOrdenTrabajo, Busqueda.cIdCaracteristica, Busqueda.vValorReferencialOtrosDatosOrdenTrabajo,
                                                          Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString

                LogAuditoria.vEvento = "INSERTAR OTROS DATOS ORDEN TRABAJO"
                LogAuditoria.vQuery = "PA_LOGI_MNT_OTROSDATOSORDENTRABAJO 'SQL_INSERT', '', '" & Busqueda.cIdTipoDocumentoCabeceraOrdenTrabajo & "', '" & Busqueda.vIdNumeroSerieCabeceraOrdenTrabajo & "', '" & Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "', '" & Busqueda.cIdEmpresa & "', " &
                                                          Busqueda.nIdNumeroItemOtrosDatosOrdenTrabajo & ", '" & Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo & "', '" & Busqueda.cIdEquipoCabeceraOrdenTrabajo & "', '" &
                                                          Busqueda.cIdCatalogoCheckListOrdenTrabajo & "', '" & Busqueda.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "', '" & Busqueda.cIdActividadCheckListOrdenTrabajo & "', '" &
                                                          Busqueda.cIdEquipoCheckListOrdenTrabajo & "', '" & Busqueda.cIdCaracteristica & "', '" & Busqueda.vValorReferencialOtrosDatosOrdenTrabajo & "', '" &
                                                          Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "'"

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function OtrosDatosOrdenTrabajoEdita(ByVal OtrosDatosOrdenTrabajo As LOGI_OTROSDATOSORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_UPDATE", "", OtrosDatosOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdEmpresa,
                                                          OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo,
                                                          OtrosDatosOrdenTrabajo.cIdCatalogoCheckListOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdJerarquiaCatalogoCheckListOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdActividadCheckListOrdenTrabajo,
                                                          OtrosDatosOrdenTrabajo.cIdEquipoCheckListOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdCaracteristica, OtrosDatosOrdenTrabajo.vValorReferencialOtrosDatosOrdenTrabajo,
                                                          OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    'Public Function OtrosDatosOrdenTrabajoElimina(ByVal OtrosDatosOrdenTrabajo As LOGI_OTROSDATOSORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_NONE", "UPDATE LOGI_OTROSDATOSORDENTRABAJO SET bEstadoRegistroOtrosDatosOrdenTrabajoOrdenTrabajo = 0 " &
    '                                                      "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OtrosDatosOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OtrosDatosOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND nIdNumeroItemOtrosDatosOrdenTrabajoOrdenTrabajo = " & OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajoOrdenTrabajo &
    '                                                      "      AND cIdEmpresa = '" & OtrosDatosOrdenTrabajo.cIdEmpresa & "'",
    '                                                      "", "", "", "", 0, "", "", "", "", " ", "", "", "", "").ReturnValue.ToString
    '    Return x
    'End Function

    Public Function OtrosDatosOrdenTrabajoExiste(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdNroItem As String) As Boolean
        If Data.PA_LOGI_MNT_OTROSDATOSORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_OTROSDATOSORDENTRABAJO " &
                                                         "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND nIdNumeroItemOtrosDatosOrdenTrabajoOrdenTrabajo = " & IdNroItem &
                                                         "      AND cIdEmpresa = '" & IdEmpresa & "'",
                                                         "", "", "", "", 0, "", "", "", "", " ", "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
