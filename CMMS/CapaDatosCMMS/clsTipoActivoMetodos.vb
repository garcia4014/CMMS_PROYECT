Imports System.Data
Imports System.Data.SqlClient
Public Class clsTipoActivoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    '    Public Function TipoActivoListarCombo(ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of LOGI_TIPOACTIVO)
    Public Function TipoActivoGetData(strQuery As String) As DataTable
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

    Public Function TipoActivoListarCombo(Optional ByVal Estado As String = "*") As List(Of LOGI_TIPOACTIVO)
        Dim Consulta = Data.PA_LOGI_MNT_TIPOACTIVO("SQL_NONE", "SELECT * FROM LOGI_TIPOACTIVO WHERE (bEstadoRegistroTipoActivo = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", "1", "")
        Dim Coleccion As New List(Of LOGI_TIPOACTIVO)
        For Each TipoActivo In Consulta
            Dim TipAct As New LOGI_TIPOACTIVO
            TipAct.cIdTipoActivo = TipoActivo.cIdTipoActivo
            TipAct.vDescripcionTipoActivo = TipoActivo.vDescripcionTipoActivo
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function

    'Public Function TipoActivoListarPorId(ByVal IdTipoActivo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_TIPOACTIVO
    Public Function TipoActivoListarPorId(ByVal IdTipoActivo As String) As LOGI_TIPOACTIVO
        Dim Consulta = Data.PA_LOGI_MNT_TIPOACTIVO("SQL_NONE", "SELECT * FROM LOGI_TIPOACTIVO WHERE cIdTipoActivo = '" & IdTipoActivo &
                                                   "' AND bEstadoRegistroTipoActivo = 1 ", "", "", "", "1", "")
        Dim Coleccion As New LOGI_TIPOACTIVO
        For Each LOGI_TIPOACTIVO In Consulta
            Coleccion.cIdTipoActivo = LOGI_TIPOACTIVO.cIdTipoActivo
            Coleccion.vDescripcionTipoActivo = LOGI_TIPOACTIVO.vDescripcionTipoActivo
            Coleccion.vDescripcionAbreviadaTipoActivo = LOGI_TIPOACTIVO.vDescripcionAbreviadaTipoActivo
            Coleccion.bEstadoRegistroTipoActivo = LOGI_TIPOACTIVO.bEstadoRegistroTipoActivo
        Next
        Return Coleccion
    End Function

    'Public Function TipoActivoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_TIPOACTIVO)
    Public Function TipoActivoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_TIPOACTIVO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_TIPOACTIVO("SQL_NONE", "SELECT * FROM LOGI_TIPOACTIVO " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroTipoActivo = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_LOGI_TIPOACTIVO)
        For Each Busqueda In Consulta
            Dim BuscarFam As New VI_LOGI_TIPOACTIVO
            BuscarFam.Codigo = Busqueda.cIdTipoActivo
            BuscarFam.Descripcion = Busqueda.vDescripcionTipoActivo
            BuscarFam.Estado = Busqueda.bEstadoRegistroTipoActivo
            Coleccion.Add(BuscarFam)
        Next
        Return Coleccion
    End Function

    Public Function TipoActivoInserta(ByVal TipoActivo As LOGI_TIPOACTIVO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOACTIVO("SQL_INSERT", "", TipoActivo.cIdTipoActivo, TipoActivo.vDescripcionTipoActivo,
                                        TipoActivo.vDescripcionAbreviadaTipoActivo, TipoActivo.bEstadoRegistroTipoActivo, TipoActivo.cIdTipoActivo).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoActivoEdita(ByVal TipoActivo As LOGI_TIPOACTIVO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOACTIVO("SQL_UPDATE", "", TipoActivo.cIdTipoActivo, TipoActivo.vDescripcionTipoActivo,
                                     TipoActivo.vDescripcionAbreviadaTipoActivo, TipoActivo.bEstadoRegistroTipoActivo, TipoActivo.cIdTipoActivo).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoActivoElimina(ByVal TipoActivo As LOGI_TIPOACTIVO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_TIPOACTIVO("SQL_NONE", "UPDATE LOGI_TIPOACTIVO SET bEstadoRegistroTipoActivo = 0 WHERE cIdTipoActivo = '" & TipoActivo.cIdTipoActivo & "'",
                                        "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoActivoExiste(ByVal IdTipoActivo As String) As Boolean
        'If Data.PA_LOGI_MNT_TIPOACTIVO("SQL_NONE", "SELECT * FROM LOGI_TIPOACTIVO WHERE cIdTipoActivo = '" & IdTipoActivo & "' " &
        '                             " AND bEstadoRegistroTipoActivo = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_TIPOACTIVO("SQL_NONE", "SELECT * FROM LOGI_TIPOACTIVO WHERE cIdTipoActivo = '" & IdTipoActivo & "' ",
                                       "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
