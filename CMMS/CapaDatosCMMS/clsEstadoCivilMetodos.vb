Public Class clsEstadoCivilMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function EstadoCivilListarCombo() As List(Of GNRL_ESTADOCIVIL)
        Dim Consulta = Data.PA_GNRL_MNT_ESTADOCIVIL("SQL_NONE", "SELECT * FROM GNRL_ESTADOCIVIL", "", "", "", "0", "")

        Dim Coleccion As New List(Of GNRL_ESTADOCIVIL)
        For Each EstCiv In Consulta
            Dim EstadoCivil As New GNRL_ESTADOCIVIL
            EstadoCivil.cIdEstadoCivil = EstCiv.cIdEstadoCivil
            EstadoCivil.vDescripcionEstadoCivil = EstCiv.vDescripcionEstadoCivil
            Coleccion.Add(EstadoCivil)
        Next
        Return Coleccion
    End Function

    Public Function EstadoCivilListarPorId(ByVal IdEstadoCivil As String) As GNRL_ESTADOCIVIL
        Dim Consulta = Data.PA_GNRL_MNT_ESTADOCIVIL("SQL_NONE", "SELECT * FROM GNRL_ESTADOCIVIL " &
                                                                "WHERE cIdEstadoCivil = '" & IdEstadoCivil & "'", "", "", "", "0", "")
        Dim Coleccion As New GNRL_ESTADOCIVIL
        For Each GNRL_ESTADOCIVIL In Consulta
            Coleccion.cIdEstadoCivil = GNRL_ESTADOCIVIL.cIdEstadoCivil
            Coleccion.vDescripcionEstadoCivil = GNRL_ESTADOCIVIL.vDescripcionEstadoCivil
            Coleccion.vDescripcionAbreviadaEstadoCivil = GNRL_ESTADOCIVIL.vDescripcionAbreviadaEstadoCivil
            Coleccion.bEstadoRegistroEstadoCivil = GNRL_ESTADOCIVIL.bEstadoRegistroEstadoCivil
        Next
        Return Coleccion
    End Function

    Public Function EstadoCivilListaGrid(ByVal Filtro As String, ByVal Buscar As String) As List(Of VI_GNRL_ESTADOCIVIL)
        Dim Consulta = Data.PA_GNRL_MNT_ESTADOCIVIL("SQL_NONE", "SELECT cIdEstadoCivil, vDescripcionEstadoCivil, vDescripcionAbreviadaEstadoCivil, bEstadoRegistroEstadoCivil " &
                                                                "FROM GNRL_ESTADOCIVIL", "", "", "", "0", "")

        Dim Coleccion As New List(Of VI_GNRL_ESTADOCIVIL)
        For Each Busqueda In Consulta
            Dim BuscarEstCiv As New VI_GNRL_ESTADOCIVIL
            BuscarEstCiv.Codigo = Busqueda.cIdEstadoCivil
            BuscarEstCiv.Descripcion = Busqueda.vDescripcionEstadoCivil
            BuscarEstCiv.Estado = Busqueda.bEstadoRegistroEstadoCivil
            Coleccion.Add(BuscarEstCiv)
        Next
        Return Coleccion
    End Function

    Public Function EstadoCivilInserta(ByVal EstadoCivil As GNRL_ESTADOCIVIL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_ESTADOCIVIL("SQL_INSERT", "", EstadoCivil.cIdEstadoCivil, EstadoCivil.vDescripcionEstadoCivil, EstadoCivil.vDescripcionAbreviadaEstadoCivil,
                                                           EstadoCivil.bEstadoRegistroEstadoCivil, EstadoCivil.cIdEstadoCivil).ReturnValue.ToString
        Return x
    End Function

    Public Function EstadoCivilEdita(ByVal EstadoCivil As GNRL_ESTADOCIVIL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_ESTADOCIVIL("SQL_UPDATE", "", EstadoCivil.cIdEstadoCivil, EstadoCivil.vDescripcionEstadoCivil, EstadoCivil.vDescripcionAbreviadaEstadoCivil,
                                                           EstadoCivil.bEstadoRegistroEstadoCivil, EstadoCivil.cIdEstadoCivil).ReturnValue.ToString
        Return x
    End Function

    Public Function EstadoCivilElimina(ByVal EstadoCivil As GNRL_ESTADOCIVIL) As Int32
        Dim x
        x = Data.PA_GNRL_MNT_ESTADOCIVIL("SQL_NONE", "UPDATE GNRL_ESTADOCIVIL SET bEstadoRegistroEstadoCivil = 0 WHERE cIdEstadoCivil = '" & EstadoCivil.cIdEstadoCivil & "'",
                                         "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function EstadoCivilExiste(ByVal IdEstadoCivil As String) As Boolean
        If Data.PA_GNRL_MNT_ESTADOCIVIL("SQL_NONE", "SELECT * FROM GNRL_ESTADOCIVIL " &
                                                    "WHERE cIdEstadoCivil = '" & IdEstadoCivil & "'", "", "", "", "0", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
