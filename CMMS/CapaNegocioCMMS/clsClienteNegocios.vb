Imports CapaDatosCMMS
Public Class clsClienteNegocios
    Dim ClienteMet As New clsClienteMetodos

    Public Function ClienteGetData(strQuery As String) As DataTable
        Return ClienteMet.ClienteGetData(strQuery)
    End Function

    Public Function ClienteListarCombo() As List(Of GNRL_CLIENTE)
        Return ClienteMet.ClienteListarCombo
    End Function

    Public Function ClienteInserta(ByVal Cliente As GNRL_CLIENTE) As Int32
        If ClienteMet.ClienteExiste(Cliente.cIdCliente, Cliente.cIdEmpresa, Cliente.cIdPuntoVenta) = True Then
            Throw New Exception("El cliente con el id " & Cliente.cIdCliente & " ya existe!")
        Else
            Return ClienteMet.ClienteInserta(Cliente)
        End If
    End Function

    Public Function ClienteEdita(ByVal Cliente As GNRL_CLIENTE) As Int32
        If ClienteMet.ClienteExiste(Cliente.cIdCliente, Cliente.cIdEmpresa, Cliente.cIdPuntoVenta) = False Then
            Throw New Exception("El cliente con el id " & Cliente.cIdCliente & " no existe!")
        Else
            Return ClienteMet.ClienteEdita(Cliente)
        End If
    End Function

    Public Function ClienteElimina(ByVal Cliente As GNRL_CLIENTE)
        If ClienteMet.ClienteExiste(Cliente.cIdCliente, Cliente.cIdEmpresa, Cliente.cIdPuntoVenta) = False Then
            Throw New Exception("El cliente con el id " & Cliente.cIdCliente & " no existe!")
        Else
            Return ClienteMet.ClienteElimina(Cliente)
        End If
    End Function

    Public Function ClienteListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, Optional ByVal booOrden As Boolean = True, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_CLIENTE)
        Return ClienteMet.ClienteListaGrid(Filtro, Buscar, IdEmpresa, IdPuntoVenta, booOrden, Estado)
    End Function

    Public Function ClienteListarPorId(ByVal IdCliente As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As GNRL_CLIENTE
        If ClienteMet.ClienteExiste(IdCliente, IdEmpresa, IdPuntoVenta) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El cliente con el id " & IdCliente & " no Existe!!!")
        Else
            Return ClienteMet.ClienteListarPorId(IdCliente, IdEmpresa, IdPuntoVenta)
        End If
    End Function

    Public Function ClienteListarPorIdSAP(ByVal IdCliente As String, ByVal IdEmpresa As String) As GNRL_CLIENTE
        'If ClienteMet.ClienteExiste(IdCliente, IdEmpresa, "*") = False Then
        '    'Si el producto no existe lanzo una excepción.
        '    Throw New Exception("El cliente con el id " & IdCliente & " no Existe!!!")
        'Else
        Return ClienteMet.ClienteListarPorIdSAP(IdCliente)
        'End If
    End Function
End Class