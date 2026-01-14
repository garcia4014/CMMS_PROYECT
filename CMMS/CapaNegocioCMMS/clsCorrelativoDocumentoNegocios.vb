Imports CapaDatosCMMS

Public Class clsCorrelativoDocumentoNegocios
    Dim CorrelativoMet As New clsCorrelativoDocumentoMetodos

    Public Function CorrelativoGetData(strQuery As String) As DataTable
        Return CorrelativoMet.CorrelativoGetData(strQuery)
    End Function

    Public Function CorrelativoInserta(ByVal Correlativo As GNRL_CORRELATIVODOCUMENTO) As Int32
        If CorrelativoMet.CorrelativoExisteV2(Correlativo.nNumeroSerieCorrelativoDocumento, Correlativo.cIdTipoDocumento, Correlativo.cIdEmpresa, Correlativo.bFacturacionElectronicaCorrelativoDocumento) = True Then
            Throw New Exception("El correlativo con número " & Correlativo.cIdTipoDocumento + " " + Correlativo.nNumeroSerieCorrelativoDocumento.ToString + "-" + Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento.ToString & " ya existe!")
        Else
            Return CorrelativoMet.CorrelativoInserta(Correlativo)
        End If
    End Function

    Public Function CorrelativoEdita(ByVal Correlativo As GNRL_CORRELATIVODOCUMENTO) As Int32
        If CorrelativoMet.CorrelativoExisteV2(Correlativo.nNumeroSerieCorrelativoDocumento, Correlativo.cIdTipoDocumento, Correlativo.cIdEmpresa, Correlativo.bFacturacionElectronicaCorrelativoDocumento) = False Then
            Throw New Exception("El correlativo con número " & Correlativo.cIdTipoDocumento + " " + Correlativo.nNumeroSerieCorrelativoDocumento.ToString + "-" + Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento.ToString & " no existe!")
        Else
            Return CorrelativoMet.CorrelativoEdita(Correlativo)
        End If
    End Function

    Public Function CorrelativoElimina(ByVal Correlativo As GNRL_CORRELATIVODOCUMENTO)
        If CorrelativoMet.CorrelativoExiste(Correlativo.cIdTipoDocumento, Correlativo.cIdEmpresa, Correlativo.bFacturacionElectronicaCorrelativoDocumento) = False Then
            Throw New Exception("El correlativo con número " & Correlativo.cIdTipoDocumento + " " + Correlativo.nNumeroSerieCorrelativoDocumento.ToString + "-" + Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento.ToString & " no existe!")
        Else
            Return CorrelativoMet.CorrelativoElimina(Correlativo)
        End If
    End Function

    Public Function CorrelativoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional FacElec As Boolean = False, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        Return CorrelativoMet.CorrelativoListaGrid(Filtro, Buscar, IdEmpresa, FacElec, Estado)
    End Function

    '    Public Function CorrelativoListarPorId(ByVal IdTipoDoc As String, ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, Optional FacElec As Boolean = False) As GNRL_CORRELATIVODOCUMENTO
    Public Function CorrelativoListarPorId(ByVal IdTipoDoc As String, ByVal IdSerieDoc As String, ByVal IdEmpresa As String, ByVal IdTipoDocumentoRef As String, Optional FacElec As Boolean = False) As GNRL_CORRELATIVODOCUMENTO
        If CorrelativoMet.CorrelativoExiste(IdTipoDoc, IdEmpresa, FacElec) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El correlativo con número " & IdTipoDoc & " " & " no existe!")
        Else
            Return CorrelativoMet.CorrelativoListarPorId(IdTipoDoc, IdSerieDoc, IdEmpresa, IdTipoDocumentoRef, FacElec)
        End If
    End Function

    'Public Function SerieListar(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String) As List(Of GNRL_CORRELATIVODOCUMENTO)
    'Public Function SerieListar(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
    Public Function SerieListar(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        'Public Function SerieListarCombo(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        Return CorrelativoMet.SerieListarCombo(IdPuntoVenta, IdEmpresa, IdTipoDocumento, IdCajero, IdTipoDocumentoRef, FacElec)
    End Function

    'Public Function NroDocumentoListar(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
    Public Function NroDocumentoListar(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        Return CorrelativoMet.CorrelativoListarCombo(IdPuntoVenta, IdEmpresa, IdTipoDocumento, IdCajero, IdTipoDocumentoRef, FacElec)
    End Function
End Class