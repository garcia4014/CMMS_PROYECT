Imports CapaDatosCMMS

Public Class clsEmpresaNegocios
    Dim EmpresaMet As New clsEmpresaMetodos

    Public Function EmpresaGetData(strQuery As String) As DataTable
        Return EmpresaMet.EmpresaGetData(strQuery)
    End Function

    Public Function EmpresaPaisListarCombo(ByVal IdPais As String, Optional ByVal Estado As String = "*") As List(Of GNRL_EMPRESA)
        Return EmpresaMet.EmpresaPaisListarCombo(IdPais, Estado)
    End Function

    Public Function EmpresaInserta(ByVal Empresa As GNRL_EMPRESA) As Int32
        If EmpresaMet.EmpresaExiste(Empresa.cIdEmpresa) = True Then
            Throw New Exception("El empresa con el id " & Empresa.cIdEmpresa & " ya existe!")
        Else
            Return EmpresaMet.EmpresaInserta(Empresa)
        End If
    End Function

    Public Function EmpresaEdita(ByVal Empresa As GNRL_EMPRESA) As Int32
        If EmpresaMet.EmpresaExiste(Empresa.cIdEmpresa) = False Then
            Throw New Exception("El empresa con el id " & Empresa.cIdEmpresa & " no existe!")
        Else
            Return EmpresaMet.EmpresaEdita(Empresa)
        End If
    End Function

    Public Function EmpresaElimina(ByVal Empresa As GNRL_EMPRESA)
        If EmpresaMet.EmpresaExiste(Empresa.cIdEmpresa) = False Then
            Throw New Exception("El empresa con el id " & Empresa.cIdEmpresa & " no existe!")
        Else
            Return EmpresaMet.EmpresaElimina(Empresa)
        End If
    End Function

    Public Function EmpresaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_GNRL_EMPRESA)
        Return EmpresaMet.EmpresaListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function EmpresaListarPorId(ByVal IdEmpresa As String) As GNRL_EMPRESA
        If EmpresaMet.EmpresaExiste(IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El empresa con el id " & IdEmpresa & " no Existe!!!")
        Else
            Return EmpresaMet.EmpresaListarPorId(IdEmpresa)
        End If
    End Function

    Public Function EmpresaListarCombo() As List(Of GNRL_EMPRESA)
        Return EmpresaMet.EmpresaListarCombo
    End Function
End Class