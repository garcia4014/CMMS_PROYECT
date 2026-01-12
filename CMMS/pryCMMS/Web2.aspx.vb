Imports CapaNegocioCMMS
Imports CapaDatosCMMS

Public Class Web2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Simular la carga de datos en el GridView
            Dim dt As New DataTable()
            dt.Columns.Add("Column1")
            dt.Columns.Add("Column2")
            dt.Columns.Add("Column3")

            For i As Integer = 1 To 100
                dt.Rows.Add("Fila " & i, "Datos " & i, "Más datos " & i)
            Next

            yourGridView.DataSource = dt
            yourGridView.DataBind()
        End If
    End Sub

    Protected Sub btnActualizar_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Aquí puedes realizar la actualización de datos
        ' Por ejemplo, volver a enlazar el GridView
        Page_Load(sender, e)
    End Sub
End Class