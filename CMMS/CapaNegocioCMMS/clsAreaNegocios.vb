Imports CapaDatosCMMS
Public Class clsAreaNegocios
    Dim AreaMet As New clsAreaMetodos
    Public Function AreaListarCombo() As List(Of GNRL_AREA)
        Return AreaMet.AreaListarCombo
    End Function

    Public Function AreaDepartamentoListarCombo() As List(Of GNRL_AREA)
        Return AreaMet.AreaDepartamentoListarCombo
    End Function

    Public Function AreaListarPorId(ByVal IdArea As String) As GNRL_AREA
        If AreaMet.AreaExiste(IdArea) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El area con el id " & IdArea & " no Existe!!!")
        Else
            Return AreaMet.AreaListarPorId(IdArea)
        End If
    End Function

    Public Function AreaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_AREA)
        Return AreaMet.AreaListaGrid(Filtro, Buscar)
    End Function

    Public Function AreaInserta(ByVal Area As GNRL_AREA) As Int32
        If AreaMet.AreaExiste(Area.cIdArea) = True Then
            Throw New Exception("El area con el id " & Area.cIdArea & " ya existe!")
        Else
            Return AreaMet.AreaInserta(Area)
        End If
    End Function

    Public Function AreaEdita(ByVal Area As GNRL_AREA) As Int32
        If AreaMet.AreaExiste(Area.cIdArea) = False Then
            Throw New Exception("El area con el id " & Area.cIdArea & " no existe!")
        Else
            Return AreaMet.AreaEdita(Area)
        End If
    End Function

    Public Function AreaElimina(ByVal Area As GNRL_AREA) As Int32
        If AreaMet.AreaExiste(Area.cIdArea) = False Then
            Throw New Exception("El area con el id " & Area.cIdArea & " no existe!")
        Else
            Return AreaMet.AreaElimina(Area)
        End If
    End Function

End Class