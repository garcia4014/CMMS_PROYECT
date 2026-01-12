Imports CapaDatosCMMS

Public Class clsTipoMonedaNegocios
    Dim TipoMonedaMet As New clsTipoMonedaMetodos

    Public Function TipoMonedaGetData(strQuery As String) As DataTable
        Return TipoMonedaMet.TipoMonedaGetData(strQuery)
    End Function

    Public Function TipoMonedaListarCombo() As List(Of GNRL_TIPOMONEDA)
        Return TipoMonedaMet.TipoMonedaListarCombo
    End Function

    Public Function TipoMonedaInserta(ByVal TipoMoneda As GNRL_TIPOMONEDA) As Int32
        If TipoMonedaMet.TipoMonedaExiste(TipoMoneda.cIdTipoMoneda) = True Then
            Throw New Exception("El tipo moneda con el id " & TipoMoneda.cIdTipoMoneda & " ya existe!")
        Else
            Return TipoMonedaMet.TipoMonedaInserta(TipoMoneda)
        End If
    End Function

    Public Function TipoMonedaEdita(ByVal TipoMoneda As GNRL_TIPOMONEDA) As Int32
        If TipoMonedaMet.TipoMonedaExiste(TipoMoneda.cIdTipoMoneda) = False Then
            Throw New Exception("El tipo moneda con el id " & TipoMoneda.cIdTipoMoneda & " no existe!")
        Else
            Return TipoMonedaMet.TipoMonedaEdita(TipoMoneda)
        End If
    End Function

    Public Function TipoMonedaElimina(ByVal TipoMoneda As GNRL_TIPOMONEDA)
        If TipoMonedaMet.TipoMonedaExiste(TipoMoneda.cIdTipoMoneda) = False Then
            Throw New Exception("El tipo moneda con el id " & TipoMoneda.cIdTipoMoneda & " no existe!")
        Else
            Return TipoMonedaMet.TipoMonedaElimina(TipoMoneda)
        End If
    End Function

    Public Function TipoMonedaEditaBase(ByVal TipoMoneda As GNRL_TIPOMONEDA) As Int32
        If TipoMonedaMet.TipoMonedaExiste(TipoMoneda.cIdTipoMoneda) = False Then
            Throw New Exception("El tipo moneda con el id " & TipoMoneda.cIdTipoMoneda & " no existe!")
        Else
            Return TipoMonedaMet.TipoMonedaEditaBase(TipoMoneda)
        End If
    End Function

    Public Function TipoMonedaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_TIPOMONEDA)
        Return TipoMonedaMet.TipoMonedaListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function TipoMonedaBase() As String
        'If TipoMonedaMet.TipoMonedaExiste(TipoMoneda.cIdTipoMoneda) = False Then
        '    Throw New Exception("El tipo moneda con el id " & TipoMoneda.cIdTipoMoneda & " no existe!")
        'Else
        Return TipoMonedaMet.TipoMonedaBase()
        'End If
    End Function
    Public Function TipoMonedaExtranjera() As String
        Return TipoMonedaMet.TipoMonedaExtranjera
    End Function

    Public Function TipoMonedaListarPorId(ByVal IdTipoMoneda As String) As GNRL_TIPOMONEDA
        If TipoMonedaMet.TipoMonedaExiste(IdTipoMoneda) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El tipo moneda con el id " & IdTipoMoneda & " no Existe!!!")
        Else
            Return TipoMonedaMet.TipoMonedaListarPorId(IdTipoMoneda)
        End If
    End Function
End Class