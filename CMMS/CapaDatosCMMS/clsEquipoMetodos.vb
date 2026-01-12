Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsEquipoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function EquipoGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        Dim constr As String = My.Settings.CMMSConnectionString
        Dim contenido As Integer

        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strQuery)
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    ''cmd.ExecuteNonQuery()
                    ''sda.ContinueUpdateOnError = True
                    contenido = sda.Fill(dt)
                End Using
            End Using
            Return dt
        End Using

    End Function

    'Public Function EquipoListarCombo(ByVal IdEquipo As String) As List(Of LOGI_EQUIPO)
    'Public Function EquipoListarCombo(ByVal IdJerarquia As String) As List(Of LOGI_EQUIPO)
    Public Function EquipoListarCombo(ByVal IdTipoActivo As String, ByVal IdCatalogo As String, ByVal IdJerarquia As String) As List(Of LOGI_EQUIPO)
        Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO " &
                                                 "WHERE cIdJerarquiaCatalogo = '" & IdJerarquia & "' " &
                                                 "      AND cIdTipoActivo = '" & IdTipoActivo & "' " &
                                                 "      AND cIdEnlaceCatalogo = '" & IdCatalogo & "'",
                                                 "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "1", "", "", "", "", "", "", "", "")
        '"", "", "", "", "", "", Now, "", "", "", "1", "", "0", Now, "", "0", "0", 0, "", "", "", "", "", "", "", "", "", Now, "", "", Now, 0, "", "", "", "", "", "", 0, 0, 0, 0, "", "", "", "")
        Dim Coleccion As New List(Of LOGI_EQUIPO)
        For Each Equipo In Consulta
            Dim Cat As New LOGI_EQUIPO
            Cat.cIdEquipo = Equipo.cIdEquipo
            Cat.vDescripcionEquipo = Equipo.cIdTipoActivo.Trim + Equipo.cIdCatalogo.Trim + Equipo.cIdEquipo.Trim + "-" + Equipo.vDescripcionEquipo
            Coleccion.Add(Cat)
        Next
        Return Coleccion
    End Function

    'Public Function EquipoListarPorIdDetalle(ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String) As LOGI_EQUIPO
    Public Function EquipoListarPorIdDetalle(ByVal IdEquipo As String, ByVal IdCatalogo As String) As LOGI_EQUIPO
        'Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO WHERE cIdEquipo = '" & IdEquipo & "' " &
        '                                       IIf(IdCatalogo = "", "", " AND cIdCatalogo = '" & IdCatalogo & "'") & IIf(IdTipoActivo = "", "", " AND cIdTipoActivo = '" & IdTipoActivo & "'") & " AND bEstadoRegistroEquipo = 1",
        '                                         "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", "")
        Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO WHERE cIdEquipo = '" & IdEquipo & "' " &
                                               IIf(IdCatalogo = "", "", " AND cIdCatalogo = '" & IdCatalogo & "'") & " AND bEstadoRegistroEquipo = 1 " &
                                               "ORDER BY cIdCatalogo",
                                               "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "1", "", "", "", "", "", "", "", "")
        Dim Coleccion As New LOGI_EQUIPO
        For Each LOGI_EQUIPO In Consulta
            Coleccion.cIdTipoActivo = LOGI_EQUIPO.cIdTipoActivo
            Coleccion.cIdCatalogo = LOGI_EQUIPO.cIdCatalogo
            Coleccion.cIdJerarquiaCatalogo = LOGI_EQUIPO.cIdJerarquiaCatalogo
            Coleccion.cIdEquipo = LOGI_EQUIPO.cIdEquipo
            Coleccion.cIdEmpresa = LOGI_EQUIPO.cIdEmpresa
            Coleccion.vDescripcionEquipo = LOGI_EQUIPO.vDescripcionEquipo
            Coleccion.vDescripcionAbreviadaEquipo = LOGI_EQUIPO.vDescripcionAbreviadaEquipo
            Coleccion.dFechaTransaccionEquipo = LOGI_EQUIPO.dFechaTransaccionEquipo
            Coleccion.cIdEnlaceEquipo = LOGI_EQUIPO.cIdEnlaceEquipo
            Coleccion.cIdEnlaceCatalogo = LOGI_EQUIPO.cIdEnlaceCatalogo
            Coleccion.bEstadoRegistroEquipo = LOGI_EQUIPO.bEstadoRegistroEquipo
            Coleccion.cIdSistemaFuncionalEquipo = LOGI_EQUIPO.cIdSistemaFuncionalEquipo
            Coleccion.vObservacionEquipo = LOGI_EQUIPO.vObservacionEquipo
            Coleccion.cIdEstadoComponenteEquipo = LOGI_EQUIPO.cIdEstadoComponenteEquipo
            Coleccion.vIdEquivalenciaEquipo = LOGI_EQUIPO.vIdEquivalenciaEquipo
            Coleccion.cIdPaisOrigenEquipo = LOGI_EQUIPO.cIdPaisOrigenEquipo
            Coleccion.nVidaUtilEquipo = LOGI_EQUIPO.nVidaUtilEquipo
            Coleccion.nPeriodoGarantiaEquipo = IIf(IsNothing(LOGI_EQUIPO.nPeriodoGarantiaEquipo), 0, LOGI_EQUIPO.nPeriodoGarantiaEquipo)
            Coleccion.nPeriodoMinimoMantenimientoEquipo = IIf(IsNothing(LOGI_EQUIPO.nPeriodoMinimoMantenimientoEquipo), 0, LOGI_EQUIPO.nPeriodoMinimoMantenimientoEquipo)
            Coleccion.vNumeroSerieEquipo = LOGI_EQUIPO.vNumeroSerieEquipo
            Coleccion.vNumeroParteEquipo = LOGI_EQUIPO.vNumeroParteEquipo
            Coleccion.cIdCliente = LOGI_EQUIPO.cIdCliente
            Coleccion.vIdClienteSAPEquipo = LOGI_EQUIPO.vIdClienteSAPEquipo
            Coleccion.cIdClienteUbicacion = LOGI_EQUIPO.cIdClienteUbicacion
            Coleccion.cIdEquipoSAPEquipo = LOGI_EQUIPO.cIdEquipoSAPEquipo
            Coleccion.dFechaCreacionEquipo = LOGI_EQUIPO.dFechaCreacionEquipo
            Coleccion.dFechaUltimaModificacionEquipo = LOGI_EQUIPO.dFechaUltimaModificacionEquipo
            Coleccion.cIdUsuarioCreacionEquipo = LOGI_EQUIPO.cIdUsuarioCreacionEquipo
            Coleccion.cIdUsuarioUltimaModificacionEquipo = LOGI_EQUIPO.cIdUsuarioUltimaModificacionEquipo
            Coleccion.dFechaRegistroTarjetaSAPEquipo = LOGI_EQUIPO.dFechaRegistroTarjetaSAPEquipo
            Coleccion.dFechaManufacturaTarjetaSAPEquipo = LOGI_EQUIPO.dFechaManufacturaTarjetaSAPEquipo
            Coleccion.cEstadoEquipo = LOGI_EQUIPO.cEstadoEquipo
            Coleccion.bTieneContratoEquipo = IIf(IsNothing(LOGI_EQUIPO.bTieneContratoEquipo), False, LOGI_EQUIPO.bTieneContratoEquipo)
            Coleccion.vContratoReferenciaActualEquipo = LOGI_EQUIPO.vContratoReferenciaActualEquipo
            Coleccion.vDescripcionEquipoSAP = LOGI_EQUIPO.vDescripcionEquipoSAP
            Coleccion.cIdTipoEquipo = LOGI_EQUIPO.cIdTipoEquipo
            Coleccion.vTagEquipo = LOGI_EQUIPO.vTagEquipo
            Coleccion.vCapacidadEquipo = LOGI_EQUIPO.vCapacidadEquipo
            Coleccion.vAreaEquipo = LOGI_EQUIPO.vAreaEquipo
            Coleccion.vIdArticuloSAPEquipo = LOGI_EQUIPO.vIdArticuloSAPEquipo
        Next
        Return Coleccion
    End Function

    'Public Function EquipoListarPorId(ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String) As LOGI_EQUIPO
    Public Function EquipoListarPorId(ByVal IdEquipo As String) As LOGI_EQUIPO
        'Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO WHERE cIdEquipo = '" & IdEquipo &
        '                                         "' AND cIdJerarquiaCatalogo = '0' AND bEstadoRegistroEquipo = 1 " &
        '                                         "AND cIdCatalogo = '" & IdCatalogo & "' AND cIdTipoActivo = '" & IdTipoActivo & "' ",
        '                                         "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", "")
        Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO WHERE cIdEquipo = '" & IdEquipo &
                                                 "' AND cIdJerarquiaCatalogo = '0' AND bEstadoRegistroEquipo = 1 " &
                                                 "",
                                                 "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "1", "", "", "", "", "", "", "", "")
        Dim Coleccion As New LOGI_EQUIPO
        For Each LOGI_EQUIPO In Consulta
            Coleccion.cIdTipoActivo = LOGI_EQUIPO.cIdTipoActivo
            Coleccion.cIdCatalogo = LOGI_EQUIPO.cIdCatalogo
            Coleccion.cIdJerarquiaCatalogo = LOGI_EQUIPO.cIdJerarquiaCatalogo
            Coleccion.cIdEquipo = LOGI_EQUIPO.cIdEquipo
            Coleccion.cIdEmpresa = LOGI_EQUIPO.cIdEmpresa
            Coleccion.vDescripcionEquipo = LOGI_EQUIPO.vDescripcionEquipo
            Coleccion.vDescripcionAbreviadaEquipo = LOGI_EQUIPO.vDescripcionAbreviadaEquipo
            Coleccion.dFechaTransaccionEquipo = LOGI_EQUIPO.dFechaTransaccionEquipo
            Coleccion.cIdEnlaceEquipo = LOGI_EQUIPO.cIdEnlaceEquipo
            Coleccion.cIdEnlaceCatalogo = LOGI_EQUIPO.cIdEnlaceCatalogo
            Coleccion.bEstadoRegistroEquipo = LOGI_EQUIPO.bEstadoRegistroEquipo
            Coleccion.cIdSistemaFuncionalEquipo = LOGI_EQUIPO.cIdSistemaFuncionalEquipo
            Coleccion.vObservacionEquipo = LOGI_EQUIPO.vObservacionEquipo
            Coleccion.cIdEstadoComponenteEquipo = LOGI_EQUIPO.cIdEstadoComponenteEquipo
            Coleccion.vIdEquivalenciaEquipo = LOGI_EQUIPO.vIdEquivalenciaEquipo
            Coleccion.cIdPaisOrigenEquipo = LOGI_EQUIPO.cIdPaisOrigenEquipo
            Coleccion.nVidaUtilEquipo = LOGI_EQUIPO.nVidaUtilEquipo
            Coleccion.nPeriodoGarantiaEquipo = LOGI_EQUIPO.nPeriodoGarantiaEquipo
            Coleccion.nPeriodoMinimoMantenimientoEquipo = LOGI_EQUIPO.nPeriodoMinimoMantenimientoEquipo
            Coleccion.vNumeroSerieEquipo = LOGI_EQUIPO.vNumeroSerieEquipo
            Coleccion.vNumeroParteEquipo = LOGI_EQUIPO.vNumeroParteEquipo
            Coleccion.cIdCliente = LOGI_EQUIPO.cIdCliente
            Coleccion.vIdClienteSAPEquipo = LOGI_EQUIPO.vIdClienteSAPEquipo
            Coleccion.cIdClienteUbicacion = LOGI_EQUIPO.cIdClienteUbicacion
            Coleccion.cIdEquipoSAPEquipo = LOGI_EQUIPO.cIdEquipoSAPEquipo
            Coleccion.dFechaCreacionEquipo = LOGI_EQUIPO.dFechaCreacionEquipo
            Coleccion.dFechaUltimaModificacionEquipo = LOGI_EQUIPO.dFechaUltimaModificacionEquipo
            Coleccion.cIdUsuarioCreacionEquipo = LOGI_EQUIPO.cIdUsuarioCreacionEquipo
            Coleccion.cIdUsuarioUltimaModificacionEquipo = LOGI_EQUIPO.cIdUsuarioUltimaModificacionEquipo
            Coleccion.dFechaRegistroTarjetaSAPEquipo = LOGI_EQUIPO.dFechaRegistroTarjetaSAPEquipo
            Coleccion.dFechaManufacturaTarjetaSAPEquipo = LOGI_EQUIPO.dFechaManufacturaTarjetaSAPEquipo
            Coleccion.cEstadoEquipo = LOGI_EQUIPO.cEstadoEquipo
            Coleccion.bTieneContratoEquipo = LOGI_EQUIPO.bTieneContratoEquipo
            Coleccion.vContratoReferenciaActualEquipo = LOGI_EQUIPO.vContratoReferenciaActualEquipo
            Coleccion.vDescripcionEquipoSAP = LOGI_EQUIPO.vDescripcionEquipoSAP
            Coleccion.cIdTipoEquipo = LOGI_EQUIPO.cIdTipoEquipo
            Coleccion.vTagEquipo = LOGI_EQUIPO.vTagEquipo
            Coleccion.vCapacidadEquipo = LOGI_EQUIPO.vCapacidadEquipo
            Coleccion.vAreaEquipo = LOGI_EQUIPO.vAreaEquipo
            Coleccion.vIdArticuloSAPEquipo = LOGI_EQUIPO.vIdArticuloSAPEquipo
        Next
        Return Coleccion
    End Function

    Public Function EquipoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Jerarquia As String) As List(Of VI_LOGI_EQUIPO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim query = "SELECT EQU.vDescripcionEquipo, EQU.vDescripcionEquipoSAP, EQU.bEstadoRegistroEquipo, EQU.cIdEnlaceEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaEquipo, EQU.cIdSistemaFuncionalEquipo, EQU.cIdEnlaceCatalogo, EQU.vObservacionEquipo, EQU.dFechaTransaccionEquipo, EQU.nVidaUtilEquipo, EQU.vIdEquivalenciaEquipo, " &
                                                   "EQU.vNumeroSerieEquipo, EQU.dFechaRegistroTarjetaSAPEquipo, EQU.dFechaUltimaModificacionEquipo, CLI.cIdCliente, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoEquipo, 'R') AS cEstadoEquipo " &
                                                   "FROM LOGI_EQUIPO AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroEquipo = 1 " &
                                                   "      AND EQU.cIdJerarquiaCatalogo = '" & Jerarquia & "' " &
                                                   "ORDER BY EQU.cIdEquipo DESC, EQU.dFechaRegistroTarjetaSAPEquipo DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalEquipo"

        Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT EQU.vDescripcionEquipo, EQU.vDescripcionEquipoSAP, EQU.bEstadoRegistroEquipo, EQU.cIdEnlaceEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaEquipo, EQU.cIdSistemaFuncionalEquipo, EQU.cIdEnlaceCatalogo, EQU.vObservacionEquipo, EQU.dFechaTransaccionEquipo, EQU.nVidaUtilEquipo, EQU.vIdEquivalenciaEquipo, " &
                                                   "EQU.vNumeroSerieEquipo, EQU.dFechaRegistroTarjetaSAPEquipo, EQU.dFechaUltimaModificacionEquipo, CLI.cIdCliente, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoEquipo, 'R') AS cEstadoEquipo " &
                                                   "FROM LOGI_EQUIPO AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroEquipo = 1 " &
                                                   "      AND EQU.cIdJerarquiaCatalogo = '" & Jerarquia & "' " &
                                                   "ORDER BY EQU.cIdEquipo DESC, EQU.dFechaRegistroTarjetaSAPEquipo DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalEquipo",
                                                   "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "1", "", "", "", "", "", "", "", "")
        Dim Coleccion As New List(Of VI_LOGI_EQUIPO)
        For Each Busqueda In Consulta
            Dim BuscarMaeAct As New VI_LOGI_EQUIPO
            'LO QUITE PORQUE NECESITAMOS EL CODIGO DE CATALOGO
            BuscarMaeAct.Codigo = Busqueda.cIdEquipo
            'BuscarMaeAct.Codigo = Busqueda.cIdCatalogo
            BuscarMaeAct.Descripcion = Busqueda.vDescripcionEquipo
            BuscarMaeAct.Estado = Busqueda.bEstadoRegistroEquipo
            BuscarMaeAct.IdTipoActivo = Busqueda.cIdTipoActivo
            BuscarMaeAct.IdSistemaFuncional = Busqueda.cIdSistemaFuncionalEquipo
            BuscarMaeAct.DescripcionAbreviada = Busqueda.vDescripcionAbreviadaEquipo
            BuscarMaeAct.IdCatalogo = Busqueda.cIdCatalogo
            'BuscarMaeAct.IdCuentaContable = Busqueda.cIdCuentaContableEquipo
            'BuscarMaeAct.FechaActivacion = Busqueda.dFechaActivacionEquipo
            'BuscarMaeAct.EstadoLeasing = Busqueda.bEstadoLeasingEquipo
            BuscarMaeAct.FechaTransaccion = Busqueda.dFechaTransaccionEquipo
            BuscarMaeAct.IdJerarquiaCatalogo = Busqueda.cIdJerarquiaCatalogo
            BuscarMaeAct.FechaRegistroTarjetaSAP = Busqueda.dFechaRegistroTarjetaSAPEquipo
            BuscarMaeAct.IdCliente = Busqueda.cIdCliente
            BuscarMaeAct.IdClienteSAPCliente = Busqueda.vIdClienteSAPEquipo
            BuscarMaeAct.NumeroSerieEquipo = Busqueda.vNumeroSerieEquipo
            BuscarMaeAct.RazonSocialCliente = Busqueda.vRazonSocialCliente
            BuscarMaeAct.RucCliente = Busqueda.vRucCliente
            BuscarMaeAct.FechaUltimaModificacion = Busqueda.dFechaUltimaModificacionEquipo
            BuscarMaeAct.StatusEquipo = Busqueda.cEstadoEquipo
            BuscarMaeAct.DescripcionEquipoSAP = Busqueda.vDescripcionEquipoSAP
            'BuscarMaeAct.TotalSegundosTrabajados = 0 'Busqueda.cEstadoEquipo
            Coleccion.Add(BuscarMaeAct)
        Next
        Return Coleccion
    End Function

    Public Function EquipoListaGridV2(ByVal Filtro As String, ByVal Buscar As String, ByVal Jerarquia As String, ByVal IdContrato As String) As List(Of VI_LOGI_EQUIPO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim query = "SELECT EQU.vDescripcionEquipo, EQU.vDescripcionEquipoSAP, EQU.bEstadoRegistroEquipo, EQU.cIdEnlaceEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaEquipo, EQU.cIdSistemaFuncionalEquipo, EQU.cIdEnlaceCatalogo, EQU.vObservacionEquipo, EQU.dFechaTransaccionEquipo, EQU.nVidaUtilEquipo, EQU.vIdEquivalenciaEquipo, " &
                                                   "EQU.vNumeroSerieEquipo, EQU.dFechaRegistroTarjetaSAPEquipo, EQU.dFechaUltimaModificacionEquipo, CLI.cIdCliente, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoEquipo, 'R') AS cEstadoEquipo " &
                                                   "FROM LOGI_EQUIPO AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "     JOIN LOGI_DETALLECONTRATO AS DETCON ON  EQU.cIdEquipo =  DETCON.cIdEquipoDetalleContrato " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroEquipo = 1 " &
                                                   "      AND EQU.cIdJerarquiaCatalogo = '" & Jerarquia & "' " &
                                                   "      AND  CONCAT(cIdTipoDocumentoCabeceraContrato, '-', vIdNumeroSerieCabeceraContrato, '-', vIdNumeroCorrelativoCabeceraContrato) = '" & IdContrato & "' " &
                                                   "ORDER BY EQU.cIdEquipo DESC, EQU.dFechaRegistroTarjetaSAPEquipo DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalEquipo"

        Dim Consulta = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT EQU.vDescripcionEquipo, EQU.vDescripcionEquipoSAP, EQU.bEstadoRegistroEquipo, EQU.cIdEnlaceEquipo, EQU.cIdJerarquiaCatalogo, EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdTipoActivo, EQU.vDescripcionAbreviadaEquipo, EQU.cIdSistemaFuncionalEquipo, EQU.cIdEnlaceCatalogo, EQU.vObservacionEquipo, EQU.dFechaTransaccionEquipo, EQU.nVidaUtilEquipo, EQU.vIdEquivalenciaEquipo, " &
                                                   "EQU.vNumeroSerieEquipo, EQU.dFechaRegistroTarjetaSAPEquipo, EQU.dFechaUltimaModificacionEquipo, CLI.cIdCliente, CLI.vRazonSocialCliente, CLI.vRucCliente, ISNULL(EQU.cEstadoEquipo, 'R') AS cEstadoEquipo " &
                                                   "FROM LOGI_EQUIPO AS EQU LEFT JOIN GNRL_CLIENTE AS CLI ON " &
                                                   "     EQU.cIdCliente = CLI.cIdCliente AND EQU.cIdEmpresa = CLI.cIdEmpresa " &
                                                   "     JOIN LOGI_DETALLECONTRATO AS DETCON ON  EQU.cIdEquipo =  DETCON.cIdEquipoDetalleContrato " &
                                                   "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND EQU.bEstadoRegistroEquipo = 1 " &
                                                   "      AND EQU.cIdJerarquiaCatalogo = '" & Jerarquia & "' " &
                                                   "      AND  CONCAT(cIdTipoDocumentoCabeceraContrato, '-', vIdNumeroSerieCabeceraContrato, '-', vIdNumeroCorrelativoCabeceraContrato) = '" & IdContrato & "' " &
                                                   "ORDER BY EQU.cIdEquipo DESC, EQU.dFechaRegistroTarjetaSAPEquipo DESC, CASE WHEN LTRIM(EQU.cIdEnlaceCatalogo) = '' THEN EQU.cIdCatalogo ELSE EQU.cIdEnlaceCatalogo END, EQU.cIdJerarquiaCatalogo, EQU.cIdSistemaFuncionalEquipo",
                                                   "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "1", "", "", "", "", "", "", "", "")
        Dim Coleccion As New List(Of VI_LOGI_EQUIPO)
        For Each Busqueda In Consulta
            Dim BuscarMaeAct As New VI_LOGI_EQUIPO
            'LO QUITE PORQUE NECESITAMOS EL CODIGO DE CATALOGO
            BuscarMaeAct.Codigo = Busqueda.cIdEquipo
            'BuscarMaeAct.Codigo = Busqueda.cIdCatalogo
            BuscarMaeAct.Descripcion = Busqueda.vDescripcionEquipo
            BuscarMaeAct.Estado = Busqueda.bEstadoRegistroEquipo
            BuscarMaeAct.IdTipoActivo = Busqueda.cIdTipoActivo
            BuscarMaeAct.IdSistemaFuncional = Busqueda.cIdSistemaFuncionalEquipo
            BuscarMaeAct.DescripcionAbreviada = Busqueda.vDescripcionAbreviadaEquipo
            BuscarMaeAct.IdCatalogo = Busqueda.cIdCatalogo
            'BuscarMaeAct.IdCuentaContable = Busqueda.cIdCuentaContableEquipo
            'BuscarMaeAct.FechaActivacion = Busqueda.dFechaActivacionEquipo
            'BuscarMaeAct.EstadoLeasing = Busqueda.bEstadoLeasingEquipo
            BuscarMaeAct.FechaTransaccion = Busqueda.dFechaTransaccionEquipo
            BuscarMaeAct.IdJerarquiaCatalogo = Busqueda.cIdJerarquiaCatalogo
            BuscarMaeAct.FechaRegistroTarjetaSAP = Busqueda.dFechaRegistroTarjetaSAPEquipo
            BuscarMaeAct.IdCliente = Busqueda.cIdCliente
            BuscarMaeAct.IdClienteSAPCliente = Busqueda.vIdClienteSAPEquipo
            BuscarMaeAct.NumeroSerieEquipo = Busqueda.vNumeroSerieEquipo
            BuscarMaeAct.RazonSocialCliente = Busqueda.vRazonSocialCliente
            BuscarMaeAct.RucCliente = Busqueda.vRucCliente
            BuscarMaeAct.FechaUltimaModificacion = Busqueda.dFechaUltimaModificacionEquipo
            BuscarMaeAct.StatusEquipo = Busqueda.cEstadoEquipo
            BuscarMaeAct.DescripcionEquipoSAP = Busqueda.vDescripcionEquipoSAP
            'BuscarMaeAct.TotalSegundosTrabajados = 0 'Busqueda.cEstadoEquipo
            Coleccion.Add(BuscarMaeAct)
        Next
        Return Coleccion
    End Function

    Public Function EquipoInsertaDetalle(ByVal DetalleEquipo As List(Of LOGI_EQUIPO), Optional ByVal strNroEnlaceEquipo As String = "", Optional ByVal strNroEnlaceCatalogo As String = "") As Int32
        Dim x
        'x = Data.PA_LOGI_MNT_Equipo("SQL_NONE", "DELETE LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & strNroEnlaceEquipo & "' " & IIf(strNroEnlaceCatalogo = "", "", "AND cIdCatalogo = '" & strNroEnlaceCatalogo & "'"), 
        'x = Data.PA_LOGI_MNT_Equipo("SQL_NONE", "DELETE LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & strNroEnlaceEquipo & "' " & IIf(strNroEnlaceCatalogo = "", "AND cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "'", "AND cIdCatalogo = '" & strNroEnlaceCatalogo & "'"), _

        'Recien lo quite
        'If strNroEnlaceEquipo <> "" And strNroEnlaceCatalogo <> "" Then 'Para movimientos esta en blanco.
        '    x = Data.PA_LOGI_MNT_Equipo("SQL_NONE", "DELETE LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & strNroEnlaceEquipo & "' AND cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "'", _
        '                                       "", "", "", "", "", "", 0, "", "", "", "1", "", "0", Now, "", "").ReturnValue.ToString
        'End If
        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        ''Dim FeriadoNeg As New clsFeriadoNegocios
        'Dim dtFeriado = EquipoGetData("SELECT CONVERT (NUMERIC, ISNULL(MAX(cIdJerarquiaEquipo), 1)) AS " & _
        '                                "FROM LOGI_EQUIPO ")

        '                                "FROM perFERIADO " & _
        '                                "WHERE cIdMesFeriado = '" & String.Format("{0:00}", CDate(txtFechaInicialDetalleTareo.Text).Month) & "' " & _
        '                                "AND cIdDiaFeriado = '" & String.Format("{0:00}", CDate(txtFechaInicialDetalleTareo.Text).Day) & "' " & _
        '                                "AND bEstadoRegistroFeriado = 1")
        'If dtFeriado.Rows.Count > 0 Then
        '    lblDiaFeriado.Text = "Día Feriado: " & dtFeriado.Rows(0).Item(0).ToString.Trim
        'End If


        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            'Dim booExisteDato As Boolean = False
            'Dim dtEquipoEliminar = EquipoGetData("SELECT cIdTipoActivo, cIdCatalogo, cIdEquipo FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & strNroEnlaceEquipo & "' AND cIdEnlaceCatalogo = '" & strNroEnlaceCatalogo & "' AND cIdJerarquiaCatalogo = '1'")
            'For i = 0 To dtEquipoEliminar.Rows.Count - 1
            '    For Each Busqueda In DetalleEquipo
            '        If dtEquipoEliminar.Rows(i).Item(0).ToString.Trim = Busqueda.cIdTipoActivo And
            '                   dtEquipoEliminar.Rows(i).Item(1).ToString.Trim = Busqueda.cIdCatalogo And
            '                   dtEquipoEliminar.Rows(i).Item(2).ToString.Trim = Busqueda.cIdEquipo Then
            '            booExisteDato = True
            '        End If
            '    Next
            '    If booExisteDato = False Then
            '        x = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "DELETE LOGI_EQUIPO WHERE cIdEquipo = '" & dtEquipoEliminar.Rows(i).Item(2).ToString.Trim & "' " &
            '                                            "AND cIdCatalogo = '" & dtEquipoEliminar.Rows(i).Item(1).ToString.Trim & "' AND cIdTipoActivo = '" & dtEquipoEliminar.Rows(i).Item(0).ToString.Trim & "'",
            '                                            "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", "").ReturnValue.ToString
            '    End If
            '    booExisteDato = False
            'Next

            For Each Busqueda In DetalleEquipo
                'If Busqueda.cIdEquipo = "" Then
                '    x = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "DELETE LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & strNroEnlaceEquipo & "' " &
                '                                        "AND cIdCatalogo = '" & Busqueda.cIdCatalogo & "' AND cIdTipoActivo = '" & Busqueda.cIdTipoActivo & "'",
                '                                        "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", "").ReturnValue.ToString
                'End If
                If Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO WHERE cIdEquipo = '" & Busqueda.cIdEquipo & "' AND cIdCatalogo = '" & Busqueda.cIdCatalogo & "' AND cIdJerarquiaCatalogo = '1'",
                             "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "0", "", "", "", "", "", "", "", "").Count > 0 Then
                    x = Data.PA_LOGI_MNT_EQUIPO("SQL_UPDATE", "", Busqueda.cIdEquipo, Busqueda.cIdCatalogo,
                                           Busqueda.cIdTipoActivo, Busqueda.vDescripcionEquipo,
                                           Busqueda.vDescripcionAbreviadaEquipo, Busqueda.cIdJerarquiaCatalogo,
                                           Busqueda.dFechaTransaccionEquipo, Busqueda.cIdEnlaceEquipo,
                                           Busqueda.cIdEnlaceCatalogo, Busqueda.cIdSistemaFuncionalEquipo, Busqueda.bEstadoRegistroEquipo,
                                           Busqueda.vObservacionEquipo, Busqueda.nVidaUtilEquipo, Busqueda.cIdEmpresa, Busqueda.cIdEstadoComponenteEquipo,
                                           Busqueda.vIdEquivalenciaEquipo, Busqueda.cIdPaisOrigenEquipo, Busqueda.vIdClienteSAPEquipo,
                                           Busqueda.cIdEquipoSAPEquipo, Busqueda.dFechaRegistroTarjetaSAPEquipo, Busqueda.dFechaManufacturaTarjetaSAPEquipo,
                                           Busqueda.nPeriodoGarantiaEquipo, Busqueda.nPeriodoMinimoMantenimientoEquipo, Busqueda.vNumeroSerieEquipo, Busqueda.vNumeroParteEquipo,
                                           Busqueda.cIdCliente, Busqueda.cIdClienteUbicacion, Busqueda.dFechaUltimaModificacionEquipo, Busqueda.dFechaCreacionEquipo,
                                           Busqueda.cIdUsuarioUltimaModificacionEquipo, Busqueda.cIdUsuarioCreacionEquipo, Busqueda.cEstadoEquipo,
                                           Busqueda.bTieneContratoEquipo, Busqueda.vContratoReferenciaActualEquipo,
                                           Busqueda.vDescripcionEquipoSAP, Busqueda.cIdTipoEquipo, Busqueda.vTagEquipo, Busqueda.vCapacidadEquipo,
                                           Busqueda.vAreaEquipo, Busqueda.vIdArticuloSAPEquipo, Busqueda.cIdEquipo).ReturnValue.ToString
                Else
                    x = Data.PA_LOGI_MNT_EQUIPO("SQL_INSERT", "", Busqueda.cIdEquipo, Busqueda.cIdCatalogo,
                                           Busqueda.cIdTipoActivo, Busqueda.vDescripcionEquipo,
                                           Busqueda.vDescripcionAbreviadaEquipo, Busqueda.cIdJerarquiaCatalogo,
                                           Busqueda.dFechaTransaccionEquipo, Busqueda.cIdEnlaceEquipo,
                                           Busqueda.cIdEnlaceCatalogo, Busqueda.cIdSistemaFuncionalEquipo, Busqueda.bEstadoRegistroEquipo,
                                           Busqueda.vObservacionEquipo, Busqueda.nVidaUtilEquipo, Busqueda.cIdEmpresa, Busqueda.cIdEstadoComponenteEquipo,
                                           Busqueda.vIdEquivalenciaEquipo, Busqueda.cIdPaisOrigenEquipo, Busqueda.vIdClienteSAPEquipo,
                                           Busqueda.cIdEquipoSAPEquipo, Busqueda.dFechaRegistroTarjetaSAPEquipo, Busqueda.dFechaManufacturaTarjetaSAPEquipo,
                                           Busqueda.nPeriodoGarantiaEquipo, Busqueda.nPeriodoMinimoMantenimientoEquipo, Busqueda.vNumeroSerieEquipo, Busqueda.vNumeroParteEquipo,
                                           Busqueda.cIdCliente, Busqueda.cIdClienteUbicacion, Busqueda.dFechaUltimaModificacionEquipo, Busqueda.dFechaCreacionEquipo,
                                           Busqueda.cIdUsuarioUltimaModificacionEquipo, Busqueda.cIdUsuarioCreacionEquipo, Busqueda.cEstadoEquipo,
                                           Busqueda.bTieneContratoEquipo, Busqueda.vContratoReferenciaActualEquipo,
                                           Busqueda.vDescripcionEquipoSAP, Busqueda.cIdTipoEquipo, Busqueda.vTagEquipo, Busqueda.vCapacidadEquipo,
                                           Busqueda.vAreaEquipo, Busqueda.vIdArticuloSAPEquipo, Busqueda.cIdEquipo).ReturnValue.ToString
                End If
                'x = Data.PA_LOGI_MNT_Equipo("SQL_INSERT", "", Busqueda.cIdEquipo, Busqueda.cIdCatalogo, _
                '                                   Busqueda.cIdTipoActivo, Busqueda.vDescripcionEquipo, _
                '                                   Busqueda.vDescripcionAbreviadaEquipo, Busqueda.cIdJerarquiaEquipo, _
                '                                   Busqueda.nVidaUtilEquipo, strNroEnlaceEquipo,
                '                                   Busqueda.cIdSistemaFuncional, Busqueda.bEstadoRegistroEquipo, _
                '                                   Busqueda.cIdEquipo).ReturnValue.ToString
                'Else
                '    x = Data.PA_LOGI_MNT_Equipo("SQL_UPDATE", "", Busqueda.cIdEquipo, Busqueda.cIdTipoActivo, Busqueda.cIdJerarquiaEquipo, _
                '                                  Busqueda.cIdSistemaFuncional, strNroEnlaceEquipo, Busqueda.vDescripcionEquipo, _
                '                                  Busqueda.vDescripcionAbreviadaEquipo, Busqueda.bEstadoRegistroEquipo, _
                '                                  Busqueda.cIdEquipo).ReturnValue.ToString
                'End If

                'x = Data.PA_PER_MNT_DETALLETAREO("SQL_INSERT", "", intNroOrdenTareo, Busqueda.cIdPeriodoTareo, Busqueda.cIdConcepto, _
                '                                            Busqueda.cIdTrabajador, Busqueda.nIdProyecto, Busqueda.cIdDetalleDias, _
                '                                            Busqueda.dFechaHoraDetalleAsistencia, Busqueda.vApellidoPaternoDetalleTareo, _
                '                                            Busqueda.vApellidoMaternoDetalleTareo, Busqueda.cIdCargo, Busqueda.vNombresDetalleTareo, _
                '                                            Busqueda.nTotalDiasLibresTareo, Busqueda.nTotalDiasTrabajados, Busqueda.nTotalDiasPendientes, _
                '                                            Busqueda.cIdSistemaTrabajo, Busqueda.vImporteDetalleTareo, Busqueda.vObservacionDetalleTareo, Busqueda.cIdMaquina, _
                '                                            intNroOrdenTareo).ReturnValue.ToString
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function EquipoInserta(ByVal Equipo As LOGI_EQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_EQUIPO("SQL_INSERT", "", Equipo.cIdEquipo, Equipo.cIdCatalogo,
                                           Equipo.cIdTipoActivo, Equipo.vDescripcionEquipo,
                                           Equipo.vDescripcionAbreviadaEquipo, Equipo.cIdJerarquiaCatalogo,
                                           Equipo.dFechaTransaccionEquipo, Equipo.cIdEnlaceEquipo,
                                           Equipo.cIdEnlaceCatalogo, Equipo.cIdSistemaFuncionalEquipo, Equipo.bEstadoRegistroEquipo,
                                           Equipo.vObservacionEquipo, Equipo.nVidaUtilEquipo, Equipo.cIdEmpresa, Equipo.cIdEstadoComponenteEquipo,
                                           Equipo.vIdEquivalenciaEquipo, Equipo.cIdPaisOrigenEquipo, Equipo.vIdClienteSAPEquipo,
                                           Equipo.cIdEquipoSAPEquipo, Equipo.dFechaRegistroTarjetaSAPEquipo, Equipo.dFechaManufacturaTarjetaSAPEquipo,
                                           Equipo.nPeriodoGarantiaEquipo, Equipo.nPeriodoMinimoMantenimientoEquipo, Equipo.vNumeroSerieEquipo, Equipo.vNumeroParteEquipo,
                                           Equipo.cIdCliente, Equipo.cIdClienteUbicacion, Equipo.dFechaUltimaModificacionEquipo, Equipo.dFechaCreacionEquipo,
                                           Equipo.cIdUsuarioUltimaModificacionEquipo, Equipo.cIdUsuarioCreacionEquipo, Equipo.cEstadoEquipo,
                                           Equipo.bTieneContratoEquipo, Equipo.vContratoReferenciaActualEquipo,
                                           Equipo.vDescripcionEquipoSAP, Equipo.cIdTipoEquipo, Equipo.vTagEquipo, Equipo.vCapacidadEquipo,
                                           Equipo.vAreaEquipo, Equipo.vIdArticuloSAPEquipo, Equipo.cIdEquipo).ReturnValue.ToString
        Return x
    End Function

    Public Function EquipoEdita(ByVal Equipo As LOGI_EQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_EQUIPO("SQL_UPDATE", "", Equipo.cIdEquipo, Equipo.cIdCatalogo,
                                           Equipo.cIdTipoActivo, Equipo.vDescripcionEquipo,
                                           Equipo.vDescripcionAbreviadaEquipo, Equipo.cIdJerarquiaCatalogo,
                                           Equipo.dFechaTransaccionEquipo, Equipo.cIdEnlaceEquipo,
                                           Equipo.cIdEnlaceCatalogo, Equipo.cIdSistemaFuncionalEquipo, Equipo.bEstadoRegistroEquipo,
                                           Equipo.vObservacionEquipo, Equipo.nVidaUtilEquipo, Equipo.cIdEmpresa, Equipo.cIdEstadoComponenteEquipo,
                                           Equipo.vIdEquivalenciaEquipo, Equipo.cIdPaisOrigenEquipo, Equipo.vIdClienteSAPEquipo,
                                           Equipo.cIdEquipoSAPEquipo, Equipo.dFechaRegistroTarjetaSAPEquipo, Equipo.dFechaManufacturaTarjetaSAPEquipo,
                                           Equipo.nPeriodoGarantiaEquipo, Equipo.nPeriodoMinimoMantenimientoEquipo, Equipo.vNumeroSerieEquipo, Equipo.vNumeroParteEquipo,
                                           Equipo.cIdCliente, Equipo.cIdClienteUbicacion, Equipo.dFechaUltimaModificacionEquipo, Equipo.dFechaCreacionEquipo,
                                           Equipo.cIdUsuarioUltimaModificacionEquipo, Equipo.cIdUsuarioCreacionEquipo, Equipo.cEstadoEquipo,
                                           Equipo.bTieneContratoEquipo, Equipo.vContratoReferenciaActualEquipo,
                                           Equipo.vDescripcionEquipoSAP, Equipo.cIdTipoEquipo, Equipo.vTagEquipo, Equipo.vCapacidadEquipo,
                                           Equipo.vAreaEquipo, Equipo.vIdArticuloSAPEquipo, Equipo.cIdEquipo).ReturnValue.ToString
        If IsNothing(Equipo.vContratoReferenciaActualEquipo) = False Then
            Dim ValoresContrato As String() = Trim(Equipo.vContratoReferenciaActualEquipo).ToString.Split("-")

            Dim dtEquiposContrato = EquipoGetData("SELECT * FROM LOGI_DETALLECONTRATO WHERE cIdTipoDocumentoCabeceraContrato = '" & ValoresContrato(0) & "' AND vIdNumeroSerieCabeceraContrato = '" & ValoresContrato(1) & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & ValoresContrato(2) & "' AND cIdEmpresa = '" & Equipo.cIdEmpresa & "' AND cIdEquipoDetalleContrato = '" & Equipo.cIdEquipo & "'")
            'For i = 0 To dtEquipoEliminar.Rows.Count - 1
            '    For Each Busqueda In DetalleEquipo
            '        If dtEquipoEliminar.Rows(i).Item(0).ToString.Trim = Busqueda.cIdTipoActivo And
            '                   dtEquipoEliminar.Rows(i).Item(1).ToString.Trim = Busqueda.cIdCatalogo And
            '                   dtEquipoEliminar.Rows(i).Item(2).ToString.Trim = Busqueda.cIdEquipo Then
            '            booExisteDato = True
            '        End If
            '    Next
            '    If booExisteDato = False Then

            If dtEquiposContrato.Rows.Count > 0 Then
                x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_UPDATE", "", ValoresContrato(0), ValoresContrato(1), ValoresContrato(2),
                                                     Equipo.cIdEmpresa, dtEquiposContrato.Rows(0).Item("nIdNumeroItemDetalleContrato"), Equipo.cIdEquipo,
                                                     Equipo.vDescripcionEquipo, Equipo.bEstadoRegistroEquipo, ValoresContrato(2)).ReturnValue.ToString
            Else
                Dim NroItem = EquipoGetData("SELECT ISNULL(MAX(nIdNumeroItemDetalleContrato), 0) + 1 FROM LOGI_DETALLECONTRATO " &
                          "WHERE cIdTipoDocumentoCabeceraContrato = '" & ValoresContrato(0) & "' AND vIdNumeroSerieCabeceraContrato = '" & ValoresContrato(1) & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & ValoresContrato(2) & "' AND cIdEmpresa = '" & Equipo.cIdEmpresa & "'").Rows(0).Item(0)
                x = Data.PA_LOGI_MNT_DETALLECONTRATO("SQL_INSERT", "", ValoresContrato(0), ValoresContrato(1), ValoresContrato(2),
                                                     Equipo.cIdEmpresa, NroItem, Equipo.cIdEquipo,
                                                     Equipo.vDescripcionEquipo, Equipo.bEstadoRegistroEquipo, ValoresContrato(2)).ReturnValue.ToString
            End If
        End If
        Return x
    End Function

    Public Function EquipoElimina(ByVal Equipo As LOGI_EQUIPO) As Int32
        Dim x
        x = Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' ",
                                           "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "1", "", "", "", "", "", "", "", "").ReturnValue.ToString
        '"", "", "", "", "", "", Now, "", "", "", "1", "", "0", Now, "", "0", "0", 0, "", "", "", "", "", "", "", "", "", Now, "", "", Now, 0, "", "", "", "", "", "", 0, 0, 0, 0, "", "", "", "").ReturnValue.ToString
        Return x
    End Function

    'Public Function EquipoExiste(ByVal IdEquipo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
    'Public Function EquipoExiste(ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String) As Boolean
    Public Function EquipoExiste(ByVal IdEquipo As String) As Boolean
        'If Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO WHERE cIdEquipo = '" & IdEquipo & "' " &
        '                                  " AND bEstadoRegistroEquipo = 1" & IIf(IdCatalogo = "", "", " AND cIdCatalogo = '" & IdCatalogo & "'") & IIf(IdTipoActivo = "", "", " AND cIdTipoActivo = '" & IdTipoActivo & "'"),
        '                                  "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "").Count > 0 Then
        If Data.PA_LOGI_MNT_EQUIPO("SQL_NONE", "SELECT * FROM LOGI_EQUIPO WHERE cIdEquipo = '" & IdEquipo & "' " &
                                      " AND bEstadoRegistroEquipo = 1",
                                      "", "", "", "", "", "", Now, "", "", "", "1", "", "0", "", "", "", "", "", "", Now, Now, 0, 0, "", "", "", "", Now, Now, "", "", " ", "1", "", "", "", "", "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
