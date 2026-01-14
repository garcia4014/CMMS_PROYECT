Imports CapaDatosCMMS
Public Class clsEstadoCivilNegocios
    Dim EstCivMet As New clsEstadoCivilMetodos

    Public Function EstadoCivilListarCombo() As List(Of GNRL_ESTADOCIVIL)
        Return EstCivMet.EstadoCivilListarCombo
    End Function

    Public Function EstadoCivilListarPorId(ByVal IdEstadoCivil As String) As GNRL_ESTADOCIVIL
        If EstCivMet.EstadoCivilExiste(IdEstadoCivil) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El estado civil con el id " & IdEstadoCivil & " no Existe!!!")
        Else
            Return EstCivMet.EstadoCivilListarPorId(IdEstadoCivil)
        End If
    End Function

    Public Function EstadoCivilListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_ESTADOCIVIL)
        Return EstCivMet.EstadoCivilListaGrid(Filtro, Buscar)
    End Function

    Public Function EstadoCivilInserta(ByVal EstadoCivil As GNRL_ESTADOCIVIL) As Int32
        If EstCivMet.EstadoCivilExiste(EstadoCivil.cIdEstadoCivil) = True Then
            Throw New Exception("El estado civil con el id " & EstadoCivil.cIdEstadoCivil & " ya existe!")
        Else
            Return EstCivMet.EstadoCivilInserta(EstadoCivil)
        End If
    End Function

    Public Function EstadoCivilEdita(ByVal EstadoCivil As GNRL_ESTADOCIVIL) As Int32
        If EstCivMet.EstadoCivilExiste(EstadoCivil.cIdEstadoCivil) = False Then
            Throw New Exception("El estado civil con el id " & EstadoCivil.cIdEstadoCivil & " no existe!")
        Else
            Return EstCivMet.EstadoCivilEdita(EstadoCivil)
        End If
    End Function

    Public Function EstadoCivilElimina(ByVal EstadoCivil As GNRL_ESTADOCIVIL) As Int32
        If EstCivMet.EstadoCivilExiste(EstadoCivil.cIdEstadoCivil) = False Then
            Throw New Exception("El estado civil con el id " & EstadoCivil.cIdEstadoCivil & " no existe!")
        Else
            Return EstCivMet.EstadoCivilElimina(EstadoCivil)
        End If
    End Function
End Class