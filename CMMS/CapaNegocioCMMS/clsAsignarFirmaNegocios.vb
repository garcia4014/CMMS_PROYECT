Imports CapaDatosCMMS
Imports CapaDatosCMMS.clsAsignarFirmaMetodos

Public Class clsAsignarFirmaNegocios
    Dim AsignarFirmaMet As New clsAsignarFirmaMetodos

    Public Function AsignarFirmaGetData(strQuery As String) As DataTable
        Return AsignarFirmaMet.AsignarFirmaGetData(strQuery)
    End Function

    Public Function AsignarFirmaInserta(ByVal AsignarFirma As RRHH_ASIGNARFIRMA) As Int32
        If AsignarFirmaMet.AsignarFirmaExiste(AsignarFirma.cIdUsuario, AsignarFirma.nItemAsignarFirma, AsignarFirma.cIdEmpresa) = True Then
            Throw New Exception("El usuario con el id " & AsignarFirma.cIdUsuario & " ya existe!")
        Else
            Return AsignarFirmaMet.AsignarFirmaInserta(AsignarFirma)
        End If
    End Function

    Public Function AsignarFirmaEdita(ByVal AsignarFirma As RRHH_ASIGNARFIRMA) As Int32
        If AsignarFirmaMet.AsignarFirmaExiste(AsignarFirma.cIdUsuario, AsignarFirma.nItemAsignarFirma, AsignarFirma.cIdEmpresa) = False Then
            Throw New Exception("El usuario con el id " & AsignarFirma.cIdUsuario & " no existe!")
        Else
            Return AsignarFirmaMet.AsignarFirmaEdita(AsignarFirma)
        End If
    End Function

    Public Function AsignarFirmaElimina(ByVal AsignarFirma As RRHH_ASIGNARFIRMA)
        If AsignarFirmaMet.AsignarFirmaExiste(AsignarFirma.cIdUsuario, AsignarFirma.nItemAsignarFirma, AsignarFirma.cIdEmpresa) = False Then
            Throw New Exception("El usuario con el id " & AsignarFirma.cIdUsuario & " no existe!")
        Else
            Return AsignarFirmaMet.AsignarFirmaElimina(AsignarFirma)
        End If
    End Function

    Public Function AsignarFirmaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of FirmaCesta)
        Return AsignarFirmaMet.AsignarFirmaListaGrid(Filtro, Buscar, IdEmpresa, Estado)
    End Function

    Public Function AsignarFirmaListarPorId(ByVal IdUsuario As String, ByVal IdItem As Integer, ByVal IdEmpresa As String) As RRHH_ASIGNARFIRMA
        If AsignarFirmaMet.AsignarFirmaExiste(IdUsuario, IdItem, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El usuario con el id " & IdUsuario & " no Existe!!!")
        Else
            Return AsignarFirmaMet.AsignarFirmaListarPorId(IdUsuario, IdItem, IdEmpresa)
        End If
    End Function

    'Public Function AsignarFirmaListarCombo() As List(Of RRHH_ASIGNARFIRMA)
    '    Return AsignarFirmaMet.AsignarFirmaListarCombo
    'End Function
End Class