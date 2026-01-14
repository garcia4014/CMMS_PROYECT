Imports CapaDatosCMMS

Public Class clsCheckListNegocios
    Dim CheckListMet As New clsCheckListMetodos

    Public Function CheckListGetData(strQuery As String) As DataTable
        Return CheckListMet.CheckListGetData(strQuery)
    End Function

    Public Function CheckListListarCombo() As List(Of LOGI_CHECKLISTORDENTRABAJO)
        Return CheckListMet.CheckListListarCombo()
    End Function

    Public Function CheckListListaGridPorOrdEquipo(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroCor As String,
                                                   ByVal IdEmp As String, ByVal IdEquipo As String, ByVal IdArticuloSAP As String,
                                                   ByVal IdCatalogoComponente As String) As List(Of VI_LOGI_CHECKLISTORDENTRABAJO)
        Return CheckListMet.CheckListListaGridPorOrdEquipo(IdTipDoc, IdNroSer, IdNroCor, IdEmp, IdEquipo, IdArticuloSAP, IdCatalogoComponente)
    End Function

    'Public Function CheckListEquipoInserta(ByVal CheckList As LOGI_CHECKLISTORDENTRABAJO) As Int32
    '    If CheckListMet.CheckListExiste(CheckList.cIdEquipo, CheckList.nIdNumeroItemCheckListEquipo) = True Then
    '        Throw New Exception("La imagen con el id " & CheckList.cIdEquipo & " ya existe!")
    '    Else
    '        Return CheckListMet.CheckListEquipoInserta(CheckList)
    '    End If
    'End Function

    'Public Function CheckListEdita(ByVal CheckListEquipo As LOGI_CheckListEquipo) As Int32
    '    If CheckListEquipoMet.CheckListEquipoExiste(CheckListEquipo.cIdEquipo, CheckListEquipo.nIdNumeroItemCheckListEquipo) = False Then
    '        Throw New Exception("La imagen con el id " & CheckListEquipo.cIdEquipo & " no existe!")
    '    Else
    '        Return CheckListEquipoMet.CheckListEquipoEdita(CheckListEquipo)
    '    End If
    'End Function

    'Public Function CheckListElimina(ByVal CheckListEquipo As LOGI_CheckListEquipo)
    '    If CheckListEquipoMet.CheckListExiste(CheckListEquipo.cIdEquipo, CheckListEquipo.nIdNumeroItemCheckListEquipo) = False Then
    '        Throw New Exception("La imagen con el id " & CheckListEquipo.cIdEquipo & " no existe!")
    '    Else
    '        Return CheckListEquipoMet.CheckListEquipoElimina(CheckListEquipo)
    '    End If
    'End Function

    'Public Function CheckListListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_CheckListEquipo)
    '    Return CheckListMet.CheckListEquipoListaGrid(Filtro, Buscar, Estado)
    'End Function

    'Public Function CheckListListarPorId(ByVal IdEquipo As String, ByVal IdNroItem As String) As LOGI_CHECKLISTORDENTRABAJO
    '    If CheckListMet.CheckListExiste(IdEquipo, IdNroItem) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("La imagen con el id " & IdEquipo & " no Existe!!!")
    '    Else
    '        Return CheckListMet.CheckListListarPorId(IdEquipo, IdNroItem)
    '    End If
    'End Function
End Class
