Public Class clsTipoDocumentoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function TipoDocumentoListarPorId(ByVal IdTipoDocumento As String) As GNRL_TIPODOCUMENTO
        Dim Consulta = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_TIPODOCUMENTO WHERE cIdTipoDocumento = '" & IdTipoDocumento & "' AND bEstadoRegistroTipoDocumento = 1", "", "", "1", "", "")
        Dim Coleccion As New GNRL_TIPODOCUMENTO
        For Each GNRL_TIPODOCUMENTO In Consulta
            Coleccion.cIdTipoDocumento = GNRL_TIPODOCUMENTO.cIdTipoDocumento
            Coleccion.vDescripcionTipoDocumento = GNRL_TIPODOCUMENTO.vDescripcionTipoDocumento
            Coleccion.bEstadoRegistroTipoDocumento = GNRL_TIPODOCUMENTO.bEstadoRegistroTipoDocumento
            Coleccion.vIdEquivalenciaContableTipoDocumento = GNRL_TIPODOCUMENTO.vIdEquivalenciaContableTipoDocumento
        Next
        Return Coleccion
    End Function

    Public Function TipoDocumentoListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_TIPODOCUMENTO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_TIPODOCUMENTO WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroTipoDocumento = 1", "", "", "1", "", "")
        Dim Coleccion As New List(Of VI_GNRL_TIPODOCUMENTO)
        For Each Busqueda In Consulta
            Dim BuscarTipDoc As New VI_GNRL_TIPODOCUMENTO
            BuscarTipDoc.Codigo = Busqueda.cIdTipoDocumento
            BuscarTipDoc.Descripcion = Busqueda.vDescripcionTipoDocumento
            BuscarTipDoc.Estado = Busqueda.bEstadoRegistroTipoDocumento
            Coleccion.Add(BuscarTipDoc)
        Next
        Return Coleccion
    End Function

    Public Function TipoDocumentoInserta(ByVal TipoDocumento As GNRL_TIPODOCUMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_INSERT", "", TipoDocumento.cIdTipoDocumento, TipoDocumento.vDescripcionTipoDocumento, TipoDocumento.bEstadoRegistroTipoDocumento, TipoDocumento.vIdEquivalenciaContableTipoDocumento, TipoDocumento.cIdTipoDocumento).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoDocumentoEdita(ByVal TipoDocumento As GNRL_TIPODOCUMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_UPDATE", "", TipoDocumento.cIdTipoDocumento, TipoDocumento.vDescripcionTipoDocumento, TipoDocumento.bEstadoRegistroTipoDocumento, TipoDocumento.vIdEquivalenciaContableTipoDocumento, TipoDocumento.cIdTipoDocumento).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoDocumentoElimina(ByVal TipoDocumento As GNRL_TIPODOCUMENTO) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_NONE", "UPDATE GNRL_TIPODOCUMENTO SET bEstadoRegistroTipoDocumento = 0 WHERE cIdTipoDocumento = '" & TipoDocumento.cIdTipoDocumento & "'",
                                        "", "", "1", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoDocumentoExiste(ByVal IdTipoDocumento As String) As Boolean
        If Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_TIPODOCUMENTO WHERE cIdTipoDocumento = '" & IdTipoDocumento & "' AND bEstadoRegistroTipoDocumento = 1", "", "", "1", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function TipoDocumentoListarCombo(Optional ByVal IdModulo As String = "*", Optional ByVal IdSistema As String = "*", Optional ByVal IdTipoDocumento As String = "*") As List(Of GNRL_TIPODOCUMENTO)
        Dim Consulta
        If IdSistema = "*" And IdModulo = "*" And IdTipoDocumento = "*" Then
            Consulta = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_TIPODOCUMENTO ORDER BY nOrdenTipoDocumento", "", "", "1", "", "")
        ElseIf IdSistema = "*" And IdModulo = "*" And IdTipoDocumento <> "*" Then
            Consulta = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_TIPODOCUMENTO WHERE cIdTipoDocumento IN ('" & IdTipoDocumento & "') ORDER BY nOrdenTipoDocumento", "", "", "1", "", "")
        Else
            Consulta = Data.PA_GNRL_MNT_TIPODOCUMENTO("SQL_NONE", "SELECT * FROM GNRL_TIPODOCUMENTO WHERE (cIdModulo = '" & IdModulo & "' OR '*' = '" & IdModulo & "') AND cIdSistema = '" & IdSistema & "' ORDER BY nOrdenTipoDocumento", "", "", "1", "", "")
        End If

        Dim Coleccion As New List(Of GNRL_TIPODOCUMENTO)
        For Each TipoDocumento In Consulta
            Dim TipDoc As New GNRL_TIPODOCUMENTO
            TipDoc.cIdTipoDocumento = TipoDocumento.cIdTipoDocumento
            TipDoc.vDescripcionTipoDocumento = TipoDocumento.vIdEquivalenciaContableTipoDocumento + " - " + TipoDocumento.vDescripcionTipoDocumento
            Coleccion.Add(TipDoc)
        Next
        Return Coleccion
    End Function
End Class