Imports System.Data
Imports System.Data.SqlClient

Public Class clsPuntoVentaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function PuntoVentaGetData(strQuery As String) As DataTable
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

    Public Function PuntoVentaListarCombo(ByVal IdEmpresa As String, ByVal IdLocal As String) As List(Of GNRL_PUNTOVENTA)
        Dim Consulta = Data.PA_GNRL_MNT_PUNTOVENTA("SQL_NONE", "SELECT * FROM GNRL_PUNTOVENTA WHERE cIdEmpresa = '" & IdEmpresa & "' AND (cIdLocal = '" & IdLocal & "' OR '*' = '" & IdLocal & "') AND bEstadoRegistroPuntoVenta = 1", "''", "''", "", "", "", "1", "", "1", "", "")
        Dim Coleccion As New List(Of GNRL_PUNTOVENTA)
        For Each PuntoVenta In Consulta
            Dim PtoVta As New GNRL_PUNTOVENTA
            PtoVta.cIdPuntoVenta = PuntoVenta.cIdPuntoVenta
            PtoVta.vDescripcionPuntoVenta = PuntoVenta.vDescripcionPuntoVenta
            Coleccion.Add(PtoVta)
        Next
        Return Coleccion
    End Function

    Public Function PuntoVentaListarPorId(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String) As GNRL_PUNTOVENTA
        Dim Consulta = Data.PA_GNRL_MNT_PUNTOVENTA("SQL_NONE", "SELECT * FROM GNRL_PUNTOVENTA WHERE cIdPuntoVenta = '" & IdPuntoVenta &
                                                   "' AND bEstadoRegistroPuntoVenta = 1 AND cIdEmpresa = '" & IdEmpresa & "'", "", "", "", "", "", "1", "", "1", "", "")
        Dim Coleccion As New GNRL_PUNTOVENTA
        For Each GNRL_PUNTOVENTA In Consulta
            Coleccion.cIdPuntoVenta = GNRL_PUNTOVENTA.cIdPuntoVenta
            Coleccion.cIdEmpresa = GNRL_PUNTOVENTA.cIdEmpresa
            Coleccion.vDescripcionPuntoVenta = GNRL_PUNTOVENTA.vDescripcionPuntoVenta
            Coleccion.vDescripcionAbreviadaPuntoVenta = GNRL_PUNTOVENTA.vDescripcionAbreviadaPuntoVenta
            Coleccion.vDireccionPuntoVenta = GNRL_PUNTOVENTA.vDireccionPuntoVenta
            Coleccion.bEstadoRegistroPuntoVenta = GNRL_PUNTOVENTA.bEstadoRegistroPuntoVenta
            'Coleccion.cIdLocalAnexoSunat = GNRL_PUNTOVENTA.cIdLocalAnexoSunat
            Coleccion.cIdLocal = GNRL_PUNTOVENTA.cIdLocal
            Coleccion.bFacturacionElectronicaPuntoVenta = GNRL_PUNTOVENTA.bFacturacionElectronicaPuntoVenta
            Coleccion.vTelefonoPuntoVenta = GNRL_PUNTOVENTA.vTelefonoPuntoVenta
        Next
        Return Coleccion
    End Function

    Public Function PuntoVentaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_GNRL_PUNTOVENTA)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_PUNTOVENTA("SQL_NONE", "SELECT * FROM GNRL_PUNTOVENTA " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                   "      AND (bEstadoRegistroPuntoVenta = '" & Estado & "' OR '*' = '" & Estado & "')" &
                                                   "      AND cIdEmpresa = '" & IdEmpresa & "'", "", "", "", "", "", "1", "", "1", "", "")
        Dim Coleccion As New List(Of VI_GNRL_PUNTOVENTA)
        For Each Busqueda In Consulta
            Dim BuscarPtoVta As New VI_GNRL_PUNTOVENTA
            BuscarPtoVta.Codigo = Busqueda.cIdPuntoVenta
            BuscarPtoVta.Descripcion = Busqueda.vDescripcionPuntoVenta
            BuscarPtoVta.Direccion = Busqueda.vDireccionPuntoVenta
            BuscarPtoVta.Estado = Busqueda.bEstadoRegistroPuntoVenta
            Coleccion.Add(BuscarPtoVta)
        Next
        Return Coleccion
    End Function

    Public Function PuntoVentaInserta(ByVal PuntoVenta As GNRL_PUNTOVENTA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_PUNTOVENTA("SQL_INSERT", "", PuntoVenta.cIdPuntoVenta, PuntoVenta.cIdEmpresa, PuntoVenta.vDescripcionPuntoVenta, PuntoVenta.vDescripcionAbreviadaPuntoVenta, PuntoVenta.vDireccionPuntoVenta, PuntoVenta.bEstadoRegistroPuntoVenta, PuntoVenta.vTelefonoPuntoVenta, PuntoVenta.bFacturacionElectronicaPuntoVenta, PuntoVenta.cIdLocal, PuntoVenta.cIdEmpresa).ReturnValue.ToString
        Return x
    End Function

    Public Function PuntoVentaEdita(ByVal PuntoVenta As GNRL_PUNTOVENTA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_PUNTOVENTA("SQL_UPDATE", "", PuntoVenta.cIdPuntoVenta, PuntoVenta.cIdEmpresa, PuntoVenta.vDescripcionPuntoVenta, PuntoVenta.vDescripcionAbreviadaPuntoVenta, PuntoVenta.vDireccionPuntoVenta, PuntoVenta.bEstadoRegistroPuntoVenta, PuntoVenta.vTelefonoPuntoVenta, PuntoVenta.bFacturacionElectronicaPuntoVenta, PuntoVenta.cIdLocal, PuntoVenta.cIdPuntoVenta).ReturnValue.ToString
        Return x
    End Function

    Public Function PuntoVentaElimina(ByVal PuntoVenta As GNRL_PUNTOVENTA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_PUNTOVENTA("SQL_NONE", "UPDATE GNRL_PUNTOVENTA SET bEstadoRegistroPuntoVenta = 0 WHERE cIdPuntoVenta = '" & PuntoVenta.cIdPuntoVenta & "' AND cIdEmpresa = '" & PuntoVenta.cIdEmpresa & "'",
                                        "", "", "", "", "", "1", "", "1", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function PuntoVentaExiste(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String) As Boolean
        If Data.PA_GNRL_MNT_PUNTOVENTA("SQL_NONE", "SELECT * FROM GNRL_PUNTOVENTA WHERE cIdPuntoVenta = '" & IdPuntoVenta &
                                       "' AND bEstadoRegistroPuntoVenta = 1 AND cIdEmpresa = '" & IdEmpresa & "'", "", "", "", "", "", "1", "", "1", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class