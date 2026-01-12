Imports System.Data.SqlClient
Imports System.Transactions

'Para adjuntar imagen redimensionando el tamaño
'necesitamos las siguientes librerias.
'Inicio
Imports System.Drawing 'Lo adicioné al proyecto.
Imports System.IO
Imports System.Web 'Lo adicioné al proyecto.
'Imports System.Data.SqlClient
Imports System
Imports System.Data
Imports System.Configuration
'Fin

'Para encriptar / desencriptar datos
Imports System.Text
Imports System.Security.Cryptography

'Para que funcione el isMatch
Imports System.Text.RegularExpressions

''Para que funcione el XML -> xmlDocument
'Imports System.Xml

'Esto lo quite
'Para enviar correos electrónicos
'Imports System.Net
'Imports System.Net.Mail

'Para enviar correos SSL implicito
Imports MailKit.Net.Smtp
Imports MailKit.Security 'Esto recien lo puse para poder utilizar el SecureSocketOptions.StartTls
Imports MimeKit

'Para comprimir
Imports Ionic.Zip

'Para exportar a Excel
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet

'Para utilizar el FTP: User y Password de BIMSIC.PE
Imports System.Net

'Nueva opción para importar información via FTP: 17/09/2021
Imports FluentFTP

'Para generar json
Imports Newtonsoft.Json

Public Class clsFuncionesMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext
    Dim TablaSistemaNeg As New clsTablaSistemaMetodos

    'Utilizado para la encriptación
    'Private TripleDes As New TripleDESCryptoServiceProvider

    Public Structure stNumeroAsiento
        Dim cIdPeriodoAsiento As String
        Dim cIdMesAsiento As String
        Dim cIdTipoLibro As String
        Dim cNumeroAsiento As String
    End Structure
    'Private Structure stAsientoTemporal
    '    Dim vSentencia As String
    'End Structure

    'Private Structure stAsientoTemporal
    '    Dim cIdPeriodoAsiento As String
    '    Dim cIdMesAsiento As String
    '    Dim cIdTipoLibro As String
    '    Dim cNumeroAsiento As String
    '    Dim cIdClienteProveedorAsiento As String
    '    Dim cIdTipoDocumento As String
    '    Dim vIdNumeroSerieDocumentoAsiento As String
    '    Dim vIdNumeroDocumentoAsiento As String
    '    Dim cIdPuntoVenta As String
    '    Dim cIdEmpresa As String
    '    Dim dFechaDocumentoAsiento As String
    '    Dim dFechaVencimientoAsiento As String
    '    Dim cNumeroLineaAsiento As String
    '    Dim vGlosaAsiento As String
    '    Dim cIdNumeroCuentaAsiento As String
    '    Dim nDebeMNAsiento As Decimal
    '    Dim nHaberMNAsiento As Decimal
    '    Dim nDebeMEAsiento As Decimal
    '    Dim nHaberMEAsiento As Decimal
    '    Dim cIdTipoMoneda As String
    '    Dim nTipoCambioAsiento As Decimal
    '    Dim cIdTipoAmarreAsiento As String
    '    Dim dFechaPagoAsiento As String
    '    Dim cIdTipoDocumentoRefAsiento As String
    '    Dim vIdNumeroSerieDocumentoRefAsiento As String
    '    Dim vIdNumeroDocumentoRefAsiento As String
    '    Dim cIdAuxiliar As String
    '    Dim bEstadoRegistroAsiento
    '    Dim vRazonSocialAsiento As String
    '    Dim dFechaDocumentoRefAsiento As String
    '    Dim cIdTipoDocumentoClienteProveedorAsiento As String
    '    Dim cIdTipoOperacion As String
    '    Dim cIdMedioPago As String
    '    Dim cNumeroPagoAsiento As String
    '    Dim cIdCentroCosto As String
    '    Dim cIdTipoPersona As String
    '    Dim cIdProducto As String
    '    Dim bEstadoAjustexDiferenciaTipoCambio As Boolean
    'End Structure

    Public Structure stAsiento
        Dim Periodo As String
        Dim Mes As String
        Dim TipoLibro As String
        Dim NroAsiento As String
        Dim ClienteProveedor As String
        Dim PuntoVenta As String
        Dim TipoPersona As String
        Dim Empresa As String
        Dim NroLinea As String
        Dim TipoMoneda As String
        Dim TipoDocumento As String
        Dim NroSerieDoc As String
        Dim NroDoc As String
        Dim FechaDoc As String
        Dim FechaVenc As String
        Dim Glosa As String
        Dim NroCuenta As String
        Dim DebeMN As Decimal
        Dim HaberMN As Decimal
        Dim DebeME As Decimal
        Dim HaberME As Decimal
        Dim CentroCosto As String
    End Structure

    Public Structure stTransaccion
        Dim IdOperacion As String
        Dim FechaRegistro As String
        Dim FechaVencimiento As String
        Dim IdTipoDocumento As String
        Dim NroSerie As String
        Dim NroDocumento As String
        Dim Importe As Decimal
        Dim IGV As Decimal
        Dim IdTipoMoneda As String
        Dim TipoCambio As Decimal
        Dim ImporteSinDescuento As Decimal
        Dim IdTipoDocumentoRef As String
        Dim NroSerieRef As String
        Dim NroDocumentoRef As String
        Dim FechaDocumentoRef As String 'Nuevo
        Dim IdCliente As String
        Dim IdTipoLibro As String
        Dim IdPuntoVenta As String
        Dim IdEmpresa As String
        Dim IdArea As String
        Dim IdTipoPersona As String
        Dim IdGrupo As String
        Dim Glosa As String
        Dim IdAmarre As String
        Dim FechaPago As String
        Dim DescripcionClienteProveedor As String
        Dim IdTipoDocClienteProveedor As String 'Si es DNI o RUC
        Dim IdAuxiliar As String 'Si es Cliente, Proveedor, Otros
        Dim EstadoRegistro As Boolean
        Dim IdTipoOperacion As String
        Dim NroContrato As String
        Dim IdBanco As String

        'Nuevos Campos
        Dim IdTipoMonedaOrigen As String 'Del documento Original
        Dim NroPago As String 'cNumeroPagoAsiento
        Dim IdMedioPago As String 'cIdMedioPago
        Dim IdProducto As String 'cIdProducto
        Dim IdCuentaCorriente As String
        'Dim cIdTipoDocumentoRefAsiento
        'Dim vIdNumeroSerieDocumentoRefAsiento
        'Dim vIdNumeroDocumentoRefAsiento
        'Dim dFechaDocumentoRefAsiento
    End Structure

    Public Function FuncionesGetData(strQuery As String) As DataTable
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

    Public Function GetDSJSon(ByVal dtOrigen As System.Data.DataTable, ByVal strNombreTabla As String) As String
        Dim dsGenerado As New DataSet
        dsGenerado.Namespace = "NetFrameWork"
        Dim Tabla As New DataTable
        Tabla.Namespace = strNombreTabla
        For x = 0 To dtOrigen.Columns.Count - 1
            Tabla.Columns.Add(New DataColumn(dtOrigen.Columns(x).ColumnName, dtOrigen.Columns(x).DataType))
        Next
        dsGenerado.Tables.Add(Tabla)

        For x = 0 To dtOrigen.Rows.Count - 1
            Dim newRow As DataRow = Tabla.NewRow
            For y = 0 To dtOrigen.Columns.Count - 1
                newRow(dtOrigen.Columns(y).ColumnName) = dtOrigen.Rows(x)(y)
            Next
            Tabla.Rows.Add(newRow)
        Next
        dsGenerado.AcceptChanges()
        Dim json As String = JsonConvert.SerializeObject(Tabla, Formatting.Indented)
        json = json.Replace(vbCrLf, "")
        json = json.Replace(": """, ": '")
        json = json.Replace("""", "'")
        Return json
    End Function

    Public Function TamanoCarpeta(ByVal strCarpeta As String) As Long
        Dim DirInfo As New DirectoryInfo(strCarpeta)
        Dim TotalTamanoCarpeta As Long
        For Each Info In DirInfo.GetFiles("*", SearchOption.AllDirectories)
            TotalTamanoCarpeta = TotalTamanoCarpeta + Info.Length
        Next
        Return TotalTamanoCarpeta / 1024 / 1024 '/ 1024
    End Function

    Public Function LogAuditoriaExiste(ByVal IdUsuario As String, ByVal IdPais As String, ByVal IdEmpresa As String, ByVal IdLocal As String, ByVal IdPuntoVenta As String, ByVal IdSistema As String, ByVal IdModulo As String) As Boolean
        If Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_NONE", "SELECT * FROM GNRL_LOGAUDITORIA " &
                                         "WHERE cIdUsuario = '" & IdUsuario & "' AND cIdEmpresa = '" & IdEmpresa & "' " &
                                         "      AND cIdPaisOrigen = '" & IdPais & "' AND cIdLocal = '" & IdLocal & "' " &
                                         "      AND cIdPuntoVenta = '" & IdPuntoVenta & "' AND cIdSistema = '" & IdSistema & "' " &
                                         "      AND cIdModulo = '" & IdModulo & "'",
                                         "", "", "", "", "", "", "", "", "", "", "", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function LogAuditoriaListarAcceso(ByVal IdUsuario As String, ByVal IdPais As String, ByVal IdEmpresa As String, ByVal IdLocal As String, ByVal IdPuntoVenta As String, ByVal IdSistema As String, ByVal IdModulo As String) As GNRL_LOGAUDITORIA
        Dim Consulta = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_NONE", "SELECT TOP 1 dFechaHora FROM GNRL_LOGAUDITORIA " &
                                                      "WHERE cIdUsuario = '" & IdUsuario & "' AND cIdEmpresa = '" & IdEmpresa & "'" &
                                                      "      AND cIdPaisOrigen = '" & IdPais & "' AND cIdLocal = '" & IdLocal & "'" &
                                                      "      AND cIdPuntoVenta = '" & IdPuntoVenta & "' AND cIdSistema = '" & IdSistema & "'" &
                                                      "      AND cIdModulo = '" & IdModulo & "'" &
                                                      "      AND vEvento = 'INGRESO AL SISTEMA' ORDER BY dFechaHora DESC", "", "", "", "", "", "", "", "", "", "", "", "", "", "")

        Dim Coleccion As New GNRL_LOGAUDITORIA

        For Each GNRL_LOGAUDITORIA In Consulta
            Coleccion.cIdPaisOrigen = GNRL_LOGAUDITORIA.cIdPaisOrigen
            Coleccion.cIdEmpresa = GNRL_LOGAUDITORIA.cIdEmpresa
            Coleccion.cIdLocal = GNRL_LOGAUDITORIA.cIdLocal
            Coleccion.cIdPuntoVenta = GNRL_LOGAUDITORIA.cIdPuntoVenta
            Coleccion.cIdSistema = GNRL_LOGAUDITORIA.cIdSistema
            Coleccion.cIdUsuario = GNRL_LOGAUDITORIA.cIdUsuario
            Coleccion.dFechaHora = GNRL_LOGAUDITORIA.dFechaHora
            Coleccion.vEvento = GNRL_LOGAUDITORIA.vEvento
            Coleccion.vIP1 = GNRL_LOGAUDITORIA.vIP1
            Coleccion.vIP2 = GNRL_LOGAUDITORIA.vIP2
            Coleccion.vPagina = GNRL_LOGAUDITORIA.vPagina
            Coleccion.vQuery = GNRL_LOGAUDITORIA.vQuery
            Coleccion.vSesion = GNRL_LOGAUDITORIA.vSesion
            Coleccion.cIdModulo = GNRL_LOGAUDITORIA.cIdModulo
            Coleccion.vIP3Usuario = GNRL_LOGAUDITORIA.vIP3Usuario
        Next
        If Coleccion.dFechaHora Is Nothing Then
            Coleccion.dFechaHora = Now
        End If
        Return Coleccion
    End Function

    Public Function LogAuditoriaInserta(ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                          LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                          LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
        Return x
    End Function

    Public Function OperacionContableCorrelativoAsiento(ByVal Transaccion As stTransaccion) As String
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim strGlosa As String = Transaccion.Glosa

        Dim strSQL As String = "SELECT cNumeroAsiento " &
                               "FROM CTBL_ASIENTO " &
                               "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                               "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                               "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' " &
                               "      AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdEmpresa = '" & Transaccion.IdEmpresa & "' " &
                               "      AND cIdClienteProveedorAsiento = '" & Transaccion.IdCliente & "' " &
                               "      AND cNumeroPagoAsiento = '" & Transaccion.NroPago & "'"

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Asiento")
        Dim strNumeroAsiento
        If Transaccion.NroPago.Trim = "" Or ds.Tables("Asiento").Rows.Count = 0 Then
            strNumeroAsiento = ""
            'Throw New Exception("No tiene número de pago.  Se generará en otro evento.")
        Else
            strNumeroAsiento = ds.Tables("Asiento").Rows(0).Item("cNumeroAsiento")
        End If

        'Throw New Exception("No se generó ningún asiento contable.  Falta parametrizar.")
        'End If

        Try
            cnx.Open()
            Return strNumeroAsiento
            cnx.Close()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function OperacionContableInserta(ByVal lstTransaccion As List(Of stTransaccion),
                                             Optional ByVal NroAsiento As String = "",
                                             Optional ByVal bCanjear As Boolean = False) As String 'As DataTable
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strNumeroAsiento = ""
        Dim bContinuar As Boolean = True 'Recien lo puse
        Dim strLibro As String = lstTransaccion.Item(0).IdTipoLibro
        Dim strCliente As String = lstTransaccion.Item(0).IdCliente
        Dim intNroRegistro As Integer = 0

        Dim cmd As New SqlCommand()
        cmd.CommandTimeout = 1
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet
        Dim dsAsiento As New DataSet
        Dim dr1 As SqlDataReader
        cmd.Connection = cnx
        cnx.Open() 'Se necesita para ejercutar los DataReader
        Dim strComparativo As String = ""
        strComparativo = lstTransaccion.Item(0).IdCliente + lstTransaccion.Item(0).IdEmpresa + lstTransaccion.Item(0).IdPuntoVenta + lstTransaccion.Item(0).IdGrupo + lstTransaccion.Item(0).IdProducto + lstTransaccion.Item(0).IdTipoLibro

        Dim strSQL As String = ""

        'Obtener el formato de la fecha: Nuevo 25/09/2014
        strSQL = "SELECT dateformat FROM sys.syslanguages WHERE name = (SELECT @@LANGUAGE AS TipoIdioma)"
        cmd.CommandText = strSQL

        Dim strFormatoFecha As String = ""

        dr1 = cmd.ExecuteReader()
        If dr1.HasRows Then 'Se ejecuta si tiene centro de costo
            dr1.Read()
            strFormatoFecha = dr1("dateformat").ToString
        End If
        dr1.Close()
        'Fin del formato de la fecha

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_CTBL_MNT_OBTENERNUMEROASIENTO"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPeriodo", SqlDbType.Char, 4))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdMesAsiento", SqlDbType.Char, 2))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cIdPeriodo").Value = Convert.ToString(Year(lstTransaccion.Item(0).FechaRegistro))
        cmd.Parameters("@cIdEmpresa").Value = lstTransaccion.Item(0).IdEmpresa
        cmd.Parameters("@cIdPuntoVenta").Value = lstTransaccion.Item(0).IdPuntoVenta
        cmd.Parameters("@cIdMesAsiento").Value = String.Format("{0:00}", Month(lstTransaccion.Item(0).FechaRegistro))

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(dsAsiento, "NumeroAsiento")

        Dim dtNumeroAsiento As New DataTable
        dtNumeroAsiento.Columns.Add(New DataColumn("cIdPeriodoAsiento", GetType(System.String))) '1
        dtNumeroAsiento.Columns.Add(New DataColumn("cIdMesAsiento", GetType(System.String))) '2
        dtNumeroAsiento.Columns.Add(New DataColumn("cIdTipoLibro", GetType(System.String))) '3
        dtNumeroAsiento.Columns.Add(New DataColumn("cNumeroAsiento", GetType(System.String))) '4

        For Each PosFila In dsAsiento.Tables("NumeroAsiento").Rows
            Dim Fila As DataRow = dtNumeroAsiento.NewRow
            'Dim stNroAsi As New stNumeroAsiento
            Fila("cIdPeriodoAsiento") = PosFila("cIdPeriodoAsiento")
            Fila("cIdMesAsiento") = PosFila("cIdMesAsiento")
            Fila("cIdTipoLibro") = PosFila("cIdTipoLibro")
            Fila("cNumeroAsiento") = PosFila("cNumeroAsiento")
            dtNumeroAsiento.Rows.Add(Fila)
        Next

        For Each NumAsiento In dtNumeroAsiento.Rows
            If NumAsiento("cIdTipoLibro") = lstTransaccion.Item(0).IdTipoLibro Then
                NumAsiento("cNumeroAsiento") = String.Format("{0:0000}", Convert.ToDecimal(NumAsiento("cNumeroAsiento")) + 1)
                strNumeroAsiento = NumAsiento("cNumeroAsiento")
            End If
        Next

        cmd.CommandType = CommandType.Text

        'Inicializo la Transacción
        Using scope As New TransactionScope()
            For Each Transaccion In lstTransaccion
                Dim strGlosa As String = Transaccion.Glosa
                'cnx.Open() 'Se necesita para ejercutar los DataReader
                'LO QUITEEEEE
                'Dim strSQL As String = "SELECT RIGHT ('0000' + CONVERT(VARCHAR(4), ISNULL (CONVERT (NUMERIC, MAX(cNumeroAsiento)), 0) + 1), 4) cNumeroAsiento " & _
                '                       "FROM CTBL_ASIENTO WITH(NOLOCK) " & _
                '                       "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " & _
                '                       "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " & _
                '                       "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' " & _
                '                       "      AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdEmpresa = '" & Transaccion.IdEmpresa & "' "


                Dim strSQL2 As String = "SELECT cIdTipoMoneda FROM GNRL_TIPOMONEDA WITH(NOLOCK) " &
                                       "WHERE bMonedaBaseTipoMoneda = 1 "

                Dim strSQL3 As String = "SELECT TRANS.cIdFormaPago, TRANS.cIdTipoMoneda, CASE WHEN TRANS.cIdOrigenTipoCambioTransaccion = 'N' THEN " &
                            "TRANS.cIdNumeroCuentaPlanCuenta ELSE ISNULL ((SELECT cIdNumeroCuentaPlanCuenta FROM GNRL_TRANSACCION WHERE cIdFormaPago = '" & Transaccion.IdOperacion & "' " &
                           "      AND cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & IIf(Transaccion.IdBanco Is Nothing Or Transaccion.IdBanco = "", "' ", "' AND cIdBanco = '" & Transaccion.IdBanco.Trim & "' ") &
                           "      AND cIdTipoMoneda = '" & Transaccion.IdTipoMonedaOrigen & "' AND cIdEmpresaTransaccion = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVentaTransaccion = '" & Transaccion.IdPuntoVenta & "' " &
                           "      AND (cIdGrupo = '" & Transaccion.IdGrupo & "' OR cIdGrupo IS NULL) AND cIdNumeroCuentaPlanCuenta <> TRANS.cIdNumeroCuentaPlanCuenta " &
                           "      AND cIdOrigenTipoCambioTransaccion = 'S'), TRANS.cIdNumeroCuentaPlanCuenta) END AS cIdNumeroCuentaPlanCuenta, TRANS.cIdPuntoVentaPlanCuenta, TRANS.cIdEmpresaPlanCuenta, " &
                            "       TRANS.cIdEmpresaTransaccion, TRANS.cIdPuntoVentaTransaccion, TRANS.bEstadoRegistroTransaccion, TRANS.cIdTipoTransaccion, TRANS.cIdGrupo, " &
                            "       TRANS.cIdTransaccion, TRANS.vDescripcionTransaccion, TRANS.vDescripcionAbreviadaTransaccion, TRANS.cIdBanco, TRANS.cIdTipoDocumento, " &
                            "       TRANS.cIdTipoDebeHaberTransaccion, TRANS.cIdTipoIGVTransaccion, TRANS.nOrdenTransaccion, TRANS.cIdOrigenTipoCambioTransaccion " &
                            "FROM GNRL_TRANSACCION TRANS WITH(NOLOCK) " &
                           "WHERE TRANS.cIdFormaPago = '" & Transaccion.IdOperacion & "' " &
                           IIf(Transaccion.IdTipoDocumento = Nothing Or Trim(Transaccion.IdTipoDocumento) = "", "", "      AND TRANS.cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & "'") & IIf(Transaccion.IdBanco.Trim = "", " ", " AND TRANS.cIdBanco = '" & Transaccion.IdBanco.Trim & "' ") &
                           IIf(Transaccion.IdCuentaCorriente.Trim = "", " ", " AND TRANS.cIdCuentaCorriente = '" & Transaccion.IdCuentaCorriente.Trim & "' ") &
                           "      AND TRANS.cIdTipoMoneda = '" & Transaccion.IdTipoMoneda & "' AND TRANS.cIdEmpresaTransaccion = '" & Transaccion.IdEmpresa & "' AND TRANS.cIdPuntoVentaTransaccion = '" & Transaccion.IdPuntoVenta & "' " &
                           "      AND (TRANS.cIdGrupo = '" & Transaccion.IdGrupo & "' OR TRANS.cIdGrupo IS NULL)" &
                           "ORDER BY TRANS.nOrdenTransaccion"

                If (bContinuar = True Or (bContinuar = False And NroAsiento <> "")) And strLibro = Transaccion.IdTipoLibro And strCliente = Transaccion.IdCliente Then
                    For Each NumAsiento In dtNumeroAsiento.Rows
                        If NumAsiento("cIdTipoLibro") = Transaccion.IdTipoLibro Then
                            strNumeroAsiento = String.Format("{0:0000}", Convert.ToDecimal(NumAsiento("cNumeroAsiento")))
                        End If
                    Next
                    NroAsiento = strNumeroAsiento
                Else
                    For Each NumAsiento In dtNumeroAsiento.Rows
                        If NumAsiento("cIdTipoLibro") = Transaccion.IdTipoLibro Then
                            NumAsiento("cNumeroAsiento") = String.Format("{0:0000}", Convert.ToDecimal(NumAsiento("cNumeroAsiento")) + 1)
                            strNumeroAsiento = NumAsiento("cNumeroAsiento")
                        End If
                    Next
                End If

                cmd = New SqlCommand(strSQL2, cnx)

                dr1 = cmd.ExecuteReader()

                Dim strTipoMoneda As String = ""
                If dr1.HasRows Then
                    dr1.Read()
                    strTipoMoneda = dr1("cIdTipoMoneda").ToString
                End If

                dr1.Close()

                cmd = New SqlCommand(strSQL3, cnx)
                da = New SqlDataAdapter(cmd)

                ds.Clear() 'Borro lo anterior para procesar nuevamente.
                da.Fill(ds, "Transaccion")

                Dim ImporteTotalMNDebe As Decimal = 0
                Dim ImporteTotalMNHaber As Decimal = 0
                If ds.Tables("Transaccion").Rows.Count > 0 Then

                    Dim PosicionFila As String = ""
                    If bContinuar = True Or (bContinuar = False And strNumeroAsiento <> "") Then 'lo quiteeeee ahoritaaaa 28/01/2014                        
                        Dim strSQL4 As String = "SELECT RIGHT ('00000' + CONVERT(VARCHAR(5), ISNULL (CONVERT (NUMERIC, MAX(" & IIf(Convert.ToString(Year(Transaccion.FechaRegistro)) <= "2013", "cNumeroLineaAsiento", "SUBSTRING (cNumeroLineaAsiento, 2, 5)") & ")), 0) + 1), 5) cNumeroLineaAsiento " &
                               "FROM CTBL_ASIENTO WITH(NOLOCK) " &
                               "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                               "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                               "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' AND cNumeroAsiento = '" & strNumeroAsiento & "'"

                        cmd = New SqlCommand(strSQL4, cnx)
                        dr1 = cmd.ExecuteReader()
                        If dr1.HasRows Then
                            dr1.Read()
                            PosicionFila = dr1("cNumeroLineaAsiento").ToString
                        End If
                        dr1.Close()
                    Else
                        PosicionFila = "00001"
                    End If
                    Dim fila As DataRow
                    Dim Diferencia As Decimal = 0

                    If (lstTransaccion.Count - 1 = intNroRegistro) Then
                        bContinuar = False
                    Else
                        ''                    If lstTransaccion.Item(intNroRegistro).IdCliente = lstTransaccion.Item(intNroRegistro + 1).IdCliente Then                    
                        'If strComparativo <> lstTransaccion.Item(intNroRegistro + 1).IdCliente + lstTransaccion.Item(intNroRegistro + 1).IdEmpresa + lstTransaccion.Item(intNroRegistro + 1).IdPuntoVenta + lstTransaccion.Item(intNroRegistro + 1).IdGrupo + lstTransaccion.Item(intNroRegistro + 1).IdProducto + lstTransaccion.Item(intNroRegistro + 1).IdTipoLibro Then
                        '    bContinuar = False
                        '    strComparativo = lstTransaccion.Item(intNroRegistro).IdCliente + lstTransaccion.Item(intNroRegistro).IdEmpresa + lstTransaccion.Item(intNroRegistro).IdPuntoVenta + lstTransaccion.Item(intNroRegistro).IdGrupo + lstTransaccion.Item(intNroRegistro).IdProducto + lstTransaccion.Item(intNroRegistro).IdTipoLibro
                        '    intNroRegistro = intNroRegistro + 1
                        'Else
                        '    strComparativo = lstTransaccion.Item(intNroRegistro).IdCliente + lstTransaccion.Item(intNroRegistro).IdEmpresa + lstTransaccion.Item(intNroRegistro).IdPuntoVenta + lstTransaccion.Item(intNroRegistro).IdGrupo + lstTransaccion.Item(intNroRegistro).IdProducto + lstTransaccion.Item(intNroRegistro).IdTipoLibro
                        '    bContinuar = True
                        'End If
                        If strComparativo <> Transaccion.IdCliente + Transaccion.IdEmpresa + Transaccion.IdPuntoVenta + Transaccion.IdGrupo + Transaccion.IdProducto + Transaccion.IdTipoLibro Then
                            bContinuar = False
                            strComparativo = Transaccion.IdCliente + Transaccion.IdEmpresa + Transaccion.IdPuntoVenta + Transaccion.IdGrupo + Transaccion.IdProducto + Transaccion.IdTipoLibro
                        Else
                            bContinuar = True
                            strComparativo = lstTransaccion.Item(intNroRegistro + 1).IdCliente + lstTransaccion.Item(intNroRegistro + 1).IdEmpresa + lstTransaccion.Item(intNroRegistro + 1).IdPuntoVenta + lstTransaccion.Item(intNroRegistro + 1).IdGrupo + lstTransaccion.Item(intNroRegistro + 1).IdProducto + lstTransaccion.Item(intNroRegistro + 1).IdTipoLibro
                            If strComparativo <> Transaccion.IdCliente + Transaccion.IdEmpresa + Transaccion.IdPuntoVenta + Transaccion.IdGrupo + Transaccion.IdProducto + Transaccion.IdTipoLibro Then
                                bContinuar = False
                                'NroAsiento = "" 'Recien lo he puestoooo
                                strComparativo = Transaccion.IdCliente + Transaccion.IdEmpresa + Transaccion.IdPuntoVenta + Transaccion.IdGrupo + Transaccion.IdProducto + Transaccion.IdTipoLibro
                            End If
                        End If
                        'lstTransaccion.Count
                        'If ds.Tables("Transaccion").Rows.Count >= 2 And intContadorRegistroTransaccion = 0 Then
                        'intContadorRegistroTransaccion = 1
                        intNroRegistro = intNroRegistro + 1
                        'End If
                    End If
                    'Recien lo puse : FIN
                    Dim NroRegistroAdicional As Integer = 0
                    For Each fila In ds.Tables("Transaccion").Rows
                        'Dim PosicionFila = Mid(Convert.ToString(ds.Tables("Transaccion").Rows.IndexOf(fila) + 100001), 2)
                        Dim bContinuarAdicional As Boolean = True

                        Dim Importe As Decimal = 0
                        Dim ImporteMN As Decimal = 0
                        Dim ImporteME As Decimal = 0

                        NroRegistroAdicional = NroRegistroAdicional + 1
                        If bContinuar = False And fila.Table.Rows.Count = NroRegistroAdicional Then
                            bContinuarAdicional = False
                        End If
                        If fila("cIdTipoIGVTransaccion") = "S" Then Importe = Convert.ToDecimal(Transaccion.IGV) 'PARTE IGV
                        If fila("cIdTipoIGVTransaccion") = "N" Then Importe = Convert.ToDecimal(Transaccion.Importe) 'PARTE NORMAL
                        If fila("cIdTipoIGVTransaccion") = "T" Then Importe = Convert.ToDecimal(Transaccion.Importe) + Convert.ToDecimal(Transaccion.IGV) 'TOTAL IGV+NORMAL
                        If fila("cIdTipoIGVTransaccion") = "I" Then Importe = Convert.ToDecimal(Transaccion.ImporteSinDescuento) 'INTEGRO SIN DESCUENTO
                        If fila("cIdTipoIGVTransaccion") = "D" Then Importe = Convert.ToDecimal(Transaccion.ImporteSinDescuento) - Convert.ToDecimal(Transaccion.Importe) 'DESCUENTO INTEGRO-NORMAL

                        Dim TipoCambioDoc As Decimal = 0.0
                        If fila("cIdOrigenTipoCambioTransaccion") = "S" Then 'Busca el Tipo de Cambio del Documento Original.
                            Dim strSQL4 As String = "SELECT vValor FROM GNRL_TABLASISTEMA WITH(NOLOCK) " &
                                                    "WHERE cIdTablaSistema = '13' AND vValorOpcionalTablaSistema = 'S'"
                            cmd = New SqlCommand(strSQL4, cnx)
                            'cmd.CommandText = strSQL4
                            Dim TipoEmpresa As String = ""
                            'da = New SqlDataAdapter(cmd)
                            'da.Fill(ds, "TipoEmpresa")
                            'Dim dr4 As SqlDataReader = cmd.ExecuteReader()
                            dr1 = cmd.ExecuteReader()
                            If dr1.HasRows Then
                                dr1.Read()
                                TipoEmpresa = dr1("vValor").ToString
                            End If
                            dr1.Close()


                            'Para Generar el Tipo de cambio se tiene que tomar en cuenta lo siguiente:
                            'Pasivo = T.C.V (Debe)
                            'Activo = T.C.C (Haber)


                            'Dim TipoEmpresa = ds.Tables("TipoEmpresa").Rows(0).Item("vValor")
                            Dim strSQL5 As String = ""
                            If TipoEmpresa = "01" And (Transaccion.IdTipoDocumento <> "ND" And Transaccion.IdTipoDocumento <> "NC") Then 'Empresa Inmobiliaria.

                                'strSQL5 = "SELECT CASE WHEN YEAR(CON.dFechaContrato) = YEAR('" & Transaccion.FechaRegistro & "') THEN CON.nTipoCambioContrato ELSE " & _
                                '          "(SELECT nTipoCambioCompra FROM GNRL_TIPOCAMBIO WHERE CONVERT (CHAR(10), (dFechaHoraTipoCambio), 103) = '31/12/' + CONVERT(CHAR(4), YEAR('" & Transaccion.FechaRegistro & "')-1)) END AS nTipoCambioDoc " & _
                                '          "FROM VTAS_CONTRATO CON " & _
                                '                        "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdContrato = '" & Transaccion.NroContrato & "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'"

                                strSQL5 = "SELECT CASE WHEN YEAR(CON.dFechaContrato) = YEAR('" & Transaccion.FechaRegistro & "') THEN CON.nTipoCambioContrato ELSE " &
                                          "(SELECT " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", "nTipoCambioVenta", "nTipoCambioCompra") & " FROM GNRL_TIPOCAMBIO WHERE CONVERT (CHAR(10), (dFechaHoraTipoCambio), 103) = '31/12/' + CONVERT(CHAR(4), YEAR('" & Transaccion.FechaRegistro & "')-1)) END AS nTipoCambioDoc " &
                                          "FROM VTAS_CONTRATO CON " &
                                                        "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdContrato = '" & Transaccion.NroContrato & "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'"

                                '                        SELECT vCabeceraBalance1, cIdNumeroCuentaBalance1 FROM CTBL_BALANCE WHERE cIdPeriodoBalance = '2013' AND cIdTipoBalance1 = 1 AND RTRIM(LTRIM(cIdNumeroCuentaBalance1)) <> ''
                                'UNION
                                'SELECT vCabeceraBalance2, cIdNumeroCuentaBalance2 FROM CTBL_BALANCE WHERE cIdPeriodoBalance = '2013' AND cIdTipoBalance1 = 1 AND RTRIM(LTRIM(cIdNumeroCuentaBalance2)) <> ''



                                cmd = New SqlCommand(strSQL5, cnx)
                                'cmd.CommandText = strSQL5

                                'da = New SqlDataAdapter(cmd)
                                'da.Fill(ds, "TipoCambioDoc")
                                'TipoCambioDoc = ds.Tables("TipoCambioDoc").Rows(0).Item("nTipoCambioDoc")
                                'Dim dr5 As SqlDataReader = cmd.ExecuteReader()
                                dr1 = cmd.ExecuteReader()
                                If dr1.HasRows Then
                                    dr1.Read()
                                    TipoCambioDoc = dr1("nTipoCambioDoc").ToString
                                End If
                                dr1.Close()
                            ElseIf TipoEmpresa = "01" And (Transaccion.IdTipoDocumento = "ND" Or Transaccion.IdTipoDocumento = "NC") Then 'Empresa Inmobiliaria.
                                strSQL5 = "SELECT CASE WHEN YEAR(DOC.dFechaEmisionCabDoc) = YEAR('" & Transaccion.FechaRegistro & "') THEN DOC.nTipoCambioCabDoc ELSE " &
                                          "(SELECT " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", "nTipoCambioVenta", "nTipoCambioCompra") & " FROM GNRL_TIPOCAMBIO WHERE CONVERT (CHAR(10), (dFechaHoraTipoCambio), 103) = '31/12/' + CONVERT(CHAR(4), YEAR('" & Transaccion.FechaRegistro & "')-1)) END AS nTipoCambioDoc " &
                                          "FROM VTAS_CABECERADOCUMENTO DOC " &
                                                        "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND vIdNumeroCorrelativoCabDoc = '" & Transaccion.NroContrato & "' AND cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & "'"
                                cmd = New SqlCommand(strSQL5, cnx)
                                'cmd.CommandText = strSQL5

                                'da = New SqlDataAdapter(cmd)
                                'da.Fill(ds, "TipoCambioDoc")
                                'Dim dr6 As SqlDataReader = cmd.ExecuteReader()
                                dr1 = cmd.ExecuteReader()
                                If dr1.HasRows Then
                                    dr1.Read()
                                    TipoCambioDoc = dr1("nTipoCambioDoc").ToString
                                Else
                                    TipoCambioDoc = Transaccion.TipoCambio
                                End If
                                dr1.Close()

                                'If ds.Tables("TipoCambioDoc").Rows.Count > 0 Then
                                '    TipoCambioDoc = ds.Tables("TipoCambioDoc").Rows(0).Item("nTipoCambioDoc")
                                'Else
                                '    TipoCambioDoc = Transaccion.TipoCambio
                                'End If
                            ElseIf TipoEmpresa = "02" Then 'Empresa Comercial.
                                'FALTA MAS PUNTOS PARA EL TEMA DE LA EMPRESA COMERCIAL.
                                strSQL5 = "SELECT DOC.nTipoCambioCabDoc FROM VTAS_CABECERADOCUMENTO DOC " &
                                                        "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND vIdNumeroCorrelativoCabDoc = '" & Transaccion.NroDocumentoRef & "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'"
                                cmd = New SqlCommand(strSQL5, cnx)
                                'cmd.CommandText = strSQL5
                                'da = New SqlDataAdapter(cmd)
                                'da.Fill(ds, "TipoCambioDoc")
                                'TipoCambioDoc = ds.Tables("TipoCambioDoc").Rows(0).Item("nTipoCambioContrato")
                                'Dim dr7 As SqlDataReader = cmd.ExecuteReader()
                                dr1 = cmd.ExecuteReader()
                                If dr1.HasRows Then
                                    dr1.Read()
                                    TipoCambioDoc = dr1("nTipoCambioContrato").ToString
                                End If
                                dr1.Close()
                            End If
                        End If

                        'If Transaccion.IdCliente.Trim = "32403740" Then
                        '    MsgBox("progaba")
                        'End If

                        ImporteMN = Math.Round(IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Importe, Importe * IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio)) + 0.0001, 2)
                        ImporteME = Math.Round(IIf(Transaccion.IdTipoMoneda <> strTipoMoneda, Importe, Importe / IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio)) + 0.0001, 2)

                        'If bContinuar = True Or (bContinuar = False And NroAsiento <> "") Then
                        '    strNumeroAsiento = NroAsiento
                        'Else
                        '    If fila("cIdTipoDebeHaberTransaccion") = "D" Then
                        '        ImporteTotalMNDebe = ImporteTotalMNDebe + ImporteMN
                        '    Else
                        '        ImporteTotalMNHaber = ImporteTotalMNHaber + ImporteMN
                        '    End If
                        'End If
                        'If ds.Tables("Transaccion").Rows = fila("nOrdenTransaccion") = "1" Then
                        'ImporteMN = IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Math.Round(IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Importe, Importe * Transaccion.TipoCambio), 2), 0)
                        'ImporteME = IIf(Transaccion.IdTipoMoneda <> strTipoMoneda, Math.Round(IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Importe, Importe / Transaccion.TipoCambio), 2), 0)

                        'OBTENIENDO LOS CENTROS DE COSTO
                        strSQL = "SELECT * FROM CTBL_CENTROCOSTO WITH(NOLOCK) " &
                                 "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "'" &
                                 "      AND cIdArea = '" & Transaccion.IdArea & IIf(Transaccion.IdGrupo = "", "'", "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'")

                        'cmd = New SqlCommand(strSQL, cnx)
                        cmd.CommandText = strSQL

                        'da = New SqlDataAdapter(cmd)
                        'da.Fill(ds, "CentroCosto")

                        'Dim dr8 As SqlDataReader = cmd.ExecuteReader()
                        dr1 = cmd.ExecuteReader()
                        If dr1.HasRows Then 'Se ejecuta si tiene centro de costo
                            dr1.Read()
                            'If ds.Tables("CentroCosto").Rows.Count > 0 Then

                            'Dim strCentroCosto = ds.Tables("CentroCosto").Rows(0).Item("cIdCentroCosto")
                            Dim strCentroCosto = dr1("cIdCentroCosto").ToString
                            dr1.Close()

                            strSQL = "SELECT * FROM CTBL_PLANCUENTA WITH(NOLOCK) " &
                                     "WHERE cIdNumeroCuentaPlanCuenta = '" & fila("cIdNumeroCuentaPlanCuenta") & "' " &
                                     "      AND cIdEmpresa = '" & fila("cIdEmpresaTransaccion") & "' " &
                                     "      AND cIdPuntoVenta = '" & fila("cIdPuntoVentaTransaccion") & "'"

                            'cmd = New SqlCommand(strSQL, cnx)
                            cmd.CommandText = strSQL

                            'da = New SqlDataAdapter(cmd)
                            'da.Fill(ds, "PlanCuenta")
                            'Dim strAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cAmarreAutomaticoPlanCuenta")
                            'Dim strDebeAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cDebePlanCuenta")
                            'Dim strHaberAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cHaberPlanCuenta")
                            Dim strAmarre As String = ""
                            Dim strDebeAmarre As String = ""
                            Dim strHaberAmarre As String = ""

                            'Dim dr9 As SqlDataReader = cmd.ExecuteReader()
                            dr1 = cmd.ExecuteReader()
                            If dr1.HasRows Then 'Se ejecuta si tiene centro de costo
                                dr1.Read()
                                strAmarre = dr1("cAmarreAutomaticoPlanCuenta").ToString
                                strDebeAmarre = dr1("cDebePlanCuenta").ToString
                                strHaberAmarre = dr1("cHaberPlanCuenta").ToString
                            End If
                            dr1.Close()

                            'Transaccion.Glosa = fila("vDescripcionAbreviadaTransaccion") & "-" & Transaccion.Glosa
                            Transaccion.Glosa = fila("vDescripcionAbreviadaTransaccion") & "-" & strGlosa
                            If Importe <> 0 Then
                                'Transaccion.IdTipoDocumento
                                strSQL = "INSERT INTO CTBL_ASIENTO " &
                                         "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " &
                                         " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " &
                                         " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " &
                                         " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " &
                                         " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " &
                                         " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion," &
                                         " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " &
                                         " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento, cIdProducto) " &
                                         "VALUES " &
                                         "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" &
                                         Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', " & IIf(IsNothing(Transaccion.IdCliente) = True, "NULL, ", "'" & Transaccion.IdCliente & "', ") & "'" &
                                         Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" &
                                         "M" & PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.IdTipoDocumentoRef, fila("cIdTipoDocumento")) & "', ") & "'" &
                                         IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroSerieRef, Transaccion.NroSerie) & "', '" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroDocumentoRef, Transaccion.NroDocumento) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaRegistro, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaRegistro))) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaVencimiento, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaVencimiento))) & "', '" &
                                         Transaccion.Glosa & "', '" & fila("cIdNumeroCuentaPlanCuenta") & "', " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteMN, 0) & ", " &
                                         IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteMN, 0) & ", " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteME, 0) & ", " &
                                         IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteME, 0) & ", " & IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio) & ", '" &
                                         "" & "', '" & strCentroCosto & "', '" &
                                         IIf(strFormatoFecha = "dmy", Transaccion.FechaPago, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaPago))) & "', '" & Transaccion.DescripcionClienteProveedor & "', '" &
                                         Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, '", "'" & Transaccion.IdTipoOperacion & "', '") &
                                         Transaccion.NroPago & "', " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "', ") & "'" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" &
                                         Transaccion.NroDocumentoRef & "', " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL", "'" & Transaccion.IdProducto & "'") & ")"
                                'PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.IdTipoDocumentoRef, fila("cIdTipoDocumento")) & "', ") & "'" & _
                                'PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True and fila("cIdTipoDocumento")<>Transaccion.IdTipoDocumento, "NULL, ", "'" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.IdTipoDocumentoRef, fila("cIdTipoDocumento")) & "', ") & "'" & _



                                '"" & "', '" & strCentroCostoArea & "', '" & strCentroCostoFuncion & "', '" & strCentroCostoLocal & "', '" & _

                                'Dim command2 As SqlCommand = New SqlCommand(strSQL, cnx)
                                'command2.ExecuteNonQuery()

                                'cmd.CommandType = CommandType.Text
                                cmd.CommandText = strSQL
                                cmd.ExecuteNonQuery()


                                'cmd.CommandType = CommandType.Text
                                'cmd.CommandText = strSQL
                                'cmd.Connection.Open()
                                'cmd.ExecuteNonQuery()
                                'cmd.Connection.Close()

                                'Dim ReturnValue As Integer
                                'Using connection2 As New SqlConnection(MiConexion.GetCnx)
                                '    Try
                                '        ' The transaction is escalated to a full distributed
                                '        ' transaction when connection2 is opened.
                                '        connection2.Open()

                                '        ' Execute the second command in the second database.
                                '        ReturnValue = 0
                                '        Dim command2 As SqlCommand = New SqlCommand(strSQL, connection2)
                                '        ReturnValue = command2.ExecuteNonQuery()
                                '        'writer.WriteLine("Rows to be affected by command2: {0}", returnValue)

                                '    Catch ex As Exception
                                '        ' Display information that command2 failed.
                                '        'writer.WriteLine("returnValue for command2: {0}", returnValue)
                                '        'writer.WriteLine("Exception Message2: {0}", ex.Message)
                                '    End Try
                                'End Using

                                'cmd.CommandType = CommandType.Text
                                'cmd.CommandText = strSQL
                                'cmd.ExecuteNonQuery()


                                'Dim MiConexion As New clsConexionDAO
                                'Dim cnx As New SqlConnection(MiConexion.GetCnx)

                                'Dim miConexion2 As New clsConexionDAO
                                'Dim cnx2 As New SqlConnection(MiConexion.GetCnx)
                                'Dim command2 As SqlCommand = New SqlCommand(strSQL, cnx)
                                'command2.ExecuteNonQuery()


                                'Dim cmd2 = New SqlCommand(strSQL, cnx2)
                                'cmd2.Connection.Open()
                                'cmd2.ExecuteNonQuery()
                                'cmd2.Connection.Close()
                            End If
                            'cmd.BeginExecuteNonQuery() 'Se necesita conexión Asincrona
                            'cmd.EndExecuteNonQuery()
                            'da = New SqlDataAdapter(cmd)




                            'If bContinuar = True Or (bContinuar = False And NroAsiento <> "") Then
                            'If bContinuar = False And NroAsiento <> "" Then
                            If bContinuarAdicional = False And NroAsiento <> "" Then
                                strSQL = "SELECT ISNULL(SUM(nDebeMNAsiento), 0) nDebeMNTotalAsiento, ISNULL(SUM(nHaberMNAsiento), 0) nHaberMNTotalAsiento " &
                                         "FROM CTBL_ASIENTO " &
                                         "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                                         "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                                         "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' " &
                                         "      AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdEmpresa = '" & Transaccion.IdEmpresa & "' " &
                                         "      AND cNumeroAsiento = '" & strNumeroAsiento & "'"

                                'cmd = New SqlCommand(strSQL, cnx)
                                cmd.CommandText = strSQL

                                'da = New SqlDataAdapter(cmd)
                                'da.Fill(ds, "DiferenciaAsiento")
                                'Dim dr10 As SqlDataReader = cmd.ExecuteReader()
                                dr1 = cmd.ExecuteReader()
                                If dr1.HasRows Then 'Se ejecuta si tiene centro de costo
                                    dr1.Read()
                                    ImporteTotalMNDebe = dr1("nDebeMNTotalAsiento")
                                    ImporteTotalMNHaber = dr1("nHaberMNTotalAsiento")
                                End If
                                dr1.Close()

                                'ImporteTotalMNDebe = ds.Tables("DiferenciaAsiento").Rows(0).Item("nDebeMNTotalAsiento")
                                'ImporteTotalMNHaber = ds.Tables("DiferenciaAsiento").Rows(0).Item("nHaberMNTotalAsiento")
                                'ds.Tables("DiferenciaAsiento").Clear()
                            Else
                                If fila("cIdTipoDebeHaberTransaccion") = "D" Then
                                    ImporteTotalMNDebe = ImporteTotalMNDebe + ImporteMN
                                Else
                                    ImporteTotalMNHaber = ImporteTotalMNHaber + ImporteMN
                                End If
                            End If

                            'Lo acabo de sacar 27/09/2014
                            'If ds.Tables("Transaccion").Rows.Count.ToString = fila("nOrdenTransaccion") And (ImporteMN <> 0 Or ImporteME <> 0) Then
                            If bContinuarAdicional = False And bContinuar = False And (ImporteMN <> 0 Or ImporteME <> 0) Then
                                Diferencia = ImporteTotalMNDebe - ImporteTotalMNHaber
                                If Convert.ToDecimal(Diferencia) > 0 Then 'Va en el Haber
                                    fila("cIdNumeroCuentaPlanCuenta") = TablaSistemaNeg.TablaSistemaListarPorId("06", "01", "CTBL", lstTransaccion.Item(0).IdEmpresa, lstTransaccion.Item(0).IdPuntoVenta).vValorOpcionalTablaSistema 'Ganancia
                                    fila("cIdTipoDebeHaberTransaccion") = "H"
                                    ImporteMN = Diferencia
                                ElseIf Convert.ToDecimal(Diferencia) < 0 Then 'Va en el Debe
                                    fila("cIdNumeroCuentaPlanCuenta") = TablaSistemaNeg.TablaSistemaListarPorId("06", "02", "CTBL", lstTransaccion.Item(0).IdEmpresa, lstTransaccion.Item(0).IdPuntoVenta).vValorOpcionalTablaSistema 'Perdida
                                    ImporteMN = Diferencia * -1
                                    fila("cIdTipoDebeHaberTransaccion") = "D"
                                End If

                                strSQL = "SELECT * FROM CTBL_PLANCUENTA " &
                                         "WHERE cIdNumeroCuentaPlanCuenta = '" & fila("cIdNumeroCuentaPlanCuenta") & "' " &
                                         "      AND cIdEmpresa = '" & fila("cIdEmpresaTransaccion") & "' " &
                                         "      AND cIdPuntoVenta = '" & fila("cIdPuntoVentaTransaccion") & "'"

                                'cmd = New SqlCommand(strSQL, cnx)
                                cmd.CommandText = strSQL

                                'da = New SqlDataAdapter(cmd)
                                'da.Fill(ds, "PlanCuenta2")
                                'strAmarre = ds.Tables("PlanCuenta2").Rows(0).Item("cAmarreAutomaticoPlanCuenta")
                                'strDebeAmarre = ds.Tables("PlanCuenta2").Rows(0).Item("cDebePlanCuenta")
                                'strHaberAmarre = ds.Tables("PlanCuenta2").Rows(0).Item("cHaberPlanCuenta")

                                'Dim dr11 As SqlDataReader = cmd.ExecuteReader()
                                dr1 = cmd.ExecuteReader()
                                If dr1.HasRows Then 'Se ejecuta si tiene centro de costo
                                    dr1.Read()
                                    strAmarre = dr1("cAmarreAutomaticoPlanCuenta")
                                    strDebeAmarre = dr1("cDebePlanCuenta")
                                    strHaberAmarre = dr1("cHaberPlanCuenta")
                                End If
                                dr1.Close()

                                'Generar Asiento por diferencia de tipo de cambio.
                                If Convert.ToDecimal(Diferencia) <> 0 And bContinuarAdicional = False Then 'And (ImporteMN <> 0 Or ImporteME <> 0) Then
                                    If InStrRev(Transaccion.Glosa, "CANCELACION", -1) = 1 Then
                                        'Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, InStrRev(Transaccion.Glosa, Transaccion.Glosa.Length - 10, -1))
                                        Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, 13, Transaccion.Glosa.Length)
                                    End If
                                    If InStrRev(Transaccion.Glosa, "POR CUOTA", -1) = 1 Then
                                        'Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, InStrRev(Transaccion.Glosa, Transaccion.Glosa.Length - 10, -1))
                                        Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, 11, Transaccion.Glosa.Length)
                                    End If
                                    If InStrRev(Transaccion.Glosa, "LETRA POR COBRAR", -1) = 1 Then
                                        'Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, InStrRev(Transaccion.Glosa, Transaccion.Glosa.Length - 10, -1))
                                        Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, 18, Transaccion.Glosa.Length)
                                    End If

                                    PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2)
                                    'Transaccion.IdTipoMoneda = strTipoMoneda

                                    strSQL = "INSERT INTO CTBL_ASIENTO " &
                                                "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " &
                                                " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " &
                                                " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " &
                                                " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " &
                                                " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " &
                                                " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion, " &
                                                " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " &
                                                " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento, cIdProducto) " &
                                                "VALUES " &
                                                "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" &
                                                Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', " & IIf(IsNothing(Transaccion.IdCliente) = True, "NULL, ", "'" & Transaccion.IdCliente & "', ") & "'" &
                                                Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" &
                                                "M" & PosicionFila & "', '" & strTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.IdTipoDocumentoRef, fila("cIdTipoDocumento")) & "', ") & "'" &
                                                IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroSerieRef, Transaccion.NroSerie) & "', '" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroDocumentoRef, Transaccion.NroDocumento) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaRegistro, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaRegistro))) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaVencimiento, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaVencimiento))) & "', '" &
                                                Transaccion.Glosa & "', '" & fila("cIdNumeroCuentaPlanCuenta") & "', " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteMN, 0) & ", " &
                                                IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteMN, 0) & ", " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", 0, 0) & ", " &
                                                IIf(fila("cIdTipoDebeHaberTransaccion") = "H", 0, 0) & ", " & IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", 0, 0) & ", '" &
                                                "D" & "', '" & strCentroCosto & "', '" &
                                                IIf(strFormatoFecha = "dmy", Transaccion.FechaPago, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaPago))) & "', '" & Transaccion.DescripcionClienteProveedor & "', '" &
                                                Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, '", "'" & Transaccion.IdTipoOperacion & "', '") &
                                                Transaccion.NroPago & "', " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "', ") & "'" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" &
                                                Transaccion.NroDocumentoRef & "', " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL", "'" & Transaccion.IdProducto & "'") & ")"
                                    'cmd = New SqlCommand(strSQL, cnx)
                                    'cmd.Connection.Open()
                                    'cmd.ExecuteNonQuery()
                                    'cmd.Connection.Close()
                                    'cmd.CommandType = CommandType.Text
                                    cmd.CommandText = strSQL
                                    cmd.ExecuteNonQuery()
                                End If
                            End If
                            'Recien lo saque
                            'If Not TypeOf (strAmarre) Is DBNull And bContinuar = False Then
                            If strAmarre.Trim <> "" And bContinuarAdicional = False Then
                                If strAmarre = "S" Then
                                    PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2)

                                    'Dim PosicionFila = Mid(Convert.ToString(ds.Tables("Transaccion").Rows.IndexOf(fila) + 100001), 2)
                                    '" vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " & _
                                    strSQL = "INSERT INTO CTBL_ASIENTO " &
                                     "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " &
                                     " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " &
                                     " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " &
                                     " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " &
                                     " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " &
                                     " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion, " &
                                     " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " &
                                     " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento, cIdProducto) " &
                                     "VALUES " &
                                     "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" &
                                     Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', " & IIf(IsNothing(Transaccion.IdCliente) = True, "NULL, ", "'" & Transaccion.IdCliente & "', ") & "'" &
                                     Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" &
                                     "M" & PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.IdTipoDocumentoRef, fila("cIdTipoDocumento")) & "', ") & "'" &
                                     IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroSerieRef, Transaccion.NroSerie) & "', '" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroDocumentoRef, Transaccion.NroDocumento) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaRegistro, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaRegistro))) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaVencimiento, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaVencimiento))) & "', '" &
                                     Transaccion.Glosa & "', '" & strDebeAmarre & "', " & ImporteMN & ", " &
                                     "0" & ", " & "0" & ", " &
                                     "0" & ", " & "0" & ", '" &
                                     "A" & "', '" & strCentroCosto & "', '" &
                                     IIf(strFormatoFecha = "dmy", Transaccion.FechaPago, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaPago))) & "', '" & Transaccion.DescripcionClienteProveedor & "', '" &
                                     Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, '", "'" & Transaccion.IdTipoOperacion & "', '") &
                                     Transaccion.NroPago & "', " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "', ") & "'" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" &
                                     Transaccion.NroDocumentoRef & "', " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL", "'" & Transaccion.IdProducto & "'") & ")"
                                    'cmd = New SqlCommand(strSQL, cnx)
                                    'cmd.Connection.Open()
                                    'cmd.ExecuteNonQuery()
                                    'cmd.Connection.Close()
                                    'cmd.CommandType = CommandType.Text
                                    cmd.CommandText = strSQL
                                    cmd.ExecuteNonQuery()

                                    'Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaVencimiento & "', '" & _
                                    PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2)
                                    strSQL = "INSERT INTO CTBL_ASIENTO " &
                                             "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " &
                                             " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " &
                                             " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " &
                                             " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " &
                                             " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " &
                                             " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion, " &
                                             " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " &
                                             " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento, cIdProducto) " &
                                             "VALUES " &
                                             "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" &
                                             Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', " & IIf(IsNothing(Transaccion.IdCliente) = True, "NULL, ", "'" & Transaccion.IdCliente & "', ") & "'" &
                                             Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" &
                                             "M" & PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.IdTipoDocumentoRef, fila("cIdTipoDocumento")) & "', ") & "'" &
                                             IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroSerieRef, Transaccion.NroSerie) & "', '" & IIf(bCanjear = True And fila("nOrdenTransaccion") = 1, Transaccion.NroDocumentoRef, Transaccion.NroDocumento) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaRegistro, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaRegistro))) & "', '" & IIf(strFormatoFecha = "dmy", Transaccion.FechaVencimiento, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaVencimiento))) & "', '" &
                                             Transaccion.Glosa & "', '" & strHaberAmarre & "', " & "0" & ", " &
                                             ImporteMN & ", " & "0" & ", " &
                                             "0" & ", " & "0" & ", '" &
                                             "A" & "', '" & strCentroCosto & "', '" &
                                             IIf(strFormatoFecha = "dmy", Transaccion.FechaPago, String.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(Transaccion.FechaPago))) & "', '" & Transaccion.DescripcionClienteProveedor & "', '" &
                                             Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, '", "'" & Transaccion.IdTipoOperacion & "', '") &
                                             Transaccion.NroPago & "', " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "', ") & "'" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" &
                                             Transaccion.NroDocumentoRef & "', " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL", "'" & Transaccion.IdProducto & "'") & ")"

                                    'Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaVencimiento & "', '" & _
                                    'cmd = New SqlCommand(strSQL, cnx)
                                    'cmd.Connection.Open()
                                    'cmd.ExecuteNonQuery()
                                    'cmd.Connection.Close()
                                    cmd.CommandType = CommandType.Text
                                    cmd.CommandText = strSQL
                                    cmd.ExecuteNonQuery()

                                    ''strAmarre & "', '" & strCentroCostoArea & "', '" & strCentroCostoFuncion & "', '" & strCentroCostoLocal & "', '" & _
                                End If
                                'End If




                                'If InStrRev(Transaccion.Glosa, "CANCELACIÓN", -1) = 1 Then
                                '    fila("Glosa") = Mid(fila("Glosa"), InStrRev(Transaccion.Glosa, Transaccion.Glosa.Length - 10, -1)))
                                '' End If

                                '.Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "01").vDescripcionTablaSistema
                                '.cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                                '.cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                                '.Debe_MN = 0
                                '.Haber_MN = Convert.ToDecimal(txtDiferencia.Text) 'Session("CestaAsiento").Rows(1)("Debe_MN")
                                'Else 'Va en el Haber
                                'AsientoAutoComp.nHaberMNAsiento = Session("CestaAsiento").Rows(1)("Haber_MN")
                                '.Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "02").vDescripcionTablaSistema
                                '.cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                                '.cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                                '.Debe_MN = Convert.ToDecimal(txtDiferencia.Text) * -1
                                '.Haber_MN = 0
                            End If

                            'End If

                            'If Diferencia <> 0 And fila("nOrdenTransaccion") <> 1 Then
                            '    ''.Auxiliar = IIf(Trim(Session("CestaAsiento").Rows(1)("Auxiliar").ToString) = "", "", Session("CestaAsiento").Rows(1)("Auxiliar"))
                            '    '.Auxiliar = Trim(Session("CestaAsiento").Rows(j)("Auxiliar").ToString)
                            '    ''.cAmarreAutomaticoPlanCuenta = "D" 'Diferencia por Tipo de Cambio.
                            '    ''.cAmarreAutomaticoPlanCuenta = " " 'Diferencia por Tipo de Cambio.
                            '    ''.cIdCentroCosto = IIf(Trim(Session("CestaAsiento").Rows(1)("CentroCosto").ToString) = "", Nothing, Session("CestaAsiento").Rows(1)("CentroCosto"))
                            '    '.cIdCentroCosto = Trim(Session("CestaAsiento").Rows(j)("CentroCosto").ToString)
                            '    '.cDebePlanCuenta = ""
                            '    '.cHaberPlanCuenta = ""
                            '    ''.cIdAuxiliar = IIf(Trim(Session("CestaAsiento").Rows(1)("cIdAuxiliar").ToString) = "", "", Session("CestaAsiento").Rows(1)("cIdAuxiliar"))
                            '    '.cIdAuxiliar = Trim(Session("CestaAsiento").Rows(j)("cIdAuxiliar").ToString)
                            '    '.cIdMesAsiento = cboMes.SelectedValue
                            '    '.cIdPeriodoAsiento = cboPeriodo.SelectedValue
                            '    '.cIdTipoLibro = cboLibro.SelectedValue
                            '    '.cNumeroAsiento = txtNroAsiento.Text
                            '    ''.Cuenta = Session("CestaAsiento").Rows(1)("Cuenta").ToString
                            '    '.Debe_ME = 0
                            '    'If Convert.ToDecimal(txtDiferencia.Text) > 0 Then 'Va en el Debe
                            '    '    .Cuenta = TablaSistemaNeg.TablaSistemaListarPorId("06", "01").vValorOpcionalTablaSistema 'Ganancia
                            '    '    .Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "01").vDescripcionTablaSistema
                            '    '    .cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                            '    '    .cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                            '    '    .Debe_MN = 0
                            '    '    .Haber_MN = Convert.ToDecimal(txtDiferencia.Text) 'Session("CestaAsiento").Rows(1)("Debe_MN")
                            '    'Else 'Va en el Haber
                            '    '    'AsientoAutoComp.nHaberMNAsiento = Session("CestaAsiento").Rows(1)("Haber_MN")
                            '    '    .Cuenta = TablaSistemaNeg.TablaSistemaListarPorId("06", "02").vValorOpcionalTablaSistema 'Perdida
                            '    '    .Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "02").vDescripcionTablaSistema
                            '    '    .cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                            '    '    .cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                            '    '    .Debe_MN = Convert.ToDecimal(txtDiferencia.Text) * -1
                            '    '    .Haber_MN = 0
                            '    'End If
                            '    '.cAmarreAutomaticoPlanCuenta = Convert.ToChar(IIf(String.IsNullOrEmpty(.cDebePlanCuenta) = True And String.IsNullOrEmpty(.cHaberPlanCuenta) = True, " ", "S")) 'Diferencia por Tipo de Cambio.
                            '    ''.Debe_MN = Registro.Debe_MN
                            '    ''.Fec_Ven = IIf(Session("CestaAsiento").Rows(1)("Fec_Ven") = "01/01/1900", Nothing, Session("CestaAsiento").Rows(1)("Fec_Ven"))
                            '    '.Fec_Ven = Session("CestaAsiento").Rows(j)("Fec_Ven")
                            '    '.Fecha = Session("CestaAsiento").Rows(j)("Fecha")
                            '    '.Glosa = Session("CestaAsiento").Rows(j)("Glosa").ToString
                            '    '.Haber_ME = 0
                            '    ''.Haber_MN = 0Registro.Haber_MN
                            '    '.Linea = Session("CestaAsiento").Rows(j)("Linea").ToString
                            '    '.Moneda = Session("IdTMonedaBase")
                            '    ''.Nomenclatura = Session("CestaAsiento").Rows(1)("Nomenclatura").ToString
                            '    '.Nro_Doc = Session("CestaAsiento").Rows(j)("Nro_Doc").ToString
                            '    '.Nro_Ser = Session("CestaAsiento").Rows(j)("Nro_Ser").ToString
                            '    '.Razon_Social = Session("CestaAsiento").Rows(j)("Razon_Social").ToString
                            '    '.T_Amarre = "D" 'Ajuste por Diferencia de Tipo de Cambio.
                            '    '.T_Cambio = 0
                            '    ''.T_Doc = IIf(Trim(Session("CestaAsiento").Rows(1)("T_Doc").ToString) = "", Nothing, Session("CestaAsiento").Rows(1)("T_Doc")) 'La conversión es porque si es DBNull.
                            '    '.T_Doc = Trim(Session("CestaAsiento").Rows(j)("T_Doc").ToString)
                            '    ''.cIdCentroCosto = IIf(Trim(Session("CestaAsiento").Rows(1)("CentroCosto").ToString) = "", Nothing, Session("CestaAsiento").Rows(1)("CentroCosto"))
                            '    ''.cIdTipoPersona = Convert.ToChar(IIf(Trim(Session("CestaAsiento").Rows(1)("cIdTipoPersona").ToString.Trim) = "", " ", Session("CestaAsiento").Rows(1)("cIdTipoPersona")))
                            '    '.cIdTipoPersona = Convert.ToChar(Session("CestaAsiento").Rows(j)("cIdTipoPersona").ToString)
                            '    ''.cIdAuxiliar = IIf(Trim(Session("CestaAsiento").Rows(1)("cIdAuxiliar").ToString) = "", Nothing, Session("CestaAsiento").Rows(1)("cIdAuxiliar"))
                            '    ''.cIdTipoOperacion = IIf(Trim(Session("CestaAsiento").Rows(1)("cIdTipoOperacion").ToString.Trim) = "", Nothing, Session("CestaAsiento").Rows(1)("cIdTipoOperacion")) 'Comunmente NO GRAVADO OSEA NO ESTA AFECTO DEL IGV.
                            '    '.cIdTipoOperacion = Session("CestaAsiento").Rows(j)("cIdTipoOperacion").ToString
                            '    '.cNumeroPagoAsiento = Session("CestaAsiento").Rows(j)("cNumeroPagoAsiento").ToString
                            '    ''.dFechaPagoAsiento = IIf(Session("CestaAsiento").Rows(1)("dFechaPagoAsiento") = "01/01/1900", Nothing, Session("CestaAsiento").Rows(1)("dFechaPagoAsiento"))
                            '    '.dFechaPagoAsiento = Session("CestaAsiento").Rows(j)("dFechaPagoAsiento")
                            '    ''.cIdMedioPago = IIf(Trim(Session("CestaAsiento").Rows(1)("cIdMedioPago").ToString.Trim) = "", Nothing, Session("CestaAsiento").Rows(1)("cIdMedioPago"))
                            '    '.cIdMedioPago = Session("CestaAsiento").Rows(j)("cIdMedioPago").ToString
                            '    '.cIdTipoDocumentoRefAsiento = Session("CestaAsiento").Rows(j)("cIdTipoDocumentoRefAsiento").ToString
                            '    '.vIdNumeroSerieDocumentoRefAsiento = Session("CestaAsiento").Rows(j)("vIdNumeroSerieDocumentoRefAsiento").ToString
                            '    '.vIdNumeroDocumentoRefAsiento = Session("CestaAsiento").Rows(j)("vIdNumeroDocumentoRefAsiento").ToString
                            '    '.dFechaDocumentoRefAsiento = Session("CestaAsiento").Rows(j)("dFechaDocumentoRefAsiento").ToString
                            '    '.cIdTipoDocumentoClienteProveedorAsiento = Session("CestaAsiento").Rows(j)("cIdTipoDocumentoClienteProveedorAsiento").ToString
                            '    '    End With


                            '    Dim strAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cAmarreAutomaticoPlanCuenta")
                            '    Dim strDebeAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cDebePlanCuenta")
                            '    Dim strHaberAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cHaberPlanCuenta")

                            '    'INSERTANDO ASIENTO
                            '    strSQL = "INSERT INTO CTBL_ASIENTO " & _
                            '             "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " & _
                            '             " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " & _
                            '             " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " & _
                            '             " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " & _
                            '             " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " & _
                            '             " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion) " & _
                            '             "VALUES " & _
                            '             "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("0{00}", Convert.ToString(Month(Transaccion.FechaRegistro))) & "', '" & _
                            '             Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', '" & Transaccion.IdCliente & "', '" & _
                            '             Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" & _
                            '             PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', '" & Transaccion.IdTipoDocumento & "', '" & _
                            '             Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaRegistro & "', '" & Transaccion.FechaVencimiento & "', '" & _
                            '             Transaccion.Glosa & "', '" & fila("cIdNumeroCuentaPlanCuenta") & "', " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteMN, 0) & ", " & _
                            '             IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteMN, 0) & ", " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteME, 0) & ", " & _
                            '             IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteME, 0) & ", " & IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio) & ", '" & _
                            '             "" & "', '" & strCentroCosto & "', '" & _
                            '             Transaccion.FechaPago & "', '" & Transaccion.DescripcionClienteProveedor & "', '" & _
                            '             Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', '" & Transaccion.IdTipoOperacion & "')"

                            '    If Convert.ToDecimal(Diferencia) > 0 Then 'Va en el Debe
                            '        fila("cIdNumeroCuentaPlanCuenta") = TablaSistemaNeg.TablaSistemaListarPorId("06", "01").vValorOpcionalTablaSistema 'Ganancia
                            '        '.Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "01").vDescripcionTablaSistema
                            '        '.cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                            '        '.cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                            '        ImporteMN =
                            '        .Debe_MN = 0
                            '        .Haber_MN = Convert.ToDecimal(txtDiferencia.Text) 'Session("CestaAsiento").Rows(1)("Debe_MN")
                            '    Else 'Va en el Haber
                            '        'AsientoAutoComp.nHaberMNAsiento = Session("CestaAsiento").Rows(1)("Haber_MN")
                            '        fila("cIdNumeroCuentaPlanCuenta") = TablaSistemaNeg.TablaSistemaListarPorId("06", "02").vValorOpcionalTablaSistema 'Perdida
                            '        .Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "02").vDescripcionTablaSistema
                            '        .cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                            '        .cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                            '        .Debe_MN = Convert.ToDecimal(txtDiferencia.Text) * -1
                            '        .Haber_MN = 0
                            '    End If





                            '    strSQL = "INSERT INTO CTBL_ASIENTO " & _
                            '             "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " & _
                            '             " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " & _
                            '             " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " & _
                            '             " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " & _
                            '             " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " & _
                            '             " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion) " & _
                            '             "VALUES " & _
                            '             "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("0{00}", Convert.ToString(Month(Transaccion.FechaRegistro))) & "', '" & _
                            '             Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', '" & Transaccion.IdCliente & "', '" & _
                            '             Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" & _
                            '             PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', '" & Transaccion.IdTipoDocumento & "', '" & _
                            '             Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaVencimiento & "', '" & _
                            '             Transaccion.Glosa & "', '" & strHaberAmarre & "', " & "0" & ", " & _
                            '             ImporteMN & ", " & "0" & ", " & _
                            '             ImporteME & ", " & Transaccion.TipoCambio & ", '" & _
                            '             strAmarre & "', '" & strCentroCosto & "', '" & _
                            '             Transaccion.FechaPago & "', '" & Transaccion.DescripcionClienteProveedor & "', '" & _
                            '             Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', '" & Transaccion.IdTipoOperacion & "')"
                            'End If
                            PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2) ' Mid(Convert.ToString(ds.Tables("Transaccion").Rows.IndexOf(fila) + 100001), 2)
                        Else
                            Throw New Exception("No se generó ningún asiento contable.  No existe el Centro de Costo.")
                        End If




                        'PARTE IGV 

                        '                            strSQL = "" & _
                        '                            " INSERT INTO EXP_CONTA " & _
                        '                            " (cCajera, cTipoAsiento, cCorrelativo, dFechaTrans, dFechaVenc, cCuenta, " & _
                        '                            "  cSubCuenta, cDebeHaber, cCuentaCte, cNroDocumento, cNroLetra, nImporte, " & _
                        '                            "  nIGV, cCentroCosto, nImporteLetra, nTipoCambio, cCentroCostoV, cCentroCostoF, " & _
                        '                            "  cTipoDocumento, cNroDocumen, nImporteDolar, cTipoMoneda, cTipoDocumentoRef, cNroDocumentoRef, cRazonSocial, cRUCDNI, cDescripcionCliente, cIdSubDiario, cIdProvision) " & _
                        '                            " VALUES " & _
                        '                            " ('" & Mid(pstrCodigoUsuario, 3, 2) & "', '" & tOperacion & "', '" & vCorre & "', '" & fRegistro & "', '" & fVencimiento & "', " & _
                        '                            "  '" & Trim(rsOperacion!cIdCuenta) & "', '', '" & rsOperacion!cTipoDebeHaber & "', " & _
                        '                            "  '" & codCliente & "', '', '', " & importeSol & ", 0, '" & CStr(CInt(cCostoArea)) & "', 0, " & _
                        '                            "  " & tCambio & ", '" & CStr(CInt(cCostoLocal)) & "', '" & CStr(CInt(cCostoFuncion)) & "', " & _
                        '                            "  '" & tDocumento & "', '" & NroSerie + "-" + NroDocumento & "', " & _
                        '                            "  " & importeDol & ", '" & tMoneda & "', '" & tDocumentoRef & "', '" & nroSerieRef + IIf(nroSerieRef = "", "", "-") + nroDocumentoRef & "', " & _
                        '                            "'" & IIf(tDocumento = "03", "", rsClienteUnico!cCentroLaboral) & "', '" & rsClienteUnico!cNroDocumento & "', '" & Trim(rsClienteUnico!cApellidoPaterno) + " " + Trim(rsClienteUnico!cApellidoMaterno) + ", " + Trim(rsClienteUnico!cNombres) & "', '" & Trim(rsOperacion!cIdSubDiario) & "', '" & Trim(rsOperacion!cIdProvision) & "')"

                    Next
                    strLibro = Transaccion.IdTipoLibro
                    strCliente = Transaccion.IdCliente
                Else
                    Throw New Exception("No se generó ningún asiento contable.  Falta parametrizar.")
                End If
                'cnx.Close() 'Se necesita para ejercutar los DataReader

            Next
            scope.Complete()
            Return strNumeroAsiento
        End Using
        'cnx.Close() 'Se necesita para ejercutar los DataReader

        ' '' '' ''Try
        ' '' '' ''    cnx.Open()
        ' '' '' ''    'Return ds.Tables(0)
        ' '' '' ''    Return strNumeroAsiento
        ' '' '' ''    cnx.Close()
        ' '' '' ''Catch ex As Exception
        ' '' '' ''    Throw New Exception(ex.Message)
        ' '' '' ''End Try


        '    Dim rsOperacion As ADODB.Recordset
        '    Dim rsAsiento As ADODB.Recordset
        '    Dim rsClienteUnico As ADODB.Recordset
        '    '*RECUPERANDO ASIENTOS DE LA OPERACION A EJECUTAR
        '    rsOperacion = New ADODB.Recordset
        '    With rsOperacion
        '        .ActiveConnection = gstrConexion
        '        .CursorType = adOpenKeyset
        '        .CursorLocation = adUseClient
        '        .Source = "" & _
        '         " SELECT * FROM CUENTA_OPERACION " & _
        '         " WHERE cIdOperacion = '" & tOperacion & "' " & _
        '         " ORDER BY nNroOperacion "
        '        .LockType = adLockOptimistic
        '        .Open()
        '    End With
        '    '*FIN DE RECUERANDO ASIENTOS DE LA OPERACION A EJECUTAR

        '    '***INICIO DE LOS DATOS DEL PACIENTE****
        '    If pstrSistemaActual = "C" Then
        '        rsClienteUnico = New ADODB.Recordset
        '        With rsClienteUnico
        '            .ActiveConnection = gstrConexion
        '            .CursorType = adOpenKeyset
        '            .CursorLocation = adUseClient
        '            .Source = "" & _
        '             " SELECT * FROM PACIENTES " & _
        '             " WHERE cIdCliente = '" & codCliente & "'"
        '            .LockType = adLockOptimistic
        '            .Open()
        '        End With
        '        'codFichaOdonto
        '    Else

        '    End If
        '    codCliente = rsClienteUnico!cIdCliente
        '    '***FIN DE LA SOLICITUD DE LOS DATOS DEL PACIENTE****

        '    '***INICIO - PARTE NUEVA EN LA CREACION DEL CORRELATIVO***
        '    rsAsiento = New ADODB.Recordset
        '    With rsAsiento
        '        .ActiveConnection = gstrConexion
        '        .CursorType = adOpenStatic
        '        .CursorLocation = adUseClient
        '        '.Source = "Select max(cCorrelativo) as cCorrelativo from EXP_CONTA where fa99fecha='" & modFunciones.fFechaHora(True, False) & "'"
        '        .Source = "SELECT MAX(cCorrelativo) AS cCorrelativo FROM EXP_CONTA WHERE CONVERT (VARCHAR, dFechaTrans, 103) = '" & modFunciones.fFechaHora(True, False) & "'"
        '        .Open()
        '    End With
        '    Dim vCorre As String
        '    If IsNull(rsAsiento!cCorrelativo) Then vCorre = "0000" Else  : vCorre = Format(rsAsiento!cCorrelativo + 1, "0000")
        '    '***FIN - PARTE NUEVA EN LA CREACION DEL CORRELATIVO***


        '    Dim cn As ADODB.Connection
        '    Dim rs As ADODB.Recordset
        '    Dim cmd As Command
        '    Dim strSQL As String

        '    cn = New Connection
        '    rs = New Recordset
        '    cmd = New Command

        '    cn.CursorLocation = adUseClient
        '    cn.ConnectionString = gstrConexion
        '    cn.Open()

        '    '*POR AHORA SE TRABAJARA CON LA TABLA EXP_CONTA,
        '    '*SE PLANEA TRABAJAR CON UNA TABLA PROPIA ASIENTOCONTABLE

        '    '*OBTIENENDO LOS CENTROS DE COSTO
        '    strSQL = "" & _
        '    " SELECT cCentroCostoLocal, cCentroCostoArea, cCentroCostoFuncion  " & _
        '    " FROM   CENTRO_COSTO " & _
        '    " WHERE  cIdCentroCosto = '" & cCosto & "' "
        '    rs.Open(strSQL, cn, adOpenStatic, adLockReadOnly)
        '    cCostoLocal = rs(0)
        '    cCostoArea = rs(1)
        '    cCostoFuncion = rs(2)
        '    rs.Close()
        '    '*FIN DE OBTIENENDO LOS CENTROS DE COSTO

        '    '*INSERTANDO LOS ASIENTOS
        '    Do While Not rsOperacion.EOF
        '        If rsOperacion!cTipoIGV = "S" Then : monto = CDbl(igv) 'PARTE IGV
        '            If rsOperacion!cTipoIGV = "N" Then : monto = CDbl(importe) 'PARTE NORMAL
        '                If rsOperacion!cTipoIGV = "T" Then : monto = CDbl(importe) + CDbl(igv) 'TOTAL IGV+NORMAL
        '                    If rsOperacion!cTipoIGV = "I" Then : monto = CDbl(tSinDescuento) 'INTEGRO SIN DESCUENTO
        '                        If rsOperacion!cTipoIGV = "D" Then : monto = CDbl(tSinDescuento) - CDbl(importe) 'DESCUENTO INTEGRO-NORMAL
        '                            importeSol = IIf(tMoneda = "S", Round(IIf(tMoneda = "S", monto, monto * CDbl(tCambio)), 2), 0)
        '                            importeDol = IIf(tMoneda = "D", Round(IIf(tMoneda = "S", monto * 1.0# / CDbl(tCambio), monto), 2), 0)
        '                            tDocumento = IIf(Trim(tDocumento) = "FA", "01", IIf(Trim(tDocumento) = "BV", "03", IIf(Trim(tDocumento) = "NC", "04", IIf(Trim(tDocumento) = "ND", "05", tDocumento))))

        '                            strSQL = "" & _
        '                            " INSERT INTO EXP_CONTA " & _
        '                            " (cCajera, cTipoAsiento, cCorrelativo, dFechaTrans, dFechaVenc, cCuenta, " & _
        '                            "  cSubCuenta, cDebeHaber, cCuentaCte, cNroDocumento, cNroLetra, nImporte, " & _
        '                            "  nIGV, cCentroCosto, nImporteLetra, nTipoCambio, cCentroCostoV, cCentroCostoF, " & _
        '                            "  cTipoDocumento, cNroDocumen, nImporteDolar, cTipoMoneda, cTipoDocumentoRef, cNroDocumentoRef, cRazonSocial, cRUCDNI, cDescripcionCliente, cIdSubDiario, cIdProvision) " & _
        '                            " VALUES " & _
        '                            " ('" & Mid(pstrCodigoUsuario, 3, 2) & "', '" & tOperacion & "', '" & vCorre & "', '" & fRegistro & "', '" & fVencimiento & "', " & _
        '                            "  '" & Trim(rsOperacion!cIdCuenta) & "', '', '" & rsOperacion!cTipoDebeHaber & "', " & _
        '                            "  '" & codCliente & "', '', '', " & importeSol & ", 0, '" & CStr(CInt(cCostoArea)) & "', 0, " & _
        '                            "  " & tCambio & ", '" & CStr(CInt(cCostoLocal)) & "', '" & CStr(CInt(cCostoFuncion)) & "', " & _
        '                            "  '" & tDocumento & "', '" & NroSerie + "-" + NroDocumento & "', " & _
        '                            "  " & importeDol & ", '" & tMoneda & "', '" & tDocumentoRef & "', '" & nroSerieRef + IIf(nroSerieRef = "", "", "-") + nroDocumentoRef & "', " & _
        '                            "'" & IIf(tDocumento = "03", "", rsClienteUnico!cCentroLaboral) & "', '" & rsClienteUnico!cNroDocumento & "', '" & Trim(rsClienteUnico!cApellidoPaterno) + " " + Trim(rsClienteUnico!cApellidoMaterno) + ", " + Trim(rsClienteUnico!cNombres) & "', '" & Trim(rsOperacion!cIdSubDiario) & "', '" & Trim(rsOperacion!cIdProvision) & "')"


        '                            'strSQL = "" & _
        '    " insert into EXP_CONTA " & _
        '    " (cCajera, cTipoAsiento, cCorrelativo, dFechaTrans, dFechaVenc, cCuenta, " & _
        '    "  cSubCuenta, cDebeHaber, cCuentaCte, cNroDocumento, cNroLetra, nImporte, " & _
        '    "  nIGV, cCentroCosto, nImporteLetra, nTipoCambio, cCentroCostoV, cCentroCostoF, " & _
        '    "  cTipoDocumento, cNroDocumen, nImporteDolar, cTipoMoneda, cTipoDocumentoRef, cNroDocumentoRef, cRazonSocial, cRUCDNI, cDescripcionCliente, fa99sudi) " & _
        '    " values " & _
        '    " ('" & Mid(pstrCodigoUsuario, 3, 2) & "', '" & tOperacion & "', '" & vCorre & "', '" & fRegistro & "', '" & fVencimiento & "', " & _
        '    "  '" & Trim(rsOperacion!cIdCuenta) & "', '', '" & rsOperacion!cTipoDebeHaber & "', " & _
        '    "  '" & codCliente & "', '', '', " & importeSol & ", 0, '" & CStr(CInt(cCostoArea)) & "', 0, " & _
        '    "  " & tCambio & ", '" & CStr(CInt(cCostoLocal)) & "', '" & CStr(CInt(cCostoFuncion)) & "', " & _
        '    "  '" & tDocumento & "', '" & nroSerie + "-" + nroDocumento & "', " & _
        '    "  " & importeDol & ", '" & tMoneda & "', '" & tDocumentoRef & "', '" & nroSerieRef + IIf(nroSerieRef = "", "", "-") + nroDocumentoRef & "', " & _
        '    "'" & IIf(tDocumento = "03", "", rsClienteUnico!cCentroLaboral) & "', '" & rsClienteUnico!cNroDocumento & "', '" & Trim(rsClienteUnico!cApellidoPaterno) + " " + Trim(rsClienteUnico!cApellidoMaterno) + ", " + Trim(rsClienteUnico!cNombres) & "', '" & Trim(rsOperacion!cIdSubDiario) & "')"

        '                            '        strSQL = "" & _
        '    " insert into EXP_CONTA " & _
        '    " (cCajera, cTipoAsiento, cCorrelativo, dFechaTrans, dFechaVenc, cCuenta, " & _
        '    "  cSubCuenta, cDebeHaber, cCuentaCte, cNroDocumento, cNroLetra, nImporte, " & _
        '    "  nIGV, cCentroCosto, nImporteLetra, nTipoCambio, cCentroCostoV, cCentroCostoF, " & _
        '    "  cTipoDocumento, nserdoc, cNroDocumen, nImporteDolar, cTipoMoneda, cTipoDocumentoRef, nserdocref, cNroDocumentoRef, cRazonSocial, cRUCDNI, cDescripcionCliente, ndoctot) " & _
        '    " values " & _
        '    " ('" & Mid(pstrCodigoUsuario, 3, 2) & "', '" & tOperacion & "', '" & vCorre & "', '" & fRegistro & "', '" & fVencimiento & "', " & _
        '    "  '" & Trim(rsOperacion!cIdCuenta) & "', '', '" & rsOperacion!cTipoDebeHaber & "', " & _
        '    "  '" & codCliente & "', '', '', " & importeSol & ", 0, '" & CStr(CInt(cCostoArea)) & "', 0, " & _
        '    "  " & tCambio & ", '" & CStr(CInt(cCostoLocal)) & "', '" & CStr(CInt(cCostoFuncion)) & "', " & _
        '    "  '" & tDocumento & "', '" & nroSerie & "', '" & nroDocumento & "', " & _
        '    "  " & importeDol & ", '" & tMoneda & "', '" & tDocumentoRef & "', '" & nroSerieRef & "', '" & nroDocumentoRef & "', " & _
        '    "'" & IIf(tDocumento = "03", "", rsClienteUnico!cCentroLaboral) & "', '" & rsClienteUnico!cNroDocumento & "', '" & Trim(rsClienteUnico!cApellidoPaterno) + " " + Trim(rsClienteUnico!cApellidoMaterno) + ", " + Trim(rsClienteUnico!cNombres) & "', '" & nroSerie + "-" + nroDocumento & "')"
        '                            'Round(CDbl(tCambio), 4)
        '                            'pstrCodigoCliente = strSQL
        '                            strSQL = fPunto(strSQL)
        '                            cmd.CommandText = strSQL
        '                            cmd.CommandType = adCmdText
        '                            cmd.ActiveConnection = cn
        '                            cmd.Execute()
        '                            rsOperacion.MoveNext()
        'Loop
        '    '*FIN DE INSERTANDO LOS ASIENTOS

        '    rsOperacion.Close() : rsOperacion = Nothing
        '    rs.ActiveConnection = Nothing
        '    cn.Close()
        '    cn = Nothing



    End Function


    Public Function OperacionContableUpdate(ByVal lstTransaccion As List(Of stTransaccion), Optional ByVal bContinuar As Boolean = False, Optional ByVal NroAsiento As String = "") As String 'As DataTable
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strNumeroAsiento = ""

        For Each Transaccion In lstTransaccion
            Dim strGlosa As String = Transaccion.Glosa

            'Dim strSQL As String = "SELECT RIGHT ('0000' + CONVERT(VARCHAR(4), ISNULL (CONVERT (NUMERIC, (cNumeroAsiento)), 0) ), 4) cNumeroAsiento " & _

            Dim strSQL As String = "SELECT * " &
                                   "FROM CTBL_ASIENTO " &
                                   "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                                   "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                                   "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' " &
                                   "      AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdEmpresa = '" & Transaccion.IdEmpresa & "' " &
                                   "      AND cIdTipoMoneda = '" & Transaccion.IdTipoMoneda & "' " &
                                   "      AND cIdClienteProveedorAsiento = '" & Transaccion.IdCliente & "' AND cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & "' " &
                                   "      AND vIdNumeroSerieDocumentoAsiento = '" & Transaccion.NroSerie & "' AND vIdNumeroDocumentoAsiento = '" & Transaccion.NroDocumento & "' " &
                                   "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' AND cNumeroLineaAsiento = '00001' AND dFechaPagoAsiento = '" & Transaccion.FechaPago & "'"

            '"      AND cIdContratoMovimiento = '" & Transaccion.NroContrato & "' AND vNumeroLetraMovimiento = '" & Transaccion.NroDocumento & "' " & _
            '"      AND vIdNumeroSerieDocumentoAsiento = '" & Transaccion.NroSerie & "'"

            Dim strSQL2 As String = "SELECT cIdTipoMoneda FROM GNRL_TIPOMONEDA " &
                                   "WHERE bMonedaBaseTipoMoneda = 1 "



            'CASE WHEN /*TRANS.cIdTipoMoneda = 'D' AND*/ TRANS.cIdOrigenTipoCambioTransaccion = 'N' THEN TRANS.cIdNumeroCuentaPlanCuenta ELSE 
            ' (SELECT cIdNumeroCuentaPlanCuenta FROM GNRL_TRANSACCION 
            '  WHERE cIdFormaPago = '00 ' AND cIdTipoDocumento = 'LT' AND cIdBanco = 'BBVA'
            '        AND cIdTipoMoneda = 'D' AND cIdEmpresaTransaccion = '01' AND cIdPuntoVentaTransaccion = '01'
            '        AND (cIdGrupo = '0002' OR cIdGrupo IS NULL) 
            '        AND cIdNumeroCuentaPlanCuenta <> TRANS.cIdNumeroCuentaPlanCuenta
            '        AND cIdOrigenTipoCambioTransaccion = 'S') END AS cIdNumeroCuentaPlanCuentaye,


            'IIf(String.IsNullOrEmpty(Asiento.cIdTipoPersona) = True, " ", Asiento.cIdTipoPersona) '23 - Este se utiliza cuando es char.
            Dim strSQL3 As String = "SELECT TRANS.cIdFormaPago, TRANS.cIdTipoMoneda, CASE WHEN TRANS.cIdOrigenTipoCambioTransaccion = 'N' THEN " &
                                    "TRANS.cIdNumeroCuentaPlanCuenta ELSE ISNULL ((SELECT cIdNumeroCuentaPlanCuenta FROM GNRL_TRANSACCION WHERE cIdFormaPago = '" & Transaccion.IdOperacion & "' " &
                                   "      AND cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & IIf(Transaccion.IdBanco Is Nothing Or Transaccion.IdBanco = "", "' ", "' AND cIdBanco = '" & Transaccion.IdBanco.Trim & "' ") &
                                   "      AND cIdTipoMoneda = '" & Transaccion.IdTipoMonedaOrigen & "' AND cIdEmpresaTransaccion = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVentaTransaccion = '" & Transaccion.IdPuntoVenta & "' " &
                                   "      AND (cIdGrupo = '" & Transaccion.IdGrupo & "' OR cIdGrupo IS NULL) AND cIdNumeroCuentaPlanCuenta <> TRANS.cIdNumeroCuentaPlanCuenta " &
                                   "      AND cIdOrigenTipoCambioTransaccion = 'S'), TRANS.cIdNumeroCuentaPlanCuenta) END AS cIdNumeroCuentaPlanCuenta, TRANS.cIdPuntoVentaPlanCuenta, TRANS.cIdEmpresaPlanCuenta, " &
                                    "       TRANS.cIdEmpresaTransaccion, TRANS.cIdPuntoVentaTransaccion, TRANS.bEstadoRegistroTransaccion, TRANS.cIdTipoTransaccion, TRANS.cIdGrupo, " &
                                    "       TRANS.cIdTransaccion, TRANS.vDescripcionTransaccion, TRANS.vDescripcionAbreviadaTransaccion, TRANS.cIdBanco, TRANS.cIdTipoDocumento, " &
                                    "       TRANS.cIdTipoDebeHaberTransaccion, TRANS.cIdTipoIGVTransaccion, TRANS.nOrdenTransaccion, TRANS.cIdOrigenTipoCambioTransaccion " &
                                    "FROM GNRL_TRANSACCION TRANS " &
                                   "WHERE TRANS.cIdFormaPago = '" & Transaccion.IdOperacion & "' " &
                                   IIf(Transaccion.IdTipoDocumento = Nothing, " AND (TRANS.cIdTipoDocumento = '' OR TRANS.cIdTipoDocumento IS NULL)", "      AND TRANS.cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & "'") & IIf(Transaccion.IdBanco.Trim = "", " ", " AND TRANS.cIdBanco = '" & Transaccion.IdBanco.Trim & "' ") &
                                   "      AND TRANS.cIdTipoMoneda = '" & Transaccion.IdTipoMoneda & "' AND TRANS.cIdEmpresaTransaccion = '" & Transaccion.IdEmpresa & "' AND TRANS.cIdPuntoVentaTransaccion = '" & Transaccion.IdPuntoVenta & "' " &
                                   "      AND (TRANS.cIdGrupo = '" & Transaccion.IdGrupo & "' OR TRANS.cIdGrupo IS NULL)" &
                                   "ORDER BY TRANS.nOrdenTransaccion"
            Dim cmd As New SqlCommand(strSQL, cnx)
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet

            da.Fill(ds, "Asiento")

            If ds.Tables("Asiento").Rows.Count > 0 Then
                strNumeroAsiento = ds.Tables("Asiento").Rows(0).Item("cNumeroAsiento")
            Else
                Dim lstNewTransaccion As New List(Of stTransaccion)
                lstNewTransaccion.Add(Transaccion)
                'Call OperacionContableInserta(Transaccion)
                Call OperacionContableInserta(lstNewTransaccion)
                Return ""
                Exit Function
            End If
            'strNumeroAsiento = ds.Tables("Asiento").Rows(0).Item("cNumeroAsiento")
            If bContinuar = True Or (bContinuar = False And NroAsiento <> "") Then
                strNumeroAsiento = NroAsiento
            Else

            End If

            'Transaccion.FechaVencimiento = ds.Tables("Asiento").Rows(0).Item("dFechaVencimientoAsiento")


            'Dim strGlosa As String = ds.Tables("Asiento").Rows(0).Item("vGlosaAsiento")

            'If bContinuar = True Or (bContinuar = False And NroAsiento <> "") Then
            '    strNumeroAsiento = NroAsiento
            'Else
            '    strNumeroAsiento = ds.Tables("Asiento").Rows(0).Item("cNumeroAsiento")
            'End If


            cmd = New SqlCommand(strSQL2, cnx)
            da = New SqlDataAdapter(cmd)

            da.Fill(ds, "TipoMoneda")

            Dim strTipoMoneda = ds.Tables("TipoMoneda").Rows(0).Item("cIdTipoMoneda")

            cmd = New SqlCommand(strSQL3, cnx)
            da = New SqlDataAdapter(cmd)

            da.Fill(ds, "Transaccion")

            Dim ImporteTotalMNDebe As Decimal = 0
            Dim ImporteTotalMNHaber As Decimal = 0
            If ds.Tables("Transaccion").Rows.Count > 0 Then
                Dim PosicionFila As String
                If bContinuar = True Or (bContinuar = False And NroAsiento <> "") Then
                    'strNumeroAsiento = NroAsiento
                    Dim strSQL4 As String = "SELECT RIGHT ('00000' + CONVERT(VARCHAR(5), ISNULL (CONVERT (NUMERIC, MAX(cNumeroLineaAsiento)), 0) + 1), 5) cNumeroLineaAsiento " &
                           "FROM CTBL_ASIENTO " &
                           "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                           "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                           "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' AND cNumeroAsiento = '" & NroAsiento & "'"

                    cmd = New SqlCommand(strSQL4, cnx)
                    da = New SqlDataAdapter(cmd)

                    da.Fill(ds, "NroLineaAsiento")
                    PosicionFila = ds.Tables("NroLineaAsiento").Rows(0).Item("cNumeroLineaAsiento")
                Else
                    PosicionFila = "00001"
                End If

                Dim st As New clsFuncionesMetodos.stAsiento
                Dim fila As DataRow
                Dim Diferencia As Decimal = 0
                For Each fila In ds.Tables("Transaccion").Rows
                    'Dim PosicionFila = Mid(Convert.ToString(ds.Tables("Transaccion").Rows.IndexOf(fila) + 100001), 2)

                    Dim Importe As Decimal = 0
                    Dim ImporteMN As Decimal = 0
                    Dim ImporteME As Decimal = 0

                    '        If rsOperacion!cTipoIGV = "S" Then : monto = CDbl(igv) 'PARTE IGV
                    '            If rsOperacion!cTipoIGV = "N" Then : monto = CDbl(importe) 'PARTE NORMAL
                    '                If rsOperacion!cTipoIGV = "T" Then : monto = CDbl(importe) + CDbl(igv) 'TOTAL IGV+NORMAL
                    '                    If rsOperacion!cTipoIGV = "I" Then : monto = CDbl(tSinDescuento) 'INTEGRO SIN DESCUENTO
                    '                        If rsOperacion!cTipoIGV = "D" Then : monto = CDbl(tSinDescuento) - CDbl(importe) 'DESCUENTO INTEGRO-NORMAL
                    '                            importeSol = IIf(tMoneda = "S", Round(IIf(tMoneda = "S", monto, monto * CDbl(tCambio)), 2), 0)
                    '                            importeDol = IIf(tMoneda = "D", Round(IIf(tMoneda = "S", monto * 1.0# / CDbl(tCambio), monto), 2), 0)
                    '                            tDocumento = IIf(Trim(tDocumento) = "FA", "01", IIf(Trim(tDocumento) = "BV", "03", IIf(Trim(tDocumento) = "NC", "04", IIf(Trim(tDocumento) = "ND", "05", tDocumento))))
                    If fila("cIdTipoIGVTransaccion") = "S" Then Importe = Convert.ToDecimal(Transaccion.IGV) 'PARTE IGV
                    If fila("cIdTipoIGVTransaccion") = "N" Then Importe = Convert.ToDecimal(Transaccion.Importe) 'PARTE NORMAL
                    If fila("cIdTipoIGVTransaccion") = "T" Then Importe = Convert.ToDecimal(Transaccion.Importe) + Convert.ToDecimal(Transaccion.IGV) 'TOTAL IGV+NORMAL
                    If fila("cIdTipoIGVTransaccion") = "I" Then Importe = Convert.ToDecimal(Transaccion.ImporteSinDescuento) 'INTEGRO SIN DESCUENTO
                    If fila("cIdTipoIGVTransaccion") = "D" Then Importe = Convert.ToDecimal(Transaccion.ImporteSinDescuento) - Convert.ToDecimal(Transaccion.Importe) 'DESCUENTO INTEGRO-NORMAL

                    Dim TipoCambioDoc As Decimal = 0.0
                    If fila("cIdOrigenTipoCambioTransaccion") = "S" Then 'Busca el Tipo de Cambio del Documento Original.
                        Dim strSQL4 As String = "SELECT vValor FROM GNRL_TABLASISTEMA " &
                                                "WHERE cIdTablaSistema = '13' AND vValorOpcionalTablaSistema = 'S'"
                        cmd = New SqlCommand(strSQL4, cnx)
                        da = New SqlDataAdapter(cmd)

                        da.Fill(ds, "TipoEmpresa")

                        Dim TipoEmpresa = ds.Tables("TipoEmpresa").Rows(0).Item("vValor")
                        Dim strSQL5 As String = ""


                        If TipoEmpresa = "01" And (Transaccion.IdTipoDocumento <> "ND" And Transaccion.IdTipoDocumento <> "NC") Then 'Empresa Inmobiliaria.

                            'strSQL5 = "SELECT CASE WHEN YEAR(CON.dFechaContrato) = YEAR('" & Transaccion.FechaRegistro & "') THEN CON.nTipoCambioContrato ELSE " & _
                            '          "(SELECT nTipoCambioCompra FROM GNRL_TIPOCAMBIO WHERE CONVERT (CHAR(10), (dFechaHoraTipoCambio), 103) = '31/12/' + CONVERT(CHAR(4), YEAR('" & Transaccion.FechaRegistro & "')-1)) END AS nTipoCambioDoc " & _
                            '          "FROM VTAS_CONTRATO CON " & _
                            '                        "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdContrato = '" & Transaccion.NroContrato & "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'"

                            strSQL5 = "SELECT CASE WHEN YEAR(CON.dFechaContrato) = YEAR('" & Transaccion.FechaRegistro & "') THEN CON.nTipoCambioContrato ELSE " &
                                      "(SELECT " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", "nTipoCambioVenta", "nTipoCambioCompra") & " FROM GNRL_TIPOCAMBIO WHERE CONVERT (CHAR(10), (dFechaHoraTipoCambio), 103) = '31/12/' + CONVERT(CHAR(4), YEAR('" & Transaccion.FechaRegistro & "')-1)) END AS nTipoCambioDoc " &
                                      "FROM VTAS_CONTRATO CON " &
                                                    "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdContrato = '" & Transaccion.NroContrato & "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'"

                            '                        SELECT vCabeceraBalance1, cIdNumeroCuentaBalance1 FROM CTBL_BALANCE WHERE cIdPeriodoBalance = '2013' AND cIdTipoBalance1 = 1 AND RTRIM(LTRIM(cIdNumeroCuentaBalance1)) <> ''
                            'UNION
                            'SELECT vCabeceraBalance2, cIdNumeroCuentaBalance2 FROM CTBL_BALANCE WHERE cIdPeriodoBalance = '2013' AND cIdTipoBalance1 = 1 AND RTRIM(LTRIM(cIdNumeroCuentaBalance2)) <> ''

                            cmd = New SqlCommand(strSQL5, cnx)
                            da = New SqlDataAdapter(cmd)

                            da.Fill(ds, "TipoCambioDoc")

                            TipoCambioDoc = ds.Tables("TipoCambioDoc").Rows(0).Item("nTipoCambioDoc")
                        ElseIf TipoEmpresa = "01" And (Transaccion.IdTipoDocumento = "ND" Or Transaccion.IdTipoDocumento = "NC") Then 'Empresa Inmobiliaria.
                            strSQL5 = "SELECT CASE WHEN YEAR(DOC.dFechaEmisionCabDoc) = YEAR('" & Transaccion.FechaRegistro & "') THEN DOC.nTipoCambioCabDoc ELSE " &
                                      "(SELECT " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", "nTipoCambioVenta", "nTipoCambioCompra") & " FROM GNRL_TIPOCAMBIO WHERE CONVERT (CHAR(10), (dFechaHoraTipoCambio), 103) = '31/12/' + CONVERT(CHAR(4), YEAR('" & Transaccion.FechaRegistro & "')-1)) END AS nTipoCambioDoc " &
                                      "FROM VTAS_CABECERADOCUMENTO DOC " &
                                                    "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND vIdNumeroCorrelativoCabDoc = '" & Transaccion.NroContrato & "' AND cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & "'"
                            cmd = New SqlCommand(strSQL5, cnx)
                            da = New SqlDataAdapter(cmd)

                            da.Fill(ds, "TipoCambioDoc")

                            If ds.Tables("TipoCambioDoc").Rows.Count > 0 Then
                                TipoCambioDoc = ds.Tables("TipoCambioDoc").Rows(0).Item("nTipoCambioDoc")
                            Else
                                TipoCambioDoc = Transaccion.TipoCambio
                            End If
                            'TipoCambioDoc = ds.Tables("TipoCambioDoc").Rows(0).Item("nTipoCambioDoc")
                        ElseIf TipoEmpresa = "02" Then 'Empresa Comercial.
                            'FALTA MAS PUNTOS PARA EL TEMA DE LA EMPRESA COMERCIAL.
                            strSQL5 = "SELECT DOC.nTipoCambioCabDoc FROM VTAS_CABECERADOCUMENTO DOC " &
                                                    "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND vIdNumeroCorrelativoCabDoc = '" & Transaccion.NroDocumentoRef & "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'"
                            cmd = New SqlCommand(strSQL5, cnx)
                            da = New SqlDataAdapter(cmd)

                            da.Fill(ds, "TipoCambioDoc")

                            TipoCambioDoc = ds.Tables("TipoCambioDoc").Rows(0).Item("nTipoCambioContrato")
                        End If
                    End If

                    ImporteMN = Math.Round(IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Importe, Importe * IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio)), 2)
                    ImporteME = Math.Round(IIf(Transaccion.IdTipoMoneda <> strTipoMoneda, Importe, Importe / IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio)), 2)

                    'If bContinuar = True Or (bContinuar = False And NroAsiento <> "") Then
                    '    strNumeroAsiento = NroAsiento
                    'Else
                    '    If fila("cIdTipoDebeHaberTransaccion") = "D" Then
                    '        ImporteTotalMNDebe = ImporteTotalMNDebe + ImporteMN
                    '    Else
                    '        ImporteTotalMNHaber = ImporteTotalMNHaber + ImporteMN
                    '    End If
                    'End If
                    'If ds.Tables("Transaccion").Rows = fila("nOrdenTransaccion") = "1" Then
                    'ImporteMN = IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Math.Round(IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Importe, Importe * Transaccion.TipoCambio), 2), 0)
                    'ImporteME = IIf(Transaccion.IdTipoMoneda <> strTipoMoneda, Math.Round(IIf(Transaccion.IdTipoMoneda = strTipoMoneda, Importe, Importe / Transaccion.TipoCambio), 2), 0)

                    'OBTENIENDO LOS CENTROS DE COSTO
                    strSQL = "SELECT * FROM CTBL_CENTROCOSTO " &
                             "WHERE cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "'" &
                             "      AND cIdArea = '" & Transaccion.IdArea & IIf(Transaccion.IdGrupo = "", "'", "' AND cIdGrupo = '" & Transaccion.IdGrupo & "'")

                    cmd = New SqlCommand(strSQL, cnx)
                    da = New SqlDataAdapter(cmd)

                    da.Fill(ds, "CentroCosto")

                    If ds.Tables("CentroCosto").Rows.Count > 0 Then

                        'Dim strCentroCostoLocal = ds.Tables("CentroCosto").Rows(0).Item("cIdCentroCostoLocal")
                        'Dim strCentroCostoArea = ds.Tables("CentroCosto").Rows(0).Item("cIdCentroCostoArea")
                        'Dim strCentroCostoFuncion = ds.Tables("CentroCosto").Rows(0).Item("cIdCentroCostoFuncion")
                        Dim strCentroCosto = ds.Tables("CentroCosto").Rows(0).Item("cIdCentroCosto")
                        strSQL = "SELECT * FROM CTBL_PLANCUENTA " &
                                 "WHERE cIdNumeroCuentaPlanCuenta = '" & fila("cIdNumeroCuentaPlanCuenta") & "' " &
                                 "      AND cIdEmpresa = '" & fila("cIdEmpresaTransaccion") & "' " &
                                 "      AND cIdPuntoVenta = '" & fila("cIdPuntoVentaTransaccion") & "'"

                        cmd = New SqlCommand(strSQL, cnx)
                        da = New SqlDataAdapter(cmd)

                        da.Fill(ds, "PlanCuenta")

                        Dim strAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cAmarreAutomaticoPlanCuenta")
                        Dim strDebeAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cDebePlanCuenta")
                        Dim strHaberAmarre = ds.Tables("PlanCuenta").Rows(0).Item("cHaberPlanCuenta")

                        Transaccion.Glosa = fila("vDescripcionAbreviadaTransaccion") & "-" & strGlosa
                        'Transaccion.Glosa = strGlosa
                        If Importe <> 0 Then
                            strSQL = "UPDATE CTBL_ASIENTO " &
                                     "SET cNumeroLineaAsiento = '" & PosicionFila & "'," &
                                     "    cIdTipoMoneda = '" & Transaccion.IdTipoMoneda & "'," &
                                     "    dFechaDocumentoAsiento = '" & Transaccion.FechaRegistro & "'," &
                                     "    dFechaVencimientoAsiento = '" & Transaccion.FechaVencimiento & "'," &
                                     "    vGlosaAsiento = '" & Transaccion.Glosa & "'," &
                                     "    nDebeMNAsiento = " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteMN, 0) & "," &
                                     "    nHaberMNAsiento = " & IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteMN, 0) & "," &
                                     "    nDebeMEAsiento = " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteME, 0) & "," &
                                     "    nHaberMEAsiento = " & IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteME, 0) & "," &
                                     "    nTipoCambioAsiento = " & IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio) & "," &
                                     "    cIdTipoAmarreAsiento = ''," &
                                     "    cIdCentroCosto = '" & strCentroCosto & "'," &
                                     "    dFechaPagoAsiento = '" & Transaccion.FechaPago & "'," &
                                     "    vRazonSocialAsiento = '" & Transaccion.DescripcionClienteProveedor & "'," &
                                     "    cIdTipoDocumentoClienteProveedorAsiento = '" & Transaccion.IdTipoDocClienteProveedor & "'," &
                                     "    cIdAuxiliar = '" & Transaccion.IdAuxiliar & "'," &
                                     "    bEstadoRegistroAsiento = '" & Transaccion.EstadoRegistro & "'," &
                                     "    cIdTipoOperacion = " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, ", "'" & Transaccion.IdTipoOperacion & "',") &
                                     "    cNumeroPagoAsiento = '" & Transaccion.NroPago & "'," &
                                     "    cIdMedioPago = " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "',") &
                                     "    cIdTipoDocumentoRefAsiento = '" & Transaccion.IdTipoDocumentoRef & "'," &
                                     "    vIdNumeroSerieDocumentoRefAsiento = '" & Transaccion.NroSerieRef & "'," &
                                     "    vIdNumeroDocumentoRefAsiento = '" & Transaccion.NroDocumentoRef & "'," &
                                     "    dFechaDocumentoRefAsiento = " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " &
                                     "    cIdProducto = " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL ", "'" & Transaccion.IdProducto & "'") & "" &
                            " WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' AND " &
                            "       cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' AND cNumeroAsiento = '" & strNumeroAsiento & "' AND cIdClienteProveedorAsiento = '" & Transaccion.IdCliente & "' AND " &
                            "       cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdTipoPersona = '" & Transaccion.IdTipoPersona & "' AND " &
                            "       cIdEmpresa = '" & Transaccion.IdEmpresa & "' AND cIdTipoDocumento = '" & Transaccion.IdTipoDocumento & "' AND " &
                            "       vIdNumeroSerieDocumentoAsiento = '" & Transaccion.NroSerie & "' AND vIdNumeroDocumentoAsiento = '" & Transaccion.NroDocumento & "' AND " &
                            "       cIdNumeroCuentaAsiento = '" & fila("cIdNumeroCuentaPlanCuenta") & "'"



                            'strSQL = "INSERT INTO CTBL_ASIENTO " & _
                            '         "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " & _
                            '         " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " & _
                            '         " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " & _
                            '         " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " & _
                            '         " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " & _
                            '         " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion," & _
                            '         " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " & _
                            '         " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento) " & _
                            '         "VALUES " & _
                            '         "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" & _
                            '         Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', '" & Transaccion.IdCliente & "', '" & _
                            '         Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" & _
                            '         PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', '" & Transaccion.IdTipoDocumento & "', '" & _
                            '         Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaRegistro & "', '" & Transaccion.FechaVencimiento & "', '" & _
                            '         Transaccion.Glosa & "', '" & fila("cIdNumeroCuentaPlanCuenta") & "', " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteMN, 0) & ", " & _
                            '         IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteMN, 0) & ", " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteME, 0) & ", " & _
                            '         IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteME, 0) & ", " & IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", TipoCambioDoc, Transaccion.TipoCambio) & ", '" & _
                            '         "" & "', '" & strCentroCosto & "', '" & _
                            '         Transaccion.FechaPago & "', '" & Transaccion.DescripcionClienteProveedor & "', '" & _
                            '         Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', '" & Transaccion.IdTipoOperacion & "', '" & _
                            '         Transaccion.NroPago & "', '" & Transaccion.IdMedioPago & "', '" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" & _
                            '         Transaccion.NroDocumentoRef & "', '" & Transaccion.FechaDocumentoRef & "')"
                            '"" & "', '" & strCentroCostoArea & "', '" & strCentroCostoFuncion & "', '" & strCentroCostoLocal & "', '" & _

                            cmd = New SqlCommand(strSQL, cnx)
                            cmd.Connection.Open()
                            cmd.ExecuteNonQuery()
                            cmd.Connection.Close()
                        End If
                        'cmd.BeginExecuteNonQuery() 'Se necesita conexión Asincrona
                        'cmd.EndExecuteNonQuery()
                        'da = New SqlDataAdapter(cmd)




                        'If bContinuar = True Or (bContinuar = False And NroAsiento <> "") Then
                        If bContinuar = False And NroAsiento <> "" Then
                            strSQL = "SELECT ISNULL(SUM(nDebeMNAsiento), 0) nDebeMNTotalAsiento, ISNULL(SUM(nHaberMNAsiento), 0) nHaberMNTotalAsiento " &
                                     "FROM CTBL_ASIENTO " &
                                     "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                                     "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                                     "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' " &
                                     "      AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdEmpresa = '" & Transaccion.IdEmpresa & "' " &
                                     "      AND cNumeroAsiento = '" & strNumeroAsiento & "'"

                            cmd = New SqlCommand(strSQL, cnx)
                            da = New SqlDataAdapter(cmd)

                            da.Fill(ds, "DiferenciaAsiento")
                            ImporteTotalMNDebe = ds.Tables("DiferenciaAsiento").Rows(0).Item("nDebeMNTotalAsiento")
                            ImporteTotalMNHaber = ds.Tables("DiferenciaAsiento").Rows(0).Item("nHaberMNTotalAsiento")
                            ds.Tables("DiferenciaAsiento").Clear()
                        Else
                            If fila("cIdTipoDebeHaberTransaccion") = "D" Then
                                ImporteTotalMNDebe = ImporteTotalMNDebe + ImporteMN
                            Else
                                ImporteTotalMNHaber = ImporteTotalMNHaber + ImporteMN
                            End If
                        End If

                        If ds.Tables("Transaccion").Rows.Count.ToString = fila("nOrdenTransaccion") And (ImporteMN <> 0 Or ImporteME <> 0) Then
                            Diferencia = ImporteTotalMNDebe - ImporteTotalMNHaber
                            If Convert.ToDecimal(Diferencia) > 0 Then 'Va en el Haber
                                fila("cIdNumeroCuentaPlanCuenta") = TablaSistemaNeg.TablaSistemaListarPorId("06", "01", "CTBL", lstTransaccion.Item(0).IdEmpresa, lstTransaccion.Item(0).IdPuntoVenta).vValorOpcionalTablaSistema 'Ganancia
                                fila("cIdTipoDebeHaberTransaccion") = "H"
                                ImporteMN = Diferencia
                            ElseIf Convert.ToDecimal(Diferencia) < 0 Then 'Va en el Debe
                                fila("cIdNumeroCuentaPlanCuenta") = TablaSistemaNeg.TablaSistemaListarPorId("06", "02", "CTBL", lstTransaccion.Item(0).IdEmpresa, lstTransaccion.Item(0).IdPuntoVenta).vValorOpcionalTablaSistema 'Perdida
                                ImporteMN = Diferencia * -1
                                fila("cIdTipoDebeHaberTransaccion") = "D"
                            End If

                            strSQL = "SELECT * FROM CTBL_PLANCUENTA " &
                                     "WHERE cIdNumeroCuentaPlanCuenta = '" & fila("cIdNumeroCuentaPlanCuenta") & "' " &
                                     "      AND cIdEmpresa = '" & fila("cIdEmpresaTransaccion") & "' " &
                                     "      AND cIdPuntoVenta = '" & fila("cIdPuntoVentaTransaccion") & "'"

                            cmd = New SqlCommand(strSQL, cnx)
                            da = New SqlDataAdapter(cmd)

                            da.Fill(ds, "PlanCuenta2")

                            strAmarre = ds.Tables("PlanCuenta2").Rows(0).Item("cAmarreAutomaticoPlanCuenta")
                            strDebeAmarre = ds.Tables("PlanCuenta2").Rows(0).Item("cDebePlanCuenta")
                            strHaberAmarre = ds.Tables("PlanCuenta2").Rows(0).Item("cHaberPlanCuenta")

                            cmd = New SqlCommand("DELETE " &
                                                 "FROM CTBL_ASIENTO " &
                                                 "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                                                 "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                                                 "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' " &
                                                 "      AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdEmpresa = '" & Transaccion.IdEmpresa & "' " &
                                                 "      AND cIdNumeroCuentaAsiento IN ('" & TablaSistemaNeg.TablaSistemaListarPorId("06", "01", "CTBL", lstTransaccion.Item(0).IdEmpresa, lstTransaccion.Item(0).IdPuntoVenta).vValorOpcionalTablaSistema & "', " &
                                                 "      '" & TablaSistemaNeg.TablaSistemaListarPorId("06", "02", "CTBL", lstTransaccion.Item(0).IdEmpresa, lstTransaccion.Item(0).IdPuntoVenta).vValorOpcionalTablaSistema & "') " &
                                                 "      AND cNumeroAsiento = '" & strNumeroAsiento & "'", cnx)
                            cmd.Connection.Open()
                            cmd.ExecuteNonQuery()
                            cmd.Connection.Close()

                            'Opcion nueva
                            Transaccion.Glosa = strGlosa
                            'Generar Asiento por diferencia de tipo de cambio.
                            If Convert.ToDecimal(Diferencia) <> 0 And bContinuar = False Then 'And (ImporteMN <> 0 Or ImporteME <> 0) Then
                                If InStrRev(Transaccion.Glosa, "CANCELACION", -1) = 1 Then
                                    'Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, InStrRev(Transaccion.Glosa, Transaccion.Glosa.Length - 10, -1))
                                    Transaccion.Glosa = "DIFERENCIA T.C. " & Mid(Transaccion.Glosa, 13, Transaccion.Glosa.Length)
                                End If
                                PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2)
                                strSQL = "INSERT INTO CTBL_ASIENTO " &
                                            "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " &
                                            " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " &
                                            " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " &
                                            " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " &
                                            " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " &
                                            " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion, " &
                                            " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " &
                                            " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento, cIdProducto) " &
                                            "VALUES " &
                                            "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" &
                                            Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', " & IIf(IsNothing(Transaccion.IdCliente) = True, "NULL, ", "'" & Transaccion.IdCliente & "', ") & "'" &
                                            Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" &
                                            PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & Transaccion.IdTipoDocumento & "', ") & "'" &
                                            Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaRegistro & "', '" & Transaccion.FechaVencimiento & "', '" &
                                            Transaccion.Glosa & "', '" & fila("cIdNumeroCuentaPlanCuenta") & "', " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", ImporteMN, 0) & ", " &
                                            IIf(fila("cIdTipoDebeHaberTransaccion") = "H", ImporteMN, 0) & ", " & IIf(fila("cIdTipoDebeHaberTransaccion") = "D", 0, 0) & ", " &
                                            IIf(fila("cIdTipoDebeHaberTransaccion") = "H", 0, 0) & ", " & IIf(fila("cIdOrigenTipoCambioTransaccion") = "S", 0, 0) & ", '" &
                                            "D" & "', '" & strCentroCosto & "', '" &
                                            Transaccion.FechaPago & "', '" & Transaccion.DescripcionClienteProveedor & "', '" &
                                            Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, '", "'" & Transaccion.IdTipoOperacion & "', '") &
                                            Transaccion.NroPago & "', " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "', ") & "'" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" &
                                            Transaccion.NroDocumentoRef & "', " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL", "'" & Transaccion.IdProducto & "'") & ")"
                                cmd = New SqlCommand(strSQL, cnx)
                                cmd.Connection.Open()
                                cmd.ExecuteNonQuery()
                                cmd.Connection.Close()
                            End If
                        End If

                        If bContinuar = False Then
                            cmd = New SqlCommand("DELETE " &
                                                 "FROM CTBL_ASIENTO " &
                                                 "WHERE cIdPeriodoAsiento = '" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "' " &
                                                 "      AND cIdMesAsiento = '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "' " &
                                                 "      AND cIdTipoLibro = '" & Transaccion.IdTipoLibro & "' " &
                                                 "      AND cIdPuntoVenta = '" & Transaccion.IdPuntoVenta & "' AND cIdEmpresa = '" & Transaccion.IdEmpresa & "' " &
                                                 "      AND cNumeroAsiento = '" & strNumeroAsiento & "' AND ISNULL(cIdTipoAmarreAsiento, '') = 'A'", cnx)
                            cmd.Connection.Open()
                            cmd.ExecuteNonQuery()
                            cmd.Connection.Close()
                        End If

                        If Not TypeOf (strAmarre) Is DBNull And bContinuar = False Then
                            If strAmarre = "S" Then
                                PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2)

                                'Dim PosicionFila = Mid(Convert.ToString(ds.Tables("Transaccion").Rows.IndexOf(fila) + 100001), 2)
                                '" vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " & _

                                strSQL = "INSERT INTO CTBL_ASIENTO " &
                                 "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " &
                                 " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " &
                                 " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " &
                                 " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " &
                                 " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " &
                                 " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion, " &
                                 " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " &
                                 " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento, cIdProducto) " &
                                 "VALUES " &
                                 "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" &
                                 Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', " & IIf(IsNothing(Transaccion.IdCliente) = True, "NULL, ", "'" & Transaccion.IdCliente & "', ") & "'" &
                                 Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" &
                                 PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & Transaccion.IdTipoDocumento & "', ") & "'" &
                                 Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaRegistro & "', '" & Transaccion.FechaVencimiento & "', '" &
                                 Transaccion.Glosa & "', '" & strDebeAmarre & "', " & ImporteMN & ", " &
                                 "0" & ", " & "0" & ", " &
                                 "0" & ", " & "0" & ", '" &
                                 "A" & "', '" & strCentroCosto & "', '" &
                                 Transaccion.FechaPago & "', '" & Transaccion.DescripcionClienteProveedor & "', '" &
                                 Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, '", "'" & Transaccion.IdTipoOperacion & "', '") &
                                 Transaccion.NroPago & "', " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "', ") & "'" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" &
                                 Transaccion.NroDocumentoRef & "', " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL", "'" & Transaccion.IdProducto & "'") & ")"

                                cmd = New SqlCommand(strSQL, cnx)
                                cmd.Connection.Open()
                                cmd.ExecuteNonQuery()
                                cmd.Connection.Close()

                                PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2)
                                strSQL = "INSERT INTO CTBL_ASIENTO " &
                                         "(cIdPeriodoAsiento, cIdMesAsiento, cIdTipoLibro, cNumeroAsiento, cIdClienteProveedorAsiento, " &
                                         " cIdPuntoVenta, cIdTipoPersona, cIdEmpresa, cNumeroLineaAsiento, cIdTipoMoneda, cIdTipoDocumento, " &
                                         " vIdNumeroSerieDocumentoAsiento, vIdNumeroDocumentoAsiento, dFechaDocumentoAsiento, dFechaVencimientoAsiento, vGlosaAsiento, " &
                                         " cIdNumeroCuentaAsiento, nDebeMNAsiento, nHaberMNAsiento, nDebeMEAsiento, nHaberMEAsiento, " &
                                         " nTipoCambioAsiento, cIdTipoAmarreAsiento, cIdCentroCosto, dFechaPagoAsiento, vRazonSocialAsiento, " &
                                         " cIdTipoDocumentoClienteProveedorAsiento, cIdAuxiliar, bEstadoRegistroAsiento, cIdTipoOperacion, " &
                                         " cNumeroPagoAsiento, cIdMedioPago, cIdTipoDocumentoRefAsiento, vIdNumeroSerieDocumentoRefAsiento, " &
                                         " vIdNumeroDocumentoRefAsiento, dFechaDocumentoRefAsiento, cIdProducto) " &
                                         "VALUES " &
                                         "('" & Convert.ToString(Year(Transaccion.FechaRegistro)) & "', '" & String.Format("{0:00}", Month(Transaccion.FechaRegistro)) & "', '" &
                                         Transaccion.IdTipoLibro & "', '" & strNumeroAsiento & "', " & IIf(IsNothing(Transaccion.IdCliente) = True, "NULL, ", "'" & Transaccion.IdCliente & "', ") & "'" &
                                         Transaccion.IdPuntoVenta & "', '" & Transaccion.IdTipoPersona & "', '" & Transaccion.IdEmpresa & "', '" &
                                         PosicionFila & "', '" & Transaccion.IdTipoMoneda & "', " & IIf(IsNothing(Transaccion.IdTipoDocumento) = True, "NULL, ", "'" & Transaccion.IdTipoDocumento & "', ") & "'" &
                                         Transaccion.NroSerie & "', '" & Transaccion.NroDocumento & "', '" & Transaccion.FechaRegistro & "', '" & Transaccion.FechaVencimiento & "', '" &
                                         Transaccion.Glosa & "', '" & strHaberAmarre & "', " & "0" & ", " &
                                         ImporteMN & ", " & "0" & ", " &
                                         "0" & ", " & "0" & ", '" &
                                         "A" & "', '" & strCentroCosto & "', '" &
                                         Transaccion.FechaPago & "', '" & Transaccion.DescripcionClienteProveedor & "', '" &
                                         Transaccion.IdTipoDocClienteProveedor & "', '" & Transaccion.IdAuxiliar & "', '" & Transaccion.EstadoRegistro & "', " & IIf(IsNothing(Transaccion.IdTipoOperacion) = True, "NULL, '", "'" & Transaccion.IdTipoOperacion & "', '") &
                                         Transaccion.NroPago & "', " & IIf(IsNothing(Transaccion.IdMedioPago) = True, "NULL, ", "'" & Transaccion.IdMedioPago & "', ") & "'" & Transaccion.IdTipoDocumentoRef & "', '" & Transaccion.NroSerieRef & "', '" &
                                         Transaccion.NroDocumentoRef & "', " & IIf(IsNothing(Transaccion.FechaDocumentoRef) = True, "NULL", "'" & Transaccion.FechaDocumentoRef & "'") & ", " & IIf(IsNothing(Transaccion.IdProducto) = True, "NULL", "'" & Transaccion.IdProducto & "'") & ")"
                                cmd = New SqlCommand(strSQL, cnx)
                                cmd.Connection.Open()
                                cmd.ExecuteNonQuery()
                                cmd.Connection.Close()

                                ''strAmarre & "', '" & strCentroCostoArea & "', '" & strCentroCostoFuncion & "', '" & strCentroCostoLocal & "', '" & _
                            End If
                            'End If




                            'If InStrRev(Transaccion.Glosa, "CANCELACIÓN", -1) = 1 Then
                            '    fila("Glosa") = Mid(fila("Glosa"), InStrRev(Transaccion.Glosa, Transaccion.Glosa.Length - 10, -1)))
                            '' End If

                            '.Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "01").vDescripcionTablaSistema
                            '.cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                            '.cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                            '.Debe_MN = 0
                            '.Haber_MN = Convert.ToDecimal(txtDiferencia.Text) 'Session("CestaAsiento").Rows(1)("Debe_MN")
                            'Else 'Va en el Haber
                            'AsientoAutoComp.nHaberMNAsiento = Session("CestaAsiento").Rows(1)("Haber_MN")
                            '.Nomenclatura = TablaSistemaNeg.TablaSistemaListarPorId("06", "02").vDescripcionTablaSistema
                            '.cDebePlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cDebePlanCuenta
                            '.cHaberPlanCuenta = CuentaNeg.PlanCuentaListarPorId(.Cuenta, Session("IdEmpresa"), Session("IdPuntoVenta")).cHaberPlanCuenta
                            '.Debe_MN = Convert.ToDecimal(txtDiferencia.Text) * -1
                            '.Haber_MN = 0
                        End If

                        'End If
                        PosicionFila = Mid(Convert.ToString(Convert.ToDecimal(PosicionFila) + 100001), 2) ' Mid(Convert.ToString(ds.Tables("Transaccion").Rows.IndexOf(fila) + 100001), 2)
                    Else
                        Throw New Exception("No se generó ningún asiento contable.  No existe el Centro de Costo.")
                    End If



                    'PARTE IGV 

                    '                            strSQL = "" & _
                    '                            " INSERT INTO EXP_CONTA " & _
                    '                            " (cCajera, cTipoAsiento, cCorrelativo, dFechaTrans, dFechaVenc, cCuenta, " & _
                    '                            "  cSubCuenta, cDebeHaber, cCuentaCte, cNroDocumento, cNroLetra, nImporte, " & _
                    '                            "  nIGV, cCentroCosto, nImporteLetra, nTipoCambio, cCentroCostoV, cCentroCostoF, " & _
                    '                            "  cTipoDocumento, cNroDocumen, nImporteDolar, cTipoMoneda, cTipoDocumentoRef, cNroDocumentoRef, cRazonSocial, cRUCDNI, cDescripcionCliente, cIdSubDiario, cIdProvision) " & _
                    '                            " VALUES " & _
                    '                            " ('" & Mid(pstrCodigoUsuario, 3, 2) & "', '" & tOperacion & "', '" & vCorre & "', '" & fRegistro & "', '" & fVencimiento & "', " & _
                    '                            "  '" & Trim(rsOperacion!cIdCuenta) & "', '', '" & rsOperacion!cTipoDebeHaber & "', " & _
                    '                            "  '" & codCliente & "', '', '', " & importeSol & ", 0, '" & CStr(CInt(cCostoArea)) & "', 0, " & _
                    '                            "  " & tCambio & ", '" & CStr(CInt(cCostoLocal)) & "', '" & CStr(CInt(cCostoFuncion)) & "', " & _
                    '                            "  '" & tDocumento & "', '" & NroSerie + "-" + NroDocumento & "', " & _
                    '                            "  " & importeDol & ", '" & tMoneda & "', '" & tDocumentoRef & "', '" & nroSerieRef + IIf(nroSerieRef = "", "", "-") + nroDocumentoRef & "', " & _
                    '                            "'" & IIf(tDocumento = "03", "", rsClienteUnico!cCentroLaboral) & "', '" & rsClienteUnico!cNroDocumento & "', '" & Trim(rsClienteUnico!cApellidoPaterno) + " " + Trim(rsClienteUnico!cApellidoMaterno) + ", " + Trim(rsClienteUnico!cNombres) & "', '" & Trim(rsOperacion!cIdSubDiario) & "', '" & Trim(rsOperacion!cIdProvision) & "')"

                Next
            Else
                Throw New Exception("No se generó ningún asiento contable.  Falta parametrizar.")
            End If
        Next

        Try
            cnx.Open()
            'Return ds.Tables(0)
            Return strNumeroAsiento
            cnx.Close()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function GeneraLetra(ByVal Importe As Decimal, ByVal TipoMoneda As String, ByVal MonedaAbrev As Boolean, ByVal SoloLetra As Boolean) As String
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strSQL As String
        If MonedaAbrev = True Then
            strSQL = "SELECT vDescripcionAbreviadaTipoMoneda + ' ' + '" & Importe & "' As vDescripcionImporte " &
                               "FROM GNRL_TIPOMONEDA " &
                               "WHERE cIdTipoMoneda = '" & TipoMoneda & "'"
        Else
            If SoloLetra = True Then
                strSQL = "SELECT dbo.NumeroEnLetra(" & Importe & ") As vDescripcionImporte " &
                               "FROM GNRL_TIPOMONEDA " &
                               "WHERE cIdTipoMoneda = '" & TipoMoneda & "'"
            Else
                strSQL = "SELECT dbo.NumeroEnLetra(" & Importe & ") + ' ' + vDescripcionTipoMoneda As vDescripcionImporte " &
                               "FROM GNRL_TIPOMONEDA " &
                               "WHERE cIdTipoMoneda = '" & TipoMoneda & "'"
            End If
        End If
        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Importe")

        'Dim strDescripcionImporte = ds.Tables("Importe").Rows(0).Item("vDescripcionImporte")
        GeneraLetra = ds.Tables("Importe").Rows(0).Item("vDescripcionImporte")
    End Function

    'Function ValidaPerfil(ByVal IdUsuario As String, ByVal IdPerfil As String, ByVal IdElemento As String, ByVal IdModulo As String,
    '                       ByVal IdSistema As String, ByVal IdArea As String) As Boolean
    Function ValidaPerfil(ByVal IdUsuario As String, ByVal IdPerfil As String, ByVal IdElemento As String, ByVal IdModulo As String,
                       ByVal IdSistema As String) As Boolean
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strSQL As String
        'strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_CONFIGURACION " &
        '         "WHERE cIdUsuario = '" & IdUsuario & "' AND cIdPerfil = '" & IdPerfil & "' " &
        '         "      AND cIdElemento = '" & IdElemento & "' AND cIdModulo = '" & IdModulo & "' AND cIdSistema = '" & IdSistema & "' " &
        '         "      AND cIdArea = '" & IdArea & "'"
        strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_CONFIGURACION " &
                 "WHERE cIdUsuario = '" & IdUsuario & "' AND cIdPerfil = '" & IdPerfil & "' " &
                 "      AND cIdElemento = '" & IdElemento & "' AND cIdModulo = '" & IdModulo & "' AND cIdSistema = '" & IdSistema & "'"

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Valida")

        'Dim strDescripcionImporte = ds.Tables("Importe").Rows(0).Item("vDescripcionImporte")
        'GeneraLetra = ds.Tables("Valida").Rows(0).Item("nCantidad")
        If ds.Tables("Valida").Rows(0).Item("nCantidad") > 0 Then
            ValidaPerfil = True 'Si ejecuta la sentencia
        Else
            ValidaPerfil = False 'No tiene permisos
        End If
    End Function

    Function ValidarCierreMes(ByVal IdPeriodo As String, ByVal IdMes As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdSistema As String) As Boolean
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strSQL As String
        strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_CIERRE " &
                 "WHERE cIdEmpresa = '" & IdEmpresa & "' AND cIdPuntoVenta = '" & IdPuntoVenta & "' " &
                 "      AND cAnoCierre = '" & IdPeriodo & "' AND cMesCierre = '" & IdMes & "' " &
                 "      AND cIdSistema = '" & IdSistema & "' AND bEstadoRegistroCierre = 1"
        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Valida")

        'Dim strDescripcionImporte = ds.Tables("Importe").Rows(0).Item("vDescripcionImporte")
        'GeneraLetra = ds.Tables("Valida").Rows(0).Item("nCantidad")
        If ds.Tables("Valida").Rows(0).Item("nCantidad") > 0 Then
            ValidarCierreMes = True 'Si ejecuta la sentencia
        Else
            ValidarCierreMes = False 'No tiene permisos
        End If
    End Function

    Public Function GuardarArchivoGeneral(Archivo As HttpPostedFile, Carpeta As String, ArchivoDestino As String) As String
        'Se carga la ruta física de la carpeta temp del sitio
        Dim strRuta As String = HttpContext.Current.Server.MapPath("~/" & Carpeta)

        'Si el directorio no existe, crearlo
        If Not (Directory.Exists(strRuta)) Then Directory.CreateDirectory(strRuta)

        Dim strExtension As String = Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 4)
        'Dim strArchivo As String = String.Format("{0}\\{1}", strRuta, Mid(Archivo.FileName, InStrRev(Archivo.FileName, "\") + 1, Archivo.FileName.Length))
        Dim strArchivo As String = String.Format("{0}\\{1}", strRuta, ArchivoDestino & "." & strExtension)

        'ArchivoDestino
        'Verificar que el archivo no exista
        'Si queremos eliminar la imagen existente, se debe usar File.Delete        
        File.Delete(strArchivo) 'LO QUITE JMUG 09/08/2019: "C:\SIW_NEW\slnSIW2019\slnSIW\VTAS\Certificados\\C:\SIW_NEW\slnSIW2019\slnSIW\VTAS\Certificados\HILDIBRANDO FLORES\HILDIBRANDO.pfx"

        If (File.Exists(strArchivo)) Then
            'throw new Exception(string.Format("Ya existe una imagen con nombre \"{0}\".", file.FileName));
            Throw New Exception(String.Format("Ya existe este archivo"))
        End If
        Archivo.SaveAs(strArchivo)
        'File.Delete(strRuta & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 3))
        'File.Copy(strRuta & "/" & Archivo.FileName, strRuta & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 3))
        'File.Delete(strRuta & "/" & Archivo.FileName)
    End Function

    Public Function GuardarArchivo(Archivo As HttpPostedFile, Redimensionar As Boolean, Carpeta As String, ArchivoDestino As String, ImagenPeque As Boolean, Optional CarpetaPeriodo As String = "", Optional Tamano As Integer = 0, Optional TamanoPeque As Integer = 0) As String
        'Se carga la ruta física de la carpeta temp del sitio
        'Dim strRuta As String = HttpContext.Current.Server.MapPath("~/temp")
        Dim strRuta As String = HttpContext.Current.Server.MapPath("~/" & Carpeta)
        Dim strRutaPeque As String = HttpContext.Current.Server.MapPath("~/" & Carpeta & "small")

        'Si el directorio no existe, crearlo
        If Not (Directory.Exists(strRuta + CarpetaPeriodo)) Then Directory.CreateDirectory(strRuta + CarpetaPeriodo)
        If ImagenPeque = True Then
            If Not (Directory.Exists(strRutaPeque + CarpetaPeriodo)) Then Directory.CreateDirectory(strRutaPeque + CarpetaPeriodo)
        End If

        'Dim strArchivo As String = String.Format("{0}\\{1}", strRuta, Archivo.FileName)
        Dim strArchivo As String = String.Format("{0}\\{1}", strRuta + CarpetaPeriodo, Mid(Archivo.FileName, InStrRev(Archivo.FileName, "\") + 1, Archivo.FileName.Length))
        'Dim strArchivoPeque As String = String.Format("{0}\\{1}", strRutaPeque, Archivo.FileName)
        Dim strArchivoPeque As String = String.Format("{0}\\{1}", strRutaPeque + CarpetaPeriodo, Mid(Archivo.FileName, InStrRev(Archivo.FileName, "\") + 1, Archivo.FileName.Length))

        'ArchivoDestino
        'Verificar que el archivo no exista
        'Si queremos eliminar la imagen existente, se debe usar File.Delete        
        File.Delete(strArchivo)
        If ImagenPeque = True Then
            File.Delete(strArchivoPeque)
        End If

        If (File.Exists(strArchivo)) Then
            'throw new Exception(string.Format("Ya existe una imagen con nombre \"{0}\".", file.FileName));
            Throw New Exception(String.Format("Ya existe una imagen con nombre"))
        ElseIf (Redimensionar) Then
            Dim Size = Tamano

            Dim bytes() As Byte = New Byte((Archivo.InputStream.Length) - 1) {}

            'Using NewImg As New Bitmap(Img, newW, newH)
            '    'Using g As New Graphics(Graphics.FromImage(NewImg))
            '    Using g As Graphics = Graphics.FromImage(NewImg)



            Archivo.InputStream.Read(bytes, 0, bytes.Length)
            bytes = ResizeImage(bytes, Tamano)
            'Using (MemoryStream ms = new MemoryStream(bytes))
            Using ms As New MemoryStream(bytes)
                'Image(img = Image.FromStream(ms))
                Dim img = Image.FromStream(ms)
                'img.Save(strArchivo, System.Drawing.Imaging.ImageFormat.Png)

                File.WriteAllBytes(strArchivo, bytes)

                ms.Flush()
                ms.Close()
            End Using

            'Imagen mas chiquita
            If ImagenPeque = True Then
                bytes = ResizeImage(bytes, TamanoPeque)
                Using ms As New MemoryStream(bytes)
                    'Image(img = Image.FromStream(ms))
                    Dim img = Image.FromStream(ms)
                    'img.Save(strArchivoPeque, System.Drawing.Imaging.ImageFormat.Png)
                    File.WriteAllBytes(strArchivoPeque, bytes)
                    ms.Flush()
                    ms.Close()
                End Using
            End If
            'Else
            'strRuta
            'File.Copy(strRuta & "/" & Archivo.FileName, "~/" & Carpeta & "/" & ArchivoDestino)
            'File.Copy("~/" & Carpeta & "/" & Archivo.FileName, "~/" & Carpeta & "/" & ArchivoDestino)
            'MsgBox(Mid(InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 3))
            File.Delete(strRuta & CarpetaPeriodo & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 4))
            'File.Copy(strRuta & "/" & Archivo.FileName, strRuta & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 3))
            File.Copy(strRuta & CarpetaPeriodo & "/" & Mid(Archivo.FileName, InStrRev(Archivo.FileName, "\") + 1, Archivo.FileName.Length), strRuta & CarpetaPeriodo & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 4))
            'File.Delete(strRuta & "/" & Archivo.FileName)
            File.Delete(strRuta & CarpetaPeriodo & "/" & Mid(Archivo.FileName, InStrRev(Archivo.FileName, "\") + 1, Archivo.FileName.Length))
            If ImagenPeque = True Then
                File.Delete(strRutaPeque & CarpetaPeriodo & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 4))
                'File.Copy(strRutaPeque & "/" & Archivo.FileName, strRutaPeque & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 3))
                File.Copy(strRutaPeque & CarpetaPeriodo & "/" & Mid(Archivo.FileName, InStrRev(Archivo.FileName, "\") + 1, Archivo.FileName.Length), strRutaPeque & CarpetaPeriodo & "/" & ArchivoDestino & "." & Mid(Archivo.FileName.ToString, InStrRev(Archivo.FileName.ToString, ".", -1) + 1, 4))
                'File.Delete(strRutaPeque & "/" & Archivo.FileName)
                File.Delete(strRutaPeque & CarpetaPeriodo & "/" & Mid(Archivo.FileName, InStrRev(Archivo.FileName, "\") + 1, Archivo.FileName.Length))
            End If
        End If

        '    If (file.Exists(archivo)) Then
        'throw new Exception(string.Format("Ya existe una imagen con nombre \"{0}\".", file.FileName));
        '    ElseIf (redimensionar) Then

    End Function

    Private Function ResizeImage(Source As Byte(), Optional Size As Integer = 0) As Byte()
        Dim Res() As Byte = {}

        'Se define un tamaño predeterminado, para el caso de que no se le dé valor a la variable size
        Const defaultSize As Integer = 100

        'El arreglo de bytes será convertido a un Stream dentro del método, el cual será vaciado al final de la operación para liberar la imagen.
        'Esto es muy útil cuando se está leyendo la imagen desde un archivo del disco, pues si el objeto Stream queda abierto, el archivo puede ser bloqueado.
        'Using (MemoryStream ms = new MemoryStream(source, 0, source.Length))
        Using Ms As New MemoryStream(Source, 0, Source.Length)
            'Se utiliza un objeto Image para leer el contenido de la imagen
            Dim Img = Image.FromStream(Ms)
            '#Region Dimensiones
            'La variable max tendrá el valor en el que se va a dimensionar la imagen.
            'En este método, este tamaño se aplicará a la anchura o a la altura, dependiendo cuál es la mayor.
            'Si se necesita aplicar siempre a la altura o siempre a la anchura, se debe cambiar esta parte del código.
            If Size = 0 Then
                Size = defaultSize
            End If
            Dim max As Integer = Size
            Dim h = Img.Height 'altura actual de la imagen
            Dim w = Img.Width 'anchura actual de la imagen
            Dim newH, newW 'variables que tendrán el nuevo tamaño, según la variable size

            If (h = w And h > max) Then 'Si el tamaño es igual en ambos lados, las nuevas dimensiones reciben el mismo valor
                newW = max
                'newH = newW = max
                newH = max
            ElseIf (h > w And h > max) Then 'Si la altura es mayor, max se aplica a la altura y la anchura se ajusta a este nuevo tamaño
                newH = max
                newW = (w * max) / h
            ElseIf (w > h And w > max) Then 'Si la anchura es mayor que la altura, se hace lo contrario a la condición anterior
                newW = max
                newH = (h * max) / w
            Else
                'Si tanto la anchura como la altura son menores a la variable max, se deja su valor actual.
                'Si queremos forzar el redimensionamiento aunque la imagen sea menor al tamaño deseado, podemos omitir esta condición y cambiar la primera
                newH = h
                newW = w
            End If

            If (h <> newH And w <> newW) Then
                'Para cambiar el tamaño de la imagen, usamos un nuevo objeto Image, dentro del cual guardaremos la imagen redimensionada
                Using NewImg As New Bitmap(Img, newW, newH)
                    'Using g As New Graphics(Graphics.FromImage(NewImg))
                    Using g As Graphics = Graphics.FromImage(NewImg)
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear
                        g.DrawImage(Img, 0, 0, NewImg.Width, NewImg.Height)
                    End Using
                    'El objeto res será el contenido en bytes de la imagen nueva

                    'res = (byte[])new ImageConverter().ConvertTo(newImg, typeof(byte[]))
                    Res = New ImageConverter().ConvertTo(NewImg, GetType(Byte()))
                End Using

            Else
                'Si no hay diferencias entre el tamaño anterior y el nuevo, el objeto res será el contenido de la imagen original
                'res = (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
                Res = New ImageConverter().ConvertTo(Img, GetType(Byte()))
            End If

            'Aquí cerramos el objeto Stream para liberar la imagen. Este paso es necesario para evitar que la imagen se bloquee.
            Ms.Flush()
            Ms.Close()

            Return Res
        End Using
        '        if (h != newH && w != newW) then
        '            'Para cambiar el tamaño de la imagen, usamos un nuevo objeto Image, dentro del cual guardaremos la imagen redimensionada
        'using NewImg as new Bitmap(img, newW, newH))
        '            End Using
        '        End If
    End Function

    'Function EncryptData(ByVal plaintext As String) As String
    '    ' Convert the plaintext string to a byte array.
    '    Dim plaintextBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(plaintext)

    '    ' Create the stream.
    '    Dim ms As New System.IO.MemoryStream
    '    ' Create the encoder to write to the stream.
    '    Dim encStream As New CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)

    '    ' Use the crypto stream to write the byte array to the stream.
    '    encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
    '    encStream.FlushFinalBlock()

    '    ' Convert the encrypted stream to a printable string.
    '    Return Convert.ToBase64String(ms.ToArray)
    'End Function

    'Function DecryptData(ByVal encryptedtext As String) As String
    '    ' Convert the encrypted text string to a byte array.
    '    Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

    '    ' Create the stream.
    '    Dim ms As New System.IO.MemoryStream
    '    ' Create the decoder to write to the stream.
    '    Dim decStream As New CryptoStream(ms, TripleDES.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write)

    '    ' Use the crypto stream to write the byte array to the stream.
    '    decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
    '    decStream.FlushFinalBlock()

    '    ' Convert the plaintext stream to a string.
    '    Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
    'End Function


    Function Encriptar(ByVal Input As String) As String
        Dim IV() As Byte = ASCIIEncoding.ASCII.GetBytes("qualityi") 'La clave debe ser de 8 caracteres
        Dim EncryptionKey() As Byte = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5") 'No se puede alterar la cantidad de caracteres pero si la clave
        Dim buffer() As Byte = Encoding.UTF8.GetBytes(Input)
        Dim des As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
        des.Key = EncryptionKey
        des.IV = IV
        Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length()))
    End Function

    Function Desencriptar(ByVal Input As String) As String
        Dim IV() As Byte = ASCIIEncoding.ASCII.GetBytes("qualityi") 'La clave debe ser de 8 caracteres
        Dim EncryptionKey() As Byte = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5") 'No se puede alterar la cantidad de caracteres pero si la clave
        Dim buffer() As Byte = Convert.FromBase64String(Input)
        Dim des As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
        des.Key = EncryptionKey
        des.IV = IV
        Return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length()))
    End Function

    'Function CambiarCadenaConexion(ByVal Conexion As String) As String
    '    My.Settings.Item("SIWConnectionString") = Conexion

    '    Return "Exito"
    'End Function

    'Function GetCnx() As String
    '    Dim strCnx As String
    '    Try
    '        strCnx = My.Settings.SIWConnectionString
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    '    Return strCnx
    'End Function
    Public Function MigrarDataMiSazonPE(ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
        'Dim x
        'x = Data.PA_CTBL_MNT_RECALCULAR_LINEASXASIENTO(IdPeriodo, IdMes, IdTipoLibro, NroAsiento, IdEmpresa, IdPuntoVenta, NroCeros).ToString
        'Return x

        'Dim consulta = Data.PA_CTBL_RPT_INVENTARIO(IdPeriodo, CostoTerreno, Metraje, IdGrupo)

        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_GNRL_MNT_MIGRARMISAZONPE"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cIdEmpresa").Value = IdEmpresa
        cmd.Parameters("@cIdPuntoVenta").Value = IdPuntoVenta

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "MigrarDatos")

        Try
            cnx.Open()
            Return True
            cnx.Close()
        Catch ex As Exception
            Return False
            Throw New Exception(ex.Message)
        End Try
    End Function

    'LO QUITARE O NO JMUG : 30/10/2018
    'Public Function GenerarDocumentoVentaElectronico(ByVal IdTipoDocumento As String, ByVal IdNumeroSerieDocumento As String,
    '    ByVal IdNumeroCorrelativo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As DataTable 'As Boolean
    '    'Dim x
    '    'x = Data.PA_CTBL_MNT_RECALCULAR_LINEASXASIENTO(IdPeriodo, IdMes, IdTipoLibro, NroAsiento, IdEmpresa, IdPuntoVenta, NroCeros).ToString
    '    'Return x

    '    'Dim consulta = Data.PA_CTBL_RPT_INVENTARIO(IdPeriodo, CostoTerreno, Metraje, IdGrupo)

    '    Dim MiConexion As New clsConexionDAO
    '    Dim cnx As New SqlConnection(MiConexion.GetCnx)
    '    Dim cmd As New SqlCommand
    '    Dim da As New SqlDataAdapter
    '    Dim ds As New DataSet
    '    'Dim dt As New DataTable
    '    'Dim dt As New DataTable()

    '    'Se Configura el comando
    '    cmd.Connection = cnx
    '    cmd.CommandTimeout = 15000
    '    cmd.CommandType = CommandType.StoredProcedure
    '    cmd.CommandText = "PA_VTAS_RPT_DOCUMENTOVENTAFE"

    '    'Se crea el objeto Parameters por cada parametro
    '    cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdTipoDocumento", SqlDbType.Char, 2))
    '    cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroSerieDocumento", SqlDbType.VarChar, 6))
    '    cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroCorrelativo", SqlDbType.VarChar, 20))
    '    cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
    '    cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))

    '    'Me.CrystalReportSource1.ReportDocument.SetParameterValue("@cIdTipoDocumento", Request.QueryString("TipoDoc"))
    '    'Me.CrystalReportSource1.ReportDocument.SetParameterValue("@vIdNumeroSerieDocumento", Request.QueryString("NroSerie"))
    '    'Me.CrystalReportSource1.ReportDocument.SetParameterValue("@vIdNumeroCorrelativo", Request.QueryString("NroDoc"))
    '    'Me.CrystalReportSource1.ReportDocument.SetParameterValue("@cIdPuntoVenta", Session("IdPuntoVenta"))
    '    'Me.CrystalReportSource1.ReportDocument.SetParameterValue("@cIdEmpresa", Session("IdEmpresa"))


    '    'Se establece los valores por cada parametro
    '    cmd.Parameters("@cIdTipoDocumento").Value = IdTipoDocumento
    '    cmd.Parameters("@vIdNumeroSerieDocumento").Value = IdNumeroSerieDocumento
    '    cmd.Parameters("@vIdNumeroCorrelativo").Value = IdNumeroCorrelativo
    '    cmd.Parameters("@cIdPuntoVenta").Value = IdPuntoVenta
    '    cmd.Parameters("@cIdEmpresa").Value = IdEmpresa

    '    'Se configura el Adaptador
    '    da.SelectCommand = cmd
    '    da.Fill(ds, "DocumentoVenta")
    '    'da.Fill(dt, "DocumentoVenta")

    '    Try
    '        cnx.Open()
    '        Return ds.Tables("DocumentoVenta")
    '        '1Return True
    '        'As DataTable



    '        'da.Fill(ds, "Valida")

    '        ''Dim strDescripcionImporte = ds.Tables("Importe").Rows(0).Item("vDescripcionImporte")
    '        ''GeneraLetra = ds.Tables("Valida").Rows(0).Item("nCantidad")
    '        'If ds.Tables("Valida").Rows(0).Item("nCantidad") > 0 Then
    '        '    ValidarCierreMes = True 'Si ejecuta la sentencia
    '        'Else
    '        '    ValidarCierreMes = False 'No tiene permisos
    '        'End If


    '        'Return ds
    '        'Return dt
    '        cnx.Close()
    '    Catch ex As Exception
    '        Return ds.Tables("DocumentoVenta")
    '        Throw New Exception(ex.Message)
    '    End Try


    '    ''Dim dt As New DataTable()
    '    ''Dim constr As String = My.Settings.SIWConnectionString
    '    ''Using con As New SqlConnection(constr)
    '    ''    Using cmd As New SqlCommand(strQuery)
    '    ''        Using sda As New SqlDataAdapter()
    '    ''            cmd.CommandType = CommandType.Text
    '    ''            cmd.Connection = con
    '    ''            sda.SelectCommand = cmd
    '    ''            sda.Fill(dt)
    '    ''        End Using
    '    ''    End Using
    '    ''    Return dt
    '    ''End Using

    'End Function

    Public Function IsValidarEmail(strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn,
               "^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
               "(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
    End Function

    Public Function IsValidarDNI_RUC(ByVal IdEmpresa As String, ByVal strDocumento As String, ByVal strTipoDocumento As String) As Boolean
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strSQL As String
        'If strTipoDocumento = "01" Or strTipoDocumento = "02" Or strTipoDocumento = "03" Then '01-DNI(01), 02-OTROS(00), 03-CARNET EXTRANJERIA(04)
        '    strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_CLIENTE " &
        '         "WHERE cIdEmpresa = '" & IdEmpresa & "' AND vDniCliente = '" & strDocumento & "'"
        'ElseIf strTipoDocumento = "04" Then '04-RUC(06)
        '    strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_CLIENTE " &
        '         "WHERE cIdEmpresa = '" & IdEmpresa & "' AND vRucCliente = '" & strDocumento & "'"
        'End If
        'If strTipoDocumento = "01" Or strTipoDocumento = "02" Or strTipoDocumento = "03" Then '01-DNI(01), 02-OTROS(00), 03-CARNET EXTRANJERIA(04)
        strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_CLIENTE " &
                 "WHERE cIdEmpresa = '" & IdEmpresa & "' AND (vDniCliente = '" & strDocumento & "' OR vRucCliente = '" & strDocumento & "')"
        'ElseIf strTipoDocumento = "04" Then '04-RUC(06)
        '    strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_CLIENTE " &
        '         "WHERE cIdEmpresa = '" & IdEmpresa & "' AND vRucCliente = '" & strDocumento & "'"
        'End If

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Valida")

        'Dim strDescripcionImporte = ds.Tables("Importe").Rows(0).Item("vDescripcionImporte")
        'GeneraLetra = ds.Tables("Valida").Rows(0).Item("nCantidad")
        If ds.Tables("Valida").Rows(0).Item("nCantidad") > 0 Then
            IsValidarDNI_RUC = True 'Si ejecuta la sentencia
        Else
            IsValidarDNI_RUC = False 'No tiene permisos
        End If
    End Function

    Public Function IsValidarDNI_RUC_Proveedor(ByVal IdEmpresa As String, ByVal strDocumento As String, ByVal strTipoDocumento As String) As Boolean
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strSQL As String
        strSQL = "SELECT COUNT(*) AS nCantidad FROM GNRL_PROVEEDOR " &
                 "WHERE cIdEmpresa = '" & IdEmpresa & "' AND (vDniProveedor = '" & strDocumento & "' OR vRucProveedor = '" & strDocumento & "')"

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Valida")

        If ds.Tables("Valida").Rows(0).Item("nCantidad") > 0 Then
            IsValidarDNI_RUC_Proveedor = True 'Si ejecuta la sentencia
        Else
            IsValidarDNI_RUC_Proveedor = False 'No tiene permisos
        End If
    End Function

    Public Function IsValidarDNI_RUC_Transportista(ByVal IdEmpresa As String, ByVal strDocumento As String, ByVal strTipoDocumento As String) As Boolean
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim strSQL As String
        If strTipoDocumento = "01" Or strTipoDocumento = "02" Or strTipoDocumento = "03" Then '01-DNI(01), 02-OTROS(00), 03-CARNET EXTRANJERIA(04)
            strSQL = "SELECT COUNT(*) AS nCantidad FROM LOGI_TRANSPORTISTA " &
                 "WHERE cIdEmpresa = '" & IdEmpresa & "' AND vDniTransportista = '" & strDocumento & "'"
        ElseIf strTipoDocumento = "04" Then '04-RUC(06)
            strSQL = "SELECT COUNT(*) AS nCantidad FROM LOGI_TRANSPORTISTA " &
                 "WHERE cIdEmpresa = '" & IdEmpresa & "' AND vRucTransportista= '" & strDocumento & "'"
        End If
        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Valida")

        If ds.Tables("Valida").Rows(0).Item("nCantidad") > 0 Then
            IsValidarDNI_RUC_Transportista = True 'Si ejecuta la sentencia
        Else
            IsValidarDNI_RUC_Transportista = False 'No tiene permisos
        End If
    End Function
    Public Function VerificarConexionURL(ByVal sURL As String) As Boolean
        Dim Peticion As System.Net.WebRequest
        Dim Respuesta As System.Net.HttpWebResponse
        Try
            Peticion = System.Net.WebRequest.Create(sURL)
            Respuesta = Peticion.GetResponse
            Return True
        Catch ex As System.Net.WebException
            If ex.Status = Net.WebExceptionStatus.NameResolutionFailure Then
                Return False
            End If
            Return False
        End Try
    End Function

    Public Function ConvertirListADataTable(Of t)(
                                                  ByVal list As IList(Of t)
                                               ) As DataTable
        Dim table As New DataTable()
        If Not list.Any Then
            'don't know schema ....
            Return table
        End If
        Dim fields() = list.First.GetType.GetProperties

        'fields(1).CustomAttributes(GetType(System.Decimal))
        For Each field In fields
            'Dim x = field.PropertyType
            'Dim y = Convert.ChangeType.x.ToString
            ' If Nullable.GetUnderlyingType(field.PropertyType) Then
            If field.PropertyType.Name.Contains("Nullable") Then
                If field.PropertyType.FullName.Contains("Decimal") Then
                    table.Columns.Add(field.Name, GetType(System.Decimal))
                ElseIf field.PropertyType.FullName.Contains("String") Then
                    table.Columns.Add(field.Name, GetType(System.String))
                ElseIf field.PropertyType.FullName.Contains("Char") Then
                    table.Columns.Add(field.Name, GetType(System.Char))
                ElseIf field.PropertyType.FullName.Contains("DateTime") Then
                    table.Columns.Add(field.Name, GetType(System.DateTime))
                ElseIf field.PropertyType.FullName.Contains("Boolean") Then
                    table.Columns.Add(field.Name, GetType(System.Boolean))
                End If
                'field.DataType = TypeOf (String);
            Else
                table.Columns.Add(field.Name, field.PropertyType)
            End If
        Next
        For Each item In list
            Dim row As DataRow = table.NewRow()
            For Each field In fields
                Dim p = item.GetType.GetProperty(field.Name)
                'If field.PropertyType.Name.Contains("Nullable") Then
                '    If field.PropertyType.FullName.Contains("Decimal") Then
                '        'p = CType(item.GetType, System.Decimal)
                '        row(field.Name) = p.GetValue(item, Nothing)
                '        'table.Columns.Add(field.Name, GetType(System.Decimal))
                '    ElseIf field.PropertyType.FullName.Contains("String") Then
                '        'table.Columns.Add(field.Name, GetType(System.String))
                '    End If
                '    'field.DataType = TypeOf (String);
                'Else
                '                    row(field.Name) = p.GetValue(item, Nothing)
                'End If

                If field.PropertyType.Name.Contains("Nullable") Then
                    If field.PropertyType.FullName.Contains("Boolean") Or field.PropertyType.FullName.Contains("Decimal") Or field.PropertyType.FullName.Contains("String") Or field.PropertyType.FullName.Contains("Char") Then
                        row(field.Name) = p.GetValue(item, Nothing)
                    Else
                        If field.PropertyType.FullName.Contains("DateTime") Then
                            If p.GetValue(item, Nothing) Is Nothing Then
                                row(field.Name) = DBNull.Value
                            Else
                                row(field.Name) = p.GetValue(item, Nothing)
                            End If
                        End If
                    End If
                Else
                    row(field.Name) = p.GetValue(item, Nothing)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    'Public Function EnviarCorreo(ByVal strFrom As String, ByVal strPwd As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMensaje As String, ByVal strRuta As String, ByVal strNombreArchivo As String, ByVal strConfiguracionCorreo As String) As Boolean
    '    Try
    '        Dim msg As New MailMessage
    '        'Dim smtp As New SmtpClient
    '        Dim ConfCorreo() As String = strConfiguracionCorreo.ToString.Split("|")
    '        Dim smtp As New SmtpClient(ConfCorreo(0).ToString, CInt(ConfCorreo(1).ToString))

    '        msg.From = New MailAddress(LCase(strFrom))
    '        msg.To.Add(New MailAddress(strTo))
    '        msg.IsBodyHtml = True 'Se utiliza si el mensaje es HTML
    '        msg.Body = strMensaje
    '        msg.Subject = strSubject
    '        msg.Attachments.Add(New Attachment(strRuta & "DocXMLPDF\" & strNombreArchivo & ".pdf"))
    '        msg.Attachments.Add(New Attachment(strRuta & "DocXMLFirmado\" & strNombreArchivo & ".xml"))
    '        'smtp.Host = ConfCorreo(0).ToString
    '        'smtp.Port = ConfCorreo(1).ToString
    '        'If InStr(UCase(strFrom), "GMAIL") > 0 Then
    '        '    smtp.Host = "smtp.gmail.com"
    '        '    smtp.Port = 587
    '        'ElseIf InStr(UCase(strFrom), "HOTMAIL") > 0 Then
    '        '    smtp.Host = "smtp.live.com"
    '        '    smtp.Port = 465
    '        'ElseIf InStr(UCase(strFrom), "YAHOO") > 0 Then
    '        '    smtp.Host = "smtp.mail.yahoo.com"
    '        '    smtp.Port = 587
    '        'Else
    '        '    smtp.Host = "smtp.mail.yahoo.com"
    '        '    smtp.Port = 587
    '        'End If
    '        smtp.UseDefaultCredentials = False
    '        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
    '        smtp.Credentials = New NetworkCredential(strFrom, strPwd)
    '        smtp.EnableSsl = True
    '        smtp.Send(msg)

    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function
    Public Function EnviarCorreo(ByVal strFrom As String, ByVal strPwd As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMensaje As String, ByVal strRuta As String, ByVal strNombreArchivo As String, ByVal strConfiguracionCorreo As String, ByVal strConfiguracionPeriodo As String, ByVal strCarpetaPDF As String, Optional ByVal booEnviarXMLFirmado As Boolean = True) As Boolean
        Try
            Dim smtp As New SmtpClient
            Dim ConfCorreo() As String = strConfiguracionCorreo.ToString.Split("|")
            Dim Periodo() As String = strConfiguracionPeriodo.ToString.Split("|")
            'smtp.Connect(LCase(ConfCorreo(0).ToString), CInt(ConfCorreo(1).ToString), True)
            smtp.Connect(LCase(ConfCorreo(0).ToString), CInt(ConfCorreo(1).ToString), SecureSocketOptions.StartTls)
            smtp.Authenticate(strFrom, strPwd)

            Dim strAno, strMes As String
            If Periodo(0).ToString = "" Then
                strAno = ""
                strMes = ""
            Else
                strAno = Periodo(0).ToString
                strMes = Periodo(1).ToString
            End If

            Dim msg As New MimeMessage
            msg.From.Add(New MailboxAddress(LCase(strFrom)))
            msg.To.Add(New MailboxAddress(strTo))
            msg.Subject = strSubject
            Dim cuerpo As New BodyBuilder
            cuerpo.HtmlBody = strMensaje
            'cuerpo.Attachments.Add(strRuta & "DocXMLPDF\" & strAno & "\" & strMes & "\" & strNombreArchivo & ".pdf")
            If strNombreArchivo <> "" Then
                cuerpo.Attachments.Add(strRuta & "" & strCarpetaPDF & "\" & strAno & "\" & strMes & "\" & strNombreArchivo & ".pdf")
            End If
            If booEnviarXMLFirmado = True Then
                cuerpo.Attachments.Add(strRuta & "DocXMLFirmado\" & strAno & "\" & strMes & "\" & strNombreArchivo & ".xml")
            End If
            msg.Body = cuerpo.ToMessageBody
            'msg.Body = strMensaje

            smtp.Send(msg)
            smtp.Disconnect(True)


            'msg.IsBodyHtml = True 'Se utiliza si el mensaje es HTML
            'msg.Body = strMensaje
            'msg.Attachments.Add(New Attachment(strRuta & "DocXMLPDF\" & strNombreArchivo & ".pdf"))
            'msg.Attachments.Add(New Attachment(strRuta & "DocXMLFirmado\" & strNombreArchivo & ".xml"))
            'smtp.Host = ConfCorreo(0).ToString
            'smtp.Port = ConfCorreo(1).ToString
            'If InStr(UCase(strFrom), "GMAIL") > 0 Then
            '    smtp.Host = "smtp.gmail.com"
            '    smtp.Port = 587
            'ElseIf InStr(UCase(strFrom), "HOTMAIL") > 0 Then
            '    smtp.Host = "smtp.live.com"
            '    smtp.Port = 465
            'ElseIf InStr(UCase(strFrom), "YAHOO") > 0 Then
            '    smtp.Host = "smtp.mail.yahoo.com"
            '    smtp.Port = 587
            'Else
            '    smtp.Host = "smtp.mail.yahoo.com"
            '    smtp.Port = 587
            'End If
            'smtp.UseDefaultCredentials = False
            'smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            'smtp.Credentials = New NetworkCredential(strFrom, strPwd)
            'smtp.EnableSsl = True
            'smtp.Send(msg)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function FormatoEnvioGeneral(ByVal strIdTipoFormato As String, ByVal strParametros As String) As String
        Dim strHTML As String
        strHTML = ""
        Dim dsFormatoBody = FuncionesGetData("SELECT * FROM GNRL_FORMATOBODY WHERE cIdFormatoBody = '" & strIdTipoFormato & "'")
        strHTML = dsFormatoBody.Rows(0).Item("vDescripcionFormatoBody")
        For Each Param In dsFormatoBody.Rows
            Dim Parametros() As String = strParametros.ToString.Split("*")
            For x = 0 To Parametros.Length - 1
                Dim Valores = Param("vParametrosFormatoBody").ToString.Split("*")
                strHTML = strHTML.Replace(Valores(x), StrConv(Parametros(x), VbStrConv.ProperCase))
            Next
        Next
        Return strHTML
    End Function

    Public Function FormatoEnvio(ByVal strCliente As String, ByVal strNroSerie As String, strNroDocumento As String,
                     ByVal strTipoDocumentoDescripcion As String, ByVal strImporte As String, ByVal strFechaEmision As String,
                     ByVal strRazonSocialEmisor As String) As String
        Dim strHTML As String
        strHTML = "<!DOCTYPE html PUBLIC "" -// W3C // DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" &
                    "<html xmlns=""http://www.w3.org/1999/xhtml"">" &
                    "<head>" &
                    "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" &
                    "<title>Facturación Electrónica BIMS International Company S.A.C.</title>" &
                    "</head>" &
                    "<body>" &
                    "<table class=MsoNormalTable border=0 cellpadding=0 width=0 style='width:600.75pt;" &
                    " mso-cellspacing:1.5pt;mso-yfti-tbllook:1184'>" &
                    " <tr style='mso-yfti-irow:1'>" &
                    "  <td style='padding:7.5pt 7.5pt 7.5pt 7.5pt'>" &
                    "  <p class=MsoNormal style='margin-bottom:12.0pt'><b><span style='font-size:" &
                    "  9.0pt;font-family:""Verdana"",sans-serif'>Estimado Cliente, <br>" &
                    "  Sr(es). " & strCliente & "<br>" &
                    "  &nbsp;</span></b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'>" &
                    "  <br>" &
                    "  <br>" &
                    "  Informamos a usted que el documento " & strNroSerie & "-" & strNroDocumento & ", ya se encuentra" &
                    "  disponible. <o:p></o:p></span></p>" &
                    "  <table class=MsoNormalTable border=0 cellpadding=0 style='mso-cellspacing:" &
                    "   1.5pt;mso-yfti-tbllook:1184;mso-padding-alt:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "   <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'>" &
                    "    <td width=23 style='width:15.0pt;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>Tipo<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>:<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'>" &
                    "    " & strTipoDocumentoDescripcion & "<o:p></o:p></span></p>" &
                    "    </td>" &
                    "   </tr>" &
                    "   <tr style='mso-yfti-irow:1'>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>Numero<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>:<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'>" & strNroSerie & "-" & strNroDocumento & "<o:p></o:p></span></p>" &
                    "    </td>" &
                    "   </tr>" &
                    "   <tr style='mso-yfti-irow:2'>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>Monto<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>:<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'>" &
                    "    " & strImporte & "<o:p></o:p></span></p>" &
                    "    </td>" &
                    "   </tr>" &
                    "   <tr style='mso-yfti-irow:3'>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>Fecha Emisión<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><b><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif;" &
                    "    color:#4D9621'>:<o:p></o:p></span></b></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'>" & strFechaEmision & "<o:p></o:p></span></p>" &
                    "    </td>" &
                    "   </tr>" &
                    "   <tr style='mso-yfti-irow:4;mso-yfti-lastrow:yes'>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'>" &
                    "    <p class=MsoNormal><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'>&nbsp;" &
                    "    <o:p></o:p></span></p>" &
                    "    </td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>" &
                    "    <td style='padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>" &
                    "   </tr>" &
                    "  </table>" &
                    "  </td>" &
                    " </tr>" &
                    " <tr style='mso-yfti-irow:2;mso-yfti-lastrow:yes'>" &
                    "  <td style='padding:7.5pt 7.5pt 7.5pt 7.5pt'>" &
                    "  <p class=MsoNormal><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'><o:p>&nbsp;</o:p></span></p>" &
                    "  <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=0" &
                    "   style='width:600.75pt;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt:" &
                    "   0cm 0cm 0cm 0cm'>" &
                    "   <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>" &
                    "    <td valign=top style='padding:0cm 0cm 0cm 0cm'>" &
                    "    <p class=MsoNormal><span style='font-size:9.0pt;font-family:""Verdana"",sans-serif'>" &
                    "    Atentamente, <br>" &
                    "    <br>" &
                    "    <b>" & strRazonSocialEmisor & "</b><o:p></o:p></span></p>" &
                    "    </td>" &
                    "    <td valign=top style='padding:0cm 0cm 0cm 0cm'></td>" &
                    "   </tr>" &
                    "  </table>" &
                    "  </td>" &
                    " </tr>" &
                    "</table>" &
                    "</body>" &
                    "</html>"
        Return strHTML
    End Function

    Public Function FormatoEnvioPortalEcomNet(ByVal strRuc As String, ByVal strRazonSocial As String, strFechaEmisionInicial As String,
                     ByVal strFechaEmisionFinal As String) As String
        Dim strHTML As String
        strHTML = "<!DOCTYPE html PUBLIC "" -// W3C // DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" &
                    "<html xmlns=""http://www.w3.org/1999/xhtml"">" &
                    "<head>" &
                    "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" &
                    "<title>Sistema de Facturación Electrónica ECOMNET.</title>" &
                    "</head>" &
                    "<body>" &
                    "<table class=MsoNormalTable border=0 cellpadding=0 width=0 style='width:600.75pt;" &
                    " mso-cellspacing:1.5pt;mso-yfti-tbllook:1184'>" &
                    " <tr style='mso-yfti-irow:1'>" &
                    "  <td style='padding:7.5pt 7.5pt 7.5pt 7.5pt'>" &
                    "  <p class=MsoNormal style='margin-bottom:12.0pt'><b><span style='font-size:" &
                    "  9.0pt;font-family:""Verdana"",sans-serif'>Procesar la siguiente información en el portal de consulta de documentos electrónicos: </span></b> <br>" &
                    "  <b>RUC: </b>" & strRuc & "<br>" &
                    "  <b>Razón Social: </b>" & strRazonSocial & "<br>" &
                    "  <b>Fecha Inicial: </b>" & strFechaEmisionInicial & "<br>" &
                    "  <b>Fecha Final: </b>" & strFechaEmisionFinal & "<br>" &
                    "  &nbsp;</p>" &
                    "  </td>" &
                    " </tr>" &
                    "</table>" &
                    "</body>" &
                    "</html>"
        Return strHTML
    End Function

    'INICIO: JMUG: 27/01/2023
    'Public Function FormatoEnvioRecordatorioEcomNet(ByVal strRuc As String, ByVal strRazonSocial As String, ByVal strCorreo As String) As String
    '    Dim strHTML As String
    '    Dim ConfEmpresaMet As New clsConfiguracionEmpresaMetodos
    '    Dim ConfEmpresa As New CONF_EMPRESA
    '    ConfEmpresa = ConfEmpresaMet.EmpresaListarPorId(strRuc)

    '    Dim dtPlan = ConfEmpresaMet.EmpresaGetData("SELECT vDescripcionPlan, nCapacidadMBHostingPlan FROM GNRL_PLAN WHERE cIdPlan = '" & ConfEmpresa.cIdPlanEmpresa & "'")
    '    strHTML = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" &
    '              "<html xmlns=""http://www.w3.org/1999/xhtml"">" &
    '              "<head>" &
    '              "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" &
    '              "<title>Sistema EcomNet</title>" &
    '              "</head>" &
    '              "<body>" &
    '              "<table border=""0"" cellpadding=""0"" width=""801"">" &
    '              "  <tr>" &
    '              "    <td style=""font-family:Verdana, Geneva, sans-serif; font-size:12px"">" &
    '              "      <p>" &
    '              "        <strong>" &
    '              "          Estimado Cliente," &
    '              "          <br />" &
    '              "          Sr(es). " & strRazonSocial & "" &
    '              "        </strong>" &
    '              "        <br />" &
    '              "      </p>" &
    '              "      <p>" &
    '              "        Adjuntamos los datos de acceso para el ingreso al Sistema EcomNet:" &
    '              "        <br />" &
    '              "      </p>" &
    '              "      <table border=""0"" cellpadding=""0"">" &
    '              "        <tr>" &
    '              "          <td width=""20""></td>" &
    '              "          <td><p><strong>Plan</strong></p></td>" &
    '              "          <td><p><strong>:</strong></p></td>" &
    '              "          <td><p>" & dtPlan.Rows(0).Item(0) & "</p></td>" &
    '              "        </tr>" &
    '              "        <tr>" &
    '              "          <td></td>" &
    '              "          <td><p><strong>Hosting</strong></p></td>" &
    '              "          <td><p><strong>:</strong></p></td>" &
    '              "          <td><p>" & dtPlan.Rows(0).Item(1) & "MB</p></td>" &
    '              "        </tr>" &
    '              "        <tr>" &
    '              "          <td></td>" &
    '              "          <td><p><strong>RUC</strong></p></td>" &
    '              "          <td><p><strong>:</strong></p></td>" &
    '              "          <td><p>" & strRuc & "</p></td>" &
    '              "        </tr>" &
    '              "        <tr>" &
    '              "          <td></td>" &
    '              "          <td><p><strong>Contraseña</strong></p></td>" &
    '              "          <td><p><strong>:</strong></p></td>" &
    '              "          <td><p>" & Desencriptar(ConfEmpresa.vPasswordSesionEmpresa) & "</p></td>" &
    '              "        </tr>" &
    '              "      </table>" &
    '              "      </td>" &
    '              "  </tr>" &
    '              "  <tr>" &
    '              "    <td style=""font-family:Verdana, Geneva, sans-serif; font-size:12px""><br />" &
    '              "      <p>" &
    '              "        Atentamente," &
    '              "        <br />" &
    '              "        <br />" &
    '              "        <strong>" &
    '              "          BUSINESS INTEGRITY AND MANAGEMENT SERVICES INTERNATIONAL COMPANY S.A.C." &
    '              "        </strong>" &
    '              "      </p>" &
    '              "     </td>" &
    '              "  </tr>" &
    '              "</table>" &
    '              "</body>" &
    '              "</html>"


    '    Return strHTML
    'End Function
    'FINAL: JMUG: 27/01/2023

    Public Function FormatoEnvioValidacionEnvioCPEEcomNet() As String

        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_VTAS_MNT_VALIDARENVIOCPE"

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "DatosSinEnviarCPE")

        Dim strHTML As String
        strHTML = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" &
                  "<html xmlns=""http://www.w3.org/1999/xhtml"">" &
                  "<head>" &
                  "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" &
                  "<title>Sistema EcomNet</title>" &
                  "</head>" &
                  "<body>" &
                  "<table border=""0"" cellpadding=""0"" width=""801"">" &
                  "  <tr>" &
                  "    <td style=""font-family:Verdana, Geneva, sans-serif; font-size:12px"">" &
                  "      <p>" &
                  "        <strong>" &
                  "          MENSAJE INFORMATIVO," &
                  "          <br />" &
                  "          Lista de " & "errores de envíos de todos los CPE." & "" &
                  "        </strong>" &
                  "        <br />" &
                  "      </p>" &
                  "      <p>" &
                  "        Adjuntamos los datos de todos los comprobantes que no han sido envíados por nuestros clientes que utilizan el Sistema EcomNet:" &
                  "        <br />" &
                  "      </p>" &
                  "      <table border=""0"" cellpadding=""0"">"

        For Each fila In ds.Tables("DatosSinEnviarCPE").Rows
            strHTML = strHTML +
                  "        <tr>" &
                  "          <td width=""20""></td>" &
                  "          <td><p><strong>Base de Datos</strong></p></td>" &
                  "          <td><p><strong>:</strong></p></td>" &
                  "          <td><p>" & fila("vBaseDatos") & "</p></td>" &
                  "        </tr>" &
                  "        <tr>" &
                  "          <td></td>" &
                  "          <td><p><strong>Empresa</strong></p></td>" &
                  "          <td><p><strong>:</strong></p></td>" &
                  "          <td><p>" & fila("cIdEmpresa") & "</p></td>" &
                  "        </tr>" &
                  "        <tr>" &
                  "          <td></td>" &
                  "          <td><p><strong>Fecha Emisión</strong></p></td>" &
                  "          <td><p><strong>:</strong></p></td>" &
                  "          <td><p>" & fila("dFechaEmision") & "</p></td>" &
                  "        </tr>" &
                  "        <tr>" &
                  "          <td></td>" &
                  "          <td><p><strong>Tipo Documento</strong></p></td>" &
                  "          <td><p><strong>:</strong></p></td>" &
                  "          <td><p>" & fila("cIdTipoDocumento") & "-" & fila("vDescripcionTipoDocumento") & "</p></td>" &
                  "        </tr>" &
                  "        <tr>" &
                  "          <td></td>" &
                  "          <td><p><strong>Cantidad</strong></p></td>" &
                  "          <td><p><strong>:</strong></p></td>" &
                  "          <td><p>" & fila("nCantidadNoEnviados") & "</p></td>" &
                  "        </tr>"
        Next

        strHTML = strHTML +
                  "      </table>" &
                  "      </td>" &
                  "  </tr>" &
                  "  <tr>" &
                  "    <td style=""font-family:Verdana, Geneva, sans-serif; font-size:12px""><br />" &
                  "      <p>" &
                  "        Atentamente," &
                  "        <br />" &
                  "        <br />" &
                  "        <strong>" &
                  "          BUSINESS INTEGRITY AND MANAGEMENT SERVICES INTERNATIONAL COMPANY S.A.C." &
                  "        </strong>" &
                  "      </p>" &
                  "     </td>" &
                  "  </tr>" &
                  "</table>" &
                  "</body>" &
                  "</html>"
        Return strHTML
    End Function

    Public Function FormatoEnvioCreacionOT(ByVal strNroOT As String, ByVal strFechaProgramada As String) As String
        Dim strHTML As String
        strHTML = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" &
                  "<html xmlns=""http://www.w3.org/1999/xhtml"">" &
                  "<head>" &
                  "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" &
                  "<title>Sistema EcomNet</title>" &
                  "</head>" &
                  "<body>" &
                  "<table border=""0"" cellpadding=""0"" width=""801"">" &
                  "  <tr>" &
                  "    <td style=""font-family:Verdana, Geneva, sans-serif; font-size:12px"">" &
                  "      <p>" &
                  "        <strong>" &
                  "          MENSAJE INFORMATIVO," &
                  "          <br />" &
                  "          Le informamos que se ha creado la siguiente Orden de Trabajo: " & strNroOT & "" & "" &
                  "        </strong>" &
                  "        <br />" &
                  "      </p>" &
                  "      <p>" &
                  "        Fecha Programada: " & strFechaProgramada &
                  "        <br />" &
                  "      </p>" &
                  "      <table border=""0"" cellpadding=""0"">"

        strHTML = strHTML +
                  "      </table>" &
                  "      </td>" &
                  "  </tr>" &
                  "  <tr>" &
                  "    <td style=""font-family:Verdana, Geneva, sans-serif; font-size:12px""><br />" &
                  "      <p>" &
                  "        Atentamente," &
                  "        <br />" &
                  "        <br />" &
                  "        <strong>" &
                  "          Movitecnica S.A.C." &
                  "        </strong>" &
                  "      </p>" &
                  "     </td>" &
                  "  </tr>" &
                  "</table>" &
                  "</body>" &
                  "</html>"
        Return strHTML
    End Function

    Public Sub GuardarImagenTabla(ByVal byteImage As Byte(), ByVal strTipo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String)
        Dim constr As String = My.Settings.CMMSConnectionString
        Dim conexion As New SqlConnection(constr)
        conexion.Open()
        If strTipo = "QR" Then
            Dim cmd As New SqlCommand("UPDATE GNRL_COLAIMPRESION SET iImagenQR = @iImagenQR WHERE cIdPuntoVentaColaImpresion = '" & IdPuntoVenta & "' AND cIdEmpresaColaImpresion = '" & IdEmpresa & "' AND vParametrosColaImpresion LIKE '%" & IdTipDoc & "*" & IdNroSer & "*" & IdNroDoc & "*%'", conexion)
            With cmd
                .Parameters.Add(New SqlParameter("@iImagenQR", SqlDbType.Image)).Value = byteImage
            End With
            cmd.ExecuteNonQuery()
        End If
        If strTipo = "Logo" Then
            Dim cmd As New SqlCommand("UPDATE GNRL_COLAIMPRESION SET iImagenLogo = @iImagenLogo WHERE cIdPuntoVentaColaImpresion = '" & IdPuntoVenta & "' AND cIdEmpresaColaImpresion = '" & IdEmpresa & "' AND vParametrosColaImpresion LIKE '%" & IdTipDoc & "*" & IdNroSer & "*" & IdNroDoc & "*%'", conexion)
            With cmd
                .Parameters.Add(New SqlParameter("@iImagenLogo", SqlDbType.Image)).Value = byteImage
            End With
            cmd.ExecuteNonQuery()
        End If
        conexion.Close()
    End Sub

    Public Sub GenerarArchivoZIP(ByVal strRutaOrigen As String, ByVal strNombreArchivo As String, Optional ByVal strTotalArchivos As String = "", Optional ByVal strRutaDestino As String = "")
        Try
            Dim zip As New ZipFile
            Dim strArchivo As String = String.Format("{0}\{1}", IIf(strRutaDestino = "", strRutaOrigen, strRutaDestino), strNombreArchivo & ".zip")
            If (File.Exists(strArchivo)) Then
                File.Delete(strArchivo)
            End If

            If Not (File.Exists(strArchivo)) Then
                If strTotalArchivos = "" Then
                    zip.AddFile(strRutaOrigen & "\" & strNombreArchivo & ".txt", "\") 'Cargo el Fichero donde esta mi XML Firmado
                    zip.Save(strRutaOrigen & "\" & strNombreArchivo & ".zip") 'Nombre del Fichero destino
                ElseIf strTotalArchivos <> "" Then
                    Dim Valores() As String = strTotalArchivos.ToString.Split(",")
                    For Each strArc In Valores
                        If strArc <> "" Then
                            zip.AddFile(strRutaOrigen & "\" & strArc, "\") 'Cargo el Fichero donde esta mi XML Firmado
                        End If
                    Next
                    zip.Save(strRutaDestino & "\" & strNombreArchivo & ".zip") 'Nombre del Fichero destino
                End If
            End If
        Catch ex As Exception
            'Return ex.Message
        End Try
    End Sub

    Public Sub DescomprimirArchivoZIP(ByVal strRuta As String, ByVal strNombreArchivo As String)
        Try
            Dim zip As New ZipFile
            'zip = ZipFile.Read(String.Format("{0}\{1}", strRuta, strNombreArchivo & ".zip"))
            zip = ZipFile.Read(String.Format("{0}\{1}", strRuta, strNombreArchivo & ".zip"))

            Dim strArchivo As String = ""
            'strArchivo = Mid(strRutaXMLCertificado, 1, InStrRev(strRutaXMLCertificado, "\")) & "DocXMLAnalizar\R-" & stDocumento.NombreArchivo & ".xml"
            strArchivo = strRuta & strNombreArchivo & ".txt"

            If Directory.Exists(String.Format("{0}\{1}", strRuta, strNombreArchivo)) Then
                Directory.Delete(String.Format("{0}\{1}", strRuta, strNombreArchivo), True)
            End If

            If (File.Exists(strArchivo)) Then
                File.Delete(strArchivo)
            End If

            zip.ExtractAll(String.Format("{0}\{1}", strRuta, strNombreArchivo))
        Catch ex As Exception
            'Return ex.Message
        End Try
    End Sub

    Public Function MigrarConsultaEcomNETTXT(ByVal FechaInicial As String, ByVal FechaFinal As String,
                               ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal Ruta As String) As String
        'Try
        Dim strNombreArchivo = ""
        Dim fichero As String = ""
        Dim texto As String = ""

        Dim strSQL As String = "SELECT DOC.dFechaEmisionCabeceraDocumento, DOC.cIdTipoDocumento, DOC.vIdNumeroSerieDocumentoCabeceraDocumento, DOC.vIdNumeroCorrelativoCabeceraDocumento, " &
                               "       DOC.cIdTipoDocumentoClienteCabeceraDocumento, DOC.vNumeroDocumentoClienteCabeceraDocumento, DOC.nTotalPrecioVentaCabeceraDocumento, EMP.vRucEmpresa, " &
                               "       TIPDOC.vIdEquivalenciaContableTipoDocumento " &
                               "FROM VTAS_CABECERADOCUMENTO DOC INNER JOIN GNRL_EMPRESA AS EMP ON " &
                               "     DOC.cIdEmpresa = EMP.cIdEmpresa INNER JOIN GNRL_TIPODOCUMENTO TIPDOC ON " &
                               "     DOC.cIdTipoDocumento = TIPDOC.cIdTipoDocumento " &
                               "WHERE CONVERT(CHAR(8), DOC.dFechaEmisionCabeceraDocumento, 112) >= '" & FechaInicial & "' AND " &
                               "      CONVERT(CHAR(8), DOC.dFechaEmisionCabeceraDocumento, 112) <= '" & FechaFinal & "' AND " &
                               "      DOC.cIdEmpresa = '" & IdEmpresa & "' AND DOC.cIdPuntoVenta = '" & IdPuntoVenta & "' AND " &
                               "      DOC.vStatusSunatCabeceraDocumento = 'ACEPTADO' " &
                               "ORDER BY dFechaEmisionCabeceraDocumento"
        Dim MiConexion As New clsConexionDAO 'clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "MigrarVentasEcomNET")
        If ds.Tables("MigrarVentasEcomNET").Rows.Count > 0 Then
            Dim strDocumentosVentaXML, strDocumentosVentaPDF As String : strDocumentosVentaXML = "" : strDocumentosVentaPDF = ""
            'I = "1"
            Dim fila As DataRow
            strNombreArchivo = "VentasEcomNET" & ds.Tables("MigrarVentasEcomNET").Rows(0).Item("vRucEmpresa") & FechaInicial & FechaFinal & IdPuntoVenta
            fichero = Ruta & strNombreArchivo & ".txt"

            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("MigrarVentasEcomNET").Rows
                texto = "INSERT GNRL_CONSULTAFE (dFechaEmisionConsultaFE, cIdTipoDocumentoConsultaFE, vIdNumeroSerieDocumentoConsultaFE, " &
                        "                        vIdNumeroCorrelativoConsultaFE, cIdTipoDocumentoClienteConsultaFE, vNumeroDocumentoClienteConsultaFE, " &
                        "                        nTotalPrecioVentaConsultaFE, vRucEmpresaConsultaFE) " &
                        "VALUES ('" & String.Format("{0:dd/MM/yyyy hh:mm:ss}", fila("dFechaEmisionCabeceraDocumento")) & "', '" & fila("cIdTipoDocumento") & "', '" & fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "', " &
                        "        '" & fila("vIdNumeroCorrelativoCabeceraDocumento") & "', '" & fila("cIdTipoDocumentoClienteCabeceraDocumento") & "', '" & fila("vNumeroDocumentoClienteCabeceraDocumento") & "', " &
                        "        " & fila("nTotalPrecioVentaCabeceraDocumento") & ", '" & ds.Tables("MigrarVentasEcomNET").Rows(0).Item("vRucEmpresa") & "')"
                sw.WriteLine(texto)
                strDocumentosVentaXML = strDocumentosVentaXML + ds.Tables("MigrarVentasEcomNET").Rows(0).Item("vRucEmpresa") & "-" & fila("vIdEquivalenciaContableTipoDocumento") & "-" & fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & fila("vIdNumeroCorrelativoCabeceraDocumento") & ".zip,"
                strDocumentosVentaPDF = strDocumentosVentaPDF + ds.Tables("MigrarVentasEcomNET").Rows(0).Item("vRucEmpresa") & "-" & fila("vIdEquivalenciaContableTipoDocumento") & "-" & fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & fila("vIdNumeroCorrelativoCabeceraDocumento") & ".pdf,"
            Next
            sw.Close()
            MigrarConsultaEcomNETTXT = strNombreArchivo & ".txt"

            GenerarArchivoZIP(Mid(Ruta, 1, InStrRev(Ruta, "\Downloads\", -1) - 1) & "\DocXMLZip\" & Mid(FechaInicial, 1, 4) & "\" & Mid(FechaInicial, 5, 2) & "\", strNombreArchivo & "XML", strDocumentosVentaXML, Ruta)
            GenerarArchivoZIP(Mid(Ruta, 1, InStrRev(Ruta, "\Downloads\", -1) - 1) & "\DocXMLPDF\" & Mid(FechaInicial, 1, 4) & "\" & Mid(FechaInicial, 5, 2) & "\", strNombreArchivo & "PDF", strDocumentosVentaPDF, Ruta)

            Dim ftpCliente As New FluentFTP.FtpClient
            ftpCliente.Host = "34.145.45.229"
            ftpCliente.Credentials = New NetworkCredential("ftpbimsic", "B1m5Int3rn4t10n4l")
            ftpCliente.Connect()

            ftpCliente.UploadFile(Ruta & strNombreArchivo & ".txt", strNombreArchivo & ".txt")

            'Envia via FTP los archivos XML firmados de los documentos de ventas.
            ftpCliente.UploadFile(Ruta & strNombreArchivo & "XML.zip", strNombreArchivo & "XML.zip")

            'Envia via FTP los archivos PDF de los documentos de ventas.
            ftpCliente.UploadFile(Ruta & strNombreArchivo & "PDF.zip", strNombreArchivo & "PDF.zip")
        End If
        'Catch ex As Exception
        '    Return ex.Message
        'End Try
    End Function

    Public Function CargarDocumentoVentaResumen(ByVal FechaInicial As String, ByVal FechaFinal As String,
                                                ByVal IdEmpresa As String, ByVal IdPuntoVenta As String,
                                                ByVal IdTipo As String) As DataTable
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_VTAS_RPT_DOCUMENTOVENTARESUMEN"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cFechaInicial", SqlDbType.Char, 8))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cFechaFinal", SqlDbType.Char, 8))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdTipoResumen", SqlDbType.Char, 1))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cFechaInicial").Value = FechaInicial
        cmd.Parameters("@cFechaFinal").Value = FechaFinal
        cmd.Parameters("@cIdEmpresa").Value = IdEmpresa
        cmd.Parameters("@cIdPuntoVenta").Value = IdPuntoVenta
        cmd.Parameters("@cIdTipoResumen").Value = IdTipo

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "DatosResumen")

        Try
            cnx.Open()
            Return ds.Tables(0)
            cnx.Close()
        Catch ex As Exception
            Return ds.Tables(0)
            Throw New Exception(ex.Message)
        End Try
    End Function


#Region "Funciones para Crear un Archivo en Excel"
    'Public Shared Function CrearDocExcel(fileName As String)
    '    Using doc As SpreadsheetDocument = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook)
    '        Dim WorkbookPart = doc.AddWorkbookPart()
    '        WorkbookPart.Workbook = New Workbook()

    '        Dim WorksheetPart = WorkbookPart.AddNewPart(Of WorksheetPart)
    '        WorksheetPart.Worksheet = New Worksheet(New SheetData())

    '        Dim Sheets = WorkbookPart.Workbook.AppendChild(New Sheets())

    '        Dim Sheet1 As Sheet = New Sheet() With {.Id = WorkbookPart.GetIdOfPart(WorksheetPart), .SheetId = 1, .Name = "Data"}

    '        Sheets.Append(Sheet1)
    '        WorkbookPart.Workbook.Save()
    '    End Using
    'End Function

    'Private Function InsertWorksheet(ByVal workbookPart As WorkbookPart, ByVal nombreHoja As String) As WorksheetPart
    '    ' Add a new worksheet part to the workbook.
    '    Dim newWorksheetPart As WorksheetPart = workbookPart.AddNewPart(Of WorksheetPart)()
    '    newWorksheetPart.Worksheet = New Worksheet(New SheetData)

    '    newWorksheetPart.Worksheet.Save()
    '    Dim sheets As Sheets = workbookPart.Workbook.GetFirstChild(Of Sheets)()
    '    Dim relationshipId As String = workbookPart.GetIdOfPart(newWorksheetPart)

    '    ' Get a unique ID for the new sheet.
    '    Dim sheetId As UInteger = 1
    '    If (sheets.Elements(Of Sheet).Count() > 0) Then
    '        sheetId = sheets.Elements(Of Sheet).Select(Function(s) s.SheetId.Value).Max() + 1
    '    End If

    '    Dim sheetName As String = nombreHoja

    '    ' Add the new worksheet and associate it with the workbook.
    '    Dim sheet As Sheet = New Sheet
    '    sheet.Id = relationshipId
    '    sheet.SheetId = sheetId 'IIf(sheetName = "Data", 0, 1) '
    '    sheet.Name = sheetName
    '    sheets.Append(sheet)
    '    workbookPart.Workbook.Save()

    '    Return newWorksheetPart
    'End Function

    'Private Function InsertCellInWorksheet(ByVal columnName As String, ByVal rowIndex As UInteger, ByVal worksheetPart As WorksheetPart) As Cell
    '    Dim worksheet As Worksheet = worksheetPart.Worksheet
    '    Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
    '    Dim cellReference As String = (columnName + rowIndex.ToString())

    '    ' If the worksheet does not contain a row with the specified row index, insert one.
    '    Dim row As Row
    '    If (sheetData.Elements(Of Row).Where(Function(r) r.RowIndex.Value = rowIndex).Count() <> 0) Then
    '        row = sheetData.Elements(Of Row).Where(Function(r) r.RowIndex.Value = rowIndex).First()
    '    Else
    '        row = New Row()
    '        row.RowIndex = rowIndex
    '        sheetData.Append(row)
    '    End If

    '    If (row.Elements(Of Cell).Where(Function(c) c.CellReference.Value = columnName + rowIndex.ToString()).Count() > 0) Then
    '        Return row.Elements(Of Cell).Where(Function(c) c.CellReference.Value = cellReference).First()
    '    Else
    '        ' Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
    '        Dim refCell As Cell = Nothing

    '        Dim newCell As Cell = New Cell
    '        newCell.CellReference = cellReference

    '        row.InsertBefore(newCell, refCell)
    '        worksheet.Save()

    '        Return newCell
    '    End If
    'End Function

    'Private Function InsertSharedStringItem(ByVal text As String, ByVal shareStringPart As SharedStringTablePart) As Integer
    '    ' If the part does not contain a SharedStringTable, create one.
    '    If (shareStringPart.SharedStringTable Is Nothing) Then
    '        shareStringPart.SharedStringTable = New SharedStringTable
    '    End If

    '    Dim i As Integer = 0

    '    For Each item As SharedStringItem In shareStringPart.SharedStringTable.Elements(Of SharedStringItem)()
    '        If (item.InnerText = text) Then
    '            Return i
    '        End If
    '        i = (i + 1)
    '    Next

    '    ' The text does not exist in the part. Create the SharedStringItem and return its index.
    '    shareStringPart.SharedStringTable.AppendChild(New SharedStringItem(New DocumentFormat.OpenXml.Spreadsheet.Text(text)))
    '    shareStringPart.SharedStringTable.Save()

    '    Return i
    'End Function

    'Public Function fExportarExcel(ByVal NombreArchivo As String, ByVal StoreProcedure As String, ByVal CamposParametros As String, ByVal ValorParametros As String,
    '                   ByVal TipoSeparador As String, ByVal Ruta As String)
    '    Try
    '        Dim MiConexion As New clsConexionDAO
    '        Dim cnx As New SqlConnection(MiConexion.GetCnx)
    '        Dim cmd As New SqlCommand
    '        Dim da As New SqlDataAdapter
    '        Dim ds As New DataSet

    '        'Se Configura el comando
    '        cmd.Connection = cnx
    '        cmd.CommandTimeout = 15000
    '        cmd.CommandType = CommandType.StoredProcedure
    '        cmd.CommandText = StoreProcedure ' "PA_VTAS_RPT_REGISTROVENTA"


    '        Dim CamposParametrosValue As String() = Split(CamposParametros, TipoSeparador)
    '        Dim ValorParametrosValue As String() = Split(ValorParametros, TipoSeparador)
    '        'ParametrosValue(5) = txtRutaReporte.Text ' txtRutaLogo.Text

    '        Dim Nombres As String
    '        Dim i As Integer = 0
    '        For Each Nombres In CamposParametrosValue
    '            cmd.Parameters.AddWithValue(Nombres, ValorParametrosValue(i))
    '            i += 1
    '        Next

    '        ''Se crea el objeto Parameters por cada parametro
    '        'cmd.Parameters.Add(New SqlClient.SqlParameter("@cFechaInicial", SqlDbType.Char, 8))
    '        'cmd.Parameters.Add(New SqlClient.SqlParameter("@cFechaFinal", SqlDbType.Char, 8))
    '        'cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))
    '        'cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))

    '        ''Se establece los valores por cada parametro
    '        'cmd.Parameters("@cFechaInicial").Value = FechaInicial
    '        'cmd.Parameters("@cFechaFinal").Value = FechaFinal
    '        'cmd.Parameters("@cIdEmpresa").Value = IdEmpresa
    '        'cmd.Parameters("@cIdPuntoVenta").Value = IdPuntoVenta
    '        'cmd.Parameters("@cIdTipoResumen").Value = IdTipo

    '        'Se configura el Adaptador
    '        da.SelectCommand = cmd
    '        da.Fill(ds, "DatosExportacion")

    '        Try
    '            cnx.Open()
    '            If ds.Tables("DatosExportacion").Rows.Count > 0 Then
    '                Dim NombreArchivoFinal = NombreArchivo & ".xls"
    '                'Save the uploaded Excel file.
    '                Dim filePath As String = Ruta & "/Downloads/" + NombreArchivo 'Path.GetFileName(FileUpload1.PostedFile.FileName)

    '                'CreateWorkbook(filePath)
    '                CrearDocExcel(filePath)

    '                Dim fila As DataRow
    '                Dim intFila As Integer : intFila = 1

    '                Using doc As SpreadsheetDocument = SpreadsheetDocument.Open(filePath, True) 'False es de solo lectura
    '                    'Read the first Sheet from Excel file.
    '                    Dim sheet As Sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild(Of Sheet)()

    '                    'Get the Worksheet instance.
    '                    Dim worksheet As Worksheet = TryCast(doc.WorkbookPart.GetPartById(sheet.Id.Value), WorksheetPart).Worksheet

    '                    Dim shareStringPart As SharedStringTablePart
    '                    shareStringPart = doc.WorkbookPart.AddNewPart(Of SharedStringTablePart)()
    '                    Dim intX = 41
    '                    Dim index(intX) As Integer
    '                    ' Insert a new worksheet.
    '                    'Dim worksheetPart1 As WorksheetPart = InsertWorksheet(doc.WorkbookPart, "Data") 'Codigos
    '                    'Obtener hoja de trabajo
    '                    Dim worksheetPart1 As WorksheetPart = doc.WorkbookPart.WorksheetParts.First() 'Codigos
    '                    'Dim worksheetPart2 As WorksheetPart = InsertWorksheet(doc.WorkbookPart, "Codigos")

    '                    'nuevo :jmug:14/01/2020
    '                    Dim SheetData = New DocumentFormat.OpenXml.Spreadsheet.SheetData()

    '                    For Each fila In ds.Tables("DatosExportacion").Rows
    '                        If fila("vIdNumeroCorrelativoCabeceraDocumento") = "252" Then
    '                            MsgBox("kk")
    '                        End If
    '                        Dim Celda As New DocumentFormat.OpenXml.Spreadsheet.Cell()
    '                        Celda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String
    '                        Celda.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionCabeceraDocumento")))

    '                        Dim newRow As New DocumentFormat.OpenXml.Spreadsheet.Row
    '                        newRow.AppendChild(Celda)
    '                        SheetData.AppendChild(newRow)



    '                        'Dim Celda(intX) As Cell
    '                        'Insert the text into the SharedStringTablePart.
    '                        'index(0) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionCabeceraDocumento")), shareStringPart)
    '                        'index(1) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("dFechaVencimientoCabeceraDocumento")), shareStringPart)
    '                        'index(2) = InsertSharedStringItem(fila("vIdEquivalenciaContableTipoDocumento"), shareStringPart)
    '                        'index(3) = InsertSharedStringItem(fila("vIdNumeroSerieDocumentoCabeceraDocumento"), shareStringPart)
    '                        'index(4) = InsertSharedStringItem(fila("vIdNumeroCorrelativoCabeceraDocumento"), shareStringPart)
    '                        'index(5) = InsertSharedStringItem(fila("cIdTipoDocumentoCliente"), shareStringPart)
    '                        'index(6) = InsertSharedStringItem(fila("vNumeroDocumentoClienteCabeceraDocumento"), shareStringPart)
    '                        'index(7) = InsertSharedStringItem(fila("vRazonSocialCabeceraDocumento"), shareStringPart)
    '                        'index(8) = InsertSharedStringItem(Math.Round(fila("nTotalExporta"), 2), shareStringPart)
    '                        'index(9) = InsertSharedStringItem(Math.Round(fila("nTotalMontoGravadoObligatorio"), 2), shareStringPart)
    '                        'index(10) = InsertSharedStringItem(Math.Round(fila("nTotalMontoExoneradoObligatorio"), 2), shareStringPart)
    '                        'index(11) = InsertSharedStringItem(Math.Round(fila("nTotalMontoInafectoObligatorio"), 2), shareStringPart)
    '                        'index(12) = InsertSharedStringItem(Math.Round(fila("nTotalISCCabeceraDocumento"), 2), shareStringPart)
    '                        'index(13) = InsertSharedStringItem(Math.Round(fila("nTotalIGVCabeceraDocumento"), 2), shareStringPart)
    '                        'index(14) = InsertSharedStringItem(Math.Round(fila("nTotalMontoGratuitaAdicional"), 2), shareStringPart)
    '                        'index(15) = InsertSharedStringItem(Math.Round(fila("nTotalPrecioVentaCabeceraDocumento"), 2), shareStringPart)
    '                        'index(16) = InsertSharedStringItem(Math.Round(fila("nTipoCambioCabeceraDocumento"), 4), shareStringPart)
    '                        'index(17) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionReferencialCabeceraDocumento")).ToString.Trim, shareStringPart)
    '                        'index(18) = InsertSharedStringItem(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim, shareStringPart)
    '                        'index(19) = InsertSharedStringItem(fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento").ToString.Trim, shareStringPart)
    '                        'index(20) = InsertSharedStringItem(fila("vIdNumeroCorrelativoReferencialCabeceraDocumento").ToString.Trim, shareStringPart)
    '                        'index(21) = InsertSharedStringItem(IIf(fila("vIdEquivalenciaContableAbreviadaTipoMoneda").ToString.Trim = "PEN", "S", "D"), shareStringPart)
    '                        'index(22) = InsertSharedStringItem(Math.Round(fila("nTotalPrecioVentaCabeceraDocumento") / fila("nTipoCambioCabeceraDocumento"), 2), shareStringPart)
    '                        'index(23) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("dFechaVencimientoCabeceraDocumento")).ToString.Trim, shareStringPart)
    '                        'index(24) = InsertSharedStringItem(IIf(String.Format("{0:yyyyMMdd}", fila("dFechaEmisionCabeceraDocumento")) <> String.Format("{0:yyyyMMdd}", fila("dFechaVencimientoCabeceraDocumento")), "CRE", "CON"), shareStringPart)
    '                        'index(25) = InsertSharedStringItem("", shareStringPart) 'Centro de Costo
    '                        'index(26) = InsertSharedStringItem("", shareStringPart) 'Centro de Costo 2
    '                        'index(27) = InsertSharedStringItem("70111", shareStringPart) 'Cuenta Contable Base Imponible
    '                        'index(28) = InsertSharedStringItem("40111", shareStringPart) 'Cuenta Contable Otros Tributos y Cargos
    '                        'index(29) = InsertSharedStringItem("101", shareStringPart) 'Cuenta Contable Total
    '                        'index(30) = InsertSharedStringItem("", shareStringPart) 'Regimen Especial
    '                        'index(31) = InsertSharedStringItem("", shareStringPart) 'Porcentaje de Regimen Especial
    '                        'index(32) = InsertSharedStringItem("", shareStringPart) 'Importe Regimen Especial
    '                        'index(33) = InsertSharedStringItem("", shareStringPart) 'Serie Documento Regimen Especial
    '                        'index(34) = InsertSharedStringItem("", shareStringPart) 'Número Documento Regimen Especial
    '                        'index(35) = InsertSharedStringItem("", shareStringPart) 'Fecha Documento Regimen Especial
    '                        'index(36) = InsertSharedStringItem("", shareStringPart) 'Codigo de Presupuesto
    '                        'index(37) = InsertSharedStringItem(Math.Round(fila("nPorcentajeImpuestoCabeceraDocumento"), 2), shareStringPart) 'Porcentaje IGV
    '                        'index(38) = InsertSharedStringItem("VENTAS DEL MES DE " & UCase(MonthName(Convert.ToDateTime(fila("dFechaEmisionCabeceraDocumento")).Month)), shareStringPart) 'Glosa
    '                        'index(39) = InsertSharedStringItem("008", shareStringPart) 'Medio de Pago - Efectivo sin utilizar medio de pago
    '                        'index(40) = InsertSharedStringItem("", shareStringPart) 'Condición de Percepción
    '                        'index(41) = InsertSharedStringItem("", shareStringPart) 'Importe para cálculo de régimen especial

    '                        ''Insert cell A1 into the new worksheet.
    '                        'Celda(0) = InsertCellInWorksheet("A", intFila, worksheetPart1)
    '                        'Celda(1) = InsertCellInWorksheet("B", intFila, worksheetPart1)
    '                        'Celda(2) = InsertCellInWorksheet("C", intFila, worksheetPart1)
    '                        'Celda(3) = InsertCellInWorksheet("D", intFila, worksheetPart1)
    '                        'Celda(4) = InsertCellInWorksheet("E", intFila, worksheetPart1)
    '                        'Celda(5) = InsertCellInWorksheet("F", intFila, worksheetPart1)
    '                        'Celda(6) = InsertCellInWorksheet("G", intFila, worksheetPart1)
    '                        'Celda(7) = InsertCellInWorksheet("H", intFila, worksheetPart1)
    '                        'Celda(8) = InsertCellInWorksheet("I", intFila, worksheetPart1)
    '                        'Celda(9) = InsertCellInWorksheet("J", intFila, worksheetPart1)
    '                        'Celda(10) = InsertCellInWorksheet("K", intFila, worksheetPart1)
    '                        'Celda(11) = InsertCellInWorksheet("L", intFila, worksheetPart1)
    '                        'Celda(12) = InsertCellInWorksheet("M", intFila, worksheetPart1)
    '                        'Celda(13) = InsertCellInWorksheet("N", intFila, worksheetPart1)
    '                        'Celda(14) = InsertCellInWorksheet("O", intFila, worksheetPart1)
    '                        'Celda(15) = InsertCellInWorksheet("P", intFila, worksheetPart1)
    '                        'Celda(16) = InsertCellInWorksheet("Q", intFila, worksheetPart1)
    '                        'Celda(17) = InsertCellInWorksheet("R", intFila, worksheetPart1)
    '                        'Celda(18) = InsertCellInWorksheet("S", intFila, worksheetPart1)
    '                        'Celda(19) = InsertCellInWorksheet("T", intFila, worksheetPart1)
    '                        'Celda(20) = InsertCellInWorksheet("U", intFila, worksheetPart1)
    '                        'Celda(21) = InsertCellInWorksheet("V", intFila, worksheetPart1)
    '                        'Celda(22) = InsertCellInWorksheet("W", intFila, worksheetPart1)
    '                        'Celda(23) = InsertCellInWorksheet("X", intFila, worksheetPart1)
    '                        'Celda(24) = InsertCellInWorksheet("Y", intFila, worksheetPart1)
    '                        'Celda(25) = InsertCellInWorksheet("Z", intFila, worksheetPart1)
    '                        'Celda(26) = InsertCellInWorksheet("AA", intFila, worksheetPart1)
    '                        'Celda(27) = InsertCellInWorksheet("AB", intFila, worksheetPart1)
    '                        'Celda(28) = InsertCellInWorksheet("AC", intFila, worksheetPart1)
    '                        'Celda(29) = InsertCellInWorksheet("AD", intFila, worksheetPart1)
    '                        'Celda(30) = InsertCellInWorksheet("AE", intFila, worksheetPart1)
    '                        'Celda(31) = InsertCellInWorksheet("AF", intFila, worksheetPart1)
    '                        'Celda(32) = InsertCellInWorksheet("AG", intFila, worksheetPart1)
    '                        'Celda(33) = InsertCellInWorksheet("AH", intFila, worksheetPart1)
    '                        'Celda(34) = InsertCellInWorksheet("AI", intFila, worksheetPart1)
    '                        'Celda(35) = InsertCellInWorksheet("AJ", intFila, worksheetPart1)
    '                        'Celda(36) = InsertCellInWorksheet("AK", intFila, worksheetPart1)
    '                        'Celda(37) = InsertCellInWorksheet("AL", intFila, worksheetPart1)
    '                        'Celda(38) = InsertCellInWorksheet("AM", intFila, worksheetPart1)
    '                        'Celda(39) = InsertCellInWorksheet("AN", intFila, worksheetPart1)
    '                        'Celda(40) = InsertCellInWorksheet("AO", intFila, worksheetPart1)
    '                        'Celda(41) = InsertCellInWorksheet("AP", intFila, worksheetPart1)
    '                        ''Celda(42) = InsertCellInWorksheet("AQ", intFila, worksheetPart1)
    '                        ''Celda(42) = InsertCellInWorksheet("AR", intFila, worksheetPart1)
    '                        ''Celda(43) = InsertCellInWorksheet("AS", intFila, worksheetPart1)
    '                        ''Celda(44) = InsertCellInWorksheet("AT", intFila, worksheetPart1)
    '                        ''Celda(45) = InsertCellInWorksheet("AU", intFila, worksheetPart1)
    '                        ''Celda(46) = InsertCellInWorksheet("AV", intFila, worksheetPart1)
    '                        ''Celda(47) = InsertCellInWorksheet("AW", intFila, worksheetPart1)
    '                        ''Celda(48) = InsertCellInWorksheet("AX", intFila, worksheetPart1)

    '                        ''Set the value of cell.
    '                        'Celda(0).CellValue = New CellValue(index(0).ToString)
    '                        'Celda(1).CellValue = New CellValue(index(1).ToString)
    '                        'Celda(2).CellValue = New CellValue(index(2).ToString)
    '                        'Celda(3).CellValue = New CellValue(index(3).ToString)
    '                        'Celda(4).CellValue = New CellValue(index(4).ToString)
    '                        'Celda(5).CellValue = New CellValue(index(5).ToString)
    '                        'Celda(6).CellValue = New CellValue(index(6).ToString)
    '                        'Celda(7).CellValue = New CellValue(index(7).ToString)
    '                        ''Celda(8).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(8)).InnerText)
    '                        ''Celda(9).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(9)).InnerText)
    '                        ''Celda(10).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(10)).InnerText)
    '                        ''Celda(11).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(11)).InnerText)
    '                        ''Celda(12).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(12)).InnerText)
    '                        ''Celda(13).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(13)).InnerText)
    '                        ''Celda(14).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(14)).InnerText)
    '                        ''Celda(15).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(15)).InnerText)
    '                        ''Celda(16).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(16)).InnerText)
    '                        'Celda(8).CellValue = New CellValue(index(8).ToString)
    '                        'Celda(9).CellValue = New CellValue(index(9).ToString)
    '                        'Celda(10).CellValue = New CellValue(index(10).ToString)
    '                        'Celda(11).CellValue = New CellValue(index(11).ToString)
    '                        'Celda(12).CellValue = New CellValue(index(12).ToString)
    '                        'Celda(13).CellValue = New CellValue(index(13).ToString)
    '                        'Celda(14).CellValue = New CellValue(index(14).ToString)
    '                        'Celda(15).CellValue = New CellValue(index(15).ToString)
    '                        'Celda(16).CellValue = New CellValue(index(16).ToString)
    '                        'Celda(17).CellValue = New CellValue(index(17).ToString)
    '                        'Celda(18).CellValue = New CellValue(index(18).ToString)
    '                        'Celda(19).CellValue = New CellValue(index(19).ToString)
    '                        'Celda(20).CellValue = New CellValue(index(20).ToString)
    '                        'Celda(21).CellValue = New CellValue(index(21).ToString)
    '                        ''Celda(22).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(22)).InnerText)
    '                        'Celda(22).CellValue = New CellValue(index(22).ToString)
    '                        'Celda(23).CellValue = New CellValue(index(23).ToString)
    '                        'Celda(24).CellValue = New CellValue(index(24).ToString)
    '                        'Celda(25).CellValue = New CellValue(index(25).ToString)
    '                        'Celda(26).CellValue = New CellValue(index(26).ToString)
    '                        'Celda(27).CellValue = New CellValue(index(27).ToString)
    '                        'Celda(28).CellValue = New CellValue(index(28).ToString)
    '                        'Celda(29).CellValue = New CellValue(index(29).ToString)
    '                        'Celda(30).CellValue = New CellValue(index(30).ToString)
    '                        'Celda(31).CellValue = New CellValue(index(31).ToString)
    '                        'Celda(32).CellValue = New CellValue(index(32).ToString)
    '                        'Celda(33).CellValue = New CellValue(index(33).ToString)
    '                        'Celda(34).CellValue = New CellValue(index(34).ToString)
    '                        'Celda(35).CellValue = New CellValue(index(35).ToString)
    '                        'Celda(36).CellValue = New CellValue(index(36).ToString)
    '                        ''Celda(37).CellValue = New CellValue(shareStringPart.SharedStringTable.Elements(index(37)).InnerText)
    '                        'Celda(37).CellValue = New CellValue(index(37).ToString)
    '                        'Celda(38).CellValue = New CellValue(index(38).ToString)
    '                        'Celda(39).CellValue = New CellValue(index(39).ToString)
    '                        'Celda(40).CellValue = New CellValue(index(40).ToString)
    '                        'Celda(41).CellValue = New CellValue(index(41).ToString)

    '                        ''Celda1.CellValue = New CellValue("Nro.Orden Tareo")
    '                        'Celda(0).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(1).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(2).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(3).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(4).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(5).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(6).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(7).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(8).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(9).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(10).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(11).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(12).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(13).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(14).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(15).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(16).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(17).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(18).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(19).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(20).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(21).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(22).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(23).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(24).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(25).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(26).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(27).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(28).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(29).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(30).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(31).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(32).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(33).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(34).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(35).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(36).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(37).DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '                        'Celda(38).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(39).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(40).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '                        'Celda(41).DataType = New EnumValue(Of CellValues)(CellValues.SharedString)

    '                        intFila += 1
    '                    Next

    '                    worksheetPart1.Worksheet.Save()
    '                End Using
    '            End If
    '            'Return ds.Tables(0)
    '            cnx.Close()
    '        Catch ex As Exception
    '            'Return ds.Tables(0)
    '            Throw New Exception(ex.Message)
    '        End Try
    '    Catch ex As Exception
    '        'lblMensaje.Text = ex.Message
    '    End Try
    'End Function

    'Function RegistroVentaContaSisCabecera(ByVal strNombreCampo As String)
    '    Try
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "dFechaEmisionCabeceraDocumento", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "dFechaVencimientoCabeceraDocumento", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "vIdEquivalenciaContableTipoDocumento", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "vIdNumeroSerieDocumentoCabeceraDocumento", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "vIdNumeroCorrelativoCabeceraDocumento", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "cIdTipoDocumentoCliente", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "vNumeroDocumentoClienteCabeceraDocumento", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "vRazonSocialCabeceraDocumento", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "nTotalExporta", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "nTotalMontoGravadoObligatorio", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "nTotalMontoExoneradoObligatorio", True, False)
    '        RegistroVentaContaSisCabecera = IIf(strNombreCampo = "nTotalMontoExoneradoObligatorio", True, False)

    '        Index(0) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionCabeceraDocumento")), shareStringPart)
    '        Index(1) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("")), shareStringPart)
    '        Index(2) = InsertSharedStringItem(fila(""), shareStringPart)
    '        Index(3) = InsertSharedStringItem(fila(""), shareStringPart)
    '        Index(4) = InsertSharedStringItem(fila(""), shareStringPart)
    '        Index(5) = InsertSharedStringItem(fila(""), shareStringPart)
    '        Index(6) = InsertSharedStringItem(fila(""), shareStringPart)
    '        Index(7) = InsertSharedStringItem(fila(""), shareStringPart)
    '        Index(8) = InsertSharedStringItem(Math.Round(fila(""), 2), shareStringPart)
    '        Index(9) = InsertSharedStringItem(Math.Round(fila(""), 2), shareStringPart)
    '        Index(10) = InsertSharedStringItem(Math.Round(fila(""), 2), shareStringPart)
    '        Index(11) = InsertSharedStringItem(Math.Round(fila("nTotalMontoInafectoObligatorio"), 2), shareStringPart)
    '        Index(12) = InsertSharedStringItem(Math.Round(fila("nTotalISCCabeceraDocumento"), 2), shareStringPart)
    '        Index(13) = InsertSharedStringItem(Math.Round(fila("nTotalIGVCabeceraDocumento"), 2), shareStringPart)
    '        Index(14) = InsertSharedStringItem(Math.Round(fila("nTotalMontoGratuitaAdicional"), 2), shareStringPart)
    '        Index(15) = InsertSharedStringItem(Math.Round(fila("nTotalPrecioVentaCabeceraDocumento"), 2), shareStringPart)
    '        Index(16) = InsertSharedStringItem(Math.Round(fila("nTipoCambioCabeceraDocumento"), 4), shareStringPart)
    '        Index(17) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionReferencialCabeceraDocumento")).ToString.Trim, shareStringPart)
    '        Index(18) = InsertSharedStringItem(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim, shareStringPart)
    '        Index(19) = InsertSharedStringItem(fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento").ToString.Trim, shareStringPart)
    '        Index(20) = InsertSharedStringItem(fila("vIdNumeroCorrelativoReferencialCabeceraDocumento").ToString.Trim, shareStringPart)
    '        Index(21) = InsertSharedStringItem(IIf(fila("vIdEquivalenciaContableAbreviadaTipoMoneda").ToString.Trim = "PEN", "S", "D"), shareStringPart)
    '        Index(22) = InsertSharedStringItem(Math.Round(fila("nTotalPrecioVentaCabeceraDocumento") / fila("nTipoCambioCabeceraDocumento"), 2), shareStringPart)
    '        Index(23) = InsertSharedStringItem(String.Format("{0:dd/MM/yyyy}", fila("dFechaVencimientoCabeceraDocumento")).ToString.Trim, shareStringPart)
    '        Index(24) = InsertSharedStringItem(IIf(String.Format("{0:yyyyMMdd}", fila("dFechaEmisionCabeceraDocumento")) <> String.Format("{0:yyyyMMdd}", fila("dFechaVencimientoCabeceraDocumento")), "CRE", "CON"), shareStringPart)
    '        Index(25) = InsertSharedStringItem("", shareStringPart) 'Centro de Costo
    '        Index(26) = InsertSharedStringItem("", shareStringPart) 'Centro de Costo 2
    '        Index(27) = InsertSharedStringItem("70111", shareStringPart) 'Cuenta Contable Base Imponible
    '        Index(28) = InsertSharedStringItem("40111", shareStringPart) 'Cuenta Contable Otros Tributos y Cargos
    '        Index(29) = InsertSharedStringItem("101", shareStringPart) 'Cuenta Contable Total
    '        Index(30) = InsertSharedStringItem("", shareStringPart) 'Regimen Especial
    '        Index(31) = InsertSharedStringItem("", shareStringPart) 'Porcentaje de Regimen Especial
    '        Index(32) = InsertSharedStringItem("", shareStringPart) 'Importe Regimen Especial
    '        Index(33) = InsertSharedStringItem("", shareStringPart) 'Serie Documento Regimen Especial
    '        Index(34) = InsertSharedStringItem("", shareStringPart) 'Número Documento Regimen Especial
    '        Index(35) = InsertSharedStringItem("", shareStringPart) 'Fecha Documento Regimen Especial
    '        Index(36) = InsertSharedStringItem("", shareStringPart) 'Codigo de Presupuesto
    '        Index(37) = InsertSharedStringItem(Math.Round(fila("nPorcentajeImpuestoCabeceraDocumento"), 2), shareStringPart) 'Porcentaje IGV
    '        Index(38) = InsertSharedStringItem("VENTAS DEL MES DE " & UCase(MonthName(Convert.ToDateTime(fila("dFechaEmisionCabeceraDocumento")).Month)), shareStringPart) 'Glosa
    '        Index(39) = InsertSharedStringItem("008", shareStringPart) 'Medio de Pago - Efectivo sin utilizar medio de pago
    '        Index(40) = InsertSharedStringItem("", shareStringPart) 'Condición de Percepción
    '        Index(41) = InsertSharedStringItem("", shareStringPart) 'Importe para cálculo de régimen especial


    '    Catch ex As Exception

    '    End Try
    'End Function

    Public Function fExportarExcel(ByVal NombreArchivo As String, ByVal StoreProcedure As String, ByVal CamposParametros As String, ByVal ValorParametros As String,
                       ByVal TipoSeparador As String, ByVal Ruta As String, ByVal ConCabecera As String) ', ByVal NombreCampo() As String)
        Try
            Dim MiConexion As New clsConexionDAO
            Dim cnx As New SqlConnection(MiConexion.GetCnx)
            Dim cmd As New SqlCommand
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet

            'Se Configura el comando
            cmd.Connection = cnx
            cmd.CommandTimeout = 15000
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = StoreProcedure ' "PA_VTAS_RPT_REGISTROVENTA"


            Dim CamposParametrosValue As String() = Split(CamposParametros, TipoSeparador)
            Dim ValorParametrosValue As String() = Split(ValorParametros, TipoSeparador)
            'ParametrosValue(5) = txtRutaReporte.Text ' txtRutaLogo.Text

            Dim Nombres As String
            Dim i As Integer = 0
            For Each Nombres In CamposParametrosValue
                cmd.Parameters.AddWithValue(Nombres, ValorParametrosValue(i))
                i += 1
            Next

            ''Se crea el objeto Parameters por cada parametro
            'cmd.Parameters.Add(New SqlClient.SqlParameter("@cFechaInicial", SqlDbType.Char, 8))
            'cmd.Parameters.Add(New SqlClient.SqlParameter("@cFechaFinal", SqlDbType.Char, 8))
            'cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))
            'cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))

            ''Se establece los valores por cada parametro
            'cmd.Parameters("@cFechaInicial").Value = FechaInicial
            'cmd.Parameters("@cFechaFinal").Value = FechaFinal
            'cmd.Parameters("@cIdEmpresa").Value = IdEmpresa
            'cmd.Parameters("@cIdPuntoVenta").Value = IdPuntoVenta
            'cmd.Parameters("@cIdTipoResumen").Value = IdTipo

            'Se configura el Adaptador
            da.SelectCommand = cmd
            da.Fill(ds, "DatosExportacion")

            Try
                cnx.Open()
                If ds.Tables("DatosExportacion").Rows.Count > 0 Then
                    Dim NombreArchivoFinal = NombreArchivo & ".xls"

                    'Save the uploaded Excel file.
                    Dim filePath As String = Ruta & "/Downloads/" + NombreArchivo 'Path.GetFileName(FileUpload1.PostedFile.FileName)

                    Using doc As SpreadsheetDocument = SpreadsheetDocument.Create(filePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook)
                        'Get the Worksheet instance.
                        Dim workbookPart = doc.AddWorkbookPart()
                        doc.WorkbookPart.Workbook = New DocumentFormat.OpenXml.Spreadsheet.Workbook()

                        'Colección de hojas
                        doc.WorkbookPart.Workbook.Sheets = New DocumentFormat.OpenXml.Spreadsheet.Sheets()

                        'Agregar la parte de la hoja de trabajo
                        Dim SheetPart = doc.WorkbookPart.AddNewPart(Of WorksheetPart)()
                        Dim SheetData = New DocumentFormat.OpenXml.Spreadsheet.SheetData()
                        SheetPart.Worksheet = New DocumentFormat.OpenXml.Spreadsheet.Worksheet(SheetData)

                        Dim sheets As DocumentFormat.OpenXml.Spreadsheet.Sheets = doc.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets)()
                        Dim relationshipId As String = doc.WorkbookPart.GetIdOfPart(SheetPart)

                        Dim sheetId As UInteger = 1
                        If sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet)().Count() > 0 Then
                            sheetId = sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet)().[Select](Function(s) s.SheetId.Value).Max() + 1
                        End If

                        'Agregar y asociar la hoja
                        Dim sheet As New DocumentFormat.OpenXml.Spreadsheet.Sheet() With {.Id = relationshipId, .sheetId = sheetId, .Name = "RegistroVenta"}
                        sheets.Append(sheet)

                        'Agregar encabezados de columna
                        Dim headerRow As New DocumentFormat.OpenXml.Spreadsheet.Row()
                        Dim Columnas As List(Of [String]) = New List(Of String)()


                        'Dim NombreColumna As String() = Split(CamposParametros, TipoSeparador)
                        'Dim i As Integer = 0
                        'For Each Nombres In CamposParametrosValue
                        '    cmd.Parameters.AddWithValue(Nombres, ValorParametrosValue(i))
                        '    i += 1
                        'Next
                        'Dim NombreColumna As String() = Split(NombreCampo, TipoSeparador)

                        'For Each NomCol In NombreCampo 'NombreColumna
                        'cmd.Parameters.AddWithValue(NomCol, ValorParametrosValue(i))

                        For Each column As DataColumn In ds.Tables("DatosExportacion").Columns
                            'If column.ColumnName = Mid(NomCol, 1, InStrRev(NomCol, "*") - 1) Then
                            '    If strTipo = "Contasis" Then 'Contasis
                            'If (RegistroVentaContaSisCabecera(NomCol) = True And strTipo = "Contasis") Then
                            Columnas.Add(column.ColumnName)
                            If ConCabecera = "SI" Then
                                Dim cell = New Cell()
                                cell.DataType = CellValues.String
                                cell.CellValue = New CellValue(column.ColumnName)
                                headerRow.AppendChild(cell)
                            End If                        'End If
                            '    End If
                            'End If
                        Next
                        'Next
                        If ConCabecera = "SI" Then
                            SheetData.AppendChild(headerRow)
                        End If


                        'Crear filas de datos
                        For Each dsrow As DataRow In ds.Tables("DatosExportacion").Rows
                            Dim newRow As New DocumentFormat.OpenXml.Spreadsheet.Row()
                            For Each col As String In Columnas
                                Dim cell = New Cell()
                                'cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String
                                Dim ColumnType As DataColumn = ds.Tables("DatosExportacion").Columns(col)
                                If ColumnType.DataType.ToString = "System.Decimal" Or ColumnType.DataType.ToString = "System.Int32" Then
                                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number
                                    'Lo quite porque me da error al abrir el archivo XLS
                                    'ElseIf ColumnType.DataType.ToString = "System.DateTime" Then
                                    '    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Date
                                ElseIf ColumnType.DataType.ToString = "System.Boolean" Then
                                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Boolean
                                Else
                                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String
                                End If
                                'Dim ColumnType As DataColumn = ds.Tables("DatosExportacion").Columns(col)
                                'If ColumnType.DataType.ToString = "System.Decimal" Or ColumnType.DataType.ToString = "System.Int32" Then
                                '    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number
                                'ElseIf ColumnType.DataType.ToString = "System.DateTime" Then
                                '    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Date
                                'ElseIf ColumnType.DataType.ToString = "System.Boolean" Then
                                '    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Boolean
                                'Else
                                '    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.[String]
                                'End If
                                'cell.CellValue = New CellValue(IIf(cell.DataType = "d", CDate(dsrow(col).ToString()), dsrow(col).ToString()))
                                cell.CellValue = New CellValue(dsrow(col).ToString())

                                'cell.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow(col).ToString())
                                'For Each NomCol In NombreCampo 'NombreColumna
                                '    If col = Mid(NomCol, 1, InStrRev(NomCol, "*") - 1) Then
                                '        Dim valor As String : valor = ""
                                '        If Mid(NomCol, InStrRev(NomCol, "*") + 1, Len(NomCol)) <> "" Then
                                '            valor = IIf(ColumnType.DataType.ToString = "System.DateTime", String.Format(Mid(NomCol, InStrRev(NomCol, "*") + 1, Len(NomCol)), dsrow(col)), dsrow(col).ToString())
                                '        Else
                                '            valor = dsrow(col).ToString
                                '        End If
                                '        cell.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(valor)
                                '        Exit For
                                '    End If
                                'Next
                                newRow.AppendChild(cell)
                            Next
                            doc.WorkbookPart.WorksheetParts.First().Worksheet.First().AppendChild(newRow)
                            'Spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet.First().AppendChild(newRow)
                            'SheetData.AppendChild(newRow)
                        Next
                        SheetPart.Worksheet.Save()
                    End Using
                End If
                'Return ds.Tables(0)
                cnx.Close()
            Catch ex As Exception
                'Return ds.Tables(0)
                Throw New Exception(ex.Message)
            End Try
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

    Public Function fNroSemanaAno(ByVal dFecha As Date)
        Dim strFecha, strAno, strNroSemana As String
        strFecha = String.Format("{0:yyyyMMdd}", dFecha) '  Format(Of Date, "yyyymmdd")()
        strNroSemana = DatePart("ww", dFecha, vbMonday, vbFirstFourDays)
        'no_sema = DatePart("ww", Date, vbMonday, vbFirstFourDays)
        strAno = Mid(strFecha, 1, 4)
        fNroSemanaAno = strAno & strNroSemana
    End Function

    Public Function fUploadFile(ByVal Server As String, ByVal User As String, ByVal Pass As String, ByVal PathOrigen As String, ByVal FileName As String) As Boolean
        Try
            Dim Client As WebClient = New WebClient
            Client.Credentials = New NetworkCredential(User, Pass)
            'Dim PathDestino As String = "ftp://" + Server + "/EcomNet/"
            Dim PathDestino As String = "ftp://" + Server
            'Dim Url = Server + PathDestino + FileName
            Dim Url = PathDestino + FileName
            'client.UploadFile("ftp://ftp.example.com/remote/path/file.zip", "C:\local\path\file.zip")
            Client.UploadFile(Url, PathOrigen + FileName)

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

End Class