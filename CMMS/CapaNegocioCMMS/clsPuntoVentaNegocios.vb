Imports CapaDatosCMMS
Public Class clsPuntoVentaNegocios
    Dim PuntoVentaMet As New clsPuntoVentaMetodos

    Public Function PuntoVentaGetData(strQuery As String) As DataTable
        Return PuntoVentaMet.PuntoVentaGetData(strQuery)
    End Function

    Public Function PuntoVentaListarCombo(ByVal IdEmpresa As String, ByVal IdLocal As String) As List(Of GNRL_PUNTOVENTA)
        Return PuntoVentaMet.PuntoVentaListarCombo(IdEmpresa, IdLocal)
    End Function

    Public Function PuntoVentaInserta(ByVal PuntoVenta As GNRL_PUNTOVENTA) As Int32
        If PuntoVentaMet.PuntoVentaExiste(PuntoVenta.cIdPuntoVenta, PuntoVenta.cIdEmpresa) = True Then
            Throw New Exception("El punto de venta con el id " & PuntoVenta.cIdPuntoVenta & " ya existe!")
        Else
            Return PuntoVentaMet.PuntoVentaInserta(PuntoVenta)
        End If
    End Function

    Public Function PuntoVentaEdita(ByVal PuntoVenta As GNRL_PUNTOVENTA) As Int32
        If PuntoVentaMet.PuntoVentaExiste(PuntoVenta.cIdPuntoVenta, PuntoVenta.cIdEmpresa) = False Then
            Throw New Exception("El punto de venta con el id " & PuntoVenta.cIdPuntoVenta & " no existe!")
        Else
            Return PuntoVentaMet.PuntoVentaEdita(PuntoVenta)
        End If
    End Function

    Public Function PuntoVentaElimina(ByVal PuntoVenta As GNRL_PUNTOVENTA)
        If PuntoVentaMet.PuntoVentaExiste(PuntoVenta.cIdPuntoVenta, PuntoVenta.cIdEmpresa) = False Then
            Throw New Exception("El el punto de venta con el id " & PuntoVenta.cIdPuntoVenta & " no existe!")
        Else
            Return PuntoVentaMet.PuntoVentaElimina(PuntoVenta)
        End If
    End Function

    Public Function PuntoVentaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_PUNTOVENTA)
        Return PuntoVentaMet.PuntoVentaListaGrid(Filtro, Buscar, IdEmpresa, Estado)
    End Function

    Public Function PuntoVentaListarPorId(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String) As GNRL_PUNTOVENTA
        If PuntoVentaMet.PuntoVentaExiste(IdPuntoVenta, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El punto de venta con el id " & IdPuntoVenta & " no existe!!!")
        Else
            Return PuntoVentaMet.PuntoVentaListarPorId(IdPuntoVenta, IdEmpresa)
        End If
    End Function
End Class