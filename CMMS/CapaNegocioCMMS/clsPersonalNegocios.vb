Imports CapaDatosCMMS

Public Class clsPersonalNegocios
    Dim PersonalMet As New clsPersonalMetodos

    Public Function PersonalListarCombo(ByVal Optional IdUnidadTrabajo As String = "*") As List(Of RRHH_PERSONAL)
        Return PersonalMet.PersonalListarCombo(IdUnidadTrabajo)
    End Function

    'Public Function PersonalListarPorId(ByVal IdPersonal As String,
    '                                    ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As RRHH_PERSONAL
    Public Function PersonalListarPorId(ByVal IdPersonal As String,
                                         ByVal IdEmpresa As String) As RRHH_PERSONAL
        If PersonalMet.PersonalExiste(IdPersonal, IdEmpresa) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El personal con el id " & IdPersonal & " no Existe!!!")
        Else
            Return PersonalMet.PersonalListarPorId(IdPersonal, IdEmpresa)
        End If
    End Function

    'Public Function PersonalListaBusqueda(ByVal Filtro As String, ByVal Buscar As String,
    '                                  ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_RRHH_PERSONAL)
    Public Function PersonalListaBusqueda(ByVal Filtro As String, ByVal Buscar As String,
                                    ByVal IdEmpresa As String) As List(Of VI_RRHH_PERSONAL)
        Return PersonalMet.PersonalListaGrid(Filtro, Buscar, IdEmpresa)
    End Function

    Public Function PersonalInserta(ByVal Personal As RRHH_PERSONAL) As Int32
        If PersonalMet.PersonalExiste(Personal.cIdPersonal, Personal.cIdEmpresa) = True Then
            Throw New Exception("El personal con el id " & Personal.cIdPersonal & " ya existe!")
        Else
            Return PersonalMet.PersonalInserta(Personal)
        End If
    End Function

    Public Function PersonalEdita(ByVal Personal As RRHH_PERSONAL) As Int32
        If PersonalMet.PersonalExiste(Personal.cIdPersonal, Personal.cIdEmpresa) = False Then
            Throw New Exception("El personal con el id " & Personal.cIdPersonal & " no existe!")
        Else
            Return PersonalMet.PersonalEdita(Personal)
        End If
    End Function

    Public Function PersonalElimina(ByVal Personal As RRHH_PERSONAL) As Int32
        If PersonalMet.PersonalExiste(Personal.cIdPersonal, Personal.cIdEmpresa) = False Then
            Throw New Exception("El personal con el id " & Personal.cIdPersonal & " no existe!")
        Else
            Return PersonalMet.PersonalElimina(Personal)
        End If
    End Function
End Class