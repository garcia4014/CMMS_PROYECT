Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsDetalleContratoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function DetalleContratoGetData(strQuery As String) As DataTable
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

    Public Function DetalleContratoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdEquipo As String) As LOGI_DETALLECONTRATO
        Dim Consulta = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_NONE", "SELECT * FROM LOGI_DETALLECONTRATO " &
                                                                   "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' " &
                                                                   "      AND vIdNumeroSerieCabeceraContrato = '" & IdNroSer & "' " &
                                                                   "      AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' " &
                                                                   "      AND cIdEquipoDetalleContrato = '" & IdEquipo & "' " &
                                                                   "      AND cIdEmpresa = '" & IdEmpresa & "'",
                                                                   "", "", "", "", 0, "", "", "1", "")
        Dim Coleccion As New LOGI_DETALLECONTRATO
        For Each LOGI_DETALLEContrato In Consulta
            Coleccion.cIdTipoDocumentoCabeceraContrato = LOGI_DETALLEContrato.cIdTipoDocumentoCabeceraContrato
            Coleccion.vIdNumeroSerieCabeceraContrato = LOGI_DETALLEContrato.vIdNumeroSerieCabeceraContrato
            Coleccion.vIdNumeroCorrelativoCabeceraContrato = LOGI_DETALLEContrato.vIdNumeroCorrelativoCabeceraContrato
            Coleccion.cIdEmpresa = LOGI_DETALLEContrato.cIdEmpresa
            Coleccion.cIdEquipoDetalleContrato = LOGI_DETALLEContrato.cIdEquipoDetalleContrato
            Coleccion.nIdNumeroItemDetalleContrato = LOGI_DETALLEContrato.nIdNumeroItemDetalleContrato
            Coleccion.vDescripcionDetalleContrato = LOGI_DETALLEContrato.vDescripcionDetalleContrato
            Coleccion.bEstadoRegistroDetalleContrato = LOGI_DETALLEContrato.bEstadoRegistroDetalleContrato
        Next
        Return Coleccion
    End Function

    Public Function lstDetalleContratoListarPorId(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdEquipo As String) As List(OF LOGI_DETALLECONTRATO)
        Dim Consulta = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_NONE", "SELECT * FROM LOGI_DETALLECONTRATO " &
                                                                   "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' " &
                                                                   "      AND vIdNumeroSerieCabeceraContrato = '" & IdNroSer & "' " &
                                                                   "      AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' " &
                                                                   "      AND cIdEquipoDetalleContrato = '" & IdEquipo & "' " &
                                                                   "      AND cIdEmpresa = '" & IdEmpresa & "'",
                                                                   "", "", "", "", 0, "", "", "1", "")
        Dim detalleContrato As New LOGI_DETALLECONTRATO
        Dim Coleccion As New List(Of LOGI_DETALLECONTRATO)
        For Each LOGI_DETALLEContrato In Consulta
            detalleContrato.cIdTipoDocumentoCabeceraContrato = LOGI_DETALLEContrato.cIdTipoDocumentoCabeceraContrato
            detalleContrato.vIdNumeroSerieCabeceraContrato = LOGI_DETALLEContrato.vIdNumeroSerieCabeceraContrato
            detalleContrato.vIdNumeroCorrelativoCabeceraContrato = LOGI_DETALLEContrato.vIdNumeroCorrelativoCabeceraContrato
            detalleContrato.cIdEmpresa = LOGI_DETALLEContrato.cIdEmpresa
            detalleContrato.cIdEquipoDetalleContrato = LOGI_DETALLEContrato.cIdEquipoDetalleContrato
            detalleContrato.nIdNumeroItemDetalleContrato = LOGI_DETALLEContrato.nIdNumeroItemDetalleContrato
            detalleContrato.vDescripcionDetalleContrato = LOGI_DETALLEContrato.vDescripcionDetalleContrato
            detalleContrato.bEstadoRegistroDetalleContrato = LOGI_DETALLEContrato.bEstadoRegistroDetalleContrato
            Coleccion.Add(detalleContrato)
        Next
        Return Coleccion
    End Function

    'Public Function DetalleContratoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_LOGI_DETALLEContrato)
    Public Function DetalleContratoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_DETALLEContrato)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_NONE", "SELECT DETORDTRA.vIdArticuloSAPDetalleContrato, DETORDTRA.vDescripcionArticuloDetalleContrato, DETORDTRA.nCantidadArticuloDetalleContrato, DETORDTRA.vDescripcionUnidadMedidaDetalleContrato " &
                                                                       "FROM LOGI_DETALLEContrato AS DETORDTRA " &
                                                                       "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CHKLISPLA.bEstadoRegistroDetalleContrato = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                                                       "", "", "", "", 0, "", "", "1", "")
        Dim Coleccion As New List(Of VI_LOGI_DETALLECONTRATO)
        For Each Busqueda In Consulta
            Dim BuscarDetCont As New VI_LOGI_DETALLECONTRATO
            BuscarDetCont.IdEquipo = Busqueda.cIdEquipoDetalleContrato
            BuscarDetCont.Descripcion = Busqueda.vDescripcionDetalleContrato
            BuscarDetCont.Estado = Busqueda.bEstadoRegistroDetalleContrato
            BuscarDetCont.IdNumeroItem = Busqueda.nIdNumeroItemDetalleContrato
            Coleccion.Add(BuscarDetCont)
        Next
        Return Coleccion
    End Function

    Public Function DetalleContratoInserta(ByVal DetalleContrato As LOGI_DETALLECONTRATO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_INSERT", "", DetalleContrato.cIdTipoDocumentoCabeceraContrato, DetalleContrato.vIdNumeroSerieCabeceraContrato,
                                        DetalleContrato.vIdNumeroCorrelativoCabeceraContrato, DetalleContrato.cIdEmpresa, DetalleContrato.nIdNumeroItemDetalleContrato,
                                        DetalleContrato.cIdEquipoDetalleContrato, DetalleContrato.vDescripcionDetalleContrato, DetalleContrato.bEstadoRegistroDetalleContrato,
                                        DetalleContrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        Return x
    End Function

    Public Function DetalleContratoInsertaDetalle(ByVal DetalleContrato As List(Of LOGI_DETALLECONTRATO), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_NONE", "DELETE LOGI_DETALLECONTRATO " &
                                         "WHERE cIdTipoDocumentoCabeceraContrato = '" & DetalleContrato(0).cIdTipoDocumentoCabeceraContrato & "' " &
                                         "      AND vIdNumeroSerieCabeceraContrato = '" & DetalleContrato(0).vIdNumeroSerieCabeceraContrato & "' " &
                                         "      AND vIdNumeroCorrelativoCabeceraContrato = '" & DetalleContrato(0).vIdNumeroCorrelativoCabeceraContrato & "' " &
                                         "      AND cIdEmpresa = '" & DetalleContrato(0).cIdEmpresa & "' ",
                                         "", "", "", "", 0, "", "", "1", "").ReturnValue.ToString

            For Each Busqueda In DetalleContrato
                'If bExiste = False Then
                x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_INSERT", "", Busqueda.cIdTipoDocumentoCabeceraContrato, Busqueda.vIdNumeroSerieCabeceraContrato,
                                        Busqueda.vIdNumeroCorrelativoCabeceraContrato, Busqueda.cIdEmpresa, Busqueda.nIdNumeroItemDetalleContrato,
                                        Busqueda.cIdEquipoDetalleContrato, Busqueda.vDescripcionDetalleContrato, Busqueda.bEstadoRegistroDetalleContrato,
                                        Busqueda.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString

                LogAuditoria.vEvento = "INSERTAR DETALLE ORDEN TRABAJO"
                LogAuditoria.vQuery = "PA_LOGI_MNT_DETALLEContrato 'SQL_INSERT', '', '" & Busqueda.cIdTipoDocumentoCabeceraContrato & "', '" & Busqueda.vIdNumeroSerieCabeceraContrato & "', '" &
                                        Busqueda.vIdNumeroCorrelativoCabeceraContrato & "', '" & Busqueda.cIdEmpresa & "', " & Busqueda.nIdNumeroItemDetalleContrato & ", '" &
                                        Busqueda.cIdEquipoDetalleContrato & "', '" & Busqueda.vDescripcionDetalleContrato & "', '" & Busqueda.bEstadoRegistroDetalleContrato & "', '" &
                                        Busqueda.vIdNumeroCorrelativoCabeceraContrato & "'"

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function DetalleContratoEdita(ByVal DetalleContrato As LOGI_DETALLECONTRATO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_UPDATE", "", DetalleContrato.cIdTipoDocumentoCabeceraContrato, DetalleContrato.vIdNumeroSerieCabeceraContrato,
                                        DetalleContrato.vIdNumeroCorrelativoCabeceraContrato, DetalleContrato.cIdEmpresa, DetalleContrato.nIdNumeroItemDetalleContrato,
                                        DetalleContrato.cIdEquipoDetalleContrato, DetalleContrato.vDescripcionDetalleContrato, DetalleContrato.bEstadoRegistroDetalleContrato,
                                        DetalleContrato.vIdNumeroCorrelativoCabeceraContrato).ReturnValue.ToString
        Return x
    End Function

    'Public Function DetalleContratoElimina(ByVal DetalleContrato As LOGI_DETALLEContrato) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_DETALLEContrato("SQL_NONE", "UPDATE LOGI_DETALLEContrato SET bEstadoRegistroDetalleContrato = 0 WHERE cIdActividadCheckList = '" & DetalleContrato.cIdActividadCheckList & "' " &
    '                                           " AND cIdTipoMantenimiento = '" & DetalleContrato.cIdTipoMantenimiento & "' AND cIdNumeroCabeceraCheckListPlantilla = '" & DetalleContrato.cIdNumeroCabeceraCheckListPlantilla & "' AND cIdCatalogo = '" & DetalleContrato.cIdCatalogo & "' AND cIdJerarquiaCatalogo = '" & DetalleContrato.cIdJerarquiaCatalogo & "'",
    '                                           "", "", "", "", 0, "", "", "", "", 0, "", "", "", "", "").ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function DetalleContratoExiste(ByVal IdTipoMantenimiento As String, ByVal IdNumero As String, ByVal IdActividadCheckList As String, ByVal IdCatalogo As String, ByVal IdJerarquiaCatalogo As String) As Boolean
    Public Function DetalleContratoExiste(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String, ByVal IdEmpresa As String, ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticulo As String) As Boolean
        'If Data.PA_LOGI_MNT_DetalleContrato("SQL_NONE", "SELECT * FROM LOGI_DETALLEContrato WHERE cIdDetalleContrato = '" & IdDetalleContrato & "' " &
        '                             " AND bEstadoRegistroDetalleContrato = 1", "", "", "", "1", "").Count > 0 Then
        If Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_NONE", "SELECT * FROM LOGI_DETALLEContrato " &
                                                "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' " &
                                                "      AND vIdNumeroSerieCabeceraContrato = '" & IdNroSer & "' " &
                                                "      AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' " &
                                                "      AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                "      AND cIdEquipoCabeceraContrato = '" & IdEquipo & "'",
                                                "", "", "", "", 0, "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
