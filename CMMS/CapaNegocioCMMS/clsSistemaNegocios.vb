Imports CapaDatosCMMS

Public Class clsSistemaNegocios
    Dim SistemaMet As New clsSistemaMetodos

    Public Function SistemaListarCombo() As List(Of GNRL_SISTEMA)
        Return SistemaMet.SistemaListarCombo
    End Function

    Public Function SistemaInserta(ByVal Sistema As GNRL_SISTEMA) As Int32
        If SistemaMet.SistemaExiste(Sistema.cIdSistema) = True Then
            Throw New Exception("El Sistema con el id " & Sistema.cIdSistema & " ya existe!")
        Else
            Return SistemaMet.SistemaInserta(Sistema)
        End If
    End Function

    Public Function SistemaEdita(ByVal Sistema As GNRL_SISTEMA) As Int32
        If SistemaMet.SistemaExiste(Sistema.cIdSistema) = False Then
            Throw New Exception("El Sistema con el id " & Sistema.cIdSistema & " no existe!")
        Else
            Return SistemaMet.SistemaEdita(Sistema)
        End If
    End Function

    Public Function SistemaElimina(ByVal Sistema As GNRL_SISTEMA)
        If SistemaMet.SistemaExiste(Sistema.cIdSistema) = False Then
            Throw New Exception("El Sistema con el id " & Sistema.cIdSistema & " no existe!")
        Else
            Return SistemaMet.SistemaElimina(Sistema)
        End If
    End Function

    Public Function SistemaListaBusqueda(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_SISTEMA)
        Return SistemaMet.SistemaListaGrid(Filtro, Buscar)
    End Function

    Public Function SistemaListarPorId(ByVal IdSistema As String) As GNRL_SISTEMA
        If SistemaMet.SistemaExiste(IdSistema) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("El Sistema con el id " & IdSistema & " no Existe!!!")
        Else
            Return SistemaMet.SistemaListarPorId(IdSistema)
        End If
    End Function
End Class
