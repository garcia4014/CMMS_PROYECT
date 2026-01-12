Imports System.Data.SqlClient

Public Class clsTablaSistemaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Dim MiConexion As New clsConexionDAO
    Dim cnx As New SqlConnection(MiConexion.GetCnx)

    Public Structure stCadenaATabla
        Dim vItem As String
    End Structure

    Public Function TablaSistemaGetData(strQuery As String) As DataTable
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

    'Public Function TablaSistemaListarCombo(ByVal IdTabla As String, ByVal IdSistema As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of GNRL_TABLASISTEMA)
    Public Function TablaSistemaListarCombo(ByVal IdTabla As String, ByVal IdSistema As String, ByVal IdEmpresa As String) As List(Of GNRL_TABLASISTEMA)
        'Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA WHERE cIdTablaSistema = '" & IdTabla & "' " &
        '                                             "AND cIdSistema = '" & IdSistema & "' AND bEstadoRegistroTablaSistema = 1 " &
        '                                             "AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "'",
        '                                             "", "", "", "", "", "1", "", "", "", "", "1", "")
        Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA WHERE cIdTablaSistema = '" & IdTabla & "' " &
                                                     "AND cIdSistema = '" & IdSistema & "' AND bEstadoRegistroTablaSistema = 1 " &
                                                     "AND cIdEmpresa = '" & IdEmpresa & "'",
                                                     "", "", "", "", "", "1", "", "", "", "1", "")
        Dim Coleccion As New List(Of GNRL_TABLASISTEMA)
        For Each TablaSistema In Consulta
            Dim TabSist As New GNRL_TABLASISTEMA
            TabSist.vValor = TablaSistema.vValor
            'TabSist.vDescripcionTablaSistema = TablaSistema.vDescripcionTablaSistema
            TabSist.vDescripcionTablaSistema = IIf(Trim(TablaSistema.vValorOpcionalTablaSistema) = "", TablaSistema.vDescripcionTablaSistema.ToString.Trim, Trim(TablaSistema.vValorOpcionalTablaSistema) & " - " & TablaSistema.vDescripcionTablaSistema.ToString.Trim)
            Coleccion.Add(TabSist)
        Next
        Return Coleccion
    End Function

    'Public Function TablaSistemaListarPorId(ByVal IdTabla As String, ByVal IdValor As String, ByVal IdSistema As String,
    '                                        ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, Optional ByVal Periodo As String = "*") As GNRL_TABLASISTEMA
    Public Function TablaSistemaListarPorId(ByVal IdTabla As String, ByVal IdValor As String, ByVal IdSistema As String,
                                            ByVal IdEmpresa As String, Optional ByVal Periodo As String = "*") As GNRL_TABLASISTEMA
        'Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA " &
        '                                                 "WHERE cIdTablaSistema = '" & IdTabla & "' " &
        '                                                 "      AND (vValor = '" & IdValor & "' OR '*' = '" & IdValor & "') AND cIdSistema = '" & IdSistema & "'" &
        '                                                 "      AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "'" &
        '                                                 "      AND (cIdPeriodoTablaSistema = '" & Periodo & "' OR '*' = '" & Periodo & "')", "", "", "", "", "", "1", "", "", "", "", "1", "")
        Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA " &
                                                         "WHERE cIdTablaSistema = '" & IdTabla & "' " &
                                                         "      AND (vValor = '" & IdValor & "' OR '*' = '" & IdValor & "') AND cIdSistema = '" & IdSistema & "'" &
                                                         "      AND cIdEmpresa = '" & IdEmpresa & "'" &
                                                         "      AND (cIdPeriodoTablaSistema = '" & Periodo & "' OR '*' = '" & Periodo & "')", "", "", "", "", "", "1", "", "", "", "1", "")
        Dim Coleccion As New GNRL_TABLASISTEMA
        For Each GNRL_TABLASISTEMA In Consulta
            Coleccion.cIdTablaSistema = GNRL_TABLASISTEMA.cIdTablaSistema
            Coleccion.vDescripcionTablaSistema = GNRL_TABLASISTEMA.vDescripcionTablaSistema
            Coleccion.vDescripcionAbreviadaTablaSistema = GNRL_TABLASISTEMA.vDescripcionAbreviadaTablaSistema
            Coleccion.bEstadoRegistroTablaSistema = GNRL_TABLASISTEMA.bEstadoRegistroTablaSistema
            Coleccion.vValor = GNRL_TABLASISTEMA.vValor
            Coleccion.vValorOpcionalTablaSistema = GNRL_TABLASISTEMA.vValorOpcionalTablaSistema
            Coleccion.cIdPeriodoTablaSistema = GNRL_TABLASISTEMA.cIdPeriodoTablaSistema
            Coleccion.cIdSistema = GNRL_TABLASISTEMA.cIdSistema
            Coleccion.cIdEmpresa = GNRL_TABLASISTEMA.cIdEmpresa
            'Coleccion.cIdPuntoVenta = GNRL_TABLASISTEMA.cIdPuntoVenta
            Coleccion.bEstadoEncryptadoTablaSistema = GNRL_TABLASISTEMA.bEstadoEncryptadoTablaSistema
        Next
        Return Coleccion
    End Function

    'Public Function TablaSistemaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Periodo As String, ByVal IdSistema As String,
    '                                      ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_GNRL_TABLASISTEMA)
    Public Function TablaSistemaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Periodo As String, ByVal IdSistema As String,
                                          ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_GNRL_TABLASISTEMA)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA " &
        '                                             "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroTablaSistema = 1 " &
        '                                             "      AND cIdPeriodoTablaSistema = '" & Periodo & "' AND cIdSistema = '" & IdSistema & "' " &
        '                                             "      AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "'",
        '                                             "", "", "", "", "", "1", "", "", "", "", "1", "")
        Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA " &
                                                     "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                     "      AND cIdPeriodoTablaSistema = '" & Periodo & "' AND cIdSistema = '" & IdSistema & "' " &
                                                     "      AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                     "      AND (bEstadoRegistroTablaSistema = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                     "", "", "", "", "", "1", "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_GNRL_TABLASISTEMA)
        For Each Busqueda In Consulta
            Dim BuscarTabSis As New VI_GNRL_TABLASISTEMA
            BuscarTabSis.IdTablaSistema = Busqueda.cIdTablaSistema
            BuscarTabSis.Codigo = Busqueda.vValor
            BuscarTabSis.Periodo = Busqueda.cIdPeriodoTablaSistema
            BuscarTabSis.Descripcion = Busqueda.vDescripcionTablaSistema
            BuscarTabSis.Valor = Busqueda.vValorOpcionalTablaSistema
            BuscarTabSis.Estado = Busqueda.bEstadoRegistroTablaSistema
            BuscarTabSis.IdSistema = Busqueda.cIdSistema
            Coleccion.Add(BuscarTabSis)
        Next
        Return Coleccion
    End Function

    'Public Function TablaSistemaListaGridPrincipal(ByVal Filtro As String, ByVal Buscar As String, ByVal Periodo As String, ByVal IdSistema As String,
    '                                               ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_GNRL_TABLASISTEMA)
    Public Function TablaSistemaListaGridPrincipal(ByVal Filtro As String, ByVal Buscar As String, ByVal Periodo As String, ByVal IdSistema As String,
                                                   ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_GNRL_TABLASISTEMA)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA " &
        '                                                 "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroTablaSistema = 1 " &
        '                                                 "      AND (cIdPeriodoTablaSistema = '" & Periodo & "' OR cIdPeriodoTablaSistema = '0000' OR '*' = '" & Periodo & "') " &
        '                                                 "      AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "'" &
        '                                                 "      AND cIdTablaSistema = '99' AND cIdSistema = '" & IdSistema & "' ORDER BY vValor",
        '                                                 "", "", "", "", "", "1", "", "", "", "", "1", "")
        Dim Consulta = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA " &
                                                         "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                         "      AND (cIdPeriodoTablaSistema = '" & Periodo & "' OR cIdPeriodoTablaSistema = '0000' OR '*' = '" & Periodo & "') " &
                                                         "      AND cIdEmpresa = '" & IdEmpresa & "'" &
                                                         "      AND (bEstadoRegistroTablaSistema = '" & Estado & "' OR '*' = '" & Estado & "')" &
                                                         "      AND cIdTablaSistema = '99' AND cIdSistema = '" & IdSistema & "' ORDER BY vValor",
                                                         "", "", "", "", "", "1", "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_GNRL_TABLASISTEMA)
        For Each Busqueda In Consulta
            Dim BuscarTabSis As New VI_GNRL_TABLASISTEMA
            BuscarTabSis.IdTablaSistema = Busqueda.cIdTablaSistema
            BuscarTabSis.Codigo = Busqueda.vValor
            BuscarTabSis.Periodo = Busqueda.cIdPeriodoTablaSistema
            BuscarTabSis.Descripcion = Busqueda.vDescripcionTablaSistema
            BuscarTabSis.Valor = Busqueda.vValorOpcionalTablaSistema
            BuscarTabSis.Estado = Busqueda.bEstadoRegistroTablaSistema
            BuscarTabSis.IdSistema = Busqueda.cIdSistema
            Coleccion.Add(BuscarTabSis)
        Next
        Return Coleccion
    End Function

    'Public Function TablaSistemaExiste(ByVal IdTabla As String, ByVal IdValor As String, ByVal IdSistema As String,
    '                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, Optional ByVal Periodo As String = "*") As Boolean
    Public Function TablaSistemaExiste(ByVal IdTabla As String, ByVal IdValor As String, ByVal IdSistema As String,
                                       ByVal IdEmpresa As String, Optional ByVal Periodo As String = "*") As Boolean
        'Se cambio, se adicionó el periodo 26/03/2018 - 21:14
        'If Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA WHERE cIdTablaSistema = '" & IdTabla & "' AND vValor = '" & IdValor & "' AND bEstadoRegistroTablaSistema = 1 AND cIdSistema = '" & IdSistema & "'" &
        '                                     "     AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "' AND (cIdPeriodoTablaSistema = '" & Periodo & "' Or '*' = '" & Periodo & "')",
        '                                     "", "", "", "", "", "1", "", "", "", "", "1", "").Count > 0 Then
        If Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "SELECT * FROM GNRL_TABLASISTEMA WHERE cIdTablaSistema = '" & IdTabla & "' AND vValor = '" & IdValor & "' AND bEstadoRegistroTablaSistema = 1 AND cIdSistema = '" & IdSistema & "'" &
                                             "     AND cIdEmpresa = '" & IdEmpresa & "' AND (cIdPeriodoTablaSistema = '" & Periodo & "' Or '*' = '" & Periodo & "')",
                                             "", "", "", "", "", "1", "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    'Public Function TablaSistemaCadenaATablaListarCombo(ByVal IdTabla As String, ByVal IdValor As String,
    '                                                    ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, Optional ByVal Periodo As String = "*", Optional ByVal Tabla As String = "GNRL_TABLASISTEMA") As List(Of String)
    Public Function TablaSistemaCadenaATablaListarCombo(ByVal IdTabla As String, ByVal IdValor As String,
                                                        ByVal IdEmpresa As String, Optional ByVal Periodo As String = "*", Optional ByVal Tabla As String = "GNRL_TABLASISTEMA") As List(Of String)
        'Dim strSQL As String = "SELECT * FROM dbo.F_CadenaATabla ('" & IdTabla & "', '" & IdValor & "', '" & Periodo & "', '" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & Tabla & "')"
        Dim strSQL As String = "SELECT * FROM dbo.F_CadenaATabla ('" & IdTabla & "', '" & IdValor & "', '" & Periodo & "', '" & IdEmpresa & "', '" & Tabla & "')"
        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Digito")

        Dim Coleccion As New List(Of String)

        If ds.Tables("Digito").Rows.Count > 0 Then
            Dim fila As DataRow
            For Each fila In ds.Tables("Digito").Rows
                If IsDBNull(fila("Items")) = True Then
                    Throw New Exception("No existen parámetros para este periodo")
                Else
                    Coleccion.Add(fila("Items"))
                End If
            Next
        End If
        Return Coleccion
    End Function

    'Public Function TablaSistemaParametroCuenta(ByVal IdTabla As String, ByVal IdValor As String, ByVal NombreCampo As String, ByVal IdPeriodo As String) As List(Of String)
    '    Dim strSQL As String = "SELECT * FROM dbo.F_ParametroCuenta ('" & IdTabla & "', '" & IdValor & "', '" & NombreCampo & "', '" & IdPeriodo & "')"
    '    Dim cmd As New SqlCommand(strSQL, cnx)
    '    Dim da As New SqlDataAdapter(cmd)
    '    Dim ds As New DataSet

    '    da.Fill(ds, "Digito")

    '    Dim Coleccion As New List(Of String)

    '    If ds.Tables("Digito").Rows.Count > 0 Then
    '        Dim fila As DataRow
    '        For Each fila In ds.Tables("Digito").Rows
    '            Coleccion.Add(fila("Items"))
    '        Next
    '    End If
    '    Return Coleccion
    'End Function

    Public Function TablaSistemaInserta(ByVal TablaSistema As GNRL_TABLASISTEMA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_INSERT", "", TablaSistema.cIdTablaSistema, TablaSistema.vValor, TablaSistema.vDescripcionTablaSistema,
                                          TablaSistema.vDescripcionAbreviadaTablaSistema, TablaSistema.vValorOpcionalTablaSistema,
                                          TablaSistema.bEstadoRegistroTablaSistema, TablaSistema.cIdPeriodoTablaSistema, TablaSistema.cIdSistema,
                                          TablaSistema.cIdEmpresa, TablaSistema.bEstadoEncryptadoTablaSistema,
                                          TablaSistema.cIdTablaSistema).ReturnValue.ToString
        Return x
    End Function

    Public Function TablaSistemaEdita(ByVal TablaSistema As GNRL_TABLASISTEMA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_UPDATE", "", TablaSistema.cIdTablaSistema, TablaSistema.vValor, TablaSistema.vDescripcionTablaSistema,
                                          TablaSistema.vDescripcionAbreviadaTablaSistema, TablaSistema.vValorOpcionalTablaSistema,
                                          TablaSistema.bEstadoRegistroTablaSistema, TablaSistema.cIdPeriodoTablaSistema, TablaSistema.cIdSistema,
                                          TablaSistema.cIdEmpresa, TablaSistema.bEstadoEncryptadoTablaSistema,
                                          TablaSistema.cIdTablaSistema).ReturnValue.ToString
        Return x
    End Function

    Public Function TablaSistemaElimina(ByVal TablaSistema As GNRL_TABLASISTEMA) As Int32
        Dim x
        'x = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "UPDATE GNRL_TABLASISTEMA SET bEstadoRegistroTablaSistema = 0 " &
        '                                  "WHERE cIdTablaSistema = '" & TablaSistema.cIdTablaSistema & "' AND vValor = '" & TablaSistema.vValor & "' AND cIdTablaSistema = '" & TablaSistema.cIdSistema & "' " &
        '                                  "      AND cIdEmpresa = '" & TablaSistema.cIdEmpresa & "' AND cIdPuntoVenta = '" & TablaSistema.cIdPuntoVenta & "'",
        '                                  "", "", "", "", "", "1", "", "", "", "", "1", "").ReturnValue.ToString
        x = Data.PA_GNRL_MNT_TABLASISTEMA("SQL_NONE", "UPDATE GNRL_TABLASISTEMA SET bEstadoRegistroTablaSistema = 0 " &
                                          "WHERE cIdTablaSistema = '" & TablaSistema.cIdTablaSistema & "' AND vValor = '" & TablaSistema.vValor & "' AND cIdTablaSistema = '" & TablaSistema.cIdSistema & "' " &
                                          "      AND cIdEmpresa = '" & TablaSistema.cIdEmpresa & "'",
                                          "", "", "", "", "", "1", "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function
End Class
