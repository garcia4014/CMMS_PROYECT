Imports System.Configuration
Imports System.Data.SqlClient

Public Class clsConexionDAO
    Public Function GetCnx() As String
        Dim strCnx As String
        Try
            'strCnx = ConfigurationManager.ConnectionStrings("SIWConnectionString").ConnectionString
            strCnx = My.Settings.CMMSConnectionString
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return strCnx
    End Function

    Function CambiarCadenaConexion(ByVal Conexion As String) As String
        'Function CambiarCadenaConexion(ByVal strSection As String, ByVal strKey As String, ByVal strConexion As String) As String
        ''Dim Settings As ConnectionStringSettings = _
        ''ConfigurationManager.ConnectionStrings("SIWConnectionString")

        ''If Not Settings Is Nothing Then
        ''    ' Retrieve the partial connection string.
        ''    Dim connectString As String = Settings.ConnectionString

        ''    ' Create a new SqlConnectionStringBuilder based on the
        ''    ' partial connection string retrieved from the config file.
        ''    Dim builder As New SqlConnectionStringBuilder(connectString)
        ''    builder.ConnectionString = Conexion
        ''    ' Supply the additional values.
        ''    'builder.DataSource = dataSource
        ''    'builder.UserID = userName
        ''    'builder.Password = userPassword

        ''    'Console.WriteLine("Modified: {0}", builder.ConnectionString)
        ''End If

        ''Este lo quite
        My.Settings.Item("SIWConnectionString") = Conexion
        'Dim config As System.Configuration.Configuration
        ''config = System.Configuration.ConfigurationManager.OpenExeConfiguration(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        'config = System.Configuration.ConfigurationManager.OpenExeConfiguration("C:\SIW_NEW\slnSIW2010\CapaDatosCtbl\app")
        'config.AppSettings.Settings.Add("SIWConnectionString" & strKey, strConexion)
        'config.Save()
        'ConfigurationManager.RefreshSection(strSection)
        ''BDNew.Connection = ConfigurationManager.AppSettings(strKey)
        ''Return ("Exito")



        'Dim config As System.Configuration.Configuration
        'Dim fileMap As New ExeConfigurationFileMap()
        'fileMap.ExeConfigFilename = "C:\SIW_NEW\slnSIW2010\CapaDatosGnrl\app.config"
        'config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)

        ''Adicionando una cadena de conexión.
        'Dim csSection _
        'As ConnectionStringsSection = _
        'config.ConnectionStrings
        ''csSection.ConnectionStrings.Add( _
        ''New ConnectionStringSettings("SIWConnectionString" & strKey, strConexion, "System.Data.SqlClient"))
        'csSection.ConnectionStrings.Add( _
        'New ConnectionStringSettings("CapaDatosGnrl.My.MySettings.SIWConnectionString" & strKey, strConexion, "System.Data.SqlClient"))

        ''Guardar el archivo de configuración.
        'config.Save(ConfigurationSaveMode.Modified)
        'config.Save()
        'ConfigurationManager.RefreshSection(strSection)

        'My.Settings.Item("SIWConnectionString" & strKey) = strConexion

        Return ("Exito")
    End Function

    Function ObtenerDatosConfiguracionEmpresa() As String
        Return ConfigurationManager.AppSettings("IdEmpresa")
    End Function

    Function ObtenerDatosConfiguracionPuntoVenta() As String
        Return ConfigurationManager.AppSettings("IdPuntoVenta")
    End Function

    Function CambiarDatosConfiguracionEmpresa(ByVal IdEmpresa As String) As String
        ConfigurationManager.AppSettings("IdEmpresa") = IdEmpresa
        Return IdEmpresa
    End Function

    Function CambiarDatosConfiguracionPuntoVenta(ByVal IdPuntoVenta As String) As String
        ConfigurationManager.AppSettings("IdPuntoVenta") = IdPuntoVenta
        Return IdPuntoVenta
    End Function
End Class
