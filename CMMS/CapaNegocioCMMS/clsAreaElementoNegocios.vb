Imports CapaDatosCMMS

Public Class clsAreaElementoNegocios
    Dim AreaElementoMet As New clsAreaElementoMetodos

    Public Function AreaElementoInserta(ByVal AreaElemento As GNRL_AREAELEMENTO) As Int32
        If AreaElementoMet.AreaElementoExiste(AreaElemento) = True Then
            Throw New Exception("El Elemento con el id " & AreaElemento.cIdElemento & " ya existe!")
        Else
            Return AreaElementoMet.AreaElementoInserta(AreaElemento)
        End If
    End Function

    Public Function AreaElementoEdita(ByVal AreaElemento As GNRL_AREAELEMENTO) As Int32
        If AreaElementoMet.AreaElementoExiste(AreaElemento) = False Then
            Throw New Exception("El AreaElemento con el id " & AreaElemento.cIdElemento & " no existe!")
        Else
            Return AreaElementoMet.AreaElementoEdita(AreaElemento)
        End If
    End Function

    Public Function AreaElementoElimina(ByVal AreaElemento As GNRL_AREAELEMENTO)
        If AreaElementoMet.AreaElementoExiste(AreaElemento) = False Then
            Throw New Exception("El AreaElemento con el id " & AreaElemento.cIdElemento & " no existe!")
        Else
            Return AreaElementoMet.AreaElementoElimina(AreaElemento)
        End If
    End Function

    'JMUG: 08/02/2023
    'Public Function AreaElementoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdSistema As String, ByVal IdModulo As String, ByVal IdArea As String, ByVal IdPerfil As String, ByVal IdUsuario As String) As List(Of VI_GNRL_ELEMENTO)
    '    Return AreaElementoMet.AreaElementoListaGrid(Filtro, Buscar, IdSistema, IdModulo, IdArea, IdPerfil, IdUsuario)
    'End Function
    Public Function AreaElementoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdSistema As String, ByVal IdModulo As String, ByVal IdPerfil As String, ByVal IdUsuario As String) As List(Of VI_GNRL_ELEMENTO)
        Return AreaElementoMet.AreaElementoListaGrid(Filtro, Buscar, IdSistema, IdModulo, IdPerfil, IdUsuario)
    End Function


    'Public Function ModuloListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdArea As String) As List(Of VI_GNRL_MODULO)
    Public Function ModuloListaBusqueda(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_MODULO)
        Return AreaElementoMet.ModuloListaGrid(Filtro, Buscar)
    End Function

    Public Function AreaElementoListaBusquedaFiltrado(ByVal Filtro As String, ByVal Buscar As String, ByVal IdSistema As String,
                                                      ByVal IdModulo As String, ByVal IdArea As String, ByVal IdPerfil As String) As List(Of VI_GNRL_ELEMENTO)
        Return AreaElementoMet.AreaElementoListaGridFiltrado(Filtro, Buscar, IdSistema, IdModulo, IdArea, IdPerfil)
    End Function

    Public Function AreaElementoListarPorId(ByVal AreaElemento As GNRL_AREAELEMENTO) As GNRL_AREAELEMENTO
        If AreaElementoMet.AreaElementoExiste(AreaElemento) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El AreaElemento con el id " & AreaElemento.cIdElemento & " no Existe!!!")
        Else
            Return AreaElementoMet.AreaElementoListarPorId(AreaElemento)
        End If
    End Function
End Class
