Imports System.Data.SqlClient
Imports System.Configuration

Public Class clsEmpresaMetodos
    'Public Con As New SqlConnection(ConfigurationManager.cone)
    'MYDataContext mycontext = new MYDataContext("Your Connection String");
    'Dim Data As New BDSIWebGnrlDataContext(Session(""))
    'Dim Data As New BDSIWebGnrlDataContext(My.Settings.SIWConnectionString) 'Si aplicamos esta opción resetea los valores de la empresa es decir si tenemos dos empresas que se están logueando a la vez toma los valores de la primera y lo reemplaza la configuración por la segunda.
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function EmpresaGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        Dim constr As String = My.Settings.CMMSConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strQuery)
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    sda.Fill(dt)
                End Using
            End Using
            Return dt
        End Using
    End Function

    Public Function EmpresaListarCombo() As List(Of GNRL_EMPRESA)
        Dim Consulta = Data.PA_GNRL_MNT_EMPRESA("SQL_NONE", "SELECT * FROM GNRL_EMPRESA WHERE bEstadoRegistroEmpresa = 1", "''", "''", "", "1", "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", "0", "", "")
        Dim Coleccion As New List(Of GNRL_EMPRESA)
        For Each Configuracion In Consulta
            Dim Config As New GNRL_EMPRESA
            Config.cIdEmpresa = Configuracion.cIdEmpresa
            Config.vDescripcionEmpresa = Configuracion.vDescripcionEmpresa
            Coleccion.Add(Config)
        Next
        Return Coleccion
    End Function

    Public Function EmpresaPaisListarCombo(ByVal IdPais As String, ByVal Estado As String) As List(Of GNRL_EMPRESA)
        Dim Consulta = Data.PA_GNRL_MNT_EMPRESA("SQL_NONE", "SELECT * FROM GNRL_EMPRESA " &
                                                "WHERE cIdPaisUbicacionGeografica = '" & IdPais & "' " &
                                                "      AND (bEstadoRegistroEmpresa = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                                "", "", "", "1", "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", "0", "", "")
        Dim Coleccion As New List(Of GNRL_EMPRESA)
        For Each Configuracion In Consulta
            Dim Config As New GNRL_EMPRESA
            Config.cIdEmpresa = Configuracion.cIdEmpresa
            Config.vDescripcionEmpresa = Configuracion.vDescripcionEmpresa
            Coleccion.Add(Config)
        Next
        Return Coleccion
    End Function

    Public Function EmpresaListarPorId(ByVal IdEmpresa As String) As GNRL_EMPRESA
        Dim Consulta = Data.PA_GNRL_MNT_EMPRESA("SQL_NONE", "SELECT * FROM GNRL_EMPRESA WHERE cIdEmpresa = '" & IdEmpresa & "'", "", "", "", "1", "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", "0", "", "")
        Dim Coleccion As New GNRL_EMPRESA
        For Each GNRL_EMPRESA In Consulta
            Coleccion.cIdEmpresa = GNRL_EMPRESA.cIdEmpresa
            Coleccion.vDescripcionEmpresa = GNRL_EMPRESA.vDescripcionEmpresa
            Coleccion.vDescripcionAbreviadaEmpresa = GNRL_EMPRESA.vDescripcionAbreviadaEmpresa
            Coleccion.bEstadoRegistroEmpresa = GNRL_EMPRESA.bEstadoRegistroEmpresa
            Coleccion.vRucEmpresa = GNRL_EMPRESA.vRucEmpresa
            Coleccion.vDomicilioFiscalEmpresa = GNRL_EMPRESA.vDomicilioFiscalEmpresa
            Coleccion.vNombreComercialEmpresa = GNRL_EMPRESA.vNombreComercialEmpresa
            Coleccion.cIdPaisUbicacionGeografica = GNRL_EMPRESA.cIdPaisUbicacionGeografica
            Coleccion.cIdDepartamentoUbicacionGeografica = GNRL_EMPRESA.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = GNRL_EMPRESA.cIdProvinciaUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = GNRL_EMPRESA.cIdDistritoUbicacionGeografica
            Coleccion.vPaginaWebEmpresa = GNRL_EMPRESA.vPaginaWebEmpresa
            Coleccion.vTelefonoEmpresa = GNRL_EMPRESA.vTelefonoEmpresa
            Coleccion.vURLLogoEmpresa = GNRL_EMPRESA.vURLLogoEmpresa
            Coleccion.nIdConfiguracionListaPrecio = GNRL_EMPRESA.nIdConfiguracionListaPrecio
            Coleccion.vDescripcionTipoDocumentoEmpresa = GNRL_EMPRESA.vDescripcionTipoDocumentoEmpresa
            Coleccion.cIdTipoMonedaBaseEmpresa = GNRL_EMPRESA.cIdTipoMonedaBaseEmpresa
            Coleccion.nIdConfiguracionListaPrecio = GNRL_EMPRESA.nIdConfiguracionListaPrecio
            Coleccion.cIdTipoGeneracionCodProd = GNRL_EMPRESA.cIdTipoGeneracionCodProd
            Coleccion.cIdTipoCodigoProductoAUsar = GNRL_EMPRESA.cIdTipoCodigoProductoAUsar
            Coleccion.bPrincipalEmpresa = GNRL_EMPRESA.bPrincipalEmpresa
            Coleccion.cIdTipoEmpresa = GNRL_EMPRESA.cIdTipoEmpresa
        Next
        Return Coleccion
    End Function

    Public Function EmpresaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_GNRL_EMPRESA)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_EMPRESA("SQL_NONE", "SELECT * FROM GNRL_EMPRESA " &
                                                "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                "AND (bEstadoRegistroEmpresa = '" & Estado & "' OR '*' = '" & Estado & "')", "", "", "", "1", "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", "0", "", "")
        Dim Coleccion As New List(Of VI_GNRL_EMPRESA)
        For Each Busqueda In Consulta
            Dim BuscarEmp As New VI_GNRL_EMPRESA
            BuscarEmp.Codigo = Busqueda.cIdEmpresa
            BuscarEmp.Descripcion = Busqueda.vDescripcionEmpresa
            BuscarEmp.Estado = Busqueda.bEstadoRegistroEmpresa
            BuscarEmp.Principal = Busqueda.bPrincipalEmpresa
            Coleccion.Add(BuscarEmp)
        Next
        Return Coleccion
    End Function

    Public Function EmpresaInserta(ByVal Empresa As GNRL_EMPRESA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_EMPRESA("SQL_INSERT", "", Empresa.cIdEmpresa, Empresa.vDescripcionEmpresa, Empresa.vDescripcionAbreviadaEmpresa, Empresa.bEstadoRegistroEmpresa, Empresa.vRucEmpresa, Empresa.vDomicilioFiscalEmpresa, Empresa.vNombreComercialEmpresa, Empresa.cIdPaisUbicacionGeografica, Empresa.cIdDepartamentoUbicacionGeografica, Empresa.cIdProvinciaUbicacionGeografica, Empresa.cIdDistritoUbicacionGeografica, Empresa.vPaginaWebEmpresa, Empresa.vTelefonoEmpresa, Empresa.vURLLogoEmpresa, Empresa.nIdConfiguracionListaPrecio, Empresa.vDescripcionTipoDocumentoEmpresa, Empresa.cIdTipoMonedaBaseEmpresa, Empresa.cIdTipoGeneracionCodProd, Empresa.cIdTipoCodigoProductoAUsar, Empresa.bPrincipalEmpresa, Empresa.cIdTipoEmpresa, Empresa.cIdEmpresa).ReturnValue.ToString
        Return x
    End Function

    Public Function EmpresaEdita(ByVal Empresa As GNRL_EMPRESA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_EMPRESA("SQL_UPDATE", "", Empresa.cIdEmpresa, Empresa.vDescripcionEmpresa, Empresa.vDescripcionAbreviadaEmpresa, Empresa.bEstadoRegistroEmpresa, Empresa.vRucEmpresa, Empresa.vDomicilioFiscalEmpresa, Empresa.vNombreComercialEmpresa, Empresa.cIdPaisUbicacionGeografica, Empresa.cIdDepartamentoUbicacionGeografica, Empresa.cIdProvinciaUbicacionGeografica, Empresa.cIdDistritoUbicacionGeografica, Empresa.vPaginaWebEmpresa, Empresa.vTelefonoEmpresa, Empresa.vURLLogoEmpresa, Empresa.nIdConfiguracionListaPrecio, Empresa.vDescripcionTipoDocumentoEmpresa, Empresa.cIdTipoMonedaBaseEmpresa, Empresa.cIdTipoGeneracionCodProd, Empresa.cIdTipoCodigoProductoAUsar, Empresa.bPrincipalEmpresa, Empresa.cIdTipoEmpresa, Empresa.cIdEmpresa).ReturnValue.ToString
        Return x
    End Function

    Public Function EmpresaElimina(ByVal Empresa As GNRL_EMPRESA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_EMPRESA("SQL_NONE", "UPDATE GNRL_EMPRESA SET bEstadoRegistroEmpresa = 0 WHERE cIdEmpresa = '" & Empresa.cIdEmpresa & "'",
                                     "", "", "", "1", "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", "0", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function EmpresaExiste(ByVal IdEmpresa As String) As Boolean
        If Data.PA_GNRL_MNT_EMPRESA("SQL_NONE", "SELECT * FROM GNRL_EMPRESA WHERE cIdEmpresa = '" & IdEmpresa & "' and bEstadoRegistroEmpresa = 1", "", "", "", "1", "", "", "", "", "", "", "", "", "", "", 0, "", "", "", "", "0", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class