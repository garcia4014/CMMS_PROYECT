Public Class clsTipoPersonaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function TipoPersonaListarCombo() As List(Of GNRL_TIPOPERSONA)
        Dim Consulta = Data.PA_GNRL_MNT_TIPOPERSONA("SQL_SELECT_ALL", "", "", "", "", "", "0", "")
        Dim Coleccion As New List(Of GNRL_TIPOPERSONA)
        For Each TipoPersona In Consulta
            Dim TipPer As New GNRL_TIPOPERSONA
            TipPer.cIdTipoPersona = TipoPersona.cIdTipoPersona
            TipPer.vDescripcionTipoPersona = TipoPersona.vDescripcionTipoPersona
            Coleccion.Add(TipPer)
        Next
        Return Coleccion
    End Function

    Public Function TipoPersonaListarPorId(ByVal IdTipoPersona As String) As GNRL_TIPOPERSONA
        Dim Consulta = Data.PA_GNRL_MNT_TIPOPERSONA("SQL_NONE", "SELECT * FROM GNRL_TIPOPERSONA WHERE cIdTipoPersona = '" & IdTipoPersona & "'", "", "", "", "", "1", "")
        Dim Coleccion As New GNRL_TIPOPERSONA
        For Each GNRL_TIPOPERSONA In Consulta
            Coleccion.cIdTipoPersona = GNRL_TIPOPERSONA.cIdTipoPersona
            Coleccion.vDescripcionTipoPersona = GNRL_TIPOPERSONA.vDescripcionTipoPersona
            Coleccion.vDescripcionAbreviadaTipoPersona = GNRL_TIPOPERSONA.vDescripcionAbreviadaTipoPersona
            Coleccion.cIdArea = GNRL_TIPOPERSONA.cIdArea
            Coleccion.bEstadoRegistroTipoPersona = GNRL_TIPOPERSONA.vDescripcionTipoPersona
        Next
        Return Coleccion
    End Function

    Public Function TipoPersonaListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_TIPOPERSONA)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_TIPOPERSONA("SQL_NONE", "SELECT * FROM GNRL_TIPOPERSONA WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroTipoPersona = 1", "", "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_GNRL_TIPOPERSONA)
        For Each Busqueda In Consulta
            Dim BuscarTipPer As New VI_GNRL_TIPOPERSONA
            BuscarTipPer.Codigo = Busqueda.cIdTipoPersona
            BuscarTipPer.Descripcion = Busqueda.vDescripcionTipoPersona
            BuscarTipPer.Area = Busqueda.cIdArea
            BuscarTipPer.Estado = Busqueda.bEstadoRegistroTipoPersona
            Coleccion.Add(BuscarTipPer)
        Next
        Return Coleccion
    End Function

    Public Function TipoPersonaInserta(ByVal TipoPersona As GNRL_TIPOPERSONA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOPERSONA("SQL_INSERT", "", TipoPersona.cIdTipoPersona, TipoPersona.vDescripcionTipoPersona, TipoPersona.vDescripcionAbreviadaTipoPersona, TipoPersona.cIdArea, TipoPersona.bEstadoRegistroTipoPersona, TipoPersona.cIdTipoPersona).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoPersonaEdita(ByVal TipoPersona As GNRL_TIPOPERSONA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOPERSONA("SQL_UPDATE", "", TipoPersona.cIdTipoPersona, TipoPersona.vDescripcionTipoPersona, TipoPersona.vDescripcionAbreviadaTipoPersona, TipoPersona.cIdArea, TipoPersona.bEstadoRegistroTipoPersona, TipoPersona.cIdTipoPersona).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoPersonaElimina(ByVal TipoPersona As GNRL_TIPOPERSONA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPOPERSONA("SQL_NONE", "UPDATE GNRL_TIPOPERSONA SET bEstadoRegistroTipoPersona = 0 WHERE cIdTipoPersona = '" & TipoPersona.cIdTipoPersona & "'",
                                     "", "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoPersonaExiste(ByVal IdTipoPersona As String) As Boolean
        If Data.PA_GNRL_MNT_TIPOPERSONA("SQL_NONE", "SELECT * FROM GNRL_TIPOPERSONA WHERE cIdTipoPersona = '" & IdTipoPersona & "' AND bEstadoRegistroTipoPersona = 1", "", "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
