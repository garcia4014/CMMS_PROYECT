'Imports CapaDatosLogi
Imports CapaDatosCMMS

Public Class clsCatalogoNegocios
    Dim CatalogoMet As New clsCatalogoMetodos

    Public Function CatalogoGetData(strQuery As String) As DataTable
        Return CatalogoMet.CatalogoGetData(strQuery)
    End Function

    'Public Function CatalogoListarCombo(ByVal IdJerarquia As String, ByVal IdTipoActivo As String, Optional ByVal IdEnlaceCatalogo As String = "", Optional ByVal Estado As String = "*") As List(Of LOGI_CATALOGO)
    Public Function CatalogoListarDescripcionCombo() As List(Of LOGI_CATALOGO)
        Return CatalogoMet.CatalogoListarDescripcionCombo()
    End Function

    Public Function CatalogoListarCombo(ByVal IdJerarquia As String, ByVal IdTipoActivo As String, Optional ByVal IdEnlaceCatalogo As String = "", Optional ByVal Estado As String = "*") As List(Of LOGI_CATALOGO)
        'Public Function CatalogoListarCombo(ByVal IdJerarquia As String, Optional ByVal IdEnlaceCatalogo As String = "") As List(Of LOGI_CATALOGO)
        Return CatalogoMet.CatalogoListarCombo(IdJerarquia, IdTipoActivo, IdEnlaceCatalogo, Estado)
    End Function


    Public Function CatalogoInsertaDetalle(ByVal DetalleCatalogo As List(Of LOGI_CATALOGO), Optional ByVal strNroEnlaceCatalogo As String = "") As Int32
        'If CatalogoMet.CatalogoExiste(Catalogo.cIdCatalogo) = True Then
        'Throw New Exception("El Catalogo con el id " & Catalogo.cIdCatalogo & " ya existe!")
        'Else
        Return CatalogoMet.CatalogoInsertaDetalle(DetalleCatalogo, strNroEnlaceCatalogo)
        'End If
    End Function

    Public Function CatalogoInserta(ByVal Catalogo As LOGI_CATALOGO) As Int32
        If CatalogoMet.CatalogoExiste(Catalogo.cIdCatalogo, Catalogo.cIdTipoActivo, Catalogo.cIdJerarquiaCatalogo) = True Then
            Throw New Exception("El Catalogo con el id " & Catalogo.cIdCatalogo & " ya existe!")
        Else
            Return CatalogoMet.CatalogoInserta(Catalogo)
        End If
    End Function

    Public Function CatalogoEdita(ByVal Catalogo As LOGI_CATALOGO) As Int32
        If CatalogoMet.CatalogoExiste(Catalogo.cIdCatalogo, Catalogo.cIdTipoActivo, Catalogo.cIdJerarquiaCatalogo) = False Then
            Throw New Exception("El Catalogo con el id " & Catalogo.cIdCatalogo & " no existe!")
        Else
            Return CatalogoMet.CatalogoEdita(Catalogo)
        End If
    End Function

    Public Function CatalogoElimina(ByVal Catalogo As LOGI_CATALOGO) As Int32
        'If CatalogoMet.CatalogoExiste(Catalogo.cIdCatalogo, Catalogo.cIdEmpresa, Catalogo.cIdPuntoVenta) = False Then
        If CatalogoMet.CatalogoExiste(Catalogo.cIdCatalogo, Catalogo.cIdTipoActivo, Catalogo.cIdJerarquiaCatalogo) = False Then
            Throw New Exception("El Catalogo con el id " & Catalogo.cIdCatalogo & " no existe!")
        Else
            Return CatalogoMet.CatalogoElimina(Catalogo)
        End If
    End Function

    Public Function CatalogoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Jerarquia As String, Optional ByVal Estado As String = "1") As List(Of VI_LOGI_CATALOGO)
        'Public Function CatalogoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdCatalogo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdJerarquia As String) As List(Of VI_LOGI_CATALOGO)
        'Return CatalogoMet.CatalogoListaGrid(Filtro, Buscar, IdCatalogo, IdEmpresa, IdPuntoVenta, IdJerarquia)
        Return CatalogoMet.CatalogoListaGrid(Filtro, Buscar, Jerarquia, Estado)
    End Function

    Public Function CatalogoListarPorId(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal Jerarquia As String, Optional ByVal Estado As String = "1") As LOGI_CATALOGO
        If CatalogoMet.CatalogoExiste(IdCatalogo, IdTipoActivo, Jerarquia) = False Then
            'Si el Catalogo no existe lanzo una excepción.
            Throw New Exception("El Catalogo con el id " & IdCatalogo & " no Existe!!!")
        Else
            Return CatalogoMet.CatalogoListarPorId(IdCatalogo, IdTipoActivo, Jerarquia, Estado)
        End If
    End Function
End Class