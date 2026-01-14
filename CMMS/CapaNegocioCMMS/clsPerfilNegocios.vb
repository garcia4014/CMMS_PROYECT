Imports CapaDatosCMMS

Public Class clsPerfilNegocios
    Dim PerfilMet As New clsPerfilMetodos
    'Dim UsuarioMet As New clsUsuarioMetodos

    Public Function PerfilObtenerPerfil(ByVal IdUsuario As String) As List(Of GNRL_PERFIL)
        Return PerfilMet.PerfilObtenerPerfil(IdUsuario)
    End Function

    Public Function PerfilListarCombo() As List(Of GNRL_PERFIL)
        Return PerfilMet.PerfilListarCombo
    End Function

    Public Function PerfilListarPorId(ByVal IdPerfil As String) As GNRL_PERFIL
        If PerfilMet.PerfilExiste(IdPerfil) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Perfil con el id " & IdPerfil & " no Existe!!!")
        Else
            Return PerfilMet.PerfilListarPorId(IdPerfil)
        End If
    End Function

    Public Function PerfilListaBusqueda(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_PERFIL)
        Return PerfilMet.PerfilListaGrid(Filtro, Buscar)
    End Function

    Public Function PerfilInserta(ByVal Perfil As GNRL_PERFIL) As Int32
        If PerfilMet.PerfilExiste(Perfil.cIdPerfil) = True Then
            Throw New Exception("El Perfil con el id " & Perfil.cIdPerfil & " ya existe!")
        Else
            Return PerfilMet.PerfilInserta(Perfil)
        End If
    End Function

    Public Function PerfilEdita(ByVal Perfil As GNRL_PERFIL) As Int32
        If PerfilMet.PerfilExiste(Perfil.cIdPerfil) = False Then
            Throw New Exception("El Perfil con el id " & Perfil.cIdPerfil & " no existe!")
        Else
            Return PerfilMet.PerfilEdita(Perfil)
        End If
    End Function

    Public Function PerfilElimina(ByVal Perfil As GNRL_PERFIL) As Int32
        If PerfilMet.PerfilExiste(Perfil.cIdPerfil) = False Then
            Throw New Exception("El Perfil con el id " & Perfil.cIdPerfil & " no existe!")
        Else
            Return PerfilMet.PerfilElimina(Perfil)
        End If
    End Function
End Class
