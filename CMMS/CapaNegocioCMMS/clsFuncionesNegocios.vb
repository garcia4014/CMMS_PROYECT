Imports CapaDatosCMMS
'Imports System.Transactions

'Para adjuntar imagen redimensionando el tamaño
'necesitamos las siguientes librerias.
'Inicio
Imports System.Web 'Lo adicioné al proyecto.
'Fin

Public Class clsFuncionesNegocios
    Dim FuncionesMet As New clsFuncionesMetodos

    Public Function FuncionesGetData(strQuery As String) As DataTable
        Return FuncionesMet.FuncionesGetData(strQuery)
    End Function

    Public Function GetDSJSon(ByVal dtOrigen As System.Data.DataTable, ByVal strNombreTabla As String) As String
        Return FuncionesMet.GetDSJSon(dtOrigen, strNombreTabla)
    End Function

    Public Function TamanoCarpeta(ByVal strCarpeta As String) As Long
        Return FuncionesMet.TamanoCarpeta(strCarpeta) 'Retorna en MB
    End Function

    Public Function LogAuditoriaListarAcceso(ByVal IdUsuario As String, ByVal IdPais As String, ByVal IdEmpresa As String,
                                             ByVal IdLocal As String, ByVal IdPuntoVenta As String, ByVal IdSistema As String,
                                             ByVal IdModulo As String) As GNRL_LOGAUDITORIA
        'If FuncionesMet.LogAuditoriaExiste(IdUsuario, IdEmpresa, IdPuntoVenta, IdSistema) = True Then
        '    'Si el producto no existe lanzo una excepción.
        '    Throw New Exception("El usuario con el id " & IdUsuario & " ya existe!!!")
        'Else
        Return FuncionesMet.LogAuditoriaListarAcceso(IdUsuario, IdPais, IdEmpresa, IdLocal, IdPuntoVenta, IdSistema, IdModulo)
        'End If
    End Function

    Public Function LogAuditoriaInserta(ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Return FuncionesMet.LogAuditoriaInserta(LogAuditoria)
    End Function

    Public Function OperacionContableCorrelativoAsiento(ByVal st As clsFuncionesMetodos.stTransaccion) As String
        Try
            Return FuncionesMet.OperacionContableCorrelativoAsiento(st)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    'Public Function OperacionContableInserta(ByVal st As List(Of clsFuncionesMetodos.stTransaccion), Optional ByVal bContinuar As Boolean = False, Optional ByVal NroAsiento As String = "", Optional ByVal bCanjear As Boolean = False) As String ' As DataTable
    Public Function OperacionContableInserta(ByVal st As List(Of clsFuncionesMetodos.stTransaccion), Optional ByVal NroAsiento As String = "", Optional ByVal bCanjear As Boolean = False) As String ' As DataTable
        'Try
        'Inicializo la Transacción
        'Using scope As New TransactionScope()
        Try
            'Return FuncionesMet.OperacionContableInserta(st, bContinuar, NroAsiento, bCanjear)
            Dim strValor As String = FuncionesMet.OperacionContableInserta(st, NroAsiento, bCanjear)
            'MsgBox("Proceso Registrado")
            '    scope.Complete()
            Return strValor
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        'End Using
    End Function

    Public Function OperacionContableUpdate(ByVal st As List(Of clsFuncionesMetodos.stTransaccion), Optional ByVal bContinuar As Boolean = False, Optional ByVal NroAsiento As String = "") As String ' As DataTable
        Try
            Return FuncionesMet.OperacionContableUpdate(st, bContinuar, NroAsiento)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function GeneraLetra(ByVal Importe As Decimal, ByVal TipoMoneda As String, ByVal MonedaAbrev As Boolean, ByVal SoloLetra As Boolean) As String
        Return FuncionesMet.GeneraLetra(Importe, TipoMoneda, MonedaAbrev, SoloLetra)
    End Function

    'Function ValidaPerfil(ByVal IdUsuario As String, ByVal IdPerfil As String, ByVal IdElemento As String, ByVal IdModulo As String,
    '                   ByVal IdSistema As String, ByVal IdArea As String) As Boolean
    Function ValidaPerfil(ByVal IdUsuario As String, ByVal IdPerfil As String, ByVal IdElemento As String, ByVal IdModulo As String,
                   ByVal IdSistema As String) As Boolean

        'If FuncionesMet.ValidaPerfil(IdUsuario, IdPerfil, IdElemento, IdModulo, IdSistema, IdArea) = False Then
        If FuncionesMet.ValidaPerfil(IdUsuario, IdPerfil, IdElemento, IdModulo, IdSistema) = False Then
            'Si no existe lanzo una excepción.
            ValidaPerfil = False
            Throw New Exception("El usuario no tiene permiso para ejecutar esta opción.")
        Else
            Return True
        End If
    End Function

    'Function ValidaPerfilObjeto(ByVal IdUsuario As String, ByVal IdPerfil As String, ByVal IdElemento As String, ByVal IdModulo As String,
    '                   ByVal IdSistema As String, ByVal IdArea As String) As Boolean
    '    Return FuncionesMet.ValidaPerfil(IdUsuario, IdPerfil, IdElemento, IdModulo, IdSistema, IdArea)
    'End Function
    Function ValidaPerfilObjeto(ByVal IdUsuario As String, ByVal IdPerfil As String, ByVal IdElemento As String, ByVal IdModulo As String,
                       ByVal IdSistema As String) As Boolean
        Return FuncionesMet.ValidaPerfil(IdUsuario, IdPerfil, IdElemento, IdModulo, IdSistema)
    End Function

    Function ValidarCierreMes(ByVal IdPeriodo As String, ByVal IdMes As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdSistema As String) As Boolean
        If FuncionesMet.ValidarCierreMes(IdPeriodo, IdMes, IdEmpresa, IdPuntoVenta, IdSistema) = False Then
            Return False
        Else
            'Si no existe lanzo una excepción.
            ValidarCierreMes = True
            Throw New Exception("Se ha realizado el Cierre Mensual.  No puede realizar ninguna modificación.  Contáctese con el Administrador.")
        End If
    End Function

    Public Function GuardarArchivoGeneral(Archivo As HttpPostedFile, Carpeta As String, ArchivoDestino As String) As String
        Try
            Return FuncionesMet.GuardarArchivoGeneral(Archivo, Carpeta, ArchivoDestino)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GuardarArchivo(Archivo As HttpPostedFile, Redimensionar As Boolean, Carpeta As String, ArchivoDestino As String, ImagenPeque As Boolean, Optional CarpetaPeriodo As String = "", Optional Tamano As Integer = 0, Optional TamanoPeque As Integer = 0) As String
        Try
            Return FuncionesMet.GuardarArchivo(Archivo, Redimensionar, Carpeta, ArchivoDestino, ImagenPeque, CarpetaPeriodo, Tamano, TamanoPeque)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    'Public Function ResizeImage(Source As Byte(), Optional Size As Integer = 0) As Byte()
    '    Try
    '        Return FuncionesMet.ResizeImage(Source, Size)
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    'End Function

    Function Encriptar(ByVal Input As String) As String
        Try
            Return FuncionesMet.Encriptar(Input)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Function Desencriptar(ByVal Input As String) As String
        Try
            Return FuncionesMet.Desencriptar(Input)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    'Function CambiarCadenaConexion(ByVal Conexion As String) As String
    '    Try
    '        Return FuncionesMet.CambiarCadenaConexion(Conexion)
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    'End Function

    'Function GetCnx() As String
    '    Try
    '        Return FuncionesMet.GetCnx()
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    'End Function
    Public Function MigrarDataMiSazonPE(ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
        Return FuncionesMet.MigrarDataMiSazonPE(IdEmpresa, IdPuntoVenta)
    End Function

    'Public Function GenerarDocumentoVentaElectronico(ByVal IdTipoDocumento As String, ByVal IdNumeroSerieDocumento As String,
    '    ByVal IdNumeroCorrelativo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As DataTable
    '    Return FuncionesMet.GenerarDocumentoVentaElectronico(IdTipoDocumento, IdNumeroSerieDocumento, IdNumeroCorrelativo, IdEmpresa, IdPuntoVenta)
    'End Function
    'Public Function GenerarDocumentoVentaElectronico(ByVal IdTipoDocumento As String, ByVal IdNumeroSerieDocumento As String,
    '    ByVal IdNumeroCorrelativo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As DataTable
    '    Return FuncionesMet.GenerarDocumentoVentaElectronico(IdTipoDocumento, IdNumeroSerieDocumento, IdNumeroCorrelativo, IdEmpresa, IdPuntoVenta)
    'End Function

    'Public Function GenerarDocumentoVentaElectronicoXML(ByVal IdTipoDocumento As String, ByVal IdNumeroSerieDocumento As String,
    '    ByVal IdNumeroCorrelativo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As String
    '    Return FuncionesMet.GenerarDocumentoVentaXML(IdTipoDocumento, IdNumeroSerieDocumento, IdNumeroCorrelativo, IdEmpresa, IdPuntoVenta)
    'End Function


    Public Function IsValidarEmail(ByVal strIn As String) As Boolean
        Return FuncionesMet.IsValidarEmail(strIn)
    End Function

    Public Function IsValidarDNI_RUC(ByVal IdEmpresa As String, ByVal strDocumento As String, ByVal strTipoDocumento As String) As Boolean
        Return FuncionesMet.IsValidarDNI_RUC(IdEmpresa, strDocumento, strTipoDocumento)
    End Function

    Public Function IsValidarDNI_RUC_Proveedor(ByVal IdEmpresa As String, ByVal strDocumento As String, ByVal strTipoDocumento As String) As Boolean
        Return FuncionesMet.IsValidarDNI_RUC_Proveedor(IdEmpresa, strDocumento, strTipoDocumento)
    End Function

    Public Function IsValidarDNI_RUC_Transportista(ByVal IdEmpresa As String, ByVal strDocumento As String, ByVal strTipoDocumento As String) As Boolean
        Return FuncionesMet.IsValidarDNI_RUC_Transportista(IdEmpresa, strDocumento, strTipoDocumento)
    End Function

    Public Function VerificarConexionURL(ByVal sURL As String) As Boolean
        Return FuncionesMet.VerificarConexionURL(sURL)
    End Function

    Public Function ConvertirListADataTable(Of t)(
                                                 ByVal list As IList(Of t)
                                              ) As DataTable
        Return FuncionesMet.ConvertirListADataTable(Of t)(list)
    End Function

    Public Function EnviarCorreo(ByVal strFrom As String, ByVal strPwd As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMensaje As String, ByVal strRuta As String, ByVal strNombreArchivo As String, ByVal strConfiguracionCorreo As String, ByVal strConfiguracionPeriodo As String, ByVal strCarpetaPDF As String, Optional ByVal booEnviarXMLFirmado As Boolean = True) As Boolean
        Return FuncionesMet.EnviarCorreo(strFrom, strPwd, strTo, strSubject, strMensaje, strRuta, strNombreArchivo, strConfiguracionCorreo, strConfiguracionPeriodo, strCarpetaPDF, booEnviarXMLFirmado)
    End Function

    Public Function FormatoEnvioGeneral(ByVal strIdTipoFormato As String, ByVal strParametros As String) As String
        Return FuncionesMet.FormatoEnvioGeneral(strIdTipoFormato, strParametros)
    End Function

    Public Function FormatoEnvio(ByVal strCliente As String, ByVal strNroSerie As String, strNroDocumento As String,
                     ByVal strTipoDocumentoDescripcion As String, ByVal strImporte As String, ByVal strFechaEmision As String,
                     ByVal strRazonSocialEmisor As String) As String
        Return FuncionesMet.FormatoEnvio(strCliente, strNroSerie, strNroDocumento, strTipoDocumentoDescripcion, strImporte, strFechaEmision, strRazonSocialEmisor)
    End Function

    Public Function FormatoEnvioPortalEcomNet(ByVal strRuc As String, ByVal strRazonSocial As String, strFechaEmisionInicial As String,
                     ByVal strFechaEmisionFinal As String) As String
        Return FuncionesMet.FormatoEnvioPortalEcomNet(strRuc, strRazonSocial, strFechaEmisionInicial, strFechaEmisionFinal)
    End Function

    'INICIO: JMUG: 27/01/2023
    'Public Function FormatoEnvioRecordatorioEcomNet(ByVal strRuc As String, ByVal strRazonSocial As String, ByVal strCorreo As String) As String
    '    Return FuncionesMet.FormatoEnvioRecordatorioEcomNet(strRuc, strRazonSocial, strCorreo)
    'End Function
    'FINAL: JMUG: 27/01/2023

    Public Function FormatoEnvioValidacionEnvioCPEEcomNet() As String
        Return FuncionesMet.FormatoEnvioValidacionEnvioCPEEcomNet()
    End Function

    Public Function FormatoEnvioCreacionOT(ByVal strNroOT As String, ByVal strFechaProgramada As String) As String
        Return FuncionesMet.FormatoEnvioCreacionOT(strNroOT, strFechaProgramada)
    End Function

    Public Sub GuardarImagenTabla(ByVal byteImage As Byte(), ByVal strTipo As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String)
        FuncionesMet.GuardarImagenTabla(byteImage, strTipo, IdEmpresa, IdPuntoVenta, IdTipDoc, IdNroSer, IdNroDoc)
    End Sub

    Public Sub GenerarArchivoZIP(ByVal strRuta As String, ByVal strNombreArchivo As String)
        FuncionesMet.GenerarArchivoZIP(strRuta, strNombreArchivo)
    End Sub

    Public Sub DescomprimirArchivoZIP(ByVal strRuta As String, ByVal strNombreArchivo As String)
        FuncionesMet.DescomprimirArchivoZIP(strRuta, strNombreArchivo)
    End Sub
    'Public Function CargarXMLUsuPwdSunat(ByVal strRuta As String, ByVal strNombreXML As String,
    '                                     ByVal strUsuario As String, ByVal strContrasena As String,
    '                                     ByVal strRuc As String) As Boolean
    '    Return FuncionesMet.CargarXMLUsuPwdSunat(strRuta, strNombreXML, strUsuario, strContrasena, strRuc)
    'End Function

    Public Function MigrarConsultaEcomNETTXT(ByVal FechaInicial As String, ByVal FechaFinal As String,
                               ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal Ruta As String) As String
        Return FuncionesMet.MigrarConsultaEcomNETTXT(FechaInicial, FechaFinal, IdEmpresa, IdPuntoVenta, Ruta)
    End Function

    Public Function CargarDocumentoVentaResumen(ByVal FechaInicial As String, ByVal FechaFinal As String,
                                                ByVal IdEmpresa As String, ByVal IdPuntoVenta As String,
                                                ByVal IdTipo As String) As DataTable
        Return FuncionesMet.CargarDocumentoVentaResumen(FechaInicial, FechaFinal, IdEmpresa, IdPuntoVenta, IdTipo)
    End Function

    Public Function fExportarExcel(ByVal NombreArchivo As String, ByVal StoreProcedure As String, ByVal CamposParametros As String, ByVal ValorParametros As String,
                       ByVal TipoSeparador As String, ByVal Ruta As String, ByVal ConCabecera As String) ', ByVal NombreCampo() As String)
        Return FuncionesMet.fExportarExcel(NombreArchivo, StoreProcedure, CamposParametros, ValorParametros, TipoSeparador, Ruta, ConCabecera) ', NombreCampo)
    End Function

    Public Function fNroSemanaAno(ByVal dFecha As Date)
        Return FuncionesMet.fNroSemanaAno(dFecha)
    End Function

    Public Function fUploadFile(ByVal Server As String, ByVal User As String, ByVal Pass As String, ByVal PathOrigen As String, ByVal FileName As String) As Boolean
        Return FuncionesMet.fUploadFile(Server, User, Pass, PathOrigen, FileName)
    End Function
End Class