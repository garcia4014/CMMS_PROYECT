Imports CapaDatosCMMS
Public Class clsTipoEquipoNegocios
    Dim TipoEquipoMet As New clsTipoEquipoMetodos

    Public Function TipoEquipoGetData(strQuery As String) As DataTable
        Return TipoEquipoMet.TipoEquipoGetData(strQuery)
    End Function

    Public Function TipoEquipoListarCombo(Optional ByVal Estado As String = "*") As List(Of LOGI_TIPOEQUIPO)
        Return TipoEquipoMet.TipoEquipoListarCombo(Estado)
    End Function

    Public Function TipoEquipoInserta(ByVal TipoEquipo As LOGI_TIPOEQUIPO) As Int32
        If TipoEquipoMet.TipoEquipoExiste(TipoEquipo.cIdTipoEquipo) = True Then
            Throw New Exception("El Tipo Equipo con el id " & TipoEquipo.cIdTipoEquipo & " ya existe!")
        Else
            Return TipoEquipoMet.TipoEquipoInserta(TipoEquipo)
        End If
    End Function

    Public Function TipoEquipoEdita(ByVal TipoEquipo As LOGI_TIPOEQUIPO) As Int32
        If TipoEquipoMet.TipoEquipoExiste(TipoEquipo.cIdTipoEquipo) = False Then
            Throw New Exception("El Tipo Equipo con el id " & TipoEquipo.cIdTipoEquipo & " no existe!")
        Else
            Return TipoEquipoMet.TipoEquipoEdita(TipoEquipo)
        End If
    End Function

    Public Function TipoEquipoElimina(ByVal TipoEquipo As LOGI_TIPOEQUIPO) As Int32
        If TipoEquipoMet.TipoEquipoExiste(TipoEquipo.cIdTipoEquipo) = False Then
            Throw New Exception("El Tipo Equipo con el id " & TipoEquipo.cIdTipoEquipo & " no existe!")
        Else
            Return TipoEquipoMet.TipoEquipoElimina(TipoEquipo)
        End If
    End Function

    Public Function TipoEquipoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "1") As List(Of VI_LOGI_TIPOEQUIPO)
        Return TipoEquipoMet.TipoEquipoListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function TipoEquipoListarPorId(ByVal IdTipoEquipo As String) As LOGI_TIPOEQUIPO
        If TipoEquipoMet.TipoEquipoExiste(IdTipoEquipo) = False Then
            'Si el TipoEquipo no existe lanzo una excepción.
            Throw New Exception("El Tipo Equipo con el id " & IdTipoEquipo & " no Existe!!!")
        Else
            Return TipoEquipoMet.TipoEquipoListarPorId(IdTipoEquipo)
        End If
    End Function
End Class