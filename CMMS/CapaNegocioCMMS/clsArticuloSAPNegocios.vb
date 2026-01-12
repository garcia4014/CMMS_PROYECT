Imports CapaDatosCMMS

Public Class clsArticuloSAPNegocios
    Dim ArticuloSAPMet As New clsArticuloSAPMetodos
    Public Function ArticuloSAPListarCombo(ByVal bEstado As Boolean) As List(Of LOGI_ARTICULOSAP)
        Return ArticuloSAPMet.ArticuloSAPListarCombo(bEstado)
    End Function

    'Public Function ArticuloSAPDepartamentoListarCombo() As List(Of LOGI_ARTICULOSAP)
    '    Return ArticuloSAPMet.ArticuloSAPDepartamentoListarCombo
    'End Function

    'Public Function ArticuloSAPListarPorId(ByVal IdArticuloSAP As String) As LOGI_ARTICULOSAP
    '    If ArticuloSAPMet.ArticuloSAPExiste(IdArticuloSAP) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("El ArticuloSAP con el id " & IdArticuloSAP & " no Existe!!!")
    '    Else
    '        Return ArticuloSAPMet.ArticuloSAPListarPorId(IdArticuloSAP)
    '    End If
    'End Function

    'Public Function ArticuloSAPListaBusqueda(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_LOGI_ARTICULOSAP)
    '    Return ArticuloSAPMet.ArticuloSAPListaGrid(Filtro, Buscar)
    'End Function

    'Public Function ArticuloSAPInserta(ByVal ArticuloSAP As LOGI_ARTICULOSAP) As Int32
    '    If ArticuloSAPMet.ArticuloSAPExiste(ArticuloSAP.cIdArticuloSAP) = True Then
    '        Throw New Exception("El ArticuloSAP con el id " & ArticuloSAP.cIdArticuloSAP & " ya existe!")
    '    Else
    '        Return ArticuloSAPMet.ArticuloSAPInserta(ArticuloSAP)
    '    End If
    'End Function

    'Public Function ArticuloSAPEdita(ByVal ArticuloSAP As LOGI_ARTICULOSAP) As Int32
    '    If ArticuloSAPMet.ArticuloSAPExiste(ArticuloSAP.cIdArticuloSAP) = False Then
    '        Throw New Exception("El ArticuloSAP con el id " & ArticuloSAP.cIdArticuloSAP & " no existe!")
    '    Else
    '        Return ArticuloSAPMet.ArticuloSAPEdita(ArticuloSAP)
    '    End If
    'End Function

    'Public Function ArticuloSAPElimina(ByVal ArticuloSAP As LOGI_ARTICULOSAP) As Int32
    '    If ArticuloSAPMet.ArticuloSAPExiste(ArticuloSAP.cIdArticuloSAP) = False Then
    '        Throw New Exception("El ArticuloSAP con el id " & ArticuloSAP.cIdArticuloSAP & " no existe!")
    '    Else
    '        Return ArticuloSAPMet.ArticuloSAPElimina(ArticuloSAP)
    '    End If
    'End Function
End Class