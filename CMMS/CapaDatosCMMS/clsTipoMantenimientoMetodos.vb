Imports System.Data
Imports System.Data.SqlClient
Public Class clsTipoMantenimientoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function TipoMantenimientoGetData(strQuery As String) As DataTable
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

    Public Function TipoMantenimientoListarCombo(ByVal Estado As String) As List(Of LOGI_TIPOMANTENIMIENTO)
        Dim Consulta = Data.PA_LOGI_MNT_TIPOMANTENIMIENTO("SQL_NONE", "SELECT * FROM LOGI_TIPOMANTENIMIENTO WHERE (bEstadoRegistroTipoMantenimiento = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", 0, "1", "")
        Dim Coleccion As New List(Of LOGI_TIPOMANTENIMIENTO)
        For Each TipoMantenimiento In Consulta
            Dim TipAct As New LOGI_TIPOMANTENIMIENTO
            TipAct.cIdTipoMantenimiento = TipoMantenimiento.cIdTipoMantenimiento
            TipAct.vDescripcionTipoMantenimiento = TipoMantenimiento.vDescripcionTipoMantenimiento
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function

    'Public Function TipoMantenimientoListarPorId(ByVal IdTipoMantenimiento As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_TipoMantenimiento
    Public Function TipoMantenimientoListarPorId(ByVal IdTipoMantenimiento As String) As LOGI_TIPOMANTENIMIENTO
        Dim Consulta = Data.PA_LOGI_MNT_TIPOMANTENIMIENTO("SQL_NONE", "SELECT * FROM LOGI_TIPOMANTENIMIENTO WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento &
                                                   "' AND bEstadoRegistroTipoMantenimiento = 1 ", "", "", "", 0, "1", "")
        Dim Coleccion As New LOGI_TIPOMANTENIMIENTO
        For Each LOGI_TIPOMANTENIMIENTO In Consulta
            Coleccion.cIdTipoMantenimiento = LOGI_TIPOMANTENIMIENTO.cIdTipoMantenimiento
            Coleccion.vDescripcionTipoMantenimiento = LOGI_TIPOMANTENIMIENTO.vDescripcionTipoMantenimiento
            Coleccion.vDescripcionAbreviadaTipoMantenimiento = LOGI_TIPOMANTENIMIENTO.vDescripcionAbreviadaTipoMantenimiento
            Coleccion.bEstadoRegistroTipoMantenimiento = LOGI_TIPOMANTENIMIENTO.bEstadoRegistroTipoMantenimiento
            Coleccion.nMesesDesdeContratoTipoMantenimiento = LOGI_TIPOMANTENIMIENTO.nMesesDesdeContratoTipoMantenimiento
        Next
        Return Coleccion
    End Function

    'Public Function TipoMantenimientoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_TipoMantenimiento)
    Public Function TipoMantenimientoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_TIPOMANTENIMIENTO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_TIPOMANTENIMIENTO("SQL_NONE", "SELECT * FROM LOGI_TIPOMANTENIMIENTO " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroTipoMantenimiento = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", 0, "1", "")
        Dim Coleccion As New List(Of VI_LOGI_TIPOMANTENIMIENTO)
        For Each Busqueda In Consulta
            Dim BuscarFam As New VI_LOGI_TIPOMANTENIMIENTO
            BuscarFam.Codigo = Busqueda.cIdTipoMantenimiento
            BuscarFam.Descripcion = Busqueda.vDescripcionTipoMantenimiento
            BuscarFam.Estado = Busqueda.bEstadoRegistroTipoMantenimiento
            Coleccion.Add(BuscarFam)
        Next
        Return Coleccion
    End Function

    Public Function TipoMantenimientoInserta(ByVal TipoMantenimiento As LOGI_TIPOMANTENIMIENTO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOMANTENIMIENTO("SQL_INSERT", "", TipoMantenimiento.cIdTipoMantenimiento, TipoMantenimiento.vDescripcionTipoMantenimiento,
                                        TipoMantenimiento.vDescripcionAbreviadaTipoMantenimiento, TipoMantenimiento.nMesesDesdeContratoTipoMantenimiento, TipoMantenimiento.bEstadoRegistroTipoMantenimiento, TipoMantenimiento.cIdTipoMantenimiento).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoMantenimientoEdita(ByVal TipoMantenimiento As LOGI_TIPOMANTENIMIENTO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOMANTENIMIENTO("SQL_UPDATE", "", TipoMantenimiento.cIdTipoMantenimiento, TipoMantenimiento.vDescripcionTipoMantenimiento,
                                     TipoMantenimiento.vDescripcionAbreviadaTipoMantenimiento, TipoMantenimiento.nMesesDesdeContratoTipoMantenimiento, TipoMantenimiento.bEstadoRegistroTipoMantenimiento, TipoMantenimiento.cIdTipoMantenimiento).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoMantenimientoElimina(ByVal TipoMantenimiento As LOGI_TIPOMANTENIMIENTO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOMANTENIMIENTO("SQL_NONE", "UPDATE LOGI_TipoMantenimiento SET bEstadoRegistroTipoMantenimiento = 0 WHERE cIdTipoMantenimiento = '" & TipoMantenimiento.cIdTipoMantenimiento & "'",
                                        "", "", "", 0, "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoMantenimientoExiste(ByVal IdTipoMantenimiento As String) As Boolean
        'If Data.PA_LOGI_MNT_TipoMantenimiento("SQL_NONE", "SELECT * FROM LOGI_TipoMantenimiento WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "' " &
        '                             " AND bEstadoRegistroTipoMantenimiento = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_TIPOMANTENIMIENTO("SQL_NONE", "SELECT * FROM LOGI_TIPOMANTENIMIENTO WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "' ",
                                       "", "", "", 0, "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
