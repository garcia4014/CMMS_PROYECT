Imports CapaDatosCMMS

Public Class clsUbicacionGeograficaNegocios
    Dim UbiGeoMet As New clsUbicacionGeograficaMetodos

    Public Function UbicacionGeograficaListarPorPaisId(ByVal IdPais As String) As GNRL_UBICACIONGEOGRAFICA
        Return UbiGeoMet.UbicacionGeograficaListarPorPaisId(IdPais)
    End Function

    Public Function PaisListarCombo() As List(Of GNRL_UBICACIONGEOGRAFICA)
        Return UbiGeoMet.PaisListarCombo
    End Function

    Public Function DepartamentoListarCombo(ByVal strPais As String) As List(Of GNRL_UBICACIONGEOGRAFICA)
        Return UbiGeoMet.DepartamentoListarCombo(strPais)
    End Function

    Public Function ProvinciaListarCombo(ByVal strPais As String, ByVal strDpto As String) As List(Of GNRL_UBICACIONGEOGRAFICA)
        Return UbiGeoMet.ProvinciaListarCombo(strPais, strDpto)
    End Function

    Public Function DistritoListarCombo(ByVal strPais As String, ByVal strDpto As String, ByVal strProvincia As String) As List(Of GNRL_UBICACIONGEOGRAFICA)
        Return UbiGeoMet.DistritoListarCombo(strPais, strDpto, strProvincia)
    End Function

    Public Function UbicacionGeograficaListarPorId(ByVal IdPais As String, ByVal IdDepartamento As String, ByVal IdProvincia As String, ByVal IdDistrito As String) As GNRL_UBICACIONGEOGRAFICA
        'If UbiGeoMet.BancoExiste(IdBanco) = False Then
        '    'Si la Ubicacino existe lanzo una excepción.
        '    Throw New Exception("El banco con el id " & IdBanco & " no Existe!!!")
        'Else
        '    Return UbiGeoMet.BancoListarPorId(IdBanco)
        'End If
        Return UbiGeoMet.UbicacionGeograficaListarPorId(IdPais, IdDepartamento, IdProvincia, IdDistrito)
    End Function

    Public Function UbicacionGeograficaListarPorIdEquivalencia(ByVal IdEquivalenciaUbicacionGeografica As String) As GNRL_UBICACIONGEOGRAFICA
        Return UbiGeoMet.UbicacionGeograficaListarPorIdEquivalencia(IdEquivalenciaUbicacionGeografica)
    End Function

    'Public Function UbicacionGeograficaExiste(ByVal IdPais As String, ByVal IdDpto As String, ByVal IdProvincia As String, ByVal IdDistrito As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As GNRL_UBICACIONGEOGRAFICA
    '    If UbiGeoMet.UbicacionGeograficaExiste(IdPais, IdDpto, IdProvincia, IdDistrito, IdEmpresa, IdPuntoVenta) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("La Ubicación Geográfica con el id " & IdDistrito & " no Existe!!!")
    '    Else
    '        Return UbiGeoMet.ClienteListarPorId(IdCliente, IdEmpresa, IdPuntoVenta)
    '    End If
    'End Function
End Class
