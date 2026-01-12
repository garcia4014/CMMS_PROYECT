Public Class clsPersonalMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function PersonalListarCombo(ByVal IdUnidadTrabajo As String) As List(Of RRHH_PERSONAL)
        Dim Consulta = Data.PA_RRHH_MNT_PERSONAL("SQL_NONE", "SELECT * FROM RRHH_PERSONAL WHERE (vIdUnidadTrabajoPersonal = '" & IdUnidadTrabajo & "' OR '*' = '" & IdUnidadTrabajo & "') ORDER BY vApellidoPaternoPersonal",
                                                 "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0",
                                                 Now, "", Now, "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "")
        Dim Coleccion As New List(Of RRHH_PERSONAL)
        For Each RRHH_PERSONAL In Consulta
            Dim Personal As New RRHH_PERSONAL
            Personal.cIdPersonal = RRHH_PERSONAL.cIdPersonal
            Personal.vNombreCompletoPersonal = RRHH_PERSONAL.vNombreCompletoPersonal
            Coleccion.Add(Personal)
        Next
        Return Coleccion
    End Function

    'Public Function PersonalListarPorId(ByVal IdPersonal As String,
    '                                    ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As RRHH_PERSONAL
    Public Function PersonalListarPorId(ByVal IdPersonal As String,
                                    ByVal IdEmpresa As String) As RRHH_PERSONAL

        'Dim Consulta = Data.PA_RRHH_MNT_PERSONAL("SQL_NONE", "SELECT * FROM RRHH_PERSONAL " &
        '                                             "WHERE cIdPersonal = '" & IdPersonal & "' AND cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "'",
        '                                             "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0",
        '                                             Now, "", Now, "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "")
        Dim Consulta = Data.PA_RRHH_MNT_PERSONAL("SQL_NONE", "SELECT * FROM RRHH_PERSONAL " &
                                                     "WHERE cIdPersonal = '" & IdPersonal & "' AND cIdEmpresa = '" & IdEmpresa & "'",
                                                     "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0",
                                                     Now, "", Now, "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "")
        Dim Coleccion As New RRHH_PERSONAL
        For Each RRHH_PERSONAL In Consulta
            Coleccion.cIdPersonal = RRHH_PERSONAL.cIdPersonal
            Coleccion.cIdTipoDocumento = RRHH_PERSONAL.cIdTipoDocumento
            Coleccion.vNumeroDocumentoPersonal = RRHH_PERSONAL.vNumeroDocumentoPersonal
            Coleccion.vApellidoPaternoPersonal = RRHH_PERSONAL.vApellidoPaternoPersonal
            Coleccion.vApellidoMaternoPersonal = RRHH_PERSONAL.vApellidoMaternoPersonal
            Coleccion.vNombresPersonal = RRHH_PERSONAL.vNombresPersonal
            Coleccion.vNombreCompletoPersonal = RRHH_PERSONAL.vNombreCompletoPersonal
            Coleccion.cGeneroPersonal = RRHH_PERSONAL.cGeneroPersonal
            Coleccion.cIdTipoPersona = RRHH_PERSONAL.cIdTipoPersona
            Coleccion.cIdPaisUbicacionGeografica = RRHH_PERSONAL.cIdPaisUbicacionGeografica
            Coleccion.cIdDepartamentoUbicacionGeografica = RRHH_PERSONAL.cIdDepartamentoUbicacionGeografica
            Coleccion.cIdProvinciaUbicacionGeografica = RRHH_PERSONAL.cIdProvinciaUbicacionGeografica
            Coleccion.cIdDistritoUbicacionGeografica = RRHH_PERSONAL.cIdDistritoUbicacionGeografica
            Coleccion.cIdPuntoVenta = RRHH_PERSONAL.cIdPuntoVenta
            Coleccion.cIdEmpresa = RRHH_PERSONAL.cIdEmpresa
            Coleccion.cIdEstadoCivil = RRHH_PERSONAL.cIdEstadoCivil
            Coleccion.cIdBanco = RRHH_PERSONAL.cIdBanco
            Coleccion.cIdTipoMoneda = RRHH_PERSONAL.cIdBanco
            Coleccion.vNumeroCuentaCorrientePersonal = RRHH_PERSONAL.vNumeroCuentaCorrientePersonal
            Coleccion.bEstadoRegistroPersonal = RRHH_PERSONAL.bEstadoRegistroPersonal
            Coleccion.dFechaNacimientoPersonal = RRHH_PERSONAL.dFechaNacimientoPersonal
            Coleccion.cIdNacionalidad = RRHH_PERSONAL.cIdNacionalidad
            Coleccion.dFechaIngresoPersonal = RRHH_PERSONAL.dFechaIngresoPersonal
            Coleccion.vTelefonoPersonal = RRHH_PERSONAL.vTelefonoPersonal
            Coleccion.vCelularPersonal = RRHH_PERSONAL.vCelularPersonal
            Coleccion.vEmailPersonal = RRHH_PERSONAL.vEmailPersonal
            Coleccion.cIdTipoDomicilioPersonal = RRHH_PERSONAL.cIdTipoDomicilioPersonal
            Coleccion.cIdTipoVia = RRHH_PERSONAL.cIdTipoVia
            Coleccion.cIdZona = RRHH_PERSONAL.cIdZona
            Coleccion.vViaPersonal = RRHH_PERSONAL.vViaPersonal
            Coleccion.vReferenciaPersonal = RRHH_PERSONAL.vReferenciaPersonal
            Coleccion.vAutoGeneradoPersonal = RRHH_PERSONAL.vAutoGeneradoPersonal
            Coleccion.vFaxPersonal = RRHH_PERSONAL.vFaxPersonal
            Coleccion.vNumeroViaPersonal = RRHH_PERSONAL.vNumeroViaPersonal
            Coleccion.vNumeroInteriorPersonal = RRHH_PERSONAL.vNumeroInteriorPersonal
            Coleccion.vZonaPersonal = RRHH_PERSONAL.vZonaPersonal
            Coleccion.dFechaCesePersonal = RRHH_PERSONAL.dFechaCesePersonal
            Coleccion.cIdCategoriaPersonal = RRHH_PERSONAL.cIdCategoriaPersonal
        Next
        Return Coleccion
    End Function

    'Public Function PersonalListaGrid(ByVal Filtro As String, ByVal Buscar As String,
    '                                  ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_RRHH_PERSONAL)
    Public Function PersonalListaGrid(ByVal Filtro As String, ByVal Buscar As String,
                                     ByVal IdEmpresa As String) As List(Of VI_RRHH_PERSONAL)
        Dim Consulta = Data.PA_RRHH_MNT_PERSONAL("SQL_NONE", "SELECT cIdPersonal, vNombreCompletoPersonal, cIdTipoPersona, " &
                                                      "cGeneroPersonal, bEstadoRegistroPersonal, '' cIdTipoPlanilla, dFechaIngresoPersonal, " &
                                                      "cIdTipoDocumento " &
                                                      "FROM RRHH_PERSONAL " &
                                                      "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroPersonal = 1 " &
                                                      "      AND cIdEmpresa = '" & IdEmpresa & "' " &
                                                      "ORDER BY " & Filtro,
                                                      "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0",
                                                      Now, "", Now, "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "")
        '"WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroPersonal = 1 " & _
        'IIf(booBuscarCompleto = False, "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroPersonal = 1 ",
        '                               "WHERE " & Filtro & " LIKE UPPER ('" & Buscar & "') AND bEstadoRegistroPersonal = 1 ") & _

        Dim Coleccion As New List(Of VI_RRHH_PERSONAL)
        For Each Busqueda In Consulta
            Dim BuscarPersonal As New VI_RRHH_PERSONAL
            BuscarPersonal.Codigo = Busqueda.cIdPersonal
            BuscarPersonal.Descripcion = Busqueda.vNombreCompletoPersonal
            BuscarPersonal.Estado = Busqueda.bEstadoRegistroPersonal
            BuscarPersonal.Genero = Busqueda.cGeneroPersonal
            BuscarPersonal.Tipo = Busqueda.cIdTipoPersona
            BuscarPersonal.Fecha_Ing = Busqueda.dFechaIngresoPersonal
            BuscarPersonal.Tipo_Doc = Busqueda.cIdTipoDocumento

            Coleccion.Add(BuscarPersonal)
        Next
        Return Coleccion
    End Function

    Public Function PersonalInserta(ByVal Personal As RRHH_PERSONAL) As Int32
        Dim x
        'x = Data.PA_RRHH_MNT_PERSONAL("SQL_INSERT", "", Personal.cIdPersonal, Personal.vDescripcionPersonal, Personal.vDescripcionAbreviadaPersonal, _
        '                                            Personal.bEstadoRegistroPersonal, "").ReturnValue.ToString
        x = Data.PA_RRHH_MNT_PERSONAL("SQL_INSERT", "", Personal.cIdPersonal, Personal.cIdTipoDocumento, Personal.vNumeroDocumentoPersonal,
                                      Personal.vApellidoPaternoPersonal, Personal.vApellidoMaternoPersonal, Personal.vNombresPersonal, Personal.vNombreCompletoPersonal,
                                      Personal.cGeneroPersonal, Personal.cIdTipoPersona, Personal.cIdPaisUbicacionGeografica, Personal.cIdDepartamentoUbicacionGeografica,
                                      Personal.cIdProvinciaUbicacionGeografica, Personal.cIdDistritoUbicacionGeografica, Personal.cIdPuntoVenta, Personal.cIdEmpresa,
                                      Personal.cIdEstadoCivil, Personal.cIdBanco, Personal.cIdTipoMoneda, Personal.vNumeroCuentaCorrientePersonal,
                                      Personal.bEstadoRegistroPersonal, Personal.dFechaNacimientoPersonal, Personal.cIdNacionalidad, Personal.dFechaIngresoPersonal,
                                      Personal.vTelefonoPersonal, Personal.vCelularPersonal, Personal.vEmailPersonal, Personal.cIdTipoDomicilioPersonal,
                                      Personal.cIdTipoVia, Personal.vViaPersonal, Personal.cIdZona, Personal.vReferenciaPersonal, Personal.vAutoGeneradoPersonal,
                                      Personal.vFaxPersonal, Personal.vNumeroViaPersonal, Personal.vNumeroInteriorPersonal, Personal.vZonaPersonal,
                                      Personal.dFechaCesePersonal, Personal.cIdCategoriaPersonal, Personal.cIdPersonal).ReturnValue.ToString
        Return x
    End Function

    Public Function PersonalEdita(ByVal Personal As RRHH_PERSONAL) As Int32
        Dim x
        'x = Data.PA_RRHH_MNT_PERSONAL("SQL_UPDATE", "", Personal.cIdPersonal, Personal.vDescripcionPersonal, Personal.vDescripcionAbreviadaPersonal, _
        '                                            Personal.bEstadoRegistroPersonal, "").ReturnValue.ToString
        x = Data.PA_RRHH_MNT_PERSONAL("SQL_UPDATE", "", Personal.cIdPersonal, Personal.cIdTipoDocumento, Personal.vNumeroDocumentoPersonal,
                                      Personal.vApellidoPaternoPersonal, Personal.vApellidoMaternoPersonal, Personal.vNombresPersonal, Personal.vNombreCompletoPersonal,
                                      Personal.cGeneroPersonal, Personal.cIdTipoPersona, Personal.cIdPaisUbicacionGeografica, Personal.cIdDepartamentoUbicacionGeografica,
                                      Personal.cIdProvinciaUbicacionGeografica, Personal.cIdDistritoUbicacionGeografica, Personal.cIdPuntoVenta, Personal.cIdEmpresa,
                                      Personal.cIdEstadoCivil, Personal.cIdBanco, Personal.cIdTipoMoneda, Personal.vNumeroCuentaCorrientePersonal,
                                      Personal.bEstadoRegistroPersonal, Personal.dFechaNacimientoPersonal, Personal.cIdNacionalidad, Personal.dFechaIngresoPersonal,
                                      Personal.vTelefonoPersonal, Personal.vCelularPersonal, Personal.vEmailPersonal, Personal.cIdTipoDomicilioPersonal,
                                      Personal.cIdTipoVia, Personal.vViaPersonal, Personal.cIdZona, Personal.vReferenciaPersonal, Personal.vAutoGeneradoPersonal,
                                      Personal.vFaxPersonal, Personal.vNumeroViaPersonal, Personal.vNumeroInteriorPersonal, Personal.vZonaPersonal,
                                      Personal.dFechaCesePersonal, Personal.cIdCategoriaPersonal, Personal.cIdPersonal).ReturnValue.ToString
        Return x
    End Function

    Public Function PersonalElimina(ByVal Personal As RRHH_PERSONAL) As Int32
        Dim x
        'x = Data.PA_RRHH_MNT_PERSONAL("SQL_NONE", "UPDATE RRHH_PERSONAL SET bEstadoRegistroPersonal = 0 WHERE cIdPersonal = '" & Personal.cIdPersonal & "'",
        '                             "", "", "", "1", "").ReturnValue.ToString
        x = Data.PA_RRHH_MNT_PERSONAL("SQL_NONE", "UPDATE RRHH_PERSONAL SET bEstadoRegistroPersonal = 0 WHERE cIdPersonal = '" & Personal.cIdPersonal & "'",
                                      "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0",
                                      Now, "", Now, "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "")
        Return x
    End Function

    'Public Function PersonalExiste(ByVal IdPersonal As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
    Public Function PersonalExiste(ByVal IdPersonal As String, ByVal IdEmpresa As String) As Boolean
        If Data.PA_RRHH_MNT_PERSONAL("SQL_NONE", "SELECT * FROM RRHH_PERSONAL " &
                                             "WHERE cIdPersonal = '" & IdPersonal & "' AND cIdEmpresa = '" & IdEmpresa & "'",
                                      "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0",
                                      Now, "", Now, "", "", "", "", "", "", "", "", "", "", "", "", "", Now, "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class