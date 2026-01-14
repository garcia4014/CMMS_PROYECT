Imports CapaDatosCMMS
Public Class clsOrdenFabricacionNegocios
    Dim OrdenFabricacionMet As New clsOrdenFabricacionMetodos

    Public Function OrdenFabricacionGetData(strQuery As String) As DataTable
        Return OrdenFabricacionMet.OrdenFabricacionGetData(strQuery)
    End Function

    Public Function OrdenFabricacionListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENFABRICACION)
        Return OrdenFabricacionMet.OrdenFabricacionListarCombo(IdTipoOrden)
    End Function

    Public Function OrdenFabricacionUnidadTrabajoListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENFABRICACION)
        Return OrdenFabricacionMet.OrdenFabricacionUnidadTrabajoListarCombo(IdTipoOrden)
    End Function

    Public Function OrdenFabricacionInserta(ByVal OrdenFabricacion As LOGI_CABECERAORDENFABRICACION) As Int32
        If OrdenFabricacionMet.OrdenFabricacionExiste(OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion, OrdenFabricacion.cIdEmpresa) = True Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion & " ya existe!")
        Else
            Return OrdenFabricacionMet.OrdenFabricacionInserta(OrdenFabricacion)
        End If
    End Function

    Public Function OrdenFabricacionEdita(ByVal OrdenFabricacion As LOGI_CABECERAORDENFABRICACION) As Int32
        If OrdenFabricacionMet.OrdenFabricacionExiste(OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion, OrdenFabricacion.cIdEmpresa) = False Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion & " no existe!")
        Else
            Return OrdenFabricacionMet.OrdenFabricacionEdita(OrdenFabricacion)
        End If
    End Function

    Public Function OrdenFabricacionElimina(ByVal OrdenFabricacion As LOGI_CABECERAORDENFABRICACION)
        If OrdenFabricacionMet.OrdenFabricacionExiste(OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion, OrdenFabricacion.cIdEmpresa) = False Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion & " no existe!")
        Else
            Return OrdenFabricacionMet.OrdenFabricacionElimina(OrdenFabricacion)
        End If
    End Function

    'Public Function OrdenFabricacionListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal booOrden As Boolean = True, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERAORDENFABRICACION)
    Public Function OrdenFabricacionListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERAORDENFABRICACION)
        Return OrdenFabricacionMet.OrdenFabricacionListaGrid(Filtro, Buscar, IdEmpresa, Estado)
    End Function

    Public Function OrdenFabricacionActualizarDisponibilidadStock(ByVal DetOrdFab As List(Of LOGI_DETALLEORDENFABRICACION), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Return OrdenFabricacionMet.OrdenFabricacionActualizarDisponibilidadStock(DetOrdFab, LogAuditoria)
    End Function

    Public Function OrdenFabricacionListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENFABRICACION
        If OrdenFabricacionMet.OrdenFabricacionExiste(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Orden de Fabricacion con el id " & IdNroDoc & " no Existe!!!")
        Else
            Return OrdenFabricacionMet.OrdenFabricacionListarPorId(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa)
        End If
    End Function

    Public Function OrdenFabricacionListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdArticuloSAPCabecera As String, Optional IdArticuloSAPDetalle As String = "*") As LOGI_DETALLEORDENFABRICACION
        'If OrdenFabricacionMet.OrdenFabricacionExiste(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa, IdArticuloSAPCabecera, IdArticuloSAPDetalle) = False Then
        '    'Si el producto no existe lanzo una excepción.
        '    Throw New Exception("La Orden de Fabricacion con el id " & IdNroDoc & " no Existe!!!")
        'Else
        Return OrdenFabricacionMet.OrdenFabricacionListarPorIdDetalle(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa, IdArticuloSAPCabecera, IdArticuloSAPDetalle)
        'End If
    End Function
End Class