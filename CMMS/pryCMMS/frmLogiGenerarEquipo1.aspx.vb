Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarEquipo1
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

            Dim UsuarioNeg As New clsUsuarioNegocios
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

    Sub CargarCestaCatalogoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        'VaciarCesta(Session("CestaCatalogoComponente"))
        clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoComponente")) 'Lo Cambie por el de arriba: 28/03/2023

        Dim CatalogoNeg As New clsCatalogoNegocios
        'Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        Dim Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        Dim intContador As Integer = 0

        For Each Registro In Coleccion
            Dim booExiste As Boolean = False
            For Each Registro2 In Coleccion2
                If Registro.Codigo = Registro2.IdCatalogo And Registro.IdSistemaFuncional = Registro2.IdSistemaFuncional And Registro.IdTipoActivo = Registro2.IdTipoActivo Then
                    booExiste = True
                    Exit For
                End If
            Next
            If booExiste = False Then
                'JMUG: 03/03/2023 AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.IdTipoActivo, Registro.IdSistemaFuncional, Registro.Estado, Registro.DescripcionAbreviada, "", Session("CestaCatalogoComponente"))
                '   clsLogiCestaCatalogo.AgregarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                '"1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                'txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                '"1", IIf(txtVidaUtilCatalogoComponente.Text.Trim = "", "0", txtVidaUtilCatalogoComponente.Text.Trim),
                'txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                'cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, Session("CestaCatalogoComponente"))
                clsLogiCestaCatalogo.AgregarCesta(Registro.Codigo, Registro.IdTipoActivo, "1", Registro.IdSistemaFuncional,
                                                  cboCatalogo.SelectedValue, UCase(Registro.Descripcion),
                                                  UCase(Registro.DescripcionAbreviada), Registro.Estado, 0, Registro.IdCuentaContable, Registro.IdCuentaContableLeasing, Registro.DescripcionTipoActivo,
                                                  Registro.DescripcionSistemaFuncional, Registro.PeriodoGarantia, Registro.PeriodoMinimoMantenimiento,
                                                  Session("CestaCatalogoComponente"))
            End If
        Next

        grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
        Me.grdCatalogoComponente.DataBind()
    End Sub

    Sub CargarCestaCatalogoCaracteristica()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '0' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '0'")
            For Each Caracteristicas In dsCaracteristica.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "", "0", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCatalogoCaracteristica"))
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

        'grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
        'Me.grdCatalogoComponente.DataBind()





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

    Sub CargarCestaCaracteristicaCatalogoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            'Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
            '                                                               "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
            '                                                               "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '1'")
            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & txtIdCatalogoComponente.Text & "' AND CATCAR.cIdJerarquiaCatalogo = '1'")
            For Each Caracteristicas In dsCaracteristica.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), "", "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaCatalogoComponente"))
            Next
            'Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCaracteristicaCatalogoComponente")
            'Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
            Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponente")
            Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaCaracteristicaEquipoPrincipal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoPrincipal"))
            'Dim dsCaracteristicaEquipoPrincipal = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "' AS cIdJerarquiaCatalogo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
            '                                                               "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
            '                                                               "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
            '                                                               "WHERE EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text & "' AND cIdJerarquiaCatalogo = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
            Dim dsCaracteristicaEquipoPrincipal = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "' AS cIdJerarquiaCatalogo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                                                                           "WHERE EQU.cIdEquipo = '" & txtIdEquipo.Text & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
            For Each CaracteristicaEquipo In dsCaracteristicaEquipoPrincipal.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(CaracteristicaEquipo("cIdCatalogo"), CaracteristicaEquipo("cIdEquipo"), IIf(lnkEsComponenteOn.Visible = True, "1", "0"), CaracteristicaEquipo("cIdCaracteristica"), CaracteristicaEquipo("vDescripcionCaracteristica"), CaracteristicaEquipo("cIdReferenciaSAPEquipoCaracteristica"), CaracteristicaEquipo("vDescripcionCampoSAPEquipoCaracteristica"), CaracteristicaEquipo("vValorReferencialEquipoCaracteristica"), Session("CestaCaracteristicaEquipoPrincipal"))
            Next
            Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
            Me.grdDetalleCaracteristicaEquipo.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Sub CargarCestaCaracteristicaEquipoPrincipalTemporal()
    '    Try
    '        clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoPrincipalFiltrado"))
    '        'Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
    '        '                                                               "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
    '        '                                                               "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
    '        '                                                               "WHERE EQUCAR.cIdEquipo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdEquipo").ToString.Trim & "'")
    '        Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
    '                                                                       "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
    '                                                                       "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
    '                                                                       "WHERE EQUCAR.cIdEquipo = '" & txtIdEquipo.Text & "' AND cIdJerarquiaEquipo = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
    '        For Each CaracteristicaEquipo In dsCaracteristicaEquipoComponente.Rows
    '            'Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdJerarquia = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
    '            Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & cboCatalogo.SelectedValue & "' AND IdJerarquia = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
    '            If resultCaracteristicaSimple.Length = 0 Then
    '                Me.grdDetalleCaracteristicaEquipo.DataSource = Nothing
    '            Else
    '                Dim rowFil As DataRow() = resultCaracteristicaSimple
    '                For Each fila As DataRow In rowFil
    '                    clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), "", fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaEquipoPrincipalFiltrado"))
    '                Next
    '                Exit Sub
    '            End If
    '        Next
    '        If dsCaracteristicaEquipoComponente.Rows.Count = 0 Then
    '            Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
    '                                                                       "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
    '                                                                       "WHERE CATCAR.cIdCatalogo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim & "' AND CATCAR.cIdJerarquiaCatalogo = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
    '            For Each Caracteristicas In dsCaracteristica.Rows
    '                Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & Caracteristicas("cIdCatalogo").Trim & "' AND IdCaracteristica = '" & Caracteristicas("cIdCaracteristica").trim & "' AND IdJerarquia = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
    '                If resultCaracteristicaSimple.Length = 0 Then
    '                    clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaEquipoPrincipal"))
    '                End If
    '                clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaEquipoPrincipalFiltrado"))
    '            Next
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub


    Sub CargarCestaCaracteristicaEquipoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponente"))
            Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '1' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                                                                           "WHERE EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
            For Each CaracteristicaEquipo In dsCaracteristicaEquipoComponente.Rows
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(CaracteristicaEquipo("cIdCatalogo"), CaracteristicaEquipo("cIdEquipo"), "1", CaracteristicaEquipo("cIdCaracteristica"), CaracteristicaEquipo("vDescripcionCaracteristica"), CaracteristicaEquipo("cIdReferenciaSAPEquipoCaracteristica"), CaracteristicaEquipo("vDescripcionCampoSAPEquipoCaracteristica"), CaracteristicaEquipo("vValorReferencialEquipoCaracteristica"), Session("CestaCaracteristicaEquipoComponente"))
            Next

        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaCaracteristicaEquipoComponenteTemporal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            'JNUG: 16/03/2023 clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponente"))
            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If
            'Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
            '                                                               "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
            '                                                               "WHERE CATCAR.cIdCatalogo = '" & cboCatalogo.SelectedValue & "' AND CATCAR.cIdJerarquiaCatalogo = '1'")
            'JMUG: 13/03/2023
            If Session("CestaEquipoComponente").Rows.Count > 0 Then
                Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '1' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                                                                           "WHERE EQUCAR.cIdEquipo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdEquipo").ToString.Trim & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
                For Each CaracteristicaEquipo In dsCaracteristicaEquipoComponente.Rows
                    If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                        If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                            Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdJerarquia = '1'")
                            If resultCaracteristicaSimple.Length = 0 Then
                                Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Nothing
                            Else
                                Dim rowFil As DataRow() = resultCaracteristicaSimple
                                For Each fila As DataRow In rowFil
                                    clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdEquipo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                                Next
                                Exit Sub
                            End If
                        End If
                    End If
                Next
                If dsCaracteristicaEquipoComponente.Rows.Count = 0 Then
                    Dim dsCaracteristica = CaracteristicaNeg.CaracteristicaGetData("SELECT CATCAR.cIdCatalogo, CATCAR.cIdJerarquiaCatalogo, CATCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, CATCAR.nIdNumeroItemCatalogoCaracteristica, CATCAR.cIdReferenciaSAPCatalogoCaracteristica, CATCAR.vDescripcionCampoSAPCatalogoCaracteristica, CATCAR.vValorCatalogoCaracteristica " &
                                                                           "FROM LOGI_CATALOGOCARACTERISTICA AS CATCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON CATCAR.cIdCaracteristica = CAR.cIdCaracteristica AND CATCAR.cIdJerarquiaCatalogo = '1' " &
                                                                           "WHERE CATCAR.cIdCatalogo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim & "' AND CATCAR.cIdJerarquiaCatalogo = '1'")
                    For Each Caracteristicas In dsCaracteristica.Rows
                        Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & Caracteristicas("cIdCatalogo").Trim & "' AND IdCaracteristica = '" & Caracteristicas("cIdCaracteristica").trim & "' AND IdJerarquia = '1'")
                        If resultCaracteristicaSimple.Length = 0 Then
                            clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaEquipoComponente"))
                        End If
                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(Caracteristicas("cIdCatalogo"), txtIdEquipoComponente.Text, "1", Caracteristicas("cIdCaracteristica"), Caracteristicas("vDescripcionCaracteristica"), Caracteristicas("cIdReferenciaSAPCatalogoCaracteristica"), Caracteristicas("vDescripcionCampoSAPCatalogoCaracteristica"), Caracteristicas("vValorCatalogoCaracteristica"), Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                    Next
                End If
            End If
            'Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
            'Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
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

            'If grdCatalogoComponente.Rows.Count > 0 Then
            If grdEquipoComponente.SelectedIndex >= (grdEquipoComponente.Rows.Count - 1) Then
                'grdCatalogoComponente.SelectedIndex = grdCatalogoComponente.Rows.Count - 1
                grdEquipoComponente.SelectedIndex = -1
            End If

            If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                    'Dim Coleccion = AreaElementoNeg.AreaElementoListaBusqueda("AREAELEM.cIdElemento", "", cboSistema.SelectedValue, grdModulo.SelectedRow.Cells(1).Text, _
                    '                                                                   cboArea.SelectedValue, txtIdPerfil.Text.Trim, txtIdUsuario.Text)
                    'Dim Coleccion = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdCatalogo", cboCatalogo.SelectedValue, 1)
                    'Dim Coleccion = MaestroActivoNeg.MaestroActivoListaBusqueda("cIdMaestroActivo", IIf(txtIdMaestroActivo.Text.Trim = "", "*", txtIdMaestroActivo.Text.Trim), 1)
                    'Dim Coleccion = EquipoNeg.EquipoListaBusqueda("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
                    Dim Coleccion = EquipoNeg.EquipoListaBusqueda("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)

                    Dim intContador As Integer = 0

                    For Each Registro In Coleccion
                        'AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.IdTipoActivo, Registro.IdSistemaFuncional, Registro.Estado, Registro.DescripcionAbreviada, Session("CestaEquipoComponente"))
                        'clsLogiCestaMaestroActivo.AgregarCesta("", Registro.Codigo, Registro.IdTipoActivo, _
                        'Opcion nueva
                        Dim Equipo1 As New LOGI_EQUIPO
                        'Equipo1 = EquipoNeg.EquipoListarPorId(txtIdEquipo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                        'Equipo1 = EquipoNeg.EquipoListarPorId(txtIdEquipo.Text)
                        Equipo1 = EquipoNeg.EquipoListarPorIdDetalle(Registro.Codigo, Registro.IdCatalogo)

                        clsLogiCestaEquipo.AgregarCesta(Equipo1.cIdEquipo, Equipo1.cIdCatalogo, Equipo1.cIdTipoActivo, Equipo1.cIdJerarquiaCatalogo,
                                                Equipo1.cIdSistemaFuncionalEquipo, Equipo1.cIdEnlaceCatalogo, Equipo1.vDescripcionEquipo,
                                                Equipo1.vDescripcionAbreviadaEquipo, IIf(IsNothing(Equipo1.dFechaTransaccionEquipo), Now, Equipo1.dFechaTransaccionEquipo), Equipo1.bEstadoRegistroEquipo,
                                                Equipo1.cIdEnlaceEquipo, Equipo1.vObservacionEquipo, Equipo1.nVidaUtilEquipo,
                                                "", "", Equipo1.nPeriodoGarantiaEquipo, Equipo1.nPeriodoMinimoMantenimientoEquipo,
                                                Equipo1.vNumeroSerieEquipo, Equipo1.vNumeroParteEquipo, Equipo1.cIdEstadoComponenteEquipo,
                                               Session("CestaEquipoComponente"))

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
                Dim Coleccion = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND EQU.cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
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
                    'Equipo = EquipoNeg.EquipoListarPorIdDetalle(txtIdEquipo.Text, Registro.IdCatalogo, Registro.IdTipoActivo)
                    'Equipo = EquipoNeg.EquipoListarPorIdDetalle(Registro.Codigo, Registro.IdCatalogo, Registro.IdTipoActivo)
                    Equipo = EquipoNeg.EquipoListarPorIdDetalle(Registro.Codigo, Registro.IdCatalogo)

                    'JMUG: 06/03/2023 - Inicio de Carga de última parte - REVISARRRR
                    clsLogiCestaEquipo.AgregarCesta(Equipo.cIdEquipo, Equipo.cIdCatalogo, Equipo.cIdTipoActivo, Equipo.cIdJerarquiaCatalogo,
                                                Equipo.cIdSistemaFuncionalEquipo, Equipo.cIdEnlaceCatalogo, Equipo.vDescripcionEquipo,
                                                Equipo.vDescripcionAbreviadaEquipo, IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo), Equipo.bEstadoRegistroEquipo,
                                                Equipo.cIdEnlaceEquipo, Equipo.vObservacionEquipo, Equipo.nVidaUtilEquipo,
                                                "", "", Equipo.nPeriodoGarantiaEquipo, Equipo.nPeriodoMinimoMantenimientoEquipo,
                                                Equipo.vNumeroSerieEquipo, Equipo.vNumeroParteEquipo, Equipo.cIdEstadoComponenteEquipo,
                                               Session("CestaEquipoComponente"))
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
                    Dim TablaCestaCatalogo As DataTable
                    'TablaCestaMaestro = Session("CestaEquipoComponente")
                    TablaCestaMaestro = Session("CestaEquipoComponente")
                    TablaCestaCatalogo = Session("CestaCatalogoComponente")
                    Dim i As Integer
                    'jyuijan@redrilsa.com.pe
                    'Juan Jose Yuijan.
                    'Coordinar tema de registro de información.
                    For i = 0 To TablaCestaMaestro.Rows.Count - 1
                        'If TablaCestaMaestro.Rows(i)("IdTipoActivo") = cboTipoActivo.SelectedValue And
                        '   TablaCestaMaestro.Rows(i)("IdCatalogo") = cboCatalogo.SelectedValue Then
                        For j = 0 To TablaCestaCatalogo.Rows.Count - 1
                            'If TablaCestaMaestro.Rows(i)("IdTipoActivo") = TablaCestaCatalogo.Rows(j)("IdTipoActivo") And
                            '   TablaCestaMaestro.Rows(i)("IdCatalogo") = TablaCestaCatalogo.Rows(j)("IdCatalogo") Then
                            If TablaCestaMaestro.Rows(i)("IdJerarquia") = TablaCestaCatalogo.Rows(j)("IdJerarquia") And
                               TablaCestaMaestro.Rows(i)("IdCatalogo") = TablaCestaCatalogo.Rows(j)("IdCatalogo") Then
                                'TablaAreaElemento.Rows(i)("IdPerfil") = txtIdPerfil.Text And _
                                QuitarCesta(intContador, Session("CestaCatalogoComponente"))
                                intContador = intContador - 1
                                Exit For
                            End If
                        Next
                        intContador = intContador + 1
                    Next
                Next
            End If
            'End If
            'Me.grdComponenteMaestroActivo.DataSource = Session("CestaEquipoComponente")
            'Me.grdComponenteMaestroActivo.DataBind()
            Me.grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            Me.grdCatalogoComponente.DataBind()
            Me.grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            Me.grdEquipoComponente.DataBind()
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
        cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivo.Items.Clear()
        cboTipoActivo.Items.Add("SELECCIONE DATO")
        cboTipoActivo.DataBind()

        cboTipoActivoMantenimientoCatalogo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoMantenimientoCatalogo.DataValueField = "cIdTipoActivo"
        'cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo(Session("IdEmpresa"), Session("IdPuntoVenta"))
        cboTipoActivoMantenimientoCatalogo.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivoMantenimientoCatalogo.Items.Clear()
        cboTipoActivoMantenimientoCatalogo.Items.Add("SELECCIONE DATO")
        cboTipoActivoMantenimientoCatalogo.DataBind()

        cboTipoActivoCatalogoComponente.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoCatalogoComponente.DataValueField = "cIdTipoActivo"
        cboTipoActivoCatalogoComponente.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivoCatalogoComponente.Items.Clear()
        cboTipoActivoCatalogoComponente.Items.Add("SELECCIONE DATO")
        cboTipoActivoCatalogoComponente.DataBind()

        cboTipoActivoEquipoComponente.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivoEquipoComponente.DataValueField = "cIdTipoActivo"
        cboTipoActivoEquipoComponente.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivoEquipoComponente.Items.Clear()
        cboTipoActivoEquipoComponente.Items.Add("SELECCIONE DATO")
        cboTipoActivoEquipoComponente.DataBind()


        'cboTipoActivoMaestroActivoPrincipal.DataTextField = "vDescripcionTipoActivo"
        'cboTipoActivoMaestroActivoPrincipal.DataValueField = "cIdTipoActivo"
        'cboTipoActivoMaestroActivoPrincipal.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        'cboTipoActivoMaestroActivoPrincipal.DataBind()

        'cboTipoActivoDetalleMaestroActivo.DataTextField = "vDescripcionTipoActivo"
        'cboTipoActivoDetalleMaestroActivo.DataValueField = "cIdTipoActivo"
        'cboTipoActivoDetalleMaestroActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo()
        'cboTipoActivoDetalleMaestroActivo.DataBind()
    End Sub


    Sub ListarTipoEquipoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoEquipoNeg As New clsTipoEquipoNegocios
        cboTipoEquipo.DataTextField = "vDescripcionTipoEquipo"
        cboTipoEquipo.DataValueField = "cIdTipoEquipo"
        'cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo(Session("IdEmpresa"), Session("IdPuntoVenta"))
        cboTipoEquipo.DataSource = TipoEquipoNeg.TipoEquipoListarCombo("1")
        cboTipoEquipo.Items.Clear()
        cboTipoEquipo.Items.Add("SELECCIONE DATO")
        cboTipoEquipo.DataBind()
    End Sub

    Sub ListarCatalogoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CatalogoNeg As New clsCatalogoNegocios
        cboCatalogo.DataTextField = "vDescripcionCatalogo"
        cboCatalogo.DataValueField = "cIdCatalogo"
        'cboCatalogo.DataSource = CatalogoNeg.CatalogoListarCombo(0, cboTipoActivo.SelectedValue)
        cboCatalogo.DataSource = CatalogoNeg.CatalogoListarCombo(0, cboTipoActivo.SelectedValue, "", "1") 'JMUG: 18/09/2023
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
        'cboSistemaFuncional.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo(cboTipoActivo.SelectedValue, cboCatalogo.SelectedValue)
        cboSistemaFuncional.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        cboSistemaFuncional.Items.Clear()
        cboSistemaFuncional.DataBind()
    End Sub

    Sub ListarSistemaFuncionalCatalogoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncionalCatalogoComponente.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncionalCatalogoComponente.DataValueField = "cIdSistemaFuncional"
        'cboSistemaFuncionalCatalogoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo(cboTipoActivoCatalogoComponente.SelectedValue, cboCatalogo.SelectedValue)
        cboSistemaFuncionalCatalogoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        cboSistemaFuncionalCatalogoComponente.Items.Clear()
        cboSistemaFuncionalCatalogoComponente.DataBind()
    End Sub

    Sub ListarSistemaFuncionalEquipoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        cboSistemaFuncionalEquipoComponente.DataTextField = "vDescripcionSistemaFuncional"
        cboSistemaFuncionalEquipoComponente.DataValueField = "cIdSistemaFuncional"
        'cboSistemaFuncionalEquipoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo(cboTipoActivoEquipoComponente.SelectedValue, cboCatalogo.SelectedValue)
        cboSistemaFuncionalEquipoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        cboSistemaFuncionalEquipoComponente.Items.Clear()
        cboSistemaFuncionalEquipoComponente.DataBind()
    End Sub

    Sub ListarCaracteristicaEquipoPrincipalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaEquipo.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaEquipo.DataValueField = "cIdCaracteristica"
        'cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("14", "LOGI")
        cboCaracteristicaEquipo.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaEquipo.Items.Clear()
        cboCaracteristicaEquipo.DataBind()
    End Sub

    Sub ListarCaracteristicaCatalogoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaCatalogoComponente.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaCatalogoComponente.DataValueField = "cIdCaracteristica"
        'cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("14", "LOGI")
        cboCaracteristicaCatalogoComponente.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaCatalogoComponente.Items.Clear()
        cboCaracteristicaCatalogoComponente.DataBind()
    End Sub

    Sub ListarCaracteristicaEquipoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaEquipoComponente.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaEquipoComponente.DataValueField = "cIdCaracteristica"
        'cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("14", "LOGI")
        cboCaracteristicaEquipoComponente.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaEquipoComponente.Items.Clear()
        cboCaracteristicaEquipoComponente.DataBind()
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

    Sub ListarDescripcionCatalogoComponente()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CatalogoNeg As New clsCatalogoNegocios
        cboDescripcionCatalogoComponente.DataTextField = "vDescripcionCatalogo"
        cboDescripcionCatalogoComponente.DataValueField = "cIdCatalogo"
        cboDescripcionCatalogoComponente.DataSource = CatalogoNeg.CatalogoListarDescripcionCombo()
        cboDescripcionCatalogoComponente.Items.Clear()
        cboDescripcionCatalogoComponente.Items.Add(New ListItem("SELECCIONE DATO", ""))
        cboDescripcionCatalogoComponente.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdEquipo.Enabled = False 'bActivar
        'cboTipoActivo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        'cboCatalogo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        txtDescripcionEquipo.Enabled = False
        txtNroSerieEquipo.Enabled = bActivar
        txtNroParteEquipo.Enabled = bActivar
        txtTagEquipo.Enabled = bActivar
        txtCapacidadEquipo.Enabled = bActivar
        'fscIdentificadorComponente.Disabled = Not bActivar
        cboTipoActivo.Enabled = bActivar
        cboTipoEquipo.Enabled = bActivar
        txtDescripcionEquipoSAP.Enabled = bActivar
        cboCatalogo.Enabled = bActivar
        cboSistemaFuncional.Enabled = bActivar
        'divSistemaFuncional.Visible = IIf(fscIdentificadorComponente.Checked, True, False)
        divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        divContratoReferencia.Visible = IIf(lnkEsConContratoOn.Visible = True, True, False)
        'cboCatalogoComponente.Enabled = bActivar
        txtIdCliente.Enabled = bActivar
        txtRazonSocial.Enabled = False
        'cboEmpresa.Visible = IIf(Session("IdTipoUsuario") = "U", False, True)
        cboTipoActivo.Enabled = bActivar

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
        txtIdCatalogoComponente.Enabled = False
        cboTipoActivoCatalogoComponente.Enabled = bActivar
        cboSistemaFuncionalCatalogoComponente.Enabled = bActivar
        txtDescripcionCatalogoComponente.Enabled = bActivar
        txtDescripcionCatalogoComponente.Enabled = bActivar
        txtVidaUtilCatalogoComponente.Enabled = bActivar
        txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        txtCuentaContableCatalogoComponente.Enabled = bActivar
        txtCuentaContableLeasingCatalogoComponente.Enabled = bActivar

        'Mantenimiento de Sistema Funcional / Componente
        txtIdSistemaFuncionalMantenimientoSistemaFuncional.Enabled = False
        txtDescripcionMantenimientoSistemaFuncional.Enabled = bActivar
        txtDescripcionAbreviadaMantenimientoSistemaFuncional.Enabled = bActivar
    End Sub

    Sub LlenarData()
        'Try 'JMUG: LO QUITE 11/09/2023
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.MaestroActivoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim)
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(4).Text).Trim)
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text).Trim)
        txtIdEquipo.Text = Equipo.cIdEquipo
        hfdFechaEquipo.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
        txtDescripcionEquipo.Text = Equipo.vDescripcionEquipo
        txtNroSerieEquipo.Text = Equipo.vNumeroSerieEquipo
        txtNroParteEquipo.Text = Equipo.vNumeroParteEquipo
        txtTagEquipo.Text = Equipo.vTagEquipo
        txtCapacidadEquipo.Text = Equipo.vCapacidadEquipo
        lnkEsComponenteOn.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", False, True)
        lnkEsComponenteOff.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", True, False)
        lnkEsConContratoOn.Visible = IIf(CBool(Equipo.bTieneContratoEquipo) = False, False, True)
        lnkEsConContratoOff.Visible = IIf(CBool(Equipo.bTieneContratoEquipo) = False, True, False)

        If CBool(Equipo.bTieneContratoEquipo) = False Then
            cboContratoReferencia.SelectedIndex = -1
        Else
            cboContratoReferencia.SelectedValue = Equipo.vContratoReferenciaActualEquipo
        End If
        divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        divContratoReferencia.Visible = IIf(CBool(Equipo.bTieneContratoEquipo) = False, False, True)
        Dim ClienteNeg As New clsClienteNegocios
        'JMUG: 22/07/2023 Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorIdSAP(Equipo.vIdClienteSAPEquipo, Session("IdEmpresa"))
        Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorId(Equipo.cIdCliente, Session("IdEmpresa"), "*")
        txtIdCliente.Text = Cliente.vRucCliente
        btnAdicionarCliente_Click(btnAdicionarCliente, New System.EventArgs())
        ListarClienteUbicacionCombo()
        'txtDescripcionAbreviada.Text = MaestroActivo.vDescripcionAbreviadaMaestroActivo
        'txtCuentaContableActivoMayor.Text = MaestroActivo.cIdCuentaContableMaestroActivo
        'txtObservacion.Text = MaestroActivo.vObservacionMaestroActivo
        cboClienteUbicacion.SelectedValue = IIf(Trim(Equipo.cIdClienteUbicacion) = "", "SELECCIONE DATO", Equipo.cIdClienteUbicacion)
        cboTipoActivo.SelectedValue = Equipo.cIdTipoActivo
        cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())
        cboTipoEquipo.SelectedValue = Equipo.cIdTipoEquipo
        txtDescripcionEquipoSAP.Text = Equipo.vDescripcionEquipoSAP
        cboCatalogo.SelectedValue = Equipo.cIdCatalogo
        ListarSistemaFuncionalCombo()
        'JMUG: 25/02/2023 lblSistemaFuncional.Value = Equipo.cIdSistemaFuncional
        'cboSistemaFuncional.SelectedValue = MaestroActivo.cIdSistemaFuncional
        'cboTipoActivoDetalleMaestroActivo_SelectedIndexChanged(cboTipoActivoDetalleMaestroActivo, New System.EventArgs())

        'JMUG: 25/02/2023 LlenarDataEquipo()

        CargarCestaCatalogoComponente()
        CargarCestaEquipoComponente()
        CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 20/03/2023
        CargarCestaCaracteristicaEquipoComponente()
        CargarCestaCaracteristicaEquipoComponenteTemporal()
        Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
        Me.grdDetalleCaracteristicaEquipoComponente.DataBind()

        hfdIdClienteSAPEquipo.Value = Equipo.vIdClienteSAPEquipo
        hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
        hfdFechaRegistroTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaRegistroTarjetaSAPEquipo), "", Equipo.dFechaRegistroTarjetaSAPEquipo)
        hfdFechaManufacturaTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaManufacturaTarjetaSAPEquipo), "", Equipo.dFechaManufacturaTarjetaSAPEquipo)
        hfdFechaCreacionEquipo.Value = IIf(IsNothing(Equipo.dFechaCreacionEquipo), Now, Equipo.dFechaCreacionEquipo)
        hfdIdUsuarioCreacionEquipo.Value = IIf(IsNothing(Equipo.cIdUsuarioCreacionEquipo), Session("IdUsuario"), Equipo.cIdUsuarioCreacionEquipo)
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
        'Catch ex As Exception 'JMUG: LO QUITE 11/09/2023
        '    'lblMensaje.Text = ex.Message
        '    ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        '    MyValidator.ErrorMessage = ex.Message
        '    MyValidator.IsValid = False
        '    MyValidator.ID = "ErrorPersonalizado"
        '    MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        '    Me.Page.Validators.Add(MyValidator)
        'End Try
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
            'CargarCestaCatalogoComponente()
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

    Sub LlenarDataCatalogoComponente()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosCatalogoComponente()
                cboTipoActivoCatalogoComponente.SelectedValue = cboTipoActivo.SelectedValue
                ListarSistemaFuncionalCatalogoComponenteCombo()
            Else
                LimpiarObjetosCatalogoComponente()
                'txtIdCatalogoComponente.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim
                txtIdCatalogoComponente.Text = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim
                'txtDescripcionCatalogoComponente.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(3).Text)
                cboDescripcionCatalogoComponente.SelectedValue = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text)
                txtDescripcionCatalogoComponente.Text = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text)
                'txtDescripcionAbreviadaCatalogoComponente.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(4).Text)
                txtDescripcionAbreviadaCatalogoComponente.Text = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(5).Text)
                'cboTipoActivoCatalogoComponente.SelectedValue = grdListaDetalle.SelectedRow.Cells(5).Text
                'cboTipoActivoCatalogoComponente.SelectedValue = grdCatalogoComponente.SelectedRow.Cells(4).Text
                cboTipoActivoCatalogoComponente.SelectedValue = grdCatalogoComponente.SelectedRow.Cells(3).Text
                'cboSistemaFuncionalCatalogoComponente.SelectedValue = grdListaDetalle.SelectedRow.Cells(7).Text
                ListarSistemaFuncionalCatalogoComponenteCombo()
                cboSistemaFuncionalCatalogoComponente.SelectedValue = grdCatalogoComponente.SelectedRow.Cells(4).Text
                'txtCuentaContableCatalogoComponente.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(9).Text)
                txtCuentaContableCatalogoComponente.Text = ""
                'txtCuentaContableLeasingCatalogoComponente.Text = Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(10).Text)
                txtCuentaContableLeasingCatalogoComponente.Text = ""
                txtVidaUtilCatalogoComponente.Text = Session("CestaCatalogoComponente").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
                txtPeriodoGarantiaCatalogoComponente.Text = Session("CestaCatalogoComponente").Rows(rowIndexDetalle)("PeriodoGarantia").ToString.Trim
                txtPeriodoMinimoCatalogoComponente.Text = Session("CestaCatalogoComponente").Rows(rowIndexDetalle)("PeriodoMinimo").ToString.Trim
            End If
            CargarCestaCaracteristicaCatalogoComponente()
            'LimpiarObjetos()
            'If hfdOperacionDetalle.Value = "E" Then
            '    txtVidaUtilCatalogoComponente.Text = Session("CestaCatalogo").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
            'Else
            '    Dim Catalogo = CatalogoNeg.CatalogoListarPorId(txtIdCatalogoComponente.Text, cboTipoActivoCatalogoComponente.SelectedValue, "1", "1")
            '    txtVidaUtilCatalogoComponente.Text = IIf(Catalogo.nVidaUtilCatalogo Is Nothing, "0", Catalogo.nVidaUtilCatalogo)
            'End If
        Catch ex As Exception
            'lblMensajeCatalogoComponente.Text = ex.Message
            MyValidator.ErrorMessage = ex.Message
        End Try
    End Sub

    Sub LlenarDataEquipoComponente()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosEquipoComponente()
                cboTipoActivoEquipoComponente.SelectedValue = cboTipoActivo.SelectedValue
                ListarSistemaFuncionalEquipoComponenteCombo()
            Else
                LimpiarObjetosEquipoComponente()
                Dim Equipo As New LOGI_EQUIPO
                If grdLista.Rows.Count > 0 Then
                    'If IsNothing(grdLista.SelectedRow) = True Then 'JMUG: 16/03/2023
                    If hfdOperacion.Value = "E" Then 'JMUG: 16/03/2023
                        'Equipo = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
                        Equipo = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, grdLista.SelectedRow.Cells(5).Text)
                    End If
                End If

                'txtIdEquipoComponente.Text = Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim
                hfdIdCatalogoEquipoComponente.Value = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim
                txtIdEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdEquipo").ToString.Trim
                hfdFechaTransaccionEquipoComponente.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
                txtDescripcionEquipoComponente.Text = Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(2).Text)
                txtDescripcionAbreviadaEquipoComponente.Text = Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(5).Text)
                cboTipoActivoEquipoComponente.SelectedValue = grdEquipoComponente.SelectedRow.Cells(3).Text 'grdEquipoComponente.SelectedRow.Cells(4).Text
                ListarSistemaFuncionalEquipoComponenteCombo()
                cboSistemaFuncionalEquipoComponente.SelectedValue = grdEquipoComponente.SelectedRow.Cells(4).Text
                txtCuentaContableEquipoComponente.Text = ""
                txtCuentaContableLeasingEquipoComponente.Text = ""
                txtVidaUtilEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("VidaUtil").ToString.Trim
                txtPeriodoGarantiaEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("PeriodoGarantia").ToString.Trim
                txtPeriodoMinimoEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("PeriodoMinimo").ToString.Trim
                txtNroSerieEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("NroSerie").ToString.Trim
                txtNroParteEquipoComponente.Text = Session("CestaEquipoComponente").Rows(rowIndexDetalle)("NroParte").ToString.Trim
                hfdFechaRegistroTarjetaSAPEquipoComponente.Value = Equipo.dFechaRegistroTarjetaSAPEquipo.ToString.Trim
                hfdFechaManufacturaTarjetaSAPEquipoComponente.Value = Equipo.dFechaManufacturaTarjetaSAPEquipo.ToString.Trim
                hfdFechaCreacionEquipoComponente.Value = Equipo.dFechaCreacionEquipo.ToString.Trim
                hfdIdUsuarioCreacionEquipoComponente.Value = IIf(IsNothing(Equipo.cIdUsuarioCreacionEquipo), "", Equipo.cIdUsuarioCreacionEquipo)
                'DetEquipo.dFechaRegistroTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaRegistroTarjetaSAPEquipoComponente.Value), hfdFechaRegistroTarjetaSAPEquipoComponente.Value, Nothing))
                'DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaManufacturaTarjetaSAPEquipoComponente.Value), hfdFechaManufacturaTarjetaSAPEquipoComponente.Value, Nothing))
            End If
            CargarCestaCaracteristicaEquipoComponenteTemporal()
        Catch ex As Exception
            'lblMensajeCatalogoComponente.Text = ex.Message
            MyValidator.ErrorMessage = ex.Message
        End Try
    End Sub

    'Sub LlenarDataEquipo()
    '    Try
    '        If hfdOperacionEquipo.Value = "N" Then
    '            Dim CatalogoNeg As New clsCatalogoNegocios
    '            'Dim Catalogo = CatalogoNeg.CatalogoListarPorId(Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text), Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(3).Text), "1", "*")
    '            Dim Catalogo = CatalogoNeg.CatalogoListarPorId(cboCatalogoMaestroActivoPrincipal.SelectedValue, cboTipoActivoMaestroActivoPrincipal.SelectedValue, "0", "1")

    '            txtIdMaestroActivoPrincipal.Text = ""
    '            txtDescripcionMaestroActivoPrincipal.Text = txtDescripcion.Text
    '            'cboTipoActivoMaestroActivoPrincipal.SelectedValue = Catalogo.cIdTipoActivo
    '            'cboTipoActivoMaestroActivoPrincipal_SelectedIndexChanged(cboTipoActivoMaestroActivoPrincipal, New System.EventArgs())
    '            'cboSistemaFuncionalMaestroActivoPrincipal.SelectedValue = Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(4).Text)
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
    '        'CargarCestaCatalogoComponente()
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
        Me.rfvDescripcionEquipoSAP.EnableClientScript = bValidar
        'Me.rfvPerfil.EnableClientScript = bValidar
    End Sub

    Sub ValidarTextoSubirImagenEquipo(ByVal bValidar As Boolean)
        Me.rfvTituloSubirImagenEquipo.EnableClientScript = bValidar
        Me.rfvDescripcionSubirImagenEquipo.EnableClientScript = bValidar
        Me.rfvObservacionSubirImagenEquipo.EnableClientScript = bValidar
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
        hfdFechaTransaccionEquipoComponente.Value = Now
        txtIdEquipo.Text = ""
        txtDescripcionEquipo.Text = ""
        txtNroSerieEquipo.Text = ""
        txtNroParteEquipo.Text = ""
        txtTagEquipo.Text = ""
        txtCapacidadEquipo.Text = ""
        'fscIdentificadorComponente.Checked = False
        lnkEsComponenteOn.Visible = False
        lnkEsComponenteOff.Visible = True
        divSistemaFuncional.Visible = False
        lnkEsConContratoOn.Visible = False
        lnkEsConContratoOff.Visible = True
        divContratoReferencia.Visible = False
        cboContratoReferencia.SelectedIndex = -1
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
        cboTipoEquipo.SelectedIndex = -1
        txtDescripcionEquipoSAP.Text = ""
        cboCatalogo.SelectedIndex = -1
        cboSistemaFuncional.SelectedIndex = -1
        hfdEstado.Value = "1"

        hfdIdAuxiliar.Value = ""
        hfdIdTipoPersonaCliente.Value = ""
        hfdIdTipoCliente.Value = ""
        hfdIdUbicacionGeograficaCliente.Value = ""
        hfdNroDocumentoCliente.Value = ""
        hfdDireccionFiscalCliente.Value = ""
        hfdTelefonoContactoCliente.Value = ""
        txtIdCliente.Text = ""
        txtRazonSocial.Text = ""

        hfdCorreoElectronicoCliente.Value = ""
        hfdDNICliente.Value = ""
        hfdRUCCliente.Value = ""
        hfdIdClienteSAPEquipo.Value = ""
        hfdIdEquipoSAPEquipo.Value = ""
        hfdFechaRegistroTarjetaSAPEquipo.Value = ""
        hfdFechaManufacturaTarjetaSAPEquipo.Value = ""
        hfdFechaCreacionEquipo.Value = ""
        hfdIdUsuarioCreacionEquipo.Value = ""
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

    Sub LimpiarObjetosCatalogoComponente()
        MyValidator.ErrorMessage = ""
        txtIdCatalogoComponente.Text = ""
        cboTipoActivoCatalogoComponente.SelectedIndex = -1
        cboSistemaFuncionalCatalogoComponente.SelectedIndex = -1
        txtDescripcionCatalogoComponente.Text = ""
        txtDescripcionAbreviadaCatalogoComponente.Text = ""
        txtVidaUtilCatalogoComponente.Text = ""
        txtCuentaContableCatalogoComponente.Text = ""
        txtCuentaContableLeasingCatalogoComponente.Text = ""
    End Sub

    Sub LimpiarObjetosEquipoComponente()
        MyValidator.ErrorMessage = ""
        txtIdEquipoComponente.Text = ""
        cboTipoActivoEquipoComponente.SelectedIndex = -1
        cboSistemaFuncionalEquipoComponente.SelectedIndex = -1
        txtDescripcionEquipoComponente.Text = ""
        txtDescripcionAbreviadaEquipoComponente.Text = ""
        txtVidaUtilEquipoComponente.Text = ""
        txtCuentaContableEquipoComponente.Text = ""
        txtCuentaContableLeasingEquipoComponente.Text = ""
    End Sub


    Protected Sub Upload_File(sender As Object, e As EventArgs)
        If hfdOperacion.Value = "N" Then
            'GenerarComprobante()
        End If
        btnNuevo.Enabled = True
    End Sub

    Sub LimpiarObjetosCaracteristicas()
        Me.lblMensajeCaracteristica.Text = ""
        txtValorCaracteristica.Text = ""
        txtIdReferenciaSAPCaracteristica.Text = ""
        txtDescripcionCampoSAPCaracteristica.Text = ""
        divIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
        txtIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
        divDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        txtDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetScrollPosition", "prm.add_beginRequest(BeginRequestHandler); prm.add_endRequest(EndRequestHandler);", True)
            strOpcionModulo = "127" 'Mantenimiento de los Maestros Activos / Componente.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroEquipo.SelectedIndex = 4
            ListarTipoActivoCombo()
            ListarTipoEquipoCombo()
            ListarCatalogoCombo()
            ListarSistemaFuncionalCombo()
            ListarDescripcionCatalogoComponente()
            ListarCaracteristicaEquipoPrincipalCombo()
            ListarCaracteristicaCatalogoComponenteCombo()
            ListarCaracteristicaEquipoComponenteCombo()
            ListarContratoReferenciaCombo()
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
            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()

            If Session("CestaCatalogoCaracteristica") Is Nothing Then
                Session("CestaCatalogoCaracteristica") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            End If
            'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            'Me.grdDetalleCaracteristica.DataBind()


            If Session("CestaCaracteristicaCatalogoComponente") Is Nothing Then
                Session("CestaCaracteristicaCatalogoComponente") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            End If


            If Session("CestaCaracteristicaEquipoPrincipal") Is Nothing Then
                Session("CestaCaracteristicaEquipoPrincipal") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoPrincipal"))
            End If

            'If Session("CestaCatalogoComponente") Is Nothing Then
            '    Session("CestaCatalogoComponente") = CrearCesta()
            'Else
            '    VaciarCesta(Session("CestaCatalogoComponente"))
            'End If

            If Session("CestaCatalogoComponente") Is Nothing Then
                Session("CestaCatalogoComponente") = clsLogiCestaCatalogo.CrearCesta()
            Else
                clsLogiCestaEquipo.VaciarCesta(Session("CestaCatalogoComponente"))
            End If

            If Session("CestaCaracteristicaCatalogoComponente") Is Nothing Then
                Session("CestaCaracteristicaCatalogoComponente") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
            End If

            If Session("CestaEquipoComponente") Is Nothing Then
                Session("CestaEquipoComponente") = clsLogiCestaEquipo.CrearCesta()
            Else
                clsLogiCestaEquipo.VaciarCesta(Session("CestaEquipoComponente"))
            End If

            If Session("CestaCaracteristicaEquipoComponente") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponente") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponente"))
            End If

            'BloquearPagina(1)
            BloquearMantenimiento(True, False, True, False)

            'Dim dtEstado = EquipoNeg.EquipoGetData("SELECT ISNULL(cEstadoEquipo, 'R') AS cEstadoEquipo FROM LOGI_EQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "' AND cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")
            'If dtEstado.Rows.Count > 0 Then

            'End If
            EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")

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
        cboTipoActivoCatalogoComponente.SelectedValue = cboTipoActivo.SelectedValue
        ListarCatalogoCombo()
        CargarCestaCatalogoComponente()
        'CargarCestaMaestroActivo()

        CargarCestaEquipoComponente()

    End Sub


    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0651", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = False
            pnlEquipo.Visible = True
            pnlComponentes.Visible = True

            hfdOperacion.Value = "N"
            'BloquearPagina(2)
            txtDescripcionEquipo.Focus()

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            CargarCestaCatalogoComponente()
            CargarCestaEquipoComponente()
            CargarCestaCaracteristicaEquipoPrincipal()
            CargarCestaCaracteristicaEquipoComponente()
            'CargarCestaCatalogo()
            ValidarTexto(True)
            ActivarObjetos(True)
            'hfTab.Value = "tab2"
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub



    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        'Dim dtEstado = EquipoNeg.EquipoGetData("SELECT ISNULL(cEstadoEquipo, 'R') AS cEstadoEquipo FROM LOGI_EQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
        'If dtEstado.Rows(0).Item(0) = "R" Then 'TERMINADO
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0651", strOpcionModulo, "CMMS")
            EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdEquipo = '" & txtIdEquipo.Text & "'")
            'ElseIf dtEstado.Rows(0).Item(0) = "T" Then 'TERMINADO
            '    EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
            '    'ElseIf dtEstado.Rows(0).Item(0) = "B" Then 'BLOQUEADO
            '    '    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
            '    '    Me.grdLista.DataBind()
            '    '    Throw New Exception("Este registro se encuentra bloqueado y utilizado por otro usuario.")
            'End If

            pnlCabecera.Visible = True
            pnlEquipo.Visible = False
            pnlComponentes.Visible = False
            BloquearMantenimiento(True, False, True, False)
            'ValidationSummary1.ValidationGroup = "vgrpValidar"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidar"
            'Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click

    'End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0652", strOpcionModulo, "CMMS")

            If IsNothing(grdLista.SelectedRow) = False Then
                If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                    hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                End If
                If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                    Throw New Exception("Este registro se encuentra desactivado.")
                End If

                Dim dtEstado = EquipoNeg.EquipoGetData("SELECT ISNULL(cEstadoEquipo, 'R') AS cEstadoEquipo FROM LOGI_EQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                If dtEstado.Rows(0).Item(0) = "R" Then 'TERMINADO
                    EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                ElseIf dtEstado.Rows(0).Item(0) = "T" Then 'TERMINADO
                    EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                ElseIf dtEstado.Rows(0).Item(0) = "B" Then 'BLOQUEADO
                    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                    Me.grdLista.DataBind()
                    Throw New Exception("Este registro se encuentra bloqueado y utilizado por otro usuario.")
                End If
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
                'divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, False, True)
            Else
                Throw New Exception("Debe de seleccionar un item.")
            End If
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
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeCaracteristica.Text = ""
            'If Session("IdEmpresa") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            '    Exit Sub
            'End If
            'LimpiarObjetosProducto()
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            lnkbtnImprimirTarjetaEquipo.Attributes.Add("onclick", "javascript:popupEmitirEquipoDetalleReporte('" & txtIdEquipo.Text & "');")
                            lnkbtnVerOrdenFabricacion.Attributes.Add("onclick", "javascript:popupEmitirOrdenFabricacionReporte('" & txtIdEquipo.Text & "');")
                            lnkbtnVerOrdenTrabajo.Attributes.Add("onclick", "javascript:popupEmitirOrdenTrabajoReporte('" & txtIdEquipo.Text & "');")
                            If MyValidator.ErrorMessage = "" Then
                                'lblMensaje.Text = "Registro encontrado con éxito"
                                MyValidator.ErrorMessage = "Registro encontrado con éxito"
                            End If
                            'txtIdReferenciaSAPCaracteristica.Text = ""
                            'txtDescripcionCampoSAPCaracteristica.Text = ""
                            'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                        End If
                    End If
                Else
                    'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()

                End If
            End If
            'ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            'Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
        'If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
        '    hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text) = "True", "1", "0")
        '    'JMUG: 25/02/2023 BloquearPagina(0)
        '    ValidarTexto(False)
        '    LlenarData()
        'End If
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
                        clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoMantenimientoCatalogo.Text, "", "0", grdListaCaracteristica.SelectedValue.trim, Server.HtmlDecode(grdListaCaracteristica.SelectedRow.Cells(1).Text).Trim, UCase(txtIdReferenciaSAPCaracteristica.Text.Trim), UCase(txtDescripcionCampoSAPCaracteristica.Text.Trim), UCase(txtValorCaracteristica.Text.Trim), Session("CestaCatalogoCaracteristica"))
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
            Catalogo.cIdSistemaFuncional = Nothing '""
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
                                                   Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
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
                    'BloquearMantenimiento(True, False, True, False)
                    BloquearMantenimiento(False, True, False, True)
                    'hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            ElseIf hfdOperacionDetalle.Value = "E" Then
                If (CatalogoNeg.CatalogoEdita(Catalogo)) = 0 Then
                    Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" &
                                               Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
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
                    'BloquearMantenimiento(True, False, True, False)
                    BloquearMantenimiento(False, True, False, True)
                    'hfTab.Value = "tab1"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    'Exit Sub
                End If
            End If
            hfdOperacionDetalle.Value = "R"
            Dim strIdCatalogo As String
            strIdCatalogo = cboCatalogo.SelectedValue
            ListarCatalogoCombo()
            cboCatalogo.SelectedValue = strIdCatalogo
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
        CargarCestaCatalogoComponente()
        CargarCestaEquipoComponente()
    End Sub

    Private Sub lnkbtnNuevoCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0672", strOpcionModulo, "CMMS")
            If cboCatalogo.SelectedValue = "SELECCIONE DATO" Then
                Throw New Exception("Debe de ingresar un catalogo para asignar sus componentes.")
            End If
            hfdOperacionDetalle.Value = "N"
            LlenarDataCatalogoComponente()
            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
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
    Protected Sub btnAceptarCatalogoComponente_Click(sender As Object, e As EventArgs) Handles btnAceptarCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            'JMUG: 01/03/2023
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0379", strOpcionModulo, Session("IdSistema"))

            If cboTipoActivoCatalogoComponente.SelectedIndex <= 0 Then
                Throw New Exception("Debe de ingresar el código del tipo de activo correcto.")
            ElseIf cboSistemaFuncionalCatalogoComponente.SelectedIndex < 0 Then
                Throw New Exception("Debe de ingresar el código del sistema funcional correcto.")
            ElseIf txtDescripcionCatalogoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción correcta.")
            ElseIf txtDescripcionAbreviadaCatalogoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción abreviada correcta.")
            Else
                Dim Coleccion As New List(Of LOGI_CATALOGO)
                For i = 0 To 0
                    Dim DetCatalogo As New LOGI_CATALOGO
                    DetCatalogo.cIdCatalogo = ""  'Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
                    DetCatalogo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                    DetCatalogo.cIdJerarquiaCatalogo = "1" 'Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
                    DetCatalogo.cIdSistemaFuncional = cboSistemaFuncionalCatalogoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString.Trim
                    DetCatalogo.cIdTipoActivo = cboTipoActivoCatalogoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString.Trim
                    DetCatalogo.vDescripcionCatalogo = UCase(txtDescripcionCatalogoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion").ToString.Trim
                    DetCatalogo.vDescripcionAbreviadaCatalogo = UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString.Trim
                    DetCatalogo.bEstadoRegistroCatalogo = True 'Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
                    'DetCatalogo.vDimensionesCatalogo = Session("CestaCatalogo").Rows(i)("Dimensiones").ToString.Trim
                    'DetCatalogo.vVoltajeCatalogo = Session("CestaCatalogo").Rows(i)("Voltaje").ToString.Trim
                    'DetCatalogo.vPesoCatalogo = Session("CestaCatalogo").Rows(i)("Peso").ToString.Trim
                    DetCatalogo.nVidaUtilCatalogo = Convert.ToInt32(IIf(txtVidaUtilCatalogoComponente.Text.Trim = "", "0", txtVidaUtilCatalogoComponente.Text)) 'Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
                    DetCatalogo.nPeriodoGarantiaCatalogo = Convert.ToInt32(IIf(txtPeriodoGarantiaMantenimientoCatalogo.Text.Trim = "", "0", txtPeriodoGarantiaMantenimientoCatalogo.Text))
                    DetCatalogo.nPeriodoMinimoMantenimientoCatalogo = Convert.ToInt32(IIf(txtPeriodoMinimoCatalogoComponente.Text.Trim = "", "0", txtPeriodoMinimoCatalogoComponente.Text))
                    DetCatalogo.cIdCuentaContableCatalogo = txtCuentaContableCatalogoComponente.Text 'Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
                    DetCatalogo.cIdCuentaContableLeasingCatalogo = txtCuentaContableLeasingCatalogoComponente.Text  'Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString.Trim
                    'DetCatalogo.bActivacionAutomaticaCatalogo = Catalogo.bActivacionAutomaticaCatalogo
                    Coleccion.Add(DetCatalogo)
                Next

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                If hfdOperacion.Value = "E" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                        For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                            Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                            DetCaracteristica.cIdCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                            DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            ColeccionCaracteristica.Add(DetCaracteristica)
                        Next
                        If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                            clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                                         txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                                         "1", txtVidaUtilCatalogoComponente.Text.Trim,
                                                         txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                                         cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                        End If
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                            Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                                Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                DetCaracteristica.cIdCatalogo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                                DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                                DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                            'If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then
                            If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                                clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                     "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                     txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                     "1", IIf(txtVidaUtilCatalogoComponente.Text.Trim = "", "0", txtVidaUtilCatalogoComponente.Text.Trim),
                                     txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                     cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"))
                            End If
                        End If
                    End If
                ElseIf hfdOperacion.Value = "N" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        'clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                        '                                 "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                        '                                 txtIdEquipo.Text.Trim, txtDescripcionCatalogoComponente.Text.Trim, txtDescripcionAbreviadaCatalogoComponente.Text.Trim,
                        '                                 "1", txtVidaUtilCatalogoComponente.Text.Trim,
                        '                                 txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                        '                                 cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                        Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                        For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                            Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                            DetCaracteristica.cIdCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                            DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            ColeccionCaracteristica.Add(DetCaracteristica)
                        Next
                        If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                            clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                                         "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                                         txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                                         "1", txtVidaUtilCatalogoComponente.Text.Trim,
                                                         txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                                         cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                        End If
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        'If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                        '    'If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then
                        '    If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                        '        clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                        '                                  "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                        '                                  txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                        '                                  "1", txtVidaUtilCatalogoComponente.Text.Trim,
                        '                                  txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                        '                                  cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"))
                        '    End If
                        'End If
                        If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                            Dim ColeccionCaracteristica As New List(Of LOGI_CATALOGOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                                Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                DetCaracteristica.cIdCatalogo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                                DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                                DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                            'If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then
                            If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                                clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                                     "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                                     txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                                     "1", IIf(txtVidaUtilCatalogoComponente.Text.Trim = "", "0", txtVidaUtilCatalogoComponente.Text.Trim),
                                     txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                                     cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"))
                            End If
                        End If
                    End If
                End If
            End If
            'Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
            'Me.grdListaDetalle.DataBind()
            'grdEquipoComponente.DataSource = Session("CestaCatalogoComponente")
            'grdEquipoComponente.DataBind()
            grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            grdCatalogoComponente.DataBind()
            '    Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Hide()
            'Catch ex As Exception
            '    'lblMensajeCatalogoComponente.Text = ex.Message
            '    MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
            '    Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            'End Try

            Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Hide()
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
            Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        End Try
    End Sub

    Protected Sub btnAceptarEquipoComponente_Click(sender As Object, e As EventArgs) Handles btnAceptarEquipoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            'JMUG: 01/03/2023
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0379", strOpcionModulo, Session("IdSistema"))

            If cboTipoActivoEquipoComponente.SelectedIndex <= 0 Then
                Throw New Exception("Debe de ingresar el código del tipo de activo correcto.")
            ElseIf cboSistemaFuncionalEquipoComponente.SelectedIndex < 0 Then
                Throw New Exception("Debe de ingresar el código del sistema funcional correcto.")
            ElseIf txtDescripcionEquipoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción correcta.")
            ElseIf txtDescripcionAbreviadaEquipoComponente.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la descripción abreviada correcta.")
            Else
                Dim Coleccion As New List(Of LOGI_EQUIPO)
                For i = 0 To 0
                    Dim DetEquipo As New LOGI_EQUIPO
                    DetEquipo.cIdCatalogo = Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text) '""  'Session("CestaCatalogo").Rows(i)("IdCatalogo").ToString.Trim
                    DetEquipo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                    DetEquipo.cIdEnlaceEquipo = txtIdEquipo.Text
                    DetEquipo.cIdEquipo = txtIdEquipoComponente.Text
                    DetEquipo.cIdJerarquiaCatalogo = "1" 'Session("CestaCatalogo").Rows(i)("IdJerarquia").ToString.Trim
                    DetEquipo.cIdSistemaFuncionalEquipo = cboSistemaFuncionalEquipoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdSistemaFuncional").ToString.Trim
                    DetEquipo.cIdTipoActivo = cboTipoActivoEquipoComponente.SelectedValue 'Session("CestaCatalogo").Rows(i)("IdTipoActivo").ToString.Trim
                    DetEquipo.vIdEquivalenciaEquipo = ""
                    DetEquipo.vDescripcionEquipo = UCase(txtDescripcionEquipoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion").ToString.Trim
                    DetEquipo.vDescripcionAbreviadaEquipo = UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim) 'Session("CestaCatalogo").Rows(i)("Descripcion_Abreviada").ToString.Trim
                    DetEquipo.bEstadoRegistroEquipo = True 'Session("CestaCatalogo").Rows(i)("Estado").ToString.Trim
                    'DetCatalogo.vDimensionesCatalogo = Session("CestaCatalogo").Rows(i)("Dimensiones").ToString.Trim
                    'DetCatalogo.vVoltajeCatalogo = Session("CestaCatalogo").Rows(i)("Voltaje").ToString.Trim
                    'DetCatalogo.vPesoCatalogo = Session("CestaCatalogo").Rows(i)("Peso").ToString.Trim
                    DetEquipo.nVidaUtilEquipo = Convert.ToInt32(IIf(txtVidaUtilEquipoComponente.Text.Trim = "", "0", txtVidaUtilEquipoComponente.Text)) 'Session("CestaCatalogo").Rows(i)("VidaUtil").ToString.Trim
                    DetEquipo.nPeriodoGarantiaEquipo = Convert.ToInt32(IIf(txtPeriodoGarantiaEquipoComponente.Text.Trim = "", "0", txtPeriodoGarantiaEquipoComponente.Text))
                    DetEquipo.nPeriodoMinimoMantenimientoEquipo = Convert.ToInt32(IIf(txtPeriodoMinimoEquipoComponente.Text.Trim = "", "0", txtPeriodoMinimoEquipoComponente.Text))
                    DetEquipo.vNumeroSerieEquipo = UCase(txtNroSerieEquipoComponente.Text.Trim)
                    DetEquipo.vNumeroParteEquipo = UCase(txtNroParteEquipoComponente.Text.Trim) 'DetEquipo.cIdCuentaContableEquipo = txtCuentaContableCatalogoComponente.Text 'Session("CestaCatalogo").Rows(i)("IdCuentaContable").ToString.Trim
                    'DetEquipo.cIdCuentaContableLeasingEquipo = txtCuentaContableLeasingCatalogoComponente.Text  'Session("CestaCatalogo").Rows(i)("IdCuentaContableLeasing").ToString.Trim
                    'DetCatalogo.bActivacionAutomaticaCatalogo = Catalogo.bActivacionAutomaticaCatalogo
                    DetEquipo.cIdCliente = hfdIdAuxiliar.Value
                    DetEquipo.cIdEmpresa = Session("IdEmpresa")
                    DetEquipo.cIdEstadoComponenteEquipo = "01" '""
                    DetEquipo.cIdPaisOrigenEquipo = Session("IdPais")
                    DetEquipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
                    DetEquipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
                    DetEquipo.dFechaTransaccionEquipo = hfdFechaTransaccionEquipoComponente.Value
                    'DetEquipo.dFechaRegistroTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaRegistroTarjetaSAPEquipoComponente.Value), IIf(hfdFechaRegistroTarjetaSAPEquipoComponente.Value = "", Now, hfdFechaRegistroTarjetaSAPEquipoComponente.Value), Nothing))
                    'DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaManufacturaTarjetaSAPEquipoComponente.Value), IIf(hfdFechaManufacturaTarjetaSAPEquipoComponente.Value = "", Now, hfdFechaManufacturaTarjetaSAPEquipoComponente.Value), Nothing))
                    DetEquipo.dFechaRegistroTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaRegistroTarjetaSAPEquipoComponente.Value), hfdFechaRegistroTarjetaSAPEquipoComponente.Value, Now))
                    DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaManufacturaTarjetaSAPEquipoComponente.Value), hfdFechaManufacturaTarjetaSAPEquipoComponente.Value, Now))
                    Coleccion.Add(DetEquipo)
                Next

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                If hfdOperacion.Value = "E" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        If Coleccion(0).cIdEquipo = "" Then
                            'If EquipoNeg.EquipoInserta(Coleccion(0)) = 0 Then
                            '    Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                            '    For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                            '        Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                            '        'DetCaracteristica.cIdEquipo = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            '        DetCaracteristica.cIdEquipo = Coleccion(0).cIdEquipo 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                            '        DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                            '        DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                            '        'DetCaracteristica.cIdJerarquiaEquipo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            '        'DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            '        'DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            '        DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                            '        DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                            '        DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            '        DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            '        ColeccionCaracteristica.Add(DetCaracteristica)
                            '    Next
                            '    If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                            '        'clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                            '        '                             "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                            '        '                             txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                            '        '                             "1", txtVidaUtilCatalogoComponente.Text.Trim,
                            '        '                             txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                            '        '                             cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                            '        'clsLogiCestaCatalogo.EditarCesta(Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim, IIf(cboTipoActivoEquipoComponente.SelectedIndex > 0, cboTipoActivoEquipoComponente.SelectedValue, " "),
                            '        '                             "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                            '        '                             txtIdEquipo.Text.Trim, UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                            '        '                             "1", txtVidaUtilEquipoComponente.Text.Trim,
                            '        '                             txtCuentaContableEquipoComponente.Text.Trim, txtCuentaContableLeasingEquipoComponente.Text.Trim, cboTipoActivoEquipoComponente.SelectedItem.Text,
                            '        '                             cboSistemaFuncionalEquipoComponente.SelectedItem.Text, txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim, Session("CestaEquipoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                            '        'clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text.Trim, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                            '        '                   Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                            '        '                   "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(4).Text),
                            '        '                   Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim,
                            '        '                   Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text),
                            '        '                   Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(5).Text), Now,
                            '        '                   "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                            '        '                   Session("CestaEquipoComponente"))
                            '        'ESTEEEEE ES EL CORRECTO : 08/03/2023
                            '        clsLogiCestaEquipo.EditarCesta(Coleccion(0).cIdEquipo, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                            '                   Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                            '                   "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                            '                   cboCatalogo.SelectedValue,
                            '                    UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                            '                    Now,
                            '                   "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                            '                   txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim,
                            '                   Session("CestaEquipoComponente"), rowIndexDetalle)
                            '        MyValidator.ErrorMessage = "Componente editado con éxito."
                            '    End If
                            'End If

                            clsLogiCestaEquipo.EditarCesta(Coleccion(0).cIdEquipo, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)
                            MyValidator.ErrorMessage = "Componente editado con éxito."
                        Else
                            ' If EquipoNeg.EquipoEdita(Coleccion(0)) = 0 Then
                            'Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                            '    For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                            '        Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                            '        'DetCaracteristica.cIdEquipo = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            '        DetCaracteristica.cIdEquipo = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                            '        DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                            '        DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                            '        'DetCaracteristica.cIdJerarquiaEquipo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            '        'DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            '        'DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            '        DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                            '        DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                            '        DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            '        DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            '        ColeccionCaracteristica.Add(DetCaracteristica)
                            '    Next
                            '    If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                            '        'clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                            '        '                             "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                            '        '                             txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                            '        '                             "1", txtVidaUtilCatalogoComponente.Text.Trim,
                            '        '                             txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                            '        '                             cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                            '        'clsLogiCestaCatalogo.EditarCesta(Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim, IIf(cboTipoActivoEquipoComponente.SelectedIndex > 0, cboTipoActivoEquipoComponente.SelectedValue, " "),
                            '        '                             "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                            '        '                             txtIdEquipo.Text.Trim, UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                            '        '                             "1", txtVidaUtilEquipoComponente.Text.Trim,
                            '        '                             txtCuentaContableEquipoComponente.Text.Trim, txtCuentaContableLeasingEquipoComponente.Text.Trim, cboTipoActivoEquipoComponente.SelectedItem.Text,
                            '        '                             cboSistemaFuncionalEquipoComponente.SelectedItem.Text, txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim, Session("CestaEquipoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                            '        'clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text.Trim, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                            '        '                   Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                            '        '                   "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(4).Text),
                            '        '                   Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim,
                            '        '                   Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text),
                            '        '                   Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(5).Text), Now,
                            '        '                   "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                            '        '                   Session("CestaEquipoComponente"))
                            '        'ESTEEEEE ES EL CORRECTO : 08/03/2023
                            '        clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text.Trim, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                            '                   Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                            '                   "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                            '                   cboCatalogo.SelectedValue,
                            '                    UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                            '                    Now,
                            '                   "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                            '                   txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim,
                            '                   Session("CestaEquipoComponente"), rowIndexDetalle)
                            '        MyValidator.ErrorMessage = "Componente editado con éxito."
                            '    End If
                            'End If

                            clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text.Trim, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)

                            Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                                For j = 0 To Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows.Count - 1
                                    If (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim) = Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdCatalogo").ToString.Trim And
                                   (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim) = Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdJerarquia").ToString.Trim And
                                   (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdCaracteristica").ToString.Trim Then
                                        'JMUG: 29/03/2023 clsLogiCestaCatalogoCaracteristica.EditarCesta(Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim, "", Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("Descripcion").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdReferenciaSAP").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("DescripcionCampoSAP").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("Valor").ToString.Trim, Session("CestaCaracteristicaEquipoComponente"), i)
                                        clsLogiCestaCatalogoCaracteristica.EditarCesta(Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim, txtIdEquipoComponente.Text.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim, Session("CestaCaracteristicaEquipoComponente").Rows(i)("Descripcion").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("IdReferenciaSAP").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("DescripcionCampoSAP").ToString.Trim, Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(j)("Valor").ToString.Trim, Session("CestaCaracteristicaEquipoComponente"), i)
                                    End If
                                Next
                                'Dim DetCaracteristica As New LOGI_CATALOGOCARACTERISTICA
                                'DetCaracteristica.cIdCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                                'DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                                'DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                'DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                                'DetCaracteristica.vValorCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Valor").ToString.Trim
                                'DetCaracteristica.cIdReferenciaSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                'DetCaracteristica.vDescripcionCampoSAPCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                'ColeccionCaracteristica.Add(DetCaracteristica)
                            Next

                            MyValidator.ErrorMessage = "Componente editado con éxito."
                        End If
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        'If EquipoNeg.EquipoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                        If EquipoNeg.EquipoInsertaDetalle(Coleccion, txtIdEquipo.Text, cboCatalogo.SelectedValue) = 0 Then
                            Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                            For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                                Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                                DetCaracteristica.cIdEquipo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                                DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                                'DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                                DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                                DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                                DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                            'If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then
                            If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(txtIdEquipo.Text, ColeccionCaracteristica, LogAuditoria) Then
                                clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoEquipoComponente.SelectedIndex > 0, cboTipoActivoEquipoComponente.SelectedValue, " "),
                                         "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                         txtIdEquipo.Text.Trim, UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                         "1", IIf(txtVidaUtilEquipoComponente.Text.Trim = "", "0", txtVidaUtilEquipoComponente.Text.Trim),
                                         txtCuentaContableEquipoComponente.Text.Trim, txtCuentaContableLeasingEquipoComponente.Text.Trim, cboTipoActivoEquipoComponente.SelectedItem.Text,
                                         cboSistemaFuncionalEquipoComponente.SelectedItem.Text, txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim, Session("CestaEquipoComponente"))
                            End If
                            MyValidator.ErrorMessage = "Componente agregado con éxito."
                        End If
                    End If
                ElseIf hfdOperacion.Value = "N" Then
                    If hfdOperacionDetalle.Value = "E" Then
                        ''clsLogiCestaCatalogo.EditarCesta(txtIdCatalogoComponente.Text.Trim, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                        ''                                 "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                        ''                                 txtIdEquipo.Text.Trim, txtDescripcionCatalogoComponente.Text.Trim, txtDescripcionAbreviadaCatalogoComponente.Text.Trim,
                        ''                                 "1", txtVidaUtilCatalogoComponente.Text.Trim,
                        ''                                 txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                        ''                                 cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                        'Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                        'For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                        '    Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                        '    'DetCaracteristica.cIdEquipo = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                        '    DetCaracteristica.cIdEquipo = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                        '    'DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                        '    DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                        '    DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                        '    DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                        '    DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                        '    DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                        '    DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                        '    ColeccionCaracteristica.Add(DetCaracteristica)
                        'Next
                        'If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                        '    clsLogiCestaCatalogo.EditarCesta(txtIdEquipoComponente.Text.Trim, IIf(cboTipoActivoEquipoComponente.SelectedIndex > 0, cboTipoActivoEquipoComponente.SelectedValue, " "),
                        '                                 "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                        '                                 txtIdEquipo.Text.Trim, UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                        '                                 "1", txtVidaUtilEquipoComponente.Text.Trim,
                        '                                 txtCuentaContableEquipoComponente.Text.Trim, txtCuentaContableLeasingEquipoComponente.Text.Trim, cboTipoActivoEquipoComponente.SelectedItem.Text,
                        '                                 cboSistemaFuncionalEquipoComponente.SelectedItem.Text, txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim, Session("CestaEquipoComponente"), rowIndexDetalle) 'grdListaDetalle.SelectedIndex)
                        'End If
                        clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text.Trim, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)
                        MyValidator.ErrorMessage = "Componente editado con éxito."
                    ElseIf hfdOperacionDetalle.Value = "N" Then
                        clsLogiCestaEquipo.EditarCesta(txtIdEquipoComponente.Text, Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                                               "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                                               cboCatalogo.SelectedValue,
                                                UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                                                Now,
                                               "1", txtIdEquipo.Text.Trim, "", txtVidaUtilEquipoComponente.Text.Trim, "", "", txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim,
                                               txtNroSerieEquipoComponente.Text.Trim, txtNroParteEquipoComponente.Text.Trim, "01",
                                               Session("CestaEquipoComponente"), rowIndexDetalle)

                        Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                        For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                            Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                            'DetCaracteristica.cIdEquipo = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                            DetCaracteristica.cIdEquipo = Coleccion(0).cIdEquipo 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                            DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                            DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                            'DetCaracteristica.cIdJerarquiaEquipo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                            'DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                            'DetCaracteristica.nIdNumeroItemCatalogoCaracteristica = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                            DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                            DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                            DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                            ColeccionCaracteristica.Add(DetCaracteristica)
                        Next


                        'clsLogiCestaCatalogoCaracteristica.AgregarCesta(Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim, txtIdEquipoComponente.Text, "1", cboCaracteristicaEquipoComponente.SelectedValue.Trim, UCase(cboCaracteristicaEquipoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoComponente"))


                        ''If CatalogoNeg.CatalogoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                        ''    'If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then
                        ''    If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                        ''        clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoCatalogoComponente.SelectedIndex > 0, cboTipoActivoCatalogoComponente.SelectedValue, " "),
                        ''                                  "1", IIf(cboSistemaFuncionalCatalogoComponente.SelectedIndex >= 0, cboSistemaFuncionalCatalogoComponente.SelectedValue, " "),
                        ''                                  txtIdEquipo.Text.Trim, UCase(txtDescripcionCatalogoComponente.Text.Trim), UCase(txtDescripcionAbreviadaCatalogoComponente.Text.Trim),
                        ''                                  "1", txtVidaUtilCatalogoComponente.Text.Trim,
                        ''                                  txtCuentaContableCatalogoComponente.Text.Trim, txtCuentaContableLeasingCatalogoComponente.Text.Trim, cboTipoActivoCatalogoComponente.SelectedItem.Text,
                        ''                                  cboSistemaFuncionalCatalogoComponente.SelectedItem.Text, txtPeriodoGarantiaCatalogoComponente.Text.Trim, txtPeriodoMinimoCatalogoComponente.Text.Trim, Session("CestaCatalogoComponente"))
                        ''    End If
                        ''End If
                        'If EquipoNeg.EquipoInsertaDetalle(Coleccion, cboCatalogo.SelectedValue) = 0 Then
                        '    Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                        '    For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                        '        Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                        '        'DetCaracteristica.cIdCatalogo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                        '        DetCaracteristica.cIdEquipo = Coleccion(0).cIdCatalogo 'Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim
                        '        'DetCaracteristica.cIdJerarquiaCatalogo = Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdJerarquia").ToString.Trim
                        '        DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                        '        DetCaracteristica.cIdCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim 'Session("CestaCatalogo").Rows(i)("IdEnlace").ToString.Trim
                        '        DetCaracteristica.nIdNumeroItemEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                        '        DetCaracteristica.vValorReferencialEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim
                        '        DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                        '        DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                        '        ColeccionCaracteristica.Add(DetCaracteristica)
                        '    Next
                        '    'If CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCaracteristica, LogAuditoria) Then
                        '    If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(ColeccionCaracteristica, LogAuditoria) Then
                        '        clsLogiCestaCatalogo.AgregarCesta(Coleccion(0).cIdCatalogo, IIf(cboTipoActivoEquipoComponente.SelectedIndex > 0, cboTipoActivoEquipoComponente.SelectedValue, " "),
                        '             "1", IIf(cboSistemaFuncionalEquipoComponente.SelectedIndex >= 0, cboSistemaFuncionalEquipoComponente.SelectedValue, " "),
                        '             txtIdEquipo.Text.Trim, UCase(txtDescripcionEquipoComponente.Text.Trim), UCase(txtDescripcionAbreviadaEquipoComponente.Text.Trim),
                        '             "1", IIf(txtVidaUtilEquipoComponente.Text.Trim = "", "0", txtVidaUtilEquipoComponente.Text.Trim),
                        '             txtCuentaContableEquipoComponente.Text.Trim, txtCuentaContableLeasingEquipoComponente.Text.Trim, cboTipoActivoEquipoComponente.SelectedItem.Text,
                        '             cboSistemaFuncionalEquipoComponente.SelectedItem.Text, txtPeriodoGarantiaEquipoComponente.Text.Trim, txtPeriodoMinimoEquipoComponente.Text.Trim, Session("CestaEquipoComponente"))
                        '    End If
                        '    MyValidator.ErrorMessage = "Componente agregado con éxito."
                        'End If
                    End If
                End If
            End If
            'Me.grdListaDetalle.DataSource = Session("CestaCatalogo")
            'Me.grdListaDetalle.DataBind()
            'grdEquipoComponente.DataSource = Session("CestaCatalogoComponente")
            'grdEquipoComponente.DataBind()
            grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            grdEquipoComponente.DataBind()
            '    Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Hide()
            'Catch ex As Exception
            '    'lblMensajeCatalogoComponente.Text = ex.Message
            '    MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
            '    Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            'End Try

            Me.lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Hide()
            ValidationSummary4.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarEquipoComponente"
            Me.Page.Validators.Add(MyValidator)
            Me.lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub cboTipoActivoCatalogoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivoCatalogoComponente.SelectedIndexChanged
        ListarSistemaFuncionalCatalogoComponenteCombo()
        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnEditarSistemaFuncionalCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarSistemaFuncionalCatalogoComponente.Click
        hfdOperacionDetalleAdicional.Value = "E"
        lnk_mostrarPanelMantenimientoSistemaFuncional_ModalPopupExtender.Show()
    End Sub

    Private Sub lnkbtnNuevoSistemaFuncionalCatalogo_Click(sender As Object, e As EventArgs) Handles lnkbtnNuevoSistemaFuncionalCatalogoComponente.Click
        hfdOperacionDetalleAdicional.Value = "N"
        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
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
            'SistemaFuncional.cIdCatalogo = cboCatalogo.SelectedValue
            'SistemaFuncional.cIdTipoActivoSistemaFuncional = cboTipoActivo.SelectedValue
            'SistemaFuncional.cIdJerarquiaCatalogo = "0"

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
                    'Session("Query") = "PA_LOGI_MNT_SISTEMAFUNCIONAL 'SQL_INSERT', '','" & SistemaFuncional.cIdSistemaFuncional & "', '" &
                    '                   SistemaFuncional.cIdCatalogo & "', '" &
                    '                   SistemaFuncional.cIdJerarquiaCatalogo & "', '" & SistemaFuncional.vDescripcionSistemaFuncional & "', '" & SistemaFuncional.vDescripcionAbreviadaSistemaFuncional & "', '" &
                    '                   SistemaFuncional.bEstadoRegistroSistemaFuncional & "', '" & SistemaFuncional.cIdSistemaFuncional & "'"
                    Session("Query") = "PA_LOGI_MNT_SISTEMAFUNCIONAL 'SQL_INSERT', '','" & SistemaFuncional.cIdSistemaFuncional & "', '" &
                                       SistemaFuncional.vDescripcionSistemaFuncional & "', '" & SistemaFuncional.vDescripcionAbreviadaSistemaFuncional & "', '" &
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
                                       SistemaFuncional.vDescripcionSistemaFuncional & "', '" & SistemaFuncional.vDescripcionAbreviadaSistemaFuncional & "', '" &
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
            ListarSistemaFuncionalCombo()
            ListarSistemaFuncionalCatalogoComponenteCombo()
            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
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

    Private Sub lnkbtnEditarCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'lblMensajeCatalogoComponente.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))
            'Función para validar si tiene permisos
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0673", strOpcionModulo, "CMMS")

            'JMUG: 04/03/2023
            'If Session("CestaCatalogoCaracteristicaFiltrado") Is Nothing Then
            '    Session("CestaCatalogoCaracteristicaFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            'Else
            '    clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristicaFiltrado"))
            'End If

            hfdOperacionDetalle.Value = "E"
            If grdCatalogoComponente.Rows.Count > 0 Then
                If grdCatalogoComponente.SelectedIndex < grdCatalogoComponente.Rows.Count Then
                    If IsNothing(grdCatalogoComponente.SelectedRow) = False Then
                        If IsReference(grdCatalogoComponente.SelectedRow.Cells(1).Text) = True Then
                            'Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(5).Text).Trim & "'")
                            Dim result As DataRow() = Session("CestaCatalogoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(3).Text).Trim & "'")
                            rowIndexDetalle = Session("CestaCatalogoComponente").Rows.IndexOf(result(0))
                            'LlenarDataDetalle()
                            BloquearMantenimiento(False, True, False, True)
                            ValidarTexto(True)
                            ActivarObjetos(True)
                            LlenarDataCatalogoComponente()
                            'CargarCestaCatalogoComponente()
                            'CargarCestaEquipoComponente()
                            'CargarCestaCaracteristicaCatalogoComponente()
                            ''CargarCestaCaracteristica()
                            ''Dim rowIndexDetalleCaracteristica As Int64
                            'Dim resultCaracteristica As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                            ''rowIndexDetalleCaracteristica = Session("CestaCatalogoCaracteristica").Rows.IndexOf(resultCaracteristica(0))
                            'If resultCaracteristica.Length = 0 Then
                            '    Me.grdDetalleCaracteristica.DataSource = Nothing
                            'Else
                            '    Me.grdDetalleCaracteristica.DataSource = resultCaracteristica(0).Table
                            'End If
                            'Me.grdDetalleCaracteristica.DataBind()





                            'JMUG: 04/03/2023
                            'Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaCatalogoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdJerarquia = '1'")
                            'If resultCaracteristicaSimple.Length = 0 Then
                            '    Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Nothing
                            'Else
                            '    Dim rowFil As DataRow() = resultCaracteristicaSimple
                            '    For Each fila As DataRow In rowFil
                            '        clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaCatalogoComponenteFiltrado"))
                            '    Next
                            'End If
                            'Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponenteFiltrado")
                            'Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()
                            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar algún item.")
                    End If
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
            'BloquearPagina(2)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaCatalogoComponente_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'lblMensajeCatalogoComponente.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            'If Session("CestaCatalogoCaracteristicaFiltrado") Is Nothing Then
            '    Session("CestaCatalogoCaracteristicaFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            'Else
            '    clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristicaFiltrado"))
            'End If

            'hfdOperacionDetalle.Value = "E"
            If hfdOperacionDetalle.Value = "N" Or hfdOperacionDetalle.Value = "E" Then
                'If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
                '    If lblMensajeCaracteristica.Text = "" Then
                For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                    'If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (hfdIdCaracteristicaCatalogoComponente.Value.ToString.Trim) And Session("CestaCatalogoCaracteristica").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogoComponente.Text Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                    If (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaCatalogoComponente.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogoComponente.Text.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                        'Dim resultCaracteristicaSimple As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                        'If resultCaracteristicaSimple.Length = 0 Then
                        '    Me.grdDetalleCaracteristica.DataSource = Nothing
                        'Else
                        '    Dim rowFil As DataRow() = resultCaracteristicaSimple
                        '    For Each fila As DataRow In rowFil
                        '        clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCatalogoCaracteristicaFiltrado"))
                        '    Next
                        'End If
                        'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristicaFiltrado")
                        'Me.grdDetalleCaracteristica.DataBind()
                        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()

                        Throw New Exception("Característica ya registrada, seleccione otro item.")
                        Exit Sub
                    End If
                Next
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", hfdIdCaracteristicaCatalogoComponente.Value.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristica"))
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "", "1", cboCaracteristicaCatalogoComponente.SelectedValue.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogoComponente"))
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", hfdIdCaracteristicaCatalogoComponente.Value.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristicaFiltrado"))
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", cboCaracteristicaCatalogoComponente.SelectedValue.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristicaFiltrado"))

                ''Dim rowIndexDetalleCaracteristica As Int64
                ''Dim resultCaracteristica As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                'Session("CestaCatalogoCaracteristica").row
                ''rowIndexDetalleCaracteristica = Session("CestaCatalogoCaracteristica").Rows.IndexOf(resultCaracteristica(0))
                'If resultCaracteristica.Length = 0 Then
                '    Me.grdDetalleCaracteristica.DataSource = Nothing
                'Else
                '    Me.grdDetalleCaracteristica.DataSource = resultCaracteristica(0).Table
                'End If
                'Me.grdDetalleCaracteristica.DataBind()

                Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponente")
                Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()
                'lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()


                'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
                'Me.grdDetalleCaracteristica.DataBind()

                'LimpiarObjetosCaracteristicas()
                'txtBuscarCaracteristicaCatalogoComponente.Text = ""
                cboCaracteristicaCatalogoComponente.SelectedIndex = -1
                'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
                grdDetalleCaracteristicaCatalogoComponente.SelectedIndex = -1
                'grdListaCaracteristica.SelectedIndex = -1
                'lblMensajeCaracteristica.Text = "Caracteristica agregada con éxito."
                'lblMensajeCatalogoComponente.Text = "Caracteristica agregada con éxito."
                MyValidator.ErrorMessage = "Caracteristica agregada con éxito."

            Else
                lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            End If
            '    End If
            'End If
            'End If

            'If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
            '            If IsNothing(grdListaDetalle.SelectedRow) = False Then
            '                If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then
            '                    Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(5).Text).Trim & "'")
            '                    rowIndexDetalle = Session("CestaCatalogo").Rows.IndexOf(result(0))
            '                    LlenarDataDetalle()
            '                    BloquearMantenimiento(False, True, False, True)
            '                    ValidarTexto(True)
            '                    ActivarObjetos(True)
            '                    lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            '                End If
            '            Else
            '                Throw New Exception("Debe de seleccionar algún item.")
            '            End If
            '        End If
            '    End If
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
            'BloquearPagina(2)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaEquipoComponente_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaEquipoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'lblMensajeCatalogoComponente.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0658", strOpcionModulo, "CMMS")

            'If Session("CestaCatalogoCaracteristicaFiltrado") Is Nothing Then
            '    Session("CestaCatalogoCaracteristicaFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            'Else
            '    clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCatalogoCaracteristicaFiltrado"))
            'End If

            'hfdOperacionDetalle.Value = "E"
            If hfdOperacionDetalle.Value = "N" Or hfdOperacionDetalle.Value = "E" Then
                'If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
                '    If lblMensajeCaracteristica.Text = "" Then
                For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                    'If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (hfdIdCaracteristicaCatalogoComponente.Value.ToString.Trim) And Session("CestaCatalogoCaracteristica").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogoComponente.Text Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                    If (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaEquipoComponente.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim = txtIdEquipoComponente.Text.Trim And Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim <> "" Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                        'Dim resultCaracteristicaSimple As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                        'If resultCaracteristicaSimple.Length = 0 Then
                        '    Me.grdDetalleCaracteristica.DataSource = Nothing
                        'Else
                        '    Dim rowFil As DataRow() = resultCaracteristicaSimple
                        '    For Each fila As DataRow In rowFil
                        '        clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCatalogoCaracteristicaFiltrado"))
                        '    Next
                        'End If
                        Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                        Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
                        lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()

                        Throw New Exception("Característica ya registrada, seleccione otro item.")
                        Exit Sub
                    End If
                Next
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", hfdIdCaracteristicaCatalogoComponente.Value.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristica"))
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "", "1", cboCaracteristicaCatalogoComponente.SelectedValue.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogoComponente"))
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", hfdIdCaracteristicaCatalogoComponente.Value.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristicaFiltrado"))
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", cboCaracteristicaCatalogoComponente.SelectedValue.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristicaFiltrado"))
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim, txtIdEquipoComponente.Text, "1", cboCaracteristicaEquipoComponente.SelectedValue.Trim, UCase(cboCaracteristicaEquipoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoComponente"))
                'clsLogiCestaCatalogoCaracteristica.AgregarCesta(Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdCatalogo").ToString.Trim, txtIdEquipoComponente.Text, "1", cboCaracteristicaEquipoComponente.SelectedValue.Trim, UCase(cboCaracteristicaEquipoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(hfdIdCatalogoEquipoComponente.Value, txtIdEquipoComponente.Text, "1", cboCaracteristicaEquipoComponente.SelectedValue.Trim, UCase(cboCaracteristicaEquipoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoComponente"))
                clsLogiCestaCatalogoCaracteristica.AgregarCesta(hfdIdCatalogoEquipoComponente.Value, txtIdEquipoComponente.Text, "1", cboCaracteristicaEquipoComponente.SelectedValue.Trim, UCase(cboCaracteristicaEquipoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoComponenteFiltrado"))

                ''Dim rowIndexDetalleCaracteristica As Int64
                ''Dim resultCaracteristica As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                'Session("CestaCatalogoCaracteristica").row
                ''rowIndexDetalleCaracteristica = Session("CestaCatalogoCaracteristica").Rows.IndexOf(resultCaracteristica(0))
                'If resultCaracteristica.Length = 0 Then
                '    Me.grdDetalleCaracteristica.DataSource = Nothing
                'Else
                '    Me.grdDetalleCaracteristica.DataSource = resultCaracteristica(0).Table
                'End If
                'Me.grdDetalleCaracteristica.DataBind()

                'Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponente")
                Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
                'lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()


                'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
                'Me.grdDetalleCaracteristica.DataBind()

                'LimpiarObjetosCaracteristicas()
                'txtBuscarCaracteristicaCatalogoComponente.Text = ""
                cboCaracteristicaEquipoComponente.SelectedIndex = -1
                'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
                grdDetalleCaracteristicaEquipoComponente.SelectedIndex = -1
                'grdListaCaracteristica.SelectedIndex = -1
                'lblMensajeCaracteristica.Text = "Caracteristica agregada con éxito."
                'lblMensajeCatalogoComponente.Text = "Caracteristica agregada con éxito."
                MyValidator.ErrorMessage = "Caracteristica agregada con éxito."

            Else
                lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
            End If
            '    End If
            'End If
            'End If

            'If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
            '            If IsNothing(grdListaDetalle.SelectedRow) = False Then
            '                If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then
            '                    Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(5).Text).Trim & "'")
            '                    rowIndexDetalle = Session("CestaCatalogo").Rows.IndexOf(result(0))
            '                    LlenarDataDetalle()
            '                    BloquearMantenimiento(False, True, False, True)
            '                    ValidarTexto(True)
            '                    ActivarObjetos(True)
            '                    lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            '                End If
            '            Else
            '                Throw New Exception("Debe de seleccionar algún item.")
            '            End If
            '        End If
            '    End If
            ValidationSummary2.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarEquipoComponente"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarEquipoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarEquipoComponente"
            Me.Page.Validators.Add(MyValidator)
            'BloquearPagina(2)
        End Try
    End Sub

    Sub ListarDistritoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDistritoMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboDistritoMensajeCliente.DataValueField = "cIdDistritoUbicacionGeografica"
        cboDistritoMensajeCliente.DataSource = UbicacionGeograficaNeg.DistritoListarCombo(cboPaisMensajeCliente.SelectedValue, cboDepartamentoMensajeCliente.SelectedValue, cboProvinciaMensajeCliente.SelectedValue)
        cboDistritoMensajeCliente.Items.Clear()
        cboDistritoMensajeCliente.DataBind()
    End Sub

    Sub ListarProvinciaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboProvinciaMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboProvinciaMensajeCliente.DataValueField = "cIdProvinciaUbicacionGeografica"
        cboProvinciaMensajeCliente.DataSource = UbicacionGeograficaNeg.ProvinciaListarCombo(cboPaisMensajeCliente.SelectedValue, cboDepartamentoMensajeCliente.SelectedValue)
        cboProvinciaMensajeCliente.Items.Clear()
        cboProvinciaMensajeCliente.DataBind()
    End Sub

    Sub ListarPaisCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboPaisMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboPaisMensajeCliente.DataValueField = "cIdPaisUbicacionGeografica"
        cboPaisMensajeCliente.DataSource = UbicacionGeograficaNeg.PaisListarCombo
        cboPaisMensajeCliente.DataBind()
    End Sub

    Sub ListarDepartamentoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDepartamentoMensajeCliente.DataTextField = "vDescripcionUbicacionGeografica"
        cboDepartamentoMensajeCliente.DataValueField = "cIdDepartamentoUbicacionGeografica"
        cboDepartamentoMensajeCliente.DataSource = UbicacionGeograficaNeg.DepartamentoListarCombo(cboPaisMensajeCliente.SelectedValue)
        cboDepartamentoMensajeCliente.Items.Clear()
        cboDepartamentoMensajeCliente.DataBind()
    End Sub

    Protected Sub cboPaisMensajeCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPaisMensajeCliente.SelectedIndexChanged
        ListarDepartamentoCombo()
        ListarProvinciaCombo()
        ListarDistritoCombo()
        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboDepartamentoMensajeCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDepartamentoMensajeCliente.SelectedIndexChanged
        ListarProvinciaCombo()
        ListarDistritoCombo()
        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboProvinciaMensajeCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProvinciaMensajeCliente.SelectedIndexChanged
        ListarDistritoCombo()
        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnAdicionarCliente_Click(sender As Object, e As EventArgs) Handles btnAdicionarCliente.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            MyValidator.ErrorMessage = ""
            hfdIdTipoPersonaCliente.Value = ""
            hfdIdTipoCliente.Value = ""
            hfdIdUbicacionGeograficaCliente.Value = ""
            hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
            hfdIdTipoDocumentoCliente.Value = ""
            txtRazonSocial.Text = ""
            hfdNroDocumentoCliente.Value = ""
            cboClienteUbicacion.SelectedIndex = -1
            'cboDireccionEntrega.SelectedIndex = -1
            hfdDireccionFiscalCliente.Value = ""
            hfdTelefonoContactoCliente.Value = ""
            'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = ""
            hfdCorreoElectronicoCliente.Value = ""
            txtTelefonoMensajeCliente.Text = ""
            lblTituloMensajeCliente.Visible = True

            Dim CliNeg As New clsClienteNegocios
            Dim ClienteRegistrado As New List(Of VI_GNRL_CLIENTE)
            If InStr(txtIdCliente.Text.Trim, "C") = 3 Then
                ClienteRegistrado = CliNeg.ClienteListaBusqueda("cIdCliente <> '' AND cIdCliente", IIf(txtIdCliente.Text.Trim = "", "NINGUNO", txtIdCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            ElseIf txtIdCliente.Text.Trim.Length = 8 Or txtIdCliente.Text.Trim.Length <> 11 Then 'DNI o CARNET EXTRANJERIA 
                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vDniCliente <> '' AND vDniCliente", IIf(txtIdCliente.Text.Trim = "", "NINGUNO", txtIdCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            Else
                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vRucCliente <> '' AND vRucCliente", IIf(txtIdCliente.Text.Trim = "", "NINGUNO", txtIdCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            End If

            'Crea la referencia al web service de BIMSIC
            Dim booValida As Boolean
            If ClienteRegistrado.Count > 0 Then
                booValida = False
            Else
                booValida = FuncionesNeg.VerificarConexionURL("https://www.bimsic.net/WSSIW/wsSistemaIntegradoWebBIMSIC.asmx")
            End If

            Dim WSCliente As New BIMSICWS.wsSistemaIntegradoWebBIMSICSoapClient
            Dim dsCliente As New DataSet
            If booValida = False Then 'No hay conexión con el WebServices
                If ClienteRegistrado.Count > 0 Then 'Existe en el sistema
                    Dim ClienteRegistradoFinal As GNRL_CLIENTE = CliNeg.ClienteListarPorId(ClienteRegistrado(0).Codigo, Session("IdEmpresa"), Session("IdPuntoVenta"))
                    IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente)
                    hfdIdAuxiliar.Value = ClienteRegistradoFinal.cIdCliente
                    hfdDNICliente.Value = ClienteRegistradoFinal.vDniCliente
                    hfdRUCCliente.Value = ClienteRegistradoFinal.vRucCliente
                    hfdNroDocumentoCliente.Value = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente) 'IIf(cboTipoDoc.SelectedValue = "FA", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                    txtRazonSocial.Text = ClienteRegistradoFinal.vRazonSocialCliente
                    hfdDireccionFiscalCliente.Value = ClienteRegistradoFinal.vDireccionCliente
                    hfdTelefonoContactoCliente.Value = ClienteRegistradoFinal.vTelefonoCliente
                    'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = hfdTelefonoContactoCliente.Value
                    hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                    hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona
                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    'ListarDireccionEntregaCombo()
                    'cboDireccionEntrega.SelectedValue = "00"

                    cboClienteUbicacion.SelectedValue = "00"
                    hfdIdTipoDocumentoCliente.Value = ClienteRegistradoFinal.cIdTipoDocumento
                    hfdCorreoElectronicoCliente.Value = ClienteRegistradoFinal.vEmailCliente
                    cboPaisMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdPaisUbicacionGeografica
                    cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                    cboDepartamentoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
                    cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                    cboProvinciaMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
                    cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                    cboDistritoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Hide()
                Else
                    If txtIdCliente.Text.Trim = "" Then
                        lnk_mostrarPanelCliente_ModalPopupExtender.Show()
                    Else
                        MyValidator.ErrorMessage = "Debe de ingresar el cliente desde la opción de mantenimiento."
                        ValidationSummary1.ValidationGroup = "vgrpValidar"
                        MyValidator.IsValid = False
                        MyValidator.ID = "ErrorPersonalizado"
                        MyValidator.ValidationGroup = "vgrpValidar"
                        Me.Page.Validators.Add(MyValidator)
                    End If
                End If
            Else
                If txtIdCliente.Text.Trim = "" Then
                    lnk_mostrarPanelCliente_ModalPopupExtender.Show()
                    Exit Sub
                ElseIf txtIdCliente.Text.Trim.Length = 8 Then 'cboTipoDoc.SelectedValue = "BV" And txtIdCliente.Text.Trim.Length = 8 Then
                    'Dim ClienteReniecNeg As New clsLibReniecNegocios
                    'Dim Cliente As New clsLibReniecMetodos.stReniecDNI
                    'Cliente = ClienteReniecNeg.ReniecLoadConsultaDNI(txtIdCliente.Text.Trim)

                    'Dim LocalNeg As New clsLocalNegocios
                    'Dim Local As GNRL_LOCAL = LocalNeg.LocalListarPorId(Session("IdLocal"), Session("IdEmpresa"))

                    'Dim dtCliente = ClienteNeg.ClienteGetData("SELECT 'ACTIVO' AS vEstadoContribuyentePadronReducido, 'HABIDO' AS vCondicionDomicilioPadronReducido, " &
                    '                                          "'" & Cliente.Dni & "' AS vDNIPadronReducido, vDescripcionUbicacionGeografica, cIdPaisUbicacionGeografica, " &
                    '                                          "cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica, " &
                    '                                          "SUBSTRING (vIdEquivalenciaUbicacionGeografica, 3, 6) AS vUbiGeoPadronReducido, '" & Cliente.ApellidoPaterno & " " & Cliente.ApellidoMaterno & ", " & Cliente.Nombres & "' AS vRazonSocialPadronReducido, " &
                    '                                          "'" & Local.vDireccionLocal.Trim & "' AS vDireccionFiscal, '' AS vRUCPadronReducido " &
                    '                                          "FROM GNRL_UBICACIONGEOGRAFICA WHERE vIdEquivalenciaUbicacionGeografica = '" & Local.vIdEquivalenciaUbicacionGeografica & "' AND LEN(RTRIM(LTRIM(vIdEquivalenciaUbicacionGeografica))) = 8")
                    'dsCliente.Tables.Add(dtCliente)
                ElseIf txtIdCliente.Text.Trim.Length = 11 Then 'cboTipoDoc.SelectedValue = "FA" And txtIdCliente.Text.Trim.Length = 11 Then
                    dsCliente = WSCliente.ConsultaRUC(txtIdCliente.Text)
                End If
                lblDatoInformativoMensajeCliente.Text = ""
                lblNroClienteMensajeCliente.Text = ""
                If dsCliente.Tables(0).Rows.Count > 0 Then
                    Dim fila As DataRow
                    For Each fila In dsCliente.Tables(0).Rows
                        If fila("vEstadoContribuyentePadronReducido") = "ACTIVO" And fila("vCondicionDomicilioPadronReducido") = "HABIDO" Then
                            'If cboTipoDoc.SelectedValue = "FA" Then
                            If fila("cIdTipoDocumento") = "04" Then
                                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vRucCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                            Else
                                ClienteRegistrado = CliNeg.ClienteListaBusqueda("vDniCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                            End If
                            If ClienteRegistrado.Count > 0 Then 'Existe en el sistema
                                Dim ClienteRegistradoFinal As GNRL_CLIENTE = CliNeg.ClienteListarPorId(ClienteRegistrado(0).Codigo, Session("IdEmpresa"), Session("IdPuntoVenta"))
                                If ClienteRegistradoFinal.vEstadoCliente = "ACTIVO" And ClienteRegistradoFinal.vCondicionCliente = "HABIDO" Then
                                    hfdIdAuxiliar.Value = ClienteRegistradoFinal.cIdCliente
                                    txtIdCliente.Text = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente)
                                    hfdDNICliente.Value = ClienteRegistradoFinal.vDniCliente
                                    hfdRUCCliente.Value = ClienteRegistradoFinal.vRucCliente
                                    hfdNroDocumentoCliente.Value = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente) 'IIf(cboTipoDoc.SelectedValue = "FA", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                                    txtRazonSocial.Text = ClienteRegistradoFinal.vRazonSocialCliente
                                    hfdDireccionFiscalCliente.Value = ClienteRegistradoFinal.vDireccionCliente
                                    hfdTelefonoContactoCliente.Value = ClienteRegistradoFinal.vTelefonoCliente
                                    'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = hfdTelefonoContactoCliente.Value
                                    hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                                    hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona

                                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                                    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                                    'ListarDireccionEntregaCombo()
                                    'cboDireccionEntrega.SelectedValue = "00"
                                    cboClienteUbicacion.SelectedValue = "00"
                                    hfdIdTipoDocumentoCliente.Value = ClienteRegistradoFinal.cIdTipoDocumento
                                    hfdCorreoElectronicoCliente.Value = ClienteRegistradoFinal.vEmailCliente

                                    cboPaisMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdPaisUbicacionGeografica
                                    cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                                    cboDepartamentoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
                                    cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                                    cboProvinciaMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
                                    cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                                    cboDistritoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
                                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Hide()
                                Else 'Si no está ACTIVO
                                    hfdIdAuxiliar.Value = ""
                                    hfdIdTipoPersonaCliente.Value = ""
                                    hfdIdTipoCliente.Value = ""
                                    hfdIdUbicacionGeograficaCliente.Value = ""
                                    hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
                                    hfdIdTipoDocumentoCliente.Value = ""
                                    txtIdCliente.Text = ""
                                    txtRazonSocial.Text = ""
                                    hfdDNICliente.Value = ""
                                    hfdRUCCliente.Value = ""
                                    hfdNroDocumentoCliente.Value = ""
                                    'cboDireccionEntrega.SelectedIndex = -1
                                    cboClienteUbicacion.SelectedIndex = -1
                                    hfdDireccionFiscalCliente.Value = ""
                                    hfdTelefonoContactoCliente.Value = ""
                                    'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = ""
                                    hfdCorreoElectronicoCliente.Value = ""
                                    lblNroClienteMensajeCliente.Text = "No puede emitir comprobantes"
                                    lblDatoInformativoMensajeCliente.Text = fila("vEstadoContribuyentePadronReducido") & "/" & fila("vCondicionDomicilioPadronReducido")
                                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                                End If
                            Else 'Esto es si no existe en el sistema pero esta como activo
                                Dim Cliente As New GNRL_CLIENTE
                                Cliente.bEstadoAceptanteCliente = False
                                Cliente.bEstadoRegistroCliente = True
                                Cliente.cGeneroCliente = "M"
                                Cliente.cIdCliente = ""
                                Cliente.cIdPaisUbicacionGeografica = "000"
                                Cliente.cIdDepartamentoUbicacionGeografica = Mid(fila("vUbiGeoPadronReducido"), 1, 2)
                                Cliente.cIdProvinciaUbicacionGeografica = Mid(fila("vUbiGeoPadronReducido"), 3, 2)
                                Cliente.cIdDistritoUbicacionGeografica = Mid(fila("vUbiGeoPadronReducido"), 5, 2)
                                Cliente.cIdEmpresa = Session("IdEmpresa")
                                Cliente.cIdEstadoCivil = "S"
                                Cliente.cIdPuntoVenta = Session("IdPuntoVenta")
                                Cliente.cIdTipoCliente = "01" 'CLIENTE FINAL
                                'Cliente.cIdTipoDocumento = IIf(cboTipoDoc.SelectedValue = "BV" And txtIdCliente.Text.Trim.Length = 8, "01", "04")
                                Cliente.cIdTipoDocumento = IIf(txtIdCliente.Text.Trim.Length = 8, "01", "04")
                                'Cliente.cIdTipoPersona = CChar(IIf(cboTipoDoc.SelectedValue = "BV" And txtIdCliente.Text.Trim.Length = 8, "N", "J"))
                                Cliente.cIdTipoPersona = CChar(IIf(txtIdCliente.Text.Trim.Length = 8, "N", "J"))
                                Cliente.dFechaNacimientoCliente = Nothing
                                'If cboTipoDoc.SelectedValue = "FA" Then
                                If fila("cIdTipoDocumento") = "04" Then
                                    Cliente.vApellidoMaternoCliente = ""
                                    Cliente.vApellidoPaternoCliente = ""
                                    Cliente.vNombresCliente = ""
                                Else
                                    Dim strDatos() As String = fila("vRazonSocialPadronReducido").Split(",")
                                    Dim strDatos2() As String = strDatos(0).Trim.Split(" ")
                                    Cliente.vApellidoPaternoCliente = strDatos2(0).Trim
                                    Cliente.vApellidoMaternoCliente = strDatos2(1).Trim
                                    Cliente.vNombresCliente = strDatos(1).Trim
                                End If

                                Cliente.vCelularCliente = ""
                                'Cliente.vDireccionCliente = IIf(cboTipoDoc.SelectedValue = "BV" And txtIdCliente.Text.Trim.Length = 8, "", Trim(fila("vDireccionFiscal")))
                                Cliente.vDireccionCliente = IIf(txtIdCliente.Text.Trim.Length = 8, "", Trim(fila("vDireccionFiscal")))
                                'If cboTipoDoc.SelectedValue = "BV" Then
                                If fila("cIdTipoDocumento") <> "04" Then
                                    Cliente.vDniCliente = Trim(fila("vDNIPadronReducido"))
                                Else
                                    Cliente.vDniCliente = ""
                                End If
                                Cliente.vEmailCliente = ""
                                Cliente.vFaxCliente = ""
                                Cliente.vRazonSocialCliente = Trim(Mid(fila("vRazonSocialPadronReducido"), 1, IIf(InStrRev(fila("vRazonSocialPadronReducido"), "-") = 0, Len(fila("vRazonSocialPadronReducido")), InStrRev(fila("vRazonSocialPadronReducido"), "-") - 1)))
                                Cliente.vRepresentanteLegalCliente = ""
                                'Cliente.vRucCliente = IIf(cboTipoDoc.SelectedValue = "FA", fila("vRUCPadronReducido"), "")
                                Cliente.vRucCliente = IIf(fila("cIdTipoDocumento") = "04", fila("vRUCPadronReducido"), "")
                                Cliente.vTelefonoCliente = ""
                                Cliente.vEstadoCliente = Trim(fila("vEstadoContribuyentePadronReducido"))
                                Cliente.vCondicionCliente = Trim(fila("vCondicionDomicilioPadronReducido"))

                                hfdIdAuxiliar.Value = ""
                                hfdIdTipoPersonaCliente.Value = Cliente.cIdTipoPersona '"J"
                                hfdIdTipoCliente.Value = Cliente.cIdTipoCliente '"01"
                                hfdIdUbicacionGeograficaCliente.Value = "PE" & fila("vUbiGeoPadronReducido") 'El PE recién lo coloqué 20/06/2019
                                hfdIdUbicacionGeograficaClienteUbicacion.Value = "PE" & fila("vUbiGeoPadronReducido") 'El PE recién lo coloqué 20/06/2019
                                txtRazonSocial.Text = Cliente.vRazonSocialCliente
                                hfdDNICliente.Value = Cliente.vDniCliente
                                hfdRUCCliente.Value = Cliente.vRucCliente
                                'hfdNroDocumentoCliente.Value = IIf(cboTipoDoc.SelectedValue = "FA", hfdRUCCliente.Value, hfdDNICliente.Value)
                                hfdNroDocumentoCliente.Value = IIf(fila("cIdTipoDocumento") = "04", hfdRUCCliente.Value, hfdDNICliente.Value)
                                'cboDireccionEntrega.SelectedIndex = -1
                                cboClienteUbicacion.SelectedIndex = -1
                                hfdDireccionFiscalCliente.Value = fila("vDireccionFiscal")
                                hfdTelefonoContactoCliente.Value = ""
                                'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = ""
                                hfdIdTipoDocumentoCliente.Value = Cliente.cIdTipoDocumento
                                If CliNeg.ClienteInserta(Cliente) = 0 Then
                                    hfdIdAuxiliar.Value = Cliente.cIdCliente
                                    lblNroClienteMensajeCliente.Text = Cliente.cIdCliente

                                    cboPaisMensajeCliente.SelectedValue = Cliente.cIdPaisUbicacionGeografica
                                    cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                                    cboDepartamentoMensajeCliente.SelectedValue = Cliente.cIdDepartamentoUbicacionGeografica
                                    cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                                    cboProvinciaMensajeCliente.SelectedValue = Cliente.cIdProvinciaUbicacionGeografica
                                    cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                                    cboDistritoMensajeCliente.SelectedValue = Cliente.cIdDistritoUbicacionGeografica

                                    lblDatoInformativoMensajeCliente.Text = "Cliente Registrado"
                                    txtDireccionAdicionalMensajeCliente.Text = hfdDireccionFiscalCliente.Value
                                    Dim TablaSistemaNeg As New clsTablaSistemaNegocios 'NUEVO JMUG: 02/03/2023
                                    hfdCorreoElectronicoCliente.Value = TablaSistemaNeg.TablaSistemaListarPorId("45", "15", Session("IdSistema"), Session("IdEmpresa"), "*").vValorOpcionalTablaSistema 'JMUG: 15/04/2022
                                    txtCorreoAdicionalMensajeCliente.Text = hfdCorreoElectronicoCliente.Value 'JMUG: 15/04/2022
                                    lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                                End If
                            End If
                        Else 'Si no está ACTIVO
                            hfdIdTipoPersonaCliente.Value = ""
                            hfdIdTipoCliente.Value = ""
                            hfdIdUbicacionGeograficaCliente.Value = ""
                            hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
                            hfdIdTipoDocumentoCliente.Value = ""
                            hfdIdAuxiliar.Value = ""
                            txtIdCliente.Text = ""
                            txtRazonSocial.Text = ""
                            hfdDNICliente.Value = ""
                            hfdRUCCliente.Value = ""
                            hfdNroDocumentoCliente.Value = ""
                            'cboDireccionEntrega.SelectedIndex = -1
                            cboClienteUbicacion.SelectedIndex = -1
                            hfdDireccionFiscalCliente.Value = ""
                            hfdTelefonoContactoCliente.Value = ""
                            'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = ""
                            hfdCorreoElectronicoCliente.Value = ""
                            lblNroClienteMensajeCliente.Text = "No puede emitir comprobantes"
                            lblDatoInformativoMensajeCliente.Text = fila("vEstadoContribuyentePadronReducido") & "/" & fila("vCondicionDomicilioPadronReducido")
                            lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                        End If
                    Next
                Else 'No existe - Porque ese RUC no está registrado en la SUNAT
                    'If cboTipoDoc.SelectedValue = "FA" Then
                    If txtIdCliente.Text.Trim.Length = 11 Then
                        ClienteRegistrado = CliNeg.ClienteListaBusqueda("vRucCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                    Else
                        ClienteRegistrado = CliNeg.ClienteListaBusqueda("vDniCliente", txtIdCliente.Text, Session("IdEmpresa"), "*", False, "1")
                    End If

                    If ClienteRegistrado.Count > 0 Then 'Existe en el sistema
                        Dim ClienteRegistradoFinal As GNRL_CLIENTE = CliNeg.ClienteListarPorId(ClienteRegistrado(0).Codigo, Session("IdEmpresa"), Session("IdPuntoVenta"))
                        hfdIdAuxiliar.Value = ClienteRegistradoFinal.cIdCliente
                        txtIdCliente.Text = IIf(ClienteRegistradoFinal.cIdTipoDocumento <> "04", ClienteRegistradoFinal.vDniCliente, ClienteRegistradoFinal.vRucCliente)
                        hfdDNICliente.Value = ClienteRegistradoFinal.vDniCliente
                        hfdRUCCliente.Value = ClienteRegistradoFinal.vRucCliente
                        'hfdNroDocumentoCliente.Value = IIf(cboTipoDoc.SelectedValue = "FA", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                        hfdNroDocumentoCliente.Value = IIf(ClienteRegistrado(0).Tipo_Doc = "04", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                        txtRazonSocial.Text = ClienteRegistradoFinal.vRazonSocialCliente
                        hfdDireccionFiscalCliente.Value = ClienteRegistradoFinal.vDireccionCliente
                        hfdTelefonoContactoCliente.Value = ClienteRegistradoFinal.vTelefonoCliente
                        'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = hfdTelefonoContactoCliente.Value
                        hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                        hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona

                        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                        Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                        UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                        hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                        hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                        'ListarDireccionEntregaCombo()
                        'cboDireccionEntrega.SelectedValue = "00"
                        cboClienteUbicacion.SelectedValue = "00"
                        hfdIdTipoDocumentoCliente.Value = ClienteRegistradoFinal.cIdTipoDocumento

                        cboPaisMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdPaisUbicacionGeografica
                        cboPaisMensajeCliente_SelectedIndexChanged(cboPaisMensajeCliente, New System.EventArgs())
                        cboDepartamentoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
                        cboDepartamentoMensajeCliente_SelectedIndexChanged(cboDepartamentoMensajeCliente, New System.EventArgs())
                        cboProvinciaMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
                        cboProvinciaMensajeCliente_SelectedIndexChanged(cboProvinciaMensajeCliente, New System.EventArgs())
                        cboDistritoMensajeCliente.SelectedValue = ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
                        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Hide()
                    Else
                        lblTituloMensajeCliente.Visible = False
                        hfdIdAuxiliar.Value = ""
                        txtIdCliente.Text = ""
                        hfdDNICliente.Value = ""
                        hfdRUCCliente.Value = ""
                        hfdNroDocumentoCliente.Value = ""
                        txtRazonSocial.Text = ""
                        hfdDireccionFiscalCliente.Value = ""
                        hfdTelefonoContactoCliente.Value = ""
                        'JMUG: 05/03/2023 txtTelefonoContactoCliente.Text = ""
                        'cboDireccionEntrega.SelectedIndex = -1
                        cboClienteUbicacion.SelectedIndex = -1
                        hfdIdTipoCliente.Value = ""
                        hfdIdTipoPersonaCliente.Value = ""
                        hfdIdUbicacionGeograficaCliente.Value = ""
                        'fdIdUbicacionGeograficaEntregaCliente.Value = ""
                        hfdIdTipoDocumentoCliente.Value = ""
                        lblNroClienteMensajeCliente.Text = "Este cliente no se encuentra registrado, por favor registrelo en el Mantenimiento de Clientes"
                        lblDatoInformativoMensajeCliente.Text = "Cliente No Registrado"
                        lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                    End If
                End If
            End If
        Catch ex As Exception
            MyValidator.ErrorMessage = "No se estableció conexión con la SUNAT. Utilizará los datos del sistema."
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnAgregarCatalogoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnAgregarCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0674", strOpcionModulo, "CMMS")

            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If

            If IsNothing(grdCatalogoComponente.SelectedRow) = False Then
                'clsLogiCestaEquipo.AgregarCesta()

                'clsLogiCestaEquipo.AgregarCesta(txtIdEquipo.Text.Trim, Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text), Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(3).Text),
                '                               "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(4).Text),
                '                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text), Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text), UCase(txtDescripcionAbreviadaDetalleMaestroActivo.Text.Trim),
                '                               IIf(txtFechaAdquisicionDetalleMaestroActivo.Text.Trim = "", "01/01/1900", txtFechaAdquisicionDetalleMaestroActivo.Text.Trim), True, "", UCase(txtObservacionAbreviadaDetalleMaestroActivo.Text.Trim), IIf(txtCostoInicialDetalleMaestroActivo.Text.Trim = "", "0.00", txtCostoInicialDetalleMaestroActivo.Text), txtCuentaContableDetalleMaestroActivo.Text,
                '                               "1",
                '                               IIf(txtFechaActivacionDetalleMaestroActivo.Text.Trim = "", "01/01/1900", txtFechaActivacionDetalleMaestroActivo.Text.Trim),
                '                               chkLeasingDetalleMaestroActivo.Checked,
                '                               UCase(txtMarcaDetalleMaestroActivo.Text.Trim),
                '                               UCase(txtCaracteristicasDetalleMaestroActivo.Text.Trim),
                '                               IIf(txtVidaUtilDetalleMaestroActivo.Text.Trim = "", "0", txtVidaUtilDetalleMaestroActivo.Text),
                '                               txtDimensionesDetalleMaestroActivo.Text,
                '                               txtVoltajeDetalleMaestroActivo.Text,
                '                               txtPesoDetalleMaestroActivo.Text,
                '                               txtIdInternoDetalleMaestroActivo.Text, cboMetodoDepreciacionDetalleMaestroActivo.SelectedValue, cboFamiliaDetalleMaestroActivo.SelectedValue, cboEstadoActivoFijoDetalleMaestroActivo.SelectedValue,
                '                               IIf(txtFechaFinalizacionDetalleMaestroActivo.Text.Trim = "", "01/01/1900", txtFechaFinalizacionDetalleMaestroActivo.Text.Trim), cboTipoMonedaDetalleMaestroActivo.SelectedValue, IIf(txtSaldoInicialDetalleMaestroActivo.Text.Trim = "", "0.00", txtSaldoInicialDetalleMaestroActivo.Text),
                '                               cboEstadoOperacionDetalleMaestroActivo.SelectedValue, cboEstadoActivoDetalleMaestroActivo.SelectedValue, txtModeloDetalleMaestroActivo.Text, txtNroSerieDetalleMaestroActivo.Text, txtPlacaDetalleMaestroActivo.Text,
                '                               IIf(txtNroUsuarioDetalleMaestroActivo.Text.Trim = "", "0", txtNroUsuarioDetalleMaestroActivo.Text), IIf(txtNroLectoraDetalleMaestroActivo.Text.Trim = "", "0", txtNroLectoraDetalleMaestroActivo.Text), IIf(txtNroContadoresDetalleMaestroActivo.Text.Trim = "", "0", txtNroContadoresDetalleMaestroActivo.Text),
                '                               txtNroContratoArrendamientoDetalleMaestroActivo.Text, IIf(txtFechaContratoArrendamientoDetalleMaestroActivo.Text.Trim = "", "01/01/1900", txtFechaContratoArrendamientoDetalleMaestroActivo.Text.Trim),
                '                               IIf(txtNroCuotasArrendamientoDetalleMaestroActivo.Text.Trim = "", "0", txtNroCuotasArrendamientoDetalleMaestroActivo.Text), hfdIdAlmacenDetalleMaestroActivo.Value, hfdIdTipoAlmacenDetalleMaestroActivo.Value,
                '                               Session("CestaEquipoComponente"))
                clsLogiCestaEquipo.AgregarCesta("", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text),
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(3).Text),
                                               "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(4).Text),
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim,
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text),
                                               Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(5).Text), Now,
                                               "1", txtIdEquipo.Text.Trim, "", "1", "", "", "1", "1",
                                               "", "", "01",
                                               Session("CestaEquipoComponente"))

                clsLogiCestaCatalogo.QuitarCesta(grdCatalogoComponente.SelectedIndex, Session("CestaCatalogoComponente"))
                MyValidator.ErrorMessage = "Componente asignado satisfactoriamente."
            End If

            'Dim result As DataRow() = Session("CestaEquipoComponente").Select("IdCatalogo ='" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text) & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text).Trim & "'")
            Dim result As DataRow() = Session("CestaEquipoComponente").Select("IdCatalogo ='" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text) & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(3).Text).Trim & "'")
            rowIndexDetalle = Session("CestaEquipoComponente").Rows.IndexOf(result(0))
            CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

            grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            grdCatalogoComponente.DataBind()

            grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            grdEquipoComponente.DataBind()

            'hfdOperacionDetalle.Value = "E"
            'If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
            '    'If IsNothing(grdListaCaracteristica.SelectedRow) = False Then
            '    '    If lblMensajeCaracteristica.Text = "" Then
            '    For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
            '        'If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (hfdIdCaracteristicaCatalogoComponente.Value.ToString.Trim) And Session("CestaCatalogoCaracteristica").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogoComponente.Text Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
            '        If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaCatalogoComponente.SelectedValue.ToString.Trim) And Session("CestaCatalogoCaracteristica").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogoComponente.Text Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
            '            'Dim resultCaracteristicaSimple As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
            '            'If resultCaracteristicaSimple.Length = 0 Then
            '            '    Me.grdDetalleCaracteristica.DataSource = Nothing
            '            'Else
            '            '    Dim rowFil As DataRow() = resultCaracteristicaSimple
            '            '    For Each fila As DataRow In rowFil
            '            '        clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCatalogoCaracteristicaFiltrado"))
            '            '    Next
            '            'End If
            '            'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristicaFiltrado")
            '            'Me.grdDetalleCaracteristica.DataBind()
            '            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()

            '            Throw New Exception("Característica ya registrada, seleccione otro item.")
            '            Exit Sub
            '        End If
            '    Next
            '    'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", hfdIdCaracteristicaCatalogoComponente.Value.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristica"))
            '    clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", cboCaracteristicaCatalogoComponente.SelectedValue.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristica"))
            '    'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", hfdIdCaracteristicaCatalogoComponente.Value.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristicaFiltrado"))
            '    'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogoComponente.Text, "1", cboCaracteristicaCatalogoComponente.SelectedValue.Trim, UCase(cboCaracteristicaCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCatalogoCaracteristicaFiltrado"))

            '    ''Dim rowIndexDetalleCaracteristica As Int64
            '    ''Dim resultCaracteristica As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
            '    'Session("CestaCatalogoCaracteristica").row
            '    ''rowIndexDetalleCaracteristica = Session("CestaCatalogoCaracteristica").Rows.IndexOf(resultCaracteristica(0))
            '    'If resultCaracteristica.Length = 0 Then
            '    '    Me.grdDetalleCaracteristica.DataSource = Nothing
            '    'Else
            '    '    Me.grdDetalleCaracteristica.DataSource = resultCaracteristica(0).Table
            '    'End If
            '    'Me.grdDetalleCaracteristica.DataBind()

            '    Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            '    Me.grdDetalleCaracteristica.DataBind()
            '    'lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()


            '    'Me.grdDetalleCaracteristica.DataSource = Session("CestaCatalogoCaracteristica")
            '    'Me.grdDetalleCaracteristica.DataBind()

            '    'LimpiarObjetosCaracteristicas()
            '    'txtBuscarCaracteristicaCatalogoComponente.Text = ""
            '    cboCaracteristicaCatalogoComponente.SelectedIndex = -1
            '    'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
            '    lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            '    grdDetalleCaracteristica.SelectedIndex = -1
            '    'grdListaCaracteristica.SelectedIndex = -1
            '    'lblMensajeCaracteristica.Text = "Caracteristica agregada con éxito."
            '    'lblMensajeCatalogoComponente.Text = "Caracteristica agregada con éxito."
            '    MyValidator.ErrorMessage = "Caracteristica agregada con éxito."

            'Else
            '    lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            'End If
            '    End If
            'End If
            'End If

            'If grdListaDetalle.SelectedIndex < grdListaDetalle.Rows.Count Then
            '            If IsNothing(grdListaDetalle.SelectedRow) = False Then
            '                If IsReference(grdListaDetalle.SelectedRow.Cells(1).Text) = True Then
            '                    Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(5).Text).Trim & "'")
            '                    rowIndexDetalle = Session("CestaCatalogo").Rows.IndexOf(result(0))
            '                    LlenarDataDetalle()
            '                    BloquearMantenimiento(False, True, False, True)
            '                    ValidarTexto(True)
            '                    ActivarObjetos(True)
            '                    lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            '                End If
            '            Else
            '                Throw New Exception("Debe de seleccionar algún item.")
            '            End If
            '        End If
            '    End If


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
            'BloquearPagina(2)
        End Try
    End Sub

    Private Sub lnkEsComponenteOn_Click(sender As Object, e As EventArgs) Handles lnkEsComponenteOn.Click
        If lnkEsComponenteOn.Visible = True Then
            lnkEsComponenteOn.Visible = False
            lnkEsComponenteOff.Visible = True
            divSistemaFuncional.Visible = False
        Else
            lnkEsComponenteOn.Visible = True
            lnkEsComponenteOff.Visible = False
            divSistemaFuncional.Visible = True
        End If
    End Sub

    Private Sub lnkEsComponenteOff_Click(sender As Object, e As EventArgs) Handles lnkEsComponenteOff.Click
        If lnkEsComponenteOff.Visible = True Then
            lnkEsComponenteOn.Visible = True
            lnkEsComponenteOff.Visible = False
            divSistemaFuncional.Visible = True
        Else
            lnkEsComponenteOn.Visible = False
            lnkEsComponenteOff.Visible = True
            divSistemaFuncional.Visible = False
        End If
    End Sub

    Private Sub lnkEsConContratoOn_Click(sender As Object, e As EventArgs) Handles lnkEsConContratoOn.Click
        If lnkEsConContratoOn.Visible = True Then
            lnkEsConContratoOn.Visible = False
            lnkEsConContratoOff.Visible = True
            divContratoReferencia.Visible = False
        Else
            lnkEsConContratoOn.Visible = True
            lnkEsConContratoOff.Visible = False
            divContratoReferencia.Visible = True
        End If
    End Sub

    Private Sub lnkEsConContratoOff_Click(sender As Object, e As EventArgs) Handles lnkEsConContratoOff.Click
        If lnkEsConContratoOff.Visible = True Then
            lnkEsConContratoOn.Visible = True
            lnkEsConContratoOff.Visible = False
            divContratoReferencia.Visible = True
        Else
            lnkEsConContratoOn.Visible = False
            lnkEsConContratoOff.Visible = True
            divContratoReferencia.Visible = False
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Codigo
            'e.Row.Cells(1).Visible = True 'RucCliente
            'e.Row.Cells(2).Visible = True 'RazonSocialCliente
            e.Row.Cells(1).Visible = True 'RucCliente/RazonSocialCliente
            e.Row.Cells(2).Visible = True 'FechaRegistroTarjetaSAP
            e.Row.Cells(3).Visible = True 'FechaUltimaModificacion
            e.Row.Cells(4).Visible = False 'IdTipoActivo
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(7).Visible = True 'Descripcion
            e.Row.Cells(8).Visible = True 'Descripcion SAP
            e.Row.Cells(9).Visible = True 'Estado Equipo
            e.Row.Cells(10).Visible = False 'Estado Registro
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Codigo
            'e.Row.Cells(1).Visible = True 'RucCliente
            'e.Row.Cells(2).Visible = True 'RazonSocialCliente
            e.Row.Cells(1).Visible = True 'RucCliente/RazonSocialCliente
            e.Row.Cells(2).Visible = True 'FechaRegistroTarjetaSAP
            e.Row.Cells(3).Visible = True 'FechaUltimaModificacion
            e.Row.Cells(4).Visible = False 'IdTipoActivo
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(7).Visible = True 'Descripcion
            e.Row.Cells(8).Visible = True 'Descripcion SAP
            e.Row.Cells(9).Visible = True 'Estado Equipo
            e.Row.Cells(10).Visible = False 'Estado Registro
            'e.Row.Cells(10).Visible = True 'Estado SI/NO
        End If
        '        <asp:BoundField DataField = "" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:BoundField DataField = "" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:BoundField DataField = "" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:BoundField DataField = "" HeaderText="Fecha Registro SAP" DataFormatString="{0:dd/MM/yyyy}"  HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:BoundField DataField = "" HeaderText="Código Catálogo" HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:BoundField DataField = "" HeaderText="Número Serie" HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:BoundField DataField = "" HeaderText="Equipo" HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:BoundField DataField = "" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
        '<asp:TemplateField HeaderStyle - CssClass = "bg-200 text-900" >
        '    <ItemTemplate>
        '        <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
        '            <div class="bootstrap-switch-container">
        '                <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
        '                <span class="bootstrap-switch-label">&nbsp;</span>
        '                <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>


    End Sub

    Private Sub imgbtnBuscarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarEquipo.Click
        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")
        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
        Me.grdLista.DataBind()
    End Sub

    Private Sub grdDetalleCaracteristicaCatalogoComponente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaCatalogoComponente.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "355", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            'JMUG: 31/03/2023 OK
            'Dim fila As Integer = 0
            'For Each row As GridViewRow In grdDetalleCaracteristicaCatalogoComponente.Rows
            '    If row.RowType = DataControlRowType.DataRow Then
            '        Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleCaracteristica"), CheckBox)
            '        If chkRow.Checked Then
            '            clsLogiCestaCatalogoCaracteristica.QuitarCesta(e.RowIndex, Session("CestaCaracteristicaCatalogoComponente"))
            '            fila += 1
            '        End If
            '    End If
            'Next
            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogoComponente.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
                            If (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaCatalogoComponente"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponente")
            Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()
            lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            'TotalizarGrid()
            'GenerarCuotas(txtNroCuotas.Text)
            ''txtImporteFormaPagoOp1.Text = Math.Round(Convert.ToDecimal(txtTotalGeneral.Text), 2) '04/07/2021
            'txtImporteFormaPagoOp1.Text = Math.Round(Convert.ToDecimal(txtTotalGeneral.Text) - Convert.ToDecimal(txtImporteTotalAplicarPago.Text), 2) 'JMUG: 17/11/2022
            ''lblCuadroDialogoAsiento.Value = "0" 'Recién lo puse
            'BloquearMantenimiento(False, True, False, False)
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

    Private Sub lnkbtnEditarEquipoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEditarEquipoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'lblMensajeCatalogoComponente.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If

            hfdOperacionDetalle.Value = "E"
            If grdEquipoComponente.Rows.Count > 0 Then
                If grdEquipoComponente.SelectedIndex < grdEquipoComponente.Rows.Count Then
                    If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                        If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                            'Dim result As DataRow() = Session("CestaCatalogo").Select("IdCatalogo ='" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(5).Text).Trim & "'")
                            Dim result As DataRow() = Session("CestaEquipoComponente").Select("IdCatalogo ='" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text).Trim & "'")
                            rowIndexDetalle = Session("CestaEquipoComponente").Rows.IndexOf(result(0))
                            'LlenarDataDetalle()
                            BloquearMantenimiento(False, True, False, True)
                            ValidarTexto(True)
                            ActivarObjetos(True)
                            LlenarDataEquipoComponente()
                            'CargarCestaCatalogoComponente()
                            'CargarCestaEquipoComponente()
                            'CargarCestaCaracteristicaCatalogoComponente()
                            ''CargarCestaCaracteristica()
                            ''Dim rowIndexDetalleCaracteristica As Int64
                            'Dim resultCaracteristica As DataRow() = Session("CestaCatalogoCaracteristica").Select("IdCatalogo = '" & Server.HtmlDecode(grdListaDetalle.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                            ''rowIndexDetalleCaracteristica = Session("CestaCatalogoCaracteristica").Rows.IndexOf(resultCaracteristica(0))
                            'If resultCaracteristica.Length = 0 Then
                            '    Me.grdDetalleCaracteristica.DataSource = Nothing
                            'Else
                            '    Me.grdDetalleCaracteristica.DataSource = resultCaracteristica(0).Table
                            'End If
                            'Me.grdDetalleCaracteristica.DataBind()





                            'JMUG: 04/03/2023
                            'Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaCatalogoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim & "' AND IdJerarquia = '1'")
                            'If resultCaracteristicaSimple.Length = 0 Then
                            '    Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Nothing
                            'Else
                            '    Dim rowFil As DataRow() = resultCaracteristicaSimple
                            '    For Each fila As DataRow In rowFil
                            '        clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaCatalogoComponenteFiltrado"))
                            '    Next
                            'End If
                            'Me.grdDetalleCaracteristicaCatalogoComponente.DataSource = Session("CestaCaracteristicaCatalogoComponenteFiltrado")
                            'Me.grdDetalleCaracteristicaCatalogoComponente.DataBind()

                            'Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(2).Text).Trim & "' AND IdJerarquia = '1'")
                            'If resultCaracteristicaSimple.Length = 0 Then
                            '    Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Nothing
                            'Else
                            '    Dim rowFil As DataRow() = resultCaracteristicaSimple
                            '    For Each fila As DataRow In rowFil
                            '        clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), "", fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                            '    Next
                            'End If
                            Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                            Me.grdDetalleCaracteristicaEquipoComponente.DataBind()

                            lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar algún item.")
                    End If
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub


    'Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

    '        pnlCabecera.Visible = False
    '        pnlEquipo.Visible = True
    '        pnlComponentes.Visible = True

    '        hfdOperacion.Value = "R"
    '        'BloquearPagina(2)
    '        txtDescripcionEquipo.Focus()

    '        BloquearMantenimiento(False, True, False, True)
    '        LimpiarObjetos()
    '        'CargarCestaCatalogo()
    '        ValidarTexto(True)
    '        ActivarObjetos(True)
    '        'hfTab.Value = "tab2"
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "142", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0653", strOpcionModulo, "CMMS")

            If lnkEsConContratoOn.Visible = True And cboContratoReferencia.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un contrato de referencia.")
            End If

            Dim Equipo As New LOGI_EQUIPO
            Equipo.cIdEquipo = UCase(txtIdEquipo.Text)
            Equipo.cIdEmpresa = Session("IdEmpresa")
            Equipo.cIdTipoActivo = IIf(cboTipoActivo.SelectedValue = "SELECCIONE DATO", Nothing, cboTipoActivo.SelectedValue)
            Equipo.vDescripcionEquipo = UCase(txtDescripcionEquipo.Text)
            Equipo.vDescripcionAbreviadaEquipo = "" 'UCase(txtDescripcionAbreviada.Text)
            Equipo.dFechaTransaccionEquipo = Convert.ToDateTime(IIf(hfdFechaEquipo.Value = "", Now, hfdFechaEquipo.Value)) 'hfdFechaTransaccionEquipoComponente.Value
            Equipo.cIdEnlaceEquipo = ""
            Equipo.cIdEnlaceCatalogo = ""
            Equipo.bEstadoRegistroEquipo = True
            Equipo.cIdSistemaFuncionalEquipo = cboSistemaFuncional.SelectedValue  'lblSistemaFuncional.Value
            Equipo.cIdFamilia = Nothing
            Equipo.cIdSubFamilia = Nothing
            Equipo.vObservacionEquipo = "" 'UCase(txtObservacion.Text)
            Equipo.cIdEstadoComponenteEquipo = "01" '"cboEstadoActivoMaestroActivoPrincipal.SelectedValue
            Equipo.vIdEquivalenciaEquipo = "" 'txtIdInternoMaestroActivoPrincipal.Text
            Equipo.cIdPaisOrigenEquipo = Session("IdPais")
            Equipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
            Equipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
            Equipo.dFechaRegistroTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaRegistroTarjetaSAPEquipo.Value), hfdFechaRegistroTarjetaSAPEquipo.Value, Nothing))
            Equipo.dFechaRegistroTarjetaSAPEquipo = IIf(Equipo.dFechaRegistroTarjetaSAPEquipo = "01/01/0001", Nothing, Equipo.dFechaRegistroTarjetaSAPEquipo)
            Equipo.dFechaManufacturaTarjetaSAPEquipo = Convert.ToDateTime(IIf(IsDate(hfdFechaManufacturaTarjetaSAPEquipo.Value), hfdFechaManufacturaTarjetaSAPEquipo.Value, Nothing))
            Equipo.dFechaManufacturaTarjetaSAPEquipo = IIf(Equipo.dFechaManufacturaTarjetaSAPEquipo = "01/01/0001", Nothing, Equipo.dFechaManufacturaTarjetaSAPEquipo)
            Equipo.cIdCatalogo = IIf(cboCatalogo.SelectedValue = "SELECCIONE DATO", Nothing, cboCatalogo.SelectedValue) 'txtIdMaestroActivo.Text
            Equipo.cIdJerarquiaCatalogo = "0"
            Equipo.nVidaUtilEquipo = Convert.ToInt32(IIf(txtVidaUtilMantenimientoCatalogo.Text.Trim = "", "0", txtVidaUtilMantenimientoCatalogo.Text.Trim)) 'Convert.ToDecimal(IIf(txtVidaUtilMaestroActivoPrincipal.Text.Trim = "", 0, txtVidaUtilMaestroActivoPrincipal.Text))
            Equipo.nPeriodoGarantiaEquipo = Convert.ToInt32(IIf(txtPeriodoGarantiaMantenimientoCatalogo.Text.Trim = "", "0", txtPeriodoGarantiaMantenimientoCatalogo.Text.Trim)) 'txtPeriodoGarant
            Equipo.nPeriodoMinimoMantenimientoEquipo = Convert.ToInt32(IIf(txtPeriodoMinimoMantenimientoCatalogo.Text.Trim = "", "0", txtPeriodoMinimoMantenimientoCatalogo.Text.Trim))
            Equipo.vNumeroSerieEquipo = txtNroSerieEquipo.Text
            Equipo.vNumeroParteEquipo = txtNroParteEquipo.Text
            Equipo.cIdCliente = hfdIdAuxiliar.Value
            Equipo.cIdClienteUbicacion = IIf(cboClienteUbicacion.SelectedValue = "SELECCIONE DATO", Nothing, cboClienteUbicacion.SelectedValue)
            Equipo.dFechaCreacionEquipo = Convert.ToDateTime(IIf(hfdFechaCreacionEquipo.Value = "", Now, hfdFechaCreacionEquipo.Value))
            Equipo.dFechaUltimaModificacionEquipo = Now
            Equipo.cIdUsuarioCreacionEquipo = hfdIdUsuarioCreacionEquipo.Value
            Equipo.cIdUsuarioUltimaModificacionEquipo = Session("IdUsuario")
            Equipo.cEstadoEquipo = "R" 'Registrado
            Equipo.bTieneContratoEquipo = lnkEsConContratoOn.Visible
            Equipo.vContratoReferenciaActualEquipo = IIf(CBool(Equipo.bTieneContratoEquipo) = True, cboContratoReferencia.SelectedValue, Nothing)
            Equipo.vTagEquipo = UCase(txtTagEquipo.Text.Trim)
            Equipo.vCapacidadEquipo = UCase(txtCapacidadEquipo.Text.Trim)
            Equipo.cIdTipoEquipo = cboTipoEquipo.SelectedValue
            Equipo.vDescripcionEquipoSAP = UCase(txtDescripcionEquipoSAP.Text.Trim)

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacion.Value = "N" Then
                If (EquipoNeg.EquipoInserta(Equipo)) = 0 Then
                    Dim Coleccion As New List(Of LOGI_EQUIPO)
                    For i = 0 To Session("CestaEquipoComponente").Rows.Count - 1
                        Dim DetEquipo As New LOGI_EQUIPO
                        DetEquipo.cIdEquipo = Session("CestaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                        DetEquipo.cIdEmpresa = Session("IdEmpresa")
                        DetEquipo.cIdTipoActivo = Session("CestaEquipoComponente").Rows(i)("IdTipoActivo").ToString.Trim
                        DetEquipo.vDescripcionEquipo = Session("CestaEquipoComponente").Rows(i)("Descripcion").ToString.Trim
                        DetEquipo.vDescripcionAbreviadaEquipo = Session("CestaEquipoComponente").Rows(i)("DescripcionAbreviada").ToString.Trim
                        DetEquipo.dFechaTransaccionEquipo = hfdFechaTransaccionEquipoComponente.Value
                        DetEquipo.cIdEnlaceEquipo = Equipo.cIdEquipo 'Session("CestaMaestroActivoComponente").Rows(i)("IdEnlace").ToString.Trim
                        DetEquipo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'MaestroActivo.cIdEnlaceCatalogo
                        DetEquipo.bEstadoRegistroEquipo = "1"
                        DetEquipo.cIdSistemaFuncionalEquipo = Session("CestaEquipoComponente").Rows(i)("IdSistemaFuncional").ToString.Trim
                        DetEquipo.cIdFamilia = Nothing
                        DetEquipo.cIdSubFamilia = Nothing
                        DetEquipo.vObservacionEquipo = Session("CestaEquipoComponente").Rows(i)("Observacion").ToString.Trim
                        DetEquipo.cIdEstadoComponenteEquipo = Session("CestaEquipoComponente").Rows(i)("IdEstadoActivo").ToString.Trim
                        DetEquipo.vIdEquivalenciaEquipo = "" 'Session("CestaEquipoComponente").Rows(i)("IdEquivalencia").ToString.Trim 'JMUG: 31/07/2023 LO QUITÉ PORQUE ME DA ERROR AL CREAR UNO NUEVO
                        DetEquipo.cIdPaisOrigenEquipo = Session("IdPais")
                        DetEquipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
                        DetEquipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
                        DetEquipo.dFechaRegistroTarjetaSAPEquipo = Now 'hfdFechaRegistroTarjetaSAPEquipo.value
                        DetEquipo.dFechaManufacturaTarjetaSAPEquipo = Now
                        DetEquipo.cIdCatalogo = Session("CestaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                        DetEquipo.cIdJerarquiaCatalogo = "1"
                        DetEquipo.nVidaUtilEquipo = Session("CestaEquipoComponente").Rows(i)("VidaUtil").ToString.Trim
                        DetEquipo.nPeriodoGarantiaEquipo = Nothing
                        DetEquipo.nPeriodoMinimoMantenimientoEquipo = Nothing
                        DetEquipo.vNumeroSerieEquipo = Session("CestaEquipoComponente").Rows(i)("NroSerie").ToString.Trim
                        DetEquipo.vNumeroParteEquipo = Session("CestaEquipoComponente").Rows(i)("NroParte").ToString.Trim
                        DetEquipo.cIdCliente = hfdIdAuxiliar.Value

                        DetEquipo.cIdClienteUbicacion = IIf(cboClienteUbicacion.SelectedValue = "SELECCIONE DATO", Nothing, cboClienteUbicacion.SelectedValue)
                        DetEquipo.dFechaCreacionEquipo = IIf(hfdFechaCreacionEquipoComponente.Value = "", Now, hfdFechaCreacionEquipoComponente.Value)
                        DetEquipo.dFechaUltimaModificacionEquipo = Now
                        DetEquipo.cIdUsuarioCreacionEquipo = hfdIdUsuarioCreacionEquipoComponente.Value
                        DetEquipo.cIdUsuarioUltimaModificacionEquipo = Session("IdUsuario")
                        Coleccion.Add(DetEquipo)
                    Next

                    If EquipoNeg.EquipoInsertaDetalle(Coleccion, Equipo.cIdEquipo, Equipo.cIdCatalogo) = 0 Then
                        Session("Query") = "PA_LOGI_MNT_EQUIPO 'SQL_INSERT', '','" & Equipo.cIdEquipo & "', '" & Equipo.cIdCatalogo & "', '" &
                                           Equipo.cIdTipoActivo & "', '" & Equipo.vDescripcionEquipo & "', '" &
                                           Equipo.vDescripcionAbreviadaEquipo & "', '" & Equipo.cIdJerarquiaCatalogo & "', '" &
                                           Equipo.dFechaTransaccionEquipo & "', '" & Equipo.cIdEnlaceEquipo & "', '" &
                                           Equipo.cIdEnlaceCatalogo & "', '" & Equipo.cIdSistemaFuncionalEquipo & "', '" & Equipo.bEstadoRegistroEquipo & "', '" &
                                           Equipo.vObservacionEquipo & "', " & Equipo.nVidaUtilEquipo & ", '" &
                                           Equipo.cIdEmpresa & "', '" & Equipo.cIdEstadoComponenteEquipo & "', '" &
                                           Equipo.vIdEquivalenciaEquipo & "', '" & Equipo.cIdPaisOrigenEquipo & "', " &
                                           Equipo.vIdClienteSAPEquipo & ", '" & Equipo.cIdEquipoSAPEquipo & "', '" &
                                           Equipo.dFechaRegistroTarjetaSAPEquipo & "', '" & Equipo.dFechaManufacturaTarjetaSAPEquipo & "', '" &
                                           Equipo.nPeriodoGarantiaEquipo & "', '" & Equipo.nPeriodoMinimoMantenimientoEquipo & "', '" &
                                           Equipo.vNumeroSerieEquipo & "', '" & Equipo.vNumeroParteEquipo & "', '" & Equipo.cIdCliente & "', '" &
                                           Equipo.cIdEquipo & "'"

                        LogAuditoria.vEvento = "INSERTAR EQUIPO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo 'Session("IdModulo")

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        txtIdEquipo.Text = Equipo.cIdEquipo
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        'CargarCestaComponenteCatalogo()
                        'CargarCestaComponenteMaestroActivo()
                        CargarCestaCatalogoComponente()
                        CargarCestaEquipoComponente()

                        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, 0)
                        Me.grdLista.DataBind()

                        pnlCabecera.Visible = True
                        pnlEquipo.Visible = False
                        pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarEquipo.Focus()
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                End If
            ElseIf hfdOperacion.Value = "E" Then
                If (EquipoNeg.EquipoEdita(Equipo)) = 0 Then
                    Dim Coleccion As New List(Of LOGI_EQUIPO)
                    For i = 0 To Session("CestaEquipoComponente").Rows.Count - 1
                        Dim DetEquipo As New LOGI_EQUIPO

                        DetEquipo.cIdEquipo = Session("CestaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                        DetEquipo.cIdEmpresa = Session("IdEmpresa")
                        DetEquipo.cIdTipoActivo = Session("CestaEquipoComponente").Rows(i)("IdTipoActivo").ToString.Trim
                        DetEquipo.vDescripcionEquipo = Session("CestaEquipoComponente").Rows(i)("Descripcion").ToString.Trim
                        DetEquipo.vDescripcionAbreviadaEquipo = Session("CestaEquipoComponente").Rows(i)("DescripcionAbreviada").ToString.Trim
                        DetEquipo.dFechaTransaccionEquipo = Session("CestaEquipoComponente").Rows(i)("FechaTransaccion").ToString.Trim
                        DetEquipo.cIdEnlaceEquipo = Equipo.cIdEquipo 'Session("CestaMaestroActivoComponente").Rows(i)("IdEnlace").ToString.Trim
                        DetEquipo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'MaestroActivo.cIdEnlaceCatalogo
                        DetEquipo.bEstadoRegistroEquipo = "1"
                        DetEquipo.cIdSistemaFuncionalEquipo = Session("CestaEquipoComponente").Rows(i)("IdSistemaFuncional").ToString.Trim
                        DetEquipo.cIdFamilia = Nothing
                        DetEquipo.cIdSubFamilia = Nothing
                        DetEquipo.vObservacionEquipo = Session("CestaEquipoComponente").Rows(i)("Observacion").ToString.Trim
                        DetEquipo.cIdEstadoComponenteEquipo = "01" 'Session("CestaEquipoComponente").Rows(i)("IdEstadoActivo").ToString.Trim
                        DetEquipo.vIdEquivalenciaEquipo = "" 'Session("CestaEquipoComponente").Rows(i)("IdEquivalencia").ToString.Trim
                        DetEquipo.cIdPaisOrigenEquipo = Session("IdPais")
                        DetEquipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
                        DetEquipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
                        DetEquipo.dFechaRegistroTarjetaSAPEquipo = IIf(hfdFechaRegistroTarjetaSAPEquipo.Value = "", Nothing, hfdFechaRegistroTarjetaSAPEquipo.Value) 'Now 'hfdFechaRegistroTarjetaSAPEquipo.value
                        DetEquipo.dFechaManufacturaTarjetaSAPEquipo = IIf(hfdFechaRegistroTarjetaSAPEquipo.Value = "", Nothing, hfdFechaRegistroTarjetaSAPEquipo.Value) 'Now
                        DetEquipo.cIdCatalogo = Session("CestaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                        DetEquipo.cIdJerarquiaCatalogo = "1"
                        DetEquipo.nVidaUtilEquipo = Session("CestaEquipoComponente").Rows(i)("VidaUtil").ToString.Trim
                        DetEquipo.nPeriodoGarantiaEquipo = Session("CestaEquipoComponente").Rows(i)("PeriodoGarantia").ToString.Trim 'Nothing
                        DetEquipo.nPeriodoMinimoMantenimientoEquipo = Session("CestaEquipoComponente").Rows(i)("PeriodoMinimo").ToString.Trim ' Nothing
                        DetEquipo.vNumeroSerieEquipo = Session("CestaEquipoComponente").Rows(i)("NroSerie").ToString.Trim
                        DetEquipo.vNumeroParteEquipo = Session("CestaEquipoComponente").Rows(i)("NroParte").ToString.Trim
                        DetEquipo.cIdCliente = hfdIdAuxiliar.Value
                        DetEquipo.cIdClienteUbicacion = IIf(cboClienteUbicacion.SelectedValue = "SELECCIONE DATO", Nothing, cboClienteUbicacion.SelectedValue)
                        DetEquipo.dFechaCreacionEquipo = Convert.ToDateTime(IIf(hfdFechaCreacionEquipoComponente.Value = "", Now, hfdFechaCreacionEquipoComponente.Value))
                        DetEquipo.dFechaUltimaModificacionEquipo = Now
                        DetEquipo.cIdUsuarioCreacionEquipo = hfdIdUsuarioCreacionEquipoComponente.Value
                        DetEquipo.cIdUsuarioUltimaModificacionEquipo = Session("IdUsuario")

                        Coleccion.Add(DetEquipo)
                    Next

                    If EquipoNeg.EquipoInsertaDetalle(Coleccion, Equipo.cIdEquipo, cboCatalogo.SelectedValue) = 0 Then
                        Session("Query") = "PA_LOGI_MNT_EQUIPO 'SQL_INSERT', '','" & Equipo.cIdEquipo & "', '" & Equipo.cIdCatalogo & "', '" &
                                           Equipo.cIdTipoActivo & "', '" & Equipo.vDescripcionEquipo & "', '" &
                                           Equipo.vDescripcionAbreviadaEquipo & "', '" & Equipo.cIdJerarquiaCatalogo & "', '" &
                                           Equipo.dFechaTransaccionEquipo & "', '" & Equipo.cIdEnlaceEquipo & "', '" &
                                           Equipo.cIdEnlaceCatalogo & "', '" & Equipo.cIdSistemaFuncionalEquipo & "', '" & Equipo.bEstadoRegistroEquipo & "', '" &
                                           Equipo.vObservacionEquipo & "', " & Equipo.nVidaUtilEquipo & ", '" &
                                           Equipo.cIdEmpresa & "', '" & Equipo.cIdEstadoComponenteEquipo & "', '" &
                                           Equipo.vIdEquivalenciaEquipo & "', '" & Equipo.cIdPaisOrigenEquipo & "', " &
                                           Equipo.vIdClienteSAPEquipo & ", '" & Equipo.cIdEquipoSAPEquipo & "', '" &
                                           Equipo.dFechaRegistroTarjetaSAPEquipo & "', '" & Equipo.dFechaManufacturaTarjetaSAPEquipo & "', '" &
                                           Equipo.nPeriodoGarantiaEquipo & "', '" & Equipo.nPeriodoMinimoMantenimientoEquipo & "', '" &
                                           Equipo.vNumeroSerieEquipo & "', '" & Equipo.vNumeroParteEquipo & "', '" & Equipo.cIdCliente & "', '" &
                                           Equipo.cIdEquipo & "'"

                        LogAuditoria.vEvento = "ACTUALIZAR EQUIPO"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo 'Session("IdModulo")

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                        Dim item As Int16
                        Dim strIdEquipo As String = ""

                        'JMUG: INICIO: 26/03/2023
                        item = 0
                        'Dim resultCaracteristicaEquipoPrincipalSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & Equipo.cIdCatalogo & "' AND IdJerarquia = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
                        Dim resultCaracteristicaEquipoPrincipalSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & Equipo.cIdCatalogo & "'")
                        If resultCaracteristicaEquipoPrincipalSimple.Length > 0 Then
                            Dim rowFil As DataRow() = resultCaracteristicaEquipoPrincipalSimple
                            For Each filacaracteristica As DataRow In rowFil
                                Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                                item = item + 1
                                DetCaracteristica.cIdEquipo = Equipo.cIdEquipo
                                DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                                DetCaracteristica.cIdCaracteristica = filacaracteristica("IdCaracteristica") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                                DetCaracteristica.nIdNumeroItemEquipoCaracteristica = item 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                                DetCaracteristica.vValorReferencialEquipoCaracteristica = UCase(filacaracteristica("Valor")) 'UCase(Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim)
                                DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = filacaracteristica("IdReferenciaSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = filacaracteristica("DescripcionCampoSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                DetCaracteristica.cIdCatalogoEquipoCaracteristica = filacaracteristica("IdCatalogo")
                                ColeccionCaracteristica.Add(DetCaracteristica)
                            Next
                        End If
                        'JMUG: FINAL: 26/03/2023

                        For Each fila In Coleccion
                            item = 0
                            Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & fila.cIdCatalogo & "' AND IdJerarquia = '1'")
                            If resultCaracteristicaSimple.Length > 0 Then
                                Dim rowFil As DataRow() = resultCaracteristicaSimple
                                For Each filacaracteristica As DataRow In rowFil
                                    Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                                    item = item + 1
                                    'DetCaracteristica.cIdEquipo = filacaracteristica("IdEquipo") 'fila.cIdEquipo
                                    DetCaracteristica.cIdEquipo = fila.cIdEquipo
                                    DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                                    DetCaracteristica.cIdCaracteristica = filacaracteristica("IdCaracteristica") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                                    DetCaracteristica.nIdNumeroItemEquipoCaracteristica = item 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                                    DetCaracteristica.vValorReferencialEquipoCaracteristica = UCase(filacaracteristica("Valor")) 'UCase(Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim)
                                    DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = filacaracteristica("IdReferenciaSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                                    DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = filacaracteristica("DescripcionCampoSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                    DetCaracteristica.cIdCatalogoEquipoCaracteristica = filacaracteristica("IdCatalogo") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                                    ColeccionCaracteristica.Add(DetCaracteristica)
                                Next
                                'Exit Sub
                            End If
                        Next

                        If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(Equipo.cIdEquipo, ColeccionCaracteristica, LogAuditoria) Then

                        End If

                        MyValidator.ErrorMessage = "Registro actualizado con éxito"

                        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, 0)
                        Me.grdLista.DataBind()
                        'pnlGeneral.Enabled = False
                        'BloquearMantenimiento(True, False, True, False)
                        'lblOperacion.Value = "R"
                        pnlCabecera.Visible = True
                        pnlEquipo.Visible = False
                        pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarEquipo.Focus()
                    Else
                        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    End If
                    hfdOperacion.Value = "R"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If

            Me.grdCatalogoComponente.DataSource = Nothing
            Me.grdCatalogoComponente.DataBind()
            Me.grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            Me.grdCatalogoComponente.DataBind()

            Me.grdEquipoComponente.DataSource = Nothing
            Me.grdEquipoComponente.DataBind()
            Me.grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            Me.grdEquipoComponente.DataBind()

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

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                'Dim Catalogo As New LOGI_CATALOGO
                Dim Equipo As New LOGI_EQUIPO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                'Catalogo.cIdCatalogo = Valores(0).ToString()
                'Catalogo.cIdTipoActivo = Valores(1).ToString()
                'Catalogo.cIdJerarquiaCatalogo = "0"
                Equipo.cIdEquipo = Valores(0).ToString

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                'CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO Set bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                'Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 0 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                Session("Query") = "UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "DESACTIVAR CATALOGO"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                MyValidator.ErrorMessage = "Registro desactivado con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                Me.Page.Validators.Add(MyValidator)

                'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                Me.grdLista.DataBind()
            End If
            If e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Equipo As New LOGI_EQUIPO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                'Catalogo.cIdCatalogo = Valores(0).ToString()
                'Catalogo.cIdTipoActivo = Valores(1).ToString()
                'Catalogo.cIdJerarquiaCatalogo = "0"
                Equipo.cIdEquipo = Valores(0).ToString

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                'CatalogoNeg.CatalogoGetData("UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                'Session("Query") = "UPDATE LOGI_CATALOGO SET bEstadoRegistroCatalogo = 1 WHERE cIdCatalogo = '" & Catalogo.cIdCatalogo & "' AND cIdTipoActivo = '" & Catalogo.cIdTipoActivo & "' AND cIdJerarquiaCatalogo = '0'" ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 1 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 1 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "ACTIVAR CATALOGO"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                MyValidator.ErrorMessage = "Registro activado con éxito"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                Me.Page.Validators.Add(MyValidator)

                'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
                Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                Me.grdLista.DataBind()
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Sub ListarContratoReferenciaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ContratoNeg As New clsContratoNegocios
        cboContratoReferencia.DataTextField = "vDescripcionCabeceraContrato"
        cboContratoReferencia.DataValueField = "vIdNumeroCorrelativoCabeceraContrato"
        cboContratoReferencia.DataSource = ContratoNeg.ContratoListarCombo("1", "'R','P'")
        cboContratoReferencia.DataBind()
    End Sub

    Sub ListarPaisClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboPaisMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboPaisMensajeClienteUbicacion.DataValueField = "cIdPaisUbicacionGeografica"
        cboPaisMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.PaisListarCombo
        cboPaisMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarDepartamentoClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDepartamentoMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboDepartamentoMensajeClienteUbicacion.DataValueField = "cIdDepartamentoUbicacionGeografica"
        cboDepartamentoMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.DepartamentoListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue)
        cboDepartamentoMensajeClienteUbicacion.Items.Clear()
        cboDepartamentoMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarProvinciaClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboProvinciaMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboProvinciaMensajeClienteUbicacion.DataValueField = "cIdProvinciaUbicacionGeografica"
        cboProvinciaMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.ProvinciaListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue)
        cboProvinciaMensajeClienteUbicacion.Items.Clear()
        cboProvinciaMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarDistritoClienteUbicacionCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
        cboDistritoMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
        cboDistritoMensajeClienteUbicacion.DataValueField = "cIdDistritoUbicacionGeografica"
        cboDistritoMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.DistritoListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue, cboProvinciaMensajeClienteUbicacion.SelectedValue)
        cboDistritoMensajeClienteUbicacion.Items.Clear()
        cboDistritoMensajeClienteUbicacion.DataBind()
    End Sub

    Sub ListarClienteUbicacionCombo()
        Try
            Dim DirClienteNeg As New clsClienteNegocios
            cboClienteUbicacion.DataTextField = "vDescripcionClienteUbicacion"
            cboClienteUbicacion.DataValueField = "cIdUbicacion"
            cboClienteUbicacion.DataSource = DirClienteNeg.ClienteGetData("SELECT cIdCliente, CLIUBI.cIdUbicacion, UPPER(vDescripcionClienteUbicacion) + ' - ' + " &
                                             "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
                                             "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + '00' + '00') " &
                                             " + ' - ' + " &
                                             "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
                                             "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + CLIUBI.cIdProvinciaUbicacionGeografica + '00') " &
                                             " + ' - ' + " &
                                             "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
                                             "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + CLIUBI.cIdProvinciaUbicacionGeografica + CLIUBI.cIdDistritoUbicacionGeografica) AS vDescripcionClienteUbicacion " &
                                             "FROM GNRL_CLIENTEUBICACION AS CLIUBI WHERE LTRIM(RTRIM(CLIUBI.cIdCliente)) = '" & hfdIdAuxiliar.Value.Trim & "' " &
                                             "ORDER BY cIdCliente")
            cboClienteUbicacion.Items.Clear()
            cboClienteUbicacion.Items.Add("SELECCIONE DATO")
            cboClienteUbicacion.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub cboPaisMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPaisMensajeClienteUbicacion.SelectedIndexChanged
        ListarDepartamentoClienteUbicacionCombo()
        ListarProvinciaClienteUbicacionCombo()
        ListarDistritoClienteUbicacionCombo()
        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboDepartamentoMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDepartamentoMensajeClienteUbicacion.SelectedIndexChanged
        ListarProvinciaClienteUbicacionCombo()
        ListarDistritoClienteUbicacionCombo()
        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    End Sub

    Protected Sub cboProvinciaMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProvinciaMensajeClienteUbicacion.SelectedIndexChanged
        ListarDistritoClienteUbicacionCombo()
        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAdicionarClienteUbicacion_Click(sender As Object, e As EventArgs) Handles btnAdicionarClienteUbicacion.Click
        Try
            lblMensajeClienteUbicacion.Text = ""
            fValidarSesion()
            cboPaisMensajeClienteUbicacion.Focus()

            Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
            Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
            UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorIdEquivalencia(hfdIdUbicacionGeograficaCliente.Value)

            ListarPaisClienteUbicacionCombo()
            lblRazonSocialClienteMensajeClienteUbicacion.Text = txtRazonSocial.Text
            'JMUG: 05/03/2023 hfdIdUbicacionGeograficaEntregaCliente.Value = hfdIdUbicacionGeograficaCliente.Value
            hfdIdUbicacionGeograficaClienteUbicacion.Value = hfdIdUbicacionGeograficaCliente.Value
            cboPaisMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdPaisUbicacionGeografica 'Mid(hfdIdUbicacionGeograficaEntregaCliente.Value, 1, 2) 'cboPaisMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdPaisUbicacionGeografica
            cboPaisMensajeClienteUbicacion_SelectedIndexChanged(cboPaisMensajeClienteUbicacion, New System.EventArgs())
            cboDepartamentoMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdDepartamentoUbicacionGeografica 'cboDepartamentoMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
            cboDepartamentoMensajeClienteUbicacion_SelectedIndexChanged(cboDepartamentoMensajeClienteUbicacion, New System.EventArgs())
            cboProvinciaMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdProvinciaUbicacionGeografica 'cboProvinciaMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
            cboProvinciaMensajeClienteUbicacion_SelectedIndexChanged(cboProvinciaMensajeClienteUbicacion, New System.EventArgs())
            cboDistritoMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdDistritoUbicacionGeografica 'cboDistritoMensajeCliente.SelectedValue 'ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
            txtDireccionAdicionalMensajeClienteUbicacion.Text = ""
            'txtCorreoAdicionalMensajeClienteUbicacion.Text = ""
            'txtTelefonoAdicionalMensajeClienteUbicacion.Text = ""
            'txtCelularAdicionalMensajeClienteUbicacion.Text = ""
            'txtFaxAdicionalMensajeClienteUbicacion.Text = ""
            lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
        Catch ex As Exception
            lblMensajeClienteUbicacion.Text = ex.Message
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAceptarMensajeClienteUbicacion_Click(sender As Object, e As EventArgs) Handles btnAceptarMensajeClienteUbicacion.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            MyValidator.ErrorMessage = ""
            lblMensajeClienteUbicacion.Text = ""
            'JMUG: 05/03/2023 hfdIdUbicacionGeograficaEntregaCliente.Value = ""
            hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
            'If txtCorreoAdicionalMensajeDireccion.Text.Trim = "" Or FuncionesNeg.IsValidarEmail(txtCorreoAdicionalMensajeDireccion.Text.Trim) = False Then
            '    Throw New Exception("Debe de ingresar un correo válido, favor de validar la información...!!!")
            'ElseIf txtDireccionAdicionalMensajeDireccion.Text.Trim = "" Then
            '    Throw New Exception("Debe de ingresar dirección entrega, favor de validar la información...!!!")
            'End If
            If txtDireccionAdicionalMensajeClienteUbicacion.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar una ubicación, favor de validar la información...!!!")
            End If

            AdicionarClienteUbicacion(hfdIdAuxiliar.Value)

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lblMensajeClienteUbicacion.Text = ex.Message
            lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
        End Try
    End Sub

    Sub AdicionarClienteUbicacion(ByVal strIdCliente As String)
        'Try
        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

        MyValidator.ErrorMessage = ""
        lblMensajeClienteUbicacion.Text = ""
        'JMUG: 05/03/2023 hfdIdUbicacionGeograficaEntregaCliente.Value = ""
        hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
        Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA

        Dim DirClienteNeg As New clsClienteNegocios
        Dim dtDirCliente As New DataTable
        dtDirCliente = DirClienteNeg.ClienteGetData("SELECT COUNT(*) AS nCantidad FROM GNRL_CLIENTEUBICACION WHERE LTRIM(RTRIM(cIdCliente)) = '" & strIdCliente & "'")

        'If dtDirCliente.Rows(0).Item(0) = 0 Then
        '    'DirClienteNeg.ClienteGetData("INSERT INTO GNRL_CLIENTEUBICACION (cIdCliente, cIdClienteDir, vNombreClienteDir, vDireccionClienteDir, vTelefonoClienteDir, vCelularClienteDir, vFaxClienteDir, vEmailClienteDir, bEstadoRegistroClienteDir, cIdPaisUbicacionGeografica, " &
        '    '                             "cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica) " &
        '    '                             "VALUES ('" & hfdIdAuxiliar.Value & "', '00', 'DIR-LEGAL', " &
        '    '                             "         '" & txtDireccionAdicionalMensajeCliente.Text.Trim.ToUpper & "', '', '', '', " &
        '    '                             "         '" & txtCorreoAdicionalMensajeCliente.Text.Trim.Trim.ToUpper & "', 1, '" & cboPaisMensajeCliente.Text.Trim.ToUpper & "', " &
        '    '                             "         '" & cboDepartamentoMensajeCliente.SelectedValue & "', '" & cboProvinciaMensajeCliente.SelectedValue & "', '" & cboDistritoMensajeCliente.SelectedValue & "')")
        '    DirClienteNeg.ClienteGetData("INSERT INTO GNRL_CLIENTEUBICACION (cIdCliente, cIdUbicacion, vDescripcionClienteUbicacion, bEstadoRegistroClienteUbicacion, cIdPaisUbicacionGeografica, " &
        '                                 "cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica) " &
        '                                 "VALUES ('" & hfdIdAuxiliar.Value & "', '00', 'DIR-LEGAL', " &
        '                                 "         '" & txtDireccionAdicionalMensajeCliente.Text.Trim.ToUpper & "', '', '', '', " &
        '                                 "         '" & txtCorreoAdicionalMensajeCliente.Text.Trim.Trim.ToUpper & "', 1, '" & cboPaisMensajeCliente.Text.Trim.ToUpper & "', " &
        '                                 "         '" & cboDepartamentoMensajeCliente.SelectedValue & "', '" & cboProvinciaMensajeCliente.SelectedValue & "', '" & cboDistritoMensajeCliente.SelectedValue & "')")
        '    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(cboPaisMensajeCliente.Text.Trim.ToUpper, cboDepartamentoMensajeCliente.SelectedValue, cboProvinciaMensajeCliente.SelectedValue, cboDistritoMensajeCliente.SelectedValue)
        'Else
        'DirClienteNeg.ClienteGetData("INSERT INTO GNRL_CLIENTEUBICACION (cIdCliente, cIdUbicacion, vDescripcionClienteUbicacion, bEstadoRegistroClienteUbicacion, " &
        '                                 "cIdPaisUbicacionGeografica, cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica) " &
        '                                 "VALUES ('" & hfdIdAuxiliar.Value & "', (SELECT RIGHT ('00000' + CONVERT (VARCHAR(5), (CONVERT (NUMERIC, ISNULL(MAX(cIdUbicacion), 0)) + 1)), 5) FROM GNRL_CLIENTEUBICACION WHERE cIdCliente = '" & hfdIdAuxiliar.Value & "'), 'DIR-" & cboDistritoMensajeClienteUbicacion.SelectedItem.Text & "', " &
        '                                 "         '" & txtDireccionAdicionalMensajeClienteUbicacion.Text.Trim.ToUpper & "', 1, '" & cboPaisMensajeClienteUbicacion.SelectedValue & "', " &
        '                                 "         '" & cboDepartamentoMensajeClienteUbicacion.SelectedValue & "', '" & cboProvinciaMensajeClienteUbicacion.SelectedValue & "', '" & cboDistritoMensajeClienteUbicacion.SelectedValue & "')")
        DirClienteNeg.ClienteGetData("INSERT INTO GNRL_CLIENTEUBICACION (cIdCliente, cIdUbicacion, vDescripcionClienteUbicacion, bEstadoRegistroClienteUbicacion, " &
                                         "cIdPaisUbicacionGeografica, cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica) " &
                                         "VALUES ('" & hfdIdAuxiliar.Value & "', (SELECT RIGHT ('00000' + CONVERT (VARCHAR(5), (CONVERT (NUMERIC, ISNULL(MAX(cIdUbicacion), 0)) + 1)), 5) FROM GNRL_CLIENTEUBICACION WHERE cIdCliente = '" & hfdIdAuxiliar.Value & "'), " &
                                         "         '" & txtDireccionAdicionalMensajeClienteUbicacion.Text.Trim.ToUpper & "', 1, '" & cboPaisMensajeClienteUbicacion.SelectedValue & "', " &
                                         "         '" & cboDepartamentoMensajeClienteUbicacion.SelectedValue & "', '" & cboProvinciaMensajeClienteUbicacion.SelectedValue & "', '" & cboDistritoMensajeClienteUbicacion.SelectedValue & "')")
        UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue, cboProvinciaMensajeClienteUbicacion.SelectedValue, cboDistritoMensajeClienteUbicacion.SelectedValue)
        'End If
        'hfdIdUbicacionGeograficaEntregaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
        hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
        'ListarDireccionEntregaCombo()
        ListarClienteUbicacionCombo()
        'cboDireccionEntrega.SelectedIndex = cboDireccionEntrega.Items.Count - 1
        cboClienteUbicacion.SelectedIndex = cboClienteUbicacion.Items.Count - 1
        'lblTituloMensajeDireccion.Visible = True
        lblTituloMensajeClienteUbicacion.Visible = True
        'JMUG: 01/04/2022 Dim dtClienteDireccion As New DataTable
        'If dtClienteDireccion.Rows.Count > 0 Then 'Existe en el sistema
        'Else
        '    If txtIdCliente.Text.Trim = "" Then
        '        lnk_mostrarPanelCliente_ModalPopupExtender.Show()
        '    Else
        '        MyValidator.ErrorMessage = "Debe de ingresar el cliente desde la opción de mantenimiento."
        '        ValidationSummary1.ValidationGroup = "vgrpValidar"
        '        MyValidator.IsValid = False
        '        MyValidator.ID = "ErrorPersonalizado"
        '        MyValidator.ValidationGroup = "vgrpValidar"
        '        Me.Page.Validators.Add(MyValidator)
        '    End If
        'JMUG: 01/04/2022 End If
        'Catch ex As Exception
        '    'JMUG: 25/03/2022 MyValidator.ErrorMessage = "No se estableció conexión con la SUNAT.  Utilizará los datos del sistema."
        '    'ValidationSummary1.ValidationGroup = "vgrpValidar"
        '    'lblMensajeDireccion.Text = MyValidator.ErrorMessage
        '    lblMensajeDireccion.Text = ex.Message
        '    lnk_mostrarPanelMensajeDireccion_ModalPopupExtender.Show()
        '    'MyValidator.IsValid = False
        '    'MyValidator.ID = "ErrorPersonalizado"
        '    'MyValidator.ValidationGroup = "vgrpValidar"
        '    'Me.Page.Validators.Add(MyValidator)
        'End Try
    End Sub

    Private Sub cboTipoActivoEquipoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivoEquipoComponente.SelectedIndexChanged
        ListarSistemaFuncionalEquipoComponenteCombo()
        lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
    End Sub

    Sub txtValorDetalle_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipo.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipo.Rows.Count * (grdDetalleCaracteristicaEquipo.PageIndex))
                Session("CestaCaracteristicaEquipoPrincipal").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtValordDetalleCaracteristicaMantenimientoCatalogo_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaMantenimientoCatalogo.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaMantenimientoCatalogo.Rows.Count * (grdDetalleCaracteristicaMantenimientoCatalogo.PageIndex))
                Session("CestaCatalogoCaracteristica").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtValorDetalleEquipoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalleEquipoComponente"), TextBox)
                'Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(row.RowIndex)("Valor") = txtValorDetalle.Text
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtValorDetalleCaracteristicaCatalogoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaCatalogoComponente.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                'Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(row.RowIndex)("Valor") = txtValorDetalle.Text
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaCatalogoComponente.Rows.Count * (grdDetalleCaracteristicaCatalogoComponente.PageIndex))
                Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtIdReferenciaSAPDetalleEquipoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalleEquipoComponente"), TextBox)
                'Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(row.RowIndex)("IdReferenciaSAP") = txtIdReferenciaSAPDetalle.Text
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("IdReferenciaSAP") = txtIdReferenciaSAPDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtDescripcionCampoSAPDetalleEquipoComponente_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalleEquipoComponente"), TextBox)
                'Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(row.RowIndex)("DescripcionCampoSAP") = txtDescripcionCampoSAPDetalle.Text
                Dim FilaActual As Int16
                FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("DescripcionCampoSAP") = txtDescripcionCampoSAPDetalle.Text
            Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdCatalogoComponente_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCatalogoComponente.PageIndexChanging
        grdCatalogoComponente.PageIndex = e.NewPageIndex
        grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
        Me.grdCatalogoComponente.DataBind()
        grdCatalogoComponente.SelectedIndex = -1
    End Sub

    Private Sub btnAdicionarCaracteristicaEquipo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaEquipo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0656", strOpcionModulo, "CMMS")

            If cboCatalogo.SelectedValue = "SELECCIONE DATO" Then
                Throw New Exception("Debe de asignarle un catalogo al equipo.")
            End If

            For i = 0 To Session("CestaCaracteristicaEquipoPrincipal").Rows.Count - 1
                If (Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaEquipo.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCatalogo").ToString.Trim = cboCatalogo.SelectedValue.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                    Throw New Exception("Característica ya registrada, seleccione otro item.")
                    Exit Sub
                End If
            Next
            'clsLogiCestaCatalogoCaracteristica.AgregarCesta(txtIdCatalogo.Text, "", "0", cboCaracteristicaCatalogo.SelectedValue.Trim, UCase(cboCaracteristicaCatalogo.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogo"))
            clsLogiCestaCatalogoCaracteristica.AgregarCesta(cboCatalogo.SelectedValue, "", "'" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'", cboCaracteristicaEquipo.SelectedValue.Trim, UCase(cboCaracteristicaEquipo.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoPrincipal"))
            Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
            Me.grdDetalleCaracteristicaEquipo.DataBind()
            cboCaracteristicaEquipo.SelectedIndex = -1
            grdDetalleCaracteristicaEquipo.SelectedIndex = -1
            MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
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

    Private Sub grdDetalleCaracteristicaEquipo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaEquipo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "489", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0657", strOpcionModulo, "CMMS")

            'JMUG: 31/03/2023 OK
            'Dim fila As Integer = 0
            'For Each row As GridViewRow In grdDetalleCaracteristicaEquipo.Rows
            '    If row.RowType = DataControlRowType.DataRow Then
            '        Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
            '        If chkRow.Checked Then
            '            'clsVtasCestaComprobante.QuitarCesta(e.RowIndex, Session("CestaComprobante"))
            '            clsLogiCestaCatalogoCaracteristica.QuitarCesta(e.RowIndex, Session("CestaCaracteristicaEquipoPrincipal"))
            '            'grdDetalle.DataBind()
            '            fila += 1
            '        End If
            '    End If
            'Next

            For Each row As GridViewRow In grdDetalleCaracteristicaEquipo.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaCaracteristicaEquipoPrincipal").Rows.Count - 1
                            If (Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoPrincipal"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
            Me.grdDetalleCaracteristicaEquipo.DataBind()

            'TotalizarGrid()
            'lblCuadroDialogoAsiento.Value = "0" 'Recién lo puse
            'JMUG: 25/02/2022 BloquearMantenimiento(False, True, False, False)
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

    Private Sub grdDetalleCaracteristicaEquipoComponente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaEquipoComponente.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "355", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0659", strOpcionModulo, "CMMS")

            'JMUG: 31/03/2023 OK
            'Dim fila As Integer = 0
            For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        Dim FilaActual As Int16
                        For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
                            FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
                            If (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim) = hfdIdCatalogoEquipoComponente.Value.Trim And
                                    (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim) = "1" And
                                    (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim And
                                    (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim) = txtIdEquipoComponente.Text.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoComponente"))
                                Exit For
                            End If
                        Next

                        For i = 0 To Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows.Count - 1
                            If (Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                                Exit For
                            End If
                        Next
                        ''clsLogiCestaCatalogoCaracteristica.QuitarCesta(e.RowIndex, Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                        'clsLogiCestaCatalogoCaracteristica.QuitarCesta(FilaActual, Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                        'fila += 1
                    End If
                End If
            Next
            'CestaCaracteristicaEquipoComponenteFiltrado
            'Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponente")
            Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
            Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
            'lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
            lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnVerGaleriaFotosEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnVerGaleriaFotosEquipo.Click
        Try
            'Me.lstGaleriaEquipo.DataSource = ListaPrecioNeg.ListaPrecioListaBusqueda("bEstadoRegistroProducto='1' AND cIdCategoria",
            '                                                                                  "", "*", Session("IdEmpresa"), cboTipoAlmacen.SelectedValue, cboAlmacen.SelectedValue, EmpresaNeg.EmpresaListarPorId(Session("IdEmpresa")).nIdConfiguracionListaPrecio, hfdIdTipoCliente.Value, "1") ' "*")


            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
                                'Dim GaleriaNeg As New clsGaleriaEquipoNegocios
                                'Me.lstGaleriaEquipo.DataSource = GaleriaNeg.GaleriaEquipoListaBusqueda("cIdEquipo", grdLista.SelectedRow.Cells(0).Text, "1")
                                'Me.lstGaleriaEquipo.DataBind()
                                'lnk_mostrarPanelGaleriaEquipo_ModalPopupExtender.Show()
                                ''MyValidator.ErrorMessage = "Registro encontrado con éxito"

                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para mostrar su galería de imagenes.")
                    End If
                Else
                    Throw New Exception("Seleccione un equipo.")
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnCargarFotoEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnCargarFotoEquipo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                'BloquearMantenimiento(False, True, False, True)
                                ValidarTextoSubirImagenEquipo(True)
                                txtTituloSubirImagenEquipo.Text = ""
                                txtDescripcionSubirImagenEquipo.Text = ""
                                txtObservacionSubirImagenEquipo.Text = ""
                                'ActivarObjetos(True)
                                'LlenarData()
                                pnlCabecera.Visible = True
                                pnlEquipo.Visible = False
                                pnlComponentes.Visible = False
                                ValidationSummary1.ValidationGroup = "vgrpValidarSubirImagenEquipo"
                                lnk_mostrarPanelSubirImagenEquipo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para mostrar su galería de imagenes.")
                    End If
                Else
                    Throw New Exception("Seleccione un equipo.")
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub




    Private Sub btnAceptarSubirImagenEquipo_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirImagenEquipo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            'JMUG: 26/02/2023
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0379", strOpcionModulo, Session("IdSistema"))

            'If cboTipoActivo.SelectedIndex = 0 Then
            '    Throw New Exception("Debe de seleccionar un tipo de activo.")
            'End If
            If Not (fupSubirImagenEquipo.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
            Dim ext As String = fupSubirImagenEquipo.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"jpg", "jpeg", "bmp", "png", "gif"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de imagen inválido.")
            Dim Size As Integer = 0
            If Not (Integer.TryParse("160", Size)) Then
                Throw New Exception("El tamaño indicado para la imagen no es válido.")
            End If

            Dim GaleriaEquipo As New LOGI_GALERIAEQUIPO
            GaleriaEquipo.cIdEquipo = grdLista.SelectedRow.Cells(0).Text
            GaleriaEquipo.nIdNumeroItemGaleriaEquipo = 0
            GaleriaEquipo.vTituloGaleriaEquipo = UCase(txtTituloSubirImagenEquipo.Text.Trim)
            GaleriaEquipo.vDescripcionGaleriaEquipo = UCase(txtDescripcionSubirImagenEquipo.Text.Trim)
            GaleriaEquipo.vObservacionGaleriaEquipo = UCase(txtObservacionSubirImagenEquipo.Text.Trim)
            GaleriaEquipo.dFechaTransaccionGaleriaEquipo = Now
            GaleriaEquipo.bEstadoRegistroGaleriaEquipo = True

            Dim ColeccionGaleria As New List(Of LOGI_GALERIAEQUIPO)
            'Dim i As Integer
            'For i = 0 To grdDetalleCaracteristicaMantenimientoCatalogo.Rows.Count - 1
            '    Dim DetDocumento As New LOGI_CATALOGOCARACTERISTICA

            '    DetDocumento.cIdCatalogo = txtIdCatalogoMantenimientoCatalogo.Text
            '    'DetDocumento.cIdEmpresa = Session("IdEmpresa")
            '    DetDocumento.cIdJerarquiaCatalogo = "0"
            '    DetDocumento.cIdCaracteristica = Server.HtmlDecode(Replace(grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i).Cells(3).Text.ToString, "&nbsp;", ""))
            '    DetDocumento.nIdNumeroItemCatalogoCaracteristica = grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i).Cells(2).Text
            '    'Dim row As GridViewRow = grdDetalleCaracteristica.SelectedRow(i)
            '    Dim row = grdDetalleCaracteristicaMantenimientoCatalogo.Rows(i)
            '    'For Each row As GridViewRow In grdDetalle.Rows
            '    'MsgBox(Session("CestaComprobante").Rows(row.RowIndex)("Codigo").ToString())
            '    Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
            '    Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalle"), TextBox)
            '    Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalle"), TextBox)
            '    '    If txtCantidadGeneral.Text = "" Then

            '    DetDocumento.vValorCatalogoCaracteristica = UCase(txtValorDetalle.Text.Trim)
            '    DetDocumento.cIdReferenciaSAPCatalogoCaracteristica = UCase(txtIdReferenciaSAPDetalle.Text.Trim) 'Server.HtmlDecode(Replace(grdDetalleCaracteristica.Rows(i).Cells(5).Text.ToString, "&nbsp;", ""))
            '    DetDocumento.vDescripcionCampoSAPCatalogoCaracteristica = UCase(txtDescripcionCampoSAPDetalle.Text.Trim) 'Server.HtmlDecode(Replace(grdDetalleCaracteristica.Rows(i).Cells(6).Text.ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("Codigo")
            '    'DetDocumento.cIdJerarquiaCatalogo = IIf(hfdOperacion.Value = "N", "", Documento.vIdNumeroCorrelativoCabeceraOfertaVenta)
            '    'DetDocumento.cIdProductoVariante = Server.HtmlDecode(grdDetalle.Rows(i).Cells(8).Text).Trim 'Session("CestaComprobante").Rows(i)("Codigo")
            '    'DetDocumento.nIdNumeroItemDetalleOfertaVenta = grdDetalle.Rows(i).Cells(2).Text 'Session("CestaComprobante").rows(i)("Item")
            '    'DetDocumento.cIdTipoMoneda = Convert.ToChar(grdDetalle.Rows(i).Cells(10).Text) 'Convert.ToChar(grdDetalle.Rows(i).Cells(7).Text) 'Convert.ToChar(Session("CestaComprobante").Rows(i)("TMoneda")) 'cboTipoMoneda.SelectedValue

            '    'Dim row = grdDetalle.Rows(i)

            '    'Dim lblDescripcionProducto As Label = TryCast(row.FindControl("lblDescripcionDetalle"), Label)
            '    'DetDocumento.vDescripcionDetalleOfertaVenta = lblDescripcionProducto.Text 'Session("CestaComprobante").Rows(i)("Descripcion")
            '    'DetDocumento.nCantidadProductoDetalleOfertaVenta = grdDetalle.Rows(i).Cells(25).Text 'Session("CestaComprobante").Rows(i)("CantidadTotalUnidMedida")
            '    'DetDocumento.cIdUnidadMedidaProductoDetalleOfertaVenta = Server.HtmlDecode(grdDetalle.Rows(i).Cells(26).Text) 'Session("CestaComprobante").Rows(i)("IdUnidMedida")
            '    'DetDocumento.nCantidad1DetalleOfertaVenta = grdDetalle.Rows(i).Cells(21).Text 'Session("CestaComprobante").Rows(i)("Cantidad1")
            '    'DetDocumento.cIdUnidadMedida1DetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(22).Text).ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("IdUnidMedida1")
            '    'DetDocumento.nCantidad2DetalleOfertaVenta = grdDetalle.Rows(i).Cells(23).Text 'Session("CestaComprobante").Rows(i)("Cantidad2")
            '    'DetDocumento.cIdUnidadMedida2DetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(24).Text).ToString, "&nbsp;", ""))  'Session("CestaComprobante").Rows(i)("IdUnidMedida2")
            '    'DetDocumento.nTamanoPesoProductoDetalleOfertaVenta = grdDetalle.Rows(i).Cells(20).Text 'Session("CestaComprobante").Rows(i)("TamanoPeso")
            '    'DetDocumento.nValorUnitarioTotalVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(28).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("ValorVentaUnitarioTotal")), 10)
            '    'DetDocumento.nPrecioOrigenVentaDetalleOfertaVenta = grdDetalle.Rows(i).Cells(33).Text 'grdDetalle.Rows(i).Cells(8).Text 'Session("CestaComprobante").Rows(i)("PrecioOrigen")
            '    'DetDocumento.nPrecioUnitarioVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(34).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(9).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PrecioUnitario")), 10)
            '    'DetDocumento.nPrecioUnitarioTotalVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(35).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(10).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PrecioUnitarioTotal")), 10)
            '    'DetDocumento.nPrecioUnitarioSugeridoDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(29).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PrecioUnitarioSugerido")), 10)
            '    'DetDocumento.nDescuentoVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(39).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(14).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("DescuentoVenta")), 10)
            '    'DetDocumento.nValorVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(36).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(11).Text), 10) - DetDocumento.nDescuentoVentaDetalleDocumento 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(11).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("ValorVenta")), 10) 'Convert.ToDecimal(Math.Round((Session("CestaComprobante").Rows(i)("Precio")) / ((Session("nImpuesto") / 100) + 1), 2)) 'Double
            '    'DetDocumento.nIGVDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(38).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(13).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("IGV")), 10) 'Convert.ToDecimal(Math.Round(Session("CestaComprobante").Rows(i)("Total")) - Math.Round((Session("CestaComprobante").Rows(i)("Precio")) / ((Session("nImpuesto") / 100) + 1), 2))
            '    'DetDocumento.nTotalPrecioVentaDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(41).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(16).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("Total")), 10) 'Convert.ToDecimal(Math.Round(Session("CestaComprobante").Rows(i)("Total"), 2))
            '    'DetDocumento.cIdTipoAfectacion = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(27).Text).ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("IdTipoAfectacion")
            '    'DetDocumento.cIdOnerosidadDetalleOfertaVenta = Convert.ToChar(Server.HtmlDecode(grdDetalle.Rows(i).Cells(30).Text)) 'Convert.ToChar(Session("CestaComprobante").Rows(i)("IdOnerosidad"))
            '    'DetDocumento.nISCDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(37).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(12).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("ISC")), 10)
            '    'DetDocumento.cIdSistemaISCDetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(31).Text).ToString, "&nbsp;", "")) 'Session("CestaComprobante").Rows(i)("IdSistemaISC")
            '    ''DetDocumento.nPorcentajePercepcionDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(32).Text), 2) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("PorcentajePercepcion")), 2)
            '    ''DetDocumento.nPercepcionDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(40).Text), 10) 'Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(15).Text), 10) 'Math.Round(Convert.ToDecimal(Session("CestaComprobante").Rows(i)("Percepcion")), 10)
            '    'DetDocumento.nPorcentajeISCDetalleOfertaVenta = Math.Round(Convert.ToDecimal(grdDetalle.Rows(i).Cells(42).Text), 10)
            '    'DetDocumento.cIdUnidadMedidaOrigenDetalleOfertaVenta = Server.HtmlDecode(grdDetalle.Rows(i).Cells(43).Text) '"IdUnidadMedidaOrigen"
            '    'DetDocumento.vObservacionDetalleOfertaVenta = Server.HtmlDecode(grdDetalle.Rows(i).Cells(44).Text).ToUpper.Trim
            '    'DetDocumento.cIdTipoExistenciaDetalleOfertaVenta = Server.HtmlDecode(Replace((grdDetalle.Rows(i).Cells(45).Text).ToString, "&nbsp;", ""))  'Session("CestaComprobante").Rows(i)("IdUnidMedida2")
            '    ColeccionCatCar.Add(DetDocumento)
            'Next

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

            Dim GaleriaEquipoNeg As New clsGaleriaEquipoNegocios
            If GaleriaEquipoNeg.GaleriaEquipoInserta(GaleriaEquipo) = 0 Then
                'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
                Dim FuncionesNeg As New clsFuncionesNegocios
                FuncionesNeg.GuardarArchivo(fupSubirImagenEquipo.PostedFile, True, "Imagenes\Equipo", Trim(UCase(GaleriaEquipo.cIdEquipo & "-" & GaleriaEquipo.nIdNumeroItemGaleriaEquipo.ToString)), True, "", 1200, 160)
                'imgProducto.ImageUrl = "~/Imagenes/Equipo/" & Trim(UCase(txtIdProducto.Text)) & ".jpg"
                imgEquipo.ImageUrl = "~/Imagenes/Equipo/" & Trim(UCase(GaleriaEquipo.cIdEquipo & "-" & GaleriaEquipo.nIdNumeroItemGaleriaEquipo.ToString)) & ".jpg"

                Session("Query") = "PA_LOGI_MNT_GALERIAEQUIPO 'SQL_INSERT', '','" & GaleriaEquipo.cIdEquipo & "', " &
                               GaleriaEquipo.nIdNumeroItemGaleriaEquipo & ", '" & GaleriaEquipo.vDescripcionGaleriaEquipo & "', '" & GaleriaEquipo.vObservacionGaleriaEquipo & "', '" &
                               GaleriaEquipo.vTituloGaleriaEquipo & "', '" & GaleriaEquipo.bEstadoRegistroGaleriaEquipo & "', '" &
                               GaleriaEquipo.dFechaTransaccionGaleriaEquipo & "', '" & GaleriaEquipo.cIdEquipo & "'"

                LogAuditoria.vEvento = "INSERTAR GALERIA EQUIPO"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                MyValidator.ErrorMessage = "Transacción registrada con éxito"

                BloquearMantenimiento(True, False, True, False)
            End If


            'If hfdOperacionDetalle.Value = "N" Then
            '    If (Ga.CatalogoInserta(Galeria)) = 0 Then
            '        'If (CaracteristicaNeg.CaracteristicaCatalogoInsertaDetalle(Catalogo, ColeccionCatCar, LogAuditoria)) Then
            '        Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_INSERT', '','" & Catalogo.cIdCatalogo & "', '" &
            '                                       Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
            '                                       Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
            '                                       Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
            '                                       Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" & Catalogo.cIdCatalogo & "'"

            '        LogAuditoria.vEvento = "INSERTAR CATALOGO"
            '        LogAuditoria.vQuery = Session("Query")
            '        LogAuditoria.cIdSistema = Session("IdSistema")
            '        LogAuditoria.cIdModulo = strOpcionModulo

            '        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

            '        txtIdCatalogoMantenimientoCatalogo.Text = Catalogo.cIdCatalogo

            '        If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
            '        End If

            '        MyValidator.ErrorMessage = "Transacción registrada con éxito"

            '        'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
            '        'Me.grdLista.DataBind()
            '        'pnlGeneral.Enabled = False
            '        BloquearMantenimiento(True, False, True, False)
            '        'hfTab.Value = "tab1"
            '    Else
            '        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
            '    End If
            'ElseIf hfdOperacionDetalle.Value = "E" Then
            '    If (CatalogoNeg.CatalogoEdita(Catalogo)) = 0 Then
            '        Session("Query") = "PA_LOGI_MNT_CATALOGO 'SQL_UPDATE', '','" & Catalogo.cIdCatalogo & "', '" &
            '                                   Catalogo.cIdJerarquiaCatalogo & "', '" & Catalogo.cIdSistemaFuncional & "', '" & Catalogo.cIdEnlaceCatalogo & "', '" &
            '                                   Catalogo.vDescripcionCatalogo & "', '" & Catalogo.vDescripcionAbreviadaCatalogo & "', '" &
            '                                   Catalogo.bEstadoRegistroCatalogo & "', '" & Catalogo.cIdCuentaContableCatalogo & "', '" & Catalogo.cIdCuentaContableLeasingCatalogo & "', " &
            '                                   Catalogo.nVidaUtilCatalogo & ", '" & Catalogo.cIdTipoActivo & "', " & Catalogo.nPeriodoGarantiaCatalogo & ", " & Catalogo.nPeriodoMinimoMantenimientoCatalogo & ", '" & Catalogo.cIdCatalogo & "'"

            '        LogAuditoria.vEvento = "ACTUALIZAR CATALOGO"
            '        LogAuditoria.vQuery = Session("Query")
            '        LogAuditoria.cIdSistema = Session("IdSistema")
            '        LogAuditoria.cIdModulo = strOpcionModulo

            '        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

            '        If (CaracteristicaNeg.CaracteristicaCatalogoInserta(Catalogo, ColeccionCatCar, LogAuditoria)) Then
            '        End If
            '        MyValidator.ErrorMessage = "Registro actualizado con éxito"
            '        'Me.grdLista.DataSource = CatalogoNeg.CatalogoListaBusqueda(cboFiltro.SelectedValue, txtBuscar.Text, "0", "*")
            '        'Me.grdLista.DataBind()
            '        'pnlGeneral.Enabled = False
            '        BloquearMantenimiento(True, False, True, False)
            '        'hfTab.Value = "tab1"
            '    Else
            '        Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
            '        'Exit Sub
            '    End If
            'End If
            'hfdOperacionDetalle.Value = "R"
            'ListarCatalogoCombo()
            ''BloquearPagina(0)
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
            ValidationSummary5.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenEquipo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirImagenEquipo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Left 'Codigo
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'RucCliente
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'RazonSocialCliente
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'FechaRegistroTarjetaSAP
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'FechaUltimaModificacion
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'IdTipoActivo
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'Descripcion
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'Estado Equipo
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'Estado Registro
            Next
        End If
    End Sub

    Private Sub grdCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCatalogoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdCatalogoComponente, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
    End Sub

    Private Sub grdCatalogoComponente_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdCatalogoComponente.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'Descripcion
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'IdTipoActivo
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdSistemaFuncional
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'DescripcionAbreviada
            Next
        End If
    End Sub

    Private Sub grdEquipoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdEquipoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True 'IdCatalogo
            e.Row.Cells(2).Visible = True 'Descripcion
            e.Row.Cells(3).Visible = False 'IdTipoActivo
            e.Row.Cells(4).Visible = False 'IdSistemaFuncional
            e.Row.Cells(5).Visible = True 'DescripcionAbreviada
        End If
    End Sub

    Private Sub grdEquipoComponente_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdEquipoComponente.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'Descripcion
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'IdTipoActivo
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdSistemaFuncional
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'DescripcionAbreviada
            Next
        End If
    End Sub

    Private Sub lnkbtnVerOrdenFabricacion_Click(sender As Object, e As EventArgs) Handles lnkbtnVerOrdenFabricacion.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            'If MyValidator.ErrorMessage = "" Then
                            '    Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
                            'End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para mostrar su orden de fabricación.")
                    End If
                Else
                    Throw New Exception("Seleccione un equipo.")
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnImprimirTarjetaEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnImprimirTarjetaEquipo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            'If MyValidator.ErrorMessage = "" Then
                            '    Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
                            'End If
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para ver la tarjeta de equipo.")
                    End If
                Else
                    Throw New Exception("Seleccione un equipo.")
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleCaracteristicaCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaCatalogoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub grdDetalleCaracteristicaEquipo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaEquipo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub grdDetalleCaracteristicaEquipoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaEquipoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub grdDetalleCaracteristicaMantenimientoCatalogo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaMantenimientoCatalogo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = True 'IdCaracteristica
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = True 'Valor
            e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
            e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
        End If
    End Sub

    Private Sub lnkbtnEliminarEquipoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEliminarEquipoComponente.Click
        Try
            If grdEquipoComponente IsNot Nothing Then
                If grdEquipoComponente.Rows.Count > 0 Then
                    If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                        If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
                            'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
                            'hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
                            lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Seleccione un equipo para ver la tarjeta de equipo.")
                    End If
                Else
                    Throw New Exception("Seleccione un equipo.")
                End If
            End If
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Private Sub btnSiMensajeDocumentoValidacion_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumentoValidacion.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0646", strOpcionModulo, Session("IdSistema"))

    '        'JMUG: 27/04/2023 EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")
    '        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")

    '        'CargarCestaCatalogoComponente()
    '        'CargarCestaEquipoComponente()
    '        'CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 20/03/2023
    '        'CargarCestaCaracteristicaEquipoComponente()
    '        'CargarCestaCatalogoComponente() 'JMUG: 27/04/2023
    '        'CargarCestaEquipoComponente() 'JMUG: 27/04/2023

    '        ''Función para validar si tiene permisos
    '        'MyValidator.ErrorMessage = ""
    '        '    fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

    '        If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
    '            Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
    '        Else
    '            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
    '        End If

    '        If IsNothing(grdEquipoComponente.SelectedRow) = False Then
    '            'clsLogiCestaCatalogo.AgregarCesta("", Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
    '            '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
    '            '                               "1", Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(4).Text),
    '            '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim,
    '            '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(2).Text),
    '            '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(5).Text), Now,
    '            '                               "1", txtIdEquipo.Text.Trim, "", "1", "", "", "1", "1",
    '            '                               "", "", "01",
    '            '                               Session("CestaCatalogoComponente"))
    '            'clsLogiCestaCatalogo.AgregarCesta(Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdTipoActivo").ToString.Trim,
    '            '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdJerarquia").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdSistemaFuncional").ToString.Trim,
    '            '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdEnlaceCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Descripcion").ToString.Trim,
    '            '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescripcionAbreviada").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Estado").ToString.Trim,
    '            '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("VidaUtil").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCuentaContable").ToString.Trim,
    '            '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCuentaContableLeasing").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescrAbrevTipoActivo").ToString.Trim,
    '            '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescrAbrevSistemaFuncional").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoGarantia").ToString.Trim,
    '            '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoMinimoMatenimiento").ToString.Trim,
    '            '                               Session("CestaCatalogoComponente"))
    '            clsLogiCestaCatalogo.AgregarCesta(Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdTipoActivo").ToString.Trim,
    '                                              Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdJerarquia").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdSistemaFuncional").ToString.Trim,
    '                                              Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdEnlaceCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Descripcion").ToString.Trim,
    '                                              Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescripcionAbreviada").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Estado").ToString.Trim,
    '                                              Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("VidaUtil").ToString.Trim, "",
    '                                              "", Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescAbrevTipoActivo").ToString.Trim,
    '                                              Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescAbrevSistemaFuncional").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoGarantia").ToString.Trim,
    '                                              Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoMinimo").ToString.Trim,
    '                                           Session("CestaCatalogoComponente"))
    '            clsLogiCestaEquipo.QuitarCesta(grdEquipoComponente.SelectedIndex, Session("CestaEquipoComponente"))
    '        End If

    '        'Dim result As DataRow() = Session("CestaCatalogoComponente").Select("IdCatalogo ='" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text) & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text).Trim & "'")
    '        'rowIndexDetalle = Session("CestaCatalogoComponente").Rows.IndexOf(result(0))

    '        rowIndexDetalle = 0
    '        CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

    '        grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
    '            grdEquipoComponente.DataBind()

    '            grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
    '            grdCatalogoComponente.DataBind()

    '        'ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        'MyValidator.IsValid = False
    '        'MyValidator.ID = "ErrorPersonalizado"
    '        'MyValidator.ValidationGroup = "vgrpValidar"
    '        'Me.Page.Validators.Add(MyValidator)
    '        'Catch ex As Exception
    '        '    ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        '    MyValidator.ErrorMessage = ex.Message
    '        '    MyValidator.IsValid = False
    '        '    MyValidator.ID = "ErrorPersonalizado"
    '        '    MyValidator.ValidationGroup = "vgrpValidar"
    '        '    Me.Page.Validators.Add(MyValidator)
    '        'End Try

    '        If MyValidator.ErrorMessage = "" Then
    '            MyValidator.ErrorMessage = "Transacción eliminada con éxito"
    '        End If

    '        'Dim ChkList As New LOGI_EQUIPO
    '        'ChkList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
    '        'ChkList.cIdNumeroCabeceraCheckListPlantilla = ""
    '        'ChkList.cIdCatalogoCabeceraCheckListPlantilla = cboCatalogo.SelectedValue
    '        'ChkList.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = "0"
    '        'ChkList.cIdTipoActivoCabeceraCheckListPlantilla = cboTipoActivo.SelectedValue
    '        'ChkList.dFechaTransaccionCabeceraCheckListPlantilla = Now
    '        'ChkList.bEstadoRegistroCabeceraCheckListPlantilla = True

    '        'Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '        'LogAuditoria.cIdPaisOrigen = Session("IdPais")
    '        'LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '        'LogAuditoria.cIdLocal = Session("IdLocal")
    '        'LogAuditoria.cIdUsuario = Session("IdUsuario")
    '        'LogAuditoria.vIP1 = Session("IP1")
    '        'LogAuditoria.vIP2 = Session("IP2")
    '        'LogAuditoria.vPagina = Session("URL")
    '        'LogAuditoria.vSesion = Session("IdSesion")
    '        'LogAuditoria.cIdSistema = Session("IdSistema")
    '        'LogAuditoria.cIdModulo = strOpcionModulo

    '        'If hfdOperacion.Value = "N" Then
    '        '    If CheckListPlantillaNeg.ChecklistPlantillaInserta(ChkList) = 0 Then
    '        '        Dim CheckListDetallePlantillaNeg As New clsDetalleChecklistPlantillaNegocios
    '        '        Dim Coleccion As New List(Of LOGI_DETALLECHECKLISTPLANTILLA)
    '        '        For i = 0 To Session("CestaActividadCatalogoComponente").Rows.Count - 1
    '        '            Dim CheckList As New LOGI_DETALLECHECKLISTPLANTILLA
    '        '            CheckList.cIdNumeroCabeceraCheckListPlantilla = ChkList.cIdNumeroCabeceraCheckListPlantilla
    '        '            CheckList.cIdActividadCheckList = Session("CestaActividadCatalogoComponente").Rows(i)("Codigo").ToString.Trim
    '        '            CheckList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
    '        '            CheckList.cIdCatalogo = cboCatalogo.SelectedValue
    '        '            CheckList.cIdJerarquiaCatalogo = "1"
    '        '            CheckList.nIdNumeroItemDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponente").Rows(i)("Item").ToString.Trim
    '        '            CheckList.vDescripcionDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponente").Rows(i)("Descripcion").ToString.Trim
    '        '            CheckList.bEstadoRegistroDetalleCheckListPlantilla = 1
    '        '            CheckList.cIdTipoActivo = cboTipoActivo.SelectedValue
    '        '            Coleccion.Add(CheckList)
    '        '        Next

    '        '        If CheckListDetallePlantillaNeg.DetalleChecklistPlantillaInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
    '        '            MyValidator.ErrorMessage = "Transacción registrada con éxito"
    '        '            Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, 0)
    '        '            Me.grdLista.DataBind()

    '        '            pnlCabecera.Visible = True
    '        '            pnlEquipo.Visible = False
    '        '            pnlComponentes.Visible = False

    '        '            BloquearMantenimiento(True, False, True, False)
    '        '            hfdOperacion.Value = "R"
    '        '            txtBuscarCheckList.Focus()
    '        '        End If
    '        '    End If
    '        'End If
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    Private Sub btnSiMensajeDocumentoValidacion_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumentoValidacion.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0646", strOpcionModulo, Session("IdSistema"))

            'JMUG: 27/04/2023 EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")
            EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")

            'CargarCestaCatalogoComponente()
            'CargarCestaEquipoComponente()
            'CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 20/03/2023
            'CargarCestaCaracteristicaEquipoComponente()
            'CargarCestaCatalogoComponente() 'JMUG: 27/04/2023
            'CargarCestaEquipoComponente() 'JMUG: 27/04/2023

            ''Función para validar si tiene permisos
            'MyValidator.ErrorMessage = ""
            '    fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If

            If IsNothing(grdEquipoComponente.SelectedRow) = False Then
                'clsLogiCestaCatalogo.AgregarCesta("", Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text),
                '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text),
                '                               "1", Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(4).Text),
                '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text).Trim,
                '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(2).Text),
                '                               Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(5).Text), Now,
                '                               "1", txtIdEquipo.Text.Trim, "", "1", "", "", "1", "1",
                '                               "", "", "01",
                '                               Session("CestaCatalogoComponente"))
                'clsLogiCestaCatalogo.AgregarCesta(Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdTipoActivo").ToString.Trim,
                '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdJerarquia").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdSistemaFuncional").ToString.Trim,
                '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdEnlaceCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Descripcion").ToString.Trim,
                '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescripcionAbreviada").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Estado").ToString.Trim,
                '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("VidaUtil").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCuentaContable").ToString.Trim,
                '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCuentaContableLeasing").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescrAbrevTipoActivo").ToString.Trim,
                '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescrAbrevSistemaFuncional").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoGarantia").ToString.Trim,
                '                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoMinimoMatenimiento").ToString.Trim,
                '                               Session("CestaCatalogoComponente"))
                clsLogiCestaCatalogo.AgregarCesta(Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdTipoActivo").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdJerarquia").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdSistemaFuncional").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdEnlaceCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Descripcion").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescripcionAbreviada").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Estado").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("VidaUtil").ToString.Trim, "",
                                                  "", Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescAbrevTipoActivo").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescAbrevSistemaFuncional").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoGarantia").ToString.Trim,
                                                  Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoMinimo").ToString.Trim,
                                               Session("CestaCatalogoComponente"))
                clsLogiCestaEquipo.QuitarCesta(grdEquipoComponente.SelectedIndex, Session("CestaEquipoComponente"))
            End If

            'Dim result As DataRow() = Session("CestaCatalogoComponente").Select("IdCatalogo ='" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(1).Text) & "' AND IdTipoActivo = '" & Server.HtmlDecode(grdEquipoComponente.SelectedRow.Cells(3).Text).Trim & "'")
            'rowIndexDetalle = Session("CestaCatalogoComponente").Rows.IndexOf(result(0))

            rowIndexDetalle = 0
            CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

            grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            grdEquipoComponente.DataBind()

            grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            grdCatalogoComponente.DataBind()

            'ValidationSummary1.ValidationGroup = "vgrpValidar"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidar"
            'Me.Page.Validators.Add(MyValidator)
            'Catch ex As Exception
            '    ValidationSummary1.ValidationGroup = "vgrpValidar"
            '    MyValidator.ErrorMessage = ex.Message
            '    MyValidator.IsValid = False
            '    MyValidator.ID = "ErrorPersonalizado"
            '    MyValidator.ValidationGroup = "vgrpValidar"
            '    Me.Page.Validators.Add(MyValidator)
            'End Try

            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Transacción eliminada con éxito"
            End If

            'Dim ChkList As New LOGI_EQUIPO
            'ChkList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
            'ChkList.cIdNumeroCabeceraCheckListPlantilla = ""
            'ChkList.cIdCatalogoCabeceraCheckListPlantilla = cboCatalogo.SelectedValue
            'ChkList.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = "0"
            'ChkList.cIdTipoActivoCabeceraCheckListPlantilla = cboTipoActivo.SelectedValue
            'ChkList.dFechaTransaccionCabeceraCheckListPlantilla = Now
            'ChkList.bEstadoRegistroCabeceraCheckListPlantilla = True

            'Dim LogAuditoria As New GNRL_LOGAUDITORIA
            'LogAuditoria.cIdPaisOrigen = Session("IdPais")
            'LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            'LogAuditoria.cIdLocal = Session("IdLocal")
            'LogAuditoria.cIdUsuario = Session("IdUsuario")
            'LogAuditoria.vIP1 = Session("IP1")
            'LogAuditoria.vIP2 = Session("IP2")
            'LogAuditoria.vPagina = Session("URL")
            'LogAuditoria.vSesion = Session("IdSesion")
            'LogAuditoria.cIdSistema = Session("IdSistema")
            'LogAuditoria.cIdModulo = strOpcionModulo

            'If hfdOperacion.Value = "N" Then
            '    If CheckListPlantillaNeg.ChecklistPlantillaInserta(ChkList) = 0 Then
            '        Dim CheckListDetallePlantillaNeg As New clsDetalleChecklistPlantillaNegocios
            '        Dim Coleccion As New List(Of LOGI_DETALLECHECKLISTPLANTILLA)
            '        For i = 0 To Session("CestaActividadCatalogoComponente").Rows.Count - 1
            '            Dim CheckList As New LOGI_DETALLECHECKLISTPLANTILLA
            '            CheckList.cIdNumeroCabeceraCheckListPlantilla = ChkList.cIdNumeroCabeceraCheckListPlantilla
            '            CheckList.cIdActividadCheckList = Session("CestaActividadCatalogoComponente").Rows(i)("Codigo").ToString.Trim
            '            CheckList.cIdTipoMantenimiento = cboTipoMantenimiento.SelectedValue
            '            CheckList.cIdCatalogo = cboCatalogo.SelectedValue
            '            CheckList.cIdJerarquiaCatalogo = "1"
            '            CheckList.nIdNumeroItemDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponente").Rows(i)("Item").ToString.Trim
            '            CheckList.vDescripcionDetalleCheckListPlantilla = Session("CestaActividadCatalogoComponente").Rows(i)("Descripcion").ToString.Trim
            '            CheckList.bEstadoRegistroDetalleCheckListPlantilla = 1
            '            CheckList.cIdTipoActivo = cboTipoActivo.SelectedValue
            '            Coleccion.Add(CheckList)
            '        Next

            '        If CheckListDetallePlantillaNeg.DetalleChecklistPlantillaInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
            '            MyValidator.ErrorMessage = "Transacción registrada con éxito"
            '            Me.grdLista.DataSource = CheckListPlantillaNeg.ChecklistPlantillaListaBusqueda(cboFiltroCheckList.SelectedValue, txtBuscarCheckList.Text, 0)
            '            Me.grdLista.DataBind()

            '            pnlCabecera.Visible = True
            '            pnlEquipo.Visible = False
            '            pnlComponentes.Visible = False

            '            BloquearMantenimiento(True, False, True, False)
            '            hfdOperacion.Value = "R"
            '            txtBuscarCheckList.Focus()
            '        End If
            '    End If
            'End If
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

    Private Sub grdDetalleCaracteristicaMantenimientoCatalogo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaMantenimientoCatalogo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "489", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            For Each row As GridViewRow In grdDetalleCaracteristicaMantenimientoCatalogo.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
                            If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCatalogoCaracteristica"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
            Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
            lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdListaCaracteristica_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdListaCaracteristica.PageIndexChanging
        grdListaCaracteristica.PageIndex = e.NewPageIndex
        Me.grdListaCaracteristica.DataSource = CaracteristicaNeg.CaracteristicaListaBusqueda(cboFiltroCaracteristica.SelectedValue, txtBuscarCaracteristica.Text.Trim, "*")
        Me.grdListaCaracteristica.DataBind() 'Recargo el grid.
        grdListaCaracteristica.SelectedIndex = -1
        lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
    End Sub

    Private Sub frmLogiGenerarEquipo_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            'Función para validar si tiene permisos
            '04/09/2023 MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0652", strOpcionModulo, "CMMS")

            If Request.QueryString("IdEquipo") <> "" Then
                txtBuscarEquipo.Text = Request.QueryString("IdEquipo")
                cboFiltroEquipo.SelectedIndex = 0
                Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
                Me.grdLista.DataBind()

                grdLista.SelectedIndex = 0
            End If
            '04/09/2023 ValidationSummary1.ValidationGroup = "vgrpValidar"
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleCaracteristicaEquipo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleCaracteristicaEquipo.PageIndexChanging
        grdDetalleCaracteristicaEquipo.PageIndex = e.NewPageIndex
        Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
        Me.grdDetalleCaracteristicaEquipo.DataBind() 'Recargo el grid.
        grdDetalleCaracteristicaEquipo.SelectedIndex = -1

    End Sub

    Private Sub cboDescripcionCatalogoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDescripcionCatalogoComponente.SelectedIndexChanged
        txtDescripcionCatalogoComponente.Text = cboDescripcionCatalogoComponente.SelectedItem.Value
        If cboDescripcionCatalogoComponente.SelectedItem.Value = "" Then
            txtDescripcionCatalogoComponente.Text = ""
        End If
        lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
    End Sub

    Sub ConcatenadorDescripcionEquipo()
        txtDescripcionEquipo.Text = IIf(cboTipoEquipo.SelectedItem.Text.Trim = "SELECCIONE DATO", "", cboTipoEquipo.SelectedItem.Text.Trim + "|") + UCase(txtCapacidadEquipo.Text.Trim) + "|" + UCase(txtNroSerieEquipo.Text.Trim) + "|" + UCase(txtTagEquipo.Text.Trim)
        txtDescripcionEquipo.Text = txtDescripcionEquipo.Text.Replace("||", "|")
        txtDescripcionEquipo.Text = IIf(InStrRev(txtDescripcionEquipo.Text.Trim, "|") = Len(txtDescripcionEquipo.Text.Trim), Mid(txtDescripcionEquipo.Text.Trim, 1, Len(txtDescripcionEquipo.Text.Trim) - 1), txtDescripcionEquipo.Text.Trim)
        txtDescripcionEquipo.Text = IIf(InStr(txtDescripcionEquipo.Text.Trim, "|") = 1, Mid(txtDescripcionEquipo.Text.Trim, 2, Len(txtDescripcionEquipo.Text.Trim) - 1), txtDescripcionEquipo.Text.Trim)
    End Sub

    Private Sub cboTipoEquipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoEquipo.SelectedIndexChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtNroSerieEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtNroSerieEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtNroParteEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtNroParteEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtTagEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtTagEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub

    Private Sub txtCapacidadEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtCapacidadEquipo.TextChanged
        ConcatenadorDescripcionEquipo()
    End Sub
End Class