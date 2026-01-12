Imports CapaDatosCMMS

Public Class clsComponenteOrdenTrabajoNegocios
    Dim ComponenteOrdenTrabajoMet As New clsComponenteOrdenTrabajoMetodos

    Public Function ComponenteOrdenTrabajoGetData(strQuery As String) As DataTable
        Return ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoGetData(strQuery)
    End Function

    Public Function ComponenteOrdenTrabajoListarCombo() As List(Of LOGI_COMPONENTEORDENTRABAJO)
        Return ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoListarCombo()
    End Function

    Public Function ComponenteOrdenTrabajoListaGridPorOrdEquipo(ByVal IdTipDoc As String, ByVal IdNroSer As String, ByVal IdNroCor As String,
                                            ByVal IdEmp As String, ByVal IdEquipo As String, ByVal IdArticuloSAP As String) As List(Of VI_LOGI_COMPONENTEORDENTRABAJO)
        Return ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoListaGridPorOrdEquipo(IdTipDoc, IdNroSer, IdNroCor, IdEmp, IdEquipo, IdArticuloSAP)
    End Function

    'Public Function ComponenteOrdenTrabajoEquipoInserta(ByVal ComponenteOrdenTrabajo As LOGI_ComponenteOrdenTrabajoORDENTRABAJO) As Int32
    '    If ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoExiste(ComponenteOrdenTrabajo.cIdEquipo, ComponenteOrdenTrabajo.nIdNumeroItemComponenteOrdenTrabajoEquipo) = True Then
    '        Throw New Exception("La imagen con el id " & ComponenteOrdenTrabajo.cIdEquipo & " ya existe!")
    '    Else
    '        Return ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoEquipoInserta(ComponenteOrdenTrabajo)
    '    End If
    'End Function

    'Public Function ComponenteOrdenTrabajoEdita(ByVal ComponenteOrdenTrabajoEquipo As LOGI_ComponenteOrdenTrabajoEquipo) As Int32
    '    If ComponenteOrdenTrabajoEquipoMet.ComponenteOrdenTrabajoEquipoExiste(ComponenteOrdenTrabajoEquipo.cIdEquipo, ComponenteOrdenTrabajoEquipo.nIdNumeroItemComponenteOrdenTrabajoEquipo) = False Then
    '        Throw New Exception("La imagen con el id " & ComponenteOrdenTrabajoEquipo.cIdEquipo & " no existe!")
    '    Else
    '        Return ComponenteOrdenTrabajoEquipoMet.ComponenteOrdenTrabajoEquipoEdita(ComponenteOrdenTrabajoEquipo)
    '    End If
    'End Function

    'Public Function ComponenteOrdenTrabajoElimina(ByVal ComponenteOrdenTrabajoEquipo As LOGI_ComponenteOrdenTrabajoEquipo)
    '    If ComponenteOrdenTrabajoEquipoMet.ComponenteOrdenTrabajoExiste(ComponenteOrdenTrabajoEquipo.cIdEquipo, ComponenteOrdenTrabajoEquipo.nIdNumeroItemComponenteOrdenTrabajoEquipo) = False Then
    '        Throw New Exception("La imagen con el id " & ComponenteOrdenTrabajoEquipo.cIdEquipo & " no existe!")
    '    Else
    '        Return ComponenteOrdenTrabajoEquipoMet.ComponenteOrdenTrabajoEquipoElimina(ComponenteOrdenTrabajoEquipo)
    '    End If
    'End Function

    'Public Function ComponenteOrdenTrabajoListaBusqueda(ByVal Filtro As String, ByVal Buscar As String, ByVal Estado As String) As List(Of VI_LOGI_ComponenteOrdenTrabajoEquipo)
    '    Return ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoEquipoListaGrid(Filtro, Buscar, Estado)
    'End Function

    'Public Function ComponenteOrdenTrabajoListarPorId(ByVal IdEquipo As String, ByVal IdNroItem As String) As LOGI_ComponenteOrdenTrabajoORDENTRABAJO
    '    If ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoExiste(IdEquipo, IdNroItem) = False Then
    '        'Si el producto no existe lanzo una excepción.
    '        Throw New Exception("La imagen con el id " & IdEquipo & " no Existe!!!")
    '    Else
    '        Return ComponenteOrdenTrabajoMet.ComponenteOrdenTrabajoListarPorId(IdEquipo, IdNroItem)
    '    End If
    'End Function
End Class
