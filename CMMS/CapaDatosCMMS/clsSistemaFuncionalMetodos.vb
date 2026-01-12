Imports System.Data
Imports System.Data.SqlClient
Public Class clsSistemaFuncionalMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function SistemaFuncionalGetData(strQuery As String) As DataTable
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

    'Public Function SistemaFuncionalListarCombo(ByVal IdTipoActivo As String, ByVal IdCatalogo As String) As List(Of LOGI_SISTEMAFUNCIONAL)
    '    'Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL " &
    '    '                                                 "WHERE cIdTipoActivo = '" & IdTipoActivo & "' " &
    '    '                                                 "      AND cIdCatalogo = '" & IdCatalogo & "'",
    '    '                                           "", "", "", "", "", "1", "")
    '    Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL " &
    '                                                     "WHERE cIdCatalogo = '" & IdCatalogo & "'",
    '                                                     "", "", "", "1", "")
    '    Dim Coleccion As New List(Of LOGI_SISTEMAFUNCIONAL)
    '    For Each SistemaFuncional In Consulta
    '        Dim TipAct As New LOGI_SISTEMAFUNCIONAL
    '        TipAct.cIdSistemaFuncional = SistemaFuncional.cIdSistemaFuncional
    '        TipAct.vDescripcionSistemaFuncional = SistemaFuncional.vDescripcionSistemaFuncional
    '        Coleccion.Add(TipAct)
    '    Next
    '    Return Coleccion
    'End Function

    Public Function SistemaFuncionalListarCombo() As List(Of LOGI_SISTEMAFUNCIONAL)
        'Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL " &
        '                                                 "WHERE cIdTipoActivo = '" & IdTipoActivo & "' " &
        '                                                 "      AND cIdCatalogo = '" & IdCatalogo & "'",
        '                                           "", "", "", "", "", "1", "")
        Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL",
                                                         "", "", "", "1", "")
        Dim Coleccion As New List(Of LOGI_SISTEMAFUNCIONAL)
        For Each SistemaFuncional In Consulta
            Dim TipAct As New LOGI_SISTEMAFUNCIONAL
            TipAct.cIdSistemaFuncional = SistemaFuncional.cIdSistemaFuncional
            TipAct.vDescripcionSistemaFuncional = SistemaFuncional.vDescripcionSistemaFuncional
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function


    'Public Function SistemaFuncionalListarPorId(ByVal IdSistemaFuncional As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_SISTEMAFUNCIONAL
    Public Function SistemaFuncionalListarPorId(ByVal IdSistemaFuncional As String) As LOGI_SISTEMAFUNCIONAL
        Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL WHERE cIdSistemaFuncional = '" & IdSistemaFuncional &
                                                   "' AND bEstadoRegistroSistemaFuncional = 1", "", "", "", "1", "")
        Dim Coleccion As New LOGI_SISTEMAFUNCIONAL
        For Each LOGI_SISTEMAFUNCIONAL In Consulta
            Coleccion.cIdSistemaFuncional = LOGI_SISTEMAFUNCIONAL.cIdSistemaFuncional
            Coleccion.vDescripcionSistemaFuncional = LOGI_SISTEMAFUNCIONAL.vDescripcionSistemaFuncional
            Coleccion.vDescripcionAbreviadaSistemaFuncional = LOGI_SISTEMAFUNCIONAL.vDescripcionAbreviadaSistemaFuncional
            Coleccion.bEstadoRegistroSistemaFuncional = LOGI_SISTEMAFUNCIONAL.bEstadoRegistroSistemaFuncional
            'Coleccion.cIdCatalogo = LOGI_SISTEMAFUNCIONAL.cIdCatalogo
            'Coleccion.cIdEmpresa = LOGI_SISTEMAFUNCIONAL.cIdEmpresa
            'Coleccion.cIdTipoActivoSistemaFuncional = LOGI_SISTEMAFUNCIONAL.cIdTipoActivoSistemaFuncional
            'Coleccion.cIdJerarquiaCatalogo = LOGI_SISTEMAFUNCIONAL.cIdJerarquiaCatalogo
        Next
        Return Coleccion
    End Function

    'Public Function SistemaFuncionalListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_SISTEMAFUNCIONAL)
    Public Function SistemaFuncionalListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_SISTEMAFUNCIONAL)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT SISFUN.cIdSistemaFuncional, SISFUN.cIdCatalogo, SISFUN.cIdTipoActivo, SISFUN.vDescripcionSistemaFuncional, " &
        '                                                 "     SISFUN.vDescripcionAbreviadaSistemaFuncional, SISFUN.bEstadoRegistroSistemaFuncional, CAT.vDescripcionCatalogo " &
        '                                                 "FROM LOGI_SISTEMAFUNCIONAL AS SISFUN LEFT JOIN LOGI_CATALOGO AS CAT ON " &
        '                                                 "     SISFUN.cIdCatalogo = CAT.cIdCatalogo AND SISFUN.cIdTipoActivo = CAT.cIdTipoActivo " &
        '                                                 "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
        '                                                 "AND (SISFUN.bEstadoRegistroSistemaFuncional = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                                 "AND SISFUN.cIdJerarquiaCatalogo = CAT.cIdJerarquiaCatalogo " &
        '                                                 "ORDER BY SISFUN.cIdTipoActivo, SISFUN.cIdCatalogo, SISFUN.cIdSistemaFuncional ",
        '                                                 "", "", "", "", "", "1", "")
        'Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT SISFUN.cIdSistemaFuncional, SISFUN.cIdCatalogo, SISFUN.cIdTipoActivoSistemaFuncional, SISFUN.vDescripcionSistemaFuncional, " &
        '                                                 "     SISFUN.vDescripcionAbreviadaSistemaFuncional, SISFUN.bEstadoRegistroSistemaFuncional, CAT.vDescripcionCatalogo " &
        '                                                 "FROM LOGI_SISTEMAFUNCIONAL AS SISFUN LEFT JOIN LOGI_CATALOGO AS CAT ON " &
        '                                                 "     SISFUN.cIdCatalogo = CAT.cIdCatalogo " &
        '                                                 "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
        '                                                 "AND (SISFUN.bEstadoRegistroSistemaFuncional = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                                 "AND SISFUN.cIdJerarquiaCatalogo = CAT.cIdJerarquiaCatalogo " &
        '                                                 "ORDER BY SISFUN.cIdCatalogo, SISFUN.cIdSistemaFuncional ",
        '                                                 "", "", "", "", "1", "")
        Dim Consulta = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT SISFUN.cIdSistemaFuncional, SISFUN.vDescripcionSistemaFuncional, " &
                                                         "     SISFUN.vDescripcionAbreviadaSistemaFuncional, SISFUN.bEstadoRegistroSistemaFuncional " &
                                                         "FROM LOGI_SISTEMAFUNCIONAL AS SISFUN " &
                                                         "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                         "AND (SISFUN.bEstadoRegistroSistemaFuncional = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                         "ORDER BY SISFUN.cIdSistemaFuncional ",
                                                         "", "", "", "1", "")

        Dim Coleccion As New List(Of VI_LOGI_SISTEMAFUNCIONAL)
        For Each Busqueda In Consulta
            Dim BuscarSisFun As New VI_LOGI_SISTEMAFUNCIONAL
            BuscarSisFun.Codigo = Busqueda.cIdSistemaFuncional
            BuscarSisFun.Descripcion = Busqueda.vDescripcionSistemaFuncional
            BuscarSisFun.Estado = Busqueda.bEstadoRegistroSistemaFuncional
            'BuscarSisFun.DescripcionCatalogo = Busqueda.vDescripcionCatalogo
            'BuscarSisFun.IdCatalogo = Busqueda.cIdCatalogo
            'BuscarSisFun.IdTipoActivo = Busqueda.cIdTipoActivoSistemaFuncional
            Coleccion.Add(BuscarSisFun)
        Next
        Return Coleccion
    End Function

    Public Function SistemaFuncionalInserta(ByVal SistemaFuncional As LOGI_SISTEMAFUNCIONAL) As Int32
        Dim x
        'x = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_INSERT", "", SistemaFuncional.cIdSistemaFuncional, SistemaFuncional.cIdCatalogo, SistemaFuncional.cIdTipoActivoSistemaFuncional,
        '                                      SistemaFuncional.cIdJerarquiaCatalogo, SistemaFuncional.vDescripcionSistemaFuncional, SistemaFuncional.vDescripcionAbreviadaSistemaFuncional, SistemaFuncional.bEstadoRegistroSistemaFuncional,
        '                                      SistemaFuncional.cIdSistemaFuncional).ReturnValue.ToString
        x = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_INSERT", "", SistemaFuncional.cIdSistemaFuncional,
                                              SistemaFuncional.vDescripcionSistemaFuncional, SistemaFuncional.vDescripcionAbreviadaSistemaFuncional, SistemaFuncional.bEstadoRegistroSistemaFuncional,
                                              SistemaFuncional.cIdSistemaFuncional).ReturnValue.ToString
        Return x
    End Function

    Public Function SistemaFuncionalEdita(ByVal SistemaFuncional As LOGI_SISTEMAFUNCIONAL) As Int32
        Dim x
        'x = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_UPDATE", "", SistemaFuncional.cIdSistemaFuncional, SistemaFuncional.cIdCatalogo, SistemaFuncional.cIdTipoActivoSistemaFuncional,
        '                                      SistemaFuncional.cIdJerarquiaCatalogo, SistemaFuncional.vDescripcionSistemaFuncional, SistemaFuncional.vDescripcionAbreviadaSistemaFuncional, SistemaFuncional.bEstadoRegistroSistemaFuncional,
        '                                      SistemaFuncional.cIdSistemaFuncional).ReturnValue.ToString
        x = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_UPDATE", "", SistemaFuncional.cIdSistemaFuncional,
                                              SistemaFuncional.vDescripcionSistemaFuncional, SistemaFuncional.vDescripcionAbreviadaSistemaFuncional, SistemaFuncional.bEstadoRegistroSistemaFuncional,
                                              SistemaFuncional.cIdSistemaFuncional).ReturnValue.ToString
        Return x
    End Function

    Public Function SistemaFuncionalElimina(ByVal SistemaFuncional As LOGI_SISTEMAFUNCIONAL) As Int32
        Dim x
        'x = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "UPDATE LOGI_SISTEMAFUNCIONAL SET bEstadoRegistroSistemaFuncional = 0 WHERE cIdSistemaFuncional = '" & SistemaFuncional.cIdSistemaFuncional & "' AND cIdCatalogo = '" & SistemaFuncional.cIdCatalogo & "'",
        '                                "", "", "", "", "1", "").ReturnValue.ToString
        x = Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "UPDATE LOGI_SISTEMAFUNCIONAL SET bEstadoRegistroSistemaFuncional = 0 WHERE cIdSistemaFuncional = '" & SistemaFuncional.cIdSistemaFuncional & "'",
                                              "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    'Public Function SistemaFuncionalExiste(ByVal IdSistemaFuncional As String, ByVal IdCatalogo As String) As Boolean
    Public Function SistemaFuncionalExiste(ByVal IdSistemaFuncional As String) As Boolean
        'If Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL WHERE cIdSistemaFuncional = '" & IdSistemaFuncional & "' AND cIdCatalogo = '" & IdCatalogo & "' " & _
        '                             " AND bEstadoRegistroSistemaFuncional = 1", "", "", "", "", "", "1", "").Count > 0 Then
        'If Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL WHERE cIdSistemaFuncional = '" & IdSistemaFuncional & "' AND cIdCatalogo = '" & IdCatalogo & "'",
        '                            "", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_SISTEMAFUNCIONAL("SQL_NONE", "SELECT * FROM LOGI_SISTEMAFUNCIONAL WHERE cIdSistemaFuncional = '" & IdSistemaFuncional & "'",
                                             "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class