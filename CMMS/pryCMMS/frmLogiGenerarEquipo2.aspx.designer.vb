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


Partial Public Class frmLogiGenerarEquipo2

    '''<summary>
    '''Control UpdatePanel1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UpdatePanel1 As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control ToolkitScriptManager1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager

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
    '''Control pnlCabecera.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlCabecera As Global.System.Web.UI.WebControls.Panel

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
    '''Control btnNuevo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnNuevo As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control btnEditar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnEditar As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control btnEliminar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnEliminar As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control grdLista.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdLista As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control pnlEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlEquipo As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control txtIdEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIdEquipo As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control btnAdicionarEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAdicionarEquipo As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control rfvIdEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvIdEquipo As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtDescripcionEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtDescripcionEquipo As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control rfvDescripcionEquipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvDescripcionEquipo As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control lblIdentificadorComponente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblIdentificadorComponente As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control fscIdentificadorComponente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscIdentificadorComponente As Global.System.Web.UI.HtmlControls.HtmlInputCheckBox

    '''<summary>
    '''Control cboTipoActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboTipoActivo As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control btnAdicionarTipoActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAdicionarTipoActivo As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control cboCatalogo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboCatalogo As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control btnAdicionarCatalogo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAdicionarCatalogo As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control rfvCatalogo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvCatalogo As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cboSistemaFuncional.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboSistemaFuncional As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control btnAdicionarSistemaFuncional.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAdicionarSistemaFuncional As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control rfvSistemaFuncional.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvSistemaFuncional As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cboCatalogoComponente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboCatalogoComponente As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control btnAdicionarCatalogoComponente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAdicionarCatalogoComponente As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control rfvCatalogoComponente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvCatalogoComponente As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtIdCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIdCliente As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control btnAdicionarCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAdicionarCliente As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control rfvIdCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvIdCliente As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control hfdIdTipoDocumentoCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdTipoDocumentoCliente As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Control hfdIdAuxiliar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdAuxiliar As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdTipoPersonaCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdTipoPersonaCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdTipoCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdTipoCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdUbicacionGeograficaCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdUbicacionGeograficaCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdNroDocumentoCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdNroDocumentoCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdDireccionFiscalCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdDireccionFiscalCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdTelefonoContactoCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdTelefonoContactoCliente As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control txtRazonSocial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtRazonSocial As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control rfvRazonSocial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvRazonSocial As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtFechaEntrega.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaEntrega As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control RequiredFieldValidator1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator1 As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control pnlComponentes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlComponentes As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control btnAdicionarComponente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAdicionarComponente As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control UpdPnlComponentes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UpdPnlComponentes As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control pnlSeleccionarElemento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlSeleccionarElemento As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control imgbtnEditarComponenteCatalogo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgbtnEditarComponenteCatalogo As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control grdComponenteCatalogo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdComponenteCatalogo As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control imgbtnEliminarDetalleMaestroActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgbtnEliminarDetalleMaestroActivo As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control imgbtnEditarDetalleMaestroActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgbtnEditarDetalleMaestroActivo As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control hfdIdAlmacen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdAlmacen As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control hfdIdTipoAlmacen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfdIdTipoAlmacen As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control grdComponenteMaestroActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdComponenteMaestroActivo As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control UpdatePanel6.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UpdatePanel6 As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control pnlDetalle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlDetalle As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control grdDetalle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdDetalle As Global.System.Web.UI.WebControls.GridView
End Class
