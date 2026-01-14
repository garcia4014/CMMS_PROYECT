'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class frmLogiOrdenVentaAlmacenXOF

    '''<summary>
    '''Control ToolkitScriptManager1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager

    '''<summary>
    '''Control updpnlContent.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents updpnlContent As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control hfdOperacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdOperacion As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdFocusObjeto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdFocusObjeto As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdEstado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdEstado As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdOperacionDetalle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdOperacionDetalle As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdOperacionDetalleAdicional.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdOperacionDetalleAdicional As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdCorreoElectronicoCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdCorreoElectronicoCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdDNICliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdDNICliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdRUCCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdRUCCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdClienteSAPEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdClienteSAPEquipo As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdEquipoSAPEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdEquipoSAPEquipo As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdArticuloSAPPrincipal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdArticuloSAPPrincipal As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdFechaCreacionEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdFechaCreacionEquipo As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdUsuarioCreacionEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdUsuarioCreacionEquipo As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control pnlCabecera.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlCabecera As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control btnGenerar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnGenerar As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control btnAsignarAsesor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAsignarAsesor As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control cboFiltroEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboFiltroEquipo As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control txtBuscarEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtBuscarEquipo As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control imgbtnBuscarEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgbtnBuscarEquipo As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control grdLista.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdLista As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control pnlControlDespacho.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlControlDespacho As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control btnAtras.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAtras As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control btnGuardar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnGuardar As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control lblNroOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNroOrden As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblNroOrdenVenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNroOrdenVenta As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDescripcionOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDescripcionOrden As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblRucCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRucCliente As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblRazonSocialCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRazonSocialCliente As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control updpnlDetalleControlDespacho.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents updpnlDetalleControlDespacho As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control grdDetalleControlDespacho.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdDetalleControlDespacho As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control ValidationSummary1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ValidationSummary1 As Global.System.Web.UI.WebControls.ValidationSummary

    '''<summary>
    '''Control updpnlMantenimientoAsignarAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents updpnlMantenimientoAsignarAsesorAuxiliar As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control lnk_mostrarPanelMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnk_mostrarPanelMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control lnk_mostrarPanelMantenimientoAsesorAuxiliar_ModalPopupExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnk_mostrarPanelMantenimientoAsesorAuxiliar_ModalPopupExtender As Global.AjaxControlToolkit.ModalPopupExtender

    '''<summary>
    '''Control pnlMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control imgbtnCancelarAsesorAuxiliarImagen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgbtnCancelarAsesorAuxiliarImagen As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control lblDescripcionMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDescripcionMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control rfvTxtFechaInicioPlanificadaMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvTxtFechaInicioPlanificadaMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control cboHorasMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboHorasMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cboMinutosMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboMinutosMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cboSegundosMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboSegundosMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cboMeridianoMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboMeridianoMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cboAuxiliarMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboAuxiliarMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control rfvAuxiliarMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvAuxiliarMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cboUnidadTrabajoMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboUnidadTrabajoMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control rfvUnidadTrabajoMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvUnidadTrabajoMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtOrdenVentaMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtOrdenVentaMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control txtOrdenCompraMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtOrdenCompraMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control txtOrdenFabricacionMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtOrdenFabricacionMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control ValidationSummary2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ValidationSummary2 As Global.System.Web.UI.WebControls.ValidationSummary

    '''<summary>
    '''Control btnAceptarMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAceptarMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control btnCancelarMantenimientoAsesorAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancelarMantenimientoAsesorAuxiliar As Global.System.Web.UI.WebControls.Button
End Class
