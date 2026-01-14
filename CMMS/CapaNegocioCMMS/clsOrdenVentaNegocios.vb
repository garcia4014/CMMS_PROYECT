Imports CapaDatosCMMS
Public Class clsOrdenVentaNegocios
    Dim OrdenVentaMet As New clsOrdenVentaMetodos

    Public Function OrdenVentaGetData(strQuery As String) As DataTable
        Return OrdenVentaMet.OrdenVentaGetData(strQuery)
    End Function

    Public Function OrdenVentaListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENVENTA)
        Return OrdenVentaMet.OrdenVentaListarCombo(IdTipoOrden)
    End Function

    Public Function OrdenVentaUnidadTrabajoListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENVENTA)
        Return OrdenVentaMet.OrdenVentaUnidadTrabajoListarCombo(IdTipoOrden)
    End Function

    Public Function OrdenVentaInserta(ByVal OrdenVenta As LOGI_CABECERAORDENVENTA) As Int32
        If OrdenVentaMet.OrdenVentaExiste(OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta, OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta, OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta, OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta, OrdenVenta.cIdEmpresa) = True Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta & " ya existe!")
        Else
            Return OrdenVentaMet.OrdenVentaInserta(OrdenVenta)
        End If
    End Function

    Public Function OrdenVentaEdita(ByVal OrdenVenta As LOGI_CABECERAORDENVENTA) As Int32
        If OrdenVentaMet.OrdenVentaExiste(OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta, OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta, OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta, OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta, OrdenVenta.cIdEmpresa) = False Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta & " no existe!")
        Else
            Return OrdenVentaMet.OrdenVentaEdita(OrdenVenta)
        End If
    End Function

    Public Function OrdenVentaElimina(ByVal OrdenVenta As LOGI_CABECERAORDENVENTA)
        If OrdenVentaMet.OrdenVentaExiste(OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta, OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta, OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta, OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta, OrdenVenta.cIdEmpresa) = False Then
            Throw New Exception("La Orden de Fabricacion con el id " & OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta & " no existe!")
        Else
            Return OrdenVentaMet.OrdenVentaElimina(OrdenVenta)
        End If
    End Function

    'Public Function OrdenVentaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal booOrden As Boolean = True, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERAOrdenVenta)
    Public Function OrdenVentaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_CABECERAORDENVENTA)
        Return OrdenVentaMet.OrdenVentaListaGrid(Filtro, Buscar, IdEmpresa, Estado)
    End Function

    Public Function OrdenVentaActualizarDisponibilidadStock(ByVal DetOrdFab As List(Of LOGI_DETALLEORDENVENTA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Return OrdenVentaMet.OrdenVentaActualizarDisponibilidadStock(DetOrdFab, LogAuditoria)
    End Function

    Public Function OrdenVentaListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdNroOF As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENVENTA
        If OrdenVentaMet.OrdenVentaExiste(IdTipDoc, IdNroSerie, IdNroDoc, IdNroOF, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Orden de Fabricacion con el id " & IdNroDoc & " no Existe!!!")
        Else
            Return OrdenVentaMet.OrdenVentaListarPorId(IdTipDoc, IdNroSerie, IdNroDoc, IdNroOF, IdEmpresa)
        End If
    End Function

    Public Function OrdenVentaListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdOrdenFabricacion As String, Optional IdArticuloSAPDetalle As String = "*") As LOGI_DETALLEORDENVENTA
        'If OrdenVentaMet.OrdenVentaExiste(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa, IdArticuloSAPCabecera, IdArticuloSAPDetalle) = False Then
        '    'Si el producto no existe lanzo una excepción.
        '    Throw New Exception("La Orden de Fabricacion con el id " & IdNroDoc & " no Existe!!!")
        'Else
        Return OrdenVentaMet.OrdenVentaListarPorIdDetalle(IdTipDoc, IdNroSerie, IdNroDoc, IdEmpresa, IdOrdenFabricacion, IdArticuloSAPDetalle)
        'End If
    End Function
End Class