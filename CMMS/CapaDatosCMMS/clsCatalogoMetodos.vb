Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsCatalogoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function CatalogoGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        'Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
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

    'Public Function CatalogoListarCombo(ByVal IdCatalogo As String) As List(Of LOGI_CATALOGO)
    Public Function CatalogoListarCombo(ByVal IdJerarquia As String, ByVal IdTipoActivo As String, Optional ByVal IdEnlaceCatalogo As String = "", Optional ByVal Estado As String = "*") As List(Of LOGI_CATALOGO)
        Dim Consulta = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT * FROM LOGI_CATALOGO " &
                                                 "WHERE cEstadoVisible=1 and cIdJerarquiaCatalogo = '" & IdJerarquia & "' AND (bEstadoRegistroCatalogo = '" & Estado & "' OR '*' = '" & Estado & "') AND cIdTipoActivo = '" & IdTipoActivo & IIf(IdEnlaceCatalogo = "", "'", "' AND cIdEnlaceCatalogo = '" & IdEnlaceCatalogo & "'"),
                                                 "", "", "", "", "", "", "1", "", "", 0, "", 0, 0, "")
        Dim Coleccion As New List(Of LOGI_CATALOGO)
        For Each Catalogo In Consulta
            Dim Cat As New LOGI_CATALOGO
            Cat.cIdCatalogo = Catalogo.cIdCatalogo
            Cat.vDescripcionCatalogo = Catalogo.vDescripcionCatalogo
            Coleccion.Add(Cat)
        Next
        Return Coleccion
    End Function

    Public Function CatalogoListarDescripcionCombo() As List(Of LOGI_CATALOGO)
        Dim Consulta = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT vDescripcionCatalogo " &
                                                 "FROM LOGI_CATALOGO " &
                                                 "WHERE cIdJerarquiaCatalogo = '1' AND bEstadoRegistroCatalogo = 1 " &
                                                 "GROUP BY vDescripcionCatalogo " &
                                                 "ORDER BY vDescripcionCatalogo ",
                                                 "", "", "", "", "", "", "1", "", "", 0, "", 0, 0, "")
        Dim Coleccion As New List(Of LOGI_CATALOGO)
        For Each Catalogo In Consulta
            Dim Cat As New LOGI_CATALOGO
            Cat.cIdCatalogo = Catalogo.vDescripcionCatalogo
            Cat.vDescripcionCatalogo = Catalogo.vDescripcionCatalogo
            Coleccion.Add(Cat)
        Next
        Return Coleccion
    End Function

    'Public Function CatalogoListarPorId(ByVal IdCatalogo As String) As LOGI_CATALOGO
    Public Function CatalogoListarPorId(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal Jerarquia As String, Optional ByVal Estado As String = "1") As LOGI_CATALOGO
        Dim Consulta = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT * FROM LOGI_CATALOGO WHERE cIdCatalogo = '" & IdCatalogo &
                                                         "' AND (bEstadoRegistroCatalogo = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                         " AND cIdTipoActivo = '" & IdTipoActivo & "'  AND cIdJerarquiaCatalogo = '" & Jerarquia & "'",
                                                         "", "", "", "", "", "", "1", "", "", 0, "", 0, 0, "")
        Dim Coleccion As New LOGI_CATALOGO
        For Each LOGI_CATALOGO In Consulta
            Coleccion.cIdCatalogo = LOGI_CATALOGO.cIdCatalogo
            Coleccion.cIdTipoActivo = LOGI_CATALOGO.cIdTipoActivo
            Coleccion.cIdJerarquiaCatalogo = LOGI_CATALOGO.cIdJerarquiaCatalogo
            Coleccion.cIdSistemaFuncional = LOGI_CATALOGO.cIdSistemaFuncional
            Coleccion.cIdEnlaceCatalogo = LOGI_CATALOGO.cIdEnlaceCatalogo
            Coleccion.vDescripcionCatalogo = LOGI_CATALOGO.vDescripcionCatalogo
            Coleccion.vDescripcionAbreviadaCatalogo = LOGI_CATALOGO.vDescripcionAbreviadaCatalogo
            Coleccion.bEstadoRegistroCatalogo = LOGI_CATALOGO.bEstadoRegistroCatalogo
            'Coleccion.vDimensionesCatalogo = LOGI_CATALOGO.vDimensionesCatalogo
            'Coleccion.vPesoCatalogo = LOGI_CATALOGO.vPesoCatalogo
            'Coleccion.vVoltajeCatalogo = LOGI_CATALOGO.vVoltajeCatalogo
            'Coleccion.bActivacionAutomaticaCatalogo = LOGI_CATALOGO.bActivacionAutomaticaCatalogo
            Coleccion.cIdCuentaContableCatalogo = LOGI_CATALOGO.cIdCuentaContableCatalogo
            Coleccion.cidCuentaContableLeasingCatalogo = LOGI_CATALOGO.cIdCuentaContableLeasingCatalogo
            Coleccion.nVidaUtilCatalogo = LOGI_CATALOGO.nVidaUtilCatalogo
            Coleccion.nPeriodoGarantiaCatalogo = LOGI_CATALOGO.nPeriodoGarantiaCatalogo
            Coleccion.nPeriodoMinimoMantenimientoCatalogo = LOGI_CATALOGO.nPeriodoMinimoMantenimientoCatalogo
        Next
        Return Coleccion
    End Function

    'Public Function CatalogoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Jerarquia As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of VI_LOGI_CATALOGO)
    Public Function CatalogoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Jerarquia As String, ByVal Estado As String) As List(Of VI_LOGI_CATALOGO)
        ''Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT CAT.cIdCatalogo, CAT.cIdTipoActivo, CAT.cIdJerarquiaCatalogo, CAT.cIdSistemaFuncional, CAT.cIdEnlaceCatalogo, " &
        '     "CAT.vDescripcionCatalogo, CAT.vDescripcionAbreviadaCatalogo, CAT.bEstadoRegistroCatalogo, CAT.vDimensionesCatalogo, " &
        '     "CAT.vVoltajeCatalogo, CAT.vPesoCatalogo, CAT.bActivacionAutomaticaCatalogo, CAT.cIdCuentaContableCatalogo, " &
        '     "CAT.cIdCuentaContableLeasingCatalogo, CAT.nVidaUtilCatalogo, TIPACT.vDescripcionTipoActivo, SISFUN.vDescripcionSistemaFuncional " &
        '     "FROM LOGI_CATALOGO AS CAT LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
        '     "     CAT.cIdTipoActivo = TIPACT.cIdTipoActivo " &
        '     "     LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
        '     "     CAT.cIdSistemaFuncional = SISFUN.cIdSistemaFuncional AND " &
        '     "     CAT.cIdCatalogo = SISFUN.cIdCatalogo AND " &
        '     "     CAT.cIdTipoActivo = SISFUN.cIdTipoActivo " &
        '     "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CAT.bEstadoRegistroCatalogo = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                           "      AND CAT.cIdJerarquiaCatalogo = '" & Jerarquia & "'",
        '                                           "", "", "", "", "", "", "", "1", "", "", 0, "", "")
        'Dim Consulta = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT CAT.cIdCatalogo, CAT.cIdTipoActivo, CAT.cIdJerarquiaCatalogo, CAT.cIdSistemaFuncional, CAT.cIdEnlaceCatalogo, " &
        '     "CAT.vDescripcionCatalogo, CAT.vDescripcionAbreviadaCatalogo, CAT.bEstadoRegistroCatalogo, " &
        '     "CAT.cIdCuentaContableCatalogo, CAT.cIdCuentaContableLeasingCatalogo, CAT.nVidaUtilCatalogo, " &
        '     "CAT.nPeriodoGarantiaCatalogo, CAT.nPeriodoMinimoMantenimientoCatalogo, " &
        '     "TIPACT.vDescripcionTipoActivo, SISFUN.vDescripcionSistemaFuncional " &
        '     "FROM LOGI_CATALOGO AS CAT LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
        '     "     CAT.cIdTipoActivo = TIPACT.cIdTipoActivo " &
        '     "     LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
        '     "     CAT.cIdSistemaFuncional = SISFUN.cIdSistemaFuncional AND " &
        '     "     CAT.cIdCatalogo = SISFUN.cIdCatalogo " &
        '     "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CAT.bEstadoRegistroCatalogo = '" & Estado & "' OR '*' = '" & Estado & "') " &
        '                                           "      AND CAT.cIdJerarquiaCatalogo = '" & Jerarquia & "'",
        '                                           "", "", "", "", "", "", "1", "", "", 0, "", 0, 0, "")

        Dim Consulta = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT CAT.cIdCatalogo, CAT.cIdTipoActivo, CAT.cIdJerarquiaCatalogo, CAT.cIdSistemaFuncional, CAT.cIdEnlaceCatalogo, " &
             "CAT.vDescripcionCatalogo, CAT.vDescripcionAbreviadaCatalogo, CAT.bEstadoRegistroCatalogo, " &
             "CAT.cIdCuentaContableCatalogo, CAT.cIdCuentaContableLeasingCatalogo, CAT.nVidaUtilCatalogo, " &
             "CAT.nPeriodoGarantiaCatalogo, CAT.nPeriodoMinimoMantenimientoCatalogo, " &
             "TIPACT.vDescripcionTipoActivo, SISFUN.vDescripcionSistemaFuncional " &
             "FROM LOGI_CATALOGO AS CAT LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON " &
             "     CAT.cIdTipoActivo = TIPACT.cIdTipoActivo " &
             "     LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON " &
             "     CAT.cIdSistemaFuncional = SISFUN.cIdSistemaFuncional " &
             "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (CAT.bEstadoRegistroCatalogo = '" & Estado & "' OR '*' = '" & Estado & "') " &
                                                   "      AND CAT.cIdJerarquiaCatalogo = '" & Jerarquia & "'",
                                                   "", "", "", "", "", "", "1", "", "", 0, "", 0, 0, "")

        Dim Coleccion As New List(Of VI_LOGI_CATALOGO)
        For Each Busqueda In Consulta
            Dim BuscarCat As New VI_LOGI_CATALOGO
            BuscarCat.Codigo = Busqueda.cIdCatalogo
            BuscarCat.Descripcion = Busqueda.vDescripcionCatalogo
            BuscarCat.Estado = Busqueda.bEstadoRegistroCatalogo
            BuscarCat.IdTipoActivo = Busqueda.cIdTipoActivo
            BuscarCat.IdSistemaFuncional = Busqueda.cIdSistemaFuncional
            BuscarCat.DescripcionAbreviada = Busqueda.vDescripcionAbreviadaCatalogo
            'BuscarCat.ActivacionAutomatica = Busqueda.bActivacionAutomaticaCatalogo
            BuscarCat.IdCuentaContable = Busqueda.cIdCuentaContableCatalogo
            BuscarCat.IdCuentaContableLeasing = Busqueda.cIdCuentaContableLeasingCatalogo
            BuscarCat.DescripcionSistemaFuncional = Busqueda.vDescripcionSistemaFuncional
            BuscarCat.DescripcionTipoActivo = Busqueda.vDescripcionTipoActivo
            BuscarCat.PeriodoGarantia = Busqueda.nPeriodoGarantiaCatalogo
            BuscarCat.PeriodoMinimoMantenimiento = Busqueda.nPeriodoMinimoMantenimientoCatalogo
            BuscarCat.PeriodoMinimoMantenimiento = Busqueda.nPeriodoMinimoMantenimientoCatalogo
            Coleccion.Add(BuscarCat)
        Next
        Return Coleccion
    End Function

    Public Function CatalogoInsertaDetalle(ByVal DetalleCatalogo As List(Of LOGI_CATALOGO), Optional ByVal strNroEnlaceCatalogo As String = "") As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Dim dtCatalogoComponente
        dtCatalogoComponente = CatalogoGetData("SELECT cIdCatalogo FROM LOGI_CATALOGO WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "'")
        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            For Each Busqueda In DetalleCatalogo
                Dim bExiste As Boolean
                bExiste = False
                For Each row In dtCatalogoComponente.Rows
                    If row("cIdCatalogo").ToString.Trim = Busqueda.cIdCatalogo Then
                        bExiste = True
                        Exit For
                    End If
                Next

                If bExiste = False Then
                    x = Data.PA_LOGI_MNT_CATALOGO("SQL_INSERT", "", Busqueda.cIdCatalogo, Busqueda.cIdJerarquiaCatalogo,
                                                  Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
                                                  Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
                                                  Busqueda.cIdCuentaContableCatalogo, Busqueda.cIdCuentaContableLeasingCatalogo,
                                                  Busqueda.nVidaUtilCatalogo, Busqueda.cIdTipoActivo,
                                                  Busqueda.nPeriodoGarantiaCatalogo, Busqueda.nPeriodoMinimoMantenimientoCatalogo,
                                                  Busqueda.cIdCatalogo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "UPDATE LOGI_EQUIPO SET vDescripcionEquipo = '" & Busqueda.vDescripcionCatalogo & "', vDescripcionAbreviadaEquipo = '" & Busqueda.vDescripcionAbreviadaCatalogo & "' " &
                                                  "WHERE cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "' and cIdCatalogo = '" & Busqueda.cIdCatalogo & "'", Busqueda.cIdCatalogo, Busqueda.cIdJerarquiaCatalogo,
                              Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
                              Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
                              Busqueda.cIdCuentaContableCatalogo, Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo, Busqueda.cIdTipoActivo,
                               Busqueda.nPeriodoGarantiaCatalogo, Busqueda.nPeriodoMinimoMantenimientoCatalogo,
                              Busqueda.cIdCatalogo).ReturnValue.ToString

                    x = Data.PA_LOGI_MNT_CATALOGO("SQL_UPDATE", "", Busqueda.cIdCatalogo, Busqueda.cIdJerarquiaCatalogo,
                                                  Busqueda.cIdSistemaFuncional, strNroEnlaceCatalogo, Busqueda.vDescripcionCatalogo,
                                                  Busqueda.vDescripcionAbreviadaCatalogo, Busqueda.bEstadoRegistroCatalogo,
                                                  Busqueda.cIdCuentaContableCatalogo, Busqueda.cIdCuentaContableLeasingCatalogo, Busqueda.nVidaUtilCatalogo,
                                                  Busqueda.cIdTipoActivo, Busqueda.nPeriodoGarantiaCatalogo, Busqueda.nPeriodoMinimoMantenimientoCatalogo,
                                                  Busqueda.cIdCatalogo).ReturnValue.ToString
                End If
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function CatalogoInserta(ByVal Catalogo As LOGI_CATALOGO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CATALOGO("SQL_INSERT", "", Catalogo.cIdCatalogo, Catalogo.cIdJerarquiaCatalogo, Catalogo.cIdSistemaFuncional,
                                      Catalogo.cIdEnlaceCatalogo, Catalogo.vDescripcionCatalogo, Catalogo.vDescripcionAbreviadaCatalogo,
                                      Catalogo.bEstadoRegistroCatalogo, Catalogo.cIdCuentaContableCatalogo, Catalogo.cIdCuentaContableLeasingCatalogo,
                                      Catalogo.nVidaUtilCatalogo, Catalogo.cIdTipoActivo, Catalogo.nPeriodoGarantiaCatalogo, Catalogo.nPeriodoMinimoMantenimientoCatalogo,
                                      Catalogo.cIdCatalogo).ReturnValue.ToString
        Return x
    End Function

    Public Function CatalogoEdita(ByVal Catalogo As LOGI_CATALOGO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CATALOGO("SQL_UPDATE", "", Catalogo.cIdCatalogo, Catalogo.cIdJerarquiaCatalogo, Catalogo.cIdSistemaFuncional,
                                      Catalogo.cIdEnlaceCatalogo, Catalogo.vDescripcionCatalogo, Catalogo.vDescripcionAbreviadaCatalogo,
                                      Catalogo.bEstadoRegistroCatalogo, Catalogo.cIdCuentaContableCatalogo, Catalogo.cIdCuentaContableLeasingCatalogo,
                                      Catalogo.nVidaUtilCatalogo, Catalogo.cIdTipoActivo, Catalogo.nPeriodoGarantiaCatalogo, Catalogo.nPeriodoMinimoMantenimientoCatalogo,
                                      Catalogo.cIdCatalogo).ReturnValue.ToString
        Return x
    End Function

    Public Function CatalogoElimina(ByVal Catalogo As LOGI_CATALOGO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' ",
                                      "", "", "", "", "", "", "1", "", "", 0, "", 0, 0, "").ReturnValue.ToString
        Return x
    End Function

    'Public Function CatalogoExiste(ByVal IdCatalogo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
    '    Public Function CatalogoListarPorId(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal Jerarquia As String, Optional ByVal Estado As String = "1") As LOGI_CATALOGO

    Public Function CatalogoExiste(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal Jerarquia As String) As Boolean
        'If Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT * FROM LOGI_CATALOGO WHERE cIdCatalogo = '" & IdCatalogo & "' " & _
        '                             " AND bEstadoRegistroCatalogo = 1", "", "", "", "", "", "", "", "1", "", "", "", "0", "", "", 0, "").Count > 0 Then
        If Data.PA_LOGI_MNT_CATALOGO("SQL_NONE", "SELECT * FROM LOGI_CATALOGO WHERE cIdCatalogo = '" & IdCatalogo & "' AND ISNULL(cIdTipoActivo, '') = '" & Trim(IdTipoActivo) & "' AND cIdJerarquiaCatalogo = '" & Jerarquia & "' ",
                                     "", "", "", "", "", "", "1", "", "", 0, "", 0, 0, "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class