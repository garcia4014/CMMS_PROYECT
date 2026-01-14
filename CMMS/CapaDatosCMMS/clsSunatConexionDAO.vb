Public Class clsSunatConexionDAO
    Function GetCnx() As String
        Dim strCnx As String
        Try
            strCnx = My.Settings.CMMSConnectionString
            'strCnx = ConfigurationManager.ConnectionStrings("SIWConnectionString").ConnectionString
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return strCnx
    End Function

    Function CambiarCadenaConexion(ByVal Conexion As String) As String
        My.Settings.Item("SIWConnectionString") = Conexion

        Return "Exito"
    End Function
End Class
