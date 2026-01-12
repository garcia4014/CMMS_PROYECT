Imports CapaNegocioCMMS
Imports CapaDatosCMMS
'----------------------------------

Public Class frmGnrlMntPerfilSeguridad
    Inherits System.Web.UI.Page

    Dim PerfilNeg As New clsPerfilNegocios
    Dim UsuarioNeg As New clsUsuarioNegocios
    Dim AreaElementoNeg As New clsAreaElementoNegocios
    Dim ConfiguracionNeg As New clsConfiguracionNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator

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

            Dim UsuarioNeg As New CapaNegocioCMMS.clsUsuarioNegocios
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

        'INICIO: Novedades en el perfil.
        Dim mpLabelNovedades As Label
        Dim dtNovedades = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                         "FROM GNRL_USUARIO USU ")
        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelNovedades = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblNovedadesNotificacion"), Label)
            If Not mpLabelNovedades Is Nothing Then
                If dtNovedades.Rows(0).Item(0) > 0 Then
                    mpLabelNovedades.Visible = False 'mpLabelNovedades.Visible = True
                    mpLabelNovedades.Text = dtNovedades.Rows(0).Item(0)
                Else
                    mpLabelNovedades.Visible = False
                End If
            End If
        End If
        'FIN: Invitación de Amigos

        'INICIO: Mis Alertas
        Dim mpLabelAlerta As Label
        Dim dtAlertas = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                      "FROM GNRL_ALERTA ALE " &
                                                      "WHERE ALE.bEstadoRegistroAlerta = 1 AND ALE.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                      "      AND (DATEDIFF(dd, GETDATE(), ALE.dFechaHoraAlerta) <= 5 And Month(ALE.dFechaHoraAlerta) = Month(GETDATE())) AND (DATEDIFF(mi, GETDATE(), dFechaHoraAlerta)) >= 0 ")
        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelAlerta = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblAlertasNotificacion"), Label)
            If Not mpLabelAlerta Is Nothing Then
                If dtAlertas.Rows(0).Item(0) > 0 Then
                    mpLabelAlerta.Visible = True
                    mpLabelAlerta.Text = dtAlertas.Rows(0).Item(0)
                Else
                    mpLabelAlerta.Visible = False
                End If
            End If
        End If
        'FIN: Mis Alertas

        'INICIO: Notificación de Eventos
        Dim mpLabelPreguntasFrecuentes As Label
        Dim dtEventoNotificacion = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                         "FROM GNRL_PREGUNTASFRECUENTES " &
                                                         "WHERE DATEDIFF(dd, dFechaCreacionPreguntasFrecuentes, GETDATE()) < 15")

        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelPreguntasFrecuentes = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblPreguntasFrecuentesNotificacion"), Label)
            If Not mpLabelPreguntasFrecuentes Is Nothing Then
                If dtEventoNotificacion.Rows(0).Item(0) > 0 Then
                    mpLabelPreguntasFrecuentes.Visible = True
                    mpLabelPreguntasFrecuentes.Text = dtEventoNotificacion.Rows(0).Item(0)
                Else
                    mpLabelPreguntasFrecuentes.Visible = False
                End If
            End If
        End If
        'FIN: Notificación de Eventos

        'INICIO: Notificación de Mensajes
        Dim mpLabelMensaje As Label
        Dim dtMensajeNotificacion = FuncionesNeg.FuncionesGetData("SELECT COUNT(*) AS nCantidad " &
                                                                  "FROM GNRL_MENSAJE MSJ " &
                                                                  "WHERE MSJ.cIdEmpresa = '" & Session("IdEmpresa") & "' AND " &
                                                                  "      MSJ.cIdPuntoVenta = '" & Session("IdPuntoVenta") & "' AND " &
                                                                  "      MSJ.bEstadoLeidoMensaje = 0 AND MSJ.vIdParaMensaje = '" & Session("IdUsuario") & "' AND " &
                                                                  "      MSJ.bEstadoRegistroMensaje = 1 ")
        If Not mpContentPlaceHolder Is Nothing Then
            mpLabelMensaje = CType(mpContentPlaceHolder.
                FindControl("pnlPerfil$lblMensajeNotificacion"), Label)
            If Not mpLabelMensaje Is Nothing Then
                If dtMensajeNotificacion.Rows(0).Item(0) > 0 Then
                    mpLabelMensaje.Visible = True
                    mpLabelMensaje.Text = dtMensajeNotificacion.Rows(0).Item(0)
                Else
                    mpLabelMensaje.Visible = False
                End If
            End If
        End If
        'FIN: Notificación de Mensajes
    End Sub

    Function fValidarSesion() As Boolean
        fValidarSesion = False
        If Session("IdUsuario") = "" Then
            fValidarSesion = True
            Throw New Exception("Su sesión ha caducado, ingrese de nuevo por favor.")
        End If
        If Session("IdPuntoVenta") <> "" Or Session("IdEmpresa") <> "" Then
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
        dt.Columns.Add(New DataColumn("Estado", GetType(System.String))) '3
        Return dt
    End Function

    Shared Function CrearCestaAreaElemento() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("IdSistema", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("IdArea", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdElemento", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdModulo", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdPerfil", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("IdUsuario", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("Usuario", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("Modulo", GetType(System.String))) '8
        dt.Columns.Add(New DataColumn("Area", GetType(System.String))) '9
        dt.Columns.Add(New DataColumn("Elemento", GetType(System.String))) '10
        dt.Columns.Add(New DataColumn("Estado", GetType(System.String))) '11
        Return dt
    End Function

    Shared Sub EditarCesta(ByVal Codigo As String, ByVal Descripcion As String, ByVal Estado As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(1) = Codigo
                Tabla.Rows(Fila)(2) = Descripcion
                Tabla.Rows(Fila)(3) = Estado
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub EditarCestaAreaElemento(ByVal IdSistema As String, ByVal IdArea As String, ByVal IdElemento As String, ByVal IdModulo As String,
                                       ByVal IdPerfil As String, ByVal IdUsuario As String, ByVal Usuario As String, ByVal Modulo As String,
                                       ByVal Area As String, ByVal Elemento As String, ByVal Estado As String,
                                       ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(1) = IdSistema
                Tabla.Rows(Fila)(2) = IdArea
                Tabla.Rows(Fila)(3) = IdElemento
                Tabla.Rows(Fila)(4) = IdModulo
                Tabla.Rows(Fila)(5) = IdPerfil
                Tabla.Rows(Fila)(6) = IdUsuario
                Tabla.Rows(Fila)(7) = Usuario
                Tabla.Rows(Fila)(8) = Modulo
                Tabla.Rows(Fila)(9) = Area
                Tabla.Rows(Fila)(10) = Elemento
                Tabla.Rows(Fila)(11) = Estado
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCesta(ByVal Codigo As String, ByVal Descripcion As String,
                            ByVal Estado As String, ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("Estado") = Estado
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub AgregarCestaAreaElemento(ByVal IdSistema As String, ByVal IdArea As String, ByVal IdElemento As String, ByVal IdModulo As String,
                                        ByVal IdPerfil As String, ByVal IdUsuario As String, ByVal Usuario As String, ByVal Modulo As String,
                                        ByVal Area As String, ByVal Elemento As String, ByVal Estado As String, ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("IdSistema") = IdSistema
        Fila("IdArea") = IdArea
        Fila("IdElemento") = IdElemento
        Fila("IdModulo") = IdModulo
        Fila("IdPerfil") = IdPerfil
        Fila("IdUsuario") = IdUsuario
        Fila("Usuario") = Usuario
        Fila("Modulo") = Modulo
        Fila("Area") = Area
        Fila("Elemento") = Elemento
        Fila("Estado") = Estado
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

    Sub CargarCestaOpciones()
        'Carga las opciones que aun no estan asignadas en la Grilla de Opciones del Módulo.
        Try
            VaciarCesta(Session("CestaElemento"))
            Dim AreaElementoNeg As New clsAreaElementoNegocios

            If grdModulo.Rows.Count > 0 Then
                If IsNothing(grdModulo.SelectedRow) = False Then
                    If IsReference(grdModulo.SelectedRow.Cells(1).Text) = True Then
                        Dim Coleccion = AreaElementoNeg.AreaElementoListaBusqueda("AREAELEM.cIdElemento", "", cboSistema.SelectedValue, grdModulo.SelectedRow.Cells(1).Text,
                                                                                  txtIdPerfil.Text.Trim, txtIdUsuario.Text)

                        Dim intContador As Integer = 0

                        For Each Registro In Coleccion
                            AgregarCesta(Registro.Codigo, Registro.Descripcion, Registro.Estado, Session("CestaElemento"))

                            Dim TablaAreaElemento As DataTable
                            TablaAreaElemento = Session("CestaAreaElemento")
                            Dim i As Integer
                            For i = 0 To TablaAreaElemento.Rows.Count - 1
                                If TablaAreaElemento.Rows(i)("IdSistema") = cboSistema.SelectedValue And
                                   TablaAreaElemento.Rows(i)("IdElemento") = Registro.Codigo And
                                   TablaAreaElemento.Rows(i)("IdModulo") = grdModulo.SelectedRow.Cells(1).Text And
                                   TablaAreaElemento.Rows(i)("IdPerfil") = txtIdPerfil.Text And
                                   TablaAreaElemento.Rows(i)("IdUsuario") = txtIdUsuario.Text.Trim Then
                                    QuitarCesta(intContador, Session("CestaElemento"))
                                    intContador = intContador - 1
                                End If
                            Next
                            intContador = intContador + 1
                        Next
                    End If
                End If
            End If
            Me.grdOpcion.DataSource = Session("CestaElemento")
            Me.grdOpcion.DataBind()

        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        Finally
            'cn.Close()
        End Try
    End Sub

    Sub CargarCestaAreaElemento()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        VaciarCesta(Session("CestaAreaElemento"))

        Dim Coleccion = ConfiguracionNeg.ConfiguracionListaBusqueda("CONF.cIdUsuario = '" & txtIdUsuario.Text & "' AND CONF.cIdPerfil", txtIdPerfil.Text, Session("IdPais"), Session("IdEmpresa"), "*")

        Dim intContador As Integer = 0

        For Each Registro In Coleccion
            AgregarCestaAreaElemento(Registro.cIdSistema, Registro.cIdArea, Registro.cIdElemento, Registro.cIdModulo, Registro.cIdPerfil, Registro.cIdUsuario, Registro.Usuario, Registro.Modulo,
                                     Registro.vDescripcionArea, Registro.Elemento, Registro.Estado, Session("CestaAreaElemento"))
        Next

        grdPermisoXArea.DataSource = Session("CestaAreaElemento")
        Me.grdPermisoXArea.DataBind()
    End Sub

    Sub ListarFiltroBusquedaUsuarioCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltroUsu.DataTextField = "vDescripcionTablaSistema"
        cboFiltroUsu.DataValueField = "vValor"
        cboFiltroUsu.DataSource = FiltroNeg.TablaSistemaListarCombo("23", "GNRL", Session("IdEmpresa"))
        cboFiltroUsu.Items.Clear()
        cboFiltroUsu.DataBind()
    End Sub

    Sub ListarFiltroBusquedaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim FiltroNeg As New clsTablaSistemaNegocios
        cboFiltro.DataTextField = "vDescripcionTablaSistema"
        cboFiltro.DataValueField = "vValor"
        cboFiltro.DataSource = FiltroNeg.TablaSistemaListarCombo("22", "GNRL", Session("IdEmpresa"))
        cboFiltro.Items.Clear()
        cboFiltro.DataBind()
    End Sub

    Sub ListarSistemaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim SistemaNeg As New clsSistemaNegocios
        cboSistema.DataTextField = "vDescripcionSistema"
        cboSistema.DataValueField = "cIdSistema"
        cboSistema.DataSource = SistemaNeg.SistemaListarCombo
        cboSistema.Items.Clear()
        cboSistema.DataBind()
    End Sub

    Sub ListarAreaCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim AreaNeg As New clsAreaNegocios
        cboArea.DataTextField = "vDescripcionArea"
        cboArea.DataValueField = "cIdArea"
        cboArea.DataSource = AreaNeg.AreaDepartamentoListarCombo
        cboArea.Items.Clear()
        cboArea.DataBind()
    End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdPerfil.Enabled = False
        txtDescripcion.Enabled = bActivar
        txtIdUsuario.Enabled = bActivar
    End Sub


    Private Function BloquearPagina(ByVal NroPagina As Integer) As Boolean
        BloquearPagina = True
        If NroPagina = 1 Then
            pnlListado.Enabled = True
            pnlGeneral.Enabled = False
            txtBuscar.Focus()
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
                            txtIdPerfil.Focus()
                            BloquearPagina = False
                        End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "N" Then
                pnlListado.Enabled = False
                pnlGeneral.Enabled = True
                txtIdPerfil.Focus()
                BloquearPagina = False
            End If
        ElseIf NroPagina = 0 Then
            If grdLista.Rows.Count = 0 Then
            Else
                If InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 1 Then
                    pnlListado.Enabled = True
                    pnlGeneral.Enabled = False
                    txtBuscar.Focus()
                    strTabContenedorActivo = 1
                ElseIf InStr(MyValidator.ErrorMessage, "permiso") > 0 And strTabContenedorActivo = 0 Then
                    pnlListado.Enabled = True
                    pnlGeneral.Enabled = False
                    txtBuscar.Focus()
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

    Sub LlenarData()
        Try
            Dim Perfil As GNRL_PERFIL = PerfilNeg.PerfilListarPorId(grdLista.SelectedRow.Cells(1).Text)
            txtIdPerfil.Text = Perfil.cIdPerfil
            txtDescripcion.Text = Perfil.vDescripcionPerfil
            txtIdUsuario.Text = ""
            txtUsuario.Text = ""

            grdListaUsu.DataSource = UsuarioNeg.UsuarioGetData("SELECT USU.cIdUsuario AS Codigo, USU.vApellidoPaternoUsuario AS Apellido_Paterno, USU.vApellidoMaternoUsuario AS Apellido_Materno, USU.vNombresUsuario AS Nombres, USU.vLoginUsuario AS Login, USU.vPasswordUsuario, " &
                                     "     USU.vCargoUsuario, USU.bEstadoRegistroUsuario AS Estado, USU.cIdPaisUbicacionGeografica, USU.cIdDepartamentoUbicacionGeografica, " &
                                     "     USU.cIdProvinciaUbicacionGeografica, USU.cIdDistritoUbicacionGeografica " &
                                     "FROM GNRL_USUARIO AS USU INNER JOIN GNRL_USUARIOACCESO AS USUACC ON " &
                                     "USU.cIdUsuario = USUACC.cIdUsuario " &
                                     "WHERE USUACC.cIdPaisOrigenUsuarioAcceso = '" & Session("IdPais") & "' AND USUACC.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                     "      AND USUACC.cIdPerfil = '" & txtIdPerfil.Text & "' " &
                                     "GROUP BY USU.cIdUsuario, USU.vApellidoPaternoUsuario, USU.vApellidoMaternoUsuario, " &
                                     "USU.vNombresUsuario, USU.vLoginUsuario, USU.vPasswordUsuario, USU.vCargoUsuario, " &
                                     "USU.bEstadoRegistroUsuario, USU.cIdPaisUbicacionGeografica, USU.cIdDepartamentoUbicacionGeografica, " &
                                     "USU.cIdProvinciaUbicacionGeografica, USU.cIdDistritoUbicacionGeografica")
            grdListaUsu.DataBind() 'Recargo el grid.

            grdModulo.DataSource = AreaElementoNeg.ModuloListaBusqueda("AREAELEM.cIdSistema", cboSistema.SelectedValue)
            Me.grdModulo.DataBind() 'Recargo el grid.

            'Llenar las Opciones que aún no se han seleccionado de cada Módulo
            CargarCestaOpciones()

            'Llenar las Opciones de todas las Áreas
            CargarCestaAreaElemento()

            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Registro encontrado con éxito"
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

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvSistema.EnableClientScript = bValidar
        Me.rfvArea.EnableClientScript = bValidar
        Me.rfvUsuario.EnableClientScript = bValidar
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean, ByVal bEliminar As Boolean)
        Me.btnNuevo.Visible = bNuevo
        Me.btnEditar.Visible = bEditar
        Me.btnGuardar.Visible = bGuardar
        Me.btnDeshacer.Visible = bDeshacer
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdPerfil.Text = ""
        txtDescripcion.Text = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al SRVPRD web
            strOpcionModulo = "046" 'Mantenimiento de Perfiles - Contabilidad.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaUsuarioCombo()
            ListarFiltroBusquedaCombo()
            cboFiltro.SelectedIndex = 1
            ListarSistemaCombo()
            ListarAreaCombo()

            If Session("CestaElemento") Is Nothing Then
                Session("CestaElemento") = CrearCesta()
            Else
                VaciarCesta(Session("CestaElemento"))
            End If

            If Session("CestaAreaElemento") Is Nothing Then
                Session("CestaAreaElemento") = CrearCestaAreaElemento()
            Else
                VaciarCesta(Session("CestaAreaElemento"))
            End If

            BloquearPagina(1)
            ValidarTexto(False)
            BloquearMantenimiento(True, False, True, False, True)

            Me.grdLista.DataSource = PerfilNeg.PerfilListaBusqueda(cboFiltro.SelectedValue,
                                                                   txtBuscar.Text)
            Me.grdLista.DataBind()
        Else
            cboFiltro.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanListado_txtBuscar')")
            txtBuscar.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanListado_imgbtnBuscarPuntoVenta')")
            imgbtnBuscarPerfilSeguridad.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanListado_grdLista')")
            grdLista.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_txtIdPerfil')")
            txtIdPerfil.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_txtDescripcion')")
            txtDescripcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_txtIdUsuario')")
            txtIdUsuario.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_txtUsuario')")
            txtUsuario.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_cboArea')")
            cboArea.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_cboSistema')")
            cboSistema.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_grdModulo')")
            grdModulo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_grdOpcion')")
            grdOpcion.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContPerfilSeguridad_TabPanGeneral_grdPermisoXArea')")
            grdPermisoXArea.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_imgbtnGuardar')")
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0174", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "N"
            BloquearPagina(2)
            BloquearMantenimiento(False, True, False, True, False)
            LimpiarObjetos()
            ValidarTexto(True)
            ActivarObjetos(True)
            hfTab.Value = "tab2"
            ValidationSummary1.ValidationGroup = "vgrpValidar"
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.TextChanged
        MyValidator.ErrorMessage = ""
        Me.grdLista.DataSource = PerfilNeg.PerfilListaBusqueda(cboFiltro.SelectedValue,
                                                               txtBuscar.Text)
        Me.grdLista.DataBind()
        Me.grdLista.SelectedIndex = 0
    End Sub

    Private Sub grdLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = PerfilNeg.PerfilListaBusqueda(cboFiltro.SelectedValue,
                                                            txtBuscar.Text)
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
        BloquearPagina(1)
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0175", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "E"
            If BloquearPagina(2) = False Then
                BloquearMantenimiento(False, True, False, True, False)
                ValidarTexto(True)
                ActivarObjetos(True)
                LlenarData()
                hfTab.Value = "tab2"
                ValidationSummary1.ValidationGroup = "vgrpValidar"
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

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0176", strOpcionModulo, Session("IdSistema"))

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            If hfdOperacion.Value = "N" Then
                Dim Configuracion As New GNRL_CONFIGURACION
                Dim TablaAreaElemento As DataTable
                TablaAreaElemento = Session("CestaAreaElemento")

                'Inicio: Proceso para la eliminación física en la tabla de configuración
                Configuracion.cIdPerfil = txtIdPerfil.Text
                Configuracion.cIdUsuario = txtIdUsuario.Text
                Configuracion.cIdPaisOrigenUsuario = Session("IdPais")
                Configuracion.cIdEmpresa = Session("IdEmpresa")
                'ConfiguracionNeg.ConfiguracionEliminaRegistro(Configuracion, Session("IdEmpresa"), Session("IdPuntoVenta"))
                ConfiguracionNeg.ConfiguracionEliminaRegistro(Configuracion)
                'Fin: Proceso para la eliminación física en la tabla de configuración

                Dim i As Integer

                For i = 0 To TablaAreaElemento.Rows.Count - 1
                    Configuracion.cIdSistema = TablaAreaElemento.Rows(i)("IdSistema")
                    Configuracion.cIdArea = TablaAreaElemento.Rows(i)("IdArea")
                    Configuracion.cIdElemento = TablaAreaElemento.Rows(i)("IdElemento")
                    Configuracion.cIdModulo = TablaAreaElemento.Rows(i)("IdModulo")
                    Configuracion.cIdPerfil = txtIdPerfil.Text
                    Configuracion.cIdUsuario = TablaAreaElemento.Rows(i)("IdUsuario") 'txtIdUsuario.Text
                    Configuracion.bEstadoRegistroConfiguracion = True

                    If (ConfiguracionNeg.ConfiguracionInserta(Configuracion)) = 0 Then
                        Session("Query") = "PA_GNRL_MNT_CONFIGURACION 'SQL_INSERT', '', '" & Configuracion.cIdUsuario & "', '" & Configuracion.cIdPerfil & "', '" &
                                            Configuracion.cIdElemento & "', '" & Configuracion.cIdModulo & "', '" & Configuracion.cIdSistema & "', '" &
                                            Configuracion.cIdArea & "', '" & Configuracion.bEstadoRegistroConfiguracion & "'"

                        LogAuditoria.vEvento = "INSERTAR CONFIGURACION"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                    End If
                Next
            ElseIf hfdOperacion.Value = "E" Then
                Dim Configuracion As New GNRL_CONFIGURACION
                Dim TablaAreaElemento As DataTable
                TablaAreaElemento = Session("CestaAreaElemento")

                'Inicio: Proceso para la eliminación física en la tabla de configuración
                Configuracion.cIdPerfil = txtIdPerfil.Text
                Configuracion.cIdUsuario = txtIdUsuario.Text
                Configuracion.cIdPaisOrigenUsuario = Session("IdPais")
                Configuracion.cIdEmpresa = Session("IdEmpresa")
                ConfiguracionNeg.ConfiguracionEliminaRegistro(Configuracion)
                'Fin: Proceso para la eliminación física en la tabla de configuración

                Dim i As Integer

                For i = 0 To TablaAreaElemento.Rows.Count - 1
                    Configuracion.cIdSistema = TablaAreaElemento.Rows(i)("IdSistema")
                    Configuracion.cIdArea = TablaAreaElemento.Rows(i)("IdArea")
                    Configuracion.cIdElemento = TablaAreaElemento.Rows(i)("IdElemento")
                    Configuracion.cIdModulo = TablaAreaElemento.Rows(i)("IdModulo")
                    Configuracion.cIdPerfil = txtIdPerfil.Text
                    Configuracion.cIdUsuario = TablaAreaElemento.Rows(i)("IdUsuario") 'txtIdUsuario.Text
                    Configuracion.bEstadoRegistroConfiguracion = True
                    If (ConfiguracionNeg.ConfiguracionInserta(Configuracion)) = 0 Then
                        Session("Query") = "PA_GNRL_MNT_CONFIGURACION 'SQL_UPDATE', '', '" & Configuracion.cIdUsuario & "', '" & Configuracion.cIdPerfil & "', '" &
                                            Configuracion.cIdElemento & "', '" & Configuracion.cIdModulo & "', '" & Configuracion.cIdSistema & "', '" &
                                            Configuracion.cIdArea & "', '" & Configuracion.bEstadoRegistroConfiguracion & "'"

                        LogAuditoria.vEvento = "ACTUALIZAR CONFIGURACION"
                        LogAuditoria.vQuery = Session("Query")
                        LogAuditoria.cIdSistema = Session("IdSistema")
                        LogAuditoria.cIdModulo = strOpcionModulo

                        FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        MyValidator.ErrorMessage = "Registro actualizado con éxito"
                    End If
                Next
                Me.grdLista.DataSource = PerfilNeg.PerfilListaBusqueda(cboFiltro.SelectedValue,
                                                                       txtBuscar.Text)
                Me.grdLista.DataBind()

                pnlGeneral.Enabled = False
                BloquearMantenimiento(True, False, True, False, True)
                hfTab.Value = "tab1"
            End If
            hfdOperacion.Value = "R"
            BloquearPagina(0)
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

    Protected Sub btnDeshacer_Click(sender As Object, e As EventArgs) Handles btnDeshacer.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0177", strOpcionModulo, Session("IdSistema"))

            hfdOperacion.Value = "R"
            BloquearPagina(0) 'BloquearPagina(1)
            BloquearMantenimiento(True, False, True, False, True)
            ValidarTexto(False)
            ActivarObjetos(False)
            hfTab.Value = "tab1"
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

    Protected Sub cboFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFiltro.SelectedIndexChanged
        Me.grdLista.DataSource = PerfilNeg.PerfilListaBusqueda(cboFiltro.SelectedValue,
                                                               txtBuscar.Text)
        Me.grdLista.DataBind()
    End Sub

    Protected Sub cboSistema_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboSistema.SelectedIndexChanged
        grdModulo.DataSource = AreaElementoNeg.ModuloListaBusqueda("AREAELEM.cIdSistema", cboSistema.SelectedValue) 'JMUG: 08/02/2023
        Me.grdModulo.DataBind() 'Recargo el grid.
        If grdModulo.Rows.Count > 0 Then
            grdModulo.SelectedIndex = 0
        End If
        CargarCestaOpciones()
    End Sub

    Private Sub grdModulo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdModulo.PageIndexChanging
        grdModulo.PageIndex = e.NewPageIndex
        grdModulo.DataSource = AreaElementoNeg.ModuloListaBusqueda("AREAELEM.cIdSistema", cboSistema.SelectedValue)
        Me.grdModulo.DataBind()
        grdModulo.SelectedIndex = -1
    End Sub

    Private Sub grdModulo_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdModulo.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center
            Next
        End If
    End Sub

    Protected Sub grdModulo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdModulo.SelectedIndexChanged
        CargarCestaOpciones()
    End Sub

    Protected Sub cboArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboArea.SelectedIndexChanged
        grdModulo.DataSource = AreaElementoNeg.ModuloListaBusqueda("AREAELEM.cIdSistema", cboSistema.SelectedValue)
        Me.grdModulo.DataBind() 'Recargo el grid.
        If grdModulo.Rows.Count > 0 Then
            grdModulo.SelectedIndex = 0
        End If
        CargarCestaOpciones()
    End Sub

    Private Sub grdOpcion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdOpcion.PageIndexChanging
        grdOpcion.PageIndex = e.NewPageIndex
        grdOpcion.DataSource = Session("CestaElemento")
        Me.grdOpcion.DataBind()
        grdOpcion.SelectedIndex = -1
    End Sub

    Private Sub grdOpcion_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdOpcion.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center
            Next
        End If
    End Sub

    Private Sub grdPermisoXArea_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPermisoXArea.PageIndexChanging
        grdPermisoXArea.PageIndex = e.NewPageIndex
        grdPermisoXArea.DataSource = Session("CestaAreaElemento")
        Me.grdPermisoXArea.DataBind()
        grdPermisoXArea.SelectedIndex = -1
    End Sub

    Private Sub grdPermisoXArea_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPermisoXArea.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdPermisoXArea_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdPermisoXArea.RowDeleting
        QuitarCesta(e.RowIndex, Session("CestaAreaElemento"))
        Me.grdPermisoXArea.DataSource = Session("CestaAreaElemento")
        Me.grdPermisoXArea.DataBind()

        grdModulo_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub grdOpcion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdOpcion.SelectedIndexChanged
        Try
            MyValidator.ErrorMessage = ""
            If txtIdUsuario.Text = "" Then
                Throw New Exception("Debe de seleccionar primero el usuario que desea asignar las opciones.")
            End If

            AgregarCestaAreaElemento(cboSistema.SelectedValue, cboArea.SelectedValue, grdOpcion.SelectedRow.Cells(1).Text, grdModulo.SelectedRow.Cells(1).Text,
                                     txtIdPerfil.Text, txtIdUsuario.Text, txtUsuario.Text, grdModulo.SelectedRow.Cells(3).Text, cboArea.SelectedItem.Text, grdOpcion.SelectedRow.Cells(2).Text,
                                     "True", Session("CestaAreaElemento"))

            grdPermisoXArea.DataSource = Session("CestaAreaElemento")
            Me.grdPermisoXArea.DataBind()

            QuitarCesta(grdOpcion.SelectedIndex, Session("CestaElemento"))
            grdOpcion.DataSource = Session("CestaElemento")
            Me.grdOpcion.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub grdPermisoXArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdPermisoXArea.SelectedIndexChanged
        grdPermisoXArea.DataSource = Session("CestaAreaElemento")
        Me.grdPermisoXArea.DataBind()
    End Sub

    Protected Sub txtBuscarUsu_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscarUsu.TextChanged
        grdListaUsu.DataSource = UsuarioNeg.UsuarioGetData("SELECT USU.cIdUsuario AS Codigo, USU.vApellidoPaternoUsuario AS Apellido_Paterno, USU.vApellidoMaternoUsuario AS Apellido_Materno, USU.vNombresUsuario AS Nombres, USU.vLoginUsuario AS Login, USU.vPasswordUsuario, " &
                                     "     USU.vCargoUsuario, USU.bEstadoRegistroUsuario AS Estado, USU.cIdPaisUbicacionGeografica, USU.cIdDepartamentoUbicacionGeografica, " &
                                     "     USU.cIdProvinciaUbicacionGeografica, USU.cIdDistritoUbicacionGeografica " &
                                     "FROM GNRL_USUARIO AS USU INNER JOIN GNRL_USUARIOACCESO AS USUACC ON " &
                                     "USU.cIdUsuario = USUACC.cIdUsuario " &
                                     "WHERE USUACC.cIdPaisOrigenUsuarioAcceso = '" & Session("IdPais") & "' AND USUACC.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                     "      AND USUACC.cIdPerfil = '" & txtIdPerfil.Text & "'")
        Me.grdListaUsu.DataBind()
    End Sub

    Protected Sub imgbtnBuscarUsu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnBuscarUsu.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            Me.txtIdUsuario_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdListaUsu_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdListaUsu.PageIndexChanging
        grdListaUsu.PageIndex = e.NewPageIndex
        grdListaUsu.DataSource = UsuarioNeg.UsuarioGetData("SELECT USU.cIdUsuario AS Codigo, USU.vApellidoPaternoUsuario AS Apellido_Paterno, USU.vApellidoMaternoUsuario AS Apellido_Materno, USU.vNombresUsuario AS Nombres, USU.vLoginUsuario AS Login, USU.vPasswordUsuario, " &
                                     "     USU.vCargoUsuario, USU.bEstadoRegistroUsuario AS Estado, USU.cIdPaisUbicacionGeografica, USU.cIdDepartamentoUbicacionGeografica, " &
                                     "     USU.cIdProvinciaUbicacionGeografica, USU.cIdDistritoUbicacionGeografica " &
                                     "FROM GNRL_USUARIO AS USU INNER JOIN GNRL_USUARIOACCESO AS USUACC ON " &
                                     "USU.cIdUsuario = USUACC.cIdUsuario " &
                                     "WHERE USUACC.cIdPaisOrigenUsuarioAcceso = '" & Session("IdPais") & "' AND USUACC.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                     "      AND USUACC.cIdPerfil = '" & txtIdPerfil.Text & "'")
        grdListaUsu.DataBind() 'Recargo el grid.
        grdListaUsu.SelectedIndex = -1
        txtIdUsuario_ModalPopupExtender.Show()
    End Sub

    Private Sub grdListaUsu_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdListaUsu.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center
            Next
        End If
    End Sub

    Protected Sub grdListaUsu_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdListaUsu.SelectedIndexChanged
        If IsReference(grdListaUsu.SelectedRow.Cells(1).Text) = True Then
            Me.txtIdUsuario_ModalPopupExtender.Show()
        End If
    End Sub

    Protected Sub btnAceptarUsu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarUsu.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            lblMensajeUsu.Text = ""
            If IsNothing(grdListaUsu.SelectedRow) = False Then
                If lblMensajeUsu.Text = "" Then
                    txtIdUsuario.Text = grdListaUsu.SelectedRow.Cells(1).Text
                    txtUsuario.Text = Trim(Server.HtmlDecode(grdListaUsu.SelectedRow.Cells(3).Text)) + " " + Trim(Server.HtmlDecode(grdListaUsu.SelectedRow.Cells(4).Text)) + ", " + Trim(Server.HtmlDecode(grdListaUsu.SelectedRow.Cells(5).Text))
                    txtIdUsuario.Focus()
                    cboArea_SelectedIndexChanged(sender, e)
                    BloquearPagina(2).ToString()

                    CargarCestaAreaElemento()

                    'Llenar las Opciones que aún no se han seleccionado de cada Módulo
                    CargarCestaOpciones()
                End If
            Else
                txtIdUsuario.Text = ""
                txtIdUsuario_ModalPopupExtender.Show()
                lblMensajeUsu.Text = "Debe de seleccionar un item."
            End If
        Catch ex As Exception
            lblMensajeUsu.Text = ex.Message
            txtIdUsuario_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdLista_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(1).Visible = True
            e.Row.Cells(2).Visible = True
            e.Row.Cells(3).Visible = True
        End If
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdLista.SelectedIndexChanged
        MyValidator.ErrorMessage = ""
        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            BloquearPagina(0)
            ValidarTexto(False)
        End If
    End Sub

    Private Sub imgbtnBuscarPerfilSeguridad_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarPerfilSeguridad.Click
        Me.grdLista.DataSource = PerfilNeg.PerfilListaBusqueda(cboFiltro.SelectedValue,
                                                                   txtBuscar.Text)
        Me.grdLista.DataBind()
    End Sub
End Class