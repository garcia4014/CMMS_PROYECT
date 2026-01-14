Imports System.Data
Imports System.Data.SqlClient
Public Class clsComponenteOrdenTrabajoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function ComponenteOrdenTrabajoGetData(strQuery As String) As DataTable
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

    Public Function ComponenteOrdenTrabajoListarCombo() As List(Of LOGI_COMPONENTEORDENTRABAJO)
        Dim Consulta = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO",
                                                      "", "", "", "", "", "", "", "", "", "", "", Now, Now, 0, 0, "")
        Dim Coleccion As New List(Of LOGI_COMPONENTEORDENTRABAJO)
        For Each ComponenteOrdenTrabajo In Consulta
            Dim ComOrdTra As New LOGI_COMPONENTEORDENTRABAJO
            ComOrdTra.cIdEquipoCabeceraOrdenTrabajo = ComponenteOrdenTrabajo.cIdEquipoCabeceraOrdenTrabajo
            'TipAct.cIdActividadComponenteOrdenTrabajoOrdenTrabajo  = ComponenteOrdenTrabajo.vDescripcionComponenteOrdenTrabajo
            ComOrdTra.cIdCatalogoComponenteOrdenTrabajo = ComponenteOrdenTrabajo.cIdCatalogoComponenteOrdenTrabajo
            Coleccion.Add(ComOrdTra)
        Next
        Return Coleccion
    End Function

    'Public Function ComponenteOrdenTrabajoListarPorId(ByVal IdEquipo As String, ByVal IdNroItem As String) As LOGI_COMPONENTEORDENTRABAJO
    '    Dim Consulta = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO WHERE " &
    '                                                          "cIdEquipo = '" & IdEquipo & "' AND nIdNumeroItemComponenteOrdenTrabajo = " & IdNroItem &
    '                                               " AND bEstadoRegistroComponenteOrdenTrabajo = 1 ",
    '                                                          "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "")
    '    Dim Coleccion As New LOGI_COMPONENTEORDENTRABAJO
    '    For Each LOGI_COMPONENTEORDENTRABAJO In Consulta
    '        Coleccion.cIdEquipo = LOGI_COMPONENTEORDENTRABAJO.cIdEquipo
    '        Coleccion.nIdNumeroItemComponenteOrdenTrabajo = LOGI_COMPONENTEORDENTRABAJO.nIdNumeroItemComponenteOrdenTrabajo
    '        Coleccion.vDescripcionComponenteOrdenTrabajo = LOGI_COMPONENTEORDENTRABAJO.vDescripcionComponenteOrdenTrabajo
    '        Coleccion.vObservacionComponenteOrdenTrabajo = LOGI_COMPONENTEORDENTRABAJO.vObservacionComponenteOrdenTrabajo
    '        Coleccion.vTituloComponenteOrdenTrabajo = LOGI_COMPONENTEORDENTRABAJO.vTituloComponenteOrdenTrabajo
    '        Coleccion.bEstadoRegistroComponenteOrdenTrabajo = LOGI_COMPONENTEORDENTRABAJO.bEstadoRegistroComponenteOrdenTrabajo
    '        Coleccion.dFechaTransaccionComponenteOrdenTrabajo = LOGI_COMPONENTEORDENTRABAJO.dFechaTransaccionComponenteOrdenTrabajo
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function ComponenteOrdenTrabajoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_COMPONENTEORDENTRABAJO)
    '    'Este si puede devolver una colección de datos es decir varios registros
    '    Dim Consulta = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO " &
    '                                               "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroComponenteOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "')",
    '                                               "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "")
    '    Dim Coleccion As New List(Of VI_LOGI_COMPONENTEORDENTRABAJO)
    '    For Each Busqueda In Consulta
    '        Dim BuscarFam As New VI_LOGI_COMPONENTEORDENTRABAJO
    '        BuscarFam.Codigo = Busqueda.cIdEquipo
    '        BuscarFam.NumeroItem = Busqueda.nIdNumeroItemComponenteOrdenTrabajo
    '        BuscarFam.Titulo = Busqueda.vTituloComponenteOrdenTrabajo
    '        BuscarFam.Descripcion = Busqueda.vDescripcionComponenteOrdenTrabajo
    '        BuscarFam.Observacion = Busqueda.vObservacionComponenteOrdenTrabajo
    '        BuscarFam.Estado = Busqueda.bEstadoRegistroComponenteOrdenTrabajo
    '        Coleccion.Add(BuscarFam)
    '    Next
    '    Return Coleccion
    'End Function

    'Public Function ComponenteOrdenTrabajoListaGridPorId(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_LOGI_COMPONENTEORDENTRABAJO)
    Public Function ComponenteOrdenTrabajoListaGridPorOrdEquipo(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroCor As String,
                                            ByVal IdEmp As String, ByVal IdEquipo As String, ByVal IdArticuloSAP As String) ' As List(Of VI_LOGI_COMPONENTEORDENTRABAJO)
        '                                        ByVal IdNroChkLis As String) As List(Of VI_LOGI_COMPONENTEORDENTRABAJO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT COMORDTRA.*, ACTComponenteOrdenTrabajo.vDescripcionActividadComponenteOrdenTrabajo FROM LOGI_COMPONENTEORDENTRABAJO AS COMORDTRA INNER JOIN LOGI_ACTIVIDADComponenteOrdenTrabajo AS ACTComponenteOrdenTrabajo ON " &
        '                                           "COMORDTRA.cIdActividadComponenteOrdenTrabajoOrdenTrabajo = ACTComponenteOrdenTrabajo.cIdActividadComponenteOrdenTrabajo " &
        '                                           "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroComponenteOrdenTrabajo = '" & Estado & "' OR '*' = '" & Estado & "')",
        '                                           "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "")
        'Dim Consulta = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT COMORDTRA.*, ACTComponenteOrdenTrabajo.vDescripcionActividadComponenteOrdenTrabajo, " &
        '                                           "EQU.vDescripcionEquipo, CAT.cIdSistemaFuncional, SISFUN.vDescripcionSistemaFuncional, EQU.cIdTipoActivo, TIPACT.vDescripcionTipoActivo, CAT.vDescripcionCatalogo, ISNULL(COMORDTRA.cEstadoActividadComponenteOrdenTrabajoOrdenTrabajo, '') AS cEstadoActividadComponenteOrdenTrabajoOrdenTrabajo " &
        '                                           "FROM LOGI_COMPONENTEORDENTRABAJO AS COMORDTRA INNER JOIN LOGI_ACTIVIDADComponenteOrdenTrabajo AS ACTComponenteOrdenTrabajo ON " &
        '                                           "COMORDTRA.cIdActividadComponenteOrdenTrabajoOrdenTrabajo = ACTComponenteOrdenTrabajo.cIdActividadComponenteOrdenTrabajo " &
        '                                           "INNER JOIN LOGI_EQUIPO AS EQU ON " &
        '                                           "COMORDTRA.cIdEquipoCabeceraOrdenTrabajo = EQU.cIdEquipo " &
        '                                           "LEFT JOIN LOGI_CATALOGO AS CAT ON " &
        '                                           "COMORDTRA.cIdCatalogoComponenteOrdenTrabajoOrdenTrabajo = CAT.cIdCatalogo AND " &
        '                                           "COMORDTRA.cIdJerarquiaCatalogoComponenteOrdenTrabajoOrdenTrabajo = CAT.cIdJerarquiaCatalogo " &
        '                                           "LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
        '                                           "EQU.cIdSistemaFuncionalEquipo = SISFUN.cIdSistemaFuncional " &
        '                                           "LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
        '                                           "EQU.cIdTipoActivo = TIPACT.cIdTipoActivo " &
        '                                           "WHERE " &
        '                                           "COMORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND " &
        '                                           "COMORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' AND " &
        '                                           "COMORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroCor & "' AND " &
        '                                           "COMORDTRA.cIdEmpresa = '" & IdEmp & "' AND " &
        '                                           "COMORDTRA.cIdEquipoCabeceraOrdenTrabajo = '" & IdEquipo & "' AND " &
        '                                           "COMORDTRA.vIdArticuloSAPCabeceraOrdenTrabajo = '" & IdArticuloSAP & "' ",
        '                                           "", "", "", "", "", "", "", "", "", "", "", Now, Now, "", "")

        Dim Consulta = ComponenteOrdenTrabajoGetData("SELECT COMORDTRA.*, " &
                                           "EQU.vDescripcionEquipo, CAT.cIdSistemaFuncional, SISFUN.vDescripcionSistemaFuncional, EQU.cIdTipoActivo, TIPACT.vDescripcionTipoActivo, CAT.vDescripcionCatalogo, ISNULL(COMORDTRA.cEstadoComponenteOrdenTrabajo, '') AS cEstadoComponenteOrdenTrabajo " &
                                           "FROM LOGI_COMPONENTEORDENTRABAJO AS COMORDTRA INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                           "COMORDTRA.cIdEquipoCabeceraOrdenTrabajo = EQU.cIdEquipo " &
                                           "LEFT JOIN LOGI_CATALOGO AS CAT ON " &
                                           "COMORDTRA.cIdCatalogoComponenteOrdenTrabajo = CAT.cIdCatalogo AND " &
                                           "COMORDTRA.cIdJerarquiaCatalogoComponenteOrdenTrabajo = CAT.cIdJerarquiaCatalogo " &
                                           "LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
                                           "EQU.cIdSistemaFuncionalEquipo = SISFUN.cIdSistemaFuncional " &
                                           "LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
                                           "EQU.cIdTipoActivo = TIPACT.cIdTipoActivo " &
                                           "WHERE " &
                                           "COMORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & IdTipDoc & "' AND " &
                                           "COMORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & IdNroSer & "' AND " &
                                           "COMORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & IdNroCor & "' AND " &
                                           "COMORDTRA.cIdEmpresa = '" & IdEmp & "' AND " &
                                           "COMORDTRA.cIdEquipoCabeceraOrdenTrabajo = '" & IdEquipo & "' AND " &
                                           "COMORDTRA.vIdArticuloSAPCabeceraOrdenTrabajo = '" & IdArticuloSAP & "' ")


        '                                                   "cIdNumeroCabeceraComponenteOrdenTrabajoPlantillaComponenteOrdenTrabajoOrdenTrabajo = '" & IdNroChkLis & "' ",


        'cIdCatalogoComponenteOrdenTrabajoOrdenTrabajo = ''
        'cIdJerarquiaCatalogoComponenteOrdenTrabajoOrdenTrabajo = ''

        Dim Coleccion As New List(Of VI_LOGI_COMPONENTEORDENTRABAJO)
        'Dim dtComponente As New DataTable
        'Dim FunMet As New clsFuncionesMetodos
        'dtComponente = FunMet.ConvertirListADataTable(Consulta)

        For Each Busqueda In Consulta.Rows
            Dim result = ValidarActividadesComponente(Busqueda("cIdTipoDocumentoCabeceraOrdenTrabajo"), Busqueda("vIdNumeroSerieCabeceraOrdenTrabajo"), Busqueda("vIdNumeroCorrelativoCabeceraOrdenTrabajo"), Busqueda("cIdEmpresa"), Busqueda("cIdCatalogoComponenteOrdenTrabajo"), Busqueda("cIdJerarquiaCatalogoComponenteOrdenTrabajo"))
            If (result > 0) Then
                Dim BuscarComOrdTra As New VI_LOGI_COMPONENTEORDENTRABAJO
                BuscarComOrdTra.IdCatalogo = Busqueda("cIdCatalogoComponenteOrdenTrabajo")
                BuscarComOrdTra.IdJerarquiaCatalogo = Busqueda("cIdJerarquiaCatalogoComponenteOrdenTrabajo")
                BuscarComOrdTra.IdArticuloSAPCabecera = Busqueda("vIdArticuloSAPCabeceraOrdenTrabajo")
                BuscarComOrdTra.IdEquipo = Busqueda("cIdEquipoCabeceraOrdenTrabajo")
                BuscarComOrdTra.Observacion = Busqueda("vObservacionComponenteOrdenTrabajo")
                BuscarComOrdTra.StatusComponente = Busqueda("cEstadoComponenteOrdenTrabajo")
                BuscarComOrdTra.IdEquipoComponente = Busqueda("cIdEquipoComponenteOrdenTrabajo")
                BuscarComOrdTra.DescripcionEquipo = Busqueda("vDescripcionCatalogo")
                Coleccion.Add(BuscarComOrdTra)

            End If
        Next
        Return Coleccion
    End Function

    Public Function ValidarActividadesComponente(ByVal cIdTipoDocumentoCabeceraOrdenTrabajo As String, ByVal vIdNumeroSerieCabeceraOrdenTrabajo As String, ByVal vIdNumeroCorrelativoCabeceraOrdenTrabajo As String, ByVal cIdEmpresa As String, ByVal cIdCatalogoComponenteOrdenTrabajo As String, ByVal cIdJerarquiaCatalogoComponenteOrdenTrabajo As String) As Int32
        Dim query As String = "SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO " &
                      "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & cIdTipoDocumentoCabeceraOrdenTrabajo & "' " &
                      "AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & vIdNumeroSerieCabeceraOrdenTrabajo & "' " &
                      "AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' " &
                      "AND cIdEmpresa = '" & cIdEmpresa & "' " &
                      "AND cIdCatalogoCheckListOrdenTrabajo = '" & cIdCatalogoComponenteOrdenTrabajo & "' " &
                      "AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & cIdJerarquiaCatalogoComponenteOrdenTrabajo & "' "

        Dim count = Data.ExecuteQuery(Of Int32)(query).FirstOrDefault
        Return count
    End Function

    'Public Function ComponenteOrdenTrabajoInserta(ByVal ComponenteOrdenTrabajo As LOGI_COMPONENTEORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_INSERT", "", ComponenteOrdenTrabajo.cIdEquipo, ComponenteOrdenTrabajo.nIdNumeroItemComponenteOrdenTrabajo, ComponenteOrdenTrabajo.vDescripcionComponenteOrdenTrabajo,
    '                                    ComponenteOrdenTrabajo.vObservacionComponenteOrdenTrabajo, ComponenteOrdenTrabajo.vTituloComponenteOrdenTrabajo, ComponenteOrdenTrabajo.bEstadoRegistroComponenteOrdenTrabajo,
    '                                    ComponenteOrdenTrabajo.dFechaTransaccionComponenteOrdenTrabajo, ComponenteOrdenTrabajo.cIdEquipo).ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function ComponenteOrdenTrabajoEdita(ByVal ComponenteOrdenTrabajo As LOGI_COMPONENTEORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_UPDATE", "", ComponenteOrdenTrabajo.cIdEquipo, ComponenteOrdenTrabajo.nIdNumeroItemComponenteOrdenTrabajo, ComponenteOrdenTrabajo.vDescripcionComponenteOrdenTrabajo,
    '                                    ComponenteOrdenTrabajo.vObservacionComponenteOrdenTrabajo, ComponenteOrdenTrabajo.vTituloComponenteOrdenTrabajo, ComponenteOrdenTrabajo.bEstadoRegistroComponenteOrdenTrabajo,
    '                                    ComponenteOrdenTrabajo.dFechaTransaccionComponenteOrdenTrabajo, ComponenteOrdenTrabajo.cIdEquipo).ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function ComponenteOrdenTrabajoElimina(ByVal ComponenteOrdenTrabajo As LOGI_COMPONENTEORDENTRABAJO) As Int32
    '    Dim x
    '    x = Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "UPDATE LOGI_COMPONENTEORDENTRABAJO Set bEstadoRegistroComponenteOrdenTrabajo = 0 WHERE cIdEquipo = '" & ComponenteOrdenTrabajo.cIdEquipo & "'",
    '                                       "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "").ReturnValue.ToString
    '    Return x
    'End Function

    'Public Function ComponenteOrdenTrabajoExiste(ByVal IdEquipo As String, ByVal IdNroItem As String) As Boolean
    '    'If Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO WHERE cIdComponenteOrdenTrabajo = '" & IdComponenteOrdenTrabajo & "' " &
    '    '                             " AND bEstadoRegistroComponenteOrdenTrabajo = 1", "", "", "", "1", "").Count > 0 Then
    '    If Data.PA_LOGI_MNT_COMPONENTEORDENTRABAJO("SQL_NONE", "SELECT * FROM LOGI_COMPONENTEORDENTRABAJO WHERE cIdEquipo = '" & IdEquipo & "' AND nIdNumeroItemComponenteOrdenTrabajo = " & IdNroItem,
    '                                      "", "", "", "", "", "", "", "", "", 0, "", "", "", "", Now, Now, 0, "").Count > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
End Class
