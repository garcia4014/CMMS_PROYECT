'Imports System.Drawing
Imports CapaNegocioCMMS

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim GaleriaNeg As New clsGaleriaEquipoNegocios
        Me.lstGaleriaEquipo.DataSource = GaleriaNeg.GaleriaEquipoListaBusqueda("cIdEquipo", "01EQ00000011872", "1")
        Me.lstGaleriaEquipo.DataBind()
        'lnk_mostrarPanelGaleriaEquipo_ModalPopupExtender.Show()
    End Sub


End Class