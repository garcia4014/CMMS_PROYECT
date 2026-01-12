Imports CapaDatosCMMS

Public Class clsGaleriaEquipoNegocios
    Dim GaleriaEquipoMet As New clsGaleriaEquipoMetodos

    Public Function GaleriaEquipoGetData(strQuery As String) As DataTable
        Return GaleriaEquipoMet.GaleriaEquipoGetData(strQuery)
    End Function

    Public Function GaleriaEquipoListarCombo() As List(Of LOGI_GALERIAEQUIPO)
        Return GaleriaEquipoMet.GaleriaEquipoListarCombo()
    End Function

    Public Function GaleriaEquipoInserta(ByVal GaleriaEquipo As LOGI_GALERIAEQUIPO) As Int32
        If GaleriaEquipoMet.GaleriaEquipoExiste(GaleriaEquipo.cIdEquipo, GaleriaEquipo.nIdNumeroItemGaleriaEquipo) = True Then
            Throw New Exception("La imagen con el id " & GaleriaEquipo.cIdEquipo & " ya existe!")
        Else
            Return GaleriaEquipoMet.GaleriaEquipoInserta(GaleriaEquipo)
        End If
    End Function

    Public Function GaleriaEquipoEdita(ByVal GaleriaEquipo As LOGI_GALERIAEQUIPO) As Int32
        If GaleriaEquipoMet.GaleriaEquipoExiste(GaleriaEquipo.cIdEquipo, GaleriaEquipo.nIdNumeroItemGaleriaEquipo) = False Then
            Throw New Exception("La imagen con el id " & GaleriaEquipo.cIdEquipo & " no existe!")
        Else
            Return GaleriaEquipoMet.GaleriaEquipoEdita(GaleriaEquipo)
        End If
    End Function

    Public Function GaleriaEquipoElimina(ByVal GaleriaEquipo As LOGI_GALERIAEQUIPO)
        If GaleriaEquipoMet.GaleriaEquipoExiste(GaleriaEquipo.cIdEquipo, GaleriaEquipo.nIdNumeroItemGaleriaEquipo) = False Then
            Throw New Exception("La imagen con el id " & GaleriaEquipo.cIdEquipo & " no existe!")
        Else
            Return GaleriaEquipoMet.GaleriaEquipoElimina(GaleriaEquipo)
        End If
    End Function

    Public Function GaleriaEquipoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_GALERIAEQUIPO)
        Return GaleriaEquipoMet.GaleriaEquipoListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function GaleriaEquipoListarPorId(ByVal IdEquipo As String, ByVal IdNroItem As String) As LOGI_GALERIAEQUIPO
        If GaleriaEquipoMet.GaleriaEquipoExiste(IdEquipo, IdNroItem) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La imagen con el id " & IdEquipo & " no Existe!!!")
        Else
            Return GaleriaEquipoMet.GaleriaEquipoListarPorId(IdEquipo, IdNroItem)
        End If
    End Function
End Class
