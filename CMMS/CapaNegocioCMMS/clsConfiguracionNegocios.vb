Imports CapaDatosCMMS

Public Class clsConfiguracionNegocios
    Dim ConfiguracionMet As New clsConfiguracionMetodos

    Public Function ConfiguracionGetData(strQuery As String) As DataTable
        Return ConfiguracionMet.ConfiguracionGetData(strQuery)
    End Function

    Public Function ConfiguracionListarCombo() As List(Of GNRL_CONFIGURACION)
        Return ConfiguracionMet.ConfiguracionListarCombo
    End Function

    Public Function ConfiguracionInserta(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        If ConfiguracionMet.ConfiguracionExiste(Configuracion) = True Then
            Throw New Exception("La Configuración con el id " & Configuracion.cIdPerfil & " ya existe!")
        Else
            Return ConfiguracionMet.ConfiguracionInserta(Configuracion)
        End If
    End Function

    Public Function ConfiguracionEdita(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        If ConfiguracionMet.ConfiguracionExiste(Configuracion) = False Then
            Throw New Exception("La Configuración con el id " & Configuracion.cIdPerfil & " no existe!")
        Else
            Return ConfiguracionMet.ConfiguracionEdita(Configuracion)
        End If
    End Function

    Public Function ConfiguracionActualizarPerfil(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        Return ConfiguracionMet.ConfiguracionActualizarPerfil(Configuracion)
    End Function

    Public Function ConfiguracionElimina(ByVal Configuracion As GNRL_CONFIGURACION)
        If ConfiguracionMet.ConfiguracionExiste(Configuracion) = False Then
            Throw New Exception("La Configuración con el id " & Configuracion.cIdPerfil & " no existe!")
        Else
            Return ConfiguracionMet.ConfiguracionElimina(Configuracion)
        End If
    End Function

    'Public Function ConfiguracionEliminaRegistro(ByVal Configuracion As GNRL_CONFIGURACION, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String)
    Public Function ConfiguracionEliminaRegistro(ByVal Configuracion As GNRL_CONFIGURACION)
        Return ConfiguracionMet.ConfiguracionEliminaRegistro(Configuracion)
    End Function

    'Public Function ConfiguracionListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_GNRL_CONFIGURACION)
    '    Return ConfiguracionMet.ConfiguracionListaGrid(Filtro, Buscar, IdEmpresa, IdPuntoVenta)
    'End Function
    Public Function ConfiguracionListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdPaisOrigen As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_CONFIGURACION)
        Return ConfiguracionMet.ConfiguracionListaGrid(Filtro, Buscar, IdPaisOrigen, IdEmpresa, Estado)
    End Function

    Public Function ConfiguracionListarPorId(ByVal Configuracion As GNRL_CONFIGURACION) As GNRL_CONFIGURACION
        If ConfiguracionMet.ConfiguracionExiste(Configuracion) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Configuración con el id " & Configuracion.cIdPerfil & " no Existe!!!")
        Else
            Return ConfiguracionMet.ConfiguracionListarPorId(Configuracion)
        End If
    End Function
End Class
