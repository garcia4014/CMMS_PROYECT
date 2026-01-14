Imports CapaDatosCMMS

Public Class clsDetalleChecklistPlantillaNegocios
    Dim ChecklistPlantillaMet As New CapaDatosCMMS.clsDetalleChecklistPlantillaMetodos

    Public Function DetalleChecklistPlantillaGetData(strQuery As String) As DataTable
        Return ChecklistPlantillaMet.DetalleChecklistPlantillaGetData(strQuery)
    End Function

    'Public Function ChecklistPlantillaListarCombo() As List(Of LOGI_CHECKLISTPLANTILLA)
    '    Return ChecklistPlantillaMet.ChecklistPlantillaListarCombo()
    'End Function

    Public Function DetalleChecklistPlantillaInserta(ByVal CheckListPlantilla As LOGI_DETALLECHECKLISTPLANTILLA) As Int32
        If ChecklistPlantillaMet.DetalleChecklistPlantillaExiste(CheckListPlantilla.cIdTipoMantenimiento, CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla, CheckListPlantilla.cIdActividadCheckList, CheckListPlantilla.cIdCatalogo, CheckListPlantilla.cIdJerarquiaCatalogo) = True Then
            Throw New Exception("La Actividad con el id " & CheckListPlantilla.cIdActividadCheckList & " ya existe!")
        Else
            Return ChecklistPlantillaMet.DetalleChecklistPlantillaInserta(CheckListPlantilla)
        End If
    End Function

    Public Function DetalleChecklistPlantillaInsertaDetalle(ByVal CabeceraChecklistPlantilla As LOGI_CABECERACHECKLISTPLANTILLA, ByVal CheckListPlantilla As List(Of LOGI_DETALLECHECKLISTPLANTILLA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Return ChecklistPlantillaMet.DetalleChecklistPlantillaInsertaDetalle(CabeceraChecklistPlantilla, CheckListPlantilla, LogAuditoria)
    End Function

    Public Function DetalleChecklistPlantillaEdita(ByVal ChecklistPlantilla As LOGI_DETALLECHECKLISTPLANTILLA) As Int32
        If ChecklistPlantillaMet.DetalleChecklistPlantillaExiste(ChecklistPlantilla.cIdTipoMantenimiento, ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla, ChecklistPlantilla.cIdActividadCheckList, ChecklistPlantilla.cIdCatalogo, ChecklistPlantilla.cIdJerarquiaCatalogo) = False Then
            Throw New Exception("La Actividad con el id " & ChecklistPlantilla.cIdActividadCheckList & " no existe!")
        Else
            Return ChecklistPlantillaMet.DetalleChecklistPlantillaEdita(ChecklistPlantilla)
        End If
    End Function

    Public Function DetalleChecklistPlantillaElimina(ByVal ChecklistPlantilla As LOGI_DETALLECHECKLISTPLANTILLA)
        If ChecklistPlantillaMet.DetalleChecklistPlantillaExiste(ChecklistPlantilla.cIdTipoMantenimiento, ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla, ChecklistPlantilla.cIdActividadCheckList, ChecklistPlantilla.cIdCatalogo, ChecklistPlantilla.cIdJerarquiaCatalogo) = False Then
            Throw New Exception("La Actividad con el id " & ChecklistPlantilla.cIdActividadCheckList & " no existe!")
        Else
            Return ChecklistPlantillaMet.DetalleChecklistPlantillaElimina(ChecklistPlantilla)
        End If
    End Function

    Public Function DetalleChecklistPlantillaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_DETALLECHECKLISTPLANTILLA)
        Return ChecklistPlantillaMet.DetalleChecklistPlantillaListaGrid(Filtro, Buscar, Estado)
    End Function

    'Public Function DetalleChecklistPlantillaListarPorIdCatalogo(ByVal IdActividadCheckList As String, ByVal IdNumero As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_DETALLECHECKLISTPLANTILLA
    '    If ChecklistPlantillaMet.DetalleChecklistPlantillaExiste(IdTipoMantenimiento, IdNumero, IdActividadCheckList, IdCatalogo, IdJerarquiaCatalogo) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("La Actividad con el id " & IdActividadCheckList & " no Existe!!!")
    '    Else
    '        Return ChecklistPlantillaMet.DetalleChecklistPlantillaListarPorId(IdTipoMantenimiento, IdNumero, IdActividadCheckList, IdCatalogo, IdJerarquiaCatalogo)
    '    End If
    'End Function

    Public Function DetalleChecklistPlantillaListarPorId(ByVal IdActividadCheckList As String, ByVal IdNumero As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_DETALLECHECKLISTPLANTILLA
        If ChecklistPlantillaMet.DetalleChecklistPlantillaExiste(IdTipoMantenimiento, IdNumero, IdActividadCheckList, IdCatalogo, IdJerarquiaCatalogo) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Actividad con el id " & IdActividadCheckList & " no Existe!!!")
        Else
            Return ChecklistPlantillaMet.DetalleChecklistPlantillaListarPorId(IdTipoMantenimiento, IdNumero)
        End If
    End Function
End Class
