Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsContratoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Class AnioVigencia
        Public Property Anio As Integer
    End Class
    Public Class MesVigencia
        Public Property Years As Integer
        Public Property Months As Integer
        Public Property NombreMes As String
    End Class

    Public Function ContratoGetData(strQuery As String) As DataTable
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

    'Public Function ContratoListarCombo(ByVal IdTipoDoc As String) As List(Of LOGI_CABECERACONTRATO)
    '    Dim Consulta = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_CABECERACONTRATO " &
    '                                             "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipoDoc & "' ",
    '                                             "", "", "", "", Now, Now, Now, Now, "", "1", "")
    '    Dim Coleccion As New List(Of LOGI_CABECERACONTRATO)
    '    For Each Contrato In Consulta
    '        Dim OrdFab As New LOGI_CABECERACONTRATO
    '        OrdFab.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
    '        OrdFab.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
    '        Coleccion.Add(OrdFab)
    '    Next
    '    Return Coleccion
    'End Function
    Public Function ContratoListarCombo(ByVal bEstado As Boolean, ByVal cEstado As String) As List(Of LOGI_CABECERACONTRATO)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_CABECERACONTRATO " &
                                                 "WHERE bEstadoRegistroCabeceraContrato = '" & bEstado & "' " &
                                                 "      AND cEstadoCabeceraContrato IN (" & cEstado & ")",
                                                 "", "", "", "", Now, Now, Now, Now, "", "1", " ", "", "", Now, Now, "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERACONTRATO)
        For Each Contrato In Consulta
            Dim OrdFab As New LOGI_CABECERACONTRATO
            OrdFab.vIdNumeroCorrelativoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
            OrdFab.vDescripcionCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato + "-" + Contrato.vDescripcionCabeceraContrato
            Coleccion.Add(OrdFab)
        Next
        Return Coleccion
    End Function

    Public Function ContratoListarComboByCliente(ByVal bEstado As Boolean, ByVal cEstado As String, ByVal cIdCliente As String) As List(Of LOGI_CABECERACONTRATO)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_CABECERACONTRATO " &
                                                 "WHERE bEstadoRegistroCabeceraContrato = '" & bEstado & "' " &
                                                 "      AND cEstadoCabeceraContrato IN (" & cEstado & ")" &
                                                 " AND cIdCliente ='" & cIdCliente & "'",
                                                 "", "", "", "", Now, Now, Now, Now, "", "1", " ", "", "", Now, Now, "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERACONTRATO)
        For Each Contrato In Consulta
            Dim OrdFab As New LOGI_CABECERACONTRATO
            OrdFab.vIdNumeroCorrelativoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
            OrdFab.vDescripcionCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato + "-" + Contrato.vDescripcionCabeceraContrato
            Coleccion.Add(OrdFab)
        Next
        Return Coleccion
    End Function

    Public Function ContratoRecursosListarCombo(ByVal Contrato As LOGI_CABECERACONTRATO) As List(Of RRHH_PERSONAL)
        Dim dsRecursos = ContratoGetData("SELECT RECHUM.*, PER.vNombreCompletoPersonal FROM LOGI_CABECERACONTRATO AS RECHUM INNER JOIN RRHH_PERSONAL AS PER ON " &
                       "RECHUM.cIdPersonal = PER.cIdPersonal " &
                       "WHERE " &
                       "RECHUM.cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND RECHUM.vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND " &
                       "RECHUM.vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND RECHUM.cIdEmpresa = '" & Contrato.cIdEmpresa & "'")
        Dim Coleccion As New List(Of RRHH_PERSONAL)
        For Each Recursos In dsRecursos.Rows
            Dim RRHH As New RRHH_PERSONAL
            RRHH.cIdPersonal = Recursos("cIdPersonal")
            RRHH.vNombreCompletoPersonal = Recursos("vNombreCompletoPersonal")
            Coleccion.Add(RRHH)
        Next
        Return Coleccion
    End Function

    'Public Function ContratoListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERACONTRATO
    '    Dim Consulta = Data.PA_LOGI_MNT_CABECERAContrato("SQL_NONE", "SELECT * FROM LOGI_CABECERACONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
    '                                           " AND bEstadoRegistroCabeceraContrato = 1 " &
    '                                           "ORDER BY cIdCatalogo",
    '                                           "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "")
    '    Dim Coleccion As New LOGI_CABECERACONTRATO
    '    For Each LOGI_CABECERACONTRATO In Consulta
    '        Coleccion.cIdTipoDocumentoCabeceraContrato = LOGI_CABECERACONTRATO.cIdTipoDocumentoCabeceraContrato
    '        Coleccion.vIdNumeroSerieCabeceraContrato = LOGI_CABECERACONTRATO.vIdNumeroSerieCabeceraContrato
    '        Coleccion.vIdNumeroCorrelativoCabeceraContrato = LOGI_CABECERACONTRATO.vIdNumeroCorrelativoCabeceraContrato
    '        Coleccion.cIdEmpresa = LOGI_CABECERACONTRATO.cIdEmpresa
    '        Coleccion.cIdEquipoSAPCabeceraContrato = LOGI_CABECERACONTRATO.cIdEquipoSAPCabeceraContrato
    '        Coleccion.vIdArticuloSAPCabeceraContrato = LOGI_CABECERACONTRATO.vIdArticuloSAPCabeceraContrato
    '        Coleccion.dFechaTransaccionCabeceraContrato = LOGI_CABECERACONTRATO.dFechaTransaccionCabeceraContrato
    '        Coleccion.vIdClienteSAPCabeceraContrato = LOGI_CABECERACONTRATO.vIdClienteSAPCabeceraContrato
    '        Coleccion.cIdCliente = LOGI_CABECERACONTRATO.cIdCliente
    '        Coleccion.cIdEquipo = LOGI_CABECERACONTRATO.cIdEquipo
    '        Coleccion.vNumeroSerieEquipoCabeceraContrato = LOGI_CABECERACONTRATO.vNumeroSerieEquipoCabeceraContrato
    '        Coleccion.vOrdenVentaCabeceraContrato = LOGI_CABECERACONTRATO.vOrdenVentaCabeceraContrato
    '        Coleccion.vOrdenCompraCabeceraContrato = LOGI_CABECERACONTRATO.vOrdenCompraCabeceraContrato
    '        Coleccion.cEstadoCabeceraContrato = LOGI_CABECERACONTRATO.cEstadoCabeceraContrato
    '        Coleccion.bEstadoRegistroCabeceraContrato = IIf(LOGI_CABECERACONTRATO.bEstadoRegistroCabeceraContrato Is Nothing, False, LOGI_CABECERACONTRATO.bEstadoRegistroCabeceraContrato)
    '        Coleccion.dFechaUltimaModificacionCabeceraContrato = LOGI_CABECERACONTRATO.dFechaUltimaModificacionCabeceraContrato
    '        Coleccion.dFechaCreacionCabeceraContrato = LOGI_CABECERACONTRATO.dFechaCreacionCabeceraContrato
    '        Coleccion.cIdUsuarioUltimaModificacionCabeceraContrato = LOGI_CABECERACONTRATO.cIdUsuarioUltimaModificacionCabeceraContrato
    '        Coleccion.cIdUsuarioCreacionCabeceraContrato = LOGI_CABECERACONTRATO.cIdUsuarioCreacionCabeceraContrato
    '    Next
    '    Return Coleccion
    'End Function

    Public Function ContratoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERACONTRATO
        Dim Consulta = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_CABECERACONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                 "AND bEstadoRegistroCabeceraContrato = 1 ",
                                                 "", "", "", "", Now, Now, Now, Now, "", "1", "", "", "", Now, Now, "", "", "")
        Dim Coleccion As New LOGI_CABECERACONTRATO
        For Each LOGI_CABECERACONTRATO In Consulta
            Coleccion.cIdTipoDocumentoCabeceraContrato = LOGI_CABECERACONTRATO.cIdTipoDocumentoCabeceraContrato
            Coleccion.vIdNumeroSerieCabeceraContrato = LOGI_CABECERACONTRATO.vIdNumeroSerieCabeceraContrato
            Coleccion.vIdNumeroCorrelativoCabeceraContrato = LOGI_CABECERACONTRATO.vIdNumeroCorrelativoCabeceraContrato
            Coleccion.cIdEmpresa = LOGI_CABECERACONTRATO.cIdEmpresa
            Coleccion.dFechaTransaccionCabeceraContrato = LOGI_CABECERACONTRATO.dFechaTransaccionCabeceraContrato
            Coleccion.dFechaEmisionCabeceraContrato = LOGI_CABECERACONTRATO.dFechaEmisionCabeceraContrato
            Coleccion.dFechaVigenciaInicialCabeceraContrato = LOGI_CABECERACONTRATO.dFechaVigenciaInicialCabeceraContrato
            Coleccion.dFechaVigenciaFinalCabeceraContrato = LOGI_CABECERACONTRATO.dFechaVigenciaFinalCabeceraContrato
            Coleccion.cIdCliente = LOGI_CABECERACONTRATO.cIdCliente
            Coleccion.bEstadoRegistroCabeceraContrato = IIf(LOGI_CABECERACONTRATO.bEstadoRegistroCabeceraContrato Is Nothing, False, LOGI_CABECERACONTRATO.bEstadoRegistroCabeceraContrato)
            Coleccion.cEstadoCabeceraContrato = LOGI_CABECERACONTRATO.cEstadoCabeceraContrato
            Coleccion.vDescripcionCabeceraContrato = LOGI_CABECERACONTRATO.vDescripcionCabeceraContrato
            Coleccion.vNroLicitacionCabeceraContrato = LOGI_CABECERACONTRATO.vNroLicitacionCabeceraContrato
            Coleccion.dFechaCreacionCabeceraContrato = LOGI_CABECERACONTRATO.dFechaCreacionCabeceraContrato
            Coleccion.dFechaUltimaModificacionCabeceraContrato = LOGI_CABECERACONTRATO.dFechaUltimaModificacionCabeceraContrato
            Coleccion.cIdUsuarioCreacionCabeceraContrato = LOGI_CABECERACONTRATO.cIdUsuarioCreacionCabeceraContrato
            Coleccion.cIdUsuarioUltimaModificacionCabeceraContrato = LOGI_CABECERACONTRATO.cIdUsuarioUltimaModificacionCabeceraContrato
        Next
        Return Coleccion
    End Function

    Public Function ContratoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal SubFiltro As String, ByVal Estado As String) As List(Of VI_LOGI_CABECERACONTRATO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT DOC.cIdTipoDocumentoCabeceraContrato, DOC.vIdNumeroSerieCabeceraContrato, DOC.vIdNumeroCorrelativoCabeceraContrato, " &
        '                                           "DOC.cIdEmpresa, DOC.cIdEquipoSAPCabeceraContrato, DOC.vIdArticuloSAPCabeceraContrato, DOC.dFechaTransaccionCabeceraContrato, " &
        '                                           "CLI.vIdClienteSAPCliente, DOC.cIdClienteCabeceraContrato, DOC.cIdEquipoCabeceraContrato, EQU.vNumeroSerieEquipo, " &
        '                                           "DOC.vOrdenFabricacionReferenciaCabeceraContrato, DOC.cEstadoCabeceraContrato, DOC.dFechaEmisionCabeceraContrato, " &
        '                                           "DOC.bEstadoRegistroCabeceraContrato, " &
        '                                           "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo " &
        '                                           "FROM LOGI_CABECERACONTRATO AS DOC LEFT JOIN GNRL_CLIENTE AS CLI ON " &
        '                                           "     DOC.cIdClienteCabeceraContrato = CLI.cIdCliente AND DOC.cIdEmpresa = CLI.cIdEmpresa " &
        '                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
        '                                           "     DOC.cIdEquipoCabeceraContrato = EQU.cIdEquipo AND DOC.cIdEmpresa = EQU.cIdEmpresa " &
        '                                           "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND (DOC.bEstadoRegistroCabeceraContrato = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                           "      AND DOC.cIdEmpresa = '" & IdEmpresa & "' " &
        '                                           IIf(SubFiltro = "", "", SubFiltro) & " " &
        '                                           "ORDER BY DOC.cIdTipoDocumentoCabeceraContrato, DOC.vIdNumeroSerieCabeceraContrato, CONVERT(INT, DOC.vIdNumeroCorrelativoCabeceraContrato) DESC",
        '                                           "", "", "", "", Now, Now, Now, Now, "", "1", "")

        Dim Consulta = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT DOC.cIdTipoDocumentoCabeceraContrato, DOC.vIdNumeroSerieCabeceraContrato, DOC.vIdNumeroCorrelativoCabeceraContrato, " &
                                                   "DOC.cIdEmpresa, DOC.dFechaTransaccionCabeceraContrato, " &
                                                   "CLI.vIdClienteSAPCliente, DOC.cIdCliente, " &
                                                   "DOC.cEstadoCabeceraContrato, DOC.dFechaEmisionCabeceraContrato, " &
                                                   "DOC.bEstadoRegistroCabeceraContrato, " &
                                                   "CLI.vRazonSocialCliente, CLI.vRucCliente, DOC.vDescripcionCabeceraContrato " &
                                                   "FROM LOGI_CABECERACONTRATO AS DOC LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     DOC.cIdCliente = CLI.cIdCliente AND DOC.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND (DOC.bEstadoRegistroCabeceraContrato = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                   "      AND DOC.cIdEmpresa = '" & IdEmpresa & "' " &
                                                   IIf(SubFiltro = "", "", SubFiltro) & " " &
                                                   "ORDER BY DOC.cIdTipoDocumentoCabeceraContrato, DOC.vIdNumeroSerieCabeceraContrato, CONVERT(INT, DOC.vIdNumeroCorrelativoCabeceraContrato) DESC",
                                                   "", "", "", "", Now, Now, Now, Now, "", "1", "", "", "", Now, Now, "", "", "")

        Dim Coleccion As New List(Of VI_LOGI_CABECERACONTRATO)
        For Each Busqueda In Consulta
            Dim BuscarContrato As New VI_LOGI_CABECERACONTRATO
            BuscarContrato.IdTipoDocumento = Busqueda.cIdTipoDocumentoCabeceraContrato
            BuscarContrato.IdNumeroSerie = Busqueda.vIdNumeroSerieCabeceraContrato
            BuscarContrato.IdNumeroCorrelativo = Busqueda.vIdNumeroCorrelativoCabeceraContrato
            BuscarContrato.DescripcionCabeceraContrato = Busqueda.vDescripcionCabeceraContrato
            'BuscarContrato.IdEquipo = Busqueda.cIdEquipoCabeceraContrato
            BuscarContrato.IdCliente = Busqueda.cIdCliente
            BuscarContrato.RazonSocialCliente = Busqueda.vRazonSocialCliente
            BuscarContrato.Estado = Busqueda.bEstadoRegistroCabeceraContrato
            BuscarContrato.RucCliente = Busqueda.vRucCliente
            BuscarContrato.FechaEmision = Busqueda.dFechaEmisionCabeceraContrato
            BuscarContrato.StatusContrato = Busqueda.cEstadoCabeceraContrato
            Coleccion.Add(BuscarContrato)
        Next
        Return Coleccion
    End Function


    Public Function ContratoInsertaDetalleReferenciaxEquipo(ByVal cIdTipoDocumentoCabeceraContrato As String, ByVal vIdNumeroSerieCabeceraContrato As String, ByVal vIdNumeroCorrelativoCabeceraContrato As String, ByVal cIdEmpresa As String, ByVal cIdEquipoDetalleContrato As String, ByVal vDescripcionDetalleContrato As String) As String
        Dim x As String
        If ContratoGetData("SELECT COUNT(*) FROM LOGI_DETALLECONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & vIdNumeroSerieCabeceraContrato & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & cIdEmpresa & "' AND cIdEquipoDetalleContrato = '" & cIdEquipoDetalleContrato & "'").Rows(0).Item(0) = 0 Then

            Dim rows = ContratoGetData("SELECT COUNT(*) FROM LOGI_DETALLECONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & vIdNumeroSerieCabeceraContrato & "' AND cIdEmpresa = '" & cIdEmpresa & "'").Rows(0).Item(0)
            x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_INSERT", "", cIdTipoDocumentoCabeceraContrato, vIdNumeroSerieCabeceraContrato, vIdNumeroCorrelativoCabeceraContrato,
                                                               cIdEmpresa, rows + 1, cIdEquipoDetalleContrato,
                                                               vDescripcionDetalleContrato, 1, vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        End If
        Return x
    End Function

    'Public Function ContratoInsertaDetalle(ByVal Contrato As LOGI_CABECERACONTRATO, ByVal DetalleContrato As List(Of LOGI_DETALLECONTRATO), ByVal CheckListContrato As List(Of LOGI_CHECKLISTORDENTRABAJO), ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
    'Public Function ContratoInsertaDetalle(ByVal Contrato As LOGI_CABECERACONTRATO, ByVal DetalleContrato As List(Of LOGI_DETALLECONTRATO), ByVal PlanificacionContrato As List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO), ByVal OrdenesTrabajo As List(Of LOGI_CABECERAORDENTRABAJO), ByVal CheckListContrato As List(Of LOGI_CHECKLISTORDENTRABAJO), ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
    Public Function ContratoInsertaDetalle(ByVal Contrato As LOGI_CABECERACONTRATO, ByVal DetalleContrato As List(Of LOGI_DETALLECONTRATO), ByVal PlanificacionContrato As List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO), ByVal OrdenesTrabajo As List(Of LOGI_CABECERAORDENTRABAJO), ByVal RecursosHumanosContrato As DataTable) As Int32 ' ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
        Dim x
        'x = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato,
        '                                   Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa, Contrato.dFechaTransaccionCabeceraContrato,
        '                                   Contrato.dFechaEmisionCabeceraContrato, Contrato.dFechaVigenciaInicialCabeceraContrato, Contrato.dFechaVigenciaFinalCabeceraContrato,
        '                                   Contrato.cIdCliente, Contrato.bEstadoRegistroCabeceraContrato, Contrato.cEstadoCabeceraContrato, Contrato.vDescripcionCabeceraContrato,
        '                                   Contrato.vNroLicitacionCabeceraContrato, Contrato.dFechaUltimaModificacionCabeceraContrato, Contrato.dFechaCreacionCabeceraContrato,
        '                                   Contrato.cIdUsuarioUltimaModificacionCabeceraContrato, Contrato.cIdUsuarioCreacionCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            If PlanificacionContrato.Count > 0 Then
                x = ContratoGetData("DELETE LOGI_PLANIFICACIONEQUIPOCONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & Contrato.cIdEmpresa & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & PlanificacionContrato(0).cIdPeriodoMesPlanificacionEquipoContrato & "'")
            End If
            'x = ContratoGetData("DELETE LOGI_DETALLECONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & Contrato.cIdEmpresa & "'")
            For Each lstDetCon In DetalleContrato
                If ContratoGetData("SELECT COUNT(*) FROM LOGI_DETALLECONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & Contrato.cIdEmpresa & "' AND cIdEquipoDetalleContrato = '" & lstDetCon.cIdEquipoDetalleContrato & "'").Rows(0).Item(0) = 0 Then
                    x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
                                                     Contrato.cIdEmpresa, lstDetCon.nIdNumeroItemDetalleContrato, lstDetCon.cIdEquipoDetalleContrato,
                                                     lstDetCon.vDescripcionDetalleContrato, lstDetCon.bEstadoRegistroDetalleContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
                End If
                'Next
                For Each lstOrdTra In OrdenesTrabajo
                    If lstDetCon.cIdEmpresa = lstOrdTra.cIdEmpresa And
                       lstDetCon.cIdEquipoDetalleContrato = lstOrdTra.cIdEquipoCabeceraOrdenTrabajo Then
                        'If ContratoGetData("SELECT * FROM CABECERAORDENTRABAJO WHERE cIdEmpresa = '" & lstOrdTra. & "' AND cIdTipoDocumentoCabeceraOrdenTrabajo = '' AND vIdNumeroSerieCabeceraOrdenTrabajo = '' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = ''").Rows.Count = 0 Then
                        If lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = "" Then
                            x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_INSERT", "", lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.cIdEmpresa, lstOrdTra.cIdEquipoCabeceraOrdenTrabajo, lstOrdTra.vIdArticuloSAPCabeceraOrdenTrabajo, lstOrdTra.cIdEquipoSAPCabeceraOrdenTrabajo, lstOrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.dFechaTransaccionCabeceraOrdenTrabajo, lstOrdTra.dFechaEmisionCabeceraOrdenTrabajo, lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo, lstOrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo,
                                                                  lstOrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo, lstOrdTra.cIdClienteCabeceraOrdenTrabajo, lstOrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, lstOrdTra.cEstadoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.bEstadoRegistroCabeceraOrdenTrabajo, Contrato.cIdTipoDocumentoCabeceraContrato & "-" & Contrato.vIdNumeroSerieCabeceraContrato & "-" & Contrato.vIdNumeroCorrelativoCabeceraContrato,
                                                                  lstOrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo, lstOrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo, lstOrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, lstOrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.nPeriodicidadDiasCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                        Else
                            x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_UPDATE", "", lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.cIdEmpresa, lstOrdTra.cIdEquipoCabeceraOrdenTrabajo, lstOrdTra.vIdArticuloSAPCabeceraOrdenTrabajo, lstOrdTra.cIdEquipoSAPCabeceraOrdenTrabajo, lstOrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.dFechaTransaccionCabeceraOrdenTrabajo, lstOrdTra.dFechaEmisionCabeceraOrdenTrabajo, lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo, lstOrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo,
                                                                  lstOrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo, lstOrdTra.cIdClienteCabeceraOrdenTrabajo, lstOrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, lstOrdTra.cEstadoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.bEstadoRegistroCabeceraOrdenTrabajo, Contrato.cIdTipoDocumentoCabeceraContrato & "-" & Contrato.vIdNumeroSerieCabeceraContrato & "-" & Contrato.vIdNumeroCorrelativoCabeceraContrato,
                                                                  lstOrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo, lstOrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo, lstOrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, lstOrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo,
                                                                  lstOrdTra.nPeriodicidadDiasCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                        End If
                        'For Each lstPlanCon In PlanificacionContrato
                        '    If lstPlanCon.cIdEquipoDetalleContrato = lstDetCon.cIdEquipoDetalleContrato And lstPlanCon.nIdNumeroItemDetalleContrato = lstDetCon.nIdNumeroItemDetalleContrato And lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = lstPlanCon.dFechaHoraProgramacionPlanificacionEquipoContrato Then
                        '        x = Data.PA_LOGI_MNT_PLANIFICACIONEQUIPOCONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
                        '                                                 Contrato.cIdEmpresa, lstPlanCon.nIdNumeroItemDetalleContrato, lstPlanCon.cIdEquipoDetalleContrato, lstPlanCon.cIdPeriodoMesPlanicacionesEquipoContrato,
                        '                                                 lstPlanCon.dFechaHoraProgramacionPlanificacionEquipoContrato, lstPlanCon.cIdTipoMantenimientoPlanificacionEquipoContrato,
                        '                                                 lstPlanCon.vOrdenTrabajoReferenciaPlanificacionEquipoContrato, lstPlanCon.vOrdenTrabajoClientePlanificacionEquipoContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
                        '    End If
                        'Next
                        For Each lstPlanCon In PlanificacionContrato
                            'JMUG: 21/08/2023 If lstPlanCon.nIdNumeroItemDetalleContrato = lstDetCon.nIdNumeroItemDetalleContrato And lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = lstPlanCon.dFechaHoraProgramacionPlanificacionEquipoContrato Then
                            If lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = lstPlanCon.dFechaHoraProgramacionPlanificacionEquipoContrato Then
                                lstPlanCon.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo + "-" + lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo + "-" + lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                                x = Data.PA_LOGI_MNT_PLANIFICACIONEQUIPOCONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
                                                                             Contrato.cIdEmpresa, lstPlanCon.nIdNumeroItemDetalleContrato, lstPlanCon.cIdEquipoDetalleContrato, lstPlanCon.cIdPeriodoMesPlanificacionEquipoContrato,
                                                                             lstPlanCon.dFechaHoraProgramacionPlanificacionEquipoContrato, lstPlanCon.cIdTipoMantenimientoPlanificacionEquipoContrato,
                                                                             lstPlanCon.vOrdenTrabajoReferenciaPlanificacionEquipoContrato, lstPlanCon.vOrdenTrabajoClientePlanificacionEquipoContrato, lstPlanCon.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato,
                                                                             Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString

                                'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(DetPlanificacion.cIdEquipoDetalleContrato)
                                Dim EquipoMet As New clsEquipoMetodos
                                Dim Equipo As LOGI_EQUIPO = EquipoMet.EquipoListarPorId(lstPlanCon.cIdEquipoDetalleContrato)
                                Dim dsCheckListPlantilla = ContratoGetData("SELECT CABCHKLISPLA.cIdTipoMantenimiento, CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, CABCHKLISPLA.dFechaTransaccionCabeceraCheckListPlantilla, " &
                                                                   "       CABCHKLISPLA.bEstadoRegistroCabeceraCheckListPlantilla, CABCHKLISPLA.cIdTipoActivoCabeceraCheckListPlantilla, CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla, " &
                                                                   "       CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, DETCHKLISPLA.cIdActividadCheckList, DETCHKLISPLA.vDescripcionDetalleCheckListPlantilla, " &
                                                                   "       DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla, DETCHKLISPLA.cIdTipoActivo, DETCHKLISPLA.cIdCatalogo, DETCHKLISPLA.cIdJerarquiaCatalogo, EQU.cIdEquipo " &
                                                                   "FROM LOGI_CABECERACHECKLISTPLANTILLA AS CABCHKLISPLA INNER JOIN LOGI_DETALLECHECKLISTPLANTILLA AS DETCHKLISPLA ON " &
                                                                   "     CABCHKLISPLA.cIdTipoMantenimiento = DETCHKLISPLA.cIdTipoMantenimiento AND " &
                                                                   "     CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla " &
                                                                   "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                                   "     DETCHKLISPLA.cIdCatalogo = EQU.cIdCatalogo AND " &
                                                                   "     DETCHKLISPLA.cIdJerarquiaCatalogo = EQU.cIdJerarquiaCatalogo AND " &
                                                                   "     EQU.cIdEnlaceEquipo = '" & lstPlanCon.cIdEquipoDetalleContrato & "' " &
                                                                   "WHERE CABCHKLISPLA.cIdTipoMantenimiento = '" & lstPlanCon.cIdTipoMantenimientoPlanificacionEquipoContrato & "' AND CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & lstPlanCon.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato & "' " &
                                                                   "      AND CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla = '" & Equipo.cIdCatalogo & "' AND CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0' " &
                                                                   "      AND DETCHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '1' " &
                                                                   "ORDER BY DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla")
                                Dim ColeccionCheckList As New List(Of LOGI_CHECKLISTORDENTRABAJO)
                                For Each CheckListPlantilla In dsCheckListPlantilla.Rows
                                    Dim ChkList As New LOGI_CHECKLISTORDENTRABAJO
                                    With ChkList
                                        .cIdTipoDocumentoCabeceraOrdenTrabajo = lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                                        .vIdNumeroSerieCabeceraOrdenTrabajo = lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                                        .vIdNumeroCorrelativoCabeceraOrdenTrabajo = lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                                        .vObservacionCheckListOrdenTrabajo = ""
                                        .cIdEmpresa = lstOrdTra.cIdEmpresa
                                        .cEstadoCheckListOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                                        .nIdNumeroItemCheckListOrdenTrabajo = CheckListPlantilla("nIdNumeroItemDetalleCheckListPlantilla")
                                        .nTotalSegundosTrabajadosCheckListOrdenTrabajo = 0
                                        .dFechaInicioCheckListOrdenTrabajo = Nothing
                                        .dFechaFinalCheckListOrdenTrabajo = Nothing
                                        .cIdEquipoCabeceraOrdenTrabajo = lstPlanCon.cIdEquipoDetalleContrato '(Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim)
                                        .cIdEquipoCheckListOrdenTrabajo = CheckListPlantilla("cIdEquipo")
                                        .cIdActividadCheckListOrdenTrabajo = CheckListPlantilla("cIdActividadCheckList")
                                        .cIdTipoMantenimientoCheckListOrdenTrabajo = CheckListPlantilla("cIdTipoMantenimiento")
                                        .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")
                                        .cIdCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdCatalogo")
                                        .cIdJerarquiaCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdJerarquiaCatalogo")
                                        .vIdArticuloSAPCabeceraOrdenTrabajo = lstOrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                                        .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = lstPlanCon.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato 'cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue
                                    End With
                                    ColeccionCheckList.Add(ChkList)
                                Next

                                For Each OTCheckList In ColeccionCheckList
                                    'If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WITH (NOLOCK) " &
                                    '                                                     "WHERE " &
                                    '                                                     "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND " &
                                    '                                                     "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & OTCheckList.cIdEmpresa & "' AND " &
                                    '                                                     "cIdEquipoCabeceraOrdenTrabajo = '" & OTCheckList.cIdEquipoCabeceraOrdenTrabajo & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & OTCheckList.cIdCatalogoCheckListOrdenTrabajo & "' AND " &
                                    '                                                     "cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & OTCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "' AND cIdActividadCheckListOrdenTrabajo = '" & OTCheckList.cIdActividadCheckListOrdenTrabajo & "' AND " &
                                    '                                                     "vIdArticuloSAPCabeceraOrdenTrabajo = '" & OTCheckList.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                                    '                                                     "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "").Count = 0 Then
                                    If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WITH (NOLOCK) " &
                                                                                         "WHERE " &
                                                                                         "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OTCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OTCheckList.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                                                                         "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OTCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & OTCheckList.cIdEmpresa & "' AND " &
                                                                                         "cIdEquipoCabeceraOrdenTrabajo = '" & OTCheckList.cIdEquipoCabeceraOrdenTrabajo & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & OTCheckList.cIdCatalogoCheckListOrdenTrabajo & "' AND " &
                                                                                         "cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & OTCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "' AND cIdActividadCheckListOrdenTrabajo = '" & OTCheckList.cIdActividadCheckListOrdenTrabajo & "' AND " &
                                                                                         "vIdArticuloSAPCabeceraOrdenTrabajo = '" & OTCheckList.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                                                                                         "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count = 0 Then
                                        'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
                                        x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_INSERT", "", lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                                           OTCheckList.cIdEmpresa, OTCheckList.cIdEquipoCabeceraOrdenTrabajo, OTCheckList.cIdEquipoCheckListOrdenTrabajo, OTCheckList.cIdCatalogoCheckListOrdenTrabajo, OTCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                                                           OTCheckList.cIdActividadCheckListOrdenTrabajo, OTCheckList.vIdArticuloSAPCabeceraOrdenTrabajo, OTCheckList.nIdNumeroItemCheckListOrdenTrabajo,
                                                                                           OTCheckList.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, OTCheckList.cIdTipoMantenimientoCheckListOrdenTrabajo, OTCheckList.vObservacionCheckListOrdenTrabajo,
                                                                                           OTCheckList.cEstadoCheckListOrdenTrabajo, OTCheckList.dFechaInicioCheckListOrdenTrabajo, OTCheckList.dFechaFinalCheckListOrdenTrabajo, OTCheckList.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                                                           OTCheckList.cEstadoActividadCheckListOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                                    Else
                                        x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_UPDATE", "", lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                                   OTCheckList.cIdEmpresa, OTCheckList.cIdEquipoCabeceraOrdenTrabajo, OTCheckList.cIdEquipoCheckListOrdenTrabajo, OTCheckList.cIdCatalogoCheckListOrdenTrabajo, OTCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                                                   OTCheckList.cIdActividadCheckListOrdenTrabajo, OTCheckList.vIdArticuloSAPCabeceraOrdenTrabajo, OTCheckList.nIdNumeroItemCheckListOrdenTrabajo,
                                                                                   OTCheckList.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, OTCheckList.cIdTipoMantenimientoCheckListOrdenTrabajo, OTCheckList.vObservacionCheckListOrdenTrabajo,
                                                                                   OTCheckList.cEstadoCheckListOrdenTrabajo, OTCheckList.dFechaInicioCheckListOrdenTrabajo, OTCheckList.dFechaFinalCheckListOrdenTrabajo, OTCheckList.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                                                   OTCheckList.cEstadoActividadCheckListOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                                    End If
                                Next

                                'For Each lstRRHHContrato In RecursosHumanosContrato
                                For Each lstRRHHContrato In RecursosHumanosContrato.Rows
                                    'If Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_RECURSOSORDENTRABAJO WITH (NOLOCK) " &
                                    '                       "WHERE " &
                                    '                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                    '                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & lstRRHHContrato.cIdEmpresa & "' AND " &
                                    '                       "cIdPersonal = '" & lstRRHHContrato.("IdPersonal") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & lstRRHHContrato("IdEquipo") & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & lstRRHHContrato.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                                    '                       "", "", "", "", Now, Now, Now, Now, "", "1", " ", "", "", Now, Now, "", "", "").Count = 0 Then
                                    If Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT ROT.* FROM LOGI_CABECERAORDENTRABAJO AS COT WITH (NOLOCK) INNER JOIN LOGI_RECURSOSORDENTRABAJO AS ROT WITH (NOLOCK) ON " &
                                                           "COT.cIdTipoDocumentoCabeceraOrdenTrabajo = ROT.cIdTipoDocumentoCabeceraOrdenTrabajo AND " &
                                                           "COT.vIdNumeroSerieCabeceraOrdenTrabajo = ROT.vIdNumeroSerieCabeceraOrdenTrabajo AND " &
                                                           "COT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ROT.vIdNumeroCorrelativoCabeceraOrdenTrabajo AND " &
                                                           "COT.cIdEmpresa = ROT.cIdEmpresa AND COT.cIdEquipoCabeceraOrdenTrabajo = ROT.cIdEquipoCabeceraOrdenTrabajo " &
                                                           "WHERE " &
                                                           "ROT.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND ROT.vIdNumeroSerieCabeceraOrdenTrabajo = '" & lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                                           "ROT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND ROT.cIdEmpresa = '" & lstOrdTra.cIdEmpresa & "' AND " &
                                                           "ROT.cIdPersonal = '" & lstRRHHContrato("IdPersonal") & "' AND ROT.cIdEquipoCabeceraOrdenTrabajo = '" & lstRRHHContrato("IdEquipo") & "' AND " &
                                                           "CONVERT(CHAR(8), COT.dFechaInicioPlanificadaCabeceraOrdenTrabajo, 112) = '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(lstRRHHContrato("FechaPlanificacion"))) & "'",
                                                           "", "", "", "", Now, Now, Now, Now, "", "1", " ", "", "", Now, Now, "", "", "").Count = 0 Then
                                        If String.Format("{0:yyyyMMdd}", Convert.ToDateTime(lstRRHHContrato("FechaPlanificacion"))) = String.Format("{0:yyyyMMdd}", lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo) And
                                                lstRRHHContrato("IdEquipo") = lstOrdTra.cIdEquipoCabeceraOrdenTrabajo Then
                                            'x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_INSERT", "", lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                            '                                      lstOrdTra.cIdEmpresa, lstRRHHContrato("IdPersonal"), "", "1",
                                            '                                      lstRRHHContrato("IdEquipo"), "0", "",
                                            '                                      IIf(CBool(lstRRHHContrato("Responsable")) = True, "1", "0"), lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                                            x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_INSERT", "", lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                                  lstOrdTra.cIdEmpresa, lstRRHHContrato("IdPersonal"), "", "1",
                                                                                  lstRRHHContrato("IdEquipo"), "0", "",
                                                                                  lstRRHHContrato("Responsable"), lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                                        End If
                                    Else
                                        If String.Format("{0:yyyyMM}", Convert.ToDateTime(lstRRHHContrato("FechaPlanificacion"))) = String.Format("{0:yyyyMM}", lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo) And
                                                lstRRHHContrato("IdEquipo") = lstOrdTra.cIdEquipoCabeceraOrdenTrabajo Then
                                            'x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_UPDATE", "", lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo, lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                            '                                      lstOrdTra.cIdEmpresa, lstRRHHContrato("IdPersonal"), "", "1",
                                            '                                      lstRRHHContrato("IdEquipo"), lstRRHHContrato("TotalMinutosTrabajados"), "",
                                            '                                      IIf(CBool(lstRRHHContrato("Responsable")) = True, "1", "0"), lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                                            ContratoGetData("UPDATE LOGI_RECURSOSORDENTRABAJO SET bResponsableRecursosOrdenTrabajo = '" & IIf(CBool(lstRRHHContrato("Responsable")) = True, "1", "0") & "' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & lstOrdTra.cIdEmpresa & "' AND cIdPersonal = '" & lstRRHHContrato("IdPersonal") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & lstRRHHContrato("IdEquipo") & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = ''")
                                        End If
                                    End If
                                Next
                            End If
                        Next





                    End If
                    'Next
                    'For Each lstPlanCon In PlanificacionContrato
                    'x = Data.PA_LOGI_MNT_PLANIFICACIONEQUIPOCONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
                    '                                                 Contrato.cIdEmpresa, lstPlanCon.nIdNumeroItemDetalleContrato, lstPlanCon.cIdEquipoDetalleContrato, lstPlanCon.cIdPeriodoMesPlanicacionesEquipoContrato,
                    '                                                 lstPlanCon.dFechaHoraProgramacionPlanificacionEquipoContrato, lstPlanCon.cIdTipoMantenimientoPlanificacionEquipoContrato,
                    '                                                 lstPlanCon.vOrdenTrabajoReferenciaPlanificacionEquipoContrato, lstPlanCon.vOrdenTrabajoClientePlanificacionEquipoContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato)
                    'x = Data.PA_LOGI_MNT_PLANIFICACIONEQUIPOCONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
                    '                                                 Contrato.cIdEmpresa, lstDetCon.nIdNumeroItemDetalleContrato, lstDetCon.cIdEquipoDetalleContrato, Format("yyyyMM", lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo), 'lstDetCon.cIdPeriodoMesPlanicacionesEquipoContrato,
                    '                                                 lstOrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo, lstOrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo,
                    '                                                 lstOrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "-" & lstOrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "-" & lstOrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                    '                                                 lstPlanCon.vOrdenTrabajoClientePlanificacionEquipoContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato)
                Next
            Next
            scope.Complete()
            Return x
        End Using
        '    For Each Busqueda In DetalleContrato
        '        If Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_DETALLECONTRATO WITH (NOLOCK) " &
        '                               "WHERE " &
        '                               "cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND " &
        '                               "vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & Busqueda.cIdEmpresa & "' AND cIdEquipoCabeceraContrato = '" & Busqueda.cIdEquipoCabeceraContrato & "' AND " &
        '                               "cIdCatalogoCheckListDetalleContrato = '" & Busqueda.cIdCatalogoCheckListDetalleContrato & "' AND " &
        '                               "cIdJerarquiaCatalogoCheckListDetalleContrato = '" & Busqueda.cIdJerarquiaCatalogoCheckListDetalleContrato & "' AND cIdActividadCheckListDetalleContrato = '" & Busqueda.cIdActividadCheckListDetalleContrato & "' AND " &
        '                               "vIdArticuloSAPCabeceraContrato = '" & Busqueda.vIdArticuloSAPCabeceraContrato & "'",
        '                               "", "", "", "", Now, Now, Now, Now, "", "1", "").Count = 0 Then
        '            'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
        '            x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
        '                                                     Busqueda.cIdEmpresa, Busqueda.nIdNumeroItemDetalleContrato, Busqueda.vIdArticuloSAPCabeceraContrato,
        '                                                     Busqueda.cIdEquipoCabeceraContrato, Busqueda.vIdArticuloSAPDetalleContrato, Busqueda.vDescripcionArticuloDetalleContrato,
        '                                                     Busqueda.nCantidadArticuloDetalleContrato, Busqueda.vDescripcionUnidadMedidaDetalleContrato, Busqueda.cIdCatalogoCheckListDetalleContrato,
        '                                                     Busqueda.cIdJerarquiaCatalogoCheckListDetalleContrato, Busqueda.cIdActividadCheckListDetalleContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        '        Else
        '            x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_UPDATE", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
        '                                                     Busqueda.cIdEmpresa, Busqueda.nIdNumeroItemDetalleContrato, Busqueda.vIdArticuloSAPCabeceraContrato,
        '                                                     Busqueda.cIdEquipoCabeceraContrato, Busqueda.vIdArticuloSAPDetalleContrato, Busqueda.vDescripcionArticuloDetalleContrato,
        '                                                     Busqueda.nCantidadArticuloDetalleContrato, Busqueda.vDescripcionUnidadMedidaDetalleContrato, Busqueda.cIdCatalogoCheckListDetalleContrato,
        '                                                     Busqueda.cIdJerarquiaCatalogoCheckListDetalleContrato, Busqueda.cIdActividadCheckListDetalleContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString

        '        End If
        '    Next
        '    For Each Busqueda1 In CheckListContrato
        '        If Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTContrato WITH (NOLOCK) " &
        '                                               "WHERE " &
        '                                               "cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND " &
        '                                               "vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & Busqueda1.cIdEmpresa & "' AND " &
        '                                               "cIdEquipoCabeceraContrato = '" & Busqueda1.cIdEquipoCabeceraContrato & "' AND cIdCatalogoCheckListContrato = '" & Busqueda1.cIdCatalogoCheckListContrato & "' AND " &
        '                                               "cIdJerarquiaCatalogoCheckListContrato = '" & Busqueda1.cIdJerarquiaCatalogoCheckListContrato & "' AND cIdActividadCheckListContrato = '" & Busqueda1.cIdActividadCheckListContrato & "' AND " &
        '                                               "vIdArticuloSAPCabeceraContrato = '" & Busqueda1.vIdArticuloSAPCabeceraContrato & "'",
        '                                               "", "", "", "", Now, Now, Now, Now, "", "1", "").Count = 0 Then
        '            'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
        '            x = Data.PA_LOGI_MNT_CHECKLISTContrato("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
        '                                                           Busqueda1.cIdEmpresa, Busqueda1.cIdEquipoCabeceraContrato, Busqueda1.cIdEquipoCheckListContrato, Busqueda1.cIdCatalogoCheckListContrato, Busqueda1.cIdJerarquiaCatalogoCheckListContrato,
        '                                                           Busqueda1.cIdActividadCheckListContrato, Busqueda1.vIdArticuloSAPCabeceraContrato, Busqueda1.nIdNumeroItemCheckListContrato,
        '                                                           Busqueda1.cIdNumeroCabeceraCheckListPlantillaCheckListContrato, Busqueda1.cIdTipoMantenimientoCheckListContrato, Busqueda1.vObservacionCheckListContrato,
        '                                                           Busqueda1.cEstadoCheckListContrato, Busqueda1.dFechaInicioCheckListContrato, Busqueda1.dFechaFinalCheckListContrato, Busqueda1.nTotalSegundosTrabajadosCheckListContrato,
        '                                                           Busqueda1.cEstadoActividadCheckListContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        '        Else
        '            x = Data.PA_LOGI_MNT_CHECKLISTContrato("SQL_UPDATE", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
        '                                                       Busqueda1.cIdEmpresa, Busqueda1.cIdEquipoCabeceraContrato, Busqueda1.cIdEquipoCheckListContrato, Busqueda1.cIdCatalogoCheckListContrato, Busqueda1.cIdJerarquiaCatalogoCheckListContrato,
        '                                                       Busqueda1.cIdActividadCheckListContrato, Busqueda1.vIdArticuloSAPCabeceraContrato, Busqueda1.nIdNumeroItemCheckListContrato,
        '                                                       Busqueda1.cIdNumeroCabeceraCheckListPlantillaCheckListContrato, Busqueda1.cIdTipoMantenimientoCheckListContrato, Busqueda1.vObservacionCheckListContrato,
        '                                                       Busqueda1.cEstadoCheckListContrato, Busqueda1.dFechaInicioCheckListContrato, Busqueda1.dFechaFinalCheckListContrato, Busqueda1.nTotalSegundosTrabajadosCheckListContrato,
        '                                                       Busqueda1.cEstadoActividadCheckListContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        '        End If
        '    Next
        '    For Each Busqueda2 In RecursosHumanosContrato
        '        If Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_RECURSOSContrato WITH (NOLOCK) " &
        '                               "WHERE " &
        '                               "cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND " &
        '                               "vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & Busqueda2.cIdEmpresa & "' AND " &
        '                               "cIdPersonal = '" & Busqueda2.cIdPersonal & "' AND cIdEquipoCabeceraContrato = '" & Busqueda2.cIdEquipoCabeceraContrato & "' AND vIdArticuloSAPCabeceraContrato = '" & Busqueda2.vIdArticuloSAPCabeceraContrato & "'",
        '                               "", "", "", "", Now, Now, Now, Now, "", "1", "").Count = 0 Then
        '            'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
        '            x = Data.PA_LOGI_MNT_RECURSOSContrato("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
        '                                                      Busqueda2.cIdEmpresa, Busqueda2.cIdPersonal, Busqueda2.vObservacionRecursosContrato, Busqueda2.bEstadoRegistroRecursosContrato,
        '                                                      Busqueda2.cIdEquipoCabeceraContrato, Busqueda2.nTotalMinutosTrabajadosRecursosContrato, Busqueda2.vIdArticuloSAPCabeceraContrato,
        '                                                      Busqueda2.bResponsableRecursosContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        '        Else
        '            x = Data.PA_LOGI_MNT_RECURSOSContrato("SQL_UPDATE", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato,
        '                                                      Busqueda2.cIdEmpresa, Busqueda2.cIdPersonal, Busqueda2.vObservacionRecursosContrato, Busqueda2.bEstadoRegistroRecursosContrato,
        '                                                      Busqueda2.cIdEquipoCabeceraContrato, Busqueda2.nTotalMinutosTrabajadosRecursosContrato, Busqueda2.vIdArticuloSAPCabeceraContrato,
        '                                                      Busqueda2.bResponsableRecursosContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        '        End If
        '    Next
        '    scope.Complete()
        '    Return x
        'End Using
    End Function
    'Public Function ContratoInsertaDetallev2(ByVal DetalleContrato As LOGI_DETALLECONTRATO, ByVal planContrato As LOGI_PLANIFICACIONEQUIPOCONTRATO, ByVal OrdenesTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal RecursosHumanosContrato As DataTable, ByVal componenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32 ' ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
    Public Function ContratoInsertaDetallev2(ByVal DetalleContrato As LOGI_DETALLECONTRATO, ByVal planContrato As LOGI_PLANIFICACIONEQUIPOCONTRATO, ByVal OrdenesTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal RecursosHumanosOrdenTrabajo As List(Of LOGI_RECURSOSORDENTRABAJO), ByVal componenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32 ' ByVal RecursosHumanosContrato As List(Of LOGI_RECURSOSORDENTRABAJO)) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)


            If OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo = "" Then
                x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_INSERT", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.cIdEmpresa, OrdenesTrabajo.cIdEquipoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OrdenesTrabajo.cIdEquipoSAPCabeceraOrdenTrabajo, OrdenesTrabajo.cIdTipoMantenimientoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.dFechaTransaccionCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaEmisionCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaEjecucionInicialCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.dFechaEjecucionFinalCabeceraOrdenTrabajo, OrdenesTrabajo.cIdClienteCabeceraOrdenTrabajo, OrdenesTrabajo.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, OrdenesTrabajo.cEstadoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.bEstadoRegistroCabeceraOrdenTrabajo, DetalleContrato.cIdTipoDocumentoCabeceraContrato & "-" & DetalleContrato.vIdNumeroSerieCabeceraContrato & "-" & DetalleContrato.vIdNumeroCorrelativoCabeceraContrato,
                                                                  OrdenesTrabajo.cIdUsuarioCreacionCabeceraOrdenTrabajo, OrdenesTrabajo.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, OrdenesTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.nPeriodicidadDiasCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
            Else
                x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_UPDATE", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.cIdEmpresa, OrdenesTrabajo.cIdEquipoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OrdenesTrabajo.cIdEquipoSAPCabeceraOrdenTrabajo, OrdenesTrabajo.cIdTipoMantenimientoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.dFechaTransaccionCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaEmisionCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaEjecucionInicialCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.dFechaEjecucionFinalCabeceraOrdenTrabajo, OrdenesTrabajo.cIdClienteCabeceraOrdenTrabajo, OrdenesTrabajo.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, OrdenesTrabajo.cEstadoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.bEstadoRegistroCabeceraOrdenTrabajo, DetalleContrato.cIdTipoDocumentoCabeceraContrato & "-" & DetalleContrato.vIdNumeroSerieCabeceraContrato & "-" & DetalleContrato.vIdNumeroCorrelativoCabeceraContrato,
                                                                  OrdenesTrabajo.cIdUsuarioCreacionCabeceraOrdenTrabajo, OrdenesTrabajo.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo, OrdenesTrabajo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, OrdenesTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo,
                                                                  OrdenesTrabajo.nPeriodicidadDiasCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
            End If



            If OrdenesTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo = planContrato.dFechaHoraProgramacionPlanificacionEquipoContrato Then
                planContrato.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo + "-" + OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo + "-" + OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                x = Data.PA_LOGI_MNT_PLANIFICACIONEQUIPOCONTRATO("SQL_INSERT", "", DetalleContrato.cIdTipoDocumentoCabeceraContrato, DetalleContrato.vIdNumeroSerieCabeceraContrato, DetalleContrato.vIdNumeroCorrelativoCabeceraContrato,
                                                                         DetalleContrato.cIdEmpresa, planContrato.nIdNumeroItemDetalleContrato, planContrato.cIdEquipoDetalleContrato, planContrato.cIdPeriodoMesPlanificacionEquipoContrato,
                                                                         planContrato.dFechaHoraProgramacionPlanificacionEquipoContrato, planContrato.cIdTipoMantenimientoPlanificacionEquipoContrato,
                                                                         planContrato.vOrdenTrabajoReferenciaPlanificacionEquipoContrato,
                                                                         planContrato.vOrdenTrabajoClientePlanificacionEquipoContrato, planContrato.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato,
                                                                         DetalleContrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString


                Dim EquipoMet As New clsEquipoMetodos
                Dim Equipo As LOGI_EQUIPO = EquipoMet.EquipoListarPorId(planContrato.cIdEquipoDetalleContrato)
                Dim dsCheckListPlantilla = ContratoGetData("SELECT CABCHKLISPLA.cIdTipoMantenimiento, CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, CABCHKLISPLA.dFechaTransaccionCabeceraCheckListPlantilla, " &
                                                               "       CABCHKLISPLA.bEstadoRegistroCabeceraCheckListPlantilla, CABCHKLISPLA.cIdTipoActivoCabeceraCheckListPlantilla, CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla, " &
                                                               "       CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, DETCHKLISPLA.cIdActividadCheckList, DETCHKLISPLA.vDescripcionDetalleCheckListPlantilla, " &
                                                               "       DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla, DETCHKLISPLA.cIdTipoActivo, DETCHKLISPLA.cIdCatalogo, DETCHKLISPLA.cIdJerarquiaCatalogo, EQU.cIdEquipo " &
                                                               "FROM LOGI_CABECERACHECKLISTPLANTILLA AS CABCHKLISPLA INNER JOIN LOGI_DETALLECHECKLISTPLANTILLA AS DETCHKLISPLA ON " &
                                                               "     CABCHKLISPLA.cIdTipoMantenimiento = DETCHKLISPLA.cIdTipoMantenimiento AND " &
                                                               "     CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla " &
                                                               "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                               "     DETCHKLISPLA.cIdCatalogo = EQU.cIdCatalogo AND " &
                                                               "     DETCHKLISPLA.cIdJerarquiaCatalogo = EQU.cIdJerarquiaCatalogo AND " &
                                                               "     EQU.cIdEnlaceEquipo = '" & planContrato.cIdEquipoDetalleContrato & "' " &
                                                               "WHERE CABCHKLISPLA.cIdTipoMantenimiento = '" & planContrato.cIdTipoMantenimientoPlanificacionEquipoContrato & "' AND CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & planContrato.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato & "' " &
                                                               "      AND CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla = '" & Equipo.cIdCatalogo & "' AND CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0' " &
                                                               "      AND DETCHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '1' " &
                                                               "ORDER BY DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla")

                Dim ColeccionCheckList As New List(Of LOGI_CHECKLISTORDENTRABAJO)
                For Each CheckListPlantilla In dsCheckListPlantilla.Rows
                    Dim ChkList As New LOGI_CHECKLISTORDENTRABAJO
                    With ChkList
                        .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo
                        .vIdNumeroSerieCabeceraOrdenTrabajo = OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo
                        .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                        .vObservacionCheckListOrdenTrabajo = ""
                        .cIdEmpresa = OrdenesTrabajo.cIdEmpresa
                        .cEstadoCheckListOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                        .nIdNumeroItemCheckListOrdenTrabajo = CheckListPlantilla("nIdNumeroItemDetalleCheckListPlantilla")
                        .nTotalSegundosTrabajadosCheckListOrdenTrabajo = 0
                        .dFechaInicioCheckListOrdenTrabajo = Nothing
                        .dFechaFinalCheckListOrdenTrabajo = Nothing
                        .cIdEquipoCabeceraOrdenTrabajo = planContrato.cIdEquipoDetalleContrato
                        .cIdEquipoCheckListOrdenTrabajo = CheckListPlantilla("cIdEquipo")
                        .cIdActividadCheckListOrdenTrabajo = CheckListPlantilla("cIdActividadCheckList")
                        .cIdTipoMantenimientoCheckListOrdenTrabajo = CheckListPlantilla("cIdTipoMantenimiento")
                        .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")
                        .cIdCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdCatalogo")
                        .cIdJerarquiaCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdJerarquiaCatalogo")
                        .vIdArticuloSAPCabeceraOrdenTrabajo = OrdenesTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo
                        .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = planContrato.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato 'cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue
                    End With
                    ColeccionCheckList.Add(ChkList)
                Next

                For Each OTCheckList In ColeccionCheckList

                    If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WITH (NOLOCK) " &
                                                                                     "WHERE " &
                                                                                     "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OTCheckList.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OTCheckList.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                                                                     "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OTCheckList.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & OTCheckList.cIdEmpresa & "' AND " &
                                                                                     "cIdEquipoCabeceraOrdenTrabajo = '" & OTCheckList.cIdEquipoCabeceraOrdenTrabajo & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & OTCheckList.cIdCatalogoCheckListOrdenTrabajo & "' AND " &
                                                                                     "cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & OTCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "' AND cIdActividadCheckListOrdenTrabajo = '" & OTCheckList.cIdActividadCheckListOrdenTrabajo & "' AND " &
                                                                                     "vIdArticuloSAPCabeceraOrdenTrabajo = '" & OTCheckList.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                                                                                     "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count = 0 Then
                        'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
                        x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_INSERT", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                                       OTCheckList.cIdEmpresa, OTCheckList.cIdEquipoCabeceraOrdenTrabajo, OTCheckList.cIdEquipoCheckListOrdenTrabajo, OTCheckList.cIdCatalogoCheckListOrdenTrabajo, OTCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                                                       OTCheckList.cIdActividadCheckListOrdenTrabajo, OTCheckList.vIdArticuloSAPCabeceraOrdenTrabajo, OTCheckList.nIdNumeroItemCheckListOrdenTrabajo,
                                                                                       OTCheckList.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, OTCheckList.cIdTipoMantenimientoCheckListOrdenTrabajo, OTCheckList.vObservacionCheckListOrdenTrabajo,
                                                                                       OTCheckList.cEstadoCheckListOrdenTrabajo, OTCheckList.dFechaInicioCheckListOrdenTrabajo, OTCheckList.dFechaFinalCheckListOrdenTrabajo, OTCheckList.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                                                       OTCheckList.cEstadoActividadCheckListOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                    Else
                        x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_UPDATE", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                               OTCheckList.cIdEmpresa, OTCheckList.cIdEquipoCabeceraOrdenTrabajo, OTCheckList.cIdEquipoCheckListOrdenTrabajo, OTCheckList.cIdCatalogoCheckListOrdenTrabajo, OTCheckList.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                                               OTCheckList.cIdActividadCheckListOrdenTrabajo, OTCheckList.vIdArticuloSAPCabeceraOrdenTrabajo, OTCheckList.nIdNumeroItemCheckListOrdenTrabajo,
                                                                               OTCheckList.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, OTCheckList.cIdTipoMantenimientoCheckListOrdenTrabajo, OTCheckList.vObservacionCheckListOrdenTrabajo,
                                                                               OTCheckList.cEstadoCheckListOrdenTrabajo, OTCheckList.dFechaInicioCheckListOrdenTrabajo, OTCheckList.dFechaFinalCheckListOrdenTrabajo, OTCheckList.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                                               OTCheckList.cEstadoActividadCheckListOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                    End If
                Next

                'For Each lstRRHHContrato In RecursosHumanosContrato.Rows

                '    If Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT ROT.* FROM LOGI_CABECERAORDENTRABAJO AS COT WITH (NOLOCK) INNER JOIN LOGI_RECURSOSORDENTRABAJO AS ROT WITH (NOLOCK) ON " &
                '                                       "COT.cIdTipoDocumentoCabeceraOrdenTrabajo = ROT.cIdTipoDocumentoCabeceraOrdenTrabajo AND " &
                '                                       "COT.vIdNumeroSerieCabeceraOrdenTrabajo = ROT.vIdNumeroSerieCabeceraOrdenTrabajo AND " &
                '                                       "COT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ROT.vIdNumeroCorrelativoCabeceraOrdenTrabajo AND " &
                '                                       "COT.cIdEmpresa = ROT.cIdEmpresa AND COT.cIdEquipoCabeceraOrdenTrabajo = ROT.cIdEquipoCabeceraOrdenTrabajo " &
                '                                       "WHERE " &
                '                                       "ROT.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND ROT.vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                '                                       "ROT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND ROT.cIdEmpresa = '" & OrdenesTrabajo.cIdEmpresa & "' AND " &
                '                                       "ROT.cIdPersonal = '" & lstRRHHContrato("IdPersonal") & "' AND ROT.cIdEquipoCabeceraOrdenTrabajo = '" & lstRRHHContrato("IdEquipo") & "' AND " &
                '                                       "CONVERT(CHAR(8), COT.dFechaInicioPlanificadaCabeceraOrdenTrabajo, 112) = '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(lstRRHHContrato("FechaPlanificacion"))) & "'",
                '                                       "", "", "", "", Now, Now, Now, Now, "", "1", " ", "", "", Now, Now, "", "", "").Count = 0 Then
                '        If String.Format("{0:yyyyMMdd}", Convert.ToDateTime(lstRRHHContrato("FechaPlanificacion"))) = String.Format("{0:yyyyMMdd}", OrdenesTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo) And
                '                            lstRRHHContrato("IdEquipo") = OrdenesTrabajo.cIdEquipoCabeceraOrdenTrabajo Then

                '            x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_INSERT", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                '                                                              OrdenesTrabajo.cIdEmpresa, lstRRHHContrato("IdPersonal"), "", "1",
                '                                                              lstRRHHContrato("IdEquipo"), "0", "",
                '                                                              lstRRHHContrato("Responsable"), OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                '        End If
                '    Else
                '        If String.Format("{0:yyyyMM}", Convert.ToDateTime(lstRRHHContrato("FechaPlanificacion"))) = String.Format("{0:yyyyMM}", OrdenesTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo) And
                '                            lstRRHHContrato("IdEquipo") = OrdenesTrabajo.cIdEquipoCabeceraOrdenTrabajo Then

                '            ContratoGetData("UPDATE LOGI_RECURSOSORDENTRABAJO SET bResponsableRecursosOrdenTrabajo = '" & IIf(CBool(lstRRHHContrato("Responsable")) = True, "1", "0") & "' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & OrdenesTrabajo.cIdEmpresa & "' AND cIdPersonal = '" & lstRRHHContrato("IdPersonal") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & lstRRHHContrato("IdEquipo") & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = ''")
                '        End If
                '    End If
                'Next
                For Each Busqueda2 In RecursosHumanosOrdenTrabajo
                    If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_RECURSOSORDENTRABAJO WITH (NOLOCK) " &
                                       "WHERE " &
                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda2.cIdEmpresa & "' AND " &
                                       "cIdPersonal = '" & Busqueda2.cIdPersonal & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda2.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                           "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count = 0 Then
                        'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
                        'If OrdenTrabajoGetData("SELECT * FROM LOGI_RECURSOSORDENTRABAJO WITH (NOLOCK) " &
                        '                   "WHERE " &
                        '                   "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                        '                   "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda2.cIdEmpresa & "' AND " &
                        '                   "cIdPersonal = '" & Busqueda2.cIdPersonal & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda2.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo & "'").Rows.Count = 0 Then
                        x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_INSERT", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda2.cIdEmpresa, Busqueda2.cIdPersonal, Busqueda2.vObservacionRecursosOrdenTrabajo, Busqueda2.bEstadoRegistroRecursosOrdenTrabajo,
                                                              Busqueda2.cIdEquipoCabeceraOrdenTrabajo, Busqueda2.nTotalMinutosTrabajadosRecursosOrdenTrabajo, Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                              Busqueda2.bResponsableRecursosOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                    Else
                        x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_UPDATE", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda2.cIdEmpresa, Busqueda2.cIdPersonal, Busqueda2.vObservacionRecursosOrdenTrabajo, Busqueda2.bEstadoRegistroRecursosOrdenTrabajo,
                                                              Busqueda2.cIdEquipoCabeceraOrdenTrabajo, Busqueda2.nTotalMinutosTrabajadosRecursosOrdenTrabajo, Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                              Busqueda2.bResponsableRecursosOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                    End If
                Next

                For Each Busqueda3 In componenteOrdenTrabajo
                    If Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO WITH (NOLOCK) " &
                                                       "WHERE " &
                                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda3.cIdEmpresa & "' AND " &
                                                       "cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda3.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo & "' AND " &
                                                       "cIdEquipoComponenteOrdenTrabajo = '" & Busqueda3.cIdEquipoComponenteOrdenTrabajo & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Busqueda3.cIdCatalogoComponenteOrdenTrabajo & "' AND " &
                                                       "cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo & "' AND nIdNumeroItemComponenteOrdenTrabajo = '" & Busqueda3.nIdNumeroItemComponenteOrdenTrabajo & "'",
                                           "", "", "", "", "", "", "", "", "", "", "1", Now, Now, 0, 0, "").Count = 0 Then
                        'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes

                        x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_INSERT", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                              Busqueda3.cIdEmpresa, Busqueda3.cIdEquipoCabeceraOrdenTrabajo, Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda3.cIdEquipoComponenteOrdenTrabajo,
                                                                              Busqueda3.cIdCatalogoComponenteOrdenTrabajo, Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo, Busqueda3.vObservacionComponenteOrdenTrabajo,
                                                                              Busqueda3.cEstadoComponenteOrdenTrabajo, Busqueda3.dFechaInicioActividadComponenteOrdenTrabajo, Busqueda3.dFechaFinalActividadComponenteOrdenTrabajo,
                                                                              Busqueda3.nIdNumeroItemComponenteOrdenTrabajo, Busqueda3.nTotalSegundosTrabajadosComponenteOrdenTrabajo,
                                                                              OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                    Else
                        x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_UPDATE", "", OrdenesTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                              Busqueda3.cIdEmpresa, Busqueda3.cIdEquipoCabeceraOrdenTrabajo, Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda3.cIdEquipoComponenteOrdenTrabajo,
                                                                              Busqueda3.cIdCatalogoComponenteOrdenTrabajo, Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo, Busqueda3.vObservacionComponenteOrdenTrabajo,
                                                                              Busqueda3.cEstadoComponenteOrdenTrabajo, Busqueda3.dFechaInicioActividadComponenteOrdenTrabajo, Busqueda3.dFechaFinalActividadComponenteOrdenTrabajo,
                                                                              Busqueda3.nIdNumeroItemComponenteOrdenTrabajo, Busqueda3.nTotalSegundosTrabajadosComponenteOrdenTrabajo,
                                                                              OrdenesTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                    End If
                Next
            End If

            scope.Complete()
            Return x
        End Using

    End Function

    Public Function ContratoInserta(ByVal Contrato As LOGI_CABECERACONTRATO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato,
                                           Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa, Contrato.dFechaTransaccionCabeceraContrato,
                                           Contrato.dFechaEmisionCabeceraContrato, Contrato.dFechaVigenciaInicialCabeceraContrato, Contrato.dFechaVigenciaFinalCabeceraContrato,
                                           Contrato.cIdCliente, Contrato.bEstadoRegistroCabeceraContrato, Contrato.cEstadoCabeceraContrato, Contrato.vDescripcionCabeceraContrato,
                                           Contrato.vNroLicitacionCabeceraContrato, Contrato.dFechaUltimaModificacionCabeceraContrato, Contrato.dFechaCreacionCabeceraContrato,
                                           Contrato.cIdUsuarioUltimaModificacionCabeceraContrato, Contrato.cIdUsuarioCreacionCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        Return x
    End Function

    Public Function ContratoEdita(ByVal Contrato As LOGI_CABECERACONTRATO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_UPDATE", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato,
                                           Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa, Contrato.dFechaTransaccionCabeceraContrato,
                                           Contrato.dFechaEmisionCabeceraContrato, Contrato.dFechaVigenciaInicialCabeceraContrato, Contrato.dFechaVigenciaFinalCabeceraContrato,
                                           Contrato.cIdCliente, Contrato.bEstadoRegistroCabeceraContrato, Contrato.cEstadoCabeceraContrato, Contrato.vDescripcionCabeceraContrato,
                                           Contrato.vNroLicitacionCabeceraContrato, Contrato.dFechaUltimaModificacionCabeceraContrato, Contrato.dFechaCreacionCabeceraContrato,
                                           Contrato.cIdUsuarioUltimaModificacionCabeceraContrato, Contrato.cIdUsuarioCreacionCabeceraContrato, Contrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        Return x
    End Function

    Public Function ContratoQuery(ByVal query As String) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", query, "", "", "", "", Now, Now, Now, Now, "", "1", "", "", "", Now, Now, "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function ContratoElimina(ByVal Contrato As LOGI_CABECERACONTRATO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "UPDATE LOGI_CABECERACONTRATO SET bEstadoRegistroCabeceraContrato = 0 WHERE cIdTipoDocumentoCabeceraContrato = '" & Contrato.cIdTipoDocumentoCabeceraContrato & "' AND vIdNumeroSerieCabeceraContrato = '" & Contrato.vIdNumeroSerieCabeceraContrato & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Contrato.vIdNumeroCorrelativoCabeceraContrato & "' AND cIdEmpresa = '" & Contrato.cIdEmpresa & "' ",
                                              "", "", "", "", Now, Now, Now, Now, "", "1", "", "", "", Now, Now, "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function ContratoExiste(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As Boolean
        If Data.PA_LOGI_MNT_CABECERACONTRATO("SQL_NONE", "SELECT * FROM LOGI_CABECERACONTRATO " &
                                                         "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                         " AND bEstadoRegistroCabeceraContrato = '1'",
                                                         "", "", "", "", Now, Now, Now, Now, "", "1", "", "", "", Now, Now, "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ContratoVigenciaInserta(ByVal Contrato As LOGI_CONTRATOVIGENCIA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CONTRATOVIGENCIA("SQL_INSERT", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato,
                                           Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa, Contrato.dFechaVigenciaInicialCabeceraContrato, Contrato.dFechaVigenciaFinalCabeceraContrato, Contrato.bEstadoActivoVigencia).ReturnValue.ToString
        Return x
    End Function

    Public Function ContratoVigenciaEdita(ByVal Contrato As LOGI_CONTRATOVIGENCIA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CONTRATOVIGENCIA("SQL_UPDATE", "", Contrato.cIdTipoDocumentoCabeceraContrato, Contrato.vIdNumeroSerieCabeceraContrato,
                                           Contrato.vIdNumeroCorrelativoCabeceraContrato, Contrato.cIdEmpresa, Contrato.dFechaVigenciaInicialCabeceraContrato, Contrato.dFechaVigenciaFinalCabeceraContrato, Contrato.bEstadoActivoVigencia).ReturnValue.ToString
        Return x
    End Function

    Public Function ContratoVigenciaQuery(ByVal query As String) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CONTRATOVIGENCIA("SQL_NONE", query, "", "",
                                           "", "", Now, Now, True).ReturnValue.ToString
        Return x
    End Function

    'Public Function ContratoPeriodoVigente(ByVal fechaIni As Date, ByVal fechaFin As Date) As List(Of BDCMMS_MovitecnicaDataContext.AnioVigencia)
    Public Function ContratoPeriodoVigente(ByVal fechaIni As Date, ByVal fechaFin As Date) As List(Of AnioVigencia)
        Dim x = Data.PA_LOGI_GET_ANIOSVIGENCIA(fechaIni, fechaFin)
        'Dim coleccion As New List(Of BDCMMS_MovitecnicaDataContext.AnioVigencia)
        Dim coleccion As New List(Of AnioVigencia)
        For Each item In x
            'Dim anio As New BDCMMS_MovitecnicaDataContext.AnioVigencia
            Dim anio As New AnioVigencia
            anio.Anio = item.Anio
            coleccion.Add(anio)
        Next
        Return coleccion
    End Function

    'Public Function ContratoPeriodoMes(ByVal fechaIni As Date, ByVal fechaFin As Date, ByVal periodo As Integer) As List(Of BDCMMS_MovitecnicaDataContext.MesVigencia)
    Public Function ContratoPeriodoMes(ByVal fechaIni As Date, ByVal fechaFin As Date, ByVal periodo As Integer) As List(Of MesVigencia)
        Dim x
        x = Data.PA_LOGI_GET_MESESXANIOVIGENCIA(fechaIni, fechaFin, periodo)

        'Dim coleccion As New List(Of BDCMMS_MovitecnicaDataContext.MesVigencia)
        Dim coleccion As New List(Of MesVigencia)

        For Each item In x
            'Dim mesVigencia As New BDCMMS_MovitecnicaDataContext.MesVigencia
            Dim mesVigencia As New MesVigencia
            mesVigencia.Years = item.Years
            mesVigencia.Months = item.Months
            mesVigencia.NombreMes = item.NombreMes
            coleccion.Add(mesVigencia)
        Next
        Return coleccion
    End Function

    Public Function ContratroVigenciaListar(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As List(Of LOGI_CONTRATOVIGENCIA)

        Dim Consulta = Data.PA_LOGI_MNT_CONTRATOVIGENCIA("SQL_NONE", "SELECT * FROM LOGI_CONTRATOVIGENCIA " &
                                                         "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "'",
                                                         "", "", "", "", Now, Now, True)

        Dim Coleccion As New List(Of LOGI_CONTRATOVIGENCIA)
        For Each Contrato In Consulta
            Dim item As New LOGI_CONTRATOVIGENCIA
            item.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
            item.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
            item.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
            item.cIdEmpresa = Contrato.cIdEmpresa
            item.dFechaVigenciaInicialCabeceraContrato = Contrato.dFechaVigenciaInicialCabeceraContrato
            item.dFechaVigenciaFinalCabeceraContrato = Contrato.dFechaVigenciaFinalCabeceraContrato
            item.bEstadoActivoVigencia = Contrato.bEstadoActivoVigencia
            item.Id = Contrato.Id
            Coleccion.Add(item)
        Next
        Return Coleccion
    End Function

End Class
