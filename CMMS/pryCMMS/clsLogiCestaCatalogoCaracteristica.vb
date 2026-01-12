Imports System.Data.SqlClient

Public Class clsLogiCestaCatalogoCaracteristica
    Public Shared Function CrearCesta() As DataTable
        'Código que se ejecuta cuando se inicia una nueva sesión
        'Aquí se crea una tabla temporal para guardar los
        'Datos del temporal
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Item", GetType(System.Int32))) '0
        dt.Columns.Add(New DataColumn("IdCatalogo", GetType(System.String))) '1
        dt.Columns.Add(New DataColumn("IdEquipo", GetType(System.String))) 'Recién lo agregué
        dt.Columns.Add(New DataColumn("IdJerarquia", GetType(System.String))) '2
        dt.Columns.Add(New DataColumn("IdCaracteristica", GetType(System.String))) '3
        dt.Columns.Add(New DataColumn("Descripcion", GetType(System.String))) '4
        dt.Columns.Add(New DataColumn("IdReferenciaSAP", GetType(System.String))) '5
        dt.Columns.Add(New DataColumn("DescripcionCampoSAP", GetType(System.String))) '6
        dt.Columns.Add(New DataColumn("Valor", GetType(System.String))) '7
        Return dt
    End Function

    Public Shared Sub EditarCesta(ByVal IdCatalogo As String, ByVal IdEquipo As String, ByVal IdJerarquia As String,
                                  ByVal IdCaracteristica As String, ByVal Descripcion As String,
                                  ByVal IdReferenciaSAP As String, ByVal DescripcionCampoSAP As String, ByVal Valor As String,
                                  ByVal Tabla As DataTable, ByVal Fila As Integer)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows(Fila).BeginEdit()
                'Tabla.Rows(Fila)(0) = IdCatalogo
                Tabla.Rows(Fila)("IdCatalogo") = IdCatalogo
                Tabla.Rows(Fila)("IdEquipo") = IdEquipo
                Tabla.Rows(Fila)("IdJerarquia") = IdJerarquia
                Tabla.Rows(Fila)("IdCaracteristica") = IdCaracteristica
                Tabla.Rows(Fila)("Descripcion") = Descripcion
                Tabla.Rows(Fila)("IdReferenciaSAP") = IdReferenciaSAP
                Tabla.Rows(Fila)("DescripcionCampoSAP") = DescripcionCampoSAP
                Tabla.Rows(Fila)("Valor") = Valor
                Tabla.Rows(Fila).EndEdit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Public Shared Sub AgregarCesta(ByVal IdCatalogo As String, ByVal IdEquipo As String, ByVal IdJerarquia As String,
                                   ByVal IdCaracteristica As String, ByVal Descripcion As String,
                                   ByVal IdReferenciaSAP As String, ByVal DescripcionCampoSAP As String, ByVal Valor As String,
                                   ByVal Tabla As DataTable)

        Dim Fila As DataRow = Tabla.NewRow
        Fila("Item") = Tabla.Rows.Count + 1
        Fila("IdCatalogo") = IdCatalogo
        Fila("IdEquipo") = IdEquipo
        Fila("IdJerarquia") = IdJerarquia
        Fila("IdCaracteristica") = IdCaracteristica
        Fila("Descripcion") = Descripcion
        Fila("IdReferenciaSAP") = IdReferenciaSAP
        Fila("DescripcionCampoSAP") = DescripcionCampoSAP
        Fila("Valor") = Valor

        Tabla.Rows.Add(Fila)
    End Sub

    Public Shared Sub QuitarCesta(ByVal Fila As Integer, ByVal Tabla As DataTable)
        Try
            If Fila > -1 And Tabla.Rows.Count > 0 Then
                Tabla.Rows.RemoveAt(Fila)
                Dim i As Integer
                For i = 0 To Tabla.Rows.Count - 1
                    Tabla.Rows(i).BeginEdit()
                    Tabla.Rows(i)(0) = i + 1
                    Tabla.Rows(i).EndEdit()
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'cn.Close()
        End Try
    End Sub

    Public Shared Sub QuitarCestaGrupo(ByVal IdCatalogoComponente As String, ByVal Id As String, ByVal Tabla As DataTable)
        Try
            If Tabla.Rows.Count > 0 Then
                For x = 0 To Tabla.Rows.Count - 1
                    If Tabla.Rows(x)("IdCatalogo") = IdCatalogoComponente And Tabla.Rows(x)("IdCaracteristica") = Id Then
                        Tabla.Rows.RemoveAt(x)
                        Dim i As Integer
                        For i = 0 To Tabla.Rows.Count - 1
                            Tabla.Rows(i).BeginEdit()
                            Tabla.Rows(i)(0) = i + 1
                            Tabla.Rows(i).EndEdit()
                        Next
                        Exit For
                    End If
                Next
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