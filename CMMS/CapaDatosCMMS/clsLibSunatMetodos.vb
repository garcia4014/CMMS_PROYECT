'Contribuyente.cs : Clase para consultar informacion de un contribuyente a partir de su RUC
'Author: www.bimsic.com
'Copyright (c) 2014

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
'Imports System.Drawing
Imports CapaDatosCMMS
'Imports CapaDatosVtas
'Se usa para generar el certificado y el formato XML SunatConfiguracionCPE
'Imports System.Security
'Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml
Imports System.Xml.Schema 'Nuevo

'Para comprimir
Imports Ionic.Zip

'Para devolver código HTML
Imports HtmlAgilityPack

'Para la Seguridad WebServices Sunat
'Imports System.Xml
Imports Microsoft.Web.Services3.Security.Tokens 'Para usar el UsernameToken
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Dispatcher
Imports Microsoft.Web.Services3
Imports Microsoft.Web
'Imports Programa.ServiceFacturacion.billServiceClient
Imports System.ServiceModel.Description
'Imports System.Net
Imports System.ServiceModel 'Para usar el EndpointAddress
Imports System.IdentityModel 'Para usar el CreateSecurityTokenManager






Imports System.Data
Imports System.Configuration
Imports System.Web.Security
Imports System.Web.Services.Discovery
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

'imports Microsoft.Web.Services3.WebServicesClientProtocol

'Inicio: Para Utilizar la Guia de Remisión: 26/12/2022
Imports Newtonsoft.Json
Imports RestSharp
Imports System.Security.Cryptography 'Para poder usar el SHA256Managed
'Final: Para Utilizar la Guia de Remisión: 26/12/2022

Public Class clsLibSunatMetodos
    'Public Structure stSunatRUC
    '    Dim Ruc As String
    '    Dim RazonSocial As String
    '    Dim AntiguoRuc As String
    '    Dim Estado As String
    '    Dim EsAgenteRetencion As String
    '    Dim NombreComercial As String
    '    Dim Direccion As String
    '    Dim Telefono As String
    '    Dim Dependencia As String
    '    Dim Tipo As String
    'End Structure
    Public Structure stSunatDNI
        Dim Dni As String
        Dim RazonSocial As String
        Dim GrupoVotacion As String
        Dim Departamento As String
        Dim Provincia As String
        Dim Distrito As String
    End Structure

    Public Structure stPlanCuentaSunat
        Dim Codigo As String
        Dim Descripcion As String
    End Structure

    Public Structure stCPE
        Dim IdTipoDocumento As String
        Dim IdTipoDocumentoSunat As String
        Dim IdNumeroSerieDocumento As String
        Dim IdNumeroCorrelativo As String
        Dim IdNumeroCorrelativoFinal As String
        Dim IdEmpresa As String
        Dim IdPuntoVenta As String
        'Dim NombreCertificado As String
        Dim RutaGeneral As String
        Dim Ruc As String
        Dim XML As String 'Estructura XML
        Dim NombreArchivo As String
        Dim FechaProceso As DateTime 'Fecha de Proceso para el Resumen
        Dim NroTicket As String 'Nro. de Ticket que devuelve la SUNAT del Resumen
        Dim CodigoRespuesta As String
        Dim Respuesta As String
        Dim NombreCertificado As String 'Nombre del Certificado Digital
        Dim IdCondicion As String 'Valor para el Resumen /SummaryDocuments/sac:SummaryDocumentsLine/cac:Status/cbc:ConditionCode
        Dim CodigoHash As String
        Dim CodigoSignature As String
        Dim IdTipoDocumentoReferencial As String
        Dim IdTipoDocumentoSunatReferencial As String
        Dim IdNumeroSerieDocumentoReferencial As String
        Dim IdNumeroCorrelativoReferencial As String
        Dim MotivoBaja As String 'Utilizado para indicar el motivo por el que se está anulando el documento.
        Dim urlHashCDR As String 'Hash del CDR de retorno para poder generarl el nuevo QR del API-Rest
    End Structure

    'Const urlinforuc As String = "http://www.sunat.gob.pe/w/wapS01Alias?ruc="
    'http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc=20550409667&codigo=TGTR
    'Const urlinforuc As String = "http://localhost:51778/wsvSunatRUC.asmx/GetSunatRUC?ruc="
    Const urlinforuc As String = "http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc={0}&codigo={1}&tipdoc=1"
    'Const urlinforuc As String = "http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc="
    'Const urlinforuc As String = "http://www.miasoftware.net/consultaruc/consultar.php?ruc="
    'Const urlinforuc As String = "http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc="
    'Const urlinfocaptcha As String = "http://www.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image"
    Private _WebRequest As HttpWebRequest
    Private _WebResponse As HttpWebResponse
    Private _WebSource As String
    Private _ok As Boolean
    Private _error As String

    Private Function LoadWebSource(ByVal url As String, ByVal captcha As String)
        'Private Function LoadWebSource(ByVal url As String)
        '_WebRequest = WebRequest.Create(url + "&codigo=" + captcha)
        _WebRequest = WebRequest.Create(url + "&codigo=" + captcha)
        _WebRequest.Proxy = Nothing
        Try
            _WebResponse = _WebRequest.GetResponse
        Catch ex As Exception
            _ok = False
            _error = "Error al consultar con Sunat"
            Return False
        End Try
        Dim _Stream = _WebResponse.GetResponseStream
        Dim encode = System.Text.Encoding.GetEncoding("utf-8")
        Dim _StreamReader = New StreamReader(_Stream, encode)

        _WebSource = WebUtility.HtmlDecode(_StreamReader.ReadToEnd)

        Return (True)
    End Function

    Public Function SunatMigrarOPTComprasTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, ByVal ImporteBaseImponible As Decimal, ByVal IdTipoDetalle As String)

        Dim strSQL As String = "EXEC PA_CTBL_RPT_OPT_REGISTROCOMPRA '" & IdPeriodo & "', '" & IdMesInicial & "', '" & IdMesFinal & "', '" &
                               "" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & IdTipoMoneda & "', '" & Orden & "', " & ImporteBaseImponible & ", '" & IdTipoDetalle & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")

        If ds.Tables("Reporte").Rows.Count > 0 Then
            Dim fila As DataRow
            Const fichero As String = "C:\SIW_NEW\slnSIW\CTBL\Downloads\Costos.txt"
            'Const fichero As String = "C:\inetpub\wwwroot\SIW_NEW\slnSIW\CTBL\Downloads\Costos.txt"
            Dim texto As String = ""
            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("Reporte").Rows
                texto = fila("nNumero") & "|" & fila("vIdEquivalenciaContableTipoDocumentoDeclarante") & "|" & fila("vRucEmpresa") & "|" & fila("cIdPeriodoAsiento") & "|" &
                        fila("cIdEquivalenciaContableTipoPersona") & "|" & fila("vIdEquivalenciaContableTipoDocumentoClienteProveedor") & "|" &
                        fila("cIdClienteProveedor") & "|" & fila("nImporteTotal") & "|" & SunatConversionCaracteresEspeciales(fila("vApellidoPaternoProveedor")) & "|" & SunatConversionCaracteresEspeciales(fila("vApellidoMaternoProveedor")) & "|" &
                        SunatConversionCaracteresEspeciales(fila("vNombres1Proveedor")) & "|" & SunatConversionCaracteresEspeciales(fila("vNombres2Proveedor")) & "|" & SunatConversionCaracteresEspeciales(fila("vRazonSocialAsiento")) & "|" ' & vbCrLf
                sw.WriteLine(texto)
            Next
            sw.Close()
        End If
    End Function

    Public Function SunatMigrarOPTVentasTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, ByVal ImporteBaseImponible As Decimal, ByVal IdTipoDetalle As String)

        Dim strSQL As String = "EXEC PA_CTBL_RPT_OPT_REGISTROVENTA '" & IdPeriodo & "', '" & IdMesInicial & "', '" & IdMesFinal & "', '" &
                               "" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & IdTipoMoneda & "', '" & Orden & "', " & ImporteBaseImponible & ", '" & IdTipoDetalle & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")

        If ds.Tables("Reporte").Rows.Count > 0 Then
            Dim fila As DataRow
            Const fichero As String = "C:\SIW_NEW\slnSIW\CTBL\Downloads\Ingresos.txt"
            'Const fichero As String = "C:\inetpub\wwwroot\SIW_NEW\slnSIW\CTBL\Downloads\Ingresos.txt"
            Dim texto As String = ""
            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("Reporte").Rows
                texto = fila("nNumero") & "|" & fila("vIdEquivalenciaContableTipoDocumentoDeclarante") & "|" & fila("vRucEmpresa") & "|" & fila("cIdPeriodoAsiento") & "|" &
                        fila("cIdEquivalenciaContableTipoPersona") & "|" & fila("vIdEquivalenciaContableTipoDocumentoClienteProveedor") & "|" &
                        fila("cIdClienteProveedor") & "|" & fila("nImporteTotal") & "|" & SunatConversionCaracteresEspeciales(fila("vApellidoPaternoCliente")) & "|" & SunatConversionCaracteresEspeciales(fila("vApellidoMaternoCliente")) & "|" &
                        SunatConversionCaracteresEspeciales(fila("vNombres1Cliente")) & "|" & SunatConversionCaracteresEspeciales(fila("vNombres2Cliente")) & "|" & SunatConversionCaracteresEspeciales(fila("vRazonSocialAsiento")) & "|" ' & vbCrLf
                sw.WriteLine(texto)
            Next
            sw.Close()
        End If
    End Function

    Public Function SunatMigrarAnexosPDTAnualTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                               ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                               ByVal ImporteBaseImponible As Decimal, ByVal IdNumeroCuentaAsiento As String,
                               ByVal NroCasilla As String)

        Dim strSQL As String = "EXEC PA_CTBL_RPT_ANEXOS_PDT_ANUAL_TXT '" & IdPeriodo & "', '" & IdMesInicial & "', '" & IdMesFinal & "', '" &
                               "" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & IdTipoMoneda & "', " & ImporteBaseImponible & ", '" & IdNumeroCuentaAsiento & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")

        If ds.Tables("Reporte").Rows.Count > 0 Then
            Dim fila As DataRow
            'ds.Tables("Asiento").Rows(0).Item("cNumeroAsiento")
            Dim strRUCEmpresa As String
            strRUCEmpresa = ds.Tables("Reporte").Rows(0).Item("vRucEmpresa")
            Dim fichero As String = "C:\SIW_NEW\slnSIW\CTBL\Downloads\0684" & strRUCEmpresa & NroCasilla & ".txt"
            'Const fichero As String = "C:\inetpub\wwwroot\SIW_NEW\slnSIW\CTBL\Downloads\Ingresos.txt"
            Dim texto As String = ""
            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("Reporte").Rows
                'texto = fila("nNumero") & "|" & fila("vIdEquivalenciaContableTipoDocumentoDeclarante") & "|" & fila("vRucEmpresa") & "|" & fila("cIdPeriodoAsiento") & "|" & _
                '        fila("cIdEquivalenciaContableTipoPersona") & "|" & fila("vIdEquivalenciaContableTipoDocumentoClienteProveedor") & "|" & _
                '        fila("cIdClienteProveedor") & "|" & fila("nImporteTotal") & "|" & Trim(fila("vApellidoPaternoCliente")) & "|" & Trim(fila("vApellidoMaternoCliente")) & "|" & _
                '        Trim(fila("vNombres1Cliente")) & "|" & Trim(fila("vNombres2Cliente")) & "|" & Trim(fila("vRazonSocialAsiento")) & "|" ' & vbCrLf

                texto = fila("vIdEquivalenciaContableTipoDocumentoClienteProveedor") & "|" & fila("cIdClienteProveedor") & "|" & fila("cIdIndicadorIngreso") & "|" & SunatConversionCaracteresEspeciales(fila("vApellidoPaternoCliente")) & "|" &
                        SunatConversionCaracteresEspeciales(fila("vApellidoMaternoCliente")) & "|" & SunatConversionCaracteresEspeciales(Trim(fila("vNombres1Cliente")) & " " & SunatConversionCaracteresEspeciales(fila("vNombres2Cliente"))) & "|" & SunatConversionCaracteresEspeciales(fila("vRazonSocialAsiento")) & "|" & fila("nImporteTotal") & "|"
                sw.WriteLine(texto)
            Next
            sw.Close()
        End If
    End Function

    Public Function SunatConversionCaracteresEspeciales(ByVal Cadena As String) As String
        Cadena = Replace(Cadena, "Ñ", "N")
        Cadena = Replace(Cadena, "Á", "A")
        Cadena = Replace(Cadena, "É", "E")
        Cadena = Replace(Cadena, "Í", "I")
        Cadena = Replace(Cadena, "Ó", "O")
        Cadena = Replace(Cadena, "Ú", "U")
        Cadena = Replace(Cadena, "'", "´")
        Cadena = Replace(Cadena, "&", "")
        SunatConversionCaracteresEspeciales = Cadena
    End Function

    Public Function SunatMigrarRegistroVentasTXT(ByVal IdPeriodo As String, ByVal IdMes As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, Ruta As String) As String
        Dim strNombreArchivo = ""
        Dim fichero As String = ""
        Dim texto As String = ""

        Dim strSQL As String = "EXEC PA_CTBL_RPT_REGISTROVENTA '" & IdPeriodo & "', '" & IdMes & "', '" &
                               "" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & IdTipoMoneda & "', '" & Orden & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")
        'O - Indicador de operaciones.
        'I - Indicador del contenido del libro o registro.
        'M - Indicador de la moneda utilizada.
        'G - Indicador de libro electrónico generado por el PLE.
        Dim O, I, M, G As String
        O = "1"
        I = "0"
        M = IIf(IdTipoMoneda = "S", "1", "2")
        G = "1"
        If ds.Tables("Reporte").Rows.Count > 0 Then
            I = "1"
            Dim fila As DataRow

            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & ds.Tables("Reporte").Rows(0).Item("cIdPeriodoAsiento") & ds.Tables("Reporte").Rows(0).Item("cIdMesAsiento") &
                               "0014010000" & O & I & M & G
            '"00140100001111"
            'Dim fichero As String = "C:\SIW_NEW\slnSIW\CTBL\Downloads\" & strNombreArchivo & ".txt"
            fichero = Ruta & strNombreArchivo & ".txt"

            'Const fichero As String = "C:\inetpub\wwwroot\SIW_NEW\slnSIW\CTBL\Downloads\Ingresos.txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("Reporte").Rows
                texto = fila("cIdPeriodoAsiento") & fila("cIdMesAsiento") & "00|" & fila("cIdTipoLibro") & Trim(fila("cNumeroAsiento")) & "|" &
                        Trim(fila("cNumeroLineaAsiento")) & "|" & fila("dFechaDocumentoAsiento") & "|" & fila("dFechaVencimientoAsiento") & "|" &
                        fila("vIdEquivalenciaContableTipoDocumento") & "|" & IIf(fila("vIdNumeroSerieDocumentoAsiento").ToString.Trim = "", "", fila("vIdNumeroSerieDocumentoAsiento").ToString.Trim) & "|" & fila("vIdNumeroDocumentoAsiento") & "|" &
                        "" & "|" & IIf(fila("cIdClienteProveedor").ToString.Trim = "00000000000", "", Convert.ToInt16(fila("vTipoDocumentoClienteProveedor"))) & "|" & IIf(fila("cIdClienteProveedor").ToString.Trim = "00000000000", "", fila("cIdClienteProveedor").ToString.Trim) & "|" & SunatConversionCaracteresEspeciales(fila("vRazonSocialAsiento")) & "|" &
                        "" & "|" & fila("nBaseImponible") & "|" & "" & "|" & fila("nIGV") & "|" & fila("nVentaNoGravada") & "|" & "" & "|" &
                        "" & "|" & "" & "|" & "" & "|" & "" & "|" & "" & "|" &
                        fila("nTotalVenta") & "|" & fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "|" & Mid(fila("nTipoCambioAsiento"), 1, 5) & "|" & fila("dFechaDocumentoRefAsiento") & "|" &
                        IIf(fila("cIdTipoDocumentoRefAsiento").ToString.Trim = "00", "", fila("cIdTipoDocumentoRefAsiento").ToString.Trim) & "|" & IIf(fila("cIdTipoDocumentoRefAsiento").ToString.Trim = "00", "", IIf(fila("vIdNumeroSerieDocumentoRefAsiento").ToString.Trim = "", "", fila("vIdNumeroSerieDocumentoRefAsiento").ToString.Trim)) & "|" & IIf(fila("cIdTipoDocumentoRefAsiento").ToString.Trim = "00", "", fila("vIdNumeroDocumentoRefAsiento").ToString.Trim) & "|" & "" & "|" &
                        "" & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumento") = "07" Or fila("vIdEquivalenciaContableTipoDocumento") = "08", "", "1") & "|" &
                        fila("cIdIndicadorLibroElectronicoAsiento") & "|"
                sw.WriteLine(texto)
            Next
            sw.Close()
            SunatMigrarRegistroVentasTXT = strNombreArchivo & ".txt"
        Else
            Dim EmpData As New clsEmpresaMetodos
            Dim Empresa As GNRL_EMPRESA = EmpData.EmpresaListarPorId(IdEmpresa)
            strNombreArchivo = "LE" & Empresa.vRucEmpresa & IdPeriodo & IdMes &
                               "0014010000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            If ds.Tables("Reporte").Rows.Count < 0 Then
                texto = ""
                sw.WriteLine(texto)
            End If
            sw.Close()
            SunatMigrarRegistroVentasTXT = strNombreArchivo & ".txt"
        End If
    End Function

    Public Function SunatVtasLEMigrarRegistroVentasTXT(ByVal FechaInicial As String, ByVal FechaFinal As String,
                                                        ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                                        Ruta As String) As String
        Dim strNombreArchivo = ""
        Dim fichero As String = ""
        Dim texto As String = ""
        Dim IdPeriodo, IdMes As String
        IdPeriodo = FechaInicial.Substring(0, 4)
        IdMes = FechaInicial.Substring(4, 2)

        Dim strSQL As String = "EXEC PA_VTAS_RPT_REGISTROVENTA '" & FechaInicial & "', '" & FechaFinal & "', '" &
                               "" & IdEmpresa & "', '" & IdPuntoVenta & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")
        'O - Indicador de operaciones.
        'I - Indicador del contenido del libro o registro.
        'M - Indicador de la moneda utilizada.
        'G - Indicador de libro electrónico generado por el PLE.
        Dim O, I, M, G As String
        O = "1"
        I = "0"
        M = IIf(IdTipoMoneda = "S", "1", "2")
        G = "1"
        If ds.Tables("Reporte").Rows.Count > 0 Then
            I = "1"
            Dim fila As DataRow

            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & IdPeriodo & IdMes &
                               "0014010000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            Dim intNroAsiento As Integer = 0
            Dim strNroAsiento, strNroLineaAsiento, strIdIndicadorLibroElectronicoAsiento, strNroContrato As String
            For Each fila In ds.Tables("Reporte").Rows
                intNroAsiento = intNroAsiento + 1
                strNroAsiento = String.Format("{0:00000}", intNroAsiento)
                strNroLineaAsiento = "M" & String.Format("{0:00000}", intNroAsiento)
                strNroContrato = ""
                texto = IdPeriodo & IdMes & "00|" & "03" & strNroAsiento & "|" &
                        strNroLineaAsiento & "|" & String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionCabeceraDocumento")) & "|" & String.Format("{0:dd/MM/yyyy}", fila("dFechaVencimientoCabeceraDocumento")) & "|" &
                        fila("vIdEquivalenciaContableTipoDocumento") & "|" & IIf(fila("vIdNumeroSerieDocumentoCabeceraDocumento").ToString.Trim = "", "", fila("vIdNumeroSerieDocumentoCabeceraDocumento").ToString.Trim) & "|" & fila("vIdNumeroCorrelativoCabeceraDocumento") & "|" &
                        "" & "|" & Convert.ToInt16(fila("cIdTipoDocumentoCliente")) & "|" & fila("vNumeroDocumentoClienteCabeceraDocumento").ToString.Trim & "|" & SunatConversionCaracteresEspeciales(fila("vRazonSocialCabeceraDocumento")) & "|" &
                        "" & "|" & Math.Round(fila("nTotalMontoGravadoObligatorio"), 2) & "|" & "0.00" & "|" & Math.Round(fila("nTotalIGVCabeceraDocumento"), 2) & "|" & "0.00" & "|" & Math.Round(fila("nTotalMontoExoneradoObligatorio"), 2) & "|" &
                        Math.Round(fila("nTotalMontoInafectoObligatorio"), 2) & "|" & Math.Round(fila("nTotalISCCabeceraDocumento"), 2) & "|" & "0.00" & "|" & "0.00" & "|" & "0.00" & "|" & "0.00" & "|" &
                        Math.Round(fila("nTotalPrecioVentaCabeceraDocumento"), 2) & "|" & fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "|" & Mid(fila("nTipoCambioCabeceraDocumento"), 1, 5) & "|" &
                        fila("dFechaEmisionReferencialCabeceraDocumento") & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "00", "", fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim) & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "00", "", IIf(fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento").ToString.Trim = "", "", fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento").ToString.Trim)) & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "00", "", fila("vIdNumeroCorrelativoReferencialCabeceraDocumento").ToString.Trim) & "|" &
                        strNroContrato & "|" &
                        "" & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "07" Or fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "08", "1", "") & "|" &
                        fila("cIdIndicadorEstadoLibroElectronicoCabeceraDocumento") & "|"

                sw.WriteLine(texto)
            Next
            sw.Close()
            SunatVtasLEMigrarRegistroVentasTXT = strNombreArchivo & ".txt"
        Else
            Dim EmpData As New clsEmpresaMetodos
            Dim Empresa As GNRL_EMPRESA = EmpData.EmpresaListarPorId(IdEmpresa)
            strNombreArchivo = "LE" & Empresa.vRucEmpresa & IdPeriodo & IdMes &
                               "0014010000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            If ds.Tables("Reporte").Rows.Count < 0 Then
                texto = ""
                sw.WriteLine(texto)
            End If
            sw.Close()
            SunatVtasLEMigrarRegistroVentasTXT = strNombreArchivo & ".txt"
        End If
    End Function

    Public Function SunatVtasLEMigrarRegistroVentasSimplificadoTXT(ByVal FechaInicial As String, ByVal FechaFinal As String,
                                                        ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                                        Ruta As String) As String
        Dim strNombreArchivo = ""
        Dim fichero As String = ""
        Dim texto As String = ""
        Dim IdPeriodo, IdMes As String
        IdPeriodo = FechaInicial.Substring(0, 4)
        IdMes = FechaInicial.Substring(4, 2)

        Dim strSQL As String = "EXEC PA_VTAS_RPT_REGISTROVENTA '" & FechaInicial & "', '" & FechaFinal & "', '" &
                               "" & IdEmpresa & "', '" & IdPuntoVenta & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")
        'O - Indicador de operaciones.
        'I - Indicador del contenido del libro o registro.
        'M - Indicador de la moneda utilizada.
        'G - Indicador de libro electrónico generado por el PLE.
        Dim O, I, M, G As String
        O = "1"
        I = "0"
        M = IIf(IdTipoMoneda = "S", "1", "2")
        G = "1"
        If ds.Tables("Reporte").Rows.Count > 0 Then
            I = "1"
            Dim fila As DataRow

            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & IdPeriodo & IdMes &
                               "0014020000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"

            Dim sw As New System.IO.StreamWriter(fichero)
            Dim intNroAsiento As Integer = 0
            Dim strNroAsiento, strNroLineaAsiento, strIdIndicadorLibroElectronicoAsiento, strNroContrato As String
            For Each fila In ds.Tables("Reporte").Rows
                intNroAsiento = intNroAsiento + 1
                strNroAsiento = String.Format("{0:00000}", intNroAsiento)
                strNroLineaAsiento = "M" & String.Format("{0:00000}", intNroAsiento)
                strNroContrato = ""
                texto = IdPeriodo & IdMes & "00|" & "03" & strNroAsiento & "|" &
                        strNroLineaAsiento & "|" & String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionCabeceraDocumento")) & "|" & String.Format("{0:dd/MM/yyyy}", fila("dFechaVencimientoCabeceraDocumento")) & "|" &
                        fila("vIdEquivalenciaContableTipoDocumento") & "|" & IIf(fila("vIdNumeroSerieDocumentoCabeceraDocumento").ToString.Trim = "", "", fila("vIdNumeroSerieDocumentoCabeceraDocumento").ToString.Trim) & "|" & fila("vIdNumeroCorrelativoCabeceraDocumento") & "|" &
                        "" & "|" & Convert.ToInt16(fila("cIdTipoDocumentoCliente")) & "|" & fila("vNumeroDocumentoClienteCabeceraDocumento").ToString.Trim & "|" & SunatConversionCaracteresEspeciales(fila("vRazonSocialCabeceraDocumento")) & "|" &
                        Math.Round(fila("nTotalMontoGravadoObligatorio"), 2) & "|" & Math.Round(fila("nTotalIGVCabeceraDocumento"), 2) & "|" & "0.00" & "|" & "0.00" & "|" &
                        Math.Round(fila("nTotalPrecioVentaCabeceraDocumento"), 2) & "|" & fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "|" & Mid(fila("nTipoCambioCabeceraDocumento"), 1, 5) & "|" &
                        String.Format("{0:dd/MM/yyyy}", fila("dFechaEmisionReferencialCabeceraDocumento")) & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "00", "", fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim) & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "00", "", IIf(fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento").ToString.Trim = "", "", fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento").ToString.Trim)) & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "00", "", fila("vIdNumeroCorrelativoReferencialCabeceraDocumento").ToString.Trim) & "|" &
                        "" & "|" & IIf(fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "07" Or fila("vIdEquivalenciaContableTipoDocumentoReferencial").ToString.Trim = "08", "1", "") & "|" &
                        fila("cIdIndicadorEstadoLibroElectronicoCabeceraDocumento") & "|"

                sw.WriteLine(texto)
            Next
            sw.Close()
            SunatVtasLEMigrarRegistroVentasSimplificadoTXT = strNombreArchivo & ".txt"
        Else
            Dim EmpData As New clsEmpresaMetodos
            Dim Empresa As GNRL_EMPRESA = EmpData.EmpresaListarPorId(IdEmpresa)
            strNombreArchivo = "LE" & Empresa.vRucEmpresa & IdPeriodo & IdMes &
                               "0014010000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            If ds.Tables("Reporte").Rows.Count < 0 Then
                texto = ""
                sw.WriteLine(texto)
            End If
            sw.Close()
            SunatVtasLEMigrarRegistroVentasSimplificadoTXT = strNombreArchivo & ".txt"
        End If
    End Function

    'Public Function SunatConfiguracionCPE(stDocumento As stCPE, strPasswordCertificado As String, intTipo As Integer, strAmbiente As String) As stCPE
    '    Dim strRutaXML As String 'Se asigna la ruta donde se guardará los archivos XML
    '    Dim strRutaFirma As String 'Se asigna la ruta donde se encuentra la firma
    '    Dim strRutaFirmaXML As String 'Se asigna la ruta donde se guardará los archivos XML Firmados

    '    '=======================Configuración de Rutas============================
    '    strRutaXML = Replace(stDocumento.RutaGeneral & "\DocXML\" & Convert.ToString(Year(stDocumento.FechaProceso)) & "\" & String.Format("{0:00}", Month(stDocumento.FechaProceso)), "\\", "\") '"D:\\SIW_NEW\\slnSIW2018\\slnSIW\\VTAS\\DocXML"
    '    strRutaFirma = Replace(stDocumento.RutaGeneral & "\Certificados", "\\", "\") '"D:\\SIW_NEW\\slnSIW2018\\slnSIW\\VTAS\\Certificados"
    '    strRutaFirmaXML = Replace(stDocumento.RutaGeneral & "\DocXMLFirmado\" & Convert.ToString(Year(stDocumento.FechaProceso)) & "\" & String.Format("{0:00}", Month(stDocumento.FechaProceso)), "\\", "\") '"D:\\SIW_NEW\\slnSIW2018\\slnSIW\\VTAS\\DocXMLFirmado"

    '    ''1 - Enviar / Analizar Factura Online.
    '    ''2 - Enviar / Recuperar Ticket Resumen Comprobantes.
    '    ''3 - Analizar Resumen Comprobantes x Tickets.
    '    ''4 - Reenviar / Factura Online.
    '    ''5 - Reprocesar / Generar XML, XML Firmado, PDF, sin enviar a Sunat.
    '    ''6 - Analizar Factura con el GetStatusCDR
    '    ''7 - Enviar Comunicación de Baja - Facturas.
    '    ''8 - Analizar Comunicación de Baja - Facturas.
    '    ''9 - Generar Guía de Remisión Online.
    '    ''10 - Reprocesar / Generar XML-Baja sin enviar a Sunat.
    '    ''10 - Generar XML Baja sin enviar a Sunat - Guías (Este aún no lo genera la SUNAT.
    '    ''11 - Reprocesar / Generar XML, XML Firmado, PDF, sin enviar a Sunat - Guías.
    '    ''12 - Reenviar / Guia Remisión.
    '    ''13 - Analizar / Guia Remisión.
    '    If (intTipo = 1 Or intTipo = 5) And Trim(stDocumento.IdNumeroCorrelativoFinal) = "" Then
    '        stDocumento = GenerarCPEXML(stDocumento, strRutaXML)
    '    ElseIf intTipo = 2 Then
    '        stDocumento = GenerarCPEXMLResumen(stDocumento, strRutaXML)
    '    ElseIf intTipo = 7 Or intTipo = 10 Then
    '        stDocumento = GenerarCPEXMLBaja(stDocumento, strRutaXML)
    '    ElseIf (intTipo = 9 Or intTipo = 11) Then 'Generar Guía
    '        'stDocumento = GenerarGuiaEXML(stDocumento, strRutaXML)
    '        stDocumento = GenerarGuiaRemitenteXML(stDocumento, strRutaXML)
    '    End If
    '    If intTipo = 1 Or intTipo = 2 Or intTipo = 5 Or intTipo = 7 Or intTipo = 9 Or intTipo = 10 Or intTipo = 11 Then
    '        GenerarFirmaXML(stDocumento, strRutaFirma, strPasswordCertificado, strRutaXML, strRutaFirmaXML)
    '    End If

    '    If stDocumento.Respuesta = "" And intTipo <> 9 And intTipo <> 11 And intTipo <> 12 And intTipo <> 13 Then
    '        stDocumento = GenerarZIPEnvio(stDocumento, strRutaFirmaXML, intTipo, strAmbiente) ', "0")
    '    ElseIf intTipo = 9 Or intTipo = 11 Or intTipo = 12 Then 'Guía de Remisión JMUG: 27/12/2022
    '        stDocumento = GenerarZIPEnvioGuia(stDocumento, strRutaFirmaXML, intTipo, strAmbiente)
    '    End If
    '    If stDocumento.Respuesta = "" And intTipo <> 5 And intTipo <> 10 And intTipo <> 11 And intTipo <> 12 Then
    '        If intTipo <> 9 And intTipo <> 13 Then
    '            stDocumento = GenerarAnalizarEnvio(stDocumento, strRutaFirmaXML, intTipo)
    '        ElseIf intTipo = 13 Then
    '            stDocumento = GenerarAnalizarEnvioAPI(stDocumento, strRutaFirmaXML, intTipo, strAmbiente)
    '        End If
    '    End If
    '    Return stDocumento
    'End Function

    Public Function GenerarFirmaXML(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRutaCertificado As String, ByVal strPasswordCertificado As String, ByVal strRutaXML As String,
                                    ByVal strRutaGuardarCertificado As String)
        'Cargar Certificados
        Dim Certificado As New System.Security.Cryptography.X509Certificates.X509Certificate2(strRutaCertificado & "\" & stDocumento.NombreCertificado & ".pfx", strPasswordCertificado, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable)
        'Carcar el XML
        Dim xmlDoc = New System.Xml.XmlDocument
        xmlDoc.PreserveWhitespace = True
        xmlDoc.Load(strRutaXML & "/" & stDocumento.NombreArchivo & ".xml")
        'Nombre del nodo donde se inyecta la firma "ExtensionContent"
        Dim nodoExtension =
                xmlDoc.GetElementsByTagName(
                    "ExtensionContent",
                    "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2").Item(0)

        Dim signedXml As SignedXml = New SignedXml(xmlDoc)
        signedXml.SigningKey = Certificado.PrivateKey

        Dim xmlSignature = signedXml.Signature

        Dim env = New XmlDsigEnvelopedSignatureTransform()

        Dim reference = New Reference(String.Empty)
        reference.AddTransform(env)
        xmlSignature.SignedInfo.AddReference(reference)

        Dim KeyInfo As KeyInfo = New KeyInfo()
        Dim x509Data As KeyInfoX509Data = New KeyInfoX509Data(Certificado)

        x509Data.AddSubjectName(Certificado.Subject)

        KeyInfo.AddClause(x509Data)
        xmlSignature.KeyInfo = KeyInfo
        xmlSignature.Id = "Sunat"
        signedXml.ComputeSignature()

        nodoExtension.AppendChild(signedXml.GetXml())

        Dim settings As New XmlWriterSettings()

        settings.Indent = True
        settings.IndentChars = "    "
        settings.Encoding = New UTF8Encoding(False)
        settings.ConformanceLevel = ConformanceLevel.Fragment

        'Si el directorio no existe, crearlo
        If Not (Directory.Exists(strRutaGuardarCertificado)) Then Directory.CreateDirectory(strRutaGuardarCertificado)

        'Using xWriter As XmlWriter = XmlWriter.Create(strRutaGuardarCertificado & "/" & stDocumento.Ruc & "-" & stDocumento.IdTipoDocumentoSunat & "-" & stDocumento.IdNumeroSerieDocumento & "-" & stDocumento.IdNumeroCorrelativo & ".xml", settings)
        Using xWriter As XmlWriter = XmlWriter.Create(strRutaGuardarCertificado & "/" & stDocumento.NombreArchivo & ".xml", settings)
            xmlDoc.WriteTo(xWriter)
        End Using

        ' Guardamos el xml en la ruta enviada
        'xmlDoc.Save(strRutaGuardarCertificado)
    End Function

    'Public Function GenerarZIPEnvio(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRutaXMLCertificado As String, ByVal intTipo As Integer, ByVal strAmbiente As String) As stCPE
    '    'Cargamos el código Hash
    '    Dim xmld As New XmlDocument
    '    xmld.Load(strRutaXMLCertificado & "\" & stDocumento.NombreArchivo & ".xml")

    '    stDocumento.CodigoHash = xmld.GetElementsByTagName("DigestValue").Item(0).InnerText
    '    stDocumento.CodigoSignature = xmld.GetElementsByTagName("SignatureValue").Item(0).InnerText

    '    Dim zip As New ZipFile
    '    Dim strArchivo As String = String.Format("{0}\{1}", Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip"), stDocumento.NombreArchivo & ".zip")

    '    'Si el directorio no existe, crearlo
    '    If Not (Directory.Exists(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))) Then Directory.CreateDirectory(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))

    '    If (File.Exists(strArchivo)) And intTipo <> 3 And intTipo <> 8 Then
    '        File.Delete(strArchivo)
    '    End If

    '    If Not (File.Exists(strArchivo)) And File.Exists(strRutaXMLCertificado & "\" & stDocumento.NombreArchivo & ".xml") Then
    '        zip.AddFile(strRutaXMLCertificado & "\" & stDocumento.NombreArchivo & ".xml", "\") 'Cargo el Fichero donde esta mi XML Firmado
    '        zip.Save(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip") & "\" & stDocumento.NombreArchivo & ".zip") 'Nombre del Fichero destino
    '    End If
    '    Dim xml_zipeado As Byte()
    '    If File.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip") & "\" & stDocumento.NombreArchivo & ".zip") Then
    '        xml_zipeado = File.ReadAllBytes(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip") & "\" & stDocumento.NombreArchivo & ".zip")
    '    End If

    '    Dim strRespuesta As String = ""

    '    System.Net.ServicePointManager.UseNagleAlgorithm = True
    '    System.Net.ServicePointManager.Expect100Continue = False
    '    System.Net.ServicePointManager.CheckCertificateRevocationList = True

    '    'Conexión a la SUNAT
    '    Dim FuncionesMet As New clsFuncionesMetodos
    '    Dim TablaSistMet As New clsTablaSistemaMetodos
    '    Dim strUsrName = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "02", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema)
    '    Dim strPassword = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "03", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema)
    '    Dim ClienteDEV = New ServicioSunat.billServiceClient("BillServiceSunatDEV")
    '    Dim Cliente = New ServicioSunat.billServiceClient("BillServiceSunatPRD")
    '    If intTipo <> "9" Then
    '        ClienteDEV = New ServicioSunat.billServiceClient("BillServiceSunatDEV") 'Desde el WebConfig VTAS
    '        Cliente = New ServicioSunat.billServiceClient("BillServiceSunatPRD") 'Desde el WebConfig VTAS
    '    Else
    '        ClienteDEV = New ServicioSunat.billServiceClient("BillServiceSunatGuiaDEV") 'Desde el WebConfig VTAS
    '        Cliente = New ServicioSunat.billServiceClient("BillServiceSunatGuiaPRD") 'Desde el WebConfig VTAS
    '    End If
    '    If strAmbiente = "1" Then
    '        Dim Behavior = New PasswordDigestBehavior(String.Concat(stDocumento.Ruc, strUsrName), strPassword)
    '        Cliente.Endpoint.EndpointBehaviors.Add(Behavior)
    '    Else
    '        Dim Behavior = New PasswordDigestBehavior(String.Concat(stDocumento.Ruc, "MODDATOS"), strPassword)
    '        ClienteDEV.Endpoint.EndpointBehaviors.Add(Behavior)
    '    End If
    '    Try
    '        If strAmbiente = "1" Then
    '            Cliente.Open()
    '        Else
    '            ClienteDEV.Open()
    '        End If
    '        If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And (intTipo = 1 Or intTipo = 9) Then
    '            If stDocumento.IdTipoDocumento <> "BV" And stDocumento.IdTipoDocumentoReferencial <> "BV" Then 'Cualquier Documento menos Boletas
    '                Dim xml_bytes As Byte()
    '                If strAmbiente = "1" Then
    '                    xml_bytes = Cliente.sendBill(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '                Else
    '                    xml_bytes = ClienteDEV.sendBill(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '                End If

    '                'Si el directorio no existe, crearlo
    '                If Not (Directory.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))) Then Directory.CreateDirectory(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))
    '                Dim fs As New FileStream(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip", FileMode.Create)

    '                fs.Write(xml_bytes, 0, xml_bytes.Length)
    '                fs.Close()
    '                stDocumento.NroTicket = ""
    '                stDocumento.Respuesta = strRespuesta
    '            End If
    '            If strAmbiente = "1" Then
    '                Cliente.Close()
    '            Else
    '                ClienteDEV.Close()
    '            End If
    '            'Aquí envio el resumen diario
    '        ElseIf Trim(stDocumento.IdNumeroCorrelativoFinal) <> "" And Trim(stDocumento.NroTicket) = "" And intTipo = 2 Then
    '            Dim xml_bytes_string As String
    '            If strAmbiente = "1" Then
    '                xml_bytes_string = Cliente.sendSummary(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '            Else
    '                xml_bytes_string = ClienteDEV.sendSummary(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '            End If

    '            'Si el directorio no existe, crearlo
    '            If Not (Directory.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))) Then Directory.CreateDirectory(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))

    '            Dim fs As New FileStream(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip", FileMode.Create)
    '            fs.Close()
    '            strRespuesta = xml_bytes_string
    '            If strAmbiente = "1" Then
    '                Cliente.Close()
    '            Else
    '                ClienteDEV.Close()
    '            End If
    '            stDocumento.NroTicket = strRespuesta
    '            stDocumento.Respuesta = IIf(IsNumeric(strRespuesta.Trim) = True, "REGISTRADO", "NO ENVIADO")
    '        ElseIf Trim(stDocumento.NroTicket) <> "" And intTipo = 3 Then 'Esto es para obtener el CDR del Resumen
    '            Dim xml_bytes_string As String
    '            Dim xml_bytes As Byte()
    '            If strAmbiente = "1" Then
    '                xml_bytes_string = Cliente.getStatus(stDocumento.NroTicket).statusCode
    '                xml_bytes = Cliente.getStatus(stDocumento.NroTicket).content
    '            Else
    '                xml_bytes_string = ClienteDEV.getStatus(stDocumento.NroTicket).statusCode
    '                xml_bytes = ClienteDEV.getStatus(stDocumento.NroTicket).content
    '            End If
    '            If (File.Exists(Mid(strRutaXMLCertificado, 1, InStrRev(strRutaXMLCertificado, "\")) & "DocXMLAnalizar\" & stDocumento.NombreArchivo & ".zip")) Then
    '                File.Delete(Mid(strRutaXMLCertificado, 1, InStrRev(strRutaXMLCertificado, "\")) & "DocXMLAnalizar\" & stDocumento.NombreArchivo & ".zip")
    '            End If
    '            If Not (File.Exists(Mid(strRutaXMLCertificado, 1, InStrRev(strRutaXMLCertificado, "\")) & "DocXMLAnalizar\" & stDocumento.NombreArchivo & ".zip")) Then
    '                'Si el directorio no existe, crearlo
    '                If Not (Directory.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))) Then Directory.CreateDirectory(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))
    '                Dim fs As New FileStream(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip", FileMode.Create)

    '                fs.Write(xml_bytes, 0, xml_bytes.Length)
    '                fs.Close()
    '            End If
    '            If strAmbiente = "1" Then
    '                Cliente.Close()
    '            Else
    '                ClienteDEV.Close()
    '            End If
    '            stDocumento.Respuesta = strRespuesta
    '        ElseIf Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And intTipo = 4 Then
    '            If stDocumento.IdTipoDocumento <> "BV" Then 'Cualquier Documento menos Boletas
    '                Dim xml_bytes As Byte()
    '                If strAmbiente = "1" Then
    '                    xml_bytes = Cliente.sendBill(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '                Else
    '                    xml_bytes = ClienteDEV.sendBill(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '                End If

    '                'Si el directorio no existe, crearlo
    '                If Not (Directory.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))) Then Directory.CreateDirectory(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))
    '                Dim fs As New FileStream(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip", FileMode.Create)

    '                fs.Write(xml_bytes, 0, xml_bytes.Length)
    '                fs.Close()

    '                stDocumento.NroTicket = ""
    '                stDocumento.Respuesta = strRespuesta
    '            End If
    '            If strAmbiente = "1" Then
    '                Cliente.Close()
    '            Else
    '                ClienteDEV.Close()
    '            End If
    '            'Envio la Baja de Facturas
    '        ElseIf Trim(stDocumento.IdNumeroCorrelativo) <> "" And Trim(stDocumento.NroTicket) = "" And intTipo = 7 Then
    '            Dim xml_bytes_string As String
    '            If strAmbiente = "1" Then
    '                xml_bytes_string = Cliente.sendSummary(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '            Else
    '                xml_bytes_string = ClienteDEV.sendSummary(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '            End If
    '            'Si el directorio no existe, crearlo
    '            If Not (Directory.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))) Then Directory.CreateDirectory(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))
    '            Dim fs As New FileStream(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip", FileMode.Create)

    '            fs.Close()
    '            strRespuesta = xml_bytes_string
    '            If strAmbiente = "1" Then
    '                Cliente.Close()
    '            Else
    '                ClienteDEV.Close()
    '            End If
    '            stDocumento.NroTicket = strRespuesta
    '            stDocumento.Respuesta = IIf(IsNumeric(strRespuesta.Trim) = True, "REGISTRADO", "NO ENVIADO")
    '        ElseIf Trim(stDocumento.NroTicket) <> "" And intTipo = 8 Then 'Esto es para obtener el CDR de la Baja
    '            Dim xml_bytes_string As String
    '            Dim xml_bytes As Byte()
    '            If strAmbiente = "1" Then
    '                xml_bytes_string = Cliente.getStatus(stDocumento.NroTicket).statusCode
    '                xml_bytes = Cliente.getStatus(stDocumento.NroTicket).content
    '            Else
    '                xml_bytes_string = ClienteDEV.getStatus(stDocumento.NroTicket).statusCode
    '                xml_bytes = ClienteDEV.getStatus(stDocumento.NroTicket).content
    '            End If
    '            If (File.Exists(Mid(strRutaXMLCertificado, 1, InStrRev(strRutaXMLCertificado, "\")) & "DocXMLAnalizar\" & stDocumento.NombreArchivo & ".zip")) Then
    '                File.Delete(Mid(strRutaXMLCertificado, 1, InStrRev(strRutaXMLCertificado, "\")) & "DocXMLAnalizar\" & stDocumento.NombreArchivo & ".zip")
    '            End If
    '            If Not (File.Exists(Mid(strRutaXMLCertificado, 1, InStrRev(strRutaXMLCertificado, "\")) & "DocXMLAnalizar\" & stDocumento.NombreArchivo & ".zip")) Then
    '                'Si el directorio no existe, crearlo
    '                If Not (Directory.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))) Then Directory.CreateDirectory(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar"))
    '                Dim fs As New FileStream(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip", FileMode.Create)

    '                fs.Write(xml_bytes, 0, xml_bytes.Length)
    '                fs.Close()
    '            End If
    '            If strAmbiente = "1" Then
    '                Cliente.Close()
    '            Else
    '                ClienteDEV.Close()
    '            End If
    '            stDocumento.Respuesta = strRespuesta
    '        End If
    '        Return stDocumento
    '    Catch ex As Exception
    '        strRespuesta = ex.Message
    '        If strAmbiente = "1" Then
    '            If Cliente.State.Opened Then
    '                Cliente.Close()
    '            End If
    '        Else
    '            If ClienteDEV.State.Opened Then
    '                ClienteDEV.Close()
    '            End If
    '        End If
    '        stDocumento.Respuesta = strRespuesta
    '        Return stDocumento
    '    End Try
    'End Function

    'Public Function GenerarZIPEnvioGuia(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRutaXMLCertificado As String, ByVal intTipo As Integer, ByVal strAmbiente As String) As stCPE
    '    'Cargamos el código Hash
    '    Dim xmld As New XmlDocument
    '    xmld.Load(strRutaXMLCertificado & "\" & stDocumento.NombreArchivo & ".xml")

    '    stDocumento.CodigoHash = xmld.GetElementsByTagName("DigestValue").Item(0).InnerText
    '    stDocumento.CodigoSignature = xmld.GetElementsByTagName("SignatureValue").Item(0).InnerText

    '    Dim zip As New ZipFile
    '    Dim strArchivo As String = String.Format("{0}\{1}", Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip"), stDocumento.NombreArchivo & ".zip")

    '    'Si el directorio no existe, crearlo
    '    If Not (Directory.Exists(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))) Then Directory.CreateDirectory(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))

    '    If (File.Exists(strArchivo)) And intTipo <> 3 And intTipo <> 8 Then
    '        File.Delete(strArchivo)
    '    End If

    '    If Not (File.Exists(strArchivo)) And File.Exists(strRutaXMLCertificado & "\" & stDocumento.NombreArchivo & ".xml") Then
    '        zip.AddFile(strRutaXMLCertificado & "\" & stDocumento.NombreArchivo & ".xml", "\") 'Cargo el Fichero donde esta mi XML Firmado
    '        zip.Save(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip") & "\" & stDocumento.NombreArchivo & ".zip") 'Nombre del Fichero destino
    '    End If
    '    Dim xml_zipeado As Byte()
    '    If File.Exists(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip") & "\" & stDocumento.NombreArchivo & ".zip") Then
    '        xml_zipeado = File.ReadAllBytes(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLZip") & "\" & stDocumento.NombreArchivo & ".zip")
    '    End If

    '    Try
    '        If intTipo <> 11 And strAmbiente = "1" Then
    '            Dim Token As String = ObtenerToken(stDocumento, strRutaXMLCertificado)

    '            Dim strArcGreZip = Convert.ToBase64String(xml_zipeado)

    '            Dim s As New SHA256Managed
    '            Dim fileBytes() As Byte = xml_zipeado 'File.ReadAllBytes(Replace("C:\SIW_NEW\slnSIW2022\slnSIW\VTAS\DocXMLFirmado\2022\12", "DocXMLFirmado", "DocXMLZip") & "\" & stDocumento.NombreArchivo & ".zip")
    '            Dim hash() As Byte = s.ComputeHash(fileBytes)
    '            Dim strHashZipSHA256 As String = ByteArrayToString(hash)

    '            'Dim client2 = New RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/gem/comprobantes/" &
    '            '                                   "20602706428-09-T001-1")
    '            Dim TablaSistMet As New clsTablaSistemaMetodos
    '            Dim sURL2 As String = Replace(TablaSistMet.TablaSistemaListarPorId("44", "10", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema, "{variable}", stDocumento.NombreArchivo)
    '            Dim client2 = New RestClient(sURL2)

    '            Dim request2 = New RestRequest(Method.POST)
    '            request2.AddHeader("cache-control", "no-cache")
    '            request2.AddHeader("content-type", "application/json")
    '            request2.AddHeader("Authorization", "Bearer " + Token)

    '            Dim body = "{" &
    '                       "    ""archivo"" : {" &
    '                       "        ""nomArchivo"" : """ & stDocumento.NombreArchivo & ".zip""," &
    '                       "        ""arcGreZip"" : """ & strArcGreZip & """," &
    '                       "        ""hashZip"" : """ & strHashZipSHA256 & """" &
    '                       "    }" &
    '                       "}"
    '            request2.AddParameter("application/json", body, ParameterType.RequestBody)
    '            Dim result2 As IRestResponse = client2.Execute(request2)
    '            Dim statusCode2 As HttpStatusCode = result2.StatusCode
    '            Dim numericStatusCode2 As Int16 = statusCode2
    '            Dim nroTicket As String = ""
    '            Dim fecRecepcion As String = ""
    '            If (numericStatusCode2 = 200) Then
    '                Dim objRpta = JsonConvert.DeserializeObject(result2.Content)
    '                nroTicket = objRpta("numTicket")
    '                fecRecepcion = objRpta("fecRecepcion")
    '                stDocumento.NroTicket = nroTicket
    '                stDocumento.Respuesta = ""
    '            Else
    '                Throw New Exception("Error de conexión con Servicio. " + result2.StatusDescription + "-" + result2.ErrorMessage)
    '            End If
    '        End If
    '        Return stDocumento
    '        'Final: JMUG: 29/12/2022 - Enviar el XML por API-Rest a SUNAT - Usando el Token
    '    Catch ex As Exception
    '        stDocumento.Respuesta = ex.Message
    '        Return stDocumento
    '    End Try
    'End Function

    'Public Function GetStatusCDR(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRuta As String, ByVal intTipo As Integer, ByVal strAmbiente As String) As stCPE
    '    Dim zip As New ZipFile
    '    Dim strArchivo As String = Replace(String.Format("{0}\{1}", Mid(strRuta, 1, InStrRev(strRuta, "\")), stDocumento.NombreArchivo & ".zip"), "\\", "\")
    '    If (File.Exists(strArchivo)) And intTipo <> 3 Then
    '        File.Delete(strArchivo)
    '    End If
    '    If Not (File.Exists(strArchivo)) Then
    '        'Si el directorio no existe, crearlo
    '        If Not (Directory.Exists(strRuta)) Then Directory.CreateDirectory(strRuta)
    '    End If

    '    Dim strRespuesta As String = ""

    '    System.Net.ServicePointManager.UseNagleAlgorithm = True
    '    System.Net.ServicePointManager.Expect100Continue = False
    '    System.Net.ServicePointManager.CheckCertificateRevocationList = True

    '    'Conexión a la SUNAT
    '    Dim FuncionesMet As New clsFuncionesMetodos
    '    Dim TablaSistMet As New clsTablaSistemaMetodos
    '    Dim strUsrName = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "02", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema)
    '    Dim strPassword = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "03", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema)

    '    Dim Cliente As New ServicioConsultaSunat.billServiceClient("BillConsultaServiceSunatPRD") 'Desde el WebConfig VTAS
    '    ''Agregamos el behavior configurado para soportar WS-Security.
    '    If strAmbiente = "1" Then
    '        Dim Behavior = New PasswordDigestBehavior(String.Concat(stDocumento.Ruc, strUsrName), strPassword)
    '        Cliente.Endpoint.EndpointBehaviors.Add(Behavior)
    '    Else
    '        Dim Behavior = New PasswordDigestBehavior(String.Concat(stDocumento.Ruc, strUsrName), strPassword)
    '        Cliente.Endpoint.EndpointBehaviors.Add(Behavior)
    '    End If
    '    Try
    '        If stDocumento.IdTipoDocumento = "GR" And strAmbiente = "1" Then
    '            Dim Token As String
    '            Token = ObtenerToken(stDocumento, strArchivo)
    '            Dim numTicket As String = stDocumento.NroTicket
    '            Dim sURL As String = Replace(TablaSistMet.TablaSistemaListarPorId("44", "11", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema, "{numTicket}", numTicket)
    '            Dim client As RestClient = New RestClient(sURL)
    '            client.Timeout = -1
    '            Dim request = New RestRequest(Method.GET)
    '            request.AddHeader("cache-control", "no-cache")
    '            request.AddHeader("content-type", "application/json")
    '            request.AddHeader("Authorization", "Bearer " + Token)
    '            Dim result As IRestResponse = client.Execute(request)
    '            Dim statusCode As HttpStatusCode = result.StatusCode
    '            Dim numericStatusCode As Int16 = statusCode
    '            If (numericStatusCode = 200) Then
    '                Dim objRpta = JsonConvert.DeserializeObject(result.Content)
    '                Dim arcCdr As String = objRpta("arcCdr")
    '                Dim xml_bytes As Byte() = Convert.FromBase64String(arcCdr)
    '                Dim fs As New FileStream(Mid(strRuta, 1, InStrRev(strRuta, "\")) & stDocumento.NombreArchivo & ".zip", FileMode.Create)
    '                fs.Write(xml_bytes, 0, xml_bytes.Length)
    '                fs.Close()
    '            Else
    '                stDocumento.CodigoRespuesta = numericStatusCode
    '                Dim objRpta = JsonConvert.DeserializeObject(result.Content)
    '                Dim strMsg As String = objRpta("msg")
    '                stDocumento.Respuesta = strMsg
    '            End If
    '        Else
    '            If strAmbiente = "1" Then
    '                Cliente.Open()
    '            Else
    '                Cliente.Open() ' ClienteDEV.Open()
    '            End If

    '            'Aquí se recupera el CDR generado por la SUNAT
    '            If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And intTipo = 1 Then
    '                If stDocumento.IdTipoDocumento <> "BV" Then 'Cualquier Documento menos Boletas
    '                    Dim xml_bytes As Byte()
    '                    Dim Respuesta
    '                    If strAmbiente = "1" Then
    '                        Respuesta = Cliente.getStatusCdr(stDocumento.Ruc, stDocumento.IdTipoDocumentoSunat, stDocumento.IdNumeroSerieDocumento, stDocumento.IdNumeroCorrelativo) 'Cliente.sendBill(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '                    Else
    '                        Respuesta = Cliente.getStatusCdr(stDocumento.Ruc, stDocumento.IdTipoDocumentoSunat, stDocumento.IdNumeroSerieDocumento, stDocumento.IdNumeroCorrelativo) 'Cliente.sendBill(stDocumento.NombreArchivo & ".zip", xml_zipeado, vbNull)
    '                    End If
    '                    xml_bytes = Respuesta.content

    '                    Dim fs As New FileStream(Mid(strRuta, 1, InStrRev(strRuta, "\")) & stDocumento.NombreArchivo & ".zip", FileMode.Create)
    '                    fs.Write(xml_bytes, 0, xml_bytes.Length)
    '                    fs.Close()
    '                    stDocumento.NroTicket = ""
    '                    stDocumento.Respuesta = strRespuesta
    '                End If
    '                If strAmbiente = "1" Then
    '                    Cliente.Close()
    '                Else
    '                    Cliente.Close() 'ClienteDEV.Close()
    '                End If
    '            End If
    '        End If
    '        Return stDocumento
    '    Catch ex As Exception
    '        strRespuesta = ex.Message
    '        If strAmbiente = "1" Then
    '            If Cliente.State.Opened Then
    '                Cliente.Close()
    '            End If
    '        Else
    '            If Cliente.State.Opened Then
    '                Cliente.Close()
    '            End If
    '        End If
    '        stDocumento.Respuesta = strRespuesta
    '        Return stDocumento
    '    End Try
    'End Function

    Public Function CapturarHash(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRutaXMLCertificado As String) As stCPE
        'Cargamos el código Hash
        Dim xmld As New XmlDocument
        xmld.Load(strRutaXMLCertificado & "\" & stDocumento.NombreArchivo & ".xml")

        stDocumento.CodigoHash = xmld.GetElementsByTagName("DigestValue").Item(0).InnerText
        stDocumento.CodigoSignature = xmld.GetElementsByTagName("SignatureValue").Item(0).InnerText
        Return stDocumento
    End Function

    Public Function GenerarAnalizarEnvio(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRutaXMLCertificado As String, ByVal intTipo As Integer) As stCPE
        Dim strRespuesta As String = ""
        Try
            If intTipo = "2" Or (intTipo = "1" And stDocumento.IdTipoDocumento = "BV") Or (intTipo = "1" And stDocumento.IdTipoDocumentoReferencial = "BV") Then Return stDocumento
            'Descompilar CDR Sunat
            Dim zip As New ZipFile
            zip = ZipFile.Read(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip")

            Dim strArchivo As String = ""
            If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And (intTipo = "1" Or intTipo = "4") Then
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "3" Then
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "6" Then 'Obtener CDR Factura
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "8" Then 'Obtener CDR de Baja Factura
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "9" Then 'Obtener CDR de Guía Remisión
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            End If

            'Si el directorio no existe, crearlo
            If Not (Directory.Exists(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))) Then Directory.CreateDirectory(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))

            If (File.Exists(strArchivo)) Then
                File.Delete(strArchivo)
            End If

            zip.ExtractAll(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar\"))

            'Buscar en el XML la respuesta del CDR
            Dim xmld As New XmlDocument
            If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And (intTipo = "1" Or intTipo = "4") Then
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "3" Then
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "6" Then 'Obtener CDR Factura
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "8" Then 'Generar Baja Factura
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            ElseIf intTipo = "9" Then 'Generar Guia de Remisión
                strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
            End If
            xmld.Load(strArchivo)
            Dim mensaje As String = xmld.GetElementsByTagName("cbc:Description").Item(0).InnerText
            Dim codigo As String = xmld.GetElementsByTagName("cbc:ResponseCode").Item(0).InnerText
            Dim ticket As String = xmld.GetElementsByTagName("cbc:ID").Item(0).InnerText
            strRespuesta = mensaje
            stDocumento.NroTicket = ticket
            stDocumento.Respuesta = strRespuesta
            stDocumento.CodigoRespuesta = codigo
            Return stDocumento
        Catch ex As Exception
            If ex.HResult = "-2147467261" Then 'Referencia a objeto no establecida como instancia de un objeto.
                'Buscar en el XML la respuesta del CDR
                Dim xmld As New XmlDocument
                If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And (intTipo = "1" Or intTipo = "4") Then
                    xmld.Load(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml")
                ElseIf intTipo = "3" Then
                    xmld.Load(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml")
                End If
                Dim mensaje As String = xmld.GetElementsByTagName("error_protocol_reason_phrase").Item(0).InnerText
                strRespuesta = mensaje
                stDocumento.CodigoRespuesta = "OBS" 'NO TIENE CODIGO LE COLOQUE ELLO
            Else
                strRespuesta = ex.Message
            End If
            stDocumento.Respuesta = strRespuesta
            Return stDocumento
        End Try
    End Function

    Public Function GenerarAnalizarEnvioAPI(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRutaXMLCertificado As String, ByVal intTipo As Integer, ByVal strAmbiente As String) As stCPE
        Dim strRespuesta As String = ""
        Dim mensaje As String = ""
        Dim codigo As String = ""
        Dim ticket As String = ""
        Dim urlHash As String = ""
        Try
            If strAmbiente = "1" Then
                If intTipo = "2" Or (intTipo = "1" And stDocumento.IdTipoDocumento = "BV") Or (intTipo = "1" And stDocumento.IdTipoDocumentoReferencial = "BV") Then Return stDocumento

                'Descompilar CDR Sunat
                Dim zip As New ZipFile
                zip = ZipFile.Read(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\" & stDocumento.NombreArchivo & ".zip")

                Dim strArchivo As String = ""
                If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And (intTipo = "13") Then
                    strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
                End If

                'Si el directorio no existe, crearlo
                If Not (Directory.Exists(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))) Then Directory.CreateDirectory(Mid(strArchivo, 1, InStrRev(strArchivo, "\")))

                If (File.Exists(strArchivo)) Then
                    File.Delete(strArchivo)
                End If

                zip.ExtractAll(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar\"))

                'Buscar en el XML la respuesta del CDR
                Dim xmld As New XmlDocument
                If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And (intTipo = "13") Then
                    strArchivo = Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml"
                End If
                xmld.Load(strArchivo)
                mensaje = xmld.GetElementsByTagName("cbc:Description").Item(0).InnerText
                codigo = xmld.GetElementsByTagName("cbc:ResponseCode").Item(0).InnerText
                ticket = xmld.GetElementsByTagName("cbc:ID").Item(0).InnerText
                If codigo = "0" Then
                    urlHash = xmld.GetElementsByTagName("cbc:DocumentDescription").Item(0).InnerText
                End If
            Else 'Ambiente de prueba
                mensaje = "No se envía a Sunat.  Ambiente de Prueba"
                codigo = ""
                ticket = ""
                urlHash = ""
            End If

            strRespuesta = mensaje
            stDocumento.NroTicket = ticket
            stDocumento.Respuesta = strRespuesta
            stDocumento.CodigoRespuesta = codigo
            stDocumento.urlHashCDR = urlHash 'JMUG: 14/01/2023
            Return stDocumento
        Catch ex As Exception
            If ex.HResult = "-2147467261" Then 'Referencia a objeto no establecida como instancia de un objeto.
                'Buscar en el XML la respuesta del CDR
                Dim xmld As New XmlDocument
                If Trim(stDocumento.IdNumeroCorrelativoFinal) = "" And (intTipo = "1" Or intTipo = "4") Then
                    xmld.Load(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml")
                ElseIf intTipo = "3" Then
                    xmld.Load(Replace(strRutaXMLCertificado, "DocXMLFirmado", "DocXMLAnalizar") & "\R-" & stDocumento.NombreArchivo & ".xml")
                End If
                mensaje = xmld.GetElementsByTagName("error_protocol_reason_phrase").Item(0).InnerText
                strRespuesta = mensaje
                stDocumento.CodigoRespuesta = "OBS" 'NO TIENE CODIGO LE COLOQUE ELLO
            Else
                strRespuesta = ex.Message
            End If
            stDocumento.Respuesta = strRespuesta
            Return stDocumento
        End Try
    End Function

    Public Function GenerarCPEXML(stDocumento As stCPE, ByVal Ruta As String) As stCPE  'As Boolean
        'CREADO PARA LA GENERACIÓN DEL ARCHIVO XML DE FACTURACIÓN ELECTRÓNICA DE SUNAT
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_VTAS_RPT_DOCUMENTOVENTAFE"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdTipoDocumento", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroSerieDocumento", SqlDbType.VarChar, 6))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroCorrelativo", SqlDbType.VarChar, 20))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cIdTipoDocumento").Value = stDocumento.IdTipoDocumento 'IdTipoDocumento
        cmd.Parameters("@vIdNumeroSerieDocumento").Value = stDocumento.IdNumeroSerieDocumento 'IdNumeroSerieDocumento
        cmd.Parameters("@vIdNumeroCorrelativo").Value = stDocumento.IdNumeroCorrelativo 'IdNumeroCorrelativo
        cmd.Parameters("@cIdPuntoVenta").Value = stDocumento.IdPuntoVenta
        cmd.Parameters("@cIdEmpresa").Value = stDocumento.IdEmpresa

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "DocumentoVenta")

        Try
            cnx.Open()
            Dim strXML As String = ""
            Dim doc As New XmlDocument()

            Dim Fila = ds.Tables("DocumentoVenta").Rows(0)
            'strNombreArchivo = Fila("vRucEmpresa") & "-" & Fila("vIdEquivalenciaContableTipoDocumento") & "-" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento")
            stDocumento.Ruc = Fila("vRucEmpresa")
            stDocumento.IdTipoDocumentoSunat = Fila("vIdEquivalenciaContableTipoDocumento")
            If stDocumento.IdTipoDocumento = "NC" Or stDocumento.IdTipoDocumento = "ND" Then
                stDocumento.IdTipoDocumentoReferencial = Fila("cIdTipoDocumentoReferencialCabeceraDocumento")
                stDocumento.IdTipoDocumentoSunatReferencial = Fila("vIdEquivalenciaContableTipoDocumentoReferencial")
                stDocumento.IdNumeroSerieDocumentoReferencial = Fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento")
                stDocumento.IdNumeroCorrelativoReferencial = Fila("vIdNumeroCorrelativoReferencialCabeceraDocumento")
            End If
            'stDocumento.NombreArchivo = Fila("vRucEmpresa") & "-" & Fila("vIdEquivalenciaContableTipoDocumento") & "-" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento")

            dt = ds.Tables("DocumentoVenta")
            If stDocumento.IdTipoDocumento = "BV" Or stDocumento.IdTipoDocumento = "FA" Then
                'Cabecera del Documento XML
                strXML = strXML + "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf &
                                  "<Invoice xmlns=""urn:oasis:names:specification:ubl:schema:xsd:Invoice-2""" & vbCrLf &
                                  "    xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""" & vbCrLf &
                                  "    xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""" & vbCrLf &
                                  "    xmlns:ccts=""urn:un:unece:uncefact:documentation:2""" & vbCrLf &
                                  "    xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""" & vbCrLf &
                                  "    xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""" & vbCrLf &
                                  "    xmlns:qdt=""urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2""" & vbCrLf &
                                  "    xmlns:udt=""urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2""" & vbCrLf &
                                  "    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" & vbCrLf

                'Para registrar la firma
                strXML = strXML + "    <ext:UBLExtensions>" & vbCrLf &
                                  "        <ext:UBLExtension>" & vbCrLf &
                                  "            <ext:ExtensionContent>" & vbCrLf &
                                  "            </ext:ExtensionContent>" & vbCrLf &
                                  "        </ext:UBLExtension>" & vbCrLf &
                                  "    </ext:UBLExtensions>" & vbCrLf

                'Datos Cabecera Documento - Versión UBL - Fecha y Hora de Emisión - Importe total en letras
                strXML = strXML + "    <cbc:UBLVersionID>2.1</cbc:UBLVersionID>" & vbCrLf &
                                  "    <cbc:CustomizationID>2.0</cbc:CustomizationID>" & vbCrLf &
                                  "    <cbc:ProfileID schemeName=""SUNAT:Identificador de Tipo de Operación"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo17"">" & Fila("cIdTipoOperacionVentasCabeceraDocumento") & "</cbc:ProfileID>" & vbCrLf &
                                  "    <cbc:ID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "    <cbc:IssueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaEmisionCabeceraDocumento"))) & "</cbc:IssueDate>" & vbCrLf &
                                  "    <cbc:IssueTime>" & Fila("dHoraEmisionCabeceraDocumento").ToString.Trim & "</cbc:IssueTime>" & vbCrLf &
                                  "    <cbc:DueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaVencimientoCabeceraDocumento"))) & "</cbc:DueDate>" & vbCrLf &
                                  "    <cbc:InvoiceTypeCode listID=""0101"" listAgencyName=""PE:SUNAT"" listName=""Tipo de Documento"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01"">" & Fila("vIdEquivalenciaContableTipoDocumento") & "</cbc:InvoiceTypeCode>" & vbCrLf &
                                  "    <cbc:Note languageLocaleID=""1000"">" & "SON " & Fila("nImporteTotalMontoLetras") & "</cbc:Note>" & vbCrLf &
                                  "    <cbc:DocumentCurrencyCode listID=""ISO 4217 Alpha"" listName=""Currency"" listAgencyName=""United Nations Economic Commission for Europe"">" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "</cbc:DocumentCurrencyCode>" & vbCrLf

                'Datos del Emisor
                strXML = strXML + "    <cac:Signature>" & vbCrLf &
                                  "        <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "        <cac:SignatoryParty>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "        </cac:SignatoryParty>" & vbCrLf &
                                  "        <cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "            <cac:ExternalReference>" & vbCrLf &
                                  "                <cbc:URI>" & Fila("vRucEmpresa") & "</cbc:URI>" & vbCrLf &
                                  "            </cac:ExternalReference>" & vbCrLf &
                                  "        </cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "    </cac:Signature>" & vbCrLf

                'Tipo de Documento y Razón Social del Emisor (Empresa)
                strXML = strXML + "    <cac:AccountingSupplierParty>" & vbCrLf &
                                  "        <cac:Party>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoEmpresa") & """ schemeName=""Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name><![CDATA[" & Fila("vNombreComercialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "            <cac:PartyLegalEntity>" & vbCrLf &
                                  "                <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]></cbc:RegistrationName>" & vbCrLf &
                                  "                <cac:RegistrationAddress>" & vbCrLf &
                                  "                    <cbc:AddressTypeCode>" & Fila("cIdAnexoEstablecimientoEmpresa") & "</cbc:AddressTypeCode>" & vbCrLf &
                                  "                </cac:RegistrationAddress>" & vbCrLf &
                                  "            </cac:PartyLegalEntity>" & vbCrLf &
                                  "        </cac:Party>" & vbCrLf &
                                  "    </cac:AccountingSupplierParty>" & vbCrLf
                'De donde saco el codigo de anexo establecimiento??????

                'Tipo de Documento y Razon Social del Cliente
                strXML = strXML + "    <cac:AccountingCustomerParty>" & vbCrLf &
                                  "        <cac:Party>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoCliente") & """ schemeName=""Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vNumeroDocumentoClienteCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyLegalEntity>" & vbCrLf &
                                  "                <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialCabeceraDocumento") & "]]></cbc:RegistrationName>" & vbCrLf &
                                  "            </cac:PartyLegalEntity>" & vbCrLf &
                                  "        </cac:Party>" & vbCrLf &
                                  "    </cac:AccountingCustomerParty>" & vbCrLf

                'Inicio: JMUG-02/04/2021 - Forma de Pago
                'If Fila("cIdTipoTransaccion") = "001" Then 'Contado
                '    strXML = strXML + "    <cac:PaymentTerms>" & vbCrLf &
                '                  "        <cbc:ID>FormaPago</cbc:ID>" & vbCrLf &
                '                  "        <cbc:PaymentMeansID>Contado</cbc:PaymentMeansID>" & vbCrLf &
                '                  "    </cac:PaymentTerms>" & vbCrLf
                'End If


                'Dirección de Entrega al Cliente
                strXML = strXML + "    <cac:DeliveryTerms>" & vbCrLf &
                                  "        <cac:DeliveryLocation>" & vbCrLf &
                                  "            <cac:Address>" & vbCrLf &
                                  "                <cbc:StreetName>" & Fila("vDireccionClienteCabeceraDocumento") & "</cbc:StreetName>" & vbCrLf &
                                  "                <cbc:CitySubdivisionName/>" & vbCrLf &
                                  "                <cbc:CityName>" & Fila("vDescripcionDepartamentoClienteCabeceraDocumento") & "</cbc:CityName>" & vbCrLf &
                                  "                <cbc:CountrySubentity>" & Fila("vDescripcionProvinciaClienteCabeceraDocumento") & "</cbc:CountrySubentity>" & vbCrLf &
                                  "                <cbc:CountrySubentityCode>" & Fila("cIdUbiGeoClienteCabeceraDocumento") & "</cbc:CountrySubentityCode>" & vbCrLf &
                                  "                <cbc:District>" & Fila("vDescripcionDistritoClienteCabeceraDocumento") & "</cbc:District>" & vbCrLf &
                                  "                <cac:Country>" & vbCrLf &
                                  "                    <cbc:IdentificationCode listID=""ISO 3166-1"" listAgencyName=""United Nations Economic Commission for Europe"" listName=""Country"">" & Fila("cIdPaisClienteCabeceraDocumento") & "</cbc:IdentificationCode>" & vbCrLf &
                                  "                </cac:Country>" & vbCrLf &
                                  "            </cac:Address>" & vbCrLf &
                                  "        </cac:DeliveryLocation>" & vbCrLf &
                                  "    </cac:DeliveryTerms>" & vbCrLf

                'Detracción: Inicio
                If Fila("vIdEquivalenciaSunatDetraccion") <> "" Then
                    strXML = strXML + "    <cac:PaymentMeans>" & vbCrLf &
                                  "        <cbc:ID>Detraccion</cbc:ID>" & vbCrLf &
                                  "        <cbc:PaymentMeansCode>001</cbc:PaymentMeansCode>" & vbCrLf &
                                  "        <cac:PayeeFinancialAccount>" & vbCrLf &
                                  "            <cbc:ID>" & Fila("vCuentaBancoNacionDetraccion") & "</cbc:ID>" & vbCrLf &
                                  "        </cac:PayeeFinancialAccount>" & vbCrLf &
                                  "    </cac:PaymentMeans>" & vbCrLf &
                                  "    <cac:PaymentTerms>" & vbCrLf &
                                  "        <cbc:ID>" & Fila("vIdEquivalenciaSunatDetraccion") & "</cbc:ID>" & vbCrLf &
                                  "        <cbc:PaymentMeansID>" & Fila("vIdEquivalenciaSunatDetraccion") & "</cbc:PaymentMeansID>" & vbCrLf &
                                  "        <cbc:PaymentPercent>" & Fila("nPorcentajeDetraccion") & "</cbc:PaymentPercent>" & vbCrLf &
                                  "        <cbc:Amount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMonedaDetraccion") & """>" & Fila("nTotalDetraccion") & "</cbc:Amount>" & vbCrLf &
                                  "    </cac:PaymentTerms>" & vbCrLf
                    'Detracción: Final
                End If

                If Fila("cIdTipoTransaccion") = "001" Then 'Contado
                    strXML = strXML + "    <cac:PaymentTerms>" & vbCrLf &
                                  "       <cbc:ID>FormaPago</cbc:ID>" & vbCrLf &
                                  "       <cbc:PaymentMeansID>Contado</cbc:PaymentMeansID>" & vbCrLf &
                                  "    </cac:PaymentTerms>" & vbCrLf
                End If
                If Fila("cIdTipoTransaccion") = "002" Then 'Crédito
                    strXML = strXML + "    <cac:PaymentTerms>" & vbCrLf &
                                  "        <cbc:ID>FormaPago</cbc:ID>" & vbCrLf &
                                  "        <cbc:PaymentMeansID>Credito</cbc:PaymentMeansID>" & vbCrLf &
                                  "        <cbc:Amount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento") - Fila("nTotalDetraccion")), 2) & "</cbc:Amount>" & vbCrLf &
                                  "    </cac:PaymentTerms>" & vbCrLf

                    'Imports System.Data.SqlClient 'Se puso eso para que funcione los commans
                    Dim dtCuotas As New DataTable()
                    Dim constrCuotas As String = My.Settings.CMMSConnectionString
                    Using conCuotas As New SqlConnection(constrCuotas)
                        Using cmdCuotas As New SqlCommand("SELECT CROPAG.*, TIPMON.vIdEquivalenciaContableAbreviadaTipoMoneda FROM VTAS_CRONOGRAMAPAGO CROPAG INNER JOIN GNRL_TIPOMONEDA TIPMON ON " &
                                                "TIPMON.cIdTipoMoneda = CROPAG.cIdTipoMoneda " &
                                                "WHERE CROPAG.cIdTipoDocumento = '" & stDocumento.IdTipoDocumento & "'" &
                                                "      AND CROPAG.vIdNumeroSerieCabeceraDocumento = '" & stDocumento.IdNumeroSerieDocumento & "'" &
                                                "      AND CROPAG.vIdNumeroCorrelativoCabeceraDocumento = '" & stDocumento.IdNumeroCorrelativo & "'" &
                                                "      AND CROPAG.cIdEmpresa = '" & stDocumento.IdEmpresa & "'")
                            Using sdaCuotas As New SqlDataAdapter()
                                cmdCuotas.CommandType = CommandType.Text
                                cmdCuotas.Connection = conCuotas
                                sdaCuotas.SelectCommand = cmdCuotas
                                sdaCuotas.Fill(dtCuotas)
                            End Using
                        End Using
                        'Return dt
                    End Using
                    'da.Fill(ds, "Cuotas")
                    If dtCuotas.Rows.Count > 0 Then
                        Dim filaCuotas As DataRow
                        For Each filaCuotas In dtCuotas.Rows
                            strXML = strXML + "    <cac:PaymentTerms>" & vbCrLf &
                                      "        <cbc:ID>FormaPago</cbc:ID>" & vbCrLf &
                                      "        <cbc:PaymentMeansID>Cuota" & String.Format("{0:000}", filaCuotas("nIdNumeroCuotaCronogramaPago")) & "</cbc:PaymentMeansID>" & vbCrLf &
                                      "        <cbc:Amount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(filaCuotas("nMontoCuotaCronogramaPago") - Fila("nTotalDetraccion")), 2) & "</cbc:Amount>" & vbCrLf &
                                      "        <cbc:PaymentDueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(filaCuotas("dFechaVencimientoCuotaCronogramaPago"))) & "</cbc:PaymentDueDate>" & vbCrLf &
                                      "    </cac:PaymentTerms>" & vbCrLf
                        Next
                    End If
                End If
                'Final: JMUG-02/04/2021 - Forma de Pago

                strXML = strXML + "    <cac:TaxTotal>" & vbCrLf

                'IGV Total + ISC Total del Documento 15/10/2019
                strXML = strXML + "        <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")) + Convert.ToDecimal(Fila("nTotalISCCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf

                'Cálculo del IGV
                If Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) > 0 Then
                    strXML = strXML + "        <cac:TaxSubtotal>" & vbCrLf &
                                  "            <cbc:TaxableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoGravadoObligatorio")), 2) + Math.Round(Convert.ToDecimal(Fila("nTotalISCCabeceraDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                  "            <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "            <cac:TaxCategory>" & vbCrLf &
                                  "                <cbc:ID schemeID=""UN/ECE 5305"" schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">S</cbc:ID>" & vbCrLf &
                                  "                <cac:TaxScheme>" & vbCrLf &
                                  "                    <cbc:ID schemeID=""UN/ECE 5153"" schemeAgencyID=""6"">1000</cbc:ID>" & vbCrLf &
                                  "                    <cbc:Name>IGV</cbc:Name>" & vbCrLf &
                                  "                    <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                  "                </cac:TaxScheme>" & vbCrLf &
                                  "            </cac:TaxCategory>" & vbCrLf &
                                  "        </cac:TaxSubtotal>" & vbCrLf
                End If

                'Cálculo del ISC
                If Math.Round(Convert.ToDecimal(Fila("nTotalISCCabeceraDocumento")), 2) > 0 Then
                    strXML = strXML + "        <cac:TaxSubtotal>" & vbCrLf &
                                  "            <cbc:TaxableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                  "            <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalISCCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "            <cac:TaxCategory>" & vbCrLf &
                                  "                <cbc:ID schemeID=""UN/ECE 5305"" schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">S</cbc:ID>" & vbCrLf &
                                  "                <cac:TaxScheme>" & vbCrLf &
                                  "                    <cbc:ID schemeID=""UN/ECE 5153"" schemeAgencyID=""6"">2000</cbc:ID>" & vbCrLf &
                                  "                    <cbc:Name>ISC</cbc:Name>" & vbCrLf &
                                  "                    <cbc:TaxTypeCode>EXC</cbc:TaxTypeCode>" & vbCrLf &
                                  "                </cac:TaxScheme>" & vbCrLf &
                                  "            </cac:TaxCategory>" & vbCrLf &
                                  "        </cac:TaxSubtotal>" & vbCrLf
                End If

                'Exonerado
                'If Fila("cIdTipoAfectacion") = "20" Or Fila("cIdTipoAfectacion") = "21" Then
                If Math.Round(Convert.ToDecimal(Fila("nTotalMontoExoneradoObligatorio")), 2) > 0 Then
                    'strXML = strXML + "        <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>0.00</cbc:TaxAmount>" & vbCrLf &
                    strXML = strXML + "        <cac:TaxSubtotal>" & vbCrLf &
                                "            <cbc:TaxableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoExoneradoObligatorio")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                "            <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>0.00</cbc:TaxAmount>" & vbCrLf &
                                "            <cac:TaxCategory>" & vbCrLf &
                                "                <cbc:ID schemeID=""UN/ECE 5305""  schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission For Europe"">E</cbc:ID>" & vbCrLf &
                                "                <cac:TaxScheme>" & vbCrLf &
                                "                    <cbc:ID schemeID=""UN/ECE 5153"" schemeAgencyID=""6"">9997</cbc:ID>" & vbCrLf &
                                "                    <cbc:Name>EXO</cbc:Name>" & vbCrLf &
                                "                    <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                "                </cac:TaxScheme>" & vbCrLf &
                                "            </cac:TaxCategory>" & vbCrLf &
                                "        </cac:TaxSubtotal>" & vbCrLf
                End If

                'Inafecto
                'If Fila("cIdTipoAfectacion") = "30" Or Fila("cIdTipoAfectacion") = "31" Or Fila("cIdTipoAfectacion") = "32" Or Fila("cIdTipoAfectacion") = "33" Or Fila("cIdTipoAfectacion") = "34" Or Fila("cIdTipoAfectacion") = "35" Or Fila("cIdTipoAfectacion") = "36" Then
                If Math.Round(Convert.ToDecimal(Fila("nTotalMontoInafectoObligatorio")), 2) > 0 Then
                    strXML = strXML + "        <cac:TaxSubtotal>" & vbCrLf &
                                "            <cbc:TaxableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoInafectoObligatorio")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                "            <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>0.00</cbc:TaxAmount>" & vbCrLf &
                                "            <cac:TaxCategory>" & vbCrLf &
                                "                <cbc:ID schemeID=""UN/ECE 5305""  schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission For Europe"">O</cbc:ID>" & vbCrLf &
                                "                <cac:TaxScheme>" & vbCrLf &
                                "                    <cbc:ID schemeID=""UN/ECE 5153"" schemeAgencyID=""6"">9998</cbc:ID>" & vbCrLf &
                                "                    <cbc:Name>INA</cbc:Name>" & vbCrLf &
                                "                    <cbc:TaxTypeCode>FRE</cbc:TaxTypeCode>" & vbCrLf &
                                "                </cac:TaxScheme>" & vbCrLf &
                                "            </cac:TaxCategory>" & vbCrLf &
                                "        </cac:TaxSubtotal>" & vbCrLf
                End If

                strXML = strXML + "    </cac:TaxTotal>" & vbCrLf

                ''Importe total de la venta, cesión en uso o del servicio prestado
                'strXML = strXML + "    <cac:LegalMonetaryTotal>" & vbCrLf &
                '                "        <cbc:AllowanceTotalAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalDescuentoCabeceraDocumento")), 2) & "</cbc:AllowanceTotalAmount>" & vbCrLf &
                '                "        <cbc:PayableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) & "</cbc:PayableAmount>" & vbCrLf &
                '                "    </cac:LegalMonetaryTotal>" & vbCrLf

                'Importe total de la venta, cesión en uso o del servicio prestado
                strXML = strXML + "    <cac:LegalMonetaryTotal>" & vbCrLf

                'Total Valor Venta Neto - Aplicado el Dscto.
                If Math.Round(Convert.ToDecimal(Fila("nTotalMontoGravadoObligatorio")), 2) > 0 Then
                    strXML = strXML + "        <cbc:LineExtensionAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoGravadoObligatorio")), 2) & "</cbc:LineExtensionAmount>" & vbCrLf
                End If

                'Total Precio de Venta
                If Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) > 0 Then
                    strXML = strXML + "        <cbc:TaxInclusiveAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) & "</cbc:TaxInclusiveAmount>" & vbCrLf
                End If

                'Total Descuento
                If Math.Round(Convert.ToDecimal(Fila("nTotalDescuentoCabeceraDocumento")), 2) > 0 Then
                    'JMUG: 22/10/2022 ' El Total Descuento es cero en este caso, hay que indagar porque sale error si se le coloca descuento.
                    'strXML = strXML + "        <cbc:AllowanceTotalAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalDescuentoCabeceraDocumento")), 2) & "</cbc:AllowanceTotalAmount>" & vbCrLf
                    strXML = strXML + "        <cbc:AllowanceTotalAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal("0.00"), 2) & "</cbc:AllowanceTotalAmount>" & vbCrLf
                End If

                'Total Otros Cargos
                If Math.Round(Convert.ToDecimal(Fila("nTotalOtrosImpuestoCabeceraDocumento")), 2) > 0 Then
                    strXML = strXML + "        <cbc:ChargeTotalAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalOtrosImpuestoCabeceraDocumento")), 2) & "</cbc:ChargeTotalAmount>" & vbCrLf
                End If

                'Total General Valor Venta + IGV + ISC - DSCTO + PERCEPCION
                If Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) > 0 Then
                    strXML = strXML + "        <cbc:PayableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) & "</cbc:PayableAmount>" & vbCrLf
                End If

                strXML = strXML + "    </cac:LegalMonetaryTotal>" & vbCrLf
            End If

            If stDocumento.IdTipoDocumento = "NC" Then
                'Cabecera del Documento XML
                strXML = strXML + "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf &
                                  "<CreditNote xmlns=""urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2""" & vbCrLf &
                                  "    xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""" & vbCrLf &
                                  "    xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""" & vbCrLf &
                                  "    xmlns:ccts=""urn:un:unece:uncefact:documentation:2""" & vbCrLf &
                                  "    xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""" & vbCrLf &
                                  "    xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""" & vbCrLf &
                                  "    xmlns:qdt=""urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2""" & vbCrLf &
                                  "    xmlns:sac=""urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1""" & vbCrLf &
                                  "    xmlns:udt=""urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2""" & vbCrLf &
                                  "    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" & vbCrLf

                'Para registrar la firma
                strXML = strXML + "    <ext:UBLExtensions>" & vbCrLf &
                                  "        <ext:UBLExtension>" & vbCrLf &
                                  "            <ext:ExtensionContent>" & vbCrLf &
                                  "            </ext:ExtensionContent>" & vbCrLf &
                                  "        </ext:UBLExtension>" & vbCrLf &
                                  "    </ext:UBLExtensions>" & vbCrLf

                'Datos Cabecera Documento - Versión UBL - Fecha y Hora de Emisión - Importe total en letras
                strXML = strXML + "    <cbc:UBLVersionID>2.1</cbc:UBLVersionID>" & vbCrLf &
                                  "    <cbc:CustomizationID>2.0</cbc:CustomizationID>" & vbCrLf &
                                  "    <cbc:ID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "    <cbc:IssueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaEmisionCabeceraDocumento"))) & "</cbc:IssueDate>" & vbCrLf &
                                  "    <cbc:IssueTime>" & Fila("dHoraEmisionCabeceraDocumento").ToString.Trim & "</cbc:IssueTime>" & vbCrLf

                'strXML = strXML + "    <cbc:DocumentCurrencyCode>" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "</cbc:DocumentCurrencyCode>" & vbCrLf &
                '                  "  <cac:DiscrepancyResponse>" & vbCrLf &
                '                  "    <cbc:ReferenceID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</cbc:ReferenceID>" & vbCrLf &
                '                  "    <cbc:ResponseCode>" & Fila("vIdEquivalenciaSunatFormaPago") & "</cbc:ResponseCode>" & vbCrLf &
                '                  "    <cbc:Description>" & Fila("vDescripcionFormaPago") & "</cbc:Description>" & vbCrLf &
                strXML = strXML + "    <cbc:DocumentCurrencyCode>" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "</cbc:DocumentCurrencyCode>" & vbCrLf &
                                  "  <cac:DiscrepancyResponse>" & vbCrLf &
                                  "    <cbc:ReferenceID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</cbc:ReferenceID>" & vbCrLf &
                                  "    <cbc:ResponseCode>" & Fila("vIdEquivalenciaSunatTipoTransaccion") & "</cbc:ResponseCode>" & vbCrLf &
                                  "    <cbc:Description>" & Fila("vDescripcionTipoTransaccion") & "</cbc:Description>" & vbCrLf &
                                  "  </cac:DiscrepancyResponse>" & vbCrLf &
                                  "    <cac:BillingReference>" & vbCrLf &
                                  "    <cac:InvoiceDocumentReference>" & vbCrLf &
                                  "      <cbc:ID>" & Fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoReferencialCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "      <cbc:DocumentTypeCode>" & Fila("vIdEquivalenciaContableTipoDocumentoReferencial") & "</cbc:DocumentTypeCode>" & vbCrLf &
                                  "    </cac:InvoiceDocumentReference>" & vbCrLf &
                                  "  </cac:BillingReference>" & vbCrLf

                'Datos del Emisor
                strXML = strXML + "    <cac:Signature>" & vbCrLf &
                                  "        <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "        <cac:SignatoryParty>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "        </cac:SignatoryParty>" & vbCrLf &
                                  "        <cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "            <cac:ExternalReference>" & vbCrLf &
                                  "                <cbc:URI>" & Fila("vRucEmpresa") & "</cbc:URI>" & vbCrLf &
                                  "            </cac:ExternalReference>" & vbCrLf &
                                  "        </cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "    </cac:Signature>" & vbCrLf

                'Tipo de Documento y Razón Social del Emisor (Empresa)
                strXML = strXML + "    <cac:AccountingSupplierParty>" & vbCrLf &
                                  "        <cac:Party>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoEmpresa") & """ schemeName=""Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name><![CDATA[" & Fila("vNombreComercialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "            <cac:PartyLegalEntity>" & vbCrLf &
                                  "                <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]></cbc:RegistrationName>" & vbCrLf &
                                  "                <cac:RegistrationAddress>" & vbCrLf &
                                  "                    <cbc:AddressTypeCode>" & Fila("cIdAnexoEstablecimientoEmpresa") & "</cbc:AddressTypeCode>" & vbCrLf &
                                  "                </cac:RegistrationAddress>" & vbCrLf &
                                  "            </cac:PartyLegalEntity>" & vbCrLf &
                                  "        </cac:Party>" & vbCrLf &
                                  "    </cac:AccountingSupplierParty>" & vbCrLf
                'De donde saco el codigo de anexo establecimiento??????

                '"<cbc:ID schemeID=""" & Fila("cIdTipoDocumentoCliente") & """ schemeName=""SUNAT:Identificador de Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vNumeroDocumentoClienteCabeceraDocumento") & "</cbc:ID>" & vbCrLf &

                'Tipo de Documento y Razon Social del Cliente
                strXML = strXML + "    <cac:AccountingCustomerParty>" & vbCrLf &
                                  "        <cac:Party>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoCliente") & """ schemeName=""Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vNumeroDocumentoClienteCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyLegalEntity>" & vbCrLf &
                                  "                <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialCabeceraDocumento") & "]]></cbc:RegistrationName>" & vbCrLf &
                                  "            </cac:PartyLegalEntity>" & vbCrLf &
                                  "        </cac:Party>" & vbCrLf &
                                  "    </cac:AccountingCustomerParty>" & vbCrLf

                'IGV Total del Documento
                strXML = strXML + "    <cac:TaxTotal>" & vbCrLf &
                                  "        <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "        <cac:TaxSubtotal>" & vbCrLf &
                                  "            <cbc:TaxableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoGravadoObligatorio")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                  "            <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "            <cac:TaxCategory>" & vbCrLf &
                                  "                <cac:TaxScheme>" & vbCrLf &
                                  "                    <cbc:ID schemeID=""UN/ECE 5153"" schemeAgencyID=""6"">1000</cbc:ID>" & vbCrLf &
                                  "                    <cbc:Name>IGV</cbc:Name>" & vbCrLf &
                                  "                    <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                  "                </cac:TaxScheme>" & vbCrLf &
                                  "            </cac:TaxCategory>" & vbCrLf &
                                  "        </cac:TaxSubtotal>" & vbCrLf &
                                  "    </cac:TaxTotal>" & vbCrLf

                'Importe total de la venta, cesión en uso o del servicio prestado
                strXML = strXML + "    <cac:LegalMonetaryTotal>" & vbCrLf
                strXML = strXML + "        <cbc:PayableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) & "</cbc:PayableAmount>" & vbCrLf &
                                  "    </cac:LegalMonetaryTotal>" & vbCrLf
            End If

            If stDocumento.IdTipoDocumento = "ND" Then
                'Cabecera del Documento XML
                strXML = strXML + "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf &
                                  "<DebitNote xmlns=""urn:oasis:names:specification:ubl:schema:xsd:DebitNote-2""" & vbCrLf &
                                  "    xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""" & vbCrLf &
                                  "    xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""" & vbCrLf &
                                  "    xmlns:ccts=""urn:un:unece:uncefact:documentation:2""" & vbCrLf &
                                  "    xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""" & vbCrLf &
                                  "    xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""" & vbCrLf &
                                  "    xmlns:qdt=""urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2""" & vbCrLf &
                                  "    xmlns:sac=""urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1""" & vbCrLf &
                                  "    xmlns:udt=""urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2""" & vbCrLf &
                                  "    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" & vbCrLf

                'Para registrar la firma
                strXML = strXML + "    <ext:UBLExtensions>" & vbCrLf &
                                  "        <ext:UBLExtension>" & vbCrLf &
                                  "            <ext:ExtensionContent>" & vbCrLf &
                                  "            </ext:ExtensionContent>" & vbCrLf &
                                  "        </ext:UBLExtension>" & vbCrLf &
                                  "    </ext:UBLExtensions>" & vbCrLf

                'Datos Cabecera Documento - Versión UBL - Fecha y Hora de Emisión - Importe total en letras
                strXML = strXML + "    <cbc:UBLVersionID>2.1</cbc:UBLVersionID>" & vbCrLf &
                                  "    <cbc:CustomizationID>2.0</cbc:CustomizationID>" & vbCrLf &
                                  "    <cbc:ID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "    <cbc:IssueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaEmisionCabeceraDocumento"))) & "</cbc:IssueDate>" & vbCrLf &
                                  "    <cbc:IssueTime>" & Fila("dHoraEmisionCabeceraDocumento").ToString.Trim & "</cbc:IssueTime>" & vbCrLf

                strXML = strXML + "    <cbc:DocumentCurrencyCode>" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "</cbc:DocumentCurrencyCode>" & vbCrLf &
                                  "  <cac:DiscrepancyResponse>" & vbCrLf &
                                  "    <cbc:ReferenceID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</cbc:ReferenceID>" & vbCrLf &
                                  "    <cbc:ResponseCode>" & Fila("vIdEquivalenciaSunatFormaPago") & "</cbc:ResponseCode>" & vbCrLf &
                                  "    <cbc:Description>" & Fila("vDescripcionFormaPago") & "</cbc:Description>" & vbCrLf &
                                  "  </cac:DiscrepancyResponse>" & vbCrLf &
                                  "    <cac:BillingReference>" & vbCrLf &
                                  "    <cac:InvoiceDocumentReference>" & vbCrLf &
                                  "      <cbc:ID>" & Fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoReferencialCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "      <cbc:DocumentTypeCode>" & Fila("vIdEquivalenciaContableTipoDocumentoReferencial") & "</cbc:DocumentTypeCode>" & vbCrLf &
                                  "    </cac:InvoiceDocumentReference>" & vbCrLf &
                                  "  </cac:BillingReference>" & vbCrLf

                'Datos del Emisor
                strXML = strXML + "    <cac:Signature>" & vbCrLf &
                                  "        <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "        <cac:SignatoryParty>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "        </cac:SignatoryParty>" & vbCrLf &
                                  "        <cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "            <cac:ExternalReference>" & vbCrLf &
                                  "                <cbc:URI>" & Fila("vRucEmpresa") & "</cbc:URI>" & vbCrLf &
                                  "            </cac:ExternalReference>" & vbCrLf &
                                  "        </cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "    </cac:Signature>" & vbCrLf

                'Tipo de Documento y Razón Social del Emisor (Empresa)
                strXML = strXML + "    <cac:AccountingSupplierParty>" & vbCrLf &
                                  "        <cac:Party>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoEmpresa") & """ schemeName=""SUNAT:Identificador de Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name><![CDATA[" & Fila("vNombreComercialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "            <cac:PartyLegalEntity>" & vbCrLf &
                                  "                <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]></cbc:RegistrationName>" & vbCrLf &
                                  "                <cac:RegistrationAddress>" & vbCrLf &
                                  "                    <cbc:AddressTypeCode>" & Fila("cIdAnexoEstablecimientoEmpresa") & "</cbc:AddressTypeCode>" & vbCrLf &
                                  "                </cac:RegistrationAddress>" & vbCrLf &
                                  "            </cac:PartyLegalEntity>" & vbCrLf &
                                  "        </cac:Party>" & vbCrLf &
                                  "    </cac:AccountingSupplierParty>" & vbCrLf
                'De donde saco el codigo de anexo establecimiento??????

                'Tipo de Documento y Razon Social del Cliente
                strXML = strXML + "    <cac:AccountingCustomerParty>" & vbCrLf &
                                  "        <cac:Party>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoCliente") & """ schemeName=""SUNAT:Identificador de Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vNumeroDocumentoClienteCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyLegalEntity>" & vbCrLf &
                                  "                <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialCabeceraDocumento") & "]]></cbc:RegistrationName>" & vbCrLf &
                                  "            </cac:PartyLegalEntity>" & vbCrLf &
                                  "        </cac:Party>" & vbCrLf &
                                  "    </cac:AccountingCustomerParty>" & vbCrLf

                'IGV Total del Documento
                strXML = strXML + "    <cac:TaxTotal>" & vbCrLf &
                                  "        <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "        <cac:TaxSubtotal>" & vbCrLf &
                                  "            <cbc:TaxableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoGravadoObligatorio")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                  "            <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "            <cac:TaxCategory>" & vbCrLf &
                                  "                <cac:TaxScheme>" & vbCrLf &
                                  "                    <cbc:ID schemeID=""UN/ECE 5153"" schemeAgencyID=""6"">1000</cbc:ID>" & vbCrLf &
                                  "                    <cbc:Name>IGV</cbc:Name>" & vbCrLf &
                                  "                    <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                  "                </cac:TaxScheme>" & vbCrLf &
                                  "            </cac:TaxCategory>" & vbCrLf &
                                  "        </cac:TaxSubtotal>" & vbCrLf &
                                  "    </cac:TaxTotal>" & vbCrLf

                'Importe total de la venta, cesión en uso o del servicio prestado
                strXML = strXML + "    <cac:RequestedMonetaryTotal>" & vbCrLf
                strXML = strXML + "        <cbc:PayableAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) & "</cbc:PayableAmount>" & vbCrLf &
                                  "    </cac:RequestedMonetaryTotal>" & vbCrLf
            End If

            'Inicio: Detalle de los Items**********************************************************************
            For Each FilaDet In dt.Rows
                If stDocumento.IdTipoDocumento = "BV" Or stDocumento.IdTipoDocumento = "FA" Then
                    strXML = strXML + "<cac:InvoiceLine>" & vbCrLf &
                                    "  <cbc:ID>" & FilaDet("nIdNumeroItemDetalleDocumento") & "</cbc:ID>" & vbCrLf &
                                    "  <cbc:InvoicedQuantity unitCode=""" & FilaDet("cIdUnidadMedidaSunat") & """ unitCodeListID=""UN/ECE rec 20"" unitCodeListAgencyName=""United Nations Economic Commission for Europe"">" & FilaDet("nCantidadProductoDetalleDocumento") & "</cbc:InvoicedQuantity>" & vbCrLf &
                                    "    <cbc:LineExtensionAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:LineExtensionAmount>" & vbCrLf &
                                    "  <cac:PricingReference>" & vbCrLf &
                                    "    <cac:AlternativeConditionPrice>" & vbCrLf &
                                    "        <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nPrecioUnitarioTotalVentaDetalleDocumento") - (FilaDet("nDescuentoVentaDetalleDocumento") * (FilaDet("nPorcentajeImpuestoCabeceraDocumento") / 100 + 1))), 2) & "</cbc:PriceAmount>" & vbCrLf &
                                    "        <cbc:PriceTypeCode listName=""Tipo de Precio"" listAgencyName=""PE:SUNAT"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo16"">" & FilaDet("cIdTipoPrecio") & "</cbc:PriceTypeCode>" & vbCrLf &
                                    "    </cac:AlternativeConditionPrice>" & vbCrLf &
                                    "  </cac:PricingReference>" & vbCrLf

                    '"        <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorUnitarioTotalVentaDetalleDocumento")), 10) & "</cbc:PriceAmount>" & vbCrLf &
                    '                                    "    <cbc:LineExtensionAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:LineExtensionAmount>" & vbCrLf &
                    '                                    "        <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorUnitarioTotalVentaDetalleDocumento")), 2) & "</cbc:PriceAmount>" & vbCrLf &

                    'Descuento
                    If Math.Round(Convert.ToDecimal(FilaDet("nDescuentoVentaDetalleDocumento")), 2) <> 0 Then
                        strXML = strXML + "  <cac:AllowanceCharge>" & vbCrLf &
                                          "    <cbc:ChargeIndicator>false</cbc:ChargeIndicator>" & vbCrLf &
                                          "    <cbc:AllowanceChargeReasonCode>" & "00" & "</cbc:AllowanceChargeReasonCode>" & vbCrLf &
                                          "    <cbc:MultiplierFactorNumeric>" & Math.Round(Convert.ToDecimal((100 * FilaDet("nDescuentoVentaDetalleDocumento") / ((FilaDet("nValorUnitarioTotalVentaDetalleDocumento") * FilaDet("nCantidadProductoDetalleDocumento")) - FilaDet("nISCDetalleDocumento"))) / 100), 2) & "</cbc:MultiplierFactorNumeric>" & vbCrLf &
                                          "    <cbc:Amount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nDescuentoVentaDetalleDocumento")), 2) & "</cbc:Amount>" & vbCrLf &
                                          "    <cbc:BaseAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorUnitarioTotalVentaDetalleDocumento") * FilaDet("nCantidadProductoDetalleDocumento")) - FilaDet("nISCDetalleDocumento"), 2) & "</cbc:BaseAmount>" & vbCrLf &
                                          "  </cac:AllowanceCharge>" & vbCrLf
                    End If
                    strXML = strXML + "  <cac:TaxTotal>" & vbCrLf
                    strXML = strXML + "      <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")) + Convert.ToDecimal(FilaDet("nISCDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf
                    '                    strXML = strXML + "      <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf

                    'Gravada - IGV
                    If FilaDet("cIdTipoAfectacion") = "10" Or FilaDet("cIdTipoAfectacion") = "11" Or FilaDet("cIdTipoAfectacion") = "12" Or FilaDet("cIdTipoAfectacion") = "13" Or FilaDet("cIdTipoAfectacion") = "14" Or FilaDet("cIdTipoAfectacion") = "15" Or FilaDet("cIdTipoAfectacion") = "16" Or FilaDet("cIdTipoAfectacion") = "17" Then
                        strXML = strXML + "      <cac:TaxSubtotal>" & vbCrLf &
                                    "          <cbc:TaxableAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")) + Convert.ToDecimal(FilaDet("nISCDetalleDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                    "          <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                    "          <cac:TaxCategory>" & vbCrLf &
                                    "              <cbc:ID schemeID=""UN/ECE 5305"" schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">S</cbc:ID>" & vbCrLf &
                                    "              <cbc:Percent>" & FilaDet("nPorcentajeImpuestoCabeceraDocumento") & "</cbc:Percent>" & vbCrLf &
                                    "              <cbc:TaxExemptionReasonCode listAgencyName=""PE:SUNAT"" listName=""Afectacion del IGV"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07"">" & FilaDet("cIdTipoAfectacion") & "</cbc:TaxExemptionReasonCode>" & vbCrLf &
                                    "              <cac:TaxScheme>" & vbCrLf &
                                    "                  <cbc:ID schemeID=""UN/ECE 5153"" schemeName=""Codigo de tributos"" schemeAgencyName=""PE:SUNAT"">1000</cbc:ID>" & vbCrLf &
                                    "                  <cbc:Name>IGV</cbc:Name>" & vbCrLf &
                                    "                  <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                    "              </cac:TaxScheme>" & vbCrLf &
                                    "          </cac:TaxCategory>" & vbCrLf &
                                    "      </cac:TaxSubtotal>" & vbCrLf

                    End If

                    'ISC - Detalle
                    If Math.Round(Convert.ToDecimal(Fila("nISCDetalleDocumento")), 2) > 0 Then
                        strXML = strXML + "      <cac:TaxSubtotal>" & vbCrLf &
                                    "          <cbc:TaxableAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nPrecioUnitarioSugeridoDetalleDocumento")) * Convert.ToDecimal(FilaDet("nCantidadProductoDetalleDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                    "          <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nISCDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                    "          <cac:TaxCategory>" & vbCrLf &
                                    "              <cbc:ID schemeID=""UN/ECE 5305"" schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">S</cbc:ID>" & vbCrLf &
                                    "              <cbc:Percent>" & FilaDet("nPorcentajeISCDetalleDocumento") & "</cbc:Percent>" & vbCrLf &
                                    "              <cbc:TierRange>" & FilaDet("cIdSistemaISCDetalleDocumento") & "</cbc:TierRange>" & vbCrLf &
                                    "              <cac:TaxScheme>" & vbCrLf &
                                    "                  <cbc:ID schemeID=""UN/ECE 5153"" schemeName=""Tax Scheme Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">2000</cbc:ID>" & vbCrLf &
                                    "                  <cbc:Name>ISC</cbc:Name>" & vbCrLf &
                                    "                  <cbc:TaxTypeCode>EXC</cbc:TaxTypeCode>" & vbCrLf &
                                    "              </cac:TaxScheme>" & vbCrLf &
                                    "          </cac:TaxCategory>" & vbCrLf &
                                    "      </cac:TaxSubtotal>" & vbCrLf
                        '"          <cbc:TaxableAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                        '"          <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &

                        '"  </cac:TaxTotal>" & vbCrLf
                    End If

                    'Exonerado
                    If FilaDet("cIdTipoAfectacion") = "20" Or FilaDet("cIdTipoAfectacion") = "21" Then
                        'strXML = strXML + "  <cac:TaxTotal>" & vbCrLf &
                        '            "      <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                        strXML = strXML + "      <cac:TaxSubtotal>" & vbCrLf &
                                    "          <cbc:TaxableAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                    "          <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & "0.00" & "</cbc:TaxAmount>" & vbCrLf &
                                    "          <cac:TaxCategory>" & vbCrLf &
                                    "              <cbc:ID schemeID=""UN/ECE 5305"" schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">E</cbc:ID>" & vbCrLf &
                                    "              <cbc:Percent>" & FilaDet("nPorcentajeImpuestoCabeceraDocumento") & "</cbc:Percent>" & vbCrLf &
                                    "              <cbc:TaxExemptionReasonCode listAgencyName=""PE:SUNAT"" listName=""SUNAT:Codigo de Tipo de Afectación del IGV"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07"">" & FilaDet("cIdTipoAfectacion") & "</cbc:TaxExemptionReasonCode>" & vbCrLf &
                                    "              <cac:TaxScheme>" & vbCrLf &
                                    "                  <cbc:ID schemeID=""UN/ECE 5153"" schemeName=""Tax Scheme Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">9997</cbc:ID>" & vbCrLf &
                                    "                  <cbc:Name>EXO</cbc:Name>" & vbCrLf &
                                    "                  <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                    "              </cac:TaxScheme>" & vbCrLf &
                                    "          </cac:TaxCategory>" & vbCrLf &
                                    "      </cac:TaxSubtotal>" & vbCrLf
                        '"  </cac:TaxTotal>" & vbCrLf
                    End If

                    'Inafecto
                    If FilaDet("cIdTipoAfectacion") = "30" Or FilaDet("cIdTipoAfectacion") = "31" Or FilaDet("cIdTipoAfectacion") = "32" Or FilaDet("cIdTipoAfectacion") = "33" Or FilaDet("cIdTipoAfectacion") = "34" Or FilaDet("cIdTipoAfectacion") = "35" Or FilaDet("cIdTipoAfectacion") = "36" Then
                        'strXML = strXML + "  <cac:TaxTotal>" & vbCrLf &
                        '            "      <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                        strXML = strXML + "      <cac:TaxSubtotal>" & vbCrLf &
                                    "          <cbc:TaxableAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                    "          <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & "0.00" & "</cbc:TaxAmount>" & vbCrLf &
                                    "          <cac:TaxCategory>" & vbCrLf &
                                    "              <cbc:ID schemeID=""UN/ECE 5305"" schemeName=""Tax Category Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">O</cbc:ID>" & vbCrLf &
                                    "              <cbc:Percent>" & FilaDet("nPorcentajeImpuestoCabeceraDocumento") & "</cbc:Percent>" & vbCrLf &
                                    "              <cbc:TaxExemptionReasonCode listAgencyName=""PE:SUNAT"" listName=""SUNAT:Codigo de Tipo de Afectación del IGV"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07"">" & FilaDet("cIdTipoAfectacion") & "</cbc:TaxExemptionReasonCode>" & vbCrLf &
                                    "              <cac:TaxScheme>" & vbCrLf &
                                    "                  <cbc:ID schemeID=""UN/ECE 5305"" schemeName=""Tax Scheme Identifier"" schemeAgencyName=""United Nations Economic Commission for Europe"">9998</cbc:ID>" & vbCrLf &
                                    "                  <cbc:Name>INA</cbc:Name>" & vbCrLf &
                                    "                  <cbc:TaxTypeCode>FRE</cbc:TaxTypeCode>" & vbCrLf &
                                    "              </cac:TaxScheme>" & vbCrLf &
                                    "          </cac:TaxCategory>" & vbCrLf &
                                    "      </cac:TaxSubtotal>" & vbCrLf
                        '"  </cac:TaxTotal>" & vbCrLf
                    End If
                    strXML = strXML + "  </cac:TaxTotal>" & vbCrLf &
                                        "  <cac:Item>" & vbCrLf &
                                        "    <cbc:Description><![CDATA[" & FilaDet("vDescripcionDetalleDocumento") & "]]></cbc:Description>" & vbCrLf &
                                        "    <cac:SellersItemIdentification>" & vbCrLf &
                                        "      <cbc:ID>" & FilaDet("cIdProducto") & "</cbc:ID>" & vbCrLf &
                                        "    </cac:SellersItemIdentification>" & vbCrLf &
                                        "    <cac:CommodityClassification>" & vbCrLf &
                                        "      <cbc:ItemClassificationCode listID=""UNSPSC"" listAgencyName=""GS1 US"" listName = ""Item Classification"" >" & FilaDet("vIdEquivalenciaSunatProducto") & "</cbc:ItemClassificationCode>" & vbCrLf &
                                        "    </cac:CommodityClassification>" & vbCrLf &
                                        "  </cac:Item>" & vbCrLf &
                                        "  <cac:Price>" & vbCrLf &
                                        "      <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorUnitarioTotalVentaDetalleDocumento")), 10) & "</cbc:PriceAmount>" & vbCrLf &
                                        "  </cac:Price>" & vbCrLf &
                                        "</cac:InvoiceLine>" & vbCrLf
                End If
                '"  <cbc:CreditedQuantity unitCode=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & FilaDet("nCantidadProductoDetalleDocumento") & "</cbc:CreditedQuantity>" & vbCrLf &
                If stDocumento.IdTipoDocumento = "NC" Then
                    strXML = strXML + "<cac:CreditNoteLine>" & vbCrLf &
                                "  <cbc:ID>" & FilaDet("nIdNumeroItemDetalleDocumento") & "</cbc:ID>" & vbCrLf &
                                "  <cbc:CreditedQuantity unitCode=""" & FilaDet("cIdUnidadMedidaSunat") & """>" & FilaDet("nCantidadProductoDetalleDocumento") & "</cbc:CreditedQuantity>" & vbCrLf &
                                "  <cbc:LineExtensionAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:LineExtensionAmount>" & vbCrLf &
                                "  <cac:PricingReference>" & vbCrLf &
                                "    <cac:AlternativeConditionPrice>" & vbCrLf &
                                "      <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nPrecioUnitarioTotalVentaDetalleDocumento")), 2) & "</cbc:PriceAmount>" & vbCrLf &
                                "      <cbc:PriceTypeCode>" & FilaDet("cIdTipoPrecio") & "</cbc:PriceTypeCode>" & vbCrLf &
                                "    </cac:AlternativeConditionPrice>" & vbCrLf &
                                "  </cac:PricingReference>" & vbCrLf

                    '"        <cbc:TaxExemptionReasonCode listAgencyName=""PE:SUNAT"" listName=""SUNAT: Codigo de Tipo de Afectaci&#xF3;n del IGV"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07"">" & FilaDet("cIdTipoAfectacion") & "</cbc:TaxExemptionReasonCode>" & vbCrLf &
                    strXML = strXML + "  <cac:TaxTotal>" & vbCrLf &
                                "    <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                "    <cac:TaxSubtotal>" & vbCrLf &
                                "      <cbc:TaxableAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                "      <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                "      <cac:TaxCategory>" & vbCrLf &
                                "        <cbc:Percent>" & FilaDet("nPorcentajeImpuestoCabeceraDocumento") & "</cbc:Percent>" & vbCrLf &
                                "        <cbc:TaxExemptionReasonCode listAgencyName=""PE:SUNAT"" listName=""Afectacion del IGV"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07"">" & FilaDet("cIdTipoAfectacion") & "</cbc:TaxExemptionReasonCode>" & vbCrLf &
                                "        <cac:TaxScheme>" & vbCrLf &
                                "          <cbc:ID>1000</cbc:ID>" & vbCrLf &
                                "          <cbc:Name>IGV</cbc:Name>" & vbCrLf &
                                "          <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                "        </cac:TaxScheme>" & vbCrLf &
                                "      </cac:TaxCategory>" & vbCrLf &
                                "    </cac:TaxSubtotal>" & vbCrLf &
                                "  </cac:TaxTotal>" & vbCrLf

                    strXML = strXML + "  <cac:Item>" & vbCrLf &
                                "    <cbc:Description><![CDATA[" & FilaDet("vDescripcionDetalleDocumento") & "]]></cbc:Description>" & vbCrLf &
                                "    <cac:SellersItemIdentification>" & vbCrLf &
                                "      <cbc:ID>" & FilaDet("cIdProducto") & "</cbc:ID>" & vbCrLf &
                                "    </cac:SellersItemIdentification>" & vbCrLf &
                                "  </cac:Item>" & vbCrLf &
                                "  <cac:Price>" & vbCrLf &
                                "    <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorUnitarioTotalVentaDetalleDocumento")), 2) & "</cbc:PriceAmount>" & vbCrLf &
                                "  </cac:Price>" & vbCrLf &
                                "</cac:CreditNoteLine>" & vbCrLf
                End If
                If stDocumento.IdTipoDocumento = "ND" Then
                    strXML = strXML + "<cac:DebitNoteLine>" & vbCrLf &
                                "  <cbc:ID>" & FilaDet("nIdNumeroItemDetalleDocumento") & "</cbc:ID>" & vbCrLf &
                                "  <cbc:DebitedQuantity unitCode=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & FilaDet("nCantidadProductoDetalleDocumento") & "</cbc:DebitedQuantity>" & vbCrLf &
                                "  <cbc:LineExtensionAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:LineExtensionAmount>" & vbCrLf &
                                "  <cac:PricingReference>" & vbCrLf &
                                "    <cac:AlternativeConditionPrice>" & vbCrLf &
                                "      <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>4</cbc:PriceAmount>" & vbCrLf &
                                "      <cbc:PriceTypeCode>" & FilaDet("cIdTipoPrecio") & "</cbc:PriceTypeCode>" & vbCrLf &
                                "    </cac:AlternativeConditionPrice>" & vbCrLf &
                                "  </cac:PricingReference>" & vbCrLf

                    strXML = strXML + "  <cac:TaxTotal>" & vbCrLf &
                                "    <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                "    <cac:TaxSubtotal>" & vbCrLf &
                                "      <cbc:TaxableAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorVentaDetalleDocumento")), 2) & "</cbc:TaxableAmount>" & vbCrLf &
                                "      <cbc:TaxAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nIGVDetalleDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                "      <cac:TaxCategory>" & vbCrLf &
                                "        <cbc:Percent>" & FilaDet("nPorcentajeImpuestoCabeceraDocumento") & "</cbc:Percent>" & vbCrLf &
                                "        <cbc:TaxExemptionReasonCode listAgencyName=""PE:SUNAT"" listName=""SUNAT: Codigo de Tipo de Afectaci&#xF3;n del IGV"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07"">" & FilaDet("cIdTipoAfectacion") & "</cbc:TaxExemptionReasonCode>" & vbCrLf &
                                "        <cac:TaxScheme>" & vbCrLf &
                                "          <cbc:ID>1000</cbc:ID>" & vbCrLf &
                                "          <cbc:Name>IGV</cbc:Name>" & vbCrLf &
                                "          <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                "        </cac:TaxScheme>" & vbCrLf &
                                "      </cac:TaxCategory>" & vbCrLf &
                                "    </cac:TaxSubtotal>" & vbCrLf &
                                "  </cac:TaxTotal>" & vbCrLf

                    strXML = strXML + "  <cac:Item>" & vbCrLf &
                                "    <cbc:Description><![CDATA[" & FilaDet("vDescripcionDetalleDocumento") & "]]></cbc:Description>" & vbCrLf &
                                "    <cac:SellersItemIdentification>" & vbCrLf &
                                "      <cbc:ID>" & FilaDet("cIdProducto") & "</cbc:ID>" & vbCrLf &
                                "    </cac:SellersItemIdentification>" & vbCrLf &
                                "  </cac:Item>" & vbCrLf &
                                "  <cac:Price>" & vbCrLf &
                                "    <cbc:PriceAmount currencyID=""" & FilaDet("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(FilaDet("nValorUnitarioTotalVentaDetalleDocumento")), 2) & "</cbc:PriceAmount>" & vbCrLf &
                                "  </cac:Price>" & vbCrLf &
                                "</cac:DebitNoteLine>" & vbCrLf
                End If
            Next
            'Final: Detalle de los Items**********************************************************************

            'Fin del Documento
            If stDocumento.IdTipoDocumento = "BV" Or stDocumento.IdTipoDocumento = "FA" Then
                strXML = strXML + "</Invoice>"
            ElseIf stDocumento.IdTipoDocumento = "NC" Then
                strXML = strXML + "</CreditNote>"
            ElseIf stDocumento.IdTipoDocumento = "ND" Then
                strXML = strXML + "</DebitNote>"
            End If

            doc.LoadXml(strXML)

            'Si el directorio no existe, crearlo
            If Not (Directory.Exists(Ruta)) Then Directory.CreateDirectory(Ruta)

            'doc.Save(Ruta & "\" & strNombreArchivo & ".xml")
            doc.Save(Ruta & "\" & stDocumento.NombreArchivo & ".xml")

            stDocumento.XML = strXML
            Return stDocumento
            cnx.Close()
        Catch ex As Exception
            'Return "" ' dt
            Throw New Exception(ex.Message)
        End Try
    End Function

    'Versión Antigua XML Guía
    Public Function GenerarGuiaEXML(stDocumento As stCPE, ByVal Ruta As String) As stCPE  'As Boolean
        'Public Function GenerarCPEXML(ByVal IdTipoDocumento As String, ByVal IdNumeroSerieDocumento As String,
        '    ByVal IdNumeroCorrelativo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal Ruta As String) As String  'As Boolean
        'CREADO PARA LA GENERACIÓN DEL ARCHIVO XML DE FACTURACIÓN ELECTRÓNICA DE SUNAT
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_LOGI_RPT_DOCUMENTOGUIA"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdTipoDocumento", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroSerieDocumentoCabeceraGuia", SqlDbType.VarChar, 6))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroCorrelativoCabeceraGuia", SqlDbType.VarChar, 20))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cIdTipoDocumento").Value = stDocumento.IdTipoDocumento 'IdTipoDocumento
        cmd.Parameters("@vIdNumeroSerieDocumentoCabeceraGuia").Value = stDocumento.IdNumeroSerieDocumento 'IdNumeroSerieDocumento
        cmd.Parameters("@vIdNumeroCorrelativoCabeceraGuia").Value = stDocumento.IdNumeroCorrelativo 'IdNumeroCorrelativo
        cmd.Parameters("@cIdPuntoVenta").Value = stDocumento.IdPuntoVenta
        cmd.Parameters("@cIdEmpresa").Value = stDocumento.IdEmpresa

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "DocumentoGuia")

        Try
            cnx.Open()
            '1Return True
            'As DataTable
            Dim strXML As String = ""
            Dim doc As New XmlDocument()

            Dim Fila = ds.Tables("DocumentoGuia").Rows(0)
            'strNombreArchivo = Fila("vRucEmpresa") & "-" & Fila("vIdEquivalenciaContableTipoDocumento") & "-" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento")
            stDocumento.Ruc = Fila("vRucEmpresa")
            stDocumento.IdTipoDocumentoSunat = Fila("vIdEquivalenciaContableTipoDocumento")
            dt = ds.Tables("DocumentoGuia")
            If stDocumento.IdTipoDocumento = "GR" Then
                'Cabecera del Documento XML
                strXML = strXML + "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf &
                                  "<DespatchAdvice xmlns=""urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2""" & vbCrLf &
                                  "    xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""" & vbCrLf &
                                  "    xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""" & vbCrLf &
                                  "    xmlns:ccts=""urn:un:unece:uncefact:documentation:2""" & vbCrLf &
                                  "    xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""" & vbCrLf &
                                  "    xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""" & vbCrLf &
                                  "    xmlns:qdt=""urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2""" & vbCrLf &
                                  "    xmlns:udt=""urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2""" & vbCrLf &
                                  "    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" & vbCrLf
                'Para registrar la firma
                strXML = strXML + "    <ext:UBLExtensions>" & vbCrLf &
                                  "        <ext:UBLExtension>" & vbCrLf &
                                  "            <ext:ExtensionContent>" & vbCrLf &
                                  "            </ext:ExtensionContent>" & vbCrLf &
                                  "        </ext:UBLExtension>" & vbCrLf &
                                  "    </ext:UBLExtensions>" & vbCrLf

                strXML = strXML + "    <cbc:UBLVersionID>2.1</cbc:UBLVersionID>" & vbCrLf &
                                  "    <cbc:CustomizationID>1.0</cbc:CustomizationID>" & vbCrLf &
                                  "    <cbc:ID>" & Fila("vIdNumeroSerieDocumentoCabeceraGuia") & "-" & Fila("vIdNumeroCorrelativoCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                  "    <cbc:IssueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaEmisionCabeceraGuia"))) & "</cbc:IssueDate>" & vbCrLf &
                                  "    <cbc:DespatchAdviceTypeCode>" & Fila("vIdEquivalenciaContableTipoDocumento") & "</cbc:DespatchAdviceTypeCode>" & vbCrLf &
                                  "    <cbc:Note>" & Fila("vObservacionCabeceraGuia") & "</cbc:Note>" & vbCrLf

                'Datos del Emisor
                strXML = strXML + "    <cac:Signature>" & vbCrLf &
                                  "        <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "        <cac:SignatoryParty>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "        </cac:SignatoryParty>" & vbCrLf &
                                  "        <cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "            <cac:ExternalReference>" & vbCrLf &
                                  "                <cbc:URI>" & Fila("vRucEmpresa") & "</cbc:URI>" & vbCrLf &
                                  "            </cac:ExternalReference>" & vbCrLf &
                                  "        </cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "    </cac:Signature>" & vbCrLf

                'Datos del Remitente
                strXML = strXML + "    <cac:DespatchSupplierParty>" & vbCrLf &
                                  "       <cbc:CustomerAssignedAccountID schemeID=""6"">" & Fila("vRucEmpresa") & "</cbc:CustomerAssignedAccountID>" & vbCrLf &
                                  "       <cac:Party>" & vbCrLf &
                                  "          <cac:PartyLegalEntity>" & vbCrLf &
                                  "             <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialEmpresa") & "]]>" & vbCrLf &
                                  "             </cbc:RegistrationName>" & vbCrLf &
                                  "          </cac:PartyLegalEntity>" & vbCrLf &
                                  "       </cac:Party>" & vbCrLf &
                                  "    </cac:DespatchSupplierParty>" & vbCrLf

                'Datos del Destinatario
                strXML = strXML + "    <cac:DeliveryCustomerParty>" & vbCrLf &
                                  "       <cbc:CustomerAssignedAccountID schemeID=""" & Fila("cIdTipoDocumentoClienteCabeceraGuia") & """>" & Fila("vNumeroDocumentoClienteCabeceraGuia") & "</cbc:CustomerAssignedAccountID>" & vbCrLf &
                                  "       <cac:Party>" & vbCrLf &
                                  "          <cac:PartyLegalEntity>" & vbCrLf &
                                  "             <cbc:RegistrationName><![CDATA[" & Fila("vRazonSocialCabeceraGuia") & "]]>" & vbCrLf &
                                  "             </cbc:RegistrationName>" & vbCrLf &
                                  "          </cac:PartyLegalEntity>" & vbCrLf &
                                  "       </cac:Party>" & vbCrLf &
                                  "    </cac:DeliveryCustomerParty>" & vbCrLf

                'Datos del Envio
                strXML = strXML + "	<cac:Shipment>" & vbCrLf &
                                  "		<cbc:ID>1</cbc:ID>" & vbCrLf &
                                  "		<cbc:HandlingCode>" & Fila("vIdEquivalenciaSunatTipoTransaccion") & "</cbc:HandlingCode>" & vbCrLf &
                                  "		<cbc:Information><![CDATA[" & Fila("vDescripcionTipoTransaccion") & "]]></cbc:Information>" & vbCrLf &
                                  "		<cbc:GrossWeightMeasure unitCode=""" & Fila("cIdUnidadMedidaCabeceraGuia") & """>" & Fila("nPesoBrutoTotalCabeceraGuia") & "</cbc:GrossWeightMeasure>" & vbCrLf &
                                  "		<cbc:TotalTransportHandlingUnitQuantity>" & Convert.ToInt32(Fila("nNumeroBultosPalletsCabeceraGuia")) & "</cbc:TotalTransportHandlingUnitQuantity>" & vbCrLf &
                                  "		<cbc:SplitConsignmentIndicator>" & LCase(Fila("bIndicadorTransbordoProgramadoCabeceraGuia")) & "</cbc:SplitConsignmentIndicator>" & vbCrLf &
                                  "		<cac:ShipmentStage>" & vbCrLf &
                                  "			<cbc:ID>1</cbc:ID>" & vbCrLf &
                                  "			<cbc:TransportModeCode>" & Fila("cIdModalidadTrasladoCabeceraGuia") & "</cbc:TransportModeCode>" & vbCrLf &
                                  "			<cac:TransitPeriod>" & vbCrLf &
                                  "				<cbc:StartDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaEntregaCabeceraGuia"))) & "</cbc:StartDate>" & vbCrLf &
                                  "			</cac:TransitPeriod>" & vbCrLf

                'Datos del Vehiculo
                strXML = strXML + "          <cac:TransportMeans>" & vbCrLf &
                                  "             <cac:RoadTransport>" & vbCrLf &
                                  "                <cbc:LicensePlateID>" & Fila("vPlacaTransportistaCabeceraGuia") & "</cbc:LicensePlateID>" & vbCrLf &
                                  "             </cac:RoadTransport>" & vbCrLf &
                                  "          </cac:TransportMeans>" & vbCrLf

                'Datos del conductor
                strXML = strXML + "          <cac:DriverPerson>" & vbCrLf &
                                  "             <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoConductorTransportistaCabeceraGuia") & """>" & Fila("vNumeroDocumentoConductorTransportistaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                  "          </cac:DriverPerson>" & vbCrLf &
                                  "       </cac:ShipmentStage>" & vbCrLf

                'Dirección punto de llegada
                strXML = strXML + "       <cac:Delivery>" & vbCrLf &
                                  "          <cac:DeliveryAddress>" & vbCrLf &
                                  "             <cbc:ID>" & Fila("cIdUbiGeoEntregaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                  "             <cbc:StreetName> <![CDATA[" & Fila("vDireccionLLegadaCabeceraGuia") & "]]></cbc:StreetName>" & vbCrLf &
                                  "          </cac:DeliveryAddress>" & vbCrLf &
                                  "       </cac:Delivery>" & vbCrLf

                'Datos del Contenedor 'LO QUITE JMUG: 20/01/2022
                'strXML = strXML + "<cac:TransportHandlingUnit>" & vbCrLf &
                '                  "<cac:TransportEquipment>" & vbCrLf &
                '                  "    <cbc:ID>120606</cbc:ID>" & vbCrLf &
                '                  "</cac:TransportEquipment>" & vbCrLf &
                '                  "</cac:TransportHandlingUnit>" & vbCrLf

                ''Datos del Transportista
                'strXML = strXML + "          <cac:CarrierParty>" & vbCrLf &
                '                  "             <cac:PartyIdentification>" & vbCrLf &
                '                  "                <cbc:ID schemeID=""6"">10209865209</cbc:ID>" & vbCrLf &
                '                  "             </cac:PartyIdentification>" & vbCrLf &
                '                  "             <cac:PartyName>" & vbCrLf &
                '                  "                <cbc:Name><![CDATA[PERUQUIMICOS S.A.C.]]>" & vbCrLf &
                '                  "                </cbc:Name>" & vbCrLf &
                '                  "             </cac:PartyName>" & vbCrLf &
                '                  "          </cac:CarrierParty>" & vbCrLf



                'Dirección punto de partida
                strXML = strXML + "       <cac:OriginAddress>" & vbCrLf &
                                  "          <cbc:ID>" & Fila("cIdUbiGeoPartidaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                  "          <cbc:StreetName> <![CDATA[" & Fila("vDireccionPartidaCabeceraGuia") & "]]></cbc:StreetName>" & vbCrLf &
                                  "       </cac:OriginAddress>" & vbCrLf

                'Código de Puerto o Aeropuerto de embarque/desembarque
                'strXML = strXML + "       <cac:FirstArrivalPortLocation>" & vbCrLf &
                '                  "          <cbc:ID>PAI</cbc:ID>" & vbCrLf &
                '                  "       </cac:FirstArrivalPortLocation>" & vbCrLf &
                '                  "    </cac:Shipment>" & vbCrLf
                strXML = strXML + "    </cac:Shipment>" & vbCrLf

                'Bienes a transportar
                For Each FilaDet In dt.Rows
                    strXML = strXML + "    <cac:DespatchLine>" & vbCrLf &
                                      "       <cbc:ID>" & FilaDet("nIdNumeroItemDetalleGuia") & "</cbc:ID>" & vbCrLf &
                                      "       <cbc:DeliveredQuantity unitCode=""" & FilaDet("cIdUnidadMedidaProductoDetalleGuia") & """>" & FilaDet("nCantidadProductoDetalleGuia") & "</cbc:DeliveredQuantity>" & vbCrLf &
                                      "       <cac:OrderLineReference>" & vbCrLf &
                                      "          <cbc:LineID>" & FilaDet("nIdNumeroItemDetalleGuia") & "</cbc:LineID>" & vbCrLf &
                                      "       </cac:OrderLineReference>" & vbCrLf &
                                      "       <cac:Item>" & vbCrLf &
                                      "          <cbc:Name><![CDATA[" & FilaDet("vDescripcionDetalleGuia") & "]]></cbc:Name>" & vbCrLf &
                                      "          <cac:SellersItemIdentification>" & vbCrLf &
                                      "          <cbc:ID>" & FilaDet("cIdProducto") & "</cbc:ID>" & vbCrLf &
                                      "          </cac:SellersItemIdentification>" & vbCrLf &
                                      "       </cac:Item>" & vbCrLf &
                                      "    </cac:DespatchLine>" & vbCrLf
                Next
                strXML = strXML + "</DespatchAdvice>"
            End If

            doc.LoadXml(strXML)

            'Si el directorio no existe, crearlo
            If Not (Directory.Exists(Ruta)) Then Directory.CreateDirectory(Ruta)

            'doc.Save(Ruta & "\" & strNombreArchivo & ".xml")
            doc.Save(Ruta & "\" & stDocumento.NombreArchivo & ".xml")

            stDocumento.XML = strXML
            Return stDocumento
            cnx.Close()
        Catch ex As Exception
            'Return "" ' dt
            Throw New Exception(ex.Message)
        End Try
    End Function

    'Nueva para el API
    Public Function GenerarGuiaRemitenteXML(stDocumento As stCPE, ByVal Ruta As String) As stCPE  'As Boolean
        'CREADO PARA LA GENERACIÓN DEL ARCHIVO XML DE FACTURACIÓN ELECTRÓNICA DE SUNAT
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_LOGI_RPT_DOCUMENTOGUIA"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdTipoDocumento", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroSerieDocumentoCabeceraGuia", SqlDbType.VarChar, 6))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroCorrelativoCabeceraGuia", SqlDbType.VarChar, 20))
        'cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cIdTipoDocumento").Value = stDocumento.IdTipoDocumento 'IdTipoDocumento
        cmd.Parameters("@vIdNumeroSerieDocumentoCabeceraGuia").Value = stDocumento.IdNumeroSerieDocumento 'IdNumeroSerieDocumento
        cmd.Parameters("@vIdNumeroCorrelativoCabeceraGuia").Value = stDocumento.IdNumeroCorrelativo 'IdNumeroCorrelativo
        'cmd.Parameters("@cIdPuntoVenta").Value = stDocumento.IdPuntoVenta
        cmd.Parameters("@cIdEmpresa").Value = stDocumento.IdEmpresa

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "DocumentoGuia")

        Try
            cnx.Open()
            '1Return True
            'As DataTable
            Dim strXML As String = ""
            Dim doc As New XmlDocument()

            Dim Fila = ds.Tables("DocumentoGuia").Rows(0)
            'strNombreArchivo = Fila("vRucEmpresa") & "-" & Fila("vIdEquivalenciaContableTipoDocumento") & "-" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento")
            stDocumento.Ruc = Fila("vRucEmpresa")
            stDocumento.IdTipoDocumentoSunat = Fila("vIdEquivalenciaContableTipoDocumento")

            dt = ds.Tables("DocumentoGuia")
            If stDocumento.IdTipoDocumento = "GR" Then
                'Cabecera del Documento XML
                strXML = strXML + "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf &
                                  "<DespatchAdvice xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""" & vbCrLf &
                                  "   xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""" & vbCrLf &
                                  "   xmlns:qdt=""urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2""" & vbCrLf &
                                  "   xmlns:sac=""urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1""" & vbCrLf &
                                  "   xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""" & vbCrLf &
                                  "   xmlns:udt=""urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2""" & vbCrLf &
                                  "   xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""" & vbCrLf &
                                  "   xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""" & vbCrLf &
                                  "   xmlns:ccts=""urn:un:unece:uncefact:documentation:2""" & vbCrLf &
                                  "   xmlns=""urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2"">" & vbCrLf

                'Para registrar la firma
                strXML = strXML + "    <ext:UBLExtensions>" & vbCrLf &
                                  "        <ext:UBLExtension>" & vbCrLf &
                                  "            <ext:ExtensionContent>" & vbCrLf &
                                  "            </ext:ExtensionContent>" & vbCrLf &
                                  "        </ext:UBLExtension>" & vbCrLf &
                                  "    </ext:UBLExtensions>" & vbCrLf

                strXML = strXML + "    <cbc:UBLVersionID>2.1</cbc:UBLVersionID>" & vbCrLf &
                                  "    <cbc:CustomizationID>2.0</cbc:CustomizationID>" & vbCrLf &
                                  "    <cbc:ID>" & Fila("vIdNumeroSerieDocumentoCabeceraGuia") & "-" & Fila("vIdNumeroCorrelativoCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                  "    <cbc:IssueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaEmisionCabeceraGuia"))) & "</cbc:IssueDate>" & vbCrLf &
                                  "    <cbc:IssueTime>" & Fila("dHoraEmisionCabeceraGuia").Trim & "</cbc:IssueTime>" & vbCrLf &
                                  "    <cbc:DespatchAdviceTypeCode listAgencyName=""PE:SUNAT"" listName=""Tipo de Documento"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01"">" & Fila("vIdEquivalenciaContableTipoDocumento") & "</cbc:DespatchAdviceTypeCode>" & vbCrLf &
                                  "    <cbc:Note>" & Fila("vObservacionCabeceraGuia") & "</cbc:Note>" & vbCrLf

                'Datos del Emisor
                strXML = strXML + "    <cac:Signature>" & vbCrLf &
                                  "        <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "        <cac:SignatoryParty>" & vbCrLf &
                                  "            <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID>" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "            </cac:PartyIdentification>" & vbCrLf &
                                  "            <cac:PartyName>" & vbCrLf &
                                  "                <cbc:Name>" & Fila("vRazonSocialEmpresa") & "</cbc:Name>" & vbCrLf &
                                  "            </cac:PartyName>" & vbCrLf &
                                  "        </cac:SignatoryParty>" & vbCrLf &
                                  "        <cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "            <cac:ExternalReference>" & vbCrLf &
                                  "                <cbc:URI>" & Fila("vRucEmpresa") & "</cbc:URI>" & vbCrLf &
                                  "            </cac:ExternalReference>" & vbCrLf &
                                  "        </cac:DigitalSignatureAttachment>" & vbCrLf &
                                  "    </cac:Signature>" & vbCrLf

                'Datos del Remitente
                strXML = strXML + "    <cac:DespatchSupplierParty>" & vbCrLf &
                                  "       <cac:Party>" & vbCrLf &
                                  "          <cac:PartyIdentification>" & vbCrLf &
                                  "             <cbc:ID schemeID=""6"" schemeName=""Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                                  "          </cac:PartyIdentification>" & vbCrLf &
                                  "          <cac:PartyLegalEntity>" & vbCrLf &
                                  "             <cbc:RegistrationName>" & SunatConversionCaracteresEspeciales(Fila("vRazonSocialEmpresa")) & "</cbc:RegistrationName>" & vbCrLf &
                                  "          </cac:PartyLegalEntity>" & vbCrLf &
                                  "       </cac:Party>" & vbCrLf &
                                  "    </cac:DespatchSupplierParty>" & vbCrLf

                'Datos del Destinatario
                strXML = strXML + "    <cac:DeliveryCustomerParty>" & vbCrLf &
                                  "       <cac:Party>" & vbCrLf &
                                  "          <cac:PartyIdentification>" & vbCrLf &
                                  "             <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoClienteCabeceraGuia") & """ schemeName=""Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vNumeroDocumentoClienteCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                  "          </cac:PartyIdentification>" & vbCrLf &
                                  "          <cac:PartyLegalEntity>" & vbCrLf &
                                  "             <cbc:RegistrationName>" & SunatConversionCaracteresEspeciales(Fila("vRazonSocialCabeceraGuia")) & "</cbc:RegistrationName>" & vbCrLf &
                                  "          </cac:PartyLegalEntity>" & vbCrLf &
                                  "       </cac:Party>" & vbCrLf &
                                  "    </cac:DeliveryCustomerParty>" & vbCrLf

                'Datos del Envio
                strXML = strXML + " <cac:Shipment>" & vbCrLf &
                                  "     <cbc:ID>1</cbc:ID>" & vbCrLf &
                                  "     <cbc:HandlingCode listAgencyName=""PE:SUNAT"" listName=""Motivo de traslado"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo20"">" & Fila("vIdEquivalenciaSunatTipoTransaccion") & "</cbc:HandlingCode>" & vbCrLf &
                                  "     <cbc:HandlingInstructions>" & Fila("vDescripcionTipoTransaccion") & "</cbc:HandlingInstructions>" & vbCrLf &
                                  "     <cbc:GrossWeightMeasure unitCode=""" & Fila("cIdUnidadMedidaCabeceraGuia") & """>" & Fila("nPesoBrutoTotalCabeceraGuia") & "</cbc:GrossWeightMeasure>" & vbCrLf &
                                  "     <cbc:TotalTransportHandlingUnitQuantity>" & Convert.ToInt32(Fila("nNumeroBultosPalletsCabeceraGuia")) & "</cbc:TotalTransportHandlingUnitQuantity>" & vbCrLf

                If Fila("cIdModalidadTrasladoCabeceraGuia") = "02" And Fila("bIndicadorConPlacaLicenciaCabeceraGuia") = False Then
                    'Traslado Vehiculo M1L - No necesita de placa ni de licencia
                    strXML = strXML + "     <cbc:SpecialInstructions>SUNAT_Envio_IndicadorTrasladoVehiculoM1L</cbc:SpecialInstructions>" & vbCrLf
                End If

                strXML = strXML + "     <cac:ShipmentStage>" & vbCrLf &
                                  "         <cbc:ID>1</cbc:ID>" & vbCrLf &
                                  "         <cbc:TransportModeCode listName=""Modalidad de traslado"" listAgencyName=""PE:SUNAT"" listURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo18"">" & Fila("cIdModalidadTrasladoCabeceraGuia") & "</cbc:TransportModeCode>" & vbCrLf &
                                  "         <cac:TransitPeriod>" & vbCrLf &
                                  "             <cbc:StartDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Fila("dFechaEntregaCabeceraGuia"))) & "</cbc:StartDate>" & vbCrLf &
                                  "         </cac:TransitPeriod>" & vbCrLf

                'If Fila("vIdEquivalenciaSunatTipoTransaccion") = "01" Then '01-Publico / 02-Privado
                If Fila("cIdModalidadTrasladoCabeceraGuia") = "01" Then '01-Publico / 02-Privado
                    'Datos del Transportista
                    strXML = strXML + "          <cac:CarrierParty>" & vbCrLf &
                                  "             <cac:PartyIdentification>" & vbCrLf &
                                  "                <cbc:ID schemeID=""6"">" & Fila("vRucTransportista") & "</cbc:ID>" & vbCrLf &
                                  "             </cac:PartyIdentification>" & vbCrLf &
                                  "             <cac:PartyLegalEntity>" & vbCrLf &
                                  "                <cbc:RegistrationName>" & SunatConversionCaracteresEspeciales(Fila("vRazonSocialTransportistaCabeceraGuia")) & "</cbc:RegistrationName>" & vbCrLf &
                                  "             </cac:PartyLegalEntity>" & vbCrLf &
                                  "          </cac:CarrierParty>" & vbCrLf &
                                  "       </cac:ShipmentStage>" & vbCrLf
                End If

                If Fila("cIdModalidadTrasladoCabeceraGuia") = "02" Then '01-Publico / 02-Privado
                    If Fila("bIndicadorConPlacaLicenciaCabeceraGuia") = True Then
                        'Datos del Vehiculo
                        strXML = strXML + "          <cac:TransportMeans>" & vbCrLf &
                                          "             <cac:RoadTransport>" & vbCrLf &
                                          "                <cbc:LicensePlateID>" & Fila("vPlacaTransportistaCabeceraGuia") & "</cbc:LicensePlateID>" & vbCrLf &
                                          "             </cac:RoadTransport>" & vbCrLf &
                                          "          </cac:TransportMeans>" & vbCrLf

                        'Datos del conductor
                        strXML = strXML + "          <cac:DriverPerson>" & vbCrLf &
                                          "             <cbc:ID schemeID=""" & Fila("cIdTipoDocumentoConductorTransportistaCabeceraGuia") & """ schemeName=""Documento de Identidad"" schemeAgencyName=""PE:SUNAT"" schemeURI=""urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"">" & Fila("vNumeroDocumentoConductorTransportistaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                          "             <cbc:FirstName>" & Fila("vRazonSocialLicenciaConductor") & "</cbc:FirstName>" & vbCrLf &
                                          "             <cbc:FamilyName>" & Fila("vRazonSocialLicenciaConductor") & "</cbc:FamilyName>" & vbCrLf &
                                          "             <cbc:JobTitle>Principal</cbc:JobTitle>" & vbCrLf &
                                          "             <cac:IdentityDocumentReference>" & vbCrLf &
                                          "                <cbc:ID>" & Fila("vLicenciaConductorTransportistaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                          "             </cac:IdentityDocumentReference>" & vbCrLf &
                                          "          </cac:DriverPerson>" & vbCrLf
                    End If
                    strXML = strXML + "       </cac:ShipmentStage>" & vbCrLf
                End If

                'Dirección punto de llegada
                'strXML = strXML + "       <cac:Delivery>" & vbCrLf &
                '                  "          <cac:DeliveryAddress>" & vbCrLf &
                '                  "             <cbc:ID schemeAgencyName = ""PE:INEI"" schemeName=""Ubigeos"">" & Fila("cIdUbiGeoEntregaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                '                  "             <cac:AddressLine>" & vbCrLf &
                '                  "                <cbc:Line>" & Fila("vDireccionLLegadaCabeceraGuia") & "</cbc:Line>" & vbCrLf &
                '                  "             </cac:AddressLine>" & vbCrLf &
                '                  "          </cac:DeliveryAddress>" & vbCrLf
                ''                  "       </cac:Delivery>" & vbCrLf

                '"             <cbc:ID schemeAgencyName = ""PE:INEI"" schemeName=""Ubigeos"">" & Fila("cIdUbiGeoEntregaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                '"             <cbc:AddressTypeCode listID="" &  & "" listAgencyName=""PE:SUNAT"" listName=""Establecimientos anexos"">0</cbc:AddressTypeCode>" & vbCrld &

                strXML = strXML + "       <cac:Delivery>" & vbCrLf &
                                  "          <cac:DeliveryAddress>" & vbCrLf &
                                  "             <cbc:ID schemeAgencyName = ""PE:INEI"" schemeName=""Ubigeos"">" & Fila("cIdUbiGeoEntregaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                                  "             <cac:AddressLine>" & vbCrLf &
                                  "                <cbc:Line>" & Fila("vDireccionLLegadaCabeceraGuia") & "</cbc:Line>" & vbCrLf &
                                  "             </cac:AddressLine>" & vbCrLf &
                                  "          </cac:DeliveryAddress>" & vbCrLf


                ''Datos del Transportista
                'strXML = strXML + "          <cac:CarrierParty>" & vbCrLf &
                '                  "             <cac:PartyIdentification>" & vbCrLf &
                '                  "                <cbc:ID schemeID=""6"">10209865209</cbc:ID>" & vbCrLf &
                '                  "             </cac:PartyIdentification>" & vbCrLf &
                '                  "             <cac:PartyName>" & vbCrLf &
                '                  "                <cbc:Name><![CDATA[PERUQUIMICOS S.A.C.]]>" & vbCrLf &
                '                  "                </cbc:Name>" & vbCrLf &
                '                  "             </cac:PartyName>" & vbCrLf &
                '                  "          </cac:CarrierParty>" & vbCrLf



                'Dirección punto de partida
                'strXML = strXML + "       <cac:OriginAddress>" & vbCrLf &
                '                  "          <cbc:ID>" & Fila("cIdUbiGeoPartidaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                '                  "          <cbc:StreetName> <![CDATA[" & Fila("vDireccionPartidaCabeceraGuia") & "]]></cbc:StreetName>" & vbCrLf &
                '                  "       </cac:OriginAddress>" & vbCrLf
                strXML = strXML + "          <cac:Despatch>" & vbCrLf &
                              "             <cac:DespatchAddress>" & vbCrLf &
                              "                <cbc:ID schemeAgencyName=""PE:INEI"" schemeName=""Ubigeos"">" & Fila("cIdUbiGeoPartidaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                              "                <cac:AddressLine>" & vbCrLf &
                              "                   <cbc:Line>" & Fila("vDireccionPartidaCabeceraGuia") & "</cbc:Line>" & vbCrLf &
                              "                </cac:AddressLine>" & vbCrLf &
                              "             </cac:DespatchAddress>" & vbCrLf &
                              "          </cac:Despatch>" & vbCrLf &
                              "       </cac:Delivery>" & vbCrLf

                strXML = strXML + "       <cac:TransportHandlingUnit>" & vbCrLf &
                              "          <cac:TransportEquipment>" & vbCrLf &
                              "             <cbc:ID>" & Fila("vPlacaTransportistaCabeceraGuia") & "</cbc:ID>" & vbCrLf &
                              "          </cac:TransportEquipment>" & vbCrLf &
                              "       </cac:TransportHandlingUnit>" & vbCrLf


                strXML = strXML + "    </cac:Shipment>" & vbCrLf

                'Bienes a transportar
                For Each FilaDet In dt.Rows
                    strXML = strXML + "    <cac:DespatchLine>" & vbCrLf &
                                  "       <cbc:ID>" & FilaDet("nIdNumeroItemDetalleGuia") & "</cbc:ID>" & vbCrLf &
                                  "       <cbc:DeliveredQuantity unitCode=""" & FilaDet("cIdUnidadMedidaProductoDetalleGuia") & """ unitCodeListID=""UN/ECE rec 20"" unitCodeListAgencyName=""United Nations Economic Commission for Europe"">" & FilaDet("nCantidadProductoDetalleGuia") & "</cbc:DeliveredQuantity>" & vbCrLf &
                                  "       <cac:OrderLineReference>" & vbCrLf &
                                  "          <cbc:LineID>" & FilaDet("nIdNumeroItemDetalleGuia") & "</cbc:LineID>" & vbCrLf &
                                  "       </cac:OrderLineReference>" & vbCrLf &
                                  "       <cac:Item>" & vbCrLf &
                                  "          <cbc:Description>" & FilaDet("vDescripcionDetalleGuia") & "</cbc:Description>" & vbCrLf &
                                  "          <cac:SellersItemIdentification>" & vbCrLf &
                                  "          <cbc:ID>" & FilaDet("cIdProducto") & "</cbc:ID>" & vbCrLf &
                                  "          </cac:SellersItemIdentification>" & vbCrLf &
                                  "       </cac:Item>" & vbCrLf &
                                  "    </cac:DespatchLine>" & vbCrLf
                Next
                strXML = strXML + "</DespatchAdvice>"
            End If

            'Sirve para validar si es un archivo XML.
            doc.LoadXml(strXML)

            'Si el directorio no existe, crearlo
            If Not (Directory.Exists(Ruta)) Then Directory.CreateDirectory(Ruta)

            'Esto lo utilizo para quita el BOM
            Using writer = New XmlTextWriter(Ruta & "\" & stDocumento.NombreArchivo & ".xml", New UTF8Encoding(False))
                Dim xmlsettings As XmlWriterSettings = New XmlWriterSettings
                xmlsettings.OmitXmlDeclaration = False
                xmlsettings.ConformanceLevel = ConformanceLevel.Fragment
                xmlsettings = writer.Settings
                doc.Save(writer)
            End Using

            stDocumento.XML = strXML
            Return stDocumento
            cnx.Close()
        Catch ex As Exception
            'Return "" ' dt
            Throw New Exception(ex.Message)
        End Try
    End Function

    'Public Function ObtenerToken(stDocumento As stCPE, ByVal Ruta As String) As String  'As Boolean

    '    'Inicio: JMUG: 28/12/2022 - Obtener el Token para consumir el API
    '    Dim FuncionesMet As New clsFuncionesMetodos
    '    Dim TablaSistMet As New clsTablaSistemaMetodos
    '    ServicePointManager.Expect100Continue = True
    '    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

    '    Dim strUsrName = stDocumento.Ruc & FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "02", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema) '"20602706428JOHNUGFE" 
    '    Dim strPassword = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "03", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema) '"J0hnc1n2020" 
    '    'Dim grant_type As String = "password"
    '    Dim grant_type As String = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "07", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema) '"password"
    '    Dim client_id As String = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "08", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema) '"daaddbe7-3939-4617-8f71-ff8ef78338fe"
    '    Dim client_secret As String = FuncionesMet.Desencriptar(TablaSistMet.TablaSistemaListarPorId("44", "09", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema) '"Tj498AOXWDLerpz0YWUAvw=="
    '    'Dim sURL As String = "https://api-seguridad.sunat.gob.pe/v1/clientessol/" &
    '    '                     client_id & "/oauth2/token/"
    '    Dim sURL As String = Replace(TablaSistMet.TablaSistemaListarPorId("44", "06", "VTAS", stDocumento.IdEmpresa, "*").vValorOpcionalTablaSistema, "{client_id}", client_id)
    '    'Dim token As String = ""
    '    Dim client As RestClient = New RestClient(sURL)
    '    client.Timeout = -1
    '    Dim request = New RestRequest(Method.POST)
    '    request.AddHeader("Cache-Control", "no-cache")
    '    request.AddHeader("Content-Type", "application/json")
    '    request.AddBody("Content-Type", "application/x-www-form-urlencoded")
    '    request.AddParameter("grant_type", grant_type)
    '    request.AddParameter("client_id", client_id)
    '    request.AddParameter("client_secret", client_secret)
    '    request.AddParameter("username", strUsrName)
    '    request.AddParameter("password", strPassword)

    '    Dim result As IRestResponse = client.Execute(request)
    '    Dim statusCode As HttpStatusCode = result.StatusCode
    '    Dim numericStatusCode As Int16 = statusCode
    '    If (numericStatusCode = 200) Then
    '        Dim objRpta = JsonConvert.DeserializeObject(result.Content)
    '        Dim access_token As String = objRpta("access_token")
    '        ObtenerToken = access_token
    '    Else
    '        Throw New Exception("Error de conexión con Servicio. " + result.StatusDescription + "-" + result.ErrorMessage)
    '    End If
    '    'Final: JMUG: 28/12/2022 - Obtener el Token para consumir el API
    'End Function

    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)
        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next
        Return sb.ToString().ToLower
    End Function

    Public Function GenerarCPEXMLResumen(stDocumento As stCPE, ByVal Ruta As String) As stCPE ', ByVal NombreArchivo As String', ByVal FechaProceso As String) As stCPE  'As Boolean
        'CREADO PARA LA GENERACIÓN DEL ARCHIVO XML DE FACTURACIÓN ELECTRÓNICA DE SUNAT
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_VTAS_RPT_DOCUMENTOVENTARESUMENFE"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdTipoDocumento", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroSerieDocumento", SqlDbType.VarChar, 6))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroCorrelativoInicial", SqlDbType.VarChar, 20))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroCorrelativoFinal", SqlDbType.VarChar, 20))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cIdTipoDocumento").Value = stDocumento.IdTipoDocumento 'IdTipoDocumento
        cmd.Parameters("@vIdNumeroSerieDocumento").Value = stDocumento.IdNumeroSerieDocumento 'IdNumeroSerieDocumento
        cmd.Parameters("@vIdNumeroCorrelativoInicial").Value = stDocumento.IdNumeroCorrelativo 'IdNumeroCorrelativo
        cmd.Parameters("@vIdNumeroCorrelativoFinal").Value = stDocumento.IdNumeroCorrelativoFinal 'IdNumeroCorrelativo
        cmd.Parameters("@cIdPuntoVenta").Value = stDocumento.IdPuntoVenta
        cmd.Parameters("@cIdEmpresa").Value = stDocumento.IdEmpresa

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "DocumentoVentaResumen")
        'da.Fill(dt, "DocumentoVenta")

        'Dim strNombreArchivo As String = ""

        Try
            cnx.Open()
            '1Return True
            'As DataTable
            Dim strXML As String = ""
            Dim doc As New XmlDocument()

            Dim CabDocFila = ds.Tables("DocumentoVentaResumen").Rows(0)
            'strNombreArchivo = Fila("vRucEmpresa") & "-" & Fila("vIdEquivalenciaContableTipoDocumento") & "-" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento")
            stDocumento.Ruc = CabDocFila("vRucEmpresa")
            stDocumento.IdTipoDocumentoSunat = CabDocFila("vIdEquivalenciaContableTipoDocumento")

            dt = ds.Tables("DocumentoVentaResumen")
            'Cabecera del Documento XML
            strXML = strXML + "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf &
                              "<SummaryDocuments " & vbCrLf &
                              "   xmlns=""urn:sunat:names:specification:ubl:peru:schema:xsd:SummaryDocuments-1""" & vbCrLf &
                              "   xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""" & vbCrLf &
                              "   xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""" & vbCrLf &
                              "   xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""" & vbCrLf &
                              "   xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""" & vbCrLf &
                              "   xmlns:sac=""urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1""" & vbCrLf &
                              "   xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" & vbCrLf

            'Para registrar la firma
            strXML = strXML + "    <ext:UBLExtensions>" & vbCrLf &
                              "        <ext:UBLExtension>" & vbCrLf &
                              "            <ext:ExtensionContent>" & vbCrLf &
                              "            </ext:ExtensionContent>" & vbCrLf &
                              "        </ext:UBLExtension>" & vbCrLf &
                              "    </ext:UBLExtensions>" & vbCrLf

            'Datos Cabecera Documento - Versión UBL - Fecha y Hora de Emisión - Importe total en letras
            strXML = strXML + "    <cbc:UBLVersionID>2.0</cbc:UBLVersionID>" & vbCrLf &
                              "    <cbc:CustomizationID>1.1</cbc:CustomizationID>" & vbCrLf &
                              "    <cbc:ID>" & Mid(stDocumento.NombreArchivo, InStr(stDocumento.NombreArchivo, "-") + 1, Len(stDocumento.NombreArchivo)) & "</cbc:ID>" & vbCrLf &
                              "    <cbc:ReferenceDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(CabDocFila("dFechaEmisionCabeceraDocumento"))) & "</cbc:ReferenceDate>" & vbCrLf &
                              "    <cbc:IssueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(stDocumento.FechaProceso)) & "</cbc:IssueDate>" & vbCrLf

            'Datos del Emisor
            strXML = strXML + "    <cac:Signature>" & vbCrLf &
                              "        <cbc:ID>" & CabDocFila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                              "        <cac:SignatoryParty>" & vbCrLf &
                              "            <cac:PartyIdentification>" & vbCrLf &
                              "                <cbc:ID>" & CabDocFila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                              "            </cac:PartyIdentification>" & vbCrLf &
                              "            <cac:PartyName>" & vbCrLf &
                              "                <cbc:Name><![CDATA[" & CabDocFila("vRazonSocialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                              "            </cac:PartyName>" & vbCrLf &
                              "        </cac:SignatoryParty>" & vbCrLf &
                              "        <cac:DigitalSignatureAttachment>" & vbCrLf &
                              "            <cac:ExternalReference>" & vbCrLf &
                              "                <cbc:URI>" & CabDocFila("vRucEmpresa") & "</cbc:URI>" & vbCrLf &
                              "            </cac:ExternalReference>" & vbCrLf &
                              "        </cac:DigitalSignatureAttachment>" & vbCrLf &
                              "    </cac:Signature>" & vbCrLf

            'Tipo de Documento y Razón Social del Emisor (Empresa)
            strXML = strXML + "    <cac:AccountingSupplierParty>" & vbCrLf &
                              "        <cbc:CustomerAssignedAccountID>" & CabDocFila("vRucEmpresa") & "</cbc:CustomerAssignedAccountID>" & vbCrLf &
                              "        <cbc:AdditionalAccountID>" & CabDocFila("cIdTipoDocumentoEmpresa") & "</cbc:AdditionalAccountID>" & vbCrLf &
                              "        <cac:Party>" & vbCrLf &
                              "            <cac:PartyLegalEntity>" & vbCrLf &
                              "                <cbc:RegistrationName><![CDATA[" & CabDocFila("vRazonSocialEmpresa") & "]]></cbc:RegistrationName>" & vbCrLf &
                              "            </cac:PartyLegalEntity>" & vbCrLf &
                              "        </cac:Party>" & vbCrLf &
                              "    </cac:AccountingSupplierParty>" & vbCrLf

            Dim intX As Int64 = 0
            For Each Fila In dt.Rows
                'Nro. Serie, Nro. Documento y Tipo de Documento
                intX += 1
                strXML = strXML + "      <sac:SummaryDocumentsLine>" & vbCrLf &
                                  "         <cbc:LineID>" & intX & "</cbc:LineID>" & vbCrLf &
                                  "         <cbc:DocumentTypeCode>" & Fila("vIdEquivalenciaContableTipoDocumento") & "</cbc:DocumentTypeCode>" & vbCrLf &
                                  "         <cbc:ID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</cbc:ID>" & vbCrLf

                'Datos Generales de la Boleta
                strXML = strXML + "         <cac:AccountingCustomerParty>" & vbCrLf &
                                  "             <cbc:CustomerAssignedAccountID>" & Fila("vNumeroDocumentoClienteCabeceraDocumento") & "</cbc:CustomerAssignedAccountID>" & vbCrLf &
                                  "             <cbc:AdditionalAccountID>" & Fila("cIdTipoDocumentoCliente") & "</cbc:AdditionalAccountID>" & vbCrLf &
                                  "         </cac:AccountingCustomerParty>"

                'Documento de Referencia si es Nota de Crédito o Débito
                If Trim(Fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento")) <> "" Then
                    strXML = strXML + "         <cac:BillingReference>" & vbCrLf &
                                  "            <cac:InvoiceDocumentReference>" & vbCrLf &
                                  "               <cbc:ID>" & Fila("vIdNumeroSerieDocumentoReferencialCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoReferencialCabeceraDocumento") & "</cbc:ID>" & vbCrLf &
                                  "               <cbc:DocumentTypeCode>" & Fila("vIdEquivalenciaSunatReferencialTipoDocumento") & "</cbc:DocumentTypeCode>" & vbCrLf &
                                  "            </cac:InvoiceDocumentReference>" & vbCrLf &
                                  "         </cac:BillingReference>"
                End If
                strXML = strXML + "         <cac:Status>" & vbCrLf &
                                  "             <cbc:ConditionCode>" & stDocumento.IdCondicion & "</cbc:ConditionCode>" & vbCrLf &
                                  "         </cac:Status>" & vbCrLf &
                                  "         <sac:TotalAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalPrecioVentaCabeceraDocumento")), 2) & "</sac:TotalAmount>" & vbCrLf

                'Total valor de venta - operaciones gravadas
                If Math.Round(Convert.ToDecimal(Fila("nTotalMontoGravadoObligatorio")), 2) > 0 Then
                    strXML = strXML + "         <sac:BillingPayment>" & vbCrLf &
                                  "             <cbc:PaidAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoGravadoObligatorio")), 2) & "</cbc:PaidAmount>" & vbCrLf &
                                  "             <cbc:InstructionID>01</cbc:InstructionID>" & vbCrLf &
                                  "         </sac:BillingPayment>" & vbCrLf
                End If

                'Total valor de venta - operaciones exoneradas
                If Math.Round(Convert.ToDecimal(Fila("nTotalMontoExoneradoObligatorio")), 2) > 0 Then
                    strXML = strXML + "         <sac:BillingPayment>" & vbCrLf &
                                  "             <cbc:PaidAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoExoneradoObligatorio")), 2) & "</cbc:PaidAmount>" & vbCrLf &
                                  "             <cbc:InstructionID>02</cbc:InstructionID>" & vbCrLf &
                                  "         </sac:BillingPayment>" & vbCrLf
                End If

                'Total valor de venta - operaciones inafectas
                If Math.Round(Convert.ToDecimal(Fila("nTotalMontoInafectoObligatorio")), 2) > 0 Then
                    strXML = strXML + "         <sac:BillingPayment>" & vbCrLf &
                                  "             <cbc:PaidAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoInafectoObligatorio")), 2) & "</cbc:PaidAmount>" & vbCrLf &
                                  "             <cbc:InstructionID>03</cbc:InstructionID>" & vbCrLf &
                                  "         </sac:BillingPayment>" & vbCrLf
                End If

                'Total valor de venta - operaciones exportaciones
                If Math.Round(Convert.ToDecimal("0.00"), 2) > 0 Then
                    strXML = strXML + "         <sac:BillingPayment>" & vbCrLf &
                                  "             <cbc:PaidAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal("0.00"), 2) & "</cbc:PaidAmount>" & vbCrLf &
                                  "             <cbc:InstructionID>04</cbc:InstructionID>" & vbCrLf &
                                  "         </sac:BillingPayment>" & vbCrLf
                End If

                'Total valor de venta - operaciones gratuitas
                If Math.Round(Convert.ToDecimal(Fila("nTotalMontoGratuitaAdicional")), 2) > 0 Then
                    strXML = strXML + "         <sac:BillingPayment>" & vbCrLf &
                                  "             <cbc:PaidAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalMontoGratuitaAdicional")), 2) & "</cbc:PaidAmount>" & vbCrLf &
                                  "             <cbc:InstructionID>05</cbc:InstructionID>" & vbCrLf &
                                  "         </sac:BillingPayment>" & vbCrLf
                End If

                'IGV
                'If Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) > 0 Then
                strXML = strXML + "         <cac:TaxTotal>" & vbCrLf &
                                  "             <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "             <cac:TaxSubtotal>" & vbCrLf &
                                  "                 <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalIGVCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "                 <cac:TaxCategory>" & vbCrLf &
                                  "                     <cac:TaxScheme>" & vbCrLf &
                                  "                         <cbc:ID>1000</cbc:ID>" & vbCrLf &
                                  "                         <cbc:Name>IGV</cbc:Name>" & vbCrLf &
                                  "                         <cbc:TaxTypeCode>VAT</cbc:TaxTypeCode>" & vbCrLf &
                                  "                     </cac:TaxScheme>" & vbCrLf &
                                  "                 </cac:TaxCategory>" & vbCrLf &
                                  "             </cac:TaxSubtotal>" & vbCrLf &
                                  "         </cac:TaxTotal>" & vbCrLf
                'End If

                'ISC
                If Math.Round(Convert.ToDecimal(Fila("nTotalISCCabeceraDocumento")), 2) > 0 Then
                    strXML = strXML + "         <cac:TaxTotal>" & vbCrLf &
                                  "             <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalISCCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "             <cac:TaxSubtotal>" & vbCrLf &
                                  "                 <cbc:TaxAmount currencyID=""" & Fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & """>" & Math.Round(Convert.ToDecimal(Fila("nTotalISCCabeceraDocumento")), 2) & "</cbc:TaxAmount>" & vbCrLf &
                                  "                 <cac:TaxCategory>" & vbCrLf &
                                  "                     <cac:TaxScheme>" & vbCrLf &
                                  "                         <cbc:ID>2000</cbc:ID>" & vbCrLf &
                                  "                         <cbc:Name>ISC</cbc:Name>" & vbCrLf &
                                  "                         <cbc:TaxTypeCode>EXC</cbc:TaxTypeCode>" & vbCrLf &
                                  "                     </cac:TaxScheme>" & vbCrLf &
                                  "                 </cac:TaxCategory>" & vbCrLf &
                                  "             </cac:TaxSubtotal>" & vbCrLf &
                                  "         </cac:TaxTotal>" & vbCrLf
                End If
                strXML = strXML + "     </sac:SummaryDocumentsLine>" & vbCrLf
            Next
            'Final: Detalle de los Items**********************************************************************

            'Fin del Resumen
            strXML = strXML + "</SummaryDocuments>"

            doc.LoadXml(strXML)

            'Si el directorio no existe, crearlo
            If Not (Directory.Exists(Ruta)) Then Directory.CreateDirectory(Ruta)

            doc.Save(Ruta & "\" & stDocumento.NombreArchivo & ".xml")

            stDocumento.XML = strXML
            Return stDocumento
            cnx.Close()
        Catch ex As Exception
            'Return "" ' dt
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GenerarCPEXMLBaja(stDocumento As stCPE, ByVal Ruta As String) As stCPE ', ByVal NombreArchivo As String', ByVal FechaProceso As String) As stCPE  'As Boolean
        'CREADO PARA LA GENERACIÓN DEL ARCHIVO XML DE BAJA FACTURACIÓN ELECTRÓNICA DE SUNAT
        Dim MiConexion As New clsConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable

        'Se Configura el comando
        cmd.Connection = cnx
        cmd.CommandTimeout = 15000
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "PA_VTAS_RPT_DOCUMENTOBAJAFE"

        'Se crea el objeto Parameters por cada parametro
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdTipoDocumento", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroSerieDocumento", SqlDbType.VarChar, 6))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@vIdNumeroCorrelativo", SqlDbType.VarChar, 20))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdPuntoVenta", SqlDbType.Char, 2))
        cmd.Parameters.Add(New SqlClient.SqlParameter("@cIdEmpresa", SqlDbType.Char, 2))

        'Se establece los valores por cada parametro
        cmd.Parameters("@cIdTipoDocumento").Value = stDocumento.IdTipoDocumento 'IdTipoDocumento
        cmd.Parameters("@vIdNumeroSerieDocumento").Value = stDocumento.IdNumeroSerieDocumento 'IdNumeroSerieDocumento
        cmd.Parameters("@vIdNumeroCorrelativo").Value = stDocumento.IdNumeroCorrelativo 'IdNumeroCorrelativo
        cmd.Parameters("@cIdPuntoVenta").Value = stDocumento.IdPuntoVenta
        cmd.Parameters("@cIdEmpresa").Value = stDocumento.IdEmpresa

        'Se configura el Adaptador
        da.SelectCommand = cmd
        da.Fill(ds, "DocumentoBaja")
        'da.Fill(dt, "DocumentoVenta")

        'Dim strNombreArchivo As String = ""

        Try
            cnx.Open()
            '1Return True
            'As DataTable
            Dim strXML As String = ""
            Dim doc As New XmlDocument()

            Dim CabDocFila = ds.Tables("DocumentoBaja").Rows(0)
            'strNombreArchivo = Fila("vRucEmpresa") & "-" & Fila("vIdEquivalenciaContableTipoDocumento") & "-" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "-" & Fila("vIdNumeroCorrelativoCabeceraDocumento")
            stDocumento.Ruc = CabDocFila("vRucEmpresa")
            stDocumento.IdTipoDocumentoSunat = CabDocFila("vIdEquivalenciaContableTipoDocumento")

            dt = ds.Tables("DocumentoBaja")
            'Cabecera del Documento XML
            strXML = strXML + "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf &
                              "<VoidedDocuments " & vbCrLf &
                              "   xmlns=""urn:sunat:names:specification:ubl:peru:schema:xsd:VoidedDocuments-1""" & vbCrLf &
                              "   xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""" & vbCrLf &
                              "   xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""" & vbCrLf &
                              "   xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""" & vbCrLf &
                              "   xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""" & vbCrLf &
                              "   xmlns:sac=""urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1""" & vbCrLf &
                              "   xmlns:xsi =""http://www.w3.org/2001/XMLSchema-instance"">" & vbCrLf

            'Para registrar la firma
            strXML = strXML + "    <ext:UBLExtensions>" & vbCrLf &
                              "        <ext:UBLExtension>" & vbCrLf &
                              "            <ext:ExtensionContent>" & vbCrLf &
                              "            </ext:ExtensionContent>" & vbCrLf &
                              "        </ext:UBLExtension>" & vbCrLf &
                              "    </ext:UBLExtensions>" & vbCrLf

            'Datos Cabecera Documento - Versión UBL - Fecha y Hora de Emisión - Importe total en letras
            strXML = strXML + "    <cbc:UBLVersionID>2.0</cbc:UBLVersionID>" & vbCrLf &
                              "    <cbc:CustomizationID>1.0</cbc:CustomizationID>" & vbCrLf &
                              "    <cbc:ID>" & Mid(stDocumento.NombreArchivo, InStr(stDocumento.NombreArchivo, "-") + 1, Len(stDocumento.NombreArchivo)) & "</cbc:ID>" & vbCrLf &
                              "    <cbc:ReferenceDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(CabDocFila("dFechaEmisionCabeceraDocumento"))) & "</cbc:ReferenceDate>" & vbCrLf &
                              "    <cbc:IssueDate>" & String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(stDocumento.FechaProceso)) & "</cbc:IssueDate>" & vbCrLf

            'Datos del Emisor
            strXML = strXML + "    <cac:Signature>" & vbCrLf &
                              "        <cbc:ID>" & CabDocFila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                              "        <cac:SignatoryParty>" & vbCrLf &
                              "            <cac:PartyIdentification>" & vbCrLf &
                              "                <cbc:ID>" & CabDocFila("vRucEmpresa") & "</cbc:ID>" & vbCrLf &
                              "            </cac:PartyIdentification>" & vbCrLf &
                              "            <cac:PartyName>" & vbCrLf &
                              "                <cbc:Name><![CDATA[" & CabDocFila("vRazonSocialEmpresa") & "]]></cbc:Name>" & vbCrLf &
                              "            </cac:PartyName>" & vbCrLf &
                              "        </cac:SignatoryParty>" & vbCrLf &
                              "        <cac:DigitalSignatureAttachment>" & vbCrLf &
                              "            <cac:ExternalReference>" & vbCrLf &
                              "                <cbc:URI>" & CabDocFila("vRucEmpresa") & "</cbc:URI>" & vbCrLf &
                              "            </cac:ExternalReference>" & vbCrLf &
                              "        </cac:DigitalSignatureAttachment>" & vbCrLf &
                              "    </cac:Signature>" & vbCrLf

            'Tipo de Documento y Razón Social del Emisor (Empresa)
            strXML = strXML + "    <cac:AccountingSupplierParty>" & vbCrLf &
                              "        <cbc:CustomerAssignedAccountID>" & CabDocFila("vRucEmpresa") & "</cbc:CustomerAssignedAccountID>" & vbCrLf &
                              "        <cbc:AdditionalAccountID>" & CabDocFila("cIdTipoDocumentoEmpresa") & "</cbc:AdditionalAccountID>" & vbCrLf &
                              "        <cac:Party>" & vbCrLf &
                              "            <cac:PartyLegalEntity>" & vbCrLf &
                              "                <cbc:RegistrationName><![CDATA[" & CabDocFila("vRazonSocialEmpresa") & "]]></cbc:RegistrationName>" & vbCrLf &
                              "            </cac:PartyLegalEntity>" & vbCrLf &
                              "        </cac:Party>" & vbCrLf &
                              "    </cac:AccountingSupplierParty>" & vbCrLf

            Dim intX As Int64 = 0
            For Each Fila In dt.Rows
                'Nro. Serie, Nro. Documento y Tipo de Documento
                intX += 1
                strXML = strXML + "      <sac:VoidedDocumentsLine>" & vbCrLf &
                                  "         <cbc:LineID>" & intX & "</cbc:LineID>" & vbCrLf &
                                  "         <cbc:DocumentTypeCode>" & Fila("vIdEquivalenciaContableTipoDocumento") & "</cbc:DocumentTypeCode>" & vbCrLf &
                                  "            <sac:DocumentSerialID>" & Fila("vIdNumeroSerieDocumentoCabeceraDocumento") & "</sac:DocumentSerialID>" & vbCrLf &
                                  "            <sac:DocumentNumberID>" & Fila("vIdNumeroCorrelativoCabeceraDocumento") & "</sac:DocumentNumberID>" & vbCrLf &
                                  "            <sac:VoidReasonDescription>" & stDocumento.MotivoBaja & "</sac:VoidReasonDescription>" & vbCrLf &
                                  "      </sac:VoidedDocumentsLine>" & vbCrLf
            Next
            'Final: Detalle de los Items**********************************************************************

            'Fin del Resumen
            strXML = strXML + "</VoidedDocuments>"

            doc.LoadXml(strXML)

            'Si el directorio no existe, crearlo
            If Not (Directory.Exists(Ruta)) Then Directory.CreateDirectory(Ruta)

            doc.Save(Ruta & "\" & stDocumento.NombreArchivo & ".xml")

            stDocumento.XML = strXML
            Return stDocumento
            cnx.Close()
        Catch ex As Exception
            'Return "" ' dt
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function SunatMigrarRegistroComprasTXT(ByVal IdPeriodo As String, ByVal IdMes As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, Ruta As String) As String

        Dim strNombreArchivo = ""
        Dim fichero As String = ""
        Dim texto As String = ""

        Dim strSQL As String = "EXEC PA_CTBL_RPT_REGISTROCOMPRA '" & IdPeriodo & "', '" & IdMes & "', '" &
                               "" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & IdTipoMoneda & "', '" & Orden & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")
        'O - Indicador de operaciones.
        'I - Indicador del contenido del libro o registro.
        'M - Indicador de la moneda utilizada.
        'G - Indicador de libro electrónico generado por el PLE.
        Dim O, I, M, G As String
        O = "1"
        I = "0"
        M = IIf(IdTipoMoneda = "S", "1", "2")
        G = "1"
        If ds.Tables("Reporte").Rows.Count > 0 Then
            I = "1"
            Dim fila As DataRow
            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & ds.Tables("Reporte").Rows(0).Item("cIdPeriodoAsiento") & ds.Tables("Reporte").Rows(0).Item("cIdMesAsiento") &
                               "0008010000" & O & I & M & G
            '"00080100001111"
            'Dim fichero As String = "C:\SIW_NEW\slnSIW\CTBL\Downloads\" & strNombreArchivo & ".txt"
            fichero = Ruta & strNombreArchivo & ".txt"

            'Const fichero As String = "C:\inetpub\wwwroot\SIW_NEW\slnSIW\CTBL\Downloads\Ingresos.txt"

            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("Reporte").Rows
                Dim dFechaInicial As Date
                Dim dFechaFinal As Date
                Dim nDiferencia As Integer
                dFechaInicial = Convert.ToDateTime("01/" & fila("cIdMesAsiento") & "/" & fila("cIdPeriodoAsiento"))
                dFechaFinal = Convert.ToDateTime("01/" & String.Format("{0:00}", Month(fila("dFechaDocumentoAsiento"))) & "/" & Year(fila("dFechaDocumentoAsiento")))
                nDiferencia = DateDiff("d", dFechaInicial, dFechaFinal)

                texto = fila("cIdPeriodoAsiento") & fila("cIdMesAsiento") & "00|" & fila("cIdTipoLibro") & Trim(fila("cNumeroAsiento")) & "|" &
                        Trim(fila("cNumeroLineaAsiento")) & "|" & fila("dFechaDocumentoAsiento") & "|" & fila("dFechaVencimientoAsiento") & "|" &
                        fila("vIdEquivalenciaContableTipoDocumento") & "|" & IIf(fila("vIdNumeroSerieDocumentoAsiento").ToString.Trim = "", "", (fila("vIdNumeroSerieDocumentoAsiento").ToString.Trim)) & "|" & "" & "|" & fila("vIdNumeroDocumentoAsiento") & "|" &
                        "" & "|" & IIf(fila("cIdClienteProveedor").ToString.Trim = "00000000000", "", Convert.ToInt16(fila("vTipoDocumentoClienteProveedor"))) & "|" & IIf(fila("cIdClienteProveedor").ToString.Trim = "00000000000", "", fila("cIdClienteProveedor").ToString.Trim) & "|" & SunatConversionCaracteresEspeciales(fila("vRazonSocialAsiento")) & "|" &
                        fila("nBaseImponibleConDerecho") & "|" & fila("nIGVConDerecho") & "|" & "" & "|" & "" & "|" & fila("nBaseImponibleSinDerecho") & "|" & fila("nIGVSinDerecho") & "|" &
                        fila("nCompraNoGravada") & "|" & "|" & "|" & fila("nTotalCompra") & "|" & fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "|" &
                        Mid(fila("nTipoCambioAsiento"), 1, 5) & "|" & fila("dFechaDocumentoRefAsiento") & "|" &
                        IIf(fila("cIdTipoDocumentoRefAsiento").ToString.Trim = "00", "", fila("cIdTipoDocumentoRefAsiento").ToString.Trim) & "|" & fila("vIdNumeroSerieDocumentoRefAsiento").ToString.Trim & "|" & "" & "|" & IIf(fila("cIdTipoDocumentoRefAsiento").ToString.Trim = "00", "", fila("vIdNumeroDocumentoRefAsiento").ToString.Trim) & "|" &
                        fila("dFechaPagoAsiento") & "|" & fila("cNumeroPagoAsiento").ToString.Trim & "|" & "" & "|||||||" &
                        IIf(fila("vIdEquivalenciaContableTipoDocumento") = "07" Or fila("vIdEquivalenciaContableTipoDocumento") = "08", "", "1") & "|" &
                        fila("cIdIndicadorLibroElectronicoAsiento") & "|"
                sw.WriteLine(texto)
            Next
            sw.Close()
            SunatMigrarRegistroComprasTXT = strNombreArchivo & ".txt"
        Else
            Dim EmpData As New clsEmpresaMetodos
            Dim Empresa As GNRL_EMPRESA = EmpData.EmpresaListarPorId(IdEmpresa)
            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & ds.Tables("Reporte").Rows(0).Item("cIdPeriodoAsiento") & ds.Tables("Reporte").Rows(0).Item("cIdMesAsiento") &
                               "0008010000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            If ds.Tables("Reporte").Rows.Count < 0 Then
                texto = ""
                sw.WriteLine(texto)
            End If
            sw.Close()
            SunatMigrarRegistroComprasTXT = strNombreArchivo & ".txt"
        End If
    End Function


    Public Function SunatMigrarRegistroComprasNoDomiciliadosTXT(ByVal IdPeriodo As String, ByVal IdMes As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, Ruta As String) As String

        Dim strNombreArchivo = ""
        Dim fichero As String = ""
        Dim texto As String = ""

        Dim strSQL As String = "EXEC PA_CTBL_RPT_REGISTROCOMPRA '" & IdPeriodo & "', '" & IdMes & "', '" &
                               "" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & IdTipoMoneda & "', '" & Orden & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")
        'O - Indicador de operaciones.
        'I - Indicador del contenido del libro o registro.
        'M - Indicador de la moneda utilizada.
        'G - Indicador de libro electrónico generado por el PLE.
        Dim O, I, M, G As String
        O = "1"
        I = "0"
        M = IIf(IdTipoMoneda = "S", "1", "2")
        G = "1"
        If ds.Tables("Reporte").Rows.Count > 0 Then
            I = "1"
            Dim fila As DataRow
            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & ds.Tables("Reporte").Rows(0).Item("cIdPeriodoAsiento") & ds.Tables("Reporte").Rows(0).Item("cIdMesAsiento") &
                               "0008020000" & O & I & M & G
            '"00080100001111"
            'Dim fichero As String = "C:\SIW_NEW\slnSIW\CTBL\Downloads\" & strNombreArchivo & ".txt"
            fichero = Ruta & strNombreArchivo & ".txt"

            'Const fichero As String = "C:\inetpub\wwwroot\SIW_NEW\slnSIW\CTBL\Downloads\Ingresos.txt"

            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("Reporte").Rows
                Dim dFechaInicial As Date
                Dim dFechaFinal As Date
                Dim nDiferencia As Integer
                dFechaInicial = Convert.ToDateTime("01/" & fila("cIdMesAsiento") & "/" & fila("cIdPeriodoAsiento"))
                dFechaFinal = Convert.ToDateTime("01/" & String.Format("{0:00}", Month(fila("dFechaDocumentoAsiento"))) & "/" & Year(fila("dFechaDocumentoAsiento")))
                nDiferencia = DateDiff("d", dFechaInicial, dFechaFinal)
                texto = ""
                sw.WriteLine(texto)
            Next
            sw.Close()
            SunatMigrarRegistroComprasNoDomiciliadosTXT = strNombreArchivo & ".txt"
        Else
            Dim EmpData As New clsEmpresaMetodos
            Dim Empresa As GNRL_EMPRESA = EmpData.EmpresaListarPorId(IdEmpresa)
            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & ds.Tables("Reporte").Rows(0).Item("cIdPeriodoAsiento") & ds.Tables("Reporte").Rows(0).Item("cIdMesAsiento") &
                               "0008020000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            If ds.Tables("Reporte").Rows.Count < 0 Then
                texto = ""
                sw.WriteLine(texto)
            End If
            sw.Close()
            SunatMigrarRegistroComprasNoDomiciliadosTXT = strNombreArchivo & ".txt"
        End If
    End Function

    Public Function SunatGetData(strQuery As String) As DataTable
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

    Public Function SunatMigrarLibroDiarioTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal IdTipoLibro As String, ByVal Orden As String, Ruta As String) As String

        Dim strNombreArchivo = ""
        Dim fichero As String = ""
        Dim texto As String = ""

        Dim strSQL As String = "EXEC PA_CTBL_RPT_LIBRODIARIO '" & IdPeriodo & "', '" & IdMesInicial & "', '" & IdMesFinal & "', '" &
                               "" & IdPuntoVenta & "', '" & IdEmpresa & "', '" & IdTipoMoneda & "', '" & IdTipoLibro & "', '" & Orden & "'"

        Dim MiConexion As New clsSunatConexionDAO
        Dim cnx As New SqlConnection(MiConexion.GetCnx)

        Dim cmd As New SqlCommand(strSQL, cnx)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet

        da.Fill(ds, "Reporte")
        'O - Indicador de operaciones.
        'I - Indicador del contenido del libro o registro.
        'M - Indicador de la moneda utilizada.
        'G - Indicador de libro electrónico generado por el PLE.
        Dim O, I, M, G As String
        O = "1"
        I = "0"
        M = IIf(IdTipoMoneda = "S", "1", "2")
        G = "1"
        If ds.Tables("Reporte").Rows.Count > 0 Then
            I = "1"
            Dim fila As DataRow
            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & ds.Tables("Reporte").Rows(0).Item("cIdPeriodoAsiento") & ds.Tables("Reporte").Rows(0).Item("cIdMesAsiento") &
                               "0005010000" & O & I & M & G
            fichero = Ruta & strNombreArchivo & ".txt"

            Dim sw As New System.IO.StreamWriter(fichero)
            For Each fila In ds.Tables("Reporte").Rows
                Dim dFechaInicial As Date
                Dim dFechaFinal As Date
                Dim nDiferencia As Integer
                dFechaInicial = Convert.ToDateTime("01/" & fila("cIdMesAsiento") & "/" & fila("cIdPeriodoAsiento"))
                dFechaFinal = Convert.ToDateTime("01/" & String.Format("{0:00}", Month(fila("dFechaDocumentoAsiento"))) & "/" & Year(fila("dFechaDocumentoAsiento")))
                nDiferencia = DateDiff("d", dFechaInicial, dFechaFinal)
                Dim strDatoEstructurado As String = ""
                strDatoEstructurado = IIf(fila("cIdTipoLibro") = "03", "140100", IIf(fila("cIdTipoLibro") = "02", IIf(fila("cIdDomiciliado").ToString = "1", "080100", "080200"), ""))
                strDatoEstructurado = IIf(strDatoEstructurado = "", "", strDatoEstructurado & "&" & fila("cIdPeriodoAsiento") & fila("cIdMesAsiento") & "00" & fila("cIdTipoLibro") & Trim(fila("cNumeroAsiento")) & "&" & Trim(fila("cNumeroLineaAsiento")))
                texto = fila("cIdPeriodoAsiento") & fila("cIdMesAsiento") & "00|" & fila("cIdTipoLibro") & Trim(fila("cNumeroAsiento")) & "|" &
                        Trim(fila("cNumeroLineaAsiento")) & "|" & fila("cIdNumeroCuentaAsiento") & "|" & fila("cIdGrupo") & IIf(fila("cIdPartidaPresupuestoAsiento").ToString.Trim = "", "", "&" & fila("cIdPartidaPresupuestoAsiento")) & "|" &
                        fila("cIdCentroCostoAsiento") & "|" & fila("vIdEquivalenciaContableAbreviadaTipoMoneda") & "|" & fila("cIdTipoDocumentoEmpresa") & "|" & fila("vRucEmpresa") & "|" & fila("vIdEquivalenciaContableTipoDocumento") & "|" &
                        fila("vIdNumeroSerieDocumentoAsiento") & "|" & fila("vIdNumeroDocumentoAsiento") & "|" & dFechaFinal & "|" & fila("dFechaVencimientoAsiento") & "|" & fila("dFechaDocumentoAsiento") & "|" &
                        fila("vGlosaAsiento") & "|" & "|" & fila("nDebe") & "|" & fila("nHaber") & "|" & strDatoEstructurado & "|" &
                        fila("cIdIndicadorLibroElectronicoAsiento") & "|"

                sw.WriteLine(texto)
            Next
            sw.Close()
            SunatMigrarLibroDiarioTXT = strNombreArchivo & ".txt"
        Else
            Dim EmpData As New clsEmpresaMetodos
            Dim Empresa As GNRL_EMPRESA = EmpData.EmpresaListarPorId(IdEmpresa)
            strNombreArchivo = "LE" & ds.Tables("Reporte").Rows(0).Item("vRucEmpresa") & ds.Tables("Reporte").Rows(0).Item("cIdPeriodoAsiento") & ds.Tables("Reporte").Rows(0).Item("cIdMesAsiento") &
                               "0005010000" & O & I & M & G
            '"00080100001111"
            fichero = Ruta & strNombreArchivo & ".txt"
            Dim sw As New System.IO.StreamWriter(fichero)
            If ds.Tables("Reporte").Rows.Count < 0 Then
                texto = ""
                sw.WriteLine(texto)
            End If
            sw.Close()
            SunatMigrarLibroDiarioTXT = strNombreArchivo & ".txt"
        End If
    End Function

    Public Function SunatLoadConsultaDNI(ByVal strDNI As String) As stSunatDNI
        'Try
        '    SunatLoadConsultaDNI.RazonSocial = ""
        '    SunatLoadConsultaDNI.Dni = ""
        '    SunatLoadConsultaDNI.Departamento = ""
        '    SunatLoadConsultaDNI.Provincia = ""
        '    SunatLoadConsultaDNI.Distrito = ""
        '    SunatLoadConsultaDNI.GrupoVotacion = ""

        '    Dim urlReniec As String = String.Format("http://clientes.reniec.gob.pe/padronElectoral2012/consulta.htm?hTipo=2&hDni={0}&fbclid=IwAR3SLRJL_PSOd1M_VFHMi4onuv5ANYJ2Tk2mrP0-gOEp03cnmfleFUURs5U", strDNI)
        '    Dim objEncoding = Encoding.GetEncoding("ISO-8859-1")
        '    Dim objCookies = New CookieCollection

        '    'Usando GET
        '    Dim getRequest As HttpWebRequest = CType(WebRequest.Create(urlReniec), HttpWebRequest)
        '    getRequest.Credentials = CredentialCache.DefaultNetworkCredentials
        '    getRequest.ProtocolVersion = HttpVersion.Version11
        '    getRequest.UserAgent = ".NET Framework 4.0"
        '    getRequest.Method = "GET"

        '    getRequest.CookieContainer = New CookieContainer()
        '    getRequest.CookieContainer.Add(objCookies)

        '    'Como se puede ver usamos el Httpwebrequest para realizar la petición a la web 
        '    'y con esto deberíamos obtener una respuesta que se cargara en el Httpwebresponse que se muestra continuación.
        '    Dim sGetResponse = String.Empty

        '    Using getResponse As HttpWebResponse = CType(getRequest.GetResponse(), HttpWebResponse)
        '        objCookies = getResponse.Cookies
        '        Using srGetResponse = New StreamReader(getResponse.GetResponseStream(), objEncoding)
        '            sGetResponse = srGetResponse.ReadToEnd
        '        End Using
        '    End Using

        '    'Obtenemos Información
        '    Dim document = New HtmlAgilityPack.HtmlDocument
        '    document.LoadHtml(sGetResponse)
        '    Dim strCadena As String = "//table //table //table //table //tr //td //table //tr"
        '    Dim NodesTr As HtmlAgilityPack.HtmlNodeCollection = document.DocumentNode.SelectNodes(strCadena)
        '    If (Not IsNothing(NodesTr)) Then
        '        For Each Node In NodesTr
        '            Dim strTipoCampo As String = ""
        '            For Each subNode In Node.Elements("td")
        '                Dim sValue As String = subNode.InnerHtml.ToString.Trim
        '                If strTipoCampo = "RazonSocial" Then
        '                    SunatLoadConsultaDNI.RazonSocial = sValue
        '                ElseIf strTipoCampo = "DNI" Then
        '                    SunatLoadConsultaDNI.Dni = sValue
        '                ElseIf strTipoCampo = "GrupoVotacion" Then
        '                    SunatLoadConsultaDNI.GrupoVotacion = sValue
        '                ElseIf strTipoCampo = "Departamento" Then
        '                    SunatLoadConsultaDNI.Departamento = sValue
        '                ElseIf strTipoCampo = "Provincia" Then
        '                    SunatLoadConsultaDNI.Provincia = sValue
        '                ElseIf strTipoCampo = "Distrito" Then
        '                    SunatLoadConsultaDNI.Distrito = sValue
        '                End If

        '                If sValue = "Apellidos y Nombres" Then
        '                    strTipoCampo = "RazonSocial"
        '                ElseIf sValue = "DNI" Then
        '                    strTipoCampo = "DNI"
        '                ElseIf sValue = "Grupo de VotaciÃ³n" Then
        '                    strTipoCampo = "GrupoVotacion"
        '                ElseIf InStr(sValue, "Departamento") <> 0 Then
        '                    strTipoCampo = "Departamento"
        '                ElseIf InStr(sValue, "Provincia") <> 0 Then
        '                    strTipoCampo = "Provincia"
        '                ElseIf InStr(sValue, "Distrito") <> 0 Then
        '                    strTipoCampo = "Distrito"
        '                Else
        '                    strTipoCampo = ""
        '                End If

        '            Next
        '        Next
        '    End If
        'Catch ex As Exception
        'End Try
    End Function

End Class