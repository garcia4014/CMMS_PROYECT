Public Class clsAreaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function AreaListarCombo() As List(Of GNRL_AREA)
        Dim Consulta = Data.PA_GNRL_MNT_AREA("SQL_NONE", "SELECT * FROM GNRL_AREA WHERE cIdentificadorDepartamentoArea = 'A'", "", "", "", "0", "", "", "")

        Dim Coleccion As New List(Of GNRL_AREA)
        For Each AreaEmpresa In Consulta
            Dim Area As New GNRL_AREA
            Area.cIdArea = AreaEmpresa.cIdArea
            Area.vDescripcionArea = AreaEmpresa.vDescripcionArea
            Coleccion.Add(Area)
        Next
        Return Coleccion
    End Function

    Public Function AreaDepartamentoListarCombo() As List(Of GNRL_AREA)
        Dim Consulta = Data.PA_GNRL_MNT_AREA("SQL_NONE", "SELECT * FROM GNRL_AREA WHERE cIdentificadorDepartamentoArea = 'D' ", "", "", "", "0", "", "", "")

        Dim Coleccion As New List(Of GNRL_AREA)
        For Each AreaEmpresa In Consulta
            Dim Area As New GNRL_AREA
            Area.cIdArea = AreaEmpresa.cIdArea
            Area.vDescripcionArea = AreaEmpresa.vDescripcionArea
            Coleccion.Add(Area)
        Next
        Return Coleccion
    End Function

    Public Function AreaListarPorId(ByVal IdArea As String) As GNRL_AREA
        Dim Consulta = Data.PA_GNRL_MNT_AREA("SQL_NONE", "SELECT * FROM GNRL_AREA " &
                                                             "WHERE cIdArea = '" & IdArea & "'", "", "", "", "0", "", "", "")
        Dim Coleccion As New GNRL_AREA
        For Each GNRL_AREA In Consulta
            Coleccion.cIdArea = GNRL_AREA.cIdArea
            Coleccion.vDescripcionArea = GNRL_AREA.vDescripcionArea
            Coleccion.vDescripcionAbreviadaArea = GNRL_AREA.vDescripcionAbreviadaArea
            Coleccion.bEstadoRegistroArea = GNRL_AREA.bEstadoRegistroArea
        Next
        Return Coleccion
    End Function

    Public Function AreaListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_AREA)
        Dim Consulta = Data.PA_GNRL_MNT_AREA("SQL_NONE", "SELECT cIdArea, vDescripcionArea, vDescripcionAbreviadaArea, " &
                                             "bEstadoRegistroArea " &
                                             "FROM GNRL_AREA " &
                                             "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND bEstadoRegistroArea = 1 " &
                                             "ORDER BY " & Filtro, "", "", "", "0", "", "", "")
        Dim Coleccion As New List(Of VI_GNRL_AREA)
        For Each Busqueda In Consulta
            Dim BuscarArea As New VI_GNRL_AREA
            BuscarArea.Codigo = Busqueda.cIdArea
            BuscarArea.Descripcion = Busqueda.vDescripcionArea
            BuscarArea.Estado = Busqueda.bEstadoRegistroArea
            Coleccion.Add(BuscarArea)
        Next
        Return Coleccion
    End Function

    Public Function AreaInserta(ByVal Area As GNRL_AREA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_AREA("SQL_INSERT", "", Area.cIdArea, Area.vDescripcionArea, Area.vDescripcionAbreviadaArea,
                                                    Area.bEstadoRegistroArea, Area.cIdAreaGeneral, Area.cIdentificadorDepartamentoArea, Area.cIdArea).ReturnValue.ToString
        Return x
    End Function

    Public Function AreaEdita(ByVal Area As GNRL_AREA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_AREA("SQL_UPDATE", "", Area.cIdArea, Area.vDescripcionArea, Area.vDescripcionAbreviadaArea,
                                                    Area.bEstadoRegistroArea, Area.cIdAreaGeneral, Area.cIdentificadorDepartamentoArea, Area.cIdArea).ReturnValue.ToString
        Return x
    End Function

    Public Function AreaElimina(ByVal Area As GNRL_AREA) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_AREA("SQL_NONE", "UPDATE GNRL_AREA SET bEstadoRegistroArea = 0 WHERE cIdArea = '" & Area.cIdArea & "'",
                                     "", "", "", "1", "", "", "").ReturnValue.ToString
        Return x
    End Function

    Public Function AreaExiste(ByVal IdArea As String) As Boolean
        If Data.PA_GNRL_MNT_AREA("SQL_NONE", "SELECT * FROM GNRL_AREA " &
                                             "WHERE cIdArea = '" & IdArea & "'", "", "", "", "0", "", "", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
