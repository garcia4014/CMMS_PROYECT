Public Class clsArticuloSAPMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function ArticuloSAPListarCombo(ByVal bEstado As Boolean) As List(Of LOGI_ARTICULOSAP)
        Dim Consulta = Data.PA_LOGI_MNT_ARTICULOSAP("SQL_NONE", "SELECT * FROM LOGI_ARTICULOSAP WHERE bEstadoRegistroArticuloSAP = '" & bEstado & "'", "", "", "", "0", "")

        Dim Coleccion As New List(Of LOGI_ARTICULOSAP)
        For Each ArticuloSAPEquipo In Consulta
            Dim ArticuloSAP As New LOGI_ARTICULOSAP
            ArticuloSAP.vIdArticuloSAP = ArticuloSAPEquipo.vIdArticuloSAP
            ArticuloSAP.vDescripcionArticuloSAP = ArticuloSAPEquipo.vIdArticuloSAP & " " & ArticuloSAPEquipo.vDescripcionArticuloSAP
            Coleccion.Add(ArticuloSAP)
        Next
        Return Coleccion
    End Function
End Class
