Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsVideoTutorialMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function VideoTutorialGetData(strQuery As String) As DataTable
        Dim dt As New DataTable()
        Dim constr As String = My.Settings.CMMSConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strQuery)
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    sda.Fill(dt)
                End Using
            End Using
            Return dt
        End Using
    End Function

    Public Function VideoTutorialListarCombo(ByVal IdVideoTutorial As String, Optional ByVal Estado As String = "*") As List(Of GNRL_VIDEOTUTORIAL)
        Dim Consulta = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_NONE", "SELECT * FROM GNRL_VIDEOTUTORIAL " &
                                                 "WHERE cIdVideoTutorial = '" & IdVideoTutorial & "' AND (bEstadoRegistroVideoTutorial = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                 "", "", "", "", Now, "", "1", "", "", "")
        Dim Coleccion As New List(Of GNRL_VIDEOTUTORIAL)
        For Each VideoTutorial In Consulta
            Dim Vid As New GNRL_VIDEOTUTORIAL
            Vid.cIdVideoTutorial = VideoTutorial.cIdVideoTutorial
            Vid.vDescripcionVideoTutorial = VideoTutorial.vDescripcionVideoTutorial
            Coleccion.Add(Vid)
        Next
        Return Coleccion
    End Function

    Public Function VideoTutorialListarPorId(ByVal IdVideoTutorial As String, Optional ByVal Estado As String = "1") As GNRL_VIDEOTUTORIAL
        Dim Consulta = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_NONE", "SELECT * FROM GNRL_VIDEOTUTORIAL WHERE cIdVideoTutorial = '" & IdVideoTutorial &
                                                      "' AND (bEstadoRegistroVideoTutorial = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                                      "", "", "", "", Now, "", "1", "", "", "")
        Dim Coleccion As New GNRL_VIDEOTUTORIAL
        For Each GNRL_VIDEOTUTORIAL In Consulta
            Coleccion.cIdVideoTutorial = GNRL_VIDEOTUTORIAL.cIdVideoTutorial
            Coleccion.vTituloVideoTutorial = GNRL_VIDEOTUTORIAL.vTituloVideoTutorial
            Coleccion.vDescripcionVideoTutorial = GNRL_VIDEOTUTORIAL.vDescripcionVideoTutorial
            Coleccion.vLinkYouTubeVideoTutorial = GNRL_VIDEOTUTORIAL.vLinkYouTubeVideoTutorial
            Coleccion.dFechaRegistroVideoTutorial = GNRL_VIDEOTUTORIAL.dFechaRegistroVideoTutorial
            Coleccion.vTiempoMinutosVideoTutorial = GNRL_VIDEOTUTORIAL.vTiempoMinutosVideoTutorial
            Coleccion.bEstadoRegistroVideoTutorial = GNRL_VIDEOTUTORIAL.bEstadoRegistroVideoTutorial
            Coleccion.vIdYouTubeVideoTutorial = GNRL_VIDEOTUTORIAL.vIdYouTubeVideoTutorial
            Coleccion.cIdSistemaVideoTutorial = GNRL_VIDEOTUTORIAL.cIdSistemaVideoTutorial
        Next
        Return Coleccion
    End Function

    Public Function VideoTutorialListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_GNRL_VIDEOTUTORIAL)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_NONE", "SELECT VIDTUT.cIdVideoTutorial, VIDTUT.vDescripcionVideoTutorial, VIDTUT.vLinkYouTubeVideoTutorial, VIDTUT.dFechaRegistroVideoTutorial, " &
                                                      "VIDTUT.bEstadoRegistroVideoTutorial, VIDTUT.vTituloVideoTutorial, VIDTUT.vTiempoMinutosVideoTutorial, VIDTUT.vIdYouTubeVideoTutorial " &
                                                      "FROM GNRL_VIDEOTUTORIAL AS VIDTUT " &
                                                      "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (VIDTUT.bEstadoRegistroVideoTutorial = '" & Estado & "' OR '*' = '" & Estado & "') ",
                                                      "", "", "", "", Now, "", "1", "", "", "")

        Dim Coleccion As New List(Of VI_GNRL_VIDEOTUTORIAL)
        For Each Busqueda In Consulta
            Dim BuscarVid As New VI_GNRL_VIDEOTUTORIAL
            BuscarVid.Codigo = Busqueda.cIdVideoTutorial
            BuscarVid.Titulo = Busqueda.vTituloVideoTutorial
            BuscarVid.Descripcion = Busqueda.vDescripcionVideoTutorial
            BuscarVid.TiempoMinutos = Busqueda.vTiempoMinutosVideoTutorial
            BuscarVid.Estado = Busqueda.bEstadoRegistroVideoTutorial
            BuscarVid.IdYouTube = Busqueda.vIdYouTubeVideoTutorial
            Coleccion.Add(BuscarVid)
        Next
        Return Coleccion
    End Function

    'Public Function VideoTutorialInsertaDetalle(ByVal DetalleVideoTutorial As List(Of LOGI_VideoTutorial), Optional ByVal strNroEnlaceVideoTutorial As String = "") As Int32
    '    Dim x

    '    'Inicializo la Transacción
    '    Dim tOption As New TransactionOptions
    '    tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
    '    tOption.Timeout = TimeSpan.MaxValue

    '    Dim dtVideoTutorialComponente
    '    dtVideoTutorialComponente = VideoTutorialGetData("SELECT cIdVideoTutorial FROM LOGI_VideoTutorial WHERE cIdEnlaceVideoTutorial = '" & strNroEnlaceVideoTutorial & "'")
    '    Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
    '        For Each Busqueda In DetalleVideoTutorial
    '            Dim bExiste As Boolean
    '            bExiste = False
    '            For Each row In dtVideoTutorialComponente.Rows
    '                If row("cIdVideoTutorial").ToString.Trim = Busqueda.cIdVideoTutorial Then
    '                    bExiste = True
    '                    Exit For
    '                End If
    '            Next

    '            If bExiste = False Then
    '                x = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_INSERT", "", Busqueda.cIdVideoTutorial, Busqueda.cIdJerarquiaVideoTutorial,
    '                                              Busqueda.cIdSistemaFuncional, strNroEnlaceVideoTutorial, Busqueda.vDescripcionVideoTutorial,
    '                                              Busqueda.vDescripcionAbreviadaVideoTutorial, Busqueda.bEstadoRegistroVideoTutorial,
    '                                              Busqueda.cIdCuentaContableVideoTutorial, Busqueda.cIdCuentaContableLeasingVideoTutorial,
    '                                              Busqueda.nVidaUtilVideoTutorial, Busqueda.cIdTipoActivo,
    '                                              Busqueda.nPeriodoGarantiaVideoTutorial, Busqueda.nPeriodoMinimoMantenimientoVideoTutorial,
    '                                              Busqueda.cIdVideoTutorial).ReturnValue.ToString
    '            Else
    '                x = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_NONE", "UPDATE LOGI_EQUIPO SET vDescripcionEquipo = '" & Busqueda.vDescripcionVideoTutorial & "', vDescripcionAbreviadaEquipo = '" & Busqueda.vDescripcionAbreviadaVideoTutorial & "' " &
    '                                              "WHERE cIdEnlaceVideoTutorial = '" & strNroEnlaceVideoTutorial & "' and cIdVideoTutorial = '" & Busqueda.cIdVideoTutorial & "'", Busqueda.cIdVideoTutorial, Busqueda.cIdJerarquiaVideoTutorial,
    '                          Busqueda.cIdSistemaFuncional, strNroEnlaceVideoTutorial, Busqueda.vDescripcionVideoTutorial,
    '                          Busqueda.vDescripcionAbreviadaVideoTutorial, Busqueda.bEstadoRegistroVideoTutorial,
    '                          Busqueda.cIdCuentaContableVideoTutorial, Busqueda.cIdCuentaContableLeasingVideoTutorial, Busqueda.nVidaUtilVideoTutorial, Busqueda.cIdTipoActivo,
    '                           Busqueda.nPeriodoGarantiaVideoTutorial, Busqueda.nPeriodoMinimoMantenimientoVideoTutorial,
    '                          Busqueda.cIdVideoTutorial).ReturnValue.ToString

    '                x = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_UPDATE", "", Busqueda.cIdVideoTutorial, Busqueda.cIdJerarquiaVideoTutorial,
    '                                              Busqueda.cIdSistemaFuncional, strNroEnlaceVideoTutorial, Busqueda.vDescripcionVideoTutorial,
    '                                              Busqueda.vDescripcionAbreviadaVideoTutorial, Busqueda.bEstadoRegistroVideoTutorial,
    '                                              Busqueda.cIdCuentaContableVideoTutorial, Busqueda.cIdCuentaContableLeasingVideoTutorial, Busqueda.nVidaUtilVideoTutorial,
    '                                              Busqueda.cIdTipoActivo, Busqueda.nPeriodoGarantiaVideoTutorial, Busqueda.nPeriodoMinimoMantenimientoVideoTutorial,
    '                                              Busqueda.cIdVideoTutorial).ReturnValue.ToString
    '            End If
    '        Next
    '        scope.Complete()
    '        Return x
    '    End Using
    'End Function

    Public Function VideoTutorialInserta(ByVal VideoTutorial As GNRL_VIDEOTUTORIAL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_INSERT", "", VideoTutorial.cIdVideoTutorial, VideoTutorial.vTituloVideoTutorial, VideoTutorial.vDescripcionVideoTutorial, VideoTutorial.vLinkYouTubeVideoTutorial,
                                           VideoTutorial.dFechaRegistroVideoTutorial, VideoTutorial.vTiempoMinutosVideoTutorial, VideoTutorial.bEstadoRegistroVideoTutorial,
                                           VideoTutorial.vIdYouTubeVideoTutorial, VideoTutorial.cIdSistemaVideoTutorial, VideoTutorial.cIdVideoTutorial).ReturnValue.ToString
        Return x
    End Function

    Public Function VideoTutorialEdita(ByVal VideoTutorial As GNRL_VIDEOTUTORIAL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_UPDATE", "", VideoTutorial.cIdVideoTutorial, VideoTutorial.vTituloVideoTutorial, VideoTutorial.vDescripcionVideoTutorial, VideoTutorial.vLinkYouTubeVideoTutorial,
                                           VideoTutorial.dFechaRegistroVideoTutorial, VideoTutorial.vTiempoMinutosVideoTutorial, VideoTutorial.bEstadoRegistroVideoTutorial,
                                           VideoTutorial.vIdYouTubeVideoTutorial, VideoTutorial.cIdSistemaVideoTutorial, VideoTutorial.cIdVideoTutorial).ReturnValue.ToString
        Return x
    End Function

    Public Function VideoTutorialElimina(ByVal VideoTutorial As GNRL_VIDEOTUTORIAL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_NONE", "UPDATE GNRL_VIDEOTUTORIAL SET bEstadoRegistroVideoTutorial = 0 WHERE cIdVideoTutorial = '" & VideoTutorial.cIdVideoTutorial & "' ",
                                           "", "", "", "", Now, "", "1", "", "", "").ReturnValue.ToString
        Return x
    End Function

    'Public Function VideoTutorialExiste(ByVal IdVideoTutorial As String, ByVal IdEmpresa As String, ByVal IdPuntoVenta As String) As Boolean
    '    Public Function VideoTutorialListarPorId(ByVal IdVideoTutorial As String, ByVal IdTipoActivo As String, ByVal Jerarquia As String, Optional ByVal Estado As String = "1") As LOGI_VideoTutorial

    Public Function VideoTutorialExiste(ByVal IdVideoTutorial As String) As Boolean
        'If Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_NONE", "SELECT * FROM LOGI_VideoTutorial WHERE cIdVideoTutorial = '" & IdVideoTutorial & "' " & _
        '                             " AND bEstadoRegistroVideoTutorial = 1", "", "", "", "", "", "", "", "1", "", "", "", "0", "", "", 0, "").Count > 0 Then
        If Data.PA_GNRL_MNT_VIDEOTUTORIAL("SQL_NONE", "SELECT * FROM GNRL_VIDEOTUTORIAL WHERE cIdVideoTutorial = '" & IdVideoTutorial & "' ",
                                          "", "", "", "", Now, "", "1", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class