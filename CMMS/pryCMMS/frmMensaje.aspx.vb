Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class frmMensaje
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Session("TipoAmbiente") = "0") Then
            lblAmbiente.Text = "(Ambiente de Prueba)"
        Else
            lblAmbiente.Text = ""
        End If

        Dim strMsg As String
        strMsg = Request.QueryString("Msg")
        If strMsg = "1" Then
            lblMensaje.Text = "Se ha finalizado la sesión satisfactoriamente." & vbCrLf & "  Gracias por su visita."
        End If
        If strMsg = "2" Then
            lblMensaje.Text = "Su sesión ha caducado, ingrese de nuevo por favor." & vbCrLf & "  Gracias por su visita."
        End If
        If strMsg = "3" Then
            lblMensaje.Text = "No se ha ingresado al sistema correctamente." & vbCrLf & "  Favor de acceder con su cuenta."
        End If
        Session.Abandon()
        If Not Page.ClientScript.IsStartupScriptRegistered("iniThis") Then
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "initThis", "initThis();", True)
        End If
    End Sub
End Class