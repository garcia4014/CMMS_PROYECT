Imports System.Data
Imports System.Data.SqlClient
Public Class clsTipoEquipoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    '    Public Function TipoEquipoListarCombo(ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of LOGI_TipoEquipo)
    Public Function TipoEquipoGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        'Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
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

    Public Function TipoEquipoListarCombo(Optional ByVal Estado As String = "*") As List(Of LOGI_TipoEquipo)
        Dim Consulta = Data.PA_LOGI_MNT_TIPOEQUIPO("SQL_NONE", "SELECT * FROM LOGI_TIPOEQUIPO WHERE (bEstadoRegistroTipoEquipo = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", "1", "")
        Dim Coleccion As New List(Of LOGI_TipoEquipo)
        For Each TipoEquipo In Consulta
            Dim TipAct As New LOGI_TipoEquipo
            TipAct.cIdTipoEquipo = TipoEquipo.cIdTipoEquipo
            TipAct.vDescripcionTipoEquipo = TipoEquipo.vDescripcionTipoEquipo
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function

    'Public Function TipoEquipoListarPorId(ByVal IdTipoEquipo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_TipoEquipo
    Public Function TipoEquipoListarPorId(ByVal IdTipoEquipo As String) As LOGI_TIPOEQUIPO
        Dim Consulta = Data.PA_LOGI_MNT_TIPOEQUIPO("SQL_NONE", "SELECT * FROM LOGI_TIPOEQUIPO WHERE cIdTipoEquipo = '" & IdTipoEquipo &
                                                   "' AND bEstadoRegistroTipoEquipo = 1 ", "", "", "", "1", "")
        Dim Coleccion As New LOGI_TIPOEQUIPO
        For Each LOGI_TipoEquipo In Consulta
            Coleccion.cIdTipoEquipo = LOGI_TipoEquipo.cIdTipoEquipo
            Coleccion.vDescripcionTipoEquipo = LOGI_TipoEquipo.vDescripcionTipoEquipo
            Coleccion.vDescripcionAbreviadaTipoEquipo = LOGI_TipoEquipo.vDescripcionAbreviadaTipoEquipo
            Coleccion.bEstadoRegistroTipoEquipo = LOGI_TipoEquipo.bEstadoRegistroTipoEquipo
        Next
        Return Coleccion
    End Function

    'Public Function TipoEquipoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_TipoEquipo)
    Public Function TipoEquipoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_TIPOEQUIPO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_TIPOEQUIPO("SQL_NONE", "SELECT * FROM LOGI_TIPOEQUIPO " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroTipoEquipo = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_LOGI_TIPOEQUIPO)
        For Each Busqueda In Consulta
            Dim BuscarFam As New VI_LOGI_TIPOEQUIPO
            BuscarFam.Codigo = Busqueda.cIdTipoEquipo
            BuscarFam.Descripcion = Busqueda.vDescripcionTipoEquipo
            BuscarFam.Estado = Busqueda.bEstadoRegistroTipoEquipo
            Coleccion.Add(BuscarFam)
        Next
        Return Coleccion
    End Function

    Public Function TipoEquipoInserta(ByVal TipoEquipo As LOGI_TIPOEQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOEQUIPO("SQL_INSERT", "", TipoEquipo.cIdTipoEquipo, TipoEquipo.vDescripcionTipoEquipo,
                                        TipoEquipo.vDescripcionAbreviadaTipoEquipo, TipoEquipo.bEstadoRegistroTipoEquipo, TipoEquipo.cIdTipoEquipo).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoEquipoEdita(ByVal TipoEquipo As LOGI_TIPOEQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOEQUIPO("SQL_UPDATE", "", TipoEquipo.cIdTipoEquipo, TipoEquipo.vDescripcionTipoEquipo,
                                     TipoEquipo.vDescripcionAbreviadaTipoEquipo, TipoEquipo.bEstadoRegistroTipoEquipo, TipoEquipo.cIdTipoEquipo).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoEquipoElimina(ByVal TipoEquipo As LOGI_TIPOEQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOEQUIPO("SQL_NONE", "UPDATE LOGI_TIPOEQUIPO SET bEstadoRegistroTipoEquipo = 0 WHERE cIdTipoEquipo = '" & TipoEquipo.cIdTipoEquipo & "'",
                                        "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoEquipoExiste(ByVal IdTipoEquipo As String) As Boolean
        'If Data.PA_LOGI_MNT_TipoEquipo("SQL_NONE", "SELECT * FROM LOGI_TipoEquipo WHERE cIdTipoEquipo = '" & IdTipoEquipo & "' " &
        '                             " AND bEstadoRegistroTipoEquipo = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_TIPOEQUIPO("SQL_NONE", "SELECT * FROM LOGI_TIPOEQUIPO WHERE cIdTipoEquipo = '" & IdTipoEquipo & "' ",
                                       "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
