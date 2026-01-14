Imports System.Data
Imports System.Data.SqlClient 'Para utilizar el SqlConnection
Imports System.Transactions

Public Class clsClienteMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function ClienteGetData(strQuery As String) As DataTable
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

    Public Function ClienteListarCombo() As List(Of GNRL_CLIENTE)
        Dim Consulta = Data.PA_GNRL_MNT_CLIENTE("SQL_NONE", "SELECT * FROM GNRL_CLIENTE WHERE bEstadoRegistroCliente = 1",
                                                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "", "", "0", "", Now, "", "", "", "", "", "", "", "", "")
        Dim Coleccion As New List(Of GNRL_CLIENTE)
        For Each Cliente In Consulta
            Dim Cli As New GNRL_CLIENTE
            Cli.cIdCliente = Cliente.cIdCliente
            Cli.vRazonSocialCliente = Cliente.vRazonSocialCliente
            Coleccion.Add(Cli)
        Next
        Return Coleccion
    End Function

    Public Function ClienteListarPorId(ByVal IdCliente As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As GNRL_CLIENTE
        'Dim Consulta = Data.pa_gnrl_mnt_Cliente("SQL_NONE", "SELECT * FROM GNRL_Cliente WHERE cIdCliente = '" & IdCliente & "'", "", "", "", "", "", "", "", "", "", "", "1", "")
        Dim Consulta = Data.PA_GNRL_MNT_CLIENTE("SQL_NONE", "SELECT * FROM GNRL_CLIENTE WHERE cIdCliente = '" & IdCliente & "'", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", IdPuntoVenta, IdEmpresa, "0", "", Now, "", "", "", "", "", "", "", "", "")
        Dim Coleccion As New GNRL_CLIENTE
        For Each GNRL_CLIENTE In Consulta
            Coleccion.cIdCliente = GNRL_CLIENTE.cIdCliente
            Coleccion.bEstadoRegistroCliente = GNRL_CLIENTE.bEstadoRegistroCliente
            Coleccion.cIdDepartamentoUbicacionGeografica = GNRL_CLIENTE.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = GNRL_CLIENTE.cIdDistritoUbicacionGeografica
            Coleccion.cIdPaisUbicacionGeografica = GNRL_CLIENTE.cIdPaisUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = GNRL_CLIENTE.cIdProvinciaUbicacionGeografica
            Coleccion.cIdTipoPersona = GNRL_CLIENTE.cIdTipoPersona
            Coleccion.vApellidoMaternoCliente = GNRL_CLIENTE.vApellidoMaternoCliente
            Coleccion.vApellidoPaternoCliente = GNRL_CLIENTE.vApellidoPaternoCliente
            Coleccion.vCelularCliente = GNRL_CLIENTE.vCelularCliente
            Coleccion.vDireccionCliente = GNRL_CLIENTE.vDireccionCliente
            Coleccion.vDniCliente = GNRL_CLIENTE.vDniCliente
            Coleccion.vEmailCliente = GNRL_CLIENTE.vEmailCliente
            Coleccion.vFaxCliente = GNRL_CLIENTE.vFaxCliente
            Coleccion.vNombresCliente = GNRL_CLIENTE.vNombresCliente
            Coleccion.vRazonSocialCliente = GNRL_CLIENTE.vRazonSocialCliente
            Coleccion.vRucCliente = GNRL_CLIENTE.vRucCliente
            Coleccion.vTelefonoCliente = GNRL_CLIENTE.vTelefonoCliente
            Coleccion.bEstadoAceptanteCliente = GNRL_CLIENTE.bEstadoAceptanteCliente
            Coleccion.cGeneroCliente = GNRL_CLIENTE.cGeneroCliente
            Coleccion.dFechaNacimientoCliente = GNRL_CLIENTE.dFechaNacimientoCliente
            Coleccion.cIdEstadoCivil = GNRL_CLIENTE.cIdEstadoCivil
            Coleccion.cIdTipoDocumento = GNRL_CLIENTE.cIdTipoDocumento
            Coleccion.vRepresentanteLegalCliente = GNRL_CLIENTE.vRepresentanteLegalCliente
            Coleccion.cIdTipoCliente = GNRL_CLIENTE.cIdTipoCliente
            Coleccion.vTipoEmpresaCliente = GNRL_CLIENTE.vTipoEmpresaCliente
            Coleccion.vEstadoCliente = GNRL_CLIENTE.vEstadoCliente
            Coleccion.vCondicionCliente = GNRL_CLIENTE.vCondicionCliente
            Coleccion.vIdClienteSAPCliente = GNRL_CLIENTE.vIdClienteSAPCliente
        Next
        Return Coleccion
    End Function

    Public Function ClienteListarPorIdSAP(ByVal IdCliente As String) As GNRL_CLIENTE
        'Dim Consulta = Data.pa_gnrl_mnt_Cliente("SQL_NONE", "SELECT * FROM GNRL_Cliente WHERE cIdCliente = '" & IdCliente & "'", "", "", "", "", "", "", "", "", "", "", "1", "")
        Dim IdPuntoVenta, IdEmpresa As String
        IdPuntoVenta = "*"
        IdEmpresa = "*"
        Dim Consulta = Data.PA_GNRL_MNT_CLIENTE("SQL_NONE", "SELECT * FROM GNRL_CLIENTE WHERE vIdClienteSAPCliente = '" & IdCliente & "'", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", IdPuntoVenta, IdEmpresa, "0", "", Now, "", "", "", "", "", "", "", "", "")
        Dim Coleccion As New GNRL_CLIENTE
        For Each GNRL_CLIENTE In Consulta
            Coleccion.cIdCliente = GNRL_CLIENTE.cIdCliente
            Coleccion.bEstadoRegistroCliente = GNRL_CLIENTE.bEstadoRegistroCliente
            Coleccion.cIdDepartamentoUbicacionGeografica = GNRL_CLIENTE.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = GNRL_CLIENTE.cIdDistritoUbicacionGeografica
            Coleccion.cIdPaisUbicacionGeografica = GNRL_CLIENTE.cIdPaisUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = GNRL_CLIENTE.cIdProvinciaUbicacionGeografica
            Coleccion.cIdTipoPersona = GNRL_CLIENTE.cIdTipoPersona
            Coleccion.vApellidoMaternoCliente = GNRL_CLIENTE.vApellidoMaternoCliente
            Coleccion.vApellidoPaternoCliente = GNRL_CLIENTE.vApellidoPaternoCliente
            Coleccion.vCelularCliente = GNRL_CLIENTE.vCelularCliente
            Coleccion.vDireccionCliente = GNRL_CLIENTE.vDireccionCliente
            Coleccion.vDniCliente = GNRL_CLIENTE.vDniCliente
            Coleccion.vEmailCliente = GNRL_CLIENTE.vEmailCliente
            Coleccion.vFaxCliente = GNRL_CLIENTE.vFaxCliente
            Coleccion.vNombresCliente = GNRL_CLIENTE.vNombresCliente
            Coleccion.vRazonSocialCliente = GNRL_CLIENTE.vRazonSocialCliente
            Coleccion.vRucCliente = GNRL_CLIENTE.vRucCliente
            Coleccion.vTelefonoCliente = GNRL_CLIENTE.vTelefonoCliente
            Coleccion.bEstadoAceptanteCliente = GNRL_CLIENTE.bEstadoAceptanteCliente
            Coleccion.cGeneroCliente = GNRL_CLIENTE.cGeneroCliente
            Coleccion.dFechaNacimientoCliente = GNRL_CLIENTE.dFechaNacimientoCliente
            Coleccion.cIdEstadoCivil = GNRL_CLIENTE.cIdEstadoCivil
            Coleccion.cIdTipoDocumento = GNRL_CLIENTE.cIdTipoDocumento
            Coleccion.vRepresentanteLegalCliente = GNRL_CLIENTE.vRepresentanteLegalCliente
            Coleccion.cIdTipoCliente = GNRL_CLIENTE.cIdTipoCliente
            Coleccion.vTipoEmpresaCliente = GNRL_CLIENTE.vTipoEmpresaCliente
            Coleccion.vEstadoCliente = GNRL_CLIENTE.vEstadoCliente
            Coleccion.vCondicionCliente = GNRL_CLIENTE.vCondicionCliente
            Coleccion.vIdClienteSAPCliente = GNRL_CLIENTE.vIdClienteSAPCliente
        Next
        Return Coleccion
    End Function

    Public Function ClienteListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, Optional ByVal booOrden As Boolean = True, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_CLIENTE)
        'Este si puede devolver una colección de datos es decir varios registros

        Dim Consulta = Data.PA_GNRL_MNT_CLIENTE("SQL_NONE", "SELECT * FROM GNRL_CLIENTE " &
                                                "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND cIdEmpresa = '" & IdEmpresa & "' AND (cIdPuntoVenta = '" & IdPuntoVenta & "' OR '*' = '" & IdPuntoVenta & "') " &
                                                "      AND (bEstadoRegistroCliente = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                IIf(booOrden = True, "ORDER BY " & Filtro, ""), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", IdPuntoVenta, IdEmpresa, "0", "", Now, "", "", "", "", "", "", "", "", "")
        Dim Coleccion As New List(Of VI_GNRL_CLIENTE)
        For Each Busqueda In Consulta
            Dim BuscarCli As New VI_GNRL_CLIENTE
            BuscarCli.Codigo = Busqueda.cIdCliente
            BuscarCli.Celular = Busqueda.vCelularCliente
            BuscarCli.Descripcion = Busqueda.vRazonSocialCliente
            BuscarCli.DNI = Busqueda.vDniCliente
            BuscarCli.Estado = Busqueda.bEstadoRegistroCliente
            BuscarCli.RUC = Busqueda.vRucCliente
            BuscarCli.Telefono = Busqueda.vTelefonoCliente
            BuscarCli.Tipo = Busqueda.cIdTipoPersona
            BuscarCli.Gen = Busqueda.cGeneroCliente
            BuscarCli.Est_Civil = Busqueda.cIdEstadoCivil
            BuscarCli.Direccion = Busqueda.vDireccionCliente
            BuscarCli.Tipo_Doc = Busqueda.cIdTipoDocumento
            Coleccion.Add(BuscarCli)
        Next
        Return Coleccion
    End Function

    Public Function ClienteInserta(ByVal Cliente As GNRL_CLIENTE) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CLIENTE("SQL_INSERT", "", Cliente.vRucCliente, Cliente.vDniCliente, Cliente.vRazonSocialCliente,
                                      Cliente.vApellidoPaternoCliente, Cliente.vApellidoMaternoCliente, Cliente.vNombresCliente,
                                      Cliente.vDireccionCliente, Cliente.vTelefonoCliente, Cliente.vCelularCliente, Cliente.vFaxCliente,
                                      Cliente.vEmailCliente, Cliente.cIdPaisUbicacionGeografica, Cliente.cIdDepartamentoUbicacionGeografica, Cliente.cIdProvinciaUbicacionGeografica,
                                      Cliente.cIdDistritoUbicacionGeografica, Cliente.cIdTipoPersona, Cliente.bEstadoRegistroCliente, Cliente.cIdPuntoVenta,
                                      Cliente.cIdEmpresa, Cliente.bEstadoAceptanteCliente, Cliente.cGeneroCliente, Cliente.dFechaNacimientoCliente,
                                      Cliente.cIdEstadoCivil, Cliente.cIdTipoDocumento, Cliente.vRepresentanteLegalCliente, Cliente.cIdTipoCliente, Cliente.vTipoEmpresaCliente,
                                      Cliente.vEstadoCliente, Cliente.vCondicionCliente, Cliente.vIdClienteSAPCliente, Cliente.cIdCliente).ReturnValue.ToString
        Return x
    End Function

    Public Function ClienteEdita(ByVal Cliente As GNRL_CLIENTE) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CLIENTE("SQL_UPDATE", Cliente.cIdCliente, Cliente.vRucCliente, Cliente.vDniCliente,
                                     Cliente.vRazonSocialCliente, Cliente.vApellidoPaternoCliente, Cliente.vApellidoMaternoCliente,
                                     Cliente.vNombresCliente, Cliente.vDireccionCliente, Cliente.vTelefonoCliente, Cliente.vCelularCliente,
                                     Cliente.vFaxCliente, Cliente.vEmailCliente, Cliente.cIdPaisUbicacionGeografica, Cliente.cIdDepartamentoUbicacionGeografica,
                                     Cliente.cIdProvinciaUbicacionGeografica, Cliente.cIdDistritoUbicacionGeografica, Cliente.cIdTipoPersona, Cliente.bEstadoRegistroCliente,
                                     Cliente.cIdPuntoVenta, Cliente.cIdEmpresa, Cliente.bEstadoAceptanteCliente, Cliente.cGeneroCliente,
                                     Cliente.dFechaNacimientoCliente, Cliente.cIdEstadoCivil, Cliente.cIdTipoDocumento, Cliente.vRepresentanteLegalCliente, Cliente.cIdTipoCliente, Cliente.vTipoEmpresaCliente,
                                     Cliente.vEstadoCliente, Cliente.vCondicionCliente, Cliente.vIdClienteSAPCliente, Cliente.cIdCliente).ReturnValue.ToString
        Return x
    End Function

    Public Function ClienteElimina(ByVal Cliente As GNRL_CLIENTE) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CLIENTE("SQL_NONE", "UPDATE GNRL_CLIENTE SET bEstadoRegistroCliente = 0 WHERE cIdCliente = '" & Cliente.cIdCliente & "' AND cIdEmpresa = '" & Cliente.cIdEmpresa & "' AND cIdPuntoVenta = '" & Cliente.cIdPuntoVenta & "'",
                                     "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "", "", "0", "", Now, "", "", "", "", "", "", "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function ClienteExiste(ByVal IdCliente As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
        'If Data.pa_gnrl_mnt_Cliente("SQL_NONE", "SELECT * FROM GNRL_Cliente WHERE cIdCliente = '" & idCliente & "'", "", "", "", "", "", "", "", "", "", "", "1", "").Count > 0 Then
        If Data.PA_GNRL_MNT_CLIENTE("SQL_NONE", "SELECT * FROM GNRL_CLIENTE WHERE cIdCliente = '" & IdCliente & "' AND bEstadoRegistroCliente = 1", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", IdPuntoVenta, IdEmpresa, "0", "", Now, "", "", "", "", "", "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class