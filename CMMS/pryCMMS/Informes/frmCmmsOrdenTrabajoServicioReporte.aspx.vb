Imports CapaNegocioCMMS
Imports CrystalDecisions.Shared
Public Class frmCmmsOrdenTrabajoServicioReporte
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
            Me.CrystalReportSource1.ReportDocument.FileName = Request.ServerVariables("APPL_PHYSICAL_PATH") & "Informes\crpCmmsOrdenTrabajoServicio.rpt"

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

        Catch ex As Exception

        End Try
    End Sub

End Class