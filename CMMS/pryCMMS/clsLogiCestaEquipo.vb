Imports System.Data.SqlClient

Public Class clsLogiCestaEquipo
    Public Shared Function CrearCesta() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) '1 Ok
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdTipoActivo", GetType(System.String))) '3 Ok
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdSistemaFuncional", GetType(System.String))) '5 Ok
        dt.Columns.Add(New DataColumn("IdEnlaceCatalogo", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '7 Ok
        dt.Columns.Add(New DataColumn("DescripcionAbreviada", GetType(System.String))) '8 Ok
        dt.Columns.Add(New DataColumn("FechaTransaccion", GetType(System.DateTime))) '9 Ok
        dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '10
        dt.Columns.Add(New DataColumn("IdEnlaceEquipo", GetType(System.String))) '10
        dt.Columns.Add(New DataColumn("Observacion", GetType(System.String))) '10 Ok
        'dt.Columns.Add(New DataColumn("CostoInicial", GetType(System.Decimal))) '10 Ok
        'dt.Columns.Add(New DataColumn("IdCuentaContable", GetType(System.String))) '10
        'dt.Columns.Add(New DataColumn("EstadoActivacion", GetType(System.Boolean))) '10
        'dt.Columns.Add(New DataColumn("FechaActivacion", GetType(System.DateTime))) '10
        'dt.Columns.Add(New DataColumn("EstadoLeasing", GetType(System.Boolean))) '10 Ok
        'dt.Columns.Add(New DataColumn("Marca", GetType(System.String))) '10 Ok
        'dt.Columns.Add(New DataColumn("Caracteristicas", GetType(System.String))) '10
        dt.Columns.Add(New DataColumn("VidaUtil", GetType(System.Int32))) '10 Ok
        dt.Columns.Add(New DataColumn("DescAbrevTipoActivo", GetType(System.String))) '15
        dt.Columns.Add(New DataColumn("DescAbrevSistemaFuncional", GetType(System.String))) '16
        dt.Columns.Add(New DataColumn("PeriodoGarantia", GetType(System.Int32))) '17
        dt.Columns.Add(New DataColumn("PeriodoMinimo", GetType(System.Int32))) '18

        'dt.Columns.Add(New DataColumn("Dimensiones", GetType(System.String))) '10 Ok
        'dt.Columns.Add(New DataColumn("Voltaje", GetType(System.String))) '10 Ok
        'dt.Columns.Add(New DataColumn("Peso", GetType(System.String))) '10 Ok

        'dt.Columns.Add(New DataColumn("IdEquivalencia", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("IdMetodoDepreciacion", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("IdFamilia", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("IdEstadoActivoFijo", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("FechaFinalizacion", GetType(System.DateTime)))
        'dt.Columns.Add(New DataColumn("IdTipoMoneda", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("SaldoInicial", GetType(System.Decimal)))
        'dt.Columns.Add(New DataColumn("IdEstadoOperacion", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("IdEstadoActivo", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("Modelo", GetType(System.String)))
        dt.Columns.Add(New DataColumn("NroSerie", GetType(System.String)))
        dt.Columns.Add(New DataColumn("NroParte", GetType(System.String)))
        dt.Columns.Add(New DataColumn("IdEstadoActivo", GetType(System.String))) '15

        'dt.Columns.Add(New DataColumn("Placa", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("NroUsuario", GetType(System.Int32)))
        'dt.Columns.Add(New DataColumn("NroLectora", GetType(System.Int32)))
        'dt.Columns.Add(New DataColumn("NroContador", GetType(System.Int32)))
        'dt.Columns.Add(New DataColumn("NroContrato", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("FechaContrato", GetType(System.DateTime)))
        'dt.Columns.Add(New DataColumn("NroCuotas", GetType(System.Int32)))
        'dt.Columns.Add(New DataColumn("IdAlmacen", GetType(System.String)))
        'dt.Columns.Add(New DataColumn("IdTipoAlmacen", GetType(System.String)))

        Return dt
    End Function

    'Public Shared Sub EditarCesta(ByVal IdMaestroActivo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String,
    '                              ByVal IdJerarquia As String, ByVal IdSistemaFuncional As String, ByVal IdEnlace As String,
    '                              ByVal Descripcion As String, ByVal DescripcionAbreviada As String, ByVal FechaAdquisicion As DateTime,
    '                              ByVal Estado As Boolean, ByVal IdEnlace2 As String, ByVal Observacion As String,
    '                              ByVal CostoInicial As Decimal, ByVal IdCuentaContable As String,
    '                              ByVal EstadoActivacion As Boolean, ByVal FechaActivacion As DateTime,
    '                              ByVal EstadoLeasing As Boolean, ByVal Marca As String, ByVal Caracteristicas As String, ByVal VidaUtil As Decimal,
    '                              ByVal Dimensiones As String, ByVal Voltaje As String, ByVal Peso As String,
    '                              ByVal IdEquivalencia As String, ByVal IdMetodoDepreciacion As String, ByVal IdFamilia As String,
    '                              ByVal IdEstadoActivoFijo As String, ByVal FechaFinalizacion As DateTime, ByVal IdTipoMoneda As String,
    '                              ByVal SaldoInicial As Decimal, ByVal IdEstadoOperacion As String, ByVal IdEstadoActivo As String,
    '                              ByVal Modelo As String, ByVal NroSerie As String, ByVal Placa As String, ByVal NroUsuario As Int32,
    '                              ByVal NroLectora As Int32, ByVal NroContador As Int32, ByVal NroContrato As String, ByVal FechaContrato As DateTime,
    '                              ByVal NroCuotas As Int32, ByVal IdAlmacen As String, ByVal IdTipoAlmacen As String,
    '                              ByVal Tabla As DataTable, ByVal Fila As Integer)
    Public Shared Sub EditarCesta(ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String,
                              ByVal IdJerarquia As String, ByVal IdSistemaFuncional As String, ByVal IdEnlaceCatalogo As String,
                              ByVal Descripcion As String, ByVal DescripcionAbreviada As String, ByVal FechaTransaccion As DateTime,
                              ByVal Estado As Boolean, ByVal IdEnlaceEquipo As String, ByVal Observacion As String,
                              ByVal VidaUtil As Int32, ByVal DescAbrevTipoActivo As String, ByVal DescAbrevSistemaFuncional As String,
                              ByVal PeriodoGarantia As Int32, ByVal PeriodoMinimoMantenimiento As Int32,
                              ByVal NroSerie As String, ByVal NroParte As String, ByVal IdEstadoActivo As String,
                              ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try



            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)("IdEquipo") = IdEquipo
                Tabla.Rows(Fila)("IdCatalogo") = IdCatalogo
                Tabla.Rows(Fila)("IdTipoActivo") = IdTipoActivo
                Tabla.Rows(Fila)("IdJerarquia") = IdJerarquia
                Tabla.Rows(Fila)("IdSistemaFuncional") = IdSistemaFuncional
                Tabla.Rows(Fila)("IdEnlaceCatalogo") = IdEnlaceCatalogo
                Tabla.Rows(Fila)("Descripcion") = Descripcion
                Tabla.Rows(Fila)("DescripcionAbreviada") = DescripcionAbreviada
                Tabla.Rows(Fila)("FechaTransaccion") = FechaTransaccion  'FechaAdquisicion
                Tabla.Rows(Fila)("Estado") = Estado
                Tabla.Rows(Fila)("IdEnlaceEquipo") = IdEnlaceEquipo
                Tabla.Rows(Fila)("Observacion") = Observacion
                Tabla.Rows(Fila)("VidaUtil") = VidaUtil
                Tabla.Rows(Fila)("DescAbrevTipoActivo") = DescAbrevTipoActivo
                Tabla.Rows(Fila)("DescAbrevSistemaFuncional") = DescAbrevSistemaFuncional
                Tabla.Rows(Fila)("PeriodoGarantia") = PeriodoGarantia
                Tabla.Rows(Fila)("PeriodoMinimo") = PeriodoMinimoMantenimiento
                Tabla.Rows(Fila)("NroSerie") = NroSerie
                Tabla.Rows(Fila)("NroParte") = NroParte
                Tabla.Rows(Fila)("IdEstadoActivo") = IdEstadoActivo
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    'Public Shared Sub AgregarCesta(ByVal IdMaestroActivo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String,
    '                              ByVal IdJerarquia As String, ByVal IdSistemaFuncional As String, ByVal IdEnlace As String,
    '                              ByVal Descripcion As String, ByVal DescripcionAbreviada As String, ByVal FechaAdquisicion As DateTime,
    '                              ByVal Estado As Boolean, ByVal IdEnlace2 As String, ByVal Observacion As String,
    '                              ByVal CostoInicial As Decimal, ByVal IdCuentaContable As String,
    '                              ByVal EstadoActivacion As Boolean, ByVal FechaActivacion As DateTime,
    '                              ByVal EstadoLeasing As Boolean, ByVal Marca As String, ByVal Caracteristicas As String, ByVal VidaUtil As Decimal,
    '                              ByVal Dimensiones As String, ByVal Voltaje As String, ByVal Peso As String,
    '                              ByVal IdEquivalencia As String, ByVal IdMetodoDepreciacion As String, ByVal IdFamilia As String,
    '                              ByVal IdEstadoActivoFijo As String, ByVal FechaFinalizacion As DateTime, ByVal IdTipoMoneda As String,
    '                              ByVal SaldoInicial As Decimal, ByVal IdEstadoOperacion As String, ByVal IdEstadoActivo As String,
    '                              ByVal Modelo As String, ByVal NroSerie As String, ByVal Placa As String, ByVal NroUsuario As Int32,
    '                              ByVal NroLectora As Int32, ByVal NroContador As Int32, ByVal NroContrato As String, ByVal FechaContrato As DateTime,
    '                              ByVal NroCuotas As Int32, ByVal IdAlmacen As String, ByVal IdTipoAlmacen As String,
    '                              ByVal Tabla As DataTable, Optional ByVal NroPagina As Integer = 0)
    Public Shared Sub AgregarCesta(ByVal IdEquipo As String, ByVal IdCatalogo As String, ByVal IdTipoActivo As String,
                              ByVal IdJerarquia As String, ByVal IdSistemaFuncional As String, ByVal IdEnlaceCatalogo As String,
                              ByVal Descripcion As String, ByVal DescripcionAbreviada As String, ByVal FechaTransaccion As DateTime,
                              ByVal Estado As Boolean, ByVal IdEnlaceEquipo As String, ByVal Observacion As String,
                              ByVal VidaUtil As Int32, ByVal DescAbrevTipoActivo As String, ByVal DescAbrevSistemaFuncional As String,
                              ByVal PeriodoGarantia As Int32, ByVal PeriodoMinimoMantenimiento As Int32,
                              ByVal NroSerie As String, ByVal NroParte As String, ByVal IdEstadoActivo As String,
                              ByVal Tabla As DataTable, Optional ByVal NroPagina As Integer = 0)

        Dim Fila As DataRow = Tabla.NewRow
        'If NroPagina > 0 Then
        '    Fila("Linea") = Mid(Convert.ToString(100000 + ((10 * NroPagina) - 10 + Tabla.Rows.Count + 1)), 2, 5)
        '    'ElseIf dblLineaMaxima > 0 Then
        '    '    Fila("Linea") = Mid(Convert.ToString(100000 + (Tabla.Rows.Count + dblLineaMaxima + 1)), 2, 5)
        'Else
        '    If Tabla.Rows.Count = 0 Then
        '        Fila("Linea") = Mid(Convert.ToString(100000 + (Tabla.Rows.Count + 1)), 2, 5)
        '    Else
        '        Fila("Linea") = Mid(Convert.ToString(100000 + (Tabla.Rows(Tabla.Rows.Count - 1)("Linea") + 1)), 2, 5)
        '    End If
        'End If
        Fila("IdEquipo") = IdEquipo
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdTipoActivo") = IdTipoActivo
        Fila("IdJerarquia") = IdJerarquia
        Fila("IdSistemaFuncional") = IdSistemaFuncional
        Fila("IdEnlaceCatalogo") = IdEnlaceCatalogo
        Fila("Descripcion") = Descripcion
        Fila("DescripcionAbreviada") = DescripcionAbreviada
        Fila("Estado") = Estado
        Fila("IdEnlaceEquipo") = IdEnlaceEquipo
        Fila("FechaTransaccion") = FechaTransaccion
        Fila("Observacion") = Observacion
        'Fila("CostoInicial") = CostoInicial
        'Fila("IdCuentaContable") = IdCuentaContable
        'Fila("EstadoActivacion") = EstadoActivacion
        'Fila("FechaActivacion") = FechaActivacion 'Ok
        'Fila("EstadoLeasing") = EstadoLeasing
        'Fila("Marca") = Marca
        'Fila("Caracteristicas") = Caracteristicas
        Fila("VidaUtil") = VidaUtil
        Fila("DescAbrevTipoActivo") = DescAbrevTipoActivo
        Fila("DescAbrevSistemaFuncional") = DescAbrevSistemaFuncional
        Fila("PeriodoGarantia") = PeriodoGarantia
        Fila("PeriodoMinimo") = PeriodoMinimoMantenimiento
        Fila("NroSerie") = NroSerie
        Fila("NroParte") = NroParte
        Fila("IdEstadoActivo") = IdEstadoActivo
        'Fila("Dimensiones") = Dimensiones
        'Fila("Voltaje") = Voltaje
        'Fila("Peso") = Peso
        'Fila("IdEquivalencia") = IdEquivalencia
        'Fila("IdMetodoDepreciacion") = IdMetodoDepreciacion
        'Fila("IdFamilia") = IdFamilia
        'Fila("IdEstadoActivoFijo") = IdEstadoActivoFijo
        'Fila("FechaFinalizacion") = FechaFinalizacion
        'Fila("IdTipoMoneda") = IdTipoMoneda
        'Fila("SaldoInicial") = SaldoInicial
        'Fila("IdEstadoOperacion") = IdEstadoOperacion
        'Fila("IdEstadoActivo") = IdEstadoActivo
        'Fila("Modelo") = Modelo
        'Fila("NroSerie") = NroSerie
        'Fila("Placa") = Placa
        'Fila("NroUsuario") = NroUsuario
        'Fila("NroLectora") = NroLectora
        'Fila("NroContador") = NroContador
        'Fila("NroContrato") = NroContrato
        'Fila("FechaContrato") = FechaContrato
        'Fila("NroCuotas") = NroCuotas
        'Fila("IdAlmacen") = IdAlmacen
        'Fila("IdTipoAlmacen") = IdTipoAlmacen
        Tabla.Rows.Add(Fila)
    End Sub

    Public Shared Sub QuitarCesta(ByVal Fila As Integer, ByVal Tabla As DataTable, Optional ByVal NroPagina As Integer = 0)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows.RemoveAt(Fila)
                'Dim i As Integer
                'For i = 0 To Tabla.Rows.Count - 1
                '    Tabla.Rows(i).BeginEdit()

                '    If NroPagina > 0 Then
                '        'Fila("Linea") = Mid(Convert.ToString(100000 + ((10 * NroPagina) - 10 + Tabla.Rows.Count + 1)), 2, 5)
                '        Tabla.Rows(i)(0) = Mid(Convert.ToString(100000 + (i + (10 * NroPagina) - 10 + 1)), 2, 5)
                '    Else
                '        'Fila("Linea") = Mid(Convert.ToString(100000 + (Tabla.Rows.Count + 1)), 2, 5)
                '        Tabla.Rows(i)(0) = Mid(Convert.ToString(100000 + (i + 1)), 2, 5)
                '    End If

                '    Tabla.Rows(i).EndEdit()
                'Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Public Shared Sub VaciarCesta(ByVal Tabla As DataTable)
        Try
            Tabla.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub
End Class