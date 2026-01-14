Imports System.Data
Imports System.Data.SqlClient
Public Class clsCorrelativoDocumentoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function CorrelativoGetData(strQuery As String) As DataTable
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

    'Public Function SerieListarCombo(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
    Public Function SerieListarCombo(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT nNroSerieCorrDoc, RIGHT ('000' + CONVERT (VARCHAR(3), nNroSerieCorrDoc), 3) AS NroSerie FROM GNRL_CORRELATIVODOCUMENTO " & _
        '                                                     "WHERE cIdPuntoVenta = '" & IdPuntoVenta & "' AND cIdEmpresa = '" & IdEmpresa & "' AND cIdTipoDocumento = '" & IdTipoDocumento & "'", "", "", "", "0", "0", "0", Now.Date, "0", "0", "", "")
        Dim ConsultaNew = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT nNumeroDigitoSerieCorrelativoDocumento FROM GNRL_CORRELATIVODOCUMENTO " &
                                                                "WHERE cIdEmpresa = '" & IdEmpresa & "' AND cIdTipoDocumento = '" & IdTipoDocumento & "' AND bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "' " & IIf(IdTipoDocumento = "BV" Or IdTipoDocumento = "FA", "", " AND cIdTipoDocumentoRefCorrelativoDocumento = '" & IdTipoDocumentoRef & "'"), "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")

        Dim iDigito As Integer
        For Each Digitos In ConsultaNew
            iDigito = Digitos.nNumeroDigitoSerieCorrelativoDocumento
        Next

        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORDOC.nNumeroSerieCorrelativoDocumento,  IIF (CORDOC.bFacturacionElectronicaCorrelativoDocumento = 1, CORDOC.vSerieCorrelativoDocumento + RIGHT (REPLICATE('0', (" + Convert.ToString(iDigito) + ")) + CONVERT (VARCHAR(" + Convert.ToString(iDigito) + "), " &
        '                                                     "CORDOC.nNumeroSerieCorrelativoDocumento), " + Convert.ToString(iDigito) + " - LEN(CORDOC.vSerieCorrelativoDocumento)), " &
        '                                                     "RIGHT (REPLICATE('0', (" + Convert.ToString(iDigito) + ")) + CONVERT (VARCHAR(" + Convert.ToString(iDigito) + "), " &
        '                                                     "CORDOC.nNumeroSerieCorrelativoDocumento), " + Convert.ToString(iDigito) + ")) AS NroSerie " &
        '                                                     "FROM GNRL_CORRELATIVODOCUMENTO AS CORDOC INNER JOIN VTAS_CAJERO AS CAJ ON " &
        '                                                     "     CAJ.cIdPuntoVenta = CORDOC.cIdPuntoVenta AND CAJ.cIdEmpresa = CORDOC.cIdEmpresa " &
        '                                                     "     AND CAJ.cIdTipoDocumentoCajero = CORDOC.cIdTipoDocumento AND CAJ.bFacturacionElectronicaCajero = CORDOC.bFacturacionElectronicaCorrelativoDocumento " &
        '                                                     "     AND CAJ.cIdTipoDocumentoRefCajero = CORDOC.cIdTipoDocumentoRefCorrelativoDocumento " &
        '                                                     "WHERE CORDOC.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORDOC.cIdEmpresa = '" & IdEmpresa & "' AND CORDOC.cIdTipoDocumento = '" & IdTipoDocumento & "' AND CORDOC.bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "' " &
        '                                                     "      AND CAJ.cIdCajero = '" & IdCajero & "' " &
        '                                                     "      AND CAJ.vIdNumeroSerieDocumentoCajero = IIF (CORDOC.bFacturacionElectronicaCorrelativoDocumento = 1, CORDOC.vSerieCorrelativoDocumento + RIGHT (REPLICATE('0', (4)) + CONVERT (VARCHAR(4), CORDOC.nNumeroSerieCorrelativoDocumento), 4 - LEN(CORDOC.vSerieCorrelativoDocumento)), RIGHT (REPLICATE('0', (4)) + CONVERT (VARCHAR(4), CORDOC.nNumeroSerieCorrelativoDocumento), 4))" &
        '                                                     IIf(IdTipoDocumento = "BV" Or IdTipoDocumento = "FA", "", " AND CORDOC.cIdTipoDocumentoRefCorrelativoDocumento = '" & IdTipoDocumentoRef & "'"), "", "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")
        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORDOC.nNumeroSerieCorrelativoDocumento, CAJ.vIdNumeroSerieDocumentoCajero AS NroSerie " &
        '                                                     "FROM GNRL_CORRELATIVODOCUMENTO AS CORDOC INNER JOIN VTAS_CAJERO AS CAJ ON " &
        '                                                     "     CAJ.cIdPuntoVenta = CORDOC.cIdPuntoVenta AND CAJ.cIdEmpresa = CORDOC.cIdEmpresa " &
        '                                                     "     AND CAJ.cIdTipoDocumentoCajero = CORDOC.cIdTipoDocumento AND CAJ.bFacturacionElectronicaCajero = CORDOC.bFacturacionElectronicaCorrelativoDocumento " &
        '                                                     "     AND CAJ.cIdTipoDocumentoRefCajero = CORDOC.cIdTipoDocumentoRefCorrelativoDocumento " &
        '                                                     "WHERE CORDOC.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORDOC.cIdEmpresa = '" & IdEmpresa & "' AND CORDOC.cIdTipoDocumento = '" & IdTipoDocumento & "' AND CORDOC.bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "' " &
        '                                                     "      AND CAJ.cIdCajero = '" & IdCajero & "' " &
        '                                                     "      AND CORDOC.vSerieCorrelativoDocumento = CAJ.vIdNumeroSerieDocumentoCajero " &
        '                                                     IIf(IdTipoDocumento = "BV" Or IdTipoDocumento = "FA", "", " AND CORDOC.cIdTipoDocumentoRefCorrelativoDocumento = '" & IdTipoDocumentoRef & "'"), "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")
        Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORDOC.nNumeroSerieCorrelativoDocumento, CAJ.vIdNumeroSerieDocumentoCajero AS NroSerie " &
                                                             "FROM GNRL_CORRELATIVODOCUMENTO AS CORDOC INNER JOIN VTAS_CAJERO AS CAJ ON " &
                                                             "     CAJ.cIdEmpresa = CORDOC.cIdEmpresa " &
                                                             "     AND CAJ.cIdTipoDocumentoCajero = CORDOC.cIdTipoDocumento AND CAJ.bFacturacionElectronicaCajero = CORDOC.bFacturacionElectronicaCorrelativoDocumento " &
                                                             "     AND CAJ.cIdTipoDocumentoRefCajero = CORDOC.cIdTipoDocumentoRefCorrelativoDocumento " &
                                                             "WHERE CAJ.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORDOC.cIdEmpresa = '" & IdEmpresa & "' AND CORDOC.cIdTipoDocumento = '" & IdTipoDocumento & "' AND CORDOC.bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "' " &
                                                             "      AND CAJ.cIdCajero = '" & IdCajero & "' " &
                                                             "      AND CORDOC.vSerieCorrelativoDocumento = CAJ.vIdNumeroSerieDocumentoCajero " &
                                                             IIf(IdTipoDocumento = "BV" Or IdTipoDocumento = "FA", "", " AND CORDOC.cIdTipoDocumentoRefCorrelativoDocumento = '" & IdTipoDocumentoRef & "'"), "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")

        Dim Coleccion As New List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        For Each NroSerie In Consulta
            Dim Serie As New VI_GNRL_CORRELATIVODOCUMENTO
            Serie.Serie = NroSerie.nNumeroSerieCorrelativoDocumento
            Serie.TipDoc = NroSerie.NroSerie
            Coleccion.Add(Serie)
        Next
        Return Coleccion
    End Function

    Public Function CorrelativoListarCombo(ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumento As String, ByVal IdCajero As String, Optional IdTipoDocumentoRef As String = "", Optional FacElec As Boolean = False) As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_SELECT_ALL9", "SELECT nNroCorrelativoDesdeCorrDoc, RIGHT ('0000000' + CONVERT (VARCHAR(7), nNroCorrelativoDesdeCorrDoc), 7) AS NroDoc FROM GNRL_CORRELATIVODOCUMENTO " & _
        '                                                     "WHERE cIdPuntoVenta = '" & IdPuntoVenta & "' AND cIdEmpresa = '" & IdEmpresa & "' AND cIdTipoDocumento = '" & IdTipoDocumento & "'", "", "", "", "0", "0", "0", Now.Date, "0", "0", "", "")

        'Dim Coleccion As New List(Of VI_GNRL_CORRELATIVODOCUMENTO)

        'For Each NroDoc In Consulta
        '    Dim NroDocumento As New VI_GNRL_CORRELATIVODOCUMENTO
        '    NroDocumento.NroDesde = NroDoc.nNumeroCorrelativoDesdeCorrDoc
        '    NroDocumento.TipDoc = NroDoc.NroDoc
        '    Coleccion.Add(NroDocumento)
        'Next
        'Return Coleccion
        'Dim ConsultaNew = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT nNumeroDigitoDocumentoCorrelativoDocumento FROM GNRL_CORRELATIVODOCUMENTO " &
        '                                                "WHERE cIdPuntoVenta = '" & IdPuntoVenta & "' AND cIdEmpresa = '" & IdEmpresa & "' AND cIdTipoDocumento = '" & IdTipoDocumento & "'", "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")
        Dim ConsultaNew = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT nNumeroDigitoDocumentoCorrelativoDocumento FROM GNRL_CORRELATIVODOCUMENTO " &
                                                        "WHERE cIdEmpresa = '" & IdEmpresa & "' AND cIdTipoDocumento = '" & IdTipoDocumento & "'", "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")

        Dim iDigito As Integer
        For Each Digitos In ConsultaNew
            iDigito = Digitos.nNumeroDigitoDocumentoCorrelativoDocumento
        Next

        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORDOC.nNumeroCorrelativoActualCorrelativoDocumento, IIF (CORDOC.bFacturacionElectronicaCorrelativoDocumento = 1,  CONVERT(VARCHAR(6), CORDOC.nNumeroCorrelativoActualCorrelativoDocumento), RIGHT (REPLICATE('0', (" + Convert.ToString(iDigito) + ")) + CONVERT (VARCHAR(" + Convert.ToString(iDigito) + "), " &
        '                                                     "CORDOC.nNumeroCorrelativoActualCorrelativoDocumento), " + Convert.ToString(iDigito) + ")) AS NroDoc " &
        '                                                     "FROM GNRL_CORRELATIVODOCUMENTO AS CORDOC  INNER JOIN VTAS_CAJERO AS CAJ ON " &
        '                                                     "     CAJ.cIdPuntoVenta = CORDOC.cIdPuntoVenta AND CAJ.cIdEmpresa = CORDOC.cIdEmpresa " &
        '                                                     "     AND CAJ.cIdTipoDocumentoCajero = CORDOC.cIdTipoDocumento AND CAJ.bFacturacionElectronicaCajero = CORDOC.bFacturacionElectronicaCorrelativoDocumento " &
        '                                                     IIf(IdTipoDocumento = "NC" Or IdTipoDocumento = "ND", "     AND CAJ.cIdTipoDocumentoRefCajero = CORDOC.cIdTipoDocumentoRefCorrelativoDocumento ", " ") &
        '                                                     "WHERE CORDOC.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORDOC.cIdEmpresa = '" & IdEmpresa & "' AND CORDOC.cIdTipoDocumento = '" & IdTipoDocumento & "' AND CORDOC.bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "' " &
        '                                                     "      AND CAJ.cIdCajero = '" & IdCajero & "'" & IIf(IdTipoDocumentoRef = "", "", " AND CAJ.cIdTipoDocumentoRefCajero = '" & IdTipoDocumentoRef & "'"), "", "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")
        Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORDOC.nNumeroCorrelativoActualCorrelativoDocumento, IIF (CORDOC.bFacturacionElectronicaCorrelativoDocumento = 1,  CONVERT(VARCHAR(6), CORDOC.nNumeroCorrelativoActualCorrelativoDocumento), RIGHT (REPLICATE('0', (" + Convert.ToString(iDigito) + ")) + CONVERT (VARCHAR(" + Convert.ToString(iDigito) + "), " &
                                                             "CORDOC.nNumeroCorrelativoActualCorrelativoDocumento), " + Convert.ToString(iDigito) + ")) AS NroDoc " &
                                                             "FROM GNRL_CORRELATIVODOCUMENTO AS CORDOC  INNER JOIN VTAS_CAJERO AS CAJ ON " &
                                                             "     CAJ.cIdEmpresa = CORDOC.cIdEmpresa " &
                                                             "     AND CAJ.cIdTipoDocumentoCajero = CORDOC.cIdTipoDocumento AND CAJ.bFacturacionElectronicaCajero = CORDOC.bFacturacionElectronicaCorrelativoDocumento " &
                                                             IIf(IdTipoDocumento = "NC" Or IdTipoDocumento = "ND", "     AND CAJ.cIdTipoDocumentoRefCajero = CORDOC.cIdTipoDocumentoRefCorrelativoDocumento ", " ") &
                                                             "WHERE CAJ.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORDOC.cIdEmpresa = '" & IdEmpresa & "' AND CORDOC.cIdTipoDocumento = '" & IdTipoDocumento & "' AND CORDOC.bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "' " &
                                                             "      AND CAJ.cIdCajero = '" & IdCajero & "' AND CORDOC.vSerieCorrelativoDocumento = CAJ.vIdNumeroSerieDocumentoCajero" & IIf(IdTipoDocumentoRef = "", "", " AND CAJ.cIdTipoDocumentoRefCajero = '" & IdTipoDocumentoRef & "'"), "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")


        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORDOC.nNumeroCorrelativoActualCorrDoc, RIGHT (REPLICATE('0', (" + Convert.ToString(iDigito) + ")) + CONVERT (VARCHAR(" + Convert.ToString(iDigito) + "), " &
        '                                                     "CORDOC.nNumeroCorrelativoActualCorrDoc), " + Convert.ToString(iDigito) + ") AS NroDoc FROM GNRL_CORRELATIVODOCUMENTO AS CORDOC " &
        '                                                     "WHERE CORDOC.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORDOC.cIdEmpresa = '" & IdEmpresa & "' AND CORDOC.cIdTipoDocumento = '" & IdTipoDocumento & "' AND CORDOC.bFacturacionElectronicaCorrDoc = '" & FacElec & "'", "", "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "")

        Dim Coleccion As New List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        For Each NroDoc In Consulta
            Dim NroDocumento As New VI_GNRL_CORRELATIVODOCUMENTO
            'NroDocumento.NroActual = NroDoc.nNumeroCorrelativoActualCorrDoc
            'NroDocumento.TipDoc = NroDoc.NroDoc
            NroDocumento.NroActual = NroDoc.nNumeroCorrelativoActualCorrelativoDocumento
            NroDocumento.TipDoc = NroDoc.NroDoc
            Coleccion.Add(NroDocumento)
        Next
        Return Coleccion
    End Function

    'Public Function CorrelativoListarPorId(ByVal IdTipoDoc As String, ByVal IdSerieDoc As String, ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, ByVal IdTipoDocumentoRef As String, Optional FacElec As Boolean = False) As GNRL_CORRELATIVODOCUMENTO
    Public Function CorrelativoListarPorId(ByVal IdTipoDoc As String, ByVal IdSerieDoc As String, ByVal IdEmpresa As String, ByVal IdTipoDocumentoRef As String, Optional FacElec As Boolean = False) As GNRL_CORRELATIVODOCUMENTO
        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_CORRELATIVODOCUMENTO " &
        '                                                     "WHERE cIdTipoDocumento = '" & IdTipoDoc & "' AND vSerieCorrelativoDocumento = '" & IdSerieDoc & "' " &
        '                                                     "AND cIdPuntoVenta = '" & IdPuntoVenta & "' AND cIdEmpresa = '" & IdEmpresa & "' AND cIdTipoDocumentoRefCorrelativoDocumento = '" & IdTipoDocumentoRef & "' AND bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "'", "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")
        Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_CORRELATIVODOCUMENTO " &
                                                             "WHERE cIdTipoDocumento = '" & IdTipoDoc & "' AND vSerieCorrelativoDocumento = '" & IdSerieDoc & "' " &
                                                             "AND cIdEmpresa = '" & IdEmpresa & "' AND cIdTipoDocumentoRefCorrelativoDocumento = '" & IdTipoDocumentoRef & "' AND bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "'", "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")
        Dim Coleccion As New GNRL_CORRELATIVODOCUMENTO
        For Each GNRL_CORRELATIVODOCUMENTO In Consulta
            Coleccion.cIdEmpresa = GNRL_CORRELATIVODOCUMENTO.cIdEmpresa
            'Coleccion.cIdPuntoVenta = GNRL_CORRELATIVODOCUMENTO.cIdPuntoVenta
            Coleccion.cIdTipoDocumento = GNRL_CORRELATIVODOCUMENTO.cIdTipoDocumento
            Coleccion.dFechaVigenciaCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.dFechaVigenciaCorrelativoDocumento
            Coleccion.nNumeroSerieCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.nNumeroSerieCorrelativoDocumento
            Coleccion.nNumeroCorrelativoDesdeCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.nNumeroCorrelativoDesdeCorrelativoDocumento
            Coleccion.nNumeroCorrelativoHastaCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.nNumeroCorrelativoHastaCorrelativoDocumento
            Coleccion.nNumeroDigitoSerieCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.nNumeroDigitoSerieCorrelativoDocumento
            Coleccion.nNumeroDigitoDocumentoCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.nNumeroDigitoDocumentoCorrelativoDocumento
            Coleccion.bFacturacionElectronicaCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.bFacturacionElectronicaCorrelativoDocumento
            Coleccion.nNumeroCorrelativoActualCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.nNumeroCorrelativoActualCorrelativoDocumento
            Coleccion.cIdTipoDocumentoRefCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.cIdTipoDocumentoRefCorrelativoDocumento
            Coleccion.vSerieCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.vSerieCorrelativoDocumento
            Coleccion.bEstadoRegistroCorrelativoDocumento = GNRL_CORRELATIVODOCUMENTO.bEstadoRegistroCorrelativoDocumento
        Next
        Return Coleccion
    End Function

    'Public Function CorrelativoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, Optional FacElec As Boolean = False, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
    Public Function CorrelativoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional FacElec As Boolean = False, Optional ByVal Estado As String = "*") As List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        'Este si puede devolver una colección de datos es decir varios registros
        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORR.nNumeroSerieCorrDoc, CORR.nNumeroCorrelativoDesdeCorrDoc, CORR.cIdPuntoVenta, TDOC.vDescripcionTipoDoc, " & _
        '                                                     "CORR.nNumeroCorrelativoHastaCorrDoc, CORR.cIdEmpresa, CORR.dFechaVigenciaCorrDoc, CORR.cIdTipoDocumento, CORR.nNumeroDigitoSerieCorrDoc, " & _
        '                                                     "CORR.nNumeroDigitoDocumentoCorrDoc, CORR.cIdArea " & _
        '                                                     "FROM GNRL_CORRELATIVODOCUMENTO CORR INNER JOIN GNRL_TIPODOCUMENTO TDOC ON " & _
        '                                                     "     CORR.cIdTipoDocumento = TDOC.cIdTipoDocumento " & _
        '                                                     "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND CORR.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORR.cIdEmpresa = '" & IdEmpresa & "'", "", "", "", "0", "0", "0", Now, "0", "0", "", "")
        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORR.nNumeroSerieCorrelativoDocumento, CORR.nNumeroCorrelativoDesdeCorrelativoDocumento, CORR.cIdPuntoVenta, TDOC.vDescripcionTipoDocumento, " &
        '                                                     "CORR.nNumeroCorrelativoHastaCorrelativoDocumento, CORR.cIdEmpresa, CORR.dFechaVigenciaCorrelativoDocumento, CORR.cIdTipoDocumento, CORR.nNumeroDigitoSerieCorrelativoDocumento, " &
        '                                                     "CORR.nNumeroDigitoDocumentoCorrelativoDocumento, CORR.nNumeroCorrelativoActualCorrelativoDocumento, CORR.bFacturacionElectronicaCorrelativoDocumento, CORR.cIdTipoDocumentoRefCorrelativoDocumento, " &
        '                                                     "CORR.vSerieCorrelativoDocumento, CORR.bEstadoRegistroCorrelativoDocumento " &
        '                                                     "FROM GNRL_CORRELATIVODOCUMENTO CORR INNER JOIN GNRL_TIPODOCUMENTO TDOC ON " &
        '                                                     "     CORR.cIdTipoDocumento = TDOC.cIdTipoDocumento " &
        '                                                     "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND CORR.cIdPuntoVenta = '" & IdPuntoVenta & "' AND CORR.cIdEmpresa = '" & IdEmpresa & "'" &
        '                                                     "      AND CORR.bFacturacionElectronicaCorrelativoDocumento = '" & IIf(FacElec = True, "1", "0") & "'" &
        '                                                     "      AND (CORR.bEstadoRegistroCorrelativoDocumento = '" & Estado & "' OR '*' = '" & Estado & "')",
        '                                                     "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")
        Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT CORR.nNumeroSerieCorrelativoDocumento, CORR.nNumeroCorrelativoDesdeCorrelativoDocumento, TDOC.vDescripcionTipoDocumento, " &
                                                             "CORR.nNumeroCorrelativoHastaCorrelativoDocumento, CORR.cIdEmpresa, CORR.dFechaVigenciaCorrelativoDocumento, CORR.cIdTipoDocumento, CORR.nNumeroDigitoSerieCorrelativoDocumento, " &
                                                             "CORR.nNumeroDigitoDocumentoCorrelativoDocumento, CORR.nNumeroCorrelativoActualCorrelativoDocumento, CORR.bFacturacionElectronicaCorrelativoDocumento, CORR.cIdTipoDocumentoRefCorrelativoDocumento, " &
                                                             "CORR.vSerieCorrelativoDocumento, CORR.bEstadoRegistroCorrelativoDocumento " &
                                                             "FROM GNRL_CORRELATIVODOCUMENTO CORR INNER JOIN GNRL_TIPODOCUMENTO TDOC ON " &
                                                             "     CORR.cIdTipoDocumento = TDOC.cIdTipoDocumento " &
                                                             "WHERE " & Filtro & " LIKE UPPER('%" & Buscar & "%') AND CORR.cIdEmpresa = '" & IdEmpresa & "'" &
                                                             "      AND CORR.bFacturacionElectronicaCorrelativoDocumento = '" & IIf(FacElec = True, "1", "0") & "'" &
                                                             "      AND (CORR.bEstadoRegistroCorrelativoDocumento = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                             "", "", "0", "0", "0", Now, "0", "0", "1", FacElec, 0, "", "", "")

        'Dim Consulta = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_CORRELATIVODOCUMENTO CORR INNER JOIN GNRL_TIPODOCUMENTO TDOC ON " & _
        '                                             "     CORR.cIdTipoDocumento = TDOC.cIdTipoDocumento " & _
        '                                             "", "", "", "", "0", "0", "0", Now, "0", "0", "", "")

        Dim Coleccion As New List(Of VI_GNRL_CORRELATIVODOCUMENTO)
        For Each Busqueda In Consulta
            Dim BuscarCorr As New VI_GNRL_CORRELATIVODOCUMENTO
            BuscarCorr.Descripcion = Busqueda.vDescripcionTipoDocumento
            BuscarCorr.NroActual = Busqueda.nNumeroCorrelativoActualCorrelativoDocumento
            BuscarCorr.NroDesde = Busqueda.nNumeroCorrelativoDesdeCorrelativoDocumento
            BuscarCorr.NroHasta = Busqueda.nNumeroCorrelativoHastaCorrelativoDocumento
            BuscarCorr.Serie = Busqueda.vSerieCorrelativoDocumento 'Busqueda.nNumeroSerieCorrelativoDocumento
            BuscarCorr.TipDoc = Busqueda.cIdTipoDocumento
            BuscarCorr.FacturacionElectronica = Busqueda.bFacturacionElectronicaCorrelativoDocumento
            BuscarCorr.TipDocRef = Busqueda.cIdTipoDocumentoRefCorrelativoDocumento
            BuscarCorr.Estado = Busqueda.bEstadoRegistroCorrelativoDocumento
            Coleccion.Add(BuscarCorr)
        Next
        Return Coleccion
    End Function

    Public Function CorrelativoInserta(ByVal Correlativo As GNRL_CORRELATIVODOCUMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_INSERT", "", Correlativo.cIdEmpresa, Correlativo.cIdTipoDocumento,
                                                  Correlativo.nNumeroSerieCorrelativoDocumento, Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento, Correlativo.nNumeroCorrelativoHastaCorrelativoDocumento,
                                                  Correlativo.dFechaVigenciaCorrelativoDocumento, Correlativo.nNumeroDigitoSerieCorrelativoDocumento, Correlativo.nNumeroDigitoDocumentoCorrelativoDocumento,
                                                  Correlativo.bEstadoRegistroCorrelativoDocumento, Correlativo.bFacturacionElectronicaCorrelativoDocumento, Correlativo.nNumeroCorrelativoActualCorrelativoDocumento,
                                                  Correlativo.cIdTipoDocumentoRefCorrelativoDocumento, Correlativo.vSerieCorrelativoDocumento, "").ReturnValue.ToString
        Return x
    End Function

    Public Function CorrelativoEdita(ByVal Correlativo As GNRL_CORRELATIVODOCUMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_UPDATE", "", Correlativo.cIdEmpresa, Correlativo.cIdTipoDocumento,
                                                  Correlativo.nNumeroSerieCorrelativoDocumento, Correlativo.nNumeroCorrelativoDesdeCorrelativoDocumento, Correlativo.nNumeroCorrelativoHastaCorrelativoDocumento,
                                                  Correlativo.dFechaVigenciaCorrelativoDocumento, Correlativo.nNumeroDigitoSerieCorrelativoDocumento, Correlativo.nNumeroDigitoDocumentoCorrelativoDocumento,
                                                  Correlativo.bEstadoRegistroCorrelativoDocumento, Correlativo.bFacturacionElectronicaCorrelativoDocumento, Correlativo.nNumeroCorrelativoActualCorrelativoDocumento,
                                                  Correlativo.cIdTipoDocumentoRefCorrelativoDocumento, Correlativo.vSerieCorrelativoDocumento, "").ReturnValue.ToString
        Return x
    End Function

    Public Function CorrelativoElimina(ByVal Correlativo As GNRL_CORRELATIVODOCUMENTO) As Int32
        Dim x
        'x = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "UPDATE GNRL_CORRELATIVODOCUMENTO SET bEstadoRegistroCorrelativoDocumento = 0 WHERE cIdEmpresa = '" & Correlativo.cIdEmpresa & "' AND cIdPuntoVenta = '" & Correlativo.cIdPuntoVenta & "' AND cIdTipoDocumento = '" & Correlativo.cIdTipoDocumento & "'",
        '                             "", "", "0", "0", "0", Now, "0", "0", "1", Correlativo.bFacturacionElectronicaCorrelativoDocumento, 0, "", "", "").ReturnValue.ToString
        x = Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "UPDATE GNRL_CORRELATIVODOCUMENTO SET bEstadoRegistroCorrelativoDocumento = 0 WHERE cIdEmpresa = '" & Correlativo.cIdEmpresa & "' AND cIdTipoDocumento = '" & Correlativo.cIdTipoDocumento & "'",
                                     "", "", "0", "0", "0", Now, "0", "0", "1", Correlativo.bFacturacionElectronicaCorrelativoDocumento, 0, "", "", "").ReturnValue.ToString
        Return x
    End Function

    'Public Function CorrelativoExiste(ByVal IdTipoDoc As String, ByVal IdPuntoVenta As String, ByVal IdEmpresa As String, Optional FacElec As Boolean = False) As Boolean
    Public Function CorrelativoExiste(ByVal IdTipoDoc As String, ByVal IdEmpresa As String, Optional FacElec As Boolean = False) As Boolean
        If Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_CORRELATIVODOCUMENTO " &
                                                 "WHERE cIdTipoDocumento = '" & IdTipoDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' AND bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "'", "", "", "0", "0", "0", Now, 0, 0, "1", FacElec, 0, "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CorrelativoExisteV2(ByVal nNumeroSerieCorrelativoDocumento As String, ByVal IdTipoDoc As String, ByVal IdEmpresa As String, Optional FacElec As Boolean = False) As Boolean
        If Data.PA_GNRL_MNT_CORRELATIVODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_CORRELATIVODOCUMENTO " &
                                                 "WHERE cIdTipoDocumento = '" & IdTipoDoc & "' AND cIdEmpresa = '" & IdEmpresa & "' AND bFacturacionElectronicaCorrelativoDocumento = '" & FacElec & "' AND nNumeroSerieCorrelativoDocumento = '" & nNumeroSerieCorrelativoDocumento & "'", "", "", "0", "0", "0", Now, 0, 0, "1", FacElec, 0, "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class