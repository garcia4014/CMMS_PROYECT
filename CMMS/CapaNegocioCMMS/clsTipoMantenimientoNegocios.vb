Imports CapaDatosCMMS

Public Class clsTipoMantenimientoNegocios
    Dim TipoMantenimientoMet As New clsTipoMantenimientoMetodos

    Public Function TipoMantenimientoGetData(strQuery As String) As DataTable
        Return TipoMantenimientoMet.TipoMantenimientoGetData(strQuery)
    End Function

    Public Function TipoMantenimientoListarCombo(ByVal Estado As String) As List(Of LOGI_TIPOMANTENIMIENTO)
        Return TipoMantenimientoMet.TipoMantenimientoListarCombo(Estado)
    End Function

    Public Function TipoMantenimientoInserta(ByVal TipoMantenimiento As LOGI_TIPOMANTENIMIENTO) As Int32
        If TipoMantenimientoMet.TipoMantenimientoExiste(TipoMantenimiento.cIdTipoMantenimiento) = True Then
            Throw New Exception("El Tipo de Mantenimiento con el id " & TipoMantenimiento.cIdTipoMantenimiento & " ya existe!")
        Else
            Return TipoMantenimientoMet.TipoMantenimientoInserta(TipoMantenimiento)
        End If
    End Function

    Public Function TipoMantenimientoEdita(ByVal TipoMantenimiento As LOGI_TIPOMANTENIMIENTO) As Int32
        If TipoMantenimientoMet.TipoMantenimientoExiste(TipoMantenimiento.cIdTipoMantenimiento) = False Then
            Throw New Exception("El Tipo de Mantenimiento con el id " & TipoMantenimiento.cIdTipoMantenimiento & " no existe!")
        Else
            Return TipoMantenimientoMet.TipoMantenimientoEdita(TipoMantenimiento)
        End If
    End Function

    Public Function TipoMantenimientoElimina(ByVal TipoMantenimiento As LOGI_TIPOMANTENIMIENTO)
        If TipoMantenimientoMet.TipoMantenimientoExiste(TipoMantenimiento.cIdTipoMantenimiento) = False Then
            Throw New Exception("El Tipo de Mantenimiento con el id " & TipoMantenimiento.cIdTipoMantenimiento & " no existe!")
        Else
            Return TipoMantenimientoMet.TipoMantenimientoElimina(TipoMantenimiento)
        End If
    End Function

    Public Function TipoMantenimientoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_TIPOMANTENIMIENTO)
        Return TipoMantenimientoMet.TipoMantenimientoListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function TipoMantenimientoListarPorId(ByVal IdTipoMantenimiento As String) As LOGI_TIPOMANTENIMIENTO
        If TipoMantenimientoMet.TipoMantenimientoExiste(IdTipoMantenimiento) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Tipo de Mantenimiento con el id " & IdTipoMantenimiento & " no Existe!!!")
        Else
            Return TipoMantenimientoMet.TipoMantenimientoListarPorId(IdTipoMantenimiento)
        End If
    End Function
End Class
