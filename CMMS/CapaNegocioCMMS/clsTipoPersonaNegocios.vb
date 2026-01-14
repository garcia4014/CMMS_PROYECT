Imports CapaDatosCMMS
Public Class clsTipoPersonaNegocios
    Dim TipoPersonaMet As New clsTipoPersonaMetodos

    Public Function TipoPersonaListarCombo() As List(Of GNRL_TIPOPERSONA)
        Return TipoPersonaMet.TipoPersonaListarCombo
    End Function

    Public Function TipoPersonaListarPorId(ByVal IdTipoPersona As String) As GNRL_TIPOPERSONA
        If TipoPersonaMet.TipoPersonaExiste(IdTipoPersona) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Tipo de Persona con el id " & IdTipoPersona & " no Existe!!!")
        Else
            Return TipoPersonaMet.TipoPersonaListarPorId(IdTipoPersona)
        End If
    End Function

    Public Function TipoPersonaListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_TIPOPERSONA)
        Return TipoPersonaMet.TipoPersonaListaGrid(Filtro, Buscar)
    End Function

    Public Function TipoPersonaInserta(ByVal TipoPersona As GNRL_TIPOPERSONA) As Int32
        If TipoPersonaMet.TipoPersonaExiste(TipoPersona.cIdTipoPersona) = True Then
            Throw New Exception("El Tipo de Persona con el id " & TipoPersona.cIdTipoPersona & " ya existe!")
        Else
            Return TipoPersonaMet.TipoPersonaInserta(TipoPersona)
        End If
    End Function

    Public Function TipoPersonaEdita(ByVal TipoPersona As GNRL_TIPOPERSONA) As Int32
        If TipoPersonaMet.TipoPersonaExiste(TipoPersona.cIdTipoPersona) = False Then
            Throw New Exception("El Tipo de Persona con el id " & TipoPersona.cIdTipoPersona & " no existe!")
        Else
            Return TipoPersonaMet.TipoPersonaEdita(TipoPersona)
        End If
    End Function

    Public Function TipoPersonaElimina(ByVal TipoPersona As GNRL_TIPOPERSONA) As Int32
        If TipoPersonaMet.TipoPersonaExiste(TipoPersona.cIdTipoPersona) = False Then
            Throw New Exception("El Tipo de Persona con el id " & TipoPersona.cIdTipoPersona & " no existe!")
        Else
            Return TipoPersonaMet.TipoPersonaElimina(TipoPersona)
        End If
    End Function
End Class
