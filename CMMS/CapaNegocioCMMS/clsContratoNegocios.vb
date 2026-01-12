Imports CapaDatosCMMS
Public Class clsContratoNegocios
    Dim ContratoMet As New clsContratoMetodos

    Public Function ContratoGetData(strQuery As String) As DataTable
        Return ContratoMet.ContratoGetData(strQuery)
    End Function

    Public Function ContratoRecursosListarCombo(ByVal OrdTra As LOGI_CABECERACONTRATO) As List(Of RRHH_PERSONAL)
        Return ContratoMet.ContratoRecursosListarCombo(OrdTra)
    End Function

    'Public Function ContratoListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERACONTRATO)
    '    Return ContratoMet.ContratoListarCombo(IdTipoOrden)
    'End Function
    Public Function ContratoListarCombo(ByVal bEstado As Boolean, ByVal cEstado As String) As List(Of LOGI_CABECERACONTRATO)
        Return ContratoMet.ContratoListarCombo(bEstado, cEstado)
    End Function

    Public Function ContratoListarComboByCliente(ByVal bEstado As Boolean, ByVal cEstado As String, ByVal cIdCliente As String) As List(Of LOGI_CABECERACONTRATO)
        Return ContratoMet.ContratoListarComboByCliente(bEstado, cEstado, cIdCliente)
    End Function

    'Public Function ContratoInsertaDetalle(ByVal Contrato As LOGI_CABECERACONTRATO, ByVal DetalleContrato As List(Of LOGI_DETALLECONTRATO), ByVal CheckListContrato As List(Of LOGI_CHECKLISTContrato), ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSContrato)) As Int32
    'Public Function ContratoInsertaDetalle(ByVal Contrato As LOGI_CABECERACONTRATO, ByVal DetalleContrato As List(Of LOGI_DETALLECONTRATO), ByVal PlanificacionContrato As List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO), ByVal OrdenesTrabajo As List(Of LOGI_CABECERAORDENTRABAJO), ByVal CheckListContrato As List(Of LOGI_CHECKLISTORDENTRABAJO), ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
    Public Function ContratoInsertaDetalle(ByVal Contrato As LOGI_CABECERACONTRATO, ByVal DetalleContrato As List(Of LOGI_DETALLECONTRATO), ByVal PlanificacionContrato As List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO), ByVal OrdenesTrabajo As List(Of LOGI_CABECERAORDENTRABAJO), ByVal RecursosHumanosContrato As DataTable) As Int32 'ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
        'Return ContratoMet.ContratoInsertaDetalle(Contrato, DetalleContrato, PlanificacionContrato, OrdenesTrabajo, CheckListContrato, RecursosHumanosContrato)
        Return ContratoMet.ContratoInsertaDetalle(Contrato, DetalleContrato, PlanificacionContrato, OrdenesTrabajo, RecursosHumanosContrato)
    End Function

    'Public Function ContratoInsertaDetallev2(ByVal DetalleContrato As LOGI_DETALLECONTRATO, ByVal PlanificacionContrato As LOGI_PLANIFICACIONEQUIPOCONTRATO, ByVal OrdenesTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal RecursosHumanosContrato As DataTable, ByVal componenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32 'ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
    Public Function ContratoInsertaDetallev2(ByVal DetalleContrato As LOGI_DETALLECONTRATO, ByVal PlanificacionContrato As LOGI_PLANIFICACIONEQUIPOCONTRATO, ByVal OrdenesTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO), ByVal componenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32 'ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
        'Return ContratoMet.ContratoInsertaDetallev2(DetalleContrato, PlanificacionContrato, OrdenesTrabajo, RecursosHumanosContrato, componenteOrdenTrabajo)
        Return ContratoMet.ContratoInsertaDetallev2(DetalleContrato, PlanificacionContrato, OrdenesTrabajo, RecursosHumanosContrato, componenteOrdenTrabajo)
    End Function


    Public Function ContratoInsertaDetalleReferenciaxEquipo(ByVal cIdTipoDocumentoCabeceraContrato As String, ByVal vIdNumeroSerieCabeceraContrato As String, ByVal vIdNumeroCorrelativoCabeceraContrato As String, ByVal cIdEmpresa As String, ByVal cIdEquipoDetalleContrato As String, ByVal vDescripcionDetalleContrato As String) As String
        Return ContratoMet.ContratoInsertaDetalleReferenciaxEquipo(cIdTipoDocumentoCabeceraContrato, vIdNumeroSerieCabeceraContrato, vIdNumeroCorrelativoCabeceraContrato, cIdEmpresa, cIdEquipoDetalleContrato, vDescripcionDetalleContrato)
    End Function

    Public Function ContratoInserta(ByVal Contrato As LOGI_CABECERACONTRATO) As Int32
        If ContratoMet.ContratoExiste(Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa) = True Then
            Throw New Exception("El Contrato con el id " & Contrato.vIdNumeroCorrelativoCabeceraContrato & " ya existe!")
        Else
            Return ContratoMet.ContratoInserta(Contrato)
        End If
    End Function

    Public Function ContratoEdita(ByVal Contrato As LOGI_CABECERACONTRATO) As Int32
        If ContratoMet.ContratoExiste(Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa) = False Then
            Throw New Exception("El Contrato con el id " & Contrato.vIdNumeroCorrelativoCabeceraContrato & " no existe!")
        Else
            Return ContratoMet.ContratoEdita(Contrato)
        End If
    End Function

    Public Function ContratoQuery(ByVal query As String) As Int32
        Return ContratoMet.ContratoQuery(query)
    End Function

    Public Function ContratoElimina(ByVal Contrato As LOGI_CABECERACONTRATO)
        If ContratoMet.ContratoExiste(Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa) = False Then
            Throw New Exception("El Contrato con el id " & Contrato.vIdNumeroCorrelativoCabeceraContrato & " no existe!")
        Else
            Return ContratoMet.ContratoElimina(Contrato)
        End If
    End Function

    ''Public Function ContratoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal booOrden As Boolean = True, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERAContrato)
    'Public Function ContratoListaBusquedaComponentes(ByVal Filtro As String, ByVal Buscar As String, ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As List(Of VI_LOGI_EQUIPO)
    '    Return ContratoMet.ContratoListaGridComponentes(Filtro, Buscar, IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa)
    'End Function
    Public Function ContratoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal SubFiltro As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERACONTRATO)
        Return ContratoMet.ContratoListaGrid(Filtro, Buscar, IdEmpresa, SubFiltro, Estado)
    End Function

    Public Function ContratoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERACONTRATO
        If ContratoMet.ContratoExiste(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Contrato con el id " & IdNroDoc & " no Existe!!!")
        Else
            Return ContratoMet.ContratoListarPorId(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa)
        End If
    End Function

    Public Function ContratoVigenciaInserta(ByVal Contrato As LOGI_CONTRATOVIGENCIA) As Int32
        'If ContratoMet.ContratoExiste(Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa) = True Then
        '    Throw New Exception("El Contrato con el id " & Contrato.vIdNumeroCorrelativoCabeceraContrato & " ya existe!")
        'Else
        '    Return ContratoMet.ContratoInserta(Contrato)
        'End If
        Return ContratoMet.ContratoVigenciaInserta(Contrato)
    End Function

    Public Function ContratoVigenciaEdita(ByVal Contrato As LOGI_CONTRATOVIGENCIA) As Int32
        'If ContratoMet.ContratoExiste(Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa) = True Then
        '    Throw New Exception("El Contrato con el id " & Contrato.vIdNumeroCorrelativoCabeceraContrato & " ya existe!")
        'Else
        '    Return ContratoMet.ContratoInserta(Contrato)
        'End If
        Return ContratoMet.ContratoVigenciaEdita(Contrato)
    End Function

    Public Function ContratoVigenciaLista(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As List(Of LOGI_CONTRATOVIGENCIA)
        Return ContratoMet.ContratroVigenciaListar(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa)
    End Function

    Public Function ContratoVigenciaQuery(ByVal query As String) As Int32
        Return ContratoMet.ContratoVigenciaQuery(query)
    End Function

    'Public Function ComboPeriodoVigencia(ByVal fechaIni As Date, ByVal fechaFin As Date) As List(Of BDCMMS_MovitecnicaDataContext.AnioVigencia)
    Public Function ComboPeriodoVigencia(ByVal fechaIni As Date, ByVal fechaFin As Date) As List(Of clsContratoMetodos.AnioVigencia)
        Return ContratoMet.ContratoPeriodoVigente(fechaIni, fechaFin)
    End Function

    'Public Function ContratoPeriodoMes(ByVal fechaIni As Date, ByVal fechaFin As Date, ByVal periodo As Integer) As List(Of BDCMMS_MovitecnicaDataContext.MesVigencia)
    Public Function ContratoPeriodoMes(ByVal fechaIni As Date, ByVal fechaFin As Date, ByVal periodo As Integer) As List(Of clsContratoMetodos.MesVigencia)
        Return ContratoMet.ContratoPeriodoMes(fechaIni, fechaFin, periodo)
    End Function
End Class