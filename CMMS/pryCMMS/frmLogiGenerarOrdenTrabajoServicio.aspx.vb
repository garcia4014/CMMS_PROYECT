Imports CapaNegocioCMMS
Imports CapaDatosCMMS
Imports AjaxControlToolkit
Imports CrystalDecisions.ReportAppServer.CommonControls
Imports System.Data.SqlClient

Public Class frmLogiGenerarOrdenTrabajoServicio
    Inherits System.Web.UI.Page
    Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios
    Dim OrdenFabricacionNeg As New clsOrdenFabricacionNegocios
    Dim DetalleOrdenTrabajoNeg As New clsDetalleOrdenTrabajoNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    Dim MyValidator As New CustomValidator
    Dim indexValueSave As Int32
    Dim BD_DataContext As New BDCMMS_MovitecnicaDataContext
    'Dim OrdTra As New LOGI_CABECERAORDENTRABAJO

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

    'begin cesta actividades
    Shared Function CrearCestaActividad() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Item", GetType(System.Int32))) '0
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("Estado", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("DescripcionComponente", GetType(System.String))) '6
        'dt.Columns.Add(New DataColumn("OrdenUbicacion", GetType(System.Int32))) '6
        Return dt
    End Function

    Shared Sub EditarCestaActividad(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal Estado As String, ByVal DescripcionComponente As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Descripcion
                Tabla.Rows(Fila)(2) = IdCatalogo
                Tabla.Rows(Fila)(3) = IdJerarquia
                Tabla.Rows(Fila)(4) = Estado
                Tabla.Rows(Fila)(5) = DescripcionComponente
                'Tabla.Rows(Fila)(6) = OrdenUbicacion
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaActividad(ByVal Codigo As String, ByVal Descripcion As String, ByVal IdCatalogo As String,
                            ByVal IdJerarquia As String, ByVal Estado As String, ByVal DescripcionComponente As String,
                            ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Tabla.Rows.Count + 1
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdJerarquia") = IdJerarquia
        Fila("Estado") = Estado
        Fila("DescripcionComponente") = DescripcionComponente
        'Fila("OrdenUbicacion") = OrdenUbicacion
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaActividad(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaActividad(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub
    'end cesta actividades

    Shared Function CrearCestaOrdenTrabajo() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Personal", GetType(System.String))) '2
        Return dt
    End Function

    Shared Sub EditarCestaOrdenTrabajo(ByVal Codigo As String, ByVal Personal As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Personal
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaOrdenTrabajo(ByVal Codigo As String, ByVal Personal As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Personal") = Personal
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaOrdenTrabajo(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaOrdenTrabajo(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Function CrearCestaInsumos() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '0
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Cantidad", GetType(System.Decimal))) '2
        dt.Columns.Add(New DataColumn("DescripcionUnidadMedida", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdActividad", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("IdArticuloSAP", GetType(System.String))) '8
        Return dt
    End Function

    Shared Sub EditarCestaInsumos(ByVal Codigo As String, ByVal Descripcion As String, ByVal Cantidad As Decimal,
                           ByVal DescripcionUnidadMedida As String, ByVal IdEquipo As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticuloSAP As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Descripcion
                Tabla.Rows(Fila)(2) = Cantidad
                Tabla.Rows(Fila)(3) = DescripcionUnidadMedida
                Tabla.Rows(Fila)(4) = IdEquipo
                Tabla.Rows(Fila)(5) = IdCatalogo
                Tabla.Rows(Fila)(6) = IdJerarquia
                Tabla.Rows(Fila)(7) = IdActividad
                Tabla.Rows(Fila)(8) = IdArticuloSAP
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaInsumos(ByVal Codigo As String, ByVal Descripcion As String, ByVal Cantidad As Decimal,
                           ByVal DescripcionUnidadMedida As String, ByVal IdEquipo As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdActividad As String, ByVal IdArticuloSAP As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("Cantidad") = Cantidad
        Fila("DescripcionUnidadMedida") = DescripcionUnidadMedida
        Fila("IdEquipo") = IdEquipo
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdJerarquia") = IdJerarquia
        Fila("IdActividad") = IdActividad
        Fila("IdArticuloSAP") = IdArticuloSAP
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaInsumos(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaInsumos(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Function CrearCestaOtrosDatos() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Item", GetType(System.Int32))) '0 - Nro.Item
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1 - IdCaracteristica
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '2 - Descripcion Caracteristica
        dt.Columns.Add(New DataColumn("ValorReferencial", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdActividad", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("IdArticuloSAP", GetType(System.String))) '8
        dt.Columns.Add(New DataColumn("IdEquipoCheckList", GetType(System.String))) '9
        Return dt
    End Function

    Shared Sub EditarCestaOtrosDatos(ByVal Codigo As String, ByVal Descripcion As String, ByVal ValorReferencial As String,
                           ByVal IdEquipo As String, ByVal IdActividad As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdArticuloSAP As String, ByVal IdEquipoCheckList As String,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(1) = Codigo
                Tabla.Rows(Fila)(2) = Descripcion
                Tabla.Rows(Fila)(3) = ValorReferencial
                Tabla.Rows(Fila)(4) = IdEquipo
                Tabla.Rows(Fila)(5) = IdActividad
                Tabla.Rows(Fila)(6) = IdCatalogo
                Tabla.Rows(Fila)(7) = IdJerarquia
                Tabla.Rows(Fila)(8) = IdArticuloSAP
                Tabla.Rows(Fila)(9) = IdEquipoCheckList
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaOtrosDatos(ByVal Codigo As String, ByVal Descripcion As String, ByVal ValorReferencial As String,
                           ByVal IdEquipo As String, ByVal IdActividad As String, ByVal IdCatalogo As String,
                           ByVal IdJerarquia As String, ByVal IdArticuloSAP As String, ByVal IdEquipoCheckList As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Tabla.Rows.Count + 1
        Fila("Codigo") = Codigo
        Fila("Descripcion") = Descripcion
        Fila("ValorReferencial") = ValorReferencial
        Fila("IdEquipo") = IdEquipo
        Fila("IdActividad") = IdActividad
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdJerarquia") = IdJerarquia
        Fila("IdArticuloSAP") = IdArticuloSAP
        Fila("IdEquipoCheckList") = IdEquipoCheckList
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaOtrosDatos(ByVal Fila As Integer, ByVal Tabla As DataTable)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows.RemoveAt(Fila)
                Dim i As Integer
                For i = 0 To Tabla.Rows.Count - 1
                    Tabla.Rows(i).BeginEdit()
                    Tabla.Rows(i)(0) = i + 1
                    Tabla.Rows(i).EndEdit()
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub VaciarCestaOtrosDatos(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Function CrearCestaPersonal() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Codigo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("Personal", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("Responsable", GetType(System.Boolean))) '3
        Return dt
    End Function

    Shared Sub EditarCestaPersonal(ByVal Codigo As String, ByVal Personal As String, ByVal Responsable As Boolean,
                           ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)(0) = Codigo
                Tabla.Rows(Fila)(1) = Personal
                Tabla.Rows(Fila)(2) = Responsable
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaPersonal(ByVal Codigo As String, ByVal Personal As String, ByVal Responsable As String,
                           ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("Codigo") = Codigo
        Fila("Personal") = Personal
        Fila("Responsable") = Responsable
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaPersonal(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaPersonal(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
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
        cboFiltroOrdenTrabajo.DataTextField = "vDescripcionTablaSistema"
        cboFiltroOrdenTrabajo.DataValueField = "vValor"
        cboFiltroOrdenTrabajo.DataSource = FiltroNeg.TablaSistemaListarCombo("86", "LOGI", Session("IdEmpresa"))
        cboFiltroOrdenTrabajo.Items.Clear()
        cboFiltroOrdenTrabajo.DataBind()
    End Sub

    Sub ListarActividadCheckListCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ActCheckListNeg As New clsActividadCheckListNegocios
        cboActividadCatalogoComponente.DataTextField = "vDescripcionActividadCheckList"
        cboActividadCatalogoComponente.DataValueField = "cIdActividadCheckList"
        cboActividadCatalogoComponente.DataSource = ActCheckListNeg.ActividadCheckListListarCombo()
        cboActividadCatalogoComponente.Items.Clear()
        cboActividadCatalogoComponente.Items.Add(New ListItem("SELECCIONE DATO", "SELECCIONE DATO"))
        cboActividadCatalogoComponente.DataBind()
    End Sub

    Sub ListarPersonalAsignadoCombo(ByVal OrdTra As LOGI_CABECERAORDENTRABAJO)
        Dim PersonalNeg As New clsOrdenTrabajoNegocios
        cboPersonalAsignadoMantenimientoTarea.DataTextField = "vNombreCompletoPersonal"
        cboPersonalAsignadoMantenimientoTarea.DataValueField = "cIdPersonal"
        cboPersonalAsignadoMantenimientoTarea.DataSource = PersonalNeg.OrdenTrabajoRecursosListarComboV2(OrdTra)
        cboPersonalAsignadoMantenimientoTarea.Items.Clear()
        cboPersonalAsignadoMantenimientoTarea.Items.Add("SELECCIONE DATO")
        cboPersonalAsignadoMantenimientoTarea.DataBind()
    End Sub

    Sub ListarPersonalCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataTextField = "vNombreCompletoPersonal"
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataValueField = "cIdPersonal"
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = ResponsableNeg.PersonalListarCombo()
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Items.Clear()
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Items.Add("SELECCIONE DATO")
        cboPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ListarInsumosCombo()

        Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
        Dim InsumosNeg As New clsDetalleOrdenTrabajoNegocios
        cboInsumosAgregarInsumos.DataTextField = "vDescripcionArticuloSAPDetalleOrdenFabricacion"
        cboInsumosAgregarInsumos.DataValueField = "vIdArticuloSAPDetalleOrdenFabricacion"
        cboInsumosAgregarInsumos.DataSource = InsumosNeg.DetalleOrdenTrabajoGetData("SELECT * FROM LOGI_DETALLEORDENFABRICACION " &
                                                                      "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & ValoresOS(0).ToString & "' " &
                                                                      "      AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & ValoresOS(1).ToString & "' " &
                                                                      "      AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & ValoresOS(2).ToString & "' " &
                                                                      "      AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
        cboInsumosAgregarInsumos.Items.Clear()
        cboInsumosAgregarInsumos.Items.Add("SELECCIONE DATO")
        cboInsumosAgregarInsumos.DataBind()
    End Sub

    Sub ListarTiempoCombo()
        'Llena los DropDownList de horas, minutos y segundos
        cboHorasMantenimientoTarea.Items.Clear()
        cboMinutosMantenimientoTarea.Items.Clear()
        cboSegundosMantenimientoTarea.Items.Clear()

        For i = 0 To 23
            cboHorasMantenimientoTarea.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next

        For i = 0 To 59
            cboMinutosMantenimientoTarea.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
            cboSegundosMantenimientoTarea.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next
        cboMeridianoMantenimientoTarea.Enabled = False
    End Sub

    'Sub ListarTiempoDetalleOrdenTrabajoCombo()
    '    'Llena los DropDownList de horas, minutos y segundos
    '    cboHorasMantenimientoTareaDetalleOrdenTrabajo.Items.Clear()
    '    cboMinutosMantenimientoTareaDetalleOrdenTrabajo.Items.Clear()
    '    cboSegundosMantenimientoTareaDetalleOrdenTrabajo.Items.Clear()

    '    For i = 0 To 23
    '        cboHorasMantenimientoTareaDetalleOrdenTrabajo.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
    '    Next

    '    For i = 0 To 59
    '        cboMinutosMantenimientoTareaDetalleOrdenTrabajo.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
    '        cboSegundosMantenimientoTareaDetalleOrdenTrabajo.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
    '    Next
    '    cboMeridianoMantenimientoTareaDetalleOrdenTrabajo.Enabled = False
    'End Sub

    Sub ListarOtrosDatosCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim CaracteristicaNeg As New clsCaracteristicaNegocios
        cboCaracteristicaOtrosDatos.DataTextField = "vDescripcionCaracteristica"
        cboCaracteristicaOtrosDatos.DataValueField = "cIdCaracteristica"
        cboCaracteristicaOtrosDatos.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
        cboCaracteristicaOtrosDatos.Items.Clear()
        cboCaracteristicaOtrosDatos.DataBind()
    End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
    End Sub

    Sub LimpiarObjetosOtrosDatos()
        'txtDescripcionCampoSAPCaracteristica.Text = ""
    End Sub

    Sub LlenarDataEquipo()
        'Try 'JMUG: LO QUITE 11/09/2023
        Dim EquipoNeg As New clsEquipoNegocios
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(grdLista.SelectedRow.Cells(9).Text)
        lblIdEquipo.Text = Equipo.cIdEquipo
        lblDescripcionEquipo.Text = Equipo.vDescripcionEquipo
        lblNroSerieEquipo.Text = Equipo.vNumeroSerieEquipo
        lblNroParteEquipo.Text = Equipo.vNumeroParteEquipo
        lnkEsComponenteOn.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", False, True)
        lnkEsComponenteOff.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", True, False)
        divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        Dim TipActNeg As New clsTipoActivoNegocios
        lblTipoActivoEquipo.Text = TipActNeg.TipoActivoListarPorId(Equipo.cIdTipoActivo).vDescripcionTipoActivo
        Dim CatNeg As New clsCatalogoNegocios
        lblCatalogoEquipo.Text = CatNeg.CatalogoListarPorId(Equipo.cIdCatalogo, Equipo.cIdTipoActivo, Equipo.cIdJerarquiaCatalogo, "1").vDescripcionCatalogo
        Dim SisFunNeg As New clsSistemaFuncionalNegocios
        lblSistemaFuncionalEquipo.Text = SisFunNeg.SistemaFuncionalListarPorId(Equipo.cIdSistemaFuncionalEquipo).vDescripcionSistemaFuncional
        Dim ClienteNeg As New clsClienteNegocios
        Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorIdSAP(Equipo.vIdClienteSAPEquipo, Session("IdEmpresa"))

        lblPlantillaChecklistEquipo.Text = EquipoNeg.EquipoGetData("SELECT vDescripcionCabeceraCheckListPlantilla 
                                                    FROM LOGI_CABECERACHECKLISTPLANTILLA
                                                    WHERE cIdTipoMantenimiento = '" & grdLista.SelectedRow.Cells(14).Text & "' 
                                                      AND cIdTipoActivoCabeceraCheckListPlantilla = '" & Equipo.cIdTipoActivo & "' 
                                                      AND cIdCatalogoCabeceraCheckListPlantilla = '" & Equipo.cIdCatalogo & "' 
                                                      AND cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '" & Equipo.cIdJerarquiaCatalogo & "'") _
                                                    .Rows(0).Item(0).ToString()
        CargarCestaInsumos()

        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro seleccionado"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        hfdCorreoElectronicoCliente.Value = ""
        hfdDNICliente.Value = ""
        hfdRUCCliente.Value = ""
        hfdIdClienteSAPEquipo.Value = ""
        hfdIdEquipoSAPEquipo.Value = ""
        hfdFechaCreacionEquipo.Value = ""
        hfdOrdenFabricacionReferencia.Value = ""
    End Sub

    Sub LimpiarObjetosImagen()
        MyValidator.ErrorMessage = ""
        txtTituloSubirImagenActividad.Text = ""
        txtDescripcionSubirImagenActividad.Text = ""
        txtObservacionSubirImagenActividad.Text = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        ' Deshabilitar la validación para esta request específica
        Dim request As HttpRequestBase = New HttpRequestWrapper(Context.Request)

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "070" 'Mantenimiento de las Solicitudes de Servicios.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroOrdenTrabajo.SelectedIndex = 4
            ListarTipoMantenimientoCombo()
            'ListarPlantillaCheckListCombo()
            'ListarPersonalResponsableCombo()
            ListarPersonalCombo()

            If Session("CestaOrdenTrabajo") Is Nothing Then
                Session("CestaOrdenTrabajo") = CrearCestaOrdenTrabajo()
            Else
                VaciarCestaOrdenTrabajo(Session("CestaOrdenTrabajo"))
            End If

            If Session("CestaInsumos") Is Nothing Then
                Session("CestaInsumos") = CrearCestaInsumos()
            Else
                VaciarCestaInsumos(Session("CestaInsumos"))
            End If

            If Session("CestaInsumosFiltrado") Is Nothing Then
                Session("CestaInsumosFiltrado") = CrearCestaInsumos()
            Else
                VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
            End If

            If Session("CestaOtrosDatos") Is Nothing Then
                Session("CestaOtrosDatos") = CrearCestaOtrosDatos()
            Else
                VaciarCestaOtrosDatos(Session("CestaOtrosDatos"))
            End If

            If Session("CestaSSPersonal") Is Nothing Then
                Session("CestaSSPersonal") = CrearCestaPersonal()
            Else
                VaciarCestaPersonal(Session("CestaSSPersonal"))
            End If

            If Session("CestaOTPersonal") Is Nothing Then
                Session("CestaOTPersonal") = CrearCestaPersonal()
            Else
                VaciarCestaPersonal(Session("CestaOTPersonal"))
            End If

            If Session("CestaActividadCatalogoComponente") Is Nothing Then
                Session("CestaActividadCatalogoComponente") = CrearCestaActividad()
            Else
                VaciarCestaActividad(Session("CestaActividadCatalogoComponente"))
            End If

            Me.grdLista.DataSource = fLlenarGrilla()
            Me.grdLista.DataBind()
            pnlCabecera.Visible = True
            pnlContenido.Visible = False
            ListarActividadCheckListCombo()
        Else
            txtBuscarOrdenTrabajo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
        End If
    End Sub

    Function fLlenarGrilla() As DataTable
        Try
            Dim SubFiltro As String = ""
            SubFiltro = ") AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        "INNER JOIN GNRL_USUARIO AS USU ON " &
                        "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        "WHERE RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

            Dim dsOT = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("(cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdUsuario") & "'AND  DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo != '' AND (cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (DOC.bEstadoRegistroCabeceraOrdenTrabajo = '1')) OR ((cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = ''  AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", ")", SubFiltro), "1")
            'Dim dsOT = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("(cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdUsuario") & "'AND  DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo != '' AND (cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (cIdUsuarioCreacionCabeceraOrdenTrabajo = 'U000000001' AND DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo != '' AND (cEstadoCabeceraOrdenTrabajo = '*' OR '*' = '*')) OR ((cEstadoCabeceraOrdenTrabajo = '*' OR '*' = '*') AND cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = ''  AND vNumeroSerieEquipo AND (DOC.bEstadoRegistroCabeceraOrdenTrabajo = '1' OR '*' = '1')  ) OR ((cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = ''  AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", ")", SubFiltro), "1")
            'Dim dsOT = OrdenTrabajoNeg.OrdenTrabajoListaBusqueda("(cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdUsuario") & "'AND  DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo != '' AND (cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND (cIdUsuarioCreacionCabeceraOrdenTrabajo = 'U000000001' AND DOC.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo != '' AND (cEstadoCabeceraOrdenTrabajo = '*' OR '*' = '*')) OR ((cEstadoCabeceraOrdenTrabajo = '*' OR '*' = '*') AND cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = ''  AND vNumeroSerieEquipo AND (DOC.bEstadoRegistroCabeceraOrdenTrabajo = '1')) OR ((cEstadoCabeceraOrdenTrabajo = '" & cboFiltroStatus.SelectedValue & "' OR '*' = '" & cboFiltroStatus.SelectedValue & "') AND cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND LTRIM(RTRIM(ISNULL(vContratoReferenciaCabeceraOrdenTrabajo, ''))) = ''  AND " & cboFiltroOrdenTrabajo.SelectedValue, txtBuscarOrdenTrabajo.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", ")", SubFiltro) "1")

            Dim dtOT As New DataTable
            dtOT = FuncionesNeg.ConvertirListADataTable(dsOT)
            dtOT.Columns.Add(New DataColumn("bVisualizarDOC", GetType(System.Boolean)))

            Dim RowCount As Int16
            For Each OdrTra In dtOT.Rows
                RowCount = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_DOCUMENTACIONEQUIPO WHERE cIdEquipo = '" & OdrTra("IdEquipo") & "'").Rows(0).Item(0)
                If RowCount > 0 Then
                    OdrTra("bVisualizarDOC") = True
                Else
                    OdrTra("bVisualizarDOC") = False
                End If
            Next

            Return dtOT
        Catch ex As Exception

        End Try
    End Function

    Sub CargarStatusComponenteTarea()
        lnkbtnIniciarOrdenTrabajoTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "D", True, False)
        'If hfdIdTipoControlTiempo.Value = "D" Then
        '    If hfdStatusOrdenTrabajoTarea.Value = "I" Or hfdStatusOrdenTrabajoTarea.Value = "" Then 'Iniciar
        '        lnkbtnIniciarOrdenTrabajoTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "D", True, False)
        '    Else
        '        lnkbtnIniciarOrdenTrabajoTarea.Visible = False
        '    End If
        '    If hfdStatusOrdenTrabajoTarea.Value = "R" Then 'En Proceso / Retomar
        '        lnkbtnPendienteOrdenTrabajoTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "D", True, False)
        '        lnkbtnFinalizarOrdenTrabajoTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "D", True, False)
        '    Else
        '        lnkbtnPendienteOrdenTrabajoTarea.Visible = False
        '    End If
        '    If hfdStatusOrdenTrabajoTarea.Value = "P" Then 'Pendiente
        '        lnkbtnRetomarOrdenTrabajoTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "D", True, False)
        '        lnkbtnFinalizarOrdenTrabajoTarea.Visible = False 'True
        '    Else
        '        lnkbtnRetomarOrdenTrabajoTarea.Visible = False
        '    End If
        '    If hfdStatusOrdenTrabajoTarea.Value = "F" Then 'Finalizar
        '        lnkbtnFinalizarOrdenTrabajoTarea.Visible = False
        '    End If
        'End If
        For Each elemento As DataListItem In lstComponentes.Items
            Dim hfdStatusComponenteTarea As HiddenField = TryCast(elemento.FindControl("hfdStatusComponenteTarea"), HiddenField)
            Dim lnkbtnIniciarComponenteTarea As LinkButton = TryCast(elemento.FindControl("lnkbtnIniciarComponenteTarea"), LinkButton)
            Dim lnkbtnPendienteComponenteTarea As LinkButton = TryCast(elemento.FindControl("lnkbtnPendienteComponenteTarea"), LinkButton)
            Dim lnkbtnRetomarComponenteTarea As LinkButton = TryCast(elemento.FindControl("lnkbtnRetomarComponenteTarea"), LinkButton)
            Dim lnkbtnFinalizarComponenteTarea As LinkButton = TryCast(elemento.FindControl("lnkbtnFinalizarComponenteTarea"), LinkButton)
            If hfdStatusComponenteTarea.Value = "I" Then 'Iniciar
                lnkbtnIniciarComponenteTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "C", True, False)
            Else
                lnkbtnIniciarComponenteTarea.Visible = False
            End If
            If hfdStatusComponenteTarea.Value = "R" Then 'En Proceso / Retomar
                lnkbtnPendienteComponenteTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "C", True, False)
                lnkbtnFinalizarComponenteTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "C", True, False)
            Else
                lnkbtnPendienteComponenteTarea.Visible = False
            End If
            If hfdStatusComponenteTarea.Value = "P" Then 'Pendiente
                lnkbtnRetomarComponenteTarea.Visible = IIf(hfdIdTipoControlTiempo.Value = "C", True, False)
                lnkbtnFinalizarComponenteTarea.Visible = False 'True
            Else
                lnkbtnRetomarComponenteTarea.Visible = False
            End If
            If hfdStatusComponenteTarea.Value = "F" Then 'Finalizar
                lnkbtnFinalizarComponenteTarea.Visible = False
            End If
        Next
    End Sub

    Function fLlenarGrillaComponentes() As DataTable
        'JMUG: INICIO - 19/12/2023
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        'JMUG: 14/07/2025 Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
        Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(3).FindControl("hlnkVerInforme"), HyperLink)
        Dim strNroOrdenTrabajo As String = ""
        'JMUG: 14/07/2025 strNroOrdenTrabajo = hlnkVerInforme.Text
        strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
        'JMUG: FINAL - 19/12/2023

        Dim ComponentesEquipoNeg As New clsComponenteOrdenTrabajoNegocios
        Dim dsComponentes = ComponentesEquipoNeg.ComponenteOrdenTrabajoListaGridPorOrdEquipo(
            Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim,
            Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim,
            strNroOrdenTrabajo,
            Session("IdEmpresa"),
            Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim,
            Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim)

        Dim dtComponentes As New DataTable
        dtComponentes = FuncionesNeg.ConvertirListADataTable(dsComponentes)
        dtComponentes.Columns.Add(New DataColumn("TotalCantidadComponentes", GetType(System.Int32)))
        dtComponentes.Columns.Add(New DataColumn("CantidadComponentesFinalizados", GetType(System.Int32)))
        dtComponentes.Columns.Add(New DataColumn("PorcentajeText", GetType(System.String)))
        dtComponentes.Columns.Add(New DataColumn("Porcentaje", GetType(System.Int32)))
        dtComponentes.Columns.Add(New DataColumn("Tiempo", GetType(System.Int32)))

        For Each Componente In dtComponentes.Rows
            Componente("TotalCantidadComponentes") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "'").Rows(0).Item(0)
            'Componente("CantidadComponentesFinalizados") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
            Componente("CantidadComponentesFinalizados") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') != 'I'").Rows(0).Item(0)
            Componente("PorcentajeText") = IIf(Componente("TotalCantidadComponentes") = 0, "Sin actividad", Math.Round((100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes")), 2) & "%")
            Componente("Porcentaje") = IIf(Componente("TotalCantidadComponentes") = 0, 0, Math.Round(100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes"), 2))
            Componente("Tiempo") = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT ISNULL(SUM(nTotalSegundosTrabajadosComponenteOrdenTrabajo), 0) FROM LOGI_COMPONENTEORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoComponenteOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
        Next
        Return dtComponentes
    End Function


    Sub LlenarData()
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Enabled = True
        txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Enabled = True
        cboTipoMantenimientoMantenimientoOrdenTrabajo.Enabled = True
        cboListadoCheckListMantenimientoOrdenTrabajo.Enabled = True

        Dim EquipoNeg As New clsEquipoNegocios
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(Server.HtmlDecode(row.Cells(9).Text).Trim)

        If (String.IsNullOrEmpty(row.Cells(4).Text)) Then
            txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = Now.ToString("yyyy-MM-dd")
        Else
            Dim dateValueIni As DateTime = DateTime.Parse(row.Cells(4).Text)
            row.Cells(4).Text = dateValueIni.ToString("yyyy-MM-dd")
            txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = row.Cells(4).Text
        End If

        If (String.IsNullOrEmpty(row.Cells(20).Text) Or row.Cells(20).Text.Equals("&nbsp;")) Then
            txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = Now.ToString("yyyy-MM-dd")
        Else
            Dim dateValueTer As DateTime = DateTime.Parse(row.Cells(20).Text)
            row.Cells(20).Text = dateValueTer.ToString("yyyy-MM-dd")
            txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = row.Cells(20).Text
        End If

        hfdIdCatalogoCabecera.Value = Equipo.cIdCatalogo
        hfdIdTipoActivoCabecera.Value = Equipo.cIdTipoActivo
        hfdIdJerarquiaCatalogoCabecera.Value = Equipo.cIdJerarquiaCatalogo

        cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue = row.Cells(14).Text
        cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(cboTipoMantenimientoMantenimientoOrdenTrabajo, New System.EventArgs())

        ListarPlantillaCheckListCombo()

        ListarPersonalResponsableCombo()

        Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                 Session("IdEmpresa"))
        'hfdIdTipoControlTiempo.Value = IIf(Trim(OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo) = "", "C", OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo)
        hfdIdTipoControlTiempo.Value = IIf(String.IsNullOrWhiteSpace(OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo.ToString()), "C", OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo)

        Dim ComponentesEquipoNeg As New clsComponenteOrdenTrabajoNegocios
        Dim dsComponentes = ComponentesEquipoNeg.ComponenteOrdenTrabajoListaGridPorOrdEquipo(Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim,
                                                                                             Session("IdEmpresa"), Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim)
        Dim tieneROF As Boolean = dsComponentes.Any(Function(componente) componente.StatusComponente = "R" OrElse componente.StatusComponente = "F")
        If tieneROF Then
            txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Enabled = False
            txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Enabled = False
            cboTipoMantenimientoMantenimientoOrdenTrabajo.Enabled = False
            cboListadoCheckListMantenimientoOrdenTrabajo.Enabled = False
        End If

        ListarPersonalAsignadoCombo(OrdTra)
        Dim PersonalNeg As New clsOrdenTrabajoNegocios
        Dim result = PersonalNeg.OrdenTrabajoRecursos(OrdTra)

        Dim responsable = result.Find(Function(item) item.bResponsableRecursosOrdenTrabajo = True And item.bEstadoRegistroRecursosOrdenTrabajo = True)
        If (responsable Is Nothing) Then
            cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0
        Else
            cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue = responsable.RRHH_PERSONAL.cIdPersonal
        End If

        cboTipoControlTiempoMantenimientoOrdenTrabajo.SelectedValue = IIf(String.IsNullOrWhiteSpace(OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo.ToString()), "C", OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo)

        VaciarCestaPersonal(Session("CestaSSPersonal"))

        For Each item In result.Where(Function(x) x.bEstadoRegistroRecursosOrdenTrabajo = True)
            AgregarCestaPersonal(item.RRHH_PERSONAL.cIdPersonal.Trim, UCase(item.RRHH_PERSONAL.vNombreCompletoPersonal).Trim, item.bResponsableRecursosOrdenTrabajo, Session("CestaSSPersonal"))
        Next

        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()

        If MyValidator.ErrorMessage = "" Then
            MyValidator.ErrorMessage = "Registro encontrado con éxito"
        End If
        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        MyValidator.IsValid = False
        MyValidator.ID = "ErrorPersonalizado"
        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        Me.Page.Validators.Add(MyValidator)
    End Sub

    Private Sub cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndexChanged
        ListarPlantillaCheckListCombo()
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Sub ListarTipoMantenimientoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim TipoMantenimientoNeg As New clsTipoMantenimientoNegocios
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataTextField = "vDescripcionTipoMantenimiento"
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataValueField = "cIdTipoMantenimiento"
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataSource = TipoMantenimientoNeg.TipoMantenimientoListarCombo("1")
        cboTipoMantenimientoMantenimientoOrdenTrabajo.Items.Clear()
        cboTipoMantenimientoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ListarPlantillaCheckListCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)

        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        cboListadoCheckListMantenimientoOrdenTrabajo.DataTextField = "vDescripcionCabeceraCheckListPlantilla"
        cboListadoCheckListMantenimientoOrdenTrabajo.DataValueField = "cIdNumeroCabeceraCheckListPlantilla"

        Dim valueTMantOT = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
        Dim valueTActCbc = IIf((valueTMantOT.Trim().Equals("MP") And hfdIdTipoActivoCabecera.Value.Trim().Equals("00")), "15", hfdIdTipoActivoCabecera.Value.Trim())
        Dim valueIdCatCbc = IIf((valueTMantOT.Trim().Equals("MP") And (hfdIdCatalogoCabecera.Value.Trim().Equals("00") Or String.IsNullOrEmpty(hfdIdCatalogoCabecera.Value))), "1500036", hfdIdCatalogoCabecera.Value)
        cboListadoCheckListMantenimientoOrdenTrabajo.DataSource = PlantillaCheckListNeg.ChecklistPlantillaListarCombo(valueTMantOT, valueTActCbc, valueIdCatCbc, hfdIdJerarquiaCatalogoCabecera.Value)
        cboListadoCheckListMantenimientoOrdenTrabajo.Items.Clear()
        cboListadoCheckListMantenimientoOrdenTrabajo.DataBind()
        cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = row.Cells(21).Text
    End Sub

    Private Sub cboListadoCheckListMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboListadoCheckListMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Sub ListarPersonalResponsableCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim ResponsableNeg As New clsPersonalNegocios
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataTextField = "vNombreCompletoPersonal"
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataValueField = "cIdPersonal"
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataSource = ResponsableNeg.PersonalListarCombo()
        cboPersonalResponsableMantenimientoOrdenTrabajo.Items.Clear()
        cboPersonalResponsableMantenimientoOrdenTrabajo.Items.Add("SELECCIONE DATO")
        cboPersonalResponsableMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Private Sub cboPersonalResponsableMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndexChanged
        Try
            If cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            End If
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                Session("CestaSSPersonal").Rows(i)("Responsable") = False
            Next
            If grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count > 0 Then
                If grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex < grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count Then
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue.ToString.Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                        End If
                    Next
                End If
            End If
            AgregarCestaPersonal(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, True, Session("CestaSSPersonal"))
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = "Personal agregado satisfactoriamente."
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click

        MyValidator.ErrorMessage = ""

        If grdLista IsNot Nothing Then
            If grdLista.Rows.Count > 0 Then
                If IsNothing(grdLista.SelectedRow) = False Then
                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                        'Dim EquipoNeg As New clsEquipoNegocios
                        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
                        'Dim DNI = Session("vIdNroDocumentoIdentidadUsuario") '"47452331"
                        Dim perfil = Session("IdPerfil")
                        'Dim query = "select  rot.* from [dbo].[LOGI_RECURSOSORDENTRABAJO] rot join [dbo].[RRHH_PERSONAL]  " &
                        '"rp on rot.cIdPersonal = rp.cIdPersonal where cIdTipoDocumentoCabeceraOrdenTrabajo = '" & row.Cells(0).Text & "' and vIdNumeroSerieCabeceraOrdenTrabajo	= '" & row.Cells(1).Text & "' and vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & row.Cells(2).Text & "' " &
                        '"And rp.vNumeroDocumentoPersonal = '" & DNI & "' " &
                        '"And rot.bEstadoRegistroRecursosOrdenTrabajo = " & 1 & " "

                        'Dim PersonalNeg As New clsOrdenTrabajoNegocios
                        'Dim result = PersonalNeg.OrdenTrabajoValidarRecurso(query)
                        If (perfil = "00001") Or (perfil = "00012") Or (perfil = "00007") Or (perfil = "00008") Then
                            LlenarData()
                            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
                        Else
                            If MyValidator.ErrorMessage = "" Then
                                MyValidator.ErrorMessage = "No esta asignado a la Orden de Trabajo " & row.Cells(0).Text & "-" & row.Cells(1).Text & "-" & row.Cells(2).Text & ""
                                ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
                                MyValidator.IsValid = False
                                MyValidator.ID = "ErrorPersonalizado"
                                MyValidator.ValidationGroup = "vgrpValidarBusqueda"
                                Me.Page.Validators.Add(MyValidator)
                                Return
                            End If

                            'Throw New Exception()
                        End If

                        'Dim lnkbtnVerEquipo As LinkButton = TryCast(row.Cells(8).FindControl("lnkbtnVerEquipo"), LinkButton) 'JMUG: 21/09/2023
                        'If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & (Server.HtmlDecode(lnkbtnVerEquipo.Text).Trim) & "'").Rows(0).Item(0) = "0" Then
                        '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                        'End If
                    End If
                Else
                    Throw New Exception("Debe de seleccionar un item.")
                End If
            End If
        End If

    End Sub
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "N"

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            'JMUG: 14/07/2025 Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(3).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            'strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                 strNroOrdenTrabajo,
                                                                 Session("IdEmpresa"))
            ListarPersonalAsignadoCombo(OrdTra)
            hfdOrdenFabricacionReferencia.Value = OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo
            hfdIdUsuarioCreacionOrdenTrabajo.Value = OrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo 'JMUG: 13/11/2023
            LlenarDataEquipo()

            lstComponentes.DataSource = fLlenarGrillaComponentes()
            lstComponentes.DataBind()

            Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios

            ' Suponiendo que tienes una referencia a tu DataList llamada lstComponentes
            Dim primeraFila As DataListItem = lstComponentes.Controls.OfType(Of DataListItem)().FirstOrDefault()
            If primeraFila IsNot Nothing Then
                ' Encontrar el primer LinkButton dentro de la primera fila
                SetColor_lstComponentes(primeraFila.ItemIndex)

                'hfdIdTipoControlTiempo.Value = IIf(Trim(OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo) = "", "C", OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo)
                hfdIdTipoControlTiempo.Value = IIf(String.IsNullOrWhiteSpace(OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo.ToString()), "C", OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo)
                Dim lnkbtnComponente As LinkButton = TryCast(primeraFila.FindControl("lnkbtnComponente"), LinkButton)

                If lnkbtnComponente IsNot Nothing Then
                    ' Realizar acciones con el LinkButton encontrado
                    ' Por ejemplo, acceder a su texto
                    Dim ValoresLinkButton() As String = lnkbtnComponente.CommandArgument.ToString.Split("*")
                    Session("rowSelectedObjectFromComponente") = ValoresLinkButton

                    Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                    (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                    strNroOrdenTrabajo,
                                                                                    Session("IdEmpresa"),
                                                                                    (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                    (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                    ValoresLinkButton(4).ToString)
                    If (ValoresLinkButton(5).ToString.Equals("I") Or ValoresLinkButton(5).ToString.Equals("F")) Then
                        Me.lstCheckList.Enabled = False
                    Else
                        Me.lstCheckList.Enabled = True
                    End If

                    If hfdIdTipoControlTiempo.Value = "D" Then
                        Me.lstCheckList.Enabled = True
                    End If

                    If (ValoresLinkButton(5).ToString.Equals("F")) Then
                        lnkbtnAddActividad.Visible = False
                    Else
                        lnkbtnAddActividad.Visible = True
                    End If
                End If
            End If


            Me.lstCheckList.DataBind()
            CargarStatusComponenteTarea()
            pnlCabecera.Visible = False
            pnlContenido.Visible = True

            If btnNuevo.Text = "Procesar" Then
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET cEstadoCabeceraOrdenTrabajo = 'P' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
            ElseIf btnNuevo.Text = "Ver" Then
                hfdOperacion.Value = "R"
            End If
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

    Sub InsertComponentesAndActividades(OrdTra As LOGI_CABECERAORDENTRABAJO, UsuCorreo As GNRL_USUARIO)
        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        Dim cIdPlantilla = String.Concat(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue.Trim(), cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue)

        Dim result = PlantillaCheckListNeg.ChecklistPlantillaInsertaCopiaComponentes(grdLista.SelectedRow.Cells(9).Text.Trim, cIdPlantilla)


        'Dim dsCheckListPlantilla = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT CABCHKLISPLA.cIdTipoMantenimiento, CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, CABCHKLISPLA.dFechaTransaccionCabeceraCheckListPlantilla, " &
        '                                                                       "       CABCHKLISPLA.bEstadoRegistroCabeceraCheckListPlantilla, CABCHKLISPLA.cIdTipoActivoCabeceraCheckListPlantilla, CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla, " &
        '                                                                       "       CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, DETCHKLISPLA.cIdActividadCheckList, DETCHKLISPLA.vDescripcionDetalleCheckListPlantilla, " &
        '                                                                       "       DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla, DETCHKLISPLA.cIdTipoActivo, DETCHKLISPLA.cIdCatalogo, DETCHKLISPLA.cIdJerarquiaCatalogo, EQU.cIdEquipo " &
        '                                                                       "FROM LOGI_CABECERACHECKLISTPLANTILLA AS CABCHKLISPLA INNER JOIN LOGI_DETALLECHECKLISTPLANTILLA AS DETCHKLISPLA ON " &
        '                                                                       "     CABCHKLISPLA.cIdTipoMantenimiento = DETCHKLISPLA.cIdTipoMantenimiento AND " &
        '                                                                       "     CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla " &
        '                                                                       "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
        '                                                                       "     DETCHKLISPLA.cIdCatalogo = EQU.cIdCatalogo AND " &
        '                                                                       "     DETCHKLISPLA.cIdJerarquiaCatalogo = EQU.cIdJerarquiaCatalogo AND " &
        '                                                                       "     EQU.cIdEnlaceEquipo = '" & grdLista.SelectedRow.Cells(9).Text.Trim & "' AND " &
        '                                                                       "     ISNULL(EQU.bEstadoRegistroEquipo, '1') = '1' " &
        '                                                                       "WHERE CABCHKLISPLA.cIdTipoControlTiempoMantenimiento = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "' AND CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "' " &
        '                                                                       "      AND CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla = '" & hfdIdCatalogoCabecera.Value & "' AND CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0' " &
        '                                                                       "      AND DETCHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '1' " &
        '                                                                       "ORDER BY DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla")
        Dim dsCheckListPlantilla = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT CABCHKLISPLA.cIdTipoMantenimiento, CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla, CABCHKLISPLA.dFechaTransaccionCabeceraCheckListPlantilla, " &
                                                                               "       CABCHKLISPLA.bEstadoRegistroCabeceraCheckListPlantilla, CABCHKLISPLA.cIdTipoActivoCabeceraCheckListPlantilla, CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla, " &
                                                                               "       CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla, DETCHKLISPLA.cIdActividadCheckList, DETCHKLISPLA.vDescripcionDetalleCheckListPlantilla, " &
                                                                               "       DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla, DETCHKLISPLA.cIdTipoActivo, DETCHKLISPLA.cIdCatalogo, DETCHKLISPLA.cIdJerarquiaCatalogo, EQU.cIdEquipo " &
                                                                               "FROM LOGI_CABECERACHECKLISTPLANTILLA AS CABCHKLISPLA INNER JOIN LOGI_DETALLECHECKLISTPLANTILLA AS DETCHKLISPLA ON " &
                                                                               "     CABCHKLISPLA.cIdTipoMantenimiento = DETCHKLISPLA.cIdTipoMantenimiento AND " &
                                                                               "     CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = DETCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla " &
                                                                               "     INNER JOIN LOGI_EQUIPO AS EQU ON " &
                                                                               "     DETCHKLISPLA.cIdCatalogo = EQU.cIdCatalogo AND " &
                                                                               "     DETCHKLISPLA.cIdJerarquiaCatalogo = EQU.cIdJerarquiaCatalogo AND " &
                                                                               "     EQU.cIdEnlaceEquipo = '" & grdLista.SelectedRow.Cells(9).Text.Trim & "' AND " &
                                                                               "     ISNULL(EQU.bEstadoRegistroEquipo, '1') = '1' " &
                                                                               "WHERE CABCHKLISPLA.cIdTipoMantenimiento = '" & cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "' AND CABCHKLISPLA.cIdNumeroCabeceraCheckListPlantilla = '" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "' " &
                                                                               "      AND CABCHKLISPLA.cIdCatalogoCabeceraCheckListPlantilla = '" & hfdIdCatalogoCabecera.Value & "' AND CABCHKLISPLA.cIdJerarquiaCatalogoCabeceraCheckListPlantilla = '0' " &
                                                                               "      AND DETCHKLISPLA.bEstadoRegistroDetalleCheckListPlantilla = '1' " &
                                                                               "ORDER BY DETCHKLISPLA.nIdNumeroItemDetalleCheckListPlantilla")


        Dim ColeccionCheckList As New List(Of LOGI_CHECKLISTORDENTRABAJO)
        For Each CheckListPlantilla In dsCheckListPlantilla.Rows
            Dim ChkList As New LOGI_CHECKLISTORDENTRABAJO
            With ChkList
                .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                .vObservacionCheckListOrdenTrabajo = ""
                .cIdEmpresa = OrdTra.cIdEmpresa
                .cEstadoCheckListOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                .nIdNumeroItemCheckListOrdenTrabajo = CheckListPlantilla("nIdNumeroItemDetalleCheckListPlantilla")
                .nTotalSegundosTrabajadosCheckListOrdenTrabajo = 0
                .dFechaInicioCheckListOrdenTrabajo = Nothing
                .dFechaFinalCheckListOrdenTrabajo = Nothing
                .cIdEquipoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim)
                .cIdEquipoCheckListOrdenTrabajo = CheckListPlantilla("cIdEquipo")
                .cIdActividadCheckListOrdenTrabajo = CheckListPlantilla("cIdActividadCheckList")
                .cIdTipoMantenimientoCheckListOrdenTrabajo = CheckListPlantilla("cIdTipoMantenimiento")
                .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")
                .cIdCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdCatalogo")
                .cIdJerarquiaCatalogoCheckListOrdenTrabajo = CheckListPlantilla("cIdJerarquiaCatalogo")
                .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                .cIdNumeroCabeceraCheckListPlantillaCheckListOrdenTrabajo = cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue
            End With

            OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo = CheckListPlantilla("cIdNumeroCabeceraCheckListPlantilla")

            ColeccionCheckList.Add(ChkList)
        Next
        Dim dsComponenteEquipo = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT EQU.cIdEquipo, EQU.cIdCatalogo, EQU.cIdJerarquiaCatalogo " &
                                                                             "FROM LOGI_EQUIPO AS EQU " &
                                                                             "WHERE EQU.cIdEnlaceEquipo = '" & grdLista.SelectedRow.Cells(9).Text.Trim & "' " &
                                                                             "      AND EQU.cIdJerarquiaCatalogo = '1' " &
                                                                             "      AND EQU.bEstadoRegistroEquipo = '1' " &
                                                                             "      AND EQU.cIdEnlaceCatalogo = '" & hfdIdCatalogoCabecera.Value & "' " &
                                                                             "ORDER BY EQU.vDescripcionEquipo")

        Dim ColeccionComponente As New List(Of LOGI_COMPONENTEORDENTRABAJO)
        For Each ComponenteEquipo In dsComponenteEquipo.Rows
            Dim ComEqu As New LOGI_COMPONENTEORDENTRABAJO
            With ComEqu
                .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                .cIdEmpresa = OrdTra.cIdEmpresa
                .cIdEquipoCabeceraOrdenTrabajo = grdLista.SelectedRow.Cells(9).Text.Trim
                .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                .cIdEquipoComponenteOrdenTrabajo = ComponenteEquipo("cIdEquipo")
                .cIdCatalogoComponenteOrdenTrabajo = ComponenteEquipo("cIdCatalogo")
                .cIdJerarquiaCatalogoComponenteOrdenTrabajo = ComponenteEquipo("cIdJerarquiaCatalogo")
                .nIdNumeroItemComponenteOrdenTrabajo = 0
                .vObservacionComponenteOrdenTrabajo = ""
                .cEstadoComponenteOrdenTrabajo = "I" 'Actividad que se encuentra sin ejecutar.
                .dFechaInicioActividadComponenteOrdenTrabajo = Nothing
                .dFechaFinalActividadComponenteOrdenTrabajo = Nothing
                .nTotalSegundosTrabajadosComponenteOrdenTrabajo = 0
            End With
            ColeccionComponente.Add(ComEqu)
        Next

        If (OrdenTrabajoNeg.OrdenTrabajoDeleteAndInsertComponentes(OrdTra, ColeccionCheckList, ColeccionComponente) = 0) Then
            Dim dtCorreoResponsable = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT PER.cIdPersonal, PER.vEmailPersonal FROM LOGI_RECURSOSORDENTRABAJO AS REC " &
                                                "INNER JOIN RRHH_PERSONAL AS PER ON " &
                                                "REC.cIdPersonal = PER.cIdPersonal AND REC.cIdEmpresa = PER.cIdEmpresa " &
                                                "WHERE REC.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND REC.vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND REC.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND REC.cIdEmpresa = '" & OrdTra.cIdEmpresa & "' " &
                                                "ORDER BY REC.bResponsableRecursosOrdenTrabajo DESC")
            If dtCorreoResponsable.Rows.Count > 0 Then
                If dtCorreoResponsable.Rows(0).Item(1).ToString() = "" Then
                    Throw New Exception("El responsable debe de tener un correo asignado.")
                Else
                    For i = 0 To dtCorreoResponsable.Rows.Count - 1

                        Dim strHTML As String
                        Dim EmpNeg As New clsEmpresaNegocios
                        strHTML = FuncionesNeg.FormatoEnvioGeneral("001", OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "-" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "-" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "*" & String.Format("{0:dd-MM-yyyy}", OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo) & "*" & EmpNeg.EmpresaListarPorId(Session("IdEmpresa")).vDescripcionEmpresa & "*" & UsuCorreo.vLoginUsuario & "*" & UsuCorreo.vPasswordUsuario)
                        Dim strFrom As String = "notificaciones@movitecnica.com.pe"
                        Dim strPwd As String = "NTmvt$1604"
                        Dim strConfiguracionCorreo As String = "smtp.office365.com" & "|" & "587"
                        Dim strConfiguracionPeriodo As String = "" 'String.Format("{0:0000}", Year(Documento.dFechaEmisionCabeceraDocumento)) & "|" & String.Format("{0:00}", Month(Documento.dFechaEmisionCabeceraDocumento))
                        FuncionesNeg.EnviarCorreo(LCase(strFrom), strPwd, LCase(dtCorreoResponsable.Rows(i).Item(1).ToString()), "Orden de Trabajo Generada", strHTML, "", "", strConfiguracionCorreo, strConfiguracionPeriodo, "", False)

                    Next
                End If
            Else
                Throw New Exception("No tiene asignado a ningún responsable para esta orden de trabajo.")
            End If

            OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(1).Text & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & grdLista.SelectedRow.Cells(2).Text & "'")
        End If

    End Sub
    Private Sub btnAceptarMantenimientoOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0676", strOpcionModulo, "CMMS")

            If txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de inicio de planificación del mantenimiento.")
            ElseIf txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de termino de planificación del mantenimiento.")
                'ElseIf cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                '    Throw New Exception("Debe de seleccionar un personal responsable.")
            ElseIf cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = "" Then
                Throw New Exception("Debe de seleccionar una plantilla.")
            End If

            Dim bTieneResponsable As Boolean = False
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Responsable")) = True Then
                    bTieneResponsable = True
                End If
            Next
            If bTieneResponsable = False Then
                Throw New Exception("Debe de seleccionar un personal responsable.")
            End If

            Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios
            Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                 Session("IdEmpresa"))

            Dim RrhhOrdTraBD = OrdenTrabajoNeg.OrdenTrabajoRecursos(OrdTra)

            Dim UsuCorreo As New GNRL_USUARIO

            Dim ColeccionRRHH As New List(Of LOGI_RECURSOSORDENTRABAJO)
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1

                Dim existe As Boolean = RrhhOrdTraBD.Any(Function(p) p.RRHH_PERSONAL.cIdPersonal.Trim = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim))
                If existe Then
                    'update
                    Dim item = RrhhOrdTraBD.Find(Function(p) p.RRHH_PERSONAL.cIdPersonal.Trim = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim))
                    item.bEstadoRegistroRecursosOrdenTrabajo = True
                    item.bResponsableRecursosOrdenTrabajo = (Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim)

                    OrdenTrabajoNeg.OrdenTrabajoUpdateResponsable(item)
                Else
                    'insert
                    Dim RrhhOrdTra As New LOGI_RECURSOSORDENTRABAJO
                    With RrhhOrdTra
                        .cIdTipoDocumentoCabeceraOrdenTrabajo = OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo
                        .vIdNumeroSerieCabeceraOrdenTrabajo = OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo
                        .vIdNumeroCorrelativoCabeceraOrdenTrabajo = OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                        .cIdEmpresa = OrdTra.cIdEmpresa
                        .cIdPersonal = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim)
                        .vObservacionRecursosOrdenTrabajo = ""
                        .bEstadoRegistroRecursosOrdenTrabajo = True
                        .cIdEquipoCabeceraOrdenTrabajo = OrdTra.cIdEquipoCabeceraOrdenTrabajo
                        .nTotalMinutosTrabajadosRecursosOrdenTrabajo = 0
                        .vIdArticuloSAPCabeceraOrdenTrabajo = OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo
                        .bResponsableRecursosOrdenTrabajo = (Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim)

                        If .bResponsableRecursosOrdenTrabajo = True Then
                            Dim dsUsuario = OrdenFabricacionNeg.OrdenFabricacionGetData("SELECT * FROM GNRL_USUARIO WHERE vIdNroDocumentoIdentidadUsuario IN (SELECT vNumeroDocumentoPersonal FROM RRHH_PERSONAL WHERE cIdPersonal = '" & .cIdPersonal & "') AND cIdEmpresa = '" & Session("IdEmpresa") & "'")
                            For Each fila In dsUsuario.Rows
                                With UsuCorreo
                                    UsuCorreo.cIdUsuario = fila("cIdUsuario")
                                    UsuCorreo.vLoginUsuario = fila("vLoginUsuario")
                                    UsuCorreo.vPasswordUsuario = fila("vPasswordUsuario")
                                End With
                            Next
                            If UsuCorreo.vLoginUsuario Is Nothing Then
                                Throw New Exception("El responsable debe de tener un DNI asignado como Usuario.")
                            End If
                        End If
                    End With


                    OrdenTrabajoNeg.OrdenTrabajoInsertResponsable(RrhhOrdTra)
                End If

            Next

            'If (row.Cells(14).Text = OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo And row.Cells(21).Text = OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo) Then
            OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
            OrdTra.dFechaTerminoPlanificadaCabeceraOrdenTrabajo = txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo.Text
            'OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo = cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
            OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo = cboTipoControlTiempoMantenimientoOrdenTrabajo.SelectedValue
            '    'OrdenTrabajoNeg.OrdenTrabajoEdita(OrdTra)
            'Else
            If (OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo <> cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue) Then
                OrdTra.cIdNumeroCabeceraCheckListPlantillaCabeceraOrdenTrabajo = cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue

                InsertComponentesAndActividades(OrdTra, UsuCorreo)

            End If

            OrdenTrabajoNeg.OrdenTrabajoEdita(OrdTra)

            Me.grdLista.DataSource = fLlenarGrilla()
            Me.grdLista.DataBind()
            MyValidator.ErrorMessage = "Transacción registrada con éxito"
            'End If

            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            'JMUG: INICIO - 19/12/2023
                            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
                            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
                            Dim strNroOrdenTrabajo As String = ""
                            strNroOrdenTrabajo = hlnkVerInforme.Text
                            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
                            'JMUG: FINAL - 19/12/2023

                            lnkbtnVerOrdenTrabajo.Attributes.Add("onclick", "javascript:popupEmitirOrdenTrabajoReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text) & "', '" & strNroOrdenTrabajo & "', '" & Session("IdEmpresa") & "');")
                            lnkbtnVerDetalleOrdenTrabajoServicio.Attributes.Add("onclick", "javascript:popupEmitirDetalleOrdenTrabajoServicioReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text) & "');")
                            lnkbtnVerCheckList.Attributes.Add("onclick", "javascript:popupEmitirOrdenTrabajoCheckListReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text) & "', '" & strNroOrdenTrabajo & "', '" & Session("IdEmpresa") & "');")
                            Dim lblEstado As Label = TryCast(grdLista.SelectedRow.Cells(13).FindControl("lblEstado"), Label)
                            If lblEstado.Text = "Terminado" Then
                                BloquearMantenimiento(True, False, True, False)
                                btnNuevo.Text = "Ver"
                                btnEditar.Visible = False
                                lnkbtnFirmarOrdenTrabajo.Visible = True
                            Else
                                btnNuevo.Text = "Procesar"
                                btnEditar.Visible = True
                                BloquearMantenimiento(True, False, True, False)
                                lnkbtnFirmarOrdenTrabajo.Visible = False
                            End If
                            If MyValidator.ErrorMessage = "" Then
                                MyValidator.ErrorMessage = "Registro seleccionado"
                            End If
                        End If
                    End If
                Else
                    'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                End If
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnVerDetalleOrdenTrabajoServicio_Click(sender As Object, e As EventArgs) Handles lnkbtnVerDetalleOrdenTrabajoServicio.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            hfdIdEquipoSAPEquipo.Value = grdLista.SelectedRow.Cells(9).Text 'JMUG: 21/09/2023
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(16).Text) = "True", "1", "0") 'JMUG: 21/09/2023
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

    Private Sub grdLista_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowCreated
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim td As TableCell
            For Each td In e.Row.Cells
                e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Center 'IdTipoDocumento
                e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center 'IdNumeroSerie
                e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Center 'IdNumeroCorrelativo
                e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'hlnkVerInforme
                e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'FechaInicio
                e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Center 'IdCliente
                e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Center 'IdClienteSAP
                e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'RucCliente
                e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'RazonSocial
                e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'IdEquipo
                e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'IdEquipoSAP
                e.Row.Cells.Item(11).HorizontalAlign = HorizontalAlign.Left 'DescripcionEquipo
                e.Row.Cells.Item(12).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
                e.Row.Cells.Item(13).HorizontalAlign = HorizontalAlign.Left 'IdArticuloSAPCabecera
                e.Row.Cells.Item(14).HorizontalAlign = HorizontalAlign.Left 'IdTipoMantenimiento
                'e.Row.Cells.Item(14).HorizontalAlign = HorizontalAlign.Left 'Estado
                e.Row.Cells.Item(15).HorizontalAlign = HorizontalAlign.Left 'Status
                'e.Row.Cells.Item(15).HorizontalAlign = HorizontalAlign.Left 'Estado VisualizarDOC
                e.Row.Cells.Item(16).HorizontalAlign = HorizontalAlign.Left 'lnkDocumentos
                e.Row.Cells.Item(17).HorizontalAlign = HorizontalAlign.Left 'Estado Registro True/False
                e.Row.Cells.Item(18).HorizontalAlign = HorizontalAlign.Left 'Estado Registro Activar / Desactivar
                e.Row.Cells.Item(19).HorizontalAlign = HorizontalAlign.Left 'FechaEmision
                e.Row.Cells.Item(20).HorizontalAlign = HorizontalAlign.Left 'FechaTermino
                e.Row.Cells.Item(21).HorizontalAlign = HorizontalAlign.Left 'IdNumeroCabeceraCheckListPlantilla
            Next
        End If
    End Sub

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = True 'IdNumeroSerie
            e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'hlnkVerInforme
            e.Row.Cells(4).Visible = True 'FechaInicio
            e.Row.Cells(5).Visible = False 'IdCliente
            e.Row.Cells(6).Visible = False 'IdClienteSAP
            e.Row.Cells(7).Visible = True 'RucCliente
            e.Row.Cells(8).Visible = True 'RazonSocial
            e.Row.Cells(9).Visible = True 'IdEquipo
            e.Row.Cells(10).Visible = False 'IdEquipoSAP
            e.Row.Cells(11).Visible = True 'DescripcionEquipo
            e.Row.Cells(12).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(13).Visible = False 'IdArticuloSAPCabecera
            e.Row.Cells(14).Visible = True 'IdTipoMantenimiento
            e.Row.Cells(15).Visible = True 'Status
            'e.Row.Cells(15).Visible = True 'Estado VisualizarDOC
            e.Row.Cells(16).Visible = False 'lnkDocumentos
            e.Row.Cells(17).Visible = False 'Estado Registro True/False
            e.Row.Cells(18).Visible = False 'Estado Registro Activar / Desactivar
            e.Row.Cells(19).Visible = False 'FechaEmision
            e.Row.Cells(20).Visible = False 'FechaTermino
            e.Row.Cells(21).Visible = False 'IdNumeroCabeceraCheckListPlantilla
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = True 'IdNumeroSerie
            e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'hlnkVerInforme
            e.Row.Cells(4).Visible = True 'FechaInicio
            e.Row.Cells(5).Visible = False 'IdCliente
            e.Row.Cells(6).Visible = False 'IdClienteSAP
            e.Row.Cells(7).Visible = True 'RucCliente
            e.Row.Cells(8).Visible = True 'RazonSocial
            e.Row.Cells(9).Visible = True 'IdEquipo
            e.Row.Cells(10).Visible = False 'IdEquipoSAP
            e.Row.Cells(11).Visible = True 'DescripcionEquipo
            e.Row.Cells(12).Visible = True 'NumeroSerieEquipo
            e.Row.Cells(13).Visible = False 'IdArticuloSAPCabecera
            e.Row.Cells(14).Visible = True 'IdTipoMantenimiento
            'e.Row.Cells(14).Visible = True 'Estado
            e.Row.Cells(15).Visible = True 'Status
            'e.Row.Cells(15).Visible = True 'Estado VisualizarDOC
            e.Row.Cells(16).Visible = False 'lnkDocumentos
            e.Row.Cells(17).Visible = False 'Estado Registro True/False
            e.Row.Cells(18).Visible = False 'Estado Registro Activar / Desactivar
            e.Row.Cells(19).Visible = False 'FechaEmision
            e.Row.Cells(20).Visible = False 'FechaTermino
            e.Row.Cells(21).Visible = False 'IdNumeroCabeceraCheckListPlantilla
        End If
    End Sub

    Sub LlenarTarea()
        Dim ahora As DateTime = DateTime.Now

        'Asignar la fecha y hora actual al TextBox de fecha
        txtFechaMantenimientoTarea.Text = ahora.ToString("dd/MM/yyyy")

        'Obtener la hora actual y asignarla a los DropDownList de horas, minutos y segundos
        Dim hora As Integer = ahora.Hour
        Dim minuto As Integer = ahora.Minute
        Dim segundo As Integer = ahora.Second

        cboHorasMantenimientoTarea.SelectedValue = hora.ToString("00")
        cboMinutosMantenimientoTarea.SelectedValue = minuto.ToString("00")
        cboSegundosMantenimientoTarea.SelectedValue = segundo.ToString("00")

        'JMUG: INICIO - 19/12/2023
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
        Dim strNroOrdenTrabajo As String = ""
        strNroOrdenTrabajo = hlnkVerInforme.Text
        strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
        'JMUG: FINAL - 19/12/2023

        If Convert.ToInt16(cboHorasMantenimientoTarea.SelectedValue) >= 12 Then
            cboMeridianoMantenimientoTarea.SelectedValue = "PM"
        Else
            cboMeridianoMantenimientoTarea.SelectedValue = "AM"
        End If
        Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
        If Valores(8).ToString = "I" Then 'Iniciar
            cboPersonalAsignadoMantenimientoTarea.Enabled = True
            txtDescripcionMantenimientoTarea.Enabled = True
            cboPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            txtDescripcionMantenimientoTarea.Text = ""
        ElseIf Valores(8).ToString = "E" Then 'En Proceso
            cboPersonalAsignadoMantenimientoTarea.Enabled = True
            txtDescripcionMantenimientoTarea.Enabled = True
            cboPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            txtDescripcionMantenimientoTarea.Text = ""
        ElseIf Valores(8).ToString = "P" Then 'Pendiente
            Dim dsTarea = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vDescripcionTareaCheckListOrdenTrabajo, cIdPersonal " &
                                                              "FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                                                              "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
            cboPersonalAsignadoMantenimientoTarea.Enabled = False
            txtDescripcionMantenimientoTarea.Enabled = False
            For Each fila In dsTarea.Rows
                cboPersonalAsignadoMantenimientoTarea.SelectedValue = fila("cIdPersonal")
                txtDescripcionMantenimientoTarea.Text = fila("vDescripcionTareaCheckListOrdenTrabajo")
            Next
        ElseIf Valores(8).ToString = "F" Then 'Finalizar
            Dim dsTarea = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vDescripcionTareaCheckListOrdenTrabajo, cIdPersonal " &
                                                              "FROM LOGI_TAREACHECKLISTORDENTRABAJO " &
                                                              "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
            cboPersonalAsignadoMantenimientoTarea.Enabled = False
            txtDescripcionMantenimientoTarea.Enabled = False
            For Each fila In dsTarea.Rows
                cboPersonalAsignadoMantenimientoTarea.SelectedValue = fila("cIdPersonal")
                txtDescripcionMantenimientoTarea.Text = fila("vDescripcionTareaCheckListOrdenTrabajo")
            Next
        End If
    End Sub

    Sub LlenarTareaComponente()
        '0 -> IdCatalogo
        '1 -> IdJerarquiaCatalogo
        '2 -> IdArticuloSAPCabecera
        '3 -> IdEquipo
        '4 -> IdEquipoComponente
        '5 -> I->Iniciar Actividad / P->Pendiente / R->Retomar / F->Finalizar

        VaciarCestaPersonal(Session("CestaSSPersonal"))
        Dim ahora As DateTime = DateTime.Now

        'Asignar la fecha y hora actual al TextBox de fecha
        txtFechaMantenimientoTarea.Text = ahora.ToString("dd/MM/yyyy")

        'Obtener la hora actual y asignarla a los DropDownList de horas, minutos y segundos
        Dim hora As Integer = ahora.Hour
        Dim minuto As Integer = ahora.Minute
        Dim segundo As Integer = ahora.Second

        cboHorasMantenimientoTarea.SelectedValue = hora.ToString("00")
        cboMinutosMantenimientoTarea.SelectedValue = minuto.ToString("00")
        cboSegundosMantenimientoTarea.SelectedValue = segundo.ToString("00")

        'JMUG: INICIO - 19/12/2023
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
        Dim strNroOrdenTrabajo As String = ""
        strNroOrdenTrabajo = hlnkVerInforme.Text
        strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
        'JMUG: FINAL - 19/12/2023

        If Convert.ToInt16(cboHorasMantenimientoTarea.SelectedValue) >= 12 Then
            cboMeridianoMantenimientoTarea.SelectedValue = "PM"
        Else
            cboMeridianoMantenimientoTarea.SelectedValue = "AM"
        End If

        Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")

        If Valores(5).ToString = "I" Then 'Iniciar
            cboPersonalAsignadoMantenimientoTarea.Enabled = True
            txtDescripcionMantenimientoTarea.Enabled = True
            cboPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            txtDescripcionMantenimientoTarea.Text = ""
            lbFechaInicio.Text = ""

        ElseIf Valores(5).ToString = "R" Then 'En Proceso / Retomar
            cboPersonalAsignadoMantenimientoTarea.Enabled = True
            txtDescripcionMantenimientoTarea.Enabled = True
            cboPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            txtDescripcionMantenimientoTarea.Text = ""

            'JMUG: 06/11/2023 NUEVO INICIO
            Dim dsTarea = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT TARCOM.cIdPersonal, PER.vNombreCompletoPersonal " &
                                                  "FROM LOGI_TAREACOMPONENTEORDENTRABAJO AS TARCOM INNER JOIN RRHH_PERSONAL AS PER ON " &
                                                  "     TARCOM.cIdPersonal = PER.cIdPersonal " &
                                                  "WHERE TARCOM.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND TARCOM.vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND TARCOM.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND TARCOM.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                  "      AND TARCOM.cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND TARCOM.cIdEquipoComponenteOrdenTrabajo = '" & Valores(4).ToString & "' AND TARCOM.cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND TARCOM.cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND TARCOM.vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' " &
                                                  "GROUP BY TARCOM.cIdPersonal, PER.vNombreCompletoPersonal")
            For Each fila In dsTarea.Rows
                AgregarCestaPersonal(fila("cIdPersonal"), UCase(Trim(fila("vNombreCompletoPersonal"))), True, Session("CestaSSPersonal"))
                lbFechaInicio.Text = ""
            Next
            'JMUG: 06/11/2023 NUEVO FINAL
        ElseIf Valores(5).ToString = "P" Then 'Pendiente
            Dim dsTarea = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT TARCOM.vDescripcionTareaComponenteOrdenTrabajo, TARCOM.cIdPersonal, PER.vNombreCompletoPersonal, CONVERT(varchar,dFechaInicioTareaComponenteOrdenTrabajo,100) AS [Inicio] " &
                                                              "FROM LOGI_TAREACOMPONENTEORDENTRABAJO AS TARCOM INNER JOIN RRHH_PERSONAL AS PER ON " &
                                                              "     TARCOM.cIdPersonal = PER.cIdPersonal " &
                                                              "WHERE TARCOM.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND TARCOM.vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND TARCOM.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND TARCOM.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                              "      AND TARCOM.cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND TARCOM.cIdEquipoComponenteOrdenTrabajo = '" & Valores(4).ToString & "' AND TARCOM.cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND TARCOM.cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND TARCOM.vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND ISNULL(TARCOM.dFechaFinalTareaComponenteOrdenTrabajo, '') = ''")

            cboPersonalAsignadoMantenimientoTarea.Enabled = False
            txtDescripcionMantenimientoTarea.Enabled = False
            For Each fila In dsTarea.Rows
                AgregarCestaPersonal(fila("cIdPersonal"), UCase(Trim(fila("vNombreCompletoPersonal"))), True, Session("CestaSSPersonal"))
                txtDescripcionMantenimientoTarea.Text = fila("vDescripcionTareaComponenteOrdenTrabajo")
                lbFechaInicio.Text = "Fecha de inicio de tarea: " + fila("Inicio")
            Next
        ElseIf Valores(5).ToString = "F" Then 'Finalizar
            Dim dsTarea = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT TARCOM.vDescripcionTareaComponenteOrdenTrabajo, TARCOM.cIdPersonal, PER.vNombreCompletoPersonal, CONVERT(varchar,dFechaInicioTareaComponenteOrdenTrabajo,100) AS [Inicio] " &
                                                              "FROM LOGI_TAREACOMPONENTEORDENTRABAJO AS TARCOM INNER JOIN RRHH_PERSONAL AS PER ON " &
                                                              "     TARCOM.cIdPersonal = PER.cIdPersonal " &
                                                              "WHERE TARCOM.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND TARCOM.vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND TARCOM.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND TARCOM.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                              "      AND TARCOM.cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND TARCOM.cIdEquipoComponenteOrdenTrabajo = '" & Valores(4).ToString & "' AND TARCOM.cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND TARCOM.cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND TARCOM.vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND ISNULL(TARCOM.dFechaFinalTareaComponenteOrdenTrabajo, '') = ''")
            cboPersonalAsignadoMantenimientoTarea.Enabled = False
            txtDescripcionMantenimientoTarea.Enabled = False
            For Each fila In dsTarea.Rows
                AgregarCestaPersonal(fila("cIdPersonal"), UCase(Trim(fila("vNombreCompletoPersonal"))), True, Session("CestaSSPersonal"))
                txtDescripcionMantenimientoTarea.Text = fila("vDescripcionTareaComponenteOrdenTrabajo")
                lbFechaInicio.Text = "Fecha de inicio de tarea: " + fila("Inicio")
            Next
        End If
        grdPersonalAsignadoMantenimientoTarea.DataSource = Session("CestaSSPersonal")
        grdPersonalAsignadoMantenimientoTarea.DataBind()
    End Sub

    Sub SetColor_lstComponentes(index As Int32)
        For Each item In lstComponentes.Items
            item.BackColor = System.Drawing.Color.Transparent
        Next

        Dim selectedItem As DataListItem = lstComponentes.Items(Convert.ToInt32(index))
        selectedItem.BackColor = System.Drawing.Color.LightGray

        Session("indexValueSave") = Convert.ToInt32(index)
    End Sub

    Protected Sub lstComponentes_RowCommand_Botones(sender As Object, e As DataListCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            '0 -> IdCatalogo
            '1 -> IdJerarquiaCatalogo
            '2 -> IdArticuloSAPCabecera
            '3 -> IdEquipo
            '4 -> IdEquipoComponente

            Dim objValor() As String = e.CommandArgument.ToString.Split("*")
            Session("rowSelectedObjectFromComponente") = objValor

            SetColor_lstComponentes(e.Item.ItemIndex)

            If e.CommandName = "AdicionarTareaComponente" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         strNroOrdenTrabajo,
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                         Valores(4).ToString)

                If (Valores(5).ToString.Equals("I") Or Valores(5).ToString.Equals("F")) Then
                    Me.lstCheckList.Enabled = False
                Else
                    Me.lstCheckList.Enabled = True
                End If

                If hfdIdTipoControlTiempo.Value = "D" Then
                    Me.lstCheckList.Enabled = True
                End If

                If (Valores(5).ToString.Equals("F")) Then
                    lnkbtnAddActividad.Visible = False
                Else
                    lnkbtnAddActividad.Visible = True
                End If

                Me.lstCheckList.DataBind()
            ElseIf e.CommandName = "Iniciar" Then
                If hfdOperacion.Value = "R" Then
                    Throw New Exception("Orden de Trabajo cerrada no se puede modificar.")
                End If
                'Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTareaComponente()
                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
            ElseIf e.CommandName = "Pendiente" Then
                If hfdOperacion.Value = "R" Then
                    Throw New Exception("Orden de Trabajo cerrada no se puede modificar.")
                End If
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTareaComponente()
                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
            ElseIf e.CommandName = "Retomar" Then
                If hfdOperacion.Value = "R" Then
                    Throw New Exception("Orden de Trabajo cerrada no se puede modificar.")
                End If
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTareaComponente()
                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
            ElseIf e.CommandName = "Finalizar" Then
                If hfdOperacion.Value = "R" Then
                    Throw New Exception("Orden de Trabajo cerrada no se puede modificar.")
                End If

                Dim spanPorcentajeText As Label = CType(e.Item.FindControl("lblPorcentajeText"), Label)
                Dim porcentaje As Int32 = Convert.ToInt32(spanPorcentajeText.Text)

                If porcentaje < 100 Then
                    Throw New Exception("Debe completar las actividades")
                End If
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "398", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarTiempoCombo()
                LlenarTareaComponente()
                lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
            End If

        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
            SetColor_lstComponentes(e.Item.ItemIndex)
        End Try
    End Sub

    Protected Sub lstCheckList_RowCommand_Botones(sender As Object, e As DataListCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            If hfdOperacion.Value = "R" Then
                Throw New Exception("Orden de Trabajo cerrada no se puede modificar.")
            End If

            If e.CommandName = "EstadoMalo" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'M', cEstadoCheckListOrdenTrabajo = 'P' " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         strNroOrdenTrabajo,
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                         Valores(7).ToString)
                Me.lstCheckList.DataBind()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado malo."
            ElseIf e.CommandName = "EstadoRegular" Then
                'Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "412", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'R', cEstadoCheckListOrdenTrabajo = 'E'" &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         strNroOrdenTrabajo,
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                         Valores(7).ToString)
                Me.lstCheckList.DataBind()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado regular."
            ElseIf e.CommandName = "EstadoBueno" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'B', cEstadoCheckListOrdenTrabajo = 'F' " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         strNroOrdenTrabajo,
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                         Valores(7).ToString)
                Me.lstCheckList.DataBind()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado bueno."
            ElseIf e.CommandName = "EstadoNoAplica" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoActividadCheckListOrdenTrabajo = 'N', cEstadoCheckListOrdenTrabajo = 'A' " &
                                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

                Dim CheckListNeg As New clsCheckListMetodos
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                         strNroOrdenTrabajo,
                                                                                         Session("IdEmpresa"),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                         Valores(7).ToString)
                Me.lstCheckList.DataBind()
                'JMUG: 07/11/2023 CargarStatusActividad()
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                MyValidator.ErrorMessage = "Se cambió a estado no aplica."
            ElseIf e.CommandName = "AgregarImagenes" Then
                '''Función para validar si tiene permisos
                ''FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                'LimpiarObjetosImagen()
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                'Dim ActNeg As New clsActividadCheckListNegocios
                'Dim Actividad As LOGI_ACTIVIDADCHECKLIST = ActNeg.ActividadCheckListListarPorId(Valores(3).ToString.Trim)
                'Dim EquNeg As New clsEquipoNegocios
                'Dim Equipo As LOGI_EQUIPO = EquNeg.EquipoListarPorId(Valores(4).ToString.Trim)
                'txtTituloSubirImagenActividad.Text = Equipo.vDescripcionEquipo '& " - " & Actividad.vDescripcionActividadCheckList
                'txtDescripcionSubirImagenActividad.Text = Actividad.vDescripcionActividadCheckList
                'lnk_mostrarPanelSubirImagenActividad_ModalPopupExtender.Show()

                'SetColor_lstComponentes(Session("indexValueSave"))
                ''JMUG: INICIO - 19/12/2023
                'Dim fila = grdLista.Rows(grdLista.SelectedRow.RowIndex)
                'Dim hlnkVerInforme_grdLista As HyperLink = TryCast(fila.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
                'Dim strNroOrdenTrabajo As String = ""
                'strNroOrdenTrabajo = hlnkVerInforme.Text
                'strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
                ''JMUG: FINAL - 19/12/2023
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")

                Dim GaleriaNeg As New clsGaleriaCheckListNegocios
                Me.lstGaleriaActividad.DataSource = GaleriaNeg.GaleriaCheckListListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString.Trim & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString.Trim & "' AND cIdEmpresa", Session("IdEmpresa"), "1")
                Me.lstGaleriaActividad.DataBind()

                btnAgregarGaleriaImagenActividad.Visible = True
                lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender.Show()
            ElseIf e.CommandName = "AgregarInsumos" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                Dim DetActNeg As New clsActividadCheckListNegocios
                lblDescripcionActividadAgregarInsumos.Text = DetActNeg.ActividadCheckListListarPorId(Valores(3).ToString.Trim).vDescripcionActividadCheckList
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                ListarInsumosCombo()
                Dim DetOrdTra As New clsDetalleOrdenTrabajoNegocios
                CargarCestaInsumosTemporal()
                lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
            ElseIf e.CommandName = "AgregarDatosAdicionales" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                Dim EquipoNeg As New clsEquipoNegocios
                Dim DetActNeg As New clsActividadCheckListNegocios
                lblDescripcionEquipoCaracteristicaOtrosDatos.Text = EquipoNeg.EquipoListarPorId(Valores(4).ToString.Trim).vDescripcionEquipo
                lblDescripcionActividadCaracteristicaOtrosDatos.Text = DetActNeg.ActividadCheckListListarPorId(Valores(3).ToString.Trim).vDescripcionActividadCheckList
                ListarOtrosDatosCombo()
                CargarCestaOtrosDatos()
                grdDetalleCaracteristicaOtrosDatos.DataSource = Session("CestaOtrosDatos")
                grdDetalleCaracteristicaOtrosDatos.DataBind()
                lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
            ElseIf e.CommandName = "AgregarObservacion" Then
                ''Función para validar si tiene permisos
                'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdValoresCheckList.Value = e.CommandArgument.ToString
                Dim EquipoNeg As New clsEquipoNegocios
                Dim CheckListNeg As New clsCheckListNegocios
                lblDescripcionEquipoMantenimientoActividad.Text = EquipoNeg.EquipoListarPorId(Valores(4).ToString.Trim).vDescripcionEquipo
                lblDescripcionComponenteMantenimientoActividad.Text = EquipoNeg.EquipoListarPorIdDetalle(Valores(7).ToString.Trim, "").vDescripcionEquipo
                Dim dtCheckList = CheckListNeg.CheckListGetData("SELECT CHKORDTRA.*, ACTCHECKLIST.vDescripcionActividadCheckList, EQU.vDescripcionEquipo, CAT.cIdSistemaFuncional, SISFUN.vDescripcionSistemaFuncional, EQU.cIdTipoActivo, TIPACT.vDescripcionTipoActivo, CAT.vDescripcionCatalogo, ISNULL(CHKORDTRA.cEstadoActividadCheckListOrdenTrabajo, '') AS cEstadoActividadCheckListOrdenTrabajo " &
                                                                "FROM LOGI_CHECKLISTORDENTRABAJO AS CHKORDTRA INNER JOIN LOGI_ACTIVIDADCHECKLIST AS ACTCHECKLIST ON CHKORDTRA.cIdActividadCheckListOrdenTrabajo = ACTCHECKLIST.cIdActividadCheckList INNER JOIN LOGI_EQUIPO AS EQU ON CHKORDTRA.cIdEquipoCabeceraOrdenTrabajo = EQU.cIdEquipo LEFT JOIN LOGI_CATALOGO AS CAT ON CHKORDTRA.cIdCatalogoCheckListOrdenTrabajo = CAT.cIdCatalogo AND CHKORDTRA.cIdJerarquiaCatalogoCheckListOrdenTrabajo = CAT.cIdJerarquiaCatalogo LEFT JOIN LOGI_SISTEMAFUNCIONAL AS SISFUN ON EQU.cIdSistemaFuncionalEquipo = SISFUN.cIdSistemaFuncional LEFT JOIN LOGI_TIPOACTIVO AS TIPACT ON EQU.cIdTipoActivo = TIPACT.cIdTipoActivo " &
                                                                "WHERE CHKORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim & "' AND CHKORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' " &
                                                                "      AND CHKORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND CHKORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                                "      AND CHKORDTRA.cIdEquipoCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim & "' AND CHKORDTRA.vIdArticuloSAPCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim & "' " &
                                                                "      AND CHKORDTRA.cIdEquipoCheckListOrdenTrabajo = '" & Valores(7).ToString.Trim & "' AND CHKORDTRA.cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString.Trim & "'")
                txtObservacionMantenimientoActividad.Text = dtCheckList.Rows(0).Item("vObservacionCheckListOrdenTrabajo")
                lnk_mostrarPanelMantenimientoActividad_ModalPopupExtender.Show()
            End If
            lstComponentes.DataSource = fLlenarGrillaComponentes()
            lstComponentes.DataBind()
            SetColor_lstComponentes(Session("indexValueSave"))
            CargarStatusComponenteTarea()
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAceptarSubirImagenActividad_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirImagenActividad.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            If Not (fupSubirImagenActividad.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            'Se verifica que la extensión sea de un formato válido
            'Hay métodos más seguros para esto, como revisar los bytes iniciales del objeto, pero aquí estamos aplicando lo más sencillos
            Dim ext As String = fupSubirImagenActividad.PostedFile.FileName 'fileUploader1.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            Dim formatos() As String = New String() {"jpg", "jpeg", "bmp", "png", "gif"}
            If (Array.IndexOf(formatos, ext) < 0) Then Throw New Exception("Formato de imagen inválido.")
            Dim Size As Integer = 0
            If Not (Integer.TryParse("160", Size)) Then
                Throw New Exception("El tamaño indicado para la imagen no es válido.")
            End If
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList

            Dim GaleriaChkLst As New LOGI_GALERIACHECKLISTORDENTRABAJO 'LOGI_GALERIAEQUIPO
            GaleriaChkLst.cIdTipoDocumentoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim)
            GaleriaChkLst.vIdNumeroSerieCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim)
            GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo = strNroOrdenTrabajo
            GaleriaChkLst.cIdEmpresa = Session("IdEmpresa")
            GaleriaChkLst.cIdEquipoCabeceraOrdenTrabajo = Valores(4).ToString
            GaleriaChkLst.cIdActividadCheckListOrdenTrabajo = Valores(3).ToString
            GaleriaChkLst.cIdCatalogoCheckListOrdenTrabajo = Valores(0).ToString
            GaleriaChkLst.cIdJerarquiaCatalogoCheckListOrdenTrabajo = Valores(1).ToString
            GaleriaChkLst.vIdArticuloSAPCabeceraOrdenTrabajo = Valores(2).ToString
            GaleriaChkLst.cIdEquipoCheckListOrdenTrabajo = Valores(7).ToString
            GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo = 0
            GaleriaChkLst.vTituloGaleriaCheckListOrdenTrabajo = UCase(txtTituloSubirImagenActividad.Text.Trim)
            GaleriaChkLst.vDescripcionGaleriaCheckListOrdenTrabajo = UCase(txtDescripcionSubirImagenActividad.Text.Trim)
            GaleriaChkLst.vObservacionGaleriaCheckListOrdenTrabajo = UCase(txtObservacionSubirImagenActividad.Text.Trim)
            GaleriaChkLst.dFechaTransaccionGaleriaCheckListOrdenTrabajo = Now
            GaleriaChkLst.bEstadoRegistroGaleriaCheckListOrdenTrabajo = True
            Dim strExtension As String = Mid(fupSubirImagenActividad.PostedFile.FileName.ToString, InStrRev(fupSubirImagenActividad.PostedFile.FileName.ToString, ".", -1) + 1, 4)
            GaleriaChkLst.vNombreArchivoGaleriaCheckListOrdenTrabajo = Trim(UCase(GaleriaChkLst.cIdTipoDocumentoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.vIdNumeroSerieCabeceraOrdenTrabajo & "-" & GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString) & "." & strExtension)

            Dim ColeccionGaleria As New List(Of LOGI_GALERIACHECKLISTORDENTRABAJO)

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

            Dim GaleriaChkLstNeg As New clsGaleriaCheckListNegocios
            If GaleriaChkLstNeg.GaleriaCheckListInserta(GaleriaChkLst) = 0 Then
                'Se guardará en carpeta o en base de datos, según lo indicado en el formulario
                Dim FuncionesNeg As New clsFuncionesNegocios
                'FuncionesNeg.GuardarArchivo(fupSubirImagenActividad.PostedFile, True, "Imagenes\Actividad", Trim(UCase(GaleriaChkLst.cIdEquipoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString)), True, "", 1200, 160)
                FuncionesNeg.GuardarArchivo(fupSubirImagenActividad.PostedFile, True, "Imagenes\Actividad", Trim(UCase(GaleriaChkLst.cIdTipoDocumentoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.vIdNumeroSerieCabeceraOrdenTrabajo & "-" & GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "-" & GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo.ToString)), True, "", 1200, 160)

                Session("Query") = "PA_LOGI_MNT_GALERIACHECKLISTORDENTRABAJO 'SQL_INSERT', '','" & GaleriaChkLst.nIdNumeroItemGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.cIdTipoDocumentoCabeceraOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.vIdNumeroSerieCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.cIdEmpresa & "', '" &
                                                          GaleriaChkLst.cIdEquipoCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.cIdActividadCheckListOrdenTrabajo & "', '" & GaleriaChkLst.cIdCatalogoCheckListOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.cIdJerarquiaCatalogoCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vIdArticuloSAPCabeceraOrdenTrabajo & "', '" & GaleriaChkLst.cIdEquipoCheckListOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.vDescripcionGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vObservacionGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vTituloGaleriaCheckListOrdenTrabajo & "', '" &
                                                          GaleriaChkLst.bEstadoRegistroGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.dFechaTransaccionGaleriaCheckListOrdenTrabajo & "', '" & GaleriaChkLst.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "'"

                LogAuditoria.vEvento = "INSERTAR GALERIA CHECK LIST"
                LogAuditoria.vQuery = Session("Query")
                LogAuditoria.cIdSistema = Session("IdSistema")
                LogAuditoria.cIdModulo = strOpcionModulo

                FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                MyValidator.ErrorMessage = "Transacción registrada con éxito"

                BloquearMantenimiento(True, False, True, False)
                SetColor_lstComponentes(Session("indexValueSave"))
            End If
            ValidationSummary1.ValidationGroup = "vgrpValidarSubirImagenActividad"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenActividad"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary5.ValidationGroup = "vgrpValidarSubirImagenActividad"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirImagenActividad"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirImagenActividad_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub lnkbtnVerGaleria_Click(sender As Object, e As EventArgs) Handles lnkbtnVerGaleria.Click
        SetColor_lstComponentes(Session("indexValueSave"))
        'JMUG: INICIO - 19/12/2023
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        Dim strNroOrdenTrabajo As String = ""
        strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
        'JMUG: FINAL - 19/12/2023

        Dim GaleriaNeg As New clsGaleriaCheckListNegocios
        Me.lstGaleriaActividad.DataSource = GaleriaNeg.GaleriaCheckListListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa", Session("IdEmpresa"), "1")
        Me.lstGaleriaActividad.DataBind()

        btnAgregarGaleriaImagenActividad.Visible = False
        lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender.Show()
    End Sub
    Private Sub lnkbtnVerActividades_Click(sender As Object, e As EventArgs) Handles lnkbtnAddActividad.Click
        SetColor_lstComponentes(Session("indexValueSave"))
        lnk_mostrarPanelAddActividades_ModalPopupExtender.Show()
    End Sub

    Protected Sub lstGaleriaActividad_ItemCommand(source As Object, e As DataListCommandEventArgs)
        If e.CommandName = "VerFotoGaleria" Then
            lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender.Hide()
            Dim i = e.Item.ItemIndex
            Dim hfdUrlGaleriaActividad As HiddenField = TryCast(lstGaleriaActividad.Items(i).FindControl("hfdUrlGaleriaActividad"), HiddenField)
            imgVerImagenActividad.ImageUrl = "~\Imagenes\Actividad\" & hfdUrlGaleriaActividad.Value 'Puede ser ahora JPG/JPEG/PNG/etc.
            lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender.Show()
            lnk_mostrarPanelVerImagenActividad_ModalPopupExtender.Show()
        ElseIf e.CommandName = "Eliminar" Then
            Dim Valores() As String = e.CommandArgument.ToString.Split("*")
            Dim GaleriaNeg As New clsGaleriaCheckListNegocios
            'GaleriaNeg.GaleriaCheckListGetData("UPDATE LOGI_GALERIACHECKLISTORDENTRABAJO
            '                                    SET bEstadoRegistroGaleriaCheckListOrdenTrabajo = 0 
            '                                    WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND 
            '                                          vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND 
            '                                          vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
            '                                          cIdEmpresa = '" & Session("IdEmpresa") & "' AND 
            '                                          nIdNumeroItemGaleriaCheckListOrdenTrabajo = " & Mid(Valores(0).ToString.Trim.Split("-")(3), 1, InStrRev(Valores(0).ToString.Trim.Split("-")(3), ".") - 1) & " AND
            '                                          cIdCatalogoCheckListOrdenTrabajo = '" & hfdValoresCheckList.Value.ToString.Split("*")(0) & "'
            '                                   ")

            GaleriaNeg.GaleriaCheckListGetData("UPDATE LOGI_GALERIACHECKLISTORDENTRABAJO
                                                SET bEstadoRegistroGaleriaCheckListOrdenTrabajo = 0 
                                                WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND 
                                                      vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND 
                                                      vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
                                                      cIdEmpresa = '" & Session("IdEmpresa") & "' AND 
                                                      nIdNumeroItemGaleriaCheckListOrdenTrabajo = " & Mid(Valores(0).ToString.Trim.Split("-")(3), 1, InStrRev(Valores(0).ToString.Trim.Split("-")(3), ".") - 1) & " 
                                               ")

            'Me.lstGaleriaActividad.DataSource = GaleriaNeg.GaleriaCheckListListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdActividadCheckListOrdenTrabajo = '" & hfdValoresCheckList.Value.ToString.Split("*")(3).ToString.Trim & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & hfdValoresCheckList.Value.ToString.Split("*")(0).ToString.Trim & "' AND cIdEmpresa", Session("IdEmpresa"), "1")
            Me.lstGaleriaActividad.DataSource = GaleriaNeg.GaleriaCheckListListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa", Session("IdEmpresa"), "1")
            Me.lstGaleriaActividad.DataBind()

            lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender.Show()
        End If
    End Sub


    Private Sub btnAdicionarActividadCatalogoComponente_Click(sender As Object, e As EventArgs) Handles btnAdicionarActividadCatalogoComponente.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0649", strOpcionModulo, "CMMS")

            For i = 0 To Session("CestaActividadCatalogoComponente").Rows.Count - 1
                If (Session("CestaActividadCatalogoComponente").Rows(i)("Codigo").ToString.Trim) = (cboActividadCatalogoComponente.SelectedValue.ToString.Trim) And Session("CestaActividadCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim = "1501678" Then
                    Throw New Exception("Actividad ya registrada, seleccione otro item.")
                    Exit Sub
                End If
            Next

            If cboActividadCatalogoComponente.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar una actividad del catálogo.")
                Exit Sub
            Else
                Dim componente = Session("rowSelectedObjectFromComponente")
                AgregarCestaActividad(cboActividadCatalogoComponente.SelectedValue.Trim, UCase(cboActividadCatalogoComponente.SelectedItem.Text).Trim, componente(0), "1", "1", lblDescripcionEquipo.Text, Session("CestaActividadCatalogoComponente"))

            End If
            Me.grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponente")
            Me.grdDetalleActividadCatalogoComponente.DataBind()
            cboActividadCatalogoComponente.SelectedIndex = -1
            grdDetalleActividadCatalogoComponente.SelectedIndex = -1
            MyValidator.ErrorMessage = "Actividad agregada con éxito."
            ValidationSummary4.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleActividadCatalogoComponente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleActividadCatalogoComponente.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "489", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            For Each row As GridViewRow In grdDetalleActividadCatalogoComponente.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleActividadCatalogoComponente"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaActividadCatalogoComponente").Rows.Count - 1
                            If (Session("CestaActividadCatalogoComponente").Rows(i)("Codigo").ToString.Trim) = row.Cells(3).Text.ToString.Trim And (Session("CestaActividadCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim) = row.Cells(5).Text.ToString.Trim Then
                                clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaActividadCatalogoComponente"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            Me.grdDetalleActividadCatalogoComponente.DataSource = Session("CestaActividadCatalogoComponente")
            Me.grdDetalleActividadCatalogoComponente.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleActividadCatalogoComponente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleActividadCatalogoComponente.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleActividadCatalogoComponente
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = False 'Codigo
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = False 'IdJerarquia
            e.Row.Cells(7).Visible = True 'DescripcionComponente
            'e.Row.Cells(8).Visible = True 'Ubic
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Seleccionar
            e.Row.Cells(1).Visible = True 'chkRowDetalleActividadCatalogoComponente
            e.Row.Cells(2).Visible = True 'Item
            e.Row.Cells(3).Visible = False 'Codigo
            e.Row.Cells(4).Visible = True 'Descripcion
            e.Row.Cells(5).Visible = False 'IdCatalogo
            e.Row.Cells(6).Visible = False 'IdJerarquia
            e.Row.Cells(7).Visible = True 'DescripcionComponente
            'e.Row.Cells(8).Visible = True 'Ubic
        End If
    End Sub

    Sub CargarCestaOtrosDatos()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaOtrosDatos(Session("CestaOtrosDatos"))

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            Dim OtrosDatosNeg As New clsOtrosDatosOrdenTrabajoNegocios

            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            Dim dsOtrosDatos = OtrosDatosNeg.OtrosDatosOrdenTrabajoGetData("SELECT OTRDATORDTRA.*, " &
                                                                "     CAR.vDescripcionCaracteristica " &
                                                                "FROM LOGI_OTROSDATOSORDENTRABAJO AS OTRDATORDTRA " &
                                                                "INNER JOIN LOGI_CARACTERISTICA AS CAR ON " &
                                                                "      OTRDATORDTRA.cIdCaracteristica = CAR.cIdCaracteristica " &
                                                                "WHERE OTRDATORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "'" &
                                                                "      AND OTRDATORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' " &
                                                                "      AND OTRDATORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' " &
                                                                "      AND OTRDATORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                                "      AND OTRDATORDTRA.cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "'" &
                                                                "      AND OTRDATORDTRA.vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'" &
                                                                "      AND OTRDATORDTRA.cIdEquipoCheckListOrdenTrabajo = '" & Valores(7).ToString & "'")
            For Each OtrosDatos In dsOtrosDatos.Rows
                AgregarCestaOtrosDatos(OtrosDatos("cIdCaracteristica"), OtrosDatos("vDescripcionCaracteristica"), OtrosDatos("vValorReferencialOtrosDatosOrdenTrabajo"), OtrosDatos("cIdEquipoCabeceraOrdenTrabajo"),
                                    OtrosDatos("cIdActividadCheckListOrdenTrabajo"), OtrosDatos("cIdCatalogoCheckListOrdenTrabajo"), OtrosDatos("cIdJerarquiaCatalogoCheckListOrdenTrabajo"),
                                    OtrosDatos("vIdArticuloSAPCabeceraOrdenTrabajo"), OtrosDatos("cIdEquipoCheckListOrdenTrabajo"), Session("CestaOtrosDatos"))
            Next
        Catch ex As Exception
            ValidationSummary7.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaInsumos()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            VaciarCestaInsumos(Session("CestaInsumos"))

            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            Dim InsumosNeg As New clsDetalleOrdenTrabajoNegocios
            Dim sentenciaSQL = "SELECT DETORDTRA.* " &
                                                                "" &
                                                                "FROM LOGI_DETALLEORDENTRABAJO AS DETORDTRA " &
                                                                "WHERE DETORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "'" &
                                                                "      AND DETORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' " &
                                                                "      AND DETORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' " &
                                                                "      AND DETORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "' "
            Dim dsInsumos = OrdenTrabajoNeg.OrdenTrabajoGetData(sentenciaSQL)
            For Each Insumos In dsInsumos.Rows
                AgregarCestaInsumos(Insumos("vIdArticuloSAPDetalleOrdenTrabajo"), Insumos("vDescripcionArticuloDetalleOrdenTrabajo"), Insumos("nCantidadArticuloDetalleOrdenTrabajo"), Insumos("vDescripcionUnidadMedidaDetalleOrdenTrabajo"),
                                    Insumos("cIdEquipoCabeceraOrdenTrabajo"), Insumos("cIdCatalogoCheckListDetalleOrdenTrabajo"), Insumos("cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo"), Insumos("cIdActividadCheckListDetalleOrdenTrabajo"), Insumos("vIdArticuloSAPCabeceraOrdenTrabajo"), Session("CestaInsumos"))
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

    Sub CargarCestaInsumosTemporal()
        'Carga las opciones en la Grilla de todos los Elementos asignados a ese Perfil.
        Try
            VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            Dim resultInsumosSimple As DataRow() = Session("CestaInsumos").Select("IdCatalogo = '" & Valores(0).ToString & "' AND IdJerarquia = '" & Valores(1).ToString & "' AND IdActividad = '" & Valores(3).ToString & "' AND IdArticuloSAP = '" & Valores(2).ToString & "'")
            If resultInsumosSimple.Length = 0 Then
                VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
            Else
                Dim rowFil As DataRow() = resultInsumosSimple
                For Each Insumos As DataRow In rowFil
                    AgregarCestaInsumos(Insumos("Codigo"), Insumos("Descripcion"), Insumos("Cantidad"), Insumos("DescripcionUnidadMedida"),
                                Insumos("IdEquipo"), Insumos("IdCatalogo"), Insumos("IdJerarquia"), Insumos("IdActividad"), Insumos("IdArticuloSAP"), Session("CestaInsumosFiltrado"))
                Next
            End If
            Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
            Me.grdDetalleAgregarInsumos.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub txtCantidadDetalle_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleAgregarInsumos.Rows
                Dim txtCantidadDetalle As TextBox = TryCast(row.Cells(4).FindControl("txtCantidadDetalle"), TextBox)
                Dim hfdRowNumber As HiddenField = TryCast(row.Cells(4).FindControl("hfdRowNumber"), HiddenField)
                Dim FilaActual As Int16
                'FilaActual = row.RowIndex + (grdDetalleAgregarInsumos.Rows.Count * (grdDetalleAgregarInsumos.PageIndex))
                FilaActual = hfdRowNumber.Value
                Session("CestaInsumosFiltrado").Rows(FilaActual)("Cantidad") = txtCantidadDetalle.Text

                For i = 0 To Session("CestaInsumos").Rows.Count - 1
                    If Session("CestaInsumos").Rows(i)("IdCatalogo") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdCatalogo") And
                       Session("CestaInsumos").Rows(i)("IdJerarquia") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdJerarquia") And
                       Session("CestaInsumos").Rows(i)("IdActividad") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdActividad") And
                       Session("CestaInsumos").Rows(i)("IdEquipo") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdEquipo") And
                       Session("CestaInsumos").Rows(i)("IdArticuloSAP") = Session("CestaInsumosFiltrado").Rows(FilaActual)("IdArticuloSAP") And
                       Session("CestaInsumos").Rows(i)("Codigo") = Session("CestaInsumosFiltrado").Rows(FilaActual)("Codigo") Then
                        EditarCestaInsumos(Session("CestaInsumosFiltrado").Rows(FilaActual)("Codigo"), Session("CestaInsumosFiltrado").Rows(FilaActual)("Descripcion"), Session("CestaInsumosFiltrado").Rows(FilaActual)("Cantidad"), Session("CestaInsumosFiltrado").Rows(FilaActual)("DescripcionUnidadMedida"),
                                            Session("CestaInsumosFiltrado").Rows(FilaActual)("IdEquipo"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdCatalogo"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdJerarquia"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdActividad"), Session("CestaInsumosFiltrado").Rows(FilaActual)("IdArticuloSAP"), Session("CestaInsumos"), i)
                    End If
                Next
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

    Private Sub btnAdicionarInsumos_Click(sender As Object, e As EventArgs) Handles btnAdicionarInsumos.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))
            If cboInsumosAgregarInsumos.SelectedIndex = 0 Then
                lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
                Throw New Exception("Debe de seleccionar algún item.")
            End If
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
            For i = 0 To Session("CestaInsumos").Rows.Count - 1
                If (Session("CestaInsumos").Rows(i)("Codigo").ToString.Trim) = (cboInsumosAgregarInsumos.SelectedValue.ToString.Trim) And Session("CestaInsumos").Rows(i)("IdCatalogo").ToString.Trim = Valores(0).ToString.Trim Then
                    Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
                    Me.grdDetalleAgregarInsumos.DataBind()
                    lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
                    Throw New Exception("Característica ya registrada, seleccione otro item.")
                End If
            Next

            Dim InsumosNeg As New clsOrdenFabricacionNegocios 'clsDetalleOrdenTrabajoNegocios
            Dim strUnidadMedida = ""
            strUnidadMedida = Trim(InsumosNeg.OrdenFabricacionListarPorIdDetalle(ValoresOS(0).Trim, ValoresOS(1).Trim, ValoresOS(2).Trim, Session("IdEmpresa"), Valores(2).ToString.Trim, cboInsumosAgregarInsumos.SelectedValue).vDescripcionUnidadMedidaSAPDetalleOrdenFabricacion)
            AgregarCestaInsumos(cboInsumosAgregarInsumos.SelectedValue, cboInsumosAgregarInsumos.SelectedItem.Text, 0, strUnidadMedida, Valores(4).ToString, Valores(0).ToString, Valores(1).ToString, Valores(3).ToString, Valores(2).ToString, Session("CestaInsumos"))
            AgregarCestaInsumos(cboInsumosAgregarInsumos.SelectedValue, cboInsumosAgregarInsumos.SelectedItem.Text, 0, strUnidadMedida, Valores(4).ToString, Valores(0).ToString, Valores(1).ToString, Valores(3).ToString, Valores(2).ToString, Session("CestaInsumosFiltrado"))
            Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
            Me.grdDetalleAgregarInsumos.DataBind()
            cboInsumosAgregarInsumos.SelectedIndex = -1
            grdDetalleAgregarInsumos.SelectedIndex = -1
            MyValidator.ErrorMessage = "Insumo agregado con éxito."
            ValidationSummary6.ValidationGroup = "vgrpValidarAgregarInsumos"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarAgregarInsumos"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary6.ValidationGroup = "vgrpValidarAgregarInsumos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarAgregarInsumos"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleAgregarInsumos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDetalleAgregarInsumos.RowDeleting
        Try
            MyValidator.ErrorMessage = ""
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            For Each row As GridViewRow In grdDetalleAgregarInsumos.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleAgregarInsumos"), CheckBox)
                    If chkRow.Checked Then
                        For i = 0 To Session("CestaInsumos").Rows.Count - 1
                            If (Session("CestaInsumos").Rows(i)("IdCatalogo").ToString.Trim) = Valores(0).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdJerarquia").ToString.Trim) = Valores(1).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdActividad").ToString.Trim) = Valores(3).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdEquipo").ToString.Trim) = Valores(4).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("IdArticuloSAP").ToString.Trim) = Valores(2).ToString.Trim And
                                    (Session("CestaInsumos").Rows(i)("Codigo").ToString.Trim) = row.Cells(2).Text.ToString.Trim Then
                                QuitarCestaInsumos(i, Session("CestaInsumos"))
                                Exit For
                            End If
                        Next

                        For i = 0 To Session("CestaInsumosFiltrado").Rows.Count - 1
                            If (Session("CestaInsumosFiltrado").Rows(i)("Codigo").ToString.Trim) = row.Cells(2).Text.ToString.Trim Then
                                QuitarCestaInsumos(i, Session("CestaInsumosFiltrado"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
            Me.grdDetalleAgregarInsumos.DataBind()
            lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdDetalleAgregarInsumos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleAgregarInsumos.PageIndexChanging
        grdDetalleAgregarInsumos.PageIndex = e.NewPageIndex
        Me.grdDetalleAgregarInsumos.DataSource = Session("CestaInsumosFiltrado")
        Me.grdDetalleAgregarInsumos.DataBind() 'Recargo el grid.
        grdDetalleAgregarInsumos.SelectedIndex = -1
        lnk_mostrarPanelAgregarInsumos_ModalPopupExtender.Show()
    End Sub

    Sub txtValorDetalleCaracteristicaOtrosDatos_TextChanged(sender As Object, e As EventArgs)
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            For Each row As GridViewRow In grdDetalleCaracteristicaOtrosDatos.Rows
                Dim txtValorDetalle As TextBox = TryCast(row.Cells(5).FindControl("txtValorDetalle"), TextBox)
                Dim hfdRowNumber As HiddenField = TryCast(row.Cells(5).FindControl("hfdRowNumber"), HiddenField)
                Dim FilaActual As Int16
                FilaActual = hfdRowNumber.Value
                Session("CestaOtrosDatos").Rows(FilaActual)("ValorReferencial") = txtValorDetalle.Text
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

    Protected Sub btnAceptarOtrosDatos_Click(sender As Object, e As EventArgs) Handles btnAceptarOtrosDatos.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "142", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            Dim Coleccion As New List(Of LOGI_OTROSDATOSORDENTRABAJO)
            For i = 0 To Session("CestaOtrosDatos").Rows.Count - 1
                Dim OtrDatOT As New LOGI_OTROSDATOSORDENTRABAJO
                OtrDatOT.cIdTipoDocumentoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim)
                OtrDatOT.vIdNumeroSerieCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim)
                OtrDatOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = strNroOrdenTrabajo
                OtrDatOT.cIdEmpresa = Session("IdEmpresa")
                OtrDatOT.cIdEquipoCabeceraOrdenTrabajo = Valores(4).ToString
                OtrDatOT.nIdNumeroItemOtrosDatosOrdenTrabajo = i + 1
                OtrDatOT.cIdCatalogoCheckListOrdenTrabajo = Valores(0).ToString
                OtrDatOT.cIdJerarquiaCatalogoCheckListOrdenTrabajo = Valores(1).ToString
                OtrDatOT.cIdActividadCheckListOrdenTrabajo = Valores(3).ToString
                OtrDatOT.vIdArticuloSAPCabeceraOrdenTrabajo = Valores(2).ToString
                OtrDatOT.cIdEquipoCheckListOrdenTrabajo = Session("CestaOtrosDatos").Rows(i)("IdEquipoCheckList").ToString.Trim
                OtrDatOT.vValorReferencialOtrosDatosOrdenTrabajo = Session("CestaOtrosDatos").Rows(i)("ValorReferencial").ToString.Trim
                OtrDatOT.cIdCaracteristica = Session("CestaOtrosDatos").Rows(i)("Codigo").ToString.Trim
                Coleccion.Add(OtrDatOT)
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

            Dim OtrosDatosOrdenTrabajoNeg As New clsOtrosDatosOrdenTrabajoNegocios
            If OtrosDatosOrdenTrabajoNeg.OtrosDatosOrdenTrabajoInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
                MyValidator.ErrorMessage = "Transacción registrada con éxito"
                CargarCestaInsumos()
            End If

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

    Private Sub btnAceptarAgregarInsumos_Click(sender As Object, e As EventArgs) Handles btnAceptarAgregarInsumos.Click
        'Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
        'Valores(0).ToString - IdCatalogo
        'Valores(1).ToString - IdJerarquiaCatalogo
        'Valores(2).ToString - IdArticuloSAPCabecera
        'Valores(3).ToString - IdActividad
        'Valores(4).ToString - IdEquipo
        'Valores(5).ToString - IdSistemaFuncional
        'Valores(6).ToString - IdTipoActivo
        'Valores(7).ToString - IdEquipoCheckList
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "142", strOpcionModulo, Session("IdSistema"), Session("IdArea"))

            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            Dim Coleccion As New List(Of LOGI_DETALLEORDENTRABAJO)
            For i = 0 To Session("CestaInsumos").Rows.Count - 1
                Dim DetOT As New LOGI_DETALLEORDENTRABAJO
                DetOT.cIdTipoDocumentoCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim)
                DetOT.vIdNumeroSerieCabeceraOrdenTrabajo = (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim)
                DetOT.vIdNumeroCorrelativoCabeceraOrdenTrabajo = strNroOrdenTrabajo
                DetOT.cIdEmpresa = Session("IdEmpresa")
                DetOT.cIdEquipoCabeceraOrdenTrabajo = Valores(4).ToString
                DetOT.nIdNumeroItemDetalleOrdenTrabajo = i + 1
                DetOT.cIdCatalogoCheckListDetalleOrdenTrabajo = Valores(0).ToString
                DetOT.cIdJerarquiaCatalogoCheckListDetalleOrdenTrabajo = Valores(1).ToString
                DetOT.cIdActividadCheckListDetalleOrdenTrabajo = Valores(3).ToString
                DetOT.vIdArticuloSAPCabeceraOrdenTrabajo = Valores(2).ToString
                DetOT.vIdArticuloSAPDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("Codigo").ToString.Trim
                DetOT.vDescripcionArticuloDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("Descripcion").ToString.Trim
                DetOT.nCantidadArticuloDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("Cantidad").ToString.Trim
                DetOT.vDescripcionUnidadMedidaDetalleOrdenTrabajo = Session("CestaInsumos").Rows(i)("DescripcionUnidadMedida").ToString.Trim
                Coleccion.Add(DetOT)
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
            'Esto lo coloco antes porque el LogAuditoria lo estoy mandando en un solo bloque.
            LogAuditoria.vEvento = "INSERTAR TIPO DE ACTIVO"
            LogAuditoria.vQuery = Session("Query")
            LogAuditoria.cIdSistema = Session("IdSistema")
            LogAuditoria.cIdModulo = strOpcionModulo

            If DetalleOrdenTrabajoNeg.DetalleOrdenTrabajoInsertaDetalle(Coleccion, LogAuditoria) = 0 Then
                CargarCestaInsumos()
                MyValidator.ErrorMessage = "Transacción registrada con éxito"
            End If
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAdicionarCaracteristicaOtrosDatos_Click(sender As Object, e As EventArgs) Handles btnAdicionarCaracteristicaOtrosDatos.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""

            If hfdOperacion.Value = "N" Or hfdOperacion.Value = "E" Then
                If MyValidator.ErrorMessage = "" Then
                    For i = 0 To Session("CestaOtrosDatos").Rows.Count - 1
                        If (Session("CestaOtrosDatos").Rows(i)("Codigo").ToString.Trim) = (cboCaracteristicaOtrosDatos.SelectedValue.ToString.Trim) Then
                            grdDetalleCaracteristicaOtrosDatos.DataSource = Session("CestaOtrosDatos")
                            grdDetalleCaracteristicaOtrosDatos.DataBind()
                            LimpiarObjetosOtrosDatos()
                            lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
                            grdDetalleCaracteristicaOtrosDatos.SelectedIndex = -1
                            Throw New Exception("Característica ya registrada, seleccione otro item.")
                        End If
                    Next
                    Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
                    AgregarCestaOtrosDatos(cboCaracteristicaOtrosDatos.SelectedValue, cboCaracteristicaOtrosDatos.SelectedItem.Text, "",
                                           Valores(4).ToString, Valores(3).ToString, Valores(0).ToString, Valores(1).ToString, Valores(2).ToString,
                                           Valores(7).ToString, Session("CestaOtrosDatos"))
                    Me.grdDetalleCaracteristicaOtrosDatos.DataSource = Session("CestaOtrosDatos")
                    Me.grdDetalleCaracteristicaOtrosDatos.DataBind()
                    LimpiarObjetosOtrosDatos()
                    lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
                    grdDetalleCaracteristicaOtrosDatos.SelectedIndex = -1
                    MyValidator.ErrorMessage = "Caracteristica agregada con éxito."
                Else
                    lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
                End If
            End If
            ValidationSummary7.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelOtrosDatos_ModalPopupExtender.Show()
            ValidationSummary7.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnAceptarAddActividades_Click(sender As Object, e As EventArgs) Handles btnAceptarAddActividades.Click
        '0 -> IdCatalogo
        '1 -> IdJerarquiaCatalogo
        '2 -> IdArticuloSAPCabecera
        '3 -> IdEquipo
        '4 -> IdEquipoComponente
        '5 -> I->Iniciar Actividad / P->Pendiente / R->Retomar / F->Finalizar
        'Session("rowSelectedObjectFromComponente")ValoresLinkButton(5).ToString
        Try

            If (Session("CestaActividadCatalogoComponente").Rows.Count > 0) Then
                Dim componente = Session("rowSelectedObjectFromComponente")
                For i = 0 To Session("CestaActividadCatalogoComponente").Rows.Count - 1
                    OrdenTrabajoNeg.OrdenTrabajoAddActividad((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim), (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim), (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                             "", Session("IdEmpresa").ToString(), "I",
                                                             0, Date.Now,
                                                             Date.Now, componente(3).ToString(), Session("CestaActividadCatalogoComponente").Rows(i)("Codigo").ToString.Trim,
                                                             "", "", componente(0).ToString(),
                                                             "1", componente(2).ToString(), "", componente(4).ToString())
                Next

                Dim CheckListNeg As New clsCheckListMetodos
                Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                    (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                    (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                                    Session("IdEmpresa"),
                                                                                    (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                    (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                    componente(4).ToString)
                Me.lstCheckList.DataBind()
            End If

            VaciarCestaActividad(Session("CestaActividadCatalogoComponente"))
            Me.grdDetalleActividadCatalogoComponente.DataBind()

            SetColor_lstComponentes(Session("indexValueSave"))


            ValidationSummary11.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)

        Catch ex As Exception
            lnk_mostrarPanelAddActividades_ModalPopupExtender.Show()
            ValidationSummary11.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)
        End Try

    End Sub

    Private Sub btnCancelarAddActividades_Click(sender As Object, e As EventArgs) Handles btnCancelarAddActividades.Click
        Try
            SetColor_lstComponentes(Session("indexValueSave"))
            VaciarCestaActividad(Session("CestaActividadCatalogoComponente"))

            Me.grdDetalleActividadCatalogoComponente.DataBind()

            ValidationSummary11.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lnk_mostrarPanelAddActividades_ModalPopupExtender.Show()
            ValidationSummary11.ValidationGroup = "vgrpValidarOtrosDatos"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarOtrosDatos"
            Me.Page.Validators.Add(MyValidator)
        End Try

    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Me.grdLista.DataSource = fLlenarGrilla()
        Me.grdLista.DataBind()
        pnlCabecera.Visible = True
        pnlContenido.Visible = False
        BloquearMantenimiento(True, False, True, False)
    End Sub

    Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Me.grdLista.DataSource = fLlenarGrilla()
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Private Sub cboHorasMantenimientoTarea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHorasMantenimientoTarea.SelectedIndexChanged
        If Convert.ToInt16(cboHorasMantenimientoTarea.SelectedValue) >= 12 Then
            cboMeridianoMantenimientoTarea.SelectedValue = "PM"
        Else
            cboMeridianoMantenimientoTarea.SelectedValue = "AM"
        End If
    End Sub

    Private Sub lnkbtnTerminarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnTerminarOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                 Session("IdEmpresa"))
                            hfdIdTipoControlTiempo.Value = IIf(String.IsNullOrWhiteSpace(OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo.ToString()), "C", OrdTra.cIdTipoControlTiempoCabeceraOrdenTrabajo)
                            Dim Valida
                            If hfdIdTipoControlTiempo.Value = "C" Then 'Por Componente
                                Valida = OrdenTrabajoNeg.OrdenTrabajoGetData("
                                SELECT COUNT(*) FROM LOGI_TAREACOMPONENTEORDENTRABAJO 
                                WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND
                                cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
                                vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
                                vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' 
                                ")

                            ElseIf hfdIdTipoControlTiempo.Value = "D" Then 'Por Orden de Trabajo
                                Valida = OrdenTrabajoNeg.OrdenTrabajoGetData("
                                SELECT COUNT(*) FROM LOGI_DETALLETAREAORDENTRABAJO 
                                WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND
                                cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
                                vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
                                vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' 
                                ")
                            End If
                            If Valida.Rows(0).Item(0) = 0 Then
                                Throw New Exception("No puede terminar la OT sin registrar mínimo una tarea.")
                            End If
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "R"
            BloquearMantenimiento(True, False, False, False) 'JMUG: 15/11/2023
            btnNuevo.Text = "Ver"
            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET cEstadoCabeceraOrdenTrabajo = 'T', dFechaTerminadaCabeceraOrdenTrabajo = GETDATE() WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")

            Dim strOrdenFabricacion As String = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vOrdenFabricacionReferenciaCabeceraOrdenTrabajo FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ").Rows(0).Item(0).ToString
            OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'T' WHERE cIdTipoDocumentoCabeceraOrdenFabricacion + '-' + vIdNumeroSerieCabeceraOrdenFabricacion + '-' + vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & strOrdenFabricacion & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'")

            If OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_COLAINFORME WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim) & "' AND vIdOrdenTrabajoReferencialColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "-" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "-" & strNroOrdenTrabajo & "'").Rows(0).Item(0) = 0 Then
                OrdenTrabajoNeg.OrdenTrabajoGetData("INSERT INTO LOGI_COLAINFORME (cIdEmpresa, cIdEquipoColaInforme, dFechaTransaccionColaInforme, cEstadoColaInforme, vIdOrdenTrabajoReferencialColaInforme) " &
                                                    "VALUES ('" & Session("IdEmpresa") & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim) & "', GETDATE(), 'R', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "-" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "-" & strNroOrdenTrabajo & "')")
            Else
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_COLAINFORME SET dFechaTransaccionColaInforme = GETDATE(), cEstadoColaInforme = 'P' " &
                                                    "WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim) & "' AND vIdOrdenTrabajoReferencialColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "-" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "-" & strNroOrdenTrabajo & "' ")
            End If

            MyValidator.ErrorMessage = "Orden de Trabajo por Servicio Terminada"
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)

            Me.grdLista.DataSource = fLlenarGrilla()
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnRegenerarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnRegenerarOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            MyValidator.ErrorMessage = "Orden de Trabajo por Servicio Terminada"
                        End If
                    Else
                        Throw New Exception("Debe de seleccionar un item.")
                    End If
                End If
            End If
            hfdOperacion.Value = "R"
            BloquearMantenimiento(True, False, False, False) 'JMUG: 15/11/2023

            btnNuevo.Text = "Ver"
            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            If OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT COUNT(*) FROM LOGI_COLAINFORME WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim) & "' AND vIdOrdenTrabajoReferencialColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "-" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "-" & strNroOrdenTrabajo & "'").Rows(0).Item(0) > 0 Then
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_COLAINFORME SET dFechaTransaccionColaInforme = GETDATE(), cEstadoColaInforme = 'P' " &
                                                    "WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim) & "' AND vIdOrdenTrabajoReferencialColaInforme = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "-" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "-" & strNroOrdenTrabajo & "' ")
            End If

            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)

            Me.grdLista.DataSource = fLlenarGrilla()
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub lnkbtnComponente_Command(sender As Object, e As CommandEventArgs)

    End Sub

    Private Sub cboPersonalAsignadoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAceptarMantenimientoTarea_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoTarea.Click
        Try
            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            If txtFechaMantenimientoTarea.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la fecha de mantenimiento.")
            ElseIf grdPersonalAsignadoMantenimientoTarea.Rows.Count <= 0 Then
                Throw New Exception("Debe de adicionar el personal asignado al mantenimiento.")
            End If

            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")

            '0 -> IdCatalogo
            '1 -> IdJerarquiaCatalogo
            '2 -> IdArticuloSAPCabecera
            '3 -> IdEquipo
            '4 -> IdEquipoComponente
            '5 -> I->Iniciar Actividad / P->Pendiente / R->Retomar / F->Finalizar

            'Obtener los valores seleccionados de los combos
            Dim hora As String = cboHorasMantenimientoTarea.SelectedValue
            Dim minutos As String = cboMinutosMantenimientoTarea.SelectedValue
            Dim segundos As String = cboSegundosMantenimientoTarea.SelectedValue
            Dim fecha As String = txtFechaMantenimientoTarea.Text

            'Concatenar los valores en una cadena en formato SQL
            Dim strFechaHoraAtencion As String = fecha & " " & hora & ":" & minutos & ":" & segundos

            If Valores(5).ToString = "I" Or Valores(5).ToString = "R" Then
                For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                    OrdenTrabajoNeg.OrdenTrabajoGetData("INSERT LOGI_TAREACOMPONENTEORDENTRABAJO (nIdNumeroItemTareaComponenteOrdenTrabajo, dFechaInicioTareaComponenteOrdenTrabajo, dFechaFinalTareaComponenteOrdenTrabajo, cIdPersonal, vDescripcionTareaComponenteOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, cIdEquipoCabeceraOrdenTrabajo, cIdCatalogoComponenteOrdenTrabajo, cIdJerarquiaCatalogoComponenteOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo, cIdEquipoComponenteOrdenTrabajo) " &
                                                    "VALUES (" & "(SELECT ISNULL(MAX(nIdNumeroItemTareaComponenteOrdenTrabajo), 0) + 1 " &
                                                    "FROM LOGI_TAREACOMPONENTEORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "')" & ", " &
                                                    "'" & strFechaHoraAtencion & "', NULL, '" & Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim & "', '" & UCase(txtDescripcionMantenimientoTarea.Text.Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "', '" & strNroOrdenTrabajo & "', '" & Session("IdEmpresa") & "', '" & Valores(3).ToString & "', '" & Valores(0).ToString & "', '" & Valores(1).ToString & "', '" & Valores(2).ToString & "', '" & Valores(4).ToString & "')")

                Next

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_COMPONENTEORDENTRABAJO SET cEstadoComponenteOrdenTrabajo = 'R' " & IIf(Valores(5).ToString = "I", ", dFechaInicioActividadComponenteOrdenTrabajo = '" & strFechaHoraAtencion & "' ", "") &
                                                    "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' " &
                                                    "      AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
                Me.lstCheckList.Enabled = True
            ElseIf Valores(5).ToString = "F" Or Valores(5).ToString = "P" Then
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_TAREACOMPONENTEORDENTRABAJO SET dFechaFinalTareaComponenteOrdenTrabajo = '" & strFechaHoraAtencion & "' " &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND dFechaFinalTareaComponenteOrdenTrabajo IS NULL")

                Dim SegundosTotales As Decimal = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT SUM(ISNULL(DATEDIFF(SECOND, dFechaInicioTareaComponenteOrdenTrabajo, dFechaFinalTareaComponenteOrdenTrabajo), 0)) AS nSegundos FROM LOGI_TAREACOMPONENTEORDENTRABAJO " &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'").Rows(0).Item(0)

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_COMPONENTEORDENTRABAJO SET cEstadoComponenteOrdenTrabajo = '" & IIf(Valores(5).ToString = "F", "F", IIf(Valores(5).ToString = "P", "P", "R")) & "', nTotalSegundosTrabajadosComponenteOrdenTrabajo = " & SegundosTotales & " " & IIf(Valores(5).ToString = "F", ", dFechaFinalActividadComponenteOrdenTrabajo = '" & strFechaHoraAtencion & "' ", "") &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoComponenteOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoComponenteOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
                Me.lstCheckList.Enabled = False 'no usar si tiene actividades por completar estado
            End If
            Dim CheckListNeg As New clsCheckListMetodos
            Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                     strNroOrdenTrabajo,
                                                                                     Session("IdEmpresa"),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                     Valores(4).ToString)
            Me.lstCheckList.DataBind()

            lstComponentes.DataSource = fLlenarGrillaComponentes()
            lstComponentes.DataBind()
            SetColor_lstComponentes(Session("indexValueSave"))
            CargarStatusComponenteTarea()
            pnlCabecera.Visible = False
            pnlContenido.Visible = True
        Catch ex As Exception
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoTarea"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTarea"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoActividad_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoActividad.Click
        Try
            'JMUG: INICIO - 19/12/2023
            Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
            'JMUG: 19/07/2025 Dim hlnkVerInforme As HyperLink = TryCast(row.Cells(2).FindControl("hlnkVerInforme"), HyperLink)
            Dim strNroOrdenTrabajo As String = ""
            'JMUG: 19/07/2025 strNroOrdenTrabajo = hlnkVerInforme.Text
            strNroOrdenTrabajo = grdLista.SelectedRow.Cells(2).Text
            'JMUG: FINAL - 19/12/2023

            If txtObservacionMantenimientoActividad.Text.Trim = "" Then
                Throw New Exception("Debe de ingresar la observación.")
            End If

            'Valores(0).ToString - IdCatalogo
            'Valores(1).ToString - IdJerarquiaCatalogo
            'Valores(2).ToString - IdArticuloSAPCabecera
            'Valores(3).ToString - IdActividad
            'Valores(4).ToString - IdEquipo
            'Valores(5).ToString - IdSistemaFuncional
            'Valores(6).ToString - IdTipoActivo
            'Valores(7).ToString - IdEquipoCheckList
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")

            OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET vObservacionCheckListOrdenTrabajo = '" & UCase(txtObservacionMantenimientoActividad.Text.Trim) & "' " &
                                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' " &
                                                "      AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' " &
                                                "      AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "' " &
                                                "      AND cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                "      AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' " &
                                                "      AND cIdEquipoCheckListOrdenTrabajo = '" & Valores(7).ToString & "' " &
                                                "      AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' " &
                                                "      AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' " &
                                                "      AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' " &
                                                "      AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")

            Dim CheckListNeg As New clsCheckListMetodos
            Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                                     strNroOrdenTrabajo,
                                                                                     Session("IdEmpresa"),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim),
                                                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim),
                                                                                     Valores(7).ToString)
            Me.lstCheckList.DataBind()
            CargarStatusComponenteTarea()
            pnlCabecera.Visible = False
            pnlContenido.Visible = True
        Catch ex As Exception
            ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoTarea"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTarea"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAdicionarPersonalAsignadoMantenimientoTarea_Click(sender As Object, e As EventArgs) Handles btnAdicionarPersonalAsignadoMantenimientoTarea.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If grdPersonalAsignadoMantenimientoTarea.Rows.Count > 0 Then
                If grdPersonalAsignadoMantenimientoTarea.SelectedIndex < grdPersonalAsignadoMantenimientoTarea.Rows.Count Then
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalAsignadoMantenimientoTarea.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                        End If
                    Next
                End If
            End If

            If cboPersonalAsignadoMantenimientoTarea.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal.")
                Exit Sub
            Else
                AgregarCestaPersonal(cboPersonalAsignadoMantenimientoTarea.SelectedValue.Trim, UCase(cboPersonalAsignadoMantenimientoTarea.SelectedItem.Text).Trim, False, Session("CestaSSPersonal"))
            End If
            Me.grdPersonalAsignadoMantenimientoTarea.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoTarea.DataBind()
            cboPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            grdPersonalAsignadoMantenimientoTarea.SelectedIndex = -1
            MyValidator.ErrorMessage = "Personal agregado con éxito."
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoTarea"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTarea"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoTarea"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTarea"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMantenimientoTarea_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPersonalAsignadoMantenimientoTarea.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMantenimientoTarea.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                    QuitarCestaPersonal(i, Session("CestaSSPersonal"))
                    Exit For
                End If
            Next
            Me.grdPersonalAsignadoMantenimientoTarea.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoTarea.DataBind()
            lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Protected Sub grdLista_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles grdLista.RowCommand
        Try
            If e.CommandName = "Activar" Then
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim OrdTra As New LOGI_CABECERAORDENTRABAJO

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = Valores(0).ToString()
                OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = Valores(1).ToString()
                OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = Valores(2).ToString()

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET bEstadoRegistroCabeceraOrdenTrabajo = 0 WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CABECERAORDENTRABAJO SET bEstadoRegistroCabeceraOrdenTrabajo = 0 WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "DESACTIVAR CABECERA ORDEN TRABAJO"
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

                Me.grdLista.DataSource = fLlenarGrilla()
                Me.grdLista.DataBind()
            ElseIf e.CommandName = "Desactivar" Then 'Utilizado para enviar como modificado cuando no recepcionó correctamente la Sunat
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                Dim OrdTra As New LOGI_CABECERAORDENTRABAJO

                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = Valores(0).ToString()
                OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = Valores(1).ToString()
                OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = Valores(2).ToString()

                Dim LogAuditoria As New GNRL_LOGAUDITORIA
                LogAuditoria.cIdPaisOrigen = Session("IdPais")
                LogAuditoria.cIdEmpresa = Session("IdEmpresa")
                LogAuditoria.cIdLocal = Session("IdLocal")
                'LogAuditoria.cIdPuntoVenta = Session("IdPuntoVenta")
                LogAuditoria.cIdUsuario = Session("IdUsuario")
                LogAuditoria.vIP1 = Session("IP1")
                LogAuditoria.vIP2 = Session("IP2")
                LogAuditoria.vPagina = Session("URL")
                LogAuditoria.vSesion = Session("IdSesion")

                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET bEstadoRegistroCabeceraOrdenTrabajo = 1 WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'") ' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'")
                Session("Query") = "UPDATE LOGI_CABECERAORDENTRABAJO SET bEstadoRegistroCabeceraOrdenTrabajo = 1 WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'" '"' AND cIdEmpresa = '" & Producto.cIdEmpresa & "' AND cIdPuntoVenta = '" & Producto.cIdPuntoVenta & "'"
                LogAuditoria.vEvento = "ACTIVAR CABECERA ORDEN TRABAJO"
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

                Me.grdLista.DataSource = fLlenarGrilla()
                Me.grdLista.DataBind()
            ElseIf e.CommandName = "VerDocumentos" Then 'Utilizado para ver los documentos asignados a ese equipo
                'Obtener la URL del documento desde el CommandArgument o el DataKey
                'Dim documentUrl As String = e.CommandArgument.ToString()
                Dim Valores() As String = e.CommandArgument.ToString.Split(",")

                'Configurar la URL del iframe
                grdListaVerDocumentosEquipo.DataSource = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vTituloDocumentacionEquipo AS Titulo, vDescripcionDocumentacionEquipo AS Descripcion, vNombreArchivoDocumentacionEquipo AS NombreArchivo " &
                                                                                             "FROM LOGI_DOCUMENTACIONEQUIPO " &
                                                                                             "WHERE cIdEquipo = '" & Valores(3).ToString() & "'")
                grdListaVerDocumentosEquipo.DataBind()

                'Mostrar el modal
                lnk_mostrarPanelVerDocumentosEquipo_ModalPopupExtender.Show()
            End If
            If e.CommandName = "EliminarDocumento" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split(",")
                OrdenTrabajoNeg.OrdenTrabajoGetData("UPDATE LOGI_DOCUMENTACIONORDENTRABAJO SET bEstadoRegistroDocumentacionOrdenTrabajo = 0 WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Valores(3).ToString & "' AND nIdNumeroItemDocumentacionOrdenTrabajo = '" & Valores(0).ToString & "'")
                grdListaVerDocumentosOrdenTrabajo.DataSource = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vTituloDocumentacionOrdenTrabajo AS Titulo, vDescripcionDocumentacionOrdenTrabajo AS Descripcion, CONVERT(VARCHAR(20),dFechaCarga,103)+' '+CONVERT(VARCHAR(20),dFechaCarga,108) AS Carga, vNombreArchivoDocumentacionOrdenTrabajo AS NombreArchivo, cIdTipoDocumentoCabeceraOrdenTrabajo AS Tipo, vIdNumeroSerieCabeceraOrdenTrabajo AS Serie, vIdNumeroCorrelativoCabeceraOrdenTrabajo AS Correlativo, nIdNumeroItemDocumentacionOrdenTrabajo AS Item " &
                                                                                             "FROM LOGI_DOCUMENTACIONORDENTRABAJO " &
                                                                                             "WHERE bEstadoRegistroDocumentacionOrdenTrabajo=1 and cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Valores(1).ToString & "' and vIdNumeroSerieCabeceraOrdenTrabajo='" & Valores(2).ToString & "' and vIdNumeroCorrelativoCabeceraOrdenTrabajo='" & Valores(3).ToString & "' ")
                grdListaVerDocumentosOrdenTrabajo.DataBind()
                divDetalleVerDocumentosOrdenTrabajo.Visible = False
                lnk_mostrarPanelSubirDocumentacionOrdenTrabajo_ModalPopupExtender.Show()
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

    Private Sub imgbtnBuscarOrdenTrabajo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarOrdenTrabajo.Click
        Me.grdLista.DataSource = fLlenarGrilla()
        Me.grdLista.DataBind()
    End Sub

    Sub ValidarTextoSubirDocumentacionOrdenTrabajo(ByVal bValidar As Boolean)
        Me.rfvTituloSubirDocumentacionOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvDescripcionSubirDocumentacionOrdenTrabajo.EnableClientScript = bValidar

    End Sub

    Sub lnkbtnSubirDocumentacionOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnSubirDocumentacionOrdenTrabajo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(0).Text) = True Then
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                ValidarTextoSubirDocumentacionOrdenTrabajo(True)
                                txtTituloSubirDocumentacionOrdenTrabajo.Text = ""
                                txtDescripcionSubirDocumentacionOrdenTrabajo.Text = ""

                                pnlCabecera.Visible = True
                                pnlContenido.Visible = False
                                ValidationSummary1.ValidationGroup = "vgrpValidarSubirDocumentacionOrdenTrabajo"

                                grdListaVerDocumentosOrdenTrabajo.DataSource = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vTituloDocumentacionOrdenTrabajo AS Titulo, vDescripcionDocumentacionOrdenTrabajo AS Descripcion, CONVERT(VARCHAR(20),dFechaCarga,103)+' '+CONVERT(VARCHAR(20),dFechaCarga,108) AS Carga, vNombreArchivoDocumentacionOrdenTrabajo AS NombreArchivo, cIdTipoDocumentoCabeceraOrdenTrabajo AS Tipo, vIdNumeroSerieCabeceraOrdenTrabajo AS Serie, vIdNumeroCorrelativoCabeceraOrdenTrabajo AS Correlativo, nIdNumeroItemDocumentacionOrdenTrabajo AS Item " &
                                                                                             "FROM LOGI_DOCUMENTACIONORDENTRABAJO " &
                                                                                             "WHERE bEstadoRegistroDocumentacionOrdenTrabajo=1 and cIdTipoDocumentoCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "' and vIdNumeroSerieCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(1).Text & "' and vIdNumeroCorrelativoCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(2).Text & "' ")
                                grdListaVerDocumentosOrdenTrabajo.DataBind()
                                divDetalleVerDocumentosOrdenTrabajo.Visible = False
                                lnk_mostrarPanelSubirDocumentacionOrdenTrabajo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione una orden de trabajo para agregar documentación.")
                    End If
                Else
                    Throw New Exception("Seleccione una orden de trabajo.")
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

    Private Sub btnAgregarVerDocumentosOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAgregarVerDocumentosOrdenTrabajo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                ValidarTextoSubirDocumentacionOrdenTrabajo(True)
                                txtTituloSubirDocumentacionOrdenTrabajo.Text = ""
                                txtDescripcionSubirDocumentacionOrdenTrabajo.Text = ""
                                pnlCabecera.Visible = True
                                pnlContenido.Visible = False
                                ValidationSummary1.ValidationGroup = "vgrpValidarSubirDocumentacionOrdenTrabajo"

                                divDetalleVerDocumentosOrdenTrabajo.Visible = True
                                lnk_mostrarPanelSubirDocumentacionOrdenTrabajo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione una orden de trabajo para agregar documentación.")
                    End If
                Else
                    Throw New Exception("Seleccione una orden de trabajo.")
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

    Private Sub btnAceptarSubirDocumentacionOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAceptarSubirDocumentacionOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If Not (fupSubirDocumentacionOrdenTrabajo.HasFile) Then
                Throw New Exception("Seleccione un archivo del disco duro.")
            End If

            Dim LogAuditoria As New GNRL_LOGAUDITORIA
            LogAuditoria.cIdPaisOrigen = Session("IdPais")
            LogAuditoria.cIdEmpresa = Session("IdEmpresa")
            LogAuditoria.cIdLocal = Session("IdLocal")
            LogAuditoria.cIdUsuario = Session("IdUsuario")
            LogAuditoria.vIP1 = Session("IP1")
            LogAuditoria.vIP2 = Session("IP2")
            LogAuditoria.vPagina = Session("URL")
            LogAuditoria.vSesion = Session("IdSesion")

            Dim sentenciaSQL As String
            Dim ext As String = fupSubirDocumentacionOrdenTrabajo.PostedFile.FileName
            ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower()

            sentenciaSQL = "INSERT INTO LOGI_DOCUMENTACIONORDENTRABAJO (nIdNumeroItemDocumentacionOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, vTituloDocumentacionOrdenTrabajo, vDescripcionDocumentacionOrdenTrabajo, bEstadoRegistroDocumentacionOrdenTrabajo, vNombreArchivoDocumentacionOrdenTrabajo, dFechaCarga) " &
                                    "VALUES ((SELECT ISNULL(MAX(nIdNumeroItemDocumentacionOrdenTrabajo), 0 ) + 1 FROM LOGI_DOCUMENTACIONORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "' and vIdNumeroSerieCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(1).Text & "' and vIdNumeroCorrelativoCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(2).Text & "' ), '" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', '" & UCase(Trim(txtTituloSubirDocumentacionOrdenTrabajo.Text)) & "', '" & UCase(Trim(txtDescripcionSubirDocumentacionOrdenTrabajo.Text)) & "', 1, '', GETDATE())"
            OrdenTrabajoNeg.OrdenTrabajoGetData(sentenciaSQL)

            sentenciaSQL = "SELECT MAX(nIdNumeroItemDocumentacionOrdenTrabajo) FROM LOGI_DOCUMENTACIONORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "' and vIdNumeroSerieCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(1).Text & "' and vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(2).Text & "' "
            Dim Item As Int16 = OrdenTrabajoNeg.OrdenTrabajoGetData(sentenciaSQL).Rows(0).Item(0)
            sentenciaSQL = "UPDATE LOGI_DOCUMENTACIONORDENTRABAJO SET vNombreArchivoDocumentacionOrdenTrabajo = '" & Trim(UCase(grdLista.SelectedRow.Cells(0).Text & grdLista.SelectedRow.Cells(1).Text & "C" & grdLista.SelectedRow.Cells(2).Text & "-" & Item.ToString)) & "." & ext & "' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "' and vIdNumeroSerieCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(1).Text & "' and vIdNumeroCorrelativoCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(2).Text & "' AND nIdNumeroItemDocumentacionOrdenTrabajo = '" & Item.ToString.Trim & "'"
            OrdenTrabajoNeg.OrdenTrabajoGetData(sentenciaSQL)

            Dim FuncionesNeg As New clsFuncionesNegocios
            FuncionesNeg.GuardarArchivoGeneral(fupSubirDocumentacionOrdenTrabajo.PostedFile, "Downloads\OrdenTrabajo", Trim(UCase(grdLista.SelectedRow.Cells(0).Text & grdLista.SelectedRow.Cells(1).Text & "C" & grdLista.SelectedRow.Cells(2).Text & "-" & Item.ToString)))

            divDetalleVerDocumentosOrdenTrabajo.Visible = False
            MyValidator.ErrorMessage = "Documentación registrada con éxito"
            BloquearMantenimiento(True, False, True, False)
            ValidationSummary6.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary6.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarSubirDocumentacionEquipo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelSubirDocumentacionOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.

            If grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count > 0 Then
                If grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex < grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count Then
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                        End If
                    Next
                End If
            End If

            If cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal.")
                Exit Sub
            Else
                AgregarCestaPersonal(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, False, Session("CestaSSPersonal"))
            End If
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
            grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
            MyValidator.ErrorMessage = "Personal agregado con éxito."
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMantenimientoOrdenTrabajo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPersonalAsignadoMantenimientoOrdenTrabajo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            Dim OrdenTrabajoNeg As New clsOrdenTrabajoNegocios
            Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                 Session("IdEmpresa"))

            Dim RrhhOrdTraBD = OrdenTrabajoNeg.OrdenTrabajoRecursos(OrdTra)

            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                    Dim existe As Boolean = RrhhOrdTraBD.Any(Function(p) p.RRHH_PERSONAL.cIdPersonal.Trim = (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) And p.bEstadoRegistroRecursosOrdenTrabajo = True)
                    If existe Then
                        'update estatus false
                        Dim query = "UPDATE LOGI_RECURSOSORDENTRABAJO SET bEstadoRegistroRecursosOrdenTrabajo = '" & False & "'
                                    WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
                                    vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
                                    vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
                                    cIdEmpresa = '" & Session("IdEmpresa") & "' And cIdPersonal = '" & (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) & "' "

                        Dim ok = OrdenTrabajoNeg.OrdenTrabajoQueryResponsable(query)
                    End If

                    QuitarCestaPersonal(i, Session("CestaSSPersonal"))
                    Exit For
                End If
            Next
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMantenimientoTarea_Load(sender As Object, e As EventArgs) Handles grdPersonalAsignadoMantenimientoTarea.Load

    End Sub

    Protected Sub btnCancelarSubirImagenActividad_Click(sender As Object, e As EventArgs) Handles btnCancelarSubirImagenActividad.Click

    End Sub

    Private Sub lnkbtnEliminarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnEliminarOrdenTrabajo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(0).Text) = True Then
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                lblDescripcionEquipoEliminarOrdenTrabajo.Text = grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text

                                pnlCabecera.Visible = True
                                pnlContenido.Visible = False
                                ValidationSummary13.ValidationGroup = "vgrpValidarEliminarOrdenTrabajo"

                                grdListaVerDocumentosOrdenTrabajo.DataSource = OrdenTrabajoNeg.OrdenTrabajoGetData("SELECT vTituloDocumentacionOrdenTrabajo AS Titulo, vDescripcionDocumentacionOrdenTrabajo AS Descripcion, CONVERT(VARCHAR(20),dFechaCarga,103)+' '+CONVERT(VARCHAR(20),dFechaCarga,108) AS Carga, vNombreArchivoDocumentacionOrdenTrabajo AS NombreArchivo, cIdTipoDocumentoCabeceraOrdenTrabajo AS Tipo, vIdNumeroSerieCabeceraOrdenTrabajo AS Serie, vIdNumeroCorrelativoCabeceraOrdenTrabajo AS Correlativo, nIdNumeroItemDocumentacionOrdenTrabajo AS Item " &
                                                                                             "FROM LOGI_DOCUMENTACIONORDENTRABAJO " &
                                                                                             "WHERE bEstadoRegistroDocumentacionOrdenTrabajo=1 and cIdTipoDocumentoCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "' and vIdNumeroSerieCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(1).Text & "' and vIdNumeroCorrelativoCabeceraOrdenTrabajo='" & grdLista.SelectedRow.Cells(2).Text & "' ")
                                grdListaVerDocumentosOrdenTrabajo.DataBind()
                                lnk_mostrarPanelEliminarOrdenTrabajo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione una orden de trabajo para elimnar.")
                    End If
                Else
                    Throw New Exception("Seleccione una orden de trabajo.")
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

    Private Sub btnSiEliminarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnSiEliminarOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            Dim query = "UPDATE LOGI_CABECERAORDENTRABAJO SET bEstadoRegistroCabeceraOrdenTrabajo = '" & False & "'
                        WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
                        vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
                        vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
                        cIdEmpresa = '" & Session("IdEmpresa") & "' "
            Dim ok = OrdenTrabajoNeg.OrdenTrabajoQueryResponsable(query)

            Me.grdLista.DataSource = fLlenarGrilla()
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False

        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnIniciarOrdenTrabajoTarea_Click(sender As Object, e As EventArgs) Handles lnkbtnIniciarOrdenTrabajoTarea.Click
        If hfdOperacion.Value = "R" Then
            Throw New Exception("Orden de Trabajo cerrada no se puede modificar.")
        End If
        'Función para validar si tiene permisos
        'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
        fLlenarGrillaPersonalAsignadoDetalle()
        grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.DataSource = fLlenarGrillaPersonalTareaDetalle()
        grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.DataBind()
        lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Function fLlenarGrillaPersonalAsignadoDetalle() 'As DataTable
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        Dim OrdTra = OrdenTrabajoNeg.OrdenTrabajoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                                                                 (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                                                                 Session("IdEmpresa"))

        Dim PersonalNeg As New clsOrdenTrabajoNegocios
        Dim result = PersonalNeg.OrdenTrabajoRecursos(OrdTra)

        VaciarCestaPersonal(Session("CestaOTPersonal"))

        'Primero buscamos si el usuario es el responsable
        If OrdTra.cIdUsuarioCreacionCabeceraOrdenTrabajo = Session("IdUsuario") Or Session("IdTipoUsuario") = "A" Then
            For Each item In result.Where(Function(x) x.bEstadoRegistroRecursosOrdenTrabajo = True)
                AgregarCestaPersonal(item.RRHH_PERSONAL.cIdPersonal.Trim, UCase(item.RRHH_PERSONAL.vNombreCompletoPersonal).Trim, item.bResponsableRecursosOrdenTrabajo, Session("CestaOTPersonal"))
            Next
        Else
            For Each item In result.Where(Function(x) x.bEstadoRegistroRecursosOrdenTrabajo = True)
                If item.RRHH_PERSONAL.vNumeroDocumentoPersonal.Trim = Trim(Session("vIdNroDocumentoIdentidadUsuario")) Then
                    AgregarCestaPersonal(item.RRHH_PERSONAL.cIdPersonal.Trim, UCase(item.RRHH_PERSONAL.vNombreCompletoPersonal).Trim, item.bResponsableRecursosOrdenTrabajo, Session("CestaOTPersonal"))
                End If
            Next
        End If

        grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.DataSource = Session("CestaOTPersonal")
        grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.DataBind()
    End Function

    Function fLlenarGrillaPersonalTareaDetalle() As DataTable
        Dim row = grdLista.Rows(grdLista.SelectedRow.RowIndex)
        Dim strNroOrdenTrabajo As String = ""
        strNroOrdenTrabajo = grdLista.SelectedRow.Cells(0).Text &
                             grdLista.SelectedRow.Cells(1).Text &
                             grdLista.SelectedRow.Cells(2).Text

        Dim PersonalTareaNeg As New clsOrdenTrabajoNegocios
        Dim dtPersonalTareaOTD = PersonalTareaNeg.OrdenTrabajoGetData("
               SELECT 
                    cIdTipoDocumentoCabeceraOrdenTrabajo,
                    vIdNumeroSerieCabeceraOrdenTrabajo,
                    vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                    cIdEquipoCabeceraOrdenTrabajo,
                    cIdEmpresa,
                    vIdArticuloSAPCabeceraOrdenTrabajo,
                    nIdNumeroItemDetalleTareaOrdenTrabajo,
                    vDescripcionTareaOrdenTrabajo,
                    cEstadoTareaOrdenTrabajo,
                    dFechaInicioTareaOrdenTrabajo,
                    dFechaFinalTareaOrdenTrabajo,
                    nTotalSegundosTrabajadosTareaOrdenTrabajo,
                    vComentarioTareaOrdenTrabajo,
                    STUFF((
                        SELECT '|' + LTRIM(RTRIM(cIdPersonal))
                        FROM LOGI_DETALLETAREAORDENTRABAJO AS T2
                        WHERE 
                            T2.cIdTipoDocumentoCabeceraOrdenTrabajo = T1.cIdTipoDocumentoCabeceraOrdenTrabajo
                            AND T2.vIdNumeroSerieCabeceraOrdenTrabajo = T1.vIdNumeroSerieCabeceraOrdenTrabajo
                            AND T2.vIdNumeroCorrelativoCabeceraOrdenTrabajo = T1.vIdNumeroCorrelativoCabeceraOrdenTrabajo
                            AND T2.cIdEquipoCabeceraOrdenTrabajo = T1.cIdEquipoCabeceraOrdenTrabajo
                            AND T2.cIdEmpresa = T1.cIdEmpresa
                            AND T2.vIdArticuloSAPCabeceraOrdenTrabajo = T1.vIdArticuloSAPCabeceraOrdenTrabajo
                            AND T2.nIdNumeroItemDetalleTareaOrdenTrabajo = T1.nIdNumeroItemDetalleTareaOrdenTrabajo
                        ORDER BY cIdPersonal
                        FOR XML PATH(''), TYPE
                    ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS cIdPersonalConcatenados
                FROM LOGI_DETALLETAREAORDENTRABAJO AS T1
                WHERE 
                    cIdTipoDocumentoCabeceraOrdenTrabajo + 
                    vIdNumeroSerieCabeceraOrdenTrabajo + 
                    vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & strNroOrdenTrabajo & "'
                    AND cIdEmpresa = '" & Session("IdEmpresa") & "'
                    AND cIdEquipoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim) & "'
                    AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim & "'
                GROUP BY 
                    cIdTipoDocumentoCabeceraOrdenTrabajo,
                    vIdNumeroSerieCabeceraOrdenTrabajo,
                    vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                    cIdEquipoCabeceraOrdenTrabajo,
                    cIdEmpresa,
                    vIdArticuloSAPCabeceraOrdenTrabajo,
                    nIdNumeroItemDetalleTareaOrdenTrabajo,
                    vDescripcionTareaOrdenTrabajo,
                    cEstadoTareaOrdenTrabajo,
                    dFechaInicioTareaOrdenTrabajo,
                    dFechaFinalTareaOrdenTrabajo,
                    nTotalSegundosTrabajadosTareaOrdenTrabajo,
                    vComentarioTareaOrdenTrabajo
                ORDER BY nIdNumeroItemDetalleTareaOrdenTrabajo DESC;")

        dtPersonalTareaOTD.Columns.Add(New DataColumn("vDescripcionPersonal", GetType(System.String)))

        For Each fila As DataRow In dtPersonalTareaOTD.Rows
            Dim idsConcatenados As String = fila("cIdPersonalConcatenados").ToString()
            If Not String.IsNullOrEmpty(idsConcatenados) Then
                Dim idsArray As String() = idsConcatenados.Split("|"c)
                For Each id As String In idsArray
                    ' Procesar cada id
                    Dim PersonalNeg As New clsPersonalNegocios
                    fila("vDescripcionPersonal") = fila("vDescripcionPersonal") + " | " + PersonalNeg.PersonalListarPorId(id, Session("IdEmpresa")).vNombreCompletoPersonal
                Next
                fila("vDescripcionPersonal") = Mid(fila("vDescripcionPersonal"), 4)
            End If
        Next

        Return dtPersonalTareaOTD
    End Function

    Private Sub btnIniciarMantenimientoTareaDetalleOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnIniciarMantenimientoTareaDetalleOrdenTrabajo.Click
        Try
            Dim PersonalTareaNeg As New clsOrdenTrabajoNegocios
            Dim x As Integer
            Dim strCodigoPersonalConcatenado, strPersonalConcatenado As String
            strCodigoPersonalConcatenado = "" : strPersonalConcatenado = ""

            Dim dFechaInicioTareaOrdenTrabajo As DateTime = Now
            For x = 0 To grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.Rows.Count - 1
                strCodigoPersonalConcatenado = strCodigoPersonalConcatenado + "|" + Server.HtmlDecode(grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.Rows(x).Cells(1).Text).Trim
                strPersonalConcatenado = strPersonalConcatenado + "|" + Server.HtmlDecode(grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.Rows(x).Cells(2).Text).Trim

                PersonalTareaNeg.OrdenTrabajoGetData("
                INSERT LOGI_DETALLETAREAORDENTRABAJO (cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo,
                       cIdEquipoCabeceraOrdenTrabajo, cIdEmpresa, vIdArticuloSAPCabeceraOrdenTrabajo, nIdNumeroItemDetalleTareaOrdenTrabajo,
                       vDescripcionTareaOrdenTrabajo, cEstadoTareaOrdenTrabajo, dFechaInicioTareaOrdenTrabajo, dFechaFinalTareaOrdenTrabajo,
                       nTotalSegundosTrabajadosTareaOrdenTrabajo, cIdPersonal, vComentarioTareaOrdenTrabajo)
                VALUES ('" & grdLista.SelectedRow.Cells(0).Text & "', '" &
                             grdLista.SelectedRow.Cells(1).Text & "', '" &
                             grdLista.SelectedRow.Cells(2).Text & "', '" &
                             Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim & "', '" &
                             Session("IdEmpresa") & "', '" &
                             Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim & "', '" &
                             "" & grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.Rows.Count + 1 & "', '" &
                             "', 'I', '" & dFechaInicioTareaOrdenTrabajo & "', '" & dFechaInicioTareaOrdenTrabajo & "', 0, '" & Server.HtmlDecode(grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.Rows(x).Cells(1).Text).Trim & "', '')")


            Next
            strCodigoPersonalConcatenado = Mid(strCodigoPersonalConcatenado, 2)
            strPersonalConcatenado = Mid(strPersonalConcatenado, 2)

            grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.DataSource = fLlenarGrillaPersonalTareaDetalle()
            grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.DataBind()

            fLlenarGrillaPersonalAsignadoDetalle()

            lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAceptarMantenimientoTareaDetalleOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoTareaDetalleOrdenTrabajo.Click
        Try
            Dim i As Integer
            For i = 0 To grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.Rows.Count - 1
                Dim row = grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.Rows(i)
                Dim lblItem As Label = TryCast(row.Cells(0).FindControl("lblItem"), Label)
                Dim txtFechaInicioTareaDetalleOrdenTrabajo As TextBox = TryCast(row.Cells(0).FindControl("txtFechaInicioTareaDetalleOrdenTrabajo"), TextBox)
                Dim txtHoraInicioTareaDetalleOrdenTrabajo As TextBox = TryCast(row.Cells(0).FindControl("txtHoraInicioTareaDetalleOrdenTrabajo"), TextBox)
                Dim txtFechaFinTareaDetalleOrdenTrabajo As TextBox = TryCast(row.Cells(0).FindControl("txtFechaFinTareaDetalleOrdenTrabajo"), TextBox)
                Dim txtHoraFinTareaDetalleOrdenTrabajo As TextBox = TryCast(row.Cells(0).FindControl("txtHoraFinTareaDetalleOrdenTrabajo"), TextBox)
                Dim lblIdResponsableTareaDetalleOrdenTrabajo As Label = TryCast(row.Cells(0).FindControl("lblIdResponsableTareaDetalleOrdenTrabajo"), Label)
                Dim txtDescripcionTareaDetalleOrdenTrabajo As TextBox = TryCast(row.Cells(0).FindControl("txtDescripcionTareaDetalleOrdenTrabajo"), TextBox)
                Dim txtComentarioTareaDetalleOrdenTrabajo As TextBox = TryCast(row.Cells(0).FindControl("txtComentarioTareaDetalleOrdenTrabajo"), TextBox)

                ' --- Combinar fecha y hora en DateTime ---
                Dim horaInicioTexto As String = txtHoraInicioTareaDetalleOrdenTrabajo.Text.Trim()
                Dim horaFinTexto As String = txtHoraFinTareaDetalleOrdenTrabajo.Text.Trim()

                Dim fechaInicio As DateTime = txtFechaInicioTareaDetalleOrdenTrabajo.Text.Trim()
                Dim fechaFin As DateTime = txtFechaFinTareaDetalleOrdenTrabajo.Text.Trim()

                Dim horaInicio As TimeSpan = TimeSpan.Parse(txtHoraInicioTareaDetalleOrdenTrabajo.Text)
                Dim horaFin As TimeSpan = TimeSpan.Parse(txtHoraFinTareaDetalleOrdenTrabajo.Text)

                ' Creamos DateTime completos
                Dim inicioCompleto As DateTime = fechaInicio.Add(horaInicio)
                Dim finCompleto As DateTime = fechaFin.Add(horaFin)

                ' --- Validaciones ---
                If inicioCompleto >= finCompleto Then
                    Throw New Exception("La fecha y hora de término deben ser mayores a la fecha y hora de inicio de la tarea.")
                End If

                Dim strSQL = "UPDATE LOGI_DETALLETAREAORDENTRABAJO
                              SET dFechaInicioTareaOrdenTrabajo = '" & txtFechaInicioTareaDetalleOrdenTrabajo.Text.Replace("-", "") & " " & txtHoraInicioTareaDetalleOrdenTrabajo.Text & "',
                                  dFechaFinalTareaOrdenTrabajo = " & IIf(txtFechaFinTareaDetalleOrdenTrabajo.Text = "", "NULL", "'" & txtFechaFinTareaDetalleOrdenTrabajo.Text.Replace("-", "") & " " & txtHoraFinTareaDetalleOrdenTrabajo.Text & "'") & ",
                                  vDescripcionTareaOrdenTrabajo = '" & txtDescripcionTareaDetalleOrdenTrabajo.Text & "',
                                  vComentarioTareaOrdenTrabajo = '" & txtComentarioTareaDetalleOrdenTrabajo.Text & "'
                              WHERE 
                                cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim & "'
                                AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "'
                                AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "'
                                AND cIdEmpresa = '" & Session("IdEmpresa") & "'
                                AND cIdEquipoCabeceraOrdenTrabajo ='" & Server.HtmlDecode(grdLista.SelectedRow.Cells(9).Text).Trim & "'
                                AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(13).Text).Trim & "'
								AND nIdNumeroItemDetalleTareaOrdenTrabajo = '" & lblItem.Text & "'"

                Dim PersonalTareaNeg As New clsOrdenTrabajoNegocios
                PersonalTareaNeg.OrdenTrabajoGetData(strSQL)
            Next
        Catch ex As Exception
            ValidationSummary14.ValidationGroup = "vgrpValidarMantenimientoTareaDetalleOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTareaDetalleOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Protected Sub grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo_RowCommand_Botones(sender As Object, e As GridViewCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()

            If e.CommandName = "Eliminar" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")

                Dim PersonalTareaNeg As New clsOrdenTrabajoNegocios
                PersonalTareaNeg.OrdenTrabajoGetData("
                DELETE LOGI_DETALLETAREAORDENTRABAJO
                WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Valores(0) & "' AND
                      vIdNumeroSerieCabeceraOrdenTrabajo = '" & Valores(1) & "' AND
                      vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Valores(2) & "' AND
                      cIdEmpresa = '" & Valores(3) & "' AND
                      cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4) & "' AND
                      vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(5) & "' AND
                      nIdNumeroItemDetalleTareaOrdenTrabajo = '" & Valores(6) & "'")

                PersonalTareaNeg.OrdenTrabajoGetData("
                ;WITH OrdenTrabajoCTE AS (
                            SELECT 
                                cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, cIdEquipoCabeceraOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo, nIdNumeroItemDetalleTareaOrdenTrabajo,
                                ROW_NUMBER() OVER (ORDER BY MIN(nIdNumeroItemDetalleTareaOrdenTrabajo)) AS NuevoNumeroItem
                            FROM LOGI_DETALLETAREAORDENTRABAJO
                            WHERE 
                                cIdTipoDocumentoCabeceraOrdenTrabajo = '" & Valores(0) & "'
                                AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & Valores(1) & "'
                                AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Valores(2) & "'
                                AND cIdEmpresa = '" & Valores(3) & "'
                                AND cIdEquipoCabeceraOrdenTrabajo ='" & Valores(4) & "'
                                AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(5) & "'
                            GROUP BY cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, 
                                     vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, 
                                     cIdEquipoCabeceraOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo, 
                                     nIdNumeroItemDetalleTareaOrdenTrabajo
                        )
                        UPDATE d SET
                            d.nIdNumeroItemDetalleTareaOrdenTrabajo = cte.NuevoNumeroItem
                        FROM LOGI_DETALLETAREAORDENTRABAJO d
                        INNER JOIN OrdenTrabajoCTE cte ON d.cIdTipoDocumentoCabeceraOrdenTrabajo = cte.cIdTipoDocumentoCabeceraOrdenTrabajo
                             AND d.vIdNumeroSerieCabeceraOrdenTrabajo = cte.vIdNumeroSerieCabeceraOrdenTrabajo 
						     AND d.vIdNumeroCorrelativoCabeceraOrdenTrabajo = cte.vIdNumeroCorrelativoCabeceraOrdenTrabajo 
						     AND d.cIdEmpresa = cte.cIdEmpresa
						     AND d.cIdEquipoCabeceraOrdenTrabajo = cte.cIdEquipoCabeceraOrdenTrabajo
						     AND d.vIdArticuloSAPCabeceraOrdenTrabajo = cte.vIdArticuloSAPCabeceraOrdenTrabajo
						     AND d.nIdNumeroItemDetalleTareaOrdenTrabajo = cte.nIdNumeroItemDetalleTareaOrdenTrabajo
                ")
                grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.DataSource = fLlenarGrillaPersonalTareaDetalle()
                grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.DataBind()
            ElseIf e.CommandName = "AdicionarTarea" Then
                Dim Valores() As String = e.CommandArgument.ToString.Split("*")
                hfdItemSeleccionadoGridPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.Value = Valores(6)
                Dim AsignarTareaNeg As New clsOrdenTrabajoNegocios
                grdListadoTareasPredefinidas.DataSource = AsignarTareaNeg.OrdenTrabajoGetData(
                    "SELECT cIdTareaPredefinida, vDescripcionTareaPredefinida FROM LOGI_TAREAPREDEFINIDA"
                    )
                grdListadoTareasPredefinidas.DataBind()

                lnk_mostrarPanelListadoTareasPredefinidas_ModalPopupExtender.Show()
            End If
        Catch ex As Exception
            ValidationSummary14.ValidationGroup = "vgrpValidarMantenimientoTareaDetalleOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTareaDetalleOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.RowDeleting
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            For i = 0 To Session("CestaOTPersonal").Rows.Count - 1
                If (Session("CestaOTPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                    QuitarCestaPersonal(i, Session("CestaOTPersonal"))
                    Exit For
                End If
            Next
            Me.grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.DataSource = Session("CestaOTPersonal")
            Me.grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary14.ValidationGroup = "vgrpValidarMantenimientoTareaDetalleOrdenTrabajo"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoTareaDetalleOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub grdListadoTareasPredefinidas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdListadoTareasPredefinidas.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            If grdListadoTareasPredefinidas IsNot Nothing Then
                If grdListadoTareasPredefinidas.Rows.Count > 0 Then
                    If IsNothing(grdListadoTareasPredefinidas.SelectedRow) = False Then
                        If IsReference(grdListadoTareasPredefinidas.SelectedRow.Cells(1).Text) = True Then

                            lnk_mostrarPanelListadoTareasPredefinidas_ModalPopupExtender.Show()

                        End If
                    End If
                Else
                    'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()
                End If
            End If
            ValidationSummary15.ValidationGroup = "vgrpValidarListadoTareasPredefinidas"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarListadoTareasPredefinidas"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary15.ValidationGroup = "vgrpValidarListadoTareasPredefinidas"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarListadoTareasPredefinidas"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub btnCancelarListadoTareasPredefinidas_Click(sender As Object, e As EventArgs) Handles btnCancelarListadoTareasPredefinidas.Click
        lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAceptarListadoTareasPredefinidas_Click(sender As Object, e As EventArgs) Handles btnAceptarListadoTareasPredefinidas.Click
        For i = 0 To grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.Rows.Count - 1
            Dim row = grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.Rows(i)
            Dim lblItem As Label = TryCast(row.Cells(0).FindControl("lblItem"), Label)
            If lblItem.Text = hfdItemSeleccionadoGridPersonalTareoMantenimientoTareaDetalleOrdenTrabajo.Value Then
                Dim txtDescripcionTareaDetalleOrdenTrabajo As TextBox = TryCast(row.Cells(0).FindControl("txtDescripcionTareaDetalleOrdenTrabajo"), TextBox)
                txtDescripcionTareaDetalleOrdenTrabajo.Text = grdListadoTareasPredefinidas.SelectedRow.Cells(2).Text
            End If
        Next

        lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAgregarGaleriaImagenActividad_Click(sender As Object, e As EventArgs) Handles btnAgregarGaleriaImagenActividad.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0649", strOpcionModulo, "CMMS")

            LimpiarObjetosImagen()
            Dim Valores() As String = hfdValoresCheckList.Value.ToString.Split("*")
            Dim ActNeg As New clsActividadCheckListNegocios
            Dim Actividad As LOGI_ACTIVIDADCHECKLIST = ActNeg.ActividadCheckListListarPorId(Valores(3).ToString.Trim)
            Dim EquNeg As New clsEquipoNegocios
            Dim Equipo As LOGI_EQUIPO = EquNeg.EquipoListarPorId(Valores(4).ToString.Trim)
            txtTituloSubirImagenActividad.Text = Equipo.vDescripcionEquipo '& " - " & Actividad.vDescripcionActividadCheckList
            txtDescripcionSubirImagenActividad.Text = Actividad.vDescripcionActividadCheckList
            lnk_mostrarPanelSubirImagenActividad_ModalPopupExtender.Show()

            MyValidator.ErrorMessage = "Actividad agregada con éxito."
            ValidationSummary4.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            ValidationSummary4.ValidationGroup = "vgrpValidarCatalogoComponente"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarCatalogoComponente"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub lnkbtnFirmarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnFirmarOrdenTrabajo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(0).Text) = True Then
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then
                                lblDescripcionEquipoFirmarOrdenTrabajo.Text = grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text

                                pnlCabecera.Visible = True
                                pnlContenido.Visible = False
                                ValidationSummary13.ValidationGroup = "vgrpValidarFirmarOrdenTrabajo"

                                Dim dsFirma = OrdenTrabajoNeg.OrdenTrabajoGetData("
                                                SELECT vURLAsignarFirma FROM RRHH_ASIGNARFIRMA WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdUsuario = '" & Session("IdUsuario") & "' AND bEstadoRegistroAsignarFirma = 1 
                                                ")

                                If dsFirma.Rows.Count > 0 Then 'Existe la Firma
                                    lblEstadoFirmaOrdenTrabajo.Text = "Firma encontrada de " & Session("IdLogin")
                                    btnSiFirmarOrdenTrabajo.Visible = True
                                Else
                                    lblEstadoFirmaOrdenTrabajo.Text = "Firma no encontrada de " & Session("IdLogin")
                                    btnSiFirmarOrdenTrabajo.Visible = False
                                End If

                                lnk_mostrarPanelFirmarOrdenTrabajo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione una orden de trabajo para firmar.")
                    End If
                Else
                    Throw New Exception("Seleccione una orden de trabajo.")
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

    Private Sub btnSiFirmarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnSiFirmarOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            Dim query As String = ""
            'Dim dsFirmaDigital = OrdenTrabajoNeg.OrdenTrabajoGetData("
            'SELECT COUNT(*) FROM LOGI_CABECERAORDENTRABAJO 
            'WHERE cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdEmpresa") & "' AND 
            '      cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
            '      vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
            '      vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
            '      cIdEmpresa = '" & Session("IdEmpresa") & "'")

            'If dsFirmaDigital.Rows(0).Item(0) > 0 Or Session("IdTipoUsuario") = "A" Then 'Validamos si el usuario es el creador - planer
            If Session("IdTipoUsuario") = "A" Or Session("IdPerfil") = "00012" Then 'Validamos si el usuario es el creador - planer
                query = "UPDATE LOGI_CABECERAORDENTRABAJO 
                        SET vNombreFirma1CabeceraOrdenTrabajo = (SELECT vURLAsignarFirma FROM RRHH_ASIGNARFIRMA WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdUsuario = '" & Session("IdUsuario") & "' AND bEstadoRegistroAsignarFirma = 1 )
                        WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
                        vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
                        vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
                        cIdEmpresa = '" & Session("IdEmpresa") & "' "
                OrdenTrabajoNeg.OrdenTrabajoGetData(query)
            ElseIf Session("IdPerfil") = "00014" Or Session("IdPerfil") = "00015" Then
                'Si no es el creador - planer, entonces se guarda la firma en el campo 2
                query = "UPDATE LOGI_CABECERAORDENTRABAJO 
                        SET vNombreFirma2CabeceraOrdenTrabajo = (SELECT vURLAsignarFirma FROM RRHH_ASIGNARFIRMA WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdUsuario = '" & Session("IdUsuario") & "' AND bEstadoRegistroAsignarFirma = 1 )
                        WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
                        vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
                        vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
                        cIdEmpresa = '" & Session("IdEmpresa") & "' "
                OrdenTrabajoNeg.OrdenTrabajoGetData(query)
            End If

            Me.grdLista.DataSource = fLlenarGrilla()
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False

            MyValidator.ErrorMessage = "Firma registrada con éxito."
            ValidationSummary1.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
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

    Private Sub lnkbtnConclusionesRecomendacionesOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnConclusionesRecomendacionesOrdenTrabajo.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(0).Text) = True Then
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(17).Text) = "True", "1", "0")
                            If MyValidator.ErrorMessage = "" Then

                                pnlCabecera.Visible = True
                                pnlContenido.Visible = False
                                ValidationSummary17.ValidationGroup = "vgrpValidarFirmarOrdenTrabajo"

                                Dim dsConclusionRecomendacion = OrdenTrabajoNeg.OrdenTrabajoGetData("
                                                SELECT vConclusionRecomendacionCabeceraOrdenTrabajo FROM LOGI_CABECERAORDENTRABAJO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND
                                                       cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
                                                       vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
                                                       vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "'")

                                If dsConclusionRecomendacion.Rows.Count > 0 Then 'Existe la Firma
                                    txtConclusionesConclusionRecomendacionOrdenTrabajo.Text = dsConclusionRecomendacion.Rows(0).Item(0).ToString.Trim
                                Else
                                    txtConclusionesConclusionRecomendacionOrdenTrabajo.Text = ""
                                End If

                                lnk_mostrarPanelConclusionRecomendacionOrdenTrabajo_ModalPopupExtender.Show()
                            End If
                        End If
                    Else
                        Throw New Exception("Seleccione una orden de trabajo para firmar.")
                    End If
                Else
                    Throw New Exception("Seleccione una orden de trabajo.")
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

    Private Sub btnGuardarConclusionRecomendacionOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnGuardarConclusionRecomendacionOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""

            Dim query As String = ""
            'Dim dsFirmaDigital = OrdenTrabajoNeg.OrdenTrabajoGetData("
            'SELECT COUNT(*) FROM LOGI_CABECERAORDENTRABAJO 
            'WHERE cIdUsuarioCreacionCabeceraOrdenTrabajo = '" & Session("IdEmpresa") & "' AND 
            '      cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND
            '      vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND
            '      vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND
            '      cIdEmpresa = '" & Session("IdEmpresa") & "'")

            'If dsFirmaDigital.Rows(0).Item(0) > 0 Or Session("IdTipoUsuario") = "A" Then 'Validamos si el usuario es el creador - planer
            query = "UPDATE LOGI_CABECERAORDENTRABAJO 
                       SET vConclusionRecomendacionCabeceraOrdenTrabajo = @conclusion 
                       WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = @tipoDoc
                         AND vIdNumeroSerieCabeceraOrdenTrabajo = @serie
                         AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = @correlativo
                         AND cIdEmpresa = @empresa"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@conclusion", txtConclusionesConclusionRecomendacionOrdenTrabajo.Text),
                New SqlParameter("@tipoDoc", Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
                New SqlParameter("@serie", Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
                New SqlParameter("@correlativo", Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
                New SqlParameter("@empresa", Session("IdEmpresa"))
            }
            OrdenTrabajoNeg.EjecutarComando(query, parametros)
            'End If

            Me.grdLista.DataSource = fLlenarGrilla()
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False

            MyValidator.ErrorMessage = "Conclusiones y recomendaciones registradas con éxito."
            ValidationSummary1.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
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

    Private Sub cboFiltroStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFiltroStatus.SelectedIndexChanged
        Me.grdLista.DataSource = fLlenarGrilla()
        Me.grdLista.DataBind()
    End Sub
End Class