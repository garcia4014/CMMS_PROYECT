Imports CapaNegocioCMMS
Imports CapaDatosCMMS
'----------------------------------
'Inicio: Conectividad para los reportes.
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
'Fin: Conectividad para los reportes.

Public Class frmCmmsOrdenTrabajoCheckListClienteReporte
    Inherits System.Web.UI.Page
    Dim FuncionesNeg As New clsFuncionesNegocios
    Shared strOpcionModulo As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Session("IdConfEmpresa") = "" Then
            '    Response.Redirect("~/frmMensaje.aspx?Msg=" & "3", False)
            '    Exit Sub
            If Session("IdUsuario") = "" Then
                Response.Redirect("~/frmMensaje.aspx?Msg=" & "2", False)
                Exit Sub
            End If

            'strOpcionModulo = "006" 'Modulo de Emisión de Registro de Ventas

            'Función para validar si tiene permisos
            'FuncionesNeg.ValidaPerfil(Session("IdUsuario"), Session("IdPerfil"), "179", strOpcionModulo, Session("IdSistema"), Session("IdArea"))
            Me.CrystalReportSource1.ReportDocument.FileName = Request.ServerVariables("APPL_PHYSICAL_PATH") & "Informes\crpCmmsOrdenTrabajoCheckListCliente.rpt"

            Dim Server As String = FuncionesNeg.Desencriptar(Session("Server"))
            Dim Database As String = FuncionesNeg.Desencriptar(Session("Database"))
            Dim User As String = FuncionesNeg.Desencriptar(Session("DBUser"))
            Dim Password As String = FuncionesNeg.Desencriptar(Session("DBPassword"))

            'The following set of code is necessary to dynamically change the SERVER and DATABASE.
            Dim ci As CrystalDecisions.Shared.ConnectionInfo = New ConnectionInfo()
            ci.Type = ConnectionInfoType.CRQE
            ci.ServerName = Server
            ci.DatabaseName = Database
            ci.UserID = User
            ci.Password = Password

            Dim tli As TableLogOnInfo = New TableLogOnInfo()
            tli.ConnectionInfo = ci
            Dim t As CrystalDecisions.CrystalReports.Engine.Table
            For Each t In CrystalReportSource1.ReportDocument.Database.Tables
                t.ApplyLogOnInfo(tli)
            Next t

            Me.CrystalReportSource1.ReportDocument.SetParameterValue("@cIdEmpresa", Session("IdEmpresa"))
            Me.CrystalReportSource1.ReportDocument.SetParameterValue("@cIdTipoDocumento", Request.QueryString("TipDoc"))
            Me.CrystalReportSource1.ReportDocument.SetParameterValue("@vIdNumeroSerie", Request.QueryString("NroSerie"))
            Me.CrystalReportSource1.ReportDocument.SetParameterValue("@vIdNumeroCorrelativo", Request.QueryString("NroDoc"))
            Me.CrystalReportSource1.ReportDocument.SetParameterValue("@vURL", Request.ServerVariables("APPL_PHYSICAL_PATH"))

            'Inicio: 23/07/2020 - Esto lo cree para que genere en HTML y se muestre el cuadro de dialogo de imprimir, tanto en chrome como en otros navegadores que no sean IE
            'Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
            'Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
            'exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat

            'exportOpts.ExportFormatOptions = pdfOpts
            'CrystalReportSource1.ReportDocument.ExportToHttpResponse(exportOpts, Response, False, "PreCuenta")
            'Final: 23/07/2020 - Esto lo cree para que genere en HTML y se muestre el cuadro de dialogo de imprimir, tanto en chrome como en otros navegadores que no sean IE
        Catch ex As Exception
            'Me.CrystalReportSource1.ReportDocument.FileName = Request.ServerVariables("APPL_PHYSICAL_PATH") & "Informes\crpVtasMensaje.rpt"
            'Dim Server As String = FuncionesNeg.Desencriptar(Session("Server"))
            'Dim Database As String = FuncionesNeg.Desencriptar(Session("Database"))
            'Dim User As String = FuncionesNeg.Desencriptar(Session("DBUser"))
            'Dim Password As String = FuncionesNeg.Desencriptar(Session("DBPassword"))

            ''The following set of code is necessary to dynamically change the SERVER and DATABASE.
            'Dim ci As CrystalDecisions.Shared.ConnectionInfo = New ConnectionInfo()
            'ci.Type = ConnectionInfoType.CRQE
            'ci.ServerName = Server
            'ci.DatabaseName = Database
            'ci.UserID = User
            'ci.Password = Password
            'Dim tli As TableLogOnInfo = New TableLogOnInfo()
            'tli.ConnectionInfo = ci
            'Dim t As CrystalDecisions.CrystalReports.Engine.Table
            'For Each t In CrystalReportSource1.ReportDocument.Database.Tables
            '    t.ApplyLogOnInfo(tli)
            'Next t
        End Try
    End Sub
End Class