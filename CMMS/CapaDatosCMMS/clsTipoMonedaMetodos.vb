Imports System.Data
Imports System.Data.SqlClient

Public Class clsTipoMonedaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function TipoMonedaGetData(strQuery As String) As DataTable
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

    Public Function TipoMonedaListarCombo() As List(Of GNRL_TIPOMONEDA)
        Dim Consulta = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "SELECT * FROM GNRL_TIPOMONEDA " &
                                                               "WHERE bEstadoRegistroTipoMoneda = 1",
                                                   "", "", "", "0", "1", 0, "", "", "", "")
        Dim Coleccion As New List(Of GNRL_TIPOMONEDA)
        For Each TipoMoneda In Consulta
            Dim TipMon As New GNRL_TIPOMONEDA
            TipMon.cIdTipoMoneda = TipoMoneda.cIdTipoMoneda
            TipMon.vDescripcionTipoMoneda = TipoMoneda.vDescripcionTipoMoneda
            Coleccion.Add(TipMon)
        Next
        Return Coleccion
    End Function

    Public Function TipoMonedaListarPorId(ByVal IdTipoMoneda As String) As GNRL_TIPOMONEDA
        Dim Consulta = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "SELECT * FROM GNRL_TIPOMONEDA " &
                                                               "WHERE cIdTipoMoneda = '" & IdTipoMoneda & "'",
                                                   "", "", "", "0", "1", 0, "", "", "", "")
        Dim Coleccion As New GNRL_TIPOMONEDA
        For Each GNRL_TIPOMONEDA In Consulta
            Coleccion.cIdTipoMoneda = GNRL_TIPOMONEDA.cIdTipoMoneda
            Coleccion.vDescripcionTipoMoneda = GNRL_TIPOMONEDA.vDescripcionTipoMoneda
            Coleccion.vDescripcionAbreviadaTipoMoneda = GNRL_TIPOMONEDA.vDescripcionAbreviadaTipoMoneda
            Coleccion.bMonedaBaseTipoMoneda = GNRL_TIPOMONEDA.bMonedaBaseTipoMoneda
            Coleccion.nOrdenTipoMoneda = GNRL_TIPOMONEDA.nOrdenTipoMoneda
            Coleccion.bEstadoRegistroTipoMoneda = GNRL_TIPOMONEDA.bEstadoRegistroTipoMoneda
            Coleccion.vIdEquivalenciaContableTipoMoneda = GNRL_TIPOMONEDA.vIdEquivalenciaContableTipoMoneda
            Coleccion.vIdEquivalenciaContableAbreviadaTipoMoneda = GNRL_TIPOMONEDA.vIdEquivalenciaContableAbreviadaTipoMoneda
            Coleccion.cIdPaisOrigenTipoMoneda = GNRL_TIPOMONEDA.cIdPaisOrigenTipoMoneda
        Next
        Return Coleccion
    End Function

    Public Function TipoMonedaListaGrid(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_TIPOMONEDA)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "SELECT * FROM GNRL_TIPOMONEDA " &
                                                               "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                               "      AND (bEstadoRegistroTipoMoneda = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                                   "", "", "", "0", "1", 0, "", "", "", "")
        Dim Coleccion As New List(Of VI_GNRL_TIPOMONEDA)
        For Each Busqueda In Consulta
            Dim BuscarTipMon As New VI_GNRL_TIPOMONEDA
            BuscarTipMon.Codigo = Busqueda.cIdTipoMoneda
            BuscarTipMon.Descripcion = Busqueda.vDescripcionTipoMoneda
            BuscarTipMon.Desc_Corta = Busqueda.vDescripcionAbreviadaTipoMoneda
            BuscarTipMon.Estado = Busqueda.bEstadoRegistroTipoMoneda
            Coleccion.Add(BuscarTipMon)
        Next
        Return Coleccion
    End Function

    Public Function TipoMonedaInserta(ByVal TipoMoneda As GNRL_TIPOMONEDA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_INSERT", "", TipoMoneda.cIdTipoMoneda, TipoMoneda.vDescripcionTipoMoneda, TipoMoneda.vDescripcionAbreviadaTipoMoneda, TipoMoneda.bMonedaBaseTipoMoneda, TipoMoneda.bEstadoRegistroTipoMoneda, TipoMoneda.nOrdenTipoMoneda, TipoMoneda.vIdEquivalenciaContableTipoMoneda, TipoMoneda.vIdEquivalenciaContableAbreviadaTipoMoneda, TipoMoneda.cIdPaisOrigenTipoMoneda, TipoMoneda.cIdTipoMoneda).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoMonedaEdita(ByVal TipoMoneda As GNRL_TIPOMONEDA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_UPDATE", "", TipoMoneda.cIdTipoMoneda, TipoMoneda.vDescripcionTipoMoneda, TipoMoneda.vDescripcionAbreviadaTipoMoneda, TipoMoneda.bMonedaBaseTipoMoneda, TipoMoneda.bEstadoRegistroTipoMoneda, TipoMoneda.nOrdenTipoMoneda, TipoMoneda.vIdEquivalenciaContableTipoMoneda, TipoMoneda.vIdEquivalenciaContableAbreviadaTipoMoneda, TipoMoneda.cIdPaisOrigenTipoMoneda, TipoMoneda.cIdTipoMoneda).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoMonedaElimina(ByVal TipoMoneda As GNRL_TIPOMONEDA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "UPDATE GNRL_TIPOMONEDA SET bEstadoRegistroTipoMoneda = 0 WHERE cIdTipoMoneda = '" & TipoMoneda.cIdTipoMoneda & "'",
                                        "", "", "", "0", "1", 0, "", "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoMonedaEditaBase(ByVal TipoMoneda As GNRL_TIPOMONEDA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "UPDATE GNRL_TIPOMONEDA SET bMonedaBaseTipoMoneda = 0 WHERE cIdTipoMoneda <> '" & TipoMoneda.cIdTipoMoneda & "'",
                                        "", "", "", "0", "1", 0, "", "", "", "").ReturnValue.ToString
        x = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "UPDATE GNRL_TIPOMONEDA SET bMonedaBaseTipoMoneda = 1 WHERE cIdTipoMoneda = '" & TipoMoneda.cIdTipoMoneda & "'",
                                        "", "", "", "0", "1", 0, "", "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoMonedaBase() As String
        'Este si puede devolver una colección de datos es decir varios registros
        TipoMonedaBase = "E"
        Dim Consulta = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "SELECT cIdTipoMoneda FROM GNRL_TIPOMONEDA WHERE bMonedaBaseTipoMoneda = 1 " &
                                                   "AND bEstadoRegistroTipoMoneda = 1", "", "", "", "0", "1", 0, "", "", "", "")
        For Each Busqueda In Consulta
            TipoMonedaBase = Busqueda.cIdTipoMoneda
        Next
    End Function

    Public Function TipoMonedaExtranjera() As String
        'Este si puede devolver una colección de datos es decir varios registros
        TipoMonedaExtranjera = "S"
        Dim Consulta = Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "SELECT TOP 1 cIdTipoMoneda FROM GNRL_TIPOMONEDA " &
                                                   "WHERE bMonedaBaseTipoMoneda = 0 AND bEstadoRegistroTipoMoneda = 1 " &
                                                   "ORDER BY nOrdenTipoMoneda", "", "", "", "0", "1", 0, "", "", "", "")
        For Each Busqueda In Consulta
            TipoMonedaExtranjera = Busqueda.cIdTipoMoneda
        Next
    End Function

    Public Function TipoMonedaExiste(ByVal IdTipoMoneda As String) As Boolean
        If Data.PA_GNRL_MNT_TIPOMONEDA("SQL_NONE", "SELECT * FROM GNRL_TIPOMONEDA WHERE cIdTipoMoneda = '" & IdTipoMoneda & "'", "", "", "", "0", "1", 0, "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class