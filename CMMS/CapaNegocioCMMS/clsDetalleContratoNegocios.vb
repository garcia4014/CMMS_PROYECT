Imports CapaDatosCMMS

Public Class clsDetalleContratoNegocios
    Dim DetalleOrdenTrabajoMet As New CapaDatosCMMS.clsDetalleOrdenTrabajoMetodos

    Public Function DetalleOrdenTrabajoGetData(strQuery As String) As DataTable
        Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoGetData(strQuery)
    End Function

    'Public Function DetalleOrdenTrabajoListarCombo() As List(Of LOGI_DetalleOrdenTrabajo)
    '    Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoListarCombo()
    'End Function

    Public Function DetalleOrdenTrabajoInserta(ByVal DetalleOrdenTrabajo As LOGI_DETALLEORDENTRABAJO) As Int32
        If DetalleOrdenTrabajoMet.DetalleOrdenTrabajoExiste(DetalleOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdEmpresa, DetalleOrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdActividadCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo) = True Then
            Throw New Exception("El Articulo con el id " & DetalleOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo & " ya existe!")
        Else
            Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoInserta(DetalleOrdenTrabajo)
        End If
    End Function

    Public Function DetalleOrdenTrabajoInsertaDetalle(ByVal DetalleOrdenTrabajo As List(Of LOGI_DETALLEORDENTRABAJO), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoInsertaDetalle(DetalleOrdenTrabajo, LogAuditoria)
    End Function

    Public Function DetalleOrdenTrabajoEdita(ByVal DetalleOrdenTrabajo As LOGI_DETALLEORDENTRABAJO) As Int32
        If DetalleOrdenTrabajoMet.DetalleOrdenTrabajoExiste(DetalleOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdEmpresa, DetalleOrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdActividadCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo) = False Then
            Throw New Exception("El Articulo con el id " & DetalleOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo & " no existe!")
        Else
            Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoEdita(DetalleOrdenTrabajo)
        End If
    End Function

    'Public Function DetalleOrdenTrabajoElimina(ByVal DetalleOrdenTrabajo As LOGI_DETALLEORDENTRABAJO)
    '    If DetalleOrdenTrabajoMet.DetalleOrdenTrabajoExiste(DetalleOrdenTrabajo.cIdTipoMantenimiento, DetalleOrdenTrabajo.cIdNumeroCabeceraDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdActividadCheckList, DetalleOrdenTrabajo.cIdCatalogo, DetalleOrdenTrabajo.cIdJerarquiaCatalogo) = False Then
    '        Throw New Exception("La Actividad con el id " & DetalleOrdenTrabajo.cIdActividadCheckList & " no existe!")
    '    Else
    '        Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoElimina(DetalleOrdenTrabajo)
    '    End If
    'End Function

    Public Function DetalleOrdenTrabajoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_DETALLEORDENTRABAJO)
        Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoListaGrid(Filtro, Buscar, Estado)
    End Function

    'Public Function DetalleOrdenTrabajoListarPorIdCatalogo(ByVal IdActividadCheckList As String, ByVal IdNumero As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_DetalleOrdenTrabajo
    '    If DetalleOrdenTrabajoMet.DetalleOrdenTrabajoExiste(IdTipoMantenimiento, IdNumero, IdActividadCheckList, IdCatalogo, IdJerarquiaCatalogo) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("La Actividad con el id " & IdActividadCheckList & " no Existe!!!")
    '    Else
    '        Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoListarPorId(IdTipoMantenimiento, IdNumero, IdActividadCheckList, IdCatalogo, IdJerarquiaCatalogo)
    '    End If
    'End Function

    'Public Function DetalleOrdenTrabajoListarPorId(ByVal IdActividadCheckList As String, ByVal IdNumero As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_DETALLEORDENTRABAJO
    Public Function DetalleOrdenTrabajoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticulo As String) As LOGI_DETALLEORDENTRABAJO
        If DetalleOrdenTrabajoMet.DetalleOrdenTrabajoExiste(IdTipDoc, IdNroSer, IdNroDoc, IdEmpresa, IdEquipo, IdCatalogo, IdJerarquia, IdActividad, IdArticulo) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Articulo con el id " & IdArticulo & " no Existe!!!")
        Else
            Return DetalleOrdenTrabajoMet.DetalleOrdenTrabajoListarPorId(IdTipDoc, IdNroSer, IdNroDoc, IdEmpresa, IdEquipo, IdCatalogo, IdJerarquia, IdActividad, IdArticulo)
        End If
    End Function
End Class