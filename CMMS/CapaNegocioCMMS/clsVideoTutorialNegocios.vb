'Imports CapaDatosLogi
Imports CapaDatosCMMS

Public Class clsVideoTutorialNegocios
    Dim VideoTutorialMet As New clsVideoTutorialMetodos

    Public Function VideoTutorialGetData(strQuery As String) As DataTable
        Return VideoTutorialMet.VideoTutorialGetData(strQuery)
    End Function

    Public Function VideoTutorialListarCombo(ByVal IdVideoTutorial As String, Optional ByVal Estado As String = "*") As List(Of GNRL_VIDEOTUTORIAL)
        Return VideoTutorialMet.VideoTutorialListarCombo(IdVideoTutorial, Estado)
    End Function

    'Public Function VideoTutorialInsertaDetalle(ByVal DetalleVideoTutorial As List(Of GNRL_VIDEOTUTORIAL), Optional ByVal strNroEnlaceVideoTutorial As String = "") As Int32
    '    'If VideoTutorialMet.VideoTutorialExiste(VideoTutorial.cIdVideoTutorial) = True Then
    '    'Throw New Exception("El VideoTutorial con el id " & VideoTutorial.cIdVideoTutorial & " ya existe!")
    '    'Else
    '    Return VideoTutorialMet.VideoTutorialInsertaDetalle(DetalleVideoTutorial, strNroEnlaceVideoTutorial)
    '    'End If
    'End Function

    Public Function VideoTutorialInserta(ByVal VideoTutorial As GNRL_VIDEOTUTORIAL) As Int32
        If VideoTutorialMet.VideoTutorialExiste(VideoTutorial.cIdVideoTutorial) = True Then
            Throw New Exception("El Video Tutorial con el id " & VideoTutorial.cIdVideoTutorial & " ya existe!")
        Else
            Return VideoTutorialMet.VideoTutorialInserta(VideoTutorial)
        End If
    End Function

    Public Function VideoTutorialEdita(ByVal VideoTutorial As GNRL_VIDEOTUTORIAL) As Int32
        If VideoTutorialMet.VideoTutorialExiste(VideoTutorial.cIdVideoTutorial) = False Then
            Throw New Exception("El Video Tutorial con el id " & VideoTutorial.cIdVideoTutorial & " no existe!")
        Else
            Return VideoTutorialMet.VideoTutorialEdita(VideoTutorial)
        End If
    End Function

    Public Function VideoTutorialElimina(ByVal VideoTutorial As GNRL_VIDEOTUTORIAL) As Int32
        If VideoTutorialMet.VideoTutorialExiste(VideoTutorial.cIdVideoTutorial) = False Then
            Throw New Exception("El Video Tutorial con el id " & VideoTutorial.cIdVideoTutorial & " no existe!")
        Else
            Return VideoTutorialMet.VideoTutorialElimina(VideoTutorial)
        End If
    End Function

    Public Function VideoTutorialListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, Optional ByVal Estado As String = "1") As List(Of VI_GNRL_VIDEOTUTORIAL)
        Return VideoTutorialMet.VideoTutorialListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function VideoTutorialListarPorId(ByVal IdVideoTutorial As String, Optional ByVal Estado As String = "1") As GNRL_VIDEOTUTORIAL
        If VideoTutorialMet.VideoTutorialExiste(IdVideoTutorial) = False Then
            'Si el VideoTutorial no existe lanzo una excepción.
            Throw New Exception("El Video Tutorial con el id " & IdVideoTutorial & " no Existe!!!")
        Else
            Return VideoTutorialMet.VideoTutorialListarPorId(IdVideoTutorial, Estado)
        End If
    End Function
End Class