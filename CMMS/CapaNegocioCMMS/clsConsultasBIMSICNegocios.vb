Imports CapaDatosCMMS

Public Class clsConsultasBIMSICNegocios
    Dim ConsultaMet As New clsConsultasBIMSICMetodos

    Public Function ConsultaBIMSICGetData(strQuery As String) As DataTable
        Return ConsultaMet.ConsultaBIMSICGetData(strQuery)
    End Function

    Public Function InsertarBIMSICGetData(strQuery As String) As DataTable
        Return ConsultaMet.InsertarBIMSICGetData(strQuery)
    End Function
End Class
