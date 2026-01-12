Imports CapaDatosCMMS

Public Class clsTablaSistemaNegocios
    Dim TablaSistemaMet As New clsTablaSistemaMetodos

    Public Function TablaSistemaGetData(strQuery As String) As DataTable
        Return TablaSistemaMet.TablaSistemaGetData(strQuery)
    End Function

    Public Function TablaSistemaListarCombo(ByVal IdTabla As String, ByVal IdSistema As String, ByVal IdEmpresa As String) As List(Of GNRL_TABLASISTEMA)
        Return TablaSistemaMet.TablaSistemaListarCombo(IdTabla, IdSistema, IdEmpresa)
    End Function

    Public Function TablaSistemaListarPorId(ByVal IdTabla As String, ByVal IdValor As String, ByVal IdSistema As String, ByVal IdEmpresa As String, Optional ByVal Periodo As String = "*") As GNRL_TABLASISTEMA
        If TablaSistemaMet.TablaSistemaExiste(IdTabla, IdValor, IdSistema, IdEmpresa, Periodo) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Tabla con el id " & IdTabla & " no Existe!!!")
        Else
            Return TablaSistemaMet.TablaSistemaListarPorId(IdTabla, IdValor, IdSistema, IdEmpresa, Periodo)
        End If
    End Function

    Public Function TablaSistemaCadenaATablaListarCombo(ByVal IdTabla As String, ByVal IdValor As String, ByVal IdEmpresa As String, Optional ByVal Periodo As String = "*") As List(Of String)
        Return TablaSistemaMet.TablaSistemaCadenaATablaListarCombo(IdTabla, IdValor, IdEmpresa, Periodo)
    End Function

    Public Function TablaSistemaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Periodo As String, ByVal IdSistema As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_GNRL_TABLASISTEMA)
        Return TablaSistemaMet.TablaSistemaListaGrid(Filtro, Buscar, Periodo, IdSistema, IdEmpresa, Estado)
    End Function

    Public Function TablaSistemaListaBusquedaPrincipal(ByVal Filtro As String, ByVal Buscar As String, ByVal Periodo As String, ByVal IdSistema As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_GNRL_TABLASISTEMA)
        Return TablaSistemaMet.TablaSistemaListaGridPrincipal(Filtro, Buscar, Periodo, IdSistema, IdEmpresa, Estado)
    End Function

    Public Function TablaSistemaInserta(ByVal TablaSistema As GNRL_TABLASISTEMA) As Int32
        If TablaSistemaMet.TablaSistemaExiste(TablaSistema.cIdTablaSistema, TablaSistema.vValor, TablaSistema.cIdSistema, TablaSistema.cIdEmpresa) = True Then
            Throw New Exception("El código con el id " & TablaSistema.cIdTablaSistema & " ya existe!")
        Else
            Return TablaSistemaMet.TablaSistemaInserta(TablaSistema)
        End If
    End Function

    Public Function TablaSistemaEdita(ByVal TablaSistema As GNRL_TABLASISTEMA) As Int32
        If TablaSistemaMet.TablaSistemaExiste(TablaSistema.cIdTablaSistema, TablaSistema.vValor, TablaSistema.cIdSistema, TablaSistema.cIdEmpresa) = False Then
            Throw New Exception("El código con el id " & TablaSistema.cIdTablaSistema & " no existe!")
        Else
            Return TablaSistemaMet.TablaSistemaEdita(TablaSistema)
        End If
    End Function

    Public Function TablaSistemaElimina(ByVal TablaSistema As GNRL_TABLASISTEMA)
        If TablaSistemaMet.TablaSistemaExiste(TablaSistema.cIdTablaSistema, TablaSistema.vValor, TablaSistema.cIdSistema, TablaSistema.cIdEmpresa) = False Then
            Throw New Exception("El código con el id " & TablaSistema.cIdTablaSistema & " no existe!")
        Else
            Return TablaSistemaMet.TablaSistemaElimina(TablaSistema)
        End If
    End Function
End Class
