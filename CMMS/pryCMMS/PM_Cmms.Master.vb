Imports CapaDatosCMMS 'Lo acabo de agragar solo para obtener el usuario.
Imports CapaNegocioCMMS

Public Class PM_Cmms
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Try
        Session("IdSesion") = Session.SessionID
        Session("IP1") = Request.ServerVariables("REMOTE_ADDR")
        Session("IP2") = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        Session("URL") = Request.ServerVariables("URL")
        Dim EmpresaNeg As New clsEmpresaNegocios
        Dim PuntoVentaNeg As New clsPuntoVentaNegocios
        Dim LocalNeg As New clsLocalNegocios

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            If Session("IdUsuario") <> "" Then
                lblEmpresa.Text = StrConv(EmpresaNeg.EmpresaListarPorId(Session("IdEmpresa")).vDescripcionEmpresa, VbStrConv.ProperCase)
                lblRucEmpresa.Text = EmpresaNeg.EmpresaListarPorId(Session("IdEmpresa")).vRucEmpresa
                'lblPuntoVenta.Text = StrConv(PuntoVentaNeg.PuntoVentaListarPorId(Session("IdPuntoVenta"), Session("IdEmpresa")).vDescripcionPuntoVenta, VbStrConv.ProperCase)
                lblLocal.Text = StrConv(LocalNeg.LocalListarPorId(Session("IdLocal"), Session("IdEmpresa")).vDescripcionLocal, VbStrConv.ProperCase)

                lblIdEmpresa.Value = Session("IdEmpresa")
                lblIdPuntoVenta.Value = Session("IdPuntoVenta")
                lblIdLocal.Value = Session("IdLocal")

                Dim UsuarioNeg As New clsUsuarioNegocios
                Dim Usu As GNRL_USUARIO = UsuarioNeg.UsuarioListarPorId(Session("IdUsuario"))
                mnuNombrePerfil.Text = Mid(Usu.vNombresUsuario, 1, IIf(InStrRev(Usu.vNombresUsuario, " ") = 0, Len(Usu.vNombresUsuario), InStrRev(Usu.vNombresUsuario, " ") - 1)) + " " + Usu.vApellidoPaternoUsuario

                Dim strOpcionModulo As String
                Dim FunNeg As New clsFuncionesNegocios
                strOpcionModulo = "000"
                'Habilitar Menú Principal
                'mnuTransaccionesEmisionAlquiler.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "604", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesEmisionEquipo.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0604", strOpcionModulo, Session("IdSistema"))
                mnuTransaccionesEmisionEquipo.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0689", strOpcionModulo, Session("IdSistema"))
                'mnuTransaccionesEmisionComanda.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "542", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesEmisionPreCuenta.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "548", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesEmitirCotizacion.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "486", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesEmisionComprobante.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "447", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesEmisionGuia.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "487", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesListadoDocumentosPreVenta.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "488", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesListadoComprobantes.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "449", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesListadoGuia.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "567", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

                mnuTransaccionesCheckListPlantilla.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0697", strOpcionModulo, Session("IdSistema"))

                mnuTransaccionesSolicitudServicio.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0698", strOpcionModulo, Session("IdSistema"))
                mnuTransaccionesOrdenTrabajoServicio.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0699", strOpcionModulo, Session("IdSistema"))

                mnuTrannsaccionesContrato.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0700", strOpcionModulo, Session("IdSistema"))
                mnuTrannsaccionesContrato2.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0700", strOpcionModulo, Session("IdSistema"))
                mnuTrannsaccionesContrato3.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0700", strOpcionModulo, Session("IdSistema"))
                'mnuTrannsaccionesContratoConfiguracion.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0700", strOpcionModulo, Session("IdSistema"))
                mnuTransaccionesOrdenTrabajoContrato.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0701", strOpcionModulo, Session("IdSistema"))

                mnuTransaccionesAsignarCertificado.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0702", strOpcionModulo, Session("IdSistema"))

                mnuTransaccionesOrdenVentaAlmacen.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0703", strOpcionModulo, Session("IdSistema"))

                'mnuTransaccionesEnviarResumenComprobantes.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "448", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesComprobantesAnulados.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "450", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

                mnuTransaccionesTipoActivo.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0704", strOpcionModulo, Session("IdSistema"))
                mnuTransaccionesCaracteristica.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0690", strOpcionModulo, Session("IdSistema"))
                mnuTransaccionesCatalogo.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0705", strOpcionModulo, Session("IdSistema"))
                mnuTransaccionesSistemaFuncional.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0706", strOpcionModulo, Session("IdSistema"))
                mnuTransaccionesCatalogoComponente.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0707", strOpcionModulo, Session("IdSistema"))

                'mnuMantenimientoCorrelativoDocumento.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "460", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoCorrelativoDocumento.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0460", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoPuntoVentas.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "461", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoGrupos.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "462", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoTipoCambio.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "463", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoTipoCambio.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0463", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoTipoMoneda.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "464", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoTipoMoneda.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0464", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoCentroCosto.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "465", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoGrupoVariante.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "568", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoGrupoVariante.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0568", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoVariante.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "569", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoVariante.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0569", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoCategoria.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0570", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoSubCategoria.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "571", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoSubCategoria.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0571", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoProductos.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "466", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoProductos.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0466", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoListaPrecio.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "467", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoControlStock.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "572", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoClientes.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "468", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoClientes.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0468", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoVendedores.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "469", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoTransportistas.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "573", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoCajeros.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "470", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoCajeros.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0470", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoBancos.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "471", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoUnidadesMedidas
                'mnuMantenimientoEmpresas.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "472", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoEmpresas.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0472", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoLocales.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "473", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoLocales.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0473", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoUsuarios.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "474", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoUsuarios.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0474", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoPerfilesSeguridad.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "475", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoPerfilesSeguridad.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0475", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoTablaSistema.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "476", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                mnuMantenimientoTablaSistema.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0476", strOpcionModulo, Session("IdSistema"))

                mnuInformesTipoCambio.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0708", strOpcionModulo, Session("IdSistema"))
                mnuInformesAuditoria.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0709", strOpcionModulo, Session("IdSistema"))
                'mnuMantenimientoProveedor.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "477", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuMantenimientoCuentaCorriente
                'mnuInformesRegistroVentas.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "478", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesResumenVentas.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "478", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesVentasXMedioPago.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "610", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesMovimientoCajaBanco.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "612", strOpcionModulo, Session("IdSistema"), Session("IdArea"))


                ''LOGISTICA
                'mnuTransaccionesEmisionCompra.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "574", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesRegistroIngreso.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "575", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesRegistroSalida.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "576", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesAperturaInventario.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "577", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesAjusteInventario.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "578", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesTransferenciaInventario.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "579", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesListadoComprobantesLogistica.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "580", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesResumenStock.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "581", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesResumenValorizado.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "582", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesRegistroCompra.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "583", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesRegistroInventario.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "603", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesInventarioCompras.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "584", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuInformesInventarioVentas.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "585", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

                ''FINANZAS
                'mnuTransaccionesEmisionMovimientoCaja.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "602", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'mnuTransaccionesEmisionReciboCobranza.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "534", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

                ''CONTABILIDAD
                'mnuTransaccionesGenerarAsiento.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "614", strOpcionModulo, "CTBL", Session("IdArea"))
                'mnuTransaccionesProcesarAjusteXDiferenciaCambio.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "615", strOpcionModulo, "CTBL", Session("IdArea"))
                'mnuTransaccionesRecalcularNumeracionAsientos.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "616", strOpcionModulo, "CTBL", Session("IdArea"))
                'mnuTransaccionesBloqueoCierreMensual.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "617", strOpcionModulo, "CTBL", Session("IdArea"))
                'mnuTransaccionesDesbloqueoCierreMensual.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "618", strOpcionModulo, "CTBL", Session("IdArea"))
                'mnuTransaccionesTransferirSaldosApertura.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "619", strOpcionModulo, "CTBL", Session("IdArea"))
                'mnuTransaccionesCierreLibrosContables.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "620", strOpcionModulo, "CTBL", Session("IdArea"))
                'mnuTransaccionesAnexosPDTAnual.Visible = FunNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "621", strOpcionModulo, "CTBL", Session("IdArea"))
            End If
        End If
    End Sub

    'Private Sub btnAceptarImagen_Click(sender As Object, e As System.EventArgs) Handles btnAceptarImagen.Click
    '    Try
    '        lblMensajeSeleccionarImagen.Text = ""

    '        If Not (fileUploader1.HasFile) Then
    '            Throw New Exception("Seleccione un archivo del disco duro.")
    '        End If

    '        'Se verifica que la extensión sea de un formato válido
    '        'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
    '        Dim ext As String = fileUploader1.PostedFile.FileName
    '        ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

    '        Dim formatos() As String = New String() {"jpg", "jpeg", "bmp", "png", "gif"}
    '        If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de imagen inválido.")
    '        Dim Size As Integer = 0
    '        If Not (Integer.TryParse("80", Size)) Then
    '            Throw New Exception("El tamaño indicado para la imagen no es válido.")
    '        End If

    '        'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
    '        Dim FuncionesNeg As New clsFuncionesNegocios
    '        FuncionesNeg.GuardarArchivo(fileUploader1.PostedFile, True, "Imagenes\Usr", Trim(Session("IdUsuario")), True, 220, 80)
    '    Catch ex As Exception
    '        lblMensajeSeleccionarImagen.Text = ex.Message
    '        imgbtnCargarImagen_ModalPopupExtender.Show()
    '    End Try
    'End Sub

    'Protected Sub Img1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles Img1.Click
    '    Response.Redirect("~\frmPaginaConstruccion.aspx?invitacion=1")
    'End Sub

    'Protected Sub Img2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles Img2.Click
    '    Response.Redirect("~\frmGnrlAlertas.aspx")
    'End Sub

    'Protected Sub Img3_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles Img3.Click
    '    Response.Redirect("~\frmGnrlPreguntasFrecuentes.aspx?invitacion=1")
    'End Sub

    'Protected Sub Img4_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles Img4.Click
    '    Response.Redirect("~\frmGnrlMensajes.aspx")
    'End Sub

    'Protected Sub lnkSubMenuCerrarSesion_Click(sender As Object, e As EventArgs) Handles lnkSubMenuCerrarSesion.Click
    '    Session.Abandon()
    '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "1", False)
    '    Context.ApplicationInstance.CompleteRequest()
    'End Sub

End Class