Imports CapaDatosCMMS

Public Class clsSistemaFuncionalNegocios
    Dim SistemaFuncionalMet As New clsSistemaFuncionalMetodos

    Public Function SistemaFuncionalGetData(strQuery As String) As DataTable
        Return SistemaFuncionalMet.SistemaFuncionalGetData(strQuery)
    End Function

    'Public Function SistemaFuncionalListarCombo(ByVal IdTipoActivo As String, ByVal IdCatalogo As String) As List(Of LOGI_SISTEMAFUNCIONAL)
    '    Return SistemaFuncionalMet.SistemaFuncionalListarCombo(IdTipoActivo, IdCatalogo)
    'End Function
    Public Function SistemaFuncionalListarCombo() As List(Of LOGI_SISTEMAFUNCIONAL)
        Return SistemaFuncionalMet.SistemaFuncionalListarCombo()
    End Function

    Public Function SistemaFuncionalInserta(ByVal SistemaFuncional As LOGI_SISTEMAFUNCIONAL) As Int32
        'If SistemaFuncionalMet.SistemaFuncionalExiste(SistemaFuncional.cIdSistemaFuncional, SistemaFuncional.cIdCatalogo) = True Then
        If SistemaFuncionalMet.SistemaFuncionalExiste(SistemaFuncional.cIdSistemaFuncional) = True Then
            Throw New Exception("El SistemaFuncional con el id " & SistemaFuncional.cIdSistemaFuncional & " ya existe!")
        Else
            Return SistemaFuncionalMet.SistemaFuncionalInserta(SistemaFuncional)
        End If
    End Function

    Public Function SistemaFuncionalEdita(ByVal SistemaFuncional As LOGI_SISTEMAFUNCIONAL) As Int32
        'If SistemaFuncionalMet.SistemaFuncionalExiste(SistemaFuncional.cIdSistemaFuncional, SistemaFuncional.cIdCatalogo) = False Then
        If SistemaFuncionalMet.SistemaFuncionalExiste(SistemaFuncional.cIdSistemaFuncional) = False Then
            Throw New Exception("El SistemaFuncional con el id " & SistemaFuncional.cIdSistemaFuncional & " no existe!")
        Else
            Return SistemaFuncionalMet.SistemaFuncionalEdita(SistemaFuncional)
        End If
    End Function

    Public Function SistemaFuncionalElimina(ByVal SistemaFuncional As LOGI_SISTEMAFUNCIONAL) As Int32
        'If SistemaFuncionalMet.SistemaFuncionalExiste(SistemaFuncional.cIdSistemaFuncional, SistemaFuncional.cIdCatalogo) = False Then
        If SistemaFuncionalMet.SistemaFuncionalExiste(SistemaFuncional.cIdSistemaFuncional) = False Then
            Throw New Exception("El SistemaFuncional con el id " & SistemaFuncional.cIdSistemaFuncional & " no existe!")
        Else
            Return SistemaFuncionalMet.SistemaFuncionalElimina(SistemaFuncional)
        End If
    End Function

    Public Function SistemaFuncionalListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "1") As List(Of VI_LOGI_SISTEMAFUNCIONAL)
        Return SistemaFuncionalMet.SistemaFuncionalListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function SistemaFuncionalListarPorId(ByVal IdSistemaFuncional As String) As LOGI_SISTEMAFUNCIONAL
        'If SistemaFuncionalMet.SistemaFuncionalExiste(IdSistemaFuncional, IdCatalogo) = False Then
        If SistemaFuncionalMet.SistemaFuncionalExiste(IdSistemaFuncional) = False Then
            'Si el SistemaFuncional no existe lanzo una excepción.
            Throw New Exception("El SistemaFuncional con el id " & IdSistemaFuncional & " no Existe!!!")
        Else
            'Return SistemaFuncionalMet.SistemaFuncionalListarPorId(IdSistemaFuncional, IdCatalogo)
            Return SistemaFuncionalMet.SistemaFuncionalListarPorId(IdSistemaFuncional)
        End If
    End Function
End Class