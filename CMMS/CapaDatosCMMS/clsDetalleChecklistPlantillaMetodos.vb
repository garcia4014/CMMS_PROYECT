Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsDetalleChecklistPlantillaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function DetalleChecklistPlantillaGetData(strQuery As String) As DataTable
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

    'Public Function DetalleChecklistPlantillaListarCombo() As List(Of LOGI_DETALLECHECKLISTPLANTILLA)
    '    Dim Consulta = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_DETALLECHECKLISTPLANTILLA",
    '                                                       "", "", "", "", "", 0, "", "1", "")
    '    Dim Coleccion As New List(Of LOGI_DETALLECHECKLISTPLANTILLA)
    '    For Each DetalleChecklistPlantilla In Consulta
    '        Dim TipAct As New LOGI_DETALLECHECKLISTPLANTILLA
    '        TipAct.cIdActividadCheckList = DetalleChecklistPlantilla.cIdActividadCheckList
    '        TipAct.vDescripcionDetalleChecklistPlantilla = DetalleChecklistPlantilla.vDescripcionDetalleChecklistPlantilla
    '        Coleccion.Add(TipAct)
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function DetalleChecklistPlantillaListarPorIdCatalogo(ByVal IdActividadCheckList As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_DETALLECHECKLISTPLANTILLA
    '    Dim Consulta = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_DETALLECHECKLISTPLANTILLA " &
    '                                                       "WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "'" &
    '                                                       "      AND cIdCatalogo = '" & IdCatalogo & "'" &
    '                                                       "      AND cIdJerarquiaCatalogo = '" & IdJerarquiaCatalogo & "'" &
    '                                                       "      AND bEstadoRegistroDetalleChecklistPlantilla = 1 ", "", "", "", "", "", 0, "", "1", "")
    '    Dim Coleccion As New LOGI_DETALLECHECKLISTPLANTILLA
    '    For Each LOGI_DETALLECHECKLISTPLANTILLA In Consulta
    '        Coleccion.cIdActividadCheckList = LOGI_DETALLECHECKLISTPLANTILLA.cIdActividadCheckList
    '        Coleccion.cIdTipoMantenimiento = LOGI_DETALLECHECKLISTPLANTILLA.cIdTipoMantenimiento
    '        Coleccion.cIdCatalogo = LOGI_DETALLECHECKLISTPLANTILLA.cIdCatalogo
    '        Coleccion.cIdJerarquiaCatalogo = LOGI_DETALLECHECKLISTPLANTILLA.cIdJerarquiaCatalogo
    '        Coleccion.nIdNumeroItemDetalleChecklistPlantilla = LOGI_DETALLECHECKLISTPLANTILLA.nIdNumeroItemDetalleChecklistPlantilla
    '        Coleccion.vDescripcionDetalleCheckListPlantilla = LOGI_DETALLECHECKLISTPLANTILLA.vDescripcionDetalleCheckListPlantilla
    '        Coleccion.bEstadoRegistroDetalleChecklistPlantilla = LOGI_DETALLECHECKLISTPLANTILLA.bEstadoRegistroDetalleChecklistPlantilla
    '        Coleccion.cIdTipoActivo = LOGI_DETALLECHECKLISTPLANTILLA.cIdTipoActivo
    '    Next
    '    Return Coleccion
    'End Function


    'Public Function DetalleChecklistPlantillaListarPorId(ByVal IdDetalleChecklistPlantilla As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_DETALLECHECKLISTPLANTILLA
    Public Function DetalleChecklistPlantillaListarPorId(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String) As LOGI_DETALLECHECKLISTPLANTILLA
        Dim Consulta = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_DETALLECHECKLISTPLANTILLA " &
                                                           "WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "'" &
                                                           "      AND cIdNumeroCabeceraCheckListPlantilla = '" & IdNumero & "'" &
                                                           "      AND bEstadoRegistroDetalleChecklistPlantilla = 1 ", "", "", "", "", "", "", 0, "", "1", "")
        Dim Coleccion As New LOGI_DETALLECHECKLISTPLANTILLA
        For Each LOGI_DETALLECHECKLISTPLANTILLA In Consulta
            Coleccion.cIdActividadCheckList = LOGI_DETALLECHECKLISTPLANTILLA.cIdActividadCheckList
            Coleccion.cIdTipoMantenimiento = LOGI_DETALLECHECKLISTPLANTILLA.cIdTipoMantenimiento
            Coleccion.cIdNumeroCabeceraCheckListPlantilla = LOGI_DETALLECHECKLISTPLANTILLA.cIdNumeroCabeceraCheckListPlantilla
            Coleccion.cIdCatalogo = LOGI_DETALLECHECKLISTPLANTILLA.cIdCatalogo
            Coleccion.cIdJerarquiaCatalogo = LOGI_DETALLECHECKLISTPLANTILLA.cIdJerarquiaCatalogo
            Coleccion.nIdNumeroItemDetalleCheckListPlantilla = LOGI_DETALLECHECKLISTPLANTILLA.nIdNumeroItemDetalleCheckListPlantilla
            Coleccion.vDescripcionDetalleCheckListPlantilla = LOGI_DETALLECHECKLISTPLANTILLA.vDescripcionDetalleCheckListPlantilla
            Coleccion.bEstadoRegistroDetalleCheckListPlantilla = LOGI_DETALLECHECKLISTPLANTILLA.bEstadoRegistroDetalleCheckListPlantilla
            Coleccion.cIdTipoActivo = LOGI_DETALLECHECKLISTPLANTILLA.cIdTipoActivo
        Next
        Return Coleccion
    End Function

    'Public Function DetalleChecklistPlantillaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_DETALLECHECKLISTPLANTILLA)
    Public Function DetalleChecklistPlantillaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_DETALLECHECKLISTPLANTILLA)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT CHKLISPLA.cIdTipoMantenimiento, CHKLISPLA.cIdCatalogo, CHKLISPLA.cIdJerarquiaCatalogo, " &
        '                                                               "       TIPMAN.vDescripcionTipoMantenimiento, CAT.vDescripcionCatalogo " &
        '                                                               "FROM LOGI_DETALLECHECKLISTPLANTILLA AS CHKLISPLA INNER JOIN " &
        '                                                               "LOGI_ACTIVIDADCHECKLIST AS ACTCHKLIS ON " &
        '                                                               "CHKLISPLA.cIdActividadCheckList = ACTCHKLIS.cIdActividadCheckList " &
        '                                                               "INNER JOIN LOGI_TIPOMANTENIMIENTO AS TIPMAN ON " &
        '                                                               "CHKLISPLA.cIdTipoMantenimiento = TIPMAN.cIdTipoMantenimiento " &
        '                                                               "INNER JOIN LOGI_CATALOGO AS CAT ON " &
        '                                                               "CHKLISPLA.cIdCatalogo = CAT.cIdCatalogo AND " &
        '                                                               "CHKLISPLA.cIdJerarquiaCatalogo = CAT.cIdJerarquiaCatalogo " &
        '                                                               "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                                               "GROUP BY CHKLISPLA.cIdTipoMantenimiento, CHKLISPLA.cIdCatalogo, CHKLISPLA.cIdJerarquiaCatalogo, " &
        '                                                               "         TIPMAN.vDescripcionTipoMantenimiento, CAT.vDescripcionCatalogo ",
        '                                                               "", "", "", "", "", "", 0, "", "1", "")
        'Dim Consulta = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT CHKLISPLA.cIdTipoMantenimiento, CHKLISPLA.cIdActividadCheckList, CHKLISPLA.cIdCatalogo, CHKLISPLA.cIdJerarquiaCatalogo, " &
        '                                                               "       TIPMAN.vDescripcionTipoMantenimiento, CAT.vDescripcionCatalogo, ACTCHKLIS.vDescripcionActividadCheckList, CHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla " &
        '                                                               "FROM LOGI_DETALLECHECKLISTPLANTILLA AS CHKLISPLA INNER JOIN " &
        '                                                               "LOGI_ACTIVIDADCHECKLIST AS ACTCHKLIS ON " &
        '                                                               "CHKLISPLA.cIdActividadCheckList = ACTCHKLIS.cIdActividadCheckList " &
        '                                                               "INNER JOIN LOGI_TIPOMANTENIMIENTO AS TIPMAN ON " &
        '                                                               "CHKLISPLA.cIdTipoMantenimiento = TIPMAN.cIdTipoMantenimiento " &
        '                                                               "INNER JOIN LOGI_CATALOGO AS CAT ON " &
        '                                                               "CHKLISPLA.cIdCatalogo = CAT.cIdCatalogo AND " &
        '                                                               "CHKLISPLA.cIdJerarquiaCatalogo = CAT.cIdJerarquiaCatalogo " &
        '                                                               "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '" & Estado & "' OR '*' = '" & Estado & "') ",
        '                                                               "", "", "", "", "", "", 0, "", "1", "")
        Dim Consulta = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT CHKLISPLA.cIdTipoMantenimiento, CHKLISPLA.cIdActividadCheckList, CHKLISPLA.cIdCatalogo, CHKLISPLA.cIdJerarquiaCatalogo, " &
                                                                       "       TIPMAN.vDescripcionTipoMantenimiento, CAT.vDescripcionCatalogo, ACTCHKLIS.vDescripcionActividadCheckList, CHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla " &
                                                                       "FROM LOGI_DETALLECHECKLISTPLANTILLA AS CHKLISPLA INNER JOIN " &
                                                                       "LOGI_ACTIVIDADCHECKLIST AS ACTCHKLIS ON " &
                                                                       "CHKLISPLA.cIdActividadCheckList = ACTCHKLIS.cIdActividadCheckList " &
                                                                       "INNER JOIN LOGI_TIPOMANTENIMIENTO AS TIPMAN ON " &
                                                                       "CHKLISPLA.cIdTipoMantenimiento = TIPMAN.cIdTipoMantenimiento " &
                                                                       "INNER JOIN LOGI_CATALOGO AS CAT ON " &
                                                                       "CHKLISPLA.cIdCatalogo = CAT.cIdCatalogo AND " &
                                                                       "CHKLISPLA.cIdJerarquiaCatalogo = CAT.cIdJerarquiaCatalogo " &
                                                                       "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                                       "ORDER BY CHKLISPLA.cIdTipoMantenimiento, CHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, CHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla",
                                                                       "", "", "", "", "", "", 0, "", "1", "")
        Dim Coleccion As New List(Of VI_LOGI_DETALLECHECKLISTPLANTILLA)
        For Each Busqueda In Consulta
            Dim BuscarFam As New VI_LOGI_DETALLECHECKLISTPLANTILLA
            BuscarFam.Codigo = Busqueda.cIdActividadCheckList
            BuscarFam.DescripcionActividad = Busqueda.vDescripcionActividadCheckList
            BuscarFam.Estado = Busqueda.bEstadoRegistroDetalleCheckListPlantilla
            BuscarFam.IdTipoMantenimiento = Busqueda.cIdTipoMantenimiento
            BuscarFam.DescripcionTipoMantenimiento = Busqueda.vDescripcionTipoMantenimiento
            BuscarFam.IdCatalogo = Busqueda.cIdCatalogo
            BuscarFam.DescripcionCatalogo = Busqueda.vDescripcionCatalogo
            Coleccion.Add(BuscarFam)
        Next
        Return Coleccion
    End Function

    Public Function DetalleChecklistPlantillaInserta(ByVal DetalleChecklistPlantilla As LOGI_DETALLECHECKLISTPLANTILLA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_INSERT", "", DetalleChecklistPlantilla.cIdActividadCheckList, DetalleChecklistPlantilla.cIdTipoMantenimiento,
                                        DetalleChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla, DetalleChecklistPlantilla.cIdTipoActivo, DetalleChecklistPlantilla.cIdCatalogo,
                                        DetalleChecklistPlantilla.cIdJerarquiaCatalogo, DetalleChecklistPlantilla.nIdNumeroItemDetalleCheckListPlantilla,
                                        DetalleChecklistPlantilla.vDescripcionDetalleCheckListPlantilla, DetalleChecklistPlantilla.bEstadoRegistroDetalleCheckListPlantilla,
                                        DetalleChecklistPlantilla.cIdActividadCheckList).ReturnValue.ToString
        Return x
    End Function

    'Public Function DetalleChecklistPlantillaInsertaDetalle(ByVal DetalleChecklistPlantilla As List(Of LOGI_DETALLECHECKLISTPLANTILLA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
    Public Function DetalleChecklistPlantillaInsertaDetalle(ByVal CabeceraChecklistPlantilla As LOGI_CABECERACHECKLISTPLANTILLA, ByVal DetalleChecklistPlantilla As List(Of LOGI_DETALLECHECKLISTPLANTILLA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            If DetalleChecklistPlantilla.Count() = 0 Then
                x = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "DELETE LOGI_DETALLECHECKLISTPLANTILLA " &
                                         "WHERE cIdTipoMantenimiento = '" & CabeceraChecklistPlantilla.cIdTipoMantenimiento & "' " &
                                         "      AND cIdNumeroCabeceraCheckListPlantilla = '" & CabeceraChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla & "'",
                                         "", "", "", "", "", "", 0, "", "1", "").ReturnValue.ToString
            Else
                x = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "DELETE LOGI_DETALLECHECKLISTPLANTILLA " &
                                         "WHERE cIdTipoMantenimiento = '" & DetalleChecklistPlantilla(0).cIdTipoMantenimiento & "' " &
                                         "      AND cIdNumeroCabeceraCheckListPlantilla = '" & DetalleChecklistPlantilla(0).cIdNumeroCabeceraCheckListPlantilla & "'",
                                         "", "", "", "", "", "", 0, "", "1", "").ReturnValue.ToString
            End If

            For Each Busqueda In DetalleChecklistPlantilla
                'If bExiste = False Then
                x = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_INSERT", "", Busqueda.cIdActividadCheckList, Busqueda.cIdTipoMantenimiento,
                                        Busqueda.cIdNumeroCabeceraCheckListPlantilla, Busqueda.cIdTipoActivo, Busqueda.cIdCatalogo,
                                        Busqueda.cIdJerarquiaCatalogo, Busqueda.nIdNumeroItemDetalleCheckListPlantilla,
                                        Busqueda.vDescripcionDetalleCheckListPlantilla, Busqueda.bEstadoRegistroDetalleCheckListPlantilla,
                                        Busqueda.cIdNumeroCabeceraCheckListPlantilla).ReturnValue.ToString
                'Else
                '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "UPDATE LOGI_EQUIPO SET vDescripcionEquipo = '" & Busqueda.vDescripcionCatalogo & "', vDescripcionAbreviadaEquipo = '" & Busqueda.vDescripcionAbreviadaCatalogo & "' " &
                '                                  "WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "' and cIdCatalogo = '" & Busqueda.cIdCatalogo & "'", Busqueda.cIdCatalogo, Busqueda.cIdJerarquiaCatalogo,
                '              Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
                '              Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
                '              Busqueda.cIdCuentaContableCatalogo, Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo, Busqueda.cIdTipoActivo,
                '               Busqueda.nPeriodoGarantiaCatalogo, Busqueda.nPeriodoMinimoMantenimientoCatalogo,
                '              Busqueda.cIdCatalogo).ReturnValue.ToString

                '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_UPDATE", "", Busqueda.cIdCatalogo, Busqueda.cIdJerarquiaCatalogo,
                '                                  Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
                '                                  Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
                '                                  Busqueda.cIdCuentaContableCatalogo, Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo,
                '                                  Busqueda.cIdTipoActivo, Busqueda.nPeriodoGarantiaCatalogo, Busqueda.nPeriodoMinimoMantenimientoCatalogo,
                '                                  Busqueda.cIdCatalogo).ReturnValue.ToString
                'End If
                LogAuditoria.vEvento = "INSERTAR CHECK DETALLE LIST PLANTILLA"
                'LogAuditoria.vQuery = "PA_LOGI_MNT_CATALOGOCARACTERISTICA 'SQL_INSERT', '', '" & Cat.cIdCatalogo & "', '" &
                '                              DetDocumento.cIdJerarquiaCatalogo & "', '" & DetDocumento.cIdCaracteristica & "', " &
                '                              DetDocumento.nIdNumeroItemCatalogoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPCatalogoCaracteristica & "', '" &
                '                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica & "', '" & DetDocumento.vValorCatalogoCaracteristica & "', '" & DetDocumento.cIdCaracteristica & "'"
                LogAuditoria.vQuery = "PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA 'SQL_INSERT', '', '" & Busqueda.cIdActividadCheckList & "', '" & Busqueda.cIdTipoMantenimiento & "', '" &
                                        Busqueda.cIdNumeroCabeceraCheckListPlantilla & "', '" & Busqueda.cIdTipoActivo & "', '" & Busqueda.cIdCatalogo & "', '" &
                                        Busqueda.cIdJerarquiaCatalogo & "', " & Busqueda.nIdNumeroItemDetalleCheckListPlantilla & ", '" &
                                        Busqueda.vDescripcionDetalleCheckListPlantilla & "', '" & Busqueda.bEstadoRegistroDetalleCheckListPlantilla & "', '" &
                                        Busqueda.cIdNumeroCabeceraCheckListPlantilla & "'"

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function DetalleChecklistPlantillaEdita(ByVal DetalleChecklistPlantilla As LOGI_DETALLECHECKLISTPLANTILLA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_UPDATE", "", DetalleChecklistPlantilla.cIdActividadCheckList, DetalleChecklistPlantilla.cIdTipoMantenimiento,
                                        DetalleChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla, DetalleChecklistPlantilla.cIdTipoActivo, DetalleChecklistPlantilla.cIdCatalogo,
                                        DetalleChecklistPlantilla.cIdJerarquiaCatalogo, DetalleChecklistPlantilla.nIdNumeroItemDetalleCheckListPlantilla,
                                        DetalleChecklistPlantilla.vDescripcionDetalleCheckListPlantilla, DetalleChecklistPlantilla.bEstadoRegistroDetalleCheckListPlantilla,
                                        DetalleChecklistPlantilla.cIdActividadCheckList).ReturnValue.ToString
        Return x
    End Function

    Public Function DetalleChecklistPlantillaElimina(ByVal DetalleChecklistPlantilla As LOGI_DETALLECHECKLISTPLANTILLA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "UPDATE LOGI_DETALLECHECKLISTPLANTILLA SET bEstadoRegistroDetalleChecklistPlantilla = 0 WHERE cIdActividadCheckList = '" & DetalleChecklistPlantilla.cIdActividadCheckList & "' " &
                                               " AND cIdTipoMantenimiento = '" & DetalleChecklistPlantilla.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & DetalleChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla & "' AND cIdCatalogo = '" & DetalleChecklistPlantilla.cIdCatalogo & "' AND cIdJerarquiaCatalogo = '" & DetalleChecklistPlantilla.cIdJerarquiaCatalogo & "'",
                                               "", "", "", "", "", "", 0, "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function DetalleChecklistPlantillaExiste(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String, ByVal IdActividadCheckList As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As Boolean
        'If Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_DETALLECHECKLISTPLANTILLA WHERE cIdDetalleChecklistPlantilla = '" & IdDetalleChecklistPlantilla & "' " &
        '                             " AND bEstadoRegistroDetalleChecklistPlantilla = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_DETALLECHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_DETALLECHECKLISTPLANTILLA WHERE cIdNumeroCabeceraCheckListPlantilla = '" & IdNumero & "' AND cIdActividadCheckList = '" & IdActividadCheckList & "' " &
                                               " AND cIdTipoMantenimiento = '" & IdTipoMantenimiento & "' AND cIdCatalogo = '" & IdCatalogo & "' AND cIdJerarquiaCatalogo = '" & IdJerarquiaCatalogo & "'",
                                               "", "", "", "", "", "", 0, "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
