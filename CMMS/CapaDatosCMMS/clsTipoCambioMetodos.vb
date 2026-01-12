Imports System.Data
Imports System.Data.SqlClient

Public Class clsTipoCambioMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function TipoCambioGetData(strQuery As String) As DataTable
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

    'Public Function TipoCambioPeriodoListarCombo() As List(Of GNRL_TIPOCAMBIO)
    Public Function TipoCambioPeriodoListarCombo() As List(Of Integer)
        Dim Consulta = Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_NONE", "SELECT CONVERT (NUMERIC(10,3), YEAR(dFechaHoraTipoCambio)) nTipoCambioVenta " &
                                                   "FROM GNRL_TIPOCAMBIO WHERE bEstadoRegistroTipoCambio = 1 GROUP BY YEAR(dFechaHoraTipoCambio) " &
                                                   "ORDER BY nTipoCambioVenta DESC", Now, "0", "0", "", "1", "", "")

        Dim TipCam As New List(Of Integer)
        For Each TipoCambio In Consulta
            'Dim TipCam As New List(Of Integer)
            TipCam.Add(TipoCambio.nTipoCambioVenta)
            'Coleccion.Add(TipCam)
        Next
        Return TipCam
        'Dim Coleccion As New List(Of GNRL_TIPOCAMBIO)
        'For Each TipoCambio In Consulta
        '    Dim TipCam As New GNRL_TIPOCAMBIO
        '    TipCam.nTipoCambioVenta = TipoCambio.nTipoCambioVenta
        '    'TipMon.dfech = TipoMoneda.vDescripcionTipoMoneda
        '    Coleccion.Add(TipCam)
        'Next
        'Return Coleccion
    End Function

    'Public Function TipoCambioListarPorId(ByVal IdTipoCambio As DateTime, ByVal IdTipoMoneda As String) As GNRL_TIPOCAMBIO
    '    Dim Consulta = Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_NONE", "SELECT * FROM GNRL_TIPOCAMBIO " & _
    '                                               "WHERE CONVERT (CHAR(10), dFechaHoraTipoCambio, 103) = '" & String.Format("{0:dd/MM/yyyy}", IdTipoCambio) & "' " & _
    '                                               "      AND cIdTipoMoneda = '" & IdTipoMoneda & "'", Now, "0", "0", "", "1")
    '    Dim Coleccion As New GNRL_TIPOCAMBIO
    '    For Each GNRL_TIPOCAMBIO In Consulta
    '        Coleccion.dFechaHoraTipoCambio = GNRL_TIPOCAMBIO.dFechaHoraTipoCambio
    '        Coleccion.cIdTipoMoneda = GNRL_TIPOCAMBIO.cIdTipoMoneda
    '        Coleccion.nTipoCambioCompra = GNRL_TIPOCAMBIO.nTipoCambioCompra
    '        Coleccion.nTipoCambioVenta = GNRL_TIPOCAMBIO.nTipoCambioVenta
    '        Coleccion.bEstadoRegistroTipoCambio = GNRL_TIPOCAMBIO.bEstadoRegistroTipoCambio
    '    Next
    '    Return Coleccion
    'End Function
    Public Function TipoCambioListarPorId(ByVal IdTipoCambio As DateTime, ByVal IdTipoMoneda As String, ByVal IdPaisOrigen As String) As GNRL_TIPOCAMBIO
        Dim Consulta = Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_NONE", "SELECT * FROM GNRL_TIPOCAMBIO " &
                                                   "WHERE CONVERT (CHAR(10), dFechaHoraTipoCambio, 103) = '" & String.Format("{0:dd/MM/yyyy}", IdTipoCambio) & "' " &
                                                   "      AND cIdTipoMoneda = '" & IdTipoMoneda & "' AND cIdPaisOrigenTipoCambio = '" & IdPaisOrigen & "'", Now, "0", "0", "", "1", "", "")
        Dim Coleccion As New GNRL_TIPOCAMBIO
        For Each GNRL_TIPOCAMBIO In Consulta
            Coleccion.dFechaHoraTipoCambio = GNRL_TIPOCAMBIO.dFechaHoraTipoCambio
            Coleccion.cIdTipoMoneda = GNRL_TIPOCAMBIO.cIdTipoMoneda
            Coleccion.nTipoCambioCompra = GNRL_TIPOCAMBIO.nTipoCambioCompra
            Coleccion.nTipoCambioVenta = GNRL_TIPOCAMBIO.nTipoCambioVenta
            Coleccion.bEstadoRegistroTipoCambio = GNRL_TIPOCAMBIO.bEstadoRegistroTipoCambio
            Coleccion.cIdTipoMonedaBaseTipoCambio = GNRL_TIPOCAMBIO.cIdTipoMonedaBaseTipoCambio
            Coleccion.cIdPaisOrigenTipoCambio = GNRL_TIPOCAMBIO.cIdPaisOrigenTipoCambio
        Next
        Return Coleccion
    End Function

    Public Function TipoCambioListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_GNRL_TIPOCAMBIO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_NONE", "SELECT * FROM GNRL_TIPOCAMBIO " &
                                                   " WHERE " & IIf(Filtro = "dFechaHoraTipoCambio", "CONVERT (CHAR(10), " & Filtro & ", 103)  LIKE UPPER ('%" & Buscar & "%') ",
                                                                   Filtro & " LIKE UPPER ('%" & Buscar & "%') ") &
                                                   "      AND (bEstadoRegistroTipoCambio = '" & Estado & "' OR '*' = '" & Estado & "')" &
                                                   "ORDER BY dFechaHoraTipoCambio DESC", Now, "0", "0", "", "1", "", "")

        Dim Coleccion As New List(Of VI_GNRL_TIPOCAMBIO)
        For Each Busqueda In Consulta
            Dim BuscarTipCam As New VI_GNRL_TIPOCAMBIO
            BuscarTipCam.Fecha = Busqueda.dFechaHoraTipoCambio
            BuscarTipCam.Tip_Mon = Busqueda.cIdTipoMoneda
            BuscarTipCam.TC_Compra = Busqueda.nTipoCambioCompra
            BuscarTipCam.TC_Venta = Busqueda.nTipoCambioVenta
            BuscarTipCam.Estado = Busqueda.bEstadoRegistroTipoCambio
            Coleccion.Add(BuscarTipCam)
        Next
        Return Coleccion
    End Function

    Public Function TipoCambioInserta(ByVal TipoCambio As GNRL_TIPOCAMBIO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_INSERT", "", TipoCambio.dFechaHoraTipoCambio, TipoCambio.nTipoCambioVenta, TipoCambio.nTipoCambioCompra, TipoCambio.cIdTipoMoneda, TipoCambio.bEstadoRegistroTipoCambio, TipoCambio.cIdTipoMonedaBaseTipoCambio, TipoCambio.cIdPaisOrigenTipoCambio).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoCambioEdita(ByVal TipoCambio As GNRL_TIPOCAMBIO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_UPDATE", "", TipoCambio.dFechaHoraTipoCambio, TipoCambio.nTipoCambioVenta, TipoCambio.nTipoCambioCompra, TipoCambio.cIdTipoMoneda, TipoCambio.bEstadoRegistroTipoCambio, TipoCambio.cIdTipoMonedaBaseTipoCambio, TipoCambio.cIdPaisOrigenTipoCambio).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoCambioElimina(ByVal TipoCambio As GNRL_TIPOCAMBIO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_NONE", "UPDATE GNRL_TIPOCAMBIO SET bEstadoRegistroTipoCambio = 0 WHERE CONVERT (CHAR(10), dFechaHoraTipoCambio, 103) = '" & (String.Format("{0:dd/MM/yyyy}", TipoCambio.dFechaHoraTipoCambio)) & "'",
                                        Now, "0", "0", "", "1", "", "").ReturnValue.ToString
        Return x
    End Function

    'Public Function TipoCambioExiste(ByVal IdTipoCambio As DateTime, ByVal IdTipoMoneda As String) As Boolean
    '    If Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_NONE", "SELECT * FROM GNRL_TIPOCAMBIO " &
    '                                   "WHERE CONVERT (CHAR(10), dFechaHoraTipoCambio, 103) = '" & (String.Format("{0:dd/MM/yyyy}", IdTipoCambio)) & "' " &
    '                                   "      AND bEstadoRegistroTipoCambio = 1 AND cIdTipoMoneda = '" & IdTipoMoneda & "'", Now, "0", "0", "", "1", "", "").Count > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    Public Function TipoCambioExiste(ByVal IdTipoCambio As DateTime, ByVal IdTipoMoneda As String, ByVal IdPaisOrigen As String) As Boolean
        If Data.PA_GNRL_MNT_TIPOCAMBIO("SQL_NONE", "SELECT * FROM GNRL_TIPOCAMBIO " &
                                       "WHERE CONVERT (CHAR(10), dFechaHoraTipoCambio, 103) = '" & (String.Format("{0:dd/MM/yyyy}", IdTipoCambio)) & "' " &
                                       "      AND bEstadoRegistroTipoCambio = 1 AND cIdTipoMoneda = '" & IdTipoMoneda & "' AND cIdPaisOrigenTipoCambio = '" & IdPaisOrigen & "'", Now, "0", "0", "", "1", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
