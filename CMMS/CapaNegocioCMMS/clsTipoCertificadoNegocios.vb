Imports CapaDatosCMMS

Public Class clsTipoCertificadoNegocios
    Dim TipoCertificadoMet As New clsTipoCertificadoMetodos

    Public Function TipoCertificadoGetData(strQuery As String) As DataTable
        Return TipoCertificadoMet.TipoCertificadoGetData(strQuery)
    End Function

    Public Function TipoCertificadoListarCombo() As List(Of RRHH_TIPOCERTIFICADO)
        Return TipoCertificadoMet.TipoCertificadoListarCombo()
    End Function

    Public Function TipoCertificadoInserta(ByVal TipoCertificado As RRHH_TIPOCERTIFICADO) As Int32
        If TipoCertificadoMet.TipoCertificadoExiste(TipoCertificado.cIdTipoCertificado) = True Then
            Throw New Exception("El tipo de certificado con el id " & TipoCertificado.cIdTipoCertificado & " ya existe!")
        Else
            Return TipoCertificadoMet.TipoCertificadoInserta(TipoCertificado)
        End If
    End Function

    Public Function DetalleAsignarCertificadoInsertaDetalle(ByVal AsignarCertificado As List(Of RRHH_ASIGNARCERTIFICADO), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Return TipoCertificadoMet.DetalleAsignarCertificadoInsertaDetalle(AsignarCertificado, LogAuditoria)
    End Function

    Public Function TipoCertificadoEdita(ByVal TipoCertificado As RRHH_TIPOCERTIFICADO) As Int32
        If TipoCertificadoMet.TipoCertificadoExiste(TipoCertificado.cIdTipoCertificado) = False Then
            Throw New Exception("El tipo de certificado con el id " & TipoCertificado.cIdTipoCertificado & " no existe!")
        Else
            Return TipoCertificadoMet.TipoCertificadoEdita(TipoCertificado)
        End If
    End Function

    Public Function TipoCertificadoElimina(ByVal TipoCertificado As RRHH_TIPOCERTIFICADO)
        If TipoCertificadoMet.TipoCertificadoExiste(TipoCertificado.cIdTipoCertificado) = False Then
            Throw New Exception("El tipo de certificado con el id " & TipoCertificado.cIdTipoCertificado & " no existe!")
        Else
            Return TipoCertificadoMet.TipoCertificadoElimina(TipoCertificado)
        End If
    End Function

    Public Function TipoCertificadoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_RRHH_TIPOCERTIFICADO)
        Return TipoCertificadoMet.TipoCertificadoListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function TipoCertificadoListarPorId(ByVal IdTipoCertificado As String) As RRHH_TIPOCERTIFICADO
        If TipoCertificadoMet.TipoCertificadoExiste(IdTipoCertificado) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El tipo de certificado con el id " & IdTipoCertificado & " no Existe!!!")
        Else
            Return TipoCertificadoMet.TipoCertificadoListarPorId(IdTipoCertificado)
        End If
    End Function
End Class
