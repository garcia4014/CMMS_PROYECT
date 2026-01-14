Imports CapaDatosCMMS

Public Class clsActividadCheckListNegocios
    Dim ActividadCheckListMet As New clsActividadChecklistMetodos

    Public Function ActividadCheckListGetData(strQuery As String) As DataTable
        Return ActividadCheckListMet.ActividadChecklistGetData(strQuery)
    End Function

    Public Function ActividadCheckListListarCombo() As List(Of LOGI_ACTIVIDADCHECKLIST)
        Return ActividadCheckListMet.ActividadChecklistListarCombo()
    End Function

    Public Function ActividadCheckListInserta(ByVal ActividadCheckList As LOGI_ACTIVIDADCHECKLIST) As Int32
        If ActividadCheckListMet.ActividadChecklistExiste(ActividadCheckList.cIdActividadCheckList) = True Then
            Throw New Exception("La Actividad con el id " & ActividadCheckList.cIdActividadCheckList & " ya existe!")
        Else
            Return ActividadCheckListMet.ActividadChecklistInserta(ActividadCheckList)
        End If
    End Function

    Public Function ActividadCheckListEdita(ByVal ActividadCheckList As LOGI_ACTIVIDADCHECKLIST) As Int32
        If ActividadCheckListMet.ActividadChecklistExiste(ActividadCheckList.cIdActividadCheckList) = False Then
            Throw New Exception("La Actividad con el id " & ActividadCheckList.cIdActividadCheckList & " no existe!")
        Else
            Return ActividadCheckListMet.ActividadChecklistEdita(ActividadCheckList)
        End If
    End Function

    Public Function ActividadCheckListElimina(ByVal ActividadCheckList As LOGI_ACTIVIDADCHECKLIST)
        If ActividadCheckListMet.ActividadChecklistExiste(ActividadCheckList.cIdActividadCheckList) = False Then
            Throw New Exception("La Actividad con el id " & ActividadCheckList.cIdActividadCheckList & " no existe!")
        Else
            Return ActividadCheckListMet.ActividadChecklistElimina(ActividadCheckList)
        End If
    End Function

    Public Function ActividadCheckListListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal IdEmpresa As String, Optional ByVal Estado As String = "*") As List(Of VI_LOGI_ActividadCheckList)
        Return ActividadCheckListMet.ActividadChecklistListaGrid(Filtro, Buscar, Estado)
    End Function

    Public Function ActividadCheckListListarPorId(ByVal IdActividadCheckList As String) As LOGI_ACTIVIDADCHECKLIST
        If ActividadCheckListMet.ActividadChecklistExiste(IdActividadCheckList) = False Then
            'Si el producto no existe lanzo una excepción.
            Throw New Exception("La Actividad con el id " & IdActividadCheckList & " no Existe!!!")
        Else
            Return ActividadCheckListMet.ActividadChecklistListarPorId(IdActividadCheckList)
        End If
    End Function
End Class
