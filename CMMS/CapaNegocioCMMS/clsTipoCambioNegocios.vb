Imports CapaDatosCMMS
Public Class clsTipoCambioNegocios
    Dim TipoCambioMet As New clsTipoCambioMetodos

    'Public Function TipoCambioPeriodoListarCombo() As List(Of GNRL_TIPOCAMBIO)
    Public Function TipoCambioGetData(strQuery As String) As DataTable
        Return TipoCambioMet.TipoCambioGetData(strQuery)
    End Function

    Public Function TipoCambioPeriodoListarCombo() As List(Of Integer)
        Return TipoCambioMet.TipoCambioPeriodoListarCombo
    End Function

    Public Function TipoCambioInserta(ByVal TipoCambio As GNRL_TIPOCAMBIO) As Int32
        ' If TipoCambioMet.TipoCambioExiste(TipoCambio.dFechaHoraTipoCambio, TipoCambio.cIdTipoMoneda) = True Then
        If TipoCambioMet.TipoCambioExiste(TipoCambio.dFechaHoraTipoCambio, TipoCambio.cIdTipoMoneda, TipoCambio.cIdPaisOrigenTipoCambio) = True Then
            Throw New Exception("El tipo de cambio con fecha " & TipoCambio.dFechaHoraTipoCambio & " ya existe!")
        Else
            Return TipoCambioMet.TipoCambioInserta(TipoCambio)
        End If
    End Function

    Public Function TipoCambioEdita(ByVal TipoCambio As GNRL_TIPOCAMBIO) As Int32
        'If TipoCambioMet.TipoCambioExiste(TipoCambio.dFechaHoraTipoCambio, TipoCambio.cIdTipoMoneda) = False Then
        If TipoCambioMet.TipoCambioExiste(TipoCambio.dFechaHoraTipoCambio, TipoCambio.cIdTipoMoneda, TipoCambio.cIdPaisOrigenTipoCambio) = False Then
            Throw New Exception("El tipo de cambio con fecha " & TipoCambio.dFechaHoraTipoCambio & " no existe!")
        Else
            Return TipoCambioMet.TipoCambioEdita(TipoCambio)
        End If
    End Function

    Public Function TipoCambioElimina(ByVal TipoCambio As GNRL_TIPOCAMBIO)
        'If TipoCambioMet.TipoCambioExiste(TipoCambio.dFechaHoraTipoCambio, TipoCambio.cIdTipoMoneda) = False Then
        If TipoCambioMet.TipoCambioExiste(TipoCambio.dFechaHoraTipoCambio, TipoCambio.cIdTipoMoneda, TipoCambio.cIdPaisOrigenTipoCambio) = False Then
            Throw New Exception("El tipo de cambio con fecha " & String.Format("{0:dd/MM/yyyy}", TipoCambio.dFechaHoraTipoCambio) & " no existe!")
        Else
            Return TipoCambioMet.TipoCambioElimina(TipoCambio)
        End If
    End Function

    Public Function TipoCambioListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_GNRL_TIPOCAMBIO)
        Return TipoCambioMet.TipoCambioListaGrid(Filtro, Buscar, Estado)
    End Function

    'Public Function TipoCambioListarPorId(ByVal IdTipoCambio As DateTime, ByVal IdTipoMoneda As String) As GNRL_TIPOCAMBIO
    '    If TipoCambioMet.TipoCambioExiste(IdTipoCambio, IdTipoMoneda) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("El tipo de cambio con fecha " & String.Format("{0:dd/MM/yyyy}", IdTipoCambio) & " no Existe!!!")
    '    Else
    '        Return TipoCambioMet.TipoCambioListarPorId(IdTipoCambio, IdTipoMoneda)
    '    End If
    'End Function
    Public Function TipoCambioListarPorId(ByVal IdTipoCambio As DateTime, ByVal IdTipoMoneda As String, ByVal IdPaisOrigen As String) As GNRL_TIPOCAMBIO
        If TipoCambioMet.TipoCambioExiste(IdTipoCambio, IdTipoMoneda, IdPaisOrigen) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El tipo de cambio con fecha " & String.Format("{0:dd/MM/yyyy}", IdTipoCambio) & " no Existe!!!")
        Else
            Return TipoCambioMet.TipoCambioListarPorId(IdTipoCambio, IdTipoMoneda, IdPaisOrigen)
        End If
    End Function
End Class
