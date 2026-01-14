Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarEquipoDesplazamiento
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
            Throw New Exception("Su sesión ha caducado, ingrese de nuevo por favor.")
        End If
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
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
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
        clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoComponente")) 'Lo Cambie por el de arriba: 28/03/2023

        Dim CatalogoNeg As New clsCatalogoNegocios
        Dim Coleccion = CatalogoNeg.CatalogoListaBusqueda("CAT.cIdTipoActivo = '" & cboTipoActivo.SelectedValue & "' AND CAT.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
        Dim intContador As Integer = 0

        For Each Registro In Coleccion
            Dim booExiste As Boolean = False
            If booExiste = False Then
                clsLogiCestaCatalogo.AgregarCesta(Registro.Codigo, Registro.IdTipoActivo, "1", Registro.IdSistemaFuncional,
                                                  cboCatalogo.SelectedValue, UCase(Registro.Descripcion),
                                                  UCase(Registro.DescripcionAbreviada), Registro.Estado, 0, Registro.IdCuentaContable, Registro.IdCuentaContableLeasing, Registro.DescripcionTipoActivo,
                                                  Registro.DescripcionSistemaFuncional, Registro.PeriodoGarantia, Registro.PeriodoMinimoMantenimiento,
                                                  Session("CestaCatalogoComponente"))
            End If
        Next
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
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaCaracteristicaCatalogoComponente()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaCatalogoComponente"))
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
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

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
            If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
                Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            End If
            If Session("CestaEquipoComponente").Rows.Count > 0 Then
                Dim dsCaracteristicaEquipoComponente = CaracteristicaNeg.CaracteristicaGetData("SELECT EQUCAR.cIdEquipo, EQU.cIdCatalogo, '1' AS cIdJerarquiaEquipo, EQUCAR.cIdCaracteristica, CAR.vDescripcionCaracteristica, EQUCAR.nIdNumeroItemEquipoCaracteristica, EQUCAR.cIdReferenciaSAPEquipoCaracteristica, EQUCAR.vDescripcionCampoSAPEquipoCaracteristica, EQUCAR.vValorReferencialEquipoCaracteristica " &
                                                                           "FROM LOGI_EQUIPOCARACTERISTICA AS EQUCAR INNER JOIN LOGI_CARACTERISTICA AS CAR ON EQUCAR.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                           "     INNER JOIN LOGI_EQUIPO AS EQU ON EQUCAR.cIdEquipo = EQU.cIdEquipo AND EQUCAR.cIdEmpresa = EQU.cIdEmpresa " &
                                                                           "WHERE EQUCAR.cIdEquipo = '" & Session("CestaEquipoComponente").Rows(rowIndexDetalle)("IdEquipo").ToString.Trim & "' AND EQU.bEstadoRegistroEquipo = '1' ORDER BY EQUCAR.nIdNumeroItemEquipoCaracteristica")
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

    Sub CargarCestaEquipoComponente()
        'Carga las opciones que aun no estan asignadas en la Grilla de Opciones del Módulo.
        Try
            VaciarCesta(Session("CestaEquipoComponente"))
            Dim EquipoActivoNeg As New clsEquipoNegocios
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
        cboFiltroEquipo.DataSource = FiltroNeg.TablaSistemaListarCombo("83", "LOGI", Session("IdEmpresa"))
        cboFiltroEquipo.Items.Clear()
        cboFiltroEquipo.DataBind()
    End Sub

    Sub ListarTipoActivoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoActivoNeg As New clsTipoActivoNegocios
        cboTipoActivo.DataTextField = "vDescripcionTipoActivo"
        cboTipoActivo.DataValueField = "cIdTipoActivo"
        cboTipoActivo.DataSource = TipoActivoNeg.TipoActivoListarCombo("1")
        cboTipoActivo.Items.Clear()
        cboTipoActivo.Items.Add("SELECCIONE DATO")
        cboTipoActivo.DataBind()
    End Sub


    Sub ListarTipoEquipoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoEquipoNeg As New clsTipoEquipoNegocios
        cboTipoEquipo.DataTextField = "vDescripcionTipoEquipo"
        cboTipoEquipo.DataValueField = "cIdTipoEquipo"
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
        cboCatalogo.DataSource = CatalogoNeg.CatalogoListarCombo(0, cboTipoActivo.SelectedValue, "", "1") 'JMUG: 18/09/2023
        cboCatalogo.Items.Clear()
        cboCatalogo.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboCatalogo.DataBind()
    End Sub

    Sub ListarSistemaFuncionalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        'cboSistemaFuncional.DataTextField = "vDescripcionSistemaFuncional"
        'cboSistemaFuncional.DataValueField = "cIdSistemaFuncional"
        'cboSistemaFuncional.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        'cboSistemaFuncional.Items.Clear()
        'cboSistemaFuncional.DataBind()
    End Sub

    Sub ListarSistemaFuncionalCatalogoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        'Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        'cboSistemaFuncionalCatalogoComponente.DataTextField = "vDescripcionSistemaFuncional"
        'cboSistemaFuncionalCatalogoComponente.DataValueField = "cIdSistemaFuncional"
        'cboSistemaFuncionalCatalogoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        'cboSistemaFuncionalCatalogoComponente.Items.Clear()
        'cboSistemaFuncionalCatalogoComponente.DataBind()
    End Sub

    Sub ListarSistemaFuncionalEquipoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        'Dim SistemaFuncionalNeg As New clsSistemaFuncionalNegocios
        'cboSistemaFuncionalEquipoComponente.DataTextField = "vDescripcionSistemaFuncional"
        'cboSistemaFuncionalEquipoComponente.DataValueField = "cIdSistemaFuncional"
        'cboSistemaFuncionalEquipoComponente.DataSource = SistemaFuncionalNeg.SistemaFuncionalListarCombo()
        'cboSistemaFuncionalEquipoComponente.Items.Clear()
        'cboSistemaFuncionalEquipoComponente.DataBind()
    End Sub

    Sub ListarCaracteristicaEquipoPrincipalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        'cboCaracteristicaEquipo.DataTextField = "vDescripcionCaracteristica"
        'cboCaracteristicaEquipo.DataValueField = "cIdCaracteristica"
        'cboCaracteristicaEquipo.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        'cboCaracteristicaEquipo.Items.Clear()
        'cboCaracteristicaEquipo.DataBind()
    End Sub

    Sub ListarCaracteristicaCatalogoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        'cboCaracteristicaCatalogoComponente.DataTextField = "vDescripcionCaracteristica"
        'cboCaracteristicaCatalogoComponente.DataValueField = "cIdCaracteristica"
        'cboCaracteristicaCatalogoComponente.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        'cboCaracteristicaCatalogoComponente.Items.Clear()
        'cboCaracteristicaCatalogoComponente.DataBind()
    End Sub

    Sub ListarCaracteristicaEquipoComponenteCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        'Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        'cboCaracteristicaEquipoComponente.DataTextField = "vDescripcionCaracteristica"
        'cboCaracteristicaEquipoComponente.DataValueField = "cIdCaracteristica"
        'cboCaracteristicaEquipoComponente.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        'cboCaracteristicaEquipoComponente.Items.Clear()
        'cboCaracteristicaEquipoComponente.DataBind()
    End Sub

    Sub ListarDescripcionCatalogoComponente()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        'Dim CatalogoNeg As New clsCatalogoNegocios
        'cboDescripcionCatalogoComponente.DataTextField = "vDescripcionCatalogo"
        'cboDescripcionCatalogoComponente.DataValueField = "cIdCatalogo"
        'cboDescripcionCatalogoComponente.DataSource = CatalogoNeg.CatalogoListarDescripcionCombo()
        'cboDescripcionCatalogoComponente.Items.Clear()
        'cboDescripcionCatalogoComponente.Items.Add(New ListItem("SELECCIONE DATO", ""))
        'cboDescripcionCatalogoComponente.DataBind()
    End Sub

    Private Function BloquearPagina(ByVal NroPagina As Integer) As Boolean
        BloquearPagina = True
        If NroPagina = 1 Then
            pnlListado.Enabled = True
            pnlGeneral.Enabled = False
            txtBuscarEquipo.Focus()
        ElseIf NroPagina = 2 Then
            If hfdOperacion.Value = "R" Or hfdOperacion.Value = "E" Or IsNothing(hfdOperacion.Value) = True Then
                If grdLista.Rows.Count = 0 Then
                Else
                    If IsNothing(grdLista.SelectedRow) = True Then
                        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                        MyValidator.ErrorMessage = "Seleccione un registro a visualizar."
                        MyValidator.IsValid = False
                        MyValidator.ID = "ErrorPersonalizado"
                        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                        Me.Page.Validators.Add(MyValidator)
                        hfdOperacion.Value = "R"
                    Else
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            If hfdOperacion.Value = "R" Then
                                pnlListado.Enabled = True
                                LlenarData()
                            Else
                                pnlListado.Enabled = False
                            End If
                            pnlGeneral.Enabled = False
                            If hfdOperacion.Value = "E" Then pnlGeneral.Enabled = True
                            txtDescripcionEquipo.Focus()
                            BloquearPagina = False
                        End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "N" Then
                pnlListado.Enabled = False
                pnlGeneral.Enabled = True
                txtDescripcionEquipo.Focus()
                BloquearPagina = False
            End If
        ElseIf NroPagina = 0 Then
            If grdLista.Rows.Count = 0 Then
            Else
                If InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 1 Then
                    pnlListado.Enabled = True
                    pnlGeneral.Enabled = False
                    txtBuscarEquipo.Focus()
                    strTabContenedorActivo = 1
                ElseIf InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 0 Then
                    pnlListado.Enabled = True
                    pnlGeneral.Enabled = False
                    txtBuscarEquipo.Focus()
                    strTabContenedorActivo = 0
                Else
                    If InStr(MyValidator.ErrorMessage, "eliminar") > 0 And strTabContenedorActivo = 0 Then
                        pnlListado.Enabled = True
                        pnlGeneral.Enabled = False
                    Else
                    End If
                End If
            End If
            pnlListado.Enabled = True
            pnlGeneral.Enabled = False
        End If
    End Function

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdEquipo.Enabled = False 'bActivar
        txtDescripcionEquipo.Enabled = False
        txtNroSerieEquipo.Enabled = bActivar
        txtNroParteEquipo.Enabled = bActivar
        txtTagEquipo.Enabled = bActivar
        txtCapacidadEquipo.Enabled = bActivar
        cboTipoActivo.Enabled = bActivar
        cboTipoEquipo.Enabled = bActivar
        txtDescripcionEquipoSAP.Enabled = bActivar
        cboCatalogo.Enabled = bActivar
        cboTipoActivo.Enabled = bActivar
    End Sub

    Sub LlenarData()
        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
    End Sub

    Sub LlenarDataTipoActivo()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosTipoActivo()
            Else
                LimpiarObjetosTipoActivo()
                Dim TipoActivoNeg As New clsTipoActivoNegocios
                Dim TipoActivo As LOGI_TIPOACTIVO = TipoActivoNeg.TipoActivoListarPorId(cboTipoActivo.SelectedValue)
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

    Sub LlenarDataCatalogo()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosCatalogo()
            Else
                LimpiarObjetosCatalogo()
                Dim CatalogoNeg As New clsCatalogoNegocios
                Dim Catalogo = CatalogoNeg.CatalogoListarPorId(cboCatalogo.SelectedValue, cboTipoActivo.SelectedValue, "0", "1")
            End If
            CargarCestaCatalogoCaracteristica()
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

        Catch ex As Exception
            MyValidator.ErrorMessage = ex.Message
        End Try
    End Sub

    Sub LlenarDataEquipoComponente()
        Try
            If hfdOperacionDetalle.Value = "N" Then
                LimpiarObjetosEquipoComponente()
                ListarSistemaFuncionalEquipoComponenteCombo()
            Else
                LimpiarObjetosEquipoComponente()
                Dim Equipo As New LOGI_EQUIPO
                If grdLista.Rows.Count > 0 Then
                    If hfdOperacion.Value = "E" Then 'JMUG: 16/03/2023
                        Equipo = EquipoNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, grdLista.SelectedRow.Cells(5).Text)
                    End If
                End If
            End If
            CargarCestaCaracteristicaEquipoComponenteTemporal()
        Catch ex As Exception
            MyValidator.ErrorMessage = ex.Message
        End Try
    End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvDescripcionEquipo.EnableClientScript = bValidar
        Me.rfvCatalogo.EnableClientScript = bValidar
        Me.rfvDescripcionEquipoSAP.EnableClientScript = bValidar
    End Sub

    Sub ValidarTextoSubirImagenEquipo(ByVal bValidar As Boolean)
        'Me.rfvTituloSubirImagenEquipo.EnableClientScript = bValidar
        'Me.rfvDescripcionSubirImagenEquipo.EnableClientScript = bValidar
        'Me.rfvObservacionSubirImagenEquipo.EnableClientScript = bValidar
    End Sub


    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        'Me.btnNuevo.Visible = bNuevo
        'Me.btnEditar.Visible = bEditar
        'Me.btnGuardar.Visible = bGuardar
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""

        cboTipoActivo.SelectedIndex = -1
        cboCatalogo.SelectedIndex = -1
        hfdEstado.Value = "1"

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
        'txtIdTipoActivoMantenimientoTipoActivo.Text = ""
        'txtDescripcionMantenimientoTipoActivo.Text = ""
        'txtDescripcionAbreviadaMantenimientoTipoActivo.Text = ""
    End Sub

    Sub LimpiarObjetosSistemaFuncional()
        'txtIdSistemaFuncionalMantenimientoSistemaFuncional.Text = ""
        'txtDescripcionMantenimientoSistemaFuncional.Text = ""
        'txtDescripcionAbreviadaMantenimientoSistemaFuncional.Text = ""
    End Sub

    Sub LimpiarObjetosCatalogo()
        'txtIdCatalogoMantenimientoCatalogo.Text = ""
        'txtDescripcionMantenimientoCatalogo.Text = ""
    End Sub

    Sub LimpiarObjetosCatalogoComponente()
        MyValidator.ErrorMessage = ""
        'txtIdCatalogoComponente.Text = ""
        'cboTipoActivoCatalogoComponente.SelectedIndex = -1
        'cboSistemaFuncionalCatalogoComponente.SelectedIndex = -1
    End Sub

    Sub LimpiarObjetosEquipoComponente()
        MyValidator.ErrorMessage = ""
        'txtIdEquipoComponente.Text = ""
        'cboTipoActivoEquipoComponente.SelectedIndex = -1
    End Sub

    Protected Sub Upload_File(sender As Object, e As EventArgs)
        If hfdOperacion.Value = "N" Then
            'GenerarComprobante()
        End If
        'btnNuevo.Enabled = True
    End Sub

    Sub LimpiarObjetosCaracteristicas()
        'Me.lblMensajeCaracteristica.Text = ""
        'txtValorCaracteristica.Text = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
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
            ListarCaracteristicaEquipoPrincipalCombo()
            ListarContratoReferenciaCombo()

            If Session("CestaCatalogoCaracteristica") Is Nothing Then
                Session("CestaCatalogoCaracteristica") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            Else
                clsLogiCestaCatalogo.VaciarCesta(Session("CestaCatalogoCaracteristica"))
            End If

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

            BloquearPagina(1)
            BloquearMantenimiento(True, False, True, False)

            EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")

            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
            Me.grdLista.DataBind()
        Else
            txtBuscarEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcion')")
        End If
    End Sub

    Protected Sub cboTipoActivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivo.SelectedIndexChanged
        ListarCatalogoCombo()
        CargarCestaCatalogoComponente()
        CargarCestaEquipoComponente()
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            BloquearPagina(0)
            ValidarTexto(False)
            LlenarData()
        End If
    End Sub

    Private Sub cboCatalogo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCatalogo.SelectedIndexChanged
        CargarCestaCatalogoComponente()
        CargarCestaEquipoComponente()
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Codigo
            e.Row.Cells(1).Visible = True 'RucCliente/RazonSocialCliente
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Codigo
            e.Row.Cells(1).Visible = True 'RucCliente/RazonSocialCliente
        End If
    End Sub

    Private Sub imgbtnBuscarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarEquipo.Click
        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "'")
        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
        Me.grdLista.DataBind()
    End Sub

    Protected Sub grdCatalogoComponente_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

    Protected Sub grdLista_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim Equipo As New LOGI_EQUIPO
                If Session("IdTipoUsuario") = "U" And FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0641", strOpcionModulo, Session("IdSistema")) Then
                    Throw New Exception("El usuario debe de ser administrador para ejecutar esta opción.")
                End If

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
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

                EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = 0 WHERE cIdEquipo = '" & Equipo.cIdEquipo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
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
        'Dim ContratoNeg As New clsContratoNegocios
        'cboContratoReferencia.DataTextField = "vDescripcionCabeceraContrato"
        'cboContratoReferencia.DataValueField = "vIdNumeroCorrelativoCabeceraContrato"
        'cboContratoReferencia.DataSource = ContratoNeg.ContratoListarCombo("1", "'R','P'")
        'cboContratoReferencia.DataBind()
    End Sub

    'Sub ListarPaisClienteUbicacionCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
    '    cboPaisMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
    '    cboPaisMensajeClienteUbicacion.DataValueField = "cIdPaisUbicacionGeografica"
    '    cboPaisMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.PaisListarCombo
    '    cboPaisMensajeClienteUbicacion.DataBind()
    'End Sub

    'Sub ListarDepartamentoClienteUbicacionCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
    '    cboDepartamentoMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
    '    cboDepartamentoMensajeClienteUbicacion.DataValueField = "cIdDepartamentoUbicacionGeografica"
    '    cboDepartamentoMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.DepartamentoListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue)
    '    cboDepartamentoMensajeClienteUbicacion.Items.Clear()
    '    cboDepartamentoMensajeClienteUbicacion.DataBind()
    'End Sub

    'Sub ListarProvinciaClienteUbicacionCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
    '    cboProvinciaMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
    '    cboProvinciaMensajeClienteUbicacion.DataValueField = "cIdProvinciaUbicacionGeografica"
    '    cboProvinciaMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.ProvinciaListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue)
    '    cboProvinciaMensajeClienteUbicacion.Items.Clear()
    '    cboProvinciaMensajeClienteUbicacion.DataBind()
    'End Sub

    'Sub ListarDistritoClienteUbicacionCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim UbicacionGeograficaNeg As New clsUbicacionGeograficaNegocios
    '    cboDistritoMensajeClienteUbicacion.DataTextField = "vDescripcionUbicacionGeografica"
    '    cboDistritoMensajeClienteUbicacion.DataValueField = "cIdDistritoUbicacionGeografica"
    '    cboDistritoMensajeClienteUbicacion.DataSource = UbicacionGeograficaNeg.DistritoListarCombo(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue, cboProvinciaMensajeClienteUbicacion.SelectedValue)
    '    cboDistritoMensajeClienteUbicacion.Items.Clear()
    '    cboDistritoMensajeClienteUbicacion.DataBind()
    'End Sub

    Sub ListarClienteUbicacionCombo()
        Try
            Dim DirClienteNeg As New clsClienteNegocios
            'cboClienteUbicacion.DataTextField = "vDescripcionClienteUbicacion"
            'cboClienteUbicacion.DataValueField = "cIdUbicacion"
            'cboClienteUbicacion.DataSource = DirClienteNeg.ClienteGetData("SELECT cIdCliente, CLIUBI.cIdUbicacion, UPPER(vDescripcionClienteUbicacion) + ' - ' + " &
            '                                 "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
            '                                 "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + '00' + '00') " &
            '                                 " + ' - ' + " &
            '                                 "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
            '                                 "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + CLIUBI.cIdProvinciaUbicacionGeografica + '00') " &
            '                                 " + ' - ' + " &
            '                                 "(SELECT vDescripcionUbicacionGeografica FROM GNRL_UBICACIONGEOGRAFICA " &
            '                                 "WHERE cIdPaisUbicacionGeografica + cIdDepartamentoUbicacionGeografica + cIdProvinciaUbicacionGeografica + cIdDistritoUbicacionGeografica = CLIUBI.cIdPaisUbicacionGeografica + CLIUBI.cIdDepartamentoUbicacionGeografica + CLIUBI.cIdProvinciaUbicacionGeografica + CLIUBI.cIdDistritoUbicacionGeografica) AS vDescripcionClienteUbicacion " &
            '                                 "FROM GNRL_CLIENTEUBICACION AS CLIUBI WHERE LTRIM(RTRIM(CLIUBI.cIdCliente)) = '" & hfdIdAuxiliar.Value.Trim & "' " &
            '                                 "ORDER BY cIdCliente")
            'cboClienteUbicacion.Items.Clear()
            'cboClienteUbicacion.Items.Add("SELECCIONE DATO")
            'cboClienteUbicacion.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Protected Sub cboPaisMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPaisMensajeClienteUbicacion.SelectedIndexChanged
    '    ListarDepartamentoClienteUbicacionCombo()
    '    ListarProvinciaClienteUbicacionCombo()
    '    ListarDistritoClienteUbicacionCombo()
    '    lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    'End Sub

    'Protected Sub cboDepartamentoMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDepartamentoMensajeClienteUbicacion.SelectedIndexChanged
    '    ListarProvinciaClienteUbicacionCombo()
    '    ListarDistritoClienteUbicacionCombo()
    '    lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    'End Sub

    'Protected Sub cboProvinciaMensajeClienteUbicacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProvinciaMensajeClienteUbicacion.SelectedIndexChanged
    '    ListarDistritoClienteUbicacionCombo()
    '    lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    'End Sub

    'Private Sub btnAdicionarClienteUbicacion_Click(sender As Object, e As EventArgs) Handles btnAdicionarClienteUbicacion.Click
    '    Try
    '        lblMensajeClienteUbicacion.Text = ""
    '        fValidarSesion()
    '        cboPaisMensajeClienteUbicacion.Focus()

    '        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
    '        Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
    '        UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorIdEquivalencia(hfdIdUbicacionGeograficaCliente.Value)

    '        ListarPaisClienteUbicacionCombo()
    '        lblRazonSocialClienteMensajeClienteUbicacion.Text = txtRazonSocial.Text
    '        hfdIdUbicacionGeograficaClienteUbicacion.Value = hfdIdUbicacionGeograficaCliente.Value
    '        cboPaisMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdPaisUbicacionGeografica 'Mid(hfdIdUbicacionGeograficaEntregaCliente.Value, 1, 2) 'cboPaisMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdPaisUbicacionGeografica
    '        cboPaisMensajeClienteUbicacion_SelectedIndexChanged(cboPaisMensajeClienteUbicacion, New System.EventArgs())
    '        cboDepartamentoMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdDepartamentoUbicacionGeografica 'cboDepartamentoMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica
    '        cboDepartamentoMensajeClienteUbicacion_SelectedIndexChanged(cboDepartamentoMensajeClienteUbicacion, New System.EventArgs())
    '        cboProvinciaMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdProvinciaUbicacionGeografica 'cboProvinciaMensajeCliente.SelectedValue  'ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica
    '        cboProvinciaMensajeClienteUbicacion_SelectedIndexChanged(cboProvinciaMensajeClienteUbicacion, New System.EventArgs())
    '        cboDistritoMensajeClienteUbicacion.SelectedValue = UbiGeo.cIdDistritoUbicacionGeografica 'cboDistritoMensajeCliente.SelectedValue 'ClienteRegistradoFinal.cIdDistritoUbicacionGeografica
    '        txtDireccionAdicionalMensajeClienteUbicacion.Text = ""
    '        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    '    Catch ex As Exception
    '        lblMensajeClienteUbicacion.Text = ex.Message
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub btnAceptarMensajeClienteUbicacion_Click(sender As Object, e As EventArgs) Handles btnAceptarMensajeClienteUbicacion.Click
    '    Try
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

    '        MyValidator.ErrorMessage = ""
    '        lblMensajeClienteUbicacion.Text = ""
    '        hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
    '        If txtDireccionAdicionalMensajeClienteUbicacion.Text.Trim = "" Then
    '            Throw New Exception("Debe de ingresar una ubicación, favor de validar la información...!!!")
    '        End If

    '        AdicionarClienteUbicacion(hfdIdAuxiliar.Value)

    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        lblMensajeClienteUbicacion.Text = ex.Message
    '        lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender.Show()
    '    End Try
    'End Sub

    'Sub AdicionarClienteUbicacion(ByVal strIdCliente As String)
    '    fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

    '    MyValidator.ErrorMessage = ""
    '    lblMensajeClienteUbicacion.Text = ""
    '    hfdIdUbicacionGeograficaClienteUbicacion.Value = ""
    '    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
    '    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA

    '    Dim DirClienteNeg As New clsClienteNegocios
    '    Dim dtDirCliente As New DataTable
    '    dtDirCliente = DirClienteNeg.ClienteGetData("SELECT COUNT(*) AS nCantidad FROM GNRL_CLIENTEUBICACION WHERE LTRIM(RTRIM(cIdCliente)) = '" & strIdCliente & "'")

    '    DirClienteNeg.ClienteGetData("INSERT INTO GNRL_CLIENTEUBICACION (cIdCliente, cIdUbicacion, vDescripcionClienteUbicacion, bEstadoRegistroClienteUbicacion, " &
    '                                     "cIdPaisUbicacionGeografica, cIdDepartamentoUbicacionGeografica, cIdProvinciaUbicacionGeografica, cIdDistritoUbicacionGeografica) " &
    '                                     "VALUES ('" & hfdIdAuxiliar.Value & "', (SELECT RIGHT ('00000' + CONVERT (VARCHAR(5), (CONVERT (NUMERIC, ISNULL(MAX(cIdUbicacion), 0)) + 1)), 5) FROM GNRL_CLIENTEUBICACION WHERE cIdCliente = '" & hfdIdAuxiliar.Value & "'), " &
    '                                     "         '" & txtDireccionAdicionalMensajeClienteUbicacion.Text.Trim.ToUpper & "', 1, '" & cboPaisMensajeClienteUbicacion.SelectedValue & "', " &
    '                                     "         '" & cboDepartamentoMensajeClienteUbicacion.SelectedValue & "', '" & cboProvinciaMensajeClienteUbicacion.SelectedValue & "', '" & cboDistritoMensajeClienteUbicacion.SelectedValue & "')")
    '    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(cboPaisMensajeClienteUbicacion.SelectedValue, cboDepartamentoMensajeClienteUbicacion.SelectedValue, cboProvinciaMensajeClienteUbicacion.SelectedValue, cboDistritoMensajeClienteUbicacion.SelectedValue)
    '    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
    '    ListarClienteUbicacionCombo()
    '    cboClienteUbicacion.SelectedIndex = cboClienteUbicacion.Items.Count - 1
    '    lblTituloMensajeClienteUbicacion.Visible = True
    'End Sub

    'Private Sub cboTipoActivoEquipoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoActivoEquipoComponente.SelectedIndexChanged
    '    ListarSistemaFuncionalEquipoComponenteCombo()
    '    lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
    'End Sub

    Sub txtValorDetalle_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            'For Each row As GridViewRow In grdDetalleCaracteristicaEquipo.Rows
            '    Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
            '    Dim FilaActual As Int16
            '    FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipo.Rows.Count * (grdDetalleCaracteristicaEquipo.PageIndex))
            '    Session("CestaCaracteristicaEquipoPrincipal").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            'Next
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
            'For Each row As GridViewRow In grdDetalleCaracteristicaMantenimientoCatalogo.Rows
            '    Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
            '    Dim FilaActual As Int16
            '    FilaActual = row.RowIndex - (grdDetalleCaracteristicaMantenimientoCatalogo.Rows.Count * (grdDetalleCaracteristicaMantenimientoCatalogo.PageIndex))
            '    Session("CestaCatalogoCaracteristica").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            'Next
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
            'For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
            '    Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalleEquipoComponente"), TextBox)
            '    Dim FilaActual As Int16
            '    FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
            '    Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            'Next
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
            'For Each row As GridViewRow In grdDetalleCaracteristicaCatalogoComponente.Rows
            '    Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
            '    Dim FilaActual As Int16
            '    FilaActual = row.RowIndex - (grdDetalleCaracteristicaCatalogoComponente.Rows.Count * (grdDetalleCaracteristicaCatalogoComponente.PageIndex))
            '    Session("CestaCaracteristicaCatalogoComponenteFiltrado").Rows(FilaActual)("Valor") = txtValorDetalle.Text
            'Next
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
            'For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
            '    Dim txtIdReferenciaSAPDetalle As TextBox = TryCast(row.Cells(6).FindControl("txtIdReferenciaSAPDetalleEquipoComponente"), TextBox)
            '    Dim FilaActual As Int16
            '    FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
            '    Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("IdReferenciaSAP") = txtIdReferenciaSAPDetalle.Text
            'Next
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
            'For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
            '    Dim txtDescripcionCampoSAPDetalle As TextBox = TryCast(row.Cells(7).FindControl("txtDescripcionCampoSAPDetalleEquipoComponente"), TextBox)
            '    Dim FilaActual As Int16
            '    FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
            '    Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(FilaActual)("DescripcionCampoSAP") = txtDescripcionCampoSAPDetalle.Text
            'Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Private Sub grdCatalogoComponente_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCatalogoComponente.PageIndexChanging
    '    grdCatalogoComponente.PageIndex = e.NewPageIndex
    '    grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
    '    Me.grdCatalogoComponente.DataBind()
    '    grdCatalogoComponente.SelectedIndex = -1
    'End Sub

    'Private Sub btnAdicionarCaracteristicaEquipo_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaEquipo.Click
    '    'Try
    '    '    'Función para validar si tiene permisos
    '    '    MyValidator.ErrorMessage = ""
    '    '    fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '    '    FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0656", strOpcionModulo, "CMMS")

    '    '    If cboCatalogo.SelectedValue = "SELECCIONE DATO" Then
    '    '        Throw New Exception("Debe de asignarle un catalogo al equipo.")
    '    '    End If

    '    '    For i = 0 To Session("CestaCaracteristicaEquipoPrincipal").Rows.Count - 1
    '    '        If (Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCaracteristica").ToString.Trim) = (cboCaracteristicaEquipo.SelectedValue.ToString.Trim) And Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCatalogo").ToString.Trim = cboCatalogo.SelectedValue.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
    '    '            Throw New Exception("Característica ya registrada, seleccione otro item.")
    '    '            Exit Sub
    '    '        End If
    '    '    Next
    '    '    clsLogiCestaCatalogoCaracteristica.AgregarCesta(cboCatalogo.SelectedValue, "", "'" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'", cboCaracteristicaEquipo.SelectedValue.Trim, UCase(cboCaracteristicaEquipo.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaEquipoPrincipal"))
    '    '    Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
    '    '    Me.grdDetalleCaracteristicaEquipo.DataBind()
    '    '    cboCaracteristicaEquipo.SelectedIndex = -1
    '    '    grdDetalleCaracteristicaEquipo.SelectedIndex = -1
    '    '    MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
    '    '    ValidationSummary1.ValidationGroup = "vgrpValidar"
    '    '    MyValidator.IsValid = False
    '    '    MyValidator.ID = "ErrorPersonalizado"
    '    '    MyValidator.ValidationGroup = "vgrpValidar"
    '    '    Me.Page.Validators.Add(MyValidator)
    '    'Catch ex As Exception
    '    '    ValidationSummary1.ValidationGroup = "vgrpValidar"
    '    '    MyValidator.ErrorMessage = ex.Message
    '    '    MyValidator.IsValid = False
    '    '    MyValidator.ID = "ErrorPersonalizado"
    '    '    MyValidator.ValidationGroup = "vgrpValidar"
    '    '    Me.Page.Validators.Add(MyValidator)
    '    'End Try
    'End Sub

    'Private Sub grdDetalleCaracteristicaEquipo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaEquipo.RowDeleting
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0657", strOpcionModulo, "CMMS")

    '        For Each row As GridViewRow In grdDetalleCaracteristicaEquipo.Rows
    '            If row.RowType = DataControlRowType.DataRow Then
    '                Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
    '                If chkRow.Checked Then
    '                    For i = 0 To Session("CestaCaracteristicaEquipoPrincipal").Rows.Count - 1
    '                        If (Session("CestaCaracteristicaEquipoPrincipal").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
    '                            clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoPrincipal"))
    '                            Exit For
    '                        End If
    '                    Next
    '                End If
    '            End If
    '        Next

    '        'Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
    '        'Me.grdDetalleCaracteristicaEquipo.DataBind()
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub grdDetalleCaracteristicaEquipoComponente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaEquipoComponente.RowDeleting
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0659", strOpcionModulo, "CMMS")

    '        For Each row As GridViewRow In grdDetalleCaracteristicaEquipoComponente.Rows
    '            If row.RowType = DataControlRowType.DataRow Then
    '                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleCaracteristica"), CheckBox)
    '                If chkRow.Checked Then
    '                    Dim FilaActual As Int16
    '                    For i = 0 To Session("CestaCaracteristicaEquipoComponente").Rows.Count - 1
    '                        FilaActual = row.RowIndex - (grdDetalleCaracteristicaEquipoComponente.Rows.Count * (grdDetalleCaracteristicaEquipoComponente.PageIndex))
    '                        If (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim) = hfdIdCatalogoEquipoComponente.Value.Trim And
    '                                (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdJerarquia").ToString.Trim) = "1" And
    '                                (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim And
    '                                (Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim) = txtIdEquipoComponente.Text.Trim Then
    '                            clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoComponente"))
    '                            Exit For
    '                        End If
    '                    Next

    '                    For i = 0 To Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows.Count - 1
    '                        If (Session("CestaCaracteristicaEquipoComponenteFiltrado").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
    '                            clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaEquipoComponenteFiltrado"))
    '                            Exit For
    '                        End If
    '                    Next
    '                End If
    '            End If
    '        Next
    '        Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
    '        Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
    '        lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub lnkbtnVerGaleriaFotosEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnVerGaleriaFotosEquipo.Click
    '    Try
    '        If grdLista IsNot Nothing Then
    '            If grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = False Then
    '                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
    '                        hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
    '                        If MyValidator.ErrorMessage = "" Then
    '                            Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
    '                        End If
    '                    End If
    '                Else
    '                    Throw New Exception("Seleccione un equipo para mostrar su galería de imagenes.")
    '                End If
    '            Else
    '                Throw New Exception("Seleccione un equipo.")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub lnkbtnCargarFotoEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnCargarFotoEquipo.Click
    '    Try
    '        If grdLista IsNot Nothing Then
    '            If grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = False Then
    '                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
    '                        hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
    '                        If MyValidator.ErrorMessage = "" Then
    '                            ValidarTextoSubirImagenEquipo(True)
    '                            txtTituloSubirImagenEquipo.Text = ""
    '                            txtDescripcionSubirImagenEquipo.Text = ""
    '                            txtObservacionSubirImagenEquipo.Text = ""
    '                            pnlCabecera.Visible = True
    '                            'pnlEquipo.Visible = False
    '                            pnlComponentes.Visible = False
    '                            ValidationSummary1.ValidationGroup = "vgrpValidarSubirImagenEquipo"
    '                            lnk_mostrarPanelSubirImagenEquipo_ModalPopupExtender.Show()
    '                        End If
    '                    End If
    '                Else
    '                    Throw New Exception("Seleccione un equipo para mostrar su galería de imagenes.")
    '                End If
    '            Else
    '                Throw New Exception("Seleccione un equipo.")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub btnAceptarSubirImagenEquipo_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirImagenEquipo.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

    '        If Not (fupSubirImagenEquipo.HasFile) Then
    '            Throw New Exception("Seleccione un archivo del disco duro.")
    '        End If

    '        'Se verifica que la extensión sea de un formato válido
    '        'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
    '        Dim ext As String = fupSubirImagenEquipo.PostedFile.FileName 'fileUploader1.PostedFile.FileName
    '        ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

    '        Dim formatos() As String = New String() {"jpg", "jpeg", "bmp", "png", "gif"}
    '        If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de imagen inválido.")
    '        Dim Size As Integer = 0
    '        If Not (Integer.TryParse("160", Size)) Then
    '            Throw New Exception("El tamaño indicado para la imagen no es válido.")
    '        End If

    '        Dim GaleriaEquipo As New LOGI_GALERIAEQUIPO
    '        GaleriaEquipo.cIdEquipo = grdLista.SelectedRow.Cells(0).Text
    '        GaleriaEquipo.nIdNumeroItemGaleriaEquipo = 0
    '        GaleriaEquipo.vTituloGaleriaEquipo = UCase(txtTituloSubirImagenEquipo.Text.Trim)
    '        GaleriaEquipo.vDescripcionGaleriaEquipo = UCase(txtDescripcionSubirImagenEquipo.Text.Trim)
    '        GaleriaEquipo.vObservacionGaleriaEquipo = UCase(txtObservacionSubirImagenEquipo.Text.Trim)
    '        GaleriaEquipo.dFechaTransaccionGaleriaEquipo = Now
    '        GaleriaEquipo.bEstadoRegistroGaleriaEquipo = True

    '        Dim ColeccionGaleria As New List(Of LOGI_GALERIAEQUIPO)

    '        Dim LogAuditoria As New GNRL_LOGAUDITORIA
    '        LogAuditoria.cIdPaisOrigen = Session("IdPais")
    '        LogAuditoria.cIdEmpresa = Session("IdEmpresa")
    '        LogAuditoria.cIdLocal = Session("IdLocal")
    '        LogAuditoria.cIdUsuario = Session("IdUsuario")
    '        LogAuditoria.vIP1 = Session("IP1")
    '        LogAuditoria.vIP2 = Session("IP2")
    '        LogAuditoria.vPagina = Session("URL")
    '        LogAuditoria.vSesion = Session("IdSesion")

    '        Dim GaleriaEquipoNeg As New clsGaleriaEquipoNegocios
    '        If GaleriaEquipoNeg.GaleriaEquipoInserta(GaleriaEquipo) = 0 Then
    '            'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
    '            Dim FuncionesNeg As New clsFuncionesNegocios
    '            FuncionesNeg.GuardarArchivo(fupSubirImagenEquipo.PostedFile, True, "Imagenes\Equipo", Trim(UCase(GaleriaEquipo.cIdEquipo & "-" & GaleriaEquipo.nIdNumeroItemGaleriaEquipo.ToString)), True, "", 1200, 160)
    '            imgEquipo.ImageUrl = "~/Imagenes/Equipo/" & Trim(UCase(GaleriaEquipo.cIdEquipo & "-" & GaleriaEquipo.nIdNumeroItemGaleriaEquipo.ToString)) & ".jpg"

    '            Session("Query") = "PA_LOGI_MNT_GALERIAEQUIPO 'SQL_INSERT', '','" & GaleriaEquipo.cIdEquipo & "', " &
    '                           GaleriaEquipo.nIdNumeroItemGaleriaEquipo & ", '" & GaleriaEquipo.vDescripcionGaleriaEquipo & "', '" & GaleriaEquipo.vObservacionGaleriaEquipo & "', '" &
    '                           GaleriaEquipo.vTituloGaleriaEquipo & "', '" & GaleriaEquipo.bEstadoRegistroGaleriaEquipo & "', '" &
    '                           GaleriaEquipo.dFechaTransaccionGaleriaEquipo & "', '" & GaleriaEquipo.cIdEquipo & "'"

    '            LogAuditoria.vEvento = "INSERTAR GALERIA EQUIPO"
    '            LogAuditoria.vQuery = Session("Query")
    '            LogAuditoria.cIdSistema = Session("IdSistema")
    '            LogAuditoria.cIdModulo = strOpcionModulo

    '            FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

    '            MyValidator.ErrorMessage = "Transacción registrada con éxito"

    '            BloquearMantenimiento(True, False, True, False)
    '        End If
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        ValidationSummary5.ValidationGroup = "vgrpValidarSubirImagenEquipo"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarSubirImagenEquipo"
    '        Me.Page.Validators.Add(MyValidator)
    '        lnk_mostrarPanelSubirImagenEquipo_ModalPopupExtender.Show()
    '    End Try
    'End Sub

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
                'e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'RazonSocialCliente
                'e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'FechaRegistroTarjetaSAP
                'e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'FechaUltimaModificacion
                'e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'IdTipoActivo
                'e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
                'e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                'e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'Descripcion
                'e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'Estado Equipo
                'e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'Estado Registro
            Next
        End If
    End Sub

    'Private Sub grdCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCatalogoComponente.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdCatalogoComponente, "Select$" + e.Row.RowIndex.ToString) & ";")
    '    End If

    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(1).Visible = True 'IdCatalogo
    '        e.Row.Cells(2).Visible = True 'Descripcion
    '        e.Row.Cells(3).Visible = False 'IdTipoActivo
    '        e.Row.Cells(4).Visible = False 'IdSistemaFuncional
    '        e.Row.Cells(5).Visible = True 'DescripcionAbreviada
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(1).Visible = True 'IdCatalogo
    '        e.Row.Cells(2).Visible = True 'Descripcion
    '        e.Row.Cells(3).Visible = False 'IdTipoActivo
    '        e.Row.Cells(4).Visible = False 'IdSistemaFuncional
    '        e.Row.Cells(5).Visible = True 'DescripcionAbreviada
    '    End If
    'End Sub

    'Private Sub grdCatalogoComponente_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdCatalogoComponente.RowCreated
    '    If Session("IdUsuario") = "" Then
    '        Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
    '        Exit Sub
    '    End If

    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim td As TableCell
    '        For Each td In e.Row.Cells
    '            e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
    '            e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'Descripcion
    '            e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'IdTipoActivo
    '            e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdSistemaFuncional
    '            e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'DescripcionAbreviada
    '        Next
    '    End If
    'End Sub

    'Private Sub grdEquipoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdEquipoComponente.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(1).Visible = True 'IdCatalogo
    '        e.Row.Cells(2).Visible = True 'Descripcion
    '        e.Row.Cells(3).Visible = False 'IdTipoActivo
    '        e.Row.Cells(4).Visible = False 'IdSistemaFuncional
    '        e.Row.Cells(5).Visible = True 'DescripcionAbreviada
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(1).Visible = True 'IdCatalogo
    '        e.Row.Cells(2).Visible = True 'Descripcion
    '        e.Row.Cells(3).Visible = False 'IdTipoActivo
    '        e.Row.Cells(4).Visible = False 'IdSistemaFuncional
    '        e.Row.Cells(5).Visible = True 'DescripcionAbreviada
    '    End If
    'End Sub

    'Private Sub grdEquipoComponente_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdEquipoComponente.RowCreated
    '    If Session("IdUsuario") = "" Then
    '        Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
    '        Exit Sub
    '    End If

    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim td As TableCell
    '        For Each td In e.Row.Cells
    '            e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left 'IdCatalogo
    '            e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left 'Descripcion
    '            e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'IdTipoActivo
    '            e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdSistemaFuncional
    '            e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'DescripcionAbreviada
    '        Next
    '    End If
    'End Sub

    'Private Sub lnkbtnVerOrdenFabricacion_Click(sender As Object, e As EventArgs) Handles lnkbtnVerOrdenFabricacion.Click
    '    Try
    '        If grdLista IsNot Nothing Then
    '            If grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = False Then
    '                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
    '                        hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
    '                    End If
    '                Else
    '                    Throw New Exception("Seleccione un equipo para mostrar su orden de fabricación.")
    '                End If
    '            Else
    '                Throw New Exception("Seleccione un equipo.")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub lnkbtnImprimirTarjetaEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnImprimirTarjetaEquipo.Click
    '    Try
    '        If grdLista IsNot Nothing Then
    '            If grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = False Then
    '                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
    '                        hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
    '                    End If
    '                Else
    '                    Throw New Exception("Seleccione un equipo para ver la tarjeta de equipo.")
    '                End If
    '            Else
    '                Throw New Exception("Seleccione un equipo.")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub grdDetalleCaracteristicaCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaCatalogoComponente.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    'End Sub

    'Private Sub grdDetalleCaracteristicaEquipo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaEquipo.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    'End Sub

    'Private Sub grdDetalleCaracteristicaEquipoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaEquipoComponente.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    'End Sub

    'Private Sub grdDetalleCaracteristicaMantenimientoCatalogo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleCaracteristicaMantenimientoCatalogo.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(0).Visible = True 'Seleccionar
    '        e.Row.Cells(1).Visible = True 'chkRowDetalleCaracteristica
    '        e.Row.Cells(2).Visible = True 'Item
    '        e.Row.Cells(3).Visible = True 'IdCaracteristica
    '        e.Row.Cells(4).Visible = True 'Descripcion
    '        e.Row.Cells(5).Visible = True 'Valor
    '        e.Row.Cells(6).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '        e.Row.Cells(7).Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    End If
    'End Sub

    'Private Sub lnkbtnEliminarEquipoComponente_Click(sender As Object, e As EventArgs) Handles lnkbtnEliminarEquipoComponente.Click
    '    Try
    '        If grdEquipoComponente IsNot Nothing Then
    '            If grdEquipoComponente.Rows.Count > 0 Then
    '                If IsNothing(grdEquipoComponente.SelectedRow) = False Then
    '                    If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
    '                        lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender.Show()
    '                    End If
    '                Else
    '                    Throw New Exception("Seleccione un equipo para ver la tarjeta de equipo.")
    '                End If
    '            Else
    '                Throw New Exception("Seleccione un equipo.")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub btnSiMensajeDocumentoValidacion_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumentoValidacion.Click
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

    '        'EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")

    '        If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
    '            Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
    '        Else
    '            clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
    '        End If

    '        If IsNothing(grdEquipoComponente.SelectedRow) = False Then
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

    '        rowIndexDetalle = 0
    '        'CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

    '        grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
    '        grdEquipoComponente.DataBind()

    '        grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
    '        grdCatalogoComponente.DataBind()

    '        If MyValidator.ErrorMessage = "" Then
    '            MyValidator.ErrorMessage = "Transacción eliminada con éxito"
    '        End If

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

    'Private Sub grdDetalleCaracteristicaMantenimientoCatalogo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleCaracteristicaMantenimientoCatalogo.RowDeleting
    '    Try
    '        'Función para validar si tiene permisos
    '        MyValidator.ErrorMessage = ""

    '        For Each row As GridViewRow In grdDetalleCaracteristicaMantenimientoCatalogo.Rows
    '            If row.RowType = DataControlRowType.DataRow Then
    '                Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
    '                If chkRow.Checked Then
    '                    For i = 0 To Session("CestaCatalogoCaracteristica").Rows.Count - 1
    '                        If (Session("CestaCatalogoCaracteristica").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
    '                            clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCatalogoCaracteristica"))
    '                            Exit For
    '                        End If
    '                    Next
    '                End If
    '            End If
    '        Next

    '        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataSource = Session("CestaCatalogoCaracteristica")
    '        Me.grdDetalleCaracteristicaMantenimientoCatalogo.DataBind()
    '        lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender.Show()
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidar"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub grdListaCaracteristica_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdListaCaracteristica.PageIndexChanging
    '    grdListaCaracteristica.PageIndex = e.NewPageIndex
    '    Me.grdListaCaracteristica.DataSource = CaracteristicaNeg.CaracteristicaListaBusqueda(cboFiltroCaracteristica.SelectedValue, txtBuscarCaracteristica.Text.Trim, "*")
    '    Me.grdListaCaracteristica.DataBind() 'Recargo el grid.
    '    grdListaCaracteristica.SelectedIndex = -1
    '    lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
    'End Sub

    'Private Sub frmLogiGenerarEquipo_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
    '    Try
    '        'Función para validar si tiene permisos
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

    '        If Request.QueryString("IdEquipo") <> "" Then
    '            txtBuscarEquipo.Text = Request.QueryString("IdEquipo")
    '            cboFiltroEquipo.SelectedIndex = 0
    '            Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
    '            Me.grdLista.DataBind()

    '            grdLista.SelectedIndex = 0
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

    'Private Sub grdDetalleCaracteristicaEquipo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleCaracteristicaEquipo.PageIndexChanging
    '    grdDetalleCaracteristicaEquipo.PageIndex = e.NewPageIndex
    '    Me.grdDetalleCaracteristicaEquipo.DataSource = Session("CestaCaracteristicaEquipoPrincipal")
    '    Me.grdDetalleCaracteristicaEquipo.DataBind() 'Recargo el grid.
    '    grdDetalleCaracteristicaEquipo.SelectedIndex = -1
    'End Sub

    'Private Sub cboDescripcionCatalogoComponente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDescripcionCatalogoComponente.SelectedIndexChanged
    '    txtDescripcionCatalogoComponente.Text = cboDescripcionCatalogoComponente.SelectedItem.Value
    '    If cboDescripcionCatalogoComponente.SelectedItem.Value = "" Then
    '        txtDescripcionCatalogoComponente.Text = ""
    '    End If
    '    lnk_mostrarPanelCatalogoComponente_ModalPopupExtender.Show()
    'End Sub

    Sub ConcatenadorDescripcionEquipo()
        'txtDescripcionEquipo.Text = IIf(cboTipoEquipo.SelectedItem.Text.Trim = "SELECCIONE DATO", "", cboTipoEquipo.SelectedItem.Text.Trim + "|") + UCase(txtCapacidadEquipo.Text.Trim) + "|" + UCase(txtNroSerieEquipo.Text.Trim) + "|" + UCase(txtTagEquipo.Text.Trim)
        'txtDescripcionEquipo.Text = txtDescripcionEquipo.Text.Replace("||", "|")
        'txtDescripcionEquipo.Text = IIf(InStrRev(txtDescripcionEquipo.Text.Trim, "|") = Len(txtDescripcionEquipo.Text.Trim), Mid(txtDescripcionEquipo.Text.Trim, 1, Len(txtDescripcionEquipo.Text.Trim) - 1), txtDescripcionEquipo.Text.Trim)
        'txtDescripcionEquipo.Text = IIf(InStr(txtDescripcionEquipo.Text.Trim, "|") = 1, Mid(txtDescripcionEquipo.Text.Trim, 2, Len(txtDescripcionEquipo.Text.Trim) - 1), txtDescripcionEquipo.Text.Trim)
    End Sub

    'Private Sub cboTipoEquipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoEquipo.SelectedIndexChanged
    '    ConcatenadorDescripcionEquipo()
    'End Sub

    'Private Sub txtNroSerieEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtNroSerieEquipo.TextChanged
    '    ConcatenadorDescripcionEquipo()
    'End Sub

    'Private Sub txtNroParteEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtNroParteEquipo.TextChanged
    '    ConcatenadorDescripcionEquipo()
    'End Sub

    'Private Sub txtTagEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtTagEquipo.TextChanged
    '    ConcatenadorDescripcionEquipo()
    'End Sub

    'Private Sub txtCapacidadEquipo_TextChanged(sender As Object, e As EventArgs) Handles txtCapacidadEquipo.TextChanged
    '    ConcatenadorDescripcionEquipo()
    'End Sub
End Class