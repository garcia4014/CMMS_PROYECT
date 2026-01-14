Imports CapaDatosCMMS
Public Class clsTipoActivoNegocios
    Dim TipoActivoMet As New clsTipoActivoMetodos

    Public Function TipoActivoGetData(strQuery As String) As DataTable
        Return TipoActivoMet.TipoActivoGetData(strQuery)
    End Function

    Public Function TipoActivoListarCombo(Optional ByVal Estado As String = "*") As List(Of LOGI_TIPOACTIVO)
        Return TipoActivoMet.TipoActivoListarCombo(Estado)
    End Function

    Public Function TipoActivoInserta(ByVal TipoActivo As LOGI_TIPOACTIVO) As Int32
        If TipoActivoMet.TipoActivoExiste(TipoActivo.cIdTipoActivo) = True Then
            Throw New Exception("El TipoActivo con el id " & TipoActivo.cIdTipoActivo & " ya existe!")
        Else
            Return TipoActivoMet.TipoActivoInserta(TipoActivo)
        End If
    End Function

    Public Function TipoActivoEdita(ByVal TipoActivo As LOGI_TIPOACTIVO) As Int32
        If TipoActivoMet.TipoActivoExiste(TipoActivo.cIdTipoActivo) = False Then
            Throw New Exception("El TipoActivo con el id " & TipoActivo.cIdTipoActivo & " no existe!")
        Else
            Return TipoActivoMet.TipoActivoEdita(TipoActivo)
        End If
    End Function

    Public Function TipoActivoElimina(ByVal TipoActivo As LOGI_TIPOACTIVO) As Int32
        If TipoActivoMet.TipoActivoExiste(TipoActivo.cIdTipoActivo) = False Then
            Throw New Exception("El TipoActivo con el id " & TipoActivo.cIdTipoActivo & " no existe!")
        Else
            Return TipoActivoMet.TipoActivoElimina(TipoActivo)
        End If
    End Function

    Public Function TipoActivoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "1") As List(Of VI_LOGI_TIPOACTIVO)
        Return TipoActivoMet.TipoActivoListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function TipoActivoListarPorId(ByVal IdTipoActivo As String) As LOGI_TIPOACTIVO
        If TipoActivoMet.TipoActivoExiste(IdTipoActivo) = False Then
            'Si el TipoActivo no existe lanzo una excepción.
            Throw New Exception("El TipoActivo con el id " & IdTipoActivo & " no Existe!!!")
        Else
            Return TipoActivoMet.TipoActivoListarPorId(IdTipoActivo)
        End If
    End Function
End Class