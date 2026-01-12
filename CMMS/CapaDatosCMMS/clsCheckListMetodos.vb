Imports System.Data
Imports System.Data.SqlClient
Public Class clsCheckListMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function CheckListGetData(strQuery As String) As DataTable
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

    Public Function CheckListListarCombo() As List(Of LOGI_CHECKLISTORDENTRABAJO)
        Dim Consulta = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO",
                                                      "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "", "")
        Dim Coleccion As New List(Of LOGI_CHECKLISTORDENTRABAJO)
        For Each CheckList In Consulta
            Dim TipAct As New LOGI_CHECKLISTORDENTRABAJO
            TipAct.cIdEquipoCabeceraOrdenTrabajo = CheckList.cIdEquipoCabeceraOrdenTrabajo
            'TipAct.cIdActividadCheckListOrdenTrabajo  = CheckList.vDescripcionCheckList
            TipAct.cIdActividadCheckListOrdenTrabajo = CheckList.cIdActividadCheckListOrdenTrabajo
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function

    'Public Function CheckListListarPorId(ByVal IdEquipo As String, ByVal IdNroItem As String) As LOGI_CHECKLISTORDENTRABAJO
    '    Dim Consulta = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WHERE " &
    '                                                          "cIdEquipo = '" & IdEquipo & "' AND nIdNumeroItemCheckList = " & IdNroItem &
    '                                               " AND bEstadoRegistroCheckList = 1 ",
    '                                                          "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "")
    '    Dim Coleccion As New LOGI_CHECKLISTORDENTRABAJO
    '    For Each LOGI_CHECKLISTORDENTRABAJO In Consulta
    '        Coleccion.cIdEquipo = LOGI_CHECKLISTORDENTRABAJO.cIdEquipo
    '        Coleccion.nIdNumeroItemCheckList = LOGI_CHECKLISTORDENTRABAJO.nIdNumeroItemCheckList
    '        Coleccion.vDescripcionCheckList = LOGI_CHECKLISTORDENTRABAJO.vDescripcionCheckList
    '        Coleccion.vObservacionCheckList = LOGI_CHECKLISTORDENTRABAJO.vObservacionCheckList
    '        Coleccion.vTituloCheckList = LOGI_CHECKLISTORDENTRABAJO.vTituloCheckList
    '        Coleccion.bEstadoRegistroCheckList = LOGI_CHECKLISTORDENTRABAJO.bEstadoRegistroCheckList
    '        Coleccion.dFechaTransaccionCheckList = LOGI_CHECKLISTORDENTRABAJO.dFechaTransaccionCheckList
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function CheckListListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_CHECKLISTORDENTRABAJO)
    '    'Este si puede devolver una colección de datos es decir varios registros
    '    Dim Consulta = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO " &
    '                                               "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroCheckList = '" & Estado & "' OR '*' = '" & Estado & "')",
    '                                               "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "")
    '    Dim Coleccion As New List(Of VI_LOGI_CHECKLISTORDENTRABAJO)
    '    For Each Busqueda In Consulta
    '        Dim BuscarFam As New VI_LOGI_CHECKLISTORDENTRABAJO
    '        BuscarFam.Codigo = Busqueda.cIdEquipo
    '        BuscarFam.NumeroItem = Busqueda.nIdNumeroItemCheckList
    '        BuscarFam.Titulo = Busqueda.vTituloCheckList
    '        BuscarFam.Descripcion = Busqueda.vDescripcionCheckList
    '        BuscarFam.Observacion = Busqueda.vObservacionCheckList
    '        BuscarFam.Estado = Busqueda.bEstadoRegistroCheckList
    '        Coleccion.Add(BuscarFam)
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function CheckListListaGridPorId(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_LOGI_CHECKLISTORDENTRABAJO)
    Public Function CheckListListaGridPorOrdEquipo(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroCor As String,
                                            ByVal IdEmp As String, ByVal IdEquipo As String, ByVal IdArticuloSAP As String,
                                            ByVal IdCatalogoComponente As String) As List(Of VI_LOGI_CHECKLISTORDENTRABAJO)
        '                                        ByVal IdNroChkLis As String) As List(Of VI_LOGI_CHECKLISTORDENTRABAJO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "SELECT CHKORDTRA.*, ACTCHECKLIST.vDescripcionActividadCheckList FROM LOGI_CHECKLISTORDENTRABAJO AS CHKORDTRA INNER JOIN LOGI_ACTIVIDADCHECKLIST AS ACTCHECKLIST ON " &
        '                                           "CHKORDTRA.cIdActividadCheckListOrdenTrabajo = ACTCHECKLIST.cIdActividadCheckList " &
        '                                           "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroCheckList = '" & Estado & "' OR '*' = '" & Estado & "')",
        '                                           "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "")

        Dim Consulta = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "SELECT CHKORDTRA.*, ACTCHECKLIST.vDescripcionActividadCheckList, " &
                                                   "EQU.vDescripcionEquipo, CAT.cIdSistemaFuncional, SISFUN.vDescripcionSistemaFuncional, EQU.cIdTipoActivo, TIPACT.vDescripcionTipoActivo, CAT.vDescripcionCatalogo, ISNULL(CHKORDTRA.cEstadoActividadCheckListOrdenTrabajo, '') AS cEstadoActividadCheckListOrdenTrabajo " &
                                                   "FROM LOGI_CHECKLISTORDENTRABAJO AS CHKORDTRA INNER JOIN LOGI_ACTIVIDADCHECKLIST AS ACTCHECKLIST ON " &
                                                   "CHKORDTRA.cIdActividadCheckListOrdenTrabajo = ACTCHECKLIST.cIdActividadCheckList " &
                                                   "INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                   "CHKORDTRA.cIdEquipoCabeceraOrdenTrabajo = EQU.cIdEquipo " &
                                                   "LEFT JOIN LOGI_CATALOGO AS CAT ON " &
                                                   "CHKORDTRA.cIdCatalogoCheckListOrdenTrabajo = CAT.cIdCatalogo AND " &
                                                   "CHKORDTRA.cIdJerarquiaCatalogoCheckListOrdenTrabajo = CAT.cIdJerarquiaCatalogo " &
                                                   "LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
                                                   "EQU.cIdSistemaFuncionalEquipo = SISFUN.cIdSistemaFuncional " &
                                                   "LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
                                                   "EQU.cIdTipoActivo = TIPACT.cIdTipoActivo " &
                                                   "WHERE " &
                                                   "CHKORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND " &
                                                   "CHKORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' AND " &
                                                   "CHKORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroCor & "' AND " &
                                                   "CHKORDTRA.cIdEmpresa = '" & IdEmp & "' AND " &
                                                   "CHKORDTRA.cIdEquipoCabeceraOrdenTrabajo = '" & IdEquipo & "' AND " &
                                                   "CHKORDTRA.vIdArticuloSAPCabeceraOrdenTrabajo = '" & IdArticuloSAP & "' AND " &
                                                   "(CHKORDTRA.cIdEquipoCheckListOrdenTrabajo = '" & IdCatalogoComponente & "' OR '*' = '" & IdCatalogoComponente & "' ) ORDER BY CASE WHEN nIdNumeroItemCheckListOrdenTrabajo IS NULL THEN 1 ELSE 0 END, dFechaInicioCheckListOrdenTrabajo",
                                                   "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "", "")
        '                                                   "cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = '" & IdNroChkLis & "' ",


        'cIdCatalogoCheckListOrdenTrabajo = ''
        'cIdJerarquiaCatalogoCheckListOrdenTrabajo = ''


        Dim Coleccion As New List(Of VI_LOGI_CHECKLISTORDENTRABAJO)
        For Each Busqueda In Consulta
            Dim BuscarChklist As New VI_LOGI_CHECKLISTORDENTRABAJO
            BuscarChklist.IdCatalogo = Busqueda.cIdCatalogoCheckListOrdenTrabajo
            BuscarChklist.IdJerarquiaCatalogo = Busqueda.cIdJerarquiaCatalogoCheckListOrdenTrabajo
            BuscarChklist.IdArticuloSAPCabecera = Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo
            BuscarChklist.IdActividad = Busqueda.cIdActividadCheckListOrdenTrabajo
            BuscarChklist.IdEquipo = Busqueda.cIdEquipoCabeceraOrdenTrabajo
            BuscarChklist.Observacion = Busqueda.vObservacionCheckListOrdenTrabajo
            BuscarChklist.DescripcionActividad = Busqueda.vDescripcionActividadCheckList
            BuscarChklist.StatusCheckList = Busqueda.cEstadoCheckListOrdenTrabajo
            BuscarChklist.DescripcionEquipo = Busqueda.vDescripcionEquipo
            BuscarChklist.DescripcionTipoActivo = Busqueda.vDescripcionTipoActivo
            BuscarChklist.DescripcionSistemaFuncional = Busqueda.vDescripcionSistemaFuncional
            BuscarChklist.DescripcionCatalogo = Busqueda.vDescripcionCatalogo
            BuscarChklist.StatusActividad = Busqueda.cEstadoActividadCheckListOrdenTrabajo
            BuscarChklist.IdEquipoCheckList = Busqueda.cIdEquipoCheckListOrdenTrabajo
            Coleccion.Add(BuscarChklist)
        Next
        Return Coleccion
    End Function

    'Public Function CheckListInserta(ByVal CheckList As LOGI_CHECKLISTORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_INSERT", "", CheckList.cIdEquipo, CheckList.nIdNumeroItemCheckList, CheckList.vDescripcionCheckList,
    '                                    CheckList.vObservacionCheckList, CheckList.vTituloCheckList, CheckList.bEstadoRegistroCheckList,
    '                                    CheckList.dFechaTransaccionCheckList, CheckList.cIdEquipo).ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function CheckListEdita(ByVal CheckList As LOGI_CHECKLISTORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_UPDATE", "", CheckList.cIdEquipo, CheckList.nIdNumeroItemCheckList, CheckList.vDescripcionCheckList,
    '                                    CheckList.vObservacionCheckList, CheckList.vTituloCheckList, CheckList.bEstadoRegistroCheckList,
    '                                    CheckList.dFechaTransaccionCheckList, CheckList.cIdEquipo).ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function CheckListElimina(ByVal CheckList As LOGI_CHECKLISTORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "UPDATE LOGI_CHECKLISTORDENTRABAJO SET bEstadoRegistroCheckList = 0 WHERE cIdEquipo = '" & CheckList.cIdEquipo & "'",
    '                                       "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "").ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function CheckListExiste(ByVal IdEquipo As String, ByVal IdNroItem As String) As Boolean
    '    'If Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdCheckList = '" & IdCheckList & "' " &
    '    '                             " AND bEstadoRegistroCheckList = 1", "", "", "", "1", "").Count > 0 Then
    '    If Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdEquipo = '" & IdEquipo & "' AND nIdNumeroItemCheckList = " & IdNroItem,
    '                                      "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "").Count > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
End Class
