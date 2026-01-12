Imports CapaDatosCMMS

Public Class clsConexionNegocios
    Dim ConexionMet As New clsConexionDAO

    Function GetCnx() As String
        Try
            Return ConexionMet.GetCnx()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function CambiarCadenaConexion(ByVal strConexion As String) As String
        Try
            Return ConexionMet.CambiarCadenaConexion(strConexion)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function ObtenerDatosConfiguracionEmpresa() As String
        Try
            Return ConexionMet.ObtenerDatosConfiguracionEmpresa()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function ObtenerDatosConfiguracionPuntoVenta() As String
        Try
            Return ConexionMet.ObtenerDatosConfiguracionPuntoVenta()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function CambiarDatosConfiguracionEmpresa(ByVal IdEmpresa As String) As String
        Try
            Return ConexionMet.CambiarDatosConfiguracionEmpresa(IdEmpresa)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function CambiarDatosConfiguracionPuntoVenta(ByVal IdEmpresa As String) As String
        Try
            Return ConexionMet.CambiarDatosConfiguracionPuntoVenta(IdEmpresa)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

End Class