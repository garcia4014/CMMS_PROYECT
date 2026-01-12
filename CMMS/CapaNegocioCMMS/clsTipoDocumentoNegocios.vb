Imports CapaDatosCMMS

Public Class clsTipoDocumentoNegocios
    Dim TipoDocumentoMet As New clsTipoDocumentoMetodos

    Public Function TipoDocumentoInserta(ByVal TipoDocumento As GNRL_TIPODOCUMENTO) As Int32
        If TipoDocumentoMet.TipoDocumentoExiste(TipoDocumento.cIdTipoDocumento) = True Then
            Throw New Exception("El tipo de documento con el id " & TipoDocumento.cIdTipoDocumento & " ya existe!")
        Else
            Return TipoDocumentoMet.TipoDocumentoInserta(TipoDocumento)
        End If
    End Function

    Public Function TipoDocumentoEdita(ByVal TipoDocumento As GNRL_TIPODOCUMENTO) As Int32
        If TipoDocumentoMet.TipoDocumentoExiste(TipoDocumento.cIdTipoDocumento) = False Then
            Throw New Exception("El tipo de documento  con el id " & TipoDocumento.cIdTipoDocumento & " no existe!")
        Else
            Return TipoDocumentoMet.TipoDocumentoEdita(TipoDocumento)
        End If
    End Function

    Public Function TipoDocumentoElimina(ByVal TipoDocumento As GNRL_TIPODOCUMENTO)
        If TipoDocumentoMet.TipoDocumentoExiste(TipoDocumento.cIdTipoDocumento) = False Then
            Throw New Exception("El tipo de documento con el id " & TipoDocumento.cIdTipoDocumento & " no existe!")
        Else
            Return TipoDocumentoMet.TipoDocumentoElimina(TipoDocumento)
        End If
    End Function

    Public Function TipoDocumentoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_TIPODOCUMENTO)
        Return TipoDocumentoMet.TipoDocumentoListaGrid(Filtro, Buscar)
    End Function

    Public Function TipoDocumentoListarPorId(ByVal IdTipoDocumento As String) As GNRL_TIPODOCUMENTO
        If TipoDocumentoMet.TipoDocumentoExiste(IdTipoDocumento) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El tipo de documento con el id " & IdTipoDocumento & " no Existe!!!")
        Else
            Return TipoDocumentoMet.TipoDocumentoListarPorId(IdTipoDocumento)
        End If
    End Function

    'Public Function TipoDocumentoListar(Optional ByVal IdModulo As String = "*", Optional ByVal IdSistema As String = "*") As List(Of GNRL_TIPODOCUMENTO) 
    Public Function TipoDocumentoListar(Optional ByVal IdModulo As String = "*", Optional ByVal IdSistema As String = "*", Optional ByVal IdTipoDocumento As String = "*") As List(Of GNRL_TIPODOCUMENTO)
        Return TipoDocumentoMet.TipoDocumentoListarCombo(IdModulo, IdSistema, IdTipoDocumento)
    End Function
End Class