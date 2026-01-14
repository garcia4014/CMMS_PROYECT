Public Class clsPerfilMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function PerfilObtenerPerfil(ByVal IdUsuario As String) As List(Of GNRL_PERFIL)
        Dim Consulta = Data.PA_GNRL_MNT_PERFIL("SQL_NONE", "SELECT DISTINCT cIdPerfil FROM GNRL_CONFIGURACION " &
                                               "WHERE bEstadoRegistroConfiguracion = 1 AND cIdUsuario = '" & IdUsuario & "'", "", "", "", "0", "")

        Dim Coleccion As New List(Of GNRL_PERFIL)

        If Data.PA_GNRL_MNT_PERFIL("SQL_NONE", "SELECT DISTINCT cIdPerfil FROM GNRL_CONFIGURACION " &
                                               "WHERE bEstadoRegistroConfiguracion = 1 AND cIdUsuario = '" & IdUsuario & "'", "", "", "", "0", "").Count > 0 Then
            For Each Perfil In Consulta
                Dim Perf As New GNRL_PERFIL
                Perf.cIdPerfil = Perfil.cIdPerfil
                'Perf.vDescripcionPerfil = Perfil.vDescripcionPerfil
                Coleccion.Add(Perf)
            Next
        Else
            Dim Perf As New GNRL_PERFIL
            Perf.cIdPerfil = ""
            Coleccion.Add(Perf)
        End If
        Return Coleccion
    End Function


    Public Function PerfilListarCombo() As List(Of GNRL_PERFIL)
        Dim Consulta = Data.PA_GNRL_MNT_PERFIL("SQL_NONE", "SELECT * FROM GNRL_PERFIL WHERE bEstadoRegistroPerfil = 1", "", "", "", "0", "")
        Dim Coleccion As New List(Of GNRL_PERFIL)
        For Each Perfil In Consulta
            Dim Perf As New GNRL_PERFIL
            Perf.cIdPerfil = Perfil.cIdPerfil
            Perf.vDescripcionPerfil = Perfil.vDescripcionPerfil
            Coleccion.Add(Perf)
        Next
        Return Coleccion
    End Function

    Public Function PerfilListarPorId(ByVal IdPerfil As String) As GNRL_PERFIL
        Dim Consulta = Data.PA_GNRL_MNT_PERFIL("SQL_NONE", "SELECT * FROM GNRL_PERFIL WHERE cIdPerfil = '" & IdPerfil & "'", "", "", "", "1", "")
        Dim Coleccion As New GNRL_PERFIL
        For Each GNRL_PERFIL In Consulta
            Coleccion.cIdPerfil = GNRL_PERFIL.cIdPerfil
            Coleccion.bEstadoRegistroPerfil = GNRL_PERFIL.bEstadoRegistroPerfil
            Coleccion.vDescripcionPerfil = GNRL_PERFIL.vDescripcionPerfil
            Coleccion.vDescripcionAbreviadaPerfil = GNRL_PERFIL.vDescripcionAbreviadaPerfil
        Next
        Return Coleccion
    End Function

    Public Function PerfilListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_PERFIL)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_PERFIL("SQL_NONE", "SELECT * FROM GNRL_PERFIL WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroPerfil = 1", "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_GNRL_PERFIL)
        For Each Busqueda In Consulta
            Dim BuscarPerf As New VI_GNRL_PERFIL
            BuscarPerf.Codigo = Busqueda.cIdPerfil
            BuscarPerf.Descripcion = Busqueda.vDescripcionPerfil
            BuscarPerf.Estado = Busqueda.bEstadoRegistroPerfil
            Coleccion.Add(BuscarPerf)
        Next
        Return Coleccion
    End Function

    Public Function PerfilInserta(ByVal Perfil As GNRL_PERFIL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_PERFIL("SQL_INSERT", "", Perfil.cIdPerfil, Perfil.vDescripcionPerfil, Perfil.vDescripcionAbreviadaPerfil, Perfil.bEstadoRegistroPerfil, Perfil.cIdPerfil).ReturnValue.ToString
        Return x
    End Function

    Public Function PerfilEdita(ByVal Perfil As GNRL_PERFIL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_PERFIL("SQL_UPDATE", "", Perfil.cIdPerfil, Perfil.vDescripcionPerfil, Perfil.vDescripcionAbreviadaPerfil, Perfil.bEstadoRegistroPerfil, Perfil.cIdPerfil).ReturnValue.ToString
        Return x
    End Function

    Public Function PerfilElimina(ByVal Perfil As GNRL_PERFIL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_PERFIL("SQL_NONE", "UPDATE GNRL_PERFIL SET bEstadoRegistroPerfil = 0 WHERE cIdPerfil = '" & Perfil.cIdPerfil & "'",
                                     "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function PerfilExiste(ByVal IdPerfil As String) As Boolean
        If Data.PA_GNRL_MNT_PERFIL("SQL_NONE", "SELECT * FROM GNRL_PERFIL WHERE cIdPerfil = '" & IdPerfil & "' and bEstadoRegistroPerfil = 1", "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
