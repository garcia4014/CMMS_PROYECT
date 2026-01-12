Imports System.Data.SqlClient
Imports CapaDatosCMMS
Public Class clsOrdenTrabajoNegocios
    Dim OrdenTrabajoMet As New clsOrdenTrabajoMetodos

    Public Function OrdenTrabajoGetData(strQuery As String) As DataTable
        Return OrdenTrabajoMet.OrdenTrabajoGetData(strQuery)
    End Function

    Public Function OrdenTrabajoRecursosListarCombo(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO) As List(Of RRHH_PERSONAL)
        Return OrdenTrabajoMet.OrdenTrabajoRecursosListarCombo(OrdTra)
    End Function

    Public Function OrdenTrabajoRecursosListarComboV2(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO) As List(Of RRHH_PERSONAL)
        Return OrdenTrabajoMet.OrdenTrabajoRecursosListarComboV2(OrdTra)
    End Function

    Public Function OrdenTrabajoRecursos(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO) As List(Of LOGI_RECURSOSORDENTRABAJO)
        Return OrdenTrabajoMet.OrdenTrabajoRecursosListar(OrdTra)
    End Function

    'Public Function TipoFrecuenciaLista() As List(Of GNRL_TIPOFRECUENCIA)
    '    Return OrdenTrabajoMet.TipoFrecuenciaLista()
    'End Function

    Public Function OrdenTrabajoValidarRecurso(ByVal query As String) As List(Of LOGI_RECURSOSORDENTRABAJO)
        Return OrdenTrabajoMet.OrdenTrabajoValidarRecurso(query)
    End Function

    Public Function OrdenTrabajoListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENTRABAJO)
        Return OrdenTrabajoMet.OrdenTrabajoListarCombo(IdTipoOrden)
    End Function

    Public Function OrdenTrabajoInsertaDetalle(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal DetalleOrdenTrabajo As List(Of LOGI_DETALLEORDENTRABAJO), ByVal CheckListOrdenTrabajo As List(Of LOGI_CHECKLISTORDENTRABAJO), ByVal RecursosHumanosOrdenTrabajo As List(Of LOGI_RECURSOSORDENTRABAJO), ByVal ComponenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32
        Return OrdenTrabajoMet.OrdenTrabajoInsertaDetalle(OrdenTrabajo, DetalleOrdenTrabajo, CheckListOrdenTrabajo, RecursosHumanosOrdenTrabajo, ComponenteOrdenTrabajo)
    End Function
    Public Function OrdenTrabajoDeleteAndInsertComponentes(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal CheckListOrdenTrabajo As List(Of LOGI_CHECKLISTORDENTRABAJO), ByVal ComponenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32
        Return OrdenTrabajoMet.OrdenTrabajoDeleteAndInsertComponentes(OrdenTrabajo, CheckListOrdenTrabajo, ComponenteOrdenTrabajo)
    End Function

    Public Function OrdenTrabajoQueryResponsable(ByRef query As String) As Int32
        Return OrdenTrabajoMet.QueryRecursosOT(query)

    End Function
    Public Function OrdenTrabajoInsertResponsable(ByRef recursoOT As LOGI_RECURSOSORDENTRABAJO) As Int32
        Return OrdenTrabajoMet.InsertRecursosOT(recursoOT)

    End Function

    Public Function OrdenTrabajoUpdateResponsable(ByRef recursoOT As LOGI_RECURSOSORDENTRABAJO) As Int32
        Return OrdenTrabajoMet.UpdateRecursosOT(recursoOT)

    End Function

    Public Function OrdenTrabajoUpdateResponsable(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO) As Int32
        If OrdenTrabajoMet.OrdenTrabajoExiste(OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa) = True Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & " ya existe!")
        Else
            Return OrdenTrabajoMet.OrdenTrabajoInserta(OrdenTrabajo)
        End If
    End Function

    Public Function OrdenTrabajoInserta(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO) As Int32
        If OrdenTrabajoMet.OrdenTrabajoExiste(OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa) = True Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & " ya existe!")
        Else
            Return OrdenTrabajoMet.OrdenTrabajoInserta(OrdenTrabajo)
        End If
    End Function

    Public Function OrdenTrabajoEdita(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO) As Int32
        If OrdenTrabajoMet.OrdenTrabajoExiste(OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa) = False Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & " no existe!")
        Else
            Return OrdenTrabajoMet.OrdenTrabajoEdita(OrdenTrabajo)
        End If
    End Function

    Public Function OrdenTrabajoElimina(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO)
        If OrdenTrabajoMet.OrdenTrabajoExiste(OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa) = False Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & " no existe!")
        Else
            Return OrdenTrabajoMet.OrdenTrabajoElimina(OrdenTrabajo)
        End If
    End Function

    ''Public Function OrdenTrabajoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal booOrden As Boolean = True, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERAOrdenTrabajo)
    'Public Function OrdenTrabajoListaBusquedaComponentes(ByVal Filtro As String, ByVal Buscar As String, ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As List(Of VI_LOGI_EQUIPO)
    '    Return OrdenTrabajoMet.OrdenTrabajoListaGridComponentes(Filtro, Buscar, IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa)
    'End Function
    Public Function OrdenTrabajoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal SubFiltro As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERAORDENTRABAJO)
        Return OrdenTrabajoMet.OrdenTrabajoListaGrid(Filtro, Buscar, IdEmpresa, SubFiltro, Estado)
    End Function

    Public Function OrdenTrabajoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENTRABAJO
        If OrdenTrabajoMet.OrdenTrabajoExiste(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Orden de Fabricacion con el id " & IdNroDoc & " no Existe!!!")
        Else
            Return OrdenTrabajoMet.OrdenTrabajoListarPorId(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa)
        End If
    End Function

    Public Function OrdenTrabajoAddActividad(ByVal cIdTipoDocumento As String,
         ByVal vIdNumeroSerie As String,
         ByVal vIdNumeroCorrelativo As String,
         ByVal vObservacion As String,
         ByVal cIdEmpresa As String,
         ByVal cEstado As System.Nullable(Of Char),
         ByVal nTotalSegundosTrabajados As System.Nullable(Of Integer),
         ByVal dFechaInicio As System.Nullable(Of Date),
         ByVal dFechaFinal As System.Nullable(Of Date),
         ByVal cIdEquipo As String,
         ByVal cIdActividad As String,
         ByVal cIdTipoMantenimiento As String,
         ByVal cIdNumeroCabeceraCheckListPlantilla As String,
         ByVal cIdCatalogo As String,
         ByVal cIdJerarquiaCatalogo As System.Nullable(Of Char),
         ByVal vIdArticuloSAPCabecera As String,
         ByVal cEstadoActividad As System.Nullable(Of Char),
         ByVal cIdEquipoCheckList As String) As Int32

        Return OrdenTrabajoMet.CheckListInsertaActividad(cIdTipoDocumento, vIdNumeroSerie, vIdNumeroCorrelativo, vObservacion, cIdEmpresa,
                                                                       cEstado, nTotalSegundosTrabajados, dFechaInicio, dFechaFinal, cIdEquipo, cIdActividad,
                                                                       cIdTipoMantenimiento, cIdNumeroCabeceraCheckListPlantilla, cIdCatalogo, cIdJerarquiaCatalogo,
                                                                       vIdArticuloSAPCabecera, cEstadoActividad, cIdEquipoCheckList)
    End Function

    Public Sub EjecutarComando(query As String, parametros As List(Of SqlParameter))
        OrdenTrabajoMet.EjecutarComando(query, parametros)
    End Sub
End Class