Public Class clsAreaElementoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function AreaElementoListarPorId(ByVal AreaElemento As GNRL_AREAELEMENTO) As GNRL_AREAELEMENTO
        Dim Consulta = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "SELECT * FROM GNRL_AREAELEMENTO " &
                                                     "WHERE cIdElemento = '" & AreaElemento.cIdElemento & "' AND cIdSistema = '" & AreaElemento.cIdSistema & "' AND " &
                                                     "      cIdModulo = '" & AreaElemento.cIdModulo & "' AND cIdArea = '" & AreaElemento.cIdArea & "'", "", "", "", "", "0", "")
        Dim Coleccion As New GNRL_AREAELEMENTO
        For Each GNRL_AREAELEMENTO In Consulta
            Coleccion.cIdElemento = GNRL_AREAELEMENTO.cIdElemento
            Coleccion.cIdSistema = GNRL_AREAELEMENTO.cIdSistema
            Coleccion.cIdModulo = GNRL_AREAELEMENTO.cIdModulo
            Coleccion.cIdArea = GNRL_AREAELEMENTO.cIdArea
            Coleccion.bEstadoRegistroAreaElemento = GNRL_AREAELEMENTO.bEstadoRegistroAreaElemento
        Next
        Return Coleccion
    End Function

    'JMUG: 08/02/2023
    'Public Function AreaElementoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdSistema As String,
    '                                      ByVal IdModulo As String, ByVal IdArea As String, ByVal IdPerfil As String,
    '                                      ByVal IdUsuario As String) As List(Of VI_GNRL_ELEMENTO)
    Public Function AreaElementoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdSistema As String,
                                      ByVal IdModulo As String, ByVal IdPerfil As String,
                                      ByVal IdUsuario As String) As List(Of VI_GNRL_ELEMENTO)
        'Este si puede devolver una colección de datos es decir varios registros
        'JMUG: 08/02/2023
        'Dim Consulta = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "SELECT AREAELEM.cIdElemento, AREAELEM.cIdModulo, AREAELEM.cIdSistema, AREAELEM.bEstadoRegistroAreaElemento, " &
        '                                             "ELEM.vDescripcionElemento, CONF.cIdUsuario " &
        '                                             "FROM GNRL_AREAELEMENTO AREAELEM INNER JOIN GNRL_ELEMENTO ELEM ON " &
        '                                             "     AREAELEM.cIdElemento = ELEM.cIdElemento AND AREAELEM.cIdSistema = ELEM.cIdSistema LEFT JOIN GNRL_CONFIGURACION CONF ON " &
        '                                             "     CONF.cIdPerfil = '" & IdPerfil & "' AND CONF.cIdElemento = AREAELEM.cIdElemento " &
        '                                             "     AND CONF.cIdModulo = AREAELEM.cIdModulo AND CONF.cIdSistema = AREAELEM.cIdSistema " &
        '                                             "     AND CONF.cIdArea = AREAELEM.cIdArea AND CONF.cIdUsuario = '" & IdUsuario & "' " &
        '                                             "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND AREAELEM.bEstadoRegistroAreaElemento = 1 AND " &
        '                                             "       AREAELEM.cIdSistema = '" & IdSistema & "' AND " &
        '                                             "       AREAELEM.cIdModulo = '" & IdModulo & "' AND AREAELEM.cIdArea = '" & IdArea & "'",
        '                                             "", "", "", "", "0", "")
        Dim Consulta = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "SELECT AREAELEM.cIdElemento, AREAELEM.cIdModulo, AREAELEM.cIdSistema, AREAELEM.bEstadoRegistroAreaElemento, " &
                                                     "ELEM.vDescripcionElemento, CONF.cIdUsuario " &
                                                     "FROM GNRL_AREAELEMENTO AREAELEM INNER JOIN GNRL_ELEMENTO ELEM ON " &
                                                     "     AREAELEM.cIdElemento = ELEM.cIdElemento AND AREAELEM.cIdSistema = ELEM.cIdSistema LEFT JOIN GNRL_CONFIGURACION CONF ON " &
                                                     "     CONF.cIdPerfil = '" & IdPerfil & "' AND CONF.cIdElemento = AREAELEM.cIdElemento " &
                                                     "     AND CONF.cIdModulo = AREAELEM.cIdModulo AND CONF.cIdSistema = AREAELEM.cIdSistema " &
                                                     "     AND CONF.cIdUsuario = '" & IdUsuario & "' " &
                                                     "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND AREAELEM.bEstadoRegistroAreaElemento = 1 AND " &
                                                     "       AREAELEM.cIdSistema = '" & IdSistema & "' AND " &
                                                     "       AREAELEM.cIdModulo = '" & IdModulo & "'",
                                                     "", "", "", "", "0", "")

        Dim Coleccion As New List(Of VI_GNRL_ELEMENTO)
        For Each Busqueda In Consulta
            Dim BuscarEle As New VI_GNRL_ELEMENTO
            BuscarEle.Codigo = Busqueda.cIdElemento
            BuscarEle.Descripcion = Busqueda.vDescripcionElemento
            BuscarEle.Estado = Busqueda.bEstadoRegistroAreaElemento
            Coleccion.Add(BuscarEle)
        Next
        Return Coleccion
    End Function

    'Public Function ModuloListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdArea As String) As List(Of VI_GNRL_MODULO)
    Public Function ModuloListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_MODULO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "SELECT AREAELEM.cIdModulo, AREAELEM.cIdSistema, AREAELEM.bEstadoRegistroAreaElemento, " &
        '                                             "MOD.vDescripcionModulo AS vDescripcionElemento " &
        '                                             "FROM GNRL_AREAELEMENTO AREAELEM INNER JOIN GNRL_MODULO MOD ON " &
        '                                             "     AREAELEM.cIdModulo = MOD.cIdModulo " &
        '                                             "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND AREAELEM.bEstadoRegistroAreaElemento = 1 AND " &
        '                                             "       AREAELEM.cIdArea = '" & IdArea & "' " &
        '                                             "GROUP BY AREAELEM.cIdModulo, AREAELEM.cIdSistema, AREAELEM.bEstadoRegistroAreaElemento, MOD.vDescripcionModulo",
        '                                             "", "", "", "", "0", "")
        Dim Consulta = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "SELECT AREAELEM.cIdModulo, AREAELEM.cIdSistema, AREAELEM.bEstadoRegistroAreaElemento, " &
                                                     "MOD.vDescripcionModulo AS vDescripcionElemento " &
                                                     "FROM GNRL_AREAELEMENTO AREAELEM INNER JOIN GNRL_MODULO MOD ON " &
                                                     "     AREAELEM.cIdModulo = MOD.cIdModulo " &
                                                     "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND AREAELEM.bEstadoRegistroAreaElemento = 1 AND MOD.bEstadoRegistroModulo = 1 " &
                                                     "GROUP BY AREAELEM.cIdModulo, AREAELEM.cIdSistema, AREAELEM.bEstadoRegistroAreaElemento, MOD.vDescripcionModulo",
                                                     "", "", "", "", "0", "")


        Dim Coleccion As New List(Of VI_GNRL_MODULO)
        For Each Busqueda In Consulta
            Dim BuscarMod As New VI_GNRL_MODULO
            BuscarMod.Codigo = Busqueda.cIdModulo
            BuscarMod.Descripcion = Busqueda.vDescripcionElemento
            BuscarMod.Estado = Busqueda.bEstadoRegistroAreaElemento
            BuscarMod.Sistema = Busqueda.cIdSistema
            Coleccion.Add(BuscarMod)
        Next
        Return Coleccion
    End Function

    Public Function AreaElementoListaGridFiltrado(ByVal Filtro As String, ByVal Buscar As String, ByVal IdSistema As String,
                                                  ByVal IdModulo As String, ByVal IdArea As String, ByVal IdPerfil As String) As List(Of VI_GNRL_ELEMENTO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "SELECT AREAELEM.cIdElemento, AREAELEM.cIdModulo, AREAELEM.cIdSistema, AREAELEM.bEstadoRegistroAreaElemento, " &
                                                     "ELEM.vDescripcionElemento " &
                                                     "FROM GNRL_AREAELEMENTO AREAELEM INNER JOIN GNRL_ELEMENTO ELEM ON " &
                                                     "     AREAELEM.cIdElemento = ELEM.cIdElemento " &
                                                     "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND AREAELEM.bEstadoRegistroAreaElemento = 1 AND " &
                                                     "       AREAELEM.cIdSistema = '" & IdSistema & "' AND " &
                                                     "       AREAELEM.cIdModulo = '" & IdModulo & "' AND AREAELEM.cIdArea = '" & IdArea & "' AND " &
                                                     "       AREAELEM.cIdElemento NOT IN (SELECT cIdElemento FROM GNRL_CONFIGURACION WHERE cIdPerfil = '" & IdPerfil & "')",
                                                     "", "", "", "", "0", "")
        Dim Coleccion As New List(Of VI_GNRL_ELEMENTO)
        For Each Busqueda In Consulta
            Dim BuscarEle As New VI_GNRL_ELEMENTO
            BuscarEle.Codigo = Busqueda.cIdElemento
            BuscarEle.Descripcion = Busqueda.vDescripcionElemento
            BuscarEle.Estado = Busqueda.bEstadoRegistroAreaElemento
            Coleccion.Add(BuscarEle)
        Next
        Return Coleccion
    End Function

    Public Function AreaElementoInserta(ByVal AreaElemento As GNRL_AREAELEMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_INSERT", "", AreaElemento.cIdElemento, AreaElemento.cIdModulo, AreaElemento.cIdSistema, AreaElemento.cIdArea, AreaElemento.bEstadoRegistroAreaElemento, AreaElemento.cIdElemento).ReturnValue.ToString
        Return x
    End Function

    Public Function AreaElementoEdita(ByVal AreaElemento As GNRL_AREAELEMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_UPDATE", "", AreaElemento.cIdElemento, AreaElemento.cIdModulo, AreaElemento.cIdSistema, AreaElemento.cIdArea, AreaElemento.bEstadoRegistroAreaElemento, AreaElemento.cIdElemento).ReturnValue.ToString
        Return x
    End Function

    Public Function AreaElementoElimina(ByVal AreaElemento As GNRL_AREAELEMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "UPDATE GNRL_AREAELEMENTO SET bEstadoRegistroAreaElemento = 0 WHERE cIdElemento = '" & AreaElemento.cIdElemento & "' AND " &
                                          "            cIdSistema = '" & AreaElemento.cIdSistema & "' AND cIdModulo = '" & AreaElemento.cIdModulo & "' AND cIdArea = '" & AreaElemento.cIdArea & "'",
                                          "", "", "", "", "0", "").ReturnValue.ToString
        Return x
    End Function

    Public Function AreaElementoExiste(ByVal AreaElemento As GNRL_AREAELEMENTO) As Boolean
        If Data.PA_GNRL_MNT_AREAELEMENTO("SQL_NONE", "SELECT * FROM GNRL_AREAELEMENTO WHERE cIdAreaElemento = '" & AreaElemento.cIdElemento & "' AND bEstadoRegistroAreaElemento = 1 AND " &
                                         "           cIdSistema = '" & AreaElemento.cIdSistema & "' AND cIdModulo = '" & AreaElemento.cIdModulo & "' AND cIdArea = '" & AreaElemento.cIdArea & "'",
                                         "", "", "", "", "0", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class