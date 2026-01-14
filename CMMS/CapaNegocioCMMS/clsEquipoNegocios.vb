Imports CapaDatosCMMS

Public Class clsEquipoNegocios
    Dim EquipoMet As New clsEquipoMetodos

    Public Function EquipoGetData(strQuery As String) As DataTable
        Return EquipoMet.EquipoGetData(strQuery)
    End Function

    'Public Function EquipoListarCombo(ByVal IdJerarquia As String) As List(Of LOGI_Equipo)
    Public Function EquipoListarCombo(ByVal IdTipoActivo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String) As List(Of LOGI_EQUIPO)
        'Return EquipoMet.EquipoListarCombo(IdJerarquia)
        Return EquipoMet.EquipoListarCombo(IdTipoActivo, IdCatalogo, IdJerarquia)
    End Function

    Public Function EquipoInsertaDetalle(ByVal DetalleEquipo As List(Of LOGI_EQUIPO), Optional ByVal strNroEnlaceEquipo As String = "", Optional ByVal strNroEnlaceCatalogo As String = "") As Int32
        'If EquipoMet.EquipoExiste(Equipo.cIdEquipo) = True Then
        'Throw New Exception("El Equipo con el id " & Equipo.cIdEquipo & " ya existe!")
        'Else
        Return EquipoMet.EquipoInsertaDetalle(DetalleEquipo, strNroEnlaceEquipo, strNroEnlaceCatalogo)
        'End If
    End Function

    Public Function EquipoInserta(ByVal Equipo As LOGI_EQUIPO) As Int32
        'If EquipoMet.EquipoExiste(Equipo.cIdEquipo, Equipo.cIdCatalogo, Equipo.cIdTipoActivo) = True Then
        If EquipoMet.EquipoExiste(Equipo.cIdEquipo) = True Then
            Throw New Exception("El Equipo con el id " & Equipo.cIdEquipo & " ya existe!")
        Else
            Return EquipoMet.EquipoInserta(Equipo)
        End If
    End Function

    Public Function EquipoEdita(ByVal Equipo As LOGI_EQUIPO) As Int32
        'If EquipoMet.EquipoExiste(Equipo.cIdEquipo, Equipo.cIdCatalogo, Equipo.cIdTipoActivo) = False Then
        If EquipoMet.EquipoExiste(Equipo.cIdEquipo) = False Then
            Throw New Exception("El Equipo con el id " & Equipo.cIdEquipo & " no existe!")
        Else
            Return EquipoMet.EquipoEdita(Equipo)
        End If
    End Function

    Public Function EquipoElimina(ByVal Equipo As LOGI_EQUIPO) As Int32
        'If EquipoMet.EquipoExiste(Equipo.cIdEquipo, Equipo.cIdEmpresa, Equipo.cIdPuntoVenta) = False Then
        If EquipoMet.EquipoExiste(Equipo.cIdEquipo) = False Then
            Throw New Exception("El Equipo con el id " & Equipo.cIdEquipo & " no existe!")
        Else
            Return EquipoMet.EquipoElimina(Equipo)
        End If
    End Function

    Public Function EquipoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Jerarquia As String) As List(Of VI_LOGI_Equipo)
        'Public Function EquipoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEquipo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdJerarquia As String) As List(Of VI_LOGI_Equipo)
        'Return EquipoMet.EquipoListaGrid(Filtro, Buscar, IdEquipo, IdEmpresa, IdPuntoVenta, IdJerarquia)
        Return EquipoMet.EquipoListaGrid(Filtro, Buscar, Jerarquia)
    End Function

    Public Function EquipoListaBusquedaV2(ByVal Filtro As String, ByVal Buscar As String, ByVal Jerarquia As String, ByVal IdContrato As String) As List(Of VI_LOGI_EQUIPO)
        'Public Function EquipoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEquipo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdJerarquia As String) As List(Of VI_LOGI_Equipo)
        'Return EquipoMet.EquipoListaGrid(Filtro, Buscar, IdEquipo, IdEmpresa, IdPuntoVenta, IdJerarquia)
        Return EquipoMet.EquipoListaGridV2(Filtro, Buscar, Jerarquia, IdContrato)
    End Function

    'Public Function EquipoListarPorId(ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String) As LOGI_EQUIPO
    Public Function EquipoListarPorId(ByVal IdEquipo As String) As LOGI_EQUIPO
        'If EquipoMet.EquipoExiste(IdEquipo, IdCatalogo, IdTipoActivo) = False Then
        If EquipoMet.EquipoExiste(IdEquipo) = False Then
            'Si el Equipo no existe lanzo una excepción.
            Throw New Exception("El Equipo con el id " & IdEquipo & " no Existe!!!")
        Else
            'Return EquipoMet.EquipoListarPorId(IdEquipo, IdCatalogo, IdTipoActivo)
            Return EquipoMet.EquipoListarPorId(IdEquipo)
        End If
    End Function

    'Public Function EquipoListarPorIdDetalle(ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String) As LOGI_EQUIPO
    Public Function EquipoListarPorIdDetalle(ByVal IdEquipo As String, ByVal IdCatalogo As String) As LOGI_EQUIPO
        'If EquipoMet.EquipoExiste(IdEquipo, IdCatalogo, IdTipoActivo) = False Then
        If EquipoMet.EquipoExiste(IdEquipo) = False Then
            'Si el Equipo no existe lanzo una excepción.
            Throw New Exception("El Equipo con el id " & IdEquipo & " no Existe!!!")
        Else
            'Return EquipoMet.EquipoListarPorId(IdEquipo)
            'Return EquipoMet.EquipoListarPorIdDetalle(IdEquipo, IdCatalogo, IdTipoActivo)
            Return EquipoMet.EquipoListarPorIdDetalle(IdEquipo, IdCatalogo)
        End If
    End Function
End Class
