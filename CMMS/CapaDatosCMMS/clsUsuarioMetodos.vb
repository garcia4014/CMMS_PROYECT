Imports System.Data
Imports System.Data.SqlClient

Public Class clsUsuarioMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function UsuarioGetData(strQuery As String) As DataTable
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

    Public Function contratoListarCombo() As DataTable
        Dim dt As New DataTable()
        Dim constr As String = My.Settings.CMMSConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("SELECT '*' as idContrato, '*' as descContrato UNION ALL SELECT cIdTipoDocumentoCabeceraContrato+'-'+vIdNumeroSerieCabeceraContrato+'-'+vIdNumeroCorrelativoCabeceraContrato as idContrato, cIdTipoDocumentoCabeceraContrato+'-'+vIdNumeroSerieCabeceraContrato+'-'+vIdNumeroCorrelativoCabeceraContrato as descContrato FROM LOGI_CABECERACONTRATO ORDER BY idContrato", con)
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

    Public Function UsuarioListarPorId(ByVal IdUsuario As String) As GNRL_USUARIO
        Dim Consulta = Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "SELECT * FROM GNRL_USUARIO " &
                                                "WHERE cIdUsuario = '" & IdUsuario & "'",
                                                "", "", "", "", "", "", "", "", "", "", "1", "", "", "", "", "", "", "", "")
        Dim Coleccion As New GNRL_USUARIO
        For Each GNRL_USUARIO In Consulta
            Coleccion.cIdUsuario = GNRL_USUARIO.cIdUsuario
            Coleccion.vApellidoPaternoUsuario = GNRL_USUARIO.vApellidoPaternoUsuario
            Coleccion.vApellidoMaternoUsuario = GNRL_USUARIO.vApellidoMaternoUsuario
            Coleccion.vNombresUsuario = GNRL_USUARIO.vNombresUsuario
            Coleccion.vCargoUsuario = GNRL_USUARIO.vCargoUsuario
            Coleccion.bEstadoRegistroUsuario = GNRL_USUARIO.bEstadoRegistroUsuario
            Coleccion.cPermisosUsuario = GNRL_USUARIO.cPermisosUsuario
            Coleccion.vLoginUsuario = GNRL_USUARIO.vLoginUsuario
            Coleccion.vPasswordUsuario = GNRL_USUARIO.vPasswordUsuario
            Coleccion.cIdEmpresa = GNRL_USUARIO.cIdEmpresa
            Coleccion.cIdPuntoVenta = GNRL_USUARIO.cIdPuntoVenta
            Coleccion.vRutaFotoUsuario = GNRL_USUARIO.vRutaFotoUsuario
            Coleccion.cIdPaisUbicacionGeografica = GNRL_USUARIO.cIdPaisUbicacionGeografica
            Coleccion.cIdDepartamentoUbicacionGeografica = GNRL_USUARIO.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = GNRL_USUARIO.cIdProvinciaUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = GNRL_USUARIO.cIdDistritoUbicacionGeografica
            Coleccion.vIdNroDocumentoIdentidadUsuario = GNRL_USUARIO.vIdNroDocumentoIdentidadUsuario
            Coleccion.vIdUnidadTrabajoUsuario = GNRL_USUARIO.vIdUnidadTrabajoUsuario
            Coleccion.IdContratoUsuario = GNRL_USUARIO.IdContratoUsuario
        Next
        Return Coleccion
    End Function

    'Public Function UsuarioListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_GNRL_USUARIO)
    Public Function UsuarioListaGrid(ByVal Filtro As String, ByVal Buscar As String, Optional TipoUsuario As String = "*", Optional ByVal Estado As String = "*") As List(Of VI_GNRL_USUARIO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "SELECT * FROM GNRL_USUARIO WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND " &
        '                                        "bEstadoRegistroUsuario = 1 AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "'",
        Dim Consulta = Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "SELECT * FROM GNRL_USUARIO WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                "      AND (bEstadoRegistroUsuario = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                IIf(TipoUsuario = "*", "", "AND cTipoUsuario = '" & TipoUsuario & "'"),
                                                "", "", "", "", "", "", "", "", "", "", "1", "", "", "", "", "", "", "", "")
        Dim Coleccion As New List(Of VI_GNRL_USUARIO)
        For Each Busqueda In Consulta
            Dim BuscarUsu As New VI_GNRL_USUARIO
            BuscarUsu.Codigo = Busqueda.cIdUsuario
            BuscarUsu.Login = Busqueda.vLoginUsuario
            BuscarUsu.Apellido_Paterno = Busqueda.vApellidoPaternoUsuario
            BuscarUsu.Apellido_Materno = Busqueda.vApellidoMaternoUsuario
            BuscarUsu.Nombres = Busqueda.vNombresUsuario
            BuscarUsu.Estado = Busqueda.bEstadoRegistroUsuario
            Coleccion.Add(BuscarUsu)
        Next
        Return Coleccion
    End Function

    Public Function UsuarioInserta(ByVal Usuario As GNRL_USUARIO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_USUARIO("SQL_INSERT", "", Usuario.vLoginUsuario, Usuario.vApellidoPaternoUsuario,
                                     Usuario.vApellidoMaternoUsuario, Usuario.vNombresUsuario, Usuario.vPasswordUsuario,
                                     Usuario.vCargoUsuario, Usuario.cPermisosUsuario,
                                     Usuario.vRutaFotoUsuario, Usuario.cIdPuntoVenta, Usuario.cIdEmpresa,
                                     Usuario.bEstadoRegistroUsuario,
                                     Usuario.cIdPaisUbicacionGeografica, Usuario.cIdDepartamentoUbicacionGeografica, Usuario.cIdProvinciaUbicacionGeografica, Usuario.cIdDistritoUbicacionGeografica,
                                     Usuario.vIdNroDocumentoIdentidadUsuario, Usuario.vIdUnidadTrabajoUsuario, Usuario.IdContratoUsuario, Usuario.cIdUsuario).ReturnValue.ToString
        Return x
    End Function

    Public Function UsuarioEdita(ByVal Usuario As GNRL_USUARIO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_USUARIO("SQL_UPDATE", "", Usuario.vLoginUsuario, Usuario.vApellidoPaternoUsuario,
                                     Usuario.vApellidoMaternoUsuario, Usuario.vNombresUsuario, Usuario.vPasswordUsuario,
                                     Usuario.vCargoUsuario, Usuario.cPermisosUsuario,
                                     Usuario.vRutaFotoUsuario, Usuario.cIdPuntoVenta, Usuario.cIdEmpresa,
                                     Usuario.bEstadoRegistroUsuario,
                                     Usuario.cIdPaisUbicacionGeografica, Usuario.cIdDepartamentoUbicacionGeografica, Usuario.cIdProvinciaUbicacionGeografica, Usuario.cIdDistritoUbicacionGeografica,
                                     Usuario.vIdNroDocumentoIdentidadUsuario, Usuario.vIdUnidadTrabajoUsuario, Usuario.IdContratoUsuario, Usuario.cIdUsuario).ReturnValue.ToString
        Return x
    End Function

    Public Function UsuarioElimina(ByVal Usuario As GNRL_USUARIO) As Int32
        Dim x
        'x = Data.pa_gnrl_mnt_usuario("SQL_NONE", "DELETE GNRL_USUARIO WHERE cIdUsuario = '" & Usuario.cIdUsuario & "'",
        '                             "", "", "", "", "", "", "", "", "", "", "1", "")
        x = Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "UPDATE GNRL_USUARIO SET bEstadoRegistroUsuario = 0 " &
                                     "WHERE cIdUsuario = '" & Usuario.cIdUsuario & "'",
                                     "", "", "", "", "", "", "", "", "", "", "1", "", "", "", "", "", "", "", "")


        'x = Data.pa_gnrl_mnt_usuario("SQL_UPDATE", Usuario.cIdUsuario, Usuario.vLoginUsuario, Usuario.vApellidoPaternoUsuario,
        'Usuario.vApellidoMaternoUsuario, Usuario.vNombresUsuario, Usuario.vPasswordUsuario,
        'Usuario.vCargoUsuario, Usuario.cPermisosUsuario, Usuario.cTipoUsuario,
        'Usuario.vRutaFotoUsuario, Usuario.cIdConfiguracion, Usuario.bEstadoUsuario, Usuario.cIdUsuario).ReturnValue.ToString
        Return x
    End Function

    Public Function UsuarioExiste(ByVal IdUsuario As String) As Boolean
        'If Data.usp_ProductosSelect(idUsuario).Count > 0 Then
        'If Data.pa_gnrl_mnt_usuario("SQL_NONE", "SELECT * FROM GNRL_USUARIO WHERE cIdUsuario = '" & idUsuario & "'", "''", "''", "''", "''", "''", "''", "''", "''", "''", "''", "1", "''").Count > 0 Then
        If Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "SELECT * FROM GNRL_USUARIO WHERE cIdUsuario = '" & IdUsuario & "' AND bEstadoRegistroUsuario = 1",
                                    "", "", "", "", "", "", "", "", "", "", "1", "", "", "", "", "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    'Public Function UsuarioValidar(ByVal IdLogin As String, ByVal strContraseña As String, ByVal strPuntoVenta As String, ByVal strEmpresa As String, ByVal strArea As String, ByVal strSistema As String, ByVal strModulo As String) As GNRL_USUARIO
    'Public Function UsuarioValidar(ByVal IdLogin As String, ByVal strContraseña As String, ByVal strEmpresa As String, ByVal strArea As String, ByVal strSistema As String, ByVal strModulo As String, ByVal strLocal As String) As GNRL_USUARIO
    Public Function UsuarioValidar(ByVal IdLogin As String, ByVal strContraseña As String, ByVal strEmpresa As String, ByVal strSistema As String, ByVal strModulo As String, ByVal strLocal As String) As GNRL_USUARIO
        'Dim Consulta = Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "SELECT USU.cIdUsuario, USU.vLoginUsuario, USU.vApellidoPaternoUsuario, USU.vNombresUsuario, USU.vPasswordUsuario, USU.vCargoUsuario, USU.cPermisosUsuario, USU.vRutaFotoUsuario, USU.vApellidoMaternoUsuario, USU.bEstadoRegistroUsuario, USU.cIdPuntoVenta, USU.cIdPaisUbicacionGeografica, USU.cIdDepartamentoUbicacionGeografica, USU.cIdProvinciaUbicacionGeografica, USU.cIdDistritoUbicacionGeografica, USUACC.cIdEmpresa " &
        '                                        "FROM GNRL_USUARIO USU INNER JOIN GNRL_CONFIGURACION CFG ON " &
        '                                        "USU.cIdUsuario = CFG.cIdUsuario " &
        '                                        "INNER JOIN GNRL_USUARIOACCESO USUACC ON " &
        '                                        "USUACC.cIdUsuario = USU.cIdUsuario AND USUACC.cIdEmpresa = '" & strEmpresa & "' " &
        '                                        "AND USUACC.cIdLocalPredeterminado = '" & strLocal & "' AND USUACC.bEstadoRegistroUsuarioAcceso = '1' " &
        '                                        "WHERE USU.vLoginUsuario = '" & IdLogin & "' AND USU.vPasswordUsuario = '" & strContraseña & "' AND " &
        '                                        "      CFG.cIdArea = '" & strArea & "' AND CFG.cIdSistema = '" & strSistema & "' AND CFG.cIdModulo = '" & strModulo & "'",
        '                                        "", "", "", "", "", "", "", "", "", "", "1", "", "", "", "", "")
        Dim Consulta = Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "SELECT USU.cIdUsuario, USU.vLoginUsuario, USU.vApellidoPaternoUsuario, USU.vNombresUsuario, USU.vPasswordUsuario, USU.vCargoUsuario, USU.cPermisosUsuario, USU.vRutaFotoUsuario, USU.vApellidoMaternoUsuario, USU.bEstadoRegistroUsuario, USU.cIdPuntoVenta, USU.cIdPaisUbicacionGeografica, USU.cIdDepartamentoUbicacionGeografica, USU.cIdProvinciaUbicacionGeografica, USU.cIdDistritoUbicacionGeografica, USU.vIdUnidadTrabajoUsuario, USUACC.cIdEmpresa, USU.IdContratoUsuario, USU.vIdNroDocumentoIdentidadUsuario " &
                                                "FROM GNRL_USUARIO USU INNER JOIN GNRL_CONFIGURACION CFG ON " &
                                                "USU.cIdUsuario = CFG.cIdUsuario " &
                                                "INNER JOIN GNRL_USUARIOACCESO USUACC ON " &
                                                "USUACC.cIdUsuario = USU.cIdUsuario AND USUACC.cIdEmpresa = '" & strEmpresa & "' " &
                                                "AND USUACC.cIdLocalPredeterminado = '" & strLocal & "' AND USUACC.bEstadoRegistroUsuarioAcceso = '1' " &
                                                "WHERE USU.vLoginUsuario = '" & IdLogin & "' AND USU.vPasswordUsuario = '" & strContraseña & "' AND " &
                                                "      CFG.cIdSistema = '" & strSistema & "' AND CFG.cIdModulo = '" & strModulo & "'",
                                                "", "", "", "", "", "", "", "", "", "", "1", "", "", "", "", "", "", "", "")

        Dim Coleccion As New GNRL_USUARIO
        For Each GNRL_USUARIO In Consulta
            Coleccion.cIdUsuario = GNRL_USUARIO.cIdUsuario
            Coleccion.vApellidoPaternoUsuario = GNRL_USUARIO.vApellidoPaternoUsuario
            Coleccion.vApellidoMaternoUsuario = GNRL_USUARIO.vApellidoMaternoUsuario
            Coleccion.vNombresUsuario = GNRL_USUARIO.vNombresUsuario
            Coleccion.vCargoUsuario = GNRL_USUARIO.vCargoUsuario
            Coleccion.bEstadoRegistroUsuario = GNRL_USUARIO.bEstadoRegistroUsuario
            Coleccion.cPermisosUsuario = GNRL_USUARIO.cPermisosUsuario
            Coleccion.vLoginUsuario = GNRL_USUARIO.vLoginUsuario
            Coleccion.vPasswordUsuario = GNRL_USUARIO.vPasswordUsuario
            Coleccion.cIdEmpresa = GNRL_USUARIO.cIdEmpresa
            Coleccion.cIdPuntoVenta = GNRL_USUARIO.cIdPuntoVenta
            Coleccion.vRutaFotoUsuario = GNRL_USUARIO.vRutaFotoUsuario
            Coleccion.cIdPaisUbicacionGeografica = GNRL_USUARIO.cIdPaisUbicacionGeografica
            Coleccion.cIdDepartamentoUbicacionGeografica = GNRL_USUARIO.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = GNRL_USUARIO.cIdProvinciaUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = GNRL_USUARIO.cIdDistritoUbicacionGeografica
            Coleccion.vIdNroDocumentoIdentidadUsuario = GNRL_USUARIO.vIdNroDocumentoIdentidadUsuario
            Coleccion.vIdUnidadTrabajoUsuario = GNRL_USUARIO.vIdUnidadTrabajoUsuario
            Coleccion.IdContratoUsuario = GNRL_USUARIO.IdContratoUsuario
        Next
        Return Coleccion
    End Function

    Public Function UsuarioListarPorLogin(ByVal IdLogin As String, ByVal strPuntoVenta As String, ByVal strEmpresa As String) As GNRL_USUARIO
        Dim Consulta = Data.PA_GNRL_MNT_USUARIO("SQL_NONE", "SELECT * FROM GNRL_USUARIO WHERE vLoginUsuario = '" & IdLogin & "' AND cIdPuntoVenta = '" & strPuntoVenta & "' AND cIdEmpresa = '" & strEmpresa & "'",
                                                "", "", "", "", "", "", "", "", "", "", "1", "", "", "", "", "", "", "", "")
        Dim Coleccion As New GNRL_USUARIO
        For Each GNRL_USUARIO In Consulta
            Coleccion.cIdUsuario = GNRL_USUARIO.cIdUsuario
            Coleccion.vApellidoPaternoUsuario = GNRL_USUARIO.vApellidoPaternoUsuario
            Coleccion.vApellidoMaternoUsuario = GNRL_USUARIO.vApellidoMaternoUsuario
            Coleccion.vNombresUsuario = GNRL_USUARIO.vNombresUsuario
            Coleccion.vCargoUsuario = GNRL_USUARIO.vCargoUsuario
            Coleccion.bEstadoRegistroUsuario = GNRL_USUARIO.bEstadoRegistroUsuario
            Coleccion.cPermisosUsuario = GNRL_USUARIO.cPermisosUsuario
            Coleccion.vLoginUsuario = GNRL_USUARIO.vLoginUsuario
            Coleccion.vPasswordUsuario = GNRL_USUARIO.vPasswordUsuario
            Coleccion.cIdEmpresa = GNRL_USUARIO.cIdEmpresa
            Coleccion.cIdPuntoVenta = GNRL_USUARIO.cIdPuntoVenta
            Coleccion.vRutaFotoUsuario = GNRL_USUARIO.vRutaFotoUsuario
            'Coleccion.vIPUsuario = GNRL_USUARIO.vIPUsuario
            Coleccion.cIdPaisUbicacionGeografica = GNRL_USUARIO.cIdPaisUbicacionGeografica
            Coleccion.cIdDepartamentoUbicacionGeografica = GNRL_USUARIO.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = GNRL_USUARIO.cIdProvinciaUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = GNRL_USUARIO.cIdDistritoUbicacionGeografica
            Coleccion.vIdNroDocumentoIdentidadUsuario = GNRL_USUARIO.vIdNroDocumentoIdentidadUsuario
            Coleccion.vIdUnidadTrabajoUsuario = GNRL_USUARIO.vIdUnidadTrabajoUsuario
            Coleccion.IdContratoUsuario = GNRL_USUARIO.IdContratoUsuario
        Next
        Return Coleccion
    End Function
End Class