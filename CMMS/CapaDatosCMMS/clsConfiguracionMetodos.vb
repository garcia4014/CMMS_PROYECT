Imports System.Data
Imports System.Data.SqlClient
Public Class clsConfiguracionMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function ConfiguracionGetData(strQuery As String) As DataTable
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

    Public Function ConfiguracionListarCombo() As List(Of GNRL_CONFIGURACION)
        Dim Consulta = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "SELECT * FROM GNRL_CONFIGURACION WHERE bEstadoRegistroConfiguracion = 1", "", "", "", "", "", "", "", "", "0")
        Dim Coleccion As New List(Of GNRL_CONFIGURACION)
        For Each Configuracion In Consulta
            Dim Conf As New GNRL_CONFIGURACION
            Conf.cIdPerfil = Configuracion.cIdPerfil
            'Conf.vDescripcionConfiguracion = Configuracion.vDescripcionConfiguracion
            Coleccion.Add(Conf)
        Next
        Return Coleccion
    End Function

    Public Function ConfiguracionListarPorId(ByVal Configuracion As GNRL_CONFIGURACION) As GNRL_CONFIGURACION
        Dim Consulta = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "SELECT * FROM GNRL_CONFIGURACION WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND " &
                                                      "cIdSistema = '" & Configuracion.cIdSistema & "' AND cIdModulo = '" & Configuracion.cIdModulo & "' AND " &
                                                      "cIdArea = '" & Configuracion.cIdArea & "' AND cIdElemento = '" & Configuracion.cIdElemento & "' AND " &
                                                      "cIdUsuario = '" & Configuracion.cIdUsuario & "'",
                                                      "", "", "", "", "", "", "", "", "1")
        Dim Coleccion As New GNRL_CONFIGURACION
        For Each GNRL_CONFIGURACION In Consulta
            Coleccion.cIdPerfil = GNRL_CONFIGURACION.cIdPerfil
            Coleccion.cIdSistema = GNRL_CONFIGURACION.cIdSistema
            Coleccion.cIdModulo = GNRL_CONFIGURACION.cIdModulo
            Coleccion.cIdArea = GNRL_CONFIGURACION.cIdArea
            Coleccion.cIdElemento = GNRL_CONFIGURACION.cIdElemento
            Coleccion.cIdUsuario = GNRL_CONFIGURACION.cIdUsuario
            Coleccion.cIdPaisOrigenUsuario = GNRL_CONFIGURACION.cIdPaisOrigenUsuario
            Coleccion.cIdEmpresa = GNRL_CONFIGURACION.cIdEmpresa
            Coleccion.bEstadoRegistroConfiguracion = GNRL_CONFIGURACION.bEstadoRegistroConfiguracion
        Next
        Return Coleccion
    End Function

    'Public Function ConfiguracionListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_GNRL_CONFIGURACION)
    '    'Este si puede devolver una colección de datos es decir varios registros
    '    Dim Consulta = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "SELECT CONF.cIdUsuario, CONF.cIdPerfil, CONF.cIdElemento, CONF.cIdModulo, CONF.cIdSistema, " &
    '                                                              "CONF.bEstadoRegistroConfiguracion, CONF.cIdArea,  AREA.vDescripcionArea, " &
    '                                                              "LTRIM(RTRIM(USU.vApellidoPaternoUsuario)) + ' ' + LTRIM(RTRIM(USU.vApellidoMaternoUsuario)) + " &
    '                                                              "', ' + LTRIM(RTRIM(USU.vNombresUsuario)) AS vUsuario, MOD.vDescripcionModulo, ELE.vDescripcionElemento " &
    '                                                              "FROM GNRL_CONFIGURACION AS CONF INNER JOIN GNRL_USUARIO AS USU ON " &
    '                                                              "CONF.cIdUsuario = USU.cIdUsuario INNER JOIN GNRL_ELEMENTO AS ELE ON " &
    '                                                              "CONF.cIdElemento = ELE.cIdElemento AND CONF.cIdSistema = ELE.cIdSistema AND CONF.cIdModulo = ELE.cIdModulo AND CONF.cIdPerfil = USU.cIdPerfil INNER JOIN GNRL_AREA AREA ON " &
    '                                                              "CONF.cIdArea = AREA.cIdArea INNER JOIN GNRL_MODULO AS MOD ON " &
    '                                                              "CONF.cIdModulo = MOD.cIdModulo AND CONF.cIdSistema = MOD.cIdSistema " &
    '                                                              "WHERE(CONF.bEstadoRegistroConfiguracion = 1) " &
    '                                                              "      AND " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroConfiguracion = 1 " &
    '                                                              "      AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "'" &
    '                                                              "ORDER BY LTRIM(RTRIM(USU.vApellidoPaternoUsuario)), CONF.cIdArea", "", "", "", "", "", "", "1")

    '    Dim Coleccion As New List(Of VI_GNRL_CONFIGURACION)
    '    For Each Busqueda In Consulta
    '        Dim BuscarConf As New VI_GNRL_CONFIGURACION
    '        BuscarConf.cIdArea = Busqueda.cIdArea
    '        BuscarConf.cIdElemento = Busqueda.cIdElemento
    '        BuscarConf.cIdModulo = Busqueda.cIdModulo
    '        BuscarConf.cIdPerfil = Busqueda.cIdPerfil
    '        BuscarConf.cIdSistema = Busqueda.cIdSistema
    '        BuscarConf.cIdUsuario = Busqueda.cIdUsuario
    '        BuscarConf.Elemento = Busqueda.vDescripcionElemento
    '        BuscarConf.Modulo = Busqueda.vDescripcionModulo
    '        BuscarConf.Usuario = Busqueda.vUsuario
    '        BuscarConf.Estado = Busqueda.bEstadoRegistroConfiguracion
    '        BuscarConf.vDescripcionArea = Busqueda.vDescripcionArea
    '        Coleccion.Add(BuscarConf)
    '    Next
    '    Return Coleccion
    'End Function
    Public Function ConfiguracionListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdPaisOrigen As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_GNRL_CONFIGURACION)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "SELECT CONF.cIdUsuario, CONF.cIdElemento, CONF.cIdModulo, CONF.cIdSistema, CONF.cIdPerfil, " &
        '                                                          "CONF.bEstadoRegistroConfiguracion, CONF.cIdArea,  AREA.vDescripcionArea, " &
        '                                                          "LTRIM(RTRIM(USU.vApellidoPaternoUsuario)) + ' ' + LTRIM(RTRIM(USU.vApellidoMaternoUsuario)) + " &
        '                                                          "', ' + LTRIM(RTRIM(USU.vNombresUsuario)) AS vUsuario, MOD.vDescripcionModulo, ELE.vDescripcionElemento " &
        '                                                          "FROM GNRL_CONFIGURACION AS CONF INNER JOIN GNRL_USUARIO AS USU ON " &
        '                                                          "CONF.cIdUsuario = USU.cIdUsuario INNER JOIN GNRL_ELEMENTO AS ELE ON " &
        '                                                          "CONF.cIdElemento = ELE.cIdElemento AND CONF.cIdSistema = ELE.cIdSistema AND CONF.cIdModulo = ELE.cIdModulo INNER JOIN GNRL_AREA AREA ON " &
        '                                                          "CONF.cIdArea = AREA.cIdArea INNER JOIN GNRL_MODULO AS MOD ON " &
        '                                                          "CONF.cIdModulo = MOD.cIdModulo AND CONF.cIdSistema = MOD.cIdSistema " &
        '                                                          "INNER JOIN GNRL_USUARIOACCESO AS USUACC ON CONF.cIdUsuario = USUACC.cIdUsuario " &
        '                                                          "WHERE " &
        '                                                          "      " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
        '                                                          "      AND (CONF.bEstadoRegistroConfiguracion = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                                          "      AND USUACC.cIdPaisOrigenUsuarioAcceso = '" & IdPaisOrigen & "' AND USUACC.cIdEmpresa = '" & IdEmpresa & "' " &
        '                                                          "ORDER BY LTRIM(RTRIM(USU.vApellidoPaternoUsuario)), CONF.cIdArea",
        '                                                          "", "", "", "", "", "", "", "", "1")
        Dim Consulta = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "SELECT CONF.cIdUsuario, CONF.cIdElemento, CONF.cIdModulo, CONF.cIdSistema, CONF.cIdPerfil, " &
                                                                  "CONF.bEstadoRegistroConfiguracion, CONF.cIdArea,  AREA.vDescripcionArea, " &
                                                                  "LTRIM(RTRIM(USU.vApellidoPaternoUsuario)) + ' ' + LTRIM(RTRIM(USU.vApellidoMaternoUsuario)) + " &
                                                                  "', ' + LTRIM(RTRIM(USU.vNombresUsuario)) AS vUsuario, MOD.vDescripcionModulo, ELE.vDescripcionElemento " &
                                                                  "FROM GNRL_CONFIGURACION AS CONF INNER JOIN GNRL_USUARIO AS USU ON " &
                                                                  "CONF.cIdUsuario = USU.cIdUsuario INNER JOIN GNRL_ELEMENTO AS ELE ON " &
                                                                  "CONF.cIdElemento = ELE.cIdElemento AND CONF.cIdSistema = ELE.cIdSistema AND CONF.cIdModulo = ELE.cIdModulo INNER JOIN GNRL_AREA AREA ON " &
                                                                  "CONF.cIdArea = AREA.cIdArea INNER JOIN GNRL_MODULO AS MOD ON " &
                                                                  "CONF.cIdModulo = MOD.cIdModulo AND CONF.cIdSistema = MOD.cIdSistema " &
                                                                  "INNER JOIN GNRL_USUARIOACCESO AS USUACC ON CONF.cIdUsuario = USUACC.cIdUsuario " &
                                                                  "WHERE " &
                                                                  "      " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                                  "      AND (CONF.bEstadoRegistroConfiguracion = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                                  "      AND USUACC.cIdPaisOrigenUsuarioAcceso = '" & IdPaisOrigen & "' AND USUACC.cIdEmpresa = '" & IdEmpresa & "' " &
                                                                  "GROUP BY CONF.cIdUsuario, CONF.cIdElemento, CONF.cIdModulo, CONF.cIdSistema, CONF.cIdPerfil, CONF.bEstadoRegistroConfiguracion, CONF.cIdArea, AREA.vDescripcionArea, LTRIM(RTRIM(USU.vApellidoPaternoUsuario)) + ' ' + LTRIM(RTRIM(USU.vApellidoMaternoUsuario)) + ', ' + LTRIM(RTRIM(USU.vNombresUsuario)), MOD.vDescripcionModulo, ELE.vDescripcionElemento " &
                                                                  "ORDER BY vUsuario, CONF.cIdArea",
                                                                  "", "", "", "", "", "", "", "", "1")

        Dim Coleccion As New List(Of VI_GNRL_CONFIGURACION)
        For Each Busqueda In Consulta
            Dim BuscarConf As New VI_GNRL_CONFIGURACION
            BuscarConf.cIdArea = Busqueda.cIdArea
            BuscarConf.cIdElemento = Busqueda.cIdElemento
            BuscarConf.cIdModulo = Busqueda.cIdModulo
            BuscarConf.cIdPerfil = Busqueda.cIdPerfil
            BuscarConf.cIdSistema = Busqueda.cIdSistema
            BuscarConf.cIdUsuario = Busqueda.cIdUsuario
            BuscarConf.Elemento = Busqueda.vDescripcionElemento
            BuscarConf.Modulo = Busqueda.vDescripcionModulo
            BuscarConf.Usuario = Busqueda.vUsuario
            BuscarConf.Estado = Busqueda.bEstadoRegistroConfiguracion
            BuscarConf.vDescripcionArea = Busqueda.vDescripcionArea
            Coleccion.Add(BuscarConf)
        Next
        Return Coleccion
    End Function

    Public Function ConfiguracionInserta(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_INSERT", "", Configuracion.cIdUsuario, Configuracion.cIdPerfil, Configuracion.cIdElemento, Configuracion.cIdModulo, Configuracion.cIdSistema, Configuracion.cIdArea, Configuracion.cIdPaisOrigenUsuario, Configuracion.cIdEmpresa, Configuracion.bEstadoRegistroConfiguracion).ReturnValue.ToString
        Return x
    End Function

    Public Function ConfiguracionEdita(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_UPDATE", "", Configuracion.cIdUsuario, Configuracion.cIdPerfil, Configuracion.cIdElemento, Configuracion.cIdModulo, Configuracion.cIdSistema, Configuracion.cIdArea, Configuracion.cIdPaisOrigenUsuario, Configuracion.cIdEmpresa, Configuracion.bEstadoRegistroConfiguracion).ReturnValue.ToString
        Return x
    End Function

    Public Function ConfiguracionActualizarPerfil(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "UPDATE GNRL_CONFIGURACION SET cIdPerfil = '" & Configuracion.cIdPerfil & "' " &
                                           "WHERE cIdUsuario = '" & Configuracion.cIdUsuario & "'",
                                           "", "", "", "", "", "", "", "", "1").ReturnValue.ToString
        Return x
    End Function

    Public Function ConfiguracionElimina(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        Dim x
        'x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "UPDATE GNRL_CONFIGURACION SET bEstadoRegistroConfiguracion = 0 " &
        '                                   "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND cIdUsuario = '" & Configuracion.cIdUsuario & "' AND " &
        '                                   "      cIdElemento = '" & Configuracion.cIdElemento & "' AND cIdModulo = '" & Configuracion.cIdModulo & "' AND " &
        '                                   "      cIdSistema = '" & Configuracion.cIdSistema & "' AND cIdArea = '" & Configuracion.cIdArea & "'",
        '                                   "", "", "", "", "", "", "", "", "1").ReturnValue.ToString
        x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "UPDATE GNRL_CONFIGURACION SET bEstadoRegistroConfiguracion = 0 " &
                                           "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND cIdUsuario = '" & Configuracion.cIdUsuario & "' AND " &
                                           "      cIdElemento = '" & Configuracion.cIdElemento & "' AND cIdModulo = '" & Configuracion.cIdModulo & "' AND " &
                                           "      cIdSistema = '" & Configuracion.cIdSistema & "' AND cIdArea = '" & Configuracion.cIdArea & "'",
                                           "", "", "", "", "", "", "", "", "1").ReturnValue.ToString
        Return x
    End Function

    Public Function ConfiguracionEliminaRegistro(ByVal Configuracion As GNRL_CONFIGURACION) As Int32
        Dim x
        'x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "DELETE GNRL_CONFIGURACION " & _
        '                                   "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND cIdUsuario + cIdPerfil IN (SELECT USU.cIdUsuario + USU.cIdPerfil FROM GNRL_USUARIO AS USU WHERE USU.cIdEmpresa = '" & IdEmpresa & "' AND USU.cIdPuntoVenta = '" & IdPuntoVenta & "')",
        '                                   "", "", "", "", "", "", "1").ReturnValue.ToString
        'x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "DELETE GNRL_CONFIGURACION " &
        '                                   "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND cIdUsuario + cIdPerfil IN (SELECT USU.cIdUsuario + USU.cIdPerfil FROM GNRL_USUARIO AS USU WHERE USU.cIdEmpresa = '" & IdEmpresa & "')",
        '                                   "", "", "", "", "", "", "1").ReturnValue.ToString
        'x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "DELETE GNRL_CONFIGURACION " &
        '                                   "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND " &
        '                                   "      cIdUsuario = '" & Configuracion.cIdUsuario & "' AND " &
        '                                   "      cIdPaisOrigenUsuario = '" & Configuracion.cIdPaisOrigenUsuario & "' AND " &
        '                                   "      cIdEmpresa = '" & Configuracion.cIdEmpresa & "' ",
        '                                   "", "", "", "", "", "", "", "", "1").ReturnValue.ToString
        'x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "DELETE GNRL_CONFIGURACION " &
        '                                   "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND " &
        '                                   "      cIdUsuario = '" & Configuracion.cIdUsuario & "' AND " &
        '                                   "      cIdPaisOrigenUsuario = '" & Configuracion.cIdPaisOrigenUsuario & "'",
        '                                   "", "", "", "", "", "", "", "", "1").ReturnValue.ToString
        x = Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "DELETE GNRL_CONFIGURACION " &
                                           "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND " &
                                           "      cIdUsuario = '" & Configuracion.cIdUsuario & "' AND " &
                                           "      cIdPaisOrigenUsuario = '" & Configuracion.cIdPaisOrigenUsuario & "'",
                                           "", "", "", "", "", "", "", "", "1").ReturnValue.ToString
        Return x
    End Function

    Public Function ConfiguracionExiste(ByVal Configuracion As GNRL_CONFIGURACION) As Boolean
        If Data.PA_GNRL_MNT_CONFIGURACION("SQL_NONE", "SELECT * FROM GNRL_CONFIGURACION " &
                                          "WHERE cIdPerfil = '" & Configuracion.cIdPerfil & "' AND cIdUsuario = '" & Configuracion.cIdUsuario & "' AND " &
                                          "      cIdElemento = '" & Configuracion.cIdElemento & "' AND cIdModulo = '" & Configuracion.cIdModulo & "' AND " &
                                          "      cIdSistema = '" & Configuracion.cIdSistema & "' AND cIdArea = '" & Configuracion.cIdArea & "' AND " &
                                          "      bEstadoRegistroConfiguracion = 1",
                                          "", "", "", "", "", "", "", "", "1").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class