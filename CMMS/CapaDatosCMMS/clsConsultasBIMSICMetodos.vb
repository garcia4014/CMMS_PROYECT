Imports MySql.Data.MySqlClient

Public Class clsConsultasBIMSICMetodos
    Public Function ConsultaBIMSICGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        Dim constr As String = My.Settings.BIMSConnectionString
        Dim cnn As New MySqlConnection
        cnn.ConnectionString = constr
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter(strQuery, cnn)
        da.Fill(ds, "Tabla_Temporal")
        Return ds.Tables("Tabla_Temporal")
        'Using con As New SqlConnection(constr)
        '    Using cmd As New SqlCommand(strQuery)
        '        Using sda As New SqlDataAdapter()
        '            cmd.CommandType = CommandType.Text
        '            cmd.Connection = con
        '            sda.SelectCommand = cmd
        '            sda.Fill(dt)
        '        End Using
        '    End Using
        '    Return dt
        'End Using
    End Function

    Public Function InsertarBIMSICGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        Dim constr As String = My.Settings.CMMSConnectionString
        Dim cnn As New MySqlConnection
        cnn.ConnectionString = constr
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter(strQuery, cnn)
        da.Fill(dt)
        Return dt
    End Function
End Class
