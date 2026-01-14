Public Class clsUbicacionGeograficaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function UbicacionGeograficaListarPorPaisId(ByVal IdPais As String) As GNRL_UBICACIONGEOGRAFICA
        Dim Consulta = Data.PA_GNRL_MNT_UBICACIONGEOGRAFICA("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA " &
                                                   " WHERE cIdDepartamentoUbicacionGeografica = '00' AND cIdProvinciaUbicacionGeografica = '00' AND cIdDistritoUbicacionGeografica = '00' " &
                                                   " AND cIdPaisUbicacionGeografica = '" & IdPais & "' ",
                                                   "", "", "", "", "", "1", "")
        Dim Coleccion As New GNRL_UBICACIONGEOGRAFICA
        For Each GNRL_UBICACIONGEOGRAFICA In Consulta
            Coleccion.cIdPaisUbicacionGeografica = GNRL_UBICACIONGEOGRAFICA.cIdPaisUbicacionGeografica
            Coleccion.vDescripcionUbicacionGeografica = GNRL_UBICACIONGEOGRAFICA.vDescripcionUbicacionGeografica
        Next
        Return Coleccion
    End Function

    Public Function PaisListarCombo() As List(Of GNRL_UBICACIONGEOGRAFICA)
        'Dim Consulta = Data.pa_gnrl_mnt_configuracion("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA", "''", "''", "1", "")
        Dim Consulta = Data.PA_GNRL_MNT_UBICACIONGEOGRAFICA("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA WHERE cIdDepartamentoUbicacionGeografica = '00' and cIdProvinciaUbicacionGeografica = '00' and cIdDistritoUbicacionGeografica = '00'", "''", "''", "''", "''", "", "", "''")
        Dim Coleccion As New List(Of GNRL_UBICACIONGEOGRAFICA)
        For Each Ubicacion In Consulta
            Dim Ubi As New GNRL_UBICACIONGEOGRAFICA
            Ubi.cIdPaisUbicacionGeografica = Ubicacion.cIdPaisUbicacionGeografica
            Ubi.vDescripcionUbicacionGeografica = Ubicacion.vDescripcionUbicacionGeografica
            Coleccion.Add(Ubi)
        Next
        Return Coleccion
    End Function

    Public Function DepartamentoListarCombo(ByVal strPais As String) As List(Of GNRL_UBICACIONGEOGRAFICA)
        Dim Consulta = Data.PA_GNRL_MNT_UBICACIONGEOGRAFICA("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA WHERE cIdPaisUbicacionGeografica = '" & strPais & "' AND cIdDepartamentoUbicacionGeografica <> '00' AND cIdProvinciaUbicacionGeografica = '00' AND cIdDistritoUbicacionGeografica = '00'", "''", "''", "''", "''", "''", "", "''")
        Dim Coleccion As New List(Of GNRL_UBICACIONGEOGRAFICA)
        For Each Ubicacion In Consulta
            Dim Ubi As New GNRL_UBICACIONGEOGRAFICA
            Ubi.cIdDepartamentoUbicacionGeografica = Ubicacion.cIdDepartamentoUbicacionGeografica
            Ubi.vDescripcionUbicacionGeografica = Ubicacion.vDescripcionUbicacionGeografica
            Coleccion.Add(Ubi)
        Next
        Return Coleccion
    End Function

    Public Function ProvinciaListarCombo(ByVal strPais As String, ByVal strDpto As String) As List(Of GNRL_UBICACIONGEOGRAFICA)
        Dim Consulta = Data.PA_GNRL_MNT_UBICACIONGEOGRAFICA("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA WHERE cIdPaisUbicacionGeografica = '" & strPais & "' AND cIdDepartamentoUbicacionGeografica = '" & strDpto & "' AND cIdProvinciaUbicacionGeografica <> '00' AND cIdDistritoUbicacionGeografica = '00'", "''", "''", "''", "''", "''", "", "''")
        Dim Coleccion As New List(Of GNRL_UBICACIONGEOGRAFICA)
        For Each Ubicacion In Consulta
            Dim Ubi As New GNRL_UBICACIONGEOGRAFICA
            Ubi.cIdProvinciaUbicacionGeografica = Ubicacion.cIdProvinciaUbicacionGeografica
            Ubi.vDescripcionUbicacionGeografica = Ubicacion.vDescripcionUbicacionGeografica
            Coleccion.Add(Ubi)
        Next
        Return Coleccion
    End Function

    Public Function DistritoListarCombo(ByVal strPais As String, ByVal strDpto As String, ByVal strProvincia As String) As List(Of GNRL_UBICACIONGEOGRAFICA)
        Dim Consulta = Data.PA_GNRL_MNT_UBICACIONGEOGRAFICA("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA WHERE cIdPaisUbicacionGeografica ='" & strPais & "' AND cIdDepartamentoUbicacionGeografica = '" & strDpto & "' AND cIdProvinciaUbicacionGeografica = '" & strProvincia & "' AND cIdDistritoUbicacionGeografica <> '00'", "''", "''", "''", "''", "''", "", "''")
        Dim Coleccion As New List(Of GNRL_UBICACIONGEOGRAFICA)
        For Each Ubicacion In Consulta
            Dim Ubi As New GNRL_UBICACIONGEOGRAFICA
            Ubi.cIdDistritoUbicacionGeografica = Ubicacion.cIdDistritoUbicacionGeografica
            Ubi.vDescripcionUbicacionGeografica = Ubicacion.vDescripcionUbicacionGeografica
            Coleccion.Add(Ubi)
        Next
        Return Coleccion
    End Function

    Public Function UbicacionGeograficaListarPorId(ByVal IdPais As String, ByVal IdDepartamento As String, ByVal IdProvincia As String, ByVal IdDistrito As String) As GNRL_UBICACIONGEOGRAFICA
        Dim Consulta = Data.PA_GNRL_MNT_UBICACIONGEOGRAFICA("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA " &
                                                            "WHERE cIdPaisUbicacionGeografica ='" & IdPais & "' " &
                                                            IIf(IdDepartamento = "" Or IdDepartamento = "*", "      AND cIdDepartamentoUbicacionGeografica = '00' ", "      AND cIdDepartamentoUbicacionGeografica = '" & IdDepartamento & "' ") &
                                                            IIf(IdProvincia = "" Or IdProvincia = "*", "      AND cIdProvinciaUbicacionGeografica = '00' ", "      AND cIdProvinciaUbicacionGeografica = '" & IdProvincia & "' ") &
                                                            IIf(IdDistrito = "" Or IdDistrito = "*", "      AND cIdDistritoUbicacionGeografica = '" & IdDistrito & "'", "      AND cIdDistritoUbicacionGeografica = '" & IdDistrito & "'"),
                                                            "", "", "", "", "", "", "")
        Dim Coleccion As New GNRL_UBICACIONGEOGRAFICA
        For Each UbiGeo In Consulta
            Coleccion.cIdPaisUbicacionGeografica = UbiGeo.cIdPaisUbicacionGeografica
            Coleccion.cIdDepartamentoUbicacionGeografica = UbiGeo.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = UbiGeo.cIdProvinciaUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = UbiGeo.cIdDistritoUbicacionGeografica
            Coleccion.vDescripcionUbicacionGeografica = UbiGeo.vDescripcionUbicacionGeografica
            Coleccion.vIdEquivalenciaUbicacionGeografica = UbiGeo.vIdEquivalenciaUbicacionGeografica
        Next
        Return Coleccion
    End Function

    Public Function UbicacionGeograficaListarPorIdEquivalencia(ByVal IdEquivalenciaUbicacionGeografica As String) As GNRL_UBICACIONGEOGRAFICA
        Dim Consulta = Data.PA_GNRL_MNT_UBICACIONGEOGRAFICA("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGEOGRAFICA " &
                                                            "WHERE vIdEquivalenciaUbicacionGeografica ='" & IdEquivalenciaUbicacionGeografica & "' ",
                                                            "", "", "", "", "", "", "")
        Dim Coleccion As New GNRL_UBICACIONGEOGRAFICA
        For Each UbiGeo In Consulta
            Coleccion.cIdPaisUbicacionGeografica = UbiGeo.cIdPaisUbicacionGeografica
            Coleccion.cIdDepartamentoUbicacionGeografica = UbiGeo.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = UbiGeo.cIdProvinciaUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = UbiGeo.cIdDistritoUbicacionGeografica
            Coleccion.vDescripcionUbicacionGeografica = UbiGeo.vDescripcionUbicacionGeografica
            Coleccion.vIdEquivalenciaUbicacionGeografica = UbiGeo.vIdEquivalenciaUbicacionGeografica
        Next
        Return Coleccion
    End Function

    'Public Function UbicacionGeograficaExiste(ByVal IdPais As String, ByVal IdDpto As String, ByVal IdProvincia As String, ByVal IdDistrito As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
    '    'If Data.pa_gnrl_mnt_Cliente("SQL_NONE", "SELECT * FROM GNRL_Cliente WHERE cIdCliente = '" & idCliente & "'", "", "", "", "", "", "", "", "", "", "", "1", "").Count > 0 Then
    '    If Data.pa_gnrl_mnt_ubicaciongeografica("SQL_NONE", "SELECT * FROM GNRL_UBICACIONGREOGRAFICA CLIENTE WHERE cIdPaisUbicacionGeografica = '" & IdPais & "' cIdDepartamentoUbicacionGeografica = '" & IdDpto & "' AND cIdProvinciaUbicacionGeografica = '" & IdProvincia & "' AND cIdDistritoUbicacionGeografica = '" & IdDistrito & "' AND bEstadoRegistroCliente = 1", "''", "''", "''", "''", "''", "''").Count > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
End Class
