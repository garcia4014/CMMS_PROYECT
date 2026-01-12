Imports System.Data
Imports System.Data.SqlClient

Public Class clsLocalMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function LocalGetData(strQuery As String) As DataTable
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

    Public Function LocalListarCombo(ByVal IdEmpresa As String) As List(Of GNRL_LOCAL)
        Dim Consulta = Data.PA_GNRL_MNT_LOCAL("SQL_NONE", "SELECT * FROM GNRL_LOCAL WHERE cIdEmpresa = '" & IdEmpresa & "' AND bEstadoRegistroLocal = 1",
                                              "", "", "", "", "", 0, "", "1", "", "")
        Dim Coleccion As New List(Of GNRL_LOCAL)
        For Each Configuracion In Consulta
            Dim Config As New GNRL_LOCAL
            Config.cIdLocal = Configuracion.cIdLocal
            Config.vDescripcionLocal = Configuracion.vDescripcionLocal
            Coleccion.Add(Config)
        Next
        Return Coleccion
    End Function

    Public Function LocalEmpresaListarCombo(ByVal IdEmpresa As String, ByVal Estado As String) As List(Of GNRL_LOCAL)
        Dim Consulta = Data.PA_GNRL_MNT_LOCAL("SQL_NONE", "SELECT * FROM GNRL_LOCAL " &
                                                "WHERE cIdEmpresa = '" & IdEmpresa & "' " &
                                                "      AND (bEstadoRegistroLocal = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                                "", "", "", "", "", 0, "", "1", "", "")
        Dim Coleccion As New List(Of GNRL_LOCAL)
        For Each Configuracion In Consulta
            Dim Config As New GNRL_LOCAL
            Config.cIdLocal = Configuracion.cIdLocal
            Config.vDescripcionLocal = Configuracion.vDescripcionLocal
            Coleccion.Add(Config)
        Next
        Return Coleccion
    End Function

    Public Function LocalListarPorId(ByVal IdLocal As String, ByVal IdEmpresa As String) As GNRL_LOCAL
        Dim Consulta = Data.PA_GNRL_MNT_LOCAL("SQL_NONE", "SELECT * FROM GNRL_LOCAL " &
                                              "WHERE cIdLocal = '" & IdLocal & "' AND cIdEmpresa = '" & IdEmpresa & "'",
                                              "", "", "", "", "", 0, "", "1", "", "")
        Dim Coleccion As New GNRL_LOCAL
        For Each GNRL_LOCAL In Consulta
            Coleccion.cIdLocal = GNRL_LOCAL.cIdLocal
            Coleccion.cIdEmpresa = GNRL_LOCAL.cIdEmpresa
            Coleccion.vDescripcionLocal = GNRL_LOCAL.vDescripcionLocal
            Coleccion.vDescripcionAbreviadaLocal = GNRL_LOCAL.vDescripcionAbreviadaLocal
            Coleccion.vDireccionLocal = GNRL_LOCAL.vDireccionLocal
            Coleccion.nAforoLocal = GNRL_LOCAL.nAforoLocal
            Coleccion.vIdLocalAnexoSunat = GNRL_LOCAL.vIdLocalAnexoSunat
            Coleccion.bEstadoRegistroLocal = GNRL_LOCAL.bEstadoRegistroLocal
            Coleccion.vIdEquivalenciaUbicacionGeografica = GNRL_LOCAL.vIdEquivalenciaUbicacionGeografica
        Next
        Return Coleccion
    End Function

    Public Function LocalListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_GNRL_LOCAL)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_LOCAL("SQL_NONE", "SELECT * FROM GNRL_LOCAL " &
                                              "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND cIdEmpresa = '" & IdEmpresa & "'" &
                                              "      AND (bEstadoRegistroLocal = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                              "", "", "", "", "", 0, "", "1", "", "")
        Dim Coleccion As New List(Of VI_GNRL_LOCAL)
        For Each Busqueda In Consulta
            Dim BuscarEmp As New VI_GNRL_LOCAL
            BuscarEmp.Codigo = Busqueda.cIdLocal
            BuscarEmp.Descripcion = Busqueda.vDescripcionLocal
            BuscarEmp.Estado = Busqueda.bEstadoRegistroLocal
            Coleccion.Add(BuscarEmp)
        Next
        Return Coleccion
    End Function

    Public Function LocalInserta(ByVal Local As GNRL_LOCAL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_LOCAL("SQL_INSERT", "", Local.cIdLocal, Local.cIdEmpresa, Local.vDescripcionLocal, Local.vDescripcionAbreviadaLocal,
                                   Local.vDireccionLocal, Local.nAforoLocal, Local.vIdLocalAnexoSunat, Local.bEstadoRegistroLocal,
                                   Local.vIdEquivalenciaUbicacionGeografica, Local.cIdLocal).ReturnValue.ToString
        Return x
    End Function

    Public Function LocalEdita(ByVal Local As GNRL_LOCAL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_LOCAL("SQL_UPDATE", "", Local.cIdLocal, Local.cIdEmpresa, Local.vDescripcionLocal, Local.vDescripcionAbreviadaLocal,
                                   Local.vDireccionLocal, Local.nAforoLocal, Local.vIdLocalAnexoSunat, Local.bEstadoRegistroLocal,
                                   Local.vIdEquivalenciaUbicacionGeografica, Local.cIdLocal).ReturnValue.ToString
        Return x
    End Function

    Public Function LocalElimina(ByVal Local As GNRL_LOCAL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_LOCAL("SQL_NONE", "UPDATE GNRL_LOCAL SET bEstadoRegistroLocal = 0 WHERE cIdLocal = '" & Local.cIdLocal & "'",
                                   "", "", "", "", "", 0, "", "1", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function LocalExiste(ByVal IdLocal As String, ByVal IdEmpresa As String) As Boolean
        'If Data.PA_GNRL_MNT_LOCAL("SQL_NONE", "SELECT * FROM GNRL_LOCAL WHERE cIdLocal = '" & IdLocal & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
        '                          "AND bEstadoRegistroLocal = 1",
        '                          "", "", "", "", "", 0, "", "1", "").Count > 0 Then
        If Data.PA_GNRL_MNT_LOCAL("SQL_NONE", "SELECT * FROM GNRL_LOCAL WHERE cIdLocal = '" & IdLocal & "' AND cIdEmpresa = '" & IdEmpresa & "' ",
                                  "", "", "", "", "", 0, "", "1", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
