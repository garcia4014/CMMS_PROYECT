Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsOrdenVentaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function OrdenVentaGetData(strQuery As String) As DataTable
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

    Public Function OrdenVentaListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENVENTA)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENVENTA " &
                                                 "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & IdTipoOrden & "' ",
                                                 "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, Now, "1", "", "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERAORDENVENTA)
        For Each OrdenVenta In Consulta
            Dim OrdFab As New LOGI_CABECERAORDENVENTA
            OrdFab.vIdNumeroCorrelativoCabeceraOrdenVenta = OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta
            OrdFab.vIdNumeroCorrelativoCabeceraOrdenVenta = OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta.Trim + OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta.Trim + OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta.Trim + "-" + OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta
            Coleccion.Add(OrdFab)
        Next
        Return Coleccion
    End Function

    Public Function OrdenVentaUnidadTrabajoListarCombo(ByVal IdTipoOrden As String) As List(Of LOGI_CABECERAORDENVENTA)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_NONE", "SELECT ISNULL(vIdUnidadTrabajoOrdenVenta, '') AS vIdUnidadTrabajoOrdenVenta FROM LOGI_CABECERAORDENVENTA " &
                                                 "WHERE (cIdTipoDocumentoCabeceraOrdenVenta = '" & IdTipoOrden & "' OR '*' = '" & IdTipoOrden & "') " &
                                                 "GROUP BY ISNULL(vIdUnidadTrabajoOrdenVenta, '') " &
                                                 "ORDER BY vIdUnidadTrabajoOrdenVenta",
                                                 "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, Now, "1", "", "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERAORDENVENTA)
        For Each OrdenVenta In Consulta
            Dim OrdFab As New LOGI_CABECERAORDENVENTA
            If IsNothing(OrdenVenta.vIdUnidadTrabajoOrdenVenta) = False Then
                OrdFab.vIdUnidadTrabajoOrdenVenta = OrdenVenta.vIdUnidadTrabajoOrdenVenta
            End If
            '            OrdFab.vIdNumeroCorrelativoCabeceraOrdenVenta = OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta.Trim + OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta.Trim + OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta.Trim + "-" + OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta
            'OrdFab.vIdNumeroCorrelativoCabeceraOrdenVenta = OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta.Trim + OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta.Trim + OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta.Trim + "-" + OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta
            Coleccion.Add(OrdFab)
        Next
        Return Coleccion
    End Function

    'Public Function OrdenVentaListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENVENTA
    '    Dim Consulta = Data.PA_LOGI_MNT_CABECERAOrdenVenta("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENVENTA WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
    '                                           " AND bEstadoRegistroCabeceraOrdenVenta = 1 " &
    '                                           "ORDER BY cIdCatalogo",
    '                                           "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "")
    '    Dim Coleccion As New LOGI_CABECERAORDENVENTA
    '    For Each LOGI_CABECERAORDENVENTA In Consulta
    '        Coleccion.cIdTipoDocumentoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdTipoDocumentoCabeceraOrdenVenta
    '        Coleccion.vIdNumeroSerieCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdNumeroSerieCabeceraOrdenVenta
    '        Coleccion.vIdNumeroCorrelativoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdNumeroCorrelativoCabeceraOrdenVenta
    '        Coleccion.cIdEmpresa = LOGI_CABECERAORDENVENTA.cIdEmpresa
    '        Coleccion.cIdEquipoSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdEquipoSAPCabeceraOrdenVenta
    '        Coleccion.vIdArticuloSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdArticuloSAPCabeceraOrdenVenta
    '        Coleccion.dFechaTransaccionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaTransaccionCabeceraOrdenVenta
    '        Coleccion.vIdClienteSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdClienteSAPCabeceraOrdenVenta
    '        Coleccion.cIdCliente = LOGI_CABECERAORDENVENTA.cIdCliente
    '        Coleccion.cIdEquipo = LOGI_CABECERAORDENVENTA.cIdEquipo
    '        Coleccion.vNumeroSerieEquipoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vNumeroSerieEquipoCabeceraOrdenVenta
    '        Coleccion.vOrdenVentaCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenVentaCabeceraOrdenVenta
    '        Coleccion.vOrdenCompraCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenCompraCabeceraOrdenVenta
    '        Coleccion.cEstadoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cEstadoCabeceraOrdenVenta
    '        Coleccion.bEstadoRegistroCabeceraOrdenVenta = IIf(LOGI_CABECERAORDENVENTA.bEstadoRegistroCabeceraOrdenVenta Is Nothing, False, LOGI_CABECERAORDENVENTA.bEstadoRegistroCabeceraOrdenVenta)
    '        Coleccion.dFechaUltimaModificacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaUltimaModificacionCabeceraOrdenVenta
    '        Coleccion.dFechaCreacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaCreacionCabeceraOrdenVenta
    '        Coleccion.cIdUsuarioUltimaModificacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdUsuarioUltimaModificacionCabeceraOrdenVenta
    '        Coleccion.cIdUsuarioCreacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdUsuarioCreacionCabeceraOrdenVenta
    '    Next
    '    Return Coleccion
    'End Function

    Public Function OrdenVentaListarPorId(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdNroOF As String, ByVal IdEmpresa As String) As LOGI_CABECERAORDENVENTA
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENVENTA WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                 " AND vOrdenFabricacionCabeceraOrdenVenta = '" & IdNroOF & "' AND bEstadoRegistroCabeceraOrdenVenta = 1 " &
                                                 "",
                                                 "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, Now, "1", "", "", "", "")
        Dim Coleccion As New LOGI_CABECERAORDENVENTA
        For Each LOGI_CABECERAORDENVENTA In Consulta
            Coleccion.cIdTipoDocumentoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdTipoDocumentoCabeceraOrdenVenta
            Coleccion.vIdNumeroSerieCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdNumeroSerieCabeceraOrdenVenta
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdNumeroCorrelativoCabeceraOrdenVenta
            Coleccion.cIdEmpresa = LOGI_CABECERAORDENVENTA.cIdEmpresa
            'Coleccion.cIdEquipoSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdEquipoSAPCabeceraOrdenVenta
            'Coleccion.vIdArticuloSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdArticuloSAPCabeceraOrdenVenta
            Coleccion.dFechaTransaccionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaTransaccionCabeceraOrdenVenta
            Coleccion.vIdClienteSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vIdClienteSAPCabeceraOrdenVenta
            Coleccion.cIdCliente = LOGI_CABECERAORDENVENTA.cIdCliente
            Coleccion.cIdEquipo = LOGI_CABECERAORDENVENTA.cIdEquipo
            Coleccion.vNumeroSerieEquipoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vNumeroSerieEquipoCabeceraOrdenVenta
            Coleccion.vOrdenVentaCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenVentaCabeceraOrdenVenta
            Coleccion.vOrdenCompraCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenCompraCabeceraOrdenVenta
            Coleccion.vOrdenFabricacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenFabricacionCabeceraOrdenVenta
            Coleccion.cEstadoCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cEstadoCabeceraOrdenVenta
            Coleccion.bEstadoRegistroCabeceraOrdenVenta = IIf(LOGI_CABECERAORDENVENTA.bEstadoRegistroCabeceraOrdenVenta Is Nothing, False, LOGI_CABECERAORDENVENTA.bEstadoRegistroCabeceraOrdenVenta)
            Coleccion.dFechaUltimaModificacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaUltimaModificacionCabeceraOrdenVenta
            Coleccion.dFechaCreacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaCreacionCabeceraOrdenVenta
            Coleccion.cIdUsuarioUltimaModificacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdUsuarioUltimaModificacionCabeceraOrdenVenta
            Coleccion.cIdUsuarioCreacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.cIdUsuarioCreacionCabeceraOrdenVenta
            Coleccion.dFechaEmisionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaEmisionCabeceraOrdenVenta
            Coleccion.vIdUnidadTrabajoOrdenVenta = LOGI_CABECERAORDENVENTA.vIdUnidadTrabajoOrdenVenta
            Coleccion.dFechaProgramacionAlmacenCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaProgramacionAlmacenCabeceraOrdenVenta
            Coleccion.vDescripcionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vDescripcionCabeceraOrdenVenta
            Coleccion.dFechaOrdenFabricacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaOrdenFabricacionCabeceraOrdenVenta
            Coleccion.dFechaOrdenVentaCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.dFechaOrdenVentaCabeceraOrdenVenta
            Coleccion.cIdPersonal = LOGI_CABECERAORDENVENTA.cIdPersonal
            Coleccion.bTieneOrdenFabricacionCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.bTieneOrdenFabricacionCabeceraOrdenVenta
            Coleccion.vOrdenVentaSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenVentaSAPCabeceraOrdenVenta
            Coleccion.vOrdenCompraSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenCompraSAPCabeceraOrdenVenta
            Coleccion.vOrdenFabricacionSAPCabeceraOrdenVenta = LOGI_CABECERAORDENVENTA.vOrdenFabricacionSAPCabeceraOrdenVenta
        Next
        Return Coleccion
    End Function

    Public Function OrdenVentaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_LOGI_CABECERAORDENVENTA)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_CABECERAOrdenVenta("SQL_NONE", "SELECT EQU.vDescripcionOrdenVenta, EQU.bEstadoRegistroOrdenVenta, EQU.cIdEnlaceOrdenVenta, EQU.cIdJerarquiaCatalogo, EQU.cIdOrdenVenta, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaOrdenVenta, EQU.cIdSistemaFuncionalOrdenVenta, EQU.cIdEnlaceCatalogo, EQU.vObservacionOrdenVenta, EQU.dFechaTransaccionOrdenVenta, EQU.nVidaUtilOrdenVenta, EQU.vIdEquivalenciaOrdenVenta, " &
        '                                           "EQU.vNumeroSerieOrdenVenta, EQU.dFechaRegistroTarjetaSAPOrdenVenta, CLI.cIdCliente, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoOrdenVenta, 'R') AS cEstadoOrdenVenta " &
        '                                           "FROM LOGI_CABECERAORDENVENTA AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
        '                                           "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
        '                                           "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroCabeceraOrdenVenta = 1 " &
        '                                           "      AND EQU.cIdJerarquiaCatalogo = '" & Jerarquia & "' " &
        '                                           "ORDER BY EQU.dFechaRegistroTarjetaSAPOrdenVenta DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdOrdenVenta, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalOrdenVenta",
        '                                           "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "")
        Dim Consulta = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_NONE", "SELECT DOC.cIdTipoDocumentoCabeceraOrdenVenta, DOC.vIdNumeroSerieCabeceraOrdenVenta, DOC.vIdNumeroCorrelativoCabeceraOrdenVenta, " &
                                                   "DOC.cIdEmpresa, DOC.dFechaTransaccionCabeceraOrdenVenta, DOC.vOrdenFabricacionCabeceraOrdenVenta, " &
                                                   "DOC.vIdClienteSAPCabeceraOrdenVenta, DOC.cIdCliente, DOC.cIdEquipo, DOC.vNumeroSerieEquipoCabeceraOrdenVenta, " &
                                                   "DOC.vOrdenVentaCabeceraOrdenVenta, DOC.vOrdenCompraCabeceraOrdenVenta, DOC.cEstadoCabeceraOrdenVenta, " &
                                                   "DOC.bEstadoRegistroCabeceraOrdenVenta, DOC.dFechaUltimaModificacionCabeceraOrdenVenta, DOC.dFechaCreacionCabeceraOrdenVenta, " &
                                                   "DOC.cIdUsuarioUltimaModificacionCabeceraOrdenVenta, DOC.cIdUsuarioCreacionCabeceraOrdenVenta, " &
                                                   "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo, " &
                                                   "DOC.dFechaEmisionCabeceraOrdenVenta, DOC.vIdUnidadTrabajoOrdenVenta, DOC.dFechaProgramacionAlmacenCabeceraOrdenVenta, " &
                                                   "DOC.cIdPersonal, PER.vNombreCompletoPersonal, DOC.vDescripcionCabeceraOrdenVenta, " &
                                                   "DOC.dFechaOrdenVentaCabeceraOrdenVenta, DOC.dFechaAsignacionAuxiliarCabeceraOrdenVenta, DOC.dFechaOrdenFabricacionCabeceraOrdenVenta, " &
                                                   "DOC.bTieneOrdenFabricacionCabeceraOrdenVenta, DOC.vOrdenVentaSAPCabeceraOrdenVenta, DOC.vOrdenCompraSAPCabeceraOrdenVenta, DOC.vOrdenFabricacionSAPCabeceraOrdenVenta, " &
                                                   "CASE WHEN DOC.bTieneOrdenFabricacionCabeceraOrdenVenta = 0 THEN " &
                                                   "COUNT(DETDOC.vIdArticuloSAPDetalleOrdenVenta) " &
                                                   "ELSE " &
                                                   "(SELECT COUNT(*) FROM LOGI_CABECERAORDENFABRICACION AS CABORDFAB INNER JOIN LOGI_DETALLEORDENFABRICACION AS DETORDFAB ON CABORDFAB.cIdTipoDocumentoCabeceraOrdenFabricacion = DETORDFAB.cIdTipoDocumentoCabeceraOrdenFabricacion AND CABORDFAB.vIdNumeroSerieCabeceraOrdenFabricacion = DETORDFAB.vIdNumeroSerieCabeceraOrdenFabricacion AND CABORDFAB.vIdNumeroCorrelativoCabeceraOrdenFabricacion = DETORDFAB.vIdNumeroCorrelativoCabeceraOrdenFabricacion AND CABORDFAB.cIdEmpresa = DETORDFAB.cIdEmpresa " &
                                                   "     WHERE CABORDFAB.cIdEmpresa = DOC.cIdEmpresa AND CABORDFAB.vOrdenFabricacionCabeceraOrdenFabricacion = DOC.vOrdenFabricacionCabeceraOrdenVenta " &
                                                   "           AND DETORDFAB.vIdArticuloSAPDetalleOrdenFabricacion NOT LIKE 'CPMO%' AND DETORDFAB.vIdArticuloSAPDetalleOrdenFabricacion NOT LIKE 'CPCI%' ) END AS nTotalItemDetalleOrdenVenta, " &
                                                   "DOC.dFechaInicioCabeceraOrdenVenta, DOC.dFechaTerminadaCabeceraOrdenVenta " &
                                                   "FROM LOGI_CABECERAORDENVENTA AS DOC LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     DOC.cIdCliente = CLI.cIdCliente AND DOC.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "     LEFT JOIN LOGI_EQUIPO AS EQU ON " &
                                                   "     DOC.cIdEquipo = EQU.cIdEquipo AND DOC.cIdEmpresa = EQU.cIdEmpresa " &
                                                   "     LEFT JOIN RRHH_PERSONAL AS PER ON " &
                                                   "     DOC.cIdPersonal = PER.cIdPersonal " &
                                                   "     LEFT JOIN LOGI_DETALLEORDENVENTA AS DETDOC ON " &
                                                   "     DOC.cIdTipoDocumentoCabeceraOrdenVenta = DETDOC.cIdTipoDocumentoCabeceraOrdenVenta " &
                                                   "     AND DOC.vIdNumeroSerieCabeceraOrdenVenta = DETDOC.vIdNumeroSerieCabeceraOrdenVenta " &
                                                   "     AND DOC.vIdNumeroCorrelativoCabeceraOrdenVenta = DETDOC.vIdNumeroCorrelativoCabeceraOrdenVenta " &
                                                   "     AND DOC.cIdEmpresa = DETDOC.cIdEmpresa AND DOC.vOrdenFabricacionCabeceraOrdenVenta = DETDOC.vOrdenFabricacionCabeceraOrdenVenta " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND (DOC.bEstadoRegistroCabeceraOrdenVenta = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                   "      AND DOC.cIdEmpresa = '" & IdEmpresa & "' " &
                                                   "      AND DETDOC.vIdArticuloSAPDetalleOrdenVenta NOT LIKE 'CPMO%' AND DETDOC.vIdArticuloSAPDetalleOrdenVenta NOT LIKE 'CPCI%' " &
                                                   "GROUP BY DOC.cIdTipoDocumentoCabeceraOrdenVenta, DOC.vIdNumeroSerieCabeceraOrdenVenta, DOC.vIdNumeroCorrelativoCabeceraOrdenVenta, " &
                                                   "DOC.cIdEmpresa, DOC.dFechaTransaccionCabeceraOrdenVenta, DOC.vOrdenFabricacionCabeceraOrdenVenta, " &
                                                   "DOC.vIdClienteSAPCabeceraOrdenVenta, DOC.cIdCliente, DOC.cIdEquipo, DOC.vNumeroSerieEquipoCabeceraOrdenVenta, " &
                                                   "DOC.vOrdenVentaCabeceraOrdenVenta, DOC.vOrdenCompraCabeceraOrdenVenta, DOC.cEstadoCabeceraOrdenVenta, " &
                                                   "DOC.bEstadoRegistroCabeceraOrdenVenta, DOC.dFechaUltimaModificacionCabeceraOrdenVenta, DOC.dFechaCreacionCabeceraOrdenVenta, " &
                                                   "DOC.cIdUsuarioUltimaModificacionCabeceraOrdenVenta, DOC.cIdUsuarioCreacionCabeceraOrdenVenta, " &
                                                   "CLI.vRazonSocialCliente, CLI.vRucCliente, EQU.vDescripcionEquipo, " &
                                                   "DOC.dFechaEmisionCabeceraOrdenVenta, DOC.vIdUnidadTrabajoOrdenVenta, DOC.dFechaProgramacionAlmacenCabeceraOrdenVenta, " &
                                                   "DOC.cIdPersonal, PER.vNombreCompletoPersonal, DOC.vDescripcionCabeceraOrdenVenta, DOC.dFechaOrdenVentaCabeceraOrdenVenta, " &
                                                   "DOC.dFechaAsignacionAuxiliarCabeceraOrdenVenta, DOC.dFechaOrdenFabricacionCabeceraOrdenVenta, DOC.bTieneOrdenFabricacionCabeceraOrdenVenta, " &
                                                   "DOC.vOrdenVentaSAPCabeceraOrdenVenta, DOC.vOrdenCompraSAPCabeceraOrdenVenta, DOC.vOrdenFabricacionSAPCabeceraOrdenVenta, " &
                                                   "DOC.dFechaInicioCabeceraOrdenVenta, DOC.dFechaTerminadaCabeceraOrdenVenta " &
                                                   "ORDER BY DOC.dFechaProgramacionAlmacenCabeceraOrdenVenta DESC, DOC.dFechaEmisionCabeceraOrdenVenta DESC",
                                                   "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, Now, "1", "", "", "", "")

        Dim Coleccion As New List(Of VI_LOGI_CABECERAORDENVENTA)
        For Each Busqueda In Consulta
            Dim BuscarOrdFab As New VI_LOGI_CABECERAORDENVENTA
            BuscarOrdFab.IdTipoDocumento = Busqueda.cIdTipoDocumentoCabeceraOrdenVenta
            BuscarOrdFab.IdNumeroSerie = Busqueda.vIdNumeroSerieCabeceraOrdenVenta
            BuscarOrdFab.IdNumeroCorrelativo = Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta
            BuscarOrdFab.IdEquipo = Busqueda.cIdEquipo
            BuscarOrdFab.IdCliente = Busqueda.cIdCliente
            BuscarOrdFab.RazonSocial = Busqueda.vRazonSocialCliente
            'BuscarOrdFab.IdEquipoSAP = Busqueda.cIdEquipoSAPCabeceraOrdenVenta
            BuscarOrdFab.IdClienteSAP = Busqueda.vIdClienteSAPCabeceraOrdenVenta
            BuscarOrdFab.NumeroSerieEquipo = Busqueda.vNumeroSerieEquipoCabeceraOrdenVenta
            BuscarOrdFab.Status = Busqueda.cEstadoCabeceraOrdenVenta
            BuscarOrdFab.Estado = Busqueda.bEstadoRegistroCabeceraOrdenVenta
            'BuscarOrdFab.DescripcionEquipo = Busqueda.vDescripcionEquipo
            BuscarOrdFab.RucCliente = Busqueda.vRucCliente
            'BuscarOrdFab.IdArticuloSAPCabecera = Busqueda.vIdArticuloSAPCabeceraOrdenVenta
            BuscarOrdFab.FechaEmision = Busqueda.dFechaEmisionCabeceraOrdenVenta
            BuscarOrdFab.OrdenVentaSAP = Busqueda.vOrdenVentaSAPCabeceraOrdenVenta
            BuscarOrdFab.OrdenCompraSAP = Busqueda.vOrdenCompraSAPCabeceraOrdenVenta
            BuscarOrdFab.OrdenFabricacionSAP = Busqueda.vOrdenFabricacionSAPCabeceraOrdenVenta
            BuscarOrdFab.IdUnidadTrabajo = Busqueda.vIdUnidadTrabajoOrdenVenta
            BuscarOrdFab.IdAuxiliar = Busqueda.cIdPersonal
            BuscarOrdFab.NombreCompletoAuxiliar = Busqueda.vNombreCompletoPersonal
            BuscarOrdFab.DescripcionOrdenVenta = Busqueda.vDescripcionCabeceraOrdenVenta
            BuscarOrdFab.CantidadItems = Busqueda.nTotalItemDetalleOrdenVenta
            BuscarOrdFab.FechaOrdenVenta = Busqueda.dFechaOrdenVentaCabeceraOrdenVenta
            BuscarOrdFab.FechaAsignacionAuxiliar = Busqueda.dFechaAsignacionAuxiliarCabeceraOrdenVenta
            BuscarOrdFab.FechaOrdenFabricacion = Busqueda.dFechaOrdenFabricacionCabeceraOrdenVenta
            BuscarOrdFab.FechaProgramacionAlmacen = Busqueda.dFechaProgramacionAlmacenCabeceraOrdenVenta
            BuscarOrdFab.OrdenVentaKey = Busqueda.vOrdenVentaCabeceraOrdenVenta
            BuscarOrdFab.OrdenCompraKey = Busqueda.vOrdenCompraCabeceraOrdenVenta
            BuscarOrdFab.OrdenFabricacionKey = Busqueda.vOrdenFabricacionCabeceraOrdenVenta
            BuscarOrdFab.FechaInicioOrdenVenta = Busqueda.dFechaInicioCabeceraOrdenVenta
            BuscarOrdFab.FechaTerminoOrdenVenta = Busqueda.dFechaTerminadaCabeceraOrdenVenta
            Coleccion.Add(BuscarOrdFab)
        Next
        Return Coleccion
    End Function

    Public Function OrdenVentaActualizarDisponibilidadStock(ByVal DetOrdVta As List(Of LOGI_DETALLEORDENVENTA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue
        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            For Each Busqueda In DetOrdVta
                Dim strSQL As String = ""
                'Si lo ejecuto por segunda vez me sale este error:
                'Se ha deshabilitado el acceso de red para el administrador de transacciones distribuidas (MSDTC). Habilite DTC para el acceso de red en la configuración de seguridad de MSDTC utilizando la herramienta administrativa Servicios de componentes.
                strSQL = "UPDATE LOGI_DETALLEORDENVENTA " &
                         "SET nCantidadAlmacenEncontradaDetalleOrdenVenta = " & Busqueda.nCantidadAlmacenEncontradaDetalleOrdenVenta & "," &
                         "    nCantidadAlmacenSaldoDetalleOrdenVenta = " & Busqueda.nCantidadAlmacenSaldoDetalleOrdenVenta & "," &
                         "    vNumeroSerieDetalleOrdenVenta = '" & Busqueda.vNumeroSerieDetalleOrdenVenta & "' " &
                         "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & Busqueda.cIdTipoDocumentoCabeceraOrdenVenta & "' " &
                         "      AND vIdNumeroSerieCabeceraOrdenVenta = '" & Busqueda.vIdNumeroSerieCabeceraOrdenVenta & "' " &
                         "      AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta & "' " &
                         "      AND cIdEmpresa = '" & Busqueda.cIdEmpresa & "' " &
                         "      AND nIdNumeroItemDetalleOrdenVenta = '" & Busqueda.nIdNumeroItemDetalleOrdenVenta & "' " &
                         "      AND vOrdenFabricacionCabeceraOrdenVenta = '" & Busqueda.vOrdenFabricacionCabeceraOrdenVenta & "'"
                OrdenVentaGetData(strSQL)
                'x = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_NONE", strSQL, "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, Now, "1", "", "", "", "").ReturnValue.ToString

                LogAuditoria.vEvento = "ACTUALIZAR DETALLE ORDEN VENTA CANTIDAD ENTREGADA"
                LogAuditoria.vQuery = strSQL

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_UPDATE", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString

            Next
            scope.Complete()
            Return x
        End Using
    End Function
    'Public Function OrdenVentaInsertaDetalle(ByVal DetalleOrdenVenta As List(Of LOGI_CABECERAORDENVENTA), Optional ByVal strNroEnlaceOrdenVenta As String = "", Optional ByVal strNroEnlaceCatalogo As String = "") As Int32
    '    Dim x = ""

    '    'Inicializo la Transacción
    '    Dim tOption As New TransactionOptions
    '    tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
    '    tOption.Timeout = TimeSpan.MaxValue

    '    Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
    '        For Each Busqueda In DetalleOrdenVenta
    '            If Data.PA_LOGI_MNT_CABECERAOrdenVenta("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENVENTA WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & Busqueda.cIdTipoDocumentoCabeceraOrdenVenta & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & Busqueda.vIdNumeroSerieCabeceraOrdenVenta & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta & "' AND cIdEmpresa = '" & Busqueda.cIdEmpresa & "'",
    '                         "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "").Count > 0 Then
    '                x = Data.PA_LOGI_MNT_CABECERAOrdenVenta("SQL_UPDATE", "", Busqueda.cIdTipoDocumentoCabeceraOrdenVenta, Busqueda.vIdNumeroSerieCabeceraOrdenVenta,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta, Busqueda.cIdEmpresa, Busqueda.cIdEquipoSAPCabeceraOrdenVenta,
    '                                       Busqueda.vIdArticuloSAPCabeceraOrdenVenta, Busqueda.dFechaTransaccionCabeceraOrdenVenta, Busqueda.vIdClienteSAPCabeceraOrdenVenta, Busqueda.cIdCliente,
    '                                       Busqueda.cIdEquipo, Busqueda.vNumeroSerieEquipoCabeceraOrdenVenta, Busqueda.vOrdenVentaCabeceraOrdenVenta, Busqueda.vOrdenCompraCabeceraOrdenVenta, Busqueda.cEstadoCabeceraOrdenVenta,
    '                                       Busqueda.bEstadoRegistroCabeceraOrdenVenta, Busqueda.dFechaUltimaModificacionCabeceraOrdenVenta, Busqueda.dFechaCreacionCabeceraOrdenVenta,
    '                                       Busqueda.cIdUsuarioUltimaModificacionCabeceraOrdenVenta, Busqueda.cIdUsuarioCreacionCabeceraOrdenVenta,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta).ReturnValue.ToString
    '            Else
    '                x = Data.PA_LOGI_MNT_CABECERAOrdenVenta("SQL_INSERT", "", Busqueda.cIdTipoDocumentoCabeceraOrdenVenta, Busqueda.vIdNumeroSerieCabeceraOrdenVenta,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta, Busqueda.cIdEmpresa, Busqueda.cIdEquipoSAPCabeceraOrdenVenta,
    '                                       Busqueda.vIdArticuloSAPCabeceraOrdenVenta, Busqueda.dFechaTransaccionCabeceraOrdenVenta, Busqueda.vIdClienteSAPCabeceraOrdenVenta, Busqueda.cIdCliente,
    '                                       Busqueda.cIdEquipo, Busqueda.vNumeroSerieEquipoCabeceraOrdenVenta, Busqueda.vOrdenVentaCabeceraOrdenVenta, Busqueda.vOrdenCompraCabeceraOrdenVenta, Busqueda.cEstadoCabeceraOrdenVenta,
    '                                       Busqueda.bEstadoRegistroCabeceraOrdenVenta, Busqueda.dFechaUltimaModificacionCabeceraOrdenVenta, Busqueda.dFechaCreacionCabeceraOrdenVenta,
    '                                       Busqueda.cIdUsuarioUltimaModificacionCabeceraOrdenVenta, Busqueda.cIdUsuarioCreacionCabeceraOrdenVenta,
    '                                       Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta).ReturnValue.ToString
    '            End If
    '        Next
    '        scope.Complete()
    '        Return x
    '    End Using
    'End Function

    Public Function OrdenVentaInserta(ByVal OrdenVenta As LOGI_CABECERAORDENVENTA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_INSERT", "", OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta, OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta,
                                           OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta, OrdenVenta.cIdEmpresa, OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta,
                                           OrdenVenta.dFechaTransaccionCabeceraOrdenVenta, OrdenVenta.vIdClienteSAPCabeceraOrdenVenta, OrdenVenta.cIdCliente,
                                           OrdenVenta.cIdEquipo, OrdenVenta.vNumeroSerieEquipoCabeceraOrdenVenta, OrdenVenta.vOrdenVentaCabeceraOrdenVenta, OrdenVenta.vOrdenCompraCabeceraOrdenVenta, OrdenVenta.cEstadoCabeceraOrdenVenta,
                                           OrdenVenta.bEstadoRegistroCabeceraOrdenVenta, OrdenVenta.dFechaUltimaModificacionCabeceraOrdenVenta, OrdenVenta.dFechaCreacionCabeceraOrdenVenta,
                                           OrdenVenta.cIdUsuarioUltimaModificacionCabeceraOrdenVenta, OrdenVenta.cIdUsuarioCreacionCabeceraOrdenVenta,
                                           OrdenVenta.dFechaEmisionCabeceraOrdenVenta, OrdenVenta.vIdUnidadTrabajoOrdenVenta, OrdenVenta.dFechaProgramacionAlmacenCabeceraOrdenVenta, OrdenVenta.cIdPersonal,
                                           OrdenVenta.vDescripcionCabeceraOrdenVenta, OrdenVenta.dFechaOrdenVentaCabeceraOrdenVenta, OrdenVenta.dFechaAsignacionAuxiliarCabeceraOrdenVenta,
                                           OrdenVenta.dFechaOrdenFabricacionCabeceraOrdenVenta, OrdenVenta.bTieneOrdenFabricacionCabeceraOrdenVenta,
                                           OrdenVenta.vOrdenVentaSAPCabeceraOrdenVenta, OrdenVenta.vOrdenCompraSAPCabeceraOrdenVenta, OrdenVenta.vOrdenFabricacionSAPCabeceraOrdenVenta,
                                           OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta).ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenVentaEdita(ByVal OrdenVenta As LOGI_CABECERAORDENVENTA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_UPDATE", "", OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta, OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta,
                                           OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta, OrdenVenta.cIdEmpresa, OrdenVenta.vOrdenFabricacionCabeceraOrdenVenta,
                                           OrdenVenta.dFechaTransaccionCabeceraOrdenVenta, OrdenVenta.vIdClienteSAPCabeceraOrdenVenta, OrdenVenta.cIdCliente,
                                           OrdenVenta.cIdEquipo, OrdenVenta.vNumeroSerieEquipoCabeceraOrdenVenta, OrdenVenta.vOrdenVentaCabeceraOrdenVenta, OrdenVenta.vOrdenCompraCabeceraOrdenVenta, OrdenVenta.cEstadoCabeceraOrdenVenta,
                                           OrdenVenta.bEstadoRegistroCabeceraOrdenVenta, OrdenVenta.dFechaUltimaModificacionCabeceraOrdenVenta, OrdenVenta.dFechaCreacionCabeceraOrdenVenta,
                                           OrdenVenta.cIdUsuarioUltimaModificacionCabeceraOrdenVenta, OrdenVenta.cIdUsuarioCreacionCabeceraOrdenVenta,
                                           OrdenVenta.dFechaEmisionCabeceraOrdenVenta, OrdenVenta.vIdUnidadTrabajoOrdenVenta, OrdenVenta.dFechaProgramacionAlmacenCabeceraOrdenVenta, OrdenVenta.cIdPersonal,
                                           OrdenVenta.vDescripcionCabeceraOrdenVenta, OrdenVenta.dFechaOrdenVentaCabeceraOrdenVenta, OrdenVenta.dFechaAsignacionAuxiliarCabeceraOrdenVenta,
                                           OrdenVenta.dFechaOrdenFabricacionCabeceraOrdenVenta, OrdenVenta.bTieneOrdenFabricacionCabeceraOrdenVenta,
                                           OrdenVenta.vOrdenVentaSAPCabeceraOrdenVenta, OrdenVenta.vOrdenCompraSAPCabeceraOrdenVenta, OrdenVenta.vOrdenFabricacionSAPCabeceraOrdenVenta,
                                           OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta).ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenVentaElimina(ByVal OrdenVenta As LOGI_CABECERAORDENVENTA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_NONE", "UPDATE LOGI_CABECERAORDENVENTA SET bEstadoRegistroCabeceraOrdenVenta = 0 WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & OrdenVenta.cIdTipoDocumentoCabeceraOrdenVenta & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & OrdenVenta.vIdNumeroSerieCabeceraOrdenVenta & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & OrdenVenta.vIdNumeroCorrelativoCabeceraOrdenVenta & "' AND cIdEmpresa = '" & OrdenVenta.cIdEmpresa & "' ",
                                           "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, Now, "1", "", "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function OrdenVentaExiste(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdNroOF As String, ByVal IdEmpresa As String) As Boolean
        If Data.PA_LOGI_MNT_CABECERAORDENVENTA("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENVENTA " &
                                                     "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & IdNroDoc & "' AND vOrdenFabricacionCabeceraOrdenVenta = '" & IdNroOF & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                      " AND bEstadoRegistroCabeceraOrdenVenta = 1",
                                      "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", Now, "", Now, "", "", Now, Now, Now, "1", "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function OrdenVentaListarPorIdDetalle(ByVal IdTipDoc As String, ByVal IdNroSerie As String, ByVal IdNroDoc As String, ByVal IdNroOF As String, ByVal IdEmpresa As String, ByVal IdOrdenFabricacion As String, Optional IdArticuloSAPDetalle As String = "*") As LOGI_DETALLEORDENVENTA
        'Data.PA_LOGI_MNT_CABECERAOrdenVenta("SQL_NONE", "SELECT * FROM LOGI_CABECERAORDENVENTA " &
        '                                             "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
        '                              " AND bEstadoRegistroCabeceraOrdenVenta = 1 AND IdArticuloSAPCabeceraOrdenTrabajo = '" & IdArticuloSAPCabecera & "' AND IdArticuloSAPDetalleOrdenTrabajo = '" & IdArticuloSAPDetalle & "' ",
        '                              "", "", "", "", "", "", Now, "", "", "", "", "", "", "", "1", Now, Now, "", "", "").Count > 0 Then
        Dim Consulta = Data.PA_LOGI_MNT_DETALLEORDENVENTA("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENVENTA " &
                                                     "WHERE cIdTipoDocumentoCabeceraOrdenVenta = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraOrdenVenta = '" & IdNroSerie & "' AND vIdNumeroCorrelativoCabeceraOrdenVenta = '" & IdNroDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                      " AND vOrdenFabricacionCabeceraOrdenVenta = '" & IdOrdenFabricacion & "' AND (vIdArticuloSAPDetalleOrdenVenta = '" & IdArticuloSAPDetalle & "' OR '*' = '" & IdArticuloSAPDetalle & "') ",
                                      "", "", "", "", 0, "", "", "", 0, "", 0, 0, 0, "")

        Dim Coleccion As New LOGI_DETALLEORDENVENTA
        For Each Busqueda In Consulta
            Coleccion.cIdTipoDocumentoCabeceraOrdenVenta = Busqueda.cIdTipoDocumentoCabeceraOrdenVenta
            Coleccion.vIdNumeroSerieCabeceraOrdenVenta = Busqueda.vIdNumeroSerieCabeceraOrdenVenta
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenVenta = Busqueda.vIdNumeroCorrelativoCabeceraOrdenVenta
            Coleccion.cIdEmpresa = Busqueda.cIdEmpresa
            Coleccion.nIdNumeroItemDetalleOrdenVenta = Busqueda.nIdNumeroItemDetalleOrdenVenta
            'Coleccion.vIdArticuloSAPCabeceraOrdenVenta = Busqueda.vIdArticuloSAPCabeceraOrdenVenta
            'Coleccion.cIdEquipoSAPCabeceraOrdenVenta = Busqueda.cIdEquipoSAPCabeceraOrdenVenta
            Coleccion.vIdArticuloSAPDetalleOrdenVenta = Busqueda.vIdArticuloSAPDetalleOrdenVenta
            Coleccion.vDescripcionArticuloSAPDetalleOrdenVenta = Busqueda.vDescripcionArticuloSAPDetalleOrdenVenta
            Coleccion.nCantidadSAPDetalleOrdenVenta = Busqueda.nCantidadSAPDetalleOrdenVenta
            Coleccion.vDescripcionUnidadMedidaSAPDetalleOrdenVenta = Busqueda.vDescripcionUnidadMedidaSAPDetalleOrdenVenta
            Coleccion.nTotalCantidadConsumidoDetalleOrdenVenta = Busqueda.nTotalCantidadConsumidoDetalleOrdenVenta
        Next
        Return Coleccion
    End Function
End Class