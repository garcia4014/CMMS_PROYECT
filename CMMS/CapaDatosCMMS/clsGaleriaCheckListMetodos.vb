Imports System.Data
Imports System.Data.SqlClient
Public Class clsGaleriaCheckListMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function GaleriaCheckListGetData(strQuery As String) As DataTable
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

    'Public Function GaleriaCheckListListarCombo() As List(Of LOGI_GALERIACHECKLISTORDENTRABAJO)
    '    Dim Consulta = Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_GALERIACHECKLISTORDENTRABAJO",
    '                                                  0, "", "", "", "", "", "", "", " ", "", "", "", "", "", "1", Now, "")
    '    Dim Coleccion As New List(Of LOGI_GALERIACHECKLISTORDENTRABAJO)
    '    For Each GaleriaCheckList In Consulta
    '        Dim TipAct As New LOGI_GALERIACHECKLISTORDENTRABAJO
    '        TipAct.cIdCheckList = GaleriaCheckList.cIdCheckList
    '        TipAct.vDescripcionGaleriaCheckList = GaleriaCheckList.vDescripcionGaleriaCheckList
    '        Coleccion.Add(TipAct)
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function GaleriaCheckListListarPorId(ByVal IdGaleriaCheckList As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_GALERIACHECKLISTORDENTRABAJO
    Public Function GaleriaCheckListListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdNroItem As String) As LOGI_GALERIACHECKLISTORDENTRABAJO
        Dim Consulta = Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_GALERIACHECKLISTORDENTRABAJO " &
                                                                     "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND nIdNumeroItemGaleriaCheckListOrdenTrabajo = " & IdNroItem &
                                                                     " AND cIdEmpresa = '" & IdEmpresa & "' AND bEstadoRegistroGaleriaCheckList = 1 ",
                                                                     0, "", "", "", "", "", "", "", " ", "", "", "", "", "", "1", Now, "", "")
        Dim Coleccion As New LOGI_GALERIACHECKLISTORDENTRABAJO
        For Each LOGI_GALERIACHECKLISTORDENTRABAJO In Consulta
            Coleccion.nIdNumeroItemGaleriaCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.nIdNumeroItemGaleriaCheckListOrdenTrabajo
            Coleccion.cIdTipoDocumentoCabeceraOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.cIdTipoDocumentoCabeceraOrdenTrabajo
            Coleccion.vIdNumeroSerieCabeceraOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.vIdNumeroSerieCabeceraOrdenTrabajo
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.vIdNumeroCorrelativoCabeceraOrdenTrabajo
            Coleccion.cIdEmpresa = LOGI_GALERIACHECKLISTORDENTRABAJO.cIdEmpresa
            Coleccion.cIdEquipoCabeceraOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.cIdEquipoCabeceraOrdenTrabajo
            Coleccion.cIdActividadCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.cIdActividadCheckListOrdenTrabajo
            Coleccion.cIdCatalogoCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.cIdCatalogoCheckListOrdenTrabajo
            Coleccion.cIdJerarquiaCatalogoCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.cIdJerarquiaCatalogoCheckListOrdenTrabajo
            Coleccion.vIdArticuloSAPCabeceraOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.vIdArticuloSAPCabeceraOrdenTrabajo
            Coleccion.cIdEquipoCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.cIdEquipoCheckListOrdenTrabajo
            Coleccion.vDescripcionGaleriaCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.vDescripcionGaleriaCheckListOrdenTrabajo
            Coleccion.vObservacionGaleriaCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.vObservacionGaleriaCheckListOrdenTrabajo
            Coleccion.vTituloGaleriaCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.vTituloGaleriaCheckListOrdenTrabajo
            Coleccion.bEstadoRegistroGaleriaCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.bEstadoRegistroGaleriaCheckListOrdenTrabajo
            Coleccion.dFechaTransaccionGaleriaCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.dFechaTransaccionGaleriaCheckListOrdenTrabajo
            Coleccion.vNombreArchivoGaleriaCheckListOrdenTrabajo = LOGI_GALERIACHECKLISTORDENTRABAJO.vNombreArchivoGaleriaCheckListOrdenTrabajo
        Next
        Return Coleccion
    End Function

    'Public Function GaleriaCheckListListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_GALERIACHECKLISTORDENTRABAJO)
    Public Function GaleriaCheckListListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_GALERIACHECKLISTORDENTRABAJO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_GALERIACHECKLISTORDENTRABAJO " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroGaleriaCheckListOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   0, "", "", "", "", "", "", "", " ", "", "", "", "", "", "1", Now, "", "")
        Dim Coleccion As New List(Of VI_LOGI_GALERIACHECKLISTORDENTRABAJO)
        For Each Busqueda In Consulta
            Dim BuscarChkLst As New VI_LOGI_GALERIACHECKLISTORDENTRABAJO
            BuscarChkLst.NumeroItem = Busqueda.nIdNumeroItemGaleriaCheckListOrdenTrabajo
            BuscarChkLst.Titulo = Busqueda.vTituloGaleriaCheckListOrdenTrabajo
            BuscarChkLst.Descripcion = Busqueda.vDescripcionGaleriaCheckListOrdenTrabajo
            BuscarChkLst.Observacion = Busqueda.vObservacionGaleriaCheckListOrdenTrabajo
            BuscarChkLst.Estado = Busqueda.bEstadoRegistroGaleriaCheckListOrdenTrabajo
            BuscarChkLst.IdEquipoCheckList = Busqueda.cIdEquipoCheckListOrdenTrabajo
            BuscarChkLst.IdEquipo = Busqueda.cIdEquipoCabeceraOrdenTrabajo
            BuscarChkLst.IdActividad = Busqueda.cIdActividadCheckListOrdenTrabajo
            BuscarChkLst.DocumentoRef = Busqueda.cIdTipoDocumentoCabeceraOrdenTrabajo + "-" + Busqueda.vIdNumeroSerieCabeceraOrdenTrabajo + "-" + Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo
            BuscarChkLst.NombreArchivo = Busqueda.vNombreArchivoGaleriaCheckListOrdenTrabajo
            Coleccion.Add(BuscarChkLst)
        Next
        Return Coleccion
    End Function

    Public Function GaleriaCheckListInserta(ByVal GaleriaCheckList As LOGI_GALERIACHECKLISTORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_INSERT", "", GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo, GaleriaCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo,
                                                          GaleriaCheckList.vIdNumeroSerieCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo, GaleriaCheckList.cIdEmpresa,
                                                          GaleriaCheckList.cIdEquipoCabeceraOrdenTrabajo, GaleriaCheckList.cIdActividadCheckListOrdenTrabajo, GaleriaCheckList.cIdCatalogoCheckListOrdenTrabajo,
                                                          GaleriaCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo, GaleriaCheckList.vIdArticuloSAPCabeceraOrdenTrabajo, GaleriaCheckList.cIdEquipoCheckListOrdenTrabajo,
                                                          GaleriaCheckList.vDescripcionGaleriaCheckListOrdenTrabajo, GaleriaCheckList.vObservacionGaleriaCheckListOrdenTrabajo, GaleriaCheckList.vTituloGaleriaCheckListOrdenTrabajo,
                                                          GaleriaCheckList.bEstadoRegistroGaleriaCheckListOrdenTrabajo, GaleriaCheckList.dFechaTransaccionGaleriaCheckListOrdenTrabajo,
                                                          GaleriaCheckList.vNombreArchivoGaleriaCheckListOrdenTrabajo, GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function GaleriaCheckListEdita(ByVal GaleriaCheckList As LOGI_GALERIACHECKLISTORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_UPDATE", "", GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo, GaleriaCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo,
                                                          GaleriaCheckList.vIdNumeroSerieCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo, GaleriaCheckList.cIdEmpresa,
                                                          GaleriaCheckList.cIdEquipoCabeceraOrdenTrabajo, GaleriaCheckList.cIdActividadCheckListOrdenTrabajo, GaleriaCheckList.cIdCatalogoCheckListOrdenTrabajo,
                                                          GaleriaCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo, GaleriaCheckList.vIdArticuloSAPCabeceraOrdenTrabajo, GaleriaCheckList.cIdEquipoCheckListOrdenTrabajo,
                                                          GaleriaCheckList.vDescripcionGaleriaCheckListOrdenTrabajo, GaleriaCheckList.vObservacionGaleriaCheckListOrdenTrabajo, GaleriaCheckList.vTituloGaleriaCheckListOrdenTrabajo,
                                                          GaleriaCheckList.bEstadoRegistroGaleriaCheckListOrdenTrabajo, GaleriaCheckList.dFechaTransaccionGaleriaCheckListOrdenTrabajo,
                                                          GaleriaCheckList.vNombreArchivoGaleriaCheckListOrdenTrabajo, GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function GaleriaCheckListElimina(ByVal GaleriaCheckList As LOGI_GALERIACHECKLISTORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_NONE", "UPDATE LOGI_GALERIACHECKLISTORDENTRABAJO SET bEstadoRegistroGaleriaCheckListOrdenTrabajo = 0 " &
                                                          "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & GaleriaCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & GaleriaCheckList.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND nIdNumeroItemGaleriaCheckListOrdenTrabajo = " & GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo &
                                                          "      AND cIdEmpresa = '" & GaleriaCheckList.cIdEmpresa & "'",
                                                          0, "", "", "", "", "", "", "", " ", "", "", "", "", "", "1", Now, "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function GaleriaCheckListExiste(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdNroItem As String) As Boolean
        'If Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_GALERIACHECKLISTORDENTRABAJO WHERE cIdGaleriaCheckList = '" & IdGaleriaCheckList & "' " &
        '                             " AND bEstadoRegistroGaleriaCheckList = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_GALERIACHECKLISTORDENTRABAJO " &
                                                         "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND nIdNumeroItemGaleriaCheckListOrdenTrabajo = " & IdNroItem &
                                                         "      AND cIdEmpresa = '" & IdEmpresa & "'",
                                                         0, "", "", "", "", "", "", "", " ", "", "", "", "", "", "1", Now, "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
