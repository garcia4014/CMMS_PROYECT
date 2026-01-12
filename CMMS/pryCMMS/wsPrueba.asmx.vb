Imports System.ComponentModel
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class wsPrueba
    Inherits System.Web.Services.WebService

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Function GetCompletionListCaracteristica(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim cn As New SqlClient.SqlConnection()
        Dim ds As New DataSet
        Dim dt As New DataTable

        'Dim FuncionesNeg As New clsFuncionesNegocios
        'Dim ConexionNeg As New clsConexionNegocios

        Dim strCn As String = "Data Source=AQP-PF3YT6VW;Initial Catalog=CMMS;Persist Security Info=True;User ID=sa;Password=M1F4m1l14--1979--C0nam0rt0d0s3pu3d3"
        cn.ConnectionString = strCn
        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = cn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = ("SELECT vDescripcionCaracteristica, cIdCaracteristica " &
                           "FROM LOGI_CARACTERISTICA WHERE vDescripcionCaracteristica LIKE '%" & UCase(prefixText) & "%' " & contextKey & "")
        Try
            cn.Open()
            cmd.ExecuteNonQuery()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds)
        Catch ex As Exception
        Finally
            cn.Close()
        End Try

        dt = ds.Tables(0)

        Dim txtItems As New List(Of String)
        Dim dbValues As String

        For Each row As DataRow In dt.Rows
            dbValues = row(0).ToString()
            dbValues = StrConv(dbValues, VbStrConv.ProperCase)
            txtItems.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dbValues, row(1).ToString().Trim))
        Next
        Return txtItems.ToArray()
    End Function
End Class