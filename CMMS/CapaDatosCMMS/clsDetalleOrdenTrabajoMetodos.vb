Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsDetalleOrdenTrabajoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function DetalleOrdenTrabajoGetData(strQuery As String) As DataTable
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

    'Public Function DetalleOrdenTrabajoListarCombo() As List(Of LOGI_DETALLEORDENTRABAJO)
    '    Dim Consulta = Data.PA_LOGI_MNT_DetalleOrdenTrabajo("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENTRABAJO",
    '                                                       "", "", "", "", "", 0, "", "1", "")
    '    Dim Coleccion As New List(Of LOGI_DETALLEORDENTRABAJO)
    '    For Each DetalleOrdenTrabajo In Consulta
    '        Dim TipAct As New LOGI_DETALLEORDENTRABAJO
    '        TipAct.cIdActividadCheckList = DetalleOrdenTrabajo.cIdActividadCheckList
    '        TipAct.vDescripcionDetalleOrdenTrabajo = DetalleOrdenTrabajo.vDescripcionDetalleOrdenTrabajo
    '        Coleccion.Add(TipAct)
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function DetalleOrdenTrabajoListarPorIdCatalogo(ByVal IdActividadCheckList As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_DETALLEORDENTRABAJO
    '    Dim Consulta = Data.PA_LOGI_MNT_DetalleOrdenTrabajo("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENTRABAJO " &
    '                                                       "WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "'" &
    '                                                       "      AND cIdCatalogo = '" & IdCatalogo & "'" &
    '                                                       "      AND cIdJerarquiaCatalogo = '" & IdJerarquiaCatalogo & "'" &
    '                                                       "      AND bEstadoRegistroDetalleOrdenTrabajo = 1 ", "", "", "", "", "", 0, "", "1", "")
    '    Dim Coleccion As New LOGI_DETALLEORDENTRABAJO
    '    For Each LOGI_DETALLEORDENTRABAJO In Consulta
    '        Coleccion.cIdActividadCheckList = LOGI_DETALLEORDENTRABAJO.cIdActividadCheckList
    '        Coleccion.cIdTipoMantenimiento = LOGI_DETALLEORDENTRABAJO.cIdTipoMantenimiento
    '        Coleccion.cIdCatalogo = LOGI_DETALLEORDENTRABAJO.cIdCatalogo
    '        Coleccion.cIdJerarquiaCatalogo = LOGI_DETALLEORDENTRABAJO.cIdJerarquiaCatalogo
    '        Coleccion.nIdNumeroItemDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.nIdNumeroItemDetalleOrdenTrabajo
    '        Coleccion.vDescripcionDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.vDescripcionDetalleOrdenTrabajo
    '        Coleccion.bEstadoRegistroDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.bEstadoRegistroDetalleOrdenTrabajo
    '        Coleccion.cIdTipoActivo = LOGI_DETALLEORDENTRABAJO.cIdTipoActivo
    '    Next
    '    Return Coleccion
    'End Function


    'Public Function DetalleOrdenTrabajoListarPorId(ByVal IdDetalleOrdenTrabajo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_DETALLEORDENTRABAJO
    'Public Function DetalleOrdenTrabajoListarPorId(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String) As LOGI_DETALLEORDENTRABAJO
    Public Function DetalleOrdenTrabajoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticulo As String) As LOGI_DETALLEORDENTRABAJO
        Dim Consulta = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENTRABAJO " &
                                                                   "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' " &
                                                                   "      AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' " &
                                                                   "      AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' " &
                                                                   "      AND cIdEquipoCabeceraOrdenTrabajo = '" & IdEquipo & "' " &
                                                                   "      AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                                   "      AND cIdCatalogoCheckListDetalleOrdenTrabajo = '" & IdCatalogo & "' " &
                                                                   "      AND cIdJerarquiaCheckListDetalleOrdenTrabajo = '" & IdJerarquia & "' " &
                                                                   "      AND cIdActividadCheckListDetalleOrdenTrabajo = '" & IdActividad & "' " &
                                                                   "      AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & IdArticulo & "'",
                                                                   "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "")
        Dim Coleccion As New LOGI_DETALLEORDENTRABAJO
        For Each LOGI_DETALLEORDENTRABAJO In Consulta
            Coleccion.cIdTipoDocumentoCabeceraOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.cIdTipoDocumentoCabeceraOrdenTrabajo
            Coleccion.vIdNumeroSerieCabeceraOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.vIdNumeroSerieCabeceraOrdenTrabajo
            Coleccion.vIdNumeroCorrelativoCabeceraOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.vIdNumeroCorrelativoCabeceraOrdenTrabajo
            Coleccion.cIdEmpresa = LOGI_DETALLEORDENTRABAJO.cIdEmpresa
            Coleccion.cIdEquipoCabeceraOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.cIdEquipoCabeceraOrdenTrabajo
            Coleccion.nIdNumeroItemDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.nIdNumeroItemDetalleOrdenTrabajo
            Coleccion.cIdCatalogoCheckListDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.cIdCatalogoCheckListDetalleOrdenTrabajo
            Coleccion.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo
            Coleccion.cIdActividadCheckListDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.cIdActividadCheckListDetalleOrdenTrabajo
            Coleccion.vIdArticuloSAPCabeceraOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.vIdArticuloSAPCabeceraOrdenTrabajo
            Coleccion.vIdArticuloSAPDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.vIdArticuloSAPDetalleOrdenTrabajo
            Coleccion.vDescripcionArticuloDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.vDescripcionArticuloDetalleOrdenTrabajo
            Coleccion.nCantidadArticuloDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.nCantidadArticuloDetalleOrdenTrabajo
            Coleccion.vDescripcionUnidadMedidaDetalleOrdenTrabajo = LOGI_DETALLEORDENTRABAJO.vDescripcionUnidadMedidaDetalleOrdenTrabajo
        Next
        Return Coleccion
    End Function

    'Public Function DetalleOrdenTrabajoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_DETALLEORDENTRABAJO)
    Public Function DetalleOrdenTrabajoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_DETALLEORDENTRABAJO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_NONE", "SELECT CHKLISPLA.cIdTipoMantenimiento, CHKLISPLA.cIdActividadCheckList, CHKLISPLA.cIdCatalogo, CHKLISPLA.cIdJerarquiaCatalogo, " &
        '                                                               "       TIPMAN.vDescripcionTipoMantenimiento, CAT.vDescripcionCatalogo, ACTCHKLIS.vDescripcionActividadCheckList, CHKLISPLA.bEstadoRegistroDetalleOrdenTrabajo " &
        '                                                               "FROM LOGI_DETALLEORDENTRABAJO AS CHKLISPLA INNER JOIN " &
        '                                                               "LOGI_ACTIVIDADCHECKLIST AS ACTCHKLIS ON " &
        '                                                               "CHKLISPLA.cIdActividadCheckList = ACTCHKLIS.cIdActividadCheckList " &
        '                                                               "INNER JOIN LOGI_TIPOMANTENIMIENTO AS TIPMAN ON " &
        '                                                               "CHKLISPLA.cIdTipoMantenimiento = TIPMAN.cIdTipoMantenimiento " &
        '                                                               "INNER JOIN LOGI_CATALOGO AS CAT ON " &
        '                                                               "CHKLISPLA.cIdCatalogo = CAT.cIdCatalogo AND " &
        '                                                               "CHKLISPLA.cIdJerarquiaCatalogo = CAT.cIdJerarquiaCatalogo " &
        '                                                               "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CHKLISPLA.bEstadoRegistroDetalleOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "') ",
        '                                                               "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "")
        Dim Consulta = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_NONE", "SELECT DETORDTRA.vIdArticuloSAPDetalleOrdenTrabajo, DETORDTRA.vDescripcionArticuloDetalleOrdenTrabajo, DETORDTRA.nCantidadArticuloDetalleOrdenTrabajo, DETORDTRA.vDescripcionUnidadMedidaDetalleOrdenTrabajo " &
                                                                       "FROM LOGI_DETALLEORDENTRABAJO AS DETORDTRA " &
                                                                       "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CHKLISPLA.bEstadoRegistroDetalleOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                                                       "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "")
        Dim Coleccion As New List(Of VI_LOGI_DETALLEORDENTRABAJO)
        For Each Busqueda In Consulta
            Dim BuscarDetOrd As New VI_LOGI_DETALLEORDENTRABAJO
            BuscarDetOrd.Codigo = Busqueda.vIdArticuloSAPDetalleOrdenTrabajo
            BuscarDetOrd.DescripcionArticuloSAP = Busqueda.vDescripcionArticuloDetalleOrdenTrabajo
            BuscarDetOrd.CantidadSAP = Busqueda.nCantidadArticuloDetalleOrdenTrabajo
            BuscarDetOrd.DescripcionUnidadMedidaSAP = Busqueda.vDescripcionUnidadMedidaDetalleOrdenTrabajo
            Coleccion.Add(BuscarDetOrd)
        Next
        Return Coleccion
    End Function

    Public Function DetalleOrdenTrabajoInserta(ByVal DetalleOrdenTrabajo As LOGI_DETALLEORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_INSERT", "", DetalleOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo,
                                        DetalleOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdEmpresa, DetalleOrdenTrabajo.nIdNumeroItemDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdArticuloSAPDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.vDescripcionArticuloDetalleOrdenTrabajo, DetalleOrdenTrabajo.nCantidadArticuloDetalleOrdenTrabajo, DetalleOrdenTrabajo.vDescripcionUnidadMedidaDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.cIdCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdActividadCheckListDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    Public Function DetalleOrdenTrabajoInsertaDetalle(ByVal DetalleOrdenTrabajo As List(Of LOGI_DETALLEORDENTRABAJO), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_NONE", "DELETE LOGI_DETALLEORDENTRABAJO " &
                                         "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & DetalleOrdenTrabajo(0).cIdTipoDocumentoCabeceraOrdenTrabajo & "' " &
                                         "      AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & DetalleOrdenTrabajo(0).vIdNumeroSerieCabeceraOrdenTrabajo & "' " &
                                         "      AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & DetalleOrdenTrabajo(0).vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' " &
                                         "      AND cIdEmpresa = '" & DetalleOrdenTrabajo(0).cIdEmpresa & "' ",
                                         "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "").ReturnValue.ToString

            For Each Busqueda In DetalleOrdenTrabajo
                'If bExiste = False Then
                x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_INSERT", "", Busqueda.cIdTipoDocumentoCabeceraOrdenTrabajo, Busqueda.vIdNumeroSerieCabeceraOrdenTrabajo,
                                        Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo, Busqueda.cIdEmpresa, Busqueda.nIdNumeroItemDetalleOrdenTrabajo,
                                        Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo, Busqueda.cIdEquipoCabeceraOrdenTrabajo, Busqueda.vIdArticuloSAPDetalleOrdenTrabajo,
                                        Busqueda.vDescripcionArticuloDetalleOrdenTrabajo, Busqueda.nCantidadArticuloDetalleOrdenTrabajo, Busqueda.vDescripcionUnidadMedidaDetalleOrdenTrabajo,
                                        Busqueda.cIdCatalogoCheckListDetalleOrdenTrabajo, Busqueda.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo, Busqueda.cIdActividadCheckListDetalleOrdenTrabajo,
                                        Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString

                LogAuditoria.vEvento = "INSERTAR DETALLE ORDEN TRABAJO"
                'LogAuditoria.vQuery = "PA_LOGI_MNT_CATALOGOCARACTERISTICA 'SQL_INSERT', '', '" & Cat.cIdCatalogo & "', '" &
                '                              DetDocumento.cIdJerarquiaCatalogo & "', '" & DetDocumento.cIdCaracteristica & "', " &
                '                              DetDocumento.nIdNumeroItemCatalogoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPCatalogoCaracteristica & "', '" &
                '                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica & "', '" & DetDocumento.vValorCatalogoCaracteristica & "', '" & DetDocumento.cIdCaracteristica & "'"
                LogAuditoria.vQuery = "PA_LOGI_MNT_DETALLEORDENTRABAJO 'SQL_INSERT', '', '" & Busqueda.cIdTipoDocumentoCabeceraOrdenTrabajo & "', '" & Busqueda.vIdNumeroSerieCabeceraOrdenTrabajo & "', '" &
                                        Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "', '" & Busqueda.cIdEmpresa & "', " & Busqueda.nIdNumeroItemDetalleOrdenTrabajo & ", '" &
                                        Busqueda.vIdArticuloSAPCabeceraOrdenTrabajo & "', '" & Busqueda.cIdEquipoCabeceraOrdenTrabajo & "', '" & Busqueda.vIdArticuloSAPDetalleOrdenTrabajo & "', '" &
                                        Busqueda.vDescripcionArticuloDetalleOrdenTrabajo & "', " & Busqueda.nCantidadArticuloDetalleOrdenTrabajo & ", '" & Busqueda.vDescripcionUnidadMedidaDetalleOrdenTrabajo & "', '" &
                                        Busqueda.cIdCatalogoCheckListDetalleOrdenTrabajo & "', '" & Busqueda.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo & "', '" & Busqueda.cIdActividadCheckListDetalleOrdenTrabajo & "', '" &
                                        Busqueda.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "'"

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function DetalleOrdenTrabajoEdita(ByVal DetalleOrdenTrabajo As LOGI_DETALLEORDENTRABAJO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_UPDATE", "", DetalleOrdenTrabajo.cIdTipoDocumentoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdNumeroSerieCabeceraOrdenTrabajo,
                                        DetalleOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdEmpresa, DetalleOrdenTrabajo.nIdNumeroItemDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.vIdArticuloSAPCabeceraOrdenTrabajo, DetalleOrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo, DetalleOrdenTrabajo.vIdArticuloSAPDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.vDescripcionArticuloDetalleOrdenTrabajo, DetalleOrdenTrabajo.nCantidadArticuloDetalleOrdenTrabajo, DetalleOrdenTrabajo.vDescripcionUnidadMedidaDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.cIdCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo, DetalleOrdenTrabajo.cIdActividadCheckListDetalleOrdenTrabajo,
                                        DetalleOrdenTrabajo.vIdNumeroCorrelativoCabeceraOrdenTrabajo).ReturnValue.ToString
        Return x
    End Function

    'Public Function DetalleOrdenTrabajoElimina(ByVal DetalleOrdenTrabajo As LOGI_DETALLEORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_NONE", "UPDATE LOGI_DETALLEORDENTRABAJO SET bEstadoRegistroDetalleOrdenTrabajo = 0 WHERE cIdActividadCheckList = '" & DetalleOrdenTrabajo.cIdActividadCheckList & "' " &
    '                                           " AND cIdTipoMantenimiento = '" & DetalleOrdenTrabajo.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & DetalleOrdenTrabajo.cIdNumeroCabeceraCheckListPlantilla & "' AND cIdCatalogo = '" & DetalleOrdenTrabajo.cIdCatalogo & "' AND cIdJerarquiaCatalogo = '" & DetalleOrdenTrabajo.cIdJerarquiaCatalogo & "'",
    '                                           "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "").ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function DetalleOrdenTrabajoExiste(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String, ByVal IdActividadCheckList As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As Boolean
    Public Function DetalleOrdenTrabajoExiste(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticulo As String) As Boolean
        'If Data.PA_LOGI_MNT_DetalleOrdenTrabajo("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENTRABAJO WHERE cIdDetalleOrdenTrabajo = '" & IdDetalleOrdenTrabajo & "' " &
        '                             " AND bEstadoRegistroDetalleOrdenTrabajo = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_DETALLEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_DETALLEORDENTRABAJO " &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' " &
                                                "      AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' " &
                                                "      AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroDoc & "' " &
                                                "      AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                "      AND cIdEquipoCabeceraOrdenTrabajo = '" & IdEquipo & "' " &
                                                "      AND cIdCatalogoCheckListDetalleOrdenTrabajo = '" & IdCatalogo & "' " &
                                                "      AND cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo = '" & IdJerarquia & "' " &
                                                "      AND cIdActividadCheckListDetalleOrdenTrabajo = '" & IdActividad & "' " &
                                                "      AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & IdArticulo & "'",
                                               "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
