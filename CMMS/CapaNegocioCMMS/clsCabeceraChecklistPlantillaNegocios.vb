Imports CapaDatosCMMS

Public Class clsCabeceraChecklistPlantillaNegocios
    Dim CheckListPlantillaMet As New clsCabeceraChecklistPlantillaMetodos

    Public Function ChecklistPlantillaGetData(strQuery As String) As DataTable
        Return CheckListPlantillaMet.ChecklistPlantillaGetData(strQuery)
    End Function

    Public Function ChecklistPlantillaListarCombo(ByVal IdTipoMantenimiento As String, ByVal IdTipoActivo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String) As List(Of LOGI_CABECERACHECKLISTPLANTILLA)
        Return CheckListPlantillaMet.ChecklistPlantillaListarCombo(IdTipoMantenimiento, IdTipoActivo, IdCatalogo, IdJerarquia)
    End Function

    Public Function ChecklistPlantillaAllListarCombo() As List(Of LOGI_CABECERACHECKLISTPLANTILLA)
        Return CheckListPlantillaMet.ChecklistPlantillaAllListarCombo()
    End Function

    Public Function ChecklistPlantillaInserta(ByVal CheckListPlantilla As LOGI_CABECERACHECKLISTPLANTILLA) As Int32
        If CheckListPlantillaMet.ChecklistPlantillaExiste(CheckListPlantilla.cIdTipoMantenimiento, CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla) = True Then
            Throw New Exception("La Plantilla de CheckList con el id " & CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla & " ya existe!")
        Else
            Return CheckListPlantillaMet.ChecklistPlantillaInserta(CheckListPlantilla)
        End If
    End Function

    Public Function ChecklistPlantillaInsertaCopiaComponentes(ByVal cIdEquipo As String, ByVal cIdPlantilla As String) As Int32
        Return CheckListPlantillaMet.ChecklistPlantillaInsertaCopiaComponentes(cIdEquipo, cIdPlantilla)
    End Function

    'Public Function ChecklistPlantillaInsertaDetalle(ByVal CheckListPlantilla As List(Of LOGI_CABECERACHECKLISTPLANTILLA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
    '    Return CheckListPlantillaMet.ChecklistPlantillaInsertaDetalle(CheckListPlantilla, LogAuditoria)
    'End Function

    Public Function ChecklistPlantillaEdita(ByVal ChecklistPlantilla As LOGI_CABECERACHECKLISTPLANTILLA) As Int32
        If CheckListPlantillaMet.ChecklistPlantillaExiste(ChecklistPlantilla.cIdTipoMantenimiento, ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla) = False Then
            Throw New Exception("La Plantilla de CheckList con el id " & ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla & " no existe!")
        Else
            Return CheckListPlantillaMet.ChecklistPlantillaEdita(ChecklistPlantilla)
        End If
    End Function

    Public Function ChecklistPlantillaElimina(ByVal ChecklistPlantilla As LOGI_CABECERACHECKLISTPLANTILLA)
        If CheckListPlantillaMet.ChecklistPlantillaExiste(ChecklistPlantilla.cIdTipoMantenimiento, ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla) = False Then
            Throw New Exception("La Plantilla de CheckList con el id " & ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla & " no existe!")
        Else
            Return CheckListPlantillaMet.ChecklistPlantillaElimina(ChecklistPlantilla)
        End If
    End Function

    'Public Function ChecklistPlantillaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERACHECKLISTPLANTILLA)
    Public Function ChecklistPlantillaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERACHECKLISTPLANTILLA)
        Return CheckListPlantillaMet.ChecklistPlantillaListaGrid(Filtro, Buscar, Estado)
    End Function

    'Public Function ChecklistPlantillaListarPorIdCatalogo(ByVal IdActividadCheckList As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_CABECERACHECKLISTPLANTILLA
    '    If CheckListPlantillaMet.ChecklistPlantillaExiste(IdActividadCheckList, IdTipoMantenimiento, IdCatalogo, IdJerarquiaCatalogo) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("La Plantilla de CheckList con el id " & IdActividadCheckList & " no Existe!!!")
    '    Else
    '        Return CheckListPlantillaMet.ChecklistPlantillaListarPorId(IdActividadCheckList, IdTipoMantenimiento, IdCatalogo, IdJerarquiaCatalogo)
    '    End If
    'End Function

    Public Function ChecklistPlantillaListarPorId(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String) As LOGI_CABECERACHECKLISTPLANTILLA
        If CheckListPlantillaMet.ChecklistPlantillaExiste(IdTipoMantenimiento, IdNumero) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Plantilla de CheckList con el id " & IdNumero & " no Existe!!!")
        Else
            Return CheckListPlantillaMet.ChecklistPlantillaListarPorId(IdTipoMantenimiento, IdNumero)
        End If
    End Function
End Class
