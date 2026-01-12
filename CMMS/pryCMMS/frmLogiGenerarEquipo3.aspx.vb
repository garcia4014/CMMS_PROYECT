Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarEquipo3
    Inherits System.Web.UI.Page
    Dim EquipoNeg As New clsEquipoNegocios
    Dim CatalogoNeg As New clsCatalogoNegocios
    Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator
    Shared rowIndexDetalle As Int64


    Public Sub CargarPerfil()
        'Dim AmigoNeg As New clsAmigoNegocios
        Dim FuncionesNeg As New clsFuncionesNegocios

        Dim mpContentPlaceHolder As ContentPlaceHolder
        Dim mpImage As Image
        Dim mpNombreUsuario As Label
        Dim mpApellidoPaternoUsuario As Label
        Dim mpApellidoMaternoUsuario As Label

        mpContentPlaceHolder =
            CType(Master.FindControl("LeftPanel$mnu_izq"),
            ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            mpImage = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$imgPerfil"), Image)
            If Not mpImage Is Nothing Then
                mpImage.ImageUrl = "~\Imagenes\Usr\" & Trim(Session("IdUsuario")) & ".JPG"
            End If

            Dim UsuarioNeg As New clsUsuarioNegocios 'CapaNegocioGnrl.clsUsuarioNegocios
            Dim Usuario As GNRL_USUARIO = UsuarioNeg.UsuarioListarPorId(Session("IdUsuario"))

            mpNombreUsuario = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblNombreUsuario"), Label)
            If Not mpNombreUsuario Is Nothing Then
                mpNombreUsuario.Text = StrConv(Usuario.vNombresUsuario, VbStrConv.ProperCase)
            End If

            mpApellidoPaternoUsuario = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblApellidoPaternoUsuario"), Label)
            If Not mpApellidoPaternoUsuario Is Nothing Then
                mpApellidoPaternoUsuario.Text = StrConv(Usuario.vApellidoPaternoUsuario, VbStrConv.ProperCase)
            End If

            mpApellidoMaternoUsuario = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblApellidoMaternoUsuario"), Label)
            If Not mpApellidoMaternoUsuario Is Nothing Then
                mpApellidoMaternoUsuario.Text = StrConv(Usuario.vApellidoMaternoUsuario, VbStrConv.ProperCase)
            End If
        End If
    End Sub

    Function fValidarSesion() As Boolean
        fValidarSesion = False
        If Session("IdUsuario") = "" Then
            fValidarSesion = True
            'Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Throw New Exception("Su sesión ha caducado, ingrese de nuevo por favor.")
            'ElseIf Session("IdConfEmpresa") = "" Then
            '    fValidarSesion = True
            '    'Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
            '    Throw New Exception("No se ha ingresado al sistema correctamente.")
        End If
        'If Session("IdPuntoVenta") <> "" Or Session("IdEmpresa") <> "" Then
        If Session("IdEmpresa") <> "" Then
            Dim mpIdEmpresa, mpIdPuntoVenta As HiddenField
            mpIdEmpresa = CType(Page.Master.FindControl("lblIdEmpresa"), HiddenField)
            mpIdPuntoVenta = CType(Page.Master.FindControl("lblIdPuntoVenta"), HiddenField)

            If Not mpIdEmpresa Is Nothing Then
                If mpIdEmpresa.Value <> Session("IdEmpresa") Then
                    fValidarSesion = True
                End If
            End If
            If Not mpIdPuntoVenta Is Nothing Then
                If mpIdPuntoVenta.Value <> Session("IdPuntoVenta") Then
                    fValidarSesion = True
                End If
            End If

            If fValidarSesion = True Then
                Dim message As String = "Tiene dos sesiones distintas abiertas en la misma ventana de navegación y eso no es correcto, presione [F5] para refrescar la información actual de su navegador.  " &
                                            "Para poder utilizar dos sesiones distintas, realicelo en dos ventanas diferentes del navegador que usted utilice."
                Dim sb As New System.Text.StringBuilder()
                sb.Append("alert('")
                sb.Append(message)
                sb.Append("');")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowAlert", sb.ToString(), True)
                Throw New Exception("Existen distintas sesiones, favor de actualizar su explorador!")
                'Dim segmentosURL = HttpContext.Current.Request.Url.Segments
                'Response.Redirect(segmentosURL(segmentosURL.Length - 1))
            End If
        End If
    End Function


    Shared Function CrearCesta() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdTipoActivo", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdSistemaFuncional", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("Estado", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("DescripcionAbreviada", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdCuentaContable", GetType(System.String))) '7
        Return dt
    End Function

    Shared Sub EditarCesta(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdTipoActivo As String,
                           ByVal IdSistemaFuncional As String, ByVal Estado As String, ByVal DescripcionAbreviada As String,
                           ByVal IdCuentaContable As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        'ByVal FecVen As System.Nullable(Of DateTime), ByVal CentroCosto As String, _
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                'Tabla.Rows(Fila)(0) = xxx 'Numero de Linea
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Descripcion
                Tabla.Rows(Fila)(2) = IdTipoActivo
                Tabla.Rows(Fila)(3) = IdSistemaFuncional
                Tabla.Rows(Fila)(4) = Estado
                Tabla.Rows(Fila)(5) = DescripcionAbreviada
                Tabla.Rows(Fila)(6) = IdCuentaContable
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCesta(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdTipoActivo As String,
                           ByVal IdSistemaFuncional As String, ByVal Estado As String, ByVal DescripcionAbreviada As String,
                           ByVal IdCuentaContable As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("Estado") = Estado
        Fila("IdTipoActivo") = IdTipoActivo
        Fila("IdSistemaFuncional") = IdSistemaFuncional
        Fila("DescripcionAbreviada") = DescripcionAbreviada
        Fila("IdCuentaContable") = IdCuentaContable
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCesta(ByVal Fila As Integer, ByVal Tabla As DataTable)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows.RemoveAt(Fila)
                Dim i As Integer
                For i = 0 To Tabla.Rows.Count - 1
                    Tabla.Rows(i).BeginEdit()
                    Tabla.Rows(i).EndEdit()
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub VaciarCesta(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub CargarCestaComponenteCatalogo()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        VaciarCesta(Session("CestaCatalogoComponente"))

        Dim CatalogoNeg As New clsCatalogoNegocios
        Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        'Dim Coleccion2 = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdCatalogo", cboCatalogo.SelectedValue, 1)
        'Dim Coleccion2 = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdMaestroActivo", IIf(txtIdMaestroActivo.Text.Trim = "", "*", txtIdMaestroActivo.Text.Trim), 1)
        'Dim Coleccion2 = MaestroActivoNeg.MaestroActivoListaBusqueda("MAEACT.cIdEnlaceMaestroActivo = '" & txtIdMaestroActivo.Text.Trim & "' AND MAEACT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        Dim Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")

        'Dim Coleccion = CatalogoNeg.ConfiguracionListaBusqueda("CONF.cIdPerfilUsuario", txtIdPerfil.Text)

        Dim intContador As Integer = 0

        For Each Registro In Coleccion
            Dim booExiste As Boolean = False
            For Each Registro2 In Coleccion2
                'If Registro.Codigo <> Registro2.Codigo And Registro.IdSistemaFuncional <> Registro2.IdSistemaFuncional And Registro.IdTipoActivo <> Registro2.IdTipoActivo Then
                'If Registro.Codigo = Registro2.Codigo And Registro.IdSistemaFuncional = Registro2.IdSistemaFuncional And Registro.IdTipoActivo = Registro2.IdTipoActivo Then
                If Registro.Codigo = Registro2.IdCatalogo And Registro.IdSistemaFuncional = Registro2.IdSistemaFuncional And Registro.IdTipoActivo = Registro2.IdTipoActivo Then
                    'AgregarCestaAreaElemento(Registro.cIdSistema, Registro.cIdArea, Registro.cIdElemento, Registro.cIdModulo, Registro.cIdLoginUsuario, Registro.Usuario, Registro.Modulo, _
                    '                         Registro.cIdPerfilUsuario, Registro.vDescripcionArea, Registro.Elemento, Registro.Estado, Session("CestaAreaElemento"))
                    booExiste = True
                End If
            Next
            If booExiste = False Then
                AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.IdTipoActivo, Registro.IdSistemaFuncional, Registro.Estado, Registro.DescripcionAbreviada, "", Session("CestaCatalogoComponente"))
            End If
        Next

        grdComponenteCatalogo.DataSource = Session("CestaCatalogoComponente")
        Me.grdComponenteCatalogo.DataBind()
    End Sub

    Sub CargarCestaCatalogoCaracteristica()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '0' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '0'")
            For Each Caracteristicas In dsCaracteristica.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
            Next
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try

        'Dim CatalogoNeg As New clsCatalogoNegocios
        'Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        ''Dim Coleccion2 = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdCatalogo", cboCatalogo.SelectedValue, 1)
        ''Dim Coleccion2 = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdMaestroActivo", IIf(txtIdMaestroActivo.Text.Trim = "", "*", txtIdMaestroActivo.Text.Trim), 1)
        ''Dim Coleccion2 = MaestroActivoNeg.MaestroActivoListaBusqueda("MAEACT.cIdEnlaceMaestroActivo = '" & txtIdMaestroActivo.Text.Trim & "' AND MAEACT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        'Dim Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")

        ''Dim Coleccion = CatalogoNeg.ConfiguracionListaBusqueda("CONF.cIdPerfilUsuario", txtIdPerfil.Text)

        'Dim intContador As Integer = 0

        'For Each Registro In Coleccion
        '    Dim booExiste As Boolean = False
        '    For Each Registro2 In Coleccion2
        '        'If Registro.Codigo <> Registro2.Codigo And Registro.IdSistemaFuncional <> Registro2.IdSistemaFuncional And Registro.IdTipoActivo <> Registro2.IdTipoActivo Then
        '        'If Registro.Codigo = Registro2.Codigo And Registro.IdSistemaFuncional = Registro2.IdSistemaFuncional And Registro.IdTipoActivo = Registro2.IdTipoActivo Then
        '        If Registro.Codigo = Registro2.IdCatalogo And Registro.IdSistemaFuncional = Registro2.IdSistemaFuncional And Registro.IdTipoActivo = Registro2.IdTipoActivo Then
        '            'AgregarCestaAreaElemento(Registro.cIdSistema, Registro.cIdArea, Registro.cIdElemento, Registro.cIdModulo, Registro.cIdLoginUsuario, Registro.Usuario, Registro.Modulo, _
        '            '                         Registro.cIdPerfilUsuario, Registro.vDescripcionArea, Registro.Elemento, Registro.Estado, Session("CestaAreaElemento"))
        '            booExiste = True
        '        End If
        '    Next
        '    If booExiste = False Then
        '        AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.IdTipoActivo, Registro.IdSistemaFuncional, Registro.Estado, Registro.DescripcionAbreviada, "", Session("CestaCatalogoComponente"))
        '    End If
        'Next

        'grdComponenteCatalogo.DataSource = Session("CestaCatalogoComponente")
        'Me.grdComponenteCatalogo.DataBind()





        'For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
        '    If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (grdListaCaracteristica.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
        '        grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
        '        grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
        '        LimpiarObjetosCaracteristicas()
        '        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
        '        grdListaCaracteristica.SelectedIndex = -1
        '        lblMensajeCaracteristica.Text = "Característica ya registrada, seleccione otro item."
        '        Exit Sub
        '    End If
        'Next

        'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoMantenimientoCatalogo.Text, "0", grdListaCaracteristica.SelectedValue.trim, Server.HtmlDecode(grdListaCaracteristica.SelectedRow.Cells(1).Text).Trim, UCase(txtIdReferenciaSAPCaracteristica.Text.Trim), UCase(txtDescripcionCampoSAPCaracteristica.Text.Trim), UCase(txtValorCaracteristica.Text.Trim), Session("CestaCatalogoCaracteristica"))
        'Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
        'Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()

    End Sub

    Sub CargarCestaCatalogoComponenteCaracteristica()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '0' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '0'")
            For Each Caracteristicas In dsCaracteristica.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
            Next
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaEquipoComponente()
        'Carga las opciones que aun no estan asignadas en la Grilla de Opciones del Módulo.
        Try
            VaciarCesta(Session("CestaEquipoComponente"))
            Dim EquipoActivoNeg As New clsEquipoNegocios

            'If grdComponenteCatalogo.Rows.Count > 0 Then
            If grdComponenteCatalogo.SelectedIndex >= (grdComponenteCatalogo.Rows.Count - 1) Then
                'grdComponenteCatalogo.SelectedIndex = grdComponenteCatalogo.Rows.Count - 1
                grdComponenteCatalogo.SelectedIndex = -1
            End If

            If IsNothing(grdComponenteCatalogo.SelectedRow) = False Then
                If IsReference(grdComponenteCatalogo.SelectedRow.Cells(1).Text) = True Then
                    'Dim Coleccion = AreaElementoNeg.AreaElementoListaBusqueda("AREAELEM.cIdElemento", "", cboSistema.SelectedValue, grdModulo.SelectedRow.Cells(1).Text, _
                    '                                                                   cboArea.SelectedValue, txtIdPerfil.Text.Trim, txtIdUsuario.Text)
                    'Dim Coleccion = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdCatalogo", cboCatalogo.SelectedValue, 1)
                    'Dim Coleccion = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdMaestroActivo", IIf(txtIdMaestroActivo.Text.Trim = "", "*", txtIdMaestroActivo.Text.Trim), 1)
                    Dim Coleccion = EquipoNeg.EquipoListaBusqueda("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)

                    Dim intContador As Integer = 0

                    For Each Registro In Coleccion
                        'AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.IdTipoActivo, Registro.IdSistemaFuncional, Registro.Estado, Registro.DescripcionAbreviada, Session("CestaEquipoComponente"))
                        'clsLogiCestaMaestroActivo.AgregarCesta("", Registro.Codigo, Registro.IdTipoActivo, _
                        'Opcion nueva
                        Dim Equipo1 As New LOGI_EQUIPO
                        Equipo1 = EquipoNeg.EquipoListarPorId(txtIdEquipo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)

                        'clsLogiCestaEquipo.AgregarCesta(txtIdMaestroActivo.Text.Trim, Registro.IdCatalogo, Registro.IdTipoActivo,
                        '                                   "1", Registro.IdSistemaFuncional,
                        '                                    txtIdMaestroActivo.Text, Registro.Descripcion, Registro.DescripcionAbreviada,
                        '                                   "01/01/1900", True, "", UCase(txtObservacionAbreviadaDetalleMaestroActivo.Text.Trim), 0,
                        '                                   Registro.IdCuentaContable, IIf(MaestroActivo1.bEstadoActivacionMaestroActivo Is Nothing, False, MaestroActivo1.bEstadoActivacionMaestroActivo),
                        '                                   IIf(MaestroActivo1.dFechaActivacionMaestroActivo Is Nothing, "01/01/1900", MaestroActivo1.dFechaActivacionMaestroActivo), IIf(MaestroActivo1.bEstadoLeasingMaestroActivo Is Nothing, False, True),
                        '                                   UCase(txtMarcaDetalleMaestroActivo.Text.Trim), UCase(txtCaracteristicasDetalleMaestroActivo.Text.Trim), txtVidaUtilDetalleMaestroActivo.Text,
                        '                                   UCase(txtDimensionesDetalleMaestroActivo.Text.Trim), UCase(txtVoltajeDetalleMaestroActivo.Text.Trim), UCase(txtPesoDetalleMaestroActivo.Text.Trim),
                        '                                   txtIdInternoDetalleMaestroActivo.Text.Trim, cboMetodoDepreciacionDetalleMaestroActivo.SelectedValue, cboFamiliaDetalleMaestroActivo.SelectedValue,
                        '                                   cboEstadoActivoFijoDetalleMaestroActivo.SelectedValue, txtFechaFinalizacionDetalleMaestroActivo.Text, cboTipoMonedaDetalleMaestroActivo.SelectedValue,
                        '                                   txtSaldoInicialDetalleMaestroActivo.Text, cboEstadoOperacionDetalleMaestroActivo.SelectedValue, cboEstadoOperacionDetalleMaestroActivo.SelectedValue,
                        '                                   txtModeloDetalleMaestroActivo.Text, txtNroSerieDetalleMaestroActivo.Text, txtPlacaDetalleMaestroActivo.Text, txtNroUsuarioDetalleMaestroActivo.Text,
                        '                                   txtNroLectoraDetalleMaestroActivo.Text, txtNroContadoresDetalleMaestroActivo.Text, txtNroContratoArrendamientoDetalleMaestroActivo.Text,
                        '                                   txtFechaContratoArrendamientoDetalleMaestroActivo.Text, txtNroCuotasArrendamientoDetalleMaestroActivo.Text, MaestroActivo1.cIdAlmacen, MaestroActivo1.cIdTipoAlmacen,
                        '                                   Session("CestaEquipoComponente"))
                        'clsLogiCestaEquipo.AgregarCesta(txtIdEquipo.Text.Trim, Registro.IdCatalogo, Registro.IdTipoActivo,
                        '                                   "1", Registro.IdSistemaFuncional,
                        '                                    txtIdEquipo.Text, Registro.Descripcion, Registro.DescripcionAbreviada,
                        '                                   "01/01/1900", True, "", UCase(txtObservacionAbreviadaDetalleMaestroActivo.Text.Trim), 0,
                        '                                   Registro.IdCuentaContable, IIf(MaestroActivo1.bEstadoActivacionMaestroActivo Is Nothing, False, MaestroActivo1.bEstadoActivacionMaestroActivo),
                        '                                   IIf(MaestroActivo1.dFechaActivacionMaestroActivo Is Nothing, "01/01/1900", MaestroActivo1.dFechaActivacionMaestroActivo), IIf(MaestroActivo1.bEstadoLeasingMaestroActivo Is Nothing, False, True),
                        '                                   UCase(txtMarcaDetalleMaestroActivo.Text.Trim), UCase(txtCaracteristicasDetalleMaestroActivo.Text.Trim), txtVidaUtilDetalleMaestroActivo.Text,
                        '                                   UCase(txtDimensionesDetalleMaestroActivo.Text.Trim), UCase(txtVoltajeDetalleMaestroActivo.Text.Trim), UCase(txtPesoDetalleMaestroActivo.Text.Trim),
                        '                                   txtIdInternoDetalleMaestroActivo.Text.Trim, cboMetodoDepreciacionDetalleMaestroActivo.SelectedValue, cboFamiliaDetalleMaestroActivo.SelectedValue,
                        '                                   cboEstadoActivoFijoDetalleMaestroActivo.SelectedValue, txtFechaFinalizacionDetalleMaestroActivo.Text, cboTipoMonedaDetalleMaestroActivo.SelectedValue,
                        '                                   txtSaldoInicialDetalleMaestroActivo.Text, cboEstadoOperacionDetalleMaestroActivo.SelectedValue, cboEstadoOperacionDetalleMaestroActivo.SelectedValue,
                        '                                   txtModeloDetalleMaestroActivo.Text, txtNroSerieDetalleMaestroActivo.Text, txtPlacaDetalleMaestroActivo.Text, txtNroUsuarioDetalleMaestroActivo.Text,
                        '                                   txtNroLectoraDetalleMaestroActivo.Text, txtNroContadoresDetalleMaestroActivo.Text, txtNroContratoArrendamientoDetalleMaestroActivo.Text,
                        '                                   txtFechaContratoArrendamientoDetalleMaestroActivo.Text, txtNroCuotasArrendamientoDetalleMaestroActivo.Text, MaestroActivo1.cIdAlmacen, MaestroActivo1.cIdTipoAlmacen,
                        '                                   Session("CestaEquipoComponente"))

                        Dim TablaCestaMaestro As DataTable
                        TablaCestaMaestro = Session("CestaEquipoComponente")
                        Dim i As Integer

                        For i = 0 To TablaCestaMaestro.Rows.Count - 1
                            If TablaCestaMaestro.Rows(i)("IdTipoActivo") = cboTipoActivo.SelectedValue And
                               TablaCestaMaestro.Rows(i)("IdCatalogo") = cboCatalogo.SelectedValue Then
                                'TablaAreaElemento.Rows(i)("IdPerfil") = txtIdPerfil.Text And _
                                QuitarCesta(intContador, Session("CestaCatalogoComponente"))
                                intContador = intContador - 1
                            End If
                        Next
                        intContador = intContador + 1
                    Next
                End If
            Else



                'Dim Coleccion = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdCatalogo", cboCatalogo.SelectedValue, 1)
                'Dim Coleccion = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdMaestroActivo", IIf(txtIdMaestroActivo.Text.Trim = "", "*", txtIdMaestroActivo.Text.Trim), 1)
                'Dim Coleccion = MaestroActivoNeg.MaestroActivoListaBusqueda("MAEACT.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND MAEACT.cIdMaestroActivo", IIf(txtIdMaestroActivo.Text.Trim = "", "*", txtIdMaestroActivo.Text.Trim), 1)
                Dim Coleccion = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND EQU.cIdEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
                Dim intContador As Integer = 0

                For Each Registro In Coleccion
                    'AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.IdTipoActivo, Registro.IdSistemaFuncional, Registro.Estado, Registro.DescripcionAbreviada, Session("CestaEquipoComponente"))
                    'clsLogiCestaMaestroActivo.AgregarCesta(txtIdMaestroActivo.Text.Trim, Registro.Codigo, Registro.IdTipoActivo, _
                    'Dim MaestroActivo As LOGI_MAESTROACTIVO = MaestroActivoNeg.MaestroActivoListarPorId(grdLista.SelectedRow.Cells(1).Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                    'Dim MaestroActivo As LOGI_MAESTROACTIVO = MaestroActivoNeg.MaestroActivoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                    'Dim MaestroActivo As LOGI_MAESTROACTIVO
                    Dim Equipo As LOGI_EQUIPO
                    'If txtIdMaestroActivo.Text.Trim = "" Then
                    '    MaestroActivo = MaestroActivoNeg.MaestroActivoListarPorIdDetalle(txtIdMaestroActivo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                    'Else
                    '    MaestroActivo = MaestroActivoNeg.MaestroActivoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                    'End If
                    'MaestroActivo = MaestroActivoNeg.MaestroActivoListarPorIdDetalle(txtIdMaestroActivo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                    'Equipo = EquipoNeg.EquipoListarPorIdDetalle(txtIdMaestroActivo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                    Equipo = EquipoNeg.EquipoListarPorIdDetalle(txtIdEquipo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)

                    'clsLogiCestaEquipo.AgregarCesta(txtIdMaestroActivo.Text.Trim, Registro.IdCatalogo, Registro.IdTipoActivo,
                    '                                       "1", Registro.IdSistemaFuncional,
                    '                                       txtIdMaestroActivo.Text, Registro.Descripcion, Registro.DescripcionAbreviada,
                    '                                       IIf(MaestroActivo.dFechaAdquisicionMaestroActivo Is Nothing, "01/01/1900", MaestroActivo.dFechaAdquisicionMaestroActivo), True, MaestroActivo.cIdEnlaceCatalogo, MaestroActivo.vObservacionMaestroActivo, MaestroActivo.nCostoInicialMaestroActivo, MaestroActivo.cIdCuentaContableMaestroActivo,
                    '                                       IIf(MaestroActivo.bEstadoActivacionMaestroActivo Is Nothing, False, MaestroActivo.bEstadoActivacionMaestroActivo),
                    '                                       IIf(MaestroActivo.dFechaActivacionMaestroActivo Is Nothing, "01/01/1900", MaestroActivo.dFechaActivacionMaestroActivo),
                    '                                       IIf(MaestroActivo.bEstadoLeasingMaestroActivo Is Nothing, False, True),
                    '                                       IIf(MaestroActivo.vMarcaMaestroActivo Is Nothing, "", MaestroActivo.vMarcaMaestroActivo), IIf(MaestroActivo.vCaracteristicasMaestroActivo Is Nothing, "", MaestroActivo.vCaracteristicasMaestroActivo), IIf(MaestroActivo.nVidaUtilMaestroActivo Is Nothing, 0, MaestroActivo.nVidaUtilMaestroActivo),
                    '                                       IIf(MaestroActivo.vDimensionesMaestroActivo Is Nothing, "", MaestroActivo.vDimensionesMaestroActivo), IIf(MaestroActivo.vVoltajeMaestroActivo Is Nothing, "", MaestroActivo.vVoltajeMaestroActivo), IIf(MaestroActivo.vPesoMaestroActivo Is Nothing, "", MaestroActivo.vPesoMaestroActivo),
                    '                                       IIf(MaestroActivo.vIdEquivalenciaMaestroActivo Is Nothing, "", MaestroActivo.vIdEquivalenciaMaestroActivo), IIf(MaestroActivo.vIdMetodoAplicadoDepreciacionMaestroActivo Is Nothing, "", MaestroActivo.vIdMetodoAplicadoDepreciacionMaestroActivo),
                    '                                       IIf(MaestroActivo.cIdFamiliaMaestroActivo Is Nothing, "", MaestroActivo.cIdFamiliaMaestroActivo), IIf(MaestroActivo.vIdEstadoActivoFijoMaestroActivo Is Nothing, "", MaestroActivo.vIdEstadoActivoFijoMaestroActivo),
                    '                                       IIf(MaestroActivo.dFechaFinalizacionDepreciacionMaestroActivo Is Nothing, "01/01/1900", MaestroActivo.dFechaFinalizacionDepreciacionMaestroActivo), IIf(MaestroActivo.cIdTipoMonedaMaestroActivo Is Nothing, "", MaestroActivo.cIdTipoMonedaMaestroActivo),
                    '                                       IIf(MaestroActivo.nSaldoInicialMaestroActivo Is Nothing, "0", MaestroActivo.nSaldoInicialMaestroActivo), IIf(MaestroActivo.vIdEstadoOperacionMaestroActivo Is Nothing, "", MaestroActivo.vIdEstadoOperacionMaestroActivo),
                    '                                       IIf(MaestroActivo.cIdEstadoComponenteMaestroActivo Is Nothing, "", MaestroActivo.cIdEstadoComponenteMaestroActivo), IIf(MaestroActivo.vModeloMaestroActivo Is Nothing, "", MaestroActivo.vModeloMaestroActivo), IIf(MaestroActivo.vNumeroSerieMaestroActivo Is Nothing, "", MaestroActivo.vNumeroSerieMaestroActivo),
                    '                                       IIf(MaestroActivo.vPlacaMaestroActivo Is Nothing, "", MaestroActivo.vPlacaMaestroActivo), IIf(MaestroActivo.nCantidadUsuariosMaestroActivo Is Nothing, "0", MaestroActivo.nCantidadUsuariosMaestroActivo),
                    '                                       IIf(MaestroActivo.nCantidadLectorasMaestroActivo Is Nothing, "0", MaestroActivo.nCantidadLectorasMaestroActivo), IIf(MaestroActivo.nCantidadContadoresMaestroActivo Is Nothing, "0", MaestroActivo.nCantidadContadoresMaestroActivo),
                    '                                       IIf(MaestroActivo.vNumeroContratoArrendamientoMaestroActivo Is Nothing, "", MaestroActivo.vNumeroContratoArrendamientoMaestroActivo), IIf(MaestroActivo.dFechaContratoArrendamientoMaestroActivo Is Nothing, "01/01/1900", MaestroActivo.dFechaContratoArrendamientoMaestroActivo),
                    '                                       IIf(MaestroActivo.nNumeroCuotasPactadasMaestroActivo Is Nothing, "0", MaestroActivo.nNumeroCuotasPactadasMaestroActivo), IIf(MaestroActivo.cIdAlmacen Is Nothing, "", MaestroActivo.cIdAlmacen), IIf(MaestroActivo.cIdTipoAlmacen Is Nothing, "", MaestroActivo.cIdTipoAlmacen),
                    '                                       Session("CestaEquipoComponente"))
                    Dim TablaCestaMaestro As DataTable
                    TablaCestaMaestro = Session("CestaEquipoComponente")
                    Dim i As Integer
                    'jyuijan@redrilsa.com.pe
                    'Juan Jose Yuijan.
                    'Coordinar tema de registro de información.
                    For i = 0 To TablaCestaMaestro.Rows.Count - 1
                        If TablaCestaMaestro.Rows(i)("IdTipoActivo") = cboTipoActivo.SelectedValue And
                           TablaCestaMaestro.Rows(i)("IdCatalogo") = cboCatalogo.SelectedValue Then
                            'TablaAreaElemento.Rows(i)("IdPerfil") = txtIdPerfil.Text And _
                            QuitarCesta(intContador, Session("CestaCatalogoComponente"))
                            intContador = intContador - 1
                        End If
                    Next
                    intContador = intContador + 1
                Next
            End If
            'End If
            'Me.grdComponenteMaestroActivo.DataSource = Session("CestaEquipoComponente")
            'Me.grdComponenteMaestroActivo.DataBind()
            Me.grdComponenteEquipo.DataSource = Session("CestaEquipoComponente")
            Me.grdComponenteEquipo.DataBind()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltroEquipo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroEquipo.DataValueField = "vValor"
        'cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("14", "LOGI")
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("83", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
    End Sub

    'Sub ListarTipoActivoCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
    '    cboTipoActivo.DataTextField = "vDescripcionUbicacionGeografica"
    '    cboTipoActivo.DataValueField = "cIdDistritoUbicacionGeografica"
    '    cboTipoActivo.DataSource = UbicacionGeograficaNeg.DistritoListarCombo(cboPais.SelectedValue, cboDepartamento.SelectedValue, cboProvincia.SelectedValue)
    '    cboTipoActivo.Items.Clear()
    '    cboTipoActivo.DataBind()
    'End Sub
    Sub ListarTipoActivoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoActivoNeg As New clsTipoActivoNegocios
        cboTipoActivo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivo.DataValueField = "cIdTipoActivo"
        'cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo(Session("IdEmpresa"), Session("IdPuntoVenta"))
        cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        cboTipoActivo.Items.Clear()
        cboTipoActivo.Items.Add("SELECCIONE DATO")
        cboTipoActivo.DataBind()

        cboTipoActivoMantenimientoCatalogo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoMantenimientoCatalogo.DataValueField = "cIdTipoActivo"
        'cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo(Session("IdEmpresa"), Session("IdPuntoVenta"))
        cboTipoActivoMantenimientoCatalogo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        cboTipoActivoMantenimientoCatalogo.Items.Clear()
        cboTipoActivoMantenimientoCatalogo.Items.Add("SELECCIONE DATO")
        cboTipoActivoMantenimientoCatalogo.DataBind()

        cboTipoActivoDetalleCatalogo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoDetalleCatalogo.DataValueField = "cIdTipoActivo"
        cboTipoActivoDetalleCatalogo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        cboTipoActivoDetalleCatalogo.Items.Clear()
        cboTipoActivoDetalleCatalogo.Items.Add("SELECCIONE DATO")
        cboTipoActivoDetalleCatalogo.DataBind()

        'cboTipoActivoMaestroActivoPrincipal.DataTextField = "vDescripcionTipoActivo"
        'cboTipoActivoMaestroActivoPrincipal.DataValueField = "cIdTipoActivo"
        'cboTipoActivoMaestroActivoPrincipal.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        'cboTipoActivoMaestroActivoPrincipal.DataBind()

        'cboTipoActivoDetalleMaestroActivo.DataTextField = "vDescripcionTipoActivo"
        'cboTipoActivoDetalleMaestroActivo.DataValueField = "cIdTipoActivo"
        'cboTipoActivoDetalleMaestroActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        'cboTipoActivoDetalleMaestroActivo.DataBind()
    End Sub

    Sub ListarCatalogoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CatalogoNeg As New clsCatalogoNegocios
        cboCatalogo.DataTextField = "vDescripcionCatalogo"
        cboCatalogo.DataValueField = "cIdCatalogo"
        cboCatalogo.DataSource = CatalogoNeg.CatalogoListarCombo(0, cboTipoActivo.SelectedValue)
        cboCatalogo.Items.Clear()
        cboCatalogo.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboCatalogo.DataBind()
    End Sub

    Sub ListarSistemaFuncionalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncional.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncional.DataValueField = "cIdSistemaFuncional"
        'cboSistemaFuncionalDetalleMaestroActivo.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo(cboTipoActivo.SelectedValue)
        cboSistemaFuncional.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo(cboTipoActivo.SelectedValue, cboCatalogo.SelectedValue)
        cboSistemaFuncional.Items.Clear()
        cboSistemaFuncional.DataBind()
    End Sub

    Sub ListarSistemaFuncionalDetalleCatalogoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncionalDetalleCatalogo.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncionalDetalleCatalogo.DataValueField = "cIdSistemaFuncional"
        cboSistemaFuncionalDetalleCatalogo.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo(cboTipoActivo.SelectedValue, cboCatalogo.SelectedValue)
        cboSistemaFuncionalDetalleCatalogo.Items.Clear()
        cboSistemaFuncionalDetalleCatalogo.DataBind()
    End Sub

    'Sub ListarCatalogoComponenteCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
    '    cboCatalogoComponente.DataTextField = "vDescripcionCatalogo"
    '    cboCatalogoComponente.DataValueField = "cIdCatalogo"
    '    cboCatalogoComponente.DataSource = CatalogoNeg.CatalogoListarCombo(0, cboTipoActivo.SelectedValue)
    '    cboCatalogoComponente.Items.Clear()
    '    cboCatalogoComponente.DataBind()
    'End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdEquipo.Enabled = False 'bActivar
        'cboTipoActivo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        'cboCatalogo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        txtDescripcionEquipo.Enabled = bActivar
        fscIdentificadorComponente.Disabled = Not bActivar
        cboTipoActivo.Enabled = bActivar
        cboCatalogo.Enabled = bActivar
        cboSistemaFuncional.Enabled = bActivar
        'cboCatalogoComponente.Enabled = bActivar
        txtIdCliente.Enabled = bActivar
        txtRazonSocial.Enabled = False
        'cboEmpresa.Visible = IIf(Session("IdTipoUsuario") = "U", False, True)

        'Mantenimiento Tipo Activo
        txtIdTipoActivoMantenimientoTipoActivo.Enabled = False
        txtDescripcionMantenimientoTipoActivo.Enabled = bActivar
        txtDescripcionAbreviadaMantenimientoTipoActivo.Enabled = bActivar

        'Mantenimiento de Catalogo
        txtIdCatalogoMantenimientoCatalogo.Enabled = False
        cboTipoActivoMantenimientoCatalogo.Enabled = bActivar
        txtDescripcionMantenimientoCatalogo.Enabled = bActivar
        txtDescripcionAbreviadaMantenimientoCatalogo.Enabled = bActivar
        txtVidaUtilMantenimientoCatalogo.Enabled = bActivar
        txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        txtCuentaContableMantenimientoCatalogo.Enabled = bActivar
        divCuentaContableMantenimientoCatalogo.Visible = False
        txtCuentaContableLeasingMantenimientoCatalogo.Enabled = bActivar
        divCuentaContableLeasingMantenimientoCatalogo.Visible = False

        'Mantenimiento de Catalogo / Características

        'Mantenimiento de Equipo

        'Mantenimiento de Equipo / Componente

        'Mantenimiento de Catalogo / Componente
        txtIdDetalleCatalogo.Enabled = False
        cboTipoActivoDetalleCatalogo.Enabled = bActivar
        cboSistemaFuncionalDetalleCatalogo.Enabled = bActivar
        txtDescripcionDetalleCatalogo.Enabled = bActivar
        txtDescripcionDetalleCatalogo.Enabled = bActivar
        txtVidaUtilDetalleCatalogo.Enabled = bActivar
        txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        txtCuentaContableDetalleCatalogo.Enabled = bActivar
        txtCuentaContableLeasingDetalleCatalogo.Enabled = bActivar

        'Mantenimiento de Sistema Funcional / Componente
        txtIdSistemaFuncionalMantenimientoSistemaFuncional.Enabled = False
        txtDescripcionMantenimientoSistemaFuncional.Enabled = bActivar
        txtDescripcionAbreviadaMantenimientoSistemaFuncional.Enabled = bActivar
    End Sub

    Sub LlenarData()
        Try
            'Dim Equipo As LOGI_EQUIPO = EquipoNeg.MaestroActivoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
            'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
            Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim)
            txtIdEquipo.Text = Equipo.cIdEquipo
            txtDescripcionEquipo.Text = Equipo.vDescripcionEquipo
            'txtDescripcionAbreviada.Text = MaestroActivo.vDescripcionAbreviadaMaestroActivo
            'txtCuentaContableActivoMayor.Text = MaestroActivo.cIdCuentaContableMaestroActivo
            'txtObservacion.Text = MaestroActivo.vObservacionMaestroActivo
            cboTipoActivo.SelectedValue = Equipo.cIdTipoActivo
            cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())
            cboCatalogo.SelectedValue = Equipo.cIdCatalogo
            ListarSistemaFuncionalCombo()
            'JMUG: 25/02/2023 lblSistemaFuncional.Value = Equipo.cIdSistemaFuncional
            'cboSistemaFuncional.SelectedValue = MaestroActivo.cIdSistemaFuncional
            'cboTipoActivoDetalleMaestroActivo_SelectedIndexChanged(cboTipoActivoDetalleMaestroActivo, New System.EventArgs())

            'JMUG: 25/02/2023 LlenarDataEquipo()

            CargarCestaComponenteCatalogo()
            CargarCestaEquipoComponente()
            'CargarCestaMaestroActivo()

            'MaestroActivo.cIdCodigoEnlaceMaestroActivo
            'MaestroActivo.cIdJerarquiaMaestroActivo
            'MaestroActivo.cIdTipoActivoMaestroActivo
            If MyValidator.ErrorMessage = "" Then
                'lblMensaje.Text = "Registro encontrado con éxito"
                MyValidator.ErrorMessage = "Registro encontrado con éxito"
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub LlenarDataTipoActivo()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosTipoActivo()
            Else
                LimpiarObjetosTipoActivo()
                Dim TipoActivoNeg As New clsTipoActivoNegocios
                Dim TipoActivo As LOGI_TIPOACTIVO = TipoActivoNeg.TipoActivoListarPorId(cboTipoActivo.SelectedValue)
                txtIdTipoActivoMantenimientoTipoActivo.Text = TipoActivo.cIdTipoActivo
                txtDescripcionMantenimientoTipoActivo.Text = TipoActivo.vDescripcionTipoActivo
                txtDescripcionAbreviadaMantenimientoTipoActivo.Text = TipoActivo.vDescripcionAbreviadaTipoActivo
            End If
            'If MyValidator.ErrorMessage = "" Then
            '    MyValidator.ErrorMessage = "Registro encontrado con éxito"
            'End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub LlenarDataCatalogo()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosCatalogo()
            Else
                'Dim Catalogo As New LOGI_CATALOGO
                'If grdLista.Rows.Count > 0 Then
                '    If IsNothing(grdLista.SelectedRow) = True Then
                '        'MaestroActivo = MaestroActivoNeg.MaestroActivoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
                '        Catalogo = CatalogoNeg.CatalogoListarPorId(cboCatalogo.SelectedValue, cboTipoActivo.SelectedValue, "0", "1")
                '    End If
                'End If
                LimpiarObjetosCatalogo()
                Dim CatalogoNeg As New clsCatalogoNegocios
                Dim Catalogo = CatalogoNeg.CatalogoListarPorId(cboCatalogo.SelectedValue, cboTipoActivo.SelectedValue, "0", "1")

                txtIdCatalogoMantenimientoCatalogo.Text = Catalogo.cIdCatalogo
                txtDescripcionMantenimientoCatalogo.Text = Catalogo.vDescripcionCatalogo
                txtDescripcionAbreviadaMantenimientoCatalogo.Text = Catalogo.vDescripcionAbreviadaCatalogo
                txtVidaUtilMantenimientoCatalogo.Text = IIf(Catalogo.nVidaUtilCatalogo Is Nothing, "0", Catalogo.nVidaUtilCatalogo)
                txtPeriodoGarantiaMantenimientoCatalogo.Text = Catalogo.nPeriodoGarantiaCatalogo
                txtPeriodoMinimoMantenimientoCatalogo.Text = Catalogo.nPeriodoMinimoMantenimientoCatalogo
                txtCuentaContableMantenimientoCatalogo.Text = Catalogo.cIdCuentaContableCatalogo
                txtCuentaContableLeasingMantenimientoCatalogo.Text = Catalogo.cIdCuentaContableLeasingCatalogo
            End If
            'CargarCestaComponenteCatalogo()
            'CargarCestaEquipoComponente()
            'CargarCestaMaestroActivo()
            CargarCestaCatalogoCaracteristica()

            'MaestroActivo.cIdCodigoEnlaceMaestroActivo
            'MaestroActivo.cIdJerarquiaMaestroActivo
            'MaestroActivo.cIdTipoActivoMaestroActivo
            'If MyValidator.ErrorMessage = "" Then
            '    MyValidator.ErrorMessage = "Registro encontrado con éxito"
            'End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub LlenarDataDetalleCatalogo()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosDetalleCatalogo()
            Else
                LimpiarObjetosDetalleCatalogo()
                'txtIdDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim
                txtIdDetalleCatalogo.Text = Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(2).Text).Trim
                'txtDescripcionDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(3).Text)
                txtDescripcionDetalleCatalogo.Text = Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(3).Text)
                'txtDescripcionAbreviadaDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(4).Text)
                txtDescripcionAbreviadaDetalleCatalogo.Text = Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(4).Text)
                'cboTipoActivoDetalleCatalogo.SelectedValue = grdListaDetalle.SelectedRow.Cells(5).Text
                cboTipoActivoDetalleCatalogo.SelectedValue = grdComponenteCatalogo.SelectedRow.Cells(5).Text
                'cboSistemaFuncionalDetalleCatalogo.SelectedValue = grdListaDetalle.SelectedRow.Cells(7).Text
                cboSistemaFuncionalDetalleCatalogo.SelectedValue = grdComponenteCatalogo.SelectedRow.Cells(7).Text
                'txtCuentaContableDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(9).Text)
                txtCuentaContableDetalleCatalogo.Text = Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(9).Text)
                'txtCuentaContableLeasingDetalleCatalogo.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(10).Text)
                txtCuentaContableLeasingDetalleCatalogo.Text = Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(10).Text)
                txtVidaUtilDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
            End If
            CargarCestaCatalogoComponenteCaracteristica()
            'LimpiarObjetos()
            'If hfdOperacionDetalle.Value = "E" Then
            '    txtVidaUtilDetalleCatalogo.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
            'Else
            '    Dim Catalogo = CatalogoNeg.CatalogoListarPorId(txtIdDetalleCatalogo.Text, cboTipoActivoDetalleCatalogo.SelectedValue, "1", "1")
            '    txtVidaUtilDetalleCatalogo.Text = IIf(Catalogo.nVidaUtilCatalogo Is Nothing, "0", Catalogo.nVidaUtilCatalogo)
            'End If
        Catch ex As Exception
            lblMensajeDetalleCatalogo.Text = ex.Message
        End Try
    End Sub


    'Sub LlenarDataEquipo()
    '    Try
    '        If hfdOperacionEquipo.Value = "N" Then
    '            Dim CatalogoNeg As New clsCatalogoNegocios
    '            'Dim Catalogo = CatalogoNeg.CatalogoListarPorId(Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(1).Text), Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(3).Text), "1", "*")
    '            Dim Catalogo = CatalogoNeg.CatalogoListarPorId(cboCatalogoMaestroActivoPrincipal.SelectedValue, cboTipoActivoMaestroActivoPrincipal.SelectedValue, "0", "1")

    '            txtIdMaestroActivoPrincipal.Text = ""
    '            txtDescripcionMaestroActivoPrincipal.Text = txtDescripcion.Text
    '            'cboTipoActivoMaestroActivoPrincipal.SelectedValue = Catalogo.cIdTipoActivo
    '            'cboTipoActivoMaestroActivoPrincipal_SelectedIndexChanged(cboTipoActivoMaestroActivoPrincipal, New System.EventArgs())
    '            'cboSistemaFuncionalMaestroActivoPrincipal.SelectedValue = Server.HtmlDecode(grdComponenteCatalogo.SelectedRow.Cells(4).Text)
    '            ListarFamiliaPrincipalCombo()
    '            txtDescripcionAbreviadaMaestroActivoPrincipal.Text = txtDescripcionAbreviada.Text
    '            txtFechaAdquisicionMaestroActivoPrincipal.Text = "" 'IIf(Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("FechaAdquisicion") = "01/01/1900", "", Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("FechaAdquisicion"))
    '            Dim TipMonNeg As New clsTipoMonedaNegocios
    '            cboTipoMonedaMaestroActivoPrincipal.SelectedValue = TipMonNeg.TipoMonedaBase(Session("IdEmpresa"))
    '            txtCostoInicialMaestroActivoPrincipal.Text = "0" 'Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("CostoInicial").ToString.Trim
    '            txtObservacionAbreviadaMaestroActivoPrincipal.Text = txtObservacion.Text 'Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("Observacion").ToString.Trim
    '            chkLeasingMaestroActivoPrincipal.Checked = False 'grdComponenteMaestroActivo.SelectedRow.Cells(7).Text
    '            Dim CuentaNeg As New clsCatalogoNegocios
    '            txtCuentaContableMaestroActivoPrincipal.Text = IIf(chkLeasingMaestroActivoPrincipal.Checked = True, CuentaNeg.CatalogoListarPorId(cboCatalogoMaestroActivoPrincipal.SelectedValue, cboTipoActivoMaestroActivoPrincipal.SelectedValue, "0", "1").cIdCuentaContableLeasingCatalogo,
    '                                                                       CuentaNeg.CatalogoListarPorId(cboCatalogoMaestroActivoPrincipal.SelectedValue, cboTipoActivoMaestroActivoPrincipal.SelectedValue, "0", "1").cIdCuentaContableCatalogo)
    '            txtMarcaMaestroActivoPrincipal.Text = "" 'Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("Marca").ToString.Trim
    '            txtCaracteristicasMaestroActivoPrincipal.Text = "" 'Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("Caracteristicas").ToString.Trim
    '            txtVidaUtilMaestroActivoPrincipal.Text = IIf(Catalogo.nVidaUtilCatalogo Is Nothing, "0", Catalogo.nVidaUtilCatalogo) 'Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("VidaUtil").ToString.Trim
    '            txtDimensionesMaestroActivoPrincipal.Text = Trim(Catalogo.vDimensionesCatalogo) 'Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("Dimensiones").ToString.Trim
    '            txtVoltajeMaestroActivoPrincipal.Text = Trim(Catalogo.vVoltajeCatalogo) 'Session("CestaEquipoComponente").Rows(grdComponenteMaestroActivo.SelectedIndex)("Voltaje").ToString.Trim
    '            txtPesoMaestroActivoPrincipal.Text = Trim(Catalogo.vPesoCatalogo)
    '        Else
    '            Dim MaestroActivo As New LOGI_MAESTROACTIVO
    '            If grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = True Then
    '                    MaestroActivo = MaestroActivoNeg.MaestroActivoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
    '                End If
    '            End If

    '            'Inicio: Datos del Panel Activo Principal
    '            txtIdMaestroActivoPrincipal.Text = MaestroActivo.cIdMaestroActivo
    '            txtIdInternoMaestroActivoPrincipal.Text = MaestroActivo.vIdEquivalenciaMaestroActivo
    '            cboMetodoDepreciacionMaestroActivoPrincipal.SelectedValue = MaestroActivo.vIdMetodoAplicadoDepreciacionMaestroActivo
    '            cboTipoActivoMaestroActivoPrincipal.SelectedValue = MaestroActivo.cIdTipoActivo
    '            ListarFamiliaPrincipalCombo()
    '            cboFamiliaMaestroActivoPrincipal.SelectedValue = MaestroActivo.cIdFamiliaMaestroActivo
    '            txtDescripcionMaestroActivoPrincipal.Text = MaestroActivo.vDescripcionMaestroActivo
    '            txtDescripcionAbreviadaMaestroActivoPrincipal.Text = MaestroActivo.vDescripcionAbreviadaMaestroActivo
    '            cboEstadoActivoFijoMaestroActivoPrincipal.SelectedIndex = 2
    '            If Not MaestroActivo.vIdEstadoActivoFijoMaestroActivo Is Nothing Then
    '                cboEstadoActivoFijoMaestroActivoPrincipal.SelectedValue = MaestroActivo.vIdEstadoActivoFijoMaestroActivo
    '            End If
    '            txtFechaAdquisicionMaestroActivoPrincipal.Text = IIf(MaestroActivo.dFechaAdquisicionMaestroActivo.ToString.Trim = "", Nothing, CDate(IIf(MaestroActivo.dFechaAdquisicionMaestroActivo.ToString.Trim = "", "01/01/1900", (MaestroActivo.dFechaAdquisicionMaestroActivo))))
    '            txtFechaActivacionMaestroActivoPrincipal.Text = IIf(MaestroActivo.dFechaActivacionMaestroActivo.ToString.Trim = "", Nothing, CDate(IIf(MaestroActivo.dFechaActivacionMaestroActivo.ToString.Trim = "", "01/01/1900", (MaestroActivo.dFechaActivacionMaestroActivo)))) 'MaestroActivo.dFechaActivacionMaestroActivo
    '            txtFechaFinalizacionMaestroActivoPrincipal.Text = IIf(MaestroActivo.dFechaFinalizacionDepreciacionMaestroActivo.ToString.Trim = "", Nothing, CDate(IIf(MaestroActivo.dFechaFinalizacionDepreciacionMaestroActivo.ToString.Trim = "", "01/01/1900", (MaestroActivo.dFechaFinalizacionDepreciacionMaestroActivo)))) 'MaestroActivo.dFechaFinalizacionDepreciacionMaestroActivo
    '            If Not MaestroActivo.cIdTipoMonedaMaestroActivo Is Nothing Then
    '                cboTipoMonedaMaestroActivoPrincipal.SelectedValue = MaestroActivo.cIdTipoMonedaMaestroActivo
    '            Else
    '                Dim TipMonNeg As New clsTipoMonedaNegocios
    '                cboTipoMonedaMaestroActivoPrincipal.SelectedValue = TipMonNeg.TipoMonedaBase(Session("IdEmpresa"))
    '            End If
    '            txtCostoInicialMaestroActivoPrincipal.Text = IIf(MaestroActivo.nCostoInicialMaestroActivo Is Nothing, "0.00", MaestroActivo.nCostoInicialMaestroActivo)
    '            txtSaldoInicialMaestroActivoPrincipal.Text = IIf(MaestroActivo.nSaldoInicialMaestroActivo Is Nothing, "0.00", MaestroActivo.nSaldoInicialMaestroActivo)
    '            txtVidaUtilMaestroActivoPrincipal.Text = IIf(MaestroActivo.nVidaUtilMaestroActivo Is Nothing, "0", MaestroActivo.nVidaUtilMaestroActivo)
    '            cboEstadoOperacionMaestroActivoPrincipal.SelectedValue = MaestroActivo.vIdEstadoOperacionMaestroActivo
    '            cboEstadoActivoMaestroActivoPrincipal.SelectedValue = MaestroActivo.cIdEstadoComponenteMaestroActivo
    '            txtMarcaMaestroActivoPrincipal.Text = Trim(MaestroActivo.vMarcaMaestroActivo)
    '            txtDimensionesMaestroActivoPrincipal.Text = Trim(MaestroActivo.vDimensionesMaestroActivo)
    '            txtVoltajeMaestroActivoPrincipal.Text = Trim(MaestroActivo.vVoltajeMaestroActivo)
    '            txtPesoMaestroActivoPrincipal.Text = Trim(MaestroActivo.vPesoMaestroActivo)
    '            txtModeloMaestroActivoPrincipal.Text = Trim(MaestroActivo.vModeloMaestroActivo)
    '            txtNroSerieMaestroActivoPrincipal.Text = Trim(MaestroActivo.vNumeroSerieMaestroActivo)
    '            txtPlacaMaestroActivoPrincipal.Text = Trim(MaestroActivo.vPlacaMaestroActivo)
    '            chkLeasingMaestroActivoPrincipal.Checked = IIf(MaestroActivo.bEstadoLeasingMaestroActivo Is Nothing, False, MaestroActivo.bEstadoLeasingMaestroActivo)
    '            txtCuentaContableMaestroActivoPrincipal.Text = Trim(MaestroActivo.cIdCuentaContableMaestroActivo)
    '            txtNroUsuarioMaestroActivoPrincipal.Text = IIf(MaestroActivo.nCantidadUsuariosMaestroActivo Is Nothing, "0", MaestroActivo.nCantidadUsuariosMaestroActivo)
    '            txtNroLectoraMaestroActivoPrincipal.Text = IIf(MaestroActivo.nCantidadLectorasMaestroActivo Is Nothing, "0", MaestroActivo.nCantidadLectorasMaestroActivo)
    '            txtNroContadoresMaestroActivoPrincipal.Text = IIf(MaestroActivo.nCantidadContadoresMaestroActivo Is Nothing, "0", MaestroActivo.nCantidadContadoresMaestroActivo)
    '            txtNroContratoArrendamientoMaestroActivoPrincipal.Text = MaestroActivo.vNumeroContratoArrendamientoMaestroActivo
    '            txtFechaContratoArrendamientoMaestroActivoPrincipal.Text = IIf(MaestroActivo.dFechaContratoArrendamientoMaestroActivo.ToString.Trim = "", Nothing, CDate(IIf(MaestroActivo.dFechaContratoArrendamientoMaestroActivo.ToString.Trim = "", "01/01/1900", (MaestroActivo.dFechaContratoArrendamientoMaestroActivo))))
    '            txtNroCuotasArrendamientoMaestroActivoPrincipal.Text = IIf(MaestroActivo.nNumeroCuotasPactadasMaestroActivo Is Nothing, "0", MaestroActivo.nNumeroCuotasPactadasMaestroActivo)
    '            txtCaracteristicasMaestroActivoPrincipal.Text = MaestroActivo.vCaracteristicasMaestroActivo
    '            txtObservacionAbreviadaMaestroActivoPrincipal.Text = MaestroActivo.vObservacionMaestroActivo
    '            hfdIdAlmacen.Value = MaestroActivo.cIdAlmacen
    '            hfdIdTipoAlmacen.Value = MaestroActivo.cIdTipoAlmacen
    '            'Final: Datos del Panel Activo Principal
    '        End If
    '        'CargarCestaComponenteCatalogo()
    '        'CargarCestaEquipoComponente()
    '        'CargarCestaMaestroActivo()

    '        'MaestroActivo.cIdCodigoEnlaceMaestroActivo
    '        'MaestroActivo.cIdJerarquiaMaestroActivo
    '        'MaestroActivo.cIdTipoActivoMaestroActivo
    '        If MyValidator.ErrorMessage = "" Then
    '            'lblMensaje.Text = "Registro encontrado con éxito"
    '            MyValidator.ErrorMessage = "Registro encontrado con éxito"
    '        End If
    '    Catch ex As Exception
    '        'lblMensaje.Text = ex.Message
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvDescripcionEquipo.EnableClientScript = bValidar
        Me.rfvCatalogo.EnableClientScript = bValidar
        Me.rfvSistemaFuncional.EnableClientScript = bValidar
        Me.rfvIdCliente.EnableClientScript = bValidar
        Me.rfvRazonSocial.EnableClientScript = bValidar
        'Me.rfvPerfil.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        'Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdEquipo.Text = ""
        txtDescripcionEquipo.Text = ""
        'fscIdentificadorComponente.Checked = False
        'txtApellidoPaterno.Text = ""
        'txtNombres.Text = ""
        'txtPassword.Text = ""
        ''cboPerfil.SelectedIndex = -1
        'txtCargo.Text = ""
        'chkLectura.Checked = False
        'chkEscritura.Checked = False
        'chkEjecucion.Checked = False
        ''cboTipo.SelectedIndex = -1
        ''chkEstado.Checked = False
        ''txtIPImpresion.Text = ""
        cboTipoActivo.SelectedIndex = -1
        cboCatalogo.SelectedIndex = -1
        cboSistemaFuncional.SelectedIndex = -1
        hfdEstado.Value = "1"
    End Sub

    Sub LimpiarObjetosTipoActivo()
        txtIdTipoActivoMantenimientoTipoActivo.Text = ""
        txtDescripcionMantenimientoTipoActivo.Text = ""
        txtDescripcionAbreviadaMantenimientoTipoActivo.Text = ""
    End Sub

    Sub LimpiarObjetosSistemaFuncional()
        txtIdSistemaFuncionalMantenimientoSistemaFuncional.Text = ""
        txtDescripcionMantenimientoSistemaFuncional.Text = ""
        txtDescripcionAbreviadaMantenimientoSistemaFuncional.Text = ""
    End Sub

    Sub LimpiarObjetosCatalogo()
        txtIdCatalogoMantenimientoCatalogo.Text = ""
        'cboTipoActivoMantenimientoCatalogo.SelectedIndex = -1
        txtDescripcionMantenimientoCatalogo.Text = ""
        txtDescripcionAbreviadaMantenimientoCatalogo.Text = ""
        txtVidaUtilMantenimientoCatalogo.Text = ""
        txtPeriodoGarantiaMantenimientoCatalogo.Text = ""
        txtPeriodoMinimoMantenimientoCatalogo.Text = ""
        txtCuentaContableMantenimientoCatalogo.Text = ""
        txtCuentaContableLeasingMantenimientoCatalogo.Text = ""
        clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
        grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
        grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
    End Sub

    Sub LimpiarObjetosDetalleCatalogo()
        MyValidator.ErrorMessage = ""
        txtIdDetalleCatalogo.Text = ""
        cboTipoActivoDetalleCatalogo.SelectedIndex = -1
        cboSistemaFuncionalDetalleCatalogo.SelectedIndex = -1
        txtDescripcionDetalleCatalogo.Text = ""
        txtDescripcionAbreviadaDetalleCatalogo.Text = ""
        txtVidaUtilDetalleCatalogo.Text = ""
        txtCuentaContableDetalleCatalogo.Text = ""
        txtCuentaContableLeasingDetalleCatalogo.Text = ""
    End Sub

    Protected Sub Upload_File(sender As Object, e As EventArgs)
        If hfdOperacion.Value = "N" Then
            'GenerarComprobante()
        End If
        btnNuevo.Enabled = True
    End Sub

    Sub LimpiarObjetosCaracteristicas()
        Me.lblMensajeCaracteristica.Text = ""
        txtIdReferenciaSAPCaracteristica.Text = ""
        txtDescripcionCampoSAPCaracteristica.Text = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "035" 'Mantenimiento de los Maestros Activos / Componente.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroEquipo.SelectedIndex = 3
            ListarTipoActivoCombo()
            ListarCatalogoCombo()
            ListarSistemaFuncionalCombo()
            'ListarTipoActivoCombo()
            'ListarSistemaFuncionalCombo()
            'ListarTipoMonedaCombo()

            'Dim TipMonNeg As New clsTipoMonedaNegocios
            'cboTipoMonedaMaestroActivoPrincipal.SelectedValue = TipMonNeg.TipoMonedaBase(Session("IdEmpresa"))
            'cboTipoMonedaDetalleMaestroActivo.SelectedValue = TipMonNeg.TipoMonedaBase(Session("IdEmpresa"))

            'ListarMetodoDepreciacionCombo()
            'ListarEstadoActivoCombo()
            'ListarIndicadorOperacionCombo()
            'ListarEstadoActivoSunatCombo()


            If Session("CestaCatalogoCaracteristica") Is Nothing Then
                Session("CestaCatalogoCaracteristica") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            End If
            'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            'Me.grdDetalleCaracteristica.DataBind()


            If Session("CestaCatalogoComponente") Is Nothing Then
                Session("CestaCatalogoComponente") = CrearCesta()
            Else
                VaciarCesta(Session("CestaCatalogoComponente"))
            End If

            If Session("CestaEquipoComponente") Is Nothing Then
                Session("CestaEquipoComponente") = clsLogiCestaEquipo.CrearCesta()
            Else
                clsLogiCestaEquipo.VaciarCesta(Session("CestaEquipoComponente"))
            End If

            'BloquearPagina(1)
            'BloquearMantenimiento(True, False, True, False)

            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlEquipo.Visible = False
            pnlComponentes.Visible = False


        Else
            txtBuscarEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            txtIdEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_cboFamilia')")
            cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcion')")
            txtDescripcionEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'If lblOperacion.Value = "E" Or lblOperacion.Value = "N" Then
            '    BloquearPagina(2)
            'End If
        End If
    End Sub

    Protected Sub cboTipoActivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivo.SelectedIndexChanged
        cboTipoActivoMantenimientoCatalogo.SelectedValue = cboTipoActivo.SelectedValue
        cboTipoActivoDetalleCatalogo.SelectedValue = cboTipoActivo.SelectedValue
        ListarCatalogoCombo()
        CargarCestaComponenteCatalogo()
        'CargarCestaMaestroActivo()

        CargarCestaEquipoComponente()

    End Sub


    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        pnlCabecera.Visible = True
        pnlEquipo.Visible = False
        pnlComponentes.Visible = False
    End Sub

    'Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click

    'End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                Throw New Exception("Este registro se encuentra desactivado.")
            End If
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0075", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "E"
            'If BloquearPagina(2) = False Then
            BloquearMantenimiento(False, True, False, True)
            ValidarTexto(True)
            ActivarObjetos(True)
            LlenarData()
            'JMUG: 25/02/2023 hfTab.Value = "tab2"
            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            'End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text) = "True", "1", "0")
            'JMUG: 25/02/2023 BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    'Private Sub btnAdicionarTipoActivo_Click(sender As Object, e As EventArgs) Handles btnAdicionarTipoActivo.Click
    '    hfdOperacionDetalle.Value = "N"
    '    lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender.Show()
    'End Sub

    Private Sub btnAceptarMantenimientoTipoActivo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoTipoActivo.Click
        Try
            'Función para validar si tiene permisos
            'lblMensaje.Text = ""
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0364", strOpcionModulo, Session("IdSistema"))

            Dim TipoActivoNeg As New clsTipoActivoNegocios
            Dim TipoActivo As New LOGI_TIPOACTIVO
            TipoActivo.cIdTipoActivo = txtIdTipoActivoMantenimientoTipoActivo.Text
            TipoActivo.vDescripcionTipoActivo = UCase(IIf(txtDescripcionMantenimientoTipoActivo.Text = "", "", txtDescripcionMantenimientoTipoActivo.Text))
            TipoActivo.vDescripcionAbreviadaTipoActivo = UCase(IIf(txtDescripcionAbreviadaMantenimientoTipoActivo.Text = "", "", txtDescripcionAbreviadaMantenimientoTipoActivo.Text))
            TipoActivo.bEstadoRegistroTipoActivo = IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(IIf(hfdEstado.Value = "", "1", hfdEstado.Value))))

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacionDetalle.Value = "N" Then
                If (TipoActivoNeg.TipoActivoInserta(TipoActivo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_TIPOACTIVO 'SQL_INSERT', '','" & TipoActivo.cIdTipoActivo & "', '" &
                                       Session("IdEmpresa") & "', '" & Session("IdPuntoVenta") & "', '" & TipoActivo.vDescripcionTipoActivo & "', '" & TipoActivo.vDescripcionAbreviadaTipoActivo & "', '" & TipoActivo.bEstadoRegistroTipoActivo & "', '" & TipoActivo.cIdTipoActivo & "'"

                    LogAuditoria.vEvento = "INSERTAR TIPO DE ACTIVO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdTipoActivoMantenimientoTipoActivo.Text = TipoActivo.cIdTipoActivo
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    ListarTipoActivoCombo()
                    'Me.grdLista.DataSource = TipoActivoNeg.TipoActivoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, IIf(Session("IdTipoUsuario") = "A", "*", "1"))
                    'Me.grdLista.DataBind()
                    'pnlGeneral.Enabled = False
                    'BloquearMantenimiento(True, False, True, False)
                    'hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (TipoActivoNeg.TipoActivoEdita(TipoActivo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_TIPOACTIVO 'SQL_UPDATE', '','" & TipoActivo.cIdTipoActivo & "', '" &
                                       Session("IdEmpresa") & "', '" & Session("IdPuntoVenta") & "', '" & TipoActivo.vDescripcionTipoActivo & "', '" & TipoActivo.vDescripcionAbreviadaTipoActivo & "', '" & TipoActivo.bEstadoRegistroTipoActivo & "', '" & TipoActivo.cIdTipoActivo & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR TIPO DE ACTIVO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    'Me.grdLista.DataSource = TipoActivoNeg.TipoActivoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
                    'Me.grdLista.DataBind()
                    'pnlGeneral.Enabled = False
                    'BloquearMantenimiento(True, False, True, False)
                    'hfTab.Value = "tab1"
                    ListarTipoActivoCombo()
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")

                End If
            End If
            hfdOperacion.Value = "R"
            'BloquearPagina(0)
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Private Sub btnAdicionarCatalogo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCatalogo.Click
    '    hfdOperacionDetalle.Value = "N"
    '    LlenarDataCatalogo()
    '    lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
    'End Sub

    Private Sub btnAdicionarCaracteristicaMantenimientoCatalogo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaMantenimientoCatalogo.Click
        Try
            fValidarSesion()
            'JMUG: 26/02/2023 FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0635", strOpcionModulo, Session("IdSistema"))

            txtBuscarCaracteristica.Focus()
            Dim EmpresaNeg As New clsEmpresaNegocios
            Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(Session("IdEmpresa"))

            Me.grdListaCaracteristica.DataSource = CaracteristicaNeg.CaracteristicaListaBusqueda(cboFiltroCaracteristica.SelectedValue, txtBuscarCaracteristica.Text.Trim, "*")
            Me.grdListaCaracteristica.DataBind()
            Me.grdListaCaracteristica.SelectedIndex = -1

            LimpiarObjetosCaracteristicas()
            'Dim TipMonNeg As New clsTipoMonedaNegocios
            'Dim strTipoMonedaAbreviada As String
            'strTipoMonedaAbreviada = TipMonNeg.TipoMonedaListarPorId(cboTipoMoneda.SelectedValue).vDescripcionAbreviadaTipoMoneda
            'lblSaldoFormaPago.Text = "Saldo " & strTipoMonedaAbreviada
            'lblImporteTotalFormaPago.Text = "Imp. Total " & strTipoMonedaAbreviada
            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
        Catch ex As Exception
            lblMensajeCaracteristica.Text = ex.Message
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub


    Private Sub imgbtnBuscarCaracteristica_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCaracteristica.Click
        Me.grdListaCaracteristica.DataSource = CaracteristicaNeg.CaracteristicaListaBusqueda(cboFiltroCaracteristica.SelectedValue, txtBuscarCaracteristica.Text.Trim, "*")
        Me.grdListaCaracteristica.DataBind()
        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
        'Me.grdListaCaracteristica.SelectedIndex = -1
    End Sub

    Private Sub grdListaCaracteristica_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdListaCaracteristica.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeCaracteristica.Text = ""
            'If Session("IdEmpresa") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            '    Exit Sub
            'End If
            'LimpiarObjetosProducto()
            If grdListaCaracteristica IsNot Nothing Then
                If grdListaCaracteristica.Rows.Count > 0 Then
                    If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
                        If IsReference(grdListaCaracteristica.SelectedRow.Cells(1).Text) = True Then
                            txtValorCaracteristica.Text = ""
                            txtIdReferenciaSAPCaracteristica.Text = ""
                            txtDescripcionCampoSAPCaracteristica.Text = ""
                            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                        End If
                    End If
                Else
                    lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                End If
            End If
        Catch ex As Exception
            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
            lblMensajeCaracteristica.Text = ex.Message
        End Try
    End Sub

    Private Sub grdListaCaracteristica_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdListaCaracteristica.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdListaCaracteristica, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Código
            e.Row.Cells(1).Visible = True 'Descripción Característica
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Código
            e.Row.Cells(1).Visible = True 'Descripcion Característica
        End If
    End Sub

    Private Sub btnAceptarCaracteristica_Click(sender As Object, e As EventArgs) Handles btnAceptarCaracteristica.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeCaracteristica.Text = ""

            If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
                If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
                    If lblMensajeCaracteristica.Text = "" Then
                        'If Convert.ToDecimal(txtPrecioTotalProducto.Text) = Convert.ToDecimal("0.00") Then
                        '    Throw New Exception("Este producto no tiene precio.")
                        'ElseIf txtIdCliente.Text = "" Then
                        '    Throw New Exception("Debe de seleccionar el cliente para adicionar un producto.")
                        'ElseIf Trim(Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(2).Text)) = "S" And (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) = "" Then
                        '    Throw New Exception("Debe de seleccionar una variante para el producto.")
                        'End If
                        For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
                            If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (grdListaCaracteristica.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                                grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
                                grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
                                'TotalizarGrid()
                                'txtImporteFormaPagoOp1.Text = Math.Round(Convert.ToDecimal(txtTotalGeneral.Text), 2)
                                'LimpiarObjetosProducto()
                                LimpiarObjetosCaracteristicas()
                                'lnk_mostrarPanelProducto_ModalPopupExtender.Show()
                                lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                                grdListaCaracteristica.SelectedIndex = -1
                                lblMensajeCaracteristica.Text = "Característica ya registrada, seleccione otro item."
                                Exit Sub
                            End If
                        Next

                        'Dim EmpresaNeg As New clsEmpresaNegocios
                        'Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(Session("IdEmpresa"))

                        'Dim ListaPrecioProducto As New VTAS_LISTAPRECIO
                        'ListaPrecioProducto = ListaPrecioNeg.ListaPrecioListarPorIdTipoCliente(grdListaProducto.SelectedValue.ToString, cboTipoAlmacen.SelectedValue, cboAlmacen.SelectedValue, EmpresaNeg.EmpresaListarPorId(Session("IdEmpresa")).nIdConfiguracionListaPrecio, Session("IdEmpresa"), hfdIdTipoCliente.Value)

                        'Dim Producto As GNRL_PRODUCTO = ProductoNeg.ProductoListarPorId(grdListaProducto.SelectedValue.ToString, Session("IdEmpresa"), "*")

                        'Dim nImporteTotalISC As Decimal
                        'Dim nValorNetoTotal, nValorNetoTotalConDscto As Decimal
                        'If strTipoCantidad = "1" Then
                        '    nImporteTotalISC = hfdISCUnitarioProducto.Value * txtCantidadTotalProducto.Text
                        '    nValorNetoTotal = hfdValorNetoUnitarioProducto.Value * txtCantidadTotalProducto.Text
                        'ElseIf strTipoCantidad = "2" Then
                        '    nImporteTotalISC = hfdISCUnitarioProducto.Value * txtCantidadProducto.Text
                        '    nValorNetoTotal = hfdValorNetoUnitarioProducto.Value * txtCantidadProducto.Text
                        'End If
                        'nValorNetoTotalConDscto = nValorNetoTotal - hfdImporteDescuentoProducto.Value 'txtImporteDescuentoProducto.Text
                        'Dim dtVariantes = ComprobanteNeg.OfertaVentaGetData("SELECT COUNT(*) AS Cantidad FROM GNRL_PRODUCTOVARIANTE WHERE cIdProducto = '" & grdListaProducto.SelectedValue.ToString & "'")
                        'Dim lblDescripcionProducto As Label = TryCast(grdListaProducto.SelectedRow.Cells(1).FindControl("lblDescripcionProducto"), Label)
                        'clsVtasCestaComprobante.AgregarCesta(grdListaProducto.SelectedValue.ToString, IIf(strTipoCantidad = "2", txtCantidadProducto.Text, txtCantidadTotalProducto.Text), txtCantidadTotalProducto.Text, lblDescripcionProducto.Text.Trim, hfdIdUnidadMedida1Producto.Value,
                        '                                 IIf(strTipoCantidad = "2", hfdUnidadMedidaAbreviada2Producto.Value, hfdUnidadMedidaAbreviada1Producto.Value), cboTipoMoneda.SelectedValue, hfdPrecioUnitarioProducto.Value, hfdPrecioUnitarioProducto.Value, hfdPrecioUnitarioTotalProducto.Value, hfdImporteDescuentoProducto.Value, hfdTamanoPesoProducto.Value,
                        '                                 hfdIdUnidadMedida1Producto.Value, IIf(cboUnidadMedidaProducto.SelectedValue = hfdIdUnidadMedida2Producto.Value, Producto.nTamanoPesoProducto, txtCantidadProducto.Text), hfdIdUnidadMedida2Producto.Value, IIf(strTipoCantidad = "1", "1", txtCantidadProducto.Text),
                        '                                 Convert.ToDecimal(txtTipoCambio.Text), hfdPrecioTotalProducto.Value, IIf(hfdIdTipoAfectacionProducto.Value = "20" Or hfdIdTipoAfectacionProducto.Value = "21" Or hfdIdTipoAfectacionProducto.Value = "30" Or hfdIdTipoAfectacionProducto.Value = "31" Or hfdIdTipoAfectacionProducto.Value = "32" Or hfdIdTipoAfectacionProducto.Value = "33" Or hfdIdTipoAfectacionProducto.Value = "34" Or hfdIdTipoAfectacionProducto.Value = "35" Or hfdIdTipoAfectacionProducto.Value = "36", 0, Session("nImpuesto")), hfdIdTipoAfectacionProducto.Value, hfdIdOnerosidadProducto.Value, hfdIdSistemaISCProducto.Value, nImporteTotalISC, txtPrecioSugeridoProducto.Text, nValorNetoTotalConDscto, hfdPorcentajePercepcionProducto.Value, hfdPercepcionProducto.Value,
                        '                                 hfdTipoAfectacionProducto.Value, hfdPorcentajeISCProducto.Value,
                        '                                 IIf(dtVariantes.Rows(0).Item(0) > 0, "S", "N"), Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim, Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(4).Text).Trim, IIf(strTipoCantidad = "2", hfdIdUnidadMedida2Producto.Value, hfdIdUnidadMedida1Producto.Value),
                        '                                 IIf(strTipoCantidad = "2", txtCantidadProducto.Text, txtCantidadTotalProducto.Text),
                        '                                 txtObservacionProducto.Text.Trim.ToUpper, hfdIdTipoExistenciaProducto.Value, Session("CestaComprobante")) 'cboTipoMoneda.SelectedValue,Session("IdTMonedaBase")
                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoMantenimientoCatalogo.Text, "0", grdListaCaracteristica.SelectedValue.trim, Server.HtmlDecode(grdListaCaracteristica.SelectedRow.Cells(1).Text).Trim, UCase(txtIdReferenciaSAPCaracteristica.Text.Trim), UCase(txtDescripcionCampoSAPCaracteristica.Text.Trim), UCase(txtValorCaracteristica.Text.Trim), Session("CestaCatalogoCaracteristica"))
                        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
                        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()

                        'TotalizarGrid()
                        'txtImporteFormaPagoOp1.Text = Math.Round(Convert.ToDecimal(txtTotalGeneral.Text), 2)
                        'LimpiarObjetosProducto()
                        LimpiarObjetosCaracteristicas()
                        'lnk_mostrarPanelProducto_ModalPopupExtender.Show()
                        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                        grdListaCaracteristica.SelectedIndex = -1
                        lblMensajeCaracteristica.Text = "Caracteristica agregada con éxito."
                    Else
                        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                    End If
                Else
                    lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                    lblMensajeCaracteristica.Text = "Debe de seleccionar un item."
                End If
            End If
        Catch ex As Exception
            lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
            lblMensajeCaracteristica.Text = ex.Message
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoCatalogo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoCatalogo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            'JMUG: 26/02/2023
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0379", strOpcionModulo, Session("IdSistema"))

            If cboTipoActivo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un tipo de activo.")
            End If

            Dim Catalogo As New LOGI_CATALOGO
            Catalogo.cIdCatalogo = UCase(txtIdCatalogoMantenimientoCatalogo.Text)
            Catalogo.cIdTipoActivo = cboTipoActivoMantenimientoCatalogo.SelectedValue
            Catalogo.cIdJerarquiaCatalogo = "0"
            Catalogo.cIdEnlaceCatalogo = "" 'UCase(txtIdCatalogo.Text)
            Catalogo.vDescripcionCatalogo = UCase(txtDescripcionMantenimientoCatalogo.Text)
            Catalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviadaMantenimientoCatalogo.Text)
            Catalogo.bEstadoRegistroCatalogo = IIf(hfdOperacionDetalle.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
            Catalogo.cIdSistemaFuncionalCatalogo = ""
            'Catalogo.cIdSistemaFuncional = ""
            'Catalogo.vDimensionesCatalogo = txtDimensiones.Text
            'Catalogo.vVoltajeCatalogo = txtVoltaje.Text
            'Catalogo.vPesoCatalogo = txtPeso.Text
            Catalogo.nVidaUtilCatalogo = Convert.ToDouble(IIf(txtVidaUtilMantenimientoCatalogo.Text.Trim = "", "0", txtVidaUtilMantenimientoCatalogo.Text))
            Catalogo.cIdCuentaContableCatalogo = txtCuentaContableMantenimientoCatalogo.Text
            Catalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasingMantenimientoCatalogo.Text
            Catalogo.nPeriodoGarantiaCatalogo = txtPeriodoGarantiaMantenimientoCatalogo.Text
            Catalogo.nPeriodoMinimoMantenimientoCatalogo = txtPeriodoMinimoMantenimientoCatalogo.Text
            'Catalogo.cIdEmpresa = Session("IdEmpresa")

            Dim ColeccionCatCar As New List(Of LOGI_CATALOGOCARACTERISTICA)
            Dim i As Integer
            For i = 0 To grdDetalleCaracteristicaMantenimientoCatalogo.Rows.Count - 1
                Dim DetDocumento As New LOGI_CATALOGOCARACTERISTICA

                DetDocumento.cIdCatalogo = txtIdCatalogoMantenimientoCatalogo.Text
                'DetDocumento.cIdEmpresa = Session("IdEmpresa")
                DetDocumento.cIdJerarquiaCatalogo = "0"
                DetDocumento.cIdCaracteristica = Server.HtmlDecode(Replace(grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i).Cells(3).Text.ToString, "&nbsp;", ""))
                DetDocumento.nIdNumeroItemCatalogoCaracteristica = grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i).Cells(2).Text
                'Dim row As GridViewRow = grdDetalleCaracteristica.SelectedRow(i)
                Dim row = grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i)
                'For Each row As GridViewRow In grdDetalle.Rows
                'MsgBox(Session("CestaComprobante").Rows(row.RowIndex)("Codigo").ToString())
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalle"), TextBox)
                Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalle"), TextBox)
                '    If txtCantidadGeneral.Text = "" Then

                DetDocumento.vValorCatalogoCaracteristica = UCase(txtValorDetalle.Text.Trim)
                DetDocumento.cIdReferenciaSAPCatalogoCaracteristica = UCase(txtIdReferenciaSAPDetalle.Text.Trim) 'Server.HtmlDecode(Replace(grdDetalleCaracteristica.Rows(i).Cells(5).Text.ToString, "&nbsp;", ""))
                DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica = UCase(txtDescripcionCampoSAPDetalle.Text.Trim) 'Server.HtmlDecode(Replace(grdDetalleCaracteristica.Rows(i).Cells(6).Text.ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("Codigo")
                'DetDocumento.cIdJerarquiaCatalogo = IIf(hfdOperacion.Value = "N", "", Documento.vIdNumeroCorrelativoCabeceraOfertaVenta)
                'DetDocumento.cIdProductoVariante = Server.HtmlDecode(grdDetalle.Rows(i).Cells(8).Text).Trim 'Session("CestaComprobante").Rows(i)("Codigo")
                'DetDocumento.nIdNumeroItemDetalleOfertaVenta = grdDetalle.Rows(i).Cells(2).Text 'Session("CestaComprobante").rows(i)("Item")
                'DetDocumento.cIdTipoMoneda = Convert.ToChar(grdDetalle.Rows(i).Cells(10).Text) 'Convert.ToChar(grdDetalle.Rows(i).Cells(7).Text) 'Convert.ToChar(Session("CestaComprobante").Rows(i)("TMoneda")) 'cboTipoMoneda.SelectedValue

                'Dim row = grdDetalle.Rows(i)

                'Dim lblDescripcionProducto As Label = TryCast(row.FindControl("lblDescripcionDetalle"), Label)
                'DetDocumento.vDescripcionDetalleOfertaVenta = lblDescripcionProducto.Text 'Session("CestaComprobante").Rows(i)("Descripcion")
                'DetDocumento.nCantidadProductoDetalleOfertaVenta = grdDetalle.Rows(i).Cells(25).Text 'Session("CestaComprobante").Rows(i)("CantidadTotalUnidMedida")
                'DetDocumento.cIdUnidadMedidaProductoDetalleOfertaVenta = Server.HtmlDecode(grdDetalle.Rows(i).Cells(26).Text) 'Session("CestaComprobante").Rows(i)("IdUnidMedida")
                'DetDocumento.nCantidad1DetalleOfertaVenta = grdDetalle.Rows(i).Cells(21).Text 'Session("CestaComprobante").Rows(i)("Cantidad1")
                'DetDocumento.cIdUnidadMedida1DetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(22).Text).ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("IdUnidMedida1")
                'DetDocumento.nCantidad2DetalleOfertaVenta = grdDetalle.Rows(i).Cells(23).Text 'Session("CestaComprobante").Rows(i)("Cantidad2")
                'DetDocumento.cIdUnidadMedida2DetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(24).Text).ToString, "&nbsp;", ""))  'Session("CestaComprobante").Rows(i)("IdUnidMedida2")
                'DetDocumento.nTamanoPesoProductoDetalleOfertaVenta = grdDetalle.Rows(i).Cells(20).Text 'Session("CestaComprobante").Rows(i)("TamanoPeso")
                'DetDocumento.nValorUnitarioTotalVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(28).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("ValorVentaUnitarioTotal")), 10)
                'DetDocumento.nPrecioOrigenVentaDetalleOfertaVenta = grdDetalle.Rows(i).Cells(33).Text 'grdDetalle.Rows(i).Cells(8).Text 'Session("CestaComprobante").Rows(i)("PrecioOrigen")
                'DetDocumento.nPrecioUnitarioVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(34).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(9).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PrecioUnitario")), 10)
                'DetDocumento.nPrecioUnitarioTotalVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(35).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(10).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PrecioUnitarioTotal")), 10)
                'DetDocumento.nPrecioUnitarioSugeridoDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(29).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PrecioUnitarioSugerido")), 10)
                'DetDocumento.nDescuentoVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(39).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(14).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("DescuentoVenta")), 10)
                'DetDocumento.nValorVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(36).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(11).Text), 10) - DetDocumento.nDescuentoVentaDetalleDocumento 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(11).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("ValorVenta")), 10) 'Convert.ToDecimal(Math.Round((Session("CestaComprobante").Rows(i)("Precio")) / ((Session("nImpuesto") / 100) + 1), 2)) 'Double
                'DetDocumento.nIGVDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(38).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(13).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("IGV")), 10) 'Convert.ToDecimal(Math.Round(Session("CestaComprobante").Rows(i)("Total")) - Math.Round((Session("CestaComprobante").Rows(i)("Precio")) / ((Session("nImpuesto") / 100) + 1), 2))
                'DetDocumento.nTotalPrecioVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(41).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(16).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("Total")), 10) 'Convert.ToDecimal(Math.Round(Session("CestaComprobante").Rows(i)("Total"), 2))
                'DetDocumento.cIdTipoAfectacion = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(27).Text).ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("IdTipoAfectacion")
                'DetDocumento.cIdOnerosidadDetalleOfertaVenta = Convert.ToChar(Server.HtmlDecode(grdDetalle.Rows(i).Cells(30).Text)) 'Convert.ToChar(Session("CestaComprobante").Rows(i)("IdOnerosidad"))
                'DetDocumento.nISCDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(37).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(12).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("ISC")), 10)
                'DetDocumento.cIdSistemaISCDetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(31).Text).ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("IdSistemaISC")
                ''DetDocumento.nPorcentajePercepcionDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(32).Text), 2) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PorcentajePercepcion")), 2)
                ''DetDocumento.nPercepcionDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(40).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(15).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("Percepcion")), 10)
                'DetDocumento.nPorcentajeISCDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(42).Text), 10)
                'DetDocumento.cIdUnidadMedidaOrigenDetalleOfertaVenta = Server.HtmlDecode(grdDetalle.Rows(i).Cells(43).Text) '"IdUnidadMedidaOrigen"
                'DetDocumento.vObservacionDetalleOfertaVenta = Server.HtmlDecode(grdDetalle.Rows(i).Cells(44).Text).ToUpper.Trim
                'DetDocumento.cIdTipoExistenciaDetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(45).Text).ToString, "&nbsp;", ""))  'Session("CestaComprobante").Rows(i)("IdUnidMedida2")
                ColeccionCatCar.Add(DetDocumento)
            Next

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
            'LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacionDetalle.Value = "N" Then
                If (CatalogoNeg.CatalogoInserta(Catalogo)) = 0 Then
                    'If (CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCatCar, LogAuditoria)) Then
                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" &
                                                   Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncionalCatalogo & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                                   Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                                   Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
                                                   Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" & Catalogo.cIdCatalogo & "'"

                    LogAuditoria.vEvento = "INSERTAR CATALOGO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdCatalogoMantenimientoCatalogo.Text = Catalogo.cIdCatalogo

                    If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
                    End If

                    MyValidator.ErrorMessage = "Transacción registrada con éxito"

                    'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                    'Me.grdLista.DataBind()
                    'pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    'hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (CatalogoNeg.CatalogoEdita(Catalogo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" &
                                               Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncionalCatalogo & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
                                               Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
                                               Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
                                               Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" & Catalogo.cIdCatalogo & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR CATALOGO"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
                    End If
                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                    'Me.grdLista.DataBind()
                    'pnlGeneral.Enabled = False
                    BloquearMantenimiento(True, False, True, False)
                    'hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    'Exit Sub
                End If
            End If
            hfdOperacion.Value = "R"
            'BloquearPagina(0)
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnCancelarCaracteristica_Click(sender As Object, e As EventArgs) Handles btnCancelarCaracteristica.Click
        lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnNuevoTipoActivo_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoTipoActivo.Click
        hfdOperacionDetalle.Value = "N"
        'LimpiarObjetosTipoActivo()
        LlenarDataTipoActivo()
        lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarTipoActivo_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarTipoActivo.Click
        hfdOperacionDetalle.Value = "E"
        'LimpiarObjetosTipoActivo()
        LlenarDataTipoActivo()
        If MyValidator.ErrorMessage = "" Then
            lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender.Show()
        End If
    End Sub

    Private Sub lnkbtnNuevoCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoCatalogo.Click
        hfdOperacionDetalle.Value = "N"
        LlenarDataCatalogo()
        lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarCatalogo.Click
        hfdOperacionDetalle.Value = "E"
        LlenarDataCatalogo()
        If MyValidator.ErrorMessage = "" Then
            lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
        End If
    End Sub

    Private Sub cboCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCatalogo.SelectedIndexChanged
        CargarCestaComponenteCatalogo()
        CargarCestaEquipoComponente()
    End Sub

    Private Sub lnkbtnNuevoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoComponente.Click
        hfdOperacionDetalle.Value = "N"
        LlenarDataDetalleCatalogo()
        lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
    End Sub

    'Protected Sub fscIdentificadorComponente_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    MsgBox("pollito con papas")
    '    '    Try
    '    '        ''Función para validar si tiene permisos
    '    '        ''lblMensaje.Text = ""
    '    '        'MyValidator.ErrorMessage = ""
    '    '        'fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '    '        ''JMUG: 08/02/2023
    '    '        ''FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "0065", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
    '    '        'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0377", strOpcionModulo, Session("IdSistema"))

    '    '        'hfdOperacion.Value = "N"
    '    '        'BloquearPagina(2)
    '    '        'txtDescripcion.Focus()
    '    '        'BloquearMantenimiento(False, True, False, True)
    '    '        'LimpiarObjetos()
    '    '        'ValidarTexto(True)
    '    '        'ActivarObjetos(True)
    '    '        'hfTab.Value = "tab2"
    '    '        ''ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '    '    Catch ex As Exception
    '    '        'ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '    '        MyValidator.ErrorMessage = ex.Message
    '    '        MyValidator.IsValid = False
    '    '        MyValidator.ID = "ErrorPersonalizado"
    '    '        'MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '    '        MyValidator.ValidationGroup = "vgrpValidar"
    '    '        Me.Page.Validators.Add(MyValidator)
    '    '    End Try
    'End Sub
    Protected Sub btnAceptarDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles btnAceptarDetalleCatalogo.Click
        Try
            If cboTipoActivoDetalleCatalogo.SelectedIndex <= 0 Then
                Throw New Exception("Debe de ingresar el código del tipo de activo correcto.")
            ElseIf cboSistemaFuncionalDetalleCatalogo.SelectedIndex < 0 Then
                Throw New Exception("Debe de ingresar el código del sistema funcional correcto.")
            ElseIf txtDescripcionDetalleCatalogo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción correcta.")
            ElseIf txtDescripcionAbreviadaDetalleCatalogo.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción abreviada correcta.")
            Else
                If hfdOperacion.Value = "E" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                                         txtIdEquipo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                                                         "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                                                         txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                                         cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, txtPeriodoGarantiaDetalleCatalogo.Text, txtPeriodoMinimoDetalleCatalogo.Text, Session("CestaEquipoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        clsLogiCestaCatalogo.AgregarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                     "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                     txtIdEquipo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                                     "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                                     txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                     cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, txtPeriodoGarantiaDetalleCatalogo.Text, txtPeriodoMinimoDetalleCatalogo.Text, Session("CestaEquipoComponente"))
                    End If
                ElseIf hfdOperacion.Value = "N" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        clsLogiCestaCatalogo.EditarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                                         txtIdEquipo.Text.Trim, txtDescripcionDetalleCatalogo.Text.Trim, txtDescripcionAbreviadaDetalleCatalogo.Text.Trim,
                                                         "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                                                         txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                                         cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, txtPeriodoGarantiaDetalleCatalogo.Text, txtPeriodoMinimoDetalleCatalogo.Text, Session("CestaEquipoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        clsLogiCestaCatalogo.AgregarCesta(txtIdDetalleCatalogo.Text.Trim, IIf(cboTipoActivoDetalleCatalogo.SelectedIndex > 0, cboTipoActivoDetalleCatalogo.SelectedValue, " "),
                                                          "1", IIf(cboSistemaFuncionalDetalleCatalogo.SelectedIndex >= 0, cboSistemaFuncionalDetalleCatalogo.SelectedValue, " "),
                                                          txtIdEquipo.Text.Trim, UCase(txtDescripcionDetalleCatalogo.Text.Trim), UCase(txtDescripcionAbreviadaDetalleCatalogo.Text.Trim),
                                                          "1", txtVidaUtilDetalleCatalogo.Text.Trim,
                                                          txtCuentaContableDetalleCatalogo.Text.Trim, txtCuentaContableLeasingDetalleCatalogo.Text.Trim, cboTipoActivoDetalleCatalogo.SelectedItem.Text,
                                                          cboSistemaFuncionalDetalleCatalogo.SelectedItem.Text, txtPeriodoGarantiaDetalleCatalogo.Text, txtPeriodoMinimoDetalleCatalogo.Text, Session("CestaEquipoComponente"))
                    End If
                End If
            End If
            'Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
            'Me.grdListaDetalle.DataBind()
            grdComponenteEquipo.DataSource = Session("CestaEquipoComponente")
            grdComponenteEquipo.DataBind()
            Me.lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Hide()
        Catch ex As Exception
            lblMensajeDetalleCatalogo.Text = ex.Message
            Me.lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub cboTipoActivoDetalleCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivoDetalleCatalogo.SelectedIndexChanged
        ListarSistemaFuncionalDetalleCatalogoCombo()
        lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarSistemaFuncionalDetalleCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarSistemaFuncionalDetalleCatalogo.Click
        hfdOperacionDetalleAdicional.Value = "E"
        lnk_mostrarPanelMantenimientoSistemaFuncional_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnNuevoSistemaFuncionalCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoSistemaFuncionalDetalleCatalogo.Click
        hfdOperacionDetalleAdicional.Value = "N"
        lnk_mostrarPanelMantenimientoSistemaFuncional_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAceptarMantenimientoSistemaFuncional_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoSistemaFuncional.Click
        Try
            'Función para validar si tiene permisos
            'lblMensaje.Text = ""
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0364", strOpcionModulo, Session("IdSistema"))

            Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
            Dim SistemaFuncional As New LOGI_SISTEMAFUNCIONAL
            SistemaFuncional.cIdSistemaFuncional = UCase(txtIdSistemaFuncionalMantenimientoSistemaFuncional.Text)
            SistemaFuncional.vDescripcionSistemaFuncional = UCase(txtDescripcionMantenimientoSistemaFuncional.Text)
            SistemaFuncional.vDescripcionAbreviadaSistemaFuncional = UCase(txtDescripcionAbreviadaMantenimientoSistemaFuncional.Text)
            SistemaFuncional.bEstadoRegistroSistemaFuncional = 1 'IIf(hfdOperacion.Value = "N", True, Convert.ToBoolean(Convert.ToInt32(hfdEstado.Value)))
            SistemaFuncional.cIdCatalogo = cboCatalogo.SelectedValue
            SistemaFuncional.cIdTipoActivoSistemaFuncional = cboTipoActivo.SelectedValue
            SistemaFuncional.cIdJerarquiaCatalogo = "0"

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")


            If hfdOperacionDetalleAdicional.Value = "N" Then
                If (SistemaFuncionalNeg.SistemaFuncionalInserta(SistemaFuncional)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_SISTEMAFUNCIONAL 'SQL_INSERT', '','" & SistemaFuncional.cIdSistemaFuncional & "', '" &
                                       SistemaFuncional.cIdCatalogo & "', '" &
                                       SistemaFuncional.cIdJerarquiaCatalogo & "', '" & SistemaFuncional.vDescripcionSistemaFuncional & "', '" & SistemaFuncional.vDescripcionAbreviadaSistemaFuncional & "', '" &
                                       SistemaFuncional.bEstadoRegistroSistemaFuncional & "', '" & SistemaFuncional.cIdSistemaFuncional & "'"
                    LogAuditoria.vEvento = "INSERTAR SISTEMA FUNCIONAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    txtIdSistemaFuncionalMantenimientoSistemaFuncional.Text = SistemaFuncional.cIdSistemaFuncional
                    MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    'Me.grdLista.DataSource = SistemaFuncionalNeg.SistemaFuncionalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
                    'Me.grdLista.DataBind()
                    'pnlGeneral.Enabled = False
                    'BloquearMantenimiento(True, False, True, False)
                    'hfdOperacion.Value = "R"
                    'hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalleAdicional.Value = "E" Then
                If (SistemaFuncionalNeg.SistemaFuncionalEdita(SistemaFuncional)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_SISTEMAFUNCIONAL 'SQL_UPDATE', '','" & SistemaFuncional.cIdSistemaFuncional & "', '" &
                                       SistemaFuncional.cIdCatalogo & "', '" &
                                       SistemaFuncional.cIdJerarquiaCatalogo & "', '" & SistemaFuncional.vDescripcionSistemaFuncional & "', '" & SistemaFuncional.vDescripcionAbreviadaSistemaFuncional & "', '" &
                                       SistemaFuncional.bEstadoRegistroSistemaFuncional & "', '" & SistemaFuncional.cIdSistemaFuncional & "'"

                    LogAuditoria.vEvento = "ACTUALIZAR SISTEMA FUNCIONAL"
                    LogAuditoria.vQuery = Session("Query")
                    LogAuditoria.cIdSistema = Session("IdSistema")
                    LogAuditoria.cIdModulo = strOpcionModulo

                    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    'Me.grdLista.DataSource = SistemaFuncionalNeg.SistemaFuncionalListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "*")
                    'Me.grdLista.DataBind()
                    'pnlGeneral.Enabled = False
                    'BloquearMantenimiento(True, False, True, False)
                    'hfdOperacion.Value = "R"
                    'hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If
            hfdOperacionDetalleAdicional.Value = "R"
            'BloquearPagina(0)
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub
End Class