Imports CapaDatosCMMS

Public Class clsLocalNegocios
    Dim LocalMet As New clsLocalMetodos

    Public Function LocalGetData(strQuery As String) As DataTable
        Return LocalMet.LocalGetData(strQuery)
    End Function

    Public Function LocalListarCombo(ByVal IdEmpresa As String) As List(Of GNRL_LOCAL)
        Return LocalMet.LocalListarCombo(IdEmpresa)
    End Function

    Public Function LocalEmpresaListarCombo(ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of GNRL_LOCAL)
        Return LocalMet.LocalEmpresaListarCombo(IdEmpresa, Estado)
    End Function

    Public Function LocalInserta(ByVal Local As GNRL_LOCAL) As Int32
        If LocalMet.LocalExiste(Local.cIdLocal, Local.cIdEmpresa) = True Then
            Throw New Exception("El Local con el id " & Local.cIdLocal & " ya existe!")
        Else
            Return LocalMet.LocalInserta(Local)
        End If
    End Function

    Public Function LocalEdita(ByVal Local As GNRL_LOCAL) As Int32
        If LocalMet.LocalExiste(Local.cIdLocal, Local.cIdEmpresa) = False Then
            Throw New Exception("El Local con el id " & Local.cIdLocal & " no existe!")
        Else
            Return LocalMet.LocalEdita(Local)
        End If
    End Function

    Public Function LocalElimina(ByVal Local As GNRL_LOCAL)
        If LocalMet.LocalExiste(Local.cIdLocal, Local.cIdEmpresa) = False Then
            Throw New Exception("El Local con el id " & Local.cIdLocal & " no existe!")
        Else
            Return LocalMet.LocalElimina(Local)
        End If
    End Function

    Public Function LocalListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_LOCAL)
        Return LocalMet.LocalListaGrid(Filtro, Buscar, IdEmpresa, Estado)
    End Function

    Public Function LocalListarPorId(ByVal IdLocal As String, ByVal IdEmpresa As String) As GNRL_LOCAL
        If LocalMet.LocalExiste(IdLocal, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Local con el id " & IdLocal & " no Existe!!!")
        Else
            Return LocalMet.LocalListarPorId(IdLocal, IdEmpresa)
        End If
    End Function
End Class
