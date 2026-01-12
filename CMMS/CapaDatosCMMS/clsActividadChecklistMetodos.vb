Imports System.Data
Imports System.Data.SqlClient
Public Class clsActividadChecklistMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function ActividadChecklistGetData(strQuery As String) As DataTable
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

    Public Function ActividadChecklistListarCombo() As List(Of LOGI_ACTIVIDADCHECKLIST)
        Dim Consulta = Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_NONE", "SELECT * FROM LOGI_ACTIVIDADCHECKLIST ORDER BY vDescripcionActividadCheckList ASC",
                                                   "", "", "", "1", "")
        Dim Coleccion As New List(Of LOGI_ACTIVIDADCHECKLIST)
        For Each ActividadChecklist In Consulta
            Dim TipAct As New LOGI_ACTIVIDADCHECKLIST
            TipAct.cIdActividadCheckList = ActividadChecklist.cIdActividadCheckList
            TipAct.vDescripcionActividadCheckList = ActividadChecklist.vDescripcionActividadCheckList
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function

    'Public Function ActividadChecklistListarPorId(ByVal IdActividadChecklist As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_ACTIVIDADCHECKLIST
    Public Function ActividadChecklistListarPorId(ByVal IdActividadChecklist As String) As LOGI_ACTIVIDADCHECKLIST
        Dim Consulta = Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_NONE", "SELECT * FROM LOGI_ACTIVIDADCHECKLIST WHERE cIdActividadCheckList = '" & IdActividadChecklist &
                                                   "' AND bEstadoRegistroActividadCheckList = 1 ", "", "", "", "1", "")
        Dim Coleccion As New LOGI_ACTIVIDADCHECKLIST
        For Each LOGI_ACTIVIDADCHECKLIST In Consulta
            Coleccion.cIdActividadCheckList = LOGI_ACTIVIDADCHECKLIST.cIdActividadCheckList
            Coleccion.vDescripcionActividadCheckList = LOGI_ACTIVIDADCHECKLIST.vDescripcionActividadCheckList
            Coleccion.vDescripcionAbreviadaActividadCheckList = LOGI_ACTIVIDADCHECKLIST.vDescripcionAbreviadaActividadCheckList
            Coleccion.bEstadoRegistroActividadChecklist = LOGI_ACTIVIDADCHECKLIST.bEstadoRegistroActividadCheckList
        Next
        Return Coleccion
    End Function

    'Public Function ActividadChecklistListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_ACTIVIDADCHECKLIST)
    Public Function ActividadChecklistListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_ACTIVIDADCHECKLIST)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_NONE", "SELECT * FROM LOGI_ACTIVIDADCHECKLIST " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroActividadCheckList = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_LOGI_ACTIVIDADCHECKLIST)
        For Each Busqueda In Consulta
            Dim BuscarFam As New VI_LOGI_ACTIVIDADCHECKLIST
            BuscarFam.Codigo = Busqueda.cIdActividadCheckList
            BuscarFam.Descripcion = Busqueda.vDescripcionActividadCheckList
            BuscarFam.Estado = Busqueda.bEstadoRegistroActividadCheckList
            Coleccion.Add(BuscarFam)
        Next
        Return Coleccion
    End Function

    Public Function ActividadChecklistInserta(ByVal ActividadChecklist As LOGI_ACTIVIDADCHECKLIST) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_INSERT", "", ActividadChecklist.cIdActividadCheckList, ActividadChecklist.vDescripcionActividadCheckList,
                                        ActividadChecklist.vDescripcionAbreviadaActividadCheckList, ActividadChecklist.bEstadoRegistroActividadCheckList, ActividadChecklist.cIdActividadCheckList).ReturnValue.ToString
        Return x
    End Function

    Public Function ActividadChecklistEdita(ByVal ActividadChecklist As LOGI_ACTIVIDADCHECKLIST) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_UPDATE", "", ActividadChecklist.cIdActividadCheckList, ActividadChecklist.vDescripcionActividadCheckList,
                                     ActividadChecklist.vDescripcionAbreviadaActividadCheckList, ActividadChecklist.bEstadoRegistroActividadCheckList, ActividadChecklist.cIdActividadCheckList).ReturnValue.ToString
        Return x
    End Function

    Public Function ActividadChecklistElimina(ByVal ActividadChecklist As LOGI_ACTIVIDADCHECKLIST) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_NONE", "UPDATE LOGI_ACTIVIDADCHECKLIST SET bEstadoRegistroActividadCheckList = 0 WHERE cIdActividadCheckList = '" & ActividadChecklist.cIdActividadCheckList & "'",
                                        "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function ActividadChecklistExiste(ByVal IdActividadChecklist As String) As Boolean
        'If Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_NONE", "SELECT * FROM LOGI_ACTIVIDADCHECKLIST WHERE cIdActividadChecklist = '" & IdActividadChecklist & "' " &
        '                             " AND bEstadoRegistroActividadChecklist = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_ACTIVIDADCHECKLIST("SQL_NONE", "SELECT * FROM LOGI_ACTIVIDADCHECKLIST WHERE cIdActividadCheckList = '" & IdActividadChecklist & "' ",
                                       "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
