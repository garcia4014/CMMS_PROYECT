Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Transactions
Public Class clsCaracteristicaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function CaracteristicaGetData(strQuery As String) As DataTable
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

    Public Function CaracteristicaListarCombo() As List(Of LOGI_CARACTERISTICA)
        Dim Consulta = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "SELECT * FROM LOGI_CARACTERISTICA WHERE bEstadoRegistroCaracteristica = 1 ORDER BY vDescripcionCaracteristica", "", "", "", "1", "")
        Dim Coleccion As New List(Of LOGI_CARACTERISTICA)
        For Each Configuracion In Consulta
            Dim Config As New LOGI_CARACTERISTICA
            Config.cIdCaracteristica = Configuracion.cIdCaracteristica
            Config.vDescripcionCaracteristica = Configuracion.vDescripcionCaracteristica
            Coleccion.Add(Config)
        Next
        Return Coleccion
    End Function

    Public Function CaracteristicaListarPorId(ByVal IdCaracteristica As String) As LOGI_CARACTERISTICA
        Dim Consulta = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "SELECT * FROM LOGI_CARACTERISTICA WHERE cIdCaracteristica = '" & IdCaracteristica & "'", "", "", "", "1", "")
        Dim Coleccion As New LOGI_CARACTERISTICA
        For Each LOGI_CARACTERISTICA In Consulta
            Coleccion.cIdCaracteristica = LOGI_CARACTERISTICA.cIdCaracteristica
            Coleccion.vDescripcionCaracteristica = LOGI_CARACTERISTICA.vDescripcionCaracteristica
            Coleccion.vDescripcionAbreviadaCaracteristica = LOGI_CARACTERISTICA.vDescripcionAbreviadaCaracteristica
            Coleccion.bEstadoRegistroCaracteristica = LOGI_CARACTERISTICA.bEstadoRegistroCaracteristica
        Next
        Return Coleccion
    End Function

    Public Function CaracteristicaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_CARACTERISTICA)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "SELECT * FROM LOGI_CARACTERISTICA " &
                                                "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                "AND (bEstadoRegistroCaracteristica = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                "ORDER BY vDescripcionCaracteristica", "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_LOGI_CARACTERISTICA)
        For Each Busqueda In Consulta
            Dim BuscarEmp As New VI_LOGI_CARACTERISTICA
            BuscarEmp.Codigo = Busqueda.cIdCaracteristica
            BuscarEmp.Descripcion = Busqueda.vDescripcionCaracteristica
            BuscarEmp.Estado = Busqueda.bEstadoRegistroCaracteristica
            Coleccion.Add(BuscarEmp)
        Next
        Return Coleccion
    End Function

    Public Function CaracteristicaInserta(ByVal Caracteristica As LOGI_CARACTERISTICA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_INSERT", "", Caracteristica.cIdCaracteristica, Caracteristica.vDescripcionCaracteristica, Caracteristica.vDescripcionAbreviadaCaracteristica, Caracteristica.bEstadoRegistroCaracteristica, Caracteristica.cIdCaracteristica).ReturnValue.ToString
        Return x
    End Function

    Public Function CaracteristicaCatalogoInserta(ByVal Cat As LOGI_CATALOGO, ByVal CatCar As List(Of LOGI_CATALOGOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA)
        Dim x
        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            Dim strIdCatalogo As String = ""
            For Each DetDocumento In CatCar
                If strIdCatalogo <> DetDocumento.cIdCatalogo Then
                    x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_CATALOGOCARACTERISTICA " &
                                             "WHERE cIdCatalogo = '" & Cat.cIdCatalogo & "' " &
                                             "      AND cIdJerarquiaCatalogo = '" & Cat.cIdJerarquiaCatalogo & "'",
                                             "", "", "", "1", "").ReturnValue.ToString
                    strIdCatalogo = DetDocumento.cIdCatalogo
                End If

                x = Data.PA_LOGI_MNT_CATALOGOCARACTERISTICA("SQL_INSERT", "", Cat.cIdCatalogo,
                                              DetDocumento.cIdJerarquiaCatalogo, DetDocumento.cIdCaracteristica,
                                              DetDocumento.nIdNumeroItemCatalogoCaracteristica, DetDocumento.cIdReferenciaSAPCatalogoCaracteristica,
                                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica, DetDocumento.vValorCatalogoCaracteristica, DetDocumento.cIdCaracteristica).ReturnValue.ToString
                LogAuditoria.vEvento = "INSERTAR CATALOGO CARACTERISTICA"
                LogAuditoria.vQuery = "PA_LOGI_MNT_CATALOGOCARACTERISTICA 'SQL_INSERT', '', '" & Cat.cIdCatalogo & "', '" &
                                              DetDocumento.cIdJerarquiaCatalogo & "', '" & DetDocumento.cIdCaracteristica & "', " &
                                              DetDocumento.nIdNumeroItemCatalogoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPCatalogoCaracteristica & "', '" &
                                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica & "', '" & DetDocumento.vValorCatalogoCaracteristica & "', '" & DetDocumento.cIdCaracteristica & "'"

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    'Public Function CaracteristicaCatalogoInsertaDetalle(ByVal Cat As LOGI_CATALOGO, ByVal CatCar As List(Of LOGI_CATALOGOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA)
    Public Function CaracteristicaCatalogoInsertaDetalle(ByVal CatCar As List(Of LOGI_CATALOGOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x = 0
        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            'x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_CATALOGOCARACTERISTICA " &
            '                                 "WHERE cIdEmpresa = '" & Cat.cIdEmpresa & "' " &
            '                                 "      AND cIdCatalogo = '" & Cat.cIdCatalogo & "' " &
            '                                 "      AND cIdJerarquiaCatalogo = '" & Cat.cIdJerarquiaCatalogo & "'",
            '                                 "", "", "", "1", "").ReturnValue.ToString
            Dim strIdCatalogo, strIdCatalogoConcatenado, strIdJerarquia As String
            strIdCatalogo = "" : strIdCatalogoConcatenado = "" : strIdJerarquia = ""
            'Borrar antes de insertar
            If IsNothing(CatCar) = False Then
                'strIdCatalogo = "("
                Dim iContar As Int16 = 0
                For Each DetDocumento In CatCar
                    If strIdCatalogo <> DetDocumento.cIdCatalogo Then
                        strIdCatalogo = DetDocumento.cIdCatalogo
                        strIdJerarquia = DetDocumento.cIdJerarquiaCatalogo
                        strIdCatalogoConcatenado = IIf(iContar = 0, "'" + DetDocumento.cIdCatalogo + "'", strIdCatalogoConcatenado + ",'" + DetDocumento.cIdCatalogo + "'")
                        iContar = iContar + 1
                    End If
                Next
            End If

            If IsNothing(CatCar) = False Then
                If CatCar.Count > 0 Then
                    'x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_CATALOGOCARACTERISTICA " &
                    '                         "WHERE cIdCatalogo IN (" & strIdCatalogoConcatenado & ") " &
                    '                         "      AND cIdJerarquiaCatalogo = '" & strIdJerarquia & "'",
                    '                         "", "", "", "1", "").ReturnValue.ToString
                    x = Data.PA_LOGI_MNT_CATALOGOCARACTERISTICA("SQL_NONE", "DELETE LOGI_CATALOGOCARACTERISTICA " &
                                             "WHERE cIdCatalogo IN (" & strIdCatalogoConcatenado & ") " &
                                             "      AND cIdJerarquiaCatalogo = '" & strIdJerarquia & "'",
                                             "", "", "", 0, "", "", "", "").ReturnValue.ToString
                End If
                For Each DetDocumento In CatCar
                    'If strIdCatalogo <> DetDocumento.cIdCatalogo Then
                    '    x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_CATALOGOCARACTERISTICA " &
                    '                         "WHERE cIdCatalogo = '" & DetDocumento.cIdCatalogo & "' " &
                    '                         "      AND cIdJerarquiaCatalogo = '" & DetDocumento.cIdJerarquiaCatalogo & "'",
                    '                         "", "", "", "1", "").ReturnValue.ToString
                    '    strIdCatalogo = DetDocumento.cIdCatalogo
                    'End If



                    'x = Data.PA_LOGI_MNT_CATALOGOCARACTERISTICA("SQL_INSERT", "", Cat.cIdCatalogo,
                    '                              DetDocumento.cIdJerarquiaCatalogo, DetDocumento.cIdCaracteristica,
                    '                              DetDocumento.nIdNumeroItemCatalogoCaracteristica, DetDocumento.cIdReferenciaSAPCatalogoCaracteristica,
                    '                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica, DetDocumento.cIdCaracteristica).ReturnValue.ToString
                    'LogAuditoria.vEvento = "INSERTAR CATALOGO CARACTERISTICA"
                    'LogAuditoria.vQuery = "PA_LOGI_MNT_CATALOGOCARACTERISTICA 'SQL_INSERT', '', '" & Cat.cIdCatalogo & "', '" &
                    '                              DetDocumento.cIdJerarquiaCatalogo & "', '" & DetDocumento.cIdCaracteristica & "', " &
                    '                              DetDocumento.nIdNumeroItemCatalogoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPCatalogoCaracteristica & "', '" &
                    '                              DetDocumento.vDescripcionCampoSAPCat0alogoCaracteristica & "', '" & DetDocumento.cIdCaracteristica & "'"

                    x = Data.PA_LOGI_MNT_CATALOGOCARACTERISTICA("SQL_INSERT", "", DetDocumento.cIdCatalogo,
                                              DetDocumento.cIdJerarquiaCatalogo, DetDocumento.cIdCaracteristica,
                                              DetDocumento.nIdNumeroItemCatalogoCaracteristica, DetDocumento.cIdReferenciaSAPCatalogoCaracteristica,
                                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica, DetDocumento.vValorCatalogoCaracteristica, DetDocumento.cIdCaracteristica).ReturnValue.ToString
                    LogAuditoria.vEvento = "INSERTAR CATALOGO CARACTERISTICA"
                    LogAuditoria.vQuery = "PA_LOGI_MNT_CATALOGOCARACTERISTICA 'SQL_INSERT', '', '" & DetDocumento.cIdCatalogo & "', '" &
                                              DetDocumento.cIdJerarquiaCatalogo & "', '" & DetDocumento.cIdCaracteristica & "', " &
                                              DetDocumento.nIdNumeroItemCatalogoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPCatalogoCaracteristica & "', '" &
                                              DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica & "', '" & DetDocumento.vValorCatalogoCaracteristica & "', '" & DetDocumento.cIdCaracteristica & "'"

                    x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
                Next
            End If
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function CaracteristicaEquipoInsertaDetalle(ByVal IdEquipoPrincipal As String, ByVal EquipoCar As List(Of LOGI_EQUIPOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA)
        Dim x
        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_EQUIPOCARACTERISTICA " &
                                             "WHERE cIdEquipo  = '" & IdEquipoPrincipal & "'",
                                             "", "", "", "1", "").ReturnValue.ToString
            Dim strIdEquipo As String = ""
            If IsNothing(EquipoCar) = False Then
                For Each DetDocumento In EquipoCar
                    If strIdEquipo <> DetDocumento.cIdEquipo Then
                        x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_EQUIPOCARACTERISTICA " &
                                             "WHERE cIdEquipo  = '" & DetDocumento.cIdEquipo & "' " &
                                             "      AND cIdEmpresa = '" & DetDocumento.cIdEmpresa & "'",
                                             "", "", "", "1", "").ReturnValue.ToString
                        strIdEquipo = DetDocumento.cIdEquipo
                    End If
                    If DetDocumento.cIdEquipo = "" Then
                        Dim dsEquipo = CaracteristicaGetData("SELECT cIdEquipo FROM LOGI_EQUIPO WHERE cIdEmpresa = '" & DetDocumento.cIdEmpresa & "' AND cIdEnlaceEquipo = '" & IdEquipoPrincipal & "' AND cIdCatalogo = '" & DetDocumento.cIdCatalogoEquipoCaracteristica & "' AND bEstadoRegistroEquipo = '1'")
                        For Each Equipo In dsEquipo.Rows
                            strIdEquipo = Equipo("cIdEquipo")
                        Next
                        x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_EQUIPOCARACTERISTICA " &
                                             "WHERE cIdEquipo  = '" & strIdEquipo & "' " &
                                             "      AND cIdEmpresa = '" & DetDocumento.cIdEmpresa & "'",
                                             "", "", "", "1", "").ReturnValue.ToString
                    End If
                Next
                For Each DetDocumento In EquipoCar
                    If strIdEquipo <> DetDocumento.cIdEquipo Then
                        strIdEquipo = DetDocumento.cIdEquipo
                    End If
                    'If strIdEquipo <> DetDocumento.cIdEquipo Then
                    '    x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_EQUIPOCARACTERISTICA " &
                    '                         "WHERE cIdEquipo  = '" & DetDocumento.cIdEquipo & "' " &
                    '                         "      AND cIdEmpresa = '" & DetDocumento.cIdEmpresa & "'",
                    '                         "", "", "", "1", "").ReturnValue.ToString
                    '    strIdEquipo = DetDocumento.cIdEquipo
                    'End If
                    If DetDocumento.cIdEquipo = "" Then
                        Dim dsEquipo = CaracteristicaGetData("SELECT cIdEquipo FROM LOGI_EQUIPO WHERE cIdEmpresa = '" & DetDocumento.cIdEmpresa & "' AND cIdEnlaceEquipo = '" & IdEquipoPrincipal & "' AND cIdCatalogo = '" & DetDocumento.cIdCatalogoEquipoCaracteristica & "' AND bEstadoRegistroEquipo = '1'")
                        For Each Equipo In dsEquipo.Rows
                            strIdEquipo = Equipo("cIdEquipo")
                        Next
                        'x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_EQUIPOCARACTERISTICA " &
                        '                     "WHERE cIdEquipo  = '" & strIdEquipo & "' " &
                        '                     "      AND cIdEmpresa = '" & DetDocumento.cIdEmpresa & "'",
                        '                     "", "", "", "1", "").ReturnValue.ToString
                    End If
                    x = Data.PA_LOGI_MNT_EQUIPOCARACTERISTICA("SQL_INSERT", "", strIdEquipo,
                                              DetDocumento.cIdEmpresa, DetDocumento.cIdCaracteristica,
                                              DetDocumento.nIdNumeroItemEquipoCaracteristica, DetDocumento.cIdReferenciaSAPEquipoCaracteristica,
                                              DetDocumento.vDescripcionCampoSAPEquipoCaracteristica, DetDocumento.vValorReferencialEquipoCaracteristica,
                                              DetDocumento.cIdCatalogoEquipoCaracteristica, strIdEquipo).ReturnValue.ToString
                    LogAuditoria.vEvento = "INSERTAR EQUIPO CARACTERISTICA"
                    LogAuditoria.vQuery = "PA_LOGI_MNT_EQUIPOCARACTERISTICA 'SQL_INSERT', '', '" & strIdEquipo & "', '" &
                                              DetDocumento.cIdEmpresa & "', '" & DetDocumento.cIdCaracteristica & "', " &
                                              DetDocumento.nIdNumeroItemEquipoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPEquipoCaracteristica & "', '" &
                                              DetDocumento.vDescripcionCampoSAPEquipoCaracteristica & "', '" & DetDocumento.vValorReferencialEquipoCaracteristica & "', '" &
                                              DetDocumento.cIdCatalogoEquipoCaracteristica & "', '" & DetDocumento.cIdEquipo & "'"

                    x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
                Next
            End If
            scope.Complete()
            Return x
        End Using
    End Function

    'Public Function CaracteristicaCatalogoComponenteInsertaDetalle(ByVal Cat As LOGI_CATALOGO, ByVal CatCar As List(Of LOGI_CATALOGOCARACTERISTICA), ByVal LogAuditoria As GNRL_LOGAUDITORIA)
    '    Dim x
    '    'Inicializo la Transacción
    '    Dim tOption As New TransactionOptions
    '    tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
    '    tOption.Timeout = TimeSpan.MaxValue

    '    Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
    '        'x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_CATALOGOCARACTERISTICA " &
    '        '                                 "WHERE cIdEmpresa = '" & Cat.cIdEmpresa & "' " &
    '        '                                 "      AND cIdCatalogo = '" & Cat.cIdCatalogo & "' " &
    '        '                                 "      AND cIdJerarquiaCatalogo = '" & Cat.cIdJerarquiaCatalogo & "'",
    '        '                                 "", "", "", "1", "").ReturnValue.ToString
    '        x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "DELETE LOGI_CATALOGOCARACTERISTICA " &
    '                                         "WHERE cIdCatalogo = '" & Cat.cIdCatalogo & "' " &
    '                                         "      AND cIdJerarquiaCatalogo = '" & Cat.cIdJerarquiaCatalogo & "'",
    '                                         "", "", "", "1", "").ReturnValue.ToString

    '        For Each DetDocumento In CatCar
    '            x = Data.PA_LOGI_MNT_CATALOGOCARACTERISTICA("SQL_INSERT", "", DetDocumento.cIdCatalogo,
    '                                          DetDocumento.cIdJerarquiaCatalogo, DetDocumento.cIdCaracteristica,
    '                                          DetDocumento.nIdNumeroItemCatalogoCaracteristica, DetDocumento.cIdReferenciaSAPCatalogoCaracteristica,
    '                                          DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica, DetDocumento.cIdCaracteristica).ReturnValue.ToString
    '            LogAuditoria.vEvento = "INSERTAR CATALOGO CARACTERISTICA"
    '            LogAuditoria.vQuery = "PA_LOGI_MNT_CATALOGOCARACTERISTICA 'SQL_INSERT', '', '" & DetDocumento.cIdCatalogo & "', '" &
    '                                          DetDocumento.cIdJerarquiaCatalogo & "', '" & DetDocumento.cIdCaracteristica & "', " &
    '                                          DetDocumento.nIdNumeroItemCatalogoCaracteristica & ", '" & DetDocumento.cIdReferenciaSAPCatalogoCaracteristica & "', '" &
    '                                          DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica & "', '" & DetDocumento.cIdCaracteristica & "'"




    '            'DetDocumento.cIdTipoDocumento & " ', '" &
    '            'DetDocumento.vIdNumeroSerieCabeceraOfertaVenta & "', '" & DetDocumento.vIdNumeroCorrelativoCabeceraOfertaVenta & "', '" & DetDocumento.cIdPuntoVenta & "', '" &
    '            '                              DetDocumento.cIdEmpresa & "', '" & DetDocumento.cIdProducto & "', " & DetDocumento.nIdNumeroItemDetalleOfertaVenta & ", '" & DetDocumento.cIdTipoMoneda & "', '" &
    '            '                              DetDocumento.vDescripcionDetalleOfertaVenta & "', " & DetDocumento.nCantidadProductoDetalleOfertaVenta & ", '" & DetDocumento.cIdUnidadMedidaProductoDetalleOfertaVenta & "', " &
    '            '                              DetDocumento.nCantidad1DetalleOfertaVenta & ", '" & DetDocumento.cIdUnidadMedida1DetalleOfertaVenta & "', " & DetDocumento.nCantidad2DetalleOfertaVenta & ", '" &
    '            '                              DetDocumento.cIdUnidadMedida2DetalleOfertaVenta & "', " & DetDocumento.nTamanoPesoProductoDetalleOfertaVenta & ", " & DetDocumento.nPrecioOrigenVentaDetalleOfertaVenta & ", " &
    '            '                              DetDocumento.nPrecioUnitarioVentaDetalleOfertaVenta & ", " & DetDocumento.nPrecioUnitarioTotalVentaDetalleOfertaVenta & ", " & DetDocumento.nValorUnitarioTotalVentaDetalleOfertaVenta & ", " & DetDocumento.nDescuentoVentaDetalleOfertaVenta & ", " & DetDocumento.nValorVentaDetalleOfertaVenta & ", " &
    '            '                              DetDocumento.nIGVDetalleOfertaVenta & ", " & DetDocumento.nTotalPrecioVentaDetalleOfertaVenta & ", '" & DetDocumento.cIdTipoAfectacion & "', '" & DetDocumento.cIdOnerosidadDetalleOfertaVenta & "', " &
    '            '                              DetDocumento.nISCDetalleOfertaVenta & ", '" & DetDocumento.cIdSistemaISCDetalleOfertaVenta & "', " & DetDocumento.nPrecioUnitarioSugeridoDetalleOfertaVenta & ", '" &
    '            '                              DetDocumento.nPorcentajeISCDetalleOfertaVenta & ", '" & DetDocumento.cIdProductoVariante & "', '" & DetDocumento.cIdUnidadMedidaOrigenDetalleOfertaVenta & "', '" &
    '            '                              DetDocumento.vObservacionDetalleOfertaVenta & "', '" & DetDocumento.cIdTipoExistenciaDetalleOfertaVenta & "', '" & DetDocumento.vIdNumeroCorrelativoCabeceraOfertaVenta & "'"

    '            x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
    '                                  LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
    '                                  LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
    '        Next
    '        scope.Complete()
    '        Return x
    '    End Using
    'End Function
    Public Function CaracteristicaEdita(ByVal Caracteristica As LOGI_CARACTERISTICA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_UPDATE", "", Caracteristica.cIdCaracteristica, Caracteristica.vDescripcionCaracteristica, Caracteristica.vDescripcionAbreviadaCaracteristica, Caracteristica.bEstadoRegistroCaracteristica, Caracteristica.cIdCaracteristica).ReturnValue.ToString
        Return x
    End Function

    Public Function CaracteristicaElimina(ByVal Caracteristica As LOGI_CARACTERISTICA) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "UPDATE LOGI_CARACTERISTICA SET bEstadoRegistroCaracteristica = 0 WHERE cIdCaracteristica = '" & Caracteristica.cIdCaracteristica & "'",
                                     "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function CaracteristicaExiste(ByVal IdCaracteristica As String) As Boolean
        If Data.PA_LOGI_MNT_CARACTERISTICA("SQL_NONE", "SELECT * FROM LOGI_CARACTERISTICA WHERE cIdCaracteristica = '" & IdCaracteristica & "' and bEstadoRegistroCaracteristica = 1", "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class