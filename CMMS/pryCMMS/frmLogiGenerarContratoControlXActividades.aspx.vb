Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmLogiGenerarContratoControlXActividades
    Inherits System.Web.UI.Page
    Dim ContratoNeg As New clsContratoNegocios
    'Dim PersonalNeg As New clsPersonalNegocios
    Dim ClienteNeg As New clsClienteNegocios
    Dim EquipoNeg As New clsEquipoNegocios
    Dim DetalleContratoNeg As New clsDetalleContratoNegocios
    'Dim CatalogoNeg As New clsCatalogoNegocios
    'Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String
    'Shared strTabContenedorActivo As String
    Dim MyValidator As New CustomValidator
    Shared intIndexContratoEquipo As Integer
    'Shared rowIndexDetalle As Int64

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

    Shared Function CrearCestaEquipo(ByVal NroDiasMes As Integer) As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Seleccionar", GetType(System.Boolean))) '1
        dt.Columns.Add(New DataColumn("Item", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("DescripcionEquipo", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '4
        For i = 1 To NroDiasMes
            dt.Columns.Add(New DataColumn("D" & i, GetType(System.String))) '12
        Next
        Return dt
    End Function

    Shared Sub EditarCestaEquipo(ByVal Seleccionar As Boolean, ByVal CodigoEquipo As String, ByVal DescripcionEquipo As String,
                                 ByVal Estado As Boolean, 'ByVal FechaActividad As DateTime,
                                 ByVal NombreColumna As String, ByVal IdTipoMantenimiento As String, 'ByVal NroDiasMes As Integer,
                                 ByVal Tabla As DataTable, ByVal Fila As Integer)
        'ByVal FecVen As System.Nullable(Of DateTime), ByVal CentroCosto As String, _
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                'Tabla.Rows(Fila)(0) = xxx 'Numero de Linea
                Tabla.Rows(Fila)("Seleccionar") = Seleccionar
                Tabla.Rows(Fila)("IdEquipo") = CodigoEquipo
                Tabla.Rows(Fila)("DescripcionEquipo") = DescripcionEquipo
                Tabla.Rows(Fila)("Estado") = Estado
                If NombreColumna <> "" Then
                    Tabla.Rows(Fila)(NombreColumna) = IdTipoMantenimiento
                End If
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaEquipo(ByVal Seleccionar As Boolean, ByVal Item As String, ByVal CodigoEquipo As String, ByVal DescripcionEquipo As String,
                                   ByVal Estado As Boolean, ByVal NombreColumna As String, ByVal IdTipoMantenimiento As String,
                                   ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        'Fila("Item") = Tabla.Rows.Count + 1
        Fila("Seleccionar") = Seleccionar
        Fila("Item") = Item
        Fila("IdEquipo") = CodigoEquipo
        Fila("DescripcionEquipo") = DescripcionEquipo
        Fila("Estado") = Estado
        Dim Columnas As String() = NombreColumna.Split("|")
        Dim DatosColumnas As String() = IdTipoMantenimiento.Split("*")
        For i = 0 To Columnas.Length - 1
            If DatosColumnas(i) <> "" Then
                Fila(Columnas(i)) = DatosColumnas(i)
            End If
        Next
        'Fila(NombreColumna) = IdTipoMantenimiento
        'For i = 1 To 31
        '    Fila("D" & i) = "||||"
        'Next
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaEquipo(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaEquipo(ByVal Tabla As DataTable)
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
        'ByVal FecVen As System.Nullable(Of DateTime), ByVal CentroCosto As String, _
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                'Tabla.Rows(Fila)(0) = xxx 'Numero de Linea
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

    ''JMUG: INICIAL: 27/07/2023
    'Shared Function CrearCestaOrdenTrabajo() As DataTable
    '    'Código que se ejecuta cuando se inicia una nueva sesión
    '    'Aquí se crea una tabla temporal para guardar los
    '    'Datos del temporal
    '    Dim dt As New DataTable
    '    dt.Columns.Add(New DataColumn("FechaPlanificacion", GetType(System.DateTime))) '1
    '    dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '2
    '    dt.Columns.Add(New DataColumn("IdTipoMantenimiento", GetType(System.String))) '3
    '    dt.Columns.Add(New DataColumn("IdContrato", GetType(System.String))) '4
    '    dt.Columns.Add(New DataColumn("IdOrdenTrabajo", GetType(System.String))) '5
    '    Return dt
    'End Function

    'Shared Sub EditarCestaOrdenTrabajo(ByVal FechaPlanificacion As DateTime, ByVal IdEquipo As String, ByVal IdTipoMantenimiento As String,
    '                                   ByVal IdContrato As String, ByVal IdOrdenTrabajo As String,
    '                                   ByVal Tabla As DataTable, ByVal Fila As Integer)
    '    Try
    '        If Fila > -1 And Tabla.Rows.Count > 0 Then
    '            '                Tabla.Rows.RemoveAt(Fila)
    '            Tabla.Rows(Fila).BeginEdit()
    '            Tabla.Rows(Fila)("FechaPlanificacion") = FechaPlanificacion
    '            Tabla.Rows(Fila)("IdEquipo") = IdEquipo
    '            Tabla.Rows(Fila)("IdTipoMantenimiento") = IdTipoMantenimiento
    '            Tabla.Rows(Fila)("IdContrato") = IdContrato
    '            Tabla.Rows(Fila)("IdOrdenTrabajo") = IdOrdenTrabajo
    '            Tabla.Rows(Fila).EndEdit()
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    Finally
    '        'cn.Close()
    '    End Try
    'End Sub

    'Shared Sub AgregarCestaOrdenTrabajo(ByVal FechaPlanificacion As DateTime, ByVal IdEquipo As String, ByVal IdTipoMantenimiento As String,
    '                                    ByVal IdContrato As String, ByVal IdOrdenTrabajo As String,
    '                                    ByVal Tabla As DataTable)
    '    Dim Fila As DataRow = Tabla.NewRow
    '    Fila("FechaPlanificacion") = FechaPlanificacion
    '    Fila("IdEquipo") = IdEquipo
    '    Fila("IdTipoMantenimiento") = IdTipoMantenimiento
    '    Fila("IdContrato") = IdContrato
    '    Fila("IdOrdenTrabajo") = IdOrdenTrabajo
    '    Tabla.Rows.Add(Fila)
    'End Sub

    'Shared Sub QuitarCestaOrdenTrabajo(ByVal Fila As Integer, ByVal Tabla As DataTable)
    '    Try
    '        If Fila > -1 And Tabla.Rows.Count > 0 Then
    '            Tabla.Rows.RemoveAt(Fila)
    '            Dim i As Integer
    '            For i = 0 To Tabla.Rows.Count - 1
    '                Tabla.Rows(i).BeginEdit()
    '                Tabla.Rows(i).EndEdit()
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    Finally
    '        'cn.Close()
    '    End Try
    'End Sub

    'Shared Sub VaciarCestaOrdenTrabajo(ByVal Tabla As DataTable)
    '    Try
    '        Tabla.Rows.Clear()
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    Finally
    '        'cn.Close()
    '    End Try
    'End Sub

    Shared Function CrearCestaPersonalOrdenTrabajo() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("FechaPlanificacion", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdContrato", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("Responsable", GetType(System.Boolean))) '4
        dt.Columns.Add(New DataColumn("IdPersonal", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("Personal", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("IdOrdenTrabajo", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("IdOrdenTrabajoCliente", GetType(System.String))) '8
        Return dt
    End Function

    Shared Sub EditarCestaPersonalOrdenTrabajo(ByVal FechaPlanificacion As String, ByVal IdEquipo As String, ByVal IdContrato As String,
                                               ByVal Responsable As Boolean, ByVal IdPersonal As String, ByVal Personal As String,
                                               ByVal IdOrdenTrabajo As String, ByVal IdOrdenTrabajoCliente As String,
                                               ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)("FechaPlanificacion") = FechaPlanificacion
                Tabla.Rows(Fila)("IdEquipo") = IdEquipo
                Tabla.Rows(Fila)("IdContrato") = IdContrato
                Tabla.Rows(Fila)("Responsable") = Responsable
                Tabla.Rows(Fila)("IdPersonal") = IdPersonal
                Tabla.Rows(Fila)("Personal") = Personal
                Tabla.Rows(Fila)("IdOrdenTrabajo") = IdOrdenTrabajo
                Tabla.Rows(Fila)("IdOrdenTrabajoCliente") = IdOrdenTrabajoCliente
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Shared Sub AgregarCestaPersonalOrdenTrabajo(ByVal FechaPlanificacion As String, ByVal IdEquipo As String, ByVal IdContrato As String,
                                                ByVal Responsable As Boolean, ByVal IdPersonal As String, ByVal Personal As String,
                                                ByVal IdOrdenTrabajo As String, ByVal IdOrdenTrabajoCliente As String,
                                                ByVal Tabla As DataTable)
        Dim Fila As DataRow = Tabla.NewRow
        Fila("FechaPlanificacion") = FechaPlanificacion
        Fila("IdEquipo") = IdEquipo
        Fila("IdContrato") = IdContrato
        Fila("Responsable") = Responsable
        Fila("IdPersonal") = IdPersonal
        Fila("Personal") = Personal
        Fila("IdOrdenTrabajo") = IdOrdenTrabajo
        Fila("IdOrdenTrabajoCliente") = IdOrdenTrabajoCliente
        Tabla.Rows.Add(Fila)
    End Sub

    Shared Sub QuitarCestaPersonalOrdenTrabajo(ByVal Fila As Integer, ByVal Tabla As DataTable)
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

    Shared Sub VaciarCestaPersonalOrdenTrabajo(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub
    'JMUG: FINAL: 27/07/2023

    Sub CargarCestaEquiposImprimir()
        'Carga las opciones en la Grilla de todos los Equipos asignados a este contrato.
        Try
            VaciarCestaEquipo(Session("CestaEquipoContratoImprimir"))
            Dim dsEquipos = ContratoNeg.ContratoGetData("SELECT CON.cIdTipoDocumentoCabeceraContrato, CON.vIdNumeroSerieCabeceraContrato, CON.vIdNumeroCorrelativoCabeceraContrato, CON.cIdEmpresa, " &
                                                        "       CON.dFechaEmisionCabeceraContrato, CON.cIdCliente, CON.cEstadoCabeceraContrato, CON.vDescripcionCabeceraContrato, " &
                                                        "       CON.vNroLicitacionCabeceraContrato, DETCON.cIdEquipoDetalleContrato, DETCON.vDescripcionDetalleContrato, " &
                                                        "       DETCON.bEstadoRegistroDetalleContrato, DETCON.nIdNumeroItemDetalleContrato " &
                                                        "FROM LOGI_CABECERACONTRATO AS CON " &
                                                        "     INNER JOIN LOGI_DETALLECONTRATO AS DETCON ON " &
                                                        "     CON.cIdEmpresa = DETCON.cIdEmpresa AND " &
                                                        "     CON.cIdTipoDocumentoCabeceraContrato = DETCON.cIdTipoDocumentoCabeceraContrato AND " &
                                                        "     CON.vIdNumeroSerieCabeceraContrato = DETCON.vIdNumeroSerieCabeceraContrato AND " &
                                                        "     CON.vIdNumeroCorrelativoCabeceraContrato = DETCON.vIdNumeroCorrelativoCabeceraContrato " &
                                                        "WHERE CON.cIdEmpresa = '" & Session("IdEmpresa") & "' AND " &
                                                        "      CON.cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND " &
                                                        "      CON.vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND " &
                                                        "      CON.vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "'")

            For Each ListaEquipos In dsEquipos.Rows
                AgregarCestaEquipo(False, ListaEquipos("nIdNumeroItemDetalleContrato"), ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), "", "", Session("CestaEquipoContratoImprimir"))
            Next

            Me.grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            Me.grdDetalleEquipoImprimirProgramacion.DataBind()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaEquipos()
        'Carga las opciones en la Grilla de todos los Equipos asignados a este contrato.
        Try
            VaciarCestaEquipo(Session("CestaEquipoContrato"))
            VaciarCestaPersonalOrdenTrabajo(Session("CestaPersonalOrdenTrabajo"))
            'Dim dsEquipos = ContratoNeg.ContratoGetData("SELECT EQU.cIdEquipo, EQU.vDescripcionEquipo, EQU.bEstadoRegistroEquipo " &
            '                                            "FROM LOGI_EQUIPO AS EQU " &
            '                                            "WHERE EQU.bEstadoRegistroEquipo = 1 AND bTieneContrato = '1'")
            Dim dsEquipos = ContratoNeg.ContratoGetData("SELECT CON.cIdTipoDocumentoCabeceraContrato, CON.vIdNumeroSerieCabeceraContrato, CON.vIdNumeroCorrelativoCabeceraContrato, CON.cIdEmpresa, " &
                                                        "       CON.dFechaEmisionCabeceraContrato, CON.cIdCliente, CON.cEstadoCabeceraContrato, CON.vDescripcionCabeceraContrato, " &
                                                        "       CON.vNroLicitacionCabeceraContrato, DETCON.cIdEquipoDetalleContrato, DETCON.vDescripcionDetalleContrato, " &
                                                        "       DETCON.bEstadoRegistroDetalleContrato, DETCON.nIdNumeroItemDetalleContrato " &
                                                        "FROM LOGI_CABECERACONTRATO AS CON " &
                                                        "     INNER JOIN LOGI_DETALLECONTRATO AS DETCON ON " &
                                                        "     CON.cIdEmpresa = DETCON.cIdEmpresa AND " &
                                                        "     CON.cIdTipoDocumentoCabeceraContrato = DETCON.cIdTipoDocumentoCabeceraContrato AND " &
                                                        "     CON.vIdNumeroSerieCabeceraContrato = DETCON.vIdNumeroSerieCabeceraContrato AND " &
                                                        "     CON.vIdNumeroCorrelativoCabeceraContrato = DETCON.vIdNumeroCorrelativoCabeceraContrato " &
                                                        "WHERE CON.cIdEmpresa = '" & Session("IdEmpresa") & "' AND " &
                                                        "      CON.cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND " &
                                                        "      CON.vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND " &
                                                        "      CON.vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "'")

            'Private Sub grdDetalleEquipos_Load(sender As Object, e As EventArgs) Handles grdDetalleEquipos.Load
            'cboTipoDocumento_SelectedIndexChanged(cboTipoDocumento, New System.EventArgs())
            grdDetalleEquipos_Load(grdDetalleEquipos, New System.EventArgs)
            'Dim strNombreColumna As String = ""
            'Dim strFechaColumna As String = "|||"
            'For i = 1 To 31
            '    If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
            '        'If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
            '        strNombreColumna = strNombreColumna & ("D" & i) & "|"
            '        strFechaColumna = strFechaColumna + grdDetalleEquipos.Columns(2 + i).HeaderText + "||*|||"
            '        'End If
            '    End If
            'Next


            For Each ListaEquipos In dsEquipos.Rows
                Dim dsPlanificacion = ContratoNeg.ContratoGetData("SELECT nIdNumeroItemDetalleContrato, cIdEquipoDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato, cIdTipoMantenimientoPlanificacionEquipoContrato, " &
                                                                  "       cIdPeriodoMesPlanificacionEquipoContrato, vOrdenTrabajoReferenciaPlanificacionEquipoContrato, vOrdenTrabajoClientePlanificacionEquipoContrato,  " &
                                                                  "       cIdNumeroPlantillaCheckListPlanificacionEquipoContrato " &
                                                                  "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                                                  "WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "' " &
                                                                  "      AND nIdNumeroItemDetalleContrato = '" & ListaEquipos("nIdNumeroItemDetalleContrato") & "' AND cIdEquipoDetalleContrato = '" & ListaEquipos("cIdEquipoDetalleContrato") & "'  " &
                                                                  "      AND YEAR(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboPeriodo.SelectedValue & " AND MONTH(dFechaHoraProgramacionPlanificacionEquipoContrato) = " & cboMes.SelectedValue & " " &
                                                                  "ORDER BY cIdPeriodoMesPlanificacionEquipoContrato, nIdNumeroItemDetalleContrato, dFechaHoraProgramacionPlanificacionEquipoContrato")
                'AgregarCestaEquipo(ListaEquipos("IdEquipo"), ListaEquipos("vDescripcionEquipo"), ListaEquipos("bEstadoRegistroEquipo"), Session("CestaEquipoContrato"))
                Dim strNombreColumna As String = ""
                Dim strDatosColumna As String = ""
                'AgregarCestaEquipo(ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), strNombreColumna, strFechaColumna, Session("CestaEquipoContrato"))
                'Dim strNombreColumnaModificar As String = ""
                'Obtener Nro.Columna
                'For x = 1 To 31
                Dim bValidaIngreso As Boolean = False
                For i = 1 To 31
                    If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                        For Each ListaPlanificacion In dsPlanificacion.Rows

                            If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato")) Then
                                strNombreColumna = strNombreColumna + ("D" & i) & "|"
                                strDatosColumna = strDatosColumna + ListaPlanificacion("cIdTipoMantenimientoPlanificacionEquipoContrato") & "|" & txtIdContrato.Text & "|" & ListaPlanificacion("cIdNumeroPlantillaCheckListPlanificacionEquipoContrato") & "|" & ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoClientePlanificacionEquipoContrato") & "*"
                                bValidaIngreso = True
                                'IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText)
                                'Exit For
                            End If
                            'If IsDate(hfdFechaPlanificacion.Value) AndAlso CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(hfdFechaPlanificacion.Value) Then
                            '    strNombreColumnaModificar = ("D" & i)
                            '    'IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText)
                            '    'Exit For
                            'End If
                            'Else
                            '    If strNombreColumna = "" Then
                            '        Throw New Exception("Esta fecha no esta asignada en la programación.")
                            '    End If
                        Next
                        If bValidaIngreso = False Then
                            strNombreColumna = strNombreColumna + ("D" & i) & "|"
                            'strDatosColumna = ListaPlanificacion("cIdTipoMantenimientoPlanificacionEquipoContrato") & "|" & hfdIdContrato.Value & "|" & ListaPlanificacion("cIdNumeroPlantillaCheckListPlanificacionEquipoContrato") & "|" & ListaPlanificacion("dFechaHoraProgramacionPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "|" & ListaPlanificacion("vOrdenTrabajoClientePlanificacionEquipoContrato") & "*"
                            strDatosColumna = strDatosColumna + "" & "|" & txtIdContrato.Text & "|" & "" & "|" & CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) & "|" & "" & "|" & "" & "*"
                        End If
                    End If
                Next                    'IIf(lnkEsComponenteOn.Visible = True, "1", "0"), CaracteristicaEquipo("cIdCaracteristica"), CaracteristicaEquipo("vDescripcionCaracteristica"), CaracteristicaEquipo("cIdReferenciaSAPEquipoCaracteristica"), CaracteristicaEquipo("vDescripcionCampoSAPEquipoCaracteristica"), CaracteristicaEquipo("vValorReferencialEquipoCaracteristica"), Session("CestaCaracteristicaEquipoPrincipal"))
                strNombreColumna = Mid(strNombreColumna, 1, strNombreColumna.Length - 1)
                AgregarCestaEquipo(False, ListaEquipos("nIdNumeroItemDetalleContrato"), ListaEquipos("cIdEquipoDetalleContrato"), ListaEquipos("vDescripcionDetalleContrato"), ListaEquipos("bEstadoRegistroDetalleContrato"), strNombreColumna, strDatosColumna, Session("CestaEquipoContrato"))
            Next
            Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
            Me.grdDetalleEquipos.DataBind()

            Dim dsPersonal = ContratoNeg.ContratoGetData("SELECT PLAEQUCON.vOrdenTrabajoClientePlanificacionEquipoContrato, ORDTRA.vContratoReferenciaCabeceraOrdenTrabajo, ORDTRA.dFechaInicioPlanificadaCabeceraOrdenTrabajo, RRHH.cIdTipoDocumentoCabeceraOrdenTrabajo, RRHH.vIdNumeroSerieCabeceraOrdenTrabajo, " &
                                                         "       RRHH.vIdNumeroCorrelativoCabeceraOrdenTrabajo, RRHH.cIdEmpresa, RRHH.cIdPersonal, RRHH.vObservacionRecursosOrdenTrabajo, RRHH.bEstadoRegistroRecursosOrdenTrabajo, RRHH.cIdEquipoCabeceraOrdenTrabajo, " &
                                                         "       RRHH.nTotalMinutosTrabajadosRecursosOrdenTrabajo, RRHH.vIdArticuloSAPCabeceraOrdenTrabajo, RRHH.bResponsableRecursosOrdenTrabajo, " &
                                                         "       PER.vApellidoPaternoPersonal + ' ' + PER.vApellidoPaternoPersonal + ', ' + PER.vNombresPersonal AS vNombreCompletoPersonal " &
                                                         "FROM LOGI_RECURSOSORDENTRABAJO AS RRHH INNER JOIN LOGI_CABECERAORDENTRABAJO AS ORDTRA ON " &
                                                         "     RRHH.cIdEmpresa = ORDTRA.cIdEmpresa AND " &
                                                         "     RRHH.cIdTipoDocumentoCabeceraOrdenTrabajo = ORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo AND " &
                                                         "     RRHH.vIdNumeroSerieCabeceraOrdenTrabajo = ORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo AND " &
                                                         "     RRHH.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                                                         "     INNER JOIN RRHH_PERSONAL AS PER ON" &
                                                         "     RRHH.cIdEmpresa = PER.cIdEmpresa AND " &
                                                         "     RRHH.cIdPersonal = PER.cIdPersonal " &
                                                         "     INNER JOIN LOGI_PLANIFICACIONEQUIPOCONTRATO AS PLAEQUCON ON " &
                                                         "     ORDTRA.cIdEmpresa = PLAEQUCON.cIdEmpresa AND ORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + ORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + ORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo = PLAEQUCON.vOrdenTrabajoReferenciaPlanificacionEquipoContrato " &
                                                         "WHERE ORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "' " &
                                                         "      AND ORDTRA.vContratoReferenciaCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "-" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "-" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "'")
            For Each ListaPersonal In dsPersonal.Rows
                AgregarCestaPersonalOrdenTrabajo(ListaPersonal("dFechaInicioPlanificadaCabeceraOrdenTrabajo"), ListaPersonal("cIdEquipoCabeceraOrdenTrabajo"), txtIdContrato.Text, ListaPersonal("bResponsableRecursosOrdenTrabajo"), ListaPersonal("cIdPersonal").ToString.Trim, ListaPersonal("vNombreCompletoPersonal").ToString.Trim, ListaPersonal("cIdTipoDocumentoCabeceraOrdenTrabajo") + "-" + ListaPersonal("vIdNumeroSerieCabeceraOrdenTrabajo") + "-" + ListaPersonal("vIdNumeroCorrelativoCabeceraOrdenTrabajo"), ListaPersonal("vOrdenTrabajoClientePlanificacionEquipoContrato"), Session("CestaPersonalOrdenTrabajo"))
            Next
            'Dim dsPersonal = ContratoNeg.ContratoGetData("SELECT cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, cIdPersonal, vObservacionRecursosOrdenTrabajo, bEstadoRegistroRecursosOrdenTrabajo, cIdEquipoCabeceraOrdenTrabajo, nTotalMinutosTrabajadosRecursosOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo, bResponsableRecursosOrdenTrabajo " &
            '                                             "FROM LOGI_RECURSOSORDENTRABAJO " &
            '                                             "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
            '                                             "IN " &
            '                                             "(SELECT cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
            '                                             "FROM LOGI_CABECERAORDENTRABAJO " &
            '                                             "WHERE vContratoReferenciaCabeceraOrdenTrabajo = '" & grdLista.SelectedRow.Cells(0).Text & "-" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "-" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "')")
            ''"WHERE CON.cIdEmpresa = '" & Session("IdEmpresa") & "' AND " &
            ''"      CON.cIdTipoDocumentoCabeceraContrato = '" & grdLista.SelectedRow.Cells(0).Text & "' AND " &
            ''"      CON.vIdNumeroSerieCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim & "' AND " &
            ''"      CON.vIdNumeroCorrelativoCabeceraContrato = '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim & "'")
            'For Each ListaPersonal In dsPersonal.Rows
            '    AgregarCestaPersonalOrdenTrabajo(ListaPersonal("dFechaInicioPlanificadaCabeceraOrdenTrabajo"), ListaPersonal("cIdEquipoCabeceraOrdenTrabajo"), txtIdContrato.Text, Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Personal").ToString.Trim, hfdIdOrdenTrabajo.Value, hfdIdOrdenTrabajoCliente.Value, Session("CestaPersonalOrdenTrabajo"))
            'Next
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Sub CargarCestaPersonalFiltrado()
        'Carga las opciones que aun no estan asignadas en la Grilla de Opciones del Módulo.
        Try
            VaciarCestaPersonal(Session("CestaSSPersonal"))
            Dim EquipoActivoNeg As New clsEquipoNegocios

            If Session("CestaPersonalOrdenTrabajo").Rows.Count > 0 Then
                'Dim resultPersonalSimple As DataRow() = Session("CestaPersonalOrdenTrabajo").Select("IdEquipo = '" & hfdIdEquipo.Value.Trim & "' AND FechaPlanificacion = '" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text & "' AND IdPersonal = '" & hfdIdAuxiliar.Value.Trim & "'")
                Dim resultPersonalSimple As DataRow() = Session("CestaPersonalOrdenTrabajo").Select("IdEquipo = '" & hfdIdEquipo.Value.Trim & "' AND FechaPlanificacion = '" & IIf(hfdOperacionDetalle.Value = "N", "", txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) & "'")
                If resultPersonalSimple.Length = 0 Then
                    Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Nothing
                Else
                    Dim rowFil As DataRow() = resultPersonalSimple
                    For Each fila As DataRow In rowFil
                        'clsLogiCestaCatalogoCaracteristica.AgregarCesta(fila("IdCatalogo"), fila("IdEquipo"), fila("IdJerarquia"), fila("IdCaracteristica"), fila("Descripcion"), fila("IdReferenciaSAP"), fila("DescripcionCampoSAP"), fila("Valor"), Session("CestaCaracteristicaEquipoComponenteFiltrado"))
                        AgregarCestaPersonal(fila("IdPersonal"), UCase(fila("Personal")), fila("Responsable"), Session("CestaSSPersonal"))
                    Next
                    Exit Sub
                End If
            End If

            'If grdEquipoComponente.SelectedIndex >= (grdEquipoComponente.Rows.Count - 1) Then
            '    grdEquipoComponente.SelectedIndex = -1
            'End If

            'If IsNothing(grdEquipoComponente.SelectedRow) = False Then
            '    If IsReference(grdEquipoComponente.SelectedRow.Cells(1).Text) = True Then
            '        Dim Coleccion = EquipoNeg.EquipoListaBusqueda("cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)

            '        Dim intContador As Integer = 0

            '        For Each Registro In Coleccion
            '            Dim Equipo1 As New LOGI_EQUIPO
            '            Equipo1 = EquipoNeg.EquipoListarPorIdDetalle(Registro.Codigo, Registro.IdCatalogo)

            '            clsLogiCestaEquipo.AgregarCesta(Equipo1.cIdEquipo, Equipo1.cIdCatalogo, Equipo1.cIdTipoActivo, Equipo1.cIdJerarquiaCatalogo,
            '                                    Equipo1.cIdSistemaFuncionalEquipo, Equipo1.cIdEnlaceCatalogo, Equipo1.vDescripcionEquipo,
            '                                    Equipo1.vDescripcionAbreviadaEquipo, IIf(IsNothing(Equipo1.dFechaTransaccionEquipo), Now, Equipo1.dFechaTransaccionEquipo), Equipo1.bEstadoRegistroEquipo,
            '                                    Equipo1.cIdEnlaceEquipo, Equipo1.vObservacionEquipo, Equipo1.nVidaUtilEquipo,
            '                                    "", "", Equipo1.nPeriodoGarantiaEquipo, Equipo1.nPeriodoMinimoMantenimientoEquipo,
            '                                    Equipo1.vNumeroSerieEquipo, Equipo1.vNumeroParteEquipo, Equipo1.cIdEstadoComponenteEquipo,
            '                                   Session("CestaEquipoComponente"))

            '            Dim TablaCestaMaestro As DataTable
            '            TablaCestaMaestro = Session("CestaEquipoComponente")
            '            Dim i As Integer

            '            For i = 0 To TablaCestaMaestro.Rows.Count - 1
            '                If TablaCestaMaestro.Rows(i)("IdTipoActivo") = cboTipoActivo.SelectedValue And
            '                   TablaCestaMaestro.Rows(i)("IdCatalogo") = cboCatalogo.SelectedValue Then
            '                    QuitarCesta(intContador, Session("CestaCatalogoComponente"))
            '                    intContador = intContador - 1
            '                End If
            '            Next
            '            intContador = intContador + 1
            '        Next
            '    End If
            'Else
            '    Dim Coleccion = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceCatalogo = '" & cboCatalogo.SelectedValue & "' AND EQU.cIdEnlaceEquipo", IIf(txtIdEquipo.Text.Trim = "", "*", txtIdEquipo.Text.Trim), 1)
            '    Dim intContador As Integer = 0

            '    For Each Registro In Coleccion
            '        Dim Equipo As LOGI_EQUIPO
            '        Equipo = EquipoNeg.EquipoListarPorIdDetalle(Registro.Codigo, Registro.IdCatalogo)
            '        clsLogiCestaEquipo.AgregarCesta(Equipo.cIdEquipo, Equipo.cIdCatalogo, Equipo.cIdTipoActivo, Equipo.cIdJerarquiaCatalogo,
            '                                    Equipo.cIdSistemaFuncionalEquipo, Equipo.cIdEnlaceCatalogo, Equipo.vDescripcionEquipo,
            '                                    Equipo.vDescripcionAbreviadaEquipo, IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo), Equipo.bEstadoRegistroEquipo,
            '                                    Equipo.cIdEnlaceEquipo, Equipo.vObservacionEquipo, Equipo.nVidaUtilEquipo,
            '                                    "", "", Equipo.nPeriodoGarantiaEquipo, Equipo.nPeriodoMinimoMantenimientoEquipo,
            '                                    Equipo.vNumeroSerieEquipo, Equipo.vNumeroParteEquipo, Equipo.cIdEstadoComponenteEquipo,
            '                                   Session("CestaEquipoComponente"))
            '        Dim TablaCestaMaestro As DataTable
            '        Dim TablaCestaCatalogo As DataTable
            '        TablaCestaMaestro = Session("CestaEquipoComponente")
            '        TablaCestaCatalogo = Session("CestaCatalogoComponente")
            '        Dim i As Integer
            '        For i = 0 To TablaCestaMaestro.Rows.Count - 1
            '            For j = 0 To TablaCestaCatalogo.Rows.Count - 1
            '                If TablaCestaMaestro.Rows(i)("IdJerarquia") = TablaCestaCatalogo.Rows(j)("IdJerarquia") And
            '                   TablaCestaMaestro.Rows(i)("IdCatalogo") = TablaCestaCatalogo.Rows(j)("IdCatalogo") Then
            '                    QuitarCesta(intContador, Session("CestaCatalogoComponente"))
            '                    intContador = intContador - 1
            '                    Exit For
            '                End If
            '            Next
            '            intContador = intContador + 1
            '        Next
            '    Next
            'End If
            'Me.grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            'Me.grdCatalogoComponente.DataBind()
            'Me.grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            'Me.grdEquipoComponente.DataBind()
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
        cboFiltroContrato.DataTextField = "vDescripcionTablaSistema"
        cboFiltroContrato.DataValueField = "vValor"
        cboFiltroContrato.DataSource = FiltroNeg.TablaSistemaListarCombo("88", "LOGI", Session("IdEmpresa"))
        cboFiltroContrato.Items.Clear()
        cboFiltroContrato.DataBind()
    End Sub

    Sub ListarPeriodoCombo()
        'Hay que hacer referencia a la Capa de Datos
        'porque se encuentran las entidades en dicha capa.
        Dim PeriodoNeg As New clsTipoCambioNegocios
        cboPeriodo.DataSource = PeriodoNeg.TipoCambioPeriodoListarCombo
        cboPeriodo.Items.Clear()
        cboPeriodo.DataBind()
    End Sub

    Sub ListarPersonalAsignadoCombo(ByVal Contrato As LOGI_CABECERACONTRATO)
        'JMUG: 23/07/2023

        'Dim PersonalNeg As New clsContratoNegocios
        'cboPersonalAsignadoMantenimientoTarea.DataTextField = "vNombreCompletoPersonal"
        'cboPersonalAsignadoMantenimientoTarea.DataValueField = "cIdPersonal"
        'cboPersonalAsignadoMantenimientoTarea.DataSource = PersonalNeg.ContratoRecursosListarCombo(Contrato)
        'cboPersonalAsignadoMantenimientoTarea.Items.Clear()
        'cboPersonalAsignadoMantenimientoTarea.Items.Add("SELECCIONE DATO")
        'cboPersonalAsignadoMantenimientoTarea.DataBind()
    End Sub

    'Sub ListarInsumosCombo()

    '    Dim ValoresOS() As String = hfdOrdenFabricacionReferencia.Value.ToString.Split("-")
    '    Dim InsumosNeg As New clsDetalleContratoNegocios
    '    cboInsumosAgregarInsumos.DataTextField = "vDescripcionArticuloSAPDetalleOrdenFabricacion"
    '    cboInsumosAgregarInsumos.DataValueField = "vIdArticuloSAPDetalleOrdenFabricacion"
    '    cboInsumosAgregarInsumos.DataSource = InsumosNeg.DetalleContratoGetData("SELECT * FROM LOGI_DETALLEORDENFABRICACION " &
    '                                                                  "WHERE cIdTipoDocumentoCabeceraOrdenFabricacion = '" & ValoresOS(0).ToString & "' " &
    '                                                                  "      AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & ValoresOS(1).ToString & "' " &
    '                                                                  "      AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & ValoresOS(2).ToString & "' " &
    '                                                                  "      AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
    '    cboInsumosAgregarInsumos.Items.Clear()
    '    cboInsumosAgregarInsumos.Items.Add("SELECCIONE DATO")
    '    cboInsumosAgregarInsumos.DataBind()
    'End Sub


    'Sub ListarOtrosDatosCombo()
    '    'Hay que hacer referencia a la Capa de Datos
    '    'porque se encuentran las entidades en dicha capa.
    '    Dim CaracteristicaNeg As New clsCaracteristicaNegocios
    '    cboCaracteristicaOtrosDatos.DataTextField = "vDescripcionCaracteristica"
    '    cboCaracteristicaOtrosDatos.DataValueField = "cIdCaracteristica"
    '    cboCaracteristicaOtrosDatos.DataSource = CaracteristicaNeg.CaracteristicaListarCombo()
    '    cboCaracteristicaOtrosDatos.Items.Clear()
    '    cboCaracteristicaOtrosDatos.DataBind()
    'End Sub

    Sub BloquearMantenimiento(ByVal bNuevo As Boolean, ByVal bGuardar As Boolean, ByVal bEditar As Boolean, ByVal bDeshacer As Boolean)
        Me.btnNuevo.Visible = bNuevo
        'Me.btnEditar.Visible = bEditar
        'Me.btnGuardar.Visible = bGuardar
        'Me.btnDeshacer.Visible = bDeshacer
    End Sub

    'Sub LimpiarObjetosOtrosDatos()

    '    'Me.lblMensajeCaracteristica.Text = ""
    '    'txtValorCaracteristica.Text = ""
    '    'txtIdReferenciaSAPCaracteristica.Text = ""
    '    'txtDescripcionCampoSAPCaracteristica.Text = ""
    '    'divIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '    'txtIdReferenciaSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0660", strOpcionModulo, Session("IdSistema")) 'Id. Ref. SAP
    '    'divDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    '    'txtDescripcionCampoSAPCaracteristica.Visible = FuncionesNeg.ValidaPerfilObjeto(Session("IdUsuario"), Session("IdPerfil"), "0661", strOpcionModulo, Session("IdSistema")) 'Campo SAP
    'End Sub

    Sub ActivarObjetos(ByVal bActivar As Boolean)
        txtIdContrato.Enabled = False 'bActivar
        'cboTipoActivo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        'cboCatalogo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        txtDescripcionContrato.Enabled = bActivar
        txtNroLicitacionContrato.Enabled = bActivar
        txtFechaEmisionContrato.Enabled = bActivar
        txtFechaVigenciaInicio.Enabled = bActivar
        txtFechaVigenciaFinal.Enabled = bActivar
        'fscIdentificadorComponente.Disabled = Not bActivar
        'cboTipoActivo.Enabled = bActivar
        'cboCatalogo.Enabled = bActivar
        'cboSistemaFuncional.Enabled = bActivar
        ''divSistemaFuncional.Visible = IIf(fscIdentificadorComponente.Checked, True, False)
        'divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        ''cboCatalogoComponente.Enabled = bActivar
        txtIdCliente.Enabled = bActivar
        txtRazonSocial.Enabled = False
        'cboEmpresa.Visible = IIf(Session("IdTipoUsuario") = "U", False, True)

        ''Mantenimiento Tipo Activo
        'txtIdTipoActivoMantenimientoTipoActivo.Enabled = False
        'txtDescripcionMantenimientoTipoActivo.Enabled = bActivar
        'txtDescripcionAbreviadaMantenimientoTipoActivo.Enabled = bActivar

        ''Mantenimiento de Catalogo
        'txtIdCatalogoMantenimientoCatalogo.Enabled = False
        'cboTipoActivoMantenimientoCatalogo.Enabled = bActivar
        'txtDescripcionMantenimientoCatalogo.Enabled = bActivar
        'txtDescripcionAbreviadaMantenimientoCatalogo.Enabled = bActivar
        'txtVidaUtilMantenimientoCatalogo.Enabled = bActivar
        'txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        'txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        'txtCuentaContableMantenimientoCatalogo.Enabled = bActivar
        'divCuentaContableMantenimientoCatalogo.Visible = False
        'txtCuentaContableLeasingMantenimientoCatalogo.Enabled = bActivar
        'divCuentaContableLeasingMantenimientoCatalogo.Visible = False

        ''Mantenimiento de Catalogo / Características

        ''Mantenimiento de Equipo

        ''Mantenimiento de Equipo / Componente

        ''Mantenimiento de Catalogo / Componente
        'txtIdCatalogoComponente.Enabled = False
        'cboTipoActivoCatalogoComponente.Enabled = bActivar
        'cboSistemaFuncionalCatalogoComponente.Enabled = bActivar
        'txtDescripcionCatalogoComponente.Enabled = bActivar
        'txtDescripcionCatalogoComponente.Enabled = bActivar
        'txtVidaUtilCatalogoComponente.Enabled = bActivar
        'txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        'txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        'txtCuentaContableCatalogoComponente.Enabled = bActivar
        'txtCuentaContableLeasingCatalogoComponente.Enabled = bActivar

        ''Mantenimiento de Sistema Funcional / Componente
        'txtIdSistemaFuncionalMantenimientoSistemaFuncional.Enabled = False
        'txtDescripcionMantenimientoSistemaFuncional.Enabled = bActivar
        'txtDescripcionAbreviadaMantenimientoSistemaFuncional.Enabled = bActivar
    End Sub

    Sub LlenarData()
        'Try 'JMUG: LO QUITE 11/09/2023
        'Dim Contrato As LOGI_CABECERACONTRATO = ContratoNeg.ContratoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(6).Text).Trim)
        Dim Contrato As LOGI_CABECERACONTRATO = ContratoNeg.ContratoListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Session("IdEmpresa"))
        txtIdContrato.Text = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato.Trim + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato.Trim
        'hfdFechaEquipo.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
        txtDescripcionContrato.Text = Contrato.vDescripcionCabeceraContrato
        txtNroLicitacionContrato.Text = Contrato.vNroLicitacionCabeceraContrato
        txtFechaEmisionContrato.Text = Contrato.dFechaEmisionCabeceraContrato
        txtFechaVigenciaInicio.Text = Contrato.dFechaVigenciaInicialCabeceraContrato
        txtFechaVigenciaFinal.Text = Contrato.dFechaVigenciaFinalCabeceraContrato

        'Dim dFechaInicial, dFechaFinal As Date
        'dFechaInicial = DateTime.ParseExact(strFechaInicial, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
        'dFechaFinal = DateTime.ParseExact(strFechaFinal, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
        cboPeriodo.SelectedValue = Year(IIf(IsNothing(Contrato.dFechaVigenciaFinalCabeceraContrato) Or Contrato.dFechaVigenciaFinalCabeceraContrato > Now, Now, Contrato.dFechaVigenciaFinalCabeceraContrato))
        cboMes.SelectedValue = Month(IIf(IsNothing(Contrato.dFechaVigenciaFinalCabeceraContrato) Or Contrato.dFechaVigenciaFinalCabeceraContrato > Now, Now, Contrato.dFechaVigenciaFinalCabeceraContrato))

        'txtNroParteEquipo.Text = Equipo.vNumeroParteEquipo
        'lnkEsComponenteOn.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", False, True)
        'lnkEsComponenteOff.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", True, False)
        'lnkEsConContratoOn.Visible = IIf(Equipo.bTieneContratoEquipo = False, False, True)
        'lnkEsConContratoOff.Visible = IIf(Equipo.bTieneContratoEquipo = False, True, False)
        'divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        Dim ClienteNeg As New clsClienteNegocios
        'JMUG: 22/07/2023 Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorIdSAP(Equipo.vIdClienteSAPEquipo, Session("IdEmpresa"))
        Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorId(Contrato.cIdCliente, Session("IdEmpresa"), "*")
        txtIdCliente.Text = Cliente.vRucCliente
        btnAdicionarCliente_Click(btnAdicionarCliente, New System.EventArgs())
        ''JMUG: 23/07/2023
        'ListarClienteUbicacionCombo()
        ''txtDescripcionAbreviada.Text = MaestroActivo.vDescripcionAbreviadaMaestroActivo
        ''txtCuentaContableActivoMayor.Text = MaestroActivo.cIdCuentaContableMaestroActivo
        ''txtObservacion.Text = MaestroActivo.vObservacionMaestroActivo
        'cboClienteUbicacion.SelectedValue = IIf(Trim(Equipo.cIdClienteUbicacion) = "", "SELECCIONE DATO", Equipo.cIdClienteUbicacion)
        'cboTipoActivo.SelectedValue = Equipo.cIdTipoActivo
        'cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())
        'cboCatalogo.SelectedValue = Equipo.cIdCatalogo
        'ListarSistemaFuncionalCombo()
        ''JMUG: 25/02/2023 lblSistemaFuncional.Value = Equipo.cIdSistemaFuncional
        ''cboSistemaFuncional.SelectedValue = MaestroActivo.cIdSistemaFuncional
        ''cboTipoActivoDetalleMaestroActivo_SelectedIndexChanged(cboTipoActivoDetalleMaestroActivo, New System.EventArgs())

        ''JMUG: 25/02/2023 LlenarDataEquipo()
        CargarCestaEquipos()
        'CargarCestaCatalogoComponente()
        'CargarCestaEquipoComponente()
        'CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 20/03/2023
        'CargarCestaCaracteristicaEquipoComponente()
        'CargarCestaCaracteristicaEquipoComponenteTemporal()
        'Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
        'Me.grdDetalleCaracteristicaEquipoComponente.DataBind()

        'hfdIdClienteSAPEquipo.Value = Equipo.vIdClienteSAPEquipo
        'hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
        'hfdFechaRegistroTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaRegistroTarjetaSAPEquipo), "", Equipo.dFechaRegistroTarjetaSAPEquipo)
        'hfdFechaManufacturaTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaManufacturaTarjetaSAPEquipo), "", Equipo.dFechaManufacturaTarjetaSAPEquipo)
        'JMUG: 23/07/2023
        hfdFechaCreacionContrato.Value = IIf(IsNothing(Contrato.dFechaCreacionCabeceraContrato), Now, Contrato.dFechaCreacionCabeceraContrato)
        hfdIdUsuarioCreacionContrato.Value = IIf(IsNothing(Contrato.cIdUsuarioCreacionCabeceraContrato), Session("IdUsuario"), Contrato.cIdUsuarioCreacionCabeceraContrato)
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

    Sub LlenarDataEquipo(ByVal ValoresReferenciales() As String)
        'Try
        'Dim EquipoNeg As New clsEquipoNegocios
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(grdDetalleEquipos.SelectedRow.Cells(1).Text)
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(ValoresReferenciales(1).ToString)
        'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(ValoresReferenciales(3).ToString)
        'Dim Equipo As LOGI_EQUIPO
        'If hfdOperacionDetalle.Value = "N" Then
        '    Equipo = EquipoNeg.EquipoListarPorId(ValoresReferenciales(1).ToString)
        'Else
        '    'Equipo = EquipoNeg.EquipoListarPorId(ValoresReferenciales(3).ToString)
        '    'Equipo = EquipoNeg.EquipoListarPorId(ValoresReferenciales(5).ToString)
        '    Equipo = EquipoNeg.EquipoListarPorId(ValoresReferenciales(6).ToString)
        'End If
        Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(ValoresReferenciales(7).ToString)
        hfdIdEquipo.Value = Equipo.cIdEquipo
        hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
        hfdIdCatalogoEquipo.Value = Equipo.cIdCatalogo
        hfdIdTipoActivoEquipo.Value = Equipo.cIdTipoActivo
        hfdJerarquiaEquipo.Value = Equipo.cIdJerarquiaCatalogo
        'hfdFechaEquipo.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
        'lblDescripcionEquipo.Text = Equipo.vDescripcionEquipo
        txtDescripcionContrato.Text = Equipo.vDescripcionEquipo
        'lblNroSerieEquipo.Text = Equipo.vNumeroSerieEquipo
        'lblNroParteEquipo.Text = Equipo.vNumeroParteEquipo
        'lnkEsComponenteOn.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", False, True)
        'lnkEsComponenteOff.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", True, False)
        'divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
        'Dim TipActNeg As New clsTipoActivoNegocios
        'lblTipoActivoEquipo.Text = TipActNeg.TipoActivoListarPorId(Equipo.cIdTipoActivo).vDescripcionTipoActivo
        'Dim CatNeg As New clsCatalogoNegocios
        'lblCatalogoEquipo.Text = CatNeg.CatalogoListarPorId(Equipo.cIdCatalogo, Equipo.cIdTipoActivo, Equipo.cIdJerarquiaCatalogo, "1").vDescripcionCatalogo
        'Dim SisFunNeg As New clsSistemaFuncionalNegocios
        'lblSistemaFuncionalEquipo.Text = SisFunNeg.SistemaFuncionalListarPorId(Equipo.cIdSistemaFuncionalEquipo).vDescripcionSistemaFuncional
        Dim ClienteNeg As New clsClienteNegocios
        Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorIdSAP(Equipo.vIdClienteSAPEquipo, Session("IdEmpresa"))
        'CargarCestaInsumos()
        ''CargarCestaInsumosTemporal()

        'If lblMensajeMantenimientoOrdenTrabajo.Text = "" Then
        '    lblMensajeMantenimientoOrdenTrabajo.Text = "Registro encontrado con éxito"
        'End If
        'If MyValidator.ErrorMessage = "" Then
        '    MyValidator.ErrorMessage = "Registro encontrado con éxito"
        'End If
        'ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        'MyValidator.IsValid = False
        'MyValidator.ID = "ErrorPersonalizado"
        'MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        'Me.Page.Validators.Add(MyValidator)
        'Catch ex As Exception
        '    'lblMensaje.Text = ex.Message
        '    ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
        '    MyValidator.ErrorMessage = ex.Message
        '    MyValidator.IsValid = False
        '    MyValidator.ID = "ErrorPersonalizado"
        '    MyValidator.ValidationGroup = "vgrpValidarBusqueda"
        '    Me.Page.Validators.Add(MyValidator)
        'End Try
    End Sub

    Sub ValidarTexto(ByVal bValidar As Boolean)
        Me.rfvDescripcionContrato.EnableClientScript = bValidar
        Me.rfvFechaEmisionContrato.EnableClientScript = bValidar
        Me.rfvFechaVigenciaInicio.EnableClientScript = bValidar
        Me.rfvFechaVigenciaFinal.EnableClientScript = bValidar
        'Me.rfvCatalogo.EnableClientScript = bValidar
        'Me.rfvSistemaFuncional.EnableClientScript = bValidar
        Me.rfvIdCliente.EnableClientScript = bValidar
        Me.rfvRazonSocial.EnableClientScript = bValidar
        'Me.rfvPerfil.EnableClientScript = bValidar
    End Sub

    Sub LimpiarObjetos()
        MyValidator.ErrorMessage = ""
        txtIdContrato.Text = ""
        txtDescripcionContrato.Text = ""
        txtNroLicitacionContrato.Text = ""
        txtFechaEmisionContrato.Text = String.Format("{0:dd/MM/yyyy}", Now)
        txtFechaVigenciaInicio.Text = String.Format("{0:dd/MM/yyyy}", Now)
        txtFechaVigenciaFinal.Text = String.Format("{0:dd/MM/yyyy}", Now)
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
        'hfdIdClienteSAPEquipo.Value = ""
        'hfdIdEquipoSAPEquipo.Value = ""
        hfdFechaCreacionContrato.Value = ""
        hfdIdUsuarioCreacionContrato.Value = ""
        'hfdOrdenFabricacionReferencia.Value = ""

        VaciarCestaEquipo(Session("CestaEquipoContrato"))
        VaciarCestaPersonal(Session("CestaSSPersonal"))
        VaciarCestaPersonalOrdenTrabajo(Session("CestaPersonalOrdenTrabajo"))
    End Sub

    Sub LimpiarObjetosOrdenTrabajo()
        lblMensajeMantenimientoOrdenTrabajo.Text = ""
        'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = ""
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = "01/" & Format(CInt(cboMes.SelectedValue), "00") & "/" & cboPeriodo.SelectedValue
        txtNroOTClienteMantenimientoOrdenTrabajo.Text = ""
        cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = -1
        cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
        VaciarCestaPersonal(Session("CestaSSPersonal"))
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
        grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
    End Sub

    Sub ValidarTextoOrdenTrabajo(ByVal bValidar As Boolean)
        Me.rfvTipoMantenimientoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalResponsableMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        Me.rfvPersonalAsignadoMantenimientoOrdenTrabajo.EnableClientScript = bValidar
        'Me.rfvDescripcionEquipo.EnableClientScript = bValidar
        'Me.rfvCatalogo.EnableClientScript = bValidar
        'Me.rfvSistemaFuncional.EnableClientScript = bValidar
        'Me.rfvIdCliente.EnableClientScript = bValidar
        'Me.rfvRazonSocial.EnableClientScript = bValidar
        'Me.rfvPerfil.EnableClientScript = bValidar
    End Sub

    Sub ActivarObjetosOrdenTrabajo(ByVal bActivar As Boolean)
        txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Enabled = bActivar
        cboTipoMantenimientoMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalResponsableMantenimientoOrdenTrabajo.Enabled = bActivar
        cboPersonalAsignadoMantenimientoOrdenTrabajo.Enabled = bActivar
        'txtIdEquipo.Enabled = False 'bActivar
        ''cboTipoActivo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        ''cboCatalogo.Enabled = IIf(hfdOperacion.Value = "E", False, bActivar)
        'txtDescripcionEquipo.Enabled = bActivar
        ''fscIdentificadorComponente.Disabled = Not bActivar
        'cboTipoActivo.Enabled = bActivar
        'cboCatalogo.Enabled = bActivar
        'cboSistemaFuncional.Enabled = bActivar
        ''divSistemaFuncional.Visible = IIf(fscIdentificadorComponente.Checked, True, False)
        'divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, False, True)
        ''cboCatalogoComponente.Enabled = bActivar
        'txtIdCliente.Enabled = bActivar
        'txtRazonSocial.Enabled = False
        ''cboEmpresa.Visible = IIf(Session("IdTipoUsuario") = "U", False, True)

        ''Mantenimiento Tipo Activo
        'txtIdTipoActivoMantenimientoTipoActivo.Enabled = False
        'txtDescripcionMantenimientoTipoActivo.Enabled = bActivar
        'txtDescripcionAbreviadaMantenimientoTipoActivo.Enabled = bActivar

        ''Mantenimiento de Catalogo
        'txtIdCatalogoMantenimientoCatalogo.Enabled = False
        'cboTipoActivoMantenimientoCatalogo.Enabled = bActivar
        'txtDescripcionMantenimientoCatalogo.Enabled = bActivar
        'txtDescripcionAbreviadaMantenimientoCatalogo.Enabled = bActivar
        'txtVidaUtilMantenimientoCatalogo.Enabled = bActivar
        'txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        'txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        'txtCuentaContableMantenimientoCatalogo.Enabled = bActivar
        'divCuentaContableMantenimientoCatalogo.Visible = False
        'txtCuentaContableLeasingMantenimientoCatalogo.Enabled = bActivar
        'divCuentaContableLeasingMantenimientoCatalogo.Visible = False

        ''Mantenimiento de Catalogo / Características

        ''Mantenimiento de Equipo

        ''Mantenimiento de Equipo / Componente

        ''Mantenimiento de Catalogo / Componente
        'txtIdCatalogoComponente.Enabled = False
        'cboTipoActivoCatalogoComponente.Enabled = bActivar
        'cboSistemaFuncionalCatalogoComponente.Enabled = bActivar
        'txtDescripcionCatalogoComponente.Enabled = bActivar
        'txtDescripcionCatalogoComponente.Enabled = bActivar
        'txtVidaUtilCatalogoComponente.Enabled = bActivar
        'txtPeriodoGarantiaMantenimientoCatalogo.Enabled = bActivar
        'txtPeriodoMinimoMantenimientoCatalogo.Enabled = bActivar
        'txtCuentaContableCatalogoComponente.Enabled = bActivar
        'txtCuentaContableLeasingCatalogoComponente.Enabled = bActivar

        ''Mantenimiento de Sistema Funcional / Componente
        'txtIdSistemaFuncionalMantenimientoSistemaFuncional.Enabled = False
        'txtDescripcionMantenimientoSistemaFuncional.Enabled = bActivar
        'txtDescripcionAbreviadaMantenimientoSistemaFuncional.Enabled = bActivar
    End Sub

    Sub LlenarDataOrdenTrabajo(ByVal ValoresReferenciales() As String)
        Try
            'ValoresReferenciales(0) -> TipoMantenimiento
            'ValoresReferenciales(1) -> Contrato
            'ValoresReferenciales(2) -> CheckList
            'ValoresReferenciales(3) -> FechaActividad
            'ValoresReferenciales(4) -> Orden de Trabajo
            'ValoresReferenciales(5) -> Nro.Item
            'ValoresReferenciales(6) -> IdEquipo
            'Dim Equipo As LOGI_EQUIPO = OrdenFabricacionNeg.MaestroActivoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
            'Dim Equipo As LOGI_EQUIPO = OrdenFabricacionNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, grdLista.SelectedRow.Cells(3).Text, grdLista.SelectedRow.Cells(2).Text)
            'Dim Equipo As LOGI_EQUIPO = OrdenFabricacionNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(1).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim)
            'Dim Equipo As LOGI_EQUIPO = OrdenFabricacionNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(5).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(4).Text).Trim)
            'Dim Equipo As LOGI_EQUIPO = OrdenFabricacionNeg.EquipoListarPorIdDetalle(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(6).Text).Trim)
            'Dim SolicitudServicio As LOGI_CABECERAORDENFABRICACION = OrdenFabricacionNeg.OrdenFabricacionListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Session("IdEmpresa"))
            Dim OrdTraNeg As New clsOrdenTrabajoNegocios
            If hfdOperacionDetalle.Value = "N" Then
                'txtIdContrato.Text = ""
                hfdIdOrdenTrabajo.Value = ""
                hfdIdOrdenTrabajoCliente.Value = ""
                hfdFechaPlanificacion.Value = ""
            Else
                txtIdContrato.Text = ValoresReferenciales(1) 'IdContrato
                txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = ValoresReferenciales(3).ToString
                txtNroOTClienteMantenimientoOrdenTrabajo.Text = ValoresReferenciales(5).ToString
                hfdFechaPlanificacion.Value = ValoresReferenciales(3)
                hfdIdOrdenTrabajo.Value = ValoresReferenciales(4)
                hfdIdOrdenTrabajoCliente.Value = ValoresReferenciales(5)
            End If
            'Dim OrdenTrabajo As LOGI_CABECERAORDENTRABAJO = OrdTraNeg.OrdenTrabajoListarPorId(grdLista.SelectedRow.Cells(0).Text, Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim, Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim, Session("IdEmpresa"))
            'If hfdIdContrato.Value <> "" Then
            If txtIdContrato.Text <> "" And hfdIdOrdenTrabajo.Value.Trim.Length <> 0 Then
                'Dim OrdenTrabajo As LOGI_CABECERAORDENTRABAJO = OrdTraNeg.OrdenTrabajoListarPorId("CN", "0001", hfdIdContrato.Value, Session("IdEmpresa"))
                Dim strOT = hfdIdOrdenTrabajo.Value.Trim.Split("-")
                Dim OrdenTrabajo As LOGI_CABECERAORDENTRABAJO = OrdTraNeg.OrdenTrabajoListarPorId(strOT(0), strOT(1), strOT(2), Session("IdEmpresa"))
            End If
            LlenarDataEquipo(ValoresReferenciales)
            ListarTipoMantenimientoCombo()
            If hfdOperacionDetalle.Value = "E" Then
                cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue = ValoresReferenciales(0) 'IdTipoMantenimiento
            End If
            ListarPlantillaCheckListCombo()
            If hfdOperacionDetalle.Value = "E" Then
                cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = ValoresReferenciales(2) 'IdPlantilla Check List
            End If
            ListarPersonalResponsableCombo()
            ListarPersonalCombo()
            CargarCestaPersonalFiltrado()
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            'Dim EquipoNeg As New clsEquipoNegocios
            'Dim Equipo As LOGI_EQUIPO = EquipoNeg.EquipoListarPorId(Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim)
            'hfdIdCatalogoEquipo.Value = Equipo.cIdCatalogo
            'txtIdEquipo.Text = SolicitudServicio.cIdEquipo
            'hfdFechaEquipo.Value = IIf(IsNothing(Equipo.dFechaTransaccionEquipo), Now, Equipo.dFechaTransaccionEquipo)
            'txtDescripcionEquipo.Text = Equipo.vDescripcionEquipo
            'txtNroSerieEquipo.Text = Equipo.vNumeroSerieEquipo
            'txtNroParteEquipo.Text = Equipo.vNumeroParteEquipo
            'lnkEsComponenteOn.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", False, True)
            'lnkEsComponenteOff.Visible = IIf(Equipo.cIdJerarquiaCatalogo = "0", True, False)
            'divSistemaFuncional.Visible = IIf(lnkEsComponenteOn.Visible = True, True, False)
            'Dim ClienteNeg As New clsClienteNegocios
            'Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorIdSAP(Equipo.vIdClienteSAPEquipo, Session("IdEmpresa"))
            'txtIdCliente.Text = Cliente.vRucCliente
            'btnAdicionarCliente_Click(btnAdicionarCliente, New System.EventArgs())
            'ListarClienteUbicacionCombo()
            ''txtDescripcionAbreviada.Text = MaestroActivo.vDescripcionAbreviadaMaestroActivo
            ''txtCuentaContableActivoMayor.Text = MaestroActivo.cIdCuentaContableMaestroActivo
            ''txtObservacion.Text = MaestroActivo.vObservacionMaestroActivo
            'cboClienteUbicacion.SelectedValue = IIf(Trim(Equipo.cIdClienteUbicacion) = "", "SELECCIONE DATO", Equipo.cIdClienteUbicacion)
            'cboTipoActivo.SelectedValue = Equipo.cIdTipoActivo
            'cboTipoActivo_SelectedIndexChanged(cboTipoActivo, New System.EventArgs())
            'cboCatalogo.SelectedValue = Equipo.cIdCatalogo
            'ListarSistemaFuncionalCombo()
            ''JMUG: 25/02/2023 lblSistemaFuncional.Value = Equipo.cIdSistemaFuncional
            ''cboSistemaFuncional.SelectedValue = MaestroActivo.cIdSistemaFuncional
            ''cboTipoActivoDetalleMaestroActivo_SelectedIndexChanged(cboTipoActivoDetalleMaestroActivo, New System.EventArgs())

            ''JMUG: 25/02/2023 LlenarDataEquipo()

            ''CargarCestaCatalogoComponente()
            ''CargarCestaEquipoComponente()
            ''CargarCestaCaracteristicaEquipoPrincipal() 'JMUG: 20/03/2023
            ''CargarCestaCaracteristicaEquipoComponente()
            '''CargarCestaMaestroActivo()
            'hfdIdClienteSAPEquipo.Value = Equipo.vIdClienteSAPEquipo
            'hfdIdEquipoSAPEquipo.Value = Equipo.cIdEquipoSAPEquipo
            'hfdFechaRegistroTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaRegistroTarjetaSAPEquipo), "", Equipo.dFechaRegistroTarjetaSAPEquipo)
            'hfdFechaManufacturaTarjetaSAPEquipo.Value = IIf(IsNothing(Equipo.dFechaManufacturaTarjetaSAPEquipo), "", Equipo.dFechaManufacturaTarjetaSAPEquipo)
            'hfdFechaCreacionContrato.Value = IIf(IsNothing(Equipo.dFechaCreacionEquipo), Now, Equipo.dFechaCreacionEquipo)
            'hfdIdUsuarioCreacionEquipo.Value = IIf(IsNothing(Equipo.cIdUsuarioCreacionEquipo), Session("IdUsuario"), Equipo.cIdUsuarioCreacionEquipo)
            ''MaestroActivo.cIdCodigoEnlaceMaestroActivo
            ''MaestroActivo.cIdJerarquiaMaestroActivo
            ''MaestroActivo.cIdTipoActivoMaestroActivo
            'If MyValidator.ErrorMessage = "" Then
            '    'lblMensaje.Text = "Registro encontrado con éxito"
            '    MyValidator.ErrorMessage = "Registro encontrado con éxito"
            'End If
            'ValidationSummary2.ValidationGroup = "vgrpValidarBusqueda"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            'Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            'lblMensaje.Text = ex.Message
            ValidationSummary2.ValidationGroup = "vgrpValidarBusqueda"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarBusqueda"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Sub LimpiarObjetosImagen()
    '    MyValidator.ErrorMessage = ""
    '    txtTituloSubirImagenActividad.Text = ""
    '    txtDescripcionSubirImagenActividad.Text = ""
    '    txtObservacionSubirImagenActividad.Text = ""
    'End Sub

    Function fValidarSiHayMovimientosOrdenTrabajo(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroDoc As String) As Boolean
        fValidarSiHayMovimientosOrdenTrabajo = False
        Dim dsContrato = ContratoNeg.ContratoGetData("SELECT vOrdenTrabajoReferenciaPlanificacionEquipoContrato " &
                                                     "FROM LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                                     "WHERE cIdTipoDocumentoCabeceraContrato = '" & IdTipDoc & "' AND vIdNumeroSerieCabeceraContrato = '" & IdNroSer & "' AND vIdNumeroCorrelativoCabeceraContrato = '" & IdNroDoc & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "'")
        For Each Fila In dsContrato.Rows
            If ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_TAREACHECKLISTORDENTRABAJO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Fila("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "'").Rows(0).Item(0) > 0 Then
                Return True
            ElseIf ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_GALERIACHECKLISTORDENTRABAJO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Fila("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "'").Rows(0).Item(0) > 0 Then
                Return True
            ElseIf ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_OTROSDATOSORDENTRABAJO WHERE cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + vIdNumeroSerieCabeceraOrdenTrabajo + '-' + vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & Fila("vOrdenTrabajoReferenciaPlanificacionEquipoContrato") & "'").Rows(0).Item(0) > 0 Then
                Return True
            End If
        Next
    End Function

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
        Dim PlantillaCheckListNeg As New clsCabeceraChecklistPlantillaNegocios
        cboListadoCheckListMantenimientoOrdenTrabajo.DataTextField = "vDescripcionCabeceraCheckListPlantilla"
        cboListadoCheckListMantenimientoOrdenTrabajo.DataValueField = "cIdNumeroCabeceraCheckListPlantilla"
        cboListadoCheckListMantenimientoOrdenTrabajo.DataSource = PlantillaCheckListNeg.ChecklistPlantillaListarCombo(cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue, hfdIdTipoActivoEquipo.Value, hfdIdCatalogoEquipo.Value, hfdJerarquiaEquipo.Value)
        cboListadoCheckListMantenimientoOrdenTrabajo.Items.Clear()
        cboListadoCheckListMantenimientoOrdenTrabajo.DataBind()
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
            'cboClienteUbicacion.SelectedIndex = -1
            hfdDireccionFiscalCliente.Value = ""
            hfdTelefonoContactoCliente.Value = ""
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
                    hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                    hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona
                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    'cboClienteUbicacion.SelectedValue = "00"
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
                ElseIf txtIdCliente.Text.Trim.Length = 11 Then 'cboTipoDoc.SelectedValue = "FA" And txtIdCliente.Text.Trim.Length = 11 Then
                    dsCliente = WSCliente.ConsultaRUC(txtIdCliente.Text)
                End If
                lblDatoInformativoMensajeCliente.Text = ""
                lblNroClienteMensajeCliente.Text = ""
                If dsCliente.Tables(0).Rows.Count > 0 Then
                    Dim fila As DataRow
                    For Each fila In dsCliente.Tables(0).Rows
                        If fila("vEstadoContribuyentePadronReducido") = "ACTIVO" And fila("vCondicionDomicilioPadronReducido") = "HABIDO" Then
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
                                    hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                                    hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona
                                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                                    hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                                    'cboClienteUbicacion.SelectedValue = "00"
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
                                    'cboClienteUbicacion.SelectedIndex = -1
                                    hfdDireccionFiscalCliente.Value = ""
                                    hfdTelefonoContactoCliente.Value = ""
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
                                Cliente.cIdTipoDocumento = IIf(txtIdCliente.Text.Trim.Length = 8, "01", "04")
                                Cliente.cIdTipoPersona = CChar(IIf(txtIdCliente.Text.Trim.Length = 8, "N", "J"))
                                Cliente.dFechaNacimientoCliente = Nothing
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
                                Cliente.vDireccionCliente = IIf(txtIdCliente.Text.Trim.Length = 8, "", Trim(fila("vDireccionFiscal")))
                                If fila("cIdTipoDocumento") <> "04" Then
                                    Cliente.vDniCliente = Trim(fila("vDNIPadronReducido"))
                                Else
                                    Cliente.vDniCliente = ""
                                End If
                                Cliente.vEmailCliente = ""
                                Cliente.vFaxCliente = ""
                                Cliente.vRazonSocialCliente = Trim(Mid(fila("vRazonSocialPadronReducido"), 1, IIf(InStrRev(fila("vRazonSocialPadronReducido"), "-") = 0, Len(fila("vRazonSocialPadronReducido")), InStrRev(fila("vRazonSocialPadronReducido"), "-") - 1)))
                                Cliente.vRepresentanteLegalCliente = ""
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
                                hfdNroDocumentoCliente.Value = IIf(fila("cIdTipoDocumento") = "04", hfdRUCCliente.Value, hfdDNICliente.Value)
                                'cboClienteUbicacion.SelectedIndex = -1
                                hfdDireccionFiscalCliente.Value = fila("vDireccionFiscal")
                                hfdTelefonoContactoCliente.Value = ""
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
                            'cboClienteUbicacion.SelectedIndex = -1
                            hfdDireccionFiscalCliente.Value = ""
                            hfdTelefonoContactoCliente.Value = ""
                            hfdCorreoElectronicoCliente.Value = ""
                            lblNroClienteMensajeCliente.Text = "No puede emitir comprobantes"
                            lblDatoInformativoMensajeCliente.Text = fila("vEstadoContribuyentePadronReducido") & "/" & fila("vCondicionDomicilioPadronReducido")
                            lnk_mostrarPanelMensajeCliente_ModalPopupExtender.Show()
                        End If
                    Next
                Else 'No existe - Porque ese RUC no está registrado en la SUNAT
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
                        hfdNroDocumentoCliente.Value = IIf(ClienteRegistrado(0).Tipo_Doc = "04", ClienteRegistradoFinal.vRucCliente, ClienteRegistradoFinal.vDniCliente)
                        txtRazonSocial.Text = ClienteRegistradoFinal.vRazonSocialCliente
                        hfdDireccionFiscalCliente.Value = ClienteRegistradoFinal.vDireccionCliente
                        hfdTelefonoContactoCliente.Value = ClienteRegistradoFinal.vTelefonoCliente
                        hfdIdTipoCliente.Value = ClienteRegistradoFinal.cIdTipoCliente
                        hfdIdTipoPersonaCliente.Value = ClienteRegistradoFinal.cIdTipoPersona
                        Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                        Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                        UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(ClienteRegistradoFinal.cIdPaisUbicacionGeografica, ClienteRegistradoFinal.cIdDepartamentoUbicacionGeografica, ClienteRegistradoFinal.cIdProvinciaUbicacionGeografica, ClienteRegistradoFinal.cIdDistritoUbicacionGeografica)
                        hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                        hfdIdUbicacionGeograficaClienteUbicacion.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                        'cboClienteUbicacion.SelectedValue = "00"
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
                        'cboClienteUbicacion.SelectedIndex = -1
                        hfdIdTipoCliente.Value = ""
                        hfdIdTipoPersonaCliente.Value = ""
                        hfdIdUbicacionGeograficaCliente.Value = ""
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = "" Then
            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            Exit Sub
        End If

        If Me.IsPostBack = False Then 'Si la pagina no a hecho Postback; es el ida y vuelta de la pagina web al servidor web
            strOpcionModulo = "071" 'Mantenimiento para Generar Contratos en el CMMS.

            CargarPerfil()

            If hfdOperacion.Value = "" Then
                hfdOperacion.Value = "R"
            End If

            ListarFiltroBusquedaCombo()
            cboFiltroContrato.SelectedIndex = 4

            ListarPaisCombo()
            ListarDepartamentoCombo()
            ListarProvinciaCombo()
            ListarDistritoCombo()
            ListarPeriodoCombo()

            If Session("CestaEquipoContrato") Is Nothing Then
                Session("CestaEquipoContrato") = CrearCestaEquipo(31)
            Else
                VaciarCestaEquipo(Session("CestaEquipoContrato"))
            End If

            If Session("CestaEquipoContratoImprimir") Is Nothing Then
                Session("CestaEquipoContratoImprimir") = CrearCestaEquipo(31)
            Else
                VaciarCestaEquipo(Session("CestaEquipoContratoImprimir"))
            End If

            If Session("CestaSSPersonal") Is Nothing Then
                Session("CestaSSPersonal") = CrearCestaPersonal()
            Else
                VaciarCestaPersonal(Session("CestaSSPersonal"))
            End If

            If Session("CestaPersonalOrdenTrabajo") Is Nothing Then
                Session("CestaPersonalOrdenTrabajo") = CrearCestaPersonalOrdenTrabajo()
            Else
                VaciarCestaPersonalOrdenTrabajo(Session("CestaPersonalOrdenTrabajo"))
            End If

            BloquearMantenimiento(True, False, True, False)

            Dim SubFiltro As String = ""
            'SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
            '            "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
            '            "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
            '            "INNER JOIN GNRL_USUARIO AS USU ON " &
            '            "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
            '            "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

            'Me.grdLista.DataSource = ContratoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
            Me.grdLista.DataBind()

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
        Else
            'txtBuscarContrato.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanListado_txtBuscar')")
            'txtIdEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_cboFamilia')")
            'cboTipoActivo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcion')")
            'txtDescripcionEquipo.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'txtDescripcionAbreviada.Attributes.Add("onkeypress", "CambiaFoco('cuerpo_TabContMaestroActivo_TabPanGeneral_txtDescripcioAbreviada')")
            'If lblOperacion.Value = "E" Or lblOperacion.Value = "N" Then
            '    BloquearPagina(2)
            'End If
        End If
    End Sub

    'Sub CargarStatusActividad()
    '    'For Each row As GridViewRow In lstCheckList.Rows
    '    '    'For Each row As DataListItem In lstCheckList.Items
    '    '    '    'If row.RowType = DataControlRowType.DataRow Then
    '    '    '    '    'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalle"), CheckBox)
    '    '    '    '    Dim lblNroDocumento As Label = TryCast(row.Cells(0).FindControl("lblNroDoc"), Label)
    '    '    '    'End If
    '    '    '    If row.ItemType = DataControlRowType.DataRow Or (row.ItemType = ListItemType.AlternatingItem) Then
    '    '    '        Dim lnkbtnIniciarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    '    '    '        Dim lnkbtnPendienteCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    '    '    '        Dim lnkbtnRetomarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    '    '    '        Dim lnkbtnFinalizarCheckList As LinkButton = TryCast(row.Find


    '    For Each elemento As DataListItem In lstCheckList.Items
    '        'For Each elemento As ListViewItem In lstCheckList.Items
    '        Dim hfdStatusCheckList As HiddenField = TryCast(elemento.FindControl("hfdStatusCheckList"), HiddenField)
    '        Dim lnkbtnIniciarCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    '        Dim lnkbtnPendienteCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnPendienteCheckList"), LinkButton)
    '        Dim lnkbtnRetomarCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnRetomarCheckList"), LinkButton)
    '        Dim lnkbtnFinalizarCheckList As LinkButton = TryCast(elemento.FindControl("lnkbtnFinalizarCheckList"), LinkButton)
    '        If hfdStatusCheckList.Value = "I" Then 'Iniciar
    '            lnkbtnIniciarCheckList.Visible = True
    '        Else
    '            lnkbtnIniciarCheckList.Visible = False
    '        End If
    '        If hfdStatusCheckList.Value = "E" Then 'En Proceso
    '            lnkbtnPendienteCheckList.Visible = True
    '            lnkbtnFinalizarCheckList.Visible = True
    '        Else
    '            lnkbtnPendienteCheckList.Visible = False
    '        End If
    '        If hfdStatusCheckList.Value = "P" Then 'Pendiente
    '            lnkbtnRetomarCheckList.Visible = True
    '            lnkbtnFinalizarCheckList.Visible = True
    '        Else
    '            lnkbtnRetomarCheckList.Visible = False
    '        End If
    '        If hfdStatusCheckList.Value = "F" Then 'Finalizar
    '            lnkbtnFinalizarCheckList.Visible = False
    '        End If
    '    Next
    'End Sub

    'Function fLlenarGrillaComponentes() As DataTable
    '    'Dim dsComponentes = ContratoNeg.OrdenTrabajoListaBusquedaComponentes("EQU.cIdEnlaceEquipo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim), (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
    '    '                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
    '    '                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
    '    '                                                         Session("IdEmpresa"))
    '    Dim ComponentesEquipoNeg As New clsEquipoNegocios
    '    '        Dim Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
    '    'Dim dsComponentes = ComponentesEquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim) & "' AND EQU.cIdEnlaceCatalogo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim), "1")
    '    Dim dsComponentes = ComponentesEquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim), "1")
    '    Dim dtComponentes As New DataTable
    '    dtComponentes = FuncionesNeg.ConvertirListADataTable(dsComponentes)
    '    dtComponentes.Columns.Add(New DataColumn("TotalCantidadComponentes", GetType(System.Int32)))
    '    dtComponentes.Columns.Add(New DataColumn("CantidadComponentesFinalizados", GetType(System.Int32)))
    '    dtComponentes.Columns.Add(New DataColumn("PorcentajeText", GetType(System.String)))
    '    dtComponentes.Columns.Add(New DataColumn("Porcentaje", GetType(System.Int32)))
    '    dtComponentes.Columns.Add(New DataColumn("Tiempo", GetType(System.Int32)))
    '    'dtComponentes.Columns.Add(New DataColumn("TiempoText", GetType(System.String)))
    '    For Each Componente In dtComponentes.Rows
    '        'Dim NombreArchivo = Empresa.vRucEmpresa & "-" & DocEmi("IdTipoDocumentoEquivalente") & "-" & DocEmi("NumeroSerieDocumento") & "-" & DocEmi("NumeroCorrelativo") & ".xml"
    '        'Componente("TotalCantidadComponentes") = ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') <> 'F'").Rows(0).Item(0)
    '        Componente("TotalCantidadComponentes") = ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "'").Rows(0).Item(0)
    '        Componente("CantidadComponentesFinalizados") = ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
    '        Componente("PorcentajeText") = IIf(Componente("TotalCantidadComponentes") = 0, "Sin actividad", Math.Round((100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes")), 2) & "%")
    '        Componente("Porcentaje") = IIf(Componente("TotalCantidadComponentes") = 0, 0, Math.Round(100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes"), 2))
    '        Componente("Tiempo") = ContratoNeg.ContratoGetData("SELECT ISNULL(SUM(nTotalSegundosTrabajadosCheckListOrdenTrabajo), 0) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
    '    Next
    '    Return dtComponentes
    'End Function

    'Function fLlenarGrillaPlanificacion(ByVal IdAno As String, ByVal IdMes As String)
    'Dim rowIndexDetalle As Int16
    'Dim resultProgramacionContratoSimple As DataRow() = Session("CestaEquipoContrato").Select("IdEquipo = '" & hfdIdEquipo.Value & "'")
    'rowIndexDetalle = Session("CestaEquipoContrato").Rows.IndexOf(resultProgramacionContratoSimple(0))
    'If resultProgramacionContratoSimple.Length = 0 Then
    '    VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
    'Else
    '    Dim rowFil As DataRow() = resultInsumosSimple
    '    For Each Insumos As DataRow In rowFil
    '        AgregarCestaInsumos(Insumos("Codigo"), Insumos("Descripcion"), Insumos("Cantidad"), Insumos("DescripcionUnidadMedida"),
    '                    Insumos("IdEquipo"), Insumos("IdCatalogo"), Insumos("IdJerarquia"), Insumos("IdActividad"), Insumos("IdArticuloSAP"), Session("CestaInsumosFiltrado"))
    '    Next
    'End If
    'End Function

    'Function fLlenarGrillaPlanificacion() As DataTable
    '    'Dim dsComponentes = ContratoNeg.OrdenTrabajoListaBusquedaComponentes("EQU.cIdEnlaceEquipo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim), (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
    '    '                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
    '    '                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
    '    '                                                         Session("IdEmpresa"))
    '    Dim ComponentesEquipoNeg As New clsEquipoNegocios
    '    '        Dim Coleccion2 = EquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & txtIdEquipo.Text.Trim & "' AND EQU.cIdEnlaceCatalogo", cboCatalogo.SelectedValue, "1")
    '    'Dim dsComponentes = ComponentesEquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim) & "' AND EQU.cIdEnlaceCatalogo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim), "1")
    '    Dim dsComponentes = ComponentesEquipoNeg.EquipoListaBusqueda("EQU.cIdEnlaceEquipo", (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim), "1")
    '    Dim dtComponentes As New DataTable
    '    dtComponentes = FuncionesNeg.ConvertirListADataTable(dsComponentes)
    '    dtComponentes.Columns.Add(New DataColumn("TotalCantidadComponentes", GetType(System.Int32)))
    '    dtComponentes.Columns.Add(New DataColumn("CantidadComponentesFinalizados", GetType(System.Int32)))
    '    dtComponentes.Columns.Add(New DataColumn("PorcentajeText", GetType(System.String)))
    '    dtComponentes.Columns.Add(New DataColumn("Porcentaje", GetType(System.Int32)))
    '    dtComponentes.Columns.Add(New DataColumn("Tiempo", GetType(System.Int32)))
    '    'dtComponentes.Columns.Add(New DataColumn("TiempoText", GetType(System.String)))
    '    For Each Componente In dtComponentes.Rows
    '        'Dim NombreArchivo = Empresa.vRucEmpresa & "-" & DocEmi("IdTipoDocumentoEquivalente") & "-" & DocEmi("NumeroSerieDocumento") & "-" & DocEmi("NumeroCorrelativo") & ".xml"
    '        'Componente("TotalCantidadComponentes") = ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') <> 'F'").Rows(0).Item(0)
    '        Componente("TotalCantidadComponentes") = ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "'").Rows(0).Item(0)
    '        Componente("CantidadComponentesFinalizados") = ContratoNeg.ContratoGetData("SELECT COUNT(*) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
    '        Componente("PorcentajeText") = IIf(Componente("TotalCantidadComponentes") = 0, "Sin actividad", Math.Round((100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes")), 2) & "%")
    '        Componente("Porcentaje") = IIf(Componente("TotalCantidadComponentes") = 0, 0, Math.Round(100 * Componente("CantidadComponentesFinalizados") / Componente("TotalCantidadComponentes"), 2))
    '        Componente("Tiempo") = ContratoNeg.ContratoGetData("SELECT ISNULL(SUM(nTotalSegundosTrabajadosCheckListOrdenTrabajo), 0) FROM LOGI_CHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Componente("IdCatalogo") & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Componente("IdJerarquiaCatalogo") & "' AND ISNULL(cEstadoCheckListOrdenTrabajo, '') = 'F'").Rows(0).Item(0)
    '    Next
    '    Return dtComponentes
    'End Function

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0663", strOpcionModulo, Session("IdSistema"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0663", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = False
            pnlContenido.Visible = True
            'If grdLista IsNot Nothing Then
            '    If grdLista.Rows.Count > 0 Then
            '        If IsNothing(grdLista.SelectedRow) = False Then
            '            If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
            '                'Dim EquipoNeg As New clsEquipoNegocios
            '                'If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "'").Rows(0).Item(0) = "0" Then
            '                '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
            '                'End If
            '            End If
            '        Else
            '            Throw New Exception("Debe de seleccionar un item.")
            '        End If
            '    End If
            'End If
            hfdOperacion.Value = "N"
            'txtDescripcionEquipo.Focus()'BORRAR

            BloquearMantenimiento(False, True, False, True)
            LimpiarObjetos()
            ValidarTexto(True) 'BORRAR
            ActivarObjetos(True) 'BORRAR
            'LlenarDataEquipo() 'BORRAR
            'Dim Contrato = ContratoNeg.ContratoListarPorId((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
            '                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
            '                                                     (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
            '                                                     Session("IdEmpresa"))
            'ListarPersonalAsignadoCombo(Contrato)
            'hfdOrdenFabricacionReferencia.Value = Contrato.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo 'JMUG: 22/07/2023
            'LlenarDataEquipo()

            'lstComponentes.DataSource = fLlenarGrillaComponentes()
            'lstComponentes.DataBind()

            'Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
            'Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
            '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
            '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
            '                                                                         Session("IdEmpresa"),
            '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text).Trim),
            '                                                                         (Server.HtmlDecode(grdLista.SelectedRow.Cells(12).Text).Trim))
            'Me.lstCheckList.DataBind()
            'CargarStatusActividad()
            'pnlCabecera.Visible = False
            'pnlContenido.Visible = True
            'ContratoNeg.ContratoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET cEstadoCabeceraOrdenTrabajo = 'P' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
            'ValidarTexto(True)
            'ActivarObjetos(True)
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

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), "142", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0665", strOpcionModulo, "CMMS")

            Dim Contrato As New LOGI_CABECERACONTRATO
            Dim strContrato = txtIdContrato.Text.ToString.Split("-")
            If hfdOperacion.Value = "N" Then
                Contrato.cIdTipoDocumentoCabeceraContrato = "CN"
                Contrato.vIdNumeroSerieCabeceraContrato = Year(Now).ToString
                Contrato.vIdNumeroCorrelativoCabeceraContrato = ""
            ElseIf hfdOperacion.Value = "E" Then
                Contrato.cIdTipoDocumentoCabeceraContrato = strContrato(0)
                Contrato.vIdNumeroSerieCabeceraContrato = strContrato(1)
                Contrato.vIdNumeroCorrelativoCabeceraContrato = strContrato(2)
            End If
            Contrato.cIdEmpresa = Session("IdEmpresa")
            Contrato.dFechaTransaccionCabeceraContrato = Now
            Contrato.dFechaEmisionCabeceraContrato = txtFechaEmisionContrato.Text
            Contrato.dFechaVigenciaInicialCabeceraContrato = txtFechaVigenciaInicio.Text
            Contrato.dFechaVigenciaFinalCabeceraContrato = txtFechaVigenciaFinal.Text
            Contrato.cIdCliente = hfdIdAuxiliar.Value
            Contrato.bEstadoRegistroCabeceraContrato = hfdEstado.Value
            Contrato.cEstadoCabeceraContrato = "R" 'Registrado
            Contrato.vDescripcionCabeceraContrato = UCase(txtDescripcionContrato.Text.Trim)
            Contrato.vNroLicitacionCabeceraContrato = UCase(txtNroLicitacionContrato.Text.Trim)
            Contrato.dFechaUltimaModificacionCabeceraContrato = Now
            Contrato.dFechaCreacionCabeceraContrato = Convert.ToDateTime(IIf(hfdFechaCreacionContrato.Value = "", Now, hfdFechaCreacionContrato.Value))
            Contrato.cIdUsuarioUltimaModificacionCabeceraContrato = Session("IdUsuario")
            Contrato.cIdUsuarioCreacionCabeceraContrato = hfdIdUsuarioCreacionContrato.Value

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
                If (ContratoNeg.ContratoInserta(Contrato)) = 0 Then
                    Dim ColeccionDetalleContrato As New List(Of LOGI_DETALLECONTRATO)
                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        Dim DetContrato As New LOGI_DETALLECONTRATO
                        DetContrato.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                        DetContrato.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                        DetContrato.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                        DetContrato.cIdEmpresa = Session("IdEmpresa")
                        DetContrato.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                        DetContrato.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")
                        DetContrato.vDescripcionDetalleContrato = Session("CestaEquipoContrato").rows(i)("DescripcionEquipo")
                        DetContrato.bEstadoRegistroDetalleContrato = Session("CestaEquipoContrato").rows(i)("Estado")
                        ColeccionDetalleContrato.Add(DetContrato)
                    Next

                    Dim ColeccionPlanificacionContrato As New List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO)
                    Dim ColeccionOrdTra As New List(Of LOGI_CABECERAORDENTRABAJO)
                    'Dim ColeccionRRHH As New List(Of LOGI_RECURSOSORDENTRABAJO)
                    'Dim ColeccionCheckList As New List(Of LOGI_CHECKLISTORDENTRABAJO)

                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        For x = 1 To 31
                            If (Session("CestaEquipoContrato").rows(i)("D" & x)).ToString.Trim <> "" Then
                                If IsDate(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)) Then
                                    If Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) <> "" Then
                                        Dim DetPlanificacion As New LOGI_PLANIFICACIONEQUIPOCONTRATO
                                        DetPlanificacion.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                                        DetPlanificacion.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                                        DetPlanificacion.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        DetPlanificacion.cIdEmpresa = Contrato.cIdEmpresa
                                        DetPlanificacion.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                                        DetPlanificacion.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")

                                        DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyMM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
                                        DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato = Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3))
                                        DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) 'Session("CestaEquipoContrato").rows(i)("IdTipoMantenimiento")
                                        DetPlanificacion.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(4) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajo")
                                        DetPlanificacion.vOrdenTrabajoClientePlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(5) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        DetPlanificacion.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(2) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        ColeccionPlanificacionContrato.Add(DetPlanificacion)

                                        Dim OrdTra As New LOGI_CABECERAORDENTRABAJO
                                        OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = "OT"
                                        OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = Year(Now).ToString
                                        OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = ""
                                        OrdTra.dFechaTransaccionCabeceraOrdenTrabajo = Now
                                        OrdTra.dFechaEmisionCabeceraOrdenTrabajo = Now
                                        OrdTra.cIdClienteCabeceraOrdenTrabajo = hfdIdAuxiliar.Value 'Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text).Trim
                                        OrdTra.cIdEquipoCabeceraOrdenTrabajo = DetPlanificacion.cIdEquipoDetalleContrato 'Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim
                                        OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato 'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                                        OrdTra.cIdEmpresa = Session("IdEmpresa")
                                        OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
                                        OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato 'cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
                                        OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(11).Text
                                        OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
                                        OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
                                        OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
                                        OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
                                        OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
                                        OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        ColeccionOrdTra.Add(OrdTra)
                                    End If
                                End If
                            End If
                        Next
                    Next

                    If ContratoNeg.ContratoInsertaDetalle(Contrato, ColeccionDetalleContrato, ColeccionPlanificacionContrato, ColeccionOrdTra, Session("CestaPersonalOrdenTrabajo")) = 0 Then
                        'If EquipoNeg.EquipoInsertaDetalle(Coleccion, Equipo.cIdEquipo, Equipo.cIdCatalogo) = 0 Then
                        '    Session("Query") = "PA_LOGI_MNT_EQUIPO 'SQL_INSERT', '','" & Equipo.cIdEquipo & "', '" & Equipo.cIdCatalogo & "', '" &
                        '                   Equipo.cIdTipoActivo & "', '" & Equipo.vDescripcionEquipo & "', '" &
                        '                   Equipo.vDescripcionAbreviadaEquipo & "', '" & Equipo.cIdJerarquiaCatalogo & "', '" &
                        '                   Equipo.dFechaTransaccionEquipo & "', '" & Equipo.cIdEnlaceEquipo & "', '" &
                        '                   Equipo.cIdEnlaceCatalogo & "', '" & Equipo.cIdSistemaFuncionalEquipo & "', '" & Equipo.bEstadoRegistroEquipo & "', '" &
                        '                   Equipo.vObservacionEquipo & "', " & Equipo.nVidaUtilEquipo & ", '" &
                        '                   Equipo.cIdEmpresa & "', '" & Equipo.cIdEstadoComponenteEquipo & "', '" &
                        '                   Equipo.vIdEquivalenciaEquipo & "', '" & Equipo.cIdPaisOrigenEquipo & "', " &
                        '                   Equipo.vIdClienteSAPEquipo & ", '" & Equipo.cIdEquipoSAPEquipo & "', '" &
                        '                   Equipo.dFechaRegistroTarjetaSAPEquipo & "', '" & Equipo.dFechaManufacturaTarjetaSAPEquipo & "', '" &
                        '                   Equipo.nPeriodoGarantiaEquipo & "', '" & Equipo.nPeriodoMinimoMantenimientoEquipo & "', '" &
                        '                   Equipo.vNumeroSerieEquipo & "', '" & Equipo.vNumeroParteEquipo & "', '" & Equipo.cIdCliente & "', '" &
                        '                   Equipo.cIdEquipo & "'"

                        '    LogAuditoria.vEvento = "INSERTAR EQUIPO"
                        '    LogAuditoria.vQuery = Session("Query")
                        '    LogAuditoria.cIdSistema = Session("IdSistema")
                        '    LogAuditoria.cIdModulo = strOpcionModulo 'Session("IdModulo")

                        '    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        '    txtIdEquipo.Text = Equipo.cIdEquipo
                        txtIdContrato.Text = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                        lblDescripcionMensajeDocumento.Text = "Se creó el siguiente número de documento"
                        lblNroDocumentoMensajeDocumento.Visible = True
                        btnSiMensajeDocumento.Visible = False
                        btnNoMensajeDocumento.Visible = False
                        lblNroDocumentoMensajeDocumento.Text = txtIdContrato.Text
                        lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        '    'CargarCestaComponenteCatalogo()
                        '    'CargarCestaComponenteMaestroActivo()
                        '    CargarCestaCatalogoComponente()
                        '    CargarCestaEquipoComponente()

                        '    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, 0)
                        '    Me.grdLista.DataBind()
                        Dim SubFiltro As String = ""
                        'SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        '            "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        '            "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        '            "INNER JOIN GNRL_USUARIO AS USU ON " &
                        '            "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        '            "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

                        'Me.grdLista.DataSource = ContratoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
                        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
                        Me.grdLista.DataBind() 'Recargo el grid.

                        pnlCabecera.Visible = True
                        pnlContenido.Visible = False
                        '    pnlEquipo.Visible = False
                        '    pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarContrato.Focus()
                        'Else
                        '    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                        'End If
                    End If
                End If
            ElseIf hfdOperacion.Value = "E" Then
                'If (EquipoNeg.EquipoEdita(Equipo)) = 0 Then
                If (ContratoNeg.ContratoEdita(Contrato)) = 0 Then
                    Dim ColeccionDetalleContrato As New List(Of LOGI_DETALLECONTRATO)
                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        Dim DetContrato As New LOGI_DETALLECONTRATO
                        DetContrato.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                        DetContrato.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                        DetContrato.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                        DetContrato.cIdEmpresa = Session("IdEmpresa")
                        DetContrato.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                        DetContrato.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")
                        DetContrato.vDescripcionDetalleContrato = Session("CestaEquipoContrato").rows(i)("DescripcionEquipo")
                        DetContrato.bEstadoRegistroDetalleContrato = Session("CestaEquipoContrato").rows(i)("Estado")
                        ColeccionDetalleContrato.Add(DetContrato)
                    Next

                    Dim ColeccionPlanificacionContrato As New List(Of LOGI_PLANIFICACIONEQUIPOCONTRATO)
                    Dim ColeccionOrdTra As New List(Of LOGI_CABECERAORDENTRABAJO)
                    'Dim ColeccionRRHH As New List(Of LOGI_RECURSOSORDENTRABAJO)
                    'Dim ColeccionCheckList As New List(Of LOGI_CHECKLISTORDENTRABAJO)

                    For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                        For x = 1 To 31
                            If (Session("CestaEquipoContrato").rows(i)("D" & x)).ToString.Trim <> "" Then
                                If IsDate(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)) Then
                                    If Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) <> "" Then
                                        Dim DetPlanificacion As New LOGI_PLANIFICACIONEQUIPOCONTRATO
                                        DetPlanificacion.cIdTipoDocumentoCabeceraContrato = Contrato.cIdTipoDocumentoCabeceraContrato
                                        DetPlanificacion.vIdNumeroSerieCabeceraContrato = Contrato.vIdNumeroSerieCabeceraContrato
                                        DetPlanificacion.vIdNumeroCorrelativoCabeceraContrato = Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        DetPlanificacion.cIdEmpresa = Contrato.cIdEmpresa
                                        DetPlanificacion.nIdNumeroItemDetalleContrato = Session("CestaEquipoContrato").rows(i)("Item")
                                        DetPlanificacion.cIdEquipoDetalleContrato = Session("CestaEquipoContrato").rows(i)("IdEquipo")

                                        DetPlanificacion.cIdPeriodoMesPlanificacionEquipoContrato = String.Format("{0:yyyyMM}", Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3)))
                                        DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato = Convert.ToDateTime(Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(3))
                                        DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(0) 'Session("CestaEquipoContrato").rows(i)("IdTipoMantenimiento")
                                        DetPlanificacion.vOrdenTrabajoReferenciaPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(4) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajo")
                                        DetPlanificacion.vOrdenTrabajoClientePlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(5) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        DetPlanificacion.cIdNumeroPlantillaCheckListPlanificacionEquipoContrato = Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(2) 'Session("CestaEquipoContrato").rows(i)("IdOrdenTrabajoCliente")
                                        ColeccionPlanificacionContrato.Add(DetPlanificacion)

                                        Dim OrdTra As New LOGI_CABECERAORDENTRABAJO
                                        Dim strOrdTra = Split((Split(Session("CestaEquipoContrato").rows(i)("D" & x), "|")(4)), "-")
                                        OrdTra.cIdTipoDocumentoCabeceraOrdenTrabajo = If(strOrdTra(0) = "", "OT", strOrdTra(0))
                                        OrdTra.vIdNumeroSerieCabeceraOrdenTrabajo = If(strOrdTra(0) = "", Year(Now).ToString, strOrdTra(1))
                                        OrdTra.vIdNumeroCorrelativoCabeceraOrdenTrabajo = If(strOrdTra(0) = "", "", strOrdTra(2))
                                        OrdTra.dFechaTransaccionCabeceraOrdenTrabajo = Now
                                        OrdTra.dFechaEmisionCabeceraOrdenTrabajo = Now
                                        OrdTra.cIdClienteCabeceraOrdenTrabajo = hfdIdAuxiliar.Value 'Server.HtmlDecode(grdLista.SelectedRow.Cells(3).Text).Trim
                                        OrdTra.cIdEquipoCabeceraOrdenTrabajo = DetPlanificacion.cIdEquipoDetalleContrato 'Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim
                                        OrdTra.dFechaInicioPlanificadaCabeceraOrdenTrabajo = DetPlanificacion.dFechaHoraProgramacionPlanificacionEquipoContrato 'txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text
                                        OrdTra.cIdEmpresa = Session("IdEmpresa")
                                        OrdTra.vOrdenFabricacionReferenciaCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(0).Text & "-" & grdLista.SelectedRow.Cells(1).Text & "-" & grdLista.SelectedRow.Cells(2).Text
                                        OrdTra.cIdTipoMantenimientoCabeceraOrdenTrabajo = DetPlanificacion.cIdTipoMantenimientoPlanificacionEquipoContrato 'cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue
                                        OrdTra.vIdArticuloSAPCabeceraOrdenTrabajo = "" 'grdLista.SelectedRow.Cells(11).Text
                                        OrdTra.dFechaEjecucionInicialCabeceraOrdenTrabajo = Nothing
                                        OrdTra.dFechaEjecucionFinalCabeceraOrdenTrabajo = Nothing
                                        OrdTra.cIdEquipoSAPCabeceraOrdenTrabajo = hfdIdEquipoSAPEquipo.Value
                                        OrdTra.cEstadoCabeceraOrdenTrabajo = "R"
                                        OrdTra.bEstadoRegistroCabeceraOrdenTrabajo = True
                                        OrdTra.vContratoReferenciaCabeceraOrdenTrabajo = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                                        ColeccionOrdTra.Add(OrdTra)
                                    End If
                                End If
                            End If
                        Next
                    Next

                    If ContratoNeg.ContratoInsertaDetalle(Contrato, ColeccionDetalleContrato, ColeccionPlanificacionContrato, ColeccionOrdTra, Session("CestaPersonalOrdenTrabajo")) = 0 Then
                        'If EquipoNeg.EquipoInsertaDetalle(Coleccion, Equipo.cIdEquipo, Equipo.cIdCatalogo) = 0 Then
                        '    Session("Query") = "PA_LOGI_MNT_EQUIPO 'SQL_INSERT', '','" & Equipo.cIdEquipo & "', '" & Equipo.cIdCatalogo & "', '" &
                        '                   Equipo.cIdTipoActivo & "', '" & Equipo.vDescripcionEquipo & "', '" &
                        '                   Equipo.vDescripcionAbreviadaEquipo & "', '" & Equipo.cIdJerarquiaCatalogo & "', '" &
                        '                   Equipo.dFechaTransaccionEquipo & "', '" & Equipo.cIdEnlaceEquipo & "', '" &
                        '                   Equipo.cIdEnlaceCatalogo & "', '" & Equipo.cIdSistemaFuncionalEquipo & "', '" & Equipo.bEstadoRegistroEquipo & "', '" &
                        '                   Equipo.vObservacionEquipo & "', " & Equipo.nVidaUtilEquipo & ", '" &
                        '                   Equipo.cIdEmpresa & "', '" & Equipo.cIdEstadoComponenteEquipo & "', '" &
                        '                   Equipo.vIdEquivalenciaEquipo & "', '" & Equipo.cIdPaisOrigenEquipo & "', " &
                        '                   Equipo.vIdClienteSAPEquipo & ", '" & Equipo.cIdEquipoSAPEquipo & "', '" &
                        '                   Equipo.dFechaRegistroTarjetaSAPEquipo & "', '" & Equipo.dFechaManufacturaTarjetaSAPEquipo & "', '" &
                        '                   Equipo.nPeriodoGarantiaEquipo & "', '" & Equipo.nPeriodoMinimoMantenimientoEquipo & "', '" &
                        '                   Equipo.vNumeroSerieEquipo & "', '" & Equipo.vNumeroParteEquipo & "', '" & Equipo.cIdCliente & "', '" &
                        '                   Equipo.cIdEquipo & "'"

                        '    LogAuditoria.vEvento = "INSERTAR EQUIPO"
                        '    LogAuditoria.vQuery = Session("Query")
                        '    LogAuditoria.cIdSistema = Session("IdSistema")
                        '    LogAuditoria.cIdModulo = strOpcionModulo 'Session("IdModulo")

                        '    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                        '    txtIdEquipo.Text = Equipo.cIdEquipo
                        txtIdContrato.Text = Contrato.cIdTipoDocumentoCabeceraContrato + "-" + Contrato.vIdNumeroSerieCabeceraContrato + "-" + Contrato.vIdNumeroCorrelativoCabeceraContrato
                        lblDescripcionMensajeDocumento.Text = "Se modificó el siguiente número de documento"
                        lblNroDocumentoMensajeDocumento.Visible = True
                        btnSiMensajeDocumento.Visible = False
                        btnNoMensajeDocumento.Visible = False
                        lblNroDocumentoMensajeDocumento.Text = txtIdContrato.Text
                        lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                        MyValidator.ErrorMessage = "Transacción registrada con éxito"
                        '    'CargarCestaComponenteCatalogo()
                        '    'CargarCestaComponenteMaestroActivo()
                        '    CargarCestaCatalogoComponente()
                        '    CargarCestaEquipoComponente()

                        '    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, 0)
                        '    Me.grdLista.DataBind()
                        Dim SubFiltro As String = ""
                        'SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
                        '            "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
                        '            "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
                        '            "INNER JOIN GNRL_USUARIO AS USU ON " &
                        '            "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
                        '            "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

                        'Me.grdLista.DataSource = ContratoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
                        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
                        Me.grdLista.DataBind() 'Recargo el grid.

                        pnlCabecera.Visible = True
                        pnlContenido.Visible = False
                        '    pnlEquipo.Visible = False
                        '    pnlComponentes.Visible = False

                        BloquearMantenimiento(True, False, True, False)
                        hfdOperacion.Value = "R"
                        txtBuscarContrato.Focus()
                        'Else
                        '    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                        'End If
                    End If

                    'Dim Coleccion As New List(Of LOGI_EQUIPO)
                    'For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                    '    Dim DetEquipo As New LOGI_EQUIPO

                    '    DetEquipo.cIdEquipo = Session("CestaEquipoComponente").Rows(i)("IdEquipo").ToString.Trim
                    '    DetEquipo.cIdEmpresa = Session("IdEmpresa")
                    '    DetEquipo.cIdTipoActivo = Session("CestaEquipoComponente").Rows(i)("IdTipoActivo").ToString.Trim
                    '    DetEquipo.vDescripcionEquipo = Session("CestaEquipoComponente").Rows(i)("Descripcion").ToString.Trim
                    '    DetEquipo.vDescripcionAbreviadaEquipo = Session("CestaEquipoComponente").Rows(i)("DescripcionAbreviada").ToString.Trim
                    '    DetEquipo.dFechaTransaccionEquipo = Session("CestaEquipoComponente").Rows(i)("FechaTransaccion").ToString.Trim
                    '    DetEquipo.cIdEnlaceEquipo = Equipo.cIdEquipo 'Session("CestaMaestroActivoComponente").Rows(i)("IdEnlace").ToString.Trim
                    '    DetEquipo.cIdEnlaceCatalogo = cboCatalogo.SelectedValue 'MaestroActivo.cIdEnlaceCatalogo
                    '    DetEquipo.bEstadoRegistroEquipo = "1"
                    '    DetEquipo.cIdSistemaFuncionalEquipo = Session("CestaEquipoComponente").Rows(i)("IdSistemaFuncional").ToString.Trim
                    '    DetEquipo.cIdFamilia = Nothing
                    '    DetEquipo.cIdSubFamilia = Nothing
                    '    DetEquipo.vObservacionEquipo = Session("CestaEquipoComponente").Rows(i)("Observacion").ToString.Trim
                    '    DetEquipo.cIdEstadoComponenteEquipo = "01" 'Session("CestaEquipoComponente").Rows(i)("IdEstadoActivo").ToString.Trim
                    '    DetEquipo.vIdEquivalenciaEquipo = "" 'Session("CestaEquipoComponente").Rows(i)("IdEquivalencia").ToString.Trim
                    '    DetEquipo.cIdPaisOrigenEquipo = Session("IdPais")
                    '    DetEquipo.vIdClienteSAPEquipo = hfdIdClienteSAPEquipo.Value
                    '    DetEquipo.cIdEquipoSAPEquipo = hfdIdEquipoSAPEquipo.Value
                    '    DetEquipo.dFechaRegistroTarjetaSAPEquipo = hfdFechaRegistroTarjetaSAPEquipo.Value 'Now 'hfdFechaRegistroTarjetaSAPEquipo.value
                    '    DetEquipo.dFechaManufacturaTarjetaSAPEquipo = hfdFechaRegistroTarjetaSAPEquipo.Value 'Now
                    '    DetEquipo.cIdCatalogo = Session("CestaEquipoComponente").Rows(i)("IdCatalogo").ToString.Trim
                    '    DetEquipo.cIdJerarquiaCatalogo = "1"
                    '    DetEquipo.nVidaUtilEquipo = Session("CestaEquipoComponente").Rows(i)("VidaUtil").ToString.Trim
                    '    DetEquipo.nPeriodoGarantiaEquipo = Session("CestaEquipoComponente").Rows(i)("PeriodoGarantia").ToString.Trim 'Nothing
                    '    DetEquipo.nPeriodoMinimoMantenimientoEquipo = Session("CestaEquipoComponente").Rows(i)("PeriodoMinimo").ToString.Trim ' Nothing
                    '    DetEquipo.vNumeroSerieEquipo = Session("CestaEquipoComponente").Rows(i)("NroSerie").ToString.Trim
                    '    DetEquipo.vNumeroParteEquipo = Session("CestaEquipoComponente").Rows(i)("NroParte").ToString.Trim
                    '    DetEquipo.cIdCliente = hfdIdAuxiliar.Value
                    '    DetEquipo.cIdClienteUbicacion = IIf(cboClienteUbicacion.SelectedValue = "SELECCIONE DATO", Nothing, cboClienteUbicacion.SelectedValue)
                    '    DetEquipo.dFechaCreacionEquipo = Convert.ToDateTime(IIf(hfdFechaCreacionContratoComponente.Value = "", Now, hfdFechaCreacionContratoComponente.Value))
                    '    DetEquipo.dFechaUltimaModificacionEquipo = Now
                    '    DetEquipo.cIdUsuarioCreacionEquipo = hfdIdUsuarioCreacionEquipoComponente.Value
                    '    DetEquipo.cIdUsuarioUltimaModificacionEquipo = Session("IdUsuario")

                    '    Coleccion.Add(DetEquipo)
                    'Next

                    'If EquipoNeg.EquipoInsertaDetalle(Coleccion, Equipo.cIdEquipo, cboCatalogo.SelectedValue) = 0 Then
                    '    Session("Query") = "PA_LOGI_MNT_EQUIPO 'SQL_INSERT', '','" & Equipo.cIdEquipo & "', '" & Equipo.cIdCatalogo & "', '" &
                    '                       Equipo.cIdTipoActivo & "', '" & Equipo.vDescripcionEquipo & "', '" &
                    '                       Equipo.vDescripcionAbreviadaEquipo & "', '" & Equipo.cIdJerarquiaCatalogo & "', '" &
                    '                       Equipo.dFechaTransaccionEquipo & "', '" & Equipo.cIdEnlaceEquipo & "', '" &
                    '                       Equipo.cIdEnlaceCatalogo & "', '" & Equipo.cIdSistemaFuncionalEquipo & "', '" & Equipo.bEstadoRegistroEquipo & "', '" &
                    '                       Equipo.vObservacionEquipo & "', " & Equipo.nVidaUtilEquipo & ", '" &
                    '                       Equipo.cIdEmpresa & "', '" & Equipo.cIdEstadoComponenteEquipo & "', '" &
                    '                       Equipo.vIdEquivalenciaEquipo & "', '" & Equipo.cIdPaisOrigenEquipo & "', " &
                    '                       Equipo.vIdClienteSAPEquipo & ", '" & Equipo.cIdEquipoSAPEquipo & "', '" &
                    '                       Equipo.dFechaRegistroTarjetaSAPEquipo & "', '" & Equipo.dFechaManufacturaTarjetaSAPEquipo & "', '" &
                    '                       Equipo.nPeriodoGarantiaEquipo & "', '" & Equipo.nPeriodoMinimoMantenimientoEquipo & "', '" &
                    '                       Equipo.vNumeroSerieEquipo & "', '" & Equipo.vNumeroParteEquipo & "', '" & Equipo.cIdCliente & "', '" &
                    '                       Equipo.cIdEquipo & "'"

                    '    LogAuditoria.vEvento = "ACTUALIZAR EQUIPO"
                    '    LogAuditoria.vQuery = Session("Query")
                    '    LogAuditoria.cIdSistema = Session("IdSistema")
                    '    LogAuditoria.cIdModulo = strOpcionModulo 'Session("IdModulo")

                    '    FuncionesNeg.LogAuditoriaInserta(LogAuditoria)

                    '    Dim ColeccionCaracteristica As New List(Of LOGI_EQUIPOCARACTERISTICA)
                    '    Dim item As Int16
                    '    Dim strIdEquipo As String = ""

                    '    'JMUG: INICIO: 26/03/2023
                    '    item = 0
                    '    'Dim resultCaracteristicaEquipoPrincipalSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & Equipo.cIdCatalogo & "' AND IdJerarquia = '" & IIf(lnkEsComponenteOn.Visible = True, "1", "0") & "'")
                    '    Dim resultCaracteristicaEquipoPrincipalSimple As DataRow() = Session("CestaCaracteristicaEquipoPrincipal").Select("IdCatalogo = '" & Equipo.cIdCatalogo & "'")
                    '    If resultCaracteristicaEquipoPrincipalSimple.Length > 0 Then
                    '        Dim rowFil As DataRow() = resultCaracteristicaEquipoPrincipalSimple
                    '        For Each filacaracteristica As DataRow In rowFil
                    '            Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                    '            item = item + 1
                    '            DetCaracteristica.cIdEquipo = Equipo.cIdEquipo
                    '            DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                    '            DetCaracteristica.cIdCaracteristica = filacaracteristica("IdCaracteristica") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                    '            DetCaracteristica.nIdNumeroItemEquipoCaracteristica = item 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                    '            DetCaracteristica.vValorReferencialEquipoCaracteristica = UCase(filacaracteristica("Valor")) 'UCase(Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim)
                    '            DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = filacaracteristica("IdReferenciaSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                    '            DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = filacaracteristica("DescripcionCampoSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                    '            DetCaracteristica.cIdCatalogoEquipoCaracteristica = filacaracteristica("IdCatalogo")
                    '            ColeccionCaracteristica.Add(DetCaracteristica)
                    '        Next
                    '    End If
                    '    'JMUG: FINAL: 26/03/2023

                    '    For Each fila In Coleccion
                    '        item = 0
                    '        Dim resultCaracteristicaSimple As DataRow() = Session("CestaCaracteristicaEquipoComponente").Select("IdCatalogo = '" & fila.cIdCatalogo & "' AND IdJerarquia = '1'")
                    '        If resultCaracteristicaSimple.Length > 0 Then
                    '            Dim rowFil As DataRow() = resultCaracteristicaSimple
                    '            For Each filacaracteristica As DataRow In rowFil
                    '                Dim DetCaracteristica As New LOGI_EQUIPOCARACTERISTICA
                    '                item = item + 1
                    '                'DetCaracteristica.cIdEquipo = filacaracteristica("IdEquipo") 'fila.cIdEquipo
                    '                DetCaracteristica.cIdEquipo = fila.cIdEquipo
                    '                DetCaracteristica.cIdEmpresa = Session("IdEmpresa")
                    '                DetCaracteristica.cIdCaracteristica = filacaracteristica("IdCaracteristica") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdCaracteristica").ToString.Trim
                    '                DetCaracteristica.nIdNumeroItemEquipoCaracteristica = item 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("Item").ToString.Trim
                    '                DetCaracteristica.vValorReferencialEquipoCaracteristica = UCase(filacaracteristica("Valor")) 'UCase(Session("CestaCaracteristicaEquipoComponente").Rows(i)("Valor").ToString.Trim)
                    '                DetCaracteristica.cIdReferenciaSAPEquipoCaracteristica = filacaracteristica("IdReferenciaSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("IdReferenciaSAP").ToString.Trim
                    '                DetCaracteristica.vDescripcionCampoSAPEquipoCaracteristica = filacaracteristica("DescripcionCampoSAP") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                    '                DetCaracteristica.cIdCatalogoEquipoCaracteristica = filacaracteristica("IdCatalogo") 'Session("CestaCaracteristicaEquipoComponente").Rows(i)("DescripcionCampoSAP").ToString.Trim
                    '                ColeccionCaracteristica.Add(DetCaracteristica)
                    '            Next
                    '            'Exit Sub
                    '        End If
                    '    Next

                    '    If CaracteristicaNeg.CaracteristicaEquipoInsertaDetalle(Equipo.cIdEquipo, ColeccionCaracteristica, LogAuditoria) Then

                    '    End If

                    '    MyValidator.ErrorMessage = "Registro actualizado con éxito"

                    '    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, 0)
                    '    Me.grdLista.DataBind()
                    '    'pnlGeneral.Enabled = False
                    '    'BloquearMantenimiento(True, False, True, False)
                    '    'lblOperacion.Value = "R"
                    '    pnlCabecera.Visible = True
                    '    pnlEquipo.Visible = False
                    '    pnlComponentes.Visible = False

                    '    BloquearMantenimiento(True, False, True, False)
                    '    hfdOperacion.Value = "R"
                    '    txtBuscarEquipo.Focus()
                    '   Else
                    '    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                    '    End If
                    '    hfdOperacion.Value = "R"
                Else
                    Throw New Exception("Contáctese con el área de sistemas para informar la incidencia.")
                End If
            End If

            'Me.grdCatalogoComponente.DataSource = Nothing
            'Me.grdCatalogoComponente.DataBind()
            'Me.grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            'Me.grdCatalogoComponente.DataBind()

            'Me.grdEquipoComponente.DataSource = Nothing
            'Me.grdEquipoComponente.DataBind()
            'Me.grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            'Me.grdEquipoComponente.DataBind()

            ''BloquearPagina(0)
            'ValidationSummary1.ValidationGroup = "vgrpValidar"
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            'ValidationSummary1.ValidationGroup = "vgrpValidar"
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub imgbtnBuscarCliente_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarCliente.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            Me.grdListaCliente.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltroCliente.SelectedValue,
                                                                            IIf(txtBuscarCliente.Text.Trim = "", "***", txtBuscarCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
            Me.grdListaCliente.DataBind()
            lnk_mostrarPanelCliente_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub


    Protected Sub txtBuscarCliente_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscarCliente.TextChanged
        ValidarTexto(False)
        Me.grdListaCliente.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltroCliente.SelectedValue,
                                                                        IIf(txtBuscarCliente.Text.Trim = "", "***", txtBuscarCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
        Me.grdListaCliente.DataBind()
        Me.lnk_mostrarPanelCliente_ModalPopupExtender.Show()
    End Sub

    Private Sub grdListaCliente_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdListaCliente.PageIndexChanging
        grdListaCliente.PageIndex = e.NewPageIndex
        Me.grdListaCliente.DataSource = ClienteNeg.ClienteListaBusqueda(cboFiltroCliente.SelectedValue,
                                                                        IIf(txtBuscarCliente.Text.Trim = "", "***", txtBuscarCliente.Text.Trim), Session("IdEmpresa"), "*", False, "1")
        grdListaCliente.DataBind()
        grdListaCliente.SelectedIndex = -1
        lnk_mostrarPanelCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub grdListaCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdListaCliente.SelectedIndexChanged
        lblMensajeCliente.Text = ""
        Me.lnk_mostrarPanelCliente_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnAceptarCliente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarCliente.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            'clsVtasCestaComprobante.VaciarCesta(Session("CestaComprobante"))
            'Me.grdDetalle.DataSource = Session("CestaComprobante")
            'Me.grdDetalle.DataBind()
            'TotalizarGrid()

            If IsNothing(grdListaCliente.SelectedRow) = False Then
                If IsReference(grdListaCliente.SelectedRow.Cells(1).Text) = True Then
                    Dim Cliente As New GNRL_CLIENTE
                    Cliente = ClienteNeg.ClienteListarPorId(grdListaCliente.SelectedRow.Cells(1).Text, Session("IdEmpresa"), Session("IdPuntoVenta"))
                    'If cboTipoDoc.SelectedValue = "BV" And (Trim(Cliente.vDniCliente) = "" And Trim(Cliente.vRucCliente) = "") Then
                    If (Trim(Cliente.vDniCliente) = "" And Trim(Cliente.vRucCliente) = "") Then
                        Throw New Exception("Debe de ingresar el número de documento identidad o ruc, favor de validar la información...!!!")
                        'ElseIf cboTipoDoc.SelectedValue = "FA" And Trim(Cliente.vRucCliente) = "" Then
                    ElseIf Cliente.cIdTipoPersona = "J" And Trim(Cliente.vRucCliente) = "" Then
                        Throw New Exception("Debe de ingresar el número de ruc, favor de validar la información...!!!")
                    ElseIf trim(Cliente.vEmailCliente) = "" Or FuncionesNeg.IsValidarEmail(trim(Cliente.vEmailCliente)) = False Then
                        Throw New Exception("Debe de ingresar un correo válido, favor de validar la información...!!!")
                    ElseIf Cliente.vDireccionCliente.Trim = "" Then
                        Throw New Exception("Debe de ingresar dirección fiscal, favor de validar la información...!!!")
                    End If

                    hfdIdAuxiliar.Value = grdListaCliente.SelectedRow.Cells(1).Text
                    txtIdCliente.Text = IIf(Cliente.cIdTipoDocumento <> "04", Cliente.vDniCliente, Cliente.vRucCliente)

                    txtRazonSocial.Text = Cliente.vRazonSocialCliente
                    'hfdNroDocumentoCliente.Value = IIf(cboTipoDoc.SelectedValue = "FA" Or cboTipoDoc.SelectedValue = "RH", Cliente.vRucCliente, IIf(Trim(Cliente.vDniCliente) = "", Trim(Cliente.vRucCliente), Trim(Cliente.vDniCliente)))
                    hfdNroDocumentoCliente.Value = IIf(Cliente.cIdTipoDocumento = "04", Cliente.vRucCliente, IIf(Trim(Cliente.vDniCliente) = "", Trim(Cliente.vRucCliente), Trim(Cliente.vDniCliente)))
                    hfdDireccionFiscalCliente.Value = Cliente.vDireccionCliente
                    'cboDireccionEntrega.SelectedValue = "00"
                    hfdTelefonoContactoCliente.Value = Cliente.vTelefonoCliente
                    'txtTelefonoContactoCliente.Text = hfdTelefonoContactoCliente.Value
                    hfdIdTipoPersonaCliente.Value = Cliente.cIdTipoPersona
                    hfdIdTipoCliente.Value = Cliente.cIdTipoCliente

                    Dim UbiGeoNeg As New clsUbicacionGeograficaNegocios
                    Dim UbiGeo As New GNRL_UBICACIONGEOGRAFICA
                    UbiGeo = UbiGeoNeg.UbicacionGeograficaListarPorId(Cliente.cIdPaisUbicacionGeografica, Cliente.cIdDepartamentoUbicacionGeografica, Cliente.cIdProvinciaUbicacionGeografica, Cliente.cIdDistritoUbicacionGeografica)
                    hfdIdUbicacionGeograficaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    'hfdIdUbicacionGeograficaEntregaCliente.Value = UbiGeo.vIdEquivalenciaUbicacionGeografica
                    'ListarDireccionEntregaCombo()
                    'lblCorreoElectronicoCliente.Value = Cliente.vEmailCliente
                    hfdIdTipoDocumentoCliente.Value = Cliente.cIdTipoDocumento
                    'lblDNICliente.Value = Cliente.vDniCliente
                    'lblRUCCliente.Value = Cliente.vRucCliente
                End If
            Else
                lnk_mostrarPanelCliente_ModalPopupExtender.Show()
                Throw New Exception("Seleccione un item de la lista")
            End If
            If txtIdCliente.Text.Trim <> "" Then
                ValidarTexto(True)
            End If
        Catch ex As Exception
            lblMensajeCliente.Text = ex.Message
            lnk_mostrarPanelCliente_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub lnkbtnAgregarEquipo_Click(sender As Object, e As EventArgs) Handles lnkbtnAgregarEquipo.Click
        Try
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0668", strOpcionModulo, "CMMS")
            lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
        Catch ex As Exception
            ValidationSummary1.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    Private Sub imgbtnBuscarSeleccionarEquipo_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarSeleccionarEquipo.Click
        Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("EQU.bTieneContratoEquipo = '1' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
                                                                 txtBuscarContrato.Text, "0")
        Me.grdListaSeleccionarEquipo.DataBind()
        lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
    End Sub

    Private Sub grdListaSeleccionarEquipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdListaSeleccionarEquipo.SelectedIndexChanged
        MyValidator.ErrorMessage = ""
        lblMensajeSeleccionarEquipo.Text = ""
        lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
    End Sub

    Private Sub btnAceptarSeleccionarEquipo_Click(sender As Object, e As EventArgs) Handles btnAceptarSeleccionarEquipo.Click
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            lblMensajeSeleccionarEquipo.Text = ""
            'If Session("IdEmpresa") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            '    Exit Sub
            'End If
            'LimpiarObjetosProducto()
            If grdListaSeleccionarEquipo IsNot Nothing Then
                If grdListaSeleccionarEquipo.Rows.Count > 0 Then
                    If IsNothing(grdListaSeleccionarEquipo.SelectedRow) = False Then
                        If IsReference(grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text) = True Then
                            'Validamos si el equipo está asignado
                            For i = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                                If (Session("CestaEquipoContrato").Rows(i)("IdEquipo").ToString.Trim) = (Server.HtmlDecode(grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text).Trim) Then
                                    lblMensajeSeleccionarEquipo.Text = "Equipo ya ingresado en este contrato"
                                    Throw New Exception("Equipo ya ingresado en este contrato")
                                End If
                            Next
                            Dim EquipoNeg As New clsEquipoNegocios
                            If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & (Server.HtmlDecode(grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text).Trim) & "'").Rows(0).Item(0) = "0" Then
                                Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                            End If




                            '-------------
                            'For x = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                            '    If (Session("CestaEquipoContrato").Rows(x)("IdEquipo").ToString.Trim) = hfdIdEquipo.Value.Trim Then
                            '        'QuitarCestaPersonal(i, Session("CestaEquipoContrato"))
                            '        'Exit For
                            '<Dim strNombreColumna As String = ""
                            'For i = 1 To 31
                            '    If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                            '        If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                            '            strNombreColumna = strNombreColumna & "|" & ("D" & i)
                            '            strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                            '            'IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText)
                            '            Exit For
                            '        End If
                            '    Else
                            '        If strNombreColumna = "" Then
                            '            Throw New Exception("Esta fecha no esta asignada en la programación.")
                            '        End If
                            '    End If
                            'Next

                            '    End If
                            'Next
                            Dim strNombreColumna As String = ""
                            Dim strFechaColumna As String = "|||"
                            For i = 1 To 31
                                If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                                    'If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                    strNombreColumna = strNombreColumna & ("D" & i) & "|"
                                    strFechaColumna = strFechaColumna + grdDetalleEquipos.Columns(2 + i).HeaderText + "||*|||"
                                    'End If
                                End If
                            Next
                            '-------------
                            strNombreColumna = Mid(strNombreColumna, 1, strNombreColumna.Length - 1)
                            'AgregarCestaEquipo(grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text, grdListaSeleccionarEquipo.SelectedRow.Cells(4).Text, "1", strNombreColumna, "|||" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text & "|", Session("CestaEquipoContrato"))
                            AgregarCestaEquipo(False, "0", grdListaSeleccionarEquipo.SelectedRow.Cells(1).Text, grdListaSeleccionarEquipo.SelectedRow.Cells(4).Text, "1", strNombreColumna, strFechaColumna, Session("CestaEquipoContrato"))
                            lblMensajeSeleccionarEquipo.Text = "Equipo asignado satisfactoriamente"
                            grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
                            grdDetalleEquipos.DataBind()
                            lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
                        End If
                    End If
                Else
                    lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
                End If
            End If
        Catch ex As Exception
            lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
            lblMensajeSeleccionarEquipo.Text = ex.Message
        End Try
    End Sub

    Private Sub grdDetalleEquipos_Load(sender As Object, e As EventArgs) Handles grdDetalleEquipos.Load
        'If txtFechaInicial.Text.Trim <> "" And txtFechaFinal.Text.Trim <> "" Then
        '    grdDetalleEquipos.Width = 1000 + (DateDiff(DateInterval.Day, Convert.ToDateTime(txtFechaInicial.Text), Convert.ToDateTime(txtFechaFinal.Text)) * 70)
        '    For i = 0 To DateDiff(DateInterval.Day, Convert.ToDateTime(txtFechaInicial.Text), Convert.ToDateTime(txtFechaFinal.Text))
        '        grdDetalleEquipos.Columns(11 + i).HeaderText = Convert.ToDateTime(txtFechaInicial.Text).AddDays(i)
        '    Next
        'Else
        '    grdDetalleEquipos.Width = 1000
        '    For i = 0 To 49
        '        grdDetalleEquipos.Columns(11 + i).HeaderText = i + 1
        '    Next
        'End If
        grdDetalleEquipos.Width = 1000
        For i = 0 To 30
            grdDetalleEquipos.Columns(3 + i).HeaderText = i + 1
        Next
        If txtFechaVigenciaInicio.Text.Trim <> "" And txtFechaVigenciaFinal.Text.Trim <> "" Then
            'grdDetalleEquipos.Width = 1000 + (DateDiff(DateInterval.Day, Convert.ToDateTime(txtFechaInicial.Text), Convert.ToDateTime(txtFechaFinal.Text)) * 70)
            Dim iDias As Int16 = 0
            Dim strFechaInicial As String = ""
            Dim strFechaFinal As String = ""
            'Establecer la Fecha Inicial
            If String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Or String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") Then
                strFechaInicial = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaVigenciaInicio.Text))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) >= Format(cboPeriodo.SelectedValue, "0000") & String.Format(cboMes.SelectedValue, "00") Then
                'strFechaInicial = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "01"))
                strFechaInicial = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) < String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Then
                'strFechaInicial = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaVigenciaInicio.Text))
                strFechaInicial = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddDays(-1))
            End If
            'Establecer la Fecha Final
            'If String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Or String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) = Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") Then
            If String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) = String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Or (String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) = Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00")) Then
                strFechaFinal = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaVigenciaFinal.Text))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) >= Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") Then
                'strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue) + 1, "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
                strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.Parse("01/" & Format(CInt(cboMes.SelectedValue) + 1, "00") & "/" & Format(CInt(cboPeriodo.SelectedValue), "0000")).AddDays(-1))
            ElseIf String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaInicio.Text)) < String.Format("{0:yyyyMM}", Convert.ToDateTime(txtFechaVigenciaFinal.Text)) Then
                'strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue) + 1, "00") & "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
                strFechaFinal = String.Format("{0:yyyyMMdd}", DateTime.Parse("01/" & Format(CInt(cboMes.SelectedValue) + 1, "00") & "/" & Format(CInt(cboPeriodo.SelectedValue), "0000")).AddDays(-1))
            End If

            'For i = 0 To DateDiff(DateInterval.Day, Convert.ToDateTime(txtFechaInicial.Text), Convert.ToDateTime(txtFechaFinal.Text))
            'For i = 0 To DateDiff(DateInterval.Day, Convert.ToDateTime(strFechaInicial), Convert.ToDateTime(strFechaFinal))
            Dim dFechaInicial, dFechaFinal As Date
            dFechaInicial = DateTime.ParseExact(strFechaInicial, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
            dFechaFinal = DateTime.ParseExact(strFechaFinal, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
            cboPeriodo.SelectedValue = Year(dFechaInicial)
            cboMes.SelectedValue = Month(dFechaInicial)
            grdDetalleEquipos.Width = 500 + (DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal) * 70)
            For i = 0 To DateDiff(DateInterval.Day, dFechaInicial, dFechaFinal)
                'grdDetalleEquipos.Columns(4 + i).HeaderText = Convert.ToDateTime(strFechaInicial).AddDays(i)
                grdDetalleEquipos.Columns(3 + i).HeaderText = dFechaInicial.AddDays(i)
            Next
            'Else
            '    grdDetalleEquipos.Width = 1000
            '    For i = 0 To 30
            '        grdDetalleEquipos.Columns(3 + i).HeaderText = i + 1
            '    Next
        End If
        Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
        Me.grdDetalleEquipos.DataBind()
    End Sub

    Private Sub grdDetalleEquipos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDetalleEquipos.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow AndAlso grdDetalleEquipos.EditIndex = e.Row.RowIndex Then
        '    Dim lnkbtnDia As LinkButton = DirectCast(e.Row.FindControl("lnkbtnD1"), LinkButton)
        'End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'Mantenimientos
            e.Row.Cells(1).Visible = True 'IdEquipo
            e.Row.Cells(2).Visible = True 'DescripcionEquipo
            e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquipos.Columns(3).HeaderText) = True, True, False)
            e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquipos.Columns(4).HeaderText) = True, True, False)
            e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquipos.Columns(5).HeaderText) = True, True, False)
            e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquipos.Columns(6).HeaderText) = True, True, False)
            e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquipos.Columns(7).HeaderText) = True, True, False)
            e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquipos.Columns(8).HeaderText) = True, True, False)
            e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquipos.Columns(9).HeaderText) = True, True, False)
            e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquipos.Columns(10).HeaderText) = True, True, False)
            e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquipos.Columns(11).HeaderText) = True, True, False)
            e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquipos.Columns(12).HeaderText) = True, True, False)
            e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquipos.Columns(13).HeaderText) = True, True, False)
            e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquipos.Columns(14).HeaderText) = True, True, False)
            e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquipos.Columns(15).HeaderText) = True, True, False)
            e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquipos.Columns(16).HeaderText) = True, True, False)
            e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquipos.Columns(17).HeaderText) = True, True, False)
            e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquipos.Columns(18).HeaderText) = True, True, False)
            e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquipos.Columns(19).HeaderText) = True, True, False)
            e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquipos.Columns(20).HeaderText) = True, True, False)
            e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquipos.Columns(21).HeaderText) = True, True, False)
            e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquipos.Columns(22).HeaderText) = True, True, False)
            e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquipos.Columns(23).HeaderText) = True, True, False)
            e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquipos.Columns(24).HeaderText) = True, True, False)
            e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquipos.Columns(25).HeaderText) = True, True, False)
            e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquipos.Columns(26).HeaderText) = True, True, False)
            e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquipos.Columns(27).HeaderText) = True, True, False)
            e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquipos.Columns(28).HeaderText) = True, True, False)
            e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquipos.Columns(29).HeaderText) = True, True, False)
            e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquipos.Columns(30).HeaderText) = True, True, False)
            e.Row.Cells(31).Visible = IIf(IsDate(grdDetalleEquipos.Columns(31).HeaderText) = True, True, False)
            e.Row.Cells(32).Visible = IIf(IsDate(grdDetalleEquipos.Columns(32).HeaderText) = True, True, False)
            e.Row.Cells(33).Visible = IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText) = True, True, False)
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'Mantenimientos
            e.Row.Cells(1).Visible = True 'IdEquipo
            e.Row.Cells(2).Visible = True 'DescripcionEquipo
            e.Row.Cells(3).Visible = IIf(IsDate(grdDetalleEquipos.Columns(3).HeaderText) = True, True, False)
            e.Row.Cells(4).Visible = IIf(IsDate(grdDetalleEquipos.Columns(4).HeaderText) = True, True, False)
            e.Row.Cells(5).Visible = IIf(IsDate(grdDetalleEquipos.Columns(5).HeaderText) = True, True, False)
            e.Row.Cells(6).Visible = IIf(IsDate(grdDetalleEquipos.Columns(6).HeaderText) = True, True, False)
            e.Row.Cells(7).Visible = IIf(IsDate(grdDetalleEquipos.Columns(7).HeaderText) = True, True, False)
            e.Row.Cells(8).Visible = IIf(IsDate(grdDetalleEquipos.Columns(8).HeaderText) = True, True, False)
            e.Row.Cells(9).Visible = IIf(IsDate(grdDetalleEquipos.Columns(9).HeaderText) = True, True, False)
            e.Row.Cells(10).Visible = IIf(IsDate(grdDetalleEquipos.Columns(10).HeaderText) = True, True, False)
            e.Row.Cells(11).Visible = IIf(IsDate(grdDetalleEquipos.Columns(11).HeaderText) = True, True, False)
            e.Row.Cells(12).Visible = IIf(IsDate(grdDetalleEquipos.Columns(12).HeaderText) = True, True, False)
            e.Row.Cells(13).Visible = IIf(IsDate(grdDetalleEquipos.Columns(13).HeaderText) = True, True, False)
            e.Row.Cells(14).Visible = IIf(IsDate(grdDetalleEquipos.Columns(14).HeaderText) = True, True, False)
            e.Row.Cells(15).Visible = IIf(IsDate(grdDetalleEquipos.Columns(15).HeaderText) = True, True, False)
            e.Row.Cells(16).Visible = IIf(IsDate(grdDetalleEquipos.Columns(16).HeaderText) = True, True, False)
            e.Row.Cells(17).Visible = IIf(IsDate(grdDetalleEquipos.Columns(17).HeaderText) = True, True, False)
            e.Row.Cells(18).Visible = IIf(IsDate(grdDetalleEquipos.Columns(18).HeaderText) = True, True, False)
            e.Row.Cells(19).Visible = IIf(IsDate(grdDetalleEquipos.Columns(19).HeaderText) = True, True, False)
            e.Row.Cells(20).Visible = IIf(IsDate(grdDetalleEquipos.Columns(20).HeaderText) = True, True, False)
            e.Row.Cells(21).Visible = IIf(IsDate(grdDetalleEquipos.Columns(21).HeaderText) = True, True, False)
            e.Row.Cells(22).Visible = IIf(IsDate(grdDetalleEquipos.Columns(22).HeaderText) = True, True, False)
            e.Row.Cells(23).Visible = IIf(IsDate(grdDetalleEquipos.Columns(23).HeaderText) = True, True, False)
            e.Row.Cells(24).Visible = IIf(IsDate(grdDetalleEquipos.Columns(24).HeaderText) = True, True, False)
            e.Row.Cells(25).Visible = IIf(IsDate(grdDetalleEquipos.Columns(25).HeaderText) = True, True, False)
            e.Row.Cells(26).Visible = IIf(IsDate(grdDetalleEquipos.Columns(26).HeaderText) = True, True, False)
            e.Row.Cells(27).Visible = IIf(IsDate(grdDetalleEquipos.Columns(27).HeaderText) = True, True, False)
            e.Row.Cells(28).Visible = IIf(IsDate(grdDetalleEquipos.Columns(28).HeaderText) = True, True, False)
            e.Row.Cells(29).Visible = IIf(IsDate(grdDetalleEquipos.Columns(29).HeaderText) = True, True, False)
            e.Row.Cells(30).Visible = IIf(IsDate(grdDetalleEquipos.Columns(30).HeaderText) = True, True, False)
            e.Row.Cells(31).Visible = IIf(IsDate(grdDetalleEquipos.Columns(31).HeaderText) = True, True, False)
            e.Row.Cells(32).Visible = IIf(IsDate(grdDetalleEquipos.Columns(32).HeaderText) = True, True, False)
            e.Row.Cells(33).Visible = IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText) = True, True, False)
        End If
    End Sub

    Protected Sub grdDetalleEquipos_RowCommand_Botones(sender As Object, e As GridViewCommandEventArgs)
        Try
            MyValidator.ErrorMessage = ""
            fValidarSesion()
            'If Session("IdConfEmpresa") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
            '    Exit Sub
            'ElseIf Session("IdUsuario") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            '    Exit Sub
            'End If
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If

            If e.CommandName = "GenerarOT" Then
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0669", strOpcionModulo, "CMMS")
                Dim Valores() As String = e.CommandArgument.ToString.Split("|")
                intIndexContratoEquipo = Valores(6)
                'Dim EquipoNeg As New clsEquipoNegocios
                'If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & Valores(1).ToString & "'").Rows(0).Item(0) = "0" Then
                '    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                'End If
                Dim EquipoNeg As New clsEquipoNegocios
                If EquipoNeg.EquipoGetData("SELECT COUNT(*) FROM LOGI_EQUIPO WHERE cIdEnlaceEquipo = '" & Valores(7).ToString & "'").Rows(0).Item(0) = "0" Then
                    Throw New Exception("No puede generar la orden de trabajo porque no tiene asignado ningun componente el equipo.")
                End If
                hfdOperacionDetalle.Value = "N"

                LimpiarObjetosOrdenTrabajo()
                ValidarTextoOrdenTrabajo(True)
                ActivarObjetosOrdenTrabajo(True)
                LlenarDataOrdenTrabajo(Valores)
                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            ElseIf e.CommandName = "EliminarEquipo" Then
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0670", strOpcionModulo, "CMMS")
                Dim Valores() As String = e.CommandArgument.ToString.Split("|")
                intIndexContratoEquipo = Valores(6)
                hfdIdEquipo.Value = Valores(7)
                If Split(Valores(1), "-")(0) <> "" AndAlso fValidarSiHayMovimientosOrdenTrabajo(Split(Valores(1), "-")(0), Split(Valores(1), "-")(1), Split(Valores(1), "-")(2)) Then
                    lblDescripcionMensajeDocumento.Text = "Existe información ya registrada en este equipo para el periodo " & cboMes.SelectedItem.Text
                    lblNroDocumentoMensajeDocumento.Visible = False
                    btnSiMensajeDocumento.Visible = False
                    btnNoMensajeDocumento.Visible = False
                    lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                Else
                    lblDescripcionMensajeDocumento.Text = "¿Desea eliminar este equipo del contrato?" '& cboMes.SelectedItem.Text
                    lblNroDocumentoMensajeDocumento.Visible = False
                    btnSiMensajeDocumento.Visible = True
                    btnNoMensajeDocumento.Visible = True
                    lnk_mostrarPanelMensajeDocumento_ModalPopupExtender.Show()
                End If
                'Me.grdListaDocumentos.DataSource = fLlenarGrilla()
                'Me.grdListaDocumentos.DataBind()
                ValidationSummary2.ValidationGroup = "vgrpValidar"
                MyValidator.IsValid = False
                MyValidator.ID = "ErrorPersonalizado"
                MyValidator.ValidationGroup = "vgrpValidar"
                Me.Page.Validators.Add(MyValidator)
            ElseIf e.CommandName = "VerMantenimientoPlanificacionEquipo" Then
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0671", strOpcionModulo, "CMMS")
                Dim Valores() As String = e.CommandArgument.ToString.Split("|")
                intIndexContratoEquipo = Valores(6)
                hfdIdEquipo.Value = Valores(7)
                hfdOperacionDetalle.Value = "E"


                LimpiarObjetosOrdenTrabajo()
                ValidarTextoOrdenTrabajo(True)
                ActivarObjetosOrdenTrabajo(True)


                LlenarDataOrdenTrabajo(Valores)

                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            End If
        Catch ex As Exception
            ValidationSummary2.ValidationGroup = "vgrpValidar"
            MyValidator.ErrorMessage = ex.Message
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidar"
            Me.Page.Validators.Add(MyValidator)
        End Try
    End Sub

    'Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
    '    Try
    '        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    '        MyValidator.ErrorMessage = ""
    '        If grdLista IsNot Nothing Then
    '            If grdLista.Rows.Count > 0 Then
    '                If IsNothing(grdLista.SelectedRow) = False Then
    '                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    '                        'txtIdEquipo.Text = grdLista.SelectedRow.Cells(0).Text
    '                        'hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(10).Text) = "True", "1", "0")
    '                        'lnkbtnImprimirTarjetaEquipo.Attributes.Add("onclick", "javascript:popupEmitirEquipoDetalleReporte('" & txtIdEquipo.Text & "');")
    '                        'lnkbtnVerOrdenFabricacion.Attributes.Add("onclick", "javascript:popupEmitirOrdenFabricacionReporte('" & txtIdEquipo.Text & "');")
    '                        lnkbtnVerOrdenTrabajo.Attributes.Add("onclick", "javascript:popupEmitirOrdenTrabajoReporte('" & Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text) & "', '" & Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text) & "', '" & Session("IdEmpresa") & "');")
    '                        If MyValidator.ErrorMessage = "" Then
    '                            MyValidator.ErrorMessage = "Registro encontrado con éxito"
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                'lnk_mostrarPanelCaracteristica_ModalPopupExtender.Show()

    '            End If
    '        End If
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidar"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Private Sub grdLista_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowCreated
    '    If Session("IdUsuario") = "" Then
    '        Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
    '        Exit Sub
    '    End If

    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim td As TableCell
    '        For Each td In e.Row.Cells
    '            e.Row.Cells.Item(0).HorizontalAlign = HorizontalAlign.Center 'IdTipoDocumento
    '            e.Row.Cells.Item(1).HorizontalAlign = HorizontalAlign.Center 'IdNumeroSerie
    '            e.Row.Cells.Item(2).HorizontalAlign = HorizontalAlign.Center 'IdNumeroCorrelativo
    '            e.Row.Cells.Item(3).HorizontalAlign = HorizontalAlign.Center 'IdCliente
    '            e.Row.Cells.Item(4).HorizontalAlign = HorizontalAlign.Center 'IdClienteSAP
    '            e.Row.Cells.Item(5).HorizontalAlign = HorizontalAlign.Left 'RucCliente
    '            e.Row.Cells.Item(6).HorizontalAlign = HorizontalAlign.Left 'RazonSocial
    '            e.Row.Cells.Item(7).HorizontalAlign = HorizontalAlign.Left 'IdEquipo
    '            e.Row.Cells.Item(8).HorizontalAlign = HorizontalAlign.Left 'IdEquipoSAP
    '            e.Row.Cells.Item(9).HorizontalAlign = HorizontalAlign.Left 'DescripcionEquipo
    '            e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'NumeroSerieEquipo
    '            e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'IdArticuloSAPCabecera
    '            e.Row.Cells.Item(10).HorizontalAlign = HorizontalAlign.Left 'Estado Registro
    '        Next
    '    End If
    'End Sub

    'Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
    '    End If
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Cells(0).Visible = True 'IdTipoDocumento
    '        e.Row.Cells(1).Visible = True 'IdNumeroSerie
    '        e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
    '        e.Row.Cells(3).Visible = False 'IdCliente
    '        e.Row.Cells(4).Visible = False 'IdClienteSAP
    '        e.Row.Cells(5).Visible = True 'RucCliente
    '        e.Row.Cells(6).Visible = True 'RazonSocial
    '        e.Row.Cells(7).Visible = True 'IdEquipo
    '        e.Row.Cells(8).Visible = False 'IdEquipoSAP
    '        e.Row.Cells(9).Visible = True 'DescripcionEquipo
    '        e.Row.Cells(10).Visible = True 'NumeroSerieEquipo
    '        e.Row.Cells(11).Visible = False 'IdArticuloSAPCabecera
    '        e.Row.Cells(12).Visible = False 'Estado Registro
    '    End If
    '    If e.Row.RowType = ListItemType.Header Then
    '        e.Row.Cells(0).Visible = True 'IdTipoDocumento
    '        e.Row.Cells(1).Visible = True 'IdNumeroSerie
    '        e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
    '        e.Row.Cells(3).Visible = False 'IdCliente
    '        e.Row.Cells(4).Visible = False 'IdClienteSAP
    '        e.Row.Cells(5).Visible = True 'RucCliente
    '        e.Row.Cells(6).Visible = True 'RazonSocial
    '        e.Row.Cells(7).Visible = True 'IdEquipo
    '        e.Row.Cells(8).Visible = False 'IdEquipoSAP
    '        e.Row.Cells(9).Visible = True 'DescripcionEquipo
    '        e.Row.Cells(10).Visible = True 'NumeroSerieEquipo
    '        e.Row.Cells(11).Visible = False 'IdArticuloSAPCabecera
    '        e.Row.Cells(12).Visible = False 'Estado Registro
    '    End If
    'End Sub

    ''Sub BloquearCheckList(ByVal bIniciar As Boolean, ByVal bPendiente As Boolean, ByVal bRetomar As Boolean, ByVal bFinalizar As Boolean)
    ''    Me.lnkbtnIniciarCheckList.Visible = bIniciar
    ''    Me.lnkbtnPendienteCheckList.Visible = bPendiente
    ''    Me.lnkbtnRetomarCheckList.Visible = bRetomar
    ''    Me.lnkbtnFinalizarCheckList.Visible = bFinalizar
    ''    Me.lnkbtnImprimirOrdenTrabajo3xxx
    ''End Sub

    ''Protected Sub lstComponentes_RowCommand_Botones(sender As Object, e As DataListCommandEventArgs)
    ''    Try
    ''        MyValidator.ErrorMessage = ""
    ''        fValidarSesion()
    ''        'If Session("IdConfEmpresa") = "" Then
    ''        '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
    ''        '    Exit Sub
    ''        'ElseIf Session("IdUsuario") = "" Then
    ''        If Session("IdUsuario") = "" Then
    ''            Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
    ''            Exit Sub
    ''        End If
    ''        'If e.CommandName = "Iniciar" Then
    ''        '    'Función para validar si tiene permisos
    ''        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
    ''        '    Dim Valores() As String = e.CommandArgument.ToString.Split("*")
    ''        '    hfdValoresCheckList.Value = e.CommandArgument.ToString
    ''        '    'Valores(0).ToString - IdCatalogo
    ''        '    'Valores(1).ToString - IdJerarquiaCatalogo
    ''        '    'Valores(2).ToString - IdArticuloSAPCabecera
    ''        '    'Valores(3).ToString - IdActividad
    ''        '    'Valores(4).ToString - IdEquipo
    ''        '    'Valores(5).ToString - IdSistemaFuncional
    ''        '    'Valores(6).ToString - IdTipoActivo
    ''        '    ''ContratoNeg.ContratoGetData("INSERT LOGI_TAREACHECKLISTORDENTRABAJO (nIdNumeroItemTareaCheckListOrdenTrabajo, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo, cIdPersonal, vDescripcionTareaCheckListOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, cIdEquipoCabeceraOrdenTrabajo, cIdActividadCheckListOrdenTrabajo, cIdCatalogoCheckListOrdenTrabajo, cIdJerarquiaCatalogoCheckListOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo) " &
    ''        '    ''                                    "VALUES nIdNumeroItemTareaCheckListOrdenTrabajo, GETDATE(), NULL, cIdPersonal, vDescripcionTareaCheckListOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, Valores(4).ToString, Valores(3).ToString, Valores(0).ToString, Valores(1).ToString, Valores(2).ToString")
    ''        '    ''For Each row As GridViewRow In lstCheckList.Rows
    ''        '    'For Each row As DataListItem In lstCheckList.Items
    ''        '    '    'If row.RowType = DataControlRowType.DataRow Then
    ''        '    '    '    'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalle"), CheckBox)
    ''        '    '    '    Dim lblNroDocumento As Label = TryCast(row.Cells(0).FindControl("lblNroDoc"), Label)
    ''        '    '    'End If
    ''        '    '    If row.ItemType = DataControlRowType.DataRow Or (row.ItemType = ListItemType.AlternatingItem) Then
    ''        '    '        Dim lnkbtnIniciarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    ''        '    '        Dim lnkbtnPendienteCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    ''        '    '        Dim lnkbtnRetomarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    ''        '    '        Dim lnkbtnFinalizarCheckList As LinkButton = TryCast(row.FindControl("lnkbtnIniciarCheckList"), LinkButton)
    ''        '    '        lnkbtnIniciarCheckList.Visible = True
    ''        '    '        lnkbtnPendienteCheckList.Visible = False
    ''        '    '        lnkbtnRetomarCheckList.Visible = False
    ''        '    '        lnkbtnFinalizarCheckList.Visible = False
    ''        '    '    End If
    ''        '    'Next

    ''        '    lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender.Show()
    ''        '    'Dim EmpresaNeg As New clsEmpresaNegocios
    ''        '    'Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(Valores(7).ToString())

    ''        '    'Dim TipoDocNeg As New clsTipoDocumentoNegocios
    ''        '    'Dim TipoDoc As GNRL_TIPODOCUMENTO = TipoDocNeg.TipoDocumentoListarPorId(Valores(4).ToString)

    ''        '    'Dim stDoc As New clsLibSunatMetodos.stCPE
    ''        '    'stDoc.IdTipoDocumento = Valores(4).ToString
    ''        '    'stDoc.IdNumeroSerieDocumento = Valores(5).ToString
    ''        '    'stDoc.IdNumeroCorrelativo = Valores(6).ToString
    ''        '    'stDoc.IdEmpresa = Valores(7).ToString 'Session("IdEmpresa")
    ''        '    'stDoc.IdPuntoVenta = Valores(8).ToString 'Session("IdPuntoVenta")
    ''        '    'stDoc.RutaGeneral = Request.ServerVariables("APPL_PHYSICAL_PATH")
    ''        '    'stDoc.Ruc = Empresa.vRucEmpresa '""
    ''        '    'stDoc.NombreArchivo = Empresa.vRucEmpresa & "-" & TipoDoc.vIdEquivalenciaContableTipoDocumento & "-" & stDoc.IdNumeroSerieDocumento & "-" & stDoc.IdNumeroCorrelativo
    ''        '    'stDoc.FechaProceso = Valores(2).ToString 'Now
    ''        '    'stDoc.NombreCertificado = FuncionesNeg.Desencriptar(TablaSistemaNeg.TablaSistemaListarPorId("44", "04", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema)
    ''        '    'stDoc.IdTipoDocumentoSunat = TipoDoc.vIdEquivalenciaContableTipoDocumento
    ''        '    'stDoc = SunatNeg.GetStatusCDR(stDoc, Replace(Request.ServerVariables("APPL_PHYSICAL_PATH") & "DocXMLAnalizar\" & String.Format("{0:0000}", Year(Valores(2))) & "\" & String.Format("{0:00}", Month(Valores(2))) & "\", "\\", "\"), 1, Session("TipoAmbiente")) 'Recuperar CDR

    ''        '    'If Trim(stDoc.Respuesta) <> "" Then
    ''        '    '    Throw New Exception(stDoc.Respuesta)
    ''        '    'End If

    ''        '    'stDoc = SunatNeg.SunatConfiguracionCPE(stDoc, FuncionesNeg.Desencriptar(TablaSistemaNeg.TablaSistemaListarPorId("44", "05", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema), 6, Session("TipoAmbiente")) 'Analizar CDR Factura.
    ''        '    'If IsNumeric(stDoc.NroTicket) = True Then
    ''        '    '    ComprobanteNeg.DocumentoGetData("UPDATE VTAS_CABECERADOCUMENTO SET vNumeroTicketSunatCabeceraDocumento = '" & stDoc.NroTicket & "' WHERE cIdEmpresa = '" & Valores(7).ToString & "' AND cIdTipoDocumento = '" & stDoc.IdTipoDocumento & "' AND vIdNumeroSerieDocumentoCabeceraDocumento = '" & stDoc.IdNumeroSerieDocumento & "' AND vIdNumeroCorrelativoCabeceraDocumento = '" & stDoc.IdNumeroCorrelativo & "'")
    ''        '    'End If

    ''        '    'ComprobanteNeg.DocumentoGetData("UPDATE VTAS_CABECERADOCUMENTO SET " & IIf(stDoc.CodigoRespuesta = "0", "vStatusSunatCabeceraDocumento = 'ACEPTADO'", IIf(Trim(stDoc.CodigoRespuesta) = "", "vStatusSunatCabeceraDocumento = 'REGISTRADO'", "vStatusSunatCabeceraDocumento = 'CON ERRORES'")) & " WHERE cIdEmpresa = '" & Valores(7).ToString & "' AND cIdTipoDocumento = '" & Valores(4).ToString & "' AND vIdNumeroSerieDocumentoCabeceraDocumento = '" & Valores(5).ToString & "' AND vIdNumeroCorrelativoCabeceraDocumento = '" & Valores(6).ToString & "'")
    ''        '    'Me.grdListaDocumentos.DataSource = fLlenarGrilla()
    ''        '    'Me.grdListaDocumentos.DataBind()
    ''        'ElseIf e.CommandName = "Pendiente" Then
    ''        '    ''Función para validar si tiene permisos
    ''        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "395", strOpcionModulo, Session("IdSistema"), Session("IdArea"))


    ''        '    Dim Valores() As String = e.CommandArgument.ToString.Split("*")
    ''        '    'Valores(0).ToString - IdCatalogo
    ''        '    'Valores(1).ToString - IdJerarquiaCatalogo
    ''        '    'Valores(2).ToString - IdArticuloSAPCabecera
    ''        '    'Valores(3).ToString - IdActividad
    ''        '    'Valores(4).ToString - IdEquipo
    ''        '    'Valores(5).ToString - IdSistemaFuncional
    ''        '    'Valores(6).ToString - IdTipoActivo
    ''        '    'ContratoNeg.ContratoGetData("INSERT LOGI_TAREACHECKLISTORDENTRABAJO (nIdNumeroItemTareaCheckListOrdenTrabajo, dFechaInicioTareaCheckListOrdenTrabajo, dFechaFinalTareaCheckListOrdenTrabajo, cIdPersonal, vDescripcionTareaCheckListOrdenTrabajo, cIdTipoDocumentoCabeceraOrdenTrabajo, vIdNumeroSerieCabeceraOrdenTrabajo, vIdNumeroCorrelativoCabeceraOrdenTrabajo, cIdEmpresa, cIdEquipoCabeceraOrdenTrabajo, cIdActividadCheckListOrdenTrabajo, cIdCatalogoCheckListOrdenTrabajo, cIdJerarquiaCatalogoCheckListOrdenTrabajo, vIdArticuloSAPCabeceraOrdenTrabajo) " &
    ''        '    '                            "VALUES (" & "(SELECT ISNULL(MAX(nIdNumeroItemTareaCheckListOrdenTrabajo), 0) + 1 " &
    ''        '    '                            "FROM LOGI_TAREACHECKLISTORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "')" & ", " &
    ''        '    '                            "GETDATE(), NULL, '" & cboPersonalAsignadoMantenimientoTarea.SelectedValue & "', '" & UCase(txtDescripcionMantenimientoTarea.Text.Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "', '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "', '" & Session("IdEmpresa") & "', '" & Valores(4).ToString & "', '" & Valores(3).ToString & "', '" & Valores(0).ToString & "', '" & Valores(1).ToString & "', '" & Valores(2).ToString & "')")
    ''        '    'ContratoNeg.ContratoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoCheckListOrdenTrabajo = 'E' " &
    ''        '    '                            "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")
    ''        '    ContratoNeg.ContratoGetData("UPDATE LOGI_TAREACHECKLISTORDENTRABAJO SET dFechaFinalTareaCheckListOrdenTrabajo = GETDATE() " &
    ''        '                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "' AND dFechaFinalTareaCheckListOrdenTrabajo IS NULL")

    ''        '    ContratoNeg.ContratoGetData("UPDATE LOGI_CHECKLISTORDENTRABAJO SET cEstadoCheckListOrdenTrabajo = 'P' " &
    ''        '                                "WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoCabeceraOrdenTrabajo = '" & Valores(4).ToString & "' AND cIdActividadCheckListOrdenTrabajo = '" & Valores(3).ToString & "' AND cIdCatalogoCheckListOrdenTrabajo = '" & Valores(0).ToString & "' AND cIdJerarquiaCatalogoCheckListOrdenTrabajo = '" & Valores(1).ToString & "' AND vIdArticuloSAPCabeceraOrdenTrabajo = '" & Valores(2).ToString & "'")


    ''        '    Dim CheckListNeg As New clsCheckListMetodos 'clsGaleriaEquipoNegocios
    ''        '    Me.lstCheckList.DataSource = CheckListNeg.CheckListListaGridPorOrdEquipo((Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim),
    ''        '                                                                             (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim),
    ''        '                                                                             (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim),
    ''        '                                                                             Session("IdEmpresa"),
    ''        '                                                                             (Server.HtmlDecode(grdLista.SelectedRow.Cells(7).Text).Trim),
    ''        '                                                                             (Server.HtmlDecode(grdLista.SelectedRow.Cells(11).Text).Trim))
    ''        '    Me.lstCheckList.DataBind()
    ''        '    CargarStatusActividad()
    ''        '    pnlCabecera.Visible = False
    ''        '    pnlContenido.Visible = True

    ''        '    'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
    ''        '    'Dim EmpresaNeg As New clsEmpresaNegocios
    ''        '    'Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(Valores(7).ToString)

    ''        '    'Dim TipoDocNeg As New clsTipoDocumentoNegocios
    ''        '    'Dim TipoDoc As GNRL_TIPODOCUMENTO = TipoDocNeg.TipoDocumentoListarPorId(Valores(4).ToString)

    ''        '    'Dim stDoc As New clsLibSunatMetodos.stCPE
    ''        '    'stDoc.IdTipoDocumento = Valores(4).ToString
    ''        '    'stDoc.IdNumeroSerieDocumento = Valores(5).ToString
    ''        '    'stDoc.IdNumeroCorrelativo = Valores(6).ToString
    ''        '    'stDoc.IdEmpresa = Valores(7).ToString() 'Session("IdEmpresa")
    ''        '    'stDoc.IdPuntoVenta = Valores(8).ToString 'Session("IdPuntoVenta")
    ''        '    'stDoc.RutaGeneral = Request.ServerVariables("APPL_PHYSICAL_PATH")
    ''        '    'stDoc.Ruc = Empresa.vRucEmpresa '""
    ''        '    'stDoc.NombreArchivo = Empresa.vRucEmpresa & "-" & TipoDoc.vIdEquivalenciaContableTipoDocumento & "-" & stDoc.IdNumeroSerieDocumento & "-" & stDoc.IdNumeroCorrelativo
    ''        '    'stDoc.FechaProceso = Valores(2).ToString 'Now
    ''        '    'stDoc.NombreCertificado = FuncionesNeg.Desencriptar(TablaSistemaNeg.TablaSistemaListarPorId("44", "04", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema)
    ''        '    'ComprobanteNeg.DocumentoGetData("UPDATE VTAS_CABECERADOCUMENTO SET vNombreArchivoCabeceraDocumento = '" & stDoc.NombreArchivo & "' WHERE cIdTipoDocumento = '" & stDoc.IdTipoDocumento & "' AND vIdNumeroSerieDocumentoCabeceraDocumento = '" & stDoc.IdNumeroSerieDocumento & "' AND vIdNumeroCorrelativoCabeceraDocumento = '" & stDoc.IdNumeroCorrelativo & "' AND cIdEmpresa = '" & Valores(7).ToString & "'")

    ''        '    'stDoc = SunatNeg.SunatConfiguracionCPE(stDoc, FuncionesNeg.Desencriptar(TablaSistemaNeg.TablaSistemaListarPorId("44", "05", Session("IdSistema"), Session("IdEmpresa"), "*").vValorOpcionalTablaSistema), 4, Session("TipoAmbiente")) 'Envio solo el ZIP.
    ''        '    'If IsNumeric(stDoc.NroTicket) = True Then
    ''        '    '    ComprobanteNeg.DocumentoGetData("UPDATE VTAS_CABECERADOCUMENTO SET vNumeroTicketSunatCabeceraDocumento = '" & stDoc.NroTicket & "' WHERE cIdTipoDocumento = '" & stDoc.IdTipoDocumento & "' AND vIdNumeroSerieDocumentoCabeceraDocumento = '" & stDoc.IdNumeroSerieDocumento & "' AND vIdNumeroCorrelativoCabeceraDocumento = '" & stDoc.IdNumeroCorrelativo & "' AND cIdEmpresa = '" & Valores(7).ToString & "'")
    ''        '    'End If
    ''        '    'ComprobanteNeg.DocumentoGetData("UPDATE VTAS_CABECERADOCUMENTO SET " & IIf(stDoc.CodigoRespuesta = "0", "vStatusSunatCabeceraDocumento = 'ACEPTADO'", IIf(Trim(stDoc.CodigoRespuesta) = "", "vStatusSunatCabeceraDocumento = 'REGISTRADO'", "vStatusSunatCabeceraDocumento = 'CON ERRORES'")) & " WHERE cIdTipoDocumento = '" & Valores(4).ToString & "' AND vIdNumeroSerieDocumentoCabeceraDocumento = '" & Valores(5).ToString & "' AND vIdNumeroCorrelativoCabeceraDocumento = '" & Valores(6).ToString & "' AND cIdEmpresa = '" & Valores(7).ToString & "'")

    ''        '    'If stDoc.CodigoRespuesta = "" Then
    ''        '    '    If stDoc.Respuesta = "" Then
    ''        '    '        MyValidator.ErrorMessage = "Por favor intente nuevamente, no hubo conexión con SUNAT"
    ''        '    '    Else
    ''        '    '        MyValidator.ErrorMessage = stDoc.Respuesta
    ''        '    '    End If
    ''        '    'Else
    ''        '    '    If InStr(UCase(stDoc.Respuesta), "ACEPTADA") > 0 Then
    ''        '    '        Dim strHTML As String
    ''        '    '        Dim TipoMonNeg As New clsTipoMonedaNegocios
    ''        '    '        Dim DocNeg As New clsDocumentoNegocios
    ''        '    '        Dim dsDocumento = DocNeg.DocumentoListaBusqueda("DATEDIFF(dd, dFechaEmisionCabeceraDocumento, '" & txtFechaEmisionInicial.Text & "') <= 0 AND DATEDIFF(dd, dFechaEmisionCabeceraDocumento, '" & txtFechaEmisionFinal.Text & "') >= 0 AND DOC.cIdTipoDocumento = '" & Valores(4).ToString & "' AND DOC.vIdNumeroSerieDocumentoCabeceraDocumento = '" & Valores(5).ToString & "' AND DOC.vIdNumeroCorrelativoCabeceraDocumento", Valores(6).ToString, Valores(7).ToString, Valores(8).ToString, "DESC")
    ''        '    '        Dim dtDocEmi As New DataTable
    ''        '    '        dtDocEmi = FuncionesNeg.ConvertirListADataTable(dsDocumento)
    ''        '    '        For Each Documento In dtDocEmi.Rows
    ''        '    '            Dim TipoMon As GNRL_TIPOMONEDA = TipoMonNeg.TipoMonedaListarPorId(Documento("IdTipoMoneda"))
    ''        '    '            strHTML = FuncionesNeg.FormatoEnvio(Documento("RazonSocial"), Documento("NumeroSerieDocumento"), Documento("NumeroCorrelativo"),
    ''        '    '                                            TipoDoc.vDescripcionTipoDocumento & " ELECTRÓNICA", TipoMon.vDescripcionAbreviadaTipoMoneda & " " & Convert.ToString(Math.Round(Convert.ToDecimal(Documento("TotalPrecio")), 2)), String.Format("{0:dd-MM-yyyy}", Documento("FechaEmision")), Empresa.vDescripcionEmpresa.Trim)
    ''        '    '            Dim strFrom As String = TablaSistemaNeg.TablaSistemaListarPorId("45", "15", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema
    ''        '    '            Dim strPwd As String = TablaSistemaNeg.TablaSistemaListarPorId("45", "19", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema
    ''        '    '            Dim strConfiguracionCorreo As String = TablaSistemaNeg.TablaSistemaListarPorId("45", "22", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema & "|" & TablaSistemaNeg.TablaSistemaListarPorId("45", "23", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema
    ''        '    '            Dim strConfiguracionPeriodo As String = String.Format("{0:0000}", Year(Documento("FechaEmision"))) & "|" & String.Format("{0:00}", Month(Documento("FechaEmision")))
    ''        '    '            Dim ClienteNeg As New clsClienteNegocios
    ''        '    '            Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorId(Documento("IdCliente"), Valores(7).ToString, Valores(8).ToString)
    ''        '    '            If Cliente.vEmailCliente = "" Then
    ''        '    '                MyValidator.ErrorMessage = "Debe de ingresar un correo válido para este cliente."
    ''        '    '            Else
    ''        '    '                FuncionesNeg.EnviarCorreo(LCase(strFrom), strPwd, LCase(Cliente.vEmailCliente), "Factura Electrónica " & stDoc.NombreArchivo, strHTML, stDoc.RutaGeneral, stDoc.NombreArchivo, strConfiguracionCorreo, strConfiguracionPeriodo, "DocXMLPDF", True)
    ''        '    '            End If
    ''        '    '        Next
    ''        '    '    End If
    ''        '    'End If
    ''        '    'Me.grdListaDocumentos.DataSource = fLlenarGrilla()
    ''        '    'Me.grdListaDocumentos.DataBind()
    ''        '    'ValidationSummary1.ValidationGroup = "vgrpValidar"
    ''        '    'MyValidator.IsValid = False
    ''        '    'MyValidator.ID = "ErrorPersonalizado"
    ''        '    'MyValidator.ValidationGroup = "vgrpValidar"
    ''        '    'Me.Page.Validators.Add(MyValidator)
    ''        'ElseIf e.CommandName = "Retomar" Then
    ''        '    ''Función para validar si tiene permisos
    ''        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
    ''        '    'hfdValoresReprocesar.Value = e.CommandArgument.ToString
    ''        '    'lnk_mostrarPanelMensajeReprocesar_ModalPopupExtender.Show()
    ''        'ElseIf e.CommandName = "Finalizar" Then
    ''        '    ''Función para validar si tiene permisos
    ''        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "398", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
    ''        '    'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
    ''        '    'Dim strSerieCajero As String
    ''        '    'strSerieCajero = FuncionesNeg.FuncionesGetData("SELECT vIdNumeroSerieDocumentoCajero FROM VTAS_CAJERO " &
    ''        '    '                                               "WHERE cIdCajero = '" & Session("IdUsuario") & "' " &
    ''        '    '                                               "AND cIdEmpresa = '" & Valores(9).ToString & "' " &
    ''        '    '                                               "AND cIdPuntoVenta = '" & Valores(10).ToString & "' " &
    ''        '    '                                               "AND cIdTipoDocumentoCajero = '" & Valores(4).ToString & "'").Rows(0).Item(0).ToString
    ''        '    'If Valores(5).ToString <> strSerieCajero Then
    ''        '    '    Throw New Exception("Debe de ser la misma serie de venta para usar esta opción.")
    ''        '    'End If
    ''        '    'strDatoCompartidoAnulado = Valores
    ''        '    ''Valores(0).ToString - IdCliente
    ''        '    ''Valores(1).ToString - StatusSunat
    ''        '    ''Valores(2).ToString - FechaEmision
    ''        '    ''Valores(3).ToString - NombreArchivo
    ''        '    ''Valores(4).ToString - IdTipoDocumento
    ''        '    ''Valores(5).ToString - NumeroSerieDocumento
    ''        '    ''Valores(6).ToString - NumeroCorrelativo
    ''        '    ''Valores(7).ToString - RUCCliente
    ''        '    ''Valores(8).ToString - RazonSocial
    ''        '    'lnk_mostrarPanelMensajeAnular_ModalPopupExtender.Show()
    ''        'ElseIf e.CommandName = "EstadoMalo" Then
    ''        '    'MyValidator.ErrorMessage = ""
    ''        '    'fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    ''        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "399", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
    ''        '    MsgBox("ya pe")
    ''        '    'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
    ''        '    'Dim strSerieCajero As String
    ''        '    'strSerieCajero = FuncionesNeg.FuncionesGetData("SELECT vIdNumeroSerieDocumentoCajero FROM VTAS_CAJERO " &
    ''        '    '                                               "WHERE cIdCajero = '" & Session("IdUsuario") & "' " &
    ''        '    '                                               "AND cIdEmpresa = '" & Valores(7).ToString & "' " &
    ''        '    '                                               "AND cIdPuntoVenta = '" & Valores(8).ToString & "' " &
    ''        '    '                                               "AND cIdTipoDocumentoCajero = '" & Valores(4).ToString & "'").Rows(0).Item(0).ToString
    ''        '    'strDatoCompartidoGeneral = Valores
    ''        '    'pnlCabecera.Visible = False
    ''        '    'pnlNotas.Visible = True
    ''        '    'lblIdTipoDocumentoReferenciaNota.Text = Valores(4).ToString
    ''        '    'lblIdNroSerieDocumentoReferenciaNota.Text = Valores(5).ToString
    ''        '    'lblIdNroDocumentoReferenciaNota.Text = Valores(6).ToString
    ''        '    'ListarTipoDocumentoNotaCombo()
    ''        '    'ListarNroSerieNotaCombo()
    ''        '    'ListarTipoMonedaNotaCombo()
    ''        '    'ListarTipoOperacionNotaDocumentoCombo()
    ''        '    'LimpiarObjetosNotas()
    ''        '    'LimpiarObjetosProducto()
    ''        '    'CargarCorrelativo()
    ''        '    'CargarCestaComprobante()
    ''        '    'ValidarTexto(False)
    ''        '    'ValidarTextoNotas(True)
    ''        '    'ActivarObjetos(False)
    ''        '    'Me.grdDetalleNota.DataSource = Session("CestaComprobanteNota")
    ''        '    'Me.grdDetalleNota.DataBind()
    ''        '    'ActivarObjetosNotas(True)
    ''        'ElseIf e.CommandName = "EstadoRegular" Then
    ''        '    ''Función para validar si tiene permisos
    ''        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "412", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
    ''        '    'Dim Valores() As String = e.CommandArgument.ToString.Split("*")

    ''        '    'strDatoCompartidoEmail = Valores

    ''        '    'Dim ClienteNeg As New clsClienteNegocios
    ''        '    'Dim Cliente As GNRL_CLIENTE = ClienteNeg.ClienteListarPorId(Valores(0).ToString, Valores(7).ToString, Valores(8).ToString)

    ''        '    'If Cliente.vEmailCliente = "" Then
    ''        '    '    txtEmailMensajeCorreo.Text = TablaSistemaNeg.TablaSistemaListarPorId("45", "15", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema
    ''        '    'Else
    ''        '    '    txtEmailMensajeCorreo.Text = Cliente.vEmailCliente
    ''        '    'End If
    ''        '    'lnk_mostrarPanelCorreo_ModalPopupExtender.Show()
    ''        'ElseIf e.CommandName = "EstadoBueno" Then
    ''        '    ''Función para validar si tiene permisos
    ''        '    'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "401", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
    ''        '    'Dim Valores() As String = e.CommandArgument.ToString.Split("*")
    ''        '    'strDatoCompartidoGeneral = Valores
    ''        '    'Dim EmpresaNeg As New clsEmpresaNegocios
    ''        '    'Dim Empresa As GNRL_EMPRESA = EmpresaNeg.EmpresaListarPorId(Valores(7).ToString)

    ''        '    'Dim TipoDocNeg As New clsTipoDocumentoNegocios
    ''        '    'Dim TipoDoc As GNRL_TIPODOCUMENTO = TipoDocNeg.TipoDocumentoListarPorId(Valores(4).ToString)

    ''        '    'Dim stDoc As New clsLibSunatMetodos.stCPE
    ''        '    'stDoc.IdTipoDocumento = Valores(4).ToString 'Documento.cIdTipoDocumento
    ''        '    'stDoc.IdNumeroSerieDocumento = Valores(5).ToString 'Documento.vIdNumeroSerieDocumentoCabeceraDocumento
    ''        '    'stDoc.IdNumeroCorrelativo = Valores(6).ToString 'Documento.vIdNumeroCorrelativoCabeceraDocumento
    ''        '    'stDoc.IdEmpresa = Valores(7).ToString 'Session("IdEmpresa")
    ''        '    'stDoc.IdPuntoVenta = Valores(8).ToString 'Session("IdPuntoVenta")
    ''        '    'stDoc.RutaGeneral = Request.ServerVariables("APPL_PHYSICAL_PATH")
    ''        '    'stDoc.Ruc = Empresa.vRucEmpresa '""
    ''        '    'stDoc.NombreArchivo = Empresa.vRucEmpresa & "-" & TipoDoc.vIdEquivalenciaContableTipoDocumento & "-" & Valores(5).ToString & "-" & Valores(6).ToString
    ''        '    'stDoc.FechaProceso = Valores(2).ToString 'Now
    ''        '    'stDoc.NombreCertificado = FuncionesNeg.Desencriptar(TablaSistemaNeg.TablaSistemaListarPorId("44", "04", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema)
    ''        '    'stDoc.IdTipoDocumentoSunat = TipoDoc.vIdEquivalenciaContableTipoDocumento
    ''        '    'Dim dsDocEmi = DocumentoNeg.DocumentoListaBusqueda("DATEDIFF(dd, dFechaEmisionCabeceraDocumento, '" & txtFechaEmisionInicial.Text & "') <= 0 AND DATEDIFF(dd, dFechaEmisionCabeceraDocumento, '" & txtFechaEmisionFinal.Text & "') >= 0 AND DOC.cIdTipoDocumento = '" & Valores(4).ToString & "' AND DOC.vIdNumeroCorrelativoCabeceraDocumento", Valores(6).ToString, Valores(7).ToString, Valores(8).ToString, "DESC")
    ''        '    'Me.grdListaDocumentos.DataSource = fLlenarGrilla()
    ''        '    'Me.grdListaDocumentos.DataBind()



    ''        '    'Dim dsDocEmitido As New DataTable
    ''        '    'dsDocEmitido = DocumentoNeg.DocumentoGetData("SELECT cIdCliente, nTotalIGVCabeceraDocumento, nTotalPrecioVentaCabeceraDocumento, dFechaEmisionCabeceraDocumento FROM VTAS_CABECERADOCUMENTO WHERE cIdEmpresa = '" & strDatoCompartidoGeneral(9).ToString & "' AND cIdPuntoVenta = '" & strDatoCompartidoGeneral(10).ToString & "' AND cIdTipoDocumento = '" & strDatoCompartidoGeneral(4).ToString & "' AND vIdNumeroSerieDocumentoCabeceraDocumento = '" & strDatoCompartidoGeneral(5).ToString & "' AND vIdNumeroCorrelativoCabeceraDocumento = '" & strDatoCompartidoGeneral(6).ToString & "'")

    ''        '    'Dim strQR As String
    ''        '    'strQR = stDoc.Ruc & "|" & stDoc.IdTipoDocumentoSunat & "|" & stDoc.IdNumeroSerieDocumento & "|" & stDoc.IdNumeroCorrelativo & "|" & Math.Round(Convert.ToDecimal(dsDocEmitido.Rows(0).Item(1)), 2) & "|" & Math.Round(Convert.ToDecimal(dsDocEmitido.Rows(0).Item(2)), 2) & "|" & String.Format("{0:yyyy-MM-dd}", dsDocEmitido.Rows(0).Item(3)) & "|0" & "|0|" & stDoc.CodigoHash & "|" & stDoc.CodigoSignature & "|"
    ''        '    'If TablaSistemaNeg.TablaSistemaListarPorId("45", "17", Session("IdSistema"), Valores(7).ToString, "*").vValorOpcionalTablaSistema = "0" Then 'Previsualizar reporte 1.- Si  0.- No
    ''        '    '    ImprimirDirecto(Valores(4).ToString, Valores(5).ToString, Valores(6).ToString, Valores(8).ToString, Valores(7).ToString, Request.ServerVariables("APPL_PHYSICAL_PATH"), strNombreArchivo, strQR)
    ''        '    '    MyValidator.ErrorMessage = "Transacción reprocesada con éxito"
    ''        '    'Else
    ''        '    '    ImprimirDirecto(Valores(4).ToString, Valores(5).ToString, Valores(6).ToString, Valores(8).ToString, Valores(7).ToString, Request.ServerVariables("APPL_PHYSICAL_PATH"), strNombreArchivo, strQR)
    ''        '    'End If
    ''        '    'ValidationSummary1.ValidationGroup = "vgrpValidar"
    ''        '    'MyValidator.IsValid = False
    ''        '    'MyValidator.ID = "ErrorPersonalizado"
    ''        '    'MyValidator.ValidationGroup = "vgrpValidar"
    ''        '    'Me.Page.Validators.Add(MyValidator)
    ''        'End If
    ''    Catch ex As Exception
    ''        ValidationSummary1.ValidationGroup = "vgrpValidar"
    ''        MyValidator.ErrorMessage = ex.Message
    ''        MyValidator.IsValid = False
    ''        MyValidator.ID = "ErrorPersonalizado"
    ''        MyValidator.ValidationGroup = "vgrpValidar"
    ''        Me.Page.Validators.Add(MyValidator)
    ''    End Try
    ''End Sub


    'Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
    '    Dim SubFiltro As String = ""
    '    SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
    '                    "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
    '                    "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
    '                    "INNER JOIN GNRL_USUARIO AS USU ON " &
    '                    "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
    '                    "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

    '    Me.grdLista.DataSource = ContratoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
    '    'Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroPersonal.SelectedValue,
    '    '                                                             txtBuscarPersonal.Text, Session("IdEmpresa"), Session("IdPuntoVenta"))
    '    Me.grdLista.DataBind()
    '    pnlCabecera.Visible = True
    '    pnlContenido.Visible = False
    '    BloquearMantenimiento(True, False, True, False)
    'End Sub

    'Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
    '    grdLista.PageIndex = e.NewPageIndex
    '    Dim SubFiltro As String = ""
    '    SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
    '                    "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
    '                    "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
    '                    "INNER JOIN GNRL_USUARIO AS USU ON " &
    '                    "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
    '                    "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

    '    Me.grdLista.DataSource = ContratoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
    '    Me.grdLista.DataBind() 'Recargo el grid.
    '    grdLista.SelectedIndex = -1
    'End Sub

    ''Private Sub lnkbtnTerminarOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles lnkbtnTerminarOrdenTrabajo.Click
    ''    Try
    ''        'Función para validar si tiene permisos
    ''        MyValidator.ErrorMessage = ""
    ''        fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
    ''        'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0637", strOpcionModulo, Session("IdSistema"))

    ''        If grdLista IsNot Nothing Then
    ''            If grdLista.Rows.Count > 0 Then
    ''                If IsNothing(grdLista.SelectedRow) = False Then
    ''                    If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
    ''                        MyValidator.ErrorMessage = "Orden de Trabajo Terminada"
    ''                    End If
    ''                Else
    ''                    Throw New Exception("Debe de seleccionar un item.")
    ''                End If
    ''            End If
    ''        End If
    ''        hfdOperacion.Value = "R"
    ''        BloquearMantenimiento(False, True, False, True)
    ''        'LimpiarObjetos()
    ''        ContratoNeg.ContratoGetData("UPDATE LOGI_CABECERAORDENTRABAJO SET cEstadoCabeceraOrdenTrabajo = 'T' WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ")
    ''        'ContratoNeg.ContratoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'P' WHERE cIdTipoDocumentoCabeceraOrdenFabricacion + '-' + vIdNumeroSerieCabeceraOrdenFabricacion + '-' + vIdNumeroCorrelativoCabeceraOrdenFabricacion = ''" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoSAPCabeceraOrdenFabricacion = '" & hfdIdEquipoSAPEquipo.Value & "' AND vIdArticuloSAPCabeceraOrdenFabricacion = '" & hfdIdArticuloSAPPrincipal.Value & "'")
    ''        Dim strOrdenFabricacion As String = ContratoNeg.ContratoGetData("SELECT vOrdenFabricacionReferenciaCabeceraOrdenTrabajo FROM LOGI_CABECERAORDENTRABAJO WHERE cIdTipoDocumentoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(0).Text).Trim) & "' AND vIdNumeroSerieCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(1).Text).Trim) & "' AND vIdNumeroCorrelativoCabeceraOrdenTrabajo = '" & (Server.HtmlDecode(grdLista.SelectedRow.Cells(2).Text).Trim) & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' ").Rows(0).Item(0).ToString
    ''        ContratoNeg.ContratoGetData("UPDATE LOGI_CABECERAORDENFABRICACION SET cEstadoCabeceraOrdenFabricacion = 'T' WHERE cIdTipoDocumentoCabeceraOrdenFabricacion + '-' + vIdNumeroSerieCabeceraOrdenFabricacion + '-' + vIdNumeroCorrelativoCabeceraOrdenFabricacion = '" & strOrdenFabricacion & "'")
    ''        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    ''        MyValidator.IsValid = False
    ''        MyValidator.ID = "ErrorPersonalizado"
    ''        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    ''        Me.Page.Validators.Add(MyValidator)

    ''        'Dim SubFiltro As String = ""
    ''        'SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
    ''        '            "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
    ''        '            "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
    ''        '            "INNER JOIN GNRL_USUARIO AS USU ON " &
    ''        '            "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
    ''        '            "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

    ''        'Me.grdLista.DataSource = ContratoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
    ''        Me.grdLista.DataSource = PersonalNeg.PersonalListaBusqueda(cboFiltroPersonal.SelectedValue,
    ''                                                                 txtBuscarPersonal.Text, Session("IdEmpresa"), Session("IdPuntoVenta"))
    ''        Me.grdLista.DataBind()

    ''        pnlCabecera.Visible = True
    ''        pnlContenido.Visible = False
    ''    Catch ex As Exception
    ''        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    ''        MyValidator.ErrorMessage = ex.Message
    ''        MyValidator.IsValid = False
    ''        MyValidator.ID = "ErrorPersonalizado"
    ''        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    ''        Me.Page.Validators.Add(MyValidator)
    ''    End Try
    ''End Sub



    Private Sub btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            'lblMensajeCatalogoComponente.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0638", strOpcionModulo, Session("IdSistema"))

            If grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count > 0 Then
                If grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex < grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows.Count Then
                    'If IsNothing(grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedRow) = False Then
                    'If IsReference(grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedRow.Cells(1).Text) = True Then
                    'Me.grdDetalleCaracteristicaEquipoComponente.DataSource = Session("CestaCaracteristicaEquipoComponenteFiltrado")
                    'Me.grdDetalleCaracteristicaEquipoComponente.DataBind()
                    'lnk_mostrarPanelEquipoComponente_ModalPopupExtender.Show()
                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        'If (Session("CestaActividadCatalogoComponente").Rows(i)("Codigo").ToString.Trim) = (cboActividadCatalogoComponente.SelectedValue.ToString.Trim) And Session("CestaActividadCatalogoComponente").Rows(i)("IdCatalogo").ToString.Trim = txtIdCatalogoComponente.Text.Trim Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                        If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = (cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedValue.ToString.Trim) Then 'And (Session("CestaCatalogoCaracteristica").Rows(i)("CodigoVariante").ToString.Trim) = (Server.HtmlDecode(grdListaProducto.SelectedRow.Cells(3).Text).Trim) Then
                            Throw New Exception("Personal ya registrado, seleccione otro item.")
                            'Exit Sub
                        End If
                    Next
                    'End If
                    'Else
                    '    Throw New Exception("Debe de seleccionar algún item de la lista de trabajadores.")
                    'End If
                End If
            End If

            'AgregarCesta(txtIdCatalogoComponente.Text, "", "1", cboActividadCatalogoComponente.SelectedValue.Trim, UCase(cboActividadCatalogoComponente.SelectedItem.Text).Trim, "", "", "", Session("CestaCaracteristicaCatalogoComponente"))
            If cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                Throw New Exception("Debe de seleccionar un personal.")
                Exit Sub
            Else
                'AgregarCestaPersonal(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(1).Text).Trim, "1", "1", Server.HtmlDecode(grdCatalogoComponente.SelectedRow.Cells(2).Text).Trim, 0, Session("CestaActividadCatalogoComponente"))
                AgregarCestaPersonal(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, False, Session("CestaSSPersonal"))
            End If
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
            grdPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = -1
            'MyValidator.ErrorMessage = "Personal agregado con éxito."
            'ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'Me.Page.Validators.Add(MyValidator)
            lblMensajeMantenimientoOrdenTrabajo.Text = "Personal agregado con éxito."
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            'ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'MyValidator.ErrorMessage = ex.Message
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub btnAceptarMantenimientoOrdenTrabajo_Click(sender As Object, e As EventArgs) Handles btnAceptarMantenimientoOrdenTrabajo.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            lblMensajeMantenimientoOrdenTrabajo.Text = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0646", strOpcionModulo, Session("IdSistema"))

            If txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text = "" Then
                Throw New Exception("Debe de ingresar la fecha de inicio de planificación del mantenimiento.")
                'ElseIf cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                '    Throw New Exception("Debe de seleccionar un tipo de mantenimiento.")
                'ElseIf cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                '    Throw New Exception("Debe de seleccionar un personal responsable.")
            ElseIf cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue = "" Then
                Throw New Exception("Debe de seleccionar una plantilla.")
                'ElseIf cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndex = 0 Then
                '    Throw New Exception("Debe de seleccionar un personal asginado.")
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

            Dim gvRow As GridViewRow = grdDetalleEquipos.Rows(intIndexContratoEquipo - ((grdDetalleEquipos.PageSize) * (grdDetalleEquipos.PageIndex))) 'JMUG: 19/05/2022
            Dim strNombreColumnaModificar As String = ""
            Dim strNombreColumna As String = ""
            'Obtener Nro.Columna
            'For x = 1 To 31
            For i = 1 To 31
                If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                    If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                        strNombreColumna = ("D" & i)
                        'IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText)
                        'Exit For
                    End If
                    If IsDate(hfdFechaPlanificacion.Value) AndAlso CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(hfdFechaPlanificacion.Value) Then
                        strNombreColumnaModificar = ("D" & i)
                        'IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText)
                        'Exit For
                    End If
                Else
                    If strNombreColumna = "" Then
                        Throw New Exception("Esta fecha no esta asignada en la programación.")
                    End If
                End If
            Next
            'Next
            Dim resultProgramacionContratoSimple As DataRow() = Session("CestaEquipoContrato").Select("IdEquipo = '" & hfdIdEquipo.Value & "'")
            'rowIndexDetalle = Session("CestaEquipoContrato").Rows.IndexOf(resultProgramacionContratoSimple(0))
            If resultProgramacionContratoSimple.Length = 0 Then
                'VaciarCestaInsumos(Session("CestaInsumosFiltrado"))
            Else
                Dim rowFil As DataRow() = resultProgramacionContratoSimple
                For Each EquipoContrato As DataRow In rowFil
                    If hfdFechaPlanificacion.Value <> "" Then
                        'EditarCestaEquipo(hfdIdEquipo.Value, EquipoContrato("DescripcionEquipo"), EquipoContrato("Estado"),
                        '              hfdFechaPlanificacion.Value, strNombreColumnaModificar, "||||", Session("CestaEquipoContrato"), intIndexContratoEquipo)
                        EditarCestaEquipo(True, hfdIdEquipo.Value, EquipoContrato("DescripcionEquipo"), EquipoContrato("Estado"),
                                       strNombreColumnaModificar, "|||" & hfdFechaPlanificacion.Value & "|" & hfdIdOrdenTrabajo.Value & "|" & hfdIdOrdenTrabajoCliente.Value, Session("CestaEquipoContrato"), intIndexContratoEquipo)
                    End If
                    'EditarCestaEquipo(hfdIdEquipo.Value, EquipoContrato("DescripcionEquipo"), EquipoContrato("Estado"),
                    '                  txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text, strNombreColumna, cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "|" & hfdIdContrato.Value & "|" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "|" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text & "|" & hfdIdOrdenTrabajo.Value, Session("CestaEquipoContrato"), intIndexContratoEquipo)
                    EditarCestaEquipo(True, hfdIdEquipo.Value, EquipoContrato("DescripcionEquipo"), EquipoContrato("Estado"),
                                      strNombreColumna, cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedValue & "|" & txtIdContrato.Text & "|" & cboListadoCheckListMantenimientoOrdenTrabajo.SelectedValue & "|" & txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text & "|" & hfdIdOrdenTrabajo.Value & "|" & txtNroOTClienteMantenimientoOrdenTrabajo.Text, Session("CestaEquipoContrato"), intIndexContratoEquipo)
                    MyValidator.ErrorMessage = "Programación registrada con éxito"
                    If hfdOperacionDetalle.Value = "E" Then
                        Dim fila As Int16 = 0
                        For i = 0 To Session("CestaPersonalOrdenTrabajo").Rows.Count - 1
                            'Dim rowIndexDetalle = Session("CestaEquipoContrato").Rows.IndexOf(resultProgramacionContratoSimple(0))
                            If Session("CestaPersonalOrdenTrabajo").Rows(fila)("IdEquipo").ToString.Trim = hfdIdEquipo.Value.Trim And Session("CestaPersonalOrdenTrabajo").Rows(fila)("FechaPlanificacion").ToString.Trim = hfdFechaPlanificacion.Value.Trim Then
                                QuitarCestaPersonalOrdenTrabajo(fila, Session("CestaPersonalOrdenTrabajo"))
                                fila = fila - 1
                            End If
                            fila = fila + 1

                            'AgregarCestaPersonalOrdenTrabajo(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text, hfdIdEquipo.Value, txtIdContrato.Text, Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Personal").ToString.Trim, hfdIdOrdenTrabajo.Value, Session("CestaPersonalOrdenTrabajo"))
                        Next

                    End If

                    For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                        AgregarCestaPersonalOrdenTrabajo(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text, hfdIdEquipo.Value, txtIdContrato.Text, Session("CestaSSPersonal").Rows(i)("Responsable").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim, Session("CestaSSPersonal").Rows(i)("Personal").ToString.Trim, hfdIdOrdenTrabajo.Value, hfdIdOrdenTrabajoCliente.Value, Session("CestaPersonalOrdenTrabajo"))
                    Next
                Next
            End If
            Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
            Me.grdDetalleEquipos.DataBind()
            'gvRow.Cells.Item(4).Text = grdListaProductoVariante.SelectedRow.Cells(1).Text
            'gvRow.Cells.Item(5).Text = grdListaProductoVariante.SelectedRow.Cells(2).Text

            'For Each ProgramacionContrato In Session("Cesta")
            '    clsLogiCestaCatalogoCaracteristica.AgregarCesta(CaracteristicaEquipo("cIdCatalogo"), CaracteristicaEquipo("cIdEquipo"), IIf(lnkEsComponenteOn.Visible = True, "1", "0"), CaracteristicaEquipo("cIdCaracteristica"), CaracteristicaEquipo("vDescripcionCaracteristica"), CaracteristicaEquipo("cIdReferenciaSAPEquipoCaracteristica"), CaracteristicaEquipo("vDescripcionCampoSAPEquipoCaracteristica"), CaracteristicaEquipo("vValorReferencialEquipoCaracteristica"), Session("CestaCaracteristicaEquipoPrincipal"))

            'Next
            'ValidationSummary1.ValidationGroup = "vgrpValidar"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidar"
            'Me.Page.Validators.Add(MyValidator)
            'MyValidator.ErrorMessage = "Personal agregado con éxito."
            ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            MyValidator.IsValid = False
            MyValidator.ID = "ErrorPersonalizado"
            MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            Me.Page.Validators.Add(MyValidator)
            'lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        Catch ex As Exception
            'ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'MyValidator.ErrorMessage = ex.Message
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'Me.Page.Validators.Add(MyValidator)
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
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
                            'Exit Sub
                        End If
                    Next
                End If
            End If

            'JMUG: 05/08/2023: Validación 
            Dim strValidacion As String = ""
            For x = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                If (Session("CestaEquipoContrato").Rows(x)("IdEquipo").ToString.Trim) = hfdIdEquipo.Value.Trim Then
                    Dim strNombreColumna As String = ""
                    For i = 1 To 31
                        If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                            If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                strNombreColumna = ("D" & i)
                                strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                                Exit For
                            End If
                        Else
                            If strNombreColumna = "" Then
                                Throw New Exception("Esta fecha no esta asignada en la programación.")
                            End If
                        End If
                    Next

                End If
            Next
            'If Mid(strValidacion, 1, 3) <> "|||" Then
            Dim strValidaFecha As String() = strValidacion.Split("|")
            If strValidaFecha(0) <> "" Then
                Throw New Exception("Esta fecha ya esta registrada en la programación para este equipo.")
            Else
                AgregarCestaPersonal(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedValue.Trim, UCase(cboPersonalResponsableMantenimientoOrdenTrabajo.SelectedItem.Text).Trim, True, Session("CestaSSPersonal"))
                'lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            End If

            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            lblMensajeMantenimientoOrdenTrabajo.Text = "Personal agregado satisfactoriamente."
            'ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'MyValidator.ErrorMessage = "Personal agregado satisfactoriamente."
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            'ValidationSummary2.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'MyValidator.ErrorMessage = ex.Message
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdPersonalAsignadoMantenimientoOrdenTrabajo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPersonalAsignadoMantenimientoOrdenTrabajo.RowDeleting
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


            'For Each row As GridViewRow In grdDetalleCaracteristicaCatalogoComponente.Rows
            '    If row.RowType = DataControlRowType.DataRow Then
            '        Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("chkRowDetalleCaracteristica"), CheckBox)
            '        If chkRow.Checked Then
            '            For i = 0 To Session("CestaCaracteristicaCatalogoComponente").Rows.Count - 1
            '                If (Session("CestaCaracteristicaCatalogoComponente").Rows(i)("IdCaracteristica").ToString.Trim) = row.Cells(3).Text.ToString.Trim Then
            '                    clsLogiCestaCatalogoCaracteristica.QuitarCesta(i, Session("CestaCaracteristicaCatalogoComponente"))
            '                    Exit For
            '                End If
            '            Next
            '        End If
            '    End If
            'Next
            For i = 0 To Session("CestaSSPersonal").Rows.Count - 1
                If (Session("CestaSSPersonal").Rows(i)("Codigo").ToString.Trim) = grdPersonalAsignadoMantenimientoOrdenTrabajo.Rows(e.RowIndex).Cells(1).Text.ToString.Trim Then
                    QuitarCestaPersonal(i, Session("CestaSSPersonal"))
                    Exit For
                End If
            Next
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataSource = Session("CestaSSPersonal")
            Me.grdPersonalAsignadoMantenimientoOrdenTrabajo.DataBind()
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
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

    Private Sub cboTipoMantenimientoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoMantenimientoMantenimientoOrdenTrabajo.SelectedIndexChanged
        ListarPlantillaCheckListCombo()
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboPersonalAsignadoMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPersonalAsignadoMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub

    Private Sub grdDetalleEquipos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleEquipos.PageIndexChanging
        grdDetalleEquipos.PageIndex = e.NewPageIndex
        Me.grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
        Me.grdDetalleEquipos.DataBind()
        grdDetalleEquipos.SelectedIndex = -1
    End Sub

    Private Sub txtFechaInicioPlanificadaMantenimientoOrdenTrabajo_TextChanged(sender As Object, e As EventArgs) Handles txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.TextChanged
        Try
            'Validar si existe datos para la planificación
            lblMensajeMantenimientoOrdenTrabajo.Text = ""
            Dim strValidacion As String = ""
            For x = 0 To Session("CestaEquipoContrato").Rows.Count - 1
                If (Session("CestaEquipoContrato").Rows(x)("IdEquipo").ToString.Trim) = hfdIdEquipo.Value.Trim Then
                    'QuitarCestaPersonal(i, Session("CestaEquipoContrato"))
                    'Exit For
                    Dim strNombreColumna As String = ""
                    For i = 1 To 31
                        If IsDate(grdDetalleEquipos.Columns(2 + i).HeaderText) Then
                            If CDate(grdDetalleEquipos.Columns(2 + i).HeaderText) = CDate(txtFechaInicioPlanificadaMantenimientoOrdenTrabajo.Text) Then
                                strNombreColumna = ("D" & i)
                                strValidacion = Session("CestaEquipoContrato").Rows(x)(strNombreColumna).ToString.Trim
                                'IIf(IsDate(grdDetalleEquipos.Columns(33).HeaderText)
                                Exit For
                            End If
                        Else
                            If strNombreColumna = "" Then
                                Throw New Exception("Esta fecha no esta asignada en la programación.")
                            End If
                        End If
                    Next

                End If
            Next

            'If Mid(strValidacion, 1, 3) <> "|||" Then
            Dim strValidaFecha As String() = strValidacion.Split("|")
            If strValidaFecha(0) <> "" Then
                Throw New Exception("Esta fecha ya esta registrada en la programación para este equipo.")
            Else
                lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
            End If
            'MyValidator.ErrorMessage = "Personal agregado con éxito."
            'ValidationSummary2.ValidationGroup = "vgrpValidar"
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidar"
            'Me.Page.Validators.Add(MyValidator)
        Catch ex As Exception
            lblMensajeMantenimientoOrdenTrabajo.Text = ex.Message
            'ValidationSummary3.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'MyValidator.ErrorMessage = ex.Message
            'MyValidator.IsValid = False
            'MyValidator.ID = "ErrorPersonalizado"
            'MyValidator.ValidationGroup = "vgrpValidarMantenimientoOrdenTrabajo"
            'Me.Page.Validators.Add(MyValidator)
            lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
        End Try
    End Sub

    Private Sub grdListaSeleccionarEquipo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdListaSeleccionarEquipo.PageIndexChanging
        grdListaSeleccionarEquipo.PageIndex = e.NewPageIndex
        Me.grdListaSeleccionarEquipo.DataSource = EquipoNeg.EquipoListaBusqueda("EQU.bTieneContratoEquipo = '1' AND EQU.cIdCliente = '" & hfdIdAuxiliar.Value & "' AND " & cboFiltroSeleccionarEquipo.SelectedValue,
                                                                 txtBuscarContrato.Text, "0")
        Me.grdListaSeleccionarEquipo.DataBind() 'Recargo el grid.
        grdListaSeleccionarEquipo.SelectedIndex = -1

        lblMensajeSeleccionarEquipo.Text = ""
        lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender.Show()
    End Sub

    Private Sub cboMes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMes.SelectedIndexChanged
        Try
            'fLlenarGrillaPlanificacion()
            CargarCestaEquipos()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub grdLista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLista.PageIndexChanging
        grdLista.PageIndex = e.NewPageIndex
        Dim SubFiltro As String = ""
        'SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
        '            "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
        '            "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
        '            "INNER JOIN GNRL_USUARIO AS USU ON " &
        '            "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
        '            "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

        'Me.grdLista.DataSource = ContratoNeg.OrdenTrabajoListaBusqueda("cIdTipoDocumentoCabeceraOrdenTrabajo = 'OT' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        Me.grdLista.DataBind() 'Recargo el grid.
        grdLista.SelectedIndex = -1
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Try
            'Función para validar si tiene permisos
            If IsNothing(grdLista.SelectedRow) = False Then
                If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                    hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                End If
                If hfdEstado.Value = "0" Or hfdEstado.Value = "" Then
                    Throw New Exception("Este registro se encuentra desactivado.")
                End If
                MyValidator.ErrorMessage = ""
                fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
                FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0664", strOpcionModulo, "CMMS")

                'LO QUITE: JMUG: 14/08/2023 FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0652", strOpcionModulo, Session("IdSistema"))

                Dim dtEstado = EquipoNeg.EquipoGetData("SELECT ISNULL(cEstadoEquipo, 'R') AS cEstadoEquipo FROM LOGI_EQUIPO WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                If dtEstado.Rows.Count > 0 Then
                    If dtEstado.Rows(0).Item(0) = "R" Then 'REGISTRADO
                        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                    ElseIf dtEstado.Rows(0).Item(0) = "T" Then 'TERMINADO
                        EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
                    ElseIf dtEstado.Rows(0).Item(0) = "B" Then 'BLOQUEADO
                        Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, "0")
                        Me.grdLista.DataBind()
                        Throw New Exception("Este registro se encuentra bloqueado y utilizado por otro usuario.")
                    End If
                End If
                hfdOperacion.Value = "E"
                'If BloquearPagina(2) = False Then
                BloquearMantenimiento(False, True, False, True)
                ValidarTexto(True)
                ActivarObjetos(True)
                LlenarData()
                'JMUG: 25/02/2023 hfTab.Value = "tab2"
                pnlCabecera.Visible = False
                pnlContenido.Visible = True
                'pnlComponentes.Visible = True
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

    Private Sub grdLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.grdLista, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = True 'IdNumeroSerie
            e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'FechaEmision
            e.Row.Cells(4).Visible = True 'IdCliente
            e.Row.Cells(5).Visible = False 'RucCliente
            e.Row.Cells(6).Visible = True 'RazonSocialCliente
            e.Row.Cells(7).Visible = True 'Estado
            e.Row.Cells(8).Visible = True 'Estado Registro
        End If
        If e.Row.RowType = ListItemType.Header Then
            e.Row.Cells(0).Visible = True 'IdTipoDocumento
            e.Row.Cells(1).Visible = True 'IdNumeroSerie
            e.Row.Cells(2).Visible = True 'IdNumeroCorrelativo
            e.Row.Cells(3).Visible = True 'FechaEmision
            e.Row.Cells(4).Visible = True 'IdCliente
            e.Row.Cells(5).Visible = False 'RucCliente
            e.Row.Cells(6).Visible = True 'RazonSocialCliente
            e.Row.Cells(7).Visible = True 'Estado
            e.Row.Cells(8).Visible = True 'Estado Registro
        End If
    End Sub

    Private Sub grdLista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdLista.SelectedIndexChanged
        Try
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            MyValidator.ErrorMessage = ""
            'lblMensajeCaracteristica.Text = ""
            'If Session("IdEmpresa") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
            '    Exit Sub
            'End If
            'LimpiarObjetosProducto()
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdContrato.Text = grdLista.SelectedRow.Cells(0).Text + "-" + grdLista.SelectedRow.Cells(1).Text + "-" + grdLista.SelectedRow.Cells(2).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                            'lnkbtnImprimirTarjetaEquipo.Attributes.Add("onclick", "javascript:popupEmitirEquipoDetalleReporte('" & txtIdEquipo.Text & "');")
                            'lnkbtnVerOrdenFabricacion.Attributes.Add("onclick", "javascript:popupEmitirOrdenFabricacionReporte('" & txtIdEquipo.Text & "');")
                            lnkbtnVerContrato.Attributes.Add("onclick", "javascript:popupEmitirContratoReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "');")
                            'btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', '');")
                            'lnkbtnVerProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte();")

                            If MyValidator.ErrorMessage = "" Then
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
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs) Handles btnAtras.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'R' WHERE cIdEquipo = '" & txtIdEquipo.Text & "'")
            'ElseIf dtEstado.Rows(0).Item(0) = "T" Then 'TERMINADO
            '    EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET cEstadoEquipo = 'B', cIdUsuarioUltimaModificacionEquipo = '" & Session("IdUsuario") & "' WHERE cIdEquipo = '" & grdLista.SelectedRow.Cells(0).Text & "'")
            '    'ElseIf dtEstado.Rows(0).Item(0) = "B" Then 'BLOQUEADO
            '    '    Me.grdLista.DataSource = EquipoNeg.EquipoListaBusqueda(cboFiltroEquipo.SelectedValue, txtBuscarEquipo.Text, "0")
            '    '    Me.grdLista.DataBind()
            '    '    Throw New Exception("Este registro se encuentra bloqueado y utilizado por otro usuario.")
            'End If
            FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0666", strOpcionModulo, "CMMS")

            pnlCabecera.Visible = True
            pnlContenido.Visible = False
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

    Private Sub btnSiMensajeDocumento_Click(sender As Object, e As EventArgs) Handles btnSiMensajeDocumento.Click
        Try
            'Función para validar si tiene permisos
            MyValidator.ErrorMessage = ""
            fValidarSesion() 'Se utiliza en el caso cambie de empresa para otra página.
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "0646", strOpcionModulo, Session("IdSistema"))

            ''JMUG: 27/04/2023 EquipoNeg.EquipoGetData("UPDATE LOGI_EQUIPO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")
            'ContratoNeg.EquipoGetData("DELETE LOGI_PLANIFICACIONEQUIPOCONTRATO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")
            'ContratoNeg.EquipoGetData("DELETE LOGI_DETALLECONTRATO SET bEstadoRegistroEquipo = '0' WHERE cIdCatalogo = '" & grdEquipoComponente.SelectedRow.Cells(1).Text & "' AND cIdEnlaceEquipo = '" & txtIdEquipo.Text & "'")
            ContratoNeg.ContratoGetData("DELETE LOGI_PLANIFICACIONEQUIPOCONTRATO " &
                                        "WHERE cIdTipoDocumentoCabeceraContrato + '-' + vIdNumeroSerieCabeceraContrato + '-' + vIdNumeroCorrelativoCabeceraContrato = '" & txtIdContrato.Text & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoDetalleContrato = '" & hfdIdEquipo.Value & "' AND cIdPeriodoMesPlanificacionEquipoContrato = '" & Format(CInt(cboPeriodo.SelectedValue), "0000") & Format(CInt(cboMes.SelectedValue), "00") & "'")
            ContratoNeg.ContratoGetData("DELETE LOGI_DETALLECONTRATO " &
                                        "WHERE cIdTipoDocumentoCabeceraContrato + '-' + vIdNumeroSerieCabeceraContrato + '-' + vIdNumeroCorrelativoCabeceraContrato = '" & txtIdContrato.Text & "' AND cIdEmpresa = '" & Session("IdEmpresa") & "' AND cIdEquipoDetalleContrato = '" & hfdIdEquipo.Value & "'")
            'If Session("CestaCaracteristicaEquipoComponenteFiltrado") Is Nothing Then
            '    Session("CestaCaracteristicaEquipoComponenteFiltrado") = clsLogiCestaCatalogoCaracteristica.CrearCesta
            'Else
            '    clsLogiCestaCatalogoCaracteristica.VaciarCesta(Session("CestaCaracteristicaEquipoComponenteFiltrado"))
            'End If

            'If IsNothing(grdEquipoComponente.SelectedRow) = False Then
            '    clsLogiCestaCatalogo.AgregarCesta(Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdTipoActivo").ToString.Trim,
            '                                      Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdJerarquia").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdSistemaFuncional").ToString.Trim,
            '                                      Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("IdEnlaceCatalogo").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Descripcion").ToString.Trim,
            '                                      Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescripcionAbreviada").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("Estado").ToString.Trim,
            '                                      Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("VidaUtil").ToString.Trim, "",
            '                                      "", Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescAbrevTipoActivo").ToString.Trim,
            '                                      Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("DescAbrevSistemaFuncional").ToString.Trim, Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoGarantia").ToString.Trim,
            '                                      Session("CestaEquipoComponente").Rows(grdEquipoComponente.SelectedRow.RowIndex)("PeriodoMinimo").ToString.Trim,
            '                                   Session("CestaCatalogoComponente"))
            '    clsLogiCestaEquipo.QuitarCesta(grdEquipoComponente.SelectedIndex, Session("CestaEquipoComponente"))
            'End If
            'rowIndexDetalle = 0
            'CargarCestaCaracteristicaEquipoComponenteTemporal() 'JMUG: 20/03/2023

            'grdEquipoComponente.DataSource = Session("CestaEquipoComponente")
            'grdEquipoComponente.DataBind()

            'grdCatalogoComponente.DataSource = Session("CestaCatalogoComponente")
            'grdCatalogoComponente.DataBind()
            QuitarCestaEquipo(intIndexContratoEquipo, Session("CestaEquipoContrato"))
            grdDetalleEquipos.DataSource = Session("CestaEquipoContrato")
            grdDetalleEquipos.DataBind()

            If MyValidator.ErrorMessage = "" Then
                MyValidator.ErrorMessage = "Transacción eliminada con éxito"
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

    Private Sub imgbtnBuscarContrato_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnBuscarContrato.Click
        Dim SubFiltro As String = ""
        'SubFiltro = " AND DOC.cIdTipoDocumentoCabeceraOrdenTrabajo + '-'+ DOC.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + DOC.vIdNumeroCorrelativoCabeceraOrdenTrabajo IN (SELECT RECORDTRA.cIdTipoDocumentoCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroSerieCabeceraOrdenTrabajo + '-' + RECORDTRA.vIdNumeroCorrelativoCabeceraOrdenTrabajo " &
        '            "FROM LOGI_RECURSOSORDENTRABAJO AS RECORDTRA INNER JOIN RRHH_PERSONAL AS PER ON " &
        '            "RECORDTRA.cIdEmpresa = PER.cIdEmpresa And RECORDTRA.cIdPersonal = PER.cIdPersonal " &
        '            "INNER JOIN GNRL_USUARIO AS USU ON " &
        '            "USU.cIdUsuario = '" & Session("IdUsuario") & "' AND USU.vIdNroDocumentoIdentidadUsuario = PER.vNumeroDocumentoPersonal " &
        '            "WHERE RECORDTRA.bResponsableRecursosOrdenTrabajo = '1' AND RECORDTRA.cIdEmpresa = '" & Session("IdEmpresa") & "')"

        Me.grdLista.DataSource = ContratoNeg.ContratoListaBusqueda("cIdTipoDocumentoCabeceraContrato = 'CN' AND " & cboFiltroContrato.SelectedValue, txtBuscarContrato.Text, Session("IdEmpresa"), IIf(Session("IdTipoUsuario") = "A", "", SubFiltro), "1")
        Me.grdLista.DataBind() 'Recargo el grid.
    End Sub

    Private Sub lnkbtnVerProgramacion_Click(sender As Object, e As EventArgs) Handles lnkbtnVerProgramacion.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdContrato.Text = grdLista.SelectedRow.Cells(0).Text + "-" + grdLista.SelectedRow.Cells(1).Text + "-" + grdLista.SelectedRow.Cells(2).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                            txtFechaInicialImprimirProgramacion.Text = Now.ToString("yyyy-MM-dd")
                            txtFechaFinalImprimirProgramacion.Text = Now.ToString("yyyy-MM-dd")
                            CargarCestaEquiposImprimir()
                            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
                            'If MyValidator.ErrorMessage = "" Then
                            '    Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
                            'End If
                        End If
                    Else
                        Throw New Exception("Seleccione un contrato para ver la programación.")
                    End If
                Else
                    Throw New Exception("Seleccione un contrato.")
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

    Private Sub lnkbtnVerContrato_Click(sender As Object, e As EventArgs) Handles lnkbtnVerContrato.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            txtIdContrato.Text = grdLista.SelectedRow.Cells(0).Text + "-" + grdLista.SelectedRow.Cells(1).Text + "-" + grdLista.SelectedRow.Cells(2).Text
                            hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                            'If MyValidator.ErrorMessage = "" Then
                            '    Response.Redirect("frmLogiGaleriaEquipo.aspx?IdEquipo=" & grdLista.SelectedRow.Cells(0).Text & "&IdJerarquia=" & "1")
                            'End If
                        End If
                    Else
                        Throw New Exception("Seleccione un contrato para ver los equipos asignados.")
                    End If
                Else
                    Throw New Exception("Seleccione un contrato.")
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

    Private Sub chkTodosImprimirProgramacion_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodosImprimirProgramacion.CheckedChanged
        If chkTodosImprimirProgramacion.Checked = True Then
            Dim x As Integer = 0
            For Each Equipo In Session("CestaEquipoContratoImprimir").rows
                EditarCestaEquipo(True, Equipo("IdEquipo"), Equipo("DescripcionEquipo"), Equipo("Estado"),
                                    "", "", Session("CestaEquipoContratoImprimir"), x)
                x = x + 1
            Next
            grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            grdDetalleEquipoImprimirProgramacion.DataBind()
            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
            '' Después de marcar las casillas en la página actual, itera a través de las páginas siguientes.
            'For pageIndex As Integer = 1 To grdDetalleEquipoImprimirProgramacion.PageCount - 1
            '    grdDetalleEquipoImprimirProgramacion.PageIndex = pageIndex
            '    grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            '    grdDetalleEquipoImprimirProgramacion.DataBind() ' Vuelve a enlazar los datos para la nueva página.

            '    For Each row As GridViewRow In grdDetalleEquipoImprimirProgramacion.Rows
            '        If row.RowType = DataControlRowType.DataRow Then
            '            Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleEquipo"), CheckBox)
            '            chkRow.Checked = True
            '        End If
            '    Next
            'Next
        Else
            Dim x As Integer = 0
            For Each Equipo In Session("CestaEquipoContratoImprimir").rows
                EditarCestaEquipo(False, Equipo("IdEquipo"), Equipo("DescripcionEquipo"), Equipo("Estado"),
                                    "", "", Session("CestaEquipoContratoImprimir"), x)
                x = x + 1
            Next
            grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            grdDetalleEquipoImprimirProgramacion.DataBind()
            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
            '' Después de marcar las casillas en la página actual, itera a través de las páginas siguientes.
            'For pageIndex As Integer = 1 To grdDetalleEquipoImprimirProgramacion.PageCount - 1
            '    grdDetalleEquipoImprimirProgramacion.PageIndex = pageIndex
            '    grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
            '    grdDetalleEquipoImprimirProgramacion.DataBind() ' Vuelve a enlazar los datos para la nueva página.

            '    For Each row As GridViewRow In grdDetalleEquipoImprimirProgramacion.Rows
            '        If row.RowType = DataControlRowType.DataRow Then
            '            Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRowDetalleEquipo"), CheckBox)
            '            chkRow.Checked = False
            '        End If
            '    Next
            'Next
        End If

        'Obtener la cadena de equipos
        Dim strEquipo As String = ""
        For Each Equipo In Session("CestaEquipoContratoImprimir").rows
            If Equipo("Seleccionar") = True Then
                strEquipo = strEquipo + Equipo("IdEquipo") & ","
                'strEquipo = strEquipo + ","
            End If
        Next
        If strEquipo <> "" Then
            strEquipo = If(InStrRev(strEquipo, ",") = Len(strEquipo), Mid(strEquipo, 1, InStrRev(strEquipo, ",") - 1), strEquipo)
        End If
        'btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('');")
        btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaInicialImprimirProgramacion.Text)) & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaFinalImprimirProgramacion.Text)) & "', '" & strEquipo & "');")
    End Sub

    Private Sub grdDetalleEquipoImprimirProgramacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDetalleEquipoImprimirProgramacion.PageIndexChanging
        grdDetalleEquipoImprimirProgramacion.PageIndex = e.NewPageIndex
        grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
        grdDetalleEquipoImprimirProgramacion.DataBind()
    End Sub

    Private Sub btnAceptarImprimirProgramacion_Click(sender As Object, e As EventArgs) Handles btnAceptarImprimirProgramacion.Click
        Try
            If grdLista IsNot Nothing Then
                If grdLista.Rows.Count > 0 Then
                    If IsNothing(grdLista.SelectedRow) = False Then
                        If IsReference(grdLista.SelectedRow.Cells(1).Text) = True Then
                            'txtIdContrato.Text = grdLista.SelectedRow.Cells(0).Text + "-" + grdLista.SelectedRow.Cells(1).Text + "-" + grdLista.SelectedRow.Cells(2).Text
                            'hfdEstado.Value = IIf(Server.HtmlDecode(grdLista.SelectedRow.Cells(8).Text) = "True", "1", "0")
                            'txtFechaInicialImprimirProgramacion.Text = Now.ToString("yyyy-MM-dd")
                            'txtFechaFinalImprimirProgramacion.Text = Now.ToString("yyyy-MM-dd")
                            'CargarCestaEquiposImprimir()
                            'Dim strEquipo As String = ""
                            'Dim x As Integer = 0
                            'For Each Equipo In Session("CestaEquipoContratoImprimir").rows
                            '    'EditarCestaEquipo(False, Equipo("IdEquipo"), Equipo("DescripcionEquipo"), Equipo("Estado"),
                            '    '    "", "", Session("CestaEquipoContratoImprimir"), x)
                            '    If Equipo("Seleccionar") = True Then
                            '        strEquipo = strEquipo & "'" & Equipo("IdEquipo") & "'"
                            '        If x < Session("CestaEquipoContratoImprimir").rows.count - 1 Then
                            '            strEquipo = strEquipo + ","
                            '        End If
                            '    End If
                            '    x = x + 1
                            'Next
                            'btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "');")
                            lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
                        End If
                    Else
                        Throw New Exception("Seleccione un contrato para ver la programación.")
                    End If
                Else
                    Throw New Exception("Seleccione un contrato.")
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

    Protected Sub chkRowDetalleEquipo_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Obtener la fila que contiene el CheckBox que se activó/desactivó
        Dim row As GridViewRow = DirectCast(DirectCast(sender, CheckBox).NamingContainer, GridViewRow)

        Dim checkBox As CheckBox = DirectCast(sender, CheckBox)
        'If checkBox.Checked Then
        '    ' El CheckBox está marcado (true)
        '    ' Realiza las acciones que desees cuando el CheckBox está marcado
        Dim FilaActual As Int16
        FilaActual = row.RowIndex - (grdDetalleEquipoImprimirProgramacion.Rows.Count * (grdDetalleEquipoImprimirProgramacion.PageIndex))
        EditarCestaEquipo(checkBox.Checked, Session("CestaEquipoContratoImprimir").Rows(FilaActual)("IdEquipo"), Session("CestaEquipoContratoImprimir").Rows(FilaActual)("DescripcionEquipo"), Session("CestaEquipoContratoImprimir").Rows(FilaActual)("Estado"),
                                    "", "", Session("CestaEquipoContratoImprimir"), FilaActual)

        grdDetalleEquipoImprimirProgramacion.DataSource = Session("CestaEquipoContratoImprimir")
        grdDetalleEquipoImprimirProgramacion.DataBind()

        Dim strEquipo As String = ""
        For Each Equipo In Session("CestaEquipoContratoImprimir").rows
            If Equipo("Seleccionar") = True Then
                strEquipo = strEquipo + Equipo("IdEquipo") & ","
            End If
        Next
        strEquipo = If(InStrRev(strEquipo, ",") = Len(strEquipo), Mid(strEquipo, 1, InStrRev(strEquipo, ",") - 1), strEquipo)
        'btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', " & strEquipo & ");")
        'lnkbtnVerProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte();")
        btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaInicialImprimirProgramacion.Text)) & "', '" & String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtFechaFinalImprimirProgramacion.Text)) & "', '" & strEquipo & "');")
        lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender.Show()
    End Sub

    'Protected Sub grdDetalleEquipoImprimirProgramacion_RowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        If e.CommandName = "Activar" Then
    '            Dim strEquipo As String = ""
    '            Dim x As Integer = 0
    '            For Each Equipo In Session("CestaEquipoContratoImprimir").rows
    '                'EditarCestaEquipo(False, Equipo("IdEquipo"), Equipo("DescripcionEquipo"), Equipo("Estado"),
    '                '    "", "", Session("CestaEquipoContratoImprimir"), x)
    '                If Equipo("Seleccionar") = True Then
    '                    strEquipo = strEquipo & "'" & Equipo("IdEquipo") & "'"
    '                    If x < Session("CestaEquipoContratoImprimir").rows.count - 1 Then
    '                        strEquipo = strEquipo + ","
    '                    End If
    '                End If
    '                x = x + 1
    '            Next
    '            btnAceptarImprimirProgramacion.Attributes.Add("onclick", "javascript:popupEmitirContratoProgramacionReporte('" & grdLista.SelectedRow.Cells(0).Text & "', '" & grdLista.SelectedRow.Cells(1).Text & "', '" & grdLista.SelectedRow.Cells(2).Text & "', '" & strEquipo & "');")
    '        End If
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = "Registro activado con éxito"
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    Catch ex As Exception
    '        ValidationSummary1.ValidationGroup = "vgrpValidarBusqueda"
    '        MyValidator.ErrorMessage = ex.Message
    '        MyValidator.IsValid = False
    '        MyValidator.ID = "ErrorPersonalizado"
    '        MyValidator.ValidationGroup = "vgrpValidarBusqueda"
    '        Me.Page.Validators.Add(MyValidator)
    '    End Try
    'End Sub

    'Protected Sub RowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    Try
    '        If e.Row.RowType = DataControlRowType.DataRow AndAlso grdListaSeleccionarEquipo.EditIndex = e.Row.RowIndex Then
    '            Dim lnkbtnDia As LinkButton = DirectCast(e.Row.FindControl("lnkbtnD1"), LinkButton)
    '            'Dim cboProyecto As DropDownList = DirectCast(e.Row.FindControl("cboProyecto"), DropDownList)
    '            'Dim cboSistema As DropDownList = DirectCast(e.Row.FindControl("cboSistema"), DropDownList)

    '            'Dim dtProyecto = DetalleSistemaTrabajoNeg.DetalleSistemaTrabajoGetData("SELECT * FROM [SBO_REMICSADRILLING]..[OUBR]")
    '            'Dim dtSistemaTrabajo = DetalleSistemaTrabajoNeg.DetalleSistemaTrabajoGetData("SELECT * FROM perSISTEMATRABAJO")

    '            'cboProyecto.DataTextField = "Name"
    '            'cboProyecto.DataValueField = "Code"
    '            'cboProyecto.DataSource = dtProyecto
    '            'cboProyecto.DataBind()

    '            'cboSistema.DataTextField = "vDescripcionAbreviadaSistemaTrabajo"
    '            'cboSistema.DataValueField = "cIdSistemaTrabajo"
    '            'cboSistema.DataSource = dtSistemaTrabajo
    '            'cboSistema.DataBind()

    '            'If TryCast(e.Row.FindControl("lblIdProyecto"), Label).Text <> "" Then
    '            '    cboProyecto.Items.FindByValue(TryCast(e.Row.FindControl("lblIdProyecto"), Label).Text).Selected = True
    '            'End If
    '            'If TryCast(e.Row.FindControl("lblIdSistemaTrabajo"), Label).Text <> "" Then
    '            '    cboSistema.Items.FindByValue(TryCast(e.Row.FindControl("lblIdSistemaTrabajo"), Label).Text).Selected = True
    '            'End If
    '        End If
    '    Catch ex As Exception
    '        lblMensajeSeleccionarEquipo.Text = ex.Message
    '    End Try
    'End Sub
    Private Sub cboListadoCheckListMantenimientoOrdenTrabajo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboListadoCheckListMantenimientoOrdenTrabajo.SelectedIndexChanged
        lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender.Show()
    End Sub
End Class