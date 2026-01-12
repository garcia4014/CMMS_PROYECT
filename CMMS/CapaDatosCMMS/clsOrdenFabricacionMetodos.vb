Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsOrdenFabricacionMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function OrdenFabricacionGetData(strQuery As String) As DataTable
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

    Public Function OrdenFabricacionListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENFABRICACION)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENFABRICACION " &
                                                 "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & IdTipoOrden & "' ",
                                                 "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, "", "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERAORDENFABRICACION)
        For Each OrdenFabricacion In Consulta
            Dim OrdFab As New LOGI_CABECERAORDENFABRICACION
            OrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion = OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion
            OrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion = OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion.Trim + OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion.Trim + OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion.Trim + "-" + OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion
            Coleccion.Add(OrdFab)
        Next
        Return Coleccion
    End Function

    Public Function OrdenFabricacionUnidadTrabajoListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENFABRICACION)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT vIdUnidadTrabajoOrdenFabricacion FROM LOGI_CABECERAORDENFABRICACION " &
                                                 "WHERE (cIdTipoDocumentoCabeceraOrdenFabricacion = '" & IdTipoOrden & "' OR '*' = '" & IdTipoOrden & "') " &
                                                 "GROUP BY vIdUnidadTrabajoOrdenFabricacion " &
                                                 "ORDER BY vIdUnidadTrabajoOrdenFabricacion",
                                                 "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, "", "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERAORDENFABRICACION)
        For Each OrdenFabricacion In Consulta
            Dim OrdFab As New LOGI_CABECERAORDENFABRICACION
            If IsNothing(OrdenFabricacion.vIdUnidadTrabajoOrdenFabricacion) = False Then
                OrdFab.vIdUnidadTrabajoOrdenFabricacion = OrdenFabricacion.vIdUnidadTrabajoOrdenFabricacion
            End If
            '            OrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion = OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion.Trim + OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion.Trim + OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion.Trim + "-" + OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion
            'OrdFab.vIdNumeroCorrelativoCabeceraOrdenFabricacion = OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion.Trim + OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion.Trim + OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion.Trim + "-" + OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion
            Coleccion.Add(OrdFab)
        Next
        Return Coleccion
    End Function

    'Public Function OrdenFabricacionListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENFABRICACION
    '    Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENFABRICACION WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
    '                                           " AND bEstadoRegistroCabeceraOrdenFabricacion = 1 " &
    '                                           "ORDER BY cIdCatalogo",
    '                                           "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "")
    '    Dim Coleccion As New LOGI_CABECERAORDENFABRICACION
    '    For Each LOGI_CABECERAORDENFABRICACION In Consulta
    '        Coleccion.cIdTipoDocumentoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdTipoDocumentoCabeceraOrdenFabricacion
    '        Coleccion.vIdNumeroSerieCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdNumeroSerieCabeceraOrdenFabricacion
    '        Coleccion.vIdNumeroCorrelativoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdNumeroCorrelativoCabeceraOrdenFabricacion
    '        Coleccion.cIdEmpresa = LOGI_CABECERAORDENFABRICACION.cIdEmpresa
    '        Coleccion.cIdEquipoSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdEquipoSAPCabeceraOrdenFabricacion
    '        Coleccion.vIdArticuloSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdArticuloSAPCabeceraOrdenFabricacion
    '        Coleccion.dFechaTransaccionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaTransaccionCabeceraOrdenFabricacion
    '        Coleccion.vIdClienteSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdClienteSAPCabeceraOrdenFabricacion
    '        Coleccion.cIdCliente = LOGI_CABECERAORDENFABRICACION.cIdCliente
    '        Coleccion.cIdEquipo = LOGI_CABECERAORDENFABRICACION.cIdEquipo
    '        Coleccion.vNumeroSerieEquipoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vNumeroSerieEquipoCabeceraOrdenFabricacion
    '        Coleccion.vOrdenVentaCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenVentaCabeceraOrdenFabricacion
    '        Coleccion.vOrdenCompraCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenCompraCabeceraOrdenFabricacion
    '        Coleccion.cEstadoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cEstadoCabeceraOrdenFabricacion
    '        Coleccion.bEstadoRegistroCabeceraOrdenFabricacion = IIf(LOGI_CABECERAORDENFABRICACION.bEstadoRegistroCabeceraOrdenFabricacion Is Nothing, False, LOGI_CABECERAORDENFABRICACION.bEstadoRegistroCabeceraOrdenFabricacion)
    '        Coleccion.dFechaUltimaModificacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaUltimaModificacionCabeceraOrdenFabricacion
    '        Coleccion.dFechaCreacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaCreacionCabeceraOrdenFabricacion
    '        Coleccion.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion
    '        Coleccion.cIdUsuarioCreacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdUsuarioCreacionCabeceraOrdenFabricacion
    '    Next
    '    Return Coleccion
    'End Function

    Public Function OrdenFabricacionListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENFABRICACION
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENFABRICACION WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                 " AND bEstadoRegistroCabeceraOrdenFabricacion = 1 " &
                                                 "",
                                                 "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, "", "", "", "")
        Dim Coleccion As New LOGI_CABECERAORDENFABRICACION
        For Each LOGI_CABECERAORDENFABRICACION In Consulta
            Coleccion.cIdTipoDocumentoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdTipoDocumentoCabeceraOrdenFabricacion
            Coleccion.vIdNumeroSerieCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdNumeroSerieCabeceraOrdenFabricacion
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdNumeroCorrelativoCabeceraOrdenFabricacion
            Coleccion.cIdEmpresa = LOGI_CABECERAORDENFABRICACION.cIdEmpresa
            Coleccion.cIdEquipoSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdEquipoSAPCabeceraOrdenFabricacion
            Coleccion.vIdArticuloSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdArticuloSAPCabeceraOrdenFabricacion
            Coleccion.dFechaTransaccionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaTransaccionCabeceraOrdenFabricacion
            Coleccion.vIdClienteSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdClienteSAPCabeceraOrdenFabricacion
            Coleccion.cIdCliente = LOGI_CABECERAORDENFABRICACION.cIdCliente
            Coleccion.cIdEquipo = LOGI_CABECERAORDENFABRICACION.cIdEquipo
            Coleccion.vNumeroSerieEquipoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vNumeroSerieEquipoCabeceraOrdenFabricacion
            Coleccion.vOrdenVentaCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenVentaCabeceraOrdenFabricacion
            Coleccion.vOrdenCompraCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenCompraCabeceraOrdenFabricacion
            Coleccion.vOrdenFabricacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenFabricacionCabeceraOrdenFabricacion
            Coleccion.cEstadoCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cEstadoCabeceraOrdenFabricacion
            Coleccion.bEstadoRegistroCabeceraOrdenFabricacion = IIf(LOGI_CABECERAORDENFABRICACION.bEstadoRegistroCabeceraOrdenFabricacion Is Nothing, False, LOGI_CABECERAORDENFABRICACION.bEstadoRegistroCabeceraOrdenFabricacion)
            Coleccion.dFechaUltimaModificacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaUltimaModificacionCabeceraOrdenFabricacion
            Coleccion.dFechaCreacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaCreacionCabeceraOrdenFabricacion
            Coleccion.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion
            Coleccion.cIdUsuarioCreacionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.cIdUsuarioCreacionCabeceraOrdenFabricacion
            Coleccion.dFechaEmisionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaEmisionCabeceraOrdenFabricacion
            Coleccion.vIdUnidadTrabajoOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vIdUnidadTrabajoOrdenFabricacion
            Coleccion.dFechaProgramacionAlmacenCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.dFechaProgramacionAlmacenCabeceraOrdenFabricacion
            Coleccion.vDescripcionCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vDescripcionCabeceraOrdenFabricacion
            Coleccion.vOrdenVentaSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenVentaSAPCabeceraOrdenFabricacion
            Coleccion.vOrdenCompraSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenCompraSAPCabeceraOrdenFabricacion
            Coleccion.vOrdenFabricacionSAPCabeceraOrdenFabricacion = LOGI_CABECERAORDENFABRICACION.vOrdenFabricacionSAPCabeceraOrdenFabricacion
            Coleccion.cIdPersonal = LOGI_CABECERAORDENFABRICACION.cIdPersonal
        Next
        Return Coleccion
    End Function

    Public Function OrdenFabricacionListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_LOGI_CABECERAORDENFABRICACION)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT EQU.vDescripcionOrdenFabricacion, EQU.bEstadoRegistroOrdenFabricacion, EQU.cIdEnlaceOrdenFabricacion, EQU.cIdJerarquiaCatalogo, EQU.cIdOrdenFabricacion, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaOrdenFabricacion, EQU.cIdSistemaFuncionalOrdenFabricacion, EQU.cIdEnlaceCatalogo, EQU.vObservacionOrdenFabricacion, EQU.dFechaTransaccionOrdenFabricacion, EQU.nVidaUtilOrdenFabricacion, EQU.vIdEquivalenciaOrdenFabricacion, " &
        '                                           "EQU.vNumeroSerieOrdenFabricacion, EQU.dFechaRegistroTarjetaSAPOrdenFabricacion, CLI.cIdCliente, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoOrdenFabricacion, 'R') AS cEstadoOrdenFabricacion " &
        '                                           "FROM LOGI_CABECERAORDENFABRICACION AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
        '                                           "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
        '                                           "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroCabeceraOrdenFabricacion = 1 " &
        '                                           "      AND EQU.cIdJerarquiaCatalogo = '" & Jerarquia & "' " &
        '                                           "ORDER BY EQU.dFechaRegistroTarjetaSAPOrdenFabricacion DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdOrdenFabricacion, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalOrdenFabricacion",
        '                                           "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "")
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT DOC.cIdTipoDocumentoCabeceraOrdenFabricacion, DOC.vIdNumeroSerieCabeceraOrdenFabricacion, DOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion, " &
                                                   "DOC.cIdEmpresa, DOC.cIdEquipoSAPCabeceraOrdenFabricacion, DOC.vIdArticuloSAPCabeceraOrdenFabricacion, DOC.dFechaTransaccionCabeceraOrdenFabricacion, " &
                                                   "DOC.vIdClienteSAPCabeceraOrdenFabricacion, DOC.cIdCliente, DOC.cIdEquipo, DOC.vNumeroSerieEquipoCabeceraOrdenFabricacion, " &
                                                   "DOC.vOrdenVentaCabeceraOrdenFabricacion, DOC.vOrdenCompraCabeceraOrdenFabricacion, DOC.vOrdenFabricacionCabeceraOrdenFabricacion, DOC.cEstadoCabeceraOrdenFabricacion, " &
                                                   "DOC.bEstadoRegistroCabeceraOrdenFabricacion, DOC.dFechaUltimaModificacionCabeceraOrdenFabricacion, DOC.dFechaCreacionCabeceraOrdenFabricacion, " &
                                                   "DOC.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion, DOC.cIdUsuarioCreacionCabeceraOrdenFabricacion, " &
                                                   "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo, DOC.vIdArticuloSAPCabeceraOrdenFabricacion, " &
                                                   "DOC.dFechaEmisionCabeceraOrdenFabricacion, DOC.vIdUnidadTrabajoOrdenFabricacion, DOC.dFechaProgramacionAlmacenCabeceraOrdenFabricacion, " &
                                                   "DOC.cIdPersonal, PER.vNombreCompletoPersonal, DOC.vDescripcionCabeceraOrdenFabricacion, COUNT(DETDOC.vIdArticuloSAPDetalleOrdenFabricacion) nTotalItemDetalleOrdenFabricacion, " &
                                                   "DOC.dFechaOrdenVentaCabeceraOrdenFabricacion, DOC.dFechaAsignacionAuxiliarCabeceraOrdenFabricacion, " &
                                                   "DOC.vOrdenVentaSAPCabeceraOrdenFabricacion, DOC.vOrdenCompraSAPCabeceraOrdenFabricacion, DOC.vOrdenFabricacionSAPCabeceraOrdenFabricacion " &
                                                   "FROM LOGI_CABECERAORDENFABRICACION AS DOC LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     DOC.cIdCliente = CLI.cIdCliente AND DOC.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                   "     DOC.cIdEquipo = EQU.cIdEquipo AND DOC.cIdEmpresa = EQU.cIdEmpresa " &
                                                   "     LEFT JOIN RRHH_PERSONAL AS PER ON " &
                                                   "     DOC.cIdPersonal = PER.cIdPersonal " &
                                                   "     LEFT JOIN LOGI_DETALLEORDENFABRICACION AS DETDOC ON " &
                                                   "     DOC.cIdTipoDocumentoCabeceraOrdenFabricacion = DETDOC.cIdTipoDocumentoCabeceraOrdenFabricacion " &
                                                   "     AND DOC.vIdNumeroSerieCabeceraOrdenFabricacion = DETDOC.vIdNumeroSerieCabeceraOrdenFabricacion " &
                                                   "     AND DOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion = DETDOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion " &
                                                   "     AND DOC.cIdEmpresa = DETDOC.cIdEmpresa AND DOC.cIdEquipoSAPCabeceraOrdenFabricacion = DETDOC.cIdEquipoSAPCabeceraOrdenFabricacion " &
                                                   "     AND DOC.vIdArticuloSAPCabeceraOrdenFabricacion = DETDOC.vIdArticuloSAPCabeceraOrdenFabricacion " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND (DOC.bEstadoRegistroCabeceraOrdenFabricacion = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                   "      AND DOC.cIdEmpresa = '" & IdEmpresa & "' " &
                                                   "GROUP BY DOC.cIdTipoDocumentoCabeceraOrdenFabricacion, DOC.vIdNumeroSerieCabeceraOrdenFabricacion, DOC.vIdNumeroCorrelativoCabeceraOrdenFabricacion, " &
                                                   "DOC.cIdEmpresa, DOC.cIdEquipoSAPCabeceraOrdenFabricacion, DOC.vIdArticuloSAPCabeceraOrdenFabricacion, DOC.dFechaTransaccionCabeceraOrdenFabricacion, " &
                                                   "DOC.vIdClienteSAPCabeceraOrdenFabricacion, DOC.cIdCliente, DOC.cIdEquipo, DOC.vNumeroSerieEquipoCabeceraOrdenFabricacion, " &
                                                   "DOC.vOrdenVentaCabeceraOrdenFabricacion, DOC.vOrdenCompraCabeceraOrdenFabricacion, DOC.vOrdenFabricacionCabeceraOrdenFabricacion, DOC.cEstadoCabeceraOrdenFabricacion, " &
                                                   "DOC.bEstadoRegistroCabeceraOrdenFabricacion, DOC.dFechaUltimaModificacionCabeceraOrdenFabricacion, DOC.dFechaCreacionCabeceraOrdenFabricacion, " &
                                                   "DOC.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion, DOC.cIdUsuarioCreacionCabeceraOrdenFabricacion, " &
                                                   "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo, DOC.vIdArticuloSAPCabeceraOrdenFabricacion, " &
                                                   "DOC.dFechaEmisionCabeceraOrdenFabricacion, DOC.vIdUnidadTrabajoOrdenFabricacion, DOC.dFechaProgramacionAlmacenCabeceraOrdenFabricacion, " &
                                                   "DOC.cIdPersonal, PER.vNombreCompletoPersonal, DOC.vDescripcionCabeceraOrdenFabricacion, DOC.dFechaOrdenVentaCabeceraOrdenFabricacion, DOC.dFechaAsignacionAuxiliarCabeceraOrdenFabricacion, " &
                                                   "DOC.vOrdenVentaSAPCabeceraOrdenFabricacion, DOC.vOrdenCompraSAPCabeceraOrdenFabricacion, DOC.vOrdenFabricacionSAPCabeceraOrdenFabricacion " &
                                                   "ORDER BY DOC.dFechaEmisionCabeceraOrdenFabricacion DESC",
                                                   "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, "", "", "", "")

        Dim Coleccion As New List(Of VI_LOGI_CABECERAORDENFABRICACION)
        For Each Busqueda In Consulta
            Dim BuscarOrdFab As New VI_LOGI_CABECERAORDENFABRICACION
            BuscarOrdFab.IdTipoDocumento = Busqueda.cIdTipoDocumentoCabeceraOrdenFabricacion
            BuscarOrdFab.IdNumeroSerie = Busqueda.vIdNumeroSerieCabeceraOrdenFabricacion
            BuscarOrdFab.IdNumeroCorrelativo = Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion
            BuscarOrdFab.IdEquipo = Busqueda.cIdEquipo
            BuscarOrdFab.IdCliente = Busqueda.cIdCliente
            BuscarOrdFab.RazonSocial = Busqueda.vRazonSocialCliente
            BuscarOrdFab.IdEquipoSAP = Busqueda.cIdEquipoSAPCabeceraOrdenFabricacion
            BuscarOrdFab.IdClienteSAP = Busqueda.vIdClienteSAPCabeceraOrdenFabricacion
            BuscarOrdFab.NumeroSerieEquipo = Busqueda.vNumeroSerieEquipoCabeceraOrdenFabricacion
            BuscarOrdFab.StatusOrdenFabricacion = Busqueda.cEstadoCabeceraOrdenFabricacion
            BuscarOrdFab.Estado = Busqueda.bEstadoRegistroCabeceraOrdenFabricacion
            BuscarOrdFab.DescripcionEquipo = Busqueda.vDescripcionEquipo
            BuscarOrdFab.RucCliente = Busqueda.vRucCliente
            BuscarOrdFab.IdArticuloSAPCabecera = Busqueda.vIdArticuloSAPCabeceraOrdenFabricacion
            BuscarOrdFab.FechaEmision = Busqueda.dFechaEmisionCabeceraOrdenFabricacion
            'BuscarOrdFab.OrdenVenta = Busqueda.vOrdenVentaCabeceraOrdenFabricacion
            'BuscarOrdFab.OrdenCompra = Busqueda.vOrdenCompraCabeceraOrdenFabricacion
            'BuscarOrdFab.OrdenFabricacion = Busqueda.vOrdenFabricacionCabeceraOrdenFabricacion
            BuscarOrdFab.OrdenVenta = Busqueda.vOrdenVentaSAPCabeceraOrdenFabricacion
            BuscarOrdFab.OrdenCompra = Busqueda.vOrdenCompraSAPCabeceraOrdenFabricacion
            BuscarOrdFab.OrdenFabricacion = Busqueda.vOrdenFabricacionSAPCabeceraOrdenFabricacion
            BuscarOrdFab.IdUnidadTrabajo = Busqueda.vIdUnidadTrabajoOrdenFabricacion
            BuscarOrdFab.IdAuxiliar = Busqueda.cIdPersonal
            BuscarOrdFab.NombreCompletoAuxiliar = Busqueda.vNombreCompletoPersonal
            BuscarOrdFab.DescripcionOrdenFabricacion = Busqueda.vDescripcionCabeceraOrdenFabricacion
            BuscarOrdFab.CantidadItems = Busqueda.nTotalItemDetalleOrdenFabricacion
            BuscarOrdFab.FechaOrdenVenta = Busqueda.dFechaOrdenVentaCabeceraOrdenFabricacion
            BuscarOrdFab.FechaAsignacionAuxiliar = Busqueda.dFechaAsignacionAuxiliarCabeceraOrdenFabricacion
            Coleccion.Add(BuscarOrdFab)
        Next
        Return Coleccion
    End Function

    Public Function OrdenFabricacionActualizarDisponibilidadStock(ByVal DetOrdFab As List(Of LOGI_DETALLEORDENFABRICACION), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue
        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            For Each Busqueda In DetOrdFab
                Dim strSQL As String = ""
                strSQL = "UPDATE LOGI_DETALLEORDENFABRICACION " &
                         "SET nCantidadAlmacenEncontradaDetalleOrdenFabricacion = " & Busqueda.nCantidadAlmacenEncontradaDetalleOrdenFabricacion & "," &
                         "    nCantidadAlmacenSaldoDetalleOrdenFabricacion = " & Busqueda.nCantidadAlmacenSaldoDetalleOrdenFabricacion & "," &
                         "    vNumeroSerieDetalleOrdenFabricacion = '" & Busqueda.vNumeroSerieDetalleOrdenFabricacion & "' " &
                         "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & Busqueda.cIdTipoDocumentoCabeceraOrdenFabricacion & "' " &
                         "      AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & Busqueda.vIdNumeroSerieCabeceraOrdenFabricacion & "' " &
                         "      AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion & "' " &
                         "      AND cIdEmpresa = '" & Busqueda.cIdEmpresa & "' " &
                         "      AND nIdNumeroItemDetalleOrdenFabricacion = '" & Busqueda.nIdNumeroItemDetalleOrdenFabricacion & "' " &
                         "      AND cIdEquipoSAPCabeceraOrdenFabricacion = '" & Busqueda.cIdEquipoSAPCabeceraOrdenFabricacion & "' " &
                         "      AND vIdArticuloSAPDetalleOrdenFabricacion = '" & Busqueda.vIdArticuloSAPDetalleOrdenFabricacion & "'"
                'OrdenFabricacionGetData(strSQL)
                x = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", strSQL, "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, "", "", "", "").ReturnValue.ToString
                LogAuditoria.vEvento = "ACTUALIZAR DETALLE ORDEN FABRICACION CANTIDAD ENTREGADA"
                LogAuditoria.vQuery = strSQL

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_UPDATE", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString

            Next
            scope.Complete()
            Return x
        End Using
    End Function
    'Public Function OrdenFabricacionInsertaDetalle(ByVal DetalleOrdenFabricacion As List(Of LOGI_CABECERAORDENFABRICACION), Optional ByVal strNroEnlaceOrdenFabricacion As String = "", Optional ByVal strNroEnlaceCatalogo As String = "") As Int32
    '    Dim x = ""

    '    'Inicializo la Transacción
    '    Dim tOption As New TransactionOptions
    '    tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
    '    tOption.Timeout = TimeSpan.MaxValue

    '    Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
    '        For Each Busqueda In DetalleOrdenFabricacion
    '            If Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENFABRICACION WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & Busqueda.cIdTipoDocumentoCabeceraOrdenFabricacion & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & Busqueda.vIdNumeroSerieCabeceraOrdenFabricacion & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion & "' AND cIdEmpresa = '" & Busqueda.cIdEmpresa & "'",
    '                         "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "").Count > 0 Then
    '                x = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_UPDATE", "", Busqueda.cIdTipoDocumentoCabeceraOrdenFabricacion, Busqueda.vIdNumeroSerieCabeceraOrdenFabricacion,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion, Busqueda.cIdEmpresa, Busqueda.cIdEquipoSAPCabeceraOrdenFabricacion,
    '                                       Busqueda.vIdArticuloSAPCabeceraOrdenFabricacion, Busqueda.dFechaTransaccionCabeceraOrdenFabricacion, Busqueda.vIdClienteSAPCabeceraOrdenFabricacion, Busqueda.cIdCliente,
    '                                       Busqueda.cIdEquipo, Busqueda.vNumeroSerieEquipoCabeceraOrdenFabricacion, Busqueda.vOrdenVentaCabeceraOrdenFabricacion, Busqueda.vOrdenCompraCabeceraOrdenFabricacion, Busqueda.cEstadoCabeceraOrdenFabricacion,
    '                                       Busqueda.bEstadoRegistroCabeceraOrdenFabricacion, Busqueda.dFechaUltimaModificacionCabeceraOrdenFabricacion, Busqueda.dFechaCreacionCabeceraOrdenFabricacion,
    '                                       Busqueda.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion, Busqueda.cIdUsuarioCreacionCabeceraOrdenFabricacion,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion).ReturnValue.ToString
    '            Else
    '                x = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_INSERT", "", Busqueda.cIdTipoDocumentoCabeceraOrdenFabricacion, Busqueda.vIdNumeroSerieCabeceraOrdenFabricacion,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion, Busqueda.cIdEmpresa, Busqueda.cIdEquipoSAPCabeceraOrdenFabricacion,
    '                                       Busqueda.vIdArticuloSAPCabeceraOrdenFabricacion, Busqueda.dFechaTransaccionCabeceraOrdenFabricacion, Busqueda.vIdClienteSAPCabeceraOrdenFabricacion, Busqueda.cIdCliente,
    '                                       Busqueda.cIdEquipo, Busqueda.vNumeroSerieEquipoCabeceraOrdenFabricacion, Busqueda.vOrdenVentaCabeceraOrdenFabricacion, Busqueda.vOrdenCompraCabeceraOrdenFabricacion, Busqueda.cEstadoCabeceraOrdenFabricacion,
    '                                       Busqueda.bEstadoRegistroCabeceraOrdenFabricacion, Busqueda.dFechaUltimaModificacionCabeceraOrdenFabricacion, Busqueda.dFechaCreacionCabeceraOrdenFabricacion,
    '                                       Busqueda.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion, Busqueda.cIdUsuarioCreacionCabeceraOrdenFabricacion,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion).ReturnValue.ToString
    '            End If
    '        Next
    '        scope.Complete()
    '        Return x
    '    End Using
    'End Function

    Public Function OrdenFabricacionInserta(ByVal OrdenFabricacion As LOGI_CABECERAORDENFABRICACION) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_INSERT", "", OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion, OrdenFabricacion.cIdEmpresa, OrdenFabricacion.cIdEquipoSAPCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vIdArticuloSAPCabeceraOrdenFabricacion, OrdenFabricacion.dFechaTransaccionCabeceraOrdenFabricacion, OrdenFabricacion.vIdClienteSAPCabeceraOrdenFabricacion, OrdenFabricacion.cIdCliente,
                                           OrdenFabricacion.cIdEquipo, OrdenFabricacion.vNumeroSerieEquipoCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenVentaCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenCompraCabeceraOrdenFabricacion, OrdenFabricacion.cEstadoCabeceraOrdenFabricacion,
                                           OrdenFabricacion.bEstadoRegistroCabeceraOrdenFabricacion, OrdenFabricacion.dFechaUltimaModificacionCabeceraOrdenFabricacion, OrdenFabricacion.dFechaCreacionCabeceraOrdenFabricacion,
                                           OrdenFabricacion.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion, OrdenFabricacion.cIdUsuarioCreacionCabeceraOrdenFabricacion,
                                           OrdenFabricacion.dFechaEmisionCabeceraOrdenFabricacion, OrdenFabricacion.vIdUnidadTrabajoOrdenFabricacion, OrdenFabricacion.dFechaProgramacionAlmacenCabeceraOrdenFabricacion, OrdenFabricacion.cIdPersonal,
                                           OrdenFabricacion.vOrdenFabricacionCabeceraOrdenFabricacion, OrdenFabricacion.dFechaOrdenVentaCabeceraOrdenFabricacion, OrdenFabricacion.dFechaAsignacionAuxiliarCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vOrdenVentaSAPCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenCompraSAPCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenFabricacionSAPCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion).ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenFabricacionEdita(ByVal OrdenFabricacion As LOGI_CABECERAORDENFABRICACION) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_UPDATE", "", OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion, OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion, OrdenFabricacion.cIdEmpresa, OrdenFabricacion.cIdEquipoSAPCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vIdArticuloSAPCabeceraOrdenFabricacion, OrdenFabricacion.dFechaTransaccionCabeceraOrdenFabricacion, OrdenFabricacion.vIdClienteSAPCabeceraOrdenFabricacion, OrdenFabricacion.cIdCliente,
                                           OrdenFabricacion.cIdEquipo, OrdenFabricacion.vNumeroSerieEquipoCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenVentaCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenCompraCabeceraOrdenFabricacion, OrdenFabricacion.cEstadoCabeceraOrdenFabricacion,
                                           OrdenFabricacion.bEstadoRegistroCabeceraOrdenFabricacion, OrdenFabricacion.dFechaUltimaModificacionCabeceraOrdenFabricacion, OrdenFabricacion.dFechaCreacionCabeceraOrdenFabricacion,
                                           OrdenFabricacion.cIdUsuarioUltimaModificacionCabeceraOrdenFabricacion, OrdenFabricacion.cIdUsuarioCreacionCabeceraOrdenFabricacion,
                                           OrdenFabricacion.dFechaEmisionCabeceraOrdenFabricacion, OrdenFabricacion.vIdUnidadTrabajoOrdenFabricacion, OrdenFabricacion.dFechaProgramacionAlmacenCabeceraOrdenFabricacion, OrdenFabricacion.cIdPersonal,
                                           OrdenFabricacion.vOrdenFabricacionCabeceraOrdenFabricacion, OrdenFabricacion.dFechaOrdenVentaCabeceraOrdenFabricacion, OrdenFabricacion.dFechaAsignacionAuxiliarCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vOrdenVentaSAPCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenCompraSAPCabeceraOrdenFabricacion, OrdenFabricacion.vOrdenFabricacionSAPCabeceraOrdenFabricacion,
                                           OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion).ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenFabricacionElimina(ByVal OrdenFabricacion As LOGI_CABECERAORDENFABRICACION) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "UPDATE LOGI_CABECERAORDENFABRICACION SET bEstadoRegistroCabeceraOrdenFabricacion = 0 WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & OrdenFabricacion.cIdTipoDocumentoCabeceraOrdenFabricacion & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & OrdenFabricacion.vIdNumeroSerieCabeceraOrdenFabricacion & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & OrdenFabricacion.vIdNumeroCorrelativoCabeceraOrdenFabricacion & "' AND cIdEmpresa = '" & OrdenFabricacion.cIdEmpresa & "' ",
                                           "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, "", "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenFabricacionExiste(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As Boolean
        If Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENFABRICACION " &
                                                     "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                      " AND bEstadoRegistroCabeceraOrdenFabricacion = 1",
                                      "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function OrdenFabricacionListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdArticuloSAPCabecera As String, Optional IdArticuloSAPDetalle As String = "*") As LOGI_DETALLEORDENFABRICACION
        'Data.PA_LOGI_MNT_CABECERAORDENFABRICACION("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENFABRICACION " &
        '                                             "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
        '                              " AND bEstadoRegistroCabeceraOrdenFabricacion = 1 AND IdArticuloSAPCabeceraOrdenTrabajo = '" & IdArticuloSAPCabecera & "' AND IdArticuloSAPDetalleOrdenTrabajo = '" & IdArticuloSAPDetalle & "' ",
        '                              "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "").Count > 0 Then
        Dim Consulta = Data.PA_LOGI_MNT_DETALLEORDENFABRICACION("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENFABRICACION " &
                                                     "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                      " AND vIdArticuloSAPCabeceraOrdenFabricacion = '" & IdArticuloSAPCabecera & "' AND (vIdArticuloSAPDetalleOrdenFabricacion = '" & IdArticuloSAPDetalle & "' OR '*' = '" & IdArticuloSAPDetalle & "') ",
                                      "", "", "", "", 0, "", "", "", "", 0, "", 0, "")

        Dim Coleccion As New LOGI_DETALLEORDENFABRICACION
        For Each Busqueda In Consulta
            Coleccion.cIdTipoDocumentoCabeceraOrdenFabricacion = Busqueda.cIdTipoDocumentoCabeceraOrdenFabricacion
            Coleccion.vIdNumeroSerieCabeceraOrdenFabricacion = Busqueda.vIdNumeroSerieCabeceraOrdenFabricacion
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenFabricacion = Busqueda.vIdNumeroCorrelativoCabeceraOrdenFabricacion
            Coleccion.cIdEmpresa = Busqueda.cIdEmpresa
            Coleccion.nIdNumeroItemDetalleOrdenFabricacion = Busqueda.nIdNumeroItemDetalleOrdenFabricacion
            Coleccion.vIdArticuloSAPCabeceraOrdenFabricacion = Busqueda.vIdArticuloSAPCabeceraOrdenFabricacion
            Coleccion.cIdEquipoSAPCabeceraOrdenFabricacion = Busqueda.cIdEquipoSAPCabeceraOrdenFabricacion
            Coleccion.vIdArticuloSAPDetalleOrdenFabricacion = Busqueda.vIdArticuloSAPDetalleOrdenFabricacion
            Coleccion.vDescripcionArticuloSAPDetalleOrdenFabricacion = Busqueda.vDescripcionArticuloSAPDetalleOrdenFabricacion
            Coleccion.nCantidadSAPDetalleOrdenFabricacion = Busqueda.nCantidadSAPDetalleOrdenFabricacion
            Coleccion.vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion = Busqueda.vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion
            Coleccion.nTotalCantidadConsumidoDetalleOrdenFabricacion = Busqueda.nTotalCantidadConsumidoDetalleOrdenFabricacion
        Next
        Return Coleccion
    End Function
End Class
