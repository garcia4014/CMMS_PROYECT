Imports CapaDatosCMMS

Public Class clsUsuarioAccesoNegocios
    Dim UsuarioAccesoMet As New clsUsuarioAccesoMetodos

    Public Function UsuarioAccesoGetData(strQuery As String) As DataTable
        Return UsuarioAccesoMet.UsuarioAccesoGetData(strQuery)
    End Function

    'Public Function UsuarioAccesoInserta(ByVal UsuarioAcceso As GNRL_USUARIOACCESO) As Int32
    '    If UsuarioAccesoMet.UsuarioAccesoExiste(UsuarioAcceso.cIdLoginUsuarioAcceso) = True Then
    '        Throw New Exception("El UsuarioAcceso con el id " & UsuarioAcceso.cIdLoginUsuarioAcceso & " ya existe!")
    '    Else
    '        Return UsuarioAccesoMet.UsuarioAccesoInserta(UsuarioAcceso)
    '    End If
    'End Function

    'Public Function UsuarioAccesoEdita(ByVal UsuarioAcceso As GNRL_UsuarioAcceso) As Int32
    '    If UsuarioAccesoMet.UsuarioAccesoExiste(UsuarioAcceso.cIdLoginUsuarioAcceso) = False Then
    '        Throw New Exception("El UsuarioAcceso con el id " & UsuarioAcceso.cIdLoginUsuarioAcceso & " no existe!")
    '    Else
    '        Return UsuarioAccesoMet.UsuarioAccesoEdita(UsuarioAcceso)
    '    End If
    'End Function

    'Public Function UsuarioAccesoElimina(ByVal UsuarioAcceso As GNRL_UsuarioAcceso)
    '    If UsuarioAccesoMet.UsuarioAccesoExiste(UsuarioAcceso.cIdLoginUsuarioAcceso) = False Then
    '        Throw New Exception("El UsuarioAcceso con el id " & UsuarioAcceso.cIdLoginUsuarioAcceso & " no existe!")
    '    Else
    '        Return UsuarioAccesoMet.UsuarioAccesoElimina(UsuarioAcceso)
    '    End If
    'End Function

    'Public Function UsuarioAccesoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional TipoUsuarioAcceso As String = "*", Optional ByVal Estado As String = "*") As List(Of VI_GNRL_UsuarioAcceso)
    '    Return UsuarioAccesoMet.UsuarioAccesoListaGrid(Filtro, Buscar, TipoUsuarioAcceso, Estado)
    'End Function
    Public Function UsuarioAccesoListarPorId(ByVal IdEmpresa As String, ByVal IdPais As String, ByVal IdLocal As String, ByVal IdLogin As String) As GNRL_USUARIOACCESO
        If UsuarioAccesoMet.UsuarioAccesoExiste(IdLogin, IdEmpresa, IdPais, IdLocal) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Usuario con el id " & IdLogin & " no tiene acceso!!!")
        Else
            Return UsuarioAccesoMet.UsuarioAccesoListarPorId(IdEmpresa, IdPais, IdLocal, IdLogin)
        End If
    End Function

    Public Function UsuarioAccesoInsertaDetalle(ByVal UsuarioAcceso As List(Of GNRL_USUARIOACCESO), ByVal IdSistema As String, ByVal IdOpcionModulo As String) As Int32
        'If UsuarioAccesoMet.UsuarioAccesoInsertaDetalle(UsuarioAcceso.cIdEmpresa, UsuarioAcceso.cIdPaisOrigenUsuarioAcceso,
        '                                                UsuarioAcceso.cIdLocalPredeterminado, UsuarioAcceso.cIdLoginUsuario) = True Then
        '    Throw New Exception("El UsuarioAcceso con el id " & UsuarioAcceso.cIdLoginUsuarioAcceso & " ya existe!")
        'Else
        Return UsuarioAccesoMet.UsuarioAccesoInsertaDetalle(UsuarioAcceso, IdSistema, IdOpcionModulo)
        'End If
    End Function
End Class
