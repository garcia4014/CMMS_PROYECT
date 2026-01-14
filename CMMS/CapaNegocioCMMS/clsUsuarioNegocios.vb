Imports CapaDatosCMMS

Public Class clsUsuarioNegocios
    Dim UsuarioMet As New clsUsuarioMetodos

    Public Function UsuarioGetData(strQuery As String) As DataTable
        Return UsuarioMet.UsuarioGetData(strQuery)
    End Function

    Public Function UsuarioInserta(ByVal Usuario As GNRL_USUARIO) As Int32
        If UsuarioMet.UsuarioExiste(Usuario.cIdUsuario) = True Then
            Throw New Exception("El usuario con el id " & Usuario.cIdUsuario & " ya existe!")
        Else
            Return UsuarioMet.UsuarioInserta(Usuario)
        End If
    End Function

    Public Function UsuarioEdita(ByVal Usuario As GNRL_USUARIO) As Int32
        If UsuarioMet.UsuarioExiste(Usuario.cIdUsuario) = False Then
            Throw New Exception("El usuario con el id " & Usuario.cIdUsuario & " no existe!")
        Else
            Return UsuarioMet.UsuarioEdita(Usuario)
        End If
    End Function

    Public Function UsuarioElimina(ByVal Usuario As GNRL_USUARIO)
        If UsuarioMet.UsuarioExiste(Usuario.cIdUsuario) = False Then
            Throw New Exception("El usuario con el id " & Usuario.cIdUsuario & " no existe!")
        Else
            Return UsuarioMet.UsuarioElimina(Usuario)
        End If
    End Function

    'Public Function CargoListar() As List(Of GNRL_PERFIL)
    '    Return UsuarioMet.CategoriaListarCombo
    'End Function

    'Public Function UsuarioListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As List(Of VI_GNRL_USUARIO)
    Public Function UsuarioListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional TipoUsuario As String = "*", Optional ByVal Estado As String = "*") As List(Of VI_GNRL_USUARIO)
        Return UsuarioMet.UsuarioListaGrid(Filtro, Buscar, TipoUsuario, Estado)
    End Function

    Public Function UsuarioListarPorId(ByVal IdUsuario As String) As GNRL_USUARIO
        If UsuarioMet.UsuarioExiste(IdUsuario) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El usuario con el id " & IdUsuario & " no Existe!!!")
        Else
            Return UsuarioMet.UsuarioListarPorId(IdUsuario)
        End If
    End Function

    Public Function UsuarioListarPorLogin(ByVal IdLogin As String, ByVal strPuntoVenta As String, ByVal strEmpresa As String) As GNRL_USUARIO
        'If UsuarioMet.UsuarioExiste(IdUsuario) = False Then
        '    'Si el producto no existe lanzo una excepción.
        '    Throw New Exception("El usuario con el id " & IdUsuario & " no Existe!!!")
        'Else
        Return UsuarioMet.UsuarioListarPorLogin(IdLogin, strPuntoVenta, strEmpresa)
        'End If
    End Function

    'Public Function UsuarioValidar(ByVal IdLogin As String, ByVal strContraseña As String, ByVal strEmpresa As String, ByVal strArea As String, ByVal strSistema As String, ByVal strModulo As String, ByVal strLocal As String) As GNRL_USUARIO
    Public Function UsuarioValidar(ByVal IdLogin As String, ByVal strContraseña As String, ByVal strEmpresa As String, ByVal strSistema As String, ByVal strModulo As String, ByVal strLocal As String) As GNRL_USUARIO
        'Return UsuarioMet.UsuarioValidar(IdLogin, strContraseña, strEmpresa, strArea, strSistema, strModulo, strLocal)
        Return UsuarioMet.UsuarioValidar(IdLogin, strContraseña, strEmpresa, strSistema, strModulo, strLocal)
    End Function

    Public Function ContratoListarCombo() As DataTable
        Return UsuarioMet.contratoListarCombo()
    End Function
End Class
