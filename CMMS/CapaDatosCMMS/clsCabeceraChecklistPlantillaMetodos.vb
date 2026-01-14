Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsCabeceraChecklistPlantillaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function ChecklistPlantillaGetData(strQuery As String) As DataTable
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

    Public Function ChecklistPlantillaListarCombo(ByVal IdTipoMantenimiento As String, ByVal IdTipoActivo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String) As List(Of LOGI_CABECERACHECKLISTPLANTILLA)
        Dim Consulta = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_CABECERACHECKLISTPLANTILLA WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "' AND cIdTipoActivoCabeceraCheckListPlantilla = '" & IdTipoActivo & "' AND cIdCatalogoCabeceraCheckListPlantilla = '" & IdCatalogo & "' AND cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '" & IdJerarquia & "'",
                                                           "", "", Now, "0", "", "", "1", "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERACHECKLISTPLANTILLA)
        For Each ChecklistPlantilla In Consulta
            Dim ChkLis As New LOGI_CABECERACHECKLISTPLANTILLA
            ChkLis.cIdNumeroCabeceraCheckListPlantilla = ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla
            ChkLis.vDescripcionCabeceraCheckListPlantilla = ChecklistPlantilla.vDescripcionCabeceraCheckListPlantilla
            Coleccion.Add(ChkLis)
        Next
        Return Coleccion
    End Function

    Public Function ChecklistPlantillaAllListarCombo() As List(Of LOGI_CABECERACHECKLISTPLANTILLA)
        Dim query = "SELECT cIdTipoMantenimiento, cIdNumeroCabeceraCheckListPlantilla, vDescripcionCabeceraCheckListPlantilla FROM   [dbo].[LOGI_CABECERACHECKLISTPLANTILLA] WHERE  bEstadoRegistroCabeceraCheckListPlantilla='1' and cIdJerarquiaCatalogoCabeceraCheckListPlantilla='0' and cIdNumeroCabeceraCheckListPlantilla>='00053'"
        Dim Consulta = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", query,
                                                           "", "", Now, "0", "", "", "1", "", "", "")
        Dim Coleccion As New List(Of LOGI_CABECERACHECKLISTPLANTILLA)
        For Each ChecklistPlantilla In Consulta
            Dim ChkLis As New LOGI_CABECERACHECKLISTPLANTILLA
            'ChkLis.cIdTipoMantenimiento = $"{ChecklistPlantilla.cIdTipoMantenimiento.Trim}{ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla.Trim}"
            ChkLis.cIdTipoMantenimiento = String.Concat(ChecklistPlantilla.cIdTipoMantenimiento.Trim(), ChecklistPlantilla.cIdNumeroCabeceraCheckListPlantilla.Trim())
            ChkLis.vDescripcionCabeceraCheckListPlantilla = ChecklistPlantilla.vDescripcionCabeceraCheckListPlantilla
            Coleccion.Add(ChkLis)
        Next
        Return Coleccion
    End Function

    'Public Function ChecklistPlantillaListarPorIdCatalogo(ByVal IdActividadCheckList As String, ByVal IdTipoMantenimiento As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As LOGI_CHECKLISTPLANTILLA
    '    Dim Consulta = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_CABECERACHECKLISTPLANTILLA " &
    '                                                       "WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "'" &
    '                                                       "      AND cIdCatalogo = '" & IdCatalogo & "'" &
    '                                                       "      AND cIdJerarquiaCatalogo = '" & IdJerarquiaCatalogo & "'" &
    '                                                       "      AND bEstadoRegistroCheckListPlantilla = 1 ", "", "", Now, "0", "", "", "1", "")
    '    Dim Coleccion As New LOGI_CABECERACHECKLISTPLANTILLA
    '    For Each LOGI_CABECERACHECKLISTPLANTILLA In Consulta
    '        'Coleccion.cIdActividadCheckList = LOGI_CHECKLISTPLANTILLA.cIdActividadCheckList
    '        Coleccion.cIdTipoMantenimiento = LOGI_CABECERACHECKLISTPLANTILLA.cIdTipoMantenimiento
    '        Coleccion.cIdNumeroCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdNumeroCabeceraCheckListPlantilla
    '        Coleccion.dFechaTransaccionCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.dFechaTransaccionCabeceraCheckListPlantilla
    '        Coleccion.bEstadoRegistroCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.bEstadoRegistroCabeceraCheckListPlantilla
    '        Coleccion.cIdTipoActivoCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdTipoActivoCabeceraCheckListPlantilla
    '        Coleccion.cIdCatalogoCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdCatalogoCabeceraCheckListPlantilla
    '        Coleccion.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla
    '    Next
    '    Return Coleccion
    'End Function


    'Public Function ChecklistPlantillaListarPorId(ByVal IdChecklistPlantilla As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As LOGI_CHECKLISTPLANTILLA
    Public Function ChecklistPlantillaListarPorId(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String) As LOGI_CABECERACHECKLISTPLANTILLA
        Dim Consulta = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_CABECERACHECKLISTPLANTILLA " &
                                                           "WHERE cIdTipoMantenimiento = '" & IdTipoMantenimiento & "'" &
                                                           "      AND cIdNumeroCabeceraCheckListPlantilla = '" & IdNumero & "'",
                                                           "", "", Now, "0", "", "", "1", "", "", "")
        Dim Coleccion As New LOGI_CABECERACHECKLISTPLANTILLA
        For Each LOGI_CABECERACHECKLISTPLANTILLA In Consulta
            Coleccion.cIdTipoMantenimiento = LOGI_CABECERACHECKLISTPLANTILLA.cIdTipoMantenimiento
            Coleccion.cIdNumeroCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdNumeroCabeceraCheckListPlantilla
            Coleccion.dFechaTransaccionCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.dFechaTransaccionCabeceraCheckListPlantilla
            Coleccion.bEstadoRegistroCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.bEstadoRegistroCabeceraCheckListPlantilla
            Coleccion.cIdTipoActivoCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdTipoActivoCabeceraCheckListPlantilla
            Coleccion.cIdCatalogoCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdCatalogoCabeceraCheckListPlantilla
            Coleccion.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla
            Coleccion.vDescripcionCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.vDescripcionCabeceraCheckListPlantilla
            Coleccion.vFormatoArchivoCabeceraCheckListPlantilla = LOGI_CABECERACHECKLISTPLANTILLA.vFormatoArchivoCabeceraCheckListPlantilla
        Next
        Return Coleccion
    End Function

    'Public Function ChecklistPlantillaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_CHECKLISTPLANTILLA)
    Public Function ChecklistPlantillaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_CABECERACHECKLISTPLANTILLA)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "SELECT CHKLIS.cIdTipoMantenimiento, CHKLIS.cIdNumeroCabeceraCheckListPlantilla, CHKLIS.cIdCatalogoCabeceraCheckListPlantilla, CHKLIS.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, " &
                                                                       "       TIPMAN.vDescripcionTipoMantenimiento, CAT.vDescripcionCatalogo, CHKLIS.dFechaTransaccionCabeceraCheckListPlantilla, CHKLIS.vDescripcionCabeceraCheckListPlantilla, " &
                                                                       "       CHKLIS.vFormatoArchivoCabeceraCheckListPlantilla, CHKLIS.bEstadoRegistroCabeceraCheckListPlantilla " &
                                                                       "FROM LOGI_CABECERACHECKLISTPLANTILLA AS CHKLIS " &
                                                                       "     INNER JOIN LOGI_TIPOMANTENIMIENTO AS TIPMAN ON " &
                                                                       "     CHKLIS.cIdTipoMantenimiento = TIPMAN.cIdTipoMantenimiento " &
                                                                       "     INNER JOIN LOGI_CATALOGO AS CAT ON " &
                                                                       "     CHKLIS.cIdCatalogoCabeceraCheckListPlantilla = CAT.cIdCatalogo AND " &
                                                                       "     CHKLIS.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = CAT.cIdJerarquiaCatalogo " &
                                                                       "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CHKLIS.bEstadoRegistroCabeceraCheckListPlantilla = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                                       "GROUP BY CHKLIS.cIdTipoMantenimiento, CHKLIS.cIdNumeroCabeceraCheckListPlantilla, CHKLIS.cIdCatalogoCabeceraCheckListPlantilla, CHKLIS.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, " &
                                                                       "         TIPMAN.vDescripcionTipoMantenimiento, CAT.vDescripcionCatalogo, CHKLIS.dFechaTransaccionCabeceraCheckListPlantilla, CHKLIS.vDescripcionCabeceraCheckListPlantilla, " &
                                                                       "         CHKLIS.vFormatoArchivoCabeceraCheckListPlantilla, CHKLIS.bEstadoRegistroCabeceraCheckListPlantilla " &
                                                                       "ORDER BY CHKLIS.cIdNumeroCabeceraCheckListPlantilla",
                                                    "", "", Now, "0", "", "", "1", "", "", "")
        Dim Coleccion As New List(Of VI_LOGI_CABECERACHECKLISTPLANTILLA)
        For Each Busqueda In Consulta
            Dim BuscarChkList As New VI_LOGI_CABECERACHECKLISTPLANTILLA
            BuscarChkList.IdTipoMantenimiento = Busqueda.cIdTipoMantenimiento
            BuscarChkList.IdNumero = Busqueda.cIdNumeroCabeceraCheckListPlantilla
            BuscarChkList.IdCatalogo = Busqueda.cIdCatalogoCabeceraCheckListPlantilla
            BuscarChkList.IdJerarquiaCatalogo = Busqueda.cIdJerarquiaCatalogoCabeceraCheckListPlantilla
            BuscarChkList.DescripcionTipoMantenimiento = Busqueda.vDescripcionTipoMantenimiento
            BuscarChkList.DescripcionCatalogo = Busqueda.vDescripcionCatalogo
            BuscarChkList.FechaTransaccion = Busqueda.dFechaTransaccionCabeceraCheckListPlantilla
            BuscarChkList.Descripcion = Busqueda.vDescripcionCabeceraCheckListPlantilla
            BuscarChkList.FormatoArchivo = Busqueda.vFormatoArchivoCabeceraCheckListPlantilla
            BuscarChkList.Estado = Busqueda.bEstadoRegistroCabeceraCheckListPlantilla
            Coleccion.Add(BuscarChkList)
        Next
        Return Coleccion
    End Function

    Public Function ChecklistPlantillaInserta(ByVal CheckListPlantilla As LOGI_CABECERACHECKLISTPLANTILLA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_INSERT", "", CheckListPlantilla.cIdTipoMantenimiento, CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla,
                                        CheckListPlantilla.dFechaTransaccionCabeceraCheckListPlantilla, CheckListPlantilla.bEstadoRegistroCabeceraCheckListPlantilla, CheckListPlantilla.cIdTipoActivoCabeceraCheckListPlantilla,
                                        CheckListPlantilla.cIdCatalogoCabeceraCheckListPlantilla, CheckListPlantilla.cIdJerarquiaCatalogoCabeceraCheckListPlantilla,
                                        CheckListPlantilla.vDescripcionCabeceraCheckListPlantilla, CheckListPlantilla.vFormatoArchivoCabeceraCheckListPlantilla, CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla).ReturnValue.ToString
        Return x
    End Function

    Public Function ChecklistPlantillaInsertaCopiaComponentes(ByVal cIdEquipo As String, ByVal cIdPlantilla As String) As Int32
        Return Data.PA_LOGI_MNT_COPIACOMPONENTESXCHECKLIST(cIdEquipo, cIdPlantilla)
    End Function

    'Public Function ChecklistPlantillaInsertaDetalle(ByVal CheckListPlantilla As List(Of LOGI_CABECERACHECKLISTPLANTILLA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
    '    Dim x

    '    'Inicializo la Transacción
    '    Dim tOption As New TransactionOptions
    '    tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
    '    tOption.Timeout = TimeSpan.MaxValue

    '    Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
    '        x = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "DELETE LOGI_CHECKLISTPLANTILLA " &
    '                                     "WHERE cIdTipoMantenimiento = '" & CheckListPlantilla(0).cIdTipoMantenimiento & "' " &
    '                                     "      AND cIdCatalogo = '" & CheckListPlantilla(0).cIdCatalogo & "'",
    '                                     "      AND cIdJerarquiaCatalogo = '" & CheckListPlantilla(0).cIdJerarquiaCatalogo & "'",
    '                                      "", "", Now, "0", "", "", "1", "").ReturnValue.ToString

    '        For Each Busqueda In CheckListPlantilla
    '            'If bExiste = False Then
    '            x = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_INSERT", "", Busqueda.cIdTipoMantenimiento, CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla,
    '                                    CheckListPlantilla.dFechaTransaccionCabeceraCheckListPlantilla, CheckListPlantilla.bEstadoRegistroCabeceraCheckListPlantilla, CheckListPlantilla.cIdTipoActivoCabeceraCheckListPlantilla,
    '                                    CheckListPlantilla.cIdCatalogoCabeceraCheckListPlantilla, CheckListPlantilla.cIdJerarquiaCatalogoCabeceraCheckListPlantilla,
    '                                    CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla).ReturnValue.ToString
    '            'Else
    '            '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "UPDATE LOGI_EQUIPO SET vDescripcionEquipo = '" & Busqueda.vDescripcionCatalogo & "', vDescripcionAbreviadaEquipo = '" & Busqueda.vDescripcionAbreviadaCatalogo & "' " &
    '            '                                  "WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "' and cIdCatalogo = '" & Busqueda.cIdCatalogo & "'", Busqueda.cIdCatalogo, Busqueda.cIdJerarquiaCatalogo,
    '            '              Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
    '            '              Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
    '            '              Busqueda.cIdCuentaContableCatalogo, Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo, Busqueda.cIdTipoActivo,
    '            '               Busqueda.nPeriodoGarantiaCatalogo, Busqueda.nPeriodoMinimoMantenimientoCatalogo,
    '            '              Busqueda.cIdCatalogo).ReturnValue.ToString

    '            '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_UPDATE", "", Busqueda.cIdCatalogo, Busqueda.cIdJerarquiaCatalogo,
    '            '                                  Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
    '            '                                  Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
    '            '                                  Busqueda.cIdCuentaContableCatalogo, Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo,
    '            '                                  Busqueda.cIdTipoActivo, Busqueda.nPeriodoGarantiaCatalogo, Busqueda.nPeriodoMinimoMantenimientoCatalogo,
    '            '                                  Busqueda.cIdCatalogo).ReturnValue.ToString
    '            'End If
    '            LogAuditoria.vEvento = "INSERTAR CHECK LIST PLANTILLA"
    '            'LogAuditoria.vQuery = "PA_LOGI_MNT_CATALOGOCARACTERISTICA 'SQL_INSERT', '', '" & Cat.cIdCatalogo & "', '" &
    '            '                              DetDocumento.cIdJerarquiaCatalogo & "', '" & DetDocumento.cIdCaracteristica & "', " &
    '            '                              DetDocumento.nIdNumeroItemCatalogoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPCatalogoCaracteristica & "', '" &
    '            '                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica & "', '" & DetDocumento.vValorCatalogoCaracteristica & "', '" & DetDocumento.cIdCaracteristica & "'"
    '            LogAuditoria.vQuery = "PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA 'SQL_INSERT', '', '" & Busqueda.cIdActividadCheckList & "', '" & Busqueda.cIdTipoMantenimiento & "', '" &
    '                                          Busqueda.cIdTipoActivo & "', '" & Busqueda.cIdCatalogo & "', '" & Busqueda.cIdJerarquiaCatalogo & "', " &
    '                                          Busqueda.nIdNumeroItemCheckListPlantilla & ", '" & Busqueda.vDatosAdicionalesCheckListPlantilla & "', '" &
    '                                          Busqueda.bEstadoRegistroCheckListPlantilla & "', '" & Busqueda.cIdActividadCheckList & "'"

    '            x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
    '                                  LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
    '                                  LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
    '        Next
    '        scope.Complete()
    '        Return x
    '    End Using
    'End Function

    Public Function ChecklistPlantillaEdita(ByVal CheckListPlantilla As LOGI_CABECERACHECKLISTPLANTILLA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_UPDATE", "", CheckListPlantilla.cIdTipoMantenimiento, CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla,
                                        CheckListPlantilla.dFechaTransaccionCabeceraCheckListPlantilla, CheckListPlantilla.bEstadoRegistroCabeceraCheckListPlantilla, CheckListPlantilla.cIdTipoActivoCabeceraCheckListPlantilla,
                                        CheckListPlantilla.cIdCatalogoCabeceraCheckListPlantilla, CheckListPlantilla.cIdJerarquiaCatalogoCabeceraCheckListPlantilla,
                                        CheckListPlantilla.vDescripcionCabeceraCheckListPlantilla, CheckListPlantilla.vFormatoArchivoCabeceraCheckListPlantilla, CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla).ReturnValue.ToString
        Return x
    End Function

    Public Function ChecklistPlantillaElimina(ByVal CheckListPlantilla As LOGI_CABECERACHECKLISTPLANTILLA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "UPDATE LOGI_CABECERACHECKLISTPLANTILLA SET bEstadoRegistroCabeceraCheckListPlantilla = 0 WHERE cIdTipoMantenimiento = '" & CheckListPlantilla.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & CheckListPlantilla.cIdNumeroCabeceraCheckListPlantilla & "'",
                                               "", "", Now, "0", "", "", "1", "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function ChecklistPlantillaExiste(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String) As Boolean
        'If Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_CHECKLISTPLANTILLA WHERE cIdChecklistPlantilla = '" & IdChecklistPlantilla & "' " &
        '                             " AND bEstadoRegistroChecklistPlantilla = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_CABECERACHECKLISTPLANTILLA("SQL_NONE", "SELECT * FROM LOGI_CABECERACHECKLISTPLANTILLA WHERE " &
                                                " cIdTipoMantenimiento = '" & IdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & IdNumero & "'",
                                                "", "", Now, "0", "", "", "1", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
