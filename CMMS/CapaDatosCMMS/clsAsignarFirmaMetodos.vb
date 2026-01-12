Imports System.Data
Imports System.Data.SqlClient

Public Class clsAsignarFirmaMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    'Public Shared Function CrearCestaFirma() As DataTable
    '    'Código que se ejecuta cuando se inicia una nueva sesión
    '    'Aquí se crea una tabla temporal para guardar los
    '    'Datos del temporal
    '    Dim dt As New DataTable
    '    dt.Columns.Add(New DataColumn("IdPersonal", GetType(System.String))) '1
    '    dt.Columns.Add(New DataColumn("IdEmpresa", GetType(System.String))) '2
    '    dt.Columns.Add(New DataColumn("IdItem", GetType(System.Int32))) '3
    '    dt.Columns.Add(New DataColumn("FechaRegistro", GetType(System.DateTime))) '4
    '    dt.Columns.Add(New DataColumn("NombrePersona", GetType(System.String))) '5
    '    dt.Columns.Add(New DataColumn("URL", GetType(System.String))) '6
    '    dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '7
    '    Return dt
    'End Function

    Public Class FirmaCesta
        Public Property IdUsuario As String
        Public Property IdEmpresa As String
        Public Property IdItem As Integer
        Public Property FechaRegistro As DateTime
        Public Property NombreCompleto As String
        Public Property URL As String
        Public Property Estado As Boolean
    End Class

    Public Function AsignarFirmaGetData(strQuery As String) As DataTable
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

    'Public Function AsignarFirmaListarCombo(ByVal IdEmpresa As String, ByVal IdLocal As String) As List(Of RRHH_ASIGNARFIRMA)
    '    Dim Consulta = Data.PA_RRHH_MNT_ASIGNARFIRMA("SQL_NONE", "SELECT * FROM RRHH_ASIGNARFIRMA WHERE cIdEmpresa = '" & IdEmpresa & "' AND (cIdLocal = '" & IdLocal & "' OR '*' = '" & IdLocal & "') AND bEstadoRegistroAsignarFirma = 1", "''", "''", "", "", "", "1", "", "1", "", "")
    '    Dim Coleccion As New List(Of RRHH_ASIGNARFIRMA)
    '    For Each AsignarFirma In Consulta
    '        Dim PtoVta As New RRHH_ASIGNARFIRMA
    '        PtoVta.cIdAsignarFirma = AsignarFirma.cIdAsignarFirma
    '        PtoVta.vDescripcionAsignarFirma = AsignarFirma.vDescripcionAsignarFirma
    '        Coleccion.Add(PtoVta)
    '    Next
    '    Return Coleccion
    'End Function

    Public Function AsignarFirmaListarPorId(ByVal IdUsuario As String, ByVal IdItem As Integer, ByVal IdEmpresa As String) As RRHH_ASIGNARFIRMA
        Dim Consulta = Data.PA_RRHH_MNT_ASIGNARFIRMA("SQL_NONE", "SELECT * FROM RRHH_ASIGNARFIRMA WHERE cIdUsuario = '" & IdUsuario &
                                                     "' AND bEstadoRegistroAsignarFirma = 1 AND cIdEmpresa = '" & IdEmpresa & "' AND nItemAsignarFirma = " & IdItem,
                                                     "", "", 0, "", "1", 0)
        Dim Coleccion As New RRHH_ASIGNARFIRMA
        For Each RRHH_ASIGNARFIRMA In Consulta
            Coleccion.cIdUsuario = RRHH_ASIGNARFIRMA.cIdUsuario
            Coleccion.cIdEmpresa = RRHH_ASIGNARFIRMA.cIdEmpresa
            Coleccion.nItemAsignarFirma = RRHH_ASIGNARFIRMA.nItemAsignarFirma
            Coleccion.vURLAsignarFirma = RRHH_ASIGNARFIRMA.vURLAsignarFirma
            Coleccion.bEstadoRegistroAsignarFirma = RRHH_ASIGNARFIRMA.bEstadoRegistroAsignarFirma
            Coleccion.dFechaRegistroAsignarFirma = RRHH_ASIGNARFIRMA.dFechaRegistroAsignarFirma
        Next
        Return Coleccion
    End Function

    Public Function AsignarFirmaListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, ByVal Estado As String) As List(Of FirmaCesta)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_RRHH_MNT_ASIGNARFIRMA("SQL_NONE", "SELECT ASIFIR.cIdUsuario, ASIFIR.cIdEmpresa, USU.vApellidoPaternoUsuario + ' ' + USU.vApellidoMaternoUsuario + ', ' + USU.vNombresUsuario AS vNombreCompleto " &
                                                     "FROM RRHH_ASIGNARFIRMA AS ASIFIR INNER JOIN GNRL_USUARIO AS USU ON " &
                                                     "     ASIFIR.cIdEmpresa = USU.cIdEmpresa AND ASIFIR.cIdUsuario = USU.cIdUsuario " &
                                                     "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') " &
                                                     "      AND (ASIFIR.bEstadoRegistroAsignarFirma = '" & Estado & "' OR '*' = '" & Estado & "')" &
                                                     "      AND ASIFIR.cIdEmpresa = '" & IdEmpresa & "' " &
                                                     "GROUP BY ASIFIR.cIdUsuario, ASIFIR.cIdEmpresa, USU.vApellidoPaternoUsuario + ' ' + USU.vApellidoMaternoUsuario + ', ' + USU.vNombresUsuario " &
                                                     "ORDER BY USU.vApellidoPaternoUsuario + ' ' + USU.vApellidoMaternoUsuario + ', ' + USU.vNombresUsuario ",
                                                     "", "", 0, "", "1", 0)
        Dim Coleccion As New List(Of FirmaCesta)
        For Each Busqueda In Consulta
            Dim BuscarFirma As New FirmaCesta With {
                .IdUsuario = Busqueda.cIdUsuario,
                .IdEmpresa = Busqueda.cIdEmpresa,
                .NombreCompleto = Busqueda.vNombreCompleto ' <-- si necesitas aquí puedes hacer otro JOIN o consulta
            }
            '.IdItem = Busqueda.nItemAsignarFirma,
            '.FechaRegistro = Busqueda.dFechaRegistroAsignarFirma,
            '.URL = Busqueda.vURLAsignarFirma,
            '.Estado = Busqueda.bEstadoRegistroAsignarFirma

            Coleccion.Add(BuscarFirma)
        Next
        Return Coleccion
    End Function

    Public Function AsignarFirmaInserta(ByVal AsignarFirma As RRHH_ASIGNARFIRMA) As Int32
        Dim x
        x = Data.PA_RRHH_MNT_ASIGNARFIRMA("SQL_INSERT", "", AsignarFirma.cIdUsuario, AsignarFirma.cIdEmpresa, AsignarFirma.nItemAsignarFirma, AsignarFirma.vURLAsignarFirma,
                                          AsignarFirma.bEstadoRegistroAsignarFirma, AsignarFirma.nItemAsignarFirma).ReturnValue.ToString
        Return x
    End Function

    Public Function AsignarFirmaEdita(ByVal AsignarFirma As RRHH_ASIGNARFIRMA) As Int32
        Dim x
        x = Data.PA_RRHH_MNT_ASIGNARFIRMA("SQL_UPDATE", "", AsignarFirma.cIdUsuario, AsignarFirma.cIdEmpresa, AsignarFirma.nItemAsignarFirma, AsignarFirma.vURLAsignarFirma,
                                          AsignarFirma.bEstadoRegistroAsignarFirma, AsignarFirma.nItemAsignarFirma).ReturnValue.ToString
        Return x
    End Function

    Public Function AsignarFirmaElimina(ByVal AsignarFirma As RRHH_ASIGNARFIRMA) As Int32
        Dim x
        x = Data.PA_RRHH_MNT_ASIGNARFIRMA("SQL_NONE", "UPDATE RRHH_ASIGNARFIRMA SET bEstadoRegistroAsignarFirma = 0 
                                          WHERE cIdUsuario = '" & AsignarFirma.cIdUsuario & "' AND cIdEmpresa = '" & AsignarFirma.cIdEmpresa & "' AND nItemAsignarFirma = '" & AsignarFirma.nItemAsignarFirma & "'",
                                          "", "", 0, "", "1", 0).ReturnValue.ToString
        Return x
    End Function

    Public Function AsignarFirmaExiste(ByVal IdUsuario As String, ByVal IdItem As Integer, ByVal IdEmpresa As String) As Boolean
        If Data.PA_RRHH_MNT_ASIGNARFIRMA("SQL_NONE", "SELECT * FROM RRHH_ASIGNARFIRMA WHERE cIdUsuario = '" & IdUsuario &
                                       "' AND nItemAsignarFirma = '" & IdItem & "' AND bEstadoRegistroAsignarFirma = 1 AND cIdEmpresa = '" & IdEmpresa & "'",
                                       "", "", 0, "", "1", 0).Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class