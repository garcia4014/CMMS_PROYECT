Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsOrdenTrabajoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function OrdenTrabajoGetData(strQuery As String) As DataTable
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

    Public Function OrdenTrabajoListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENTRABAJO)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENTRABAJO " &
                                                 "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipoOrden & "' ",
                                                 "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "")
        Dim Coleccion As New List(Of LOGI_CABECERAORDENTRABAJO)
        For Each OrdenTrabajo In Consulta
            Dim OrdFab As New LOGI_CABECERAORDENTRABAJO
            OrdFab.vIdNumeroSerieCabeceraOrdenTrabajo = OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo
            OrdFab.vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo
            Coleccion.Add(OrdFab)
        Next
        Return Coleccion
    End Function

    'Public Function TipoFrecuenciaLista() As List(Of GNRL_TIPOFRECUENCIA)
    '    Dim consulta = Data.PA_GNRL_TIPOFRECUENCIA("SQL_NONE", "SELECT * FROM GNRL_TIPOFRECUENCIA where bEstado = 1", 0, "", True)
    '    Dim coleccion As New List(Of GNRL_TIPOFRECUENCIA)

    '    For Each tipoFrecuencia In consulta
    '        Dim item As New GNRL_TIPOFRECUENCIA
    '        item.Id = tipoFrecuencia.Id
    '        item.DescripcionTipoFrecuencia = tipoFrecuencia.DescripcionTipoFrecuencia
    '        item.bEstadoActivoVigencia = tipoFrecuencia.bEstado
    '        coleccion.Add(item)
    '    Next

    '    Return coleccion
    'End Function

    Public Function OrdenTrabajoRecursosListarCombo(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO) As List(Of RRHH_PERSONAL)

        Dim dsRecursos = OrdenTrabajoGetData("SELECT RECHUM.*, PER.vNombreCompletoPersonal FROM LOGI_RECURSOSORDENTRABAJO AS RECHUM INNER JOIN RRHH_PERSONAL AS PER ON " &
                       "RECHUM.cIdPersonal = PER.cIdPersonal " &
                       "WHERE " &
                       "RECHUM.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND RECHUM.vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                       "RECHUM.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND RECHUM.cIdEmpresa = '" & OrdTra.cIdEmpresa & "' AND " &
                       "RECHUM.cIdEquipoCabeceraOrdenTrabajo = '" & OrdTra.cIdEquipoCabeceraOrdenTrabajo & "' AND RECHUM.vIdArticuloSAPCabeceraOrdenTrabajo = '" & OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo & "'")
        Dim Coleccion As New List(Of RRHH_PERSONAL)
        For Each Recursos In dsRecursos.Rows
            Dim RRHH As New RRHH_PERSONAL
            RRHH.cIdPersonal = Recursos("cIdPersonal")
            RRHH.vNombreCompletoPersonal = Recursos("vNombreCompletoPersonal")
            Coleccion.Add(RRHH)
        Next
        Return Coleccion
    End Function

    Public Function OrdenTrabajoRecursosListarComboV2(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO) As List(Of RRHH_PERSONAL)

        Dim dsRecursos = OrdenTrabajoGetData("SELECT RECHUM.*, PER.vNombreCompletoPersonal FROM LOGI_RECURSOSORDENTRABAJO AS RECHUM INNER JOIN RRHH_PERSONAL AS PER ON " &
                       "RECHUM.cIdPersonal = PER.cIdPersonal " &
                       "WHERE " &
                       "RECHUM.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND RECHUM.vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                       "RECHUM.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND RECHUM.cIdEmpresa = '" & OrdTra.cIdEmpresa & "' AND " & " RECHUM.bEstadoRegistroRecursosOrdenTrabajo = " & 1 & " AND " &
                       "RECHUM.cIdEquipoCabeceraOrdenTrabajo = '" & OrdTra.cIdEquipoCabeceraOrdenTrabajo & "' AND RECHUM.vIdArticuloSAPCabeceraOrdenTrabajo = '" & OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo & "'")
        Dim Coleccion As New List(Of RRHH_PERSONAL)
        For Each Recursos In dsRecursos.Rows
            Dim RRHH As New RRHH_PERSONAL
            RRHH.cIdPersonal = Recursos("cIdPersonal")
            RRHH.vNombreCompletoPersonal = Recursos("vNombreCompletoPersonal")
            Coleccion.Add(RRHH)
        Next
        Return Coleccion
    End Function

    Public Function OrdenTrabajoRecursosListar(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO) As List(Of LOGI_RECURSOSORDENTRABAJO)

        Dim dsRecursos = OrdenTrabajoGetData("SELECT RECHUM.*, PER.vNombreCompletoPersonal, PER.vNumeroDocumentoPersonal FROM LOGI_RECURSOSORDENTRABAJO AS RECHUM INNER JOIN RRHH_PERSONAL AS PER ON " &
                       "RECHUM.cIdPersonal = PER.cIdPersonal " &
                       "WHERE " &
                       "RECHUM.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND RECHUM.vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                       "RECHUM.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND RECHUM.cIdEmpresa = '" & OrdTra.cIdEmpresa & "' AND " &
                       "RECHUM.cIdEquipoCabeceraOrdenTrabajo = '" & OrdTra.cIdEquipoCabeceraOrdenTrabajo & "' AND RECHUM.vIdArticuloSAPCabeceraOrdenTrabajo = '" & OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo & "'")
        Dim Coleccion As New List(Of LOGI_RECURSOSORDENTRABAJO)
        For Each Recursos In dsRecursos.Rows
            Dim RRHH As New LOGI_RECURSOSORDENTRABAJO
            RRHH.cIdTipoDocumentoCabeceraOrdenTrabajo = Recursos("cIdTipoDocumentoCabeceraOrdenTrabajo")
            RRHH.vIdNumeroSerieCabeceraOrdenTrabajo = Recursos("vIdNumeroSerieCabeceraOrdenTrabajo")
            RRHH.vIdNumeroCorrelativoCabeceraOrdenTrabajo = Recursos("vIdNumeroCorrelativoCabeceraOrdenTrabajo")
            RRHH.cIdEmpresa = Recursos("cIdEmpresa")
            RRHH.cIdPersonal = Recursos("cIdPersonal")
            RRHH.vObservacionRecursosOrdenTrabajo = Recursos("vObservacionRecursosOrdenTrabajo")
            RRHH.bEstadoRegistroRecursosOrdenTrabajo = Recursos("bEstadoRegistroRecursosOrdenTrabajo")
            RRHH.cIdEquipoCabeceraOrdenTrabajo = Recursos("cIdEquipoCabeceraOrdenTrabajo")
            RRHH.nTotalMinutosTrabajadosRecursosOrdenTrabajo = Recursos("nTotalMinutosTrabajadosRecursosOrdenTrabajo")
            RRHH.vIdArticuloSAPCabeceraOrdenTrabajo = Recursos("vIdArticuloSAPCabeceraOrdenTrabajo")
            RRHH.bResponsableRecursosOrdenTrabajo = Recursos("bResponsableRecursosOrdenTrabajo")
            RRHH.RRHH_PERSONAL = New RRHH_PERSONAL
            RRHH.RRHH_PERSONAL.cIdPersonal = Recursos("cIdPersonal")
            RRHH.RRHH_PERSONAL.vNombreCompletoPersonal = Recursos("vNombreCompletoPersonal")
            RRHH.RRHH_PERSONAL.vNumeroDocumentoPersonal = Recursos("vNumeroDocumentoPersonal")
            Coleccion.Add(RRHH)
        Next
        Return Coleccion
    End Function

    Public Function OrdenTrabajoValidarRecurso(ByVal query As String) As List(Of LOGI_RECURSOSORDENTRABAJO)

        Dim dsRecursos = OrdenTrabajoGetData(query)

        Dim Coleccion As New List(Of LOGI_RECURSOSORDENTRABAJO)
        For Each Recursos In dsRecursos.Rows
            Dim RRHH As New LOGI_RECURSOSORDENTRABAJO
            RRHH.cIdTipoDocumentoCabeceraOrdenTrabajo = Recursos("cIdTipoDocumentoCabeceraOrdenTrabajo")
            RRHH.vIdNumeroSerieCabeceraOrdenTrabajo = Recursos("vIdNumeroSerieCabeceraOrdenTrabajo")
            RRHH.vIdNumeroCorrelativoCabeceraOrdenTrabajo = Recursos("vIdNumeroCorrelativoCabeceraOrdenTrabajo")
            RRHH.cIdEmpresa = Recursos("cIdEmpresa")
            RRHH.cIdPersonal = Recursos("cIdPersonal")
            RRHH.vObservacionRecursosOrdenTrabajo = Recursos("vObservacionRecursosOrdenTrabajo")
            RRHH.bEstadoRegistroRecursosOrdenTrabajo = Recursos("bEstadoRegistroRecursosOrdenTrabajo")
            RRHH.cIdEquipoCabeceraOrdenTrabajo = Recursos("cIdEquipoCabeceraOrdenTrabajo")
            RRHH.nTotalMinutosTrabajadosRecursosOrdenTrabajo = Recursos("nTotalMinutosTrabajadosRecursosOrdenTrabajo")
            RRHH.vIdArticuloSAPCabeceraOrdenTrabajo = Recursos("vIdArticuloSAPCabeceraOrdenTrabajo")
            RRHH.bResponsableRecursosOrdenTrabajo = Recursos("bResponsableRecursosOrdenTrabajo")
            Coleccion.Add(RRHH)
        Next
        Return Coleccion
    End Function

    'Public Function OrdenTrabajoListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENTRABAJO
    '    Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
    '                                           " AND bEstadoRegistroCabeceraOrdenTrabajo = 1 " &
    '                                           "ORDER BY cIdCatalogo",
    '                                           "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "")
    '    Dim Coleccion As New LOGI_CABECERAORDENTRABAJO
    '    For Each LOGI_CABECERAORDENTRABAJO In Consulta
    '        Coleccion.cIdTipoDocumentoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdTipoDocumentoCabeceraOrdenTrabajo
    '        Coleccion.vIdNumeroSerieCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vIdNumeroSerieCabeceraOrdenTrabajo
    '        Coleccion.vIdNumeroCorrelativoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vIdNumeroCorrelativoCabeceraOrdenTrabajo
    '        Coleccion.cIdEmpresa = LOGI_CABECERAORDENTRABAJO.cIdEmpresa
    '        Coleccion.cIdEquipoSAPCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdEquipoSAPCabeceraOrdenTrabajo
    '        Coleccion.vIdArticuloSAPCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vIdArticuloSAPCabeceraOrdenTrabajo
    '        Coleccion.dFechaTransaccionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaTransaccionCabeceraOrdenTrabajo
    '        Coleccion.vIdClienteSAPCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vIdClienteSAPCabeceraOrdenTrabajo
    '        Coleccion.cIdCliente = LOGI_CABECERAORDENTRABAJO.cIdCliente
    '        Coleccion.cIdEquipo = LOGI_CABECERAORDENTRABAJO.cIdEquipo
    '        Coleccion.vNumeroSerieEquipoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vNumeroSerieEquipoCabeceraOrdenTrabajo
    '        Coleccion.vOrdenVentaCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vOrdenVentaCabeceraOrdenTrabajo
    '        Coleccion.vOrdenCompraCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vOrdenCompraCabeceraOrdenTrabajo
    '        Coleccion.cEstadoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cEstadoCabeceraOrdenTrabajo
    '        Coleccion.bEstadoRegistroCabeceraOrdenTrabajo = IIf(LOGI_CABECERAORDENTRABAJO.bEstadoRegistroCabeceraOrdenTrabajo Is Nothing, False, LOGI_CABECERAORDENTRABAJO.bEstadoRegistroCabeceraOrdenTrabajo)
    '        Coleccion.dFechaUltimaModificacionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaUltimaModificacionCabeceraOrdenTrabajo
    '        Coleccion.dFechaCreacionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaCreacionCabeceraOrdenTrabajo
    '        Coleccion.cIdUsuarioUltimaModificacionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdUsuarioUltimaModificacionCabeceraOrdenTrabajo
    '        Coleccion.cIdUsuarioCreacionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdUsuarioCreacionCabeceraOrdenTrabajo
    '    Next
    '    Return Coleccion
    'End Function

    Public Function OrdenTrabajoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENTRABAJO
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                 "AND bEstadoRegistroCabeceraOrdenTrabajo = 1 ",
                                                 "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "")
        Dim Coleccion As New LOGI_CABECERAORDENTRABAJO
        For Each LOGI_CABECERAORDENTRABAJO In Consulta
            Coleccion.cIdTipoDocumentoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdTipoDocumentoCabeceraOrdenTrabajo
            Coleccion.vIdNumeroSerieCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vIdNumeroSerieCabeceraOrdenTrabajo
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vIdNumeroCorrelativoCabeceraOrdenTrabajo
            Coleccion.dFechaTransaccionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaTransaccionCabeceraOrdenTrabajo
            Coleccion.dFechaEmisionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaEmisionCabeceraOrdenTrabajo
            Coleccion.cIdClienteCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdClienteCabeceraOrdenTrabajo
            Coleccion.cIdEquipoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdEquipoCabeceraOrdenTrabajo
            Coleccion.dFechaInicioPlanificadaCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaInicioPlanificadaCabeceraOrdenTrabajo
            Coleccion.cIdEmpresa = LOGI_CABECERAORDENTRABAJO.cIdEmpresa
            Coleccion.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo
            Coleccion.cIdTipoMantenimientoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdTipoMantenimientoCabeceraOrdenTrabajo
            Coleccion.vIdArticuloSAPCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vIdArticuloSAPCabeceraOrdenTrabajo
            Coleccion.dFechaEjecucionInicialCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaEjecucionInicialCabeceraOrdenTrabajo
            Coleccion.dFechaEjecucionFinalCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaEjecucionFinalCabeceraOrdenTrabajo
            Coleccion.cIdEquipoSAPCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdEquipoSAPCabeceraOrdenTrabajo
            Coleccion.cEstadoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cEstadoCabeceraOrdenTrabajo
            Coleccion.bEstadoRegistroCabeceraOrdenTrabajo = IIf(LOGI_CABECERAORDENTRABAJO.bEstadoRegistroCabeceraOrdenTrabajo Is Nothing, False, LOGI_CABECERAORDENTRABAJO.bEstadoRegistroCabeceraOrdenTrabajo)
            Coleccion.cIdUsuarioCreacionCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdUsuarioCreacionCabeceraOrdenTrabajo
            Coleccion.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo
            Coleccion.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.dFechaTerminoPlanificadaCabeceraOrdenTrabajo
            Coleccion.vContratoReferenciaCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.vContratoReferenciaCabeceraOrdenTrabajo
            Coleccion.cIdTipoControlTiempoCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.cIdTipoControlTiempoCabeceraOrdenTrabajo
            Coleccion.nPeriodicidadDiasCabeceraOrdenTrabajo = LOGI_CABECERAORDENTRABAJO.nPeriodicidadDiasCabeceraOrdenTrabajo
        Next
        Return Coleccion
    End Function

    Public Function OrdenTrabajoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal SubFiltro As String, ByVal Estado As String) As List(Of VI_LOGI_CABECERAORDENTRABAJO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT DOC.cIdTipoDocumentoCabeceraOrdenTrabajo, DOC.vIdNumeroSerieCabeceraOrdenTrabajo, DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo, " &
        '                                           "DOC.cIdEmpresa, DOC.cIdEquipoSAPCabeceraOrdenTrabajo, DOC.vIdArticuloSAPCabeceraOrdenTrabajo, DOC.dFechaTransaccionCabeceraOrdenTrabajo, " &
        '                                           "CLI.vIdClienteSAPCliente, DOC.cIdClienteCabeceraOrdenTrabajo, DOC.cIdEquipoCabeceraOrdenTrabajo, EQU.vNumeroSerieEquipo, " &
        '                                           "DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, DOC.cEstadoCabeceraOrdenTrabajo,  " &
        '                                           "DOC.bEstadoRegistroCabeceraOrdenTrabajo, DOC.dFechaUltimaModificacionCabeceraOrdenTrabajo, DOC.dFechaCreacionCabeceraOrdenTrabajo, " &
        '                                           "DOC.cIdUsuarioUltimaModificacionCabeceraOrdenTrabajo, DOC.cIdUsuarioCreacionCabeceraOrdenTrabajo, " &
        '                                           "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo " &
        '                                           "FROM LOGI_CABECERAORDENTRABAJO AS DOC LEFT JOIN GNRL_CLIENTE AS CLI ON " &
        '                                           "     DOC.cIdClienteCabeceraOrdenTrabajo = CLI.cIdCliente AND DOC.cIdEmpresa = CLI.cIdEmpresa " &
        '                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
        '                                           "     DOC.cIdEquipoCabeceraOrdenTrabajo = EQU.cIdEquipo AND DOC.cIdEmpresa = EQU.cIdEmpresa " &
        '                                           "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND (DOC.bEstadoRegistroCabeceraOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                           "      AND DOC.cIdEmpresa = '" & IdEmpresa & "' " &
        '                                           "ORDER BY DOC.cIdTipoDocumentoCabeceraOrdenTrabajo, DOC.vIdNumeroSerieCabeceraOrdenTrabajo, DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo",
        '                                           "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "")
        'Dim query = "SELECT DOC.cIdTipoDocumentoCabeceraOrdenTrabajo, DOC.vIdNumeroSerieCabeceraOrdenTrabajo, DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo, " &
        '                                           "DOC.cIdEmpresa, DOC.cIdEquipoSAPCabeceraOrdenTrabajo, DOC.vIdArticuloSAPCabeceraOrdenTrabajo, DOC.dFechaTransaccionCabeceraOrdenTrabajo, " &
        '                                           "CLI.vIdClienteSAPCliente, DOC.cIdClienteCabeceraOrdenTrabajo, DOC.cIdEquipoCabeceraOrdenTrabajo, EQU.vNumeroSerieEquipo, " &
        '                                           "DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, DOC.cEstadoCabeceraOrdenTrabajo, DOC.dFechaEmisionCabeceraOrdenTrabajo, " &
        '                                           "DOC.bEstadoRegistroCabeceraOrdenTrabajo, DOC.cIdUsuarioCreacionCabeceraOrdenTrabajo, " &
        '                                           "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo, DOC.cIdTipoMantenimientoCabeceraOrdenTrabajo, " &
        '                                           "COLA.vIdOrdenTrabajoReferencialColaInforme , DOC.dFechaInicioPlanificadaCabeceraOrdenTrabajo, DOC.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, DOC.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo " &
        '                                           "FROM LOGI_CABECERAORDENTRABAJO AS DOC LEFT JOIN GNRL_CLIENTE AS CLI ON " &
        '                                           "     DOC.cIdClienteCabeceraOrdenTrabajo = CLI.cIdCliente AND DOC.cIdEmpresa = CLI.cIdEmpresa " &
        '                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
        '                                           "     DOC.cIdEquipoCabeceraOrdenTrabajo = EQU.cIdEquipo AND DOC.cIdEmpresa = EQU.cIdEmpresa " &
        '                                           "     LEFT JOIN LOGI_COLAINFORME AS COLA ON DOC.cIdEmpresa = COLA.cIdEmpresa AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo = COLA.vIdOrdenTrabajoReferencialColaInforme AND COLA.cEstadoColaInforme = 'T' " &
        '                                           "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND (DOC.bEstadoRegistroCabeceraOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                           "      AND DOC.cIdEmpresa = '" & IdEmpresa & "' " &
        '                                           IIf(SubFiltro = "", "", SubFiltro) & " " &
        '                                           "ORDER BY DOC.cIdTipoDocumentoCabeceraOrdenTrabajo, CONVERT(INT, DOC.vIdNumeroSerieCabeceraOrdenTrabajo) DESC, CONVERT(INT, DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo) DESC"


        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT DOC.cIdTipoDocumentoCabeceraOrdenTrabajo, DOC.vIdNumeroSerieCabeceraOrdenTrabajo, DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo, " &
                                                   "DOC.cIdEmpresa, DOC.cIdEquipoSAPCabeceraOrdenTrabajo, DOC.vIdArticuloSAPCabeceraOrdenTrabajo, DOC.dFechaTransaccionCabeceraOrdenTrabajo, " &
                                                   "CLI.vIdClienteSAPCliente, DOC.cIdClienteCabeceraOrdenTrabajo, DOC.cIdEquipoCabeceraOrdenTrabajo, EQU.vNumeroSerieEquipo, " &
                                                   "DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, DOC.cEstadoCabeceraOrdenTrabajo, DOC.dFechaEmisionCabeceraOrdenTrabajo, " &
                                                   "DOC.bEstadoRegistroCabeceraOrdenTrabajo, DOC.cIdUsuarioCreacionCabeceraOrdenTrabajo, " &
                                                   "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo, DOC.cIdTipoMantenimientoCabeceraOrdenTrabajo, " &
                                                   "COLA.vIdOrdenTrabajoReferencialColaInforme , DOC.dFechaInicioPlanificadaCabeceraOrdenTrabajo, DOC.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, DOC.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo " &
                                                   "FROM LOGI_CABECERAORDENTRABAJO AS DOC LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     DOC.cIdClienteCabeceraOrdenTrabajo = CLI.cIdCliente AND DOC.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                   "     DOC.cIdEquipoCabeceraOrdenTrabajo = EQU.cIdEquipo AND DOC.cIdEmpresa = EQU.cIdEmpresa " &
                                                   "     LEFT JOIN LOGI_COLAINFORME AS COLA ON DOC.cIdEmpresa = COLA.cIdEmpresa AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo = COLA.vIdOrdenTrabajoReferencialColaInforme AND COLA.cEstadoColaInforme = 'T' " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND (DOC.bEstadoRegistroCabeceraOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                   "      AND DOC.cIdEmpresa = '" & IdEmpresa & "' " &
                                                   IIf(SubFiltro = "", "", SubFiltro) & " " &
                                                   "ORDER BY DOC.cIdTipoDocumentoCabeceraOrdenTrabajo, CONVERT(INT, DOC.vIdNumeroSerieCabeceraOrdenTrabajo) DESC, CONVERT(INT, DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo) DESC",
                                                   "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "")

        Dim Coleccion As New List(Of VI_LOGI_CABECERAORDENTRABAJO)
        For Each Busqueda In Consulta
            Dim BuscarOrdTra As New VI_LOGI_CABECERAORDENTRABAJO
            BuscarOrdTra.IdTipoDocumento = Busqueda.cIdTipoDocumentoCabeceraOrdenTrabajo
            BuscarOrdTra.IdNumeroSerie = Busqueda.vIdNumeroSerieCabeceraOrdenTrabajo
            BuscarOrdTra.IdNumeroCorrelativo = Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo
            BuscarOrdTra.IdEquipo = Busqueda.cIdEquipoCabeceraOrdenTrabajo
            BuscarOrdTra.IdCliente = Busqueda.cIdClienteCabeceraOrdenTrabajo
            BuscarOrdTra.RazonSocial = Busqueda.vRazonSocialCliente
            BuscarOrdTra.IdEquipoSAP = Busqueda.cIdEquipoSAPCabeceraOrdenTrabajo
            BuscarOrdTra.IdClienteSAP = Busqueda.vIdClienteSAPCliente
            BuscarOrdTra.NumeroSerieEquipo = Busqueda.vNumeroSerieEquipo
            BuscarOrdTra.StatusOrdenTrabajo = Busqueda.cEstadoCabeceraOrdenTrabajo
            BuscarOrdTra.Estado = Busqueda.bEstadoRegistroCabeceraOrdenTrabajo
            BuscarOrdTra.DescripcionEquipo = Busqueda.vDescripcionEquipo
            BuscarOrdTra.RucCliente = Busqueda.vRucCliente
            BuscarOrdTra.IdArticuloSAPCabecera = Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo
            BuscarOrdTra.FechaEmision = Busqueda.dFechaEmisionCabeceraOrdenTrabajo
            BuscarOrdTra.IdTipoMantenimiento = Busqueda.cIdTipoMantenimientoCabeceraOrdenTrabajo
            BuscarOrdTra.NroOrdenTrabajoReferencial = IIf(IsNothing(Busqueda.vIdOrdenTrabajoReferencialColaInforme), "", Busqueda.vIdOrdenTrabajoReferencialColaInforme)
            BuscarOrdTra.FechaInicio = Busqueda.dFechaInicioPlanificadaCabeceraOrdenTrabajo
            BuscarOrdTra.FechaTermino = Busqueda.dFechaTerminoPlanificadaCabeceraOrdenTrabajo
            BuscarOrdTra.IdNumeroCabeceraCheckListPlantilla = Busqueda.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo
            Coleccion.Add(BuscarOrdTra)
        Next
        Return Coleccion
    End Function

    'Public Function OrdenTrabajoListaGridComponentes(ByVal Filtro As String, ByVal Buscar As String, ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As List(Of VI_LOGI_EQUIPO)
    '    'Este si puede devolver una colección de datos es decir varios registros
    '    'Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT EQU.vDescripcionEquipo, EQU.bEstadoRegistroEquipo, EQU.cIdEnlaceEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaEquipo, EQU.cIdSistemaFuncionalEquipo, EQU.cIdEnlaceCatalogo, EQU.vObservacionEquipo, EQU.dFechaTransaccionEquipo, EQU.nVidaUtilEquipo, EQU.vIdEquivalenciaEquipo, " &
    '    '                                           "EQU.vNumeroSerieEquipo, EQU.dFechaRegistroTarjetaSAPEquipo, CLI.cIdCliente, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoEquipo, 'R') AS cEstadoEquipo " &
    '    '                                           "FROM LOGI_EQUIPO AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
    '    '                                           "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
    '    '                                           "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroEquipo = 1 " &
    '    '                                           "      AND EQU.cIdJerarquiaCatalogo = '" & Jerarquia & "' " &
    '    '                                           "ORDER BY EQU.dFechaRegistroTarjetaSAPEquipo DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalEquipo",
    '    '                                           "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "")
    '    Dim dsConsulta = OrdenTrabajoGetData("SELECT EQU.dFechaUltimaModificacionEquipo, EQU.vDescripcionEquipo, EQU.bEstadoRegistroEquipo, EQU.cIdEnlaceEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaEquipo, EQU.cIdSistemaFuncionalEquipo, EQU.cIdEnlaceCatalogo, EQU.vObservacionEquipo, EQU.dFechaTransaccionEquipo, EQU.nVidaUtilEquipo, EQU.vIdEquivalenciaEquipo, " &
    '                                         "EQU.vNumeroSerieEquipo, EQU.dFechaRegistroTarjetaSAPEquipo, CLI.cIdCliente, EQU.vIdClienteSAPEquipo, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoEquipo, 'R') AS cEstadoEquipo, " &
    '                                         "SUM(ISNULL(CHKLIS.nTotalMinutosTrabajadosCheckListOrdenTrabajo, 0)) AS nTotalMinutosTrabajadosCheckListOrdenTrabajo " &
    '                                         "FROM LOGI_EQUIPO AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
    '                                         "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
    '                                         "     LEFT JOIN LOGI_CHECKLISTORDENTRABAJO AS CHKLIS ON " &
    '                                         "     EQU.cIdEquipo = CHKLIS.cIdEquipoCheckListOrdenTrabajo AND EQU.cIdEmpresa = CHKLIS.cIdEmpresa AND " &
    '                                         "     CHKLIS.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND CHKLIS.vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSerie & "' AND CHKLIS.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND CHKLIS.cIdEmpresa = '" & IdEmpresa & "' " &
    '                                         "     " &
    '                                         "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroEquipo = 1 " &
    '                                         "GROUP BY EQU.dFechaUltimaModificacionEquipo, EQU.vDescripcionEquipo, EQU.bEstadoRegistroEquipo, EQU.cIdEnlaceEquipo, " &
    '                                         "         EQU.cIdJerarquiaCatalogo, EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaEquipo, " &
    '                                         "         EQU.cIdSistemaFuncionalEquipo, EQU.cIdEnlaceCatalogo, EQU.vObservacionEquipo, EQU.dFechaTransaccionEquipo, " &
    '                                         "         EQU.nVidaUtilEquipo, EQU.vIdEquivalenciaEquipo, EQU.vNumeroSerieEquipo, EQU.dFechaRegistroTarjetaSAPEquipo, " &
    '                                         "         CLI.cIdCliente, EQU.vIdClienteSAPEquipo, CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.cEstadoEquipo " &
    '                                         "ORDER BY EQU.dFechaRegistroTarjetaSAPEquipo DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalEquipo")
    '    Dim Coleccion As New List(Of VI_LOGI_EQUIPO)
    '    For Each Comp In dsConsulta.Rows
    '        Dim BuscarMaeAct As New VI_LOGI_EQUIPO
    '        BuscarMaeAct.Codigo = Comp("cIdEquipo")
    '        BuscarMaeAct.Descripcion = Comp("vDescripcionEquipo")
    '        BuscarMaeAct.Estado = Comp("bEstadoRegistroEquipo")
    '        BuscarMaeAct.IdTipoActivo = Comp("cIdTipoActivo")
    '        BuscarMaeAct.IdSistemaFuncional = Comp("cIdSistemaFuncionalEquipo")
    '        BuscarMaeAct.DescripcionAbreviada = Comp("vDescripcionAbreviadaEquipo")
    '        BuscarMaeAct.IdCatalogo = Comp("cIdCatalogo")
    '        BuscarMaeAct.FechaTransaccion = Comp("dFechaTransaccionEquipo")
    '        BuscarMaeAct.IdJerarquiaCatalogo = Convert.ToChar(Comp("cIdJerarquiaCatalogo"))
    '        BuscarMaeAct.FechaRegistroTarjetaSAP = Comp("dFechaRegistroTarjetaSAPEquipo")
    '        BuscarMaeAct.IdCliente = Comp("cIdCliente")
    '        BuscarMaeAct.IdClienteSAPCliente = Comp("vIdClienteSAPEquipo")
    '        BuscarMaeAct.NumeroSerieEquipo = Comp("vNumeroSerieEquipo")
    '        BuscarMaeAct.RazonSocialCliente = Comp("vRazonSocialCliente")
    '        BuscarMaeAct.RucCliente = Comp("vRucCliente")
    '        BuscarMaeAct.FechaUltimaModificacion = Comp("dFechaUltimaModificacionEquipo")
    '        BuscarMaeAct.StatusEquipo = Convert.ToChar(Comp("cEstadoEquipo"))
    '        BuscarMaeAct.TotalSegundosTrabajados = Comp("nTotalMinutosTrabajadosCheckListOrdenTrabajo")
    '        Coleccion.Add(BuscarMaeAct)
    '        'clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "", "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
    '    Next
    '    Return Coleccion
    'End Function


    'Public Function OrdenTrabajoInsertaDetalle(ByVal DetalleOrdenTrabajo As List(Of LOGI_CABECERAORDENTRABAJO), Optional ByVal strNroEnlaceOrdenTrabajo As String = "", Optional ByVal strNroEnlaceCatalogo As String = "") As Int32
    Public Function OrdenTrabajoInsertaDetalle(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal DetalleOrdenTrabajo As List(Of LOGI_DETALLEORDENTRABAJO), ByVal CheckListOrdenTrabajo As List(Of LOGI_CHECKLISTORDENTRABAJO), ByVal RecursosHumanosOrdenTrabajo As List(Of LOGI_RECURSOSORDENTRABAJO), ByVal ComponenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa, OrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdEquipoSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdTipoMantenimientoCabeceraOrdenTrabajo, OrdenTrabajo.dFechaTransaccionCabeceraOrdenTrabajo,
                                           OrdenTrabajo.dFechaEmisionCabeceraOrdenTrabajo, OrdenTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionInicialCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionFinalCabeceraOrdenTrabajo,
                                           OrdenTrabajo.cIdClienteCabeceraOrdenTrabajo, OrdenTrabajo.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cEstadoCabeceraOrdenTrabajo, OrdenTrabajo.bEstadoRegistroCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vContratoReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cIdUsuarioCreacionCabeceraOrdenTrabajo, OrdenTrabajo.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo,
                                           OrdenTrabajo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo, OrdenTrabajo.nPeriodicidadDiasCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            For Each Busqueda In DetalleOrdenTrabajo
                If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENTRABAJO WITH (NOLOCK) " &
                                       "WHERE " &
                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda.cIdEmpresa & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda.cIdEquipoCabeceraOrdenTrabajo & "' AND " &
                                       "cIdCatalogoCheckListDetalleOrdenTrabajo = '" & Busqueda.cIdCatalogoCheckListDetalleOrdenTrabajo & "' AND " &
                                       "cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo = '" & Busqueda.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo & "' AND cIdActividadCheckListDetalleOrdenTrabajo = '" & Busqueda.cIdActividadCheckListDetalleOrdenTrabajo & "' AND " &
                                       "vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                                       "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count = 0 Then
                    'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
                    'If OrdenTrabajoGetData("SELECT * FROM LOGI_DETALLEORDENTRABAJO WITH (NOLOCK) " &
                    '                   "WHERE " &
                    '                   "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                    '                   "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda.cIdEmpresa & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda.cIdEquipoCabeceraOrdenTrabajo & "' AND " &
                    '                   "cIdCatalogoCheckListDetalleOrdenTrabajo = '" & Busqueda.cIdCatalogoCheckListDetalleOrdenTrabajo & "' AND " &
                    '                   "cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo = '" & Busqueda.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo & "' AND cIdActividadCheckListDetalleOrdenTrabajo = '" & Busqueda.cIdActividadCheckListDetalleOrdenTrabajo & "' AND " &
                    '                   "vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo & "'").Rows.Count = 0 Then
                    x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                             Busqueda.cIdEmpresa, Busqueda.nIdNumeroItemDetalleOrdenTrabajo, Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                             Busqueda.cIdEquipoCabeceraOrdenTrabajo, Busqueda.vIdArticuloSAPDetalleOrdenTrabajo, Busqueda.vDescripcionArticuloDetalleOrdenTrabajo,
                                                             Busqueda.nCantidadArticuloDetalleOrdenTrabajo, Busqueda.vDescripcionUnidadMedidaDetalleOrdenTrabajo, Busqueda.cIdCatalogoCheckListDetalleOrdenTrabajo,
                                                             Busqueda.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo, Busqueda.cIdActividadCheckListDetalleOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_UPDATE", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                             Busqueda.cIdEmpresa, Busqueda.nIdNumeroItemDetalleOrdenTrabajo, Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                             Busqueda.cIdEquipoCabeceraOrdenTrabajo, Busqueda.vIdArticuloSAPDetalleOrdenTrabajo, Busqueda.vDescripcionArticuloDetalleOrdenTrabajo,
                                                             Busqueda.nCantidadArticuloDetalleOrdenTrabajo, Busqueda.vDescripcionUnidadMedidaDetalleOrdenTrabajo, Busqueda.cIdCatalogoCheckListDetalleOrdenTrabajo,
                                                             Busqueda.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo, Busqueda.cIdActividadCheckListDetalleOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString

                End If
            Next
            For Each Busqueda1 In CheckListOrdenTrabajo
                If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WITH (NOLOCK) " &
                                                       "WHERE " &
                                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda1.cIdEmpresa & "' AND " &
                                                       "cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda1.cIdEquipoCabeceraOrdenTrabajo & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Busqueda1.cIdCatalogoCheckListOrdenTrabajo & "' AND " &
                                                       "cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Busqueda1.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "' AND cIdActividadCheckListOrdenTrabajo = '" & Busqueda1.cIdActividadCheckListOrdenTrabajo & "' AND " &
                                                       "vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda1.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                           "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count = 0 Then
                    'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
                    'If OrdenTrabajoGetData("SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WITH (NOLOCK) " &
                    '                                   "WHERE " &
                    '                                   "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                    '                                   "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda1.cIdEmpresa & "' AND " &
                    '                                   "cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda1.cIdEquipoCabeceraOrdenTrabajo & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Busqueda1.cIdCatalogoCheckListOrdenTrabajo & "' AND " &
                    '                                   "cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Busqueda1.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "' AND cIdActividadCheckListOrdenTrabajo = '" & Busqueda1.cIdActividadCheckListOrdenTrabajo & "' AND " &
                    '                                   "vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda1.vIdArticuloSAPCabeceraOrdenTrabajo & "'").Rows.Count = 0 Then
                    x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                   Busqueda1.cIdEmpresa, Busqueda1.cIdEquipoCabeceraOrdenTrabajo, Busqueda1.cIdEquipoCheckListOrdenTrabajo, Busqueda1.cIdCatalogoCheckListOrdenTrabajo, Busqueda1.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                                   Busqueda1.cIdActividadCheckListOrdenTrabajo, Busqueda1.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda1.nIdNumeroItemCheckListOrdenTrabajo,
                                                                   Busqueda1.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, Busqueda1.cIdTipoMantenimientoCheckListOrdenTrabajo, Busqueda1.vObservacionCheckListOrdenTrabajo,
                                                                   Busqueda1.cEstadoCheckListOrdenTrabajo, Busqueda1.dFechaInicioCheckListOrdenTrabajo, Busqueda1.dFechaFinalCheckListOrdenTrabajo, Busqueda1.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                                   Busqueda1.cEstadoActividadCheckListOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_UPDATE", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                               Busqueda1.cIdEmpresa, Busqueda1.cIdEquipoCabeceraOrdenTrabajo, Busqueda1.cIdEquipoCheckListOrdenTrabajo, Busqueda1.cIdCatalogoCheckListOrdenTrabajo, Busqueda1.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                               Busqueda1.cIdActividadCheckListOrdenTrabajo, Busqueda1.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda1.nIdNumeroItemCheckListOrdenTrabajo,
                                                               Busqueda1.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, Busqueda1.cIdTipoMantenimientoCheckListOrdenTrabajo, Busqueda1.vObservacionCheckListOrdenTrabajo,
                                                               Busqueda1.cEstadoCheckListOrdenTrabajo, Busqueda1.dFechaInicioCheckListOrdenTrabajo, Busqueda1.dFechaFinalCheckListOrdenTrabajo, Busqueda1.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                               Busqueda1.cEstadoActividadCheckListOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                End If
            Next
            For Each Busqueda2 In RecursosHumanosOrdenTrabajo
                If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_RECURSOSORDENTRABAJO WITH (NOLOCK) " &
                                       "WHERE " &
                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda2.cIdEmpresa & "' AND " &
                                       "cIdPersonal = '" & Busqueda2.cIdPersonal & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda2.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                           "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count = 0 Then
                    'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
                    'If OrdenTrabajoGetData("SELECT * FROM LOGI_RECURSOSORDENTRABAJO WITH (NOLOCK) " &
                    '                   "WHERE " &
                    '                   "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                    '                   "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda2.cIdEmpresa & "' AND " &
                    '                   "cIdPersonal = '" & Busqueda2.cIdPersonal & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda2.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo & "'").Rows.Count = 0 Then
                    x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda2.cIdEmpresa, Busqueda2.cIdPersonal, Busqueda2.vObservacionRecursosOrdenTrabajo, Busqueda2.bEstadoRegistroRecursosOrdenTrabajo,
                                                              Busqueda2.cIdEquipoCabeceraOrdenTrabajo, Busqueda2.nTotalMinutosTrabajadosRecursosOrdenTrabajo, Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                              Busqueda2.bResponsableRecursosOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_UPDATE", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda2.cIdEmpresa, Busqueda2.cIdPersonal, Busqueda2.vObservacionRecursosOrdenTrabajo, Busqueda2.bEstadoRegistroRecursosOrdenTrabajo,
                                                              Busqueda2.cIdEquipoCabeceraOrdenTrabajo, Busqueda2.nTotalMinutosTrabajadosRecursosOrdenTrabajo, Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                              Busqueda2.bResponsableRecursosOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                End If
            Next
            For Each Busqueda3 In ComponenteOrdenTrabajo
                If Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO WITH (NOLOCK) " &
                                       "WHERE " &
                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda3.cIdEmpresa & "' AND " &
                                       "cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda3.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo & "' AND " &
                                       "cIdEquipoComponenteOrdenTrabajo = '" & Busqueda3.cIdEquipoComponenteOrdenTrabajo & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Busqueda3.cIdCatalogoComponenteOrdenTrabajo & "' AND " &
                                       "cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo & "' AND nIdNumeroItemComponenteOrdenTrabajo = '" & Busqueda3.nIdNumeroItemComponenteOrdenTrabajo & "'",
                           "", "", "", "", "", "", "", "", "", "", "1", Now, Now, 0, 0, "").Count = 0 Then
                    'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes
                    'If OrdenTrabajoGetData("SELECT * FROM LOGI_RECURSOSORDENTRABAJO WITH (NOLOCK) " &
                    '                   "WHERE " &
                    '                   "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                    '                   "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda2.cIdEmpresa & "' AND " &
                    '                   "cIdPersonal = '" & Busqueda2.cIdPersonal & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda2.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda2.vIdArticuloSAPCabeceraOrdenTrabajo & "'").Rows.Count = 0 Then
                    x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda3.cIdEmpresa, Busqueda3.cIdEquipoCabeceraOrdenTrabajo, Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda3.cIdEquipoComponenteOrdenTrabajo,
                                                              Busqueda3.cIdCatalogoComponenteOrdenTrabajo, Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo, Busqueda3.vObservacionComponenteOrdenTrabajo,
                                                              Busqueda3.cEstadoComponenteOrdenTrabajo, Busqueda3.dFechaInicioActividadComponenteOrdenTrabajo, Busqueda3.dFechaFinalActividadComponenteOrdenTrabajo,
                                                              Busqueda3.nIdNumeroItemComponenteOrdenTrabajo, Busqueda3.nTotalSegundosTrabajadosComponenteOrdenTrabajo,
                                                              OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_UPDATE", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda3.cIdEmpresa, Busqueda3.cIdEquipoCabeceraOrdenTrabajo, Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda3.cIdEquipoComponenteOrdenTrabajo,
                                                              Busqueda3.cIdCatalogoComponenteOrdenTrabajo, Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo, Busqueda3.vObservacionComponenteOrdenTrabajo,
                                                              Busqueda3.cEstadoComponenteOrdenTrabajo, Busqueda3.dFechaInicioActividadComponenteOrdenTrabajo, Busqueda3.dFechaFinalActividadComponenteOrdenTrabajo,
                                                              Busqueda3.nIdNumeroItemComponenteOrdenTrabajo, Busqueda3.nTotalSegundosTrabajadosComponenteOrdenTrabajo,
                                                              OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                End If
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function OrdenTrabajoDeleteAndInsertComponentes(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO, ByVal CheckListOrdenTrabajo As List(Of LOGI_CHECKLISTORDENTRABAJO), ByVal ComponenteOrdenTrabajo As List(Of LOGI_COMPONENTEORDENTRABAJO)) As Int32
        'ELIMINAMOS EL CHECKLIST ( componentes y actividades)
        Dim x = Data.PA_LOGI_MNT_DELETEORDENTRABAJO_COMPANDCHECKLIST(OrdenTrabajo.cIdEmpresa, OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo)

        'x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo,
        '                                   OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa, OrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo,
        '                                   OrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdEquipoSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdTipoMantenimientoCabeceraOrdenTrabajo, OrdenTrabajo.dFechaTransaccionCabeceraOrdenTrabajo,
        '                                   OrdenTrabajo.dFechaEmisionCabeceraOrdenTrabajo, OrdenTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionInicialCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionFinalCabeceraOrdenTrabajo,
        '                                   OrdenTrabajo.cIdClienteCabeceraOrdenTrabajo, OrdenTrabajo.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cEstadoCabeceraOrdenTrabajo, OrdenTrabajo.bEstadoRegistroCabeceraOrdenTrabajo,
        '                                   OrdenTrabajo.vContratoReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cIdUsuarioCreacionCabeceraOrdenTrabajo, OrdenTrabajo.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo,
        '                                   OrdenTrabajo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)

            For Each Busqueda1 In CheckListOrdenTrabajo
                If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTORDENTRABAJO WITH (NOLOCK) " &
                                                       "WHERE " &
                                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda1.cIdEmpresa & "' AND " &
                                                       "cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda1.cIdEquipoCabeceraOrdenTrabajo & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Busqueda1.cIdCatalogoCheckListOrdenTrabajo & "' AND " &
                                                       "cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Busqueda1.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "' AND cIdActividadCheckListOrdenTrabajo = '" & Busqueda1.cIdActividadCheckListOrdenTrabajo & "' AND " &
                                                       "vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda1.vIdArticuloSAPCabeceraOrdenTrabajo & "'",
                           "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count = 0 Then

                    x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                                   Busqueda1.cIdEmpresa, Busqueda1.cIdEquipoCabeceraOrdenTrabajo, Busqueda1.cIdEquipoCheckListOrdenTrabajo, Busqueda1.cIdCatalogoCheckListOrdenTrabajo, Busqueda1.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                                   Busqueda1.cIdActividadCheckListOrdenTrabajo, Busqueda1.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda1.nIdNumeroItemCheckListOrdenTrabajo,
                                                                   Busqueda1.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, Busqueda1.cIdTipoMantenimientoCheckListOrdenTrabajo, Busqueda1.vObservacionCheckListOrdenTrabajo,
                                                                   Busqueda1.cEstadoCheckListOrdenTrabajo, Busqueda1.dFechaInicioCheckListOrdenTrabajo, Busqueda1.dFechaFinalCheckListOrdenTrabajo, Busqueda1.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                                   Busqueda1.cEstadoActividadCheckListOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO("SQL_UPDATE", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                               Busqueda1.cIdEmpresa, Busqueda1.cIdEquipoCabeceraOrdenTrabajo, Busqueda1.cIdEquipoCheckListOrdenTrabajo, Busqueda1.cIdCatalogoCheckListOrdenTrabajo, Busqueda1.cIdJerarquiaCatalogoCheckListOrdenTrabajo,
                                                               Busqueda1.cIdActividadCheckListOrdenTrabajo, Busqueda1.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda1.nIdNumeroItemCheckListOrdenTrabajo,
                                                               Busqueda1.cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo, Busqueda1.cIdTipoMantenimientoCheckListOrdenTrabajo, Busqueda1.vObservacionCheckListOrdenTrabajo,
                                                               Busqueda1.cEstadoCheckListOrdenTrabajo, Busqueda1.dFechaInicioCheckListOrdenTrabajo, Busqueda1.dFechaFinalCheckListOrdenTrabajo, Busqueda1.nTotalSegundosTrabajadosCheckListOrdenTrabajo,
                                                               Busqueda1.cEstadoActividadCheckListOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                End If
            Next

            For Each Busqueda3 In ComponenteOrdenTrabajo
                If Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO WITH (NOLOCK) " &
                                       "WHERE " &
                                       "cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND " &
                                       "vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Busqueda3.cIdEmpresa & "' AND " &
                                       "cIdEquipoCabeceraOrdenTrabajo = '" & Busqueda3.cIdEquipoCabeceraOrdenTrabajo & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo & "' AND " &
                                       "cIdEquipoComponenteOrdenTrabajo = '" & Busqueda3.cIdEquipoComponenteOrdenTrabajo & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Busqueda3.cIdCatalogoComponenteOrdenTrabajo & "' AND " &
                                       "cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo & "' AND nIdNumeroItemComponenteOrdenTrabajo = '" & Busqueda3.nIdNumeroItemComponenteOrdenTrabajo & "'",
                           "", "", "", "", "", "", "", "", "", "", "1", Now, Now, 0, 0, "").Count = 0 Then

                    x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda3.cIdEmpresa, Busqueda3.cIdEquipoCabeceraOrdenTrabajo, Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda3.cIdEquipoComponenteOrdenTrabajo,
                                                              Busqueda3.cIdCatalogoComponenteOrdenTrabajo, Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo, Busqueda3.vObservacionComponenteOrdenTrabajo,
                                                              Busqueda3.cEstadoComponenteOrdenTrabajo, Busqueda3.dFechaInicioActividadComponenteOrdenTrabajo, Busqueda3.dFechaFinalActividadComponenteOrdenTrabajo,
                                                              Busqueda3.nIdNumeroItemComponenteOrdenTrabajo, Busqueda3.nTotalSegundosTrabajadosComponenteOrdenTrabajo,
                                                              OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_UPDATE", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              Busqueda3.cIdEmpresa, Busqueda3.cIdEquipoCabeceraOrdenTrabajo, Busqueda3.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda3.cIdEquipoComponenteOrdenTrabajo,
                                                              Busqueda3.cIdCatalogoComponenteOrdenTrabajo, Busqueda3.cIdJerarquiaCatalogoComponenteOrdenTrabajo, Busqueda3.vObservacionComponenteOrdenTrabajo,
                                                              Busqueda3.cEstadoComponenteOrdenTrabajo, Busqueda3.dFechaInicioActividadComponenteOrdenTrabajo, Busqueda3.dFechaFinalActividadComponenteOrdenTrabajo,
                                                              Busqueda3.nIdNumeroItemComponenteOrdenTrabajo, Busqueda3.nTotalSegundosTrabajadosComponenteOrdenTrabajo,
                                                              OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
                End If
            Next
            scope.Complete()
            Return x
        End Using
    End Function


    Public Function QueryRecursosOT(ByRef query As String) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_NONE", query, "", "", "",
                                                              "", "", "", False,
                                                              "", 0, "",
                                                              False, "").ReturnValue.ToString
        Return x
    End Function

    Public Function InsertRecursosOT(ByRef recursoOT As LOGI_RECURSOSORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_INSERT", "", recursoOT.cIdTipoDocumentoCabeceraOrdenTrabajo, recursoOT.vIdNumeroSerieCabeceraOrdenTrabajo, recursoOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              recursoOT.cIdEmpresa, recursoOT.cIdPersonal, recursoOT.vObservacionRecursosOrdenTrabajo, recursoOT.bEstadoRegistroRecursosOrdenTrabajo,
                                                              recursoOT.cIdEquipoCabeceraOrdenTrabajo, recursoOT.nTotalMinutosTrabajadosRecursosOrdenTrabajo, recursoOT.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                              recursoOT.bResponsableRecursosOrdenTrabajo, recursoOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function UpdateRecursosOT(ByRef recursoOT As LOGI_RECURSOSORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_RECURSOSORDENTRABAJO("SQL_UPDATE", "", recursoOT.cIdTipoDocumentoCabeceraOrdenTrabajo, recursoOT.vIdNumeroSerieCabeceraOrdenTrabajo, recursoOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                                                              recursoOT.cIdEmpresa, recursoOT.RRHH_PERSONAL.cIdPersonal, recursoOT.vObservacionRecursosOrdenTrabajo, recursoOT.bEstadoRegistroRecursosOrdenTrabajo,
                                                              recursoOT.cIdEquipoCabeceraOrdenTrabajo, recursoOT.nTotalMinutosTrabajadosRecursosOrdenTrabajo, recursoOT.vIdArticuloSAPCabeceraOrdenTrabajo,
                                                              recursoOT.bResponsableRecursosOrdenTrabajo, recursoOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenTrabajoInserta(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_INSERT", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa, OrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdEquipoSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdTipoMantenimientoCabeceraOrdenTrabajo, OrdenTrabajo.dFechaTransaccionCabeceraOrdenTrabajo,
                                           OrdenTrabajo.dFechaEmisionCabeceraOrdenTrabajo, OrdenTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionInicialCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionFinalCabeceraOrdenTrabajo,
                                           OrdenTrabajo.cIdClienteCabeceraOrdenTrabajo, OrdenTrabajo.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cEstadoCabeceraOrdenTrabajo, OrdenTrabajo.bEstadoRegistroCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vContratoReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cIdUsuarioCreacionCabeceraOrdenTrabajo, OrdenTrabajo.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo,
                                           OrdenTrabajo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo, OrdenTrabajo.nPeriodicidadDiasCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenTrabajoEdita(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_UPDATE", "", OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, OrdenTrabajo.cIdEmpresa, OrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdEquipoSAPCabeceraOrdenTrabajo, OrdenTrabajo.cIdTipoMantenimientoCabeceraOrdenTrabajo, OrdenTrabajo.dFechaTransaccionCabeceraOrdenTrabajo,
                                           OrdenTrabajo.dFechaEmisionCabeceraOrdenTrabajo, OrdenTrabajo.dFechaInicioPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionInicialCabeceraOrdenTrabajo, OrdenTrabajo.dFechaEjecucionFinalCabeceraOrdenTrabajo,
                                           OrdenTrabajo.cIdClienteCabeceraOrdenTrabajo, OrdenTrabajo.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cEstadoCabeceraOrdenTrabajo, OrdenTrabajo.bEstadoRegistroCabeceraOrdenTrabajo,
                                           OrdenTrabajo.vContratoReferenciaCabeceraOrdenTrabajo, OrdenTrabajo.cIdUsuarioCreacionCabeceraOrdenTrabajo, OrdenTrabajo.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo,
                                           OrdenTrabajo.dFechaTerminoPlanificadaCabeceraOrdenTrabajo, OrdenTrabajo.cIdTipoControlTiempoCabeceraOrdenTrabajo, OrdenTrabajo.nPeriodicidadDiasCabeceraOrdenTrabajo, OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenTrabajoElimina(ByVal OrdenTrabajo As LOGI_CABECERAORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "UPDATE LOGI_CABECERAORDENTRABAJO SET bEstadoRegistroCabeceraOrdenTrabajo = 0 WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & OrdenTrabajo.cIdEmpresa & "' ",
                                           "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenTrabajoExiste(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As Boolean
        If Data.PA_LOGI_MNT_CABECERAORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENTRABAJO " &
                                                     "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                      " AND bEstadoRegistroCabeceraOrdenTrabajo = '1'",
                                      "", "", "", "", "", "", "", "", Now, Now, Now, Now, Now, "", "", "", "1", "", "", "", Now, "", 0, "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CheckListInsertaActividad(ByVal cIdTipoDocumento As String,
         ByVal vIdNumeroSerie As String,
         ByVal vIdNumeroCorrelativo As String,
         ByVal vObservacion As String,
         ByVal cIdEmpresa As String,
         ByVal cEstado As System.Nullable(Of Char),
         ByVal nTotalSegundosTrabajados As System.Nullable(Of Integer),
         ByVal dFechaInicio As System.Nullable(Of Date),
         ByVal dFechaFinal As System.Nullable(Of Date),
         ByVal cIdEquipo As String,
         ByVal cIdActividad As String,
         ByVal cIdTipoMantenimiento As String,
         ByVal cIdNumeroCabeceraCheckListPlantilla As String,
         ByVal cIdCatalogo As String,
         ByVal cIdJerarquiaCatalogo As System.Nullable(Of Char),
         ByVal vIdArticuloSAPCabecera As String,
         ByVal cEstadoActividad As System.Nullable(Of Char),
         ByVal cIdEquipoCheckList As String) As Int32
        Dim x = Data.PA_LOGI_MNT_CHECKLISTORDENTRABAJO_INSERTACTIVIDAD(cIdTipoDocumento, vIdNumeroSerie, vIdNumeroCorrelativo, vObservacion, cIdEmpresa,
                                                                       cEstado, nTotalSegundosTrabajados, dFechaInicio, dFechaFinal, cIdEquipo, cIdActividad,
                                                                       cIdTipoMantenimiento, cIdNumeroCabeceraCheckListPlantilla, cIdCatalogo, cIdJerarquiaCatalogo,
                                                                       vIdArticuloSAPCabecera, cEstadoActividad, cIdEquipoCheckList)
        Return x
    End Function

    Public Sub EjecutarComando(query As String, parametros As List(Of SqlParameter))
        Dim constr As String = My.Settings.CMMSConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query, con)
                cmd.CommandType = CommandType.Text
                If parametros IsNot Nothing Then
                    cmd.Parameters.AddRange(parametros.ToArray())
                End If
                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub
End Class
