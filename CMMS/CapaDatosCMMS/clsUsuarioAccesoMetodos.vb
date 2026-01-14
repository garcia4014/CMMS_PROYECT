Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection
Public Class clsUsuarioAccesoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function UsuarioAccesoGetData(strQuery As String) As DataTable
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

    Public Function UsuarioAccesoListarPorId(ByVal IdEmpresa As String, ByVal IdPais As String, ByVal IdLocal As String, ByVal IdLogin As String) As GNRL_USUARIOACCESO
        Dim Consulta = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_NONE", "SELECT * FROM GNRL_USUARIOACCESO " &
                                                      "WHERE cIdEmpresa = '" & IdEmpresa & "' AND cIdPaisOrigenUsuarioAcceso = '" & IdPais & "' " &
                                                      "AND cIdLocalPredeterminado = '" & IdLocal & "' AND cIdUsuario = '" & IdLogin & "'",
                                                      "", "", "", "", "1", "", "")
        Dim Coleccion As New GNRL_USUARIOACCESO
        For Each GNRL_USUARIOACCESO In Consulta
            Coleccion.cIdEmpresa = GNRL_USUARIOACCESO.cIdEmpresa
            Coleccion.cIdPaisOrigenUsuarioAcceso = GNRL_USUARIOACCESO.cIdPaisOrigenUsuarioAcceso
            Coleccion.cIdLocalPredeterminado = GNRL_USUARIOACCESO.cIdLocalPredeterminado
            Coleccion.cIdUsuario = GNRL_USUARIOACCESO.cIdUsuario
            Coleccion.bEstadoRegistroUsuarioAcceso = GNRL_USUARIOACCESO.bEstadoRegistroUsuarioAcceso
            Coleccion.cIdTipoUsuario = GNRL_USUARIOACCESO.cIdTipoUsuario
            Coleccion.cIdPerfil = GNRL_USUARIOACCESO.cIdPerfil
        Next
        Return Coleccion
    End Function

    'Public Function UsuarioAccesoListaGrid(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_USUARIOACCESO)
    '    'Este si puede devolver una colección de datos es decir varios registros
    '    Dim Consulta = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_NONE", "SELECT * FROM GNRL_USUARIOACCESO " &
    '                                                  "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
    '                                                  "      AND (bEstadoRegistroUsuarioAcceso = '" & Estado & "' OR '*' = '" & Estado & "') " &
    '                                            IIf(TipoUsuarioAcceso = "*", "", "AND cIdPerfilUsuarioAcceso = '" & TipoUsuarioAcceso & "'"),
    '                                            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "", "", "", "1", "", "0", "", "", "")
    '    Dim Coleccion As New List(Of VI_GNRL_USUARIOACCESO)
    '    For Each Busqueda In Consulta
    '        Dim BuscarUsu As New VI_GNRL_USUARIOACCESO
    '        BuscarUsu.Codigo = Busqueda.cIdLoginUsuarioAcceso
    '        'BuscarUsu.Perfil = Busqueda.cIdPerfilUsuarioAcceso
    '        BuscarUsu.Ape_Paterno = Busqueda.vApellidoPaternoUsuarioAcceso
    '        BuscarUsu.Ape_Materno = Busqueda.vApellidoMaternoUsuarioAcceso
    '        BuscarUsu.Nombres = Busqueda.vNombresUsuarioAcceso
    '        BuscarUsu.Estado = Busqueda.bEstadoRegistroUsuarioAcceso
    '        Coleccion.Add(BuscarUsu)
    '    Next
    '    Return Coleccion
    'End Function

    Public Function UsuarioAccesoInsertaDetalle(ByVal UsuarioAcceso As List(Of GNRL_USUARIOACCESO), ByVal IdSistema As String, ByVal IdOpcionModulo As String) As Int32
        Dim x

        ''Inicializo la Transacción
        'Dim tOption As New TransactionOptions
        'tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        'tOption.Timeout = TimeSpan.MaxValue

        '''Conexión para poder realizar el update con un command.
        '''Dim constr As String = My.Settings.INTRANET_CONEYConnectionString
        '''Dim con As New SqlConnection(constr)

        ''''Dim FeriadoNeg As New clsFeriadoNegocios
        '''Dim dtFeriado = CatalogoGetData("SELECT CONVERT (NUMERIC, ISNULL(MAX(cIdJerarquiaCatalogo), 1)) AS " & _
        '''                                "FROM LOGI_CATALOGO ")

        '''                                "FROM perFERIADO " & _
        '''                                "WHERE cIdMesFeriado = '" & String.Format("{0:00}", CDate(txtFechaInicialDetalleTareo.Text).Month) & "' " & _
        '''                                "AND cIdDiaFeriado = '" & String.Format("{0:00}", CDate(txtFechaInicialDetalleTareo.Text).Day) & "' " & _
        '''                                "AND bEstadoRegistroFeriado = 1")
        '''If dtFeriado.Rows.Count > 0 Then
        '''    lblDiaFeriado.Text = "Día Feriado: " & dtFeriado.Rows(0).Item(0).ToString.Trim
        '''End If

        '''dtCatalogoComponente = CatalogoGetData("SELECT COUNT(*) AS Cantidad FROM LOGI_MAESTROACTIVO WHERE cIdEnlaceCatalogo = '" & Busqueda.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Busqueda.cIdCatalogo & "'")
        '''dtCatalogoComponente = CatalogoGetData("SELECT cIdCatalogo FROM LOGI_MAESTROACTIVO WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "'")
        ''dtCatalogoComponente = CatalogoGetData("SELECT cIdCatalogo FROM LOGI_CATALOGO WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "'")
        'Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
        For Each Busqueda In UsuarioAcceso
            Dim bExiste As Boolean
            bExiste = False

            Dim dtUsuarioAcceso = UsuarioAccesoGetData("SELECT COUNT(*) AS Cantidad FROM GNRL_USUARIOACCESO " &
                                                       "WHERE cIdEmpresa = '" & Busqueda.cIdEmpresa & "' AND cIdPaisOrigenUsuarioAcceso = '" & Busqueda.cIdPaisOrigenUsuarioAcceso & "' " &
                                                       "      AND cIdLocalPredeterminado = '" & Busqueda.cIdLocalPredeterminado & "' AND cIdUsuario = '" & Busqueda.cIdUsuario & "'")
            For Each row In dtUsuarioAcceso.Rows
                If row("Cantidad") >= 1 Then
                    bExiste = True
                End If
            Next

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            Dim FuncionesNeg As New clsFuncionesMetodos
            If bExiste = False Then
                x = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_INSERT", "", Busqueda.cIdEmpresa, Busqueda.cIdPaisOrigenUsuarioAcceso,
                                                   Busqueda.cIdLocalPredeterminado, Busqueda.cIdUsuario, Busqueda.bEstadoRegistroUsuarioAcceso,
                                                   Busqueda.cIdTipoUsuario, Busqueda.cIdPerfil).ReturnValue.ToString
                'Se dehabilitó porque da el siguiente error:   • Error de comunicación con el administrador de transacciones subyacente.
                Dim strQuery = "PA_GNRL_MNT_USUARIOACCESO 'SQL_INSERT', '', '" & Busqueda.cIdEmpresa & "', '" & Busqueda.cIdPaisOrigenUsuarioAcceso & "', '" &
                                  Busqueda.cIdLocalPredeterminado & "', '" & Busqueda.cIdUsuario & "', '" & Busqueda.bEstadoRegistroUsuarioAcceso & "', '" &
                                  Busqueda.cIdTipoUsuario & "', '" & Busqueda.cIdPerfil & "'"
                LogAuditoria.vEvento = "INSERTAR USUARIO ACCESO"
                LogAuditoria.vQuery = strQuery
                LogAuditoria.cIdSistema = IdSistema
                LogAuditoria.cIdModulo = IdOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)
            Else
                x = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_UPDATE", "", Busqueda.cIdEmpresa, Busqueda.cIdPaisOrigenUsuarioAcceso,
                                                   Busqueda.cIdLocalPredeterminado, Busqueda.cIdUsuario, Busqueda.bEstadoRegistroUsuarioAcceso,
                                                   Busqueda.cIdTipoUsuario, Busqueda.cIdPerfil).ReturnValue.ToString
                'Se dehabilitó porque da el siguiente error:   • Error de comunicación con el administrador de transacciones subyacente.
                Dim strQuery = "PA_GNRL_MNT_USUARIOACCESO 'SQL_UPDATE', '', '" & Busqueda.cIdEmpresa & "', '" & Busqueda.cIdPaisOrigenUsuarioAcceso & "', '" &
                                   Busqueda.cIdLocalPredeterminado & "', '" & Busqueda.cIdUsuario & "', '" & Busqueda.bEstadoRegistroUsuarioAcceso & "', '" &
                                   Busqueda.cIdTipoUsuario & "', '" & Busqueda.cIdPerfil & "'"
                LogAuditoria.vEvento = "ACTUALIZAR USUARIO ACCESO"
                LogAuditoria.vQuery = strQuery
                LogAuditoria.cIdSistema = IdSistema
                LogAuditoria.cIdModulo = IdOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)
            End If

            ''x = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT COUNT(*) AS Cantidad FROM LOGI_MAESTROACTIVO WHERE cIdEnlaceCatalogo = '" & Busqueda.cIdEnlaceCatalogo & "' AND cIdCatalogo = '" & Busqueda.cIdCatalogo & "'", Busqueda.cIdCatalogo, Busqueda.cIdTipoActivo, Busqueda.cIdJerarquiaCatalogo, _
            ''Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo, _
            ''Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo, _
            ''Busqueda.vDimensionesCatalogo, Busqueda.vVoltajeCatalogo, Busqueda.vPesoCatalogo, _
            ''Busqueda.cIdCatalogo)

            ''If Busqueda.cIdCatalogo = "" Then
            ''If dtCatalogoComponente.Rows(0).Item(0) <= 0 Then
            'If bExiste = False Then
            '    'If x.Rows(0).Item(0) <= 0 Then
            '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_INSERT", "", Busqueda.cIdCatalogo, Busqueda.cIdTipoActivo, Busqueda.cIdJerarquiaCatalogo,
            '                                  Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
            '                                  Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
            '                                  Busqueda.vDimensionesCatalogo, Busqueda.vVoltajeCatalogo, Busqueda.vPesoCatalogo,
            '                                  Busqueda.bActivacionAutomaticaCatalogo, Busqueda.cIdCuentaContableCatalogo,
            '                                  Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo, Busqueda.cIdCatalogo).ReturnValue.ToString
            'Else
            '    'CatalogoGetData("UPDATE LOGI_MAESTROACTIVO SET vDescripcionMaestroActivo = '" & Busqueda.vDescripcionCatalogo & "', vDescripcionAbreviadaMaestroActivo = '" & Busqueda.vDescripcionAbreviadaCatalogo & "' " & _
            '    '                "WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "' and cIdCatalogo = '" & Busqueda.cIdCatalogo & "'")

            '    'Aquí se utiliza la Conexión para poder realizar el update con un command.
            '    'Using con As New SqlConnection(constr)
            '    'Using cmd As New SqlCommand("UPDATE LOGI_MAESTROACTIVO SET vDescripcionMaestroActivo = '" & Busqueda.vDescripcionCatalogo & "', vDescripcionAbreviadaMaestroActivo = '" & Busqueda.vDescripcionAbreviadaCatalogo & "' " & _
            '    '                            "WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "' and cIdCatalogo = '" & Busqueda.cIdCatalogo & "'")
            '    '    Using sda As New SqlDataAdapter()
            '    '        con.Open()
            '    '        cmd.CommandType = CommandType.Text
            '    '        cmd.Connection = con
            '    '        'sda.SelectCommand = cmd
            '    '        sda.UpdateCommand = cmd
            '    '        sda.UpdateCommand.ExecuteNonQuery()
            '    '        'sda.Fill(dt)
            '    '    End Using
            '    'End Using
            '    'End Using

            '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "UPDATE LOGI_MAESTROACTIVO SET vDescripcionMaestroActivo = '" & Busqueda.vDescripcionCatalogo & "', vDescripcionAbreviadaMaestroActivo = '" & Busqueda.vDescripcionAbreviadaCatalogo & "' " &
            '                                  "WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "' and cIdCatalogo = '" & Busqueda.cIdCatalogo & "'", Busqueda.cIdCatalogo, Busqueda.cIdTipoActivo, Busqueda.cIdJerarquiaCatalogo,
            '              Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
            '              Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
            '              Busqueda.vDimensionesCatalogo, Busqueda.vVoltajeCatalogo, Busqueda.vPesoCatalogo,
            '              Busqueda.bActivacionAutomaticaCatalogo, Busqueda.cIdCuentaContableCatalogo,
            '              Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo, Busqueda.cIdCatalogo).ReturnValue.ToString

            '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_UPDATE", "", Busqueda.cIdCatalogo, Busqueda.cIdTipoActivo, Busqueda.cIdJerarquiaCatalogo,
            '                                  Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
            '                                  Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
            '                                  Busqueda.vDimensionesCatalogo, Busqueda.vVoltajeCatalogo, Busqueda.vPesoCatalogo,
            '                                  Busqueda.bActivacionAutomaticaCatalogo, Busqueda.cIdCuentaContableCatalogo,
            '                                  Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo, Busqueda.cIdCatalogo).ReturnValue.ToString
            'End If

            'Else
            '    x = Data.PA_LOGI_MNT_CATALOGO("SQL_UPDATE", "", Busqueda.cIdCatalogo, Busqueda.cIdTipoActivo, Busqueda.cIdJerarquiaCatalogo, _
            '                                  Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo, _
            '                                  Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo, _
            '                                  Busqueda.cIdCatalogo).ReturnValue.ToString
            'End If

            'x = Data.PA_PER_MNT_DETALLETAREO("SQL_INSERT", "", intNroOrdenTareo, Busqueda.cIdPeriodoTareo, Busqueda.cIdConcepto, _
            '                                            Busqueda.cIdTrabajador, Busqueda.nIdProyecto, Busqueda.cIdDetalleDias, _
            '                                            Busqueda.dFechaHoraDetalleAsistencia, Busqueda.vApellidoPaternoDetalleTareo, _
            '                                            Busqueda.vApellidoMaternoDetalleTareo, Busqueda.cIdCargo, Busqueda.vNombresDetalleTareo, _
            '                                            Busqueda.nTotalDiasLibresTareo, Busqueda.nTotalDiasTrabajados, Busqueda.nTotalDiasPendientes, _
            '                                            Busqueda.cIdSistemaTrabajo, Busqueda.vImporteDetalleTareo, Busqueda.vObservacionDetalleTareo, Busqueda.cIdMaquina, _
            '                                            intNroOrdenTareo).ReturnValue.ToString
        Next
        'scope.Complete()
        Return x
        'End Using
    End Function

    'Public Function UsuarioAccesoInserta(ByVal UsuarioAcceso As GNRL_USUARIOACCESO) As Int32
    '    Dim x
    '    x = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_INSERT", "", UsuarioAcceso.cIdLoginUsuarioAcceso, UsuarioAcceso.cIdPerfilUsuarioAcceso, UsuarioAcceso.vApellidoPaternoUsuarioAcceso,
    '                                 UsuarioAcceso.vApellidoMaternoUsuarioAcceso, UsuarioAcceso.vNombresUsuarioAcceso, UsuarioAcceso.vPasswordUsuarioAcceso,
    '                                 UsuarioAcceso.vCargoUsuarioAcceso, UsuarioAcceso.vDireccionUsuarioAcceso, UsuarioAcceso.vTelefonoCasaUsuarioAcceso,
    '                                 UsuarioAcceso.vTelefonoOficinaUsuarioAcceso, UsuarioAcceso.vCelularUsuarioAcceso, UsuarioAcceso.vEmailUsuarioAcceso, UsuarioAcceso.vEmailOpcionUsuarioAcceso,
    '                                 UsuarioAcceso.vDniUsuarioAcceso, UsuarioAcceso.cGeneroUsuarioAcceso, UsuarioAcceso.dFechaNacimientoUsuarioAcceso, UsuarioAcceso.cIdDistritoUbicacionGeografica,
    '                                 UsuarioAcceso.cIdProvinciaUbicacionGeografica, UsuarioAcceso.cIdDepartamentoUbicacionGeografica, UsuarioAcceso.cIdPaisUbicacionGeografica,
    '                                 UsuarioAcceso.bEstadoRegistroUsuarioAcceso, UsuarioAcceso.cIdTipoUsuarioAcceso, UsuarioAcceso.vIdProyectoPredeterminado, UsuarioAcceso.cIdEmpresa,
    '                                 UsuarioAcceso.cIdPaisOrigenUsuarioAcceso, UsuarioAcceso.cIdLoginUsuarioAcceso).ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function UsuarioAccesoEdita(ByVal UsuarioAcceso As GNRL_USUARIOACCESO) As Int32
    '    Dim x
    '    x = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_UPDATE", "", UsuarioAcceso.cIdLoginUsuarioAcceso, UsuarioAcceso.cIdPerfilUsuarioAcceso, UsuarioAcceso.vApellidoPaternoUsuarioAcceso,
    '                                 UsuarioAcceso.vApellidoMaternoUsuarioAcceso, UsuarioAcceso.vNombresUsuarioAcceso, UsuarioAcceso.vPasswordUsuarioAcceso,
    '                                 UsuarioAcceso.vCargoUsuarioAcceso, UsuarioAcceso.vDireccionUsuarioAcceso, UsuarioAcceso.vTelefonoCasaUsuarioAcceso,
    '                                 UsuarioAcceso.vTelefonoOficinaUsuarioAcceso, UsuarioAcceso.vCelularUsuarioAcceso, UsuarioAcceso.vEmailUsuarioAcceso, UsuarioAcceso.vEmailOpcionUsuarioAcceso,
    '                                 UsuarioAcceso.vDniUsuarioAcceso, UsuarioAcceso.cGeneroUsuarioAcceso, UsuarioAcceso.dFechaNacimientoUsuarioAcceso, UsuarioAcceso.cIdDistritoUbicacionGeografica,
    '                                 UsuarioAcceso.cIdProvinciaUbicacionGeografica, UsuarioAcceso.cIdDepartamentoUbicacionGeografica, UsuarioAcceso.cIdPaisUbicacionGeografica,
    '                                 UsuarioAcceso.bEstadoRegistroUsuarioAcceso, UsuarioAcceso.cIdTipoUsuarioAcceso, UsuarioAcceso.vIdProyectoPredeterminado, UsuarioAcceso.cIdEmpresa,
    '                                 UsuarioAcceso.cIdPaisOrigenUsuarioAcceso, UsuarioAcceso.cIdLoginUsuarioAcceso).ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function UsuarioAccesoElimina(ByVal UsuarioAcceso As GNRL_USUARIOACCESO) As Int32
    '    Dim x
    '    'x = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_NONE", "DELETE GNRL_USUARIOACCESO WHERE cIdUsuarioAcceso = '" & UsuarioAcceso.cIdUsuarioAcceso & "'",
    '    '                             "", "", "", "", "", "", "", "", "", "", "1", "")
    '    x = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_NONE", "UPDATE GNRL_USUARIOACCESO SET bEstadoRegistroUsuarioAcceso = 0 " &
    '                                 "WHERE cIdLoginUsuarioAcceso = '" & UsuarioAcceso.cIdLoginUsuarioAcceso & "'",
    '                                  "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "", "", "", "1", "", "0", "", "", "")


    '    'x = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_UPDATE", UsuarioAcceso.cIdUsuarioAcceso, UsuarioAcceso.vLoginUsuarioAcceso, UsuarioAcceso.vApellidoPaternoUsuarioAcceso,
    '    'UsuarioAcceso.vApellidoMaternoUsuarioAcceso, UsuarioAcceso.vNombresUsuarioAcceso, UsuarioAcceso.vPasswordUsuarioAcceso,
    '    'UsuarioAcceso.vCargoUsuarioAcceso, UsuarioAcceso.cPermisosUsuarioAcceso, UsuarioAcceso.cTipoUsuarioAcceso,
    '    'UsuarioAcceso.vRutaFotoUsuarioAcceso, UsuarioAcceso.cIdConfiguracion, UsuarioAcceso.bEstadoUsuarioAcceso, UsuarioAcceso.cIdUsuarioAcceso).ReturnValue.ToString
    '    Return x
    'End Function

    Public Function UsuarioAccesoExiste(ByVal IdUsuarioAcceso As String, ByVal IdEmpresa As String, ByVal IdPaisOrigen As String, ByVal IdLocal As String) As Boolean
        'If Data.PA_GNRL_MNT_USUARIOACCESO("SQL_NONE", "SELECT * FROM GNRL_USUARIOACCESO " &
        '                                  "WHERE cIdLoginUsuarioAcceso = '" & IdUsuarioAcceso & "' " &
        '                                  "      AND bEstadoRegistroUsuarioAcceso = 1",
        '                                  "", "", "", "", "1", "", "").Count > 0 Then
        If Data.PA_GNRL_MNT_USUARIOACCESO("SQL_NONE", "SELECT * FROM GNRL_USUARIOACCESO " &
                                          "WHERE cIdUsuario = '" & IdUsuarioAcceso & "' " &
                                          " AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPaisOrigenUsuarioAcceso = '" & IdPaisOrigen & "' AND cIdLocalPredeterminado = '" & IdLocal & "' ",
                                          "", "", "", "", "1", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    'Public Function UsuarioAccesoValidar(ByVal IdUsuarioAcceso As String, ByVal strContraseña As String) As GNRL_USUARIOACCESO
    '    Dim Consulta = Data.PA_GNRL_MNT_USUARIOACCESO("SQL_NONE", "SELECT * FROM GNRL_USUARIOACCESO WHERE cIdLoginUsuarioAcceso = '" & IdUsuarioAcceso & "' " &
    '                                            "AND vPasswordUsuarioAcceso = '" & strContraseña & "'", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "", "", "", "1", "", "0", "", "", "")
    '    Dim Coleccion As New GNRL_USUARIOACCESO
    '    For Each GNRL_USUARIOACCESO In Consulta
    '        Coleccion.cIdLoginUsuarioAcceso = GNRL_USUARIOACCESO.cIdLoginUsuarioAcceso
    '        Coleccion.vApellidoPaternoUsuarioAcceso = GNRL_USUARIOACCESO.vApellidoPaternoUsuarioAcceso
    '        Coleccion.vApellidoMaternoUsuarioAcceso = GNRL_USUARIOACCESO.vApellidoMaternoUsuarioAcceso
    '        Coleccion.vNombresUsuarioAcceso = GNRL_USUARIOACCESO.vNombresUsuarioAcceso
    '        Coleccion.vCargoUsuarioAcceso = GNRL_USUARIOACCESO.vCargoUsuarioAcceso
    '        Coleccion.bEstadoRegistroUsuarioAcceso = GNRL_USUARIOACCESO.bEstadoRegistroUsuarioAcceso
    '        Coleccion.cGeneroUsuarioAcceso = GNRL_USUARIOACCESO.cGeneroUsuarioAcceso
    '        Coleccion.cIdPaisUbicacionGeografica = GNRL_USUARIOACCESO.cIdPaisUbicacionGeografica
    '        Coleccion.cIdDepartamentoUbicacionGeografica = GNRL_USUARIOACCESO.cIdDepartamentoUbicacionGeografica
    '        Coleccion.cIdProvinciaUbicacionGeografica = GNRL_USUARIOACCESO.cIdProvinciaUbicacionGeografica
    '        Coleccion.cIdDistritoUbicacionGeografica = GNRL_USUARIOACCESO.cIdDistritoUbicacionGeografica
    '        Coleccion.vDireccionUsuarioAcceso = GNRL_USUARIOACCESO.vDireccionUsuarioAcceso
    '        Coleccion.vPasswordUsuarioAcceso = GNRL_USUARIOACCESO.vPasswordUsuarioAcceso
    '        Coleccion.dFechaNacimientoUsuarioAcceso = GNRL_USUARIOACCESO.dFechaNacimientoUsuarioAcceso
    '        Coleccion.cIdPerfilUsuarioAcceso = GNRL_USUARIOACCESO.cIdPerfilUsuarioAcceso
    '        Coleccion.vCelularUsuarioAcceso = GNRL_USUARIOACCESO.vCelularUsuarioAcceso
    '        Coleccion.vDniUsuarioAcceso = GNRL_USUARIOACCESO.vDniUsuarioAcceso
    '        Coleccion.vEmailOpcionUsuarioAcceso = GNRL_USUARIOACCESO.vEmailOpcionUsuarioAcceso
    '        Coleccion.vEmailUsuarioAcceso = GNRL_USUARIOACCESO.vEmailUsuarioAcceso
    '        Coleccion.vTelefonoCasaUsuarioAcceso = GNRL_USUARIOACCESO.vTelefonoCasaUsuarioAcceso
    '        Coleccion.vTelefonoOficinaUsuarioAcceso = GNRL_USUARIOACCESO.vTelefonoOficinaUsuarioAcceso
    '        Coleccion.cIdTipoUsuarioAcceso = GNRL_USUARIOACCESO.cIdTipoUsuarioAcceso
    '        Coleccion.vIdProyectoPredeterminado = GNRL_USUARIOACCESO.vIdProyectoPredeterminado
    '        Coleccion.cIdPaisOrigenUsuarioAcceso = GNRL_USUARIOACCESO.cIdPaisOrigenUsuarioAcceso
    '        Coleccion.cIdEmpresa = GNRL_USUARIOACCESO.cIdEmpresa
    '    Next
    '    Return Coleccion
    'End Function
End Class