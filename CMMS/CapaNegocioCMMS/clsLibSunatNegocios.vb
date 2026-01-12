Imports CapaDatosCMMS

Public Class clsLibSunatNegocios
    Dim SunatMet As New clsLibSunatMetodos ' clsPersonalMetodos

    Public Function SunatConversionCaracteresEspeciales(ByVal Cadena As String) As String
        Return SunatMet.SunatConversionCaracteresEspeciales(Cadena)
    End Function

    Public Function SunatMigrarOPTComprasTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, ByVal ImporteBaseImponible As Decimal, ByVal IdTipoDetalle As String)
        Return SunatMet.SunatMigrarOPTComprasTXT(IdPeriodo, IdMesInicial, IdMesFinal,
                                       IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                       Orden, ImporteBaseImponible, IdTipoDetalle)
    End Function

    Public Function SunatMigrarOPTVentasTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, ByVal ImporteBaseImponible As Decimal, ByVal IdTipoDetalle As String)
        Return SunatMet.SunatMigrarOPTVentasTXT(IdPeriodo, IdMesInicial, IdMesFinal,
                                       IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                       Orden, ImporteBaseImponible, IdTipoDetalle)
    End Function

    Public Function SunatMigrarAnexosPDTAnualTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                           ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                           ByVal ImporteBaseImponible As Decimal, ByVal IdNumeroCuentaAsiento As String,
                           ByVal NroCasilla As String)
        Return SunatMet.SunatMigrarAnexosPDTAnualTXT(IdPeriodo, IdMesInicial, IdMesFinal,
                               IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                               ImporteBaseImponible, IdNumeroCuentaAsiento, NroCasilla)
    End Function

    Public Function SunatMigrarRegistroVentasTXT(ByVal IdPeriodo As String, ByVal IdMes As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, Ruta As String) As String
        Return SunatMet.SunatMigrarRegistroVentasTXT(IdPeriodo, IdMes,
                                       IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                       Orden, Ruta)
    End Function

    Public Function SunatVtasLEMigrarRegistroVentasTXT(ByVal FechaInicial As String, ByVal FechaFinal As String,
                                                        ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                                        Ruta As String) As String

        Return SunatMet.SunatVtasLEMigrarRegistroVentasTXT(FechaInicial, FechaFinal,
                                                            IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                                            Ruta)
    End Function

    Public Function SunatVtasLEMigrarRegistroVentasSimplificadoTXT(ByVal FechaInicial As String, ByVal FechaFinal As String,
                                                        ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                                        Ruta As String) As String

        Return SunatMet.SunatVtasLEMigrarRegistroVentasSimplificadoTXT(FechaInicial, FechaFinal,
                                                            IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                                            Ruta)
    End Function

    Public Function SunatMigrarRegistroComprasTXT(ByVal IdPeriodo As String, ByVal IdMes As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, Ruta As String) As String
        Return SunatMet.SunatMigrarRegistroComprasTXT(IdPeriodo, IdMes,
                                       IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                       Orden, Ruta)
    End Function

    Public Function SunatMigrarRegistroComprasNoDomiciliadosTXT(ByVal IdPeriodo As String, ByVal IdMes As String,
                                   ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                   ByVal Orden As String, Ruta As String) As String
        Return SunatMet.SunatMigrarRegistroComprasNoDomiciliadosTXT(IdPeriodo, IdMes,
                                       IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                       Orden, Ruta)
    End Function

    Public Function SunatGetData(strQuery As String) As DataTable
        Return SunatMet.SunatGetData(strQuery)
    End Function

    Public Function SunatMigrarLibroDiarioTXT(ByVal IdPeriodo As String, ByVal IdMesInicial As String, ByVal IdMesFinal As String,
                                       ByVal IdEmpresa As String, ByVal IdPuntoVenta As String, ByVal IdTipoMoneda As String,
                                       ByVal IdTipoLibro As String, ByVal Orden As String, Ruta As String) As String

        Return SunatMet.SunatMigrarLibroDiarioTXT(IdPeriodo, IdMesInicial, IdMesFinal,
                                       IdEmpresa, IdPuntoVenta, IdTipoMoneda,
                                       IdTipoLibro, Orden, Ruta)
    End Function

    Public Function GenerarCPEXML(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal Ruta As String) As clsLibSunatMetodos.stCPE ', ByVal Tipo As Integer) As clsLibSunatMetodos.stCPE
        Return SunatMet.GenerarCPEXML(stDocumento, Ruta) ', Tipo)
    End Function

    'Public Function SunatConfiguracionCPE(stDocumento As clsLibSunatMetodos.stCPE, strPasswordCertificado As String, intTipo As Integer, strAmbiente As String) As clsLibSunatMetodos.stCPE
    '    Return SunatMet.SunatConfiguracionCPE(stDocumento, strPasswordCertificado, intTipo, strAmbiente)
    'End Function

    'Public Function GetStatusCDR(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRuta As String, ByVal intTipo As Integer, ByVal strAmbiente As String) As clsLibSunatMetodos.stCPE
    '    Return SunatMet.GetStatusCDR(stDocumento, strRuta, intTipo, strAmbiente)
    'End Function

    Public Function CapturarHash(ByVal stDocumento As clsLibSunatMetodos.stCPE, ByVal strRutaXMLCertificado As String) As clsLibSunatMetodos.stCPE
        Return SunatMet.CapturarHash(stDocumento, strRutaXMLCertificado)
    End Function

    Public Function SunatLoadConsultaDNI(ByVal DNI As String) As clsLibSunatMetodos.stSunatDNI
        Return SunatMet.SunatLoadConsultaDNI(DNI)
    End Function
End Class