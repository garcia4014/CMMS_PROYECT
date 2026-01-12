Public Class clsSistemaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function SistemaListarCombo() As List(Of GNRL_SISTEMA)
        Dim Consulta = Data.PA_GNRL_MNT_SISTEMA("SQL_NONE", "SELECT * FROM GNRL_SISTEMA WHERE bEstadoRegistroSistema = 1", "", "", "", "", Now, Now, "0", "")
        Dim Coleccion As New List(Of GNRL_SISTEMA)
        For Each Sistema In Consulta
            Dim Sist As New GNRL_SISTEMA
            Sist.cIdSistema = Sistema.cIdSistema
            Sist.vDescripcionSistema = Sistema.vDescripcionSistema
            Coleccion.Add(Sist)
        Next
        Return Coleccion
    End Function

    Public Function SistemaListarPorId(ByVal IdSistema As String) As GNRL_SISTEMA
        Dim Consulta = Data.PA_GNRL_MNT_SISTEMA("SQL_NONE", "SELECT * FROM GNRL_Sistema WHERE cIdSistema = '" & IdSistema & "'", "", "", "", "", Now, Now, "0", "")
        Dim Coleccion As New GNRL_SISTEMA
        For Each GNRL_SISTEMA In Consulta
            Coleccion.cIdSistema = GNRL_SISTEMA.cIdSistema
            Coleccion.vDescripcionSistema = GNRL_SISTEMA.vDescripcionSistema
            Coleccion.vDescripcionAbreviadaSistema = GNRL_SISTEMA.vDescripcionAbreviadaSistema
            Coleccion.vRutaBaseDatoSistema = GNRL_SISTEMA.vRutaBaseDatoSistema
            Coleccion.dFechaActualizacionSistema = GNRL_SISTEMA.dFechaActualizacionSistema
            Coleccion.dFechaCreacionSistema = GNRL_SISTEMA.dFechaCreacionSistema
            Coleccion.bEstadoRegistroSistema = GNRL_SISTEMA.bEstadoRegistroSistema
        Next
        Return Coleccion
    End Function

    Public Function SistemaListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_SISTEMA)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_SISTEMA("SQL_NONE", "SELECT * FROM GNRL_Sistema WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroSistema = 1", "", "", "", "", Now, Now, "0", "")
        Dim Coleccion As New List(Of VI_GNRL_SISTEMA)
        For Each Busqueda In Consulta
            Dim BuscarSist As New VI_GNRL_SISTEMA
            BuscarSist.Codigo = Busqueda.cIdSistema
            BuscarSist.Descripcion = Busqueda.vDescripcionSistema
            BuscarSist.Estado = Busqueda.bEstadoRegistroSistema
            Coleccion.Add(BuscarSist)
        Next
        Return Coleccion
    End Function

    Public Function SistemaInserta(ByVal Sistema As GNRL_SISTEMA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_SISTEMA("SQL_INSERT", "", Sistema.cIdSistema, Sistema.vDescripcionSistema, Sistema.vDescripcionAbreviadaSistema,
                                     Sistema.vRutaBaseDatoSistema, Sistema.dFechaCreacionSistema, Sistema.dFechaActualizacionSistema,
                                     Sistema.bEstadoRegistroSistema, Sistema.cIdSistema).ReturnValue.ToString
        Return x
    End Function

    Public Function SistemaEdita(ByVal Sistema As GNRL_SISTEMA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_SISTEMA("SQL_UPDATE", "", Sistema.cIdSistema, Sistema.vDescripcionSistema, Sistema.vDescripcionAbreviadaSistema,
                                     Sistema.vRutaBaseDatoSistema, Sistema.dFechaCreacionSistema, Sistema.dFechaActualizacionSistema,
                                     Sistema.bEstadoRegistroSistema, Sistema.cIdSistema).ReturnValue.ToString
        Return x
    End Function

    Public Function SistemaElimina(ByVal Sistema As GNRL_SISTEMA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_SISTEMA("SQL_NONE", "UPDATE GNRL_SISTEMA SET bEstadoRegistroSistema = 0 WHERE cIdSistema = '" & Sistema.cIdSistema & "'",
                                     "", "", "", "", Now, Now, "0", "").ReturnValue.ToString
        Return x
    End Function

    Public Function SistemaExiste(ByVal IdSistema As String) As Boolean
        If Data.PA_GNRL_MNT_SISTEMA("SQL_NONE", "SELECT * FROM GNRL_SISTEMA WHERE cIdSistema = '" & IdSistema & "' AND bEstadoRegistroSistema = 1", "", "", "", "", Now, Now, "0", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
