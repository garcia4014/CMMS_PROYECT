Imports System.Data.SqlClient

Public Class clsLogiCestaCatalogo
    Public Shared Function CrearCesta() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("IdTipoActivo", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("IdSistemaFuncional", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdEnlace", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("DescripcionAbreviada", GetType(System.String))) '7
        dt.Columns.Add(New DataColumn("Estado", GetType(System.Boolean))) '8
        'dt.Columns.Add(New DataColumn("Dimensiones", GetType(System.String))) '9
        'dt.Columns.Add(New DataColumn("Voltaje", GetType(System.String))) '10
        'dt.Columns.Add(New DataColumn("Peso", GetType(System.String))) '11
        'dt.Columns.Add(New DataColumn("VidaUtil", GetType(System.Decimal))) '12
        dt.Columns.Add(New DataColumn("VidaUtil", GetType(System.Int32))) '9
        dt.Columns.Add(New DataColumn("IdCuentaContable", GetType(System.String))) '10
        dt.Columns.Add(New DataColumn("IdCuentaContableLeasing", GetType(System.String))) '11
        dt.Columns.Add(New DataColumn("DescAbrevTipoActivo", GetType(System.String))) '12
        dt.Columns.Add(New DataColumn("DescAbrevSistemaFuncional", GetType(System.String))) '13
        dt.Columns.Add(New DataColumn("PeriodoGarantia", GetType(System.Int32))) '14
        dt.Columns.Add(New DataColumn("PeriodoMinimo", GetType(System.Int32))) '15
        Return dt
    End Function

    'Public Shared Sub EditarCesta(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal IdJerarquia As String,
    '                              ByVal IdSistemaFuncional As String, ByVal IdEnlace As String, ByVal Descripcion As String,
    '                              ByVal DescripcionAbreviada As String, ByVal Estado As Boolean, ByVal Dimensiones As String,
    '                              ByVal Voltaje As String, ByVal Peso As String, ByVal VidaUtil As Decimal, ByVal IdCuentaContable As String,
    '                              ByVal IdCuentaContableLeasing As String, ByVal DescAbrevTipoActivo As String, ByVal DescAbrevSistemaFuncional As String,
    '                              ByVal Tabla As DataTable, ByVal Fila As Integer)
    Public Shared Sub EditarCesta(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal IdJerarquia As String,
                              ByVal IdSistemaFuncional As String, ByVal IdEnlace As String, ByVal Descripcion As String,
                              ByVal DescripcionAbreviada As String, ByVal Estado As Boolean, ByVal VidaUtil As Decimal, ByVal IdCuentaContable As String,
                              ByVal IdCuentaContableLeasing As String, ByVal DescAbrevTipoActivo As String, ByVal DescAbrevSistemaFuncional As String,
                              ByVal PeriodoGarantia As Int32, ByVal PeriodoMinimoMantenimiento As Int32,
                              ByVal Tabla As DataTable, ByVal Fila As Integer)

        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                '                Tabla.Rows.RemoveAt(Fila)
                Tabla.Rows(Fila).BeginEdit()
                Tabla.Rows(Fila)("IdCatalogo") = IdCatalogo
                Tabla.Rows(Fila)("IdTipoActivo") = IdTipoActivo
                Tabla.Rows(Fila)("IdJerarquia") = IdJerarquia
                Tabla.Rows(Fila)("IdSistemaFuncional") = IdSistemaFuncional
                Tabla.Rows(Fila)("IdEnlace") = IdEnlace
                Tabla.Rows(Fila)("Descripcion") = Descripcion
                Tabla.Rows(Fila)("DescripcionAbreviada") = DescripcionAbreviada
                Tabla.Rows(Fila)("Estado") = Estado
                Tabla.Rows(Fila)("VidaUtil") = VidaUtil
                Tabla.Rows(Fila)("IdCuentaContable") = IdCuentaContable
                Tabla.Rows(Fila)("IdCuentaContableLeasing") = IdCuentaContableLeasing
                Tabla.Rows(Fila)("DescAbrevTipoActivo") = DescAbrevTipoActivo
                Tabla.Rows(Fila)("DescAbrevSistemaFuncional") = DescAbrevSistemaFuncional
                Tabla.Rows(Fila)("PeriodoGarantia") = PeriodoGarantia
                Tabla.Rows(Fila)("PeriodoMinimo") = PeriodoMinimoMantenimiento
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    'Public Shared Sub AgregarCesta(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal IdJerarquia As String,
    '                               ByVal IdSistemaFuncional As String, ByVal IdEnlace As String, ByVal Descripcion As String,
    '                               ByVal DescripcionAbreviada As String, ByVal Estado As Boolean, ByVal Dimensiones As String,
    '                               ByVal Voltaje As String, ByVal Peso As String, ByVal VidaUtil As Decimal, ByVal IdCuentaContable As String,
    '                               ByVal IdCuentaContableLeasing As String, ByVal DescAbrevTipoActivo As String, ByVal DescAbrevSistemaFuncional As String,
    '                               ByVal Tabla As DataTable, Optional ByVal NroPagina As Integer = 0)
    Public Shared Sub AgregarCesta(ByVal IdCatalogo As String, ByVal IdTipoActivo As String, ByVal IdJerarquia As String,
                               ByVal IdSistemaFuncional As String, ByVal IdEnlace As String, ByVal Descripcion As String,
                               ByVal DescripcionAbreviada As String, ByVal Estado As Boolean, ByVal VidaUtil As Decimal, ByVal IdCuentaContable As String,
                               ByVal IdCuentaContableLeasing As String, ByVal DescAbrevTipoActivo As String, ByVal DescAbrevSistemaFuncional As String,
                               ByVal PeriodoGarantia As Int32, ByVal PeriodoMinimoMantenimiento As Int32,
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
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdTipoActivo") = IdTipoActivo
        Fila("IdJerarquia") = IdJerarquia
        Fila("IdSistemaFuncional") = IdSistemaFuncional
        Fila("IdEnlace") = IdEnlace
        Fila("Descripcion") = Descripcion
        Fila("DescripcionAbreviada") = DescripcionAbreviada
        Fila("Estado") = Estado
        'Fila("Dimensiones") = Dimensiones
        'Fila("Voltaje") = Voltaje
        'Fila("Peso") = Peso
        Fila("VidaUtil") = VidaUtil
        Fila("IdCuentaContable") = IdCuentaContable
        Fila("IdCuentaContableLeasing") = IdCuentaContableLeasing
        Fila("DescAbrevTipoActivo") = DescAbrevTipoActivo
        Fila("DescAbrevSistemaFuncional") = DescAbrevSistemaFuncional
        Fila("PeriodoGarantia") = PeriodoGarantia
        Fila("PeriodoMinimo") = PeriodoMinimoMantenimiento

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