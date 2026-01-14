Imports CapaDatosCMMS

Public Class clsCaracteristicaNegocios
    Dim CaracteristicaMet As New clsCaracteristicaMetodos

    Public Function CaracteristicaGetData(strQuery As String) As DataTable
        Return CaracteristicaMet.CaracteristicaGetData(strQuery)
    End Function

    Public Function CaracteristicaInserta(ByVal Caracteristica As LOGI_CARACTERISTICA) As Int32
        If CaracteristicaMet.CaracteristicaExiste(Caracteristica.cIdCaracteristica) = True Then
            Throw New Exception("El Caracteristica con el id " & Caracteristica.cIdCaracteristica & " ya existe!")
        Else
            Return CaracteristicaMet.CaracteristicaInserta(Caracteristica)
        End If
    End Function

    Public Function CaracteristicaCatalogoInserta(ByVal Cat As LOGI_CATALOGO, ByVal CatCar As List(Of LOGI_CATALOGOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA)
        Return CaracteristicaMet.CaracteristicaCatalogoInserta(Cat, CatCar, LogAuditoria)
    End Function

    'Public Function CaracteristicaCatalogoInsertaDetalle(ByVal Cat As LOGI_CATALOGO, ByVal CatCar As List(Of LOGI_CATALOGOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA)
    Public Function CaracteristicaCatalogoInsertaDetalle(ByVal CatCar As List(Of LOGI_CATALOGOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        'Return CaracteristicaMet.CaracteristicaCatalogoInsertaDetalle(Cat, CatCar, LogAuditoria)
        Return CaracteristicaMet.CaracteristicaCatalogoInsertaDetalle(CatCar, LogAuditoria)
    End Function
    Public Function CaracteristicaEquipoInsertaDetalle(ByVal IdEquipoPrincipal As String, ByVal EquipoCar As List(Of LOGI_EQUIPOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA)
        'Return CaracteristicaMet.CaracteristicaCatalogoInsertaDetalle(Cat, CatCar, LogAuditoria)
        Return CaracteristicaMet.CaracteristicaEquipoInsertaDetalle(IdEquipoPrincipal, EquipoCar, LogAuditoria)
    End Function

    Public Function CaracteristicaEdita(ByVal Caracteristica As LOGI_CARACTERISTICA) As Int32
        If CaracteristicaMet.CaracteristicaExiste(Caracteristica.cIdCaracteristica) = False Then
            Throw New Exception("El Caracteristica con el id " & Caracteristica.cIdCaracteristica & " no existe!")
        Else
            Return CaracteristicaMet.CaracteristicaEdita(Caracteristica)
        End If
    End Function

    Public Function CaracteristicaElimina(ByVal Caracteristica As LOGI_CARACTERISTICA)
        If CaracteristicaMet.CaracteristicaExiste(Caracteristica.cIdCaracteristica) = False Then
            Throw New Exception("El Caracteristica con el id " & Caracteristica.cIdCaracteristica & " no existe!")
        Else
            Return CaracteristicaMet.CaracteristicaElimina(Caracteristica)
        End If
    End Function

    Public Function CaracteristicaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_CARACTERISTICA)
        Return CaracteristicaMet.CaracteristicaListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function CaracteristicaListarPorId(ByVal IdCaracteristica As String) As LOGI_CARACTERISTICA
        If CaracteristicaMet.CaracteristicaExiste(IdCaracteristica) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Caracteristica con el id " & IdCaracteristica & " no Existe!!!")
        Else
            Return CaracteristicaMet.CaracteristicaListarPorId(IdCaracteristica)
        End If
    End Function

    Public Function CaracteristicaListarCombo() As List(Of LOGI_CARACTERISTICA)
        Return CaracteristicaMet.CaracteristicaListarCombo
    End Function
End Class