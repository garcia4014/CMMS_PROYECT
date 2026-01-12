Imports CapaDatosCMMS

Public Class clsOtrosDatosOrdenTrabajoNegocios
    Dim OtrosDatosOrdenTrabajoMet As New clsOtrosDatosOrdenTrabajoMetodos

    Public Function OtrosDatosOrdenTrabajoGetData(strQuery As String) As DataTable
        Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoGetData(strQuery)
    End Function

    'Public Function OtrosDatosOrdenTrabajoListarCombo() As List(Of LOGI_OTROSDATOSORDENTRABAJO)
    '    Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoListarCombo()
    'End Function

    Public Function OtrosDatosOrdenTrabajoInserta(ByVal OtrosDatosOrdenTrabajo As LOGI_OTROSDATOSORDENTRABAJO) As Int32
        If OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoExiste(OtrosDatosOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdEmpresa, OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajo) = True Then
            Throw New Exception("La característica con el id " & OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajo.ToString & " ya existe!")
        Else
            Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoInserta(OtrosDatosOrdenTrabajo)
        End If
    End Function

    Public Function OtrosDatosOrdenTrabajoInsertaDetalle(ByVal OtrosDatosOrdenTrabajo As List(Of LOGI_OTROSDATOSORDENTRABAJO), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoInsertaDetalle(OtrosDatosOrdenTrabajo, LogAuditoria)
    End Function


    Public Function OtrosDatosOrdenTrabajoEdita(ByVal OtrosDatosOrdenTrabajo As LOGI_OTROSDATOSORDENTRABAJO) As Int32
        If OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoExiste(OtrosDatosOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdEmpresa, OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajo) = True Then
            Throw New Exception("La característica con el id " & OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajo.ToString & " no existe!")
        Else
            Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoEdita(OtrosDatosOrdenTrabajo)
        End If
    End Function

    'Public Function OtrosDatosOrdenTrabajoElimina(ByVal OtrosDatosOrdenTrabajo As LOGI_OTROSDATOSORDENTRABAJO)
    '    If OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoExiste(OtrosDatosOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OtrosDatosOrdenTrabajo.cIdEmpresa, OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajoOrdenTrabajo) = True Then
    '        Throw New Exception("La imagen con el id " & OtrosDatosOrdenTrabajo.nIdNumeroItemOtrosDatosOrdenTrabajoOrdenTrabajo.ToString & " no existe!")
    '    Else
    '        Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoElimina(OtrosDatosOrdenTrabajo)
    '    End If
    'End Function

    Public Function OtrosDatosOrdenTrabajoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_LOGI_OTROSDATOSORDENTRABAJO)
        Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoListaGrid(Filtro, Buscar)
    End Function

    Public Function OtrosDatosOrdenTrabajoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdNroItem As String) As LOGI_OTROSDATOSORDENTRABAJO
        If OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoExiste(IdTipDoc, IdNroSer, IdNroDoc, IdEmpresa, IdNroItem) = True Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La imagen con el id " & IdNroItem.ToString & " no Existe!!!")
        Else
            Return OtrosDatosOrdenTrabajoMet.OtrosDatosOrdenTrabajoListarPorId(IdTipDoc, IdNroSer, IdNroDoc, IdEmpresa, IdNroItem)
        End If
    End Function
End Class
