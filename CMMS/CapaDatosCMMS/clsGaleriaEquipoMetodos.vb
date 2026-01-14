Imports System.Data
Imports System.Data.SqlClient
Public Class clsGaleriaEquipoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function GaleriaEquipoGetData(strQuery As String) As DataTable
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

    Public Function GaleriaEquipoListarCombo() As List(Of LOGI_GALERIAEQUIPO)
        Dim Consulta = Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_NONE", "SELECT * FROM LOGI_GALERIAEQUIPO",
                                                      "", 0, "", "", "", "1", Now, "", "")
        Dim Coleccion As New List(Of LOGI_GALERIAEQUIPO)
        For Each GaleriaEquipo In Consulta
            Dim TipAct As New LOGI_GALERIAEQUIPO
            TipAct.cIdEquipo = GaleriaEquipo.cIdEquipo
            TipAct.vDescripcionGaleriaEquipo = GaleriaEquipo.vDescripcionGaleriaEquipo
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function

    'Public Function GaleriaEquipoListarPorId(ByVal IdGaleriaEquipo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_GALERIAEQUIPO
    Public Function GaleriaEquipoListarPorId(ByVal IdEquipo As String, ByVal IdNroItem As String) As LOGI_GALERIAEQUIPO
        Dim Consulta = Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_NONE", "SELECT * FROM LOGI_GALERIAEQUIPO WHERE cIdEquipo = '" & IdEquipo & "' AND nIdNumeroItemGaleriaEquipo = " & IdNroItem &
                                                   " AND bEstadoRegistroGaleriaEquipo = 1 ", "", 0, "", "", "", "1", Now, "", "")
        Dim Coleccion As New LOGI_GALERIAEQUIPO
        For Each LOGI_GALERIAEQUIPO In Consulta
            Coleccion.cIdEquipo = LOGI_GALERIAEQUIPO.cIdEquipo
            Coleccion.nIdNumeroItemGaleriaEquipo = LOGI_GALERIAEQUIPO.nIdNumeroItemGaleriaEquipo
            Coleccion.vDescripcionGaleriaEquipo = LOGI_GALERIAEQUIPO.vDescripcionGaleriaEquipo
            Coleccion.vObservacionGaleriaEquipo = LOGI_GALERIAEQUIPO.vObservacionGaleriaEquipo
            Coleccion.vTituloGaleriaEquipo = LOGI_GALERIAEQUIPO.vTituloGaleriaEquipo
            Coleccion.bEstadoRegistroGaleriaEquipo = LOGI_GALERIAEQUIPO.bEstadoRegistroGaleriaEquipo
            Coleccion.dFechaTransaccionGaleriaEquipo = LOGI_GALERIAEQUIPO.dFechaTransaccionGaleriaEquipo
            Coleccion.vNombreArchivoGaleriaEquipo = LOGI_GALERIAEQUIPO.vNombreArchivoGaleriaEquipo
        Next
        Return Coleccion
    End Function

    'Public Function GaleriaEquipoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_GALERIAEQUIPO)
    Public Function GaleriaEquipoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_GALERIAEQUIPO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_NONE", "SELECT * FROM LOGI_GALERIAEQUIPO " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroGaleriaEquipo = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", 0, "", "", "", "1", Now, "", "")
        Dim Coleccion As New List(Of VI_LOGI_GALERIAEQUIPO)
        For Each Busqueda In Consulta
            Dim BuscarGalEqu As New VI_LOGI_GALERIAEQUIPO
            BuscarGalEqu.Codigo = Busqueda.cIdEquipo
            BuscarGalEqu.NumeroItem = Busqueda.nIdNumeroItemGaleriaEquipo
            BuscarGalEqu.Titulo = Busqueda.vTituloGaleriaEquipo
            BuscarGalEqu.Descripcion = Busqueda.vDescripcionGaleriaEquipo
            BuscarGalEqu.Observacion = Busqueda.vObservacionGaleriaEquipo
            BuscarGalEqu.Estado = Busqueda.bEstadoRegistroGaleriaEquipo
            BuscarGalEqu.NombreArchivo = Busqueda.vNombreArchivoGaleriaEquipo
            Coleccion.Add(BuscarGalEqu)
        Next
        Return Coleccion
    End Function

    Public Function GaleriaEquipoInserta(ByVal GaleriaEquipo As LOGI_GALERIAEQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_INSERT", "", GaleriaEquipo.cIdEquipo, GaleriaEquipo.nIdNumeroItemGaleriaEquipo, GaleriaEquipo.vDescripcionGaleriaEquipo,
                                        GaleriaEquipo.vObservacionGaleriaEquipo, GaleriaEquipo.vTituloGaleriaEquipo, GaleriaEquipo.bEstadoRegistroGaleriaEquipo,
                                        GaleriaEquipo.dFechaTransaccionGaleriaEquipo, GaleriaEquipo.vNombreArchivoGaleriaEquipo, GaleriaEquipo.cIdEquipo).ReturnValue.ToString
        Return x
    End Function

    Public Function GaleriaEquipoEdita(ByVal GaleriaEquipo As LOGI_GALERIAEQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_UPDATE", "", GaleriaEquipo.cIdEquipo, GaleriaEquipo.nIdNumeroItemGaleriaEquipo, GaleriaEquipo.vDescripcionGaleriaEquipo,
                                        GaleriaEquipo.vObservacionGaleriaEquipo, GaleriaEquipo.vTituloGaleriaEquipo, GaleriaEquipo.bEstadoRegistroGaleriaEquipo,
                                        GaleriaEquipo.dFechaTransaccionGaleriaEquipo, GaleriaEquipo.vNombreArchivoGaleriaEquipo, GaleriaEquipo.cIdEquipo).ReturnValue.ToString
        Return x
    End Function

    Public Function GaleriaEquipoElimina(ByVal GaleriaEquipo As LOGI_GALERIAEQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_NONE", "UPDATE LOGI_GALERIAEQUIPO SET bEstadoRegistroGaleriaEquipo = 0 WHERE cIdEquipo = '" & GaleriaEquipo.cIdEquipo & "'",
                                           "", 0, "", "", "", "1", Now, "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function GaleriaEquipoExiste(ByVal IdEquipo As String, ByVal IdNroItem As String) As Boolean
        'If Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_NONE", "SELECT * FROM LOGI_GALERIAEQUIPO WHERE cIdGaleriaEquipo = '" & IdGaleriaEquipo & "' " &
        '                             " AND bEstadoRegistroGaleriaEquipo = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_GALERIAEQUIPO("SQL_NONE", "SELECT * FROM LOGI_GALERIAEQUIPO WHERE cIdEquipo = '" & IdEquipo & "' AND nIdNumeroItemGaleriaEquipo = " & IdNroItem,
                                          "", 0, "", "", "", "1", Now, "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
