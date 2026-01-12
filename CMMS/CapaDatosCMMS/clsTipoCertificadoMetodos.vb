Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Reflection

Public Class clsTipoCertificadoMetodos
    Dim Data As New BDCMMS_MovitecnicaDataContext

    Public Function TipoCertificadoGetData(strQuery As String) As DataTable
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

    Public Function TipoCertificadoListarCombo() As List(Of RRHH_TIPOCERTIFICADO)
        Dim Consulta = Data.PA_RRHH_MNT_TIPOCERTIFICADO("SQL_NONE", "SELECT * FROM RRHH_TIPOCERTIFICADO",
                                                      "", "", "", "1", "")
        Dim Coleccion As New List(Of RRHH_TIPOCERTIFICADO)
        For Each TipoCertificado In Consulta
            Dim TipAct As New RRHH_TIPOCERTIFICADO
            TipAct.cIdTipoCertificado = TipoCertificado.cIdTipoCertificado
            TipAct.vDescripcionTipoCertificado = TipoCertificado.vDescripcionTipoCertificado
            Coleccion.Add(TipAct)
        Next
        Return Coleccion
    End Function

    Public Function TipoCertificadoListarPorId(ByVal IdTipoCertificado As String) As RRHH_TIPOCERTIFICADO
        Dim Consulta = Data.PA_RRHH_MNT_TIPOCERTIFICADO("SQL_NONE", "SELECT * FROM RRHH_TIPOCERTIFICADO WHERE cIdTipoCertificado = '" & IdTipoCertificado & "' " &
                                                   " AND bEstadoRegistroTipoCertificado = 1 ", "", "", "", "1", "")
        Dim Coleccion As New RRHH_TIPOCERTIFICADO
        For Each RRHH_TIPOCERTIFICADO In Consulta
            Coleccion.cIdTipoCertificado = RRHH_TIPOCERTIFICADO.cIdTipoCertificado
            Coleccion.vDescripcionTipoCertificado = RRHH_TIPOCERTIFICADO.vDescripcionTipoCertificado
            Coleccion.vDescripcionAbreviadaTipoCertificado = RRHH_TIPOCERTIFICADO.vDescripcionAbreviadaTipoCertificado
            Coleccion.bEstadoRegistroTipoCertificado = RRHH_TIPOCERTIFICADO.bEstadoRegistroTipoCertificado
        Next
        Return Coleccion
    End Function

    Public Function TipoCertificadoListaGrid(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_RRHH_TIPOCERTIFICADO)
        'Este si puede devolver una colección de datos es decir varios registros
        Dim Consulta = Data.PA_RRHH_MNT_TIPOCERTIFICADO("SQL_NONE", "SELECT * FROM RRHH_TIPOCERTIFICADO " &
                                                   "WHERE " & Filtro & " LIKE UPPER ('%" & Buscar & "%') AND (bEstadoRegistroTipoCertificado = '" & Estado & "' OR '*' = '" & Estado & "')",
                                                   "", "", "", "1", "")
        Dim Coleccion As New List(Of VI_RRHH_TIPOCERTIFICADO)
        For Each Busqueda In Consulta
            Dim BuscarTipCer As New VI_RRHH_TIPOCERTIFICADO
            BuscarTipCer.Codigo = Busqueda.cIdTipoCertificado
            BuscarTipCer.Descripcion = Busqueda.vDescripcionTipoCertificado
            BuscarTipCer.Estado = Busqueda.bEstadoRegistroTipoCertificado
            Coleccion.Add(BuscarTipCer)
        Next
        Return Coleccion
    End Function

    Public Function TipoCertificadoInserta(ByVal TipoCertificado As RRHH_TIPOCERTIFICADO) As Int32
        Dim x
        x = Data.PA_RRHH_MNT_TIPOCERTIFICADO("SQL_INSERT", "", TipoCertificado.cIdTipoCertificado, TipoCertificado.vDescripcionTipoCertificado, TipoCertificado.vDescripcionAbreviadaTipoCertificado,
                                        TipoCertificado.bEstadoRegistroTipoCertificado, TipoCertificado.cIdTipoCertificado).ReturnValue.ToString
        Return x
    End Function

    Public Function DetalleAsignarCertificadoInsertaDetalle(ByVal AsignarCertificado As List(Of RRHH_ASIGNARCERTIFICADO), ByVal LogAuditoria As GNRL_LOGAUDITORIA) As Int32
        Dim x

        'Inicializo la Transacción
        Dim tOption As New TransactionOptions
        tOption.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        tOption.Timeout = TimeSpan.MaxValue

        Using scope As New TransactionScope(TransactionScopeOption.Required, tOption)
            x = Data.PA_RRHH_MNT_ASIGNARCERTIFICADO("SQL_NONE", "DELETE RRHH_ASIGNARCERTIFICADO " &
                                         "WHERE cIdPersonal = '" & AsignarCertificado(0).cIdPersonal & "' ",
                                         "", "", 0, Now, Now, "", "", "1", "", "").ReturnValue.ToString

            For Each Busqueda In AsignarCertificado
                'If bExiste = False Then
                x = Data.PA_RRHH_MNT_ASIGNARCERTIFICADO("SQL_INSERT", "", Busqueda.cIdPersonal, Busqueda.cIdTipoCertificado, Busqueda.nItemAsignarCertificado,
                                                        Busqueda.dFechaVigenciaInicioAsignarCertificado, Busqueda.dFechaVigenciaFinalAsignarCertificado, Busqueda.vURLAsignarCertificado,
                                                        Busqueda.vNumeroReferenciaAsignarCertificado, Busqueda.bEstadoRegistroAsignarCertificado, Busqueda.cEstadoRegistroAsignarCertificado, Busqueda.cIdPersonal).ReturnValue.ToString

                LogAuditoria.vEvento = "INSERTAR ASIGNAR CERTIFICADO PERSONAL"
                LogAuditoria.vQuery = "PA_RRHH_MNT_ASIGNARCERTIFICADO 'SQL_INSERT', '', '" & Busqueda.cIdPersonal & "', '" & Busqueda.cIdTipoCertificado & "', " & Busqueda.nItemAsignarCertificado & ", '" &
                                                        Busqueda.dFechaVigenciaInicioAsignarCertificado & "', '" & Busqueda.dFechaVigenciaFinalAsignarCertificado & "', '" & Busqueda.vURLAsignarCertificado & "', '" &
                                                        Busqueda.vNumeroReferenciaAsignarCertificado & "', '" & Busqueda.bEstadoRegistroAsignarCertificado & "', '" & Busqueda.cEstadoRegistroAsignarCertificado & "', '" & Busqueda.cIdPersonal & "'"

                x = Data.PA_GNRL_MNT_LOGAUDITORIA("SQL_INSERT", "", LogAuditoria.cIdUsuario, LogAuditoria.cIdPaisOrigen, LogAuditoria.cIdEmpresa, LogAuditoria.cIdLocal, LogAuditoria.cIdPuntoVenta, LogAuditoria.vSesion,
                                      LogAuditoria.vIP1, LogAuditoria.vIP2, LogAuditoria.vEvento, LogAuditoria.vPagina, Replace(LogAuditoria.vQuery, "'", "´"),
                                      LogAuditoria.cIdSistema, LogAuditoria.cIdModulo, LogAuditoria.vIP3Usuario).ReturnValue.ToString
            Next
            scope.Complete()
            Return x
        End Using
    End Function

    Public Function TipoCertificadoEdita(ByVal TipoCertificado As RRHH_TIPOCERTIFICADO) As Int32
        Dim x
        x = Data.PA_RRHH_MNT_TIPOCERTIFICADO("SQL_UPDATE", "", TipoCertificado.cIdTipoCertificado, TipoCertificado.vDescripcionTipoCertificado, TipoCertificado.vDescripcionAbreviadaTipoCertificado,
                                        TipoCertificado.bEstadoRegistroTipoCertificado, TipoCertificado.cIdTipoCertificado).ReturnValue.ToString
        Return x
    End Function

    Public Function TipoCertificadoElimina(ByVal TipoCertificado As RRHH_TIPOCERTIFICADO) As Int32
        Dim x
        x = Data.PA_RRHH_MNT_TIPOCERTIFICADO("SQL_NONE", "UPDATE RRHH_TIPOCERTIFICADO SET bEstadoRegistroTipoCertificado = 0 WHERE cIdTipoCertificado = '" & TipoCertificado.cIdTipoCertificado & "'",
                                           "", "", "", "1", "").ReturnValue.ToString
        Return x
    End Function

    Public Function TipoCertificadoExiste(ByVal IdTipoCertificado As String) As Boolean
        If Data.PA_RRHH_MNT_TIPOCERTIFICADO("SQL_NONE", "SELECT * FROM RRHH_TIPOCERTIFICADO WHERE cIdTipoCertificado = '" & IdTipoCertificado & "'",
                                          "", "", "", "1", "").Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
