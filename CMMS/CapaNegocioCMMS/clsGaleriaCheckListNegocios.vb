Imports CapaDatosCMMS

Public Class clsGaleriaCheckListNegocios
    Dim GaleriaCheckListMet As New clsGaleriaCheckListMetodos

    Public Function GaleriaCheckListGetData(strQuery As String) As DataTable
        Return GaleriaCheckListMet.GaleriaCheckListGetData(strQuery)
    End Function

    'Public Function GaleriaCheckListListarCombo() As List(Of LOGI_GALERIACHECKLISTORDENTRABAJO)
    '    Return GaleriaCheckListMet.GaleriaCheckListListarCombo()
    'End Function

    Public Function GaleriaCheckListInserta(ByVal GaleriaCheckList As LOGI_GALERIACHECKLISTORDENTRABAJO) As Int32
        If GaleriaCheckListMet.GaleriaCheckListExiste(GaleriaCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroSerieCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo, GaleriaCheckList.cIdEmpresa, GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo) = True Then
            Throw New Exception("La imagen con el id " & GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString & " ya existe!")
        Else
            Return GaleriaCheckListMet.GaleriaCheckListInserta(GaleriaCheckList)
        End If
    End Function

    Public Function GaleriaCheckListEdita(ByVal GaleriaCheckList As LOGI_GALERIACHECKLISTORDENTRABAJO) As Int32
        If GaleriaCheckListMet.GaleriaCheckListExiste(GaleriaCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroSerieCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo, GaleriaCheckList.cIdEmpresa, GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo) = True Then
            Throw New Exception("La imagen con el id " & GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString & " no existe!")
        Else
            Return GaleriaCheckListMet.GaleriaCheckListEdita(GaleriaCheckList)
        End If
    End Function

    Public Function GaleriaCheckListElimina(ByVal GaleriaCheckList As LOGI_GALERIACHECKLISTORDENTRABAJO)
        If GaleriaCheckListMet.GaleriaCheckListExiste(GaleriaCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroSerieCabeceraOrdenTrabajo, GaleriaCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo, GaleriaCheckList.cIdEmpresa, GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo) = True Then
            Throw New Exception("La imagen con el id " & GaleriaCheckList.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString & " no existe!")
        Else
            Return GaleriaCheckListMet.GaleriaCheckListElimina(GaleriaCheckList)
        End If
    End Function

    Public Function GaleriaCheckListListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_GALERIACHECKLISTORDENTRABAJO)
        Return GaleriaCheckListMet.GaleriaCheckListListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function GaleriaCheckListListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdNroItem As String) As LOGI_GALERIACHECKLISTORDENTRABAJO
        If GaleriaCheckListMet.GaleriaCheckListExiste(IdTipDoc, IdNroSer, IdNroDoc, IdEmpresa, IdNroItem) = True Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La imagen con el id " & IdNroItem.ToString & " no Existe!!!")
        Else
            Return GaleriaCheckListMet.GaleriaCheckListListarPorId(IdTipDoc, IdNroSer, IdNroDoc, IdEmpresa, IdNroItem)
        End If
    End Function
End Class
